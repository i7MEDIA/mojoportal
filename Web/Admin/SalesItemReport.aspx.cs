// Author:					
// Created:					2009-02-11
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

    public partial class SalesItemReportPage : NonCmsBasePage
    {
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        private bool isSiteEditor = false;
        private bool isCommerceReportViewer = false;
        private Guid moduleGuid = Guid.Empty;
        private Module module = null;
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
                (!WebUser.IsAdmin)
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
            if (module == null) { return; }

            Title = SiteUtils.FormatPageTitle(siteSettings, module.ModuleTitle + " - " + Resource.CommerceReportItemSales);

            heading.Text = module.ModuleTitle + " - " + Resource.CommerceReportItemSales;
            lnkModuleReport.Text = module.ModuleTitle;
            lnkModuleReport.NavigateUrl = SiteRoot + "/Admin/SalesByModule.aspx?m=" + moduleGuid.ToString();

            using (IDataReader reader = CommerceReport.GetItemsPageByModule(
                moduleGuid,
                pageNumber,
                pageSize,
                out totalPages))
            {
                string pageUrl = SiteRoot + "/Admin/SalesItemReport.aspx"
                    + "?m=" + moduleGuid.ToString()
                    + "&amp;pagenumber={0}";

                pgrItems.PageURLFormat = pageUrl;
                pgrItems.ShowFirstLast = true;
                pgrItems.CurrentIndex = pageNumber;
                pgrItems.PageSize = pageSize;
                pgrItems.PageCount = totalPages;
                pgrItems.Visible = (totalPages > 1);

                grdItems.DataSource = reader;
                grdItems.PageIndex = pageNumber;
                grdItems.PageSize = pageSize;
                grdItems.DataBind();
            }


        }


        private void PopulateLabels()
        {
            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkCommerceReports.Text = Resource.CommerceReportsLink;
            lnkCommerceReports.NavigateUrl = SiteRoot + "/Admin/SalesSummary.aspx";

            lnkThisPage.Text = Resource.CommerceReportItemSales;
            lnkThisPage.NavigateUrl = SiteRoot + Request.RawUrl;

            grdItems.Columns[0].HeaderText = Resource.CommerceReportItemHeading;
            grdItems.Columns[1].HeaderText = Resource.RevenueSummaryUnitsSold;
            grdItems.Columns[2].HeaderText = Resource.RevenueSummaryRevenue;
        }

        private void LoadSettings()
        {
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            isCommerceReportViewer = WebUser.IsInRoles(siteSettings.CommerceReportViewRoles);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);
            moduleGuid = WebUtils.ParseGuidFromQueryString("m", moduleGuid);
            if (moduleGuid != Guid.Empty)
            {
                module = new Module(moduleGuid);

                if (module.SiteGuid != siteSettings.SiteGuid) { module = null; }
            }

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
