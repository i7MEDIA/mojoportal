// Author:					Joe Audette
// Created:				    2008-07-12
// Last Modified:			2014-09-25
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
using mojoPortal.Business;
using mojoPortal.Business.Commerce;
using mojoPortal.Business.WebHelpers.PaymentGateway;
using Resources;
using WebStore.Business;
using WebStore.Helpers;
using log4net;

namespace WebStore.UI
{
    public partial class PayPalExpressCheckoutPage : NonCmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PayPalExpressCheckoutPage));

        protected int PageId = -1;
        protected int ModuleId = -1;
        protected SiteUser siteUser;
        protected Store store;
        protected CultureInfo currencyCulture;
        protected Cart cart;
        private CommerceConfiguration commerceConfig = null;
        private Guid payPalGetExpressCheckoutLogGuid = Guid.Empty;
        private PayPalLog checkoutDetailsLog = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();

            LoadSettings();

            if (!UserCanViewPage(ModuleId, Store.FeatureGuid))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if ((store == null) || (store.IsClosed))
            {
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                return;
            }

            PopulateLabels();
            PopulateControls();
            AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsWebStoreSection", "store");

        }

        private void PopulateControls()
        {
            if (Page.IsPostBack) { return; }

            ShowCart();

        }

        private void ShowCart()
        {
            
            if ((store == null) || (cart == null)) return;


            //if (cart.CartOffers != null)
            //{
            //    rptCartItems.DataSource = cart.CartOffers;
            //    rptCartItems.DataBind();

            //}

            using (IDataReader reader = cart.GetItems())
            {
                rptCartItems.DataSource = reader;
                rptCartItems.DataBind();
            }

            if (cart.SubTotal > 0) btnMakePayment.Enabled = true;

            litSubTotal.Text = cart.SubTotal.ToString("c", currencyCulture);

            litShippingTotal.Text = cart.ShippingTotal.ToString("c", currencyCulture);

            litTaxTotal.Text = cart.TaxTotal.ToString("c", currencyCulture);

            litOrderTotal.Text = cart.OrderTotal.ToString("c", currencyCulture);

            if (cart.ShippingTotal == 0)
            {
                pnlShippingTotal.Visible = false;
            }

            if (cart.TaxTotal == 0)
            {
                pnlTaxTotal.Visible = false;
            }

            if ((cart.ShippingTotal == 0) && (cart.TaxTotal == 0))
            {
                pnlSubTotal.Visible = false;
            }


            if (cart.OrderInfo != null)
            {
                litBillingName.Text = cart.OrderInfo.BillingFirstName + " " + cart.OrderInfo.BillingLastName + "<br />";
                litPayPalEmail.Text = cart.OrderInfo.CustomerEmail;
                if (cart.OrderInfo.BillingCompany.Length > 0)
                {
                    litBillingCompany.Text = cart.OrderInfo.BillingCompany + "<br />";
                }

                litBillingAddress1.Text = cart.OrderInfo.BillingAddress1 + "<br />";

                if (cart.OrderInfo.BillingAddress2.Length > 0)
                {
                    litBillingAddress2.Text = cart.OrderInfo.BillingAddress2 + "<br />";
                }

                if (cart.OrderInfo.BillingSuburb.Length > 0)
                {
                    litBillingSuburb.Text = cart.OrderInfo.BillingSuburb + "<br />";
                }

                litBillingCity.Text = cart.OrderInfo.BillingCity + ",&nbsp;";

                litBillingState.Text = cart.OrderInfo.BillingState + "&nbsp;&nbsp;";

                litBillingPostalCode.Text = cart.OrderInfo.BillingPostalCode + "<br />";

                litBillingCountry.Text = cart.OrderInfo.BillingCountry + "<br />";

                if (cart.HasShippingProducts())
                {
                    pnlShippingAddress.Visible = true;

                    litShippingName.Text = cart.OrderInfo.DeliveryFirstName + " " + cart.OrderInfo.DeliveryLastName + "<br />";

                    if (cart.OrderInfo.DeliveryCompany.Length > 0)
                    {
                        litShippingCompany.Text = cart.OrderInfo.DeliveryCompany + "<br />";
                    }

                    litShippingAddress1.Text = cart.OrderInfo.DeliveryAddress1 + "<br />";

                    if (cart.OrderInfo.DeliveryAddress2.Length > 0)
                    {
                        litShippingAddress2.Text = cart.OrderInfo.DeliveryAddress2 + "<br />";
                    }

                    if (cart.OrderInfo.DeliverySuburb.Length > 0)
                    {
                        litShippingSuburb.Text = cart.OrderInfo.DeliverySuburb + "<br />";
                    }

                    litShippingCity.Text = cart.OrderInfo.DeliveryCity + ",&nbsp;";

                    litShippingState.Text = cart.OrderInfo.DeliveryState + "&nbsp;&nbsp;";

                    litShippingPostalCode.Text = cart.OrderInfo.DeliveryPostalCode + "<br />";

                    litShippingCountry.Text = cart.OrderInfo.DeliveryCountry + "<br />";

                }


            }

        }

        void btnMakePayment_Click(object sender, EventArgs e)
        {
            
            PayPalExpressGateway gateway
                = new PayPalExpressGateway(
                    commerceConfig.PayPalAPIUsername,
                    commerceConfig.PayPalAPIPassword,
                    commerceConfig.PayPalAPISignature,
                    commerceConfig.PayPalStandardEmailAddress);

            gateway.UseTestMode = commerceConfig.PaymentGatewayUseTestMode;
            gateway.PayPalToken = checkoutDetailsLog.Token;
            gateway.PayPalPayerId = checkoutDetailsLog.PayerId;

            gateway.MerchantCartId = cart.CartGuid.ToString();
            gateway.ChargeTotal = cart.OrderTotal;
            gateway.ReturnUrl = SiteRoot + "/Services/PayPalReturnHandler.ashx";
            gateway.CancelUrl = SiteUtils.GetCurrentPageUrl();
            gateway.CurrencyCode = siteSettings.GetCurrency().Code;

            // **** here's where the payment is requested ******
            bool executed = gateway.CallDoExpressCheckoutPayment();

            PayPalLog payPalLog = new PayPalLog();
            payPalLog.RequestType = "DoExpressCheckoutPayment";
            payPalLog.ProviderName = WebStorePayPalReturnHandler.ProviderName;
            payPalLog.SerializedObject = checkoutDetailsLog.SerializedObject;
            payPalLog.ReturnUrl = checkoutDetailsLog.ReturnUrl;
            payPalLog.RawResponse = gateway.RawResponse;

            payPalLog.TransactionId = gateway.TransactionId;
            payPalLog.PaymentType = gateway.PayPalPaymentType;
            payPalLog.PaymentStatus = gateway.PayPalPaymentStatus;
            payPalLog.PendingReason = gateway.PayPalPendingReason;
            payPalLog.ReasonCode = gateway.ReasonCode;
            payPalLog.PayPalAmt = gateway.ChargeTotal;
            payPalLog.FeeAmt = gateway.PayPalFeeAmount;
            payPalLog.SettleAmt = gateway.PayPalSettlementAmount;
            payPalLog.TaxAmt = gateway.PayPalTaxTotal;

            payPalLog.Token = gateway.PayPalToken;
            payPalLog.PayerId = gateway.PayPalPayerId;
            payPalLog.RequestType = "DoExpressCheckoutPayment";
            payPalLog.SiteGuid = store.SiteGuid;
            payPalLog.StoreGuid = store.Guid;
            payPalLog.CartGuid = cart.CartGuid;
            payPalLog.UserGuid = cart.UserGuid;
            payPalLog.CartTotal = cart.OrderTotal;
            payPalLog.CurrencyCode = gateway.CurrencyCode;

            if (gateway.PayPalExchangeRate.Length > 0)
                payPalLog.ExchangeRate = decimal.Parse(gateway.PayPalExchangeRate);


            payPalLog.Save();

            if (!executed)
            {

                lblMessage.Text = WebStoreResources.TransactionNotInitiatedMessage;

                if (gateway.LastExecutionException != null)
                {
                    log.Error("ExpressCheckout gateway error", gateway.LastExecutionException);

                    if (commerceConfig.PaymentGatewayUseTestMode)
                    {
                        lblMessage.Text = gateway.LastExecutionException.ToString();
                    }
                        
                }
                else
                {
                    if (commerceConfig.PaymentGatewayUseTestMode)
                    {
                        lblMessage.Text = gateway.RawResponse;
                    }
                        
                }

                return;
            }

            string redirectUrl = string.Empty;

            if (gateway.TransactionId.Length == 0)
            {
                // TODO: redirect where?
                redirectUrl = SiteRoot + "/WebStore/PayPalGatewayError.aspx?plog=" + payPalLog.RowGuid.ToString();
                Response.Redirect(redirectUrl);
            }


            Guid orderStatusGuid;
            if (payPalLog.PaymentStatus == "Completed")
            {
                orderStatusGuid = OrderStatus.OrderStatusFulfillableGuid;
            }
            else
            {
                orderStatusGuid = OrderStatus.OrderStatusReceivedGuid;
            }


            Order order = Order.CreateOrder(
                    store,
                    cart,
                    payPalLog.RawResponse,
                    payPalLog.TransactionId,
                    string.Empty,
                    siteSettings.GetCurrency().Code,
                    "PayPal",
                    orderStatusGuid);

            StoreHelper.ClearCartCookie(cart.StoreGuid);
            

            // send confirmation email
            // paypal sends an order confirmation so no need

            // redirect to order details
            redirectUrl = SiteRoot +
                "/WebStore/OrderDetail.aspx?pageid="
                + PageId.ToString(CultureInfo.InvariantCulture)
                + "&mid=" + store.ModuleId.ToString(CultureInfo.InvariantCulture)
                + "&orderid=" + order.OrderGuid.ToString();

            Response.Redirect(redirectUrl);

            
        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.PayPalCheckoutButtonAltText);

            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                
            }

            litLoginInstructions.Text = WebStoreResources.CheckoutRequiresLoginOrRegisterMessage;
            litLoginPrompt.Text = WebStoreResources.CheckoutLoginPrompt;
            litRegisterPrompt.Text = WebStoreResources.CheckoutRegisterPrompt;

            lnkLogin.Text = WebStoreResources.LoginLink;
            lnkLogin.NavigateUrl = SiteRoot + "/Secure/Login.aspx";
            lnkRegister.Text = WebStoreResources.RegisterLink;
            lnkRegister.NavigateUrl = SiteRoot + "/Secure/Register.aspx?returnurl=" + Server.UrlEncode(Request.RawUrl);

            heading.Text = WebStoreResources.CheckoutHeader;
            litBillingHeader.Text = WebStoreResources.CheckoutBillingInfoHeader;
            litOrderSummary.Text = WebStoreResources.CheckoutOrderSummaryHeader;
            litShippingHeader.Text = WebStoreResources.CheckoutShippingInfoHeader;

            btnMakePayment.Text = WebStoreResources.PayNow;
            btnMakePayment.Attributes.Add("onclick", "this.value='" + WebStoreResources.PaymentButtonDisabledText + "';this.disabled = true;" + Page.ClientScript.GetPostBackEventReference(this.btnMakePayment, ""));
            

        }

        private void LoadSettings()
        {
            PageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            ModuleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            payPalGetExpressCheckoutLogGuid = WebUtils.ParseGuidFromQueryString("plog", payPalGetExpressCheckoutLogGuid);

            if (payPalGetExpressCheckoutLogGuid == Guid.Empty)
            {
                Response.Redirect(SiteUtils.GetCurrentPageUrl());
            }

            checkoutDetailsLog = new PayPalLog(payPalGetExpressCheckoutLogGuid);

            if (checkoutDetailsLog.RowGuid == Guid.Empty)
            {
                Response.Redirect(SiteUtils.GetCurrentPageUrl());
            }

            cart = (Cart)SerializationHelper.DeserializeFromString(typeof(Cart), checkoutDetailsLog.SerializedObject);
            
            if (cart == null)
            {
                Response.Redirect(SiteUtils.GetCurrentPageUrl());
            }
            cart.DeSerializeCartOffers();

            cart.RefreshTotals();

            
            if ((cart.LastModified < DateTime.UtcNow.AddDays(-1)) && (cart.DiscountCodesCsv.Length > 0))
            {
                StoreHelper.EnsureValidDiscounts(store, cart);
            }
            

            siteUser = SiteUtils.GetCurrentSiteUser();
            //if (siteUser == null)
            //{
            //    Response.Redirect(SiteUtils.GetCurrentPageUrl());
            //}

            if ((siteUser != null)&&(cart.UserGuid == Guid.Empty))
            {
                // user wasn't logged in when express checkout was called
                cart.UserGuid = siteUser.UserGuid;
                cart.Save();
                //if (checkoutDetailsLog.UserGuid == Guid.Empty)
                //{
                //    // we need to make sure we have the user in the log and serialized cart
                //    checkoutDetailsLog.UserGuid = siteUser.UserGuid;
                //    cart.SerializeCartOffers();
                //    checkoutDetailsLog.SerializedObject = SerializationHelper.SerializeToSoap(cart);
                //    checkoutDetailsLog.Save();

                //}
            }

            if ((siteUser != null)&&(cart.UserGuid != siteUser.UserGuid))
            {
                Response.Redirect(SiteUtils.GetCurrentPageUrl());
            }

            

            if (ModuleId == -1)
            {
                ModuleId = StoreHelper.FindStoreModuleId(CurrentPage);
            }

            
            store = StoreHelper.GetStore();
            

            commerceConfig = SiteUtils.GetCommerceConfig();
            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);

            if (siteUser != null)
            {
                pnlRequireLogin.Visible = false;
            }
            else
            {
                btnMakePayment.Visible = false;
            }

            AddClassToBody("webstore webstoreexpresscheckout");
        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnMakePayment.Click += new EventHandler(btnMakePayment_Click);

            SuppressPageMenu();

        }

        

        #endregion
    }
}
