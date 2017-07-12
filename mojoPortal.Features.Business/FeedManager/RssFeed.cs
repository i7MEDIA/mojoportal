// Author:					
// Created:				    2005-03-27
// Last Modified:			2009-10-20
// 
// Based on code example by Joseph Hill
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
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    /// Represents an RSS or Atom Feed
    /// </summary>
    /// 
    public class RssFeed
    {
        private const string featureGuid = "5ef82464-e2d3-4982-8dfd-01e1afa615f9";

        public static Guid FeatureGuid
        {
            get { return new Guid(featureGuid); }
        }

        #region Constructors

        public RssFeed()
        { }

        public RssFeed(int moduleId)
        {
            this.moduleID = moduleId;
        }


        public RssFeed(int moduleId, int itemId)
        {
            this.moduleID = moduleId;
            if (itemId > 0)
            {
                GetRssFeed(itemId);
            }

        }


        #endregion

        #region Private Properties

        private static readonly log4net.ILog log
            = log4net.LogManager.GetLogger(typeof(RssFeed));

        private Guid itemGuid = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;
        private Guid userGuid = Guid.Empty;
        private Guid lastModUserGuid = Guid.Empty;
        private DateTime lastModUtc = DateTime.UtcNow;

        private int itemID = -1;
        private int moduleID = -1;
        private DateTime createdDate = DateTime.UtcNow;
        private int userID = -1;
        private string author = string.Empty;
        private string url = string.Empty;
        private string rssUrl = string.Empty;
        private string imageUrl = string.Empty;
        private string feedType = "Rss";
        private bool publishByDefault = false;
        private int sortRank = 500;

        
        

        #endregion

        #region Public Properties

        public Guid ItemGuid
        {
            get { return itemGuid; }
        }

        public Guid ModuleGuid
        {
            get { return moduleGuid; }
            set { moduleGuid = value; }
        }

        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        public Guid LastModUserGuid
        {
            get { return lastModUserGuid; }
            set { lastModUserGuid = value; }
        }

        public int ItemId
        {
            get { return itemID; }
            set { itemID = value; }
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

        public DateTime LastModUtc
        {
            get { return lastModUtc; }
            set { lastModUtc = value; }
        }


        public int UserId
        {
            get { return userID; }
            set { userID = value; }
        }
        public string Author
        {
            get { return author; }
            set { author = value; }
        }
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        public string RssUrl
        {
            get { return rssUrl; }
            set { rssUrl = value; }
        }

        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }

        public string FeedType
        {
            get { return feedType; }
            set { feedType = value; }
        }

        public bool PublishByDefault
        {
            get { return publishByDefault; }
            set { publishByDefault = value; }
        }

        public int SortRank
        {
            get { return sortRank; }
            set { sortRank = value; }
        }

        #endregion

        #region Private Methods

        private void GetRssFeed(int itemId)
        {
            using (IDataReader reader = DBRssFeed.GetRssFeed(itemId))
            {
                if (reader.Read())
                {
                    this.itemID = Convert.ToInt32(reader["ItemID"]);
                    this.moduleID = Convert.ToInt32(reader["ModuleID"]);
                    this.createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                    this.userID = Convert.ToInt32(reader["UserID"]);
                    this.author = reader["Author"].ToString();
                    this.url = reader["Url"].ToString();
                    this.rssUrl = reader["RssUrl"].ToString();

                    this.imageUrl = reader["ImageUrl"].ToString();
                    this.feedType = reader["FeedType"].ToString();
                    this.publishByDefault = Convert.ToBoolean(reader["PublishByDefault"]);

                    this.itemGuid = new Guid(reader["ItemGuid"].ToString());
                    this.moduleGuid = new Guid(reader["ModuleGuid"].ToString());

                    string user = reader["UserGuid"].ToString();
                    if (user.Length == 36) this.userGuid = new Guid(user);

                    user = reader["LastModUserGuid"].ToString();
                    if (user.Length == 36) this.lastModUserGuid = new Guid(user);

                    if (reader["LastModUtc"] != DBNull.Value)
                        this.lastModUtc = Convert.ToDateTime(reader["LastModUtc"]);

                    this.sortRank = Convert.ToInt32(reader["SortRank"]);

                }

            }

        }

        private bool Create()
        {
            int newID = -1;
            this.itemGuid = Guid.NewGuid();
            this.createdDate = DateTime.UtcNow;
            this.lastModUtc = this.createdDate;

            newID = DBRssFeed.AddRssFeed(
                this.itemGuid,
                this.moduleGuid,
                this.userGuid,
                this.moduleID,
                this.userID,
                this.author,
                this.url,
                this.rssUrl,
                this.createdDate,
                this.imageUrl,
                this.feedType,
                this.publishByDefault,
                this.sortRank);

            this.itemID = newID;

            
            return (newID > -1);

        }

        private bool Update()
        {
            this.lastModUtc = DateTime.UtcNow;

            bool result = DBRssFeed.UpdateRssFeed(
                this.itemID,
                this.moduleID,
                this.author,
                this.url,
                this.rssUrl,
                this.lastModUserGuid,
                this.lastModUtc,
                this.imageUrl,
                this.feedType,
                this.publishByDefault,
                this.sortRank);

            

            return result;

        }



        #endregion


        #region Public Methods


        public bool Save()
        {
            if (this.itemID > 0)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }


        #endregion


        #region Static Methods

        public static bool DeleteFeed(int itemId)
        {
            return DBRssFeed.DeleteRssFeed(itemId);
        }

        public static DataTable GetFeeds(int moduleId)
        {
            
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ItemID", typeof(int));
            dataTable.Columns.Add("ItemGuid", typeof(Guid));
            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("ModuleGuid", typeof(Guid));
            dataTable.Columns.Add("Author", typeof(string));
            dataTable.Columns.Add("Url", typeof(string));
            dataTable.Columns.Add("RssUrl", typeof(string));
            dataTable.Columns.Add("ImageUrl", typeof(string));
            dataTable.Columns.Add("FeedType", typeof(string));
            dataTable.Columns.Add("PublishByDefault", typeof(bool));
            dataTable.Columns.Add("TotalEntries", typeof(int));


            using (IDataReader reader = DBRssFeed.GetFeeds(moduleId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["ItemID"] = reader["ItemID"];
                    row["ItemGuid"] = new Guid(reader["ItemGuid"].ToString());
                    row["ModuleID"] = reader["ModuleID"];
                    row["ModuleGuid"] = new Guid(reader["ModuleGuid"].ToString());
                    row["Author"] = reader["Author"];
                    row["Url"] = reader["Url"];
                    row["RssUrl"] = reader["RssUrl"];
                    row["ImageUrl"] = reader["ImageUrl"];
                    row["FeedType"] = reader["FeedType"];
                    row["PublishByDefault"] = Convert.ToBoolean(reader["PublishByDefault"]);
                    row["TotalEntries"] = Convert.ToInt32(reader["TotalEntries"]);

                    dataTable.Rows.Add(row);

                }
            }

            return dataTable;
        }


        /// <summary>
        /// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
        /// </summary>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteExpiredEntriesByModule(Guid moduleGuid, DateTime expiredDate)
        {
            return DBRssFeed.DeleteExpiredEntriesByModule(moduleGuid, expiredDate);
        }

        /// <summary>
        /// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
        /// </summary>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteUnPublishedEntriesByModule(Guid moduleGuid)
        {
            return DBRssFeed.DeleteUnPublishedEntriesByModule(moduleGuid);
        }

        /// <summary>
        /// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
        /// </summary>
        /// <param name="feedId"> feedId </param>
        /// <returns>bool</returns>
        public static bool DeleteUnPublishedEntriesByFeed(int feedId)
        {
            return DBRssFeed.DeleteUnPublishedEntriesByFeed(feedId);
        }

        /// <summary>
        /// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
        /// </summary>
        /// <param name="feedId"> feedId </param>
        /// <returns>bool</returns>
        public static bool DeleteEntriesByFeed(int feedId)
        {
            return DBRssFeed.DeleteEntriesByFeed(feedId);
        }

        /// <summary>
        /// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
        /// </summary>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteEntriesByModule(Guid moduleGuid)
        {
            return DBRssFeed.DeleteEntriesByModule(moduleGuid);
        }

        public static bool DeleteByModule(int moduleId)
        {
            return DBRssFeed.DeleteByModule(moduleId);
        }

        public static bool DeleteBySite(int siteId)
        {
            return DBRssFeed.DeleteBySite(siteId);
        }

        /// <summary>
        /// Gets a count of rows in the mp_RssFeedEntries table.
        /// </summary>
        public static bool EntryExists(Guid moduleGuid, int entryHash)
        {
            return DBRssFeed.EntryExists(moduleGuid, entryHash);
        }

        /// <summary>
        /// Gets the most recent cache time for the module
        /// </summary>
        public static DateTime GetLastCacheTime(Guid moduleGuid)
        {
            return DBRssFeed.GetLastCacheTime(moduleGuid);
        }

        /// <summary>
        /// Inserts a row in the mp_RssFeedEntries table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="feedGuid"> feedGuid </param>
        /// <param name="pubDate"> pubDate </param>
        /// <param name="title"> title </param>
        /// <param name="author"> author </param>
        /// <param name="blogUrl"> blogUrl </param>
        /// <param name="description"> description </param>
        /// <param name="link"> link </param>
        /// <param name="confirmed"> confirmed </param>
        /// <param name="entryHash"> entryHash </param>
        /// <param name="cachedTimeUtc"> cachedTimeUtc </param>
        /// <returns>int</returns>
        public static int CreateEntry(
            Guid rowGuid,
            Guid moduleGuid,
            Guid feedGuid,
            int feedId,
            DateTime pubDate,
            string title,
            string author,
            string blogUrl,
            string description,
            string link,
            bool confirmed,
            int entryHash,
            DateTime cachedTimeUtc)
        {
            return DBRssFeed.CreateEntry(
                rowGuid,
                moduleGuid,
                feedGuid,
                feedId,
                pubDate,
                title,
                author,
                blogUrl,
                description,
                link,
                confirmed,
                entryHash,
                cachedTimeUtc);

        }

        /// <summary>
        /// Updates a row in the mp_RssFeedEntries table. Returns true if row updated.
        /// </summary>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="title"> title </param>
        /// <param name="author"> author </param>
        /// <param name="blogUrl"> blogUrl </param>
        /// <param name="description"> description </param>
        /// <param name="link"> link </param>
        /// <param name="entryHash"> entryHash </param>
        /// <param name="cachedTimeUtc"> cachedTimeUtc </param>
        /// <returns>bool</returns>
        public static bool UpdateEnry(
            Guid moduleGuid,
            string title,
            string author,
            string blogUrl,
            string description,
            string link,
            int entryHash,
            DateTime cachedTimeUtc)
        {
            return DBRssFeed.UpdateEnry(
                moduleGuid,
                title,
                author,
                blogUrl,
                description,
                link,
                entryHash,
                cachedTimeUtc);

        }

        /// <summary>
        /// Updates a row in the mp_RssFeedEntries table. Returns true if row updated.
        /// </summary>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="entryHash"> entryHash </param>
        public static void Publish(
            Guid moduleGuid,
            int entryHash)
        {

            DBRssFeed.UpdatePublishing(
                moduleGuid,
                true,
                entryHash);
        }

        /// <summary>
        /// Updates a row in the mp_RssFeedEntries table. Returns true if row updated.
        /// </summary>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="entryHash"> entryHash </param>
        public static void UnPublish(
            Guid moduleGuid,
            int entryHash)
        {

            DBRssFeed.UpdatePublishing(
                moduleGuid,
                false,
                entryHash);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_RssFeedEntries table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static DataTable GetEntries(Guid moduleGuid)
        {

            return DBRssFeed.GetEntries(moduleGuid);
            
            //DataTable dataTable = new DataTable();
            //dataTable.Columns.Add("FeedId", typeof(int));
            //dataTable.Columns.Add("FeedName", typeof(string));
            //dataTable.Columns.Add("PubDate", typeof(DateTime));
            //dataTable.Columns.Add("Author", typeof(string));
            //dataTable.Columns.Add("Title", typeof(string));
            //dataTable.Columns.Add("Description", typeof(string));
            //dataTable.Columns.Add("BlogUrl", typeof(string));
            //dataTable.Columns.Add("Link", typeof(string));
            //dataTable.Columns.Add("Confirmed", typeof(bool));
            //dataTable.Columns.Add("EntryHash", typeof(int));

            //using (IDataReader reader = DBRssFeed.GetEntries(moduleGuid))
            //{
            //    while (reader.Read())
            //    {
            //        DataRow row = dataTable.NewRow();
            //        row["FeedId"] = reader["FeedId"];
            //        row["FeedName"] = reader["FeedName"];
            //        row["PubDate"] = reader["PubDate"];
            //        row["Author"] = reader["Author"];
            //        row["Title"] = reader["Title"];
            //        row["Description"] = reader["Description"];
            //        row["BlogUrl"] = reader["BlogUrl"];
            //        row["Link"] = reader["Link"];
            //        row["Confirmed"] = Convert.ToBoolean(reader["Confirmed"]);
            //        row["EntryHash"] = reader["EntryHash"];

            //        dataTable.Rows.Add(row);

            //    }
            //}

            //return dataTable;
        }
        


        #endregion

    }
}
