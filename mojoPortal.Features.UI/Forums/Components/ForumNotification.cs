

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web;
using mojoPortal.Net;
using mojoPortal.Web.Framework;
using mojoPortal.Business;

namespace mojoPortal.Web.ForumUI
{
    public static class ForumNotification
    {
        public static void SendForumModeratorNotificationEmail(object oNotificationInfo)
        {
            if (oNotificationInfo == null) return;
            if (!(oNotificationInfo is ForumNotificationInfo)) return;

            ForumNotificationInfo notificationInfo = oNotificationInfo as ForumNotificationInfo;

            if (notificationInfo.ModeratorEmailAddresses == null) return;

            foreach(string emailAddress in notificationInfo.ModeratorEmailAddresses)
            {
                StringBuilder body = new StringBuilder();

               
                body.Append(notificationInfo.ModeratorTemplate);
                body.Replace("{SiteName}", notificationInfo.SiteName);
                body.Replace("{ModuleName}", notificationInfo.ModuleName);
                body.Replace("{ForumName}", notificationInfo.ForumName);
                body.Replace("{AdminEmail}", notificationInfo.FromEmail);
                body.Replace("{MessageLink}", notificationInfo.MessageLink);
                body.Replace("{MessageBody}", notificationInfo.MessageBody);
                //body.Replace("{UnsubscribeForumThreadLink}", notificationInfo.UnsubscribeForumThreadLink + "?ts=" + threadSubGuid.ToString());
                //body.Replace("{UnsubscribeForumLink}", notificationInfo.UnsubscribeForumLink + "?fs=" + forumSubGuid.ToString());

                StringBuilder emailSubject = new StringBuilder();
                if (notificationInfo.SubjectTemplate.Length == 0)
                {
                    notificationInfo.SubjectTemplate = "[{SiteName} - {ForumName}] {Subject}";
                }
                emailSubject.Append(notificationInfo.SubjectTemplate);
                emailSubject.Replace("{SiteName}", notificationInfo.SiteName);
                emailSubject.Replace("{ModuleName}", notificationInfo.ModuleName);
                emailSubject.Replace("{ForumName}", notificationInfo.ForumName);
                emailSubject.Replace("{Subject}", notificationInfo.Subject);

                Email.Send(
                    notificationInfo.SmtpSettings,
                    notificationInfo.FromEmail,
                    notificationInfo.FromAlias,
                    string.Empty,
                    emailAddress,
                    string.Empty,
                    string.Empty,
                    emailSubject.ToString(),
                    body.ToString(),
                    false,
                    "Normal");


            }

        }

