// Author:					
// Created:				    2008-03-05
// Last Modified:		    2013-06-27
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;

namespace mojoPortal.Business.WebHelpers.PaymentGateway
{


    /// <summary>
    /// This is just a stub that you can copy for a starting point in implementing new gateways
    /// 
    /// </summary>
    public class NotImplementedPaymentGateway : IPaymentGateway
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(NotImplementedPaymentGateway));

        #region Constructors

        public NotImplementedPaymentGateway()
        { }



        #endregion

        #region Private Properties



        private string provider = "mojoPortal Not Implemented Payment Gateway";
        private PaymentGatewayTransactionType transactionType = PaymentGatewayTransactionType.AuthCapture;
        private PaymentGatewayResponse response = PaymentGatewayResponse.NoRequestInitiated;

        private string testUrl = "";
        private string productionUrl = "";

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


        private string currencyCode = "";
        private decimal chargeTotal = 0;

        private bool useTestMode = false;
        private bool useTestUrl = false;
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

        public bool UseTestUrl
        {
            get { return useTestUrl; }
            set { useTestUrl = value; }
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

        public PaymentGatewayType Type { get; set; } = PaymentGatewayType.None;

        #endregion

        #region IPaymentGateway Methods


        public bool ExecuteTransaction()
        {
            response = PaymentGatewayResponse.NoRequestInitiated;
            return false;

        }

        public void LogTransaction(Guid siteGuid, Guid moduleGuid, Guid storeGuid, Guid cartGuid, Guid userGuid, string providerName, string method, string serializedCart)
        { 
        }

        #endregion

        #endregion

        #region Private Methods

        private String FormatCharge()
        {
            return chargeTotal.ToString();
        }

        private void ParseResponse()
        {
            if (rawResponse.Length > 0)
            {

            }
        }

        #endregion

        #region Constants

        // positions in the string array after split on pipe


        #endregion

    }


}
