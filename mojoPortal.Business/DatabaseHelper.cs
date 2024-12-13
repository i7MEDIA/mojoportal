using System.Collections;
using System.Data;
using System.Data.Common;
using System.IO;
using log4net;
using mojoPortal.Data;

namespace mojoPortal.Business;

public static class DatabaseHelper
{
	private static readonly ILog log = LogManager.GetLogger(typeof(DatabaseHelper));

	public static string GetApplicationName() => "mojoportal-core";

	public static Guid GetApplicationId() => new("077e4857-f583-488e-836e-34a4b04be855");

	public static string DBPlatform() => DBPortal.DBPlatform();

	/// <summary>
	/// Maintained/updated in code to make Setup run the new version upgrade script
	/// </summary>
	/// <returns>Version</returns>
	public static Version AppCodeVersion() => new(2, 9, 1, 0);

	public static Version SchemaVersion()
	{
		// this should never change
		// its the last version before auto upgrades
		var major = 2;
		var minor = 2;
		var build = 1;
		var revision = 5;

		var found = false;

		try
		{
			using (IDataReader reader = DBPortal.SchemaVersionGetSchemaVersion(GetApplicationId()))
			{
				if (reader.Read())
				{
					major = Convert.ToInt32(reader["Major"]);
					minor = Convert.ToInt32(reader["Minor"]);
					build = Convert.ToInt32(reader["Build"]);
					revision = Convert.ToInt32(reader["Revision"]);

					found = true;
				}
			}

			if (!found)
			{
				DBPortal.SchemaVersionAddSchemaVersion(
					GetApplicationId(),
					GetApplicationName(),
					major,
					minor,
					build,
					revision
				);
			}
		}
		catch (DbException) { }
		catch (InvalidOperationException) { }

		return new Version(major, minor, build, revision);
	}

	public static void EnsureDatabase() => DBPortal.EnsureDatabase();

	public static bool UpdateSchemaVersion(
		Guid applicationId,
		string applicationName,
		int major,
		int minor,
		int build,
		int revision
	)
	{
		if (!DBPortal.SchemaVersionExists(applicationId))
		{
			return DBPortal.SchemaVersionAddSchemaVersion(
				applicationId,
				applicationName,
				major,
				minor,
				build,
				revision
			);
		}

		return DBPortal.SchemaVersionUpdateSchemaVersion(
			applicationId,
			applicationName,
			major,
			minor,
			build,
			revision
		);
	}

	public static bool DeleteSchemaVersion(Guid applicationId) => DBPortal.SchemaVersionDeleteSchemaVersion(applicationId);

	public static Version GetSchemaVersion(string applicationName)
	{
		Guid appGuid = GetApplicationId(applicationName);

		return GetSchemaVersion(appGuid);
	}

	public static Version GetSchemaVersion(Guid applicationId)
	{
		var major = 0;
		var minor = 0;
		var build = 0;
		var revision = 0;

		try
		{
			using IDataReader reader = DBPortal.SchemaVersionGetSchemaVersion(applicationId);
			if (reader.Read())
			{
				major = Convert.ToInt32(reader["Major"]);
				minor = Convert.ToInt32(reader["Minor"]);
				build = Convert.ToInt32(reader["Build"]);
				revision = Convert.ToInt32(reader["Revision"]);
			}
		}
		catch (DbException) { }
		catch (InvalidOperationException) { }
		catch (Exception ex)
		{
			// hate to trap System.Exception but SqlCeException does not inherit from DbException as it should
			if (DBPlatform() != "SqlCe")
			{
				throw;
			}

			log.Error(ex);
		}

		return new Version(major, minor, build, revision);
	}

	public static int AddSchemaScriptHistory(
		Guid applicationId,
		string scriptFile,
		DateTime runTime,
		bool errorOccurred,
		string errorMessage,
		string scriptBody
	)
	{
		return DBPortal.SchemaScriptHistoryAddSchemaScriptHistory(
			applicationId,
			scriptFile,
			runTime,
			errorOccurred,
			errorMessage,
			scriptBody
		);
	}

