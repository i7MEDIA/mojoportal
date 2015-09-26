/// Author:					Joe Audette
/// Created:				2007-07-17
/// Last Modified:		    2013-08-19
/// 
/// This data Facade has all static methods and serves to abstract the underlying database
/// from the business layer. To port this application to another database platform, you only have to
/// replace this class. Make a copy of it and change the implementation to support your db of choice, 
/// then compile and replace the original mojoPortal.Data.dll
/// 
/// This implementation is for Firebird. 
/// 
/// I based this partly on work done by Gareth Goslett who 
/// implemented firebird in mojoPortal 1.x version but mainly on 
/// the MySql data layer.
/// 
/// For inserts that return an integer identity I found I had to use
/// stored procedures to be able to get the new id as a return value.
/// Most other methods are just implemented with sql statements.
/// Perhaps Gareth or some other Firebird gurus will make improvements 
/// to use more stored procedures, though I suspect
/// the performance improvements would be relatively small and hard to 
/// measure except perhaps under very heavy load.
/// 
/// Still, part of the benefit of using a true data layer instead of
/// ORM approaches is that each data layer can be optimised for the 
/// db platform. I don't claim this initial implementation to be
/// optimal though I think it is reasonably well implemented. 
/// In order to implement it quickly I took advantage of similar code
/// from the existing MySql data layer.
/// 

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using FirebirdSql.Data.FirebirdClient;
using log4net;


namespace mojoPortal.Data
{
   
