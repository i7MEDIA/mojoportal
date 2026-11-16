using mojoPortal.Net;
using System.Text;

namespace mojoPortal.Web.BlogUI
{
	public static class BlogNotification
	{
		public static void SendBlogCommentNotificationEmail(
			SmtpSettings smtpSettings,
			string messageTemplate,
			string fromEmail,
			string siteName,
			string authorEmail,
			string messageLink)
		{

			StringBuilder message = new StringBuilder();
			message.Append(messageTemplate);
			message.Replace("{SiteName}", siteName);
			message.Replace("{MessageLink}", messageLink);

			smtpSettings.SenderHeader = "BlogNotification";

			Email.SendEmail(
				smtpSettings,
				fromEmail,
				authorEmail, "", "",
				siteName,
				message.ToString(), false, "Normal");



		}

	}
}