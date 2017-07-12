// Author:					
// Created:				    2012-01-09
// Last Modified:			2012-01-09
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Configuration;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.PaymentGateway;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.Commerce
{
    public class AuthorizeNetPaymentGatewayProvider : PaymentGatewayProvider
    {
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

        public override IPaymentGateway GetPaymentGateway()
        {
            
            if(!didLoadSettings)
            {
                LoadSettings();
            }
            
            if((authorizeNetAPILogin.Length > 0)&&(authorizeNetAPITransactionKey.Length > 0))
            {
                AuthorizeNETPaymentGateway gateway = new AuthorizeNETPaymentGateway(authorizeNetAPILogin, authorizeNetAPITransactionKey);
                gateway.UseTestMode = paymentGatewayUseTestMode;
                return gateway;

            }

            return null;
        }

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
                if (siteSettings == null)  { return;  }

                configPrefix = "Site" + siteSettings.SiteId.ToInvariantString() + "-";

                paymentGatewayUseTestMode
                    = ConfigHelper.GetBoolProperty(configPrefix + PaymentGatewayUseTestModeConfig,
                    paymentGatewayUseTestMode);

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
    }
}