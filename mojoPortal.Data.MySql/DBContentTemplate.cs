using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;


public static class DBContentTemplate
{
	/// <summary>
	/// Inserts a row in the mp_ContentTemplate table. Returns rows affected count.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="title"> title </param>
	/// <param name="imageFileName"> imageFileName </param>
	/// <param name="description"> description </param>
	/// <param name="body"> body </param>
	/// <param name="allowedRoles"> allowedRoles </param>
	/// <param name="createdByUser"> createdByUser </param>
	/// <param name="createdUtc"> createdUtc </param>
	/// <returns>int</returns>
	public static int Create(
		Guid guid,
		Guid siteGuid,
		string title,
		string imageFileName,
		string description,
		string body,
		string allowedRoles,
		Guid createdByUser,
		DateTime createdUtc)
	{
		string sqlCommand = @"
INSERT INTO 
    mp_ContentTemplate (
        Guid, 
        SiteGuid, 
        Title, 
        ImageFileName, 
        Description, 
        Body, 
        AllowedRoles, 
        CreatedByUser, 
        LastModUser, 
        CreatedUtc, 
        LastModUtc 
    ) 
VALUES (
    ?Guid, 
    ?SiteGuid, 
    ?Title, 
    ?ImageFileName, 
    ?Description, 
    ?Body, 
    ?AllowedRoles, 
    ?CreatedByUser, 
    ?LastModUser, 
    ?CreatedUtc, 
    ?LastModUtc 
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

			new("?ImageFileName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = imageFileName
			},

			new("?Description", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?Body", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = body
			},

			new("?AllowedRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = allowedRoles
			},

			new("?CreatedByUser", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = createdByUser.ToString()
			},

			new("?LastModUser", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = createdByUser.ToString()
			},

			new("?CreatedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdUtc
			},

			new("?LastModUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdUtc
			},
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}


	/// <summary>
	/// Updates a row in the mp_ContentTemplate table. Returns true if row updated.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="title"> title </param>
	/// <param name="imageFileName"> imageFileName </param>
	/// <param name="description"> description </param>
	/// <param name="body"> body </param>
	/// <param name="allowedRoles"> allowedRoles </param>
	/// <param name="lastModUser"> lastModUser </param>
	/// <param name="lastModUtc"> lastModUtc </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid guid,
		Guid siteGuid,
		string title,
		string imageFileName,
		string description,
		string body,
		string allowedRoles,
		Guid lastModUser,
		DateTime lastModUtc)
	{
		string sqlCommand = @"
UPDATE 
    mp_ContentTemplate 
SET  
    SiteGuid = ?SiteGuid, 
    Title = ?Title, 
    ImageFileName = ?ImageFileName, 
    Description = ?Description, 
    Body = ?Body, 
    AllowedRoles = ?AllowedRoles, 
    LastModUser = ?LastModUser, 
    LastModUtc = ?LastModUtc 
WHERE  
    Guid = ?Guid;";

		var arParams = new List<MySqlParameter>
		{

			new ("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new ("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new ("?Title", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},

			new ("?ImageFileName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = imageFileName
			},

			new ("?Description", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new ("?Body", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = body
			},

			new ("?AllowedRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = allowedRoles
			},

			new ("?LastModUser", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = lastModUser.ToString()
			},

			new ("?LastModUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastModUtc
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_ContentTemplate table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid guid)
	{
		string sqlCommand = @"
DELETE FROM mp_ContentTemplate 
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
	/// Gets an IDataReader with one row from the mp_ContentTemplate table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetOne(Guid guid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_ContentTemplate 
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
	/// Gets a count of rows in the mp_ContentTemplate table.
	/// </summary>
	public static int GetCount(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_ContentTemplate 
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
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

	}

	/// <summary>
	/// Gets an IDataReader with all rows in the mp_ContentTemplate table.
	/// </summary>
	public static IDataReader GetAll(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_ContentTemplate 
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
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets a page of data from the mp_ContentTemplate table.
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
SELECT	* 
FROM	mp_ContentTemplate  
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
