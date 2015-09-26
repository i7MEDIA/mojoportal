using System;
using System.Configuration;
using mojoPortal.Data;

namespace WebStore.Data
{
    public static class WebStoreConnectionString
    {

        public static string GetReadConnectionString()
        {
            if (ConfigurationManager.AppSettings["WebStoreMSSQLConnectionString"] != null)
            {
                return ConfigurationManager.AppSettings["WebStoreMSSQLConnectionString"];
            }

            return ConnectionString.GetReadConnectionString();

        }

        /// <summary>
        /// Gets the connection string for write.
        /// </summary>
        /// <returns></returns>
        public static string GetWriteConnectionString()
        {
            if (ConfigurationManager.AppSettings["WebStoreMSSQLConnectionString"] != null)
            {
                return ConfigurationManager.AppSettings["WebStoreMSSQLConnectionString"];
            }

            return ConnectionString.GetWriteConnectionString();

        }
    }
}
