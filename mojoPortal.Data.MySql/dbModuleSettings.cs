using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace mojoPortal.Data;

public static class DBModuleSettings
{
	public static bool DeleteModuleSettings(int moduleId)
	{
		var sqlCommand = """
			DELETE FROM mp_ModuleSettings
			WHERE ModuleID = ?ModuleID;
			""";

		var sqlParams = new MySqlParameter[]
		{
			new("?ModuleID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = moduleId }
		};

		var rowsAffected = MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			sqlParams
		);

		return rowsAffected > 0;
	}


	public static IDataReader GetModuleSettings(int moduleId)
	{
		var sqlCommand = """
			SELECT DISTINCT
				ms.ID,
				ms.ModuleID,
				ms.SettingName,
				ms.SettingValue,
				mds.ModuleDefID,
				mds.FeatureGuid,
				mds.ResourceFile,
				mds.ControlType,
				mds.RegexValidationExpression,
				mds.ControlSrc,
				mds.SortOrder,
				mds.HelpKey,
				mds.GroupName,
				mds.Attributes,
				mds.Options
				mds.Roles
				mds.ShowToUnauthorized
			FROM mp_ModuleSettings ms
			JOIN mp_Modules m ON ms.ModuleID = m.ModuleID
			JOIN mp_ModuleDefinitionSettings mds
				ON m.ModuleDefID = mds.ModuleDefID
				AND mds.SettingName = ms.SettingName
			WHERE ms.ModuleID = ?ModuleID
			ORDER BY mds.SortOrder, mds.GroupName;
			""";

		var sqlParams = new MySqlParameter[]
		{
			new("?ModuleID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = moduleId }
		};

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			sqlParams
		);
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
		int sortOrder
	)
	{
		var sqlCommand = """
			INSERT INTO mp_ModuleSettings (
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
			);
			""";

		var sqlParams = new MySqlParameter[]
		{
			new("?ModuleID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = moduleId },
			new("?SettingName", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = settingName },
			new("?SettingValue", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = settingValue },
			new("?ControlType", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = controlType },
			new("?RegexValidationExpression", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = regexValidationExpression },
			new("?SettingGuid", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = settingGuid.ToString() },
			new("?ModuleGuid", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = moduleGuid.ToString() },
			new("?ControlSrc", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = controlSrc },
			new("?HelpKey", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = helpKey },
			new("?SortOrder", MySqlDbType.Int16) { Direction = ParameterDirection.Input, Value = sortOrder },
		};

		var rowsAffected = MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			sqlParams
		);

		return rowsAffected > 0;
	}


	public static bool UpdateModuleSetting(
		Guid moduleGuid,
		int moduleId,
		string settingName,
		string settingValue
	)
	{
		var sqlCommand = """
			SELECT COUNT(*)
			FROM mp_ModuleSettings
			WHERE ModuleID = ?ModuleID
			AND SettingName = ?SettingName;
			""";

		var sqlParams = new MySqlParameter[]
		{
			new("?ModuleID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = moduleId },
			new("?SettingName", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = settingName }
		};

		var count = Convert.ToInt32(
			MySqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				sqlCommand,
				sqlParams
			).ToString()
		);

		if (count <= 0)
		{
			//should not reach here
			return false;
		}

		sqlCommand = """
			UPDATE mp_ModuleSettings
			SET SettingValue = ?SettingValue
			WHERE ModuleID = ?ModuleID
			AND SettingName = ?SettingName
			""";

		sqlParams = [
			new("?ModuleID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = moduleId },
			new("?SettingName", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = settingName },
			new("?SettingValue", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = settingValue }
		];

		var rowsAffected = MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			sqlParams
		);

		return rowsAffected > 0;
	}


	public static IDataReader GetDefaultModuleSettings(int moduleDefId)
	{
		var sqlCommand = """
			SELECT *
			FROM mp_ModuleDefinitionSettings
			WHERE ModuleDefID = ?ModuleDefID
			ORDER BY SortOrder, GroupName;
			""";

		var sqlParams = new MySqlParameter[]
		{
			new("?ModuleDefID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = moduleDefId }
		};

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			sqlParams
		);
	}


	public static DataTable GetDefaultModuleSettingsForModule(int moduleId)
	{
		var sqlCommand = """
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
			FROM mp_Modules m
			JOIN mp_ModuleDefinitionSettings ds ON ds.ModuleDefID = m.ModuleDefID
			WHERE m.ModuleID = ?ModuleID
			ORDER BY ds.SortOrder, ds.GroupName;
			""";

		var sqlParams = new MySqlParameter[]
		{
			new("?ModuleID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = moduleId }
		};

		var reader = MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			sqlParams
		);

		return DBPortal.GetTableFromDataReader(reader);
	}


	public static bool CreateDefaultModuleSettings(int moduleId)
	{
		var dataTable = GetDefaultModuleSettingsForModule(moduleId);

		foreach (DataRow row in dataTable.Rows)
		{
			var sortOrder = 100;

			if (row["SortOrder"] != DBNull.Value)
			{
				sortOrder = Convert.ToInt32(row["SortOrder"]);
			}

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
				sortOrder
			);
		}

		return dataTable.Rows.Count > 0;
	}
}
