using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;


public static class DBLetterSendLog
{

	/// <summary>
	/// Inserts a row in the mp_LetterSendLog table. Returns new integer id.
	/// </summary>
	/// <param name="letterGuid"> letterGuid </param>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="emailAddress"> emailAddress </param>
	/// <param name="uTC"> uTC </param>
	/// <param name="errorOccurred"> errorOccurred </param>
	/// <param name="errorMessage"> errorMessage </param>
	/// <returns>int</returns>
	public static int Create(
		Guid letterGuid,
		Guid userGuid,
		Guid subscribeGuid,
		string emailAddress,
		DateTime uTC,
		bool errorOccurred,
		string errorMessage)
	{

		#region Bit Conversion
		int intErrorOccurred;
		if (errorOccurred)
		{
			intErrorOccurred = 1;
		}
		else
		{
			intErrorOccurred = 0;
		}


		#endregion

		string sqlCommand = @"
INSERT INTO 
    mp_LetterSendLog (
        LetterGuid, 
        UserGuid, 
        SubscribeGuid, 
        EmailAddress, 
        UTC, 
        ErrorOccurred, 
        ErrorMessage 
    )
VALUES (
    ?LetterGuid, 
    ?UserGuid, 
    ?SubscribeGuid, 
    ?EmailAddress, 
    ?UTC, 
    ?ErrorOccurred, 
    ?ErrorMessage 
);

SELECT 
    LAST_INSERT_ID();";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterGuid.ToString()
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?EmailAddress", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = emailAddress
			},

			new("?UTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = uTC
			},

			new ("?ErrorOccurred", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intErrorOccurred
			},

			new ("?ErrorMessage", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = errorMessage
			},

			new("?SubscribeGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = subscribeGuid.ToString()
			}
		};

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams).ToString());
		return newID;

	}


	/// <summary>
	/// Updates a row in the mp_LetterSendLog table. Returns true if row updated.
	/// </summary>
	/// <param name="rowID"> rowID </param>
	/// <param name="letterGuid"> letterGuid </param>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="emailAddress"> emailAddress </param>
	/// <param name="uTC"> uTC </param>
	/// <param name="errorOccurred"> errorOccurred </param>
	/// <param name="errorMessage"> errorMessage </param>
	/// <returns>bool</returns>
	public static bool Update(
		int rowId,
		Guid letterGuid,
		Guid userGuid,
		string emailAddress,
		DateTime uTC,
		bool errorOccurred,
		string errorMessage)
	{
		#region Bit Conversion

		int intErrorOccurred;
		if (errorOccurred)
		{
			intErrorOccurred = 1;
		}
		else
		{
			intErrorOccurred = 0;
		}


		#endregion

		string sqlCommand = @"
UPDATE 
    mp_LetterSendLog 
SET  
    LetterGuid = ?LetterGuid, 
    UserGuid = ?UserGuid, 
    EmailAddress = ?EmailAddress, 
    UTC = ?UTC, 
    ErrorOccurred = ?ErrorOccurred, 
    ErrorMessage = ?ErrorMessage 
WHERE  
    RowID = ?RowID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = rowId
			},

			new("?LetterGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterGuid.ToString()
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?EmailAddress", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = emailAddress
			},

			new("?UTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = uTC
			},

			new("?ErrorOccurred", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intErrorOccurred
			},

			new("?ErrorMessage", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = errorMessage
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_LetterSendLog table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowID"> rowID </param>
	/// <returns>bool</returns>
	public static bool Delete(int rowId)
	{
		string sqlCommand = @"
DELETE FROM mp_LetterSendLog 
WHERE RowID = ?RowID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = rowId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}


	/// <summary>
	/// Deletes from the mp_LetterSendLog table for the letterGuid. Returns true if row deleted.
	/// </summary>
	/// <param name="letterGuid"> letterGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteByLetter(Guid letterGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_LetterSendLog 
WHERE LetterGuid = ?LetterGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	public static bool DeleteByLetterInfo(Guid letterInfoGuid)
	{
		string sqlCommand = @"
DELETE FROM 
    mp_LetterSendLog 
WHERE 
    LetterGuid 
IN (
    SELECT LetterGuid 
    FROM mp_Letter 
    WHERE LetterInfoGuid = ?LetterInfoGuid
);";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterInfoGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	public static bool DeleteBySite(Guid siteGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_LetterSendLog 
WHERE LetterGuid IN (
    SELECT LetterGuid 
    FROM mp_Letter 
    WHERE LetterInfoGuid IN (
        SELECT LetterInfoGuid 
        FROM mp_LetterInfo 
        WHERE SiteGuid = ?SiteGuid
    )
);";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_LetterSendLog table.
	/// </summary>
	/// <param name="rowID"> rowID </param>
	public static IDataReader GetOne(
		int rowId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_LetterSendLog 
WHERE RowID = ?RowID ";

		var arParams = new List<MySqlParameter>
		{
			new("?RowID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = rowId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with all rows in the mp_LetterSendLog table.
	/// </summary>
	public static IDataReader GetByLetter(Guid letterGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_LetterSendLog 
WHERE LetterGuid = ?LetterGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);
	}

	/// <summary>
	/// Gets a count of rows in the mp_LetterSendLog table.
	/// </summary>
	public static int GetCount(Guid letterGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_LetterSendLog 
WHERE LetterGuid = ?LetterGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterGuid.ToString()
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));
	}

	/// <summary>
	/// Gets a page of data from the mp_LetterSendLog table.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static IDataReader GetPage(
		Guid letterGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCount(letterGuid);

		if (pageSize > 0) totalPages = totalRows / pageSize;

		if (totalRows <= pageSize)
		{
			totalPages = 1;
		}
		else
		{
			int remainder;
			Math.DivRem(totalRows, pageSize, out remainder);
			if (remainder > 0)
			{
				totalPages += 1;
			}
		}

		string sqlCommand = @"
SELECT * 
FROM mp_LetterSendLog  
WHERE LetterGuid = ?LetterGuid 
LIMIT " + pageLowerBound.ToString() + ", ?PageSize;";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterGuid.ToString()
			},

			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);
	}



}
