using mojoPortal.Data;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace PollFeature.Data;

public static class DBPoll
{

	/// <summary>
	/// Inserts a row in the mp_Polls table. Returns rows affected count.
	/// </summary>
	/// <param name="pollGuid"> pollGuid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="question"> question </param>
	/// <param name="active"> active </param>
	/// <param name="anonymousVoting"> anonymousVoting </param>
	/// <param name="allowViewingResultsBeforeVoting"> allowViewingResultsBeforeVoting </param>
	/// <param name="showOrderNumbers"> showOrderNumbers </param>
	/// <param name="showResultsWhenDeactivated"> showResultsWhenDeactivated </param>
	/// <param name="activeFrom"> activeFrom </param>
	/// <param name="activeTo"> activeTo </param>
	/// <returns>int</returns>
	public static int Add(
		Guid pollGuid,
		Guid siteGuid,
		string question,
		bool active,
		bool anonymousVoting,
		bool allowViewingResultsBeforeVoting,
		bool showOrderNumbers,
		bool showResultsWhenDeactivated,
		DateTime activeFrom,
		DateTime activeTo)
	{

		#region Bit Conversion

		int intActive;
		if (active)
		{
			intActive = 1;
		}
		else
		{
			intActive = 0;
		}

		int intAnonymousVoting;
		if (anonymousVoting)
		{
			intAnonymousVoting = 1;
		}
		else
		{
			intAnonymousVoting = 0;
		}

		int intAllowViewingResultsBeforeVoting;
		if (allowViewingResultsBeforeVoting)
		{
			intAllowViewingResultsBeforeVoting = 1;
		}
		else
		{
			intAllowViewingResultsBeforeVoting = 0;
		}

		int intShowOrderNumbers;
		if (showOrderNumbers)
		{
			intShowOrderNumbers = 1;
		}
		else
		{
			intShowOrderNumbers = 0;
		}

		int intShowResultsWhenDeactivated;
		if (showResultsWhenDeactivated)
		{
			intShowResultsWhenDeactivated = 1;
		}
		else
		{
			intShowResultsWhenDeactivated = 0;
		}


		#endregion

		string sqlCommand = @"
INSERT INTO mp_Polls (
    PollGuid, 
    SiteGuid, 
    Question, 
    Active, 
    AnonymousVoting, 
    AllowViewingResultsBeforeVoting, 
    ShowOrderNumbers, 
    ShowResultsWhenDeactivated, 
    ActiveFrom, 
    ActiveTo 
) 
VALUES (
    ?PollGuid, 
    ?SiteGuid, 
    ?Question, 
    ?Active, 
    ?AnonymousVoting, 
    ?AllowViewingResultsBeforeVoting, 
    ?ShowOrderNumbers, 
    ?ShowResultsWhenDeactivated, 
    ?ActiveFrom, 
    ?ActiveTo 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?PollGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pollGuid.ToString()
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?Question", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = question
			},

			new("?Active", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intActive
			},

			new("?AnonymousVoting", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAnonymousVoting
			},

			new("?AllowViewingResultsBeforeVoting", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAllowViewingResultsBeforeVoting
			},

			new("?ShowOrderNumbers", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intShowOrderNumbers
			},

			new("?ShowResultsWhenDeactivated", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intShowResultsWhenDeactivated
			},

			new("?ActiveFrom", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = activeFrom
			},

			new("?ActiveTo", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = activeTo
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}

	/// <summary>
	/// Updates a row in the mp_Polls table. Returns true if row updated.
	/// </summary>
	/// <param name="pollGuid"> pollGuid </param>
	/// <param name="question"> question </param>
	/// <param name="active"> active </param>
	/// <param name="anonymousVoting"> anonymousVoting </param>
	/// <param name="allowViewingResultsBeforeVoting"> allowViewingResultsBeforeVoting </param>
	/// <param name="showOrderNumbers"> showOrderNumbers </param>
	/// <param name="showResultsWhenDeactivated"> showResultsWhenDeactivated </param>
	/// <param name="activeFrom"> activeFrom </param>
	/// <param name="activeTo"> activeTo </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid pollGuid,
		String question,
		bool anonymousVoting,
		bool allowViewingResultsBeforeVoting,
		bool showOrderNumbers,
		bool showResultsWhenDeactivated,
		bool active,
		DateTime activeFrom,
		DateTime activeTo)
	{
		#region Bit Conversion

		int intActive;
		if (active)
		{
			intActive = 1;
		}
		else
		{
			intActive = 0;
		}

		int intAnonymousVoting;
		if (anonymousVoting)
		{
			intAnonymousVoting = 1;
		}
		else
		{
			intAnonymousVoting = 0;
		}

		int intAllowViewingResultsBeforeVoting;
		if (allowViewingResultsBeforeVoting)
		{
			intAllowViewingResultsBeforeVoting = 1;
		}
		else
		{
			intAllowViewingResultsBeforeVoting = 0;
		}

		int intShowOrderNumbers;
		if (showOrderNumbers)
		{
			intShowOrderNumbers = 1;
		}
		else
		{
			intShowOrderNumbers = 0;
		}

		int intShowResultsWhenDeactivated;
		if (showResultsWhenDeactivated)
		{
			intShowResultsWhenDeactivated = 1;
		}
		else
		{
			intShowResultsWhenDeactivated = 0;
		}


		#endregion

		string sqlCommand = @"
UPDATE 
    mp_Polls 
SET  
    Question = ?Question, 
    Active = ?Active, 
    AnonymousVoting = ?AnonymousVoting, 
    AllowViewingResultsBeforeVoting = ?AllowViewingResultsBeforeVoting, 
    ShowOrderNumbers = ?ShowOrderNumbers, 
    ShowResultsWhenDeactivated = ?ShowResultsWhenDeactivated, 
    ActiveFrom = ?ActiveFrom, 
    ActiveTo = ?ActiveTo 
WHERE  
    PollGuid = ?PollGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?PollGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pollGuid.ToString()
			},

