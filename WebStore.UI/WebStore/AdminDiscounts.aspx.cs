// Author:					Joe Audette
// Created:					2009-03-03
// Last Modified:			2012-10-02
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
using System.Globalization;
using System.Web.UI;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using WebStore.Business;
using WebStore.Helpers;

namespace WebStore.UI
{

    public partial class AdminDiscountsPage : NonCmsBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private Store store = null;
        private int pageNumber = 1;
        private int pageSize = 15;
        private int totalPages = 1;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParams();

            if (!UserCanEditModule(moduleId, Store.FeatureGuid))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();
            if (store == null)
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
            BindGrid();

        }

        private void BindGrid()
        {
            if (store == null) { return; }

            using (IDataReader reader = Discount.GetPage(store.ModuleGuid, pageNumber, pageSize, out totalPages))
            {
                if (totalPages > 1)
                {

                    string pageUrl = SiteRoot + "/WebStore/AdminDiscounts.aspx"
                        + "?pageid=" + pageId.ToInvariantString()
                        + "&amp;mid=" + moduleId.ToInvariantString()
                        + "&amp;pagenumber" + this.moduleId.ToInvariantString()
                        + "={0}";

                    pgrDiscounts.Visible = true;
                    pgrDiscounts.PageURLFormat = pageUrl;
                    pgrDiscounts.ShowFirstLast = true;
                    pgrDiscounts.CurrentIndex = pageNumber;
                    pgrDiscounts.PageSize = pageSize;
                    pgrDiscounts.PageCount = totalPages;

                }
                else
                {
                    pgrDiscounts.Visible = false;
                }

                grdDiscount.DataSource = reader;
                grdDiscount.PageIndex = pageNumber;
                grdDiscount.PageSize = pageSize;
                grdDiscount.DataBind();
            }

        }


        protected string BuildQueryString(string discountGuid)
        {

            string result = "?pageid=" + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString()
                + "&d=" + discountGuid;

            return result;

        }

        private void PopulateLabels()
        {
            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs = crumbs.ItemWrapperTop + "<a href='"
                    + SiteRoot
                    + "/WebStore/AdminDashboard.aspx?pageid=" + pageId.ToInvariantString()
                    + "&mid=" + moduleId.ToInvariantString()
                    + "' class='unselectedcrumb'>" + WebStoreResources.StoreManagerLink
                    + "</a>" + crumbs.ItemWrapperBottom;
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.DiscountAdministration);
            heading.Text = WebStoreResources.DiscountAdministration;
            lnkNewDiscount.Text = WebStoreResources.NewDiscountLink;

            grdDiscount.Columns[0].HeaderText = " ";
            grdDiscount.Columns[1].HeaderText = WebStoreResources.DiscountDescriptionHeading;
            grdDiscount.Columns[2].HeaderText = WebStoreResources.DiscountCodeHeading;

            
        }

        private void LoadSettings()
        {
            store = StoreHelper.GetStore();
          
            lnkNewDiscount.NavigateUrl
                = SiteRoot
                + "/WebStore/AdminDiscountEdit.aspx?pageid=" 
                + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString();

            AddClassToBody("webstore webstorediscounts");
        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);

        }


        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            ScriptConfig.IncludeJQTable = true;

        }

        #endregion
    }
}

