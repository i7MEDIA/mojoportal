
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;

public static class DBLetter
{

	/// <summary>
	/// Inserts a row in the mp_Letter table. Returns rows affected count.
	/// </summary>
	/// <param name="letterGuid"> letterGuid </param>
	/// <param name="letterInfoGuid"> letterInfoGuid </param>
	/// <param name="subject"> subject </param>
	/// <param name="htmlBody"> htmlBody </param>
	/// <param name="textBody"> textBody </param>
	/// <param name="createdBy"> createdBy </param>
	/// <param name="createdUTC"> createdUTC </param>
	/// <param name="lastModBy"> lastModBy </param>
	/// <param name="lastModUTC"> lastModUTC </param>
	/// <param name="isApproved"> isApproved </param>
	/// <param name="approvedBy"> approvedBy </param>
	/// <returns>int</returns>
	public static int Create(
		Guid letterGuid,
		Guid letterInfoGuid,
		string subject,
		string htmlBody,
		string textBody,
		Guid createdBy,
		DateTime createdUtc,
		Guid lastModBy,
		DateTime lastModUtc,
		bool isApproved,
		Guid approvedBy)
	{
		#region Bit Conversion

		int intIsApproved;
		if (isApproved)
		{
			intIsApproved = 1;
		}
		else
		{
			intIsApproved = 0;
		}


		#endregion

		string sqlCommand = @"
INSERT INTO 
    mp_Letter (
        LetterGuid, 
        LetterInfoGuid, 
        Subject, 
        HtmlBody, 
        TextBody, 
        CreatedBy, 
        CreatedUTC, 
        LastModBy, 
        LastModUTC, 
        IsApproved, 
        ApprovedBy, 
        SendCount 
    )
VALUES (
    ?LetterGuid, 
    ?LetterInfoGuid, 
    ?Subject, 
    ?HtmlBody, 
    ?TextBody, 
    ?CreatedBy, 
    ?CreatedUTC, 
    ?LastModBy, 
    ?LastModUTC, 
    ?IsApproved, 
    ?ApprovedBy, 
    0 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterGuid.ToString()
			},

			new ("?LetterInfoGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
	},

			new ("?Subject", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = subject
			},

			new("?HtmlBody", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = htmlBody
			},

			new("?TextBody", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = textBody
			},

			new("?CreatedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = createdBy.ToString()
			},

			new("?CreatedUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdUtc
			},

			new("?LastModBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = lastModBy.ToString()
			},

			new("?LastModUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastModUtc
			},

			new("?IsApproved", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIsApproved
			},

			new("?ApprovedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = approvedBy.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}


	/// <summary>
	/// Updates a row in the mp_Letter table. Returns true if row updated.
	/// </summary>
	/// <param name="letterGuid"> letterGuid </param>
	/// <param name="letterInfoGuid"> letterInfoGuid </param>
	/// <param name="subject"> subject </param>
	/// <param name="htmlBody"> htmlBody </param>
	/// <param name="textBody"> textBody </param>
	/// <param name="lastModBy"> lastModBy </param>
	/// <param name="lastModUTC"> lastModUTC </param>
	/// <param name="isApproved"> isApproved </param>
	/// <param name="approvedBy"> approvedBy </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid letterGuid,
		Guid letterInfoGuid,
		string subject,
		string htmlBody,
		string textBody,
		Guid lastModBy,
		DateTime lastModUtc,
		bool isApproved,
		Guid approvedBy)
	{
		#region Bit Conversion

		int intIsApproved;
		if (isApproved)
		{
			intIsApproved = 1;
		}
		else
		{
			intIsApproved = 0;
		}


		#endregion

		string sqlCommand = @"
UPDATE 
    mp_Letter 
SET  
    LetterInfoGuid = ?LetterInfoGuid, 
    Subject = ?Subject, 
    HtmlBody = ?HtmlBody, 
    TextBody = ?TextBody, 
    LastModBy = ?LastModBy, 
    LastModUTC = ?LastModUTC, 
    IsApproved = ?IsApproved, 
    ApprovedBy = ?ApprovedBy 
WHERE  
    LetterGuid = ?LetterGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterGuid.ToString()
			},

			new("?LetterInfoGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
			},

			new("?Subject", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = subject
			},

			new("?HtmlBody", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = htmlBody
			},

			new("?TextBody", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = textBody
			},

			new("?LastModBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = lastModBy.ToString()
			},

			new("?LastModUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastModUtc
			},

			new("?IsApproved", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIsApproved
			},

			new("?ApprovedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = approvedBy.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_Letter table. Returns true if row deleted.
	/// </summary>
	/// <param name="letterGuid"> letterGuid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid letterGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_Letter 
WHERE LetterGuid = ?LetterGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	/// <summary>
	/// Deletes a row from the mp_Letter table. Returns true if row deleted.
	/// </summary>
	/// <param name="letterGuid"> letterGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteByLetterInfo(Guid letterInfoGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_Letter 
WHERE LetterInfoGuid = ?LetterInfoGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterInfoGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}


	/// <summary>
	/// Records the click of the send button in the db
	/// </summary>
	/// <param name="letterGuid"> letterGuid </param>
	/// <param name="sendClickedUtc"> sendClickedUtc </param>
	/// <returns>bool</returns>
	public static bool SendClicked(
		Guid letterGuid,
		DateTime sendClickedUtc)
	{

		string sqlCommand = @"
UPDATE mp_Letter 
SET SendClickedUTC = ?SendClickedUTC 
WHERE LetterGuid = ?LetterGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterGuid.ToString()
			},

			new("?SendClickedUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = sendClickedUtc
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}


	/// <summary>
	/// Records the start of sending in the db
	/// </summary>
	/// <param name="letterGuid"> letterGuid </param>
	/// <param name="sendClickedUtc"> sendClickedUtc </param>
	/// <returns>bool</returns>
	public static bool SendStarted(
		Guid letterGuid,
		DateTime sendStartedUtc)
	{

		string sqlCommand = @"
UPDATE mp_Letter 
SET SendStartedUTC = ?SendStartedUTC 
WHERE LetterGuid = ?LetterGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterGuid.ToString()
			},

			new("?SendStartedUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = sendStartedUtc
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Records the complete of sending in the db
	/// </summary>
	/// <param name="letterGuid"> letterGuid </param>
	/// <param name="sendClickedUtc"> sendClickedUtc </param>
	/// <returns>bool</returns>
	public static bool SendComplete(
		Guid letterGuid,
		DateTime sendCompleteUtc,
		int sendCount)
	{

		string sqlCommand = @"
UPDATE mp_Letter 
SET SendCompleteUTC = ?SendCompleteUTC, 
SendCount = ?SendCount 
WHERE LetterGuid = ?LetterGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterGuid.ToString()
			},

			new("?SendCompleteUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = sendCompleteUtc
			},

			new("?SendCount", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sendCount
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}



	/// <summary>
	/// Gets an IDataReader with one row from the mp_Letter table.
	/// </summary>
	/// <param name="letterGuid"> letterGuid </param>
	public static IDataReader GetOne(Guid letterGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Letter 
WHERE LetterGuid = ?LetterGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with all rows in the mp_Letter table.
	/// </summary>
	public static IDataReader GetAll(Guid letterInfoGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Letter 
WHERE LetterInfoGuid = ?LetterInfoGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterInfoGuid", MySqlDbType.VarChar, 36)
			{

				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}

	/// <summary>
	/// Gets a count of rows in the mp_Letter table.
	/// </summary>
	public static int GetCount(Guid letterInfoGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Letter 
WHERE LetterInfoGuid = ?LetterInfoGuid 
AND SendClickedUTC IS NOT NULL; ";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterInfoGuid", MySqlDbType.VarChar, 36)
			{

				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));
	}

	/// <summary>
	/// Gets a count of rows in the mp_Letter table.
	/// </summary>
	public static int GetCountOfDrafts(Guid letterInfoGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Letter 
WHERE LetterInfoGuid = ?LetterInfoGuid 
AND SendClickedUTC IS NULL;";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterInfoGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));
	}

	/// <summary>
	/// Gets a page of data from the mp_Letter table.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static IDataReader GetPage(
		Guid letterInfoGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCount(letterInfoGuid);

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
FROM mp_Letter  
WHERE LetterInfoGuid = ?LetterInfoGuid 
AND SendClickedUTC IS NOT NULL 
ORDER BY SendClickedUTC DESC 
LIMIT " + pageLowerBound.ToString() + ", ?PageSize;";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterInfoGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
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
	/// Gets a page of data from the mp_Letter table corresponding to unsent letters.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static IDataReader GetDrafts(
		Guid letterInfoGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCountOfDrafts(letterInfoGuid);

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
FROM mp_Letter  
WHERE 
LetterInfoGuid = ?LetterInfoGuid 
AND SendClickedUTC IS NULL 
ORDER BY CreatedUTC 
LIMIT " + pageLowerBound.ToString() + ", ?PageSize;";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterInfoGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
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
