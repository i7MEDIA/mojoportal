//  Author:                     
//  Created:                    2008-07-05
//	Last Modified:              2014-03-12
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
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using log4net;

namespace mojoPortal.Business.WebHelpers.PaymentGateway
{
    /// <summary>
    ///  
    /// </summary>
    public class PayPalStandardPaymentGateway
    {
        #region Constructors

        public PayPalStandardPaymentGateway(
            string payPalStandardUrl,
            string businessEmail,
            string pdtId
            )
        {
            this.payPalStandardUrl = payPalStandardUrl;
            this.businessEmail = businessEmail;
            this.pdtId = pdtId;

            if (this.payPalStandardUrl.Length == 0)
            {
                throw new ArgumentException("payPalStandardUrl must be provided");
            }

            if (this.businessEmail.Length == 0)
            {
                throw new ArgumentException("businessEmail must be provided");
            }

            items = new List<PayPalOrderItem>();
        }

        #endregion


        #region Properties

        #region Private Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(PayPalStandardPaymentGateway));

        private string payPalStandardUrl = string.Empty;
        private string businessEmail = string.Empty;
        private decimal amount = 0;
        private decimal tax = 0;
        private decimal shipping = 0;
        private decimal cartDiscount = 0;
        private string currencyCode = "USD";
        private bool orderHasShippableProducts = false;
        private string shippingFirstName = string.Empty;
        private string shippingLastName = string.Empty;
        private string shippingAddress1 = string.Empty;
        private string shippingAddress2 = string.Empty;
        private string shippingCity = string.Empty;
        private string shippingState = string.Empty;
        private string shippingPostalCode = string.Empty;
        private string custom = string.Empty;
        private string returnUrl = string.Empty;
        private string cancelUrl = string.Empty;
        private string notificationUrl = string.Empty;
        private string pdtId = string.Empty;
        private string transactionId = string.Empty;
        private string ipnForm = string.Empty;
        private string orderDescription = string.Empty;
        private List<PayPalOrderItem> items = null;

        

        #endregion

        #region Public Properties

        public List<PayPalOrderItem> Items
        {
            get { return items; }
            set { items = value; }
        }

        public string TransactionId
        {
            get { return transactionId; }
            set { transactionId = value; }
        }

        public string OrderDescription
        {
            get { return orderDescription; }
            set 
            {
                if (value.Length > 127)
                {
                    orderDescription = value.Substring(0, 127);
                }
                else
                {
                    orderDescription = value;
                }
            }
        }

        public string IPNForm
        {
            get { return ipnForm; }
            set { ipnForm = value; }
        }

        

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public decimal Tax
        {
            get { return tax; }
            set { tax = value; }
        }

        public decimal Shipping
        {
            get { return shipping; }
            set { shipping = value; }
        }

        public decimal CartDiscount
        {
            get { return cartDiscount; }
            set { cartDiscount = value; }
        }

        public string CurrencyCode
        {
            get { return currencyCode; }
            set { currencyCode = value; }
        }

        public bool OrderHasShippableProducts
        {
            get { return orderHasShippableProducts; }
            set { orderHasShippableProducts = value; }
        }


        public string ShippingFirstName
        {
            get { return shippingFirstName; }
            set { shippingFirstName = value; }
        }

        public string ShippingLastName
        {
            get { return shippingLastName; }
            set { shippingLastName = value; }
        }

        public string ShippingAddress1
        {
            get { return shippingAddress1; }
            set { shippingAddress1 = value; }
        }

        public string ShippingAddress2
        {
            get { return shippingAddress2; }
            set { shippingAddress2 = value; }
        }

        public string ShippingCity
        {
            get { return shippingCity; }
            set { shippingCity = value; }
        }

        public string ShippingState
        {
            get { return shippingState; }
            set { shippingState = value; }
        }

        public string ShippingPostalCode
        {
            get { return shippingPostalCode; }
            set { shippingPostalCode = value; }
        }

        public string Custom
        {
            get { return custom; }
            set { custom = value; }
        }

        public string ReturnUrl
        {
            get { return returnUrl; }
            set { returnUrl = value; }
        }

        public string CancelUrl
        {
            get { return cancelUrl; }
            set { cancelUrl = value; }
        }

        public string NotificationUrl
        {
            get { return notificationUrl; }
            set { notificationUrl = value; }
        }

