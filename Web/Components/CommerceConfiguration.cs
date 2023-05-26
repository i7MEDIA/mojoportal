using mojoPortal.Business;
using mojoPortal.Business.WebHelpers.PaymentGateway;
using mojoPortal.Web.Commerce;
using mojoPortal.Web.Framework;
using System.Configuration;

namespace mojoPortal.Web
{
	public class CommerceConfiguration
	{
		#region Constants

		private const string Is503TaxExemptSetting = "Is503TaxExempt";
		private const string PaymentGatewayUseTestModeConfig = "PaymentGatewayUseTestMode";
		private const string PrimaryPaymentGatewayConfig = "PrimaryPaymentGateway";

		//private const string AuthorizeNetProductionAPILoginConfig = "AuthorizeNetProductionAPILogin";
		//private const string AuthorizeNetProductionAPITransactionKeyConfig = "AuthorizeNetProductionAPITransactionKey";

		//private const string AuthorizeNetSandboxAPILoginConfig = "AuthorizeNetSandboxAPILogin";
		//private const string AuthorizeNetSandboxAPITransactionKeyConfig = "AuthorizeNetSandboxAPITransactionKey";

		//private const string PlugNPayProductionAPIPublisherNameConfig = "PlugNPayProductionAPIPublisherName";
		//private const string PlugNPayProductionAPIPublisherPasswordConfig = "PlugNPayProductionAPIPublisherPassword";

		//private const string PlugNPaySandboxAPIPublisherNameConfig = "PlugNPaySandboxAPIPublisherName";
		//private const string PlugNPaySandboxAPIPublisherPasswordConfig = "PlugNPaySandboxAPIPublisherPassword";


		private const string PayPalUsePayPalStandardConfig = "PayPalUsePayPalStandard";
		private const string PayPalStandardProductionEmailConfig = "PayPalStandardProductionEmail";
		private const string PayPalStandardSandboxEmailConfig = "PayPalStandardSandboxEmail";
		private const string PayPalStandardProductionPDTConfig = "PayPalStandardProductionPDTId";
		private const string PayPalStandardSandboxPDTConfig = "PayPalStandardSandboxPDTId";

		private const string PayPalStandardProductionUrlConfig = "PayPalStandardProductionUrl";
		private const string PayPalStandardSandboxUrlConfig = "PayPalStandardSandboxUrl";

		private const string PayPalProductionAPIUsernameConfig = "PayPalProductionAPIUsername";
		private const string PayPalProductionAPIPasswordConfig = "PayPalProductionAPIPassword";
		private const string PayPalProductionAPISignatureConfig = "PayPalProductionAPISignature";

		private const string PayPalSandboxAPIUsernameConfig = "PayPalSandboxAPIUsername";
		private const string PayPalSandboxAPIPasswordConfig = "PayPalSandboxAPIPassword";
		private const string PayPalSandboxAPISignatureConfig = "PayPalSandboxAPISignature";

		private const string GoogleProductionMerchantIDConfig = "GoogleProductionMerchantID";
		private const string GoogleProductionMerchantKeyConfig = "GoogleProductionMerchantKey";

		private const string GoogleSandboxMerchantIDConfig = "GoogleSandboxMerchantID";
		private const string GoogleSandboxMerchantKeyConfig = "GoogleSandboxMerchantKey";

		#endregion


		#region Private Properties

		private readonly IPaymentGateway paymentGateway = null;
		private readonly string configPrefix = string.Empty;

		// PayPal Pro
		private string payPalProductionAPIUsername = string.Empty;
		private string payPalProductionAPIPassword = string.Empty;
		private string payPalProductionAPISignature = string.Empty;
		private string payPalSandboxAPIUsername = string.Empty;
		private string payPalSandboxAPIPassword = string.Empty;
		private string payPalSandboxAPISignature = string.Empty;
		private string payPalAccountProductionEmailAddress = string.Empty;
		private string payPalAccountProductionPDTId = string.Empty;
		private string payPalStandardProductionUrl = "https://www.paypal.com/cgi-bin/webscr";
		private string payPalAccountSandboxEmailAddress = string.Empty;
		private string payPalAccountSandboxPDTId = string.Empty;
		private string payPalStandardSandboxUrl = "https://www.sandbox.paypal.com/cgi-bin/webscr";
		// end PayPalStandard

