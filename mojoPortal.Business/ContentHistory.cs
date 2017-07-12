// Author:					
// Created:					2009-03-31
// Last Modified:			2009-04-09
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business
{

    public class ContentHistory
    {

        #region Constructors

        public ContentHistory()
        { }


        public ContentHistory(Guid guid)
        {
            GetContentHistory(guid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid userGuid = Guid.Empty;
        private Guid contentGuid = Guid.Empty;
        private string title = string.Empty;
        private string contentText = string.Empty;
        private string customData = string.Empty;
        private DateTime createdUtc = DateTime.UtcNow;
        private DateTime historyUtc = DateTime.UtcNow;
        private string userName = string.Empty;
        private string userLogin = string.Empty;

        

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }
        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }
        public Guid ContentGuid
        {
            get { return contentGuid; }
            set { contentGuid = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string ContentText
        {
            get { return contentText; }
            set { contentText = value; }
        }
        public string CustomData
        {
            get { return customData; }
            set { customData = value; }
        }
        public DateTime CreatedUtc
        {
            get { return createdUtc; }
            set { createdUtc = value; }
        }
        public DateTime HistoryUtc
        {
            get { return historyUtc; }
            set { historyUtc = value; }
        }

        public string UserName
        {
            get { return userName; }
        }

        public string UserLogin
        {
            get { return userLogin; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of ContentHistory.
        /// </summary>
        /// <param name="guid"> guid </param>
        private void GetContentHistory(Guid guid)
        {
            using (IDataReader reader = DBContentHistory.GetOne(guid))
            {
                PopulateFromReader(reader);
            }

        }


        private void PopulateFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                this.guid = new Guid(reader["Guid"].ToString());
                this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                this.userGuid = new Guid(reader["UserGuid"].ToString());
                this.contentGuid = new Guid(reader["ContentGuid"].ToString());
                this.title = reader["Title"].ToString();
                this.contentText = reader["ContentText"].ToString();
                this.customData = reader["CustomData"].ToString();
                this.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                this.historyUtc = Convert.ToDateTime(reader["HistoryUtc"]);
                this.userLogin = reader["LoginName"].ToString();
                this.userName = reader["Name"].ToString();

            }

        }

        /// <summary>
        /// Persists a new instance of ContentHistory. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.guid = Guid.NewGuid();

            int rowsAffected = DBContentHistory.Create(
                this.guid,
                this.siteGuid,
                this.userGuid,
                this.contentGuid,
                this.title,
                this.contentText,
                this.customData,
                this.createdUtc,
                this.historyUtc);

            return (rowsAffected > 0);

        }


        





        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of ContentHistory. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            return Create(); 
        }




        #endregion

        #region Static Methods

        /// <summary>
        /// Deletes an instance of ContentHistory. Returns true on success.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            return DBContentHistory.Delete(guid);
        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            return DBContentHistory.DeleteBySite(siteGuid);
        }

        public static bool DeleteByContent(Guid contentGuid)
        {
            return DBContentHistory.DeleteByContent(contentGuid);
        }


        /// <summary>
        /// Gets a count of ContentHistory. 
        /// </summary>
        public static int GetCount(Guid contentGuid)
        {
            return DBContentHistory.GetCount(contentGuid);
        }

        private static List<ContentHistory> LoadListFromReader(IDataReader reader)
        {
            List<ContentHistory> contentHistoryList = new List<ContentHistory>();
            try
            {
                while (reader.Read())
                {
                    ContentHistory contentHistory = new ContentHistory();
                    contentHistory.guid = new Guid(reader["Guid"].ToString());
                    contentHistory.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    contentHistory.userGuid = new Guid(reader["UserGuid"].ToString());
                    contentHistory.contentGuid = new Guid(reader["ContentGuid"].ToString());
                    contentHistory.title = reader["Title"].ToString();
                    contentHistory.contentText = reader["ContentText"].ToString();
                    contentHistory.customData = reader["CustomData"].ToString();
                    contentHistory.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    contentHistory.historyUtc = Convert.ToDateTime(reader["HistoryUtc"]);
                    contentHistory.userLogin = reader["LoginName"].ToString();
                    contentHistory.userName = reader["Name"].ToString();
                    contentHistoryList.Add(contentHistory);

                }
            }
            finally
            {
                reader.Close();
            }

            return contentHistoryList;

        }

        

        /// <summary>
        /// Gets an IList with page of instances of ContentHistory.
        /// </summary>
        public static List<ContentHistory> GetPage(
            Guid contentGuid,
            int pageNumber, 
            int pageSize, 
            out int totalPages)
        {
            totalPages = 1;

            IDataReader reader = DBContentHistory.GetPage(
                contentGuid,
                pageNumber, 
                pageSize, 
                out totalPages);

            return LoadListFromReader(reader);
        }

        public static IDataReader GetPageAsReader(
            Guid contentGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBContentHistory.GetPage(
                contentGuid,
                pageNumber,
                pageSize,
                out totalPages);
        }



        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of ContentHistory.
        /// </summary>
        public static int CompareByTitle(ContentHistory contentHistory1, ContentHistory contentHistory2)
        {
            return contentHistory1.Title.CompareTo(contentHistory2.Title);
        }
        /// <summary>
        /// Compares 2 instances of ContentHistory.
        /// </summary>
        public static int CompareByContentText(ContentHistory contentHistory1, ContentHistory contentHistory2)
        {
            return contentHistory1.ContentText.CompareTo(contentHistory2.ContentText);
        }
        /// <summary>
        /// Compares 2 instances of ContentHistory.
        /// </summary>
        public static int CompareByCustomData(ContentHistory contentHistory1, ContentHistory contentHistory2)
        {
            return contentHistory1.CustomData.CompareTo(contentHistory2.CustomData);
        }
        /// <summary>
        /// Compares 2 instances of ContentHistory.
        /// </summary>
        public static int CompareByCreatedUtc(ContentHistory contentHistory1, ContentHistory contentHistory2)
        {
            return contentHistory1.CreatedUtc.CompareTo(contentHistory2.CreatedUtc);
        }
        /// <summary>
        /// Compares 2 instances of ContentHistory.
        /// </summary>
        public static int CompareByHistoryUtc(ContentHistory contentHistory1, ContentHistory contentHistory2)
        {
            return contentHistory1.HistoryUtc.CompareTo(contentHistory2.HistoryUtc);
        }

        #endregion


    }

}
