using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using MySqlConnector;

namespace mojoPortal.Data;


public static class DBSiteUser
{

	public static IDataReader GetUserCountByYearMonth(int siteId)
	{
		string sqlCommand = @"
SELECT 
	YEAR(DateCreated) As Y,  
	MONTH(DateCreated) As M, 
	CONCAT(YEAR(DateCreated), '-', MONTH(DateCreated)) As Label, 
	COUNT(*) As Users 
FROM 
	mp_Users 
WHERE 
	SiteID = ?SiteID 
	GROUP BY YEAR(DateCreated), MONTH(DateCreated) 
	ORDER BY YEAR(DateCreated), MONTH(DateCreated); ";


		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);
	}


	public static IDataReader GetUserList(int siteId)
	{
		string sqlCommand = @"
SELECT 
	UserID, 
	Name, 
	PasswordSalt, 
	Pwd, 
	Email 
FROM 
	mp_Users 
WHERE 
	SiteID = ?SiteID 
ORDER BY 
	Email;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);
	}

	public static IDataReader GetSmartDropDownData(int siteId, string query, int rowsToGet)
	{

		string sqlCommand = @"
SELECT 
	UserID, 
	UserGuid, 
	Email, 
	FirstName, 
	LastName, 
	Name As SiteUser 
FROM 
	mp_Users 
WHERE 
	SiteID = ?SiteID 
AND IsDeleted = 0 
AND (
	(LOWER(Name) LIKE LOWER(?Query)) 
	OR (LOWER(FirstName) LIKE LOWER(?Query)) 
	OR (LOWER(LastName) LIKE LOWER(?Query)) 
) 
UNION 
SELECT 
	UserID, 
	UserGuid, 
	Email, 
	FirstName, 
	LastName, 
	Email As SiteUser 
FROM 
	mp_Users 
WHERE 
	SiteID = ?SiteID 
AND 
	IsDeleted = 0 
AND 
	LOWER(Email) LIKE LOWER(?Query) 
ORDER BY 
	SiteUser 
LIMIT " + rowsToGet.ToString() + "; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?Query", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = query + "%"
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);


	}

	public static IDataReader EmailLookup(int siteId, string query, int rowsToGet)
	{
		string sqlCommand = @"
SELECT 
	UserID, 
	UserGuid, 
	Email 
FROM 
	mp_Users 
WHERE 
	SiteID = ?SiteID 
	AND IsDeleted = 0 
	AND (
		(LOWER(Email) LIKE LOWER(?Query)) 
		OR (LOWER(Name) LIKE LOWER(?Query)) 
		OR (LOWER(FirstName) LIKE LOWER(?Query)) 
		OR (LOWER(LastName) LIKE LOWER(?Query)) 
	) 
ORDER BY 
	Email 
LIMIT " + rowsToGet.ToString() + " ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?Query", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = query + "%"
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);
	}

	public static int UserCount(int siteId)
	{
		string sqlCommand = @"
SELECT COUNT(*) FROM mp_Users WHERE SiteID = ?SiteID;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams).ToString());

		return count;

	}

	public static int CountLockedOutUsers(int siteId)
	{
		string sqlCommand = @"
SELECT COUNT(*) FROM mp_Users WHERE SiteID = ?SiteID AND IsLockedOut = 1;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams).ToString());

		return count;

	}

	public static int CountNotApprovedUsers(int siteId)
	{
		string sqlCommand = @"
SELECT COUNT(*) FROM mp_Users WHERE SiteID = ?SiteID AND ApprovedForForums = 0;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams).ToString());

		return count;
	}

	public static int UserCount(int siteId, String nameBeginsWith, string nameFilterMode)
	{
		string sqlCommand = @"
SELECT COUNT(*) FROM mp_Users 
WHERE SiteID = ?SiteID 
AND IsDeleted = 0 
AND ProfileApproved = 1 ";

		switch (nameFilterMode)
		{
			case "display":
			default:
				sqlCommand += "AND Lower(Name) LIKE LOWER(?BeginsWith) ";
				break;
			case "lastname":
				sqlCommand += "AND Lower(LastName) LIKE LOWER(?BeginsWith) ";
				break;
			case "":
				break;
		}

		sqlCommand += "; ";

		var arParams = new List<MySqlParameter>()
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new("?BeginsWith", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = nameBeginsWith + "%"
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams.ToArray()).ToString());

		return count;

	}

	public static int CountUsersByRegistrationDateRange(
		int siteId,
		DateTime beginDate,
		DateTime endDate)
	{
		string sqlCommand = @"
SELECT COUNT(*) FROM mp_Users WHERE SiteID = ?SiteID 
AND DateCreated >= ?BeginDate 
AND DateCreated < ?EndDate; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?BeginDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = beginDate
			},

			new("?EndDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = endDate
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams).ToString());

		return count;

	}

	public static int CountOnlineSince(int siteId, DateTime sinceTime)
	{
		string sqlCommand = @"
SELECT COUNT(*) FROM mp_Users WHERE SiteID = ?SiteID 
AND LastActivityDate > ?SinceTime ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?SinceTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = sinceTime
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams).ToString());

		return count;

	}

	public static IDataReader GetUsersOnlineSince(int siteId, DateTime sinceTime)
	{
		string sqlCommand = @"
SELECT * FROM mp_Users 
WHERE SiteID = ?SiteID 
AND LastActivityDate >= ?SinceTime   
AND DisplayInMemberList = 1 ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?SinceTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = sinceTime
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);


	}

	public static IDataReader GetTop50UsersOnlineSince(int siteId, DateTime sinceTime)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Users 
