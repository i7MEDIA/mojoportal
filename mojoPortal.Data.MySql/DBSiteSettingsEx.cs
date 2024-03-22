using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;

public static class DBSiteSettingsEx
{
	public static IDataReader GetSiteSettingsExList(int siteId)
	{
		string sqlCommand = @"
SELECT e.* 
FROM mp_SiteSettingsEx e 
JOIN mp_SiteSettingsExDef d 
ON e.KeyName = d.KeyName 
AND e.GroupName = d.GroupName 
WHERE e.SiteID = ?SiteID 
ORDER BY d.GroupName, d.SortOrder;";

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

	public static void EnsureSettings()
	{
		string sqlCommand = @"
INSERT INTO 
    mp_SiteSettingsEx ( 
        SiteID, 
        SiteGuid, 
        KeyName, 
        KeyValue, 
        GroupName 
    )
SELECT 
    t.SiteID, 
    t.SiteGuid, 
    t.KeyName, 
    t.DefaultValue, 
    t.GroupName  
FROM ( 
    SELECT 
    s.SiteID, 
    s.SiteGuid, 
    d.KeyName, 
    d.DefaultValue, 
    d.GroupName 
    FROM 
    mp_Sites s, 
    mp_SiteSettingsExDef d 
) t 
LEFT OUTER JOIN 
    mp_SiteSettingsEx e 
ON 
    e.SiteID = t.SiteID 
AND 
    e.KeyName = t.KeyName 
WHERE 
    e.SiteID IS NULL; ";

		CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString());

	}

	public static bool SaveExpandoProperty(
	   int siteId,
	   Guid siteGuid,
	   string groupName,
	   string keyName,
	   string keyValue)
	{
		int count = GetCount(siteId, keyName);
		if (count > 0)
		{
			return Update(siteId, keyName, keyValue);

		}
		else
		{
			return Create(siteId, siteGuid, keyName, keyValue, groupName);

		}

	}

	public static bool UpdateRelatedSitesProperty(
		int siteId,
		string keyName,
		string keyValue)
	{

		string sqlCommand = @"
UPDATE mp_SiteSettingsEx 
SET KeyValue = ?KeyValue 
WHERE SiteID <> ?SiteID AND 
KeyName = ?KeyName;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?KeyName", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = keyName
			},

			new("?KeyValue", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = keyValue
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	private static bool Create(
		int siteId,
		Guid siteGuid,
		string keyName,
		string keyValue,
		string groupName)
	{

		string sqlCommand = @"
INSERT INTO 
    mp_SiteSettingsEx (
        SiteID, 
        SiteGuid, 
        KeyName, 
        KeyValue, 
        GroupName 
    )
VALUES (
    ?SiteId, 
    ?SiteGuid, 
    ?KeyName, 
    ?KeyValue, 
    ?GroupName 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteId", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?KeyName", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = keyName
			},

			new("?KeyValue", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = keyValue
			},

			new("?GroupName", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = groupName
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	private static bool Update(
		int siteID,
		string keyName,
		string keyValue)
	{
		string sqlCommand = @"
UPDATE mp_SiteSettingsEx 
SET KeyValue = ?KeyValue 
WHERE SiteID = ?SiteID AND 
KeyName = ?KeyName;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteID
			},

			new("?KeyName", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = keyName
			},

			new("?KeyValue", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = keyValue
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	private static int GetCount(
		int siteID,
		string keyName)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_SiteSettingsEx 
WHERE SiteID = ?SiteID AND 
KeyName = ?KeyName;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteID
			},

			new("?KeyName", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = keyName
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));
	}

	public static IDataReader GetDefaultExpandoSettings()
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SiteSettingsExDef ;";

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString());
	}


}
