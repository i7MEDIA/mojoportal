/// Author:				
/// Created:			2007-06-22
/// Last Modified:		2007-06-22
/// 
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.


namespace mojoPortal.Data
{
    using System;
    using System.IO;
    using System.Text;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Configuration;

    public static class DBCurrency
    {
        //private static string GetConnectionString()
        //{
        //    if (ConfigurationManager.AppSettings["WebStoreMSSQLConnectionString"] != null)
        //    {
        //        return ConfigurationManager.AppSettings["WebStoreMSSQLConnectionString"];
        //    }

        //    return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        //}

        //public static String DBPlatform()
        //{
        //    return "MSSQL";
        //}

        /// <summary>
        /// Inserts a row in the mp_Currency table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="title"> title </param>
        /// <param name="code"> code </param>
        /// <param name="symbolLeft"> symbolLeft </param>
        /// <param name="symbolRight"> symbolRight </param>
        /// <param name="decimalPointChar"> decimalPointChar </param>
        /// <param name="thousandsPointChar"> thousandsPointChar </param>
        /// <param name="decimalPlaces"> decimalPlaces </param>
        /// <param name="value"> value </param>
        /// <param name="lastModified"> lastModified </param>
        /// <param name="created"> created </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            string title,
            string code,
            string symbolLeft,
            string symbolRight,
            string decimalPointChar,
            string thousandsPointChar,
            string decimalPlaces,
            decimal value,
            DateTime lastModified,
            DateTime created)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Currency_Insert", 11);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 50, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Code", SqlDbType.NChar, 3, ParameterDirection.Input, code);
            sph.DefineSqlParameter("@SymbolLeft", SqlDbType.NVarChar, 15, ParameterDirection.Input, symbolLeft);
            sph.DefineSqlParameter("@SymbolRight", SqlDbType.NVarChar, 15, ParameterDirection.Input, symbolRight);
            sph.DefineSqlParameter("@DecimalPointChar", SqlDbType.NChar, 1, ParameterDirection.Input, decimalPointChar);
            sph.DefineSqlParameter("@ThousandsPointChar", SqlDbType.NChar, 1, ParameterDirection.Input, thousandsPointChar);
            sph.DefineSqlParameter("@DecimalPlaces", SqlDbType.NChar, 1, ParameterDirection.Input, decimalPlaces);
            sph.DefineSqlParameter("@Value", SqlDbType.Decimal, ParameterDirection.Input, value);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }



        /// <summary>
        /// Updates a row in the mp_Currency table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="title"> title </param>
        /// <param name="code"> code </param>
        /// <param name="symbolLeft"> symbolLeft </param>
        /// <param name="symbolRight"> symbolRight </param>
        /// <param name="decimalPointChar"> decimalPointChar </param>
        /// <param name="thousandsPointChar"> thousandsPointChar </param>
        /// <param name="decimalPlaces"> decimalPlaces </param>
        /// <param name="value"> value </param>
        /// <param name="lastModified"> lastModified </param>
        /// <param name="created"> created </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            string title,
            string code,
            string symbolLeft,
            string symbolRight,
            string decimalPointChar,
            string thousandsPointChar,
            string decimalPlaces,
            decimal value,
            DateTime lastModified)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Currency_Update", 10);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 50, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Code", SqlDbType.NChar, 3, ParameterDirection.Input, code);
            sph.DefineSqlParameter("@SymbolLeft", SqlDbType.NVarChar, 15, ParameterDirection.Input, symbolLeft);
            sph.DefineSqlParameter("@SymbolRight", SqlDbType.NVarChar, 15, ParameterDirection.Input, symbolRight);
            sph.DefineSqlParameter("@DecimalPointChar", SqlDbType.NChar, 1, ParameterDirection.Input, decimalPointChar);
            sph.DefineSqlParameter("@ThousandsPointChar", SqlDbType.NChar, 1, ParameterDirection.Input, thousandsPointChar);
            sph.DefineSqlParameter("@DecimalPlaces", SqlDbType.NChar, 1, ParameterDirection.Input, decimalPlaces);
            sph.DefineSqlParameter("@Value", SqlDbType.Decimal, ParameterDirection.Input, value);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_Currency table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Currency_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_Currency table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Currency_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();

        }



        /// <summary>
        /// Gets an IDataReader with all rows in the mp_Currency table.
        /// </summary>
        public static IDataReader GetAll()
        {

            return SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_Currency_SelectAll",
                null);

        }



    }
}
