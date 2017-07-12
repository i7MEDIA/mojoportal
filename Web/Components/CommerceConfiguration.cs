// Author:					
// Created:				    2008-03-06
// Last Modified:			2012-09-28
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Configuration;
using System.Globalization;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers.PaymentGateway;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Commerce;

namespace mojoPortal.Web
{
    /// <summary>
    /// 
    /// </summary>
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

        private bool is503TaxExempt = false;
        private string configPrefix = string.Empty;

        private bool paymentGatewayUseTestMode = true;

        private string primaryPaymentGateway = string.Empty;

        //private string authorizeNetProductionAPILogin = string.Empty;
        //private string authorizeNetProductionAPITransactionKey = string.Empty;

        //private string authorizeNetSandboxAPILogin = string.Empty;
        //private string authorizeNetSandboxAPITransactionKey = string.Empty;

        //// Plug N Pay
        //private string PlugNPayProductionAPIPublisherName = string.Empty;
        //private string PlugNPayProductionAPIPublisherPassword = string.Empty;

        //private string PlugNPaySandboxAPIPublisherName = string.Empty;
        //private string PlugNPaySandboxAPIPublisherPassword = string.Empty;
        //// end Plug N Pay



        // PayPal Pro
        private string payPalProductionAPIUsername = string.Empty;
        private string payPalProductionAPIPassword = string.Empty;
        private string payPalProductionAPISignature = string.Empty;

        private string payPalSandboxAPIUsername = string.Empty;
        private string payPalSandboxAPIPassword = string.Empty;
        private string payPalSandboxAPISignature = string.Empty;
        // end PayPal Pro

        //PayPal Standard
        private bool usePayPalStandard = false;
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

        private string worldPayInstallationId = string.Empty;

        private string worldPayMerchantCode = string.Empty;

        private string worldPayMd5Secret = string.Empty;

        private string worldPayResponsePassword = string.Empty;

        private bool worldPayProduceShopperResponse = true;

        private bool worldPayProduceShopperCancellationResponse = true;

        

        private string worldPayShopperResponseTemplate = "DefaultWorldPayOrderConfirmationTemplate.config";
        private string worldPayShopperCancellationResponseTemplate = "DefaultWorldPayOrderCancelledTemplate.config";

        private int defaultTimeoutInMilliseconds = 30000; // 30 seconds

        private string defaultConfirmationEmailSubjectTemplate = "DefaultOrderConfirmationEmailSubjectTemplate.config";
        private string defaultConfirmationEmailTextBodyTemplate = "DefaultOrderConfirmationPlainTextEmailTemplate.config";

        private string defaultOrderReceivedEmailSubjectTemplate = "DefaultOrderReceivedEmailSubjectTemplate.config";
        private string defaultOrderReceivedEmailTextBodyTemplate = "DefaultOrderReceivedPlainTextEmailTemplate.config";
        private string defaultEcheckOrderReceivedEmailTextBodyTemplate = "DefaultEcheckOrderReceivedPlainTextEmailTemplate.config";

        private IPaymentGateway paymentGateway = null;
        

        #endregion

        #region Public Properties

        public bool Is503TaxExempt
        {
            get { return is503TaxExempt; }
        }

        public bool PaymentGatewayUseTestMode
        {
            get { return paymentGatewayUseTestMode; }
        }

        public string WorldPayInstallationId
        {
            get { return worldPayInstallationId; }
        }

        public string WorldPayMerchantCode
        {
            get { return worldPayMerchantCode; }
        }

        public string WorldPayMd5Secret
        {
            get { return worldPayMd5Secret; }
        }

        public string WorldPayResponsePassword
        {
            get { return worldPayResponsePassword; }
        }

        public bool WorldPayProduceShopperResponse
        {
            get { return worldPayProduceShopperResponse; }
        }

        public bool WorldPayProduceShopperCancellationResponse
        {
            get { return worldPayProduceShopperCancellationResponse; }
        }

        public string WorldPayShopperResponseTemplate
        {
            get { return worldPayShopperResponseTemplate; }
        }

