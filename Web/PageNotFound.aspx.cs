
using System;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web;

public partial class PageNotFoundPage : NonCmsBasePage
{
	protected string SiteNavigationRoot = SiteUtils.GetNavigationSiteRoot();
	protected string CultureCode = "en";

	protected void Page_Load(object sender, EventArgs e)
	{
		Response.StatusCode = 404;

		pnlGoogle404Enhancement.Visible = WebConfigSettings.EnableGoogle404Enhancement;
		if (BrowserHelper.IsIE()) { pnlGoogle404Enhancement.Visible = false; }

		Control registerLink = Page.Master.FindControl("RegisterLink");
		if (registerLink != null) { registerLink.Visible = false; }

		Control loginLink = Page.Master.FindControl("LoginLink");
		if (loginLink != null) { loginLink.Visible = false; }

		litErrorMessage.Text = $"{Resource.PageNotFoundMessage} {Resource.PageNotFoundPleaseTry} <a href=\"{SiteRoot}/SiteMap.aspx\" class=\"pnflink\">{Resource.SiteMapLink}</a>";

		Title = Resource.PageNotFoundTitle;

		if (WebConfigSettings.SuppressMenuOnBuiltIn404Page) { SuppressAllMenus(); }

		AddClassToBody("pagenotfound");
	}

	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		this.Load += new EventHandler(this.Page_Load);
		EnableViewState = false;

		if (Request.Params.Get("c") != null)
		{
			string culture = Request.Params.Get("c");
			try
			{
				CultureInfo requestCulture = CultureInfo.GetCultureInfo(culture);
				if ((requestCulture != null) && (!requestCulture.IsNeutralCulture))
				{
					Thread.CurrentThread.CurrentCulture = requestCulture;
					Thread.CurrentThread.CurrentUICulture = requestCulture;
					CultureCode = requestCulture.TwoLetterISOLanguageName;
				}
			}
			catch (ArgumentNullException) { }
			catch (ArgumentException) { }
		}
	}
	#endregion
}