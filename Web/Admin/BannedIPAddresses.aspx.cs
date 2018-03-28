/// Author:					
/// Created:				2007-09-23
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
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class BannedIPAddressesPage : NonCmsBasePage
    {
        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 20;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;

        

        protected void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}

			if (!WebUser.IsAdmin)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (!siteSettings.IsServerAdminSite)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Admin/AdminMenu.aspx");
                return;
            }

            if (SiteUtils.IsFishyPost(this))
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
            if (!IsPostBack) BindGrid();

        }

        private void BindGrid()
        {
            if (txtIPAddress.Text.Length > 0)
            {
                BindSearch();
                return;
            }

            List <BannedIPAddress> bannedIPs
                = BannedIPAddress.GetPage(
                pageNumber, 
                pageSize, 
                out totalPages);


            if (this.totalPages > 1)
            {
                string pageUrl = SiteUtils.GetNavigationSiteRoot()
                    + "/Admin/BannedIPAddresses.aspx?pagenumber={0}";

                pgrBannedIPAddresses.PageURLFormat = pageUrl;
                pgrBannedIPAddresses.ShowFirstLast = true;
                pgrBannedIPAddresses.CurrentIndex = pageNumber;
                pgrBannedIPAddresses.PageSize = pageSize;
                pgrBannedIPAddresses.PageCount = totalPages;

            }
            else
            {
                pgrBannedIPAddresses.Visible = false;
            }

            grdBannedIPAddresses.DataSource = bannedIPs;
            grdBannedIPAddresses.PageIndex = pageNumber;
            grdBannedIPAddresses.PageSize = pageSize;
            grdBannedIPAddresses.DataBind();

        }

        void btnIPLookup_Click(object sender, EventArgs e)
        {
            if(txtIPAddress.Text.Length == 0)
            {
                WebUtils.SetupRedirect(this, Request.RawUrl);
                return;
            }

            BindSearch();

        }

        private void BindSearch()
        {
            pgrBannedIPAddresses.Visible = false;

            using (IDataReader reader = BannedIPAddress.GeByIpAddress(txtIPAddress.Text))
            {
                grdBannedIPAddresses.DataSource = reader;
                grdBannedIPAddresses.DataBind();
            }

        }

        void grdBannedIPAddresses_Sorting(object sender, GridViewSortEventArgs e)
        {
            // TODO: 
            //String redirectUrl = WebUtils.GetSiteRoot()
            //    + "/YourPath/AdminBannedIPAddresses.aspx?pageid=" + PageID.ToString(CultureInfo.InvariantCulture)
            //    + "&mid=" + ModuleID.ToString(CultureInfo.InvariantCulture)
            //    + "&pagenumber"
            //    + ModuleID.ToString(CultureInfo.InvariantCulture)
            //    + "=" + pageNumber.ToString(CultureInfo.InvariantCulture)
            //    + "&sort"
            //    + ModuleID.ToString(CultureInfo.InvariantCulture)
            //    + "=" + e.SortExpression;

            //WebUtils.SetupRedirect(this, redirectUrl);

        }

        void grdBannedIPAddresses_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grid = (GridView)sender;

            int rowID = (int)grid.DataKeys[e.RowIndex].Value;
            TextBox txtBannedIP = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtBannedIP");
            TextBox txtBannedUTC = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtBannedUTC");
            TextBox txtBannedReason = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtBannedReason");

            BannedIPAddress bannedIPAddress = new BannedIPAddress(rowID);
            bannedIPAddress.BannedIP = txtBannedIP.Text;
            DateTime bannedTime = DateTime.UtcNow;
            
            DateTime.TryParse(txtBannedUTC.Text, out bannedTime);

            if (timeZone != null)
            {
                bannedTime = bannedTime.ToUtc(timeZone);
            }

            bannedIPAddress.BannedUtc = bannedTime;
            bannedIPAddress.BannedReason = txtBannedReason.Text;
            bannedIPAddress.Save();
            //String pathToCacheDependencyFile
            //        = HttpContext.Current.Server.MapPath(
            //    "~/Data/bannedipcachedependency.config");
            //CacheHelper.TouchCacheFile(pathToCacheDependencyFile);

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        void grdBannedIPAddresses_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            int rowID = (int)grid.DataKeys[e.RowIndex].Value;
            BannedIPAddress.Delete(rowID);
            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        void grdBannedIPAddresses_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        void grdBannedIPAddresses_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.EditIndex = e.NewEditIndex;
            BindGrid();

            Button btnDelete = (Button)grid.Rows[e.NewEditIndex].Cells[0].FindControl("btnGridDelete");
            if (btnDelete != null)
            {
                btnDelete.Attributes.Add("OnClick", "return confirm('"
                    + Resource.BannedIPAddressDeleteConfirmMessage + "');");
            }
        }

        void btnAddNew_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("RowID", typeof(int));
            dataTable.Columns.Add("BannedIP", typeof(string));
            dataTable.Columns.Add("BannedUTC", typeof(DateTime));
            dataTable.Columns.Add("BannedReason", typeof(string));
            dataTable.Columns.Add("TotalPages", typeof(int));

            DataRow row = dataTable.NewRow();
            row["RowID"] = -1;
            row["BannedIP"] = string.Empty;
            row["BannedUTC"] = DateTime.UtcNow;
            row["BannedReason"] = string.Empty;
            row["TotalPages"] = 1;
            dataTable.Rows.Add(row);

            grdBannedIPAddresses.EditIndex = 0;
            grdBannedIPAddresses.DataSource = dataTable.DefaultView;
            grdBannedIPAddresses.DataBind();

            btnAddNew.Visible = false;
            pgrBannedIPAddresses.Visible = false;

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.BannedIPAddressesHeading);
            heading.Text = Resource.BannedIPAddressesHeading;

            btnAddNew.Text = Resource.BannedIPAddressesAddNewButton;
            grdBannedIPAddresses.Columns[1].HeaderText = Resource.BannedIPAddressLabel;
            grdBannedIPAddresses.Columns[2].HeaderText = Resource.BannedIPAddressReasonLabel;
            grdBannedIPAddresses.Columns[3].HeaderText = Resource.BannedIPAddressUTCLabel;

            
            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
            lnkAdvancedTools.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";

            lnkBannedIPs.Text = Resource.AdminMenuBannedIPAddressesLabel;
            lnkBannedIPs.NavigateUrl = SiteRoot + "/Admin/BannedIPAddresses.aspx";

            btnIPLookup.Text = Resource.BannedIPSearchButton;

        }

        private void LoadSettings()
        {
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            AddClassToBody("administration");
            AddClassToBody("ipadmin");

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.grdBannedIPAddresses.Sorting += new GridViewSortEventHandler(grdBannedIPAddresses_Sorting);
            this.grdBannedIPAddresses.RowEditing += new GridViewEditEventHandler(grdBannedIPAddresses_RowEditing);
            this.grdBannedIPAddresses.RowCancelingEdit += new GridViewCancelEditEventHandler(grdBannedIPAddresses_RowCancelingEdit);
            this.grdBannedIPAddresses.RowUpdating += new GridViewUpdateEventHandler(grdBannedIPAddresses_RowUpdating);
            this.grdBannedIPAddresses.RowDeleting += new GridViewDeleteEventHandler(grdBannedIPAddresses_RowDeleting);
            this.btnAddNew.Click += new EventHandler(btnAddNew_Click);
            btnIPLookup.Click += new EventHandler(btnIPLookup_Click);

            SuppressMenuSelection();
            SuppressPageMenu();
            ScriptConfig.IncludeJQTable = true;
            
        }

        

        #endregion
    }
}
