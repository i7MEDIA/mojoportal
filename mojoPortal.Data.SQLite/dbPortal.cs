/// Author:					
/// Created:				2004-08-03
/// Last Modified:		    2013-08-19
/// 
/// This data Facade has all static methods and serves to abstract the underlying database
/// from the business layer. To port this application to another database platform, you only have to
/// replace this class. Make a copy of it and change the implementation to support your db of choice, 
/// then compile and replace the original mojoPortal.Data.dll
/// 
/// This implementation is for SQLite. 

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Globalization;
using System.IO;
using System.Web;
using log4net;
using Mono.Data.Sqlite;

namespace mojoPortal.Data 
{
	
	public static class DBPortal 
    {
        // Create a logger for use in this class
        private static readonly ILog log = LogManager.GetLogger(typeof(DBPortal));

        public static String DBPlatform()
        {
            return "SQLite";
        }

        private static string GetConnectionString()
        {
            string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];
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

        public static void EnsureDatabase()
        {
        }

        
        #region Versioning and Upgrade Helpers

       
       

        #region Schema Table Methods

        public static IDataReader DatabaseHelperGetApplicationId(string applicationName)
        {
            return DatabaseHelperGetReader(
                GetConnectionString(),
                "mp_SchemaVersion",
                " WHERE LOWER(ApplicationName) = '" + applicationName.ToLower() + "'");

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
            #region Bit Conversion


            #endregion

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
            return (rowsAffected > -1);

        }


