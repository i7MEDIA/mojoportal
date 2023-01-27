// Author:					
// Created:				    2008-01-06
// Last Modified:			2009-05-30
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
using System.Data.Common;
using System.Threading;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web
{
    /// <summary>
    ///
    /// </summary>
    [Serializable()]
    public class WebTaskManager : ITaskQueueTask
    {

        #region ITaskQueueTask

        private Guid taskGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid queuedBy = Guid.Empty;
        private string taskName = "WebTaskManager";
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
            set { }
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
            
            if (this.taskGuid != Guid.Empty) return;

            TaskQueue task = new TaskQueue();
            task.SiteGuid = SiteSettings.GetRootSiteGuid();

            if (task.SiteGuid == Guid.Empty) return;

            task.QueuedBy = this.queuedBy;
            task.SerializedTaskType = this.GetType().AssemblyQualifiedName;
            task.TaskName = taskName;
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

            task.Status = statusRunningMessage;
            task.StartUTC = DateTime.UtcNow;
            task.LastStatusUpdateUTC = DateTime.UtcNow;
            task.Save();

            log.Info("Queued WebTaskManager on a new thread");


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






        private static readonly ILog log = LogManager.GetLogger(typeof(WebTaskManager));
        private int millisecondsPerSecond = 1000; 
        //private int minutesToSleep = 1;
        private List<TaskQueue> tasksNotStarted;
        private List<TaskQueue> unfinshedTasks;


        private static void RunTaskOnNewThread(object oTask)
        {
            if (oTask == null) return;
            WebTaskManager task = (WebTaskManager)oTask;

            log.Info("deserialized WebTaskManager task");

            // give a little time to make sure the taskqueue was updated after spawning the thread
            Thread.Sleep(100); // 0.10 seconds

            task.RunTask();

            log.Info("started WebTaskManager task");

        }

        private void RunTask()
        {
            unfinshedTasks = TaskQueue.GetUnfinished();
            tasksNotStarted = TaskQueue.GetTasksNotStarted();

           
            while ((unfinshedTasks.Count > 0) || (tasksNotStarted.Count > 0))
            {
                ResumeOrKillStalledTasks();
                StartNewTasks();
                DoReporting();
                DoSleeping();
                unfinshedTasks = TaskQueue.GetUnfinished();
                tasksNotStarted = TaskQueue.GetTasksNotStarted();

                if ((tasksNotStarted.Count == 0) && (unfinshedTasks.Count == 1))
                {
                    if(unfinshedTasks[0].SerializedTaskType == this.GetType().AssemblyQualifiedName)
                    break; 
                }

              

            }

            TaskQueue task = new TaskQueue(this.taskGuid);
            task.Status = statusCompleteMessage;
            task.CompleteRatio = 1;
            task.LastStatusUpdateUTC = DateTime.UtcNow;
            task.CompleteUTC = DateTime.UtcNow;
            task.Save();

        }

        private void ResumeOrKillStalledTasks()
        {
            List<TaskQueue> unfinshedTasks = TaskQueue.GetUnfinished();

            foreach (TaskQueue task in unfinshedTasks)
            {
                
                if (TaskQueue.IsStalled(task))
                {
                    if (task.CanResume)
                    {
                        Type t = Type.GetType(task.SerializedTaskType);

                        if (t == null)
                        {
                            log.Error("Failed to deserialize and resume task " + task.TaskName);
                            task.Status = "Stalled";
                            task.CompleteUTC = DateTime.UtcNow;
                            task.Save();


                        }
                        else
                        {
                            try
                            {
                                ITaskQueueTask qTask
                                    = (ITaskQueueTask)SerializationHelper.DeserializeFromString(t, task.SerializedTaskObject);
                                if (qTask != null)
                                {
                                    qTask.ResumeTask();
                                }
                                else
                                {
                                    log.Error("Failed to deserialize and resume task " + task.TaskName);

                                }
                            }
                            catch (InvalidOperationException ex)
                            {
                                log.Error("Failed to deserialize and resume task " + task.TaskName + " so cancelling", ex);
                                task.CompleteRatio = 1;
                                task.CompleteUTC = DateTime.UtcNow;
                                task.Save();
                                log.Info("Failed Serialized Task was: " + task.SerializedTaskObject);
                            }
                        }

                    }
                    else
                    {
                        
                        // TODO: localize
                        task.Status = "Stalled";
                        task.CompleteUTC = DateTime.UtcNow;
                        task.Save();
                    }

                }
            }

        }


        private void StartNewTasks()
        {
            List<TaskQueue> tasksNotStarted = TaskQueue.GetTasksNotStarted();
            foreach (TaskQueue task in tasksNotStarted)
            {
                if ((task.SerializedTaskType.Length == 0) || (task.SerializedTaskObject.Length == 0))
                {
                    // is a task somehow gets queued without a task type or serialized object
                    // just set it as completed
                    task.CompleteRatio = 1;
                    task.CompleteUTC = DateTime.UtcNow;
                    task.Save();
                    continue;
                }

                Type t = null;

                try
                {
                    t = Type.GetType(task.SerializedTaskType);

                }
                catch (ArgumentException) { }
                catch (TypeLoadException) { }
                
                
                if (t == null)
                {
                    log.Error("Failed to deserialize and resume task " + task.TaskName);
                    task.CompleteRatio = 1;
                    task.CompleteUTC = DateTime.UtcNow;
                    task.Save();
                    continue;

                }

                try
                {
                    ITaskQueueTask qTask
                        = (ITaskQueueTask)SerializationHelper.DeserializeFromString(Type.GetType(task.SerializedTaskType), task.SerializedTaskObject);
                    if (qTask != null)
                    {
                        qTask.StartTask();
                    }
                    else
                    {
                        log.Error("Failed to deserialize and start task " + task.TaskName);

                    }
                }
                catch (InvalidOperationException ex)
                {
                    log.Error("Failed to deserialize and start task " + task.TaskName + " so cancelling task", ex);
                    task.CompleteRatio = 1;
                    task.CompleteUTC = DateTime.UtcNow;
                    task.Save();
                    log.Info("Failed Serialized Task was: " + task.SerializedTaskObject);
                }
            }

        }

        private void DoReporting()
        {
            if (this.taskGuid == Guid.Empty) return;

            TaskQueue task = new(this.taskGuid)
            {
                Status = statusRunningMessage,
                CompleteRatio = 0.5,
                LastStatusUpdateUTC = DateTime.UtcNow
            };
            task.Save();

        }

        private void DoSleeping()
        {
            Thread.Sleep(updateFrequency * millisecondsPerSecond);
        }



        public static void StartOrResumeTasks()
        {
            if (WebConfigSettings.DisableTaskQueue) { return; }

            bool appWasRestarted = false;
            StartOrResumeTasks(appWasRestarted);
            
        }

        public static void StartOrResumeTasks(bool appWasRestarted)
        {
            if (WebConfigSettings.DisableTaskQueue) { return; }

            
            List<TaskQueue> unfinishedTasks;
            SiteSettings siteSettings = null;

            try
            {
                TaskQueue.DeleteCompleted();

                // the default is false and it may be problematic to try and use this as true
                // because the app start event only fires 1 time not 1 time per site
                if (WebConfigSettings.UsePerSiteTaskQueue)
                {
                    // this also doesn't work in IIS 7 integrated pipeline mode because HttpContext is null
                    siteSettings = CacheHelper.GetCurrentSiteSettings();
                }

                if ((WebConfigSettings.UsePerSiteTaskQueue) && (siteSettings != null))
                {
                    unfinishedTasks = TaskQueue.GetUnfinished(siteSettings.SiteGuid);

                }
                else
                {
                    unfinishedTasks = TaskQueue.GetUnfinished();
                }
                if (WebTaskManagerIsRunning(unfinishedTasks, appWasRestarted)) return;
                if ((appWasRestarted) && (unfinishedTasks.Count == 0)) return;

                WebTaskManager taskManager = new WebTaskManager();
                taskManager.QueueTask();
                taskManager.StartTask();
            }
            catch (DbException ex)
            {
                log.Error(ex);
            }
            catch (InvalidOperationException ex)
            {
                log.Error(ex);
            }
            catch (Exception ex)
            {
                // hate to trap System.Exception but SqlCeException doe snot inherit from DbException as it should
                if (DatabaseHelper.DBPlatform() != "SqlCe") { throw; }
                log.Error(ex);
            }

        }


        private static bool WebTaskManagerIsRunning(List<TaskQueue> unfinishedTasks, bool appWasRestarted)
        {
            string thisType = typeof(WebTaskManager).AssemblyQualifiedName;
            Type webTaskType = Type.GetType(thisType);
            
            foreach (TaskQueue task in unfinishedTasks)
            {
                Type t = Type.GetType(task.SerializedTaskType);

                if ((t == null) || (t.FullName == webTaskType.FullName))
                {
                    if ((appWasRestarted) || (TaskQueue.IsStalled(task)))
                    {
                        task.CompleteRatio = 1;
                        task.CompleteUTC = DateTime.UtcNow;
                        task.Save();
                        
                    }
                    else
                    {
                        return true;
                    }

                }

            }


            return false;

        }
    }
}
