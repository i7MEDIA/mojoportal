using mojoPortal.Data;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace PollFeature.Data;

public static class DBPollOption
{

	/// <summary>
	/// Inserts a row in the mp_PollOptions table. Returns rows affected count.
	/// </summary>
	/// <param name="optionGuid"> optionGuid </param>
	/// <param name="pollGuid"> pollGuid </param>
	/// <param name="answer"> answer </param>
	/// <param name="votes"> votes </param>
	/// <param name="order"> order </param>
	/// <returns>int</returns>
	public static int Add(
		Guid optionGuid,
		Guid pollGuid,
		string answer,
		int order)
	{
		int votes = 0;

		string sqlCommand = @"
INSERT INTO mp_PollOptions (
    OptionGuid, 
    PollGuid, 
    Answer, 
    Votes, 
    `Order` 
) 
VALUES (
    ?OptionGuid, 
    ?PollGuid, 
    ?Answer, 
    ?Votes, 
    ?Order 
) ;";

		var arParams = new List<MySqlParameter>
		{
			new("?OptionGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = optionGuid.ToString()
			},

			new("?PollGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pollGuid.ToString()
			},

			new("?Answer", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = answer
			},

			new("?Votes", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = votes
			},

			new("?Order", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = order
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		return rowsAffected;

	}

	/// <summary>
	/// Updates a row in the mp_PollOptions table. Returns true if row updated.
	/// </summary>
	/// <param name="optionGuid"> optionGuid </param>
	/// <param name="pollGuid"> pollGuid </param>
	/// <param name="answer"> answer </param>
	/// <param name="votes"> votes </param>
	/// <param name="order"> order </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid optionGuid,
		string answer,
		int order)
	{

		string sqlCommand = @"
UPDATE mp_PollOptions 
SET 
    Answer = ?Answer, 
    `Order` = ?Order 
WHERE  
OptionGuid = ?OptionGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?OptionGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = optionGuid.ToString()
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
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		return rowsAffected > 0;

	}

	/// <summary>
	/// Deletes a row from the mp_PollOptions table. Returns true if row deleted.
	/// </summary>
	/// <param name="optionGuid"> optionGuid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid optionGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_PollOptions 
WHERE OptionGuid = ?OptionGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?OptionGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = optionGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		return rowsAffected > 0;

	}



	/// <summary>
	/// Gets an IDataReader from the mp_PollOptions table.
	/// </summary>
	/// <param name="optionGuid"> pollGuid </param>
	public static IDataReader GetPollOptions(Guid pollGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_PollOptions 
WHERE PollGuid = ?PollGuid 
ORDER BY `Order`, Answer ;";

		var arParams = new List<MySqlParameter>
		{
			new("?PollGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pollGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_PollOptions table.
	/// </summary>
	/// <param name="optionGuid"> optionGuid </param>
	public static IDataReader GetPollOption(Guid optionGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_PollOptions 
WHERE OptionGuid = ?OptionGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?OptionGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = optionGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);

	}

	public static bool IncrementVotes(
			Guid pollGuid,
			Guid optionGuid,
			Guid userGuid)
	{
		if (DBPoll.UserHasVoted(pollGuid, userGuid)) return false;

		string sqlCommand = @"";

		if (userGuid != Guid.Empty)
		{

			sqlCommand += @"
INSERT INTO mp_PollUsers (
    PollGuid, 
    OptionGuid, 
    UserGuid 
) 
VALUES (
    ?PollGuid, 
    ?OptionGuid, 
    ?UserGuid 
);";

		}
		sqlCommand += @"
UPDATE mp_PollOptions 
SET Votes = Votes + 1 
WHERE OptionGuid = ?OptionGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?OptionGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = optionGuid.ToString()
			},

			new("?PollGuid", MySqlDbType.VarChar, 36) {
				Direction = ParameterDirection.Input,
				Value = pollGuid.ToString()
			},

			new("?UserGuid", MySqlDbType.VarChar, 36) {
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		return rowsAffected > 0;


	}




}
