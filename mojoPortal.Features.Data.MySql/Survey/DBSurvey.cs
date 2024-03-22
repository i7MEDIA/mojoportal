using mojoPortal.Data;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurveyFeature.Data;

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
	?SurveyGuid,
	?SiteGuid,
	?SurveyName,
	?CreationDate,
	?StartPageText,
	?EndPageText,
	?SubmissionLimit
);";

		var arParams = new List<MySqlParameter>
		{
			new("?SurveyGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = surveyGuid.ToString()
			},
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},
			new("?SurveyName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = surveyName
			},
			new("?CreationDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = creationDate
			},
			new("?StartPageText", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = startPageText
			},
			new("?EndPageText", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = endPageText
			},
			new("?SubmissionLimit", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = submissionLimit
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
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
	SiteGuid = ?SiteGuid,
	SurveyName = ?SurveyName,
	CreationDate = ?CreationDate,
	StartPageText = ?StartPageText,
	EndPageText = ?EndPageText,
	SubmissionLimit = ?SubmissionLimit
WHERE
	SurveyGuid = ?SurveyGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?SurveyGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = surveyGuid.ToString()
			},
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},
			new("?SurveyName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = surveyName
			},
			new("?CreationDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = creationDate
			},
			new("?StartPageText", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = startPageText
			},
			new("?EndPageText", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = endPageText
			},
			new("?SubmissionLimit", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = submissionLimit
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams.ToArray()
		);

		return rowsAffected > -1;
	}


	/// <summary>
	/// Deletes a row from the mp_Surveys table. Returns true if row deleted.
	/// </summary>
	/// <param name="surveyGuid"> surveyGuid </param>
	/// <returns>bool</returns>
	public static void Delete(Guid surveyGuid)
	{
		//first delete questionOptions
		var sqlCommand = @"
DELETE FROM 
	mp_SurveyQuestionOptions 
WHERE QuestionGuid 
IN (
	SELECT QuestionGuid 
	FROM mp_SurveyQuestions 
	WHERE PageGuid 
	IN (
		SELECT PageGuid 
		FROM mp_SurveyPages 
		WHERE SurveyGuid = ?SurveyGuid
	)
);";

		//now delete survey questions
		sqlCommand += @"
DELETE FROM 
	mp_SurveyQuestions 
WHERE PageGuid 
IN (
	SELECT PageGuid 
	FROM mp_SurveyPages 
	WHERE SurveyGuid = ?SurveyGuid
);";

		//now delete survey pages
		sqlCommand += @"
DELETE FROM mp_SurveyPages 
WHERE SurveyGuid = ?SurveyGuid; ";

		//now delete survey
		sqlCommand += @"
DELETE FROM mp_Surveys 
WHERE SurveyGuid = ?SurveyGuid; ";

		CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			new MySqlParameter("?SurveyGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = surveyGuid.ToString()
			}
			);
	}

	/// <summary>
	/// deletes all survey content for the passed in siteid
	/// </summary>
	/// <param name="siteId"></param>
	/// <returns></returns>
	public static bool DeleteBySite(int siteId)
	{
		//first delete questionOptions
		string sqlCommand = @"
DELETE FROM 
	mp_SurveyQuestionOptions 
WHERE 
	QuestionGuid 
IN (
	SELECT QuestionGuid 
	FROM mp_SurveyQuestions 
	WHERE 
	PageGuid 
	IN (
		SELECT PageGuid 
		FROM mp_SurveyPages 
		WHERE 
		SurveyGuid 
		IN (
			SELECT SurveyGuid 
			FROM mp_Surveys 
			WHERE SiteGuid 
			IN (
				SELECT SiteGuid 
				FROM mp_Sites 
				WHERE SiteID = ?SiteID
			) 
		)
	)
); ";

		//now delete survey questions
		sqlCommand += @"
DELETE FROM 
	mp_SurveyQuestions 
WHERE 
	PageGuid 
	IN (
		SELECT PageGuid 
		FROM mp_SurveyPages 
		WHERE 
		SurveyGuid 
		IN (
			SELECT SurveyGuid 
			FROM mp_Surveys 
			WHERE SiteGuid 
			IN (
				SELECT SiteGuid 
				FROM mp_Sites 
				WHERE SiteID = ?SiteID
			) 
		)
	)
); ";

		//now delete survey pages
		sqlCommand += @"