			new("?Question", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = question
			},

			new("?Active", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intActive
			},

			new("?AnonymousVoting", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAnonymousVoting
			},

			new("?AllowViewingResultsBeforeVoting", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAllowViewingResultsBeforeVoting
			},

			new("?ShowOrderNumbers", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intShowOrderNumbers
			},

			new("?ShowResultsWhenDeactivated", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intShowResultsWhenDeactivated
			},

			new("?ActiveFrom", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = activeFrom
			},

			new("?ActiveTo", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = activeTo
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_Polls table. Returns true if row deleted.
	/// </summary>
	/// <param name="pollGuid"> pollGuid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid pollGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_PollModules 
WHERE PollGuid = ?PollGuid ;
DELETE FROM mp_PollUsers 
WHERE PollGuid = ?PollGuid ;
DELETE FROM mp_PollOptions 
WHERE PollGuid = ?PollGuid ;
DELETE FROM mp_Polls 
WHERE PollGuid = ?PollGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?PollGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pollGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool DeleteBySite(int siteId)
	{
		string sqlCommand = @"
DELETE FROM mp_PollModules 
WHERE ModuleID IN (
    SELECT ModuleID 
    FROM mp_Modules 
    WHERE SiteID = ?SiteID
);
DELETE FROM mp_PollUsers 
WHERE PollGuid IN (
    SELECT PollGuid 
    FROM mp_Polls 
    WHERE SiteGuid IN (
        SELECT SiteGuid 
        FROM mp_Sites 
        WHERE SiteID = ?SiteID
    )
);
DELETE FROM mp_PollOptions 
WHERE PollGuid IN (
    SELECT PollGuid 
    FROM mp_Polls 
    WHERE SiteGuid IN (
        SELECT SiteGuid 
        FROM mp_Sites 
        WHERE SiteID = ?SiteID
    )
);
DELETE FROM mp_Polls 
WHERE PollGuid IN (
    SELECT PollGuid 
    FROM mp_Polls 
    WHERE SiteGuid IN (
        SELECT SiteGuid 
        FROM mp_Sites 
        WHERE SiteID = ?SiteID
    )
); ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	/// <summary>
	/// Clears the vote count a row from the mp_Polls table. 
	/// </summary>
	/// <param name="pollGuid"> pollGuid </param>
	/// <returns>bool</returns>
	public static bool ClearVotes(Guid pollGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_PollUsers 
WHERE PollGuid = ?PollGuid ; 
UPDATE mp_PollUsers 
SET Votes = 0 
WHERE PollGuid = ?PollGuid ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?PollGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pollGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}


	/// <summary>
	/// Gets an IDataReader with one row from the mp_Polls table.
	/// </summary>
	/// <param name="pollGuid"> pollGuid </param>
	public static IDataReader GetPoll(Guid pollGuid)
	{
		string sqlCommand = @"
SELECT 
    p.*, 
    (SELECT SUM(Votes) FROM mp_PollOptions WHERE mp_PollOptions.PollGuid = ?PollGuid) 
    As TotalVotes 
FROM mp_Polls p 
WHERE p.PollGuid = ?PollGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?PollGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pollGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_Polls table.
	/// </summary>
	/// <param name="pollGuid"> pollGuid </param>
	public static IDataReader GetPollByModuleID(int moduleId)
	{
		string sqlCommand = @"
SELECT p.*, 
(SELECT SUM(po.Votes) FROM mp_PollOptions po WHERE po.PollGuid = p.PollGuid) As TotalVotes 
FROM mp_Polls p 
JOIN mp_PollModules pm 
ON p.PollGuid = pm.PollGuid 
WHERE pm.ModuleID = ?ModuleID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}


	/// <summary>
	/// Gets an IDataReader with all rows from the mp_Polls table matching the siteGuid.
	/// </summary>
	/// <param name="siteGuid"> siteGuid </param>
	public static IDataReader GetPolls(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Polls 
WHERE SiteGuid = ?SiteGuid 
ORDER BY ActiveFrom DESC, Question ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with all rows from the mp_Polls table matching the siteGuid.
	/// </summary>
	/// <param name="siteGuid"> siteGuid </param>
	public static IDataReader GetActivePolls(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Polls 
WHERE 
    SiteGuid = ?SiteGuid 
    AND ActiveFrom <= ?CurrentTime 
    AND ActiveTo >= ?CurrentTime 
ORDER BY Question ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DateTime.UtcNow
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with all rows from the mp_Polls table matching the siteGuid.
	/// </summary>
	/// <param name="siteGuid"> siteGuid </param>
	public static IDataReader GetPollsByUserGuid(Guid userGuid)
	{
		string sqlCommand = @"
SELECT 
    p.*, 
    po.OptionGuid, 
    po.Answer 
FROM mp_Polls p 
JOIN 
    mp_PollUsers pu 
ON p.PollGuid = pu.PollGuid 
JOIN 
    mp_PollOptions po 
ON pu.OptionGuid = po.OptionGuid 
WHERE 
    pu.UserGuid = ?UserGuid 
ORDER BY ActiveFrom DESC, Question ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}


	/// <summary>
	/// Gets a count of rows in the mp_Polls table.
	/// </summary>
	public static bool UserHasVoted(Guid pollGuid, Guid userGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_PollUsers 
WHERE UserGuid = ?UserGuid 
AND PollGuid = ?PollGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?PollGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pollGuid.ToString()
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

		return count > 0;
	}

	/// <summary>
	/// to comment
	/// </summary>
	/// <param name="pollGuid"> pollGuid </param>
	/// <returns>bool</returns>
	public static bool AddToModule(Guid pollGuid, int moduleId)
	{
		string sqlCommand = @"
DELETE FROM mp_PollModules 
WHERE ModuleID = ?ModuleID ;
INSERT INTO mp_PollModules (
    PollGuid, 
    ModuleID
) 
VALUES( 
    ?PollGuid, 
    ?ModuleID 
) ;";


		var arParams = new List<MySqlParameter>
		{
			new("?PollGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pollGuid.ToString()
			},

			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	/// <summary>
	/// to comment
	/// </summary>
	/// <param name="moduleID"> moduleID </param>
	/// <returns>bool</returns>
	public static bool RemoveFromModule(int moduleId)
	{
		string sqlCommand = @"
DELETE FROM mp_PollModules 
WHERE ModuleID = ?ModuleID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}



}
