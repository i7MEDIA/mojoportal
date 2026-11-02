using log4net;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;

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
		var sqlCommand = $"""
			SELECT *
			FROM mp_SchemaVersion
			WHERE LOWER(ApplicationName) = ?ApplicationName;
			""";

		var sqlParams = new MySqlParameter[]
		{
			new("?ApplicationName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = applicationName.ToLower()
			}
		};

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			sqlParams);
	}


	public static bool SchemaVersionAddSchemaVersion(
		Guid applicationId,
		string applicationName,
		int major,
		int minor,
		int build,
		int revision)
	{
		var sqlCommand = new StringBuilder();
		sqlCommand.Append("INSERT INTO mp_SchemaVersion (");
		sqlCommand.Append("ApplicationID, ");
		sqlCommand.Append("ApplicationName, ");
		sqlCommand.Append("Major, ");
		sqlCommand.Append("Minor, ");
		sqlCommand.Append("Build, ");
		sqlCommand.Append("Revision )");

		sqlCommand.Append(" VALUES (");
		sqlCommand.Append("?ApplicationID, ");
		sqlCommand.Append("?ApplicationName, ");
		sqlCommand.Append("?Major, ");
		sqlCommand.Append("?Minor, ");
		sqlCommand.Append("?Build, ");
		sqlCommand.Append("?Revision );");


		MySqlParameter[] arParams = new MySqlParameter[6];

		arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = applicationId.ToString();

		arParams[1] = new MySqlParameter("?ApplicationName", MySqlDbType.VarChar, 255);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = applicationName;

		arParams[2] = new MySqlParameter("?Major", MySqlDbType.Int32);
		arParams[2].Direction = ParameterDirection.Input;
		arParams[2].Value = major;

		arParams[3] = new MySqlParameter("?Minor", MySqlDbType.Int32);
		arParams[3].Direction = ParameterDirection.Input;
		arParams[3].Value = minor;

		arParams[4] = new MySqlParameter("?Build", MySqlDbType.Int32);
		arParams[4].Direction = ParameterDirection.Input;
		arParams[4].Value = build;

		arParams[5] = new MySqlParameter("?Revision", MySqlDbType.Int32);
		arParams[5].Direction = ParameterDirection.Input;
		arParams[5].Value = revision;

		int rowsAffected = MySqlHelper.ExecuteNonQuery(
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
		StringBuilder sqlCommand = new StringBuilder();

		sqlCommand.Append("UPDATE mp_SchemaVersion ");
		sqlCommand.Append("SET  ");
		sqlCommand.Append("ApplicationName = ?ApplicationName, ");
		sqlCommand.Append("Major = ?Major, ");
		sqlCommand.Append("Minor = ?Minor, ");
		sqlCommand.Append("Build = ?Build, ");
		sqlCommand.Append("Revision = ?Revision ");

		sqlCommand.Append("WHERE  ");
		sqlCommand.Append("ApplicationID = ?ApplicationID ;");

		MySqlParameter[] arParams = new MySqlParameter[6];

		arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = applicationId.ToString();

		arParams[1] = new MySqlParameter("?ApplicationName", MySqlDbType.VarChar, 255);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = applicationName;

		arParams[2] = new MySqlParameter("?Major", MySqlDbType.Int32);
		arParams[2].Direction = ParameterDirection.Input;
		arParams[2].Value = major;

		arParams[3] = new MySqlParameter("?Minor", MySqlDbType.Int32);
		arParams[3].Direction = ParameterDirection.Input;
		arParams[3].Value = minor;

		arParams[4] = new MySqlParameter("?Build", MySqlDbType.Int32);
		arParams[4].Direction = ParameterDirection.Input;
		arParams[4].Value = build;

		arParams[5] = new MySqlParameter("?Revision", MySqlDbType.Int32);
		arParams[5].Direction = ParameterDirection.Input;
		arParams[5].Value = revision;

		int rowsAffected = MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;
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
		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("SELECT  * ");
		sqlCommand.Append("FROM	mp_SchemaVersion ");
		sqlCommand.Append("WHERE ");
		sqlCommand.Append("ApplicationID = ?ApplicationID ;");

		MySqlParameter[] arParams = new MySqlParameter[1];

		arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = applicationId.ToString();

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader SchemaVersionGetNonCore()
	{
		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("SELECT  * ");
		sqlCommand.Append("FROM	mp_SchemaVersion ");
		sqlCommand.Append("WHERE ");
		sqlCommand.Append("ApplicationID <> '077E4857-F583-488E-836E-34A4B04BE855' ");
		sqlCommand.Append("ORDER BY ApplicationName ");
		sqlCommand.Append(";");

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			null);
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

		var intErrorOccurred = 0;
		
		if (errorOccurred)
		{
			intErrorOccurred = 1;
		}

		#endregion

		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("INSERT INTO mp_SchemaScriptHistory (");
		sqlCommand.Append("ApplicationID, ");
		sqlCommand.Append("ScriptFile, ");
		sqlCommand.Append("RunTime, ");
		sqlCommand.Append("ErrorOccurred, ");
		sqlCommand.Append("ErrorMessage, ");
		sqlCommand.Append("ScriptBody )");

		sqlCommand.Append(" VALUES (");
		sqlCommand.Append("?ApplicationID, ");
		sqlCommand.Append("?ScriptFile, ");
		sqlCommand.Append("?RunTime, ");
		sqlCommand.Append("?ErrorOccurred, ");
		sqlCommand.Append("?ErrorMessage, ");
		sqlCommand.Append("?ScriptBody );");

		sqlCommand.Append("SELECT LAST_INSERT_ID();");

		MySqlParameter[] arParams = new MySqlParameter[6];

		arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = applicationId.ToString();

		arParams[1] = new MySqlParameter("?ScriptFile", MySqlDbType.VarChar, 255);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = scriptFile;

		arParams[2] = new MySqlParameter("?RunTime", MySqlDbType.DateTime);
		arParams[2].Direction = ParameterDirection.Input;
		arParams[2].Value = runTime;

		arParams[3] = new MySqlParameter("?ErrorOccurred", MySqlDbType.Int32);
		arParams[3].Direction = ParameterDirection.Input;
		arParams[3].Value = intErrorOccurred;

		arParams[4] = new MySqlParameter("?ErrorMessage", MySqlDbType.Text);
		arParams[4].Direction = ParameterDirection.Input;
		arParams[4].Value = errorMessage;

		arParams[5] = new MySqlParameter("?ScriptBody", MySqlDbType.Text);
		arParams[5].Direction = ParameterDirection.Input;
		arParams[5].Value = scriptBody;


		int newID = 0;
		newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams).ToString());
		return newID;

	}

	public static bool SchemaScriptHistoryExists(Guid applicationId, string scriptFile)
	{
		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("SELECT Count(*) ");
		sqlCommand.Append("FROM	mp_SchemaScriptHistory ");
		sqlCommand.Append("WHERE ");
		sqlCommand.Append("ApplicationID = ?ApplicationID ");
		sqlCommand.Append("AND ScriptFile = ?ScriptFile ");

		sqlCommand.Append(" ;");

		MySqlParameter[] arParams = new MySqlParameter[2];

		arParams[0] = new MySqlParameter("?ApplicationID", MySqlDbType.VarChar, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = applicationId.ToString();

		arParams[1] = new MySqlParameter("?ScriptFile", MySqlDbType.VarChar, 255);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = scriptFile;

		int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
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
		DataTable dataTable = new DataTable();

		using (reader)
		{
			DataTable schemaTable = reader.GetSchemaTable();
			DataColumn column;
			DataRow row;
			ArrayList arrayList = new ArrayList();

			for (int i = 0; i < schemaTable.Rows.Count; i++)
			{

				column = new DataColumn();

				if (!dataTable.Columns.Contains(schemaTable.Rows[i]["ColumnName"].ToString()))
				{

					column.ColumnName = schemaTable.Rows[i]["ColumnName"].ToString();
					// we don't always want to enforce constrainnts, it may be fine to have duplicates in a query even if the underlying table has a unique constraint
					//column.Unique = Convert.ToBoolean(schemaTable.Rows[i]["IsUnique"]);
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
					row[(string)arrayList[i]] = reader[(string)arrayList[i]];
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
			overrideConnectionInfo != null &&
			overrideConnectionInfo.Length > 0)
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
			{
				connection.Close();
			}
		}

		return exception;
	}

	public static bool DatabaseHelperCanAccessDatabase(string overrideConnectionInfo)
	{
		var result = false;
		MySqlConnection connection;

		if (
			overrideConnectionInfo != null &&
			overrideConnectionInfo.Length > 0)
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
			result = connection.State == ConnectionState.Open;
		}
		catch { }
		finally
		{
			if (connection.State == ConnectionState.Open)
			{
				connection.Close();
			}
		}

		return result;
	}

	public static bool DatabaseHelperCanAccessDatabase()
	{
		return DatabaseHelperCanAccessDatabase(null);
	}

	public static bool DatabaseHelperCanAlterSchema(string overrideConnectionInfo)
	{
		var result = true;

		var sqlCommand = $"""
			CREATE TABLE `mp_Testdb` (
				`FooID` int(11) NOT NULL auto_increment,
				`Foo` varchar(255) NOT NULL default '',
				PRIMARY KEY (`FooID`)
			) ENGINE=MyISAM;
			""";

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

	public static bool DatabaseHelperRunScript(
		FileInfo scriptFile,
		string overrideConnectionInfo)
	{
		if (scriptFile == null)
		{
			return false;
		}

		string script = File.ReadAllText(scriptFile.FullName);

		if (script == null || script.Length == 0)
		{
			return true;
		}

		return DatabaseHelperRunScript(script, overrideConnectionInfo);

	}

	public static bool DatabaseHelperRunScript(string script, string overrideConnectionInfo)
	{
		if (script == null || script.Trim().Length == 0)
		{
			return true;
		}

		bool result = false;
		MySqlConnection connection;

		if (
			overrideConnectionInfo != null
			&& overrideConnectionInfo.Length > 0
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
			// this fixed the problems with mysql 5.1
			MySqlScript mySqlScript = new MySqlScript(connection, script);
			mySqlScript.Execute();

			//this worked in all versions of mysql prior to 5.1
			//MySqlHelper.ExecuteNonQuery(
			//    connection,
			//    script, 
			//    null);

			transaction.Commit();
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

	public static IDataReader DatabaseHelperGetReader(
		string connectionString,
		string query
		)
	{
		if (string.IsNullOrEmpty(connectionString)) { connectionString = ConnectionString.GetReadConnectionString(); }

		return MySqlHelper.ExecuteReader(
			connectionString,
			query);

	}

	public static int DatabaseHelperExecteNonQuery(
		string connectionString,
		string query
		)
	{
		if (string.IsNullOrEmpty(connectionString)) { connectionString = ConnectionString.GetWriteConnectionString(); }

		int rowsAffected = MySqlHelper.ExecuteNonQuery(
			connectionString,
			query);

		return rowsAffected;

	}


	public static bool DatabaseHelperSitesTableExists()
	{
		var result = false;

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
		using var connection = new MySqlConnection(ConnectionString.GetWriteConnectionString());
		var restrictions = new string[4];

		restrictions[2] = tableName;
		connection.Open();

		var table = connection.GetSchema("Tables", restrictions);

		connection.Close();

		if (table != null)
		{
			return table.Rows.Count > 0;
		}

		return false;
	}
	#endregion
}
