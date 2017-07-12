// Author:					
// Created:				    2009-10-13
// Last Modified:			2009-10-13
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
using System.IO;
using System.Threading;
using log4net;
using mojoPortal.Business;
using mojoPortal.Net;

namespace mojoPortal.Web
{
    /// <summary>
    /// This is a simple task for deleting a folder asynchronously on a background thread 
    /// 
    /// </summary>
    [Serializable()]
    public class FolderDeleteTask : ITaskQueueTask
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FolderDeleteTask));

        public FolderDeleteTask()
        { }

        #region ITaskQueueTask

        private Guid taskGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid queuedBy = Guid.Empty;
        private string taskName = "Folder Delete Task";
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
                throw new Exception("Couldn't queue the FolderDeleteTask on a new thread.");
            }

            task.Status = statusStartedMessage;
            task.StartUTC = DateTime.UtcNow;
            task.LastStatusUpdateUTC = DateTime.UtcNow;
            task.Save();

            log.Info("Queued Folder Delete on a new thread");


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

        private string folderToDelete = string.Empty;
        public string FolderToDelete
        {
            get { return folderToDelete; }
            set { folderToDelete = value; }
        }


        private static void RunTaskOnNewThread(object oTask)
        {
            if (oTask == null) return;
            FolderDeleteTask task = oTask as FolderDeleteTask;

            log.Info("deserialized FolderDeleteTask task");

            // give a little time to make sure the taskqueue was updated after spawning the thread
            Thread.Sleep(100); // 0.10 seconds

            task.RunTask();

            log.Info("started FolderDeleteTask task");

        }

        private void RunTask()
        {
            if (Directory.Exists(folderToDelete))
            {
                try
                {
                    Directory.Delete(folderToDelete, true);
                }
                catch (IOException ex)
                {
                    log.Error(ex);
                }
                catch (UnauthorizedAccessException ex)
                {
                    log.Error(ex);
                }
                catch (ArgumentException ex)
                {
                    log.Error(ex);
                }
            }
            else
            {
                log.Info("Folder Delete task existing because the folder to delete does not exist. " + folderToDelete);
            }

            ReportStatus();
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
