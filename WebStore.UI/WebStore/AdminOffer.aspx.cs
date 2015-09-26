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
    public partial class AdminOfferPage : NonCmsBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private string pageNumberParam;
        //private string sortParam;
        private int pageNumber = 1;
        private int pageSize = 15;
        private int totalPages = 0;
        //private string sort = "Name";
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
            if (store == null) { return; }

            if (!Page.IsPostBack)
            {
                BindGrid();
            }

        }

        

        private void BindGrid()
        {
            if (store == null) { return; }

            using (IDataReader reader = Offer.GetPage(store.Guid, pageNumber, pageSize, out totalPages))
            {
               
                string pageUrl = WebUtils.GetSiteRoot() + "/WebStore/AdminOffer.aspx"
                    + "?pageid=" + pageId.ToInvariantString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "&amp;pagenumber" + moduleId.ToInvariantString()
                    + "={0}";

                pgrOffer.PageURLFormat = pageUrl;
                pgrOffer.ShowFirstLast = true;
                pgrOffer.CurrentIndex = pageNumber;
                pgrOffer.PageSize = pageSize;
                pgrOffer.PageCount = totalPages;
                pgrOffer.Visible = (totalPages > 1);

                grdOffer.DataSource = reader;
                grdOffer.PageIndex = pageNumber;
                grdOffer.PageSize = pageSize;
                grdOffer.DataBind();
            }

        }

        //private void grdOffer_Sorting(object sender, GridViewSortEventArgs e)
        //{
      
        //    String redirectUrl = WebUtils.GetSiteRoot()
        //        + "/WebStore/AdminOffer.aspx?pageid=" + pageId.ToString(CultureInfo.InvariantCulture)
        //        + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
        //        + "&pagenumber"
        //        + moduleId.ToString(CultureInfo.InvariantCulture)
        //        + "=" + pageNumber.ToString(CultureInfo.InvariantCulture)
        //        + "&sort"
        //        + moduleId.ToString(CultureInfo.InvariantCulture)
        //        + "=" + e.SortExpression;

        //    WebUtils.SetupRedirect(this, redirectUrl);

        //}

        //private void btnAddNew_Click(object sender, EventArgs e)
        //{
        //    String redirectUrl = SiteRoot
        //        + "/WebStore/AdminOfferEdit.aspx?pageid=" + pageId.ToString(CultureInfo.InvariantCulture)
        //        + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture);

        //    WebUtils.SetupRedirect(this, redirectUrl);
        //}

        protected string BuildOfferQueryString(string offerID)
        {

            string result = "?pageid=" + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString()
                + "&offer=" + offerID;

            return result;

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

            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.OfferAdministrationLink);
            heading.Text = WebStoreResources.OfferAdministrationLink;

           
            grdOffer.Columns[1].HeaderText = WebStoreResources.OfferNameLabel;
            grdOffer.Columns[2].HeaderText = WebStoreResources.OfferIsVisibleLabel;
            grdOffer.Columns[3].HeaderText = WebStoreResources.OfferIsSpecialLabel;


            lnkAddNew.Text = WebStoreResources.OfferAddNewButton;
            

        }

        private void LoadSettings()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);

            store = StoreHelper.GetStore();
            
            pageNumberParam = "pagenumber" + this.moduleId.ToInvariantString();
            pageNumber = WebUtils.ParseInt32FromQueryString(pageNumberParam, 1);

            lnkAddNew.NavigateUrl = SiteRoot
                + "/WebStore/AdminOfferEdit.aspx?pageid=" + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString();

            AddClassToBody("webstore adminoffer");

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
            //this.grdOffer.Sorting += new GridViewSortEventHandler(grdOffer_Sorting);
            
            //this.btnAddNew.Click += new EventHandler(btnAddNew_Click);

            SuppressPageMenu();
            ScriptConfig.IncludeJQTable = true;
        }

        #endregion

    }
}