DELETE FROM mp_SurveyPages 
WHERE 
	SurveyGuid 
	IN (
		SELECT SurveyGuid 
		FROM mp_Surveys 
		WHERE SiteGuid 
		IN (
			SELECT SiteGuid 
			FROM mp_Sites 
			WHERE SiteID = ?SiteID
		) 
	)
)); ";

		//now delete survey
		sqlCommand += @"
DELETE FROM mp_Surveys 
WHERE 
	SiteGuid 
	IN (
		SELECT SiteGuid 
		FROM mp_Sites 
		WHERE SiteID = ?SiteID
	)
;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		return rowsAffected > 0;
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
	(SELECT COUNT(*) FROM mp_SurveyPages sp WHERE sp.SurveyGuid = s.SurveyGuid) AS PageCount, 
	(SELECT COUNT(*) FROM mp_SurveyResponses sr WHERE sr.SurveyGuid = s.SurveyGuid) AS ResponseCount 
FROM mp_Surveys s 
WHERE s.SurveyGuid = ?SurveyGuid; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SurveyGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = surveyGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);

	}


	/// <summary>
	/// Gets a count of rows in the mp_Surveys table.
	/// </summary>
	public static int GetCount()
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Surveys; ";

		return Convert.ToInt32(
			CommandHelper.ExecuteScalar(
				ConnectionString.GetRead(),
				sqlCommand));
	}

	/// <summary>
	/// Gets a count of responses in a survey.
	/// </summary>
	public static int GetResponseCount(Guid surveyGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_SurveyResponses 
WHERE SurveyGuid = ?SurveyGuid 
And Complete = 1; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SurveyGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = surveyGuid.ToString()
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams));

	}

	/// <summary>
	/// Gets an IDataReader with all rows in the mp_Surveys table.
	/// </summary>
	public static IDataReader GetAll(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT 
	s.SurveyGuid, 
	s.SiteGuid, 
	s.SurveyName, 
	s.CreationDate, 
	s.StartPageText, 
	s.EndPageText, 
	s.SubmissionLimit, 
	(SELECT COUNT(*) FROM mp_SurveyPages sp WHERE sp.SurveyGuid = s.SurveyGuid) AS PageCount, 
	(SELECT COUNT(*) FROM mp_SurveyResponses sr WHERE sr.SurveyGuid = s.SurveyGuid) AS ResponseCount 
FROM 
	mp_Surveys s 
WHERE 
	s.SiteGuid = ?SiteGuid 
ORDER BY 
	s.SurveyName ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);
	}

	/// <summary>
	/// Gets the count of pages in a survey
	/// </summary>
	/// <param name="surveyGuid"></param>
	/// <returns></returns>
	public static int PagesCount(Guid surveyGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_SurveyPages 
WHERE SurveyGuid = ?SurveyGuid; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SurveyGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = surveyGuid.ToString()
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams));
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
FROM mp_SurveyModules 
WHERE ModuleId = ?moduleId 
INSERT INTO mp_SurveyModules (SurveyGuid, ModuleId) 
VALUES(?SurveyGuid, ?ModuleId); ";

		var arParams = new List<MySqlParameter>
		{
			new("?SurveyGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = surveyGuid.ToString()
			},

			new("?ModuleId", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);
	}

	public static void RemoveFromModule(Guid surveyGuid, int moduleId)
	{
		string sqlCommand = @"
DELETE 
FROM mp_SurveyModules 
WHERE ModuleId = ?ModuleId 
AND SurveyGuid = ?SurveyGuid; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SurveyGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = surveyGuid.ToString()
			},

			new("?ModuleId", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);
	}

	public static void RemoveFromModule(int moduleId)
	{
		string sqlCommand = @"
DELETE 
FROM mp_SurveyModules 
WHERE ModuleId = ?ModuleId ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleId", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);
	}


	/// <summary>
	/// Gets the Module's current survey
	/// </summary>
	/// <param name="moduleId">moduleId</param>
	/// <returns></returns>
	public static Guid GetModulesCurrentSurvey(int moduleId)
	{
		string sqlCommand = @"
SELECT SurveyGuid
FROM mp_SurveyModules
WHERE moduleId = ?moduleId";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleId", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId.ToString()
			}
		};

		object id = CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams.ToArray()
		);

		if (id == null) return Guid.Empty;

		return new Guid(id.ToString());
	}


	/// <summary>
	/// Gets the guid of the first page in the survey
	/// </summary>
	/// <param name="surveyGuid">surveyGuid</param>
	/// <returns></returns>
	public static Guid GetFirstPageGuid(Guid surveyGuid)
	{
		string sqlCommand = @"
SELECT PageGuid
FROM mp_SurveyPages
WHERE SurveyGuid = ?SurveyGuid
ORDER BY PageOrder
LIMIT 1;";

		var arParams = new List<MySqlParameter>
		{
			new("?SurveyGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = surveyGuid.ToString()
			}
		};

		object id = CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams.ToArray()
		);

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
		string sqlCommand = @"