        public string WorldPayShopperCancellationResponseTemplate
        {
            get { return worldPayShopperCancellationResponseTemplate; }
        }

        public string PrimaryPaymentGateway
        {
            get { return primaryPaymentGateway; }
        }


        public bool IsConfigured
        {
            get
            {
                if (CanProcessStandardCards) { return true; }
                if (GoogleCheckoutIsEnabled) { return true; }
                if (PayPalIsEnabled) { return true; }

                return false;

            }
        }

        /// <summary>
        /// indicates whether we can process payments directly.
        /// true if using PayPalDirect or Authorize.NET for the primary gateway.
        /// False if not using one of these. If we process using PayPal Standard or Google Checkout,
        /// we are not processing cards directly, they are processed on external sites.
        /// </summary>
        public bool CanProcessStandardCards
        {
            get
            {
                if (paymentGateway != null) { return true; }
                
                
                return false;
            }
        }

        public bool GoogleCheckoutIsEnabled
        {
            get
            {
                if (GoogleMerchantID.Length == 0) { return false; }

                if (GoogleMerchantKey.Length == 0) { return false; }

                return true;
            }
        }

        public bool PayPalIsEnabled
        {
            get 
            {
                if (!PayPalUsePayPalStandard)
                {
                    if (PayPalAPIUsername.Length == 0) { return false; }

                    if (PayPalAPIPassword.Length == 0) { return false; }

                    if (PayPalAPISignature.Length == 0) { return false; }
                    //if((paymentGateway == null)|| (!(paymentGateway is PayPalDirectPaymentGateway))) { return false; }
                }

                if ((PayPalUsePayPalStandard) && (PayPalStandardEmailAddress.Length == 0)) { return false; }


                return true; 
            }
        }

        public bool PayPalUsePayPalStandard
        {
            get { return usePayPalStandard; }
        }

        public string PayPalStandardEmailAddress
        {
            get
            {
                if (paymentGatewayUseTestMode)
                    return payPalAccountSandboxEmailAddress;

                return payPalAccountProductionEmailAddress;
            }
        }

        public string PayPalStandardPDTId
        {
            get
            {
                if (paymentGatewayUseTestMode)
                    return payPalAccountSandboxPDTId;

                return payPalAccountProductionPDTId;
            }
        }

        public string PayPalStandardUrl
        {
            get
            {
                if (paymentGatewayUseTestMode)
                    return payPalStandardSandboxUrl;

                return payPalStandardProductionUrl;
            }
        }

        public string PayPalAPIUsername
        {
            get
            {
                if (paymentGatewayUseTestMode)
                    return payPalSandboxAPIUsername;

                return payPalProductionAPIUsername;
            }
        }

        public string PayPalAPIPassword
        {
            get
            {
                if (paymentGatewayUseTestMode)
                    return payPalSandboxAPIPassword;

                return payPalProductionAPIPassword;
            }
        }

        public string PayPalAPISignature
        {
            get
            {
                if (paymentGatewayUseTestMode)
                    return payPalSandboxAPISignature;

                return payPalProductionAPISignature;
            }
        }

        public GCheckout.EnvironmentType GoogleEnvironment
        {
            get
            {
                if (paymentGatewayUseTestMode)
                    return GCheckout.EnvironmentType.Sandbox;

                return GCheckout.EnvironmentType.Production;
            }
        }

        public string GoogleMerchantID
        {
            get
            {
                if (paymentGatewayUseTestMode)
                    return googleSandboxMerchantID;

                return googleProductionMerchantID;
            }
        }

        public string GoogleMerchantKey
        {
            get
            {
                if (paymentGatewayUseTestMode)
                    return googleSandboxMerchantKey;

                return googleProductionMerchantKey;
            }
        }

        public int DefaultTimeoutInMilliseconds
        {
            get { return defaultTimeoutInMilliseconds; }
            
        }

        public string DefaultConfirmationEmailSubjectTemplate
        {
            get { return defaultConfirmationEmailSubjectTemplate; }
        }

