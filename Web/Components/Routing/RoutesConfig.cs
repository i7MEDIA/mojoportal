#nullable enable
using log4net;
using mojoPortal.Web.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace mojoPortal.Web.Routing;

public class RoutesConfig
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(RoutesConfig));

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
			try
			{
				var routeConfigFile = XmlHelper.GetXmlDocument(fileInfo.FullName);

				LoadRoutes(routesConfig, routeConfigFile.DocumentElement);
			}
			catch (Exception ex)
			{
				_log.Warn($"Skipping route registrar file '{fileInfo.FullName}': {ex.Message}");
			}
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
		if (documentElement is null || documentElement.Name != "Routes")
		{
			return;
		}

		var pluginAssemblies = new HashSet<string>(
			PluginLoader.EnabledPlugins
				.Where(p => p.Instance != null)
				.SelectMany(p => p.Assemblies)
				.Select(a => a.GetName().Name),
			StringComparer.OrdinalIgnoreCase);

		foreach (XmlNode node in documentElement.ChildNodes)
		{
			if (node.Name == "IRegisterRoutes")
			{
				var attributeCollection = node.Attributes;
				var typeName = attributeCollection?["type"]?.Value;

				if (string.IsNullOrWhiteSpace(typeName))
				{
					continue;
				}

				try
				{
					var targetType = Type.GetType(typeName);

					if (targetType is null || !typeof(IRegisterRoutes).IsAssignableFrom(targetType))
					{
						continue;
					}

					// If this assembly already has an enabled IPlugin that registers routes directly, skip legacy XML entry
					var asmName = targetType.Assembly.GetName().Name;

					if (pluginAssemblies.Contains(asmName))
					{
						continue;
					}

					if (Activator.CreateInstance(targetType) is IRegisterRoutes registrar)
					{
						config.RouteRegistrars.Add(registrar);
					}
				}
				catch (Exception ex)
				{
					_log.Warn($"Skipping legacy IRegisterRoutes '{typeName}': {ex.Message}");
				}
			}
		}
	}
}
