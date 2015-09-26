using System;
using System.Configuration;


namespace mojoPortal.Data
{
    public static class ConnectionString
    {
        public static string GetConnectionString()
        {
            string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];
            if (connectionString == "defaultdblocation")
            {

                connectionString = "version=3,URI=file:"
                    + System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/mojo.db.config");

            }
            return connectionString;
        }

        // these methods are only for compatibility with import and upgrade utils
        // there is no replication supported for SQLite so there is no real need for different connection
        // strings for read/write
        public static String GetReadConnectionString()
        {
            return GetConnectionString();
        }

        public static String GetWriteConnectionString()
        {
            return GetConnectionString();
        }
    }
}
