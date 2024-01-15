using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace mojoPortal.Data;

public static class DBModuleDefinition
{


	public static int AddModuleDefinition(
		Guid featureGuid,
		int siteId,
		string featureName,
		string controlSrc,
		int sortOrder,
		int defaultCacheTime,
		String icon,
		bool isAdmin,
		string resourceFile,
		bool isCacheable,
		bool isSearchable,
		string searchListName,
		bool supportsPageReuse,
		string deleteProvider,
		string partialView,
		string skinFileName)
	{
		int intIsAdmin = 0;
		if (isAdmin) { intIsAdmin = 1; }

		int intIsCacheable = 0;
		if (isCacheable) { intIsCacheable = 1; }

		int intIsSearchable = 0;
		if (isSearchable) { intIsSearchable = 1; }

		int intSupportsPageReuse = 0;
		if (supportsPageReuse) { intSupportsPageReuse = 1; }


		string sqlCommand = @"
INSERT INTO 
	mp_ModuleDefinitions (
		Guid, 
		FeatureName, 
		ControlSrc, 
		SortOrder, 
		DefaultCacheTime, 
		Icon, 
		IsAdmin, 
		IsCacheable, 
		IsSearchable, 
		SearchListName, 
		SupportsPageReuse, 
		DeleteProvider, 
		PartialView, 
		ResourceFile, 
		SkinFileName 
	)
VALUES (
	?FeatureGuid, 
	?FeatureName, 
	?ControlSrc, 
	?SortOrder, 
	?DefaultCacheTime, 
	?Icon, 
	?IsAdmin, 
	?IsCacheable, 
	?IsSearchable, 
	?SearchListName, 
	?SupportsPageReuse, 
	?DeleteProvider, 
	?PartialView, 
	?ResourceFile, 
	?SkinFileName 
);
SELECT LAST_INSERT_ID();";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?FeatureName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = featureName
			},

			new("?ControlSrc", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = controlSrc
			},

			new("?SortOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sortOrder
			},

			new("?IsAdmin", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIsAdmin
			},

			new("?Icon", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = icon
			},

			new("?DefaultCacheTime", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = defaultCacheTime
			},

			new("?FeatureGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = featureGuid
			},

			new ("?ResourceFile", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = resourceFile
			},

			new ("?IsCacheable", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIsCacheable
			},

			new("?IsSearchable", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIsSearchable
			},

			new("?SearchListName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = searchListName
			},

			new("?SupportsPageReuse", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intSupportsPageReuse
			},

			new("?DeleteProvider", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = deleteProvider
			},

			new("?PartialView", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = partialView
			},

