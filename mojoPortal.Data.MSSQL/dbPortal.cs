/// Author:        
/// Created:      2004-07-19
/// The use and distribution terms for this software are covered by the
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by
/// the terms of this license.
/// You must not remove this notice, or any other, from this software.
/// 2007/04/29 column names shortened to 31 chars or less as required by Firebird Sql
/// mp_Users rename columns
/// FailedPasswordAnswerAttemptCount &gt; FailedPwdAnswerAttemptCount 27
/// FailedPasswordAnswerAttemptWindowStart &gt; FailedPwdAnswerWindowStart 26
/// FailedPasswordAttemptWindowStart &gt; FailedPwdAttemptWindowStart 27
/// mp_Sites rename columns
/// MinRequiredNonAlphanumericCharacters &gt; MinReqNonAlphaChars 19
/// PasswordStrengthRegularExpression &gt; PwdStrengthRegex 16
/// 
/// 2007-11-03 moved most code to feature specific class files
/// Last Modified:    2009-02-01


using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
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

        
        ///// <summary>
        ///// Gets the connection string.
        ///// </summary>
        ///// <returns></returns>
        //private static string GetConnectionString()
        //{
        //    return ConfigurationManager.AppSettings["MSSQLConnectionString"];
        //    //return ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString;
        //}

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

        public static void EnsureDatabase()
        {
            //logic added by Luis Silva 2012-06-13
            // only works if connection string has been configured and if you add this to user.config or Webc.onfig:
            // <add key="TryToCreateMsSqlDatabase" value="true"/>

            SqlConnection connection = new SqlConnection(ConnectionString.GetWriteConnectionString());
            SqlConnection MasterConnection = new SqlConnection();

            try
            {
                SqlConnectionStringBuilder master = new SqlConnectionStringBuilder();
                master.ConnectionString = connection.ConnectionString;
                master["server"] = connection.DataSource;
                master["database"] = "master";

                MasterConnection = new SqlConnection(master.ConnectionString);

                StringBuilder sql = new StringBuilder();
                sql.Append("IF not EXISTS (SELECT name FROM sys.databases WHERE name = @Name) CREATE DATABASE ");
                sql.Append(connection.Database);
                sql.Append(" else select 1");

                SqlCommand command = new SqlCommand(sql.ToString(), MasterConnection);
                command.Parameters.AddWithValue("@Name", connection.Database);
                MasterConnection.Open();
                int res = command.ExecuteNonQuery();

                if (res == -1) 
                { log.Info("Successfully created MS SQL Database"); }
                else 
                { log.Info("Failed to Create MS SQL Database"); }

            }
            catch(Exception ex) 
            {
                log.Error("Failed to Create MS SQL Database", ex);
            }
            finally
            {
                if (MasterConnection.State == ConnectionState.Open)
                    MasterConnection.Close();
            }

        }

        /// <summary>
        /// Gets the database platform.
        /// </summary>
        /// <returns></returns>
        public static String DBPlatform()
        {
            return "MSSQL";
        }

        #region Versioning and Upgrade Helpers


        #region Schema Table Methods

        public static IDataReader DatabaseHelperGetApplicationId(string applicationName)
        {
            return DatabaseHelperGetReader(
                ConnectionString.GetReadConnectionString(),
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SchemaVersion_Insert", 6);
            sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
            sph.DefineSqlParameter("@ApplicationName", SqlDbType.NVarChar, ParameterDirection.Input, applicationName);
            sph.DefineSqlParameter("@Major", SqlDbType.Int, ParameterDirection.Input, major);
            sph.DefineSqlParameter("@Minor", SqlDbType.Int, ParameterDirection.Input, minor);
            sph.DefineSqlParameter("@Build", SqlDbType.Int, ParameterDirection.Input, build);
            sph.DefineSqlParameter("@Revision", SqlDbType.Int, ParameterDirection.Input, revision);
            int rowsAffected = Convert.ToInt32(sph.ExecuteNonQuery());
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SchemaVersion_Update", 6);
            sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
            sph.DefineSqlParameter("@ApplicationName", SqlDbType.NVarChar, ParameterDirection.Input, applicationName);
            sph.DefineSqlParameter("@Major", SqlDbType.Int, ParameterDirection.Input, major);
            sph.DefineSqlParameter("@Minor", SqlDbType.Int, ParameterDirection.Input, minor);
            sph.DefineSqlParameter("@Build", SqlDbType.Int, ParameterDirection.Input, build);
            sph.DefineSqlParameter("@Revision", SqlDbType.Int, ParameterDirection.Input, revision);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool SchemaVersionDeleteSchemaVersion(Guid applicationId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SchemaVersion_Delete", 1);
            sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
            int rowsAffected = sph.ExecuteNonQuery();
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

        public static bool SchemaScriptHistoryDeleteSchemaScriptHistory(int id)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SchemaScriptHistory_Delete", 1);
            sph.DefineSqlParameter("@ID", SqlDbType.Int, ParameterDirection.Input, id);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader SchemaScriptHistoryGetSchemaScriptHistory(int id)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SchemaScriptHistory_SelectOne", 1);
            sph.DefineSqlParameter("@ID", SqlDbType.Int, ParameterDirection.Input, id);
            return sph.ExecuteReader();
        }

        public static IDataReader SchemaScriptHistoryGetSchemaScriptHistory(Guid applicationId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SchemaScriptHistory_SelectByApp", 1);
            sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
            return sph.ExecuteReader();
        }

        public static IDataReader SchemaScriptHistoryGetSchemaScriptErrorHistory(Guid applicationId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SchemaScriptHistory_SelectErrorsByApp", 1);
            sph.DefineSqlParameter("@ID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
            return sph.ExecuteReader();
        }

        public static bool SchemaScriptHistoryExists(Guid applicationId, String scriptFile)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SchemaScriptHistory_Exists", 2);
            sph.DefineSqlParameter("@ApplicationID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, applicationId);
            sph.DefineSqlParameter("@ScriptFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, scriptFile);
            int count = Convert.ToInt32(sph.ExecuteScalar());
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

        public static bool DatabaseHelperCanAccessDatabase(String overrideConnectionInfo)
        {
            // TODO: FxCop says not to swallow nonspecific exceptions
            // need to find all possible exceptions that could happen here and
            // catch them specifically
            // ultimately we want to return false on any exception

            bool result = false;

            SqlConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
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

            SqlConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
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
                    connection.Close();
            }


            return exception;

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
//            sqlCommand.Append(
//                @"SET ANSI_NULLS ON
//                GO
//                SET QUOTED_IDENTIFIER ON
//                GO
//                IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Testdb]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
//                BEGIN
//                CREATE TABLE [dbo].[mp_Testdb](
//                  [FooID] [int] IDENTITY(1,1) NOT NULL,
//                  [Foo] [nvarchar](255) NOT NULL,
//                 CONSTRAINT [PK_mp_Testdb] PRIMARY KEY CLUSTERED 
//                (
//                  [FooID] ASC
//                ) ON [PRIMARY]
//                ) ON [PRIMARY]
//                END
//                GO
//                ");

//            sqlCommand.Append(
//               @"SET QUOTED_IDENTIFIER ON
//                GO
//                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mp_Testdb]') AND type in (N'U'))
//                BEGIN
//                CREATE TABLE [dbo].[mp_Testdb](
//                  [FooID] [int] IDENTITY(1,1) NOT NULL,
//                  [Foo] [nvarchar](255) NOT NULL,
//                 CONSTRAINT [PK_mp_Testdb] PRIMARY KEY CLUSTERED 
//                (
//                  [FooID] ASC
//                ) WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
//                ) 
//                END
//                GO
//                ");

            sqlCommand.Append(
               @"SET QUOTED_IDENTIFIER ON
                GO
                CREATE TABLE [dbo].[mp_Testdb](
                  [FooID] [int] IDENTITY(1,1) NOT NULL,
                  [Foo] [nvarchar](255) NOT NULL,
                 CONSTRAINT [PK_mp_Testdb] PRIMARY KEY CLUSTERED 
                (
                  [FooID] ASC
                ) WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
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
                @"ALTER TABLE [dbo].[mp_Testdb] ADD
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
                @"DROP TABLE [dbo].[mp_Testdb] 
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

            //if (!result)
            //{
            //    if (DatabaseHelperCanAlterSchemaHackForMissingServicePak(overrideConnectionInfo)) { result = true; }
            //}


            return result;

        }

        private static bool DatabaseHelperCanAlterSchemaHackForMissingServicePak(string overrideConnectionInfo)
        {

            bool result = true;
            // Make sure we can create, alter and drop tables

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append(
                @"SET ANSI_NULLS ON
                            GO
                            SET QUOTED_IDENTIFIER ON
                            GO
                            IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Testdb]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
                            BEGIN
                            CREATE TABLE [dbo].[mp_Testdb](
                              [FooID] [int] IDENTITY(1,1) NOT NULL,
                              [Foo] [nvarchar](255) NOT NULL,
                             CONSTRAINT [PK_mp_Testdb] PRIMARY KEY CLUSTERED 
                            (
                              [FooID] ASC
                            ) ON [PRIMARY]
                            ) ON [PRIMARY]
                            END
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
                @"ALTER TABLE [dbo].[mp_Testdb] ADD
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
                @"DROP TABLE [dbo].[mp_Testdb] 
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append(
                @"SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
                CREATE TABLE #Test 
                (
                  IndexID int IDENTITY (1, 1) NOT NULL,
                  UserName nvarchar(50),
                  LoginName nvarchar(50)
                )
                DROP TABLE #Test
                GO
                  
                ");

            try
            {
                DatabaseHelperRunScript(sqlCommand.ToString(), ConnectionString.GetWriteConnectionString());
                result = true;
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


        public static bool DatabaseHelperRunScript(string script, string overrideConnectionInfo)
        {
            if ((script == null) || (script.Length == 0)) return true;

            string ownerPrefix = GetdbOwnerPrefix();
            if (ownerPrefix != "[dbo].")
            {
                script = script.Replace("[dbo].", ownerPrefix);
            }

            bool result = false;
            SqlConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new SqlConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new SqlConnection(ConnectionString.GetWriteConnectionString());
            }

            string[] delimiter = new string[] { "GO\r\n" };

            script = script.Replace("GO", "GO\r\n");

            string[] arrSqlStatements = script.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

            connection.Open();

            SqlTransaction transaction = connection.BeginTransaction();
            string currentStatement = string.Empty;

            try
            {
                foreach (String sqlStatement in arrSqlStatements)
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

        //public static bool DatabaseHelperUpdateTableField(
        //    String connectionString,
        //    String tableName,
        //    String keyFieldName,
        //    String keyFieldValue,
        //    String dataFieldName,
        //    String dataFieldValue,
        //    String additionalWhere)
        //{
        //    bool result = false;

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE " + tableName + " ");
        //    sqlCommand.Append(" SET " + dataFieldName + " = @fieldValue ");
        //    sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
        //    sqlCommand.Append(" " + additionalWhere + " ");
        //    sqlCommand.Append("  ");

        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), sqlCommand.ToString(), CommandType.Text, 1);
        //    sph.DefineSqlParameter("@fieldValue", SqlDbType.NText, ParameterDirection.Input, dataFieldValue);

        //    SqlConnection connection = new SqlConnection(connectionString);
        //    connection.Open();
        //    try
        //    {
        //        int rowsAffected = sph.ExecuteNonQuery();
        //        result = (rowsAffected > 0);

        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //    return result;

        //}

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

            SqlParameter[] arParams = new SqlParameter[1];

            arParams[0] = new SqlParameter("@fieldValue", SqlDbType.Text);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = dataFieldValue;

            //SqlConnection connection = new SqlConnection(connectionString);
            //connection.Open();
            //try
            //{
                int rowsAffected = SqlHelper.ExecuteNonQuery(
                    connectionString,
                    CommandType.Text,
                    sqlCommand.ToString(), arParams);

                result = (rowsAffected > 0);

            //}
            //finally
            //{
            //    connection.Close();
            //}

            return result;

        }

        //public static bool DatabaseHelperUpdateTableField(
        //    String tableName,
        //    String keyFieldName,
        //    String keyFieldValue,
        //    String dataFieldName,
        //    String dataFieldValue,
        //    String additionalWhere)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE " + tableName + " ");
        //    sqlCommand.Append(" SET " + dataFieldName + " = @fieldValue ");
        //    sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
        //    sqlCommand.Append(" " + additionalWhere + " ");
        //    sqlCommand.Append("  ");

        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), sqlCommand.ToString(), CommandType.Text, 1);
        //    sph.DefineSqlParameter("@fieldValue", SqlDbType.NText, ParameterDirection.Input, dataFieldValue);

        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > 0);
        //}

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

            SqlParameter[] arParams = new SqlParameter[1];

            arParams[0] = new SqlParameter("@fieldValue", SqlDbType.Text);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = dataFieldValue;

            //SqlConnection connection = new SqlConnection(GetConnectionString());
            //connection.Open();
            //try
            //{
                int rowsAffected = SqlHelper.ExecuteNonQuery(
                    ConnectionString.GetWriteConnectionString(),
                    CommandType.Text,
                    sqlCommand.ToString(), arParams);

                result = (rowsAffected > 0);

            //}
            //finally
            //{
            //    connection.Close();
            //}

            return result;

        }

        //public static IDataReader DatabaseHelperGetReader(
        //    String connectionString,
        //    String tableName,
        //    String whereClause)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT * ");
        //    sqlCommand.Append("FROM " + tableName + " ");
        //    sqlCommand.Append(whereClause);
        //    sqlCommand.Append("  ");

        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), sqlCommand.ToString(), CommandType.Text, 0);
        //    return sph.ExecuteReader();
        //}

        //public static DataTable DatabaseHelperGetTable(
        //    String connectionString,
        //    String tableName,
        //    String whereClause)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT * ");
        //    sqlCommand.Append("FROM " + tableName + " ");
        //    sqlCommand.Append(whereClause);
        //    sqlCommand.Append("  ");

        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), sqlCommand.ToString(), CommandType.Text, 0);
        //    DataSet ds = sph.ExecuteDataset();
        //    return ds.Tables[0];
        //}

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
            if (string.IsNullOrEmpty(connectionString)) { connectionString = ConnectionString.GetReadConnectionString(); }
           
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
            if (string.IsNullOrEmpty(connectionString)) { connectionString = ConnectionString.GetWriteConnectionString(); }

            int rowsAffected =  SqlHelper.ExecuteNonQuery(
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
            //return mojoPortal.Data.Common.DBPortal.DatabaseHelperTableExists(tableName);

            using (SqlConnection connection = new SqlConnection(ConnectionString.GetReadConnectionString()))
            {
                string[] restrictions = new string[4];
                restrictions[2] = tableName;
                connection.Open();
                DataTable table = connection.GetSchema("Tables", restrictions);
                if (table != null)
                {
                    return (table.Rows.Count > 0);
                }
            }
            
            return false;
        }


        



        /*
        
        public DataTable GetLinkedServerList()
          {
              StringBuilder sqlCommand = new StringBuilder();
              sqlCommand.Append("EXEC sp_linkedservers  ");
           
              DataSet ds = sph.ExecuteDataset();
     * {
                      this._cnnString,
                      CommandType.Text,
                      sqlCommand.ToString());
     * }  
              if (ds.Tables.Count > 0)
              {
                  return ds.Tables[0];
              }
  
              return null;
  
  
          }
  
          public DataTable GetCatalogList(String serverName)
          {
              StringBuilder sqlCommand = new StringBuilder();
              sqlCommand.Append("SELECT CATALOG_NAME  ");
              sqlCommand.Append("FROM " + serverName + ".master.INFORMATION_SCHEMA.SCHEMATA ");
              sqlCommand.Append("WHERE CATALOG_NAME NOT IN ('master', 'tempdb', 'msdb', 'model') ");
              sqlCommand.Append("ORDER BY CATALOG_NAME ");
  
              DataSet ds = sph.ExecuteDataset();
     * {
                      this._cnnString,
                      CommandType.Text,
                      sqlCommand.ToString());
     * }  
              if (ds.Tables.Count > 0)
              {
                  return ds.Tables[0];
              }
  
              return null;
  
  
          }
  
          public DataTable GetTableList(String catalogName)
          {
              StringBuilder sqlCommand = new StringBuilder();
              sqlCommand.Append("SELECT TABLE_NAME  ");
              sqlCommand.Append("FROM INFORMATION_SCHEMA.TABLES ");
              sqlCommand.Append("WHERE TABLE_CATALOG = @catalogName ");
              sqlCommand.Append("ORDER BY TABLE_NAME ");
  
              SqlParameter[] arParams = new SxqlParameter[1];
  
              arParams[0] = new SxqlParameter("@catalogName", SqlDbType.NVarChar, 128);
              arParams[0].Direction = ParameterDirection.Input;
              arParams[0].Value = catalogName;
  
              DataSet ds = sph.ExecuteDataset();
     * {
                      this._cnnString,
                      CommandType.Text,
                      sqlCommand.ToString(),
                      arParams);
     * }  
              if (ds.Tables.Count > 0)
              {
                  return ds.Tables[0];
              }
  
              return null;
  
  
          }
  
          public DataTable GetColumnList(String catalogName, String tableName)
          {
              StringBuilder sqlCommand = new StringBuilder();
              sqlCommand.Append("SELECT COLUMN_NAME  ");
              sqlCommand.Append("FROM INFORMATION_SCHEMA.COLUMNS ");
              sqlCommand.Append("WHERE TABLE_CATALOG = @catalogName ");
              sqlCommand.Append("AND TABLE_NAME = @tableName ");
              sqlCommand.Append("ORDER BY COLUMN_NAME ");
  
              SqlParameter[] arParams = new SxqlParameter[2];
  
              arParams[0] = new SxqlParameter("@catalogName", SqlDbType.NVarChar, 128);
              arParams[0].Direction = ParameterDirection.Input;
              arParams[0].Value = catalogName;
  
              arParams[1] = new SxqlParameter("@tableName", SqlDbType.NVarChar, 128);
              arParams[1].Direction = ParameterDirection.Input;
              arParams[1].Value = tableName;
  
              DataSet ds = sph.ExecuteDataset();
     * {
                      this._cnnString,
                      CommandType.Text,
                      sqlCommand.ToString(),
                      arParams);
     * }  
              if (ds.Tables.Count > 0)
              {
                  return ds.Tables[0];
              }
  
              return null;
  
  
        }
        
        
        
        */


        #endregion

       
    }
}