        #endregion


        #endregion

        #region Public Methods

        /// <summary>
        /// This builds an url for buy now button links.
        /// </summary>
        /// <returns></returns>
        public string GetBuyNowButtonUrl()
        {
            StringBuilder url = new StringBuilder();

            url.Append(payPalStandardUrl);

            url.Append("?cmd=_xclick");

            url.Append("&currency_code=" + currencyCode);

            if (orderDescription.Length == 0)
            {
                throw new ArgumentException("orderDescription must be provided");
            }

            url.Append("&business=" + HttpUtility.UrlEncode(businessEmail));

            
            if (amount == 0)
            {
                throw new ArgumentException("Amount must be greater than 0");
            }

            amount = Math.Round(amount, 2);

            url.Append("&amount=" + amount.ToString().Replace(",", "."));

            
            if (tax > 0)
            {
                tax = Math.Round(tax, 2);
                url.Append("&tax=" + tax.ToString().Replace(",", "."));
            }

            if (shipping > 0)
            {
                shipping = Math.Round(shipping, 2);
                url.Append("&shipping=" + shipping.ToString().Replace(",", "."));
            }


            if (orderHasShippableProducts)
            {
                url.Append("&no_shipping=1");
                url.Append("&first_name=" + HttpUtility.UrlEncode(shippingFirstName));
                url.Append("&last_name=" + HttpUtility.UrlEncode(shippingLastName));
                url.Append("&address1=" + HttpUtility.UrlEncode(shippingAddress1));
                url.Append("&address2=" + HttpUtility.UrlEncode(shippingAddress2));
                url.Append("&city=" + HttpUtility.UrlEncode(shippingCity));
                url.Append("&state=" + HttpUtility.UrlEncode(shippingState));
                url.Append("&zip=" + HttpUtility.UrlEncode(shippingPostalCode));

            }

            url.Append("&item_name=" + HttpUtility.UrlEncode(orderDescription));
            // the cartguid/orderguid
            url.Append("&item_number=" + HttpUtility.UrlEncode(custom));

            //if(items.Count == 0)
            //{
            //    throw new ArgumentException("must have at least one PayPalOrderItem");
            //}
            
            //string sItemNum = string.Empty;
            //int index = 1;
            //decimal itemAmount = 0;
            //foreach (PayPalOrderItem item in items)
            //{
            //    sItemNum = index.ToString();
            //    itemAmount = Math.Round(item.Amount, 2);

            //    url.Append("&item_name_" + sItemNum + "=" + HttpUtility.UrlEncode(item.ItemName));
            //    url.Append("&quantity_" + sItemNum + "=" + HttpUtility.UrlEncode(item.Quantity.ToString(CultureInfo.InvariantCulture)));
            //    url.Append("&item_number_" + sItemNum + "=" + HttpUtility.UrlEncode(item.ItemNumber));
            //    url.Append("&amount_" + sItemNum + "=" + HttpUtility.UrlEncode(itemAmount.ToString().Replace(",", ".")));

            //    index++;
            //}
            
            url.Append("&custom=" + HttpUtility.UrlEncode(custom));

            if (returnUrl.Length > 0)
            {
                url.Append("&return=" + HttpUtility.UrlEncode(returnUrl));
            }

            if (cancelUrl.Length > 0)
            {
                url.Append("&cancel_return=" + HttpUtility.UrlEncode(cancelUrl));
            }

            if (notificationUrl.Length > 0)
            {
                url.Append("&notify_url=" + HttpUtility.UrlEncode(notificationUrl));
            }

            url.Append("&bn=SourceTreeSolutions_SP");

            return url.ToString();

        }

