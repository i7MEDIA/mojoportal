using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;
using System.Web.UI;

namespace mojoPortal.Web.Controls.DatePicker
{
    /// <summary>
    /// Author:		        
    /// Created:            2007-11-07
    /// Last Modified:      2007-11-07
    /// 
    /// Licensed under the terms of the GNU Lesser General Public License:
    ///	http://www.opensource.org/licenses/lgpl-license.php
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// </summary>
    public sealed class DatePickerManager
    {
        //private static bool isInitialized = false;
        //private static Exception initializationException;
        private static object initializationLock = new object();

        static DatePickerManager()
        {
            Initialize();
        }

        private static void Initialize()
        {

            //try
            //{
                DatePickerConfiguration config = DatePickerConfiguration.GetConfig();

                if (
                    (config.DefaultProvider == null)
                    || (config.Providers == null)
                    || (config.Providers.Count < 1)
                    )
                {
                    throw new ProviderException("You must specify a valid default provider.");
                }


                providerCollection = new DatePickerProviderCollection();

                ProvidersHelper.InstantiateProviders(
                    config.Providers, 
                    providerCollection,
                    typeof(DatePickerProvider));
                
                providerCollection.SetReadOnly();
                defaultProvider = providerCollection[config.DefaultProvider];
                
            //}
            //catch (Exception ex)
            //{
            //    initializationException = ex;
            //    //isInitialized = true;
            //    throw ex;
            //}

            //isInitialized = true; 
        }


        private static DatePickerProvider defaultProvider;
        private static DatePickerProviderCollection providerCollection;

        public static DatePickerProvider Provider
        {
            get
            {
                return defaultProvider;
            }
        }

        public static DatePickerProviderCollection Providers
        {
            get
            {
                return providerCollection;
            }
        }

    }
}
