// Author:					
// Created:				    2010-10-18
// Last Modified:			2014-07-11
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Web.Routing;
using System.Web;
using System.Web.UI;
using System.Web.Compilation;
using System.Web.Mvc;



namespace mojoPortal.RouteHelpers
{
    public class RoutingHandler : IRouteHandler
    {
        public static void Configure(RouteCollection routes)
        {
            
            RoutingConfiguration config = RoutingConfiguration.GetConfig();

            if (config == null) { return; }

            foreach (RouteDefinition route in config.RouteDefinitions)
            {
                routes.Add(route.Name, GetRouteForElement(route));
            }

        }

        public static Route GetRouteForElement(RouteDefinition route)
        {

            IRouteHandler handler;
            RouteValueDictionary defaults = new RouteValueDictionary();
            RouteValueDictionary restrictions = new RouteValueDictionary();

            if (String.IsNullOrEmpty(route.VirtualPath))
            {
                if (route.RouteHandler == null)
                {
                    //Stop Route ...
                    handler = new StopRoutingHandler();
                }
                else
                {
                    handler = route.RouteHandler;
                }
            }
            else
            {
                handler = new mojoPortal.RouteHelpers.RoutingHandler(route.VirtualPath);
            }

            foreach (RouteDefault d in route.RouteDefaults)
            {
                defaults.Add(d.ParameterName, d.DefaultValue);
            }


            foreach (RouteRestriction r in route.RouteRestrictions)
            {
                restrictions.Add(r.ParameterName, r.Restriction);
            }

            return new Route(route.RouteUrl,
                defaults, restrictions, handler);
        }

        public RoutingHandler(string virtualPath)
        {
            this.VirtualPath = virtualPath;
        }

        public string VirtualPath { get; private set; }

        #region IRouteHandler Members

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            string finalPath = VirtualPath;
            if (requestContext.RouteData.Values.Count > 0)
            {
                List<string> values = new List<string>();
                //Add these to the virtual path as QS arguments
                foreach (var item in requestContext.RouteData.Values)
                {
                    values.Add(item.Key + "=" + item.Value);
                }
                finalPath += "?" + String.Join("&", values.ToArray());

            }

            //Rewrite the path to pass the query string values. 
            HttpContext.Current.RewritePath(finalPath);

            var page = BuildManager.CreateInstanceFromVirtualPath(
                VirtualPath, typeof(Page)) as IHttpHandler;
            return page;
        }


        #endregion

    }
}

