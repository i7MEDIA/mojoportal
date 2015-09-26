/// Author:					Joe Audette
/// Created:				2008-03-01
/// Last Modified:		    2010-03-23
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Web;
using GCheckout.AutoGen;
using GCheckout.AutoGen.Extended;
using mojoPortal.Web;
using mojoPortal.Business;
using mojoPortal.Business.Commerce;
using WebStore.Business;
using mojoPortal.Business.WebHelpers.PaymentGateway;
using log4net;

namespace WebStore.Helpers
{
    public class WebStoreGCheckoutNotificationHandlerProvider : GCheckoutNotificationHandlerProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WebStoreGCheckoutNotificationHandlerProvider));

        public WebStoreGCheckoutNotificationHandlerProvider()
        { }


        private Cart DeserializeCart(MerchantData merchantData)
        {
            if (merchantData == null) return null;
            if (merchantData.SerializedObject.Length == 0) return null;


            object objCart = SerializationHelper.DeserializeFromString(typeof(Cart), SerializationHelper.RestoreXmlDeclaration(merchantData.SerializedObject));
            if (objCart != null)
                return objCart as Cart;


            return null;

        }

        public override void HandleNewOrderNotificationExtended(
            string requestXml,
            NewOrderNotificationExtended newOrder, 
            MerchantData merchantData)
        {
            //NotificationSerialNumber = newOrder.serialnumber;

            
            GoogleCheckoutLog gLog = new GoogleCheckoutLog();
            gLog.ProviderName = "WebStoreGCheckoutNotificationHandlerProvider";
            gLog.RawResponse = requestXml;
            gLog.NotificationType = "NewOrderNotification";
            gLog.SerialNumber = newOrder.serialnumber;
            gLog.OrderNumber = newOrder.googleordernumber;
            gLog.OrderTotal = newOrder.ordertotal.Value;
            gLog.BuyerId = newOrder.buyerid.ToString(CultureInfo.InvariantCulture);
            gLog.FullfillState = newOrder.fulfillmentorderstate.ToString();
            gLog.FinanceState = newOrder.financialorderstate.ToString();
            gLog.ShippingTotal = newOrder.ShippingCost;
            gLog.TaxTotal = newOrder.orderadjustment.totaltax.Value;
            //gLog.DiscountTotal = ext.orderadjustment.adjustmenttotal.Value;
            gLog.EmailListOptIn = newOrder.buyermarketingpreferences.emailallowed;
            gLog.GTimestamp = newOrder.timestamp;
            gLog.CartXml = SerializationHelper.RestoreXmlDeclaration(merchantData.SerializedObject);
            gLog.Save();


            Cart gCart = DeserializeCart(merchantData);

            Guid cartGuid = Guid.Empty;
            if (gCart != null)
                cartGuid = gCart.CartGuid;

            if (cartGuid == Guid.Empty) return;

            Cart cart = new Cart(cartGuid);
            if (cart.CartGuid != cartGuid) return;

            Store store = new Store(gCart.StoreGuid);
            if (store.Guid != cart.StoreGuid) return;

            gCart.DeSerializeCartOffers();

            gLog.SiteGuid = store.SiteGuid;
            gLog.UserGuid = gCart.UserGuid;
            gLog.CartGuid = gCart.CartGuid;
            gLog.StoreGuid = gCart.StoreGuid;
            gLog.Save();

            gCart.OrderInfo.CompletedFromIP = SiteUtils.GetIP4Address();
            

            gCart.OrderInfo.Completed = DateTime.UtcNow;
            if (newOrder.buyerbillingaddress.structuredname != null)
            {
                gCart.OrderInfo.CustomerFirstName = newOrder.buyerbillingaddress.structuredname.firstname;
                gCart.OrderInfo.CustomerLastName = newOrder.buyerbillingaddress.structuredname.lastname;
            }
            else
            {
                gCart.OrderInfo.CustomerFirstName = newOrder.buyerbillingaddress.contactname;
                gCart.OrderInfo.CustomerLastName = newOrder.buyerbillingaddress.contactname;
            }
            gCart.OrderInfo.CustomerEmail = newOrder.buyerbillingaddress.email;
            gCart.OrderInfo.CustomerCompany = newOrder.buyerbillingaddress.companyname;
            gCart.OrderInfo.CustomerAddressLine1 = newOrder.buyerbillingaddress.address1;
            gCart.OrderInfo.CustomerAddressLine2 = newOrder.buyerbillingaddress.address2;
            gCart.OrderInfo.CustomerCity = newOrder.buyerbillingaddress.city;
            gCart.OrderInfo.CustomerState = newOrder.buyerbillingaddress.region;
            gCart.OrderInfo.CustomerCountry = newOrder.buyerbillingaddress.countrycode;
            gCart.OrderInfo.CustomerPostalCode = newOrder.buyerbillingaddress.postalcode;
            gCart.OrderInfo.CustomerTelephoneDay = newOrder.buyerbillingaddress.phone;

            
            gCart.CopyCustomerToBilling();

            if (newOrder.buyershippingaddress.structuredname != null)
            {
                gCart.OrderInfo.DeliveryFirstName = newOrder.buyershippingaddress.structuredname.firstname;
                gCart.OrderInfo.DeliveryLastName = newOrder.buyershippingaddress.structuredname.lastname;
            }
            else
            {
                gCart.OrderInfo.DeliveryFirstName = newOrder.buyershippingaddress.contactname;
                gCart.OrderInfo.DeliveryLastName = newOrder.buyershippingaddress.contactname;
            }
            gCart.OrderInfo.DeliveryCompany = newOrder.buyershippingaddress.companyname;
            gCart.OrderInfo.DeliveryAddress1 = newOrder.buyershippingaddress.address1;
            gCart.OrderInfo.DeliveryAddress2 = newOrder.buyershippingaddress.address2;
            gCart.OrderInfo.DeliveryCity = newOrder.buyershippingaddress.city;
            gCart.OrderInfo.DeliveryState = newOrder.buyershippingaddress.region;
            gCart.OrderInfo.DeliveryCountry = newOrder.buyershippingaddress.countrycode;
            gCart.OrderInfo.DeliveryPostalCode = newOrder.buyershippingaddress.postalcode;

            gCart.TaxTotal = newOrder.orderadjustment.totaltax.Value;
            if (newOrder.ShippingCost > 0)
            {
                gCart.ShippingTotal = newOrder.ShippingCost;
            }
            gCart.OrderTotal = newOrder.ordertotal.Value;


            Guid orderStatusGuid = OrderStatus.OrderStatusReceivedGuid;


            if (
                (newOrder.financialorderstate == FinancialOrderState.CHARGEABLE)
                || (newOrder.financialorderstate == FinancialOrderState.CHARGED)
                || (newOrder.financialorderstate == FinancialOrderState.CHARGING)
                )
            {
                orderStatusGuid = OrderStatus.OrderStatusFulfillableGuid;

            }

            StoreHelper.EnsureUserForOrder(gCart);

            gCart.Save();

            //Currency currency = new Currency(store.DefaultCurrencyId);
            SiteSettings siteSettings = new SiteSettings(store.SiteGuid);

            Order order = Order.CreateOrder(
                store,
                gCart,
                gLog.RawResponse,
                gLog.OrderNumber,
                string.Empty,
                siteSettings.GetCurrency().Code,
                "GoogleCheckout",
                orderStatusGuid);

            //StoreHelper.ClearCartCookie(cart.StoreGuid);
            if (orderStatusGuid == OrderStatus.OrderStatusFulfillableGuid)
            {
                StoreHelper.ConfirmOrder(store, order);
                PayPalLog.DeleteByCart(order.OrderGuid);

            }

            if (orderStatusGuid == OrderStatus.OrderStatusReceivedGuid)
            {
                StoreHelper.ConfirmOrderReceived(store, order);
            }
            

        }

        public override void HandleOrderStateChangeNotification(
            string requestXml,
            OrderStateChangeNotification notification)
        {
            
            GoogleCheckoutLog gLog = new GoogleCheckoutLog();
            
            Guid orderGuid = GoogleCheckoutLog.GetCartGuidFromOrderNumber(notification.googleordernumber);

            gLog.RawResponse = requestXml;
            gLog.NotificationType = "OrderStateChangeNotification";
            gLog.ProviderName = "WebStoreGCheckoutNotificationHandlerProvider";
            gLog.SerialNumber = notification.serialnumber;
            gLog.OrderNumber = notification.googleordernumber;
            gLog.FinanceState = notification.newfinancialorderstate.ToString();
            gLog.FullfillState = notification.newfulfillmentorderstate.ToString();
            gLog.GTimestamp = notification.timestamp;
            gLog.AvsResponse = notification.reason;
            gLog.CartGuid = orderGuid;
            gLog.Save();

            if (orderGuid == Guid.Empty) return;

            Order order = new Order(orderGuid);
            if (order.OrderGuid != orderGuid) return;

            Store store = new Store(order.StoreGuid);
            if (store.Guid != order.StoreGuid) return;

            gLog.SiteGuid = store.SiteGuid;
            gLog.UserGuid = order.UserGuid;
            gLog.CartGuid = order.OrderGuid;
            gLog.StoreGuid = order.StoreGuid;
            gLog.Save();


            if (notification.newfinancialorderstate == FinancialOrderState.CHARGED)
            {
                order.StatusGuid = OrderStatus.OrderStatusFulfillableGuid;
                order.Save();

                if (!order.HasShippingProducts())
                {
                    // order only has download products so tell google the order is fulfilled

                    try
                    {
                        CommerceConfiguration commerceConfig = SiteUtils.GetCommerceConfig();

                        string gEvironment;
                        if (commerceConfig.GoogleEnvironment == GCheckout.EnvironmentType.Sandbox)
                        {
                            gEvironment = "Sandbox";
                        }
                        else
                        {
                            gEvironment = "Production";
                        }

                        GCheckout.OrderProcessing.DeliverOrderRequest fulfillNotification
                            = new GCheckout.OrderProcessing.DeliverOrderRequest(
                                commerceConfig.GoogleMerchantID,
                                commerceConfig.GoogleMerchantKey,
                                gEvironment,
                                notification.googleordernumber);

                        fulfillNotification.Send();

                        StoreHelper.ConfirmOrder(store, order);
                        PayPalLog.DeleteByCart(order.OrderGuid);
                        

                        log.Info("Sent DeliverOrderRequest to google api for google order " + notification.googleordernumber);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }

            }

        }

        public override void HandleRiskInformationNotification(
            string requestXml,
            RiskInformationNotification notification)
        {
            GoogleCheckoutLog gLog = new GoogleCheckoutLog();
            gLog.ProviderName = "WebStoreGCheckoutNotificationHandlerProvider";
            gLog.RawResponse = requestXml;
            gLog.NotificationType = "RiskInformationNotification";
            gLog.SerialNumber = notification.serialnumber;
            gLog.OrderNumber = notification.googleordernumber;
            gLog.GTimestamp = notification.timestamp;
            gLog.AvsResponse = notification.riskinformation.avsresponse;
            gLog.CvnResponse = notification.riskinformation.cvnresponse;
            gLog.BuyerId = notification.riskinformation.ipaddress;
            gLog.Save();

            Guid orderGuid = GoogleCheckoutLog.GetCartGuidFromOrderNumber(notification.googleordernumber);

            if (orderGuid == Guid.Empty) return;

            Order order = new Order(orderGuid);
            if (order.OrderGuid != orderGuid) return;

            Store store = new Store(order.StoreGuid);
            if (store.Guid != order.StoreGuid) return;

            gLog.SiteGuid = store.SiteGuid;
            gLog.UserGuid = order.UserGuid;
            gLog.CartGuid = order.OrderGuid;
            gLog.StoreGuid = order.StoreGuid;
            gLog.Save();

        }

        public override void HandleChargeAmountNotification(
            string requestXml,
            ChargeAmountNotification notification)
        {
            GoogleCheckoutLog gLog = new GoogleCheckoutLog();
            gLog.ProviderName = "WebStoreGCheckoutNotificationHandlerProvider";
            gLog.RawResponse = requestXml;
            gLog.NotificationType = "ChargeAmountNotification";
            gLog.SerialNumber = notification.serialnumber;
            gLog.OrderNumber = notification.googleordernumber;
            gLog.GTimestamp = notification.timestamp;
            gLog.TotalChgAmt = notification.totalchargeamount.Value;
            gLog.LatestChgAmt = notification.latestchargeamount.Value;

            gLog.Save();

            Guid orderGuid = GoogleCheckoutLog.GetCartGuidFromOrderNumber(notification.googleordernumber);

            if (orderGuid == Guid.Empty) return;

            Order order = new Order(orderGuid);
            if (order.OrderGuid != orderGuid) return;

            Store store = new Store(order.StoreGuid);
            if (store.Guid != order.StoreGuid) return;

            gLog.SiteGuid = store.SiteGuid;
            gLog.UserGuid = order.UserGuid;
            gLog.CartGuid = order.OrderGuid;
            gLog.StoreGuid = order.StoreGuid;
            gLog.Save();

        }

        public override void HandleChargebackAmountNotification(
            string requestXml,
            ChargebackAmountNotification notification)
        {
            GoogleCheckoutLog gLog = new GoogleCheckoutLog();
            gLog.ProviderName = "WebStoreGCheckoutNotificationHandlerProvider";
            gLog.NotificationType = "ChargebackAmountNotification";
            gLog.RawResponse = requestXml;
            gLog.SerialNumber = notification.serialnumber;
            gLog.OrderNumber = notification.googleordernumber;
            gLog.GTimestamp = notification.timestamp;
            gLog.LatestChargeback = notification.latestchargebackamount.Value;
            gLog.TotalChargeback = notification.totalchargebackamount.Value;

            gLog.Save();

            Guid orderGuid = GoogleCheckoutLog.GetCartGuidFromOrderNumber(notification.googleordernumber);

            if (orderGuid == Guid.Empty) return;

            Order order = new Order(orderGuid);
            if (order.OrderGuid != orderGuid) return;

            Store store = new Store(order.StoreGuid);
            if (store.Guid != order.StoreGuid) return;

            gLog.SiteGuid = store.SiteGuid;
            gLog.UserGuid = order.UserGuid;
            gLog.CartGuid = order.OrderGuid;
            gLog.StoreGuid = order.StoreGuid;
            gLog.Save();

        }

        public override void HandleAuthorizationAmountNotification(
            string requestXml,
            AuthorizationAmountNotification notification)
        {
            GoogleCheckoutLog gLog = new GoogleCheckoutLog();
            gLog.ProviderName = "WebStoreGCheckoutNotificationHandlerProvider";
            gLog.NotificationType = "AuthorizationAmountNotification";
            gLog.RawResponse = requestXml;
            gLog.SerialNumber = notification.serialnumber;
            gLog.OrderNumber = notification.googleordernumber;
            gLog.GTimestamp = notification.timestamp;
            gLog.AuthAmt = notification.authorizationamount.Value;
            gLog.AuthExpDate = notification.authorizationexpirationdate;
            gLog.CvnResponse = notification.cvnresponse;
            gLog.AvsResponse = notification.avsresponse;

            gLog.Save();

            Guid orderGuid = GoogleCheckoutLog.GetCartGuidFromOrderNumber(notification.googleordernumber);

            if (orderGuid == Guid.Empty) return;

            Order order = new Order(orderGuid);
            if (order.OrderGuid != orderGuid) return;

            Store store = new Store(order.StoreGuid);
            if (store.Guid != order.StoreGuid) return;

            gLog.SiteGuid = store.SiteGuid;
            gLog.UserGuid = order.UserGuid;
            gLog.CartGuid = order.OrderGuid;
            gLog.StoreGuid = order.StoreGuid;
            gLog.Save();

        }

        public override void HandleRefundAmountNotification(
            string requestXml,
            RefundAmountNotification notification)
        {
            GoogleCheckoutLog gLog = new GoogleCheckoutLog();
            gLog.ProviderName = "WebStoreGCheckoutNotificationHandlerProvider";
            gLog.NotificationType = "RefundAmountNotification";
            gLog.RawResponse = requestXml;
            gLog.SerialNumber = notification.serialnumber;
            gLog.OrderNumber = notification.googleordernumber;
            gLog.GTimestamp = notification.timestamp;

            gLog.LatestRefundAmt = notification.latestrefundamount.Value;
            gLog.TotalRefundAmt = notification.totalrefundamount.Value;


            gLog.Save();

            Guid orderGuid = GoogleCheckoutLog.GetCartGuidFromOrderNumber(notification.googleordernumber);

            if (orderGuid == Guid.Empty) return;

            Order order = new Order(orderGuid);
            if (order.OrderGuid != orderGuid) return;

            Store store = new Store(order.StoreGuid);
            if (store.Guid != order.StoreGuid) return;

            gLog.SiteGuid = store.SiteGuid;
            gLog.UserGuid = order.UserGuid;
            gLog.CartGuid = order.OrderGuid;
            gLog.StoreGuid = order.StoreGuid;
            gLog.Save();

        }

        
    }
}
