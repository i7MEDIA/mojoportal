/// Author:					Joe Audette
/// Created:				2005-01-05
/// Last Modified:			2014-03-11
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using log4net;
using mojoPortal.Data;

namespace mojoPortal.Business
{
	/// <summary>
	/// Represents a folder in the shared files feature
	/// </summary>
	public class SharedFileFolder
	{

		#region Constructors

		public SharedFileFolder()
		{}
	    
		public SharedFileFolder(int moduleId,int folderId) 
		{

			this.moduleID = moduleId;
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

		#endregion

		#region Private Methods

		private void GetSharedFileFolder(int folderId) 
		{
            using (IDataReader reader = DBSharedFiles.GetSharedFileFolder(folderId))
            {
                if (reader.Read())
                {
                    this.folderID = Convert.ToInt32(reader["FolderID"]);
                    this.moduleID = Convert.ToInt32(reader["ModuleID"]);
                    this.folderName = reader["FolderName"].ToString();
                    this.parentID = Convert.ToInt32(reader["ParentID"]);

                    this.folderGuid = new Guid(reader["FolderGuid"].ToString());
                    this.moduleGuid = new Guid(reader["ModuleGuid"].ToString());

                    string p = reader["ParentGuid"].ToString();
                    if (p.Length == 36) this.parentGuid = new Guid(p);

                }

            }
		
		}


		private bool Create()
		{ 
			int newID;
            this.folderGuid = Guid.NewGuid();
			
			newID = DBSharedFiles.AddSharedFileFolder(
                this.folderGuid,
                this.moduleGuid,
                this.parentGuid,
				this.moduleID, 
				this.folderName, 
				this.parentID); 

			this.folderID = newID;
					
			return (newID > 0);

		}

		private bool Update()
		{

			return DBSharedFiles.UpdateSharedFileFolder(
				this.folderID, 
				this.moduleID, 
				this.folderName, 
				this.parentID,
                this.parentGuid); 
				
		}

		




		#endregion

		#region Public Methods

		public bool Save()
		{
			if(this.folderID > -1)
			{
				return Update();
			}
			else
			{
				return Create();
			}
		}
		

        //public void DeleteAllFiles(string fileBasePath)
        //{
        //    // method implemented by Jean-Michel 2008-07-31

        //    // TODO: implement check whether versioning is enabled before calling this method
        //    // if we are keeping versions we should not delete the files

        //    if (FolderId == -1) { return; }

        //    ArrayList folders = new ArrayList();
        //    ArrayList files = new ArrayList();
        //    using (IDataReader reader = SharedFile.GetSharedFiles(ModuleId, FolderId))
        //    {
        //        while (reader.Read())
        //        {
        //            files.Add(Convert.ToInt32(reader["ItemID"]));
        //        }
        //    }

        //    using (IDataReader reader = SharedFileFolder.GetSharedFolders(ModuleId, FolderId))
        //    {
        //        while (reader.Read())
        //        {
        //            folders.Add(Convert.ToInt32(reader["FolderID"]));
        //        }
        //    }

        //    foreach (int id in files)
        //    {
        //        SharedFile sharedFile = new SharedFile(ModuleId, id);
        //        sharedFile.Delete(fileBasePath);
        //    }

        //    foreach (int id in folders)
        //    {
        //        SharedFileFolder folder = new SharedFileFolder(ModuleId, id);
        //        folder.DeleteAllFiles(fileBasePath);
        //        SharedFileFolder.DeleteSharedFileFolder(id);
        //    }
           


        //}

		

		
		
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
                    dt.Rows.Add(dr);
                }
            }


			dt.AcceptChanges();

			return dt;



		}



		#endregion


        


	}
	
}