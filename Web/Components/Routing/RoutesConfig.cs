using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace mojoPortal.Web.Routing;

public class RoutesConfig
{
	public List<IRegisterRoutes> RouteRegistrars { get; } = [];

	public static RoutesConfig GetConfig()
	{
		var routesConfig = new RoutesConfig();

		if (HttpRuntime.Cache["RoutesConfig"] is RoutesConfig cachedConfig)
		{
			return cachedConfig;
		}

		var configFolderName = WebConfigSettings.RouteConfigPath; //"~/Setup/RouteRegistrars/";

		var pathToConfigFolder = System.Web.Hosting.HostingEnvironment.MapPath(configFolderName);

		if (!Directory.Exists(pathToConfigFolder))
		{
			return routesConfig;
		}

		var directoryInfo = new DirectoryInfo(pathToConfigFolder);
		var routeFiles = directoryInfo.GetFiles("*.config");

		foreach (var fileInfo in routeFiles)
		{
			var routeConfigFile = XmlHelper.GetXmlDocument(fileInfo.FullName);
			LoadRoutes(routesConfig, routeConfigFile.DocumentElement);
		}

		//cache can be cleared by touching Web.config
		var cacheDependency = new CacheDependency(HttpContext.Current.Server.MapPath("~/Web.config"));

		HttpRuntime.Cache.Insert(
			"RoutesConfig",
			routesConfig,
			cacheDependency,
			DateTime.Now.AddMinutes(5),
			TimeSpan.Zero,
			CacheItemPriority.Default,
			null);

		return routesConfig;
	}


	private static void LoadRoutes(RoutesConfig config, XmlNode documentElement)
	{
		if (documentElement.Name != "Routes")
		{
			return;
		}

		foreach (XmlNode node in documentElement.ChildNodes)
		{
			if (node.Name == "IRegisterRoutes")
			{
				var attributeCollection = node.Attributes;

				if (attributeCollection["type"] != null && typeof(IRegisterRoutes).IsAssignableFrom(Type.GetType(attributeCollection["type"].Value)))
				{
					IRegisterRoutes registrar = Activator.CreateInstance(Type.GetType(attributeCollection["type"].Value)) as IRegisterRoutes;
					config.RouteRegistrars.Add(registrar);
				}
			}
		}
	}
}