// Author:					Joe Audette
// Created:					2010-08-26
// Last Modified:			2010-08-26
// 
// You must not remove this notice, or any other, from this software.

using System.Configuration;

namespace sts.Analytics.Data
{
    internal static class ConnectionString
    {
        /// <summary>
        /// Gets the connection string for read.
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            if (ConfigurationManager.AppSettings["sts_analytics_ConnectionString"] != null)
            {
                return ConfigurationManager.AppSettings["sts_analytics_ConnectionString"];
            }

            return ConfigurationManager.AppSettings["MySqlConnectionString"];

        }



    }
}
