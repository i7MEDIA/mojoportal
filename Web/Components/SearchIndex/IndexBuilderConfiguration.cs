//  Author:                     
//  Created:                    2007-08-30
//	Last Modified:              2013-01-15
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
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Caching;
using System.Xml;
using log4net;

namespace mojoPortal.SearchIndex
{
    /// <summary>
    ///  
    /// </summary>
    public class IndexBuilderConfiguration
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(IndexBuilderConfiguration));


        private ProviderSettingsCollection providerSettingsCollection 
            = new ProviderSettingsCollection();

        public ProviderSettingsCollection Providers
        {
            get { return providerSettingsCollection; }
        }

        public static IndexBuilderConfiguration GetConfig()
        {
            try
            {
                if (
                    (HttpRuntime.Cache["mojoIndexBuilderConfiguration"] != null)
                    && (HttpRuntime.Cache["mojoIndexBuilderConfiguration"] is IndexBuilderConfiguration)
                )
                {
                    return (IndexBuilderConfiguration)HttpRuntime.Cache["mojoIndexBuilderConfiguration"];
                }

                IndexBuilderConfiguration indexBuilderConfig
                    = new IndexBuilderConfiguration();

                String configFolderName = "~/Setup/ProviderConfig/indexbuilders/";
                
                string pathToConfigFolder
                    = HostingEnvironment.MapPath(configFolderName);


                if (!Directory.Exists(pathToConfigFolder)) return indexBuilderConfig;

                DirectoryInfo directoryInfo
                    = new DirectoryInfo(pathToConfigFolder);

                FileInfo[] configFiles = directoryInfo.GetFiles("*.config");

                foreach (FileInfo fileInfo in configFiles)
                {
					var configXml = Core.Helpers.XmlHelper.GetXmlDocument(fileInfo.FullName);
					indexBuilderConfig.LoadValuesFromConfigurationXml(configXml.DocumentElement);

                }

                AggregateCacheDependency aggregateCacheDependency
                    = new AggregateCacheDependency();

                string pathToWebConfig
                    = HostingEnvironment.MapPath("~/Web.config");

                aggregateCacheDependency.Add(new CacheDependency(pathToWebConfig));

                System.Web.HttpRuntime.Cache.Insert(
                    "mojoIndexBuilderConfiguration",
                    indexBuilderConfig,
                    aggregateCacheDependency,
                    DateTime.Now.AddYears(1),
                    TimeSpan.Zero,
                    System.Web.Caching.CacheItemPriority.Default,
                    null);

                return (IndexBuilderConfiguration)HttpRuntime.Cache["mojoIndexBuilderConfiguration"];

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

        public void LoadValuesFromConfigurationXml(XmlNode node)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "providers")
                {
                    foreach (XmlNode providerNode in child.ChildNodes)
                    {
                        if (
                            (providerNode.NodeType == XmlNodeType.Element)
                            && (providerNode.Name == "add")
                            )
                        {
                            if (
                                (providerNode.Attributes["name"] != null)
                                && (providerNode.Attributes["type"] != null)
                                )
                            {
                                ProviderSettings providerSettings
                                    = new ProviderSettings(
                                    providerNode.Attributes["name"].Value,
                                    providerNode.Attributes["type"].Value);

                                providerSettingsCollection.Add(providerSettings);
                            }

                        }
                    }

                }
            }
        }


    }
}
