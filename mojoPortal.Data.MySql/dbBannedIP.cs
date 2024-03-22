using System;
using System.Data;
using System.Globalization;
using MySqlConnector;

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
	public static int Add(string bannedIP, DateTime bannedUtc, string bannedReason)
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

		var arParams = new MySqlParameter[]
		{
			new("?BannedIP", MySqlDbType.VarChar, 50) { Value = bannedIP },
			new("?BannedUTC", MySqlDbType.DateTime) { Value = bannedUtc },
			new("?BannedReason", MySqlDbType.VarChar, 255) { Value = bannedReason }
		};

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(ConnectionString.GetWrite(), sqlCommand, arParams));

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
	public static bool Update(int rowId, string bannedIP, DateTime bannedUtc, string bannedReason)
	{
		var sqlCommand = @"
UPDATE mp_BannedIPAddresses 
SET  
    BannedIP = ?BannedIP, 
    BannedUTC = ?BannedUTC, 
    BannedReason = ?BannedReason 
WHERE  
    RowID = ?RowID;";

		var arParams = new MySqlParameter[]
		{
			new("?RowID", MySqlDbType.Int32) { Value = rowId },
			new("?BannedIP", MySqlDbType.VarChar, 50) { Value = bannedIP },
			new("?BannedUTC", MySqlDbType.DateTime) { Value = bannedUtc },
			new("?BannedReason", MySqlDbType.VarChar, 255) { Value = bannedReason }
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(ConnectionString.GetWrite(), sqlCommand, arParams);

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
		string sqlCommand = @"DELETE FROM mp_BannedIPAddresses WHERE RowID = ?RowID;";

		var param = new MySqlParameter("?RowID", MySqlDbType.Int32) { Value = rowId };

		int rowsAffected = CommandHelper.ExecuteNonQuery(ConnectionString.GetWrite(), sqlCommand.ToString(), param);

		return rowsAffected > 0;
	}

	/// <summary>
	/// Returns true if the passed in address is banned
	/// </summary>
	/// <param name="rowID"> rowID </param>
	/// <returns>bool</returns>
	public static bool IsBanned(string ipAddress)
	{
		string sqlCommand = @"SELECT Count(*) FROM mp_BannedIPAddresses WHERE BannedIP = ?BannedIP;";

		var param = new MySqlParameter("?BannedIP", MySqlDbType.VarChar, 50) { Value = ipAddress };

		int foundRows = Convert.ToInt32(CommandHelper.ExecuteScalar(ConnectionString.GetRead(), sqlCommand, param));

		return foundRows > 0;
	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_BannedIPAddresses table.
	/// </summary>
	/// <param name="rowID"> rowID </param>
	public static IDataReader GetOne(int rowId)
	{
		string sqlCommand = @"SELECT * FROM	mp_BannedIPAddresses WHERE RowID = ?RowID;";

		var param = new MySqlParameter("?RowID", MySqlDbType.Int32) { Value = rowId };

		return CommandHelper.ExecuteReader(ConnectionString.GetRead(), sqlCommand, param);
	}

	/// <summary>
	/// Gets an IDataReader with rows from the mp_BannedIPAddresses table.
	/// </summary>
	/// <param name="ipAddress"> ipAddress </param>
	public static IDataReader GeByIpAddress(string ipAddress)
	{
		string sqlCommand = @"SELECT * FROM mp_BannedIPAddresses WHERE BannedIP = ?BannedIP;";

		var param = new MySqlParameter("?BannedIP", MySqlDbType.VarChar, 50) { Value = ipAddress };

		return CommandHelper.ExecuteReader(ConnectionString.GetRead(), sqlCommand.ToString(), param);

	}

	/// <summary>
	/// Gets an IDataReader with all rows in the mp_BannedIPAddresses table.
	/// </summary>
	public static IDataReader GetAll() => CommandHelper.ExecuteReader(ConnectionString.GetRead(), "SELECT * FROM mp_BannedIPAddresses;");

	/// <summary>
	/// Gets a count of rows in the mp_BannedIPAddresses table.
	/// </summary>
	public static int GetCount() => Convert.ToInt32(CommandHelper.ExecuteScalar(ConnectionString.GetRead(), "SELECT Count(*) FROM mp_BannedIPAddresses;"));

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

		string sqlCommand = @$"
SELECT * 
FROM mp_BannedIPAddresses  
ORDER BY  BannedIP 
LIMIT {pageLowerBound.ToString(CultureInfo.InvariantCulture)}, ?PageSize;";

		var arParams = new MySqlParameter[]
		{
			new("?PageNumber", MySqlDbType.Int32) { Value = pageNumber },
			new("?PageSize", MySqlDbType.Int32) { Value = pageSize }
		};

		return CommandHelper.ExecuteReader(ConnectionString.GetRead(), sqlCommand.ToString(), arParams);
	}
}
