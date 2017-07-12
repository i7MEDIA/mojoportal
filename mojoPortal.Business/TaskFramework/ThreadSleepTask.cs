// Author:					
// Created:				    2007-12-30
// Last Modified:			2008-01-11
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Threading;
using log4net;

namespace mojoPortal.Business
{
    /// <summary>
    /// This is just a task for testing purposes. 
    /// It can be configured how long to run and how long to sleep in between reporting.
    /// 
    /// </summary>
    [Serializable()]
    public class ThreadSleepTask : ITaskQueueTask
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ThreadSleepTask));

        #region ITaskQueueTask

        private Guid siteGuid = Guid.Empty;
        private Guid taskGuid = Guid.Empty;
        private Guid queuedBy = Guid.Empty;
        private string taskName = "Sleep Task for testing purposes";
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
        private bool canResume = false;
        private int updateFrequency = 5;


        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }

        public Guid TaskGuid
        {
            get { return taskGuid; }
            set { taskGuid = value; }
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
        /// The frequency in minutes at which task status updates are expected.
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

        public void QueueTask()
        {
            if (this.siteGuid == Guid.Empty) return;

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
            task.Status = "Queued";
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
                throw new Exception("Couldn't queue the task on a new thread.");
            }

            task.Status = "Started";
            task.StartUTC = DateTime.UtcNow;
            task.LastStatusUpdateUTC = DateTime.UtcNow;
            task.Save();

            log.Info("Queued ThreadSleepTask on a new thread");

        }

        
        public void StopTask()
        {
            throw new System.NotImplementedException("This feature is not implemented");

        }

        public void ResumeTask()
        {
            throw new System.NotImplementedException("This feature is not implemented");

        }

        

        #endregion

        private DateTime startTime = DateTime.UtcNow;
        private DateTime endTime;
        private int millisecondsPerMinute = 15000; //60000;
        private int minutesToSleep = 1;
        private int maxRunTimeMinutes = 5;
        private int countOfIterations = 0;
        private TimeSpan timeToRun;


        public int MinutesToSleep
        {
            get { return minutesToSleep; }
            set { minutesToSleep = value; }
        }

        public int MaxRunTimeMinutes
        {
            get { return maxRunTimeMinutes; }
            set { maxRunTimeMinutes = value; }
        }

        public int CountOfIterations
        {
            get { return countOfIterations; }
            
        }

        private static void RunTaskOnNewThread(object threadSleepTask)
        {
            if (threadSleepTask == null) return;
            ThreadSleepTask task = (ThreadSleepTask)threadSleepTask;

            log.Info("deserialized ThreadSleep task");

            // give a little time to make sure the taskqueue was updated after spawning the thread
            Thread.Sleep(10000); // 10 seconds

            task.RunTask();

            log.Info("started ThreadSleep task");

        }


        private void RunTask()
        {
            startTime = DateTime.UtcNow;
            endTime = startTime.AddMinutes(maxRunTimeMinutes);
            timeToRun = endTime.Subtract(startTime);
           

            while (DateTime.UtcNow < endTime)
            {
                countOfIterations += 1;
                DoReporting();
                DoSleeping();
            }

            TaskQueue task = new TaskQueue(this.taskGuid);
            task.Status = "Finished";
            task.CompleteRatio = 1;
            task.LastStatusUpdateUTC = DateTime.UtcNow;
            task.CompleteUTC = DateTime.UtcNow;
            task.Save();

        }

        private void DoReporting()
        {
            if (this.taskGuid == Guid.Empty) return;

            

            TaskQueue task = new TaskQueue(this.taskGuid);
            task.Status = "Running " + countOfIterations.ToString(CultureInfo.InvariantCulture);
            if ((timeToRun != null)&&(timeToRun.TotalSeconds > 0))
            {
                TimeSpan timeLeft = endTime.Subtract(DateTime.UtcNow);
                task.CompleteRatio = ((timeToRun.TotalSeconds - timeLeft.TotalSeconds)/timeToRun.TotalSeconds);


            }
            task.LastStatusUpdateUTC = DateTime.UtcNow;
            task.Save();

            log.Info("Task " + task.TaskName 
                + " completed iteration " 
                + countOfIterations.ToString(CultureInfo.InvariantCulture));

            


        }

        private void DoSleeping()
        {

            //do nothing until next check time
            if (minutesToSleep < updateFrequency)
            {
                Thread.Sleep(minutesToSleep * millisecondsPerMinute);
            }
            else
            {
                Thread.Sleep(updateFrequency * millisecondsPerMinute);
            }

        }

    }
}
