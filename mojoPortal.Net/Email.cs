using log4net;
using mojoPortal.Web.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace mojoPortal.Net
{
	/// <summary>
	/// A class for sending email.
	/// </summary>
	public static class Email
    {
       
        private static readonly ILog log = LogManager.GetLogger(typeof(Email));
        private static bool debugLog = log.IsDebugEnabled;

        public const string PriorityLow = "Low";
        public const string PriorityNormal = "Normal";
        public const string PriorityHigh = "High";

        const int SmtpAuthenticated = 1;

        

        
        public static void SendEmail(
            SmtpSettings smtpSettings,
            string from,
            string to,
            string cc,
            string bcc,
            string subject,
            string messageBody,
            bool html,
            string priority)
        {
            if (to == "admin@admin.com") { return; } //demo site

            if ((ConfigurationManager.AppSettings["DisableSmtp"] != null)&&(ConfigurationManager.AppSettings["DisableSmtp"] == "true"))
            {
                log.Info("Not Sending email because DisableSmtp is true in config.");
                return;
            }

            if ((smtpSettings == null) || (!smtpSettings.IsValid))
            {
                log.Error("Invalid smtp settings detected in SendEmail ");
                return;
            }

            SendEmailNormal(smtpSettings, from, to, cc, bcc, subject, messageBody, html, priority);
            return;

           
        }

        
        public static void SendEmail(
            SmtpSettings smtpSettings,
            string from,
            string replyTo,
            string to,
            string cc,
            string bcc,
            string subject,
            string messageBody,
            bool html,
            string priority)
        {
            if (to == "admin@admin.com") { return; } //demo site

            if ((ConfigurationManager.AppSettings["DisableSmtp"] != null) && (ConfigurationManager.AppSettings["DisableSmtp"] == "true"))
            {
                log.Info("Not Sending email because DisableSmtp is true in config.");
                return;
            }

            if ((smtpSettings == null) || (!smtpSettings.IsValid))
            {
                log.Error("Invalid smtp settings detected in SendEmail ");
                return;
            }

           
            if (replyTo.Length > 0)
            {
                SendEmailNormal(
                    smtpSettings,
                    from,
                    replyTo,
                    to,
                    cc,
                    bcc,
                    subject,
                    messageBody,
                    html,
                    priority);

                return;
            }

            SendEmail(
                smtpSettings,
                from,
                to,
                cc,
                bcc,
                subject,
                messageBody,
                html,
                priority);

        }

        /// <summary>
        /// This method uses the built in .NET classes to send mail.
        /// </summary>
        /// <param name="smtpSettings"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="messageBody"></param>
        /// <param name="html"></param>
        /// <param name="priority"></param>
        public static void SendEmailNormal(
            SmtpSettings smtpSettings,
            string from,
            string to,
            string cc,
            string bcc,
            string subject,
            string messageBody,
            bool html,
            string priority)
        {
            if (to == "admin@admin.com") { return; } //demo site

            if ((ConfigurationManager.AppSettings["DisableSmtp"] != null) && (ConfigurationManager.AppSettings["DisableSmtp"] == "true"))
            {
                log.Info("Not Sending email because DisableSmtp is true in config.");
                return;
            }

            string replyTo = string.Empty;
            SendEmailNormal(
                smtpSettings,
                from,
                replyTo,
                to,
                cc,
                bcc,
                subject,
                messageBody,
                html,
                priority);

        }

        /// <summary>
        /// This method uses the built in .NET classes to send mail.
        /// </summary>
        /// <param name="smtpSettings"></param>
        /// <param name="from"></param>
        /// <param name="replyTo"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="messageBody"></param>
        /// <param name="html"></param>
        /// <param name="priority"></param>
        public static void SendEmailNormal(
            SmtpSettings smtpSettings,
            string from,
            string replyTo,
            string to,
            string cc,
            string bcc,
            string subject,
            string messageBody,
            bool html,
            string priority)
        {
            if (to == "admin@admin.com") { return; } //demo site

            if ((ConfigurationManager.AppSettings["DisableSmtp"] != null) && (ConfigurationManager.AppSettings["DisableSmtp"] == "true"))
            {
                log.Info("Not Sending email because DisableSmtp is true in config.");
                return;
            }

            string[] attachmentPaths = new string[0];
            string[] attachmentNames = new string[0];

            SendEmailNormal(
                smtpSettings,
                from,
                replyTo,
                to,
                cc,
                bcc,
                subject,
                messageBody,
                html,
                priority,
                attachmentPaths,
                attachmentNames);

            

        }

        /// <summary>
        /// This method uses the built in .NET classes to send mail.
        /// </summary>
        /// <param name="smtpSettings"></param>
        /// <param name="from"></param>
        /// <param name="replyTo"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="messageBody"></param>
        /// <param name="html"></param>
        /// <param name="priority"></param>
        /// <param name="attachmentPaths"></param>
        /// <param name="attachmentNames"></param>
        public static void SendEmailNormal(
            SmtpSettings smtpSettings,
            string from,
            string replyTo,
            string to,
            string cc,
            string bcc,
            string subject,
            string messageBody,
            bool html,
            string priority,
            string[] attachmentPaths,
            string[] attachmentNames)
        {
            if (to == "admin@admin.com") { return; } //demo site

            Send(smtpSettings,
                from,
                replyTo,
                to,
                cc,
                bcc,
                subject,
                messageBody,
                html,
                priority,
                attachmentPaths,
                attachmentNames);

        }

        public static void SetMessageEncoding(SmtpSettings smtpSettings, MailMessage mail)
        {
            //http://msdn.microsoft.com/en-us/library/system.text.encoding.aspx

            if (smtpSettings.PreferredEncoding.Length > 0)
            {
                switch (smtpSettings.PreferredEncoding)
                {
                    case "ascii":
                    case "us-ascii":
                        // do nothing since this is the default
                        break;

                    case "utf32":
                    case "utf-32":

                        mail.BodyEncoding = Encoding.UTF32;
                        mail.SubjectEncoding = Encoding.UTF32;

                        break;

                    case "unicode":

                        mail.BodyEncoding = Encoding.Unicode;
                        mail.SubjectEncoding = Encoding.Unicode;

                        break;

                    case "utf8":
                    case "utf-8":

                        mail.BodyEncoding = Encoding.UTF8;
                        mail.SubjectEncoding = Encoding.UTF8;

                        break;

                    default:

                        try
                        {
                            mail.BodyEncoding = Encoding.GetEncoding(smtpSettings.PreferredEncoding);
                            mail.SubjectEncoding = Encoding.GetEncoding(smtpSettings.PreferredEncoding);
                        }
                        catch (ArgumentException ex)
                        {
                            log.Error(ex);
                        }

                        break;
                }

            }

        }

        /// <summary>
        /// This method uses the built in .NET classes to send mail.
        /// </summary>
        /// <param name="smtpSettings"></param>
        /// <param name="from"></param>
        /// <param name="replyTo"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="messageBody"></param>
        /// <param name="html"></param>
        /// <param name="priority"></param>
        /// <param name="attachmentPaths"></param>
        /// <param name="attachmentNames"></param>
        public static bool Send(
            SmtpSettings smtpSettings,
            string from,
            string replyTo,
            string to,
            string cc,
            string bcc,
            string subject,
            string messageBody,
            bool html,
            string priority,
            string[] attachmentPaths,
            string[] attachmentNames)
        {
            if (to == "admin@admin.com") { return false; } //demo site

            string fromAlias = string.Empty;
            return Send(
                smtpSettings,
                from,
                fromAlias,
                replyTo,
                to,
                cc,
                bcc,
                subject,
                messageBody,
                html,
                priority,
                attachmentPaths,
                attachmentNames);
                
        }
		public static bool Send(
			SmtpSettings smtpSettings,
			string from,
			string fromAlias,
			string replyTo,
			string to,
			string cc,
			string bcc,
			string subject,
			string messageBody,
			bool html,
			string priority)
		{
			return Send(smtpSettings, from, fromAlias, replyTo, to, cc, bcc, subject, messageBody, html, priority, out _);
		}


		public static bool Send(
            SmtpSettings smtpSettings,
            string from,
            string fromAlias,
            string replyTo,
            string to,
            string cc,
            string bcc,
            string subject,
            string messageBody,
            bool html,
            string priority,
			out string result)
        {
            if (to == "admin@admin.com") {
				//demo site
				result = "can't use admin@admin.com email address";
				return false;
			}

            string[] attachmentPaths = new string[0];
            string[] attachmentNames = new string[0];
			//result = string.Empty;
            return Send(
                smtpSettings,
                from,
                fromAlias,
                replyTo,
                to,
                cc,
                bcc,
                subject,
                messageBody,
                html,
                priority,
                attachmentPaths,
                attachmentNames,
				out result);
        }

		public static bool Send(
			SmtpSettings smtpSettings,
			string from,
			string fromAlias,
			string replyTo,
			string to,
			string cc,
			string bcc,
			string subject,
			string messageBody,
			bool html,
			string priority,
			string[] attachmentPaths,
			string[] attachmentNames)
		{
			return Send(smtpSettings,
				from,
				fromAlias,
				replyTo,
				to,
				cc,
				bcc,
				subject,
				messageBody,
				html,
				priority,
				attachmentPaths,
				attachmentNames,
				out _);
		}

		/// <summary>
		/// This method uses the built in .NET classes to send mail.
		/// </summary>
		public static bool Send(
            SmtpSettings smtpSettings,
            string from,
            string fromAlias,
            string replyTo,
            string to,
            string cc,
            string bcc,
            string subject,
            string messageBody,
            bool html,
            string priority,
            string[] attachmentPaths,
            string[] attachmentNames,
			out string result)
        {
            
                // add attachments if there are any
                List<Attachment> attachments = new List<Attachment>();
                if ((attachmentPaths.Length > 0) && (attachmentNames.Length == attachmentPaths.Length))
                {
                    for (int i = 0; i < attachmentPaths.Length; i++)
                    {
                        if (!File.Exists(attachmentPaths[i]))
                        {
                            log.Error("could not find file for email attachment " + attachmentPaths[i]);
                            continue;
                        }

                        Attachment a = new Attachment(attachmentPaths[i]);
                        a.Name = attachmentNames[i];
                        //mail.Attachments.Add(a);
                        attachments.Add(a);

                    }

                }

                return Send(
                    smtpSettings,
                    from,
                    fromAlias,
                    replyTo,
                    to,
                    cc,
                    bcc,
                    subject,
                    messageBody,
                    html,
                    priority,
                    attachments,
					out result);


        }
		public static bool Send(
			SmtpSettings smtpSettings,
			string from,
			string fromAlias,
			string replyTo,
			string to,
			string cc,
			string bcc,
			string subject,
			string messageBody,
			bool html,
			string priority,
			List<Attachment> attachments)
		{
			return Send(smtpSettings, from, fromAlias, replyTo, to, cc, bcc, subject, messageBody, html, priority, attachments, out _);
		}

		public static bool Send(
            SmtpSettings smtpSettings,
            string from,
            string fromAlias,
            string replyTo,
            string to,
            string cc,
            string bcc,
            string subject,
            string messageBody,
            bool html,
            string priority,
            List<Attachment> attachments,
			out string result)
        {
            if (to == "admin@admin.com") {
				//demo site
				result = "can't use admin@admin.com email address";
				return false;
			}

            if ((ConfigurationManager.AppSettings["DisableSmtp"] != null) && (ConfigurationManager.AppSettings["DisableSmtp"] == "true"))
            {
				result = "Not Sending email because DisableSmtp is true in config.";
                log.Info(result);
				return false;
            }

            if ((smtpSettings == null) || (!smtpSettings.IsValid))
            {
				result = "Invalid smtp settings detected in Email.Send ";
				log.Error(result);
                return false;
            }

            if (debugLog) log.Debug($"In Email.Send({from}, {to}, {cc}, {bcc}, {subject}, {messageBody}, {html}, {priority})");

            using (MailMessage mail = new MailMessage())
            {
                SetMessageEncoding(smtpSettings, mail);


                MailAddress fromAddress;
                try
                {
                    if (fromAlias.Length > 0)
                    {
                        fromAddress = new MailAddress(from, fromAlias);
                    }
                    else
                    {
                        fromAddress = new MailAddress(from);
                    }
                }
                catch (ArgumentException)
                {
					result = $"invalid from address {from}";
                    log.Error(result);
                    log.Info("no valid from address was provided so not sending message " + messageBody);
                    return false;
                }
                catch (FormatException)
                {
					result = $"invalid from address {from}";
					log.Error(result);
					log.Info("no valid from address was provided so not sending message " + messageBody);
                    return false;
                }

                mail.From = fromAddress;

                List<string> toAddresses = to.Replace(";", ",").SplitOnChar(',');
                foreach (string toAddress in toAddresses)
                {
                    try
                    {
                        MailAddress a = new MailAddress(toAddress);
                        mail.To.Add(a);
                    }
                    catch (ArgumentException)
                    {
                        log.Error("ignoring invalid to address " + toAddress);
                    }
                    catch (FormatException)
                    {
                        log.Error("ignoring invalid to address " + toAddress);
                    }

                }

                if (mail.To.Count == 0)
                {
					result = $"no valid to address was provided so not sending message {messageBody}";
					log.Error(result);
                    return false;
                }

                if (replyTo.Length > 0)
                {
                    try
                    {
                        MailAddress replyAddress = new(replyTo);
                        mail.ReplyToList.Add(replyAddress);
                    }
                    catch (ArgumentException)
                    {
                        log.Error("ignoring invalid replyto address " + replyTo);
                    }
                    catch (FormatException)
                    {
                        log.Error("ignoring invalid replyto address " + replyTo);
                    }
                }

                if (cc.Length > 0)
                {
                    List<string> ccAddresses = cc.Replace(";", ",").SplitOnChar(',');

                    foreach (string ccAddress in ccAddresses)
                    {
                        try
                        {
                            MailAddress a = new MailAddress(ccAddress);
                            mail.CC.Add(a);
                        }
                        catch (ArgumentException)
                        {
                            log.Error("ignoring invalid cc address " + ccAddress);
                        }
                        catch (FormatException)
                        {
                            log.Error("ignoring invalid cc address " + ccAddress);
                        }
                    }

                }

                if (bcc.Length > 0)
                {
                    List<string> bccAddresses = bcc.Replace(";", ",").SplitOnChar(',');

                    foreach (string bccAddress in bccAddresses)
                    {
                        try
                        {
                            MailAddress a = new MailAddress(bccAddress);
                            mail.Bcc.Add(a);
                        }
                        catch (ArgumentException)
                        {
                            log.Error("invalid bcc address " + bccAddress);
                        }
                        catch (FormatException)
                        {
                            log.Error("invalid bcc address " + bccAddress);
                        }
                    }

                }

                mail.Subject = subject.RemoveLineBreaks();

                switch (priority)
                {
                    case PriorityHigh:
                        mail.Priority = MailPriority.High;
                        break;

                    case PriorityLow:
                        mail.Priority = MailPriority.Low;
                        break;

                    case PriorityNormal:
                    default:
                        mail.Priority = MailPriority.Normal;
                        break;

                }



                if (html)
                {
                    mail.IsBodyHtml = true;
                    // this char can reportedly cause problems in some email clients so replace it if it exists
                    mail.Body = messageBody.Replace("\xA0", "&nbsp;");
                }
                else
                {
                    mail.Body = messageBody;
                }

                // add attachments if there are any
                if (attachments != null)
                {
                    foreach (Attachment a in attachments)
                    {
                        mail.Attachments.Add(a);
                    }
                }

                if (smtpSettings.AddBulkMailHeader)
                {
                    mail.Headers.Add("Precedence", "bulk");
                }

                return Send(smtpSettings, mail, out result);

            }// end using MailMessage

        }

        private static string GetGlobalBccAddress()
        {
            // I use this for the demo site so I get copied on every message
            // so that I will know if anyone is managing to send spam from the demo site

            if ((ConfigurationManager.AppSettings["GlobalBCC"] != null)&&(ConfigurationManager.AppSettings["GlobalBCC"].Length > 0))
            {
                return ConfigurationManager.AppSettings["GlobalBCC"];
            }

            return string.Empty;

        }
		public static bool Send(SmtpSettings smtpSettings, MailMessage message)
		{
			return Send(smtpSettings, message, out _);
		}
        public static bool Send(SmtpSettings smtpSettings, MailMessage message, out string result)
        {
            if (message.To.ToString() == "admin@admin.com")
			{ 
				//demo site
				result = "can't use admin@admin.com email address";
				return false;
			} 

            string globalBcc = GetGlobalBccAddress();
            if (globalBcc.Length > 0)
            {
                MailAddress bcc = new MailAddress(globalBcc);
                message.Bcc.Add(bcc);
            }

            int timeoutMilliseconds = ConfigHelper.GetIntProperty("SMTPTimeoutInMilliseconds", 15000);
            SmtpClient smtpClient = new SmtpClient(smtpSettings.Server, smtpSettings.Port);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = smtpSettings.UseSsl;
            smtpClient.Timeout = timeoutMilliseconds;

            if (smtpSettings.RequiresAuthentication)
            {

                NetworkCredential smtpCredential
                    = new NetworkCredential(
                        smtpSettings.User,
                        smtpSettings.Password);

                CredentialCache myCache = new CredentialCache();
                myCache.Add(smtpSettings.Server, smtpSettings.Port, "LOGIN", smtpCredential);

                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = myCache;
            }
            else
            {
                //aded 2010-01-22 JA
                smtpClient.UseDefaultCredentials = true;
            }

            foreach (var header in smtpSettings.AdditionalHeaders)
            {
                message.Headers.Add(header.Name, header.Value);
            }

            //message.Headers.Add(smtpSettings.AdditionalHeaders);
            if (!string.IsNullOrWhiteSpace(smtpSettings.SenderHeader))
                message.Headers.Add("X-mojo-Sender", smtpSettings.SenderHeader);

            try
            {
                smtpClient.Send(message);
                //log.Debug("Sent Message: " + subject);
                //log.Info("Sent Message: " + subject);

                bool logEmail = ConfigHelper.GetBoolProperty("LogAllEmailsWithSubject", false);

                if (logEmail) 
                {
                    log.Info("Sent message " + message.Subject + " to " + message.To[0].Address); 
                }
				result = "sent";
                return true;
            }
            catch (System.Net.Mail.SmtpException ex)
            {
				//log.Error("error sending email to " + to + " from " + from, ex);
				result = $"error: {ex}";
                log.Error("error sending email to " + message.To.ToString() + " from " + message.From.ToString() + ", will retry", ex);
                return RetrySend(message, smtpClient, ex);

            }
            catch (WebException ex)
            {
				result = $"error: {ex}";
                log.Error("error sending email to " + message.To.ToString() + " from " + message.From.ToString() + ", message was: " + message.Body, ex);
                return false;
            }
            catch (SocketException ex)
            {
				result = $"error: {ex}";
				log.Error("error sending email to " + message.To.ToString() + " from " + message.From.ToString() + ", message was: " + message.Body, ex);
                return false;
            }
            catch (InvalidOperationException ex)
            {
				result = $"error: {ex}";
				log.Error("error sending email to " + message.To.ToString() + " from " + message.From.ToString() + ", message was: " + message.Body, ex);
                return false;
            }
            catch (FormatException ex)
            {
				result = $"error: {ex}";
				log.Error("error sending email to " + message.To.ToString() + " from " + message.From.ToString() + ", message was: " + message.Body, ex);
                return false;
            }

        }

		private static bool RetrySend(MailMessage message, SmtpClient smtp, Exception ex)
		{
			return RetrySend(message, smtp, ex, out _);
		}


		private static bool RetrySend(MailMessage message, SmtpClient smtp, Exception ex, out string result)
        {
            //retry
            int timesToRetry = ConfigHelper.GetIntProperty("TimesToRetryOnSmtpError", 3);
            for (int i = 1; i <= timesToRetry; )
            {
                if (RetrySend(message, smtp, i)) { result = "sent"; return true; }
                i += 1;
                Thread.Sleep(1000); // 1 second sleep in case it is a temporary network issue
            }

            // allows use of localhost as  backup 
            if (ConfigurationManager.AppSettings["BackupSmtpServer"] != null)
            {
                string backupServer = ConfigurationManager.AppSettings["BackupSmtpServer"];
                int timeoutMilliseconds = ConfigHelper.GetIntProperty("SMTPTimeoutInMilliseconds", 15000);
                int backupSmtpPort = ConfigHelper.GetIntProperty("BackupSmtpPort", 25);
                SmtpClient smtpClient = new SmtpClient(backupServer, backupSmtpPort);
                smtpClient.UseDefaultCredentials = true;

                try
                {
                    smtpClient.Send(message);
                    log.Info("success using backup smtp server sending email to " + message.To.ToString() + " from " + message.From);
					result = "sent";
                    return true;
                }
                catch (System.Net.Mail.SmtpException) { }
                catch (WebException) { }
                catch (SocketException) { }
                catch (InvalidOperationException) { }
                catch (FormatException) { }

            }

            //log.Info("all retries failed sending email to " + message.To.ToString() + " from " + message.From);
            log.Error("all retries failed sending email to " + message.To.ToString() + " from " + message.From.ToString() + ", message was: " + message.Body, ex);
			result = "fail";
            return false;

        }

        private static bool RetrySend(MailMessage message, SmtpClient smtp, int tryNumber)
        {
            try
            {
                smtp.Send(message);
                log.Info("success on retry " + tryNumber.ToInvariantString() + " sending email to " + message.To.ToString() + " from " + message.From);
                return true;
            }
            catch (System.Net.Mail.SmtpException) { }
            catch (WebException) { }
            catch (SocketException) { }
            catch (InvalidOperationException) { }
            catch (FormatException) { }

            return false;
        }

        public static bool IsValidEmailAddressSyntax(string emailAddress)
        {
            return SecurityHelper.IsValidEmailAddress(emailAddress);
            //Regex emailPattern;
            ////emailPattern = new Regex("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$");
            //emailPattern = new Regex(SecurityHelper.GetEmailRegexExpression());

            //Match emailAddressToValidate = emailPattern.Match(emailAddress);

            //if (emailAddressToValidate.Success)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}

        }

    }
}
