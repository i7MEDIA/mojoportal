using mojoPortal.Business.WebHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml;

namespace mojoPortal.Web;

public class ContentAdminLink
{
	#region Properties

	public string ResourceFile { get; set; } = string.Empty;
	public string ResourceKey { get; set; } = string.Empty;
	public string Url { get; set; } = string.Empty;
	public string CssClass { get; set; } = "customadminlink";
	public string VisibleToRoles { get; set; } = string.Empty;
	public int SortOrder { get; set; } = 500;
	public string IconCssClass { get; set; } = string.Empty;
	public string Parent { get; set; } = "root";
	public Dictionary<string, string> Attributes { get; set; } = [];

	#endregion

	public ContentAdminLink()
	{ }


	public static void LoadLinksFromJson(ContentAdminLinksConfiguration config, string configPath)
	{
		var configFile = new FileInfo(configPath);

		if (configFile.Exists)
		{
			var content = File.ReadAllText(configFile.FullName);
			var items = JsonConvert.DeserializeObject<IEnumerable<ContentAdminLink>>(content);

			config.AdminLinks.AddRange(items);
		}
	}

	
	public static void LoadLinksFromXml(ContentAdminLinksConfiguration config, XmlNode documentElement)
	{
		if (
			HttpContext.Current == null ||
			documentElement.Name != "adminMenuLinks"
		)
		{
			return;
		}

		foreach (XmlNode node in documentElement.ChildNodes)
		{
			if (node.Name == "adminMenuLink")
			{
				ContentAdminLink item = new ContentAdminLink();

				XmlAttributeCollection attributeCollection = node.Attributes;

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
					item.Url = attributeCollection["url"].Value.ToLinkBuilder().ToString();
				}

				if (attributeCollection["visibleToRoles"] != null)
				{
					item.VisibleToRoles = attributeCollection["visibleToRoles"].Value;
				}

				if (attributeCollection["sortOrder"] != null)
				{
					item.SortOrder = Convert.ToInt32(attributeCollection["sortOrder"].Value);
				}

				if (attributeCollection["parent"] != null)
				{
					item.Parent = attributeCollection["parent"].Value;
				}

				if (WebUser.IsInRoles(item.VisibleToRoles))
				{
					config.AdminLinks.Add(item);
				}
			}
		}
	}
}
