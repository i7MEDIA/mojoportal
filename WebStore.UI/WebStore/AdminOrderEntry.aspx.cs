// Author:					Joe Audette
// Created:					2009-07-27
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
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using WebStore.Business;
using WebStore.Helpers;
using Resources;

namespace WebStore.UI
{

    public partial class AdminOrderEntryPage : NonCmsBasePage
    {
        protected int pageId = -1;
        protected int moduleId = -1;
        protected int pageNumber = 1;
        private int totalPages = 1;
        private int pageSize = 10;
        private Store store = null;
        private Cart cart = null;
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        private DataSet dsProducts = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (SiteUtils.SslIsAvailable()) { SiteUtils.ForceSsl(); }

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
            

            if(!Page.IsPostBack)
            {
                BindProducts();
                BindCart();
            }
        }

        private void BindProducts()
        {


            dsProducts = store.GetProductPageWithOffers(
                pageNumber,
                pageSize,
                out totalPages);

            
            string pageUrl = SiteRoot + "/WebStore/AdminOrderEntry.aspx"
                + "?pageid=" + pageId.ToInvariantString()
                + "&amp;mid=" + moduleId.ToInvariantString()
                + "&amp;pagenumber={0}";

            pgr.PageURLFormat = pageUrl;
            pgr.ShowFirstLast = true;
            pgr.CurrentIndex = pageNumber;
            pgr.PageSize = pageSize;
            pgr.PageCount = totalPages;
            pgr.Visible = (this.totalPages > 1);

           
            rptProducts.DataSource = dsProducts.Tables["Products"];
            rptProducts.DataBind();


        }

