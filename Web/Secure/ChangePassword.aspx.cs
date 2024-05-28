using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Core.Extensions;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI.Pages;

public partial class ChangePassword : NonCmsBasePage
{
	private static readonly ILog log = LogManager.GetLogger(typeof(ChangePassword));

	#region OnInit
	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);

		Load += new EventHandler(Page_Load);
		ChangePassword1.ChangedPassword += new EventHandler(ChangePassword1_ChangedPassword);

		if (Global.SkinConfig.MenuOptions.HideOnChangePassword)
		{
			SuppressAllMenus();
		}
	}
	#endregion

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Request.IsAuthenticated)
		{
			SiteUtils.RedirectToLoginPage(this);
			return;
		}

		if (SiteUtils.SslIsAvailable())
		{
			SiteUtils.ForceSsl();
		}

		SecurityHelper.DisableBrowserCache();

		PopulateLabels();
	}

	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.ChangePasswordLabel);

		var changePasswordButton = (Button)ChangePassword1.ChangePasswordTemplateContainer.FindControl("ChangePasswordPushButton");
		var cancelButton = (Button)ChangePassword1.ChangePasswordTemplateContainer.FindControl("CancelPushButton");

		if (changePasswordButton != null)
		{
			changePasswordButton.Text = Resource.ChangePasswordButton;
			SiteUtils.SetButtonAccessKey(changePasswordButton, AccessKeys.ChangePasswordButtonAccessKey);
		}
		else
		{
			log.Debug("couldn't find changepasswordbutton so couldn't set label");
		}

		if (cancelButton != null)
		{
			cancelButton.Text = Resource.ChangePasswordCancelButton;
			SiteUtils.SetButtonAccessKey(cancelButton, AccessKeys.ChangePasswordCancelButtonAccessKey);
		}
		else
		{
			log.Debug("couldn't find cancelbutton so couldn't set label");
		}

		ChangePassword1.CancelDestinationPageUrl = $"{SiteUtils.GetNavigationSiteRoot()}/Secure/UserProfile.aspx";
		ChangePassword1.ChangePasswordFailureText = Resource.ChangePasswordFailureText;

		var newPasswordCompare = (CompareValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPasswordCompare");

		if (newPasswordCompare != null)
		{
			newPasswordCompare.ErrorMessage = Resource.ChangePasswordMustMatchConfirmMessage;
		}

		var confirmNewPasswordRequired = (RequiredFieldValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("ConfirmNewPasswordRequired");

		if (confirmNewPasswordRequired != null)
		{
			confirmNewPasswordRequired.ErrorMessage = Resource.ChangePasswordConfirmPasswordRequiredMessage;
		}

		ChangePassword1.ContinueDestinationPageUrl = SiteUtils.GetNavigationSiteRoot();

		var newPasswordRequired = (RequiredFieldValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPasswordRequired");

		if (newPasswordRequired != null)
		{
			newPasswordRequired.ErrorMessage = Resource.ChangePasswordNewPasswordRequired;
		}

		var currentPasswordRequired = (RequiredFieldValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("CurrentPasswordRequired");

		if (currentPasswordRequired != null)
		{
			currentPasswordRequired.ErrorMessage = Resource.ChangePasswordCurrentPasswordRequiredWarning;
		}

		var newPasswordRegex = (RegularExpressionValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPasswordRegex");

		if (newPasswordRegex != null)
		{
			newPasswordRegex.ErrorMessage = Resource.ChangePasswordPasswordRegexFailureMessage;
			if (siteSettings.PasswordRegexWarning.Length > 0)
			{
				newPasswordRegex.ErrorMessage = siteSettings.PasswordRegexWarning;
			}

			newPasswordRegex.ValidationExpression = Membership.PasswordStrengthRegularExpression;

			if (Membership.PasswordStrengthRegularExpression.Length == 0)
			{
				newPasswordRegex.Visible = false;
				newPasswordRegex.ValidationGroup = "None";
			}
		}

		var newPasswordRulesValidator = (CustomValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPasswordRulesValidator");
		if (newPasswordRulesValidator != null)
		{
			newPasswordRulesValidator.ServerValidate += new ServerValidateEventHandler(NewPasswordRulesValidator_ServerValidate);
		}

		ChangePassword1.SuccessTitleText = string.Empty;
		ChangePassword1.SuccessText = Resource.ChangePasswordSuccessText;

		AddClassToBody("changepassword");
	}

	void ChangePassword1_ChangedPassword(object sender, EventArgs e)
	{
		//this is needed to prevent a script error in IE8 after the password is changed
		var vSummary = (ValidationSummary)ChangePassword1.ChangePasswordTemplateContainer.FindControl("vSummary");
		if (vSummary != null) { vSummary.Visible = false; }
		if (WebConfigSettings.LogIpAddressForPasswordChanges)
		{
			SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
			if (currentUser != null)
			{
				log.Info($"user {currentUser.Name} changed their password from ip address {SiteUtils.GetIP4Address()}");
			}
		}
	}

	void NewPasswordRulesValidator_ServerValidate(object source, ServerValidateEventArgs args)
	{
		CustomValidator validator = source as CustomValidator;
		validator.ErrorMessage = string.Empty;

		if (args.Value.Length < Membership.MinRequiredPasswordLength)
		{
			args.IsValid = false;
			validator.ErrorMessage += $"{Resource.ChangePasswordMinimumLengthWarning} {Membership.MinRequiredPasswordLength.ToInvariantString()}<br />";
		}

		if (!HasEnoughNonAlphaNumericCharacters(args.Value))
		{
			args.IsValid = false;
			validator.ErrorMessage += $"{Resource.ChangePasswordMinNonAlphanumericCharsWarning} {Membership.MinRequiredNonAlphanumericCharacters.ToInvariantString()}<br />";

		}

		var currentPassword = (TextBox)ChangePassword1.ChangePasswordTemplateContainer.FindControl("CurrentPassword");

		var newPassword = (TextBox)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPassword");

		SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
		if (currentUser != null)
		{
			if (currentPassword != null)
			{
				switch (Membership.Provider.PasswordFormat)
				{
					case MembershipPasswordFormat.Clear:
						if (currentPassword.Text != currentUser.Password)
						{
							args.IsValid = false;
							validator.ErrorMessage += $"{Resource.ChangePasswordCurrentPasswordIncorrectWarning}<br />";
						}
						break;

					case MembershipPasswordFormat.Encrypted:
						break;

					case MembershipPasswordFormat.Hashed:
						break;
				}
			}
		}

		if (newPassword is not null && currentPassword is not null)
		{
			if (newPassword.Text == currentPassword.Text)
			{
				args.IsValid = false;
				validator.ErrorMessage += $"{Resource.ChangePasswordNewMatchesOldWarning}<br />";
			}
		}
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
}