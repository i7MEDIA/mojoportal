// Author:					
// Created:				    2009-12-30
// Last Modified:			2009-12-30
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

namespace mojoPortal.FileSystem
{
    public sealed class FileSystemManager
    {
        private static object initializationLock = new object();

        static FileSystemManager()
        {
            Initialize();
        }

        private static void Initialize()
        {

            
            FileSystemConfiguration config = FileSystemConfiguration.GetConfig();

                if (
                    (config.DefaultProvider == null)
                    || (config.Providers == null)
                    || (config.Providers.Count < 1)
                    )
                {
                    throw new ProviderException("You must specify a valid default provider.");
                }


                providerCollection = new FileSystemProviderCollection();

                ProvidersHelper.InstantiateProviders(
                    config.Providers, 
                    providerCollection,
                    typeof(FileSystemProvider));
                
                providerCollection.SetReadOnly();
                defaultProvider = providerCollection[config.DefaultProvider];
                
            
        }


        private static FileSystemProvider defaultProvider;
        private static FileSystemProviderCollection providerCollection;

        public static FileSystemProvider Provider
        {
            get
            {
                return defaultProvider;
            }
        }

        public static FileSystemProviderCollection Providers
        {
            get
            {
                return providerCollection;
            }
        }
    }
}
