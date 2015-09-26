/// Author:					Rob Henry
/// Created:				2007-10-16
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
using System.Globalization;
using SurveyFeature.Data;

namespace SurveyFeature.Business
{
    /// <summary>
    /// Reprsents a response to asurvey
    /// </summary>
    public class SurveyResponse
    {

        #region Constructors

        public SurveyResponse()
        { }


        public SurveyResponse(Guid responseGuid)
        {
            GetSurveyResponse(responseGuid);
        }

        #endregion

        #region Private Properties

        private Guid responseGuid = Guid.Empty;
        private Guid surveyGuid = Guid.Empty;
        private Guid userGuid = Guid.Empty;
        private DateTime submissionDate = DateTime.MinValue;
        private bool annonymous = false;
        private bool complete = false;

        #endregion

        #region Public Properties

        public Guid ResponseGuid
        {
            get { return responseGuid; }
            //set { responseGuid = value; }
        }
        public Guid SurveyGuid
        {
            get { return surveyGuid; }
            set { surveyGuid = value; }
        }
        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }
        public DateTime SubmissionDate
        {
            get { return submissionDate; }
            set { submissionDate = value; }
        }
        public bool Annonymous
        {
            get { return annonymous; }
            set { annonymous = value; }
        }
        public bool Complete
        {
            get { return complete; }
            set { complete = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of SurveyResponse.
        /// </summary>
        /// <param name="responseGuid"> responseGuid </param>
        private void GetSurveyResponse(Guid responseGuid)
        {
            using (IDataReader reader = DBSurveyResponse.GetOne(responseGuid))
            {
                if (reader.Read())
                {
                    this.responseGuid = new Guid(reader["ResponseGuid"].ToString());
                    this.surveyGuid = new Guid(reader["SurveyGuid"].ToString());
                    this.userGuid = new Guid(reader["UserGuid"].ToString());

                    if (reader["SubmissionDate"] != DBNull.Value)
                    {
                        this.submissionDate = Convert.ToDateTime(reader["SubmissionDate"], CultureInfo.InvariantCulture);
                    }

                    this.annonymous = Convert.ToBoolean(reader["Annonymous"]);
                    this.complete = Convert.ToBoolean(reader["Complete"]);

                }

            }

        }

        /// <summary>
        /// Persists a new instance of SurveyResponse. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            
            this.responseGuid = Guid.NewGuid();

            int rowsAffected = DBSurveyResponse.Add(
                this.responseGuid,
                this.surveyGuid,
                this.userGuid,
                this.annonymous,
                this.complete);

            return (rowsAffected > 0);

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of SurveyResponse. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            bool result = false;

            if(responseGuid == Guid.Empty)
                result = Create();

            if (submissionDate > DateTime.MinValue)
            {
                complete = true;
                result = DBSurveyResponse.Update(responseGuid, submissionDate, complete);
            }

            return result;
        }
       
        #endregion

        #region Static Methods

        /// <summary>
        /// Deletes an instance of SurveyResponse. Returns true on success.
        /// </summary>
        /// <param name="responseGuid"> responseGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid responseGuid)
        {
            return DBSurveyResponse.Delete(
                responseGuid);
        }

        /// <summary>
        /// Gets an IList with all instances of SurveyResponse.
        /// </summary>
        public static List<SurveyResponse> GetAll(Guid surveyGuid)
        {
            IDataReader reader = DBSurveyResponse.GetAll(surveyGuid);
            return LoadFromReader(reader);

        }

        private static List<SurveyResponse> LoadFromReader(IDataReader reader)
        {
            List<SurveyResponse> surveyResponseList = new List<SurveyResponse>();
            try
            {
                while (reader.Read())
                {
                    SurveyResponse surveyResponse = new SurveyResponse();
                    surveyResponse.responseGuid = new Guid(reader["ResponseGuid"].ToString());
                    surveyResponse.surveyGuid = new Guid(reader["SurveyGuid"].ToString());

                    surveyResponse.userGuid = new Guid(reader["UserGuid"].ToString());

                    if (reader["SubmissionDate"] != DBNull.Value)
                        surveyResponse.submissionDate = Convert.ToDateTime(reader["SubmissionDate"], CultureInfo.InvariantCulture);

                    surveyResponse.annonymous = Convert.ToBoolean(reader["Annonymous"], CultureInfo.InvariantCulture);
                    surveyResponse.complete = Convert.ToBoolean(reader["Complete"], CultureInfo.InvariantCulture);
                    surveyResponseList.Add(surveyResponse);
                }
            }
            finally
            {
                reader.Close();
            }

            return surveyResponseList;

        }

        private static SurveyResponse FromReader(IDataReader reader)
        {
            SurveyResponse response = null;
            try
            {
                if (reader.Read())
                {
                    response = new SurveyResponse();
                    response.responseGuid = new Guid(reader["ResponseGuid"].ToString());
                    response.surveyGuid = new Guid(reader["SurveyGuid"].ToString());
                    response.userGuid = new Guid(reader["UserGuid"].ToString());

                    if (reader["SubmissionDate"] != DBNull.Value)
                        response.submissionDate = Convert.ToDateTime(reader["SubmissionDate"], CultureInfo.InvariantCulture);

                    response.annonymous = Convert.ToBoolean(reader["Annonymous"], CultureInfo.InvariantCulture);
                    response.complete = Convert.ToBoolean(reader["Complete"], CultureInfo.InvariantCulture);
                }
            }
            finally
            {
                reader.Close();
            }

            return response;

        }




        public static SurveyResponse GetFirst(Guid surveyGuid)
        {
            IDataReader reader = DBSurveyResponse.GetFirst(surveyGuid);
            return FromReader(reader);

        }

        public static SurveyResponse GetNext(Guid responseGuid)
        {

            IDataReader reader = DBSurveyResponse.GetNext(responseGuid);
            return FromReader(reader);

        }

        public static SurveyResponse GetPrevious(Guid responseGuid)
        {

            IDataReader reader = DBSurveyResponse.GetPrevious(responseGuid);
            return FromReader(reader);

            
        }
        #endregion

    }

}






