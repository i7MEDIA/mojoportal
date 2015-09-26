// Author:					Joe Audette
// Created:					2009-07-28
// Last Modified:			2014-03-18
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.Commerce;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.PaymentGateway;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using WebStore.Business;
using WebStore.Helpers;



namespace WebStore.UI
{

    public partial class AdminOrderCheckoutPage : NonCmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AdminOrderCheckoutPage));
        private int pageId = -1;
        private int moduleId = -1;
        private Store store = null;
        private Cart cart = null;
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        protected DataTable tblCountryList = null;
        protected ArrayList checkoutErrors;
        protected GeoCountry storeCountry;
        private CommerceConfiguration commerceConfig = null;

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
            if (!Page.IsPostBack)
            {
                BindCustomerCountryList();
                BindShippingCountryList();
                ShowCart();
            }

        }

        private void ShowCart()
        {
            if (cart == null) { return; }


            if (!cart.HasShippingProducts())
            {
                pnlShipping.Visible = false;
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
            
            if (cart.UserGuid != Guid.Empty)
            {
                btnClearUser.Visible = true;
                divUserLink.Visible = true;

                if (WebUser.IsAdmin)
                {
                    lnkUser.Text = WebStoreResources.ManageUserLink;
                    lnkUser.NavigateUrl = SiteRoot + "/Admin/ManageUsers.aspx?u=" + cart.UserGuid.ToString();
                }
                else
                {
                    lnkUser.Text = WebStoreResources.UserProfileLink;
                    lnkUser.NavigateUrl = SiteRoot + "/ProfileView.aspx?u=" + cart.UserGuid.ToString();
                }

            }
            else
            {
                btnClearUser.Visible = false;
                divUserLink.Visible = false;
            }

            pnlDiscount.Visible = (cart.Discount > 0);
            pnlShippingTotal.Visible = (cart.ShippingTotal > 0);
            pnlTaxTotal.Visible = (cart.TaxTotal > 0);

            if ((cart.ShippingTotal == 0) && (cart.TaxTotal == 0) && (cart.Discount == 0))
            {
                pnlSubTotal.Visible = false;
            }

            ListItem listItem;

            if (cart.OrderInfo == null)
            {
            
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

            }


            Page.Validate("OrderInfo");

            if ((Page.IsValid)&&(store.IsValidForCheckout(cart, out checkoutErrors)))
            {
                pnlCheckout.Visible = true;
                PopulateYearList();
                if (!commerceConfig.CanProcessStandardCards)
                {
                    frmCardInput.Visible = false;
                }
            }

           

        }

        void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("OrderInfo");
            if (Page.IsValid)
            {
                SaveCartDetail();

                WebUtils.SetupRedirect(this, Request.RawUrl);
            }
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

            if (ddCustomerGeoZone.SelectedIndex > -1)
            {
                country = new GeoCountry(cart.OrderInfo.CustomerCountry);
                taxZone = GeoZone.GetByCode(country.Guid, ddCustomerGeoZone.SelectedValue);

                cart.OrderInfo.TaxZoneGuid = taxZone.Guid;
            }

            if (pnlShipping.Visible)
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

                if (ddDeliveryGeoZone.SelectedIndex > -1)
                {
                    country = new GeoCountry(cart.OrderInfo.DeliveryCountry);
                    taxZone = GeoZone.GetByCode(country.Guid, ddDeliveryGeoZone.SelectedValue);

                    cart.OrderInfo.TaxZoneGuid = taxZone.Guid;
                }

            }
            else
            {
                cart.CopyCustomerToShipping();
                
            }

            cart.CopyCustomerToBilling();

            cart.OrderInfo.CompletedFromIP = SiteUtils.GetIP4Address();
            cart.OrderInfo.Save();
            cart.RefreshTotals();


        }

        void btnSetUserFromGreyBox_Click(object sender, ImageClickEventArgs e)
        {
            if (cart != null)
            {
                Guid userGuid = Guid.Empty;

                if (hdnUserGuid.Value.Length == 36)
                {
                    try
                    {
                        userGuid = new Guid(hdnUserGuid.Value);
                    }
                    catch (FormatException) { }

                    SiteUser selectedUser = new SiteUser(siteSettings, userGuid);
                    if (selectedUser.UserGuid != Guid.Empty)
                    {
                        StoreHelper.InitializeOrderInfo(cart, selectedUser);
                        store.LoadCustomerInfoFromMostRecentOrder(cart);
                    }

                }


            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        void btnClearUser_Click(object sender, EventArgs e)
        {
            if (cart != null)
            {
                cart.UserGuid = Guid.Empty;
                cart.Save();
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        void btnCreateOrder_Click(object sender, EventArgs e)
        {
            if (cart.UserGuid == Guid.Empty)
            {
                SiteUser newUser = SiteUtils.CreateMinimalUser(siteSettings, cart.OrderInfo.CustomerEmail, true, WebStoreResources.UserCreatedForOrder);
                cart.UserGuid = newUser.UserGuid;
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
                txtTransactionId.Text, 
                txtAuthCode.Text, 
                siteSettings.GetCurrency().Code, 
                "OrderEntry", 
                OrderStatus.OrderStatusFulfillableGuid);
            
            
            StoreHelper.ClearClerkCartCookie(cart.StoreGuid);

            // send confirmation email
            try
            {
                if (chkOrderEntrySendConfirmationEamil.Checked)
                {
                    StoreHelper.ConfirmOrder(store, order);
                }
                Module m = new Module(store.ModuleId);
                Order.EnsureSalesReportData(m.ModuleGuid, pageId, moduleId);
            }
            catch (Exception ex)
            {
                log.Error("error sending confirmation email", ex);
            }

            // redirect to order details
            string redirectUrl = SiteRoot +
                "/WebStore/AdminOrderDetail.aspx?pageid="
                + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString()
                + "&order=" + order.OrderGuid.ToString();

            WebUtils.SetupRedirect(this, redirectUrl);

        }

        void btnMakePayment_Click(object sender, EventArgs e)
        {
            if (
                 (store != null)
                 && (cart != null)
                 )
            {
                IPaymentGateway gateway = commerceConfig.GetDirectPaymentGateway();
                if (gateway == null)
                {
                    lblMessage.Text = WebStoreResources.PaymentGatewayNotConfiguredForDirectCardProcessing;
                    btnMakePayment.Enabled = false;
                    return;
                }
                if (gateway is PlugNPayPaymentGateway)
                {
                    gateway.MerchantInvoiceNumber = cart.CartGuid.ToString("N");
                    string CartItems = "";
                    int itemnum = 0;
                    foreach (CartOffer coffer in cart.CartOffers)
                    {
                        itemnum++;
                        CartItems += string.Format("&item{1}={0}&cost{1}={2}&description{1}={3}&quantity{1}={4}", coffer.OfferGuid, itemnum, coffer.OfferPrice, coffer.Name, coffer.Quantity);
                    }
                    gateway.MerchantTransactionDescription = CartItems; //not sure if this is the intended purpose or not
                }

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

                if (gateway is PayPalDirectPaymentGateway)
                {
                    // this is capturing of the serialized cart is not needed for other providers and I'm not sure its even needed for PayPal Direct
                    // it was needed for other paypal solutions. I just don't want to remove it until I'm sure
                    cart.SerializeCartOffers();
                    serializedCart = SerializationHelper.SerializeToString(cart);

                    gateway.LogTransaction(
                        siteSettings.SiteGuid,
                        store.ModuleGuid,
                        store.Guid,
                        cart.CartGuid,
                        cart.UserGuid,
                        "WebStorePayPalDirect",
                        "DirectPayment",
                        serializedCart);

                }
                else
                {
                    gateway.LogTransaction(
                        siteSettings.SiteGuid,
                        store.ModuleGuid,
                        store.Guid,
                        cart.CartGuid,
                        cart.UserGuid,
                        string.Empty,
                        "WebStoreCheckout",
                        serializedCart);
                }


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
                            StoreHelper.ClearClerkCartCookie(cart.StoreGuid);

                            // send confirmation email
                            try
                            {
                                if (chkSendConfirmationEmail.Checked)
                                {
                                    StoreHelper.ConfirmOrder(store, order);
                                }
                                Module m = new Module(store.ModuleId);
                                Order.EnsureSalesReportData(m.ModuleGuid, pageId, moduleId);
                            }
                            catch (Exception ex)
                            {
                                log.Error("error sending confirmation email", ex);
                            }

                            // redirect to order details
                            string redirectUrl = SiteRoot +
                                "/WebStore/AdminOrderDetail.aspx?pageid="
                                + pageId.ToInvariantString()
                                + "&mid=" + moduleId.ToInvariantString()
                                + "&order=" + order.OrderGuid.ToString();

                            WebUtils.SetupRedirect(this, redirectUrl);
                            return;

                        case PaymentGatewayResponse.Declined:

                            lblMessage.Text = WebStoreResources.TransactionDeclinedMessage;

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
                        if (commerceConfig.PaymentGatewayUseTestMode)
                            lblMessage.Text = gateway.LastExecutionException.ToString();
                    }
                }

            }

            btnMakePayment.Text = WebStoreResources.PaymentButton;


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


        private void ddCustomerCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCustomerGeoZoneList();
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
                    + "</a>" + crumbs.ItemWrapperBottom
                    + crumbs.Separator
                    + crumbs.ItemWrapperTop + "<a href='"
                    + SiteRoot + "/WebStore/AdminOrderEntry.aspx?pageid=" + pageId.ToInvariantString() + "&amp;mid=" + moduleId.ToInvariantString()
                    + "' class='unselectedcrumb'>" + WebStoreResources.OrderEntry
                    + "</a>" + crumbs.ItemWrapperBottom;
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.OrderEntryCheckoutHeader);
            heading.Text = WebStoreResources.OrderEntryCheckoutHeader;

            btnSave.Text = WebStoreResources.UpdateCustomerButton;

            btnSetUserFromGreyBox.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/1x1.gif");
            btnSetUserFromGreyBox.Attributes.Add("tabIndex", "-1");
            btnSetUserFromGreyBox.AlternateText = " ";

            //lnkUserLookup.Text = WebStoreResources.LookupUser;
            //lnkUserLookup.ToolTip = WebStoreResources.LookupUser;
            //lnkUserLookup.DialogCloseText = WebStoreResources.CloseDialogButton;
            //lnkUserLookup.NavigateUrl = SiteRoot + "/Dialog/UserSelectorDialog.aspx";

            lnkUserSearch.Text = WebStoreResources.LookupUser;
            lnkUserSearch.ToolTip = WebStoreResources.LookupUser;
            lnkUserSearch.NavigateUrl = SiteRoot + "/Dialog/UserSelectorDialog.aspx";

            

            btnClearUser.Text = WebStoreResources.ClearUserButton;

            btnMakePayment.Text = WebStoreResources.PaymentButton;
            btnMakePayment.Attributes.Add("onclick", "this.value='" + WebStoreResources.PaymentButtonDisabledText + "';this.disabled = true;" + Page.ClientScript.GetPostBackEventReference(this.btnMakePayment, ""));

            btnCreateOrder.Text = WebStoreResources.CreateOrderButton;

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
           

        }

        

        private void SetupSelectUserScript()
        {
            StringBuilder script = new StringBuilder();

            script.Append("\n<script type='text/javascript'>");
            script.Append("function SelectUser(userGuid, userId) {");

            //script.Append("GB_hide();");
           // script.Append("alert(userGuid);");

            script.Append("var hdnUG = document.getElementById('" + this.hdnUserGuid.ClientID + "'); ");
            script.Append("hdnUG.value = userGuid; ");

            script.Append("var hdnUI = document.getElementById('" + this.hdnUserID.ClientID + "'); ");
            script.Append("hdnUI.value = userId; ");


            script.Append("var btn = document.getElementById('" + this.btnSetUserFromGreyBox.ClientID + "');  ");
            script.Append("btn.click(); ");
            script.Append("$.colorbox.close(); ");

            script.Append("}");
            script.Append("</script>");


            Page.ClientScript.RegisterStartupScript(typeof(Page), "SelectUserHandler", script.ToString());

        }

        private void LoadSettings()
        {
            ScriptConfig.IncludeColorBox = true;
            storeCountry = new GeoCountry(siteSettings.DefaultCountryGuid);
            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);
            commerceConfig = SiteUtils.GetCommerceConfig();

            store = StoreHelper.GetStore();
            if (store == null) { return; }

            cart = StoreHelper.GetClerkCart(store);
            tblCountryList = GeoCountry.GetList();


            if (WebUser.IsInRoles(siteSettings.RolesThatCanLookupUsers))
            {
                SetupSelectUserScript();
                //lnkUserLookup.Visible = true;
                lnkUserSearch.Visible = true;
            }

            AddClassToBody("webstore admincheckout");

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
            this.ddCustomerCountry.SelectedIndexChanged += new EventHandler(ddCustomerCountry_SelectedIndexChanged);
            this.ddDeliveryCountry.SelectedIndexChanged += new EventHandler(ddDeliveryCountry_SelectedIndexChanged);
            this.btnSetUserFromGreyBox.Click += new ImageClickEventHandler(btnSetUserFromGreyBox_Click);
            btnClearUser.Click += new EventHandler(btnClearUser_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnMakePayment.Click += new EventHandler(btnMakePayment_Click);
            btnCreateOrder.Click += new EventHandler(btnCreateOrder_Click);

        }

        

        

        

        

        

        #endregion
    }
}
