using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using log4net;
using Npgsql;
using NpgsqlTypes;

namespace mojoPortal.Data;

public static class DBPortal
{

	// Create a logger for use in this class
	private static readonly ILog log = LogManager.GetLogger(typeof(DBPortal));

	public static String DBPlatform()
	{
		return "pgsql";
	}

	//private static String GetConnectionString()
	//{
	//    return ConfigurationManager.AppSettings["PostgreSQLConnectionString"];

	//}

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
			"mp_schemaversion",
			" WHERE applicationname ILIKE '" + applicationName.ToLower() + "'");

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

		return (rowsAffected > 0);

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

		return (rowsAffected > -1);

	}

	public static bool SchemaVersionDeleteSchemaVersion(Guid applicationId)
	{
		NpgsqlParameter[] arParams = new NpgsqlParameter[1];

		arParams[0] = new NpgsqlParameter(":applicationid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = applicationId.ToString();

		int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
			CommandType.StoredProcedure,
			"mp_schemaversion_delete(:applicationid)",
			arParams);

		return (rowsAffected > -1);

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

	public static bool SchemaScriptHistoryDeleteSchemaScriptHistory(int id)
	{
		NpgsqlParameter[] arParams = new NpgsqlParameter[1];

		arParams[0] = new NpgsqlParameter(":id", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = id;

		int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
			CommandType.StoredProcedure,
			"mp_schemascripthistory_delete(:id)",
			arParams);

		return (rowsAffected > -1);

	}

	public static IDataReader SchemaScriptHistoryGetSchemaScriptHistory(int id)
	{
		NpgsqlParameter[] arParams = new NpgsqlParameter[1];

		arParams[0] = new NpgsqlParameter(":id", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = id;

		return NpgsqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			CommandType.StoredProcedure,
			"mp_schemascripthistory_select_one(:id)",
			arParams);

	}

	public static IDataReader SchemaScriptHistoryGetSchemaScriptHistory(Guid applicationId)
	{
		NpgsqlParameter[] arParams = new NpgsqlParameter[1];

		arParams[0] = new NpgsqlParameter(":applicationid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = applicationId.ToString();

		return NpgsqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			CommandType.StoredProcedure,
			"mp_schemascripthistory_select_byapp(:applicationid)",
			arParams);

	}

	public static IDataReader SchemaScriptHistoryGetSchemaScriptErrorHistory(Guid applicationId)
	{
		NpgsqlParameter[] arParams = new NpgsqlParameter[1];

		arParams[0] = new NpgsqlParameter(":applicationid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = applicationId.ToString();

		return NpgsqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			CommandType.StoredProcedure,
			"mp_schemascripthistory_select_errorsbyapp(:applicationid)",
			arParams);

	}

	public static bool SchemaScriptHistoryExists(Guid applicationId, String scriptFile)
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

		return (count > 0);

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

					row[((System.String)arrayList[i])] = reader[(System.String)arrayList[i]];

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

	public static DbException DatabaseHelperGetConnectionError(String overrideConnectionInfo)
	{
		DbException exception = null;

		NpgsqlConnection connection;

		if (
			(overrideConnectionInfo != null)
			&& (overrideConnectionInfo.Length > 0)
		  )
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
				connection.Close();
		}


		return exception;

	}

	public static bool DatabaseHelperCanAccessDatabase(String overrideConnectionInfo)
	{
		bool result = false;

		NpgsqlConnection connection;

		if (
			(overrideConnectionInfo != null)
			&& (overrideConnectionInfo.Length > 0)
		  )
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

	public static bool DatabaseHelperCanAlterSchema(String overrideConnectionInfo)
	{

		bool result = true;
		// Make sure we can create, alter and drop tables

		StringBuilder sqlCommand = new StringBuilder();

		sqlCommand.Append("CREATE SEQUENCE \"mp_testdb_fooid_seq\"; CREATE TABLE \"mp_testdb\" ( \"categoryid\"	int4 NOT NULL DEFAULT nextval('\"mp_testdb_fooid_seq\"'::text), \"fooid\" int4 NOT NULL, \"foo\" varchar(255) NULL );");

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

	public static bool DatabaseHelperCanCreateTemporaryTables()
	{
		bool result = true;
		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append(" create temp table t_temptabletest ");
		sqlCommand.Append("(id int, pagename varchar (100)); ");

		sqlCommand.Append("drop table t_temptabletest;");
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
		String overrideConnectionInfo)
	{
		if (scriptFile == null) return false;

		string script = File.ReadAllText(scriptFile.FullName);

		if ((script == null) || (script.Length == 0)) return true;

		return DatabaseHelperRunScript(script, overrideConnectionInfo);

	}

	public static bool DatabaseHelperRunScript(String script, String overrideConnectionInfo)
	{
		if ((script == null) || (script.Length == 0)) return true;

		bool result = false;
		NpgsqlConnection connection;

		if (
			(overrideConnectionInfo != null)
			&& (overrideConnectionInfo.Length > 0)
		  )
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

	public static bool DatabaseHelperUpdateTableField(
		String connectionString,
		String tableName,
		String keyFieldName,
		String keyFieldValue,
		String dataFieldName,
		String dataFieldValue,
		String additionalWhere)
	{
		bool result = false;

		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("UPDATE " + tableName.ToLower() + " ");
		sqlCommand.Append(" SET " + dataFieldName.ToLower() + " = :fieldvalue ");
		sqlCommand.Append(" WHERE " + keyFieldName.ToLower() + " = " + keyFieldValue);
		sqlCommand.Append(" " + additionalWhere + " ");
		sqlCommand.Append(" ; ");

		NpgsqlParameter[] arParams = new NpgsqlParameter[1];

		arParams[0] = new NpgsqlParameter(":fieldvalue", NpgsqlDbType.Text);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = dataFieldValue;

		//NpgsqlConnection connection = new NpgsqlConnection(connectionString);
		//connection.Open();
		//try
		//{
		int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
			connectionString,
			CommandType.Text,
			sqlCommand.ToString(),
			arParams);

		result = (rowsAffected > 0);
		//}
		//finally
		//{
		//    connection.Close();
		//}

		return result;

	}

	public static bool DatabaseHelperUpdateTableField(
		String tableName,
		String keyFieldName,
		String keyFieldValue,
		String dataFieldName,
		String dataFieldValue,
		String additionalWhere)
	{
		bool result = false;

		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("UPDATE " + tableName.ToLower() + " ");
		sqlCommand.Append(" SET " + dataFieldName.ToLower() + " = :fieldvalue ");
		sqlCommand.Append(" WHERE " + keyFieldName.ToLower() + " = " + keyFieldValue);
		sqlCommand.Append(" " + additionalWhere + " ");
		sqlCommand.Append(" ; ");

		NpgsqlParameter[] arParams = new NpgsqlParameter[1];

		arParams[0] = new NpgsqlParameter(":fieldvalue", NpgsqlDbType.Text);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = dataFieldValue;

		//NpgsqlConnection connection = new NpgsqlConnection(GetConnectionString());
		//connection.Open();
		//try
		//{
		int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			CommandType.Text,
			sqlCommand.ToString(),
			arParams);

		result = (rowsAffected > 0);
		//}
		//finally
		//{
		//    connection.Close();
		//}

		return result;

	}

	public static IDataReader DatabaseHelperGetReader(
		String connectionString,
		String tableName,
		String whereClause)
	{
		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("select * ");
		sqlCommand.Append("from " + tableName.ToLower() + " ");
		sqlCommand.Append(whereClause);
		sqlCommand.Append(" ; ");

		return NpgsqlHelper.ExecuteReader(
			connectionString,
			CommandType.Text,
			sqlCommand.ToString());

	}

	public static IDataReader DatabaseHelperGetReader(
		string connectionString,
		string query
		)
	{
		if (string.IsNullOrEmpty(connectionString)) { connectionString = ConnectionString.GetReadConnectionString(); }

		return NpgsqlHelper.ExecuteReader(
			connectionString,
			CommandType.Text,
			query);


	}

	public static int DatabaseHelperExecteNonQuery(
		string connectionString,
		string query
		)
	{
		if (string.IsNullOrEmpty(connectionString)) { connectionString = ConnectionString.GetWriteConnectionString(); }

		int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
			connectionString,
			CommandType.Text,
			query);

		return rowsAffected;

	}

	public static DataTable DatabaseHelperGetTable(
		String connectionString,
		String tableName,
		String whereClause)
	{
		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("select * ");
		sqlCommand.Append("from " + tableName.ToLower() + " ");
		sqlCommand.Append(whereClause);
		sqlCommand.Append(" ; ");

		DataSet ds = NpgsqlHelper.ExecuteDataset(
			connectionString,
			CommandType.Text,
			sqlCommand.ToString());

		return ds.Tables[0];

	}

	public static void DatabaseHelperDoForumVersion2202PostUpgradeTasks(
		String overrideConnectionInfo)
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
			"mp_forums",
			" where (forumguid is null OR forumguid = '00000000-0000-0000-0000-000000000000') ");


		foreach (DataRow row in dataTable.Rows)
		{
			DatabaseHelperUpdateTableField(
				"mp_forums",
				"itemid",
				row["ItemID"].ToString(),
				"forumguid",
				Guid.NewGuid().ToString(),
				"  ");

		}

		dataTable = DatabaseHelperGetTable(
			connectionString,
			"mp_forumthreads",
			" where (threadguid is null OR threadguid = '00000000-0000-0000-0000-000000000000') ");


		foreach (DataRow row in dataTable.Rows)
		{
			DatabaseHelperUpdateTableField(
				"mp_forumthreads",
				"threadid",
				row["ThreadID"].ToString(),
				"threadguid",
				Guid.NewGuid().ToString(),
				"  ");

		}

		dataTable = DatabaseHelperGetTable(
			connectionString,
			"mp_forumposts",
			" where (postguid is null OR postguid = '00000000-0000-0000-0000-000000000000') ");


		foreach (DataRow row in dataTable.Rows)
		{
			DatabaseHelperUpdateTableField(
				"mp_forumposts",
				"postid",
				row["PostID"].ToString(),
				"postguid",
				Guid.NewGuid().ToString(),
				"  ");

		}

	}

	public static void DatabaseHelperDoForumVersion2203PostUpgradeTasks(
		String overrideConnectionInfo)
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

		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("SELECT subscriptionid ");
		sqlCommand.Append("FROM mp_forumsubscriptions ");
		sqlCommand.Append(" where (subguid is null OR subguid = '00000000-0000-0000-0000-000000000000') ");
		sqlCommand.Append(" ; ");

		DataSet ds = NpgsqlHelper.ExecuteDataset(
			connectionString,
			CommandType.Text,
			sqlCommand.ToString(),
			null);

		DataTable dataTable = ds.Tables[0];


		foreach (DataRow row in dataTable.Rows)
		{
			DatabaseHelperUpdateTableField(
				"mp_forumsubscriptions",
				"subscriptionid",
				row["SubscriptionID"].ToString(),
				"subguid",
				Guid.NewGuid().ToString(),
				"  ");

		}



		sqlCommand = new StringBuilder();
		sqlCommand.Append("SELECT threadsubscriptionid ");
		sqlCommand.Append("FROM mp_forumthreadsubscriptions ");
		sqlCommand.Append(" where (subguid is null OR subguid = '00000000-0000-0000-0000-000000000000') ");
		sqlCommand.Append(" ; ");

		ds = NpgsqlHelper.ExecuteDataset(
			connectionString,
			CommandType.Text,
			sqlCommand.ToString(),
			null);

		dataTable = ds.Tables[0];


		foreach (DataRow row in dataTable.Rows)
		{
			DatabaseHelperUpdateTableField(
				"mp_ForumThreadSubscriptions",
				"threadsubscriptionid",
				row["ThreadSubscriptionID"].ToString(),
				"subguid",
				Guid.NewGuid().ToString(),
				"  ");

		}



	}


	public static bool RunPostUpgradeTask(Version version, string connectionString)
	{
		if (string.IsNullOrWhiteSpace(connectionString))
		{
			connectionString = ConnectionString.GetWriteConnectionString();
		}

		bool result = true;
		DataTable dataTable = new();
		string sqlCommand;
		bool localResult;

		switch (version)
		{
			case var _ when version == new Version(2, 3, 2, 0):
				sqlCommand = @"
SELECT  
	u.siteguid, 
	ls.letterinfoguid, 
	ls.userguid, 
	u.email, 
	ls.beginutc, 
	ls.usehtml
FROM mp_lettersubscriber ls
JOIN mp_users u ON u.userguid = ls.userguid; ";

				var ds = NpgsqlHelper.ExecuteDataset(connectionString, CommandType.Text, sqlCommand);

				dataTable = ds.Tables[0];

				foreach (DataRow row in dataTable.Rows)
				{

					int n = DBLetterSubscription.Create(
						Guid.NewGuid(),
						new Guid(row["SiteGuid"].ToString()),
						new Guid(row["LetterInfoGuid"].ToString()),
						new Guid(row["UserGuid"].ToString()),
						row["Email"].ToString().ToLower(),
						true,
						new Guid("00000000-0000-0000-0000-000000000000"),
						Convert.ToDateTime(row["BeginUTC"]),
						Convert.ToBoolean(row["UseHtml"])
						);
					if (n < 1)
					{
						result = false;
					}
				}

				return result;

			case var _ when version == new Version(2, 2, 3, 0):
				dataTable = DatabaseHelperGetTable(connectionString, "mp_moduledefinitions", " where guid is null ");

				// UPDATE mp_ModuleDefinitions SET [Guid] = newid() 
				// WHERE [Guid] IS NULL
				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField(
						"mp_moduledefinitions",
						"moduledefid",
						row["ModuleDefID"].ToString(),
						"guid",
						Guid.NewGuid().ToString(),
						" and guid is null ");

					if (!localResult)
					{
						result = localResult;
					}
				}

				sqlCommand = @"
UPDATE mp_moduledefinitionsettings 
SET featureguid = (
	SELECT guid 
	FROM mp_moduledefinitions 
	WHERE mp_moduledefinitions.moduledefid = mp_moduledefinitionsettings.moduleDefid LIMIT 1
);";

				localResult = DatabaseHelperRunScript(sqlCommand.ToString(), connectionString);
				if (!localResult)
				{
					result = localResult;
				}

				localResult = DatabaseHelperRunScript("ALTER TABLE mp_moduledefinitions ALTER COLUMN guid SET NOT NULL;", connectionString);
				if (!localResult)
				{
					result = localResult;
				}

				localResult = DatabaseHelperRunScript("ALTER TABLE mp_moduledefinitionsettings ALTER COLUMN featureguid SET NOT NULL;", connectionString);
				if (!localResult)
				{
					result = localResult;
				}
				return result;


			case var _ when version == new Version(2, 2, 3, 4):

				dataTable = DatabaseHelperGetTable(connectionString, "mp_pages", " where pageguid is null ");

				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField(
						"mp_pages",
						"pageid",
						row["PageID"].ToString(),
						"pageguid",
						Guid.NewGuid().ToString(),
						" and pageguid is null ");

					if (!localResult)
					{
						result = localResult;
					}
				}

				return result;

			case var _ when version == new Version(2, 2, 4, 7):
				dataTable = DatabaseHelperGetTable(connectionString, "mp_friendlyurls", " where itemguid is null ");

				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField(
						"mp_friendlyurls",
						"urlid",
						row["UrlID"].ToString(),
						"itemguid",
						Guid.NewGuid().ToString(),
						" and itemguid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}


				dataTable = DatabaseHelperGetTable(connectionString, "mp_modules", " where guid is null ");

				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField(
						"mp_modules",
						"moduleid",
						row["ModuleID"].ToString(),
						"guid",
						Guid.NewGuid().ToString(),
						" and guid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}


				dataTable = DatabaseHelperGetTable(connectionString, "mp_roles", " where roleguid is null ");

				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField(
						"mp_roles",
						"roleid",
						row["RoleID"].ToString(),
						"roleguid",
						Guid.NewGuid().ToString(),
						" and roleguid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}


				dataTable = DatabaseHelperGetTable(connectionString, "mp_modulesettings", " where settingguid is null ");

				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField(
						"mp_modulesettings",
						"id",
						row["ID"].ToString(),
						"settingguid",
						Guid.NewGuid().ToString(),
						" and settingguid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}

				dataTable = DatabaseHelperGetTable(connectionString, "mp_blogs", " where blogguid is null ");

				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField(
						"mp_blogs",
						"itemid",
						row["ItemID"].ToString(),
						"blogguid",
						Guid.NewGuid().ToString(),
						" and blogguid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}

				dataTable = DatabaseHelperGetTable(connectionString, "mp_calendarevents", " where itemguid is null ");


				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField(
						"mp_calendarevents",
						"itemid",
						row["ItemID"].ToString(),
						"itemguid",
						Guid.NewGuid().ToString(),
						" and itemguid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}


				dataTable = DatabaseHelperGetTable(connectionString, "mp_galleryimages", " where itemguid is null ");

				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField(
						"mp_galleryimages",
						"itemid",
						row["ItemID"].ToString(),
						"itemguid",
						Guid.NewGuid().ToString(),
						" and itemguid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}

				dataTable = DatabaseHelperGetTable(connectionString, "mp_htmlcontent", " where itemguid is null ");

				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField(
						"mp_htmlcontent",
						"itemid",
						row["ItemID"].ToString(),
						"itemguid",
						Guid.NewGuid().ToString(),
						" and itemguid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}

				dataTable = DatabaseHelperGetTable(connectionString, "mp_links", " where itemguid is null ");

				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField(
						"mp_links",
						"itemid",
						row["ItemID"].ToString(),
						"itemguid",
						Guid.NewGuid().ToString(),
						" and itemguid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}


				dataTable = DatabaseHelperGetTable(connectionString, "mp_sharedfilefolders", " where folderguid is null ");

				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField(
						"mp_sharedfilefolders",
						"folderid",
						row["FolderID"].ToString(),
						"folderguid",
						Guid.NewGuid().ToString(),
						" and folderguid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}

				dataTable = DatabaseHelperGetTable(connectionString, "mp_sharedfiles", " where itemguid is null ");

				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField(
						"mp_sharedfiles",
						"itemid",
						row["ItemID"].ToString(),
						"itemguid",
						Guid.NewGuid().ToString(),
						" and itemguid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}
				return result;

			case var _ when version == new Version(2, 2, 5, 3):

				dataTable = DatabaseHelperGetTable(connectionString, "mp_rssfeeds", " where itemguid is null ");

				foreach (DataRow row in dataTable.Rows)
				{
					localResult = DatabaseHelperUpdateTableField(
						"mp_rssfeeds",
						"itemid",
						row["ItemID"].ToString(),
						"itemguid",
						Guid.NewGuid().ToString(),
						" and itemguid is null ");
					if (!localResult)
					{
						result = localResult;
					}
				}

				return result;
		}

		return true;
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
			return (table.Rows.Count > 0);
		}

		return false;
	}



	#endregion



}
