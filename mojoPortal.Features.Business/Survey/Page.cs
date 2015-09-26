/// Author:					Rob Henry
/// Created:				2007-09-22
/// Last Modified:			2009-02-01
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Data;
using SurveyFeature.Data;
using System.Globalization;

namespace SurveyFeature.Business
{
    /// <summary>
    /// Represents a page in the survey
    /// </summary>
    public class Page
    {

        #region Constructors

        public Page()
        { }


        public Page(
            Guid pageGuid)
        {
            GetPage(
                pageGuid);
        }

        #endregion

        #region Private Properties

        private Guid pageGuid = Guid.Empty;
        private Guid surveyGuid = Guid.Empty;
        private string pageTitle;
        private int pageOrder;
        private bool pageEnabled;
        private int questionCount = 0;

        #endregion

        #region Public Properties

        public Guid SurveyPageGuid
        {
            get { return pageGuid; }
            set { pageGuid = value; }
        }
        public Guid SurveyGuid
        {
            get { return surveyGuid; }
            set { surveyGuid = value; }
        }
        public string PageTitle
        {
            get { return pageTitle; }
            set { pageTitle = value; }
        }
        public int PageOrder
        {
            get { return pageOrder; }
            set { pageOrder = value; }
        }
        public bool PageEnabled
        {
            get { return pageEnabled; }
            set { pageEnabled = value; }
        }

        public int QuestionCount
        {
            get { return questionCount; }
            
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of Page.
        /// </summary>
        /// <param name="pageGuid"> pageGuid </param>
        private void GetPage(Guid pageGuid)
        {
            using (IDataReader reader = DBSurveyPage.GetOne(pageGuid))
            {
                if (reader.Read())
                {
                    this.pageGuid = new Guid(reader["PageGuid"].ToString());
                    this.surveyGuid = new Guid(reader["SurveyGuid"].ToString());
                    this.pageTitle = reader["PageTitle"].ToString();
                    this.pageOrder = Convert.ToInt32(reader["PageOrder"]);
                    this.pageEnabled = Convert.ToBoolean(reader["PageEnabled"]);
                    this.questionCount = Convert.ToInt32(reader["QuestionCount"]);
                }

            }

        }

        /// <summary>
        /// Persists a new instance of Page. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            
            this.pageGuid = Guid.NewGuid();

            int rowsAffected = DBSurveyPage.Add(
                this.pageGuid,
                this.surveyGuid,
                this.pageTitle,
                this.pageEnabled);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of Page. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBSurveyPage.Update(
                this.pageGuid,
                this.surveyGuid,
                this.pageTitle,
                this.pageOrder,
                this.pageEnabled);

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of Page. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.pageGuid != Guid.Empty)
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
        /// Deletes an instance of Page. Returns true on success.
        /// </summary>
        /// <param name="pageGuid"> pageGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid pageGuid)
        {
            return DBSurveyPage.Delete(pageGuid);
        }

        ///// <summary>
        ///// Returns the count of questions on the given page
        ///// </summary>
        ///// <param name="pageGuid"> pageGuid </param>
        ///// <returns></returns>
        //public static int QuestionCount(Guid pageGuid)
        //{
        //    return DB.PageQuestionsCount(pageGuid);
        //}

        /// <summary>
        /// Gets an IList with all instances of Page.
        /// </summary>
        public static List<Page> GetAll(Guid surveyGuid)
        {
            List<Page> pageList
                = new List<Page>();

            using (IDataReader reader = DBSurveyPage.GetAll(surveyGuid))
            {
                while (reader.Read())
                {
                    Page page = new Page();
                    page.pageGuid = new Guid(reader["PageGuid"].ToString());
                    page.surveyGuid = new Guid(reader["SurveyGuid"].ToString());
                    page.pageTitle = reader["PageTitle"].ToString();
                    page.pageOrder = Convert.ToInt32(reader["PageOrder"], CultureInfo.CurrentCulture);
                    page.pageEnabled = Convert.ToBoolean(reader["PageEnabled"], CultureInfo.CurrentCulture);
                    page.questionCount = Convert.ToInt32(reader["QuestionCount"]);
                    pageList.Add(page);
                }
            }

            return pageList;

        }


        #endregion

    }

}






