/// Author:					    Joe Audette
/// Created:				    2005-01-05
/// Last Modified:			    2013-02-18
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
//using System.IO;
using System.Data;
using log4net;
using mojoPortal.Data;

namespace mojoPortal.Business
{
	/// <summary>
	/// Represents an instance of a file in the shared files feature
	/// </summary>
    public class SharedFile : IIndexableContent
	{
        private const string featureGuid = "dc873d76-5bf2-4ac5-bff7-434a87a3fc8e";

        public static Guid FeatureGuid
        {
            get { return new Guid(featureGuid); }
        }

		#region Constructors

		public SharedFile()
		{}
	    
	
		public SharedFile(int moduleId, int itemId) 
		{
			GetSharedFile(itemId); 
		}

		#endregion

		#region Private Properties

		private static readonly ILog log = LogManager.GetLogger(typeof(SharedFile));

        private Guid itemGuid = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;
        private Guid userGuid = Guid.Empty;
        private Guid folderGuid = Guid.Empty;

		private int itemID = -1; 
		private int moduleID = -1; 
		private int uploadUserID; 
		private string friendlyName = string.Empty; 
		private string originalFileName = string.Empty; 
		private string serverFileName = string.Empty; 
		private int sizeInKB = 0; 
		private DateTime uploadDate = DateTime.UtcNow; 
		private int folderID = -1;
        private string description = string.Empty;
        private int downloadCount = 0;
		
//		private bool errorsOccurred = false;
//		private string errorMessage = string.Empty;
    
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

        public Guid FolderGuid
        {
            get { return folderGuid; }
            set { folderGuid = value; }
        }

        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
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
		public int UploadUserId 
		{
			get { return uploadUserID; }
			set { uploadUserID = value; }
		}

        public int DownloadCount
        {
            get { return downloadCount; }
        }

		public string FriendlyName 
		{
			get { return friendlyName; }
			set { friendlyName = value; }
		}
		public string OriginalFileName 
		{
			get { return originalFileName; }
			set { originalFileName = value; }
		}
		public string ServerFileName 
		{
			get { return serverFileName; }
			set { serverFileName = value; }
		}

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
		
		public int SizeInKB 
		{
			get { return sizeInKB; }
			set { sizeInKB = value; }
		}
		public DateTime UploadDate 
		{
			get { return uploadDate; }
			set { uploadDate = value; }
		}
		public int FolderId 
		{
			get { return folderID; }
			set { folderID = value; }
		}

		#endregion

		#region Private Methods

		private void GetSharedFile(int itemId) 
		{
            using (IDataReader reader = DBSharedFiles.GetSharedFile(itemId))
            {
                if (reader.Read())
                {
                    this.itemID = Convert.ToInt32(reader["ItemID"]);
                    this.moduleID = Convert.ToInt32(reader["ModuleID"]);
                    this.uploadUserID = Convert.ToInt32(reader["UploadUserID"]);
                    this.friendlyName = reader["FriendlyName"].ToString();
                    this.originalFileName = reader["OriginalFileName"].ToString();
                    this.serverFileName = reader["ServerFileName"].ToString();
                    this.sizeInKB = Convert.ToInt32(reader["SizeInKB"]);
                    this.uploadDate = Convert.ToDateTime(reader["UploadDate"]);
                    this.folderID = Convert.ToInt32(reader["FolderID"]);
                    this.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                    string u = reader["UserGuid"].ToString();
                    if (u.Length == 36) this.userGuid = new Guid(u);
                    u = reader["FolderGuid"].ToString();
                    if (u.Length == 36) this.folderGuid = new Guid(u);
                    this.description = reader["Description"].ToString();
                    this.downloadCount = Convert.ToInt32(reader["DownloadCount"]);

                }

            }
		
		
		}


		private bool Create()
		{ 
			int newID = 0;
			if(this.serverFileName.Length == 0)
			{
				this.serverFileName = System.Guid.NewGuid().ToString() + ".config";
			}

            this.itemGuid = Guid.NewGuid();

			newID = DBSharedFiles.AddSharedFile(
                this.itemGuid,
                this.moduleGuid,
                this.userGuid,
                this.folderGuid,
				this.moduleID, 
				this.uploadUserID, 
				this.friendlyName, 
				this.originalFileName, 
				this.serverFileName, 
				this.sizeInKB, 
				this.uploadDate, 
				this.folderID,
                this.description); 
			
			this.itemID = newID;

            bool result = (newID > 0);

            //IndexHelper.IndexItem(this);
            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                OnContentChanged(e);
            }

