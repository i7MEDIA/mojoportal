using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;

public static class DBLetterHtmlTemplate
{

	/// <summary>
	/// Inserts a row in the mp_LetterHtmlTemplate table. Returns rows affected count.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="title"> title </param>
	/// <param name="html"> html </param>
	/// <param name="lastModUTC"> lastModUTC </param>
	/// <returns>int</returns>
	public static int Create(
		Guid guid,
		Guid siteGuid,
		string title,
		string html,
		DateTime lastModUTC)
	{
		#region Bit Conversion


		#endregion

		string sqlCommand = @"
INSERT INTO 
    mp_LetterHtmlTemplate (
        Guid, 
        SiteGuid, 
        Title, 
        Html, 
        LastModUTC 
    )
VALUES (
    ?Guid, 
    ?SiteGuid, 
    ?Title, 
    ?Html, 
    ?LastModUTC 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?Title", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},

			new("?Html", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = html
			},

			new("?LastModUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastModUTC
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}


	/// <summary>
	/// Updates a row in the mp_LetterHtmlTemplate table. Returns true if row updated.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="title"> title </param>
	/// <param name="html"> html </param>
	/// <param name="lastModUTC"> lastModUTC </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid guid,
		string title,
		string html,
		DateTime lastModUTC)
	{
		#region Bit Conversion


		#endregion

		string sqlCommand = @"
UPDATE 
    mp_LetterHtmlTemplate 
SET 
    Title = ?Title, 
    Html = ?Html, 
    LastModUTC = ?LastModUTC 
WHERE 
Guid = ?Guid;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?Title", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},

			new("?Html", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = html
			},

			new("?LastModUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastModUTC
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_LetterHtmlTemplate table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool Delete(
		Guid guid)
	{
		string sqlCommand = @"
DELETE FROM mp_LetterHtmlTemplate 
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
	/// Gets an IDataReader with one row from the mp_LetterHtmlTemplate table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetOne(
		Guid guid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_LetterHtmlTemplate 
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
	/// Gets an IDataReader with all rows in the mp_LetterHtmlTemplate table.
	/// </summary>
	public static IDataReader GetAll(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_LetterHtmlTemplate 
WHERE SiteGuid = ?SiteGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);
	}

	/// <summary>
	/// Gets a count of rows in the mp_LetterHtmlTemplate table.
	/// </summary>
	public static int GetCount(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_LetterHtmlTemplate 
WHERE SiteGuid = ?SiteGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));
	}

	/// <summary>
	/// Gets a page of data from the mp_LetterHtmlTemplate table.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static IDataReader GetPage(
		Guid siteGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCount(siteGuid);

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
FROM mp_LetterHtmlTemplate  
WHERE SiteGuid = ?SiteGuid 
LIMIT " + pageLowerBound.ToString() + ", ?PageSize;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
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
