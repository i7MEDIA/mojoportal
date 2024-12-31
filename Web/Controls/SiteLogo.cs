using mojoPortal.Business.WebHelpers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI;

public class SiteLogo : WebControl
{
	public string HeadingElement { get; set; } = "h1";
	public bool UseH1 { get; set; } = false;
	public string OverrideUrl { get; set; } = string.Empty;
	public string OverrideTitle { get; set; } = string.Empty;
	public string OverrideImageUrl { get; set; } = string.Empty;
	public string ImageCssClass { get; set; } = "sitelogo";
	public string LinkCssClass { get; set; } = string.Empty;
	public string H1CssClass { get; set; } = "sitelogo";
	public bool UseUrl { get; set; } = true;


	protected override void Render(HtmlTextWriter writer)
	{

		if (HttpContext.Current == null)
		{
			writer.Write($"[{ID}]");
		}
		else
		{
			var siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (siteSettings == null || string.IsNullOrWhiteSpace(siteSettings.Logo))
			{
				return;
			}

			var urlToUse = !string.IsNullOrWhiteSpace(OverrideUrl) ?
				OverrideUrl.ToLinkBuilder().ToString() :
				"~/".ToLinkBuilder().ToString();

			var titleToUse = !string.IsNullOrWhiteSpace(OverrideTitle) ?
				OverrideTitle :
				siteSettings.SiteName.Replace("\"", "'");

			var mediaPath = WebConfigSettings.SiteLogoUseMediaFolder ?
				"media/" :
				string.Empty;

			var imageUrlToUse = !string.IsNullOrWhiteSpace(OverrideImageUrl) ?
				OverrideImageUrl.ToLinkBuilder().ToString() :
				Invariant($"~/Data/Sites/{siteSettings.SiteId}/{mediaPath}logos/{siteSettings.Logo}").ToLinkBuilder().ToString();

			if (UseH1)
			{
				writer.WriteBeginTag(HeadingElement);
				writer.WriteAttribute("class", H1CssClass);
			}

			if (UseUrl)
			{
				writer.WriteBeginTag("a");
				writer.WriteAttribute("href", urlToUse);
				writer.WriteAttribute("title", titleToUse);
				writer.WriteAttribute("class", LinkCssClass);
				writer.Write(HtmlTextWriter.TagRightChar);
			}

			writer.WriteBeginTag("img");
			writer.WriteAttribute("class", ImageCssClass);
			writer.WriteAttribute("alt", titleToUse);
			writer.WriteAttribute("src", imageUrlToUse);
			writer.Write(HtmlTextWriter.SelfClosingTagEnd);

			if (UseUrl)
			{
				writer.WriteEndTag("a");
			}

			if (UseH1)
			{
				writer.WriteEndTag(HeadingElement);
			}
		}
	}
}
