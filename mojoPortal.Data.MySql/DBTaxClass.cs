using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;

public static class DBTaxClass
{


	/// <summary>
	/// Inserts a row in the mp_TaxClass table. Returns rows affected count.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="title"> title </param>
	/// <param name="description"> description </param>
	/// <param name="lastModified"> lastModified </param>
	/// <param name="created"> created </param>
	/// <returns>int</returns>
	public static int Create(
		Guid guid,
		Guid siteGuid,
		string title,
		string description,
		DateTime lastModified,
		DateTime created)
	{

		string sqlCommand = @"
INSERT INTO mp_TaxClass (
    Guid, 
    SiteGuid, 
    Title, 
    Description, 
    LastModified, 
    Created 
) 
VALUES (
    ?Guid, 
    ?SiteGuid, 
    ?Title, 
    ?Description, 
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

			new("?Description", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = description
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
	/// Updates a row in the mp_TaxClass table. Returns true if row updated.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="title"> title </param>
	/// <param name="description"> description </param>
	/// <param name="lastModified"> lastModified </param>
	/// <param name="created"> created </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid guid,
		string title,
		string description,
		DateTime lastModified,
		DateTime created)
	{

		string sqlCommand = @"
UPDATE mp_TaxClass 
SET  
    Title = ?Title, 
    Description = ?Description, 
    LastModified = ?LastModified, 
    Created = ?Created 
WHERE Guid = ?Guid ;";

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

			new("?Description", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = description
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

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_TaxClass table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid guid)
	{
		string sqlCommand = @"
DELETE FROM mp_TaxClass 
WHERE Guid = ?Guid ;";

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
	/// Gets an IDataReader with one row from the mp_TaxClass table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetOne(Guid guid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_TaxClass 
WHERE Guid = ?Guid ;";

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
	/// Gets an IDataReader with one row from the mp_TaxClass table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetBySite(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_TaxClass 
WHERE SiteGuid = ?SiteGuid 
ORDER BY Title ;";

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
	/// Gets a count of rows in the mp_TaxClass table.
	/// </summary>
	public static int GetCountBySite(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_TaxClass 
WHERE SiteGuid = ?SiteGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));
	}

	/// <summary>
	/// Gets a page of data from the mp_TaxClass table.
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
		int totalRows = GetCountBySite(siteGuid);

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
FROM	mp_TaxClass  
WHERE 
SiteGuid = ?SiteGuid 
ORDER BY Title 
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ";";

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
