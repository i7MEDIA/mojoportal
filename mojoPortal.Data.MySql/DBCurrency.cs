using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;

public static class DBCurrency
{

	/// <summary>
	/// Inserts a row in the mp_Currency table. Returns rows affected count.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="title"> title </param>
	/// <param name="code"> code </param>
	/// <param name="symbolLeft"> symbolLeft </param>
	/// <param name="symbolRight"> symbolRight </param>
	/// <param name="decimalPointChar"> decimalPointChar </param>
	/// <param name="thousandsPointChar"> thousandsPointChar </param>
	/// <param name="decimalPlaces"> decimalPlaces </param>
	/// <param name="value"> value </param>
	/// <param name="lastModified"> lastModified </param>
	/// <param name="created"> created </param>
	/// <returns>int</returns>
	public static int Create(
		Guid guid,
		string title,
		string code,
		string symbolLeft,
		string symbolRight,
		string decimalPointChar,
		string thousandsPointChar,
		string decimalPlaces,
		decimal value,
		DateTime lastModified,
		DateTime created)
	{

		string sqlCommand = @"
INSERT INTO mp_Currency (
        Guid, 
        Title, 
        Code, 
        SymbolLeft, 
        SymbolRight, 
        DecimalPointChar, 
        ThousandsPointChar, 
        DecimalPlaces, 
        Value, 
        LastModified, 
        Created 
    ) 
VALUES (
    ?Guid, 
    ?Title, 
    ?Code, 
    ?SymbolLeft, 
    ?SymbolRight, 
    ?DecimalPointChar, 
    ?ThousandsPointChar, 
    ?DecimalPlaces, 
    ?Value, 
    ?LastModified, 
    ?Created 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?Title", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},

			new("?Code", MySqlDbType.VarChar, 3)
			{
				Direction = ParameterDirection.Input,
				Value = code
			},

			new("?SymbolLeft", MySqlDbType.VarChar, 15)
			{
				Direction = ParameterDirection.Input,
				Value = symbolLeft
			},

			new("?SymbolRight", MySqlDbType.VarChar, 15)
			{
				Direction = ParameterDirection.Input,
				Value = symbolRight
			},

			new("?DecimalPointChar", MySqlDbType.VarChar, 1)
			{
				Direction = ParameterDirection.Input,
				Value = decimalPointChar
			},

			new("?ThousandsPointChar", MySqlDbType.VarChar, 1)
			{
				Direction = ParameterDirection.Input,
				Value = thousandsPointChar
			},

			new("?DecimalPlaces", MySqlDbType.VarChar, 1)
			{
				Direction = ParameterDirection.Input,
				Value = decimalPlaces
			},

			new("?Value", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = value
			},

			new("?LastModified", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastModified
			},

			new("?Created", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = created
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}


	/// <summary>
	/// Updates a row in the mp_Currency table. Returns true if row updated.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="title"> title </param>
	/// <param name="code"> code </param>
	/// <param name="symbolLeft"> symbolLeft </param>
	/// <param name="symbolRight"> symbolRight </param>
	/// <param name="decimalPointChar"> decimalPointChar </param>
	/// <param name="thousandsPointChar"> thousandsPointChar </param>
	/// <param name="decimalPlaces"> decimalPlaces </param>
	/// <param name="value"> value </param>
	/// <param name="lastModified"> lastModified </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid guid,
		string title,
		string code,
		string symbolLeft,
		string symbolRight,
		string decimalPointChar,
		string thousandsPointChar,
		string decimalPlaces,
		decimal value,
		DateTime lastModified)
	{

		string sqlCommand = @"
UPDATE 
    mp_Currency 
SET  
    Title = ?Title, 
    Code = ?Code, 
    SymbolLeft = ?SymbolLeft, 
    SymbolRight = ?SymbolRight, 
    DecimalPointChar = ?DecimalPointChar, 
    ThousandsPointChar = ?ThousandsPointChar, 
    DecimalPlaces = ?DecimalPlaces, 
    Value = ?Value, 
    LastModified = ?LastModified 
WHERE  
    Guid = ?Guid;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?Title", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},

			new("?Code", MySqlDbType.VarChar, 3)
			{
				Direction = ParameterDirection.Input,
				Value = code
			},

			new("?SymbolLeft", MySqlDbType.VarChar, 15)
			{
				Direction = ParameterDirection.Input,
				Value = symbolLeft
			},

			new("?SymbolRight", MySqlDbType.VarChar, 15)
			{
				Direction = ParameterDirection.Input,
				Value = symbolRight
			},

			new("?DecimalPointChar", MySqlDbType.VarChar, 1)
			{
				Direction = ParameterDirection.Input,
				Value = decimalPointChar
			},

			new("?ThousandsPointChar", MySqlDbType.VarChar, 1)
			{
				Direction = ParameterDirection.Input,
				Value = thousandsPointChar
			},

			new("?DecimalPlaces", MySqlDbType.VarChar, 1)
			{
				Direction = ParameterDirection.Input,
				Value = decimalPlaces
			},

			new("?Value", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = value
			},

			new("?LastModified", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastModified
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_Currency table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid guid)
	{
		string sqlCommand = @"
DELETE FROM mp_Currency 
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
	/// Gets an IDataReader with one row from the mp_Currency table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetOne(Guid guid)
	{
		string sqlCommand = @"
SELECT  * 
FROM mp_Currency 
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
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with all rows in the mp_Currency table.
	/// </summary>
	public static IDataReader GetAll()
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Currency;";

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString());
	}
}
