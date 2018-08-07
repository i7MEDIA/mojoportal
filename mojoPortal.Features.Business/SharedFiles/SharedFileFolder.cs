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
using System.Collections.Generic;
using System.Data;
using mojoPortal.Features.Business.SharedFiles.Models;

namespace mojoPortal.Business
{
	/// <summary>
	/// Represents a folder in the shared files feature
	/// </summary>
	public class SharedFileFolder
	{
		#region Constructors

		public SharedFileFolder()
		{ }

		public SharedFileFolder(int moduleId, int folderId)
		{
			moduleID = moduleId;
			GetSharedFileFolder(folderId);
		}

		#endregion


		#region Private Properties

		private static readonly ILog log = LogManager.GetLogger(typeof(SharedFileFolder));
		private Guid folderGuid = Guid.Empty;
		private Guid moduleGuid = Guid.Empty;
		private Guid parentGuid = Guid.Empty;
		private int folderID = -1;
		private int moduleID = -1;
		private string folderName = string.Empty;
		private int parentID = -1;
		private string viewRoles = string.Empty;

		#endregion


		#region Public Properties

		public Guid FolderGuid
		{
			get { return folderGuid; }
		}
		public Guid ModuleGuid
		{
			get { return moduleGuid; }
			set { moduleGuid = value; }
		}
		public Guid ParentGuid
		{
			get { return parentGuid; }
			set { parentGuid = value; }
		}
		public int FolderId
		{
			get { return folderID; }
			set { folderID = value; }
		}
		public int ModuleId
		{
			get { return moduleID; }
			set { moduleID = value; }
		}
		public string FolderName
		{
			get { return folderName; }
			set { folderName = value; }
		}
		public int ParentId
		{
			get { return parentID; }
			set { parentID = value; }
		}
		public string ViewRoles
		{
			get { return viewRoles; }
			set { viewRoles = value; }
		}

		#endregion


		#region Private Methods

		private void GetSharedFileFolder(int folderId)
		{
			using (IDataReader reader = DBSharedFiles.GetSharedFileFolder(folderId))
			{
				if (reader.Read())
				{
					folderID = Convert.ToInt32(reader["FolderID"]);
					moduleID = Convert.ToInt32(reader["ModuleID"]);
					folderName = reader["FolderName"].ToString();
					parentID = Convert.ToInt32(reader["ParentID"]);
					folderGuid = new Guid(reader["FolderGuid"].ToString());
					moduleGuid = new Guid(reader["ModuleGuid"].ToString());
					viewRoles = reader["ViewRoles"].ToString();

					string p = reader["ParentGuid"].ToString();

					if (p.Length == 36)
					{
						parentGuid = new Guid(p);
					}
				}
			}
		}


		private bool Create()
		{
			folderGuid = Guid.NewGuid();

			int newID = DBSharedFiles.AddSharedFileFolder(
				folderGuid,
				moduleGuid,
				parentGuid,
				moduleID,
				folderName,
				parentID,
				viewRoles
			);

			folderID = newID;

			return (newID > 0);
		}


		private bool Update()
		{
			return DBSharedFiles.UpdateSharedFileFolder(
				folderID,
				moduleID,
				folderName,
				parentID,
				parentGuid,
				viewRoles
			);
		}

		#endregion


		#region Public Methods

