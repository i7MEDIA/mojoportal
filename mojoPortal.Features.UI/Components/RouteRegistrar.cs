using mojoPortal.Web.Routing;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;


namespace mojoPortal.Features
{
	public class RouteRegistrar : IRegisterRoutes
	{
		// API Routes
		public void Register(HttpConfiguration config)
		{
			config.Routes.MapHttpRoute(
				name: "ForumMod",
				routeTemplate: "api/forummod/{id}",
				defaults: new { controller = "ForumMod", id = RouteParameter.Optional }
			);

			config.Routes.MapHttpRoute(
				name: "BIG",
				routeTemplate: "api/BetterImageGallery/",
				defaults: new { controller = "BetterImageGallery" }
			);

		}

		// MVC Routes
		public void RegisterRoutes(RouteCollection routes)
		{ }

		public void RegisterGlobalFilters(GlobalFilterCollection filters)
		{ }
	}
}