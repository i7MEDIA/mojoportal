using mojoPortal.Data;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;


namespace SurveyFeature.Data;

public static class DBQuestionOption
{


	/// <summary>
	/// Inserts a row in the mp_SurveyQuestionOptions table. Returns rows affected count.
	/// </summary>
	/// <param name="questionOptionGuid"> questionOptionGuid </param>
	/// <param name="questionGuid"> questionGuid </param>
	/// <param name="answer"> answer </param>
	/// <param name="order"> order </param>
	/// <returns>int</returns>
	public static int Add(
		Guid questionOptionGuid,
		Guid questionGuid,
		string answer,
		int order)
	{

		string sqlCommand = @"
INSERT INTO mp_SurveyQuestionOptions (
    QuestionOptionGuid, 
    QuestionGuid, 
    Answer, 
    `Order`
) 
VALUES (
    ?QuestionOptionGuid, 
    ?QuestionGuid, 
    ?Answer, 
    ?Order
); ";

		var arParams = new List<MySqlParameter>
		{
			new("?QuestionOptionGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = questionOptionGuid.ToString()
			},

			new("?QuestionGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = questionGuid.ToString()
			},

			new("?Answer", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = answer
			},

			new("?Order", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = order
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}


	/// <summary>
	/// Updates a row in the mp_SurveyQuestionOptions table. Returns true if row updated.
	/// </summary>
	/// <param name="questionOptionGuid"> questionOptionGuid </param>
	/// <param name="questionGuid"> questionGuid </param>
	/// <param name="answer"> answer </param>
	/// <param name="order"> order </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid questionOptionGuid,
		Guid questionGuid,
		string answer,
		int order)
	{

		string sqlCommand = @"
UPDATE 
    mp_SurveyQuestionOptions 
SET  
    QuestionGuid = ?QuestionGuid, 
    Answer = ?Answer, 
    `Order` = ?Order 
WHERE 
    QuestionOptionGuid = ?QuestionOptionGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?QuestionOptionGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = questionOptionGuid.ToString()
			},

			new("?QuestionGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = questionGuid.ToString()
			},

			new("?Answer", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = answer
			},

			new("?Order", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = order
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_SurveyQuestionOptions table. Returns true if row deleted.
	/// </summary>
	/// <param name="questionOptionGuid"> questionOptionGuid </param>
	/// <returns>bool</returns>
	public static bool Delete(
		Guid questionOptionGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_SurveyQuestionOptions 
WHERE QuestionOptionGuid = ?QuestionOptionGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?QuestionOptionGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = questionOptionGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	/// <summary>
	/// Gets an IDataReader with all rows in the mp_SurveyQuestionOptions table.
	/// </summary>
	public static IDataReader GetAll(Guid questionGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SurveyQuestionOptions 
WHERE QuestionGuid = ?QuestionGuid 
ORDER BY `Order` ;";

		var arParams = new List<MySqlParameter>
		{
			new("?QuestionGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = questionGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_SurveyQuestionOptions table.
	/// </summary>
	/// <param name="questionOptionGuid"> questionOptionGuid </param>
	public static IDataReader GetOne(
		Guid questionOptionGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SurveyQuestionOptions 
WHERE QuestionOptionGuid = ?QuestionOptionGuid 
ORDER BY `Order`; ";

		var arParams = new List<MySqlParameter>
		{
			new("?QuestionOptionGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = questionOptionGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}


}
