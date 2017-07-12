// Author:					
// Created:					2009-02-14
// Last Modified:			2009-12-16
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

namespace mojoPortal.Web.UI
{
    public partial class UserCommerceHistory : UserControl
    {
        private Guid userGuid = Guid.Empty;
        private bool showAdminOrderLink = false;
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        private SiteSettings siteSettings = null;
        private int pageNumber = 1;
        private int totalPages = 1;
        private int pageSize = 10;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;
        protected string SiteRoot = string.Empty;

        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        public bool ShowAdminOrderLink
        {
            get { return showAdminOrderLink; }
            set { showAdminOrderLink = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            pgrItems.Command += new System.Web.UI.WebControls.CommandEventHandler(pgrItems_Command);
        }

        

        protected void Page_Load(object sender, EventArgs e)
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            SiteRoot = SiteUtils.GetNavigationSiteRoot();
            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }
            PopulateLabels();

        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (Page.IsPostBack) { return; }
            if (userGuid == Guid.Empty) { this.Visible = false; return; }

            if (siteSettings == null) { this.Visible = false; return; }

            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);
            BindGrid();

        }


        private void BindGrid()
        {
            using (IDataReader reader = CommerceReport.GetUserItemsPage(siteSettings.SiteGuid, userGuid, pageNumber, pageSize, out totalPages))
            {
                grdUserItems.DataSource = reader;
                grdUserItems.DataBind();

                if (this.totalPages > 1)
                {

                    pgrItems.ShowFirstLast = true;
                    pgrItems.PageSize = pageSize;
                    pgrItems.PageCount = totalPages;
                }
                else
                {
                    pgrItems.Visible = false;
                }

            }


        }

        void pgrItems_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            pageNumber = Convert.ToInt32(e.CommandArgument);
            pgrItems.CurrentIndex = pageNumber;
            BindGrid();
            updItems.Update();
        }

        private void PopulateLabels()
        {
            grdUserItems.Columns[0].HeaderText = Resource.CommerceReportSourceHeading;
            grdUserItems.Columns[1].HeaderText = Resource.CommerceReportItemHeading;
            grdUserItems.Columns[2].HeaderText = Resource.CommerceReportPriceHeading;
            grdUserItems.Columns[3].HeaderText = Resource.CommerceReportOrderDateHeading;
        }

    }
}