        public static void SendForumNotificationEmail(object oNotificationInfo)
        {
            if (oNotificationInfo == null) return;
            if (!(oNotificationInfo is ForumNotificationInfo)) return;

            ForumNotificationInfo notificationInfo = oNotificationInfo as ForumNotificationInfo;

            if (notificationInfo.Subscribers == null) return;

            //if (debugLog) log.Debug("In SendForumNotificationEmail()");
            if (notificationInfo.Subscribers.Tables.Count > 0)
            {
                if (notificationInfo.Subscribers.Tables[0].Rows.Count > 0)
                {
                    int timeoutBetweenMessages = ConfigHelper.GetIntProperty("SmtpTimeoutBetweenMessages", 1000);
                    // use the same setting as newsletter to throttle the send rate, sending too fast can make you appear as a spammer
                    int maxPerMinute = ConfigHelper.GetIntProperty("Forum:NotificationMaxToSendPerMinute", 0);

                    int sentSoFar = 0;

                    notificationInfo.SmtpSettings.AddBulkMailHeader = true;


                    foreach (DataRow row in notificationInfo.Subscribers.Tables[0].Rows)
                    {
                        int threadsubId = Convert.ToInt32(row["ThreadSubID"]);
                        int forumsubId = Convert.ToInt32(row["ForumSubID"]);
                        Guid threadSubGuid = new Guid(row["ThreadSubGuid"].ToString());
                        Guid forumSubGuid = new Guid(row["ForumSubGuid"].ToString());

                        StringBuilder body = new StringBuilder();

                        body.Append(notificationInfo.MessageBody);

                        if ((threadsubId > -1) && (forumsubId > -1))
                        {
                            body.Append(notificationInfo.BodyTemplate);
                        }
                        else if (forumsubId > -1)
                        {
                            body.Append(notificationInfo.ForumOnlyTemplate);
                        }
                        else if (threadsubId > -1)
                        {
                            body.Append(notificationInfo.ThreadOnlyTemplate);
                        }

                        body.Replace("{SiteName}", notificationInfo.SiteName);
                        body.Replace("{ModuleName}", notificationInfo.ModuleName);
                        body.Replace("{ForumName}", notificationInfo.ForumName);
                        body.Replace("{AdminEmail}", notificationInfo.FromEmail);
                        body.Replace("{MessageLink}", notificationInfo.MessageLink);
                        body.Replace("{UnsubscribeForumThreadLink}", notificationInfo.UnsubscribeForumThreadLink + "?ts=" + threadSubGuid.ToString());
                        body.Replace("{UnsubscribeForumLink}", notificationInfo.UnsubscribeForumLink + "?fs=" + forumSubGuid.ToString());

                        StringBuilder emailSubject = new StringBuilder();
                        if (notificationInfo.SubjectTemplate.Length == 0)
                        {
                            notificationInfo.SubjectTemplate = "[{SiteName} - {ForumName}] {Subject}";
                        }
                        emailSubject.Append(notificationInfo.SubjectTemplate);
                        emailSubject.Replace("{SiteName}", notificationInfo.SiteName);
                        emailSubject.Replace("{ModuleName}", notificationInfo.ModuleName);
                        emailSubject.Replace("{ForumName}", notificationInfo.ForumName);
                        emailSubject.Replace("{Subject}", notificationInfo.Subject);

                        Email.Send(
                            notificationInfo.SmtpSettings,
                            notificationInfo.FromEmail,
                            notificationInfo.FromAlias,
                            string.Empty,
                            row["Email"].ToString(),
                            string.Empty,
                            string.Empty,
                            emailSubject.ToString(),
                            body.ToString(),
                            false,
                            "Normal");

                        sentSoFar += 1;

                        if (sentSoFar == maxPerMinute)
                        {
                            Thread.Sleep(60000); //sleep 1 minute
                            sentSoFar = 0; //reset counter
                        }
                        else
                        {

                            Thread.Sleep(timeoutBetweenMessages);
                        }
                    } // end for each row


                }
            }

            if((notificationInfo.PostId > -1)&&(notificationInfo.ThreadId > -1))
            {
                ForumThread thread = new ForumThread(notificationInfo.ThreadId, notificationInfo.PostId);
                if (thread.PostId > -1)
                {
                    thread.NotificationSent = true;
                    thread.UpdatePost();
                }
                
            }

        }