        public string DefaultConfirmationEmailTextBodyTemplate
        {
            get { return defaultConfirmationEmailTextBodyTemplate; }
        }

        public string DefaultOrderReceivedEmailSubjectTemplate
        {
            get { return defaultOrderReceivedEmailSubjectTemplate; }
        }

        public string DefaultOrderReceivedEmailTextBodyTemplate
        {
            get { return defaultOrderReceivedEmailTextBodyTemplate; }
        }

        public string DefaultEcheckOrderReceivedEmailTextBodyTemplate
        {
            get { return defaultEcheckOrderReceivedEmailTextBodyTemplate; }
        }


        

        #endregion

        #region Constructors

        

        public CommerceConfiguration(SiteSettings siteSettings)
        {

            configPrefix = "Site" 
                + siteSettings.SiteId.ToInvariantString()
                + "-";

            LoadConfigSettings();

            PaymentGatewayProvider gatewayProvider =  PaymentGatewayManager.Providers[PrimaryPaymentGateway];
            if (gatewayProvider != null)
            {
                paymentGateway = gatewayProvider.GetPaymentGateway();
            }


        }

        #endregion

        #region Public Methods

         public IPaymentGateway GetDirectPaymentGateway()
        {
            if (!CanProcessStandardCards) { return null; }

            return paymentGateway;

        }



        #endregion

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
            is503TaxExempt = WebConfigSettings.CommerceGlobalIs503TaxExempt;
            paymentGatewayUseTestMode = WebConfigSettings.CommerceGlobalUseTestMode;
            primaryPaymentGateway = WebConfigSettings.CommerceGlobalPrimaryGateway;

            worldPayInstallationId = WebConfigSettings.CommerceGlobalWorldPayInstallationId;
            worldPayMerchantCode = WebConfigSettings.CommerceGlobalWorldPayMerchantCode;
            worldPayMd5Secret = WebConfigSettings.CommerceGlobalWorldPayMd5Secret;
            worldPayResponsePassword = WebConfigSettings.CommerceGlobalWorldPayResponsePassword;

            if (WebConfigSettings.CommerceGlobalWorldPayShopperResponseTemplate.Length > 0)
            {
                worldPayShopperResponseTemplate = WebConfigSettings.CommerceGlobalWorldPayShopperResponseTemplate;
            }

            if (WebConfigSettings.CommerceGlobalWorldPayShopperCancellationResponseTemplate.Length > 0)
            {
                worldPayShopperCancellationResponseTemplate = WebConfigSettings.CommerceGlobalWorldPayShopperCancellationResponseTemplate;
            }

            worldPayProduceShopperResponse = WebConfigSettings.CommerceGlobalWorldPayProduceShopperResponse;
            worldPayProduceShopperCancellationResponse = WebConfigSettings.CommerceGlobalWorldPayProduceShopperCancellationResponse;
            
            payPalAccountProductionEmailAddress = WebConfigSettings.CommerceGlobalPayPalAccountProductionEmailAddress;
            payPalAccountProductionPDTId = WebConfigSettings.CommerceGlobalPayPalAccountProductionPDTId;
            payPalAccountSandboxEmailAddress = WebConfigSettings.CommerceGlobalPayPalAccountSandboxEmailAddress;
            payPalAccountSandboxPDTId = WebConfigSettings.CommerceGlobalPayPalAccountSandboxPDTId;
            usePayPalStandard = WebConfigSettings.CommerceGlobalUsePayPalStandard;
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
            is503TaxExempt
                    = ConfigHelper.GetBoolProperty(configPrefix + Is503TaxExemptSetting,
                    is503TaxExempt);

            paymentGatewayUseTestMode
                    = ConfigHelper.GetBoolProperty(configPrefix + PaymentGatewayUseTestModeConfig,
                    paymentGatewayUseTestMode);

