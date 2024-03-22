using mojoPortal.Data;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurveyFeature.Data;

public static class DBSurveyPage
{

	/// <summary>
	/// Inserts a row in the mp_SurveyPages table. Returns rows affected count.
	/// </summary>
	/// <param name="pageGuid"> pageGuid </param>
	/// <param name="surveyGuid"> surveyGuid </param>
	/// <param name="pageTitle"> pageTitle </param>
	/// <param name="pageOrder"> pageOrder </param>
	/// <param name="pageEnabled"> pageEnabled </param>
	/// <returns>int</returns>
	public static int Add(
		Guid pageGuid,
		Guid surveyGuid,
		string pageTitle,
		bool pageEnabled)
	{
		#region Bit Conversion

		int intPageEnabled;

		if (pageEnabled)
		{
			intPageEnabled = 1;
		}
		else
		{
			intPageEnabled = 0;
		}


		#endregion

		string sqlCommand = @"
INSERT INTO mp_SurveyPages (
    PageGuid, 
    SurveyGuid, 
    PageTitle, 
    PageOrder, 
    PageEnabled 
) 
SELECT 
    ?PageGuid, 
    ?SurveyGuid, 
    ?PageTitle, 
    Count(*), ?PageEnabled FROM mp_SurveyPages; ";

		var arParams = new List<MySqlParameter>
		{
			new("?PageGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pageGuid.ToString()
			},

			new("?SurveyGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = surveyGuid.ToString()
			},

			new("?PageTitle", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pageTitle
			},

			new("?PageEnabled", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intPageEnabled
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);
		return rowsAffected;

	}


	/// <summary>
	/// Updates a row in the mp_SurveyPages table. Returns true if row updated.
	/// </summary>
	/// <param name="pageGuid"> pageGuid </param>
	/// <param name="surveyGuid"> surveyGuid </param>
	/// <param name="pageTitle"> pageTitle </param>
	/// <param name="pageOrder"> pageOrder </param>
	/// <param name="pageEnabled"> pageEnabled </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid pageGuid,
		Guid surveyGuid,
		string pageTitle,
		int pageOrder,
		bool pageEnabled)
	{
		#region Bit Conversion

		int intPageEnabled;
		if (pageEnabled)
		{
			intPageEnabled = 1;
		}
		else
		{
			intPageEnabled = 0;
		}


		#endregion

		string sqlCommand = @"
UPDATE 
    mp_SurveyPages 
SET 
    SurveyGuid = ?SurveyGuid, 
    PageTitle = ?PageTitle, 
    PageOrder = ?PageOrder, 
    PageEnabled = ?PageEnabled 
WHERE 
    PageGuid = ?PageGuid; ";

		var arParams = new List<MySqlParameter>
		{
			new("?PageGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pageGuid.ToString()
			},

			new("?SurveyGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = surveyGuid.ToString()
			},

			new("?PageTitle", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pageTitle
			},

			new("?PageOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageOrder
			},

			new("?PageEnabled", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intPageEnabled
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_SurveyPages table. Returns true if row deleted.
	/// </summary>
	/// <param name="pageGuid"> pageGuid </param>
	/// <returns>bool</returns>
	public static bool Delete(
		Guid pageGuid)
	{
		//first delete questionOptions
		string sqlCommand = @"
DELETE FROM mp_SurveyQuestionOptions 
WHERE QuestionGuid 
IN (
    SELECT QuestionGuid 
    FROM mp_SurveyQuestions 
    WHERE 
    PageGuid = ?PageGuid
); ";

		//now delete survey questions
		sqlCommand += @"
DELETE FROM mp_SurveyQuestions 
WHERE PageGuid = ?PageGuid; ";

		//now delete pages
		sqlCommand += @"
DELETE FROM mp_SurveyPages 
WHERE PageGuid = ?PageGuid; ";


		var arParams = new List<MySqlParameter>
		{
			new("?PageGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pageGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);
		return rowsAffected > 0;

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_SurveyPages table.
	/// </summary>
	/// <param name="pageGuid"> pageGuid </param>
	public static IDataReader GetOne(
		Guid pageGuid)
	{
		string sqlCommand = @"
SELECT sp.*, 
    (SELECT COUNT(*) FROM mp_SurveyQuestions sq WHERE sp.PageGuid = sq.PageGuid) AS QuestionCount 
FROM 
    mp_SurveyPages sp 
WHERE 
    sp.PageGuid = ?PageGuid; ";

		var arParams = new List<MySqlParameter>
		{
			new("?PageGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pageGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);

	}


	/// <summary>
	/// Gets an IDataReader with all rows in the mp_SurveyPages table.
	/// </summary>
	public static IDataReader GetAll(Guid surveyGuid)
	{
		string sqlCommand = @"
SELECT 
    sp.*, 
    (SELECT COUNT(*) FROM mp_SurveyQuestions sq WHERE sp.PageGuid = sq.PageGuid) AS QuestionCount 
FROM
    mp_SurveyPages sp 
WHERE 
    sp.SurveyGuid = ?SurveyGuid 
ORDER BY 
    sp.Pageorder ";

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
	/// Gets a count of rows in the mp_SurveyPages table.
	/// </summary>
	public static int GetQuestionsCount(Guid pageGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_SurveyQuestions 
WHERE PageGuid = ?PageGuid; ";

		var arParams = new List<MySqlParameter>
		{
			new("?PageGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pageGuid.ToString()
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams));
	}





}
