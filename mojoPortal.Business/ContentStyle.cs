// Author:					
// Created:					2009-06-02
// Last Modified:			2009-06-02
// 
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
using mojoPortal.Data;

namespace mojoPortal.Business
{

    public class ContentStyle
    {

        #region Private Constructors

        private ContentStyle()
        { }


        private ContentStyle(Guid guid)
        {
            GetContentStyle(guid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private string name = string.Empty;
        private string element = string.Empty;
        private string cssClass = string.Empty;
        private string skinName = string.Empty;
        private bool isActive = true;
        private DateTime createdUtc = DateTime.UtcNow;
        private DateTime lastModUtc = DateTime.UtcNow;
        private Guid createdBy = Guid.Empty;
        private Guid lastModBy = Guid.Empty;

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
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Element
        {
            get { return element; }
            set { element = value; }
        }
        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }
        public string SkinName
        {
            get { return skinName; }
            set { skinName = value; }
        }
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
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
        public Guid CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Guid LastModBy
        {
            get { return lastModBy; }
            set { lastModBy = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of ContentStyle.
        /// </summary>
        /// <param name="guid"> guid </param>
        private void GetContentStyle(Guid guid)
        {
            using (IDataReader reader = DBContentStyle.GetOne(guid))
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
                this.name = reader["Name"].ToString();
                this.element = reader["Element"].ToString();
                this.cssClass = reader["CssClass"].ToString();
                this.skinName = reader["SkinName"].ToString();
                this.isActive = Convert.ToBoolean(reader["IsActive"]);
                this.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                this.lastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
                this.createdBy = new Guid(reader["CreatedBy"].ToString());
                this.lastModBy = new Guid(reader["LastModBy"].ToString());

            }

        }

        /// <summary>
        /// Persists a new instance of ContentStyle. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.guid = Guid.NewGuid();

            int rowsAffected = DBContentStyle.Create(
                this.guid,
                this.siteGuid,
                this.name,
                this.element,
                this.cssClass,
                this.skinName,
                this.isActive,
                DateTime.UtcNow,
                this.createdBy);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of ContentStyle. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBContentStyle.Update(
                this.guid,
                this.siteGuid,
                this.name,
                this.element,
                this.cssClass,
                this.skinName,
                this.isActive,
                DateTime.UtcNow,
                this.lastModBy);

        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of ContentStyle. Returns true on success.
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
        /// Looks up and returns an instance of ContentStyle, if not found returns null.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static ContentStyle Get(Guid guid)
        {
            ContentStyle style = new ContentStyle(guid);
            if (style.Guid == Guid.Empty) { return null; }
            return style;
        }

        public static ContentStyle GetNew(Guid siteGuid)
        {
            ContentStyle style = new ContentStyle();
            style.siteGuid = siteGuid;
            return style;
        }

        /// <summary>
        /// Deletes an instance of ContentStyle. Returns true on success.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            return DBContentStyle.Delete(guid);
        }

        /// <summary>
        /// Deletes rows from the mp_ContentStyle table for the passed siteGuid. Returns true if rows deleted.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            return DBContentStyle.DeleteBySite(siteGuid);
        }

        /// <summary>
        /// Deletes rows from the mp_ContentStyle table for the passed siteGuid. Returns true if rows deleted.
        /// </summary>  
        public static bool DeleteBySkin(Guid siteGuid, string skinName)
        {
            return DBContentStyle.DeleteBySkin(siteGuid, skinName);

        }

        /// <summary>
        /// Makes all styles of the given skin name active.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="skinName"> skinName </param>
        /// <returns>bool</returns>
        public static bool MakeActiveBySkin(Guid siteGuid, string skinName)
        {
            bool isActive = true;
            return DBContentStyle.SetActivationBySkin(siteGuid, skinName, isActive);
        }

        /// <summary>
        /// Makes all styles of the given skin name inactive.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="skinName"> skinName </param>
        /// <returns>bool</returns>
        public static bool MakeInActiveBySkin(Guid siteGuid, string skinName)
        {
            bool isActive = false;
            return DBContentStyle.SetActivationBySkin(siteGuid, skinName, isActive);
        }


        /// <summary>
        /// Gets a count of ContentStyle. 
        /// </summary>
        public static int GetCount(Guid siteGuid)
        {
            return DBContentStyle.GetCount(siteGuid);
        }

        /// <summary>
        /// Gets a count of ContentStyle. 
        /// </summary>
        public static int GetCount(Guid siteGuid, string skinName)
        {
            return DBContentStyle.GetCount(siteGuid, skinName);
        }

