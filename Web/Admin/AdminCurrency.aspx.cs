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
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class AdminCurrencyPage : NonCmsBasePage
    {
        //private int pageNumber = 1;
        //private int pageSize = 20;
        //private int totalPages = 0;

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
                SiteUtils.RedirectToAccessDeniedPage(this);
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
            DataTable dt = Currency.GetAll();
            

            grdCurrency.DataSource = dt;
            grdCurrency.DataBind();
        }

        private void grdCurrency_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grid = (GridView)sender;

            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            TextBox txtTitle = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtTitle");
            TextBox txtCode = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtCode");
            //TextBox txtSymbolLeft = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtSymbolLeft");
            //TextBox txtSymbolRight = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtSymbolRight");
            //TextBox txtDecimalPointChar = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtDecimalPointChar");
            //TextBox txtThousandsPointChar = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtThousandsPointChar");
            //TextBox txtDecimalPlaces = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtDecimalPlaces");
            //TextBox txtValue = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtValue");

            Currency currency;
            if (guid != Guid.Empty)
            {
                currency = new Currency(guid);
            }
            else
            {
                currency = new Currency();
            }
            currency.Title = txtTitle.Text;
            currency.Code = txtCode.Text;
            //currency.SymbolLeft = txtSymbolLeft.Text;
            //currency.SymbolRight = txtSymbolRight.Text;
            //currency.DecimalPointChar = txtDecimalPointChar.Text;
            //currency.ThousandsPointChar = txtThousandsPointChar.Text;
            //currency.DecimalPlaces = txtDecimalPlaces.Text;
            //decimal d;
            //if (!decimal.TryParse(txtValue.Text, out d)) d = 1;
            //currency.Value = d;
            currency.LastModified = DateTime.UtcNow;
            if (currency.Guid == Guid.Empty)
            {
                currency.Created = DateTime.UtcNow;
            }
            currency.Save();

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        private void grdCurrency_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            Currency.Delete(guid);
            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        private void grdCurrency_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        private void grdCurrency_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.EditIndex = e.NewEditIndex;
            grid.ShowHeader = false;
            grid.BorderStyle = BorderStyle.None;
            grid.GridLines = GridLines.None;
            BindGrid();

            Button btnDelete = (Button)grid.Rows[e.NewEditIndex].Cells[0].FindControl("btnGridDelete");
            if (btnDelete != null)
            {
                btnDelete.Attributes.Add("OnClick", "return confirm('"
                    + Resource.CurrencyGridDeleteWarning + "');");
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Code", typeof(string));
            //dataTable.Columns.Add("SymbolLeft", typeof(string));
            //dataTable.Columns.Add("SymbolRight", typeof(string));
            //dataTable.Columns.Add("DecimalPointChar", typeof(string));
            //dataTable.Columns.Add("ThousandsPointChar", typeof(string));
            //dataTable.Columns.Add("DecimalPlaces", typeof(string));
            //dataTable.Columns.Add("Value", typeof(decimal));
            dataTable.Columns.Add("LastModified", typeof(DateTime));
            dataTable.Columns.Add("Created", typeof(DateTime));
            dataTable.Columns.Add("TotalPages", typeof(int));

            DataRow row = dataTable.NewRow();
            row["Guid"] = Guid.Empty;
            row["Title"] = string.Empty;
            row["Code"] = string.Empty;
            //row["SymbolLeft"] = string.Empty;
            //row["SymbolRight"] = string.Empty;
            //row["DecimalPointChar"] = '.';
            //row["ThousandsPointChar"] = ',';
            //row["DecimalPlaces"] = '2';
            //row["Value"] = 1;
            row["LastModified"] = DateTime.UtcNow;
            row["Created"] = DateTime.UtcNow;
            dataTable.Rows.Add(row);

            grdCurrency.EditIndex = 0;
            grdCurrency.DataSource = dataTable.DefaultView;
            grdCurrency.DataBind();

            btnAddNew.Visible = false;

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.CurrencyAdministrationHeading);

            if (!Page.IsPostBack)
            {
                grdCurrency.Columns[1].HeaderText = Resource.CurrencyGridTitleHeading;
            }

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkCoreData.Text = Resource.CoreDataAdministrationLink;
            lnkCoreData.NavigateUrl = SiteRoot + "/Admin/CoreData.aspx";

            lnkCurrentPage.Text = Resource.CurrencyAdministrationLink;
            lnkCurrentPage.NavigateUrl = SiteRoot + "/Admin/AdminCurrency.aspx";

            heading.Text = Resource.CurrencyAdministrationHeading;
            btnAddNew.Text = Resource.CurrencyGridAddNewButton;

        }

        private void LoadSettings()
        {
            //pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);

            AddClassToBody("administration");
            AddClassToBody("currencyadmin");

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.grdCurrency.RowEditing += new GridViewEditEventHandler(grdCurrency_RowEditing);
            this.grdCurrency.RowCancelingEdit += new GridViewCancelEditEventHandler(grdCurrency_RowCancelingEdit);
            this.grdCurrency.RowUpdating += new GridViewUpdateEventHandler(grdCurrency_RowUpdating);
            this.grdCurrency.RowDeleting += new GridViewDeleteEventHandler(grdCurrency_RowDeleting);
            this.btnAddNew.Click += new EventHandler(btnAddNew_Click);

            SuppressMenuSelection();
            SuppressPageMenu();
            ScriptConfig.IncludeJQTable = true;


        }

        #endregion
    }
}