        /// <summary>
        /// This builds a string of the html form fields for posting to PayPal for Standard Checkout Cart Upload
        /// </summary>
        /// <returns></returns>
        public string GetCartUploadFormFields()
        {
            if (items.Count == 0)
            {
                throw new ArgumentException("must have at least one PayPalOrderItem");
            }

            if (amount == 0)
            {
                throw new ArgumentException("Amount must be greater than 0");
            }

            StringBuilder formVars = new StringBuilder();

            
            formVars.Append("<input type='hidden' name='cmd' value='_cart' /> ");
            formVars.Append("<input type='hidden' name='upload' value='1' /> ");
            formVars.Append("<input type='hidden' name='business' value='" + HttpUtility.HtmlAttributeEncode(businessEmail) + "' /> ");

            formVars.Append("<input type='hidden' name='currency_code' value='" + currencyCode + "' /> ");
            //identifies the commerce application vendor
            formVars.Append("<input name='bn' value='SourceTreeSolutions_SP' type='hidden' />");

            // display the cart to the customer
            //formVars.Append("<input type='hidden' name='display' value='1' /> ");
            
            amount = Math.Round(amount, 2);

            //formVars.Append("<input type='hidden' name='amount' value='" + amount.ToString().Replace(",", ".") + "' /> ");
            
            //if (tax > 0)
            //{
            //    tax = Math.Round(tax, 2);
            //    formVars.Append("<input type='hidden' name='tax' value='" + tax.ToString().Replace(",", ".") + "' /> ");
            //}

            //if (shipping > 0)
            //{
            //    shipping = Math.Round(shipping, 2);
            //    formVars.Append("<input type='hidden' name='shipping' value='" + shipping.ToString().Replace(",", ".") + "' /> ");
            //}

            if (cartDiscount > 0)
            {
                cartDiscount = Math.Round(cartDiscount, 2);
                formVars.Append("<input type='hidden' name='discount_amount_cart' value='" + cartDiscount.ToString().Replace(",", ".") + "' /> ");
            }

            if (shippingFirstName.Length > 0)
            {
                formVars.Append("<input type='hidden' name='first_name' value='"
                    + HttpUtility.HtmlAttributeEncode(shippingFirstName) + "' /> ");
            }

            if (shippingLastName.Length > 0)
            {
                formVars.Append("<input type='hidden' name='last_name' value='"
                    + HttpUtility.HtmlAttributeEncode(shippingLastName) + "' /> ");
            }

            if (shippingAddress1.Length > 0)
            {
                formVars.Append("<input type='hidden' name='address1' value='"
                    + HttpUtility.HtmlAttributeEncode(shippingAddress1) + "' /> ");
            }

            if (shippingAddress2.Length > 0)
            {
                formVars.Append("<input type='hidden' name='address2' value='"
                    + HttpUtility.HtmlAttributeEncode(shippingAddress2) + "' /> ");
            }

            if (shippingCity.Length > 0)
            {
                formVars.Append("<input type='hidden' name='city' value='"
                    + HttpUtility.HtmlAttributeEncode(shippingCity) + "' /> ");
            }

            if (shippingState.Length > 0)
            {
                formVars.Append("<input type='hidden' name='state' value='"
                    + HttpUtility.HtmlAttributeEncode(shippingState) + "' /> ");
            }

            if (shippingPostalCode.Length > 0)
            {
                formVars.Append("<input type='hidden' name='zip' value='"
                    + HttpUtility.HtmlAttributeEncode(shippingPostalCode) + "' /> ");
            }

            // this tells PayPal NOT to prompt user for shipping address
            // when we implement support for shippable products need to decide whether this is the best idea.
            // Do we just rely on the shipping address we pass to PayPal? 
            // or should we allow the user to enter it there
            // can PayPal do the shipping cost calculation?
            // 2008-07-15 since the idea from PayPal's point of view is that if they use PayPal 
            // they should not have to enter shipping/billing address on our site
            // as a convenience to the user for using PayPal. so lets defer to the shipping address they provide if they handle the order
            // not sure how we can set that up yet for calculating the shipping costs
            // if this is set to 1 then no address is returned by PayPal in the PDT or IPN
            //if (shippingAddress1.Length > 0)
            //{
            //    formVars.Append("<input type='hidden' name='no_shipping' value='1' /> ");
            //}

            //formVars.Append("<input type='hidden' name='item_name' value='" + HttpUtility.HtmlEncode(orderDescription) + "' /> ");
            //formVars.Append("<input type='hidden' name='item_number' value='" + HttpUtility.HtmlEncode(custom) + "' /> ");
            formVars.Append("<input type='hidden' name='custom' value='" + HttpUtility.HtmlAttributeEncode(custom) + "' /> ");

            string sItemNum = string.Empty;
            int index = 1;
            decimal itemAmount = 0;
            decimal taxAmount = 0;

            foreach (PayPalOrderItem item in items)
            {
                sItemNum = index.ToString();
                itemAmount = Math.Round(item.Amount, 2);
                taxAmount = Math.Round(item.Tax, 2);

                formVars.Append("<input type='hidden' name='item_name_" + sItemNum
                    + "' value='" + HttpUtility.HtmlAttributeEncode(item.ItemName) + "' /> ");

                formVars.Append("<input type='hidden' name='item_number_" + sItemNum
                    + "' value='" + HttpUtility.HtmlAttributeEncode(item.ItemNumber) + "' /> ");

                formVars.Append("<input type='hidden' name='quantity_" + sItemNum
                    + "' value='" + HttpUtility.HtmlAttributeEncode(item.Quantity.ToString(CultureInfo.InvariantCulture)) + "' /> ");

                formVars.Append("<input type='hidden' name='amount_" + sItemNum
                    + "' value='" + HttpUtility.HtmlAttributeEncode(itemAmount.ToString().Replace(",", ".")) + "' /> ");

                if (taxAmount > 0)
                {
                    formVars.Append("<input type='hidden' name='tax_" + sItemNum
                        + "' value='" + HttpUtility.HtmlAttributeEncode(taxAmount.ToString().Replace(",", ".")) + "' /> ");
                }

                index++;
            }



            if (returnUrl.Length > 0)
            {
                formVars.Append("<input type='hidden' name='return' value='" + returnUrl + "' /> ");
            }

            if (cancelUrl.Length > 0)
            {
                formVars.Append("<input type='hidden' name='cancel_return' value='" + cancelUrl + "' /> ");
            }

            if (notificationUrl.Length > 0)
            {
                formVars.Append("<input type='hidden' name='notify_url' value='" + notificationUrl + "' /> ");
            }

            return formVars.ToString();

        }


