// Author:					
// Created:					2012-08-11
// Last Modified:			2012-08-28
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
using mojoPortal.Data; 
	
namespace mojoPortal.Data
{
	public static class DBComments
    {
        /// <summary>
        /// Inserts a row in the mp_Comments table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="parentGuid"> parentGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="featureGuid"> featureGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="contentGuid"> contentGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="title"> title </param>
        /// <param name="userComment"> userComment </param>
        /// <param name="userName"> userName </param>
        /// <param name="userEmail"> userEmail </param>
        /// <param name="userUrl"> userUrl </param>
        /// <param name="userIp"> userIp </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="moderationStatus"> moderationStatus </param>
        /// <param name="moderatedBy"> moderatedBy </param>
        /// <param name="moderationReason"> moderationReason </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid parentGuid,
            Guid siteGuid,
            Guid featureGuid,
            Guid moduleGuid,
            Guid contentGuid,
            Guid userGuid,
            string title,
            string userComment,
            string userName,
            string userEmail,
            string userUrl,
            string userIp,
            DateTime createdUtc,
            byte moderationStatus,
            Guid moderatedBy,
            string moderationReason)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Comments (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("ParentGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("FeatureGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("ContentGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("UserComment, ");
            sqlCommand.Append("UserName, ");
            sqlCommand.Append("UserEmail, ");
            sqlCommand.Append("UserUrl, ");
            sqlCommand.Append("UserIp, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("LastModUtc, ");
            sqlCommand.Append("ModerationStatus, ");
            sqlCommand.Append("ModeratedBy, ");
            sqlCommand.Append("ModerationReason )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":Guid, ");
            sqlCommand.Append(":ParentGuid, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":FeatureGuid, ");
            sqlCommand.Append(":ModuleGuid, ");
            sqlCommand.Append(":ContentGuid, ");
            sqlCommand.Append(":UserGuid, ");
            sqlCommand.Append(":Title, ");
            sqlCommand.Append(":UserComment, ");
            sqlCommand.Append(":UserName, ");
            sqlCommand.Append(":UserEmail, ");
            sqlCommand.Append(":UserUrl, ");
            sqlCommand.Append(":UserIp, ");
            sqlCommand.Append(":CreatedUtc, ");
            sqlCommand.Append(":LastModUtc, ");
            sqlCommand.Append(":ModerationStatus, ");
            sqlCommand.Append(":ModeratedBy, ");
            sqlCommand.Append(":ModerationReason )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[18];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":ParentGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentGuid.ToString();

            arParams[2] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new SqliteParameter(":FeatureGuid", DbType.String, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = featureGuid.ToString();

            arParams[4] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = moduleGuid.ToString();

            arParams[5] = new SqliteParameter(":ContentGuid", DbType.String, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = contentGuid.ToString();

            arParams[6] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = userGuid.ToString();

            arParams[7] = new SqliteParameter(":Title", DbType.String, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = title;

            arParams[8] = new SqliteParameter(":UserComment", DbType.Object);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = userComment;

            arParams[9] = new SqliteParameter(":UserName", DbType.String, 50);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = userName;

            arParams[10] = new SqliteParameter(":UserEmail", DbType.String, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = userEmail;

            arParams[11] = new SqliteParameter(":UserUrl", DbType.String, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = userUrl;

            arParams[12] = new SqliteParameter(":UserIp", DbType.String, 50);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = userIp;

            arParams[13] = new SqliteParameter(":CreatedUtc", DbType.DateTime);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = createdUtc;

            arParams[14] = new SqliteParameter(":LastModUtc", DbType.DateTime);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = createdUtc;

            arParams[15] = new SqliteParameter(":ModerationStatus", DbType.UInt16);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = moderationStatus;

            arParams[16] = new SqliteParameter(":ModeratedBy", DbType.String, 36);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = moderatedBy.ToString();

            arParams[17] = new SqliteParameter(":ModerationReason", DbType.String, 255);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = moderationReason;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;
        }


        /// <summary>
        /// Updates a row in the mp_Comments table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="title"> title </param>
        /// <param name="userComment"> userComment </param>
        /// <param name="userName"> userName </param>
        /// <param name="userEmail"> userEmail </param>
        /// <param name="userUrl"> userUrl </param>
        /// <param name="userIp"> userIp </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="moderationStatus"> moderationStatus </param>
        /// <param name="moderatedBy"> moderatedBy </param>
        /// <param name="moderationReason"> moderationReason </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            Guid userGuid,
            string title,
            string userComment,
            string userName,
            string userEmail,
            string userUrl,
            string userIp,
            DateTime lastModUtc,
            byte moderationStatus,
            Guid moderatedBy,
            string moderationReason)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_Comments ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("UserGuid = :UserGuid, ");
            sqlCommand.Append("Title = :Title, ");
            sqlCommand.Append("UserComment = :UserComment, ");
            sqlCommand.Append("UserName = :UserName, ");
            sqlCommand.Append("UserEmail = :UserEmail, ");
            sqlCommand.Append("UserUrl = :UserUrl, ");
            sqlCommand.Append("UserIp = :UserIp, ");
            sqlCommand.Append("LastModUtc = :LastModUtc, ");
            sqlCommand.Append("ModerationStatus = :ModerationStatus, ");
            sqlCommand.Append("ModeratedBy = :ModeratedBy, ");
            sqlCommand.Append("ModerationReason = :ModerationReason ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[12];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new SqliteParameter(":Title", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new SqliteParameter(":UserComment", DbType.Object);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userComment;

            arParams[4] = new SqliteParameter(":UserName", DbType.String, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userName;

            arParams[5] = new SqliteParameter(":UserEmail", DbType.String, 100);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = userEmail;

            arParams[6] = new SqliteParameter(":UserUrl", DbType.String, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = userUrl;

            arParams[7] = new SqliteParameter(":UserIp", DbType.String, 50);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = userIp;

            arParams[8] = new SqliteParameter(":LastModUtc", DbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModUtc;

            arParams[9] = new SqliteParameter(":ModerationStatus", DbType.UInt16);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = moderationStatus;

            arParams[10] = new SqliteParameter(":ModeratedBy", DbType.String, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = moderatedBy.ToString();

            arParams[11] = new SqliteParameter(":ModerationReason", DbType.String, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = moderationReason;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_Comments table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Comments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes rows from the mp_Comments table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByContent(Guid contentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Comments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentGuid = :ContentGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ContentGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes rows from the mp_Comments table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Comments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = :SiteGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes rows from the mp_Comments table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByFeature(Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Comments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = :SiteGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":FeatureGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes rows from the mp_Comments table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Comments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = :ModuleGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes rows from the mp_Comments table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByParent(Guid parentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Comments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ParentGuid = :ParentGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ParentGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = parentGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_Comments table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT  c.*, ");
            
            //sqlCommand.Append("SELECT ");
            //sqlCommand.Append("c.Guid AS Guid, ");
            //sqlCommand.Append("c.ParentGuid AS ParentGuid, ");
            //sqlCommand.Append("c.SiteGuid AS SiteGuid, ");
            //sqlCommand.Append("c.FeatureGuid AS FeatureGuid, ");
            //sqlCommand.Append("c.ModuleGuid AS ModuleGuid, ");
            //sqlCommand.Append("c.ContentGuid AS ContentGuid, ");
            //sqlCommand.Append("c.UserGuid AS UserGuid, ");
            //sqlCommand.Append("c.Title AS Title, ");
            //sqlCommand.Append("c.UserComment AS UserComment, ");
            //sqlCommand.Append("c.UserName AS UserName ");
            //sqlCommand.Append("c.UserEmail AS UserEmail, ");
            //sqlCommand.Append("c.UserUrl AS UserUrl, ");
            //sqlCommand.Append("c.UserIp AS UserIp, ");
            //sqlCommand.Append("c.CreatedUtc AS CreatedUtc, ");
            //sqlCommand.Append("c.LastModUtc AS LastModUtc, ");
            //sqlCommand.Append("c.ModerationStatus AS ModerationStatus, ");
            //sqlCommand.Append("c.ModeratedBy AS ModeratedBy, ");
            //sqlCommand.Append("c.ModerationReason AS ModerationReason, ");

            sqlCommand.Append("COALESCE(u.Name, c.UserName) AS PostAuthor, ");
            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("COALESCE(u.Email, c.UserEmail) AS AuthorEmail, ");
            sqlCommand.Append("COALESCE(u.TotalRevenue, 0) AS UserRevenue, ");
            sqlCommand.Append("COALESCE(u.Trusted, 0) AS Trusted, ");
            sqlCommand.Append("u.AvatarUrl AS PostAuthorAvatar, ");
            sqlCommand.Append("COALESCE(c.UserUrl, u.WebSiteURL) AS PostAuthorWebSiteUrl ");

            sqlCommand.Append("FROM	mp_Comments c ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
            sqlCommand.Append("ON c.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("c.Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_Comments table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByContentAsc(Guid contentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT  c.*, ");

            //sqlCommand.Append("SELECT ");
            //sqlCommand.Append("c.Guid AS Guid, ");
            //sqlCommand.Append("c.ParentGuid AS ParentGuid, ");
            //sqlCommand.Append("c.SiteGuid AS SiteGuid, ");
            //sqlCommand.Append("c.FeatureGuid AS FeatureGuid, ");
            //sqlCommand.Append("c.ModuleGuid AS ModuleGuid, ");
            //sqlCommand.Append("c.ContentGuid AS ContentGuid, ");
            //sqlCommand.Append("c.UserGuid AS UserGuid, ");
            //sqlCommand.Append("c.Title AS Title, ");
            //sqlCommand.Append("c.UserComment AS UserComment, ");
            //sqlCommand.Append("c.UserName AS UserName ");
            //sqlCommand.Append("c.UserEmail AS UserEmail, ");
            //sqlCommand.Append("c.UserUrl AS UserUrl, ");
            //sqlCommand.Append("c.UserIp AS UserIp, ");
            //sqlCommand.Append("c.CreatedUtc AS CreatedUtc, ");
            //sqlCommand.Append("c.LastModUtc AS LastModUtc, ");
            //sqlCommand.Append("c.ModerationStatus AS ModerationStatus, ");
            //sqlCommand.Append("c.ModeratedBy AS ModeratedBy, ");
            //sqlCommand.Append("c.ModerationReason AS ModerationReason, ");

            sqlCommand.Append("COALESCE(u.Name, c.UserName) AS PostAuthor, ");
            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("COALESCE(u.Email, c.UserEmail) AS AuthorEmail, ");
            sqlCommand.Append("COALESCE(u.TotalRevenue, 0) AS UserRevenue, ");
            sqlCommand.Append("COALESCE(u.Trusted, 0) AS Trusted, ");
            sqlCommand.Append("u.AvatarUrl AS PostAuthorAvatar, ");
            sqlCommand.Append("COALESCE(c.UserUrl, u.WebSiteURL) AS PostAuthorWebSiteUrl ");

            sqlCommand.Append("FROM	mp_Comments c ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
            sqlCommand.Append("ON c.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("c.ContentGuid = :ContentGuid ");
            sqlCommand.Append("ORDER BY c.CreatedUtc ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ContentGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_Comments table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByContentDesc(Guid contentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT  c.*, ");

            //sqlCommand.Append("SELECT ");
            //sqlCommand.Append("c.Guid AS Guid, ");
            //sqlCommand.Append("c.ParentGuid AS ParentGuid, ");
            //sqlCommand.Append("c.SiteGuid AS SiteGuid, ");
            //sqlCommand.Append("c.FeatureGuid AS FeatureGuid, ");
            //sqlCommand.Append("c.ModuleGuid AS ModuleGuid, ");
            //sqlCommand.Append("c.ContentGuid AS ContentGuid, ");
            //sqlCommand.Append("c.UserGuid AS UserGuid, ");
            //sqlCommand.Append("c.Title AS Title, ");
            //sqlCommand.Append("c.UserComment AS UserComment, ");
            //sqlCommand.Append("c.UserName AS UserName ");
            //sqlCommand.Append("c.UserEmail AS UserEmail, ");
            //sqlCommand.Append("c.UserUrl AS UserUrl, ");
            //sqlCommand.Append("c.UserIp AS UserIp, ");
            //sqlCommand.Append("c.CreatedUtc AS CreatedUtc, ");
            //sqlCommand.Append("c.LastModUtc AS LastModUtc, ");
            //sqlCommand.Append("c.ModerationStatus AS ModerationStatus, ");
            //sqlCommand.Append("c.ModeratedBy AS ModeratedBy, ");
            //sqlCommand.Append("c.ModerationReason AS ModerationReason, ");

            sqlCommand.Append("COALESCE(u.Name, c.UserName) AS PostAuthor, ");
            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("COALESCE(u.Email, c.UserEmail) AS AuthorEmail, ");
            sqlCommand.Append("COALESCE(u.TotalRevenue, 0) AS UserRevenue, ");
            sqlCommand.Append("COALESCE(u.Trusted, 0) AS Trusted, ");
            sqlCommand.Append("u.AvatarUrl AS PostAuthorAvatar, ");
            sqlCommand.Append("COALESCE(c.UserUrl, u.WebSiteURL) AS PostAuthorWebSiteUrl ");

            sqlCommand.Append("FROM	mp_Comments c ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
            sqlCommand.Append("ON c.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("c.ContentGuid = :ContentGuid ");
            sqlCommand.Append("ORDER BY c.CreatedUtc DESC ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ContentGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_Comments table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByParentAsc(Guid parentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT  c.*, ");

            //sqlCommand.Append("SELECT ");
            //sqlCommand.Append("c.Guid AS Guid, ");
            //sqlCommand.Append("c.ParentGuid AS ParentGuid, ");
            //sqlCommand.Append("c.SiteGuid AS SiteGuid, ");
            //sqlCommand.Append("c.FeatureGuid AS FeatureGuid, ");
            //sqlCommand.Append("c.ModuleGuid AS ModuleGuid, ");
            //sqlCommand.Append("c.ContentGuid AS ContentGuid, ");
            //sqlCommand.Append("c.UserGuid AS UserGuid, ");
            //sqlCommand.Append("c.Title AS Title, ");
            //sqlCommand.Append("c.UserComment AS UserComment, ");
            //sqlCommand.Append("c.UserName AS UserName ");
            //sqlCommand.Append("c.UserEmail AS UserEmail, ");
            //sqlCommand.Append("c.UserUrl AS UserUrl, ");
            //sqlCommand.Append("c.UserIp AS UserIp, ");
            //sqlCommand.Append("c.CreatedUtc AS CreatedUtc, ");
            //sqlCommand.Append("c.LastModUtc AS LastModUtc, ");
            //sqlCommand.Append("c.ModerationStatus AS ModerationStatus, ");
            //sqlCommand.Append("c.ModeratedBy AS ModeratedBy, ");
            //sqlCommand.Append("c.ModerationReason AS ModerationReason, ");

            sqlCommand.Append("COALESCE(u.Name, c.UserName) AS PostAuthor, ");
            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("COALESCE(u.Email, c.UserEmail) AS AuthorEmail, ");
            sqlCommand.Append("COALESCE(u.TotalRevenue, 0) AS UserRevenue, ");
            sqlCommand.Append("COALESCE(u.Trusted, 0) AS Trusted, ");
            sqlCommand.Append("u.AvatarUrl AS PostAuthorAvatar, ");
            sqlCommand.Append("COALESCE(c.UserUrl, u.WebSiteURL) AS PostAuthorWebSiteUrl ");

            sqlCommand.Append("FROM	mp_Comments c ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
            sqlCommand.Append("ON c.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("c.ParentGuid = :ParentGuid ");
            sqlCommand.Append("ORDER BY c.CreatedUtc ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ParentGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = parentGuid.ToString();

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_Comments table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByParentDesc(Guid parentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT  c.*, ");

            //sqlCommand.Append("SELECT ");
            //sqlCommand.Append("c.Guid AS Guid, ");
            //sqlCommand.Append("c.ParentGuid AS ParentGuid, ");
            //sqlCommand.Append("c.SiteGuid AS SiteGuid, ");
            //sqlCommand.Append("c.FeatureGuid AS FeatureGuid, ");
            //sqlCommand.Append("c.ModuleGuid AS ModuleGuid, ");
            //sqlCommand.Append("c.ContentGuid AS ContentGuid, ");
            //sqlCommand.Append("c.UserGuid AS UserGuid, ");
            //sqlCommand.Append("c.Title AS Title, ");
            //sqlCommand.Append("c.UserComment AS UserComment, ");
            //sqlCommand.Append("c.UserName AS UserName ");
            //sqlCommand.Append("c.UserEmail AS UserEmail, ");
            //sqlCommand.Append("c.UserUrl AS UserUrl, ");
            //sqlCommand.Append("c.UserIp AS UserIp, ");
            //sqlCommand.Append("c.CreatedUtc AS CreatedUtc, ");
            //sqlCommand.Append("c.LastModUtc AS LastModUtc, ");
            //sqlCommand.Append("c.ModerationStatus AS ModerationStatus, ");
            //sqlCommand.Append("c.ModeratedBy AS ModeratedBy, ");
            //sqlCommand.Append("c.ModerationReason AS ModerationReason, ");


            sqlCommand.Append("COALESCE(u.Name, c.UserName) AS PostAuthor, ");
            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("COALESCE(u.Email, c.UserEmail) AS AuthorEmail, ");
            sqlCommand.Append("COALESCE(u.TotalRevenue, 0) AS UserRevenue, ");
            sqlCommand.Append("COALESCE(u.Trusted, 0) AS Trusted, ");
            sqlCommand.Append("u.AvatarUrl AS PostAuthorAvatar, ");
            sqlCommand.Append("COALESCE(c.UserUrl, u.WebSiteURL) AS PostAuthorWebSiteUrl ");

            sqlCommand.Append("FROM	mp_Comments c ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
            sqlCommand.Append("ON c.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("c.ParentGuid = :ParentGuid ");
            sqlCommand.Append("ORDER BY c.CreatedUtc DESC ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ParentGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = parentGuid.ToString();

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the mp_Comments table.
        /// </summary>
        public static int GetCount(Guid contentGuid, int moderationStatus)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Comments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentGuid = :ContentGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ModerationStatus = :ModerationStatus ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ContentGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            arParams[1] = new SqliteParameter(":ModerationStatus", DbType.UInt16);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moderationStatus;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        public static int GetCountByModule(Guid moduleGuid, int moderationStatus)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Comments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = :ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ModerationStatus = :ModerationStatus ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new SqliteParameter(":ModerationStatus", DbType.UInt16);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moderationStatus;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        public static int GetCountBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Comments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append(":SiteGuid = '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("OR ");
            sqlCommand.Append("SiteGuid = :SiteGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }


    
    }
}