WHERE SiteID = ?SiteID 
AND LastActivityDate >= ?SinceTime 
ORDER BY LastActivityDate desc 
LIMIT 50 ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?SinceTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = sinceTime
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);


	}

	public static int GetNewestUserId(int siteId)
	{
		string sqlCommand = @"
SELECT MAX(UserID) FROM mp_Users WHERE SiteID = ?SiteID;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams).ToString());

		return count;
	}



	public static int Count(int siteId, string userNameBeginsWith)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Users 
WHERE SiteID = ?SiteID 
AND IsDeleted = 0 
AND ProfileApproved = 1 ";

		if (userNameBeginsWith.Length > 0)
		{
			sqlCommand += " AND Name  LIKE ?UserNameBeginsWith ";
		}
		sqlCommand += " ;  ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?UserNameBeginsWith", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = userNameBeginsWith + "%"
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

		return count;

	}


	public static IDataReader GetUserListPage(
		int siteId,
		int pageNumber,
		int pageSize,
		string beginsWith,
		int sortMode,
		string nameFilterMode,
		out int totalPages
	)
	{

		string sqlCommand1 = @"
SELECT * 
FROM mp_Users u
WHERE u.ProfileApproved = 1
AND DisplayInMemberList = 1
AND u.SiteID = ?SiteID
AND u.IsDeleted = 0";

		sqlCommand1 += nameFilterMode switch
		{
			"lastname" => " AND Lower(LastName) LIKE LOWER(?BeginsWith)",
			_ => " AND Lower(Name) LIKE LOWER(?BeginsWith)",
		};
		sqlCommand1 += sortMode switch
		{
			1 => " ORDER BY u.DateCreated DESC",
			2 => " ORDER BY u.LastName, u.FirstName, u.Name",
			_ => " ORDER BY u.Name",
		};
		sqlCommand1 += $" LIMIT ?Offset,?PageSize;";

		var offset = (pageSize * pageNumber) - pageSize;
		var totalRows = UserCount(siteId, beginsWith, nameFilterMode);

		// VS says that one of the casts are redundant, but I remember it being an issue in the past so we'll just leave it
		totalPages = (int)Math.Ceiling((decimal)totalRows / (decimal)pageSize);

		var commandParameters = new MySqlParameter[]
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new("?BeginsWith", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = beginsWith + "%"
			},
			new("?Offset", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = offset
			},
			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			},
			new("?NameFilterMode", MySqlDbType.VarChar, 10)
			{
				Direction = ParameterDirection.Input,
				Value = nameFilterMode
			},
			new("?SortMode", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sortMode
			},
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand1,
			commandParameters
		);
	}


	private static int CountForSearch(int siteId, string searchInput)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Users 
