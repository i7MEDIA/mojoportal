using System.Collections;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business;

/// <summary>
/// Represents settings for a feature instance
/// </summary>
public sealed class ModuleSettings
{
	public ModuleSettings() { }

	#region Static Methods

	public static bool CreateDefaultModuleSettings(int moduleId) => DBModuleSettings.CreateDefaultModuleSettings(moduleId);

	public static bool DeleteModuleSettings(int moduleId) => DBModuleSettings.DeleteModuleSettings(moduleId);

	public static Hashtable GetModuleSettings(int moduleId)
	{
		var settings = new Hashtable();

		using (IDataReader dr = DBModuleSettings.GetModuleSettings(moduleId))
		{
			while (dr.Read())
			{
				settings[dr["SettingName"].ToString().Trim()] = dr["SettingValue"].ToString();
			}
		}

		return settings;
	}

	public static ArrayList GetDefaultSettings(int moduleDefId)
	{
		var defaultCustomSettings = new ArrayList();
		using (IDataReader reader = DBModuleSettings.GetDefaultModuleSettings(moduleDefId))
		{
			while (reader.Read())
			{
				int sortOrder = 100;
				if (reader["SortOrder"] != DBNull.Value)
					sortOrder = Convert.ToInt32(reader["SortOrder"]);

				var setting = new CustomModuleSetting(
					new Guid(reader["FeatureGuid"].ToString()),
					Convert.ToInt32(reader["ID"]),
					reader["ResourceFile"].ToString(),
					reader["SettingName"].ToString().Trim(),
					reader["SettingValue"].ToString(),
					reader["ControlType"].ToString(),
					reader["RegexValidationExpression"].ToString(),
					reader["ControlSrc"].ToString(),
					reader["HelpKey"].ToString(),
					sortOrder,
					reader["Attributes"].ToString(),
					reader["Options"].ToString()
					);

				setting.GroupName = reader["GroupName"].ToString();
				defaultCustomSettings.Add(setting);
			}
		}

		return defaultCustomSettings;
	}

	public static ArrayList GetCustomSettingValues(int moduleId)
	{
		var customSettings = new ArrayList();
		using (IDataReader reader = DBModuleSettings.GetModuleSettings(moduleId))
		{
			while (reader.Read())
			{
				int sortOrder = 100;
				if (reader["SortOrder"] != DBNull.Value)
				{
					sortOrder = Convert.ToInt32(reader["SortOrder"]);
				}

				var setting = new CustomModuleSetting(
						new Guid(reader["FeatureGuid"].ToString()),
						-1,
						reader["ResourceFile"].ToString(),
						reader["SettingName"].ToString().Trim(),
						reader["SettingValue"].ToString(),
						reader["ControlType"].ToString(),
						reader["RegexValidationExpression"].ToString(),
						reader["ControlSrc"].ToString(),
						reader["HelpKey"].ToString(),
						sortOrder,
						reader["Attributes"].ToString(),
						reader["Options"].ToString()
						);
				customSettings.Add(setting);
			}
		}

		return customSettings;
	}

	public static bool CreateModuleSetting(
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
		return DBModuleSettings.CreateModuleSetting(
			Guid.NewGuid(),
			moduleGuid,
			moduleId,
			settingName,
			settingValue,
			controlType,
			regexValidationExpression,
			controlSrc,
			helpKey,
			sortOrder);
	}

	public static bool UpdateModuleSetting(
		Guid moduleGuid,
		int moduleId,
		string settingName,
		string settingValue)
	{
		return DBModuleSettings.UpdateModuleSetting(moduleGuid, moduleId, settingName, settingValue);
	}
	#endregion
}
