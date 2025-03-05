using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Caching;
using System.Xml;
using log4net;

namespace mojoPortal.SearchIndex;

public class IndexBuilderConfiguration
{
	private static readonly ILog log = LogManager.GetLogger(typeof(IndexBuilderConfiguration));

	private ProviderSettingsCollection providerSettingsCollection = [];

	public ProviderSettingsCollection Providers => providerSettingsCollection;

	public static IndexBuilderConfiguration GetConfig()
	{
		try
		{
			if (HttpRuntime.Cache["mojoIndexBuilderConfiguration"] is not null and IndexBuilderConfiguration)
			{
				return (IndexBuilderConfiguration)HttpRuntime.Cache["mojoIndexBuilderConfiguration"];
			}

			var indexBuilderConfig = new IndexBuilderConfiguration();

			var configFolderName = "~/Setup/ProviderConfig/indexbuilders/";

			var pathToConfigFolder = HostingEnvironment.MapPath(configFolderName);

			if (!Directory.Exists(pathToConfigFolder))
			{
				return indexBuilderConfig;
			}

			var directoryInfo = new DirectoryInfo(pathToConfigFolder);

			var configFiles = directoryInfo.GetFiles("*.config");

			foreach (var fileInfo in configFiles)
			{
				var configXml = XmlHelper.GetXmlDocument(fileInfo.FullName);
				indexBuilderConfig.LoadValuesFromConfigurationXml(configXml.DocumentElement);
			}

			var aggregateCacheDependency = new AggregateCacheDependency();

			string pathToWebConfig = HostingEnvironment.MapPath("~/Web.config");

			aggregateCacheDependency.Add(new CacheDependency(pathToWebConfig));

			HttpRuntime.Cache.Insert(
				"mojoIndexBuilderConfiguration",
				indexBuilderConfig,
				aggregateCacheDependency,
				DateTime.Now.AddYears(1),
				TimeSpan.Zero,
				CacheItemPriority.Default,
				null);

			return (IndexBuilderConfiguration)HttpRuntime.Cache["mojoIndexBuilderConfiguration"];

		}
		catch (HttpException ex)
		{
			log.Error(ex);

		}
		catch (XmlException ex)
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
								providerNode.Attributes["type"].Value);

							providerSettingsCollection.Add(providerSettings);
						}
					}
				}
			}
		}
	}
}
