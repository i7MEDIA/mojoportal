using log4net;
using mojoPortal.Net;
using mojoPortal.Web.Components;
using mojoPortal.Web.Controls;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI.Pages;

public partial class RecoverPassword : NonCmsBasePage
{
	private static readonly ILog log = LogManager.GetLogger(typeof(RecoverPassword));
	protected Label EnterUserNameLabel;

	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);

		if (AppConfig.OAuth.Configured)
		{
			WebUtils.SetupRedirect(this, PageUrlService.GetLoginLink());
			return;
		}

		Load += new EventHandler(Page_Load);
		Error += new EventHandler(RecoverPassword_Error);
		base.EnsureChildControls();
		PasswordRecovery1.SendingMail += new MailMessageEventHandler(PasswordRecovery1_SendingMail);
		PasswordRecovery1.SendMailError += new SendMailErrorEventHandler(PasswordRecovery1_SendMailError);

		if (Global.SkinConfig.MenuOptions.HideOnPasswordRecovery)
		{
			SuppressAllMenus();
		}
	}

	#endregion

	protected void Page_Load(object sender, EventArgs e)
	{
		if (SiteUtils.SslIsAvailable())
			SiteUtils.ForceSsl();


		bool allow = (siteSettings.AllowPasswordRetrieval || siteSettings.AllowPasswordReset)
			&& (!siteSettings.UseLdapAuth || (siteSettings.UseLdapAuth && siteSettings.AllowDbFallbackWithLdap));

		if (!allow)
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		PopulateLabels();
	}

	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.PasswordRecoveryTitle);

		MetaDescription = Server.HtmlEncode(string.Format(CultureInfo.InvariantCulture, Resource.RecoverPasswordMetaDescriptionFormat, siteSettings.SiteName));

		if (Request.IsAuthenticated)
		{
			PasswordRecovery1.Visible = false;
			litMessage.Text = string.Format(DisplaySettings.AlertErrorMarkup, Resource.PasswordRecoveryNotAllowedWhenAuthenticated);
		}

		litHeading.Text = string.Format(DisplaySettings.DefaultPageHeaderMarkup, Resource.ForgotPasswordLabel);

		EnterUserNameLabel = (Label)PasswordRecovery1.UserNameTemplateContainer.FindControl("lblEnterUserName");

		var useEmailForLogin = (siteSettings != null) && (siteSettings.UseEmailForLogin);

		if (EnterUserNameLabel != null)
		{
			if (useEmailForLogin)
			{
				EnterUserNameLabel.Text = Resource.EnterEmailLabel;
			}
			else
			{
				EnterUserNameLabel.Text = Resource.EnterUserNameLabel;
			}

		}

		// UserName template
		var userNameNextButton = (Button)PasswordRecovery1.UserNameTemplateContainer.FindControl("SubmitButton");
		if (userNameNextButton != null)
		{
			userNameNextButton.Text = Resource.PasswordRecoveryNextButton;
			SiteUtils.SetButtonAccessKey(userNameNextButton, AccessKeys.PasswordRecoveryAccessKey);
		}

		var reqUserName = (RequiredFieldValidator)PasswordRecovery1.UserNameTemplateContainer.FindControl("UserNameRequired");

		if (reqUserName != null)
		{
			if (useEmailForLogin)
			{
				reqUserName.ErrorMessage = Resource.PasswordRecoveryEmailAddressRequiredWarning;
			}
			else
			{
				reqUserName.ErrorMessage = Resource.PasswordRecoveryUserNameRequiredWarning;
			}
		}

		// Question Template
		var questionNextButton = (Button)PasswordRecovery1.QuestionTemplateContainer.FindControl("SubmitButton");
		if (questionNextButton != null)
		{
			questionNextButton.Text = Resource.PasswordRecoveryNextButton;
			SiteUtils.SetButtonAccessKey(questionNextButton, AccessKeys.PasswordRecoveryAccessKey);
		}

		var reqAnswer = (RequiredFieldValidator)PasswordRecovery1.QuestionTemplateContainer.FindControl("AnswerRequired");

		if (reqAnswer != null)
		{
			reqAnswer.ErrorMessage = Resource.PasswordRecoveryAnswerRequired;
		}

		PasswordRecovery1.GeneralFailureText = string.Format(DisplaySettings.AlertErrorMarkup, Resource.PasswordRecoveryGeneralFailureText);
		PasswordRecovery1.QuestionFailureText = string.Format(DisplaySettings.AlertErrorMarkup, Resource.PasswordRecoveryQuestionFailureText);

		if (useEmailForLogin)
		{
			PasswordRecovery1.UserNameFailureText = string.Format(DisplaySettings.AlertErrorMarkup, Resource.PasswordRecoveryEmailAddressFailureText);
		}
		else
		{
			PasswordRecovery1.UserNameFailureText = string.Format(DisplaySettings.AlertErrorMarkup, Resource.PasswordRecoveryUserNameFailureText);
		}

		PasswordRecovery1.MailDefinition.From = siteSettings.DefaultEmailFromAddress;
		PasswordRecovery1.MailDefinition.Subject = string.Format(Resource.PasswordRecoveryEmailSubjectFormatString, siteSettings.SiteName);

		string emailFilename;

		string pattern;

		if (Membership.Provider.PasswordFormat == MembershipPasswordFormat.Hashed)
		{
			pattern = WebConfigSettings.HashedPasswordRecoveryEmailTemplateFileNamePattern;
		}
		else
		{
			pattern = WebConfigSettings.PasswordRecoveryEmailTemplateFileNamePattern;
		}

		emailFilename =
			ResourceHelper.GetFullResourceFilePath(
			CultureInfo.CurrentUICulture,
			GetEmailTemplatesFolder(),
			pattern);

		PasswordRecovery1.MailDefinition.BodyFileName =
			string.IsNullOrWhiteSpace(emailFilename) ?
			Server.MapPath($"~/Data/MessageTemplates/{SiteUtils.GetDefaultUICulture()}-{pattern}") :
			emailFilename;

		PasswordRecovery1.MailDefinition.IsBodyHtml = Core.Configuration.ConfigHelper.GetBoolProperty("UseHtmlBodyInPasswordRecoveryEmail", false);
		PasswordRecovery1.RenderOuterTable = false;

		AddClassToBody("passwordrecovery");
	}

	void PasswordRecovery1_SendingMail(object sender, MailMessageEventArgs e)
	{
		e.Message.Body = e.Message.Body.Replace("{SiteName}", siteSettings.SiteName);
		e.Message.Body = e.Message.Body.Replace("{SiteLink}", SiteUtils.GetNavigationSiteRoot());

		SmtpSettings smtpSettings = SiteUtils.GetSmtpSettings();
		Email.SetMessageEncoding(smtpSettings, e.Message);

		if (siteSettings.DefaultFromEmailAlias.Length > 0)
		{
			e.Message.From = new System.Net.Mail.MailAddress(siteSettings.DefaultEmailFromAddress, siteSettings.DefaultFromEmailAlias);
		}
		else
		{
			e.Message.From = new System.Net.Mail.MailAddress(siteSettings.DefaultEmailFromAddress);
		}
		Email.Send(smtpSettings, e.Message);
		e.Cancel = true; //stop here so the "built-in" mail routine doesn't run
	}

	void PasswordRecovery1_SendMailError(object sender, SendMailErrorEventArgs e)
	{
		var logMessage = "Unable to send password recovery email. Please check the SMTP Configuration";

		log.Error(logMessage, e.Exception);
		lblMailError.Text = Resource.PasswordRecoveryMailFailureMessage;
		e.Handled = true;
		var successLabel = (SiteLabel)PasswordRecovery1.SuccessTemplateContainer.FindControl("successLabel");
		if (successLabel != null)
		{
			successLabel.Visible = false;
		}
	}

	protected void RecoverPassword_Error(object sender, EventArgs e)
	{
		Exception rawException = Server.GetLastError();
		if (rawException is not null)
		{
			if (rawException is MojoMembershipException)
			{
				Server.ClearError();
				WebUtils.SetupRedirect(this, SiteUtils.GetNavigationSiteRoot());
				return;
			}
		}
	}

	private string GetEmailTemplatesFolder()
	{
		if (HttpContext.Current == null)
		{
			return String.Empty;
		}

		return HttpContext.Current.Server.MapPath(WebUtils.GetApplicationRoot() + "/Data/MessageTemplates") + System.IO.Path.DirectorySeparatorChar;
	}
}