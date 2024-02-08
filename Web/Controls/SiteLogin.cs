using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.UserSignInHandlers;
using mojoPortal.Net;
using mojoPortal.Web.Controls;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI;

public class SiteLogin : Login
{
	private static readonly ILog log = LogManager.GetLogger(typeof(SiteLogin));

	private readonly SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
	private readonly string siteRoot = SiteUtils.GetNavigationSiteRoot();

	public bool SetRedirectUrl { get; set; } = true;


	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);

		Load += new EventHandler(SiteLogin_Load);
		LoginError += new EventHandler(SiteLogin_LoginError);
		LoggingIn += new LoginCancelEventHandler(SiteLogin_LoggingIn);
		LoggedIn += new EventHandler(SiteLogin_LoggedIn);

		CreateUserText = Resource.SignInRegisterLinkText;
		CreateUserUrl = siteRoot + "/Secure/Register.aspx";
		FailureText = ResourceHelper.GetMessageTemplate("LoginFailedMessage.config");
		LoginButtonText = Resource.SignInLinkText;
		PasswordRecoveryText = Resource.SignInSendPasswordButton;
		PasswordRecoveryUrl = siteRoot + "/Secure/RecoverPassword.aspx";
		RememberMeText = Resource.SignInSendRememberMeLabel;
		RememberMeSet = WebConfigSettings.ForcePersistentAuthCheckboxChecked;
		RenderOuterTable = false;
		CssClass = string.Empty;
	}


	void SiteLogin_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			ViewState["LoginErrorCount"] = 0;

			if (Page.Request.UrlReferrer != null)
			{
				string urlReferrer = Page.Request.UrlReferrer.ToString();

				if (urlReferrer.StartsWith(siteRoot) || urlReferrer.StartsWith(siteRoot.Replace("https://", "http://")))
				{
					ViewState["ReturnUrl"] = urlReferrer;
				}
			}

			string returnUrlParam = Page.Request.Params.Get("returnurl");

			if (!String.IsNullOrEmpty(returnUrlParam))
			{
				returnUrlParam = SecurityHelper.RemoveMarkup(returnUrlParam);

				string redirectUrl = Page.ResolveUrl(SecurityHelper.RemoveMarkup(Page.Server.UrlDecode(returnUrlParam)));

				if (
					redirectUrl.StartsWith("/") &&
					!redirectUrl.StartsWith("//") ||
					redirectUrl.StartsWith(siteRoot) ||
					redirectUrl.StartsWith(siteRoot.Replace("https://", "http://"))
				)
				{
					ViewState["ReturnUrl"] = redirectUrl;
				}
			}
		}

		if (SetRedirectUrl)
		{
			DestinationPageUrl = GetRedirectPath();
		}

		if (WebConfigSettings.DebugLoginRedirect)
		{
			log.Info($"Login redirect url was {DestinationPageUrl} for Site Root {siteRoot}");
		}
	}


	protected void SiteLogin_LoginError(object sender, EventArgs e)
	{
		var errorCount = (int)ViewState["LoginErrorCount"] + 1;

		ViewState["LoginErrorCount"] = errorCount;

		if (
			siteSettings != null &&
			!siteSettings.UseLdapAuth &&
			siteSettings.PasswordFormat != 1 &&
			siteSettings.AllowPasswordRetrieval &&
			errorCount >= siteSettings.MaxInvalidPasswordAttempts &&
			PasswordRecoveryUrl != String.Empty
		)
		{
			WebUtils.SetupRedirect(this, PasswordRecoveryUrl);
		}
	}


	void SiteLogin_LoggingIn(object sender, LoginCancelEventArgs e)
	{
		if (siteSettings.RequireCaptchaOnLogin)
		{
			var captcha = (CaptchaControl)FindControl("captcha");

			if (captcha != null)
			{
				// if (!captcha.Captcha.IsValid)
				if (!captcha.IsValid)
				{
					e.Cancel = true;
					return;
				}
			}

		}

		var siteUser = new SiteUser(siteSettings, UserName);

		if (siteUser.UserId > -1)
		{
			if (siteSettings.UseSecureRegistration && siteUser.RegisterConfirmGuid != Guid.Empty)
			{
				var lblFailure = (Label)FindControl("FailureText");

				if (lblFailure != null)
				{
					lblFailure.Visible = true;
					lblFailure.Text = Resource.LoginUnconfirmedEmailMessage;

				}

				// send email with confirmation link that will approve profile
				Notification.SendRegistrationConfirmationLink(
					SiteUtils.GetSmtpSettings(),
					ResourceHelper.GetMessageTemplate("RegisterConfirmEmailMessage.config"),
					siteSettings.DefaultEmailFromAddress,
					siteSettings.DefaultFromEmailAlias,
					siteUser.Email,
					siteSettings.SiteName,
					$"{WebUtils.GetSiteRoot()}/ConfirmRegistration.aspx?ticket={siteUser.RegisterConfirmGuid}"
				);

				// user has not confirmed
				e.Cancel = true;

				return;
			}

			if (siteUser.EmailMfaGuid != Guid.Empty)
			{
				var lblFailure = (Label)FindControl("FailureText");

				if (lblFailure != null)
				{
					lblFailure.Visible = true;
					lblFailure.Text = Resource.LoginUnconfirmedEmailMfaMessage;

				}

				var emailMfaLoginLink = new EmailMessageTask(SiteUtils.GetSmtpSettings())
				{
					EmailTo = siteUser.Email,
					SiteGuid = siteSettings.SiteGuid,
					
					//$"{WebUtils.GetSiteRoot()}/ConfirmRegistration.aspx?ticket={siteUser.RegisterConfirmGuid}"
				};

				// send email with confirmation link that will approve profile
				Notification.SendRegistrationConfirmationLink(
					SiteUtils.GetSmtpSettings(),
					ResourceHelper.GetMessageTemplate("RegisterConfirmEmailMessage.config"),
					siteSettings.DefaultEmailFromAddress,
					siteSettings.DefaultFromEmailAlias,
					siteUser.Email,
					siteSettings.SiteName,
					$"{WebUtils.GetSiteRoot()}/ConfirmRegistration.aspx?ticket={siteUser.RegisterConfirmGuid}"
				);

				// user has not confirmed
				e.Cancel = true;

				return;
			}

			if (siteUser.IsDeleted)
			{
				var lblFailure = (Label)FindControl("FailureText");

				if (lblFailure != null)
				{
					lblFailure.Visible = true;
					lblFailure.Text = ResourceHelper.GetMessageTemplate("LoginFailedMessage.config");
				}

				e.Cancel = true;

				return;
			}

			if (siteUser.IsLockedOut)
			{
				var lblFailure = (Label)FindControl("FailureText");

				if (lblFailure != null)
				{
					lblFailure.Visible = true;
					lblFailure.Text = Resource.LoginAccountLockedMessage;
				}

				e.Cancel = true;

				return;
			}

			if (siteSettings.RequireApprovalBeforeLogin && !siteUser.ApprovedForLogin)
			{
				var lblFailure = (Label)FindControl("FailureText");

				if (lblFailure != null)
				{
					lblFailure.Visible = true;
					lblFailure.Text = Resource.LoginNotApprovedMessage;
				}

				e.Cancel = true;

				return;
			}

			if (siteSettings.MaxInvalidPasswordAttempts > 0)
			{
				if (siteUser.FailedPasswordAttemptCount >= siteSettings.MaxInvalidPasswordAttempts)
				{
					if (siteUser.FailedPasswordAttemptWindowStart.AddMinutes(siteSettings.PasswordAttemptWindowMinutes) > DateTime.UtcNow)
					{

						var lblFailure = (Label)FindControl("FailureText");

						if (lblFailure != null)
						{
							lblFailure.Visible = true;
							lblFailure.Text = Resource.AccountLockedTemporarilyDueToPasswordFailures;
						}

						e.Cancel = true;

						return;
					}
				}
			}
		}
	}


	protected void SiteLogin_LoggedIn(object sender, EventArgs e)
	{
		if (siteSettings == null)
		{
			return;
		}

		var siteUser = new SiteUser(siteSettings, UserName);

		if (WebConfigSettings.UseFolderBasedMultiTenants)
		{
			var cookieName = $"siteguid{siteSettings.SiteGuid}";

			CookieHelper.SetCookie(cookieName, siteUser.UserGuid.ToString(), RememberMeSet);
		}

		if (siteUser.UserId > -1 && siteSettings.AllowUserSkins && siteUser.Skin.Length > 0)
		{
			SiteUtils.SetSkinCookie(siteUser);
		}

		if (siteUser.UserGuid == Guid.Empty)
		{
			return;
		}

		// track user ip address
		try
		{
			var userLocation = new UserLocation(siteUser.UserGuid, SiteUtils.GetIP4Address())
			{
				SiteGuid = siteSettings.SiteGuid,
				Hostname = Page.Request.UserHostName
			};

			userLocation.Save();
		}
		catch (Exception ex)
		{
			log.Error(SiteUtils.GetIP4Address(), ex);
		}

		var u = new UserSignInEventArgs(siteUser);

		OnUserSignIn(u);
	}


	#region Events

	protected void OnUserSignIn(UserSignInEventArgs e)
	{
		foreach (UserSignInHandlerProvider handler in UserSignInHandlerProviderManager.Providers)
		{
			handler.UserSignInEventHandler(null, e);
		}
	}

	#endregion


	private string GetRedirectPath()
	{
		var redirectPath = WebConfigSettings.PageToRedirectToAfterSignIn;

		if (redirectPath.Length > 0)
		{
			return redirectPath;
		}

		string defaultRedirect = siteRoot;
		if (
			!siteSettings.IsServerAdminSite &&
			WebConfigSettings.UseFolderBasedMultiTenants &&
			WebConfigSettings.AppendDefaultPageToFolderRootUrl
		)
		{
			defaultRedirect += "/Default.aspx";
		}

		if (ViewState["ReturnUrl"] != null)
		{
			redirectPath = ViewState["ReturnUrl"].ToString();
		}

		if (
			string.IsNullOrEmpty(redirectPath) ||
			redirectPath.Contains("AccessDenied") ||
			redirectPath.Contains("Login") ||
			redirectPath.Contains("SignIn") ||
			redirectPath.Contains("ConfirmRegistration.aspx") ||
			redirectPath.Contains("OpenIdRpxHandler.aspx") ||
			redirectPath.Contains("RecoverPassword.aspx") ||
			redirectPath.Contains("Register")
		)
		{
			redirectPath = defaultRedirect;
		}

		if (Page.Request.Params["r"] == "h")
		{
			redirectPath = defaultRedirect;
		}

		if (WebHelper.IsSecureRequest())
		{
			if (redirectPath.StartsWith("http:"))
			{
				redirectPath = redirectPath.Replace("http:", "https:");
			}
		}

		return redirectPath;
	}
}
