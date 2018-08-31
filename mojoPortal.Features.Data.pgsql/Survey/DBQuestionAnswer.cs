// Created:       2008-08-29
// Last Modified: 2012-08-13 
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.


using mojoPortal.Data;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurveyFeature.Data
{
	public static class DBQuestionAnswer
	{
		/// <summary>
		///     Inserts a row in the mp_SurveyQuestionAnswers table. Returns rows affected count.
		/// </summary>
		/// <param name="answerGuid"> answerGuid </param>
		/// <param name="questionGuid"> questionGuid </param>
		/// <param name="responseGuid"> responseGuid </param>
		/// <param name="answer"> answer </param>
		/// <returns>int</returns>
		public static int Add(
			Guid answerGuid,
			Guid questionGuid,
			Guid responseGuid,
			string answer
		)
		{
			const string sqlCommand = @"
				INSERT INTO
					mp_surveyquestionanswers (
						answerguid,
						questionguid,
						responseguid,
						answer,
						answereddate
					)
					VALUES (
						:answerguid,
						:questionguid,
						:responseguid,
						:answer,
						:answereddate
					);";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("answerguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = answerGuid.ToString()
				},
				new NpgsqlParameter("questionguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = questionGuid.ToString()
				},
				new NpgsqlParameter("responseguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = responseGuid.ToString()
				},
				new NpgsqlParameter("answer", NpgsqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = answer
				},
				new NpgsqlParameter("answereddate", NpgsqlDbType.Timestamp)
				{
					Direction = ParameterDirection.Input,
					Value = DateTime.UtcNow
				}
			};

			var rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams.ToArray()
			);

			return rowsAffected;
		}


		/// <summary>
		///     Updates a row in the mp_SurveyQuestionAnswers table. Returns true if row updated.
		/// </summary>
		/// <param name="answerGuid"> answerGuid </param>
		/// <param name="questionGuid"> questionGuid </param>
		/// <param name="responseGuid"> responseGuid </param>
		/// <param name="answer"> answer </param>
		/// <returns>bool</returns>
		public static bool Update(
			Guid answerGuid,
			Guid questionGuid,
			Guid responseGuid,
			string answer
		)
		{
			const string sqlCommand = @"
				UPDATE
					mp_surveyquestionanswers
				SET 
					questionguid = :questionguid,
					responseguid = :responseguid,
					answer = :answer
				WHERE
					answerguid = :answerguid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("answerguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = answerGuid.ToString()
				},
				new NpgsqlParameter("questionguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = questionGuid.ToString()
				},
				new NpgsqlParameter("responseguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = responseGuid.ToString()
				},
				new NpgsqlParameter("answer", NpgsqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = answer
				}
			};

			var rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams.ToArray()
			);

			return rowsAffected > -1;
		}


		/// <summary>
		///     Gets an IDataReader with one row from the mp_SurveyQuestionAnswers table.
		/// </summary>
		/// <param name="responseGuid"></param>
		/// <param name="questionGuid"></param>
		public static IDataReader GetOne(Guid responseGuid, Guid questionGuid)
		{
			const string sqlCommand = @"
				SELECT
					*
				FROM
					mp_surveyquestionanswers 
				WHERE
					questionguid = :questionguid
				AND
					responseguid = :responseguid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("questionguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = questionGuid.ToString()
				},
				new NpgsqlParameter("responseguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = responseGuid.ToString()
				}
			};

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams.ToArray()
			);
		}
	}
}