// Author:					
// Created:				    2008-07-20
// Last Modified:			2011-03-21
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 

using System;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using log4net;
using mojoPortal.Business;
using mojoPortal.Net;

namespace mojoPortal.Web
{
    /// <summary>
    /// This is a simple task for sending a single email message. 
    /// 
    /// </summary>
    [Serializable()]
    public class EmailMessageTask : ITaskQueueTask
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EmailMessageTask));

        public EmailMessageTask()
        { }

        public EmailMessageTask(SmtpSettings smtpSettings)
        {
            User = smtpSettings.User;
            Password = smtpSettings.Password;
            Server = smtpSettings.Server;
            Port = smtpSettings.Port;
            RequiresAuthentication = smtpSettings.RequiresAuthentication;
            UseSsl = smtpSettings.UseSsl;
            PreferredEncoding = smtpSettings.PreferredEncoding;
        }

        #region ITaskQueueTask

        private Guid taskGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid queuedBy = Guid.Empty;
        private string taskName = "Email Message Task";
        private bool notifyOnCompletion = false;
        private string notificationToEmail = String.Empty;
        private string notificationFromEmail = String.Empty;
        private string notificationSubject = String.Empty;
        private string taskCompleteMessage = string.Empty;
        private string statusQueuedMessage = "Queued";
        private string statusStartedMessage = "Started";
        private string statusRunningMessage = "Running.";
        private string statusCompleteMessage = "Complete";
        private bool canStop = false;
        private bool canResume = false;
        // report status every 15 seconds by default
        private int updateFrequency = 15;

        #region Public ITaskQueueTask Properties

        public Guid TaskGuid
        {
            get { return taskGuid; }
            set { taskGuid = value; }
        }

        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }

        public Guid QueuedBy
        {
            get { return queuedBy; }
            set { queuedBy = value; }
        }

        public string TaskName
        {
            get { return taskName; }
            set { 
                //taskName = value; 
            }
        }

        public bool NotifyOnCompletion
        {
            get { return notifyOnCompletion; }
            set { notifyOnCompletion = value; }
        }

        public string NotificationToEmail
        {
            get { return notificationToEmail; }
            set { notificationToEmail = value; }
        }

        public string NotificationFromEmail
        {
            get { return notificationFromEmail; }
            set { notificationFromEmail = value; }
        }

        public string NotificationSubject
        {
            get { return notificationSubject; }
            set { notificationSubject = value; }
        }

        public string TaskCompleteMessage
        {
            get { return taskCompleteMessage; }
            set { taskCompleteMessage = value; }
        }

        public string StatusQueuedMessage
        {
            get { return statusQueuedMessage; }
            set { statusQueuedMessage = value; }
        }

        public string StatusStartedMessage
        {
            get { return statusStartedMessage; }
            set { statusStartedMessage = value; }
        }

        public string StatusRunningMessage
        {
            get { return statusRunningMessage; }
            set { statusRunningMessage = value; }
        }

        public string StatusCompleteMessage
        {
            get { return statusCompleteMessage; }
            set { statusCompleteMessage = value; }
        }


        /// <summary>
        /// The frequency in second at which task status updates are expected.
        /// If no update to taskqueue status for 3x this value the taks is considered stalled.
        /// </summary>
        public int UpdateFrequency
        {
            get { return updateFrequency; }

        }

        public bool CanStop
        {
            get { return canStop; }

        }

        public bool CanResume
        {
            get { return canResume; }

        }

        #endregion

        public void QueueTask()
        {
            if (this.siteGuid == Guid.Empty) return;

            // don't queue a task that has already been created
            if (this.taskGuid != Guid.Empty) return;

            TaskQueue task = new TaskQueue();
            task.SiteGuid = this.siteGuid;
            task.QueuedBy = this.queuedBy;
            task.TaskName = this.taskName;
            task.NotifyOnCompletion = this.notifyOnCompletion;
            task.NotificationToEmail = this.notificationToEmail;
            task.NotificationFromEmail = this.notificationFromEmail;
            task.NotificationSubject = this.notificationSubject;
            task.TaskCompleteMessage = this.taskCompleteMessage;
            task.CanResume = this.canResume;
            task.CanStop = this.canStop;
            task.UpdateFrequency = this.updateFrequency;
            task.Status = statusQueuedMessage;
            task.LastStatusUpdateUTC = DateTime.UtcNow;
            this.taskGuid = task.NewGuid;
            task.SerializedTaskObject = SerializationHelper.SerializeToString(this);
            task.SerializedTaskType = this.GetType().AssemblyQualifiedName;
            task.Save();


        }

        public void StartTask()
        {
            if (this.taskGuid == Guid.Empty) return;

            TaskQueue task = new TaskQueue(this.taskGuid);

            if (task.Guid == Guid.Empty) return; // task not found

            if (!ThreadPool.QueueUserWorkItem(new WaitCallback(RunTaskOnNewThread), this))
            {
                throw new Exception("Couldn't queue the EmailMessageTask on a new thread.");
            }

            task.Status = statusStartedMessage;
            task.StartUTC = DateTime.UtcNow;
            task.LastStatusUpdateUTC = DateTime.UtcNow;
            task.Save();

            log.Info("Queued EmailMessageTask on a new thread");


        }

        public void StopTask()
        {
            throw new System.NotImplementedException("This feature is not implemented");

        }

        public void ResumeTask()
        {
            StartTask();

        }

        #endregion

        private static void RunTaskOnNewThread(object oTask)
        {
            if (oTask == null) return;
            EmailMessageTask task = oTask as EmailMessageTask;

            log.Info("deserialized EmailMessageTask task");

            // give a little time to make sure the taskqueue was updated after spawning the thread
            Thread.Sleep(100); // 0.10 seconds

            task.RunTask();

            log.Info("started EmailMessageTask task");

        }

        #region Task Specific Properties

        #region Private Task Specific Properties

        private string subject = string.Empty;
        private string textBody = string.Empty;
        private string htmlBody = string.Empty;
        private bool useHtml = false;
        private string emailFrom = string.Empty;
        private string emailFromAlias = string.Empty;
        private string emailTo = string.Empty;
        private string emailReplyTo = string.Empty;
        private string emailCc = string.Empty;
        private string emailBcc = string.Empty;
        private string emailPriority = "Normal";

        private string user = string.Empty;
        private string password = string.Empty;
        private string server = string.Empty;
        private int port = 25;
        private bool requiresAuthentication = false;
        private bool useSsl = false;
        private string preferredEncoding = string.Empty;

        #endregion


        #region Public Task Specific Properties

        [XmlIgnore]
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        
        // This is needed to support xml serialization, string with special characterscan cause invalid xml, base 64 encoding them gets around the problem.
        
        [XmlElement(ElementName = "subject", DataType = "base64Binary")]
        public byte[] SubjectSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(Subject);
            }
            set
            {
                Subject = System.Text.Encoding.Unicode.GetString(value);
            }
        }  

        [XmlIgnore]
        public string TextBody
        {
            get { return textBody; }
            set { textBody = value; }
        }

        [XmlElement(ElementName = "textBody", DataType = "base64Binary")]
        public byte[] TextBodySerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(TextBody);
            }
            set
            {
                TextBody = System.Text.Encoding.Unicode.GetString(value);
            }
        }

        public bool UseHtml
        {
            get { return useHtml; }
            set { useHtml = value; }
        }

        [XmlIgnore]
        public string HtmlBody
        {
            get { return htmlBody; }
            set { htmlBody = value; }
        }

        [XmlElement(ElementName = "htmlBody", DataType = "base64Binary")]
        public byte[] HtmlBodySerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(HtmlBody);
            }
            set
            {
                HtmlBody = System.Text.Encoding.Unicode.GetString(value);
            }
        }  

       
        
        public string EmailFrom
        {
            get { return emailFrom; }
            set { emailFrom = value; }
        }

        

        [XmlIgnore]
        public string EmailFromAlias
        {
            get { return emailFromAlias; }
            set { emailFromAlias = value; }
        }

        [XmlElement(ElementName = "emailFromAlias", DataType = "base64Binary")]
        public byte[] EmailFromSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(EmailFromAlias);
            }
            set
            {
                EmailFromAlias = System.Text.Encoding.Unicode.GetString(value);
            }
        }

        public string EmailReplyTo
        {
            get { return emailReplyTo; }
            set { emailReplyTo = value; }
        }

        public string EmailTo
        {
            get { return emailTo; }
            set { emailTo = value; }
        }

        public string EmailCc
        {
            get { return emailCc; }
            set { emailCc = value; }
        }

        public string EmailBcc
        {
            get { return emailBcc; }
            set { emailBcc = value; }
        }

        public string EmailPriority
        {
            get { return emailPriority; }
            set { emailPriority = value; }
        }

        public string User
        {
            get { return user; }
            set { user = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Server
        {
            get { return server; }
            set { server = value; }
        }

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        public bool RequiresAuthentication
        {
            get { return requiresAuthentication; }
            set { requiresAuthentication = value; }
        }

        public bool UseSsl
        {
            get { return useSsl; }
            set { useSsl = value; }
        }

        public string PreferredEncoding
        {
            get { return preferredEncoding; }
            set { preferredEncoding = value; }
        }


        #endregion


        #endregion

        private void RunTask()
        {
            if (IsValid())
            {
                try
                {
                    SmtpSettings smtpSettings = new SmtpSettings();
                    smtpSettings.Password = password;
                    smtpSettings.Port = port;
                    smtpSettings.PreferredEncoding = preferredEncoding;
                    smtpSettings.RequiresAuthentication = requiresAuthentication;
                    smtpSettings.UseSsl = useSsl;
                    smtpSettings.User = user;
                    smtpSettings.Server = server;

                    string messageBody;
                    if (useHtml)
                    {
                        messageBody = htmlBody;
                    }
                    else
                    {
                        messageBody = textBody;
                    }

                    Email.Send(
                        smtpSettings,
                        emailFrom,
                        emailFromAlias,
                        emailReplyTo,
                        emailTo,
                        emailCc,
                        emailBcc,
                        subject,
                        messageBody,
                        useHtml,
                        emailPriority);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }


            ReportStatus();


        }

        private bool IsValid()
        {
            // TODO: validate email format
            if (emailTo.Length == 0) { return false; }
            if (subject.Length == 0) { return false; }
            if ((!useHtml) &&(textBody.Length == 0)) { return false; }
            if ((useHtml) && (htmlBody.Length == 0)) { return false; }


            return true;

        }


        private void ReportStatus()
        {
            TaskQueue task = new TaskQueue(this.taskGuid);

            task.CompleteRatio = 1; //nothing to do so mark as complete
            task.Status = statusCompleteMessage;

            if (task.CompleteUTC == DateTime.MinValue)
                task.CompleteUTC = DateTime.UtcNow;

            task.LastStatusUpdateUTC = DateTime.UtcNow;
            task.Save();

        }


    }
}
