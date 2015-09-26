using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using log4net;
using Npgsql;
using NpgsqlTypes;

namespace WebStore.Data
{
    /// <summary>
    /// Author:				Joe Audette
    /// Created:			2/9/2007
    /// Last Modified:		2/9/2007
    /// 
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    ///  
    /// </summary>
    public static class db
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(db));

        public static String dbPlatform()
        {
            return "pgsql";
        }

        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["WebStorePostgreSQLConnectionString"];
        }

        private static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(GetConnectionString());

        }


    }
}
