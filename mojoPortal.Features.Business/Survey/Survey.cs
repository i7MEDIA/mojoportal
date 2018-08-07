// Author:        Rob Henry
// Created:       2007-10-03
// Last Modified: 2018-07-31
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using SurveyFeature.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace SurveyFeature.Business
{
	/// <summary>
	/// Represents a survey
	/// </summary>
	public class Survey
	{
		private const string featureGuid = "263ecff1-f321-4bb8-8d0a-64c0ba89caa7";
		public static Guid FeatureGuid => new Guid(featureGuid);


		#region Constructors

		public Survey() { }
		public Survey(Guid surveyGuid) => GetSurvey(surveyGuid);

		#endregion


		#region Public Properties

		public Guid SurveyGuid { get; set; } = Guid.Empty;
		public Guid SiteGuid { get; set; } = Guid.Empty;
		public string SurveyName { get; set; }
		public int SubmissionLimit { get; set; } = 0;
		public DateTime CreationDate { get; set; }
		public string StartPageText { get; set; }
		public string EndPageText { get; set; }
		public int PageCount { get; private set; } = 0;
		public int ResponseCount { get; private set; } = 0;

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
					SurveyGuid = new Guid(reader["SurveyGuid"].ToString());
					SiteGuid = new Guid(reader["SiteGuid"].ToString());
					SurveyName = reader["SurveyName"].ToString();
					CreationDate = Convert.ToDateTime(reader["CreationDate"], CultureInfo.CurrentCulture);
					StartPageText = reader["StartPageText"].ToString();
					EndPageText = reader["EndPageText"].ToString();
					SubmissionLimit = Convert.ToInt32(reader["SubmissionLimit"]);
					PageCount = Convert.ToInt32(reader["PageCount"]);
					ResponseCount = Convert.ToInt32(reader["ResponseCount"]);
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
			SurveyGuid = newID;

			int rowsAffected = DBSurvey.Add(
				SurveyGuid,
				SiteGuid,
				SurveyName,
				DateTime.Now,
				StartPageText,
				EndPageText,
				SubmissionLimit
			);

			return (rowsAffected > 0);
		}


		/// <summary>
		/// Updates this instance of Survey. Returns true on success.
		/// </summary>
		/// <returns>bool</returns>
		private bool Update()
		{
			return DBSurvey.Update(
				SurveyGuid,
				SiteGuid,
				SurveyName,
				CreationDate,
				StartPageText,
				EndPageText,
				SubmissionLimit
			);
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Saves this instance of Survey. Returns true on success.
		/// </summary>
		/// <returns>bool</returns>
		public bool Save()
		{
			if (SurveyGuid != Guid.Empty)
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
			DBSurvey.AddToModule(SurveyGuid, moduleId);
		}


		public void RemoveFromModule(int moduleId)
		{
			DBSurvey.RemoveFromModule(SurveyGuid, moduleId);
		}

		#endregion


		#region Static Methods

		/// <summary>
		/// Deletes an instance of Survey. Returns true on success.
		/// </summary>
		/// <param name="surveyGuid"> surveyGuid </param>
		/// <returns>bool</returns>
		public static void Delete(Guid surveyGuid)
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


		/// <summary>
		/// Gets an IList with all instances of Survey.
		/// </summary>
		public static List<Survey> GetAll(Guid siteGuid)
		{
			List<Survey> surveyList = new List<Survey>();

			using (IDataReader reader = DBSurvey.GetAll(siteGuid))
			{
				while (reader.Read())
				{
					Survey survey = new Survey
					{
						SurveyGuid = new Guid(reader["SurveyGuid"].ToString()),
						SiteGuid = new Guid(reader["SiteGuid"].ToString()),
						SurveyName = reader["SurveyName"].ToString(),
						CreationDate = Convert.ToDateTime(reader["CreationDate"]),
						StartPageText = reader["StartPageText"].ToString(),
						EndPageText = reader["EndPageText"].ToString(),
						SubmissionLimit = Convert.ToInt32(reader["SubmissionLimit"]),
						PageCount = Convert.ToInt32(reader["PageCount"]),
						ResponseCount = Convert.ToInt32(reader["ResponseCount"])
					};

					surveyList.Add(survey);
				}
			}

			return surveyList;
		}


		public static Guid GetModulesCurrentSurvey(int moduleId)
		{
			return DBSurvey.GetModulesCurrentSurvey(moduleId);
		}


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
					string questionName = reader["QuestionName"].ToString();
					Guid questionGuid = new Guid(reader["QuestionGuid"].ToString());

					result = new Result(questionGuid, responseGuid, answer, pageTitle, questionName);

					results.Add(result);
				}
			}

			return results;
		}

		#endregion
	}
}