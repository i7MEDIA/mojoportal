using mojoPortal.Web.Components;
using Resources;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace mojoPortal.Web.UI;

public class LogoutLink : WebControl
{
	public bool UseLeftSeparator { get; set; } = false;
	public string LeftSeparatorImageUrl { get; set; } = string.Empty;
	public bool RenderAsListItem { get; set; } = false;
	public string ListItemCss { get; set; } = "topnavitem";
	public string OverrideText { get; set; } = string.Empty;


	protected override void OnLoad(System.EventArgs e)
	{
		base.OnLoad(e);
		Visible = Page.Request.IsAuthenticated;
	}

	
	protected override void Render(HtmlTextWriter writer)
	{
		if (HttpContext.Current == null)
		{
			writer.Write($"[{ID}]");

			return;
		}

		if (!Page.Request.IsAuthenticated || Context.User.Identity.AuthenticationType != "Forms")
		{
			return;
		}

		if (RenderAsListItem)
		{
			writer.WriteBeginTag("li");
			writer.WriteAttribute("class", ListItemCss);
			writer.Write(HtmlTextWriter.TagRightChar);
		}

		if (UseLeftSeparator)
		{
			if (LeftSeparatorImageUrl.Length > 0)
			{
				writer.Write($"""<img class="accent" alt="" src="{Page.ResolveUrl(LeftSeparatorImageUrl)}" border="0" />""");
			}
			else
			{
				writer.Write("""<span class="accent">|</span>""");
			}
		}

		if (CssClass.Length == 0)
		{
			CssClass = "sitelink";
		}

		writer.WriteBeginTag("a");
		writer.WriteAttribute("class", CssClass);
		writer.WriteAttribute("href", PageUrlService.GetLogoutLink());
		writer.Write(HtmlTextWriter.TagRightChar);

		if (OverrideText.Length > 0)
		{
			writer.WriteEncodedText(OverrideText);
		}
		else
		{
			writer.WriteEncodedText(Resource.LogoutLink);
		}

		writer.WriteEndTag("a");

		if (RenderAsListItem)
		{
			writer.WriteEndTag("li");
		}
	}
}
