using mojoPortal.Business.WebHelpers;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI;

/// <summary>
/// A convenience link for the File Manager. The link renders only for those in roles that can use it
/// </summary>
public class FileManagerLink : HyperLink
{
	private mojoBasePage basePage = null;

	public bool RenderAsListItem { get; set; } = false;
	public string ListItemCss { get; set; } = string.Empty;
	public string ListItemID { get; set; } = string.Empty;
	public bool OpenInModal { get; set; } = true;
	public string ModalHookType { get; set; } = "CssClass";
	public string ModalHookValue { get; set; } = "cblink";
	public string QueryString { get; set; } = string.Empty;
	public string LiteralExtraTopContent { get; set; } = string.Empty;
	public string LiteralExtraBottomContent { get; set; } = string.Empty;
	public string LinkImageUrl { get; set; } = string.Empty;


	public bool ShouldRender()
	{
		if (basePage == null)
		{
			return false;
		}

		if (!Page.Request.IsAuthenticated)
		{
			return false;
		}

		if (!WebConfigSettings.ShowFileManagerLink)
		{
			return false;
		}

		if (WebConfigSettings.DisableFileManager)
		{
			return false;
		}

		if (basePage.SiteInfo == null)
		{
			return false;
		}

		if ((!CacheHelper.GetCurrentSiteSettings().IsServerAdminSite) && (!AppConfig.MultiTenancy.AllowFileManager))
		{
			return false;
		}

		if (SiteUtils.UserIsSiteEditor() || WebUser.IsAdminOrContentAdmin)
		{
			return true;
		}

		// Only roles that can delete can use File Manager
		if (!WebUser.IsInRoles(basePage.SiteInfo.RolesThatCanDeleteFilesInEditor))
		{
			return false;
		}

		if (WebUser.IsInRoles(basePage.SiteInfo.UserFilesBrowseAndUploadRoles)
			|| WebUser.IsInRoles(basePage.SiteInfo.GeneralBrowseAndUploadRoles)
			|| WebUser.IsInRoles(basePage.SiteInfo.GeneralBrowseRoles))
		{
			return true;
		}

		return false;
	}


	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);

		if (HttpContext.Current == null)
		{
			return;
		}

		EnableViewState = false;
		basePage = Page as mojoBasePage;
		Visible = ShouldRender();

		if (!Visible)
		{
			return;
		}

		if (basePage == null)
		{
			return;
		}

		if (string.IsNullOrWhiteSpace(Global.SkinConfig.ModalTemplatePath) || string.IsNullOrWhiteSpace(Global.SkinConfig.ModalScriptPath))
		{
			basePage.ScriptConfig.IncludeColorBox = true;
		}
		else
		{
			basePage.EnsureDefaultModal();
		}

		if (OpenInModal && ModalHookType == "CssClass")
		{
			CssClass = $"adminlink filemanlink {CssClass} {ModalHookValue}";
		}
		else
		{
			CssClass = $"adminlink filemanlink {CssClass}";
		}

		if (OpenInModal && (ModalHookType == "Attributes"))
		{
			Dictionary<string, string> keyValuePairs = ModalHookValue.Split(',')
				.Select(v => v.Split(':'))
				.ToDictionary(pair => pair[0], pair => pair[1]);

			foreach (KeyValuePair<string, string> item in keyValuePairs)
			{
				Attributes.Add(item.Key, item.Value);
			}
		}

		Controls.Add(new Literal
		{
			Text = LiteralExtraTopContent
		});

		Controls.Add(new Literal
		{
			Text = Resource.AdminMenuFileManagerLink
		});

		Controls.Add(new Literal
		{
			Text = LiteralExtraBottomContent
		});

		ToolTip = Resource.AdminMenuFileManagerLink;

		if (SiteUtils.SslIsAvailable())
		{
			NavigateUrl = $"{basePage.SiteRoot}/FileManager{QueryString}";
		}
		else
		{
			NavigateUrl = $"{basePage.RelativeSiteRoot}/FileManager{QueryString}";
		}

		if (!string.IsNullOrWhiteSpace(LinkImageUrl) && !OpenInModal)
		{
			if (LinkImageUrl.StartsWith("~/"))
			{
				ImageUrl = Page.ResolveUrl(LinkImageUrl);
			}
			else
			{
				ImageUrl = SiteUtils.DetermineSkinBaseUrl(page: Page) + LinkImageUrl.TrimStart('/');
			}
		}
	}

	protected override void Render(HtmlTextWriter writer)
	{
		if (HttpContext.Current == null)
		{
			writer.Write("[" + ID + "]");
			return;
		}

		if (RenderAsListItem)
		{
			string liClass = string.Empty;
			string liID = string.Empty;

			if (ListItemID.Length > 0)
			{
				liID = " id=\"" + ListItemID + "\"";
			}

			if (ListItemCss.Length > 0)
			{
				liClass = " class=\"" + ListItemCss + "\"";
			}

			writer.Write("<li" + liID + liClass + ">");
		}

		base.Render(writer);

		if (RenderAsListItem)
		{
			writer.Write("</li>");
		}
	}
}
