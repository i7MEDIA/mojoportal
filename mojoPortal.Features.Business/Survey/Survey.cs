/// Author:					Rob Henry
/// Created:				2007-10-03
/// Last Modified:			2009-06-23
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
using mojoPortal.Business;

namespace SurveyFeature.Business
{
	/// <summary>
	/// Represents a survey
	/// </summary>
	public class Survey
	{

        private const string featureGuid = "263ecff1-f321-4bb8-8d0a-64c0ba89caa7";

        public static Guid FeatureGuid
        {
            get { return new Guid(featureGuid); }
        }

#region Constructors

	    public Survey()
		{}
	    
	
	    public Survey(
			Guid surveyGuid) 
		{
	        GetSurvey(
				surveyGuid); 
	    }

#endregion

#region Private Properties

		private Guid surveyGuid = Guid.Empty;
		private Guid siteGuid = Guid.Empty;
		private string surveyName;
		private DateTime creationDate;
		private string startPageText;
		private string endPageText;
        private int pageCount = 0;
        private int responseCount = 0;

        
		
#endregion

#region Public Properties

		public Guid SurveyGuid 
		{
			get { return surveyGuid; }
			set { surveyGuid = value; }
		}
		public Guid SiteGuid 
		{
			get { return siteGuid; }
			set { siteGuid = value; }
		}
		public string SurveyName 
		{
			get { return surveyName; }
			set { surveyName = value; }
		}
		public DateTime CreationDate 
		{
			get { return creationDate; }
			set { creationDate = value; }
		}
		public string StartPageText 
		{
			get { return startPageText; }
			set { startPageText = value; }
		}
		public string EndPageText 
		{
			get { return endPageText; }
			set { endPageText = value; }
		}

        public int PageCount
        {
            get { return pageCount; }
        }

        public int ResponseCount
        {
            get { return responseCount; }
        }

#endregion

#region Private Methods

		/// <summary>
		/// Gets an instance of Survey.
		/// </summary>
		/// <param name="surveyGuid"> surveyGuid </param>
		private void GetSurvey(Guid guidSurveyGuid) 
		{
            using (IDataReader reader = DBSurvey.GetOne(guidSurveyGuid))
            {
                if (reader.Read())
                {
                    this.surveyGuid = new Guid(reader["SurveyGuid"].ToString());
                    this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    this.surveyName = reader["SurveyName"].ToString();
                    this.creationDate = Convert.ToDateTime(reader["CreationDate"], CultureInfo.CurrentCulture);
                    this.startPageText = reader["StartPageText"].ToString();
                    this.endPageText = reader["EndPageText"].ToString();

                    this.pageCount = Convert.ToInt32(reader["PageCount"]);
                    this.responseCount = Convert.ToInt32(reader["ResponseCount"]);

                }

            }
		
		}
		
		/// <summary>
        /// Persists a new instance of Survey. Returns true on success.
        /// </summary>
        /// <returns></returns>
		private bool Create()
		{ 
			Guid newID = Guid.NewGuid();
			
			this.surveyGuid = newID;

            int rowsAffected = DBSurvey.Add(
				this.surveyGuid, 
				this.siteGuid, 
				this.surveyName, 
				DateTime.Now,
				this.startPageText, 
				this.endPageText); 
				
			return (rowsAffected > 0);

		}

		
		/// <summary>
        /// Updates this instance of Survey. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
		private bool Update()
		{

            return DBSurvey.Update(
				this.surveyGuid, 
				this.siteGuid, 
				this.surveyName, 
				this.creationDate,
				this.startPageText, 
				this.endPageText); 
				
		}

#endregion

#region Public Methods

		/// <summary>
        /// Saves this instance of Survey. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
		public bool Save()
		{
			if(this.surveyGuid != Guid.Empty)
			{
				return Update();
			}
			else
			{
				return Create();
			}
		}


        public void AddToModule(int moduleId)
        {
            DBSurvey.AddToModule(surveyGuid, moduleId);
        }

        public void RemoveFromModule(int moduleId)
        {
            DBSurvey.RemoveFromModule(surveyGuid, moduleId);
        }		
	
		
#endregion

#region Static Methods