		private string googleProductionMerchantID = string.Empty;
		private string googleProductionMerchantKey = string.Empty;
		private string googleSandboxMerchantID = string.Empty;
		private string googleSandboxMerchantKey = string.Empty;

		#endregion


		#region Public Properties

		public bool Is503TaxExempt { get; private set; } = false;
		public bool PaymentGatewayUseTestMode { get; private set; } = true;
		public string WorldPayInstallationId { get; private set; } = string.Empty;
		public string WorldPayMerchantCode { get; private set; } = string.Empty;
		public string WorldPayMd5Secret { get; private set; } = string.Empty;
		public string WorldPayResponsePassword { get; private set; } = string.Empty;
		public bool WorldPayProduceShopperResponse { get; private set; } = true;
		public bool WorldPayProduceShopperCancellationResponse { get; private set; } = true;
		public string WorldPayShopperResponseTemplate { get; private set; } = "DefaultWorldPayOrderConfirmationTemplate.config";
		public string WorldPayShopperCancellationResponseTemplate { get; private set; } = "DefaultWorldPayOrderCancelledTemplate.config";
		public string PrimaryPaymentGateway { get; private set; } = string.Empty;
		public bool IsConfigured => CanProcessStandardCards || GoogleCheckoutIsEnabled || PayPalIsEnabled;

		/// <summary>
		/// indicates whether we can process payments directly.
		/// true if using PayPalDirect or Authorize.NET for the primary gateway.
		/// False if not using one of these. If we process using PayPal Standard or Google Checkout,
		/// we are not processing cards directly, they are processed on external sites.
		/// </summary>
		public bool CanProcessStandardCards => paymentGateway != null;
		public bool GoogleCheckoutIsEnabled => false;

		public bool PayPalIsEnabled
		{
			get
			{
				if (!PayPalUsePayPalStandard)
				{
					if (PayPalAPIUsername.Length == 0 || PayPalAPIPassword.Length == 0 || PayPalAPISignature.Length == 0)
					{
						return false;
					}
				}

				if (PayPalUsePayPalStandard && PayPalStandardEmailAddress.Length == 0)
				{
					return false;
				}


				return true;
			}
		}

		public bool PayPalUsePayPalStandard { get; private set; } = false;
		public string PayPalStandardEmailAddress => PaymentGatewayUseTestMode ? payPalAccountSandboxEmailAddress : payPalAccountProductionEmailAddress;
		public string PayPalStandardPDTId => PaymentGatewayUseTestMode ? payPalAccountSandboxPDTId : payPalAccountProductionPDTId;
		public string PayPalStandardUrl => PaymentGatewayUseTestMode ? payPalStandardSandboxUrl : payPalStandardProductionUrl;
		public string PayPalAPIUsername => PaymentGatewayUseTestMode ? payPalSandboxAPIUsername : payPalProductionAPIUsername;
		public string PayPalAPIPassword => PaymentGatewayUseTestMode ? payPalSandboxAPIPassword : payPalProductionAPIPassword;
		public string PayPalAPISignature => PaymentGatewayUseTestMode ? payPalSandboxAPISignature : payPalProductionAPISignature;
		//public string GoogleMerchantID => PaymentGatewayUseTestMode ? googleSandboxMerchantID : googleProductionMerchantID;
		//public string GoogleMerchantKey => PaymentGatewayUseTestMode ? googleSandboxMerchantKey : googleProductionMerchantKey;
		public int DefaultTimeoutInMilliseconds { get; } = 30000;
		public string DefaultConfirmationEmailSubjectTemplate { get; private set; } = "DefaultOrderConfirmationEmailSubjectTemplate.config";
		public string DefaultConfirmationEmailTextBodyTemplate { get; private set; } = "DefaultOrderConfirmationPlainTextEmailTemplate.config";
		public string DefaultOrderReceivedEmailSubjectTemplate { get; private set; } = "DefaultOrderReceivedEmailSubjectTemplate.config";
		public string DefaultOrderReceivedEmailTextBodyTemplate { get; private set; } = "DefaultOrderReceivedPlainTextEmailTemplate.config";
		public string DefaultEcheckOrderReceivedEmailTextBodyTemplate { get; private set; } = "DefaultEcheckOrderReceivedPlainTextEmailTemplate.config";

		#endregion


		#region Constructors

