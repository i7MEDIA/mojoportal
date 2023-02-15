// Author:					    
// Created:				        2013-01-11
// Last Modified:			    2013-01-15
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
using System.Web;
using System.Web.Caching;
using System.Xml;
using mojoPortal.Web.Framework;

namespace mojoPortal.SearchIndex
{
    public class LuceneSettingsConfiguration
    {
        

        private ProviderSettingsCollection providerSettingsCollection = new ProviderSettingsCollection();
        private string defaultProvider = "StandardAnalysisProvider";

        public LuceneSettingsConfiguration(XmlNode node)
        {
            LoadValuesFromConfigurationXml(node);
        }

        public ProviderSettingsCollection Providers
        {
            get { return providerSettingsCollection; }
        }


        public string DefaultProvider
        {
            get {return defaultProvider;}
        }

        public void LoadValuesFromConfigurationXml(XmlNode node)
        {
            if (node.Attributes["defaultProvider"] != null)
            {
                defaultProvider = node.Attributes["defaultProvider"].Value;
            }

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

        public static LuceneSettingsConfiguration GetConfig()
        {
            LuceneSettingsConfiguration config = null;

            if (
                (HttpRuntime.Cache["LuceneSettingsConfiguration"] != null)
                && (HttpRuntime.Cache["LuceneSettingsConfiguration"] is LuceneSettingsConfiguration)
            )
            {
                return (LuceneSettingsConfiguration)HttpRuntime.Cache["LuceneSettingsConfiguration"];
            }
            else
            {
                string configFileName = "LuceneSettings.config";
                if (ConfigurationManager.AppSettings["LuceneSettingsConfigFileName"] != null)
                {
                    configFileName = ConfigurationManager.AppSettings["LuceneSettingsConfigFileName"];

                }
 
                if (!configFileName.StartsWith("~/"))
                {
                    configFileName = "~/" + configFileName;
                }

                string pathToConfigFile = System.Web.Hosting.HostingEnvironment.MapPath(configFileName);

				var configXml = Core.Helpers.XmlHelper.GetXmlDocument(pathToConfigFile);

				config = new LuceneSettingsConfiguration(configXml.DocumentElement);

                AggregateCacheDependency aggregateCacheDependency = new AggregateCacheDependency();
                aggregateCacheDependency.Add(new CacheDependency(pathToConfigFile));
                
                System.Web.HttpRuntime.Cache.Insert(
                    "LuceneSettingsConfiguration",
                    config,
                    aggregateCacheDependency,
                    DateTime.Now.AddYears(1),
                    TimeSpan.Zero,
                    System.Web.Caching.CacheItemPriority.Default,
                    null);

                return (LuceneSettingsConfiguration)HttpRuntime.Cache["LuceneSettingsConfiguration"];

            }

            

        }

    }
}