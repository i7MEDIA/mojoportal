// Author:					
// Created:				    2005-06-01
// Last Modified:			2010-02-16
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
using mojoPortal.Data;

namespace mojoPortal.Business
{
	/// <summary>
	///
	/// </summary>
	public class FriendlyUrl
	{

		#region Constructors

		public FriendlyUrl()
		{}
	    
	
		public FriendlyUrl(int urlId) 
		{
			GetFriendlyUrl(urlId); 
		}

        public FriendlyUrl(string hostName, string friendlyUrl)
        {
            GetFriendlyUrl(hostName, friendlyUrl);
        }

        public FriendlyUrl(int siteId, string friendlyUrl)
        {
            if (string.IsNullOrEmpty(friendlyUrl)) { return; }
            GetFriendlyUrl(siteId, friendlyUrl);
        }

		#endregion

		#region Private Properties

        private Guid itemGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid pageGuid = Guid.Empty;
		private int urlID = -1; 
		private int siteID = -1; 
		private string friendlyUrl = string.Empty; 
		private string realUrl = string.Empty; 
		private bool isPattern; 
		private bool foundFriendlyUrl = false;

        
		
		#endregion

		#region Public Properties

        public Guid ItemGuid
        {
            get { return itemGuid; }

        }

        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }

        public Guid PageGuid
        {
            get { return pageGuid; }
            set { pageGuid = value; }
        }

		public bool FoundFriendlyUrl 
		{
			get { return foundFriendlyUrl; }
			
		}

		public int UrlId 
		{
			get { return urlID; }
			set { urlID = value; }
		}
		public int SiteId 
		{
			get { return siteID; }
			set { siteID = value; }
		}
		public string Url 
		{
			get { return friendlyUrl; }
			set { friendlyUrl = value; }
		}
		public string RealUrl 
		{
			get { return realUrl; }
			set 
			{ 
				if(!value.StartsWith("~/"))
				{
					value = "~/" + value;
				}
				realUrl = value; 
			}
		}
		public bool IsPattern 
		{
			get { return isPattern; }
			set { isPattern = value; }
		}

		#endregion

		#region Private Methods

		private void GetFriendlyUrl(int urlId) 
		{
            using (IDataReader reader = DBFriendlyUrl.GetFriendlyUrl(urlId))
            {
                GetFriendlyUrl(reader);
            }
			
		}

        private void GetFriendlyUrl(int siteId, string friendlyUrl)
        {
            using (IDataReader reader = DBFriendlyUrl.GetFriendlyUrl(siteId, friendlyUrl))
            {
                GetFriendlyUrl(reader);
            }

        }

        private void GetFriendlyUrl(string hostName, string friendlyUrl)
        {
            using (IDataReader reader = DBFriendlyUrl.GetByUrl(hostName, friendlyUrl))
            {
                GetFriendlyUrl(reader);
            }

        }

        private void GetFriendlyUrl(IDataReader reader)
        {
            if (reader.Read())
            {
                this.foundFriendlyUrl = true;
                this.urlID = Convert.ToInt32(reader["UrlID"]);
                this.siteID = Convert.ToInt32(reader["SiteID"]);
                this.friendlyUrl = reader["FriendlyUrl"].ToString();
                this.realUrl = reader["RealUrl"].ToString();
                this.isPattern = Convert.ToBoolean(reader["IsPattern"]);
                string pg = reader["PageGuid"].ToString();
                if (pg.Length == 36) this.pageGuid = new Guid(pg);
                this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                this.itemGuid = new Guid(reader["ItemGuid"].ToString());

            }
        }
		
		private bool Create()
		{ 
			int newID = 0;
            this.itemGuid = Guid.NewGuid();

			newID = DBFriendlyUrl.AddFriendlyUrl(
                this.itemGuid,
                this.siteGuid,
                this.pageGuid,
				this.siteID, 
				this.friendlyUrl, 
				this.realUrl, 
				this.isPattern); 
			
			this.urlID = newID;
	
			return (newID > 0);

		}

		private bool Update()
		{

			return DBFriendlyUrl.UpdateFriendlyUrl(
				this.urlID, 
				this.siteID, 
                this.PageGuid,
				this.friendlyUrl, 
				this.realUrl, 
				this.isPattern); 
				
		}


		#endregion

		#region Public Methods