		public bool Save()
		{
			if (folderID > -1)
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

		public static bool DeleteSharedFileFolder(int folderId)
		{
			return DBSharedFiles.DeleteSharedFileFolder(folderId);
		}


		public static IDataReader GetSharedModuleFolders(int moduleId)
		{
			return DBSharedFiles.GetSharedModuleFolders(moduleId);
		}


		public static List<SharedFileFolder> GetSharedModuleFolderList(int moduleId)
		{
			return LoadListFromReader(DBSharedFiles.GetSharedModuleFolders(moduleId));
		}


		public static IDataReader GetSharedFolders(int moduleId, int parentId)
		{
			return DBSharedFiles.GetSharedFolders(moduleId, parentId);
		}


		private static List<SharedFileFolder> LoadListFromReader(IDataReader reader)
		{
			List<SharedFileFolder> sharedFileFolderList = new List<SharedFileFolder>();
			try
			{
				while (reader.Read())
				{
					SharedFileFolder sharedFileFolder = new SharedFileFolder();
					sharedFileFolder.folderID = Convert.ToInt32(reader["FolderID"]);
					sharedFileFolder.moduleID = Convert.ToInt32(reader["ModuleID"]);
					sharedFileFolder.folderName = reader["FolderName"].ToString();
					sharedFileFolder.parentID = Convert.ToInt32(reader["ParentID"]);
					sharedFileFolder.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
					sharedFileFolder.folderGuid = new Guid(reader["FolderGuid"].ToString());
					sharedFileFolder.viewRoles = reader["ViewRoles"].ToString();

					if (reader["ParentGuid"] != DBNull.Value)
					{
						sharedFileFolder.parentGuid = new Guid(reader["ParentGuid"].ToString());
					}

					sharedFileFolderList.Add(sharedFileFolder);
				}
			}
			finally
			{
				reader.Close();
			}

			return sharedFileFolderList;
		}


		public static DataTable GetFoldersAndFiles(int moduleId, int parentId)
		{
			DataTable dt = new DataTable();

			dt.Columns.Add("ID", typeof(string));
			dt.Columns.Add("filename", typeof(string));
			dt.Columns.Add("OriginalFileName", typeof(string));
			dt.Columns.Add("Description", typeof(string));
			dt.Columns.Add("size", typeof(int));
			dt.Columns.Add("type", typeof(string));
			dt.Columns.Add("DownloadCount", typeof(string));
			dt.Columns.Add("modified", typeof(DateTime));
			dt.Columns.Add("username", typeof(string));
			dt.Columns.Add("ViewRoles", typeof(string));

			DataRow dr;

			using (IDataReader reader = GetSharedFolders(moduleId, parentId))
			{
				while (reader.Read())
				{
					dr = dt.NewRow();

					dr["ID"] = reader["FolderID"].ToString() + "~folder";
					dr["filename"] = reader["FolderName"];
					dr["OriginalFileName"] = string.Empty;
					dr["Description"] = string.Empty;
					dr["size"] = reader["SizeInKB"].ToString();
					dr["type"] = "0";
					dr["DownloadCount"] = string.Empty;
					dr["ViewRoles"] = reader["ViewRoles"].ToString();

					dt.Rows.Add(dr);
				}
			}

			using (IDataReader reader = SharedFile.GetSharedFiles(moduleId, parentId))
			{
				while (reader.Read())
				{
					dr = dt.NewRow();

					dr["ID"] = reader["ItemID"].ToString() + "~file";
					dr["filename"] = reader["FriendlyName"];
					dr["OriginalFileName"] = reader["OriginalFileName"];
					dr["Description"] = reader["Description"];
					dr["size"] = reader["SizeInKB"].ToString();
					dr["type"] = "1";
					dr["modified"] = Convert.ToDateTime(reader["UploadDate"]);
					dr["username"] = reader["UserName"].ToString();
					dr["DownloadCount"] = reader["DownloadCount"].ToString();
					dr["ViewRoles"] = reader["ViewRoles"].ToString();

					dt.Rows.Add(dr);
				}
			}

			dt.AcceptChanges();

			return dt;
		}

		public static FoldersAndFiles GetFoldersAndFilesModel(int moduleId, int parentId)
		{
			FoldersAndFiles foldersAndFiles = new FoldersAndFiles();

			using (IDataReader reader = GetSharedFolders(moduleId, parentId))
			{
				while (reader.Read())
				{
					Folder newFolder = new Folder
					{
						ID = Convert.ToInt32(reader["FolderID"]),
						ModuleID = Convert.ToInt32(reader["ModuleID"]),
						Name = reader["FolderName"].ToString(),
						ParentID = Convert.ToInt32(reader["ParentID"]),
						ModuleGuid = new Guid(reader["ModuleGuid"].ToString()),
						FolderGuid = new Guid(reader["FolderGuid"].ToString()),
						ParentGuid = new Guid(reader["ParentGuid"].ToString()),
						ViewRoles = reader["ViewRoles"].ToString()
					};

					foldersAndFiles.Folders.Add(newFolder);
				}
			}

			using (IDataReader reader = SharedFile.GetSharedFiles(moduleId, parentId))
			{
				while (reader.Read())
				{
					File newFile = new File
					{
						ID = Convert.ToInt32(reader["ItemID"]),
						ModuleID = Convert.ToInt32(reader["ModuleID"]),
						UploadUserID = Convert.ToInt32(reader["UploadUserID"]),
						Name = reader["FriendlyName"].ToString(),
						OriginalFileName = reader["OriginalFileName"].ToString(),
						ServerFileName = reader["ServerFileName"].ToString(),
						SizeInKB = Convert.ToInt32(reader["SizeInKB"]),
						UploadDate = Convert.ToDateTime(reader["UploadDate"]),
						FolderID = Convert.ToInt32(reader["FolderID"]),
						ItemGuid = new Guid(reader["ItemGuid"].ToString()),
						ModuleGuid = new Guid(reader["ModuleGuid"].ToString()),
						UserGuid = new Guid(reader["UserGuid"].ToString()),
						FolderGuid = new Guid(reader["FolderGuid"].ToString()),
						Description = reader["Description"].ToString(),
						DownloadCount = Convert.ToInt32(reader["DownloadCount"]),
						ViewRoles = reader["ViewRoles"].ToString()
					};

					foldersAndFiles.Files.Add(newFile);
				}
			}

			return foldersAndFiles;
		}

		#endregion
	}
}
