//  Author:                     
//  Created:                    2009-06-21
//	Last Modified:              2009-06-21
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

namespace mojoPortal.Business.WebHelpers
{
    public sealed class ContentDeleteHandlerProviderManager
    {
        private static object initializationLock = new object();
        private static readonly ILog log = LogManager.GetLogger(typeof(ContentDeleteHandlerProviderManager));

        static ContentDeleteHandlerProviderManager()
        {
            Initialize();
        }

        private static void Initialize()
        {
            providerCollection = new ContentDeleteHandlerProviderCollection();

            try
            {
                ContentDeleteHandlerProviderConfig config
                    = ContentDeleteHandlerProviderConfig.GetConfig();

                if (config != null)
                {

                    if (
                        (config.Providers == null)
                        || (config.Providers.Count < 1)
                        )
                    {
                        throw new ProviderException("No ContentDeleteHandlerProviderCollection found.");
                    }

                    ProvidersHelper.InstantiateProviders(
                        config.Providers,
                        providerCollection,
                        typeof(ContentDeleteHandlerProvider));

                }
                else
                {
                    // config was null, not a good thing
                    log.Error("ContentDeleteHandlerProviderCollection could not be loaded so empty provider collection was returned");

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


        }


        private static ContentDeleteHandlerProviderCollection providerCollection;

        public static ContentDeleteHandlerProviderCollection Providers
        {
            get
            {
                if (providerCollection == null) Initialize();
                return providerCollection;

            }
        }

    }
}
