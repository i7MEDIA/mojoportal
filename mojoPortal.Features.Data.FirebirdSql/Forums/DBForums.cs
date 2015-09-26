// Author:					Joe Audette
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
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using FirebirdSql.Data.FirebirdClient;

namespace mojoPortal.Data
{
    public static class DBForums
    {
        
        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

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

            int intIsModerated;
            if (isModerated)
            {
                intIsModerated = 1;
            }
            else
            {
                intIsModerated = 0;
            }

            int intIsActive;
            if (isActive)
            {
                intIsActive = 1;
            }
            else
            {
                intIsActive = 0;
            }

            int intAllowAnonymousPosts;
            if (allowAnonymousPosts)
            {
                intAllowAnonymousPosts = 1;
            }
            else
            {
                intAllowAnonymousPosts = 0;
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

            FbParameter[] arParams = new FbParameter[26];

            arParams[0] = new FbParameter(":ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter(":CreatedDate", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            arParams[2] = new FbParameter(":CreatedBy", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userId;

            arParams[3] = new FbParameter(":Title", FbDbType.VarChar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = title;

            arParams[4] = new FbParameter(":Description", FbDbType.VarChar);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = description;

            arParams[5] = new FbParameter(":IsModerated", FbDbType.SmallInt);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intIsModerated;

            arParams[6] = new FbParameter(":IsActive", FbDbType.SmallInt);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = intIsActive;

            arParams[7] = new FbParameter(":SortOrder", FbDbType.Integer);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = sortOrder;

            arParams[8] = new FbParameter(":ThreadCount", FbDbType.Integer);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = 0;

            arParams[9] = new FbParameter(":PostCount", FbDbType.Integer);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = 0;

            arParams[10] = new FbParameter(":MOSTRECENTPOSTUSERID", FbDbType.Integer);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = -1;

            arParams[11] = new FbParameter(":PostsPerPage", FbDbType.Integer);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = postsPerPage;

            arParams[12] = new FbParameter(":ThreadsPerPage", FbDbType.Integer);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = threadsPerPage;

            arParams[13] = new FbParameter(":AllowAnonymousPosts", FbDbType.SmallInt);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = intAllowAnonymousPosts;

            arParams[14] = new FbParameter(":ForumGuid", FbDbType.VarChar, 36);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = forumGuid.ToString();

            arParams[15] = new FbParameter(":RolesThatCanPost", FbDbType.VarChar);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = rolesThatCanPost;

            arParams[16] = new FbParameter(":RolesThatCanModerate", FbDbType.VarChar);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = rolesThatCanModerate;

            arParams[17] = new FbParameter(":ModeratorNotifyEmail", FbDbType.VarChar);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = moderatorNotifyEmail;

            arParams[18] = new FbParameter(":IncludeInGoogleMap", FbDbType.Integer);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = intIncludeInGoogleMap;

            arParams[19] = new FbParameter(":AddNoIndexMeta", FbDbType.Integer);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = intAddNoIndexMeta;

            arParams[20] = new FbParameter(":Closed", FbDbType.Integer);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = intClosed;

            arParams[21] = new FbParameter(":Visible", FbDbType.Integer);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = intVisible;

            arParams[22] = new FbParameter(":RequireModeration", FbDbType.Integer);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = intRequireModeration;

            arParams[23] = new FbParameter(":RequireModForNotify", FbDbType.Integer);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = intRequireModForNotify;

            arParams[24] = new FbParameter(":AllowTrustedDirectPosts", FbDbType.Integer);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = intAllowTrustedDirectPosts;

            arParams[25] = new FbParameter(":AllowTrustedDirectNotify", FbDbType.Integer);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = intAllowTrustedDirectNotify;



            int newID = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_FORUMS_INSERT ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
                arParams));

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

            #region bit conversion

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
            sqlCommand.Append("SET	Title = @Title, ");
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

            sqlCommand.Append("WHERE ItemID = @ItemID ;");

            FbParameter[] arParams = new FbParameter[20];


            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new FbParameter("@Title", FbDbType.VarChar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new FbParameter("@Description", FbDbType.VarChar);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = description;

            arParams[3] = new FbParameter("@IsModerated", FbDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moderated;

            arParams[4] = new FbParameter("@IsActive", FbDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = active;

            arParams[5] = new FbParameter("@SortOrder", FbDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sortOrder;

            arParams[6] = new FbParameter("@PostsPerPage", FbDbType.Integer);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = postsPerPage;

            arParams[7] = new FbParameter("@ThreadsPerPage", FbDbType.Integer);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = threadsPerPage;

            arParams[8] = new FbParameter("@AllowAnonymousPosts", FbDbType.Integer);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = allowAnonymous;

            arParams[9] = new FbParameter("@RolesThatCanPost", FbDbType.VarChar);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = rolesThatCanPost;

            arParams[10] = new FbParameter("@RolesThatCanModerate", FbDbType.VarChar);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = rolesThatCanModerate;

            arParams[11] = new FbParameter("@ModeratorNotifyEmail", FbDbType.VarChar);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = moderatorNotifyEmail;

            arParams[12] = new FbParameter("@IncludeInGoogleMap", FbDbType.Integer);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = intIncludeInGoogleMap;

            arParams[13] = new FbParameter("@AddNoIndexMeta", FbDbType.Integer);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = intAddNoIndexMeta;

            arParams[14] = new FbParameter("@Closed", FbDbType.Integer);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = intClosed;

            arParams[15] = new FbParameter("@Visible", FbDbType.Integer);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = intVisible;

            arParams[16] = new FbParameter("@RequireModeration", FbDbType.Integer);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = intRequireModeration;

            arParams[17] = new FbParameter("@RequireModForNotify", FbDbType.Integer);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = intRequireModForNotify;

            arParams[18] = new FbParameter("@AllowTrustedDirectPosts", FbDbType.Integer);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = intAllowTrustedDirectPosts;

            arParams[19] = new FbParameter("@AllowTrustedDirectNotify", FbDbType.Integer);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = intAllowTrustedDirectNotify;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Forums ");
            sqlCommand.Append("WHERE ItemID = @ItemID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByModule(int moduleId)
        {
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumPosts WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID  = @ModuleID) );");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumThreadSubscriptions WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID  = @ModuleID) );");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID = @ModuleID);");
            
            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumSubscriptions WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID = @ModuleID) ;");
            
            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Forums WHERE ModuleID = @ModuleID;");
            
            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(int siteId)
        {
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumPosts WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID IN  (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID)) );");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumThreadSubscriptions WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID IN  (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID)) );");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID IN  (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID));");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumSubscriptions WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID IN  (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID)) ;");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Forums WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID);");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }



        public static IDataReader GetForums(int moduleId, int userId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT f.*, ");
            sqlCommand.Append("u.Name As MostRecentPostUser, ");

            sqlCommand.Append(" CASE ");
            sqlCommand.Append(" WHEN s.SubscribeDate IS NOT NULL AND s.UnSubscribeDate IS NULL THEN 1 ");
            sqlCommand.Append(" ELSE 0 END AS Subscribed, ");
            sqlCommand.Append("(SELECT COUNT(*) FROM mp_ForumSubscriptions fs WHERE fs.ForumID = f.ItemID AND fs.UnSubscribeDate IS NULL) As SubscriberCount  ");
            
            sqlCommand.Append("FROM	mp_Forums f ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON f.MostRecentPostUserID = u.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_ForumSubscriptions s ");
            sqlCommand.Append("ON f.ItemID = s.ForumID AND s.UserID = @UserID AND s.UnSubscribeDate IS NULL ");

            sqlCommand.Append("WHERE f.ModuleID	= @ModuleID ");
            sqlCommand.Append("AND f.IsActive = 1 ");

            sqlCommand.Append("ORDER BY	f.SortOrder, f.ItemID ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            return FBSqlHelper.ExecuteReader(
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
            sqlCommand.Append("WHERE f.ItemID	= @ItemID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return FBSqlHelper.ExecuteReader(
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
            sqlCommand.Append("SET MostRecentPostDate = @MostRecentPostDate, ");
            sqlCommand.Append("MostRecentPostUserID = @MostRecentPostUserID, ");
            sqlCommand.Append("PostCount = PostCount + 1 ");

            sqlCommand.Append("WHERE ItemID = @ItemID ;");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new FbParameter("@MostRecentPostUserID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostUserId;

            arParams[2] = new FbParameter("@MostRecentPostDate", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = mostRecentPostDate;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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
                sqlCommand.Append("WHERE UserID = @UserID ");
            }
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("WHERE ItemID = @ItemID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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

            sqlCommand.Append("WHERE ItemID = @ItemID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ForumID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            sqlCommand.Append("SELECT FIRST 1 ");
            sqlCommand.Append("MostRecentPostDate, ");
            sqlCommand.Append("MostRecentPostUserID ");
            sqlCommand.Append("FROM mp_ForumThreads ");
            sqlCommand.Append("WHERE ForumID = @ForumID ");
            sqlCommand.Append("ORDER BY MostRecentPostDate DESC ");
            sqlCommand.Append(" ;");

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
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
            sqlCommand.Append("WHERE ForumID = @ForumID ;");

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
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
            sqlCommand.Append("MostRecentPostDate = @MostRecentPostDate,	 ");
            sqlCommand.Append("MostRecentPostUserID = @MostRecentPostUserID,	 ");
            sqlCommand.Append("PostCount = @PostCount	 ");
            sqlCommand.Append("WHERE ItemID = @ForumID ;");

            arParams = new FbParameter[4];

            arParams[0] = new FbParameter("@ForumID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new FbParameter("@MostRecentPostDate", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostDate;

            arParams[2] = new FbParameter("@MostRecentPostUserID", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = mostRecentPostUserID;

            arParams[3] = new FbParameter("@PostCount", FbDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = postCount;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("WHERE ItemID = @ItemID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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

            sqlCommand.Append("WHERE ItemID = @ItemID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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

            sqlCommand.Append("WHERE m.SiteID = @SiteID AND ft.ThreadID IN (Select DISTINCT ThreadID FROM mp_ForumPosts WHERE mp_ForumPosts.UserID = @UserID) ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteId;

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
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
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString() + " ");
            sqlCommand.Append("	SKIP " + pageLowerBound.ToString() + " ");

            sqlCommand.Append(" t.*, ");
            sqlCommand.Append("f.Title As Forum, ");
            sqlCommand.Append("f.ModuleID, ");
            sqlCommand.Append("(SELECT FIRST 1 PageID FROM mp_PageModules WHERE mp_PageModules.ModuleID = f.ModuleID AND (PublishEndDate IS NULL OR PublishEndDate > @CurrentDate)) As PageID, ");
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

            sqlCommand.Append("WHERE m.SiteID = @SiteID AND t.ThreadID IN (Select DISTINCT ThreadID FROM mp_ForumPosts WHERE mp_ForumPosts.UserID = @UserID) ");

            sqlCommand.Append("ORDER BY	t.MostRecentPostDate DESC  ");

            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new FbParameter("@CurrentDate", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            arParams[2] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteId;


            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
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

            int skip = pageSize * (pageNumber - 1);

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT	 ");
            sqlCommand.Append(" FIRST " + pageSize.ToString() + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append(" SKIP " + skip.ToString() + "  ");
            }

            sqlCommand.Append(" t.*, ");
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("s.Name As StartedBy ");
            sqlCommand.Append("FROM	mp_ForumThreads t ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON t.MostRecentPostUserID = u.UserID ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users s ");
            sqlCommand.Append("ON t.StartedByUserID = s.UserID ");

            sqlCommand.Append("WHERE t.ForumID = @ForumID ");

            sqlCommand.Append("ORDER BY t.SortOrder, t.MostRecentPostDate DESC ; ");


            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ForumID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            //arParams[1] = new FbParameter("@PageNumber", FbDbType.Integer);
            //arParams[1].Direction = ParameterDirection.Input;
            //arParams[1].Value = PageNumber;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int ForumThreadGetPostCount(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            sqlCommand.Append("SELECT COUNT(*) FROM mp_ForumPosts WHERE ThreadID = @ThreadID ");

            int count = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
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
            sqlCommand.Append("ForumID = @ForumID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("UnSubscribeDate IS NULL");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ForumID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
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
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
         
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
            sqlCommand.Append("fs.ForumID = @ForumID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("fs.UnSubscribeDate IS NULL ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("u.Name  ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ForumID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static bool AddSubscriber(int forumId, int userId, Guid subGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT COUNT(*) As SubscriptionCount ");
            sqlCommand.Append("FROM mp_ForumSubscriptions  ");
            sqlCommand.Append("WHERE ForumID = @ForumID AND UserID = @UserID AND UnSubscribeDate IS NULL ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ForumID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int subscriptionCount = 0;
            int rowsAffected = -1;

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
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

                arParams = new FbParameter[3];

                arParams[0] = new FbParameter("@ForumID", FbDbType.Integer);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = forumId;

                arParams[1] = new FbParameter("@UserID", FbDbType.Integer);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = userId;

                arParams[2] = new FbParameter("@SubscribeDate", FbDbType.TimeStamp);
                arParams[2].Direction = ParameterDirection.Input;
                arParams[2].Value = DateTime.UtcNow;

                sqlCommand.Append("UPDATE mp_ForumSubscriptions ");
                sqlCommand.Append("SET UnSubscribeDate = @SubscribeDate ");
               
                sqlCommand.Append("WHERE ForumID = @ForumID AND UserID = @UserID AND UnSubscribeDate IS NULL ;");

                rowsAffected = FBSqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("@ForumID, ");
            sqlCommand.Append("@UserID, ");
            sqlCommand.Append("@SubGuid, ");
            sqlCommand.Append("@SubscribeDate");
            sqlCommand.Append(") ;");


            arParams = new FbParameter[4];

            arParams[0] = new FbParameter("@ForumID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new FbParameter("@SubscribeDate", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            arParams[3] = new FbParameter("@SubGuid", FbDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = subGuid.ToString();

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("SubscriptionID = @SubscriptionID ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SubscriptionID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subscriptionId;


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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

            sqlCommand.Append("WHERE SubGuid = @SubGuid ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SubGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static bool Unsubscribe(Guid subGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_ForumSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = @UnSubscribeDate ");
            sqlCommand.Append("WHERE SubGuid = @SubGuid ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SubGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subGuid.ToString();

            arParams[1] = new FbParameter("@UnSubscribeDate", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Unsubscribe(int forumId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_ForumSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = @UnSubscribeDate ");
            sqlCommand.Append("WHERE ForumID = @ForumID AND UserID = @UserID ;");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ForumID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new FbParameter("@UnSubscribeDate", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new FbParameter("@UnSubscribeDate", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumSubscriptionExists(int forumId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_ForumSubscriptions ");
            sqlCommand.Append("WHERE ForumID = @ForumID AND UserID = @UserID AND UnSubscribeDate IS NULL ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ForumID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int count = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
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
            sqlCommand.Append("WHERE ThreadID = @ThreadID AND UserID = @UserID AND UnSubscribeDate IS NULL ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int count = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
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
            sqlCommand.Append("AND ft.IncludeInSiteMap = 1 ");

            sqlCommand.Append("ORDER BY ft.ThreadID DESC ");

            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
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

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader ForumThreadGetPost(int postId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	fp.* ");
            sqlCommand.Append("FROM	mp_ForumPosts fp ");
            sqlCommand.Append("WHERE fp.PostID = @PostID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@PostID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            return FBSqlHelper.ExecuteReader(
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

            #region Bit Conversion
            int intIsLocked =0;
            if (isLocked) { intIsLocked = 1; }

            byte isQ = 1;
            if (!isQuestion) { isQ = 0; }

            byte inMap = 1;
            if (!includeInSiteMap) { inMap = 0; }

            byte noIndex = 1;
            if (!setNoIndexMeta) { noIndex = 0; }
            

            #endregion


            StringBuilder sqlCommand = new StringBuilder();
            int forumSequence = 1;
            sqlCommand.Append("SELECT COALESCE(Max(ForumSequence) + 1,1) As ForumSequence FROM mp_ForumThreads WHERE ForumID = @ForumID ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ForumID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    forumSequence = Convert.ToInt32(reader["ForumSequence"]);
                }
            }


            arParams = new FbParameter[18];

            arParams[0] = new FbParameter(":ForumID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new FbParameter(":ThreadSubject", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadSubject;

            arParams[2] = new FbParameter(":ThreadDate", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = threadDate;

            arParams[3] = new FbParameter(":TotalViews", FbDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = 0;

            arParams[4] = new FbParameter(":TotalReplies", FbDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = 0;

            arParams[5] = new FbParameter(":SortOrder", FbDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sortOrder;

            arParams[6] = new FbParameter(":IsLocked", FbDbType.SmallInt);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = intIsLocked;

            arParams[7] = new FbParameter(":ForumSequence", FbDbType.Integer);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = forumSequence;

            arParams[8] = new FbParameter(":MostRecentPostDate", FbDbType.TimeStamp);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = DateTime.UtcNow;

            arParams[9] = new FbParameter(":MostRecentPostUserID", FbDbType.Integer);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = startedByUserId;

            arParams[10] = new FbParameter(":StartedByUserID", FbDbType.Integer);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = startedByUserId;

            arParams[11] = new FbParameter(":ThreadGuid", FbDbType.VarChar, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = threadGuid.ToString();

            arParams[12] = new FbParameter(":IsQuestion", FbDbType.Integer);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = isQ;

            arParams[13] = new FbParameter(":IncludeInSiteMap", FbDbType.Integer);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = inMap;

            arParams[14] = new FbParameter(":SetNoIndexMeta", FbDbType.Integer);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = noIndex;

            arParams[15] = new FbParameter(":PTitleOverride", FbDbType.VarChar, 36);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = pageTitleOverride;

            arParams[16] = new FbParameter("@ModStatus", FbDbType.Integer);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = modStatus;

            arParams[17] = new FbParameter("@ThreadType", FbDbType.VarChar, 100);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = threadType;



            int newID = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_FORUMTHREADS_INSERT ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
                arParams));

            //sqlCommand = new StringBuilder();
            //sqlCommand.Append("INSERT INTO mp_ForumThreadSubscriptions (ThreadID, UserID) ");
            //sqlCommand.Append("    SELECT " + newID.ToString() + " , fs.UserID FROM mp_ForumSubscriptions fs ");
            //sqlCommand.Append("        WHERE fs.ForumID = @ForumID AND fs.SubscribeDate IS NOT NULL AND fs.UnSubscribeDate IS NULL;");

            //arParams = new FbParameter[1];

            //arParams[0] = new FbParameter("@ForumID", FbDbType.Integer);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = forumId;

            //FBSqlHelper.ExecuteNonQuery(
            //    GetConnectionString(),
            //    sqlCommand.ToString(),
            //    arParams);

            return newID;

        }

        public static bool ForumThreadDelete(int threadId)
        {
            ForumThreadDeletePosts(threadId);
            ForumThreadDeleteSubscriptions(threadId);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumThreads ");
            sqlCommand.Append("WHERE ThreadID = @ThreadID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadDeletePosts(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumPosts ");
            sqlCommand.Append("WHERE ThreadID = @ThreadID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadDeleteSubscriptions(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumThreadSubscriptions ");
            sqlCommand.Append("WHERE ThreadID = @ThreadID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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

            FbParameter[] arParams = new FbParameter[15];

            arParams[0] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new FbParameter("@ForumID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = forumId;

            arParams[2] = new FbParameter("@ThreadSubject", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = threadSubject;

            arParams[3] = new FbParameter("@SortOrder", FbDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortOrder;

            arParams[4] = new FbParameter("@IsLocked", FbDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = locked;

            arParams[5] = new FbParameter("@IsQuestion", FbDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = isQ;

            arParams[6] = new FbParameter("@IncludeInSiteMap", FbDbType.Integer);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = inMap;

            arParams[7] = new FbParameter("@SetNoIndexMeta", FbDbType.Integer);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = noIndex;

            arParams[8] = new FbParameter("@PTitleOverride", FbDbType.VarChar, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = pageTitleOverride;

            arParams[9] = new FbParameter("@ModStatus", FbDbType.Integer);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = modStatus;

            arParams[10] = new FbParameter("@ThreadType", FbDbType.VarChar, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = threadType;

            arParams[11] = new FbParameter("@AssignedTo", FbDbType.VarChar, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = assignedTo.ToString();

            arParams[12] = new FbParameter("@LockedBy", FbDbType.VarChar, 36);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = lockedBy.ToString();

            arParams[13] = new FbParameter("@LockedReason", FbDbType.VarChar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = lockedReason;

            arParams[14] = new FbParameter("@LockedUtc", FbDbType.TimeStamp);
            arParams[14].Direction = ParameterDirection.Input;
            if (lockedUtc < DateTime.MaxValue)
            {
                arParams[14].Value = lockedUtc;
            }
            else
            {
                arParams[14].Value = DBNull.Value;
            }

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("SET MostRecentPostUserID = @MostRecentPostUserID, ");
            sqlCommand.Append("TotalReplies = TotalReplies + 1, ");
            sqlCommand.Append("MostRecentPostDate = @MostRecentPostDate ");
            sqlCommand.Append("WHERE ThreadID = @ThreadID ;");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new FbParameter("@MostRecentPostUserID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostUserId;

            arParams[2] = new FbParameter("@MostRecentPostDate", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = mostRecentPostDate;


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(), sqlCommand.ToString(), arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadDecrementReplyStats(int threadId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT UserID, PostDate ");
            sqlCommand.Append("FROM mp_ForumPosts ");
            sqlCommand.Append("WHERE ThreadID = @ThreadID ");
            sqlCommand.Append("ORDER BY PostID DESC ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int userId = 0;
            DateTime postDate = DateTime.Now;

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    userId = Convert.ToInt32(reader["UserID"]);
                    postDate = Convert.ToDateTime(reader["PostDate"]);
                }
            }

            sqlCommand = new StringBuilder();


            sqlCommand.Append("UPDATE mp_ForumThreads ");
            sqlCommand.Append("SET MostRecentPostUserID = @MostRecentPostUserID, ");
            sqlCommand.Append("TotalReplies = TotalReplies - 1, ");
            sqlCommand.Append("MostRecentPostDate = @MostRecentPostDate ");
            sqlCommand.Append("WHERE ThreadID = @ThreadID ;");

            arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new FbParameter("@MostRecentPostUserID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new FbParameter("@MostRecentPostDate", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = postDate;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("WHERE ThreadID = @ThreadID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader ForumThreadGetPosts(
            int threadId,
            int pageNumber)
        {
            StringBuilder sqlCommand = new StringBuilder();

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int pageSize = 10;
            int totalRows = 0;

            sqlCommand.Append("SELECT	f.PostsPerPage ");
            sqlCommand.Append("FROM		mp_ForumThreads ft ");
            sqlCommand.Append("JOIN		mp_Forums f ");
            sqlCommand.Append("ON		ft.ForumID = f.ItemID ");
            sqlCommand.Append("WHERE	ft.ThreadID = @ThreadID ;");

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    pageSize = Convert.ToInt32(reader["PostsPerPage"]);
                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) As PostCount ");
            sqlCommand.Append("FROM		mp_ForumPosts fp ");
            sqlCommand.Append("WHERE	fp.ThreadID = @ThreadID ;");

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    totalRows = Convert.ToInt32(reader["PostCount"]);
                }
            }

            sqlCommand = new StringBuilder();

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

            int skip = pageSize * (pageNumber - 1);

            sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT	 ");
            sqlCommand.Append(" FIRST " + pageSize.ToString() + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append(" SKIP " + skip.ToString() + "  ");
            }

            sqlCommand.Append(" p.*, ");
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
            sqlCommand.Append("COALESCE(up.AvatarUrl, 'blank.gif') As PostAuthorAvatar, ");
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

            sqlCommand.Append("ORDER BY p.SortOrder, p.ThreadSequence ;");

            arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetPosts(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();

            FbParameter[] arParams;

            sqlCommand.Append("SELECT	p.*, ");
            sqlCommand.Append("ft.ForumID, ");
            sqlCommand.Append("ft.IsLocked, ");
            // TODO:
            //using 'Guest' here is not culture neutral, need to pass in a label
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
            sqlCommand.Append("COALESCE(up.Name, 'Guest') As PostAuthor, ");
            sqlCommand.Append("COALESCE(up.Email, '') As AuthorEmail, ");
            sqlCommand.Append("up.TotalPosts As PostAuthorTotalPosts, ");
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

            //sqlCommand.Append("ORDER BY	p.SortOrder, p.PostID DESC ;");
            sqlCommand.Append("ORDER BY p.PostID  ;");

            arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;



            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetPostsReverseSorted(int threadId)
        {

            StringBuilder sqlCommand = new StringBuilder();

            FbParameter[] arParams;

            sqlCommand.Append("SELECT	p.*, ");
            sqlCommand.Append("ft.ForumID, ");
            sqlCommand.Append("ft.IsLocked, ");
            // TODO:
            //using 'Guest' here is not culture neutral, need to pass in a label
            sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
            sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
            sqlCommand.Append("COALESCE(up.Name, 'Guest') As PostAuthor, ");
            sqlCommand.Append("COALESCE(up.Email, '') As AuthorEmail, ");
            sqlCommand.Append("up.TotalPosts As PostAuthorTotalPosts, ");
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

            //sqlCommand.Append("ORDER BY	p.SortOrder, p.PostID DESC ;");
            sqlCommand.Append("ORDER BY p.ThreadSequence DESC  ;");

            arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
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

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
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

            sqlCommand.Append("	mp_ForumThreads ft ");
            

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

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
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
            //sqlCommand.Append("AND	(@MaximumDays = -1 OR datediff('dd', now(), fp.PostDate) <= @MaximumDays) ");

            sqlCommand.Append("AND	( (@MaximumDays = -1) OR  ((now() - @MaximumDays) >= fp.PostDate )) ");


            sqlCommand.Append("ORDER BY	fp.PostDate DESC ; ");

            FbParameter[] arParams = new FbParameter[6];

            arParams[0] = new FbParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@PageID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            arParams[2] = new FbParameter("@ModuleID", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleId;

            arParams[3] = new FbParameter("@ItemID", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = itemId;

            arParams[4] = new FbParameter("@ThreadID", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = threadId;

            arParams[5] = new FbParameter("@MaximumDays", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = maximumDays;

            //String test = sqlCommand.ToString();

            return FBSqlHelper.ExecuteReader(
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
            sqlCommand.Append("AND fts.ThreadID = @ThreadID ");
            sqlCommand.Append("AND fts.UnSubscribeDate IS NULL ");

            sqlCommand.Append("LEFT OUTER JOIN mp_ForumSubscriptions fs ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("fs.UserID = u.UserID ");
            sqlCommand.Append("AND fs.ForumID = @ForumID ");
            sqlCommand.Append("AND fs.UnSubscribeDate IS NULL ");

            sqlCommand.Append("WHERE  ");

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

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ForumID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadId;

            arParams[2] = new FbParameter("@CurrentPostUserID", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentPostUserId;

            return FBSqlHelper.ExecuteDataset(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetSubscriber(Guid subGuid)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_ForumThreadSubscriptions ");

            sqlCommand.Append("WHERE SubGuid = @SubGuid ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SubGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static bool ForumThreadAddSubscriber(int threadId, int userId, Guid subGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT COUNT(*) As SubscriptionCount ");
            sqlCommand.Append("FROM mp_ForumThreadSubscriptions  ");
            sqlCommand.Append("WHERE ThreadID = @ThreadID AND UserID = @UserID AND UnSubscribeDate IS NULL ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int subscriptionCount = 0;

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
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

            arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new FbParameter("@SubscribeDate", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            int rowsAffected = -1;


            if (subscriptionCount > 0)
            {
                sqlCommand.Append("UPDATE mp_ForumThreadSubscriptions ");
                sqlCommand.Append("SET UnSubscribeDate = @SubscribeDate ");
                //sqlCommand.Append("UnSubscribeDate = Null ");
                sqlCommand.Append("WHERE ThreadID = @ThreadID AND UserID = @UserID AND UnSubscribeDate IS NULL ;");

                rowsAffected = FBSqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("@ThreadID, ");
            sqlCommand.Append("@UserID, ");
            sqlCommand.Append("@SubGuid, ");
            sqlCommand.Append("@SubscribeDate ");
            sqlCommand.Append(") ;");

           arParams = new FbParameter[4];

            arParams[0] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new FbParameter("@SubscribeDate", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            arParams[3] = new FbParameter("@SubGuid", FbDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = subGuid.ToString();
            

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SubGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subGuid.ToString();

            arParams[1] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadUNSubscribe(int threadId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_ForumThreadSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = now() ");
            sqlCommand.Append("WHERE ThreadID = @ThreadID AND UserID = @UserID ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumThreadUnsubscribeAll(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_ForumThreadSubscriptions ");
            sqlCommand.Append("SET UnSubscribeDate = now() ");
            sqlCommand.Append("WHERE UserID = @UserID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT COALESCE(Max(ThreadSequence) + 1,1) As ThreadSequence FROM mp_ForumPosts WHERE ThreadID = @ThreadID ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int threadSequence = 1;

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    threadSequence = Convert.ToInt32(reader["ThreadSequence"]);
                }
            }

            #region Bit Conversion
            int intApproved;
            if (approved)
            {
                intApproved = 1;
            }
            else
            {
                intApproved = 0;
            }

            int intNotificationSent = notificationSent ? 1 : 0;


            #endregion

            arParams = new FbParameter[14];

            arParams[0] = new FbParameter(":ThreadID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new FbParameter(":ThreadSequence", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadSequence;

            arParams[2] = new FbParameter(":Subject", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = subject;

            arParams[3] = new FbParameter(":PostDate", FbDbType.TimeStamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = postDate;

            arParams[4] = new FbParameter(":Approved", FbDbType.SmallInt);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = intApproved;

            arParams[5] = new FbParameter(":UserID", FbDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = userId;

            arParams[6] = new FbParameter(":SortOrder", FbDbType.Integer);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = 100;

            arParams[7] = new FbParameter(":Post", FbDbType.VarChar);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = post;

            arParams[8] = new FbParameter(":PostGuid", FbDbType.VarChar, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = postGuid.ToString();

            arParams[9] = new FbParameter(":ApprovedBy", FbDbType.VarChar, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = approvedBy.ToString();

            arParams[10] = new FbParameter(":ApprovedUtc", FbDbType.TimeStamp);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = approvedUtc;

            arParams[11] = new FbParameter(":UserIp", FbDbType.VarChar, 50);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = userIp;

            arParams[12] = new FbParameter(":NotificationSent", FbDbType.Integer);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = intNotificationSent;

            arParams[13] = new FbParameter(":ModStatus", FbDbType.Integer);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = modStatus;

            int newID = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_FORUMPOSTS_INSERT ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
                arParams));

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
            sqlCommand.Append("SET Subject = @Subject, ");
            sqlCommand.Append("Post = @Post, ");
            sqlCommand.Append("SortOrder = @SortOrder, ");
            sqlCommand.Append("ApprovedBy = @ApprovedBy, ");
            sqlCommand.Append("ApprovedUtc = @ApprovedUtc, ");

            sqlCommand.Append("NotificationSent = @NotificationSent, ");
            sqlCommand.Append("ModStatus = @ModStatus, ");

            sqlCommand.Append("Approved = @Approved ");

            sqlCommand.Append("WHERE PostID = @PostID ;");

            FbParameter[] arParams = new FbParameter[9];

            arParams[0] = new FbParameter("@PostID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            arParams[1] = new FbParameter("@Subject", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = subject;

            arParams[2] = new FbParameter("@Post", FbDbType.VarChar);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = post;

            arParams[3] = new FbParameter("@SortOrder", FbDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortOrder;

            arParams[4] = new FbParameter("@Approved", FbDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = approve;

            arParams[5] = new FbParameter("@ApprovedBy", FbDbType.VarChar, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = approvedBy.ToString();

            arParams[6] = new FbParameter("@ApprovedUtc", FbDbType.TimeStamp);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = approvedUtc;

            arParams[7] = new FbParameter("@NotificationSent", FbDbType.Integer);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intNotificationSent;

            arParams[8] = new FbParameter("@ModStatus", FbDbType.Integer);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = modStatus;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("SET ThreadSequence = @ThreadSequence ");
            sqlCommand.Append("WHERE PostID = @PostID ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@PostID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            arParams[1] = new FbParameter("@ThreadSequence", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadSequence;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumPostDelete(int postId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ForumPosts ");
            sqlCommand.Append("WHERE PostID = @PostID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@PostID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }






    }
}
