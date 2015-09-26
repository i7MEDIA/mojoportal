// Author:					Joe Audette
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
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Data.SqlServerCe;
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
            sqlCommand.Append("INSERT INTO mp_Comments ");
            sqlCommand.Append("(");
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
            sqlCommand.Append("ModerationReason ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@ParentGuid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@FeatureGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@ContentGuid, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@Title, ");
            sqlCommand.Append("@UserComment, ");
            sqlCommand.Append("@UserName, ");
            sqlCommand.Append("@UserEmail, ");
            sqlCommand.Append("@UserUrl, ");
            sqlCommand.Append("@UserIp, ");
            sqlCommand.Append("@CreatedUtc, ");
            sqlCommand.Append("@LastModUtc, ");
            sqlCommand.Append("@ModerationStatus, ");
            sqlCommand.Append("@ModeratedBy, ");
            sqlCommand.Append("@ModerationReason ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[18];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@ParentGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentGuid;

            arParams[2] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid;

            arParams[3] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = featureGuid;

            arParams[4] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = moduleGuid;

            arParams[5] = new SqlCeParameter("@ContentGuid", SqlDbType.UniqueIdentifier);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = contentGuid;

            arParams[6] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = userGuid;

            arParams[7] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = title;

            arParams[8] = new SqlCeParameter("@UserComment", SqlDbType.NText);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = userComment;

            arParams[9] = new SqlCeParameter("@UserName", SqlDbType.NVarChar, 50);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = userName;

            arParams[10] = new SqlCeParameter("@UserEmail", SqlDbType.NVarChar, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = userEmail;

            arParams[11] = new SqlCeParameter("@UserUrl", SqlDbType.NVarChar, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = userUrl;

            arParams[12] = new SqlCeParameter("@UserIp", SqlDbType.NVarChar, 50);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = userIp;

            arParams[13] = new SqlCeParameter("@CreatedUtc", SqlDbType.DateTime);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = createdUtc;

            arParams[14] = new SqlCeParameter("@LastModUtc", SqlDbType.DateTime);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = createdUtc;

            arParams[15] = new SqlCeParameter("@ModerationStatus", SqlDbType.TinyInt);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = moderationStatus;

            arParams[16] = new SqlCeParameter("@ModeratedBy", SqlDbType.UniqueIdentifier);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = moderatedBy;

            arParams[17] = new SqlCeParameter("@ModerationReason", SqlDbType.NVarChar, 255);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = moderationReason;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
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
            
            sqlCommand.Append("UserGuid = @UserGuid, ");
            sqlCommand.Append("Title = @Title, ");
            sqlCommand.Append("UserComment = @UserComment, ");
            sqlCommand.Append("UserName = @UserName, ");
            sqlCommand.Append("UserEmail = @UserEmail, ");
            sqlCommand.Append("UserUrl = @UserUrl, ");
            sqlCommand.Append("UserIp = @UserIp, ");
            
            sqlCommand.Append("LastModUtc = @LastModUtc, ");
            sqlCommand.Append("ModerationStatus = @ModerationStatus, ");
            sqlCommand.Append("ModeratedBy = @ModeratedBy, ");
            sqlCommand.Append("ModerationReason = @ModerationReason ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[12];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid;

            arParams[2] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new SqlCeParameter("@UserComment", SqlDbType.NVarChar);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userComment;

            arParams[4] = new SqlCeParameter("@UserName", SqlDbType.NVarChar, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userName;

            arParams[5] = new SqlCeParameter("@UserEmail", SqlDbType.NVarChar, 100);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = userEmail;

            arParams[6] = new SqlCeParameter("@UserUrl", SqlDbType.NVarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = userUrl;

            arParams[7] = new SqlCeParameter("@UserIp", SqlDbType.NVarChar, 50);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = userIp;

            arParams[8] = new SqlCeParameter("@LastModUtc", SqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModUtc;

            arParams[9] = new SqlCeParameter("@ModerationStatus", SqlDbType.TinyInt);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = moderationStatus;

            arParams[10] = new SqlCeParameter("@ModeratedBy", SqlDbType.UniqueIdentifier);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = moderatedBy;

            arParams[11] = new SqlCeParameter("@ModerationReason", SqlDbType.NVarChar, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = moderationReason;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

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
            sqlCommand.Append("ContentGuid = @ContentGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ContentGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

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
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
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
            sqlCommand.Append("FeatureGuid = @FeatureGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
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
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
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
            sqlCommand.Append("ParentGuid = @ParentGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ParentGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = parentGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_Comments table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  c.*, ");
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
            sqlCommand.Append("c.Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            return SqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("c.ContentGuid = @ContentGuid ");
            sqlCommand.Append("ORDER BY c.CreatedUtc ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ContentGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid;

            return SqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("c.ContentGuid = @ContentGuid ");
            sqlCommand.Append("ORDER BY c.CreatedUtc DESC ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ContentGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid;

            return SqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("c.ParentGuid = @ParentGuid ");
            sqlCommand.Append("ORDER BY c.CreatedUtc ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ParentGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = parentGuid;

            return SqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("c.ParentGuid = @ParentGuid ");
            sqlCommand.Append("ORDER BY c.CreatedUtc DESC ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ParentGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = parentGuid;

            return SqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("ContentGuid = @ContentGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ModerationStatus = @ModerationStatus ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ContentGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid;

            arParams[1] = new SqlCeParameter("@ModerationStatus", SqlDbType.TinyInt);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moderationStatus;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));
        }

        public static int GetCountByModule(Guid moduleGuid, int moderationStatus)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Comments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ModerationStatus = @ModerationStatus ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            arParams[1] = new SqlCeParameter("@ModerationStatus", SqlDbType.TinyInt);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moderationStatus;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));
        }

        public static int GetCountBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Comments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("@SiteGuid = '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("OR ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

           
            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));
        }

        
    }
}
