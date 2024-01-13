using System.Collections.Generic;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business;

/// <summary>
/// Represents a feature that can plug into the content management system.
/// </summary>
public class ModuleDefinition
{
	public ModuleDefinition() { }

	public ModuleDefinition(int moduleDefId)
	{
		GetModuleDefinition(moduleDefId);
	}

	public ModuleDefinition(Guid featureGuid)
	{
		providedFeatureGuid = featureGuid;
		GetModuleDefinition(featureGuid);
	}

	private Guid providedFeatureGuid = Guid.Empty;

	public int ModuleDefId { get; set; } = -1;
	public Guid FeatureGuid { get; set; } = Guid.Empty;
	public int SiteId { get; set; } = -1;
	public string ResourceFile { get; set; } = "Resource";
	public string FeatureName { get; set; } = string.Empty;
	public bool IsCacheable { get; set; } = false;
	public bool IsSearchable { get; set; } = false;
	public string SearchListName { get; set; } = string.Empty;
	public bool SupportsPageReuse { get; set; } = true;
	public string DeleteProvider { get; set; } = string.Empty;
	public string ControlSrc { get; set; } = string.Empty;
	public int SortOrder { get; set; } = 500;
	public int DefaultCacheTime { get; set; } = 0;
	public bool IsAdmin { get; set; } = false;
	public string Icon { get; set; } = string.Empty;
	public string PartialView { get; set; } = string.Empty;
	public string SkinFileName { get; set; } = string.Empty;

	#region Private Methods

	private void GetModuleDefinition(Guid featureGuid)
	{
		using IDataReader reader = DBModuleDefinition.GetModuleDefinition(featureGuid);
		GetModuleDefinition(reader);
	}

	private void GetModuleDefinition(int moduleDefId)
	{
		using IDataReader reader = DBModuleDefinition.GetModuleDefinition(moduleDefId);
		GetModuleDefinition(reader);
	}

	private void GetModuleDefinition(IDataReader reader)
	{
		if (reader.Read())
		{
			ModuleDefId = Convert.ToInt32(reader["ModuleDefID"]);
			FeatureName = reader["FeatureName"].ToString();
			ControlSrc = reader["ControlSrc"].ToString();
			SortOrder = Convert.ToInt32(reader["SortOrder"]);
			DefaultCacheTime = Convert.ToInt32(reader["DefaultCacheTime"]);
			IsAdmin = Convert.ToBoolean(reader["IsAdmin"]);
			Icon = reader["Icon"].ToString();
			FeatureGuid = new Guid(reader["Guid"].ToString());
			ResourceFile = reader["ResourceFile"].ToString();
			SearchListName = reader["SearchListName"].ToString();
			if (reader["IsCacheable"] != DBNull.Value)
			{
				IsCacheable = Convert.ToBoolean(reader["IsCacheable"]);
			}

			if (reader["IsSearchable"] != DBNull.Value)
			{
				IsSearchable = Convert.ToBoolean(reader["IsSearchable"]);
			}

			if (reader["SupportsPageReuse"] != DBNull.Value)
			{
				SupportsPageReuse = Convert.ToBoolean(reader["SupportsPageReuse"]);
			}

			DeleteProvider = reader["DeleteProvider"].ToString();
			PartialView = reader["PartialView"].ToString();
			SkinFileName = reader["SkinFileName"].ToString();
		}
	}

	private bool Create()
	{
		int newID = -1;

		if (FeatureGuid == Guid.Empty)
		{
			if (providedFeatureGuid != Guid.Empty)
			{
				FeatureGuid = providedFeatureGuid;
			}
			else
			{
				FeatureGuid = Guid.NewGuid();
			}
		}

		newID = DBModuleDefinition.AddModuleDefinition(
			FeatureGuid,
			SiteId,
			FeatureName,
			ControlSrc,
			SortOrder,
			DefaultCacheTime,
			Icon,
			IsAdmin,
			ResourceFile,
			IsCacheable,
			IsSearchable,
			SearchListName,
			SupportsPageReuse,
			DeleteProvider,
			PartialView,
			SkinFileName);

		ModuleDefId = newID;

		return newID > -1;
	}

	private bool Update()
	{
		return DBModuleDefinition.UpdateModuleDefinition(
			ModuleDefId,
			FeatureName,
			ControlSrc,
			SortOrder,
			DefaultCacheTime,
			Icon,
			IsAdmin,
			ResourceFile,
			IsCacheable,
			IsSearchable,
			SearchListName,
			SupportsPageReuse,
			DeleteProvider,
			PartialView,
			SkinFileName);
	}
	#endregion

