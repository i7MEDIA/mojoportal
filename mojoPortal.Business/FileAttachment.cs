// Author:					
// Created:					2009-03-08
// Last Modified:			2012-09-19
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

    public class FileAttachment
    {

        #region Constructors

        public FileAttachment()
        { }


        public FileAttachment(Guid rowGuid)
        {
            GetFileAttachment(rowGuid);
        }

        #endregion

        #region Private Properties

        private Guid rowGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;
        private Guid itemGuid = Guid.Empty;
        private Guid specialGuid1 = Guid.Empty;
        private Guid specialGuid2 = Guid.Empty;
        private string serverFileName = string.Empty;
        private string fileName = string.Empty;
        private string contentTitle = string.Empty;
        private long contentLength = 0;
        private string contentType = string.Empty;
        private DateTime createdUtc = DateTime.UtcNow;
        private Guid createdBy = Guid.Empty;

        
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
        public Guid ModuleGuid
        {
            get { return moduleGuid; }
            set { moduleGuid = value; }
        }
        public Guid ItemGuid
        {
            get { return itemGuid; }
            set { itemGuid = value; }
        }
        public Guid SpecialGuid1
        {
            get { return specialGuid1; }
            set { specialGuid1 = value; }
        }
        public Guid SpecialGuid2
        {
            get { return specialGuid2; }
            set { specialGuid2 = value; }
        }
        public string ServerFileName
        {
            get { return serverFileName; }
            set { serverFileName = value; }
        }
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        public DateTime CreatedUtc
        {
            get { return createdUtc; }
            set { createdUtc = value; }
        }
        public Guid CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        public long ContentLength
        {
            get { return contentLength; }
            set { contentLength = value; }
        }

        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        public string ContentTitle
        {
            get { return contentTitle; }
            set { contentTitle = value; }
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of FileAttachment.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        private void GetFileAttachment(Guid rowGuid)
        {
            using (IDataReader reader = DBFileAttachment.GetOne(rowGuid))
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
                this.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                this.itemGuid = new Guid(reader["ItemGuid"].ToString());
                this.specialGuid1 = new Guid(reader["SpecialGuid1"].ToString());
                this.specialGuid2 = new Guid(reader["SpecialGuid2"].ToString());
                this.serverFileName = reader["ServerFileName"].ToString();
                this.fileName = reader["FileName"].ToString();
                this.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                this.createdBy = new Guid(reader["CreatedBy"].ToString());

                if (reader["ContentLength"] != DBNull.Value)
                {
                    this.contentLength = Convert.ToInt64(reader["ContentLength"]);
                }

                this.contentType = reader["ContentType"].ToString();
                this.contentTitle = reader["ContentTitle"].ToString();

            }

        }

        /// <summary>
        /// Persists a new instance of FileAttachment. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.rowGuid = Guid.NewGuid();
            if (this.serverFileName.Length == 0)
            {
                this.serverFileName = rowGuid.ToString() + ".config";
            }

            int rowsAffected = DBFileAttachment.Create(
                this.rowGuid,
                this.siteGuid,
                this.moduleGuid,
                this.itemGuid,
                this.specialGuid1,
                this.specialGuid2,
                this.serverFileName,
                this.fileName,
                this.contentTitle,
                this.contentLength,
                this.contentType,
                this.createdUtc,
                this.createdBy);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of FileAttachment. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBFileAttachment.Update(
                this.rowGuid,
                this.serverFileName,
                this.fileName,
                this.contentTitle,
                this.contentLength,
                this.contentType);

        }





        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of FileAttachment. Returns true on success.
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
        /// Deletes an instance of FileAttachment. Returns true on success.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            return DBFileAttachment.Delete(rowGuid);
        }

        /// <summary>
        /// Deletes rows from the mp_FileAttachment table. Returns true if row deleted.
        /// </summary>
        public static bool DeleteBySite(Guid siteGuid)
        {
            return DBFileAttachment.DeleteBySite(siteGuid);
        }


        /// <summary>
        /// Deletes rows from the mp_FileAttachment table. Returns true if row deleted.
        /// </summary>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            return DBFileAttachment.DeleteByModule(moduleGuid);
        }

        /// <summary>
        /// Deletes rows from the mp_FileAttachment table. Returns true if row deleted.
        /// </summary>
        public static bool DeleteByItem(Guid itemGuid)
        {
            return DBFileAttachment.DeleteByItem(itemGuid);
        }

        //public static void DeleteAttachmentFiles(List<FileAttachment> attachments, string basePath)
        //{
        //    if (attachments == null) { return; }
        //    if(string.IsNullOrEmpty(basePath)) { return;}

        //    foreach (FileAttachment f in attachments)
        //    {
        //        if(File.Exists(Path.Combine(basePath, f.ServerFileName)))
        //        {
        //            File.Delete(Path.Combine(basePath, f.ServerFileName));
        //        }
        //    }

        //}

        //public static void DeleteAttachmentFile(FileAttachment attachment, string basePath)
        //{
        //    if (attachment == null) { return; }
        //    if(string.IsNullOrEmpty(basePath)) { return;}

           
        //    if(File.Exists(Path.Combine(basePath, attachment.ServerFileName)))
        //    {
        //        File.Delete(Path.Combine(basePath, attachment.ServerFileName));
        //    }
            

        //}

        

        private static List<FileAttachment> LoadListFromReader(IDataReader reader)
        {
            List<FileAttachment> fileAttachmentList = new List<FileAttachment>();
            try
            {
                while (reader.Read())
                {
                    FileAttachment fileAttachment = new FileAttachment();
                    fileAttachment.rowGuid = new Guid(reader["RowGuid"].ToString());
                    fileAttachment.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    fileAttachment.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                    fileAttachment.itemGuid = new Guid(reader["ItemGuid"].ToString());
                    fileAttachment.specialGuid1 = new Guid(reader["SpecialGuid1"].ToString());
                    fileAttachment.specialGuid2 = new Guid(reader["SpecialGuid2"].ToString());
                    fileAttachment.serverFileName = reader["ServerFileName"].ToString();
                    fileAttachment.fileName = reader["FileName"].ToString();
                    fileAttachment.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    fileAttachment.createdBy = new Guid(reader["CreatedBy"].ToString());

                    if (reader["ContentLength"] != DBNull.Value)
                    {
                        fileAttachment.contentLength = Convert.ToInt64(reader["ContentLength"]);
                    }

                    fileAttachment.contentTitle = reader["ContentTitle"].ToString();
                    fileAttachment.contentType = reader["ContentType"].ToString();

                    fileAttachmentList.Add(fileAttachment);

                }
            }
            finally
            {
                reader.Close();
            }

            return fileAttachmentList;

        }

        
        /// <summary>
        /// Gets an IDataReader with row sfrom the mp_FileAttachment table.
        /// </summary>
        /// <param name="moduleGuid"> moduleGuid </param>
        public static IDataReader SelectByModule(Guid moduleGuid)
        {
            return DBFileAttachment.SelectByModule(moduleGuid);
        }

        public static List<FileAttachment> GetListByModule(Guid moduleGuid)
        {
            using (IDataReader reader = DBFileAttachment.SelectByModule(moduleGuid))
            {
                return LoadListFromReader(reader);
            }
        }

        
        public static IDataReader SelectByItem(Guid itemGuid)
        {
            return DBFileAttachment.SelectByItem(itemGuid);
        }

        
        public static List<FileAttachment> GetListByItem(Guid itemGuid)
        {
            using (IDataReader reader = DBFileAttachment.SelectByItem(itemGuid))
            {
                return LoadListFromReader(reader);
            }
        }

        
        public static IDataReader SelectBySpecial1(Guid specialGuid1)
        {
            return DBFileAttachment.SelectBySpecial1(specialGuid1);
        }

       
        public static IDataReader SelectBySpecial2(Guid specialGuid2)
        {
            return DBFileAttachment.SelectBySpecial2(specialGuid2);
        }


        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of FileAttachment.
        /// </summary>
        public static int CompareByServerFileName(FileAttachment fileAttachment1, FileAttachment fileAttachment2)
        {
            return fileAttachment1.ServerFileName.CompareTo(fileAttachment2.ServerFileName);
        }
        /// <summary>
        /// Compares 2 instances of FileAttachment.
        /// </summary>
        public static int CompareByFileName(FileAttachment fileAttachment1, FileAttachment fileAttachment2)
        {
            return fileAttachment1.FileName.CompareTo(fileAttachment2.FileName);
        }
        /// <summary>
        /// Compares 2 instances of FileAttachment.
        /// </summary>
        public static int CompareByCreatedUtc(FileAttachment fileAttachment1, FileAttachment fileAttachment2)
        {
            return fileAttachment1.CreatedUtc.CompareTo(fileAttachment2.CreatedUtc);
        }

        #endregion


    }

}