	public static bool DeleteSchemaScriptHistory(int id) => DBPortal.SchemaScriptHistoryDeleteSchemaScriptHistory(id);

	public static IDataReader SchemaVersionGetNonCore() => DBPortal.SchemaVersionGetNonCore();

	public static IDataReader GetSchemaScriptHistory(int id) => DBPortal.SchemaScriptHistoryGetSchemaScriptHistory(id);

	public static IDataReader GetSchemaScriptHistory(Guid applicationId) => DBPortal.SchemaScriptHistoryGetSchemaScriptHistory(applicationId);

	public static IDataReader GetSchemaScriptErrorHistory(Guid applicationId) => DBPortal.SchemaScriptHistoryGetSchemaScriptErrorHistory(applicationId);

	public static bool SchemaScriptHasBeenRun(Guid applicationId, string scriptFile)
	{
		try
		{
			return DBPortal.SchemaScriptHistoryExists(applicationId, scriptFile);
		}
		catch (DbException ex)
		{
			log.Error("Exception caught when checking for Schema Script History.", ex);
		}

		return false;
	}

	public static bool CanAccessDatabase() => DBPortal.DatabaseHelperCanAccessDatabase();

	public static DbException GetConnectionError(string overrideConnectionInfo) => DBPortal.DatabaseHelperGetConnectionError(overrideConnectionInfo);

	public static bool CanAlterSchema(string overrideConnectionInfo)
	{
		try
		{
			return DBPortal.DatabaseHelperCanAlterSchema(overrideConnectionInfo);
		}
		catch (DbException ex)
		{
			log.Error(ex.ToString());

			return false;
		}
	}

	public static bool SchemaHasBeenCreated() => DBPortal.DatabaseHelperSitesTableExists();

	public static int ExistingSiteCount()
	{
		var siteCount = 0;

		try
		{
			siteCount = SiteSettings.SiteCount();
		}
		catch (DbException) { }
		catch (InvalidOperationException) { }
		catch (Exception ex)
		{
			//this is a needed hack because SqlCeException does not inherit from DbException like other data layers
			//instead it inherits from System.Exception which we would rather not trap
			if (DBPlatform().ToLower() != "sqlce")
			{
				throw ex;
			}
		}

		return siteCount;
	}

	public static string RunScript(Guid applicationId, FileInfo scriptFile, string overrideConnectionInfo)
	{
		// returning empty string indicates success
		// else return error message
		var resultMessage = string.Empty;

		if (scriptFile == null)
		{
			return resultMessage;
		}

		try
		{
			bool result = DBPortal.DatabaseHelperRunScript(scriptFile, overrideConnectionInfo);

			if (!result)
			{
				resultMessage = "script failed with no error message";
			}
		}
		catch (DbException ex)
		{
			resultMessage = ex.ToString();
		}

		return resultMessage;
	}

	public static IDataReader GetReader(string query) => GetReader(string.Empty, query);

	public static IDataReader GetReader(string connectionString, string query) => DBPortal.DatabaseHelperGetReader(connectionString, query);

	public static int ExecteNonQuery(string query) => ExecteNonQuery(string.Empty, query);

	public static int ExecteNonQuery(string connectionString, string query) => DBPortal.DatabaseHelperExecteNonQuery(connectionString, query);


	public static Version ParseVersionFromFileName(String fileName)
	{
		Version version = null;
		var success = true;

		if (fileName != null)
		{
			char[] separator = ['.'];
			string[] args = fileName.Replace(".config", string.Empty).Split(separator);

			if (args.Length >= 4)
			{
				if (!int.TryParse(args[0], out int major))
				{
					major = 0;
					success = false;
				}

				if (!int.TryParse(args[1], out int minor))
				{
					minor = 0;
					success = false;
				}

				if (!int.TryParse(args[2], out int build))
				{
					build = 0;
					success = false;
				}

				if (!int.TryParse(args[3], out int revision))
				{
					revision = 0;
					success = false;
				}

				if (success)
				{
					version = new Version(major, minor, build, revision);
				}
			}
		}

		return version;
	}

