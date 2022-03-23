using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using log4net;

namespace mojoPortal.Business.WebHelpers.PaymentGateway
{
   
    // PayPal Direct Payment uses a single api call. Users enter their billing and shipping
    // and PayPal charges the card.
    // 
    // Required fields:
    // USER=apiUsername&PWD=apiPassword&SIGNATURE=apiSignature
    // 
    // &SUBJECT=optionalThirdPartyEmailAddress&VERSION=3.2
    // 
    // &METHOD=methodName&otherRequiredAndOptionalParameters
    // 
    // Response Types:
    // Success, SuccessWithWarning, Error, Warning
    // 
    // Response Success Fields:
    // ACK=Success&TIMESTAMP=date/timeOfResponse
    // &CORRELATIONID=debuggingToken&VERSION=3.200000
    // &BUILD=buildNumber
    // 
    // Response Error Fields
    // ACK=Error&TIMESTAMP=date/timeOfResponse&
    // CORRELATIONID=debuggingToken&VERSION=3.200000&
    // BUILD=buildNumber&L_ERRORCODE0=errorCode&
    // L_SHORTMESSAGE0=shortMessage&
    // L_LONGMESSAGE0=longMessage&
    // L_SEVERITYCODE0=severityCode
    // 
    // API Responses fields
    // &NAME1=value1&NAME2=value2&NAME3=value3&...
    // 
    // 
    // Use DoDirectPayment to charge a credit card or to authorize a credit card for later capture. 
    // Always include the following parameters with DoDirectPayment:
    // PAYMENTACTION       
    //     - To authorize a credit card for later capture, include the PAYMENTACTION=Authorization
    //     - To charge a credit card for a final sale, include the PAYMENTACTION=Sale
    //     - To capture the payment, use DoCapture
    // CREDITCARDTYPE
    // ACCT
    // EXPDATE
    // CVV2
    // IPADDRESS
    // FIRSTNAME
    // LASTNAME
    //
    // On success, the DoDirectPayment response returns the Address Verification System (AVS) code, a PayPal 
    // transaction ID, and the amount charged.
    // 
   
