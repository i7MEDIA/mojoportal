//  Author:                     
//  Created:                    2007-08-30
//	Last Modified:              2008-03-24
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

namespace mojoPortal.SearchIndex
{
    /// <summary>
    ///  
    /// </summary>
    public sealed class IndexBuilderManager
    {
        //private static bool isInitialized = false;
        //private static Exception initializationException;
        private static object initializationLock = new object();
        private static readonly ILog log
            = LogManager.GetLogger(typeof(IndexBuilderManager));

        static IndexBuilderManager()
        {
            Initialize();
        }

        private static void Initialize()
        {
            providerCollection = new IndexBuilderProviderCollection();

            try
            {
                IndexBuilderConfiguration config 
                    = IndexBuilderConfiguration.GetConfig();

                if (config != null)
                {

                    if (
                        (config.Providers == null)
                        || (config.Providers.Count < 1)
                        )
                    {
                        throw new ProviderException("No IndexBuilderProvider found.");
                    }

                    ProvidersHelper.InstantiateProviders(
                        config.Providers,
                        providerCollection,
                        typeof(IndexBuilderProvider));

                }
                else
                {
                    // config was null, not a good thing
                    log.Error("IndexBuilderConfiguration could not be loaded so empty provider collection was returned");

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


        private static IndexBuilderProviderCollection providerCollection;

        public static IndexBuilderProviderCollection Providers
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