        private static List<ContentStyle> LoadListFromReader(IDataReader reader)
        {
            List<ContentStyle> contentStyleList = new List<ContentStyle>();
            try
            {
                while (reader.Read())
                {
                    ContentStyle contentStyle = new ContentStyle();
                    contentStyle.guid = new Guid(reader["Guid"].ToString());
                    contentStyle.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    contentStyle.name = reader["Name"].ToString();
                    contentStyle.element = reader["Element"].ToString();
                    contentStyle.cssClass = reader["CssClass"].ToString();
                    contentStyle.skinName = reader["SkinName"].ToString();
                    contentStyle.isActive = Convert.ToBoolean(reader["IsActive"]);
                    contentStyle.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    contentStyle.lastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
                    contentStyle.createdBy = new Guid(reader["CreatedBy"].ToString());
                    contentStyle.lastModBy = new Guid(reader["LastModBy"].ToString());
                    contentStyleList.Add(contentStyle);

                }
            }
            finally
            {
                reader.Close();
            }

            return contentStyleList;

        }

        /// <summary>
        /// Gets an IList with all instances of ContentStyle.
        /// </summary>
        public static List<ContentStyle> GetAll(Guid siteGuid)
        {
            IDataReader reader = DBContentStyle.GetAll(siteGuid);
            return LoadListFromReader(reader);

        }

        /// <summary>
        /// Gets an IList with all instances of ContentStyle for the given skin.
        /// </summary>
        public static List<ContentStyle> GetAll(Guid siteGuid, string skinName)
        {
            IDataReader reader = DBContentStyle.GetAll(siteGuid, skinName);
            return LoadListFromReader(reader);

        }

        /// <summary>
        /// Gets an IDataReader with all active instances of ContentStyle.
        /// </summary>
        public static IDataReader GetAllActive(Guid siteGuid)
        {
            return DBContentStyle.GetAllActive(siteGuid);
           
        }

        /// <summary>
        /// Gets an IDataReader with all active instances of ContentStyle for the given skin.
        /// </summary>
        public static IDataReader GetAllActive(Guid siteGuid, string skinName)
        {
            return DBContentStyle.GetAllActive(siteGuid, skinName);
           
        }

        /// <summary>
        /// Gets an IList with page of instances of ContentStyle.
        /// </summary>
        public static List<ContentStyle> GetPage(
            Guid siteGuid,
            int pageNumber, 
            int pageSize, 
            out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBContentStyle.GetPage(siteGuid, pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }

        /// <summary>
        /// Gets an IList with page of instances of ContentStyle.
        /// </summary>
        public static List<ContentStyle> GetPage(
            Guid siteGuid,
            string skinName,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBContentStyle.GetPage(siteGuid, skinName, pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }



        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of ContentStyle.
        /// </summary>
        public static int CompareByName(ContentStyle contentStyle1, ContentStyle contentStyle2)
        {
            return contentStyle1.Name.CompareTo(contentStyle2.Name);
        }
        /// <summary>
        /// Compares 2 instances of ContentStyle.
        /// </summary>
        public static int CompareByElement(ContentStyle contentStyle1, ContentStyle contentStyle2)
        {
            return contentStyle1.Element.CompareTo(contentStyle2.Element);
        }
        /// <summary>
        /// Compares 2 instances of ContentStyle.
        /// </summary>
        public static int CompareByCssClass(ContentStyle contentStyle1, ContentStyle contentStyle2)
        {
            return contentStyle1.CssClass.CompareTo(contentStyle2.CssClass);
        }
        /// <summary>
        /// Compares 2 instances of ContentStyle.
        /// </summary>
        public static int CompareBySkinName(ContentStyle contentStyle1, ContentStyle contentStyle2)
        {
            return contentStyle1.SkinName.CompareTo(contentStyle2.SkinName);
        }
        /// <summary>
        /// Compares 2 instances of ContentStyle.
        /// </summary>
        public static int CompareByCreatedUtc(ContentStyle contentStyle1, ContentStyle contentStyle2)
        {
            return contentStyle1.CreatedUtc.CompareTo(contentStyle2.CreatedUtc);
        }
        /// <summary>
        /// Compares 2 instances of ContentStyle.
        /// </summary>
        public static int CompareByLastModUtc(ContentStyle contentStyle1, ContentStyle contentStyle2)
        {
            return contentStyle1.LastModUtc.CompareTo(contentStyle2.LastModUtc);
        }

        #endregion


    }

}
