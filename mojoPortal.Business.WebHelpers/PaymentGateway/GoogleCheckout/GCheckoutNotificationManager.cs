//  Author:                     
//  Created:                    2008-06-24
//	Last Modified:              2008-06-24
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration.Provider;
using System.Web.Configuration;
using log4net;

namespace mojoPortal.Business.WebHelpers.PaymentGateway
{
    /// <summary>
    ///  
    /// </summary>
    public sealed class GCheckoutNotificationManager
    {
        private static object initializationLock = new object();
        private static readonly ILog log
            = LogManager.GetLogger(typeof(GCheckoutNotificationManager));

        static GCheckoutNotificationManager()
        {
            Initialize();
        }

        private static void Initialize()
        {
            providerCollection = new GCheckoutNotificationHandlerProviderCollection();

            try
            {
                GCheckoutNotificationConfiguration config 
                    = GCheckoutNotificationConfiguration.GetConfig();

                if (config != null)
                {

                    if (
                        (config.Providers == null)
                        || (config.Providers.Count < 1)
                        )
                    {
                        throw new ProviderException("No GCheckoutNotificationHandlerProviderCollection found.");
                    }

                    ProvidersHelper.InstantiateProviders(
                        config.Providers,
                        providerCollection,
                        typeof(GCheckoutNotificationHandlerProvider));

                }
                else
                {
                    // config was null, not a good thing
                    log.Error("GCheckoutNotificationConfiguration could not be loaded so empty provider collection was returned");

                }
            }
            catch (NullReferenceException ex)
            {
                log.Error(ex);
            }
            catch (TypeInitializationException ex)
            {
                log.Error(ex);
            }
            catch (ProviderException ex)
            {
                log.Error(ex);
            }
            
            providerCollection.SetReadOnly();
                
            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex);
            //    initializationException = ex;
            //    isInitialized = false;
            //    //throw ex;
            //    return;
            //}

            //isInitialized = true; 
        }


        private static GCheckoutNotificationHandlerProviderCollection providerCollection;

        public static GCheckoutNotificationHandlerProviderCollection Providers
        {
            get
            {
                //try
                //{
                    if (providerCollection == null) Initialize();
                    return providerCollection;
                //}
                //catch (NullReferenceException ex)
                //{
                //    log.Error(ex);  
                //}
                //catch (TypeInitializationException ex)
                //{
                //    log.Error(ex);
                //}
                //catch (ProviderException ex)
                //{
                //    log.Error(ex);  
                //}

                //return null;
            }
        }
    }
}
