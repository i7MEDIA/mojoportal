using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web 
{
    public abstract class SiteModuleControl : mojoUserControl
	{
        private Module moduleConfiguration;
        private bool isEditable = false;
        private bool forbidModuleSettings = false;
        private int siteID = -1;
        private Hashtable settings;
		private string imageSiteRoot = null;
        private bool isSiteEditor = false;
        private bool enableWorkflow = false;
        private bool use3LevelWorkflow = false; //joe davis
        private bool IsOnInitExecuted = false;

	    protected PageSettings currentPage;
	    protected SiteSettings siteSettings;
        protected ScriptManager ScriptController;
        
        

	    
        protected override void OnInit(EventArgs e)
        {
            // Alexander Yushchenko: workaround to make old custom modules work
            // Before 03.19.2007 this method was "new" and called from descendant classes
            // To avoid multiple self-calls a boolean flag is used
            if (IsOnInitExecuted) return;
            IsOnInitExecuted = true;

            base.OnInit(e);

            if (HttpContext.Current == null) { return; }

            siteSettings = CacheHelper.GetCurrentSiteSettings();
            
            currentPage = CacheHelper.GetCurrentPage();
            ScriptController = (ScriptManager)Page.Master.FindControl("ScriptManager1");

            if (siteSettings != null)
            {
                this.siteID = siteSettings.SiteId;
                if (!WebUser.IsAdminOrContentAdmin)
                {
                    forbidModuleSettings = WebUser.IsInRoles(siteSettings.RolesNotAllowedToEditModuleSettings);
                }
            }

            if (Page.Request.IsAuthenticated)
            {
                isSiteEditor = SiteUtils.UserIsSiteEditor();

                //if (WebUser.IsAdminOrContentAdmin || isSiteEditor || WebUser.IsInRoles(currentPage.EditRoles)
                //    || ((moduleConfiguration != null)
                //           && (WebUser.IsInRoles(moduleConfiguration.AuthorizedEditRoles))
                //       )
                //   )
                //{
                //    isEditable = true;
                //}

                isEditable = ShouldAllowEdit();

                if ((moduleConfiguration != null)&&(!moduleConfiguration.IsGlobal))
                {
                    if (WebConfigSettings.EnableContentWorkflow && siteSettings.EnableContentWorkflow && (this is IWorkflow))
                    {
                        enableWorkflow = true;

                        use3LevelWorkflow = WebConfigSettings.Use3LevelContentWorkflow; //joe davis

                        if (!isEditable)
                        {
                            if (
                                (WebUser.IsInRoles(currentPage.DraftEditOnlyRoles)) 
                                || (WebUser.IsInRoles(moduleConfiguration.DraftEditRoles))
                                || (use3LevelWorkflow && WebUser.IsInRoles(moduleConfiguration.DraftApprovalRoles))
                                )
                            {
                                isEditable = true;

                            }

                        }
                    }
                }
                
                if (!isEditable && (moduleConfiguration != null) && (moduleConfiguration.EditUserId > 0))
                {
                    SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                    if (
                        (siteUser != null)
                        &&(moduleConfiguration.EditUserId == siteUser.UserId)
                        )
                    {
                        isEditable = true;
                    }
                }
            }

            if (moduleConfiguration != null)
            {
                this.m_title = moduleConfiguration.ModuleTitle;
                this.m_description = moduleConfiguration.FeatureName;
            }
        }

        private bool ShouldAllowEdit()
        {
            if (WebUser.IsAdmin)
            {
                return true;
            }
            
            if (moduleConfiguration != null)
            {
                if (moduleConfiguration.AuthorizedEditRoles == "Admins;") { return false; }
                if (currentPage.EditRoles == "Admins;") { return false; }

                if (WebUser.IsContentAdmin)  { return true; }

                if (isSiteEditor) { return true; }

                if (WebUser.IsInRoles(moduleConfiguration.AuthorizedEditRoles)) { return true; }

                if((!moduleConfiguration.IsGlobal)&&WebUser.IsInRoles(currentPage.EditRoles)) { return true;}

            }

            return false;
        }


        public bool RenderInWebPartMode { get; set; } = false;


		public int ModuleId
        {
            get { return moduleConfiguration == null ? 0 : moduleConfiguration.ModuleId; }
            set
            {
                if (moduleConfiguration == null) moduleConfiguration = new Module(value);
                moduleConfiguration.ModuleId = value;
            }
        }

        public Guid ModuleGuid
        {
            get { return moduleConfiguration == null ? Guid.Empty : moduleConfiguration.ModuleGuid; }
            set
            {
                if (moduleConfiguration == null) moduleConfiguration = new Module(value);
                moduleConfiguration.ModuleGuid = value;
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PageId
        {
            get { return moduleConfiguration == null ? 0 : moduleConfiguration.PageId; }
        }


		public string SiteRoot
		{
			get 
            {
                //if ((siteSettings != null)
                //    &&(siteSettings.SiteFolderName.Length > 0))
                //{
                //    return siteSettings.SiteRoot;
                //}
                //return WebUtils.GetSiteRoot(); 
                return SiteUtils.GetNavigationSiteRoot();
            }
		}

        public string ImageSiteRoot
        {
            get
            {
                if (imageSiteRoot == null)
                {
                    //imageSiteRoot = WebUtils.GetSiteRoot();
                    imageSiteRoot = SiteUtils.GetImageSiteRoot(Page);
                }
                return imageSiteRoot;
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEditable
        {
            get { return isEditable; }
        }

        public bool ForbidModuleSettings
        {
            get { return forbidModuleSettings; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool EnableWorkflow
        {
            get { return enableWorkflow; }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Module ModuleConfiguration
        {
            get { return moduleConfiguration; }
            set { moduleConfiguration = value; }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Hashtable Settings
        {
            get
            {
                if (settings == null) settings = ModuleSettings.GetModuleSettings(ModuleId);
                return settings;
            }
        }


         #region IWebPart Members

         private string m_title = String.Empty;
         private string m_subTitle = String.Empty;
         private string m_description = String.Empty;
         private string m_titleUrl = String.Empty;
         private string m_titleIconImageUrl = String.Empty;
         private string m_catalogIconImageUrl = String.Empty;


         // Title
         public string Title
         {
             get
             {
                 return m_title;
             }
             set
             {
                 m_title = value;
             }
         }
         //  Subtitle
         public string Subtitle
         {
             get
             {
                 return m_subTitle;
             }
             set
             {
                 m_subTitle = value;
             }
         }
         //  Description
         public string Description
         {
             get
             {
                 return m_description;
             }
             set
             {
                 m_description = value;
             }
         }
         //  TitleUrl
         public string TitleUrl
         {
             get
             {
                 return m_titleUrl;
             }
             set
             {
                 m_titleUrl = value;
             }
         }
         //  TitleIconImageUrl
         public string TitleIconImageUrl
         {
             get
             {
                 return m_titleIconImageUrl;
             }
             set
             {
                 m_titleIconImageUrl = value;
             }
         }
         //  CatalogIconImageUrl
         public string CatalogIconImageUrl
         {
             get
             {
                 return m_catalogIconImageUrl;
             }
             set
             {
                 m_catalogIconImageUrl = value;
             }
         }


         #endregion

    }
    
    
}
