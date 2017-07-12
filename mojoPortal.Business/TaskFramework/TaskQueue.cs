// Author:					
// Created:				    2007-12-30
// Last Modified:			2012-03-20
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
using mojoPortal.Data;
using log4net;

namespace mojoPortal.Business
{
    /// <summary>
    /// Represents a queue for tasks run on a bcakcground thread
    /// </summary>
    public class TaskQueue
    {

        #region Constructors

        public TaskQueue()
        { }


        public TaskQueue(Guid guid)
        {
            GetTaskQueue(guid);
        }

        #endregion

        #region Private Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(TaskQueue));

        private Guid newGuid = Guid.NewGuid();
        private Guid guid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid queuedBy = Guid.Empty;
        private string taskName = string.Empty;

        private bool notifyOnCompletion = false;
        private string notificationToEmail = String.Empty;
        private string notificationFromEmail = String.Empty;
        private string notificationSubject = String.Empty;
        private string taskCompleteMessage = string.Empty;
        private bool canStop = false;
        private bool canResume = false;
        private int updateFrequency = 5;

        private DateTime queuedUTC = DateTime.UtcNow;
        private DateTime startUTC = DateTime.MinValue;
        private DateTime completeUTC = DateTime.MinValue;
        private DateTime lastStatusUpdateUTC = DateTime.UtcNow;
        private DateTime notificationSentUTC = DateTime.MinValue;
        private double completeRatio = 0;
        private string status = string.Empty;
        private string serializedTaskObject = string.Empty;
        private string serializedTaskType = string.Empty;

        
        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            //set { guid = value; }
        }

        public Guid NewGuid
        {
            get { return newGuid; }
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
            set { taskName = value; }
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

        /// <summary>
        /// The frequency in seconds at which task status updates are expected.
        /// If no update to taskqueue status for within this value the task is considered stalled.
        /// </summary>
        public int UpdateFrequency
        {
            get { return updateFrequency; }
            set { updateFrequency = value; }

        }

        public bool CanStop
        {
            get { return canStop; }
            set { canStop = value; }

        }

        public bool CanResume
        {
            get { return canResume; }
            set { canResume = value; }

        }

        public DateTime QueuedUTC
        {
            get { return queuedUTC; }
            set { queuedUTC = value; }
        }

        public DateTime StartUTC
        {
            get { return startUTC; }
            set { startUTC = value; }
        }

        public DateTime CompleteUTC
        {
            get { return completeUTC; }
            set { completeUTC = value; }
        }

        public DateTime LastStatusUpdateUTC
        {
            get { return lastStatusUpdateUTC; }
            set { lastStatusUpdateUTC = value; }
        }

        public DateTime NotificationSentUTC
        {
            get { return notificationSentUTC; }
            set { notificationSentUTC = value; }
        }

        /// <summary>
        /// A value between 0 and 1 indicating percent complete where 1 = 100%, 0.5 = 50%, etc.
        /// </summary>
        public double CompleteRatio
        {
            get { return completeRatio; }
            set { completeRatio = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public string SerializedTaskObject
        {
            get { return serializedTaskObject; }
            set { serializedTaskObject = value; }
        }

        public string SerializedTaskType
        {
            get { return serializedTaskType; }
            set { serializedTaskType = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of TaskQueue.
        /// </summary>
        /// <param name="guid"> guid </param>
        private void GetTaskQueue(Guid guid)
        {
            using (IDataReader reader = DBTaskQueue.GetOne(guid))
            {
                PopulateFromReader(reader);
            }

        }


        private void PopulateFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                this.guid = new Guid(reader["Guid"].ToString());
                this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                this.queuedBy = new Guid(reader["QueuedBy"].ToString());
                this.taskName = reader["TaskName"].ToString();
                this.notifyOnCompletion = Convert.ToBoolean(reader["NotifyOnCompletion"]);
                this.notificationToEmail = reader["NotificationToEmail"].ToString();
                this.notificationFromEmail = reader["NotificationFromEmail"].ToString();
                this.notificationSubject = reader["NotificationSubject"].ToString();
                this.taskCompleteMessage = reader["TaskCompleteMessage"].ToString();

                if (reader["NotificationSentUTC"] != DBNull.Value)
                this.notificationSentUTC = Convert.ToDateTime(reader["NotificationSentUTC"]);

                this.canStop = Convert.ToBoolean(reader["CanStop"]);
                this.canResume = Convert.ToBoolean(reader["CanResume"]);
                this.updateFrequency = Convert.ToInt32(reader["UpdateFrequency"]);
                this.queuedUTC = Convert.ToDateTime(reader["QueuedUTC"]);

                if (reader["StartUTC"] != DBNull.Value)
                this.startUTC = Convert.ToDateTime(reader["StartUTC"]);

                if (reader["CompleteUTC"] != DBNull.Value)
                this.completeUTC = Convert.ToDateTime(reader["CompleteUTC"]);

                if (reader["LastStatusUpdateUTC"] != DBNull.Value)
                this.lastStatusUpdateUTC = Convert.ToDateTime(reader["LastStatusUpdateUTC"]);
                this.completeRatio = Convert.ToDouble(reader["CompleteRatio"]);
                this.status = reader["Status"].ToString();
                this.serializedTaskObject = reader["SerializedTaskObject"].ToString();
                this.serializedTaskType = reader["SerializedTaskType"].ToString();

            }
            
        }

        

        /// <summary>
        /// Persists a new instance of TaskQueue. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            
            int rowsAffected = DBTaskQueue.Create(
                this.newGuid,
                this.siteGuid,
                this.queuedBy,
                this.taskName,
                this.notifyOnCompletion,
                this.notificationToEmail,
                this.notificationFromEmail,
                this.notificationSubject,
                this.taskCompleteMessage,
                this.canStop,
                this.canResume,
                this.updateFrequency,
                this.queuedUTC,
                this.completeRatio,
                this.status,
                this.serializedTaskObject,
                this.serializedTaskType);

            this.guid = newGuid;

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of TaskQueue. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            if (startUTC == DateTime.MinValue) { startUTC = DateTime.UtcNow; }

            if (lastStatusUpdateUTC == DateTime.MinValue) { lastStatusUpdateUTC = DateTime.UtcNow; }

            return DBTaskQueue.Update(
                this.guid,
                this.startUTC,
                this.completeUTC,
                this.lastStatusUpdateUTC,
                this.completeRatio,
                this.status);

        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of TaskQueue. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.guid != Guid.Empty)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }

        public bool UpdateNotification()
        {
            return DBTaskQueue.UpdateNotification(this.guid, DateTime.UtcNow);
        }




        #endregion

        #region Static Methods

        /// <summary>
        /// Deletes an instance of TaskQueue. Returns true on success.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            return DBTaskQueue.Delete(guid);
        }

