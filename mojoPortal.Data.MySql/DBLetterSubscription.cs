using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;

public static class DBLetterSubscription
{

	/// <summary>
	/// Inserts a row in the mp_LetterSubscribe table. Returns rows affected count.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="letterInfoGuid"> letterInfoGuid </param>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="email"> email </param>
	/// <param name="isVerified"> isVerified </param>
	/// <param name="verifyGuid"> verifyGuid </param>
	/// <param name="beginUtc"> beginUtc </param>
	/// <param name="useHtml"> useHtml </param>
	/// <returns>int</returns>
	public static int Create(
		Guid guid,
		Guid siteGuid,
		Guid letterInfoGuid,
		Guid userGuid,
		string email,
		bool isVerified,
		Guid verifyGuid,
		DateTime beginUtc,
		bool useHtml,
		string ipAddress)
	{
		#region Bit Conversion

		int intIsVerified = 0;
		if (isVerified) { intIsVerified = 1; }
		int intUseHtml = 0;
		if (useHtml) { intUseHtml = 1; }

		#endregion

		string sqlCommand = @"
INSERT INTO 
    mp_LetterSubscribe (
        Guid, 
        SiteGuid, 
        LetterInfoGuid, 
        UserGuid, 
        Email, 
        IsVerified, 
        VerifyGuid, 
        IpAddress, 
        BeginUtc, 
        UseHtml 
    )
VALUES (
    ?Guid, 
    ?SiteGuid, 
    ?LetterInfoGuid, 
    ?UserGuid, 
    ?Email, 
    ?IsVerified, 
    ?VerifyGuid, 
    ?IpAddress, 
    ?BeginUtc, 
    ?UseHtml 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36) {
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?LetterInfoGuid", MySqlDbType.VarChar, 36) {
				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
			},

			new("?UserGuid", MySqlDbType.VarChar, 36) {
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?Email", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = email
			},

			new("?IsVerified", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIsVerified
			},

			new ("?VerifyGuid", MySqlDbType.VarChar, 36) {
				Direction = ParameterDirection.Input,
				Value = verifyGuid.ToString()
			},

			new ("?BeginUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = beginUtc
			},

			new("?UseHtml", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intUseHtml
			},

			new("?IpAddress", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = ipAddress
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected;

	}

	/// <summary>
	/// Inserts a row in the mp_LetterSubscribe table. Returns rows affected count.
	/// This method is a legacy method only needed to retain the correct signature for the
	/// DatabaseHelperDoVersion2320PostUpgradeTasks
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="letterInfoGuid"> letterInfoGuid </param>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="email"> email </param>
	/// <param name="isVerified"> isVerified </param>
	/// <param name="verifyGuid"> verifyGuid </param>
	/// <param name="beginUtc"> beginUtc </param>
	/// <param name="useHtml"> useHtml </param>
	/// <returns>int</returns>
	public static int Create(
		Guid guid,
		Guid siteGuid,
		Guid letterInfoGuid,
		Guid userGuid,
		string email,
		bool isVerified,
		Guid verifyGuid,
		DateTime beginUtc,
		bool useHtml)
	{
		#region Bit Conversion

		int intIsVerified = 0;
		if (isVerified) { intIsVerified = 1; }
		int intUseHtml = 0;
		if (useHtml) { intUseHtml = 1; }

		#endregion

		string sqlCommand = @"
INSERT INTO 
    mp_LetterSubscribe (
    Guid, 
    SiteGuid, 
    LetterInfoGuid, 
    UserGuid, 
    Email, 
    IsVerified, 
    VerifyGuid, 
    BeginUtc, 
    UseHtml 
)
VALUES (
    ?Guid, 
    ?SiteGuid, 
    ?LetterInfoGuid, 
    ?UserGuid, 
    ?Email, 
    ?IsVerified, 
    ?VerifyGuid, 
    ?BeginUtc, 
    ?UseHtml 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36) {
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?LetterInfoGuid", MySqlDbType.VarChar, 36) {
				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
			},

			new("?UserGuid", MySqlDbType.VarChar, 36) {
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?Email", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = email
			},

			new("?IsVerified", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIsVerified
			},

			new("?VerifyGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = verifyGuid.ToString()
			},

			new("?BeginUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = beginUtc
			},

			new("?UseHtml", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intUseHtml
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected;

	}

	public static bool Update(
		Guid guid,
		Guid userGuid,
		bool useHtml)
	{
		#region Bit Conversion

		int intUseHtml = 0;
		if (useHtml) { intUseHtml = 1; }

		#endregion

		string sqlCommand = @"
UPDATE 
`mp_LetterSubscribe 
SET  
    UseHtml = ?UseHtml, 
    UserGuid = ?UserGuid 
WHERE  
    Guid = ?Guid;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?UseHtml", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intUseHtml
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static bool Exists(Guid letterInfoGuid, string email)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_LetterSubscribe 
WHERE LetterInfoGuid = ?LetterInfoGuid 
AND Email = @Email;";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterInfoGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
			},

			new("?Email", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = email
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

		return count > 0;

	}


	public static bool Verify(
		Guid guid,
		bool isVerified,
		Guid verifyGuid)
	{
		#region Bit Conversion

		int intIsVerified = 0;
		if (isVerified) { intIsVerified = 1; }


		#endregion

		string sqlCommand = @"
UPDATE 
    mp_LetterSubscribe 
SET  
    IsVerified = ?IsVerified, 
    VerifyGuid = ?VerifyGuid 
WHERE  
    Guid = ?Guid 
;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?IsVerified", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIsVerified
			},

			new("?VerifyGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = verifyGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static bool Delete(Guid guid)
	{
		string sqlCommand = @"
DELETE FROM mp_LetterSubscribe 
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
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool DeleteByLetter(Guid letterInfoGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_LetterSubscribe 
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
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool DeleteUnverified(Guid letterInfoGuid, DateTime olderThanUtc)
	{
		string sqlCommand = @"
DELETE FROM 
    mp_LetterSubscribe 
WHERE 
    LetterInfoGuid = ?LetterInfoGuid 
AND 
    IsVerified = 0 
AND 
    BeginUtc < ?OlderThanUtc 
;";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterInfoGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
			},

			new("?OlderThanUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = olderThanUtc
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool DeleteByUser(Guid userGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_LetterSubscribe 
WHERE UserGuid = ?UserGuid;";

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

	public static bool DeleteBySite(Guid siteGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_LetterSubscribe 
WHERE SiteGuid = ?SiteGuid;";

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

	public static IDataReader GetOne(Guid guid)
	{
		string sqlCommand = @"
SELECT 
    ls.Guid, 
    ls.SiteGuid, 
    ls.LetterInfoGuid, 
    ls.UserGuid, 
    ls.IsVerified, 
    ls.VerifyGuid, 
    ls.BeginUtc, 
    ls.UseHtml, 
    ls.IpAddress, 
    COALESCE(u.Email, ls.Email) As Email, 
    u.Email AS UserEmail, 
    COALESCE(u.Name, ls.Email) AS Name, 
    u.FirstName, 
    u.LastName 
FROM 
    mp_LetterSubscribe ls 
LEFT OUTER JOIN 
    mp_Users u 
ON 
    u.UserGuid = ls.UserGuid 
WHERE 
    ls.Guid = ?Guid 
;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetByLetter(Guid letterInfoGuid)
	{
		string sqlCommand = @"
SELECT 
    ls.Guid, 
    ls.SiteGuid, 
    ls.LetterInfoGuid, 
    ls.UserGuid, 
    ls.IsVerified, 
    ls.VerifyGuid, 
    ls.BeginUtc, 
    ls.UseHtml, 
    ls.IpAddress, 
    COALESCE(u.Email, ls.Email) As Email, 
    u.Email AS UserEmail, 
    COALESCE(u.Name, ls.Email) AS Name, 
    u.FirstName, 
    u.LastName 
FROM 
    mp_LetterSubscribe ls 
LEFT OUTER JOIN 
    mp_Users u 
ON 
    u.UserGuid = ls.UserGuid 
WHERE 
    ls.LetterInfoGuid = ?LetterInfoGuid 
;";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterInfoGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetByUser(Guid siteGuid, Guid userGuid)
	{
		string sqlCommand = @"
SELECT 
    ls.Guid, 
    ls.SiteGuid, 
    ls.LetterInfoGuid, 
    ls.UserGuid, 
    ls.IsVerified, 
    ls.VerifyGuid, 
    ls.BeginUtc, 
    ls.UseHtml, 
    ls.IpAddress, 
    COALESCE(u.Email, ls.Email) As Email, 
    u.Email AS UserEmail, 
    COALESCE(u.Name, ls.Email) AS Name, 
    u.FirstName, 
    u.LastName 
FROM 
    mp_LetterSubscribe ls 
LEFT OUTER JOIN 
    mp_Users u 
ON 
    u.UserGuid = ls.UserGuid 
WHERE 
    ls.SiteGuid = ?SiteGuid 
AND 
    ls.UserGuid = ?UserGuid 
;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
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

	public static IDataReader Search(Guid letterInfoGuid, string emailOrIpAddress)
	{
		string sqlCommand = @"
SELECT 
    ls.Guid, 
    ls.SiteGuid, 
    ls.LetterInfoGuid, 
    ls.UserGuid, 
    ls.IsVerified, 
    ls.VerifyGuid, 
    ls.BeginUtc, 
    ls.UseHtml, 
    ls.IpAddress, 
    COALESCE(u.Email, ls.Email) As Email, 
    u.Email AS UserEmail, 
    COALESCE(u.Name, ls.Email) AS Name, 
    u.FirstName, 
    u.LastName 
FROM 
    mp_LetterSubscribe ls 
LEFT OUTER JOIN 
    mp_Users u 
ON 
    u.UserGuid = ls.UserGuid      
WHERE 
    ls.LetterInfoGuid = ?LetterInfoGuid 
AND (
        (ls.Email LIKE ?EmailOrIpAddress) 
        OR 
        (u.Email LIKE ?EmailOrIpAddress) 
        OR 
        (ls.IpAddress LIKE ?EmailOrIpAddress)
);";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterInfoGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
			},

			new("?EmailOrIpAddress", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = emailOrIpAddress
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetByEmail(Guid siteGuid, string email)
	{
		string sqlCommand = @"
SELECT 
    ls.Guid, 
    ls.SiteGuid, 
    ls.LetterInfoGuid, 
    ls.UserGuid, 
    ls.IsVerified, 
    ls.VerifyGuid, 
    ls.BeginUtc, 
    ls.UseHtml, 
    ls.IpAddress, 
    COALESCE(u.Email, ls.Email) As Email, 
    u.Email AS UserEmail, 
    COALESCE(u.Name, ls.Email) AS Name, 
    u.FirstName, 
    u.LastName 
FROM 
    mp_LetterSubscribe ls 
LEFT OUTER JOIN 
    mp_Users u 
ON 
    u.UserGuid = ls.UserGuid 
WHERE 
    ls.SiteGuid = ?SiteGuid 
AND (
    (ls.Email = ?Email) OR (u.Email = ?Email)
);";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?Email", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = email
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetByEmail(Guid siteGuid, Guid letterInfoGuid, string email)
	{
		string sqlCommand = @"
SELECT 
    ls.Guid, 
    ls.SiteGuid, 
    ls.LetterInfoGuid, 
    ls.UserGuid, 
    ls.IsVerified, 
    ls.VerifyGuid, 
    ls.BeginUtc, 
    ls.UseHtml, 
    ls.IpAddress, 
    COALESCE(u.Email, ls.Email) As Email, 
    u.Email AS UserEmail, 
    COALESCE(u.Name, ls.Email) AS Name, 
    u.FirstName, 
    u.LastName 
FROM 
    mp_LetterSubscribe ls 
LEFT OUTER JOIN 
    mp_Users u 
ON 
    u.UserGuid = ls.UserGuid 
WHERE 
    ls.SiteGuid = ?SiteGuid 
AND 
    ls.LetterInfoGuid = ?LetterInfoGuid 
AND (
    (ls.Email = ?Email) OR (u.Email = ?Email)
);";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?LetterInfoGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
			},

			new("?Email", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = email
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static int CountUsersNotSubscribedByLetter(Guid siteGuid, Guid letterInfoGuid, bool excludeIfAnyUnsubscribeHx)
	{
		int intExcludeIfAnyUnsubscribeHx = 0;
		if (excludeIfAnyUnsubscribeHx)
		{
			intExcludeIfAnyUnsubscribeHx = 1;
		}

		string sqlCommand = @"
SELECT 
    COUNT(*)  
FROM 
    mp_Users u 
WHERE 
    u.SiteGuid = ?SiteGuid 
    AND u.IsDeleted = 0 
    AND u.ProfileApproved = 1 
    AND u.IsLockedOut = 0 
    AND (u.RegisterConfirmGuid IS NULL OR u.RegisterConfirmGuid = '00000000-0000-0000-0000-000000000000') 
    AND u.UserGuid NOT IN (
        SELECT ls.UserGuid 
        FROM mp_LetterSubscribe ls 
        WHERE ls.LetterInfoGuid = ?LetterInfoGuid 
    ) 
    AND u.UserGuid NOT IN (
        SELECT lsx.UserGuid 
        FROM mp_LetterSubscribeHx lsx 
        WHERE (
            (?ExcludeIfAnyUnsubscribeHx = 1) OR (lsx.LetterInfoGuid = ?LetterInfoGuid)
    ) 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?LetterInfoGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
			},

			new("?ExcludeIfAnyUnsubscribeHx", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intExcludeIfAnyUnsubscribeHx
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

		return count;
	}

	public static IDataReader GetTop1000UsersNotSubscribed(Guid siteGuid, Guid letterInfoGuid, bool excludeIfAnyUnsubscribeHx)
	{
		int intExcludeIfAnyUnsubscribeHx = 0;
		if (excludeIfAnyUnsubscribeHx)
		{
			intExcludeIfAnyUnsubscribeHx = 1;
		}

		string sqlCommand = @"
SELECT 
    u.UserID, 
    u.UserGuid, 
    u.Email 
FROM 
    mp_Users u 
WHERE 
    u.SiteGuid = ?SiteGuid 
    AND u.IsDeleted = 0 
    AND u.ProfileApproved = 1 
    AND u.IsLockedOut = 0 
    AND (u.RegisterConfirmGuid IS NULL OR u.RegisterConfirmGuid = '00000000-0000-0000-0000-000000000000') 
    AND u.UserGuid NOT IN (
        SELECT ls.UserGuid 
        FROM mp_LetterSubscribe ls 
        WHERE ls.LetterInfoGuid = ?LetterInfoGuid 
    ) 
    AND u.UserGuid NOT IN (
        SELECT lsx.UserGuid 
        FROM mp_LetterSubscribeHx lsx 
        WHERE (
            (?ExcludeIfAnyUnsubscribeHx = 1) OR (lsx.LetterInfoGuid = ?LetterInfoGuid)
        ) 
    ) 
ORDER BY 
    u.UserID 
LIMIT 1000;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?LetterInfoGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
			},

			new("?ExcludeIfAnyUnsubscribeHx", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intExcludeIfAnyUnsubscribeHx
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetSubscribersNotSentYet(
		Guid letterGuid,
		Guid letterInfoGuid)
	{
		string sqlCommand = @"
SELECT 
    ls.Guid, 
    ls.SiteGuid, 
    ls.LetterInfoGuid, 
    ls.UserGuid, 
    ls.IsVerified, 
    ls.VerifyGuid, 
    ls.BeginUtc, 
    ls.UseHtml, 
    ls.IpAddress, 
    COALESCE(u.Email, ls.Email) As Email, 
    u.Name, 
    u.Email AS UserEmail, 
    u.FirstName, 
    u.LastName 
FROM 
    mp_LetterSubscribe ls  
LEFT OUTER JOIN	
    mp_Users u  
ON 
    u.UserGuid = ls.UserGuid  
WHERE 
    ls.LetterInfoGuid = ?LetterInfoGuid 
AND 
    ls.IsVerified = 1 
AND 
    ls.Guid   
NOT IN (  
    SELECT SubscribeGuid 
    FROM mp_LetterSendLog 
    WHERE LetterGuid = ?LetterGuid 
);";

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
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}


	public static int GetCountByLetter(Guid letterInfoGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_LetterSubscribe 
WHERE LetterInfoGuid = ?LetterInfoGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?LetterInfoGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

		return count;

	}

	public static IDataReader GetPage(
		Guid letterInfoGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCountByLetter(letterInfoGuid);

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
SELECT 
    ls.Guid, 
    ls.SiteGuid, 
    ls.LetterInfoGuid, 
    ls.UserGuid, 
    ls.IsVerified, 
    ls.VerifyGuid, 
    ls.BeginUtc, 
    ls.UseHtml, 
    ls.IpAddress, 
    COALESCE(u.Email, ls.Email) As Email, 
    COALESCE(u.Name, ls.Email) AS Name, 
    u.Email AS UserEmail, 
    u.FirstName, 
    u.LastName 
FROM 
    mp_LetterSubscribe ls  
LEFT OUTER JOIN	
    mp_Users u  
ON 
    u.UserGuid = ls.UserGuid  
WHERE 
    ls.LetterInfoGuid = ?LetterInfoGuid 
ORDER BY 
    ls.BeginUtc DESC 
LIMIT 
    " + pageLowerBound.ToString() + ", ?PageSize;";

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
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Inserts a row in the mp_LetterSubscribeHx table. Returns rows affected count.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="subscribeGuid"> subscribeGuid </param>
	/// <param name="letterInfoGuid"> letterInfoGuid </param>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="email"> email </param>
	/// <param name="isVerified"> isVerified </param>
	/// <param name="useHtml"> useHtml </param>
	/// <param name="beginUtc"> beginUtc </param>
	/// <param name="endUtc"> endUtc </param>
	/// <returns>int</returns>
	public static int CreateHistory(
		Guid rowGuid,
		Guid siteGuid,
		Guid subscribeGuid,
		Guid letterInfoGuid,
		Guid userGuid,
		string email,
		bool isVerified,
		bool useHtml,
		DateTime beginUtc,
		DateTime endUtc,
		string ipAddress)
	{
		#region Bit Conversion

		int intIsVerified = 0;
		if (isVerified) { intIsVerified = 1; }
		int intUseHtml = 0;
		if (useHtml) { intUseHtml = 1; }

		#endregion

		string sqlCommand = @"
INSERT INTO 
    mp_LetterSubscribeHx (
        RowGuid, 
        SiteGuid, 
        SubscribeGuid, 
        LetterInfoGuid, 
        UserGuid, 
        Email, 
        IsVerified, 
        IpAddress, 
        UseHtml, 
        BeginUtc, 
        EndUtc )
         VALUES (
        ?RowGuid, 
        ?SiteGuid, 
        ?SubscribeGuid, 
        ?LetterInfoGuid, 
        ?UserGuid, 
        ?Email, 
        ?IsVerified, 
        ?IpAddress, 
        ?UseHtml, 
        ?BeginUtc, 
        ?EndUtc 
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

			new("?SubscribeGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = subscribeGuid.ToString()
			},

			new("?LetterInfoGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = letterInfoGuid.ToString()
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?Email", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = email
			},

			new("?IsVerified", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIsVerified
			},

			new("?UseHtml", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intUseHtml
			},

			new("?BeginUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = beginUtc
			},

			new ("?EndUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = endUtc
			},

			new ("?IpAddress", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = ipAddress
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected;

	}

	public static bool DeleteHistoryBySite(Guid siteGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_LetterSubscribeHx 
WHERE SiteGuid = ?SiteGuid;";

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

	public static bool DeleteHistoryByLetterInfo(Guid letterInfoGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_LetterSubscribeHx 
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
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}


}
