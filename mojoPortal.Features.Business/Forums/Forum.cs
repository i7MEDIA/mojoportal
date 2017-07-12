// Author:					
// Created:				    2004-09-12
// Last Modified:			2014-06-21
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
using log4net;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    /// Represents a forum
    /// </summary>
    public class Forum
    {
        private const string featureGuid = "38aa5a84-9f5c-42eb-8f4c-105983d419fb";

        public static Guid FeatureGuid
        {
            get { return new Guid(featureGuid); }
        }

        public const int NormalThreadSort = 100;

        #region Constructors

        public Forum()
        { }

        public Forum(int forumId)
        {
            if (forumId > -1)
            {
                GetForum(forumId);
            }

        }

        #endregion

        #region Private Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(Forum));

        private int itemID = -1;
        private Guid forumGuid = Guid.Empty;
        private int moduleID = -1;
        private DateTime createdDate = DateTime.UtcNow;
        private string createdBy = string.Empty;
        private int createdByUserID;
        private string title = string.Empty;
        private string description = string.Empty;
        private bool isModerated;
        private bool isActive = true;
        //sorted in db by SortOrder, ItemID
        private int sortOrder = 100;
        private int postsPerPage = 10;
        private int threadsPerPage = 20;
        private int threadCount;
        private int postCount;
        private int totalPages = 1;
        private DateTime mostRecentPostDate = DateTime.UtcNow;
        private string mostRecentPostUser = string.Empty;
        private bool allowAnonymousPosts = false;

        private string rolesThatCanPost = "Authenticated Users;";

        public string RolesThatCanPost
        {
            get { return rolesThatCanPost; }
            set { rolesThatCanPost = value; }
        }

        private string rolesThatCanModerate = string.Empty;

        public string RolesThatCanModerate
        {
            get { return rolesThatCanModerate; }
            set { rolesThatCanModerate = value; }
        }

        private string moderatorNotifyEmail = string.Empty;

        public string ModeratorNotifyEmail
        {
            get { return moderatorNotifyEmail; }
            set { moderatorNotifyEmail = value; }
        }

        private bool includeInGoogleMap = true;

        public bool IncludeInGoogleMap
        {
            get { return includeInGoogleMap; }
            set { includeInGoogleMap = value; }
        }

        private bool addNoIndexMeta = false;

        public bool AddNoIndexMeta
        {
            get { return addNoIndexMeta; }
            set { addNoIndexMeta = value; }
        }

        private bool closed = false;

        public bool Closed
        {
            get { return closed; }
            set { closed = value; }
        }

        private bool visible = true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        private bool requireModeration = false;
        /// <summary>
        /// require approval before posts appear in list?
        /// </summary>
        public bool RequireModeration
        {
            get { return requireModeration; }
            set { requireModeration = value; }
        }

        private bool requireModForNotify = false;
        /// <summary>
        /// require approval before email notification of a post is sent?
        /// </summary>
        public bool RequireModForNotify
        {
            get { return requireModForNotify; }
            set { requireModForNotify = value; }
        }

        private bool allowTrustedDirectPosts = false;
        /// <summary>
        /// allow users marked as trusted to bypass moderation?
        /// ie show their posts immediately
        /// </summary>
        public bool AllowTrustedDirectPosts
        {
            get { return allowTrustedDirectPosts; }
            set { allowTrustedDirectPosts = value; }
        }

        private bool allowTrustedDirectNotify = false;
        /// <summary>
        /// when trusted user makes post send notification?
        /// applicable only when AllowTrustedDirectPosts is true
        /// and RequireModeration or RequireModForNotify is true
        /// </summary>
        public bool AllowTrustedDirectNotify
        {
            get { return allowTrustedDirectNotify; }
            set { allowTrustedDirectNotify = value; }
        }

        #endregion

        #region Public Properties

        public int ItemId
        {
            get { return itemID; }
        }

        public Guid ForumGuid
        {
            get { return forumGuid; }
        }

        public int ModuleId
        {
            get { return moduleID; }
            set { moduleID = value; }
        }

        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        public int CreatedByUserId
        {
            get { return createdByUserID; }
            set { createdByUserID = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public bool AllowAnonymousPosts
        {
            get { return allowAnonymousPosts; }
            set { allowAnonymousPosts = value; }
        }


        public bool IsModerated
        {
            get { return isModerated; }
            set { isModerated = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public int SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }

        public int PostsPerPage
        {
            get { return postsPerPage; }
            set { postsPerPage = value; }
        }

        public int ThreadsPerPage
        {
            get { return threadsPerPage; }
            set { threadsPerPage = value; }
        }

        public int ThreadCount
        {
            get { return threadCount; }
            set { threadCount = value; }
        }

        public int PostCount
        {
            get { return postCount; }
            set { postCount = value; }
        }

        public int TotalPages
        {
            get { return totalPages; }
        }

        public DateTime MostRecentPostDate
        {
            get { return mostRecentPostDate; }
        }

        public string MostRecentPostUser
        {
            get { return mostRecentPostUser; }
        }




        #endregion

        #region Private Methods

        private void GetForum(int forumId)
        {
            using (IDataReader reader = DBForums.GetForum(forumId))
            {
                if (reader.Read())
                {

                    this.itemID = int.Parse(reader["ItemID"].ToString());
                    this.moduleID = int.Parse(reader["ModuleID"].ToString());
                    this.createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                    this.createdBy = reader["CreatedByUser"].ToString();
                    this.title = reader["Title"].ToString();
                    this.description = reader["Description"].ToString();
                    // this is to support dbs that don't have bit data type
                    string anon = reader["AllowAnonymousPosts"].ToString();
                    this.allowAnonymousPosts = (anon == "True" || anon == "1");
                    string moderated = reader["IsModerated"].ToString();
                    this.isModerated = (moderated == "True" || moderated == "1");
                    string active = reader["IsActive"].ToString();
                    this.isActive = (active == "True" || active == "1");
                    this.sortOrder = int.Parse(reader["SortOrder"].ToString());
                    this.postsPerPage = int.Parse(reader["PostsPerPage"].ToString());
                    this.threadsPerPage = int.Parse(reader["ThreadsPerPage"].ToString());
                    this.threadCount = int.Parse(reader["ThreadCount"].ToString());
                    this.postCount = int.Parse(reader["PostCount"].ToString());
                    if (reader["MostRecentPostDate"] != DBNull.Value)
                    {
                        this.mostRecentPostDate = Convert.ToDateTime(reader["MostRecentPostDate"]);
                    }
                    this.mostRecentPostUser = reader["MostRecentPostUser"].ToString();

                    if (this.threadCount > this.threadsPerPage)
                    {
                        this.totalPages = this.threadCount / this.threadsPerPage;
                        int remainder = 0;
                        Math.DivRem(this.threadCount, this.threadsPerPage, out remainder);
                        if (remainder > 0)
                        {
                            this.totalPages += 1;
                        }
                    }
                    else
                    {
                        this.totalPages = 1;
                    }

                    if (reader["ForumGuid"] != DBNull.Value)
                    {
                        this.forumGuid = new Guid(reader["ForumGuid"].ToString());
                    }

                    this.rolesThatCanPost = reader["RolesThatCanPost"].ToString();
                    this.rolesThatCanModerate = reader["RolesThatCanModerate"].ToString();

                    if((rolesThatCanPost.Length > 0)&&(!rolesThatCanPost.EndsWith(";")))
                    {
                        rolesThatCanPost += ";";
                    }

                    if ((rolesThatCanModerate.Length > 0) && (!rolesThatCanModerate.EndsWith(";")))
                    {
                        rolesThatCanModerate += ";";
                    }

                    this.moderatorNotifyEmail = reader["ModeratorNotifyEmail"].ToString();

                    if (reader["IncludeInGoogleMap"] != DBNull.Value)
                    {
                        this.includeInGoogleMap = Convert.ToBoolean(reader["IncludeInGoogleMap"]);
                    }

                    if (reader["AddNoIndexMeta"] != DBNull.Value)
                    {
                        this.addNoIndexMeta = Convert.ToBoolean(reader["AddNoIndexMeta"]);
                    }

                    if (reader["Closed"] != DBNull.Value)
                    {
                        this.closed = Convert.ToBoolean(reader["Closed"]);
                    }

                    if (reader["Visible"] != DBNull.Value)
                    {
                        this.visible = Convert.ToBoolean(reader["Visible"]);
                    }

                    if (reader["RequireModeration"] != DBNull.Value)
                    {
                        this.requireModeration = Convert.ToBoolean(reader["RequireModeration"]);
                    }

                    if (reader["RequireModForNotify"] != DBNull.Value)
                    {
                        this.requireModForNotify = Convert.ToBoolean(reader["RequireModForNotify"]);
                    }

                    if (reader["AllowTrustedDirectPosts"] != DBNull.Value)
                    {
                        this.allowTrustedDirectPosts = Convert.ToBoolean(reader["AllowTrustedDirectPosts"]);
                    }

                    if (reader["AllowTrustedDirectNotify"] != DBNull.Value)
                    {
                        this.allowTrustedDirectNotify = Convert.ToBoolean(reader["AllowTrustedDirectNotify"]);
                    }

                }
                else
                {
                    if (log.IsErrorEnabled)
                    {
                        log.Error("IDataReader didn't read in Forum.GetForum");
                    }


                }
            }
            


        }

        private bool Create()
        {
            int newID = -1;
            if (forumGuid == Guid.Empty) { forumGuid = Guid.NewGuid(); }

            newID = DBForums.Create(
                this.forumGuid,
                this.moduleID,
                this.createdByUserID,
                this.title,
                this.description,
                this.isModerated,
                this.isActive,
                this.sortOrder,
                this.postsPerPage,
                this.threadsPerPage,
                this.allowAnonymousPosts,
                this.rolesThatCanPost,
                this.rolesThatCanModerate,
                this.moderatorNotifyEmail,
                this.includeInGoogleMap,
                this.addNoIndexMeta,
                this.closed,
                this.visible,
                this.requireModeration,
                this.requireModForNotify,
                this.allowTrustedDirectPosts,
                this.allowTrustedDirectNotify);

            this.itemID = newID;

            return (newID > -1);

        }


        private bool Update()
        {

            return DBForums.Update(
                this.itemID,
                this.createdByUserID,
                this.title,
                this.description,
                this.isModerated,
                this.isActive,
                this.sortOrder,
                this.postsPerPage,
                this.threadsPerPage,
                this.allowAnonymousPosts,
                this.rolesThatCanPost,
                this.rolesThatCanModerate,
                this.moderatorNotifyEmail,
                this.includeInGoogleMap,
                this.addNoIndexMeta,
                this.closed,
                this.visible,
                this.requireModeration,
                this.requireModForNotify,
                this.allowTrustedDirectPosts,
                this.allowTrustedDirectNotify);
        }

        #endregion

        #region Public Methods


        public bool Save()
        {
            if (this.itemID > -1)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }


        public IDataReader GetThreads(int pageNumber)
        {
            return DBForums.GetThreads(this.itemID, pageNumber);
        }

        public bool Subscribe(int userId)
        {
            if (DBForums.ForumSubscriptionExists(this.itemID, userId)) { return true; }

            return DBForums.AddSubscriber(this.itemID, userId, Guid.NewGuid());
        }

        public bool Unsubscribe(int userId)
        {
            return DBForums.Unsubscribe(this.itemID, userId);
        }

        //public static int GetUserIdForSubscription(Guid subGuid)
        //{
        //    int userId = -1;

        //    using (IDataReader reader = DBForums.GetForumSubscription(subGuid))
        //    {
        //        if (reader.Read())
        //        {
        //            userId = Convert.ToInt32(reader["UserID"]);
        //        }
        //    }


        //    return userId;

        //}

        public static bool Unsubscribe(Guid subGuid)
        {
            return DBForums.Unsubscribe(subGuid);
        }

       
        #endregion

        #region Static Methods

        public static bool UnsubscribeAll(int userId)
        {
            return DBForums.UnsubscribeAll(userId);
        }

        public static IDataReader GetForums(int moduleId, int userId)
        {
            return DBForums.GetForums(moduleId, userId);
        }


        public static bool IncrementPostCount(int forumId, int mostRecentPostUserId, DateTime mostRecentPostDate)
        {
            return DBForums.IncrementPostCount(forumId, mostRecentPostUserId, mostRecentPostDate);
        }

        public static bool IncrementPostCount(int forumId)
        {
            return DBForums.IncrementPostCount(forumId);
        }

        public static bool DecrementPostCount(int forumId)
        {
            return DBForums.DecrementPostCount(forumId);
        }

        public static bool RecalculatePostStats(int forumId)
        {
            //implemented for PostgreSQL. --Dean 9/11/05
            return DBForums.RecalculatePostStats(forumId);

        }

        public static bool DecrementThreadCount(int forumId)
        {
            return DBForums.DecrementThreadCount(forumId);
        }

        public static bool IncrementThreadCount(int forumId)
        {
            return DBForums.IncrementThreadCount(forumId);
        }

        public static bool Delete(int itemId)
        {
            return DBForums.Delete(itemId);
        }

        public static bool DeleteByModule(int moduleId)
        {
            return DBForums.DeleteByModule(moduleId);
        }

        public static bool DeleteBySite(int siteId)
        {
            return DBForums.DeleteBySite(siteId);
        }

        public static bool IsSubscribed(int forumId, int userId)
        {
            return DBForums.ForumSubscriptionExists(forumId, userId);
        }

        /// <summary>
        /// passing in -1 for userId will update the stats of all users.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool UpdateUserStats(int userId)
        {
            return DBForums.UpdateUserStats(userId);
        }

        public static IDataReader GetSubscriberPage(
            int forumId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBForums.GetSubscriberPage(
                forumId,
                pageNumber,
                pageSize,
                out totalPages);
        }

        public static bool DeleteSubscription(int subscriptionId)
        {
            return DBForums.DeleteSubscription(subscriptionId);
        }


        #endregion

    }
}