		public CommerceConfiguration(SiteSettings siteSettings)
		{
			configPrefix = $"Site{siteSettings.SiteId.ToInvariantString()}-";

			LoadConfigSettings();

			PaymentGatewayProvider gatewayProvider = PaymentGatewayManager.Providers[PrimaryPaymentGateway];

			if (gatewayProvider != null)
			{
				paymentGateway = gatewayProvider.GetPaymentGateway();
			}
		}

		#endregion


		#region Public Methods

		public IPaymentGateway GetDirectPaymentGateway() => !CanProcessStandardCards ? null : paymentGateway;

		#endregion


		#region Private Methods

		private void LoadConfigSettings()
		{
			if (WebConfigSettings.CommerceUseGlobalSettings) //false by default
			{
				LoadGlobalSettings();
			}
			else
			{
				LoadSiteSpecificSettings();
			}
		}


		private void LoadGlobalSettings()
		{
			Is503TaxExempt = WebConfigSettings.CommerceGlobalIs503TaxExempt;
			PaymentGatewayUseTestMode = WebConfigSettings.CommerceGlobalUseTestMode;
			PrimaryPaymentGateway = WebConfigSettings.CommerceGlobalPrimaryGateway;

			WorldPayInstallationId = WebConfigSettings.CommerceGlobalWorldPayInstallationId;
			WorldPayMerchantCode = WebConfigSettings.CommerceGlobalWorldPayMerchantCode;
			WorldPayMd5Secret = WebConfigSettings.CommerceGlobalWorldPayMd5Secret;
			WorldPayResponsePassword = WebConfigSettings.CommerceGlobalWorldPayResponsePassword;

			if (WebConfigSettings.CommerceGlobalWorldPayShopperResponseTemplate.Length > 0)
			{
				WorldPayShopperResponseTemplate = WebConfigSettings.CommerceGlobalWorldPayShopperResponseTemplate;
			}

			if (WebConfigSettings.CommerceGlobalWorldPayShopperCancellationResponseTemplate.Length > 0)
			{
				WorldPayShopperCancellationResponseTemplate = WebConfigSettings.CommerceGlobalWorldPayShopperCancellationResponseTemplate;
			}

			WorldPayProduceShopperResponse = WebConfigSettings.CommerceGlobalWorldPayProduceShopperResponse;
			WorldPayProduceShopperCancellationResponse = WebConfigSettings.CommerceGlobalWorldPayProduceShopperCancellationResponse;

			payPalAccountProductionEmailAddress = WebConfigSettings.CommerceGlobalPayPalAccountProductionEmailAddress;
			payPalAccountProductionPDTId = WebConfigSettings.CommerceGlobalPayPalAccountProductionPDTId;
			payPalAccountSandboxEmailAddress = WebConfigSettings.CommerceGlobalPayPalAccountSandboxEmailAddress;
			payPalAccountSandboxPDTId = WebConfigSettings.CommerceGlobalPayPalAccountSandboxPDTId;
			PayPalUsePayPalStandard = WebConfigSettings.CommerceGlobalUsePayPalStandard;
			payPalStandardProductionUrl = WebConfigSettings.CommerceGlobalPayPalStandardProductionUrl;
			payPalStandardSandboxUrl = WebConfigSettings.CommerceGlobalPayPalStandardSandboxUrl;
			payPalProductionAPIUsername = WebConfigSettings.CommerceGlobalPayPalProductionAPIUsername;
			payPalProductionAPIPassword = WebConfigSettings.CommerceGlobalPayPalProductionAPIPassword;
			payPalProductionAPISignature = WebConfigSettings.CommerceGlobalPayPalProductionAPISignature;
			payPalSandboxAPIUsername = WebConfigSettings.CommerceGlobalPayPalSandboxAPIUsername;
			payPalSandboxAPIPassword = WebConfigSettings.CommerceGlobalPayPalSandboxAPIPassword;
			payPalSandboxAPISignature = WebConfigSettings.CommerceGlobalPayPalSandboxAPISignature;

			googleProductionMerchantID = WebConfigSettings.CommerceGlobalGoogleProductionMerchantID;
			googleProductionMerchantKey = WebConfigSettings.CommerceGlobalGoogleProductionMerchantKey;
			googleSandboxMerchantID = WebConfigSettings.CommerceGlobalGoogleSandboxMerchantID;
			googleSandboxMerchantKey = WebConfigSettings.CommerceGlobalGoogleSandboxMerchantKey;
		}


