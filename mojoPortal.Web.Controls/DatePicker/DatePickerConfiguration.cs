using System;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace mojoPortal.Web.Controls.DatePicker
{
    /// <summary>
    /// Author:		        
    /// Created:            2007-11-07
    /// Last Modified:      2007-11-07
    /// 
    /// Licensed under the terms of the GNU Lesser General Public License:
    ///	http://www.opensource.org/licenses/lgpl-license.php
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// </summary>
    public class DatePickerConfiguration
    {
        private ProviderSettingsCollection providerSettingsCollection = new ProviderSettingsCollection();
        private string defaultProvider = "jsCalendarDatePickerProvider";

        public DatePickerConfiguration(XmlNode node)
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

        public static DatePickerConfiguration GetConfig()
        {
            DatePickerConfiguration datePickerConfig = null;

            if (
                (HttpRuntime.Cache["mojoDatePickerConfig"] != null)
                && (HttpRuntime.Cache["mojoDatePickerConfig"] is DatePickerConfiguration)
            )
            {
                return (DatePickerConfiguration)HttpRuntime.Cache["mojoDatePickerConfig"];
            }
            else
            {
                String configFileName = "mojoDatePicker.config";
                if (ConfigurationManager.AppSettings["mojoDatePickerConfigFileName"] != null)
                {
                    configFileName
                        = ConfigurationManager.AppSettings["mojoDatePickerConfigFileName"];
                }
 
                if (!configFileName.StartsWith("~/"))
                {
                    configFileName = "~/" + configFileName;
                }

                String pathToConfigFile 
                    = HttpContext.Current.Server.MapPath(configFileName);

                XmlDocument configXml = new XmlDocument();
                configXml.Load(pathToConfigFile);

                datePickerConfig
                    = new DatePickerConfiguration(configXml.DocumentElement);

                AggregateCacheDependency aggregateCacheDependency 
                    = new AggregateCacheDependency();

                aggregateCacheDependency.Add(new CacheDependency(pathToConfigFile));
                
                System.Web.HttpRuntime.Cache.Insert(
                    "mojoDatePickerConfig",
                    datePickerConfig,
                    aggregateCacheDependency,
                    DateTime.Now.AddYears(1),
                    TimeSpan.Zero,
                    System.Web.Caching.CacheItemPriority.Default,
                    null);

                return (DatePickerConfiguration)HttpRuntime.Cache["mojoDatePickerConfig"];

            }

            

        }


    }
}
