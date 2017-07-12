// Author:					
// Created:				    2007-02-12
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
    /// 
    /// </summary>
    public interface IPaymentGateway
    {
        string Provider { get; }
        bool UseTestMode { get; set; }
        //bool UseTestUrl { get; set; }

        string CardType { get; set; }
        string CardNumber { get; set; }
        string CardSecurityCode { get; set; } // CVV2, CVC2, or CID
        string AuthenticationIndicator { get; set; }
        string CardholderAuthenticationValue { get; set; } // CAVV, AVV, or UCAF
        string CardExpiration { get; set; }

        string CardOwnerFirstName { get; set; }
        string CardOwnerLastName { get; set; }
        string CardOwnerCompanyName { get; set; }
        string CardBillingAddress { get; set; }
        string CardBillingCity { get; set; }
        string CardBillingState { get; set; }
        string CardBillingPostalCode { get; set; }
        string CardBillingCountry { get; set; }
        string CardBillingCountryCode { get; set; }
        string CardBillingPhone { get; set; }
        string CardBillingEmail { get; set; }
        string ShipToFirstName { get; set; }
        string ShipToLastName { get; set; }
        string ShipToCompanyName { get; set; }
        string ShipToAddress { get; set; }
        string ShipToCity { get; set; }
        string ShipToState { get; set; }
        string ShipToPostalCode { get; set; }
        string ShipToCountry { get; set; }
        string CustomerIPAddress { get; set; }
        string CustomerTaxId { get; set; }
        string CustomerId { get; set; }

        string MerchantInvoiceNumber { get; set; }
        string MerchantTransactionDescription { get; set; }

        bool SendCustomerEmailConfirmation { get; set; }
        string MerchantEmailConfirmationHeader { get; set; }
        string MerchantEmailConfirmationFooter { get; set; }
        string MerchantEmail { get; set; }

        string PreviousTransactionId { get; set; } // only for capture, void , or credit
        string TransactionId { get; }
        string PreviousApprovalCode { get; set; } // only for capture, void , or credit
        string ApprovalCode { get; }
        string ResponseCode { get; }
        string ReasonCode { get; }
        string ResponseReason { get; }
        string AvsResultCode { get; }
        string AvsResultText { get; }
        string CardSecurityCodeResponseCode { get; }
        string CardholderAuthenticationValueResponseCode { get; }

        CultureInfo CurrencyCulture { get; set; }
        string CurrencyCode { get; set; }
        decimal ChargeTotal { get; set; }


        int TimeoutInMilliseconds { get; set; }
        PaymentGatewayTransactionType TransactionType { get; set; }
        PaymentGatewayResponse Response { get; }
        string RawResponse { get; }
        Exception LastExecutionException { get; }

        bool ExecuteTransaction();
        void LogTransaction(Guid siteGuid, Guid moduleGuid, Guid storeGuid, Guid cartGuid, Guid userGuid, string providerName, string method, string serializedCart);

    }

}
