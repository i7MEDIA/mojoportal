using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.UserRegisteredHandlers;
using mojoPortal.Business.WebHelpers.UserSignInHandlers;
using mojoPortal.Net;
using mojoPortal.Web.Configuration;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI;

/// <summary>
/// Handles redirect from rpxnow.com authentication, receives and process the auth token
/// </summary>
public partial class OpenIdRpxHandlerPage : NonCmsBasePage
{
	private static readonly ILog log = LogManager.GetLogger(typeof(OpenIdRpxHandlerPage));

	private string tokenUrl = string.Empty;
	private string authToken = string.Empty;
	private string rpxApiKey = string.Empty;
	private string rpxBaseUrl = string.Empty;
	private Double timeOffset = 0;
	private TimeZoneInfo timeZone = null;
	private Collection<mojoProfilePropertyDefinition> requiredProfileProperties = [];

	private string returnUrlCookieName = string.Empty;
	private string returnUrl = string.Empty;
	private string termsOfUse = string.Empty;
	private List<LetterInfo> siteAvailableSubscriptions = null;
	protected bool IncludeDescriptionInList = true;
	private SubscriberRepository subscriptions = new SubscriberRepository();

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
		PopulateLabels();
		PopulateRequiredProfileControls();
		BindNewsletterList();

