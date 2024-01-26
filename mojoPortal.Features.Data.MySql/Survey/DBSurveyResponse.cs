using mojoPortal.Data;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurveyFeature.Data;

public static class DBSurveyResponse
{


	/// <summary>
	/// Inserts a row in the mp_SurveyResponses table. Returns rows affected count.
	/// </summary>
	/// <param name="responseGuid"> responseGuid </param>
	/// <param name="surveyGuid"> surveyGuid </param>
	/// <param name="userId"> userId </param>
	/// <param name="annonymous"> annonymous </param>
	/// <param name="complete"> complete </param>
	/// <returns>int</returns>
	public static int Add(
		Guid responseGuid,
		Guid surveyGuid,
		Guid userGuid,
		bool annonymous,
		bool complete)
	{
		#region Bit Conversion

		int intAnnonymous;
		if (annonymous)
		{
			intAnnonymous = 1;
		}
		else
		{
			intAnnonymous = 0;
		}

		int intComplete;
		if (complete)
		{
			intComplete = 1;
		}
		else
		{
			intComplete = 0;
		}


		#endregion

		string sqlCommand = @"
INSERT INTO mp_SurveyResponses (
    ResponseGuid, 
    SurveyGuid, 
    UserGuid, 
    Annonymous, 
    Complete 
) 
VALUES (
    ?ResponseGuid, 
    ?SurveyGuid, 
    ?UserGuid, 
    ?Annonymous, 
    ?Complete 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?ResponseGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = responseGuid.ToString()
			},

			new("?SurveyGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = surveyGuid.ToString()
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?Annonymous", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAnnonymous
			},

			new("?Complete", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intComplete
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}

	/// <summary>
	/// Updates the status of a response. Returns true if row updated.
	/// </summary>
	/// <param name="responseGuid"> responseGuid </param>
	/// <param name="complete"> complete </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid responseGuid,
		DateTime submissionDate,
		bool complete)
	{

		#region Bit Conversion

		int intComplete;

		if (complete)
		{
			intComplete = 1;
		}
		else
		{
			intComplete = 0;
		}

		#endregion

		string sqlCommand = @"
UPDATE 
    mp_SurveyResponses 
SET 
    Complete = ?Complete, 
    SubmissionDate = ?SubmissionDate 
WHERE 
    ResponseGuid = ?ResponseGuid ";

		var arParams = new List<MySqlParameter>
		{
			new("?ResponseGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = responseGuid.ToString()
			},

			new("?Complete", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intComplete
			},

			new("?SubmissionDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = submissionDate
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;
	}

	/// <summary>
	/// Deletes a row from the mp_SurveyResponses table. Returns true if row deleted.
	/// </summary>
	/// <param name="responseGuid"> responseGuid </param>
	/// <returns>bool</returns>
	public static bool Delete(
		Guid responseGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_SurveyResponses 
WHERE ResponseGuid = ?ResponseGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ResponseGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = responseGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_SurveyResponses table.
	/// </summary>
	/// <param name="responseGuid"> responseGuid </param>
	public static IDataReader GetOne(
		Guid responseGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SurveyResponses 
WHERE ResponseGuid = ?ResponseGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ResponseGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = responseGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with all rows in the mp_SurveyResponses table.
	/// </summary>
	public static IDataReader GetAll(Guid surveyGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SurveyResponses 
WHERE SurveyGuid = ?SurveyGuid; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SurveyGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = surveyGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}


	/// <summary>
	/// Gets an IDataReader with the first response to a survey
	/// </summary>
	public static IDataReader GetFirst(Guid surveyGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SurveyResponses 
WHERE SurveyGuid = ?SurveyGuid 
AND Complete = 1 
ORDER BY SubmissionDate, ResponseGuid 
LIMIT 1; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SurveyGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = surveyGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}


	/// <summary>
	/// Gets an IDataReader with the next response to a survey
	/// </summary>
	public static IDataReader GetNext(Guid responseGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SurveyResponses 
WHERE SubmissionDate > (
    SELECT SubmissionDate 
    FROM mp_SurveyResponses 
    WHERE ResponseGuid = ?ResponseGuid
) 
AND SurveyGuid = (
    SELECT SurveyGuid 
    FROM mp_SurveyResponses 
    WHERE 
    ResponseGuid = ?ResponseGuid
) 
AND Complete = 1 
Order By 
    SubmissionDate, 
    ResponseGuid 
Limit 1; ";

		var arParams = new List<MySqlParameter>
		{
			new("?ResponseGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = responseGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}

	/// <summary>
	/// Gets an IDataReader with the next response to a survey
	/// </summary>
	public static IDataReader GetPrevious(Guid responseGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SurveyResponses 
WHERE Complete = 1 
AND SubmissionDate < (
    SELECT SubmissionDate 
    FROM mp_SurveyResponses 
    WHERE 
    ResponseGuid = ?ResponseGuid
) 
AND 
SurveyGuid = (
    SELECT SurveyGuid 
    FROM mp_SurveyResponses 
    WHERE 
    ResponseGuid = ?ResponseGuid
) 
Order By 
    SubmissionDate DESC, 
    ResponseGuid; ";

		var arParams = new List<MySqlParameter>
		{
			new("?ResponseGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = responseGuid.ToString()
			}
		};



		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}



}
