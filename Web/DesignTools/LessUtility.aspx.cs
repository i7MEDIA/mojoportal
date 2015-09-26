// Author:					Joe Audette
// Created:					2012-05-02
// Last Modified:			2012-05-02
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;
using dotless.Core;
using dotless.Core.configuration;

namespace mojoPortal.Web.AdminUI
{
	
    public partial class LessUtilityPage : mojoBasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins)))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();
            PopulateLabels();
        }

        protected void btnConvert_Click(object sender, EventArgs e)
        {
            DotlessConfiguration config = DotlessConfiguration.GetDefaultWeb();

            config.CacheEnabled = false;
            config.DisableUrlRewriting = true;
            config.DisableParameters = true;
            config.MapPathsToWeb = false;
            config.MinifyOutput = false;
            config.HandleWebCompression = false;

            txtOutput.Text = Less.Parse(txtInput.Text, config);
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, DevTools.LessUtility);
            heading.Text = DevTools.LessUtility;

            litLessInstructions.Text = DevTools.LessInstructions;

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
            lnkAdvancedTools.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";

            lnkDesignerTools.Text = DevTools.DesignTools;
            lnkDesignerTools.NavigateUrl = SiteRoot + "/DesignTools/Default.aspx";

            lnkThisPage.Text = DevTools.LessUtility;
            lnkThisPage.NavigateUrl = SiteRoot + "/DesignTools/LessUtility.aspx";

            
            btnConvert.Text = DevTools.GenerateCss;

        }

        private void LoadSettings()
        {
           

            AddClassToBody("administration");
            AddClassToBody("designtools");
        }

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            btnConvert.Click += new EventHandler(btnConvert_Click);

            SuppressMenuSelection();
            SuppressPageMenu();

        }

        #endregion
    }
}