using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;
using System.Web.UI;

namespace mojoPortal.Web.Editor
{
    /// <summary>
    /// Author:		        
    /// Created:            2007/05/18
    /// Last Modified:      2007/05/30
    ///
    /// Licensed under the terms of the GNU Lesser General Public License:
    ///	http://www.opensource.org/licenses/lgpl-license.php
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// </summary>
    public sealed class EditorManager
    {
        //private static bool isInitialized = false;
        //private static Exception initializationException;
        private static object initializationLock = new object();

        static EditorManager()
        {
            Initialize();
        }

        private static void Initialize()
        {

            //try
            //{
                EditorConfiguration editorConfig = EditorConfiguration.GetConfig();

                if (
                    (editorConfig.DefaultProvider == null)
                    || (editorConfig.Providers == null)
                    || (editorConfig.Providers.Count < 1)
                    )
                {
                    throw new ProviderException("You must specify a valid default provider.");
                }

 
                providerCollection = new EditorProviderCollection();

                ProvidersHelper.InstantiateProviders(
                    editorConfig.Providers, 
                    providerCollection, 
                    typeof(EditorProvider));
                
                providerCollection.SetReadOnly();
                defaultProvider = providerCollection[editorConfig.DefaultProvider];
                
            //}
            //catch (Exception ex)
            //{
            //    initializationException = ex;
            //    isInitialized = true;
            //    throw ex;
            //}

            //isInitialized = true; 
        }

 
        private static EditorProvider defaultProvider;
        private static EditorProviderCollection providerCollection;

        public static EditorProvider Provider
        {
            get
            {
                return defaultProvider;
            }
        }

        public static EditorProviderCollection Providers
        {
            get
            {
                return providerCollection;
            }
        }


    }
}