        public static bool SchemaVersionDeleteSchemaVersion(
            Guid applicationId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SchemaVersion ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ApplicationID = :ApplicationID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ApplicationID", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return (rowsAffected > 0);

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

        public static bool SchemaScriptHistoryDeleteSchemaScriptHistory(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SchemaScriptHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ID = :ID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return (rowsAffected > 0);

        }

        public static IDataReader SchemaScriptHistoryGetSchemaScriptHistory(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SchemaScriptHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ID = :ID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader SchemaScriptHistoryGetSchemaScriptHistory(Guid applicationId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SchemaScriptHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ApplicationID = :ApplicationID ");
            //sqlCommand.Append("AND ErrorOccurred = 0 ");
            sqlCommand.Append(" ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ApplicationID", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader SchemaScriptHistoryGetSchemaScriptErrorHistory(Guid applicationId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SchemaScriptHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ApplicationID = :ApplicationID ");
            sqlCommand.Append("AND ErrorOccurred = 1 ");
            sqlCommand.Append(" ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ApplicationID", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static bool SchemaScriptHistoryExists(Guid applicationId, String scriptFile)
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

            SqliteConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
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
                    connection.Close();
            }


            return exception;

        }

        public static bool DatabaseHelperCanAccessDatabase(String overrideConnectionInfo)
        {
            bool result = false;

            SqliteConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
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
            sqlCommand.Append(@"
                CREATE TABLE `mp_Testdb` (
                  `FooID` INTEGER PRIMARY KEY,
                  `Foo` varchar(255) NOT NULL default ''
                );
                ");

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
            //catch (SqliteExecutionException)
            //{
            //    result = false;
            //}


            sqlCommand = new StringBuilder();
            sqlCommand.Append("ALTER TABLE mp_Testdb ADD COLUMN `MoreFoo` varchar(255) NULL;");

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
            sqlCommand.Append("DROP TABLE mp_Testdb;");

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
            sqlCommand.Append(" CREATE TEMPORARY TABLE Temptest ");
            sqlCommand.Append("(IndexID INT  ,");
            sqlCommand.Append(" foo VARCHAR (100) );");
            sqlCommand.Append(" DROP TABLE Temptest;");
            try
            {
                DatabaseHelperRunScript(sqlCommand.ToString(), GetConnectionString());
            }
            catch 
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
            SqliteConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new SqliteConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new SqliteConnection(GetConnectionString());
            }

            connection.Open();

            SqliteTransaction transaction = (SqliteTransaction)connection.BeginTransaction();

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
			sqlCommand.Append("UPDATE " + tableName + " ");
			sqlCommand.Append(" SET " + dataFieldName + " = :fieldValue ");
			sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue );
			sqlCommand.Append(" " + additionalWhere + " ");
			sqlCommand.Append(" ; ");
			
			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":fieldValue", DbType.String);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = dataFieldValue;

			SqliteConnection connection = new SqliteConnection(connectionString);
			connection.Open();
            try
            {
                int rowsAffected = SqliteHelper.ExecuteNonQuery(connection, sqlCommand.ToString(), arParams);
                result = (rowsAffected > 0);
            }
            finally
            {
                connection.Close();
            }

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
            sqlCommand.Append("UPDATE " + tableName + " ");
            sqlCommand.Append(" SET " + dataFieldName + " = :fieldValue ");
            sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
            sqlCommand.Append(" " + additionalWhere + " ");
            sqlCommand.Append(" ; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":fieldValue", DbType.String);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = dataFieldValue;

            SqliteConnection connection = new SqliteConnection(GetConnectionString());
            connection.Open();
            try
            {
                int rowsAffected = SqliteHelper.ExecuteNonQuery(connection, sqlCommand.ToString(), arParams);
                result = (rowsAffected > 0);
            }
            finally
            {
                connection.Close();
            }

            return result;

        }

		public static IDataReader DatabaseHelperGetReader(
			String connectionString,
			String tableName, 
			String whereClause)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT * ");
			sqlCommand.Append("FROM " + tableName + " ");
			sqlCommand.Append(whereClause);
			sqlCommand.Append(" ; ");

			return SqliteHelper.ExecuteReader(
				connectionString, 
				sqlCommand.ToString());

		}

        public static IDataReader DatabaseHelperGetReader(
            string connectionString,
            string query
            )
        {
            if (string.IsNullOrEmpty(connectionString)) { connectionString = GetConnectionString(); }

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

		public static DataTable DatabaseHelperGetTable(
			String connectionString,
			String tableName, 
			String whereClause)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT * ");
			sqlCommand.Append("FROM " + tableName + " ");
			sqlCommand.Append(whereClause);
			sqlCommand.Append(" ; ");

			DataSet ds = SqliteHelper.ExecuteDataset(
				connectionString, 
				sqlCommand.ToString());

			return ds.Tables[0];

		}

        public static void DatabaseHelperDoForumVersion2202PostUpgradeTasks(
            String overrideConnectionInfo)
        {
            // we need to poulate the new guid fields from .net code in this db platform

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
                connectionString = GetConnectionString();
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
            sqlCommand.Append("SELECT SubscriptionID ");
            sqlCommand.Append("FROM mp_ForumSubscriptions ");
            sqlCommand.Append(" where (SubGuid is null OR SubGuid = '00000000-0000-0000-0000-000000000000') ");
            sqlCommand.Append(" ; ");

            DataSet ds = SqliteHelper.ExecuteDataset(
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



            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ThreadSubscriptionID ");
            sqlCommand.Append("FROM mp_ForumThreadSubscriptions ");
            sqlCommand.Append(" where (SubGuid is null OR SubGuid = '00000000-0000-0000-0000-000000000000') ");
            sqlCommand.Append(" ; ");

            ds = SqliteHelper.ExecuteDataset(
                connectionString,
                sqlCommand.ToString());

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
                connectionString = GetConnectionString();
            }


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("u.SiteGuid, ");
            sqlCommand.Append("ls.LetterInfoGuid, ");
            sqlCommand.Append("ls.UserGuid, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("ls.BeginUTC, ");
            sqlCommand.Append("ls.UseHtml ");


            sqlCommand.Append("FROM ");
            sqlCommand.Append("mp_LetterSubscriber ls ");
            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.UserGuid = ls.UserGuid ");
            sqlCommand.Append(" ; ");

            DataSet ds = SqliteHelper.ExecuteDataset(
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
                    Convert.ToBoolean(row["UseHtml"])
                    );

            }

        }

        public static void DatabaseHelperDoVersion2230PostUpgradeTasks(
            String overrideConnectionInfo)
        {


        }

        public static void DatabaseHelperDoVersion2234PostUpgradeTasks(
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
                connectionString = GetConnectionString();
            }

            DataTable dataTable = DatabaseHelperGetTable(
                connectionString,
                "mp_Pages",
                " where PageGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "mp_Pages",
                    "PageID",
                    row["PageID"].ToString(),
                    "PageGuid",
                    Guid.NewGuid().ToString(),
                    " and PageGuid is null ");

            }


        }

        public static void DatabaseHelperDoVersion2247PostUpgradeTasks(
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
                connectionString = GetConnectionString();
            }

            DataTable dataTable = DatabaseHelperGetTable(
                connectionString,
                "mp_FriendlyUrls",
                " where ItemGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "mp_FriendlyUrls",
                    "UrlID",
                    row["UrlID"].ToString(),
                    "ItemGuid",
                    Guid.NewGuid().ToString(),
                    " and ItemGuid is null ");

            }

