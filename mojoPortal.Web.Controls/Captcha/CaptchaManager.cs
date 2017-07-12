using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;
using System.Web.UI;

namespace mojoPortal.Web.Controls.Captcha
{
    /// <summary>
    /// Author:		        
    /// Created:            2007-08-15
    /// Last Modified:      2007-08-15
    /// 
    /// Licensed under the terms of the GNU Lesser General Public License:
    ///	http://www.opensource.org/licenses/lgpl-license.php
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// </summary>
    public sealed class CaptchaManager
    {
        //private static bool isInitialized = false;
        //private static Exception initializationException;
        private static object initializationLock = new object();

        static CaptchaManager()
        {
            Initialize();
        }

        private static void Initialize()
        {

            //try
            //{
                CaptchaConfiguration config = CaptchaConfiguration.GetConfig();

                if (
                    (config.DefaultProvider == null)
                    || (config.Providers == null)
                    || (config.Providers.Count < 1)
                    )
                {
                    throw new ProviderException("You must specify a valid default provider.");
                }

 
                providerCollection = new CaptchaProviderCollection();

                ProvidersHelper.InstantiateProviders(
                    config.Providers, 
                    providerCollection, 
                    typeof(CaptchaProvider));
                
                providerCollection.SetReadOnly();
                defaultProvider = providerCollection[config.DefaultProvider];
                
            //}
            //catch (Exception ex)
            //{
            //    initializationException = ex;
            //    isInitialized = true;
            //    throw ex;
            //}

            //isInitialized = true; 
        }

 
        private static CaptchaProvider defaultProvider;
        private static CaptchaProviderCollection providerCollection;

        public static CaptchaProvider Provider
        {
            get
            {
                return defaultProvider;
            }
        }

        public static CaptchaProviderCollection Providers
        {
            get
            {
                return providerCollection;
            }
        }
    }
}
