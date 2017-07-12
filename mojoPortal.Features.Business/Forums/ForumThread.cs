// Author:					
// Created:				    2004-09-19
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
using System.Configuration;
using System.Data;
using log4net;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    /// Encapsulates a thread and post
    /// </summary>
    public class ForumThread : IIndexableContent
    {

        #region Constructors

        public ForumThread()
        {

        }

        public ForumThread(int threadId)
        {
            GetThread(threadId);
        }

        public ForumThread(int threadId, int postId)
        {
            GetThread(threadId);
            GetPost(postId);
        }

        #endregion


        #region Private Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(ForumThread));


        private Guid threadGuid = Guid.Empty;
        private int threadID = -1;
        private int forumID = -1;
        private int moduleID = -1;
        private int origForumID = 0;
        private DateTime threadDate = DateTime.UtcNow;
        private string startedBy = string.Empty;
        private int startedByUserID = -1;
        private string subject = string.Empty;
        private int totalViews = 0;
        private int totalReplies = 0;
        private bool isLocked = false;
        //sorted in db by SortOrder, ItemID
        private int sortOrder = 100;
        private int forumSequence = 1;
        private DateTime mostRecentPostDate = DateTime.UtcNow;
        private string mostRecentPostUser = string.Empty;
        private int mostRecentPostUserID = 0;
        private int postsPerPage = 0;
        private int totalPages = 1;

        private int modStatus = 1; //approved
        /// <summary>
        /// mod status of the thread/should correspond to the status of the first post on the thread
        /// </summary>
        public int ModStatus
        {
            get { return ModStatus; }
            set { ModStatus = value; }
        }

        private string threadType = string.Empty;

        public string ThreadType
        {
            get { return threadType; }
            set { threadType = value; }
        }

        private Guid assignedTo = Guid.Empty;

        public Guid AssignedTo
        {
            get { return assignedTo; }
            set { assignedTo = value; }
        }

        private Guid lockedBy = Guid.Empty;

        public Guid LockedBy
        {
            get { return lockedBy; }
            set { lockedBy = value; }
        }

        private string lockedReason = string.Empty;

        public string LockedReason
        {
            get { return lockedReason; }
            set { lockedReason = value; }
        }

        private DateTime lockedUtc = DateTime.MaxValue;

        public DateTime LockedUtc
        {
            get { return lockedUtc; }
            set { lockedUtc = value; }
        }



        //post properties
        private int postID = -1;
        private int threadSequence = 1;
        private string postSubject = string.Empty;
        private string postDate = string.Empty;
        private DateTime currentPostDate = DateTime.UtcNow;
        private bool isApproved = true;
        private int postUserID = -1;
        private string postUserName = string.Empty;
        private int postSortOrder = 100;
        private string postMessage = string.Empty;
        private bool subscribeUserToThread = false;
        private bool notificationSent = false;

        public bool NotificationSent
        {
            get { return notificationSent; }
            set { notificationSent = value; }
        }

        private int postModStatus = 1; //approved

        public int PostModStatus
        {
            get { return postModStatus; }
            set { postModStatus = value; }
        }


        private int siteId = -1;
        private string searchIndexPath = string.Empty;


        #endregion


        #region Public Properties

        /// <summary>
        /// This is not persisted to the db. It is only set and used when indexing forum threads in the search index.
        /// Its a convenience because when we queue the task to index on a new thread we can only pass one object.
        /// So we store extra properties here so we don't need any other objects.
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        /// <summary>
        /// This is not persisted to the db. It is only set and used when indexing forum threads in the search index.
        /// Its a convenience because when we queue the task to index on a new thread we can only pass one object.
        /// So we store extra properties here so we don't need any other objects.
        /// </summary>
        public string SearchIndexPath
        {
            get { return searchIndexPath; }
            set { searchIndexPath = value; }
        }

        public Guid ThreadGuid
        {
            get { return threadGuid; }
        }

        private bool isQuestion = true;

        public bool IsQuestion
        {
            get { return isQuestion; }
            set { isQuestion = value; }
        }

        private bool includeInSiteMap = true;

        public bool IncludeInSiteMap
        {
            get { return includeInSiteMap; }
            set { includeInSiteMap = value; }
        }

        private bool setNoIndexMeta = false;

        public bool SetNoIndexMeta
        {
            get { return setNoIndexMeta; }
            set { setNoIndexMeta = value; }
        }

        private string pageTitleOverride = string.Empty;

        public string PageTitleOverride
        {
            get { return pageTitleOverride; }
            set { pageTitleOverride = value; }
        }
        

        public int ThreadId
        {
            get { return threadID; }
        }

        public int ForumId
        {
            get { return forumID; }
            set { forumID = value; }
        }

        public int ModuleId
        {
            get { return moduleID; }

        }

        public DateTime ThreadDate
        {
            get { return threadDate; }
        }

        public DateTime CurrentPostDate
        {
            get { return currentPostDate; }
        }

        public string StartedBy
        {
            get { return startedBy; }
            set { startedBy = value; }
        }

        public int StartedByUserId
        {
            get { return startedByUserID; }
            set { startedByUserID = value; }
        }

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public int TotalViews
        {
            get { return totalViews; }
        }

        public int TotalReplies
        {
            get { return totalReplies; }
        }

        public int TotalPages
        {
            get { return totalPages; }
        }


        public bool IsLocked
        {
            get { return isLocked; }
            set { isLocked = value; }
        }

        public bool IsClosed(int daysOldToClose)
        {
            if (daysOldToClose == -1) { return false; }

            if (this.mostRecentPostDate < DateTime.UtcNow.AddDays(-daysOldToClose)) { return true; }

            return false;
        }

        public int SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }

        public int ForumSequence
        {
            get { return forumSequence; }
        }


        public DateTime MostRecentPostDate
        {
            get { return mostRecentPostDate; }
        }

        public string MostRecentPostUser
        {
            get { return mostRecentPostUser; }
        }

        public int MostRecentPostUserId
        {
            get { return mostRecentPostUserID; }
        }

        //post properties

        private Guid postGuid = Guid.Empty;

        public Guid PostGuid
        {
            get { return postGuid; }
        }

        public int PostId
        {
            get { return postID; }
        }

        public int ThreadSequence
        {
            get { return threadSequence; }
        }

        public string PostSubject
        {
            get { return postSubject; }
            set { postSubject = value; }
        }

        public string PostDate
        {
            get { return postDate; }
        }

        public bool IsApproved
        {
            get { return isApproved; }
            set { isApproved = value; }
        }

        public int PostUserId
        {
            get { return postUserID; }
            set { postUserID = value; }
        }

        public string PostUserName
        {
            get { return postUserName; }
            set { postUserName = value; }
        }

        public int PostSortOrder
        {
            get { return postSortOrder; }
            set { postSortOrder = value; }
        }

        public string PostMessage
        {
            get { return postMessage; }
            set { postMessage = value; }
        }

        private int answerVotes = 0;

        public int AnswerVotes
        {
            get { return answerVotes; }
            set { answerVotes = value; }
        }

        private Guid approvedBy = Guid.Empty;

        public Guid ApprovedBy
        {
            get { return approvedBy; }
            set { approvedBy = value; }
        }

        private DateTime approvedUtc = DateTime.MinValue;
        public DateTime ApprovedUtc
        {
            get { return approvedUtc; }
            set { approvedUtc = value; }
        }

        private string userIp = string.Empty;

        public string UserIp
        {
            get { return userIp; }
            set { userIp = value; }
        }

        public bool SubscribeUserToThread
        {
            get { return subscribeUserToThread; }
            set { subscribeUserToThread = value; }
        }


        #endregion


        #region Private Methods

        private void GetThread(int threadId)
        {
            using (IDataReader reader = DBForums.ForumThreadGetThread(threadId))
            {
                if (reader.Read())
                {

                    this.threadID = int.Parse(reader["ThreadID"].ToString());
                    if (reader["ForumID"] != DBNull.Value)
                    {
                        this.forumID = Convert.ToInt32(reader["ForumID"]);
                        this.origForumID = forumID;
                    }
                    if (reader["ModuleID"] != DBNull.Value)
                    {
                        this.moduleID = Convert.ToInt32(reader["ModuleID"]);
                    }
                    if (reader["ThreadDate"] != DBNull.Value)
                    {
                        this.threadDate = Convert.ToDateTime(reader["ThreadDate"].ToString());
                    }
                    this.startedBy = reader["StartedBy"].ToString();
                    if (reader["StartedByUserID"] != DBNull.Value)
                    {
                        this.startedByUserID = int.Parse(reader["StartedByUserID"].ToString());
                    }

                    this.subject = reader["ThreadSubject"].ToString();
                    if (reader["TotalViews"] != DBNull.Value)
                    {
                        this.totalViews = Convert.ToInt32(reader["TotalViews"]);
                    }

                    if (reader["TotalReplies"] != DBNull.Value)
                    {
                        this.totalReplies = Convert.ToInt32(reader["TotalReplies"]);
                    }

                    if (reader["SortOrder"] != DBNull.Value)
                    {
                        this.sortOrder = Convert.ToInt32(reader["SortOrder"]);
                    }
                    if (reader["ForumSequence"] != DBNull.Value)
                    {
                        this.forumSequence = Convert.ToInt32(reader["ForumSequence"]);
                    }

                    if (reader["PostsPerPage"] != DBNull.Value)
                    {
                        this.postsPerPage = Convert.ToInt32(reader["PostsPerPage"]);
                    }

                    if (this.totalReplies + 1 > this.postsPerPage)
                    {
                        this.totalPages = this.totalReplies / this.postsPerPage;
                        int remainder = 0;
                        int pageCount = Math.DivRem(this.totalReplies + 1, this.postsPerPage, out remainder);
                        if ((remainder > 0) || (pageCount > this.totalPages))
                        {
                            this.totalPages += 1;
                        }
                    }
                    else
                    {
                        this.totalPages = 1;
                    }

                    // this is to support dbs that don't have bit data type
                    string locked = reader["IsLocked"].ToString();
                    this.isLocked = (locked == "True" || locked == "1");

                    if (reader["MostRecentPostDate"] != DBNull.Value)
                    {
                        this.mostRecentPostDate = Convert.ToDateTime(reader["MostRecentPostDate"]);
                    }

                    this.mostRecentPostUser = reader["MostRecentPostUser"].ToString();

                    if (reader["MostRecentPostUserID"] != DBNull.Value)
                    {
                        this.mostRecentPostUserID = Convert.ToInt32(reader["MostRecentPostUserID"]);
                    }

                    if (reader["ThreadGuid"] != DBNull.Value)
                    {
                        this.threadGuid = new Guid(reader["ThreadGuid"].ToString());
                    }

                    if (reader["IsQuestion"] != DBNull.Value)
                    {
                        this.isQuestion = Convert.ToBoolean(reader["IsQuestion"]);
                    }

                    if (reader["IncludeInSiteMap"] != DBNull.Value)
                    {
                        this.includeInSiteMap = Convert.ToBoolean(reader["IncludeInSiteMap"]);
                    }

                    if (reader["SetNoIndexMeta"] != DBNull.Value)
                    {
                        this.setNoIndexMeta = Convert.ToBoolean(reader["SetNoIndexMeta"]);
                    }

                    this.pageTitleOverride = reader["PTitleOverride"].ToString();

                    if (reader["ModStatus"] != DBNull.Value)
                    {
                        this.modStatus = Convert.ToInt32(reader["ModStatus"]);
                    }

                    if (reader["AssignedTo"] != DBNull.Value)
                    {
                        this.assignedTo = new Guid(reader["AssignedTo"].ToString());
                    }

                    if (reader["LockedBy"] != DBNull.Value)
                    {
                        this.lockedBy = new Guid(reader["LockedBy"].ToString());
                    }

                    this.threadType = reader["ThreadType"].ToString();

                    this.LockedReason = reader["LockedReason"].ToString();

                    if (reader["LockedUtc"] != DBNull.Value)
                    {
                        this.lockedUtc = Convert.ToDateTime(reader["LockedUtc"]);
                    }
                    

                    //

                    

                }

            }


        }

        private void GetPost(int postId)
        {
            using (IDataReader reader = DBForums.ForumThreadGetPost(postId))
            {
                if (reader.Read())
                {
                    this.postID = Convert.ToInt32(reader["PostID"]);
                    this.postUserID = Convert.ToInt32(reader["UserID"]);
                    this.postSubject = reader["Subject"].ToString();
                    this.postMessage = reader["Post"].ToString();

                    
                    this.isApproved = Convert.ToBoolean(reader["Approved"]);
                    this.postSortOrder = Convert.ToInt32(reader["SortOrder"]);
                    currentPostDate = Convert.ToDateTime(reader["PostDate"]);

                    if (reader["PostGuid"] != DBNull.Value)
                    {
                        this.postGuid = new Guid(reader["PostGuid"].ToString());
                    }

                    if (reader["AnswerVotes"] != DBNull.Value)
                    {
                        this.answerVotes = Convert.ToInt32(reader["AnswerVotes"]);
                    }

                    if (reader["ApprovedBy"] != DBNull.Value)
                    {
                        this.approvedBy = new Guid(reader["ApprovedBy"].ToString());
                    }

                    if (reader["ApprovedUtc"] != DBNull.Value)
                    {
                        this.approvedUtc = Convert.ToDateTime(reader["ApprovedUtc"]);
                    }

                    this.userIp = reader["UserIp"].ToString();

                    notificationSent = Convert.ToBoolean(reader["NotificationSent"]);
                    postModStatus = Convert.ToInt32(reader["ModStatus"]);

                    
                    
                }

            }

        }


        private bool CreateThread()
        {
            int newID = -1;

            if (threadGuid == Guid.Empty) { threadGuid = Guid.NewGuid(); }

            newID = DBForums.ForumThreadCreate(
                this.forumID,
                this.postSubject,
                this.sortOrder,
                this.isLocked,
                this.postUserID,
                DateTime.UtcNow,
                this.threadGuid,
                this.isQuestion,
                this.includeInSiteMap,
                this.setNoIndexMeta,
                this.pageTitleOverride,
                this.modStatus,
                this.threadType);


            this.threadID = newID;
            Forum.IncrementThreadCount(this.forumID);

            return (newID > -1);

        }

        private bool CreatePost()
        {
            int newID = -1;
            bool approved = false;
            if (
                (ConfigurationManager.AppSettings["PostsApprovedByDefault"] != null)
                && (string.Equals(ConfigurationManager.AppSettings["PostsApprovedByDefault"], "true", StringComparison.InvariantCultureIgnoreCase))
                )
            {
                approved = true;
            }

            this.mostRecentPostDate = DateTime.UtcNow;

            if (postGuid == Guid.Empty) { postGuid = Guid.NewGuid(); }

            newID = DBForums.ForumPostCreate(
                this.threadID,
                this.postSubject,
                this.postMessage,
                approved,
                this.PostUserId,
                this.mostRecentPostDate,
                this.postGuid,
                this.approvedBy,
                this.approvedUtc,
                this.userIp,
                this.notificationSent,
                this.postModStatus);

            this.postID = newID;
            Forum.IncrementPostCount(this.forumID, this.postUserID, this.mostRecentPostDate);
            SiteUser.IncrementTotalPosts(this.postUserID);
            //IndexHelper.IndexItem(this);

            bool result = (newID > -1);

            //if (result)
            //{
            //    ContentChangedEventArgs e = new ContentChangedEventArgs();
            //    OnContentChanged(e);
            //}

            return result;

        }

        public bool UpdatePost()
        {
            bool result = false;

            result = DBForums.ForumPostUpdate(
                this.postID,
                this.postSubject,
                this.postMessage,
                this.postSortOrder,
                this.isApproved,
                this.approvedBy,
                this.approvedUtc,
                this.notificationSent,
                this.postModStatus);

            //IndexHelper.IndexItem(this);
            //if (result)
            //{
            //    ContentChangedEventArgs e = new ContentChangedEventArgs();
            //    OnContentChanged(e);
            //}

            return result;

        }



        private bool IncrementReplyStats()
        {
            return DBForums.ForumThreadIncrementReplyStats(
                this.threadID,
                this.postUserID,
                this.mostRecentPostDate);

        }

        private void ResetThreadSequences()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PostID", typeof(int));

            using (IDataReader reader = DBForums.ForumThreadGetPosts(this.threadID))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["PostID"] = reader["PostID"];
                    dataTable.Rows.Add(row);
                }

            }

            int sequence = 1;
            foreach (DataRow row in dataTable.Rows)
            {
                DBForums.ForumPostUpdateThreadSequence(
                    Convert.ToInt32(row["PostID"]),
                    sequence);
                sequence += 1;
            }

        }


        #endregion


        #region Public Methods

        public int Post()
        {
            bool newThread = (this.threadID < 0);
            if (newThread)
            {
                this.CreateThread();
            }

            if (this.postID > -1)
            {
                this.UpdatePost();
            }
            else
            {
                this.CreatePost();
                if (!newThread)
                {
                    this.IncrementReplyStats();
                }

            }

            if (this.subscribeUserToThread)
            {
                if (!DBForums.ForumSubscriptionExists(this.forumID, this.postUserID))
                {
                    DBForums.ForumThreadAddSubscriber(this.threadID, this.postUserID, Guid.NewGuid());
                }

            }

            if (this.postID > -1)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                OnContentChanged(e);
            }


            return this.postID;

        }

        public bool DeletePost(int postId)
        {
            bool deleted = DBForums.ForumPostDelete(postId);
            if (deleted)
            {
                Forum.DecrementPostCount(this.forumID);
                if (this.totalReplies > 0)
                {
                    DBForums.ForumThreadDecrementReplyStats(this.threadID);
                }
                Forum forum = new Forum(this.forumID);

                this.moduleID = forum.ModuleId;
                this.postID = postId;

                ContentChangedEventArgs e = new ContentChangedEventArgs();
                e.IsDeleted = true;
                OnContentChanged(e);

                int threadPostCount = ForumThread.GetPostCount(this.threadID);
                if (threadPostCount == 0)
                {
                    ForumThread.Delete(this.threadID);
                    Forum.DecrementThreadCount(this.forumID);

                }

                ResetThreadSequences();
            }


            return deleted;
        }



        public bool UpdateThreadViewStats()
        {
            return DBForums.ForumThreadUpdateViewStats(this.threadID);


        }

        public IDataReader GetPosts(int pageNumber)
        {
            return DBForums.ForumThreadGetPosts(this.threadID, pageNumber);
        }

        public IDataReader GetPosts()
        {
            return DBForums.ForumThreadGetPosts(this.threadID);
        }

        public DataTable GetPostIdList()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PostID", typeof(int));

            using (IDataReader reader = DBForums.ForumThreadGetPosts(this.threadID))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["PostID"] = reader["PostID"];
                    dataTable.Rows.Add(row);

                }
            }

            return dataTable;

        }

        public IDataReader GetPostsReverseSorted()
        {
            return DBForums.ForumThreadGetPostsReverseSorted(this.threadID);
        }


        public DataSet GetThreadSubscribers(bool includeCurrentUser)
        {
            return DBForums.ForumThreadGetSubscribers(this.forumID, this.threadID, this.postUserID, includeCurrentUser);
        }

        public bool UpdateThread()
        {
            bool result = false;

            result = DBForums.ForumThreadUpdate(
                this.threadID,
                this.forumID,
                this.subject,
                this.sortOrder,
                this.isLocked,
                this.isQuestion,
                this.includeInSiteMap,
                this.setNoIndexMeta,
                this.pageTitleOverride,
                this.modStatus,
                this.threadType,
                this.assignedTo,
                this.lockedBy,
                this.lockedReason,
                this.lockedUtc);

            if (this.forumID != this.origForumID)
            {

                Forum.DecrementThreadCount(this.origForumID);
                Forum.IncrementThreadCount(this.forumID);

                ForumThreadMovedArgs e = new ForumThreadMovedArgs();
                e.ForumId = forumID;
                e.OriginalForumId = origForumID;
                OnThreadMoved(e);


                Forum.RecalculatePostStats(this.origForumID);
                Forum.RecalculatePostStats(this.forumID);
            }

            return result;
        }



        #endregion


        #region Static Methods


        public static bool Unsubscribe(int threadId, int userId)
        {
            return DBForums.ForumThreadUNSubscribe(threadId, userId);
        }

        public static bool UnsubscribeAll(int userId)
        {
            return DBForums.ForumThreadUnsubscribeAll(userId);
        }

        public static bool Unsubscribe(Guid subGuid)
        {
            return DBForums.ForumThreadUnSubscribe(subGuid);
        }

        public static bool UnsubscribeAll(Guid subGuid)
        {
            int userId = GetUserIdForSubscription(subGuid);
            return DBForums.ForumThreadUnsubscribeAll(userId);
        }

        public static int GetUserIdForSubscription(Guid subGuid)
        {
            int userId = -1;

            using (IDataReader reader = DBForums.ForumThreadGetSubscriber(subGuid))
            {
                if (reader.Read())
                {
                    userId = Convert.ToInt32(reader["UserID"]);
                }
            }


            return userId;

        }

        public static bool Delete(int threadId)
        {
            bool status = false;

            ForumThread forumThread = new ForumThread(threadId);

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PostID", typeof(int));

            using (IDataReader reader = DBForums.ForumThreadGetPosts(threadId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["PostID"] = reader["PostID"];
                    dataTable.Rows.Add(row);
                }
            }

            foreach (DataRow row in dataTable.Rows)
            {
                forumThread.DeletePost(Convert.ToInt32(row["PostID"]));
            }

            status = DBForums.ForumThreadDelete(threadId);

            return status;
        }

        public static int GetPostCount(int threadId)
        {
            return DBForums.ForumThreadGetPostCount(threadId);
        }


        public static DataTable GetPostsByPage(int siteId, int pageId)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PostID", typeof(int));
            dataTable.Columns.Add("ItemID", typeof(int));
            dataTable.Columns.Add("ThreadID", typeof(int));
            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("Subject", typeof(string));
            dataTable.Columns.Add("Post", typeof(string));
            dataTable.Columns.Add("ViewRoles", typeof(string));

            using (IDataReader reader = DBForums.ForumThreadGetPostsByPage(siteId, pageId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["PostID"] = reader["PostID"];
                    row["ItemID"] = reader["ItemID"];
                    row["ModuleID"] = reader["ModuleID"];
                    row["ThreadID"] = reader["ThreadID"];
                    row["ModuleTitle"] = reader["ModuleTitle"];
                    row["Subject"] = reader["Subject"];
                    row["Post"] = reader["Post"];
                    row["ViewRoles"] = reader["ViewRoles"];

                    dataTable.Rows.Add(row);

                }
            }

            return dataTable;
        }

        public static DataTable GetThreadsByPage(int siteId, int pageId)
        {
            DataTable dataTable = new DataTable();
            //dataTable.Columns.Add("PostID", typeof(int));
            dataTable.Columns.Add("ItemID", typeof(int));
            dataTable.Columns.Add("ThreadID", typeof(int));
            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("Subject", typeof(string));
            dataTable.Columns.Add("Post", typeof(string));
            dataTable.Columns.Add("ViewRoles", typeof(string));
            dataTable.Columns.Add("ThreadDate", typeof(DateTime));
            dataTable.Columns.Add("MostRecentPostDate", typeof(DateTime));

            using (IDataReader reader = DBForums.ForumThreadGetThreadsByPage(siteId, pageId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                   // row["PostID"] = reader["PostID"];
                    row["ItemID"] = reader["ItemID"];
                    row["ModuleID"] = reader["ModuleID"];
                    row["ThreadID"] = reader["ThreadID"];
                    row["ModuleTitle"] = reader["ModuleTitle"];
                    row["Subject"] = reader["ThreadSubject"];
                   // row["Post"] = reader["Post"];
                    row["ViewRoles"] = reader["ViewRoles"];
                    row["ThreadDate"] = reader["ThreadDate"];
                    row["MostRecentPostDate"] = reader["MostRecentPostDate"];

                    dataTable.Rows.Add(row);

                }
            }

            return dataTable;
        }


        public static DataTable GetPostsByThread(int threadId)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PostID", typeof(int));
            //dataTable.Columns.Add("ItemID", typeof(int));
            dataTable.Columns.Add("ThreadID", typeof(int));
            //dataTable.Columns.Add("ModuleID", typeof(int));
            //dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("Subject", typeof(string));
            dataTable.Columns.Add("Post", typeof(string));
            //dataTable.Columns.Add("ViewRoles", typeof(string));

            using (IDataReader reader = DBForums.ForumThreadGetPosts(threadId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["PostID"] = reader["PostID"];
                    //row["ItemID"] = reader["ItemID"];
                    //row["ModuleID"] = reader["ModuleID"];
                    row["ThreadID"] = reader["ThreadID"];
                    //row["ModuleTitle"] = reader["ModuleTitle"];
                    row["Subject"] = reader["Subject"];
                    row["Post"] = reader["Post"];
                    //row["ViewRoles"] = reader["ViewRoles"];

                    dataTable.Rows.Add(row);

                }
            }

            return dataTable;

        }


        public static IDataReader GetPageByUser(
            int userId,
            int siteId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {

            return DBForums.GetThreadPageByUser(
                userId,
                siteId,
                pageNumber,
                pageSize,
                out totalPages);

        }



        public static bool IsSubscribed(int threadId, int userId)
        {
            return DBForums.ForumThreadSubscriptionExists(threadId, userId);
        }

        public static IDataReader GetThreadsForSiteMap(int siteId)
        {
            return DBForums.GetThreadsForSiteMap(siteId);
        }


        #endregion

        #region IIndexableContent

        public event ContentChangedEventHandler ContentChanged;

        protected void OnContentChanged(ContentChangedEventArgs e)
        {
            if (ContentChanged != null)
            {
                ContentChanged(this, e);
            }
        }


        #endregion

        public delegate void ThreadMovedEventHandler(object sender, ForumThreadMovedArgs e);

        public event ThreadMovedEventHandler ThreadMoved;

        protected void OnThreadMoved(ForumThreadMovedArgs e)
        {
            if (ThreadMoved != null)
            {
                ThreadMoved(this, e);
            }
        }

    }
}