SELECT
	PageGuid
FROM
	mp_SurveyPages
WHERE
	PageOrder > (
		SELECT PageOrder
		FROM mp_SurveyPages
		WHERE PageGuid = ?PageGuid
	)
AND
SurveyGuid = (
	SELECT SurveyGuid
	FROM mp_SurveyPages
	WHERE PageGuid = ?PageGuid
)
ORDER BY PageOrder 
LIMIT 1;";

		var arParams = new List<MySqlParameter>
		{
			new("?PageGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pageGuid.ToString()
			}
		};

		object id = CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams.ToArray()
		);

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
		string sqlCommand = @"
SELECT
	PageGuid
FROM
	mp_SurveyPages
WHERE
	PageOrder < (
		SELECT PageOrder
		FROM mp_SurveyPages
		WHERE PageGuid = ?PageGuid
	)
AND 
SurveyGuid = (
	SELECT SurveyGuid
	FROM mp_SurveyPages
	WHERE PageGuid = ?PageGuid
) 
ORDER BY PageOrder DESC
LIMIT 1;";

		var arParams = new List<MySqlParameter>
		{
			new("?PageGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pageGuid.ToString()
			}
		};

		object id = CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
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
ON qa.ResponseGuid = sr.ResponseGuid
JOIN
	mp_SurveyQuestions q
ON qa.QuestionGuid = q.QuestionGuid
LEFT OUTER JOIN
	mp_Users u
ON u.UserGuid = sr.UserGuid
WHERE sr.SurveyGuid = ?SurveyGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?SurveyGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = surveyGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams.ToArray()
		);
	}


	/// <summary>
	/// Gets an IDataReader with all rows in the mp_SurveyQuestionAnswers table.
	/// </summary>
	/// <param name="responseGuid"> responseGuid </param>
	public static IDataReader GetOneResult(Guid responseGuid)
	{
		string sqlCommand = @"
SELECT *
FROM mp_Surveys s
JOIN 
	mp_SurveyResponses sr
ON s.SurveyGuid = sr.SurveyGuid
JOIN
	mp_SurveyPages sp
ON sr.SurveyGuid = sp.SurveyGuid
JOIN
	mp_SurveyQuestions sq
ON sp.PageGuid = sq.PageGuid
LEFT JOIN
	mp_SurveyQuestionAnswers qa
ON sq.QuestionGuid = qa.QuestionGuid
AND sr.ResponseGuid = qa.ResponseGuid
WHERE
	sr.ResponseGuid = ?ResponseGuid
AND
	sr.Complete = 1
AND
	sp.PageEnabled = 1
ORDER BY
	sp.PageOrder, sq.Questionorder";

		var arParams = new List<MySqlParameter>
		{
			new("?ResponseGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = responseGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams.ToArray()
		);
	}
}