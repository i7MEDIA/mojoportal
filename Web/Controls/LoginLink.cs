using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using Resources;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI;

public class LoginLink : WebControl
{
	public bool UseLeftSeparator { get; set; } = false;
	public string LeftSeparatorImageUrl { get; set; } = string.Empty;
	public bool RenderAsListItem { get; set; } = false;
	public string ListItemCss { get; set; } = "topnavitem";
	public string OverrideText { get; set; } = string.Empty;
	public bool UseRelNoFollow { get; set; } = true;


	protected override void OnLoad(System.EventArgs e)
	{
		base.OnLoad(e);
		EnableViewState = false;

		if (Page.Request.IsAuthenticated)
		{
			Visible = false;

			return;
		}
	}


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
		if (Page.Request.IsAuthenticated)
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
				writer.Write($"""<img class="accent" alt="" src="{Page.ResolveUrl(LeftSeparatorImageUrl)}" border="0" /> """);
			}
			else
			{
				writer.Write("""<span class="accent">|</span>""");
			}
		}

		var currentPage = CacheHelper.GetCurrentPage();
		var loginUrl = PageUrlService.GetLoginLink();

		if (!currentPage.HideAfterLogin)
		{
			if (
				Page is not Pages.LoginPage &&
				Page is not Pages.ConfirmRegistration &&
				Page is not Pages.RecoverPassword &&
				Page is not Pages.AccessDeniedPage
			)
			{
				var redirectLink = PageUrlService.GetLoginRedirectLink();

				if (!string.IsNullOrWhiteSpace(redirectLink))
				{
					loginUrl = PageUrlService.GetLoginLink(redirectLink);
				}
				else
				{
					loginUrl = PageUrlService.GetLoginLink(Page.Request.RawUrl);
				}
			}

			if (Page is Pages.ConfirmRegistration)
			{
				var returnUrlParam = Page.Request.Params.Get("returnurl");

				if (!string.IsNullOrWhiteSpace(returnUrlParam))
				{
					loginUrl = PageUrlService.GetLoginLink(returnUrlParam);
				}
			}
		}
		else
		{
			loginUrl += "?r=h";
		}

		if (CssClass.Length == 0)
		{
			CssClass = "sitelink";
		}

		writer.WriteBeginTag("a");
		writer.WriteAttribute("class", CssClass);

		if (UseRelNoFollow)
		{
			writer.WriteAttribute("rel", "nofollow");
		}


		writer.WriteAttribute("href", loginUrl);
		writer.Write(HtmlTextWriter.TagRightChar);

		if (OverrideText.Length > 0)
		{
			writer.WriteEncodedText(OverrideText);
		}
		else
		{
			writer.WriteEncodedText(Resource.LoginLink);
		}

		writer.WriteEndTag("a");

		if (RenderAsListItem)
		{
			writer.WriteEndTag("li");
		}
	}
}
