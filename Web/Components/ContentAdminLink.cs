// Author:					
// Created:				    2007-08-10
// Last Modified:			2018-10-30
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using System;
using System.Web;
using System.Xml;

namespace mojoPortal.Web
{
    public class ContentAdminLink
    {
        public ContentAdminLink()
        { }

		public string ResourceFile { get; set; } = string.Empty;

		public string ResourceKey { get; set; } = string.Empty;

		public string Url { get; set; } = string.Empty;

		public string CssClass { get; set; } = "customadminlink";

		private string visibleToRoles { get; set; } = string.Empty;

		public int SortOrder { get; set; } = 500;

		public string IconCssClass { get; set; } = string.Empty;

		public string Parent { get; set; } = "root";

		public static void LoadLinksFromXml(
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
                        item.ResourceFile = attributeCollection["resourceFile"].Value;
                    }

                    if (attributeCollection["resourceKey"] != null)
                    {
                        item.ResourceKey = attributeCollection["resourceKey"].Value;
                    }

                    if (attributeCollection["cssClass"] != null)
                    {
                        item.CssClass = attributeCollection["cssClass"].Value;
                    }

					if (attributeCollection["iconCssClass"] != null)
					{
						item.IconCssClass = attributeCollection["iconCssClass"].Value;
					}

					if (attributeCollection["url"] != null)
                    {
                        item.Url = WebUtils.ResolveUrl(attributeCollection["url"].Value);
                    }

                    if (attributeCollection["visibleToRoles"] != null)
                    {
                        item.visibleToRoles = attributeCollection["visibleToRoles"].Value;
                    }

					if (attributeCollection["sortOrder"] != null)
					{
						item.SortOrder = Convert.ToInt32(attributeCollection["sortOrder"].Value);
					}

					if (attributeCollection["parent"] != null)
					{
						item.Parent = attributeCollection["parent"].Value;
					}

					if (WebUser.IsInRoles(item.visibleToRoles))
					{
						config.AdminLinks.Add(item);
					}
                    


                }

            }


        }

    }
}
