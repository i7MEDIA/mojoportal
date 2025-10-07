using System;
using System.Configuration;
using System.Data;
using System.Text;
using Mono.Data.Sqlite;

namespace mojoPortal.Data
{
    public static class DBSiteFolder
    {
        
        public static String DBPlatform()
        {
            return "SQLite";
        }

        private static string GetConnectionString()
        {
            string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];
            if (connectionString == "defaultdblocation")
            {
                connectionString = "version=3,URI=file:"
                    + System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/mojo.db.config");

            }
            return connectionString;
        }



		public static int Add(Guid guid, Guid siteGuid, int siteId, string folderName)
		{
			SqliteParameter[] arParams =
			[
				new SqliteParameter(":Guid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				},
				new SqliteParameter(":SiteGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				},
				new SqliteParameter(":FolderName", DbType.String, 255)
				{
					Direction = ParameterDirection.Input,
					Value = folderName
				},
				new SqliteParameter(":SiteID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = siteId
				},
			];

			var sqlCommand = """
                INSERT INTO mp_SiteFolders (Guid, SiteGuid, FolderName, SiteID)
                VALUES (:Guid, :SiteGuid, :FolderName, :SiteID);
            """;

			int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);

			return rowsAffected;
		}


		public static bool Update(Guid guid, string folderName)
		{
			SqliteParameter[] arParams =
			[
				new SqliteParameter(":Guid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				},
				new SqliteParameter(":FolderName", DbType.String, 255)
				{
					Direction = ParameterDirection.Input,
					Value = folderName
				},
			];

			var sqlCommand = """
                UPDATE mp_SiteFolders 
                SET FolderName = :FolderName
                WHERE Guid = :Guid ;
                """;

			int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);

			return (rowsAffected > -1);
		}


		public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SiteFolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = :Guid ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return (rowsAffected > 0);

        }


        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SiteFolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = :Guid ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SiteFolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = :SiteGuid ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static Guid GetSiteGuid(string folderName)
        {
            StringBuilder sqlCommand = new StringBuilder();

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":FolderName", DbType.String, 255);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = folderName;

            Guid siteGuid = Guid.Empty;

            sqlCommand.Append("SELECT SiteGuid ");
            sqlCommand.Append("FROM mp_SiteFolders ");
            sqlCommand.Append("WHERE FolderName = :FolderName ;");

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    siteGuid = new Guid(reader["SiteGuid"].ToString());
                }
            }

            if (siteGuid == Guid.Empty)
            {

                sqlCommand = new StringBuilder();
                sqlCommand.Append("SELECT SiteGuid ");
                sqlCommand.Append("FROM	mp_Sites ");
                sqlCommand.Append("ORDER BY	SiteID ");
                sqlCommand.Append("LIMIT 1 ;");


                using (IDataReader reader = SqliteHelper.ExecuteReader(
                    GetConnectionString(),
                    sqlCommand.ToString(),
                    null))
                {
                    if (reader.Read())
                    {
                        siteGuid = new Guid(reader["SiteGuid"].ToString());
                    }
                }

            }

            return siteGuid;

        }

        public static bool Exists(string folderName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_SiteFolders ");
            sqlCommand.Append("WHERE FolderName = :FolderName ; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":FolderName", DbType.String, 255);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = folderName;

            int count = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        


    }
}
