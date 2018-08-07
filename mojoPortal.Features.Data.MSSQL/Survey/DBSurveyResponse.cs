// Author:        Rob Henry
// Created:       2007-08-31
// Last Modified: 2008-01-21
// 
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Data;
using System;
using System.Data;

namespace SurveyFeature.Data
{
	public static class DBSurveyResponse
	{
		/// <summary>
		/// Inserts a row in the mp_SurveyResponses table. Returns rows affected count.
		/// </summary>
		/// <param name="responseGuid"> responseGuid </param>
		/// <param name="surveyGuid"> surveyGuid </param>
		/// <param name="userGuid"> userGuid </param>
		/// <param name="annonymous"> annonymous </param>
		/// <param name="complete"> complete </param>
		/// <returns>int</returns>
		public static int Add(
			Guid responseGuid,
			Guid surveyGuid,
			Guid userGuid,
			bool annonymous,
			bool complete
		)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SurveyResponses_Insert", 5);

			sph.DefineSqlParameter("@ResponseGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, responseGuid);
			sph.DefineSqlParameter("@SurveyGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, surveyGuid);
			sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
			sph.DefineSqlParameter("@Annonymous", SqlDbType.Bit, ParameterDirection.Input, annonymous);
			sph.DefineSqlParameter("@Complete", SqlDbType.Bit, ParameterDirection.Input, complete);

			return sph.ExecuteNonQuery();
		}


		/// <summary>
		/// Updates the status of a response. Returns true if row updated.
		/// </summary>
		/// <param name="responseGuid"> responseGuid </param>
		/// <param name="submissionDate"> submissionDate </param>
		/// <param name="complete"> complete </param>
		/// <returns>bool</returns>
		public static bool Update(
			Guid responseGuid,
			DateTime submissionDate,
			bool complete
		)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SurveyResponses_Update", 3);

			sph.DefineSqlParameter("@ResponseGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, responseGuid);
			sph.DefineSqlParameter("@SubmissionDate", SqlDbType.DateTime, ParameterDirection.Input, submissionDate);
			sph.DefineSqlParameter("@Complete", SqlDbType.Bit, ParameterDirection.Input, complete);

			int rowsAffected = sph.ExecuteNonQuery();

			return (rowsAffected > 0);
		}


		/// <summary>
		/// Deletes a row from the mp_SurveyResponses table. Returns true if row deleted.
		/// </summary>
		/// <param name="responseGuid"> responseGuid </param>
		/// <returns>bool</returns>
		public static bool Delete(Guid responseGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SurveyResponses_Delete", 1);

			sph.DefineSqlParameter("@ResponseGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, responseGuid);

			int rowsAffected = sph.ExecuteNonQuery();

			return (rowsAffected > 0);
		}


		/// <summary>
		/// Gets an IDataReader with one row from the mp_SurveyResponses table.
		/// </summary>
		/// <param name="responseGuid"> responseGuid </param>
		public static IDataReader GetOne(Guid responseGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SurveyResponses_SelectOne", 1);

			sph.DefineSqlParameter("@ResponseGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, responseGuid);

			return sph.ExecuteReader();
		}


		/// <summary>
		/// Gets an IDataReader with all rows in the mp_SurveyResponses table.
		/// </summary>
		/// <param name="surveyGuid"> surveyGuid </param>
		public static IDataReader GetAll(Guid surveyGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SurveyResponses_SelectAll", 1);

			sph.DefineSqlParameter("@SurveyGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, surveyGuid);

			return sph.ExecuteReader();
		}


		/// <summary>
		/// Gets an IDataReader with the first response to a survey
		/// </summary>
		/// <param name="surveyGuid"> surveyGuid </param>
		public static IDataReader GetFirst(Guid surveyGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SurveyResponses_GetFirst", 1);

			sph.DefineSqlParameter("@SurveyGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, surveyGuid);

			return sph.ExecuteReader();
		}


		/// <summary>
		/// Gets an IDataReader with the next response to a survey
		/// </summary>
		/// <param name="responseGuid"> responseGuid </param>
		public static IDataReader GetNext(Guid responseGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SurveyResponses_GetNext", 1);

			sph.DefineSqlParameter("@ResponseGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, responseGuid);

			return sph.ExecuteReader();
		}


		/// <summary>
		/// Gets an IDataReader with the next response to a survey
		/// </summary>
		/// <param name="responseGuid"> responseGuid </param>
		public static IDataReader GetPrevious(Guid responseGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SurveyResponses_GetPrevious", 1);

			sph.DefineSqlParameter("@ResponseGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, responseGuid);

			return sph.ExecuteReader();
		}
	}
}