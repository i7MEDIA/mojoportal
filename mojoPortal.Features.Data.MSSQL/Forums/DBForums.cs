/// Author:					
/// Created:				2007-11-03
/// Last Modified:			2014-06-22
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;


namespace mojoPortal.Data
{
    
    public static class DBForums
    {
        
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Forums_Insert", 22);
            sph.DefineSqlParameter("@ForumGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, forumGuid);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 100, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@IsModerated", SqlDbType.Bit, ParameterDirection.Input, isModerated);
            sph.DefineSqlParameter("@IsActive", SqlDbType.Bit, ParameterDirection.Input, isActive);
            sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@PostsPerPage", SqlDbType.Int, ParameterDirection.Input, postsPerPage);
            sph.DefineSqlParameter("@ThreadsPerPage", SqlDbType.Int, ParameterDirection.Input, threadsPerPage);
            sph.DefineSqlParameter("@AllowAnonymousPosts", SqlDbType.Bit, ParameterDirection.Input, allowAnonymousPosts);

            sph.DefineSqlParameter("@RolesThatCanPost", SqlDbType.NVarChar, -1, ParameterDirection.Input, rolesThatCanPost);
            sph.DefineSqlParameter("@RolesThatCanModerate", SqlDbType.NVarChar, -1, ParameterDirection.Input, rolesThatCanModerate);
            sph.DefineSqlParameter("@ModeratorNotifyEmail", SqlDbType.NVarChar, -1, ParameterDirection.Input, moderatorNotifyEmail);
            sph.DefineSqlParameter("@IncludeInGoogleMap", SqlDbType.Bit, ParameterDirection.Input, includeInGoogleMap);
            sph.DefineSqlParameter("@AddNoIndexMeta", SqlDbType.Bit, ParameterDirection.Input, addNoIndexMeta);
            sph.DefineSqlParameter("@Closed", SqlDbType.Bit, ParameterDirection.Input, closed);
            sph.DefineSqlParameter("@Visible", SqlDbType.Bit, ParameterDirection.Input, visible);
            sph.DefineSqlParameter("@RequireModeration", SqlDbType.Bit, ParameterDirection.Input, requireModeration);
            sph.DefineSqlParameter("@RequireModForNotify", SqlDbType.Bit, ParameterDirection.Input, requireModForNotify);
            sph.DefineSqlParameter("@AllowTrustedDirectPosts", SqlDbType.Bit, ParameterDirection.Input, allowTrustedDirectPosts);
            sph.DefineSqlParameter("@AllowTrustedDirectNotify", SqlDbType.Bit, ParameterDirection.Input, allowTrustedDirectNotify);
            
            

            int newID = Convert.ToInt32(sph.ExecuteScalar());
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Forums_Update", 20);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 100, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@IsModerated", SqlDbType.Bit, ParameterDirection.Input, isModerated);
            sph.DefineSqlParameter("@IsActive", SqlDbType.Bit, ParameterDirection.Input, isActive);
            sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@PostsPerPage", SqlDbType.Int, ParameterDirection.Input, postsPerPage);
            sph.DefineSqlParameter("@ThreadsPerPage", SqlDbType.Int, ParameterDirection.Input, threadsPerPage);
            sph.DefineSqlParameter("@AllowAnonymousPosts", SqlDbType.Bit, ParameterDirection.Input, allowAnonymousPosts);

