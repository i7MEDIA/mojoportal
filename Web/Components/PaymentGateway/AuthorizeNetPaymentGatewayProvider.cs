using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.PaymentGateway;
using mojoPortal.Web.Framework;
using System.Configuration;

namespace mojoPortal.Web.Commerce
{
	public class AuthorizeNetPaymentGatewayProvider : PaymentGatewayProvider
	{
		#region Fields

		private const string PaymentGatewayUseTestModeConfig = "PaymentGatewayUseTestMode";

		private const string AuthorizeNetProductionAPILoginConfig = "AuthorizeNetProductionAPILogin";
		private const string AuthorizeNetProductionAPITransactionKeyConfig = "AuthorizeNetProductionAPITransactionKey";

		private const string AuthorizeNetSandboxAPILoginConfig = "AuthorizeNetSandboxAPILogin";
		private const string AuthorizeNetSandboxAPITransactionKeyConfig = "AuthorizeNetSandboxAPITransactionKey";

		private string configPrefix = string.Empty;
		private bool paymentGatewayUseTestMode = false;
		private string authorizeNetAPILogin = string.Empty;
		private string authorizeNetAPITransactionKey = string.Empty;
		private bool didLoadSettings = false;

		#endregion


		#region Public Methods

		public override IPaymentGateway GetPaymentGateway()
		{
			if (!didLoadSettings)
			{
				LoadSettings();
			}

			if (authorizeNetAPILogin.Length > 0 && authorizeNetAPITransactionKey.Length > 0)
			{
				AuthorizeNETPaymentGateway gateway = new AuthorizeNETPaymentGateway(authorizeNetAPILogin, authorizeNetAPITransactionKey)
				{
					UseTestMode = paymentGatewayUseTestMode
				};

				return gateway;
			}

			return null;
		}

		#endregion


		#region Private Methods

		private void LoadSettings()
		{
			if (WebConfigSettings.CommerceUseGlobalSettings) //false by default
			{
				paymentGatewayUseTestMode = WebConfigSettings.CommerceGlobalUseTestMode;

				if (paymentGatewayUseTestMode)
				{
					authorizeNetAPILogin = WebConfigSettings.CommerceGlobalAuthorizeNetSandboxAPILogin;
					authorizeNetAPITransactionKey = WebConfigSettings.CommerceGlobalAuthorizeNetSandboxAPITransactionKey;
				}
				else
				{
					authorizeNetAPILogin = WebConfigSettings.CommerceGlobalAuthorizeNetProductionAPILogin;
					authorizeNetAPITransactionKey = WebConfigSettings.CommerceGlobalAuthorizeNetProductionAPITransactionKey;
				}
			}
			else
			{
				SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

				if (siteSettings == null)
				{
					return;
				}

				configPrefix = "Site" + siteSettings.SiteId.ToInvariantString() + "-";

				paymentGatewayUseTestMode = Core.Configuration.ConfigHelper.GetBoolProperty(configPrefix + PaymentGatewayUseTestModeConfig, paymentGatewayUseTestMode);

				if (paymentGatewayUseTestMode)
				{
					if (ConfigurationManager.AppSettings[configPrefix + AuthorizeNetSandboxAPILoginConfig] != null)
					{
						authorizeNetAPILogin = ConfigurationManager.AppSettings[configPrefix + AuthorizeNetSandboxAPILoginConfig];
					}

					if (ConfigurationManager.AppSettings[configPrefix + AuthorizeNetSandboxAPITransactionKeyConfig] != null)
					{
						authorizeNetAPITransactionKey = ConfigurationManager.AppSettings[configPrefix + AuthorizeNetSandboxAPITransactionKeyConfig];
					}
				}
				else
				{
					if (ConfigurationManager.AppSettings[configPrefix + AuthorizeNetProductionAPILoginConfig] != null)
					{
						authorizeNetAPILogin = ConfigurationManager.AppSettings[configPrefix + AuthorizeNetProductionAPILoginConfig];
					}

					if (ConfigurationManager.AppSettings[configPrefix + AuthorizeNetProductionAPITransactionKeyConfig] != null)
					{
						authorizeNetAPITransactionKey = ConfigurationManager.AppSettings[configPrefix + AuthorizeNetProductionAPITransactionKeyConfig];
					}
				}
			}

			didLoadSettings = true;
		}

		#endregion
	}
}
