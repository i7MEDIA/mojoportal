using log4net;
using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace mojoPortal.Business.WebHelpers.SiteCreatedEventHandlers;

public class SiteCreatedEventHandlerProviderConfig
{
	private static readonly ILog log = LogManager.GetLogger(typeof(SiteCreatedEventHandlerProviderConfig));

	private readonly ProviderSettingsCollection providerSettingsCollection = [];


	public ProviderSettingsCollection Providers => providerSettingsCollection;


	public static SiteCreatedEventHandlerProviderConfig GetConfig()
	{
		try
		{
			if (HttpRuntime.Cache["SiteCreatedEventHandlerProviderConfig"] is SiteCreatedEventHandlerProviderConfig config)
			{
				return config;
			}

			config = new SiteCreatedEventHandlerProviderConfig();

			var configFolderName = "~/Setup/ProviderConfig/sitecreatedeventhandlers/";
			var pathToConfigFolder = HttpContext.Current.Server.MapPath(configFolderName);

			if (!Directory.Exists(pathToConfigFolder))
			{
				return config;
			}

			var directoryInfo = new DirectoryInfo(pathToConfigFolder);
			var configFiles = directoryInfo.GetFiles("*.config");

			foreach (var fileInfo in configFiles)
			{
				var configXml = Core.Helpers.XmlHelper.GetXmlDocument(fileInfo.FullName);

				config.LoadValuesFromConfigurationXml(configXml.DocumentElement);
			}

			var aggregateCacheDependency = new AggregateCacheDependency();
			var pathToWebConfig = HttpContext.Current.Server.MapPath("~/Web.config");

			aggregateCacheDependency.Add(new CacheDependency(pathToWebConfig));

			HttpRuntime.Cache.Insert(
				"SiteCreatedEventHandlerProviderConfig",
				config,
				aggregateCacheDependency,
				DateTime.Now.AddYears(1),
				TimeSpan.Zero,
				CacheItemPriority.Default,
				null
			);

			return (SiteCreatedEventHandlerProviderConfig)HttpRuntime.Cache["SiteCreatedEventHandlerProviderConfig"];
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
					if (
						providerNode.NodeType == XmlNodeType.Element &&
						providerNode.Name == "add"
					)
					{
						if (
							providerNode.Attributes["name"] != null &&
							providerNode.Attributes["type"] != null
						)
						{
							var providerSettings = new ProviderSettings(
								providerNode.Attributes["name"].Value,
								providerNode.Attributes["type"].Value
							);

							providerSettingsCollection.Add(providerSettings);
						}
					}
				}
			}
		}
	}
}
