// Author:					
// Created:				    2010-10-18
// Last Modified:			2010-10-18
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 2011-03-09 changes from Michael to suport MVC3

using System;
using System.Collections.ObjectModel;
using System.Web;
using System.Web.Routing;
using System.Xml;

namespace mojoPortal.RouteHelpers
{
    public class RouteDefinition
    {

        private string name = string.Empty;

        public string Name
        {
            get { return name; }
        }

        private string routeUrl = string.Empty;

        public string RouteUrl
        {
            get { return routeUrl; }
        }

        private string virtualPath = string.Empty;

        public string VirtualPath
        {
            get { return virtualPath; }
        }

        private Collection<RouteDefault> routeDefaults = new Collection<RouteDefault>();

        public Collection<RouteDefault> RouteDefaults
        {
            get
            {
                return routeDefaults;
            }
        }

        private Collection<RouteRestriction> routeRestrictions = new Collection<RouteRestriction>();

        public Collection<RouteRestriction> RouteRestrictions
        {
            get
            {
                return routeRestrictions;
            }
        }

        private IRouteHandler routeHandler = null;
        public IRouteHandler RouteHandler
        {
            get { return routeHandler; }
        }

        public static void LoadRoutes(
            RoutingConfiguration config,
            XmlNode documentElement)
        {
            if (HttpContext.Current == null) return;
            if (documentElement.Name != "Routes") return;

            foreach (XmlNode node in documentElement.ChildNodes)
            {
                if (node.Name == "Route")
                {
                    RouteDefinition routeDef = new RouteDefinition();

                    XmlAttributeCollection attributeCollection
                        = node.Attributes;

                    if (attributeCollection["name"] != null)
                    {
                        routeDef.name = attributeCollection["name"].Value;
                    }

                    if (attributeCollection["routeUrl"] != null)
                    {
                        routeDef.routeUrl = attributeCollection["routeUrl"].Value;
                    }

                    if (attributeCollection["virtualPath"] != null)
                    {
                        routeDef.virtualPath = attributeCollection["virtualPath"].Value;
                    }

                    if (attributeCollection["routeHandler"] != null && typeof(IRouteHandler).IsAssignableFrom(Type.GetType(attributeCollection["routeHandler"].Value)))
                    {
                        routeDef.routeHandler = Activator.CreateInstance(Type.GetType(attributeCollection["routeHandler"].Value)) as IRouteHandler;
                    }


                    foreach (XmlNode child in node.ChildNodes)
                    {
                        if (child.Name == "Defaults")
                        {
                            RouteDefault.Load(
                                routeDef,
                                child);
                        }

                        if (child.Name == "Restrictions")
                        {
                            RouteRestriction.Load(
                                routeDef,
                                child);
                        }
                    }

                    config.RouteDefinitions.Add(routeDef);

                }

            }


        }


    }
}
