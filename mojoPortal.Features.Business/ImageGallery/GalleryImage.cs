// Author:					
// Created:				    2004-12-03
// Last Modified:			2011-08-22
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
using System.Xml;
using log4net;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    /// Represents an image in the gallery
    /// </summary>
    public class GalleryImage : IIndexableContent
    {

        #region Constructors

        public GalleryImage()
        {
            this.imageFile = ModuleId.ToString() + "mfull" + Guid.NewGuid().ToString() + ".jpg";
            this.webImageFile = ModuleId.ToString() + "mweb" + Guid.NewGuid().ToString() + ".jpg";
            this.thumbnailFile = ModuleId.ToString() + "mthumb" + Guid.NewGuid().ToString() + ".jpg";

        }

        public GalleryImage(int galleryId)
        {
            this.moduleID = galleryId;
            this.metaData = new XmlDocument();
            this.metaData.XmlResolver = null;
            this.metaData.AppendChild(this.metaData.CreateElement("MetaData"));

            this.imageFile = moduleID.ToString() + "mfull" + Guid.NewGuid().ToString() + ".jpg";
            this.webImageFile = moduleID.ToString() + "mweb" + Guid.NewGuid().ToString() + ".jpg";
            this.thumbnailFile = moduleID.ToString() + "mthumb" + Guid.NewGuid().ToString() + ".jpg";
        }


        public GalleryImage(int galleryId, int itemId)
        {
            this.moduleID = galleryId;
            
            GetGalleryImage(itemId);
        }

        #endregion

        #region Private Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(GalleryImage));

        private Guid itemGuid = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;
        private Guid userGuid = Guid.Empty;
        private int itemID = -1;
        private int moduleID = -1;
        private int displayOrder = 100;
        private string caption = string.Empty;
        private string description = string.Empty;
        private XmlDocument metaData;
        private string metaDataXml = string.Empty;
        private string imageFile = string.Empty;
        private string webImageFile = string.Empty;
        private string thumbnailFile = string.Empty;
        private DateTime uploadDate = DateTime.UtcNow;
        private string uploadUser = string.Empty;
       // private string storageFolderPath = string.Empty;
        private int thumbNailHeight = 75;
        private int thumbNailWidth = 100;
        private int webImageHeight = 320;
        private int webImageWidth = 480;


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

        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }

        public int ThumbNailHeight
        {
            get { return thumbNailHeight; }
            set { thumbNailHeight = value; }
        }

        public int ThumbNailWidth
        {
            get { return thumbNailWidth; }
            set { thumbNailWidth = value; }
        }

        public int WebImageHeight
        {
            get { return webImageHeight; }
            set { webImageHeight = value; }
        }

        public int WebImageWidth
        {
            get { return webImageWidth; }
            set { webImageWidth = value; }
        }

        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string MetaDataXml
        {
            get { return metaDataXml; }
            set { metaDataXml = value; }
        }
        //public string StorageFolderPath
        //{
        //    get { return storageFolderPath; }
        //    set { storageFolderPath = value; }
        //}
        public string ImageFile
        {
            get { return imageFile; }
            set { imageFile = value; }
        }
        public string WebImageFile
        {
            get { return webImageFile; }
            set { webImageFile = value; }
        }
        public string ThumbnailFile
        {
            get { return thumbnailFile; }
            set { thumbnailFile = value; }
        }
        public DateTime UploadDate
        {
            get { return uploadDate; }
            set { uploadDate = value; }
        }
        public string UploadUser
        {
            get { return uploadUser; }
            set { uploadUser = value; }
        }

        #endregion

        #region Private Methods

        private void GetGalleryImage(int itemId)
        {
            using (IDataReader reader = DBGallery.GetGalleryImage(itemId))
            {
                if (reader.Read())
                {
                    this.itemID = Convert.ToInt32(reader["ItemID"]);
                    this.moduleID = Convert.ToInt32(reader["ModuleID"]);
                    this.displayOrder = Convert.ToInt32(reader["DisplayOrder"]);
                    this.caption = reader["Caption"].ToString();
                    this.description = reader["Description"].ToString();
                    this.metaDataXml = reader["MetaDataXml"].ToString();
                    this.imageFile = reader["ImageFile"].ToString();
                    this.webImageFile = reader["WebImageFile"].ToString();
                    this.thumbnailFile = reader["ThumbnailFile"].ToString();
                    this.uploadDate = Convert.ToDateTime(reader["UploadDate"]);
                    this.uploadUser = reader["UploadUser"].ToString();

                    this.itemGuid = new Guid(reader["ItemGuid"].ToString());
                    this.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                    string user = reader["UserGuid"].ToString();
                    if (user.Length == 36) this.userGuid = new Guid(user);

                }
            }

        }

        private bool Create()
        {
            int newID = -1;
            this.itemGuid = Guid.NewGuid();

            newID = DBGallery.AddGalleryImage(
                this.itemGuid,
                this.moduleGuid,
                this.moduleID,
                this.displayOrder,
                this.caption,
                this.description,
                this.metaDataXml,
                this.imageFile,
                this.webImageFile,
                this.thumbnailFile,
                this.uploadDate,
                this.uploadUser,
                this.userGuid);

            this.itemID = newID;

            bool result = (newID > -1);

            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                OnContentChanged(e);
            }

            //IndexHelper.IndexItem(this);

            return result;

        }

        private bool Update()
        {

            bool result = DBGallery.UpdateGalleryImage(
                this.itemID,
                this.moduleID,
                this.displayOrder,
                this.caption,
                this.description,
                this.metaDataXml,
                this.imageFile,
                this.webImageFile,
                this.thumbnailFile,
                this.uploadDate,
                this.uploadUser);

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
            if (this.itemID > -1)
            {
                return Update();
            }
            else
            {
                return Create();
            }


        }

        

        

        public bool Delete()
        {
            bool result = false;

            result = DBGallery.DeleteGalleryImage(itemID);

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

        //public static bool DeleteGalleryImage(int moduleID, int itemID) 
        //{
        //    bool result =  dbGallery.DeleteGalleryImage(itemID);

        //    IndexHelper.RemoveGalleryImageIndexItem(moduleID, itemID);

        //    return result;
        //}

        public static DataTable GetImagesByPage(int siteId, int pageId)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ItemID", typeof(int));
            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("Caption", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("ViewRoles", typeof(string));
            dataTable.Columns.Add("UploadDate", typeof(DateTime));

            using (IDataReader reader = DBGallery.GetImagesByPage(siteId, pageId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();

                    row["ItemID"] = reader["ItemID"];
                    row["ModuleID"] = reader["ModuleID"];
                    row["ModuleTitle"] = reader["ModuleTitle"];
                    row["Caption"] = reader["Caption"];
                    row["Description"] = reader["Description"];
                    row["ViewRoles"] = reader["ViewRoles"];
                    row["UploadDate"] = Convert.ToDateTime(reader["UploadDate"]);

                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;

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
