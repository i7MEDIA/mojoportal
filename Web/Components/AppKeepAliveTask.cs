// Author:					
// Created:				    2008-01-02
// Last Modified:			2009-11-29
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
using System.Collections.Generic;
using System.Net;
using System.Threading;
using log4net;
using mojoPortal.Business;

namespace mojoPortal.Web
{
    /// <summary>
    /// The purpose of this task is to keep the web application alive so that other running tasks
    /// can complete. If the site is not getting much traffic tha application can stop and this will
    /// cause any running tasks to stop. Typically if no pages are requested for 20 minutes the application will stop.
    /// This task will keep running as long as there are unfinished tasks
    /// and it will make periodic web page requests to keep the app alive.
    /// This task cannnot prevent the application from stopping if it is recycled either manually or by schedule.
    /// As long as tasks are implented in such a way that they can resume, then we should be able to restart
    /// incomplete tasks in the Application_Start event in global.asax.cs
    /// 
    /// </summary>
    [Serializable()]
    public class AppKeepAliveTask : ITaskQueueTask
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AppKeepAliveTask));

        #region ITaskQueueTask

        private Guid siteGuid = Guid.Empty;
        private Guid taskGuid = Guid.Empty;
        private Guid queuedBy = Guid.Empty;
        private string taskName = "App Keep Alive Task";
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
            //if (this.siteGuid == Guid.Empty) return;

            if (this.taskGuid != Guid.Empty) return;

            if (this.urlToRequest.Length == 0) return;

            TaskQueue task = new TaskQueue();
            task.SiteGuid = this.siteGuid;
            task.TaskName = this.taskName;
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
                throw new Exception("Couldn't queue the AppKeepAliveTask task on a new thread.");
            }

            task.Status = "Started";
            task.StartUTC = DateTime.UtcNow;
            task.LastStatusUpdateUTC = DateTime.UtcNow;
            task.Save();

            log.Info("Queued AppKeepAliveTask on a new thread");




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
        private string urlToRequest = string.Empty;
        private int webErrorCount = 0;
        private int maxAllowedWebErrors = 10;


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

        public string UrlToRequest
        {
            get { return urlToRequest; }
            set { urlToRequest = value; }
        }

        public int CountOfIterations
        {
            get { return countOfIterations; }

        }

        private static void RunTaskOnNewThread(object threadSleepTask)
        {
            if (threadSleepTask == null) return;
            AppKeepAliveTask task = (AppKeepAliveTask)threadSleepTask;

            log.Info("deserialized AppKeepAliveTask task");

            // give a little time to make sure the taskqueue was updated after spawning the thread
            Thread.Sleep(10000); // 10 seconds

            task.RunTask();

            log.Info("started AppKeepAliveTask task");

        }


        private void RunTask()
        {
            if (IsAlreadyRunning())
            {
                MarkAsComplete();
                return;
            }

            startTime = DateTime.UtcNow;
            endTime = startTime.AddMinutes(maxRunTimeMinutes);
            timeToRun = endTime.Subtract(startTime);


            while ((DateTime.UtcNow < endTime)&&(webErrorCount < maxAllowedWebErrors))
            {
                countOfIterations += 1;
                DoKeepAlive();
                DoSleeping();
            }

            TaskQueue task = new TaskQueue(this.taskGuid);
            task.Status = "Finished";
            task.CompleteRatio = 1;
            task.LastStatusUpdateUTC = DateTime.UtcNow;
            task.CompleteUTC = DateTime.UtcNow;
            task.Save();

        }

        private bool IsAlreadyRunning()
        {
            List<TaskQueue> unfinishedTasks = TaskQueue.GetUnfinished();

            string thisType = typeof(AppKeepAliveTask).AssemblyQualifiedName;

            Type taskType = Type.GetType(thisType);

            foreach (TaskQueue task in unfinishedTasks)
            {
                Type t = Type.GetType(task.SerializedTaskType);
                if (t == null)
                {
                    task.CompleteRatio = 1;
                    task.CompleteUTC = DateTime.UtcNow;
                    task.Status = this.statusCompleteMessage;
                    task.Save();

                }
                else
                {

                    if (
                        (t.FullName == taskType.FullName)
                        && (task.Guid != this.taskGuid)
                        )
                    {
                        if (TaskQueue.IsStalled(task))
                        {
                            task.CompleteRatio = 1;
                            task.CompleteUTC = DateTime.UtcNow;
                            task.Status = this.statusCompleteMessage;
                            task.Save();
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void MarkAsComplete()
        {
            TaskQueue task = new TaskQueue(this.taskGuid);
            task.Status = statusCompleteMessage;
            task.CompleteRatio = 1;
            task.LastStatusUpdateUTC = DateTime.UtcNow;
            task.CompleteUTC = DateTime.UtcNow;
            task.Save();


        }

        private void DoKeepAlive()
        {
            if (this.taskGuid == Guid.Empty) return;

            TaskQueue task = new TaskQueue(this.taskGuid);
            task.Status = "Running";

            task.LastStatusUpdateUTC = DateTime.UtcNow;
            task.Save();

            if (urlToRequest.Length == 0)
            {
                log.Info("No url provided for app keep alive task so task is quiting");
                //make the task end
                endTime = DateTime.UtcNow;
                return;
            }

            //make web request to keep the app alive
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlToRequest);
                webRequest.Method = "GET";
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            }
            catch (UriFormatException ex)
            {
                webErrorCount = maxAllowedWebErrors; // this particular error means the ulr is bad so no need to try again
                log.Error(ex);

            }
            catch (NotSupportedException ex)
            {
                webErrorCount = maxAllowedWebErrors; // no need to try again
                log.Error(ex);

            }
            catch (System.Security.SecurityException ex)
            {
                webErrorCount = maxAllowedWebErrors; // no need to try again
                log.Error(ex);

            }
            catch (WebException ex)
            {
                webErrorCount += 1;
                log.Error(ex);

            }
            catch (InvalidOperationException ex)
            {
                webErrorCount = maxAllowedWebErrors; // no need to try again
                log.Error(ex);

            }
            






        }

        private void DoSleeping()
        {

            //do nothing until next check time
            Thread.Sleep(minutesToSleep * millisecondsPerMinute);

        }

    }
}
