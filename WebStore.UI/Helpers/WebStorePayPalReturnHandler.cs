/// Author:                     Joe Audette
/// Created:                    2008-06-28
///	Last Modified:              2008-07-09
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
using System.Web;
using log4net;
using mojoPortal.Web;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers.PaymentGateway;
using WebStore.Business;


namespace WebStore.Helpers
{
    
    public class WebStorePayPalReturnHandler : PayPalReturnHandlerProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WebStorePayPalReturnHandler));

        public const string ProviderName = "WebStorePayPalHandler";

        public WebStorePayPalReturnHandler()
        { }

        public override string HandleRequestAndReturnUrlForRedirect(
            HttpContext context,
            string payPalToken,
            string payPalPayerId,
            PayPalLog setExpressCheckoutLog)
        {
            string redirectUrl = string.Empty;

            if ((payPalToken == null) || (payPalToken.Length == 0))
            {
                log.Error("WebStorePayPalReturnHandler received empty payPalToken");
                return redirectUrl;

            }

            if (setExpressCheckoutLog == null)
            {
                log.Error("WebStorePayPalReturnHandler received null setExpressCheckoutLog for payPalToken " + payPalToken);
                return redirectUrl;

            }

            if (setExpressCheckoutLog.SerializedObject.Length == 0)
            {
                log.Error("WebStorePayPalReturnHandler cart was not previously serialized for payPalToken " + payPalToken);
                return redirectUrl;

            }

            if (setExpressCheckoutLog.CreatedUtc.AddHours(4) < DateTime.UtcNow)
            {
                log.Error("payPalToken " + payPalToken + " was more than 4 hours old, it should expire after 3 hours ");
                return redirectUrl;

            }

            CommerceConfiguration commerceConfig = SiteUtils.GetCommerceConfig();

            PayPalExpressGateway gateway
                = new PayPalExpressGateway(
                    commerceConfig.PayPalAPIUsername,
                    commerceConfig.PayPalAPIPassword,
                    commerceConfig.PayPalAPISignature,
                    commerceConfig.PayPalStandardEmailAddress);

            gateway.UseTestMode = commerceConfig.PaymentGatewayUseTestMode;
            gateway.PayPalToken = payPalToken;
            gateway.PayPalPayerId = payPalPayerId;


            Cart savedCart = (Cart)SerializationHelper.DeserializeFromString(typeof(Cart), setExpressCheckoutLog.SerializedObject);
            savedCart.DeSerializeCartOffers();

            string siteRoot = SiteUtils.GetNavigationSiteRoot();
            
            gateway.MerchantCartId = savedCart.CartGuid.ToString();
            gateway.ChargeTotal = savedCart.OrderTotal;
            gateway.ReturnUrl = siteRoot + "/Services/PayPalReturnHandler.ashx";
            gateway.CancelUrl = siteRoot;
            //gateway.PayPalPayerId = payPalPayerId;

            gateway.CallGetExpressCheckoutDetails();
            

            PayPalLog payPalLog = new PayPalLog();
            payPalLog.ProviderName = WebStorePayPalReturnHandler.ProviderName;
            payPalLog.SerializedObject = setExpressCheckoutLog.SerializedObject;
            payPalLog.ReturnUrl = setExpressCheckoutLog.ReturnUrl;
            payPalLog.RawResponse = gateway.RawResponse;
            payPalLog.TransactionId = gateway.TransactionId;
            payPalLog.CurrencyCode = gateway.CurrencyCode;
            // TODO: add versions to gateways
            //log.ApiVersion = gateway.
            payPalLog.CartGuid = savedCart.CartGuid;

            Store store = new Store(savedCart.StoreGuid);
            payPalLog.Token = payPalToken;
            payPalLog.PayerId = payPalPayerId;
            payPalLog.RequestType = "GetExpressCheckoutDetails";
            payPalLog.SiteGuid = store.SiteGuid;
            payPalLog.StoreGuid = store.Guid;
            payPalLog.UserGuid = savedCart.UserGuid;

            // update the order with customer shipping info
            savedCart.OrderInfo.DeliveryCompany = gateway.ShipToCompanyName;
            savedCart.OrderInfo.DeliveryAddress1 = gateway.ShipToAddress;
            savedCart.OrderInfo.DeliveryAddress2 = gateway.ShipToAddress2;
            savedCart.OrderInfo.DeliveryCity = gateway.ShipToCity;
            savedCart.OrderInfo.DeliveryFirstName = gateway.ShipToFirstName;
            savedCart.OrderInfo.DeliveryLastName = gateway.ShipToLastName;
            savedCart.OrderInfo.DeliveryPostalCode = gateway.ShipToPostalCode;
            savedCart.OrderInfo.DeliveryState = gateway.ShipToState;
            savedCart.OrderInfo.DeliveryCountry = gateway.ShipToCountry;

            //Note that PayPal only returns a phone number if your Merchant accounts is configured to require the
            // buyer to provide it.
            if (gateway.ShipToPhone.Length > 0)
            {
                savedCart.OrderInfo.CustomerTelephoneDay = gateway.ShipToPhone;
            }

            if (gateway.BuyerEmail.Length > 0)
            {
                savedCart.OrderInfo.CustomerEmail = gateway.BuyerEmail;
            }

            // if customer and billing aren't populated already, user was anonymous when checkout began, make them the same as shipping
            //if (savedCart.UserGuid == Guid.Empty)
            //{
            //2013-12-23 since all we get is shipping address this can be considered as the same thing as billing address for paypal purposes so always use it
            // especially because we may need to calculate tax for express checkout
            // based on the address provided by paypal
                savedCart.CopyShippingToBilling();
                savedCart.CopyShippingToCustomer();
            //}

            GeoCountry country = new GeoCountry(savedCart.OrderInfo.DeliveryCountry);
            GeoZone taxZone = GeoZone.GetByCode(country.Guid, savedCart.OrderInfo.DeliveryState);
            savedCart.OrderInfo.TaxZoneGuid = taxZone.Guid;

            savedCart.OrderInfo.Save();

            // refresh totals to calculate tax or shipping now that we have an address
            savedCart.RefreshTotals();
            savedCart.Save();

            savedCart.SerializeCartOffers();
            payPalLog.SerializedObject = SerializationHelper.SerializeToString(savedCart);
            
            payPalLog.Save();

            if (gateway.Response == PaymentGatewayResponse.Error)
            {
                redirectUrl = siteRoot + "/WebStore/PayPalGatewayError.aspx?plog=" + payPalLog.RowGuid.ToString();
                return redirectUrl;
            }

            if (gateway.PayPalPayerId.Length == 0)
            {
                redirectUrl = siteRoot + "/WebStore/PayPalGatewayError.aspx?plog=" + payPalLog.RowGuid.ToString();
                return redirectUrl;
            }

            
            int pageId = -1;

            List<PageModule> pageModules = PageModule.GetPageModulesByModule(store.ModuleId);
            foreach (PageModule pm in pageModules)
            {
                // use first pageid found, really a store should only
                // be on one page
                pageId = pm.PageId;
                break;

            }

            // after the CallGetExpressCheckoutDetails
            // we have the option of directing to a final review page before
            // calling CallDoExpressCheckoutPayment

            redirectUrl = siteRoot +
                "/WebStore/PayPalExpressCheckout.aspx?pageid="
                + pageId.ToString(CultureInfo.InvariantCulture)
                + "&mid=" + store.ModuleId.ToString(CultureInfo.InvariantCulture)
                + "&plog=" + payPalLog.RowGuid.ToString();

            return redirectUrl;



            

        }

    }
}