            dataTable = DatabaseHelperGetTable(
                connectionString,
                "mp_Modules",
                " where Guid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "mp_Modules",
                    "ModuleID",
                    row["ModuleID"].ToString(),
                    "Guid",
                    Guid.NewGuid().ToString(),
                    " and Guid is null ");

            }


            dataTable = DatabaseHelperGetTable(
                connectionString,
                "mp_Roles",
                " where RoleGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "mp_Roles",
                    "RoleID",
                    row["RoleID"].ToString(),
                    "RoleGuid",
                    Guid.NewGuid().ToString(),
                    " and RoleGuid is null ");

            }

            dataTable = DatabaseHelperGetTable(
                connectionString,
                "mp_ModuleSettings",
                " where SettingGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "mp_ModuleSettings",
                    "ID",
                    row["ID"].ToString(),
                    "SettingGuid",
                    Guid.NewGuid().ToString(),
                    " and SettingGuid is null ");

            }

            dataTable = DatabaseHelperGetTable(
                connectionString,
                "mp_Blogs",
                " where BlogGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "mp_Blogs",
                    "ItemID",
                    row["ItemID"].ToString(),
                    "BlogGuid",
                    Guid.NewGuid().ToString(),
                    " and BlogGuid is null ");

            }

            dataTable = DatabaseHelperGetTable(
                connectionString,
                "mp_CalendarEvents",
                " where ItemGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "mp_CalendarEvents",
                    "ItemID",
                    row["ItemID"].ToString(),
                    "ItemGuid",
                    Guid.NewGuid().ToString(),
                    " and ItemGuid is null ");

            }


            dataTable = DatabaseHelperGetTable(
                connectionString,
                "mp_GalleryImages",
                " where ItemGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "mp_GalleryImages",
                    "ItemID",
                    row["ItemID"].ToString(),
                    "ItemGuid",
                    Guid.NewGuid().ToString(),
                    " and ItemGuid is null ");

            }

            dataTable = DatabaseHelperGetTable(
                connectionString,
                "mp_HtmlContent",
                " where ItemGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "mp_HtmlContent",
                    "ItemID",
                    row["ItemID"].ToString(),
                    "ItemGuid",
                    Guid.NewGuid().ToString(),
                    " and ItemGuid is null ");

            }

            dataTable = DatabaseHelperGetTable(
                connectionString,
                "mp_Links",
                " where ItemGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "mp_Links",
                    "ItemID",
                    row["ItemID"].ToString(),
                    "ItemGuid",
                    Guid.NewGuid().ToString(),
                    " and ItemGuid is null ");

            }


            dataTable = DatabaseHelperGetTable(
                connectionString,
                "mp_SharedFileFolders",
                " where FolderGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "mp_SharedFileFolders",
                    "FolderID",
                    row["FolderID"].ToString(),
                    "FolderGuid",
                    Guid.NewGuid().ToString(),
                    " and FolderGuid is null ");

            }

            dataTable = DatabaseHelperGetTable(
                connectionString,
                "mp_SharedFiles",
                " where ItemGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "mp_SharedFiles",
                    "ItemID",
                    row["ItemID"].ToString(),
                    "ItemGuid",
                    Guid.NewGuid().ToString(),
                    " and ItemGuid is null ");

            }


        }

        public static void DatabaseHelperDoVersion2253PostUpgradeTasks(
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
                connectionString = GetConnectionString();
            }

            DataTable dataTable = DatabaseHelperGetTable(
                connectionString,
                "mp_RssFeeds",
                " where ItemGuid is null ");


            foreach (DataRow row in dataTable.Rows)
            {
                DatabaseHelperUpdateTableField(
                    "mp_RssFeeds",
                    "ItemID",
                    row["ItemID"].ToString(),
                    "ItemGuid",
                    Guid.NewGuid().ToString(),
                    " and ItemGuid is null ");

            }




        }

        public static bool DatabaseHelperSitesTableExists()
        {
            return DatabaseHelperTableExists("mp_Sites");
        }

        public static bool DatabaseHelperTableExists(string tableName)
        {
            SqliteConnection connection = new SqliteConnection(GetConnectionString());
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

        

        //#region Private Message System

        //public static int PrivateMessage_AddPrivateMessage(
        //    Guid messageID,
        //    Guid fromUser,
        //    Guid priorityID,
        //    string subject,
        //    string body,
        //    string toCSVList,
        //    string ccCSVList,
        //    string bccCSVList,
        //    string toCSVLabels,
        //    string ccCSVLabels,
        //    string bccCSVLabels,
        //    DateTime createdDate,
        //    DateTime sentDate)
        //{
        //    #region Bit Conversion


        //    #endregion

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("INSERT INTO mp_PrivateMessages (");
        //    sqlCommand.Append("MessageID, ");
        //    sqlCommand.Append("FromUser, ");
        //    sqlCommand.Append("PriorityID, ");
        //    sqlCommand.Append("Subject, ");
        //    sqlCommand.Append("Body, ");
        //    sqlCommand.Append("ToCSVList, ");
        //    sqlCommand.Append("CcCSVList, ");
        //    sqlCommand.Append("BccCSVList, ");
        //    sqlCommand.Append("ToCSVLabels, ");
        //    sqlCommand.Append("CcCSVLabels, ");
        //    sqlCommand.Append("BccCSVLabels, ");
        //    sqlCommand.Append("CreatedDate, ");
        //    sqlCommand.Append("SentDate )");

        //    sqlCommand.Append(" VALUES (");
        //    sqlCommand.Append(":MessageID, ");
        //    sqlCommand.Append(":FromUser, ");
        //    sqlCommand.Append(":PriorityID, ");
        //    sqlCommand.Append(":Subject, ");
        //    sqlCommand.Append(":Body, ");
        //    sqlCommand.Append(":ToCSVList, ");
        //    sqlCommand.Append(":CcCSVList, ");
        //    sqlCommand.Append(":BccCSVList, ");
        //    sqlCommand.Append(":ToCSVLabels, ");
        //    sqlCommand.Append(":CcCSVLabels, ");
        //    sqlCommand.Append(":BccCSVLabels, ");
        //    sqlCommand.Append(":CreatedDate, ");
        //    sqlCommand.Append(":SentDate );");

        //    SqliteParameter[] arParams = new SqliteParameter[13];

        //    arParams[0] = new SqliteParameter(":MessageID", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = messageID.ToString();

        //    arParams[1] = new SqliteParameter(":FromUser", DbType.String, 36);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = fromUser.ToString();

        //    arParams[2] = new SqliteParameter(":PriorityID", DbType.String, 36);
        //    arParams[2].Direction = ParameterDirection.Input;
        //    arParams[2].Value = priorityID.ToString();

        //    arParams[3] = new SqliteParameter(":Subject", DbType.String, 255);
        //    arParams[3].Direction = ParameterDirection.Input;
        //    arParams[3].Value = subject;

        //    arParams[4] = new SqliteParameter(":Body", DbType.Object);
        //    arParams[4].Direction = ParameterDirection.Input;
        //    arParams[4].Value = body;

        //    arParams[5] = new SqliteParameter(":ToCSVList", DbType.Object);
        //    arParams[5].Direction = ParameterDirection.Input;
        //    arParams[5].Value = toCSVList;

        //    arParams[6] = new SqliteParameter(":CcCSVList", DbType.Object);
        //    arParams[6].Direction = ParameterDirection.Input;
        //    arParams[6].Value = ccCSVList;

        //    arParams[7] = new SqliteParameter(":BccCSVList", DbType.Object);
        //    arParams[7].Direction = ParameterDirection.Input;
        //    arParams[7].Value = bccCSVList;

        //    arParams[8] = new SqliteParameter(":ToCSVLabels", DbType.Object);
        //    arParams[8].Direction = ParameterDirection.Input;
        //    arParams[8].Value = toCSVLabels;

        //    arParams[9] = new SqliteParameter(":CcCSVLabels", DbType.Object);
        //    arParams[9].Direction = ParameterDirection.Input;
        //    arParams[9].Value = ccCSVLabels;

        //    arParams[10] = new SqliteParameter(":BccCSVLabels", DbType.Object);
        //    arParams[10].Direction = ParameterDirection.Input;
        //    arParams[10].Value = bccCSVLabels;

        //    arParams[11] = new SqliteParameter(":CreatedDate", DbType.DateTime);
        //    arParams[11].Direction = ParameterDirection.Input;
        //    arParams[11].Value = createdDate;

        //    arParams[12] = new SqliteParameter(":SentDate", DbType.DateTime);
        //    arParams[12].Direction = ParameterDirection.Input;
        //    arParams[12].Value = sentDate;


        //    int rowsAffected = 0;
        //    rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
        //    return rowsAffected;

        //}




        //public static bool PrivateMessage_UpdatePrivateMessage(
        //    Guid messageID,
        //    Guid fromUser,
        //    Guid priorityID,
        //    string subject,
        //    string body,
        //    string toCSVList,
        //    string ccCSVList,
        //    string bccCSVList,
        //    string toCSVLabels,
        //    string ccCSVLabels,
        //    string bccCSVLabels,
        //    DateTime createdDate,
        //    DateTime sentDate)
        //{
        //    #region Bit Conversion


        //    #endregion

        //    StringBuilder sqlCommand = new StringBuilder();

        //    sqlCommand.Append("UPDATE mp_PrivateMessages ");
        //    sqlCommand.Append("SET  ");
        //    sqlCommand.Append("FromUser = :FromUser, ");
        //    sqlCommand.Append("PriorityID = :PriorityID, ");
        //    sqlCommand.Append("Subject = :Subject, ");
        //    sqlCommand.Append("Body = :Body, ");
        //    sqlCommand.Append("ToCSVList = :ToCSVList, ");
        //    sqlCommand.Append("CcCSVList = :CcCSVList, ");
        //    sqlCommand.Append("BccCSVList = :BccCSVList, ");
        //    sqlCommand.Append("ToCSVLabels = :ToCSVLabels, ");
        //    sqlCommand.Append("CcCSVLabels = :CcCSVLabels, ");
        //    sqlCommand.Append("BccCSVLabels = :BccCSVLabels, ");
        //    sqlCommand.Append("CreatedDate = :CreatedDate, ");
        //    sqlCommand.Append("SentDate = :SentDate ");

        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("MessageID = :MessageID ;");

        //    SqliteParameter[] arParams = new SqliteParameter[13];

        //    arParams[0] = new SqliteParameter(":MessageID", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = messageID.ToString();

        //    arParams[1] = new SqliteParameter(":FromUser", DbType.String, 36);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = fromUser.ToString();

        //    arParams[2] = new SqliteParameter(":PriorityID", DbType.String, 36);
        //    arParams[2].Direction = ParameterDirection.Input;
        //    arParams[2].Value = priorityID.ToString();

        //    arParams[3] = new SqliteParameter(":Subject", DbType.String, 255);
        //    arParams[3].Direction = ParameterDirection.Input;
        //    arParams[3].Value = subject;

        //    arParams[4] = new SqliteParameter(":Body", DbType.Object);
        //    arParams[4].Direction = ParameterDirection.Input;
        //    arParams[4].Value = body;

        //    arParams[5] = new SqliteParameter(":ToCSVList", DbType.Object);
        //    arParams[5].Direction = ParameterDirection.Input;
        //    arParams[5].Value = toCSVList;

        //    arParams[6] = new SqliteParameter(":CcCSVList", DbType.Object);
        //    arParams[6].Direction = ParameterDirection.Input;
        //    arParams[6].Value = ccCSVList;

        //    arParams[7] = new SqliteParameter(":BccCSVList", DbType.Object);
        //    arParams[7].Direction = ParameterDirection.Input;
        //    arParams[7].Value = bccCSVList;

        //    arParams[8] = new SqliteParameter(":ToCSVLabels", DbType.Object);
        //    arParams[8].Direction = ParameterDirection.Input;
        //    arParams[8].Value = toCSVLabels;

        //    arParams[9] = new SqliteParameter(":CcCSVLabels", DbType.Object);
        //    arParams[9].Direction = ParameterDirection.Input;
        //    arParams[9].Value = ccCSVLabels;

        //    arParams[10] = new SqliteParameter(":BccCSVLabels", DbType.Object);
        //    arParams[10].Direction = ParameterDirection.Input;
        //    arParams[10].Value = bccCSVLabels;

        //    arParams[11] = new SqliteParameter(":CreatedDate", DbType.DateTime);
        //    arParams[11].Direction = ParameterDirection.Input;
        //    arParams[11].Value = createdDate;

        //    arParams[12] = new SqliteParameter(":SentDate", DbType.DateTime);
        //    arParams[12].Direction = ParameterDirection.Input;
        //    arParams[12].Value = sentDate;


        //    int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
        //    return (rowsAffected > -1);

        //}


        //public static bool PrivateMessage_DeletePrivateMessage(Guid messageID)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("DELETE FROM mp_PrivateMessages ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("MessageID = :MessageID ;");

        //    SqliteParameter[] arParams = new SqliteParameter[1];

        //    arParams[0] = new SqliteParameter(":MessageID", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = messageID.ToString();


        //    int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
        //    return (rowsAffected > 0);

        //}


        //public static IDataReader PrivateMessage_GetPrivateMessage(Guid messageID)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_PrivateMessages ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("MessageID = :MessageID ;");

        //    SqliteParameter[] arParams = new SqliteParameter[1];

        //    arParams[0] = new SqliteParameter(":MessageID", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = messageID.ToString();

        //    return SqliteHelper.ExecuteReader(
        //        GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        //public static int PrivateMessagePriority_AddPrivateMessagePriority(
        //    Guid priorityID,
        //    string priority)
        //{
        //    #region Bit Conversion


        //    #endregion

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("INSERT INTO mp_PrivateMessagePriority (");
        //    sqlCommand.Append("PriorityID, ");
        //    sqlCommand.Append("Priority )");

        //    sqlCommand.Append(" VALUES (");
        //    sqlCommand.Append(":PriorityID, ");
        //    sqlCommand.Append(":Priority );");

        //    SqliteParameter[] arParams = new SqliteParameter[2];

        //    arParams[0] = new SqliteParameter(":PriorityID", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = priorityID.ToString();

        //    arParams[1] = new SqliteParameter(":Priority", DbType.String, 50);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = priority;


        //    int rowsAffected = 0;
        //    rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
        //    return rowsAffected;

        //}




        //public static bool PrivateMessagePriority_UpdatePrivateMessagePriority(
        //    Guid priorityID,
        //    string priority)
        //{
        //    #region Bit Conversion


        //    #endregion

        //    StringBuilder sqlCommand = new StringBuilder();

        //    sqlCommand.Append("UPDATE mp_PrivateMessagePriority ");
        //    sqlCommand.Append("SET  ");
        //    sqlCommand.Append("Priority = :Priority ");

        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("PriorityID = :PriorityID ;");

        //    SqliteParameter[] arParams = new SqliteParameter[2];

        //    arParams[0] = new SqliteParameter(":PriorityID", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = priorityID.ToString();

        //    arParams[1] = new SqliteParameter(":Priority", DbType.String, 50);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = priority;


        //    int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
        //    return (rowsAffected > -1);

        //}


        //public static bool PrivateMessagePriority_DeletePrivateMessagePriority(Guid priorityID)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("DELETE FROM mp_PrivateMessagePriority ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("PriorityID = :PriorityID ;");

        //    SqliteParameter[] arParams = new SqliteParameter[1];

        //    arParams[0] = new SqliteParameter(":PriorityID", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = priorityID.ToString();


        //    int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
        //    return (rowsAffected > 0);

        //}


        //public static IDataReader PrivateMessagePriority_GetPrivateMessagePriority(Guid priorityID)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_PrivateMessagePriority ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("PriorityID = :PriorityID ;");

        //    SqliteParameter[] arParams = new SqliteParameter[1];

        //    arParams[0] = new SqliteParameter(":PriorityID", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = priorityID.ToString();

        //    return SqliteHelper.ExecuteReader(
        //        GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        //public static int PrivateMessageAttachment_AddPrivateMessageAttachment(
        //    Guid attachmentID,
        //    Guid messageID,
        //    string originalFileName,
        //    string serverFileName,
        //    DateTime createdDate)
        //{
        //    #region Bit Conversion


        //    #endregion

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("INSERT INTO mp_PrivateMessageAttachments (");
        //    sqlCommand.Append("AttachmentID, ");
        //    sqlCommand.Append("MessageID, ");
        //    sqlCommand.Append("OriginalFileName, ");
        //    sqlCommand.Append("ServerFileName, ");
        //    sqlCommand.Append("CreatedDate )");

        //    sqlCommand.Append(" VALUES (");
        //    sqlCommand.Append(":AttachmentID, ");
        //    sqlCommand.Append(":MessageID, ");
        //    sqlCommand.Append(":OriginalFileName, ");
        //    sqlCommand.Append(":ServerFileName, ");
        //    sqlCommand.Append(":CreatedDate );");

        //    SqliteParameter[] arParams = new SqliteParameter[5];

        //    arParams[0] = new SqliteParameter(":AttachmentID", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = attachmentID.ToString();

        //    arParams[1] = new SqliteParameter(":MessageID", DbType.String, 36);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = messageID.ToString();

        //    arParams[2] = new SqliteParameter(":OriginalFileName", DbType.String, 255);
        //    arParams[2].Direction = ParameterDirection.Input;
        //    arParams[2].Value = originalFileName;

        //    arParams[3] = new SqliteParameter(":ServerFileName", DbType.String, 50);
        //    arParams[3].Direction = ParameterDirection.Input;
        //    arParams[3].Value = serverFileName;

        //    arParams[4] = new SqliteParameter(":CreatedDate", DbType.DateTime);
        //    arParams[4].Direction = ParameterDirection.Input;
        //    arParams[4].Value = createdDate;


        //    int rowsAffected = 0;
        //    rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
        //    return rowsAffected;

        //}




        //public static bool PrivateMessageAttachment_UpdatePrivateMessageAttachment(
        //    Guid attachmentID,
        //    Guid messageID,
        //    string originalFileName,
        //    string serverFileName,
        //    DateTime createdDate)
        //{
        //    #region Bit Conversion


        //    #endregion

        //    StringBuilder sqlCommand = new StringBuilder();

        //    sqlCommand.Append("UPDATE mp_PrivateMessageAttachments ");
        //    sqlCommand.Append("SET  ");
        //    sqlCommand.Append("MessageID = :MessageID, ");
        //    sqlCommand.Append("OriginalFileName = :OriginalFileName, ");
        //    sqlCommand.Append("ServerFileName = :ServerFileName, ");
        //    sqlCommand.Append("CreatedDate = :CreatedDate ");

        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("AttachmentID = :AttachmentID ;");

        //    SqliteParameter[] arParams = new SqliteParameter[5];

        //    arParams[0] = new SqliteParameter(":AttachmentID", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = attachmentID.ToString();

        //    arParams[1] = new SqliteParameter(":MessageID", DbType.String, 36);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = messageID.ToString();

        //    arParams[2] = new SqliteParameter(":OriginalFileName", DbType.String, 255);
        //    arParams[2].Direction = ParameterDirection.Input;
        //    arParams[2].Value = originalFileName;

        //    arParams[3] = new SqliteParameter(":ServerFileName", DbType.String, 50);
        //    arParams[3].Direction = ParameterDirection.Input;
        //    arParams[3].Value = serverFileName;

        //    arParams[4] = new SqliteParameter(":CreatedDate", DbType.DateTime);
        //    arParams[4].Direction = ParameterDirection.Input;
        //    arParams[4].Value = createdDate;


        //    int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
        //    return (rowsAffected > -1);

        //}


        //public static bool PrivateMessageAttachment_DeletePrivateMessageAttachment(
        //    Guid attachmentID)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("DELETE FROM mp_PrivateMessageAttachments ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("AttachmentID = :AttachmentID ;");

        //    SqliteParameter[] arParams = new SqliteParameter[1];

        //    arParams[0] = new SqliteParameter(":AttachmentID", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = attachmentID.ToString();


        //    int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
        //    return (rowsAffected > 0);

        //}


        //public static IDataReader PrivateMessageAttachment_GetPrivateMessageAttachment(
        //    Guid attachmentID)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_PrivateMessageAttachments ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("AttachmentID = :AttachmentID ;");

        //    SqliteParameter[] arParams = new SqliteParameter[1];

        //    arParams[0] = new SqliteParameter(":AttachmentID", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = attachmentID.ToString();

        //    return SqliteHelper.ExecuteReader(
        //        GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        //#endregion


        //#region UserEmailAccount

        //public static int UserEmailAccount_AddUserEmailAccount(
        //    Guid id,
        //    Guid userGuid,
        //    string accountName,
        //    string userName,
        //    string email,
        //    string password,
        //    string pop3Server,
        //    int pop3Port,
        //    string smtpServer,
        //    int smtpPort,
        //    bool useSSL)
        //{
        //    #region Bit Conversion

        //    byte ussl;
        //    if (useSSL)
        //    {
        //        ussl = 1;
        //    }
        //    else
        //    {
        //        ussl = 0;
        //    }

        //    #endregion

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("INSERT INTO mp_UserEmailAccounts (");
        //    sqlCommand.Append("ID, ");
        //    sqlCommand.Append("UserGuid, ");
        //    sqlCommand.Append("AccountName, ");
        //    sqlCommand.Append("UserName, ");
        //    sqlCommand.Append("Email, ");
        //    sqlCommand.Append("Password, ");
        //    sqlCommand.Append("Pop3Server, ");
        //    sqlCommand.Append("Pop3Port, ");
        //    sqlCommand.Append("SmtpServer, ");
        //    sqlCommand.Append("SmtpPort, ");
        //    sqlCommand.Append("UseSSL )");

        //    sqlCommand.Append(" VALUES (");
        //    sqlCommand.Append(":ID, ");
        //    sqlCommand.Append(":UserGuid, ");
        //    sqlCommand.Append(":AccountName, ");
        //    sqlCommand.Append(":UserName, ");
        //    sqlCommand.Append(":Email, ");
        //    sqlCommand.Append(":Password, ");
        //    sqlCommand.Append(":Pop3Server, ");
        //    sqlCommand.Append(":Pop3Port, ");
        //    sqlCommand.Append(":SmtpServer, ");
        //    sqlCommand.Append(":SmtpPort, ");
        //    sqlCommand.Append(":UseSSL );");

        //    SqliteParameter[] arParams = new SqliteParameter[11];

        //    arParams[0] = new SqliteParameter(":ID", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = id.ToString();

        //    arParams[1] = new SqliteParameter(":UserGuid", DbType.String, 36);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = userGuid.ToString();

        //    arParams[2] = new SqliteParameter(":AccountName", DbType.String, 50);
        //    arParams[2].Direction = ParameterDirection.Input;
        //    arParams[2].Value = accountName;

        //    arParams[3] = new SqliteParameter(":UserName", DbType.String, 75);
        //    arParams[3].Direction = ParameterDirection.Input;
        //    arParams[3].Value = userName;

        //    arParams[4] = new SqliteParameter(":Email", DbType.String, 100);
        //    arParams[4].Direction = ParameterDirection.Input;
        //    arParams[4].Value = email;

        //    arParams[5] = new SqliteParameter(":Password", DbType.String, 255);
        //    arParams[5].Direction = ParameterDirection.Input;
        //    arParams[5].Value = password;

        //    arParams[6] = new SqliteParameter(":Pop3Server", DbType.String, 75);
        //    arParams[6].Direction = ParameterDirection.Input;
        //    arParams[6].Value = pop3Server;

        //    arParams[7] = new SqliteParameter(":Pop3Port", DbType.Int32);
        //    arParams[7].Direction = ParameterDirection.Input;
        //    arParams[7].Value = pop3Port;

        //    arParams[8] = new SqliteParameter(":SmtpServer", DbType.String, 75);
        //    arParams[8].Direction = ParameterDirection.Input;
        //    arParams[8].Value = smtpServer;

        //    arParams[9] = new SqliteParameter(":SmtpPort", DbType.Int32);
        //    arParams[9].Direction = ParameterDirection.Input;
        //    arParams[9].Value = smtpPort;

        //    arParams[10] = new SqliteParameter(":UseSSL", DbType.Int32);
        //    arParams[10].Direction = ParameterDirection.Input;
        //    arParams[10].Value = ussl;


        //    int rowsAffected = 0;
        //    rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
        //    return rowsAffected;

        //}




        //public static bool UserEmailAccount_UpdateUserEmailAccount(
        //    Guid id,
        //    string accountName,
        //    string userName,
        //    string email,
        //    string password,
        //    string pop3Server,
        //    int pop3Port,
        //    string smtpServer,
        //    int smtpPort,
        //    bool useSSL)
        //{
        //    #region Bit Conversion

        //    byte ussl;
        //    if (useSSL)
        //    {
        //        ussl = 1;
        //    }
        //    else
        //    {
        //        ussl = 0;
        //    }

        //    #endregion

        //    StringBuilder sqlCommand = new StringBuilder();

        //    sqlCommand.Append("UPDATE mp_UserEmailAccounts ");
        //    sqlCommand.Append("SET  ");
        //    sqlCommand.Append("AccountName = :AccountName, ");
        //    sqlCommand.Append("UserName = :UserName, ");
        //    sqlCommand.Append("Email = :Email, ");
        //    sqlCommand.Append("Password = :Password, ");
        //    sqlCommand.Append("Pop3Server = :Pop3Server, ");
        //    sqlCommand.Append("Pop3Port = :Pop3Port, ");
        //    sqlCommand.Append("SmtpServer = :SmtpServer, ");
        //    sqlCommand.Append("SmtpPort = :SmtpPort, ");
        //    sqlCommand.Append("UseSSL = :UseSSL ");

        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("ID = :ID ;");

        //    SqliteParameter[] arParams = new SqliteParameter[10];

        //    arParams[0] = new SqliteParameter(":ID", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = id.ToString();

        //    arParams[1] = new SqliteParameter(":AccountName", DbType.String, 50);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = accountName;

        //    arParams[2] = new SqliteParameter(":UserName", DbType.String, 75);
        //    arParams[2].Direction = ParameterDirection.Input;
        //    arParams[2].Value = userName;

        //    arParams[3] = new SqliteParameter(":Email", DbType.String, 100);
        //    arParams[3].Direction = ParameterDirection.Input;
        //    arParams[3].Value = email;

        //    arParams[4] = new SqliteParameter(":Password", DbType.String, 255);
        //    arParams[4].Direction = ParameterDirection.Input;
        //    arParams[4].Value = password;

        //    arParams[5] = new SqliteParameter(":Pop3Server", DbType.String, 75);
        //    arParams[5].Direction = ParameterDirection.Input;
        //    arParams[5].Value = pop3Server;

        //    arParams[6] = new SqliteParameter(":Pop3Port", DbType.Int32);
        //    arParams[6].Direction = ParameterDirection.Input;
        //    arParams[6].Value = pop3Port;

        //    arParams[7] = new SqliteParameter(":SmtpServer", DbType.String, 75);
        //    arParams[7].Direction = ParameterDirection.Input;
        //    arParams[7].Value = smtpServer;

        //    arParams[8] = new SqliteParameter(":SmtpPort", DbType.Int32);
        //    arParams[8].Direction = ParameterDirection.Input;
        //    arParams[8].Value = smtpPort;

        //    arParams[9] = new SqliteParameter(":UseSSL", DbType.Int32);
        //    arParams[9].Direction = ParameterDirection.Input;
        //    arParams[9].Value = ussl;


        //    int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
        //    return (rowsAffected > -1);

        //}


        //public static bool UserEmailAccount_DeleteUserEmailAccount(Guid id)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("DELETE FROM mp_UserEmailAccounts ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ID = :ID ;");

        //    SqliteParameter[] arParams = new SqliteParameter[1];

        //    arParams[0] = new SqliteParameter(":ID", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = id.ToString();


        //    int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
        //    return (rowsAffected > 0);

        //}


        //public static IDataReader UserEmailAccount_GetUserEmailAccount(Guid id)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_UserEmailAccounts ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ID = :ID ;");

        //    SqliteParameter[] arParams = new SqliteParameter[1];

        //    arParams[0] = new SqliteParameter(":ID", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = id.ToString();

        //    return SqliteHelper.ExecuteReader(
        //        GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        //public static IDataReader UserEmailAccount_GetUserEmailAccountByUser(Guid userGuid)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_UserEmailAccounts ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("UserGuid = :UserGuid ;");

        //    SqliteParameter[] arParams = new SqliteParameter[1];

        //    arParams[0] = new SqliteParameter(":UserGuid", DbType.String, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = userGuid.ToString();

        //    return SqliteHelper.ExecuteReader(
        //        GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //}


        //#endregion

        

        

    }
}
