using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;


public static class DBUserLogins
{


	public static bool Create(string loginProvider, string providerKey, string userId)
	{

		string sqlCommand = @"
INSERT INTO mp_UserLogins (
	LoginProvider ,
	ProviderKey, 
	UserId 
) 
VALUES (
	?LoginProvider, 
	?ProviderKey, 
	?UserId 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?LoginProvider", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = loginProvider
			},

			new("?ProviderKey", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = providerKey
			},

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

		return rowsAffected > -1;

	}



	public static bool Delete(
		string loginProvider,
		string providerKey,
		string userId)
	{
		string sqlCommand = @"
DELETE FROM mp_UserLogins 
WHERE 
	LoginProvider = ?LoginProvider AND 
	ProviderKey = ?ProviderKey AND 
	UserId = ?UserId ;";

		var arParams = new List<MySqlParameter>
		{
			new("?LoginProvider", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = loginProvider
			},

			new("?ProviderKey", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = providerKey
			},

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

	public static bool DeleteByUser(string userId)
	{
		string sqlCommand = @"
DELETE FROM mp_UserLogins 
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

	public static bool DeleteBySite(Guid siteGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_UserLogins 
WHERE UserId IN (
	SELECT UserGuid 
	FROM mp_Users 
	WHERE SiteGuid = ?SiteGuid
) ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
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

	public static IDataReader Find(string loginProvider, string providerKey)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_UserLogins 
WHERE LoginProvider = ?LoginProvider 
AND ProviderKey = ?ProviderKey ;";

		var arParams = new List<MySqlParameter>
		{
			new("?LoginProvider", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = loginProvider
			},

			new("?ProviderKey", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = providerKey
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetByUser(string userId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_UserLogins 
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

