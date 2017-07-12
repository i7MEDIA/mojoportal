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

using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;

namespace mojoPortal.Web.Commerce
{
    public sealed class PaymentGatewayManager
    {
        private static object initializationLock = new object();

        static PaymentGatewayManager()
        {
            Initialize();
        }

        private static void Initialize()
        {


            PaymentGatewayConfiguration config = PaymentGatewayConfiguration.GetConfig();

                if (
                    (config.DefaultProvider == null)
                    || (config.Providers == null)
                    || (config.Providers.Count < 1)
                    )
                {
                    throw new ProviderException("You must specify a valid default provider.");
                }


                providerCollection = new PaymentGatewayProviderCollection();

                ProvidersHelper.InstantiateProviders(
                    config.Providers, 
                    providerCollection,
                    typeof(PaymentGatewayProvider));
                
                providerCollection.SetReadOnly();
                defaultProvider = providerCollection[config.DefaultProvider];
                
            
        }


        private static PaymentGatewayProvider defaultProvider;
        private static PaymentGatewayProviderCollection providerCollection;

        public static PaymentGatewayProvider Provider
        {
            get
            {
                return defaultProvider;
            }
        }

        public static PaymentGatewayProviderCollection Providers
        {
            get
            {
                return providerCollection;
            }
        }

    }
}