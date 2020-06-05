using log4net;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Globalization;

namespace mojoPortal.Web.UI.Pages
{
	public partial class LoginPage : NonCmsBasePage
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(LoginPage));

		private string rpxApiKey = string.Empty;
		private string rpxApplicationName = string.Empty;
		private string returnUrlCookieName = string.Empty;

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Load += new EventHandler(this.Page_Load);
			this.AppendQueryStringToAction = false;

			rpxApiKey = siteSettings.RpxNowApiKey;
			rpxApplicationName = siteSettings.RpxNowApplicationName;

			if (WebConfigSettings.UseOpenIdRpxSettingsFromWebConfig)
			{
				if (WebConfigSettings.OpenIdRpxApiKey.Length > 0)
				{
					rpxApiKey = WebConfigSettings.OpenIdRpxApiKey;
				}

				if (WebConfigSettings.OpenIdRpxApplicationName.Length > 0)
				{
					rpxApplicationName = WebConfigSettings.OpenIdRpxApplicationName;
				}
			}

			returnUrlCookieName = "returnurl" + siteSettings.SiteId.ToInvariantString();

			if ((WebConfigSettings.EnableOpenIdAuthentication) && (siteSettings.AllowOpenIdAuth))
			{
				pnlOpenID.Visible = true;
				OpenIdLoginControl oidLogin = (OpenIdLoginControl)Page.LoadControl("~/Controls/OpenIDLoginControl.ascx");
				oidLogin.ID = "oidLogin";
				pnlOpenID.Controls.Add(oidLogin);

				AddClassToBody("openid");
			}

			if (rpxApiKey.Length > 0)
			{
				if ((!WebConfigSettings.DisableRpxAuthentication))
				{
					pnlOpenID.Visible = true;
					OpenIdRpxNowLink rpxNowLink = new OpenIdRpxNowLink();
					pnlOpenID.Controls.Add(rpxNowLink);

					AddClassToBody("janrain");

				}
			}

			if (WebConfigSettings.HideMenusOnLoginPage)
			{ SuppressAllMenus(); }
		}

		private void Page_Load(object sender, EventArgs e)
		{
			if (SiteUtils.SslIsAvailable())
				SiteUtils.ForceSsl();
			SecurityHelper.DisableBrowserCache();

			if (Request.IsAuthenticated)
			{
				string returnUrlParam = Page.Request.Params.Get("returnurl");

				if (!String.IsNullOrEmpty(returnUrlParam) && !returnUrlParam.ToLower().Contains("/accessdenied.aspx"))
				{
					returnUrlParam = SecurityHelper.RemoveMarkup(Page.Server.UrlDecode(returnUrlParam));


					string redirectUrl = Page.ResolveUrl(returnUrlParam);
					if (
						((redirectUrl.StartsWith("/")) && (!(redirectUrl.StartsWith("//"))))
						|| (redirectUrl.StartsWith(SiteRoot))
						|| (redirectUrl.StartsWith(SiteRoot.Replace("https://", "http://"))))
					{
						WebUtils.SetupRedirect(this, returnUrlParam);
						return;
					}
				}

				// user is logged in
				WebUtils.SetupRedirect(this, SiteRoot + "/Default.aspx");
				return;
			}

			PopulateLabels();

			login1.SetFocus = true;

			if (siteSettings.LoginInfoTop.Length > 0)
			{
				pnlTopContent.Visible = true;
				litTopContent.Text = siteSettings.LoginInfoTop;
			}

			if (siteSettings.LoginInfoBottom.Length > 0)
			{
				pnlBottomContent.Visible = true;
				litBottomContent.Text = siteSettings.LoginInfoBottom;
			}

			SetupReturnUrlCookie();

			if (siteSettings.DisableDbAuth)
			{ pnlStandardLogin.Visible = false; }
		}

		private void SetupReturnUrlCookie()
		{
			if (Page.IsPostBack)
			{ return; }

			string returnUrl = string.Empty;

			if (Page.Request.UrlReferrer != null)
			{
				string urlReferrer = Page.Request.UrlReferrer.ToString();
				if ((urlReferrer.StartsWith(SiteRoot)) || (urlReferrer.StartsWith(SiteRoot.Replace("https://", "http://"))))
				{
					returnUrl = urlReferrer;

				}
			}

			string returnUrlParam = Page.Request.Params.Get("returnurl");

			if (!String.IsNullOrEmpty(returnUrlParam))
			{
				returnUrlParam = SecurityHelper.RemoveMarkup(Page.Server.UrlDecode(returnUrlParam));
				string redirectUrl = Page.ResolveUrl(returnUrlParam);
				if ((redirectUrl.StartsWith(SiteRoot)) || (redirectUrl.StartsWith(SiteRoot.Replace("https://", "http://"))))
				{
					returnUrl = redirectUrl;
				}
			}

			if (returnUrl.Length > 0)
			{ CookieHelper.SetCookie(returnUrlCookieName, returnUrl); }
		}

		private void PopulateLabels()
		{

			Title = SiteUtils.FormatPageTitle(siteSettings, Resource.LoginLink);
			litHeading.Text = string.Format(coreDisplaySettings.DefaultPageHeaderMarkup, Resource.SignInLabel);
			MetaDescription = string.Format(CultureInfo.InvariantCulture, Resource.MetaDescriptionSignInPageFormat, siteSettings.SiteName);

			AddClassToBody("loginpage");
		}
	}
}