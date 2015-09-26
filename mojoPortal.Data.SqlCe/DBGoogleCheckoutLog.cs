// Author:					Joe Audette
// Created:					2010-04-04
// Last Modified:			2012-03-04
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Data.SqlServerCe;

namespace mojoPortal.Data
{
    public static class DBGoogleCheckoutLog
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }


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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_GoogleCheckoutLog ");
            sqlCommand.Append("(");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("StoreGuid, ");
            sqlCommand.Append("CartGuid, ");
            sqlCommand.Append("NotificationType, ");
            sqlCommand.Append("RawResponse, ");
            sqlCommand.Append("SerialNumber, ");
            sqlCommand.Append("GTimestamp, ");
            sqlCommand.Append("OrderNumber, ");
            sqlCommand.Append("BuyerId, ");
            sqlCommand.Append("FullfillState, ");
            sqlCommand.Append("FinanceState, ");
            sqlCommand.Append("EmailListOptIn, ");
            sqlCommand.Append("AvsResponse, ");
            sqlCommand.Append("CvnResponse, ");
            sqlCommand.Append("AuthExpDate, ");
            sqlCommand.Append("AuthAmt, ");
            sqlCommand.Append("DiscountTotal, ");
            sqlCommand.Append("ShippingTotal, ");
            sqlCommand.Append("TaxTotal, ");
            sqlCommand.Append("OrderTotal, ");
            sqlCommand.Append("LatestChgAmt, ");
            sqlCommand.Append("TotalChgAmt, ");
            sqlCommand.Append("LatestRefundAmt, ");
            sqlCommand.Append("TotalRefundAmt, ");
            sqlCommand.Append("LatestChargeback, ");
            sqlCommand.Append("TotalChargeback, ");
            sqlCommand.Append("CartXml, ");
            sqlCommand.Append("ProviderName ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@RowGuid, ");
            sqlCommand.Append("@CreatedUtc, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@StoreGuid, ");
            sqlCommand.Append("@CartGuid, ");
            sqlCommand.Append("@NotificationType, ");
            sqlCommand.Append("@RawResponse, ");
            sqlCommand.Append("@SerialNumber, ");
            sqlCommand.Append("@GTimestamp, ");
            sqlCommand.Append("@OrderNumber, ");
            sqlCommand.Append("@BuyerId, ");
            sqlCommand.Append("@FullfillState, ");
            sqlCommand.Append("@FinanceState, ");
            sqlCommand.Append("@EmailListOptIn, ");
            sqlCommand.Append("@AvsResponse, ");
            sqlCommand.Append("@CvnResponse, ");
            sqlCommand.Append("@AuthExpDate, ");
            sqlCommand.Append("@AuthAmt, ");
            sqlCommand.Append("@DiscountTotal, ");
            sqlCommand.Append("@ShippingTotal, ");
            sqlCommand.Append("@TaxTotal, ");
            sqlCommand.Append("@OrderTotal, ");
            sqlCommand.Append("@LatestChgAmt, ");
            sqlCommand.Append("@TotalChgAmt, ");
            sqlCommand.Append("@LatestRefundAmt, ");
            sqlCommand.Append("@TotalRefundAmt, ");
            sqlCommand.Append("@LatestChargeback, ");
            sqlCommand.Append("@TotalChargeback, ");
            sqlCommand.Append("@CartXml, ");
            sqlCommand.Append("@ProviderName ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[31];

            arParams[0] = new SqlCeParameter("@RowGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid;

            arParams[1] = new SqlCeParameter("@CreatedUtc", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = createdUtc;

            arParams[2] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid;

            arParams[3] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid;

            arParams[4] = new SqlCeParameter("@StoreGuid", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = storeGuid;

            arParams[5] = new SqlCeParameter("@CartGuid", SqlDbType.UniqueIdentifier);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = cartGuid;

            arParams[6] = new SqlCeParameter("@NotificationType", SqlDbType.NVarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = notificationType;

            arParams[7] = new SqlCeParameter("@RawResponse", SqlDbType.NText);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = rawResponse;

            arParams[8] = new SqlCeParameter("@SerialNumber", SqlDbType.NVarChar, 50);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = serialNumber;

            arParams[9] = new SqlCeParameter("@GTimestamp", SqlDbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = gTimestamp;

            arParams[10] = new SqlCeParameter("@OrderNumber", SqlDbType.NVarChar, 50);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = orderNumber;

            arParams[11] = new SqlCeParameter("@BuyerId", SqlDbType.NVarChar, 50);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = buyerId;

            arParams[12] = new SqlCeParameter("@FullfillState", SqlDbType.NVarChar, 50);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = fullfillState;

            arParams[13] = new SqlCeParameter("@FinanceState", SqlDbType.NVarChar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = financeState;

            arParams[14] = new SqlCeParameter("@EmailListOptIn", SqlDbType.Bit);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = emailListOptIn;

            arParams[15] = new SqlCeParameter("@AvsResponse", SqlDbType.NVarChar, 5);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = avsResponse;

            arParams[16] = new SqlCeParameter("@CvnResponse", SqlDbType.NVarChar, 5);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = cvnResponse;

            arParams[17] = new SqlCeParameter("@AuthExpDate", SqlDbType.DateTime);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = authExpDate;

            arParams[18] = new SqlCeParameter("@AuthAmt", SqlDbType.Decimal);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = authAmt;

            arParams[19] = new SqlCeParameter("@DiscountTotal", SqlDbType.Decimal);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = discountTotal;

            arParams[20] = new SqlCeParameter("@ShippingTotal", SqlDbType.Decimal);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = shippingTotal;

            arParams[21] = new SqlCeParameter("@TaxTotal", SqlDbType.Decimal);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = taxTotal;

            arParams[22] = new SqlCeParameter("@OrderTotal", SqlDbType.Decimal);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = orderTotal;

            arParams[23] = new SqlCeParameter("@LatestChgAmt", SqlDbType.Decimal);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = latestChgAmt;

            arParams[24] = new SqlCeParameter("@TotalChgAmt", SqlDbType.Decimal);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = totalChgAmt;

            arParams[25] = new SqlCeParameter("@LatestRefundAmt", SqlDbType.Decimal);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = latestRefundAmt;

            arParams[26] = new SqlCeParameter("@TotalRefundAmt", SqlDbType.Decimal);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = totalRefundAmt;

            arParams[27] = new SqlCeParameter("@LatestChargeback", SqlDbType.Decimal);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = latestChargeback;

            arParams[28] = new SqlCeParameter("@TotalChargeback", SqlDbType.Decimal);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = totalChargeback;

            arParams[29] = new SqlCeParameter("@CartXml", SqlDbType.NText);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = cartXml;

            arParams[30] = new SqlCeParameter("@ProviderName", SqlDbType.NVarChar, 255);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = providerName;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_GoogleCheckoutLog ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("CreatedUtc = @CreatedUtc, ");
            sqlCommand.Append("SiteGuid = @SiteGuid, ");
            sqlCommand.Append("UserGuid = @UserGuid, ");
            sqlCommand.Append("StoreGuid = @StoreGuid, ");
            sqlCommand.Append("CartGuid = @CartGuid, ");
            sqlCommand.Append("NotificationType = @NotificationType, ");
            sqlCommand.Append("RawResponse = @RawResponse, ");
            sqlCommand.Append("SerialNumber = @SerialNumber, ");
            sqlCommand.Append("GTimestamp = @GTimestamp, ");
            sqlCommand.Append("OrderNumber = @OrderNumber, ");
            sqlCommand.Append("BuyerId = @BuyerId, ");
            sqlCommand.Append("FullfillState = @FullfillState, ");
            sqlCommand.Append("FinanceState = @FinanceState, ");
            sqlCommand.Append("EmailListOptIn = @EmailListOptIn, ");
            sqlCommand.Append("AvsResponse = @AvsResponse, ");
            sqlCommand.Append("CvnResponse = @CvnResponse, ");
            sqlCommand.Append("AuthExpDate = @AuthExpDate, ");
            sqlCommand.Append("AuthAmt = @AuthAmt, ");
            sqlCommand.Append("DiscountTotal = @DiscountTotal, ");
            sqlCommand.Append("ShippingTotal = @ShippingTotal, ");
            sqlCommand.Append("TaxTotal = @TaxTotal, ");
            sqlCommand.Append("OrderTotal = @OrderTotal, ");
            sqlCommand.Append("LatestChgAmt = @LatestChgAmt, ");
            sqlCommand.Append("TotalChgAmt = @TotalChgAmt, ");
            sqlCommand.Append("LatestRefundAmt = @LatestRefundAmt, ");
            sqlCommand.Append("TotalRefundAmt = @TotalRefundAmt, ");
            sqlCommand.Append("LatestChargeback = @LatestChargeback, ");
            sqlCommand.Append("TotalChargeback = @TotalChargeback, ");
            sqlCommand.Append("CartXml = @CartXml ");
      

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RowGuid = @RowGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[30];

            arParams[0] = new SqlCeParameter("@RowGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid;

            arParams[1] = new SqlCeParameter("@CreatedUtc", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = createdUtc;

            arParams[2] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid;

            arParams[3] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid;

            arParams[4] = new SqlCeParameter("@StoreGuid", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = storeGuid;

            arParams[5] = new SqlCeParameter("@CartGuid", SqlDbType.UniqueIdentifier);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = cartGuid;

            arParams[6] = new SqlCeParameter("@NotificationType", SqlDbType.NVarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = notificationType;

            arParams[7] = new SqlCeParameter("@RawResponse", SqlDbType.NText);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = rawResponse;

            arParams[8] = new SqlCeParameter("@SerialNumber", SqlDbType.NVarChar, 50);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = serialNumber;

            arParams[9] = new SqlCeParameter("@GTimestamp", SqlDbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = gTimestamp;

            arParams[10] = new SqlCeParameter("@OrderNumber", SqlDbType.NVarChar, 50);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = orderNumber;

            arParams[11] = new SqlCeParameter("@BuyerId", SqlDbType.NVarChar, 50);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = buyerId;

            arParams[12] = new SqlCeParameter("@FullfillState", SqlDbType.NVarChar, 50);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = fullfillState;

            arParams[13] = new SqlCeParameter("@FinanceState", SqlDbType.NVarChar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = financeState;

            arParams[14] = new SqlCeParameter("@EmailListOptIn", SqlDbType.Bit);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = emailListOptIn;

            arParams[15] = new SqlCeParameter("@AvsResponse", SqlDbType.NVarChar, 5);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = avsResponse;

            arParams[16] = new SqlCeParameter("@CvnResponse", SqlDbType.NVarChar, 5);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = cvnResponse;

            arParams[17] = new SqlCeParameter("@AuthExpDate", SqlDbType.DateTime);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = authExpDate;

            arParams[18] = new SqlCeParameter("@AuthAmt", SqlDbType.Decimal);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = authAmt;

            arParams[19] = new SqlCeParameter("@DiscountTotal", SqlDbType.Decimal);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = discountTotal;

            arParams[20] = new SqlCeParameter("@ShippingTotal", SqlDbType.Decimal);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = shippingTotal;

            arParams[21] = new SqlCeParameter("@TaxTotal", SqlDbType.Decimal);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = taxTotal;

            arParams[22] = new SqlCeParameter("@OrderTotal", SqlDbType.Decimal);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = orderTotal;

            arParams[23] = new SqlCeParameter("@LatestChgAmt", SqlDbType.Decimal);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = latestChgAmt;

            arParams[24] = new SqlCeParameter("@TotalChgAmt", SqlDbType.Decimal);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = totalChgAmt;

            arParams[25] = new SqlCeParameter("@LatestRefundAmt", SqlDbType.Decimal);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = latestRefundAmt;

            arParams[26] = new SqlCeParameter("@TotalRefundAmt", SqlDbType.Decimal);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = totalRefundAmt;

            arParams[27] = new SqlCeParameter("@LatestChargeback", SqlDbType.Decimal);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = latestChargeback;

            arParams[28] = new SqlCeParameter("@TotalChargeback", SqlDbType.Decimal);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = totalChargeback;

            arParams[29] = new SqlCeParameter("@CartXml", SqlDbType.NText);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = cartXml;

            
            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GoogleCheckoutLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = @RowGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@RowGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByCart(Guid cartGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GoogleCheckoutLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CartGuid = @CartGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@CartGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GoogleCheckoutLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByStore(Guid storeGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GoogleCheckoutLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = @StoreGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@StoreGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GoogleCheckoutLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = @RowGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@RowGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetMostRecentByOrder(string googleOrderId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GoogleCheckoutLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("OrderNumber = @OrderNumber ");
            sqlCommand.Append("AND CartGuid <> '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("AND NotificationType = 'NewOrderNotification' ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("CreatedUtc desc ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@OrderNumber", SqlDbType.NVarChar, 50);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = googleOrderId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the ws_GoogleCheckoutLog table.
        /// </summary>
        public static int GetCountByCart(Guid cartGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_GoogleCheckoutLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CartGuid = @CartGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@CartGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public static int GetCountByStore(Guid storeGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_GoogleCheckoutLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = @StoreGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@StoreGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") * ");

            sqlCommand.Append("FROM	mp_GoogleCheckoutLog  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CartGuid = @CartGuid ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("CreatedUtc DESC  ");

            sqlCommand.Append(") AS t1 ");
            //sqlCommand.Append("ORDER BY  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@CartGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a page of data from the ws_GoogleCheckoutLog table.
        /// </summary>
        /// <param name="storeGuid">The storeGuid</param>
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") * ");

            sqlCommand.Append("FROM	mp_GoogleCheckoutLog  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = @StoreGuid ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("CreatedUtc DESC  ");

            sqlCommand.Append(") AS t1 ");
            //sqlCommand.Append("ORDER BY  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@StoreGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


    }
}
