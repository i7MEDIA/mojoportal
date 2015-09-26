

using System;
using System.Collections.Generic;
using System.Security;
using log4net;
using Microsoft.ApplicationServer.Caching;
using mojoPortal.Web;


namespace mojoPortal.Web.Caching
{
    public class AppFabricCacheFactory: DistributedCacheFactoryBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AppFabricCacheFactory));
        private static bool debugLog = log.IsDebugEnabled;
        private const string DEFAULT_EndpointConfig = "localhost:22233";

        // Getting started
        //http://msdn.microsoft.com/en-us/library/windowsazure/gg278356.aspx
        // Create a Cache
        //http://msdn.microsoft.com/en-us/library/windowsazure/gg618004.aspx
        // Managing caches
        //http://msdn.microsoft.com/en-us/library/windowsazure/gg618005.aspx
        //Understanding Cache Quotas
        //http://msdn.microsoft.com/en-us/library/windowsazure/gg185683.aspx
        // Hanselman
        //http://www.hanselman.com/blog/InstallingConfiguringAndUsingWindowsServerAppFabricAndTheVelocityMemoryCacheIn10Minutes.aspx

        public AppFabricCacheFactory()
        {
            
        }
        

        public DataCache ConstructCache(string endPointConfig)
        {
#if NET35
            if (string.IsNullOrEmpty(endPointConfig))
                endPointConfig = DEFAULT_EndpointConfig;
#else
            if (string.IsNullOrWhiteSpace(endPointConfig))
                endPointConfig = DEFAULT_EndpointConfig;
#endif

            var endPoints = ParseConfig(endPointConfig);
			var dataCacheEndpoints = new List<DataCacheServerEndpoint>();
			endPoints.ForEach(e => dataCacheEndpoints.Add(new DataCacheServerEndpoint(e.IPAddressOrHostName,e.Port)));

            var config = new DataCacheFactoryConfiguration();
            config.Servers = dataCacheEndpoints;
            SetSecuritySettings(config);
            

            try
            {
                var factory = new DataCacheFactory(config);
                DataCacheClientLogManager.ChangeLogLevel(System.Diagnostics.TraceLevel.Error);

				// Note: When setting up AppFabric. The configured cache needs to be created by the admin using the New-Cache powershell command
            	//var cache = factory.GetCache(WebConfigSettings.DistributedCacheName);
                var cache = factory.GetDefaultCache();
                return cache;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            
        }

        private void SetSecuritySettings(DataCacheFactoryConfiguration config)
        {
            string securityModeValue = WebConfigSettings.AzureCacheSecurityMode.ToLowerInvariant();
            string securityAuthValue = WebConfigSettings.AzureCacheAuthorizationInfo;
            bool useSsl = WebConfigSettings.AzureCacheUseSsl;
            DataCacheSecurity securityProps;

            switch (securityModeValue)
            {
                case "message":

                    if (securityAuthValue.Length > 0)
                    {
                        SecureString secureToken = new SecureString();
                        foreach (var ch in securityAuthValue)
                        {
                            secureToken.AppendChar(ch);
                        }

                        securityProps = new DataCacheSecurity(secureToken, useSsl);
                        config.SecurityProperties = securityProps;
                    }

                    break;

                case "transport":

                    securityProps = new DataCacheSecurity(DataCacheSecurityMode.Transport,DataCacheProtectionLevel.None);
					config.SecurityProperties = securityProps;

                    break;

            }


            
            
        }

    }
}
