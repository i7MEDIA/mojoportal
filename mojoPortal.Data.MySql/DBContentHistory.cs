using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;


public static class DBContentHistory
{

	/// <summary>
	/// Inserts a row in the mp_ContentHistory table. Returns rows affected count.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="contentGuid"> contentGuid </param>
	/// <param name="title"> title </param>
	/// <param name="contentText"> contentText </param>
	/// <param name="customData"> customData </param>
	/// <param name="createdUtc"> createdUtc </param>
	/// <param name="historyUtc"> historyUtc </param>
	/// <returns>int</returns>
	public static int Create(
		Guid guid,
		Guid siteGuid,
		Guid userGuid,
		Guid contentGuid,
		string title,
		string contentText,
		string customData,
		DateTime createdUtc,
		DateTime historyUtc)
	{
		string sqlCommand = @"
INSERT INTO mp_ContentHistory(
	Guid, 
	SiteGuid, 
	UserGuid, 
	ContentGuid, 
	Title, 
	ContentText, 
	CustomData, 
	CreatedUtc, 
	HistoryUtc 
	)
VALUES (
	?Guid, 
	?SiteGuid, 
	?UserGuid, 
	?ContentGuid, 
	?Title, 
	?ContentText, 
	?CustomData, 
	?CreatedUtc, 
	?HistoryUtc 
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

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?ContentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = contentGuid.ToString()
			},

			new("?Title", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},

			new("?ContentText", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = contentText
			},

			new("?CustomData", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = customData
			},

			new("?CreatedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdUtc
			},

			new("?HistoryUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = historyUtc
			}
		};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}




	/// <summary>
	/// Deletes a row from the mp_ContentHistory table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid guid)
	{
		string sqlCommand = @"
DELETE FROM 
	mp_ContentHistory 
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

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	/// <summary>
	/// Deletes rows from the mp_ContentHistory table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> siteGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteBySite(Guid siteGuid)
	{
		string sqlCommand = @"
DELETE FROM 
	mp_ContentHistory 
WHERE 
	SiteGuid = ?SiteGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}
	};




		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return (rowsAffected > 0);
	}

	/// <summary>
	/// Deletes rows from the mp_ContentHistory table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> contentGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteByContent(Guid contentGuid)
	{
		string sqlCommand = @"
DELETE FROM 
	mp_ContentHistory 
WHERE 
	ContentGuid = ?ContentGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?ContentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = contentGuid.ToString()
			}

		};




		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return (rowsAffected > 0);

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_ContentHistory table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetOne(Guid guid)
	{
		string sqlCommand = @"
SELECT 
	ch.*, 
	u.Name, 
	u.LoginName, 
	u.Email 
FROM	
	mp_ContentHistory ch 
LEFT OUTER JOIN 
	mp_Users u ON u.UserGuid = ch.UserGuid 
WHERE 
	ch.Guid = ?Guid;";

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
	/// Gets a count of rows in the mp_ContentHistory table.
	/// </summary>
	public static int GetCount(Guid contentGuid)
	{
		string sqlCommand = @"
SELECT 
	Count(*) 
FROM 
	mp_ContentHistory 
WHERE 
	ContentGuid = ?ContentGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?ContentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = contentGuid.ToString()
			}
		};




		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));
	}

	/// <summary>
	/// Gets a page of data from the mp_ContentHistory table.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static IDataReader GetPage(
		Guid contentGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCount(contentGuid);

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
SELECT ch.*, 
u.Name, 
u.LoginName, 
u.Email 
FROM	mp_ContentHistory ch 
LEFT OUTER JOIN 
mp_Users u 
ON 
u.UserGuid = ch.UserGuid 
WHERE 
ch.ContentGuid = ?ContentGuid 
ORDER BY  
ch.HistoryUtc DESC  
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?ContentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = contentGuid.ToString()
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
