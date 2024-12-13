using log4net;
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

			//routes.MapRoute(
			//	name: "BadWord",
			//	url: "BadWord/{action}",
			//	defaults: new { controller = "BadWord", action = "CheckString" }
			//);

			var registrarConfig = RoutesConfig.GetConfig();

			foreach (IRegisterRoutes registrar in registrarConfig.RouteRegistrars)
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

		//if (WebConfigSettings.AddDefaultMvcRoute)
		//{
		//	routes.MapRoute(
		//		name: "Default",
		//		url: "{controller}/{action}/{id}",
		//		defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
		//	);
		//}
	}
}