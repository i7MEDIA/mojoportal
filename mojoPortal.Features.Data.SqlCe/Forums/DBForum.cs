// Author:					Joe Audette
// Created:				    2010-07-02
// Last Modified:			2014-06-25
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 
// Note moved into separate class file from dbPortal 2007-11-03

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Data.SqlServerCe;
using mojoPortal.Data;
//using log4net;

namespace mojoPortal.Data
{
    public static class DBForums
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(DBForums));

        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }


        public static int Create(
            Guid forumGuid,
            int moduleId,
            int userId,
            string title,
            string description,
            bool isModerated,
            bool isActive,
            int sortOrder,
            int postsPerPage,
            int threadsPerPage,
            bool allowAnonymousPosts,

            string rolesThatCanPost,
            string rolesThatCanModerate,
            string moderatorNotifyEmail,
            bool includeInGoogleMap,
            bool addNoIndexMeta,
            bool closed,
            bool visible,
            bool requireModeration,
            bool requireModForNotify,
            bool allowTrustedDirectPosts,
            bool allowTrustedDirectNotify
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Forums ");
            sqlCommand.Append("(");
            sqlCommand.Append("ForumGuid, ");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("CreatedDate, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("IsModerated, ");
            sqlCommand.Append("IsActive, ");
            sqlCommand.Append("SortOrder, ");
            sqlCommand.Append("ThreadCount, ");
            sqlCommand.Append("PostCount, ");
            //sqlCommand.Append("MostRecentPostDate, ");
            sqlCommand.Append("MostRecentPostUserID, ");
            sqlCommand.Append("PostsPerPage, ");
            sqlCommand.Append("ThreadsPerPage, ");

            sqlCommand.Append("RolesThatCanPost, ");
            sqlCommand.Append("RolesThatCanModerate, ");
            sqlCommand.Append("ModeratorNotifyEmail, ");
            sqlCommand.Append("IncludeInGoogleMap, ");
            sqlCommand.Append("AddNoIndexMeta, ");
            sqlCommand.Append("Closed, ");
            sqlCommand.Append("Visible, ");
            sqlCommand.Append("RequireModeration, ");
            sqlCommand.Append("RequireModForNotify, ");
            sqlCommand.Append("AllowTrustedDirectPosts, ");
            sqlCommand.Append("AllowTrustedDirectNotify, ");


            sqlCommand.Append("AllowAnonymousPosts ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ForumGuid, ");
            sqlCommand.Append("@ModuleID, ");
            sqlCommand.Append("@CreatedDate, ");
            sqlCommand.Append("@CreatedBy, ");
            sqlCommand.Append("@Title, ");
            sqlCommand.Append("@Description, ");
            sqlCommand.Append("@IsModerated, ");
            sqlCommand.Append("@IsActive, ");
            sqlCommand.Append("@SortOrder, ");
            sqlCommand.Append("@ThreadCount, ");
            sqlCommand.Append("@PostCount, ");
           // sqlCommand.Append("@MostRecentPostDate, ");
            sqlCommand.Append("@MostRecentPostUserID, ");
            sqlCommand.Append("@PostsPerPage, ");
            sqlCommand.Append("@ThreadsPerPage, ");

            sqlCommand.Append("@RolesThatCanPost, ");
            sqlCommand.Append("@RolesThatCanModerate, ");
            sqlCommand.Append("@ModeratorNotifyEmail, ");
            sqlCommand.Append("@IncludeInGoogleMap, ");
            sqlCommand.Append("@AddNoIndexMeta, ");
            sqlCommand.Append("@Closed, ");
            sqlCommand.Append("@Visible, ");
            sqlCommand.Append("@RequireModeration, ");
            sqlCommand.Append("@RequireModForNotify, ");
            sqlCommand.Append("@AllowTrustedDirectPosts, ");
            sqlCommand.Append("@AllowTrustedDirectNotify, ");

            sqlCommand.Append("@AllowAnonymousPosts ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[26];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@CreatedDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            arParams[2] = new SqlCeParameter("@CreatedBy", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userId;

            arParams[3] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = title;

            arParams[4] = new SqlCeParameter("@Description", SqlDbType.NText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = description;

            arParams[5] = new SqlCeParameter("@IsModerated", SqlDbType.Bit);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = isModerated;

            arParams[6] = new SqlCeParameter("@IsActive", SqlDbType.Bit);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = isActive;

            arParams[7] = new SqlCeParameter("@SortOrder", SqlDbType.Int);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = sortOrder;

            arParams[8] = new SqlCeParameter("@ThreadCount", SqlDbType.Int);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = 0;

            arParams[9] = new SqlCeParameter("@PostCount", SqlDbType.Int);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = 0;

            arParams[10] = new SqlCeParameter("@MostRecentPostUserID", SqlDbType.Int);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = -1;

            arParams[11] = new SqlCeParameter("@PostsPerPage", SqlDbType.Int);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = postsPerPage;

            arParams[12] = new SqlCeParameter("@ThreadsPerPage", SqlDbType.Int);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = threadsPerPage;

            arParams[13] = new SqlCeParameter("@AllowAnonymousPosts", SqlDbType.Bit);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = allowAnonymousPosts;

            arParams[14] = new SqlCeParameter("@ForumGuid", SqlDbType.UniqueIdentifier);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = forumGuid;

            arParams[15] = new SqlCeParameter("@RolesThatCanPost", SqlDbType.NText);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = rolesThatCanPost;

            arParams[16] = new SqlCeParameter("@RolesThatCanModerate", SqlDbType.NText);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = rolesThatCanModerate;

            arParams[17] = new SqlCeParameter("@ModeratorNotifyEmail", SqlDbType.NText);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = moderatorNotifyEmail;

            arParams[18] = new SqlCeParameter("@IncludeInGoogleMap", SqlDbType.Bit);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = includeInGoogleMap;

            arParams[19] = new SqlCeParameter("@AddNoIndexMeta", SqlDbType.Bit);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = addNoIndexMeta;

            arParams[20] = new SqlCeParameter("@Closed", SqlDbType.Bit);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = closed;

            arParams[21] = new SqlCeParameter("@Visible", SqlDbType.Bit);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = visible;

            arParams[22] = new SqlCeParameter("@RequireModeration", SqlDbType.Bit);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = requireModeration;

            arParams[23] = new SqlCeParameter("@RequireModForNotify", SqlDbType.Bit);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = requireModForNotify;

            arParams[24] = new SqlCeParameter("@AllowTrustedDirectPosts", SqlDbType.Bit);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = allowTrustedDirectPosts;

            arParams[25] = new SqlCeParameter("@AllowTrustedDirectNotify", SqlDbType.Bit);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = allowTrustedDirectPosts;




            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;

        }

        public static bool Update(
            int itemId,
            int userId,
            string title,
            string description,
            bool isModerated,
            bool isActive,
            int sortOrder,
            int postsPerPage,
            int threadsPerPage,
            bool allowAnonymousPosts,

            string rolesThatCanPost,
            string rolesThatCanModerate,
            string moderatorNotifyEmail,
            bool includeInGoogleMap,
            bool addNoIndexMeta,
            bool closed,
            bool visible,
            bool requireModeration,
            bool requireModForNotify,
            bool allowTrustedDirectPosts,
            bool allowTrustedDirectNotify
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Forums ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("Title = @Title, ");
            sqlCommand.Append("Description = @Description, ");
            sqlCommand.Append("IsModerated = @IsModerated, ");
            sqlCommand.Append("IsActive = @IsActive, ");
            sqlCommand.Append("SortOrder = @SortOrder, ");
           
            sqlCommand.Append("PostsPerPage = @PostsPerPage, ");
            sqlCommand.Append("ThreadsPerPage = @ThreadsPerPage, ");

            sqlCommand.Append("RolesThatCanPost = @RolesThatCanPost, ");
            sqlCommand.Append("RolesThatCanModerate = @RolesThatCanModerate, ");
            sqlCommand.Append("ModeratorNotifyEmail = @ModeratorNotifyEmail, ");
            sqlCommand.Append("IncludeInGoogleMap = @IncludeInGoogleMap, ");
            sqlCommand.Append("AddNoIndexMeta = @AddNoIndexMeta, ");
            sqlCommand.Append("Closed = @Closed, ");
            sqlCommand.Append("Visible = @Visible, ");
            sqlCommand.Append("RequireModeration = @RequireModeration, ");
            sqlCommand.Append("RequireModForNotify = @RequireModForNotify, ");
            sqlCommand.Append("AllowTrustedDirectPosts = @AllowTrustedDirectPosts, ");
            sqlCommand.Append("AllowTrustedDirectNotify = @AllowTrustedDirectNotify, ");


            sqlCommand.Append("AllowAnonymousPosts = @AllowAnonymousPosts ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ItemID = @ItemID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[20];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new SqlCeParameter("@Description", SqlDbType.NVarChar);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = description;

            arParams[3] = new SqlCeParameter("@IsModerated", SqlDbType.Bit);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = isModerated;

            arParams[4] = new SqlCeParameter("@IsActive", SqlDbType.Bit);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = isActive;

            arParams[5] = new SqlCeParameter("@SortOrder", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sortOrder;

            arParams[6] = new SqlCeParameter("@PostsPerPage", SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = postsPerPage;

            arParams[7] = new SqlCeParameter("@ThreadsPerPage", SqlDbType.Int);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = threadsPerPage;

            arParams[8] = new SqlCeParameter("@AllowAnonymousPosts", SqlDbType.Bit);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = allowAnonymousPosts;

            arParams[9] = new SqlCeParameter("@RolesThatCanPost", SqlDbType.NText);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = rolesThatCanPost;

            arParams[10] = new SqlCeParameter("@RolesThatCanModerate", SqlDbType.NText);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = rolesThatCanModerate;

            arParams[11] = new SqlCeParameter("@ModeratorNotifyEmail", SqlDbType.NText);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = moderatorNotifyEmail;

            arParams[12] = new SqlCeParameter("@IncludeInGoogleMap", SqlDbType.Bit);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = includeInGoogleMap;

            arParams[13] = new SqlCeParameter("@AddNoIndexMeta", SqlDbType.Bit);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = addNoIndexMeta;

            arParams[14] = new SqlCeParameter("@Closed", SqlDbType.Bit);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = closed;

            arParams[15] = new SqlCeParameter("@Visible", SqlDbType.Bit);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = visible;

            arParams[16] = new SqlCeParameter("@RequireModeration", SqlDbType.Bit);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = requireModeration;

            arParams[17] = new SqlCeParameter("@RequireModForNotify", SqlDbType.Bit);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = requireModForNotify;

            arParams[18] = new SqlCeParameter("@AllowTrustedDirectPosts", SqlDbType.Bit);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = allowTrustedDirectPosts;

            arParams[19] = new SqlCeParameter("@AllowTrustedDirectNotify", SqlDbType.Bit);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = allowTrustedDirectPosts;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumPosts WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads WHERE ForumID = @ItemID );");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumThreadSubscriptions WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads WHERE ForumID = @ItemID );");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumThreads WHERE ForumID = @ItemID;");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumSubscriptions WHERE ForumID = @ItemID ;");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Forums ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = @ItemID ");
            sqlCommand.Append(";");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumPosts WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID = @ModuleID) );");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumThreadSubscriptions WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID = @ModuleID) );");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID = @ModuleID);");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumSubscriptions WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID = @ModuleID) ;");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Forums WHERE ModuleID = @ModuleID;");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumPosts WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID IN  (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID)) );");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumThreadSubscriptions WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID IN  (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID)) );");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID IN  (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID));");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumSubscriptions WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID IN  (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID)) ;");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Forums WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID);");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetForums(int moduleId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT f.*, ");
            sqlCommand.Append("u.Name As MostRecentPostUser, ");
            sqlCommand.Append("CASE WHEN s.[SubscribeDate] IS NOT NULL and s.[UnSubscribeDate] IS NULL THEN 1 ");
            sqlCommand.Append("Else 0 End AS Subscribed,");


            sqlCommand.Append("COALESCE(s2.SubscriberCount, 0) AS SubscriberCount  ");
            //sqlCommand.Append("(SELECT COUNT(*) FROM mp_ForumSubscriptions fs WHERE fs.ForumID = f.ItemID AND fs.UnSubscribeDate IS NULL) As SubscriberCount  ");

            sqlCommand.Append("FROM	mp_Forums f ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON f.MostRecentPostUserID = u.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_ForumSubscriptions s ");
            sqlCommand.Append("ON f.ItemID = s.ForumID AND s.UserID = @UserID ");

            sqlCommand.Append("LEFT OUTER JOIN ( ");
            sqlCommand.Append("SELECT ForumID, Count(*) As SubscriberCount ");
            sqlCommand.Append("FROM mp_ForumSubscriptions ");
            sqlCommand.Append("GROUP BY ForumID ");
            sqlCommand.Append(") s2  ");
            sqlCommand.Append("ON s2.ForumID = f.ItemID ");

            sqlCommand.Append("WHERE f.ModuleID	= @ModuleID ");
            sqlCommand.Append("AND f.IsActive = 1 ");

            sqlCommand.Append("ORDER BY	f.SortOrder, f.ItemID ; ");

            //log.Info(sqlCommand.ToString());

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetForum(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT f.*, ");
            sqlCommand.Append("u.Name As CreatedByUser, ");
            sqlCommand.Append("up.Name As MostRecentPostUser ");
            sqlCommand.Append("FROM	mp_Forums f ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON f.CreatedBy = u.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_Users up ");
            sqlCommand.Append("ON f.MostRecentPostUserID = up.UserID ");
            sqlCommand.Append("WHERE f.ItemID	= @ItemID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static bool IncrementThreadCount(int forumId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE	mp_Forums ");
            sqlCommand.Append("SET	ThreadCount = ThreadCount + 1 ");
            sqlCommand.Append("WHERE ItemID = @ItemID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DecrementThreadCount(int forumId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Forums ");
            sqlCommand.Append("SET ThreadCount = ThreadCount - 1 ");

            sqlCommand.Append("WHERE ItemID = @ItemID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static int GetUserThreadCount(int userId, int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");

            sqlCommand.Append("FROM	mp_ForumThreads ft ");

            sqlCommand.Append("JOIN mp_Forums f ");
            sqlCommand.Append("ON ft.ForumID = f.ItemID ");

            sqlCommand.Append("JOIN mp_Modules m ");
            sqlCommand.Append("ON f.ModuleID = m.ModuleID ");

            sqlCommand.Append("WHERE m.SiteID = @SiteID AND ft.ThreadID IN (Select DISTINCT ThreadID FROM mp_ForumPosts WHERE mp_ForumPosts.UserID = @UserID) ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteId;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public static IDataReader GetThreadPageByUser(
            int userId,
            int siteId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetUserThreadCount(userId, siteId);

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

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	 ");
            sqlCommand.Append(" t.*, ");
            sqlCommand.Append("f.Title As Forum, ");
            sqlCommand.Append("f.ModuleID, ");
            sqlCommand.Append("pm.PageID , ");
            //sqlCommand.Append("(SELECT TOP(1) PageID FROM mp_PageModules WHERE mp_PageModules.ModuleID = f.ModuleID AND (PublishEndDate IS NULL OR PublishEndDate > @CurrentDate)) As PageID, ");
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("s.Name As StartedBy ");

            sqlCommand.Append("FROM	mp_ForumThreads t ");

            sqlCommand.Append("JOIN	mp_Forums f ");
            sqlCommand.Append("ON t.ForumID = f.ItemID ");

            sqlCommand.Append("JOIN mp_Modules m ");
            sqlCommand.Append("ON f.ModuleID = m.ModuleID ");

            sqlCommand.Append("LEFT OUTER JOIN mp_PageModules pm ");
            sqlCommand.Append("ON pm.ModuleID = f.ModuleID ");
            sqlCommand.Append("AND pm.PageID IN (");
            sqlCommand.Append("SELECT TOP(1) PageID FROM mp_PageModules WHERE ModuleID = f.ModuleID AND (PublishEndDate IS NULL OR PublishEndDate > @CurrentDate) ");

            sqlCommand.Append(") ");



            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON t.MostRecentPostUserID = u.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users s ");
            sqlCommand.Append("ON t.StartedByUserID = s.UserID ");

            sqlCommand.Append("WHERE m.SiteID = @SiteID AND t.ThreadID IN (Select DISTINCT ThreadID FROM mp_ForumPosts WHERE mp_ForumPosts.UserID = @UserID) ");

            sqlCommand.Append("ORDER BY	t.MostRecentPostDate DESC  ");

            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

            sqlCommand.Append(";");

            //log.Info(sqlCommand.ToString());

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new SqlCeParameter("@CurrentDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            arParams[2] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static bool UpdateUserStats(int userId)
        {
            if (userId > -1) //updating a single user
            {
                int postCount = UserGetUserPostCount(userId);
                return UpdateSingleUserStats(userId, postCount);

            }


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("UserID,  ");
            sqlCommand.Append("COUNT(*) As Count ");
            sqlCommand.Append("FROM mp_ForumPosts  ");
            sqlCommand.Append("GROUP BY UserID ");
            sqlCommand.Append(";");

            DataTable dataTable = new DataTable(); ;

            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null))
            {
                dataTable.Load(reader);
            }

            bool didUpdate = false;
           
            foreach (DataRow row in dataTable.Rows)
            {
                int uId = Convert.ToInt32(row[0]);
                int postCount = Convert.ToInt32(row[1]);
                UpdateSingleUserStats(uId, postCount);
                didUpdate = true;
            }

            return didUpdate;

        }

        private static bool UpdateSingleUserStats(int userId, int postCount)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("TotalPosts = @PostCount ");
            sqlCommand.Append("WHERE UserID = @UserID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new SqlCeParameter("@PostCount", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = postCount;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        private static int UserGetUserPostCount(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM mp_ForumPosts ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserID = @UserID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public static bool IncrementPostCount(
                int forumId,
                int mostRecentPostUserId,
                DateTime mostRecentPostDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Forums ");
            sqlCommand.Append("SET MostRecentPostDate = @MostRecentPostDate, ");
            sqlCommand.Append("MostRecentPostUserID = @MostRecentPostUserID, ");
            sqlCommand.Append("PostCount = PostCount + 1 ");

            sqlCommand.Append("WHERE ItemID = @ItemID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqlCeParameter("@MostRecentPostUserID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostUserId;

            arParams[2] = new SqlCeParameter("@MostRecentPostDate", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = mostRecentPostDate;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static bool IncrementPostCount(int forumId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Forums ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("PostCount = PostCount + 1 ");
            sqlCommand.Append("WHERE ItemID = @ItemID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static bool DecrementPostCount(int forumId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Forums ");
            sqlCommand.Append("SET PostCount = PostCount - 1 ");
            sqlCommand.Append("WHERE ItemID = @ItemID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static bool RecalculatePostStats(int forumId)
        {
            DateTime mostRecentPostDate = DateTime.UtcNow;
            int mostRecentPostUserID = 0;
            int postCount = 0;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP (1) ");
            sqlCommand.Append("MostRecentPostDate, ");
            sqlCommand.Append("MostRecentPostUserID ");
            sqlCommand.Append("FROM mp_ForumThreads ");
            sqlCommand.Append("WHERE ForumID = @ItemID ");
            sqlCommand.Append("ORDER BY MostRecentPostDate DESC ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    mostRecentPostUserID = Convert.ToInt32(reader["MostRecentPostUserID"]);
                    mostRecentPostDate = Convert.ToDateTime(reader["MostRecentPostDate"]);
                }

            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("COALESCE(SUM(TotalReplies) + COUNT(*),0) As PostCount ");
            sqlCommand.Append("FROM mp_ForumThreads ");
            sqlCommand.Append("WHERE ForumID = @ItemID ;");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            using (IDataReader reader = SqlHelper.ExecuteReader(
               GetConnectionString(),
               CommandType.Text,
               sqlCommand.ToString(),
               arParams))
            {
                if (reader.Read())
                {
                    postCount = Convert.ToInt32(reader["PostCount"]);
                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE	mp_Forums ");
            sqlCommand.Append("SET	 ");
            sqlCommand.Append("MostRecentPostDate = @MostRecentPostDate,	 ");
            sqlCommand.Append("MostRecentPostUserID = @MostRecentPostUserID,	 ");
            sqlCommand.Append("PostCount = @PostCount	 ");
            sqlCommand.Append("WHERE ItemID = @ItemID ;");

            arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqlCeParameter("@MostRecentPostUserID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostUserID;

            arParams[2] = new SqlCeParameter("@PostCount", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = postCount;

            arParams[3] = new SqlCeParameter("@MostRecentPostDate", SqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = mostRecentPostDate;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static int GetSubscriberCount(int forumId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_ForumSubscriptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ForumID = @ItemID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("UnSubscribeDate IS NULL");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public static IDataReader GetSubscriberPage(
            int forumId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetSubscriberCount(forumId);

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

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("fs.SubscriptionID, ");
            sqlCommand.Append("fs.SubscribeDate, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("FROM	mp_ForumSubscriptions fs  ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.UserID = fs.UserID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("fs.ForumID = @ItemID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("fs.UnSubscribeDate IS NULL ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("u.Name  ");

            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static bool AddSubscriber(int forumId, int userId, Guid subGuid)
        {
            

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT COUNT(*) As SubscriptionCount ");
            sqlCommand.Append("FROM mp_ForumSubscriptions  ");
            sqlCommand.Append("WHERE ForumID = @ItemID AND UserID = @UserID  AND UnSubscribeDate IS NULL; ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int subscriptionCount = 0;
            int rowsAffected = -1;

            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    subscriptionCount = Convert.ToInt32(reader["SubscriptionCount"]);
                }

            }

            
            if (subscriptionCount > 0)
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("UPDATE mp_ForumSubscriptions ");
                sqlCommand.Append("SET UnSubscribeDate = @SubscribeDate ");

                sqlCommand.Append("WHERE ForumID = @ItemID AND UserID = @UserID AND UnSubscribeDate IS NULL ;");

                arParams = new SqlCeParameter[3];

                arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = forumId;

                arParams[1] = new SqlCeParameter("@UserID", SqlDbType.Int);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = userId;

                arParams[2] = new SqlCeParameter("@SubscribeDate", SqlDbType.DateTime);
                arParams[2].Direction = ParameterDirection.Input;
                arParams[2].Value = DateTime.UtcNow;

                rowsAffected = SqlHelper.ExecuteNonQuery(
                    GetConnectionString(),
                    CommandType.Text,
                    sqlCommand.ToString(),
                    arParams);

            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO	mp_ForumSubscriptions ( ");
            sqlCommand.Append("ForumID, ");
            sqlCommand.Append("UserID, ");
            sqlCommand.Append("SubGuid, ");
            sqlCommand.Append("SubscribeDate");
            sqlCommand.Append(") ");
            sqlCommand.Append("VALUES ( ");
            sqlCommand.Append("@ItemID, ");
            sqlCommand.Append("@UserID, ");
            sqlCommand.Append("@SubGuid, ");
            sqlCommand.Append("@SubscribeDate");
            sqlCommand.Append(") ;");

            
            arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new SqlCeParameter("@SubscribeDate", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            arParams[3] = new SqlCeParameter("@SubGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = subGuid;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetForumSubscription(Guid subGuid)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_ForumSubscriptions ");

            sqlCommand.Append("WHERE SubGuid = @SubGuid ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SubGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static bool DeleteSubscription(int subscriptionId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumSubscriptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SubscriptionID = @SubscriptionID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SubscriptionID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subscriptionId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Unsubscribe(Guid subGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_ForumSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = @UnSubscribeDate ");
            sqlCommand.Append("WHERE SubGuid = @SubGuid ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SubGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subGuid;

            arParams[1] = new SqlCeParameter("@UnSubscribeDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);



        }

        public static bool Unsubscribe(int forumId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_ForumSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = @UnSubscribeDate ");
            sqlCommand.Append("WHERE ForumID = @ItemID AND UserID = @UserID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new SqlCeParameter("@UnSubscribeDate", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UnsubscribeAll(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_ForumSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = @UnSubscribeDate ");
            sqlCommand.Append("WHERE UserID = @UserID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new SqlCeParameter("@UnSubscribeDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumSubscriptionExists(int forumId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_ForumSubscriptions ");
            sqlCommand.Append("WHERE ForumID = @ItemID AND UserID = @UserID AND UnSubscribeDate IS NULL ; ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public static bool ForumThreadSubscriptionExists(int threadId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_ForumThreadSubscriptions ");
            sqlCommand.Append("WHERE ThreadID = @ThreadID AND UserID = @UserID AND UnSubscribeDate IS NULL ; ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public static IDataReader GetThreadsForSiteMap(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("ft.ThreadID, ");
            sqlCommand.Append("ft.MostRecentPostDate, ");
            sqlCommand.Append("f.ModuleID, ");
            sqlCommand.Append("m.ViewRoles, ");
            sqlCommand.Append("p.PageID, ");
            sqlCommand.Append("p.AuthorizedRoles ");


            sqlCommand.Append("FROM	mp_ForumThreads ft ");

            sqlCommand.Append("JOIN	mp_Forums f ");
            sqlCommand.Append("ON f.ItemID = ft.ForumID ");

            sqlCommand.Append("JOIN	mp_Modules m ");
            sqlCommand.Append("ON f.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN	mp_PageModules pm ");
            sqlCommand.Append("ON pm.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN	mp_Pages p ");
            sqlCommand.Append("ON p.PageID = pm.PageID ");


            sqlCommand.Append("WHERE p.SiteID = @SiteID ");
            sqlCommand.Append("AND IncludeInSiteMap = 1 ");

            sqlCommand.Append("ORDER BY ft.ThreadID DESC ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetThreads(int forumId, int pageNumber)
        {
            int pageSize = 1;
            int totalRows = 0;
            using (IDataReader reader = GetForum(forumId))
            {
                if (reader.Read())
                {
                    pageSize = Convert.ToInt32(reader["ThreadsPerPage"]);
                    totalRows = Convert.ToInt32(reader["ThreadCount"]);
                }
            }

            int totalPages = totalRows / pageSize;
            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder = 0;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	 ");
            sqlCommand.Append(" t.*, ");
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("s.Name As StartedBy ");
            sqlCommand.Append("FROM	mp_ForumThreads t ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON t.MostRecentPostUserID = u.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_Users s ");
            sqlCommand.Append("ON t.StartedByUserID = s.UserID ");
            sqlCommand.Append("WHERE t.ForumID = @ItemID ");

            sqlCommand.Append("ORDER BY t.SortOrder, t.MostRecentPostDate DESC  ");

            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

            sqlCommand.Append(" ; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetThread(int threadId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	t.*, ");
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
            sqlCommand.Append("f.PostsPerPage, ");
            sqlCommand.Append("f.ModuleID ");

            sqlCommand.Append("FROM	mp_ForumThreads t ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON t.MostRecentPostUserID = u.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_Users s ");
            sqlCommand.Append("ON t.StartedByUserID = s.UserID ");
            sqlCommand.Append("JOIN	mp_Forums f ");
            sqlCommand.Append("ON f.ItemID = t.ForumID ");
            sqlCommand.Append("WHERE t.ThreadID = @ThreadID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetPost(int postId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	fp.* ");
            sqlCommand.Append("FROM	mp_ForumPosts fp ");
            sqlCommand.Append("WHERE fp.PostID = @PostID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PostID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static int ForumThreadGetPostCount(int threadId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_ForumPosts WHERE ThreadID = @ThreadID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));
        }

        public static int ForumThreadCreate(
            int forumId,
            string threadSubject,
            int sortOrder,
            bool isLocked,
            int startedByUserId,
            DateTime threadDate,
            Guid threadGuid,
            bool isQuestion,
            bool includeInSiteMap,
            bool setNoIndexMeta,
            string pageTitleOverride,
            int modStatus,
            string threadType)
        {
            int forumSequence = 1;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COALESCE(Max(ForumSequence) + 1,1) As ForumSequence FROM mp_ForumThreads WHERE ForumID = @ForumID ; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ForumID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    forumSequence = Convert.ToInt32(reader["ForumSequence"]);
                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_ForumThreads ");
            sqlCommand.Append("(");
            sqlCommand.Append("ForumID, ");
            sqlCommand.Append("ThreadSubject, ");
            sqlCommand.Append("ThreadDate, ");
            sqlCommand.Append("TotalViews, ");
            sqlCommand.Append("TotalReplies, ");
            sqlCommand.Append("SortOrder, ");
            sqlCommand.Append("IsLocked, ");
            sqlCommand.Append("ForumSequence, ");
            sqlCommand.Append("MostRecentPostDate, ");
            sqlCommand.Append("MostRecentPostUserID, ");
            sqlCommand.Append("ThreadGuid, ");
            sqlCommand.Append("IsQuestion, ");
            sqlCommand.Append("IncludeInSiteMap, ");
            sqlCommand.Append("SetNoIndexMeta, ");
            sqlCommand.Append("PTitleOverride, ");

            sqlCommand.Append("ModStatus, ");
            sqlCommand.Append("ThreadType, ");

            sqlCommand.Append("StartedByUserID ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ForumID, ");
            sqlCommand.Append("@ThreadSubject, ");
            sqlCommand.Append("@ThreadDate, ");
            sqlCommand.Append("@TotalViews, ");
            sqlCommand.Append("@TotalReplies, ");
            sqlCommand.Append("@SortOrder, ");
            sqlCommand.Append("@IsLocked, ");
            sqlCommand.Append("@ForumSequence, ");
            sqlCommand.Append("@MostRecentPostDate, ");
            sqlCommand.Append("@MostRecentPostUserID, ");

            sqlCommand.Append("@ThreadGuid, ");
            sqlCommand.Append("@IsQuestion, ");
            sqlCommand.Append("@IncludeInSiteMap, ");
            sqlCommand.Append("@SetNoIndexMeta, ");
            sqlCommand.Append("@PTitleOverride, ");

            sqlCommand.Append("@ModStatus, ");
            sqlCommand.Append("@ThreadType, ");

            sqlCommand.Append("@StartedByUserID ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            arParams = new SqlCeParameter[18];

            arParams[0] = new SqlCeParameter("@ForumID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqlCeParameter("@ThreadSubject", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadSubject;

            arParams[2] = new SqlCeParameter("@ThreadDate", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = threadDate;

            arParams[3] = new SqlCeParameter("@TotalViews", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = 0;

            arParams[4] = new SqlCeParameter("@TotalReplies", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = 0;

            arParams[5] = new SqlCeParameter("@SortOrder", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sortOrder;

            arParams[6] = new SqlCeParameter("@IsLocked", SqlDbType.Bit);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = isLocked;

            arParams[7] = new SqlCeParameter("@ForumSequence", SqlDbType.Int);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = forumSequence;

            arParams[8] = new SqlCeParameter("@MostRecentPostDate", SqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = threadDate;

            arParams[9] = new SqlCeParameter("@MostRecentPostUserID", SqlDbType.Int);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = startedByUserId;

            arParams[10] = new SqlCeParameter("@StartedByUserID", SqlDbType.Int);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = startedByUserId;

            arParams[11] = new SqlCeParameter("@ThreadGuid", SqlDbType.UniqueIdentifier);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = threadGuid;

            arParams[12] = new SqlCeParameter("@IsQuestion", SqlDbType.Bit);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = isQuestion;

            arParams[13] = new SqlCeParameter("@IncludeInSiteMap", SqlDbType.Bit);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = includeInSiteMap;

            arParams[14] = new SqlCeParameter("@SetNoIndexMeta", SqlDbType.Bit);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = setNoIndexMeta;

            arParams[15] = new SqlCeParameter("@PTitleOverride", SqlDbType.NVarChar, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = pageTitleOverride;

            arParams[16] = new SqlCeParameter("@ModStatus", SqlDbType.Int);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = modStatus;

            arParams[17] = new SqlCeParameter("@ThreadType", SqlDbType.NVarChar, 100);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = threadType;


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

           

            return newId;

        }

        public static bool ForumThreadDelete(int threadId)
        {
            ForumThreadDeletePosts(threadId);
            ForumThreadDeleteSubscriptions(threadId);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumThreads ");
            sqlCommand.Append("WHERE ThreadID = @ThreadID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadDeletePosts(int threadId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumPosts ");
            sqlCommand.Append("WHERE ThreadID = @ThreadID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadDeleteSubscriptions(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumThreadSubscriptions ");
            sqlCommand.Append("WHERE ThreadID = @ThreadID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadUpdate(
            int threadId,
            int forumId,
            string threadSubject,
            int sortOrder,
            bool isLocked,
            bool isQuestion,
            bool includeInSiteMap,
            bool setNoIndexMeta,
            string pageTitleOverride,
            int modStatus,
            string threadType,
            Guid assignedTo,
            Guid lockedBy,
            string lockedReason,
            DateTime lockedUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_ForumThreads ");
            sqlCommand.Append("SET	ForumID = @ForumID, ");
            sqlCommand.Append("ThreadSubject = @ThreadSubject, ");
            sqlCommand.Append("SortOrder = @SortOrder, ");

            sqlCommand.Append("IsQuestion = @IsQuestion, ");
            sqlCommand.Append("IncludeInSiteMap = @IncludeInSiteMap, ");
            sqlCommand.Append("SetNoIndexMeta = @SetNoIndexMeta, ");
            sqlCommand.Append("PTitleOverride = @PTitleOverride, ");

            sqlCommand.Append("ModStatus = @ModStatus, ");
            sqlCommand.Append("ThreadType = @ThreadType, ");
            sqlCommand.Append("AssignedTo = @AssignedTo, ");
            sqlCommand.Append("LockedBy = @LockedBy, ");
            sqlCommand.Append("LockedReason = @LockedReason, ");
            sqlCommand.Append("LockedUtc = @LockedUtc, ");

            sqlCommand.Append("IsLocked = @IsLocked ");
            sqlCommand.Append("WHERE ThreadID = @ThreadID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[15];

            arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqlCeParameter("@ForumID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = forumId;

            arParams[2] = new SqlCeParameter("@ThreadSubject", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = threadSubject;

            arParams[3] = new SqlCeParameter("@SortOrder", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortOrder;

            arParams[4] = new SqlCeParameter("@IsLocked", SqlDbType.Bit);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = isLocked;

            arParams[5] = new SqlCeParameter("@IsQuestion", SqlDbType.Bit);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = isQuestion;

            arParams[6] = new SqlCeParameter("@IncludeInSiteMap", SqlDbType.Bit);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = includeInSiteMap;

            arParams[7] = new SqlCeParameter("@SetNoIndexMeta", SqlDbType.Bit);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = setNoIndexMeta;

            arParams[8] = new SqlCeParameter("@PTitleOverride", SqlDbType.NVarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = pageTitleOverride;

            arParams[9] = new SqlCeParameter("@ModStatus", SqlDbType.Int);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = modStatus;

            arParams[10] = new SqlCeParameter("@ThreadType", SqlDbType.NVarChar, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = threadType;

            arParams[11] = new SqlCeParameter("@AssignedTo", SqlDbType.UniqueIdentifier);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = assignedTo;

            arParams[12] = new SqlCeParameter("@LockedBy", SqlDbType.UniqueIdentifier);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = lockedBy;

            arParams[13] = new SqlCeParameter("@LockedReason", SqlDbType.NVarChar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = lockedReason;

            arParams[14] = new SqlCeParameter("@LockedUtc", SqlDbType.DateTime);
            arParams[14].Direction = ParameterDirection.Input;
            

            if (lockedUtc < DateTime.MaxValue)
            {
                arParams[14].Value = lockedUtc;
            }
            else
            {
                arParams[14].Value = DBNull.Value;
            }


            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadIncrementReplyStats(
          int threadId,
          int mostRecentPostUserId,
          DateTime mostRecentPostDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_ForumThreads ");
            sqlCommand.Append("SET MostRecentPostUserID = @MostRecentPostUserID, ");
            sqlCommand.Append("TotalReplies = TotalReplies + 1, ");
            sqlCommand.Append("MostRecentPostDate = @MostRecentPostDate ");
            sqlCommand.Append("WHERE ThreadID = @ThreadID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqlCeParameter("@MostRecentPostDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostDate;

            arParams[2] = new SqlCeParameter("@MostRecentPostUserID", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = mostRecentPostUserId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadDecrementReplyStats(int threadId)
        {
            int userId = -1;
            DateTime postDate = DateTime.Now;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT UserID, PostDate ");
            sqlCommand.Append("FROM mp_ForumPosts ");
            sqlCommand.Append("WHERE ThreadID = @ThreadID ");
            sqlCommand.Append("ORDER BY PostID DESC ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    if (reader["UserID"] != DBNull.Value)
                    {
                        userId = Convert.ToInt32(reader["UserID"]);
                    }

                    if (reader["PostDate"] != DBNull.Value)
                    {
                        postDate = Convert.ToDateTime(reader["PostDate"]);
                    }

                    
                }

            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_ForumThreads ");
            sqlCommand.Append("SET MostRecentPostUserID = @MostRecentPostUserID, ");
            sqlCommand.Append("TotalReplies = TotalReplies - 1, ");
            sqlCommand.Append("MostRecentPostDate = @MostRecentPostDate ");
            sqlCommand.Append("WHERE ThreadID = @ThreadID ;");

            arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqlCeParameter("@MostRecentPostUserID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new SqlCeParameter("@MostRecentPostDate", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = postDate;

            

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadUpdateViewStats(int threadId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_ForumThreads ");
            sqlCommand.Append("SET TotalViews = TotalViews + 1 ");
            sqlCommand.Append("WHERE ThreadID = @ThreadID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader ForumThreadGetPosts(int threadId, int pageNumber)
        {
            int postsPerPage = 10;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	f.PostsPerPage ");
            sqlCommand.Append("FROM		mp_ForumThreads ft ");
            sqlCommand.Append("JOIN		mp_Forums f ");
            sqlCommand.Append("ON		ft.ForumID = f.ItemID ");
            sqlCommand.Append("WHERE	ft.ThreadID = @ThreadID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    postsPerPage = Convert.ToInt32(reader["PostsPerPage"]);
                }
            }

            int currentPageMaxThreadSequence = postsPerPage * pageNumber;
            int beginSequence = 1;
            if (currentPageMaxThreadSequence > postsPerPage)
            {
                beginSequence = currentPageMaxThreadSequence - postsPerPage + 1;

            }

            int endSequence = beginSequence + postsPerPage;

            sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT	p.*, ");
            sqlCommand.Append("ft.ForumID, ");
            sqlCommand.Append("ft.IsLocked, ");
            // TODO:
            //using 'Guest' here is not culture neutral, need to pass in a label
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
            sqlCommand.Append("COALESCE(up.Name, 'Guest') As PostAuthor, ");
            sqlCommand.Append("COALESCE(up.Email, '') As AuthorEmail, ");
            sqlCommand.Append("COALESCE(up.TotalPosts, 0) As PostAuthorTotalPosts, ");
            sqlCommand.Append("COALESCE(up.TotalRevenue, 0) As UserRevenue, ");
            sqlCommand.Append("COALESCE(up.Trusted, 0) As Trusted, ");
            sqlCommand.Append("COALESCE(up.AvatarUrl, 'blank.gif') As PostAuthorAvatar, ");
            sqlCommand.Append("up.WebSiteURL As PostAuthorWebSiteUrl, ");
            sqlCommand.Append("up.Signature As PostAuthorSignature ");

            sqlCommand.Append("FROM	mp_ForumPosts p ");

            sqlCommand.Append("JOIN	mp_ForumThreads ft ");
            sqlCommand.Append("ON p.ThreadID = ft.ThreadID ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON ft.MostRecentPostUserID = u.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users s ");
            sqlCommand.Append("ON ft.StartedByUserID = s.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users up ");
            sqlCommand.Append("ON up.UserID = p.UserID ");

            sqlCommand.Append("WHERE ft.ThreadID = @ThreadID ");
            sqlCommand.Append("AND p.ThreadSequence >= @BeginSequence ");
            sqlCommand.Append("AND p.ThreadSequence <= @EndSequence ");

            sqlCommand.Append("ORDER BY p.SortOrder, p.ThreadSequence ;");

            arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqlCeParameter("@BeginSequence", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginSequence;

            arParams[2] = new SqlCeParameter("@EndSequence", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = endSequence;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader ForumThreadGetPosts(int threadId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	p.*, ");
            sqlCommand.Append("ft.ForumID, ");
            sqlCommand.Append("ft.IsLocked, ");
            // TODO:
            //using 'Guest' here is not culture neutral, need to pass in a label
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
            sqlCommand.Append("COALESCE(up.Name, 'Guest') As PostAuthor, ");
            sqlCommand.Append("COALESCE(up.Email, '') As AuthorEmail, ");
            sqlCommand.Append("COALESCE(up.TotalPosts, 0) As PostAuthorTotalPosts, ");
            sqlCommand.Append("COALESCE(up.AvatarUrl, 'blank.gif') As PostAuthorAvatar, ");
            sqlCommand.Append("COALESCE(up.TotalRevenue, 0) As UserRevenue, ");
            sqlCommand.Append("COALESCE(up.Trusted, 0) As Trusted, ");
            sqlCommand.Append("up.WebSiteURL As PostAuthorWebSiteUrl, ");
            sqlCommand.Append("up.Signature As PostAuthorSignature ");


            sqlCommand.Append("FROM	mp_ForumPosts p ");
            sqlCommand.Append("JOIN	mp_ForumThreads ft ");
            sqlCommand.Append("ON p.ThreadID = ft.ThreadID ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON ft.MostRecentPostUserID = u.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN mp_Users s ");
            sqlCommand.Append("ON ft.StartedByUserID = s.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_Users up ");
            sqlCommand.Append("ON up.UserID = p.UserID ");
            sqlCommand.Append("WHERE ft.ThreadID = @ThreadID ");
            sqlCommand.Append("ORDER BY p.PostID  ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetPostsReverseSorted(int threadId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("p.PostID, ");
            sqlCommand.Append("p.ThreadID, ");
            sqlCommand.Append("p.ThreadSequence, ");
            sqlCommand.Append("p.Subject, ");
            sqlCommand.Append("p.PostDate, ");
            sqlCommand.Append("p.Approved, ");
            sqlCommand.Append("p.UserID, ");
            sqlCommand.Append("p.SortOrder, ");
            sqlCommand.Append("p.Post, ");

            sqlCommand.Append("ft.ForumID, ");
            sqlCommand.Append("ft.IsLocked, ");
            // TODO:
            //using 'Guest' here is not culture neutral, need to pass in a label
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
            sqlCommand.Append("COALESCE(up.Name, 'Guest') As PostAuthor, ");
            sqlCommand.Append("COALESCE(up.Email, '') As AuthorEmail, ");
            sqlCommand.Append("COALESCE(up.TotalPosts, 0) As PostAuthorTotalPosts, ");
            sqlCommand.Append("COALESCE(up.AvatarUrl, 'blank.gif') As PostAuthorAvatar, ");
            sqlCommand.Append("COALESCE(up.TotalRevenue, 0) As UserRevenue, ");
            sqlCommand.Append("COALESCE(up.Trusted, 0) As Trusted, ");
            sqlCommand.Append("up.WebSiteURL As PostAuthorWebSiteUrl, ");
            sqlCommand.Append("up.Signature As PostAuthorSignature ");

            sqlCommand.Append("FROM	mp_ForumPosts p ");

            sqlCommand.Append("JOIN	mp_ForumThreads ft ");
            sqlCommand.Append("ON p.ThreadID = ft.ThreadID ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON ft.MostRecentPostUserID = u.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users s ");
            sqlCommand.Append("ON ft.StartedByUserID = s.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users up ");
            sqlCommand.Append("ON up.UserID = p.UserID ");

            sqlCommand.Append("WHERE ft.ThreadID = @ThreadID ");

            sqlCommand.Append("ORDER BY p.ThreadSequence DESC  ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetPostsByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  fp.*, ");

            sqlCommand.Append("f.ModuleID, ");
            sqlCommand.Append("f.ItemID, ");

            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("m.ViewRoles, ");
            sqlCommand.Append("md.FeatureName ");

            sqlCommand.Append("FROM	mp_ForumPosts fp ");

            sqlCommand.Append("JOIN	mp_ForumThreads ft ");
            sqlCommand.Append("ON fp.ThreadID = ft.ThreadID ");

            sqlCommand.Append("JOIN	mp_Forums f ");
            sqlCommand.Append("ON f.ItemID = ft.ForumID ");

            sqlCommand.Append("JOIN	mp_Modules m ");
            sqlCommand.Append("ON f.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN	mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("JOIN	mp_PageModules pm ");
            sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");

            sqlCommand.Append("JOIN	mp_Pages p ");
            sqlCommand.Append("ON p.PageID = pm.PageID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.SiteID = @SiteID ");
            sqlCommand.Append("AND pm.PageID = @PageID ");
            sqlCommand.Append(" ; ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetThreadsByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ft.*, ");

            sqlCommand.Append("f.ModuleID, ");
            sqlCommand.Append("f.ItemID, ");

            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("m.ViewRoles, ");
            sqlCommand.Append("md.FeatureName ");

            sqlCommand.Append("FROM ");

            sqlCommand.Append("mp_ForumThreads ft ");
            

            sqlCommand.Append("JOIN	mp_Forums f ");
            sqlCommand.Append("ON f.ItemID = ft.ForumID ");

            sqlCommand.Append("JOIN	mp_Modules m ");
            sqlCommand.Append("ON f.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN	mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("JOIN	mp_PageModules pm ");
            sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");

            sqlCommand.Append("JOIN	mp_Pages p ");
            sqlCommand.Append("ON p.PageID = pm.PageID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.SiteID = @SiteID ");
            sqlCommand.Append("AND pm.PageID = @PageID ");
            sqlCommand.Append(" ; ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetPostsForRss(int siteId, int pageId, int moduleId, int itemId, int threadId, int maximumDays)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT		fp.*, ");
            sqlCommand.Append("ft.ThreadSubject, ");
            sqlCommand.Append("ft.ForumID, ");
            sqlCommand.Append("p.PageID, ");
            sqlCommand.Append("p.AuthorizedRoles, ");
            sqlCommand.Append("m.ModuleID, ");
            sqlCommand.Append("m.ViewRoles, ");

            sqlCommand.Append("COALESCE(s.Name,'Guest') as StartedBy, ");
            sqlCommand.Append("COALESCE(up.Name, 'Guest') as PostAuthor, ");
            sqlCommand.Append("up.TotalPosts as PostAuthorTotalPosts, ");
            sqlCommand.Append("up.AvatarUrl as PostAuthorAvatar, ");
            sqlCommand.Append("up.WebSiteURL as PostAuthorWebSiteUrl, ");
            sqlCommand.Append("up.Signature as PostAuthorSignature ");


            sqlCommand.Append("FROM		mp_ForumPosts fp ");

            sqlCommand.Append("JOIN		mp_ForumThreads ft ");
            sqlCommand.Append("ON		fp.ThreadID = ft.ThreadID ");

            sqlCommand.Append("JOIN		mp_Forums f ");
            sqlCommand.Append("ON		ft.ForumID = f.ItemID ");

            sqlCommand.Append("JOIN		mp_Modules m ");
            sqlCommand.Append("ON		f.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN		mp_PageModules pm ");
            sqlCommand.Append("ON		pm.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN		mp_Pages p ");
            sqlCommand.Append("ON		pm.PageID = p.PageID ");

            sqlCommand.Append("LEFT OUTER JOIN		mp_Users u ");
            sqlCommand.Append("ON		ft.MostRecentPostUserID = u.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN		mp_Users s ");
            sqlCommand.Append("ON		ft.StartedByUserID = s.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN		mp_Users up ");
            sqlCommand.Append("ON		up.UserID = fp.UserID ");

            sqlCommand.Append("WHERE	p.SiteID = @SiteID ");
            sqlCommand.Append("AND	(@PageID = -1 OR p.PageID = @PageID) ");
            sqlCommand.Append("AND	(@ModuleID = -1 OR m.ModuleID = @ModuleID) ");
            sqlCommand.Append("AND	(@ItemID = -1 OR f.ItemID = @ItemID) ");
            sqlCommand.Append("AND	(@ThreadID = -1 OR ft.ThreadID = @ThreadID) ");
            
            //sqlCommand.Append("AND	( (@MaximumDays = -1) OR  ( (GetDate() - @MaximumDays) >= fp.PostDate ) ) ");
            sqlCommand.Append("AND	(@MaximumDays = -1 OR datediff(dd, getdate(), fp.PostDate) <= @MaximumDays) ");


            sqlCommand.Append("ORDER BY	fp.PostDate DESC ; ");

            SqlCeParameter[] arParams = new SqlCeParameter[6];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            arParams[2] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleId;

            arParams[3] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = itemId;

            arParams[4] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = threadId;

            arParams[5] = new SqlCeParameter("@MaximumDays", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = maximumDays;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static DataSet ForumThreadGetSubscribers(int forumId, int threadId, int currentPostUserId, bool includeCurrentUser)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT u.Email, ");
            sqlCommand.Append("COALESCE(fts.ThreadSubscriptionID, -1) AS ThreadSubID, ");
            sqlCommand.Append("COALESCE(fs.SubscriptionID, -1) AS ForumSubID, ");
            sqlCommand.Append("COALESCE(fts.SubGuid, '00000000-0000-0000-0000-000000000000') AS ThreadSubGuid, ");
            sqlCommand.Append("COALESCE(fs.SubGuid, '00000000-0000-0000-0000-000000000000') AS ForumSubGuid ");

            sqlCommand.Append("FROM	mp_Users u ");

            sqlCommand.Append("LEFT OUTER JOIN mp_ForumThreadSubscriptions fts ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("fts.UserID = u.UserID ");
            sqlCommand.Append("AND fts.ThreadID = @ThreadID ");
            sqlCommand.Append("AND fts.UnSubscribeDate IS NULL ");

            sqlCommand.Append("LEFT OUTER JOIN mp_ForumSubscriptions fs ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("fs.UserID = u.UserID ");
            sqlCommand.Append("AND fs.ForumID = @ForumID ");
            sqlCommand.Append("AND fs.UnSubscribeDate IS NULL ");

            sqlCommand.Append("WHERE ");

            if (!includeCurrentUser)
            {
                sqlCommand.Append(" u.UserID <> @CurrentPostUserID ");
                sqlCommand.Append("AND ");
            }

            sqlCommand.Append("(");

            sqlCommand.Append("(");
            sqlCommand.Append("fts.ThreadSubscriptionID IS NOT NULL ");
            sqlCommand.Append(")");

            sqlCommand.Append("OR ");

            sqlCommand.Append("(");
            sqlCommand.Append("fs.SubscriptionID IS NOT NULL ");
            sqlCommand.Append(")");

            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ForumID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadId;

            arParams[2] = new SqlCeParameter("@CurrentPostUserID", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentPostUserId;

            return SqlHelper.ExecuteDataset(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetSubscriber(Guid subGuid)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_ForumThreadSubscriptions ");

            sqlCommand.Append("WHERE SubGuid = @SubGuid ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SubGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public static bool ForumThreadAddSubscriber(int threadId, int userId, Guid subGuid)
        {
            int subscriptionCount = 0;
            int rowsAffected = -1;

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT COUNT(*) As SubscriptionCount ");
            sqlCommand.Append("FROM mp_ForumThreadSubscriptions  ");
            sqlCommand.Append("WHERE ThreadID = @ThreadID AND UserID = @UserID AND UnSubscribeDate IS NULL ; ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;


            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    subscriptionCount = Convert.ToInt32(reader["SubscriptionCount"]);
                }
            }

            


            if (subscriptionCount > 0)
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("UPDATE mp_ForumThreadSubscriptions ");
                sqlCommand.Append("SET UnSubscribeDate = @CurrentTime ");
                
                sqlCommand.Append("WHERE ThreadID = @ThreadID AND UserID = @UserID AND UnSubscribeDate IS NULL ;");

                arParams = new SqlCeParameter[3];

                arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = threadId;

                arParams[1] = new SqlCeParameter("@UserID", SqlDbType.Int);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = userId;

                arParams[2] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
                arParams[2].Direction = ParameterDirection.Input;
                arParams[2].Value = DateTime.UtcNow;

                rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            }


            sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO	mp_ForumThreadSubscriptions ( ");
            sqlCommand.Append("ThreadID, ");
            sqlCommand.Append("UserID, ");
            sqlCommand.Append("SubGuid, ");
            sqlCommand.Append("SubscribeDate ");
            sqlCommand.Append(") ");
            sqlCommand.Append("VALUES ( ");
            sqlCommand.Append("@ThreadID, ");
            sqlCommand.Append("@UserID, ");
            sqlCommand.Append("@SubGuid, ");
            sqlCommand.Append("@SubscribeDate ");
            sqlCommand.Append(") ;");



            arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            arParams[3] = new SqlCeParameter("@SubGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = subGuid;



            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        public static bool ForumThreadUnSubscribe(Guid subGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_ForumThreadSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = @CurrentTime ");
            sqlCommand.Append("WHERE SubGuid = @SubGuid ;");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SubGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subGuid;

            arParams[1] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

            

        }

        public static bool ForumThreadUNSubscribe(int threadId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_ForumThreadSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = GetDate() ");
            sqlCommand.Append("WHERE ThreadID = @ThreadID AND UserID = @UserID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadUnsubscribeAll(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_ForumThreadSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = GetDate() ");
            sqlCommand.Append("WHERE UserID = @UserID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static int ForumPostCreate(
            int threadId,
            string subject,
            string post,
            bool approved,
            int userId,
            DateTime postDate,
            Guid postGuid,
            Guid approvedBy,
            DateTime approvedUtc,
            string userIp,
            bool notificationSent,
            int modStatus)
        {
            int threadSequence = 1;
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COALESCE(Max(ThreadSequence) + 1,1) As ThreadSequence FROM mp_ForumPosts WHERE ThreadID = @ThreadID ; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    threadSequence = Convert.ToInt32(reader["ThreadSequence"]);
                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_ForumPosts ");
            sqlCommand.Append("(");
            sqlCommand.Append("ThreadID, ");
            sqlCommand.Append("ThreadSequence, ");
            sqlCommand.Append("Subject, ");
            sqlCommand.Append("PostDate, ");
            sqlCommand.Append("Approved, ");
            sqlCommand.Append("UserID, ");
            sqlCommand.Append("SortOrder, ");

            sqlCommand.Append("AnswerVotes, ");
            sqlCommand.Append("PostGuid, ");
            sqlCommand.Append("ApprovedBy, ");
            sqlCommand.Append("ApprovedUtc, ");
            sqlCommand.Append("UserIp, ");

            sqlCommand.Append("NotificationSent, ");
            sqlCommand.Append("ModStatus, ");

            sqlCommand.Append("Post ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ThreadID, ");
            sqlCommand.Append("@ThreadSequence, ");
            sqlCommand.Append("@Subject, ");
            sqlCommand.Append("@PostDate, ");
            sqlCommand.Append("@Approved, ");
            sqlCommand.Append("@UserID, ");
            sqlCommand.Append("@SortOrder, ");

            sqlCommand.Append("0, ");
            sqlCommand.Append("@PostGuid, ");
            sqlCommand.Append("@ApprovedBy, ");
            sqlCommand.Append("@ApprovedUtc, ");
            sqlCommand.Append("@UserIp, ");

            sqlCommand.Append("@NotificationSent, ");
            sqlCommand.Append("@ModStatus, ");

            sqlCommand.Append("@Post ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            arParams = new SqlCeParameter[14];

            arParams[0] = new SqlCeParameter("@ThreadID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqlCeParameter("@ThreadSequence", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadSequence;

            arParams[2] = new SqlCeParameter("@Subject", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = subject;

            arParams[3] = new SqlCeParameter("@PostDate", SqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = postDate;

            arParams[4] = new SqlCeParameter("@Approved", SqlDbType.Bit);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = approved;

            arParams[5] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = userId;

            arParams[6] = new SqlCeParameter("@SortOrder", SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = 100;

            arParams[7] = new SqlCeParameter("@Post", SqlDbType.NText);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = post;

            arParams[8] = new SqlCeParameter("@PostGuid", SqlDbType.UniqueIdentifier);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = postGuid;

            arParams[9] = new SqlCeParameter("@ApprovedBy", SqlDbType.UniqueIdentifier);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = approvedBy;

            arParams[10] = new SqlCeParameter("@ApprovedUtc", SqlDbType.DateTime);
            arParams[10].Direction = ParameterDirection.Input;
            if (approvedUtc > DateTime.MinValue)
            {
                arParams[10].Value = approvedUtc;
            }
            else
            {
                arParams[10].Value = DBNull.Value;
            }

            arParams[11] = new SqlCeParameter("@UserIp", SqlDbType.NVarChar, 50);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = userIp;

            arParams[12] = new SqlCeParameter("@NotificationSent", SqlDbType.Bit);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = notificationSent;

            arParams[13] = new SqlCeParameter("@ModStatus", SqlDbType.Int);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = modStatus;


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;

        }

        public static bool ForumPostUpdate(
            int postId,
            string subject,
            string post,
            int sortOrder,
            bool approved,
            Guid approvedBy,
            DateTime approvedUtc,
            bool notificationSent,
            int modStatus)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_ForumPosts ");
            sqlCommand.Append("SET Subject = @Subject, ");
            sqlCommand.Append("Post = @Post, ");
            sqlCommand.Append("SortOrder = @SortOrder, ");

            sqlCommand.Append("NotificationSent = @NotificationSent, ");
            sqlCommand.Append("ModStatus = @ModStatus, ");

            sqlCommand.Append("ApprovedBy = @ApprovedBy, ");
            sqlCommand.Append("ApprovedUtc = @ApprovedUtc, ");

            sqlCommand.Append("Approved = @Approved ");
            sqlCommand.Append("WHERE PostID = @PostID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[9];

            arParams[0] = new SqlCeParameter("@PostID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            arParams[1] = new SqlCeParameter("@Subject", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = subject;

            arParams[2] = new SqlCeParameter("@Approved", SqlDbType.Bit);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = approved;

            arParams[3] = new SqlCeParameter("@SortOrder", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortOrder;

            arParams[4] = new SqlCeParameter("@Post", SqlDbType.NText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = post;

            arParams[5] = new SqlCeParameter("@ApprovedBy", SqlDbType.UniqueIdentifier);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = approvedBy;

            arParams[6] = new SqlCeParameter("@ApprovedUtc", SqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            if (approvedUtc > DateTime.MinValue)
            {
                arParams[6].Value = approvedUtc;
            }
            else
            {
                arParams[6].Value = DBNull.Value;
            }

            arParams[7] = new SqlCeParameter("@NotificationSent", SqlDbType.Bit);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = notificationSent;

            arParams[8] = new SqlCeParameter("@ModStatus", SqlDbType.Int);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = modStatus;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumPostDelete(int postId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumPosts ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PostID = @PostID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PostID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumPostUpdateThreadSequence(
            int postId,
            int threadSequence)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_ForumPosts ");
            sqlCommand.Append("SET ThreadSequence = @ThreadSequence ");
            sqlCommand.Append("WHERE PostID = @PostID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@PostID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            arParams[1] = new SqlCeParameter("@ThreadSequence", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadSequence;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

    }
}
