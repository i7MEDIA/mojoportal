using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;


public static class DBContentRating
{


	/// <summary>
	/// Inserts a row in the mp_ContentRating table. Returns rows affected count.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="contentGuid"> contentGuid </param>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="emailAddress"> emailAddress </param>
	/// <param name="rating"> rating </param>
	/// <param name="comments"> comments </param>
	/// <param name="ipAddress"> ipAddress </param>
	/// <param name="createdUtc"> createdUtc </param>
	/// <returns>int</returns>
	public static int Create(
		Guid rowGuid,
		Guid siteGuid,
		Guid contentGuid,
		Guid userGuid,
		string emailAddress,
		int rating,
		string comments,
		string ipAddress,
		DateTime createdUtc)
	{
		string sqlCommand = @"
INSERT INTO 
	mp_ContentRating (
		RowGuid, 
		SiteGuid, 
		ContentGuid, 
		UserGuid, 
		EmailAddress, 
		Rating, 
		Comments, 
		IpAddress, 
		CreatedUtc, 
		LastModUtc 
	)
VALUES (
	?RowGuid, 
	?SiteGuid, 
	?ContentGuid, 
	?UserGuid, 
	?EmailAddress, 
	?Rating, 
	?Comments, 
	?IpAddress, 
	?CreatedUtc, 
	?CreatedUtc 
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


			new("?ContentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = contentGuid.ToString()
			},


			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},


			new("?EmailAddress", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = emailAddress
			},


			new("?Rating", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = rating
			},


			new("?Comments", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = comments
			},


			new("?IpAddress", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = ipAddress
			},


			new("?CreatedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdUtc
			},

		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected;

	}

	/// <summary>
	/// Updates a row in the mp_ContentRating table. Returns true if row updated.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <param name="emailAddress"> emailAddress </param>
	/// <param name="rating"> rating </param>
	/// <param name="comments"> comments </param>
	/// <param name="ipAddress"> ipAddress </param>
	/// <param name="lastModUtc"> lastModUtc </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid rowGuid,
		string emailAddress,
		int rating,
		string comments,
		string ipAddress,
		DateTime lastModUtc)
	{
		string sqlCommand = @"
UPDATE 
	mp_ContentRating 
SET  
	EmailAddress = ?EmailAddress, 
	Rating = ?Rating, 
	Comments = ?Comments, 
	IpAddress = ?IpAddress, 
	LastModUtc = ?LastModUtc 
WHERE  
	RowGuid = ?RowGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
		{
			Direction = ParameterDirection.Input,
			Value = rowGuid.ToString()
		},

			new("?EmailAddress", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = emailAddress
			},

			new("?Rating", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = rating
			},

			new("?Comments", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = comments
			},

			new("?IpAddress", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = ipAddress
			},

			new("?LastModUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastModUtc
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_ContentRating table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid rowGuid)
	{
		string sqlCommand = @"
DELETE FROM 
	mp_ContentRating 
WHERE 
	RowGuid = ?RowGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	/// <summary>
	/// Deletes a row from the mp_ContentRating table. Returns true if row deleted.
	/// </summary>
	/// <param name="contentGuid"> contentGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteByContent(Guid contentGuid)
	{
		string sqlCommand = @"
DELETE FROM 
	mp_ContentRating 
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
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	/// <summary>
	/// Deletes a row from the mp_ContentRating table. Returns true if row deleted.
	/// </summary>
	/// <param name="siteGuid"> siteGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteBySite(Guid siteGuid)
	{
		string sqlCommand = @"
DELETE FROM 
	mp_ContentRating 
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
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	/// <summary>
	/// Deletes a row from the mp_ContentRating table. Returns true if row deleted.
	/// </summary>
	/// <param name="userGuid"> userGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteByUser(Guid userGuid)
	{
		string sqlCommand = @"
DELETE FROM 
	mp_ContentRating 
WHERE 
	UserGuid = ?UserGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_ContentRating table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static IDataReader GetOne(Guid rowGuid)
	{
		string sqlCommand = @"
SELECT 
	* 
FROM 
	mp_ContentRating 
WHERE 
	RowGuid = ?RowGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			}
		};


		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_ContentRating table.
	/// </summary>
	/// <param name="contentGuid"> contentGuid </param>
	/// <param name="userGuid"> userGuid </param>
	public static IDataReader GetOneByContentAndUser(Guid contentGuid, Guid userGuid)
	{
		string sqlCommand = @"
SELECT 
	* 
FROM 
	mp_ContentRating 
WHERE 
	ContentGuid = ?ContentGuid 
	AND UserGuid = ?UserGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?ContentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = contentGuid.ToString()
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};


		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_ContentRating table.
	/// </summary>
	/// <param name="contentGuid"> contentGuid </param>
	public static IDataReader GetStatsByContent(Guid contentGuid)
	{
		string sqlCommand = @"
SELECT 
	COALESCE(AVG(Rating),0) As CurrentRating, 
	Count(*) As TotalRatings 
FROM 
	mp_ContentRating 
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


		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);


	}

	/// <summary>
	/// Gets a count of rows in the mp_ContentRating table.
	/// </summary>
	public static int GetCountByContent(Guid contentGuid)
	{
		string sqlCommand = @"
SELECT 
	Count(*) 
FROM 
	mp_ContentRating 
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
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

	}

	/// <summary>
	/// Gets a count of rows in the mp_ContentRating table.
	/// </summary>
	public static int GetCountByContentAndUser(Guid contentGuid, Guid userGuid)
	{

		string sqlCommand = @"
SELECT 
	Count(*) 
FROM 
	mp_ContentRating 
WHERE 
	ContentGuid = ?ContentGuid 
	AND UserGuid = ?UserGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?ContentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = contentGuid.ToString()
			},


			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};


		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

	}

	/// <summary>
	/// Gets a count of rows in the mp_ContentRating table.
	/// </summary>
	public static int GetCountOfRatingsSince(Guid contentGuid, string ipAddress, DateTime beginUtc)
	{
		string sqlCommand = @"
SELECT 
	Count(*) 
FROM 
	mp_ContentRating 
WHERE 
	ContentGuid = ?ContentGuid 
	AND UserGuid = '00000000-0000-0000-0000-000000000000' 
	AND IpAddress = ?IpAddress 
	AND CreatedUtc > ?BeginUtc;";

		var arParams = new List<MySqlParameter>
		{
			new("?ContentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = contentGuid.ToString()
			},

			new("?IpAddress", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = ipAddress
			},

			new("?BeginUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = beginUtc
			}
		};


		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

	}

	/// <summary>
	/// Gets a page of data from the mp_ContentRating table.
	/// </summary>
	/// <param name="contentGuid">contentGuid</param>
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
		int totalRows = GetCountByContent(contentGuid);

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
FROM	mp_ContentRating  
WHERE 
ContentGuid = ?ContentGuid 
ORDER BY CreatedUtc DESC  
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
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}
}