        /// <summary>
        /// Deletes all completed tasks 
        /// </summary>
        public static void DeleteCompleted()
        {
            DBTaskQueue.DeleteCompleted();
        }

        
        private static List<TaskQueue> LoadListFromReader(IDataReader reader)
        {
            List<TaskQueue> taskQueueList = new List<TaskQueue>();
            try
            {
                while (reader.Read())
                {
                    TaskQueue taskQueue = new TaskQueue();
                    taskQueue.guid = new Guid(reader["Guid"].ToString());
                    taskQueue.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    taskQueue.queuedBy = new Guid(reader["QueuedBy"].ToString());
                    taskQueue.taskName = reader["TaskName"].ToString();
                    taskQueue.notifyOnCompletion = Convert.ToBoolean(reader["NotifyOnCompletion"]);
                    taskQueue.notificationToEmail = reader["NotificationToEmail"].ToString();
                    taskQueue.notificationFromEmail = reader["NotificationFromEmail"].ToString();
                    taskQueue.notificationSubject = reader["NotificationSubject"].ToString();
                    taskQueue.taskCompleteMessage = reader["TaskCompleteMessage"].ToString();

                    if (reader["NotificationSentUTC"] != DBNull.Value)
                        taskQueue.notificationSentUTC = Convert.ToDateTime(reader["NotificationSentUTC"]);

                    taskQueue.canStop = Convert.ToBoolean(reader["CanStop"]);
                    taskQueue.canResume = Convert.ToBoolean(reader["CanResume"]);
                    taskQueue.updateFrequency = Convert.ToInt32(reader["UpdateFrequency"]);
                    taskQueue.queuedUTC = Convert.ToDateTime(reader["QueuedUTC"]);

                    if (reader["StartUTC"] != DBNull.Value)
                        taskQueue.startUTC = Convert.ToDateTime(reader["StartUTC"]);

                    if (reader["CompleteUTC"] != DBNull.Value)
                        taskQueue.completeUTC = Convert.ToDateTime(reader["CompleteUTC"]);

                    if (reader["LastStatusUpdateUTC"] != DBNull.Value)
                        taskQueue.lastStatusUpdateUTC = Convert.ToDateTime(reader["LastStatusUpdateUTC"]);

                    taskQueue.completeRatio = Convert.ToDouble(reader["CompleteRatio"]);
                    taskQueue.status = reader["Status"].ToString();
                    taskQueue.serializedTaskObject = reader["SerializedTaskObject"].ToString();
                    taskQueue.serializedTaskType = reader["SerializedTaskType"].ToString();

                    taskQueueList.Add(taskQueue);


                }
            }
            finally
            {
                reader.Close();
            }

            return taskQueueList;

        }

        /// <summary>
        /// Gets an IList with all instances of TaskQueue that have not been started yet.
        /// </summary>
        public static List<TaskQueue> GetTasksNotStarted()
        {
            IDataReader reader = DBTaskQueue.GetTasksNotStarted();
            return LoadListFromReader(reader);
        }