            sph.DefineSqlParameter("@RolesThatCanPost", SqlDbType.NVarChar, -1, ParameterDirection.Input, rolesThatCanPost);
            sph.DefineSqlParameter("@RolesThatCanModerate", SqlDbType.NVarChar, -1, ParameterDirection.Input, rolesThatCanModerate);
            sph.DefineSqlParameter("@ModeratorNotifyEmail", SqlDbType.NVarChar, -1, ParameterDirection.Input, moderatorNotifyEmail);
            sph.DefineSqlParameter("@IncludeInGoogleMap", SqlDbType.Bit, ParameterDirection.Input, includeInGoogleMap);
            sph.DefineSqlParameter("@AddNoIndexMeta", SqlDbType.Bit, ParameterDirection.Input, addNoIndexMeta);
            sph.DefineSqlParameter("@Closed", SqlDbType.Bit, ParameterDirection.Input, closed);
            sph.DefineSqlParameter("@Visible", SqlDbType.Bit, ParameterDirection.Input, visible);
            sph.DefineSqlParameter("@RequireModeration", SqlDbType.Bit, ParameterDirection.Input, requireModeration);
            sph.DefineSqlParameter("@RequireModForNotify", SqlDbType.Bit, ParameterDirection.Input, requireModForNotify);
            sph.DefineSqlParameter("@AllowTrustedDirectPosts", SqlDbType.Bit, ParameterDirection.Input, allowTrustedDirectPosts);
            sph.DefineSqlParameter("@AllowTrustedDirectNotify", SqlDbType.Bit, ParameterDirection.Input, allowTrustedDirectNotify);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool Delete(int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Forums_Delete", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteByModule(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Forums_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Forums_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }

        public static IDataReader GetForums(int moduleId, int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Forums_Select", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetForum(int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Forums_SelectOne", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            return sph.ExecuteReader();
        }

        //public static bool ForumUpdatePostStats(int forumId, int mostRecentPostUserId)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "mp_Forums_UpdatePostStats", 2);
        //    sph.DefineSqlParameter("@ForumID", SqlDbType.Int, ParameterDirection.Input, forumId);
        //    sph.DefineSqlParameter("@MostRecentPostUserID", SqlDbType.Int, ParameterDirection.Input, mostRecentPostUserId);
        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > -1);
        //}

