using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;

public static class DBRedirectList
{

	/// <summary>
	/// Inserts a row in the mp_RedirectList table. Returns rows affected count.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="siteId"> siteId </param>
	/// <param name="oldUrl"> oldUrl </param>
	/// <param name="newUrl"> newUrl </param>
	/// <param name="createdUtc"> createdUtc </param>
	/// <param name="expireUtc"> expireUtc </param>
	/// <returns>int</returns>
	public static int Create(
		Guid rowGuid,
		Guid siteGuid,
		int siteId,
		string oldUrl,
		string newUrl,
		DateTime createdUtc,
		DateTime expireUtc)
	{
		string sqlCommand = @"
INSERT INTO 
    mp_RedirectList (
        RowGuid, 
        SiteGuid, 
        siteId, 
        OldUrl, 
        NewUrl, 
        CreatedUtc, 
        ExpireUtc 
)
VALUES (
    ?RowGuid, 
    ?SiteGuid, 
    ?siteId, 
    ?OldUrl, 
    ?NewUrl, 
    ?CreatedUtc, 
    ?ExpireUtc 
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

			new("?siteId", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?OldUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = oldUrl
			},

			new("?NewUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = newUrl
			},

			new("?CreatedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdUtc
			},

			new("?ExpireUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = expireUtc
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}


	/// <summary>
	/// Updates a row in the mp_RedirectList table. Returns true if row updated.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <param name="oldUrl"> oldUrl </param>
	/// <param name="newUrl"> newUrl </param>
	/// <param name="expireUtc"> expireUtc </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid rowGuid,
		string oldUrl,
		string newUrl,
		DateTime expireUtc)
	{
		string sqlCommand = @"
UPDATE mp_RedirectList 
SET  
    OldUrl = ?OldUrl, 
    NewUrl = ?NewUrl, 
    ExpireUtc = ?ExpireUtc 
WHERE  
    RowGuid = ?RowGuid 
;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			},

			new("?OldUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = oldUrl
			},

			new("?NewUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = newUrl
			},

			new("?ExpireUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = expireUtc
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_RedirectList table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid rowGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_RedirectList 
WHERE RowGuid = ?RowGuid;";

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

	/// <summary>
	/// Gets an IDataReader with one row from the mp_RedirectList table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static IDataReader GetOne(Guid rowGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_RedirectList 
WHERE RowGuid = ?RowGuid;";

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

	/// <summary>
	/// Gets an IDataReader with one row from the mp_RedirectList table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static IDataReader GetBySiteAndUrl(int siteId, string oldUrl)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_RedirectList 
WHERE siteId = ?siteId 
AND OldUrl = ?OldUrl 
AND ExpireUtc < ?CurrentTime;";

		var arParams = new List<MySqlParameter>
		{
			new("?siteId", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?OldUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = oldUrl
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DateTime.UtcNow
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}


	/// <summary>
	/// returns true if the record exists
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static bool Exists(int siteId, string oldUrl)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_RedirectList 
WHERE siteId = ?siteId 
AND OldUrl = ?OldUrl;";

		var arParams = new List<MySqlParameter>
		{
			new("?siteId", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?OldUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = oldUrl
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

		return count > 0;

	}


	/// <summary>
	/// Gets a count of rows in the mp_RedirectList table.
	/// </summary>
	public static int GetCount(int siteId, string searchTerm = "")
	{
		var useSearch = !string.IsNullOrWhiteSpace(searchTerm);
		var sqlCommand = $@"SELECT  Count(*) 
				FROM	mp_RedirectList
				WHERE
				siteId = ?siteId
				{(useSearch ? "AND NewUrl LIKE ?SearchTerm OR OldUrl LIKE ?SearchTerm;" : ";")}";

		var sqlParams = new List<MySqlParameter>
			{
				new MySqlParameter("?siteId", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = siteId
				}
			};

		if (useSearch)
		{
			sqlParams.Add(
				new MySqlParameter("?SearchTerm", MySqlDbType.VarChar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = "%" + searchTerm + "%"
				}
			);
		}

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			sqlParams.ToArray()));
	}

	/// <summary>
	/// Gets a page of data from the mp_RedirectList table with search term.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	/// <param name="searchTerm">search term</param>
	public static IDataReader GetPage(
		int siteId,
		int pageNumber,
		int pageSize,
		out int totalPages,
		string searchTerm = "")
	{
		var useSearch = !string.IsNullOrWhiteSpace(searchTerm);
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCount(siteId, searchTerm);

		if (pageSize > 0) totalPages = totalRows / pageSize;

		if (totalRows <= pageSize)
		{
			totalPages = 1;
		}
		else
		{
			Math.DivRem(totalRows, pageSize, out int remainder);
			if (remainder > 0)
			{
				totalPages += 1;
			}
		}

		var sqlCommand = $@"SELECT	* 
				FROM	mp_RedirectList  
				WHERE siteId = ?siteId 
				{(useSearch ? "AND NewUrl LIKE ?SearchTerm OR OldUrl LIKE ?SearchTerm" : "")}
				ORDER BY OldUrl 
				LIMIT ?PageSize 
				{(pageNumber > 1 ? "OFFSET ?OffsetRows;" : ";")}";

		var sqlParams = new List<MySqlParameter>
			{
				new MySqlParameter("?siteId", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = siteId
				},
				new MySqlParameter("?PageSize", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = pageSize
				},
				new MySqlParameter("?OffsetRows", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = pageLowerBound
				}
			};

		if (useSearch)
		{
			sqlParams.Add(
				new MySqlParameter("?SearchTerm", MySqlDbType.VarChar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = "%" + searchTerm + "%"
				}
			);
		}

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			sqlParams.ToArray());


	}

}
