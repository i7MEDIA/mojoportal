using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using mojoPortal.Web.Routing;
using System.Web.Routing;
using System.Web.Mvc;


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