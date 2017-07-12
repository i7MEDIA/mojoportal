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
    public class PayPalDirectPaymentGatewayProvider : PaymentGatewayProvider
    {
        private const string PaymentGatewayUseTestModeConfig = "PaymentGatewayUseTestMode";

        private const string PayPalProductionAPIUsernameConfig = "PayPalProductionAPIUsername";
        private const string PayPalProductionAPIPasswordConfig = "PayPalProductionAPIPassword";
        private const string PayPalProductionAPISignatureConfig = "PayPalProductionAPISignature";

        private const string PayPalSandboxAPIUsernameConfig = "PayPalSandboxAPIUsername";
        private const string PayPalSandboxAPIPasswordConfig = "PayPalSandboxAPIPassword";
        private const string PayPalSandboxAPISignatureConfig = "PayPalSandboxAPISignature";

        private string configPrefix = string.Empty;
        private bool paymentGatewayUseTestMode = false;
        private string payPalAPIUsername = string.Empty;
        private string payPalAPIPassword = string.Empty;
        private string payPalAPISignature = string.Empty;
        private bool didLoadSettings = false;

        public override IPaymentGateway GetPaymentGateway()
        {

            if (!didLoadSettings)
            {
                LoadSettings();
            }

            if ((payPalAPIUsername.Length > 0) && (payPalAPIPassword.Length > 0) && (payPalAPISignature.Length > 0))
            {
                PayPalDirectPaymentGateway gateway = new PayPalDirectPaymentGateway(payPalAPIUsername, payPalAPIPassword, payPalAPISignature);
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
                    payPalAPIUsername = WebConfigSettings.CommerceGlobalPayPalSandboxAPIUsername;
                    payPalAPIPassword = WebConfigSettings.CommerceGlobalPayPalSandboxAPIPassword;
                    payPalAPISignature = WebConfigSettings.CommerceGlobalPayPalSandboxAPISignature;
                }
                else
                {
                    payPalAPIUsername = WebConfigSettings.CommerceGlobalPayPalProductionAPIUsername;
                    payPalAPIPassword = WebConfigSettings.CommerceGlobalPayPalProductionAPIPassword;
                    payPalAPISignature = WebConfigSettings.CommerceGlobalPayPalProductionAPISignature;
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
                    if (ConfigurationManager.AppSettings[configPrefix + PayPalSandboxAPIUsernameConfig] != null)
                    {
                        payPalAPIUsername = ConfigurationManager.AppSettings[configPrefix + PayPalSandboxAPIUsernameConfig];

                    }

                    if (ConfigurationManager.AppSettings[configPrefix + PayPalSandboxAPIPasswordConfig] != null)
                    {
                        payPalAPIPassword = ConfigurationManager.AppSettings[configPrefix + PayPalSandboxAPIPasswordConfig];

                    }

                    if (ConfigurationManager.AppSettings[configPrefix + PayPalSandboxAPISignatureConfig] != null)
                    {
                        payPalAPISignature = ConfigurationManager.AppSettings[configPrefix + PayPalSandboxAPISignatureConfig];

                    }
                }
                else
                {
                    if (ConfigurationManager.AppSettings[configPrefix + PayPalProductionAPIUsernameConfig] != null)
                    {
                        payPalAPIUsername = ConfigurationManager.AppSettings[configPrefix + PayPalProductionAPIUsernameConfig];

                    }

                    if (ConfigurationManager.AppSettings[configPrefix + PayPalProductionAPIPasswordConfig] != null)
                    {
                        payPalAPIPassword = ConfigurationManager.AppSettings[configPrefix + PayPalProductionAPIPasswordConfig];

                    }

                    if (ConfigurationManager.AppSettings[configPrefix + PayPalProductionAPISignatureConfig] != null)
                    {
                        payPalAPISignature = ConfigurationManager.AppSettings[configPrefix + PayPalProductionAPISignatureConfig];

                    }

                }
            }

            didLoadSettings = true;
        }

    }
}