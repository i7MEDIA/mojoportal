using System;
using System.Text;

namespace mojoPortal.Net;

/// <summary>
/// this class is deprecated, better to create an EmailMessageTask messageTask = new EmailMessageTask(SiteUtils.GetSmtpSettings());
/// </summary>
[Obsolete]
public sealed class Notification
{
	public static void SendPassword(
		SmtpSettings smtpSettings,
		string messageTemplate,
		string fromEmail,
		string userEmail,
		string userPassword,
		string siteName,
		string siteLink
	)
	{

		var message = new StringBuilder();

		message.Append(messageTemplate);
		message.Replace("{SiteName}", siteName);
		message.Replace("{AdminEmail}", fromEmail);
		message.Replace("{UserEmail}", userEmail);
		message.Replace("{UserPassword}", userPassword);
		message.Replace("{SiteLink}", siteLink);

		Email.SendEmail(
			smtpSettings,
			fromEmail,
			userEmail,
			string.Empty,
			string.Empty,
			siteName,
			message.ToString(),
			false,
			"Normal"
		);
	}

	public static void SendRegistrationConfirmationLink(
		SmtpSettings smtpSettings,
		string messageTemplate,
		string fromEmail,
		string fromAlias,
		string userEmail,
		string siteName,
		string confirmationLink
	)
	{
		var message = new StringBuilder();

		message.Append(messageTemplate);
		message.Replace("{SiteName}", siteName);
		message.Replace("{AdminEmail}", fromEmail);
		message.Replace("{UserEmail}", userEmail);
		message.Replace("{ConfirmationLink}", confirmationLink);

		Email.Send(
			smtpSettings,
			fromEmail,
			fromAlias,
			string.Empty,
			userEmail,
			string.Empty,
			string.Empty,
			siteName,
			message.ToString(),
			false,
			"Normal"
		);
	}
}