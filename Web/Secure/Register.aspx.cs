//	Last Modified: 2015-07-04
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using AjaxControlToolkit;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.UserRegisteredHandlers;
using mojoPortal.Web.Configuration;
using mojoPortal.Web.Controls;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI.Pages
{

	//#if !NET35

	//    //http://stackoverflow.com/questions/4575214/how-to-change-the-layout-of-the-createuserwizard-control
	//    //http://channel9.msdn.com/Shows/HanselminutesOn9/Hanselminutes-on-9-ASPNET-4-and-Marcin-Dobosz-on-New-Markup-from-Old-Controls
	//    //http://www.velocityreviews.com/forums/t122608-createuserwizard-ignores-startnavigationtemplate.html
	//    //http://stackoverflow.com/questions/1077121/added-to-createuserwizard-control-additional-wizard-steps-after-createuserwiz

	public class LayoutTemplate : ITemplate
	{
		// this gets rid of the default rendering with tables
		void ITemplate.InstantiateIn(Control container)
		{
			Panel p = new Panel();
			container.Controls.Add(new PlaceHolder() { ID = "wizardStepPlaceholder" });
			container.Controls.Add(new PlaceHolder() { ID = "navigationPlaceholder" });
			//container.Controls.Add(new PlaceHolder() { ID = "headerPlaceholder" });
			// container.Controls.Add(new PlaceHolder() { ID = "sideBarPlaceholder" });
		}
	}


	//public class StartNavigationTemplate : ITemplate
	//{
	//    // this in theory should have made it possible to get rid fo the table around the button
	//    // but it doesn't work
	//    void ITemplate.InstantiateIn(Control container)
	//    {
	//        container.Controls.Add(new mojoButton() { ID = "StartNextButton", CommandName = "MoveNext", Text = "Go Baby" });

	//    }
	//}


	//#endif

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

			this.AppendQueryStringToAction = false;


#if !NET35
			RegisterUser.LayoutTemplate = new LayoutTemplate(); //this gets rid of the outer table in .NET 4
																//RegisterUser.StartNavigationTemplate = new StartNavigationTemplate();


#endif


		}

		#region OnInit

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Load += new EventHandler(this.Page_Load);
			RegisterUser.CreatingUser += new LoginCancelEventHandler(RegisterUser_CreatingUser);
			RegisterUser.CreatedUser += new EventHandler(RegisterUser_CreatedUser);

			if (WebConfigSettings.HideMenusOnRegisterPage)
			{ SuppressAllMenus(); }



		}



		#endregion



		private void Page_Load(object sender, EventArgs e)
		{
			if ((WebConfigSettings.CustomRegistrationPage.Length > 0) && (WebConfigSettings.RedirectRegistrationPageToCustomPage))
			{
				WebUtils.SetupRedirect(this, SiteRoot + WebConfigSettings.CustomRegistrationPage);
				return;
			}

			if (SiteUtils.SslIsAvailable())
				SiteUtils.ForceSsl();
			SecurityHelper.DisableBrowserCache();


			if (!siteSettings.AllowNewRegistration)
			{
				WebUtils.SetupRedirect(this, SiteRoot);
				//Response.Redirect(SiteRoot, true);
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
			{ return; }
			if (clNewsletters == null)
			{ return; }

#if !NET35
			clNewsletters.RepeatLayout = RepeatLayout.UnorderedList;
#endif

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
					if (item != null)
					{ item.Selected = true; }
				}
			}



		}


		private void SetInitialFocus()
		{
			// if custom profile properties are above then it is strange to set focus to a field further down the page
			if (!WebConfigSettings.ShowCustomProfilePropertiesAboveManadotoryRegistrationFields)
			{
				if ((siteSettings.UseEmailForLogin) && (WebConfigSettings.AutoGenerateAndHideUserNamesWhenUsingEmailForLogin))
				{
					TextBox txtEmail
						= (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("Email");
					if (txtEmail != null)
					{
						txtEmail.Focus();
					}
				}
				else
				{
					TextBox txtUserName
						= (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName");
					if (txtUserName != null)
					{
						txtUserName.Focus();
					}
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
#if !MONO
						// we are using the new TimeZoneInfo list but it doesn't work under Mono
						// this makes us skip the TimeOffsetHours setting from mojoProfile.config which is not used under windows
						if (propertyDefinition.Name == mojoProfilePropertyDefinition.TimeOffsetHoursKey)
						{ continue; }
#endif
						if ((propertyDefinition.RequiredForRegistration) || (propertyDefinition.ShowOnRegistration))
						{
							if ((propertyDefinition.Name == mojoProfilePropertyDefinition.TimeZoneIdKey) && (propertyDefinition.DefaultValue.Length == 0))
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

			if ((siteSettings.RequireCaptchaOnRegistration) && (captcha != null))
			{
				if (!captcha.IsValid)
				{
					e.Cancel = true;
				}

			}
		}



		void RegisterUser_CreatedUser(object sender, EventArgs e)
		{
			TextBox txtEmail = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("Email");
			TextBox txtUserName = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName");

			if (txtEmail == null)
			{ return; }
			if (txtUserName == null)
			{ return; }

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
				return;

			if (pnlProfile != null)
			{
				mojoProfileConfiguration profileConfig = mojoProfileConfiguration.GetConfig();

				// set default values first
				foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
				{
					mojoProfilePropertyDefinition.SavePropertyDefault(siteUser, propertyDefinition);
				}

				foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
				{
					if ((propertyDefinition.RequiredForRegistration) || (propertyDefinition.ShowOnRegistration))
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
			UserLocation userLocation = new UserLocation(siteUser.UserGuid, SiteUtils.GetIP4Address());
			userLocation.SiteGuid = siteSettings.SiteGuid;
			userLocation.Hostname = Page.Request.UserHostName;
			userLocation.Save();

			CacheHelper.ClearMembershipStatisticsCache();

			if (
				(!siteSettings.UseSecureRegistration)
				&& (
					(!siteSettings.RequireApprovalBeforeLogin)
					|| (siteUser.ApprovedForLogin)
				  )
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
					string cookieName = "siteguid" + siteSettings.SiteGuid;
					CookieHelper.SetCookie(cookieName, siteUser.UserGuid.ToString(), false);
				}

				siteUser.UpdateLastLoginTime();

			}

			DoSubscribe(siteUser);


			UserRegisteredEventArgs u = new UserRegisteredEventArgs(siteUser);
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
			CustomValidator validator = source as CustomValidator;
			validator.ErrorMessage = string.Empty;

			if (args.Value.Length < Membership.MinRequiredPasswordLength)
			{
				args.IsValid = false;
				validator.ErrorMessage
					+= Resource.RegisterPasswordMinLengthWarning
					+ Membership.MinRequiredPasswordLength.ToString(CultureInfo.InvariantCulture) + "<br />";
			}

			if (!HasEnoughNonAlphaNumericCharacters(args.Value))
			{
				args.IsValid = false;
				validator.ErrorMessage
					+= Resource.RegisterPasswordMinNonAlphaCharsWarning
					+ Membership.MinRequiredNonAlphanumericCharacters.ToString(CultureInfo.InvariantCulture) + "<br />";

			}

		}

		void failSafeUserNameValidator_ServerValidate(object source, ServerValidateEventArgs args)
		{
			if (args.Value.Contains("<"))
			{ args.IsValid = false; }
			if (args.Value.Contains(">"))
			{ args.IsValid = false; }
			if (args.Value.Contains("/"))
			{ args.IsValid = false; }
			if (args.Value.IndexOf("script", StringComparison.InvariantCultureIgnoreCase) > -1)
			{ args.IsValid = false; }
			//if (args.Value.IndexOf("java", StringComparison.InvariantCultureIgnoreCase) > -1) { args.IsValid = false; }

		}

		private bool HasEnoughNonAlphaNumericCharacters(string newPassword)
		{
			bool result = false;
			string alphanumeric = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			char[] passwordChars = newPassword.ToCharArray();
			int nonAlphaNumericCharCount = 0;
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
			this.RegisterUser.ContinueButtonStyle.Font.Bold = true;
			this.RegisterUser.CreateUserButtonStyle.Font.Bold = true;


			Title = SiteUtils.FormatPageTitle(siteSettings, Resource.RegisterLink);
			litHeading.Text = string.Format(coreDisplaySettings.DefaultPageHeaderMarkup, Resource.RegisterLabel);

			litAlreadyAuthenticated.Text = Resource.AlreadyRegisteredMessage;

			MetaDescription = string.Format(CultureInfo.InvariantCulture,
				Resource.MetaDescriptionRegistrationPageFormat, siteSettings.SiteName);


			mojoRegisterButton StartNextButton = (mojoRegisterButton)CreateUserWizardStep1.ContentTemplateContainer.FindControl("StartNextButton");
			StartNextButton.Text = Resource.RegisterButton;




			RequiredFieldValidator userNameRequired
				= (RequiredFieldValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserNameRequired");

			userNameRequired.ErrorMessage = Resource.RegisterLoginNameRequiredMessage;

			if (WebConfigSettings.UserNameValidationExpression.Length > 0)
			{
				RegularExpressionValidator regexUserName
					= (RegularExpressionValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("regexUserName");

				if (regexUserName != null)
				{
					regexUserName.ValidationExpression = WebConfigSettings.UserNameValidationExpression;
					regexUserName.ErrorMessage = WebConfigSettings.UserNameValidationWarning;
					regexUserName.Enabled = true;

				}

			}

			CustomValidator failSafeUserNameValidator
				= (CustomValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("FailSafeUserNameValidator");

			failSafeUserNameValidator.ErrorMessage = Resource.UserNameHasInvalidCharsWarning;
			failSafeUserNameValidator.ServerValidate += new ServerValidateEventHandler(failSafeUserNameValidator_ServerValidate);

			RequiredFieldValidator emailRequired
				= (RequiredFieldValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("EmailRequired");

			emailRequired.ErrorMessage = Resource.RegisterEmailRequiredMessage;

			RegularExpressionValidator emailRegex
				= (RegularExpressionValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("EmailRegex");

			emailRegex.ErrorMessage = Resource.RegisterEmailRegexMessage;

			if (WebConfigSettings.CustomEmailRegexWarning.Length > 0)
			{
				emailRegex.ErrorMessage = WebConfigSettings.CustomEmailRegexWarning;
			}

			CustomValidator passwordRulesValidator
				= (CustomValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("PasswordRulesValidator");

			// hookup event to handle validation
			passwordRulesValidator.ServerValidate += new ServerValidateEventHandler(PasswordRulesValidator_ServerValidate);


			RequiredFieldValidator passwordRequired
				= (RequiredFieldValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("PasswordRequired");

			passwordRequired.ErrorMessage = Resource.RegisterPasswordRequiredMessage;




			RegularExpressionValidator passwordRegex
				= (RegularExpressionValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("PasswordRegex");

			if (siteSettings.PasswordRegexWarning.Length > 0)
			{
				passwordRegex.ErrorMessage = siteSettings.PasswordRegexWarning;
			}
			else
			{
				passwordRegex.ErrorMessage = Resource.RegisterPasswordRegexWarning;
			}


			RequiredFieldValidator confirmPasswordRequired
				= (RequiredFieldValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ConfirmPasswordRequired");

			confirmPasswordRequired.ErrorMessage = Resource.RegisterConfirmPasswordRequiredMessage;

			CompareValidator passwordCompare
				= (CompareValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("PasswordCompare");

			passwordCompare.ErrorMessage = Resource.RegisterComparePasswordWarning;

			RequiredFieldValidator questionRequired
				= (RequiredFieldValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("QuestionRequired");

			questionRequired.ErrorMessage = Resource.RegisterSecurityQuestionRequiredMessage;

			RequiredFieldValidator answerRequired
				= (RequiredFieldValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("AnswerRequired");

			answerRequired.ErrorMessage = Resource.RegisterSecurityAnswerRequiredMessage;

			this.RegisterUser.RequireEmail = true;

			this.RegisterUser.CreateUserButtonText = Resource.RegisterButton;
			RegisterUser.CreateUserButtonStyle.CssClass += " createuserbutton";

			this.RegisterUser.CancelButtonText = Resource.RegisterCancelButton;

			if (WebConfigSettings.UseShortcutKeys)
			{
				this.RegisterUser.AccessKey = AccessKeys.RegisterAccessKey;
				this.RegisterUser.CreateUserButtonText +=
					SiteUtils.GetButtonAccessKeyPostfix(this.RegisterUser.AccessKey);
				this.RegisterUser.ContinueButtonText +=
					SiteUtils.GetButtonAccessKeyPostfix(this.RegisterUser.AccessKey);
			}

			this.RegisterUser.InvalidQuestionErrorMessage = Resource.RegisterInvalidQuestionErrorMessage;
			this.RegisterUser.InvalidAnswerErrorMessage = Resource.RegisterInvalidAnswerErrorMessage;
			this.RegisterUser.InvalidEmailErrorMessage = Resource.RegisterEmailRegexMessage;

			this.RegisterUser.StartNextButtonText = Resource.RegisterButton;

			this.RegisterUser.DuplicateEmailErrorMessage
				= Resource.RegisterDuplicateEmailMessage;

			this.RegisterUser.DuplicateUserNameErrorMessage
				= Resource.RegisterDuplicateUserNameMessage;

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

				HtmlContainerControl divQuestion
				= (HtmlContainerControl)CreateUserWizardStep1.ContentTemplateContainer.FindControl("divQuestion");

				divQuestion.Visible = false;
				questionRequired.Visible = false;

				HtmlContainerControl divAnswer
				= (HtmlContainerControl)CreateUserWizardStep1.ContentTemplateContainer.FindControl("divAnswer");

				divAnswer.Visible = false;
				answerRequired.Visible = false;


			}

			litOr.Text = Resource.LiteralOr;

			Button continueButton =
				(Button)CompleteWizardStep1.ContentTemplateContainer.FindControl("ContinueButton");

			continueButton.Text = Resource.RegisterContinueButton;

			Literal completeMessage =
				(Literal)CompleteWizardStep1.ContentTemplateContainer.FindControl("CompleteMessage");

			completeMessage.Text = "";
			if (siteSettings.UseSecureRegistration)
			{
				this.RegisterUser.LoginCreatedUser = false;
				completeMessage.Text = Resource.RegistrationRequiresEmailConfirmationMessage;
			}
			else if (siteSettings.RequireApprovalBeforeLogin)
			{
				this.RegisterUser.LoginCreatedUser = false;
				completeMessage.Text = Resource.RegistrationRequiresApprovalMessage;
			}
			else
			{
				this.RegisterUser.LoginCreatedUser = true;
				completeMessage.Text = Resource.RegisterCompleteMessage;

			}

			HtmlContainerControl divAgreement
				= (HtmlContainerControl)CreateUserWizardStep1.ContentTemplateContainer.FindControl("divAgreement");

			HtmlContainerControl divPreamble
				= (HtmlContainerControl)CreateUserWizardStep1.ContentTemplateContainer.FindControl("divPreamble");

			CustomValidator MustAgree
				= (CustomValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("MustAgree");

			if (divPreamble != null)
			{
				string preamble = siteSettings.RegistrationPreamble;
				if (preamble.Length == 0)
				{
					preamble = ResourceHelper.GetMessageTemplate("RegisterPreamble.config");
				}
				if (preamble.Length > 0)
				{
					Literal pre = new Literal();
					pre.Text = preamble;
					divPreamble.Controls.Add(pre);
				}
			}

			string termsOfUse = siteSettings.RegistrationAgreement;
			if (termsOfUse.Length == 0)
			{
				termsOfUse = ResourceHelper.GetMessageTemplate("RegisterLicense.config");
			}
			if (termsOfUse.Length > 0)
			{
				if (MustAgree != null)
				{
					MustAgree.ServerValidate += new ServerValidateEventHandler(MustAgree_ServerValidate);
					MustAgree.ClientValidationFunction = "CheckBoxRequired_ClientValidate";
					MustAgree.ErrorMessage = Resource.TermsOfUseWarning;
					MustAgree.Enabled = true;
				}

				if (chkAgree != null)
				{
					chkAgree.Text = Resource.TermsOfUseAgree;
					includeAgreeValidator = true;
				}

				Literal agreement = new Literal();
				agreement.Text = termsOfUse;
				divAgreement.Controls.Add(agreement);
			}
			else
			{
				if (MustAgree != null)
				{ MustAgree.Enabled = false; }
				if (chkAgree != null)
				{ chkAgree.Visible = false; }

			}


			lnkOpenIDRegistration.Text = Resource.OpenIDRegistrationLink;
			lnkOpenIDRegistration.ToolTip = Resource.OpenIDRegistrationLink;

			lnkWindowsLiveID.Text = Resource.WindowsLiveIDRegistrationLink;
			lnkWindowsLiveID.ToolTip = Resource.WindowsLiveIDRegistrationLink;

			litThirdPartyAuthHeading.Text = Resource.ThirdPartyRegistrationHeading;



			if ((siteSettings.UseEmailForLogin) && (WebConfigSettings.AutoGenerateAndHideUserNamesWhenUsingEmailForLogin))
			{
				Panel userNamePanel
				= (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("pnlUserName");

				TextBox txtUserName = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName");

				if (userNamePanel != null)
				{ userNamePanel.Attributes.Add("style", "display:none;"); }
				userNameRequired.Enabled = false;
				userNameRequired.Visible = false;
				if (txtUserName != null)
				{ txtUserName.Text = "nothing"; }
			}

			//if (
			//	(WebConfigSettings.EnableAjaxControlPasswordStrength) // false as of 2014-12-15 because enabling it causes viewstate mac error
			//	&& (siteSettings.ShowPasswordStrengthOnRegistration))
			//{
			//	PasswordStrength passwordStrengthChecker = (PasswordStrength)CreateUserWizardStep1.ContentTemplateContainer.FindControl("passwordStrengthChecker");
			//	if (passwordStrengthChecker != null)
			//	{
			//		passwordStrengthChecker.Enabled = true;
			//		passwordStrengthChecker.RequiresUpperAndLowerCaseCharacters = true;
			//		passwordStrengthChecker.MinimumLowerCaseCharacters = WebConfigSettings.PasswordStrengthMinimumLowerCaseCharacters;
			//		passwordStrengthChecker.MinimumUpperCaseCharacters = WebConfigSettings.PasswordStrengthMinimumUpperCaseCharacters;
			//		passwordStrengthChecker.MinimumSymbolCharacters = siteSettings.MinRequiredNonAlphanumericCharacters;
			//		passwordStrengthChecker.PreferredPasswordLength = siteSettings.MinRequiredPasswordLength;

			//		passwordStrengthChecker.PrefixText = Resource.PasswordStrengthPrefix;
			//		passwordStrengthChecker.TextStrengthDescriptions = Resource.PasswordStrengthDescriptions;
			//		passwordStrengthChecker.CalculationWeightings = WebConfigSettings.PasswordStrengthCalculationWeightings;

			//		try
			//		{
			//			passwordStrengthChecker.StrengthIndicatorType = (StrengthIndicatorTypes)Enum.Parse(typeof(StrengthIndicatorTypes), WebConfigSettings.PasswordStrengthIndicatorType, true);
			//		}
			//		catch (ArgumentException)
			//		{
			//			passwordStrengthChecker.StrengthIndicatorType = StrengthIndicatorTypes.Text;
			//		}
			//		catch (OverflowException)
			//		{
			//			passwordStrengthChecker.StrengthIndicatorType = StrengthIndicatorTypes.Text;
			//		}

			//		try
			//		{
			//			passwordStrengthChecker.DisplayPosition = (DisplayPosition)Enum.Parse(typeof(DisplayPosition), WebConfigSettings.PasswordStrengthDisplayPosition, true);
			//		}
			//		catch (ArgumentException)
			//		{
			//			passwordStrengthChecker.DisplayPosition = DisplayPosition.RightSide;
			//		}
			//		catch (OverflowException)
			//		{
			//			passwordStrengthChecker.DisplayPosition = DisplayPosition.RightSide;
			//		}
			//	}

			//}

			if (siteSettings.RequireEnterEmailTwiceOnRegistration)
			{
				Panel divConfirmEmail = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("divConfirmEmail");
				CompareValidator EmailCompare = (CompareValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("EmailCompare");
				RequiredFieldValidator ConfirmEmailRequired = (RequiredFieldValidator)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ConfirmEmailRequired");

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

			Panel divCaptcha = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("divCaptcha");
			captcha = (CaptchaControl)CreateUserWizardStep1.ContentTemplateContainer.FindControl("captcha");

			if ((divCaptcha != null) && (captcha != null))
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
			{ return; }


			if (includeAgreeValidator)
			{
				StringBuilder script = new StringBuilder();
				script.Append("\n <script type=\"text/javascript\"> ");
				script.Append("function CheckBoxRequired_ClientValidate(sender, e) {");
				script.Append("e.IsValid = $('#" + chkAgree.ClientID + "').is(':checked'); }");

				script.Append("\n </script>");

				Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
						"regval", script.ToString());
			}

			if (StyleCombiner.UsingjQueryHintsOnRegisterPage)
			{
				SetupjQueryValidate();
			}



		}

		private void SetupjQueryValidate()
		{
			StringBuilder script = new StringBuilder();
			script.Append("\n<script type=\"text/javascript\">\n");

			script.Append("function CheckFieldLength(fn, wn, rn, mc) { ");
			script.Append("var len = fn.value.length; ");
			script.Append("if (len > mc) { ");
			script.Append("fn.value = fn.value.substring(0, mc); ");
			script.Append("len = mc; ");
			script.Append("} ");
			script.Append("document.getElementById(wn).innerHTML = len; ");
			script.Append("document.getElementById(rn).innerHTML = mc - len; ");
			script.Append("} ");


			script.Append("function showHint(myObj) { ");
			script.Append("$(myObj).next(\"div\").css('display','inline-block'); ");
			script.Append("} ");


			script.Append("function hideHint(myObj) { ");
			script.Append("$(myObj).next(\"div\").css('display','none'); ");
			script.Append("} ");

			script.Append("function checkUsernameForLength(whatYouTyped) { ");
			script.Append("var fieldset = whatYouTyped.parentNode; ");
			script.Append("var txt = whatYouTyped.value; ");
			script.Append("if (txt.length > " + WebConfigSettings.MinUserNameLength.ToInvariantString() + ") { ");
			script.Append("fieldset.className = \"settingrow registerrow welldone\"; ");
			script.Append("} else { ");
			script.Append("fieldset.className = \"settingrow registerrow\"; ");
			script.Append("} ");
			script.Append("} ");

			script.Append("function checkEmail(o) { ");
			script.Append("var fieldset = o.parentNode; ");
			script.Append("var txt = o.value; ");
			// not sure how good this expression is
			script.Append("var email  = /^([a-zA-Z0-9_\\.\\-])+\\@(([a-zA-Z0-9\\-])+\\.)+([a-zA-Z0-9]{2,4})+$/; ");
			script.Append("if (o.value.match(email)) { ");
			script.Append("fieldset.className = \"settingrow registerrow welldone\"; ");

			script.Append("$(o).next(\"div\").text(\"" + Server.HtmlEncode(Resource.EmailIsValidHint) + "\"); ");
			script.Append("} else { ");
			script.Append("fieldset.className = \"settingrow registerrow\"; ");

			script.Append("$(o).next(\"div\").text(\"" + Server.HtmlEncode(Resource.EmailInvalidHint) + "\"); ");
			script.Append("} ");
			script.Append("} ");


			script.Append("function checkPasswordConfirm(original, confirm) { ");
			script.Append("var fieldset = confirm.parentNode; ");
			script.Append("if((original[0].value != null) && (confirm.value != null) && (original[0].value == confirm.value)) { ");
			script.Append("fieldset.className = \"settingrow registerrow welldone\"; ");

			script.Append("$(confirm).next(\"div\").text(\"" + Resource.PasswordConfirmedHint + "\"); ");
			script.Append("} else { ");
			script.Append("if (fieldset != null) { ");
			script.Append("fieldset.className = \"settingrow registerrow \"; ");

			script.Append("$(confirm).next(\"div\").text(\"" + Resource.PasswordNotConfirmedHint + "\"); ");
			script.Append("} ");
			script.Append("} ");
			script.Append("} ");

			Label lblUserNameHint = (Label)CreateUserWizardStep1.ContentTemplateContainer.FindControl("lblUserNameHint");
			if (lblUserNameHint != null)
			{
				lblUserNameHint.Text = string.Format(CultureInfo.InvariantCulture, Resource.RegisterLoginNameHintFormat, WebConfigSettings.MinUserNameLength);
			}

			TextBox ConfirmEmail = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ConfirmEmail");


			if (siteSettings.RequireEnterEmailTwiceOnRegistration)
			{
				if (ConfirmEmail != null)
				{
					script.Append("function confirmEmail(original, confirm) { ");
					script.Append("var fieldset = confirm.parentNode; ");
					script.Append("if((original[0].value != null) && (confirm.value != null) && (original[0].value == confirm.value)) { ");
					script.Append("fieldset.className = \"settingrow registerrow welldone\"; ");

					script.Append("$(confirm).next(\"div\").text(\"" + Resource.EmailConfirmedHint + "\"); ");
					script.Append("} else { ");
					script.Append("if (fieldset != null) { ");
					script.Append("fieldset.className = \"settingrow registerrow \"; ");

					script.Append("$(confirm).next(\"div\").text(\"" + Resource.EmailNotConfirmedHint + "\"); ");
					script.Append("} ");
					script.Append("} ");
					script.Append("} ");

				}

			}

			script.Append("\n</script>\n");

			Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "jqvalidatehelpers", script.ToString());

			TextBox txtUserName = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName");
			if (txtUserName != null)
			{
				txtUserName.Attributes.Add("onkeyup", "checkUsernameForLength(this);");
				txtUserName.Attributes.Add("onfocus", "showHint(this);");
				txtUserName.Attributes.Add("onblur", "hideHint(this);");
			}

			TextBox txtEmail = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("Email");
			if (txtEmail != null)
			{
				txtEmail.Attributes.Add("onkeyup", "checkEmail(this);");
				txtEmail.Attributes.Add("onfocus", "showHint(this);");
				txtEmail.Attributes.Add("onblur", "hideHint(this);");

				if (siteSettings.RequireEnterEmailTwiceOnRegistration)
				{
					if (ConfirmEmail != null)
					{
						//ConfirmEmail.Attributes.Add("onkeyup", "confirmEmail(this);");
						ConfirmEmail.Attributes.Add("onfocus", "showHint(this);");
						ConfirmEmail.Attributes.Add("onblur", "hideHint(this);");
					}

				}
			}

			TextBox txtConfirm = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ConfirmPassword");
			TextBox txtPassword = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("Password");

			if ((txtConfirm != null) && (txtPassword != null))
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

				script = new StringBuilder();
				script.Append("\n<script type=\"text/javascript\">\n");

				script.Append(" $('#" + txtConfirm.ClientID + "').keyup(function() { checkPasswordConfirm($('#" + txtPassword.ClientID + "'),(this))});");

				if (siteSettings.RequireEnterEmailTwiceOnRegistration)
				{
					if (ConfirmEmail != null)
					{
						Panel pnlEmailConfirmHint = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("pnlEmailConfirmHint");
						if (pnlEmailConfirmHint != null)
						{ pnlEmailConfirmHint.Visible = true; }

						script.Append(" $('#" + ConfirmEmail.ClientID + "').keyup(function() { confirmEmail($('#" + txtEmail.ClientID + "'),(this))});");
					}
				}

				script.Append("\n</script>\n");

				Page.ClientScript.RegisterStartupScript(typeof(Page), "checkPasswordConfirm", script.ToString());
			}

			Panel pnlUserNameHint = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("pnlUserNameHint");
			if (pnlUserNameHint != null)
			{ pnlUserNameHint.Visible = true; }

			Panel pnlEmailHint = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("pnlEmailHint");
			if (pnlEmailHint != null)
			{ pnlEmailHint.Visible = true; }

			Panel pnlPasswordHint = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("pnlPasswordHint");
			if (pnlPasswordHint != null)
			{ pnlPasswordHint.Visible = true; }

			Panel pnlConfirmPasswordHint = (Panel)CreateUserWizardStep1.ContentTemplateContainer.FindControl("pnlConfirmPasswordHint");
			if (pnlConfirmPasswordHint != null)
			{ pnlConfirmPasswordHint.Visible = true; }



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
				rbHtmlFormat = (RadioButton)pnlSubscribe.FindControl("rbHtmlFormat");
				rbPlainText = (RadioButton)pnlSubscribe.FindControl("rbPlainText");

				rbHtmlFormat.Text = Resource.NewsletterHtmlFormatLabel;
				rbPlainText.Text = Resource.NewsletterPlainTextFormatLabel;
				Label lblNewsletterListHeading = (Label)pnlSubscribe.FindControl("lblNewsletterListHeading");
				lblNewsletterListHeading.Text = displaySettings.NewsletterListHeading.Coalesce(Resource.NewsletterPreferencesHeader);
				// fix bug https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=11390~1#post47409
				if ((IsPostBack) && (clNewsletters.Items.Count == 0))
				{ pnlSubscribe.Visible = false; }
			}

			if (WebConfigSettings.AllowUserProfilePage)
			{
				string destinationUrl = WebConfigSettings.PageToRedirectToAfterRegistration;
				if (destinationUrl.Length == 0)
				{ destinationUrl = "/Secure/UserProfile.aspx"; }

				this.RegisterUser.FinishDestinationPageUrl
					= SiteRoot + destinationUrl;

				this.RegisterUser.ContinueDestinationPageUrl
					= SiteRoot + destinationUrl;

				this.RegisterUser.EditProfileUrl = SiteRoot + "/Secure/UserProfile.aspx";
			}
			else
			{
				this.RegisterUser.FinishDestinationPageUrl = SiteRoot;

				this.RegisterUser.ContinueDestinationPageUrl = SiteRoot;

				this.RegisterUser.EditProfileUrl = SiteRoot;

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

			//string returnUrlParam = Page.Request.Params.Get("returnurl");
			//if (!String.IsNullOrEmpty(returnUrlParam))
			//{
			//    string redirectUrl = Page.ResolveUrl(Page.Server.UrlDecode(returnUrlParam));
			//    this.RegisterUser.FinishDestinationPageUrl = redirectUrl;
			//    this.RegisterUser.ContinueDestinationPageUrl = redirectUrl;

			//}



			if (ViewState["returnurl"] != null)
			{
				this.RegisterUser.ContinueDestinationPageUrl = ViewState["returnurl"].ToString();
			}

			// only allow return urls that are relative or start with the site root
			//http://www.mojoportal.com/Forums/Thread.aspx?thread=5314&mid=34&pageid=5&ItemID=2&pagenumber=1#post22121

			if (Request.Params.Get("returnurl") != null)
			{
				//string returnUrlParam = Page.Request.Params.Get("returnurl");
				//if (!String.IsNullOrEmpty(returnUrlParam))
				//{
				//returnUrlParam = SecurityHelper.RemoveMarkup(returnUrlParam);
				string redirectUrl = SiteUtils.GetReturnUrlParam(Page, SiteRoot);
				//string redirectUrl = Page.ResolveUrl(SecurityHelper.RemoveMarkup(Page.Server.UrlDecode(returnUrlParam)));
				//if (redirectUrl.StartsWith("/")) { redirectUrl = SiteRoot + redirectUrl; }

				//if (
				//    (redirectUrl.StartsWith(SiteRoot)) 
				//    || (redirectUrl.StartsWith(SiteRoot.Replace("https://", "http://")))
				//    )
				//{
				if (redirectUrl.Length > 0)
				{
					this.RegisterUser.ContinueDestinationPageUrl = redirectUrl;
				}
				//}
				// }


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

			showRpx = ((!WebConfigSettings.DisableRpxAuthentication) && (rpxApiKey.Length > 0));

			showOpenId = (
				(WebConfigSettings.EnableOpenIdAuthentication && siteSettings.AllowOpenIdAuth)

				);

			string wlAppId = siteSettings.WindowsLiveAppId;
			if (ConfigurationManager.AppSettings["GlobalWindowsLiveAppId"] != null)
			{
				wlAppId = ConfigurationManager.AppSettings["GlobalWindowsLiveAppId"];
				if (wlAppId.Length == 0)
				{ wlAppId = siteSettings.WindowsLiveAppId; }
			}

			showWindowsLive
				= WebConfigSettings.EnableWindowsLiveAuthentication
				&& siteSettings.AllowWindowsLiveAuth
				&& (wlAppId.Length > 0);

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

			pnlThirdPartyAuth.Visible = (showOpenId || showWindowsLive || showRpx);
			divLiteralOr.Visible = (showOpenId && showWindowsLive);
			pnlOpenID.Visible = showOpenId;
			pnlWindowsLiveID.Visible = showWindowsLive;
			pnlRpx.Visible = showRpx;


			if (siteSettings.DisableDbAuth)
			{ pnlStandardRegister.Visible = false; }


		}




	}
}

namespace mojoPortal.Web.UI
{
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	/// <summary>
	/// this control doesn't render anything, it is used only as a themeable collection of settings for things we would like to be able to configure from theme.skin
	/// </summary>
	public class RegistrationPageDisplaySettings : WebControl
	{

		public string NewsletterListHeading { get; set; } = string.Empty;

		public bool IncludeNewsletterDescriptionInList { get; set; } = false;

		public bool ShowNewsLetters { get; set; } = true;

		protected override void Render(HtmlTextWriter writer)
		{
			if (HttpContext.Current == null)
			{
				writer.Write("[" + this.ID + "]");
				return;
			}

			// nothing to render
		}
	}

}

