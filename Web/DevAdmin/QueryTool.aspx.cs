// Author:					
// Created:				    2009-12-20
// Last Modified:			2009-12-24
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class QueryToolPage : Page
    {

        //private SiteSettings siteSettings = null;
        protected string SiteRoot = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();

            // the control will handle its own security and redirects
            //if ((siteSettings == null) || (!siteSettings.IsServerAdminSite) || (!WebUser.IsAdmin) || (!WebConfigSettings.EnableDeveloperMenuInAdminMenu))
            //{
            //    SiteUtils.RedirectToAccessDeniedPage(this);
            //    return;
            //}

            PopulateLabels();

            
        }


        

        private void PopulateLabels()
        {
            Title = DevTools.QueryToolLink;

            lnkHome.Text = Resource.HomePageLink;
            lnkHome.NavigateUrl = SiteRoot;
            
            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
            lnkAdvancedTools.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";

            lnkDevTools.Text = DevTools.DevToolsHeading;
            lnkDevTools.NavigateUrl = SiteRoot + "/DevAdmin/Default.aspx";

            lnkThisPage.Text = DevTools.QueryToolLink;
            lnkThisPage.NavigateUrl = SiteRoot + "/DevAdmin/QueryTool.aspx";

        }


        private void LoadSettings()
        {
            //siteSettings = CacheHelper.GetCurrentSiteSettings();
            SiteRoot = SiteUtils.GetNavigationSiteRoot();
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);
            
        }

        
    }
}
