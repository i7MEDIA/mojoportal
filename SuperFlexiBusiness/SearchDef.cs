// Author:					i7MEDIA
// Created:					2016-09-12
// Last Modified:			2016-09-12
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using SuperFlexiData;

namespace SuperFlexiBusiness
{
    public class SearchDef : Hashtable
    {
        #region Constructors

        public SearchDef() { }
        public SearchDef(Guid guid)
        {

            GetSearchDef(guid);
        }
        #endregion

        #region Private Properties
        private Guid guid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid featureGuid = Guid.Empty;
        private Guid fieldDefinitionGuid = Guid.Empty;
        private string title = string.Empty;
        private string keywords = string.Empty;
        private string description = string.Empty;
        private string link = string.Empty;
        private string linkQueryAddendum = string.Empty;
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
        public Guid FeatureGuid
        {
            get { return featureGuid; }
            set { featureGuid = value; }
        }
        public Guid FieldDefinitionGuid
        {
            get { return fieldDefinitionGuid; }
            set { fieldDefinitionGuid = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Keywords
        {
            get { return keywords; }
            set { keywords = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Link
        {
            get { return link; }
            set { link = value; }
        }

        public string LinkQueryAddendum
        {
            get { return linkQueryAddendum; }
            set { linkQueryAddendum = value; }
        }

        #endregion

        #region private methods
        private void GetSearchDef(Guid guid)
        {
            using (IDataReader reader = DBSearchDefs.GetOne(guid))
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
                this.featureGuid = new Guid(reader["FeatureGuid"].ToString());
                this.fieldDefinitionGuid = new Guid(reader["FieldDefinitionGuid"].ToString());
                this.title = reader["Title"].ToString();
                this.keywords = reader["Keywords"].ToString();
                this.description = reader["Description"].ToString();
                this.link = reader["Link"].ToString();
                this.linkQueryAddendum = reader["LinkQueryAddendum"].ToString();


            }
        }

        private bool Create()
        {
            this.guid = Guid.NewGuid();
            return DBSearchDefs.Create(this.guid, this.siteGuid, this.featureGuid, this.fieldDefinitionGuid, this.title, this.keywords, this.description, this.link, this.linkQueryAddendum);
        }

        private bool Update()
        {
            return DBSearchDefs.Update(this.guid, this.siteGuid, this.featureGuid, this.fieldDefinitionGuid, this.title, this.keywords, this.description, this.link, this.linkQueryAddendum);
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

        /// <summary>
        /// Deletes a search definition. Returns true on success.
        /// </summary>
        /// <param name="defGuid"> defGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid defGuid)
        {
            return DBSearchDefs.Delete(defGuid);
        }

        /// <summary>
        /// Deletes Search Definitions by Site. Returns true on success.
        /// </summary>
        public static bool DeleteBySite(Guid siteGuid)
        {
            return DBSearchDefs.DeleteBySite(siteGuid);
        }

        /// <summary>
        /// Deletes Search Definition by Field Definition. Returns true on success.
        /// </summary>
        /// <param name="fieldDefGuid"> fieldDefGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByFieldDefinition(Guid fieldDefGuid)
        {
            return DBSearchDefs.DeleteByFieldDefinition(fieldDefGuid);
        }

        public static SearchDef GetByFieldDefinition(Guid fieldDefinitionGuid)
        {
            SearchDef searchDef = new SearchDef();
            using (IDataReader reader = DBSearchDefs.GetByFieldDefinition(fieldDefinitionGuid))
            {
                searchDef.PopulateFromReader(reader);
            }

            if (searchDef.FieldDefinitionGuid == Guid.Empty) return null;

            return searchDef;
        }
        #endregion
    }
}
