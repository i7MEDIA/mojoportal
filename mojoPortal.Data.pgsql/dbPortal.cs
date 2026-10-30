using log4net;
using Npgsql;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace mojoPortal.Data;

public static class DBPortal
{
	// Create a logger for use in this class
	private static readonly ILog log = LogManager.GetLogger(typeof(DBPortal));

	public static string DBPlatform()
	{
		return "pgsql";
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
			FROM mp_schemaversion
			WHERE applicationname ILIKE :applicationname;
			""";

		var sqlParams = new NpgsqlParameter[]
		{
			new(":applicationname", NpgsqlTypes.NpgsqlDbType.Varchar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = applicationName.ToLower()
			}
		};

		return NpgsqlHelper.ExecuteReader(
			ConnectionString.GetWriteConnectionString(),
			CommandType.Text,
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

		NpgsqlParameter[] arParams = new NpgsqlParameter[6];

		arParams[0] = new NpgsqlParameter(":applicationid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = applicationId.ToString();

		arParams[1] = new NpgsqlParameter(":applicationname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = applicationName;

		arParams[2] = new NpgsqlParameter(":major", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[2].Direction = ParameterDirection.Input;
		arParams[2].Value = major;

		arParams[3] = new NpgsqlParameter(":minor", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[3].Direction = ParameterDirection.Input;
		arParams[3].Value = minor;

		arParams[4] = new NpgsqlParameter(":build", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[4].Direction = ParameterDirection.Input;
		arParams[4].Value = build;

		arParams[5] = new NpgsqlParameter(":revision", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[5].Direction = ParameterDirection.Input;
		arParams[5].Value = revision;

		int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
			CommandType.StoredProcedure,
			"mp_schemaversion_insert(:applicationid,:applicationname,:major,:minor,:build,:revision)",
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
		NpgsqlParameter[] arParams = new NpgsqlParameter[6];

		arParams[0] = new NpgsqlParameter(":applicationid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = applicationId.ToString();

		arParams[1] = new NpgsqlParameter(":applicationname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = applicationName;

		arParams[2] = new NpgsqlParameter(":major", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[2].Direction = ParameterDirection.Input;
		arParams[2].Value = major;

		arParams[3] = new NpgsqlParameter(":minor", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[3].Direction = ParameterDirection.Input;
		arParams[3].Value = minor;

		arParams[4] = new NpgsqlParameter(":build", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[4].Direction = ParameterDirection.Input;
		arParams[4].Value = build;

		arParams[5] = new NpgsqlParameter(":revision", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[5].Direction = ParameterDirection.Input;
		arParams[5].Value = revision;

		int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
			CommandType.StoredProcedure,
			"mp_schemaversion_update(:applicationid,:applicationname,:major,:minor,:build,:revision)",
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
		NpgsqlParameter[] arParams = new NpgsqlParameter[1];

		arParams[0] = new NpgsqlParameter(":applicationid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = applicationId.ToString();

		return NpgsqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			CommandType.StoredProcedure,
			"mp_schemaversion_select_one(:applicationid)",
			arParams);
	}

	public static IDataReader SchemaVersionGetNonCore()
	{
		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("SELECT  * ");
		sqlCommand.Append("FROM	mp_schemaversion ");
		sqlCommand.Append("WHERE ");
		sqlCommand.Append("applicationid <> '077E4857-F583-488E-836E-34A4B04BE855' ");
		sqlCommand.Append("ORDER BY applicationname ");
		sqlCommand.Append(";");

		return NpgsqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			CommandType.Text,
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
		NpgsqlParameter[] arParams = new NpgsqlParameter[6];

		arParams[0] = new NpgsqlParameter(":applicationid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = applicationId.ToString();

		arParams[1] = new NpgsqlParameter(":scriptfile", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = scriptFile;

		arParams[2] = new NpgsqlParameter(":runtime", NpgsqlTypes.NpgsqlDbType.Date);
		arParams[2].Direction = ParameterDirection.Input;
		arParams[2].Value = runTime;

		arParams[3] = new NpgsqlParameter(":erroroccurred", NpgsqlTypes.NpgsqlDbType.Boolean);
		arParams[3].Direction = ParameterDirection.Input;
		arParams[3].Value = errorOccurred;

		arParams[4] = new NpgsqlParameter(":errormessage", NpgsqlTypes.NpgsqlDbType.Text);
		arParams[4].Direction = ParameterDirection.Input;
		arParams[4].Value = errorMessage;

		arParams[5] = new NpgsqlParameter(":scriptbody", NpgsqlTypes.NpgsqlDbType.Text);
		arParams[5].Direction = ParameterDirection.Input;
		arParams[5].Value = scriptBody;

		int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(ConnectionString.GetWriteConnectionString(),
			CommandType.StoredProcedure,
			"mp_schemascripthistory_insert(:applicationid,:scriptfile,:runtime,:erroroccurred,:errormessage,:scriptbody)",
			arParams));

		return newID;
	}

	public static bool SchemaScriptHistoryExists(Guid applicationId, string scriptFile)
	{
		NpgsqlParameter[] arParams = new NpgsqlParameter[2];

		arParams[0] = new NpgsqlParameter(":applicationid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = applicationId.ToString();

		arParams[1] = new NpgsqlParameter(":scriptfile", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = scriptFile;

		int count = 0;

		count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			CommandType.StoredProcedure,
			"mp_schemascripthistory_exists(:applicationid,:scriptfile)",
			arParams));

		return count > 0;
	}

	#endregion

	#endregion


	#region DatabaseHelper

	public static DataTable GetTableFromDataReader(IDataReader reader)
	{
		DataTable dataTable = new DataTable();

		try
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
		finally
		{
			reader.Close();
		}

		return dataTable;
	}

	public static DbException DatabaseHelperGetConnectionError(string overrideConnectionInfo)
	{
		DbException exception = null;
		NpgsqlConnection connection;

		if (
			overrideConnectionInfo != null &&
			overrideConnectionInfo.Length > 0)
		{
			connection = new NpgsqlConnection(overrideConnectionInfo);
		}
		else
		{
			connection = new NpgsqlConnection(ConnectionString.GetWriteConnectionString());
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
		NpgsqlConnection connection;

		if (
			overrideConnectionInfo != null &&
			overrideConnectionInfo.Length > 0)
		{
			connection = new NpgsqlConnection(overrideConnectionInfo);
		}
		else
		{
			connection = new NpgsqlConnection(ConnectionString.GetWriteConnectionString());
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
		bool result = true;
		// Make sure we can create, alter and drop tables

		StringBuilder sqlCommand = new StringBuilder();

		sqlCommand.Append("""
			CREATE SEQUENCE "mp_testdb_fooid_seq";
			CREATE TABLE "mp_testdb"(
				"categoryid" int4 NOT NULL DEFAULT nextval('"mp_testdb_fooid_seq"'::text),
				"fooid" int4 NOT NULL, "foo" varchar(255) NULL
			);
			""");

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

		sqlCommand = new StringBuilder();
		sqlCommand.Append("BEGIN; LOCK mp_testdb; ALTER TABLE mp_testdb ADD COLUMN morefoo character varying(255);  COMMIT;");

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

		sqlCommand = new StringBuilder();
		sqlCommand.Append("DROP TABLE \"mp_testdb\"; DROP SEQUENCE \"mp_testdb_fooid_seq\" ;");

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
		if (script == null || script.Length == 0)
		{
			return true;
		}

		bool result = false;
		NpgsqlConnection connection;

		if (
			overrideConnectionInfo != null &&
			overrideConnectionInfo.Length > 0)
		{
			connection = new NpgsqlConnection(overrideConnectionInfo);
		}
		else
		{
			connection = new NpgsqlConnection(ConnectionString.GetWriteConnectionString());
		}

		connection.Open();

		NpgsqlTransaction transaction = connection.BeginTransaction();

		try
		{
			NpgsqlHelper.ExecuteNonQuery(transaction, CommandType.Text, script);
			transaction.Commit();
			result = true;
		}
		catch (NpgsqlException ex)
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
		string query)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			connectionString = ConnectionString.GetReadConnectionString();
		}

		return NpgsqlHelper.ExecuteReader(
			connectionString,
			CommandType.Text,
			query);
	}

	public static int DatabaseHelperExecteNonQuery(
		string connectionString,
		string query)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			connectionString = ConnectionString.GetWriteConnectionString();
		}

		int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
			connectionString,
			CommandType.Text,
			query);

		return rowsAffected;
	}

	public static bool DatabaseHelperSitesTableExists()
	{
		return DatabaseHelperTableExists("mp_sites");
	}

	public static bool DatabaseHelperTableExists(string tableName)
	{
		var connection = new NpgsqlConnection(ConnectionString.GetWriteConnectionString());
		string[] restrictions = new string[4];
		restrictions[2] = tableName;
		connection.Open();
		DataTable table = connection.GetSchema("Tables", restrictions);
		connection.Close();

		if (table != null)
		{
			return table.Rows.Count > 0;
		}

		return false;
	}

	#endregion
}
