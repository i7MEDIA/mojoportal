using System;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace mojoPortal.Web.Controls.Captcha
{
    /// <summary>
    /// Author:		        
    /// Created:            2007-08-15
    /// Last Modified:      2007-08-15
    /// 
    /// Licensed under the terms of the GNU Lesser General Public License:
    ///	http://www.opensource.org/licenses/lgpl-license.php
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// </summary>
    public class CaptchaConfiguration
    {
        private ProviderSettingsCollection providerSettingsCollection = new ProviderSettingsCollection();
        private string defaultProvider = "SimpleMathCaptchaProvider";

        public CaptchaConfiguration(XmlNode node)
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

        public static CaptchaConfiguration GetConfig()
        {
            CaptchaConfiguration captchaConfig = null;

            if (
                (HttpRuntime.Cache["mojoCaptchaConfig"] != null)
                && (HttpRuntime.Cache["mojoCaptchaConfig"] is CaptchaConfiguration)
            )
            {
                return (CaptchaConfiguration)HttpRuntime.Cache["mojoCaptchaConfig"];
            }
            else
            {
                String configFileName = "mojoCaptcha.config";
                if (ConfigurationManager.AppSettings["mojoCaptchaConfigFileName"] != null)
                {
                    configFileName 
                        = ConfigurationManager.AppSettings["mojoCaptchaConfigFileName"];
                }
 
                if (!configFileName.StartsWith("~/"))
                {
                    configFileName = "~/" + configFileName;
                }

                String pathToConfigFile 
                    = HttpContext.Current.Server.MapPath(configFileName);

                XmlDocument configXml = new XmlDocument();
                configXml.Load(pathToConfigFile);
                captchaConfig 
                    = new CaptchaConfiguration(configXml.DocumentElement);

                AggregateCacheDependency aggregateCacheDependency 
                    = new AggregateCacheDependency();

                aggregateCacheDependency.Add(new CacheDependency(pathToConfigFile));
                
                System.Web.HttpRuntime.Cache.Insert(
                    "mojoCaptchaConfig",
                    captchaConfig,
                    aggregateCacheDependency,
                    DateTime.Now.AddYears(1),
                    TimeSpan.Zero,
                    System.Web.Caching.CacheItemPriority.Default,
                    null);

                return (CaptchaConfiguration)HttpRuntime.Cache["mojoCaptchaConfig"];

            }

            //return captchaConfig;

        }
    }
}
