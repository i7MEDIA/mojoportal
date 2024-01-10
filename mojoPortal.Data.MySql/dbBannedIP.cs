using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;

public static class DBBannedIP
{

	/// <summary>
	/// Inserts a row in the mp_BannedIPAddresses table. Returns new integer id.
	/// </summary>
	/// <param name="bannedIP"> bannedIP </param>
	/// <param name="bannedUTC"> bannedUTC </param>
	/// <param name="bannedReason"> bannedReason </param>
	/// <returns>int</returns>
	public static int Add(
		string bannedIP,
		DateTime bannedUtc,
		string bannedReason)
	{

		var sqlCommand = @"
INSERT INTO mp_BannedIPAddresses (
    BannedIP, 
    BannedUTC, 
    BannedReason )
VALUES (
    ?BannedIP, 
    ?BannedUTC, 
    ?BannedReason );
SELECT LAST_INSERT_ID();";

		var arParams = new List<MySqlParameter>
		{
			new("?BannedIP", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = bannedIP
			},

			new("?BannedUTC", MySqlDbType.DateTime)
			{
			Direction = ParameterDirection.Input,
			Value = bannedUtc
			},

			new("?BannedReason", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = bannedReason
			}
		};

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams).ToString());
		return newID;

	}


	/// <summary>
	/// Updates a row in the mp_BannedIPAddresses table. Returns true if row updated.
	/// </summary>
	/// <param name="rowID"> rowID </param>
	/// <param name="bannedIP"> bannedIP </param>
	/// <param name="bannedUTC"> bannedUTC </param>
	/// <param name="bannedReason"> bannedReason </param>
	/// <returns>bool</returns>
	public static bool Update(
		int rowId,
		string bannedIP,
		DateTime bannedUtc,
		string bannedReason)
	{

		var sqlCommand = @"
UPDATE mp_BannedIPAddresses 
SET  
    BannedIP = ?BannedIP, 
    BannedUTC = ?BannedUTC, 
    BannedReason = ?BannedReason 
WHERE  
    RowID = ?RowID;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = rowId
			},

			new("?BannedIP", MySqlDbType.VarChar, 50)
			{
			Direction = ParameterDirection.Input,
			Value = bannedIP
			},

			new("?BannedUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = bannedUtc
			},

			new("?BannedReason", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = bannedReason
			},
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_BannedIPAddresses table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowID"> rowID </param>
	/// <returns>bool</returns>
	public static bool Delete(
		int rowId)
	{
		string sqlCommand = @"
DELETE FROM mp_BannedIPAddresses
WHERE
    RowID = ?RowID;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = rowId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	/// <summary>
	/// Returns true if the passed in address is banned
	/// </summary>
	/// <param name="rowID"> rowID </param>
	/// <returns>bool</returns>
	public static bool IsBanned(string ipAddress)
	{
		string sqlCommand = @"
SELECT  Count(*) 
FROM    mp_BannedIPAddresses 
WHERE 
    BannedIP = ?BannedIP;";

		var arParams = new List<MySqlParameter>
		{

		new("?BannedIP", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = ipAddress
			}
		};

		int foundRows = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

		return foundRows > 0;
	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_BannedIPAddresses table.
	/// </summary>
	/// <param name="rowID"> rowID </param>
	public static IDataReader GetOne(int rowId)
	{
		string sqlCommand = @"
SELECT  * 
FROM	mp_BannedIPAddresses 
WHERE 
    RowID = ?RowID;";

		var arParams = new List<MySqlParameter>
		{
			new ("?RowID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = rowId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with rows from the mp_BannedIPAddresses table.
	/// </summary>
	/// <param name="ipAddress"> ipAddress </param>
	public static IDataReader GeByIpAddress(string ipAddress)
	{
		string sqlCommand = @"
SELECT  * 
FROM	mp_BannedIPAddresses 
WHERE 
    BannedIP = ?BannedIP;";

		var arParams = new List<MySqlParameter>
		{
			new("?BannedIP", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = ipAddress
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with all rows in the mp_BannedIPAddresses table.
	/// </summary>
	public static IDataReader GetAll()
	{
		string sqlCommand = @"
SELECT  *
FROM	mp_BannedIPAddresses;";

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString());
	}

	/// <summary>
	/// Gets a count of rows in the mp_BannedIPAddresses table.
	/// </summary>
	public static int GetCount()
	{
		string sqlCommand = @"
SELECT  Count(*) 
FROM	mp_BannedIPAddresses;";

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString()));
	}

	/// <summary>
	/// Gets a page of data from the mp_BannedIPAddresses table.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static IDataReader GetPage(
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCount();

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
SELECT	* 
FROM	mp_BannedIPAddresses  
ORDER BY  BannedIP 
LIMIT " + pageLowerBound.ToString() + ", ?PageSize;";

		var arParams = new List<MySqlParameter>
		{
			new("?PageNumber", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageNumber
			},

			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			}

		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}
}