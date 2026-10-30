using log4net;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace mojoPortal.Data;

/// <summary>
/// This data Facade has all static methods and serves to abstract the underlying database
/// from the business layer.
/// </summary>
public static class DBPortal
{
	// Create a logger for use in this class
	private static readonly ILog log = LogManager.GetLogger(typeof(DBPortal));


	/// <summary>
	/// Getdbs the owner prefix.
	/// </summary>
	/// <returns></returns>
	private static string GetdbOwnerPrefix()
	{
		var ownerPrefix = "[dbo].";

		if (ConfigurationManager.AppSettings["MSSQLOwnerPrefix"] != null)
		{
			ownerPrefix = ConfigurationManager.AppSettings["MSSQLOwnerPrefix"];
		}

		return ownerPrefix;
	}

	public static void EnsureDatabase()
	{
		//logic added by Luis Silva 2012-06-13
		// only works if connection string has been configured and if you add this to user.config or Web.config:
		// <add key="TryToCreateMsSqlDatabase" value="true"/>
		var connection = new SqlConnection(ConnectionString.GetWriteConnectionString());
		var masterConnection = new SqlConnection();

		try
		{
			var master = new SqlConnectionStringBuilder
			{
				ConnectionString = connection.ConnectionString
			};

			master["server"] = connection.DataSource;
			master["database"] = "master";

			masterConnection = new SqlConnection(master.ConnectionString);

			var sql = new StringBuilder();

			sql.Append("IF not EXISTS (SELECT name FROM sys.databases WHERE name = @Name) CREATE DATABASE ");
			sql.Append(connection.Database);
			sql.Append(" else select 1");

			var command = new SqlCommand(sql.ToString(), masterConnection);

			command.Parameters.AddWithValue("@Name", connection.Database);
			masterConnection.Open();

			var res = command.ExecuteNonQuery();

			if (res == -1)
			{
				log.Info("Successfully created MS SQL Database");
			}
			else
			{
				log.Info("Failed to Create MS SQL Database");
			}

		}
		catch (Exception ex)
		{
			log.Error("Failed to Create MS SQL Database", ex);
		}
		finally
		{
			if (masterConnection.State == ConnectionState.Open)
			{
				masterConnection.Close();
			}
		}

	}

	/// <summary>
	/// Gets the database platform.
	/// </summary>
	/// <returns></returns>
	public static string DBPlatform()
	{
		return "MSSQL";
	}


	#region Versioning and Upgrade Helpers

	#region Schema Table Methods

