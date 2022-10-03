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
    public class PlugNPayPaymentGateway : IPaymentGateway
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PlugNPayPaymentGateway));

        #region Constructors

        static PlugNPayPaymentGateway()
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

        //public PlugNPayPaymentGateway()
        //{ }

        public PlugNPayPaymentGateway(string login, string transactionKey)
        {
            if (login != null) merchantAPILogin = login;
            if (transactionKey != null) merchantAPITransactionKey = transactionKey;

        }

        #endregion

        #region Private Properties

        private static Hashtable AVSResultTextLookup;
        private Hashtable _ResponseDictionary = new Hashtable();
        private string provider = "Plug.N.Pay";
        private PaymentGatewayTransactionType transactionType = PaymentGatewayTransactionType.AuthCapture;
        private PaymentGatewayResponse response = PaymentGatewayResponse.NoRequestInitiated;

        private string testUrl = "https://pay1.plugnpay.com/payment/pnpremote.cgi";
        private string productionUrl = "https://pay1.plugnpay.com/payment/pnpremote.cgi";

        string credentials = "";
        string maskCredentials = "publisher-name=xxxxxxxx&publisher-password=xxxxxxxx";


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

        public string RawResponse //we don't want to return the actual card number or gateway credintials
        {
            get {
                if (rawResponse.Length > (16 + maskCredentials.Length))
                    return rawResponse.Replace(cardNumber, cardNumber.Substring(cardNumber.Length - 5)).Replace(credentials, maskCredentials);
                else return rawResponse;//doesn't have ccnum and credentials
            }

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

        #region IPaymentGateway Methods


        public bool ExecuteTransaction()
        {

            bool result = false;
            StringBuilder requestBody = new StringBuilder();
            requestBody.Append("publisher-name=" + merchantAPILogin);
            requestBody.Append("&publisher-password=" + merchantAPITransactionKey);
            requestBody.Append("&paymethod=credit");
            credentials = String.Format("publisher-name={0}&publisher-password={1}", merchantAPILogin, merchantAPITransactionKey);

            switch (transactionType)
            {
                case PaymentGatewayTransactionType.AuthCapture:
                    requestBody.Append("&mode=auth");
                    requestBody.Append("&authtype=authpostauth");//pnp Batches everything this autmatically marks transactions for settlement... useless if merchant uses automatic batches
                    break;

                case PaymentGatewayTransactionType.AuthOnly:
                    requestBody.Append("&mode=auth");
                    requestBody.Append("&authtype=authonly");//defaults to authonly, filled in for clarity
                    break;

                case PaymentGatewayTransactionType.CaptureOnly:
                    requestBody.Append("&mode=auth");
                    requestBody.Append("&authtype=authpostauth");//AFAIK they don't do this, with maybe the exception of Mercury Gift cards... which are catured realtime 
                    break;

                case PaymentGatewayTransactionType.Credit:
                    requestBody.Append("&mode=newreturn");//mode becomes newreturn
                    break;

                case PaymentGatewayTransactionType.PriorAuthCapture:
                    requestBody.Append("&authtype=authprev");//don't know the translation here between anet and pnp assUme force-auth or authprev
                    break;

                case PaymentGatewayTransactionType.Void:
                    requestBody.Append("&mode=void");
                    requestBody.Append("&txn-type=auth");
                    break;

            }

            requestBody.Append("&card-amount=" + FormatCharge());
            //requestBody.Append("&x_delim_data=TRUE");
            //requestBody.Append("&x_delim_char=|");
            //requestBody.Append("&x_relay_response=FALSE");

            requestBody.Append("&card-number=" + cardNumber);
            requestBody.Append("&card-exp=" + cardExpiration);

            if (cardSecurityCode.Length > 0)
            {
                requestBody.Append("&card-cvv=" + cardSecurityCode);
            }

            //if (authenticationIndicator.Length > 0)
            //{
            //    requestBody.Append("&x_authentication_indicator=" + authenticationIndicator);
            //}

            //if (cardholderAuthenticationValue.Length > 0)
            //{
            //   requestBody.Append("&x_cardholder_authentication_value=" + cardholderAuthenticationValue);
            //}

            if (previousTransactionID.Length > 0)
            {
                requestBody.Append("&prevorderid=" + previousTransactionID);
            }

            // if (previousApprovalCode.Length > 0)
            //{
            // requestBody.Append("&x_auth_code=" + previousApprovalCode);
            // }

            //requestBody.Append("&x_first_name=" + cardOwnerFirstName);
            //requestBody.Append("&x_last_name=" + cardOwnerLastName);
            requestBody.Append("&card-name=" + cardOwnerFirstName + " " + cardOwnerLastName);//This could be bad, should try to figure out how to have "Name as it appears on you cc"
            //requestBody.Append("&x_company=" + cardOwnerCompanyName);
            requestBody.Append("&card-address1=" + cardBillingAddress);
            requestBody.Append("&card-city=" + cardBillingCity);
            if ((cardBillingCountry != "US") && (cardBillingCountry != "CA"))
            {
                requestBody.Append("&card-state=ZZ");
                requestBody.Append("&card-prov=" + cardBillingState);
            }
            else
                requestBody.Append("&card-state=" + cardBillingState);
            requestBody.Append("&card-zip=" + cardBillingPostalCode);
            requestBody.Append("&card-country=" + cardBillingCountry);
            requestBody.Append("&phone=" + cardBillingPhone);
            bool shipInfo = false;

            if (shipToFirstName.Length > 0)
            {
                requestBody.Append("&shipname=" + shipToFirstName);
            }
            if (shipToLastName.Length > 0)
            {
                requestBody.Append(" " + shipToLastName);
            }
            //            if (shipToCompanyName.Length > 0)
            //            {
            //                requestBody.Append("&x_ship_to_company=" + shipToCompanyName);
            //            }
            if (shipToAddress.Length > 0)
            {
                requestBody.Append("&shipaddress1=" + shipToAddress);
            }
            if (shipToCity.Length > 0)
            {
                requestBody.Append("&city=" + shipToCity);
            }
            if (shipToState.Length > 0)
            {
                if ((shipToCountry != "US") && (shipToCountry != "CA"))
                {
                    requestBody.Append("&state=ZZ");
                    requestBody.Append("&province=" + shipToState);
                }
                else
                    requestBody.Append("&state=" + shipToState);
            }
            if (shipToPostalCode.Length > 0)
            {
                requestBody.Append("&zip=" + shipToPostalCode);
            }
            if (shipToCountry.Length > 0)
            {
                requestBody.Append("&country=" + shipToCountry);
            }
            if (shipInfo)
                requestBody.Append("&shipinfo=1");

            if (customerID.Length > 0)
            {
                requestBody.Append("&acct_code=" + customerID);
            }

            if (customerTaxID.Length > 0)
            {
                requestBody.Append("&acct_code2=" + customerTaxID);
            }

            requestBody.Append("&ipaddress=" + customerIPAddress);

            if (sendCustomerEmailConfirmation)
            {
                requestBody.Append("&email=" + cardBillingEmail);//pnp the merchant sets the email template in pnp admin area
            }
            else
            {
                requestBody.Append("&dontsndmail=yes");//pnp Do Not Send Email
            }

            if (merchantEmail.Length > 0)
            {
                requestBody.Append("&publisher-email=" + merchantEmail);
            }

            if (merchantInvoiceNumber.Length > 0)
            {
                requestBody.Append("&order-id=" + merchantInvoiceNumber);//Not to be confused with orderID which must be unique and numeric
            }
            if (merchantTransactionDescription.Length > 0)
            {
                 requestBody.Append(merchantTransactionDescription);//This may not be what the property is for but PNP allows you to have all the cart items passed to be used in their "easycart"
                 requestBody.Append("&easycart=1");
            }
            
            if (currencyCode.Length > 0)
            {
                requestBody.Append("&currency=" + currencyCode);
            }

            if (useTestMode)
            {
                // Plug.N.Pay defines testmode by the account used to access it, there is no test server all transactions are performed on the live server
            }

            //requestBody.Append("&x_version=3.1");

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

        #endregion

        #region Private Methods

        private String FormatCharge()
        {
            return chargeTotal.ToString(currencyCulture);
        }
        /// <summary>
        /// Split, decode and store query string into a hashtable
        /// </summary>
        private void dictionaryResponseDecode(string strQuery)
        {
            string sNVPairs = strQuery;
            String[] sbNameValuePair;
            String[] sbNameValuePairs;

            //split into name-value pairs: "name=value"
            sbNameValuePairs = sNVPairs.Split('&');

            for (int i = 0; i < sbNameValuePairs.Length; i++)
            {
                //split the name value pair: var1=name, var2=value
                sbNameValuePair = sbNameValuePairs[i].Split('=');
                //decode the values and add the pair to a collection
                _ResponseDictionary.Add(System.Web.HttpUtility.UrlDecode(sbNameValuePair[0]), System.Web.HttpUtility.UrlDecode(sbNameValuePair[1]));
            }

        }


        private void ParseResponse()
        {
            if (rawResponse.Length > 0)
            {
                //PARSE THE RESPONSE FROM PNP
                dictionaryResponseDecode(rawResponse);


                if (_ResponseDictionary.ContainsKey("FinalStatus"))
                {
                    if (_ResponseDictionary["FinalStatus"].ToString() == "success")
                    {
                        response = PaymentGatewayResponse.Approved;
                    }
                    else
                    {
                        response = PaymentGatewayResponse.Declined;
                    }
                }
                else
                {
                    response = PaymentGatewayResponse.Error;
                }

                try
                {
                    reasonCode = _ResponseDictionary["FinalStatus"].ToString();
                }
                catch { }

                try
                {
                    responseReason = _ResponseDictionary["MerrMsg"].ToString();
                }
                catch { }

                try
                {
                    approvalCode = _ResponseDictionary["auth-code"].ToString();
                }
                catch { }
                try
                {
                    avsResultCode = _ResponseDictionary["avs-code"].ToString();
                }
                catch { }

                if (AVSResultTextLookup.Contains(avsResultCode))
                {
                    avsResultText = (string)AVSResultTextLookup[avsResultCode];
                }

                try
                {
                    cardSecurityCodeResponseCode = _ResponseDictionary["cvvresp"].ToString();
                }
                catch { }

                transactionID = _ResponseDictionary["orderID"].ToString(); // not to be confused with order-id which we sent to the gateway
                
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
