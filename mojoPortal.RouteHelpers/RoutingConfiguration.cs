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

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Web;
using System.Xml;

namespace mojoPortal.RouteHelpers
{
    public class RoutingConfiguration
    {
        private Collection<RouteDefinition> routeDefinitions
            = new Collection<RouteDefinition>();

        public Collection<RouteDefinition> RouteDefinitions
        {
            get
            {
                return routeDefinitions;
            }
        }

        public static RoutingConfiguration GetConfig()
        {
            RoutingConfiguration config = new RoutingConfiguration();

            String configFolderName = "~/Setup/Routes/";

            string pathToConfigFolder = System.Web.Hosting.HostingEnvironment.MapPath(configFolderName);

            if (!Directory.Exists(pathToConfigFolder)) return config;


            DirectoryInfo directoryInfo
                = new DirectoryInfo(pathToConfigFolder);

            FileInfo[] routeFiles = directoryInfo.GetFiles("*.config");

            foreach (FileInfo fileInfo in routeFiles)
            {
                XmlDocument routeConfigFile = new XmlDocument();
                routeConfigFile.Load(fileInfo.FullName);
                LoadRoutes(config, routeConfigFile.DocumentElement);

            }

            return config;



        }

        private static void LoadRoutes(
            RoutingConfiguration config,
            XmlNode node
            )
        {
            RouteDefinition.LoadRoutes(config, node);


        }

    }
}