			new("?SkinFileName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = skinFileName
			}
		};



		int newID = -1;

		newID = Convert.ToInt32(
			CommandHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams).ToString());

		if (siteId > -1)
		{
			string sqlCommand1 = @"
INSERT INTO 
	mp_SiteModuleDefinitions (
		SiteID, 
		SiteGuid, 
		FeatureGuid, 
		AuthorizedRoles, 
		ModuleDefID 
) 
VALUES (
	?SiteID, (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID LIMIT 1), 
		(SELECT Guid FROM mp_ModuleDefinitions WHERE ModuleDefID = ?ModuleDefID LIMIT 1), 
	'All Users', 
	?ModuleDefID 
);";

			var arParams1 = new List<MySqlParameter>
			{
				new("?SiteID", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = siteId
				},

				new("?ModuleDefID", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = newID
				}
			};

			CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand1.ToString(),
				arParams1);
		}

		return newID;

	}


	public static bool UpdateModuleDefinition(
		int moduleDefId,
		string featureName,
		string controlSrc,
		int sortOrder,
		int defaultCacheTime,
		String icon,
		bool isAdmin,
		string resourceFile,
		bool isCacheable,
		bool isSearchable,
		string searchListName,
		bool supportsPageReuse,
		string deleteProvider,
		string partialView,
		string skinFileName)
	{
		int intIsAdmin = 0;
		if (isAdmin) { intIsAdmin = 1; }

		int intIsCacheable = 0;
		if (isCacheable) { intIsCacheable = 1; }

		int intIsSearchable = 0;
		if (isSearchable) { intIsSearchable = 1; }

		int intSupportsPageReuse = 0;
		if (supportsPageReuse) { intSupportsPageReuse = 1; }

		string sqlCommand = @"
UPDATE 
	mp_ModuleDefinitions 
SET  
	FeatureName = ?FeatureName, 
	ControlSrc = ?ControlSrc, 
	SortOrder = ?SortOrder, 
	DefaultCacheTime = ?DefaultCacheTime, 
	Icon = ?Icon, 
	IsAdmin = ?IsAdmin, 
	IsCacheable = ?IsCacheable, 
	IsSearchable = ?IsSearchable, 
	SearchListName = ?SearchListName, 
	SupportsPageReuse = ?SupportsPageReuse, 
	DeleteProvider = ?DeleteProvider, 
	PartialView = ?PartialView, 
	ResourceFile = ?ResourceFile, 
	SkinFileName = ?SkinFileName 
WHERE 
	ModuleDefID = ?ModuleDefID
;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleDefID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleDefId
			},

			new("?FeatureName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = featureName
			},

			new("?ControlSrc", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = controlSrc
			},

			new("?SortOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sortOrder
			},

			new("?IsAdmin", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIsAdmin
			},

			new("?Icon", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = icon
			},

			new("?DefaultCacheTime", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = defaultCacheTime
			},

			new("?ResourceFile", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = resourceFile
			},

			new("?IsCacheable", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIsCacheable
			},

			new("?IsSearchable", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIsSearchable
			},

			new("?SearchListName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = searchListName
			},

			new("?SupportsPageReuse", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intSupportsPageReuse
			},

			new("?DeleteProvider", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = deleteProvider
			},

			new("?PartialView", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = partialView
			},

			new("?SkinFileName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = skinFileName
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static bool UpdateSiteModulePermissions(int siteId, int moduleDefId, string authorizedRoles)
	{
		string sqlCommand = @"
UPDATE mp_SiteModuleDefinitions 
SET AuthorizedRoles = ?AuthorizedRoles 
WHERE SiteID = ?SiteID AND 
ModuleDefID = ?ModuleDefID;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?ModuleDefID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleDefId
			},

			new("?AuthorizedRoles", MySqlDbType.Text) {

				Direction = ParameterDirection.Input,
				Value = authorizedRoles
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}


	public static bool DeleteModuleDefinition(int moduleDefId)
	{
		string sqlCommand = @"
DELETE FROM mp_ModuleDefinitions 
WHERE ModuleDefID = ?ModuleDefID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleDefID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleDefId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool DeleteModuleDefinitionFromSites(int moduleDefId)
	{
		string sqlCommand = @"
DELETE FROM mp_SiteModuleDefinitions 
WHERE ModuleDefID = ?ModuleDefID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleDefID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleDefId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}


	public static IDataReader GetModuleDefinition(int moduleDefId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_ModuleDefinitions 
WHERE ModuleDefID = ?ModuleDefID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleDefID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleDefId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetModuleDefinition(
		Guid featureGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_ModuleDefinitions 
WHERE Guid = ?FeatureGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?FeatureGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = featureGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static int GetModuleDefinitionIDFromGuid(Guid featureGuid)
	{
		string sqlCommand = @"
SELECT ModuleDefID 
FROM mp_ModuleDefinitions 
WHERE Guid = ?FeatureGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?FeatureGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = featureGuid.ToString()
			}
		};

		int moduleDefId = -1;

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams))
		{
			if (reader.Read())
			{
				moduleDefId = Convert.ToInt32(reader["ModuleDefID"].ToString(), CultureInfo.InvariantCulture);
			}

		}

		return moduleDefId;

	}

	public static void EnsureInstallationInAdminSites()
	{
		string sqlCommand = @"
INSERT INTO 
	mp_SiteModuleDefinitions (
		SiteID, 
		SiteGuid, 
		FeatureGuid, 
		ModuleDefID, 
		AuthorizedRoles 
	) 
SELECT 
	s.SiteID, 
	s.SiteGuid, 
	md.Guid, 
	md.ModuleDefID, 
	'All Users' 
FROM 
	mp_Sites s, 
	mp_ModuleDefinitions md 
WHERE 
	s.IsServerAdminSite = 1 
AND 
	md.ModuleDefID NOT IN ( 
		SELECT sd.ModuleDefID 
		FROM mp_SiteModuleDefinitions sd 
		WHERE sd.SiteID = s.SiteID 
	);";

		CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString());

	}


	public static IDataReader GetModuleDefinitions(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT 
	md.*, 
	smd.AuthorizedRoles 
FROM 
	mp_ModuleDefinitions md 
JOIN 
	mp_SiteModuleDefinitions smd  
ON 
	md.ModuleDefID = smd.ModuleDefID  
WHERE 
	smd.SiteGuid = ?SiteGuid 
ORDER BY 
	md.SortOrder, md.FeatureName ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static DataTable GetModuleDefinitionsBySite(Guid siteGuid)
	{
		//string sqlCommand = @"
		//sqlCommand.Append("SELECT md.* ");
		//sqlCommand.Append("FROM	mp_ModuleDefinitions md ");

		//sqlCommand.Append("JOIN	mp_SiteModuleDefinitions smd  ");
		//sqlCommand.Append("ON md.ModuleDefID = smd.ModuleDefID  ");

		//sqlCommand.Append("WHERE smd.SiteGuid = ?SiteGuid ");
		//sqlCommand.Append("ORDER BY md.SortOrder, md.FeatureName ;");

		//var arParams = new List<MySqlParameter> 

		//new("?SiteGuid", MySqlDbType.VarChar, 36);
		//Direction = ParameterDirection.Input,
		//Value = siteGuid.ToString();

		DataTable dt = new();
		dt.Columns.Add("ModuleDefID", typeof(int));
		dt.Columns.Add("FeatureGuid", typeof(String));
		dt.Columns.Add("FeatureName", typeof(String));
		dt.Columns.Add("ControlSrc", typeof(String));
		dt.Columns.Add("AuthorizedRoles", typeof(String));

		using (IDataReader reader = GetModuleDefinitions(siteGuid))
		{

			while (reader.Read())
			{
				DataRow row = dt.NewRow();
				row["ModuleDefID"] = reader["ModuleDefID"];
				row["FeatureGuid"] = reader["Guid"].ToString();
				row["FeatureName"] = reader["FeatureName"];
				row["ControlSrc"] = reader["ControlSrc"];
				row["AuthorizedRoles"] = reader["AuthorizedRoles"];
				dt.Rows.Add(row);

			}

		}

		return dt;

	}

	public static IDataReader GetModuleDefinitionBySkinFileName(string skinFileName)
	{
		string sqlCommand = @"
SELECT * FROM mp_ModuleDefinitions WHERE SkinFileName = ?SkinFileName LIMIT 1;";

		var arParams = new List<MySqlParameter>
		{
			new("?SkinFileName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = skinFileName
			}
		};

		return CommandHelper.ExecuteReader(
			 ConnectionString.GetReadConnectionString(),
			 sqlCommand.ToString(),
			 arParams);
	}

	public static IDataReader GetAllModuleSkinFileNames()
	{
		string sqlCommand = @"
SELECT SkinFileName FROM mp_ModuleDefinitions;";

		return CommandHelper.ExecuteReader(
			 ConnectionString.GetReadConnectionString(),
			 sqlCommand.ToString());
	}


	public static IDataReader GetUserModules(int siteId)
	{
		var commandText = @"
SELECT 
	md.*, 
	smd.FeatureGuid, 
	smd.AuthorizedRoles
FROM 
	mp_ModuleDefinitions md
JOIN 
	mp_SiteModuleDefinitions smd
ON 
	smd.ModuleDefID = md.ModuleDefID
WHERE 
	smd.SiteID = ?SiteID
AND 
	md.IsAdmin = 0
ORDER BY 
	md.SortOrder,
	md.FeatureName";

		var commandParameters = new MySqlParameter[]
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			commandText,
			commandParameters
		);
	}


	public static IDataReader GetSearchableModules(int siteId)
	{
		string sqlCommand = @"
SELECT md.* 
FROM mp_ModuleDefinitions md 
JOIN mp_SiteModuleDefinitions smd  
ON md.ModuleDefID = smd.ModuleDefID  
WHERE smd.SiteID = ?SiteID AND md.IsAdmin = 0 AND md.IsSearchable = 1 
ORDER BY md.SortOrder, md.SearchListName ;";

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
		string sqlCommand = @"
SELECT count(*) 
FROM mp_ModuleDefinitionSettings 
WHERE (ModuleDefID = ?ModuleDefID OR FeatureGuid = ?FeatureGuid)  
AND SettingName = ?SettingName;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleDefID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleDefId
			},

			new("?SettingName", MySqlDbType.VarChar, 50) {

				Direction = ParameterDirection.Input,
				Value = settingName
			},

			new("?FeatureGuid", MySqlDbType.VarChar, 36) {

				Direction = ParameterDirection.Input,
				Value = featureGuid
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams).ToString());



		int rowsAffected = 0;

		if (count > 0)
		{
			string sqlCommand1 = @"
UPDATE 
	mp_ModuleDefinitionSettings 
SET 
	SettingValue = ?SettingValue,  
	FeatureGuid = ?FeatureGuid,  
	ResourceFile = ?ResourceFile,  
	ControlType = ?ControlType,  
	ControlSrc = ?ControlSrc,  
	HelpKey = ?HelpKey,  
	SortOrder = ?SortOrder,  
	GroupName = ?GroupName,  
	RegexValidationExpression = ?RegexValidationExpression,  
	Attributes = ?Attributes,  
	Options = ?Options  
WHERE (
		ModuleDefID = ?ModuleDefID OR FeatureGuid = ?FeatureGuid
	)  
AND 
	SettingName = ?SettingName;";

			var arParams1 = new List<MySqlParameter>
			{
				new("?ModuleDefID", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleDefId
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

				new("?FeatureGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = featureGuid
				},

				new("?ResourceFile", MySqlDbType.VarChar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = resourceFile
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

				new("?SortOrder", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = sortOrder
				},

				new("?GroupName", MySqlDbType.VarChar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = groupName
				},

				new("?Attributes", MySqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = attributes
				},

				new("?Options", MySqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = options
				}
			};

			rowsAffected = CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand1.ToString(),
				arParams1);

			return rowsAffected > 0;

		}
		else
		{
			string sqlCommand1 = @"
INSERT INTO 
	mp_ModuleDefinitionSettings 
		( 
			FeatureGuid, 
			ModuleDefID, 
			ResourceFile, 
			SettingName, 
			SettingValue, 
			ControlType, 
			ControlSrc, 
			HelpKey, 
			SortOrder, 
			GroupName, 
			RegexValidationExpression, 
			Attributes, 
			Options 
		)
VALUES (  
	?FeatureGuid , 
	?ModuleDefID , 
	?ResourceFile  , 
	?SettingName  , 
	?SettingValue  ,
	?ControlType  ,
	?ControlSrc, 
	?HelpKey, 
	?SortOrder, 
	?GroupName, 
	?RegexValidationExpression,  
	?Attributes,  
	?Options  
);";

			var arParams1 = new List<MySqlParameter>
			{
				new("?ModuleDefID", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleDefId
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

				new("?FeatureGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = featureGuid
				},

				new("?ResourceFile", MySqlDbType.VarChar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = resourceFile
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

				new("?SortOrder", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = sortOrder
				},

				new("?GroupName", MySqlDbType.VarChar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = groupName
				},

				new("?Attributes", MySqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = attributes
				},

				new("?Options", MySqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = options
				}
			};

			rowsAffected = CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand1.ToString(),
				arParams1);

			return rowsAffected > 0;

		}

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
		string sqlCommand = @"
UPDATE 
	mp_ModuleDefinitionSettings 
SET 
	SettingName = ?SettingName,  
	ResourceFile = ?ResourceFile,  
	SettingValue = ?SettingValue,  
	ControlType = ?ControlType,  
	ControlSrc = ?ControlSrc,  
	HelpKey = ?HelpKey,  
	SortOrder = ?SortOrder,  
	GroupName = ?GroupName,  
	RegexValidationExpression = ?RegexValidationExpression,  
	Attributes = ?Attributes,  
	Options = ?Options  
WHERE 
	ID = ?ID  
AND 
	ModuleDefID = ?ModuleDefID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = id
			},

			new("?ModuleDefID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleDefId
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

			new("?ResourceFile", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = resourceFile
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

			new("?SortOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sortOrder
			},

			new("?GroupName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = groupName
			},

			new("?Attributes", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = attributes
			},

			new("?Options", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = options
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool DeleteSettingById(int id)
	{
		string sqlCommand = @"
DELETE FROM mp_ModuleDefinitionSettings 
WHERE ID = ?ID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ID", MySqlDbType.Int32)
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

	public static bool DeleteSettingsByFeature(int moduleDefId)
	{
		string sqlCommand = @"
DELETE FROM mp_ModuleDefinitionSettings 
WHERE 
ModuleDefID = ?ModuleDefID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleDefID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleDefId
			}
		};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}


	public static IDataReader ModuleDefinitionSettingsGetSetting(
		Guid featureGuid,
		string settingName)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_ModuleDefinitionSettings 
WHERE FeatureGuid = ?FeatureGuid  
AND SettingName = ?SettingName;";

		var arParams = new List<MySqlParameter>
		{
			new("?FeatureGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = featureGuid.ToString()
			},

			new("?SettingName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = settingName
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}




}
