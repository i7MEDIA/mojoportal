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
using System.Collections.Generic;
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

    public partial class AdminCountryPage : NonCmsBasePage
    {
        private int pageNumber = 1;
        private int pageSize = 15;
        private int totalPages = 0;
        private string sort = "Name";

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

            BindGrid();

        }

        private void BindGrid()
        {
            btnAddNew.Visible = true;

            List<GeoCountry> countries = GeoCountry.GetPage(pageNumber, pageSize, out totalPages);
            Comparison<GeoCountry> sortComparer;

            switch (sort)
            {
                case "ISOCode2":
                    sortComparer
                        = new Comparison<GeoCountry>(GeoCountry.CompareByIsoCode2);
                    break;

                case "ISOCode3":
                    sortComparer
                        = new Comparison<GeoCountry>(GeoCountry.CompareByIsoCode3);
                    break;

                case "Name":
                default:
                    sortComparer
                        = new Comparison<GeoCountry>(GeoCountry.CompareByName);
                    break;
            }

            if (sortComparer != null)
            {
                countries.Sort(sortComparer);
            }



            if (this.totalPages > 1)
            {
                string pageUrl = SiteRoot + "/Admin/AdminCountry.aspx?pagenumber={0}&amp;sort=" + sort;

                pgrCountry.Visible = true;
                pgrCountry.PageURLFormat = pageUrl;
                pgrCountry.ShowFirstLast = true;
                pgrCountry.CurrentIndex = pageNumber;
                pgrCountry.PageSize = pageSize;
                pgrCountry.PageCount = totalPages;

            }
            else
            {
                pgrCountry.Visible = false;
            }

            grdCountry.DataSource = countries;
            grdCountry.PageIndex = pageNumber;
            grdCountry.PageSize = pageSize;
            grdCountry.DataBind();

        }

        private void grdCountry_Sorting(object sender, GridViewSortEventArgs e)
        {
            String redirectUrl = SiteRoot
                + "/Admin/AdminCountry.aspx?pagenumber=" + pageNumber.ToString(CultureInfo.InvariantCulture)
                + "&sort=" + e.SortExpression;

            WebUtils.SetupRedirect(this, redirectUrl);

        }

        private void grdCountry_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grid = (GridView)sender;
            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());

            TextBox txtName = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtName");
            TextBox txtISOCode2 = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtISOCode2");
            TextBox txtISOCode3 = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtISOCode3");
            GeoCountry country;
            if (guid == Guid.Empty)
            {
                country = new GeoCountry();
            }
            else
            {
                country = new GeoCountry(guid);
            }
            country.Name = txtName.Text;
            country.IsoCode2 = txtISOCode2.Text;
            country.IsoCode3 = txtISOCode3.Text;
            country.Save();

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        private void grdCountry_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            GeoCountry.Delete(guid);
            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        private void grdCountry_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        private void grdCountry_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.EditIndex = e.NewEditIndex;
            BindGrid();

            Button btnDelete = (Button)grid.Rows[e.NewEditIndex].Cells[0].FindControl("btnGridDelete");
            if (btnDelete != null)
            {
                btnDelete.Attributes.Add("OnClick", "return confirm('"
                    + Resource.CountryGridDeleteWarning + "');");
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("Name", typeof(String));
            dataTable.Columns.Add("ISOCode2", typeof(String));
            dataTable.Columns.Add("ISOCode3", typeof(String));
            dataTable.Columns.Add("TotalPages", typeof(int));

            DataRow row = dataTable.NewRow();
            row["Guid"] = Guid.Empty;
            row["Name"] = string.Empty;
            row["ISOCode2"] = string.Empty;
            row["ISOCode3"] = string.Empty;
            row["TotalPages"] = 1;
            dataTable.Rows.Add(row);

            btnAddNew.Visible = false;
            pgrCountry.Visible = false;
            grdCountry.EditIndex = 0;
            grdCountry.DataSource = dataTable;
            grdCountry.DataBind();

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.CountryAdministrationHeading);

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkCoreData.Text = Resource.CoreDataAdministrationLink;
            lnkCoreData.NavigateUrl = SiteRoot + "/Admin/CoreData.aspx";

            lnkCurrentPage.Text = Resource.CountryAdministrationLink;
            lnkCurrentPage.NavigateUrl = SiteRoot + "/Admin/AdminCountry.aspx";

            heading.Text = Resource.CountryAdministrationHeading;

            grdCountry.ToolTip = Resource.CountryAdministrationHeading;

            this.grdCountry.Columns[1].HeaderText = Resource.CountryGridNameHeader;
            this.grdCountry.Columns[2].HeaderText = Resource.CountryGridISOCode2Header;
            this.grdCountry.Columns[3].HeaderText = Resource.CountryGridISOCode3Header;
            btnAddNew.Text = Resource.CountryGridAddNewButton;

        }

        private void LoadSettings()
        {
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);

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

            this.grdCountry.Sorting += new GridViewSortEventHandler(grdCountry_Sorting);
            this.grdCountry.RowEditing += new GridViewEditEventHandler(grdCountry_RowEditing);
            this.grdCountry.RowCancelingEdit += new GridViewCancelEditEventHandler(grdCountry_RowCancelingEdit);
            this.grdCountry.RowUpdating += new GridViewUpdateEventHandler(grdCountry_RowUpdating);
            this.grdCountry.RowDeleting += new GridViewDeleteEventHandler(grdCountry_RowDeleting);
            this.btnAddNew.Click += new EventHandler(btnAddNew_Click);

            SuppressMenuSelection();
            SuppressPageMenu();

            ScriptConfig.IncludeJQTable = true;

        }

        #endregion
    }
}