		private void LoadSiteSpecificSettings()
		{
			Is503TaxExempt = Core.Configuration.ConfigHelper.GetBoolProperty(configPrefix + Is503TaxExemptSetting, Is503TaxExempt);
			PaymentGatewayUseTestMode = Core.Configuration.ConfigHelper.GetBoolProperty(configPrefix + PaymentGatewayUseTestModeConfig, PaymentGatewayUseTestMode);

			if (ConfigurationManager.AppSettings[configPrefix + PrimaryPaymentGatewayConfig] != null)
			{
				PrimaryPaymentGateway = ConfigurationManager.AppSettings[configPrefix + PrimaryPaymentGatewayConfig];
			}

			// paypal standard
			if (ConfigurationManager.AppSettings[configPrefix + PayPalStandardProductionEmailConfig] != null)
			{
				payPalAccountProductionEmailAddress = ConfigurationManager.AppSettings[configPrefix + PayPalStandardProductionEmailConfig];
			}

			if (ConfigurationManager.AppSettings[configPrefix + PayPalStandardProductionPDTConfig] != null)
			{
				payPalAccountProductionPDTId = ConfigurationManager.AppSettings[configPrefix + PayPalStandardProductionPDTConfig];
			}

			if (ConfigurationManager.AppSettings[configPrefix + PayPalStandardSandboxPDTConfig] != null)
			{
				payPalAccountSandboxPDTId = ConfigurationManager.AppSettings[configPrefix + PayPalStandardSandboxPDTConfig];
			}

			PayPalUsePayPalStandard = Core.Configuration.ConfigHelper.GetBoolProperty(configPrefix + PayPalUsePayPalStandardConfig, PayPalUsePayPalStandard);

			if (ConfigurationManager.AppSettings[configPrefix + PayPalStandardProductionUrlConfig] != null)
			{
				payPalStandardProductionUrl = ConfigurationManager.AppSettings[configPrefix + PayPalStandardProductionUrlConfig];
			}

			if (ConfigurationManager.AppSettings[configPrefix + PayPalStandardSandboxEmailConfig] != null)
			{
				payPalAccountSandboxEmailAddress = ConfigurationManager.AppSettings[configPrefix + PayPalStandardSandboxEmailConfig];
			}

			if (ConfigurationManager.AppSettings[configPrefix + PayPalStandardSandboxUrlConfig] != null)
			{
				payPalStandardSandboxUrl = ConfigurationManager.AppSettings[configPrefix + PayPalStandardSandboxUrlConfig];
			}

			if (ConfigurationManager.AppSettings[configPrefix + PayPalProductionAPIUsernameConfig] != null)
			{
				payPalProductionAPIUsername = ConfigurationManager.AppSettings[configPrefix + PayPalProductionAPIUsernameConfig];
			}

			if (ConfigurationManager.AppSettings[configPrefix + PayPalProductionAPIPasswordConfig] != null)
			{
				payPalProductionAPIPassword = ConfigurationManager.AppSettings[configPrefix + PayPalProductionAPIPasswordConfig];
			}

			if (ConfigurationManager.AppSettings[configPrefix + PayPalProductionAPISignatureConfig] != null)
			{
				payPalProductionAPISignature = ConfigurationManager.AppSettings[configPrefix + PayPalProductionAPISignatureConfig];
			}

			if (ConfigurationManager.AppSettings[configPrefix + PayPalSandboxAPIUsernameConfig] != null)
			{
				payPalSandboxAPIUsername = ConfigurationManager.AppSettings[configPrefix + PayPalSandboxAPIUsernameConfig];
			}

			if (ConfigurationManager.AppSettings[configPrefix + PayPalSandboxAPIPasswordConfig] != null)
			{
				payPalSandboxAPIPassword = ConfigurationManager.AppSettings[configPrefix + PayPalSandboxAPIPasswordConfig];
			}

			if (ConfigurationManager.AppSettings[configPrefix + PayPalSandboxAPISignatureConfig] != null)
			{
				payPalSandboxAPISignature = ConfigurationManager.AppSettings[configPrefix + PayPalSandboxAPISignatureConfig];
			}

			// Google Checkout
			if (ConfigurationManager.AppSettings[configPrefix + GoogleProductionMerchantIDConfig] != null)
			{
				googleProductionMerchantID = ConfigurationManager.AppSettings[configPrefix + GoogleProductionMerchantIDConfig];
			}

			if (ConfigurationManager.AppSettings[configPrefix + GoogleProductionMerchantKeyConfig] != null)
			{
				googleProductionMerchantKey = ConfigurationManager.AppSettings[configPrefix + GoogleProductionMerchantKeyConfig];
			}

			if (ConfigurationManager.AppSettings[configPrefix + GoogleSandboxMerchantIDConfig] != null)
			{
				googleSandboxMerchantID = ConfigurationManager.AppSettings[configPrefix + GoogleSandboxMerchantIDConfig];
			}

			if (ConfigurationManager.AppSettings[configPrefix + GoogleSandboxMerchantKeyConfig] != null)
			{
				googleSandboxMerchantKey = ConfigurationManager.AppSettings[configPrefix + GoogleSandboxMerchantKeyConfig];
			}

			// World Pay
			if (ConfigurationManager.AppSettings[configPrefix + "WorldPayInstallationId"] != null)
			{
				WorldPayInstallationId = ConfigurationManager.AppSettings[configPrefix + "WorldPayInstallationId"];
			}

			if (ConfigurationManager.AppSettings[configPrefix + "WorldPayMerchantCode"] != null)
			{
				WorldPayMerchantCode = ConfigurationManager.AppSettings[configPrefix + "WorldPayMerchantCode"];
			}

			if (ConfigurationManager.AppSettings[configPrefix + "WorldPayMd5Secret"] != null)
			{
				WorldPayMd5Secret = ConfigurationManager.AppSettings[configPrefix + "WorldPayMd5Secret"];
			}

			if (ConfigurationManager.AppSettings[configPrefix + "WorldPayResponsePassword"] != null)
			{
				WorldPayResponsePassword = ConfigurationManager.AppSettings[configPrefix + "WorldPayResponsePassword"];
			}

			if (ConfigurationManager.AppSettings[configPrefix + "WorldPayShopperResponseTemplate"] != null)
			{
				WorldPayShopperResponseTemplate = ConfigurationManager.AppSettings[configPrefix + "WorldPayShopperResponseTemplate"];
			}

			if (ConfigurationManager.AppSettings[configPrefix + "WorldPayShopperCancellationResponseTemplate"] != null)
			{
				WorldPayShopperCancellationResponseTemplate = ConfigurationManager.AppSettings[configPrefix + "WorldPayShopperCancellationResponseTemplate"];
			}

			WorldPayProduceShopperResponse = Core.Configuration.ConfigHelper.GetBoolProperty(configPrefix + "WorldPayProduceShopperResponse", WorldPayProduceShopperResponse);
			WorldPayProduceShopperCancellationResponse = Core.Configuration.ConfigHelper.GetBoolProperty(configPrefix + "WorldPayProduceShopperCancellationResponse", WorldPayProduceShopperCancellationResponse);

			// Default configuration settings
			if (ConfigurationManager.AppSettings[configPrefix + "ConfirmationEmailPlainTextTemplate"] != null)
			{
				DefaultConfirmationEmailTextBodyTemplate = ConfigurationManager.AppSettings[configPrefix + "ConfirmationEmailPlainTextTemplate"];
			}

			if (ConfigurationManager.AppSettings[configPrefix + "ConfirmationEmailSubjectTemplate"] != null)
			{
				DefaultConfirmationEmailSubjectTemplate = ConfigurationManager.AppSettings[configPrefix + "ConfirmationEmailSubjectTemplate"];
			}

			if (ConfigurationManager.AppSettings[configPrefix + "OrderReceivedEmailPlainTextTemplate"] != null)
			{
				DefaultOrderReceivedEmailTextBodyTemplate = ConfigurationManager.AppSettings[configPrefix + "OrderReceivedEmailPlainTextTemplate"];
			}

			if (ConfigurationManager.AppSettings[configPrefix + "OrderReceivedEmailSubjectTemplate"] != null)
			{
				DefaultOrderReceivedEmailSubjectTemplate = ConfigurationManager.AppSettings[configPrefix + "OrderReceivedEmailSubjectTemplate"];
			}

			if (ConfigurationManager.AppSettings[configPrefix + "EcheckOrderReceivedEmailPlainTextTemplate"] != null)
			{
				DefaultEcheckOrderReceivedEmailTextBodyTemplate = ConfigurationManager.AppSettings[configPrefix + "EcheckOrderReceivedEmailPlainTextTemplate"];
			}
		}

		#endregion
	}
}
