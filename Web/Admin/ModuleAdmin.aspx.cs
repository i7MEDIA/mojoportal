/// Author:					
/// Created:				2004-07-21
/// Last Modified:			2018-03-28
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class ModuleAdminPage : NonCmsBasePage
    {
        //protected string EditLabel = string.Empty;
        //protected string SettingsLabel = string.Empty;

        private bool IsAdmin = false;
        protected string EditPropertiesImage =string.Empty;
        protected bool isServerAdminSite = false;

        protected void Page_Load(object sender, EventArgs e)
        {
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadSettings();

            if (!IsAdmin)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/AccessDenied.aspx");
                return;
            }

            if (SiteUtils.IsFishyPost(this))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            //if (!siteSettings.IsServerAdminSite)
            //{
            //    WebUtils.SetupRedirect(this, SiteRoot + "/Admin/AdminMenu.aspx");
            //    return;
            //}
            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (Page.IsPostBack) return;

            using (IDataReader reader = ModuleDefinition.GetModuleDefinitions(siteSettings.SiteGuid))
            {
                defsList.DataSource = reader;
                defsList.DataBind();
            }

        }

        

        private void DefsList_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            // TODO: why not make this a link instead of
            // postback then redirect? JA

            int moduleDefID = (int)defsList.DataKeys[e.Item.ItemIndex];
            string redirectUrl
                = SiteRoot + "/Admin/ModuleDefinitions.aspx?defId="
                + moduleDefID.ToString(CultureInfo.InvariantCulture);

            WebUtils.SetupRedirect(this, redirectUrl);

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuFeatureModulesLink);
            heading.Text = Resource.AdminMenuFeatureModulesLink;

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
            lnkAdvancedTools.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";

            lnkFeatureAdmin.Text = Resource.AdminMenuFeatureModulesLink;
            lnkFeatureAdmin.ToolTip = Resource.AdminMenuFeatureModulesLink;
            lnkFeatureAdmin.NavigateUrl = SiteRoot + "/Admin/ModuleAdmin.aspx";

            lnkNewModule.Text = Resource.ModuleDefsAddButton;
            lnkNewModule.ToolTip = Resource.ModuleDefsAddButton;
            
        }


        protected String GetEditImageUrl()
        {
            return ImageSiteRoot + "/Data/SiteImages/" + EditPropertiesImage;
        }

        private void LoadSettings()
        {
            IsAdmin = WebUser.IsAdmin;
            EditPropertiesImage = WebConfigSettings.EditPropertiesImage;
            isServerAdminSite = siteSettings.IsServerAdminSite;

            lnkNewModule.Visible = isServerAdminSite;

            AddClassToBody("administration");
            AddClassToBody("moduleadmin");

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.defsList.ItemCommand += new DataListCommandEventHandler(this.DefsList_ItemCommand);

            SuppressMenuSelection();
            SuppressPageMenu();
            
        }

        #endregion
    }
}
