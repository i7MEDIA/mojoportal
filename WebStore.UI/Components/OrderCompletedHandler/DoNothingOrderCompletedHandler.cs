// Author:					Joe Audette
// Created:					2011-10-11
// Last Modified:			2011-10-11
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using log4net;
using System.Web;
using mojoPortal.Business;
using mojoPortal.Web;
using mojoPortal.Net;
using WebStore.Business;
using WebStore.UI;

namespace WebStore.Components
{
    /// <summary>
    /// the provider collection cannot be empty so this donothingprovider is intended so that at least one provider exists
    /// but also to provide some example code for those who wish to implement there own handlers.
    /// 
    /// For example if you sell products that require some kind of activation code, you could implement a custom handler
    /// to check the order for the product and if needed generate an activation key and email it to the customer
    /// 
    /// you would implement your own handler in your own class library project, compile it into a dll, put it in the /bin folder
    /// then create an xml file to declare your handler and put it in the /WebStore/OrderCompletedHandlers folder
    /// you will find an example xml file for this donothinghandler in that folder so you can see how to declare yours.
    /// 
    /// NOTE: all existing providers will be called for all orders after the payment is completed, not upon order creation
    /// for example if a user checks out with PayPal or google we will first get notification of the order having been received
    /// at that point an order is created but its status is OrderReceived.
    /// after the payment actually clears we get another notification from PayPal or Google and at this point the order status will be set to fulfillable
    /// and the ordercompleted event handlers will be fired, passing in the order as an event argument.
    /// It is also possible that the order could be cancelled at PayPal or Google before payment is cleared, therefore we wait 
    /// until payment has cleared before firing the order completed handlers.
    /// 
    /// 
    /// </summary>
    public class DoNothingOrderCompletedHandler : OrderCompletedHandlerProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DoNothingOrderCompletedHandler));

        public DoNothingOrderCompletedHandler()
        { }

        public override void HandelOrderCompleted(OrderCompletedEventArgs e)
        {
            if (e == null) return;
            if (e.Order == null) return;
            // do nothing

            if (!WebStoreConfiguration.LogDoNothingOrderHandler) { return; }

            log.Info("DoNothingOrderCompletedHandler called");

            StringBuilder orderDetails = new StringBuilder();
            orderDetails.Append("Order from " + e.Order.CustomerFirstName + " " + e.Order.CustomerLastName);
            orderDetails.Append("\r\n");

            // you can also get the customer information from the order
            // e.Order.BillingLastName
            // e.Order.BillingFirstName
            // e.Order.CustomerEmail
            // etc, lots of other properties on the order object

            // this DataSet has 2 relaqted tables Offers and Products
            // where the products correspond to the products included in the offer(s)
            DataSet dsOffers = Order.GetOrderOffersAndProducts(e.Order.StoreGuid, e.Order.OrderGuid);

            foreach (DataRow row in dsOffers.Tables["Offers"].Rows)
            {
                string oguid = row["OfferGuid"].ToString();
                orderDetails.Append(row["Name"].ToString() + " ");
                orderDetails.Append(row["Quantity"].ToString() + " @ ");
                orderDetails.Append(string.Format(e.CurrencyCulture, "{0:c}", Convert.ToDecimal(row["OfferPrice"])));
                orderDetails.Append("\r\n");

                string whereClause = string.Format("OfferGuid = '{0}'", oguid);
                DataView dv = new DataView(dsOffers.Tables["Products"], whereClause, "", DataViewRowState.CurrentRows);

                if (dv.Count > 1)
                {
                    foreach (DataRow r in dsOffers.Tables["Products"].Rows)
                    {
                        string poguid = r["OfferGuid"].ToString();
                        if (oguid == poguid)
                        {
                            orderDetails.Append(r["Name"].ToString() + " ");
                            orderDetails.Append(r["Quantity"].ToString() + "  \r\n");

                        }

                    }
                }

            }

            log.Info(orderDetails.ToString());

        }


    }
}