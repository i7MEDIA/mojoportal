using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;

public static class DBLetterInfo
{
	/// <summary>
	/// Inserts a row in the mp_LetterInfo table. Returns rows affected count.
	/// </summary>
	/// <param name="letterInfoGuid"> letterInfoGuid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="title"> title </param>
	/// <param name="description"> description </param>
	/// <param name="availableToRoles"> availableToRoles </param>
	/// <param name="enabled"> enabled </param>
	/// <param name="allowUserFeedback"> allowUserFeedback </param>
	/// <param name="allowAnonFeedback"> allowAnonFeedback </param>
	/// <param name="fromAddress"> fromAddress </param>
	/// <param name="fromName"> fromName </param>
	/// <param name="replyToAddress"> replyToAddress </param>
	/// <param name="sendMode"> sendMode </param>
	/// <param name="enableViewAsWebPage"> enableViewAsWebPage </param>
	/// <param name="enableSendLog"> enableSendLog </param>
	/// <param name="rolesThatCanEdit"> rolesThatCanEdit </param>
	/// <param name="rolesThatCanApprove"> rolesThatCanApprove </param>
	/// <param name="rolesThatCanSend"> rolesThatCanSend </param>
	/// <param name="createdUTC"> createdUTC </param>
	/// <param name="createdBy"> createdBy </param>
	/// <param name="lastModUTC"> lastModUTC </param>
	/// <param name="lastModBy"> lastModBy </param>
	/// <returns>int</returns>
	public static int Create(
		Guid letterInfoGuid,
		Guid siteGuid,
		string title,
		string description,
		string availableToRoles,
		bool enabled,
		bool allowUserFeedback,
		bool allowAnonFeedback,
		string fromAddress,
		string fromName,
		string replyToAddress,
		int sendMode,
		bool enableViewAsWebPage,
		bool enableSendLog,
		string rolesThatCanEdit,
		string rolesThatCanApprove,
		string rolesThatCanSend,
		DateTime createdUtc,
		Guid createdBy,
		DateTime lastModUtc,
		Guid lastModBy,
		bool allowArchiveView,
		bool profileOptIn,
		int sortRank,
		string displayNameDefault,
		string firstNameDefault,
		string lastNameDefault)
	{
		#region Bit Conversion

		int intAllowArchiveView = 0;
		if (allowArchiveView) { intAllowArchiveView = 1; }

		int intProfileOptIn = 0;
		if (profileOptIn) { intProfileOptIn = 1; }

		int intEnabled = 0;
		if (enabled) { intEnabled = 1; }

		int intAllowUserFeedback = 0;
		if (allowUserFeedback) { intAllowUserFeedback = 1; }

		int intAllowAnonFeedback = 0;
		if (allowAnonFeedback) { intAllowAnonFeedback = 1; }

		int intEnableViewAsWebPage = 0;
		if (enableViewAsWebPage) { intEnableViewAsWebPage = 1; }

		int intEnableSendLog = 0;
		if (enableSendLog) { intEnableSendLog = 1; }


		#endregion

		string sqlCommand = @"
INSERT INTO 
    mp_LetterInfo (
        LetterInfoGuid, 
        SiteGuid, 
        Title, 
        Description, 
        AvailableToRoles, 
        Enabled, 
        AllowUserFeedback, 
        AllowAnonFeedback, 
        FromAddress, 
        FromName, 
        ReplyToAddress, 
        SendMode, 
        EnableViewAsWebPage, 
        EnableSendLog, 
        RolesThatCanEdit, 
        RolesThatCanApprove, 
        RolesThatCanSend, 
        SubscriberCount, 
        UnVerifiedCount, 
        AllowArchiveView, 
        ProfileOptIn, 
        SortRank, 
        DisplayNameDefault, 
        FirstNameDefault, 
        LastNameDefault, 
        CreatedUTC, 
        CreatedBy, 
        LastModUTC, 
        LastModBy 
    )
VALUES (
    ?LetterInfoGuid, 
    ?SiteGuid, 
    ?Title, 
    ?Description, 
    ?AvailableToRoles, 
    ?Enabled, 
    ?AllowUserFeedback, 
    ?AllowAnonFeedback, 
    ?FromAddress, 
    ?FromName, 
    ?ReplyToAddress, 
    ?SendMode, 
    ?EnableViewAsWebPage, 
    ?EnableSendLog, 
    ?RolesThatCanEdit, 
    ?RolesThatCanApprove, 
    ?RolesThatCanSend, 
    0, 
    0, 
    ?AllowArchiveView, 
    ?ProfileOptIn, 
    ?SortRank, 
    ?DisplayNameDefault, 
    ?FirstNameDefault, 
    ?LastNameDefault, 
    ?CreatedUTC, 
    ?CreatedBy, 
    ?LastModUTC, 
    ?LastModBy 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterInfoGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
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

			new ("?Description", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new ("?AvailableToRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = availableToRoles
			},

			new("?Enabled", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intEnabled
			},

			new("?AllowUserFeedback", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAllowUserFeedback
			},

			new("?AllowAnonFeedback", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAllowAnonFeedback
			},

			new("?FromAddress", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = fromAddress
			},

			new("?FromName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = fromName
			},

			new("?ReplyToAddress", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = replyToAddress
			},

			new("?SendMode", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sendMode
			},

			new("?EnableViewAsWebPage", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intEnableViewAsWebPage
			},

			new("?EnableSendLog", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intEnableSendLog
			},

			new("?RolesThatCanEdit", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = rolesThatCanEdit
			},

			new("?RolesThatCanApprove", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = rolesThatCanApprove
			},

			new("?RolesThatCanSend", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = rolesThatCanSend
			},

			new("?CreatedUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdUtc
			},

			new("?CreatedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = createdBy.ToString()
			},

			new("?LastModUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastModUtc
			},

			new("?LastModBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = lastModBy.ToString()
			},

			new("?AllowArchiveView", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAllowArchiveView
			},

			new("?ProfileOptIn", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intProfileOptIn
			},

			new("?SortRank", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sortRank
			},

			new("?DisplayNameDefault", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = displayNameDefault
			},

			new("?FirstNameDefault", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = firstNameDefault
			},

			new("?LastNameDefault", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = lastNameDefault
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected;

	}


	/// <summary>
	/// Updates a row in the mp_LetterInfo table. Returns true if row updated.
	/// </summary>
	/// <param name="letterInfoGuid"> letterInfoGuid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="title"> title </param>
	/// <param name="description"> description </param>
	/// <param name="availableToRoles"> availableToRoles </param>
	/// <param name="enabled"> enabled </param>
	/// <param name="allowUserFeedback"> allowUserFeedback </param>
	/// <param name="allowAnonFeedback"> allowAnonFeedback </param>
	/// <param name="fromAddress"> fromAddress </param>
	/// <param name="fromName"> fromName </param>
	/// <param name="replyToAddress"> replyToAddress </param>
	/// <param name="sendMode"> sendMode </param>
	/// <param name="enableViewAsWebPage"> enableViewAsWebPage </param>
	/// <param name="enableSendLog"> enableSendLog </param>
	/// <param name="rolesThatCanEdit"> rolesThatCanEdit </param>
	/// <param name="rolesThatCanApprove"> rolesThatCanApprove </param>
	/// <param name="rolesThatCanSend"> rolesThatCanSend </param>
	/// <param name="createdUTC"> createdUTC </param>
	/// <param name="createdBy"> createdBy </param>
	/// <param name="lastModUTC"> lastModUTC </param>
	/// <param name="lastModBy"> lastModBy </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid letterInfoGuid,
		Guid siteGuid,
		string title,
		string description,
		string availableToRoles,
		bool enabled,
		bool allowUserFeedback,
		bool allowAnonFeedback,
		string fromAddress,
		string fromName,
		string replyToAddress,
		int sendMode,
		bool enableViewAsWebPage,
		bool enableSendLog,
		string rolesThatCanEdit,
		string rolesThatCanApprove,
		string rolesThatCanSend,
		DateTime createdUtc,
		Guid createdBy,
		DateTime lastModUtc,
		Guid lastModBy,
		bool allowArchiveView,
		bool profileOptIn,
		int sortRank,
		string displayNameDefault,
		string firstNameDefault,
		string lastNameDefault)
	{
		#region Bit Conversion

		int intAllowArchiveView = 0;
		if (allowArchiveView) { intAllowArchiveView = 1; }

		int intProfileOptIn = 0;
		if (profileOptIn) { intProfileOptIn = 1; }

		int intEnabled = 0;
		if (enabled) { intEnabled = 1; }

		int intAllowUserFeedback = 0;
		if (allowUserFeedback) { intAllowUserFeedback = 1; }

		int intAllowAnonFeedback = 0;
		if (allowAnonFeedback) { intAllowAnonFeedback = 1; }

		int intEnableViewAsWebPage = 0;
		if (enableViewAsWebPage) { intEnableViewAsWebPage = 1; }

		int intEnableSendLog = 0;
		if (enableSendLog) { intEnableSendLog = 1; }


		#endregion

		string sqlCommand = @"
UPDATE 
    mp_LetterInfo 
SET  
    SiteGuid = ?SiteGuid, 
    Title = ?Title, 
    Description = ?Description, 
    AvailableToRoles = ?AvailableToRoles, 
    Enabled = ?Enabled, 
    AllowUserFeedback = ?AllowUserFeedback, 
    AllowAnonFeedback = ?AllowAnonFeedback, 
    FromAddress = ?FromAddress, 
    FromName = ?FromName, 
    ReplyToAddress = ?ReplyToAddress, 
    SendMode = ?SendMode, 
    EnableViewAsWebPage = ?EnableViewAsWebPage, 
    EnableSendLog = ?EnableSendLog, 
    RolesThatCanEdit = ?RolesThatCanEdit, 
    RolesThatCanApprove = ?RolesThatCanApprove, 
    RolesThatCanSend = ?RolesThatCanSend, 
    AllowArchiveView = ?AllowArchiveView, 
    ProfileOptIn = ?ProfileOptIn, 
    SortRank = ?SortRank, 
    DisplayNameDefault = ?DisplayNameDefault, 
    FirstNameDefault = ?FirstNameDefault, 
    LastNameDefault = ?LastNameDefault, 
    CreatedUTC = ?CreatedUTC, 
    CreatedBy = ?CreatedBy, 
    LastModUTC = ?LastModUTC, 
    LastModBy = ?LastModBy 
WHERE  
    LetterInfoGuid = ?LetterInfoGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterInfoGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
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

			new ("?Description", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new ("?AvailableToRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = availableToRoles
			},

			new("?Enabled", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intEnabled
			},

			new("?AllowUserFeedback", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAllowUserFeedback
			},

			new("?AllowAnonFeedback", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAllowAnonFeedback
			},

			new("?FromAddress", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = fromAddress
			},

			new("?FromName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = fromName
			},

			new("?ReplyToAddress", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = replyToAddress
			},

			new("?SendMode", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sendMode
			},

			new("?EnableViewAsWebPage", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intEnableViewAsWebPage
			},

			new("?EnableSendLog", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intEnableSendLog
			},

			new("?RolesThatCanEdit", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = rolesThatCanEdit
			},

			new("?RolesThatCanApprove", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = rolesThatCanApprove
			},

			new("?RolesThatCanSend", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = rolesThatCanSend
			},

			new("?CreatedUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdUtc
			},

			new("?CreatedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = createdBy.ToString()
			},

			new("?LastModUTC", MySqlDbType.DateTime)
			{
				 Direction = ParameterDirection.Input,
				Value = lastModUtc
			},

			new("?LastModBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = lastModBy.ToString()
			},

			new("?AllowArchiveView", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAllowArchiveView
			},

			new("?ProfileOptIn", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intProfileOptIn
			},

			new("?SortRank", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sortRank
			},

			new("?DisplayNameDefault", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = displayNameDefault
			},

			new("?FirstNameDefault", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = firstNameDefault
			},

			new("?LastNameDefault", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = lastNameDefault
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Updates the subscriber count on a row in the mp_LetterInfo table. Returns true if row updated.
	/// </summary>
	/// <param name="letterInfoGuid"> letterInfoGuid </param>
	/// <returns>bool</returns>
	public static bool UpdateSubscriberCount(Guid letterInfoGuid)
	{


		string sqlCommand = @"
UPDATE 
    mp_LetterInfo 
SET  
    SubscriberCount = (  
        SELECT COUNT(*) 
        FROM mp_LetterSubscribe  
        WHERE  
        LetterInfoGuid = ?LetterInfoGuid  
    ),  
    UnVerifiedCount = (  
        SELECT COUNT(*) 
        FROM mp_LetterSubscribe  
        WHERE  
        LetterInfoGuid = ?LetterInfoGuid AND IsVerified = 0  
    )  
WHERE  
    LetterInfoGuid = ?LetterInfoGuid;";

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


		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_LetterInfo table. Returns true if row deleted.
	/// </summary>
	/// <param name="letterInfoGuid"> letterInfoGuid </param>
	/// <returns>bool</returns>
	public static bool Delete(
		Guid letterInfoGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_LetterInfo 
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
	/// Gets an IDataReader with one row from the mp_LetterInfo table.
	/// </summary>
	/// <param name="letterInfoGuid"> letterInfoGuid </param>
	public static IDataReader GetOne(
		Guid letterInfoGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_LetterInfo 
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
	/// Gets an IDataReader with all rows in the mp_LetterInfo table.
	/// </summary>
	public static IDataReader GetAll(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT 
    li.*, 
    l.SendClickedUTC
FROM 
    mp_LetterInfo li
LEFT JOIN (
    SELECT LetterInfoGuid, 
    MAX(SendClickedUTC) 
    AS SendClickedUTC 
    FROM mp_Letter 
    GROUP BY LetterInfoGuid) 
    AS l 
    ON l.LetterInfoGuid = li.LetterInfoGuid
WHERE 
    li.SiteGuid = ?SiteGuid
ORDER BY 
    SortRank, Title;";

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
			sqlCommand,
			arParams);
	}

	/// <summary>
	/// Gets a count of rows in the mp_LetterInfo table.
	/// </summary>
	public static int GetCount(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_LetterInfo 
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
	/// Gets a page of data from the mp_LetterInfo table.
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
FROM mp_LetterInfo  
WHERE SiteGuid = ?SiteGuid 
ORDER BY SortRank, Title 
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
