using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;

namespace mojoPortal.Web.UI;


public class UserProfileLink : WebControl
{
	public bool UseLeftSeparator { get; set; } = false;

	public string LeftSeparatorImageUrl { get; set; } = string.Empty;

	public bool RenderAsListItem { get; set; } = false;

	public string ListItemCss { get; set; } = "topnavitem";

	public string OverrideText { get; set; } = string.Empty;

	protected override void OnLoad(System.EventArgs e)
	{
		base.OnLoad(e);
		if (!Page.Request.IsAuthenticated) { this.Visible = false; return; }
	}

	protected override void Render(HtmlTextWriter writer)
	{
		if (HttpContext.Current == null)
		{
			writer.Write($"[{this.ID}]");
			return;
		}

		if (!Page.Request.IsAuthenticated) { return; }
		if (!WebConfigSettings.AllowUserProfilePage) { return; }

		if (RenderAsListItem)
		{
			//writer.Write("<li class='" + listItemCSS + "'>");
			writer.WriteBeginTag("li");
			writer.WriteAttribute("class", ListItemCss);
			writer.Write(HtmlTextWriter.TagRightChar);

		}

		if (LeftSeparatorImageUrl.Length > 0)
		{
			writer.Write($"<img class=\"accent\" alt=\"\" src=\"{Page.ResolveUrl(LeftSeparatorImageUrl)}\" border=\"0\" /> ");
		}
		else
		{
			if (UseLeftSeparator)
			{
				writer.Write("<span class=\"accent\">|</span>");
			}
		}

		string urlToUse = "Secure/UserProfile.aspx".ToLinkBuilder().ToString();
		if (CssClass.Length == 0)
		{
			CssClass = "sitelink";
		}


		writer.WriteBeginTag("a");
		writer.WriteAttribute("class", CssClass);
		//writer.WriteAttribute("title", Resource.ProfileLink);
		writer.WriteAttribute("href", Page.ResolveUrl(urlToUse));
		writer.Write(HtmlTextWriter.TagRightChar);
		if (OverrideText.Length > 0)
		{
			writer.WriteEncodedText(OverrideText);
		}
		else
		{
			writer.WriteEncodedText(Resource.ProfileLink);
		}
		writer.WriteEndTag("a");


		if (RenderAsListItem)
		{
			writer.WriteEndTag("li");
		}
	}
}
