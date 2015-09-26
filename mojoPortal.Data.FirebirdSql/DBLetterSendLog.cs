/// Author:					Joe Audette
/// Created:				2007-12-27
/// Last Modified:			2009-10-31
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
    
    public static class DBLetterSendLog
    {
        
        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

        }

        /// <summary>
        /// Inserts a row in the mp_LetterSendLog table. Returns new integer id.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="emailAddress"> emailAddress </param>
        /// <param name="uTC"> uTC </param>
        /// <param name="errorOccurred"> errorOccurred </param>
        /// <param name="errorMessage"> errorMessage </param>
        /// <returns>int</returns>
        public static int Create(
            Guid letterGuid,
            Guid userGuid,
            Guid subscribeGuid,
            string emailAddress,
            DateTime uTC,
            bool errorOccurred,
            string errorMessage)
        {

            #region Bit Conversion
            int intErrorOccurred;
            if (errorOccurred)
            {
                intErrorOccurred = 1;
            }
            else
            {
                intErrorOccurred = 0;
            }


            #endregion

            FbParameter[] arParams = new FbParameter[7];

            arParams[0] = new FbParameter(":LetterGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            arParams[1] = new FbParameter(":UserGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new FbParameter(":SubscribeGuid", FbDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = subscribeGuid.ToString();

            arParams[3] = new FbParameter(":EmailAddress", FbDbType.VarChar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = emailAddress;

            arParams[4] = new FbParameter(":UTC", FbDbType.TimeStamp);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = uTC;

            arParams[5] = new FbParameter(":ErrorOccurred", FbDbType.SmallInt);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intErrorOccurred;

            arParams[6] = new FbParameter(":ErrorMessage", FbDbType.VarChar);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = errorMessage;

            int newID = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_LETTERSENDLOG_INSERT ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
                arParams));

            return newID;
        }


        /// <summary>
        /// Updates a row in the mp_LetterSendLog table. Returns true if row updated.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <param name="letterGuid"> letterGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="emailAddress"> emailAddress </param>
        /// <param name="uTC"> uTC </param>
        /// <param name="errorOccurred"> errorOccurred </param>
        /// <param name="errorMessage"> errorMessage </param>
        /// <returns>bool</returns>
        public static bool Update(
            int rowId,
            Guid letterGuid,
            Guid userGuid,
            string emailAddress,
            DateTime uTC,
            bool errorOccurred,
            string errorMessage)
        {
            #region Bit Conversion

            int intErrorOccurred;
            if (errorOccurred)
            {
                intErrorOccurred = 1;
            }
            else
            {
                intErrorOccurred = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_LetterSendLog ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("LetterGuid = @LetterGuid, ");
            sqlCommand.Append("UserGuid = @UserGuid, ");
            sqlCommand.Append("EmailAddress = @EmailAddress, ");
            sqlCommand.Append("UTC = @UTC, ");
            sqlCommand.Append("ErrorOccurred = @ErrorOccurred, ");
            sqlCommand.Append("ErrorMessage = @ErrorMessage ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RowID = @RowID ");
            sqlCommand.Append(";");



            FbParameter[] arParams = new FbParameter[7];

            arParams[0] = new FbParameter("@RowID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowId;

            arParams[1] = new FbParameter("@LetterGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterGuid.ToString();

            arParams[2] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid.ToString();

            arParams[3] = new FbParameter("@EmailAddress", FbDbType.VarChar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = emailAddress;

            arParams[4] = new FbParameter("@UTC", FbDbType.TimeStamp);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = uTC;

            arParams[5] = new FbParameter("@ErrorOccurred", FbDbType.SmallInt);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intErrorOccurred;

            arParams[6] = new FbParameter("@ErrorMessage", FbDbType.VarChar);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = errorMessage;


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_LetterSendLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int rowID)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSendLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowID = @RowID ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@RowID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowID;


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        /// <summary>
        /// Deletes from the mp_LetterSendLog table for the letterGuid. Returns true if row deleted.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByLetter(Guid letterGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSendLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterGuid = @LetterGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@LetterGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByLetterInfo(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSendLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterGuid IN (SELECT LetterGuid FROM mp_Letter WHERE LetterInfoGuid = @LetterInfoGuid) ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSendLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterGuid IN (SELECT LetterGuid FROM mp_Letter WHERE ");
            sqlCommand.Append("LetterInfoGuid IN (SELECT LetterInfoGuid FROM mp_LetterInfo WHERE SiteGuid = @SiteGuid)) ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_LetterSendLog table.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        public static IDataReader GetOne(int rowId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_LetterSendLog ");
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
        /// Gets a count of rows in the mp_LetterSendLog table.
        /// </summary>
        public static int GetCount(Guid letterGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_LetterSendLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterGuid = @LetterGuid ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@LetterGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_LetterSendLog table.
        /// </summary>
        public static IDataReader GetByLetter(Guid letterGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_LetterSendLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterGuid = @LetterGuid ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@LetterGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        /// <summary>
        /// Gets a page of data from the mp_LetterSendLog table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid letterGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(letterGuid);

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
            sqlCommand.Append("FROM	mp_LetterSendLog  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterGuid = @LetterGuid ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@LetterGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


    }
}
