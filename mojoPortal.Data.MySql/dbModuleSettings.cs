using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;

public static class DBModuleSettings
{

	public static bool DeleteModuleSettings(int moduleId)
	{
		string sqlCommand = @"
DELETE FROM mp_ModuleSettings 
WHERE ModuleID = ?ModuleID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}


	public static IDataReader GetModuleSettings(int moduleId)
	{
		string sqlCommand = @"
SELECT DISTINCT 
    ms.ID, 
    ms.ModuleID, 
    ms.SettingName, 
    ms.SettingValue, 
    mds.ModuleDefID, 
    mds.FeatureGuid, 
    mds.ControlType, 
    mds.RegexValidationExpression, 
    mds.SortOrder, 
    mds.ControlSrc, 
    mds.HelpKey, 
    mds.GroupName, 
    mds.Attributes, 
    mds.Options, 
    mds.ResourceFile 
FROM 
    mp_ModuleSettings ms 
JOIN 
    mp_Modules m 
ON 
    ms.ModuleID = m.ModuleID 
JOIN 
    mp_ModuleDefinitionSettings mds 
ON 
    m.ModuleDefID = mds.ModuleDefID 
AND 
    mds.SettingName = ms.SettingName 
WHERE 
    ms.ModuleID = ?ModuleID  
ORDER BY 
    mds.SortOrder, mds.GroupName;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);
	}


	public static bool CreateModuleSetting(
		Guid settingGuid,
		Guid moduleGuid,
		int moduleId,
		string settingName,
		string settingValue,
		string controlType,
		string regexValidationExpression,
		string controlSrc,
		string helpKey,
		int sortOrder)
	{
		string sqlCommand = @"
INSERT INTO 
    mp_ModuleSettings (
        ModuleID, 
        SettingName, 
        SettingValue, 
        ControlType, 
        ControlSrc, 
        HelpKey, 
        SortOrder, 
        RegexValidationExpression, 
        SettingGuid, 
        ModuleGuid 
    )
VALUES (
    ?ModuleID, 
    ?SettingName, 
    ?SettingValue, 
    ?ControlType, 
    ?ControlSrc, 
    ?HelpKey, 
    ?SortOrder, 
    ?RegexValidationExpression, 
    ?SettingGuid, 
    ?ModuleGuid 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?SettingName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = settingName
			},

			new("?SettingValue", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = settingValue
			},

			new("?ControlType", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = controlType
			},

			new("?RegexValidationExpression", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = regexValidationExpression
			},

			new ("?SettingGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = settingGuid.ToString()
			},

			new ("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			},

			new("?ControlSrc", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = controlSrc
			},

			new("?HelpKey", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = helpKey
			},

			new("?SortOrder", MySqlDbType.Int16)
			{
				Direction = ParameterDirection.Input,
				Value = sortOrder
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool UpdateModuleSetting(
		Guid moduleGuid,
		int moduleId,
		string settingName,
		string settingValue)
	{
		string sqlCommand = @"
SELECT count(*) 
FROM mp_ModuleSettings 
WHERE ModuleID = ?ModuleID  
AND SettingName = ?SettingName;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?SettingName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = settingName
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams).ToString());


		int rowsAffected = 0;

		if (count > 0)
		{
			string sqlCommand1 = @"
UPDATE mp_ModuleSettings 
SET SettingValue = ?SettingValue  
WHERE ModuleID = ?ModuleID  
AND SettingName = ?SettingName";

			var arParams1 = new List<MySqlParameter>
			{
				new("?ModuleID", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				},

				new("?SettingName", MySqlDbType.VarChar, 50)
				{
					Direction = ParameterDirection.Input,
					Value = settingName
				},

				new("?SettingValue", MySqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = settingValue
				}
			};

			rowsAffected = CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWrite(),
				sqlCommand1.ToString(),
				arParams1);

			return rowsAffected > 0;

		}
		else
		{
			//should not reach here


			return false;

			//return CreateModuleSetting(
			//    Guid.NewGuid(),
			//    moduleGuid,
			//    moduleId,
			//    settingName,
			//    settingValue,
			//    "TextBox",
			//    string.Empty);



		}

	}


	public static IDataReader GetDefaultModuleSettings(int moduleDefId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_ModuleDefinitionSettings 
WHERE ModuleDefID = ?ModuleDefID 
ORDER BY SortOrder, GroupName;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleDefID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleDefId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);
	}


	public static DataTable GetDefaultModuleSettingsForModule(int moduleId)
	{
		string sqlCommand = @"
SELECT 
    m.ModuleID,  
    m.Guid AS ModuleGuid,  
    ds.SettingName, 
    ds.SettingValue, 
    ds.ControlType, 
    ds.ControlSrc, 
    ds.HelpKey, 
    ds.SortOrder, 
    ds.GroupName, 
    ds.RegexValidationExpression 
FROM
    mp_Modules m 
JOIN
    mp_ModuleDefinitionSettings ds 
ON 
    ds.ModuleDefID = m.ModuleDefID 
WHERE 
    m.ModuleID = ?ModuleID 
ORDER BY 
    ds.SortOrder, ds.GroupName;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

		return DBPortal.GetTableFromDataReader(reader);

	}


	public static bool CreateDefaultModuleSettings(int moduleId)
	{
		DataTable dataTable = GetDefaultModuleSettingsForModule(moduleId);

		foreach (DataRow row in dataTable.Rows)
		{
			int sortOrder = 100;
			if (row["SortOrder"] != DBNull.Value)
				sortOrder = Convert.ToInt32(row["SortOrder"]);

			CreateModuleSetting(
				Guid.NewGuid(),
				new Guid(row["ModuleGuid"].ToString()),
				moduleId,
				row["SettingName"].ToString(),
				row["SettingValue"].ToString(),
				row["ControlType"].ToString(),
				row["RegexValidationExpression"].ToString(),
				row["ControlSrc"].ToString(),
				row["HelpKey"].ToString(),
				sortOrder);

		}

		return (dataTable.Rows.Count > 0);


	}






}