            if (ConfigurationManager.AppSettings[configPrefix + PrimaryPaymentGatewayConfig] != null)
            {
                primaryPaymentGateway = ConfigurationManager.AppSettings[configPrefix + PrimaryPaymentGatewayConfig];

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


            usePayPalStandard = ConfigHelper.GetBoolProperty(configPrefix + PayPalUsePayPalStandardConfig, usePayPalStandard);

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

            if (ConfigurationManager.AppSettings[configPrefix + "WorldPayInstallationId"] != null)
            {
                worldPayInstallationId = ConfigurationManager.AppSettings[configPrefix + "WorldPayInstallationId"];

            }

            if (ConfigurationManager.AppSettings[configPrefix + "WorldPayMerchantCode"] != null)
            {
                worldPayMerchantCode = ConfigurationManager.AppSettings[configPrefix + "WorldPayMerchantCode"];

            }

            if (ConfigurationManager.AppSettings[configPrefix + "WorldPayMd5Secret"] != null)
            {
                worldPayMd5Secret = ConfigurationManager.AppSettings[configPrefix + "WorldPayMd5Secret"];

            }

            if (ConfigurationManager.AppSettings[configPrefix + "WorldPayResponsePassword"] != null)
            {
                worldPayResponsePassword = ConfigurationManager.AppSettings[configPrefix + "WorldPayResponsePassword"];

            }

            if (ConfigurationManager.AppSettings[configPrefix + "WorldPayShopperResponseTemplate"] != null)
            {
                worldPayShopperResponseTemplate = ConfigurationManager.AppSettings[configPrefix + "WorldPayShopperResponseTemplate"];

            }

            if (ConfigurationManager.AppSettings[configPrefix + "WorldPayShopperCancellationResponseTemplate"] != null)
            {
                worldPayShopperCancellationResponseTemplate = ConfigurationManager.AppSettings[configPrefix + "WorldPayShopperCancellationResponseTemplate"];

            }


            worldPayProduceShopperResponse = ConfigHelper.GetBoolProperty(configPrefix + "WorldPayProduceShopperResponse", worldPayProduceShopperResponse);

            worldPayProduceShopperCancellationResponse = ConfigHelper.GetBoolProperty(configPrefix + "WorldPayProduceShopperCancellationResponse", worldPayProduceShopperCancellationResponse);

            
            
            



            if (ConfigurationManager.AppSettings[configPrefix + "ConfirmationEmailPlainTextTemplate"] != null)
            {
                defaultConfirmationEmailTextBodyTemplate = ConfigurationManager.AppSettings[configPrefix + "ConfirmationEmailPlainTextTemplate"];

            }



            if (ConfigurationManager.AppSettings[configPrefix + "ConfirmationEmailSubjectTemplate"] != null)
            {
                defaultConfirmationEmailSubjectTemplate = ConfigurationManager.AppSettings[configPrefix + "ConfirmationEmailSubjectTemplate"];

            }

            if (ConfigurationManager.AppSettings[configPrefix + "OrderReceivedEmailPlainTextTemplate"] != null)
            {
                defaultOrderReceivedEmailTextBodyTemplate = ConfigurationManager.AppSettings[configPrefix + "OrderReceivedEmailPlainTextTemplate"];

            }

            if (ConfigurationManager.AppSettings[configPrefix + "OrderReceivedEmailSubjectTemplate"] != null)
            {
                defaultOrderReceivedEmailSubjectTemplate = ConfigurationManager.AppSettings[configPrefix + "OrderReceivedEmailSubjectTemplate"];

            }

            if (ConfigurationManager.AppSettings[configPrefix + "EcheckOrderReceivedEmailPlainTextTemplate"] != null)
            {
                defaultEcheckOrderReceivedEmailTextBodyTemplate = ConfigurationManager.AppSettings[configPrefix + "EcheckOrderReceivedEmailPlainTextTemplate"];

            }

        }


        

        //public static string GetPaymentGatewayTestModeKey(SiteSettings siteSettings)
        //{
        //    return "Site" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)
        //        + "-" + PaymentGatewayUseTestModeConfig;
        //}

    }
}
