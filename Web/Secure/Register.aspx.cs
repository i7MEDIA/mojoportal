using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.UserRegisteredHandlers;
using mojoPortal.Web.Components;
using mojoPortal.Web.Configuration;
using mojoPortal.Web.Controls;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI.Pages;


//http://stackoverflow.com/questions/4575214/how-to-change-the-layout-of-the-createuserwizard-control
//http://channel9.msdn.com/Shows/HanselminutesOn9/Hanselminutes-on-9-ASPNET-4-and-Marcin-Dobosz-on-New-Markup-from-Old-Controls
//http://www.velocityreviews.com/forums/t122608-createuserwizard-ignores-startnavigationtemplate.html
//http://stackoverflow.com/questions/1077121/added-to-createuserwizard-control-additional-wizard-steps-after-createuserwiz

public class LayoutTemplate : ITemplate
{
	// this gets rid of the default rendering with tables
	void ITemplate.InstantiateIn(Control container)
	{
		container.Controls.Add(new PlaceHolder() { ID = "wizardStepPlaceholder" });
		container.Controls.Add(new PlaceHolder() { ID = "navigationPlaceholder" });
	}
}

public partial class Register : NonCmsBasePage
{
	private static readonly ILog log = LogManager.GetLogger(typeof(Register));

	private string rpxApiKey = string.Empty;
	private string rpxApplicationName = string.Empty;
	private Panel pnlProfile;
	private Double timeOffset = 0;
	private TimeZoneInfo timeZone = null;
	private bool showWindowsLive = false;
	private bool showOpenId = false;
	private bool showRpx = false;
	private CheckBox chkAgree = null;
	private CaptchaControl captcha = null;
	private List<LetterInfo> siteAvailableSubscriptions = null;
	private CheckBoxList clNewsletters = null;
	private Panel pnlSubscribe = null;
	private RadioButton rbHtmlFormat = null;
	private RadioButton rbPlainText = null;
	protected bool IncludeDescriptionInList = true;
	private SubscriberRepository subscriptions = new SubscriberRepository();


