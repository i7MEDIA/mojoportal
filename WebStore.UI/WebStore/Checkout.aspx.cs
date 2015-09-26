/// Author:					Joe Audette
/// Created:				2007-03-09
/// Last Modified:			2013-09-17
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using log4net;
using mojoPortal.Business;
using WebStore.Business;
using mojoPortal.Business.Commerce;
using mojoPortal.Business.WebHelpers.PaymentGateway;
using Resources;
using WebStore.Helpers;
using GCheckout.Checkout;
using GCheckout.Util;

namespace WebStore.UI
{

    public partial class CheckoutPage : NonCmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CheckoutPage));

        protected int pageId = -1;
        protected int moduleId = -1;
        protected SiteUser siteUser;
        protected Store store;
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        protected Cart cart;
        private List<CartOffer> cartOffers = null;
        private CommerceConfiguration commerceConfig = null;
        private string PayPalExpressButtonImageUrl = "https://www.paypal.com/en_US/i/btn/btn_xpressCheckout.gif";
        private bool canCheckoutWithoutAuthentication = false;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SiteUtils.SslIsAvailable()) { SiteUtils.ForceSsl(); }
            SecurityHelper.DisableBrowserCache();

            LoadParams();

            if (!UserCanViewPage(moduleId, Store.FeatureGuid))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            // populate labels first because some of them may be modifed by settings
            PopulateLabels();

            LoadSettings();

            if ((store == null) || (store.IsClosed))
            {
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                return;
            }

            if ((!canCheckoutWithoutAuthentication) && (!Request.IsAuthenticated))
            {
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                return;
            }

           
            
            PopulateControls();
            AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsWebStoreSection", "store");

        }

        private void PopulateControls()
        {
            if (!Page.IsPostBack)
            {
                PopulateYearList();
            }

            ShowCart();
            SetupPaymentUI();

        }

        private void ShowCart()
        {
            btnMakePayment.Enabled = false;
            if (commerceConfig.GoogleCheckoutIsEnabled)
            {
                if ((!commerceConfig.Is503TaxExempt)&& (cart.HasDonations()))
                {
                    btnGoogleCheckout.Visible = true;
                    btnGoogleCheckout.Enabled = false;
                    lblGoogleMessage.Text = WebStoreResources.GoogleCheckoutDisabledForDonationsMessage;
                    lblGoogleMessage.Visible = true;
                 
                }
                else
                {
                    btnGoogleCheckout.Visible = true;
                }
            }
            else
            {
                btnGoogleCheckout.Visible = false;
            }

           
            if ((store == null) || (cart == null)) return;

            if (cartOffers != null)
            {
                rptCartItems.DataSource = cartOffers;
                rptCartItems.DataBind();
            }

            

            litSubTotal.Text = cart.SubTotal.ToString("c", currencyCulture);
            litDiscount.Text = cart.Discount.ToString("c", currencyCulture);
            litShippingTotal.Text = cart.ShippingTotal.ToString("c", currencyCulture);
            litTaxTotal.Text = cart.TaxTotal.ToString("c", currencyCulture);
            litOrderTotal.Text = cart.OrderTotal.ToString("c", currencyCulture);

            pnlDiscount.Visible = (cart.Discount > 0);
            pnlShippingTotal.Visible = (cart.ShippingTotal > 0);
            pnlTaxTotal.Visible = (cart.TaxTotal > 0);

            if ((cart.ShippingTotal == 0) && (cart.TaxTotal == 0) && (cart.Discount == 0))
            {
                pnlSubTotal.Visible = false;
            }


            if (cart.OrderInfo != null)
            {
                litBillingName.Text = cart.OrderInfo.BillingFirstName + " " + cart.OrderInfo.BillingLastName + "<br />";
                
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

            if (commerceConfig.PaymentGatewayUseTestMode)
            {
                //pnp requires the name to be test user and the ccnumber to be 4111111111111111
                txtCardOwnerFirstName.Text = "John";
                txtCardOwnerLastName.Text = "Doe";
                txtCardNumber.Text = "4721930402892796";
                txtCardSecurityCode.Text = "456";

                ListItem item = ddExpireYear.Items.FindByValue(DateTime.UtcNow.AddYears(1).Year.ToString(CultureInfo.InvariantCulture));
                if (item != null)
                {
                    ddExpireYear.ClearSelection();
                    item.Selected = true;

                }


            }

        }

        

        

        private void btnMakePayment_Click(object sender, System.EventArgs e)
        {
            //Page.Validate();

            if (
                (store != null)
                && (cart != null)
                && (IsValidForCheckout())
                )
            {
                IPaymentGateway gateway = commerceConfig.GetDirectPaymentGateway();
                if (gateway == null)
                {
                    lblMessage.Text = WebStoreResources.PaymentGatewayNotConfiguredForDirectCardProcessing;
                    btnMakePayment.Enabled = false;
                    return;
                }
                //if (gateway is PlugNPayPaymentGateway)
                //{
                    gateway.MerchantInvoiceNumber = cart.CartGuid.ToString("N");
                    string CartItems = "";
                    int itemnum =0;
                    foreach (CartOffer coffer in cart.CartOffers)
                    {
                        itemnum++;
                        CartItems += string.Format("&item{1}={0}&cost{1}={2}&description{1}={3}&quantity{1}={4}", coffer.OfferGuid, itemnum, coffer.OfferPrice, coffer.Name, coffer.Quantity);
                    }
                    gateway.MerchantTransactionDescription = CartItems; //not sure if this is the intended purpose or not
                //}

                gateway.CardType = ddCardType.SelectedValue;
                gateway.CardNumber = txtCardNumber.Text;
                gateway.CardExpiration = ddExpireMonth.SelectedValue + ddExpireYear.SelectedValue;
                gateway.ChargeTotal = cart.OrderTotal;
                gateway.CardSecurityCode = txtCardSecurityCode.Text;
                
                gateway.CardOwnerFirstName = txtCardOwnerFirstName.Text;
                gateway.CardOwnerLastName = txtCardOwnerLastName.Text;

                gateway.CardOwnerCompanyName = cart.OrderInfo.BillingCompany;
                gateway.CardBillingAddress = cart.OrderInfo.BillingAddress1
                    + " " + cart.OrderInfo.BillingAddress2;

                gateway.CardBillingCity = cart.OrderInfo.BillingCity;
                gateway.CardBillingState = cart.OrderInfo.BillingState;
                gateway.CardBillingPostalCode = cart.OrderInfo.BillingPostalCode;
                gateway.CardBillingCountry = cart.OrderInfo.BillingCountry;

                
                gateway.CardBillingCountryCode = cart.OrderInfo.BillingCountry;

                gateway.CardBillingPhone = cart.OrderInfo.CustomerTelephoneDay;
                gateway.CustomerIPAddress = SiteUtils.GetIP4Address();
                gateway.CurrencyCulture = currencyCulture;

                // this is where the actual request is made, it can timeout here
                bool executed = false;
                try
                {
                    executed = gateway.ExecuteTransaction();
                }
                catch (WebException ex)
                {
                    if (commerceConfig.PaymentGatewayUseTestMode)
                    {
                        lblMessage.Text = ex.Message;
                    }
                    else
                    {
                        lblMessage.Text = WebStoreResources.PaymentGatewayCouldNotConnectMessage;
                    }
                    return;
                }

                string serializedCart = string.Empty;

                gateway.LogTransaction(
                        siteSettings.SiteGuid,
                        store.ModuleGuid,
                        store.Guid,
                        cart.CartGuid,
                        cart.UserGuid,
                        string.Empty,
                        "WebStoreCheckout",
                        serializedCart);

                //if (gateway is PayPalDirectPaymentGateway)
                //{
                //    // this is capturing of the serialized cart is not needed for other providers and I'm not sure its even needed for PayPal Direct
                //    // it was needed for other paypal solutions. I just don't want to remove it until I'm sure
                //    cart.SerializeCartOffers();
                //    serializedCart = SerializationHelper.SerializeToString(cart);

                //    gateway.LogTransaction(
                //        siteSettings.SiteGuid,
                //        store.ModuleGuid,
                //        store.Guid,
                //        cart.CartGuid,
                //        cart.UserGuid,
                //        "WebStorePayPalDirect",
                //        "DirectPayment",
                //        serializedCart);

                //}
                //else
                //{
                //    gateway.LogTransaction(
                //        siteSettings.SiteGuid,
                //        store.ModuleGuid,
                //        store.Guid,
                //        cart.CartGuid,
                //        cart.UserGuid,
                //        string.Empty,
                //        "WebStoreCheckout",
                //        serializedCart);
                //}

                
                if (executed)
                {
                    switch (gateway.Response)
                    {
                        case PaymentGatewayResponse.Approved:
                            cart.LastUserActivity = DateTime.UtcNow;
                            cart.OrderInfo.CompletedFromIP = SiteUtils.GetIP4Address();
                            cart.OrderInfo.Completed = DateTime.UtcNow;
                            StoreHelper.EnsureUserForOrder(cart);
                            cart.Save();

                            //Order order = Order.CreateOrder(store, cart, gateway, store.DefaultCurrency, "CreditCard");
                            Order order = Order.CreateOrder(store, cart, gateway, siteSettings.GetCurrency().Code, "CreditCard");
                            StoreHelper.ClearCartCookie(cart.StoreGuid);

                            // send confirmation email
                            try
                            {
                                // this also happens in StoreHelper.ConfirmOrder
                                //Module m = new Module(store.ModuleId);
                                //Order.EnsureSalesReportData(m.ModuleGuid, pageId, moduleId);

                                StoreHelper.ConfirmOrder(store, order);      
                                PayPalLog.DeleteByCart(order.OrderGuid);
                                GoogleCheckoutLog.DeleteByCart(order.OrderGuid);
                            }
                            catch (Exception ex)
                            {
                                log.Error("error sending confirmation email", ex);
                            }

                            // redirect to order details
                            string redirectUrl = SiteRoot +
                                "/WebStore/OrderDetail.aspx?pageid="
                                + pageId.ToInvariantString()
                                + "&mid=" + moduleId.ToInvariantString()
                                + "&orderid=" + order.OrderGuid.ToString();

                            //TODO: if we charged a card here we can safely delete any paypal log or googlecheckout logs
                            // need methods to delete those by carguid


                            if (WebStoreConfiguration.LogCardTransactionStatus)
                            {
                                log.Info("accepted transaction " + gateway.ChargeTotal.ToString("c") + " " + gateway.ResponseCode + " " + gateway.ResponseReason);
                            }

                            WebUtils.SetupRedirect(this, redirectUrl);
                            return;

                        case PaymentGatewayResponse.Declined:

                            lblMessage.Text = WebStoreResources.TransactionDeclinedMessage;

                            if ((WebStoreConfiguration.LogCardTransactionStatus) || (WebStoreConfiguration.LogCardFailedTransactionStatus))
                            {
                                log.Info("declined transaction " + gateway.ChargeTotal.ToString("c") + " " +  gateway.ResponseCode + " " + gateway.ResponseReason);
                            }

                            break;


                        case PaymentGatewayResponse.Error:

                            if (gateway.UseTestMode)
                            {
                                if (gateway.LastExecutionException != null)
                                {
                                    lblMessage.Text = gateway.LastExecutionException.ToString();
                                }
                                else
                                {
                                    // TODO: should not show user real messages? Mask CCNumber and login credentials in the gateways RawResponse property ... those shouldn't be logged either
                                    lblMessage.Text = gateway.RawResponse;
                                }
                            }
                            else
                            {
                                lblMessage.Text = WebStoreResources.TransactionErrorMessage;
                                if ((WebStoreConfiguration.LogCardTransactionStatus)||(WebStoreConfiguration.LogCardFailedTransactionStatus))
                                {
                                    if (gateway.LastExecutionException != null)
                                    {
                                        log.Info("transaction error " + gateway.LastExecutionException.ToString());
                                    }
                                }
                            }


                            break;

                        case PaymentGatewayResponse.NoRequestInitiated:

                            lblMessage.Text = WebStoreResources.TransactionNotInitiatedMessage;
                            break;
                    }
                }
                else
                {
                    lblMessage.Text = WebStoreResources.TransactionNotInitiatedMessage;

                    if (gateway.LastExecutionException != null)
                    {
                        if(commerceConfig.PaymentGatewayUseTestMode)
                        lblMessage.Text = gateway.LastExecutionException.ToString();
                    }
                }
 
            }
            
            btnMakePayment.Text = WebStoreResources.PaymentButton;
        }

        void btnFreeCheckout_Click(object sender, EventArgs e)
        {
            if (cart.OrderTotal > 0) 
            {
                WebUtils.SetupRedirect(this, Request.RawUrl);
                return; 
            }

            cart.LastUserActivity = DateTime.UtcNow;
            cart.OrderInfo.CompletedFromIP = SiteUtils.GetIP4Address();
            cart.OrderInfo.Completed = DateTime.UtcNow;
            StoreHelper.EnsureUserForOrder(cart);
            cart.Save();

            Order order = Order.CreateOrder(
                store,
                cart,
                string.Empty,
                string.Empty,
                string.Empty,
                siteSettings.GetCurrency().Code,
                "NoCharge",
                OrderStatus.OrderStatusFulfillableGuid);
            
            StoreHelper.ClearCartCookie(cart.StoreGuid);

            // send confirmation email
            try
            {
                StoreHelper.ConfirmOrder(store, order);
                Module m = new Module(store.ModuleId);
                Order.EnsureSalesReportData(m.ModuleGuid, pageId, moduleId);
            }
            catch (Exception ex)
            {
                log.Error("error sending confirmation email", ex);
            }

            // redirect to order details
            string redirectUrl = SiteRoot +
                "/WebStore/OrderDetail.aspx?pageid="
                + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString()
                + "&orderid=" + order.OrderGuid.ToString();

            WebUtils.SetupRedirect(this, redirectUrl);

        }

        void btnGoogleCheckout_Click(object sender, ImageClickEventArgs e)
        {
            if (
                (store != null)
                && (cart != null)
                )
            { //&& (IsValidForCheckout()) ?

                int cartTimeoutInMinutes = 30;
               
                CheckoutShoppingCartRequest Req = new CheckoutShoppingCartRequest(
                    commerceConfig.GoogleMerchantID,
                    commerceConfig.GoogleMerchantKey,
                    commerceConfig.GoogleEnvironment,
                    siteSettings.GetCurrency().Code,
                    cartTimeoutInMinutes);


                foreach (CartOffer cartOffer in cartOffers)
                {

                    Req.AddItem(
                        cartOffer.Name, 
                        string.Empty, 
                        cartOffer.OfferPrice, 
                        cartOffer.Quantity);
                }

                //Req.AddMerchantCalculatedShippingMethod
                //Req.AnalyticsData
                //Req.ContinueShoppingUrl
                //Req.EditCartUrl
                
                //Req.RequestInitialAuthDetails
                //Req.AddParameterizedUrl

                // we need to serialize the cart and it items to xml here
                // so when we get it back from google
                // we can validate against the existing cart
                // as its possible items were added to the cart
                // after we passed the user to google
                
                //Req.MerchantPrivateData = cart.CartGuid.ToString();
                //cart.SerializeCartOffers();
                //Req.MerchantPrivateData = SerializationHelper.SerializeToSoap(cart);

                cart.SerializeCartOffers();
                MerchantData merchantData = new MerchantData();
                merchantData.ProviderName = "WebStoreGCheckoutNotificationHandlerProvider";
                merchantData.SerializedObject = SerializationHelper.RemoveXmlDeclaration(SerializationHelper.SerializeToString(cart));

                Req.MerchantPrivateData = SerializationHelper.RemoveXmlDeclaration(SerializationHelper.SerializeToString(merchantData));

                Req.RequestBuyerPhoneNumber = true;

                // flat rate shipping example
                //Req.AddFlatRateShippingMethod("UPS Ground", 5);

                //Add a rule to tax all items at 7.5% for Ohio
                //Req.AddStateTaxRule("NC", .15, true);
                //TODO: lookup tax 

                GCheckoutResponse Resp = Req.Send();
         
                if (Resp.IsGood)
                {
                    Response.Redirect(Resp.RedirectUrl, true);
                }
                else
                {
                    lblMessage.Text = Resp.ErrorMessage;
                }
            }
            
        }

        /// <summary>
        /// This is really a fallback method, it will not be executed in normal use of PayPalStandard because we are setting the postbackurl of the button
        /// to post directly to paypal, so this method should not fire unless somehow the page is manipulated to postback to itself or if using PayPalPro Express Checkout
        /// In this case, we just consolidate the cart into a buy now button.
        /// </summary>
        void btnPayPal_Click(object sender, ImageClickEventArgs e)
        {
            if (commerceConfig.PayPalUsePayPalStandard)
            {
                // should not reach here
                DoPayPalStandardCheckout();
            }
            else
            {
                DoPayPalExpressCeckout();
            }

        }

        
        private void DoPayPalStandardCheckout()
        {
            PayPalLog payPalLog = new PayPalLog();

            payPalLog.ProviderName = WebStorePayPalReturnHandler.ProviderName;
            payPalLog.PDTProviderName = WebStorePayPalPDTHandlerProvider.ProviderName;
            payPalLog.IPNProviderName = WebStorePayPalIPNHandlerProvider.ProviderName;
            payPalLog.ReturnUrl = SiteRoot +
                                "/WebStore/OrderDetail.aspx?pageid="
                                + pageId.ToInvariantString()
                                + "&mid=" + moduleId.ToInvariantString()
                                + "&orderid=" + cart.CartGuid.ToString();
            
            payPalLog.RequestType = "StandardCheckout";
            
            cart.SerializeCartOffers();
            payPalLog.SerializedObject = SerializationHelper.SerializeToString(cart);

            //Currency currency = new Currency(store.DefaultCurrencyId);

            payPalLog.CartGuid = cart.CartGuid;
            payPalLog.SiteGuid = store.SiteGuid;
            payPalLog.StoreGuid = store.Guid;
            payPalLog.UserGuid = cart.UserGuid;
            payPalLog.CartTotal = cart.OrderTotal;
            payPalLog.CurrencyCode = siteSettings.GetCurrency().Code;

            payPalLog.Save();

            string payPalStandardUrl = StoreHelper.GetBuyNowUrl(
                payPalLog.RowGuid,
                cart,
                store,
                commerceConfig);

            WebUtils.SetupRedirect(this, payPalStandardUrl);

        }

        private void SetupPayPalStandardForm()
        {
            PayPalLog payPalLog = StoreHelper.EnsurePayPalStandardCheckoutLog(cart, store, SiteRoot, pageId, moduleId);
            if (payPalLog == null)
            {
                // this shouldn't happen but if we don't have the log then hide the button
                btnPayPal.Visible = false;
                return;
            }

            litPayPalFormVariables.Text = StoreHelper.GetCartUploadFormFields(
                payPalLog.RowGuid,
                cart,
                store,
                commerceConfig);

            btnPayPal.PostBackUrl = commerceConfig.PayPalStandardUrl;
        }

        

        private void DoPayPalExpressCeckout()
        {
            PayPalExpressGateway gateway
                = new PayPalExpressGateway(
                    commerceConfig.PayPalAPIUsername,
                    commerceConfig.PayPalAPIPassword,
                    commerceConfig.PayPalAPISignature,
                    commerceConfig.PayPalStandardEmailAddress);

            gateway.UseTestMode = commerceConfig.PaymentGatewayUseTestMode;

            gateway.MerchantCartId = cart.CartGuid.ToString();
            gateway.ChargeTotal = cart.OrderTotal;
            
            string siteRoot = SiteUtils.GetNavigationSiteRoot();
            gateway.ReturnUrl = siteRoot + "/Services/PayPalReturnHandler.ashx";
            gateway.CancelUrl = siteRoot + Request.RawUrl;
            //Currency currency = new Currency(store.DefaultCurrencyId);
            gateway.CurrencyCode = siteSettings.GetCurrency().Code;
            gateway.OrderDescription = store.Name + " " + WebStoreResources.OrderHeading;
            
            gateway.BuyerEmail = cart.OrderInfo.CustomerEmail;
            gateway.ShipToFirstName = cart.OrderInfo.DeliveryFirstName;
            gateway.ShipToLastName = cart.OrderInfo.DeliveryLastName;
            gateway.ShipToAddress = cart.OrderInfo.DeliveryAddress1;
            gateway.ShipToAddress2 = cart.OrderInfo.DeliveryAddress2;
            gateway.ShipToCity = cart.OrderInfo.DeliveryCity;
            gateway.ShipToState = cart.OrderInfo.DeliveryState;
            gateway.ShipToCountry = cart.OrderInfo.DeliveryCountry;
            gateway.ShipToPostalCode = cart.OrderInfo.DeliveryPostalCode;
            gateway.ShipToPhone = cart.OrderInfo.CustomerTelephoneDay;
            
            // this tells paypal to use the shipping address we pass in
            // rather than what the customer has on file
            // when we implement shippable products we'll do shipping calculations before
            // sending the user to paypal
            //gateway.OverrideShippingAddress = true;

            //commented out the above, we want user to be able to populate shipping info from their paypal account

            bool executed = gateway.CallSetExpressCheckout();
            if (executed)
            {
                //TODO: log the raw response
                if (gateway.PayPalExpressUrl.Length > 0)
                {
                    // record the gateway.PayPalToken
                    PayPalLog payPalLog = new PayPalLog();
                    payPalLog.RawResponse = gateway.RawResponse;
                    payPalLog.ProviderName = WebStorePayPalReturnHandler.ProviderName;
                    payPalLog.ReturnUrl = siteRoot + Request.RawUrl;
                    payPalLog.Token = HttpUtility.UrlDecode(gateway.PayPalToken);
                    payPalLog.RequestType = "SetExpressCheckout";
                    //payPalLog.PendingReason = gateway.PayPalExpressUrl;

                    cart.SerializeCartOffers();
                    payPalLog.SerializedObject = SerializationHelper.SerializeToString(cart);

                    payPalLog.CartGuid = cart.CartGuid;
                    payPalLog.SiteGuid = store.SiteGuid;
                    payPalLog.StoreGuid = store.Guid;
                    payPalLog.UserGuid = cart.UserGuid;

                    payPalLog.Save();

                    Response.Redirect(gateway.PayPalExpressUrl);

                }
                else
                {
                    if (commerceConfig.PaymentGatewayUseTestMode)
                        lblMessage.Text = gateway.RawResponse;
                }

            }
            else
            {
                lblMessage.Text = WebStoreResources.TransactionNotInitiatedMessage;

                if (gateway.LastExecutionException != null)
                {
                    if (commerceConfig.PaymentGatewayUseTestMode)
                        lblMessage.Text = gateway.LastExecutionException.ToString();
                }
                else
                {
                    if (commerceConfig.PaymentGatewayUseTestMode)
                        lblMessage.Text = gateway.RawResponse;

                }
            }


        }

        private void SetupWorldPay()
        {
            if ((!Request.IsAuthenticated) && (!canCheckoutWithoutAuthentication)) { return; }

            if (commerceConfig.WorldPayInstallationId.Length == 0) { return; }

            if (cart.OrderTotal <= 0) { return; }

            pnlWorldPay.Visible = true;
            frmCardInput.Visible = false;
            heading.Text = WebStoreResources.WorldPayCheckoutHeader;

            worldPayAcceptanceMark.InstId = commerceConfig.WorldPayInstallationId;
            btnWorldPay.Text = WebStoreResources.ContinueButton;

            btnWorldPay.InstId = commerceConfig.WorldPayInstallationId;
            btnWorldPay.Md5Secret = commerceConfig.WorldPayMd5Secret;
            btnWorldPay.MerchantCode = commerceConfig.WorldPayMerchantCode;
            btnWorldPay.CurrencyCode = siteSettings.GetCurrency().Code;

            btnWorldPay.Amount = cart.OrderTotal;
            btnWorldPay.UseTestServer = commerceConfig.PaymentGatewayUseTestMode;
            btnWorldPay.OrderDescription = cart.GetStringOfCartOfferNames();

            btnWorldPay.CustomerEmail = cart.OrderInfo.CustomerEmail;
            if ((cart.OrderInfo.CustomerFirstName.Length > 0) && (cart.OrderInfo.CustomerLastName.Length > 0))
            {
                btnWorldPay.CustomerName = string.Format(CultureInfo.InvariantCulture, WebStoreResources.FirstNameLastNameFormat,
                    cart.OrderInfo.CustomerFirstName, cart.OrderInfo.CustomerLastName);
            }

            btnWorldPay.Address1 = cart.OrderInfo.CustomerAddressLine1;
            btnWorldPay.Address2 = cart.OrderInfo.CustomerAddressLine2;
            btnWorldPay.Town = cart.OrderInfo.CustomerCity;
            btnWorldPay.Region = cart.OrderInfo.CustomerState;
            btnWorldPay.Country = cart.OrderInfo.CustomerCountry;
            btnWorldPay.PostalCode = cart.OrderInfo.CustomerPostalCode;
            btnWorldPay.CustomerPhone = cart.OrderInfo.CustomerTelephoneDay;


            PayPalLog worldPayLog = StoreHelper.EnsureWorldPayCheckoutLog(
                cart,
                store,
                SiteRoot,
                WebUtils.ResolveServerUrl(SiteUtils.GetCurrentPageUrl()),
                pageId,
                moduleId);

            // note that we actually pass the PayPalLog guid not the cart id
            // we then deserialize the cart form tha PayPsalLog to ensure it has not been modified
            // since the user left our site and went to WorldPay
            btnWorldPay.CartId = worldPayLog.RowGuid.ToString();
            //btnWorldPay.CustomData = worldPayLog.RowGuid.ToString();


        }
        

        private bool IsValidForCheckout()
        {
            bool result = true;

            int year = int.Parse(ddExpireYear.SelectedValue);
            int month = int.Parse(ddExpireMonth.SelectedValue);
            if (
                (year == DateTime.Now.Year)
                && (month < DateTime.Now.Month)
                )
            {

                result = false;
                this.lblMessage.Text += " " + WebStoreResources.CardExpiredWarning;

            }

            return result;

        }

        private void SetupPaymentUI()
        {
            switch (commerceConfig.PrimaryPaymentGateway)
            {
                case "PayPalDirect":
                case "Authorize.NET":
                case "PlugNPay":
                    frmCardInput.Visible = true;
                    break;

                case "PayPalStandard":
                case "GoogleCheckout":
                default:
                    frmCardInput.Visible = false;
                    break;

            }

            if (!commerceConfig.CanProcessStandardCards)
            {
                frmCardInput.Visible = false;
            }

            if (cart.OrderTotal > 0)
            {
                btnMakePayment.Enabled = true;
            }
            else
            {
                btnGoogleCheckout.Visible = false;
                if (cart.CartOffers.Count > 0)
                {
                    // order total is 0 but items in cart means free order
                    frmCardInput.Visible = false;
                    btnFreeCheckout.Visible = true;

                }
            }

            SetupWorldPay();

        }

        private void PopulateYearList()
        {
            DateTime date = DateTime.Today;

            for (int i = 0; i < 9; i++)
            {
                ListItem listItem
                    = new ListItem(date.Year.ToString(), date.Year.ToString());

                ddExpireYear.Items.Add(listItem);
                date = date.AddYears(1);

            }

        }

        private void PopulateLabels()
        {
            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                
            }

            heading.Text = WebStoreResources.CheckoutHeader;
            litBillingHeader.Text = WebStoreResources.CheckoutBillingInfoHeader;
            litOrderSummary.Text = WebStoreResources.CheckoutOrderSummaryHeader;
            litShippingHeader.Text = WebStoreResources.CheckoutShippingInfoHeader;

            btnMakePayment.Text = WebStoreResources.PaymentButton;
            btnMakePayment.Attributes.Add("onclick", "this.value='" + WebStoreResources.PaymentButtonDisabledText + "';this.disabled = true;" + Page.ClientScript.GetPostBackEventReference(this.btnMakePayment, ""));
            
            btnPayPal.ImageUrl = PayPalExpressButtonImageUrl;
            btnGoogleCheckout.UseHttps = SiteUtils.SslIsAvailable();

            btnFreeCheckout.Text = WebStoreResources.CompleteOrderButton;

            Title = SiteUtils.FormatPageTitle(siteSettings, CurrentPage.PageName + " - " + WebStoreResources.Checkout);

            lnkCart.PageID = pageId;
            lnkCart.ModuleID = moduleId;

        }

        

        private void ConfigureCheckoutButtons()
        {
            bool shouldShowPayPal = ShouldShowPayPal();
            bool shouldShowGoogle = ShouldShowGoogle();

            btnPayPal.Visible = shouldShowPayPal;
            btnGoogleCheckout.Visible = shouldShowGoogle;
            //litOr.Visible = (shouldShowPayPal || shouldShowGoogle);

            if (shouldShowGoogle)
            {
                if ((!commerceConfig.Is503TaxExempt) && (cart.HasDonations()))
                {
                    //lblGoogleMessage.Text = WebStoreResources.GoogleCheckoutDisabledForDonationsMessage;
                    //lblGoogleMessage.Visible = true;
                    btnGoogleCheckout.Visible = false;
                    //if (!Request.IsAuthenticated)
                    //{
                    //    PaymentAcceptanceMark mark = (PaymentAcceptanceMark)pam1;
                    //    mark.SuppressGoogleCheckout = true;
                    //}
                }

            }

            if ((shouldShowPayPal) && (commerceConfig.PayPalUsePayPalStandard))
            {
                SetupPayPalStandardForm();
            }


        }

        private void LoadSettings()
        {
            store = StoreHelper.GetStore();
            if (store == null) { return; }

            commerceConfig = SiteUtils.GetCommerceConfig();
            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);

            if (Request.IsAuthenticated)
            {
                siteUser = SiteUtils.GetCurrentSiteUser();
            }

            if (StoreHelper.UserHasCartCookie(store.Guid))
            {
                cart = StoreHelper.GetCart();
                if (cart != null)
                {
                    cartOffers = cart.GetOffers();

                    canCheckoutWithoutAuthentication = store.CanCheckoutWithoutAuthentication(cart);

                    if ((cart.LastModified < DateTime.UtcNow.AddDays(-1)) && (cart.DiscountCodesCsv.Length > 0))
                    {
                        StoreHelper.EnsureValidDiscounts(store, cart);
                    }

                    if ((cart.UserGuid == Guid.Empty)&&(siteUser != null))
                    {
                        cart.UserGuid = siteUser.UserGuid;
                        cart.Save();
                    }
                    cart.RefreshTotals();
                }
            }

            ConfigureCheckoutButtons();

            AddClassToBody("webstore webstorecheckout");

        }

        private bool ShouldShowPayPal()
        {
            if (store == null) { return false; }
            if (cart == null) { return false; }
            if (cart.SubTotal == 0) { return false; }
            if (cart.OrderTotal == 0) { return false; }
            if (commerceConfig == null) { return false; }
            if ((!Request.IsAuthenticated) && (!canCheckoutWithoutAuthentication)) { return false; }
            if (!commerceConfig.PayPalIsEnabled) { return false; }



            return true;
        }

        private bool ShouldShowGoogle()
        {
            if (store == null) { return false; }
            if (cart == null) { return false; }
            if (cart.SubTotal == 0) { return false; }
            if (cart.OrderTotal == 0) { return false; }
            if (commerceConfig == null) { return false; }
            if ((!Request.IsAuthenticated) && (!canCheckoutWithoutAuthentication)) { return false; }
            if (!commerceConfig.GoogleCheckoutIsEnabled) { return false; }

            // google checkout goes away 2013-11-20
            DateTime endDate = new DateTime(2013, 11, 20);
            if (DateTime.UtcNow > endDate) { return false; }

            if ((cart.HasDonations()) && (!commerceConfig.Is503TaxExempt)) { return false; }

            return true;
        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", true, moduleId);
            if (moduleId == -1)
            {
                moduleId = StoreHelper.FindStoreModuleId(CurrentPage);
            }

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

            this.btnMakePayment.Click += new EventHandler(btnMakePayment_Click);

            this.btnGoogleCheckout.Click += new ImageClickEventHandler(btnGoogleCheckout_Click);
            this.btnPayPal.Click += new ImageClickEventHandler(btnPayPal_Click);
            btnFreeCheckout.Click += new EventHandler(btnFreeCheckout_Click);

            SuppressPageMenu();
            SuppressGoogleAds();

            
        }

        

        

        

        #endregion
    }
}
