/// Author:					
/// Created:				2008-06-22
/// Last Modified:			2012-08-11
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.
/// 

using System;
using System.Data;
using System.Text;
using Npgsql;

namespace mojoPortal.Data
{
    
    public static class DBPayPalLog
    {
       
        /// <summary>
        /// Inserts a row in the mp_PayPalLog table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="storeGuid"> storeGuid </param>
        /// <param name="cartGuid"> cartGuid </param>
        /// <param name="requestType"> requestType </param>
        /// <param name="apiVersion"> apiVersion </param>
        /// <param name="rawResponse"> rawResponse </param>
        /// <param name="token"> token </param>
        /// <param name="payerId"> payerId </param>
        /// <param name="transactionId"> transactionId </param>
        /// <param name="paymentType"> paymentType </param>
        /// <param name="paymentStatus"> paymentStatus </param>
        /// <param name="pendingReason"> pendingReason </param>
        /// <param name="reasonCode"> reasonCode </param>
        /// <param name="currencyCode"> currencyCode </param>
        /// <param name="exchangeRate"> exchangeRate </param>
        /// <param name="cartTotal"> cartTotal </param>
        /// <param name="payPalAmt"> payPalAmt </param>
        /// <param name="taxAmt"> taxAmt </param>
        /// <param name="feeAmt"> feeAmt </param>
        /// <param name="settleAmt"> settleAmt </param>
        /// <returns>int</returns>
        public static int Create(
            Guid rowGuid,
            DateTime createdUtc,
            Guid siteGuid,
            Guid userGuid,
            Guid storeGuid,
            Guid cartGuid,
            string requestType,
            string apiVersion,
            string rawResponse,
            string token,
            string payerId,
            string transactionId,
            string paymentType,
            string paymentStatus,
            string pendingReason,
            string reasonCode,
            string currencyCode,
            decimal exchangeRate,
            decimal cartTotal,
            decimal payPalAmt,
            decimal taxAmt,
            decimal feeAmt,
            decimal settleAmt,
            string providerName,
            string returnUrl,
            string serializedObject,
            string pdtProviderName,
            string ipnProviderName,
            string response)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[29];

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

