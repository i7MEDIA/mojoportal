
using System;
using System.Configuration;

namespace mojoPortal.Data
{
    public static class ConnectionString
    {
        public static string GetReadConnectionString()
        {
            if (UseConnectionStringSection()) { return GetReadConnectionStringFromConnectionStringSection(); }

            return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        }

        /// <summary>
        /// Gets the connection string for write.
        /// </summary>
        /// <returns></returns>
        public static string GetWriteConnectionString()
        {
            if (UseConnectionStringSection()) { return GetWriteConnectionStringFromConnectionStringSection(); }

            if (ConfigurationManager.AppSettings["MSSQLWriteConnectionString"] != null)
            {
                return ConfigurationManager.AppSettings["MSSQLWriteConnectionString"];
            }

            return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        }

        private static string GetWriteConnectionStringFromConnectionStringSection()
        {
            if (ConfigurationManager.ConnectionStrings["MSSQLWriteConnectionString"] != null)
            {
                return ConfigurationManager.ConnectionStrings["MSSQLWriteConnectionString"].ConnectionString;
            }

            return ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString;

        }

        private static string GetReadConnectionStringFromConnectionStringSection()
        {
            return ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString;

        }

        private static bool UseConnectionStringSection()
        {
            if (ConfigurationManager.AppSettings["UseConnectionStringSection"] != null)
            {
                if (ConfigurationManager.AppSettings["UseConnectionStringSection"] == "true") { return true; }

            }


            return false;
        }

    }

    


}

