using mojoPortal.Data;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurveyFeature.Data;

public static class DBQuestionAnswer
{

	/// <summary>
	/// Inserts a row in the mp_SurveyQuestionAnswers table. Returns rows affected count.
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
		string answer)
	{
		#region Bit Conversion


		#endregion

		string sqlCommand = @"
INSERT INTO mp_SurveyQuestionAnswers (
    AnswerGuid, 
    QuestionGuid, 
    ResponseGuid, 
    Answer, 
    AnsweredDate 
) 
VALUES (
    ?AnswerGuid, 
    ?QuestionGuid, 
    ?ResponseGuid, 
    ?Answer, 
    ?AnsweredDate 
); ";


		var arParams = new List<MySqlParameter>
		{
			new("?AnswerGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = answerGuid.ToString()
			},

			new("?QuestionGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = questionGuid.ToString()
			},

			new("?ResponseGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = responseGuid.ToString()
			},

			new("?Answer", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = answer
			},

			new("?AnsweredDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DateTime.UtcNow
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}


	/// <summary>
	/// Updates a row in the mp_SurveyQuestionAnswers table. Returns true if row updated.
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
		string answer)
	{
		#region Bit Conversion


		#endregion

		string sqlCommand = @"
UPDATE 
    mp_SurveyQuestionAnswers 
SET  
    QuestionGuid = ?QuestionGuid, 
    ResponseGuid = ?ResponseGuid, 
    Answer = ?Answer 
WHERE 
    AnswerGuid = ?AnswerGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?AnswerGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = answerGuid.ToString()
			},

			new("?QuestionGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = questionGuid.ToString()
			},

			new("?ResponseGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = responseGuid.ToString()
			},

			new("?Answer", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = answer
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_SurveyQuestionAnswers table.
	/// </summary>
	/// <param name="answerGuid"> answerGuid </param>
	public static IDataReader GetOne(Guid responseGuid, Guid questionGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SurveyQuestionAnswers 
WHERE QuestionGuid = ?QuestionGuid 
AND ResponseGuid = ?ResponseGuid; ";

		var arParams = new List<MySqlParameter>
		{
			new("?QuestionGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = questionGuid.ToString()
			},

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
