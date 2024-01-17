using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;


public static class DBUserLocation
{


	/// <summary>
	/// Inserts a row in the mp_UserLocation table. Returns rows affected count.
	/// </summary>
	/// <param name="rowID"> rowID </param>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="iPAddress"> iPAddress </param>
	/// <param name="iPAddressLong"> iPAddressLong </param>
	/// <param name="hostname"> hostname </param>
	/// <param name="longitude"> longitude </param>
	/// <param name="latitude"> latitude </param>
	/// <param name="iSP"> iSP </param>
	/// <param name="continent"> continent </param>
	/// <param name="country"> country </param>
	/// <param name="region"> region </param>
	/// <param name="city"> city </param>
	/// <param name="timeZone"> timeZone </param>
	/// <param name="captureCount"> captureCount </param>
	/// <param name="firstCaptureUTC"> firstCaptureUTC </param>
	/// <param name="lastCaptureUTC"> lastCaptureUTC </param>
	/// <returns>int</returns>
	public static int Create(
		Guid rowID,
		Guid userGuid,
		Guid siteGuid,
		string iPAddress,
		long iPAddressLong,
		string hostname,
		double longitude,
		double latitude,
		string iSP,
		string continent,
		string country,
		string region,
		string city,
		string timeZone,
		int captureCount,
		DateTime firstCaptureUTC,
		DateTime lastCaptureUTC)
	{
		#region Bit Conversion


		#endregion

		string sqlCommand = @"
INSERT INTO mp_UserLocation (
    RowID, 
    UserGuid, 
    SiteGuid, 
    IPAddress, 
    IPAddressLong, 
    Hostname, 
    Longitude, 
    Latitude, 
    ISP, 
    Continent, 
    Country, 
    Region, 
    City, 
    TimeZone, 
    CaptureCount, 
    FirstCaptureUTC, 
    LastCaptureUTC 
) 
VALUES (
    ?RowID, 
    ?UserGuid, 
    ?SiteGuid, 
    ?IPAddress, 
    ?IPAddressLong, 
    ?Hostname, 
    ?Longitude, 
    ?Latitude, 
    ?ISP, 
    ?Continent, 
    ?Country, 
    ?Region, 
    ?City, 
    ?TimeZone, 
    ?CaptureCount, 
    ?FirstCaptureUTC, 
    ?LastCaptureUTC 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?RowID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowID.ToString()
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?IPAddress", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = iPAddress
			},

			new("?IPAddressLong", MySqlDbType.Int64)
			{
				Direction = ParameterDirection.Input,
				Value = iPAddressLong
			},

			new("?Hostname", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = hostname
			},

			new("?Longitude", MySqlDbType.Float)
			{
				Direction = ParameterDirection.Input,
				Value = longitude
			},

			new("?Latitude", MySqlDbType.Float)
			{
				Direction = ParameterDirection.Input,
				Value = latitude
			},

			new("?ISP", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = iSP
			},

			new("?Continent", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = continent
			},

			new("?Country", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = country
			},

			new("?Region", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = region
			},

			new("?City", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = city
			},

			new("?TimeZone", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = timeZone
			},

			new("?CaptureCount", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = captureCount
			},

			new("?FirstCaptureUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = firstCaptureUTC
			},

			new("?LastCaptureUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastCaptureUTC
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}


	/// <summary>
	/// Updates a row in the mp_UserLocation table. Returns true if row updated.
	/// </summary>
	/// <param name="rowID"> rowID </param>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="iPAddress"> iPAddress </param>
	/// <param name="iPAddressLong"> iPAddressLong </param>
	/// <param name="hostname"> hostname </param>
	/// <param name="longitude"> longitude </param>
	/// <param name="latitude"> latitude </param>
	/// <param name="iSP"> iSP </param>
	/// <param name="continent"> continent </param>
	/// <param name="country"> country </param>
	/// <param name="region"> region </param>
	/// <param name="city"> city </param>
	/// <param name="timeZone"> timeZone </param>
	/// <param name="captureCount"> captureCount </param>
	/// <param name="lastCaptureUTC"> lastCaptureUTC </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid rowID,
		Guid userGuid,
		Guid siteGuid,
		string iPAddress,
		long iPAddressLong,
		string hostname,
		double longitude,
		double latitude,
		string iSP,
		string continent,
		string country,
		string region,
		string city,
		string timeZone,
		int captureCount,
		DateTime lastCaptureUTC)
	{
		#region Bit Conversion


		#endregion

		string sqlCommand = @"
UPDATE mp_UserLocation 
SET  
    UserGuid = ?UserGuid, 
    SiteGuid = ?SiteGuid, 
    IPAddress = ?IPAddress, 
    IPAddressLong = ?IPAddressLong, 
    Hostname = ?Hostname, 
    Longitude = ?Longitude, 
    Latitude = ?Latitude, 
    ISP = ?ISP, 
    Continent = ?Continent, 
    Country = ?Country, 
    Region = ?Region, 
    City = ?City, 
    TimeZone = ?TimeZone, 
    CaptureCount = ?CaptureCount, 
    LastCaptureUTC = ?LastCaptureUTC 
WHERE RowID = ?RowID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowID.ToString()
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?IPAddress", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = iPAddress
			},

