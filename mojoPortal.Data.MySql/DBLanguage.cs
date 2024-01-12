using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;

public static class DBLanguage
{

	/// <summary>
	/// Inserts a row in the mp_Language table. Returns rows affected count.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="name"> name </param>
	/// <param name="code"> code </param>
	/// <param name="sort"> sort </param>
	/// <returns>int</returns>
	public static int Create(
		Guid guid,
		string name,
		string code,
		int sort)
	{

		string sqlCommand = @"
INSERT INTO mp_Language (
Guid, 
Name, 
Code, 
Sort )

 VALUES (
?Guid, 
?Name, 
?Code, 
?Sort )
;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?Name", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = name
			},

			new("?Code", MySqlDbType.VarChar, 2)
			{
				Direction = ParameterDirection.Input,
				Value = code
			},

			new("?Sort", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sort
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected;

	}


	/// <summary>
	/// Updates a row in the mp_Language table. Returns true if row updated.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="name"> name </param>
	/// <param name="code"> code </param>
	/// <param name="sort"> sort </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid guid,
		string name,
		string code,
		int sort)
	{


		string sqlCommand = @"
UPDATE 
    mp_Language 
SET  
    Name = ?Name, 
    Code = ?Code, 
    Sort = ?Sort 
WHERE  
    Guid = ?Guid;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?Name", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = name
			},

			new("?Code", MySqlDbType.VarChar, 2)
			{
				Direction = ParameterDirection.Input,
				Value = code
			},

			new("?Sort", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sort
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_Language table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid guid)
	{
		string sqlCommand = @"
DELETE FROM mp_Language 
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
	/// Gets an IDataReader with one row from the mp_Language table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetOne(Guid guid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Language 
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
	/// Gets an IDataReader with all rows in the mp_Language table.
	/// </summary>
	public static IDataReader GetAll()
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Language 
ORDER BY Sort;";

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString());
	}

	/// <summary>
	/// Gets a count of rows in the mp_Language table.
	/// </summary>
	public static int GetCount()
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Language;";

		return Convert.ToInt32(
			CommandHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString()
			)
		);
	}

	/// <summary>
	/// Gets a page of data from the mp_Language table.
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
FROM mp_Language  
ORDER BY Sort 
LIMIT ?PageSize";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}
		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			},

			new("?OffsetRows", MySqlDbType.Int32)
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
