// Author:					    
// Created:				        2011-08-20
// Last Modified:			    2013-06-10\4
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
using System.Linq;
using System.Web;
using mojoPortal.Business;
using mojoPortal.FileSystem;


namespace mojoPortal.Web.SharedFilesUI
{
    public static class SharedFilesHelper
    {

        public static void DeleteAllFiles(SharedFileFolder folder, IFileSystem fileSystem, string fileVirtualBasePath, SharedFilesConfiguration config)
        {
            // method implemented by Jean-Michel 2008-07-31

            // TODO: implement check whether versioning is enabled before calling this method
            // if we are keeping versions we should not delete the files

            if (folder == null) { return; }
            if (fileSystem == null) { return; }
            if (string.IsNullOrEmpty(fileVirtualBasePath)) { return; }
            if (folder.FolderId == -1) { return; }

            ArrayList folders = new ArrayList();
            ArrayList files = new ArrayList();
            using (IDataReader reader = SharedFile.GetSharedFiles(folder.ModuleId, folder.FolderId))
            {
                while (reader.Read())
                {
                    files.Add(Convert.ToInt32(reader["ItemID"]));
                }
            }

            using (IDataReader reader = SharedFileFolder.GetSharedFolders(folder.ModuleId, folder.FolderId))
            {
                while (reader.Read())
                {
                    folders.Add(Convert.ToInt32(reader["FolderID"]));
                }
            }

            foreach (int id in files)
            {
                SharedFile sharedFile = new SharedFile(folder.ModuleId, id);
                sharedFile.Delete();

                if (!config.EnableVersioning)
                {
                    fileSystem.DeleteFile(VirtualPathUtility.Combine(fileVirtualBasePath, sharedFile.ServerFileName));

                }
            }

            foreach (int id in folders)
            {
                SharedFileFolder subFolder = new SharedFileFolder(folder.ModuleId, id);

                DeleteAllFiles(subFolder, fileSystem, fileVirtualBasePath, config);

                SharedFileFolder.DeleteSharedFileFolder(id);
            }



        }



        public static bool CreateHistory(SharedFile file, IFileSystem fileSystem, string virtualFilePath, string virtualHistoryPath)
        {
            bool historyCreated = false;

            //File.Move(Path.Combine(sourceFilePath, Path.GetFileName(this.serverFileName)), Path.Combine(historyFolderPath, Path.GetFileName(this.serverFileName)));
            fileSystem.MoveFile(
                VirtualPathUtility.Combine(virtualFilePath, file.ServerFileName), 
                VirtualPathUtility.Combine(virtualHistoryPath, file.ServerFileName), 
                true);

            historyCreated = SharedFile.AddHistory(
                file.ItemGuid,
                file.ModuleGuid,
                file.UserGuid,
                file.ItemId,
                file.ModuleId,
                file.FriendlyName,
                file.OriginalFileName,
                file.ServerFileName,
                file.SizeInKB,
                file.UploadDate,
                file.UploadUserId,
				file.ViewRoles
			);

            
            return historyCreated;
        }


