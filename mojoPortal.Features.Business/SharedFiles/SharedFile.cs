// Created:       2005-01-05
// Last Modified: 2018-06-03
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
// 
// You must not remove this notice, or any other, from this software.

using log4net;
using mojoPortal.Data;
using System;
using System.Data;

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
		{ }


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
		private string viewRoles = "All Users";

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
		public string ViewRoles
		{
			get { return viewRoles; }
			set { viewRoles = value; }
		}

		#endregion


		#region Private Methods

		private void GetSharedFile(int itemId)
		{
			using (IDataReader reader = DBSharedFiles.GetSharedFile(itemId))
			{
				if (reader.Read())
				{
					itemID = Convert.ToInt32(reader["ItemID"]);
					moduleID = Convert.ToInt32(reader["ModuleID"]);
					uploadUserID = Convert.ToInt32(reader["UploadUserID"]);
					friendlyName = reader["FriendlyName"].ToString();
					originalFileName = reader["OriginalFileName"].ToString();
					serverFileName = reader["ServerFileName"].ToString();
					sizeInKB = Convert.ToInt32(reader["SizeInKB"]);
					uploadDate = Convert.ToDateTime(reader["UploadDate"]);
					folderID = Convert.ToInt32(reader["FolderID"]);
					moduleGuid = new Guid(reader["ModuleGuid"].ToString());

					string u = reader["UserGuid"].ToString();

					if (u.Length == 36)
					{
						userGuid = new Guid(u);
					}

					u = reader["FolderGuid"].ToString();

					if (u.Length == 36)
					{
						folderGuid = new Guid(u);
					}

					description = reader["Description"].ToString();
					downloadCount = Convert.ToInt32(reader["DownloadCount"]);
					viewRoles = reader["ViewRoles"].ToString();
				}
			}
		}


		private bool Create()
		{
			int newID = 0;

			if (serverFileName.Length == 0)
			{
				serverFileName = Guid.NewGuid().ToString() + ".config";
			}

			itemGuid = Guid.NewGuid();

			newID = DBSharedFiles.AddSharedFile(
				itemGuid,
				moduleGuid,
				userGuid,
				folderGuid,
				moduleID,
				uploadUserID,
				friendlyName,
				originalFileName,
				serverFileName,
				sizeInKB,
				uploadDate,
				folderID,
				description,
				viewRoles
			);

			itemID = newID;

			bool result = (newID > 0);

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
				itemID,
				moduleID,
				uploadUserID,
				friendlyName,
				originalFileName,
				serverFileName,
				sizeInKB,
				uploadDate,
				folderID,
				folderGuid,
				userGuid,
				description,
				viewRoles
			);

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
			if (itemID > 0)
			{
				return Update();
			}
			else
			{
				return Create();
			}
		}


		public IDataReader GetHistory()
		{
			return DBSharedFiles.GetHistory(moduleID, itemID);
		}


		public bool Delete()
		{
			bool result = false;

			if (itemID == -1)
			{
				return result;
			}

			SharedFile sharedFile = new SharedFile(moduleID, itemID);

			DBSharedFiles.DeleteHistoryByItemID(itemID);

			// this just deletes the entry from the db
			result = DBSharedFiles.DeleteSharedFile(itemID);

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
		   int uploadUserId,
		   string viewRoles)
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
				DateTime.UtcNow,
				viewRoles
			);
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
			// this just deletes the entry from the db
			return DBSharedFiles.DeleteHistory(id);
		}


		public static IDataReader GetSharedFiles(int moduleId, int folderId)
		{
			return DBSharedFiles.GetSharedFiles(moduleId, folderId);
		}


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
			ContentChanged?.Invoke(this, e);
		}

		#endregion
	}
}
