// Author:					
// Created:					2009-02-08
// Last Modified:			2018-03-28
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

//using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Web.UI;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;
using ZedGraph;
using ZedGraph.Web;

namespace mojoPortal.Web.AdminUI
{

    public partial class SalesByModulePage : NonCmsBasePage
    {
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        private System.Data.DataTable salesByYearData = null;
        private System.Data.DataTable salesByMonthData = null;
        private bool isSiteEditor = false;
        private bool isCommerceReportViewer = false;
        private Guid moduleGuid = Guid.Empty;
        private Module module = null;

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
            Title = SiteUtils.FormatPageTitle(siteSettings, module.ModuleTitle + " - " + Resource.SalesOverviewReportHeading);

            if (moduleGuid == Guid.Empty) { return; }
            if (salesByMonthData == null) { salesByMonthData = CommerceReport.GetSalesByYearMonthByModule(moduleGuid); }
            if (salesByMonthData == null) { return; }
            

            

            heading.Text = Server.HtmlEncode(module.ModuleTitle);
            lnkThisPage.Text = module.ModuleTitle;
            lnkAllItems.NavigateUrl = SiteRoot + "/Admin/SalesItemReport.aspx?m=" + moduleGuid.ToString();

            try
            {
                if (salesByYearData == null) { salesByYearData = CommerceReport.GetSalesByYearByModule(moduleGuid); }
                grdSalesByYear.DataSource = salesByYearData;
                grdSalesByYear.DataBind();
            }
            catch
            {
                //this will throw an error until the upgrade script for 2.3.4.6 is enabled
                grdSalesByYear.Visible = false;
            } 

            grdSales.DataSource = salesByMonthData;
            grdSales.DataBind();

            DataTable dt = CommerceReport.GetItemRevenueByModule(moduleGuid);
            grdTopItems.DataSource = dt;
            grdTopItems.DataBind();

            decimal allTimeRevenue = CommerceReport.GetAllTimeRevenueByModule(moduleGuid);

            litAllTimeRevenue.Text = string.Format(currencyCulture,
                Resource.AllTimeRevenueFormatString, allTimeRevenue.ToString("c", currencyCulture));

            //if (WebConfigSettings.DisableZedGraph)
            //{
            //    BindBarChart();
            //}

        }

        //private void BindBarChart()
        //{
        //    zgSales.Visible = false;
        //    bcSales.Visible = true;
        //    if (salesByMonthData == null) { salesByMonthData = CommerceReport.GetSalesByYearMonthByModule(moduleGuid); }

        //    StringBuilder categories = new StringBuilder();

        //    string comma = string.Empty;
        //    List<decimal> revenue = new List<decimal>();

        //    // original data is sorted descending on Y, M resorting here 
        //    DataRow[] result = salesByMonthData.Select(string.Empty, "Y ASC, M ASC");

        //    int spaceInterval = 0;
        //    int totalItems = result.Length;
        //    int itemsAdded = 0;
        //    if (totalItems > 12)
        //    {
        //        spaceInterval = 4;
        //    }
        //    int nextItemToShow = 0;

        //    foreach (DataRow row in result)
        //    {
        //        categories.Append(comma);

        //        if (itemsAdded == nextItemToShow)
        //        {
        //            categories.Append(row["Y"].ToString());
        //            categories.Append("-");
        //            categories.Append(row["M"].ToString());
        //            nextItemToShow = itemsAdded + spaceInterval;
        //        }

        //        comma = ",";

        //        revenue.Add(Convert.ToDecimal(row["Sales"]));
        //        itemsAdded += 1;

        //    }

        //    bcSales.ChartTitle = Resource.SalesByMonthChartLabel;


        //    bcSales.CategoriesAxis = categories.ToString();
        //    BarChartSeries series = new BarChartSeries();
        //    //series.Name = Resource.SalesByMonthChartSalesLabel;
        //    series.Data = revenue.ToArray();

        //    bcSales.Series.Add(series);

        //    //bcSales.CategoriesAxis
        //    //bcSales.Series.

        //}

        //private void BindChart()
        //{
        //    // line chart is very slow to render when there are lots of data points
        //    // bar chart is much faster
        //    zgSales.Visible = false;
        //    lcSales.Visible = true;
        //    if (salesByMonthData == null) { salesByMonthData = CommerceReport.GetSalesByYearMonthByModule(moduleGuid); }

        //    StringBuilder categories = new StringBuilder();

        //    string comma = string.Empty;
        //    List<decimal> revenue = new List<decimal>();

        //    // original data is sorted descending on Y, M resorting here 
        //    DataRow[] result = salesByMonthData.Select(string.Empty, "Y ASC, M ASC");

