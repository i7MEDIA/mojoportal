// Created:       2008-08-29
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
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurveyFeature.Data
{
	public static class DBQuestion
	{
		/// <summary>
		/// Inserts a row in the mp_SurveyQuestions table. Returns rows affected count.
		/// </summary>
		/// <param name="questionGuid"> questionGuid </param>
		/// <param name="pageGuid"> pageGuid </param>
		/// <param name="questionName"> questionText </param>
		/// <param name="questionText"> questionText </param>
		/// <param name="questionTypeId"> questionTypeId </param>
		/// <param name="answerIsRequired"> answerIsRequired </param>
		/// <param name="questionOrder"> questionOrder </param>
		/// <param name="validationMessage"> validationMessage </param>
		/// <returns>int</returns>
		public static int Add(
			Guid questionGuid,
			Guid pageGuid,
			string questionName,
			string questionText,
			int questionTypeId,
			bool answerIsRequired,
			string validationMessage
		)
		{
			string sqlCommand = @"
				INSERT INTO
					mp_surveyquestions (
						questionguid, 
						pageguid, 
						questionname, 
						questiontext, 
						questiontypeid, 
						answerisrequired, 
						questionorder, 
						validationmessage
					)
				SELECT 
					:questionguid, 
					:pageguid, 
					:questionname, 
					:questiontext, 
					:questiontypeid, 
					:answerisrequired, 
					Count(*), 
					:validationmessage 
				FROM
					mp_surveypages;";

			var sqlParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("questionguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = questionGuid.ToString()
				},
				new NpgsqlParameter("pageguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = pageGuid.ToString()
				},
				new NpgsqlParameter("questionname", NpgsqlTypes.NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = questionName
				},
				new NpgsqlParameter("questiontext", NpgsqlTypes.NpgsqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = questionText
				},
				new NpgsqlParameter("questiontypeid", NpgsqlTypes.NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = questionTypeId
				},
				new NpgsqlParameter("answerisrequired", NpgsqlTypes.NpgsqlDbType.Boolean)
				{
					Direction = ParameterDirection.Input,
					Value = answerIsRequired
				},
				new NpgsqlParameter("validationmessage", NpgsqlTypes.NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = validationMessage
				},

			};

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand,
				sqlParams.ToArray()
			);

			return rowsAffected;
		}


		/// <summary>
		/// Updates a row in the mp_SurveyQuestions table. Returns true if row updated.
		/// </summary>
		/// <param name="questionGuid"> questionGuid </param>
		/// <param name="pageGuid"> pageGuid </param>
		/// <param name="questionName"> questionText </param>
		/// <param name="questionText"> questionText </param>
		/// <param name="questionTypeId"> questionTypeId </param>
		/// <param name="answerIsRequired"> answerIsRequired </param>
		/// <param name="questionOrder"> questionOrder </param>
		/// <param name="validationMessage"> validationMessage </param>
		/// <returns>bool</returns>
		public static bool Update(
			Guid questionGuid,
			Guid pageGuid,
			string questionName,
			string questionText,
			int questionTypeId,
			bool answerIsRequired,
			int questionOrder,
			string validationMessage
		)
		{
			string sqlCommand = @"
				UPDATE
					mp_surveyquestions
				SET
					pageguid = :pageguid,
					questionname = :questionname,
					questiontext = :questiontext,
					questiontypeid = :questiontypeid,
					answerisrequired = :answerisrequired,
					questionorder = :questionorder,
					validationmessage = :validationmessage
				WHERE
					questionguid = :questionguid;";

			var arParams = new List<NpgsqlParameter> {
				new NpgsqlParameter("questionguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = questionGuid.ToString()
				},
				new NpgsqlParameter("pageguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = pageGuid.ToString()
				},
				new NpgsqlParameter("questionname", NpgsqlTypes.NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = questionName
				},
				new NpgsqlParameter("questiontext", NpgsqlTypes.NpgsqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = questionText
				},
				new NpgsqlParameter("questiontypeid", NpgsqlTypes.NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = questionTypeId
				},
				new NpgsqlParameter("answerisrequired", NpgsqlTypes.NpgsqlDbType.Boolean)
				{
					Direction = ParameterDirection.Input,
					Value = answerIsRequired
				},
				new NpgsqlParameter("questionorder", NpgsqlTypes.NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = questionOrder
				},
				new NpgsqlParameter("validationmessage", NpgsqlTypes.NpgsqlDbType.Varchar, 256)
				{
					Direction = ParameterDirection.Input,
					Value = validationMessage
				}
			};

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams.ToArray()
			);

			return (rowsAffected > -1);
		}


		/// <summary>
		/// Deletes a row from the mp_SurveyQuestions table. Returns true if row deleted.
		/// </summary>
		/// <param name="questionGuid"> questionGuid </param>
		/// <returns>bool</returns>
		public static bool Delete(Guid questionGuid)
		{
			string sqlCommand = @"
				DELETE FROM
					mp_surveyquestions
				WHERE
					questionguid = :questionguid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("questionguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = questionGuid.ToString()
				}
			};

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams.ToArray()
			);

			return (rowsAffected > -1);
		}


		/// <summary>
		/// Gets an IDataReader with one row from the mp_SurveyQuestions table.
		/// </summary>
		/// <param name="questionGuid"> questionGuid </param>
		public static IDataReader GetOne(Guid questionGuid)
		{
			string sqlCommand = @"
				SELECT
					*
				FROM
					mp_surveyquestions
				WHERE
					questionguid = :questionguid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("questionguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = questionGuid.ToString()
				}
			};

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams.ToArray()
			);
		}


		/// <summary>
		/// Gets an IDataReader with all rows in the mp_SurveyQuestions table.
		/// </summary>
		public static IDataReader GetAllByPage(Guid pageGuid)
		{
			string sqlCommand = @"
				SELECT
					*
				FROM
					mp_surveyquestions
				WHERE
					pageguid = :pageguid
				ORDER BY
					questionorder;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("pageguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = pageGuid.ToString()
				}
			};

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams.ToArray()
			);
		}
	}
}
