// Author:					
// Created:				    2012-07-11
// Last Modified:			2012-07-12
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Threading;
using log4net;
using mojoPortal.Business;
using mojoPortal.Net;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web
{
    [Serializable()]
    public class LetterOptInTask : ITaskQueueTask
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LetterOptInTask));

        private bool excludeIfAnyUnsubscribeHx = true;

        public bool ExcludeIfAnyUnsubscribeHx
        {
            get { return excludeIfAnyUnsubscribeHx; }
            set { excludeIfAnyUnsubscribeHx = value; }
        }

        private Guid letterInfoGuid = Guid.Empty;

        public Guid LetterInfoGuid
        {
            get { return letterInfoGuid; }
            set { letterInfoGuid = value; }
        }






        #region ITaskQueueTask

        private Guid taskGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid queuedBy = Guid.Empty;
        private string taskName = "Newsletter Opt In Task";
        private bool notifyOnCompletion = false;
        private string notificationToEmail = String.Empty;
        private string notificationFromEmail = String.Empty;
        private string notificationSubject = String.Empty;
        private string taskCompleteMessage = string.Empty;
        private string statusQueuedMessage = "Queued";
        private string statusStartedMessage = "Started";
        private string statusRunningMessage = "Running";
        private string statusCompleteMessage = "Complete";
        private bool canStop = false;
        private bool canResume = true;
        // report status every 15 seconds by default
        private int updateFrequency = 65; //update frequency is also used to tell if a task is stalled

        //in this task we report more frequently if possible but must be able to sleep to throttle messages
        //private int reportingFrequency = 15;


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
            set
            {
                //foo = value; //don't localize it can cause problems but need to keep th setter for backward compat
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

            if (this.taskGuid != Guid.Empty) return;

            if (this.letterInfoGuid == Guid.Empty) return;


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

            this.taskGuid = task.Guid;


        }

        public void StartTask()
        {
            if (this.taskGuid == Guid.Empty) return;

            TaskQueue task = new TaskQueue(this.taskGuid);

            if (task.Guid == Guid.Empty) return; // task not found

            if (!ThreadPool.QueueUserWorkItem(new WaitCallback(RunTaskOnNewThread), this))
            {
                throw new Exception("Couldn't queue the task on a new thread.");
            }

            task.Status = statusStartedMessage;
            task.StartUTC = DateTime.UtcNow;
            task.LastStatusUpdateUTC = DateTime.UtcNow;
            task.Save();

            log.Info("Queued LetterOptInTask on a new thread");


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
            LetterOptInTask task = (LetterOptInTask)oTask;

            log.Info("deserialized LetterSendTask task");

            // give a little time to make sure the taskqueue was updated after spawning the thread
            Thread.Sleep(10000); // 10 seconds

            task.RunTask();

            log.Info("started LetterOptInTask task");

        }

        private DataTable GetData()
        {
            return SubscriberRepository.GetTop1000UsersNotSubscribed(siteGuid, letterInfoGuid, excludeIfAnyUnsubscribeHx);
        }

        private void RunTask()
        {
            
            // this is where the work gets done

            // Get a data table of up to 1000 users
            // who have not opted in but also have not previously
            // opted out and have valid email accounts and account is not locked
            DataTable dataTable = GetData();

            double completeRatio = .5;

            if (dataTable.Rows.Count == 0)
            {
                completeRatio = 1; //complete
                ReportStatus(completeRatio);
                return;

            }

            SubscriberRepository repository = new SubscriberRepository();

            int count = 0;

            foreach (DataRow row in dataTable.Rows)
            {
                Guid userGuid = new Guid(row["UserGuid"].ToString());
                string email = row["email"].ToString();

                LetterSubscriber s = new LetterSubscriber();
                s.SiteGuid = SiteGuid;
                s.UserGuid = userGuid;
                s.EmailAddress = email;
                s.LetterInfoGuid = LetterInfoGuid;
                s.UseHtml = true;
                s.IsVerified = true;

                repository.Save(s);

                count += 1;

                if (count > 20)
                {
                    ReportStatus(completeRatio);
                    count = 0;
                }

            }

            LetterInfo.UpdateSubscriberCount(LetterInfoGuid);
            ReportStatus(completeRatio);

            // repeat until the table comes back with 0 rows
            RunTask();

        }

        private void ReportStatus(double completeRatio)
        {

           TaskQueue task = new TaskQueue(this.taskGuid);
           task.CompleteRatio = completeRatio;


           if (task.CompleteRatio >= 1)
           {
               task.Status = statusCompleteMessage;

               if (task.CompleteUTC == DateTime.MinValue)
               {
                   task.CompleteUTC = DateTime.UtcNow;
               } 

           }
           else
           {
               task.Status = this.statusRunningMessage; 
           }


           task.LastStatusUpdateUTC = DateTime.UtcNow;
           task.Save();

           //nextStatusUpdateTime = DateTime.UtcNow.AddSeconds(updateFrequency);


        }


    }
}