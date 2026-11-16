#nullable enable
using log4net;
using mojoPortal.Web.Plugins;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace mojoPortal.Web.Routing;

public class RouteRegistrar
{
	private static readonly ILog log = LogManager.GetLogger(typeof(RouteRegistrar));


	public static void RegisterRoutes(RouteCollection routes)
	{
		try
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.MapMvcAttributeRoutes();

			routes.MapRoute(
				name: "FileManager",
				url: "FileManager/{action}/{id}",
				defaults: new { controller = "FileManager", action = "Index", id = UrlParameter.Optional }
			);

			if (AppConfig.OAuth.Configured)
			{
				routes.MapRoute(
					name: "ExternalLogin",
					url: "ExternalLogin/{action}",
					defaults: new { controller = "ExternalLogin", action = "Index" }
				);
			}

			// Register MVC 5 routes from enabled plugins in Order sequence
			foreach (var descriptor in PluginLoader.EnabledPlugins)
			{
				if (descriptor.Instance is null)
				{
					continue;
				}

				try
				{
					descriptor.Instance.RegisterRoutes(routes);
				}
				catch (Exception ex)
				{
					log.Error($"Failed to register MVC routes for plugin '{descriptor.Name}'.", ex);
				}
			}

			var registrarConfig = RoutesConfig.GetConfig();

			foreach (var registrar in registrarConfig.RouteRegistrars)
			{
				try
				{
					registrar.RegisterRoutes(routes);
				}
				catch (Exception ex)
				{
					log.Error(ex);
				}
			}
		}
		catch (Exception ex)
		{
			log.Error(ex);
		}
	}
}
