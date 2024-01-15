using System.Text;
using log4net;

namespace mojoPortal.Net
{
	/// <summary>
	/// this class is deprecated, better to create an EmailMessageTask messageTask = new EmailMessageTask(SiteUtils.GetSmtpSettings());
	/// </summary>
	public sealed class Notification
    {
        // Create a logger for use in this class
        private static readonly ILog log = LogManager.GetLogger(typeof(Notification));
        private static bool debugLog = log.IsDebugEnabled;

        private Notification()
        {
            //only public static methods so no public constructor
        }

        public static void SendPassword(
            SmtpSettings smtpSettings,
            string messageTemplate,
            string fromEmail,
            string userEmail,
            string userPassword,
            string siteName,
            string siteLink)
        {

            StringBuilder message = new StringBuilder();
            message.Append(messageTemplate);
            message.Replace("{SiteName}", siteName);
            message.Replace("{AdminEmail}", fromEmail);
            message.Replace("{UserEmail}", userEmail);
            message.Replace("{UserPassword}", userPassword);
            message.Replace("{SiteLink}", siteLink);

            Email.SendEmail(
                smtpSettings,
                fromEmail,
                userEmail, "", "",
                siteName,
                message.ToString(), false, "Normal");
        }

        public static void SendRegistrationConfirmationLink(
            SmtpSettings smtpSettings,
            string messageTemplate,
            string fromEmail,
            string fromAlias,
            string userEmail,
            string siteName,
            string confirmationLink)
        {

            StringBuilder message = new StringBuilder();
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
                "Normal");
        }
    }
}