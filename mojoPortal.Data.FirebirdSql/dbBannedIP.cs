/// Author:					Joe Audette
/// Created:				2007-11-03
/// Last Modified:			2009-05-12
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using FirebirdSql.Data.FirebirdClient;

namespace mojoPortal.Data
{
    
    public static class DBBannedIP
    {
       
        public static String DBPlatform()
        {
            return "FirebirdSql";
        }

        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

        }

        

        
        /// <summary>
        /// Inserts a row in the mp_BannedIPAddresses table. Returns new integer id.
        /// </summary>
        /// <param name="bannedIP"> bannedIP </param>
        /// <param name="bannedUTC"> bannedUTC </param>
        /// <param name="bannedReason"> bannedReason </param>
        /// <returns>int</returns>
        public static int Add(
            string bannedIP,
            DateTime bannedUtc,
            string bannedReason)
        {

            #region Bit Conversion

            #endregion

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter(":BannedIP", FbDbType.VarChar, 50);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = bannedIP;

            arParams[1] = new FbParameter(":BannedUTC", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = bannedUtc;

            arParams[2] = new FbParameter(":BannedReason", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = bannedReason;

            int newID = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_BANNEDIPADDRESSES_INSERT ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
                arParams));

            return newID;
        }


        /// <summary>
        /// Updates a row in the mp_BannedIPAddresses table. Returns true if row updated.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <param name="bannedIP"> bannedIP </param>
        /// <param name="bannedUTC"> bannedUTC </param>
        /// <param name="bannedReason"> bannedReason </param>
        /// <returns>bool</returns>
        public static bool Update(
            int rowId,
            string bannedIP,
            DateTime bannedUtc,
            string bannedReason)
        {
            #region Bit Conversion


            #endregion

            FbParameter[] arParams = new FbParameter[4];

            arParams[0] = new FbParameter("@RowID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowId;

            arParams[1] = new FbParameter("@BannedIP", FbDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = bannedIP;

            arParams[2] = new FbParameter("@BannedUTC", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = bannedUtc;

            arParams[3] = new FbParameter("@BannedReason", FbDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = bannedReason;


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_BANNEDIPADDRESSES_UPDATE ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_BannedIPAddresses table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int rowId)
        {
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@RowID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowId;


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_BANNEDIPADDRESSES_DELETE ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_BannedIPAddresses table.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        public static IDataReader GetOne(int rowId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_BannedIPAddresses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowID = @RowID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@RowID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_BannedIPAddresses table.
        /// </summary>
        /// <param name="ipAddress"> ipAddress </param>
        public static IDataReader GeByIpAddress(string ipAddress)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_BannedIPAddresses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("BannedIP = @BannedIP ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@BannedIP", FbDbType.VarChar, 50);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = ipAddress;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Returns true if the passed in address is banned
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <returns>bool</returns>
        public static bool IsBanned(string ipAddress)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_BannedIPAddresses ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("BannedIP = @BannedIP ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@BannedIP", FbDbType.VarChar, 50);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = ipAddress;

            int foundRows = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (foundRows > 0);

        }

        /// <summary>
        /// Gets a count of rows in the mp_BannedIPAddresses table.
        /// </summary>
        public static int GetCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_BannedIPAddresses ;");

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                null));

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_BannedIPAddresses table.
        /// </summary>
        public static IDataReader GetAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_BannedIPAddresses ;");

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                null);

        }


        /// <summary>
        /// Gets a page of data from the mp_BannedIPAddresses table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
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
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString() + " ");
            sqlCommand.Append("	SKIP " + pageLowerBound.ToString() + " ");
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_BannedIPAddresses  ");
            sqlCommand.Append("ORDER BY  BannedIP ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@PageNumber", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageNumber;

            arParams[1] = new FbParameter("@PageSize", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

       


    }
}
