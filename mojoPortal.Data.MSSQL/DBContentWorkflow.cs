// Author:					Kevin Needham
// Created:					2009-06-19
// Last Modified:			2013-04-23 
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
using System.Text;
using System.Configuration;
using System.Data;

namespace mojoPortal.Data
{
    public static class DBContentWorkflow
    {
        
        /// <summary>
        /// Inserts a row in the mp_ContentWorkflow table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="createdDateUtc"> createdDateUtc </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="status"> status </param>
        /// <param name="contentText"> contentText </param>
        /// <param name="customData"> customData </param>
        /// <param name="customReferenceNumber"> customReferenceNumber </param>
        /// <param name="customReferenceGuid"> customReferenceGuid </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid moduleGuid,
            Guid userGuid,
            DateTime createdDateUtc,
            string contentText,
            string customData,
            int customReferenceNumber,
            Guid customReferenceGuid,
            string status)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentWorkflow_Insert", 10);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@CreatedDateUtc", SqlDbType.DateTime, ParameterDirection.Input, createdDateUtc);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@Status", SqlDbType.NVarChar, 20, ParameterDirection.Input, status);
            sph.DefineSqlParameter("@ContentText", SqlDbType.NVarChar, -1, ParameterDirection.Input, contentText);
            sph.DefineSqlParameter("@CustomData", SqlDbType.NVarChar, -1, ParameterDirection.Input, customData);

            //object customReferenceNumberVal = customReferenceNumber.HasValue ? (object)customReferenceNumber.Value : DBNull.Value;
            sph.DefineSqlParameter("@CustomReferenceNumber", SqlDbType.Int, ParameterDirection.Input, customReferenceNumber);

            //object customReferenceGuidVal = customReferenceGuid.HasValue ? (object)customReferenceGuid.Value : DBNull.Value;
            sph.DefineSqlParameter("@CustomReferenceGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, customReferenceGuid);

            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;
        }

        /// <summary>
        /// Updates a row in the mp_ContentWorkflow table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="lastModUserGuid"> lastModUserGuid </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="status"> status </param>
        /// <param name="contentText"> contentText </param>
        /// <param name="customData"> customData </param>
        /// <param name="customReferenceNumber"> customReferenceNumber </param>
        /// <param name="customReferenceGuid"> customReferenceGuid </param>
        /// <returns>bool</returns>
        public static int Update(
            Guid guid,
            Guid lastModUserGuid,
            DateTime lastModUtc,
            string contentText,
            string customData,
            int customReferenceNumber,
            Guid customReferenceGuid,
            string status)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentWorkflow_Update", 8);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@LastModUserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModUserGuid);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            sph.DefineSqlParameter("@Status", SqlDbType.NVarChar, 20, ParameterDirection.Input, status);
            sph.DefineSqlParameter("@ContentText", SqlDbType.NVarChar, -1, ParameterDirection.Input, contentText);
            sph.DefineSqlParameter("@CustomData", SqlDbType.NVarChar, -1, ParameterDirection.Input, customData);

            //object customReferenceNumberVal = customReferenceNumber.HasValue ? (object)customReferenceNumber.Value : DBNull.Value;
            sph.DefineSqlParameter("@CustomReferenceNumber", SqlDbType.Int, ParameterDirection.Input, customReferenceNumber);

            //object customReferenceGuidVal = customReferenceGuid.HasValue ? (object)customReferenceGuid.Value : Guid.Empty;
            sph.DefineSqlParameter("@CustomReferenceGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, customReferenceGuid);

            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;
        }

        

        /// <summary>
        /// Deletes rows from the mp_ContentWorkflow table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentWorkflow_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_ContentWorkflow table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentWorkflow_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        //public static bool RejectContentChanges(Guid moduleGuid, Guid rejectorGuid, string notes)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(GetWriteConnectionString(), "mp_ContentWorkflow_RejectChanges", 4);
        //    sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
        //    sph.DefineSqlParameter("@RejectorGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rejectorGuid);
        //    sph.DefineSqlParameter("@Notes", SqlDbType.NVarChar, ParameterDirection.Input, notes);
        //    sph.DefineSqlParameter("@CreatedDateUtc", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);

        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > 0);
        //}

        public static IDataReader GetWorkInProgress(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentWorkflow_SelectWorkInProgress", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetWorkInProgress(Guid moduleGuid, string status)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentWorkflow_SelectWorkInProgressByStatus", 2);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@Status", SqlDbType.NVarChar, 20, ParameterDirection.Input, status);
            return sph.ExecuteReader();
        }

        public static int GetWorkInProgressCountByPage(Guid pageGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentWorkflow_GetInProgressCountByPage", 1);
            sph.DefineSqlParameter("@PageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pageGuid);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static int GetCount(Guid siteGuid, string status)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentWorkflow_GetCount", 2);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@Status", SqlDbType.NVarChar, ParameterDirection.Input, status);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static Guid GetDraftSubmitter(Guid contentWorkflowGuid)
        {
            Guid result = Guid.Empty;
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentWorkflowAuditHistory_GetDraftSubmitter", 1);
            sph.DefineSqlParameter("@ContentWorkflowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, contentWorkflowGuid);
            using (IDataReader reader = sph.ExecuteReader())
            {
                if (reader.Read())
                {
                    result = new Guid(reader[0].ToString());
                }
            }
            
            return result;
        }

        public static IDataReader GetPage(
            Guid siteGuid,
            string status,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = GetCount(siteGuid, status);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentWorkflow_SelectPage", 4);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@Status", SqlDbType.NVarChar, ParameterDirection.Input, status);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static IDataReader GetPageInfoForPage(
            Guid siteGuid,
            string status,
            int pageNumber,
            int pageSize)
        {

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentWorkflow_SelectPageInfoForPage", 4);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@Status", SqlDbType.NVarChar, ParameterDirection.Input, status);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }


        public static int CreateAuditHistory(
            Guid guid,
            Guid workflowGuid,
            Guid moduleGuid,
            Guid userGuid,
            DateTime createdDateUtc,
            string status,
            string notes,
            bool active)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentWorkflowAuditHistory_Insert", 8);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@ContentWorkflowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, workflowGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@CreatedDateUtc", SqlDbType.DateTime, ParameterDirection.Input, createdDateUtc);
            sph.DefineSqlParameter("@Status", SqlDbType.NVarChar, 20, ParameterDirection.Input, status);
            sph.DefineSqlParameter("@Notes", SqlDbType.NVarChar, -1, ParameterDirection.Input, notes);
            sph.DefineSqlParameter("@Active", SqlDbType.Bit, ParameterDirection.Input, active);
            

            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;
        }

        public static bool DeactivateAudit(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentWorkflowAuditHistory_Deactivate", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }


    }

}
