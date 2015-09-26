// Author:		        Joe Audette
// Created:             2012-09-23
// Last Modified:       2012-10-04
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
using System.Text;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.Commerce;
using mojoPortal.Web;
using mojoPortal.Web.Commerce;
using mojoPortal.Web.Framework;
using WebStore.Business;
using log4net;

namespace WebStore.Helpers
{
    public class WebStoreWorldPayResponseHandler : WorldPayResponseHandlerProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WebStoreWorldPayResponseHandler));

        private CommerceConfiguration config;

        public override bool HandleRequest(
            WorldPayPaymentResponse wpResponse,
            PayPalLog worldPayLog,
            Page page)
        {
            bool result = false;
            

            if (worldPayLog.SerializedObject.Length == 0) { return result; }

            Cart cart = (Cart)SerializationHelper.DeserializeFromString(typeof(Cart), worldPayLog.SerializedObject);

            Store store = new Store(cart.StoreGuid);
            //SiteSettings siteSettings = new SiteSettings(store.SiteGuid);
            config = SiteUtils.GetCommerceConfig();

            switch (wpResponse.TransStatus)
            {
                case "Y": //success
                    ProcessOrder(cart, store, wpResponse, worldPayLog, page);

                    result = true;
                    break;

                case "C": // cancelled
                default:
                    ProcessCancellation(cart, store, wpResponse, worldPayLog, page);
                    break;

            }


            return result;
        }

        private void ProcessOrder(
            Cart cart,
            Store store,
            WorldPayPaymentResponse wpResponse,
            PayPalLog worldPayLog,
            Page page)
        {
            // process the cart into an order then
            // return an html order result template for use at world pay

           

            cart.DeSerializeCartOffers();

            if (wpResponse.CompName.Length > 0)
            {
                cart.OrderInfo.CustomerCompany = wpResponse.CompName;
            }
            if (wpResponse.Address1.Length > 0)
            {
                cart.OrderInfo.CustomerAddressLine1 = wpResponse.Address1;
            }

            if (wpResponse.Address2.Length > 0)
            {
                cart.OrderInfo.CustomerAddressLine2 = wpResponse.Address2;
            }

            if (wpResponse.Address3.Length > 0)
            {
                cart.OrderInfo.CustomerAddressLine2 += " " + wpResponse.Address3;
            }

            if (wpResponse.Town.Length > 0)
            {
                cart.OrderInfo.CustomerCity = wpResponse.Town;
            }
            //cart.OrderInfo.DeliveryFirstName = wpResponse.Name;
            if(
                (wpResponse.Name.Length > 0)
                && ((cart.OrderInfo.CustomerLastName.Length == 0) || (!wpResponse.Name.Contains((cart.OrderInfo.CustomerLastName))))
                )
            {
                cart.OrderInfo.CustomerLastName = wpResponse.Name; // this is full name
            }
            if (wpResponse.Postcode.Length > 0)
            {
                cart.OrderInfo.CustomerPostalCode = wpResponse.Postcode;
            }
            if (wpResponse.Region.Length > 0)
            {
                cart.OrderInfo.CustomerState = wpResponse.Region;
            }
            if (wpResponse.Country.Length > 0)
            {
                cart.OrderInfo.CustomerCountry = wpResponse.Country;
            }

            if (wpResponse.Tel.Length > 0)
            {
                cart.OrderInfo.CustomerTelephoneDay = wpResponse.Tel;
            }

            if (wpResponse.Email.Length > 0)
            {
                cart.OrderInfo.CustomerEmail = wpResponse.Email;
            }

            cart.CopyCustomerToBilling();
            cart.CopyCustomerToShipping();
            //cart.TaxTotal = taxAmount;
            //cart.OrderTotal = grossAmount;
            //if (shippingAmount > 0)
            //{
            //    cart.ShippingTotal = shippingAmount;
            //}

            StoreHelper.EnsureUserForOrder(cart);

            cart.Save();

            Order order = Order.CreateOrder(
                store,
                cart,
                wpResponse.TransId,
                wpResponse.TransId,
                string.Empty,
                wpResponse.Currency,
                "WorldPay",
                OrderStatus.OrderStatusFulfillableGuid);

            // grab the return url before we delete the un-needed logs
            string orderDetailUrl = worldPayLog.ReturnUrl;
            string storePageUrl = worldPayLog.RawResponse;

            // remove any previous logs
            GoogleCheckoutLog.DeleteByCart(order.OrderGuid);
            PayPalLog.DeleteByCart(order.OrderGuid);

            // create a final log that has the serialized reposnse from worldpay rather than the serialized cart
            worldPayLog = new PayPalLog();
            worldPayLog.SiteGuid = store.SiteGuid;
            worldPayLog.StoreGuid = store.Guid;
            worldPayLog.CartGuid = order.OrderGuid;
            worldPayLog.UserGuid = order.UserGuid;
            worldPayLog.ProviderName = "WebStoreWorldPayResponseHandler";
            worldPayLog.RequestType = "WorldPay";
            worldPayLog.PaymentStatus = "Paid";
            worldPayLog.PaymentType = "WorldPay";
            worldPayLog.CartTotal = order.OrderTotal;
            worldPayLog.PayPalAmt = wpResponse.AuthAmount;
            worldPayLog.TransactionId = wpResponse.TransId;
            worldPayLog.CurrencyCode = wpResponse.Currency;
            worldPayLog.ReasonCode = wpResponse.AVS;
            worldPayLog.RawResponse = SerializationHelper.SerializeToString(wpResponse);
            worldPayLog.CreatedUtc = DateTime.UtcNow;
            worldPayLog.ReturnUrl = orderDetailUrl;
            worldPayLog.Save();

            
            try
            {
                StoreHelper.ConfirmOrder(store, order);
                
            }
            catch (Exception ex)
            {
                log.Error("error sending confirmation email", ex);
            }
           
            // retrun the html

            if (config.WorldPayProduceShopperResponse)
            {
                CultureInfo currencyCulture = ResourceHelper.GetCurrencyCulture(wpResponse.Currency);

                string htmlTemplate = ResourceHelper.GetMessageTemplate(CultureInfo.CurrentUICulture, config.WorldPayShopperResponseTemplate);
                StringBuilder finalOutput = new StringBuilder();
                finalOutput.Append(htmlTemplate);
                finalOutput.Replace("#WorldPayBannerToken", "<WPDISPLAY ITEM=banner>"); //required by worldpay
                finalOutput.Replace("#CustomerName", wpResponse.Name);
                finalOutput.Replace("#StoreName", store.Name);
                finalOutput.Replace("#OrderId", order.OrderGuid.ToString());
                finalOutput.Replace("#StorePageLink", "<a href='" + storePageUrl + "'>" + storePageUrl + "</a>");
                finalOutput.Replace("#OrderDetailLink", "<a href='" + orderDetailUrl + "'>" + orderDetailUrl + "</a>");
            

                StringBuilder orderDetails = new StringBuilder();
                DataSet dsOffers = Order.GetOrderOffersAndProducts(store.Guid, order.OrderGuid);

                foreach (DataRow row in dsOffers.Tables["Offers"].Rows)
                {
                    string og = row["OfferGuid"].ToString();
                    orderDetails.Append(row["Name"].ToString() + " ");
                    orderDetails.Append(row["Quantity"].ToString() + " @ ");
                    orderDetails.Append(string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(row["OfferPrice"])));
                    orderDetails.Append("<br />");

                    string whereClause = string.Format("OfferGuid = '{0}'", og);
                    DataView dv = new DataView(dsOffers.Tables["Products"], whereClause, "", DataViewRowState.CurrentRows);

                    if (dv.Count > 1)
                    {
                        foreach (DataRow r in dsOffers.Tables["Products"].Rows)
                        {
                            string pog = r["OfferGuid"].ToString();
                            if (og == pog)
                            {
                                orderDetails.Append(r["Name"].ToString() + " ");
                                orderDetails.Append(r["Quantity"].ToString() + "  <br />");

                            }

                        }
                    }

                }

                finalOutput.Replace("#OrderDetails", orderDetails.ToString());
                page.Response.Write(finalOutput.ToString());
                page.Response.Flush();

            }
           



        }

        private void ProcessCancellation(
            Cart cart,
            Store store,
            WorldPayPaymentResponse wpResponse,
            PayPalLog worldPayLog,
            Page page)
        {
            //string serializedResponse = SerializationHelper.SerializeToString(wpResponse);
            //log.Info("received cancellation worldpay postback, xml to follow");
            //log.Info(serializedResponse);

            // return an html order cancelled template for use at world pay
            if (config.WorldPayProduceShopperCancellationResponse)
            {
                string htmlTemplate = ResourceHelper.GetMessageTemplate(CultureInfo.CurrentUICulture, config.WorldPayShopperCancellationResponseTemplate);
                StringBuilder finalOutput = new StringBuilder();
                finalOutput.Append(htmlTemplate);
                finalOutput.Replace("#WorldPayBannerToken", "<WPDISPLAY ITEM=banner>"); //required by worldpay
                finalOutput.Replace("#CustomerName", wpResponse.Name);
                finalOutput.Replace("#StoreName", store.Name);

                string storePageUrl = worldPayLog.RawResponse;

                finalOutput.Replace("#StorePageLink", "<a href='" + storePageUrl + "'>" + storePageUrl + "</a>");

                page.Response.Write(finalOutput.ToString());
                page.Response.Flush();

            }


        }
    }
}