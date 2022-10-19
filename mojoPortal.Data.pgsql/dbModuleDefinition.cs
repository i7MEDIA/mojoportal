using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace mojoPortal.Data
{
	public static class DBModuleDefinition
	{
		/// <summary>
		/// Inserts a row in the mp_ModuleDefinitions table. Returns new integer id.
		/// </summary>
		/// <param name="featureName"> featureName </param>
		/// <param name="controlSrc"> controlSrc </param>
		/// <param name="sortOrder"> sortOrder </param>
		/// <param name="isAdmin"> isAdmin </param>
		/// <param name="icon"> icon </param>
		/// <param name="defaultCacheTime"> defaultCacheTime </param>
		/// <param name="guid"> guid </param>
		/// <param name="resourceFile"> resourceFile </param>
		/// <param name="isCacheable"> isCacheable </param>
		/// <param name="isSearchable"> isSearchable </param>
		/// <param name="searchListName"> searchListName </param>
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
			string skinFileName
		)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.AppendFormat("INSERT INTO mp_moduledefinitions ({0}) VALUES ({1});"
				, @"featurename
				  ,controlsrc
				  ,sortorder
				  ,isadmin
				  ,icon
				  ,defaultcachetime
				  ,guid
				  ,resourcefile
				  ,supportspagereuse
				  ,deleteprovider
				  ,iscacheable
				  ,issearchable
				  ,partialview
				  ,skinfilename
				  ,searchlistname"
				, @":featurename
				  ,:controlsrc
				  ,:sortorder
				  ,:isadmin
				  ,:icon
				  ,:defaultcachetime
				  ,:guid
				  ,:resourcefile
				  ,:supportspagereuse
				  ,:deleteprovider
				  ,:iscacheable
				  ,:issearchable
				  ,:partialview
				  ,:skinfilename
				  ,:searchlistname");

			sqlCommand.Append(" SELECT CURRVAL('mp_moduledefinitions_moduledefid_seq');");

			var sqlParams = new List<NpgsqlParameter>()
			{
				new NpgsqlParameter(":featurename", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = featureName
				},
				new NpgsqlParameter(":controlsrc", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = controlSrc
				},
				new NpgsqlParameter(":sortorder", NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = sortOrder
				},
				new NpgsqlParameter(":isadmin", NpgsqlDbType.Boolean)
				{
					Direction = ParameterDirection.Input,
					Value = isAdmin
				},
				new NpgsqlParameter(":icon", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = icon
				},
				new NpgsqlParameter(":defaultcachetime", NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = defaultCacheTime
				},
				new NpgsqlParameter(":guid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = featureGuid.ToString()
				},
				new NpgsqlParameter(":resourcefile", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = resourceFile
				},
				new NpgsqlParameter(":iscacheable", NpgsqlDbType.Boolean)
				{
					Direction = ParameterDirection.Input,
					Value = isCacheable
				},
				new NpgsqlParameter(":issearchable", NpgsqlDbType.Boolean)
				{
					Direction = ParameterDirection.Input,
					Value = isSearchable
				},
				new NpgsqlParameter(":searchlistname", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = searchListName
				},
				new NpgsqlParameter(":supportspagereuse", NpgsqlDbType.Boolean)
				{
					Direction = ParameterDirection.Input,
					Value = supportsPageReuse
				},
				new NpgsqlParameter(":deleteprovider", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = deleteProvider
				},
				new NpgsqlParameter(":partialview", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = partialView
				},
				new NpgsqlParameter(":skinfilename", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = skinFileName
				}
			};

			int newID = Convert.ToInt32(
				NpgsqlHelper.ExecuteScalar(
					ConnectionString.GetWriteConnectionString(),
					CommandType.Text,
					sqlCommand.ToString(),
					sqlParams.ToArray()
				)
			);

			if (siteId > -1)
			{
				// now add to mp_SiteModuleDefinitions
				sqlCommand = new StringBuilder();
				sqlCommand.AppendFormat("INSERT INTO mp_sitemoduledefinitions ({0}) VALUES ({1});"
					, @"siteid
					 ,siteguid
					 ,featureguid
					 ,moduledefid
					 ,authorizedroles"
					, @":siteid
					 ,(SELECT siteguid FROM mp_sites WHERE siteid = :siteid LIMIT 1)
					 ,(SELECT guid FROM mp_moduledefinitions WHERE moduledefid = :moduledefid LIMIT 1)
					 ,:moduledefid
					 ,'All Users'"
				);

				sqlParams = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter(":siteid", NpgsqlDbType.Integer)
					{
						Direction = ParameterDirection.Input,
						Value = siteId
					},
					new NpgsqlParameter(":moduledefid", NpgsqlDbType.Integer)
					{
						Direction = ParameterDirection.Input,
						Value = newID
					}
				};

				NpgsqlHelper.ExecuteNonQuery(
					ConnectionString.GetWriteConnectionString(),
					CommandType.Text,
					sqlCommand.ToString(),
					sqlParams.ToArray()
				);
			}

			return newID;
		}


		/// <summary>
		/// Updates a row in the mp_ModuleDefinitions table. Returns true if row updated.
		/// </summary>
		/// <param name="moduleDefID"> moduleDefID </param>
		/// <param name="featureName"> featureName </param>
		/// <param name="controlSrc"> controlSrc </param>
		/// <param name="sortOrder"> sortOrder </param>
		/// <param name="isAdmin"> isAdmin </param>
		/// <param name="icon"> icon </param>
		/// <param name="defaultCacheTime"> defaultCacheTime </param>
		/// <param name="guid"> guid </param>
		/// <param name="resourceFile"> resourceFile </param>
		/// <param name="isCacheable"> isCacheable </param>
		/// <param name="isSearchable"> isSearchable </param>
		/// <param name="searchListName"> searchListName </param>
		/// <returns>bool</returns>
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
			string skinFileName
		)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.AppendFormat("UPDATE mp_moduledefinitions SET {0} WHERE moduledefid = :moduledefid;"
				, @"featurename = :featurename
				  ,controlsrc = :controlsrc
				  ,sortorder = :sortorder
				  ,isadmin = :isadmin
				  ,icon = :icon
				  ,defaultcachetime = :defaultcachetime
				  ,resourcefile = :resourcefile
				  ,supportspagereuse = :supportspagereuse
				  ,deleteprovider = :deleteprovider
				  ,partialview = :partialview
				  ,skinfilename = :skinfilename
				  ,iscacheable = :iscacheable
				  ,issearchable = :issearchable
				  ,searchlistname = :searchlistname");

			var sqlParams = new List<NpgsqlParameter>()
			{
				new NpgsqlParameter(":moduledefid", NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = moduleDefId
				},
				new NpgsqlParameter(":featurename", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = featureName
				},
				new NpgsqlParameter(":controlsrc", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = controlSrc
				},
				new NpgsqlParameter(":sortorder", NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = sortOrder
				},
				new NpgsqlParameter(":isadmin", NpgsqlDbType.Boolean)
				{
					Direction = ParameterDirection.Input,
					Value = isAdmin
				},
				new NpgsqlParameter(":icon", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = icon
				},
				new NpgsqlParameter(":defaultcachetime", NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = defaultCacheTime
				},
				new NpgsqlParameter(":resourcefile", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = resourceFile
				},
				new NpgsqlParameter(":iscacheable", NpgsqlDbType.Boolean)
				{
					Direction = ParameterDirection.Input,
					Value = isCacheable
				},
				new NpgsqlParameter(":issearchable", NpgsqlDbType.Boolean)
				{
					Direction = ParameterDirection.Input,
					Value = isSearchable
				},
				new NpgsqlParameter(":searchlistname", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = searchListName
				},
				new NpgsqlParameter(":supportspagereuse", NpgsqlDbType.Boolean)
				{
					Direction = ParameterDirection.Input,
					Value = supportsPageReuse
				},
				new NpgsqlParameter(":deleteprovider", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = deleteProvider
				},
				new NpgsqlParameter(":partialview", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = partialView
				},
				new NpgsqlParameter(":skinfilename", NpgsqlDbType.Varchar, 255) {
					Direction = ParameterDirection.Input,
					Value = skinFileName
				}
			};

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				sqlParams.ToArray()
			);

			return rowsAffected > -1;
		}


		public static bool UpdateSiteModulePermissions(
			int siteId,
			int moduleDefId,
			string authorizedRoles
		)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append(@"UPDATE mp_sitemoduledefinitions
				SET authorizedroles = :authorizedroles 
				WHERE siteid = :siteid 
					AND moduledefid = :moduledefid;");

			var sqlParams = new List<NpgsqlParameter>()
			{
				new NpgsqlParameter(":siteid", NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = siteId
				},
				new NpgsqlParameter(":moduledefid", NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = moduleDefId
				},
				new NpgsqlParameter(":authorizedroles", NpgsqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = authorizedRoles
				}
			};

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				sqlParams.ToArray()
			);

			return rowsAffected > -1;
		}


		public static bool DeleteModuleDefinition(int moduleDefId)
		{
			var arParams = new NpgsqlParameter[]
			{
				new NpgsqlParameter(":moduledefid", NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = moduleDefId
				}
			};


			int rowsAffected = Convert.ToInt32(
					NpgsqlHelper.ExecuteScalar(
					ConnectionString.GetWriteConnectionString(),
					CommandType.StoredProcedure,
					"mp_moduledefinitions_delete(:moduledefid)",
					arParams
				)
			);

			return rowsAffected > -1;
		}


		public static bool DeleteModuleDefinitionFromSites(int moduleDefId)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("DELETE FROM mp_sitemoduledefinitions ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("moduledefid = :moduledefid ");
			sqlCommand.Append(";");

			var arParams = new NpgsqlParameter[]
			{
				new NpgsqlParameter(":moduledefid", NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = moduleDefId
				}
			};


			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > -1;
		}


		public static bool DeleteSettingById(int id)
		{
			var arParams = new NpgsqlParameter[]
			{
				new NpgsqlParameter(":id", NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = id
				}
			};


			int rowsAffected = Convert.ToInt32(
				NpgsqlHelper.ExecuteScalar(
					ConnectionString.GetWriteConnectionString(),
					CommandType.StoredProcedure,
					"mp_moduledefinitions_deletesettingbyid(:id)",
					arParams
				)
			);

			return rowsAffected > -1;
		}


		public static bool DeleteSettingsByFeature(int moduleDefId)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("DELETE FROM mp_moduledefinitionsettings ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("moduledefid = :moduledefid;");

			var arParams = new NpgsqlParameter[]
			{
				new NpgsqlParameter(":moduledefid", NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = moduleDefId
				}
			};

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > -1;
		}


		public static IDataReader GetModuleDefinition(int moduleDefId)
		{
			var arParams = new NpgsqlParameter[]
			{
				new NpgsqlParameter(":moduledefid", NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = moduleDefId
				}
			};

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.StoredProcedure,
				"mp_moduledefinitions_selectone(:moduledefid)",
				arParams
			);
		}


		public static IDataReader GetModuleDefinition(Guid featureGuid)
		{
			var arParams = new NpgsqlParameter[]
			{
				new NpgsqlParameter(":featureguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = featureGuid.ToString()
				}
			};

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.StoredProcedure,
				"mp_moduledefinitions_selectonebyguid(:featureguid)",
				arParams
			);
		}


		public static void EnsureInstallationInAdminSites()
		{
			var sqlCommand = @"
insert into mp_sitemoduledefinitions (
	siteid,
	moduledefid,
	siteguid,
	featureguid,
	authorizedroles
)
select distinct  
	s.siteid,  
	md.moduledefid,  
	s.siteguid,  
	(
		select guid
		from mp_moduledefinitions
		where moduledefid = md.moduledefid
		limit 1
	), 
	'All Users'
from mp_sites s,
mp_moduledefinitions md
where s.isserveradminsite = true
and md.moduledefid
not in
(
	select smd.moduledefid 
	from mp_sitemoduledefinitions smd 
	where smd.siteid = s.siteid
);";

			NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand,
				null
			);
		}


		public static IDataReader GetModuleDefinitions(Guid siteGuid)
		{
			var sqlCommand = @"SELECT md.*, smd.authorizedroles 
					FROM	mp_moduledefinitions md 
					JOIN	mp_sitemoduledefinitions smd  
								ON md.moduledefid = smd.moduledefid  
					WHERE smd.siteguid = :siteguid 
					ORDER BY md.sortorder, md.featurename ;";

			var sqlParams = new List<NpgsqlParameter>()
			{
				new NpgsqlParameter(":siteguid", NpgsqlDbType.Char, 36) {
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				}
			};

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand,
				sqlParams.ToArray()
			);
		}


		public static DataTable GetModuleDefinitionsBySite(Guid siteGuid)
		{
			DataTable dt = new DataTable();

			dt.Columns.Add("ModuleDefID", typeof(int));
			dt.Columns.Add("FeatureGuid", typeof(string));
			dt.Columns.Add("FeatureName", typeof(string));
			dt.Columns.Add("ControlSrc", typeof(string));
			dt.Columns.Add("AuthorizedRoles", typeof(string));

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
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("select * from mp_moduledefinitions where skinfilename = :skinfilename limit 1;");

			var sqlParams = new List<NpgsqlParameter>()
			{
				new NpgsqlParameter(":skinfilename", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = skinFileName
				}
			};

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				sqlParams.ToArray()
			);
		}


		public static IDataReader GetAllModuleSkinFileNames()
		{
			var sqlCommand = "SELECT skinfilename FROM mp_moduledefinitions;";

			return NpgsqlHelper.ExecuteReader(
				 ConnectionString.GetReadConnectionString(),
				 CommandType.Text,
				 sqlCommand
			);
		}


		public static IDataReader GetUserModules(int siteId)
		{
			var commandText = @"
SELECT md.*, smd.FeatureGuid, smd.AuthorizedRoles
FROM mp_ModuleDefinitions md
JOIN mp_SiteModuleDefinitions smd
ON smd.ModuleDefID = md.ModuleDefID
WHERE smd.SiteID = :SiteID
AND md.IsAdmin = FALSE
ORDER BY 
md.SortOrder,
md.FeatureName";

			var commandParameters = new NpgsqlParameter[]
			{
				new NpgsqlParameter(":siteid", NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = siteId
				}
			};

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				commandText,
				commandParameters
			);
		}


		public static IDataReader GetSearchableModules(int siteId)
		{
			var sqlCommand = @"SELECT md.* 
				FROM	mp_moduledefinitions md 
				JOIN	mp_sitemoduledefinitions smd  
							ON md.moduledefid = smd.moduledefid  
				WHERE smd.siteid = :siteid 
					AND md.isadmin = false 
					AND md.issearchable = true 
				ORDER BY md.sortorder, md.searchlistname ;";

			var sqlParams = new List<NpgsqlParameter>()
			{
				new NpgsqlParameter(":siteid", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = siteId }
			};

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand,
				sqlParams.ToArray()
			);
		}


		private static bool SettingExists(
			Guid featureGuid,
			int moduleDefId,
			string settingName
		)
		{
			var sqlCommand = @"SELECT  Count(*) 
				FROM	mp_moduledefinitionsettings
				WHERE (moduledefid = :moduledefid OR featureguid = :featureguid)
				AND settingname = :settingname;";

			var sqlParams = new List<NpgsqlParameter>()
			{
				new NpgsqlParameter(":featureguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = featureGuid.ToString()
				},
				new NpgsqlParameter(":moduledefid", NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = moduleDefId
				},
				new NpgsqlParameter(":settingname", NpgsqlDbType.Varchar, 50)
				{
					Direction = ParameterDirection.Input,
					Value = settingName
				}
			};

			int count = Convert.ToInt32(
				NpgsqlHelper.ExecuteScalar(
					ConnectionString.GetReadConnectionString(),
					CommandType.Text,
					sqlCommand,
					sqlParams.ToArray()
				)
			);

			return count > 0;
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
			string options
		)
		{
			if (!SettingExists(featureGuid, moduleDefId, settingName))
			{
				return CreateSetting(
					moduleDefId,
					groupName,
					settingName,
					settingValue,
					controlType,
					regexValidationExpression,
					featureGuid,
					resourceFile,
					controlSrc,
					sortOrder,
					helpKey,
					attributes,
					options
				);
			}
			else
			{
				var sqlCommand = @"UPDATE mp_moduledefinitionsettings
					SET settingvalue = :settingvalue
						,controltype = :controltype
						,regexvalidationexpression = :regexvalidationexpression
						,featureguid = :featureguid
						,resourcefile = :resourcefile
						,controlsrc = :controlsrc
						,sortorder = :sortorder
						,groupname = :groupname 
						,helpkey = :helpkey
						,attributes = :attributes
						,options = :options
					WHERE (moduledefid = :moduledefid OR featureguid = :featureguid)  
					  AND settingname = :settingname;";

				var sqlParams = new List<NpgsqlParameter>()
				{
					new NpgsqlParameter(":featureguid", NpgsqlDbType.Char, 36)
					{
						 Direction = ParameterDirection.Input,
						 Value = featureGuid.ToString()
					},
					new NpgsqlParameter(":moduledefid", NpgsqlDbType.Integer)
					{
						 Direction = ParameterDirection.Input,
						 Value = moduleDefId
					},
					new NpgsqlParameter(":settingname", NpgsqlDbType.Varchar, 50)
					{
						 Direction = ParameterDirection.Input,
						 Value = settingName
					},
					new NpgsqlParameter(":settingvalue", NpgsqlDbType.Text)
					{
						 Direction = ParameterDirection.Input,
						 Value = settingValue
					},
					new NpgsqlParameter(":controltype", NpgsqlDbType.Varchar, 50)
					{
						 Direction = ParameterDirection.Input,
						 Value = controlType
					},
					new NpgsqlParameter(":regexvalidationexpression", NpgsqlDbType.Text)
					{
						 Direction = ParameterDirection.Input,
						 Value = regexValidationExpression
					},
					new NpgsqlParameter(":resourcefile", NpgsqlDbType.Varchar, 255)
					{
						 Direction = ParameterDirection.Input,
						 Value = resourceFile
					},
					new NpgsqlParameter(":controlsrc", NpgsqlDbType.Varchar, 255)
					{
						 Direction = ParameterDirection.Input,
						 Value = controlSrc
					},
					new NpgsqlParameter(":sortorder", NpgsqlDbType.Integer)
					{
						 Direction = ParameterDirection.Input,
						 Value = sortOrder
					},
					new NpgsqlParameter(":helpkey", NpgsqlDbType.Varchar, 255)
					{
						 Direction = ParameterDirection.Input,
						 Value = helpKey
					},
					new NpgsqlParameter(":groupname", NpgsqlDbType.Varchar, 255)
					{
						 Direction = ParameterDirection.Input,
						 Value = groupName
					},
					new NpgsqlParameter(":attributes", NpgsqlDbType.Text)
					{
						 Direction = ParameterDirection.Input,
						 Value = attributes
					},
					new NpgsqlParameter(":options", NpgsqlDbType.Text)
					{
						 Direction = ParameterDirection.Input,
						 Value = options
					}
				};

				int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
					ConnectionString.GetWriteConnectionString(),
					CommandType.Text,
					sqlCommand.ToString(),
					sqlParams.ToArray()
				);

				return rowsAffected > -1;
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
			string options
		)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.AppendFormat("UPDATE mp_moduledefinitionsettings SET {0} WHERE id = :id;"
				, @"moduledefid = :moduledefid
				  ,settingname = :settingname
				  ,settingvalue = :settingvalue
				  ,controltype = :controltype
				  ,regexvalidationexpression = :regexvalidationexpression
				  ,resourcefile = :resourcefile
				  ,controlsrc = :controlsrc
				  ,sortorder = :sortorder
				  ,groupname = :groupname
				  ,helpkey = :helpkey
				  ,attributes = :attributes
				  ,options = :options"
			);

			var sqlParams = new List<NpgsqlParameter>()
			{
				new NpgsqlParameter(":id", NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = id
				},
				new NpgsqlParameter(":moduledefid", NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = moduleDefId
				},
				new NpgsqlParameter(":settingname", NpgsqlDbType.Varchar, 50)
				{
					Direction = ParameterDirection.Input,
					Value = settingName
				},
				new NpgsqlParameter(":settingvalue", NpgsqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = settingValue
				},
				new NpgsqlParameter(":controltype", NpgsqlDbType.Varchar, 50)
				{
					Direction = ParameterDirection.Input,
					Value = controlType
				},
				new NpgsqlParameter(":regexvalidationexpression", NpgsqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = regexValidationExpression
				},
				new NpgsqlParameter(":resourcefile", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = resourceFile
				},
				new NpgsqlParameter(":controlsrc", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = controlSrc
				},
				new NpgsqlParameter(":sortorder", NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = sortOrder
				},
				new NpgsqlParameter(":helpkey", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = helpKey
				},
				new NpgsqlParameter(":groupname", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = groupName
				},
				new NpgsqlParameter(":attributes", NpgsqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = attributes
				},
				new NpgsqlParameter(":options", NpgsqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = options
				}
			};

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				sqlParams.ToArray()
			);

			return rowsAffected > -1;
		}


		private static bool CreateSetting(
			int moduleDefId,
			string groupName,
			string settingName,
			string settingValue,
			string controlType,
			string regexValidationExpression,
			Guid featureGuid,
			string resourceFile,
			string controlSrc,
			int sortOrder,
			string helpKey,
			string attributes,
			string options
		)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.AppendFormat("INSERT INTO mp_moduledefinitionsettings ({0}) VALUES ({1});"
				, @"featureguid
				  ,moduledefid
				  ,resourcefile
				  ,settingname
				  ,settingvalue
				  ,controltype
				  ,controlsrc
				  ,helpkey
				  ,sortorder
				  ,groupname
				  ,regexvalidationexpression
				  ,attributes
				  ,options"
				, @":featureguid
				  ,:moduledefid
				  ,:resourcefile
				  ,:settingname
				  ,:settingvalue
				  ,:controltype
				  ,:controlsrc
				  ,:helpkey
				  ,:sortorder
				  ,:groupname
				  ,:regexvalidationexpression
				  ,:attributes
				  ,:options"
			);

			var sqlParams = new List<NpgsqlParameter>()
			{
				new NpgsqlParameter(":featureguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = featureGuid.ToString()
				},
				new NpgsqlParameter(":moduledefid", NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = moduleDefId
				},
				new NpgsqlParameter(":settingname", NpgsqlDbType.Varchar, 50)
				{
					Direction = ParameterDirection.Input,
					Value = settingName
				},
				new NpgsqlParameter(":settingvalue", NpgsqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = settingValue
				},
				new NpgsqlParameter(":controltype", NpgsqlDbType.Varchar, 50)
				{
					Direction = ParameterDirection.Input,
					Value = controlType
				},
				new NpgsqlParameter(":regexvalidationexpression", NpgsqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = regexValidationExpression
				},
				new NpgsqlParameter(":resourcefile", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = resourceFile
				},
				new NpgsqlParameter(":controlsrc", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = controlSrc
				},
				new NpgsqlParameter(":sortorder", NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = sortOrder
				},
				new NpgsqlParameter(":helpkey", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = helpKey
				},
				new NpgsqlParameter(":groupname", NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = groupName
				},
				new NpgsqlParameter(":attributes", NpgsqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = attributes
				},
				new NpgsqlParameter(":options", NpgsqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = options
				}
			};

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				sqlParams.ToArray()
			);

			return rowsAffected > 0;
		}


		public static IDataReader ModuleDefinitionSettingsGetSetting(Guid featureGuid, string settingName)
		{
			var sqlParams = new List<NpgsqlParameter>()
			{
				new NpgsqlParameter(":featureguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = featureGuid.ToString()
				},
				new NpgsqlParameter(":settingname", NpgsqlDbType.Varchar, 50)
				{
					Direction = ParameterDirection.Input,
					Value = settingName
				}
			};

			return NpgsqlHelper.ExecuteReader(ConnectionString.GetReadConnectionString(),
				CommandType.StoredProcedure,
				"mp_moduledefinitionsettings_selectone(:featureguid,:settingname)",
				sqlParams.ToArray()
			);
		}
	}
}
