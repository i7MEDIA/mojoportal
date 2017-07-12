//  Author:                     /Huw Reddick
//  Created:                    2012-07-09
//	Last Modified:              2012-07-09
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

namespace mojoPortal.Business.WebHelpers.ProfileUpdatedHandlers
{
    /// <summary>
    ///  
    /// </summary>
    public sealed class ProfileUpdatedHandlerProviderManager
    {
        private static object initializationLock = new object();
        private static readonly ILog log
            = LogManager.GetLogger(typeof(ProfileUpdatedHandlerProviderManager));

        static ProfileUpdatedHandlerProviderManager()
        {
            Initialize();
        }

        private static void Initialize()
        {
            providerCollection = new ProfileUpdatedHandlerProviderCollection();

            try
            {
                ProfileUpdatedHandlerProviderConfig config
                    = ProfileUpdatedHandlerProviderConfig.GetConfig();

                if (config != null)
                {

                    if (
                        (config.Providers == null)
                        || (config.Providers.Count < 1)
                        )
                    {
                        throw new ProviderException("No ProfileUpdatedHandlerProviderCollection found.");
                    }

                    ProvidersHelper.InstantiateProviders(
                        config.Providers,
                        providerCollection,
                        typeof(ProfileUpdatedHandlerProvider));

                }
                else
                {
                    // config was null, not a good thing
                    log.Error("ProfileUpdatedHandlerProviderConfig could not be loaded so empty provider collection was returned");

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


        private static ProfileUpdatedHandlerProviderCollection providerCollection;

        public static ProfileUpdatedHandlerProviderCollection Providers
        {
            get
            {
                if (providerCollection == null) Initialize();
                return providerCollection;
                
            }
        }

    }
}