        //    int spaceInterval = 0;
        //    int totalItems = result.Length;
        //    int itemsAdded = 0;
        //    if (totalItems > 12)
        //    {
        //        spaceInterval = 4;
        //    }
        //    int nextItemToShow = 0;

        //    foreach (DataRow row in result)
        //    {
        //        categories.Append(comma);

        //        if (itemsAdded == nextItemToShow)
        //        {
        //            categories.Append(row["Y"].ToString());
        //            categories.Append("-");
        //            categories.Append(row["M"].ToString());
        //            nextItemToShow = itemsAdded + spaceInterval;
        //        }
                
        //        comma = ",";

        //        revenue.Add(Convert.ToDecimal(row["Sales"]));
        //        itemsAdded += 1;
                
        //    }

        //    lcSales.ChartTitle = Resource.SalesByMonthChartLabel;
            

        //    lcSales.CategoriesAxis = categories.ToString();
        //    LineChartSeries series = new LineChartSeries();
        //    series.Name = Resource.SalesByMonthChartSalesLabel;
        //    series.Data = revenue.ToArray();

        //    lcSales.Series.Add(series);

        //    //bcSales.CategoriesAxis
        //    //bcSales.Series.

        //}

        private void OnRenderUserChart(ZedGraphWeb z, Graphics g, MasterPane masterPane)
        {
            if (module == null) { return; }
            if (moduleGuid == Guid.Empty) { return; }

            GraphPane graphPane = masterPane[0];
            graphPane.Title.Text = Resource.SalesByMonthChartLabel;
            graphPane.XAxis.Title.Text = Resource.SalesByMonthChartMonthLabel;
            graphPane.YAxis.Title.Text = Resource.SalesByMonthChartSalesLabel;

            PointPairList pointList = new PointPairList();

            if (salesByMonthData == null) { salesByMonthData = CommerceReport.GetSalesByYearMonthByModule(moduleGuid); }

            foreach (DataRow row in salesByMonthData.Rows)
            {
                double x = new XDate(Convert.ToInt32(row["Y"]), Convert.ToInt32(row["M"]), 1);
                double y = Convert.ToDouble(row["Sales"]);
                pointList.Add(x, y);
            }

            LineItem myCurve2 = graphPane.AddCurve(Resource.SalesByMonthChartLabel, pointList, Color.Blue, SymbolType.Circle);
            // Fill the area under the curve with a white-red gradient at 45 degrees
            myCurve2.Line.Fill = new Fill(Color.White, Color.Green, 45F);
            // Make the symbols opaque by filling them with white
            myCurve2.Symbol.Fill = new Fill(Color.White);

            // Set the XAxis to date type
            graphPane.XAxis.Type = AxisType.Date;
            graphPane.XAxis.CrossAuto = true;

            // Fill the axis background with a color gradient
            graphPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45F);

            masterPane.AxisChange(g);
        }




        private void PopulateLabels()
        {
            
            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkCommerceReports.Text = Resource.CommerceReportsLink;
            lnkCommerceReports.NavigateUrl = SiteRoot + "/Admin/SalesSummary.aspx";

            lnkThisPage.Text = Resource.CommerceReportsLink;
            lnkThisPage.NavigateUrl = SiteRoot + Request.RawUrl;
            lnkAllItems.Text = Resource.CommerceReportSeeAllItemsLink;

            grdSalesByYear.Columns[0].HeaderText = Resource.YearLabel;
            grdSalesByYear.Columns[1].HeaderText = Resource.SalesLabel;
            grdSalesByYear.Columns[2].HeaderText = Resource.UnitsSold;

            grdSales.Columns[0].HeaderText = Resource.YearLabel;
            grdSales.Columns[1].HeaderText = Resource.MonthLabel;
            grdSales.Columns[2].HeaderText = Resource.SalesLabel;
            grdSales.Columns[3].HeaderText = Resource.UnitsSold;

            grdTopItems.Columns[0].HeaderText = Resource.CommerceReportTopItemsHeading;
            grdTopItems.Columns[1].HeaderText = Resource.RevenueSummaryUnitsSold;
            grdTopItems.Columns[2].HeaderText = Resource.RevenueSummaryRevenue;

        }

        private void LoadSettings()
        {
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            isCommerceReportViewer = WebUser.IsInRoles(siteSettings.CommerceReportViewRoles);

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

            if (!WebConfigSettings.DisableZedGraph)
            {
                this.zgSales.RenderGraph += new ZedGraph.Web.ZedGraphWebControlEventHandler(this.OnRenderUserChart);

                this.zgSales.RenderedImagePath = "~/Data/Sites/" + siteSettings.SiteId.ToString()
                + "/systemfiles/";
            }

            SuppressPageMenu();
            SuppressMenuSelection();
            ScriptConfig.IncludeJQTable = true;

        }

        #endregion
    }
}
