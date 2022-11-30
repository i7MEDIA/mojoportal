using AutoMapper;
using AutoMapper.Configuration;
using log4net;
using mojoPortal.Web.App_Start;
using Newtonsoft.Json.Serialization;
using System;
using System.CodeDom.Compiler;
using System.Net.Http.Headers;
using System.Web.Http;

namespace mojoPortal.Web.Routing
{
	public class WebApiConfig
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(WebApiConfig));

		public static void Register(HttpConfiguration config)
		{

			// enable attribute routing
			config.MapHttpAttributeRoutes();

			try
			{
				RoutesConfig registrarConfig = RoutesConfig.GetConfig();

				foreach (IRegisterRoutes registrar in registrarConfig.RouteRegistrars)
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

			//Mapper.Initialize(c => c.AddProfile<MappingProfile>());

			//var mapperConfig = new MapperConfiguration(cfg =>
			//{
			//	cfg.AddProfile<MappingProfile>();
			//});

			//var mapper = mapperConfig.CreateMapper();

			var settings = config.Formatters.JsonFormatter.SerializerSettings;
			settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			// Comment out line below for production
			settings.Formatting = Newtonsoft.Json.Formatting.Indented;
			// The setting below is to make the Angular File Manager work
			settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";

			config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
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

			//config.Routes.MapHttpRoute(
			//	name: "BadWord",
			//	routeTemplate: "BadWord/{action}/{id}",
			//	defaults: new {controller = "BadWord", action = "CheckString", id = RouteParameter.Optional }
			//);
		}
	}
}