﻿///							DBTaskQueue.cs
/// Author:					
/// Created:				2007-12-30
/// Last Modified:			2012-07-20
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace mojoPortal.Data
{
    public static class DBTaskQueue
    {
        
        /// <summary>
        /// Inserts a row in the mp_TaskQueue table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="queuedBy"> queuedBy </param>
        /// <param name="taskName"> taskName </param>
        /// <param name="notifyOnCompletion"> notifyOnCompletion </param>
        /// <param name="notificationToEmail"> notificationToEmail </param>
        /// <param name="notificationFromEmail"> notificationFromEmail </param>
        /// <param name="notificationSubject"> notificationSubject </param>
        /// <param name="taskCompleteMessage"> taskCompleteMessage </param>
        /// <param name="canStop"> canStop </param>
        /// <param name="canResume"> canResume </param>
        /// <param name="updateFrequency"> updateFrequency </param>
        /// <param name="queuedUTC"> queuedUTC </param>
        /// <param name="completeRatio"> completeRatio </param>
        /// <param name="status"> status </param>
        /// <param name="serializedTaskObject"> serializedTaskObject </param>
        /// <param name="serializedTaskType"> serializedTaskType </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid queuedBy,
            string taskName,
            bool notifyOnCompletion,
            string notificationToEmail,
            string notificationFromEmail,
            string notificationSubject,
            string taskCompleteMessage,
            bool canStop,
            bool canResume,
            int updateFrequency,
            DateTime queuedUTC,
            double completeRatio,
            string status,
            string serializedTaskObject,
            string serializedTaskType)
        {
            
            #region Bit Conversion

            int intNotifyOnCompletion;
            if (notifyOnCompletion)
            {
                intNotifyOnCompletion = 1;
            }
            else
            {
                intNotifyOnCompletion = 0;
            }

            int intCanStop;
            if (canStop)
            {
                intCanStop = 1;
            }
            else
            {
                intCanStop = 0;
            }

            int intCanResume;
            if (canResume)
            {
                intCanResume = 1;
            }
            else
            {
                intCanResume = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_TaskQueue (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("QueuedBy, ");
            sqlCommand.Append("TaskName, ");
            sqlCommand.Append("NotifyOnCompletion, ");
            sqlCommand.Append("NotificationToEmail, ");
            sqlCommand.Append("NotificationFromEmail, ");
            sqlCommand.Append("NotificationSubject, ");
            sqlCommand.Append("TaskCompleteMessage, ");
            sqlCommand.Append("CanStop, ");
            sqlCommand.Append("CanResume, ");
            sqlCommand.Append("UpdateFrequency, ");
            sqlCommand.Append("QueuedUTC, ");
            sqlCommand.Append("CompleteRatio, ");
            sqlCommand.Append("Status, ");
            sqlCommand.Append("SerializedTaskObject, ");
            sqlCommand.Append("SerializedTaskType )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?QueuedBy, ");
            sqlCommand.Append("?TaskName, ");
            sqlCommand.Append("?NotifyOnCompletion, ");
            sqlCommand.Append("?NotificationToEmail, ");
            sqlCommand.Append("?NotificationFromEmail, ");
            sqlCommand.Append("?NotificationSubject, ");
            sqlCommand.Append("?TaskCompleteMessage, ");
            sqlCommand.Append("?CanStop, ");
            sqlCommand.Append("?CanResume, ");
            sqlCommand.Append("?UpdateFrequency, ");
            sqlCommand.Append("?QueuedUTC, ");
            sqlCommand.Append("?CompleteRatio, ");
            sqlCommand.Append("?Status, ");
            sqlCommand.Append("?SerializedTaskObject, ");
            sqlCommand.Append("?SerializedTaskType )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[17];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?QueuedBy", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = queuedBy.ToString();

            arParams[3] = new MySqlParameter("?TaskName", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = taskName;

            arParams[4] = new MySqlParameter("?NotifyOnCompletion", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = intNotifyOnCompletion;

            arParams[5] = new MySqlParameter("?NotificationToEmail", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = notificationToEmail;

            arParams[6] = new MySqlParameter("?NotificationFromEmail", MySqlDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = notificationFromEmail;

            arParams[7] = new MySqlParameter("?NotificationSubject", MySqlDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = notificationSubject;

            arParams[8] = new MySqlParameter("?TaskCompleteMessage", MySqlDbType.Blob);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = taskCompleteMessage;

            arParams[9] = new MySqlParameter("?CanStop", MySqlDbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = intCanStop;

            arParams[10] = new MySqlParameter("?CanResume", MySqlDbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = intCanResume;

            arParams[11] = new MySqlParameter("?UpdateFrequency", MySqlDbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = updateFrequency;

            arParams[12] = new MySqlParameter("?QueuedUTC", MySqlDbType.DateTime);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = queuedUTC;

            arParams[13] = new MySqlParameter("?CompleteRatio", MySqlDbType.Float);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = completeRatio;

            arParams[14] = new MySqlParameter("?Status", MySqlDbType.VarChar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = status;

            arParams[15] = new MySqlParameter("?SerializedTaskObject", MySqlDbType.LongText);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = serializedTaskObject;

            arParams[16] = new MySqlParameter("?SerializedTaskType", MySqlDbType.VarChar, 255);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = serializedTaskType;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_TaskQueue table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="startUTC"> startUTC </param>
        /// <param name="completeUTC"> completeUTC </param>
        /// <param name="lastStatusUpdateUTC"> lastStatusUpdateUTC </param>
        /// <param name="completeRatio"> completeRatio </param>
        /// <param name="status"> status </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            DateTime startUTC,
            DateTime completeUTC,
            DateTime lastStatusUpdateUTC,
            double completeRatio,
            string status)
        {
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_TaskQueue ");
            sqlCommand.Append("SET  ");

            if (startUTC > DateTime.MinValue)
            sqlCommand.Append("StartUTC = ?StartUTC, ");

            if (completeUTC > DateTime.MinValue)
            sqlCommand.Append("CompleteUTC = ?CompleteUTC, ");

            sqlCommand.Append("LastStatusUpdateUTC = ?LastStatusUpdateUTC, ");
            sqlCommand.Append("CompleteRatio = ?CompleteRatio, ");
            sqlCommand.Append("Status = ?Status ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?StartUTC", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = startUTC;

            arParams[2] = new MySqlParameter("?CompleteUTC", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = completeUTC;

            arParams[3] = new MySqlParameter("?LastStatusUpdateUTC", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = lastStatusUpdateUTC;

            arParams[4] = new MySqlParameter("?CompleteRatio", MySqlDbType.Float);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = completeRatio;

            arParams[5] = new MySqlParameter("?Status", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = status;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Updates a row in the mp_TaskQueue table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="notificationSentUTC"> notificationSentUTC </param>
        /// <returns>bool</returns>
        public static bool UpdateNotification(
            Guid guid,
            DateTime notificationSentUTC)
        {
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_TaskQueue ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("NotificationSentUTC = ?NotificationSentUTC ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?NotificationSentUTC", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = notificationSentUTC;

           
            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_TaskQueue table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes all completed tasks from mp_TaskQueue table
        /// </summary>
        public static void DeleteCompleted()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE CompleteUTC IS NOT NULL; ");

            MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                null);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_TaskQueue table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        public static int GetCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append(";");

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                null));
        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        /// <param name="siteGuid"> guid </param>
        public static int GetCount(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        public static int GetCountUnfinishedByType(string taskType)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SerializedTaskType LIKE ?TaskType ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?TaskType", MySqlDbType.VarChar, 266);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = taskType + "%";

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        public static bool DeleteByType(string taskType)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SerializedTaskType LIKE ?TaskType ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?TaskType", MySqlDbType.VarChar, 266);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = taskType + "%";

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        public static int GetCountUnfinished()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CompleteUTC IS NULL ");
            sqlCommand.Append(";");

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                null));
        }

        /// <summary>
        /// Gets a count of rows in the mp_TaskQueue table.
        /// </summary>
        /// <param name="siteGuid"> guid </param>
        public static int GetCountUnfinished(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND CompleteUTC IS NULL ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_TaskQueue table.
        /// </summary>
        public static IDataReader GetTasksNotStarted()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StartUTC IS NULL ");
            sqlCommand.Append(";");

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                null);
        }

        /// <summary>
        /// Gets an IDataReader with all tasks in the mp_TaskQueue table that have completed but not yet sent notification.
        /// </summary>
        public static IDataReader GetTasksForNotification()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("NotifyOnCompletion = 1 ");
            sqlCommand.Append("AND CompleteUTC IS NOT NULL ");
            sqlCommand.Append("AND NotificationSentUTC IS NULL ");
            sqlCommand.Append(";");

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                null);
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_TaskQueue table.
        /// </summary>
        public static IDataReader GetUnfinished()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CompleteUTC IS NULL ");
            sqlCommand.Append(";");

            return Task.Run(async () => await MySqlHelper.ExecuteReaderAsync(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                null)).Result;
        }


        /// <summary>
        /// Gets an IDataReader with all rows in the mp_TaskQueue table.
        /// </summary>
        /// <param name="siteGuid"> guid </param>
        public static IDataReader GetUnfinished(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_TaskQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND CompleteUTC IS NULL ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        

        /// <summary>
        /// Gets a page of data from the mp_TaskQueue table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount();

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_TaskQueue  ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("QueuedUTC DESC  ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", ?PageSize ");
            
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageSize;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets a page of data from the mp_TaskQueue table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        /// <param name="siteGuid"> guid </param>
        public static IDataReader GetPageBySite(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(siteGuid);

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_TaskQueue  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("QueuedUTC DESC  ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", ?PageSize ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }


        /// <summary>
        /// Gets a page of data from the mp_TaskQueue table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPageUnfinished(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountUnfinished();

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_TaskQueue  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CompleteUTC IS NULL ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("QueuedUTC DESC  ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", ?PageSize ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageSize;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }


        /// <summary>
        /// Gets a page of data from the mp_TaskQueue table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        /// <param name="siteGuid"> guid </param>
        public static IDataReader GetPageUnfinishedBySite(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountUnfinished(siteGuid);

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_TaskQueue  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND CompleteUTC IS NULL ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("QueuedUTC DESC  ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", ?PageSize ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }


    }
}
