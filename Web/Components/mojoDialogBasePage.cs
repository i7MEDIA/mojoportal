// Author:					
// Created:				    2009-04-08
// Last Modified:		    2017-03-17
// 
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using System;
using System.Web.UI;

namespace mojoPortal.Web
{
    public class mojoDialogBasePage : Page
    {

        private PageSettings currentPage = null;
        protected SiteSettings siteSettings = null;
        private string siteRoot = string.Empty;
        private ScriptLoader scriptLoader = null;

        public SiteSettings CurrentSite
        {
            get
            {
                EnsureSiteSettings();
                return siteSettings;
            }
        }

        public PageSettings CurrentPage
        {
            get
            {
                if (currentPage == null) currentPage = CacheHelper.GetCurrentPage();
                return currentPage;
            }
        }

        public string SiteRoot
        {
            get
            {
                if (siteRoot.Length == 0) { siteRoot = SiteUtils.GetNavigationSiteRoot(); }
                return siteRoot;
            }
        }

        protected void EnsureSiteSettings()
        {
            if (siteSettings == null) siteSettings = CacheHelper.GetCurrentSiteSettings();

        }

        public ScriptLoader ScriptConfig
        {
            get
            {
                if (scriptLoader == null)
                {
                    scriptLoader = Master.FindControl("ScriptInclude") as ScriptLoader;  
                }
                // older skins may not have the script loader so we can add it below in OnInit if scriptLoaderFoundInMaster is false
                if (scriptLoader == null) { scriptLoader = new ScriptLoader(); }

                return scriptLoader;
            }

        }

        public bool UserCanEditPage(int pageId)
        {
            if (pageId == -1) { return false; }
            if (CurrentPage == null) { return false; }
            if (CurrentPage.PageId != pageId) { return false; }

            if (WebUser.IsAdmin) { return true; }

            if (WebUser.IsContentAdmin)
            {
                if (CurrentPage.EditRoles == "Admins;") { return false; }
                return true;
            }

            if (SiteUtils.UserIsSiteEditor())
            {
                if (CurrentPage.EditRoles == "Admins;") { return false; }
                return true;
            }

            return WebUser.IsInRoles(CurrentPage.EditRoles);
        }


        /// <summary>
        /// Returns true if the module exists on the page and the user has permission to edit the page or the module.
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public bool UserCanEditModule(int moduleId)
        {
            return UserCanEditModule(moduleId, Guid.Empty);

        }

        /// <summary>
        /// this overload is preferred because it checks if the module represents an instance of the feature
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="featureGuid"></param>
        /// <returns></returns>
        public bool UserCanEditModule(int moduleId, Guid featureGuid)
        {
            if (!Request.IsAuthenticated) return false;

            if (WebUser.IsAdminOrContentAdmin) return true;

            if (SiteUtils.UserIsSiteEditor()) { return true; }

            if (CurrentPage == null) return false;

            bool moduleFoundOnPage = false;
            foreach (Module m in CurrentPage.Modules)
            {
                if (
                    (m.ModuleId == moduleId)
                    && ((featureGuid == Guid.Empty) || (m.FeatureGuid == featureGuid))
                    )
                {
                    moduleFoundOnPage = true;
                }
            }

            if (!moduleFoundOnPage) return false;

            if (WebUser.IsInRoles(CurrentPage.EditRoles)) return true;

            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser == null) return false;

            foreach (Module m in CurrentPage.Modules)
            {
                if (m.ModuleId == moduleId)
                {
                    if (m.EditUserId == currentUser.UserId) return true;
                    if (WebUser.IsInRoles(m.AuthorizedEditRoles)) return true;
                }
            }

            return false;

        }

        public Module GetModule(int moduleId)
        {
            return GetModule(moduleId, Guid.Empty);
        }

        public Module GetModule(int moduleId, Guid featureGuid)
        {
            if (CurrentPage == null) { return null; }

            foreach (Module m in CurrentPage.Modules)
            {
                if (
                    (m.ModuleId == moduleId)
                    && ((featureGuid == Guid.Empty) || (m.FeatureGuid == featureGuid))
                    )
                { return m; }
            }

            return null;
        }

        public bool UserCanOnlyEditModuleAsDraft(int moduleId)
        {
            return UserCanOnlyEditModuleAsDraft(moduleId, Guid.Empty);

        }

        public bool UserCanOnlyEditModuleAsDraft(int moduleId, Guid featureGuid)
        {
            if (!Request.IsAuthenticated) return false;

            if (WebUser.IsAdminOrContentAdmin) return false;

            if (SiteUtils.UserIsSiteEditor()) { return false; }

            if (!WebConfigSettings.EnableContentWorkflow) { return false; }
            if (CurrentSite == null) { return false; }
            if (!CurrentSite.EnableContentWorkflow) { return false; }

            if (CurrentPage == null) return false;

            bool moduleFoundOnPage = false;
            foreach (Module m in CurrentPage.Modules)
            {
                if (
                    (m.ModuleId == moduleId)
                    && ((featureGuid == Guid.Empty) || (m.FeatureGuid == featureGuid))
                    )
                {
                    moduleFoundOnPage = true;
                }
            }

            if (!moduleFoundOnPage) return false;

            if (WebUser.IsInRoles(CurrentPage.DraftEditOnlyRoles)) return true;

            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser == null) return false;

            foreach (Module m in CurrentPage.Modules)
            {
                if (m.ModuleId == moduleId)
                {
                    if (WebUser.IsInRoles(m.DraftEditRoles)) return true;
                }
            }

            return false;

        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            SetupTheme();

        }

        private void SetupTheme()
        {
            if (CurrentSite == null) { return; }

            bool allowSkinOverride = false;
            string skinName = SiteUtils.GetSkinName(allowSkinOverride, this);

            bool registeredVirtualThemes = Global.RegisteredVirtualThemes; //should always be true under .NET 4
            if ((!registeredVirtualThemes) && (!WebConfigSettings.UseSiteIdAppThemesInMediumTrust))
            {
                this.Theme = "default";
                return;
            }

            if ((!registeredVirtualThemes) && (WebConfigSettings.UseSiteIdAppThemesInMediumTrust))
            {
                this.Theme = "default" + CurrentSite.SiteId.ToInvariantString();
                return;
            }

            // default to site skin
            this.Theme = "default" + CurrentSite.SiteId.ToInvariantString() + skinName;  

        }

    }
}