        /// <summary>
        /// Synchronizes the specified args.
        /// </summary>
        public string ValidatePDT()
        {
            if (transactionId.Length == 0)
            {
                throw new ArgumentException("transactionId must be provided");
            }

            if (payPalStandardUrl.Length == 0)
            {
                throw new ArgumentException("payPalStandardUrl must be provided");
            }

            string request = "&cmd=_notify-synch&tx=" + transactionId;
            string response = string.Empty;

            if (pdtId.Length > 0)
            {
                request += "&at=" + this.pdtId;
            }
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] buffer = encoding.GetBytes(request);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(payPalStandardUrl);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = request.Length;

            Stream sendStream = webRequest.GetRequestStream();
            sendStream.Write(buffer, 0, buffer.Length);
            sendStream.Close();

            StreamReader responseStream = new StreamReader(webRequest.GetResponse().GetResponseStream());
            response = responseStream.ReadToEnd();
            responseStream.Close();

            response = HttpUtility.UrlDecode(response);
            return response;

        }

        /// <summary>
        /// Synchronizes the specified args.
        /// </summary>
        public string ValidateIPN()
        {

            if (ipnForm.Length == 0)
            {
                throw new ArgumentException("ipnForm must be provided");
            }

            if (payPalStandardUrl.Length == 0)
            {
                throw new ArgumentException("payPalStandardUrl must be provided");
            }

         
            string request = string.Format("{0}&cmd=_notify-validate", ipnForm);
            string response = string.Empty;

            
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] buffer = encoding.GetBytes(request);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(payPalStandardUrl);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = request.Length;

            Stream sendStream = webRequest.GetRequestStream();
            sendStream.Write(buffer, 0, buffer.Length);
            sendStream.Close();

            StreamReader responseStream = new StreamReader(webRequest.GetResponse().GetResponseStream());
            response = responseStream.ReadToEnd();
            responseStream.Close();

            response = HttpUtility.UrlDecode(response);
            return response;

        }

        /// <summary>
        /// parses the form key value pairs into a StringDictionary
        /// </summary>
        /// <param name="pdt"></param>
        /// <returns></returns>
        public static StringDictionary GetPDTValues(string pdt)
        {
            StringDictionary responseResults = new StringDictionary();
            string[] keys = pdt.Split('\n');
            
            foreach (string s in keys)
            {
                string[] keyValuePair = s.Split('=');
                if (keyValuePair.Length > 1)
                {
                    responseResults.Add(keyValuePair[0], keyValuePair[1]);
                }
            }

            return responseResults;

        }

        


        #endregion

    }
}