			new("?IPAddressLong", MySqlDbType.Int64)
			{
				Direction = ParameterDirection.Input,
				Value = iPAddressLong
			},

			new("?Hostname", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = hostname
			},

			new("?Longitude", MySqlDbType.Float)
			{
				Direction = ParameterDirection.Input,
				Value = longitude
			},

			new("?Latitude", MySqlDbType.Float)
			{
				Direction = ParameterDirection.Input,
				Value = latitude
			},

			new("?ISP", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = iSP
			},

			new("?Continent", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = continent
			},

			new("?Country", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = country
			},

			new("?Region", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = region
			},

			new("?City", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = city
			},

			new("?TimeZone", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = timeZone
			},

			new("?CaptureCount", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = captureCount
			},

			new("?LastCaptureUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastCaptureUTC
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_UserLocation table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowID"> rowID </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid rowID)
	{
		string sqlCommand = @"
DELETE FROM mp_UserLocation 
WHERE RowID = ?RowID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowID.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	public static bool DeleteByUser(Guid userGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_UserLocation 
WHERE UserGuid = ?UserGuid ;";

		var arParams = new List<MySqlParameter>
		{
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
		return rowsAffected > 0;
	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_UserLocation table.
	/// </summary>
	/// <param name="rowID"> rowID </param>
	public static IDataReader GetOne(Guid rowID)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_UserLocation 
WHERE RowID = ?RowID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowID.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_UserLocation table.
	/// </summary>
	/// <param name="userguid"> userguid </param>
	/// <param name="iPAddress"> iPAddress </param>
	public static IDataReader GetOne(Guid userGuid, long iPAddressLong)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_UserLocation 
WHERE Userguid = ?Userguid 
AND IPAddressLong = ?IPAddressLong ;";

		var arParams = new List<MySqlParameter>
		{
			new("?Userguid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?IPAddressLong", MySqlDbType.Int64)
			{
				Direction = ParameterDirection.Input,
				Value = iPAddressLong
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_UserLocation table.
	/// </summary>
	/// <param name="userGuid"> userGuid </param>
	public static IDataReader GetByUser(Guid userGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_UserLocation 
WHERE UserGuid = ?UserGuid 
ORDER BY LastCaptureUTC DESC ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_UserLocation table.
	/// </summary>
	/// <param name="siteGuid"> siteGuid </param>
	public static IDataReader GetBySite(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_UserLocation 
WHERE SiteGuid = ?SiteGuid 
ORDER BY IPAddressLong ;";

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
	/// Gets an IDataReader with rows from the mp_Users table which have the passed in IP Address
	/// </summary>
	/// <param name="siteGuid"> siteGuid </param>
	public static IDataReader GetUsersByIPAddress(Guid siteGuid, string ipv4Address)
	{
		string sqlCommand = @"
SELECT u.* 
FROM mp_UserLocation ul 
JOIN mp_Users u 
ON ul.UserGuid = u.UserGuid 
WHERE (
    u.SiteGuid = ?SiteGuid OR ?SiteGuid = '00000000-0000-0000-0000-000000000000'
) 
AND ul.IPAddress = ?IPAddress 
ORDER BY ul.LastCaptureUTC DESC ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?IPAddress", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = ipv4Address
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets a count of rows in the mp_UserLocation table.
	/// </summary>
	/// <param name="userGuid"> userGuid </param>
	public static int GetCountByUser(Guid userGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_UserLocation 
WHERE UserGuid = ?UserGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));
	}

	/// <summary>
	/// Gets a count of rows in the mp_UserLocation table.
	/// </summary>
	/// <param name="siteGuid"> siteGuid </param>
	public static int GetCountBySite(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_UserLocation 
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
	/// Gets a page of data from the mp_UserLocation table.
	/// </summary>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static IDataReader GetPageByUser(
		Guid userGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCountByUser(userGuid);

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
FROM mp_UserLocation  
WHERE UserGuid = ?UserGuid 
ORDER BY IPAddressLong 
LIMIT " + pageLowerBound.ToString() + ", ?PageSize ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}

	/// <summary>
	/// Gets a page of data from the mp_UserLocation table.
	/// </summary>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static IDataReader GetPageBySite(
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
SELECT * 
FROM mp_UserLocation 
WHERE SiteGuid = ?SiteGuid 
ORDER BY IPAddressLong 
LIMIT " + pageLowerBound.ToString() + ", ?PageSize ;";

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
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}


}
