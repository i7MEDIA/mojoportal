// Author:              
// Created:             2006-02-06
// Last Modified:       2009-04-01

using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;
using log4net;

namespace mojoPortal.Web.Framework
{
    
    public class mojoEncryptionConfiguration
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(mojoEncryptionConfiguration));

        private XmlNode rsaNode;

        

        public void LoadValuesFromConfigurationXml(XmlNode node)
        {

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "RSAKeyValue")
                    rsaNode = child;
            }
        }

        public String RsaKey
        {
            get
            {
                if (this.rsaNode != null)
                    return this.rsaNode.OuterXml;

                return String.Empty;

            }

        }

        public static mojoEncryptionConfiguration GetConfig()
        {
            //return (mojoEncryptionConfiguration)ConfigurationManager.GetSection("system.web/mojoEncryption");

            try
            {
                if (
                    (HttpRuntime.Cache["mojoEncryptionConfiguration"] != null)
                    && (HttpRuntime.Cache["mojoEncryptionConfiguration"] is mojoEncryptionConfiguration)
                )
                {
                    return (mojoEncryptionConfiguration)HttpRuntime.Cache["mojoEncryptionConfiguration"];
                }

                var config = new mojoEncryptionConfiguration();

                

                var pathToConfigFile = System.Web.Hosting.HostingEnvironment.MapPath(Core.Configuration.ConfigHelper.GetStringProperty("mojoCryptoHelperKeyFile", "~/mojoEncryption.config"));

                log.Debug("path to crypto key " + pathToConfigFile);

                if (!File.Exists(pathToConfigFile))
                {
                    log.Error("crypto file not found " + pathToConfigFile);
                    return config;
                }

                FileInfo fileInfo = new FileInfo(pathToConfigFile);
                
				var configXml = Core.Helpers.XmlHelper.GetXmlDocument(fileInfo.FullName);

                config.LoadValuesFromConfigurationXml(configXml.DocumentElement);

                

                var aggregateCacheDependency = new AggregateCacheDependency();

               
                aggregateCacheDependency.Add(new CacheDependency(pathToConfigFile));

                System.Web.HttpRuntime.Cache.Insert(
                    "mojoEncryptionConfiguration",
                    config,
                    aggregateCacheDependency,
                    DateTime.Now.AddYears(1),
                    TimeSpan.Zero,
                    System.Web.Caching.CacheItemPriority.Default,
                    null);

                return (mojoEncryptionConfiguration)HttpRuntime.Cache["mojoEncryptionConfiguration"];

            }
            catch (HttpException ex)
            {
                log.Error(ex);

            }
            catch (System.Xml.XmlException ex)
            {
                log.Error(ex);

            }
            catch (ArgumentException ex)
            {
                log.Error(ex);

            }
            catch (NullReferenceException ex)
            {
                log.Error(ex);

            }

            return null;



        }


    }
}
