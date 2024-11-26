//  Author:                     /Huw Reddick

using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;
using log4net;

namespace mojoPortal.Business.WebHelpers.ProfileUpdatedHandlers;
public class ProfileUpdatedHandlerProviderConfig
{
	private static readonly ILog log = LogManager.GetLogger(typeof(ProfileUpdatedHandlerProviderConfig));


	private ProviderSettingsCollection providerSettingsCollection = [];

	public ProviderSettingsCollection Providers => providerSettingsCollection;

	public static ProfileUpdatedHandlerProviderConfig GetConfig()
	{
		try
		{
			if (
				(HttpRuntime.Cache["ProfileUpdatedHandlerProviderConfig"] != null)
				&& (HttpRuntime.Cache["ProfileUpdatedHandlerProviderConfig"] is ProfileUpdatedHandlerProviderConfig)
			)
			{
				return (ProfileUpdatedHandlerProviderConfig)HttpRuntime.Cache["ProfileUpdatedHandlerProviderConfig"];
			}

			var config = new ProfileUpdatedHandlerProviderConfig();
			var configFolderName = "~/Setup/ProviderConfig/userprofileupdatedhandlers/";
			var pathToConfigFolder = HttpContext.Current.Server.MapPath(configFolderName);

			if (!Directory.Exists(pathToConfigFolder))
			{
				return config;
			}

			var directoryInfo = new DirectoryInfo(pathToConfigFolder);

			var configFiles = directoryInfo.GetFiles("*.config");

			foreach (FileInfo fileInfo in configFiles)
			{
				var configXml = Core.Helpers.XmlHelper.GetXmlDocument(fileInfo.FullName);
				config.LoadValuesFromConfigurationXml(configXml.DocumentElement);
			}

			var aggregateCacheDependency = new AggregateCacheDependency();

			var pathToWebConfig = HttpContext.Current.Server.MapPath("~/Web.config");

			aggregateCacheDependency.Add(new CacheDependency(pathToWebConfig));

			HttpRuntime.Cache.Insert(
				"ProfileUpdatedHandlerProviderConfig",
				config,
				aggregateCacheDependency,
				DateTime.Now.AddYears(1),
				TimeSpan.Zero,
				CacheItemPriority.Default,
				null);

			return (ProfileUpdatedHandlerProviderConfig)HttpRuntime.Cache["ProfileUpdatedHandlerProviderConfig"];

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
