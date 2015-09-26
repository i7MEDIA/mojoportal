// Author:					Joe Audette
// Created:					2009-07-17
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
using FirebirdSql.Data.FirebirdClient;

namespace mojoPortal.Data
{
    public static class DBContentWorkflow
    {
        private static String GetReadConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

        }

        private static String GetWriteConnectionString()
        {
            if (ConfigurationManager.AppSettings["FirebirdWriteConnectionString"] != null)
            {
                return ConfigurationManager.AppSettings["FirebirdWriteConnectionString"];
            }

            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

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

            FbParameter[] arParams = new FbParameter[12];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new FbParameter("@ModuleGuid", FbDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new FbParameter("@CreatedDateUtc", FbDbType.TimeStamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = createdDateUtc;

            arParams[4] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userGuid.ToString();

            arParams[5] = new FbParameter("@LastModUserGuid", FbDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = userGuid.ToString();

            arParams[6] = new FbParameter("@LastModUtc", FbDbType.TimeStamp);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdDateUtc;

            arParams[7] = new FbParameter("@Status", FbDbType.VarChar, 20);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = status;

            arParams[8] = new FbParameter("@ContentText", FbDbType.VarChar);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = contentText;

            arParams[9] = new FbParameter("@CustomData", FbDbType.VarChar);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = customData;

            arParams[10] = new FbParameter("@CustomReferenceNumber", FbDbType.Integer);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = customReferenceNumber;

            arParams[11] = new FbParameter("@CustomReferenceGuid", FbDbType.Char, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = customReferenceGuid.ToString();


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_ContentWorkflow (");
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
            sqlCommand.Append("CustomReferenceGuid )");

            sqlCommand.Append(" VALUES (");
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
            sqlCommand.Append("@CustomReferenceGuid )");
            sqlCommand.Append(";");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
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

            FbParameter[] arParams = new FbParameter[8];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@LastModUserGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = lastModUserGuid.ToString();

            arParams[2] = new FbParameter("@LastModUtc", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = lastModUtc;

            arParams[3] = new FbParameter("@Status", FbDbType.VarChar, 20);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = status;

            arParams[4] = new FbParameter("@ContentText", FbDbType.VarChar);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = contentText;

            arParams[5] = new FbParameter("@CustomData", FbDbType.VarChar);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = customData;

            arParams[6] = new FbParameter("@CustomReferenceNumber", FbDbType.Integer);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = customReferenceNumber;

            arParams[7] = new FbParameter("@CustomReferenceGuid", FbDbType.Char, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = customReferenceGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
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
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentWorkflowAuditHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(";");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentWorkflow ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(";");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
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
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentWorkflowAuditHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentWorkflowGuid IN (SELECT Guid FROM mp_ContentWorkflow WHERE SiteGuid = @SiteGuid)  ");
            sqlCommand.Append(";");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentWorkflow ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append(";");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
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
            sqlCommand.Append("createdBy.Name as CreatedByUserName, ");
            sqlCommand.Append("createdBy.LoginName as CreatedByUserLogin, ");
            sqlCommand.Append("createdBy.Email as CreatedByUserEmail, ");
            sqlCommand.Append("createdBy.FirstName as CreatedByFirstName, ");
            sqlCommand.Append("createdBy.LastName as CreatedByLastName, ");
            sqlCommand.Append("createdBy.UserID as CreatedByUserID, ");
            sqlCommand.Append("createdBy.AvatarUrl as CreatedByAvatar, ");
            sqlCommand.Append("createdBy.AuthorBio as CreatedByAuthorBio, ");

            sqlCommand.Append("modifiedBy.Name as ModifiedByUserName, ");
            sqlCommand.Append("modifiedBy.FirstName as ModifiedByFirstName, ");
            sqlCommand.Append("modifiedBy.LastName as ModifiedByLastName, ");
            sqlCommand.Append("modifiedBy.LoginName as ModifiedByUserLogin, ");
            sqlCommand.Append("modifiedBy.Email as ModifiedByUserEmail, ");

            sqlCommand.Append("cwah.Notes as Notes, ");
            sqlCommand.Append("cwah.CreatedDateUtc as RecentActionOn, ");

            sqlCommand.Append("recentActionBy.Name as RecentActionByUserName, ");
            sqlCommand.Append("recentActionBy.LoginName as RecentActionByUserLogin, ");
            sqlCommand.Append("recentActionBy.Email as RecentActionByUserEmail ");

            sqlCommand.Append("FROM	mp_ContentWorkflow cw ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cw.ModuleGuid = m.Guid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users createdBy ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("createdBy.UserGuid = cw.UserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users modifiedBy ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("modifiedBy.UserGuid = cw.LastModUserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_ContentWorkflowAuditHistory cwah ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cwah.ContentWorkflowGuid = cw.Guid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cwah.\"Active\" = 1 ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users recentActionBy ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("recentActionBy.UserGuid = cwah.UserGuid ");


            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cw.ModuleGuid = @ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.Status NOT IN ('Cancelled','Approved') ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("CreatedDateUtc DESC ");

            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetWorkInProgress(Guid moduleGuid, string status)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT  cw.*, ");
            sqlCommand.Append("m.ModuleID, ");
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("createdBy.Name as CreatedByUserName, ");
            sqlCommand.Append("createdBy.LoginName as CreatedByUserLogin, ");
            sqlCommand.Append("createdBy.Email as CreatedByUserEmail, ");
            sqlCommand.Append("createdBy.FirstName as CreatedByFirstName, ");
            sqlCommand.Append("createdBy.LastName as CreatedByLastName, ");
            sqlCommand.Append("createdBy.UserID as CreatedByUserID, ");
            sqlCommand.Append("createdBy.AvatarUrl as CreatedByAvatar, ");
            sqlCommand.Append("createdBy.AuthorBio as CreatedByAuthorBio, ");

            sqlCommand.Append("modifiedBy.Name as ModifiedByUserName, ");
            sqlCommand.Append("modifiedBy.FirstName as ModifiedByFirstName, ");
            sqlCommand.Append("modifiedBy.LastName as ModifiedByLastName, ");
            sqlCommand.Append("modifiedBy.LoginName as ModifiedByUserLogin, ");
            sqlCommand.Append("modifiedBy.Email as ModifiedByUserEmail, ");


            sqlCommand.Append("cwah.Notes as Notes, ");
            sqlCommand.Append("cwah.CreatedDateUtc as RecentActionOn, ");
            sqlCommand.Append("recentActionBy.Name as RecentActionByUserName, ");
            sqlCommand.Append("recentActionBy.LoginName as RecentActionByUserLogin, ");
            sqlCommand.Append("recentActionBy.Email as RecentActionByUserEmail ");

            sqlCommand.Append("FROM	mp_ContentWorkflow cw ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cw.ModuleGuid = m.Guid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users createdBy ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("createdBy.UserGuid = cw.UserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users modifiedBy ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("modifiedBy.UserGuid = cw.LastModUserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_ContentWorkflowAuditHistory cwah ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cwah.ContentWorkflowGuid = cw.Guid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cwah.\"Active\" = 1 ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users recentActionBy ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("recentActionBy.UserGuid = cwah.UserGuid ");


            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cw.ModuleGuid = @ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.Status = @Status ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("CreatedDateUtc DESC ");

            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ModuleGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new FbParameter("@Status", FbDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = status;

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
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
            sqlCommand.Append("cw.Status Not In ('Cancelled','Approved') ");

            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@PageGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static Guid GetDraftSubmitter(Guid contentWorkflowGuid)
        {
            Guid result = Guid.Empty;

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ContentWorkflowGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentWorkflowGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST 1 UserGuid ");
            sqlCommand.Append("FROM	mp_ContentWorkflowAuditHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentWorkflowGuid = @ContentWorkflowGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("NewStatus = 'AwaitingApproval' ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("CreatedDateUtc DESC ");
            sqlCommand.Append(";");

            using(IDataReader reader = FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
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
            sqlCommand.Append("Status = @Status ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new FbParameter("@Status", FbDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = status;

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetReadConnectionString(),
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
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }

            sqlCommand.Append("	cw.*, ");
            sqlCommand.Append("m.ModuleID, ");
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("createdBy.Name as CreatedByUserName, ");
            sqlCommand.Append("createdBy.LoginName as CreatedByUserLogin, ");
            sqlCommand.Append("createdBy.Email as CreatedByUserEmail, ");
            sqlCommand.Append("cwah.Notes as Notes, ");
            sqlCommand.Append("cwah.CreatedDateUtc as RecentActionOn, ");
            sqlCommand.Append("recentActionBy.Name as RecentActionByUserName, ");
            sqlCommand.Append("recentActionBy.LoginName as RecentActionByUserLogin, ");
            sqlCommand.Append("recentActionBy.Email as RecentActionByUserEmail ");

            sqlCommand.Append("FROM	mp_ContentWorkflow cw  ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cw.ModuleGuid = m.Guid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users createdBy ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("createdBy.UserGuid = cw.UserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_ContentWorkflowAuditHistory cwah ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cwah.ContentWorkflowGuid = cw.Guid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cwah.\"Active\" = 1 ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users recentActionBy ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("recentActionBy.UserGuid = cwah.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cw.SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.Status = @Status ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("cw.CreatedDateUtc DESC ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new FbParameter("@Status", FbDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = status;

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetPageInfoForPage(
            Guid siteGuid,
            string status,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }

            sqlCommand.Append("p.PageID, ");
            sqlCommand.Append("p.PageGuid, ");
            sqlCommand.Append("p.PageName, ");
            sqlCommand.Append("p.UseUrl, ");
            sqlCommand.Append("p.Url As PageUrl, ");
            sqlCommand.Append("cw.Guid as WorkflowGuid ");
            
            sqlCommand.Append("FROM	mp_ContentWorkflow cw  ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cw.ModuleGuid = m.Guid ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_PageModules pm ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("pm.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Pages p ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("pm.PageID = p.PageID ");
            
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cw.SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.Status = @Status ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("cw.CreatedDateUtc DESC ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new FbParameter("@Status", FbDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = status;

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
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
            #region Bit Conversion

            int intActive = 0;
            if (active) { intActive = 1; }


            #endregion

            FbParameter[] arParams = new FbParameter[8];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@ContentWorkflowGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = workflowGuid.ToString();

            arParams[2] = new FbParameter("@ModuleGuid", FbDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new FbParameter("@CreatedDateUtc", FbDbType.TimeStamp);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = createdDateUtc;

            arParams[5] = new FbParameter("@NewStatus", FbDbType.VarChar, 20);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = status;

            arParams[6] = new FbParameter("@Notes", FbDbType.VarChar);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = notes;

            arParams[7] = new FbParameter("@Active", FbDbType.Integer);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intActive;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_ContentWorkflowAuditHistory (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("ContentWorkflowGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("CreatedDateUtc, ");
            sqlCommand.Append("NewStatus, ");
            sqlCommand.Append("Notes, ");
            sqlCommand.Append("\"Active\" )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@ContentWorkflowGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@CreatedDateUtc, ");
            sqlCommand.Append("@NewStatus, ");
            sqlCommand.Append("@Notes, ");
            sqlCommand.Append("@Active )");
            sqlCommand.Append(";");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;


        }

        public static bool DeactivateAudit(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_ContentWorkflowAuditHistory ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("\"Active\" = 0 ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


    }
}
