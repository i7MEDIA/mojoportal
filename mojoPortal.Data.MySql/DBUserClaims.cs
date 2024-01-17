using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;


public static class DBUserClaims
{

	public static int Create(
		string userId,
		string claimType,
		string claimValue)
	{

		string sqlCommand = @"
INSERT INTO mp_UserClaims (
    UserId, 
    ClaimType, 
    ClaimValue 
) 
VALUES (
    ?UserId, 
    ?ClaimType, 
    ?ClaimValue 
);
SELECT LAST_INSERT_ID();";

		var arParams = new List<MySqlParameter>
		{
			new("?UserId", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},

			new("?ClaimType", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = claimType
			},

			new("?ClaimValue", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = claimValue
			}
		};

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams).ToString());

		return newID;

	}


	public static bool Delete(int id)
	{
		string sqlCommand = @"
DELETE FROM mp_UserClaims 
WHERE Id = ?Id ;";

		var arParams = new List<MySqlParameter>
		{
			new("?Id", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = id
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool DeleteByUser(string userId)
	{
		string sqlCommand = @"
DELETE FROM mp_UserClaims 
WHERE UserId = ?UserId ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserId", MySqlDbType.VarChar, 128)
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

	public static bool DeleteByUser(string userId, string claimType)
	{
		string sqlCommand = @"
DELETE FROM mp_UserClaims 
WHERE UserId = ?UserId 
AND ClaimType = ?ClaimType ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserId", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},

			new("?ClaimType", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = claimType
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;


	}

	public static bool DeleteByUser(string userId, string claimType, string claimValue)
	{
		string sqlCommand = @"
DELETE FROM mp_UserClaims 
WHERE UserId = ?UserId 
AND ClaimType = ?ClaimType 
AND ClaimValue = ?ClaimValue ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserId", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},

			new("?ClaimType", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = claimType
			},

			new("?ClaimValue", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = claimValue
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool DeleteBySite(Guid siteGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_UserClaims 
WHERE 
UserId IN (
    SELECT UserGuid 
    FROM mp_Users 
    WHERE SiteGuid = ?SiteGuid
) ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static IDataReader GetByUser(string userId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_UserClaims 
WHERE UserId = ?UserId ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserId", MySqlDbType.VarChar, 128)
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






}
