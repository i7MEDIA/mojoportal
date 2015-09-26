// Author:					Joe Audette
// Created:					2010-04-03
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
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Data.SqlServerCe;

namespace mojoPortal.Data
{
    public static class DBContentWorkflow
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_ContentWorkflow ");
            sqlCommand.Append("(");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("CreatedDateUtc, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("LastModUserGuid, ");
            sqlCommand.Append("LastModUtc, ");
            sqlCommand.Append("Status, ");
            sqlCommand.Append("ContentText, ");
            sqlCommand.Append("CustomData, ");
            sqlCommand.Append("CustomReferenceNumber, ");
            sqlCommand.Append("CustomReferenceGuid ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@CreatedDateUtc, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@LastModUserGuid, ");
            sqlCommand.Append("@LastModUtc, ");
            sqlCommand.Append("@Status, ");
            sqlCommand.Append("@ContentText, ");
            sqlCommand.Append("@CustomData, ");
            sqlCommand.Append("@CustomReferenceNumber, ");
            sqlCommand.Append("@CustomReferenceGuid ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[12];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid;

            arParams[2] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid;

            arParams[3] = new SqlCeParameter("@CreatedDateUtc", SqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = createdDateUtc;

            arParams[4] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userGuid;

            arParams[5] = new SqlCeParameter("@LastModUserGuid", SqlDbType.UniqueIdentifier);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = userGuid;

            arParams[6] = new SqlCeParameter("@LastModUtc", SqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdDateUtc;

            arParams[7] = new SqlCeParameter("@Status", SqlDbType.NVarChar, 20);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = status;

            arParams[8] = new SqlCeParameter("@ContentText", SqlDbType.NText);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = contentText;

            arParams[9] = new SqlCeParameter("@CustomData", SqlDbType.NText);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = customData;

            arParams[10] = new SqlCeParameter("@CustomReferenceNumber", SqlDbType.Int);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = customReferenceNumber;

            arParams[11] = new SqlCeParameter("@CustomReferenceGuid", SqlDbType.UniqueIdentifier);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = customReferenceGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_ContentWorkflow ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("LastModUserGuid = @LastModUserGuid, ");
            sqlCommand.Append("LastModUtc = @LastModUtc, ");
            sqlCommand.Append("Status = @Status, ");
            sqlCommand.Append("ContentText = @ContentText, ");
            sqlCommand.Append("CustomData = @CustomData, ");
            sqlCommand.Append("CustomReferenceNumber = @CustomReferenceNumber, ");
            sqlCommand.Append("CustomReferenceGuid = @CustomReferenceGuid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[8];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@LastModUserGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = lastModUserGuid;

            arParams[2] = new SqlCeParameter("@LastModUtc", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = lastModUtc;

            arParams[3] = new SqlCeParameter("@Status", SqlDbType.NVarChar, 20);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = status;

            arParams[4] = new SqlCeParameter("@ContentText", SqlDbType.NText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = contentText;

            arParams[5] = new SqlCeParameter("@CustomData", SqlDbType.NText);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = customData;

            arParams[6] = new SqlCeParameter("@CustomReferenceNumber", SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = customReferenceNumber;

            arParams[7] = new SqlCeParameter("@CustomReferenceGuid", SqlDbType.UniqueIdentifier);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = customReferenceGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Deletes rows from the mp_ContentWorkflow table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            DeleteAuditHistoryByModule(moduleGuid);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentWorkflow ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        private static bool DeleteAuditHistoryByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentWorkflowAuditHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes rows from the mp_ContentWorkflow table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            DeleteAuditHistoryBySite(siteGuid);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentWorkflow ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        private static bool DeleteAuditHistoryBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentWorkflowAuditHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentWorkflowGuid IN ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT [Guid] FROM mp_ContentWorkflow");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append(") ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetWorkInProgress(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  cw.*, ");
            sqlCommand.Append("m.ModuleID, ");
            sqlCommand.Append("m.ModuleTitle, ");

            sqlCommand.Append("createdBy.[Name] as CreatedByUserName, ");
            sqlCommand.Append("createdBy.LoginName as CreatedByUserLogin, ");
            sqlCommand.Append("createdBy.Email as CreatedByUserEmail, ");
            sqlCommand.Append("createdBy.FirstName as CreatedByFirstName, ");
            sqlCommand.Append("createdBy.LastName as CreatedByLastName, ");
            sqlCommand.Append("createdBy.UserID as CreatedByUserID, ");
            sqlCommand.Append("createdBy.AvatarUrl as CreatedByAvatar, ");
            sqlCommand.Append("createdBy.AuthorBio as CreatedByAuthorBio, ");

            sqlCommand.Append("modifiedBy.[Name] as ModifiedByUserName, ");
            sqlCommand.Append("modifiedBy.FirstName as ModifiedByFirstName, ");
            sqlCommand.Append("modifiedBy.LastName as ModifiedByLastName, ");
            sqlCommand.Append("modifiedBy.LoginName as ModifiedByUserLogin, ");
            sqlCommand.Append("modifiedBy.Email as ModifiedByUserEmail, ");


            sqlCommand.Append("cwah.Notes as Notes, ");
            sqlCommand.Append("cwah.CreatedDateUtc as RecentActionOn, ");
            sqlCommand.Append("recentActionBy.[Name] as RecentActionByUserName,");
            sqlCommand.Append("recentActionBy.LoginName as RecentActionByUserLogin, ");
            sqlCommand.Append("recentActionBy.Email as RecentActionByUserEmail ");

            sqlCommand.Append("FROM	mp_ContentWorkflow cw ");

            sqlCommand.Append("JOIN mp_Modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cw.ModuleGuid = m.[Guid] ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("[mp_Users] createdBy ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("createdBy.UserGuid = cw.UserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("[mp_Users] modifiedBy ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("modifiedBy.UserGuid = cw.LastModUserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("[mp_ContentWorkflowAuditHistory] cwah ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cwah.ContentWorkflowGuid = cw.[Guid] ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cwah.Active = 1	");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("[mp_Users] recentActionBy ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("recentActionBy.UserGuid = cwah.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cw.ModuleGuid = @ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.[Status] Not In ('Cancelled','Approved') ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("cw.CreatedDateUtc DESC ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetWorkInProgress(Guid moduleGuid, string status)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  cw.*, ");
            sqlCommand.Append("m.ModuleID, ");
            sqlCommand.Append("m.ModuleTitle, ");

            sqlCommand.Append("createdBy.[Name] as CreatedByUserName, ");
            sqlCommand.Append("createdBy.LoginName as CreatedByUserLogin, ");
            sqlCommand.Append("createdBy.Email as CreatedByUserEmail, ");
            sqlCommand.Append("createdBy.FirstName as CreatedByFirstName, ");
            sqlCommand.Append("createdBy.LastName as CreatedByLastName, ");
            sqlCommand.Append("createdBy.UserID as CreatedByUserID, ");
            sqlCommand.Append("createdBy.AvatarUrl as CreatedByAvatar, ");
            sqlCommand.Append("createdBy.AuthorBio as CreatedByAuthorBio, ");

            sqlCommand.Append("modifiedBy.[Name] as ModifiedByUserName, ");
            sqlCommand.Append("modifiedBy.FirstName as ModifiedByFirstName, ");
            sqlCommand.Append("modifiedBy.LastName as ModifiedByLastName, ");
            sqlCommand.Append("modifiedBy.LoginName as ModifiedByUserLogin, ");
            sqlCommand.Append("modifiedBy.Email as ModifiedByUserEmail, ");



            sqlCommand.Append("cwah.Notes as Notes, ");
            sqlCommand.Append("cwah.CreatedDateUtc as RecentActionOn, ");
            sqlCommand.Append("recentActionBy.[Name] as RecentActionByUserName,");
            sqlCommand.Append("recentActionBy.LoginName as RecentActionByUserLogin, ");
            sqlCommand.Append("recentActionBy.Email as RecentActionByUserEmail ");

            sqlCommand.Append("FROM	mp_ContentWorkflow cw ");

            sqlCommand.Append("JOIN mp_Modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cw.ModuleGuid = m.[Guid] ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("[mp_Users] createdBy ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("createdBy.UserGuid = cw.UserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("[mp_Users] modifiedBy ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("modifiedBy.UserGuid = cw.LastModUserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("[mp_ContentWorkflowAuditHistory] cwah ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cwah.ContentWorkflowGuid = cw.[Guid] ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cwah.Active = 1	");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("[mp_Users] recentActionBy ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("recentActionBy.UserGuid = cwah.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cw.ModuleGuid = @ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.[Status] = @Status ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("cw.CreatedDateUtc DESC ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            arParams[1] = new SqlCeParameter("@Status", SqlDbType.NVarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = status;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetWorkInProgressCountByPage(Guid pageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_ContentWorkflow cw ");
            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_PageModules pm ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("pm.ModuleGuid = cw.ModuleGuid ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pm.PageGuid = @PageGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.[Status] Not In ('Cancelled','Approved') ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public static Guid GetDraftSubmitter(Guid contentWorkflowGuid)
        {
            Guid result = Guid.Empty;

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ContentWorkflowGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentWorkflowGuid;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP(1) UserGuid ");
            sqlCommand.Append("FROM	mp_ContentWorkflowAuditHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentWorkflowGuid = @ContentWorkflowGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("NewStatus = 'AwaitingApproval' ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("CreatedDateUtc DESC ");
            sqlCommand.Append(";");

            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    result = new Guid(reader[0].ToString());
                }
            }

            return result;
        }

        public static int GetCount(Guid siteGuid, string status)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_ContentWorkflow ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("[Status] = @Status ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new SqlCeParameter("@Status", SqlDbType.NVarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = status;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public static IDataReader GetPage(
            Guid siteGuid,
            string status,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") * ");

            sqlCommand.Append("FROM	mp_ContentWorkflow  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("[Status] = @Status ");
            //sqlCommand.Append("ORDER BY  ");
            //sqlCommand.Append("DESC  ");

            sqlCommand.Append(") AS t1 ");
            //sqlCommand.Append("ORDER BY  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new SqlCeParameter("@Status", SqlDbType.NVarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = status;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetPageInfoForPage(
            Guid siteGuid,
            string status,
            int pageNumber,
            int pageSize)
        {
            

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ")  ");
            sqlCommand.Append("p.PageID, ");
            sqlCommand.Append("p.PageGuid, ");
            sqlCommand.Append("p.PageName, ");
            sqlCommand.Append("p.UseUrl, ");
            sqlCommand.Append("p.Url As PageUrl, ");
            sqlCommand.Append("cw.[Guid] As WorkflowGuid ");

            sqlCommand.Append("FROM	mp_ContentWorkflow cw  ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cw.ModuleGuid = m.[Guid] ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("[mp_PageModules] pm ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("pm.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("[mp_Pages] p ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("pm.PageID = p.PageID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cw.SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.[Status] = @Status ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("cw.CreatedDateUtc DESC  ");

            sqlCommand.Append(") AS t1 ");
            //sqlCommand.Append("ORDER BY  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new SqlCeParameter("@Status", SqlDbType.NVarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = status;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_ContentWorkflowAuditHistory ");
            sqlCommand.Append("(");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("ContentWorkflowGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("CreatedDateUtc, ");
            sqlCommand.Append("NewStatus, ");
            sqlCommand.Append("Notes, ");
            sqlCommand.Append("Active ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@ContentWorkflowGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@CreatedDateUtc, ");
            sqlCommand.Append("@NewStatus, ");
            sqlCommand.Append("@Notes, ");
            sqlCommand.Append("@Active ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[8];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@ContentWorkflowGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = workflowGuid;

            arParams[2] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid;

            arParams[3] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid;

            arParams[4] = new SqlCeParameter("@CreatedDateUtc", SqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = createdDateUtc;

            arParams[5] = new SqlCeParameter("@NewStatus", SqlDbType.NVarChar, 20);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = status;

            arParams[6] = new SqlCeParameter("@Notes", SqlDbType.NText);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = notes;

            arParams[7] = new SqlCeParameter("@Active", SqlDbType.Bit);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = active;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;


        }

        public static bool DeactivateAudit(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_ContentWorkflowAuditHistory ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("Active = 0 ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


    }
}
