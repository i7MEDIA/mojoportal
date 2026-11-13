using log4net;
using mojoPortal.Web.Plugins;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace mojoPortal.Web.Routing;

public class WebApiConfig
{
	private static readonly ILog log = LogManager.GetLogger(typeof(WebApiConfig));

	public static void Register(HttpConfiguration config)
	{
		// enable attribute routing
		config.MapHttpAttributeRoutes();

		// Register Web API routes from enabled plugins in Order sequence
		foreach (var descriptor in PluginLoader.EnabledPlugins)
		{
			if (descriptor.Instance == null)
			{
				continue;
			}

			try
			{
				descriptor.Instance.RegisterWebApi(config);
			}
			catch (Exception ex)
			{
				log.Error($"Failed to register Web API routes for plugin '{descriptor.Name}'.", ex);
			}
		}

		try
		{
			var routesConfig = RoutesConfig.GetConfig();

			foreach (var registrar in routesConfig.RouteRegistrars)
			{
				try
				{
					registrar.Register(config);
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

		config.Formatters.Clear();
		config.Formatters.Add(new JsonMediaTypeFormatter());

		var settings = config.Formatters.JsonFormatter.SerializerSettings;

		settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

		if (AppConfig.Debug)
		{
			settings.Formatting = Newtonsoft.Json.Formatting.Indented;
		}

		settings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; //"Angular" File Manager wants dates in this format

		config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
		config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));

		config.Routes.MapHttpRoute(
			name: "DefaultApi",
			routeTemplate: "api/{controller}/{id}",
			defaults: new { id = RouteParameter.Optional }
		);

		config.Routes.MapHttpRoute(
			name: "FileService",
			routeTemplate: "FileService/{controller}/{id}",
			defaults: new { controller = "FileService", id = RouteParameter.Optional, action = RouteParameter.Optional }
		);
	}
}
