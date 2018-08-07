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
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Hosting;

namespace SurveyFeature.Data
{
	public static class DBQuestion
	{
		private static string GetConnectionString()
		{
			string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];

			if (connectionString == "defaultdblocation")
			{
				connectionString = $"version=3,URI=file:{HostingEnvironment.MapPath("~/Data/sqlitedb/mojo.db.config")}";
			}

			return connectionString;
		}


		/// <summary>
		/// Inserts a row in the mp_SurveyQuestions table. Returns rows affected count.
		/// </summary>
		/// <param name="questionGuid"> questionGuid </param>
		/// <param name="pageGuid"> pageGuid </param>
		/// <param name="questionName"> questionName </param>
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
			string validationMessage)
		{
			#region Bit Conversion

			int intAnswerIsRequired;

			if (answerIsRequired)
			{
				intAnswerIsRequired = 1;
			}
			else
			{
				intAnswerIsRequired = 0;
			}

			#endregion

			string sqlCommand = @"
				INSERT INTO
					mp_SurveyQuestions (
						QuestionGuid,
						PageGuid,
						QuestionName,
						QuestionText,
						QuestionTypeId,
						AnswerIsRequired,
						QuestionOrder,
						ValidationMessage
					)
				SELECT
					:QuestionGuid,
					:PageGuid,
					:QuestionName,
					:QuestionText,
					:QuestionTypeId,
					:AnswerIsRequired,
					Count(*),
					:ValidationMessage
				FROM
					mp_SurveyPages;";

			var arParams = new List<SqliteParameter>
			{
				new SqliteParameter(":QuestionGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = questionGuid.ToString()
				},
				new SqliteParameter(":PageGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = pageGuid.ToString()
				},
				new SqliteParameter(":QuestionName", DbType.String, 256)
				{
					Direction = ParameterDirection.Input,
					Value = questionName
				},
				new SqliteParameter(":QuestionText", DbType.Object)
				{
					Direction = ParameterDirection.Input,
					Value = questionText
				},
				new SqliteParameter(":QuestionTypeId", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = questionTypeId
				},
				new SqliteParameter(":AnswerIsRequired", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = intAnswerIsRequired
				},
				new SqliteParameter(":ValidationMessage", DbType.String, 256)
				{
					Direction = ParameterDirection.Input,
					Value = validationMessage
				},
			};

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				sqlCommand,
				arParams.ToArray()
			);

			return rowsAffected;
		}


		/// <summary>
		/// Updates a row in the mp_SurveyQuestions table. Returns true if row updated.
		/// </summary>
		/// <param name="questionGuid"> questionGuid </param>
		/// <param name="pageGuid"> pageGuid </param>
		/// <param name="questionName"> questionName </param>
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
			#region Bit Conversion

			int intAnswerIsRequired;

			if (answerIsRequired)
			{
				intAnswerIsRequired = 1;
			}
			else
			{
				intAnswerIsRequired = 0;
			}

			#endregion

			string sqlCommand = @"
				UPDATE
					mp_SurveyQuestions
				SET
					PageGuid = :PageGuid,
					QuestionName = :QuestionName,
					QuestionText = :QuestionText,
					QuestionTypeId = :QuestionTypeId,
					AnswerIsRequired = :AnswerIsRequired,
					QuestionOrder = :QuestionOrder,
					ValidationMessage = :ValidationMessage
				WHERE
					QuestionGuid = :QuestionGuid;";

			var arParams = new List<SqliteParameter>
			{
				new SqliteParameter(":QuestionGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = questionGuid.ToString()
				},
				new SqliteParameter(":PageGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = pageGuid.ToString()
				},
				new SqliteParameter(":QuestionName", DbType.String, 256)
				{
					Direction = ParameterDirection.Input,
					Value = questionName
				},
				new SqliteParameter(":QuestionText", DbType.Object)
				{
					Direction = ParameterDirection.Input,
					Value = questionText
				},
				new SqliteParameter(":QuestionTypeId", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = questionTypeId
				},
				new SqliteParameter(":AnswerIsRequired", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = intAnswerIsRequired
				},
				new SqliteParameter(":QuestionOrder", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = questionOrder
				},
				new SqliteParameter(":ValidationMessage", DbType.String, 256)
				{
					Direction = ParameterDirection.Input,
					Value = validationMessage
				}
			};

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
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
					mp_SurveyQuestionOptions
				WHERE
					QuestionGuid = :QuestionGuid;
				DELETE FROM
					mp_SurveyQuestions
				WHERE
					QuestionGuid = :QuestionGuid;";

			var arParams = new List<SqliteParameter>
			{
				new SqliteParameter(":QuestionGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = questionGuid.ToString()
				}
			};

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				sqlCommand,
				arParams.ToArray()
			);

			return (rowsAffected > 0);
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
					mp_SurveyQuestions
				WHERE
					QuestionGuid = :QuestionGuid;";

			var arParams = new List<SqliteParameter>
			{
				new SqliteParameter(":QuestionGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = questionGuid.ToString()
				}
			};


			return SqliteHelper.ExecuteReader(
				GetConnectionString(),
				sqlCommand,
				arParams.ToArray()
			);
		}


		/// <summary>
		/// Gets an IDataReader with all rows in the mp_SurveyQuestions table.
		/// </summary>
		/// <param name="pageGuid"> pageGuid </param>
		public static IDataReader GetAllByPage(Guid pageGuid)
		{
			string sqlCommand = @"
				SELECT
					*
				FROM
					mp_SurveyQuestions
				WHERE
					PageGuid = :PageGuid
				ORDER BY
					QuestionOrder;";

			var arParams = new List<SqliteParameter>
			{
				new SqliteParameter(":PageGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = pageGuid.ToString()
				}
			};

			return SqliteHelper.ExecuteReader(
				GetConnectionString(),
				sqlCommand.ToString(),
				arParams.ToArray()
			);
		}
	}
}