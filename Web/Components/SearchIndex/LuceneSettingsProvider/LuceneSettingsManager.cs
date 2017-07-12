// Author:					    
// Created:				        2013-01-11
// Last Modified:			    2013-01-11
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
using mojoPortal.Web.Framework;

namespace mojoPortal.SearchIndex
{
    public sealed class LuceneSettingsManager
    {
       
        private static bool isInitialized = false;

        static LuceneSettingsManager()
        {
            if (!isInitialized)
            {
                Initialize();
            }
        }

        private static void Initialize()
        {

            //try
            //{
            LuceneSettingsConfiguration config = LuceneSettingsConfiguration.GetConfig();

                if (
                    (config.DefaultProvider == null)
                    || (config.Providers == null)
                    || (config.Providers.Count < 1)
                    )
                {
                    throw new ProviderException("You must specify a valid default provider.");
                }


                providerCollection = new LuceneSettingsProviderCollection();

                ProvidersHelper.InstantiateProviders(
                    config.Providers, 
                    providerCollection,
                    typeof(LuceneSettingsProvider));
                
                providerCollection.SetReadOnly();
                defaultProvider = providerCollection[config.DefaultProvider];
                
            //}
            //catch (Exception ex)
            //{
            //    initializationException = ex;
            //    isInitialized = true;
            //    throw ex;
            //}

            isInitialized = true; 
        }


        private static LuceneSettingsProvider defaultProvider;
        private static LuceneSettingsProviderCollection providerCollection;

        public static LuceneSettingsProvider Provider
        {
            get
            {
                return defaultProvider;
            }
        }

        public static LuceneSettingsProviderCollection Providers
        {
            get
            {
                return providerCollection;
            }
        }

    }
}