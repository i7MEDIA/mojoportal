using mojoPortal.Web.Routing;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;


namespace SuperFlexiUI
{
	public class RouteRegistrar : IRegisterRoutes
	{
		public void Register(HttpConfiguration config)
		{
			// api routes

			config.Routes.MapHttpRoute(
				name: "SuperFlexi",
				routeTemplate: "SuperFlexi/{action}/{id}",
				defaults: new { controller = "SuperFlexi", id = RouteParameter.Optional }
			);
		}

		public void RegisterRoutes(RouteCollection routes)
		{
			//mvc routes

		}

		public void RegisterGlobalFilters(GlobalFilterCollection filters)
		{

		}

	}
}