        protected void rptOffers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if ((store != null)&&(cart != null)&&(e.CommandName == "AddToCart"))
            {
                Guid offerGuid = Guid.Empty;
                int qtyOrdered = 1;
                string offerG = e.CommandArgument.ToString();
                if (offerG.Length == 36) { offerGuid = new Guid(offerG); }

                TextBox txtQuantity = (TextBox)e.Item.FindControl("txtQuantity");

                if ((txtQuantity != null) && (txtQuantity.Text.Length > 0))
                {
                    int.TryParse(txtQuantity.Text, out qtyOrdered);
                }

                if (offerGuid != Guid.Empty)
                {
                    Offer offer = new Offer(offerGuid);
                    if (offer.StoreGuid == store.Guid)
                    {
                        cart.AddOfferToCart(offer, qtyOrdered);
                    }
                }



            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        void rptProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (dsProducts == null) { return; }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string productGuid = ((DataRowView)e.Item.DataItem).Row.ItemArray[0].ToString();
                Repeater rptOffers = (Repeater)e.Item.FindControl("rptOffers");

                if (rptOffers == null) { return; }

                string whereClause = string.Format("ProductGuid = '{0}'", productGuid);
                DataView dv = new DataView(dsProducts.Tables["ProductOffers"], whereClause, "", DataViewRowState.CurrentRows);

                rptOffers.DataSource = dv;
                rptOffers.DataBind();


            }
        }


        private void BindCart()
        {
            if (cart == null) { return; }

            litSubTotal.Text = cart.SubTotal.ToString("c", currencyCulture);
            litDiscount.Text = cart.Discount.ToString("c", currencyCulture);
            litShippingTotal.Text = cart.ShippingTotal.ToString("c", currencyCulture);
            litTaxTotal.Text = cart.TaxTotal.ToString("c", currencyCulture);
            litOrderTotal.Text = cart.OrderTotal.ToString("c", currencyCulture);

            using (IDataReader reader = cart.GetItems())
            {
                rptCartItems.DataSource = reader;
                rptCartItems.DataBind();
            }
        }

        private void rptCartItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (cart == null) { return; }

            string strGuid = e.CommandArgument.ToString();
            if (strGuid.Length != 36) { return; }

            Guid itemGuid = new Guid(strGuid);

            switch (e.CommandName)
            {
                case "updateQuantity":

                    int quantity = 1;
                    TextBox txtQty = e.Item.FindControl("txtQuantity") as TextBox;
                    if (txtQty != null)
                    {
                        try
                        {
                            int.TryParse(txtQty.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out quantity);
                        }
                        catch (ArgumentException) { }
                    }
                    cart.UpdateCartItemQuantity(itemGuid, quantity);

                    break;

                case "delete":

                    cart.DeleteItem(itemGuid);
                    cart.ResetCartOffers();
                    cart.RefreshTotals();
                    cart.Save();

                    break;

            }

            StoreHelper.EnsureValidDiscounts(store, cart);

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        void btnApplyDiscount_Click(object sender, EventArgs e)
        {
            string errorMessage = string.Empty;
            if (StoreHelper.ApplyDiscount(store, cart, txtDiscountCode.Text, out errorMessage))
            {
                WebUtils.SetupRedirect(this, Request.RawUrl);
                return;
            }
            lblDiscountError.Text = errorMessage;

        }

        void btnApplyManualDiscount_Click(object sender, EventArgs e)
        {
            if ((cart != null) && (cart.SubTotal > 0))
            {
                SiteUser clerk = SiteUtils.GetCurrentSiteUser();
                decimal discount = 0;
                decimal.TryParse(txtDiscountAmount.Text, NumberStyles.Currency, currencyCulture, out discount);
                if (discount <= cart.SubTotal)
                {
                    cart.Discount = discount;
                }
                else
                {
                    cart.Discount = cart.SubTotal;
                }
                cart.ClerkGuid = clerk.UserGuid;
                cart.RefreshTotals();


            }

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        private void PopulateLabels()
        {
            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs
                    = crumbs.ItemWrapperTop + "<a href='" + SiteRoot
                    + "/WebStore/AdminDashboard.aspx?pageid="
                    + pageId.ToInvariantString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "' class='unselectedcrumb'>" + WebStoreResources.StoreManagerLink
                    + "</a>" + crumbs.ItemWrapperBottom;
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.OrderEntry);

            heading.Text = WebStoreResources.OrderEntry;
            litProductListHeader.Text = WebStoreResources.OfferListHeading;
            litCartHeader.Text = WebStoreResources.CartHeader;

            lnkCheckout.Text = WebStoreResources.Checkout;
            btnApplyDiscount.Text = WebStoreResources.ApplyDiscountButton;
            btnApplyManualDiscount.Text = WebStoreResources.ApplyDiscountButton;
            lblDiscountError.Text = string.Empty;

        }

        private void LoadSettings()
        {
            store = StoreHelper.GetStore();
            if (store == null) { return; }
            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);
            
           
            cart = StoreHelper.GetClerkCart(store);

            lnkCheckout.NavigateUrl = SiteRoot + "/WebStore/AdminOrderCheckout.aspx?pageid=" + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString();

            divCheckoutLink.Visible = (cart.SubTotal > 0);

            int countOfDiscountCodes = Discount.GetCountOfActiveDiscountCodes(store.ModuleGuid);
            pnlDiscountCode.Visible = ((countOfDiscountCodes > 0)&&(cart.SubTotal > 0));

            pnlManualDiscount.Visible = ((WebUser.IsAdmin) && (cart.SubTotal > 0));


            AddClassToBody("webstore orderentry");

        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", true, moduleId);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);


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
            rptProducts.ItemDataBound += new RepeaterItemEventHandler(rptProducts_ItemDataBound);
            
            rptCartItems.ItemCommand += new RepeaterCommandEventHandler(rptCartItems_ItemCommand);
            btnApplyDiscount.Click += new EventHandler(btnApplyDiscount_Click);
            btnApplyManualDiscount.Click += new EventHandler(btnApplyManualDiscount_Click);
        }

        

        

        #endregion
    }
}
