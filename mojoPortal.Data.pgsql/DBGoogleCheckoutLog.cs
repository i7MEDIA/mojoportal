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

using System;
using System.Text;
using System.Data;
using Npgsql;

namespace mojoPortal.Data
{
    public static class DBGoogleCheckoutLog
    {
       
        /// <summary>
        /// Inserts a row in the mp_GoogleCheckoutLog table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="storeGuid"> storeGuid </param>
        /// <param name="cartGuid"> cartGuid </param>
        /// <param name="notificationType"> notificationType </param>
        /// <param name="rawResponse"> rawResponse </param>
        /// <param name="serialNumber"> serialNumber </param>
        /// <param name="gTimestamp"> gTimestamp </param>
        /// <param name="orderNumber"> orderNumber </param>
        /// <param name="buyerId"> buyerId </param>
        /// <param name="fullfillState"> fullfillState </param>
        /// <param name="financeState"> financeState </param>
        /// <param name="emailListOptIn"> emailListOptIn </param>
        /// <param name="avsResponse"> avsResponse </param>
        /// <param name="cvnResponse"> cvnResponse </param>
        /// <param name="authExpDate"> authExpDate </param>
        /// <param name="authAmt"> authAmt </param>
        /// <param name="discountTotal"> discountTotal </param>
        /// <param name="shippingTotal"> shippingTotal </param>
        /// <param name="taxTotal"> taxTotal </param>
        /// <param name="orderTotal"> orderTotal </param>
        /// <param name="latestChgAmt"> latestChgAmt </param>
        /// <param name="totalChgAmt"> totalChgAmt </param>
        /// <param name="latestRefundAmt"> latestRefundAmt </param>
        /// <param name="totalRefundAmt"> totalRefundAmt </param>
        /// <param name="latestChargeback"> latestChargeback </param>
        /// <param name="totalChargeback"> totalChargeback </param>
        /// <param name="cartXml"> cartXml </param>
        /// <returns>int</returns>
        public static int Create(
            Guid rowGuid,
            DateTime createdUtc,
            Guid siteGuid,
            Guid userGuid,
            Guid storeGuid,
            Guid cartGuid,
            string notificationType,
            string rawResponse,
            string serialNumber,
            DateTime gTimestamp,
            string orderNumber,
            string buyerId,
            string fullfillState,
            string financeState,
            bool emailListOptIn,
            string avsResponse,
            string cvnResponse,
            DateTime authExpDate,
            decimal authAmt,
            decimal discountTotal,
            decimal shippingTotal,
            decimal taxTotal,
            decimal orderTotal,
            decimal latestChgAmt,
            decimal totalChgAmt,
            decimal latestRefundAmt,
            decimal totalRefundAmt,
            decimal latestChargeback,
            decimal totalChargeback,
            string cartXml,
            string providerName)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[31];

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

