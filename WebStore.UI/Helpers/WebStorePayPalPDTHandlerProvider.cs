/// Author:                     Joe Audette
/// Created:                    2008-07-05
///	Last Modified:              2012-03-30
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Specialized;
using System.Globalization;
using mojoPortal.Business;
using mojoPortal.Business.Commerce;
using mojoPortal.Business.WebHelpers.PaymentGateway;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using WebStore.Business;
using log4net;

namespace WebStore.Helpers
{
    
    public class WebStorePayPalPDTHandlerProvider : PayPalPDTHandlerProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WebStorePayPalPDTHandlerProvider));

        public const string ProviderName = "WebStorePayPalPDTHandlerProvider";

        public override string HandleRequestAndReturnUrlForRedirect(
            string rawResponse,
            StringDictionary pdtItems,
            string transactionId,
            PayPalLog standardCheckoutLog)
        {
            string redirectUrl = string.Empty;

            if (standardCheckoutLog.SerializedObject.Length == 0) { return redirectUrl; }

            Cart cart = (Cart)SerializationHelper.DeserializeFromString(typeof(Cart), standardCheckoutLog.SerializedObject);

            Store store = new Store(cart.StoreGuid);

            bool debugPayPal = WebConfigSettings.DebugPayPal;

            string firstName = string.Empty;
            string lastName = string.Empty;
            string paymentStatus = string.Empty;
            string payerEmail = string.Empty;
            string currencyUsed = string.Empty;
            string paymentGross = string.Empty;
            string payPalFee = string.Empty;
            string payPalTax = string.Empty;
            string payPalShipping = string.Empty;
            string payPalSettlement = string.Empty;
            string pendingReason = string.Empty;
            string reasonCode = string.Empty;
            string paymentType = string.Empty;

            string customerAddress = string.Empty;
            string customerCity = string.Empty;
            string customerState = string.Empty;
            string customerPostalCode = string.Empty;
            string customerCountry = string.Empty;
            string customerPhone = string.Empty;
            string customerBusinessName = string.Empty;
            string customerMemo = string.Empty;

            if (pdtItems.ContainsKey("first_name"))
            {
                firstName = pdtItems["first_name"];
            }

            if (pdtItems.ContainsKey("last_name"))
            {
                lastName = pdtItems["last_name"];
            }

            if (pdtItems.ContainsKey("payment_status"))
            {
                paymentStatus = pdtItems["payment_status"];
            }

            if (pdtItems.ContainsKey("payer_email"))
            {
                payerEmail = pdtItems["payer_email"];
            }

            if (pdtItems.ContainsKey("mc_gross"))
            {
                paymentGross = pdtItems["mc_gross"];
            }

            if (pdtItems.ContainsKey("mc_fee"))
            {
                payPalFee = pdtItems["mc_fee"];
            }

            if (pdtItems.ContainsKey("tax"))
            {
                payPalTax = pdtItems["tax"];
            }

            if (pdtItems.ContainsKey("shipping"))
            {
                payPalShipping = pdtItems["shipping"];
            }

            if (pdtItems.ContainsKey("mc_currency"))
            {
                currencyUsed = pdtItems["mc_currency"];
            }

            if (pdtItems.ContainsKey("pending_reason"))
            {
                pendingReason = pdtItems["pending_reason"];
            }

            if (pdtItems.ContainsKey("reason_code"))
            {
                reasonCode = pdtItems["reason_code"];
            }

            if (pdtItems.ContainsKey("txn_type"))
            {
                paymentType = pdtItems["txn_type"];
            }

            if (pdtItems.ContainsKey("settle_amount"))
            {
                payPalSettlement = pdtItems["settle_amount"];
            }

            if (pdtItems.ContainsKey("address_street"))
            {
                customerAddress = pdtItems["address_street"];
            }

            if (pdtItems.ContainsKey("address_city"))
            {
                customerCity = pdtItems["address_city"];
            }

            if (pdtItems.ContainsKey("address_state"))
            {
                customerState = pdtItems["address_state"];
            }

            if (pdtItems.ContainsKey("address_zip"))
            {
                customerPostalCode = pdtItems["address_zip"];
            }

            if (pdtItems.ContainsKey("address_country"))
            {
                customerCountry = pdtItems["address_country"];
            }

            if (pdtItems.ContainsKey("contact_phone"))
            {
                customerPhone = pdtItems["contact_phone"];
            }

            if (pdtItems.ContainsKey("payer_business_name"))
            {
                customerBusinessName = pdtItems["payer_business_name"];
            }

            // TODO: we need to store this somewhere on the cart/order
            // its the message the user enters in special instructions on paypal checkout
            if (pdtItems.ContainsKey("memo"))
            {
                customerMemo = pdtItems["memo"];
            }

            //Regardless of the specified currency, the format will have decimal point 
            //with exactly two digits to the right and an optional thousands separator to the left, 
            //which must be a comma; for example, EUR 2.000,00 must be specified as 2000.00 or 2,000.00
            // So we want to parse it with US Culture

            CultureInfo currencyCulture = new CultureInfo("en-US");
            //if (currencyUsed.Length > 0)
            //{
            //    currencyCulture = ResourceHelper.GetCurrencyCulture(currencyUsed);
            //    if (debugPayPal) { log.Info("PayPal currencyUsed was " + currencyUsed); }
            //}
            //else
            //{
            //    SiteSettings siteSettings = new SiteSettings(store.SiteGuid);
            //    //Currency currency = new Currency(store.DefaultCurrencyId);
            //    //currencyCulture = ResourceHelper.GetCurrencyCulture(currency.Code);
            //    Currency currency = siteSettings.GetCurrency();
            //    currencyCulture = ResourceHelper.GetCurrencyCulture(currency.Code);
            //    currencyUsed = currency.Code;
            //}

            if (debugPayPal) { log.Info("PayPal rawResponse was " + rawResponse); }

            if (debugPayPal) { log.Info("PayPal final currency culture was " + currencyUsed); }
            

            decimal grossAmount = 0;
            decimal.TryParse(paymentGross, NumberStyles.Currency, currencyCulture, out grossAmount);

            decimal feeAmount = 0;
            decimal.TryParse(payPalFee, NumberStyles.Currency, currencyCulture, out feeAmount);

            decimal taxAmount = 0;
            decimal.TryParse(payPalTax, NumberStyles.Currency, currencyCulture, out taxAmount);

            decimal shippingAmount = 0;
            decimal.TryParse(payPalShipping, NumberStyles.Currency, currencyCulture, out shippingAmount);

            decimal settleAmount = 0;
            decimal.TryParse(payPalSettlement, NumberStyles.Currency, currencyCulture, out settleAmount);

            
            if (debugPayPal)
            {
                log.Info("PayPal paymentGross was " + paymentGross + " which was parsed as " + grossAmount.ToString());
                log.Info("PayPal payPalFee was " + payPalFee + " which was parsed as " + feeAmount.ToString());
                log.Info("PayPal payPalTax was " + payPalTax + " which was parsed as " + taxAmount.ToString());
                log.Info("PayPal payPalShipping was " + payPalShipping + " which was parsed as " + shippingAmount.ToString());
                log.Info("PayPal payPalSettlement was " + payPalSettlement + " which was parsed as " + settleAmount.ToString());

            }

            PayPalLog payPalLog = new PayPalLog();
            payPalLog.PDTProviderName = standardCheckoutLog.PDTProviderName;
            payPalLog.IPNProviderName = standardCheckoutLog.IPNProviderName;
            payPalLog.ReturnUrl = standardCheckoutLog.ReturnUrl;
            payPalLog.ProviderName = standardCheckoutLog.ProviderName;
            payPalLog.SiteGuid = standardCheckoutLog.SiteGuid;
            payPalLog.StoreGuid = standardCheckoutLog.StoreGuid;
            payPalLog.UserGuid = standardCheckoutLog.UserGuid;
            payPalLog.ApiVersion = standardCheckoutLog.ApiVersion;
            payPalLog.CartGuid = standardCheckoutLog.CartGuid;
            payPalLog.SerializedObject = standardCheckoutLog.SerializedObject;
            payPalLog.CartTotal = standardCheckoutLog.CartTotal;
            payPalLog.PayPalAmt = grossAmount;
            payPalLog.FeeAmt = feeAmount;
            if (settleAmount > 0)
            {
                payPalLog.SettleAmt = settleAmount;
            }
            else
            {
                payPalLog.SettleAmt = (grossAmount - feeAmount);
            }
            payPalLog.TaxAmt = taxAmount;
            payPalLog.CurrencyCode = currencyUsed;
            payPalLog.TransactionId = transactionId;
            payPalLog.RawResponse = rawResponse;
            payPalLog.Response = "PDTSuccess";
            payPalLog.RequestType = "PDT";
            payPalLog.PayerId = payerEmail;
            payPalLog.PaymentType = paymentType;
            payPalLog.PaymentStatus = paymentStatus;
            payPalLog.PendingReason = pendingReason;
            payPalLog.ReasonCode = reasonCode;
            payPalLog.Save();

            

           
            // see if this cart has already been proceesed
            Order existingOrder = new Order(cart.CartGuid);
            // order already exists
            if (existingOrder.OrderGuid != Guid.Empty) 
            { 
                // lookup order status if needed make it fullfillable
                // then redirect to order detail page
                if (existingOrder.StatusGuid == OrderStatus.OrderStatusReceivedGuid)
                {
                    if (paymentStatus == "Completed")
                    {
                        existingOrder.StatusGuid = OrderStatus.OrderStatusFulfillableGuid;
                        existingOrder.Save();
                        try
                        {
                            StoreHelper.ConfirmOrder(store, existingOrder);
                            GoogleCheckoutLog.DeleteByCart(existingOrder.OrderGuid);
                        }
                        catch (Exception ex)
                        {
                            log.Error("error sending confirmation email", ex);
                        }
                    }

                    
                    
                }

                // this was set in Checkout.aspx and should return to order detail page
                if (standardCheckoutLog.ReturnUrl.Length > 0)
                {
                    redirectUrl = standardCheckoutLog.ReturnUrl;
                }

                payPalLog.ReasonCode = "existing order found";
                payPalLog.Save();

                return redirectUrl; 
            }
            
            // if we get here the cart has not yet been processed into an order
            cart.DeSerializeCartOffers();

            Guid orderStatus;
            if (paymentStatus == "Completed")
            {
                orderStatus = OrderStatus.OrderStatusFulfillableGuid;
            }
            else
            {
                orderStatus = OrderStatus.OrderStatusReceivedGuid;
            }

            // update the order with customer shipping info
            cart.OrderInfo.DeliveryCompany = customerBusinessName;
            cart.OrderInfo.DeliveryAddress1 = customerAddress;

            cart.OrderInfo.DeliveryCity = customerCity;
            cart.OrderInfo.DeliveryFirstName = firstName;
            cart.OrderInfo.DeliveryLastName = lastName;
            cart.OrderInfo.DeliveryPostalCode = customerPostalCode;
            cart.OrderInfo.DeliveryState = customerState;
            cart.OrderInfo.DeliveryCountry = customerCountry;

            if (customerPhone.Length > 0)
            {
                cart.OrderInfo.CustomerTelephoneDay = customerPhone;
            }

            if (payerEmail.Length > 0)
            {
                cart.OrderInfo.CustomerEmail = payerEmail;
            }

            cart.CopyShippingToBilling();
            cart.CopyShippingToCustomer();
            cart.TaxTotal = taxAmount;
            cart.OrderTotal = grossAmount;
            if (shippingAmount > 0)
            {
                cart.ShippingTotal = shippingAmount;
            }

            StoreHelper.EnsureUserForOrder(cart);
            
           
            cart.Save();

            cart.SerializeCartOffers();
            payPalLog.SerializedObject = SerializationHelper.SerializeToString(cart);
            payPalLog.Save();

            Order order = Order.CreateOrder(
                store,
                cart,
                transactionId,
                transactionId,
                string.Empty,
                currencyUsed,
                "PayPal",
                orderStatus);

            if (standardCheckoutLog.ReturnUrl.Length > 0)
            {
                redirectUrl = standardCheckoutLog.ReturnUrl;
            }

            if (orderStatus == OrderStatus.OrderStatusFulfillableGuid)
            {
                try
                {
                    StoreHelper.ConfirmOrder(store, order);
                    GoogleCheckoutLog.DeleteByCart(order.OrderGuid);
                }
                catch (Exception ex)
                {
                    log.Error("error sending confirmation email", ex);
                }
            }

            if (orderStatus == OrderStatus.OrderStatusReceivedGuid)
            {
                if ((paymentStatus == "Pending") && (pendingReason == "echeck"))
                {
                    StoreHelper.ConfirmOrderReceived(store, existingOrder, true);
                }
            }

            

            return redirectUrl; 

            

        }
    }
}