            return result;

		}

		private bool Update()
		{

			bool result = DBSharedFiles.UpdateSharedFile(
				this.itemID, 
				this.moduleID, 
				this.uploadUserID, 
				this.friendlyName, 
				this.originalFileName, 
				this.serverFileName, 
				this.sizeInKB, 
				this.uploadDate, 
				this.folderID,
                this.folderGuid,
                this.userGuid,
                this.description);

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

		

		public bool Save()
		{
			if(this.itemID > 0)
			{
				return Update();
			}
			else
			{
				return Create();
			}
			
		}

        //public bool CreateHistory(string sourceFilePath, string historyFolderPath)
        //{
        //    bool historyCreated = false;

        //    File.Move(Path.Combine(sourceFilePath, Path.GetFileName(this.serverFileName)),Path.Combine(historyFolderPath, Path.GetFileName(this.serverFileName)));
				
        //    historyCreated = DBSharedFiles.AddHistory(
        //        this.itemGuid,
        //        this.moduleGuid,
        //        this.userGuid,
        //        this.itemID, 
        //        this.moduleID, 
        //        this.friendlyName,
        //        this.originalFileName,
        //        this.serverFileName, 
        //        this.sizeInKB, 
        //        this.uploadDate, 
        //        this.uploadUserID,
        //        DateTime.UtcNow);

        //    if(historyCreated)
        //    {
        //        this.serverFileName = System.Guid.NewGuid().ToString() + ".config";
        //    }
        //    return historyCreated;
        //}

		public IDataReader GetHistory()  
		{
			return DBSharedFiles.GetHistory(this.moduleID, this.itemID);
		}

        public bool Delete()
        {
            bool result = false;

            if (itemID == -1) { return result; }
            SharedFile sharedFile = new SharedFile(moduleID, itemID);
          
            DBSharedFiles.DeleteHistoryByItemID(itemID);
            // this just deletes the entry from the db
            result =  DBSharedFiles.DeleteSharedFile(itemID);

            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                e.IsDeleted = true;
                OnContentChanged(e);
            }


            return result;


        }
		
		#endregion

		#region Static Methods

        public static bool IncrementDownloadCount(int itemId)
        {
            return DBSharedFiles.IncrementDownloadCount(itemId);
        }

        public static bool AddHistory(
           Guid itemGuid,
           Guid moduleGuid,
           Guid userGuid,
           int itemId,
           int moduleId,
           string friendlyName,
           string originalFileName,
           string serverFileName,
           int sizeInKB,
           DateTime uploadDate,
           int uploadUserId)
        {
            return DBSharedFiles.AddHistory(
                itemGuid,
                moduleGuid,
                userGuid,
                itemId,
                moduleId,
                friendlyName,
                originalFileName,
                serverFileName,
                sizeInKB,
                uploadDate,
                uploadUserId,
                DateTime.UtcNow);
        }

        public static SharedFileHistory GetHistoryFile(int historyFileId)
        {
            SharedFileHistory historyFile = null;
            using (IDataReader reader = DBSharedFiles.GetHistoryFile(historyFileId))
            {
                if (reader.Read())
                {
                    historyFile = new SharedFileHistory();
                    historyFile.FriendlyName = reader["FriendlyName"].ToString();
                    historyFile.ServerFileName = reader["ServerFileName"].ToString();


                }
            }

            return historyFile;


        }

        public static IDataReader GetHistoryFileAsIDataReader(int id)
        {
            return DBSharedFiles.GetHistoryFile(id);
        }

        //public static bool DeleteSharedFile(int moduleID, int itemID, string sourcePath) 
        //{
        //    if(sourcePath != null)
        //    {
        //        SharedFile sharedFile = new SharedFile(moduleID, itemID);
        //        String filePath = Path.Combine(sourcePath, Path.GetFileName(sharedFile.ServerFileName));
        //        if(File.Exists(filePath))
        //        {
        //            File.Delete(filePath);
        //        }
        //    }

        //    IndexHelper.RemoveIndexItem(moduleID, itemID);

        //    // this just deletes the entry from the db
        //    return dbSharedFiles.DeleteSharedFile(itemID);
        //}

