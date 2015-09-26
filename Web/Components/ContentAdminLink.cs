// Author:					Joe Audette
// Created:				    2007-08-10
// Last Modified:			2007-08-10
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Web;
using System.Xml;

namespace mojoPortal.Web
{
    /// <summary>
    ///
    /// </summary>
    public class ContentAdminLink
    {
        private ContentAdminLink()
        { }

        private string resourceFile = string.Empty;
        private string resourceKey = string.Empty;
        private string url = string.Empty;
        private string visibleToRoles = string.Empty;
        private string cssClass = "customadminlink";

        public string ResourceFile
        {
            get { return resourceFile; }
        }

        public string ResourceKey
        {
            get { return resourceKey; }
        }

        public string Url
        {
            get { return url; }
        }

        public string CssClass
        {
            get { return cssClass; }
        }

        public string VisibleToRoles
        {
            get { return visibleToRoles; }
        }

        public static void LoadLinks(
            ContentAdminLinksConfiguration config,
            XmlNode documentElement)
        {
            if (HttpContext.Current == null) return;
            if (documentElement.Name != "adminMenuLinks") return;

            foreach (XmlNode node in documentElement.ChildNodes)
            {
                if (node.Name == "adminMenuLink")
                {
                    ContentAdminLink item = new ContentAdminLink();

                    XmlAttributeCollection attributeCollection
                        = node.Attributes;

                    if (attributeCollection["resourceFile"] != null)
                    {
                        item.resourceFile = attributeCollection["resourceFile"].Value;
                    }

                    if (attributeCollection["resourceKey"] != null)
                    {
                        item.resourceKey = attributeCollection["resourceKey"].Value;
                    }

                    if (attributeCollection["cssClass"] != null)
                    {
                        item.cssClass = attributeCollection["cssClass"].Value;
                    }

                    if (attributeCollection["url"] != null)
                    {
                        item.url = attributeCollection["url"].Value;
                    }

                    if (attributeCollection["visibleToRoles"] != null)
                    {
                        item.visibleToRoles = attributeCollection["visibleToRoles"].Value;
                    }

                    config.AdminLinks.Add(item);
                    


                }

            }


        }

    }
}
