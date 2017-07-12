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
    public class PlugNPayPaymentGatewayProvider : PaymentGatewayProvider
    {
        private const string PaymentGatewayUseTestModeConfig = "PaymentGatewayUseTestMode";

        private const string PlugNPayProductionAPIPublisherNameConfig = "PlugNPayProductionAPIPublisherName";
        private const string PlugNPayProductionAPIPublisherPasswordConfig = "PlugNPayProductionAPIPublisherPassword";

        private const string PlugNPaySandboxAPIPublisherNameConfig = "PlugNPaySandboxAPIPublisherName";
        private const string PlugNPaySandboxAPIPublisherPasswordConfig = "PlugNPaySandboxAPIPublisherPassword";

        private string configPrefix = string.Empty;
        private bool paymentGatewayUseTestMode = false;
        private string plugNPayAPIPublisherName = string.Empty;
        private string plugNPayAPIPublisherPassword = string.Empty;
        private bool didLoadSettings = false;

        public override IPaymentGateway GetPaymentGateway()
        {
            
            if(!didLoadSettings)
            {
                LoadSettings();
            }

            if ((plugNPayAPIPublisherName.Length > 0) && (plugNPayAPIPublisherPassword.Length > 0))
            {
                PlugNPayPaymentGateway gateway = new PlugNPayPaymentGateway(plugNPayAPIPublisherName, plugNPayAPIPublisherPassword);
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
                    plugNPayAPIPublisherName = WebConfigSettings.CommerceGlobalPlugNPaySandboxAPIPublisherName;
                    plugNPayAPIPublisherPassword = WebConfigSettings.CommerceGlobalPlugNPaySandboxAPIPublisherPassword;
                }
                else
                {
                    plugNPayAPIPublisherName = WebConfigSettings.CommerceGlobalPlugNPayProductionAPIPublisherName;
                    plugNPayAPIPublisherPassword = WebConfigSettings.CommerceGlobalPlugNPayProductionAPIPublisherPassword;
                }

            }
            else
            {
                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                if (siteSettings == null) { return; }

                configPrefix = "Site" + siteSettings.SiteId.ToInvariantString() + "-";

                paymentGatewayUseTestMode
                    = ConfigHelper.GetBoolProperty(configPrefix + PaymentGatewayUseTestModeConfig,
                    paymentGatewayUseTestMode);

                if (paymentGatewayUseTestMode)
                {
                    if (ConfigurationManager.AppSettings[configPrefix + PlugNPaySandboxAPIPublisherNameConfig] != null)
                    {
                        plugNPayAPIPublisherName = ConfigurationManager.AppSettings[configPrefix + PlugNPaySandboxAPIPublisherNameConfig];

                    }

                    if (ConfigurationManager.AppSettings[configPrefix + PlugNPaySandboxAPIPublisherPasswordConfig] != null)
                    {
                        plugNPayAPIPublisherPassword = ConfigurationManager.AppSettings[configPrefix + PlugNPaySandboxAPIPublisherPasswordConfig];

                    }
                }
                else
                {
                    if (ConfigurationManager.AppSettings[configPrefix + PlugNPayProductionAPIPublisherNameConfig] != null)
                    {
                        plugNPayAPIPublisherName = ConfigurationManager.AppSettings[configPrefix + PlugNPayProductionAPIPublisherNameConfig];

                    }

                    if (ConfigurationManager.AppSettings[configPrefix + PlugNPayProductionAPIPublisherPasswordConfig] != null)
                    {
                        plugNPayAPIPublisherPassword = ConfigurationManager.AppSettings[configPrefix + PlugNPayProductionAPIPublisherPasswordConfig];

                    }

                }
            }

            didLoadSettings = true;
        }
    }
}