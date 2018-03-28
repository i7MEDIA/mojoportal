/// Author:					
/// Created:				2008-06-26
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
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class AdminTaxRatePage : NonCmsBasePage
    {
        private string sort = "Description";
        
        // usa a71d6727-61e7-4282-9fcb-526d1e7bc24f
        private Guid countryGuid = new Guid("a71d6727-61e7-4282-9fcb-526d1e7bc24f");
        private Guid geoZoneGuid = Guid.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}

			LoadSettings();
            if (!WebUser.IsAdminOrContentAdmin)
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
            BindGeoZoneList();

            if ((geoZoneGuid == Guid.Empty)&&(ddGeoZones.SelectedValue.Length == 36))
            {
                geoZoneGuid = new Guid(ddGeoZones.SelectedValue);
            }

            BindGrid();
            
        }

        private void BindCountryList()
        {
            DataTable dataTable = GeoCountry.GetList();
            ddCountry.DataSource = dataTable;
            ddCountry.DataBind();

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

        private void BindGeoZoneList()
        {
            if (ddCountry.SelectedIndex > -1)
            {
                countryGuid = new Guid(ddCountry.SelectedValue);
                using (IDataReader reader = GeoZone.GetByCountry(countryGuid))
                {
                    ddGeoZones.DataSource = reader;
                    ddGeoZones.DataBind();
                }

                //if ((!Page.IsPostBack)
                //&& (geoZoneGuid != Guid.Empty)
                //)
                //{
                
                //}

            }

            ListItem listItem = ddGeoZones.Items.FindByValue(geoZoneGuid.ToString());
            if (listItem != null)
            {
                ddGeoZones.ClearSelection();
                listItem.Selected = true;

            }
            else
            {
                // this is needed because the guid string may be upper case
                // it is in firebird db
                listItem = ddGeoZones.Items.FindByValue(geoZoneGuid.ToString().ToUpper());
                if (listItem != null)
                {
                    ddGeoZones.ClearSelection();
                    listItem.Selected = true;

                }

            }

        }

        private void ddCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            countryGuid = new Guid(ddCountry.SelectedValue);
            geoZoneGuid = Guid.Empty;
            BindGeoZoneList();
            BindGrid();


        }

        private void ddGeoZones_SelectedIndexChanged(object sender, EventArgs e)
        {
            geoZoneGuid = new Guid(ddGeoZones.SelectedValue);
            if (ddCountry.SelectedIndex > -1)
            {
                countryGuid = new Guid(ddCountry.SelectedValue);
            }
            BindGrid();

        }



        private void BindGrid()
        {
            DataTable dt = TaxRate.GetAll(siteSettings.SiteGuid, geoZoneGuid);
            DataView dv = dt.DefaultView;
            dv.Sort = sort;
            grdTaxRate.DataSource = dv;
            grdTaxRate.DataBind();

        }

        private void grdTaxRate_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grid = (GridView)sender;
            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            SiteUser siteUser = SiteUtils.GetCurrentSiteUser();

            if (ddGeoZones.SelectedIndex > -1)
            {
                geoZoneGuid = new Guid(ddGeoZones.SelectedValue);
            }

            TextBox txtDescription = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtDescription");
            DropDownList ddTaxClass = (DropDownList)grid.Rows[e.RowIndex].Cells[1].FindControl("ddTaxClass");
            TextBox txtPriority = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtPriority");
            TextBox txtRate = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtRate");

            TaxRate taxRate;

            if (guid != Guid.Empty)
            {
                taxRate = new TaxRate(guid);
            }
            else
            {
                taxRate = new TaxRate(siteSettings.SiteGuid, geoZoneGuid);
                taxRate.Created = DateTime.UtcNow;
                taxRate.CreatedBy = siteUser.UserGuid;
            }

            taxRate.Description = txtDescription.Text;
            taxRate.GeoZoneGuid = geoZoneGuid;
            Guid taxClassGuid = new Guid(ddTaxClass.SelectedValue);
            taxRate.TaxClassGuid = taxClassGuid;

            int priority;
            if (!int.TryParse(txtPriority.Text, out priority))
            {
                priority = 1;
            }
            taxRate.Priority = priority;

            decimal rate;
            if (!decimal.TryParse(txtRate.Text, out rate))
            {
                rate = 0;
            }

            taxRate.Rate = rate;
            taxRate.LastModified = DateTime.UtcNow;
            taxRate.ModifiedBy = siteUser.UserGuid;
            taxRate.Save();

            WebUtils.SetupRedirect(this, GetRefreshUrl());

        }

        private void grdTaxRate_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            TaxRate.Delete(guid);
            WebUtils.SetupRedirect(this, GetRefreshUrl());

        }

        private void grdTaxRate_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        private void grdTaxRate_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (ddGeoZones.SelectedIndex > -1)
            {
                geoZoneGuid = new Guid(ddGeoZones.SelectedValue);
            }

            GridView grid = (GridView)sender;
            grid.EditIndex = e.NewEditIndex;
            Guid guid = new Guid(grid.DataKeys[grid.EditIndex].Value.ToString());
            BindGrid();

            Button btnDelete = (Button)grid.Rows[e.NewEditIndex].Cells[0].FindControl("btnGridDelete");
            if (btnDelete != null)
            {
                btnDelete.Attributes.Add("OnClick", "return confirm('"
                    + Resource.TaxRateGridDeleteWarning + "');");
            }

            DropDownList ddTaxClass = (DropDownList)grid.Rows[grid.EditIndex].Cells[0].FindControl("ddTaxClass");
            if (ddTaxClass != null)
            {
                using (IDataReader reader = TaxClass.GetBySite(siteSettings.SiteGuid))
                {
                    ddTaxClass.DataSource = reader;
                    ddTaxClass.DataBind();
                }

                if (guid != Guid.Empty)
                {
                    TaxRate taxRate = new TaxRate(guid);
                    ListItem listItem = ddTaxClass.Items.FindByValue(taxRate.TaxClassGuid.ToString());
                    if (listItem != null)
                    {
                        ddTaxClass.ClearSelection();
                        listItem.Selected = true;
                    }
                }
            }

            ddCountry.Enabled = false;
            ddGeoZones.Enabled = false;
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            if (ddGeoZones.SelectedIndex > -1)
            {
                geoZoneGuid = new Guid(ddGeoZones.SelectedValue);
            }

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("SiteGuid", typeof(Guid));
            dataTable.Columns.Add("GeoZoneGuid", typeof(Guid));
            dataTable.Columns.Add("TaxClassGuid", typeof(Guid));
            dataTable.Columns.Add("Priority", typeof(int));
            dataTable.Columns.Add("Rate", typeof(decimal));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("Created", typeof(DateTime));
            dataTable.Columns.Add("CreatedBy", typeof(Guid));
            dataTable.Columns.Add("LastModified", typeof(DateTime));
            dataTable.Columns.Add("ModifiedBy", typeof(Guid));

            DataRow row = dataTable.NewRow();
            row["Guid"] = Guid.Empty;
            row["SiteGuid"] = siteSettings.SiteGuid;
            row["GeoZoneGuid"] = geoZoneGuid;
            row["TaxClassGuid"] = Guid.Empty;
            row["Priority"] = 1;
            row["Rate"] = 0;
            row["Description"] = string.Empty;
            row["Created"] = DateTime.UtcNow;
            row["CreatedBy"] = Guid.Empty;
            row["LastModified"] = DateTime.UtcNow;
            row["ModifiedBy"] = Guid.Empty;
            dataTable.Rows.Add(row);

            grdTaxRate.EditIndex = 0;
            grdTaxRate.DataSource = dataTable.DefaultView;
            grdTaxRate.DataBind();

            btnAddNew.Visible = false;

            Button btnDelete = (Button)grdTaxRate.Rows[grdTaxRate.EditIndex].Cells[1].FindControl("btnGridDelete");
            if (btnDelete != null)
            {
                btnDelete.Visible = false;
            }

            DropDownList ddTaxClass = (DropDownList)grdTaxRate.Rows[grdTaxRate.EditIndex].Cells[1].FindControl("ddTaxClass");
            if (ddTaxClass != null)
            {
                using (IDataReader reader = TaxClass.GetBySite(siteSettings.SiteGuid))
                {
                    ddTaxClass.DataSource = reader;
                    ddTaxClass.DataBind();
                }

                if (ddTaxClass.Items.Count == 0)
                {
                    TaxClass taxClass = new TaxClass();
                    taxClass.SiteGuid = siteSettings.SiteGuid;
                    taxClass.Title = Resource.TaxClassTaxable;
                    taxClass.Description = Resource.TaxClassTaxable;
                    taxClass.Save();

                    taxClass = new TaxClass();
                    taxClass.SiteGuid = siteSettings.SiteGuid;
                    taxClass.Title = Resource.TaxClassNotTaxable;
                    taxClass.Description = Resource.TaxClassNotTaxable;
                    taxClass.Save();

                    using (IDataReader reader = TaxClass.GetBySite(siteSettings.SiteGuid))
                    {
                        ddTaxClass.DataSource = reader;
                        ddTaxClass.DataBind();
                    }

                }

            }

            ddCountry.Enabled = false;
            ddGeoZones.Enabled = false;

        }

        private string GetRefreshUrl()
        {
            if (ddCountry.SelectedIndex > -1)
            {
                countryGuid = new Guid(ddCountry.SelectedValue);
            }

            if (ddGeoZones.SelectedIndex > -1)
            {
                geoZoneGuid = new Guid(ddGeoZones.SelectedValue);
            }

            string refreshUrl = SiteRoot + "/Admin/AdminTaxRate.aspx?country=" + countryGuid.ToString()
                    + "&zone=" + geoZoneGuid.ToString();

            return refreshUrl;
        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.TaxRateAdminHeading);

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkCoreData.Text = Resource.CoreDataAdministrationLink;
            lnkCoreData.NavigateUrl = SiteRoot + "/Admin/CoreData.aspx";

            lnkCurrentPage.Text = Resource.TaxRateAdminLink;
            lnkCurrentPage.NavigateUrl = SiteRoot + "/Admin/AdminTaxRate.aspx";




            heading.Text = Resource.TaxRateAdminHeading;

            grdTaxRate.Columns[1].HeaderText = Resource.TaxRateGridDescriptionHeader;
            grdTaxRate.Columns[2].HeaderText = Resource.TaxRateGridRateHeader;

            btnAddNew.Text = Resource.TaxRateGridAddNewButton;
        }

        private void LoadSettings()
        {
            countryGuid = WebUtils.ParseGuidFromQueryString("country", countryGuid);
            geoZoneGuid = WebUtils.ParseGuidFromQueryString("zone", geoZoneGuid);

            
            ddCountry.Enabled = true;
            ddGeoZones.Enabled = true;

            AddClassToBody("administration");
            AddClassToBody("taxadmin");

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.grdTaxRate.RowEditing += new GridViewEditEventHandler(grdTaxRate_RowEditing);
            this.grdTaxRate.RowCancelingEdit += new GridViewCancelEditEventHandler(grdTaxRate_RowCancelingEdit);
            this.grdTaxRate.RowUpdating += new GridViewUpdateEventHandler(grdTaxRate_RowUpdating);
            this.grdTaxRate.RowDeleting += new GridViewDeleteEventHandler(grdTaxRate_RowDeleting);
            this.btnAddNew.Click += new EventHandler(btnAddNew_Click);
            this.ddCountry.SelectedIndexChanged += new EventHandler(ddCountry_SelectedIndexChanged);
            this.ddGeoZones.SelectedIndexChanged += new EventHandler(ddGeoZones_SelectedIndexChanged);


            SuppressMenuSelection();
            SuppressPageMenu();

            ScriptConfig.IncludeJQTable = true;

        }

        #endregion
    }
}
