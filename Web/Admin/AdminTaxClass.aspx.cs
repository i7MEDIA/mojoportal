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
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class AdminTaxClassPage : NonCmsBasePage
    {
        private int pageNumber = 1;
        private int pageSize = 15;
        private int totalPages = 0;
        private string sort = "Title";

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
            
            BindGrid();
            
        }

        private void BindGrid()
        {
            using (IDataReader reader = TaxClass.GetPage(
                siteSettings.SiteGuid,
                pageNumber,
                pageSize,
                out totalPages))
            {
                if (this.totalPages > 1)
                {
                    string pageUrl = SiteRoot + "/Admin/AdminTaxClass.aspx?pagenumber={0}&amp;sort=" + this.sort;

                    pgrTaxClass.Visible = true;
                    pgrTaxClass.PageURLFormat = pageUrl;
                    pgrTaxClass.ShowFirstLast = true;
                    pgrTaxClass.CurrentIndex = pageNumber;
                    pgrTaxClass.PageSize = pageSize;
                    pgrTaxClass.PageCount = totalPages;

                }
                else
                {
                    pgrTaxClass.Visible = false;
                }

                grdTaxClass.DataSource = reader;
                grdTaxClass.PageIndex = pageNumber;
                grdTaxClass.PageSize = pageSize;
                grdTaxClass.DataBind();
            }

            EnsureTaxClasses();
        }

        private void grdTaxClass_Sorting(object sender, GridViewSortEventArgs e)
        {

            String redirectUrl = SiteRoot
                + "/Admin/AdminTaxClass.aspx?pagenumber=" + pageNumber.ToString(CultureInfo.InvariantCulture)
                + "&sort=" + e.SortExpression;

            WebUtils.SetupRedirect(this, redirectUrl);

        }

        private void grdTaxClass_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grid = (GridView)sender;
            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());

            TextBox txtTitle = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtTitle");
            TextBox txtDescription = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtDescription");
            TaxClass taxClass;
            if (guid != Guid.Empty)
            {
                taxClass = new TaxClass(guid);
            }
            else
            {
                taxClass = new TaxClass();
                taxClass.Created = DateTime.UtcNow;
            }

            taxClass.SiteGuid = siteSettings.SiteGuid;
            taxClass.Title = txtTitle.Text;
            taxClass.Description = txtDescription.Text;
            taxClass.LastModified = DateTime.UtcNow;
            taxClass.Save();

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        private void grdTaxClass_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            TaxClass.Delete(guid);
            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        private void grdTaxClass_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        private void grdTaxClass_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.EditIndex = e.NewEditIndex;
            BindGrid();

            Button btnDelete = (Button)grid.Rows[e.NewEditIndex].Cells[0].FindControl("btnGridDelete");
            if (btnDelete != null)
            {
                btnDelete.Attributes.Add("OnClick", "return confirm('"
                    + Resource.TaxClassGridDeleteWarning + "');");
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("SiteGuid", typeof(Guid));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("LastModified", typeof(DateTime));
            dataTable.Columns.Add("Created", typeof(DateTime));
            dataTable.Columns.Add("TotalPages", typeof(int));

            DataRow row = dataTable.NewRow();

            row["Guid"] = Guid.Empty;
            row["SiteGuid"] = siteSettings.SiteGuid;
            row["Title"] = string.Empty;
            row["Description"] = string.Empty;
            row["LastModified"] = DateTime.UtcNow;
            row["Created"] = DateTime.UtcNow;
            row["TotalPages"] = 1;
            dataTable.Rows.Add(row);


            grdTaxClass.EditIndex = 0;
            grdTaxClass.DataSource = dataTable.DefaultView;
            grdTaxClass.DataBind();

            pgrTaxClass.Visible = false;
            btnAddNew.Visible = false;

        }

        private void EnsureTaxClasses()
        {
            if (grdTaxClass.Rows.Count == 0)
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

                WebUtils.SetupRedirect(this, Request.RawUrl);

            }

        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.TaxClassAdminHeading);

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkCoreData.Text = Resource.CoreDataAdministrationLink;
            lnkCoreData.NavigateUrl = SiteRoot + "/Admin/CoreData.aspx";

            lnkCurrentPage.Text = Resource.TaxClassAdminLink;
            lnkCurrentPage.NavigateUrl = SiteRoot + "/Admin/AdminTaxClass.aspx";

            heading.Text = Resource.TaxClassAdminHeading;

            grdTaxClass.Columns[1].HeaderText = Resource.TaxClassGridTitleLabel;

            btnAddNew.Text = Resource.TaxClassGridAddNewButton;

        }

        private void LoadSettings()
        {
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);

            AddClassToBody("administration");
            AddClassToBody("taxadmin");

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.grdTaxClass.Sorting += new GridViewSortEventHandler(grdTaxClass_Sorting);
            this.grdTaxClass.RowEditing += new GridViewEditEventHandler(grdTaxClass_RowEditing);
            this.grdTaxClass.RowCancelingEdit += new GridViewCancelEditEventHandler(grdTaxClass_RowCancelingEdit);
            this.grdTaxClass.RowUpdating += new GridViewUpdateEventHandler(grdTaxClass_RowUpdating);
            this.grdTaxClass.RowDeleting += new GridViewDeleteEventHandler(grdTaxClass_RowDeleting);
            this.btnAddNew.Click += new EventHandler(btnAddNew_Click);

            SuppressMenuSelection();
            SuppressPageMenu();
            ScriptConfig.IncludeJQTable = true;


        }

        #endregion
    }
}
