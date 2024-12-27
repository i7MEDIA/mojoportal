using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Core.Extensions;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI.Pages;

public partial class PasswordResetPage : NonCmsBasePage
{
	private SiteUser siteUser = null;
	private string redirectUrl = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (SiteUtils.SslIsAvailable())
		{
			SiteUtils.ForceSsl();
		}

		SecurityHelper.DisableBrowserCache();

		LoadSettings();

		if (!Request.IsAuthenticated)
		{
			WebUtils.SetupRedirect(this, redirectUrl);
			return;
		}

		// this page is only for user who must change password
		// the regular ChangePassword.aspx uses the ChangePassword Wizard which requires the user to enter the current password
		if ((siteUser == null) || (!siteUser.MustChangePwd))
		{
			WebUtils.SetupRedirect(this, redirectUrl);
			return;
		}

		PopulateLabels();
	}

	void btnChangePassword_Click(object sender, EventArgs e)
	{
		Page.Validate("ChangePassword1");
		if (Page.IsValid)
		{
			siteUser.PasswordResetGuid = Guid.Empty;
			mojoMembershipProvider m = Membership.Provider as mojoMembershipProvider;
			siteUser.Password = m.EncodePassword(siteSettings, siteUser, txtNewPassword.Text);
			siteUser.MustChangePwd = false;
			siteUser.Save();
			siteUser.UpdateLastPasswordChangeTime();

			WebUtils.SetupRedirect(this, redirectUrl);
			return;
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

		mojoMembershipProvider m = Membership.Provider as mojoMembershipProvider;
		if (siteUser.Password == m.EncodePassword(txtNewPassword.Text, siteUser.PasswordSalt, siteSettings))
		{
			args.IsValid = false;
			validator.ErrorMessage += $"{Resource.ChangePasswordNewMatchesOldWarning}<br />";
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

	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.PasswordReset);
		btnChangePassword.Text = Resource.ChangePasswordButton;
		NewPasswordCompare.ErrorMessage = Resource.ChangePasswordMustMatchConfirmMessage;
		ConfirmNewPasswordRequired.ErrorMessage = Resource.ChangePasswordConfirmPasswordRequiredMessage;
		NewPasswordRequired.ErrorMessage = Resource.ChangePasswordNewPasswordRequired;
		NewPasswordRegex.ErrorMessage = Resource.ChangePasswordPasswordRegexFailureMessage;

		if (siteSettings.PasswordRegexWarning.Length > 0)
		{
			NewPasswordRegex.ErrorMessage = siteSettings.PasswordRegexWarning;
		}

		NewPasswordRegex.ValidationExpression = Membership.PasswordStrengthRegularExpression;

		if (Membership.PasswordStrengthRegularExpression.Length == 0)
		{
			NewPasswordRegex.Enabled = false;
		}
	}

	private void LoadSettings()
	{
		siteUser = SiteUtils.GetCurrentSiteUser();

		string returnUrlParam = Page.Request.Params.Get("returnurl");
		if (!string.IsNullOrWhiteSpace(returnUrlParam))
		{
			returnUrlParam = returnUrlParam.RemoveMarkup();
			string requestedRedirectUrl = Page.ResolveUrl(Page.Server.UrlDecode(returnUrlParam).RemoveMarkup());
			if (requestedRedirectUrl.StartsWith("/"))
			{
				redirectUrl = requestedRedirectUrl;
			}
		}
		else
		{
			redirectUrl = SiteRoot;
			if (!siteSettings.IsServerAdminSite
				&& WebConfigSettings.UseFolderBasedMultiTenants
				&& WebConfigSettings.AppendDefaultPageToFolderRootUrl
				)
			{
				redirectUrl += "~/Default.aspx".ToLinkBuilder().ToString();
			}
		}

		AddClassToBody("passwordreset");
	}

	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		IsChangePasswordPage = true; //used to prevent an infinite redirect loop when forcing redirect to change password in mojoBasePage
		Load += new EventHandler(this.Page_Load);
		NewPasswordRulesValidator.ServerValidate += new ServerValidateEventHandler(NewPasswordRulesValidator_ServerValidate);
		btnChangePassword.Click += new EventHandler(btnChangePassword_Click);

		if (Global.SkinConfig.MenuOptions.HideOnChangePassword) 
		{ 
			SuppressAllMenus(); 
		}
	}
	#endregion
}