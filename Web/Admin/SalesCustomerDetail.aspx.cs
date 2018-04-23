// Author:					
// Created:					2009-02-14
// Last Modified:			2018-03-28
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web.UI;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;



namespace mojoPortal.Web.AdminUI
{

    public partial class SalesCustomerDetailPage : NonCmsBasePage
    {
        private Guid userGuid = Guid.Empty;
        protected bool IsAdmin = false;
        private bool isSiteEditor = false;
        private bool isCommerceReportViewer = false;
        private SiteUser customerUser = null;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SiteUtils.SslIsAvailable()) { SiteUtils.ForceSsl(); }
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadSettings();
            if (
                (!IsAdmin)
                && (!isSiteEditor)
                && (!isCommerceReportViewer)
                )
            {
				SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (customerUser == null) { return; }

            Title = SiteUtils.FormatPageTitle(siteSettings, customerUser.Name + " - "
                + Resource.CommercePurchaseHistory);

            heading.Text = Server.HtmlEncode(customerUser.Name + " - " + Resource.CommercePurchaseHistory);

            lnkThisPage.Text = customerUser.Name;
        }


        private void PopulateLabels()
        {
            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkCommerceReports.Text = Resource.CommerceReportsLink;
            lnkCommerceReports.NavigateUrl = SiteRoot + "/Admin/SalesSummary.aspx";

            lnkCustomerReport.Text = Resource.CommerceReportCustomerReportHeading;
            lnkCustomerReport.NavigateUrl = SiteRoot + "/Admin/SalesCustomerReport.aspx";

            lnkThisPage.Text = Resource.CommerceReportCustomerHeading;
            lnkThisPage.NavigateUrl = SiteRoot + Request.RawUrl;
        }

        private void LoadSettings()
        {
            IsAdmin = WebUser.IsAdmin;
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            isCommerceReportViewer = WebUser.IsInRoles(siteSettings.CommerceReportViewRoles);

            userGuid = WebUtils.ParseGuidFromQueryString("u", userGuid);

            customerUser = new SiteUser(siteSettings, userGuid);

            UserCommerceHistory commerceHistory = purchaseHx as UserCommerceHistory;

            commerceHistory.UserGuid = userGuid;
            commerceHistory.ShowAdminOrderLink = true;

            AddClassToBody("administration");
            AddClassToBody("commercereports");

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            SuppressPageMenu();
            SuppressMenuSelection();
            ScriptConfig.IncludeJQTable = true;

        }

        #endregion
    }
}