            arParams[6] = new NpgsqlParameter(":notificationtype", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = notificationType;

            arParams[7] = new NpgsqlParameter(":rawresponse", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = rawResponse;

            arParams[8] = new NpgsqlParameter(":serialnumber", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = serialNumber;

            arParams[9] = new NpgsqlParameter(":gtimestamp", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = gTimestamp;

            arParams[10] = new NpgsqlParameter(":ordernumber", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = orderNumber;

            arParams[11] = new NpgsqlParameter(":buyerid", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = buyerId;

            arParams[12] = new NpgsqlParameter(":fullfillstate", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = fullfillState;

            arParams[13] = new NpgsqlParameter(":financestate", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = financeState;

            arParams[14] = new NpgsqlParameter(":emaillistoptin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = emailListOptIn;

            arParams[15] = new NpgsqlParameter(":avsresponse", NpgsqlTypes.NpgsqlDbType.Varchar, 5);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = avsResponse;

            arParams[16] = new NpgsqlParameter(":cvnresponse", NpgsqlTypes.NpgsqlDbType.Varchar, 5);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = cvnResponse;

            arParams[17] = new NpgsqlParameter(":authexpdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = authExpDate;

            arParams[18] = new NpgsqlParameter(":authamt", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = authAmt;

            arParams[19] = new NpgsqlParameter(":discounttotal", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = discountTotal;

            arParams[20] = new NpgsqlParameter(":shippingtotal", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = shippingTotal;

            arParams[21] = new NpgsqlParameter(":taxtotal", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = taxTotal;

            arParams[22] = new NpgsqlParameter(":ordertotal", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = orderTotal;

            arParams[23] = new NpgsqlParameter(":latestchgamt", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = latestChgAmt;

            arParams[24] = new NpgsqlParameter(":totalchgamt", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = totalChgAmt;

            arParams[25] = new NpgsqlParameter(":latestrefundamt", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = latestRefundAmt;

            arParams[26] = new NpgsqlParameter(":totalrefundamt", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = totalRefundAmt;

            arParams[27] = new NpgsqlParameter(":latestchargeback", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = latestChargeback;

            arParams[28] = new NpgsqlParameter(":totalchargeback", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = totalChargeback;

            arParams[29] = new NpgsqlParameter(":cartxml", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = cartXml;

            arParams[30] = new NpgsqlParameter(":providername", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = providerName;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_googlecheckoutlog (");
            sqlCommand.Append("rowguid, ");
            sqlCommand.Append("createdutc, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("storeguid, ");
            sqlCommand.Append("cartguid, ");
            sqlCommand.Append("notificationtype, ");
            sqlCommand.Append("rawresponse, ");
            sqlCommand.Append("serialnumber, ");
            sqlCommand.Append("gtimestamp, ");
            sqlCommand.Append("ordernumber, ");
            sqlCommand.Append("buyerid, ");
            sqlCommand.Append("fullfillstate, ");
            sqlCommand.Append("financestate, ");
            sqlCommand.Append("emaillistoptin, ");
            sqlCommand.Append("avsresponse, ");
            sqlCommand.Append("cvnresponse, ");
            sqlCommand.Append("authexpdate, ");
            sqlCommand.Append("authamt, ");
            sqlCommand.Append("discounttotal, ");
            sqlCommand.Append("shippingtotal, ");
            sqlCommand.Append("taxtotal, ");
            sqlCommand.Append("ordertotal, ");
            sqlCommand.Append("latestchgamt, ");
            sqlCommand.Append("totalchgamt, ");
            sqlCommand.Append("latestrefundamt, ");
            sqlCommand.Append("totalrefundamt, ");
            sqlCommand.Append("latestchargeback, ");
            sqlCommand.Append("totalchargeback, ");
            sqlCommand.Append("cartxml, ");
            sqlCommand.Append("providername )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":rowguid, ");
            sqlCommand.Append(":createdutc, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":userguid, ");
            sqlCommand.Append(":storeguid, ");
            sqlCommand.Append(":cartguid, ");
            sqlCommand.Append(":notificationtype, ");
            sqlCommand.Append(":rawresponse, ");
            sqlCommand.Append(":serialnumber, ");
            sqlCommand.Append(":gtimestamp, ");
            sqlCommand.Append(":ordernumber, ");
            sqlCommand.Append(":buyerid, ");
            sqlCommand.Append(":fullfillstate, ");
            sqlCommand.Append(":financestate, ");
            sqlCommand.Append(":emaillistoptin, ");
            sqlCommand.Append(":avsresponse, ");
            sqlCommand.Append(":cvnresponse, ");
            sqlCommand.Append(":authexpdate, ");
            sqlCommand.Append(":authamt, ");
            sqlCommand.Append(":discounttotal, ");
            sqlCommand.Append(":shippingtotal, ");
            sqlCommand.Append(":taxtotal, ");
            sqlCommand.Append(":ordertotal, ");
            sqlCommand.Append(":latestchgamt, ");
            sqlCommand.Append(":totalchgamt, ");
            sqlCommand.Append(":latestrefundamt, ");
            sqlCommand.Append(":totalrefundamt, ");
            sqlCommand.Append(":latestchargeback, ");
            sqlCommand.Append(":totalchargeback, ");
            sqlCommand.Append(":cartxml, ");
            sqlCommand.Append(":providername ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_GoogleCheckoutLog table. Returns true if row updated.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="storeGuid"> storeGuid </param>
        /// <param name="cartGuid"> cartGuid </param>
        /// <param name="notificationType"> notificationType </param>
        /// <param name="rawResponse"> rawResponse </param>
        /// <param name="serialNumber"> serialNumber </param>
        /// <param name="gTimestamp"> gTimestamp </param>
        /// <param name="orderNumber"> orderNumber </param>
        /// <param name="buyerId"> buyerId </param>
        /// <param name="fullfillState"> fullfillState </param>
        /// <param name="financeState"> financeState </param>
        /// <param name="emailListOptIn"> emailListOptIn </param>
        /// <param name="avsResponse"> avsResponse </param>
        /// <param name="cvnResponse"> cvnResponse </param>
        /// <param name="authExpDate"> authExpDate </param>
        /// <param name="authAmt"> authAmt </param>
        /// <param name="discountTotal"> discountTotal </param>
        /// <param name="shippingTotal"> shippingTotal </param>
        /// <param name="taxTotal"> taxTotal </param>
        /// <param name="orderTotal"> orderTotal </param>
        /// <param name="latestChgAmt"> latestChgAmt </param>
        /// <param name="totalChgAmt"> totalChgAmt </param>
        /// <param name="latestRefundAmt"> latestRefundAmt </param>
        /// <param name="totalRefundAmt"> totalRefundAmt </param>
        /// <param name="latestChargeback"> latestChargeback </param>
        /// <param name="totalChargeback"> totalChargeback </param>
        /// <param name="cartXml"> cartXml </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid rowGuid,
            DateTime createdUtc,
            Guid siteGuid,
            Guid userGuid,
            Guid storeGuid,
            Guid cartGuid,
            string notificationType,
            string rawResponse,
            string serialNumber,
            DateTime gTimestamp,
            string orderNumber,
            string buyerId,
            string fullfillState,
            string financeState,
            bool emailListOptIn,
            string avsResponse,
            string cvnResponse,
            DateTime authExpDate,
            decimal authAmt,
            decimal discountTotal,
            decimal shippingTotal,
            decimal taxTotal,
            decimal orderTotal,
            decimal latestChgAmt,
            decimal totalChgAmt,
            decimal latestRefundAmt,
            decimal totalRefundAmt,
            decimal latestChargeback,
            decimal totalChargeback,
            string cartXml)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[30];

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

            arParams[6] = new NpgsqlParameter(":notificationtype", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = notificationType;

            arParams[7] = new NpgsqlParameter(":rawresponse", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = rawResponse;

            arParams[8] = new NpgsqlParameter(":serialnumber", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = serialNumber;

            arParams[9] = new NpgsqlParameter(":gtimestamp", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = gTimestamp;

            arParams[10] = new NpgsqlParameter(":ordernumber", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = orderNumber;

            arParams[11] = new NpgsqlParameter(":buyerid", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = buyerId;

            arParams[12] = new NpgsqlParameter(":fullfillstate", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = fullfillState;

            arParams[13] = new NpgsqlParameter(":financestate", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = financeState;

            arParams[14] = new NpgsqlParameter(":emaillistoptin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = emailListOptIn;

            arParams[15] = new NpgsqlParameter(":avsresponse", NpgsqlTypes.NpgsqlDbType.Varchar, 5);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = avsResponse;

            arParams[16] = new NpgsqlParameter(":cvnresponse", NpgsqlTypes.NpgsqlDbType.Varchar, 5);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = cvnResponse;

            arParams[17] = new NpgsqlParameter(":authexpdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = authExpDate;

            arParams[18] = new NpgsqlParameter(":authamt", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = authAmt;

            arParams[19] = new NpgsqlParameter(":discounttotal", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = discountTotal;

            arParams[20] = new NpgsqlParameter(":shippingtotal", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = shippingTotal;

            arParams[21] = new NpgsqlParameter(":taxtotal", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = taxTotal;

            arParams[22] = new NpgsqlParameter(":ordertotal", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = orderTotal;

            arParams[23] = new NpgsqlParameter(":latestchgamt", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = latestChgAmt;

            arParams[24] = new NpgsqlParameter(":totalchgamt", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = totalChgAmt;

            arParams[25] = new NpgsqlParameter(":latestrefundamt", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = latestRefundAmt;

            arParams[26] = new NpgsqlParameter(":totalrefundamt", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = totalRefundAmt;

            arParams[27] = new NpgsqlParameter(":latestchargeback", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = latestChargeback;

            arParams[28] = new NpgsqlParameter(":totalchargeback", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = totalChargeback;

            arParams[29] = new NpgsqlParameter(":cartxml", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = cartXml;



            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_googlecheckoutlog ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("createdutc = :createdutc, ");
            sqlCommand.Append("siteguid = :siteguid, ");
            sqlCommand.Append("userguid = :userguid, ");
            sqlCommand.Append("storeguid = :storeguid, ");
            sqlCommand.Append("cartguid = :cartguid, ");
            sqlCommand.Append("notificationtype = :notificationtype, ");
            sqlCommand.Append("rawresponse = :rawresponse, ");
            sqlCommand.Append("serialnumber = :serialnumber, ");
            sqlCommand.Append("gtimestamp = :gtimestamp, ");
            sqlCommand.Append("ordernumber = :ordernumber, ");
            sqlCommand.Append("buyerid = :buyerid, ");
            sqlCommand.Append("fullfillstate = :fullfillstate, ");
            sqlCommand.Append("financestate = :financestate, ");
            sqlCommand.Append("emaillistoptin = :emaillistoptin, ");
            sqlCommand.Append("avsresponse = :avsresponse, ");
            sqlCommand.Append("cvnresponse = :cvnresponse, ");
            sqlCommand.Append("authexpdate = :authexpdate, ");
            sqlCommand.Append("authamt = :authamt, ");
            sqlCommand.Append("discounttotal = :discounttotal, ");
            sqlCommand.Append("shippingtotal = :shippingtotal, ");
            sqlCommand.Append("taxtotal = :taxtotal, ");
            sqlCommand.Append("ordertotal = :ordertotal, ");
            sqlCommand.Append("latestchgamt = :latestchgamt, ");
            sqlCommand.Append("totalchgamt = :totalchgamt, ");
            sqlCommand.Append("latestrefundamt = :latestrefundamt, ");
            sqlCommand.Append("totalrefundamt = :totalrefundamt, ");
            sqlCommand.Append("latestchargeback = :latestchargeback, ");
            sqlCommand.Append("totalchargeback = :totalchargeback, ");
            sqlCommand.Append("cartxml = :cartxml ");

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
        /// Deletes a row from the mp_GoogleCheckoutLog table. Returns true if row deleted.
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
            sqlCommand.Append("DELETE FROM mp_googlecheckoutlog ");
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
            sqlCommand.Append("DELETE FROM mp_googlecheckoutlog ");
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
            sqlCommand.Append("DELETE FROM mp_googlecheckoutlog ");
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
            sqlCommand.Append("DELETE FROM mp_googlecheckoutlog ");
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
        /// Gets an IDataReader with one row from the mp_GoogleCheckoutLog table.
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
            sqlCommand.Append("FROM	mp_googlecheckoutlog ");
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
        /// Gets an IDataReader with one row from the mp_GoogleCheckoutLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetMostRecentByOrder(string googleOrderId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":ordernumber", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = googleOrderId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_googlecheckoutlog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ordernumber = :ordernumber ");
            sqlCommand.Append("AND cartguid <> '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("AND notificationtype = 'NewOrderNotification' ");
            sqlCommand.Append("ORDER BY createdutc DESC  ");
            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_GoogleCheckoutLog table.
        /// </summary>
        public static IDataReader GetAll()
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_googlecheckoutlog ");
            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null);


        }

        /// <summary>
        /// Gets a count of rows in the mp_GoogleCheckoutLog table.
        /// </summary>
        public static int GetCountByCart(Guid cartGuid)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_googlecheckoutlog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cartguid = :cartguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":cartguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));


        }

        /// <summary>
        /// Gets a count of rows in the mp_GoogleCheckoutLog table.
        /// </summary>
        public static int GetCountByStore(Guid storeGuid)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_googlecheckoutlog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("storeguid = :storeguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":storeguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));


        }


        /// <summary>
        /// Gets a page of data from the ws_GoogleCheckoutLog table.
        /// </summary>
        /// <param name="cartGuid">The cartGuid</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPageByCart(
            Guid cartGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountByCart(cartGuid);

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



            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter(":cartguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();

            arParams[1] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new NpgsqlParameter(":pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_googlecheckoutlog  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cartguid = :cartguid ");
            sqlCommand.Append("ORDER BY createdutc DESC  ");
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

        /// <summary>
        /// Gets a page of data from the mp_GoogleCheckoutLog table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPageByStore(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountByStore(storeGuid);

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



            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter(":storeguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            arParams[1] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new NpgsqlParameter(":pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_googlecheckoutlog  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("storeguid = :storeguid ");
            sqlCommand.Append("ORDER BY createdutc DESC  ");
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
