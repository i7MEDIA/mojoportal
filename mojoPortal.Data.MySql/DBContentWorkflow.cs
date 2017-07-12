// Author:					
// Created:					2009-07-19
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
using System.Configuration;
using MySql.Data.MySqlClient;

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
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?ModuleGuid, ");
            sqlCommand.Append("?CreatedDateUtc, ");
            sqlCommand.Append("?UserGuid, ");
            sqlCommand.Append("?LastModUserGuid, ");
            sqlCommand.Append("?LastModUtc, ");
            sqlCommand.Append("?Status, ");
            sqlCommand.Append("?ContentText, ");
            sqlCommand.Append("?CustomData, ");
            sqlCommand.Append("?CustomReferenceNumber, ");
            sqlCommand.Append("?CustomReferenceGuid )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[12];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new MySqlParameter("?CreatedDateUtc", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = createdDateUtc;

            arParams[4] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userGuid.ToString();

            arParams[5] = new MySqlParameter("?LastModUserGuid", MySqlDbType.VarChar, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = userGuid.ToString();

            arParams[6] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdDateUtc;

            arParams[7] = new MySqlParameter("?Status", MySqlDbType.VarChar, 20);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = status;

            arParams[8] = new MySqlParameter("?ContentText", MySqlDbType.LongText);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = contentText;

            arParams[9] = new MySqlParameter("?CustomData", MySqlDbType.Text);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = customData;

            arParams[10] = new MySqlParameter("?CustomReferenceNumber", MySqlDbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = customReferenceNumber;

            arParams[11] = new MySqlParameter("?CustomReferenceGuid", MySqlDbType.VarChar, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = customReferenceGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            
            sqlCommand.Append("LastModUserGuid = ?LastModUserGuid, ");
            sqlCommand.Append("LastModUtc = ?LastModUtc, ");
            sqlCommand.Append("Status = ?Status, ");
            sqlCommand.Append("ContentText = ?ContentText, ");
            sqlCommand.Append("CustomData = ?CustomData, ");
            sqlCommand.Append("CustomReferenceNumber = ?CustomReferenceNumber, ");
            sqlCommand.Append("CustomReferenceGuid = ?CustomReferenceGuid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[8];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?LastModUserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = lastModUserGuid.ToString();

            arParams[2] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = lastModUtc;

            arParams[3] = new MySqlParameter("?Status", MySqlDbType.VarChar, 20);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = status;

            arParams[4] = new MySqlParameter("?ContentText", MySqlDbType.LongText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = contentText;

            arParams[5] = new MySqlParameter("?CustomData", MySqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = customData;

            arParams[6] = new MySqlParameter("?CustomReferenceNumber", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = customReferenceNumber;

            arParams[7] = new MySqlParameter("?CustomReferenceGuid", MySqlDbType.VarChar, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = customReferenceGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append(";");

            sqlCommand.Append("DELETE FROM mp_ContentWorkflow ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("ContentWorkflowGuid IN (SELECT Guid FROM mp_ContentWorkflow WHERE SiteGuid = ?SiteGuid)  ");
            sqlCommand.Append(";");

            sqlCommand.Append("DELETE FROM mp_ContentWorkflow ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("cw.ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.Status NOT IN ('Cancelled','Approved') ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("CreatedDateUtc DESC ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("cw.ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.Status = ?Status ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("CreatedDateUtc DESC ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?Status", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = status;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("pm.PageGuid = ?PageGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.Status Not In ('Cancelled','Approved') ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?PageGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static Guid GetDraftSubmitter(Guid contentWorkflowGuid)
        {
            Guid result = Guid.Empty;

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ContentWorkflowGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentWorkflowGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT UserGuid ");
            sqlCommand.Append("FROM	mp_ContentWorkflowAuditHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentWorkflowGuid = ?ContentWorkflowGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("NewStatus = 'AwaitingApproval' ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("CreatedDateUtc DESC ");
            sqlCommand.Append("LIMIT 1 ");
            sqlCommand.Append(";");

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("Status = ?Status ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?Status", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = status;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("cw.SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.Status = ?Status ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("cw.CreatedDateUtc DESC ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?Status", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = status;

            arParams[2] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("cw.SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.Status = ?Status ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("cw.CreatedDateUtc DESC ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?Status", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = status;

            arParams[2] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?ContentWorkflowGuid, ");
            sqlCommand.Append("?ModuleGuid, ");
            sqlCommand.Append("?UserGuid, ");
            sqlCommand.Append("?CreatedDateUtc, ");
            sqlCommand.Append("?NewStatus, ");
            sqlCommand.Append("?Notes, ");
            sqlCommand.Append("?Active )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[8];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?ContentWorkflowGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = workflowGuid.ToString();

            arParams[2] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new MySqlParameter("?CreatedDateUtc", MySqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = createdDateUtc;

            arParams[5] = new MySqlParameter("?NewStatus", MySqlDbType.VarChar, 20);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = status;

            arParams[6] = new MySqlParameter("?Notes", MySqlDbType.Text);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = notes;

            arParams[7] = new MySqlParameter("?Active", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intActive;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }



    }
}