        public static bool RestoreHistoryFile(
            int historyId, 
            IFileSystem fileSystem,
            string virtualSourcePath, 
            string virtualHistoryPath)
        {

            bool historyRestored = false;

            if (string.IsNullOrEmpty(virtualSourcePath)) { return historyRestored; }
            if (string.IsNullOrEmpty(virtualHistoryPath)) { return historyRestored; }
            if (fileSystem == null) { return historyRestored; }
            

            int itemId = 0;
            int moduleId = 0;
            string historyFriendlyName = string.Empty;
            string historyOriginalName = string.Empty;
            string historyServerName = string.Empty;
            DateTime historyUploadDate = DateTime.Now;
            int historyUploadUserID = 0;
            int historyFileSize = 0;

            using (IDataReader reader = SharedFile.GetHistoryFileAsIDataReader(historyId))
            {
                if (reader.Read())
                {
                    itemId = Convert.ToInt32(reader["ItemID"]);
                    moduleId = Convert.ToInt32(reader["ModuleID"]);
                    historyFriendlyName = reader["FriendlyName"].ToString();
                    historyOriginalName = reader["OriginalFileName"].ToString();
                    historyServerName = reader["ServerFileName"].ToString();
                    historyFileSize = Convert.ToInt32(reader["SizeInKB"]);
                    historyUploadUserID = Convert.ToInt32(reader["UploadUserID"]);
                    historyUploadDate = DateTime.Parse(reader["UploadDate"].ToString());

                }
            }

            SharedFile sharedFile = new SharedFile(moduleId, itemId);
            CreateHistory(sharedFile, fileSystem, virtualSourcePath, virtualHistoryPath);

            //File.Move(Path.Combine(historyPath, Path.GetFileName(historyServerName)), Path.Combine(sourcePath, Path.GetFileName(historyServerName)));
            fileSystem.MoveFile(
                VirtualPathUtility.Combine(virtualHistoryPath, historyServerName),
                VirtualPathUtility.Combine(virtualSourcePath, historyServerName),
                true);

            sharedFile.ServerFileName = historyServerName;
            sharedFile.OriginalFileName = historyOriginalName;
            sharedFile.FriendlyName = historyFriendlyName;
            sharedFile.SizeInKB = historyFileSize;
            sharedFile.UploadDate = historyUploadDate;
            sharedFile.UploadUserId = historyUploadUserID;
            historyRestored = sharedFile.Save();
            SharedFile.DeleteHistory(historyId);

            fileSystem.DeleteFile(VirtualPathUtility.Combine(virtualHistoryPath, historyServerName));
           

            return historyRestored;

        }

        public static void DeleteHistoryFile(int id, IFileSystem fileSystem, string virtualHistoryPath)
        {

            string historyServerName = string.Empty;

            using (IDataReader reader = SharedFile.GetHistoryFileAsIDataReader(id))
            {
                if (reader.Read())
                {

                    historyServerName = reader["ServerFileName"].ToString();


                }
            }

            if (historyServerName.Length > 0)
            {
                //File.Delete(Path.Combine(historyPath, Path.GetFileName(historyServerName)));
                string fullPath = VirtualPathUtility.Combine(virtualHistoryPath, historyServerName);
                fileSystem.DeleteFile(fullPath);

            }


            
            
        }

        public static SharedFileFolder GetFolderFromListById(int folderId, List<SharedFileFolder> allFolders)
        {
            foreach (SharedFileFolder folder in allFolders)
            {
                if (folder.FolderId == folderId) { return folder; }
            }

            return null;

        }

        public static List<SharedFileFolder> GetAllParentsFolder(SharedFileFolder folder, List<SharedFileFolder> allFolders)
        {
            List<SharedFileFolder> list = new List<SharedFileFolder>();
            while (folder.ParentId > -1)
            {
                folder = GetFolderFromListById(folder.ParentId, allFolders);
                list.Insert(0, folder);// note: Insert at 0 to preserve a path stucture
            }
            return list;
        }



        public static List<int> GetAllParentsFolderIds(SharedFileFolder folder, List<SharedFileFolder> allFolders)
        {
            return GetAllParentsFolder(folder, allFolders).Select(fld => fld.FolderId).ToList();
        }

        //public static List<int> GetAllParentsFolderIds(SharedFileFolder folder, List<SharedFileFolder> allFolders)
        //{
        //    List<int> list = new List<int>();
        //    do
        //    {
        //        if (folder.ParentId > -1)
        //        {
        //            list.Insert(0, folder.ParentId);// note: Insert at 0 to preserve a path stucture
        //            //folder = new SharedFileFolder(folder.ModuleId, folder.ParentId);
        //            folder = GetFolderFromListById(folder.ParentId, allFolders);
        //        }
        //        else
        //        {
        //            folder = null;
        //        }
        //    } while (folder != null);
        //    return list;
        //}

    }
}