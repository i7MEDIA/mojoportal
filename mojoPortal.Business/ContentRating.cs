// Author:					
// Created:				    2008-10-06
// Last Modified:			2011-10-05
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    ///
    /// </summary>
    public class ContentRating
    {

        #region Constructors

        public ContentRating()
        { }


        public ContentRating(Guid rowGuid)
        {
            GetContentRating(rowGuid);
        }

        public ContentRating(Guid contentGuid, Guid userGuid)
        {
            GetContentRating(contentGuid, userGuid);
        }

        #endregion

        #region Private Properties

        private Guid rowGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid contentGuid = Guid.Empty;
        private Guid userGuid = Guid.Empty;
        private string emailAddress = string.Empty;
        private int rating = -1;
        private string comments = string.Empty;
        private string ipAddress = string.Empty;
        private DateTime createdUtc;
        private DateTime lastModUtc;

        #endregion

        #region Public Properties

        public Guid RowGuid
        {
            get { return rowGuid; }
            set { rowGuid = value; }
        }
        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }
        public Guid ContentGuid
        {
            get { return contentGuid; }
            set { contentGuid = value; }
        }
        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }
        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }
        public int Rating
        {
            get { return rating; }
            set { rating = value; }
        }
        public string Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }
        public DateTime CreatedUtc
        {
            get { return createdUtc; }
            set { createdUtc = value; }
        }
        public DateTime LastModUtc
        {
            get { return lastModUtc; }
            set { lastModUtc = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of ContentRating.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        private void GetContentRating(Guid rowGuid)
        {
            IDataReader reader = DBContentRating.GetOne(rowGuid);
            PopulateFromReader(reader);

        }

        /// <summary>
        /// Gets an instance of ContentRating.
        /// </summary>
        private void GetContentRating(Guid contentGuid, Guid userGuid)
        {
            IDataReader reader = DBContentRating.GetOneByContentAndUser(contentGuid, userGuid);
            PopulateFromReader(reader);

        }


        private void PopulateFromReader(IDataReader reader)
        {
            try
            {
                if (reader.Read())
                {
                    this.rowGuid = new Guid(reader["RowGuid"].ToString());
                    this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    this.contentGuid = new Guid(reader["ContentGuid"].ToString());
                    this.userGuid = new Guid(reader["UserGuid"].ToString());
                    this.emailAddress = reader["EmailAddress"].ToString();
                    this.rating = Convert.ToInt32(reader["Rating"]);
                    this.comments = reader["Comments"].ToString();
                    this.ipAddress = reader["IpAddress"].ToString();
                    this.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    this.lastModUtc = Convert.ToDateTime(reader["LastModUtc"]);

                }
            }
            finally
            {
                reader.Close();
            }
        }

        /// <summary>
        /// Persists a new instance of ContentRating. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.rowGuid = Guid.NewGuid();

            int rowsAffected = DBContentRating.Create(
                this.rowGuid,
                this.siteGuid,
                this.contentGuid,
                this.userGuid,
                this.emailAddress,
                this.rating,
                this.comments,
                this.ipAddress,
                this.createdUtc);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of ContentRating. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBContentRating.Update(
                this.rowGuid,
                this.emailAddress,
                this.rating,
                this.comments,
                this.ipAddress,
                this.lastModUtc);

        }





        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of ContentRating. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.rowGuid != Guid.Empty)
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

        /// <summary>
        /// Deletes an instance of ContentRating. Returns true on success.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            return DBContentRating.Delete(rowGuid);
        }


        ///// <summary>
        ///// Gets a count of ContentRating. 
        ///// </summary>
        //public static int GetCountByContent(Guid contentGuid)
        //{
        //    return DBContentRating.GetCountByContent(contentGuid);
        //}

        private static List<ContentRating> LoadListFromReader(IDataReader reader)
        {
            List<ContentRating> contentRatingList = new List<ContentRating>();
            using(reader)
            {
                while (reader.Read())
                {
                    ContentRating contentRating = new ContentRating();
                    contentRating.rowGuid = new Guid(reader["RowGuid"].ToString());
                    contentRating.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    contentRating.contentGuid = new Guid(reader["ContentGuid"].ToString());
                    contentRating.userGuid = new Guid(reader["UserGuid"].ToString());
                    contentRating.emailAddress = reader["EmailAddress"].ToString();
                    contentRating.rating = Convert.ToInt32(reader["Rating"]);
                    contentRating.comments = reader["Comments"].ToString();
                    contentRating.ipAddress = reader["IpAddress"].ToString();
                    contentRating.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    contentRating.lastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
                    contentRatingList.Add(contentRating);

                }
            }
            
            return contentRatingList;

        }

        

        /// <summary>
        /// Gets an IList with page of instances of ContentRating.
        /// </summary>
        public static List<ContentRating> GetPage(Guid contentGuid, int pageNumber, int pageSize, out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBContentRating.GetPage(contentGuid, pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }

        public static ContentRatingStats GetStats(Guid contentGuid)
        {
            ContentRatingStats stats = new ContentRatingStats();
            
            using(IDataReader reader = DBContentRating.GetStatsByContent(contentGuid))
            {
                if (reader.Read())
                {
                    stats.TotalVotes = Convert.ToInt32(reader["TotalRatings"]);
                    stats.AverageRating = Convert.ToInt32(reader["CurrentRating"]);
                }
            }

            return stats;

        }


        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of ContentRating.
        /// </summary>
        public static int CompareByEmailAddress(ContentRating contentRating1, ContentRating contentRating2)
        {
            return contentRating1.EmailAddress.CompareTo(contentRating2.EmailAddress);
        }
        /// <summary>
        /// Compares 2 instances of ContentRating.
        /// </summary>
        public static int CompareByRating(ContentRating contentRating1, ContentRating contentRating2)
        {
            return contentRating1.Rating.CompareTo(contentRating2.Rating);
        }
        /// <summary>
        /// Compares 2 instances of ContentRating.
        /// </summary>
        public static int CompareByComments(ContentRating contentRating1, ContentRating contentRating2)
        {
            return contentRating1.Comments.CompareTo(contentRating2.Comments);
        }
        /// <summary>
        /// Compares 2 instances of ContentRating.
        /// </summary>
        public static int CompareByIpAddress(ContentRating contentRating1, ContentRating contentRating2)
        {
            return contentRating1.IpAddress.CompareTo(contentRating2.IpAddress);
        }
        /// <summary>
        /// Compares 2 instances of ContentRating.
        /// </summary>
        public static int CompareByCreatedUtc(ContentRating contentRating1, ContentRating contentRating2)
        {
            return contentRating1.CreatedUtc.CompareTo(contentRating2.CreatedUtc);
        }
        /// <summary>
        /// Compares 2 instances of ContentRating.
        /// </summary>
        public static int CompareByLastModUtc(ContentRating contentRating1, ContentRating contentRating2)
        {
            return contentRating1.LastModUtc.CompareTo(contentRating2.LastModUtc);
        }

        public static void RateContent(
            Guid siteGuid,
            Guid contentGuid, 
            Guid userGuid,
            int rating,
            string emailAddress,
            string comments,
            string ipAddress,
            int minutesBetweenAnonymousVotes
            )
        {
            bool userHasRated = false;

            if (userGuid != Guid.Empty)
            {
               userHasRated = (DBContentRating.GetCountByContentAndUser(contentGuid, userGuid) > 0);
            }

            if (userHasRated)
            {
                ContentRating contentRating = new ContentRating(contentGuid, userGuid);
                contentRating.Rating = rating;
                if ((!string.IsNullOrEmpty(emailAddress))&&(contentRating.EmailAddress.Length == 0))
                {
                    contentRating.EmailAddress = emailAddress;
                }

                if ((!string.IsNullOrEmpty(comments)) && (contentRating.Comments.Length == 0))
                {
                    contentRating.Comments = comments;
                }

                contentRating.IpAddress = ipAddress;
                contentRating.Save();



            }
            else
            {
                bool doRating = true;

                if (userGuid == Guid.Empty)
                {
                    // if the anonymous user has rated this item from this ip address
                    // within the last x minutes, don't let them vote again
                    int countOfRatings = DBContentRating.GetCountOfRatingsSince(
                        contentGuid,
                        ipAddress,
                        DateTime.UtcNow.AddMinutes(-minutesBetweenAnonymousVotes));
                    

                    doRating = (countOfRatings == 0);

                }

                if (doRating)
                {
                    DBContentRating.Create(
                        Guid.NewGuid(),
                        siteGuid,
                        contentGuid,
                        userGuid,
                        emailAddress,
                        rating,
                        comments,
                        ipAddress,
                        DateTime.UtcNow);
                }
            }


        }

        #endregion


    }

}