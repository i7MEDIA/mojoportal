using mojoPortal.Web.Routing;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace mojoPortal.RouteHelpers
{
    public class ConfigRouteRegistrar : IRegisterRoutes
    {

        public void Register(HttpConfiguration config)
        {
            // api routes



        }

        public void RegisterRoutes(RouteCollection routes)
        {
            //this hooks up the legacy route config system routes
            RoutingHandler.Configure(routes);

        }

        public void RegisterGlobalFilters(GlobalFilterCollection filters)
        {

        }
    }
}
