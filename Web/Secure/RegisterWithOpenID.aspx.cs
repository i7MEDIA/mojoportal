using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Web.Security;
using System.Web.UI.WebControls;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.UserRegisteredHandlers;
using mojoPortal.Net;
using mojoPortal.Web.Configuration;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI.Pages;

public partial class RegisterWithOpenId : NonCmsBasePage
{
	private static readonly ILog log = LogManager.GetLogger(typeof(RegisterWithOpenId));

	private Double timeOffset = 0;
	private TimeZoneInfo timeZone = null;
	private Collection<mojoProfilePropertyDefinition> requiredProfileProperties = [];
	private string openidCookieName;
	private string openIdEmailCookieName;
	private string openIdFullNameCookieName;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!WebConfigSettings.EnableOpenIdAuthentication
			|| !siteSettings.AllowOpenIdAuth
			|| !siteSettings.AllowNewRegistration
			)
		{
			WebUtils.SetupRedirect(this, SiteRoot);
			return;
		}

		if (SiteUtils.SslIsAvailable())
		{
			SiteUtils.ForceSsl();
		}

		SecurityHelper.DisableBrowserCache();

		// overriding base page default here
		EnableViewState = true;

		LoadSettings();
		PopulateLabels();

		if (Request.IsAuthenticated)
		{
			pnlRegisterWrapper.Visible = false;
			pnlAuthenticated.Visible = true;
			return;
		}

		PopulateRequiredProfileControls();
	}


	/// <summary>
	/// This event fires upon successfull authentication from
	/// open id provider.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OpenIdLogin1_LoggedIn(object sender, OpenIdEventArgs e)
	{
		// prevent the base control from 
		// setting forms auth cookie for us
		// we'll login after further verification
		e.Cancel = true;

		Guid userGuid = SiteUser.GetUserGuidFromOpenId(siteSettings.SiteId, e.ClaimedIdentifier.ToString());

		if (userGuid == Guid.Empty)
		{
			// this is expected result for new users
			// user not found in db by open id
			DoNewUserLogic(e);
		}
		else
		{
			// user found so just login
			DoExistingUserLogic(userGuid);
		}
	}

	private void DoNewUserLogic(OpenIdEventArgs e)
	{
		if (e == null) { return; }

		ClaimsResponse claim = e.Response.GetExtension<ClaimsResponse>();

		if (claim == null) { return; }

		if (IsValidForUserCreation(e, claim))
		{
			if (SiteUser.EmailExistsInDB(siteSettings.SiteId, claim.Email))
			{
				// show message that user should login and associate 
				// their open id account on their profile page.
				lblError.Text = Resource.OpenIDRegisterUserEmailExistsMessage;
			}
			else
			{
				// create user automagically since we have all 
				// the needed data
				string loginName = SecurityHelper.RemoveMarkup(e.ClaimedIdentifier.ToString().Replace("http://", string.Empty).Replace("https://", string.Empty).Replace("/", string.Empty));

				CreateUser(
					e.ClaimedIdentifier.ToString(),
					claim.Email,
					loginName,
					SecurityHelper.RemoveMarkup(claim.FullName));

				return;
			}
		}
		else
		{
			// prompt user to enter needed fields
			CookieHelper.SetSecureCookie(openidCookieName, e.ClaimedIdentifier.ToString());
			if (
				claim != null
				&& claim.Email != null
				&& claim.Email.Length > 3
				&& Email.IsValidEmailAddressSyntax(claim.Email)
				)
			{
				CookieHelper.SetSecureCookie(openIdEmailCookieName, claim.Email);
				divEmailInput.Visible = false;
				divEmailDisplay.Visible = true;
				litEmail.Text = claim.Email;
			}
			else
			{
				divEmailInput.Visible = true;
				divEmailDisplay.Visible = false;
			}

			if (
				claim != null
				&& claim.FullName != null
				&& claim.FullName.Length > 0
				)
			{
				CookieHelper.SetSecureCookie(openIdFullNameCookieName, SecurityHelper.RemoveMarkup(claim.FullName));
			}

			pnlNeededProfileProperties.Visible = true;
			pnlOpenID.Visible = false;

			if (e.ClaimedIdentifier != null)
			{
				litOpenIDURI.Text = e.ClaimedIdentifier.ToString();
			}

			litInfoNeededMessage.Text = Resource.OpenIDAdditionalInfoNeededMessage;
		}
	}

	void btnCreateUser_Click(object sender, EventArgs e)
	{
		Page.Validate("profile");
		if (Page.IsValid)
		{
			//PopulateRequiredProfileControls();
			string openID = CookieHelper.GetSecureCookieValue(openidCookieName);
			string email = txtEmail.Text;

			if (CookieHelper.CookieExists(openIdEmailCookieName) && email.Length == 0)
			{
				email = CookieHelper.GetSecureCookieValue(openIdEmailCookieName);
			}

			if (openID.Length == 0)
			{
				return;
			}

			string loginName = openID.Replace("http://", string.Empty).Replace("https://", string.Empty).Replace("/", string.Empty);

			string name = loginName;

			if (CookieHelper.CookieExists(openIdFullNameCookieName))
			{
				name = CookieHelper.GetSecureCookieValue(openIdFullNameCookieName);
			}

			if (SiteUser.EmailExistsInDB(siteSettings.SiteId, email))
			{
				lblError.Text = Resource.RegisterDuplicateEmailMessage;
			}
			else
			{
				CreateUser(openID, email, SecurityHelper.RemoveMarkup(loginName), SecurityHelper.RemoveMarkup(name));
			}
		}
	}

	private void CreateUser(
		string openId,
		string email,
		string loginName,
		string name)
	{
		var newUser = new SiteUser(siteSettings)
		{
			Email = email
		};

		if (loginName.Length > 50)
		{
			loginName = loginName.Substring(0, 50);
		}

		int i = 1;
		while (SiteUser.LoginExistsInDB(siteSettings.SiteId, loginName))
		{
			loginName += i.ToString();
			if (loginName.Length > 50) loginName = loginName.Remove(40, 1);
			i++;
		}
		if (string.IsNullOrWhiteSpace(name))
		{
			name = loginName;
		}

		newUser.LoginName = loginName;
		newUser.Name = name;

		var mojoMembership = (mojoMembershipProvider)Membership.Provider;
		newUser.Password = mojoMembership.EncodePassword(siteSettings, newUser, SiteUser.CreateRandomPassword(7, WebConfigSettings.PasswordGeneratorChars));
		newUser.PasswordQuestion = Resource.ManageUsersDefaultSecurityQuestion;
		newUser.PasswordAnswer = Resource.ManageUsersDefaultSecurityAnswer;
		newUser.OpenIdUri = openId;
		newUser.Save();

		if (siteSettings.UseSecureRegistration)
		{
			newUser.SetRegistrationConfirmationGuid(Guid.NewGuid());
		}

		var profileConfig = mojoProfileConfiguration.GetConfig();

		// set default values first
		foreach (var propertyDefinition in profileConfig.PropertyDefinitions)
		{
			mojoProfilePropertyDefinition.SavePropertyDefault(newUser, propertyDefinition);
		}

		foreach (var propertyDefinition in profileConfig.PropertyDefinitions)
		{
			if (propertyDefinition.RequiredForRegistration || propertyDefinition.ShowOnRegistration)
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
		var userLocation = new UserLocation(newUser.UserGuid, SiteUtils.GetIP4Address())
		{
			SiteGuid = siteSettings.SiteGuid,
			Hostname = Page.Request.UserHostName
		};
		userLocation.Save();

		var u = new UserRegisteredEventArgs(newUser);
		OnUserRegistered(u);

		CacheHelper.ClearMembershipStatisticsCache();

		NewsletterHelper.ClaimExistingSubscriptions(newUser);

		DoUserLogin(newUser);
	}

	#region Events

	private void HookupRegistrationEventHandlers()
	{
		// this is a hook so that custom code can be fired when pages are created
		// implement a PageCreatedEventHandlerPovider and put a config file for it in
		// /Setup/ProviderConfig/pagecreatedeventhandlers
		try
		{
			foreach (UserRegisteredHandlerProvider handler in UserRegisteredHandlerProviderManager.Providers)
			{
				UserRegistered += handler.UserRegisteredHandler;
			}
		}
		catch (TypeInitializationException ex)
		{
			log.Error(ex);
		}
	}

	public event UserRegistreredEventHandler UserRegistered;

	protected void OnUserRegistered(UserRegisteredEventArgs e)
	{
		UserRegistered?.Invoke(this, e);
	}

	#endregion

	private void DoUserLogin(SiteUser siteUser)
	{
		if (siteSettings.UseSecureRegistration && siteUser.RegisterConfirmGuid != Guid.Empty)
		{
			Notification.SendRegistrationConfirmationLink(
				SiteUtils.GetSmtpSettings(),
				ResourceHelper.GetMessageTemplate("RegisterConfirmEmailMessage.config"),
				siteSettings.DefaultEmailFromAddress,
				siteSettings.DefaultFromEmailAlias,
				siteUser.Email,
				siteSettings.SiteName,
				SiteRoot + "/ConfirmRegistration.aspx?ticket=" +
				siteUser.RegisterConfirmGuid.ToString());

			lblError.Text = Resource.RegistrationRequiresEmailConfirmationMessage;
			pnlNeededProfileProperties.Visible = false;
			pnlOpenID.Visible = false;
		}
		else
		{
			if (siteUser.IsLockedOut)
			{
				lblError.Text = Resource.LoginAccountLockedMessage;
			}
			else
			{
				if (siteSettings.UseEmailForLogin)
				{
					FormsAuthentication.SetAuthCookie(
						siteUser.Email, true);
				}
				else
				{
					FormsAuthentication.SetAuthCookie(
						siteUser.LoginName, true);
				}

				WebUtils.SetupRedirect(this, SiteRoot + "/Secure/UserProfile.aspx");
			}
		}
	}

	private void DoExistingUserLogic(Guid userGuid)
	{
		// really shouldn't hit this code unless the user has
		// forgotten that they already registered or is
		// purposefully trying to register again with the same account
		// if allowed just login and send to profile
		var user = new SiteUser(siteSettings, userGuid);
		DoUserLogin(user);
	}

	private bool IsValidForUserCreation(OpenIdEventArgs e, ClaimsResponse claim)
	{
		if (e == null
			|| claim == null
			|| e.ClaimedIdentifier == null
			|| string.IsNullOrWhiteSpace(claim.Email)
			|| string.IsNullOrWhiteSpace(claim.FullName)
			|| !Email.IsValidEmailAddressSyntax(claim.Email))
		{
			return false;
		}

		// if custom profile fields are required
		// must pass them on to registration page
		mojoProfileConfiguration profileConfig = mojoProfileConfiguration.GetConfig();
		if (profileConfig != null)
		{
			if (profileConfig.HasRequiredCustomProperties())
			{
				return false;
			}
		}

		return true;
	}

	void OpenIdLogin1_Failed(object sender, OpenIdEventArgs e)
	{
		lblLoginFailed.Visible = true;
	}

	protected void OpenIdLogin1_Canceled(object sender, OpenIdEventArgs e)
	{
		lblLoginCanceled.Visible = true;
	}

	private void PopulateRequiredProfileControls()
	{
		foreach (mojoProfilePropertyDefinition propertyDefinition in requiredProfileProperties)
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

	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.RegisterWithOpenIDLink);
		MetaDescription = string.Format(CultureInfo.InvariantCulture, Resource.MetaDescriptionOpenIDRegistrationPageFormat, siteSettings.SiteName);

		litAlreadyAuthenticated.Text = Resource.AlreadyRegisteredMessage;
		lblError.Text = string.Empty;
		lblLoginFailed.Text = Resource.OpenIDRegistrationFailedMessage;
		lblLoginCanceled.Text = Resource.OpenIDRegistrationCanceledMessage;
		OpenIdLogin1.ButtonText = Resource.OpenIDRegisterButton;
		OpenIdLogin1.ButtonToolTip = Resource.OpenIDRegisterButton;
		OpenIdLogin1.ExamplePrefix = Resource.OpenIDExamplePrefix;
		OpenIdLogin1.ExampleUrl = Resource.OpenIDExampleUrl;
		OpenIdLogin1.ButtonFontBold = true;
		EmailRequired.ErrorMessage = Resource.RegisterEmailRequiredMessage;

		divAgreement.Controls.Add(new Literal
		{
			Text = ResourceHelper.GetMessageTemplate("RegisterLicense.config")
		});

		btnCreateUser.Text = Resource.RegisterButton;
		regexEmail.ErrorMessage = Resource.RegisterEmailRegexMessage;
	}

	/// <summary>
	/// This is only for testing/development to see the response 
	/// from open id provider
	/// </summary>
	/// <param name="e"></param>
	private void DisplayResults(OpenIdEventArgs e, ClaimsResponse claim)
	{
		litResult.Text = @$"uri: {e.ClaimedIdentifier}<br />
Full Name: {claim.FullName}<br />
Nickname: {claim.Nickname}<br />
Birthdate: {claim.BirthDate}<br />
Country: {claim.Country}<br />
Culture: {claim.Culture}<br />
Gender: {claim.Gender}<br />
Language: {claim.Language}<br />
MailAddress: {claim.MailAddress}<br />
Postal Code: {claim.PostalCode} <br />
Email: {claim.Email}<br />
TimeZone: {claim.TimeZone}<br />";
	}

	private void LoadSettings()
	{
		timeOffset = SiteUtils.GetUserTimeOffset();
		timeZone = SiteUtils.GetUserTimeZone();

		var profileConfig = mojoProfileConfiguration.GetConfig();

		foreach (var propertyDefinition in profileConfig.PropertyDefinitions)
		{
			if (propertyDefinition.RequiredForRegistration || propertyDefinition.ShowOnRegistration)
			{
				requiredProfileProperties.Add(propertyDefinition);
			}
		}

		openidCookieName = $"openid{siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)}";
		openIdEmailCookieName = $"openidemail{siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)}";
		openIdFullNameCookieName = $"openidname{siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)}";

		AddClassToBody("registeropenidpage");
	}

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);

		AppendQueryStringToAction = false;
		btnCreateUser.Click += new EventHandler(btnCreateUser_Click);
		OpenIdLogin1.LoggedIn += new EventHandler<OpenIdEventArgs>(OpenIdLogin1_LoggedIn);
		OpenIdLogin1.Failed += new EventHandler<OpenIdEventArgs>(OpenIdLogin1_Failed);
		OpenIdLogin1.Canceled += new EventHandler<OpenIdEventArgs>(OpenIdLogin1_Canceled);

		OpenIdLogin1.RequestEmail = DemandLevel.Request;
		OpenIdLogin1.RequestFullName = DemandLevel.Request;
		OpenIdLogin1.RequestTimeZone = DemandLevel.Request;
		OpenIdLogin1.RequestGender = DemandLevel.Request;
		OpenIdLogin1.RequestLanguage = DemandLevel.Request;
		OpenIdLogin1.RequestNickname = DemandLevel.Request;
		OpenIdLogin1.RequestPostalCode = DemandLevel.Request;
		OpenIdLogin1.RequestCountry = DemandLevel.Request;
		OpenIdLogin1.RequestBirthDate = DemandLevel.Request;
		OpenIdLogin1.EnableRequestProfile = true;

		if (Global.SkinConfig.MenuOptions.HideOnRegister)
		{
			SuppressAllMenus();
		}

		HookupRegistrationEventHandlers();
	}
}