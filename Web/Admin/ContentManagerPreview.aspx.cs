/// Author:					    
/// Created:				    2006-05-17
/// Last Modified:			    2018-03-28
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software. 

using System;
using System.Globalization;
using System.IO;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class ContentManagerPreview : NonCmsBasePage
    {
        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            SuppressMenuSelection();
            SuppressPageMenu();
        }
        #endregion

        private int moduleId = -1;
        private int pageId = -1;
        private bool isAdmin = false;
        private bool isSiteEditor = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            SecurityHelper.DisableBrowserCache();
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}

			LoadParams();
            LoadSettings();

            if ((!isAdmin) && (!isSiteEditor))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();

            if (moduleId > -1)
            {
                PopulateControls();
            }
            else
            {

                WebUtils.SetupRedirect(this, SiteRoot + "/Admin/ContentCatalog.aspx");

            }

        }

        private void PopulateControls()
        {
            Module module = new Module(moduleId);
            if (module.SiteId != siteSettings.SiteId) { return; }
            if (module.ViewRoles == "Admins;")
            {
                if (!WebUser.IsAdmin)
                {
                    SiteUtils.RedirectToAccessDeniedPage(this);
                    return;
                }
            }

            ModuleDefinition feature = new ModuleDefinition(module.ModuleDefId);
            pnlWarning.Visible = !feature.SupportsPageReuse;
            lblModuleTitle.Text = module.ModuleTitle;
            String controlToLoad = "~/" + module.ControlSource;
            if (File.Exists(Server.MapPath(controlToLoad)))
            {
                Control c = Page.LoadControl("~/" + module.ControlSource);

                if (c is SiteModuleControl)
                {
                    SiteModuleControl siteModule = c as SiteModuleControl;

                    siteModule.SiteId = siteSettings.SiteId;
                    siteModule.ModuleConfiguration = module;
                    pnlViewModule.Controls.Add(siteModule);
                    
                }
                else
                {
                    // plain UserControl
                    pnlViewModule.Controls.Add(c);
                }

                lnkPublish.Text = Resource.ContentManagerPublishContentLink;
                lnkPublish.NavigateUrl = SiteRoot + WebConfigSettings.ContentPublishPageRelativeUrl + "?mid=" + this.moduleId;
                if (pageId > -1)
                {
                    lnkPublish.NavigateUrl += "&pageid=" + pageId.ToString(CultureInfo.InvariantCulture);
                }

            }
            else
            {

                WebUtils.SetupRedirect(this, SiteRoot + "/Admin/ContentCatalog.aspx");
                
            }

        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuContentManagerLink);

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            //lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";
            lnkContentManager.Text = Resource.AdminMenuContentManagerLink;
            //lnkContentManager.ToolTip = Resource.AdminMenuContentManagerLink;
            lnkContentManager.NavigateUrl = SiteRoot + "/Admin/ContentCatalog.aspx";


            if (pageId > -1)
            {
                this.lnkBackToList.NavigateUrl = SiteRoot + "/Admin/ContentCatalog.aspx";
                this.lnkBackToList.Visible = true;
                this.lnkBackToList.Text = Resource.ContentManagerBackToListLink;
            }
        }

        private void LoadSettings()
        {
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            isAdmin = WebUser.IsAdminOrContentAdmin;

            AddClassToBody("administration");
            AddClassToBody("cmpreview");
        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);

            
        }

    }
}
