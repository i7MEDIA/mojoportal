/// Author:					
/// Created:				2008-02-04
/// Last Modified:			2011-03-15
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.AdminUI
{

    public partial class DevToolsPage : NonCmsBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
            if (
                (!WebUser.IsAdmin)
                || (!siteSettings.IsServerAdminSite)
                || (!WebConfigSettings.EnableDeveloperMenuInAdminMenu))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {


        }


        private void PopulateLabels()
        {
            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
            lnkAdvancedTools.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";

            lnkThisPage.Text = DevTools.DevToolsHeading;
            lnkThisPage.NavigateUrl = SiteRoot + "/DevAdmin/Default.aspx";

            //lnkDataAdmin.Text = DevTools.DBDataAdminHeading;
            //lnkDataAdmin.NavigateUrl = SiteRoot + "/DevAdmin/dbadmin.aspx";
            

            Title = SiteUtils.FormatPageTitle(siteSettings, DevTools.DevToolsHeading);
            heading.Text = DevTools.DevToolsHeading;

            //lnkCodeGenerator.Text = DevTools.CodeGeneratorLink;
            //lnkCodeGenerator.NavigateUrl = SiteRoot + "/DevAdmin/CodeGenerator.aspx";

            lnkServerVariables.Text = Resource.ServerVariablesLink;
            lnkServerVariables.NavigateUrl = SiteRoot + "/DevAdmin/ServerVariables.aspx";

            lnkQueryTool.Text = DevTools.QueryToolLink;
            lnkQueryTool.NavigateUrl = SiteRoot + "/DevAdmin/QueryTool.aspx";

            
        }

        private void LoadSettings()
        {
            liQueryTool.Visible = WebConfigSettings.EnableQueryTool;

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            SuppressPageMenu();


        }

        #endregion
    }
}