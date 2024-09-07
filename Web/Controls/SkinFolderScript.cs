using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.UI;

/// <summary>
/// allows you to inclde a javascript file form the skin folder by just its name, the path to the skin folder will be resolved.
/// </summary>
public class SkinFolderScript : WebControl
{
	private Guid skinVersion = Guid.Empty;

	public string ScriptFileName { get; set; } = string.Empty;

	public string ScriptFullUrl { get; set; } = string.Empty;

	/// <summary>
	/// If true the skin version guid will be appended to the script url. This helps for controlling caching of the script.
	/// </summary>
	public bool UseSkinVersion { get; set; } = true;

	public bool IsStartup { get; set; } = false;

	public bool AddToCombinedScript { get; set; } = false;

	/// <summary>
	/// a semi colon separated list of role names
	/// Admins;Content Administrators
	/// </summary>
	public string VisibleRoles { get; set; } = string.Empty;

	/// <summary>
	/// a comma separated list of relative urls where the script file should be used
	/// if specified then the link will only be rendered if the current Request.RawUrl contains on of the specified values
	/// /Admin,/HtmlEdit.aspx would add the css only on pages in the Admin folder and on the HtmlEdit.aspx page in the root
	/// </summary>
	public string VisibleUrls { get; set; } = string.Empty;

	/// <summary>
	/// if true script will be rendered in same location as SkinFolderScript control
	/// </summary>
	public bool RenderInPlace { get; set; } = false;
	public string ScriptRefFormat { get; set; } = "<script src=\"{0}\" data-loader=\"skinfolderscript\"></script>";

	protected override void OnPreRender(System.EventArgs e)
	{
		base.OnPreRender(e);

		if (CacheHelper.GetCurrentSiteSettings() is SiteSettings siteSettings)
		{
			skinVersion = siteSettings.SkinVersion;
		}

		if (string.IsNullOrWhiteSpace(ScriptFileName))
		{
			return;
		}

		if (ShouldRender() && !RenderInPlace)
		{
			SetupScript();
		}
	}

	private void SetupScript()
	{
		if (!string.IsNullOrWhiteSpace(ScriptFullUrl))
		{
			if (IsStartup)
			{
				ScriptManager.RegisterStartupScript(
					this,
					typeof(Page),
					ClientID,
					string.Format(CultureInfo.InvariantCulture, $"\n{ScriptRefFormat}", BuildUrl(ScriptFullUrl)),
					false);
			}
			else
			{
				ScriptManager.RegisterClientScriptBlock(
					this,
					typeof(Page),
					ClientID,
					string.Format(CultureInfo.InvariantCulture, $"\n{ScriptRefFormat}", BuildUrl(ScriptFullUrl)),
					false);
			}
			return;

		}

		if (string.IsNullOrWhiteSpace(ScriptFileName))
		{
			return;
		}

		string scriptUrl = SiteUtils.DetermineSkinBaseUrl(true, Page) + ScriptFileName;

		if (IsStartup)
		{
			ScriptManager.RegisterStartupScript(
				this,
				typeof(Page),
				ScriptFileName,
				string.Format(CultureInfo.InvariantCulture, $"\n{ScriptRefFormat}", BuildUrl(scriptUrl)),
				false);
		}
		else
		{
			if (AddToCombinedScript && scriptUrl.StartsWith("/") && (Page is mojoBasePage basePage))
			{
				basePage.ScriptConfig.AddPathScriptReference(BuildUrl(scriptUrl, false).ToString());
			}
			else
			{
				ScriptManager.RegisterClientScriptBlock(
					this,
					typeof(Page),
					ScriptFileName,
					string.Format(CultureInfo.InvariantCulture, $"\n{ScriptRefFormat}", BuildUrl(scriptUrl)),
					false);
			}
		}
	}


	protected override void Render(HtmlTextWriter writer)
	{
		if (!RenderInPlace || !ShouldRender())
		{
			return;
		}

		if (!string.IsNullOrWhiteSpace(ScriptFullUrl))
		{
			writer.Write(string.Format(CultureInfo.InvariantCulture, $"\n{ScriptRefFormat}", BuildUrl(ScriptFullUrl)));
		}
		else
		{
			string scriptUrl = SiteUtils.DetermineSkinBaseUrl(true, Page) + ScriptFileName;
			writer.Write(string.Format(CultureInfo.InvariantCulture, $"\n{ScriptRefFormat}", BuildUrl(scriptUrl)));
		}

	}

	protected bool ShouldRender()
	{
		if (!string.IsNullOrWhiteSpace(VisibleRoles))
		{
			if (!WebUser.IsInRoles(VisibleRoles))
			{
				return false;
			}
		}

		if (!string.IsNullOrWhiteSpace(VisibleUrls))
		{
			bool match = false;
			List<string> allowedUrls = VisibleUrls.SplitOnChar(',');
			foreach (string u in allowedUrls)
			{
				//Page.AppRelativeVirtualPath will match for things like blog posts where the friendly url is something like /my-cool-post which
				//is then mapped to the /Blog/ViewPost.aspx page. So, one could use /Blog/ViewPost.aspx in the AllowedUrls property to render
				//a script on blog post pages.
				if (base.Page.AppRelativeVirtualPath.Contains(u, StringComparison.OrdinalIgnoreCase))
				{
					match = true;
				}

				//Page.Request.RawUrl is the url used for the request, as in the example above '/my-cool-post'
				if (base.Page.Request.RawUrl.Contains(u, StringComparison.OrdinalIgnoreCase))
				{
					match = true;
				}
			}

			if (!match)
			{
				return false;
			}
		}

		return true;
	}

	//private string GetSkinVersionGuidQueryParam(string url)
	//{
	//	if (UseSkinVersion)
	//	{
	//		string sep = url.IndexOf('?') >= 0 ? "&" : "?";

	//		return sep + CacheHelper.GetCurrentSiteSettings().SkinVersion.ToString();
	//	}

	//	return string.Empty;
	//}

	private LinkBuilder BuildUrl(string url, bool includeSiteRoot = true)
	{
		var link = url.ToLinkBuilder(includeSiteRoot);

		if (UseSkinVersion)
		{
			return link.AddParam("v", skinVersion);
		}

		return link;
	}
}
