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
using WebStore.Business;
using WebStore.Helpers;

namespace WebStore.UI
{
    public partial class AdminProductPage : NonCmsBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private string pageNumberParam;
        private string sortParam;
        private int pageNumber = 1;
        private int pageSize = 10;
        private int totalPages = 0;
        private string sort = "Name";
        private Store store;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();

            if ((!UserCanEditModule(moduleId, Store.FeatureGuid))||(store == null))
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
            if (!Page.IsPostBack)
            {
                BindGrid();
            }

        }

        

        private void BindGrid()
        {
            if (store == null) { return; }
            

            DataTable dt = Product.GetPageForAdminList(
                store.Guid,
                pageNumber,
                pageSize,
                out totalPages);

            //if (dt.Rows.Count > 0)
            //{
            //    totalPages = Convert.ToInt32(dt.Rows[0]["TotalPages"]);
            //}

            DataView dv = dt.DefaultView;
            dv.Sort = sort;

            if (this.totalPages > 1)
            {
                string pageUrl = WebUtils.GetSiteRoot() + "/WebStore/AdminProduct.aspx"
                    + "?pageid=" + pageId.ToInvariantString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "&amp;sort" + this.moduleId.ToInvariantString()
                    + "=" + this.sort
                    + "&amp;pagenumber" + this.moduleId.ToInvariantString()
                    + "={0}";

                pgrProduct.Visible = true;
                pgrProduct.PageURLFormat = pageUrl;
                pgrProduct.ShowFirstLast = true;
                pgrProduct.CurrentIndex = pageNumber;
                pgrProduct.PageSize = pageSize;
                pgrProduct.PageCount = totalPages;

            }
            else
            {
                pgrProduct.Visible = false;
            }

            grdProduct.DataSource = dv;
            grdProduct.PageIndex = pageNumber;
            grdProduct.PageSize = pageSize;
            grdProduct.DataBind();

        }

        private void grdProduct_Sorting(object sender, GridViewSortEventArgs e)
        {

            String redirectUrl = WebUtils.GetSiteRoot()
                + "/WebStore/AdminProduct.aspx?pageid=" + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString()
                + "&pagenumber"
                + moduleId.ToInvariantString()
                + "=" + pageNumber.ToInvariantString()
                + "&sort"
                + moduleId.ToInvariantString()
                + "=" + e.SortExpression;

            WebUtils.SetupRedirect(this, redirectUrl);

        }

        //private void btnAddNew_Click(object sender, EventArgs e)
        //{
        //    String redirectUrl = WebUtils.GetSiteRoot()
        //        + "/WebStore/AdminProductEdit.aspx?pageid=" + pageId.ToString(CultureInfo.InvariantCulture)
        //        + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture);

        //    WebUtils.SetupRedirect(this, redirectUrl);
        //}


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

            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.ProductAdministrationLink);
            heading.Text = WebStoreResources.ProductAdministrationLink;

            

            grdProduct.Columns[1].HeaderText = WebStoreResources.ProductModelNumberLabel;
            grdProduct.Columns[2].HeaderText = WebStoreResources.ProductNameLabel;

            lnkNewProduct.Text = WebStoreResources.ProductGridAddNewButton;

        }

        private void LoadSettings()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);

            store = StoreHelper.GetStore();
            if (store == null) { return; }


            pageNumberParam = "pagenumber" + this.moduleId.ToInvariantString();
            pageNumber = WebUtils.ParseInt32FromQueryString(pageNumberParam, 1);

            sortParam = "sort" + this.moduleId.ToInvariantString();

            if (Request.Params[sortParam] != null)
            {
                sort = Request.Params[sortParam];
            }

            lnkNewProduct.NavigateUrl = SiteRoot +"/WebStore/AdminProductEdit.aspx?pageid=" + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString();

            AddClassToBody("webstore webstoreadminproduct");

        }

        protected string BuildProductQueryString(string productID)
        {

            string result = "?pageid=" + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString()
                + "&prod=" + productID;

            return result;

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
            this.grdProduct.Sorting += new GridViewSortEventHandler(grdProduct_Sorting);
            //this.btnAddNew.Click += new EventHandler(btnAddNew_Click);
            
            SuppressPageMenu();

            ScriptConfig.IncludeJQTable = true;
        }

        #endregion

    }
}
