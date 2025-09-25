using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.UserSignInHandlers;
using mojoPortal.Net;
using mojoPortal.Web.Components;
using mojoPortal.Web.Extensions;
using mojoPortal.Web.Framework;
using System;
using System.Globalization;
using System.Web.Security;
using System.Web.UI;
using ConsentToken = mojoPortal.Web.WindowsLiveLogin.ConsentToken;

namespace mojoPortal.Web;

public partial class WindowsLiveAuthHandler : Page
{
	private static readonly ILog log = LogManager.GetLogger(typeof(WindowsLiveAuthHandler));

	const string WindowsLiveSecurityAlgorithm = "wsignin1.0";
	private readonly string LoginPage = PageUrlService.GetLoginLink();
	private string winliveCookieName;
	static WindowsLiveLogin windowsLive = null;
	private WindowsLiveLogin.User user = null;
	private SiteSettings siteSettings;
	private string siteRoot;
	private string returnUrlCookieName;
	private bool persistCookie = false;

	private WindowsLiveMessenger messengerApplication = null;
	private const string consentTokenCookie = "msgr-consent-token";
	private const string delegationTokenCookie = "msgr-delegation-token";

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		//HookupSignInEventHandlers();
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();

		if (windowsLive == null)
		{
			WebUtils.SetupRedirect(this, siteRoot);
			return;
		}

		if (Request.Params["ConsentToken"] != null)
		{
			if (WebConfigSettings.DebugWindowsLive)
			{
				log.Info("ConsentToken was not null");
			}
			HandleConsent();
			return;
		}

		if (Request.Params["Result"] != null)
		{
			if (WebConfigSettings.DebugWindowsLive)
			{
				log.Info("Result was" + Request.Params["Result"]);
			}
			HandleResult();
			return;
		}