	public static Guid GetApplicationId(string applicationName)
	{
		if (string.Equals(applicationName, "mojoportal-core", StringComparison.InvariantCultureIgnoreCase))
		{
			return new Guid("077e4857-f583-488e-836e-34a4b04be855");
		}

		var appID = Guid.NewGuid();

		using (IDataReader reader = DBPortal.DatabaseHelperGetApplicationId(applicationName))
		{
			if (reader.Read())
			{
				appID = new Guid(reader["ApplicationID"].ToString());
			}
		}

		return appID;
	}

	public static string GetApplicationName(Guid applicationId)
	{
		string appName = null;

		using (IDataReader reader = DBPortal.SchemaVersionGetSchemaVersion(applicationId))
		{
			if (reader.Read())
			{
				appName = reader["ApplicationName"].ToString();
			}
		}

		return appName;
	}

	public static bool UpdateTableField(
		string connectionString,
		string tableName,
		string keyFieldName,
		string keyFieldValue,
		string dataFieldName,
		string dataFieldValue,
		string additionalWhere
	)
	{
		if (dataFieldName is null)
		{
			throw new ArgumentNullException(nameof(dataFieldName));
		}

		return DBPortal.DatabaseHelperUpdateTableField(
			connectionString,
			tableName,
			keyFieldName,
			keyFieldValue,
			dataFieldName,
			dataFieldValue,
			additionalWhere
		);
	}

	public static bool UpdateTableField(
		string tableName,
		string keyFieldName,
		string keyFieldValue,
		string dataFieldName,
		string dataFieldValue,
		string additionalWhere
	)
	{
		return DBPortal.DatabaseHelperUpdateTableField(
			tableName,
			keyFieldName,
			keyFieldValue,
			dataFieldName,
			dataFieldValue,
			additionalWhere
		);
	}

	public static IDataReader GetReader(string connectionString, string tableName, string whereClause)
	{
		return DBPortal.DatabaseHelperGetReader(connectionString, tableName, whereClause);
	}

	public static DataTable GetTable(string connectionString, string tableName, string whereClause)
	{
		return DBPortal.DatabaseHelperGetTable(connectionString, tableName, whereClause);
	}

	#region Version Specific tasks
	public static bool RunPostUpgradeTasks(Version version, string overrideConnectionString)
	{
		return DBPortal.RunPostUpgradeTask(version, overrideConnectionString);
	}

	public static void DoForumVersion2202PostUpgradeTasks(string overrideConnectionInfo)
	{
		DBPortal.DatabaseHelperDoForumVersion2202PostUpgradeTasks(overrideConnectionInfo);
	}

	public static void DoForumVersion2203PostUpgradeTasks(string overrideConnectionInfo)
	{
		DBPortal.DatabaseHelperDoForumVersion2203PostUpgradeTasks(overrideConnectionInfo);
	}


	#endregion

	public static DataTable GetTableFromDataReader(IDataReader reader)
	{
		var schemaTable = reader.GetSchemaTable();
		var dataTable = new DataTable();
		DataColumn column;
		DataRow row;
		var arrayList = new ArrayList();

		for (int i = 0; i < schemaTable.Rows.Count; i++)
		{
			column = new DataColumn();

			if (!dataTable.Columns.Contains(schemaTable.Rows[i]["ColumnName"].ToString()))
			{
				column.ColumnName = schemaTable.Rows[i]["ColumnName"].ToString();
				column.Unique = Convert.ToBoolean(schemaTable.Rows[i]["IsUnique"]);
				column.AllowDBNull = Convert.ToBoolean(schemaTable.Rows[i]["AllowDBNull"]);
				column.ReadOnly = Convert.ToBoolean(schemaTable.Rows[i]["IsReadOnly"]);
				arrayList.Add(column.ColumnName);
				dataTable.Columns.Add(column);
			}
		}

		try
		{
			while (reader.Read())
			{
				row = dataTable.NewRow();

				for (int i = 0; i < arrayList.Count; i++)
				{
					row[(string)arrayList[i]] = reader[(string)arrayList[i]];

				}

				dataTable.Rows.Add(row);
			}
		}
		finally
		{
			reader.Close();
		}

		return dataTable;
	}
}