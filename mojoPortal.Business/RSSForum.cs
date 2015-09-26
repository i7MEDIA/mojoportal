using System;
using System.Collections;
using System.Data;
using mojoPortal.Data;
//using System.Web;
//using System.Web.Caching;
//using Rss;

namespace mojoPortal.Business
{
	/// <summary>
	/// Author:					Joseph Hill
	/// Created:				2006-01-09
	/// Last Modified:			2007-11-03
	/// 
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
	public class RssForum
	{
		#region Constructors

		public RssForum()
		{}

		#endregion

		#region Private Properties
		private int siteID = -1;
		private int pageID = -1;
		private int moduleID = -1;
		private int itemID = -1;
		private int threadID = -1;
		private int maximumDays = -1;
		private DateTime createdDate; 
		
		#endregion

		#region Public Properties

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
		public int PageId 
		{
			get { return pageID; }
			set { pageID = value; }
		}
		public int SiteId 
		{
			get { return siteID; }
			set { siteID = value; }
		}
		public int ThreadId {
			get { return threadID; }
			set { threadID = value; }
		}
		public int MaximumDays {
			get { return maximumDays; }
			set { maximumDays = value; }
		}

        
		#endregion

		#region Public Methods
		public IDataReader GetPostsForRss() 
		{
			return DBForums.ForumThreadGetPostsForRss(SiteId, PageId, ModuleId, ItemId, ThreadId, MaximumDays); 
		}
		#endregion
		
	}
}