		if (authToken.Length == 0)
		{
			log.DebugFormat("openid-debug: authToken ('token') cookie empty ");

			if (Request.IsAuthenticated)
			{
				log.DebugFormat("openid-debug: user authenticated, don't need to check openid ");
				Response.Redirect(SiteRoot);

				return;
			}

			if (!IsPostBack)
			{
				log.DebugFormat("openid-debug: not postback, need to login ");
				Response.Redirect(SiteRoot + "/Secure/Login.aspx");
			}
		}
		else
		{
			log.Debug($"openid-debug: authToken ('token') value is \"{authToken}\" ");

			ProcessToken();
		}
	}


	private void BindNewsletterList()
	{
		if (!displaySettings.ShowNewsLetters) { return; }

		if (clNewsletters.Items.Count > 0) { return; }

		clNewsletters.DataSource = siteAvailableSubscriptions;
		clNewsletters.DataBind();

		if (clNewsletters.Items.Count == 0)
		{
			pnlSubscribe.Visible = false;
			return;
		}

		foreach (LetterInfo l in siteAvailableSubscriptions)
		{
			if (l.ProfileOptIn)
			{
				ListItem item = clNewsletters.Items.FindByValue(l.LetterInfoGuid.ToString());
				if (item != null) { item.Selected = true; }
			}
		}
	}

	private void ProcessToken()
	{
		OpenIdRpxHelper rpxHelper = new OpenIdRpxHelper(rpxApiKey, rpxBaseUrl);
		OpenIdRpxAuthInfo authInfo = rpxHelper.AuthInfo(authToken, tokenUrl);

		if ((authInfo == null) || (!authInfo.IsValid))
		{
			log.Debug($"openid-debug: authInfo is null or authInfo.IsValid='false' ");

			Response.Redirect(SiteRoot + "/Secure/Login.aspx");
			return;
		}

		if (Request.IsAuthenticated)
		{
			log.Debug($"openid-debug: authInfo is valid and user exists, authenticated ");
			HandleAuthenticatedUser(rpxHelper, authInfo);
			return;
		}

		Guid userGuid = Guid.Empty;
		SiteUser user = null;

		//first find a site user by email
		// this allows associating the openid user with an existing user.
		if ((authInfo.Email.Length > 0))
		{
			log.Debug($"openid-debug: found user by email ");

			user = SiteUser.GetByEmail(siteSettings, authInfo.Email);

		}

		if (authInfo.PrimaryKey.Length == 36)
		{
			try
			{
				userGuid = new Guid(authInfo.PrimaryKey);
			}
			catch (FormatException) { }
			catch (OverflowException) { }
		}

		if ((user == null) && (userGuid == Guid.Empty))
		{
			userGuid = SiteUser.GetUserGuidFromOpenId(
				siteSettings.SiteId,
				authInfo.Identifier);
		}

		if ((user == null) && (userGuid != Guid.Empty))
		{
			user = new SiteUser(siteSettings, userGuid);
			if (WebConfigSettings.UseRelatedSiteMode)
			{
				if (user.UserId == -1)
				{
					user = null;
					log.Debug($"openid-debug: user not found ");
				}
			}
			else if (user.SiteGuid != siteSettings.SiteGuid)
			{
				user = null;
				log.Debug($"openid-debug: user not connected to this site ({siteSettings.SiteId.ToString()}) ");
			}

		}

		if (user == null)
		{
			// not an existing user
			if (siteSettings.AllowNewRegistration)
			{
				HandleNewUser(rpxHelper, authInfo);
			}
			else
			{
				log.Debug($"openid-debug: user not found, AllowNewRegistrations='false' ");
				WebUtils.SetupRedirect(this, SiteRoot);
				return;

			}
		}
		else
		{
			log.Debug($"openid-debug: user found ({user.LoweredEmail}, {user.UserId.ToString()}) ");

			bool needToSave = false;
			if ((siteSettings.UseSecureRegistration) && (user.RegisterConfirmGuid != Guid.Empty))
			{
				if (authInfo.VerifiedEmail.Length > 0)
				{
					user.SetRegistrationConfirmationGuid(Guid.Empty);
					user.Email = authInfo.VerifiedEmail;
					needToSave = true;

				}

			}

			if (user.OpenIdUri.Length == 0)
			{
				user.OpenIdUri = authInfo.Identifier;
				needToSave = true;
			}

			if (needToSave) { user.Save(); }

			if (WebConfigSettings.OpenIdRpxUseMappings)
			{
				if ((authInfo.PrimaryKey.Length == 0) || (authInfo.PrimaryKey != user.UserGuid.ToString()))
				{
					rpxHelper.Map(authInfo.Identifier, user.UserGuid.ToString());
				}
			}


			SignInUser(user, false);

		}

	}

	private void SignInUser(SiteUser user, bool isNewUser)
	{
		if (
			(siteSettings.UseSecureRegistration)
			&& (user.RegisterConfirmGuid != Guid.Empty)
			)
		{

			Notification.SendRegistrationConfirmationLink(
				SiteUtils.GetSmtpSettings(),
				ResourceHelper.GetMessageTemplate("RegisterConfirmEmailMessage.config"),
				siteSettings.DefaultEmailFromAddress,
				siteSettings.DefaultFromEmailAlias,
				user.Email,
				siteSettings.SiteName,
				SiteRoot + "/ConfirmRegistration.aspx?ticket=" +
				user.RegisterConfirmGuid.ToString());


			log.Info("User " + user.Name + " tried to login but email address is not confirmed.");

			lblError.Text = Resource.RegistrationRequiresEmailConfirmationMessage;
			litInfoNeededMessage.Visible = false;
			pnlRequiredProfileProperties.Visible = false;
			btnCreateUser.Visible = false;

			return;

		}

		if (user.IsLockedOut)
		{

			log.Info("User " + user.Name + " tried to login but account is locked.");

			lblError.Text = Resource.LoginAccountLockedMessage;

			return;
		}

		if ((siteSettings.RequireApprovalBeforeLogin) && (!user.ApprovedForLogin))
		{

			log.Info("User " + user.Name + " tried to login but account is not approved yet.");

			lblError.Text = Resource.LoginNotApprovedMessage;

			return;
		}


		if (siteSettings.UseEmailForLogin)
		{
			FormsAuthentication.SetAuthCookie(user.Email, true);
		}
		else
		{
			FormsAuthentication.SetAuthCookie(user.LoginName, true);
		}

		if (WebConfigSettings.UseFolderBasedMultiTenants)
		{
			string cookieName = "siteguid" + siteSettings.SiteGuid;
			CookieHelper.SetCookie(cookieName, user.UserGuid.ToString(), true);
		}

		if (user.UserId > -1 && siteSettings.AllowUserSkins && user.Skin.Length > 0)
		{
			SiteUtils.SetSkinCookie(user);
		}

		user.UpdateLastLoginTime();

		// track user ip address
		UserLocation userLocation = new UserLocation(user.UserGuid, SiteUtils.GetIP4Address());
		userLocation.SiteGuid = siteSettings.SiteGuid;
		userLocation.Hostname = Request.UserHostName;
		userLocation.Save();

		UserSignInEventArgs u = new UserSignInEventArgs(user);
		OnUserSignIn(u);

		if (CookieHelper.CookieExists(returnUrlCookieName))
		{
			returnUrl = CookieHelper.GetCookieValue(returnUrlCookieName);
			CookieHelper.ExpireCookie(returnUrlCookieName);
		}
		string requestedReturnUrl = SiteUtils.GetReturnUrlParam(Page, SiteRoot);
		returnUrl = requestedReturnUrl;

		if (isNewUser)
		{

			if (WebConfigSettings.PageToRedirectToAfterRegistration.Length > 0)
			{
				returnUrl = SiteRoot + WebConfigSettings.PageToRedirectToAfterRegistration;
			}
		}

		if (String.IsNullOrEmpty(returnUrl) ||
			returnUrl.Contains("AccessDenied") ||
			returnUrl.Contains("Login") ||
			returnUrl.Contains("SignIn") ||
			returnUrl.Contains("ConfirmRegistration.aspx") ||
			returnUrl.Contains("OpenIdRpxHandler.aspx") ||
			returnUrl.Contains("RecoverPassword.aspx") ||
			returnUrl.Contains("Register")
			)
		{
			returnUrl = SiteRoot;
		}

		if (returnUrl.Length > 0)
		{
			if (mojoPortal.Core.Helpers.WebHelper.IsSecureRequest())
			{
				if (returnUrl.StartsWith("http:"))
				{
					returnUrl = returnUrl.Replace("http:", "https:");
				}
			}

			WebUtils.SetupRedirect(this, returnUrl);
			return;

		}

		if (mojoPortal.Core.Helpers.WebHelper.IsSecureRequest())
		{
			if (SiteRoot.StartsWith("http:"))
			{
				WebUtils.SetupRedirect(this, SiteRoot.Replace("http:", "https:"));
				return;
			}
		}


		WebUtils.SetupRedirect(this, SiteRoot);
		return;

	}

	private void HandleAuthenticatedUser(OpenIdRpxHelper rpxHelper, OpenIdRpxAuthInfo authInfo)
	{
		// user is already authenticated so must be updating open id in profile

		SiteUser currentUser = SiteUtils.GetCurrentSiteUser();

		if (currentUser == null)
		{
			Response.Redirect(SiteRoot);
			return;
		}

		log.Debug($"openid-debug: user already authenticated, updating openID in profile ({currentUser.LoweredEmail}, {currentUser.UserId.ToString()}) ");

		rpxHelper.Map(authInfo.Identifier, currentUser.UserGuid.ToString());

		currentUser.OpenIdUri = authInfo.Identifier;
		currentUser.Save();

		Response.Redirect(SiteRoot + "/Secure/UserProfile.aspx?t=i");


	}

	private void HandleNewUser(OpenIdRpxHelper rpxHelper, OpenIdRpxAuthInfo authInfo)
	{
		log.Debug($"openid-debug: adding user ");

		if (!IsValidForUserCreation(authInfo))
		{
			PromptForNeededInfo(rpxHelper, authInfo);
			return;

		}

		string loginName = string.Empty;

		if ((authInfo.PreferredUsername.Length > 0) && (!SiteUser.LoginExistsInDB(siteSettings.SiteId, authInfo.PreferredUsername)))
		{
			loginName = SecurityHelper.RemoveMarkup(authInfo.PreferredUsername);
		}

		if (loginName.Length == 0) { loginName = SiteUtils.SuggestLoginNameFromEmail(siteSettings.SiteId, authInfo.Email); }

		string name = loginName;

		if (authInfo.DisplayName.Length > 0)
		{
			name = SecurityHelper.RemoveMarkup(authInfo.DisplayName);
		}

		bool emailIsVerified = (authInfo.VerifiedEmail == authInfo.Email);

		SiteUser newUser = CreateUser(
				authInfo.Identifier,
				authInfo.Email,
				loginName,
				name,
				emailIsVerified);

		log.Debug($"openid-debug: user created ({newUser.LoweredEmail}, {newUser.UserId.ToString()}) ");

		SignInUser(newUser, true);

	}

	private void PromptForNeededInfo(OpenIdRpxHelper rpxHelper, OpenIdRpxAuthInfo authInfo)
	{

		if (Email.IsValidEmailAddressSyntax(authInfo.Email))
		{
			divEmailInput.Visible = false;
			divEmailDisplay.Visible = true;
			litEmail.Text = authInfo.Email;
			hdnEmail.Value = authInfo.Email;
			//email is verified go ahead and track new registration in analytics
			//or we won't have another opportunity to track it 

			//TODO: implement analytics tracking for new registrations

			//if (authInfo.VerifiedEmail.Length > 0)
			//{
			//	AnalyticsAsyncTopScript asyncAnalytics = Page.Master.FindControl("analyticsTop") as AnalyticsAsyncTopScript;
			//	if (asyncAnalytics != null)
			//	{
			//		asyncAnalytics.PageToTrack = "/RegistrationConfirmed.aspx";
			//	}
			//	else
			//	{
			//		mojoGoogleAnalyticsScript analytics = Page.Master.FindControl("mojoGoogleAnalyticsScript1") as mojoGoogleAnalyticsScript;
			//		if (analytics != null)
			//		{
			//			analytics.PageToTrack = "/RegistrationConfirmed.aspx";
			//		}
			//	}
			//}
		}
		else
		{
			divEmailInput.Visible = true;
			divEmailDisplay.Visible = false;
		}

		pnlNeededProfileProperties.Visible = true;
		pnlSubscribe.Visible = displaySettings.ShowNewsLetters;
		pnlOpenID.Visible = false;

		litOpenIDURI.Text = authInfo.Identifier;
		hdnIdentifier.Value = authInfo.Identifier;
		hdnPreferredUsername.Value = authInfo.PreferredUsername;
		hdnDisplayName.Value = authInfo.DisplayName;

		if (authInfo.ProviderName.Length > 0)
		{
			litHeading.Text = string.Format(CultureInfo.InvariantCulture, Resource.RpxRegistrationHeadingFormat, authInfo.ProviderName);
		}

		//PopulateRequiredProfileControls();
		pnlRequiredProfileProperties.Visible = true;


		litInfoNeededMessage.Text = Resource.OpenIDAdditionalInfoNeededMessage;

		if (termsOfUse.Length > 0)
		{
			Literal agreement = new Literal();
			agreement.Text = termsOfUse;
			divAgreement.Controls.Add(agreement);

		}
		else
		{
			chkAgree.Visible = false;
		}


	}

	void MustAgree_ServerValidate(object source, ServerValidateEventArgs args)
	{
		if (chkAgree != null)
		{
			args.IsValid = chkAgree.Checked;
		}
		else
		{
			args.IsValid = false;
		}

	}

	private void SetupScript()
	{
		if (WebConfigSettings.DisablejQuery) { return; }

		//StringBuilder script = new StringBuilder();
		//script.Append("\n <script type=\"text/javascript\"> ");

		//script.Append("function CheckBoxRequired_ClientValidate(sender, e) {");
		//script.Append("e.IsValid = $('#" + chkAgree.ClientID + "').is(':checked'); }");

		//script.Append("\n </script>");

		string script = $@"<script type=""text/javascript"">
				function CheckBoxRequired_ClientValidate(sender, e)
				{{
					e.IsValid = $('#{chkAgree.ClientID}').is(':checked');
				}}
		        </script>";

		Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "regval", script);
	}

	private void PopulateRequiredProfileControls()
	{
		//foreach (mojoProfilePropertyDefinition propertyDefinition in requiredProfileProperties)
		//{
		//    mojoProfilePropertyDefinition.SetupPropertyControl(
		//        this,
		//        pnlRequiredProfileProperties,
		//        propertyDefinition,
		//        timeOffset,
		//        timeZone,
		//        SiteRoot);
		//}

		mojoProfileConfiguration profileConfig = mojoProfileConfiguration.GetConfig();
		if (profileConfig != null)
		{
			foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
			{
				// we are using the new TimeZoneInfo list but it doesn't work under Mono
				// this makes us skip the TimeOffsetHours setting from mojoProfile.config which is not used under windows
				if (propertyDefinition.Name == mojoProfilePropertyDefinition.TimeOffsetHoursKey) { continue; }

				if ((propertyDefinition.RequiredForRegistration) || (propertyDefinition.ShowOnRegistration))
				{
					mojoProfilePropertyDefinition.SetupPropertyControl(
						this,
						pnlRequiredProfileProperties,
						propertyDefinition,
						timeOffset,
						timeZone,
						SiteRoot);
				}
			}
		}
	}

	void btnCreateUser_Click(object sender, EventArgs e)
	{
		Page.Validate("profile");
		if (!Page.IsValid) { return; }

		if (hdnIdentifier.Value.Length == 0)
		{   // form manipulation if this is missing
			Response.Redirect(SiteRoot + "/Secure/Register.aspx");
			return;
		}

		string email = txtEmail.Text;

		if (email.Length == 0)
		{
			if ((hdnEmail.Value.Length > 0) && (!SiteUser.EmailExistsInDB(siteSettings.SiteId, hdnEmail.Value)))
			{
				email = hdnEmail.Value;
			}

		}

		string loginName = string.Empty;

		if ((hdnPreferredUsername.Value.Length > 0) && (!SiteUser.LoginExistsInDB(siteSettings.SiteId, hdnPreferredUsername.Value)))
		{
			loginName = hdnPreferredUsername.Value;
		}

		if (loginName.Length == 0) { loginName = SiteUtils.SuggestLoginNameFromEmail(siteSettings.SiteId, email); }

		string name = loginName;

		if (hdnDisplayName.Value.Length > 0)
		{
			name = hdnDisplayName.Value;
		}


		if (SiteUser.EmailExistsInDB(siteSettings.SiteId, email))
		{
			lblError.Text = Resource.RegisterDuplicateEmailMessage;
		}
		else
		{
			bool emailIsVerified = false;
			SiteUser newUser = CreateUser(
				hdnIdentifier.Value,
				email,
				loginName,
				name,
				emailIsVerified);

			SignInUser(newUser, true);
		}


	}

	private SiteUser CreateUser(
		string openId,
		string email,
		string loginName,
		string name,
		bool emailIsVerified)
	{
		SiteUser newUser = new SiteUser(siteSettings);
		newUser.Email = email;

		if (loginName.Length > 50) loginName = loginName.Substring(0, 50);

		int i = 1;
		while (SiteUser.LoginExistsInDB(
			siteSettings.SiteId, loginName))
		{
			loginName += i.ToString();
			if (loginName.Length > 50) loginName = loginName.Remove(40, 1);
			i++;

		}
		if ((name == null) || (name.Length == 0)) name = loginName;
		newUser.LoginName = loginName;
		newUser.Name = name;
		//newUser.Password = SiteUser.CreateRandomPassword(7);
		mojoMembershipProvider mojoMembership = (mojoMembershipProvider)Membership.Provider;
		newUser.Password = mojoMembership.EncodePassword(siteSettings, newUser, SiteUser.CreateRandomPassword(7, WebConfigSettings.PasswordGeneratorChars));
		newUser.PasswordQuestion = Resource.ManageUsersDefaultSecurityQuestion;
		newUser.PasswordAnswer = Resource.ManageUsersDefaultSecurityAnswer;
		newUser.OpenIdUri = openId;
		newUser.Save();

		//test
		//emailIsVerified = false;

		if (siteSettings.UseSecureRegistration)
		{
			if (!emailIsVerified)
			{
				newUser.SetRegistrationConfirmationGuid(Guid.NewGuid());

			}
		}



		mojoProfileConfiguration profileConfig
			= mojoProfileConfiguration.GetConfig();

		// set default values first
		foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
		{
			// we are using the new TimeZoneInfo list but it doesn't work under Mono
			// this makes us skip the TimeOffsetHours setting from mojoProfile.config which is not used under windows
			if (propertyDefinition.Name == mojoProfilePropertyDefinition.TimeOffsetHoursKey) { continue; }
			mojoProfilePropertyDefinition.SavePropertyDefault(
				newUser, propertyDefinition);
		}

		foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
		{
			// we are using the new TimeZoneInfo list but it doesn't work under Mono
			// this makes us skip the TimeOffsetHours setting from mojoProfile.config which is not used under windows
			if (propertyDefinition.Name == mojoProfilePropertyDefinition.TimeOffsetHoursKey) { continue; }
			if ((propertyDefinition.RequiredForRegistration) || (propertyDefinition.ShowOnRegistration))
			{
				mojoProfilePropertyDefinition.SaveProperty(
					newUser,
					pnlRequiredProfileProperties,
					propertyDefinition,
					timeOffset,
					timeZone);
			}
		}

		// track user ip address
		UserLocation userLocation = new UserLocation(newUser.UserGuid, SiteUtils.GetIP4Address());
		userLocation.SiteGuid = siteSettings.SiteGuid;
		userLocation.Hostname = Page.Request.UserHostName;
		userLocation.Save();

		UserRegisteredEventArgs u = new UserRegisteredEventArgs(newUser);
		OnUserRegistered(u);

		CacheHelper.ClearMembershipStatisticsCache();

		// we'll map them next time they login
		//OpenIdRpxHelper rpxHelper = new OpenIdRpxHelper(rpxApiKey, rpxBaseUrl);
		//rpxHelper.Map(openId, newUser.UserGuid.ToString());

		DoSubscribe(newUser);

		NewsletterHelper.ClaimExistingSubscriptions(newUser);

		return newUser;



	}

	private void DoSubscribe(SiteUser siteUser)
	{
		foreach (LetterInfo available in siteAvailableSubscriptions)
		{
			ListItem item = clNewsletters.Items.FindByValue(available.LetterInfoGuid.ToString());
			if ((item != null) && (item.Selected))
			{
				DoSubscribe(available, siteUser);
			}
		}

		List<LetterSubscriber> memberSubscriptions = subscriptions.GetListByUser(siteUser.SiteGuid, siteUser.UserGuid);
		NewsletterHelper.RemoveDuplicates(memberSubscriptions);

	}

	private void DoSubscribe(LetterInfo letter, SiteUser siteUser)
	{

		LetterSubscriber s = new LetterSubscriber();
		s.SiteGuid = siteSettings.SiteGuid;
		s.EmailAddress = siteUser.Email;
		s.UserGuid = siteUser.UserGuid;
		s.LetterInfoGuid = letter.LetterInfoGuid;
		s.UseHtml = rbHtmlFormat.Checked;
		s.IsVerified = (!siteSettings.UseSecureRegistration);
		s.IpAddress = SiteUtils.GetIP4Address();
		subscriptions.Save(s);

		LetterInfo.UpdateSubscriberCount(s.LetterInfoGuid);

	}

	#region Events

	//private void HookupRegistrationEventHandlers()
	//{
	//    // this is a hook so that custom code can be fired when pages are created
	//    // implement a PageCreatedEventHandlerPovider and put a config file for it in
	//    // /Setup/ProviderConfig/pagecreatedeventhandlers
	//    try
	//    {
	//        foreach (UserRegisteredHandlerProvider handler in UserRegisteredHandlerProviderManager.Providers)
	//        {
	//            this.UserRegistered += handler.UserRegisteredHandler;
	//        }
	//    }
	//    catch (TypeInitializationException ex)
	//    {
	//        log.Error(ex);
	//    }

	//}

	//public event UserRegistreredEventHandler UserRegistered;

	protected void OnUserRegistered(UserRegisteredEventArgs e)
	{
		foreach (UserRegisteredHandlerProvider handler in UserRegisteredHandlerProviderManager.Providers)
		{
			handler.UserRegisteredHandler(null, e);
		}

		//if (UserRegistered != null)
		//{
		//    UserRegistered(this, e);
		//}
	}

	#endregion


	public event UserSignInEventHandler UserSignIn;

	protected void OnUserSignIn(UserSignInEventArgs e)
	{
		if (UserSignIn != null)
		{
			UserSignIn(this, e);
		}
	}

	private bool IsValidForUserCreation(OpenIdRpxAuthInfo authInfo)
	{
		bool result = true;

		if (authInfo == null) { return false; }

		if (String.IsNullOrEmpty(authInfo.Email)) { return false; }
		if (termsOfUse.Length > 0) { return false; }
		if ((displaySettings.ShowNewsLetters) && (siteAvailableSubscriptions.Count > 0)) { return false; }


		if (!Email.IsValidEmailAddressSyntax(authInfo.Email)) { return false; }

		mojoProfileConfiguration profileConfig = mojoProfileConfiguration.GetConfig();
		if (profileConfig.HasRequiredCustomProperties()) { result = false; }

		return result;

	}

	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.RegisterWithOpenIDLink);

		litHeading.Text = Resource.OpenIDRegistrationHeading;

		EmailRequired.ErrorMessage = Resource.RegisterEmailRequiredMessage;


		btnCreateUser.Text = Resource.RegisterButton;

		MustAgree.ServerValidate += new ServerValidateEventHandler(MustAgree_ServerValidate);
		MustAgree.ClientValidationFunction = "CheckBoxRequired_ClientValidate";
		MustAgree.ErrorMessage = Resource.TermsOfUseWarning;
		chkAgree.Text = Resource.TermsOfUseAgree;

		if (termsOfUse.Length > 0)
		{
			MustAgree.Enabled = true;
			SetupScript();
		}

		rbHtmlFormat.Text = Resource.NewsletterHtmlFormatLabel;
		rbPlainText.Text = Resource.NewsletterPlainTextFormatLabel;

	}

	private void LoadSettings()
	{
		if (Request.QueryString["token"] != null)
		{
			authToken = Request.QueryString["token"];
		}
		else if (Request.Form["token"] != null)
		{
			authToken = Request.Form["token"];
		}

		timeOffset = SiteUtils.GetUserTimeOffset();
		timeZone = SiteUtils.GetUserTimeZone();

		siteAvailableSubscriptions = NewsletterHelper.GetAvailableNewslettersForCurrentUser(siteSettings.SiteGuid);
		IncludeDescriptionInList = displaySettings.IncludeNewsletterDescriptionInList;
		pnlSubscribe.Visible = displaySettings.ShowNewsLetters;
		lblNewsletterListHeading.Text = displaySettings.NewsletterListHeading.Coalesce(Resource.NewsletterPreferencesHeader);

		tokenUrl = SiteRoot + "/Secure/OpenIdRpxHandler.aspx";
		rpxBaseUrl = "https://rpxnow.com";
		rpxApiKey = siteSettings.RpxNowApiKey;

		if (WebConfigSettings.UseOpenIdRpxSettingsFromWebConfig)
		{
			if (WebConfigSettings.OpenIdRpxApiKey.Length > 0)
			{
				rpxApiKey = WebConfigSettings.OpenIdRpxApiKey;
			}

		}


		regexEmail.ErrorMessage = Resource.RegisterEmailRegexMessage;

		returnUrlCookieName = "returnurl" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

		termsOfUse = siteSettings.RegistrationAgreement;
		if (termsOfUse.Length == 0)
		{
			termsOfUse = ResourceHelper.GetMessageTemplate("RegisterLicense.config");
		}

		AddClassToBody("rpxhandler");
	}




	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		this.Load += new EventHandler(this.Page_Load);
		btnCreateUser.Click += new EventHandler(btnCreateUser_Click);
#if NET35
            if (WebConfigSettings.DisablePageViewStateByDefault) {Page.EnableViewState = true; }
#endif

	}

	#endregion
}
