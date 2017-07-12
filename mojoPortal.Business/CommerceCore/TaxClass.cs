// Author:					
// Created:				    2007-02-17
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
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    /// Represents the taxability of a product or service
    /// </summary>
    public class TaxClass
    {

        #region Constructors

        public TaxClass()
        { }


        public TaxClass(Guid guid)
        {
            GetTaxClass(guid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private string title = string.Empty;
        private string description = string.Empty;
        private DateTime lastModified = DateTime.UtcNow;
        private DateTime created = DateTime.UtcNow;

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
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public DateTime LastModified
        {
            get { return lastModified; }
            set { lastModified = value; }
        }
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        #endregion

        #region Private Methods

        private void GetTaxClass(Guid guid)
        {
            using (IDataReader reader = DBTaxClass.GetOne(guid))
            {
                if (reader.Read())
                {
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    this.title = reader["Title"].ToString();
                    this.description = reader["Description"].ToString();
                    this.lastModified = Convert.ToDateTime(reader["LastModified"]);
                    this.created = Convert.ToDateTime(reader["Created"]);

                }

            }

        }

        private bool Create()
        {
            Guid newID = Guid.NewGuid();

            this.guid = newID;

            int rowsAffected = DBTaxClass.Create(
                this.guid,
                this.siteGuid,
                this.title,
                this.description,
                this.lastModified,
                this.created);

            return (rowsAffected > 0);

        }



        private bool Update()
        {

            return DBTaxClass.Update(
                this.guid,
                this.title,
                this.description,
                this.lastModified,
                this.created);

        }


        #endregion

        #region Public Methods


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

        public static bool Delete(Guid guid)
        {
            return DBTaxClass.Delete(guid);
        }

        private static List<TaxClass> LoadListFromReader(IDataReader reader)
        {
            List<TaxClass> taxClassList = new List<TaxClass>();
            try
            {
                while (reader.Read())
                {
                    TaxClass taxClass = new TaxClass();
                    taxClass.guid = new Guid(reader["Guid"].ToString());
                    taxClass.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    taxClass.title = reader["Title"].ToString();
                    taxClass.description = reader["Description"].ToString();
                    taxClass.lastModified = Convert.ToDateTime(reader["LastModified"]);
                    taxClass.created = Convert.ToDateTime(reader["Created"]);
                    taxClassList.Add(taxClass);

                }
            }
            finally
            {
                reader.Close();
            }

            return taxClassList;

        }


        public static IDataReader GetBySite(Guid siteGuid)
        {
            return DBTaxClass.GetBySite(siteGuid);
        }

        public static List<TaxClass> GetList(Guid siteGuid)
        {
            IDataReader reader = DBTaxClass.GetBySite(siteGuid);

            return LoadListFromReader(reader);

        }


        public static IDataReader GetPage(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            
            return DBTaxClass.GetPage(storeGuid, pageNumber, pageSize, out totalPages);
            
        }

        



        #endregion


        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of TaxClas.
        /// </summary>
        public static int CompareByTitle(TaxClass taxClass1, TaxClass taxClass2)
        {
            return taxClass1.Title.CompareTo(taxClass2.Title);
        }
        /// <summary>
        /// Compares 2 instances of TaxClas.
        /// </summary>
        public static int CompareByDescription(TaxClass taxClass1, TaxClass taxClass2)
        {
            return taxClass1.Description.CompareTo(taxClass2.Description);
        }
        /// <summary>
        /// Compares 2 instances of TaxClas.
        /// </summary>
        public static int CompareByLastModified(TaxClass taxClass1, TaxClass taxClass2)
        {
            return taxClass1.LastModified.CompareTo(taxClass2.LastModified);
        }
        /// <summary>
        /// Compares 2 instances of TaxClas.
        /// </summary>
        public static int CompareByCreated(TaxClass taxClass1, TaxClass taxClass2)
        {
            return taxClass1.Created.CompareTo(taxClass2.Created);
        }

        #endregion


    }

}
