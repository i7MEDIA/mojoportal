using System;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace mojoPortal.SearchIndex;
public class LuceneSettingsConfiguration
{
	public LuceneSettingsConfiguration(XmlNode node) => LoadValuesFromConfigurationXml(node);

	public ProviderSettingsCollection Providers { get; } = [];


	public string DefaultProvider { get; private set; } = "StandardAnalysisProvider";

	public void LoadValuesFromConfigurationXml(XmlNode node)
	{
		if (node.Attributes["defaultProvider"] != null)
		{
			DefaultProvider = node.Attributes["defaultProvider"].Value;
		}

		foreach (XmlNode child in node.ChildNodes)
		{
			if (child.Name == "providers")
			{
				foreach (XmlNode providerNode in child.ChildNodes)
				{
					if (providerNode.NodeType == XmlNodeType.Element
						&& providerNode.Name == "add"
						)
					{
						if (providerNode.Attributes["name"] != null
							&& providerNode.Attributes["type"] != null
							)
						{
							var providerSettings = new ProviderSettings(
								providerNode.Attributes["name"].Value,
								providerNode.Attributes["type"].Value
							);

							Providers.Add(providerSettings);
						}
					}
				}
			}
		}
	}

	public static LuceneSettingsConfiguration GetConfig()
	{
		LuceneSettingsConfiguration config = null;

		if (HttpRuntime.Cache["LuceneSettingsConfiguration"] != null
			&& HttpRuntime.Cache["LuceneSettingsConfiguration"] is LuceneSettingsConfiguration
		)
		{
			return (LuceneSettingsConfiguration)HttpRuntime.Cache["LuceneSettingsConfiguration"];
		}
		else
		{
			var configFileName = "LuceneSettings.config";
			if (ConfigurationManager.AppSettings["LuceneSettingsConfigFileName"] != null)
			{
				configFileName = ConfigurationManager.AppSettings["LuceneSettingsConfigFileName"];
			}

			if (!configFileName.StartsWith("~/"))
			{
				configFileName = $"~/{configFileName}";
			}

			var pathToConfigFile = System.Web.Hosting.HostingEnvironment.MapPath(configFileName);

			var configXml = XmlHelper.GetXmlDocument(pathToConfigFile);

			config = new LuceneSettingsConfiguration(configXml.DocumentElement);

			var aggregateCacheDependency = new AggregateCacheDependency();
			aggregateCacheDependency.Add(new CacheDependency(pathToConfigFile));

			HttpRuntime.Cache.Insert(
				"LuceneSettingsConfiguration",
				config,
				aggregateCacheDependency,
				DateTime.Now.AddYears(1),
				TimeSpan.Zero,
				CacheItemPriority.Default,
				null);

			return (LuceneSettingsConfiguration)HttpRuntime.Cache["LuceneSettingsConfiguration"];
		}
	}
}
