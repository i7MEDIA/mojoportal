using System;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using mojoPortal.Web.Components;
using Resources;

namespace mojoPortal.Web;

public partial class PageNotFoundPage : NonCmsBasePage
{
	protected string SiteNavigationRoot = SiteUtils.GetNavigationSiteRoot();
	protected string CultureCode = "en";
	private Models.PageNotFoundModel model;
	protected void Page_Load(object sender, EventArgs e)
	{
		Response.StatusCode = 404;
		model = new Models.PageNotFoundModel
		{
			CultureCode = CultureCode
		};

		if (Page.Master.FindControl("RegisterLink") is Control registerLink)
		{
			registerLink.Visible = false;
		}

		if (Page.Master.FindControl("LoginLink") is Control loginLink)
		{
			loginLink.Visible = false;
		}

		Title = Resource.PageNotFoundTitle;

		if (WebConfigSettings.SuppressMenuOnBuiltIn404Page)
		{
			SuppressAllMenus();
		}

		AddClassToBody("pagenotfound");

		litOutput.Text = RazorBridge.RenderPartialToString("PageNotFound", model, "Shared");

		//litErrorMessage.Text = $"{Resource.PageNotFoundMessage} {Resource.PageNotFoundPleaseTry} <a href=\"{SiteRoot}/SiteMap.aspx\" class=\"pnflink\">{Resource.SiteMapLink}</a>";



	}

	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		EnableViewState = false;

		if (Request.Params.Get("c") is string culture)
		{
			try
			{
				CultureInfo requestCulture = CultureInfo.GetCultureInfo(culture);
				if (requestCulture != null && !requestCulture.IsNeutralCulture)
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