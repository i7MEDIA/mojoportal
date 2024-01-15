using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;

namespace mojoPortal.Web.UI;

public class ChangePasswordLink : WebControl
{
	public bool UseLeftSeparator { get; set; } = false;
	public string LeftSeparatorImageUrl { get; set; } = string.Empty;
	public bool RenderAsListItem { get; set; } = false;
	public string ListItemCss { get; set; } = "topnavitem";

	protected override void Render(HtmlTextWriter writer)
	{
		if (Page.Request.IsAuthenticated && Context.User.Identity.AuthenticationType == "Forms")
		{
			if (RenderAsListItem) writer.Write($"<li class='{ListItemCss}'>");

			if (UseLeftSeparator)
			{
				if (LeftSeparatorImageUrl.Length > 0)
				{
					writer.Write($"<img class=\"accent\"' src=\"{Page.ResolveUrl(LeftSeparatorImageUrl)}\" />");
				}
				else
				{
					writer.Write("<span class=\"accent\">|</span>");
				}
			}

			if (string.IsNullOrWhiteSpace(CssClass))
			{
				CssClass = "sitelink";
			}

			string urlToUse = SiteUtils.GetNavigationSiteRoot() + "/Secure/ChangePassword.aspx";

			writer.Write($"<a href=\"{Page.ResolveUrl(urlToUse)}\" title=\"{Resource.ChangePasswordLink}\" class=\"{CssClass}\">{Resource.ChangePasswordLink}</a>");

			if (RenderAsListItem) writer.Write("</li>");
		}
	}
}