        public static bool DeleteByModule(int moduleId)
        {
            return DBSharedFiles.DeleteByModule(moduleId);
        }

        public static bool DeleteBySite(int siteId)
        {
            return DBSharedFiles.DeleteBySite(siteId);
        }

		public static bool DeleteHistory(int id) 
		{

            //string historyServerName = string.Empty;
            
            //using (IDataReader reader = DBSharedFiles.GetHistoryFile(id))
            //{
            //    if (reader.Read())
            //    {
                   
            //        historyServerName = reader["ServerFileName"].ToString();
                    

            //    }
            //}

            //if (historyServerName.Length > 0)
            //{
            //    File.Delete(Path.Combine(historyPath, Path.GetFileName(historyServerName)));
            //}


			// this just deletes the entry from the db
			return DBSharedFiles.DeleteHistory(id);
		}

		public static IDataReader GetSharedFiles(int moduleId, int folderId)  
		{
			return DBSharedFiles.GetSharedFiles(moduleId, folderId);
		}

        //public static bool RestoreHistoryFile(int historyId, string sourcePath, string historyPath)
        //{
        //    bool historyRestored = false;

        //    if((sourcePath != null)&&(historyPath != null))
        //    {
			
        //        int itemId = 0;
        //        int moduleId = 0;
        //        string historyFriendlyName = string.Empty;
        //        string historyOriginalName = string.Empty;
        //        string historyServerName = string.Empty;
        //        DateTime historyUploadDate = DateTime.Now;
        //        int historyUploadUserID = 0;
        //        int historyFileSize = 0;

        //        using (IDataReader reader = DBSharedFiles.GetHistoryFile(historyId))
        //        {
        //            if (reader.Read())
        //            {
        //                itemId = Convert.ToInt32(reader["ItemID"]);
        //                moduleId = Convert.ToInt32(reader["ModuleID"]);
        //                historyFriendlyName = reader["FriendlyName"].ToString();
        //                historyOriginalName = reader["OriginalFileName"].ToString();
        //                historyServerName = reader["ServerFileName"].ToString();
        //                historyFileSize = Convert.ToInt32(reader["SizeInKB"]);
        //                historyUploadUserID = Convert.ToInt32(reader["UploadUserID"]);
        //                historyUploadDate = DateTime.Parse(reader["UploadDate"].ToString());

        //            }
        //        }

        //        SharedFile sharedFile = new SharedFile(moduleId, itemId);
        //        sharedFile.CreateHistory(sourcePath, historyPath);

        //        File.Move(Path.Combine(historyPath, Path.GetFileName(historyServerName)),Path.Combine(sourcePath, Path.GetFileName(historyServerName)));
				
        //        sharedFile.ServerFileName = historyServerName;
        //        sharedFile.OriginalFileName = historyOriginalName;
        //        sharedFile.FriendlyName = historyFriendlyName;
        //        sharedFile.SizeInKB = historyFileSize;
        //        sharedFile.UploadDate = historyUploadDate;
        //        sharedFile.UploadUserId = historyUploadUserID;
        //        historyRestored = sharedFile.Update();
        //        SharedFile.DeleteHistory(historyId, historyPath);
        //    }
			
        //    return historyRestored;

        //}


		public static DataTable GetSharedFilesByPage(int siteId, int pageId)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ItemID", typeof(int));
            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("FriendlyName", typeof(string));
            dataTable.Columns.Add("ViewRoles", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("UploadDate", typeof(DateTime));

            using (IDataReader reader = DBSharedFiles.GetSharedFilesByPage(siteId, pageId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();

                    row["ItemID"] = reader["ItemID"];
                    row["ModuleID"] = reader["ModuleID"];
                    row["ModuleTitle"] = reader["ModuleTitle"];
                    row["FriendlyName"] = reader["FriendlyName"];
                    row["ViewRoles"] = reader["ViewRoles"];
                    row["Description"] = reader["Description"];
                    row["UploadDate"] = Convert.ToDateTime(reader["UploadDate"]);

                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;

        }

        public static IDataReader GetSharedFiles(int moduleId)
        {
            return DBSharedFiles.GetSharedFiles(moduleId);
        }

        public static IDataReader GetHistoryByModule(int moduleId)
        {
            return DBSharedFiles.GetHistoryByModule(moduleId);
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


	}
	
}