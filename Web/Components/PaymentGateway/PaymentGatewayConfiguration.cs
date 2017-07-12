// Author:					
// Created:				    2012-01-09
// Last Modified:			2012-01-09
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
using mojoPortal.Web;

namespace mojoPortal.Web.Commerce
{
    public class PaymentGatewayConfiguration
    {
        private ProviderSettingsCollection providerSettingsCollection = new ProviderSettingsCollection();
        private string defaultProvider = "NotImplementedPaymentGatewayProvider";

        public PaymentGatewayConfiguration(XmlNode node)
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

        public static PaymentGatewayConfiguration GetConfig()
        {
            PaymentGatewayConfiguration config = null;

            if (
                (HttpRuntime.Cache["PaymentGatewayConfiguration"] != null)
                && (HttpRuntime.Cache["PaymentGatewayConfiguration"] is PaymentGatewayConfiguration)
            )
            {
                return (PaymentGatewayConfiguration)HttpRuntime.Cache["PaymentGatewayConfiguration"];
            }
            else
            {

                String configFileName = WebConfigSettings.PaymentGatewayConfigFileName;
                
 
                if (!configFileName.StartsWith("~/"))
                {
                    configFileName = "~/" + configFileName;
                }

                String pathToConfigFile = HttpContext.Current.Server.MapPath(configFileName);

                XmlDocument configXml = new XmlDocument();
                configXml.Load(pathToConfigFile);
                config = new PaymentGatewayConfiguration(configXml.DocumentElement);

                AggregateCacheDependency aggregateCacheDependency = new AggregateCacheDependency();
                aggregateCacheDependency.Add(new CacheDependency(pathToConfigFile));
                
                System.Web.HttpRuntime.Cache.Insert(
                    "PaymentGatewayConfiguration",
                    config,
                    aggregateCacheDependency,
                    DateTime.Now.AddYears(1),
                    TimeSpan.Zero,
                    System.Web.Caching.CacheItemPriority.Default,
                    null);

                return (PaymentGatewayConfiguration)HttpRuntime.Cache["PaymentGatewayConfiguration"];

            }

            

        }

    }
}