            arParams[6] = new NpgsqlParameter(":requesttype", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = requestType;

            arParams[7] = new NpgsqlParameter(":apiversion", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = apiVersion;

            arParams[8] = new NpgsqlParameter(":rawresponse", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = rawResponse;

            arParams[9] = new NpgsqlParameter(":token", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = token;

            arParams[10] = new NpgsqlParameter(":payerid", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = payerId;

            arParams[11] = new NpgsqlParameter(":transactionid", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = transactionId;

            arParams[12] = new NpgsqlParameter(":paymenttype", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = paymentType;

            arParams[13] = new NpgsqlParameter(":paymentstatus", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = paymentStatus;

            arParams[14] = new NpgsqlParameter(":pendingreason", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = pendingReason;

            arParams[15] = new NpgsqlParameter(":reasoncode", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = reasonCode;

            arParams[16] = new NpgsqlParameter(":currencycode", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = currencyCode;

            arParams[17] = new NpgsqlParameter(":exchangerate", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = exchangeRate;

            arParams[18] = new NpgsqlParameter(":carttotal", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = cartTotal;

            arParams[19] = new NpgsqlParameter(":paypalamt", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = payPalAmt;

            arParams[20] = new NpgsqlParameter(":taxamt", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = taxAmt;

            arParams[21] = new NpgsqlParameter(":feeamt", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = feeAmt;

            arParams[22] = new NpgsqlParameter(":settleamt", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = settleAmt;

            arParams[23] = new NpgsqlParameter(":providername", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = providerName;

            arParams[24] = new NpgsqlParameter(":returnurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = returnUrl;

            arParams[25] = new NpgsqlParameter(":serializedobject", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = serializedObject;

            arParams[26] = new NpgsqlParameter(":pdtprovidername", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = pdtProviderName;

            arParams[27] = new NpgsqlParameter(":ipnprovidername", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = ipnProviderName;

            arParams[28] = new NpgsqlParameter(":response", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = response;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_paypallog (");
            sqlCommand.Append("rowguid, ");
            sqlCommand.Append("createdutc, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("storeguid, ");
            sqlCommand.Append("cartguid, ");
            sqlCommand.Append("requesttype, ");
            sqlCommand.Append("apiversion, ");
            sqlCommand.Append("rawresponse, ");
            sqlCommand.Append("token, ");
            sqlCommand.Append("payerid, ");
            sqlCommand.Append("transactionid, ");
            sqlCommand.Append("paymenttype, ");
            sqlCommand.Append("paymentstatus, ");
            sqlCommand.Append("pendingreason, ");
            sqlCommand.Append("reasoncode, ");
            sqlCommand.Append("currencycode, ");
            sqlCommand.Append("exchangerate, ");
            sqlCommand.Append("carttotal, ");
            sqlCommand.Append("paypalamt, ");
            sqlCommand.Append("taxamt, ");
            sqlCommand.Append("feeamt, ");
            sqlCommand.Append("providername, ");
            sqlCommand.Append("returnurl, ");
            sqlCommand.Append("serializedobject, ");

            sqlCommand.Append("pdtprovidername, ");
            sqlCommand.Append("ipnprovidername, ");
            sqlCommand.Append("response, ");

            sqlCommand.Append("settleamt )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":rowguid, ");
            sqlCommand.Append(":createdutc, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":userguid, ");
            sqlCommand.Append(":storeguid, ");
            sqlCommand.Append(":cartguid, ");
            sqlCommand.Append(":requesttype, ");
            sqlCommand.Append(":apiversion, ");
            sqlCommand.Append(":rawresponse, ");
            sqlCommand.Append(":token, ");
            sqlCommand.Append(":payerid, ");
            sqlCommand.Append(":transactionid, ");
            sqlCommand.Append(":paymenttype, ");
            sqlCommand.Append(":paymentstatus, ");
            sqlCommand.Append(":pendingreason, ");
            sqlCommand.Append(":reasoncode, ");
            sqlCommand.Append(":currencycode, ");
            sqlCommand.Append(":exchangerate, ");
            sqlCommand.Append(":carttotal, ");
            sqlCommand.Append(":paypalamt, ");
            sqlCommand.Append(":taxamt, ");
            sqlCommand.Append(":feeamt, ");
            sqlCommand.Append(":providername, ");
            sqlCommand.Append(":returnurl, ");
            sqlCommand.Append(":serializedobject, ");

            sqlCommand.Append(":pdtprovidername, ");
            sqlCommand.Append(":ipnprovidername, ");
            sqlCommand.Append(":response, ");

            sqlCommand.Append(":settleamt ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_PayPalLog table. Returns true if row updated.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="storeGuid"> storeGuid </param>
        /// <param name="cartGuid"> cartGuid </param>
        /// <param name="requestType"> requestType </param>
        /// <param name="apiVersion"> apiVersion </param>
        /// <param name="rawResponse"> rawResponse </param>
        /// <param name="token"> token </param>
        /// <param name="payerId"> payerId </param>
        /// <param name="transactionId"> transactionId </param>
        /// <param name="paymentType"> paymentType </param>
        /// <param name="paymentStatus"> paymentStatus </param>
        /// <param name="pendingReason"> pendingReason </param>
        /// <param name="reasonCode"> reasonCode </param>
        /// <param name="currencyCode"> currencyCode </param>
        /// <param name="exchangeRate"> exchangeRate </param>
        /// <param name="cartTotal"> cartTotal </param>
        /// <param name="payPalAmt"> payPalAmt </param>
        /// <param name="taxAmt"> taxAmt </param>
        /// <param name="feeAmt"> feeAmt </param>
        /// <param name="settleAmt"> settleAmt </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid rowGuid,
            DateTime createdUtc,
            Guid siteGuid,
            Guid userGuid,
            Guid storeGuid,
            Guid cartGuid,
            string requestType,
            string apiVersion,
            string rawResponse,
            string token,
            string payerId,
            string transactionId,
            string paymentType,
            string paymentStatus,
            string pendingReason,
            string reasonCode,
            string currencyCode,
            decimal exchangeRate,
            decimal cartTotal,
            decimal payPalAmt,
            decimal taxAmt,
            decimal feeAmt,
            decimal settleAmt)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[23];

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

            arParams[6] = new NpgsqlParameter(":requesttype", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = requestType;

            arParams[7] = new NpgsqlParameter(":apiversion", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = apiVersion;

            arParams[8] = new NpgsqlParameter(":rawresponse", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = rawResponse;

            arParams[9] = new NpgsqlParameter(":token", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = token;

            arParams[10] = new NpgsqlParameter(":payerid", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = payerId;

            arParams[11] = new NpgsqlParameter(":transactionid", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = transactionId;

            arParams[12] = new NpgsqlParameter(":paymenttype", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = paymentType;

            arParams[13] = new NpgsqlParameter(":paymentstatus", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = paymentStatus;

            arParams[14] = new NpgsqlParameter(":pendingreason", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = pendingReason;

            arParams[15] = new NpgsqlParameter(":reasoncode", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = reasonCode;

            arParams[16] = new NpgsqlParameter(":currencycode", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = currencyCode;

            arParams[17] = new NpgsqlParameter(":exchangerate", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = exchangeRate;

            arParams[18] = new NpgsqlParameter(":carttotal", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = cartTotal;

            arParams[19] = new NpgsqlParameter(":paypalamt", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = payPalAmt;

            arParams[20] = new NpgsqlParameter(":taxamt", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = taxAmt;

            arParams[21] = new NpgsqlParameter(":feeamt", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = feeAmt;

            arParams[22] = new NpgsqlParameter(":settleamt", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = settleAmt;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_paypallog ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("createdutc = :createdutc, ");
            sqlCommand.Append("siteguid = :siteguid, ");
            sqlCommand.Append("userguid = :userguid, ");
            sqlCommand.Append("storeguid = :storeguid, ");
            sqlCommand.Append("cartguid = :cartguid, ");
            sqlCommand.Append("requesttype = :requesttype, ");
            sqlCommand.Append("apiversion = :apiversion, ");
            sqlCommand.Append("rawresponse = :rawresponse, ");
            sqlCommand.Append("token = :token, ");
            sqlCommand.Append("payerid = :payerid, ");
            sqlCommand.Append("transactionid = :transactionid, ");
            sqlCommand.Append("paymenttype = :paymenttype, ");
            sqlCommand.Append("paymentstatus = :paymentstatus, ");
            sqlCommand.Append("pendingreason = :pendingreason, ");
            sqlCommand.Append("reasoncode = :reasoncode, ");
            sqlCommand.Append("currencycode = :currencycode, ");
            sqlCommand.Append("exchangerate = :exchangerate, ");
            sqlCommand.Append("carttotal = :carttotal, ");
            sqlCommand.Append("paypalamt = :paypalamt, ");
            sqlCommand.Append("taxamt = :taxamt, ");
            sqlCommand.Append("feeamt = :feeamt, ");
            sqlCommand.Append("settleamt = :settleamt ");

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
        /// Deletes a row from the mp_PayPalLog table. Returns true if row deleted.
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
            sqlCommand.Append("DELETE FROM mp_paypallog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("rowguid = :rowguid ");
            sqlCommand.Append(";");
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByCart(Guid cartGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":cartguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_paypallog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cartguid = :cartguid ");
            sqlCommand.Append(";");
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_paypallog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid = :siteguid ");
            sqlCommand.Append(";");
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByStore(Guid storeGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":storeguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_paypallog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("storeguid = :storeguid ");
            sqlCommand.Append(";");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_PayPalLog table.
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
            sqlCommand.Append("FROM	mp_paypallog ");
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
        /// Gets an IDataReader with rows from the mp_PayPalLog table.
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
            sqlCommand.Append("FROM	mp_paypallog ");
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

        /// <summary>
        /// Gets an IDataReader with one row from the mp_PayPalLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetSetExpressCheckout(string token)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":token", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = token;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_paypallog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("token = :token ");
            sqlCommand.Append("AND requesttype = 'SetExpressCheckout' ");
            sqlCommand.Append("ORDER BY createdutc DESC ");
            sqlCommand.Append("LIMIT 1 ");
            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_PayPalLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetMostRecentLog(Guid cartGuid, string requestType)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":cartguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();

            arParams[1] = new NpgsqlParameter(":requesttype", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = requestType;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_paypallog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cartguid = :cartguid ");
            sqlCommand.Append("AND (requesttype = :requesttype OR :requesttype = '') ");
            sqlCommand.Append("ORDER BY createdutc DESC ");
            sqlCommand.Append("LIMIT 1 ");
            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_PayPalLog table.
        /// </summary>
        public static IDataReader GetAll()
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_paypallog ");
            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null);

        }

        /// <summary>
        /// Gets a count of rows in the mp_PayPalLog table.
        /// </summary>
        public static int GetCount()
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_paypallog ");
            sqlCommand.Append(";");

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null));
        }

        /// <summary>
        /// Gets a page of data from the mp_PayPalLog table.
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

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageSize;

            arParams[1] = new NpgsqlParameter(":pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_paypallog  ");
            //sqlCommand.Append("WHERE  ");
            //sqlCommand.Append("ORDER BY  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }
    }
}
