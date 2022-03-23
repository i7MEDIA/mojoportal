using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace mojoPortal.Business.WebHelpers.PaymentGateway
{
	public class AuthorizeNETPaymentGateway : IPaymentGateway
	{
		//private static readonly ILog log = LogManager.GetLogger(typeof(AuthorizeNETPaymentGateway));

		#region Fields

		private static Hashtable AVSResultTextLookup;
		private string merchantAPILogin = string.Empty;
		private string merchantAPITransactionKey = string.Empty;

		#endregion


		#region Constructors

		static AuthorizeNETPaymentGateway()
		{
			// Create static AVS Text Lookup
			AVSResultTextLookup = new Hashtable
			{
				// AVS indications from AIM_guide.pdf
				{ "A", "Address (Street) matches, ZIP does not" },
				{ "B", "Address information not provided for AVS check" },
				{ "E", "AVS error" },
				{ "G", "Non-U.S. Card Issuing Bank" },
				{ "N", "No Match on Address (Street) or ZIP" },
				{ "P", "AVS not applicable for this transaction" },
				{ "R", "Retry - System unavailable or timed out" },
				{ "S", "Service not supported by issuer" },
				{ "U", "Address information is unavailable" },
				{ "W", "9 digit ZIP matches, Address (Street) does not" },
				{ "X", "Address (Street) and 9 digit ZIP match" },
				{ "Y", "Address (Street) and 5 digit ZIP match" },
				{ "Z", "5 digit ZIP matches, Address (Street) does not" }
			};
		}


		public AuthorizeNETPaymentGateway(string login, string transactionKey)
		{
			if (login != null)
			{
				merchantAPILogin = login;
			}

			if (transactionKey != null)
			{
				merchantAPITransactionKey = transactionKey;
			}
		}

		#endregion


		#region ARB Automated Recurring Billing Properties

		//https://api.authorize.net/xml/v1/schema/AnetApiSchema.xsd

		//private string arbProductionUrl = "https://api.authorize.net/xml/v1/request.api";

		//private string arbTestUrl = "https://apitest.authorize.net/xml/v1/request.api";

		public bool IsARB { get; set; } = false;

		#endregion


		#region Public Properties

		public string TestUrl { get; } = "https://test.authorize.net/gateway/transact.dll";
		public string ProductionUrl { get; } = "https://secure2.authorize.net/gateway/transact.dll";

		#endregion


		#region IPaymentGateway

		#region IPaymentGateway Properties

		public CultureInfo CurrencyCulture { get; set; } = CultureInfo.CurrentCulture;
		public string Provider { get; } = "Authorize.NET";
		public string CardType { get; set; } = string.Empty;
		public string CardNumber { get; set; } = string.Empty;
		public string CardExpiration { get; set; } = string.Empty;
		public string CardSecurityCode { get; set; } = string.Empty;
		public string AuthenticationIndicator { get; set; } = string.Empty;
		public string CardholderAuthenticationValue { get; set; } = string.Empty;
		public string CardOwnerFirstName { get; set; } = string.Empty;
		public string CardOwnerLastName { get; set; } = string.Empty;
		public string CardOwnerCompanyName { get; set; } = string.Empty;
		public string CardBillingAddress { get; set; } = string.Empty;
		public string CardBillingCity { get; set; } = string.Empty;
		public string CardBillingState { get; set; } = string.Empty;
		public string CardBillingPostalCode { get; set; } = string.Empty;
		public string CardBillingCountry { get; set; } = string.Empty;
		public string CardBillingCountryCode { get; set; } = string.Empty;
		public string CardBillingPhone { get; set; } = string.Empty;
		public string CardBillingEmail { get; set; } = string.Empty;
		public string ShipToFirstName { get; set; } = string.Empty;
		public string ShipToLastName { get; set; } = string.Empty;
		public string ShipToCompanyName { get; set; } = string.Empty;
		public string ShipToAddress { get; set; } = string.Empty;
		public string ShipToCity { get; set; } = string.Empty;
		public string ShipToState { get; set; } = string.Empty;
		public string ShipToPostalCode { get; set; } = string.Empty;
		public string ShipToCountry { get; set; } = string.Empty;
		public string CustomerIPAddress { get; set; } = string.Empty;
		public string CustomerTaxId { get; set; } = string.Empty;
		public string CustomerId { get; set; } = string.Empty;
		public bool SendCustomerEmailConfirmation { get; set; } = false;
		public string MerchantEmail { get; set; } = string.Empty;
		public string MerchantInvoiceNumber { get; set; } = string.Empty;
		public string MerchantTransactionDescription { get; set; } = string.Empty;
		public string MerchantEmailConfirmationHeader { get; set; } = string.Empty;
		public string MerchantEmailConfirmationFooter { get; set; } = string.Empty;
		public string CurrencyCode { get; set; } = "";
		public decimal ChargeTotal { get; set; } = 0;
		public PaymentGatewayTransactionType TransactionType { get; set; } = PaymentGatewayTransactionType.AuthCapture;
		public string PreviousTransactionId { get; set; } = string.Empty;
		public string PreviousApprovalCode { get; set; } = string.Empty;
		public string TransactionId { get; private set; } = string.Empty;
		public string ApprovalCode { get; private set; } = string.Empty;
		public string ResponseCode { get; private set; } = string.Empty;
		public string ReasonCode { get; private set; } = string.Empty;
		public string ResponseReason { get; private set; } = string.Empty;
		public PaymentGatewayResponse Response { get; private set; } = PaymentGatewayResponse.NoRequestInitiated;
		public string AvsResultCode { get; private set; } = string.Empty;
		public string AvsResultText { get; private set; } = string.Empty;
		public string CardSecurityCodeResponseCode { get; } = string.Empty;
		public string CardholderAuthenticationValueResponseCode { get; } = string.Empty;
		public bool UseTestMode { get; set; } = false;
		public string RawResponse { get; set; } = string.Empty;
		public Exception LastExecutionException { get; } = null;
		public int TimeoutInMilliseconds { get; set; } = 120000;
		public PaymentGatewayType Type { get; set; } = PaymentGatewayType.Direct;

		#endregion


		#region IPaymentGateway Methods

		public bool ExecuteTransaction()
		{
			var result = false;
			var requestBody = new StringBuilder();

			requestBody.Append("x_login=" + merchantAPILogin);
			requestBody.Append("&x_tran_key=" + merchantAPITransactionKey);
			requestBody.Append("&x_method=CC");

			switch (TransactionType)
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

			requestBody.Append("&x_card_num=" + CardNumber);
			requestBody.Append("&x_exp_date=" + CardExpiration);

			if (CardSecurityCode.Length > 0)
			{
				requestBody.Append("&x_card_code=" + CardSecurityCode);
			}

			if (AuthenticationIndicator.Length > 0)
			{
				requestBody.Append("&x_authentication_indicator=" + AuthenticationIndicator);
			}

			if (CardholderAuthenticationValue.Length > 0)
			{
				requestBody.Append("&x_cardholder_authentication_value=" + CardholderAuthenticationValue);
			}

			if (PreviousTransactionId.Length > 0)
			{
				requestBody.Append("&x_trans_id=" + PreviousTransactionId);
			}

			if (PreviousApprovalCode.Length > 0)
			{
				requestBody.Append("&x_auth_code=" + PreviousApprovalCode);
			}

			requestBody.Append("&x_first_name=" + CardOwnerFirstName);
			requestBody.Append("&x_last_name=" + CardOwnerLastName);
			requestBody.Append("&x_company=" + CardOwnerCompanyName);
			requestBody.Append("&x_address=" + CardBillingAddress);
			requestBody.Append("&x_city=" + CardBillingCity);
			requestBody.Append("&x_state=" + CardBillingState);
			requestBody.Append("&x_zip=" + CardBillingPostalCode);
			requestBody.Append("&x_country=" + CardBillingCountry);
			requestBody.Append("&x_phone=" + CardBillingPhone);

			if (ShipToFirstName.Length > 0)
			{
				requestBody.Append("&x_ship_to_first_name=" + ShipToFirstName);
			}

			if (ShipToLastName.Length > 0)
			{
				requestBody.Append("&x_ship_to_last_name=" + ShipToLastName);
			}

			if (ShipToCompanyName.Length > 0)
			{
				requestBody.Append("&x_ship_to_company=" + ShipToCompanyName);
			}

			if (ShipToAddress.Length > 0)
			{
				requestBody.Append("&x_ship_to_address=" + ShipToAddress);
			}

			if (ShipToCity.Length > 0)
			{
				requestBody.Append("&x_ship_to_city=" + ShipToCity);
			}

			if (ShipToState.Length > 0)
			{
				requestBody.Append("&x_ship_to_state=" + ShipToState);
			}

			if (ShipToPostalCode.Length > 0)
			{
				requestBody.Append("&x_ship_to_zip=" + ShipToPostalCode);
			}

			if (ShipToCountry.Length > 0)
			{
				requestBody.Append("&x_ship_to_country=" + ShipToCountry);
			}

			if (CustomerId.Length > 0)
			{
				requestBody.Append("&x_cust_id=" + CustomerId);
			}

			if (CustomerTaxId.Length > 0)
			{
				requestBody.Append("&x_customer_tax_id=" + CustomerTaxId);
			}

			requestBody.Append("&x_customer_ip=" + CustomerIPAddress);

			if (SendCustomerEmailConfirmation)
			{
				requestBody.Append("&x_email_customer=TRUE");
				requestBody.Append("&x_email=" + CardBillingEmail);

				if (MerchantEmailConfirmationHeader.Length > 0)
				{
					requestBody.Append("&x_header_email_receipt=" + MerchantEmailConfirmationHeader);
				}

				if (MerchantEmailConfirmationFooter.Length > 0)
				{
					requestBody.Append("&x_footer_email_receipt=" + MerchantEmailConfirmationFooter);
				}
			}

			if (MerchantEmail.Length > 0)
			{
				requestBody.Append("&x_merchant_email=" + MerchantEmail);
			}

			if (MerchantInvoiceNumber.Length > 0)
			{
				requestBody.Append("&x_invoice_num=" + MerchantInvoiceNumber);
			}

			if (MerchantTransactionDescription.Length > 0)
			{
				requestBody.Append("&x_description=" + MerchantTransactionDescription);
			}

			if (CurrencyCode.Length > 0)
			{
				requestBody.Append("&x_currency_code=" + CurrencyCode);
			}

			if (UseTestMode)
			{
				// authorize.net supports test requests
				// this is supported on both production and test servers
				// I'm commenting this out because I want to test on test server as if it were production
				// so I don't want to pass this
				//requestBody.Append("&x_test_request=TRUE");
			}

			requestBody.Append("&x_version=3.1");

			string url;

			if (UseTestMode)
			{
				url = TestUrl;
			}
			else
			{
				url = ProductionUrl;
			}

			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

			webRequest.Method = "POST";
			webRequest.Timeout = TimeoutInMilliseconds;
			webRequest.ContentType = "application/x-www-form-urlencoded";
			webRequest.ContentLength = requestBody.Length;

			var requestStream = new StreamWriter(webRequest.GetRequestStream());

			if (requestStream != null)
			{
				requestStream.Write(requestBody.ToString());
			}

			if (requestStream != null)
			{
				requestStream.Close();
			}

			HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

			if (webResponse != null)
			{
				using (StreamReader responseStream = new StreamReader(webResponse.GetResponseStream()))
				{
					RawResponse = responseStream.ReadToEnd();
					result = true;
				}

				ParseResponse();
			}
			else
			{
				// TODO: error message?
				Response = PaymentGatewayResponse.Error;

				return false;
			}

			return result;
		}


		public void LogTransaction(Guid siteGuid, Guid moduleGuid, Guid storeGuid, Guid cartGuid, Guid userGuid, string providerName, string method, string serializedCart)
		{
			var transactionLog = new PaymentLog
			{
				RawResponse = RawResponse,
				Amount = ChargeTotal,
				AuthCode = ApprovalCode,
				AvsCode = AvsResultCode,
				CartGuid = cartGuid,
				CcvCode = CardSecurityCodeResponseCode,
				Reason = ResponseReason,
				ResponseCode = ResponseCode,
				SiteGuid = siteGuid,
				StoreGuid = storeGuid,
				TransactionId = TransactionId,
				TransactionType = TransactionType.ToString(),
				UserGuid = userGuid,
				Method = method
			};

			transactionLog.Save();
		}

		#endregion

		#endregion


		#region Private Methods

		private string FormatCharge()
		{
			return ChargeTotal.ToString(CurrencyCulture);
		}

		private void ParseResponse()
		{
			if (RawResponse.Length > 0)
			{
				char[] separator = { '|' };
				string[] responseValues = RawResponse.Split(separator, StringSplitOptions.None);

				//should be at least 39 elements
				if (responseValues.Length > 38)
				{
					ResponseCode = responseValues[ResponseCodePosition];

					switch (ResponseCode)
					{
						case "1":
							Response = PaymentGatewayResponse.Approved;
							break;

						case "2":
							Response = PaymentGatewayResponse.Declined;
							break;

						case "3":
							Response = PaymentGatewayResponse.Error;
							break;
					}

					ReasonCode = responseValues[ResponseReasonCodePosition];
					ResponseReason = responseValues[ResponseReasonTextPosition];
					ApprovalCode = responseValues[ResponseAuthCodePosition];
					AvsResultCode = responseValues[ResponseAvsCodePosition];

					if (AVSResultTextLookup.Contains(AvsResultCode))
					{
						AvsResultText = (string)AVSResultTextLookup[AvsResultCode];
					}

					TransactionId = responseValues[ResponseTransactionIdPosition];
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
