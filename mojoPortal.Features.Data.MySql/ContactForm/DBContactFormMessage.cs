using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;


namespace mojoPortal.Data;

public static class DBContactFormMessage
{

	/// <summary>
	/// Inserts a row in the mp_ContactFormMessage table. Returns rows affected count.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <param name="email"> email </param>
	/// <param name="url"> url </param>
	/// <param name="subject"> subject </param>
	/// <param name="message"> message </param>
	/// <param name="createdUtc"> createdUtc </param>
	/// <param name="createdFromIpAddress"> createdFromIpAddress </param>
	/// <param name="userGuid"> userGuid </param>
	/// <returns>int</returns>
	public static int Create(
		Guid rowGuid,
		Guid siteGuid,
		Guid moduleGuid,
		string email,
		string url,
		string subject,
		string message,
		DateTime createdUtc,
		string createdFromIpAddress,
		Guid userGuid)
	{
		string sqlCommand = @"
INSERT INTO mp_ContactFormMessage (
    RowGuid, 
    SiteGuid, 
    ModuleGuid, 
    Email, 
    Url, 
    Subject, 
    Message, 
    CreatedUtc, 
    CreatedFromIpAddress, 
    UserGuid 
)
VALUES (
    ?RowGuid, 
    ?SiteGuid, 
    ?ModuleGuid, 
    ?Email, 
    ?Url, 
    ?Subject, 
    ?Message, 
    ?CreatedUtc, 
    ?CreatedFromIpAddress, 
    ?UserGuid 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			},

			new("?Email", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = email
			},

			new("?Url", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = url
			},

			new("?Subject", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = subject
			},

			new("?Message", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = message
			},

			new("?CreatedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdUtc
			},

			new("?CreatedFromIpAddress", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = createdFromIpAddress
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}


	/// <summary>
	/// Updates a row in the mp_ContactFormMessage table. Returns true if row updated.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <param name="email"> email </param>
	/// <param name="url"> url </param>
	/// <param name="subject"> subject </param>
	/// <param name="message"> message </param>
	/// <param name="createdUtc"> createdUtc </param>
	/// <param name="createdFromIpAddress"> createdFromIpAddress </param>
	/// <param name="userGuid"> userGuid </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid rowGuid,
		Guid siteGuid,
		Guid moduleGuid,
		string email,
		string url,
		string subject,
		string message,
		DateTime createdUtc,
		string createdFromIpAddress,
		Guid userGuid)
	{
		string sqlCommand = @"
UPDATE mp_ContactFormMessage 
SET  
    SiteGuid = ?SiteGuid,
    ModuleGuid = ?ModuleGuid,
    Email = ?Email,
    Url = ?Url,
    Subject = ?Subject,
    Message = ?Message,
    CreatedUtc = ?CreatedUtc,
    CreatedFromIpAddress = ?CreatedFromIpAddress,
    UserGuid = ?UserGuid 
WHERE 
RowGuid = ?RowGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			},

			new("?Email", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = email
			},

			new("?Url", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = url
			},

			new("?Subject", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = subject
			},

			new("?Message", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = message
			},

			new("?CreatedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdUtc
			},

			new("?CreatedFromIpAddress", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = createdFromIpAddress
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_ContactFormMessage table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid rowGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_ContactFormMessage 
WHERE RowGuid = ?RowGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	public static bool DeleteByModule(Guid moduleGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_ContactFormMessage 
WHERE ModuleGuid = ?ModuleGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	public static bool DeleteBySite(int siteId)
	{
		string sqlCommand = @"
DELETE FROM mp_ContactFormMessage 
WHERE SiteGuid IN (
    SELECT SiteGuid 
    FROM mp_Sites 
    WHERE SiteID = ?SiteID
) ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;
	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_ContactFormMessage table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static IDataReader GetOne(
		Guid rowGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_ContactFormMessage 
WHERE RowGuid = ?RowGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}


	public static int GetCount(Guid moduleGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROMmp_ContactFormMessage 
WHERE ModuleGuid = ?ModuleGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));
	}

	/// <summary>
	/// Gets a page of data from the mp_ContactFormMessage table.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static IDataReader GetPage(
		Guid moduleGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCount(moduleGuid);

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

		string sqlCommand = $@"
SELECT * 
FROM mp_ContactFormMessage  
WHERE ModuleGuid = ?ModuleGuid 
ORDER BY CreatedUtc DESC 
LIMIT {pageLowerBound.ToString(CultureInfo.InvariantCulture)} 
{pageSize.ToString(CultureInfo.InvariantCulture)} 
 ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}
}
