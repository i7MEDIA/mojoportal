using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;

public static class DBUserPage
{

	public static int AddUserPage(
		Guid userPageId,
		Guid siteGuid,
		int siteId,
		Guid userGuid,
		string pageName,
		string pagePath,
		int pageOrder)
	{
		string sqlCommand = @"
INSERT INTO mp_UserPages ( 
    UserPageID, 
    SiteGuid, 
    SiteID, 
    UserGuid, 
    PageName, 
    PagePath, 
    PageOrder 
) 
VALUES (
    ?UserPageID, 
    ?SiteGuid, 
    ?SiteID, 
    ?UserGuid, 
    ?PageName, 
    ?PagePath, 
    ?PageOrder 
);";


		var arParams = new List<MySqlParameter>
		{
			new("?UserPageID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userPageId.ToString()
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

			new("?PageName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pageName
			},

			new("?PagePath", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pagePath
			},

			new("?PageOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageOrder
			},

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

		return rowsAffected;

	}

	public static bool UpdateUserPage(
	Guid userPageId,
	string pageName,
	int pageOrder)
	{
		string sqlCommand = @"
UPDATE mp_UserPages 
SET 
    PageName = ?PageName, 
    PageOrder = ?PageOrder 
WHERE 
    UserPageID = ?UserPageID; ";

		var arParams = new List<MySqlParameter>
		{
			new("?UserPageID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userPageId.ToString()
			},

			new("?PageName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pageName
			},

			new("?PageOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageOrder
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool DeleteUserPage(Guid userPageId)
	{
		string sqlCommand = @"
DELETE FROM mp_UserPages 
WHERE UserPageID = ?UserPageID; ";

		var arParams = new List<MySqlParameter>
		{
			new("?UserPageID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userPageId.ToString()
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
DELETE FROM mp_UserPages 
WHERE UserGuid = ?UserGuid; ";

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

	public static IDataReader GetUserPage(Guid userPageId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_UserPages 
WHERE UserPageID = ?UserPageID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserPageID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userPageId.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader SelectByUser(Guid userGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_UserPages 
WHERE UserGuid = ?UserGuid 
ORDER BY PageOrder ;";

		var arParams = new List<MySqlParameter>
		{
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

	public static int GetNextPageOrder(Guid userGuid)
	{
		string sqlCommand = @"
SELECT COALESCE(MAX(PageOrder),-1)  
FROM mp_UserPages 
WHERE UserGuid = ?UserGuid ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};

		int nextPageOrder = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams)) + 2;

		if (nextPageOrder == 1)
		{
			nextPageOrder = 3;
		}

		return nextPageOrder;

	}

	public static bool UpdatePageOrder(Guid userPageId, int pageOrder)
	{
		string sqlCommand = @"
UPDATE mp_UserPages 
SET PageOrder = ?PageOrder 
WHERE UserPageID = ?UserPageID; ";

		var arParams = new List<MySqlParameter>
		{
			new("?UserPageID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userPageId.ToString()
			},

			new("?PageOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageOrder
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}




}
