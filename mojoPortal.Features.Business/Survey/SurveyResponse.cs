// Author:        Rob Henry
// Created:       2007-10-16
// Last Modified: 2018-07-26
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

		public SurveyResponse() { }
		public SurveyResponse(Guid responseGuid) => GetSurveyResponse(responseGuid);

		#endregion


		#region Public Properties

		public Guid ResponseGuid { get; private set; } = Guid.Empty;
		public Guid SurveyGuid { get; set; } = Guid.Empty;
		public Guid UserGuid { get; set; } = Guid.Empty;
		public DateTime SubmissionDate { get; set; } = DateTime.MinValue;
		public bool Annonymous { get; set; } = false;
		public bool Complete { get; set; } = false;

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
					ResponseGuid = new Guid(reader["ResponseGuid"].ToString());
					SurveyGuid = new Guid(reader["SurveyGuid"].ToString());
					UserGuid = new Guid(reader["UserGuid"].ToString());

					if (reader["SubmissionDate"] != DBNull.Value)
					{
						SubmissionDate = Convert.ToDateTime(reader["SubmissionDate"], CultureInfo.InvariantCulture);
					}

					Annonymous = Convert.ToBoolean(reader["Annonymous"]);
					Complete = Convert.ToBoolean(reader["Complete"]);
				}
			}
		}


		/// <summary>
		/// Persists a new instance of SurveyResponse. Returns true on success.
		/// </summary>
		/// <returns></returns>
		private bool Create()
		{
			ResponseGuid = Guid.NewGuid();

			int rowsAffected = DBSurveyResponse.Add(
				ResponseGuid,
				SurveyGuid,
				UserGuid,
				Annonymous,
				Complete
			);

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

			if (ResponseGuid == Guid.Empty) result = Create();

			if (SubmissionDate > DateTime.MinValue)
			{
				Complete = true;
				result = DBSurveyResponse.Update(ResponseGuid, SubmissionDate, Complete);
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
		public static bool Delete(Guid responseGuid) => DBSurveyResponse.Delete(responseGuid);


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

					surveyResponse.ResponseGuid = new Guid(reader["ResponseGuid"].ToString());
					surveyResponse.SurveyGuid = new Guid(reader["SurveyGuid"].ToString());
					surveyResponse.UserGuid = new Guid(reader["UserGuid"].ToString());

					if (reader["SubmissionDate"] != DBNull.Value)
					{
						surveyResponse.SubmissionDate = Convert.ToDateTime(reader["SubmissionDate"], CultureInfo.InvariantCulture);
					}

					surveyResponse.Annonymous = Convert.ToBoolean(reader["Annonymous"], CultureInfo.InvariantCulture);
					surveyResponse.Complete = Convert.ToBoolean(reader["Complete"], CultureInfo.InvariantCulture);

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

					response.ResponseGuid = new Guid(reader["ResponseGuid"].ToString());
					response.SurveyGuid = new Guid(reader["SurveyGuid"].ToString());
					response.UserGuid = new Guid(reader["UserGuid"].ToString());

					if (reader["SubmissionDate"] != DBNull.Value)
					{
						response.SubmissionDate = Convert.ToDateTime(reader["SubmissionDate"], CultureInfo.InvariantCulture);
					}

					response.Annonymous = Convert.ToBoolean(reader["Annonymous"], CultureInfo.InvariantCulture);
					response.Complete = Convert.ToBoolean(reader["Complete"], CultureInfo.InvariantCulture);
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