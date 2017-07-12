// Author:					
// Created:				    2007-12-27
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
    /// Represents an html template for email news letter
    /// </summary>
    public class LetterHtmlTemplate
    {

        #region Constructors

        public LetterHtmlTemplate()
        { }


        public LetterHtmlTemplate(
            Guid guid)
        {
            GetLetterHtmlTemplate(
                guid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private string title = string.Empty;
        private string html = string.Empty;
        private DateTime lastModUTC = DateTime.UtcNow;

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
        public string Html
        {
            get { return html; }
            set { html = value; }
        }
        public DateTime LastModUTC
        {
            get { return lastModUTC; }
            set { lastModUTC = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of LetterHtmlTemplate.
        /// </summary>
        /// <param name="guid"> guid </param>
        private void GetLetterHtmlTemplate(Guid guid)
        {
            using (IDataReader reader = DBLetterHtmlTemplate.GetOne(guid))
            {
                if (reader.Read())
                {
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    this.title = reader["Title"].ToString();
                    this.html = reader["Html"].ToString();
                    this.lastModUTC = Convert.ToDateTime(reader["LastModUTC"]);

                }

            }

        }

        /// <summary>
        /// Persists a new instance of LetterHtmlTemplate. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            Guid newID = Guid.NewGuid();

            this.guid = newID;

            int rowsAffected = DBLetterHtmlTemplate.Create(
                this.guid,
                this.siteGuid,
                this.title,
                this.html,
                this.lastModUTC);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of LetterHtmlTemplate. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            this.lastModUTC = DateTime.UtcNow;

            return DBLetterHtmlTemplate.Update(
                this.guid,
                this.title,
                this.html,
                this.lastModUTC);

        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of LetterHtmlTemplate. Returns true on success.
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
        /// Deletes an instance of LetterHtmlTemplate. Returns true on success.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            return DBLetterHtmlTemplate.Delete(guid);
        }

        /// <summary>
        /// Gets an IList with all instances of LetterHtmlTemplate.
        /// </summary>
        public static List<LetterHtmlTemplate> GetAll(Guid siteGuid)
        {
            List<LetterHtmlTemplate> letterHtmlTemplateList
                = new List<LetterHtmlTemplate>();

            using (IDataReader reader = DBLetterHtmlTemplate.GetAll(siteGuid))
            {
                while (reader.Read())
                {
                    LetterHtmlTemplate letterHtmlTemplate = new LetterHtmlTemplate();
                    letterHtmlTemplate.guid = new Guid(reader["Guid"].ToString());
                    letterHtmlTemplate.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    letterHtmlTemplate.title = reader["Title"].ToString();
                    letterHtmlTemplate.html = reader["Html"].ToString();
                    letterHtmlTemplate.lastModUTC = Convert.ToDateTime(reader["LastModUTC"]);
                    letterHtmlTemplateList.Add(letterHtmlTemplate);
                }
            }

            return letterHtmlTemplateList;

        }

        /// <summary>
        /// Gets an IList with page of instances of LetterHtmlTemplate.
        /// </summary>
        public static List<LetterHtmlTemplate> GetPage(
            Guid siteGuid,
            int pageNumber, 
            int pageSize, 
            out int totalPages)
        {
            totalPages = 1;

            List<LetterHtmlTemplate> letterHtmlTemplateList = new List<LetterHtmlTemplate>();

            using (IDataReader reader = DBLetterHtmlTemplate.GetPage(siteGuid, pageNumber, pageSize, out totalPages))
            {
                while (reader.Read())
                {
                    LetterHtmlTemplate letterHtmlTemplate = new LetterHtmlTemplate();
                    letterHtmlTemplate.guid = new Guid(reader["Guid"].ToString());
                    letterHtmlTemplate.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    letterHtmlTemplate.title = reader["Title"].ToString();
                    letterHtmlTemplate.html = reader["Html"].ToString();
                    letterHtmlTemplate.lastModUTC = Convert.ToDateTime(reader["LastModUTC"]);
                    letterHtmlTemplateList.Add(letterHtmlTemplate);

                }
            }

            return letterHtmlTemplateList;

        }



        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of LetterHtmlTemplate.
        /// </summary>
        public static int CompareByTitle(LetterHtmlTemplate letterHtmlTemplate1, LetterHtmlTemplate letterHtmlTemplate2)
        {
            return letterHtmlTemplate1.Title.CompareTo(letterHtmlTemplate2.Title);
        }
        /// <summary>
        /// Compares 2 instances of LetterHtmlTemplate.
        /// </summary>
        public static int CompareByHtml(LetterHtmlTemplate letterHtmlTemplate1, LetterHtmlTemplate letterHtmlTemplate2)
        {
            return letterHtmlTemplate1.Html.CompareTo(letterHtmlTemplate2.Html);
        }
        /// <summary>
        /// Compares 2 instances of LetterHtmlTemplate.
        /// </summary>
        public static int CompareByLastModUTC(LetterHtmlTemplate letterHtmlTemplate1, LetterHtmlTemplate letterHtmlTemplate2)
        {
            return letterHtmlTemplate1.LastModUTC.CompareTo(letterHtmlTemplate2.LastModUTC);
        }

        #endregion


    }

}
