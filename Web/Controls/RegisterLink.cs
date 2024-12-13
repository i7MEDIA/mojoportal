using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using Resources;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#nullable enable

namespace mojoPortal.Web.UI;

public class RegisterLink : WebControl
{
	public bool UseLeftSeparator { get; set; } = false;
	public bool RenderAsListItem { get; set; } = false;
	public string ListItemCss { get; set; } = "topnavitem";
	public string OverrideText { get; set; } = string.Empty;
	public bool UseRelNoFollow { get; set; } = true;
	public bool AutoDetectReturnUrl { get; set; } = true;


	protected override void Render(HtmlTextWriter writer)
	{
		if (HttpContext.Current == null)
		{
			writer.Write($"[{ID}]");

			return;
		}

		DoRender(writer);
	}


	private void DoRender(HtmlTextWriter writer)
	{
		if (string.IsNullOrWhiteSpace(CssClass))
		{
			CssClass = "sitelink";
		}

		var siteSettings = CacheHelper.GetCurrentSiteSettings();

		if (
			Page.Request.IsAuthenticated ||
			siteSettings == null ||
			!siteSettings.AllowNewRegistration ||
			siteSettings.UseLdapAuth && !siteSettings.AllowDbFallbackWithLdap ||
			//everything is disabled so render nothing
			siteSettings.DisableDbAuth && !siteSettings.AllowOpenIdAuth
		)
		{
			return;
		}

		var registrationLink = GetRegistrationLink(siteSettings);

		if (UseLeftSeparator)
		{
			writer.Write("""<span class="accent">|</span>""");
		}

		if (RenderAsListItem)
		{
			writer.WriteBeginTag("li");
			writer.WriteAttribute("class", ListItemCss);
			writer.Write(HtmlTextWriter.TagRightChar);
		}

		writer.WriteBeginTag("a");
		writer.WriteAttribute("class", CssClass);

		if (UseRelNoFollow)
		{
			writer.WriteAttribute("rel", "nofollow");
		}

		writer.WriteAttribute("href", Page.ResolveUrl(registrationLink));
		writer.Write(HtmlTextWriter.TagRightChar);

		if (!string.IsNullOrWhiteSpace(OverrideText))
		{
			writer.WriteEncodedText(OverrideText);
		}
		else
		{
			writer.WriteEncodedText(Resource.RegisterLink);
		}

		writer.WriteEndTag("a");

		if (RenderAsListItem)
		{
			writer.WriteEndTag("li");
		}
	}


	private string GetRegistrationLink(SiteSettings siteSettings)
	{
		string? returnUrl;

		if (AutoDetectReturnUrl && Page is not Pages.Register)
		{
			returnUrl = Page.Request.RawUrl;
		}
		else
		{
			returnUrl = Page.Request.Params.Get("returnurl");

			if (Page is Pages.Register || Page is Pages.ConfirmRegistration)
			{
				// no need to render it on register page
				returnUrl = null;
			}
		}

		var registrationLink = PageUrlService.GetRegisterLink(returnUrl);

		return registrationLink;
	}
}
