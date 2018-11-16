// Author:					
// Created:				    2008-11-19
// Last Modified:			2009-02-01
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
    /// <summary>
    ///	Represents urls that have been permanently moved.
    /// </summary>
    public class RedirectInfo
    {

        #region Constructors

        public RedirectInfo()
        { }


        public RedirectInfo(
            Guid rowGuid)
        {
            GetRedirectList(
                rowGuid);
        }

        #endregion

        #region Private Properties

        private Guid rowGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private int siteId = -1;
        private string oldUrl = string.Empty;
        private string newUrl = string.Empty;
        private DateTime createdUtc = DateTime.UtcNow;
        private DateTime expireUtc = DateTime.UtcNow;

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
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        public string OldUrl
        {
            get { return oldUrl; }
            set { oldUrl = value; }
        }
        public string NewUrl
        {
            get { return newUrl; }
            set { newUrl = value; }
        }
        public DateTime CreatedUtc
        {
            get { return createdUtc; }
            set { createdUtc = value; }
        }
        /// <summary>
        /// The date that the old url expired or expires
        /// </summary>
        public DateTime ExpireUtc
        {
            get { return expireUtc; }
            set { expireUtc = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of RedirectList.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        private void GetRedirectList(Guid rowGuid)
        {
            using (IDataReader reader = DBRedirectList.GetOne(rowGuid))
            {
                PopulateFromReader(reader);
            }

        }


        private void PopulateFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                this.rowGuid = new Guid(reader["RowGuid"].ToString());
                this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                this.siteId = Convert.ToInt32(reader["SiteID"]);
                this.oldUrl = reader["OldUrl"].ToString();
                this.newUrl = reader["NewUrl"].ToString();
                this.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                this.expireUtc = Convert.ToDateTime(reader["ExpireUtc"]);

            }
            
        }

        /// <summary>
        /// Persists a new instance of RedirectList. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.rowGuid = Guid.NewGuid();
            //https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=11174~1#post46572
            if (string.IsNullOrEmpty(this.oldUrl)) { return false; }

            int rowsAffected = DBRedirectList.Create(
                this.rowGuid,
                this.siteGuid,
                this.siteId,
                this.oldUrl,
                this.newUrl,
                this.createdUtc,
                this.expireUtc);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of RedirectList. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            //https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=11174~1#post46572
            if (string.IsNullOrEmpty(this.oldUrl)) { return false; }

            return DBRedirectList.Update(
                this.rowGuid,
                this.oldUrl,
                this.newUrl,
                this.expireUtc);

        }





        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of RedirectList. Returns true on success.
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
        /// Deletes an instance of RedirectList. Returns true on success.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            return DBRedirectList.Delete(rowGuid);
        }


        /// <summary>
        /// Gets a count of RedirectList. 
        /// </summary>
        public static int GetCount(int siteId)
        {
            return DBRedirectList.GetCount(siteId);
        }

        private static List<RedirectInfo> LoadListFromReader(IDataReader reader)
        {
            List<RedirectInfo> redirectListList = new List<RedirectInfo>();
            try
            {
                while (reader.Read())
                {
                    RedirectInfo redirectList = new RedirectInfo();
                    redirectList.rowGuid = new Guid(reader["RowGuid"].ToString());
                    redirectList.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    redirectList.siteId = Convert.ToInt32(reader["SiteID"]);
                    redirectList.oldUrl = reader["OldUrl"].ToString();
                    redirectList.newUrl = reader["NewUrl"].ToString();
                    redirectList.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    redirectList.expireUtc = Convert.ToDateTime(reader["ExpireUtc"]);
                    redirectListList.Add(redirectList);

                }
            }
            finally
            {
                reader.Close();
            }

            return redirectListList;

        }



		/// <summary>
		/// Gets an IList with page of instances of RedirectList.
		/// </summary>
		//public static List<RedirectInfo> GetPage(int siteId, int pageNumber, int pageSize, out int totalPages)
		//{
		//	totalPages = 1;
		//	IDataReader reader = DBRedirectList.GetPage(siteId, pageNumber, pageSize, out totalPages);
		//	return LoadListFromReader(reader);
		//}

		/// <summary>
		/// Gets an IList with page of instances of RedirectList with search term.
		/// </summary>
		public static List<RedirectInfo> GetPage(int siteId, int pageNumber, int pageSize, out int totalPages, string searchTerm = "")
		{
			totalPages = 1;
			IDataReader reader = DBRedirectList.GetPage(siteId, pageNumber, pageSize, out totalPages, searchTerm);
			return LoadListFromReader(reader);
		}

		/// <summary>
		/// Gets an IDataReader with one row from the mp_RedirectList table.
		/// </summary>
		public static IDataReader GetBySiteAndUrl(int siteId, string oldUrl)
        {
            return DBRedirectList.GetBySiteAndUrl(siteId, oldUrl);
        }

        /// <summary>
        /// returns true if the record exists
        /// </summary>
        public static bool Exists(int siteId, string oldUrl)
        {
            return DBRedirectList.Exists(siteId, oldUrl);
        }

        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of RedirectList.
        /// </summary>
        public static int CompareBySiteID(RedirectInfo redirectList1, RedirectInfo redirectList2)
        {
            return redirectList1.SiteId.CompareTo(redirectList2.SiteId);
        }
        /// <summary>
        /// Compares 2 instances of RedirectList.
        /// </summary>
        public static int CompareByOldUrl(RedirectInfo redirectList1, RedirectInfo redirectList2)
        {
            return redirectList1.OldUrl.CompareTo(redirectList2.OldUrl);
        }
        /// <summary>
        /// Compares 2 instances of RedirectList.
        /// </summary>
        public static int CompareByNewUrl(RedirectInfo redirectList1, RedirectInfo redirectList2)
        {
            return redirectList1.NewUrl.CompareTo(redirectList2.NewUrl);
        }
        /// <summary>
        /// Compares 2 instances of RedirectList.
        /// </summary>
        public static int CompareByCreatedUtc(RedirectInfo redirectList1, RedirectInfo redirectList2)
        {
            return redirectList1.CreatedUtc.CompareTo(redirectList2.CreatedUtc);
        }
        /// <summary>
        /// Compares 2 instances of RedirectList.
        /// </summary>
        public static int CompareByExpireUtc(RedirectInfo redirectList1, RedirectInfo redirectList2)
        {
            return redirectList1.ExpireUtc.CompareTo(redirectList2.ExpireUtc);
        }

        #endregion


    }

}
