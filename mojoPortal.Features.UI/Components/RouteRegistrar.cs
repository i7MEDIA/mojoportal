using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using mojoPortal.Web.Routing;
using System.Web.Routing;
using System.Web.Mvc;


namespace mojoPortal.Features
{
    public class RouteRegistrar : IRegisterRoutes
    {
        public void Register(HttpConfiguration config)
        {
           // api routes

            config.Routes.MapHttpRoute(
                name: "ForumMod",
                routeTemplate: "api/forummod/{id}",
                defaults: new { controller = "ForumMod", id = RouteParameter.Optional }
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