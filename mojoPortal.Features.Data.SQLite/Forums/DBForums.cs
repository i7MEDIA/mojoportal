// Author:					
// Created:				    2007-11-03
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
   
    public static class DBForums
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
            #region Bit Conversion

            byte isMod = 1;
            if (!isModerated)
            {
                isMod = 0;
            }

            byte active = 1;
            if (!isActive)
            {
                active = 0;
            }

            byte allowAnonymous = 1;
            if (!allowAnonymousPosts)
            {
                allowAnonymous = 0;
            }

            int intIncludeInGoogleMap = includeInGoogleMap ? 1 : 0;
            int intAddNoIndexMeta = addNoIndexMeta ? 1 : 0;
            int intClosed = closed ? 1 : 0;
            int intVisible = visible ? 1 : 0;
            int intRequireModeration = requireModeration ? 1 : 0;
            int intRequireModForNotify = requireModForNotify ? 1 : 0;
            int intAllowTrustedDirectPosts = allowTrustedDirectPosts ? 1 : 0;
            int intAllowTrustedDirectNotify = allowTrustedDirectNotify ? 1 : 0;

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO	mp_Forums ( ");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("CreatedDate, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Description , ");
            sqlCommand.Append("IsModerated , ");
            sqlCommand.Append("IsActive , ");
            sqlCommand.Append("SortOrder , ");
            sqlCommand.Append("PostsPerPage , ");
            sqlCommand.Append("ThreadsPerPage , ");
            sqlCommand.Append("ForumGuid , ");

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

            sqlCommand.Append("AllowAnonymousPosts  ");
            sqlCommand.Append(" ) ");

            sqlCommand.Append("VALUES (");

            sqlCommand.Append(" :ModuleID , ");
            sqlCommand.Append(" :UserID  , ");
            sqlCommand.Append(" datetime('now','localtime'), ");
            sqlCommand.Append(" :Title , ");
            sqlCommand.Append(" :Description , ");
            sqlCommand.Append(" :IsModerated , ");
            sqlCommand.Append(" :IsActive , ");
            sqlCommand.Append(" :SortOrder , ");
            sqlCommand.Append(" :PostsPerPage , ");
            sqlCommand.Append(" :ThreadsPerPage , ");
            sqlCommand.Append(":ForumGuid , ");

            sqlCommand.Append(":RolesThatCanPost, ");
            sqlCommand.Append(":RolesThatCanModerate, ");
            sqlCommand.Append(":ModeratorNotifyEmail, ");
            sqlCommand.Append(":IncludeInGoogleMap, ");
            sqlCommand.Append(":AddNoIndexMeta, ");
            sqlCommand.Append(":Closed, ");
            sqlCommand.Append(":Visible, ");
            sqlCommand.Append(":RequireModeration, ");
            sqlCommand.Append(":RequireModForNotify, ");
            sqlCommand.Append(":AllowTrustedDirectPosts, ");
            sqlCommand.Append(":AllowTrustedDirectNotify, ");

            sqlCommand.Append(" :AllowAnonymousPosts  ");

            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[22];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new SqliteParameter(":Title", DbType.String, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new SqliteParameter(":Description", DbType.Object);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new SqliteParameter(":IsModerated", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = isMod;

            arParams[5] = new SqliteParameter(":IsActive", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = active;

            arParams[6] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = sortOrder;

            arParams[7] = new SqliteParameter(":PostsPerPage", DbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = postsPerPage;

            arParams[8] = new SqliteParameter(":ThreadsPerPage", DbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = threadsPerPage;

            arParams[9] = new SqliteParameter(":AllowAnonymousPosts", DbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = allowAnonymous;

            arParams[10] = new SqliteParameter(":ForumGuid", DbType.String, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = forumGuid.ToString();

            arParams[11] = new SqliteParameter(":RolesThatCanPost", DbType.Object);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = rolesThatCanPost;

            arParams[12] = new SqliteParameter(":RolesThatCanModerate", DbType.Object);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = rolesThatCanModerate;

            arParams[13] = new SqliteParameter(":ModeratorNotifyEmail", DbType.Object);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = moderatorNotifyEmail;

            arParams[14] = new SqliteParameter(":IncludeInGoogleMap", DbType.Int32);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = intIncludeInGoogleMap;

            arParams[15] = new SqliteParameter(":AddNoIndexMeta", DbType.Int32);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = intAddNoIndexMeta;

            arParams[16] = new SqliteParameter(":Closed", DbType.Int32);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = intClosed;

            arParams[17] = new SqliteParameter(":Visible", DbType.Int32);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = intVisible;

            arParams[18] = new SqliteParameter(":RequireModeration", DbType.Int32);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = intRequireModeration;

            arParams[19] = new SqliteParameter(":RequireModForNotify", DbType.Int32);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = intRequireModForNotify;

            arParams[20] = new SqliteParameter(":AllowTrustedDirectPosts", DbType.Int32);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = intAllowTrustedDirectPosts;

            arParams[21] = new SqliteParameter(":AllowTrustedDirectNotify", DbType.Int32);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = intAllowTrustedDirectNotify;

            int newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return newID;

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

            #region Bit Conversion

            byte moderated = 1;
            if (!isModerated)
            {
                moderated = 0;
            }

            byte active = 1;
            if (!isActive)
            {
                active = 0;
            }

            byte allowAnonymous = 1;
            if (!allowAnonymousPosts)
            {
                allowAnonymous = 0;
            }

            int intIncludeInGoogleMap = includeInGoogleMap ? 1 : 0;
            int intAddNoIndexMeta = addNoIndexMeta ? 1 : 0;
            int intClosed = closed ? 1 : 0;
            int intVisible = visible ? 1 : 0;
            int intRequireModeration = requireModeration ? 1 : 0;
            int intRequireModForNotify = requireModForNotify ? 1 : 0;
            int intAllowTrustedDirectPosts = allowTrustedDirectPosts ? 1 : 0;
            int intAllowTrustedDirectNotify = allowTrustedDirectNotify ? 1 : 0;

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE		mp_Forums ");
            sqlCommand.Append("SET	Title = :Title, ");
            sqlCommand.Append("Description = :Description, ");
            sqlCommand.Append("IsModerated = :IsModerated, ");
            sqlCommand.Append("IsActive = :IsActive, ");
            sqlCommand.Append("SortOrder = :SortOrder, ");
            sqlCommand.Append("PostsPerPage = :PostsPerPage, ");
            sqlCommand.Append("ThreadsPerPage = :ThreadsPerPage, ");

            sqlCommand.Append("RolesThatCanPost = :RolesThatCanPost, ");
            sqlCommand.Append("RolesThatCanModerate = :RolesThatCanModerate, ");
            sqlCommand.Append("ModeratorNotifyEmail = :ModeratorNotifyEmail, ");
            sqlCommand.Append("IncludeInGoogleMap = :IncludeInGoogleMap, ");
            sqlCommand.Append("AddNoIndexMeta = :AddNoIndexMeta, ");
            sqlCommand.Append("Closed = :Closed, ");
            sqlCommand.Append("Visible = :Visible, ");
            sqlCommand.Append("RequireModeration = :RequireModeration, ");
            sqlCommand.Append("RequireModForNotify = :RequireModForNotify, ");
            sqlCommand.Append("AllowTrustedDirectPosts = :AllowTrustedDirectPosts, ");
            sqlCommand.Append("AllowTrustedDirectNotify = :AllowTrustedDirectNotify, ");

            sqlCommand.Append("AllowAnonymousPosts = :AllowAnonymousPosts ");

            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[20];


            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqliteParameter(":Title", DbType.String, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new SqliteParameter(":Description", DbType.Object);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = description;

            arParams[3] = new SqliteParameter(":IsModerated", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moderated;

            arParams[4] = new SqliteParameter(":IsActive", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = active;

            arParams[5] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sortOrder;

            arParams[6] = new SqliteParameter(":PostsPerPage", DbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = postsPerPage;

            arParams[7] = new SqliteParameter(":ThreadsPerPage", DbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = threadsPerPage;

            arParams[8] = new SqliteParameter(":AllowAnonymousPosts", DbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = allowAnonymous;

            arParams[9] = new SqliteParameter(":RolesThatCanModerate", DbType.Object);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = rolesThatCanModerate;

            arParams[10] = new SqliteParameter(":ModeratorNotifyEmail", DbType.Object);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = moderatorNotifyEmail;

            arParams[11] = new SqliteParameter(":IncludeInGoogleMap", DbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = intIncludeInGoogleMap;

            arParams[12] = new SqliteParameter(":AddNoIndexMeta", DbType.Int32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = intAddNoIndexMeta;

            arParams[13] = new SqliteParameter(":Closed", DbType.Int32);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = intClosed;

            arParams[14] = new SqliteParameter(":Visible", DbType.Int32);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = intVisible;

            arParams[15] = new SqliteParameter(":RequireModeration", DbType.Int32);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = intRequireModeration;

            arParams[16] = new SqliteParameter(":RequireModForNotify", DbType.Int32);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = intRequireModForNotify;

            arParams[17] = new SqliteParameter(":AllowTrustedDirectPosts", DbType.Int32);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = intAllowTrustedDirectPosts;

            arParams[18] = new SqliteParameter(":AllowTrustedDirectNotify", DbType.Int32);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = intAllowTrustedDirectNotify;

            arParams[19] = new SqliteParameter(":RolesThatCanPost", DbType.Object);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = rolesThatCanPost;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Forums ");
            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumPosts WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID = :ModuleID) );");
            sqlCommand.Append("DELETE FROM mp_ForumThreadSubscriptions WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID = :ModuleID) );");
            sqlCommand.Append("DELETE FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID = :ModuleID);");
            sqlCommand.Append("DELETE FROM mp_ForumSubscriptions WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID = :ModuleID) ;");
            sqlCommand.Append("DELETE FROM mp_Forums WHERE ModuleID = :ModuleID;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumPosts WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID IN  (SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID)) );");
            sqlCommand.Append("DELETE FROM mp_ForumThreadSubscriptions WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID IN  (SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID)) );");
            sqlCommand.Append("DELETE FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID IN  (SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID));");
            sqlCommand.Append("DELETE FROM mp_ForumSubscriptions WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID IN  (SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID)) ;");
            sqlCommand.Append("DELETE FROM mp_Forums WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID);");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        



        public static IDataReader GetForums(int moduleId, int userId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT f.*,  ");
            //sqlCommand.Append("f.ItemID AS ItemID, ");
            //sqlCommand.Append("f.ModuleID AS ModuleID, ");
            //sqlCommand.Append("f.CreatedDate AS CreatedDate, ");
            //sqlCommand.Append("f.CreatedBy AS CreatedBy, ");
            //sqlCommand.Append("f.Title AS Title, ");
            //sqlCommand.Append("f.Description AS Description, ");
            //sqlCommand.Append("f.IsModerated AS IsModerated, ");
            //sqlCommand.Append("f.IsActive AS IsActive, ");
            //sqlCommand.Append("f.SortOrder AS SortOrder, ");
            //sqlCommand.Append("f.ThreadCount AS ThreadCount, ");
            //sqlCommand.Append("f.PostCount AS PostCount, ");
            //sqlCommand.Append("COALESCE(f.MostRecentPostDate, '" + DateTime.UtcNow + "') AS MostRecentPostDate, ");
            //sqlCommand.Append("f.MostRecentPostUserID AS MostRecentPostUserID, ");
            //sqlCommand.Append("f.PostsPerPage AS PostsPerPage, ");
            //sqlCommand.Append("f.ThreadsPerPage AS ThreadsPerPage, ");
            //sqlCommand.Append("f.AllowAnonymousPosts AS AllowAnonymousPosts ");

            sqlCommand.Append("u.Name As MostRecentPostUser, ");
            sqlCommand.Append("s.SubscribeDate IS NOT NULL AND s.UnSubscribeDate IS NULL As Subscribed, ");
            sqlCommand.Append("(SELECT COUNT(*) FROM mp_ForumSubscriptions fs WHERE fs.ForumID = f.ItemID AND fs.UnSubscribeDate IS NULL) As SubscriberCount  ");

            sqlCommand.Append("FROM	mp_Forums f ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON f.MostRecentPostUserID = u.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_ForumSubscriptions s ");
            sqlCommand.Append("ON f.ItemID = s.ForumID AND s.UserID = :UserID AND s.UnSubscribeDate IS NULL ");

            sqlCommand.Append("WHERE f.ModuleID	= :ModuleID ");
            sqlCommand.Append("AND f.IsActive = 1 ");
            sqlCommand.Append("ORDER BY		f.SortOrder, f.ItemID ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("WHERE f.ItemID	= :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static bool IncrementPostCount(
            int forumId,
            int mostRecentPostUserId,
            DateTime mostRecentPostDate)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Forums ");
            sqlCommand.Append("SET MostRecentPostDate = :MostRecentPostDate, ");
            sqlCommand.Append("MostRecentPostUserID = :MostRecentPostUserID, ");
            sqlCommand.Append("PostCount = PostCount + 1 ");

            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqliteParameter(":MostRecentPostUserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostUserId;

            arParams[2] = new SqliteParameter(":MostRecentPostDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = mostRecentPostDate;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(), sqlCommand.ToString(), arParams);

            return (rowsAffected > -1);

        }

        public static bool UpdateUserStats(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("TotalPosts = (SELECT COUNT(*) FROM mp_ForumPosts WHERE mp_ForumPosts.UserID = mp_Users.UserID) ");
            if (userId > -1)
            {
                sqlCommand.Append("WHERE UserID = :UserID ");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DecrementPostCount(int forumId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Forums ");
            sqlCommand.Append("SET PostCount = PostCount - 1 ");

            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool RecalculatePostStats(int forumId)
        {
            DateTime mostRecentPostDate = DateTime.Now;
            int mostRecentPostUserID = -1;
            int postCount = 0;

            StringBuilder sqlCommand = new StringBuilder();
            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("MostRecentPostDate, ");
            sqlCommand.Append("MostRecentPostUserID ");
            sqlCommand.Append("FROM mp_ForumThreads ");
            sqlCommand.Append("WHERE ForumID = :ForumID ");
            sqlCommand.Append("ORDER BY MostRecentPostDate DESC ");
            sqlCommand.Append("LIMIT 1 ;");

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    if (reader["MostRecentPostUserID"] != DBNull.Value)
                    {
                        mostRecentPostUserID = Convert.ToInt32(reader["MostRecentPostUserID"]);
                    }

                    if (reader["MostRecentPostDate"] != DBNull.Value)
                    {
                        mostRecentPostDate = Convert.ToDateTime(reader["MostRecentPostDate"]);
                    }
                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("COALESCE(SUM(TotalReplies) + COUNT(*),0) As PostCount ");
            sqlCommand.Append("FROM mp_ForumThreads ");
            sqlCommand.Append("WHERE ForumID = :ForumID ;");

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("MostRecentPostDate = :MostRecentPostDate,	 ");
            sqlCommand.Append("MostRecentPostUserID = :MostRecentPostUserID,	 ");
            sqlCommand.Append("PostCount = :PostCount	 ");
            sqlCommand.Append("WHERE ItemID = :ForumID ;");

            arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqliteParameter(":MostRecentPostDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostDate;

            arParams[2] = new SqliteParameter(":MostRecentPostUserID", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = mostRecentPostUserID;

            arParams[3] = new SqliteParameter(":PostCount", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = postCount;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool IncrementThreadCount(int forumId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE	mp_Forums ");
            sqlCommand.Append("SET	ThreadCount = ThreadCount + 1 ");
            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DecrementThreadCount(int forumId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Forums ");
            sqlCommand.Append("SET ThreadCount = ThreadCount - 1 ");

            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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

            sqlCommand.Append("WHERE m.SiteID = :SiteID AND ft.ThreadID IN (Select DISTINCT ThreadID FROM mp_ForumPosts WHERE mp_ForumPosts.UserID = :UserID) ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteId;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	 ");
            sqlCommand.Append(" t.*, ");
            sqlCommand.Append("f.Title As Forum, ");
            sqlCommand.Append("f.ModuleID, ");
            sqlCommand.Append("(SELECT PageID FROM mp_PageModules WHERE mp_PageModules.ModuleID = f.ModuleID AND (PublishEndDate IS NULL OR PublishEndDate > :CurrentDate) LIMIT 1) As PageID, ");
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("s.Name As StartedBy ");

            sqlCommand.Append("FROM	mp_ForumThreads t ");

            sqlCommand.Append("JOIN	mp_Forums f ");
            sqlCommand.Append("ON t.ForumID = f.ItemID ");

            sqlCommand.Append("JOIN mp_Modules m ");
            sqlCommand.Append("ON f.ModuleID = m.ModuleID ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON t.MostRecentPostUserID = u.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users s ");
            sqlCommand.Append("ON t.StartedByUserID = s.UserID ");

            sqlCommand.Append("WHERE m.SiteID = :SiteID AND t.ThreadID IN (Select DISTINCT ThreadID FROM mp_ForumPosts WHERE mp_ForumPosts.UserID = :UserID) ");

            sqlCommand.Append("ORDER BY	t.MostRecentPostDate DESC  ");

            sqlCommand.Append("LIMIT " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + ", :PageSize ");
            sqlCommand.Append(";");


            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new SqliteParameter(":CurrentDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            arParams[3] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = siteId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

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


            sqlCommand.Append("WHERE p.SiteID = :SiteID ");
            sqlCommand.Append("AND ft.IncludeInSiteMap = 1 ");

            sqlCommand.Append("ORDER BY ft.ThreadID DESC ");

            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetThreads(int forumId, int pageNumber)
        {
            int threadsPerPage = 1;
            int totalThreads = 0;
            using (IDataReader reader = GetForum(forumId))
            {
                if (reader.Read())
                {
                    threadsPerPage = Convert.ToInt32(reader["ThreadsPerPage"]);
                    totalThreads = Convert.ToInt32(reader["ThreadCount"]);
                }
            }

            int pageLowerBound = (threadsPerPage * pageNumber) - threadsPerPage;

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT	t.*, ");
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("s.Name As StartedBy ");
            sqlCommand.Append("FROM	mp_ForumThreads t ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON t.MostRecentPostUserID = u.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_Users s ");
            sqlCommand.Append("ON t.StartedByUserID = s.UserID ");
            sqlCommand.Append("WHERE	t.ForumID = :ForumID ");
            sqlCommand.Append("ORDER BY t.SortOrder, t.MostRecentPostDate DESC ");
            sqlCommand.Append("LIMIT		" + threadsPerPage + " ");
            sqlCommand.Append("OFFSET		" + pageLowerBound + " ");
            sqlCommand.Append(" ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqliteParameter(":PageNumber", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int ForumThreadGetPostCount(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            sqlCommand.Append("SELECT COUNT(*) FROM mp_ForumPosts WHERE ThreadID = :ThreadID ");

            int count = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return count;

        }

        public static int GetSubscriberCount(int forumId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_ForumSubscriptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ForumID = :ForumID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("UnSubscribeDate IS NULL");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("fs.SubscriptionID, ");
            sqlCommand.Append("fs.SubscribeDate, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("FROM	mp_ForumSubscriptions fs ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.UserID = fs.UserID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("fs.ForumID = :ForumID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("fs.UnSubscribeDate IS NULL ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("u.Name  ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static bool AddSubscriber(int forumId, int userId, Guid subGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT COUNT(*) As SubscriptionCount ");
            sqlCommand.Append("FROM mp_ForumSubscriptions  ");
            sqlCommand.Append("WHERE ForumID = :ForumID AND UserID = :UserID AND UnSubscribeDate IS NULL ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int subscriptionCount = 0;
            int rowsAffected = -1;

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    subscriptionCount = Convert.ToInt32(reader["SubscriptionCount"]);
                }
            }

            sqlCommand = new StringBuilder();

           
            if (subscriptionCount > 0)
            {
                arParams = new SqliteParameter[3];

                arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = forumId;

                arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = userId;

                arParams[2] = new SqliteParameter(":SubscribeDate", DbType.DateTime);
                arParams[2].Direction = ParameterDirection.Input;
                arParams[2].Value = DateTime.UtcNow;

                sqlCommand.Append("UPDATE mp_ForumSubscriptions ");
                sqlCommand.Append("SET UnSubscribeDate = :SubscribeDate ");
                //sqlCommand.Append("UnSubscribeDate = Null ");
                sqlCommand.Append("WHERE ForumID = :ForumID AND UserID = :UserID AND UnSubscribeDate IS NULL ;");

                rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append(":ForumID, ");
            sqlCommand.Append(":UserID, ");
            sqlCommand.Append(":SubGuid, ");
            sqlCommand.Append(":SubscribeDate");
            sqlCommand.Append(") ;");

            

            arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new SqliteParameter(":SubscribeDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            arParams[3] = new SqliteParameter(":SubGuid", DbType.String, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = subGuid.ToString();

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteSubscription(int subscriptionId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumSubscriptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SubscriptionID = :SubscriptionID ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SubscriptionID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subscriptionId;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool Unsubscribe(int forumId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_ForumSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = :UnSubscribeDate ");
            sqlCommand.Append("WHERE ForumID = :ForumID AND UserID = :UserID ;");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new SqliteParameter(":UnSubscribeDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UnsubscribeAll(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_ForumSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = :UnSubscribeDate ");
            sqlCommand.Append("WHERE UserID = :UserID ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new SqliteParameter(":UnSubscribeDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Unsubscribe(Guid subGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_ForumSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = :UnSubscribeDate ");
            sqlCommand.Append("WHERE SubGuid = :SubGuid ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SubGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subGuid.ToString();

            arParams[1] = new SqliteParameter(":UnSubscribeDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        public static IDataReader GetForumSubscription(Guid subGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_ForumSubscriptions ");

            sqlCommand.Append("WHERE SubGuid = :SubGuid ;");


            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SubGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static bool ForumSubscriptionExists(int forumId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_ForumSubscriptions ");
            sqlCommand.Append("WHERE ForumID = :ForumID AND UserID = :UserID AND UnSubscribeDate IS NULL ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int count = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public static bool ForumThreadSubscriptionExists(int threadId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_ForumThreadSubscriptions ");
            sqlCommand.Append("WHERE ThreadID = :ThreadID AND UserID = :UserID AND UnSubscribeDate IS NULL ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int count = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public static IDataReader ForumThreadGetThread(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	t.*, ");
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
            sqlCommand.Append("f.PostsPerPage As PostsPerPage, ");
            sqlCommand.Append("f.ModuleID AS ModuleID ");

            sqlCommand.Append("FROM	mp_ForumThreads t ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON t.MostRecentPostUserID = u.UserID ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_Users s ");
            sqlCommand.Append("ON t.StartedByUserID = s.UserID ");
            sqlCommand.Append("JOIN	mp_Forums f ");
            sqlCommand.Append("ON f.ItemID = t.ForumID ");
            sqlCommand.Append("WHERE t.ThreadID = :ThreadID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader ForumThreadGetPost(int postId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	fp.* ");
            sqlCommand.Append("FROM	mp_ForumPosts fp ");
            sqlCommand.Append("WHERE fp.PostID = :PostID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":PostID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

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

            byte locked = 1;
            if (!isLocked) { locked = 0; }

            byte isQ = 1;
            if (!isQuestion) { isQ = 0; }

            byte inMap = 1;
            if (!includeInSiteMap) { inMap = 0; }

            byte noIndex = 1;
            if (!setNoIndexMeta) { noIndex = 0; }

            StringBuilder sqlCommand = new StringBuilder();
            int forumSequence = 1;
            sqlCommand.Append("SELECT COALESCE(Max(ForumSequence) + 1,1) As ForumSequence FROM mp_ForumThreads WHERE ForumID = :ForumID ; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    forumSequence = Convert.ToInt32(reader["ForumSequence"]);
                }
            }


            sqlCommand = new StringBuilder();
            //sqlCommand.Append("SET @ForumSequence = " + ForumSequence.ToString() + " ; ");

            sqlCommand.Append("INSERT INTO mp_ForumThreads ( ");
            sqlCommand.Append("ThreadGuid, ");
            sqlCommand.Append("ForumID, ");
            sqlCommand.Append("ThreadSubject, ");
            sqlCommand.Append("SortOrder, ");
            sqlCommand.Append("ForumSequence, ");
            sqlCommand.Append("IsLocked, ");
            sqlCommand.Append("StartedByUserID, ");
            sqlCommand.Append("ThreadDate, ");

            sqlCommand.Append("IsQuestion, ");
            sqlCommand.Append("IncludeInSiteMap, ");
            sqlCommand.Append("SetNoIndexMeta, ");
            sqlCommand.Append("PTitleOverride, ");

            sqlCommand.Append("ModStatus, ");
            sqlCommand.Append("ThreadType, ");

            sqlCommand.Append("MostRecentPostUserID, ");
            sqlCommand.Append("MostRecentPostDate ");
            sqlCommand.Append(" ) ");

            sqlCommand.Append("VALUES (");
            sqlCommand.Append(":ThreadGuid, ");
            sqlCommand.Append(" :ForumID , ");
            sqlCommand.Append(" :ThreadSubject  , ");
            sqlCommand.Append(" :SortOrder, ");
            sqlCommand.Append(" :ForumSequence, ");
            sqlCommand.Append(" :IsLocked , ");
            sqlCommand.Append(" :StartedByUserID , ");
            sqlCommand.Append(" :ThreadDate , ");

            sqlCommand.Append(":IsQuestion, ");
            sqlCommand.Append(":IncludeInSiteMap, ");
            sqlCommand.Append(":SetNoIndexMeta, ");
            sqlCommand.Append(":PTitleOverride, ");

            sqlCommand.Append(":ModStatus, ");
            sqlCommand.Append(":ThreadType, ");

            sqlCommand.Append(" :StartedByUserID , ");
            sqlCommand.Append(" :ThreadDate  ");
            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            arParams = new SqliteParameter[14];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqliteParameter(":ThreadSubject", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadSubject;

            arParams[2] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = sortOrder;

            arParams[3] = new SqliteParameter(":IsLocked", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = locked;

            arParams[4] = new SqliteParameter(":StartedByUserID", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = startedByUserId;

            arParams[5] = new SqliteParameter(":ForumSequence", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = forumSequence;

            arParams[6] = new SqliteParameter(":ThreadDate", DbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = threadDate;

            arParams[7] = new SqliteParameter(":ThreadGuid", DbType.String, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = threadGuid.ToString();

            arParams[8] = new SqliteParameter(":IsQuestion", DbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = isQ;

            arParams[9] = new SqliteParameter(":IncludeInSiteMap", DbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = inMap;

            arParams[10] = new SqliteParameter(":SetNoIndexMeta", DbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = noIndex;

            arParams[11] = new SqliteParameter(":PTitleOverride", DbType.String, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = pageTitleOverride;

            arParams[12] = new SqliteParameter(":ModStatus", DbType.Int32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = modStatus;

            arParams[13] = new SqliteParameter(":ThreadType", DbType.String, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = threadType;

            int newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            

            return newID;

        }

        public static bool ForumThreadDelete(int threadId)
        {
            ForumThreadDeletePosts(threadId);
            ForumThreadDeleteSubscriptions(threadId);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumThreads ");
            sqlCommand.Append("WHERE ThreadID = :ThreadID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadDeletePosts(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumPosts ");
            sqlCommand.Append("WHERE ThreadID = :ThreadID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadDeleteSubscriptions(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumThreadSubscriptions ");
            sqlCommand.Append("WHERE ThreadID = :ThreadID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            byte locked = 1;
            if (!isLocked) { locked = 0; }

            byte isQ = 1;
            if (!isQuestion) { isQ = 0; }

            byte inMap = 1;
            if (!includeInSiteMap) { inMap = 0; }

            byte noIndex = 1;
            if (!setNoIndexMeta) { noIndex = 0; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE	 mp_ForumThreads ");
            sqlCommand.Append("SET	ForumID = :ForumID, ");
            sqlCommand.Append("ThreadSubject = :ThreadSubject, ");
            sqlCommand.Append("SortOrder = :SortOrder, ");

            sqlCommand.Append("IsQuestion = :IsQuestion, ");
            sqlCommand.Append("IncludeInSiteMap = :IncludeInSiteMap, ");
            sqlCommand.Append("SetNoIndexMeta = :SetNoIndexMeta, ");
            sqlCommand.Append("PTitleOverride = :PTitleOverride, ");

            sqlCommand.Append("ModStatus = :ModStatus, ");
            sqlCommand.Append("ThreadType = :ThreadType, ");
            sqlCommand.Append("AssignedTo = :AssignedTo, ");
            sqlCommand.Append("LockedBy = :LockedBy, ");
            sqlCommand.Append("LockedReason = :LockedReason, ");
            sqlCommand.Append("LockedUtc = :LockedUtc, ");

            sqlCommand.Append("IsLocked = :IsLocked ");


            sqlCommand.Append("WHERE ThreadID = :ThreadID ;");

            SqliteParameter[] arParams = new SqliteParameter[15];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = forumId;

            arParams[2] = new SqliteParameter(":ThreadSubject", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = threadSubject;

            arParams[3] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortOrder;

            arParams[4] = new SqliteParameter(":IsLocked", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = locked;

            arParams[5] = new SqliteParameter(":IsQuestion", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = isQ;

            arParams[6] = new SqliteParameter(":IncludeInSiteMap", DbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = inMap;

            arParams[7] = new SqliteParameter(":SetNoIndexMeta", DbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = noIndex;

            arParams[8] = new SqliteParameter(":PTitleOverride", DbType.String, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = pageTitleOverride;

            arParams[9] = new SqliteParameter(":ModStatus", DbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = modStatus;

            arParams[10] = new SqliteParameter(":ThreadType", DbType.String, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = threadType;

            arParams[11] = new SqliteParameter(":AssignedTo", DbType.String, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = assignedTo.ToString();

            arParams[12] = new SqliteParameter(":LockedBy", DbType.String, 36);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = lockedBy.ToString();

            arParams[13] = new SqliteParameter(":LockedReason", DbType.String, 100);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = lockedReason;

            arParams[14] = new SqliteParameter(":LockedUtc", DbType.DateTime);
            arParams[14].Direction = ParameterDirection.Input;
            

            if (lockedUtc < DateTime.MaxValue)
            {
                arParams[14].Value = lockedUtc;
            }
            else
            {
                arParams[14].Value = DBNull.Value;
            }

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("SET MostRecentPostUserID = :MostRecentPostUserID, ");
            sqlCommand.Append("TotalReplies = TotalReplies + 1, ");
            sqlCommand.Append("MostRecentPostDate = :MostRecentPostDate ");
            sqlCommand.Append("WHERE ThreadID = :ThreadID ;");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqliteParameter(":MostRecentPostUserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostUserId;

            arParams[2] = new SqliteParameter(":MostRecentPostDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = mostRecentPostDate;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(), sqlCommand.ToString(), arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadDecrementReplyStats(int threadId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT UserID, PostDate ");
            sqlCommand.Append("FROM mp_ForumPosts ");
            sqlCommand.Append("WHERE ThreadID = :ThreadID ");
            sqlCommand.Append("ORDER BY PostID DESC ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int userID = 0;
            DateTime postDate = DateTime.Now;

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    userID = Convert.ToInt32(reader["UserID"]);
                    postDate = Convert.ToDateTime(reader["PostDate"]);
                }
            }

            sqlCommand = new StringBuilder();


            sqlCommand.Append("UPDATE mp_ForumThreads ");
            sqlCommand.Append("SET MostRecentPostUserID = :MostRecentPostUserID, ");
            sqlCommand.Append("TotalReplies = TotalReplies - 1, ");
            sqlCommand.Append("MostRecentPostDate = :MostRecentPostDate ");
            sqlCommand.Append("WHERE ThreadID = :ThreadID ;");

            arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqliteParameter(":MostRecentPostUserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userID;

            arParams[2] = new SqliteParameter(":MostRecentPostDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = postDate;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadUpdateViewStats(int threadId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_ForumThreads ");
            sqlCommand.Append("SET TotalViews = TotalViews + 1 ");
            sqlCommand.Append("WHERE ThreadID = :ThreadID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader ForumThreadGetPosts(int threadId, int pageNumber)
        {

            StringBuilder sqlCommand = new StringBuilder();

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int postsPerPage = 10;

            sqlCommand.Append("SELECT	f.PostsPerPage As PostsPerPage ");
            sqlCommand.Append("FROM		mp_ForumThreads ft ");
            sqlCommand.Append("JOIN		mp_Forums f ");
            sqlCommand.Append("ON		ft.ForumID = f.ItemID ");
            sqlCommand.Append("WHERE	ft.ThreadID = :ThreadID ;");

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {

                if (reader.Read())
                {
                    postsPerPage = Convert.ToInt32(reader["PostsPerPage"]);
                }
            }

            sqlCommand = new StringBuilder();
            int currentPageMaxThreadSequence = postsPerPage * pageNumber;
            int beginSequence = 0;
           
            if (currentPageMaxThreadSequence > postsPerPage)
            {
                beginSequence = currentPageMaxThreadSequence - postsPerPage;

            }


            sqlCommand.Append("SELECT	p.*, ");
            sqlCommand.Append("ft.ForumID As ForumID, ");
            sqlCommand.Append("ft.IsLocked As IsLocked, ");
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
            sqlCommand.Append("WHERE ft.ThreadID = :ThreadID ");

            sqlCommand.Append("ORDER BY	p.SortOrder, p.PostID ");
            sqlCommand.Append("LIMIT " + postsPerPage + " ");
            sqlCommand.Append("OFFSET " + beginSequence + " ; ");

            arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqliteParameter(":PageNumber", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetPosts(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();

            SqliteParameter[] arParams;

            sqlCommand.Append("SELECT	p.*, ");
            sqlCommand.Append("ft.ForumID As ForumID, ");
            sqlCommand.Append("ft.IsLocked As IsLocked, ");
            // TODO:
            //using 'Guest' here is not culture neutral, need to pass in a label
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
            sqlCommand.Append("COALESCE(up.Name, 'Guest') As PostAuthor, ");
            sqlCommand.Append("COALESCE(up.Email, '') As AuthorEmail, ");
            //sqlCommand.Append("up.TotalPosts As PostAuthorTotalPosts, ");
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

            sqlCommand.Append("WHERE ft.ThreadID = :ThreadID ");

            sqlCommand.Append("ORDER BY	p.PostID  ;");

            arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetPostsReverseSorted(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT	p.*, ");
            
            sqlCommand.Append("ft.ForumID As ForumID, ");
            sqlCommand.Append("ft.IsLocked As IsLocked, ");
            // TODO:
            //using 'Guest' here is not culture neutral, need to pass in a label
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
            sqlCommand.Append("COALESCE(up.Name, 'Guest') As PostAuthor, ");
            sqlCommand.Append("COALESCE(up.Email, '') As AuthorEmail, ");
            //sqlCommand.Append("up.TotalPosts As PostAuthorTotalPosts, ");
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

            sqlCommand.Append("WHERE ft.ThreadID = :ThreadID ");

            sqlCommand.Append("ORDER BY p.ThreadSequence DESC  ;");

            SqliteParameter[] arParams;
            arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetPostsByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  fp.*, ");
            sqlCommand.Append("f.ModuleID As ModuleID, ");
            sqlCommand.Append("f.ItemID As ItemID, ");
            sqlCommand.Append("m.ModuleTitle As ModuleTitle, ");
            sqlCommand.Append("m.ViewRoles As ViewRoles, ");
            sqlCommand.Append("md.FeatureName As FeatureName, ");
            sqlCommand.Append("md.ResourceFile As ResourceFile ");
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
            sqlCommand.Append("p.SiteID = :SiteID ");
            sqlCommand.Append("AND pm.PageID = :PageID ");

            sqlCommand.Append(" ; ");
            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":PageID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetThreadsByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ft.*, ");
            sqlCommand.Append("f.ModuleID As ModuleID, ");
            sqlCommand.Append("f.ItemID As ItemID, ");
            sqlCommand.Append("m.ModuleTitle As ModuleTitle, ");
            sqlCommand.Append("m.ViewRoles As ViewRoles, ");
            sqlCommand.Append("md.FeatureName As FeatureName, ");
            sqlCommand.Append("md.ResourceFile As ResourceFile ");

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
            sqlCommand.Append("p.SiteID = :SiteID ");
            sqlCommand.Append("AND pm.PageID = :PageID ");

            sqlCommand.Append(" ; ");
            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":PageID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetPostsForRss(int siteId, int pageId, int moduleId, int itemId, int threadId, int maximumDays)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT		fp.*, ");
            sqlCommand.Append("ft.ThreadSubject As ThreadSubject, ");
            sqlCommand.Append("ft.ForumID As ForumID, ");
            sqlCommand.Append("p.PageID, ");
            sqlCommand.Append("p.AuthorizedRoles, ");
            sqlCommand.Append("m.ModuleID, ");
            sqlCommand.Append("m.ViewRoles, ");

            sqlCommand.Append("COALESCE(s.[Name],'Guest') as StartedBy, ");
            sqlCommand.Append("COALESCE(up.[Name], 'Guest') as PostAuthor, ");
            sqlCommand.Append("up.TotalPosts as PostAuthorTotalPosts,");
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

            sqlCommand.Append("WHERE	p.SiteID = :SiteID ");
            sqlCommand.Append("AND	(:PageID = -1 OR p.PageID = :PageID) ");
            sqlCommand.Append("AND	(:ModuleID = -1 OR m.ModuleID = :ModuleID) ");
            sqlCommand.Append("AND	(:ItemID = -1 OR f.ItemID = :ItemID) ");
            sqlCommand.Append("AND	(:ThreadID = -1 OR ft.ThreadID = :ThreadID) ");
            sqlCommand.Append("AND	(:MaximumDays = -1 OR datetime(fp.PostDate) >= datetime('now', '-" + maximumDays + " days')) ");

            sqlCommand.Append("ORDER BY	fp.PostDate DESC ; ");

            SqliteParameter[] arParams = new SqliteParameter[6];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":PageID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            arParams[2] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleId;

            arParams[3] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = itemId;

            arParams[4] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = threadId;

            arParams[5] = new SqliteParameter(":MaximumDays", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = maximumDays;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetSubscriber(Guid subGuid)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_ForumThreadSubscriptions ");

            sqlCommand.Append("WHERE SubGuid = :SubGuid ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SubGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static DataSet ForumThreadGetSubscribers(int forumId, int threadId, int currentPostUserId, bool includeCurrentUser)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT u.Email As Email, ");
            sqlCommand.Append("COALESCE(fts.ThreadSubscriptionID, -1) AS ThreadSubID, ");
            sqlCommand.Append("COALESCE(fs.SubscriptionID, -1) AS ForumSubID, ");
            sqlCommand.Append("COALESCE(fts.SubGuid, '00000000-0000-0000-0000-000000000000') AS ThreadSubGuid, ");
            sqlCommand.Append("COALESCE(fs.SubGuid, '00000000-0000-0000-0000-000000000000') AS ForumSubGuid ");

            sqlCommand.Append("FROM	mp_Users u ");

            sqlCommand.Append("LEFT OUTER JOIN mp_ForumThreadSubscriptions fts ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("fts.UserID = u.UserID ");
            sqlCommand.Append("AND fts.ThreadID = :ThreadID ");
            sqlCommand.Append("AND fts.UnSubscribeDate IS NULL ");

            sqlCommand.Append("LEFT OUTER JOIN mp_ForumSubscriptions fs ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("fs.UserID = u.UserID ");
            sqlCommand.Append("AND fs.ForumID = :ForumID ");
            sqlCommand.Append("AND fs.UnSubscribeDate IS NULL ");

            sqlCommand.Append("WHERE  ");
            if (!includeCurrentUser)
            {
                sqlCommand.Append("u.UserID <> :CurrentPostUserID ");
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

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ForumID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadId;

            arParams[2] = new SqliteParameter(":CurrentPostUserID", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentPostUserId;

            return SqliteHelper.ExecuteDataset(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static bool ForumThreadAddSubscriber(int threadId, int userId, Guid subGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT COUNT(*) As SubscriptionCount ");
            sqlCommand.Append("FROM mp_ForumThreadSubscriptions  ");
            sqlCommand.Append("WHERE ThreadID = :ThreadID AND UserID = :UserID AND UnSubscribeDate IS NULL ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int subscriptionCount = 0;
            int rowsAffected = -1;

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
                sqlCommand.Append("SET UnSubscribeDate = :UnSubscribeDate ");
                
                sqlCommand.Append("WHERE ThreadID = :ThreadID AND UserID = :UserID AND UnSubscribeDate IS NULL ;");

                arParams = new SqliteParameter[3];

                arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = threadId;

                arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = userId;

                arParams[2] = new SqliteParameter(":UnSubscribeDate", DbType.DateTime);
                arParams[2].Direction = ParameterDirection.Input;
                arParams[2].Value = DateTime.UtcNow;

                rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append(":ThreadID, ");
            sqlCommand.Append(":UserID, ");
            sqlCommand.Append(":SubGuid, ");
            sqlCommand.Append(":SubscribeDate ");
            sqlCommand.Append(") ;");

            

            arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new SqliteParameter(":SubscribeDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            arParams[3] = new SqliteParameter(":SubGuid", DbType.String, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = subGuid.ToString();

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadUnSubscribe(Guid subGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_ForumThreadSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = :CurrentTime ");
            sqlCommand.Append("WHERE SubGuid = :SubGuid  ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SubGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subGuid.ToString();

            arParams[1] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        public static bool ForumThreadUNSubscribe(int threadId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_ForumThreadSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = datetime('now','localtime') ");
            sqlCommand.Append("WHERE ThreadID = :ThreadID AND UserID = :UserID ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadUnsubscribeAll(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_ForumThreadSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = datetime('now','localtime') ");
            sqlCommand.Append("WHERE UserID = :UserID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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

            byte approve = 1;
            if (!approved)
            {
                approve = 0;
            }

            int intNotificationSent = notificationSent ? 1 : 0;

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT COALESCE(Max(ThreadSequence) + 1,1) As ThreadSequence FROM mp_ForumPosts WHERE ThreadID = :ThreadID ; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int threadSequence = 1;

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    threadSequence = Convert.ToInt32(reader["ThreadSequence"]);
                }
            }

            sqlCommand = new StringBuilder();

            sqlCommand.Append("INSERT INTO mp_ForumPosts ( ");
            sqlCommand.Append("ThreadID, ");
            sqlCommand.Append("Subject, ");
            sqlCommand.Append("Post, ");
            sqlCommand.Append("PostDate, ");
            sqlCommand.Append("Approved, ");
            sqlCommand.Append("UserID, ");

            sqlCommand.Append("AnswerVotes, ");
            sqlCommand.Append("PostGuid, ");
            sqlCommand.Append("ApprovedBy, ");
            sqlCommand.Append("ApprovedUtc, ");
            sqlCommand.Append("UserIp, ");

            sqlCommand.Append("NotificationSent, ");
            sqlCommand.Append("ModStatus, ");

            sqlCommand.Append("ThreadSequence ");

            sqlCommand.Append(" ) ");

            sqlCommand.Append("VALUES (");
            sqlCommand.Append(" :ThreadID , ");
            sqlCommand.Append(" :Subject  , ");
            sqlCommand.Append(" :Post, ");
            sqlCommand.Append(" :PostDate, ");
            sqlCommand.Append(" :Approved , ");
            sqlCommand.Append(" :UserID , ");

            sqlCommand.Append("0, ");
            sqlCommand.Append(":PostGuid, ");
            sqlCommand.Append(":ApprovedBy, ");
            sqlCommand.Append(":ApprovedUtc, ");
            sqlCommand.Append(":UserIp, ");

            sqlCommand.Append(":NotificationSent, ");
            sqlCommand.Append(":ModStatus, ");

            sqlCommand.Append(" :ThreadSequence  ");

            sqlCommand.Append(");");
            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            arParams = new SqliteParameter[13];

            arParams[0] = new SqliteParameter(":ThreadID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new SqliteParameter(":Subject", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = subject;

            arParams[2] = new SqliteParameter(":Post", DbType.Object);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = post;

            arParams[3] = new SqliteParameter(":Approved", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = approve;

            arParams[4] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userId;

            arParams[5] = new SqliteParameter(":ThreadSequence", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = threadSequence;

            arParams[6] = new SqliteParameter(":PostDate", DbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = postDate;

            arParams[7] = new SqliteParameter(":PostGuid", DbType.String, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = postGuid.ToString();

            arParams[8] = new SqliteParameter(":ApprovedBy", DbType.String, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = approvedBy.ToString();

            arParams[9] = new SqliteParameter(":ApprovedUtc", DbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            if (approvedUtc > DateTime.MinValue)
            {
                arParams[9].Value = approvedUtc;
            }
            else
            {
                arParams[9].Value = DBNull.Value;
            }

            arParams[10] = new SqliteParameter(":UserIp", DbType.String, 50);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = userIp;

            arParams[11] = new SqliteParameter(":NotificationSent", DbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = intNotificationSent;

            arParams[12] = new SqliteParameter(":ModStatus", DbType.Int32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = modStatus;

            int newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return newID;

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
            byte approve = 1;
            if (!approved)
            {
                approve = 0;
            }

            int intNotificationSent = notificationSent ? 1 : 0;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_ForumPosts ");
            sqlCommand.Append("SET Subject = :Subject, ");
            sqlCommand.Append("Post = :Post, ");
            sqlCommand.Append("SortOrder = :SortOrder, ");
            sqlCommand.Append("ApprovedBy = :ApprovedBy, ");
            sqlCommand.Append("ApprovedUtc = :ApprovedUtc, ");

            sqlCommand.Append("NotificationSent = :NotificationSent, ");
            sqlCommand.Append("ModStatus = :ModStatus, ");

            sqlCommand.Append("Approved = :Approved ");
            sqlCommand.Append("WHERE PostID = :PostID ;");

            SqliteParameter[] arParams = new SqliteParameter[9];

            arParams[0] = new SqliteParameter(":PostID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            arParams[1] = new SqliteParameter(":Subject", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = subject;

            arParams[2] = new SqliteParameter(":Post", DbType.Object);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = post;

            arParams[3] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortOrder;

            arParams[4] = new SqliteParameter(":Approved", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = approve;

            arParams[5] = new SqliteParameter(":ApprovedBy", DbType.String, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = approvedBy.ToString();

            arParams[6] = new SqliteParameter(":ApprovedUtc", DbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            if (approvedUtc > DateTime.MinValue)
            {
                arParams[6].Value = approvedUtc;
            }
            else
            {
                arParams[6].Value = DBNull.Value;
            }

            arParams[7] = new SqliteParameter(":NotificationSent", DbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intNotificationSent;

            arParams[8] = new SqliteParameter(":ModStatus", DbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = modStatus;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("SET ThreadSequence = :ThreadSequence ");
            sqlCommand.Append("WHERE PostID = :PostID ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":PostID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            arParams[1] = new SqliteParameter(":ThreadSequence", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadSequence;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumPostDelete(int postId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumPosts ");
            sqlCommand.Append("WHERE PostID = :PostID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":PostID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }






    }
}
