using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;
using log4net;

namespace mojoPortal.Web.ExternalNewsletter
{
    public class ExternalNewsletterProviderConfig
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ExternalNewsletterProviderConfig));

        private readonly ProviderSettingsCollection providerSettingsCollection = new ProviderSettingsCollection();

        public ProviderSettingsCollection Providers => providerSettingsCollection;

        public static ExternalNewsletterProviderConfig GetConfig()
        {
            try
            {
                if (HttpRuntime.Cache["ExternalNewsletterProviderConfig"] is ExternalNewsletterProviderConfig cached)
                {
                    return cached;
                }

                ExternalNewsletterProviderConfig config = new ExternalNewsletterProviderConfig();
                string configFolderName = "~/Setup/ProviderConfig/externalnewsletter/";

                if (HttpContext.Current == null)
                {
                    return config;
                }

                string pathToConfigFolder = HttpContext.Current.Server.MapPath(configFolderName);
                if (!Directory.Exists(pathToConfigFolder))
                {
                    return config;
                }

                DirectoryInfo directoryInfo = new DirectoryInfo(pathToConfigFolder);
                FileInfo[] configFiles = directoryInfo.GetFiles("*.config");

                AggregateCacheDependency aggregateCacheDependency = new AggregateCacheDependency();

                foreach (FileInfo fileInfo in configFiles)
                {
                    var configXml = Core.Helpers.XmlHelper.GetXmlDocument(fileInfo.FullName);
                    if (configXml?.DocumentElement != null)
                    {
                        config.LoadValuesFromConfigurationXml(configXml.DocumentElement);
                    }
                    aggregateCacheDependency.Add(new CacheDependency(fileInfo.FullName));
                }

                string pathToWebConfig = HttpContext.Current.Server.MapPath("~/Web.config");
                if (File.Exists(pathToWebConfig))
                {
                    aggregateCacheDependency.Add(new CacheDependency(pathToWebConfig));
                }

                HttpRuntime.Cache.Insert(
                    "ExternalNewsletterProviderConfig",
                    config,
                    aggregateCacheDependency,
                    DateTime.Now.AddYears(1),
                    TimeSpan.Zero,
                    CacheItemPriority.Default,
                    null);

                return config;
            }
            catch (Exception ex)
            {
                log.Error("Error loading ExternalNewsletterProviderConfig", ex);
            }

            return new ExternalNewsletterProviderConfig();
        }

        public void LoadValuesFromConfigurationXml(XmlNode node)
        {
            if (node == null) return;

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "providers")
                {
                    foreach (XmlNode providerNode in child.ChildNodes)
                    {
                        if (providerNode.NodeType == XmlNodeType.Element && providerNode.Name == "add")
                        {
                            var nameAttr = providerNode.Attributes?["name"];
                            var typeAttr = providerNode.Attributes?["type"];

                            if (nameAttr != null && typeAttr != null)
                            {
                                ProviderSettings providerSettings = new ProviderSettings(nameAttr.Value, typeAttr.Value);

                                if (providerNode.Attributes != null)
                                {
                                    foreach (XmlAttribute attr in providerNode.Attributes)
                                    {
                                        if (attr.Name != "name" && attr.Name != "type")
                                        {
                                            providerSettings.Parameters.Add(attr.Name, attr.Value);
                                        }
                                    }
                                }

                                if (providerNode.HasChildNodes)
                                {
                                    providerSettings.Parameters.Add("rawConfigXml", providerNode.InnerXml);
                                }

                                providerSettingsCollection.Add(providerSettings);
                            }
                        }
                    }
                }
            }
        }
    }
}
