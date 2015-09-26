/// Author:					Joe Audette
/// Created:				2007-02-15
/// Last Modified:		    2012-10-02
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
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using mojoPortal.Business;
using WebStore.Business;
using WebStore.Helpers;

namespace WebStore.UI
{
    public partial class AdminDownloadTermsPage : NonCmsBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private string pageNumberParam;
        private string sortParam;
        private int pageNumber = 1;
        private int pageSize = 15;
        private int totalPages = 0;
        private string sort = "Name";
        private Store store;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();

            if ((store == null) || (!UserCanEditModule(moduleId, Store.FeatureGuid)))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            
            PopulateLabels();
            PopulateControls();
            AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsWebStoreSection", "store");

        }

        private void PopulateControls()
        {
            if (store == null) { return; }

            if (!Page.IsPostBack)
            {
                BindGrid();

            }

        }

       

        private void BindGrid()
        {
            if (store == null) { return; }

            btnAddNew.Visible = true;

            DataTable dt = FullfillDownloadTerms.GetPage(store.Guid, pageNumber, pageSize);

            if (dt.Rows.Count > 0)
            {
                totalPages = Convert.ToInt32(dt.Rows[0]["TotalPages"]);
            }

            DataView dv = dt.DefaultView;
            dv.Sort = sort;

            if (this.totalPages > 1)
            {

                string pageUrl = SiteRoot + "/WebStore/AdminDownloadTerms.aspx"
                    + "?pageid=" + pageId.ToInvariantString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "&amp;sort" + this.moduleId.ToInvariantString()
                    + "=" + this.sort
                    + "&amp;pagenumber" + this.moduleId.ToInvariantString()
                    + "={0}";

                pgrDownloadTerms.Visible = true;
                pgrDownloadTerms.PageURLFormat = pageUrl;
                pgrDownloadTerms.ShowFirstLast = true;
                pgrDownloadTerms.CurrentIndex = pageNumber;
                pgrDownloadTerms.PageSize = pageSize;
                pgrDownloadTerms.PageCount = totalPages;


            }
            else
            {
                pgrDownloadTerms.Visible = false;
            }

            grdFullfillDownloadTerms.DataSource = dv;
            grdFullfillDownloadTerms.PageIndex = pageNumber;
            grdFullfillDownloadTerms.PageSize = pageSize;
            grdFullfillDownloadTerms.DataBind();


        }

        private void grdFullfillDownloadTerms_Sorting(object sender, GridViewSortEventArgs e)
        {
           
      
            String redirectUrl = SiteRoot
                + "/WebStore/AdminDownloadTerms.aspx?pageid=" + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString()
                + "&pagenumber"
                + moduleId.ToInvariantString()
                + "=" + pageNumber.ToInvariantString()
                + "&sort"
                + moduleId.ToInvariantString()
                + "=" + e.SortExpression;

            WebUtils.SetupRedirect(this, redirectUrl);

        }

        private void grdFullfillDownloadTerms_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (store == null) { return; }

            GridView grid = (GridView)sender;
            Guid guid = (Guid)grid.DataKeys[e.RowIndex].Value;
            SiteUser siteUser = SiteUtils.GetCurrentSiteUser();

            TextBox txtName = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtName");
            TextBox txtDescription = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtDescription");
            TextBox txtDownloadsAllowed = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtDownloadsAllowed");
            TextBox txtExpireAfterDays = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtExpireAfterDays");
            CheckBox chkCountAfterDownload = (CheckBox)grid.Rows[e.RowIndex].Cells[1].FindControl("chkCountAfterDownload");

            FullfillDownloadTerms fullfillDownloadTerms;
            if (guid != Guid.Empty)
            {
                fullfillDownloadTerms = new FullfillDownloadTerms(guid);
            }
            else
            {
                fullfillDownloadTerms = new FullfillDownloadTerms();
                fullfillDownloadTerms.Created = DateTime.UtcNow;
                fullfillDownloadTerms.CreatedBy = siteUser.UserGuid;
                fullfillDownloadTerms.CreatedFromIP = SiteUtils.GetIP4Address();
            }
            
            fullfillDownloadTerms.StoreGuid = store.Guid;
            fullfillDownloadTerms.Name = txtName.Text;
            fullfillDownloadTerms.Description = txtDescription.Text;

            int downloadsAllowed;
            if (!int.TryParse(txtDownloadsAllowed.Text, out downloadsAllowed))
            {
                downloadsAllowed = 0;
            }
            fullfillDownloadTerms.DownloadsAllowed = downloadsAllowed;

            int expireAfterDays;
            if (!int.TryParse(txtExpireAfterDays.Text, out expireAfterDays))
            {
                expireAfterDays = 0;
            }
            fullfillDownloadTerms.ExpireAfterDays = expireAfterDays;

            fullfillDownloadTerms.CountAfterDownload = chkCountAfterDownload.Checked;
            fullfillDownloadTerms.LastModified = DateTime.UtcNow;
            fullfillDownloadTerms.LastModifedBy = siteUser.UserGuid;
            fullfillDownloadTerms.LastModifedFromIPAddress = SiteUtils.GetIP4Address();
            fullfillDownloadTerms.Save();

            WebUtils.SetupRedirect(this, GetRefreshUrl());

        }

        private void grdFullfillDownloadTerms_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            Guid guid = (Guid)grid.DataKeys[e.RowIndex].Value;
            FullfillDownloadTerms.Delete(guid);
            WebUtils.SetupRedirect(this, GetRefreshUrl());

        }

        private void grdFullfillDownloadTerms_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            WebUtils.SetupRedirect(this, GetRefreshUrl());
        }

        private void grdFullfillDownloadTerms_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.EditIndex = e.NewEditIndex;
            BindGrid();

            Button btnDelete = (Button)grid.Rows[e.NewEditIndex].Cells[0].FindControl("btnGridDelete");
            if (btnDelete != null)
            {
                btnDelete.Attributes.Add("OnClick", "return confirm('"
                    + WebStoreResources.DownloadTermsDeleteWarning + "');");
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("StoreGuid", typeof(Guid));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("DownloadsAllowed", typeof(int));
            dataTable.Columns.Add("ExpireAfterDays", typeof(int));
            dataTable.Columns.Add("CountAfterDownload", typeof(bool));
            dataTable.Columns.Add("Created", typeof(DateTime));
            dataTable.Columns.Add("CreatedBy", typeof(Guid));
            dataTable.Columns.Add("CreatedFromIP", typeof(string));
            dataTable.Columns.Add("LastModified", typeof(DateTime));
            dataTable.Columns.Add("LastModifedBy", typeof(Guid));
            dataTable.Columns.Add("LastModifedFromIPAddress", typeof(string));
            dataTable.Columns.Add("TotalPages", typeof(int));

            DataRow row = dataTable.NewRow();

            row["Guid"] = Guid.Empty;
            row["StoreGuid"] = store.Guid;
            row["Name"] = string.Empty;
            row["Description"] = string.Empty;
            row["DownloadsAllowed"] = 0;
            row["ExpireAfterDays"] = 0;
            row["CountAfterDownload"] = true;
            row["Created"] = DateTime.UtcNow;
            row["CreatedBy"] = Guid.Empty;
            row["CreatedFromIP"] = string.Empty;
            row["LastModified"] = DateTime.UtcNow;
            row["LastModifedBy"] = Guid.Empty;
            row["LastModifedFromIPAddress"] = string.Empty;
            row["TotalPages"] = 1;
            dataTable.Rows.Add(row);

            
            grdFullfillDownloadTerms.EditIndex = 0;
            grdFullfillDownloadTerms.DataSource = dataTable.DefaultView;
            grdFullfillDownloadTerms.DataBind();
            pgrDownloadTerms.Visible = false;
            btnAddNew.Visible = false;

        }


        private void PopulateLabels()
        {
            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs = crumbs.ItemWrapperTop + "<a href='" + SiteRoot
                    + "/WebStore/AdminDashboard.aspx?pageid=" + pageId.ToInvariantString() + "&mid=" + moduleId.ToInvariantString()
                    + "' class='unselectedcrumb'>" + WebStoreResources.StoreManagerLink
                    + "</a>" + crumbs.ItemWrapperBottom;
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.DownloadTermsAdministrationLink);
            heading.Text = WebStoreResources.DownloadTermsAdministrationLink;

            grdFullfillDownloadTerms.Columns[1].HeaderText = WebStoreResources.DownloadTermsGridNameHeader;

            btnAddNew.Text = WebStoreResources.DownloadTermsGridAddNewButton;

        }

        private void LoadSettings()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);

            store = StoreHelper.GetStore();
           
            pageNumberParam = "pagenumber" + this.moduleId.ToInvariantString();
            pageNumber = WebUtils.ParseInt32FromQueryString(pageNumberParam, 1);

            sortParam = "sort" + this.moduleId.ToInvariantString();

            if (Page.Request.Params[sortParam] != null)
            {
                sort = Page.Request.Params[sortParam];
            }

            AddClassToBody("webstore webstoredownloadterms");
        }

        private string GetRefreshUrl()
        {
           
            string refreshUrl = SiteRoot + "/WebStore/AdminDownloadTerms.aspx"
                    + "?pageid=" + pageId.ToInvariantString()
                    + "&mid=" + moduleId.ToInvariantString()
                    + "&sort" + this.moduleId.ToInvariantString()
                    + "=" + this.sort
                    + "&pagenumber" + this.moduleId.ToInvariantString()
                    + "=" + pageNumber.ToInvariantString();

            return refreshUrl;
        }

        

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.grdFullfillDownloadTerms.Sorting += new GridViewSortEventHandler(grdFullfillDownloadTerms_Sorting);
            this.grdFullfillDownloadTerms.RowEditing += new GridViewEditEventHandler(grdFullfillDownloadTerms_RowEditing);
            this.grdFullfillDownloadTerms.RowCancelingEdit += new GridViewCancelEditEventHandler(grdFullfillDownloadTerms_RowCancelingEdit);
            this.grdFullfillDownloadTerms.RowUpdating += new GridViewUpdateEventHandler(grdFullfillDownloadTerms_RowUpdating);
            this.grdFullfillDownloadTerms.RowDeleting += new GridViewDeleteEventHandler(grdFullfillDownloadTerms_RowDeleting);
            this.btnAddNew.Click += new EventHandler(btnAddNew_Click);
           

            SuppressPageMenu();
            ScriptConfig.IncludeJQTable = true;
        }

        #endregion

    }
}
