// Created:       2008-08-29
// Last Modified: 2018-08-01
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
using System.Text;

namespace SurveyFeature.Data
{
	public static class DBSurvey
	{
		private static string GetConnectionString()
		{
			string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];

			if (connectionString == "defaultdblocation")
			{
				connectionString = $"version=3,URI=file:{System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/mojo.db.config")}";
			}

			return connectionString;
		}


		/// <summary>
		/// Inserts a row in the mp_Surveys table. Returns rows affected count.
		/// </summary>
		/// <param name="surveyGuid"> surveyGuid </param>
		/// <param name="siteGuid"> siteGuid </param>
		/// <param name="surveyName"> surveyName </param>
		/// <param name="creationDate"> creationDate </param>
		/// <param name="startPageText"> startPageText </param>
		/// <param name="endPageText"> endPageText </param>
		/// <returns>int</returns>
		public static int Add(
			Guid surveyGuid,
			Guid siteGuid,
			string surveyName,
			DateTime creationDate,
			string startPageText,
			string endPageText,
			int submissionLimit
		)
		{
			string sqlCommand = @"
				INSERT INTO
					mp_Surveys (
						SurveyGuid,
						SiteGuid,
						SurveyName,
						CreationDate,
						StartPageText,
						EndPageText,
						SubmissionLimit
					)
					VALUES (
						:SurveyGuid,
						:SiteGuid,
						:SurveyName,
						:CreationDate,
						:StartPageText,
						:EndPageText,
						:SubmissionLimit
					);";

			var arParams = new List<SqliteParameter>
			{
				new SqliteParameter(":SurveyGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = surveyGuid.ToString()
				},
				new SqliteParameter(":SiteGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				},
				new SqliteParameter(":SurveyName", DbType.String, 255)
				{
					Direction = ParameterDirection.Input,
					Value = surveyName
				},
				new SqliteParameter(":CreationDate", DbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = creationDate
				},
				new SqliteParameter(":StartPageText", DbType.Object)
				{
					Direction = ParameterDirection.Input,
					Value = startPageText
				},
				new SqliteParameter(":EndPageText", DbType.Object)
				{
					Direction = ParameterDirection.Input,
					Value = endPageText
				},
				new SqliteParameter(":SubmissionLimit", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = submissionLimit
				}
			};

			int rowsAffected = 0;

			rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				sqlCommand,
				arParams.ToArray()
			);

			return rowsAffected;
		}


		/// <summary>
		/// Updates a row in the mp_Surveys table. Returns true if row updated.
		/// </summary>
		/// <param name="surveyGuid"> surveyGuid </param>
		/// <param name="siteGuid"> siteGuid </param>
		/// <param name="surveyName"> surveyName </param>
		/// <param name="creationDate"> creationDate </param>
		/// <param name="startPageText"> startPageText </param>
		/// <param name="endPageText"> endPageText </param>
		/// <returns>bool</returns>
		public static bool Update(
			Guid surveyGuid,
			Guid siteGuid,
			string surveyName,
			DateTime creationDate,
			string startPageText,
			string endPageText,
			int submissionLimit
		)
		{
			string sqlCommand = @"
				UPDATE
					mp_Surveys
				SET
					SiteGuid = :SiteGuid,
					SurveyName = :SurveyName,
					CreationDate = :CreationDate,
					StartPageText = :StartPageText,
					EndPageText = :EndPageText,
					SubmissionLimit = :SubmissionLimit
				WHERE 
					SurveyGuid = :SurveyGuid;";

			var arParams = new List<SqliteParameter>
			{
				new SqliteParameter(":SurveyGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = surveyGuid.ToString()
				},
				new SqliteParameter(":SiteGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				},
				new SqliteParameter(":SurveyName", DbType.String, 255)
				{
					Direction = ParameterDirection.Input,
					Value = surveyName
				},
				new SqliteParameter(":CreationDate", DbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = creationDate
				},
				new SqliteParameter(":StartPageText", DbType.Object)
				{
					Direction = ParameterDirection.Input,
					Value = startPageText
				},
				new SqliteParameter(":EndPageText", DbType.Object)
				{
					Direction = ParameterDirection.Input,
					Value = endPageText
				},
				new SqliteParameter(":SubmissionLimit", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = submissionLimit
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
		/// Deletes a row from the mp_Surveys table. Returns true if row deleted.
		/// </summary>
		/// <param name="surveyGuid"> surveyGuid </param>
		/// <returns>bool</returns>
		public static void Delete(Guid surveyGuid)
		{
			//first delete questionOptions
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_SurveyQuestionOptions ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("QuestionGuid IN (");
			sqlCommand.Append("SELECT QuestionGuid ");
			sqlCommand.Append("FROM mp_SurveyQuestions ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("PageGuid IN (");
			sqlCommand.Append("SELECT PageGuid ");
			sqlCommand.Append("FROM mp_SurveyPages ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("SurveyGuid = :SurveyGuid)); ");

			//now delete survey questions
			sqlCommand.Append("DELETE FROM mp_SurveyQuestions ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("PageGuid IN (");
			sqlCommand.Append("SELECT PageGuid ");
			sqlCommand.Append("FROM mp_SurveyPages ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("SurveyGuid = :SurveyGuid);");

			//now delete survey pages
			sqlCommand.Append("DELETE FROM mp_SurveyPages ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("SurveyGuid = :SurveyGuid; ");

			//now delete survey
			sqlCommand.Append("DELETE FROM mp_Surveys ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("SurveyGuid = :SurveyGuid;");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":SurveyGuid", DbType.String, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = surveyGuid.ToString();

			SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		/// <summary>
		/// deletes all survey content for the passed in siteid
		/// </summary>
		/// <param name="siteId"></param>
		/// <returns></returns>
		public static bool DeleteBySite(int siteId)
		{
			//first delete questionOptions
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_SurveyQuestionOptions ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("QuestionGuid IN (");
			sqlCommand.Append("SELECT QuestionGuid ");
			sqlCommand.Append("FROM mp_SurveyQuestions ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("PageGuid IN (");
			sqlCommand.Append("SELECT PageGuid ");
			sqlCommand.Append("FROM mp_SurveyPages ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("SurveyGuid IN (SELECT SurveyGuid FROM mp_Surveys ");
			sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = :SiteID) ");
			sqlCommand.Append("))); ");

			//now delete survey questions
			sqlCommand.Append("DELETE FROM mp_SurveyQuestions ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("PageGuid IN (");
			sqlCommand.Append("SELECT PageGuid ");
			sqlCommand.Append("FROM mp_SurveyPages ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("SurveyGuid IN (SELECT SurveyGuid FROM mp_Surveys ");
			sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = :SiteID) ");
			sqlCommand.Append("))); ");

			//now delete survey pages
			sqlCommand.Append("DELETE FROM mp_SurveyPages ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("SurveyGuid IN (SELECT SurveyGuid FROM mp_Surveys ");
			sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = :SiteID) ");
			sqlCommand.Append("))); ");

			//now delete survey
			sqlCommand.Append("DELETE FROM mp_Surveys ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = :SiteID);");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > 0);
		}

		/// <summary>
		/// Gets an IDataReader with one row from the mp_Surveys table.
		/// </summary>
		/// <param name="surveyGuid"> surveyGuid </param>
		public static IDataReader GetOne(
			Guid surveyGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT  s.*, ");
			sqlCommand.Append("(SELECT COUNT(*) FROM mp_SurveyPages sp WHERE sp.SurveyGuid = s.SurveyGuid) AS PageCount, ");
			sqlCommand.Append("(SELECT COUNT(*) FROM mp_SurveyResponses sr WHERE sr.SurveyGuid = s.SurveyGuid) AS ResponseCount ");
			sqlCommand.Append("FROM	mp_Surveys s ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("s.SurveyGuid = :SurveyGuid; ");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":SurveyGuid", DbType.String, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = surveyGuid.ToString();

			return SqliteHelper.ExecuteReader(
				GetConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		/// <summary>
		/// Gets a count of rows in the mp_Surveys table.
		/// </summary>
		public static int GetCount()
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT  Count(*) ");
			sqlCommand.Append("FROM	mp_Surveys ");
			sqlCommand.Append(";");

			return Convert.ToInt32(SqliteHelper.ExecuteScalar(
				GetConnectionString(),
				sqlCommand.ToString(),
				null));
		}

		/// <summary>
		/// Gets a count of responses in a survey.
		/// </summary>
		public static int GetResponseCount(Guid surveyGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT Count(*) ");
			sqlCommand.Append("FROM	mp_SurveyResponses ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("SurveyGuid = :SurveyGuid ");
			sqlCommand.Append("And Complete = 1; ");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":SurveyGuid", DbType.String, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = surveyGuid.ToString();

			return Convert.ToInt32(SqliteHelper.ExecuteScalar(
			   GetConnectionString(),
			   sqlCommand.ToString(),
			   arParams));
		}

		/// <summary>
		/// Gets an IDataReader with all rows in the mp_Surveys table.
		/// </summary>
		public static IDataReader GetAll(Guid siteGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT ");
			sqlCommand.Append("s.SurveyGuid, ");
			sqlCommand.Append("s.SiteGuid, ");
			sqlCommand.Append("s.SurveyName, ");
			sqlCommand.Append("s.CreationDate, ");
			sqlCommand.Append("s.StartPageText, ");
			sqlCommand.Append("s.EndPageText, ");
			sqlCommand.Append("s.SubmissionLimit, ");
			sqlCommand.Append("(SELECT COUNT(*) FROM mp_SurveyPages sp WHERE sp.SurveyGuid = s.SurveyGuid) AS PageCount, ");
			sqlCommand.Append("(SELECT COUNT(*) FROM mp_SurveyResponses sr WHERE sr.SurveyGuid = s.SurveyGuid) AS ResponseCount ");
			sqlCommand.Append(" ");
			sqlCommand.Append(" ");
			sqlCommand.Append(" ");

			sqlCommand.Append("FROM	mp_Surveys s ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("s.SiteGuid = :SiteGuid ");
			sqlCommand.Append("ORDER BY ");
			sqlCommand.Append("s.SurveyName ");
			//sqlCommand.Append(" ");
			sqlCommand.Append(";");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteGuid.ToString();

			return SqliteHelper.ExecuteReader(
				GetConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		/// <summary>
		/// Gets the count of pages in a survey
		/// </summary>
		/// <param name="surveyGuid"></param>
		/// <returns></returns>
		public static int PagesCount(Guid surveyGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT Count(*) ");
			sqlCommand.Append("FROM	mp_SurveyPages ");
			sqlCommand.Append("WHERE SurveyGuid = :SurveyGuid; ");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":SurveyGuid", DbType.String, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = surveyGuid.ToString();

			return Convert.ToInt32(SqliteHelper.ExecuteScalar(
			   GetConnectionString(),
			   sqlCommand.ToString(),
			   arParams));
		}

		/// <summary>
		/// Set the current survey for a module
		/// </summary>
		/// <param name="surveyGuid"></param>
		/// <param name="moduleId"></param>
		public static void AddToModule(Guid surveyGuid, int moduleId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE ");
			sqlCommand.Append("FROM mp_SurveyModules ");
			sqlCommand.Append("WHERE ModuleId = :ModuleId; ");

			sqlCommand.Append("INSERT INTO mp_SurveyModules (SurveyGuid, ModuleId) ");
			sqlCommand.Append("VALUES(:SurveyGuid, :ModuleId); ");

			SqliteParameter[] arParams = new SqliteParameter[2];

			arParams[0] = new SqliteParameter(":SurveyGuid", DbType.String, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = surveyGuid.ToString();

			arParams[1] = new SqliteParameter(":ModuleId", DbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleId;

			SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);

		}

		public static void RemoveFromModule(Guid surveyGuid, int moduleId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE ");
			sqlCommand.Append("FROM mp_SurveyModules ");
			sqlCommand.Append("WHERE ModuleId = :ModuleId ");
			sqlCommand.Append("AND SurveyGuid = :SurveyGuid; ");

			SqliteParameter[] arParams = new SqliteParameter[2];

			arParams[0] = new SqliteParameter(":SurveyGuid", DbType.String, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = surveyGuid.ToString();

			arParams[1] = new SqliteParameter(":ModuleId", DbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleId;

			SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
		}

		public static void RemoveFromModule(int moduleId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE ");
			sqlCommand.Append("FROM mp_SurveyModules ");
			sqlCommand.Append("WHERE ModuleId = :ModuleId ");
			sqlCommand.Append("; ");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":ModuleId", DbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
		}

		public static Guid GetModulesCurrentSurvey(int moduleId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT SurveyGuid ");
			sqlCommand.Append("FROM mp_SurveyModules ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("moduleId = :ModuleId; ");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":ModuleId", DbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			object id = SqliteHelper.ExecuteScalar(
			   GetConnectionString(),
			   sqlCommand.ToString(),
			   arParams);

			if (id == null) return Guid.Empty;

			return new Guid(id.ToString());
		}

		public static Guid GetFirstPageGuid(Guid surveyGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT PageGuid ");
			sqlCommand.Append("FROM	mp_SurveyPages ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("SurveyGuid = :SurveyGuid ");
			sqlCommand.Append("Order By PageOrder ");
			sqlCommand.Append("Limit 1; ");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":SurveyGuid", DbType.String, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = surveyGuid.ToString();

			object id = SqliteHelper.ExecuteScalar(
			   GetConnectionString(),
			   sqlCommand.ToString(),
			   arParams);

			if (id == null) return Guid.Empty;

			return new Guid(id.ToString());

		}

		/// <summary>
		/// Gets the guid of the next page in the survey
		/// </summary>
		/// <param name="pageGuid">The guid of the current page</param>
		/// <returns></returns>
		public static Guid GetNextPageGuid(Guid pageGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT PageGuid ");
			sqlCommand.Append("FROM	mp_SurveyPages ");
			sqlCommand.Append("WHERE PageOrder > (");
			sqlCommand.Append("SELECT PageOrder ");
			sqlCommand.Append("FROM mp_SurveyPages ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("PageGuid = :PageGuid) ");
			sqlCommand.Append("AND ");
			sqlCommand.Append("SurveyGuid = (");
			sqlCommand.Append("SELECT SurveyGuid ");
			sqlCommand.Append("FROM mp_SurveyPages ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("PageGuid = :PageGuid) ");
			sqlCommand.Append("Order By PageOrder ");
			sqlCommand.Append("Limit 1; ");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":PageGuid", DbType.String, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = pageGuid.ToString();

			object id = SqliteHelper.ExecuteScalar(
			   GetConnectionString(),
			   sqlCommand.ToString(),
			   arParams);

			if (id == null) return Guid.Empty;

			return new Guid(id.ToString());

		}

		/// <summary>
		/// Gets the previous page Guid in the survey
		/// </summary>
		/// <param name="pageGuid">Current page Guid</param>
		/// <returns></returns>
		public static Guid GetPreviousPageGuid(Guid pageGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT PageGuid ");

			sqlCommand.Append("FROM	mp_SurveyPages ");

			sqlCommand.Append("WHERE PageOrder < (");
			sqlCommand.Append("SELECT PageOrder ");
			sqlCommand.Append("FROM mp_SurveyPages ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("PageGuid = :PageGuid) ");

			sqlCommand.Append("AND ");
			sqlCommand.Append("SurveyGuid = (");
			sqlCommand.Append("SELECT SurveyGuid ");
			sqlCommand.Append("FROM mp_SurveyPages ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("PageGuid = :PageGuid) ");
			sqlCommand.Append("Order By PageOrder DESC ");
			sqlCommand.Append("Limit 1; ");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":PageGuid", DbType.String, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = pageGuid.ToString();

			object id = SqliteHelper.ExecuteScalar(
			   GetConnectionString(),
			   sqlCommand.ToString(),
			   arParams);

			if (id == null) return Guid.Empty;

			return new Guid(id.ToString());
		}


		/// <summary>
		/// Gets an IDataReader from the mp_SurveyQuestionAnswers table.
		/// </summary>
		/// <param name="surveyGuid"> surveyGuid </param>
		public static IDataReader GetResults(Guid surveyGuid)
		{
			string sqlCommand = @"
				SELECT
					qa.ResponseGuid AS ResponseSet,
					u.Name AS UserName,
					u.Email AS UserEmail,
					qa.AnsweredDate AS Date,
					q.QuestionName AS Question,
					qa.Answer
				FROM
					mp_SurveyQuestionAnswers qa
				JOIN
					mp_SurveyResponses sr
				ON
					qa.ResponseGuid = sr.ResponseGuid
				JOIN
					mp_SurveyQuestions q
				ON
					qa.QuestionGuid = q.QuestionGuid
				LEFT OUTER JOIN
					mp_Users u
				ON
					u.UserGuid = sr.UserGuid
				WHERE
					sr.SurveyGuid = :SurveyGuid;";

			var arParams = new List<SqliteParameter>
			{
				new SqliteParameter(":SurveyGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = surveyGuid.ToString()
				}
			};

			return SqliteHelper.ExecuteReader(
				GetConnectionString(),
				sqlCommand.ToString(),
				arParams.ToArray()
			);
		}


		/// <summary>
		/// Gets an IDataReader with all rows in the mp_SurveyQuestionAnswers table.
		/// </summary>
		public static IDataReader GetOneResult(Guid responseGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT * ");
			sqlCommand.Append("FROM mp_Surveys s ");

			sqlCommand.Append("JOIN mp_SurveyResponses sr ");
			sqlCommand.Append("ON s.SurveyGuid = sr.SurveyGuid ");

			sqlCommand.Append("JOIN mp_SurveyPages sp ");
			sqlCommand.Append("ON sr.SurveyGuid = sp.SurveyGuid ");

			sqlCommand.Append("JOIN mp_SurveyQuestions sq ");
			sqlCommand.Append("ON sp.PageGuid = sq.PageGuid ");

			sqlCommand.Append("LEFT JOIN mp_SurveyQuestionAnswers qa ");
			sqlCommand.Append("ON sq.QuestionGuid = qa.QuestionGuid ");
			sqlCommand.Append("AND sr.ResponseGuid = qa.ResponseGuid ");

			sqlCommand.Append("WHERE sr.ResponseGuid = :ResponseGuid ");
			sqlCommand.Append("AND sr.Complete = 1 ");
			sqlCommand.Append("AND sp.PageEnabled = 1 ");

			sqlCommand.Append("ORDER BY sp.PageOrder, sq.QuestionOrder; ");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":ResponseGuid", DbType.String, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = responseGuid.ToString();

			return SqliteHelper.ExecuteReader(
				GetConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

	}
}
