//	Author:				        Dean Brettle
//	Created:			        2005-09-07
//
//	Modified:                   2011-06-13 by 
//	

using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.ForumUI
{
	
    public partial class ForumModuleEditSubscriptions : NonCmsBasePage
	{
        protected ForumConfiguration config = null;
        private Module module = null;
        private Hashtable settings;
        private int moduleId = -1; 
        private int pageId = -1; 

        private void Page_Load(object sender, EventArgs e)
		{
            if ((siteSettings != null) && (CurrentPage != null))
            {
                if ((SiteUtils.SslIsAvailable())
                    && ((siteSettings.UseSslOnAllPages) || (CurrentPage.RequireSsl))
                    )
                {
                    SiteUtils.ForceSsl();
                }
                else
                {
                    SiteUtils.ClearSsl();
                }

            }

            SecurityHelper.DisableBrowserCache();

            LoadSettings();

            if (!UserCanViewPage(moduleId, Forum.FeatureGuid))
            {
                if (!Request.IsAuthenticated)
                {
                    SiteUtils.RedirectToLoginPage(this);
                }
                else
                {
                    SiteUtils.RedirectToAccessDeniedPage(this);
                }
                return;
            }
            //can't edit subscription without being signed in
            if (!Request.IsAuthenticated)
            {
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                return;
            }

			PopulateLabels();
			
            AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsForumSection", "forums");
		}

		
		private void btnCancel_Click(object sender, EventArgs e)
		{
           
            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}

		private void PopulateLabels()
		{
            Title = SiteUtils.FormatPageTitle(siteSettings, ForumResources.ForumSubscriptions);
            if (module != null)
            {
                heading.Text = module.ModuleTitle;
            }
            
        }

       
        private void LoadSettings()
        {
            
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            module = GetModule(moduleId, Forum.FeatureGuid);
            settings = ModuleSettings.GetModuleSettings(moduleId);
            config = new ForumConfiguration(settings);

            forumList.Config = config;
            forumList.ModuleId = moduleId;
            forumList.PageId = pageId;
            forumList.SiteRoot = SiteRoot;
            forumList.ImageSiteRoot = ImageSiteRoot;
            //forumList.IsEditable = IsEditable;

            forumListAlt.Config = config;
            forumListAlt.ModuleId = moduleId;
            forumListAlt.PageId = pageId;
            forumListAlt.SiteRoot = SiteRoot;
            forumListAlt.ImageSiteRoot = ImageSiteRoot;
            //forumListAlt.IsEditable = IsEditable;

            if (displaySettings.UseAltForumList)
            {
                forumList.Visible = false;
                forumListAlt.Visible = true;
            }

            AddClassToBody("editforumsubscriptions");
        }

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

        }

        #endregion

	}
}
