// Author:					
// Created:				    2004-07-25
// Last Modified:		    2013-04-24
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
	/// Represents an instance of Html Content
	/// </summary>
    public class HtmlContent : IIndexableContent
	{
        private const string featureGuid = "881e4e00-93e4-444c-b7b0-6672fb55de10";

        public static Guid FeatureGuid
        {
            get { return new Guid(featureGuid); }
        }


		#region Constructors

		public HtmlContent()
		{}

        [Obsolete("This constructor is deprecated and may be removed in future versions. You should use HtmlRepository.Fetch instead.")]
        public HtmlContent(int moduleId)
        {
            this.moduleID = moduleId;
            GetHtmlContent(moduleId);

        }

        //[Obsolete("This constructor is deprecated and may be removed in future versions. You should use HtmlRepository.Fetch instead.")]
        //public HtmlContent(int moduleId, int itemId)
        //{
        //    this.moduleID = moduleId;
        //    GetHtmlContent(moduleId, itemId);
			
        //}

		#endregion

		#region Prviate Properties

        private Guid itemGuid = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;
		private int moduleID = -1;
		private int itemID = -1;
		private string title = string.Empty;
		private string excerpt = string.Empty;
		private string body = string.Empty;
		private string moreLink = string.Empty;
		private int sortOrder = 500;
		private DateTime beginDate = DateTime.UtcNow;
        private DateTime endDate = DateTime.UtcNow.AddYears(20);
        private DateTime createdDate = DateTime.UtcNow;
		private int createdBy = -1;
        private Guid userGuid = Guid.Empty;
        private Guid lastModUserGuid = Guid.Empty;
        private DateTime lastModUtc = DateTime.UtcNow;
        private int siteId = -1;
        private string searchIndexPath = string.Empty;
        private bool excludeFromRecentContent = false;

        

		#endregion


		#region Public Properties

        public Guid ItemGuid
        {
            get { return itemGuid; }
            set { itemGuid = value; }
        }

        public Guid ModuleGuid
        {
            get { return moduleGuid; }
            set { moduleGuid = value; }
        }

		public int ModuleId
		{	
			get {return moduleID;}
			set {moduleID = value;}
		}

		public int ItemId
		{	
			get {return itemID;}
			set {itemID = value;}
		}

		public string Title
		{	
			get {return title;}
			set {title = value;}
		}

		public string Excerpt
		{	
			get {return excerpt;}
			set {excerpt = value;}
		}

		public string Body
		{	
			get {return body;}
			set {body = value;}
		}

		public string MoreLink
		{	
			get {return moreLink;}
			set {moreLink = value;}
		}

		public int SortOrder
		{	
			get {return sortOrder;}
			set {sortOrder = value;}
		}

		public DateTime BeginDate
		{	
			get {return beginDate;}
			set {beginDate = value;}
		}

		public DateTime EndDate
		{	
			get {return endDate;}
			set {endDate = value;}
		}

		public DateTime CreatedDate
		{	
			get {return createdDate;}
            set { createdDate = value; }
			
		}
        [Obsolete("This property is obsolete, use UserGuid instead")]
        public int CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
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

        public DateTime LastModUtc
        {
            get { return lastModUtc; }
            set { lastModUtc = value; }
        }

        public bool ExcludeFromRecentContent
        {
            get { return excludeFromRecentContent; }
            set { excludeFromRecentContent = value; }
        }

        private string createdByName = string.Empty;

        public string CreatedByName
        {
            get { return createdByName; }
            set { createdByName = value; }
        }

        private string createdByFirstName = string.Empty;

        public string CreatedByFirstName
        {
            get { return createdByFirstName; }
            set { createdByFirstName = value; }
        }

        private string createdByLastName = string.Empty;

        public string CreatedByLastName
        {
            get { return createdByLastName; }
            set { createdByLastName = value; }
        }

        private string createdByEmail = string.Empty;

        public string CreatedByEmail
        {
            get { return createdByEmail; }
            set { createdByEmail = value; }
        }


        private string authorBio = string.Empty;

        public string AuthorBio
        {
            get { return authorBio; }
            set { authorBio = value; }
            
        }

        private string authorAvatar = string.Empty;

        public string AuthorAvatar
        {
            get { return authorAvatar; }
            set { authorAvatar = value; }
            
        }

        private int authorUserId = -1;

        public int AuthorUserId
        {
            get { return authorUserId; }
            set { authorUserId = value; }
        }

        private string lastModByName = string.Empty;

        public string LastModByName
        {
            get { return lastModByName; }
            set { lastModByName = value; }
        }

        private string lastModByFirstName = string.Empty;

        public string LastModByFirstName
        {
            get { return lastModByFirstName; }
            set { lastModByFirstName = value; }
        }

        private string lastModByLastName = string.Empty;

        public string LastModByLastName
        {
            get { return lastModByLastName; }
            set { lastModByLastName = value; }
        }

        private string lastModByEmail = string.Empty;

        public string LastModByEmail
        {
            get { return lastModByEmail; }
            set { lastModByEmail = value; }
        }

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

		#endregion

		#region Private Methods

        private void LoadFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                this.itemID = Convert.ToInt32(reader["ItemID"]);
                this.moduleID = Convert.ToInt32(reader["ModuleID"]);
                this.title = reader["Title"].ToString();
                //this.excerpt = reader["Excerpt"].ToString();
                this.body = reader["Body"].ToString();
                //this.moreLink = reader["MoreLink"].ToString();
                //this.sortOrder = Convert.ToInt32(reader["SortOrder"]);
                //this.beginDate = Convert.ToDateTime(reader["BeginDate"]);
                //this.endDate = Convert.ToDateTime(reader["EndDate"]);
                this.createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                //this.createdBy = Convert.ToInt32(reader["UserID"]);

                this.itemGuid = new Guid(reader["ItemGuid"].ToString());
                this.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                string user = reader["UserGuid"].ToString();
                if (user.Length == 36) this.userGuid = new Guid(user);

                user = reader["LastModUserGuid"].ToString();
                if (user.Length == 36) this.lastModUserGuid = new Guid(user);

                if(reader["LastModUtc"] != DBNull.Value)
                this.lastModUtc = Convert.ToDateTime(reader["LastModUtc"]);

                this.createdByName = reader["CreatedByName"].ToString();
                this.createdByFirstName = reader["CreatedByFirstName"].ToString();
                this.createdByLastName = reader["CreatedByLastName"].ToString();
                this.createdByEmail = reader["CreatedByEmail"].ToString();
                this.excludeFromRecentContent = Convert.ToBoolean(reader["ExcludeFromRecentContent"]);

                this.lastModByName = reader["LastModByName"].ToString();
                this.lastModByFirstName = reader["LastModByFirstName"].ToString();
                this.lastModByLastName = reader["LastModByLastName"].ToString();
                this.LastModByEmail = reader["LastModByEmail"].ToString();

            }

            

        }

        ///// <summary>
        ///// Gets an instance of HtmlContent.
        ///// </summary>
        ///// <param name="moduleID">moduleID</param>
        ///// <param name="itemID">itemID</param>
        //private void GetHtmlContent(int moduleId, int itemId) 
        //{
        //    using (IDataReader reader = DBHtmlContent.GetHtmlContent(moduleId, itemId))
        //    {
        //        LoadFromReader(reader);
        //    }
		
        //}

        /// <summary>
        /// Gets an instance of HtmlContent.
        /// </summary>
        /// <param name="moduleId">moduleId</param>
        private void GetHtmlContent(int moduleId)
        {
            using (IDataReader reader = DBHtmlContent.GetHtmlContent(moduleId, DateTime.UtcNow))
            {
                LoadFromReader(reader);
            }

        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
		private bool Create()
		{ 
			int newID = -1;
            this.itemGuid = Guid.NewGuid();

			newID = DBHtmlContent.AddHtmlContent(
                this.itemGuid,
                this.moduleGuid,
				this.moduleID, 
				this.title, 
				this.excerpt, 
				this.body, 
				this.moreLink, 
				this.sortOrder, 
				this.beginDate, 
				this.endDate, 
				this.createdDate, 
				this.createdBy,
                this.userGuid,
                this.excludeFromRecentContent); 
			
			this.itemID = newID;

            bool result = (newID > -1);

            //IndexHelper.IndexItem(this);
            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                OnContentChanged(e);
            }
					
			return result;

		}

        /// <summary>
        /// Updates this instance.
        /// </summary>
        /// <returns>bool</returns>
        [Obsolete("This method is deprecated and may be removed in future versions. You should use the corresponding method on HtmlRepository instead.")]
		private bool Update()
		{
            this.lastModUtc = DateTime.UtcNow;

			bool result = DBHtmlContent.UpdateHtmlContent(
				this.itemID, 
				this.moduleID, 
				this.title, 
				this.excerpt, 
				this.body, 
				this.moreLink, 
				this.sortOrder, 
				this.beginDate, 
				this.endDate,
				this.lastModUtc, 
				this.lastModUserGuid,
                this.excludeFromRecentContent);

            //IndexHelper.IndexItem(this);
            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                OnContentChanged(e);
            }

			return result;
				
		}



		#endregion

		#region Public Methods



        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
        [Obsolete("This method is deprecated and may be removed in future versions. You should use the corresponding method on HtmlRepository instead.")]
		public bool Save()
		{
			if(this.itemID > -1)
			{
				return Update();
			}
			else
			{
				return Create();
			}
			
		}

        public void CreateHistory(Guid siteGuid)
        {
            if (this.itemGuid == Guid.Empty) { return; }

            HtmlContent currentVersion = new HtmlContent(moduleID);
            if (currentVersion.Body == this.Body) { return; }

            ContentHistory history = new ContentHistory();
            history.ContentGuid = currentVersion.ModuleGuid;
            history.ContentText = currentVersion.Body;
            history.SiteGuid = siteGuid;
            history.UserGuid = currentVersion.LastModUserGuid;
            history.CreatedUtc = currentVersion.LastModUtc;
            history.Save();

        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <returns></returns>
        [Obsolete("This method is deprecated and may be removed in future versions. You should use the corresponding method on HtmlRepository instead.")]
        public bool Delete()
        {
            bool result = DBHtmlContent.DeleteHtmlContent(this.itemID);

            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                e.IsDeleted = true;
                OnContentChanged(e);
            }

            return result;

        }

        public void ApproveContent(Guid siteGuid, Guid approvalUserGuid, bool allowUnsubmitted)
        {
            // get latest workflow record waiting for approval
            ContentWorkflow submittedContent = ContentWorkflow.GetWorkInProgress(this.moduleGuid, ContentWorkflowStatus.AwaitingApproval.ToString());

            if ((submittedContent == null) && (allowUnsubmitted))
            {
                submittedContent = ContentWorkflow.GetWorkInProgress(this.moduleGuid, ContentWorkflowStatus.Draft.ToString());
            }

            if (submittedContent == null) { return; }

            // create a new content history record of the existing live content
            ContentHistory history = new ContentHistory();
            history.ContentGuid = ModuleGuid;
            history.ContentText = Body;
            history.SiteGuid = siteGuid;
            history.UserGuid = LastModUserGuid;
            history.CreatedUtc = LastModUtc;
            history.Save();

            //update the html with the approved content
            this.body = submittedContent.ContentText;
            this.lastModUserGuid = submittedContent.LastModUserGuid;
            this.lastModUtc = DateTime.UtcNow;
            this.Save();

            //update content workflow to show record is now approved
            submittedContent.Status = ContentWorkflowStatus.Approved;
            submittedContent.LastModUserGuid = approvalUserGuid;
            submittedContent.LastModUtc = DateTime.UtcNow;
            submittedContent.Save(); 

            
        }

        

        /// <summary>
        /// Approves a draft and sets it's status as AwaitingPublishing
        /// </summary>
        /// <param name="siteGuid"></param>
        /// <param name="approvalUserGuid"></param>
        public void ApproveContentForPublishing(Guid siteGuid, Guid approvalUserGuid)
        {
            // get latest workflow record waiting for approval
            ContentWorkflow submittedContent = ContentWorkflow.GetWorkInProgress(this.moduleGuid, ContentWorkflowStatus.AwaitingApproval.ToString());

            if (submittedContent == null) { return; }

            //update content workflow to show record is now AwaitingPublishing
            submittedContent.Status = ContentWorkflowStatus.AwaitingPublishing;
            submittedContent.LastModUserGuid = approvalUserGuid;
            submittedContent.LastModUtc = DateTime.UtcNow;
            submittedContent.Save();
        }

        /// <summary>
        /// Publishes an approved content draft.
        /// </summary>
        /// <param name="siteGuid"></param>
        /// <param name="publishingUserGuid"></param>
        public void PublishApprovedContent(Guid siteGuid, Guid publishingUserGuid)
        {
            ContentWorkflow approvedContent = ContentWorkflow.GetWorkInProgress(this.moduleGuid, ContentWorkflowStatus.AwaitingPublishing.ToString());

            if (approvedContent == null) { return; }

            // create a new content history record of the existing live content
            ContentHistory history = new ContentHistory();
            history.ContentGuid = ModuleGuid;
            history.ContentText = Body;
            history.SiteGuid = siteGuid;
            history.UserGuid = LastModUserGuid;
            history.CreatedUtc = LastModUtc;
            history.Save();

            //update the html with the approved content
            this.body = approvedContent.ContentText;
            Guid submitterGuid = ContentWorkflow.GetDraftSubmitter(approvedContent.Guid);
            if (submitterGuid != Guid.Empty)
            {
                this.lastModUserGuid = submitterGuid;
            }
            this.lastModUtc = DateTime.UtcNow;
            this.Save();

            //update content workflow to show record is now approved
            approvedContent.Status = ContentWorkflowStatus.Approved;
            approvedContent.LastModUserGuid = publishingUserGuid;
            approvedContent.LastModUtc = DateTime.UtcNow;

            approvedContent.Save();
        }

        public void PublishDraft(Guid siteGuid, Guid approvalUserGuid)
        {
            // get latest workflow record waiting for approval
            ContentWorkflow submittedContent = ContentWorkflow.GetWorkInProgress(this.moduleGuid, ContentWorkflowStatus.Draft.ToString());

            if (submittedContent == null) { return; }

            // create a new content history record of the existing live content
            ContentHistory history = new ContentHistory();
            history.ContentGuid = ModuleGuid;
            history.ContentText = Body;
            history.SiteGuid = siteGuid;
            history.UserGuid = LastModUserGuid;
            history.CreatedUtc = LastModUtc;
            history.Save();

            //update the html with the approved content
            this.body = submittedContent.ContentText;
            // Joe D - this line looks like it would be wrong if not using 3 level approval
            // so I commented it out. I also added the if not Guid.Empty but then decided it was still wrong
            // a draft could be pubklished directly by an editor without ever submitting for approval
            // Joe A 2013-04-24 integration comment
            //Guid submitterGuid = ContentWorkflow.GetDraftSubmitter(submittedContent.Guid);
            //if (submitterGuid != Guid.Empty)
            //{
            //    this.lastModUserGuid = submitterGuid;
            //}
            //else
            //{
               this.lastModUserGuid = submittedContent.LastModUserGuid;
            //}
            this.lastModUtc = DateTime.UtcNow;
            this.Save();

            //update content workflow to show record is now approved
            submittedContent.Status = ContentWorkflowStatus.Approved;
            submittedContent.LastModUserGuid = approvalUserGuid;
            submittedContent.LastModUtc = DateTime.UtcNow;
            submittedContent.Save();


        }


		#endregion

		#region Static Methods

        //[Obsolete("This method is deprecated and may be removed in future versions. You should use the corresponding method on HtmlRepository instead.")]
        //public static IDataReader GetHtml(int moduleId, DateTime beginDate)  
        //{
        //    return DBHtmlContent.GetHtmlContent(moduleId, beginDate);
        //}

        [Obsolete("This method is deprecated and may be removed in future versions. You should use the corresponding method on HtmlRepository instead.")]
        public static bool DeleteByModule(int moduleId)
        {
            return DBHtmlContent.DeleteByModule(moduleId);
        }

        [Obsolete("This method is deprecated and may be removed in future versions. You should use the corresponding method on HtmlRepository instead.")]
        public static bool DeleteBySite(int siteId)
        {
            return DBHtmlContent.DeleteBySite(siteId);
        }

        //public static bool DeleteItem(int moduleID,int itemID) 
        //{
        //    bool result = dbHtmlContent.DeleteHtmlContent(itemID);

        //    IndexHelper.RemoveIndexItem(moduleID, itemID);

        //    return result;
        //}

        [Obsolete("This method is deprecated and may be removed in future versions. You should use the corresponding method on HtmlRepository instead.")]
		public static DataTable GetHtmlContentByPage(int siteId, int pageId)
        {
            DataTable dataTable = new DataTable();
            
            dataTable.Columns.Add("ItemID", typeof(int));
            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Body", typeof(string));
            dataTable.Columns.Add("ViewRoles", typeof(string));

            using (IDataReader reader = DBHtmlContent.GetHtmlContentByPage(siteId, pageId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();

                    row["ItemID"] = reader["ItemID"];
                    row["ModuleID"] = reader["ModuleID"];
                    row["ModuleTitle"] = reader["ModuleTitle"];
                    row["Title"] = reader["Title"];
                    row["Body"] = reader["Body"];
                    row["ViewRoles"] = reader["ViewRoles"];

                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }


		#endregion

        #region IIndexableContent

        public event ContentChangedEventHandler ContentChanged;

        public void OnContentChanged(ContentChangedEventArgs e)
        {
            if (ContentChanged != null)
            {
                ContentChanged(this, e);
            }
        }

        #endregion
	}
}
