/*
 * Copied from MS CSSFriendly and modified by mojoPortal Team
 * 
 */

using System;
using System.Reflection;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace mojoPortal.Web;

public class AdapterHelpers
{
	private AdapterHelpers()
	{
	}

	public static int GetListItemIndex(ListControl control, ListItem item)
	{
		int num = control.Items.IndexOf(item);
		if (num == -1)
		{
			throw new NullReferenceException("ListItem does not exist ListControl.");
		}

		return num;
	}

	public static string GetListItemClientID(ListControl control, ListItem item)
	{
		if (control == null)
		{
			throw new ArgumentNullException("Control can not be null.");
		}

		int listItemIndex = GetListItemIndex(control, item);
		return $"{control.ClientID}_{listItemIndex}";
	}

	public static string GetListItemUniqueID(ListControl control, ListItem item)
	{
		if (control == null)
		{
			throw new ArgumentNullException("Control can not be null.");
		}

		int listItemIndex = GetListItemIndex(control, item);
		return $"{control.UniqueID}${listItemIndex}";
	}

	public static bool HeadContainsLinkHref(Page page, string href)
	{
		if (page == null)
		{
			throw new ArgumentNullException("page");
		}

		foreach (Control control in page.Header.Controls)
		{
			if (control is HtmlLink && (control as HtmlLink).Href == href)
			{
				return true;
			}
		}

		return false;
	}

	public static void RegisterEmbeddedCSS(string css, Type type, Page page)
	{
		string webResourceUrl = page.ClientScript.GetWebResourceUrl(type, css);
		if (!string.IsNullOrEmpty(webResourceUrl) && !HeadContainsLinkHref(page, webResourceUrl))
		{
			var htmlLink = new HtmlLink();
			htmlLink.Href = page.ResolveUrl(webResourceUrl);
			htmlLink.Attributes["type"] = "text/css";
			htmlLink.Attributes["rel"] = "stylesheet";
			page.Header.Controls.Add(htmlLink);
		}
	}

	public static void RegisterClientScript(string resource, Type type, Page page)
	{
		string text = page.ClientScript.GetWebResourceUrl(type, resource);
		if (string.IsNullOrEmpty(text))
		{
			string text2 = WebConfigurationManager.AppSettings.Get("CSSFriendly-JavaScript-Path");
			if (string.IsNullOrEmpty(text2))
			{
				text2 = "~/JavaScript";
			}

			text = (text2.EndsWith("/") ? (text2 + resource) : (text2 + "/" + resource));
		}

		if (!page.ClientScript.IsClientScriptIncludeRegistered(type, resource))
		{
			page.ClientScript.RegisterClientScriptInclude(type, resource, page.ResolveUrl(text));
		}
	}

	public static object GetPrivateField(object container, string fieldName)
	{
		Type type = container.GetType();
		return type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(container);
	}
}
