// Author:					
// Created:					2009-06-01
// Last Modified:			2009-06-01
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

    public class ContentTemplate
    {

        #region Private Constructors

        private ContentTemplate()
        { }


        private ContentTemplate(Guid guid)
        {
            GetContentTemplate(guid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private string title = string.Empty;
        private string imageFileName = "blank.gif";
        private string description = string.Empty;
        private string body = string.Empty;
        private string allowedRoles = string.Empty;
        private Guid createdByUser = Guid.Empty;
        private Guid lastModUser = Guid.Empty;
        private DateTime createdUtc = DateTime.UtcNow;
        private DateTime lastModUtc = DateTime.UtcNow;

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string ImageFileName
        {
            get { return imageFileName; }
            set { imageFileName = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string Body
        {
            get { return body; }
            set { body = value; }
        }
        public string AllowedRoles
        {
            get { return allowedRoles; }
            set { allowedRoles = value; }
        }
        public Guid CreatedByUser
        {
            get { return createdByUser; }
            set { createdByUser = value; }
        }
        public Guid LastModUser
        {
            get { return lastModUser; }
            set 
            { 
                lastModUser = value;
                if (createdByUser == Guid.Empty) { createdByUser = value; }
            }
        }
        public DateTime CreatedUtc
        {
            get { return createdUtc; }
            set { createdUtc = value; }
        }
        public DateTime LastModUtc
        {
            get { return lastModUtc; }
            set { lastModUtc = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of ContentTemplate.
        /// </summary>
        /// <param name="guid"> guid </param>
        private void GetContentTemplate(
            Guid guid)
        {
            using (IDataReader reader = DBContentTemplate.GetOne(guid))
            {
                PopulateFromReader(reader);
            }

        }


        private void PopulateFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                this.guid = new Guid(reader["Guid"].ToString());
                this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                this.title = reader["Title"].ToString();
                this.imageFileName = reader["ImageFileName"].ToString();
                this.description = reader["Description"].ToString();
                this.body = reader["Body"].ToString();
                this.allowedRoles = reader["AllowedRoles"].ToString();
                this.createdByUser = new Guid(reader["CreatedByUser"].ToString());
                this.lastModUser = new Guid(reader["LastModUser"].ToString());
                this.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                this.lastModUtc = Convert.ToDateTime(reader["LastModUtc"]);

            }

        }

        /// <summary>
        /// Persists a new instance of ContentTemplate. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.guid = Guid.NewGuid();

            int rowsAffected = DBContentTemplate.Create(
                this.guid,
                this.siteGuid,
                this.title,
                this.imageFileName,
                this.description,
                this.body,
                this.allowedRoles,
                this.createdByUser,
                this.createdUtc);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of ContentTemplate. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBContentTemplate.Update(
                this.guid,
                this.siteGuid,
                this.title,
                this.imageFileName,
                this.description,
                this.body,
                this.allowedRoles,
                this.lastModUser,
                this.lastModUtc);

        }





        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of ContentTemplate. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.guid != Guid.Empty)
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
        /// Looks up and returns an instance of ContentTemplate, if not found returns null.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static ContentTemplate Get(Guid guid)
        {
            ContentTemplate template = new ContentTemplate(guid);
            if (template.Guid == Guid.Empty) { return null; }
            return template;
        }

        public static ContentTemplate GetNew(Guid siteGuid)
        {
            ContentTemplate template = new ContentTemplate();
            template.siteGuid = siteGuid;
            return template;
        }

        public static ContentTemplate GetEmpty()
        {
            ContentTemplate template = new ContentTemplate();
            
            return template;
        }

        /// <summary>
        /// Deletes an instance of ContentTemplate. Returns true on success.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            return DBContentTemplate.Delete(guid);
        }


        /// <summary>
        /// Gets a count of ContentTemplate. 
        /// </summary>
        public static int GetCount(Guid siteGuid)
        {
            return DBContentTemplate.GetCount(siteGuid);
        }

        private static List<ContentTemplate> LoadListFromReader(IDataReader reader)
        {
            List<ContentTemplate> contentTemplateList = new List<ContentTemplate>();
            try
            {
                while (reader.Read())
                {
                    ContentTemplate contentTemplate = new ContentTemplate();
                    contentTemplate.guid = new Guid(reader["Guid"].ToString());
                    contentTemplate.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    contentTemplate.title = reader["Title"].ToString();
                    contentTemplate.imageFileName = reader["ImageFileName"].ToString();
                    contentTemplate.description = reader["Description"].ToString();
                    contentTemplate.body = reader["Body"].ToString();
                    contentTemplate.allowedRoles = reader["AllowedRoles"].ToString();
                    contentTemplate.createdByUser = new Guid(reader["CreatedByUser"].ToString());
                    contentTemplate.lastModUser = new Guid(reader["LastModUser"].ToString());
                    contentTemplate.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    contentTemplate.lastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
                    contentTemplateList.Add(contentTemplate);

                }
            }
            finally
            {
                reader.Close();
            }

            return contentTemplateList;

        }

        /// <summary>
        /// Gets an IList with all instances of ContentTemplate.
        /// </summary>
        public static List<ContentTemplate> GetAll(Guid siteGuid)
        {
            IDataReader reader = DBContentTemplate.GetAll(siteGuid);
            return LoadListFromReader(reader);

        }

        /// <summary>
        /// Gets an IList with page of instances of ContentTemplate.
        /// </summary>
        public static List<ContentTemplate> GetPage(Guid siteGuid, int pageNumber, int pageSize, out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBContentTemplate.GetPage(siteGuid, pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }



        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of ContentTemplate.
        /// </summary>
        public static int CompareByTitle(ContentTemplate contentTemplate1, ContentTemplate contentTemplate2)
        {
            return contentTemplate1.Title.CompareTo(contentTemplate2.Title);
        }
        /// <summary>
        /// Compares 2 instances of ContentTemplate.
        /// </summary>
        public static int CompareByImageFileName(ContentTemplate contentTemplate1, ContentTemplate contentTemplate2)
        {
            return contentTemplate1.ImageFileName.CompareTo(contentTemplate2.ImageFileName);
        }
        /// <summary>
        /// Compares 2 instances of ContentTemplate.
        /// </summary>
        public static int CompareByDescription(ContentTemplate contentTemplate1, ContentTemplate contentTemplate2)
        {
            return contentTemplate1.Description.CompareTo(contentTemplate2.Description);
        }
        /// <summary>
        /// Compares 2 instances of ContentTemplate.
        /// </summary>
        public static int CompareByBody(ContentTemplate contentTemplate1, ContentTemplate contentTemplate2)
        {
            return contentTemplate1.Body.CompareTo(contentTemplate2.Body);
        }
        /// <summary>
        /// Compares 2 instances of ContentTemplate.
        /// </summary>
        public static int CompareByAllowedRoles(ContentTemplate contentTemplate1, ContentTemplate contentTemplate2)
        {
            return contentTemplate1.AllowedRoles.CompareTo(contentTemplate2.AllowedRoles);
        }
        /// <summary>
        /// Compares 2 instances of ContentTemplate.
        /// </summary>
        public static int CompareByCreatedUtc(ContentTemplate contentTemplate1, ContentTemplate contentTemplate2)
        {
            return contentTemplate1.CreatedUtc.CompareTo(contentTemplate2.CreatedUtc);
        }
        /// <summary>
        /// Compares 2 instances of ContentTemplate.
        /// </summary>
        public static int CompareByLastModUtc(ContentTemplate contentTemplate1, ContentTemplate contentTemplate2)
        {
            return contentTemplate1.LastModUtc.CompareTo(contentTemplate2.LastModUtc);
        }

        #endregion


    }

}
