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

using System.Xml;

namespace mojoPortal.RouteHelpers
{
    public class RouteDefault
    {


        private string parameterName = string.Empty;

        public string ParameterName
        {
            get { return parameterName; }
            set { parameterName = value; }
        }

        private string defaultValue = string.Empty;

        public string DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        public static void Load(
            RouteDefinition route,
            XmlNode defaultsNode)
        {
            foreach (XmlNode node in defaultsNode.ChildNodes)
            {
                if (node.Name == "add")
                {
                    if (
                        (node.Attributes["parameterName"] != null)
                        && (node.Attributes["defaultValue"] != null)
                        )
                    {
                        RouteDefault d = new RouteDefault();
                        d.parameterName = node.Attributes["parameterName"].Value;
                        d.defaultValue = node.Attributes["defaultValue"].Value;
                        route.RouteDefaults.Add(d);
                    }

                }

            }
        }
    }
}
