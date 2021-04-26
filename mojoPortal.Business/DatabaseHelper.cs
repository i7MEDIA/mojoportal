using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.IO;
using mojoPortal.Data;
using log4net;

namespace mojoPortal.Business
{
	
	public static class DatabaseHelper
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(DatabaseHelper));


        public static String GetApplicationName()
        {
            return "mojoportal-core";
        }

        public static Guid GetApplicationId()
        {
            Guid appGuid = new Guid("077e4857-f583-488e-836e-34a4b04be855");
            return appGuid;
        }

        public static String DBPlatform()
        {
            return DBPortal.DBPlatform();
        }

        public static Version DBCodeVersion()
        {
            // this must be maintained/updated in code to make it run the new version upgrade script
            int major = 2;
            int minor = 8;
            int build = 0;
            int revision = 3;
            return new Version(major, minor, build, revision);

        }

        public static Version DBSchemaVersion()
        {
            // this should never change
            // its the last version before auto upgrades
            int major = 2;
            int minor = 2;
            int build = 1;
            int revision = 4;

            bool found = false;
            try
            {
                using(IDataReader reader = DBPortal.SchemaVersionGetSchemaVersion(GetApplicationId()))
                {
                    if (reader.Read())
                    {
                        major = Convert.ToInt32(reader["Major"]);
                        minor = Convert.ToInt32(reader["Minor"]);
                        build = Convert.ToInt32(reader["Build"]);
                        revision = Convert.ToInt32(reader["Revision"]);

                        found = true;

                    }
                }

                if(!found)
                {
                    DBPortal.SchemaVersionAddSchemaVersion(
                        GetApplicationId(),
                        GetApplicationName(),
                        major,
                        minor,
                        build,
                        revision);
                }

            }
            catch(DbException){ }
            catch (InvalidOperationException) { }


            return new Version(major, minor, build, revision);

        }

        public static void EnsureDatabase()
        {
            DBPortal.EnsureDatabase();
        }

        //public static int AddSchemaVersion(
        //    Guid applicationID,
        //    string applicationName,
        //    int major,
        //    int minor,
        //    int build,
        //    int revision)
        //{
        //    return dbPortal.SchemaVersion_AddSchemaVersion(
        //        applicationID,
        //        applicationName,
        //        major,
        //        minor,
        //        build,
        //        revision);

        //}

        public static bool UpdateSchemaVersion(
            Guid applicationId,
            string applicationName,
            int major,
            int minor,
            int build,
            int revision)
        {
            if (!DBPortal.SchemaVersionExists(applicationId))
            {
                return DBPortal.SchemaVersionAddSchemaVersion(
                    applicationId,
                    applicationName,
                    major,
                    minor,
                    build,
                    revision);
            }

            return DBPortal.SchemaVersionUpdateSchemaVersion(
                applicationId,
                applicationName,
                major,
                minor,
                build,
                revision);

        }

        public static bool DeleteSchemaVersion(Guid applicationId)
        {
            return DBPortal.SchemaVersionDeleteSchemaVersion(applicationId);

        }

        public static Version GetSchemaVersion(string applicationName)
        {
            Guid appGuid = GetApplicationId(applicationName);
            return GetSchemaVersion(appGuid);

        }

        public static Version GetSchemaVersion(Guid applicationId)
        {
            //Guid mojoappID = new Guid("077e4857-f583-488e-836e-34a4b04be855");
            //if (applicationID == mojoappID)
            //{
            //    return dbSchemaVersion();
            //}

            int major = 0;
            int minor = 0;
            int build = 0;
            int revision = 0;

            try
            {
                using(IDataReader reader = DBPortal.SchemaVersionGetSchemaVersion(applicationId))
                {
                    if (reader.Read())
                    {
                        major = Convert.ToInt32(reader["Major"]);
                        minor = Convert.ToInt32(reader["Minor"]);
                        build = Convert.ToInt32(reader["Build"]);
                        revision = Convert.ToInt32(reader["Revision"]);
                    }
                }

            }
            catch(DbException){ }
            catch (InvalidOperationException) { }
            catch (Exception ex)
            {
                // hate to trap System.Exception but SqlCeException doe snot inherit from DbException as it should
                if (DatabaseHelper.DBPlatform() != "SqlCe") { throw; }
                log.Error(ex);
            }


            return new Version(major, minor, build, revision);
        }

        public static int AddSchemaScriptHistory(
            Guid applicationId,
            string scriptFile,
            DateTime runTime,
            bool errorOccurred,
            string errorMessage,
            string scriptBody)
        {
            return DBPortal.SchemaScriptHistoryAddSchemaScriptHistory(
                applicationId,
                scriptFile,
                runTime,
                errorOccurred,
                errorMessage,
                scriptBody);


        }

        public static bool DeleteSchemaScriptHistory(int id)
        {
            return DBPortal.SchemaScriptHistoryDeleteSchemaScriptHistory(id);

        }

        public static IDataReader SchemaVersionGetNonCore()
        {
            return DBPortal.SchemaVersionGetNonCore();
        }

        public static IDataReader GetSchemaScriptHistory(int id)
        {
            return DBPortal.SchemaScriptHistoryGetSchemaScriptHistory(id);
        }

        public static IDataReader GetSchemaScriptHistory(Guid applicationId)
        {
            return DBPortal.SchemaScriptHistoryGetSchemaScriptHistory(applicationId);

        }

        public static IDataReader GetSchemaScriptErrorHistory(Guid applicationId)
        {
            return DBPortal.SchemaScriptHistoryGetSchemaScriptErrorHistory(applicationId);
        }



        public static bool SchemaScriptHasBeenRun(Guid applicationId, String scriptFile)
        {
            try
            {
                return DBPortal.SchemaScriptHistoryExists(applicationId, scriptFile);
            }
            catch (DbException)
            { }

            return false;

        }

        public static bool CanAccessDatabase()
        {
            return DBPortal.DatabaseHelperCanAccessDatabase();
        }

        public static DbException GetConnectionError(String overrideConnectionInfo)
        {
            return DBPortal.DatabaseHelperGetConnectionError(overrideConnectionInfo);
        }

        

        public static bool CanAlterSchema(String overrideConnectionInfo)
        {
            try
            {
                return DBPortal.DatabaseHelperCanAlterSchema(overrideConnectionInfo);
            }
            catch(DbException ex)
            {
                log.Error(ex.ToString());
                
                return false;
            }
        }

        public static bool SchemaHasBeenCreated()
        {
            return DBPortal.DatabaseHelperSitesTableExists();
        }

        public static int ExistingSiteCount()
        {
            int siteCount = 0;
            try
            {
                siteCount = SiteSettings.SiteCount();
            }
            catch (DbException) { }
            catch (InvalidOperationException) { }
            catch (Exception ex)
            {
                //this is a needed hack because SqlCeException does not inherit from DbException like other data layers
                //instead it inherits from System.Exception which we would rather not trap
                if (DBPlatform().ToLower() != "sqlce") { throw ex; }
            }

            return siteCount;

        }

        
        public static string RunScript(
            Guid applicationId, 
            FileInfo scriptFile, 
            String overrideConnectionInfo)
        {
            // returning empty string indicates success
            // else return error message
            string resultMessage = string.Empty;

            if (scriptFile == null) return resultMessage;
           
            
            
            try
            {
                bool result = DBPortal.DatabaseHelperRunScript(
                    scriptFile, overrideConnectionInfo);

                if (!result)
                {
                    resultMessage = "script failed with no error message";
                }

            }
            catch (DbException ex)
            {
                resultMessage = ex.ToString();
            }

            return resultMessage;

        }

        public static IDataReader GetReader(string query)
        {
            return GetReader(string.Empty, query);
        }

        public static IDataReader GetReader(string connectionString, string query)
        {
            return DBPortal.DatabaseHelperGetReader(connectionString, query);

        }

        public static int ExecteNonQuery(string query)
        {
            return ExecteNonQuery(string.Empty, query);
        }

        public static int ExecteNonQuery(string connectionString, string query)
        {
            return DBPortal.DatabaseHelperExecteNonQuery(connectionString, query);
        }

        public static Version ParseVersionFromFileName(String fileName)
        {
            Version version = null;

            int major = 0;
            int minor = 0;
            int build = 0;
            int revision = 0;
            bool success = true;


            if (fileName != null)
            {
                char[] separator = { '.' };
                string[] args = fileName.Replace(".config",String.Empty).Split(separator);
                if(args.Length >= 4)
                {
                    if(!(int.TryParse(args[0], out major)))
                    {
                        major = 0;
                        success = false;
                    }

                    if (!(int.TryParse(args[1], out minor)))
                    {
                        minor = 0;
                        success = false;
                    }

                    if (!(int.TryParse(args[2], out build)))
                    {
                        build = 0;
                        success = false;
                    }

                    if (!(int.TryParse(args[3], out revision)))
                    {
                        revision = 0;
                        success = false;
                    }

                    if (success)
                    {
                        version = new Version(major, minor, build, revision);
                    }

                }

            }

            
            return version;

        }

        public static Guid GetApplicationId(string applicationName)
        {
            if(string.Equals(applicationName, "mojoportal-core", StringComparison.InvariantCultureIgnoreCase))
                return new Guid("077e4857-f583-488e-836e-34a4b04be855");


            Guid appID = Guid.NewGuid();

            using (IDataReader reader = DBPortal.DatabaseHelperGetApplicationId(applicationName))
            {
                if (reader.Read())
                {
                    appID = new Guid(reader["ApplicationID"].ToString());

                }
            }

            return appID;

        }


        public static String GetApplicationName(Guid applicationId)
        {
            String appName = null;
            using (IDataReader reader = DBPortal.SchemaVersionGetSchemaVersion(applicationId))
            {
                if (reader.Read())
                {
                    appName = reader["ApplicationName"].ToString();
                }
            }

            return appName;

        }




		public static bool UpdateTableField(
			String connectionString,
			String tableName, 
			String keyFieldName,
			String keyFieldValue,
			String dataFieldName, 
			String dataFieldValue,
			String additionalWhere)
		{
			return DBPortal.DatabaseHelperUpdateTableField(
				connectionString,
				tableName,
				keyFieldName,
				keyFieldValue,
				dataFieldName,
				dataFieldValue,
				additionalWhere);
			
		}

        public static bool UpdateTableField(
            String tableName,
            String keyFieldName,
            String keyFieldValue,
            String dataFieldName,
            String dataFieldValue,
            String additionalWhere)
        {
            return DBPortal.DatabaseHelperUpdateTableField(
                tableName,
                keyFieldName,
                keyFieldValue,
                dataFieldName,
                dataFieldValue,
                additionalWhere);

        }

		public static IDataReader GetReader(
			String connectionString,
			String tableName, 
			String whereClause)
		{
			return DBPortal.DatabaseHelperGetReader(connectionString, tableName, whereClause);

		}

		public static DataTable GetTable(
			String connectionString,
			String tableName, 
			String whereClause)
		{
			return DBPortal.DatabaseHelperGetTable(connectionString, tableName, whereClause);

        }

        #region Version Specific tasks

        public static void DoVersion2320PostUpgradeTasks(
            String overrideConnectionInfo)
        {

            DBPortal.DatabaseHelperDoVersion2320PostUpgradeTasks(overrideConnectionInfo);
        }

        public static void DoVersion2230PostUpgradeTasks(
            String overrideConnectionInfo)
        {
            DBPortal.DatabaseHelperDoVersion2230PostUpgradeTasks(overrideConnectionInfo);

        }

        public static void DoVersion2234PostUpgradeTasks(
            String overrideConnectionInfo)
        {
            DBPortal.DatabaseHelperDoVersion2234PostUpgradeTasks(overrideConnectionInfo);

        }

        public static void DoVersion2247PostUpgradeTasks(
            String overrideConnectionInfo)
        {
            DBPortal.DatabaseHelperDoVersion2247PostUpgradeTasks(overrideConnectionInfo);

        }

        public static void DoVersion2253PostUpgradeTasks(
            String overrideConnectionInfo)
        {
            DBPortal.DatabaseHelperDoVersion2253PostUpgradeTasks(overrideConnectionInfo);

        }

        public static void DoForumVersion2202PostUpgradeTasks(
            String overrideConnectionInfo)
        {
            DBPortal.DatabaseHelperDoForumVersion2202PostUpgradeTasks(overrideConnectionInfo);

        }

        public static void DoForumVersion2203PostUpgradeTasks(
            String overrideConnectionInfo)
        {
            DBPortal.DatabaseHelperDoForumVersion2203PostUpgradeTasks(overrideConnectionInfo);

        }


        #endregion

        public static DataTable GetTableFromDataReader(IDataReader reader)
        {

            DataTable schemaTable = reader.GetSchemaTable();
            DataTable dataTable = new DataTable();
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

            try
            {
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


    }
}
