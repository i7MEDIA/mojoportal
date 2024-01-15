using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using log4net;
using MySqlConnector;


namespace mojoPortal.Data;


public static class DBPortal
{

	private static readonly ILog log = LogManager.GetLogger(typeof(DBPortal));

	public static string DBPlatform()
	{
		return "MySQL";
	}


	public static void EnsureDatabase()
	{
		//not applicable for this platform

	}


	#region Versioning and Upgrade Helpers



	#region Schema Table Methods

	public static IDataReader DatabaseHelperGetApplicationId(string applicationName)
	{
		return DatabaseHelperGetReader(
			ConnectionString.GetReadConnectionString(),
			"mp_SchemaVersion",
			" WHERE LCASE(ApplicationName) = '" + applicationName.ToLower() + "'");

	}


	public static bool SchemaVersionAddSchemaVersion(
		Guid applicationId,
		string applicationName,
		int major,
		int minor,
		int build,
		int revision)
	{

		#region Bit Conversion


		#endregion

		string sqlCommand = @"
INSERT INTO 
	mp_SchemaVersion (
		ApplicationID, 
		ApplicationName, 
		Major, 
		Minor, 
		Build, 
		Revision 
	)
VALUES (
	?ApplicationID, 
	?ApplicationName, 
	?Major, 
	?Minor, 
	?Build, 
	?Revision 
);";


		var arParams = new List<MySqlParameter>
		{
			new("?ApplicationID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = applicationId.ToString()
			},

			new("?ApplicationName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = applicationName
			},

			new("?Major", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = major
			},

			new("?Minor", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = minor
			},

			new("?Build", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = build
			},

			new("?Revision", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = revision
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}


	public static bool SchemaVersionUpdateSchemaVersion(
		Guid applicationId,
		string applicationName,
		int major,
		int minor,
		int build,
		int revision)
	{
		#region Bit Conversion


		#endregion

		string sqlCommand = @"
UPDATE mp_SchemaVersion 
SET ApplicationName = ?ApplicationName, 
Major = ?Major, 
Minor = ?Minor, 
Build = ?Build, 
Revision = ?Revision 
WHERE ApplicationID = ?ApplicationID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ApplicationID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = applicationId.ToString()
			},

			new("?ApplicationName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = applicationName
			},

			new("?Major", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = major
			},

			new("?Minor", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = minor
			},

			new("?Build", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = build
			},

			new("?Revision", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = revision
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}


	public static bool SchemaVersionDeleteSchemaVersion(
		Guid applicationId)
	{
		string sqlCommand = @"
DELETE FROM mp_SchemaVersion 
WHERE ApplicationID = ?ApplicationID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ApplicationID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = applicationId.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool SchemaVersionExists(Guid applicationId)
	{
		bool result = false;

		using (IDataReader reader = SchemaVersionGetSchemaVersion(applicationId))
		{
			if (reader.Read())
			{
				result = true;
			}
		}

		return result;
	}

	public static IDataReader SchemaVersionGetSchemaVersion(
		Guid applicationId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SchemaVersion 
WHERE ApplicationID = ?ApplicationID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ApplicationID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = applicationId.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader SchemaVersionGetNonCore()
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SchemaVersion 
WHERE ApplicationID <> '077E4857-F583-488E-836E-34A4B04BE855' 
ORDER BY ApplicationName;";

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString());

	}

	public static int SchemaScriptHistoryAddSchemaScriptHistory(
		Guid applicationId,
		string scriptFile,
		DateTime runTime,
		bool errorOccurred,
		string errorMessage,
		string scriptBody)
	{

		#region Bit Conversion
		int intErrorOccurred;
		if (errorOccurred)
		{
			intErrorOccurred = 1;
		}
		else
		{
			intErrorOccurred = 0;
		}


		#endregion

		string sqlCommand = @"
INSERT INTO 
	mp_SchemaScriptHistory (
		ApplicationID, 
		ScriptFile, 
		RunTime, 
		ErrorOccurred, 
		ErrorMessage, 
		ScriptBody 
	)
VALUES (
	?ApplicationID, 
	?ScriptFile, 
	?RunTime, 
	?ErrorOccurred, 
	?ErrorMessage, 
	?ScriptBody 
);
SELECT LAST_INSERT_ID();";

		var arParams = new List<MySqlParameter>
		{
			new("?ApplicationID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = applicationId.ToString()
			},

			new("?ScriptFile", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = scriptFile
			},

			new("?RunTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = runTime
			},

			new("?ErrorOccurred", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intErrorOccurred
			},

			new("?ErrorMessage", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = errorMessage
			},

			new("?ScriptBody", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = scriptBody
			}
		};

		int newID = 0;
		newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams).ToString());
		return newID;

	}