		DoValidation();
	}

	private void HandleResult()
	{
		string result = Request.Params["Result"];

		if (result == "Accepted")
		{
			if (Request.Params["Id"] != null)
			{
				if (WebConfigSettings.DebugWindowsLive)
				{
					log.Info("HandleResult Id was " + Request.Params["Id"]);
				}

				if (!Request.IsAuthenticated)
				{
					WebUtils.SetupRedirect(this, siteRoot);
					return;
				}

				SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
				if (currentUser != null && currentUser.UserGuid != Guid.Empty)
				{
					if (WebConfigSettings.DebugWindowsLive)
					{
						log.Info("HandleResult obtained siteUser");
					}
					currentUser.LiveMessengerId = Request.Params["Id"];
					currentUser.Save();

					WebUtils.SetupRedirect(this, siteRoot + "/Secure/UserProfile.aspx");
					return;
				}
			}
		}

		if (WebConfigSettings.DebugWindowsLive)
		{
			log.Info("HandleResult redirecting to site root");
		}

		WebUtils.SetupRedirect(this, siteRoot);
	}

	private void HandleConsent()
	{
		//http://msdn.microsoft.com/en-us/library/cc287661.aspx
		ConsentToken consent;
		messengerApplication = new WindowsLiveMessenger(windowsLive);

		if (string.IsNullOrWhiteSpace(Request.Params["ConsentToken"]))
		{
			if (WebConfigSettings.DebugWindowsLive)
			{
				log.Info("usertoken was null");
			}

			consent = messengerApplication.HandleConsentResponse(Request.Params);
		}
		else
		{
			if (WebConfigSettings.DebugWindowsLive)
			{
				log.Info("usertoken was not null");
			}
			consent = messengerApplication.DecodeToken(Request.Params["ConsentToken"]);
		}

		//the windows live id on the siteUser is I think the same thing as usertoken
		//it just needs to be decoded

		if (consent is null)
		{
			if (WebConfigSettings.DebugWindowsLive)
			{
				log.Info("HandleConsentCompleted consent was null");
			}
			WebUtils.SetupRedirect(this, siteRoot);
			return;
		}

		if (WebConfigSettings.DebugWindowsLive)
		{
			if (consent.IsValid())
			{
				log.Info("HandleConsent obtained valid consent");
			}
			else
			{
				log.Info("HandleConsent obtained invalid consent");
			}
		}

		CookieHelper.SetCookie(consentTokenCookie, consent.Token);
		CookieHelper.SetCookie(delegationTokenCookie, consent.DelegationToken);

		SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
		if (currentUser is not null && currentUser.UserGuid != Guid.Empty)
		{
			if (WebConfigSettings.DebugWindowsLive)
			{
				log.Info("HandleConsent obtained siteUser");
			}

			currentUser.LiveMessengerId = consent.CID;
			currentUser.LiveMessengerDelegationToken = consent.Token;

			currentUser.Save();

			if (WebConfigSettings.DebugWindowsLive)
			{
				log.Info($"HandleConsent saved CID {consent.CID} for user {currentUser.Email} {currentUser.Name}");
			}

			WebUtils.SetupRedirect(this, siteRoot + "/Secure/UserProfile.aspx");
			return;
		}

		if (WebConfigSettings.DebugWindowsLive)
		{
			log.Info("HandleConsent redirecting to site root");
		}

		WebUtils.SetupRedirect(this, siteRoot);
		return;
	}

	private void DoValidation()
	{
		var action = Request.QueryString.Get("action");

		if (WebConfigSettings.DebugWindowsLive)
		{
			log.Info($"action was {action}");
		}
		/*
              If action is 'logout', clear the login cookie and redirect
              to the logout page.

              If action is 'clearcookie', clear the login cookie and
              return a GIF as response to signify success.

              By default, try to process a login. If login was
              successful, cache the user token in a cookie and redirect
             If login failed, clear the cookie and redirect 
            */

		if (action == "logout")
		{
			CookieHelper.ExpireCookie(winliveCookieName);
			WebUtils.SetupRedirect(this, siteRoot + "/Logoff.aspx");
			return;
		}
		else if (action == "delauth")
		{
			HandleConsent();
			return;
		}
		else if (action == "clearcookie")
		{
			CookieHelper.ExpireCookie(winliveCookieName);

			windowsLive.GetClearCookieResponse(out string type, out byte[] content);
			Response.ContentType = type;
			Response.OutputStream.Write(content, 0, content.Length);
			Response.End();
		}
		else
		{
			// action is login
			user = windowsLive.ProcessLogin(Request.Form);
			var userGuid = Guid.Empty;
			if (user != null)
			{
				// auth succeeded see if its a current mojo user or not
				// emtpy guid means new user
				userGuid = SiteUser.GetUserGuidFromWindowsLiveId(siteSettings.SiteId, user.Id);
				persistCookie = user.UsePersistentCookie;

				CookieHelper.SetCookie(winliveCookieName, user.Token, user.UsePersistentCookie);

				if (userGuid == Guid.Empty)
				{
					// WindowsLiveID Authentication succeeded
					// no mojo user found so send to Register
					//WebUtils.SetupRedirect(this, "RegisterWithWindowsLiveID.aspx");

					if (siteSettings.AllowNewRegistration)
					{
						WebUtils.SetupRedirect(this, siteRoot + "/Secure/RegisterWithWindowsLiveID.aspx");
						return;
					}

					WebUtils.SetupRedirect(this, siteRoot);
					return;
				}
				else
				{
					//WebUtils.SetupRedirect(
					//    this,
					//    SiteUtils.GetNavigationSiteRoot());
					DoExistingUserLogic(userGuid);
					return;
				}
			}
			else
			{
				// auth failed so clear the cookie
				CookieHelper.ExpireCookie(winliveCookieName);
				Response.Redirect(LoginPage);
				return;
			}
		}
	}

	private void DoExistingUserLogic(Guid userGuid)
	{
		// user found so login if allowed
		var user = new SiteUser(siteSettings, userGuid);

		bool canLogin = true;

		if (siteSettings.UseSecureRegistration && user.RegisterConfirmGuid != Guid.Empty)
		{
			Notification.SendRegistrationConfirmationLink(
				SiteUtils.GetSmtpSettings(),
				ResourceHelper.GetMessageTemplate("RegisterConfirmEmailMessage.config"),
				siteSettings.DefaultEmailFromAddress,
				siteSettings.DefaultFromEmailAlias,
				user.Email,
				siteSettings.SiteName,
				WebUtils.GetSiteRoot() + "/ConfirmRegistration.aspx?ticket=" +
				user.RegisterConfirmGuid.ToString());

			log.Info($"User {user.Name} tried to login but email address is not confirmed.");

			canLogin = false;
		}

		if (user.IsLockedOut)
		{
			log.Info($"User {user.Name} tried to login but account is locked.");
			canLogin = false;
		}

		if (siteSettings.RequireApprovalBeforeLogin && !user.ApprovedForLogin)
		{
			log.Info($"User {user.Name} tried to login but account is not approved yet.");
			canLogin = false;
		}

		if (canLogin)
		{
			if (siteSettings.UseEmailForLogin)
			{
				FormsAuthentication.SetAuthCookie(user.Email, persistCookie);
			}
			else
			{
				FormsAuthentication.SetAuthCookie(user.LoginName, persistCookie);
			}

			if (user.LiveMessengerDelegationToken.Length > 0)
			{
				var m = new WindowsLiveMessenger(windowsLive);
				var token = m.DecodeToken(user.LiveMessengerDelegationToken);
				token = m.RefreshConsent(token);
				if (token is not null)
				{
					CookieHelper.SetCookie(consentTokenCookie, token.Token);
					CookieHelper.SetCookie(delegationTokenCookie, token.DelegationToken);
				}
			}

			if (WebConfigSettings.UseFolderBasedMultiTenants)
			{
				var cookieName = $"siteguid{siteSettings.SiteGuid}";
				CookieHelper.SetCookie(cookieName, user.UserGuid.ToString(), persistCookie);
			}

			if (user.UserId > -1 && siteSettings.AllowUserSkins && user.Skin.Length > 0)
			{
				SiteUtils.SetSkinCookie(user);
			}

			user.TrackUserActivity();

			string redirectUrl = GetRedirectPath();
			CookieHelper.ExpireCookie(returnUrlCookieName);

			var u = new UserSignInEventArgs(user);
			OnUserSignIn(u);

			Response.Redirect(redirectUrl);
			return;
		}
		else
		{
			// redirect to login
			// need to make login page show
			// reason for failure
			//WebUtils.SetupRedirect(this, LoginPage);
			Response.Redirect(LoginPage);
		}
	}

	private string GetRedirectPath()
	{
		var redirectPath = string.Empty;

		if (CookieHelper.CookieExists(returnUrlCookieName))
		{
			redirectPath = CookieHelper.GetCookieValue(returnUrlCookieName);
		}

		if (
			string.IsNullOrWhiteSpace(redirectPath) ||
			redirectPath.Contains("AccessDenied", StringComparison.OrdinalIgnoreCase) ||
			redirectPath.Contains("Login", StringComparison.OrdinalIgnoreCase) ||
			redirectPath.Contains("SignIn", StringComparison.OrdinalIgnoreCase) ||
			redirectPath.Contains("ConfirmRegistration.aspx", StringComparison.OrdinalIgnoreCase) ||
			redirectPath.Contains("Register", StringComparison.OrdinalIgnoreCase)
		)
		{
			return SiteUtils.GetNavigationSiteRoot();
		}

		return redirectPath;
	}

	#region Events

	//private void HookupSignInEventHandlers()
	//{
	//    // this is a hook so that custom code can be fired when pages are created
	//    // implement a PageCreatedEventHandlerPovider and put a config file for it in
	//    // /Setup/ProviderConfig/pagecreatedeventhandlers
	//    try
	//    {
	//        foreach (UserSignInHandlerProvider handler in UserSignInHandlerProviderManager.Providers)
	//        {
	//            this.UserSignIn += handler.UserSignInEventHandler;
	//        }
	//    }
	//    catch (TypeInitializationException ex)
	//    {
	//        log.Error(ex);
	//    }

	//}

	//public event UserSignInEventHandler UserSignIn;

	protected void OnUserSignIn(UserSignInEventArgs e)
	{
		foreach (UserSignInHandlerProvider handler in UserSignInHandlerProviderManager.Providers)
		{
			handler.UserSignInEventHandler(null, e);
		}

		//if (UserSignIn != null)
		//{
		//    UserSignIn(this, e);
		//}
	}

	#endregion

	private void LoadSettings()
	{
		siteSettings = CacheHelper.GetCurrentSiteSettings();
		siteRoot = SiteUtils.GetNavigationSiteRoot();
		winliveCookieName = $"winliveid{siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)}";
		returnUrlCookieName = $"ret{siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)}";
		windowsLive = WindowsLiveHelper.GetWindowsLiveLogin();
	}
}