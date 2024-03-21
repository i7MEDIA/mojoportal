using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using Resources;

namespace mojoPortal.Web.UI;

public partial class WelcomeMessage : WebControl
{
	public bool RenderAsListItem { get; set; } = false;
	public string ListItemCss { get; set; } = "topnavitem";
	public bool UseFirstLast { get; set; } = false;
	/// <summary>
	/// allows using first and last name in the welcome message, the default value is "Signed in as: {0} {1}"
	/// the {0} is required and will be replaced by the first name and {1} will be replaced by the last name
	/// However this is only useful if first and last name are actually populated
	/// which it may not be if you have not required it on registration and there are existing users
	/// also requires setting UseFirstLast to true
	/// </summary>
	public string FirstLastFormat { get; set; } = string.Empty;
	/// <summary>
	/// allows overriding the welcome message, the default value is "Signed in as: {0}"
	/// the {0} is required and will be replaced by the user name
	/// </summary>
	public string OverrideFormat { get; set; } = string.Empty;
	public bool WrapInAnchor { get; set; } = false;
	public bool WrapInProfileLink { get; set; } = false;

	protected override void Render(HtmlTextWriter writer)
	{
		if (HttpContext.Current == null)
		{
			writer.Write("[" + this.ID + "]");
			return;
		}

		DoRender(writer);


	}

	private void DoRender(HtmlTextWriter writer)
	{
		if (!HttpContext.Current.Request.IsAuthenticated) { return; }

		SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
		if ((siteUser == null) || (siteUser.UserId <= -1)) { return; }

		if (CssClass.Length == 0) CssClass = "sitelink";

		if (RenderAsListItem) writer.Write("<li class='" + ListItemCss + "'>");

		if ((UseFirstLast) && (siteUser.FirstName.Length > 0) && (siteUser.LastName.Length > 0))
		{
			if (FirstLastFormat.Length == 0) { FirstLastFormat = Resource.FirstLastFormat; }

			if (WrapInProfileLink)
			{
				writer.Write("<a class='" + CssClass + "' href='" + SiteUtils.GetNavigationSiteRoot() + "/Secure/UserProfile.aspx" + "'>" + string.Format(FirstLastFormat,
					HttpUtility.HtmlEncode(siteUser.FirstName), HttpUtility.HtmlEncode(siteUser.LastName)) + "</a>");
			}
			else if (WrapInAnchor)
			{
				writer.Write("<a class='" + CssClass + "' name='welcome'>" + string.Format(FirstLastFormat,
					HttpUtility.HtmlEncode(siteUser.FirstName), HttpUtility.HtmlEncode(siteUser.LastName)) + "</a>");
			}
			else
			{

				writer.Write("<span class='" + CssClass + "'>" + string.Format(FirstLastFormat,
					HttpUtility.HtmlEncode(siteUser.FirstName), HttpUtility.HtmlEncode(siteUser.LastName)) + "</span>");
			}

		}
		else
		{
			string format = Resource.WelcomeMessageFormat;

			if (OverrideFormat.Length > 0)
			{
				format = OverrideFormat;
			}

			if (WrapInProfileLink)
			{
				writer.Write($"<a class=\"{CssClass}\" href=\"{SiteUtils.GetNavigationSiteRoot()}/Secure/UserProfile.aspx\">{string.Format(format, HttpUtility.HtmlEncode(siteUser.Name), SiteUtils.GetPrivateProfileUrl(), Resource.ProfileLink)}</a>");
			}
			else if (WrapInAnchor)
			{
				writer.Write($"<a class=\"{CssClass}\" name=\"welcome\">{string.Format(format, HttpUtility.HtmlEncode(siteUser.Name), SiteUtils.GetPrivateProfileUrl(), Resource.ProfileLink)}</a>");
			}
			else
			{

				writer.Write($"<span class=\"{CssClass}\">{string.Format(format, HttpUtility.HtmlEncode(siteUser.Name), SiteUtils.GetPrivateProfileUrl(), Resource.ProfileLink)}</span>");
			}
		}

		if (RenderAsListItem)
		{
			writer.Write("</li>");
		}
	}
}