	public static bool SchemaScriptHistoryDeleteSchemaScriptHistory(int id)
	{
		string sqlCommand = @"
DELETE FROM mp_SchemaScriptHistory 
WHERE ID = ?ID ;";

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

	public static IDataReader SchemaScriptHistoryGetSchemaScriptHistory(int id)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SchemaScriptHistory 
WHERE ID = ?ID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = id
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader SchemaScriptHistoryGetSchemaScriptHistory(Guid applicationId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SchemaScriptHistory 
WHERE ApplicationID = ?ApplicationID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ApplicationID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = applicationId.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader SchemaScriptHistoryGetSchemaScriptErrorHistory(Guid applicationId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SchemaScriptHistory 
WHERE ApplicationID = ?ApplicationID 
AND ErrorOccurred = 1;";

		var arParams = new List<MySqlParameter>
		{
			new("?ApplicationID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = applicationId.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static bool SchemaScriptHistoryExists(Guid applicationId, string scriptFile)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_SchemaScriptHistory 
WHERE ApplicationID = ?ApplicationID 
AND ScriptFile = ?ScriptFile;";

		var arParams = new List<MySqlParameter>
		{
			new("?ApplicationID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = applicationId.ToString()
			},

			new("?ScriptFile", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = scriptFile
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

		return count > 0;

	}




	#endregion

	#endregion


	#region DatabaseHelper

	public static DataTable GetTableFromDataReader(IDataReader reader)
	{
		DataTable dataTable = new();

		using (reader)
		{
			DataTable schemaTable = reader.GetSchemaTable();
			DataColumn column;
			DataRow row;
			ArrayList arrayList = [];

			for (int i = 0; i < schemaTable.Rows.Count; i++)
			{

				column = new DataColumn();

				if (!dataTable.Columns.Contains(schemaTable.Rows[i]["ColumnName"].ToString()))
				{

					column.ColumnName = schemaTable.Rows[i]["ColumnName"].ToString();

					// We don't always want to enforce constraints, it may be fine to have duplicates in a query even if the underlying table has a unique constraint
					// column.Unique = Convert.ToBoolean(schemaTable.Rows[i]["IsUnique"]);

					column.AllowDBNull = Convert.ToBoolean(schemaTable.Rows[i]["AllowDBNull"]);
					column.ReadOnly = Convert.ToBoolean(schemaTable.Rows[i]["IsReadOnly"]);
					arrayList.Add(column.ColumnName);
					dataTable.Columns.Add(column);

				}

			}

			while (reader.Read())
			{

				row = dataTable.NewRow();

				for (int i = 0; i < arrayList.Count; i++)
				{

					row[((string)arrayList[i])] = reader[(string)arrayList[i]];

				}

				dataTable.Rows.Add(row);

			}

		}

		return dataTable;

	}

	public static DbException DatabaseHelperGetConnectionError(string overrideConnectionInfo)
	{
		DbException exception = null;

		MySqlConnection connection;

		if (
			(overrideConnectionInfo != null)
			&& (overrideConnectionInfo.Length > 0)
		  )
		{
			connection = new MySqlConnection(overrideConnectionInfo);
		}
		else
		{
			connection = new MySqlConnection(ConnectionString.GetWriteConnectionString());
		}

		try
		{
			connection.Open();


		}
		catch (DbException ex)
		{
			exception = ex;
		}
		finally
		{
			if (connection.State == ConnectionState.Open)
				connection.Close();
		}


		return exception;

	}

	public static bool DatabaseHelperCanAccessDatabase(string overrideConnectionInfo)
	{
		bool result = false;

		MySqlConnection connection;

		if (
			(overrideConnectionInfo != null)
			&& (overrideConnectionInfo.Length > 0)
		  )
		{
			connection = new MySqlConnection(overrideConnectionInfo);
		}
		else
		{
			connection = new MySqlConnection(ConnectionString.GetWriteConnectionString());
		}

		try
		{
			connection.Open();
			result = (connection.State == ConnectionState.Open);

		}
		catch { }
		finally
		{
			if (connection.State == ConnectionState.Open)
				connection.Close();
		}


		return result;

	}

	public static bool DatabaseHelperCanAccessDatabase()
	{
		return DatabaseHelperCanAccessDatabase(null);
	}

	public static bool DatabaseHelperCanAlterSchema(string engine, string overrideConnectionInfo)
	{

		bool result = true;
		// Make sure we can create, alter and drop tables
		var sqlCommand = $@"
                CREATE TABLE `mp_Testdb` (
                  `FooID` int(11) NOT NULL auto_increment,
                  `Foo` varchar(255) NOT NULL default '',
                  PRIMARY KEY  (`FooID`)
                ) ENGINE={engine};";

		try
		{
			DatabaseHelperRunScript(sqlCommand, overrideConnectionInfo);
		}
		catch (DbException)
		{
			result = false;
		}
		catch (ArgumentException)
		{
			result = false;
		}

		try
		{
			DatabaseHelperRunScript("ALTER TABLE mp_Testdb ADD COLUMN `MoreFoo` varchar(255) NULL;", overrideConnectionInfo);
		}
		catch (DbException)
		{
			result = false;
		}
		catch (ArgumentException)
		{
			result = false;
		}

		try
		{
			DatabaseHelperRunScript("DROP TABLE mp_Testdb;", overrideConnectionInfo);
		}
		catch (DbException)
		{
			result = false;
		}
		catch (ArgumentException)
		{
			result = false;
		}

		return result;
	}

	public static bool DatabaseHelperCanCreateTemporaryTables()
	{
		bool result = true;
		string sqlCommand = @"
CREATE TEMPORARY TABLE IF NOT EXISTS Temptest 
(IndexID INT NOT NULL AUTO_INCREMENT PRIMARY KEY ,
foo VARCHAR (100) NOT NULL);
DROP TABLE Temptest;";

		try
		{
			DatabaseHelperRunScript(sqlCommand.ToString(), ConnectionString.GetWriteConnectionString());
		}
		catch (Exception)
		{
			result = false;
		}


		return result;
	}

	public static bool DatabaseHelperRunScript(
		FileInfo scriptFile,
		string overrideConnectionInfo)
	{
		if (scriptFile == null) return false;

		string script = File.ReadAllText(scriptFile.FullName);

		if ((script == null) || (script.Length == 0)) return true;

		return DatabaseHelperRunScript(script, overrideConnectionInfo);

	}

	public static bool DatabaseHelperRunScript(string script, string overrideConnectionInfo)
	{
		if ((script == null) || (script.Trim().Length == 0)) return true;

		bool result = false;
		MySqlConnection connection;

		if (
			(overrideConnectionInfo != null)
			&& (overrideConnectionInfo.Length > 0)
		  )
		{
			connection = new MySqlConnection(overrideConnectionInfo);
		}
		else
		{
			connection = new MySqlConnection(ConnectionString.GetWriteConnectionString());
		}

		connection.Open();

		MySqlTransaction transaction = connection.BeginTransaction();

		try
		{
			CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			script);
			result = true;
		}
		catch (MySqlException ex)
		{
			transaction.Rollback();
			log.Error("dbPortal.RunScript failed", ex);
			throw;
		}
		finally
		{
			connection.Close();

		}

		return result;
	}

	public static bool DatabaseHelperUpdateTableField(
		string connectionString,
		string tableName,
		string keyFieldName,
		string keyFieldValue,
		string dataFieldName,
		string dataFieldValue,
		string additionalWhere)
	{

		string sqlCommand = @"
UPDATE " + tableName + " SET " + dataFieldName + " = ?fieldValue WHERE " + keyFieldName + " = " + keyFieldValue + " " + additionalWhere + " ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?fieldValue", MySqlDbType.Blob)
			{
				Direction = ParameterDirection.Input,
				Value = dataFieldValue
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			connectionString,
			sqlCommand.ToString(),
			arParams);


		return rowsAffected > 0;

	}

	public static bool DatabaseHelperUpdateTableField(
		string tableName,
		string keyFieldName,
		string keyFieldValue,
		string dataFieldName,
		string dataFieldValue,
		string additionalWhere)
	{
		string sqlCommand = @"
UPDATE " + tableName + " SET " + dataFieldName + " = ?fieldValue WHERE " + keyFieldName + " = " + keyFieldValue + " " + additionalWhere + " ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?fieldValue", MySqlDbType.Blob)
			{
				Direction = ParameterDirection.Input,
				Value = dataFieldValue
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static IDataReader DatabaseHelperGetReader(
		string connectionString,
		string tableName,
		string whereClause)
	{
		string sqlCommand = @"
SELECT * FROM " + tableName + " " + whereClause + " ; ";

		return CommandHelper.ExecuteReader(
			connectionString,
			sqlCommand.ToString());

	}

	public static IDataReader DatabaseHelperGetReader(
		string connectionString,
		string query
		)
	{
		if (string.IsNullOrEmpty(connectionString)) { connectionString = ConnectionString.GetReadConnectionString(); }

		return CommandHelper.ExecuteReader(
			connectionString,
			query);

	}

	public static int DatabaseHelperExecteNonQuery(
		string connectionString,
		string query
		)
	{
		if (string.IsNullOrEmpty(connectionString)) { connectionString = ConnectionString.GetWriteConnectionString(); }

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			connectionString,
			query);

		return rowsAffected;

	}

	public static DataTable DatabaseHelperGetTable(
		string connectionString,
		string tableName,
		string whereClause)
	{
		string sqlCommand = @"
SELECT * FROM " + tableName + " " + whereClause + " ; ";

		DataSet ds = CommandHelper.ExecuteDataset(
			connectionString,
			sqlCommand.ToString());

		return ds.Tables[0];

	}

	public static void DatabaseHelperDoForumVersion2202PostUpgradeTasks(
		string overrideConnectionInfo)
	{
		string connectionString;
		if (
			(overrideConnectionInfo != null)
			&& (overrideConnectionInfo.Length > 0)
		  )
		{
			connectionString = overrideConnectionInfo;
		}
		else
		{
			connectionString = ConnectionString.GetWriteConnectionString();
		}

		DataTable dataTable = DatabaseHelperGetTable(
			connectionString,
			"mp_Forums",
			" where (ForumGuid is null OR ForumGuid = '00000000-0000-0000-0000-000000000000') ");


		foreach (DataRow row in dataTable.Rows)
		{
			DatabaseHelperUpdateTableField(
				"mp_Forums",
				"ItemID",
				row["ItemID"].ToString(),
				"ForumGuid",
				Guid.NewGuid().ToString(),
				"  ");

		}

		dataTable = DatabaseHelperGetTable(
			connectionString,
			"mp_ForumThreads",
			" where (ThreadGuid is null OR ThreadGuid = '00000000-0000-0000-0000-000000000000') ");


		foreach (DataRow row in dataTable.Rows)
		{
			DatabaseHelperUpdateTableField(
				"mp_ForumThreads",
				"ThreadID",
				row["ThreadID"].ToString(),
				"ThreadGuid",
				Guid.NewGuid().ToString(),
				"  ");

		}

		dataTable = DatabaseHelperGetTable(
			connectionString,
			"mp_ForumPosts",
			" where (PostGuid is null OR PostGuid = '00000000-0000-0000-0000-000000000000') ");


		foreach (DataRow row in dataTable.Rows)
		{
			DatabaseHelperUpdateTableField(
				"mp_ForumPosts",
				"PostID",
				row["PostID"].ToString(),
				"PostGuid",
				Guid.NewGuid().ToString(),
				"  ");

		}

	}

	public static void DatabaseHelperDoForumVersion2203PostUpgradeTasks(
		string overrideConnectionInfo)
	{
		string connectionString;
		if (
			(overrideConnectionInfo != null)
			&& (overrideConnectionInfo.Length > 0)
		  )
		{
			connectionString = overrideConnectionInfo;
		}
		else
		{
			connectionString = ConnectionString.GetWriteConnectionString();
		}

		string sqlCommand = @"
SELECT SubscriptionID 
FROM mp_ForumSubscriptions 
WHERE (SubGuid is null 
OR SubGuid = '00000000-0000-0000-0000-000000000000') ; ";

		DataSet ds = CommandHelper.ExecuteDataset(
			connectionString,
			sqlCommand.ToString());

		DataTable dataTable = ds.Tables[0];


		foreach (DataRow row in dataTable.Rows)
		{
			DatabaseHelperUpdateTableField(
				"mp_ForumSubscriptions",
				"SubscriptionID",
				row["SubscriptionID"].ToString(),
				"SubGuid",
				Guid.NewGuid().ToString(),
				"  ");

		}



		string sqlCommand1 = @"
SELECT ThreadSubscriptionID 
FROM mp_ForumThreadSubscriptions 
WHERE (SubGuid is null 
OR SubGuid = '00000000-0000-0000-0000-000000000000') ; ";

		ds = CommandHelper.ExecuteDataset(
			connectionString,
			sqlCommand1.ToString());

		dataTable = ds.Tables[0];


		foreach (DataRow row in dataTable.Rows)
		{
			DatabaseHelperUpdateTableField(
				"mp_ForumThreadSubscriptions",
				"ThreadSubscriptionID",
				row["ThreadSubscriptionID"].ToString(),
				"SubGuid",
				Guid.NewGuid().ToString(),
				"  ");

		}



	}

	public static void DatabaseHelperDoVersion2320PostUpgradeTasks(
		string overrideConnectionInfo)
	{
		string connectionString;
		if (
			(overrideConnectionInfo != null)
			&& (overrideConnectionInfo.Length > 0)
		  )
		{
			connectionString = overrideConnectionInfo;
		}
		else
		{
			connectionString = ConnectionString.GetReadConnectionString();
		}


		string sqlCommand = @"
SELECT  
	u.SiteGuid, 
	ls.LetterInfoGuid, 
	ls.UserGuid, 
	u.Email, 
	ls.BeginUTC, 
	ls.UseHtml 
FROM 
	mp_LetterSubscriber ls 
JOIN 
	mp_Users u 
ON 
	u.UserGuid = ls.UserGuid;";

		DataSet ds = CommandHelper.ExecuteDataset(
			connectionString,
			sqlCommand.ToString());

		DataTable dataTable = ds.Tables[0];

		foreach (DataRow row in dataTable.Rows)
		{

			DBLetterSubscription.Create(
				Guid.NewGuid(),
				new Guid(row["SiteGuid"].ToString()),
				new Guid(row["LetterInfoGuid"].ToString()),
				new Guid(row["UserGuid"].ToString()),
				row["Email"].ToString().ToLower(),
				true,
				new Guid("00000000-0000-0000-0000-000000000000"),
				Convert.ToDateTime(row["BeginUTC"]),
				Convert.ToBoolean(row["UseHtml"]));

		}

	}

	public static void DatabaseHelperDoVersion2230PostUpgradeTasks(
		string overrideConnectionInfo)
	{
		string connectionString;
		if (
			(overrideConnectionInfo != null)
			&& (overrideConnectionInfo.Length > 0)
		  )
		{
			connectionString = overrideConnectionInfo;
		}
		else
		{
			connectionString = ConnectionString.GetWriteConnectionString();
		}

		DataTable dataTable = DatabaseHelperGetTable(
			connectionString,
			"mp_ModuleDefinitions",
			" where Guid is null ");

		// UPDATE mp_ModuleDefinitions SET [Guid] = newid() 
		// WHERE [Guid] IS NULL
		foreach (DataRow row in dataTable.Rows)
		{
			DatabaseHelperUpdateTableField(
				"mp_ModuleDefinitions",
				"ModuleDefID",
				row["ModuleDefID"].ToString(),
				"guid",
				Guid.NewGuid().ToString(),
				" AND Guid is null ");

		}

		string sqlCommand = @"
UPDATE mp_ModuleDefinitionSettings 
SET FeatureGuid = (SELECT Guid 
FROM mp_ModuleDefinitions 
WHERE mp_ModuleDefinitions.ModuleDefID 
 = mp_ModuleDefinitionSettings.ModuleDefID LIMIT 1)
;";

		DatabaseHelperRunScript(sqlCommand.ToString(), overrideConnectionInfo);

		DatabaseHelperRunScript(
			"ALTER TABLE `mp_ModuleDefinitions` CHANGE `Guid` `Guid` CHAR(36)  NOT NULL;",
			overrideConnectionInfo);

		DatabaseHelperRunScript(
			"ALTER TABLE `mp_ModuleDefinitionSettings` CHANGE `FeatureGuid` `FeatureGuid` CHAR(36)  NOT NULL;",
			overrideConnectionInfo);


	}


	/// <summary>
	/// Runs tasks after Upgrade scripts
	/// </summary>
	/// <param name="version"></param>
	/// <param name="overrideConnectionString"></param>
	/// <returns>True if tasks for versions completed successfully, false if they did not.</returns>
	public static bool RunPostUpgradeTask(Version version, string overrideConnectionString)
	{
		var connectionString = ConnectionString.GetWriteConnectionString();
		if (!string.IsNullOrWhiteSpace(overrideConnectionString))
		{
			connectionString = overrideConnectionString;
		}

		bool result = true;
		DataTable dataTable = new();
		string sqlCommand;
		bool localResult;
		switch (version)
		{
			case var _ when version == new Version(2, 2, 3, 0):
				dataTable = DatabaseHelperGetTable(connectionString, "mp_ModuleDefinitions", " where Guid is null ");
				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField("mp_ModuleDefinitions", "ModuleDefID", row["ModuleDefID"].ToString(), "guid", Guid.NewGuid().ToString(), " AND Guid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}

				sqlCommand = @"
UPDATE mp_ModuleDefinitionSettings
SET FeatureGuid = (
	SELECT Guid 
	FROM mp_ModuleDefinitions 
	WHERE mp_ModuleDefinitions.ModuleDefID = mp_ModuleDefinitionSettings.ModuleDefID 
	LIMIT 1
);";
				localResult = DatabaseHelperRunScript(sqlCommand.ToString(), connectionString);
				if (!localResult)
				{
					result = localResult;
				}

				localResult = DatabaseHelperRunScript("ALTER TABLE `mp_ModuleDefinitions` CHANGE `Guid` `Guid` CHAR(36) NOT NULL;", connectionString);
				if (!localResult)
				{
					result = localResult;
				}

				localResult = DatabaseHelperRunScript("ALTER TABLE `mp_ModuleDefinitionSettings` CHANGE `FeatureGuid` `FeatureGuid` CHAR(36) NOT NULL;", connectionString);
				if (!localResult)
				{
					result = localResult;
				}
				return result;
			case var _ when version == new Version(2, 2, 3, 4):
				dataTable = DatabaseHelperGetTable(connectionString, "mp_Pages", " where PageGuid is null ");
				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField("mp_Pages", "PageID", row["PageID"].ToString(), "PageGuid", Guid.NewGuid().ToString(), " and PageGuid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}
				return result;
			case var _ when version == new Version(2, 2, 4, 7):
				dataTable = DatabaseHelperGetTable(connectionString, "mp_FriendlyUrls", " where ItemGuid is null ");
				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField("mp_FriendlyUrls", "UrlID", row["UrlID"].ToString(), "ItemGuid", Guid.NewGuid().ToString(), " and ItemGuid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}

				dataTable = DatabaseHelperGetTable(connectionString, "mp_Modules", " where Guid is null ");
				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField("mp_Modules", "ModuleID", row["ModuleID"].ToString(), "Guid", Guid.NewGuid().ToString(), " and Guid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}

				dataTable = DatabaseHelperGetTable(connectionString, "mp_Roles", " where RoleGuid is null ");
				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField("mp_Roles", "RoleID", row["RoleID"].ToString(), "RoleGuid", Guid.NewGuid().ToString(), " and RoleGuid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}

				dataTable = DatabaseHelperGetTable(connectionString, "mp_ModuleSettings", " where SettingGuid is null ");
				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField("mp_ModuleSettings", "ID", row["ID"].ToString(), "SettingGuid", Guid.NewGuid().ToString(), " and SettingGuid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}

				dataTable = DatabaseHelperGetTable(connectionString, "mp_Blogs", " where BlogGuid is null ");
				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField("mp_Blogs", "ItemID", row["ItemID"].ToString(), "BlogGuid", Guid.NewGuid().ToString(), " and BlogGuid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}

				dataTable = DatabaseHelperGetTable(connectionString, "mp_CalendarEvents", " where ItemGuid is null ");
				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField("mp_CalendarEvents", "ItemID", row["ItemID"].ToString(), "ItemGuid", Guid.NewGuid().ToString(), " and ItemGuid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}

				dataTable = DatabaseHelperGetTable(connectionString, "mp_GalleryImages", " where ItemGuid is null ");
				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField("mp_GalleryImages", "ItemID", row["ItemID"].ToString(), "ItemGuid", Guid.NewGuid().ToString(), " and ItemGuid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}

				dataTable = DatabaseHelperGetTable(connectionString, "mp_HtmlContent", " where ItemGuid is null ");
				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField("mp_HtmlContent", "ItemID", row["ItemID"].ToString(), "ItemGuid", Guid.NewGuid().ToString(), " and ItemGuid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}

				dataTable = DatabaseHelperGetTable(connectionString, "mp_Links", " where ItemGuid is null ");
				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField("mp_Links", "ItemID", row["ItemID"].ToString(), "ItemGuid", Guid.NewGuid().ToString(), " and ItemGuid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}

				dataTable = DatabaseHelperGetTable(connectionString, "mp_SharedFileFolders", " where FolderGuid is null ");
				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField("mp_SharedFileFolders", "FolderID", row["FolderID"].ToString(), "FolderGuid", Guid.NewGuid().ToString(), " and FolderGuid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}

				dataTable = DatabaseHelperGetTable(connectionString, "mp_SharedFiles", " where ItemGuid is null ");
				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField("mp_SharedFiles", "ItemID", row["ItemID"].ToString(), "ItemGuid", Guid.NewGuid().ToString(), " and ItemGuid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}
				return result;

			case var _ when version == new Version(2, 2, 5, 3):
				dataTable = DatabaseHelperGetTable(connectionString, "mp_RssFeeds", " where ItemGuid is null ");
				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField("mp_RssFeeds", "ItemID", row["ItemID"].ToString(), "ItemGuid", Guid.NewGuid().ToString(), " and ItemGuid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}
				return result;

			case var _ when version == new Version(2, 3, 2, 0):
				sqlCommand = @"
SELECT  
	u.SiteGuid 
	,ls.LetterInfoGuid
	,ls.UserGuid
	,u.Email
	,ls.BeginUTC
	,ls.UseHtml
FROM mp_LetterSubscriber ls 
JOIN mp_Users u ON u.UserGuid = ls.UserGuid;";

				DataSet ds = CommandHelper.ExecuteDataset(connectionString, sqlCommand.ToString());
				dataTable = ds.Tables[0];
				foreach (DataRow row in dataTable.Rows)
				{
					DBLetterSubscription.Create(
						Guid.NewGuid(),
						new Guid(row["SiteGuid"].ToString()),
						new Guid(row["LetterInfoGuid"].ToString()),
						new Guid(row["UserGuid"].ToString()),
						row["Email"].ToString().ToLower(),
						true,
						new Guid("00000000-0000-0000-0000-000000000000"),
						Convert.ToDateTime(row["BeginUTC"]),
						Convert.ToBoolean(row["UseHtml"]));
				}
				break;
			default:
				return false;
		}

		return false;
	}

	public static bool RunFeaturePostUpgradeTask(Guid featureGuid, Version version, string overrideConnectionString)
	{
		return false;
	}

	public static bool DatabaseHelperSitesTableExists()
	{
		bool result = false;
		//return DatabaseHelper_TableExists("`mp_Sites`");
		try
		{
			using (IDataReader reader = DBSiteSettings.GetSiteList())
			{
				if (reader.Read())
				{
					reader.Close();
				}
			}
			// no error yet it must exist
			result = true;
		}
		catch { }

		return result;
	}

	public static bool DatabaseHelperTableExists(string tableName)
	{
		using MySqlConnection connection = new(ConnectionString.GetWriteConnectionString());
		string[] restrictions = new string[4];
		restrictions[2] = tableName;
		connection.Open();
		DataTable table = connection.GetSchema("Tables", restrictions);
		connection.Close();
		if (table != null)
		{
			return (table.Rows.Count > 0);
		}

		return false;
	}
	#endregion
}