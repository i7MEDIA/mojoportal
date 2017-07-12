// Author:					
// Created:				    2008-07-03
// Last Modified:			2009-12-26
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Web.UI;
using Resources;

namespace mojoPortal.Web.AdminUI
{
   
    public partial class ServerVariablesPage : Page
    {
        protected string SiteRoot = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            LoadSettings();
            PopulateLabels();
            

        }

        
        private void PopulateLabels()
        {
            Title = Resource.ServerVariablesHeading;
            litHeading.Text = Resource.ServerVariablesHeading;

            lnkHome.Text = Resource.HomePageLink;
            lnkHome.NavigateUrl = SiteRoot;

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
            lnkAdvancedTools.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";

            lnkDevTools.Text = DevTools.DevToolsHeading;
            lnkDevTools.NavigateUrl = "~/DevAdmin/Default.aspx";

            lnkThisPage.Text = DevTools.ServerVariablesLink;
            lnkThisPage.NavigateUrl = "~/DevAdmin/ServerVariables.aspx";


        }

        

        private void LoadSettings()
        {
            SiteRoot = SiteUtils.GetNavigationSiteRoot();

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

           

        }

        #endregion
    }
}
