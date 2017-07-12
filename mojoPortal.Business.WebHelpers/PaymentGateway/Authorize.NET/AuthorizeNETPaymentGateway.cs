// Author:					
// Created:				    2007-02-12
// Last Modified:		    2013-10-07
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
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using log4net;

namespace mojoPortal.Business.WebHelpers.PaymentGateway
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorizeNETPaymentGateway : IPaymentGateway
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AuthorizeNETPaymentGateway));

        #region Constructors

        static AuthorizeNETPaymentGateway()
        {
            // Create static AVS Text Lookup
            AVSResultTextLookup = new Hashtable();

            // AVS indications from AIM_guide.pdf
            AVSResultTextLookup.Add("A", "Address (Street) matches, ZIP does not");
            AVSResultTextLookup.Add("B", "Address information not provided for AVS check");
            AVSResultTextLookup.Add("E", "AVS error");
            AVSResultTextLookup.Add("G", "Non-U.S. Card Issuing Bank");
            AVSResultTextLookup.Add("N", "No Match on Address (Street) or ZIP");
            AVSResultTextLookup.Add("P", "AVS not applicable for this transaction");
            AVSResultTextLookup.Add("R", "Retry - System unavailable or timed out");
            AVSResultTextLookup.Add("S", "Service not supported by issuer");
            AVSResultTextLookup.Add("U", "Address information is unavailable");
            AVSResultTextLookup.Add("W", "9 digit ZIP matches, Address (Street) does not");
            AVSResultTextLookup.Add("X", "Address (Street) and 9 digit ZIP match");
            AVSResultTextLookup.Add("Y", "Address (Street) and 5 digit ZIP match");
            AVSResultTextLookup.Add("Z", "5 digit ZIP matches, Address (Street) does not");
        }

        //public AuthorizeNETPaymentGateway()
        //{ }

        public AuthorizeNETPaymentGateway(string login, string transactionKey)
        {
            if (login != null) merchantAPILogin = login;
            if (transactionKey != null) merchantAPITransactionKey = transactionKey;

        }

        #endregion

        #region ARB Automated Recurring Billing Properties

        //https://api.authorize.net/xml/v1/schema/AnetApiSchema.xsd

        //private string arbProductionUrl = "https://api.authorize.net/xml/v1/request.api";

        //private string arbTestUrl = "https://apitest.authorize.net/xml/v1/request.api";

        private bool isARB = false;

        public bool IsARB
        {
            get { return isARB; }
            set { isARB = value; }
        }

        #endregion

        #region Private Properties

        private static Hashtable AVSResultTextLookup;

        private string provider = "Authorize.NET";
        private PaymentGatewayTransactionType transactionType = PaymentGatewayTransactionType.AuthCapture;
        private PaymentGatewayResponse response = PaymentGatewayResponse.NoRequestInitiated;

        private string testUrl = "https://test.authorize.net/gateway/transact.dll";

        // 2015-08-18 updated to new url per email notification from Authorize.NET
        // http://www.authorize.net/support/akamaifaqs/
        //private string productionUrl = "https://secure.authorize.net/gateway/transact.dll";
        private string productionUrl = "https://secure2.authorize.net/gateway/transact.dll";

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
        private string shipToFirstName = string.Empty;
        private string shipToLastName = string.Empty;
        private string shipToCompanyName = string.Empty;
        private string shipToAddress = string.Empty;
        private string shipToCity = string.Empty;
        private string shipToState = string.Empty;
        private string shipToPostalCode = string.Empty;
        private string shipToCountry = string.Empty;
        private string cardBillingPhone = string.Empty;
        private string cardBillingEmail = string.Empty;
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

        //public string CardholderAuthenticationIndicator
        //{
        //    get { return cardholderAuthenticationIndicator; }
        //    set { cardholderAuthenticationIndicator = value; }
        //}

        //public string CardholderAuthenticationValueResponseCode
        //{
        //    get { return cardholderAuthenticationValueResponseCode; }
        //}



        private string currencyCode = "";
        private decimal chargeTotal = 0;

        private bool useTestMode = false;
        //private bool useTestUrl = false;
        private string rawResponse = string.Empty;
        private int timeoutInMilliseconds = 120000; // 120 seconds
        private Exception lastExecutionException = null;



        #endregion

        #region Public Properties

        public string TestUrl
        {
            get { return testUrl; }
        }

        public string ProductionUrl
        {
            get { return productionUrl; }
        }

        #endregion

        #region IPaymentGateway

        #region IPaymentGateway Properties


        private CultureInfo currencyCulture = CultureInfo.CurrentCulture;

        public CultureInfo CurrencyCulture
        {
            get { return currencyCulture; }
            set { currencyCulture = value; }
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

        //public bool UseTestUrl
        //{
        //    get { return useTestUrl; }
        //    set { useTestUrl = value; }
        //}

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

        #region IPaymentGateway Methods


        public bool ExecuteTransaction()
        {

            bool result = false;
            StringBuilder requestBody = new StringBuilder();
            requestBody.Append("x_login=" + merchantAPILogin);
            requestBody.Append("&x_tran_key=" + merchantAPITransactionKey);
            requestBody.Append("&x_method=CC");

            switch (transactionType)
            {
                case PaymentGatewayTransactionType.AuthCapture:
                    requestBody.Append("&x_type=AUTH_CAPTURE");
                    break;

                case PaymentGatewayTransactionType.AuthOnly:
                    requestBody.Append("&x_type=AUTH_ONLY");
                    break;

                case PaymentGatewayTransactionType.CaptureOnly:
                    requestBody.Append("&x_type=CAPTURE_ONLY");
                    break;

                case PaymentGatewayTransactionType.Credit:
                    requestBody.Append("&x_type=CREDIT");
                    break;

                case PaymentGatewayTransactionType.PriorAuthCapture:
                    requestBody.Append("&x_type=PRIOR_AUTH_CAPTURE");
                    break;

                case PaymentGatewayTransactionType.Void:
                    requestBody.Append("&x_type=VOID");
                    break;

            }

            requestBody.Append("&x_amount=" + FormatCharge());
            requestBody.Append("&x_delim_data=TRUE");
            requestBody.Append("&x_delim_char=|");
            requestBody.Append("&x_relay_response=FALSE");

            requestBody.Append("&x_card_num=" + cardNumber);
            requestBody.Append("&x_exp_date=" + cardExpiration);

            if (cardSecurityCode.Length > 0)
            {
                requestBody.Append("&x_card_code=" + cardSecurityCode);
            }

            if (authenticationIndicator.Length > 0)
            {
                requestBody.Append("&x_authentication_indicator=" + authenticationIndicator);
            }

            if (cardholderAuthenticationValue.Length > 0)
            {
                requestBody.Append("&x_cardholder_authentication_value=" + cardholderAuthenticationValue);
            }

            if (previousTransactionID.Length > 0)
            {
                requestBody.Append("&x_trans_id=" + previousTransactionID);
            }

            if (previousApprovalCode.Length > 0)
            {
                requestBody.Append("&x_auth_code=" + previousApprovalCode);
            }

            requestBody.Append("&x_first_name=" + cardOwnerFirstName);
            requestBody.Append("&x_last_name=" + cardOwnerLastName);
            requestBody.Append("&x_company=" + cardOwnerCompanyName);
            requestBody.Append("&x_address=" + cardBillingAddress);
            requestBody.Append("&x_city=" + cardBillingCity);
            requestBody.Append("&x_state=" + cardBillingState);
            requestBody.Append("&x_zip=" + cardBillingPostalCode);
            requestBody.Append("&x_country=" + cardBillingCountry);
            requestBody.Append("&x_phone=" + cardBillingPhone);

            if (shipToFirstName.Length > 0)
                requestBody.Append("&x_ship_to_first_name=" + shipToFirstName);
            if (shipToLastName.Length > 0)
                requestBody.Append("&x_ship_to_last_name=" + shipToLastName);
            if (shipToCompanyName.Length > 0)
                requestBody.Append("&x_ship_to_company=" + shipToCompanyName);
            if (shipToAddress.Length > 0)
                requestBody.Append("&x_ship_to_address=" + shipToAddress);
            if (shipToCity.Length > 0)
                requestBody.Append("&x_ship_to_city=" + shipToCity);
            if (shipToState.Length > 0)
                requestBody.Append("&x_ship_to_state=" + shipToState);
            if (shipToPostalCode.Length > 0)
                requestBody.Append("&x_ship_to_zip=" + shipToPostalCode);
            if (shipToCountry.Length > 0)
                requestBody.Append("&x_ship_to_country=" + shipToCountry);

            if (customerID.Length > 0)
            {
                requestBody.Append("&x_cust_id=" + customerID);
            }

            if (customerTaxID.Length > 0)
            {
                requestBody.Append("&x_customer_tax_id=" + customerTaxID);
            }

            requestBody.Append("&x_customer_ip=" + customerIPAddress);

            if (sendCustomerEmailConfirmation)
            {
                requestBody.Append("&x_email_customer=TRUE");
                requestBody.Append("&x_email=" + cardBillingEmail);
                if (merchantEmailConfirmationHeader.Length > 0)
                {
                    requestBody.Append("&x_header_email_receipt=" + merchantEmailConfirmationHeader);
                }
                if (merchantEmailConfirmationFooter.Length > 0)
                {
                    requestBody.Append("&x_footer_email_receipt=" + merchantEmailConfirmationFooter);
                }
            }

            if (merchantEmail.Length > 0)
            {
                requestBody.Append("&x_merchant_email=" + merchantEmail);
            }

            if (merchantInvoiceNumber.Length > 0)
            {
                requestBody.Append("&x_invoice_num=" + merchantInvoiceNumber);
            }

            if (merchantTransactionDescription.Length > 0)
            {
                requestBody.Append("&x_description=" + merchantTransactionDescription);
            }

            if (currencyCode.Length > 0)
            {
                requestBody.Append("&x_currency_code=" + currencyCode);
            }

            if (useTestMode)
            {
                // authorize.net supports test requests
                // this is supported on both production and test servers
                // I'm commenting this out because I want to test on test server as if it were production
                // so I don't want to pass this
                //requestBody.Append("&x_test_request=TRUE");
            }

            requestBody.Append("&x_version=3.1");

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

            //string payload =  HttpUtility.UrlEncode(requestBody.ToString());
            //byte[] somebytes = Encoding.UTF8.GetBytes(payload);
            //webRequest.ContentLength = somebytes.Length;
            webRequest.ContentLength = requestBody.Length;


            //try
            //{
            requestStream = new StreamWriter(webRequest.GetRequestStream());
            if (requestStream != null)
                requestStream.Write(requestBody.ToString());

            //}
            //catch (Exception e)
            //{
            //    lastExecutionException = e;
            //    result = false;
            //}
            //finally
            //{
            if (requestStream != null)
                requestStream.Close();
            //}

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
                // TODO: error message?
                response = PaymentGatewayResponse.Error;
                return false;
            }



            return result;

        }

        public void LogTransaction(Guid siteGuid, Guid moduleGuid, Guid storeGuid, Guid cartGuid, Guid userGuid, string providerName, string method, string serializedCart)
        {
            PaymentLog alog = new PaymentLog();
            alog.RawResponse = RawResponse;
            alog.Amount = ChargeTotal;
            alog.AuthCode = ApprovalCode;
            alog.AvsCode = AvsResultCode;
            alog.CartGuid = cartGuid;
            alog.CcvCode = CardSecurityCodeResponseCode;
            alog.Reason = ResponseReason;
            alog.ResponseCode = ResponseCode;
            alog.SiteGuid = siteGuid;
            alog.StoreGuid = storeGuid;
            alog.TransactionId = TransactionId;
            alog.TransactionType = TransactionType.ToString();
            alog.UserGuid = userGuid;
            alog.Method = method;
            alog.Save();

        }

        #endregion

        #endregion

        #region Private Methods

        private String FormatCharge()
        {
            return chargeTotal.ToString(currencyCulture);
        }

        private void ParseResponse()
        {
            if (rawResponse.Length > 0)
            {
                char[] separator = { '|' };
                string[] responseValues = rawResponse.Split(separator, StringSplitOptions.None);

                //should be at least 39 elements
                if (responseValues.Length > 38)
                {
                    responseCode = responseValues[ResponseCodePosition];
                    switch (responseCode)
                    {
                        case "1":
                            response = PaymentGatewayResponse.Approved;
                            break;

                        case "2":
                            response = PaymentGatewayResponse.Declined;
                            break;

                        case "3":
                            response = PaymentGatewayResponse.Error;
                            break;

                    }

                    reasonCode = responseValues[ResponseReasonCodePosition];
                    responseReason = responseValues[ResponseReasonTextPosition];
                    approvalCode = responseValues[ResponseAuthCodePosition];
                    avsResultCode = responseValues[ResponseAvsCodePosition];
                    if (AVSResultTextLookup.Contains(avsResultCode))
                    {
                        avsResultText = (string)AVSResultTextLookup[avsResultCode];
                    }

                    transactionID = responseValues[ResponseTransactionIdPosition];
                }
            }
        }

        #endregion

        #region Constants

        // positions in the string array after split on pipe
        const int ResponseCodePosition = 0;
        const int ResponseSubCodePosition = 1;
        const int ResponseReasonCodePosition = 2;
        const int ResponseReasonTextPosition = 3;
        const int ResponseAuthCodePosition = 4;
        const int ResponseAvsCodePosition = 5;
        const int ResponseTransactionIdPosition = 6;
        const int ResponseMD5HashPosition = 37;
        const int ResponseSecurityCodeResultPosition = 38;
        const int ResponseAuthenticationValueResultPosition = 39;

        public const string ReasonInvalidCardNumber = "6";
        public const string ReasonExpiredCard = "8";
        public const string ReasonInvalidExpirationDate = "7";

        #endregion

    }


}
