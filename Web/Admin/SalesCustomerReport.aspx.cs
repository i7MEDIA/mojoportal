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
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.AdminUI
{

    public partial class SalesCustomerReportPage : NonCmsBasePage
    {
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        protected bool IsAdmin = false;
        private bool isSiteEditor = false;
        private bool isCommerceReportViewer = false;
        private int pageNumber = 1;
        private int pageSize = 15;
        private int totalPages = 0;

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
            
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.CommerceReportCustomerReportHeading);

            heading.Text = Server.HtmlEncode(siteSettings.SiteName + " - " + Resource.CommerceReportCustomerReportHeading);
            
            using (IDataReader reader = CommerceReport.GetUserItemPageBySite(
                siteSettings.SiteGuid,
                pageNumber,
                pageSize,
                out totalPages))
            {
                string pageUrl = SiteRoot + "/Admin/SalesCustomerReport.aspx"
                    + "?pagenumber={0}";

                pgrUsers.PageURLFormat = pageUrl;
                pgrUsers.ShowFirstLast = true;
                pgrUsers.CurrentIndex = pageNumber;
                pgrUsers.PageSize = pageSize;
                pgrUsers.PageCount = totalPages;
                pgrUsers.Visible = (totalPages > 1);

                grdUsers.DataSource = reader;
                grdUsers.PageIndex = pageNumber;
                grdUsers.PageSize = pageSize;
                grdUsers.DataBind();
            }


        }


        private void PopulateLabels()
        {
            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkCommerceReports.Text = Resource.CommerceReportsLink;
            lnkCommerceReports.NavigateUrl = SiteRoot + "/Admin/SalesSummary.aspx";

            lnkThisPage.Text = Resource.CommerceReportCustomerReportHeading;
            lnkThisPage.NavigateUrl = SiteRoot + "/Admin/SalesCustomerReport.aspx";

            grdUsers.Columns[0].HeaderText = Resource.CommerceReportCustomerHeading;
            grdUsers.Columns[1].HeaderText = Resource.RevenueSummaryRevenue;
        }

        private void LoadSettings()
        {
            IsAdmin = WebUser.IsAdmin;
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            isCommerceReportViewer = WebUser.IsInRoles(siteSettings.CommerceReportViewRoles);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);

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
