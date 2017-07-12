using System;
using System.Collections;
using System.Data;
using mojoPortal.Data;


namespace mojoPortal.Business
{
    /// <summary>
    /// Author:					
    /// Created:				2005-03-27
    /// Last Modified:			2008-01-28
    /// 
    /// Based on code example by Joseph Hill
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    /// </summary>
    /// 
    public class RssFeed
    {
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


        #endregion

        #region Private Methods

        private void GetRssFeed(int itemId)
        {
            IDataReader reader = DBRssFeed.GetRssFeed(itemId);

            if (reader.Read())
            {
                this.itemID = Convert.ToInt32(reader["ItemID"]);
                this.moduleID = Convert.ToInt32(reader["ModuleID"]);
                this.createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                this.userID = Convert.ToInt32(reader["UserID"]);
                this.author = reader["Author"].ToString();
                this.url = reader["Url"].ToString();
                this.rssUrl = reader["RssUrl"].ToString();

                this.itemGuid = new Guid(reader["ItemGuid"].ToString());
                this.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                
                string user = reader["UserGuid"].ToString();
                if (user.Length == 36) this.userGuid = new Guid(user);

                user = reader["LastModUserGuid"].ToString();
                if (user.Length == 36) this.lastModUserGuid = new Guid(user);

                if(reader["LastModUtc"] != DBNull.Value)
                this.lastModUtc = Convert.ToDateTime(reader["LastModUtc"]);

            }

            reader.Close();

        }

        private bool Create()
        {
            int newID = 0;
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
                this.createdDate);

            this.itemID = newID;

            // TODO: move to UI
            //RSSFeed.ClearCache(this.moduleID);

            return (newID > 0);

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
                this.lastModUtc);

            // TODO: move to UI
            //RSSFeed.ClearCache(this.moduleID);

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

        public static DataSet GetFeeds(int moduleId)
        {
            return DBRssFeed.GetFeeds(moduleId);
        }


        

        

        #endregion

    }
}
