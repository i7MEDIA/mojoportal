// Author:					
// Created:				    2013-02-22
// Last Modified:		    2013-02-22
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using System;
using System.Globalization;
using System.Web.UI;

namespace mojoPortal.Web
{
    public class mojoEditServiceBasePage : Page
    {
        private PageSettings currentPage = null;
        private SiteSettings siteSettings = null;
        private string siteRoot = string.Empty;
        private PageViewMode viewMode = PageViewMode.WorkInProgress;

        public PageViewMode ViewMode
        {
            get { return viewMode; }
        }

        private bool userIsAdmin = false;

        public bool UserIsAdmin
        {
            get { return userIsAdmin; }
        }

        private bool userIsContentAdmin = false;

        public bool UserIsContentAdmin
        {
            get { return userIsContentAdmin; }
        }

        private bool userIsSiteEditor = false;

        public bool UserIsSiteEditor
        {
            get { return userIsSiteEditor; }
        }

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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            userIsAdmin = WebUser.IsAdmin;
            userIsContentAdmin = WebUser.IsContentAdmin;
            userIsSiteEditor = SiteUtils.UserIsSiteEditor();
            viewMode = GetUserViewMode();
        }

        protected PageViewMode GetUserViewMode()
        {
            PageViewMode viewMode = PageViewMode.WorkInProgress;

            // we let the query string value trump because we pass it in an url from the Admin workflow pages
            if (!string.IsNullOrEmpty(Request.QueryString["vm"]))
            {
                try
                {
                    viewMode = (PageViewMode)Enum.Parse(typeof(PageViewMode), Request.QueryString["vm"]);

                }
                catch (ArgumentException)
                { }

            }
            else
            {
                // if query string is not passed use the cookie
                string viewCookieValue = CookieHelper.GetCookieValue(GetViewModeCookieName());
                if (!string.IsNullOrEmpty(viewCookieValue))
                {
                    try
                    {
                        viewMode = (PageViewMode)Enum.Parse(typeof(PageViewMode), viewCookieValue);

                    }
                    catch (ArgumentException)
                    { }
                }

            }

            return viewMode;

        }

        private string GetViewModeCookieName()
        {
            string cookieName = "viewmode";
            if (CurrentSite != null) { cookieName += CurrentSite.SiteId.ToString(CultureInfo.InvariantCulture); }

            return cookieName;

        }

        protected void EnsureSiteSettings()
        {
            if (siteSettings == null) siteSettings = CacheHelper.GetCurrentSiteSettings();

        }

        public bool UserCanEditPage(int pageId)
        {
            if (pageId == -1) { return false; }
            if (CurrentPage == null) { return false; }
            if (CurrentPage.PageId != pageId) { return false; }

            if (userIsContentAdmin)
            {
                if (CurrentPage.EditRoles == "Admins;") { return false; }
                return true;
            }

            if (userIsSiteEditor)
            {
                if (CurrentPage.EditRoles == "Admins;") { return false; }
                return true;
            }

            return WebUser.IsInRoles(CurrentPage.EditRoles);
        }


        

        /// <summary>
        /// this overload is preferred because it checks if the module represents an instance of the feature
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="featureGuid"></param>
        /// <returns></returns>
        public bool UserCanEditModule(int moduleId, Guid featureGuid)
        {
            if (!Request.IsAuthenticated) { return false; }

            if (CurrentPage == null) { return false; }

            if ((!userIsAdmin)&&(CurrentPage.EditRoles == "Admins;")) { return false; }

            bool moduleFoundOnPage = false;
            string moduleEditRoles = string.Empty;

            foreach (Module m in CurrentPage.Modules)
            {
                if (
                    (m.ModuleId == moduleId)
                    && ((featureGuid == Guid.Empty) || (m.FeatureGuid == featureGuid))
                    )
                {
                    moduleFoundOnPage = true;
                    moduleEditRoles = m.AuthorizedEditRoles;
                }
            }

            if (!moduleFoundOnPage) return false;

            if ((!userIsAdmin) && (moduleEditRoles == "Admins;")) { return false; }

            if (userIsAdmin || userIsContentAdmin || userIsSiteEditor) return true;

            if (WebUser.IsInRoles(moduleEditRoles)) return true;

            if (WebUser.IsInRoles(CurrentPage.EditRoles)) return true;

            return false;

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

        
        public bool UserCanOnlyEditModuleAsDraft(int moduleId, Guid featureGuid)
        {
            if (!Request.IsAuthenticated) return false;

            if (userIsAdmin || userIsContentAdmin || userIsSiteEditor) { return false; }

            if (!WebConfigSettings.EnableContentWorkflow) { return false; }
            if (CurrentSite == null) { return false; }
            if (!CurrentSite.EnableContentWorkflow) { return false; }

            if (CurrentPage == null) { return false; }

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

    }
}