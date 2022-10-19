/// Author:					Voir Hillaire
/// Created:				2009-03-24
/// Last Modified:			2012-08-11
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
using System.Text;
using Npgsql;

namespace mojoPortal.Data
{
    
    public static class DBPlugNPayLog
    {
        

        /// <summary>
        /// Inserts a row in the mp_PlugNPayLog table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="storeGuid"> storeGuid </param>
        /// <param name="cartGuid"> cartGuid </param>
        /// <param name="rawResponse"> rawResponse </param>
        /// <param name="responseCode"> responseCode </param>
        /// <param name="responseReasonCode"> responseReasonCode </param>
        /// <param name="reason"> reason </param>
        /// <param name="avsCode"> avsCode </param>
        /// <param name="ccvCode"> ccvCode </param>
        /// <param name="cavCode"> cavCode </param>
        /// <param name="transactionId"> transactionId </param>
        /// <param name="transactionType"> transactionType </param>
        /// <param name="method"> method </param>
        /// <param name="authCode"> authCode </param>
        /// <param name="amount"> amount </param>
        /// <param name="tax"> tax </param>
        /// <param name="duty"> duty </param>
        /// <param name="freight"> freight </param>
        /// <returns>int</returns>
        public static int Create(
            Guid rowGuid,
            DateTime createdUtc,
            Guid siteGuid,
            Guid userGuid,
            Guid storeGuid,
            Guid cartGuid,
            string rawResponse,
            string responseCode,
            string responseReasonCode,
            string reason,
            string avsCode,
            string ccvCode,
            string cavCode,
            string transactionId,
            string transactionType,
            string method,
            string authCode,
            decimal amount,
            decimal tax,
            decimal duty,
            decimal freight)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[21];

