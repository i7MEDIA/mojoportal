using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;


public static class DBRoles
{

	public static int RoleCreate(
		Guid roleGuid,
		Guid siteGuid,
		int siteId,
		string roleName,
		string displayName,
		string description
	)
	{
		var sqlCommand = @"
INSERT INTO mp_Roles (
        SiteID, 
        RoleName, 
        DisplayName, 
        Description, 
        SiteGuid, 
        RoleGuid 
    )
VALUES (
    ?SiteID,
    ?RoleName,
    ?DisplayName,
    ?Description,
    ?SiteGuid,
    ?RoleGuid 
);
SELECT LAST_INSERT_ID();";

		var sqlParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?RoleName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = roleName
			},
			new("?DisplayName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = displayName
			},
			new("?Description", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},
			new("?RoleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = roleGuid.ToString()
			}
		};

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			sqlParams.ToArray()).ToString());

		return newID;
	}

	public static bool Update(int roleId, string displayName, string description)
	{
		var sqlCommand = @"
UPDATE mp_Roles ( 
    SET DisplayName = ?DisplayName,
    SET Description = ?Description  
WHERE RoleID = ?RoleID);  ";


		var sqlParams = new List<MySqlParameter>
		{
			new("?RoleId", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = roleId
			},
			new("?DisplayName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = displayName
			},
			new("?Description", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			sqlParams.ToArray());

		return rowsAffected > 0;
	}

	public static bool Delete(int roleId)
	{
		string sqlCommand = @"
DELETE FROM mp_Roles 
WHERE RoleID = ?RoleID 
AND RoleName <> 'Admins' 
AND RoleName <> 'Content Administrators' 
AND RoleName <> 'Authenticated Users' 
AND RoleName <> 'Role Admins'  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?RoleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = roleId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}

	public static bool DeleteUserRoles(int userId)
	{
		string sqlCommand = @"
DELETE FROM mp_UserRoles 
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
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}

	public static IDataReader GetById(int roleId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Roles 
WHERE RoleID = ?RoleID ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?RoleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = roleId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetByName(int siteId, string roleName)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Roles 
WHERE SiteID = ?SiteID 
AND RoleName = ?RoleName ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?RoleName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = roleName
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static bool Exists(int siteId, string roleName)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Roles 
WHERE SiteID = ?SiteID 
AND RoleName = ?RoleName ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?RoleName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = roleName
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

		return count > 0;

	}

	public static IDataReader GetSiteRoles(int siteId)
	{
		string sqlCommand = @"
SELECT 
    r.RoleID, 
    r.SiteID, 
    r.RoleName, 
    r.DisplayName, 
    r.Description, 
    r.SiteGuid, 
    r.RoleGuid, 
    COUNT(ur.UserID) As MemberCount 
FROM
    mp_Roles r 
LEFT OUTER JOIN 
    mp_UserRoles ur 
ON 
    ur.RoleID = r.RoleID 
WHERE 
    r.SiteID = ?SiteID  
GROUP BY 
    r.RoleID, 
    r.SiteID, 
    r.RoleName, 
    r.DisplayName, 
    r.SiteGuid, 
    r.RoleGuid 
ORDER BY 
    r.DisplayName 
;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetRoleMembers(int roleId)
	{
		string sqlCommand = @"
SELECT 
    mp_UserRoles.UserID, 
    mp_Users.Name, 
    mp_Users.LoginName, 
    mp_Users.Email 
FROM mp_UserRoles 
INNER JOIN mp_Users 
ON mp_Users.UserID = mp_UserRoles.UserID 
WHERE mp_UserRoles.RoleID = ?RoleID ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?RoleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = roleId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static int GetCountOfUsersNotInRole(int siteId, int roleId)
	{
		string sqlCommand = @"
SELECT 
Count(*)     
FROM mp_Users u      
WHERE u.SiteID = ?SiteID  
AND u.UserID NOT IN (
    SELECT UserID FROM mp_UserRoles 
    WHERE RoleID = ?RoleID 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?RoleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = roleId
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

	}

	public static IDataReader GetUsersNotInRole(
		int siteId,
		int roleId,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCountOfUsersNotInRole(siteId, roleId);

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
    u.UserID, 
    u.Name, 
    u.Email, 
    u.LoginName 
FROM mp_Users u      
WHERE u.SiteID = ?SiteID  
AND u.UserID NOT IN (
    SELECT UserID FROM mp_UserRoles 
    WHERE RoleID = ?RoleID 
)
ORDER BY u.Name  
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?RoleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = roleId
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

	public static int GetCountOfUsersInRole(int siteId, int roleId)
	{
		string sqlCommand = @"
SELECT 
Count(*) 
FROM mp_Users u 
WHERE u.SiteID = ?SiteID  
AND u.UserID IN (
    SELECT UserID FROM mp_UserRoles 
    WHERE RoleID = ?RoleID 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?RoleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = roleId
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

	}

	public static IDataReader GetUsersInRole(
		int siteId,
		int roleId,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCountOfUsersInRole(siteId, roleId);

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
    u.UserID, 
    u.Name, 
    u.Email, 
    u.LoginName 
FROM mp_Users u 
WHERE u.SiteID = ?SiteID  
AND u.UserID IN (
    SELECT UserID FROM mp_UserRoles 
    WHERE RoleID = ?RoleID 
)
ORDER BY u.Name  
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?RoleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = roleId
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

	public static IDataReader GetRolesUserIsNotIn(
		int siteId,
		int userId)
	{
		string sqlCommand = @"
SELECT r.* 
FROM mp_Roles r 
LEFT OUTER JOIN mp_UserRoles ur 
ON r.RoleID = ur.RoleID 
AND ur.UserID = ?UserID 
WHERE r.SiteID = ?SiteID  
AND ur.UserID IS NULL  
ORDER BY r.DisplayName  ;";

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
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static bool AddUser(
		int roleId,
		int userId,
		Guid roleGuid,
		Guid userGuid
		)
	{
		// MS SQL proc checks that no matching record exists, may need to check that
		// here 
		string sqlCommand = @"
INSERT INTO 
    mp_UserRoles (
        UserID, RoleID, RoleGuid, UserGuid
    ) 
VALUES ( 
?UserID , 
?RoleID, 
?RoleGuid, 
?UserGuid
); ";

		var arParams = new List<MySqlParameter>
		{
			new("?RoleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = roleId
			},

			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},

			new("?RoleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = roleGuid.ToString()
			},

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

		return rowsAffected > -1;

	}

	public static bool RemoveUser(int roleId, int userId)
	{
		string sqlCommand = @"
DELETE FROM mp_UserRoles 
WHERE UserID = ?UserID  
AND RoleID = ?RoleID  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?RoleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = roleId
			},

			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static int GetCountOfSiteRoles(int siteId)
	{
		string sqlCommand = @"
SELECT 
Count(*) 
FROM mp_Roles 
WHERE SiteID = ?SiteID  
;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

	}

}
