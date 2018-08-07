// Author:        Rob Henry
// Created:       2007-08-31
// Last Modified: 2018-07-31
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
	public static class DBQuestion
	{
		public static int Add(
			Guid questionGuid,
			Guid surveyPageGuid,
			string questionName,
			string questionText,
			int questionTypeId,
			bool answerIsRequired,
			string validationMessage
		)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SurveyQuestions_Insert", 7);

			sph.DefineSqlParameter("@QuestionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, questionGuid);
			sph.DefineSqlParameter("@PageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, surveyPageGuid);
			sph.DefineSqlParameter("@QuestionName", SqlDbType.NVarChar, 255, ParameterDirection.Input, questionName);
			sph.DefineSqlParameter("@QuestionText", SqlDbType.NVarChar, -1, ParameterDirection.Input, questionText);
			sph.DefineSqlParameter("@QuestionTypeId", SqlDbType.Int, ParameterDirection.Input, questionTypeId);
			sph.DefineSqlParameter("@AnswerIsRequired", SqlDbType.Bit, ParameterDirection.Input, answerIsRequired);
			sph.DefineSqlParameter("@ValidationMessage", SqlDbType.NVarChar, 255, ParameterDirection.Input, validationMessage);

			int rowsAffected = sph.ExecuteNonQuery();

			return rowsAffected;
		}


		public static bool Update(
			Guid questionGuid,
			Guid surveyPageGuid,
			string questionName,
			string questionText,
			int questionTypeId,
			bool answerIsRequired,
			int questionOrder,
			string validationMessage
		)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SurveyQuestions_Update", 8);

			sph.DefineSqlParameter("@QuestionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, questionGuid);
			sph.DefineSqlParameter("@PageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, surveyPageGuid);
			sph.DefineSqlParameter("@QuestionName", SqlDbType.NVarChar, 255, ParameterDirection.Input, questionName);
			sph.DefineSqlParameter("@QuestionText", SqlDbType.NVarChar, -1, ParameterDirection.Input, questionText);
			sph.DefineSqlParameter("@QuestionTypeId", SqlDbType.Int, ParameterDirection.Input, questionTypeId);
			sph.DefineSqlParameter("@AnswerIsRequired", SqlDbType.Bit, ParameterDirection.Input, answerIsRequired);
			sph.DefineSqlParameter("@QuestionOrder", SqlDbType.Int, ParameterDirection.Input, questionOrder);
			sph.DefineSqlParameter("@ValidationMessage", SqlDbType.NVarChar, 255, ParameterDirection.Input, validationMessage);

			int rowsAffected = sph.ExecuteNonQuery();

			return (rowsAffected > -1);
		}


		public static bool Delete(Guid questionGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SurveyQuestions_Delete", 1);

			sph.DefineSqlParameter("@QuestionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, questionGuid);

			int rowsAffected = sph.ExecuteNonQuery();

			return (rowsAffected > -1);
		}


		public static IDataReader GetOne(Guid questionGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SurveyQuestions_SelectOne", 1);

			sph.DefineSqlParameter("@QuestionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, questionGuid);

			return sph.ExecuteReader();
		}


		public static int GetCount()
		{
			return Convert.ToInt32(
				SqlHelper.ExecuteScalar(
					ConnectionString.GetReadConnectionString(),
					CommandType.StoredProcedure,
					"mp_SurveyQuestions_GetCount",
					null
				)
			);
		}


		public static IDataReader GetAll()
		{

			return SqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.StoredProcedure,
				"mp_SurveyQuestions_SelectAll",
				null
			);
		}


		public static IDataReader GetAllByPage(Guid pageQuestionGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SurveyQuestions_SelectAllByPage", 1);

			sph.DefineSqlParameter("@PageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pageQuestionGuid);

			return sph.ExecuteReader();
		}
	}
}