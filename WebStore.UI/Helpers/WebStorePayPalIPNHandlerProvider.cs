/// Author:                     Joe Audette
/// Created:                    2008-07-05
///	Last Modified:              2010-03-23
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration.Provider;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Web;
using mojoPortal.Business;
using mojoPortal.Business.Commerce;
using mojoPortal.Business.WebHelpers.PaymentGateway;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using WebStore.Business;
using log4net;

namespace WebStore.Helpers
{
    public class WebStorePayPalIPNHandlerProvider : PayPalIPNHandlerProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WebStorePayPalIPNHandlerProvider));

        public const string ProviderName = "WebStorePayPalIPNHandlerProvider";

        /// <summary>
        /// return true if the transaction was processed with no problems
        /// </summary>
        /// <param name="context"></param>
        /// <param name="transactionId"></param>
        /// <param name="orderId"></param>
        /// <param name="grossAmount"></param>
        /// <param name="standardCheckoutLog"></param>
        /// <returns></returns>
        public override bool HandleRequest(
            string transactionId,
            NameValueCollection form,
            PayPalLog standardCheckoutLog)
        {
            bool result = false;

            if (standardCheckoutLog.SerializedObject.Length == 0) { return result; }

            Cart cart = (Cart)SerializationHelper.DeserializeFromString(typeof(Cart), standardCheckoutLog.SerializedObject);

            Store store = new Store(cart.StoreGuid);
            SiteSettings siteSettings = new SiteSettings(store.SiteGuid);

            bool debugPayPal = WebConfigSettings.DebugPayPal;

            //mc_gross=5.00
            //&address_status=confirmed
            //&item_number1=d28a6bed-7e51-4f18-a893-77b4d5665a64
            //&payer_id=zzzzzz
            //&tax=0.00
            //&address_street=nnnn
            //&payment_date=10%3A08%3A08+Jul+29%2C+2008+PDT
            //&payment_status=Completed
            //&charset=windows-1252
            //&address_zip=92843
            //&mc_shipping=0.00
            //&mc_handling=0.00
            //&first_name=zz
            //&mc_fee=0.45
            //&address_country_code=US
            //&address_name=zzzz
            //&notify_version=2.4
            //&custom=d9ef5324-2201-4749-b06a-9bba7a9dce61
            //&payer_status=verified
            //&business=sales%40mojoportal.com
            //&address_country=United+States
            //&num_cart_items=1
            //&mc_handling1=0.00
            //&address_city=nnnn
            //&verify_sign=
            //&payer_email=zzzzzz
            //&mc_shipping1=0.00
            //&tax1=0.00
            //&txn_id=81Y88484JA1416221
            //&payment_type=instant
            //&payer_business_name=EBShoes
            //&last_name=Ngo
            //&address_state=CA
            //&item_name1=Buy+Joe+a+Beer
            //&receiver_email=sales%40mojoportal.com
            //&payment_fee=0.45
            //&quantity1=1
            //&receiver_id=nnnn
            //&txn_type=cart
            //&mc_gross_1=5.00
            //&mc_currency=USD
            //&residence_country=US
            //&payment_gross=5.00

            string firstName = string.Empty;
            if (form["first_name"] != null)
            {
                firstName = form["first_name"].ToString();
            }

            string lastName = string.Empty;
            if (form["last_name"] != null)
            {
                lastName = form["last_name"].ToString();
            }

            string paymentStatus = string.Empty;
            if (form["payment_status"] != null)
            {
                paymentStatus = form["payment_status"].ToString();
            }

            string payerEmail = string.Empty;
            if (form["payer_email"] != null)
            {
                payerEmail = form["payer_email"].ToString();
            }

            string paymentGross = string.Empty;
            if (form["mc_gross"] != null)
            {
                paymentGross = form["mc_gross"].ToString();
            }

            string payPalFee = string.Empty;
            if (form["mc_fee"] != null)
            {
                payPalFee = form["mc_fee"].ToString();
            }

            string payPalTax = string.Empty;
            if (form["tax"] != null)
            {
                payPalTax = form["tax"].ToString();
            }

            string payPalShipping = string.Empty;
            if (form["mc_shipping"] != null)
            {
                payPalShipping = form["mc_shipping"].ToString();
            }

            string currencyUsed = string.Empty;
            if (form["mc_currency"] != null)
            {
                currencyUsed = form["mc_currency"].ToString();
            }


            string pendingReason = string.Empty;
            if (form["pending_reason"] != null)
            {
                pendingReason = form["pending_reason"].ToString();
            }

            string reasonCode = string.Empty;
            if (form["reason_code"] != null)
            {
                reasonCode = form["reason_code"].ToString();
            }

            string paymentType = string.Empty;
            if (form["txn_type"] != null)
            {
                paymentType = form["txn_type"].ToString();
            }

            string payPalSettlement = "0";
            if (form["settle_amount"] != null)
            {
                payPalSettlement = form["settle_amount"].ToString();
            }

            string customerAddress = string.Empty;
            if (form["address_street"] != null)
            {
                customerAddress = form["address_street"].ToString();
            }

            string customerCity = string.Empty;
            if (form["address_city"] != null)
            {
                customerCity = form["address_city"].ToString();
            }

            string customerState = string.Empty;
            if (form["address_state"] != null)
            {
                customerState = form["address_state"].ToString();
            }


            string customerPostalCode = string.Empty;
            if (form["address_zip"] != null)
            {
                customerPostalCode = form["address_zip"].ToString();
            }


            string customerCountry = string.Empty;
            if (form["address_country"] != null)
            {
                customerCountry = form["address_country"].ToString();
            }

            string customerPhone = string.Empty;
            if (form["contact_phone"] != null)
            {
                customerPhone = form["contact_phone"].ToString();
            }

            string customerBusinessName = string.Empty;
            if (form["payer_business_name"] != null)
            {
                customerBusinessName = form["payer_business_name"].ToString();
            }

            // TODO: we need to store this somewhere on the cart/order
            // its the message the user enters in special instructions on paypal checkout
            string customerMemo = string.Empty;
            if (form["memo"] != null)
            {
                customerMemo = form["memo"].ToString();
            }

            if (debugPayPal) { log.Info("PayPal currencyUsed was " + currencyUsed); }

            //Regardless of the specified currency, the format will have decimal point 
            //with exactly two digits to the right and an optional thousands separator to the left, 
            //which must be a comma; for example, EUR 2.000,00 must be specified as 2000.00 or 2,000.00
            // So we want to parse it with US Culture
            
            CultureInfo currencyCulture = new CultureInfo("en-US");
            //if (currencyUsed.Length > 0)
            //{
            //    currencyCulture = ResourceHelper.GetCurrencyCulture(currencyUsed);
            //}
            //else
            //{
            //    //Currency currency = new Currency(store.DefaultCurrencyId);
            //    //currencyCulture = ResourceHelper.GetCurrencyCulture(currency.Code);
            //    Currency currency = siteSettings.GetCurrency();
            //    currencyCulture = ResourceHelper.GetCurrencyCulture(currency.Code);
            //    currencyUsed = currency.Code;
            //}

            //if (debugPayPal) { log.Info("PayPal final currency culture was " + currencyUsed); }

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
            payPalLog.CartTotal = grossAmount;
            payPalLog.PayPalAmt = feeAmount;
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
            payPalLog.RawResponse = form.ToString();
            payPalLog.Response = "IPNSuccess";
            payPalLog.RequestType = "IPN";
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

                result = true;

                payPalLog.ReasonCode = "existing order found";
                payPalLog.Save();

                return result;
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
                StoreHelper.ConfirmOrderReceived(store, order);
            }

            result = true;

            return result;
        }
    }
}