	public static IDataReader DatabaseHelperGetApplicationId(string applicationName)
	{
		var sqlCommand = $"""
			SELECT *
			FROM [dbo].[mp_SchemaVersion]
			WHERE LOWER(ApplicationName) = @ApplicationName;
			""";

		var sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), sqlCommand, 1);
		sph.DefineSqlParameter("@ApplicationName", SqlDbType.NVarChar, ParameterDirection.Input, applicationName.ToLower());

		return sph.ExecuteReader();
	}

	/// <summary>
	/// Schemas the version_ add schema version.
	/// </summary>
	/// <param name="applicationID">The application Guid.</param>
	/// <param name="applicationName">Name of the application.</param>
	/// <param name="major">major</param>
	/// <param name="minor">minor</param>
	/// <param name="build">build</param>
	/// <param name="revision">revision</param>
	/// <returns></returns>
	public static bool SchemaVersionAddSchemaVersion(
	  Guid applicationId,
	  string applicationName,
	  int major,
	  int minor,
	  int build,
	  int revision)
	{

		SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SchemaVersion_Insert", 6);
		sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
		sph.DefineSqlParameter("@ApplicationName", SqlDbType.NVarChar, ParameterDirection.Input, applicationName);
		sph.DefineSqlParameter("@Major", SqlDbType.Int, ParameterDirection.Input, major);
		sph.DefineSqlParameter("@Minor", SqlDbType.Int, ParameterDirection.Input, minor);
		sph.DefineSqlParameter("@Build", SqlDbType.Int, ParameterDirection.Input, build);
		sph.DefineSqlParameter("@Revision", SqlDbType.Int, ParameterDirection.Input, revision);
		int rowsAffected = Convert.ToInt32(sph.ExecuteNonQuery());
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
		SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SchemaVersion_Update", 6);
		sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
		sph.DefineSqlParameter("@ApplicationName", SqlDbType.NVarChar, ParameterDirection.Input, applicationName);
		sph.DefineSqlParameter("@Major", SqlDbType.Int, ParameterDirection.Input, major);
		sph.DefineSqlParameter("@Minor", SqlDbType.Int, ParameterDirection.Input, minor);
		sph.DefineSqlParameter("@Build", SqlDbType.Int, ParameterDirection.Input, build);
		sph.DefineSqlParameter("@Revision", SqlDbType.Int, ParameterDirection.Input, revision);
		int rowsAffected = sph.ExecuteNonQuery();
		return rowsAffected > -1;
	}

	public static bool SchemaVersionExists(Guid applicationId)
	{
		var result = false;

		using IDataReader reader = SchemaVersionGetSchemaVersion(applicationId);

		if (reader.Read())
		{
			result = true;
		}

		return result;
	}

	public static IDataReader SchemaVersionGetSchemaVersion(Guid applicationId)
	{
		SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SchemaVersion_SelectOne", 1);
		sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
		return sph.ExecuteReader();
	}

	public static IDataReader SchemaVersionGetNonCore()
	{
		SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SchemaVersion_SelectNonCore", 0);
		return sph.ExecuteReader();
	}

	public static int SchemaScriptHistoryAddSchemaScriptHistory(
		Guid applicationId,
		string scriptFile,
		DateTime runTime,
		bool errorOccurred,
		string errorMessage,
		string scriptBody)
	{
		SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SchemaScriptHistory_Insert", 6);
		sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
		sph.DefineSqlParameter("@ScriptFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, scriptFile);
		sph.DefineSqlParameter("@RunTime", SqlDbType.DateTime, ParameterDirection.Input, runTime);
		sph.DefineSqlParameter("@ErrorOccurred", SqlDbType.Bit, ParameterDirection.Input, errorOccurred);
		sph.DefineSqlParameter("@ErrorMessage", SqlDbType.NVarChar, -1, ParameterDirection.Input, errorMessage);
		sph.DefineSqlParameter("@ScriptBody", SqlDbType.NVarChar, -1, ParameterDirection.Input, scriptBody);
		int newID = Convert.ToInt32(sph.ExecuteScalar());
		return newID;
	}

	public static bool SchemaScriptHistoryExists(Guid applicationId, string scriptFile)
	{
		SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SchemaScriptHistory_Exists", 2);
		sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
		sph.DefineSqlParameter("@ScriptFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, scriptFile);
		int count = Convert.ToInt32(sph.ExecuteScalar());
		return count > 0;
	}

	#endregion

	#endregion


	#region DatabaseHelper

	public static DataTable GetTableFromDataReader(IDataReader reader)
	{
		var dataTable = new DataTable();
		DataColumn column;
		DataRow row;
		var arrayList = new ArrayList();

		try
		{
			var schemaTable = reader.GetSchemaTable();

			for (var i = 0; i < schemaTable.Rows.Count; i++)
			{
				column = new DataColumn();

				if (!dataTable.Columns.Contains(schemaTable.Rows[i]["ColumnName"].ToString()))
				{

					column.ColumnName = schemaTable.Rows[i]["ColumnName"].ToString();
					// we don't always want to enforce constraints, it may be fine to have duplicates in a query even if the underlying table has a unique constraint
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

				for (var i = 0; i < arrayList.Count; i++)
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

	public static bool DatabaseHelperCanAccessDatabase(string overrideConnectionInfo)
	{
		// TODO: catch specific exceptions
		var result = false;
		SqlConnection connection;

		if (
			overrideConnectionInfo != null &&
			overrideConnectionInfo.Length > 0)
		{
			connection = new SqlConnection(overrideConnectionInfo);
		}
		else
		{
			connection = new SqlConnection(ConnectionString.GetReadConnectionString());
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

	public static DbException DatabaseHelperGetConnectionError(string overrideConnectionInfo)
	{
		DbException exception = null;
		SqlConnection connection;

		if (
			overrideConnectionInfo != null &&
			overrideConnectionInfo.Length > 0)
		{
			connection = new SqlConnection(overrideConnectionInfo);
		}
		else
		{
			connection = new SqlConnection(ConnectionString.GetReadConnectionString());
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

	public static bool DatabaseHelperCanAccessDatabase()
	{
		return DatabaseHelperCanAccessDatabase(null);
	}

	public static bool DatabaseHelperCanAlterSchema(string overrideConnectionInfo)
	{
		var result = true;

		var sqlCommand = """
			SET QUOTED_IDENTIFIER ON
			GO

			CREATE TABLE [dbo].[mp_Testdb](
				[FooID] [int] IDENTITY(1,1) NOT NULL,
				[Foo] [nvarchar](255) NOT NULL,
				CONSTRAINT [PK_mp_Testdb] PRIMARY KEY CLUSTERED ([FooID] ASC WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
			)
			GO
			""";

		try
		{
			DatabaseHelperRunScript(
				sqlCommand.ToString(),
				overrideConnectionInfo);
		}
		catch (DbException)
		{
			result = false;
		}
		catch (ArgumentException)
		{
			result = false;
		}

		sqlCommand = "ALTER TABLE [dbo].[mp_Testdb] ADD [MoreFoo] [nvarchar] (255) NULL GO";

		try
		{
			DatabaseHelperRunScript(
				sqlCommand.ToString(),
				overrideConnectionInfo);
		}
		catch (DbException)
		{
			result = false;
		}
		catch (ArgumentException)
		{
			result = false;
		}

		sqlCommand = "DROP TABLE [dbo].[mp_Testdb] GO";

		try
		{
			DatabaseHelperRunScript(sqlCommand.ToString(), overrideConnectionInfo);
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

	public static bool DatabaseHelperRunScript(FileInfo scriptFile, string overrideConnectionInfo)
	{
		if (scriptFile == null)
		{
			return false;
		}

		var script = File.ReadAllText(scriptFile.FullName);

		if (script == null || script.Length == 0)
		{
			return true;
		}

		return DatabaseHelperRunScript(script, overrideConnectionInfo);
	}

	public static bool DatabaseHelperRunScript(string script, string overrideConnectionInfo)
	{
		if (script == null || script.Length == 0)
		{
			return true;
		}

		var ownerPrefix = GetdbOwnerPrefix();

		if (ownerPrefix != "[dbo].")
		{
			script = script.Replace("[dbo].", ownerPrefix);
		}

		var result = false;
		SqlConnection connection;

		if (
			overrideConnectionInfo != null &&
			overrideConnectionInfo.Length > 0)
		{
			connection = new SqlConnection(overrideConnectionInfo);
		}
		else
		{
			connection = new SqlConnection(ConnectionString.GetWriteConnectionString());
		}

		var delimiter = new string[] { "GO\r\n" };

		script = script.Replace("GO", "GO\r\n");

		var arrSqlStatements = script.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

		connection.Open();

		var transaction = connection.BeginTransaction();
		var currentStatement = string.Empty;

		try
		{
			foreach (string sqlStatement in arrSqlStatements)
			{
				if (sqlStatement.Trim().Length > 0)
				{
					currentStatement = sqlStatement;
					SqlHelper.ExecuteNonQuery(
						transaction,
						CommandType.Text,
						sqlStatement,
						null);
				}
			}

			transaction.Commit();
			result = true;

		}
		catch (SqlException ex)
		{
			transaction.Rollback();
			log.Error("dbPortal.RunScript failed", ex);
			log.Info("last script statement was " + currentStatement);
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

		return SqlHelper.ExecuteReader(
			connectionString,
			CommandType.Text,
			query);

	}

	public static int DatabaseHelperExecteNonQuery(
		string connectionString,
		string query)
	{
		if (string.IsNullOrEmpty(connectionString)) { connectionString = ConnectionString.GetWriteConnectionString(); }

		var rowsAffected = SqlHelper.ExecuteNonQuery(
			connectionString,
			CommandType.Text,
			query);

		return rowsAffected;
	}

	public static bool DatabaseHelperSitesTableExists()
	{
		return DatabaseHelperTableExists("mp_Sites");
	}

	public static bool DatabaseHelperTableExists(string tableName)
	{
		using var connection = new SqlConnection(ConnectionString.GetReadConnectionString());
		var restrictions = new string[4];
		restrictions[2] = tableName;

		connection.Open();

		var table = connection.GetSchema("Tables", restrictions);

		if (table != null)
		{
			return table.Rows.Count > 0;
		}

		return false;
	}

	#endregion
}
