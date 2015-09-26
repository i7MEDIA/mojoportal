using System;
using System.Configuration;


namespace mojoPortal.Data
{
    public static class ConnectionString
    {
        public static String GetReadConnectionString()
        {
            return ConfigurationManager.AppSettings["PostgreSQLConnectionString"];

        }

        public static String GetWriteConnectionString()
        {
            if (ConfigurationManager.AppSettings["PostgreSQLWriteConnectionString"] != null)
            {
                return ConfigurationManager.AppSettings["PostgreSQLWriteConnectionString"];
            }
            return ConfigurationManager.AppSettings["PostgreSQLConnectionString"];

        }

    }
}
