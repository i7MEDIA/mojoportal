using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI;

/// <summary>
/// a convenience link for the Administration menu. The link renders only for those in roles that can use the admin menu
/// </summary>
public class AdminMenuLink : HyperLink
{
	private string relativeUrl = "/Admin/AdminMenu.aspx";
	private mojoBasePage basePage = null;

	public bool RenderAsListItem { get; set; } = false;

	public string ListItemCss { get; set; } = string.Empty;

	public string LiteralExtraTopContent { get; set; } = string.Empty;

	public string LiteralExtraBottomContent { get; set; } = string.Empty;

	public string LinkImageUrl { get; set; } = string.Empty;

	private bool ShouldRender()
	{
		if (basePage == null) {
			return false;
		}

		if (!Page.Request.IsAuthenticated) {
			return false;
		}

		Controls.Add(new Literal { Text = LiteralExtraTopContent });
		Controls.Add(new Literal { Text = Resource.AdminLink });
		Controls.Add(new Literal { Text = LiteralExtraBottomContent});

		ToolTip = Resource.AdminMenuLink;

		if (!string.IsNullOrWhiteSpace(LinkImageUrl))
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

		if (WebUser.IsAdminOrContentAdminOrRoleAdmin) {
			return true;
		}

		if (basePage.CurrentPage == null) {
			return false;
		}

		if (!WebConfigSettings.UseRelatedSiteMode) {
			return false;
		}

		if (basePage.SiteInfo == null) {
			return false;
		}

		// in related sites mode users in site editors role can use admin menu
		if (WebUser.IsInRoles(basePage.SiteInfo.SiteRootEditRoles)) {
			return true;
		}

		return false;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (HttpContext.Current == null) { return; }
		EnableViewState = false;
		basePage = Page as mojoBasePage;

		Visible = ShouldRender();
		if (!Visible) { return; }

		if (basePage == null) { return; }

		this.SetOrAppendCss($"adminlink adminmenulink {CssClass}");

		if (SiteUtils.SslIsAvailable())
		{
			NavigateUrl = Page.ResolveUrl(basePage.SiteRoot + relativeUrl);
		}
		else
		{
			NavigateUrl = Page.ResolveUrl(basePage.RelativeSiteRoot + relativeUrl);
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
			if (ListItemCss.Length > 0)
			{
				writer.Write($"<li class=\"{ListItemCss}\">");
			}
			else
			{
				writer.Write("<li>");
			}
		}

		base.Render(writer);

		if (RenderAsListItem)
		{
			writer.Write("</li>");
		}
	}
}