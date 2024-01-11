using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;

public static class DBGeoZone
{
	/// <summary>
	/// Inserts a row in the mp_GeoZone table. Returns rows affected count.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="countryGuid"> countryGuid </param>
	/// <param name="name"> name </param>
	/// <param name="code"> code </param>
	/// <returns>int</returns>
	public static int Create(
		Guid guid,
		Guid countryGuid,
		string name,
		string code)
	{

		string sqlCommand = @"
INSERT INTO mp_GeoZone (
    Guid, 
    CountryGuid, 
    Name, 
    Code )
        VALUES (
    ?Guid, 
    ?CountryGuid, 
    ?Name, 
    ?Code 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
		{
			Direction = ParameterDirection.Input,
			Value = guid.ToString()
		},

			new("?CountryGuid", MySqlDbType.VarChar, 36) {
			Direction = ParameterDirection.Input,
			Value = countryGuid.ToString()
			},

			new("?Name", MySqlDbType.VarChar, 255) {
			Direction = ParameterDirection.Input,
			Value = name
			},

			new("?Code", MySqlDbType.VarChar, 255) {
			Direction = ParameterDirection.Input,
			Value = code
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}


	/// <summary>
	/// Updates a row in the mp_GeoZone table. Returns true if row updated.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="countryGuid"> countryGuid </param>
	/// <param name="name"> name </param>
	/// <param name="code"> code </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid guid,
		Guid countryGuid,
		string name,
		string code)
	{

		string sqlCommand = @"
UPDATE 
    mp_GeoZone 
SET  
    CountryGuid = ?CountryGuid, 
    Name = ?Name, 
    Code = ?Code 
WHERE  
    Guid = ?Guid;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?CountryGuid", MySqlDbType.VarChar, 36) {
			Direction = ParameterDirection.Input,
			Value = countryGuid.ToString()
			},

			new("?Name", MySqlDbType.VarChar, 255) {
			Direction = ParameterDirection.Input,
			Value = name
			},

			new("?Code", MySqlDbType.VarChar, 255) {
			Direction = ParameterDirection.Input,
			Value = code
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_GeoZone table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid guid)
	{
		string sqlCommand = @"
DELETE FROM mp_GeoZone 
WHERE Guid = ?Guid;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_GeoZone table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetOne(Guid guid)
	{
		string sqlCommand = @"
SELECT  * 
FROM	mp_GeoZone 
WHERE 
Guid = ?Guid;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_GeoZone table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetByCode(Guid countryGuid, string code)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_GeoZone 
WHERE CountryGuid = ?CountryGuid 
AND Code = ?Code;";

		var arParams = new List<MySqlParameter>
		{
			new("?CountryGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = countryGuid.ToString()
			},

			new("?Code", MySqlDbType.VarChar, 255) {
			Direction = ParameterDirection.Input,
			Value = code
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with all rows in the mp_GeoZone table.
	/// </summary>
	public static IDataReader GetByCountry(Guid countryGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_GeoZone 
WHERE CountryGuid = ?CountryGuid 
ORDER BY Name;";

		var arParams = new List<MySqlParameter>
		{
			new("?CountryGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = countryGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}

	/// <summary>
	/// Gets a count of rows in the mp_GeoZone table.
	/// </summary>
	public static int GetCount(Guid countryGuid)
	{
		string sqlCommand = @"
SELECT  Count(*) 
FROM	mp_GeoZone 
WHERE 
CountryGuid = ?CountryGuid 
;";

		var arParams = new List<MySqlParameter>
		{
			new("?CountryGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = countryGuid.ToString()
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));
	}

	/// <summary>
	/// Gets a page of data from the mp_GeoZone table.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static IDataReader GetPage(
		Guid countryGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCount(countryGuid);

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
SELECT gz.*, gc.Name As CountryName 
FROM mp_GeoZone gz 
JOIN mp_GeoCountry gc 
ON gz.CountryGuid = gc.Guid 
WHERE gz.CountryGuid = ?CountryGuid 
ORDER BY gz.Name 
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}
		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?CountryGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = countryGuid.ToString()
			},

			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			},

			new ("?OffsetRows", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageLowerBound
			}
		};



		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}
}
