/// Author:         Joe Audette
/// Created:        2010-03-09
/// Last Modified   2013-08-19
/// 
/// The use and distribution terms for this software are covered by the
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by
/// the terms of this license.
/// 
/// You must not remove this notice, or any other, from this software.
/// 

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Configuration;
using log4net;

namespace mojoPortal.Data
{
    /// <summary>
    /// This data Facade has all static methods and serves to abstract the underlying database
    /// from the business layer. 
    /// </summary>
    public static class DBPortal
    {
        // Create a logger for use in this class
       private static readonly ILog log = LogManager.GetLogger(typeof(DBPortal));


        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            if (ConfigurationManager.AppSettings["SqlCeApp_Data_FileName"] != null)
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/" + ConfigurationManager.AppSettings["SqlCeApp_Data_FileName"]);
                string connectionString = "Data Source=" + path + ";Persist Security Info=False;";

                return connectionString;
            }

            return ConfigurationManager.AppSettings["SqlCeConnectionString"];
            
        }

        public static void EnsureDatabase()
        {
            try
            {
                if (ConfigurationManager.AppSettings["SqlCeApp_Data_FileName"] != null)
                {
                    string path = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/" + ConfigurationManager.AppSettings["SqlCeApp_Data_FileName"]);
                    string connectionString = "Data Source=" + path + ";Persist Security Info=False;";
                    
                    if (!File.Exists(path))
                    {
                        lock (typeof(DBPortal))
                        {
                            if (!File.Exists(path))
                            {
                                using (SqlCeEngine engine = new SqlCeEngine(connectionString))
                                {
                                    engine.CreateDatabase();
                                }
                            }

                        }

                    }
                    
                }
            }
            catch (Exception ex)
            {
                log.Error("SqlCe database file is not present, tried to create it but this error occurred.", ex);

            }

        }

        /// <summary>
        /// Getdbs the owner prefix.
        /// </summary>
        /// <returns></returns>
        private static string GetdbOwnerPrefix()
        {
            string ownerPrefix = "[dbo].";
            if (ConfigurationManager.AppSettings["MSSQLOwnerPrefix"] != null)
            {
                ownerPrefix = ConfigurationManager.AppSettings["MSSQLOwnerPrefix"];

            }

            return ownerPrefix;
        }

        /// <summary>
        /// Gets the database platform.
        /// </summary>
        /// <returns></returns>
        public static String DBPlatform()
        {
            return "SqlCe";
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SchemaVersion ");
            sqlCommand.Append("(");
            sqlCommand.Append("ApplicationID, ");
            sqlCommand.Append("ApplicationName, ");
            sqlCommand.Append("Major, ");
            sqlCommand.Append("Minor, ");
            sqlCommand.Append("Build, ");
            sqlCommand.Append("Revision ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ApplicationID, ");
            sqlCommand.Append("@ApplicationName, ");
            sqlCommand.Append("@Major, ");
            sqlCommand.Append("@Minor, ");
            sqlCommand.Append("@Build, ");
            sqlCommand.Append("@Revision ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[6];

            arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId;

            arParams[1] = new SqlCeParameter("@ApplicationName", SqlDbType.NVarChar);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = applicationName;

            arParams[2] = new SqlCeParameter("@Major", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = major;

            arParams[3] = new SqlCeParameter("@Minor", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = minor;

            arParams[4] = new SqlCeParameter("@Build", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = build;

            arParams[5] = new SqlCeParameter("@Revision", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = revision;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_SchemaVersion ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("ApplicationName = @ApplicationName, ");
            sqlCommand.Append("Major = @Major, ");
            sqlCommand.Append("Minor = @Minor, ");
            sqlCommand.Append("Build = @Build, ");
            sqlCommand.Append("Revision = @Revision ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ApplicationID = @ApplicationID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[6];

            arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId;

            arParams[1] = new SqlCeParameter("@ApplicationName", SqlDbType.NVarChar);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = applicationName;

            arParams[2] = new SqlCeParameter("@Major", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = major;

            arParams[3] = new SqlCeParameter("@Minor", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = minor;

            arParams[4] = new SqlCeParameter("@Build", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = build;

            arParams[5] = new SqlCeParameter("@Revision", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = revision;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        public static bool SchemaVersionDeleteSchemaVersion(Guid applicationId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SchemaVersion ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ApplicationID = @ApplicationID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
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



        public static IDataReader SchemaVersionGetSchemaVersion(Guid applicationId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SchemaVersion ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ApplicationID = @ApplicationID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SchemaScriptHistory ");
            sqlCommand.Append("(");
            sqlCommand.Append("ApplicationID, ");
            sqlCommand.Append("ScriptFile, ");
            sqlCommand.Append("RunTime, ");
            sqlCommand.Append("ErrorOccurred, ");
            sqlCommand.Append("ErrorMessage, ");
            sqlCommand.Append("ScriptBody ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ApplicationID, ");
            sqlCommand.Append("@ScriptFile, ");
            sqlCommand.Append("@RunTime, ");
            sqlCommand.Append("@ErrorOccurred, ");
            sqlCommand.Append("@ErrorMessage, ");
            sqlCommand.Append("@ScriptBody ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[6];

            arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId;

            arParams[1] = new SqlCeParameter("@ScriptFile", SqlDbType.NVarChar);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = scriptFile;

            arParams[2] = new SqlCeParameter("@RunTime", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = runTime;

            arParams[3] = new SqlCeParameter("@ErrorOccurred", SqlDbType.Bit);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = errorOccurred;

            arParams[4] = new SqlCeParameter("@ErrorMessage", SqlDbType.NText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = errorMessage;

            arParams[5] = new SqlCeParameter("@ScriptBody", SqlDbType.NText);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = scriptBody;


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            //log.Info("Identity was " + newId.ToString());

            return newId;
        }

        public static bool SchemaScriptHistoryDeleteSchemaScriptHistory(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SchemaScriptHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ID = @ID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static IDataReader SchemaScriptHistoryGetSchemaScriptHistory(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SchemaScriptHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ID = @ID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader SchemaScriptHistoryGetSchemaScriptHistory(Guid applicationId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SchemaScriptHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ApplicationID = @ApplicationID ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("ID  ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader SchemaScriptHistoryGetSchemaScriptErrorHistory(Guid applicationId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SchemaScriptHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ApplicationID = @ApplicationID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ErrorOccurred = 1 ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("ID  ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public static bool SchemaScriptHistoryExists(Guid applicationId, String scriptFile)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_SchemaScriptHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ApplicationID = @ApplicationID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ScriptFile = @ScriptFile ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId;

            arParams[1] = new SqlCeParameter("@ScriptFile", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = scriptFile;

            int count =  Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
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
            DataColumn column;
            DataRow row;
            ArrayList arrayList = new ArrayList();

            try
            {
                DataTable schemaTable = reader.GetSchemaTable();

                for (int i = 0; i < schemaTable.Rows.Count; i++)
                {

                    column = new DataColumn();

                    if (!dataTable.Columns.Contains(schemaTable.Rows[i]["ColumnName"].ToString()))
                    {

                        column.ColumnName = schemaTable.Rows[i]["ColumnName"].ToString();
                        // we don't always want to enforce constrainnts, it may be fine to have duplicates in a query even if the underlying table has a unique constraint
                        //if (schemaTable.Rows[i]["IsUnique"] != DBNull.Value)
                        //{
                        //    column.Unique = Convert.ToBoolean(schemaTable.Rows[i]["IsUnique"]);
                        //}
                        if (schemaTable.Rows[i]["AllowDBNull"] != DBNull.Value)
                        {
                            column.AllowDBNull = Convert.ToBoolean(schemaTable.Rows[i]["AllowDBNull"]);
                        }
                        if (schemaTable.Rows[i]["IsReadOnly"] != DBNull.Value)
                        {
                            column.ReadOnly = Convert.ToBoolean(schemaTable.Rows[i]["IsReadOnly"]);
                        }
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

        public static bool DatabaseHelperCanAccessDatabase(String overrideConnectionInfo)
        {
            // TODO: FxCop says not to swallow nonspecific exceptions
            // need to find all possible exceptions that could happen here and
            // catch them specifically
            // ultimately we want to return false on any exception

            bool result = false;

            SqlCeConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new SqlCeConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new SqlCeConnection(GetConnectionString());
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

        public static DbException DatabaseHelperGetConnectionError(String overrideConnectionInfo)
        {
            DbException exception = null;

            SqlCeConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new SqlCeConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new SqlCeConnection(GetConnectionString());
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

        public static bool DatabaseHelperCanAccessDatabase()
        {
            return DatabaseHelperCanAccessDatabase(null);
        }

        public static bool DatabaseHelperCanAlterSchema(String overrideConnectionInfo)
        {

            bool result = true;
            // Make sure we can create, alter and drop tables

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append(
                @" 
                CREATE TABLE [mp_Testdb](
                  [FooID] [int] IDENTITY(1,1) NOT NULL,
                  [Foo] [nvarchar](255) NOT NULL,
                 CONSTRAINT [PK_mp_Testdb] PRIMARY KEY 
                (
                  [FooID] 
                ) 
                ) 
                GO
                ");

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

            sqlCommand = new StringBuilder();
            sqlCommand.Append(
                @"ALTER TABLE [mp_Testdb] ADD
                [MoreFoo] [nvarchar] (255)  NULL
                GO
                ");

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

            sqlCommand = new StringBuilder();
            sqlCommand.Append(
                @"DROP TABLE [mp_Testdb] 
                GO
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



            return result;

        }

        public static bool DatabaseHelperCanCreateTemporaryTables()
        {
            bool result = false;

//            StringBuilder sqlCommand = new StringBuilder();
//            sqlCommand.Append(
//                @"SET ANSI_NULLS ON
//                GO
//                SET QUOTED_IDENTIFIER ON
//                GO
//                CREATE TABLE #Test 
//                (
//                  IndexID int IDENTITY (1, 1) NOT NULL,
//                  UserName nvarchar(50),
//                  LoginName nvarchar(50)
//                )
//                DROP TABLE #Test
//                GO
//                  
//                ");

//            try
//            {
//                DatabaseHelperRunScript(sqlCommand.ToString(), GetConnectionString());
//                result = true;
//            }
//            catch
//            {
//                result = false;
//            }


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

            string ownerPrefix = GetdbOwnerPrefix();
            if (ownerPrefix != "[dbo].")
            {
                script = script.Replace("[dbo].", ownerPrefix);
            }

            bool result = false;
            SqlCeConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new SqlCeConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new SqlCeConnection(GetConnectionString());
            }

            string[] delimiter = new string[] { "GO\r\n" };

            script = script.Replace("GO", "GO\r\n");

            string[] arrSqlStatements = script.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

            connection.Open();

            //string[] subdelimiter = new string[] { ";" };

            SqlCeTransaction transaction = connection.BeginTransaction();
            string currentStatement = string.Empty;
            try
            {
                foreach (string sqlStatement in arrSqlStatements)
                {
                    //string[] subStatements = s.Split(subdelimiter, StringSplitOptions.RemoveEmptyEntries);

                    //foreach (string sqlStatement in subStatements)
                    //{
                        if (sqlStatement.Trim().Length > 0)
                        {
                            currentStatement = sqlStatement;
                            SqlHelper.ExecuteNonQuery(
                                transaction,
                                CommandType.Text,
                                sqlStatement,
                                null);

                        }
                   // }
                }


                transaction.Commit();
                result = true;

            }
            catch (SqlCeException ex)
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
            sqlCommand.Append(" SET " + dataFieldName + " = @fieldValue ");
            sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
            sqlCommand.Append(" " + additionalWhere + " ");
            sqlCommand.Append("  ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@fieldValue", SqlDbType.Text);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = dataFieldValue;

           
            int rowsAffected = SqlHelper.ExecuteNonQuery(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString(), 
                arParams);

            result = (rowsAffected > 0);

           

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
            sqlCommand.Append(" SET " + dataFieldName + " = @fieldValue ");
            sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
            sqlCommand.Append(" " + additionalWhere + " ");
            sqlCommand.Append("  ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@fieldValue", SqlDbType.Text);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = dataFieldValue;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(), arParams);

            result = (rowsAffected > 0);

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
            sqlCommand.Append("  ");
            sqlCommand.Append(";");

            return SqlHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString());

        }

        public static IDataReader DatabaseHelperGetReader(
            string connectionString,
            string query
            )
        {
            if (string.IsNullOrEmpty(connectionString)) { connectionString = GetConnectionString(); }

            return SqlHelper.ExecuteReader(
                connectionString,
                CommandType.Text,
                query);

        }

        public static int DatabaseHelperExecteNonQuery(
            string connectionString,
            string query
            )
        {
            if (string.IsNullOrEmpty(connectionString)) { connectionString = GetConnectionString(); }

            int rowsAffected = SqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM " + tableName + " ");
            sqlCommand.Append(whereClause);
            sqlCommand.Append("  ");
            sqlCommand.Append(";");

            DataSet ds = SqlHelper.ExecuteDataset(
                connectionString,
                CommandType.Text,
                sqlCommand.ToString());

            return ds.Tables[0];

        }

        public static void DatabaseHelperDoForumVersion2202PostUpgradeTasks(
            String overrideConnectionInfo)
        {


        }

        public static void DatabaseHelperDoForumVersion2203PostUpgradeTasks(
            String overrideConnectionInfo)
        {

        }

        public static void DatabaseHelperDoVersion2320PostUpgradeTasks(
            String overrideConnectionInfo)
        {


        }

        public static void DatabaseHelperDoVersion2230PostUpgradeTasks(String overrideConnectionInfo)
        {
        }

        public static void DatabaseHelperDoVersion2234PostUpgradeTasks(String overrideConnectionInfo)
        {
        }

        public static void DatabaseHelperDoVersion2247PostUpgradeTasks(String overrideConnectionInfo)
        {
        }

        public static void DatabaseHelperDoVersion2253PostUpgradeTasks(
            String overrideConnectionInfo)
        {


        }

        public static bool DatabaseHelperSitesTableExists()
        {
            return DatabaseHelperTableExists("mp_Sites");
        }

        public static bool DatabaseHelperTableExists(string tableName)
        {
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	INFORMATION_SCHEMA.TABLES ");
            sqlCommand.Append("WHERE TABLE_NAME = @TableName ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@TableName", SqlDbType.NVarChar, 100);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = tableName;

            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);
        }


        


        


        #endregion


    }
}
