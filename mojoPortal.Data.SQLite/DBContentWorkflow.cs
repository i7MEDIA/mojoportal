// Author:					
// Created:					2009-07-15
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
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using Mono.Data.Sqlite;


namespace mojoPortal.Data
{
    public static class DBContentWorkflow
    {
        private static string GetConnectionString()
        {
            string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];
            if (connectionString == "defaultdblocation")
            {

                connectionString = "version=3,URI=file:"
                    + System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/mojo.db.config");

            }
            return connectionString;
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
            sqlCommand.Append(":Guid, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":ModuleGuid, ");
            sqlCommand.Append(":CreatedDateUtc, ");
            sqlCommand.Append(":UserGuid, ");
            sqlCommand.Append(":LastModUserGuid, ");
            sqlCommand.Append(":LastModUtc, ");
            sqlCommand.Append(":Status, ");
            sqlCommand.Append(":ContentText, ");
            sqlCommand.Append(":CustomData, ");
            sqlCommand.Append(":CustomReferenceNumber, ");
            sqlCommand.Append(":CustomReferenceGuid )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[12];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new SqliteParameter(":CreatedDateUtc", DbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = createdDateUtc;

            arParams[4] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userGuid.ToString();

            arParams[5] = new SqliteParameter(":LastModUserGuid", DbType.String, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = userGuid.ToString();

            arParams[6] = new SqliteParameter(":LastModUtc", DbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdDateUtc;

            arParams[7] = new SqliteParameter(":Status", DbType.String, 20);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = status;

            arParams[8] = new SqliteParameter(":ContentText", DbType.Object);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = contentText;

            arParams[9] = new SqliteParameter(":CustomData", DbType.Object);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = customData;

            arParams[10] = new SqliteParameter(":CustomReferenceNumber", DbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = customReferenceNumber;

            arParams[11] = new SqliteParameter(":CustomReferenceGuid", DbType.String, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = customReferenceGuid.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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
           
            sqlCommand.Append("LastModUserGuid = :LastModUserGuid, ");
            sqlCommand.Append("LastModUtc = :LastModUtc, ");
            sqlCommand.Append("Status = :Status, ");
            sqlCommand.Append("ContentText = :ContentText, ");
            sqlCommand.Append("CustomData = :CustomData, ");
            sqlCommand.Append("CustomReferenceNumber = :CustomReferenceNumber, ");
            sqlCommand.Append("CustomReferenceGuid = :CustomReferenceGuid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[8];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":LastModUserGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = lastModUserGuid.ToString();

            arParams[2] = new SqliteParameter(":LastModUtc", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = lastModUtc;

            arParams[3] = new SqliteParameter(":Status", DbType.String, 20);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = status;

            arParams[4] = new SqliteParameter(":ContentText", DbType.Object);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = contentText;

            arParams[5] = new SqliteParameter(":CustomData", DbType.Object);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = customData;

            arParams[6] = new SqliteParameter(":CustomReferenceNumber", DbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = customReferenceNumber;

            arParams[7] = new SqliteParameter(":CustomReferenceGuid", DbType.String, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = customReferenceGuid.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("DELETE FROM mp_ContentWorkflowAuditHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = :ModuleGuid ");
            sqlCommand.Append(";");

            sqlCommand.Append("DELETE FROM mp_ContentWorkflow ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = :ModuleGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_ContentWorkflow table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("DELETE FROM mp_ContentWorkflowAuditHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentWorkflowGuid IN (SELECT Guid FROM mp_ContentWorkflow WHERE SiteGuid = :SiteGuid)  ");
            sqlCommand.Append(";");

            sqlCommand.Append("DELETE FROM mp_ContentWorkflow ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = :SiteGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

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
            sqlCommand.Append("cwah.Active = 1 ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users recentActionBy ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("recentActionBy.UserGuid = cwah.UserGuid ");


            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cw.ModuleGuid = :ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.Status NOT IN ('Cancelled','Approved') ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("cw.CreatedDateUtc DESC ");

            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("cwah.Active = 1 ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users recentActionBy ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("recentActionBy.UserGuid = cwah.UserGuid ");


            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cw.ModuleGuid = :ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.Status = :Status ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("cw.CreatedDateUtc DESC ");

            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new SqliteParameter(":Status", DbType.String, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = status;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("pm.PageGuid = :PageGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.Status Not In ('Cancelled','Approved') ");

            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":PageGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static Guid GetDraftSubmitter(Guid contentWorkflowGuid)
        {
            Guid result = Guid.Empty;

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ContentWorkflowGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentWorkflowGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT UserGuid ");
            sqlCommand.Append("FROM	mp_ContentWorkflowAuditHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentWorkflowGuid = :ContentWorkflowGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("NewStatus = 'AwaitingApproval' ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("CreatedDateUtc DESC ");
            sqlCommand.Append("LIMIT 1 ");
            sqlCommand.Append(";");

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("SiteGuid = :SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("Status = :Status ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new SqliteParameter(":Status", DbType.String, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = status;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
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
            sqlCommand.Append("SELECT ");

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
            sqlCommand.Append("cwah.Active = 1 ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users recentActionBy ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("recentActionBy.UserGuid = cwah.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cw.SiteGuid = :SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.Status = :Status ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("cw.CreatedDateUtc DESC ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            
            sqlCommand.Append(";");


            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new SqliteParameter(":Status", DbType.String, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = status;

            arParams[2] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("SELECT ");

            sqlCommand.Append("p.PageID As PageID, ");
            sqlCommand.Append("p.PageGuid As PageGuid, ");
            sqlCommand.Append("p.PageName As PageName, ");
            sqlCommand.Append("p.UseUrl As UseUrl, ");
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
            sqlCommand.Append("cw.SiteGuid = :SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.Status = :Status ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("cw.CreatedDateUtc DESC ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }

            sqlCommand.Append(";");


            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new SqliteParameter(":Status", DbType.String, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = status;

            arParams[2] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_ContentWorkflowAuditHistory (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("ContentWorkflowGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("CreatedDateUtc, ");
            sqlCommand.Append("NewStatus, ");
            sqlCommand.Append("Notes, ");
            sqlCommand.Append("Active )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":Guid, ");
            sqlCommand.Append(":ContentWorkflowGuid, ");
            sqlCommand.Append(":ModuleGuid, ");
            sqlCommand.Append(":UserGuid, ");
            sqlCommand.Append(":CreatedDateUtc, ");
            sqlCommand.Append(":NewStatus, ");
            sqlCommand.Append(":Notes, ");
            sqlCommand.Append(":Active )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[8];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":ContentWorkflowGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = workflowGuid.ToString();

            arParams[2] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new SqliteParameter(":CreatedDateUtc", DbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = createdDateUtc;

            arParams[5] = new SqliteParameter(":NewStatus", DbType.String, 20);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = status;

            arParams[6] = new SqliteParameter(":Notes", DbType.Object);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = notes;

            arParams[7] = new SqliteParameter(":Active", DbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intActive;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("ModuleGuid = :ModuleGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }



    }
}