        public static bool IncrementThreadCount(int forumId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Forums_IncrementThreadCount", 1);
            sph.DefineSqlParameter("@ForumID", SqlDbType.Int, ParameterDirection.Input, forumId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DecrementThreadCount(int forumId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Forums_DecrementThreadCount", 1);
            sph.DefineSqlParameter("@ForumID", SqlDbType.Int, ParameterDirection.Input, forumId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }


        public static int GetUserThreadCount(int userId, int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ForumThreads_CountByUser", 2);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return Convert.ToInt32(sph.ExecuteScalar());

        }

        public static IDataReader GetThreadPageByUser(
            int userId,
            int siteId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetUserThreadCount(userId, siteId);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ForumThreads_SelectPageByUser", 4);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();


        }


        public static bool UpdateUserStats(int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Forums_UpdateUserStats", 1);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }



        public static bool IncrementPostCount(
                int forumId,
                int mostRecentPostUserId,
                DateTime mostRecentPostDate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Forums_IncrementPostCount", 3);
            sph.DefineSqlParameter("@ForumID", SqlDbType.Int, ParameterDirection.Input, forumId);
            sph.DefineSqlParameter("@MostRecentPostUserID", SqlDbType.Int, ParameterDirection.Input, mostRecentPostUserId);
            sph.DefineSqlParameter("@MostRecentPostDate", SqlDbType.DateTime, ParameterDirection.Input, mostRecentPostDate);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool IncrementPostCount(int forumId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Forums_IncrementPostCountOnly", 1);
            sph.DefineSqlParameter("@ForumID", SqlDbType.Int, ParameterDirection.Input, forumId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DecrementPostCount(int forumId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Forums_DecrementPostCount", 1);
            sph.DefineSqlParameter("@ForumID", SqlDbType.Int, ParameterDirection.Input, forumId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool RecalculatePostStats(int forumId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Forums_RecalculatePostStats", 1);
            sph.DefineSqlParameter("@ForumID", SqlDbType.Int, ParameterDirection.Input, forumId);
            int rowsAffected = Convert.ToInt32(sph.ExecuteScalar());
            return (rowsAffected > 0);
        }

        public static int GetSubscriberCount(int forumId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ForumSubscriptions_GetCount", 1);
            sph.DefineSqlParameter("@ForumID", SqlDbType.Int, ParameterDirection.Input, forumId);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return count;

        }

        public static IDataReader GetSubscriberPage(
            int forumId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ForumSubscriptions_SelectPage", 3);
            sph.DefineSqlParameter("@ForumID", SqlDbType.Int, ParameterDirection.Input, forumId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

        public static bool AddSubscriber(int forumId, int userId, Guid subGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ForumSubscriptions_Insert", 4);
            sph.DefineSqlParameter("@ForumID", SqlDbType.Int, ParameterDirection.Input, forumId);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@SubGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, subGuid);
            sph.DefineSqlParameter("@SubscribeDate", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            int rowsAffected = Convert.ToInt32(sph.ExecuteScalar());
            return (rowsAffected > 0);
        }

        public static IDataReader GetForumSubscription(Guid subGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ForumSubscriptions_SelectOne", 1);
            sph.DefineSqlParameter("@SubGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, subGuid);
            return sph.ExecuteReader();
        }

        public static bool DeleteSubscription(int subscriptionId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ForumSubscriptions_Delete", 1);
            sph.DefineSqlParameter("@SubscriptionID", SqlDbType.Int, ParameterDirection.Input, subscriptionId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool Unsubscribe(Guid subGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ForumSubscriptions_Unsub", 2);
            sph.DefineSqlParameter("@SubGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, subGuid);
            sph.DefineSqlParameter("@CurrentUtc", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            int rowsAffected = Convert.ToInt32(sph.ExecuteNonQuery());
            return (rowsAffected > 0);
        }

        public static bool Unsubscribe(int forumId, int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Forumsubscriptions_UnSubscribe", 2);
            sph.DefineSqlParameter("@ForumID", SqlDbType.Int, ParameterDirection.Input, forumId);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            int rowsAffected = Convert.ToInt32(sph.ExecuteNonQuery());
            return (rowsAffected > 0);
        }

        public static bool UnsubscribeAll(int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ForumSubscriptions_UnsubscribeAll", 1);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            int rowsAffected = Convert.ToInt32(sph.ExecuteNonQuery());
            return (rowsAffected > 0);
        }

        public static bool ForumSubscriptionExists(int forumId, int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ForumSubscriptions_Exists", 2);
            sph.DefineSqlParameter("@ForumID", SqlDbType.Int, ParameterDirection.Input, forumId);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return (count > 0);
        }

        public static bool ForumThreadSubscriptionExists(int threadId, int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ForumThreadSubscriptions_Exists", 2);
            sph.DefineSqlParameter("@ThreadID", SqlDbType.Int, ParameterDirection.Input, threadId);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return (count > 0);
        }

        public static IDataReader GetThreadsForSiteMap(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ForumThreads_SelectForSiteMap", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetThreads(int forumId, int pageNumber)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ForumThreads_SelectByForumDesc_v2", 2);
            sph.DefineSqlParameter("@ForumID", SqlDbType.Int, ParameterDirection.Input, forumId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            return sph.ExecuteReader();
        }

        public static IDataReader ForumThreadGetThread(int threadId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ForumThreads_SelectOne", 1);
            sph.DefineSqlParameter("@ThreadID", SqlDbType.Int, ParameterDirection.Input, threadId);
            return sph.ExecuteReader();
        }

        public static IDataReader ForumThreadGetPost(int postId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ForumPosts_SelectOne", 1);
            sph.DefineSqlParameter("@PostID", SqlDbType.Int, ParameterDirection.Input, postId);
            return sph.ExecuteReader();
        }

        public static int ForumThreadGetPostCount(int threadId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ForumPosts_CountByThread", 1);
            sph.DefineSqlParameter("@ThreadID", SqlDbType.Int, ParameterDirection.Input, threadId);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return count;
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
            string threadType
            )
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ForumThreads_Insert", 13);
            sph.DefineSqlParameter("@ForumID", SqlDbType.Int, ParameterDirection.Input, forumId);
            sph.DefineSqlParameter("@ThreadSubject", SqlDbType.NVarChar, 255, ParameterDirection.Input, threadSubject);
            sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@IsLocked", SqlDbType.Bit, ParameterDirection.Input, isLocked);
            sph.DefineSqlParameter("@StartedByUserID", SqlDbType.Int, ParameterDirection.Input, startedByUserId);
            sph.DefineSqlParameter("@ThreadDate", SqlDbType.DateTime, ParameterDirection.Input, threadDate);

            sph.DefineSqlParameter("@ThreadGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, threadGuid);
            sph.DefineSqlParameter("@IsQuestion", SqlDbType.Bit, ParameterDirection.Input, isQuestion);
            sph.DefineSqlParameter("@IncludeInSiteMap", SqlDbType.Bit, ParameterDirection.Input, includeInSiteMap);
            sph.DefineSqlParameter("@SetNoIndexMeta", SqlDbType.Bit, ParameterDirection.Input, setNoIndexMeta);
            sph.DefineSqlParameter("@PTitleOverride", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageTitleOverride);
            sph.DefineSqlParameter("@ModStatus", SqlDbType.Int, ParameterDirection.Input, modStatus);
            sph.DefineSqlParameter("@ThreadType", SqlDbType.NVarChar, 100, ParameterDirection.Input, threadType);

            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool ForumThreadDelete(int threadId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ForumThreads_Delete", 1);
            sph.DefineSqlParameter("@ThreadID", SqlDbType.Int, ParameterDirection.Input, threadId);
            int rowsAffected = sph.ExecuteNonQuery();
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ForumThreads_Update", 15);
            sph.DefineSqlParameter("@ThreadID", SqlDbType.Int, ParameterDirection.Input, threadId);
            sph.DefineSqlParameter("@ForumID", SqlDbType.Int, ParameterDirection.Input, forumId);
            sph.DefineSqlParameter("@ThreadSubject", SqlDbType.NVarChar, 255, ParameterDirection.Input, threadSubject);
            sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@IsLocked", SqlDbType.Bit, ParameterDirection.Input, isLocked);

            sph.DefineSqlParameter("@IsQuestion", SqlDbType.Bit, ParameterDirection.Input, isQuestion);
            sph.DefineSqlParameter("@IncludeInSiteMap", SqlDbType.Bit, ParameterDirection.Input, includeInSiteMap);
            sph.DefineSqlParameter("@SetNoIndexMeta", SqlDbType.Bit, ParameterDirection.Input, setNoIndexMeta);
            sph.DefineSqlParameter("@PTitleOverride", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageTitleOverride);
            sph.DefineSqlParameter("@ModStatus", SqlDbType.Int, ParameterDirection.Input, modStatus);
            sph.DefineSqlParameter("@ThreadType", SqlDbType.NVarChar, 100, ParameterDirection.Input, threadType);
            sph.DefineSqlParameter("@AssignedTo", SqlDbType.UniqueIdentifier, ParameterDirection.Input, assignedTo);
            sph.DefineSqlParameter("@LockedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lockedBy);
            sph.DefineSqlParameter("@LockedReason", SqlDbType.NVarChar, 255, ParameterDirection.Input, lockedReason);

            if (lockedUtc < DateTime.MaxValue)
            {
                sph.DefineSqlParameter("@LockedUtc", SqlDbType.DateTime, ParameterDirection.Input, lockedUtc);
            }
            else
            {
                sph.DefineSqlParameter("@LockedUtc", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value);
            }

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool ForumThreadIncrementReplyStats(
          int threadId,
          int mostRecentPostUserId,
                DateTime mostRecentPostDate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ForumThreads_IncrementReplyCount", 3);
            sph.DefineSqlParameter("@ThreadID", SqlDbType.Int, ParameterDirection.Input, threadId);
            sph.DefineSqlParameter("@MostRecentPostUserID", SqlDbType.Int, ParameterDirection.Input, mostRecentPostUserId);
            sph.DefineSqlParameter("@MostRecentPostDate", SqlDbType.DateTime, ParameterDirection.Input, mostRecentPostDate);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool ForumThreadDecrementReplyStats(int threadId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ForumThreads_DecrementReplyCount", 1);
            sph.DefineSqlParameter("@ThreadID", SqlDbType.Int, ParameterDirection.Input, threadId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool ForumThreadUpdateViewStats(int threadId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ForumThreads_UpdateViewStats", 1);
            sph.DefineSqlParameter("@ThreadID", SqlDbType.Int, ParameterDirection.Input, threadId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader ForumThreadGetPosts(int threadId, int pageNumber)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ForumPosts_SelectByThread", 2);
            sph.DefineSqlParameter("@ThreadID", SqlDbType.Int, ParameterDirection.Input, threadId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            return sph.ExecuteReader();
        }

        public static IDataReader ForumThreadGetPosts(int threadId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ForumPosts_SelectAllByThread", 1);
            sph.DefineSqlParameter("@ThreadID", SqlDbType.Int, ParameterDirection.Input, threadId);
            return sph.ExecuteReader();
        }

        public static IDataReader ForumThreadGetPostsReverseSorted(int threadId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ForumPosts_SelectAllByThreadRevereSorted", 1);
            sph.DefineSqlParameter("@ThreadID", SqlDbType.Int, ParameterDirection.Input, threadId);
            return sph.ExecuteReader();
        }

        //public static IDataReader ForumThreadGetSortedPosts(int threadId)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "mp_ForumPosts_SelectSortedThread", 1);
        //    sph.DefineSqlParameter("@ThreadID", SqlDbType.Int, ParameterDirection.Input, threadId);
        //    return sph.ExecuteReader();
        //}

        public static IDataReader ForumThreadGetPostsByPage(int siteId, int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ForumThreads_SelectByPage", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            return sph.ExecuteReader();
        }

        public static IDataReader ForumThreadGetThreadsByPage(int siteId, int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ForumThreads_SelectThreadsByPage", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            return sph.ExecuteReader();
        }

        public static IDataReader ForumThreadGetPostsForRss(int siteId, int pageId, int moduleId, int itemId, int threadId, int maximumDays)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ForumPosts_SelectForRss", 6);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            sph.DefineSqlParameter("@ModuleId", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            sph.DefineSqlParameter("@ThreadID", SqlDbType.Int, ParameterDirection.Input, threadId);
            sph.DefineSqlParameter("@MaximumDays", SqlDbType.Int, ParameterDirection.Input, maximumDays);
            return sph.ExecuteReader();
        }

        public static DataSet ForumThreadGetSubscribers(int forumId, int threadId, int currentPostUserId, bool includeCurrentUser)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ForumThreadSubscribers_SelectByThread", 4);
            sph.DefineSqlParameter("@ForumID", SqlDbType.Int, ParameterDirection.Input, forumId);
            sph.DefineSqlParameter("@ThreadID", SqlDbType.Int, ParameterDirection.Input, threadId);
            sph.DefineSqlParameter("@CurrentPostUserID", SqlDbType.Int, ParameterDirection.Input, currentPostUserId);
            sph.DefineSqlParameter("@IncludeCurrentUser", SqlDbType.Bit, ParameterDirection.Input, includeCurrentUser);
            return sph.ExecuteDataset();
        }


        public static IDataReader ForumThreadGetSubscriber(Guid subGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ForumThreadSubscriptions_SelectOne", 1);
            sph.DefineSqlParameter("@SubGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, subGuid);
            
            return sph.ExecuteReader();
        }

        public static bool ForumThreadAddSubscriber(int threadId, int userId, Guid subGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ForumThreadSubscriptions_Insert", 4);
            sph.DefineSqlParameter("@ThreadID", SqlDbType.Int, ParameterDirection.Input, threadId);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@SubGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, subGuid);
            sph.DefineSqlParameter("@SubscribeDate", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool ForumThreadUnSubscribe(Guid subGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ForumThreadSubscriptions_UnsubThread", 2);
            sph.DefineSqlParameter("@SubGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, subGuid);
            sph.DefineSqlParameter("@CurrentUtc", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool ForumThreadUNSubscribe(int threadId, int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ForumThreadSubscriptions_UnsubscribeThread", 2);
            sph.DefineSqlParameter("@ThreadID", SqlDbType.Int, ParameterDirection.Input, threadId);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool ForumThreadUnsubscribeAll(int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ForumThreadSubscriptions_UnsubscribeAllThreads", 1);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ForumPosts_Insert", 12);
            sph.DefineSqlParameter("@ThreadID", SqlDbType.Int, ParameterDirection.Input, threadId);
            sph.DefineSqlParameter("@Subject", SqlDbType.NVarChar, 255, ParameterDirection.Input, subject);
            sph.DefineSqlParameter("@Post", SqlDbType.NVarChar, -1, ParameterDirection.Input, post);
            sph.DefineSqlParameter("@Approved", SqlDbType.Bit, ParameterDirection.Input, approved);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@PostDate", SqlDbType.DateTime, ParameterDirection.Input, postDate);

            sph.DefineSqlParameter("@PostGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, postGuid);
            sph.DefineSqlParameter("@ApprovedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, approvedBy);

            if (approvedUtc > DateTime.MinValue)
            {
                sph.DefineSqlParameter("@ApprovedUtc", SqlDbType.DateTime, ParameterDirection.Input, approvedUtc);
            }
            else
            {
                sph.DefineSqlParameter("@ApprovedUtc", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value);
            }
            sph.DefineSqlParameter("@UserIp", SqlDbType.NVarChar, 50, ParameterDirection.Input, userIp);
            sph.DefineSqlParameter("@NotificationSent", SqlDbType.Bit, ParameterDirection.Input, notificationSent);
            sph.DefineSqlParameter("@ModStatus", SqlDbType.Int, ParameterDirection.Input, modStatus);


            int newID = Convert.ToInt32(sph.ExecuteScalar());
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ForumPosts_Update", 9);
            sph.DefineSqlParameter("@PostID", SqlDbType.Int, ParameterDirection.Input, postId);
            sph.DefineSqlParameter("@Subject", SqlDbType.NVarChar, 255, ParameterDirection.Input, subject);
            sph.DefineSqlParameter("@Post", SqlDbType.NVarChar, -1, ParameterDirection.Input, post);
            sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@Approved", SqlDbType.Bit, ParameterDirection.Input, approved);
            sph.DefineSqlParameter("@ApprovedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, approvedBy);

            if (approvedUtc > DateTime.MinValue)
            {
                sph.DefineSqlParameter("@ApprovedUtc", SqlDbType.DateTime, ParameterDirection.Input, approvedUtc);
            }
            else
            {
                sph.DefineSqlParameter("@ApprovedUtc", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value);
            }

            sph.DefineSqlParameter("@NotificationSent", SqlDbType.Bit, ParameterDirection.Input, notificationSent);
            sph.DefineSqlParameter("@ModStatus", SqlDbType.Int, ParameterDirection.Input, modStatus);


           
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool ForumPostDelete(int postId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ForumPosts_Delete", 1);
            sph.DefineSqlParameter("@PostID", SqlDbType.Int, ParameterDirection.Input, postId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }


        public static bool ForumPostUpdateThreadSequence(
            int postId,
            int threadSequence)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ForumPosts_UpdateSequence", 2);
            sph.DefineSqlParameter("@PostID", SqlDbType.Int, ParameterDirection.Input, postId);
            sph.DefineSqlParameter("@ThreadSequence", SqlDbType.Int, ParameterDirection.Input, threadSequence);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }



    }
}
