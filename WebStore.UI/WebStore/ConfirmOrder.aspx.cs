/// Author:					Joe Audette
/// Created:				2007-02-13
/// Last Modified:		    2015-04-13 (Joe Davis)
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
using System.Collections;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.Commerce;
using mojoPortal.Business.WebHelpers.PaymentGateway;
using WebStore.Business;
using Resources;
using WebStore.Helpers;
using GCheckout.Checkout;
using GCheckout.Util;

namespace WebStore.UI
{

    public partial class ConfirmOrderPage : NonCmsBasePage
    {
        protected int pageId = -1;
        protected int moduleId = -1;
        protected Store store;
        protected GeoCountry storeCountry;
        protected Cart cart = null;
        protected SiteUser siteUser = null;
        protected DataTable tblCountryList = null;
        protected ArrayList checkoutErrors;
        private CommerceConfiguration commerceConfig = null;
        protected CultureInfo currencyCulture;
        private bool canCheckoutWithoutAuthentication = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SiteUtils.SslIsAvailable()) { SiteUtils.ForceSsl(); }

            LoadParams();

            if (!UserCanViewPage(moduleId, Store.FeatureGuid))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            //not using this yet. Not sure if there is a need for different billing from customer info
            pnlBillingInfo.Visible = false;

            //will be shown later if shippable products are in cart
            pnlShipping.Visible = false;

