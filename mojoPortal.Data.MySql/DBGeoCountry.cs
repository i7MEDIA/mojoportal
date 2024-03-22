using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;

public static class DBGeoCountry
{

	/// <summary>
	/// Inserts a row in the mp_GeoCountry table. Returns rows affected count.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="name"> name </param>
	/// <param name="iSOCode2"> iSOCode2 </param>
	/// <param name="iSOCode3"> iSOCode3 </param>
	/// <returns>int</returns>
	public static int Create(
		Guid guid,
		string name,
		string iSOCode2,
		string iSOCode3)
	{

		string sqlCommand = @"
INSERT INTO mp_GeoCountry (
        Guid, 
        Name, 
        ISOCode2, 
        ISOCode3 
    ) 
VALUES (
    ?Guid, 
    ?Name, 
    ?ISOCode2, 
    ?ISOCode3 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?Name", MySqlDbType.VarChar, 255) {
			Direction = ParameterDirection.Input,
			Value = name
			},

			new("?ISOCode2", MySqlDbType.VarChar, 2) {
			Direction = ParameterDirection.Input,
			Value = iSOCode2
			},

			new("?ISOCode3", MySqlDbType.VarChar, 3) {
			Direction = ParameterDirection.Input,
			Value = iSOCode3
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}


	/// <summary>
	/// Updates a row in the mp_GeoCountry table. Returns true if row updated.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="name"> name </param>
	/// <param name="iSOCode2"> iSOCode2 </param>
	/// <param name="iSOCode3"> iSOCode3 </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid guid,
		string name,
		string iSOCode2,
		string iSOCode3)
	{

		string sqlCommand = @"
UPDATE 
    mp_GeoCountry 
SET  
    Name = ?Name, 
    ISOCode2 = ?ISOCode2, 
    ISOCode3 = ?ISOCode3 
WHERE  
    Guid = ?Guid;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?Name", MySqlDbType.VarChar, 255) {
			Direction = ParameterDirection.Input,
			Value = name
			},

			new("?ISOCode2", MySqlDbType.VarChar, 2) {
			Direction = ParameterDirection.Input,
			Value = iSOCode2
			},

			new("?ISOCode3", MySqlDbType.VarChar, 3) {
			Direction = ParameterDirection.Input,
			Value = iSOCode3
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_GeoCountry table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid guid)
	{
		string sqlCommand = @"
DELETE FROM mp_GeoCountry 
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
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_GeoCountry table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetOne(Guid guid)
	{
		string sqlCommand = @"
SELECT  * 
FROM mp_GeoCountry 
WHERE Guid = ?Guid;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			}
		};


		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_GeoCountry table.
	/// </summary>
	/// <param name="countryISOCode2"> countryISOCode2 </param>
	public static IDataReader GetByISOCode2(string countryISOCode2)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_GeoCountry 
WHERE ISOCode2 = ?ISOCode2;";

		var arParams = new List<MySqlParameter>
		{
			new("?ISOCode2", MySqlDbType.VarChar, 2)
			{
				Direction = ParameterDirection.Input,
				Value = countryISOCode2
			}
		};


		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with all rows in the mp_GeoCountry table.
	/// </summary>
	public static IDataReader GetAll()
	{
		string sqlCommand = @"
SELECT * 
FROM mp_GeoCountry 
ORDER BY Name;";

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString());
	}

	/// <summary>
	/// Gets a count of rows in the mp_GeoCountry table.
	/// </summary>
	public static int GetCount()
	{
		string sqlCommand = @"
SELECT  Count(*) 
FROM mp_GeoCountry;";

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString()));
	}

	/// <summary>
	/// Gets a page of data from the mp_GeoCountry table.
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
SELECT * 
FROM mp_GeoCountry  
ORDER BY Name  
LIMIT ?Offset, ?PageSize;";

		var arParams = new List<MySqlParameter>
		{
			new("?Offset", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageLowerBound
			},

			new("?PageSize", MySqlDbType.Int32) {
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
