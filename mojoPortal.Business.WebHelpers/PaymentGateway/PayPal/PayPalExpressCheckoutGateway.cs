// Author:					
// Created:				    2008-01-16
// Last Modified:		    2014-09-26
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using log4net;
using mojoPortal.Web.Framework;

namespace mojoPortal.Business.WebHelpers.PaymentGateway
{
    /// <summary>
    /// PayPal Express Checkout
    /// 
    /// Flow: Call SetExpressCheckout to get a token then redirect to paypal with the token.
    /// When PayPal redirects back to your landing page you make a server side request with
    /// GetExpressCheckoutDetails passing the token and PalPal respond swith the payment status and details.
    /// On success you can either give the user a final chance to review the order
    /// or just make the DoExpressCheckoutPayment request to PayPal and process the order
    /// In either case you make the same call but if you want the user to review the order 
    /// again then do it from a user button click
    /// 
    /// </summary>
    public class PayPalExpressGateway //: IPaymentGateway
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PayPalExpressGateway));

        #region Constructors

        public PayPalExpressGateway(
            string apiLogin,
            string apiPassword,
            string apiTransactionKey,
            string payPalMerchantEmailAddress)
        {
            if (apiLogin != null) merchantAPILogin = apiLogin;
            if (apiPassword != null) merchantAPIPassword = apiPassword;
            if (apiTransactionKey != null) merchantAPITransactionKey = apiTransactionKey;
            if (payPalMerchantEmailAddress != null) merchantPayPalEmailAddress = payPalMerchantEmailAddress;

        }

        #endregion

        #region Private Properties

       
        private PaymentGatewayResponse response = PaymentGatewayResponse.NoRequestInitiated;
        private string testUrl = "https://api-3t.sandbox.paypal.com/nvp";
        private string productionUrl = "https://api-3t.paypal.com/nvp";

        //private string payPalExpressProductionCheckoutUrl = "https://www.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=";
        //private string payPalExpressSandboxCheckoutUrl = "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=";

        private string payPalExpressProductionCheckoutUrl = Core.Configuration.ConfigHelper.GetStringProperty(
            "PayPalExpressProductionUrl", 
            "https://www.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=");

        private string payPalExpressSandboxCheckoutUrl = Core.Configuration.ConfigHelper.GetStringProperty(
            "PayPalExpressSandboxUrl", 
            "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=");
        
        
        private bool useTestMode = false;

        private string rawResponse = string.Empty;
        private int timeoutInMilliseconds = 120000; // 120 seconds
        private Exception lastExecutionException = null;

        private string merchantAPILogin = string.Empty;
        private string merchantAPIPassword = string.Empty;
        private string merchantAPITransactionKey = string.Empty;
        private string merchantPayPalEmailAddress = string.Empty;

        private string merchantEmail = string.Empty;
        private string merchantInvoiceNumber = string.Empty;
        private string merchantTransactionDescription = string.Empty;
        private string merchantEmailConfirmationHeader = string.Empty;
        private string merchantEmailConfirmationFooter = string.Empty;

        private string returnUrl = string.Empty;
        private string cancelUrl = string.Empty;
        private string notificationUrl = string.Empty;
        private string orderDescription = string.Empty;
        private string merchantCartId = string.Empty;

        private string shipToSalutation = string.Empty;
        private string shipToFirstName = string.Empty;
        private string shipToLastName = string.Empty;
        private string shipToMiddleName = string.Empty;
        private string shipToNameSuffix = string.Empty;
        private string shipToCompanyName = string.Empty;
        private string shipToAddress = string.Empty;
        private string shipToAddress2 = string.Empty;
        private string shipToCity = string.Empty;
        private string shipToState = string.Empty;
        private string shipToPostalCode = string.Empty;
        private string shipToCountry = string.Empty;
        private string shipToPhone = string.Empty;
        private string shipToAddressStatus = string.Empty;
        
        private string transactionID = string.Empty;
        private string reasonCode = string.Empty;
        private string responseReason = string.Empty;
        
        private string currencyCode = "USD";
        private decimal chargeTotal = 0;

        private bool requireConfirmedShippingAddress = false;
        private bool noShipping = false;
        private bool overrideShippingAddress = false;
        private string payPalExpressUrl = string.Empty;
        private string payPalToken = string.Empty;
        private string payPalPayerId = string.Empty;
        private string payPalPayerStatus = "unverified";
        private string payPalPaymentType = string.Empty;
        private decimal payPalFeeAmount = 0;
        private decimal payPalSettlementAmount = 0;
        private decimal payPalTaxTotal = 0;
        private string payPalExchangeRate = string.Empty;
        private string payPalPaymentStatus = string.Empty;
        private string payPalPendingReason = string.Empty;
        private string payPalTransactionType = string.Empty;
        private DateTime payPalOrderTimeStamp = DateTime.UtcNow;
        private string buyerEmail = string.Empty;
        private string apiVersion = "3.2";


        #endregion

        #region Public Properties

        /// <summary>
        /// If this is populated by SetExpressCheckout you redirect to it
        /// </summary>
        public string PayPalExpressUrl
        {
            get { return payPalExpressUrl; }
        }

        public string PayPalPayerStatus
        {
            get { return payPalPayerStatus; }
        }

        public DateTime PayPalOrderTimeStamp
        {
            get { return payPalOrderTimeStamp; }
        }

        /// <summary>
        /// none, echeck, instant
        /// </summary>
        public string PayPalPaymentType
        {
            get { return payPalPaymentType; }
        }

        /// <summary>
        /// cart, express-checkout
        /// </summary>
        public string PayPalTransactionType
        {
            get { return payPalTransactionType; }
        }

        public decimal PayPalFeeAmount
        {
            get { return payPalFeeAmount; }
        }

        public decimal PayPalSettlementAmount
        {
            get { return payPalSettlementAmount; }
        }

        public decimal PayPalTaxTotal
        {
            get { return payPalTaxTotal; }
        }

        public string PayPalExchangeRate
        {
            get { return payPalExchangeRate; }
        }

        public string PayPalPaymentStatus
        {
            get { return payPalPaymentStatus; }
        }

        public string PayPalPendingReason
        {
            get { return payPalPendingReason; }
        }

        /// <summary>
        /// If this is populated then we are ready to redirect
        /// The PayPalExpressUrl will include this param but you can read
        /// it form here to store it
        /// </summary>
        public string PayPalToken
        {
            get { return payPalToken; }
            set { payPalToken = value; }

        }

        public string PayPalPayerId
        {
            get { return payPalPayerId; }
            set { payPalPayerId = value; }

        }

        /// <summary>
        /// PayPal can pre-populate the login form at their site with this
        /// </summary>
        public string BuyerEmail
        {
            get { return buyerEmail; }
            set { buyerEmail = value; }

        }

        /// <summary>
        /// Required url for PayPal to redirect 
        /// URL to which the customer’s browser is returned after choosing to pay with PayPal
        /// </summary>
        public string ReturnUrl
        {
            get { return returnUrl; }
            set { returnUrl = value; }
        }

        /// <summary>
        /// URL to which the customer is returned if he does not approve the use of
        /// PayPal to pay you.
        /// </summary>
        public string CancelUrl
        {
            get { return cancelUrl; }
            set { cancelUrl = value; }
        }

        /// <summary>
        /// for PayPal to post instant payment notification IPN
        /// 
        /// </summary>
        public string NotificationUrl
        {
            get { return notificationUrl; }
            set { notificationUrl = value; }
        }

        /// <summary>
        /// Description of items the customer is purchasing
        /// 
        /// </summary>
        public string OrderDescription
        {
            get { return orderDescription; }
            set { orderDescription = value; }
        }

        /// <summary>
        /// Assing the cart id string here
        /// it will be passed back to use by PayPal
        /// so we can use it to lookup our cart
        /// </summary>
        public string MerchantCartId
        {
            get { return merchantCartId; }
            set { merchantCartId = value; }
        }

        /// <summary>
        /// 
        /// indicates that you require that the customer’s shipping
        /// address on file with PayPal be a confirmed address.
        /// NOTE: Setting this field overrides the setting you have specified in your
        /// Merchant Account Profile.
        /// 
        /// </summary>
        public bool RequireConfirmedShippingAddress
        {
            get { return requireConfirmedShippingAddress; }
            set { requireConfirmedShippingAddress = value; }
        }

        /// <summary>
        /// 
        /// true indicates that on the PayPal pages, no shipping address fields
        /// should be displayed whatsoever.
        /// 
        /// </summary>
        public bool NoShipping
        {
            get { return noShipping; }
            set { noShipping = value; }
        }

        /// <summary>
        /// 
        /// true indicates that on the PayPal pages, no shipping address fields
        /// should be displayed whatsoever.
        /// 
        /// </summary>
        public bool OverrideShippingAddress
        {
            get { return overrideShippingAddress; }
            set { overrideShippingAddress = value; }
        }

        public string TestUrl
        {
            get { return testUrl; }
        }

        public string ProductionUrl
        {
            get { return productionUrl; }
        }

        public string ShipToFirstName
        {
            get { return shipToFirstName; }
            set { shipToFirstName = value; }
        }

        public string ShipToLastName
        {
            get { return shipToLastName; }
            set { shipToLastName = value; }
        }

        public string ShipToSalutation
        {
            get { return shipToSalutation; }
        }

        public string ShipToMiddleName
        {
            get { return shipToMiddleName; }
        }

        public string ShipToNameSuffix
        {
            get { return shipToNameSuffix; }
        }

        public string ShipToCompanyName
        {
            get { return shipToCompanyName; }
            set { shipToCompanyName = value; }
        }

        public string ShipToAddress
        {
            get { return shipToAddress; }
            set { shipToAddress = value; }
        }

        public string ShipToAddress2
        {
            get { return shipToAddress2; }
            set { shipToAddress2 = value; }
        }

        public string ShipToCity
        {
            get { return shipToCity; }
            set { shipToCity = value; }
        }

        public string ShipToState
        {
            get { return shipToState; }
            set { shipToState = value; }
        }

        public string ShipToPostalCode
        {
            get { return shipToPostalCode; }
            set { shipToPostalCode = value; }
        }

        public string ShipToCountry
        {
            get { return shipToCountry; }
            set { shipToCountry = value; }
        }

        public string ShipToPhone
        {
            get { return shipToPhone; }
            set { shipToPhone = value; }
        }

        public string ShipToAddressStatus
        {
            get { return shipToAddressStatus; }
        }

        public string MerchantEmail
        {
            get { return merchantEmail; }
            set { merchantEmail = value; }
        }

        public string MerchantInvoiceNumber
        {
            get { return merchantInvoiceNumber; }
            set { merchantInvoiceNumber = value; }
        }

        public string MerchantTransactionDescription
        {
            get { return merchantTransactionDescription; }
            set { merchantTransactionDescription = value; }
        }

        public string MerchantEmailConfirmationHeader
        {
            get { return merchantEmailConfirmationHeader; }
            set { merchantEmailConfirmationHeader = value; }
        }

        public string MerchantEmailConfirmationFooter
        {
            get { return merchantEmailConfirmationFooter; }
            set { merchantEmailConfirmationFooter = value; }
        }

        public string CurrencyCode
        {
            get { return currencyCode; }
            set { currencyCode = value; }
        }

        public decimal ChargeTotal
        {
            get { return chargeTotal; }
            set { chargeTotal = value; }
        }

        public string TransactionId
        {
            get { return transactionID; }
        }

        public string ReasonCode
        {
            get { return reasonCode; }
        }

        public string ResponseReason
        {
            get { return responseReason; }
        }

        public PaymentGatewayResponse Response
        {
            get { return response; }
        }

        public bool UseTestMode
        {
            get { return useTestMode; }
            set { useTestMode = value; }
        }

        public string RawResponse
        {
            get { return rawResponse; }
            set { rawResponse = value; }
        }

        public Exception LastExecutionException
        {
            get { return lastExecutionException; }
        }

        public int TimeoutInMilliseconds
        {
            get { return timeoutInMilliseconds; }
            set { timeoutInMilliseconds = value; }
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Pass order info to paypal and recieve back a token
        /// then redirect to PayPal passing the token.
        /// When PayPal redirects back to your site call
        /// GetExpressCheckoutDetails passing the token
        /// </summary>
        /// <returns></returns>
        public bool CallSetExpressCheckout()
        {
            bool result = false;

            StringBuilder requestBody = new StringBuilder();
            requestBody.Append("USER=" + HttpUtility.UrlEncode(merchantAPILogin));
            requestBody.Append("&PWD=" + HttpUtility.UrlEncode(merchantAPIPassword));
            requestBody.Append("&SIGNATURE=" + HttpUtility.UrlEncode(merchantAPITransactionKey));
            requestBody.Append("&VERSION=" + HttpUtility.UrlEncode(apiVersion));
            requestBody.Append("&METHOD=SetExpressCheckout");

            // ## REQUIRED 
            //AMT is deprecated since version 63.0. Use PAYMENTREQUEST_0_AMT instead.
            requestBody.Append("&AMT=" + HttpUtility.UrlEncode(FormatCharge()));
            //requestBody.Append("&PAYMENTREQUEST_0_AMT=" + HttpUtility.UrlEncode(FormatCharge()));

            //Sum of cost of all items in this order. For digital goods, this field is required.
            //requestBody.Append("&PAYMENTREQUEST_0_ITEMAMT=" + HttpUtility.UrlEncode(FormatCharge()));

            if (returnUrl.Length == 0) throw new ArgumentException("ReturnUrl must be provided");

            // TODO: do we need to urlencode here?
            requestBody.Append("&RETURNURL=" + HttpUtility.UrlEncode(returnUrl));
            // URL to which the customer’s browser is returned after choosing to pay
            // with PayPal.
            // NOTE: PayPal recommends that the value be the final review page on
            // which the customer confirms the order and payment or billing
            // agreement.

            if (cancelUrl.Length == 0) throw new ArgumentException("CancelUrl must be provided");

            // TODO: do we need to urlencode here?
            requestBody.Append("&CANCELURL=" + HttpUtility.UrlEncode(cancelUrl));
            //CANCELURL URL to which the customer is returned if he does not approve the use of
            //PayPal to pay you.
            //NOTE: PayPal recommends that the value be the original page on which
            //the customer chose to pay with PayPal or establish a billing
            //agreement.

            // ## END REQUIRED

            requestBody.Append("&PAYMENTACTION=Sale");
            //How you want to obtain payment:

            // Sale indicates that this is a final sale for which you are requesting payment.

            // Authorization indicates that this payment is a basic authorization
            // subject to settlement with PayPal Authorization & Capture.

            // Order indicates that this payment is an order authorization subject to
            // settlement with PayPal Authorization & Capture.

            //If the transaction does not include a one-time purchase, this field is
            //ignored.
            //NOTE:You cannot set this value to Sale in SetExpressCheckout
            //request and then change this value to Authorization or Order
            //on the final API DoExpressCheckoutPayment request. If the
            //value is set to Authorization or Order in
            //SetExpressCheckout, the value may be set to Sale or the same
            //value (either Authorization or Order) in
            //DoExpressCheckoutPayment.
            //Character length and limit: Up to 13 single-byte alphabetic characters
            //Default value: Sale



            if (currencyCode.Length > 0)
            {
                //CURRENCYCODE is deprecated since version 63.0. Use PAYMENTREQUEST_n_CURRENCYCODE instead.
                requestBody.Append("&CURRENCYCODE=" + HttpUtility.UrlEncode(currencyCode));
                //requestBody.Append("&PAYMENTREQUEST_0_CURRENCYCODE=" + HttpUtility.UrlEncode(currencyCode));
            }

            //requestBody.Append("");

            //requestBody.Append("&MAXAMT=");
            //MAXAMT - optional The expected maximum total amount of the complete order, including
            // shipping cost and tax charges.

            //EMAIL customer email
            if (buyerEmail.Length > 0)
            {
                requestBody.Append("&EMAIL=" + HttpUtility.UrlEncode(buyerEmail));
            }

            // DESC order description
            // Description of items the customer is purchasing
            if (orderDescription.Length > 0)
            {
                requestBody.Append("&DESC=" + HttpUtility.UrlEncode(orderDescription));
            }

            //CUSTOM A free-form field for your own use, such as a tracking number or other
            //value you want PayPal to return on GetExpressCheckoutDetails
            //response and DoExpressCheckoutPayment response. max length 256
            if (merchantCartId.Length > 0)
            {
                requestBody.Append("&CUSTOM=" + HttpUtility.UrlEncode(merchantCartId));
            }

            //INVNUM 
            //Your own unique invoice or tracking number. PayPal returns this value to
            //you on DoExpressCheckoutPayment response.
            //If the transaction does not include a one-time purchase, this field is
            //ignored.
            if (merchantInvoiceNumber.Length > 0)
            {
                requestBody.Append("&INVNUM=" + HttpUtility.UrlEncode(merchantInvoiceNumber));
            }

            //REQCONFIRMSHIPPING  1 = require a validated address for shipping, default 0
            if (requireConfirmedShippingAddress)
            {
                requestBody.Append("&REQCONFIRMSHIPPING=1");

            }

            //NOSHIPPING 1 = true means dont show shipping options
            if (noShipping)
            {
                requestBody.Append("&NOSHIPPING=1");
            }

            // ADDROVERRIDE 1 = true 0 = false
            // indicates that the PayPal pages should display the shipping
            // address set by you
            if (overrideShippingAddress)
            {
                requestBody.Append("&ADDROVERRIDE=1");
            }

            //SHIPTOSTREET
            if (shipToAddress.Length > 0)
            {
                requestBody.Append("&SHIPTOSTREET=" + HttpUtility.UrlEncode(shipToAddress));

                //SHIPTONAME
                if (shipToLastName.Length > 0)
                {
                    requestBody.Append("&SHIPTONAME=" + HttpUtility.UrlEncode(shipToFirstName + " " + shipToLastName));
                }

                //SHIPTOSTREET2

                //SHIPTOCITY
                if (shipToCity.Length > 0)
                {
                    requestBody.Append("&SHIPTOCITY=" + HttpUtility.UrlEncode(shipToCity));
                }

                //SHIPTOSTATE
                if (shipToState.Length > 0)
                {
                    requestBody.Append("&SHIPTOSTATE=" + HttpUtility.UrlEncode(shipToState));
                }

                //SHIPTOCOUNTRYCODE
                if (shipToCountry.Length > 0)
                {
                    requestBody.Append("&SHIPTOCOUNTRYCODE=" + HttpUtility.UrlEncode(shipToCountry));
                }

                //SHIPTOZIP
                if (shipToPostalCode.Length > 0)
                {
                    requestBody.Append("&SHIPTOZIP=" + HttpUtility.UrlEncode(shipToPostalCode));
                }

                //PHONENUM
                if (shipToPhone.Length > 0)
                {
                    requestBody.Append("&PHONENUM=" + HttpUtility.UrlEncode(shipToPhone));
                }

            }

            //TOKEN -- not required here as PayPal will generate it ad provide it in the response
            // we will pass it for subsequent requests, ie GetCheckoutDetails DoPayment
            // A timestamped token by which you identify to PayPal that you are
            // processing this payment with Express Checkout.

            //LOCALECODE  optional
            //Locale of pages displayed by PayPal during Express Checkout
            // unknown values default to US
            // options AU DE FR IT GB ES US
            if (this.shipToCountry.Length > 0)
            {
                requestBody.Append("&LOCALECODE=" + HttpUtility.UrlEncode(shipToCountry));
            }

            //PAGESTYLE
            //Sets the Custom Payment Page Style for payment pages associated with
            //this button/link. This value corresponds to the HTML variable page_style
            //for customizing payment pages. The value is the same as the Page Style
            //Name you chose when adding or editing the page style from the Profile
            //subtab of the My Account tab of your PayPal account.
            //Character length and limitations: 30 single-byte alphabetic characters.

            //HDRIMG
            //URL for the image you want to appear at the top left of the payment page.
            //The image has a maximum size of 750 pixels wide by 90 pixels high.
            //PayPal recommends that you provide an image that is stored on a secure
            //(https) server. If you do not specify an image, the business name is
            //displayed.

            //HDRBORDERCOLOR
            //Sets the border color around the header of the payment page. The border
            //is a 2-pixel perimeter around the header space, which is 750 pixels wide
            //by 90 pixels high. By default, the color is black.
            //Character length and limitations: Six character HTML hexadecimal color
            //code in ASCII

            //HDRBACKCOLOR
            //Sets the background color for the header of the payment page. By default,
            //the color is white.
            //Character length and limitation: Six character HTML hexadecimal color
            //code in ASCII

            //GIROPAYSUCCESSURL
            //The URL on the merchant site to redirect to after a successful giropay
            //payment. .
            //Use this field only if you are using giropay or bank transfer payment
            //methods in Germany.

            //GIROPAYCANCELURL
            //The URL on the merchant site to redirect to after a giropay or bank
            //transfer payment is cancelled or fails.
            //Use this field only if you are using giropay or bank transfer payment
            //methods in Germany.

            //BANKTXNPENDINGURL
            //The URL on the merchant site to transfer to after a bank transfer payment.
            //Use this field only if you are using giropay or bank transfer payment
            //methods in Germany.

            //L_BILLINGTYPEn Type of billing agreement.
            //For recurring payments, this field is required and must be set to
            //RecurringPayments.

            //L_BILLING
            //AGREEMENT
            //DESCRIPTIONn
            //Description of goods or services associated with the billing agreement.
            //PayPal recommends that you provide a brief summary of the terms &
            //conditions of the billing agreement

            //L_CUSTOMn Custom annotation field for your own use.
            //NOTE: This field is ignored for recurring payments

            //L_PAYMENTTYPEn Specifies type of PayPal payment you require for the billing agreement,
            //which is one of the following values.
            // Any
            //InstantOnly
            //NOTE: This field is ignored for recurring payments

            //IMPORTANT: To use SetExpressCheckout for recurring payments, you must set the
            //version parameter to 50.0 in your NVP API calls.

            //requestBody.Append("&VERSION=50.0");

            String url;
            if (useTestMode)
            {
                url = testUrl;
            }
            else
            {
                url = productionUrl;
            }

            StreamWriter requestStream = null;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "POST";

            webRequest.Timeout = timeoutInMilliseconds;

            webRequest.ContentType = "application/x-www-form-urlencoded";

            string encodedBody = requestBody.ToString();
            log.Debug(encodedBody);

            webRequest.ContentLength = encodedBody.Length;

            requestStream = new StreamWriter(webRequest.GetRequestStream());

            if (requestStream != null)
            {

                requestStream.Write(encodedBody);

            }

            if (requestStream != null)
                requestStream.Close();

            HttpWebResponse webResponse
                = (HttpWebResponse)webRequest.GetResponse();

            if (webResponse != null)
            {
                using (StreamReader responseStream =
                   new StreamReader(webResponse.GetResponseStream()))
                {
                    rawResponse = responseStream.ReadToEnd();
                    result = true;
                }


                ParseSetExpressCheckoutResponse();
            }
            else
            {

                response = PaymentGatewayResponse.Error;
                return false;
            }

            //After you receive a successful response from SetExpressCheckout, add the TOKEN from
            //SetExpressCheckout response as a name/value pair to the following URL, and redirect your
            //customer’s browser to it:
            //https://www.paypal.com/cgi-bin/webscr?cmd=_express-checkout&
            //token=value_from_SetExpressCheckoutResponse
            //For redirecting the customer’s browser to the PayPal login page, PayPal recommends that you
            //use the HTTPS response 302 “Object Moved” with the URL above as the value of the
            //Location header in the HTTPS response. Ensure that you use an SSL-enabled server to prevent
            //browser warnings about a mix of secure and insecure graphics.

            return result;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CallGetExpressCheckoutDetails()
        {
            bool result = false;

            StringBuilder requestBody = new StringBuilder();
            requestBody.Append("USER=" + HttpUtility.UrlEncode(merchantAPILogin));
            requestBody.Append("&PWD=" + HttpUtility.UrlEncode(merchantAPIPassword));
            requestBody.Append("&SIGNATURE=" + HttpUtility.UrlEncode(merchantAPITransactionKey));
            //if (merchantPayPalEmailAddress.Length > 0)
            //{
            //    requestBody.Append("&SUBJECT=" + HttpUtility.UrlEncode(merchantPayPalEmailAddress));
            //}
            requestBody.Append("&VERSION=" + HttpUtility.UrlEncode(apiVersion));

            //TOKEN A timestamped token, the value of which was returned by
            //SetExpressCheckout response.
            //Character length and limitations: 20 single-byte characters
            if (payPalToken.Length == 0) throw new ArgumentException("PayPalToken must be provided");

            requestBody.Append("&TOKEN=" + HttpUtility.UrlEncode(payPalToken));
            requestBody.Append("&METHOD=GetExpressCheckoutDetails");

            String url;
            if (useTestMode)
            {
                url = testUrl;
            }
            else
            {
                url = productionUrl;
            }

            StreamWriter requestStream = null;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "POST";

            webRequest.Timeout = timeoutInMilliseconds;

            webRequest.ContentType = "application/x-www-form-urlencoded";

            string encodedBody = requestBody.ToString();
            log.Debug(encodedBody);

            webRequest.ContentLength = encodedBody.Length;

            requestStream = new StreamWriter(webRequest.GetRequestStream());

            if (requestStream != null)
            {

                requestStream.Write(encodedBody);

            }

            if (requestStream != null)
                requestStream.Close();

            HttpWebResponse webResponse
                = (HttpWebResponse)webRequest.GetResponse();

            if (webResponse != null)
            {
                using (StreamReader responseStream =
                   new StreamReader(webResponse.GetResponseStream()))
                {
                    rawResponse = responseStream.ReadToEnd();
                    result = true;
                }


                ParseGetExpressCheckoutDetailsResponse();

            }
            else
            {

                response = PaymentGatewayResponse.Error;
                return false;
            }

            return result;

        }

        /// <summary>
        /// DoExpressCheckoutPayment Request
        /// Request to obtain payment with PayPal Express Checkout.
        /// IMPORTANT: PayPal requires that a merchant using Express Checkout display to the
        /// customer the same amount that the merchant sends to PayPal in the AMT
        /// parameter with the DoExpressCheckoutPayment request API.
        /// </summary>
        /// <returns></returns>
        public bool CallDoExpressCheckoutPayment()
        {
            bool result = false;

            StringBuilder requestBody = new StringBuilder();
            requestBody.Append("USER=" + HttpUtility.UrlEncode(merchantAPILogin));
            requestBody.Append("&PWD=" + HttpUtility.UrlEncode(merchantAPIPassword));
            requestBody.Append("&SIGNATURE=" + HttpUtility.UrlEncode(merchantAPITransactionKey));
            //requestBody.Append("&VERSION=3.2");
            requestBody.Append("&VERSION=" + HttpUtility.UrlEncode(apiVersion));
            requestBody.Append("&METHOD=DoExpressCheckoutPayment");

            //TOKEN
            if (payPalToken.Length == 0) throw new ArgumentException("PayPalToken must be provided");

            requestBody.Append("&TOKEN=" + HttpUtility.UrlEncode(payPalToken));

            //PAYMENTACTION
            requestBody.Append("&PAYMENTACTION=Sale");
            //PAYERID
            if (payPalPayerId.Length == 0) throw new ArgumentException("PayPalPayerId must be provided");

            requestBody.Append("&PAYERID=" + HttpUtility.UrlEncode(payPalPayerId));

            //Total of order, including shipping, handling, and tax.
            //AMT
            requestBody.Append("&AMT=" + HttpUtility.UrlEncode(FormatCharge()));

            // ## END REQUIRED

            // DESC order description
            // Description of items the customer is purchasing
            if (orderDescription.Length > 0)
            {
                requestBody.Append("&DESC=" + HttpUtility.UrlEncode(orderDescription));
            }

            //CUSTOM A free-form field for your own use, such as a tracking number or other
            //value you want PayPal to return on GetExpressCheckoutDetails
            //response and DoExpressCheckoutPayment response. max length 256
            if (merchantCartId.Length > 0)
            {
                requestBody.Append("&CUSTOM=" + HttpUtility.UrlEncode(merchantCartId));
            }

            //INVNUM 
            //Your own unique invoice or tracking number. PayPal returns this value to
            //you on DoExpressCheckoutPayment response.
            //If the transaction does not include a one-time purchase, this field is
            //ignored.
            if (merchantInvoiceNumber.Length > 0)
            {
                requestBody.Append("&INVNUM=" + HttpUtility.UrlEncode(merchantInvoiceNumber));
            }

            //BUTTONSOURCE -
            //An identification code for use by third-party applications to identify
            // transactions.
            //
            requestBody.Append("&BUTTONSOURCE=SourceTreeSolutions_SP");

            //NOTIFYURL
            //Your URL for receiving Instant Payment Notification (IPN) about this
            //transaction.
            //NOTE: If you do not specify this value in the request, the notification URL
            //from your Merchant Profile is used, if one exists.
            if (notificationUrl.Length > 0)
            {
                requestBody.Append("&NOTIFYURL=" + HttpUtility.UrlEncode(notificationUrl));
            }

            //ITEMAMT Sum of cost of all items in this order.
            //SHIPPINGAMT Total shipping costs for this order.

            //HANDLINGAMT Total handling costs for this order.

            //TAXAMT Sum of tax for all items in this order.

            // CURRENCYCODE default is USD
            if (currencyCode.Length > 0)
            {
                requestBody.Append("&CURRENCYCODE=" + HttpUtility.UrlEncode(currencyCode));
            }

            // L_NAMEn Item name. max length 127
            //L_NUMBERn Item number.
            //L_QTYn Item quantity.
            //L_TAXAMTn Item sales tax.
            //L_AMTn Cost of item
            //L_EBAYITEMNUMBERn Auction item number
            //L_EBAYITEMAUCTIONTXNIDn
            //L_EBAYITEMORDERIDn

            //SHIPTOSTREET
            if (shipToAddress.Length > 0)
            {
                requestBody.Append("&SHIPTOSTREET=" + HttpUtility.UrlEncode(shipToAddress));

                //SHIPTONAME
                if (shipToLastName.Length > 0)
                {
                    requestBody.Append("&SHIPTONAME=" + HttpUtility.UrlEncode(shipToFirstName + " " + shipToLastName));
                }

                //SHIPTOSTREET2

                //SHIPTOCITY
                if (shipToCity.Length > 0)
                {
                    requestBody.Append("&SHIPTOCITY=" + HttpUtility.UrlEncode(shipToCity));
                }

                //SHIPTOSTATE
                if (shipToState.Length > 0)
                {
                    requestBody.Append("&SHIPTOSTATE=" + HttpUtility.UrlEncode(shipToState));
                }

                //SHIPTOCOUNTRYCODE
                if (shipToCountry.Length > 0)
                {
                    requestBody.Append("&SHIPTOCOUNTRYCODE=" + HttpUtility.UrlEncode(shipToCountry));
                }

                //SHIPTOZIP
                if (shipToPostalCode.Length > 0)
                {
                    requestBody.Append("&SHIPTOZIP=" + HttpUtility.UrlEncode(shipToPostalCode));
                }

                //SHIPTOPHONENUM
                if (shipToPhone.Length > 0)
                {
                    requestBody.Append("&SHIPTOPHONENUM=" + HttpUtility.UrlEncode(shipToPhone));
                }

            }

            String url;
            if (useTestMode)
            {
                url = testUrl;
            }
            else
            {
                url = productionUrl;
            }

            StreamWriter requestStream = null;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "POST";

            webRequest.Timeout = timeoutInMilliseconds;

            webRequest.ContentType = "application/x-www-form-urlencoded";

            string encodedBody = requestBody.ToString();
            log.Debug(encodedBody);

            webRequest.ContentLength = encodedBody.Length;

            requestStream = new StreamWriter(webRequest.GetRequestStream());

            if (requestStream != null)
            {

                requestStream.Write(encodedBody);

            }

            if (requestStream != null)
                requestStream.Close();

            HttpWebResponse webResponse
                = (HttpWebResponse)webRequest.GetResponse();

            if (webResponse != null)
            {
                using (StreamReader responseStream =
                   new StreamReader(webResponse.GetResponseStream()))
                {
                    rawResponse = responseStream.ReadToEnd();
                    result = true;
                }

                ParseDoExpressCheckoutPaymentResponse();

            }
            else
            {
                response = PaymentGatewayResponse.Error;
                return false;
            }

            return result;

        }


        #endregion


        #region Private Methods

        private void ParseSetExpressCheckoutResponse()
        {
            if (rawResponse.Length > 0)
            {
                char[] pairSeparator = { '&' };
                char[] keyValSeparator = { '=' };
                string[] keyValPairs = rawResponse.Split(pairSeparator, StringSplitOptions.None);

                StringDictionary responseResults = new StringDictionary();

                foreach (string keyVal in keyValPairs)
                {
                    string[] pair = keyVal.Split(keyValSeparator, StringSplitOptions.None);
                    if (pair.Length >= 2)
                    {
                        responseResults.Add(pair[0], pair[1]);
                    }
                }

                if (responseResults.ContainsKey("ACK"))
                {
                    switch (responseResults["ACK"])
                    {
                        case "Success":
                        case "SuccessWithWarning":
                            response = PaymentGatewayResponse.Approved;
                            break;

                        case "2":
                            response = PaymentGatewayResponse.Declined;
                            break;

                        case "Warning":
                        case "Failure":
                        case "FailureWithWarning":
                        case "Error":
                            
                            response = PaymentGatewayResponse.Error;

                            if (responseResults.ContainsKey("L_LONGMESSAGE0"))
                            {
                                responseReason = HttpUtility.UrlDecode(responseResults["L_LONGMESSAGE0"]);
                            }

                            if (responseResults.ContainsKey("L_ERRORCODE0"))
                            {
                                reasonCode = HttpUtility.UrlDecode(responseResults["L_ERRORCODE0"]);

                            }

                            break;
                    }

                }

                //TOKEN A timestamped token by which you identify to PayPal that you are processing this
                //payment with Express Checkout.
                //NOTE: The token expires after three hours.
                //If you set the token in the SetExpressCheckout request, the value of the token in the
                //response is identical to the value in the request.
                //Character length and limitations: 20 single-byte characters
                if (responseResults.ContainsKey("TOKEN"))
                {
                    payPalToken = HttpUtility.UrlDecode(responseResults["TOKEN"]);

                    if (useTestMode)
                    {
                        payPalExpressUrl = payPalExpressSandboxCheckoutUrl + HttpUtility.UrlEncode(payPalToken);
                    }
                    else
                    {
                        payPalExpressUrl = payPalExpressProductionCheckoutUrl + HttpUtility.UrlEncode(payPalToken);
                    }

                }

            }

        }

        private void ParseGetExpressCheckoutDetailsResponse()
        {
            if (rawResponse.Length > 0)
            {
                char[] pairSeparator = { '&' };
                char[] keyValSeparator = { '=' };
                string[] keyValPairs = rawResponse.Split(pairSeparator, StringSplitOptions.None);

                StringDictionary responseResults = new StringDictionary();

                foreach (string keyVal in keyValPairs)
                {
                    string[] pair = keyVal.Split(keyValSeparator, StringSplitOptions.None);
                    if (pair.Length >= 2)
                    {
                        responseResults.Add(pair[0], pair[1]);
                    }

                }

                if (responseResults.ContainsKey("ACK"))
                {
                    switch (responseResults["ACK"])
                    {
                        case "Success":
                        case "SuccessWithWarning":

                            response = PaymentGatewayResponse.Approved;
                            break;

                        case "2":
                            response = PaymentGatewayResponse.Declined;
                            break;

                        case "Warning":
                        case "Failure":
                        case "FailureWithWarning":
                        case "Error":
                            
                            response = PaymentGatewayResponse.Error;

                            if (responseResults.ContainsKey("L_LONGMESSAGE0"))
                            {
                                responseReason = HttpUtility.UrlDecode(responseResults["L_LONGMESSAGE0"]);
                            }

                            if (responseResults.ContainsKey("L_ERRORCODE0"))
                            {
                                reasonCode = HttpUtility.UrlDecode(responseResults["L_ERRORCODE0"]);

                            }

                            break;
                    }

                }

                //TOKEN The timestamped token value that was returned by SetExpressCheckout response
                //and passed on GetExpressCheckoutDetails request
                if (responseResults.ContainsKey("TOKEN"))
                {
                    payPalToken = HttpUtility.UrlDecode(responseResults["TOKEN"]);

                }

                //EMAIL Email address of payer
                if (responseResults.ContainsKey("EMAIL"))
                {
                    buyerEmail = HttpUtility.UrlDecode(responseResults["EMAIL"]);

                }

                //PAYERID Unique PayPal customer account identification number
                if (responseResults.ContainsKey("PAYERID"))
                {
                    payPalPayerId = HttpUtility.UrlDecode(responseResults["PAYERID"]);

                }

                //PAYERSTATUS Status of payer. verified or unverified
                if (responseResults.ContainsKey("PAYERSTATUS"))
                {
                    payPalPayerStatus = HttpUtility.UrlDecode(responseResults["PAYERSTATUS"]);

                }

                //SALUTATION Payer’s salutation
                if (responseResults.ContainsKey("SALUTATION"))
                {
                    shipToSalutation = HttpUtility.UrlDecode(responseResults["SALUTATION"]);

                }

                //FIRSTNAME Payer’s first name
                if (responseResults.ContainsKey("FIRSTNAME"))
                {
                    shipToFirstName = HttpUtility.UrlDecode(responseResults["FIRSTNAME"]);

                }

                //MIDDLENAME Payer’s middle name
                //LASTNAME Payer’s last name
                if (responseResults.ContainsKey("LASTNAME"))
                {
                    shipToLastName = HttpUtility.UrlDecode(responseResults["LASTNAME"]);

                }
                //SUFFIX Payer’s suffix
                if (responseResults.ContainsKey("SUFFIX"))
                {
                    shipToNameSuffix = HttpUtility.UrlDecode(responseResults["SUFFIX"]);

                }

                //COUNTRYCODE Payer’s country of residence in the form of ISO standard 3166 two-character country
                //codes.
                
                //SHIPTONAME Person’s name associated with this address
                //if (responseResults.ContainsKey("SHIPTONAME"))
                //{
                //    shipToLastName = HttpUtility.UrlDecode(responseResults["SHIPTONAME"]);

                //}

                //SHIPTOSTREET First street address
                if (responseResults.ContainsKey("SHIPTOSTREET"))
                {
                    shipToAddress = HttpUtility.UrlDecode(responseResults["SHIPTOSTREET"]);

                }

                //SHIPTOSTREET2 Second street address
                if (responseResults.ContainsKey("SHIPTOSTREET2"))
                {
                    shipToAddress2 = HttpUtility.UrlDecode(responseResults["SHIPTOSTREET2"]);

                }

                //SHIPTOCITY Name of city
                if (responseResults.ContainsKey("SHIPTOCITY"))
                {
                    shipToCity = HttpUtility.UrlDecode(responseResults["SHIPTOCITY"]);

                }

                //SHIPTOSTATE State or province
                if (responseResults.ContainsKey("SHIPTOSTATE"))
                {
                    shipToState = HttpUtility.UrlDecode(responseResults["SHIPTOSTATE"]);

                }

                //SHIPTOCOUNTRYCODE Country code
                if (responseResults.ContainsKey("SHIPTOCOUNTRYCODE"))
                {
                    shipToCountry = HttpUtility.UrlDecode(responseResults["SHIPTOCOUNTRYCODE"]);

                }

                //SHIPTOZIP U.S. Zip code or other country-specific postal code.
                if (responseResults.ContainsKey("SHIPTOZIP"))
                {
                    shipToPostalCode = HttpUtility.UrlDecode(responseResults["SHIPTOZIP"]);

                }

                //ADDRESSSTATUS Status of street address on file with PayPal
                if (responseResults.ContainsKey("ADDRESSSTATUS"))
                {
                    shipToAddressStatus = HttpUtility.UrlDecode(responseResults["ADDRESSSTATUS"]);

                }

                //CUSTOM A free-form field for your own use, as set by you in the Custom element of
                //SetExpressCheckout request
                if (responseResults.ContainsKey("CUSTOM"))
                {
                    this.merchantCartId = HttpUtility.UrlDecode(responseResults["CUSTOM"]);

                }

                //INVNUM Your own invoice or tracking number, as set by you in the element of the same name in
                if (responseResults.ContainsKey("INVNUM"))
                {
                    this.merchantInvoiceNumber = HttpUtility.UrlDecode(responseResults["INVNUM"]);

                }

                //SetExpressCheckout request .
                //PHONENUM Payer’s contact telephone number
                if (responseResults.ContainsKey("PHONENUM"))
                {
                    this.shipToPhone = HttpUtility.UrlDecode(responseResults["PHONENUM"]);

                }

                //REDIRECTREQUIRED Flag to indicate whether you need to redirect the customer to back to PayPal after
                //completing the transaction.
                //NOTE: Use this field only if you are using giropay or bank transfer payment methods in
                //Germany.

            }


        }

        private void ParseDoExpressCheckoutPaymentResponse()
        {
            if (rawResponse.Length > 0)
            {
                char[] pairSeparator = { '&' };
                char[] keyValSeparator = { '=' };
                string[] keyValPairs = rawResponse.Split(pairSeparator, StringSplitOptions.None);

                StringDictionary responseResults = new StringDictionary();

                foreach (string keyVal in keyValPairs)
                {
                    string[] pair = keyVal.Split(keyValSeparator, StringSplitOptions.None);
                    if (pair.Length >= 2)
                    {
                        responseResults.Add(pair[0], pair[1]);
                    }
                }

                if (responseResults.ContainsKey("ACK"))
                {
                    switch (responseResults["ACK"])
                    {
                        case "Success":
                        case "SuccessWithWarning":

                            response = PaymentGatewayResponse.Approved;
                            break;

                        case "2":
                            response = PaymentGatewayResponse.Declined;
                            break;

                        case "Warning":
                        case "Failure":
                        case "FailureWithWarning":
                        case "Error":
                            
                            response = PaymentGatewayResponse.Error;

                            if (responseResults.ContainsKey("L_LONGMESSAGE0"))
                            {
                                responseReason = HttpUtility.UrlDecode(responseResults["L_LONGMESSAGE0"]);
                            }

                            if (responseResults.ContainsKey("L_ERRORCODE0"))
                            {
                                reasonCode = HttpUtility.UrlDecode(responseResults["L_ERRORCODE0"]);

                            }

                            break;
                    }

                }

                //TOKEN The timestamped token value that was returned by SetExpressCheckout response and
                // passed on GetExpressCheckoutDetails request.
                if (responseResults.ContainsKey("TOKEN"))
                {
                    payPalToken = HttpUtility.UrlDecode(responseResults["TOKEN"]);

                }

                //TRANSACTIONID Unique transaction ID of the payment
                if (responseResults.ContainsKey("TRANSACTIONID"))
                {
                    transactionID = HttpUtility.UrlDecode(responseResults["TRANSACTIONID"]);

                }

                //TRANSACTIONTYPE The type of transaction
                //The type of transaction
                //Character length and limitations:15 single-byte characters
                //Possible values:
                // cart
                // express-checkout
                if (responseResults.ContainsKey("TRANSACTIONTYPE"))
                {
                    payPalTransactionType = HttpUtility.UrlDecode(responseResults["TRANSACTIONTYPE"]);

                }

                //PAYMENTTYPE Indicates whether the payment is instant or delayed
                // none echeck instant
                if (responseResults.ContainsKey("PAYMENTTYPE"))
                {
                    payPalPaymentType = HttpUtility.UrlDecode(responseResults["PAYMENTTYPE"]);

                }

                //ORDERTIME Time/date stamp of payment
                if (responseResults.ContainsKey("ORDERTIME"))
                {
                    DateTime orderTime;
                    if(DateTime.TryParse(HttpUtility.UrlDecode(responseResults["ORDERTIME"]), out orderTime))
                    {
                        payPalOrderTimeStamp = orderTime;
                    }

                }

                //CURRENCYCODE A three-character currency code for one of the currencies
                if (responseResults.ContainsKey("CURRENCYCODE"))
                {
                    currencyCode = HttpUtility.UrlDecode(responseResults["CURRENCYCODE"]);

                }

                CultureInfo currencyCulture = ResourceHelper.GetCurrencyCulture(currencyCode);

                
                //AMT The final amount charged, including any shipping and taxes from your Merchant Profile
                if (responseResults.ContainsKey("AMT"))
                {
                    this.chargeTotal = decimal.Parse(HttpUtility.UrlDecode(responseResults["AMT"]), currencyCulture);

                }

                

                //FEEAMT PayPal fee amount charged for the transaction
                if (responseResults.ContainsKey("FEEAMT"))
                {
                    this.payPalFeeAmount = decimal.Parse(HttpUtility.UrlDecode(responseResults["FEEAMT"]), currencyCulture);

                }

                //SETTLEAMT Amount deposited in your PayPal account after a currency conversion
                if (responseResults.ContainsKey("SETTLEAMT"))
                {
                    this.payPalSettlementAmount = decimal.Parse(HttpUtility.UrlDecode(responseResults["SETTLEAMT"]), currencyCulture);

                }

                //TAXAMT Tax charged on the transaction
                if (responseResults.ContainsKey("TAXAMT"))
                {
                    this.payPalTaxTotal = decimal.Parse(HttpUtility.UrlDecode(responseResults["TAXAMT"]), currencyCulture);

                }

                //EXCHANGERATE Exchange rate if a currency conversion occurred
                if (responseResults.ContainsKey("EXCHANGERATE"))
                {
                    payPalExchangeRate = HttpUtility.UrlDecode(responseResults["EXCHANGERATE"]);

                }

                //PAYMENTSTATUS Status of the payment
                // Completed, Pending
                if (responseResults.ContainsKey("PAYMENTSTATUS"))
                {
                    payPalPaymentStatus = HttpUtility.UrlDecode(responseResults["PAYMENTSTATUS"]);

                    if (payPalPaymentStatus == "Completed")
                        response = PaymentGatewayResponse.Approved;

                    if (payPalPaymentStatus == "Pending")
                        response = PaymentGatewayResponse.Pending;

                }

                //PENDINGREASON The reason the payment is pending
                if (responseResults.ContainsKey("PENDINGREASON"))
                {
                    payPalPendingReason = HttpUtility.UrlDecode(responseResults["PENDINGREASON"]);

                }

                //none: No pending reason

                //address: The payment is pending because your customer did not include a confirmed
                //shipping address and your Payment Receiving Preferences is set such that you want to
                //manually accept or deny each of these payments. To change your preference, go to the
                //Preferences section of your Profile.

                //echeck: The payment is pending because it was made by an eCheck that has not yet
                //cleared.

                //intl: The payment is pending because you hold a non-U.S. account and do not have a
                //withdrawal mechanism. You must manually accept or deny this payment from your
                //Account Overview.

                //REASONCODE The reason for a reversal if TransactionType is reversal
                if (responseResults.ContainsKey("REASONCODE"))
                {
                    reasonCode = HttpUtility.UrlDecode(responseResults["REASONCODE"]);

                }
                // none: No reason code
                //chargeback: A reversal has occurred on this transaction due to a chargeback by your customer.
                //guarantee: A reversal has occurred on this transaction due to your customer
                //triggering a money-back guarantee
                //buyer-complaint: A reversal has occurred on this transaction due to a complaint
                //about the transaction from your customer.
                //refund: A reversal has occurred on this transaction because you have given the
                //customer a refund.
                //other: A reversal has occurred on this transaction due to a reason not listed above

                //REDIRECTREQUIRED Flag to indicate whether you need to redirect the customer to back to PayPal after
                //completing the transaction.
                //NOTE: Use this field only if you are using giropay or bank transfer payment methods in
                //Germany.

            }


        }


        private String FormatCharge()
        {
            return chargeTotal.ToString("#####.##");
        }

        

        #endregion



    }
}