        /// <summary>
        /// Gets an IList with all instances of TaskQueue that have completed but not been notified.
        /// </summary>
        public static List<TaskQueue> GetTasksForNotification()
        {
            IDataReader reader = DBTaskQueue.GetTasksForNotification();
            return LoadListFromReader(reader);
        }


        /// <summary>
        /// Gets an IList with all instances of TaskQueue that have not finished running yet.
        /// </summary>
        public static List<TaskQueue> GetUnfinished()
        {
            IDataReader reader = DBTaskQueue.GetUnfinished();
            return LoadListFromReader(reader);
        }

        /// <summary>
        /// Gets an IList with all site specific instances of TaskQueue that have not finished running yet.
        /// </summary>
        public static List<TaskQueue> GetUnfinished(Guid siteGuid)
        {
            IDataReader reader = DBTaskQueue.GetUnfinished(siteGuid);
            return LoadListFromReader(reader);
        }



        /// <summary>
        /// Gets an IList with page of instances of TaskQueue.
        /// </summary>
        public static List<TaskQueue> GetPage(
            int pageNumber, 
            int pageSize, 
            out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBTaskQueue.GetPage(pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);

        }

        /// <summary>
        /// Gets an IList with page of site specific instances of TaskQueue.
        /// </summary>
        public static List<TaskQueue> GetPageBySite(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBTaskQueue.GetPageBySite(siteGuid, pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);

        }


        /// <summary>
        /// Gets an IList with page of instances of TaskQueue.
        /// </summary>
        public static List<TaskQueue> GetPageUnfinished(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBTaskQueue.GetPageUnfinished(pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);

        }


        /// <summary>
        /// Gets an IList with page of site specific instances of TaskQueue.
        /// </summary>
        public static List<TaskQueue> GetPageUnfinishedBySite(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBTaskQueue.GetPageUnfinishedBySite(siteGuid, pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);

        }

        public static bool IsStalled(TaskQueue task)
        {
            // TODO: make config setting
            int taskTimeoutPaddingSeconds = 300; //5 minutes

            return (DateTime.UtcNow > task.LastStatusUpdateUTC.AddSeconds(task.UpdateFrequency + taskTimeoutPaddingSeconds));

        }

        public static bool UnfinishedTaskExists(string taskType)
        {
            if (taskType.Length > 255) { taskType = taskType.Substring(255); }
            return (DBTaskQueue.GetCountUnfinishedByType(taskType) > 0);
        }

        public static bool DeleteByType(string taskType)
        {
            if (taskType.Length > 255) { taskType = taskType.Substring(255); }
            return DBTaskQueue.DeleteByType(taskType);
        }



        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of TaskQueue.
        /// </summary>
        public static int CompareByTaskName(TaskQueue taskQueue1, TaskQueue taskQueue2)
        {
            return taskQueue1.TaskName.CompareTo(taskQueue2.TaskName);
        }
        /// <summary>
        /// Compares 2 instances of TaskQueue.
        /// </summary>
        public static int CompareByQueuedUTC(TaskQueue taskQueue1, TaskQueue taskQueue2)
        {
            return taskQueue1.QueuedUTC.CompareTo(taskQueue2.QueuedUTC);
        }
        /// <summary>
        /// Compares 2 instances of TaskQueue.
        /// </summary>
        public static int CompareByStartUTC(TaskQueue taskQueue1, TaskQueue taskQueue2)
        {
            return taskQueue1.StartUTC.CompareTo(taskQueue2.StartUTC);
        }
        /// <summary>
        /// Compares 2 instances of TaskQueue.
        /// </summary>
        public static int CompareByCompleteUTC(TaskQueue taskQueue1, TaskQueue taskQueue2)
        {
            return taskQueue1.CompleteUTC.CompareTo(taskQueue2.CompleteUTC);
        }
        /// <summary>
        /// Compares 2 instances of TaskQueue.
        /// </summary>
        public static int CompareByLastStatusUpdateUTC(TaskQueue taskQueue1, TaskQueue taskQueue2)
        {
            return taskQueue1.LastStatusUpdateUTC.CompareTo(taskQueue2.LastStatusUpdateUTC);
        }
        /// <summary>
        /// Compares 2 instances of TaskQueue.
        /// </summary>
        public static int CompareByStatus(TaskQueue taskQueue1, TaskQueue taskQueue2)
        {
            return taskQueue1.Status.CompareTo(taskQueue2.Status);
        }
        /// <summary>
        /// Compares 2 instances of TaskQueue.
        /// </summary>
        public static int CompareBySerializedTaskObject(TaskQueue taskQueue1, TaskQueue taskQueue2)
        {
            return taskQueue1.SerializedTaskObject.CompareTo(taskQueue2.SerializedTaskObject);
        }
        /// <summary>
        /// Compares 2 instances of TaskQueue.
        /// </summary>
        public static int CompareBySerializedTaskType(TaskQueue taskQueue1, TaskQueue taskQueue2)
        {
            return taskQueue1.SerializedTaskType.CompareTo(taskQueue2.SerializedTaskType);
        }

        #endregion


    }

}
