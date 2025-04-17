using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;

namespace mojoPortal.Data;

public static class DBSiteUser
{
	public static IDataReader GetUserCountByYearMonth(int siteId)
	{
		var sqlCommand = """
			SELECT 
				YEAR(DateCreated) As Y,  
				MONTH(DateCreated) As M, 
				CONCAT(YEAR(DateCreated), '-', MONTH(DateCreated)) As Label, 
				COUNT(*) As Users 
			FROM mp_Users 
			WHERE SiteID = ?SiteID 
			GROUP BY YEAR(DateCreated), MONTH(DateCreated) 
			ORDER BY YEAR(DateCreated), MONTH(DateCreated); 
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
		];

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);
	}

	public static IDataReader GetUserList(int siteId)
	{
		var sqlCommand = """
			SELECT 
				UserID,
				Name, 
				PasswordSalt, 
				Pwd, 
				Email 
			FROM mp_Users 
			WHERE SiteID = ?SiteID 
			ORDER BY Email;
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
		];

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);
	}

	public static IDataReader GetSmartDropDownData(int siteId, string query, int rowsToGet)
	{
		var sqlCommand = $"""
			SELECT
				UserID,
				UserGuid,
				Email,
				FirstName,
				LastName,
				Name As SiteUser
			FROM mp_Users
			WHERE SiteID = ?SiteID
				AND IsDeleted = 0
				{GetUserSearchSQL(query, true)} 
			ORDER BY SiteUser
			LIMIT {rowsToGet};
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new MySqlParameter("?Query", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = $"%{query}%"
			},
		];
		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);
	}

	public static IDataReader EmailLookup(int siteId, string query, int rowsToGet)
	{
		var sqlCommand = $"""
			SELECT 
				UserID, 
				UserGuid, 
				Email 
			FROM mp_Users 
			WHERE SiteID = ?SiteID 
				AND IsDeleted = 0 
				{GetUserSearchSQL(query, true)} 
			ORDER BY Email 
			LIMIT {rowsToGet};
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new MySqlParameter("?Query", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = query + "%"
			},
		];

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);
	}

	public static int UserCount(int siteId)
	{
		var sqlCommand = """
			SELECT COUNT(*) 
			FROM mp_Users 
			WHERE SiteID = ?SiteID;
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
		];

		int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams).ToString());

		return count;
	}

	public static int CountLockedOutUsers(int siteId)
	{
		var sqlCommand = """
			SELECT COUNT(*) 
			FROM mp_Users 
			WHERE SiteID = ?SiteID 
				AND IsLockedOut = 1;
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
		];

		int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams).ToString());

		return count;
	}

	public static int CountNotApprovedUsers(int siteId)
	{
		var sqlCommand = """
			SELECT COUNT(*) 
			FROM mp_Users 
			WHERE SiteID = ?SiteID 
				AND ApprovedForForums = 0;
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
		];

		int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams).ToString());

		return count;
	}

	public static int UserCount(int siteId, string nameBeginsWith, string nameFilterMode)
	{
		var sqlCommand = $"""
			SELECT COUNT(*) 
			FROM mp_Users 
			WHERE SiteID = ?SiteID 
				AND IsDeleted = 0 
				AND ProfileApproved = 1 
				{GetNameFilterSQL(nameFilterMode)}
			;
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new MySqlParameter("?BeginsWith", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = nameBeginsWith + "%"
			}
		];

		int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams).ToString());

		return count;
	}

	public static int CountUsersByRegistrationDateRange(int siteId, DateTime beginDate, DateTime endDate)
	{
		var sqlCommand = """
			SELECT COUNT(*) 
			FROM mp_Users 
			WHERE SiteID = ?SiteID 
				AND DateCreated >= ?BeginDate 
				AND DateCreated < ?EndDate;
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new MySqlParameter("?BeginDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = beginDate
			},
			new MySqlParameter("?EndDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = endDate
			},
		];

		int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams).ToString());

		return count;
	}

	public static int CountOnlineSince(int siteId, DateTime sinceTime)
	{
		var sqlCommand = """
			SELECT COUNT(*) 
			FROM mp_Users
			WHERE SiteID = ?SiteID 
				AND LastActivityDate > ?SinceTime ;
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new MySqlParameter("?SinceTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = sinceTime
			},
		];

		int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams).ToString());

		return count;
	}

	public static IDataReader GetUsersOnlineSince(int siteId, DateTime sinceTime)
	{
		var sqlCommand = """
			SELECT * 
			FROM mp_Users 
			WHERE SiteID = ?SiteID 
				AND LastActivityDate >= ?SinceTime 
				AND DisplayInMemberList = 1;
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new MySqlParameter("?SinceTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = sinceTime
			},
		];

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);
	}

	public static IDataReader GetTopUsersSince(int siteId, DateTime sinceTime, int limit)
	{
		var sqlCommand = """
			SELECT * 
			FROM mp_Users 
			WHERE SiteID = ?SiteID 
				AND LastActivityDate >= ?SinceTime
			ORDER BY LastActivityDate DESC
			LIMIT ?Limit ; 
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new MySqlParameter("?SinceTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = sinceTime
			},
			new MySqlParameter("?Limit", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = limit
			},
		];

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);
	}

	public static int GetNewestUserId(int siteId)
	{
		var sqlCommand = "SELECT MAX(UserID) FROM mp_Users WHERE SiteID = ?SiteID;";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
		];

		int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams).ToString());

		return count;
	}

	public static int Count(int siteId, string userNameBeginsWith)
	{
		var beginsWith = string.Empty;
		if (!string.IsNullOrWhiteSpace(userNameBeginsWith))
		{
			beginsWith = "AND Name LIKE ?UserNameBeginsWith";
		}

		var sqlCommand = $"""
			SELECT Count(*) FROM mp_Users WHERE SiteID = ?SiteID 
			AND IsDeleted = 0 
			AND ProfileApproved = 1 
			{beginsWith};
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new MySqlParameter("?UserNameBeginsWith", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = $"{userNameBeginsWith}%"
			},
		];

		int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
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
		var commandText = $"""
			SELECT * 
			FROM mp_Users u
			WHERE u.ProfileApproved = 1
				AND DisplayInMemberList = 1
				AND u.SiteID = ?SiteID
				AND u.IsDeleted = 0
				{GetNameFilterSQL(nameFilterMode)}
			{GetMemberListSortMethodSQL(sortMode)}
			LIMIT ?Offset, ?PageSize;
			""";

		var offset = (pageSize * pageNumber) - pageSize;
		var totalRows = UserCount(siteId, beginsWith, nameFilterMode);

		totalPages = GetTotalPages(totalRows, pageSize);

		MySqlParameter[] commandParameters =
		[
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new("?BeginsWith", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = $"{beginsWith}%"
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
		];

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			commandText,
			commandParameters
		);
	}

	private static int CountForSearch(int siteId, string searchInput, bool isAdmin)
	{
		var restrictedSearch = string.Empty;

		if (isAdmin)
		{
			restrictedSearch = """
			AND ProfileApproved = 1 
			AND DisplayInMemberList = 1 
			AND IsDeleted = 0 
			""";
		}

		var sqlCommand = $"""
			SELECT Count(*) 
			FROM mp_Users 
			WHERE SiteID = ?SiteID
				{restrictedSearch}
				{GetUserSearchSQL(searchInput, isAdmin)};
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new MySqlParameter("?SearchInput", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = $"%{searchInput}%"
			},
		];

		int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams));

		return count;
	}

	public static IDataReader GetUserSearchPage(
		int siteId,
		int pageNumber,
		int pageSize,
		string searchInput,
		int sortMode,
		bool isAdmin,
		out int totalPages)
	{
		var totalRows = CountForSearch(siteId, searchInput, false);
		totalPages = GetTotalPages(totalRows, pageSize);

		var restrictedSearch = string.Empty;

		if (isAdmin)
		{
			restrictedSearch = """
			AND ProfileApproved = 1 
			AND DisplayInMemberList = 1 
			AND IsDeleted = 0 
			""";
		}

		var sqlCommand = $"""
			SELECT *  
			FROM	mp_Users 
			WHERE SiteID = ?SiteID 
				{restrictedSearch}
				{GetUserSearchSQL(searchInput, isAdmin)}
			{GetMemberListSortMethodSQL(sortMode)}
			LIMIT {(pageSize * pageNumber) - pageSize}, ?PageSize;
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			},
			new MySqlParameter("?SearchInput", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = $"%{searchInput}%"
			},
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
		];

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);
	}

	public static IDataReader GetUserAdminSearchPage(
		int siteId,
		int pageNumber,
		int pageSize,
		string searchInput,
		int sortMode,
		out int totalPages)
	{
		return GetUserSearchPage(siteId, pageNumber, pageSize, searchInput, sortMode, isAdmin: true, out totalPages);
	}

	public static IDataReader GetPageLockedUsers(
		int siteId,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		var totalRows = CountLockedOutUsers(siteId);
		totalPages = GetTotalPages(totalRows, pageSize);

		var sqlCommand = $"""
				SELECT *
				FROM mp_Users  
				WHERE SiteID = ?SiteID    
					AND IsLockedOut = 1 
				ORDER BY Name 
				LIMIT {(pageSize * pageNumber) - pageSize}, ?PageSize;
				""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new MySqlParameter("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			},
		];
		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}

	public static IDataReader GetPageNotApprovedUsers(
		int siteId,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		var totalRows = CountNotApprovedUsers(siteId);
		totalPages = GetTotalPages(totalRows, pageSize);

		var sqlCommand = $"""
				SELECT *
				FROM mp_Users  
				WHERE SiteID = ?SiteID    
					AND ApprovedForForums = 0 
				ORDER BY Name 
				LIMIT {(pageSize * pageNumber) - pageSize}, ?PageSize;
				""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new MySqlParameter("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			},
		];

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);
	}

	private static int GetTotalPages(int totalRows, int pageSize) => (int)Math.Ceiling((decimal)totalRows / (decimal)pageSize);

	private static string GetNameFilterSQL(string nameFilterMode)
	{
		return nameFilterMode switch
		{
			"lastname" => "AND Lower(LastName) LIKE LOWER(?BeginsWith)",
			_ => "AND Lower(Name) LIKE LOWER(?BeginsWith)",
		};
	}

	private static string GetMemberListSortMethodSQL(int sortMode) => sortMode switch
	{
		2 => "ORDER BY u.LastName DESC, u.FirstName DESC, u.Name DESC",
		3 => "ORDER BY u.DateCreated ASC",
		4 => "ORDER BY u.DateCreated DESC",
		5 => "ORDER BY u.Name",
		6 => "ORDER BY u.Name DESC",
		_ => "ORDER BY u.LastName, u.FirstName, u.Name",
	};

	public static string GetUserSearchSQL(string searchInput, bool canManageUsers = false)
	{
		if (!string.IsNullOrWhiteSpace(searchInput))
		{
			var emailSearch = string.Empty;
			if (canManageUsers)
			{
				emailSearch = """
					OR
					(LOWER(Email) LIKE LOWER(?SearchInput))
					""";
			}
			return $"""
				 AND (
						(LOWER(Name) LIKE LOWER(?SearchInput)) 
						OR 
						(LOWER(LoginName) LIKE LOWER(?SearchInput))
						OR
						(LOWER(LastName) LIKE LOWER(?SearchInput)) 
						OR
						(LOWER(FirstName) LIKE LOWER(?SearchInput)) 
						{emailSearch}
					)
				""";
		}

		return string.Empty;
	}

	public static int AddUser(
		Guid siteGuid,
		int siteId,
		string fullName,
		string loginName,
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
		var sqlCommand = """
			INSERT INTO mp_Users (
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
					?SiteGuid,
					?SiteID,
					?FullName,
					?LoginName,
					?Email,
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
			SELECT LAST_INSERT_ID();
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?FullName", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = fullName
			},
			new MySqlParameter("?LoginName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = loginName
			},
			new MySqlParameter("?Email", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = email
			},
			new MySqlParameter("?Password", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = password
			},
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
			new MySqlParameter("?DateCreated", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = dateCreated
			},
			new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},
			new MySqlParameter("?MustChangePwd", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = Convert.ToInt32(mustChangePwd)
			},
			new MySqlParameter("?FirstName", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = firstName
			},
			new MySqlParameter("?LastName", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = lastName
			},
			new MySqlParameter("?TimeZoneId", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = timeZoneId
			},
			new MySqlParameter("?EmailChangeGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = Guid.Empty.ToString()
			},
			new MySqlParameter("?PasswordSalt", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = passwordSalt
			},
			new MySqlParameter("?DateOfBirth", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = dateOfBirth == DateTime.MinValue ? DBNull.Value : dateOfBirth
			},
			new MySqlParameter("?EmailConfirmed", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = Convert.ToInt32(emailConfirmed)
			},
			new MySqlParameter("?PwdFormat", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pwdFormat
			},
			new MySqlParameter("?PasswordHash", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = passwordHash
			},
			new MySqlParameter("?SecurityStamp", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = securityStamp
			},
			new MySqlParameter("?PhoneNumber", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = phoneNumber
			},
			new MySqlParameter("?PhoneNumberConfirmed", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = Convert.ToInt32(phoneNumberConfirmed)
			},
			new MySqlParameter("?TwoFactorEnabled", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = Convert.ToInt32(twoFactorEnabled)
			},
			new MySqlParameter("?LockoutEndDateUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lockoutEndDateUtc == null ? DBNull.Value : lockoutEndDateUtc
			},
		];
		int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			arParams).ToString());

		return newID;
	}

	public static bool UpdateUser(
		int userId,
		string fullName,
		string loginName,
		string email,
		string password,
		string passwordSalt,
		string gender,
		bool profileApproved,
		bool approvedForForums,
		bool trusted,
		bool displayInMemberList,
		string webSiteUrl,
		string country,
		string state,
		string occupation,
		string interests,
		string msn,
		string yahoo,
		string aim,
		string icq,
		string avatarUrl,
		string signature,
		string skin,
		string loweredEmail,
		string passwordQuestion,
		string passwordAnswer,
		string comment,
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
		var sqlCommand = """
			UPDATE mp_Users
			SET Email = ?Email,
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
			WHERE UserID = ?UserID;
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},
			new MySqlParameter("?Email", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = email
			},
			new MySqlParameter("?Password", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = password
			},
			new MySqlParameter("?Gender", MySqlDbType.VarChar, 1)
			{
				Direction = ParameterDirection.Input,
				Value = gender
			},
			new MySqlParameter("?ProfileApproved", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = Convert.ToInt32(profileApproved)
			},
			new MySqlParameter("?ApprovedForForums", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = Convert.ToInt32(approvedForForums)
			},
			new MySqlParameter("?Trusted", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = Convert.ToInt32(trusted)
			},
			new MySqlParameter("?DisplayInMemberList", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = Convert.ToInt32(displayInMemberList)
			},
			new MySqlParameter("?WebSiteURL", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = webSiteUrl
			},
			new MySqlParameter("?Country", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = country
			},
			new MySqlParameter("?State", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = state
			},
			new MySqlParameter("?Occupation", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = occupation
			},
			new MySqlParameter("?Interests", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = interests
			},
			new MySqlParameter("?MSN", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = msn
			},
			new MySqlParameter("?Yahoo", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = yahoo
			},
			new MySqlParameter("?AIM", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = aim
			},
			new MySqlParameter("?ICQ", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = icq
			},
			new MySqlParameter("?AvatarUrl", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = avatarUrl
			},
			new MySqlParameter("?Signature", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = signature
			},
			new MySqlParameter("?Skin", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = skin
			},
			new MySqlParameter("?FullName", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = fullName
			},
			new MySqlParameter("?LoginName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = loginName
			},
			new MySqlParameter("?LoweredEmail", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = loweredEmail
			},
			new MySqlParameter("?PasswordQuestion", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = passwordQuestion
			},
			new MySqlParameter("?PasswordAnswer", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = passwordAnswer
			},
			new MySqlParameter("?Comment", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = comment
			},
			new MySqlParameter("?TimeOffsetHours", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = timeOffsetHours
			},
			new MySqlParameter("?OpenIDURI", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = openIdUri
			},
			new MySqlParameter("?WindowsLiveID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = windowsLiveId
			},
			new MySqlParameter("?MustChangePwd", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = Convert.ToInt32(mustChangePwd)
			},
			new MySqlParameter("?FirstName", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = firstName
			},
			new MySqlParameter("?LastName", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = lastName
			},
			new MySqlParameter("?TimeZoneId", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = timeZoneId
			},
			new MySqlParameter("?EditorPreference", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = editorPreference
			},
			new MySqlParameter("?NewEmail", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = newEmail
			},
			new MySqlParameter("?EmailChangeGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = emailChangeGuid.ToString()
			},
			new MySqlParameter("?PasswordResetGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = passwordResetGuid.ToString()
			},
			new MySqlParameter("?PasswordSalt", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = passwordSalt
			},
			new MySqlParameter("?RolesChanged", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = Convert.ToInt32(rolesChanged)
			},
			new MySqlParameter("?AuthorBio", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = authorBio
			},
			new MySqlParameter("?DateOfBirth", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = dateOfBirth == DateTime.MinValue ? DBNull.Value : dateOfBirth
			},
			new MySqlParameter("?EmailConfirmed", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = Convert.ToInt32(emailConfirmed)
			},
			new MySqlParameter("?PwdFormat", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pwdFormat
			},
			new MySqlParameter("?PasswordHash", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = passwordHash
			},
			new MySqlParameter("?SecurityStamp", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = securityStamp
			},
			new MySqlParameter("?PhoneNumber", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = phoneNumber
			},
			new MySqlParameter("?PhoneNumberConfirmed", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = Convert.ToInt32(phoneNumberConfirmed)
			},
			new MySqlParameter("?TwoFactorEnabled", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = Convert.ToInt32(twoFactorEnabled)
			},
			new MySqlParameter("?LockoutEndDateUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lockoutEndDateUtc == null ? DBNull.Value : lockoutEndDateUtc
			},
		];

		int rowsAffected = MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			arParams);

		return rowsAffected > 0;
	}


	public static bool DeleteUser(int userId)
	{
		var sqlCommand = """
			DELETE FROM mp_Users 
			WHERE UserID = ?UserID;
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},
		];
		int rowsAffected = MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			arParams);

		return rowsAffected > 0;
	}

	public static bool UpdateLastActivityTime(Guid userGuid, DateTime lastUpdate)
	{
		var sqlCommand = """
			UPDATE mp_Users 
			SET LastActivityDate = ?LastUpdate  
			WHERE UserGuid = ?UserGuid;
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
			new MySqlParameter("?LastUpdate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastUpdate
			},
		];
		int rowsAffected = MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}

	public static bool UpdateLastLoginTime(Guid userGuid, DateTime lastLoginTime)
	{
		var sqlCommand = """
			UPDATE mp_Users 
			SET LastLoginDate = ?LastLoginTime,
				FailedPasswordAttemptCount = 0,
				FailedPwdAnswerAttemptCount = 0 
			WHERE UserGuid = ?UserGuid;
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
			new MySqlParameter("?LastLoginTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastLoginTime
			},
		];
		int rowsAffected = MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}

	public static bool AccountLockout(Guid userGuid, DateTime lockoutTime)
	{
		var sqlCommand = """
			UPDATE mp_Users 
			SET IsLockedOut = 1, 
				LastLockoutDate = ?LockoutTime 
			WHERE UserGuid = ?UserGuid;
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
			new MySqlParameter("?LockoutTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lockoutTime
			},
		];

		int rowsAffected = MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			arParams);

		return rowsAffected > 0;
	}

	public static bool UpdateLastPasswordChangeTime(Guid userGuid, DateTime lastPasswordChangeTime)
	{
		var sqlCommand = """
			UPDATE mp_Users 
			SET LastPasswordChangedDate = ?LastPasswordChangedDate 
			WHERE UserGuid = ?UserGuid;
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
			new MySqlParameter("?LastPasswordChangedDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastPasswordChangeTime
			},
		];
		int rowsAffected = MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;
	}

	public static bool UpdateFailedPasswordAttemptStartWindow(Guid userGuid, DateTime windowStartTime)
	{
		var sqlCommand = """
			UPDATE mp_Users 
			SET FailedPwdAttemptWindowStart = ?FailedPasswordAttemptWindowStart
			WHERE UserGuid = ?UserGuid;
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
			new MySqlParameter("?FailedPasswordAttemptWindowStart", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = windowStartTime
			},
		];
		int rowsAffected = MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			arParams);
		return rowsAffected > 0;

	}

	public static bool UpdateFailedPasswordAttemptCount(Guid userGuid, int attemptCount)
	{
		var sqlCommand = """
			UPDATE mp_Users 
			SET FailedPasswordAttemptCount = ?AttemptCount  
			WHERE UserGuid = ?UserGuid;
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
			new MySqlParameter("?AttemptCount", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = attemptCount
			},
		];
		int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				arParams);
		return rowsAffected > 0;
	}

	public static bool UpdateFailedPasswordAnswerAttemptStartWindow(Guid userGuid, DateTime windowStartTime)
	{
		var sqlCommand = """
			UPDATE mp_Users 
			SET FailedPwdAnswerWindowStart = ?FailedPasswordAnswerAttemptWindowStart  
			WHERE UserGuid = ?UserGuid;
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
			new MySqlParameter("?FailedPasswordAnswerAttemptWindowStart", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = windowStartTime
			},
		];
		int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				arParams);
		return rowsAffected > 0;
	}

	public static bool UpdateFailedPasswordAnswerAttemptCount(Guid userGuid, int attemptCount)
	{
		var sqlCommand = """
			UPDATE mp_Users 
			SET FailedPwdAnswerAttemptCount = ?AttemptCount
			WHERE UserGuid = ?UserGuid;
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
			new MySqlParameter("?AttemptCount", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = attemptCount
			},
		];
		int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				arParams);
		return rowsAffected > 0;
	}

	public static bool SetRegistrationConfirmationGuid(Guid userGuid, Guid registrationConfirmationGuid)
	{
		var sqlCommand = """
			UPDATE mp_Users 
			SET IsLockedOut = 1,  
				RegisterConfirmGuid = ?RegisterConfirmGuid  
			WHERE UserGuid = ?UserGuid;
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
			new MySqlParameter("?RegisterConfirmGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = registrationConfirmationGuid.ToString()
			},
		];
		int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				arParams);
		return rowsAffected > 0;
	}

	public static bool ConfirmRegistration(Guid emptyGuid, Guid registrationConfirmationGuid)
	{
		var sqlCommand = """
			UPDATE mp_Users 
			SET   
			IsLockedOut = 0,  
			RegisterConfirmGuid = ?EmptyGuid  
			WHERE RegisterConfirmGuid = ?RegisterConfirmGuid  ;
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?EmptyGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = emptyGuid.ToString()
			},
			new MySqlParameter("?RegisterConfirmGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = registrationConfirmationGuid.ToString()
			},
		];
		int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				arParams);
		return rowsAffected > 0;
	}

	public static bool AccountClearLockout(Guid userGuid)
	{
		var sqlCommand = """
			UPDATE mp_Users 
			SET IsLockedOut = 0,  
				FailedPasswordAttemptCount = 0, 
				FailedPwdAnswerAttemptCount = 0 
			WHERE UserGuid = ?UserGuid;
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
		];
		int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				arParams);
		return rowsAffected > 0;
	}

	public static bool UpdatePasswordAndSalt(
		int userId,
		int pwdFormat,
		string password,
		string passwordSalt)
	{
		var sqlCommand = """
			UPDATE mp_Users 
			SET Pwd = ?Password,  
				PasswordSalt = ?PasswordSalt, 
				PwdFormat = ?PwdFormat 
			WHERE UserID = ?UserID;
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},
			new MySqlParameter("?Password", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = password
			},
			new MySqlParameter("?PasswordSalt", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = passwordSalt
			},
			new MySqlParameter("?PwdFormat", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pwdFormat
			},
		];

		int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				arParams);
		return rowsAffected > 0;
	}

	public static bool UpdatePasswordQuestionAndAnswer(
		Guid userGuid,
		string passwordQuestion,
		string passwordAnswer)
	{
		var sqlCommand = """
			UPDATE mp_Users 
			SET PasswordQuestion = ?PasswordQuestion, 
			PasswordAnswer = ?PasswordAnswer 
			WHERE UserGuid = ?UserGuid;
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
			new MySqlParameter("?PasswordQuestion", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = passwordQuestion
			},
			new MySqlParameter("?PasswordAnswer", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = passwordAnswer
			},
		];
		int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				arParams);
		return rowsAffected > 0;
	}

	public static void UpdateTotalRevenue(Guid userGuid)
	{
		var sqlCommand = """
			UPDATE mp_Users 
			SET TotalRevenue = COALESCE(
				(SELECT SUM(SubTotal) FROM mp_CommerceReport WHERE UserGuid = ?UserGuid)  
				, 0) 
			WHERE UserGuid = ?UserGuid;
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
		];

		MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			arParams);

	}

	public static void UpdateTotalRevenue()
	{
		var sqlCommand = """
			UPDATE mp_Users 
			SET TotalRevenue = COALESCE(
				(SELECT SUM(SubTotal) FROM mp_CommerceReport WHERE UserGuid = mp_Users.UserGuid)  
				, 0) 
			;
		""";
		MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			null);
	}

	public static bool FlagAsDeleted(int userId)
	{
		var sqlCommand = """
			UPDATE mp_Users 
			SET IsDeleted = 1 
			WHERE UserID = ?UserID;
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},
		];

		int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				arParams);
		return rowsAffected > 0;
	}

	public static bool FlagAsNotDeleted(int userId)
	{
		var sqlCommand = """
			UPDATE mp_Users 
			SET IsDeleted = 0 
			WHERE UserID = ?UserID;
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},
		];
		int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				arParams);

		return rowsAffected > 0;
	}

	public static bool IncrementTotalPosts(int userId)
	{
		var sqlCommand = """
			UPDATE mp_Users 
			SET	TotalPosts = TotalPosts + 1
			WHERE UserID = ?UserID;
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},
		];
		int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				arParams);
		return 0 > 0;
	}

	public static bool DecrementTotalPosts(int userId)
	{

		var sqlCommand = """
			UPDATE mp_Users 
			SET	TotalPosts = TotalPosts - 1 
			WHERE UserID = ?UserID;
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},
		];
		int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				arParams);
		return rowsAffected > 0;
	}

	public static IDataReader GetRolesByUser(int siteId, int userId)
	{
		var sqlCommand = """
			SELECT
				r.RoleID,
				r.DisplayName,
				r.RoleName,
				r.RoleGuid,
				r.Description,
				r.SiteID,
				r.SiteGuid
			FROM mp_UserRoles AS ur
			INNER JOIN mp_Users AS u ON ur.UserID = u.UserID
			INNER JOIN mp_Roles AS r ON ur.RoleID = r.RoleID
			WHERE u.SiteID = ?SiteID
			AND u.UserID = ?UserID;
			""";

		var arParams = new MySqlParameter[]
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

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams
		);
	}

	public static IDataReader GetUserByRegistrationGuid(int siteId, Guid registerConfirmGuid)
	{
		var sqlCommand = """
			SELECT * 
			FROM mp_Users 
			WHERE SiteID = ?SiteID 
				AND RegisterConfirmGuid = ?RegisterConfirmGuid; 
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new MySqlParameter("?RegisterConfirmGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = registerConfirmGuid
			},
		];

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);
	}

	public static IDataReader GetSingleUser(int siteId, string email)
	{
		var sqlCommand = """
			SELECT * 
			FROM mp_Users 
			WHERE SiteID = ?SiteID 
				AND LoweredEmail = ?Email; 
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new MySqlParameter("?Email", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = email.ToLower()
			},
		];

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);
	}

	public static IDataReader GetSingleUserByLoginName(int siteId, string loginName, bool allowEmailFallback)
	{
		var sqlCommand = """
			SELECT * 
			FROM mp_Users 
			WHERE SiteID = ?SiteID;
		""";

		if (allowEmailFallback)
		{
			sqlCommand += "AND (LoginName = ?LoginName OR Email = ?LoginName);";
		}
		else
		{
			sqlCommand += "AND LoginName = ?LoginName;";
		}

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new MySqlParameter("?LoginName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = loginName
			},
		];
		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);
	}


	public static IDataReader GetSingleUser(int userId, int siteId)
	{
		var sqlCommand = """
				SELECT *
				FROM mp_Users
				WHERE UserID = ?UserID
					AND SiteID = ?SiteID;
			""";

		MySqlParameter[] arParams =
		[
			new ("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},
			new ("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		];

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams
		);
	}


	public static IDataReader GetSingleUser(Guid userGuid, int siteId)
	{
		var sqlCommand = """
			SELECT *
			FROM mp_Users
			WHERE UserGuid = ?UserGuid
				AND SiteID = ?SiteID;
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		];

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams
		);
	}

	public static Guid GetUserGuidFromOpenId(int siteId, string openIdUri)
	{
		var sqlCommand = """
			SELECT UserGuid 
			FROM mp_Users 
			WHERE 
			SiteID = ?SiteID  
			AND OpenIDURI = ?OpenIDURI;
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new MySqlParameter("?OpenIDURI", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = openIdUri
			},
		];

		Guid userGuid = Guid.Empty;

		using (IDataReader reader = MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams))
		{

			if (reader.Read())
			{
				userGuid = new Guid(reader["UserGuid"].ToString());
			}
		}

		return userGuid;
	}

	public static Guid GetUserGuidFromWindowsLiveId(int siteId, string windowsLiveId)
	{
		var sqlCommand = """
			SELECT UserGuid 
			FROM mp_Users 
			WHERE SiteID = ?SiteID  
				AND WindowsLiveID = ?WindowsLiveID;  
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new MySqlParameter("?WindowsLiveID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = windowsLiveId
			},
		];

		Guid userGuid = Guid.Empty;

		using (IDataReader reader = MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
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
		var sqlCommand = """
			SELECT Name 
			FROM mp_Users 
			WHERE Email = ?Email 
				AND SiteID = ?SiteID 
				AND IsDeleted = 0 
				AND Pwd = ?Password; 
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new MySqlParameter("?Email", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = email
			},
			new MySqlParameter("?Password", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = password
			},
		];

		var userName = string.Empty;

		using (IDataReader reader = MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
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

	public static string Login(int siteId, string loginName, string password)
	{
		var sqlCommand = """
			SELECT Name 
			FROM  mp_Users 
			WHERE LoginName = ?LoginName 
				AND SiteID = ?SiteID 
				AND IsDeleted = 0 
				AND Pwd = ?Password; 
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
			new MySqlParameter("?LoginName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = loginName
			},
			new MySqlParameter("?Password", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = password
			},
		];

		var userName = string.Empty;

		using (IDataReader reader = MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
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
		var sqlCommand = """
			SELECT * 
			FROM mp_UserProperties 
			WHERE UserGuid = ?UserGuid;
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
		];

		var dataTable = new DataTable();
		dataTable.Columns.Add("UserGuid", typeof(string));
		dataTable.Columns.Add("PropertyName", typeof(string));
		dataTable.Columns.Add("PropertyValueString", typeof(string));
		dataTable.Columns.Add("PropertyValueBinary", typeof(object));

		using (IDataReader reader = MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
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

	public static IDataReader GetLazyLoadedProperty(Guid userGuid, string propertyName)
	{
		var sqlCommand = """
			SELECT  * 
			FROM mp_UserProperties 
			WHERE UserGuid = ?UserGuid 
				AND PropertyName = ?PropertyName
			LIMIT 1 ; 
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
			new MySqlParameter("?PropertyName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = propertyName
			},
		];
		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);
	}

	public static bool PropertyExists(Guid userGuid, string propertyName)
	{
		var sqlCommand = """
			SELECT Count(*)
			FROM mp_UserProperties 
			WHERE UserGuid = ?UserGuid 
			AND PropertyName = ?PropertyName; 
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
			new MySqlParameter("?PropertyName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = propertyName
			},
		];
		int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams));

		return count > 0;
	}

	public static void CreateProperty(
		Guid propertyId,
		Guid userGuid,
		string propertyName,
		string propertyValues,
		byte[] propertyValueb,
		DateTime lastUpdatedDate,
		bool isLazyLoaded)
	{
		var sqlCommand = """
			INSERT INTO mp_UserProperties (
				PropertyID, 
				UserGuid, 
				PropertyName, 
				PropertyValueString, 
				PropertyValueBinary, 
				LastUpdatedDate, 
				IsLazyLoaded 
			) VALUES (
				?PropertyID, 
				?UserGuid, 
				?PropertyName, 
				?PropertyValueString, 
				?PropertyValueBinary, 
				?LastUpdatedDate, 
				?IsLazyLoaded 
			);
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?PropertyID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = propertyId.ToString()
			},
			new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
			new MySqlParameter("?PropertyName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = propertyName
			},
			new MySqlParameter("?PropertyValueString", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = propertyValues
			},
			new MySqlParameter("?PropertyValueBinary", MySqlDbType.LongBlob)
			{
				Direction = ParameterDirection.Input,
				Value = propertyValueb
			},
			new MySqlParameter("?LastUpdatedDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastUpdatedDate
			},
			new MySqlParameter("?IsLazyLoaded", MySqlDbType.Bit)
			{
				Direction = ParameterDirection.Input,
				Value = isLazyLoaded
			},
		];

		MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			arParams);
	}

	public static void UpdateProperty(
		Guid userGuid,
		string propertyName,
		string propertyValues,
		byte[] propertyValueb,
		DateTime lastUpdatedDate,
		bool isLazyLoaded)
	{
		var sqlCommand = """
			UPDATE mp_UserProperties 
			SET PropertyValueString = ?PropertyValueString, 
				PropertyValueBinary = ?PropertyValueBinary, 
				LastUpdatedDate = ?LastUpdatedDate, 
				IsLazyLoaded = ?IsLazyLoaded 
			WHERE UserGuid = ?UserGuid 
				AND PropertyName = ?PropertyName;
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
			new MySqlParameter("?PropertyName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = propertyName
			},
			new MySqlParameter("?PropertyValueString", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = propertyValues
			},
			new MySqlParameter("?PropertyValueBinary", MySqlDbType.LongBlob)
			{
				Direction = ParameterDirection.Input,
				Value = propertyValueb
			},
			new MySqlParameter("?LastUpdatedDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastUpdatedDate
			},
			new MySqlParameter("?IsLazyLoaded", MySqlDbType.Bit)
			{
				Direction = ParameterDirection.Input,
				Value = isLazyLoaded
			},
		];
		MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			arParams);
	}

	public static bool DeletePropertiesByUser(Guid userGuid)
	{
		var sqlCommand = new StringBuilder();
		sqlCommand.Append("DELETE FROM mp_UserProperties ");
		sqlCommand.Append("WHERE ");
		sqlCommand.Append("UserGuid = ?UserGuid ");
		sqlCommand.Append(";");

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
		];

		int rowsAffected = MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}
}