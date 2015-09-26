//	Author:                 Joe Audette
//  Created:			    2011-07-23
//	Last Modified:		    2011-07-27
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Text;
using FirebirdSql.Data.FirebirdClient;

namespace mojoPortal.Data
{
    public static class DBSystemLog
    {
        private static String GetReadConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

        }

        private static String GetWriteConnectionString()
        {
            if (ConfigurationManager.AppSettings["FirebirdWriteConnectionString"] != null)
            {
                return ConfigurationManager.AppSettings["FirebirdWriteConnectionString"];
            }

            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

        }

        /// <summary>
        /// Inserts a row in the mp_SystemLog table. Returns new integer id.
        /// </summary>
        /// <param name="logDate"> logDate </param>
        /// <param name="ipAddress"> ipAddress </param>
        /// <param name="culture"> culture </param>
        /// <param name="url"> url </param>
        /// <param name="shortUrl"> shortUrl </param>
        /// <param name="thread"> thread </param>
        /// <param name="logLevel"> logLevel </param>
        /// <param name="logger"> logger </param>
        /// <param name="message"> message </param>
        /// <returns>int</returns>
        public static int Create(
            DateTime logDate,
            string ipAddress,
            string culture,
            string url,
            string shortUrl,
            string thread,
            string logLevel,
            string logger,
            string message)
        {
            FbParameter[] arParams = new FbParameter[9];

            arParams[0] = new FbParameter(":LogDate", FbDbType.TimeStamp);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = logDate;

            arParams[1] = new FbParameter(":IpAddress", FbDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = ipAddress;

            arParams[2] = new FbParameter(":Culture", FbDbType.VarChar, 10);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = culture;

            arParams[3] = new FbParameter(":Url", FbDbType.VarChar);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = url;

            arParams[4] = new FbParameter(":ShortUrl", FbDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = shortUrl;

            arParams[5] = new FbParameter(":Thread", FbDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = thread;

            arParams[6] = new FbParameter(":LogLevel", FbDbType.VarChar, 20);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = logLevel;

            arParams[7] = new FbParameter(":Logger", FbDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = logger;

            arParams[8] = new FbParameter(":Message", FbDbType.VarChar);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = message;

            int newID = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_SYSTEMLOG_INSERT ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
                arParams));

            return newID;

        }

        /// <summary>
        /// Deletes rows from the mp_SystemLog table. Returns true if rows deleted.
        /// </summary>
        public static void DeleteAll()
        {

            //TODO: does firebird support truncate table?
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SystemLog ");
            //sqlCommand.Append("WHERE ");
            //sqlCommand.Append("ID = @ID ");
            sqlCommand.Append(";");

            //FbParameter[] arParams = new FbParameter[1];

            //arParams[0] = new FbParameter("@ID", FbDbType.Integer);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = id;


            FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                null);

        }

        /// <summary>
        /// Deletes a row from the mp_SystemLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public static bool Delete(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SystemLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ID = @ID ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes rows from the mp_SystemLog table. Returns true if rows deleted.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public static bool DeleteOlderThan(DateTime cutoffDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SystemLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LogDate < @CutoffDate ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@CutoffDate", FbDbType.TimeStamp);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cutoffDate;


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes rows from the mp_SystemLog table. Returns true if rows deleted.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public static bool DeleteByLevel(string logLevel)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SystemLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LogLevel = @LogLevel ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@LogLevel", FbDbType.VarChar, 20);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = logLevel;


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets a count of rows in the mp_SystemLog table.
        /// </summary>
        public static int GetCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_SystemLog ");
            sqlCommand.Append(";");

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                null));

        }

        /// <summary>
        /// Gets a page of data from the mp_SystemLog table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPageAscending(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount();

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_SystemLog  ");
            //sqlCommand.Append("WHERE   ");
            sqlCommand.Append("ORDER BY ID  ");
            sqlCommand.Append("	; ");

            //FbParameter[] arParams = new FbParameter[1];

            //arParams[0] = new FbParameter("@CountryGuid", FbDbType.Char, 36);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = countryGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                null);

        }

        /// <summary>
        /// Gets a page of data from the mp_SystemLog table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPageDescending(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount();

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_SystemLog  ");
            //sqlCommand.Append("WHERE   ");
            sqlCommand.Append("ORDER BY ID DESC  ");
            sqlCommand.Append("	; ");

            //FbParameter[] arParams = new FbParameter[1];

            //arParams[0] = new FbParameter("@CountryGuid", FbDbType.Char, 36);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = countryGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                null);

        }

    }
}
