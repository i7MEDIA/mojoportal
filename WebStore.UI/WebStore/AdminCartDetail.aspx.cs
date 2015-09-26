/// Author:					Joe Audette
/// Created:				2008-03-26
/// Last Modified:			2012-10-02
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
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using WebStore.Business;
using WebStore.Helpers;
using Resources;

namespace WebStore.UI
{
    public partial class AdminCartDetailPage : NonCmsBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        protected Store store;
        protected Double timeOffset = 0;
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        protected Guid cartGuid = Guid.Empty;
        private Cart cart;
        private SiteUser cartUser = null;
        private bool canEdit = false;


        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParams();

            if (!canEdit)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();

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

            if (Page.IsPostBack) return;

            if ((store != null) && (cart != null))
            {
                lblIPAddress.Text = cart.CreatedFromIP;

                if (cartUser != null)
                {
                    lblSiteUser.Text = cartUser.LoginName + " " + cartUser.Name + " " + cartUser.Email;
                    if (WebUser.IsAdmin)
                    {
                        lnkUser.Text = WebStoreResources.ManageUserLink;
                        lnkUser.NavigateUrl = SiteRoot + "/Admin/ManageUsers.aspx?userid=" + cartUser.UserId.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        lnkUser.Text = WebStoreResources.UserProfileLink;
                        lnkUser.NavigateUrl = SiteRoot + "/ProfileView.aspx?userid=" + cartUser.UserId.ToString(CultureInfo.InvariantCulture);
                    }
                }

                using (IDataReader reader = cart.GetItems())
                {
                    rptCartItems.DataSource = reader;
                    rptCartItems.DataBind();
                }

                litSubTotal.Text = cart.SubTotal.ToString("c", currencyCulture);

                lblCustomerAddressLine1.Text = cart.OrderInfo.CustomerAddressLine1;
                lblCustomerAddressLine2.Text = cart.OrderInfo.CustomerAddressLine2;
                lblCustomerCity.Text = cart.OrderInfo.CustomerCity;
                lblCustomerCompany.Text = cart.OrderInfo.CustomerCompany;
                lblCustomerCountry.Text = cart.OrderInfo.CustomerCountry;
                lblCustomerEmail.Text = cart.OrderInfo.CustomerEmail;
                lblCustomerGeoZone.Text = cart.OrderInfo.CustomerState;
                lblCustomerFirstName.Text = cart.OrderInfo.CustomerFirstName;
                lblCustomerLastName.Text = cart.OrderInfo.CustomerLastName;
                lblCustomerPostalCode.Text = cart.OrderInfo.CustomerPostalCode;
                lblCustomerSuburb.Text = cart.OrderInfo.CustomerSuburb;
                lblCustomerTelephoneDay.Text = cart.OrderInfo.CustomerTelephoneDay;
                lblCustomerTelephoneNight.Text = cart.OrderInfo.CustomerTelephoneNight;

                Control c = Page.LoadControl("~/Controls/GCheckoutLogList.ascx");
                if (c != null)
                {
                    GCheckoutLogList googleLog = c as GCheckoutLogList;
                    googleLog.CartGuid = cart.CartGuid;
                    pnlCheckoutLog.Controls.Add(googleLog);
                }

                c = Page.LoadControl("~/Controls/PayPalLogList.ascx");
                if (c != null)
                {
                    PayPalLogList logList = c as PayPalLogList;
                    logList.CartGuid = cart.CartGuid;
                    pnlCheckoutLog.Controls.Add(logList);
                }

                c = Page.LoadControl("~/Controls/PaymentLogList.ascx");
                if (c != null)
                {
                    PaymentLogList logList = c as PaymentLogList;
                    logList.CartGuid = cart.CartGuid;
                    pnlCheckoutLog.Controls.Add(logList);
                }

                //c = Page.LoadControl("~/Controls/AuthorizeNetLogList.ascx");
                //if (c != null)
                //{
                //    AuthorizeNetLogList logList = c as AuthorizeNetLogList;
                //    logList.CartGuid = cart.CartGuid;
                //    pnlCheckoutLog.Controls.Add(logList);
                //}

                //c = Page.LoadControl("~/Controls/PlugNPayLogList.ascx");
                //if (c != null)
                //{
                //    PlugNPayLogList logList = c as PlugNPayLogList;
                //    logList.CartGuid = cart.CartGuid;
                //    pnlCheckoutLog.Controls.Add(logList);
                //}
                
               

            }


        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            Cart.Delete(cartGuid);

            string redirectUrl = "AdminCartBrowser.aspx?pageid="
                    + pageId.ToString(CultureInfo.InvariantCulture)
                    + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture);

            WebUtils.SetupRedirect(this, redirectUrl);
            

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.BrowseCartsLink);

            Control c = Master.FindControl("Breadcrumbs");
            if ((c != null) && (store != null))
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs
                    = crumbs.ItemWrapperTop + "<a href='" + SiteRoot
                    + "/WebStore/AdminDashboard.aspx?pageid="
                    + pageId.ToInvariantString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "'>" + WebStoreResources.StoreManagerLink
                    + "</a>" + crumbs.ItemWrapperBottom
                    + crumbs.Separator + crumbs.ItemWrapperTop
                    + "<a href='" + SiteRoot
                    + "/WebStore/AdminCartBrowser.aspx?pageid="
                    + pageId.ToInvariantString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "'>" + WebStoreResources.BrowseCartsLink
                    + "</a>" + crumbs.ItemWrapperBottom;

            }

            

            btnDelete.Text = WebStoreResources.AdminDeleteCartButton;

            UIHelper.AddConfirmationDialog(btnDelete, WebStoreResources.AdminDeleteCartButtonWarning);


        }

        private void LoadSettings()
        {
            store = StoreHelper.GetStore();
            if (store == null) { return; }

            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);

            if (cartGuid != Guid.Empty) 
            {
                cart = new Cart(cartGuid);
                
            }

            if (cart.StoreGuid != store.Guid)
                cart = null;

            if ((cart != null) && (cart.UserGuid != Guid.Empty))
            {
                cartUser = new SiteUser(siteSettings, cart.UserGuid);
            }

            timeOffset = SiteUtils.GetUserTimeOffset();

            AddClassToBody("webstore webstoreadmincartdetail");
        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            cartGuid = WebUtils.ParseGuidFromQueryString("cart", cartGuid);

            canEdit = UserCanEditModule(moduleId, Store.FeatureGuid);
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
            this.Load += this.Page_Load;
            this.btnDelete.Click += btnDelete_Click;

            SuppressPageMenu();

        }

        

        #endregion
    }
}