	#region Public Methods

	public bool Save()
	{
		if (ModuleDefId > -1)
		{
			return Update();
		}
		else
		{
			return Create();
		}
	}

	#endregion

	#region Static Methods

	public static bool DeleteModuleDefinition(int moduleDefId)
	{
		DBModuleDefinition.DeleteModuleDefinitionFromSites(moduleDefId);
		return DBModuleDefinition.DeleteModuleDefinition(moduleDefId);
	}

	public static bool DeleteSettingsByFeature(int moduleDefId) => DBModuleDefinition.DeleteSettingsByFeature(moduleDefId);

	public static IDataReader GetModuleDefinitions(Guid siteGuid) => DBModuleDefinition.GetModuleDefinitions(siteGuid);

	public static DataTable GetModuleDefinitionsBySite(Guid siteGuid) => DBModuleDefinition.GetModuleDefinitionsBySite(siteGuid);

	public static SiteModuleDefinition GetSiteFeature(Guid siteGuid, int moduleDefId)
	{
		DataTable features = GetModuleDefinitionsBySite(siteGuid);

		foreach (DataRow row in features.Rows)
		{
			int id = Convert.ToInt32(row["ModuleDefID"]);
			if (id == moduleDefId)
			{
				var feature = new SiteModuleDefinition
				{
					ModueDefId = id,
					FeatureGuid = new Guid(row["FeatureGuid"].ToString()),
					FeatureName = row["FeatureName"].ToString(),
					AuthorizedRoles = row["AuthorizedRoles"].ToString()
				};
				return feature;
			}
		}

		return null;
	}

	public static ModuleDefinition GetModuleDefinitionBySkinFileName(string skinFileName)
	{
		ModuleDefinition md = new ModuleDefinition();
		md.GetModuleDefinition(DBModuleDefinition.GetModuleDefinitionBySkinFileName(skinFileName));
		return md;
	}

	public static List<string> GetAllModuleSkinFileNames()
	{
		List<string> list = new List<string>();
		using (IDataReader reader = DBModuleDefinition.GetAllModuleSkinFileNames())
		{
			while (reader.Read())
			{
				list.Add(reader["SkinFileName"].ToString());
			}
		}

		return list;
	}

	public static IDataReader GetUserModules(int siteId) => DBModuleDefinition.GetUserModules(siteId);

	public static IDataReader GetSearchableModules(int siteId) => DBModuleDefinition.GetSearchableModules(siteId);

	public static bool UpdateModuleDefinitionSetting(
		Guid featureGuid,
		int moduleDefId,
		string resourceFile,
		string groupName,
		string settingName,
		string settingValue,
		string controlType,
		string regexValidationExpression,
		string controlSrc,
		string helpKey,
		int sortOrder,
		string attributes,
		string options)
	{
		return DBModuleDefinition.UpdateModuleDefinitionSetting(
			featureGuid,
			moduleDefId,
			resourceFile,
			groupName,
			settingName,
			settingValue,
			controlType,
			regexValidationExpression,
			controlSrc,
			helpKey,
			sortOrder,
			attributes,
			options);
	}

	public static bool UpdateModuleDefinitionSettingById(
		int id,
		int moduleDefId,
		string resourceFile,
		string groupName,
		string settingName,
		string settingValue,
		string controlType,
		string regexValidationExpression,
		string controlSrc,
		string helpKey,
		int sortOrder,
		string attributes,
		string options)
	{
		return DBModuleDefinition.UpdateModuleDefinitionSettingById(
			id,
			moduleDefId,
			resourceFile,
			groupName,
			settingName,
			settingValue,
			controlType,
			regexValidationExpression,
			controlSrc,
			helpKey,
			sortOrder,
			attributes,
			options);
	}

	public static bool DeleteSettingById(int id) => DBModuleDefinition.DeleteSettingById(id);

	public static bool SettingExists(Guid featureGuid, string settingName)
	{
		bool result = false;

		using (IDataReader reader = DBModuleDefinition.ModuleDefinitionSettingsGetSetting(featureGuid, settingName))
		{
			if (reader.Read())
			{
				result = true;
			}
		}

		return result;
	}

	public static void EnsureInstallationInAdminSites()
	{
		DBModuleDefinition.EnsureInstallationInAdminSites();
	}

	public static bool UpdateSiteModulePermissions(int siteId, int moduleDefId, string authorizedRoles)
	{
		return DBModuleDefinition.UpdateSiteModulePermissions(siteId, moduleDefId, authorizedRoles);
	}

	#endregion
}