        public static void NotifySubscribers(
            Forum forum,
            ForumThread thread,
            Module module,
            SiteUser siteUser,
            SiteSettings siteSettings,
            ForumConfiguration config,
            string siteRoot,
            int pageId,
            int pageNumber,
            CultureInfo defaultCulture,
            SmtpSettings smtpSettings,
            bool notifyModeratorOnly)
        {
            string threadViewUrl;

            if (ForumConfiguration.CombineUrlParams)
            {
                threadViewUrl = siteRoot + "/Forums/Thread.aspx?pageid=" + pageId.ToInvariantString()
                    + "&t=" + thread.ThreadId.ToInvariantString()
                    + "~" + pageNumber.ToInvariantString()
                    + "#post" + thread.PostId.ToInvariantString();
            }
            else
            {
                threadViewUrl = siteRoot + "/Forums/Thread.aspx?thread="
                    + thread.ThreadId.ToInvariantString()
                    + "&mid=" + module.ModuleId.ToInvariantString()
                    + "&pageid=" + pageId.ToInvariantString()
                    + "&ItemID=" + forum.ItemId.ToInvariantString()
                    + "&pagenumber=" + pageNumber.ToInvariantString()
                    + "#post" + thread.PostId.ToInvariantString();
            }

			ForumNotificationInfo notificationInfo = new ForumNotificationInfo
			{
				ThreadId = thread.ThreadId,
				PostId = thread.PostId,
				SubjectTemplate = ResourceHelper.GetMessageTemplate(defaultCulture, "ForumNotificationEmailSubject.config")
			};

			if (config.IncludePostBodyInNotification)
            {
                string postedBy = string.Empty;
                if (siteUser != null)
                {
                    string sigFormat = ResourceHelper.GetResourceString("ForumResources", "PostedByFormat", defaultCulture, true);
                    postedBy = string.Format(CultureInfo.InvariantCulture, sigFormat, siteUser.Name) + "\r\n\r\n"; ;
                }

                string bodyWithFullLinks = SiteUtils.ChangeRelativeLinksToFullyQualifiedLinks(
                    siteRoot,
                    thread.PostMessage);

                List<string> urls = SiteUtils.ExtractUrls(bodyWithFullLinks);

                notificationInfo.MessageBody = System.Web.HttpUtility.HtmlDecode(SecurityHelper.RemoveMarkup(thread.PostMessage));

                if (urls.Count > 0)
                {
                    notificationInfo.MessageBody += "\r\n" + ResourceHelper.GetResourceString("ForumResources", "PostedLinks", defaultCulture, true);
                    foreach (string s in urls)
                    {
                        notificationInfo.MessageBody += "\r\n" + s.Replace("&amp;", "&"); // html decode url params
                    }

                    notificationInfo.MessageBody += "\r\n\r\n";
                }

                notificationInfo.MessageBody += "\r\n\r\n" + postedBy;
            }

            notificationInfo.BodyTemplate = ResourceHelper.GetMessageTemplate(defaultCulture, "ForumNotificationEmail.config");
            notificationInfo.ForumOnlyTemplate = ResourceHelper.GetMessageTemplate(defaultCulture, "ForumNotificationEmail-ForumOnly.config");
            notificationInfo.ThreadOnlyTemplate = ResourceHelper.GetMessageTemplate(defaultCulture, "ForumNotificationEmail-ThreadOnly.config");
            notificationInfo.ModeratorTemplate = ResourceHelper.GetMessageTemplate(defaultCulture, "ForumModeratorNotificationEmail.config");

            notificationInfo.FromEmail = siteSettings.DefaultEmailFromAddress;
            notificationInfo.FromAlias = siteSettings.DefaultFromEmailAlias;

            if (config.OverrideNotificationFromAddress.Length > 0)
            {
                notificationInfo.FromEmail = config.OverrideNotificationFromAddress;
                notificationInfo.FromAlias = config.OverrideNotificationFromAlias;
            }

            notificationInfo.SiteName = siteSettings.SiteName;
            notificationInfo.ModuleName = module.ModuleTitle;
            notificationInfo.ForumName = forum.Title;
            notificationInfo.Subject = SecurityHelper.RemoveMarkup(thread.PostSubject);

            notificationInfo.MessageLink = threadViewUrl;

            notificationInfo.UnsubscribeForumThreadLink = siteRoot + "/Forums/UnsubscribeThread.aspx";
            notificationInfo.UnsubscribeForumLink = siteRoot + "/Forums/UnsubscribeForum.aspx";

            notificationInfo.SmtpSettings = smtpSettings;

            if (notifyModeratorOnly)
            {
                // just send notification to moderator
                List<string> moderatorsEmails = forum.ModeratorNotifyEmail.SplitOnChar(',');
                if (moderatorsEmails.Count > 0)
                {
                    notificationInfo.ModeratorEmailAddresses = moderatorsEmails;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ForumNotification.SendForumModeratorNotificationEmail), notificationInfo);
                }

            }
            else
            {

                // Send notification to subscribers
                DataSet dsThreadSubscribers = thread.GetThreadSubscribers(config.IncludeCurrentUserInNotifications);
                notificationInfo.Subscribers = dsThreadSubscribers;


                ThreadPool.QueueUserWorkItem(new WaitCallback(ForumNotification.SendForumNotificationEmail), notificationInfo);
            }

           



        }

    }
}