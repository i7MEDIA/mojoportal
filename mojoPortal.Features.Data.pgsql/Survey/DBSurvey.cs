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
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurveyFeature.Data
{
	public static class DBSurvey
	{
		/// <summary>
		/// Inserts a row in the mp_Surveys table. Returns rows affected count.
		/// </summary>
		/// <param name="surveyGuid"> surveyGuid </param>
		/// <param name="siteGuid"> siteGuid </param>
		/// <param name="surveyName"> surveyName </param>
		/// <param name="creationDate"> creationDate </param>
		/// <param name="startPageText"> startPageText </param>
		/// <param name="endPageText"> endPageText </param>
		/// <param name="submissionLimit"> submissionLimit </param>
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
					mp_surveys (
						surveyguid,
						siteguid,
						surveyname,
						creationdate,
						startpagetext,
						endpagetext,
						submissionlimit
					)
					VALUES (
						:surveyguid,
						:siteguid,
						:surveyname,
						:creationdate,
						:startpagetext,
						:endpagetext,
						:submissionlimit
					);";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = surveyGuid.ToString()
				},
				new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				},
				new NpgsqlParameter("surveyname", NpgsqlTypes.NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = surveyName
				},
				new NpgsqlParameter("creationdate", NpgsqlTypes.NpgsqlDbType.Timestamp)
				{
					Direction = ParameterDirection.Input,
					Value = creationDate
				},
				new NpgsqlParameter("startpagetext", NpgsqlTypes.NpgsqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = startPageText
				},
				new NpgsqlParameter("endpagetext", NpgsqlTypes.NpgsqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = endPageText
				},
				new NpgsqlParameter("submissionlimit", NpgsqlTypes.NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = submissionLimit
				}
			};

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
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
		/// <param name="submissionLimit"> submissionLimit </param>
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
					mp_surveys
				SET 
					siteguid = :siteguid,
					surveyname = :surveyname,
					creationdate = :creationdate,
					startpagetext = :startpagetext,
					endpagetext = :endpagetext,
					submissionlimit = :submissionlimit
				WHERE
					surveyguid = :surveyguid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = surveyGuid.ToString()
				},
				new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				},
				new NpgsqlParameter("surveyname", NpgsqlTypes.NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = surveyName
				},
				new NpgsqlParameter("creationdate", NpgsqlTypes.NpgsqlDbType.Timestamp)
				{
					Direction = ParameterDirection.Input,
					Value = creationDate
				},
				new NpgsqlParameter("startpagetext", NpgsqlTypes.NpgsqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = startPageText
				},
				new NpgsqlParameter("endpagetext", NpgsqlTypes.NpgsqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = endPageText
				},
				new NpgsqlParameter("submissionlimit", NpgsqlTypes.NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = submissionLimit
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
		/// Deletes a row from the mp_Surveys table. Returns true if row deleted.
		/// </summary>
		/// <param name="surveyGuid"> surveyGuid </param>
		/// <returns>bool</returns>
		public static void Delete(Guid surveyGuid)
		{
			string sqlCommand = @"
				DELETE FROM
					mp_surveyquestionoptions
				WHERE
					questionguid IN (
						SELECT
							questionguid
						FROM
							mp_surveyquestions
						WHERE
							pageguid
						IN (
							SELECT
								pageguid
							FROM
								mp_surveypages
							WHERE
								surveyguid = :surveyguid
						)
					);


				DELETE FROM
					mp_surveyquestions
				WHERE
					pageguid IN (
						SELECT
							pageguid
						FROM
							mp_surveypages
						WHERE
							surveyguid = :surveyguid
					);


				DELETE FROM
					mp_surveypages
				WHERE
					surveyguid = :surveyguid;


				DELETE FROM
					mp_surveys
				WHERE
					surveyguid = :surveyguid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = surveyGuid.ToString()
				}
			};

			NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams.ToArray()
			);
		}

		/// <summary>
		/// deletes all survey content for the passed in siteid
		/// </summary>
		/// <param name="siteId"></param>
		/// <returns></returns>
		public static bool DeleteBySite(int siteId)
		{
			string sqlCommand = @"
				DELETE FROM
					mp_surveyquestionoptions
				WHERE
					questionguid IN (
						SELECT
							questionguid
						FROM
							mp_surveyquestions
						WHERE
							pageguid IN (
								SELECT
									pageguid
								FROM
									mp_surveypages
								WHERE
									surveyguid IN (
										SELECT
											surveyguid
										FROM
											mp_surveys
										WHERE
											siteguid IN (
												SELECT
													siteguid
												FROM
													mp_sites
												WHERE
													siteid = :siteid
											)
									)
							)
					);


				DELETE FROM
					mp_surveyquestions
				WHERE
					pageguid IN (
						SELECT
							pageguid
						FROM
							mp_surveypages
						WHERE
							surveyguid IN (
								SELECT
									surveyguid
								FROM
									mp_surveys
								WHERE
									siteguid IN (
										SELECT
											siteguid
										FROM
											mp_sites
										WHERE
										siteid = :siteid
									)
							)
					);


					DELETE FROM
						mp_surveypages
					WHERE
						surveyguid IN (
							SELECT
								surveyguid
							FROM
								mp_surveys
							WHERE
								siteguid IN (
									SELECT
										siteguid
									FROM
										mp_sites
									WHERE
										siteid = :siteid
								)
						);


				DELETE FROM
					mp_surveys
				WHERE
					siteguid IN (
						SELECT
							siteguid
						FROM
							mp_sites
						WHERE
							siteid = :siteid
					);";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = siteId
				}
			};

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams.ToArray()
			);

			return (rowsAffected > 0);
		}


		/// <summary>
		/// Gets an IDataReader with one row from the mp_Surveys table.
		/// </summary>
		/// <param name="surveyGuid"> surveyGuid </param>
		public static IDataReader GetOne(
			Guid surveyGuid)
		{
			string sqlCommand = @"
				SELECT
					s.*,
					(
						SELECT
							COUNT(*)
						FROM
							mp_surveypages sp
						WHERE
							sp.surveyguid = s.surveyguid
					)
					AS
						pagecount,
					(
						SELECT
							COUNT(*)
						FROM
							mp_surveyresponses sr
						WHERE
							sr.surveyguid = s.surveyguid
					)
					AS
						responsecount
				FROM
					mp_surveys s
				WHERE
					s.surveyguid = :surveyguid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = surveyGuid.ToString()
				}
			};

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams.ToArray()
			);
		}


		/// <summary>
		/// Gets a count of rows in the mp_Surveys table.
		/// </summary>
		public static int GetCount()
		{
			string sqlCommand = @"
				SELECT
					Count(*)
				FROM
					mp_surveys;";

			return Convert.ToInt32(
				NpgsqlHelper.ExecuteScalar(
					ConnectionString.GetReadConnectionString(),
					CommandType.Text,
					sqlCommand,
					null
				)
			);
		}


		/// <summary>
		/// Gets a count of responses in a survey.
		/// </summary>
		public static int GetResponseCount(Guid surveyGuid)
		{
			string sqlCommand = @"
				SELECT
					Count(*)
				FROM
					mp_surveyresponses
				WHERE
					surveyguid = :surveyguid
				AND
					complete = true;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = surveyGuid.ToString()
				}
			};

			return Convert.ToInt32(
				NpgsqlHelper.ExecuteScalar(
					ConnectionString.GetReadConnectionString(),
					CommandType.Text,
					sqlCommand,
					arParams.ToArray()
				)
			);
		}


		/// <summary>
		/// Gets an IDataReader with all rows in the mp_Surveys table.
		/// </summary>
		public static IDataReader GetAll(Guid siteGuid)
		{
			string sqlCommand = @"
				SELECT
					s.surveyguid,
					s.siteguid,
					s.surveyname,
					s.creationdate,
					s.startpagetext,
					s.endpagetext,
					s.submissionlimit,
					(
						SELECT
							COUNT(*)
						FROM
							mp_surveypages sp
						WHERE
							sp.surveyguid = s.surveyguid
					)
					AS
						pagecount,
					(
						SELECT
							COUNT(*)
						FROM
							mp_surveyresponses sr
						WHERE
							sr.surveyguid = s.surveyguid
					)
					AS
						responsecount
				FROM
					mp_surveys s
				WHERE
					s.siteguid = :siteguid
				ORDER BY
					s.surveyname;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				}
			};

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams.ToArray()
			);
		}


		/// <summary>
		/// Gets the count of pages in a survey
		/// </summary>
		/// <param name="surveyGuid"></param>
		/// <returns></returns>
		public static int PagesCount(Guid surveyGuid)
		{
			string sqlCommand = @"
				SELECT
					Count(*)
				FROM
					mp_surveypages
				WHERE
					surveyguid = :surveyguid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = surveyGuid.ToString()
				}
			};

			return Convert.ToInt32(
				NpgsqlHelper.ExecuteScalar(
					ConnectionString.GetReadConnectionString(),
					CommandType.Text,
					sqlCommand,
					arParams.ToArray()
				)
			);
		}


		/// <summary>
		/// Set the current survey for a module
		/// </summary>
		/// <param name="surveyGuid"></param>
		/// <param name="moduleId"></param>
		public static void AddToModule(Guid surveyGuid, int moduleId)
		{
			string sqlCommand = @"
				DELETE
				FROM
					mp_surveymodules
				WHERE
					moduleid = :moduleid;
				INSERT INTO
					mp_surveymodules (
						surveyguid,
						moduleid
					)
					VALUES (
						:surveyguid,
						:moduleid
					);";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = surveyGuid.ToString()
				},
				new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				}
			};

			NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams.ToArray()
			);
		}


		/// <summary>
		/// Remove one Survey from Module
		/// </summary>
		/// <param name="surveyGuid"></param>
		/// <param name="moduleId"></param>
		public static void RemoveFromModule(Guid surveyGuid, int moduleId)
		{
			string sqlCommand = @"
				DELETE FROM
					mp_surveymodules
				WHERE
					moduleid = :moduleid
				AND
					surveyguid = :surveyguid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = surveyGuid.ToString()
				},
				new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				}
			};

			NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams.ToArray()
			);
		}


		/// <summary>
		/// Remove all surveys from Module
		/// </summary>
		/// <param name="moduleId">The current Module's ID</param>
		public static void RemoveFromModule(int moduleId)
		{
			string sqlCommand = @"
				DELETE FROM
					mp_surveymodules
				WHERE
					moduleid = :moduleid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				}
			};

			NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams.ToArray()
			);
		}


		/// <summary>
		/// Get the Module's current survey
		/// </summary>
		/// <param name="moduleId">The current Module's ID</param>
		/// <returns></returns>
		public static Guid GetModulesCurrentSurvey(int moduleId)
		{
			string sqlCommand = @"
				SELECT
					surveyguid
				FROM
					mp_surveymodules
				WHERE
					moduleid = :moduleid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				}
			};

			object id = NpgsqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams.ToArray()
			);

			if (id == null) return Guid.Empty;

			return new Guid(id.ToString());
		}


		/// <summary>
		/// Get the survey's first page GUID
		/// </summary>
		/// <param name="surveyGuid">The GUID of the current Survey</param>
		/// <returns></returns>
		public static Guid GetFirstPageGuid(Guid surveyGuid)
		{
			string sqlCommand = @"
				SELECT
					pageguid
				FROM
					mp_surveypages
				WHERE
					surveyguid = :surveyguid
				ORDER BY
					pageorder
				LIMIT
					1;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = surveyGuid.ToString()
				}
			};

			object id = NpgsqlHelper.ExecuteScalar(ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams.ToArray()
			);

			if (id == null) return Guid.Empty;

			return new Guid(id.ToString());
		}


		/// <summary>
		/// Gets the guid of the next page in the survey
		/// </summary>
		/// <param name="pageGuid">The GUID of the current page</param>
		/// <returns></returns>
		public static Guid GetNextPageGuid(Guid pageGuid)
		{
			string sqlCommand = @"
				SELECT
					pageguid
				FROM
					mp_surveypages
				WHERE
					pageorder > (
						SELECT
							pageorder
						FROM
							mp_surveypages
						WHERE
							pageguid = :pageguid
					)
				AND 
					surveyguid = (
						SELECT
							surveyguid
						FROM
							mp_surveypages
						WHERE
							pageguid = :pageguid
					)
				Order BY
					pageorder 
				LIMIT
					1;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("pageguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = pageGuid.ToString()
				}
			};

			object id = NpgsqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams.ToArray()
			);

			if (id == null) return Guid.Empty;

			return new Guid(id.ToString());
		}


		/// <summary>
		/// Gets the previous page Guid in the survey
		/// </summary>
		/// <param name="pageGuid">Current page GUID</param>
		/// <returns></returns>
		public static Guid GetPreviousPageGuid(Guid pageGuid)
		{
			string sqlCommand = @"
				SELECT
					pageguid
				FROM
					mp_surveypages
				WHERE
					pageorder < (
						SELECT
							pageorder
						FROM
							mp_surveypages
						WHERE
							pageguid = :pageguid
					)
				AND 
					surveyguid = (
						SELECT
							surveyguid
						FROM
							mp_surveypages
						WHERE
							pageguid = :pageguid
					)
				ORDER BY
					pageorder DESC
				LIMIT
					1;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("pageguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = pageGuid.ToString()
				}
			};

			object id = NpgsqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams.ToArray()
			);

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
					qa.responseguid AS responseset,
					u.name AS username,
					u.email AS useremail,
					qa.answereddate AS date,
					q.questionname AS question,
					qa.answer
				FROM
					mp_SurveyQuestionAnswers qa
				JOIN
					mp_SurveyResponses sr
				ON
					qa.responseguid = sr.responseguid
				JOIN
					mp_SurveyQuestions q
				ON
					qa.questionguid = q.questionguid
				LEFT OUTER JOIN
					mp_Users u
				ON
					u.userguid = sr.userguid
				WHERE
					sr.surveyguid = :surveyguid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = surveyGuid.ToString()
				}
			};

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams.ToArray()
			);
		}


		/// <summary>
		/// Gets an IDataReader with all rows in the mp_SurveyQuestionAnswers table.
		/// </summary>
		public static IDataReader GetOneResult(Guid responseGuid)
		{
			string sqlCommand = @"
				SELECT
					*
				FROM
					mp_surveys s
				JOIN
					mp_surveyresponses sr
				ON
					s.surveyguid = sr.SurveyGuid
				JOIN
					mp_surveypages sp
				ON
					sr.surveyguid = sp.SurveyGuid
				JOIN
					mp_surveyquestions sq
				ON
					sp.pageguid = sq.pageguid
				LEFT JOIN
					mp_SurveyQuestionAnswers qa
				ON
					sq.questionguid = qa.questionguid
				AND
					sr.responseguid = qa.responseguid
				WHERE
					sr.responseguid = :responseguid
				AND
					sr.complete = true
				AND
					sp.pageenabled = true
				ORDER BY
					sp.pageorder, sq.questionorder;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("responseguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
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