    public static class DBPortal
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(DBPortal));

        public static String DBPlatform()
        {
            return "FirebirdSql";
        }

        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

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
                GetConnectionString(),
                "MP_SCHEMAVERSION",
                " WHERE UPPER(ApplicationName) = '" + applicationName.ToUpper() + "'");

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
            sqlCommand.Append("@ApplicationID, ");
            sqlCommand.Append("@ApplicationName, ");
            sqlCommand.Append("@Major, ");
            sqlCommand.Append("@Minor, ");
            sqlCommand.Append("@Build, ");
            sqlCommand.Append("@Revision );");


            FbParameter[] arParams = new FbParameter[6];

            arParams[0] = new FbParameter("@ApplicationID", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            arParams[1] = new FbParameter("@ApplicationName", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = applicationName;

            arParams[2] = new FbParameter("@Major", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = major;

            arParams[3] = new FbParameter("@Minor", FbDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = minor;

            arParams[4] = new FbParameter("@Build", FbDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = build;

            arParams[5] = new FbParameter("@Revision", FbDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = revision;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("ApplicationName = @ApplicationName, ");
            sqlCommand.Append("Major = @Major, ");
            sqlCommand.Append("Minor = @Minor, ");
            sqlCommand.Append("Build = @Build, ");
            sqlCommand.Append("Revision = @Revision ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ApplicationID = @ApplicationID ;");

            FbParameter[] arParams = new FbParameter[6];

            arParams[0] = new FbParameter("@ApplicationID", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            arParams[1] = new FbParameter("@ApplicationName", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = applicationName;

            arParams[2] = new FbParameter("@Major", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = major;

            arParams[3] = new FbParameter("@Minor", FbDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = minor;

            arParams[4] = new FbParameter("@Build", FbDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = build;

            arParams[5] = new FbParameter("@Revision", FbDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = revision;


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);

            return (rowsAffected > -1);

        }


        public static bool SchemaVersionDeleteSchemaVersion(
            Guid applicationId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SchemaVersion ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ApplicationID = @ApplicationID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ApplicationID", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);

            return (rowsAffected > 0);

        }


        public static IDataReader SchemaVersionGetSchemaVersion(
            Guid applicationId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SchemaVersion ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ApplicationID = @ApplicationID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ApplicationID", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            return FBSqlHelper.ExecuteReader(
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

            return FBSqlHelper.ExecuteReader(
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

            FbParameter[] arParams = new FbParameter[6];

            arParams[0] = new FbParameter(":ApplicationID", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            arParams[1] = new FbParameter(":ScriptFile", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = scriptFile;

            arParams[2] = new FbParameter(":RunTime", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = runTime;

            arParams[3] = new FbParameter(":ErrorOccurred", FbDbType.SmallInt);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = intErrorOccurred;

            arParams[4] = new FbParameter(":ErrorMessage", FbDbType.VarChar);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = errorMessage;

            arParams[5] = new FbParameter(":ScriptBody", FbDbType.VarChar);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = scriptBody;

            int newID = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_SCHEMASCRIPTHISTORY_INSERT ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
                arParams));

            return newID;

        }

        public static bool SchemaScriptHistoryDeleteSchemaScriptHistory(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SchemaScriptHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ID = @ID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        public static IDataReader SchemaScriptHistoryGetSchemaScriptHistory(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SchemaScriptHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ID = @ID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            return FBSqlHelper.ExecuteReader(
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
            sqlCommand.Append("ApplicationID = @ApplicationID ");
            //sqlCommand.Append("AND ErrorOccurred = 0 ");

            sqlCommand.Append(" ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ApplicationID", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            return FBSqlHelper.ExecuteReader(
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
            sqlCommand.Append("ApplicationID = @ApplicationID ");
            sqlCommand.Append("AND ErrorOccurred = 1 ");

            sqlCommand.Append(" ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ApplicationID", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            return FBSqlHelper.ExecuteReader(
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
            sqlCommand.Append("ApplicationID = @ApplicationID ");
            sqlCommand.Append("AND ScriptFile = @ScriptFile ");

            sqlCommand.Append(" ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ApplicationID", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = applicationId.ToString();

            arParams[1] = new FbParameter("@ScriptFile", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = scriptFile;

            int count = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
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
                        column.Unique = Convert.ToBoolean(schemaTable.Rows[i]["IsUnique"]);
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

            FbConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new FbConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new FbConnection(GetConnectionString());
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

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="overrideConnectionInfo"></param>
        /// <returns></returns>
        public static bool DatabaseHelperCanAccessDatabase(String overrideConnectionInfo)
        {
            // TODO: FXCop says we should not swallow unspecific exceptions

            bool result = false;

            FbConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new FbConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new FbConnection(GetConnectionString());
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
                CREATE TABLE MP_TESTDB (
                  FOOID INTEGER NOT NULL ,
                  FOO VARCHAR(255) NOT NULL ,
                  PRIMARY KEY (FOOID)
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
            


            sqlCommand = new StringBuilder();
            sqlCommand.Append("ALTER TABLE MP_TESTDB ADD MOREFOO varchar(255) ;");

            try
            {
                DatabaseHelperRunScript(sqlCommand.ToString(), overrideConnectionInfo);
            }
            catch (DbException)
            {
                result = false;
            }
            

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DROP TABLE MP_TESTDB;");

            try
            {
                DatabaseHelperRunScript(sqlCommand.ToString(), overrideConnectionInfo);
            }
            catch (DbException)
            {
                result = false;
            }
            
            return result;
        }

        public static bool DatabaseHelperCanCreateTemporaryTables()
        {
            // TODO: no temp tables supported, but currently not needed
            return true;
        }

        public static bool DatabaseHelperRunScript(
            FileInfo scriptFile, 
            String overrideConnectionInfo)
        {
            if (scriptFile == null) return false;

            if (
                (overrideConnectionInfo == null)
                || (overrideConnectionInfo.Length == 0)
              )
            {
                overrideConnectionInfo = GetConnectionString();
            }

            if (scriptFile.FullName.EndsWith(".config"))
            {
                string pathToScripts = scriptFile.FullName.Replace(".config", string.Empty);
                if (Directory.Exists(pathToScripts))
                {
                    DirectoryInfo scriptDirectory
                        = new DirectoryInfo(pathToScripts);

                    FileInfo[] scriptFiles 
                        = scriptDirectory.GetFiles("*.config");

                    Array.Sort(scriptFiles, CompareFileNames);

                    foreach (FileInfo file in scriptFiles)
                    {
                        
                        bool result = FBSqlHelper.ExecuteBatchScript(
                            overrideConnectionInfo,
                            file.FullName);

                        if (!result)
                        {
                            log.Error("Failed with no exception running script "
                            + file.FullName);
                        }

                    }
                }

                return true;
            }

            return false;

        }

        private static int CompareFileNames(FileInfo f1, FileInfo f2)
        {
            return f1.FullName.CompareTo(f2.FullName);
        }



        public static bool DatabaseHelperRunScript(
            String script,
            String overrideConnectionInfo)
        {
            if ((script == null) || (script.Length == 0)) return true;

            bool result = false;
            FbConnection connection;

            if (
                (overrideConnectionInfo != null)
                && (overrideConnectionInfo.Length > 0)
              )
            {
                connection = new FbConnection(overrideConnectionInfo);
            }
            else
            {
                connection = new FbConnection(GetConnectionString());
            }

            connection.Open();

            FbTransaction transaction = connection.BeginTransaction();

            try
            {
                FBSqlHelper.ExecuteNonQuery(transaction, CommandType.Text, script, null);

                transaction.Commit();
                result = true;

            }
            catch (FbException ex)
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

        public static bool DatabaseHelperRunScriptByStatements(
            DirectoryInfo scriptDirectory, 
            string overrideConnectionInfo)
        {
            if (scriptDirectory == null) return false;

            bool result = false;

         
            FileInfo[] scriptFiles = scriptDirectory.GetFiles("*.config");

            Array.Sort(scriptFiles, CompareFileNames);

            foreach (FileInfo scriptFile in scriptFiles)
            {
                if (
                (overrideConnectionInfo == null)
                || (overrideConnectionInfo.Length == 0)
              )
                {
                    overrideConnectionInfo = GetConnectionString();
                }

                result = FBSqlHelper.ExecuteBatchScript(
                    overrideConnectionInfo,
                    scriptFile.FullName);
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE " + tableName + " ");
            sqlCommand.Append(" SET " + dataFieldName + " = @fieldValue ");
            sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
            sqlCommand.Append(" " + additionalWhere + " ");
            sqlCommand.Append(" ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@fieldValue", FbDbType.VarChar);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = dataFieldValue;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                connectionString,
                sqlCommand.ToString(),
                arParams);


            return (rowsAffected > 0);

        }

        public static bool DatabaseHelperUpdateTableField(
            String tableName,
            String keyFieldName,
            String keyFieldValue,
            String dataFieldName,
            String dataFieldValue,
            String additionalWhere)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE " + tableName + " ");
            sqlCommand.Append(" SET " + dataFieldName + " = @fieldValue ");
            sqlCommand.Append(" WHERE " + keyFieldName + " = " + keyFieldValue);
            sqlCommand.Append(" " + additionalWhere + " ");
            sqlCommand.Append(" ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@fieldValue", FbDbType.VarChar);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = dataFieldValue;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

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

            return FBSqlHelper.ExecuteReader(
                connectionString,
                sqlCommand.ToString());

        }

        public static IDataReader DatabaseHelperGetReader(
            string connectionString,
            string query
            )
        {
            if(string.IsNullOrEmpty(connectionString)){ connectionString = GetConnectionString(); }

             return FBSqlHelper.ExecuteReader(
                connectionString,
                query);
        }

         public static int DatabaseHelperExecteNonQuery(
            string connectionString,
            string query
            )
        {
            if(string.IsNullOrEmpty(connectionString)){ connectionString = GetConnectionString(); }

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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

            DataSet ds = FBSqlHelper.ExecuteDataset(
                connectionString,
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

            DataSet ds = FBSqlHelper.ExecuteDataset(
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

            ds = FBSqlHelper.ExecuteDataset(
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

            DataSet ds = FBSqlHelper.ExecuteDataset(
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_ModuleDefinitionSettings ");
            sqlCommand.Append("SET FeatureGuid = (SELECT FIRST 1 Guid ");
            sqlCommand.Append("FROM mp_ModuleDefinitions ");
            sqlCommand.Append("WHERE mp_ModuleDefinitions.ModuleDefID ");
            sqlCommand.Append(" = mp_ModuleDefinitionSettings.ModuleDefID )");
            sqlCommand.Append(";");

            DatabaseHelperRunScript(sqlCommand.ToString(), overrideConnectionInfo);

            DatabaseHelperRunScript(
                "ALTER TABLE `mp_ModuleDefinitions` CHANGE `Guid` `Guid` CHAR(36)  NOT NULL;",
                overrideConnectionInfo);

            DatabaseHelperRunScript(
                "ALTER TABLE `mp_ModuleDefinitionSettings` CHANGE `FeatureGuid` `FeatureGuid` CHAR(36)  NOT NULL;",
                overrideConnectionInfo);


        }

        public static void DatabaseHelperDoVersion2234PostUpgradeTasks(
            String overrideConnectionInfo)
        {
            
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
            return DatabaseHelperTableExists("MP_SITES");
        }

        public static bool DatabaseHelperTableExists(string tableName)
        {
            FbConnection connection = new FbConnection(GetConnectionString());
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
}