WHERE SiteID = ?SiteID
AND ProfileApproved = 1 
AND DisplayInMemberList = 1 
AND IsDeleted = 0 ";

		if (searchInput.Length > 0)
		{
			sqlCommand += @" 
AND ((Lower(Name) LIKE LOWER(?SearchInput)) 
OR (Lower(LoginName) LIKE LOWER(?SearchInput))
OR (Lower(LastName) LIKE LOWER(?SearchInput)) 
OR (Lower(FirstName) LIKE LOWER(?SearchInput))";
		}

		sqlCommand += " ;  ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?SearchInput", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = "%" + searchInput + "%"
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

		return count;

	}

	public static IDataReader GetUserSearchPage(
		int siteId,
		int pageNumber,
		int pageSize,
		string searchInput,
		int sortMode,
		out int totalPages)
	{

		int pageLowerBound = (pageSize * pageNumber) - pageSize;

		int totalRows
			= CountForSearch(siteId, searchInput);

		totalPages = 1;
		if (pageSize > 0)
			totalPages = totalRows / pageSize;

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

		string sqlCommand = @"
SELECT *  
FROM mp_Users  
WHERE SiteID = ?SiteID    
AND ProfileApproved = 1 
AND DisplayInMemberList = 1 
AND IsDeleted = 0 ";

		if (searchInput.Length > 0)
		{
			sqlCommand += @" 
AND ((Lower(Name) LIKE LOWER(?SearchInput)) 
OR (Lower(LoginName) LIKE LOWER(?SearchInput))
OR (Lower(LastName) LIKE LOWER(?SearchInput)) 
OR (Lower(FirstName) LIKE LOWER(?SearchInput)))";

		}

		switch (sortMode)
		{
			case 1:
				sqlCommand += " ORDER BY DateCreated DESC ";
				break;

			case 2:
				sqlCommand += " ORDER BY LastName, FirstName,  Name ";
				break;

			case 0:
			default:
				sqlCommand += " ORDER BY Name ";
				break;
		}

		sqlCommand += "LIMIT " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + ", ?PageSize  ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			},

			new("?SearchInput", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = "%" + searchInput + "%"
			},

			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	private static int CountForAdminSearch(int siteId, string searchInput)
	{
		string sqlCommand = "SELECT Count(*) FROM mp_Users WHERE SiteID = ?SiteID ";

		if (searchInput.Length > 0)
		{
			sqlCommand += " AND ";
			sqlCommand += "(";
			sqlCommand += " (Name LIKE ?SearchInput) ";
			sqlCommand += " OR ";
			sqlCommand += " (LoginName LIKE ?SearchInput) ";
			sqlCommand += " OR ";
			sqlCommand += " (Email LIKE ?SearchInput) ";
			sqlCommand += ")";
		}

		sqlCommand += " ;  ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?SearchInput", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = "%" + searchInput + "%"
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

		return count;

	}

	public static IDataReader GetUserAdminSearchPage(
		int siteId,
		int pageNumber,
		int pageSize,
		string searchInput,
		int sortMode,
		out int totalPages)
	{

		int pageLowerBound = (pageSize * pageNumber) - pageSize;

		int totalRows = CountForAdminSearch(siteId, searchInput);

		totalPages = 1;
		if (pageSize > 0)
			totalPages = totalRows / pageSize;

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
SELECT *  FROM mp_Users WHERE SiteID = ?SiteID  ";

		if (searchInput.Length > 0)
		{
			sqlCommand += @" 
AND ((Lower(Name) 
LIKE LOWER(?SearchInput)) 
OR (Lower(LoginName) LIKE LOWER(?SearchInput)) 
OR (Lower(LastName) LIKE LOWER(?SearchInput)) 
OR (Lower(FirstName) LIKE LOWER(?SearchInput)) 
OR (Lower(Email) LIKE LOWER(?SearchInput)) 
)";
		}

		switch (sortMode)
		{
			case 1:
				sqlCommand += " ORDER BY DateCreated DESC ";
				break;

			case 2:
				sqlCommand += " ORDER BY LastName, FirstName,  Name ";
				break;

			case 0:
			default:
				sqlCommand += " ORDER BY Name ";
				break;
		}

		sqlCommand += "LIMIT " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + ", ?PageSize  ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			},

			new("?SearchInput", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = "%" + searchInput + "%"
			},

			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetPageLockedUsers(
		int siteId,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		totalPages = 1;
		int totalRows = CountLockedOutUsers(siteId);
		int pageLowerBound = (pageSize * pageNumber) - pageSize;

		if (pageSize > 0)
			totalPages = totalRows / pageSize;

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
SELECT *  FROM	
	mp_Users  
WHERE   
	SiteID = ?SiteID    
AND 
	IsLockedOut = 1 
ORDER BY Name 
LIMIT " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + ", ?PageSize  ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
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

	public static IDataReader GetPageNotApprovedUsers(
		int siteId,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		totalPages = 1;
		int totalRows = CountNotApprovedUsers(siteId);
		int pageLowerBound = (pageSize * pageNumber) - pageSize;

		if (pageSize > 0)
			totalPages = totalRows / pageSize;

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
FROM mp_Users  
WHERE SiteID = ?SiteID    
AND ApprovedForForums = 0 
ORDER BY Name 
LIMIT " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + ", ?PageSize  ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
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




	public static int AddUser(
		Guid siteGuid,
		int siteId,
		string fullName,
		String loginName,
		string email,
		string password,
		string passwordSalt,
		Guid userGuid,
		DateTime dateCreated,
		bool mustChangePwd,
		string firstName,
		string lastName,
		string timeZoneId,
		DateTime dateOfBirth,
		bool emailConfirmed,
		int pwdFormat,
		string passwordHash,
		string securityStamp,
		string phoneNumber,
		bool phoneNumberConfirmed,
		bool twoFactorEnabled,
		DateTime? lockoutEndDateUtc)
	{
		#region bit conversion

		int intmustChangePwd = 0;
		if (mustChangePwd)
		{ intmustChangePwd = 1; }
		int intEmailConfirmed = 0;
		if (emailConfirmed)
		{ intEmailConfirmed = 1; }
		int intPhoneNumberConfirmed = 0;
		if (phoneNumberConfirmed)
		{ intPhoneNumberConfirmed = 1; }
		int intTwoFactorEnabled = 0;
		if (twoFactorEnabled)
		{ intTwoFactorEnabled = 1; }

		#endregion

		string sqlCommand = @"
INSERT INTO 
	mp_Users (
		SiteGuid, 
		SiteID, 
		Name, 
		LoginName, 
		Email, 
		FirstName, 
		LastName, 
		TimeZoneId, 
		EmailChangeGuid, 
		PasswordResetGuid, 
		Pwd, 
		PasswordSalt, 
		MustChangePwd, 
		RolesChanged, 
		DateCreated, 
		TotalPosts, 
		TotalRevenue, 
		DateOfBirth, 
		EmailConfirmed, 
		PwdFormat, 
		PasswordHash, 
		SecurityStamp, 
		PhoneNumber, 
		PhoneNumberConfirmed, 
		TwoFactorEnabled, 
		LockoutEndDateUtc, 
		UserGuid
)
VALUES (
	?SiteGuid , 
	?SiteID , 
	?FullName , 
	?LoginName , 
	?Email , 
	?FirstName, 
	?LastName, 
	?TimeZoneId, 
	?EmailChangeGuid, 
	'00000000-0000-0000-0000-000000000000', 
	?Password, 
	?PasswordSalt, 
	?MustChangePwd, 
	0, 
	?DateCreated, 
	0, 
	0, 
	?DateOfBirth, 
	?EmailConfirmed, 
	?PwdFormat, 
	?PasswordHash, 
	?SecurityStamp, 
	?PhoneNumber, 
	?PhoneNumberConfirmed, 
	?TwoFactorEnabled, 
	?LockoutEndDateUtc, 
	?UserGuid 
);
SELECT LAST_INSERT_ID();";

		var arParams = new List<MySqlParameter>
		{
			new("?FullName", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = fullName
			},

			new("?LoginName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = loginName
			},

			new("?Email", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = email
			},

			new("?Password", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = password
			},

			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?DateCreated", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = dateCreated
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?MustChangePwd", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intmustChangePwd
			},

			new("?FirstName", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = firstName
			},

			new("?LastName", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = lastName
			},

			new("?TimeZoneId", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = timeZoneId
			},

			new("?EmailChangeGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = Guid.Empty.ToString()
			},

			new("?PasswordSalt", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = passwordSalt
			},

			//new("?DateOfBirth", MySqlDbType.DateTime)
			//{
			//	Direction = ParameterDirection.Input
			//},

			new("?EmailConfirmed", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intEmailConfirmed
			},

			new("?PwdFormat", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pwdFormat
			},

			new("?PasswordHash", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = passwordHash
			},

			new("?SecurityStamp", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = securityStamp
			},

			new("?PhoneNumber", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = phoneNumber
			},

			new("?PhoneNumberConfirmed", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intPhoneNumberConfirmed
			},

			new("?TwoFactorEnabled", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intTwoFactorEnabled
			},

			//new("?LockoutEndDateUtc", MySqlDbType.DateTime)
			//{
			//	Direction = ParameterDirection.Input
			//}
		};


		if (dateOfBirth == DateTime.MinValue)
		{
			arParams.Add(new("?DateOfBirth", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DBNull.Value
			});
		}
		else
		{
			arParams.Add(new("?DateOfBirth", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = dateOfBirth
			});
		}


		if (lockoutEndDateUtc == null)
		{
			arParams.Add(new("?LockoutEndDateUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DBNull.Value
			});
		}
		else
		{
			arParams.Add(new("?LockoutEndDateUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lockoutEndDateUtc
			});
		}

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams).ToString());

		return newID;

	}

	public static bool UpdateUser(
		int userId,
		String fullName,
		String loginName,
		String email,
		String password,
		string passwordSalt,
		String gender,
		bool profileApproved,
		bool approvedForForums,
		bool trusted,
		bool displayInMemberList,
		String webSiteUrl,
		String country,
		String state,
		String occupation,
		String interests,
		String msn,
		String yahoo,
		String aim,
		String icq,
		String avatarUrl,
		String signature,
		String skin,
		String loweredEmail,
		String passwordQuestion,
		String passwordAnswer,
		String comment,
		int timeOffsetHours,
		string openIdUri,
		string windowsLiveId,
		bool mustChangePwd,
		string firstName,
		string lastName,
		string timeZoneId,
		string editorPreference,
		string newEmail,
		Guid emailChangeGuid,
		Guid passwordResetGuid,
		bool rolesChanged,
		string authorBio,
		DateTime dateOfBirth,
		bool emailConfirmed,
		int pwdFormat,
		string passwordHash,
		string securityStamp,
		string phoneNumber,
		bool phoneNumberConfirmed,
		bool twoFactorEnabled,
		DateTime? lockoutEndDateUtc)
	{
		#region bit conversion

		byte approved = 1;
		if (!profileApproved)
		{
			approved = 0;
		}

		byte canPost = 1;
		if (!approvedForForums)
		{
			canPost = 0;
		}

		byte trust = 1;
		if (!trusted)
		{
			trust = 0;
		}

		byte displayInList = 1;
		if (!displayInMemberList)
		{
			displayInList = 0;
		}
		int intmustChangePwd = 0;
		if (mustChangePwd)
		{ intmustChangePwd = 1; }

		int introlesChanged = 0;
		if (rolesChanged)
		{ introlesChanged = 1; }
		int intEmailConfirmed = 0;
		if (emailConfirmed)
		{ intEmailConfirmed = 1; }

		int intPhoneNumberConfirmed = 0;
		if (phoneNumberConfirmed)
		{ intPhoneNumberConfirmed = 1; }

		int intTwoFactorEnabled = 0;
		if (twoFactorEnabled)
		{ intTwoFactorEnabled = 1; }

		#endregion


		string sqlCommand = @"
UPDATE 
	mp_Users 
SET 
	Email = ?Email ,   
	Name = ?FullName,    
	LoginName = ?LoginName,    
	FirstName = ?FirstName,    
	LastName = ?LastName,    
	TimeZoneId = ?TimeZoneId,    
	EditorPreference = ?EditorPreference,    
	NewEmail = ?NewEmail,    
	EmailChangeGuid = ?EmailChangeGuid,    
	PasswordResetGuid = ?PasswordResetGuid,    
	Pwd = ?Password,    
	PasswordSalt = ?PasswordSalt,    
	MustChangePwd = ?MustChangePwd,    
	RolesChanged = ?RolesChanged,    
	Gender = ?Gender,    
	ProfileApproved = ?ProfileApproved,    
	ApprovedForForums = ?ApprovedForForums,    
	Trusted = ?Trusted,    
	DisplayInMemberList = ?DisplayInMemberList,    
	WebSiteURL = ?WebSiteURL,    
	Country = ?Country,    
	State = ?State,    
	Occupation = ?Occupation,    
	Interests = ?Interests,    
	MSN = ?MSN,    
	Yahoo = ?Yahoo,   
	AIM = ?AIM,   
	ICQ = ?ICQ,    
	AvatarUrl = ?AvatarUrl,    
	Signature = ?Signature,    
	Skin = ?Skin,    
	LoweredEmail = ?LoweredEmail, 
	PasswordQuestion = ?PasswordQuestion, 
	PasswordAnswer = ?PasswordAnswer, 
	Comment = ?Comment, 
	OpenIDURI = ?OpenIDURI, 
	WindowsLiveID = ?WindowsLiveID, 
	AuthorBio = ?AuthorBio, 
	DateOfBirth = ?DateOfBirth, 
	EmailConfirmed = ?EmailConfirmed, 
	PwdFormat = ?PwdFormat, 
	PasswordHash = ?PasswordHash, 
	SecurityStamp = ?SecurityStamp, 
	PhoneNumber = ?PhoneNumber, 
	PhoneNumberConfirmed = ?PhoneNumberConfirmed, 
	TwoFactorEnabled = ?TwoFactorEnabled, 
	LockoutEndDateUtc = ?LockoutEndDateUtc, 
	TimeOffsetHours = ?TimeOffsetHours    
WHERE 
	UserID = ?UserID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},

			new("?Email", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = email
			},

			new("?Password", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = password
			},

			new("?Gender", MySqlDbType.VarChar, 1)
			{
				Direction = ParameterDirection.Input,
				Value = gender
			},

			new("?ProfileApproved", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = approved
			},

			new("?ApprovedForForums", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = canPost
			},

			new("?Trusted", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = trust
			},

			new("?DisplayInMemberList", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = displayInList
			},

			new("?WebSiteURL", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = webSiteUrl
			},

			new("?Country", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = country
			},

			new("?State", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = state
			},

			new("?Occupation", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = occupation
			},

			new("?Interests", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = interests
			},

			new("?MSN", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = msn
			},

			new("?Yahoo", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = yahoo
			},

			new("?AIM", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = aim
			},

			new("?ICQ", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = icq
			},

			new("?AvatarUrl", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = avatarUrl
			},

			new("?Signature", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = signature
			},

			new("?Skin", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = skin
			},

			new("?FullName", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = fullName
			},

			new("?LoginName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = loginName
			},

			new("?LoweredEmail", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = loweredEmail
			},

			new("?PasswordQuestion", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = passwordQuestion
			},

			new("?PasswordAnswer", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = passwordAnswer
			},

			new("?Comment", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = comment
			},

			new("?TimeOffsetHours", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = timeOffsetHours
			},

			new("?OpenIDURI", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = openIdUri
			},

			new("?WindowsLiveID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = windowsLiveId
			},

			new("?MustChangePwd", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intmustChangePwd
			},

			new("?FirstName", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = firstName
			},

			new("?LastName", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = lastName
			},

			new("?TimeZoneId", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = timeZoneId
			},

			new("?EditorPreference", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = editorPreference
			},

			new("?NewEmail", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = newEmail
			},


			new("?EmailChangeGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = emailChangeGuid.ToString()
			},

			new("?PasswordResetGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = passwordResetGuid.ToString()
			},

			new("?PasswordSalt", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = passwordSalt
			},

			new("?RolesChanged", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = introlesChanged
			},

			new("?AuthorBio", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = authorBio
			},

			//new("?DateOfBirth", MySqlDbType.DateTime)
			//{
			//	Direction = ParameterDirection.Input
			//},

			new("?EmailConfirmed", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intEmailConfirmed
			},

			new("?PwdFormat", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pwdFormat
			},

			new("?PasswordHash", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = passwordHash
			},

			new("?SecurityStamp", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = securityStamp
			},

			new("?PhoneNumber", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = phoneNumber
			},

			new("?PhoneNumberConfirmed", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intPhoneNumberConfirmed
			},

			new("?TwoFactorEnabled", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intTwoFactorEnabled
			},

			//new("?LockoutEndDateUtc", MySqlDbType.DateTime)
			//{
			//	Direction = ParameterDirection.Input
			//}
		};


		if (dateOfBirth == DateTime.MinValue)
		{
			arParams.Add(new("?DateOfBirth", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DBNull.Value
			});
		}
		else
		{
			arParams.Add(new("?DateOfBirth", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = dateOfBirth
			});
		}


		if (lockoutEndDateUtc == null)
		{
			arParams.Add(new("?LockoutEndDateUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DBNull.Value
			});
		}
		else
		{
			arParams.Add(new("?LockoutEndDateUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lockoutEndDateUtc
			});
		}


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}


	public static bool DeleteUser(int userId)
	{
		string sqlCommand = @"
DELETE FROM mp_Users 
WHERE UserID = ?UserID  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);


		return rowsAffected > 0;
	}

	public static bool UpdateLastActivityTime(Guid userGuid, DateTime lastUpdate)
	{
		string sqlCommand = @"
UPDATE mp_Users 
SET LastActivityDate = ?LastUpdate  
WHERE UserGuid = ?UserGuid  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?LastUpdate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastUpdate
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool UpdateLastLoginTime(Guid userGuid, DateTime lastLoginTime)
	{
		string sqlCommand = @"
UPDATE mp_Users 
SET 
	LastLoginDate = ?LastLoginTime,  
	FailedPasswordAttemptCount = 0, 
	FailedPwdAnswerAttemptCount = 0 
WHERE UserGuid = ?UserGuid  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?LastLoginTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastLoginTime
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool AccountLockout(Guid userGuid, DateTime lockoutTime)
	{
		string sqlCommand = @"
UPDATE mp_Users 
SET 
	IsLockedOut = 1,  
	LastLockoutDate = ?LockoutTime  
WHERE UserGuid = ?UserGuid  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?LockoutTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lockoutTime
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool UpdateLastPasswordChangeTime(Guid userGuid, DateTime lastPasswordChangeTime)
	{
		string sqlCommand = @"
UPDATE mp_Users 
SET LastPasswordChangedDate = ?LastPasswordChangedDate  
WHERE UserGuid = ?UserGuid  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?LastPasswordChangedDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastPasswordChangeTime
			}
		};

		int rowsAffected = 0;

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool UpdateFailedPasswordAttemptStartWindow(
		Guid userGuid,
		DateTime windowStartTime)
	{
		string sqlCommand = @"
UPDATE mp_Users 
SET FailedPwdAttemptWindowStart = ?FailedPasswordAttemptWindowStart  
WHERE UserGuid = ?UserGuid  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?FailedPasswordAttemptWindowStart", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = windowStartTime
			}
		};

		int rowsAffected = 0;

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool UpdateFailedPasswordAttemptCount(
		Guid userGuid,
		int attemptCount)
	{
		string sqlCommand = @"
UPDATE mp_Users 
SET FailedPasswordAttemptCount = ?AttemptCount  
WHERE UserGuid = ?UserGuid  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?AttemptCount", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = attemptCount
			}
		};

		int rowsAffected = 0;

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool UpdateFailedPasswordAnswerAttemptStartWindow(
		Guid userGuid,
		DateTime windowStartTime)
	{
		string sqlCommand = @"
UPDATE mp_Users 
SET FailedPwdAnswerWindowStart = ?FailedPasswordAnswerAttemptWindowStart  
WHERE UserGuid = ?UserGuid  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?FailedPasswordAnswerAttemptWindowStart", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = windowStartTime
			}
		};

		int rowsAffected = 0;

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool UpdateFailedPasswordAnswerAttemptCount(
		Guid userGuid,
		int attemptCount)
	{
		string sqlCommand = @"
UPDATE mp_Users 
SET FailedPwdAnswerAttemptCount = ?AttemptCount  
WHERE UserGuid = ?UserGuid  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?AttemptCount", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = attemptCount
			}
		};

		int rowsAffected = 0;

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool SetRegistrationConfirmationGuid(Guid userGuid, Guid registrationConfirmationGuid)
	{
		string sqlCommand = @"
UPDATE mp_Users 
SET IsLockedOut = 1,  
RegisterConfirmGuid = ?RegisterConfirmGuid  
WHERE UserGuid = ?UserGuid  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?RegisterConfirmGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = registrationConfirmationGuid.ToString()
			}
		};

		int rowsAffected = 0;

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool ConfirmRegistration(Guid emptyGuid, Guid registrationConfirmationGuid)
	{
		string sqlCommand = @"
UPDATE mp_Users 
SET 
	IsLockedOut = 0,  
	RegisterConfirmGuid = ?EmptyGuid  
WHERE RegisterConfirmGuid = ?RegisterConfirmGuid  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?EmptyGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = emptyGuid.ToString()
			},

			new("?RegisterConfirmGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = registrationConfirmationGuid.ToString()
			}
		};

		int rowsAffected = 0;

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}

	public static bool AccountClearLockout(Guid userGuid)
	{
		string sqlCommand = @"
UPDATE mp_Users 
SET 
	IsLockedOut = 0,  
	FailedPasswordAttemptCount = 0, 
	FailedPwdAnswerAttemptCount = 0 
WHERE UserGuid = ?UserGuid  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};

		int rowsAffected = 0;

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}

	public static bool UpdatePasswordAndSalt(
		int userId,
		int pwdFormat,
		string password,
		string passwordSalt)
	{
		string sqlCommand = @"
UPDATE mp_Users 
SET 
	Pwd = ?Password,  
	PasswordSalt = ?PasswordSalt,  
	PwdFormat = ?PwdFormat  
WHERE UserID = ?UserID  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},

			new("?Password", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = password
			},

			new("?PasswordSalt", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = passwordSalt
			},

			new("?PwdFormat", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pwdFormat
			}
		};



		int rowsAffected = 0;

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool UpdatePasswordQuestionAndAnswer(
		Guid userGuid,
		String passwordQuestion,
		String passwordAnswer)
	{
		string sqlCommand = @"
UPDATE mp_Users 
SET 
	PasswordQuestion = ?PasswordQuestion,  
	PasswordAnswer = ?PasswordAnswer  
WHERE UserGuid = ?UserGuid  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?PasswordQuestion", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = passwordQuestion
			},

			new("?PasswordAnswer", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = passwordAnswer
			}
		};

		int rowsAffected = 0;

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}

	public static void UpdateTotalRevenue(Guid userGuid)
	{
		string sqlCommand = @"
UPDATE mp_Users 
SET TotalRevenue = COALESCE((  
	SELECT SUM(SubTotal) 
	FROM mp_CommerceReport 
	WHERE UserGuid = ?UserGuid)  
, 0) 
WHERE UserGuid = ?UserGuid  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};

		CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

	}

	public static void UpdateTotalRevenue()
	{
		string sqlCommand = @"
UPDATE mp_Users 
SET TotalRevenue = COALESCE((  
	SELECT SUM(SubTotal) 
	FROM mp_CommerceReport 
	WHERE UserGuid = mp_Users.UserGuid)  
, 0) ;";

		CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString());
	}



	public static bool FlagAsDeleted(int userId)
	{
		string sqlCommand = @"
UPDATE mp_Users 
SET IsDeleted = 1 
WHERE UserID = ?UserID  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			}
		};

		int rowsAffected = 0;

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}

	public static bool FlagAsNotDeleted(int userId)
	{
		string sqlCommand = @"
UPDATE mp_Users 
SET IsDeleted = 0 
WHERE UserID = ?UserID  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			}
		};


		int rowsAffected = 0;

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}

	public static bool IncrementTotalPosts(int userId)
	{

		string sqlCommand = @"
UPDATE mp_Users 
SET	TotalPosts = TotalPosts + 1 
WHERE UserID = ?UserID  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			}
		};

		int rowsAffected = 0;

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);


		return rowsAffected > 0;
	}

	public static bool DecrementTotalPosts(int userId)
	{

		string sqlCommand = @"
UPDATE mp_Users 
SET	TotalPosts = TotalPosts - 1 
WHERE UserID = ?UserID  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			}
		};

		int rowsAffected = 0;

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}

	public static IDataReader GetRolesByUser(int siteId, int userId)
	{
		string sqlCommand = @"
SELECT 
	mp_Roles.RoleID, 
	mp_Roles.DisplayName, 
	mp_Roles.RoleName 
FROM mp_UserRoles 
INNER JOIN mp_Users 
ON mp_UserRoles.UserID = mp_Users.UserID 
INNER JOIN mp_Roles 
ON mp_UserRoles.RoleID = mp_Roles.RoleID 
WHERE mp_Users.SiteID = ?SiteID 
AND mp_Users.UserID = ?UserID  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetUserByRegistrationGuid(int siteId, Guid registerConfirmGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Users 
WHERE SiteID = ?SiteID AND RegisterConfirmGuid = ?RegisterConfirmGuid  ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?RegisterConfirmGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = registerConfirmGuid
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}


	public static IDataReader GetSingleUser(int siteId, string email)
	{
		string sqlCommand = @"
SELECT * 
FROM	mp_Users 
WHERE SiteID = ?SiteID AND LoweredEmail = ?Email  ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?Email", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = email.ToLower()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetSingleUserByLoginName(int siteId, string loginName, bool allowEmailFallback)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Users 
WHERE SiteID = ?SiteID  ";

		if (allowEmailFallback)
		{
			sqlCommand += "AND ";
			sqlCommand += "(";
			sqlCommand += "LoginName = ?LoginName ";
			sqlCommand += "OR Email = ?LoginName ";
			sqlCommand += ")";
		}
		else
		{
			sqlCommand += "AND LoginName = ?LoginName ";
		}

		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?LoginName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = loginName
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}


	public static IDataReader GetSingleUser(int userId, int siteId)
	{
		const string sqlCommand = @"
SELECT
	*
FROM
	mp_Users
WHERE
	UserID = ?UserID
AND
	SiteID = ?SiteID;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},

			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams.ToArray()
		);
	}


	public static IDataReader GetSingleUser(Guid userGuid, int siteId)
	{
		const string sqlCommand = @"
SELECT
	*
FROM
	mp_Users
WHERE
	UserGuid = ?UserGuid
AND
	SiteID = ?SiteID;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams.ToArray()
		);
	}


	public static Guid GetUserGuidFromOpenId(
		int siteId,
		string openIdUri)
	{
		string sqlCommand = @"
SELECT UserGuid 
FROM mp_Users 
WHERE SiteID = ?SiteID  
AND OpenIDURI = ?OpenIDURI  ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?OpenIDURI", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = openIdUri
			}
		};

		Guid userGuid = Guid.Empty;

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams))
		{

			if (reader.Read())
			{
				userGuid = new Guid(reader["UserGuid"].ToString());
			}
		}

		return userGuid;

	}

	public static Guid GetUserGuidFromWindowsLiveId(
		int siteId,
		string windowsLiveId)
	{
		string sqlCommand = @"
SELECT UserGuid 
FROM mp_Users 
WHERE SiteID = ?SiteID  
AND WindowsLiveID = ?WindowsLiveID ;  ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?WindowsLiveID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = windowsLiveId
			}
		};

		Guid userGuid = Guid.Empty;

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams))
		{

			if (reader.Read())
			{
				userGuid = new Guid(reader["UserGuid"].ToString());
			}
		}

		return userGuid;

	}

	public static string LoginByEmail(int siteId, string email, string password)
	{
		string sqlCommand = @"
SELECT Name 
FROM  mp_Users 
WHERE Email = ?Email  
AND SiteID = ?SiteID  
AND IsDeleted = 0 
AND Pwd = ?Password ;  ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?Email", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = email
			},

			new("?Password", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = password
			}
		};

		string userName = string.Empty;

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams))
		{
			if (reader.Read())
			{
				userName = reader["Name"].ToString();
			};

		}

		return userName;
	}

	public static string Login(int siteId, string loginName, string password)
	{

		string sqlCommand = @"
SELECT Name 
FROM  mp_Users 
WHERE LoginName = ?LoginName  
AND SiteID = ?SiteID  
AND IsDeleted = 0 
AND Pwd = ?Password ;  ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?LoginName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = loginName
			},

			new("?Password", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = password
			}
		};

		string userName = string.Empty;

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams))
		{
			if (reader.Read())
			{
				userName = reader["Name"].ToString();
			}

		}
		return userName;
	}

	public static DataTable GetNonLazyLoadedPropertiesForUser(Guid userGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_UserProperties 
WHERE 
UserGuid = ?UserGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};

		DataTable dataTable = new();
		dataTable.Columns.Add("UserGuid", typeof(String));
		dataTable.Columns.Add("PropertyName", typeof(String));
		dataTable.Columns.Add("PropertyValueString", typeof(String));
		dataTable.Columns.Add("PropertyValueBinary", typeof(object));

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams))
		{
			while (reader.Read())
			{
				DataRow row = dataTable.NewRow();
				row["UserGuid"] = reader["UserGuid"].ToString();
				row["PropertyName"] = reader["PropertyName"].ToString();
				row["PropertyValueString"] = reader["PropertyValueString"].ToString();
				row["PropertyValueBinary"] = reader["PropertyValueBinary"];
				dataTable.Rows.Add(row);
			}

		}

		return dataTable;
	}

	public static IDataReader GetLazyLoadedProperty(Guid userGuid, String propertyName)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_UserProperties 