	    /// <summary>
        /// Deletes an instance of Survey. Returns true on success.
        /// </summary>
	    /// <param name="surveyGuid"> surveyGuid </param>
        /// <returns>bool</returns>
	    public static void Delete(Guid  surveyGuid) 
	    {
            DBSurvey.Delete(surveyGuid); 
	    }

        public static bool DeleteBySite(int siteId)
        {
            return DBSurvey.DeleteBySite(siteId);
        }

        public static void DeleteFromModule(int moduleId)
        {
            DBSurvey.RemoveFromModule(moduleId);
        }

        ///// <summary>
        ///// Returns the count of pages on the given survey
        ///// </summary>
        ///// <param name="surveyGuid"> pageGuid </param>
        ///// <returns></returns>
        //public static int GetPageCount(Guid surveyGuid)
        //{
        //    return DB.SurveyPagesCount(surveyGuid);
        //}

	    /// <summary>
        /// Gets an IList with all instances of Survey.
        /// </summary>
	    public static List<Survey> GetAll(Guid siteGuid)
        {
            List<Survey> surveyList 
			    = new List<Survey>();

            using (IDataReader reader = DBSurvey.GetAll(siteGuid))
            {
                while (reader.Read())
                {
                    Survey survey = new Survey();
                    survey.surveyGuid = new Guid(reader["SurveyGuid"].ToString());
                    survey.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    survey.surveyName = reader["SurveyName"].ToString();
                    survey.creationDate = Convert.ToDateTime(reader["CreationDate"]);
                    survey.startPageText = reader["StartPageText"].ToString();
                    survey.endPageText = reader["EndPageText"].ToString();
                    survey.pageCount = Convert.ToInt32(reader["PageCount"]);
                    survey.responseCount = Convert.ToInt32(reader["ResponseCount"]);
                    surveyList.Add(survey);
                }
            }

            return surveyList;

        }

        public static Guid GetModulesCurrentSurvey(int moduleId)
        {
            return DBSurvey.GetModulesCurrentSurvey(moduleId);
        }

        //public static int ResponseCount(Guid surveyGuid)
        //{
        //    return DB.SurveyGetResponseCount(surveyGuid);
        //}


        public static DataTable GetResultsTable(Guid surveyGuid)
        {
            IDataReader reader = DBSurvey.GetResults(surveyGuid);
            return DatabaseHelper.GetTableFromDataReader(reader);

        }



        public static List<Result> GetResults(Guid responseGuid)
        {
            List<Result> results = new List<Result>();
            Result result;
            using (IDataReader reader = DBSurvey.GetOneResult(responseGuid))
            {
                while (reader.Read())
                {
                    string pageTitle = reader["PageTitle"].ToString();
                    string answer = reader["Answer"].ToString();
                    string questionText = reader["QuestionText"].ToString();
                    Guid questionGuid = new Guid(reader["QuestionGuid"].ToString());
                    result = new Result(questionGuid, responseGuid, answer, pageTitle, questionText);

                    results.Add(result);
                }

            }
            return results;
        }

        //public static StringBuilder GetResultsCsv(Guid surveyGuid)
        //{
        //    StringBuilder results = new StringBuilder();

        //    List<SurveyResponse> responses = SurveyResponse.GetAll(surveyGuid);

        //    foreach (SurveyResponse response in responses)
        //    {
        //        IDataReader reader = DB.SurveyResultsGetOne(response.ResponseGuid);

        //        while (reader.Read())
        //        {
        //            string tmpResult = reader["Answer"].ToString();
                    
        //            if (!tmpResult.Contains(","))
        //            {
        //                results.Append(tmpResult + ",");
        //            }
        //            else
        //            {
        //                results.Append("\"" + tmpResult + "\","); 
        //            }
        //        }

        //        if (results.Length > 0) results.Remove(results.Length - 1, 1);
        //        results.Append(Environment.NewLine);
        //        reader.Close();
        //    }

        //    return results;
        //}
	
#endregion

}
	
}