	protected override void OnPreInit(EventArgs e)
	{
		base.OnPreInit(e);

		AppendQueryStringToAction = false;

		RegisterUser.LayoutTemplate = new LayoutTemplate(); //this gets rid of the outer table in .NET 4
															//RegisterUser.StartNavigationTemplate = new StartNavigationTemplate();
	}

	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);

		if (AppConfig.OAuth.Configured)
		{
			WebUtils.SetupRedirect(this, PageUrlService.GetRegisterLink());
			return;
		}

		Load += new EventHandler(Page_Load);
		RegisterUser.CreatingUser += new LoginCancelEventHandler(RegisterUser_CreatingUser);
		RegisterUser.CreatedUser += new EventHandler(RegisterUser_CreatedUser);

		if (Global.SkinConfig.MenuOptions.HideOnRegister)
		{
			SuppressAllMenus();
		}
	}
	#endregion

	private void Page_Load(object sender, EventArgs e)
	{
		if (
			!string.IsNullOrWhiteSpace(AppConfig.RegistrationLink) &&
			WebConfigSettings.RedirectRegistrationPageToCustomPage
		)
		{
			WebUtils.SetupRedirect(this, PageUrlService.GetRegisterLink());

			return;
		}

		if (SiteUtils.SslIsAvailable())
		{
			SiteUtils.ForceSsl();
		}

		SecurityHelper.DisableBrowserCache();

		if (!siteSettings.AllowNewRegistration)
		{
			WebUtils.SetupRedirect(this, SiteRoot);
			return;
		}

		LoadSettings();
		PopulateLabels();
		SetupScript();

		if (Request.IsAuthenticated)
		{
			pnlRegisterWrapper.Visible = false;
			pnlAuthenticated.Visible = true;
			return;
		}

		PopulateRequiredProfileControls();
		if (!IsPostBack)
		{
			BindNewsletterList();
			SetInitialFocus();
		}
	}

	private void BindNewsletterList()
	{
		if (!displaySettings.ShowNewsLetters)
		{
			return;
		}

		if (clNewsletters == null)
		{
			return;
		}

		clNewsletters.RepeatLayout = RepeatLayout.UnorderedList;
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
				if (item is not null)
				{
					item.Selected = true;
				}
			}
		}
	}

	private void SetInitialFocus()
	{
		// if custom profile properties are above then it is strange to set focus to a field further down the page
		if (!WebConfigSettings.ShowCustomProfilePropertiesAboveManadotoryRegistrationFields)
		{
			if (siteSettings.UseEmailForLogin && WebConfigSettings.AutoGenerateAndHideUserNamesWhenUsingEmailForLogin)
			{
				var txtEmail = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("Email");
				txtEmail?.Focus();
			}
			else
			{
				var txtUserName = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName");
				txtUserName?.Focus();
			}
		}
	}

	private void PopulateRequiredProfileControls()
	{
		if (pnlProfile != null)
		{
			mojoProfileConfiguration profileConfig = mojoProfileConfiguration.GetConfig();
			if (profileConfig != null)
			{
				foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
				{

					// we are using the new TimeZoneInfo list but it doesn't work under Mono
					// this makes us skip the TimeOffsetHours setting from mojoProfile.config which is not used under windows
					if (propertyDefinition.Name == mojoProfilePropertyDefinition.TimeOffsetHoursKey)
					{
						continue;
					}

					if (propertyDefinition.RequiredForRegistration || propertyDefinition.ShowOnRegistration)
					{
						if (propertyDefinition.Name == mojoProfilePropertyDefinition.TimeZoneIdKey && propertyDefinition.DefaultValue.Length == 0)
						{
							propertyDefinition.DefaultValue = siteSettings.TimeZoneId;
						}

						mojoProfilePropertyDefinition.SetupPropertyControl(
							this,
							pnlProfile,
							propertyDefinition,
							timeOffset,
							timeZone,
							SiteRoot);
					}
				}
			}
		}
	}

	void RegisterUser_CreatingUser(object sender, LoginCancelEventArgs e)
	{
		Page.Validate("profile");
		if (!Page.IsValid)
		{
			e.Cancel = true;
		}

		if (siteSettings.RequireCaptchaOnRegistration && captcha is not null)
		{
			if (!captcha.IsValid)
			{
				e.Cancel = true;
			}
		}
	}

	void RegisterUser_CreatedUser(object sender, EventArgs e)
	{
		var txtEmail = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("Email");
		var txtUserName = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName");

		if (txtEmail == null)
		{
			return;
		}

		if (txtUserName == null)
		{
			return;
		}

		SiteUser siteUser;

		if (siteSettings.UseEmailForLogin)
		{
			siteUser = new SiteUser(siteSettings, txtEmail.Text);
		}
		else
		{
			siteUser = new SiteUser(siteSettings, txtUserName.Text);
		}

		if (siteUser.UserId == -1)
		{
			return;
		}

		if (pnlProfile != null)
		{
			var profileConfig = mojoProfileConfiguration.GetConfig();

			// set default values first
			foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
			{
				mojoProfilePropertyDefinition.SavePropertyDefault(siteUser, propertyDefinition);
			}

			foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
			{
				if (propertyDefinition.RequiredForRegistration || propertyDefinition.ShowOnRegistration)
				{
					mojoProfilePropertyDefinition.SaveProperty(
						siteUser,
						pnlProfile,
						propertyDefinition,
						timeOffset,
						timeZone);
				}
			}
		}

		// track user ip address
		var userLocation = new UserLocation(siteUser.UserGuid, SiteUtils.GetIP4Address())
		{
			SiteGuid = siteSettings.SiteGuid,
			Hostname = Page.Request.UserHostName
		};
		userLocation.Save();

		CacheHelper.ClearMembershipStatisticsCache();

		if (!siteSettings.UseSecureRegistration
			&& (!siteSettings.RequireApprovalBeforeLogin || siteUser.ApprovedForLogin)
			)
		{
			if (siteSettings.UseEmailForLogin)
			{
				FormsAuthentication.SetAuthCookie(siteUser.Email, false);
			}
			else
			{
				FormsAuthentication.SetAuthCookie(siteUser.LoginName, false);
			}

			if (WebConfigSettings.UseFolderBasedMultiTenants)
			{
				string cookieName = $"siteguid{siteSettings.SiteGuid}";
				CookieHelper.SetCookie(cookieName, siteUser.UserGuid.ToString(), false);
			}

			siteUser.UpdateLastLoginTime();
		}

		DoSubscribe(siteUser);

		var u = new UserRegisteredEventArgs(siteUser);
		OnUserRegistered(u);
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

		var memberSubscriptions = subscriptions.GetListByUser(siteUser.SiteGuid, siteUser.UserGuid);
		NewsletterHelper.RemoveDuplicates(memberSubscriptions);
	}

	private void DoSubscribe(LetterInfo letter, SiteUser siteUser)
	{
		var subscriber = new LetterSubscriber
		{
			SiteGuid = siteSettings.SiteGuid,
			EmailAddress = siteUser.Email,
			UserGuid = siteUser.UserGuid,
			LetterInfoGuid = letter.LetterInfoGuid,
			UseHtml = rbHtmlFormat.Checked,
			IsVerified = (!siteSettings.UseSecureRegistration),
			IpAddress = SiteUtils.GetIP4Address()
		};
		subscriptions.Save(subscriber);

		LetterInfo.UpdateSubscriberCount(subscriber.LetterInfoGuid);
	}

	#region Events

	protected void OnUserRegistered(UserRegisteredEventArgs e)
	{
		foreach (UserRegisteredHandlerProvider handler in UserRegisteredHandlerProviderManager.Providers)
		{
			handler.UserRegisteredHandler(null, e);
		}
	}

	#endregion

	void PasswordRulesValidator_ServerValidate(object source, ServerValidateEventArgs args)
	{
		var validator = source as CustomValidator;
		validator.ErrorMessage = string.Empty;

		if (args.Value.Length < Membership.MinRequiredPasswordLength)
		{
			args.IsValid = false;
			validator.ErrorMessage += $"{Resource.RegisterPasswordMinLengthWarning} {Membership.MinRequiredPasswordLength.ToString(CultureInfo.InvariantCulture)}<br />";
		}

		if (!HasEnoughNonAlphaNumericCharacters(args.Value))
		{
			args.IsValid = false;
			validator.ErrorMessage += $"{Resource.RegisterPasswordMinNonAlphaCharsWarning} {Membership.MinRequiredNonAlphanumericCharacters.ToString(CultureInfo.InvariantCulture)}<br />";
		}
	}

	void failSafeUserNameValidator_ServerValidate(object source, ServerValidateEventArgs args)
	{
		if (args.Value.Contains("<"))
		{
			args.IsValid = false;
		}

		if (args.Value.Contains(">"))
		{
			args.IsValid = false;
		}

		if (args.Value.Contains("/"))
		{
			args.IsValid = false;
		}

		if (args.Value.IndexOf("script", StringComparison.InvariantCultureIgnoreCase) > -1)
		{
			args.IsValid = false;
		}
	}

	private bool HasEnoughNonAlphaNumericCharacters(string newPassword)
	{
		var result = false;
		var alphanumeric = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		var passwordChars = newPassword.ToCharArray();
		var nonAlphaNumericCharCount = 0;

		foreach (char c in passwordChars)
		{
			if (!alphanumeric.Contains(c.ToString()))
			{
				nonAlphaNumericCharCount += 1;
			}
		}

		if (nonAlphaNumericCharCount >= Membership.MinRequiredNonAlphanumericCharacters)
		{
			result = true;
		}

		return result;
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

	private void PopulateLabels()
	{
		RegisterUser.ContinueButtonStyle.Font.Bold = true;
		RegisterUser.CreateUserButtonStyle.Font.Bold = true;

		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.RegisterLink);
		litHeading.Text = string.Format(coreDisplaySettings.DefaultPageHeaderMarkup, Resource.RegisterLabel);

		litAlreadyAuthenticated.Text = Resource.AlreadyRegisteredMessage;

		MetaDescription = string.Format(CultureInfo.InvariantCulture, Resource.MetaDescriptionRegistrationPageFormat, siteSettings.SiteName);

		var StartNextButton = (mojoRegisterButton)CreateUserWizardStep1.ContentTemplateContainer.FindControl("StartNextButton");
		StartNextButton.Text = Resource.RegisterButton;

		var userNameRequired = (RequiredFieldValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserNameRequired");
		userNameRequired.ErrorMessage = Resource.RegisterLoginNameRequiredMessage;

		if (WebConfigSettings.UserNameValidationExpression.Length > 0)
		{
			var regexUserName = (RegularExpressionValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("regexUserName");

			if (regexUserName != null)
			{
				regexUserName.ValidationExpression = WebConfigSettings.UserNameValidationExpression;
				regexUserName.ErrorMessage = WebConfigSettings.UserNameValidationWarning;
				regexUserName.Enabled = true;
			}
		}

		var failSafeUserNameValidator = (CustomValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("FailSafeUserNameValidator");
		failSafeUserNameValidator.ErrorMessage = Resource.UserNameHasInvalidCharsWarning;
		failSafeUserNameValidator.ServerValidate += new ServerValidateEventHandler(failSafeUserNameValidator_ServerValidate);

		var emailRequired = (RequiredFieldValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("EmailRequired");
		emailRequired.ErrorMessage = Resource.RegisterEmailRequiredMessage;

		var emailRegex = (RegularExpressionValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("EmailRegex");
		emailRegex.ErrorMessage = Resource.RegisterEmailRegexMessage;

		if (!string.IsNullOrWhiteSpace(WebConfigSettings.CustomEmailRegexWarning))
		{
			emailRegex.ErrorMessage = WebConfigSettings.CustomEmailRegexWarning;
		}

		// hookup event to handle validation
		var passwordRulesValidator = (CustomValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("PasswordRulesValidator");
		passwordRulesValidator.ServerValidate += new ServerValidateEventHandler(PasswordRulesValidator_ServerValidate);

		var passwordRequired = (RequiredFieldValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("PasswordRequired");
		passwordRequired.ErrorMessage = Resource.RegisterPasswordRequiredMessage;

		var passwordRegex = (RegularExpressionValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("PasswordRegex");

		if (string.IsNullOrWhiteSpace(siteSettings.PasswordRegexWarning))
		{
			passwordRegex.ErrorMessage = Resource.RegisterPasswordRegexWarning;
		}
		else
		{
			passwordRegex.ErrorMessage = siteSettings.PasswordRegexWarning;
		}

		var confirmPasswordRequired = (RequiredFieldValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ConfirmPasswordRequired");
		confirmPasswordRequired.ErrorMessage = Resource.RegisterConfirmPasswordRequiredMessage;

		var passwordCompare = (CompareValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("PasswordCompare");
		passwordCompare.ErrorMessage = Resource.RegisterComparePasswordWarning;

		var questionRequired = (RequiredFieldValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("QuestionRequired");
		questionRequired.ErrorMessage = Resource.RegisterSecurityQuestionRequiredMessage;

		var answerRequired = (RequiredFieldValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("AnswerRequired");
		answerRequired.ErrorMessage = Resource.RegisterSecurityAnswerRequiredMessage;

		RegisterUser.RequireEmail = true;
		RegisterUser.CreateUserButtonText = Resource.RegisterButton;
		RegisterUser.CreateUserButtonStyle.CssClass += " createuserbutton";
		RegisterUser.CancelButtonText = Resource.RegisterCancelButton;
		RegisterUser.InvalidQuestionErrorMessage = Resource.RegisterInvalidQuestionErrorMessage;
		RegisterUser.InvalidAnswerErrorMessage = Resource.RegisterInvalidAnswerErrorMessage;
		RegisterUser.InvalidEmailErrorMessage = Resource.RegisterEmailRegexMessage;
		RegisterUser.StartNextButtonText = Resource.RegisterButton;
		RegisterUser.DuplicateEmailErrorMessage = Resource.RegisterDuplicateEmailMessage;
		RegisterUser.DuplicateUserNameErrorMessage = Resource.RegisterDuplicateUserNameMessage;

		if (WebConfigSettings.UseShortcutKeys)
		{
			RegisterUser.AccessKey = AccessKeys.RegisterAccessKey;
			RegisterUser.CreateUserButtonText += SiteUtils.GetButtonAccessKeyPostfix(RegisterUser.AccessKey);
			RegisterUser.ContinueButtonText += SiteUtils.GetButtonAccessKeyPostfix(RegisterUser.AccessKey);
		}

		if (Membership.Provider.PasswordStrengthRegularExpression.Length == 0)
		{
			passwordRegex.Visible = false;
		}
		else
		{
			passwordRegex.ValidationExpression = Membership.Provider.PasswordStrengthRegularExpression;
		}

		if (!Membership.Provider.RequiresQuestionAndAnswer)
		{
			var divQuestion = (HtmlContainerControl)CreateUserWizardStep1.ContentTemplateContainer.FindControl("divQuestion");

			divQuestion.Visible = false;
			questionRequired.Visible = false;

			var divAnswer = (HtmlContainerControl)CreateUserWizardStep1.ContentTemplateContainer.FindControl("divAnswer");

			divAnswer.Visible = false;
			answerRequired.Visible = false;
		}

		litOr.Text = Resource.LiteralOr;

		var continueButton = (Button)CompleteWizardStep1.ContentTemplateContainer.FindControl("ContinueButton");
		continueButton.Text = Resource.RegisterContinueButton;

		var completeMessage = (Literal)CompleteWizardStep1.ContentTemplateContainer.FindControl("CompleteMessage");
		completeMessage.Text = string.Empty;

		if (siteSettings.UseSecureRegistration)
		{
			RegisterUser.LoginCreatedUser = false;
			completeMessage.Text = Resource.RegistrationRequiresEmailConfirmationMessage;
		}
		else if (siteSettings.RequireApprovalBeforeLogin)
		{
			RegisterUser.LoginCreatedUser = false;
			completeMessage.Text = Resource.RegistrationRequiresApprovalMessage;
		}
		else
		{
			RegisterUser.LoginCreatedUser = true;
			completeMessage.Text = Resource.RegisterCompleteMessage;
		}

		var divAgreement = (HtmlContainerControl)CreateUserWizardStep1.ContentTemplateContainer.FindControl("divAgreement");
		var divPreamble = (HtmlContainerControl)CreateUserWizardStep1.ContentTemplateContainer.FindControl("divPreamble");
		var MustAgree = (CustomValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("MustAgree");

		if (divPreamble != null)
		{
			var preamble = siteSettings.RegistrationPreamble;
			if (string.IsNullOrWhiteSpace(preamble))
			{
				preamble = ResourceHelper.GetMessageTemplate("RegisterPreamble.config");
			}
			if (!string.IsNullOrWhiteSpace(preamble))
			{
				divPreamble.Controls.Add(new Literal
				{
					Text = preamble
				});
			}
		}

		var termsOfUse = siteSettings.RegistrationAgreement;
		if (string.IsNullOrWhiteSpace(termsOfUse))
		{
			termsOfUse = ResourceHelper.GetMessageTemplate("RegisterLicense.config");
		}
		if (!string.IsNullOrWhiteSpace(termsOfUse))
		{
			if (MustAgree is not null)
			{
				MustAgree.ServerValidate += new ServerValidateEventHandler(MustAgree_ServerValidate);
				MustAgree.ClientValidationFunction = "CheckBoxRequired_ClientValidate";
				MustAgree.ErrorMessage = Resource.TermsOfUseWarning;
				MustAgree.Enabled = true;
			}

			if (chkAgree is not null)
			{
				chkAgree.Text = Resource.TermsOfUseAgree;
				includeAgreeValidator = true;
			}

			divAgreement.Controls.Add(new Literal
			{
				Text = termsOfUse
			});
		}
		else
		{
			if (MustAgree is not null)
			{
				MustAgree.Enabled = false;
			}

			if (chkAgree is not null)
			{
				chkAgree.Visible = false;
			}
		}

		lnkOpenIDRegistration.Text = Resource.OpenIDRegistrationLink;
		lnkOpenIDRegistration.ToolTip = Resource.OpenIDRegistrationLink;

		lnkWindowsLiveID.Text = Resource.WindowsLiveIDRegistrationLink;
		lnkWindowsLiveID.ToolTip = Resource.WindowsLiveIDRegistrationLink;

		litThirdPartyAuthHeading.Text = Resource.ThirdPartyRegistrationHeading;

		if (siteSettings.UseEmailForLogin && WebConfigSettings.AutoGenerateAndHideUserNamesWhenUsingEmailForLogin)
		{
			var userNamePanel = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("pnlUserName");
			var txtUserName = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName");

			userNamePanel?.Attributes.Add("style", "display:none;");
			userNameRequired.Enabled = false;
			userNameRequired.Visible = false;

			if (txtUserName is not null)
			{
				txtUserName.Text = "nothing";
			}
		}

		if (siteSettings.RequireEnterEmailTwiceOnRegistration)
		{
			var divConfirmEmail = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("divConfirmEmail");
			var EmailCompare = (CompareValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("EmailCompare");
			var ConfirmEmailRequired = (RequiredFieldValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ConfirmEmailRequired");

			//ConfirmEmailRequired
			if ((divConfirmEmail != null) && (EmailCompare != null))
			{
				divConfirmEmail.Visible = true;
				EmailCompare.ErrorMessage = Resource.RegisterCompareEmailWarning;
				EmailCompare.Enabled = true;
				EmailCompare.EnableClientScript = true;

				ConfirmEmailRequired.ErrorMessage = Resource.RegisterCompareEmailRequired;
				ConfirmEmailRequired.Enabled = true;
			}
		}

		var divCaptcha = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("divCaptcha");
		captcha = (CaptchaControl)CreateUserWizardStep1.ContentTemplateContainer.FindControl("captcha");

		if (divCaptcha is not null && captcha is not null)
		{
			if (!siteSettings.RequireCaptchaOnRegistration)
			{
				captcha.Enabled = false;
				captcha.Visible = false;
				divCaptcha.Visible = false;
			}
			else
			{
				captcha.ProviderName = siteSettings.CaptchaProvider;
				captcha.RecaptchaPrivateKey = siteSettings.RecaptchaPrivateKey;
				captcha.RecaptchaPublicKey = siteSettings.RecaptchaPublicKey;
			}
		}
	}

	private bool includeAgreeValidator = false;

	private void SetupScript()
	{
		if (WebConfigSettings.DisablejQuery)
		{
			return;
		}

		if (includeAgreeValidator)
		{
			var script = @$"
<script data-loader=""Register.aspx"">
	function CheckBoxRequired_ClientValidate(sender, e) {{
		e.IsValid = $('#{chkAgree.ClientID}').is(':checked'); 
	}}
</script>";
			Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "regval", script.ToString());
		}

		if (StyleCombiner.UsingjQueryHintsOnRegisterPage)
		{
			SetupjQueryValidate();
		}
	}

	private void SetupjQueryValidate()
	{
		var script = $@"<script data-loader=""Register.aspx"">
function CheckFieldLength(fn, wn, rn, mc) {{
	var len = fn.value.length; 
	if (len > mc) {{ 
		fn.value = fn.value.substring(0, mc); 
		len = mc; 
	}} 
	document.getElementById(wn).innerHTML = len; 
	document.getElementById(rn).innerHTML = mc - len; 
}}

function showHint(myObj) {{
	$(myObj).next(""div"").css('display','inline-block');
}}
function hideHint(myObj) {{
	$(myObj).next(""div"").css('display','none');
}}

function checkUsernameForLength(whatYouTyped) {{
	var fieldset = whatYouTyped.parentNode;
	var txt = whatYouTyped.value;
	
	if (txt.length > {WebConfigSettings.MinUserNameLength.ToInvariantString()}) {{
		fieldset.className = ""settingrow registerrow welldone""; 
	}} else {{ 
		fieldset.className = ""settingrow registerrow"";
	}}
}}

function checkEmail(o) {{
	var fieldset = o.parentNode;
	var txt = o.value;
	// not sure how good this expression is
	var email  = /^([a-zA-Z0-9_\\.\\-])+\\@(([a-zA-Z0-9\\-])+\\.)+([a-zA-Z0-9]{{2,4}})+$/;
			
	if (o.value.match(email)) {{
		fieldset.className = ""settingrow registerrow welldone"";
		$(o).next(""div"").text(""{Server.HtmlEncode(Resource.EmailIsValidHint)}"");
	}} else {{
		fieldset.className = ""settingrow registerrow"";
		$(o).next(""div"").text(""{Server.HtmlEncode(Resource.EmailInvalidHint)}"");
	}}
}}
function checkPasswordConfirm(original, confirm) {{ 
	var fieldset = confirm.parentNode; 
	if ((original[0].value != null) && (confirm.value != null) && (original[0].value == confirm.value)) {{
		fieldset.className = ""settingrow registerrow welldone""; 
		$(confirm).next(""div"").text(""{Resource.PasswordConfirmedHint}"");
	}} else {{ 
		if (fieldset != null) {{ 
			fieldset.className = ""settingrow registerrow "";
			$(confirm).next(""div"").text(""{Resource.PasswordNotConfirmedHint}"");
		}}
	}}
}}";

		var lblUserNameHint = (Label)CreateUserWizardStep1.ContentTemplateContainer.FindControl("lblUserNameHint");
		if (lblUserNameHint != null)
		{
			lblUserNameHint.Text = string.Format(CultureInfo.InvariantCulture, Resource.RegisterLoginNameHintFormat, WebConfigSettings.MinUserNameLength);
		}

		var ConfirmEmail = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ConfirmEmail");


		if (siteSettings.RequireEnterEmailTwiceOnRegistration && ConfirmEmail is not null)
		{
			script += @$"
function confirmEmail(original, confirm) {{
	var fieldset = confirm.parentNode; 
	if((original[0].value != null) && (confirm.value != null) && (original[0].value == confirm.value)) {{ 
		fieldset.className = ""settingrow registerrow welldone""; 
		$(confirm).next(""div"").text(""{Resource.EmailConfirmedHint}""); 
	}} else {{ 
		if (fieldset != null) {{ 
			fieldset.className = ""settingrow registerrow ""; 
			$(confirm).next(""div"").text(\""{Resource.EmailNotConfirmedHint}""); 
		}} 
	}} 
}}";
		}

		script += "\r\n</script>\r\n";

		Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "jqvalidatehelpers", script.ToString());

		var txtUserName = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName");
		if (txtUserName != null)
		{
			txtUserName.Attributes.Add("onkeyup", "checkUsernameForLength(this);");
			txtUserName.Attributes.Add("onfocus", "showHint(this);");
			txtUserName.Attributes.Add("onblur", "hideHint(this);");
		}

		var txtEmail = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("Email");
		if (txtEmail != null)
		{
			txtEmail.Attributes.Add("onkeyup", "checkEmail(this);");
			txtEmail.Attributes.Add("onfocus", "showHint(this);");
			txtEmail.Attributes.Add("onblur", "hideHint(this);");

			if (siteSettings.RequireEnterEmailTwiceOnRegistration)
			{
				if (ConfirmEmail != null)
				{
					ConfirmEmail.Attributes.Add("onfocus", "showHint(this);");
					ConfirmEmail.Attributes.Add("onblur", "hideHint(this);");
				}
			}
		}

		var txtConfirm = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ConfirmPassword");
		var txtPassword = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("Password");

		if (txtConfirm is not null && txtPassword is not null)
		{
			// the hint and the strength meter are not both needed
			if (!siteSettings.ShowPasswordStrengthOnRegistration)
			{
				//txtPassword.Attributes.Add("onkeyup", "checkPassword(this);");
				txtPassword.Attributes.Add("onfocus", "showHint(this);");
				txtPassword.Attributes.Add("onblur", "hideHint(this);");
			}

			txtConfirm.Attributes.Add("onfocus", "showHint(this);");
			txtConfirm.Attributes.Add("onblur", "hideHint(this);");

			var confirmScript = @$"
<script data-loader=""Register.aspx"">
$('#{txtConfirm.ClientID}').keyup(function() {{ checkPasswordConfirm($('#{txtPassword.ClientID}'),(this))}});";

			if (siteSettings.RequireEnterEmailTwiceOnRegistration)
			{
				if (ConfirmEmail != null)
				{
					var pnlEmailConfirmHint = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("pnlEmailConfirmHint");
					if (pnlEmailConfirmHint != null)
					{
						pnlEmailConfirmHint.Visible = true;
					}

					confirmScript += @$"
$(""#{ConfirmEmail.ClientID}"").keyup(function() {{confirmEmail($(""#{txtEmail.ClientID}""),(this))}});";
				}
			}

			confirmScript += "\n</script>\n";

			Page.ClientScript.RegisterStartupScript(typeof(Page), "checkPasswordConfirm", confirmScript);
		}

		var pnlUserNameHint = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("pnlUserNameHint");
		if (pnlUserNameHint != null)
		{
			pnlUserNameHint.Visible = true;
		}

		var pnlEmailHint = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("pnlEmailHint");
		if (pnlEmailHint != null)
		{
			pnlEmailHint.Visible = true;
		}

		var pnlPasswordHint = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("pnlPasswordHint");
		if (pnlPasswordHint != null)
		{
			pnlPasswordHint.Visible = true;
		}

		var pnlConfirmPasswordHint = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("pnlConfirmPasswordHint");
		if (pnlConfirmPasswordHint != null)
		{
			pnlConfirmPasswordHint.Visible = true;
		}
	}

	private void LoadSettings()
	{
		AddClassToBody("registerpage");

		siteAvailableSubscriptions = NewsletterHelper.GetAvailableNewslettersForSiteMembers(siteSettings.SiteGuid);

		IncludeDescriptionInList = displaySettings.IncludeNewsletterDescriptionInList;
		pnlSubscribe = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("pnlSubscribe");

		if (pnlSubscribe != null)
		{
			pnlSubscribe.Visible = displaySettings.ShowNewsLetters;

			clNewsletters = (CheckBoxList)pnlSubscribe.FindControl("clNewsletters");

			// fix bug https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=11390~1#post47409
			if (IsPostBack && clNewsletters.Items.Count == 0)
			{
				pnlSubscribe.Visible = false;
			}
			else
			{
				rbHtmlFormat = (RadioButton)pnlSubscribe.FindControl("rbHtmlFormat");
				rbHtmlFormat.Text = Resource.NewsletterHtmlFormatLabel;

				rbPlainText = (RadioButton)pnlSubscribe.FindControl("rbPlainText");
				rbPlainText.Text = Resource.NewsletterPlainTextFormatLabel;

				var lblNewsletterListHeading = (Label)pnlSubscribe.FindControl("lblNewsletterListHeading");
				lblNewsletterListHeading.Text = displaySettings.NewsletterListHeading.Coalesce(Resource.NewsletterPreferencesHeader);
			}
		}

		if (WebConfigSettings.AllowUserProfilePage)
		{
			var destinationUrl = WebConfigSettings.PageToRedirectToAfterRegistration;
			if (string.IsNullOrWhiteSpace(destinationUrl))
			{
				destinationUrl = "/Secure/UserProfile.aspx";
			}

			RegisterUser.FinishDestinationPageUrl = SiteRoot + destinationUrl;
			RegisterUser.ContinueDestinationPageUrl = SiteRoot + destinationUrl;
			RegisterUser.EditProfileUrl = SiteRoot + "/Secure/UserProfile.aspx";
		}
		else
		{
			RegisterUser.FinishDestinationPageUrl = SiteRoot;
			RegisterUser.ContinueDestinationPageUrl = SiteRoot;
			RegisterUser.EditProfileUrl = SiteRoot;
		}

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

		if (ViewState["returnurl"] != null)
		{
			RegisterUser.ContinueDestinationPageUrl = ViewState["returnurl"].ToString();
		}

		// only allow return urls that are relative or start with the site root
		//http://www.mojoportal.com/Forums/Thread.aspx?thread=5314&mid=34&pageid=5&ItemID=2&pagenumber=1#post22121

		if (Request.Params.Get("returnurl") != null)
		{
			var redirectUrl = SiteUtils.GetReturnUrlParam(Page, SiteRoot);
			if (!string.IsNullOrWhiteSpace(redirectUrl))
			{
				RegisterUser.ContinueDestinationPageUrl = redirectUrl;
			}
		}

		timeOffset = SiteUtils.GetUserTimeOffset();
		timeZone = SiteUtils.GetUserTimeZone();

		if (WebConfigSettings.ShowCustomProfilePropertiesAboveManadotoryRegistrationFields)
		{
			pnlProfile = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("pnlRequiredProfilePropertiesUpper");
		}
		else
		{
			pnlProfile = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("pnlRequiredProfileProperties");
		}

		chkAgree = (CheckBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("chkAgree");
		showRpx = !WebConfigSettings.DisableRpxAuthentication && rpxApiKey.Length > 0;
		showOpenId = WebConfigSettings.EnableOpenIdAuthentication && siteSettings.AllowOpenIdAuth;

		string wlAppId = siteSettings.WindowsLiveAppId;
		if (ConfigurationManager.AppSettings["GlobalWindowsLiveAppId"] != null)
		{
			wlAppId = ConfigurationManager.AppSettings["GlobalWindowsLiveAppId"];
			if (string.IsNullOrWhiteSpace(wlAppId))
			{
				wlAppId = siteSettings.WindowsLiveAppId;
			}
		}

		showWindowsLive
			= WebConfigSettings.EnableWindowsLiveAuthentication
			&& siteSettings.AllowWindowsLiveAuth
			&& !string.IsNullOrWhiteSpace(wlAppId);

		if (IsPostBack)
		{
			showOpenId = false;
			showWindowsLive = false;
			showRpx = false;
		}

		if (showOpenId)
		{
			AddClassToBody("openid");
		}

		if (showWindowsLive)
		{
			AddClassToBody("windowslive");
		}

		if (showRpx)
		{
			AddClassToBody("janrain");
		}

		pnlThirdPartyAuth.Visible = showOpenId || showWindowsLive || showRpx;
		divLiteralOr.Visible = showOpenId && showWindowsLive;
		pnlOpenID.Visible = showOpenId;
		pnlWindowsLiveID.Visible = showWindowsLive;
		pnlRpx.Visible = showRpx;

		if (siteSettings.DisableDbAuth)
		{
			pnlStandardRegister.Visible = false;
		}
	}
}