using Mono.Data.Sqlite;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace mojoPortal.Data;

public static class DBPortal
{
	public static string DBPlatform()
	{
		return "SQLite";
	}

	public static void EnsureDatabase()
	{ }

	private static string GetConnectionString()
	{
		var connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];

		if (connectionString == "defaultdblocation")
		{
			FileInfo theDb = new FileInfo(System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/mojo.db.config"));
			FileInfo seedDb = new FileInfo(System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/mojo-seed.db.config"));

			if (!theDb.Exists && seedDb.Exists && Convert.ToBoolean(ConfigurationManager.AppSettings["TryToCopySQLiteSeedDatabase"].ToString()))
			{
				seedDb.CopyTo("~/Data/sqlitedb/mojo.db.config");
			}

			connectionString = "version=3,URI=file:"
				+ System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/mojo.db.config");
		}

		return connectionString;

	}


	#region Versioning and Upgrade Helpers

	#region Schema Table Methods

	public static IDataReader DatabaseHelperGetApplicationId(string applicationName)
	{
		var sqlCommand = $"""
			SELECT *
			FROM mp_SchemaVersion
			WHERE LOWER(ApplicationName) = :ApplicationName;
			""";

		var sqlParams = new SqliteParameter[]
		{
			new(":ApplicationName", DbType.String, 255)
			{
				Direction = ParameterDirection.Input,
				Value = applicationName.ToLower()
			}
		};

		return SqliteHelper.ExecuteReader(
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
		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("INSERT INTO mp_SchemaVersion (");
		sqlCommand.Append("ApplicationID, ");
		sqlCommand.Append("ApplicationName, ");
		sqlCommand.Append("Major, ");
		sqlCommand.Append("Minor, ");
		sqlCommand.Append("Build, ");
		sqlCommand.Append("Revision )");

		sqlCommand.Append(" VALUES (");
		sqlCommand.Append(":ApplicationID, ");
		sqlCommand.Append(":ApplicationName, ");
		sqlCommand.Append(":Major, ");
		sqlCommand.Append(":Minor, ");
		sqlCommand.Append(":Build, ");
		sqlCommand.Append(":Revision );");

		SqliteParameter[] arParams = new SqliteParameter[6];

		arParams[0] = new SqliteParameter(":ApplicationID", DbType.String, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = applicationId.ToString();

		arParams[1] = new SqliteParameter(":ApplicationName", DbType.String, 255);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = applicationName;

		arParams[2] = new SqliteParameter(":Major", DbType.Int32);
		arParams[2].Direction = ParameterDirection.Input;
		arParams[2].Value = major;

		arParams[3] = new SqliteParameter(":Minor", DbType.Int32);
		arParams[3].Direction = ParameterDirection.Input;
		arParams[3].Value = minor;

		arParams[4] = new SqliteParameter(":Build", DbType.Int32);
		arParams[4].Direction = ParameterDirection.Input;
		arParams[4].Value = build;

		arParams[5] = new SqliteParameter(":Revision", DbType.Int32);
		arParams[5].Direction = ParameterDirection.Input;
		arParams[5].Value = revision;

		int rowsAffected = SqliteHelper.ExecuteNonQuery(
			GetConnectionString(),
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
		sqlCommand.Append("ApplicationName = :ApplicationName, ");
		sqlCommand.Append("Major = :Major, ");
		sqlCommand.Append("Minor = :Minor, ");
		sqlCommand.Append("Build = :Build, ");
		sqlCommand.Append("Revision = :Revision ");

		sqlCommand.Append("WHERE  ");
		sqlCommand.Append("ApplicationID = :ApplicationID ;");

		SqliteParameter[] arParams = new SqliteParameter[6];

		arParams[0] = new SqliteParameter(":ApplicationID", DbType.String, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = applicationId.ToString();

		arParams[1] = new SqliteParameter(":ApplicationName", DbType.String, 255);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = applicationName;

		arParams[2] = new SqliteParameter(":Major", DbType.Int32);
		arParams[2].Direction = ParameterDirection.Input;
		arParams[2].Value = major;

		arParams[3] = new SqliteParameter(":Minor", DbType.Int32);
		arParams[3].Direction = ParameterDirection.Input;
		arParams[3].Value = minor;

		arParams[4] = new SqliteParameter(":Build", DbType.Int32);
		arParams[4].Direction = ParameterDirection.Input;
		arParams[4].Value = build;

		arParams[5] = new SqliteParameter(":Revision", DbType.Int32);
		arParams[5].Direction = ParameterDirection.Input;
		arParams[5].Value = revision;


		int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
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
		sqlCommand.Append("ApplicationID = :ApplicationID ;");

		SqliteParameter[] arParams = new SqliteParameter[1];

		arParams[0] = new SqliteParameter(":ApplicationID", DbType.String, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = applicationId.ToString();

		return SqliteHelper.ExecuteReader(
			GetConnectionString(),
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

		return SqliteHelper.ExecuteReader(
			GetConnectionString(),
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
		sqlCommand.Append(":ApplicationID, ");
		sqlCommand.Append(":ScriptFile, ");
		sqlCommand.Append(":RunTime, ");
		sqlCommand.Append(":ErrorOccurred, ");
		sqlCommand.Append(":ErrorMessage, ");
		sqlCommand.Append(":ScriptBody );");

		sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

		SqliteParameter[] arParams = new SqliteParameter[6];

		arParams[0] = new SqliteParameter(":ApplicationID", DbType.String, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = applicationId.ToString();

		arParams[1] = new SqliteParameter(":ScriptFile", DbType.String, 255);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = scriptFile;

		arParams[2] = new SqliteParameter(":RunTime", DbType.DateTime);
		arParams[2].Direction = ParameterDirection.Input;
		arParams[2].Value = runTime;

		arParams[3] = new SqliteParameter(":ErrorOccurred", DbType.Int32);
		arParams[3].Direction = ParameterDirection.Input;
		arParams[3].Value = intErrorOccurred;

		arParams[4] = new SqliteParameter(":ErrorMessage", DbType.Object);
		arParams[4].Direction = ParameterDirection.Input;
		arParams[4].Value = errorMessage;

		arParams[5] = new SqliteParameter(":ScriptBody", DbType.Object);
		arParams[5].Direction = ParameterDirection.Input;
		arParams[5].Value = scriptBody;


		int newID = 0;
		newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(GetConnectionString(), sqlCommand.ToString(), arParams).ToString());
		return newID;
	}

	public static bool SchemaScriptHistoryExists(Guid applicationId, string scriptFile)
	{
		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("SELECT Count(*) ");
		sqlCommand.Append("FROM	mp_SchemaScriptHistory ");
		sqlCommand.Append("WHERE ");
		sqlCommand.Append("ApplicationID = :ApplicationID ");
		sqlCommand.Append("AND ScriptFile = :ScriptFile ");

		sqlCommand.Append(" ;");

		SqliteParameter[] arParams = new SqliteParameter[2];

		arParams[0] = new SqliteParameter(":ApplicationID", DbType.String, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = applicationId.ToString();

		arParams[1] = new SqliteParameter(":ScriptFile", DbType.String, 255);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = scriptFile;

		int count = Convert.ToInt32(SqliteHelper.ExecuteScalar(
			GetConnectionString(),
			sqlCommand.ToString(),
			arParams));

		return count > 0;

	}

	#endregion

	#endregion


	#region DatabaseHelper

	public static DbException DatabaseHelperGetConnectionError(string overrideConnectionInfo)
	{
		DbException exception = null;
		SqliteConnection connection;

		if (
			overrideConnectionInfo != null &&
			overrideConnectionInfo.Length > 0)
		{
			connection = new SqliteConnection(overrideConnectionInfo);
		}
		else
		{
			connection = new SqliteConnection(GetConnectionString());
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
		bool result = false;

		SqliteConnection connection;

		if (
			overrideConnectionInfo != null &&
			overrideConnectionInfo.Length > 0)
		{
			connection = new SqliteConnection(overrideConnectionInfo);
		}
		else
		{
			connection = new SqliteConnection(GetConnectionString());
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
		// Make sure we can create, alter and drop tables

		var sqlCommand = """
			CREATE TABLE `mp_Testdb` (
			    `FooID` INTEGER PRIMARY KEY,
			    `Foo` varchar(255) NOT NULL default ''
			);
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

		sqlCommand = "ALTER TABLE mp_Testdb ADD COLUMN `MoreFoo` varchar(255) NULL;";

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

		sqlCommand = "DROP TABLE mp_Testdb;";

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

		return result;
	}

	public static bool DatabaseHelperRunScript(
		FileInfo scriptFile,
		string overrideConnectionInfo)
	{
		if (scriptFile == null) return false;

		string script = File.ReadAllText(scriptFile.FullName);

		if (script == null || script.Length == 0) return true;

		return DatabaseHelperRunScript(script, overrideConnectionInfo);

	}

	public static bool DatabaseHelperRunScript(string script, string overrideConnectionInfo)
	{
		if (script == null || script.Length == 0) return true;

		bool result = false;
		SqliteConnection connection;

		if (
			overrideConnectionInfo != null &&
			overrideConnectionInfo.Length > 0)
		{
			connection = new SqliteConnection(overrideConnectionInfo);
		}
		else
		{
			connection = new SqliteConnection(GetConnectionString());
		}

		connection.Open();

		var transaction = connection.BeginTransaction();

		try
		{
			SqliteHelper.ExecuteNonQuery(connection, script, null);
			transaction.Commit();
			result = true;
		}
		finally
		{
			connection.Close();
		}

		return result;
	}

	public static IDataReader DatabaseHelperGetReader(string connectionString, string query)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			connectionString = GetConnectionString();
		}

		return SqliteHelper.ExecuteReader(
			connectionString,
			query);

	}

	public static int DatabaseHelperExecteNonQuery(
		string connectionString,
		string query
		)
	{
		if (string.IsNullOrEmpty(connectionString)) { connectionString = GetConnectionString(); }

		int rowsAffected = SqliteHelper.ExecuteNonQuery(
			connectionString,
			query);

		return rowsAffected;

	}

	public static bool DatabaseHelperSitesTableExists()
	{
		return DatabaseHelperTableExists("mp_Sites");
	}

	public static bool DatabaseHelperTableExists(string tableName)
	{
		var connection = new SqliteConnection(GetConnectionString());
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