            LoadSettings();

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
            if (!Page.IsPostBack)
            {
                BindBillingCountryList();
                BindShippingCountryList();
                BindCustomerCountryList();
                ShowCart();
            }

        }

        private void ShowCart()
        {
            if (cart == null) { return; }

            
            if (cart.HasShippingProducts())
            {
                pnlShipping.Visible = true;
            }

            using (IDataReader reader = cart.GetItems())
            {
                rptCartItems.DataSource = reader;
                rptCartItems.DataBind();
            }

            litSubTotal.Text = cart.SubTotal.ToString("c", currencyCulture);
            litDiscount.Text = cart.Discount.ToString("c", currencyCulture);
            litShippingTotal.Text = cart.ShippingTotal.ToString("c", currencyCulture);
            litTaxTotal.Text = cart.TaxTotal.ToString("c", currencyCulture);
            litOrderTotal.Text = cart.OrderTotal.ToString("c", currencyCulture);

            pnlDiscount.Visible = (cart.Discount > 0);
            pnlShippingTotal.Visible = (cart.ShippingTotal > 0);
            pnlOrderTotal.Visible = (cart.OrderTotal > 0);
            pnlTaxTotal.Visible = (cart.TaxTotal > 0);
            
            if ((cart.ShippingTotal == 0) && (cart.TaxTotal == 0) && (cart.Discount == 0))
            {
                pnlSubTotal.Visible = false;
            }

            ListItem listItem;

            if (cart.OrderInfo == null)
            {
                listItem = ddBillingCountry.Items.FindByValue(storeCountry.IsoCode2);
                if (listItem != null)
                {
                    ddBillingCountry.ClearSelection();
                    listItem.Selected = true;
                    BindBillingGeoZoneList();
                }

                listItem = ddCustomerCountry.Items.FindByValue(storeCountry.IsoCode2);
                if (listItem != null)
                {
                    ddCustomerCountry.ClearSelection();
                    listItem.Selected = true;
                    BindCustomerGeoZoneList();
                }

            }

            if (cart.OrderInfo != null)
            {
                txtBillingFirstName.Text = cart.OrderInfo.BillingFirstName;
                txtBillingLastName.Text = cart.OrderInfo.BillingLastName;
                txtBillingCompany.Text = cart.OrderInfo.BillingCompany;
                txtBillingAddress1.Text = cart.OrderInfo.BillingAddress1;
                txtBillingAddress2.Text = cart.OrderInfo.BillingAddress2;
                txtBillingSuburb.Text = cart.OrderInfo.BillingSuburb;
                txtBillingCity.Text = cart.OrderInfo.BillingCity;
                txtBillingPostalCode.Text = cart.OrderInfo.BillingPostalCode;

                listItem = ddBillingCountry.Items.FindByValue(cart.OrderInfo.BillingCountry.Trim());
                if (listItem != null)
                {
                    ddBillingCountry.ClearSelection();
                    listItem.Selected = true;
                    BindBillingGeoZoneList();

                    listItem = ddBillingGeoZone.Items.FindByValue(cart.OrderInfo.BillingState.Trim());
                    if (listItem != null)
                    {
                        ddBillingGeoZone.ClearSelection();
                        listItem.Selected = true;
                    }
                }
                else
                {
                    if (storeCountry != null)
                    {
                        listItem = ddBillingCountry.Items.FindByValue(storeCountry.IsoCode2);
                        if (listItem != null)
                        {
                            ddBillingCountry.ClearSelection();
                            listItem.Selected = true;
                            BindBillingGeoZoneList();
                        }
                    }
                }

                txtCustomerFirstName.Text = cart.OrderInfo.CustomerFirstName;
                txtCustomerLastName.Text = cart.OrderInfo.CustomerLastName;
                txtCustomerCompany.Text = cart.OrderInfo.CustomerCompany;
                txtCustomerAddressLine1.Text = cart.OrderInfo.CustomerAddressLine1;
                txtCustomerAddressLine2.Text = cart.OrderInfo.CustomerAddressLine2;
                txtCustomerSuburb.Text = cart.OrderInfo.CustomerSuburb;
                txtCustomerCity.Text = cart.OrderInfo.CustomerCity;
                txtCustomerPostalCode.Text = cart.OrderInfo.CustomerPostalCode;

                listItem = ddCustomerCountry.Items.FindByValue(cart.OrderInfo.CustomerCountry.Trim());
                if (listItem != null)
                {
                    ddCustomerCountry.ClearSelection();
                    listItem.Selected = true;
                    BindCustomerGeoZoneList();

                    listItem = ddCustomerGeoZone.Items.FindByValue(cart.OrderInfo.CustomerState.Trim());
                    if (listItem != null)
                    {
                        ddCustomerGeoZone.ClearSelection();
                        listItem.Selected = true;
                    }
                }
                else
                {
                    listItem = ddCustomerCountry.Items.FindByValue(storeCountry.IsoCode2);
                    if (listItem != null)
                    {
                        ddCustomerCountry.ClearSelection();
                        listItem.Selected = true;
                        BindCustomerGeoZoneList();
                    }
                }

                txtCustomerTelephoneDay.Text = cart.OrderInfo.CustomerTelephoneDay;
                txtCustomerTelephoneNight.Text = cart.OrderInfo.CustomerTelephoneNight;
                txtCustomerEmail.Text = cart.OrderInfo.CustomerEmail;

                txtDeliveryFirstName.Text = cart.OrderInfo.DeliveryFirstName;
                txtDeliveryLastName.Text = cart.OrderInfo.DeliveryLastName;
                txtDeliveryCompany.Text = cart.OrderInfo.DeliveryCompany;
                txtDeliveryAddress1.Text = cart.OrderInfo.DeliveryAddress1;
                txtDeliveryAddress2.Text = cart.OrderInfo.DeliveryAddress2;
                txtDeliverySuburb.Text = cart.OrderInfo.DeliverySuburb;
                txtDeliveryCity.Text = cart.OrderInfo.DeliveryCity;
                txtDeliveryPostalCode.Text = cart.OrderInfo.DeliveryPostalCode;

                listItem = ddDeliveryCountry.Items.FindByValue(cart.OrderInfo.DeliveryCountry.Trim());
                if (listItem != null)
                {
                    ddDeliveryCountry.ClearSelection();
                    listItem.Selected = true;
                    BindShippingGeoZoneList();

                    listItem = ddDeliveryGeoZone.Items.FindByValue(cart.OrderInfo.DeliveryState.Trim());
                    if (listItem != null)
                    {
                        ddDeliveryGeoZone.ClearSelection();
                        listItem.Selected = true;

                    }
                }
                else
                {
                    listItem = ddDeliveryCountry.Items.FindByValue(siteSettings.DefaultCountryGuid.ToString());
                    if (listItem != null)
                    {
                        ddDeliveryCountry.ClearSelection();
                        listItem.Selected = true;
                        BindShippingGeoZoneList();
                    }
                }

                if (store.IsValidForCheckout(cart, out checkoutErrors))
                {
                    btnContinue.Enabled = true;
                }
                else
                {
                    btnContinue.Enabled = false;
                }

            }

            Page.Validate("OrderInfo");

            btnContinue.Enabled
                = (
                (Page.IsValid)
                && (store.IsValidForCheckout(cart, out checkoutErrors))
                );
            
        }

        private void BindBillingCountryList()
        {
            if (tblCountryList != null)
            {
                ddBillingCountry.DataSource = tblCountryList;
                ddBillingCountry.DataBind();
            }

        }

        private void BindBillingGeoZoneList()
        {
            if ((ddBillingCountry.SelectedIndex > -1)
                && (ddBillingCountry.SelectedValue.Length > 0)
                )
            {
                GeoCountry country = new GeoCountry(ddBillingCountry.SelectedValue);
                if (country.Guid == Guid.Empty) return;

                using (IDataReader reader = GeoZone.GetByCountry(country.Guid))
                {
                    ddBillingGeoZone.DataSource = reader;
                    ddBillingGeoZone.DataBind();
                }

                divBillingState.Visible = (ddBillingGeoZone.Items.Count > 0);

            }
        }

        private void BindShippingCountryList()
        {
            if (tblCountryList != null)
            {
                ddDeliveryCountry.DataSource = tblCountryList;
                ddDeliveryCountry.DataBind();
            }
        }

        private void BindShippingGeoZoneList()
        {
            if ((ddDeliveryCountry.SelectedIndex > -1)
                && (ddDeliveryCountry.SelectedValue.Length > 0)
                )
            {
                GeoCountry country = new GeoCountry(ddDeliveryCountry.SelectedValue);
                if (country.Guid == Guid.Empty) return;

                using (IDataReader reader = GeoZone.GetByCountry(country.Guid))
                {
                    ddDeliveryGeoZone.DataSource = reader;
                    ddDeliveryGeoZone.DataBind();
                }

                divShippingState.Visible = (ddDeliveryGeoZone.Items.Count > 0);


            }
        }

        private void BindCustomerCountryList()
        {
            if (tblCountryList != null)
            {
                ddCustomerCountry.DataSource = tblCountryList;
                ddCustomerCountry.DataBind();
            }

        }

        private void BindCustomerGeoZoneList()
        {
            if ((ddCustomerCountry.SelectedIndex > -1)
                && (ddCustomerCountry.SelectedValue.Length > 0)
                )
            {
                GeoCountry country = new GeoCountry(ddCustomerCountry.SelectedValue);
                if (country.Guid == Guid.Empty) return;

                using (IDataReader reader = GeoZone.GetByCountry(country.Guid))
                {
                    ddCustomerGeoZone.DataSource = reader;
                    ddCustomerGeoZone.DataBind();
                }

                divCustomerState.Visible = (ddCustomerGeoZone.Items.Count > 0);
            }

        }

        private void ddDeliveryCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindShippingGeoZoneList();
        }

        private void ddBillingCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindBillingGeoZoneList();
        }

        private void ddCustomerCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCustomerGeoZoneList();
        }

        private void SaveCartDetail()
        {
            if (cart == null) { return; }

            GeoCountry country;
            GeoZone taxZone;

            cart.OrderInfo.CustomerFirstName = txtCustomerFirstName.Text;
            cart.OrderInfo.CustomerLastName = txtCustomerLastName.Text;
            cart.OrderInfo.CustomerCompany = txtCustomerCompany.Text;
            cart.OrderInfo.CustomerAddressLine1 = txtCustomerAddressLine1.Text;
            cart.OrderInfo.CustomerAddressLine2 = txtCustomerAddressLine2.Text;
            cart.OrderInfo.CustomerSuburb = txtCustomerSuburb.Text;
            cart.OrderInfo.CustomerCity = txtCustomerCity.Text;
            cart.OrderInfo.CustomerPostalCode = txtCustomerPostalCode.Text;
            if (ddCustomerGeoZone.SelectedIndex > -1)
            {
                cart.OrderInfo.CustomerState = ddCustomerGeoZone.SelectedItem.Value;
            }
            else
            {
                cart.OrderInfo.CustomerState = string.Empty;
            }
            cart.OrderInfo.CustomerCountry = ddCustomerCountry.SelectedItem.Value;
            cart.OrderInfo.CustomerTelephoneDay = txtCustomerTelephoneDay.Text;
            cart.OrderInfo.CustomerTelephoneNight = txtCustomerTelephoneNight.Text;
            cart.OrderInfo.CustomerEmail = txtCustomerEmail.Text;

            if (cart.OrderInfo.CustomerState.Length > 0) 
            {
                country = new GeoCountry(cart.OrderInfo.CustomerCountry);
                taxZone = GeoZone.GetByCode(country.Guid, cart.OrderInfo.CustomerState);

                cart.OrderInfo.TaxZoneGuid = taxZone.Guid;
            }

            if (cart.HasShippingProducts())
            {
                cart.OrderInfo.DeliveryFirstName = txtDeliveryFirstName.Text;
                cart.OrderInfo.DeliveryLastName = txtDeliveryLastName.Text;
                cart.OrderInfo.DeliveryCompany = txtDeliveryCompany.Text;
                cart.OrderInfo.DeliveryAddress1 = txtDeliveryAddress1.Text;
                cart.OrderInfo.DeliveryAddress2 = txtDeliveryAddress2.Text;
                cart.OrderInfo.DeliverySuburb = txtDeliverySuburb.Text;
                cart.OrderInfo.DeliveryCity = txtDeliveryCity.Text;
                cart.OrderInfo.DeliveryPostalCode = txtDeliveryPostalCode.Text;
                if (ddDeliveryGeoZone.SelectedIndex > -1)
                {
                    cart.OrderInfo.DeliveryState = ddDeliveryGeoZone.SelectedItem.Value;
                }
                else
                {
                    cart.OrderInfo.DeliveryState = string.Empty;
                }

                cart.OrderInfo.DeliveryCountry = ddDeliveryCountry.SelectedItem.Value;

                if(ddDeliveryGeoZone.SelectedIndex > -1)
                {
                    country = new GeoCountry(cart.OrderInfo.DeliveryCountry);
                    taxZone = GeoZone.GetByCode(country.Guid, ddDeliveryGeoZone.SelectedValue);

                    cart.OrderInfo.TaxZoneGuid = taxZone.Guid;
                }

            }
            else
            {
                cart.OrderInfo.DeliveryFirstName = txtCustomerFirstName.Text;
                cart.OrderInfo.DeliveryLastName = txtCustomerLastName.Text;
                cart.OrderInfo.DeliveryCompany = txtCustomerCompany.Text;
                cart.OrderInfo.DeliveryAddress1 = txtCustomerAddressLine1.Text;
                cart.OrderInfo.DeliveryAddress2 = txtCustomerAddressLine2.Text;
                cart.OrderInfo.DeliverySuburb = txtCustomerSuburb.Text;
                cart.OrderInfo.DeliveryCity = txtCustomerCity.Text;
                cart.OrderInfo.DeliveryPostalCode = txtCustomerPostalCode.Text;
                if (ddCustomerGeoZone.SelectedIndex > -1)
                {
                    cart.OrderInfo.DeliveryState = ddCustomerGeoZone.SelectedItem.Value;
                }
                else
                {
                    cart.OrderInfo.DeliveryState = string.Empty;
                }

                cart.OrderInfo.DeliveryCountry = ddCustomerCountry.SelectedItem.Value;
            }

            if (pnlBillingInfo.Visible)
            {
                cart.OrderInfo.BillingFirstName = txtBillingFirstName.Text;
                cart.OrderInfo.BillingLastName = txtBillingLastName.Text;
                cart.OrderInfo.BillingCompany = txtBillingCompany.Text;
                cart.OrderInfo.BillingAddress1 = txtBillingAddress1.Text;
                cart.OrderInfo.BillingAddress2 = txtBillingAddress2.Text;
                cart.OrderInfo.BillingSuburb = txtBillingSuburb.Text;
                cart.OrderInfo.BillingCity = txtBillingCity.Text;
                cart.OrderInfo.BillingPostalCode = txtBillingPostalCode.Text;
                if (ddBillingGeoZone.SelectedIndex > -1)
                {
                    cart.OrderInfo.BillingState = ddBillingGeoZone.SelectedItem.Value;
                }
                else
                {
                    cart.OrderInfo.BillingState = string.Empty;
                }
                cart.OrderInfo.BillingCountry = ddBillingCountry.SelectedItem.Value;
            }
            else
            {
                cart.OrderInfo.BillingFirstName = txtCustomerFirstName.Text;
                cart.OrderInfo.BillingLastName = txtCustomerLastName.Text;
                cart.OrderInfo.BillingCompany = txtCustomerCompany.Text;
                cart.OrderInfo.BillingAddress1 = txtCustomerAddressLine1.Text;
                cart.OrderInfo.BillingAddress2 = txtCustomerAddressLine2.Text;
                cart.OrderInfo.BillingSuburb = txtCustomerSuburb.Text;
                cart.OrderInfo.BillingCity = txtCustomerCity.Text;
                cart.OrderInfo.BillingPostalCode = txtCustomerPostalCode.Text;
                if (ddCustomerGeoZone.SelectedIndex > -1)
                {
                    cart.OrderInfo.BillingState = ddCustomerGeoZone.SelectedItem.Value;
                }
                else
                {
                    cart.OrderInfo.BillingState = string.Empty;
                }

                cart.OrderInfo.BillingCountry = ddCustomerCountry.SelectedItem.Value;
            }

            cart.OrderInfo.CompletedFromIP = SiteUtils.GetIP4Address();
            cart.OrderInfo.Save();
            cart.RefreshTotals();


        }

        private void btnSaveAndValidate_Click(object sender, EventArgs e)
        {
            Page.Validate("OrderInfo");
            if (Page.IsValid)
            {
                SaveCartDetail();

                WebUtils.SetupRedirect(this, Request.RawUrl);
            }
            else
            {
                btnContinue.Enabled = false;

            }
        }

        private void lnkCopyBillingToShipping_Click(object sender, EventArgs e)
        {
            if (cart == null) { return; }

            SaveCartDetail();
            cart.CopyBillingToShipping();
            //cart.OrderInfo.Save();
            cart.RefreshTotals();
            WebUtils.SetupRedirect(this, Request.RawUrl);
            

        }

        private void lnkCopyCustomerToShipping_Click(object sender, EventArgs e)
        {
            if (cart == null) { return; }

            SaveCartDetail();
            cart.CopyCustomerToShipping();
            //cart.OrderInfo.Save();
            cart.RefreshTotals();
            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        private void lnkCopyCustomerToBilling_Click(object sender, EventArgs e)
        {
            if (cart == null) { return; }

            SaveCartDetail();
            cart.CopyShippingToBilling();
            //cart.OrderInfo.Save();
            cart.RefreshTotals();
            
            WebUtils.SetupRedirect(this, Request.RawUrl);
            return;

            

        }


        private void btnContinue_Click(object sender, System.EventArgs e)
        {
            SaveCartDetail();

            Page.Validate();
            if (Page.IsValid)
            {
                if (store.IsValidForCheckout(cart, out checkoutErrors))
                {
                    string checkoutUrl = SiteRoot + "/WebStore/Checkout.aspx?pageid="
                        + pageId.ToInvariantString()
                        + "&mid=" + moduleId.ToInvariantString();

                    WebUtils.SetupRedirect(this, checkoutUrl);
                    return;
                }
                else
                {
                    ShowCheckoutErrors();
                }
            }
            else
            {
                btnContinue.Enabled = false;
            }

        }

        private void ShowCheckoutErrors()
        {
            if (checkoutErrors != null)
            {



            }


        }

        void btnPayPal_Click(object sender, ImageClickEventArgs e)
        {
            if (commerceConfig.PayPalUsePayPalStandard)
            {
                DoPayPalStandardCheckout();
            }
            else
            {
                DoPayPalExpressCeckout();
            }
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
            //gateway.CurrencyCode = currency.Code;
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

            }

        }

        /// <summary>
        /// This is really a fallback method, it will not be executed in normal use because we are setting the postbackurl of the button
        /// to post directly to paypal, so this method should not fire unless somehow the page is manipulated to postback to itself.
        /// In this case, we just consolidate the cart into a buy now button.
        /// </summary>
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
            if (cart == null) { return; }
            if (siteUser == null) { return; }

            if (cart.UserGuid == Guid.Empty)
            {
                cart.UserGuid = siteUser.UserGuid;
                cart.Save();
            }

            if (cart.CartOffers.Count == 0)
            {
                litOr.Visible = false;
                btnPayPal.Visible = false;
                btnGoogleCheckout.Visible = false;
                return;
            }

            PayPalLog payPalLog = StoreHelper.EnsurePayPalStandardCheckoutLog(cart, store, SiteRoot, pageId, moduleId);
            if (payPalLog == null) return;

            litPayPalFormVariables.Text = StoreHelper.GetCartUploadFormFields(
                payPalLog.RowGuid,
                cart,
                store,
                commerceConfig);

            btnPayPal.PostBackUrl = commerceConfig.PayPalStandardUrl;
        }

        void btnGoogleCheckout_Click(object sender, ImageClickEventArgs e)
        {
            if (
                (store != null)
                && (cart != null)
                )
            { //&& (IsValidForCheckout()) ?

                int cartTimeoutInMinutes = 30;
                //string currency = "USD";
                //CheckoutShoppingCartRequest Req = btnGoogleCheckout.CreateRequest();
                CheckoutShoppingCartRequest Req = new CheckoutShoppingCartRequest(
                    commerceConfig.GoogleMerchantID,
                    commerceConfig.GoogleMerchantKey,
                    commerceConfig.GoogleEnvironment,
                    siteSettings.GetCurrency().Code,
                    cartTimeoutInMinutes);


                foreach (CartOffer cartOffer in cart.CartOffers)
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


        private void PopulateLabels()
        {
            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
               
            }
            
            Title = SiteUtils.FormatPageTitle(siteSettings, CurrentPage.PageName + " - " + WebStoreResources.ConfirmOrderHeading);
            heading.Text = WebStoreResources.ConfirmOrderHeading;
            litCartHeader.Text = WebStoreResources.CartHeader;
            
            SignInOrRegisterPrompt signinPrompt = srPrompt as SignInOrRegisterPrompt;
            if (canCheckoutWithoutAuthentication)
            {
                signinPrompt.Instructions = WebStoreResources.CheckoutSuggestLoginOrRegisterMessage;
            }
            else
            {
                signinPrompt.Instructions = WebStoreResources.CheckoutRequiresLoginOrRegisterMessage;
            }
        
            litOr.Text = WebStoreResources.LiteralOr;
            lnkCopyCustomerToBilling.Text = WebStoreResources.CopyCustomerToBillingLink;
            lnkCopyCustomerToBilling.ToolTip = WebStoreResources.CopyCustomerToBillingLink;
            lnkCopyCustomerToShipping.Text = WebStoreResources.CopyCustomerToShippingLink;
            lnkCopyCustomerToShipping.ToolTip = WebStoreResources.CopyCustomerToShippingLink;
            lnkCopyBillingToShipping.Text = WebStoreResources.CopyBillingToShippingLink;
            lnkCopyBillingToShipping.ToolTip = WebStoreResources.CopyBillingToShippingLink;

            reqBillingAddress1.Text = WebStoreResources.RequiredFieldSymbol;
            reqBillingAddress1.ErrorMessage = WebStoreResources.BillingAddress1RequiredMessage;
            reqBillingCity.Text = WebStoreResources.RequiredFieldSymbol;
            reqBillingCity.ErrorMessage = WebStoreResources.BillingCityRequiredMessage;
            reqBillingFirstName.Text = WebStoreResources.RequiredFieldSymbol;
            reqBillingFirstName.ErrorMessage = WebStoreResources.BillingFirstNameRequiredMessage;
            reqBillingLastName.Text = WebStoreResources.RequiredFieldSymbol;
            reqBillingLastName.ErrorMessage = WebStoreResources.BillingLastNameRequiredMessage;
            reqBillingPostalCode.Text = WebStoreResources.RequiredFieldSymbol;
            reqBillingPostalCode.ErrorMessage = WebStoreResources.BillingPostalCodeRequiredMessage;

            reqCustomerAddress1.Text = WebStoreResources.RequiredFieldSymbol;
            reqCustomerAddress1.ErrorMessage = WebStoreResources.CustomerAddress1RequiredMessage;
            reqCustomerCity.Text = WebStoreResources.RequiredFieldSymbol;
            reqCustomerCity.ErrorMessage = WebStoreResources.CustomerCityRequiredMessage;
            reqCustomerDayPhone.Text = WebStoreResources.RequiredFieldSymbol;
            reqCustomerDayPhone.ErrorMessage = WebStoreResources.CustomerDayPhoneRequiredMessage;
            reqCustomerEmail.Text = WebStoreResources.RequiredFieldSymbol;
            reqCustomerEmail.ErrorMessage = WebStoreResources.CustomerEmailRequiredMessage;
            reqCustomerFirstName.Text = WebStoreResources.RequiredFieldSymbol;
            reqCustomerFirstName.ErrorMessage = WebStoreResources.CustomerFirstNameRequiredMessage;
            reqCustomerLastName.Text = WebStoreResources.RequiredFieldSymbol;
            reqCustomerLastName.ErrorMessage = WebStoreResources.CustomerLastNameRequiredMessage;
            reqCustomerPostalCode.Text = WebStoreResources.RequiredFieldSymbol;
            reqCustomerPostalCode.ErrorMessage = WebStoreResources.CustomerPostalCodeRequiredMessage;

            reqDeliveryAddress1.Text = WebStoreResources.RequiredFieldSymbol;
            reqDeliveryAddress1.ErrorMessage = WebStoreResources.DeliveryAddress1RequiredMessage;
            reqDeliveryCity.Text = WebStoreResources.RequiredFieldSymbol;
            reqDeliveryCity.ErrorMessage = WebStoreResources.DeliveryCityRequiredMessage;
            reqDeliveryFirstName.Text = WebStoreResources.RequiredFieldSymbol;
            reqDeliveryFirstName.ErrorMessage = WebStoreResources.DeliveryFirstNameRequiredMessage;
            reqDeliveryLastName.Text = WebStoreResources.RequiredFieldSymbol;
            reqDeliveryLastName.ErrorMessage = WebStoreResources.DeliveryLastNameRequiredMessage;
            reqDeliveryPostalCode.Text = WebStoreResources.RequiredFieldSymbol;
            reqDeliveryPostalCode.ErrorMessage = WebStoreResources.DeliveryPostalCodeRequiredMessage;

            regexCustomerEmail.Text = WebStoreResources.RequiredFieldSymbol;
            regexCustomerEmail.ErrorMessage = WebStoreResources.CustomerEmailRegexFailureMessage;
            
            btnSaveAndValidate.Text = WebStoreResources.UpdateCustomerButton;
            btnContinue.Text = WebStoreResources.ContinueButton;
            btnGoogleCheckout.UseHttps = SiteUtils.SslIsAvailable();

            lnkCart.PageID = pageId;
            lnkCart.ModuleID = moduleId;


        }

        

        private void ConfigureCheckoutButtons()
        {
            bool shouldShowPayPal = ShouldShowPayPal();
            bool shouldShowGoogle = ShouldShowGoogle();

            btnPayPal.Visible = shouldShowPayPal;
            btnGoogleCheckout.Visible = shouldShowGoogle;
            litOr.Visible = (shouldShowPayPal || shouldShowGoogle);

            if (shouldShowGoogle)
            {
                if ((!commerceConfig.Is503TaxExempt) && (cart.HasDonations()))
                {
                    lblGoogleMessage.Text = WebStoreResources.GoogleCheckoutDisabledForDonationsMessage;
                    lblGoogleMessage.Visible = true;
                    btnGoogleCheckout.Enabled = false;
                    if (!Request.IsAuthenticated)
                    {
                        PaymentAcceptanceMark mark = (PaymentAcceptanceMark)pam1;
                        mark.SuppressGoogleCheckout = true;
                    }
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
            storeCountry = new GeoCountry(siteSettings.DefaultCountryGuid);

            if (Request.IsAuthenticated)
            {
                siteUser = SiteUtils.GetCurrentSiteUser();
            }

            if (StoreHelper.UserHasCartCookie(store.Guid))
            {
                cart = StoreHelper.GetCart();

                if (cart != null)
                {
                    if ((cart.LastModified < DateTime.UtcNow.AddDays(-1)) && (cart.DiscountCodesCsv.Length > 0))
                    {
                        StoreHelper.EnsureValidDiscounts(store, cart);
                    }

                    if (siteUser != null)
                    {
                        if (cart.UserGuid == Guid.Empty)
                        {
                            // take ownership of anonymous cart
                            cart.UserGuid = siteUser.UserGuid;
                            cart.Save();
                            StoreHelper.InitializeOrderInfo(cart, siteUser);
                        }
                        else
                        {
                            // cart already has a user guid but 
                            // check if it matches the current user
                            // cart cookie could have been left behind by a previous user
                            // on shared computers
                            // if cart user guid doesn't match reset billing shipping info
                            // and any other identifiers
                            // but leave items in cart
                            if (cart.UserGuid != siteUser.UserGuid)
                            {
                                cart.ResetUserInfo();
                                cart.UserGuid = siteUser.UserGuid;
                                cart.Save();
                                StoreHelper.InitializeOrderInfo(cart, siteUser);
                            }
                        }
                    }

                    if (WebStoreConfiguration.IsDemo)
                    {
                        LoadDemoCustomer();
                    }

                    
                    canCheckoutWithoutAuthentication = store.CanCheckoutWithoutAuthentication(cart);
                    

                    // disable till I finish
                    //canCheckoutWithoutAuthentication = false;

                }

                AddClassToBody("webstore webstoreconfirmorder");
            }


            pnlRequireLogin.Visible = !Request.IsAuthenticated;
            
            if ((canCheckoutWithoutAuthentication)||(Request.IsAuthenticated))
            {

                pnlOrderDetail.Visible = commerceConfig.CanProcessStandardCards || (commerceConfig.WorldPayInstallationId.Length > 0);
                pnlRequireLogin.Visible = true;
                pnlShippingTotal.Visible = false;
                pnlTaxTotal.Visible = false;
                pnlOrderTotal.Visible = false;

            }

            if ((cart != null) && (cart.SubTotal == 0) && (cart.CartOffers.Count > 0))
            {
                // free checkout
                pnlOrderDetail.Visible = true;
            }

            if (pnlOrderDetail.Visible) { tblCountryList = GeoCountry.GetList(); }

           
            ConfigureCheckoutButtons();
            
            //if (!Page.IsPostBack)
            //{
            //    if ((commerceConfig.PayPalIsEnabled) && (commerceConfig.PayPalUsePayPalStandard))
            //    {
            //        if (Request.IsAuthenticated)
            //        {
            //            SetupPayPalStandardForm();
            //        }
            //        else
            //        {
            //            // we need the user to be signed in before we send them to paypal if using PayPal Standard
            //            // because we want to return them to their order summary and that requires login
            //            // so we need to know who the user is before sending them to PayPal
            //            litOr.Visible = false;
            //            btnPayPal.Visible = false;
            //            btnGoogleCheckout.Visible = false;
            //        }
            //    }
            //}

            //if (!Request.IsAuthenticated)
            //{
               
            //    pnlOrderDetail.Visible = false;
            //    pnlRequireLogin.Visible = true;
            //    pnlShippingTotal.Visible = false;
            //    pnlTaxTotal.Visible = false;
            //    pnlOrderTotal.Visible = false;

            //    if (commerceConfig.GoogleCheckoutIsEnabled)
            //    {
            //        if (
            //        (!commerceConfig.Is503TaxExempt)
            //        && ((cart != null) && (cart.HasDonations()))
            //        )
            //        {
            //            //btnGoogleCheckout.Visible = false;
            //            lblGoogleMessage.Text = WebStoreResources.GoogleCheckoutDisabledForDonationsMessage;
            //            lblGoogleMessage.Visible = true;
            //            PaymentAcceptanceMark mark = (PaymentAcceptanceMark)pam1;
            //            mark.SuppressGoogleCheckout = true;

            //            mark = (PaymentAcceptanceMark)PaymentAcceptanceMark1;
            //            mark.SuppressGoogleCheckout = true;

            //            btnGoogleCheckout.Visible = true;
            //            btnGoogleCheckout.Enabled = false;
            //        }
            //    }

            //}
            //else
            //{
            //    if (
            //        (!commerceConfig.Is503TaxExempt)
            //         && ((cart != null)&& (cart.HasDonations()))
            //        && (commerceConfig.GoogleCheckoutIsEnabled)
            //        )
            //    {
            //        btnGoogleCheckout.Visible = true;
            //        btnGoogleCheckout.Enabled = false;
            //        lblGoogleMessage.Text = WebStoreResources.GoogleCheckoutDisabledForDonationsMessage;
            //        lblGoogleMessage.Visible = true;
            //    }


            //}

        }

        private void LoadDemoCustomer()
        {
            // this is for the demo site so users don't have to fill out customer info to try the store

            if (cart.OrderInfo.CustomerFirstName.Length > 0) { return; } //data already initialized

            GeoCountry country;
            GeoZone taxZone;

            cart.OrderInfo.CustomerFirstName = "John";
            cart.OrderInfo.CustomerLastName = "Doe";
            cart.OrderInfo.CustomerEmail = "admin@admin.com";

            cart.OrderInfo.CustomerAddressLine1 = "123 Any St.";

            cart.OrderInfo.CustomerCity = "Anytown";
            cart.OrderInfo.CustomerPostalCode = "12345";

            cart.OrderInfo.CustomerState = "NC";
            
            cart.OrderInfo.CustomerCountry = "US";
            cart.OrderInfo.CustomerTelephoneDay = "123-234-3456";
            
            if (cart.OrderInfo.CustomerState.Length > 0)
            {
                country = new GeoCountry(cart.OrderInfo.CustomerCountry);
                taxZone = GeoZone.GetByCode(country.Guid, cart.OrderInfo.CustomerState);
                if (taxZone != null)
                {
                    cart.OrderInfo.TaxZoneGuid = taxZone.Guid;
                }
            }

            cart.CopyCustomerToShipping();
            cart.CopyCustomerToBilling();

            
            cart.OrderInfo.Save();
            cart.RefreshTotals();

        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", true, moduleId);

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

            return true;
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
            this.btnSaveAndValidate.Click += new EventHandler(btnSaveAndValidate_Click);
            this.btnContinue.Click += new System.EventHandler(btnContinue_Click);
            this.lnkCopyCustomerToBilling.Click += new EventHandler(lnkCopyCustomerToBilling_Click);
            this.lnkCopyCustomerToShipping.Click += new EventHandler(lnkCopyCustomerToShipping_Click);
            this.lnkCopyBillingToShipping.Click += new EventHandler(lnkCopyBillingToShipping_Click);
            this.ddCustomerCountry.SelectedIndexChanged += new EventHandler(ddCustomerCountry_SelectedIndexChanged);
            this.ddBillingCountry.SelectedIndexChanged += new EventHandler(ddBillingCountry_SelectedIndexChanged);
            this.ddDeliveryCountry.SelectedIndexChanged += new EventHandler(ddDeliveryCountry_SelectedIndexChanged);
            this.btnPayPal.Click += new ImageClickEventHandler(btnPayPal_Click);
            btnGoogleCheckout.Click += new ImageClickEventHandler(btnGoogleCheckout_Click);

            SuppressPageMenu();
            SuppressGoogleAds();

        }



        #endregion

    }
}