		public bool Save()
		{
            if (friendlyUrl.Trim().Length == 0) { return false; }
            if (realUrl.Length == 0) { return false; }

			if( this.urlID > 0)
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


        //public static DataTable GetByHostName(string  hostName)  
        //{
        //    return DBFriendlyUrl.GetByHostName(hostName);
		
        //}

        //public static DataTable GetBySite(int siteId)
        //{
        //    return DBFriendlyUrl.GetBySite(siteId);

        //}

        /// <summary>
        /// Gets a page of data from the mp_FriendlyUrls table.
        /// </summary>
        /// <param name="siteId">The siteId.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            int siteId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBFriendlyUrl.GetPage(
                siteId,
                pageNumber,
                pageSize,
                out totalPages);

        }

        public static IDataReader GetPage(
            int siteId,
            string searchTerm,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBFriendlyUrl.GetPage(
                siteId,
                searchTerm,
                pageNumber,
                pageSize,
                out totalPages);

        }


		public static bool DeleteUrl(int urlId) 
		{
			return DBFriendlyUrl.DeleteFriendlyUrl(urlId);
		}

        //public static bool DeleteUrlByPageId(int pageId)
        //{
        //    return DBFriendlyUrl.DeleteByPageId(pageId); ;
        //}

        public static bool DeleteByPageGuid(Guid pageGuid)
        {
            return DBFriendlyUrl.DeleteByPageGuid(pageGuid);
        }

        public static bool Exists(int siteId, String friendlyUrl)
        {
            bool result = false;

            using (IDataReader reader = DBFriendlyUrl.GetFriendlyUrl(siteId, friendlyUrl))
            {
                while (reader.Read())
                {
                    result = true;
                }
            }

            return result;

        }

        [Obsolete("This method is obsolete. You should use SiteUtils.SuggestFriendlyUrl")]
        public static String SuggestFriendlyUrl(
            String pageName, 
            SiteSettings siteSettings)
        {
            String friendlyUrl = CleanStringForUrl(pageName);

            switch (siteSettings.DefaultFriendlyUrlPattern)
            {
                case SiteSettings.FriendlyUrlPattern.PageNameWithDotASPX:
                    friendlyUrl += ".aspx";
                    break;

            }

            int i = 1;
            while (FriendlyUrl.Exists(siteSettings.SiteId, friendlyUrl))
            {
                friendlyUrl = i.ToString() + friendlyUrl;
            }

            bool forceToLowerCase = GetBoolPropertyFromConfig("ForceFriendlyUrlsToLowerCase", true);

            if (forceToLowerCase) return friendlyUrl.ToLower();

            return friendlyUrl;
        }


        
        public static String CleanStringForUrl(String input)
        {
            String ouputString = RemovePunctuation(input).Replace(" - ", "-").Replace(" ", "-").Replace("/", String.Empty).Replace("\"", String.Empty).Replace("'", String.Empty).Replace("#", String.Empty).Replace("~", String.Empty).Replace("`", String.Empty).Replace("@", String.Empty).Replace("$", String.Empty).Replace("*", String.Empty).Replace("^", String.Empty).Replace("(", String.Empty).Replace(")", String.Empty).Replace("+", String.Empty).Replace("=", String.Empty).Replace("%", String.Empty).Replace(">", String.Empty).Replace("<", String.Empty);
            
            return ouputString;

        }

        public static String RemovePunctuation(String input)
        {
            String outputString = String.Empty;
            if (input != null)
            {
                outputString = input.Replace(".", String.Empty).Replace(",", String.Empty).Replace(":", String.Empty).Replace("?", String.Empty).Replace("!", String.Empty).Replace(";", String.Empty).Replace("&", String.Empty).Replace("{", String.Empty).Replace("}", String.Empty).Replace("[", String.Empty).Replace("]", String.Empty);
            }
            return outputString;
        }


        private static bool GetBoolPropertyFromConfig(string key, bool defaultValue)
        {
            if (ConfigurationManager.AppSettings[key] == null) return defaultValue;

            if (string.Equals(ConfigurationManager.AppSettings[key], "true", StringComparison.InvariantCultureIgnoreCase)) 
                return true;
            
            if (string.Equals(ConfigurationManager.AppSettings[key], "false", StringComparison.InvariantCultureIgnoreCase))
                return false;
            
            return defaultValue;


        }


		#endregion


	}
	
}