            arParams[0] = new NpgsqlParameter(":rowguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new NpgsqlParameter(":createdutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = createdUtc;

            arParams[2] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new NpgsqlParameter(":storeguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = storeGuid.ToString();

            arParams[5] = new NpgsqlParameter(":cartguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = cartGuid.ToString();

            arParams[6] = new NpgsqlParameter(":rawresponse", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = rawResponse;

            arParams[7] = new NpgsqlParameter(":responsecode", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = responseCode;

            arParams[8] = new NpgsqlParameter(":responsereasoncode", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = responseReasonCode;

            arParams[9] = new NpgsqlParameter(":reason", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = reason;

            arParams[10] = new NpgsqlParameter(":avscode", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = avsCode;

            arParams[11] = new NpgsqlParameter(":ccvcode", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = ccvCode;

            arParams[12] = new NpgsqlParameter(":cavcode", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = cavCode;

            arParams[13] = new NpgsqlParameter(":transactionid", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = transactionId;

            arParams[14] = new NpgsqlParameter(":transactiontype", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = transactionType;

            arParams[15] = new NpgsqlParameter(":method", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = method;

            arParams[16] = new NpgsqlParameter(":authcode", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = authCode;

            arParams[17] = new NpgsqlParameter(":amount", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = amount;

            arParams[18] = new NpgsqlParameter(":tax", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = tax;

            arParams[19] = new NpgsqlParameter(":duty", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = duty;

            arParams[20] = new NpgsqlParameter(":freight", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = freight;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_PlugNPaylog (");
            sqlCommand.Append("rowguid, ");
            sqlCommand.Append("createdutc, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("storeguid, ");
            sqlCommand.Append("cartguid, ");
            sqlCommand.Append("rawresponse, ");
            sqlCommand.Append("responsecode, ");
            sqlCommand.Append("responsereasoncode, ");
            sqlCommand.Append("reason, ");
            sqlCommand.Append("avscode, ");
            sqlCommand.Append("ccvcode, ");
            sqlCommand.Append("cavcode, ");
            sqlCommand.Append("transactionid, ");
            sqlCommand.Append("transactiontype, ");
            sqlCommand.Append("method, ");
            sqlCommand.Append("authcode, ");
            sqlCommand.Append("amount, ");
            sqlCommand.Append("tax, ");
            sqlCommand.Append("duty, ");
            sqlCommand.Append("freight )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":rowguid, ");
            sqlCommand.Append(":createdutc, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":userguid, ");
            sqlCommand.Append(":storeguid, ");
            sqlCommand.Append(":cartguid, ");
            sqlCommand.Append(":rawresponse, ");
            sqlCommand.Append(":responsecode, ");
            sqlCommand.Append(":responsereasoncode, ");
            sqlCommand.Append(":reason, ");
            sqlCommand.Append(":avscode, ");
            sqlCommand.Append(":ccvcode, ");
            sqlCommand.Append(":cavcode, ");
            sqlCommand.Append(":transactionid, ");
            sqlCommand.Append(":transactiontype, ");
            sqlCommand.Append(":method, ");
            sqlCommand.Append(":authcode, ");
            sqlCommand.Append(":amount, ");
            sqlCommand.Append(":tax, ");
            sqlCommand.Append(":duty, ");
            sqlCommand.Append(":freight ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_PlugNPayLog table. Returns true if row updated.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="storeGuid"> storeGuid </param>
        /// <param name="cartGuid"> cartGuid </param>
        /// <param name="rawResponse"> rawResponse </param>
        /// <param name="responseCode"> responseCode </param>
        /// <param name="responseReasonCode"> responseReasonCode </param>
        /// <param name="reason"> reason </param>
        /// <param name="avsCode"> avsCode </param>
        /// <param name="ccvCode"> ccvCode </param>
        /// <param name="cavCode"> cavCode </param>
        /// <param name="transactionId"> transactionId </param>
        /// <param name="transactionType"> transactionType </param>
        /// <param name="method"> method </param>
        /// <param name="authCode"> authCode </param>
        /// <param name="amount"> amount </param>
        /// <param name="tax"> tax </param>
        /// <param name="duty"> duty </param>
        /// <param name="freight"> freight </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid rowGuid,
            Guid siteGuid,
            Guid userGuid,
            Guid storeGuid,
            Guid cartGuid,
            string rawResponse,
            string responseCode,
            string responseReasonCode,
            string reason,
            string avsCode,
            string ccvCode,
            string cavCode,
            string transactionId,
            string transactionType,
            string method,
            string authCode,
            decimal amount,
            decimal tax,
            decimal duty,
            decimal freight)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[20];

            arParams[0] = new NpgsqlParameter(":rowguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid.ToString();

            arParams[3] = new NpgsqlParameter(":storeguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = storeGuid.ToString();

            arParams[4] = new NpgsqlParameter(":cartguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = cartGuid.ToString();

            arParams[5] = new NpgsqlParameter(":rawresponse", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = rawResponse;

            arParams[6] = new NpgsqlParameter(":responsecode", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = responseCode;

            arParams[7] = new NpgsqlParameter(":responsereasoncode", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = responseReasonCode;

            arParams[8] = new NpgsqlParameter(":reason", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = reason;

            arParams[9] = new NpgsqlParameter(":avscode", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = avsCode;

            arParams[10] = new NpgsqlParameter(":ccvcode", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = ccvCode;

            arParams[11] = new NpgsqlParameter(":cavcode", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = cavCode;

            arParams[12] = new NpgsqlParameter(":transactionid", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = transactionId;

            arParams[13] = new NpgsqlParameter(":transactiontype", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = transactionType;

            arParams[14] = new NpgsqlParameter(":method", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = method;

            arParams[15] = new NpgsqlParameter(":authcode", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = authCode;

            arParams[16] = new NpgsqlParameter(":amount", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = amount;

            arParams[17] = new NpgsqlParameter(":tax", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = tax;

            arParams[18] = new NpgsqlParameter(":duty", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = duty;

            arParams[19] = new NpgsqlParameter(":freight", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = freight;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_PlugNPaylog ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("siteguid = :siteguid, ");
            sqlCommand.Append("userguid = :userguid, ");
            sqlCommand.Append("storeguid = :storeguid, ");
            sqlCommand.Append("cartguid = :cartguid, ");
            sqlCommand.Append("rawresponse = :rawresponse, ");
            sqlCommand.Append("responsecode = :responsecode, ");
            sqlCommand.Append("responsereasoncode = :responsereasoncode, ");
            sqlCommand.Append("reason = :reason, ");
            sqlCommand.Append("avscode = :avscode, ");
            sqlCommand.Append("ccvcode = :ccvcode, ");
            sqlCommand.Append("cavcode = :cavcode, ");
            sqlCommand.Append("transactionid = :transactionid, ");
            sqlCommand.Append("transactiontype = :transactiontype, ");
            sqlCommand.Append("method = :method, ");
            sqlCommand.Append("authcode = :authcode, ");
            sqlCommand.Append("amount = :amount, ");
            sqlCommand.Append("tax = :tax, ");
            sqlCommand.Append("duty = :duty, ");
            sqlCommand.Append("freight = :freight ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("rowguid = :rowguid ");
            sqlCommand.Append(";");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_PlugNPayLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":rowguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_PlugNPaylog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("rowguid = :rowguid ");
            sqlCommand.Append(";");
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_PlugNPayLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetOne(Guid rowGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":rowguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_PlugNPaylog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("rowguid = :rowguid ");
            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_PlugNPayLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetByCart(Guid cartGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":cartguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_PlugNPaylog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cartguid = :cartguid ");
            sqlCommand.Append("ORDER BY createdutc ");
            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

       
    }
}
