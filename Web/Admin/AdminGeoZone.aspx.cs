/// Author:					
/// Created:				2008-06-23
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
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class AdminGeoZonePage : NonCmsBasePage
    {
        private int pageNumber = 1;
        private int pageSize = 15;
        private int totalPages = 0;
        private string sort = "CountryName, Name";
        private DataTable countryList = null;
        private Guid countryGuid = Guid.Empty;
        // usa a71d6727-61e7-4282-9fcb-526d1e7bc24f
        private Guid defaultCountry = SiteUtils.GetDefaultCountry();

        protected void Page_Load(object sender, EventArgs e)
        {
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}

			LoadSettings();
            if ((!WebUser.IsAdminOrContentAdmin) || (!siteSettings.IsServerAdminSite))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            if (SiteUtils.IsFishyPost(this))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (Page.IsPostBack) return;

            BindCountryList();
            BindGrid();

        }

        private void BindCountryList()
        {
            ddCountry.DataSource = CountryList();
            ddCountry.DataBind();

            if (
                (!Page.IsPostBack)
                && (countryGuid == Guid.Empty)
                )
            {
                countryGuid = defaultCountry;
            }

            if ((!Page.IsPostBack)
                && (countryGuid != Guid.Empty)
                )
            {
                ListItem listItem = ddCountry.Items.FindByValue(countryGuid.ToString());
                if (listItem != null)
                {
                    ddCountry.ClearSelection();
                    listItem.Selected = true;

                }
            }

        }

        private void ddCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            countryGuid = new Guid(ddCountry.SelectedValue);
            pageNumber = 1;
            sort = "CountryName, Name";
            BindGrid();
        }

        private void BindGrid()
        {
            btnAddNew.Visible = true;

            DataTable dt = GeoZone.GetPage(countryGuid, pageNumber, pageSize, out totalPages);
         
            DataView dv = dt.DefaultView;
            dv.Sort = sort + " ASC ";

            string pageUrl = SiteRoot + "/Admin/AdminGeoZone.aspx?country=" + countryGuid.ToString()
                + "&amp;sort=" + sort
                + "&amp;pagenumber={0}";

            pgrGeoZone.Visible = true;
            pgrGeoZone.PageURLFormat = pageUrl;
            pgrGeoZone.ShowFirstLast = true;
            pgrGeoZone.CurrentIndex = pageNumber;
            pgrGeoZone.PageSize = pageSize;
            pgrGeoZone.PageCount = totalPages;
            pgrGeoZone.Visible = (this.totalPages > 1);
            

            grdGeoZone.DataSource = dv;
            grdGeoZone.PageIndex = pageNumber;
            grdGeoZone.PageSize = pageSize;
            grdGeoZone.DataBind();

        }

        private void grdGeoZone_Sorting(object sender, GridViewSortEventArgs e)
        {

            String redirectUrl = SiteRoot + "/Admin/AdminGeoZone.aspx?country=" + ddCountry.SelectedValue
                + "&pagenumber=" + pageNumber.ToString(CultureInfo.InvariantCulture)
                + "&sort=" + e.SortExpression;

            WebUtils.SetupRedirect(this, redirectUrl);

        }

        private void grdGeoZone_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grid = (GridView)sender;
            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());

            TextBox txtName = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtName");
            TextBox txtCode = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtCode");

            GeoZone geoZone;
            if (guid != Guid.Empty)
            {
                geoZone = new GeoZone(guid);
            }
            else
            {
                geoZone = new GeoZone();
            }

            if (ddCountry.SelectedIndex > -1)
            {
                countryGuid = new Guid(ddCountry.SelectedValue);
            }
            else
            {
                if (countryGuid == Guid.Empty)
                {
                    countryGuid = defaultCountry;
                }
            }

            geoZone.CountryGuid = countryGuid;
            geoZone.Name = txtName.Text;
            geoZone.Code = txtCode.Text;
            geoZone.Save();

            WebUtils.SetupRedirect(this, GetRefreshUrl());

        }

        private void grdGeoZone_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            GeoZone.Delete(guid);
            WebUtils.SetupRedirect(this, GetRefreshUrl());

        }

        private void grdGeoZone_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            WebUtils.SetupRedirect(this, GetRefreshUrl());
        }

        private void grdGeoZone_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.EditIndex = e.NewEditIndex;
            
            if (ddCountry.SelectedIndex > -1)
            {
                countryGuid = new Guid(ddCountry.SelectedValue);
            }
            else
            {
                countryGuid = defaultCountry;
            }
            

            BindGrid();

            Button btnDelete = (Button)grid.Rows[e.NewEditIndex].Cells[0].FindControl("btnGridDelete");
            if (btnDelete != null)
            {
                btnDelete.Attributes.Add("OnClick", "return confirm('"
                    + Resource.GeoZoneGridDeleteWarning + "');");
            }
        }



        private void btnAddNew_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("CountryGuid", typeof(Guid));
            dataTable.Columns.Add("CountryName", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Code", typeof(string));
            //dataTable.Columns.Add("TotalPages", typeof(int));

            DataRow row = dataTable.NewRow();
            row["Guid"] = Guid.Empty;
            row["CountryGuid"] = countryGuid;
            row["CountryName"] = string.Empty;
            row["Name"] = string.Empty;
            row["Code"] = string.Empty;
            //row["TotalPages"] = 1;
            dataTable.Rows.Add(row);

            pgrGeoZone.Visible = false;
            btnAddNew.Visible = false;
            grdGeoZone.EditIndex = 0;
            grdGeoZone.PageIndex = pageNumber;
            grdGeoZone.PageSize = pageSize;
            grdGeoZone.DataSource = dataTable.DefaultView;
            grdGeoZone.DataBind();

            Button btnDelete = (Button)grdGeoZone.Rows[grdGeoZone.EditIndex].Cells[0].FindControl("btnGridDelete");
            if (btnDelete != null)
            {
                btnDelete.Visible = false;
            }

            btnAddNew.Visible = false;

        }

        private string GetRefreshUrl()
        {
            if (ddCountry.SelectedIndex > -1)
            {
                countryGuid = new Guid(ddCountry.SelectedValue);
            }

            string refreshUrl = SiteRoot + "/Admin/AdminGeoZone.aspx?country=" + countryGuid.ToString()
                    + "&sort=" + sort
                    + "&pagenumber=" + pageNumber.ToString(CultureInfo.InvariantCulture);

            return refreshUrl;
        }

        protected DataTable CountryList()
        {
            if (countryList == null)
            {
                countryList = GeoCountry.GetList();
            }

            return countryList;

        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.GeoZoneAdministrationHeading);

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkCoreData.Text = Resource.CoreDataAdministrationLink;
            lnkCoreData.NavigateUrl = SiteRoot + "/Admin/CoreData.aspx";

            lnkCurrentPage.Text = Resource.GeoZoneAdministrationLink;
            lnkCurrentPage.NavigateUrl = SiteRoot + "/Admin/AdminGeoZone.aspx";


            heading.Text = Resource.GeoZoneAdministrationHeading;

            grdGeoZone.Columns[1].HeaderText = Resource.GeoZoneGridZoneHeader;
            grdGeoZone.Columns[2].HeaderText = Resource.GeoZoneGridCodeHeader;

            btnAddNew.Text = Resource.GeoZoneGridAddNewButton;
            lnkCountryListAdmin.Text = Resource.CountryAdministrationLink;
            
            lnkCountryListAdmin.NavigateUrl = SiteRoot + "/Admin/AdminCountry.aspx";

        }

        private void LoadSettings()
        {
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            countryGuid = WebUtils.ParseGuidFromQueryString("country", defaultCountry);
            
            if (Page.Request.Params["sort"] != null)
            {
                sort = Page.Request.Params["sort"];
            }

            AddClassToBody("administration");
            AddClassToBody("geoadmin");


        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.grdGeoZone.Sorting += new GridViewSortEventHandler(grdGeoZone_Sorting);
            this.grdGeoZone.RowEditing += new GridViewEditEventHandler(grdGeoZone_RowEditing);
            this.grdGeoZone.RowCancelingEdit += new GridViewCancelEditEventHandler(grdGeoZone_RowCancelingEdit);
            this.grdGeoZone.RowUpdating += new GridViewUpdateEventHandler(grdGeoZone_RowUpdating);
            this.grdGeoZone.RowDeleting += new GridViewDeleteEventHandler(grdGeoZone_RowDeleting);
            this.btnAddNew.Click += new EventHandler(btnAddNew_Click);
            this.ddCountry.SelectedIndexChanged += new EventHandler(ddCountry_SelectedIndexChanged);

            SuppressMenuSelection();
            SuppressPageMenu();
            ScriptConfig.IncludeJQTable = true;


        }

        #endregion
    }
}
