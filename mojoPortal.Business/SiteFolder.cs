// Author:					
// Created:				    2007-05-11
// Last Modified:			2009-02-01
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
using System.Configuration;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    /// Represents a site folder used to demark a sub site in a multi site installation using folder based child sites.
    /// </summary>
    public class SiteFolder
    {

        #region Constructors

        public SiteFolder()
        { }


        public SiteFolder(Guid guid)
        {
            GetSiteFolder(guid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private string folderName = string.Empty;

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
        public string FolderName
        {
            get { return folderName; }
            set { folderName = value; }
        }

        #endregion

        #region Private Methods

        private void GetSiteFolder(Guid guid)
        {
            using (IDataReader reader = DBSiteFolder.GetOne(guid))
            {
                if (reader.Read())
                {
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    this.folderName = reader["FolderName"].ToString();

                }

            }

        }

        private bool Create()
        {
            Guid newID = Guid.NewGuid();

            this.guid = newID;

            int rowsAffected = DBSiteFolder.Add(
                this.guid,
                this.siteGuid,
                this.folderName);

            return (rowsAffected > 0);

        }



        private bool Update()
        {

            return DBSiteFolder.Update(
                this.guid,
                this.siteGuid,
                this.folderName);

        }


        #endregion

        #region Public Methods


        public bool Save()
        {
            if (!IsAllowedFolder(this.folderName))
            {
                throw new ArgumentException("Invalid Folder Name");
            }

            if (HasInvalidChars(this.folderName))
            {
                throw new ArgumentException("Invalid Folder Name");
            }

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

        public static Guid GetSiteGuid(string folderName)
        {
            return DBSiteFolder.GetSiteGuid(folderName);
        }

        public static bool Delete(Guid guid)
        {
            return DBSiteFolder.Delete(guid);
        }

        public static bool Exists(string folderName)
        {
            return DBSiteFolder.Exists(folderName);
        }

        public static bool IsAllowedFolder(string folderName)
        {
            bool result = true;
            if (ConfigurationManager.AppSettings["DisallowedVirtualFolderNames"] != null)
            {
                string[] disallowedNames 
                    = ConfigurationManager.AppSettings["DisallowedVirtualFolderNames"].Split(new char[] { ';' });

                foreach (string disallowedName in disallowedNames)
                {
                    if(string.Equals(folderName, disallowedName, StringComparison.InvariantCultureIgnoreCase))result = false;
                }

            }


            return result;
            
        }

        public static bool HasInvalidChars(string folderName)
        {
            bool result = false;
            char[] nameChars = folderName.ToCharArray();
            foreach(char c in nameChars)
            {
                if (!Char.IsLetterOrDigit(c) &&(c != '-')) // allow dashes 2014-01-10
                {
                    result = true;
                }
            }

            return result;
        }



        public static List<SiteFolder> GetBySite(Guid siteGuid)
        {
            List<SiteFolder> siteFolderList
                = new List<SiteFolder>();

            using (IDataReader reader = DBSiteFolder.GetBySite(siteGuid))
            {
                while (reader.Read())
                {
                    SiteFolder siteFolder = new SiteFolder();
                    siteFolder.guid = new Guid(reader["Guid"].ToString());
                    siteFolder.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    siteFolder.folderName = reader["FolderName"].ToString();
                    siteFolderList.Add(siteFolder);
                }
            }

            return siteFolderList;

        }

        
        #endregion

        #region Comparison Methods

        public static int CompareByFolderName(SiteFolder siteFolder1, SiteFolder siteFolder2)
        {
            return siteFolder1.FolderName.CompareTo(siteFolder2.FolderName);
        }

        #endregion


    }

}