    public class PayPalDirectPaymentGateway : IPaymentGateway
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PayPalDirectPaymentGateway));

        public PayPalDirectPaymentGateway(
            string apiLogin,
            string apiPassword,
            string apiTransactionKey)
        {
            if (apiLogin != null) merchantAPILogin = apiLogin;
            if (apiPassword != null) merchantAPIPassword = apiPassword;
            if (apiTransactionKey != null) merchantAPITransactionKey = apiTransactionKey;

        }

        #region Private Properties

        private string apiVersion = "3.2";
        private string merchantAPIPassword = string.Empty;

        private string provider = "PayPalDirect";
        private PaymentGatewayTransactionType transactionType = PaymentGatewayTransactionType.AuthCapture;
        private PaymentGatewayResponse response = PaymentGatewayResponse.NoRequestInitiated;

        private string testUrl = "https://api-3t.sandbox.paypal.com/nvp";
        private string productionUrl = "https://api-3t.paypal.com/nvp";

        private string merchantAPILogin = string.Empty;
        private string merchantAPITransactionKey = string.Empty;

        private string cardType = string.Empty;
        private string cardNumber = string.Empty;
        private string cardExpiration = string.Empty;
        private string cardSecurityCode = string.Empty;
        private string authenticationIndicator = string.Empty;
        private string cardholderAuthenticationValue = string.Empty;

        private string cardOwnerFirstName = string.Empty;
        private string cardOwnerLastName = string.Empty;
        private string cardOwnerCompanyName = string.Empty;
        private string cardBillingAddress = string.Empty;
        private string cardBillingCity = string.Empty;
        private string cardBillingState = string.Empty;
        private string cardBillingPostalCode = string.Empty;
        private string cardBillingCountry = string.Empty;
        private string cardBillingCountryCode = string.Empty;
        private string cardBillingPhone = string.Empty;
        private string cardBillingEmail = string.Empty;
        private string shipToFirstName = string.Empty;
        private string shipToLastName = string.Empty;
        private string shipToCompanyName = string.Empty;
        private string shipToAddress = string.Empty;
        private string shipToCity = string.Empty;
        private string shipToState = string.Empty;
        private string shipToPostalCode = string.Empty;
        private string shipToCountry = string.Empty;
        private string customerIPAddress = string.Empty;
        private string customerTaxID = string.Empty;
        private string customerID = string.Empty;
        private bool sendCustomerEmailConfirmation = false;

        private string merchantEmail = string.Empty;
        private string merchantInvoiceNumber = string.Empty;
        private string merchantTransactionDescription = string.Empty;
        private string merchantEmailConfirmationHeader = string.Empty;
        private string merchantEmailConfirmationFooter = string.Empty;

        private string previousTransactionID = string.Empty;
        private string transactionID = string.Empty;
        private string previousApprovalCode = string.Empty;
        private string approvalCode = string.Empty;
        private string responseCode = string.Empty;
        private string reasonCode = string.Empty;
        private string responseReason = string.Empty;
        private string avsResultCode = string.Empty;
        private string avsResultText = string.Empty;
        private string cardSecurityCodeResponseCode = string.Empty;
        private string cardholderAuthenticationValueResponseCode = string.Empty;

        private string currencyCode = "USD";
        private decimal chargeTotal = 0;

        private bool useTestMode = false;
        private string rawResponse = string.Empty;
        private int timeoutInMilliseconds = 120000; // 120 seconds
        private Exception lastExecutionException = null;

        #endregion


        #region IPaymentGateway

        #region Public Properties

        private CultureInfo currencyCulture = CultureInfo.CurrentCulture;

        public CultureInfo CurrencyCulture
        {
            get { return currencyCulture; }
            set { currencyCulture = value; }
        }

        public string TestUrl
        {
            get { return testUrl; }
        }

        public string ProductionUrl
        {
            get { return productionUrl; }
        }

        public string Provider
        {
            get { return provider; }
        }

        public string CardType
        {
            get { return cardType; }
            set { cardType = value; }
        }

        public string CardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; }
        }

        public string CardExpiration
        {
            get { return cardExpiration; }
            set { cardExpiration = value; }
        }

        public string CardSecurityCode
        {
            get { return cardSecurityCode; }
            set { cardSecurityCode = value; }
        }

        public string AuthenticationIndicator
        {
            get { return authenticationIndicator; }
            set { authenticationIndicator = value; }
        }

        public string CardholderAuthenticationValue
        {
            get { return cardholderAuthenticationValue; }
            set { cardholderAuthenticationValue = value; }
        }

        public string CardOwnerFirstName
        {
            get { return cardOwnerFirstName; }
            set { cardOwnerFirstName = value; }
        }

        public string CardOwnerLastName
        {
            get { return cardOwnerLastName; }
            set { cardOwnerLastName = value; }
        }

        public string CardOwnerCompanyName
        {
            get { return cardOwnerCompanyName; }
            set { cardOwnerCompanyName = value; }
        }

        public string CardBillingAddress
        {
            get { return cardBillingAddress; }
            set { cardBillingAddress = value; }
        }

        public string CardBillingCity
        {
            get { return cardBillingCity; }
            set { cardBillingCity = value; }
        }

        public string CardBillingState
        {
            get { return cardBillingState; }
            set { cardBillingState = value; }
        }

        public string CardBillingPostalCode
        {
            get { return cardBillingPostalCode; }
            set { cardBillingPostalCode = value; }
        }

        public string CardBillingCountry
        {
            get { return cardBillingCountry; }
            set { cardBillingCountry = value; }
        }

        public string CardBillingCountryCode
        {
            get { return cardBillingCountryCode; }
            set { cardBillingCountryCode = value; }
        }

        public string CardBillingPhone
        {
            get { return cardBillingPhone; }
            set { cardBillingPhone = value; }
        }

        public string CardBillingEmail
        {
            get { return cardBillingEmail; }
            set { cardBillingEmail = value; }
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

        public string CustomerIPAddress
        {
            get { return customerIPAddress; }
            set { customerIPAddress = value; }
        }

        public string CustomerTaxId
        {
            get { return customerTaxID; }
            set { customerTaxID = value; }
        }

        public string CustomerId
        {
            get { return customerID; }
            set { customerID = value; }
        }

        public bool SendCustomerEmailConfirmation
        {
            get { return sendCustomerEmailConfirmation; }
            set { sendCustomerEmailConfirmation = value; }
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

        public PaymentGatewayTransactionType TransactionType
        {
            get { return transactionType; }
            set { transactionType = value; }
        }

        public string PreviousTransactionId
        {
            get { return previousTransactionID; }
            set { previousTransactionID = value; }
        }

        public string PreviousApprovalCode
        {
            get { return previousApprovalCode; }
            set { previousApprovalCode = value; }
        }

        public string TransactionId
        {
            get { return transactionID; }
        }

        public string ApprovalCode
        {
            get { return approvalCode; }
        }

        public string ResponseCode
        {
            get { return responseCode; }
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

        public string AvsResultCode
        {
            get { return avsResultCode; }
        }

        public string AvsResultText
        {
            get { return avsResultText; }
        }

        public string CardSecurityCodeResponseCode
        {
            get { return cardSecurityCodeResponseCode; }
        }

        public string CardholderAuthenticationValueResponseCode
        {
            get { return cardholderAuthenticationValueResponseCode; }
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

        public PaymentGatewayType Type { get; set; } = PaymentGatewayType.Direct;

        #endregion



        public bool ExecuteTransaction()
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

            requestBody.Append("&BUTTONSOURCE=SourceTreeSolutions_SP");
            

            switch (transactionType)
            {
                // default transaction type 
                case PaymentGatewayTransactionType.AuthCapture:
                    requestBody.Append("&METHOD=DoDirectPayment");
                    requestBody.Append("&PAYMENTACTION=Sale");
                    requestBody.Append("&AMT=" + HttpUtility.UrlEncode(FormatCharge()));
                    break;

                case PaymentGatewayTransactionType.AuthOnly:
                    requestBody.Append("&METHOD=DoDirectPayment");
                    requestBody.Append("&PAYMENTACTION=Authorization");
                    requestBody.Append("&AMT=" + HttpUtility.UrlEncode(FormatCharge()));
                    break;

                case PaymentGatewayTransactionType.CaptureOnly:

                    throw new NotSupportedException();

                case PaymentGatewayTransactionType.Credit:
                    if (previousTransactionID.Length == 0)
                        throw new NotSupportedException("Credit not supported without a previous transactionID");

                    requestBody.Append("&METHOD=RefundTransaction, ");
                    requestBody.Append("&TRANSACTIONID=" + HttpUtility.UrlEncode(previousTransactionID));
                    requestBody.Append("&AMT=" + HttpUtility.UrlEncode(FormatCharge()));
                    requestBody.Append("&REFUNDTYPE=Full");

                    break;

                case PaymentGatewayTransactionType.PriorAuthCapture:

                    if (previousTransactionID.Length == 0)
                        throw new NotSupportedException("PriorAuthCapture not supported without a previous transactionID");

                    requestBody.Append("&METHOD=DoCapture");
                    requestBody.Append("&TRANSACTIONID=" + HttpUtility.UrlEncode(previousTransactionID));
                    requestBody.Append("&AMT=" + HttpUtility.UrlEncode(FormatCharge()));
                    requestBody.Append("&COMPLETETYPE=Complete");

                    break;

                case PaymentGatewayTransactionType.Void:

                    if (previousTransactionID.Length == 0)
                        throw new NotSupportedException("Void not supported without a previous transactionID");

                    requestBody.Append("&METHOD=DoVoid");
                    requestBody.Append("&TRANSACTIONID=" + HttpUtility.UrlEncode(previousTransactionID));
                    requestBody.Append("&AMT=" + HttpUtility.UrlEncode(FormatCharge()));
                    requestBody.Append("&COMPLETETYPE=Complete");

                    break;

            }

            requestBody.Append("&CREDITCARDTYPE=" + HttpUtility.UrlEncode(cardType));
            requestBody.Append("&ACCT=" + HttpUtility.UrlEncode(cardNumber));
            requestBody.Append("&EXPDATE=" + HttpUtility.UrlEncode(cardExpiration));

            if (cardSecurityCode.Length > 0)
            {
                requestBody.Append("&CVV2=" + HttpUtility.UrlEncode(cardSecurityCode));
            }

            requestBody.Append("&FIRSTNAME=" + HttpUtility.UrlEncode(cardOwnerFirstName));
            requestBody.Append("&LASTNAME=" + HttpUtility.UrlEncode(cardOwnerLastName));
            requestBody.Append("&STREET=" + HttpUtility.UrlEncode(cardBillingAddress));
            requestBody.Append("&CITY=" + HttpUtility.UrlEncode(cardBillingCity));
            requestBody.Append("&STATE=" + HttpUtility.UrlEncode(cardBillingState));
            requestBody.Append("&ZIP=" + HttpUtility.UrlEncode(cardBillingPostalCode));
            requestBody.Append("&COUNTRYCODE=" + HttpUtility.UrlEncode(cardBillingCountryCode));
            requestBody.Append("&PHONENUM=" + HttpUtility.UrlEncode(cardBillingPhone));
            requestBody.Append("&IPADDRESS=" + HttpUtility.UrlEncode(customerIPAddress));

            if (merchantInvoiceNumber.Length > 0)
            {
                requestBody.Append("&INVNUM=" + HttpUtility.UrlEncode(merchantInvoiceNumber));
            }

            if (currencyCode.Length > 0)
            {
                requestBody.Append("&CURRENCYCODE=" + HttpUtility.UrlEncode(currencyCode));
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

                ParseResponse();
            }
            else
            {
                response = PaymentGatewayResponse.Error;
                return false;
            }

            return result;

        }

        public void LogTransaction(Guid siteGuid, Guid moduleGuid, Guid storeGuid, Guid cartGuid, Guid userGuid, string providerName, string method, string serializedCart)
        {
            //PayPalLog payPalLog = new PayPalLog();
            //payPalLog.ProviderName = providerName;
            //payPalLog.RawResponse = RawResponse;
            //payPalLog.RequestType = "DirectPayment";
            //payPalLog.CartGuid = cartGuid;
            //payPalLog.StoreGuid = storeGuid;
            //payPalLog.UserGuid = userGuid;
            //payPalLog.SiteGuid = siteGuid;
            //payPalLog.PendingReason = ResponseReason;
            //payPalLog.ReasonCode = ReasonCode;
            //payPalLog.PaymentType = "CreditCard";
            //payPalLog.PaymentStatus = Response.ToString();
            //payPalLog.TransactionId = TransactionId;
            //payPalLog.CartTotal = ChargeTotal;
            //payPalLog.CurrencyCode = CurrencyCode;
            //payPalLog.SerializedObject = serializedCart;
            //payPalLog.Save();

            PaymentLog pnplog = new PaymentLog();
            pnplog.RawResponse = RawResponse;
            pnplog.Amount = ChargeTotal;
            pnplog.AuthCode = ApprovalCode;
            pnplog.AvsCode = AvsResultCode;
            pnplog.CartGuid = cartGuid;
            pnplog.CcvCode = CardSecurityCodeResponseCode;
            pnplog.Reason = ResponseReason;
            pnplog.ResponseCode = ResponseCode;
            pnplog.SiteGuid = siteGuid;
            pnplog.StoreGuid = storeGuid;
            pnplog.TransactionId = TransactionId;
            pnplog.TransactionType = TransactionType.ToString();
            pnplog.UserGuid = userGuid;
            pnplog.Method = method;
            pnplog.Save();
        }


        #endregion


        #region Private Methods

        private String FormatCharge()
        {
            return chargeTotal.ToString("#####.##");
        }

        private void ParseResponse()
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
                           
                            if (responseResults.ContainsKey("L_LONGMESSAGE0"))
                            {
                                responseReason = HttpUtility.UrlDecode(responseResults["L_LONGMESSAGE0"]);
                            }

                            if (responseResults.ContainsKey("L_ERRORCODE0"))
                            {
                                reasonCode = HttpUtility.UrlDecode(responseResults["L_ERRORCODE0"]);

                            }

                            string shortMessage = string.Empty;

                            if (responseResults.ContainsKey("L_SHORTMESSAGE0"))
                            {
                                shortMessage = HttpUtility.UrlDecode(responseResults["L_SHORTMESSAGE0"]);
                            }

                            switch (shortMessage)
                            {
                                case "Gateway Decline":
                                    response = PaymentGatewayResponse.Declined;
                                    break;

                                default:
                                    response = PaymentGatewayResponse.Error;
                                    break;

                            }

                            break;

                    }

                }

                if (responseResults.ContainsKey("TIMESTAMP"))
                {


                }

                if (responseResults.ContainsKey("AVSCODE"))
                {
                    avsResultCode = HttpUtility.UrlDecode(responseResults["AVSCODE"]);

                }

                if (responseResults.ContainsKey("TRANSACTIONID"))
                {
                    transactionID = HttpUtility.UrlDecode(responseResults["TRANSACTIONID"]);

                }

                if (responseResults.ContainsKey("AUTHORIZATIONID"))
                {
                    approvalCode = HttpUtility.UrlDecode(responseResults["AUTHORIZATIONID"]);

                }

                //if (responseResults.ContainsKey("CVV2MATCH"))
                //{


                //}

                //if (responseResults.ContainsKey("AMT"))
                //{


                //}

                //



            }


        }

        #endregion



    }
}
