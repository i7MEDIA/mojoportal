using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Caching;

namespace mojoPortal.Web;

public class ContentAdminLinksConfiguration
{
	public List<ContentAdminLink> AdminLinks { get; } = [];


	public static ContentAdminLinksConfiguration GetConfig(int siteId)
	{
		var cacheKey = $"ContentAdminLinksConfiguration-{siteId}";

		if (HttpRuntime.Cache[cacheKey] is ContentAdminLinksConfiguration configuration)
		{
			return configuration;
		}
		else
		{
			var config = new ContentAdminLinksConfiguration();
			var configFolderName = "~/Setup/initialcontent/supplementaladminmenulinks";
			var pathToConfigFolder = HttpContext.Current.Server.MapPath(configFolderName);

			if (!Directory.Exists(pathToConfigFolder))
			{
				return config;
			}

			var directoryInfo = new DirectoryInfo(pathToConfigFolder);
			var files = directoryInfo.GetFiles("*.json");
			//FileInfo[] files = directoryInfo.GetFiles("*.config");

			foreach (var file in files)
			{
				ContentAdminLink.LoadLinksFromJson(config, file.FullName);

				//var configFile = XmlHelper.GetXmlDocument(fileInfo.FullName);

				//ContentAdminLink.LoadLinksFromXml(
				//	config,
				//	configFile.DocumentElement
				//);
			}

			// now look for site specific links
			configFolderName = $"~/Data/Sites/{siteId.ToInvariantString()}/supplementaladminmenulinks";
			pathToConfigFolder = HttpContext.Current.Server.MapPath(configFolderName);

			if (Directory.Exists(pathToConfigFolder))
			{
				directoryInfo = new DirectoryInfo(pathToConfigFolder);
				files = directoryInfo.GetFiles("*.json");
				//files = directoryInfo.GetFiles("*.config");

				foreach (var file in files)
				{
					ContentAdminLink.LoadLinksFromJson(config, file.FullName);

					//var configFile = XmlHelper.GetXmlDocument(file.FullName);

					//ContentAdminLink.LoadLinksFromXml(
					//	config,
					//	configFile.DocumentElement
					//);
				}
			}

			// cache can be cleared by touching Web.config
			var cacheDependency = new CacheDependency(HttpContext.Current.Server.MapPath("~/Web.config"));

			HttpRuntime.Cache.Insert(
				cacheKey,
				config,
				cacheDependency,
				DateTime.Now.AddYears(1),
				TimeSpan.Zero,
				CacheItemPriority.Default,
				null
			);

			return HttpRuntime.Cache[cacheKey] as ContentAdminLinksConfiguration;
		}
	}
}