WHERE UserGuid = ?UserGuid AND PropertyName = ?PropertyName  
LIMIT 1 ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?PropertyName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = propertyName
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static bool PropertyExists(Guid userGuid, string propertyName)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_UserProperties 
WHERE UserGuid = ?UserGuid AND PropertyName = ?PropertyName ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?PropertyName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = propertyName
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

		return count > 0;

	}

	public static void CreateProperty(
		Guid propertyId,
		Guid userGuid,
		String propertyName,
		String propertyValues,
		byte[] propertyValueb,
		DateTime lastUpdatedDate,
		bool isLazyLoaded)
	{
		string sqlCommand = @"
INSERT INTO mp_UserProperties (
	PropertyID, 
	UserGuid, 
	PropertyName, 
	PropertyValueString, 
	PropertyValueBinary, 
	LastUpdatedDate, 
	IsLazyLoaded 
)
VALUES (
	?PropertyID, 
	?UserGuid, 
	?PropertyName, 
	?PropertyValueString, 
	?PropertyValueBinary, 
	?LastUpdatedDate, 
	?IsLazyLoaded 
);";


		var arParams = new List<MySqlParameter>
		{
			new("?PropertyID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = propertyId.ToString()
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?PropertyName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = propertyName
			},

			new("?PropertyValueString", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = propertyValues
			},

			new("?PropertyValueBinary", MySqlDbType.LongBlob)
			{
				Direction = ParameterDirection.Input,
				Value = propertyValueb
			},

			new("?LastUpdatedDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastUpdatedDate
			},

			new("?IsLazyLoaded", MySqlDbType.Bit)
			{
				Direction = ParameterDirection.Input,
				Value = isLazyLoaded
			}
		};

		CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

	}

	public static void UpdateProperty(
		Guid userGuid,
		String propertyName,
		String propertyValues,
		byte[] propertyValueb,
		DateTime lastUpdatedDate,
		bool isLazyLoaded)
	{
		string sqlCommand = @"

UPDATE mp_UserProperties 
SET  
	PropertyValueString = ?PropertyValueString, 
	PropertyValueBinary = ?PropertyValueBinary, 
	LastUpdatedDate = ?LastUpdatedDate, 
	IsLazyLoaded = ?IsLazyLoaded 
WHERE  
 UserGuid = ?UserGuid AND PropertyName = ?PropertyName ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?PropertyName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = propertyName
			},

			new("?PropertyValueString", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = propertyValues
			},

			new("?PropertyValueBinary", MySqlDbType.LongBlob)
			{
				Direction = ParameterDirection.Input,
				Value = propertyValueb
			},

			new("?LastUpdatedDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastUpdatedDate
			},

			new("?IsLazyLoaded", MySqlDbType.Bit)
			{
				Direction = ParameterDirection.Input,
				Value = isLazyLoaded
			}
		};

		CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);


	}

	public static bool DeletePropertiesByUser(Guid userGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_UserProperties 
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
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}



}
