using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data
{
	public static class DBSystemLog
	{

		/// <summary>
		/// Inserts a row in the mp_SystemLog table. Returns new integer id.
		/// </summary>
		/// <param name="logDate"> logDate </param>
		/// <param name="ipAddress"> ipAddress </param>
		/// <param name="culture"> culture </param>
		/// <param name="url"> url </param>
		/// <param name="shortUrl"> shortUrl </param>
		/// <param name="thread"> thread </param>
		/// <param name="logLevel"> logLevel </param>
		/// <param name="logger"> logger </param>
		/// <param name="message"> message </param>
		/// <returns>int</returns>
		public static int Create(
			DateTime logDate,
			string ipAddress,
			string culture,
			string url,
			string shortUrl,
			string thread,
			string logLevel,
			string logger,
			string message)
		{
			string sqlCommand = @"
INSERT INTO mp_SystemLog (
    LogDate, 
    IpAddress, 
    Culture, 
    Url, 
    ShortUrl, 
    Thread, 
    LogLevel, 
    Logger, 
    Message )
     VALUES (
    ?LogDate, 
    ?IpAddress, 
    ?Culture, 
    ?Url, 
    ?ShortUrl, 
    ?Thread, 
    ?LogLevel, 
    ?Logger, 
    ?Message 
); 
SELECT LAST_INSERT_ID();";

			var arParams = new List<MySqlParameter>
			{
				new("?LogDate", MySqlDbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = logDate
				},

				new("?IpAddress", MySqlDbType.VarChar, 50)
				{
					Direction = ParameterDirection.Input,
					Value = ipAddress
				},

				new("?Culture", MySqlDbType.VarChar, 10)
				{
					Direction = ParameterDirection.Input,
					Value = culture
				},

				new("?Url", MySqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = url
				},

				new("?ShortUrl", MySqlDbType.VarChar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = shortUrl
				},

				new("?Thread", MySqlDbType.VarChar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = thread
				},

				new("?LogLevel", MySqlDbType.VarChar, 20) {
				Direction = ParameterDirection.Input,
				Value = logLevel
				},

				new("?Logger", MySqlDbType.VarChar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = logger
				},

				new("?Message", MySqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = message
				}
			};

			int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
				ConnectionString.GetWrite(),
				sqlCommand.ToString(),
				arParams).ToString());

			return newID;

		}

		/// <summary>
		/// Deletes rows from the mp_SystemLog table. Returns true if rows deleted.
		/// </summary>
		public static void DeleteAll()
		{
			//TODO: using TRUNCATE Table might be more efficient but possibly will cuase errors in some installations
			//http://dev.mysql.com/doc/refman/5.1/en/truncate-table.html

			string sqlCommand = @"
DELETE FROM mp_SystemLog ;";

			CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWrite(),
				sqlCommand.ToString());

		}

		/// <summary>
		/// Deletes a row from the mp_SystemLog table. Returns true if row deleted.
		/// </summary>
		/// <param name="id"> id </param>
		/// <returns>bool</returns>
		public static bool Delete(int id)
		{
			string sqlCommand = @"
DELETE FROM mp_SystemLog 
WHERE ID = ?ID ;";

			var arParams = new List<MySqlParameter>
			{
				new("?ID", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = id
				}
			};

			int rowsAffected = CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWrite(),
				sqlCommand.ToString(),
				arParams);

			return rowsAffected > 0;
		}

		/// <summary>
		/// Deletes rows from the mp_SystemLog table. Returns true if rows deleted.
		/// </summary>
		/// <param name="id"> id </param>
		/// <returns>bool</returns>
		public static bool DeleteOlderThan(DateTime cutoffDate)
		{
			string sqlCommand = @"
DELETE FROM mp_SystemLog 
WHERE LogDate < ?CutoffDate ;";

			var arParams = new List<MySqlParameter>
			{
				new("?CutoffDate", MySqlDbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = cutoffDate
				}
			};

			int rowsAffected = CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWrite(),
				sqlCommand.ToString(),
				arParams);

			return rowsAffected > 0;

		}

		/// <summary>
		/// Deletes rows from the mp_SystemLog table. Returns true if rows deleted.
		/// </summary>
		/// <param name="id"> id </param>
		/// <returns>bool</returns>
		public static bool DeleteByLevel(string logLevel)
		{
			string sqlCommand = @"
DELETE FROM mp_SystemLog 
WHERE LogLevel = ?LogLevel;";

			var arParams = new List<MySqlParameter>
			{
				new("?LogLevel", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = logLevel
				}
			};

			int rowsAffected = CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWrite(),
				sqlCommand.ToString(),
				arParams);

			return rowsAffected > 0;

		}

		/// <summary>
		/// Gets a count of rows in the mp_SystemLog table.
		/// </summary>
		public static int GetCount()
		{
			string sqlCommand = @"
SELECT Count(*) 
FROM mp_SystemLog ;";

			return Convert.ToInt32(
				CommandHelper.ExecuteScalar(
					ConnectionString.GetRead(),
					sqlCommand.ToString()
				)
			);

		}

		/// <summary>
		/// Gets a page of data from the mp_SystemLog table.
		/// </summary>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="totalPages">total pages</param>
		public static IDataReader GetPageAscending(
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
FROM mp_SystemLog  
ORDER BY ID  
LIMIT ?PageSize ";

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
				ConnectionString.GetRead(),
				sqlCommand.ToString(),
				arParams);

		}


		/// <summary>
		/// Gets a page of data from the mp_SystemLog table.
		/// </summary>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="totalPages">total pages</param>
		public static IDataReader GetPageDescending(
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
FROM mp_SystemLog  
ORDER BY ID DESC  
LIMIT ?PageSize ";

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
				ConnectionString.GetRead(),
				sqlCommand.ToString(),
				arParams);

		}


	}
}
