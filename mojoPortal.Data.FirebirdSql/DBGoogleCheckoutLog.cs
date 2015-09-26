/// Author:					Joe Audette
/// Created:				2008-06-22
/// Last Modified:			2012-03-04
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
    public static class DBGoogleCheckoutLog
    {
        
        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

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

            #region Bit Conversion

            int intEmailListOptIn;
            if (emailListOptIn)
            {
                intEmailListOptIn = 1;
            }
            else
            {
                intEmailListOptIn = 0;
            }


            #endregion

            FbParameter[] arParams = new FbParameter[31];


            arParams[0] = new FbParameter("@RowGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new FbParameter("@CreatedUtc", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = createdUtc;

            arParams[2] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new FbParameter("@StoreGuid", FbDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = storeGuid.ToString();

            arParams[5] = new FbParameter("@CartGuid", FbDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = cartGuid.ToString();

            arParams[6] = new FbParameter("@NotificationType", FbDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = notificationType;

            arParams[7] = new FbParameter("@RawResponse", FbDbType.VarChar);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = rawResponse;

            arParams[8] = new FbParameter("@SerialNumber", FbDbType.VarChar, 50);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = serialNumber;

            arParams[9] = new FbParameter("@GTimestamp", FbDbType.TimeStamp);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = gTimestamp;

            arParams[10] = new FbParameter("@OrderNumber", FbDbType.VarChar, 50);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = orderNumber;

            arParams[11] = new FbParameter("@BuyerId", FbDbType.VarChar, 50);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = buyerId;

            arParams[12] = new FbParameter("@FullfillState", FbDbType.VarChar, 50);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = fullfillState;

            arParams[13] = new FbParameter("@FinanceState", FbDbType.VarChar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = financeState;

            arParams[14] = new FbParameter("@EmailListOptIn", FbDbType.SmallInt);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = intEmailListOptIn;

            arParams[15] = new FbParameter("@AvsResponse", FbDbType.VarChar, 5);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = avsResponse;

            arParams[16] = new FbParameter("@CvnResponse", FbDbType.VarChar, 5);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = cvnResponse;

            arParams[17] = new FbParameter("@AuthExpDate", FbDbType.TimeStamp);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = authExpDate;

            arParams[18] = new FbParameter("@AuthAmt", FbDbType.Decimal);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = authAmt;

            arParams[19] = new FbParameter("@DiscountTotal", FbDbType.Decimal);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = discountTotal;

            arParams[20] = new FbParameter("@ShippingTotal", FbDbType.Decimal);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = shippingTotal;

            arParams[21] = new FbParameter("@TaxTotal", FbDbType.Decimal);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = taxTotal;

            arParams[22] = new FbParameter("@OrderTotal", FbDbType.Decimal);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = orderTotal;

            arParams[23] = new FbParameter("@LatestChgAmt", FbDbType.Decimal);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = latestChgAmt;

            arParams[24] = new FbParameter("@TotalChgAmt", FbDbType.Decimal);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = totalChgAmt;

            arParams[25] = new FbParameter("@LatestRefundAmt", FbDbType.Decimal);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = latestRefundAmt;

            arParams[26] = new FbParameter("@TotalRefundAmt", FbDbType.Decimal);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = totalRefundAmt;

            arParams[27] = new FbParameter("@LatestChargeback", FbDbType.Decimal);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = latestChargeback;

            arParams[28] = new FbParameter("@TotalChargeback", FbDbType.Decimal);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = totalChargeback;

            arParams[29] = new FbParameter("@CartXml", FbDbType.VarChar);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = cartXml;

            arParams[30] = new FbParameter("@ProviderName", FbDbType.VarChar, 255);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = providerName;


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_GoogleCheckoutLog (");
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
            sqlCommand.Append("CartXml ,");
            sqlCommand.Append("ProviderName )");


            sqlCommand.Append(" VALUES (");
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
            sqlCommand.Append("@CartXml ,");
            sqlCommand.Append("@ProviderName )");
            sqlCommand.Append(";");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            #region Bit Conversion

            int intEmailListOptIn;
            if (emailListOptIn)
            {
                intEmailListOptIn = 1;
            }
            else
            {
                intEmailListOptIn = 0;
            }


            #endregion

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
            FbParameter[] arParams = new FbParameter[30];

            arParams[0] = new FbParameter("@RowGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new FbParameter("@CreatedUtc", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = createdUtc;

            arParams[2] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new FbParameter("@StoreGuid", FbDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = storeGuid.ToString();

            arParams[5] = new FbParameter("@CartGuid", FbDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = cartGuid.ToString();

            arParams[6] = new FbParameter("@NotificationType", FbDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = notificationType;

            arParams[7] = new FbParameter("@RawResponse", FbDbType.VarChar);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = rawResponse;

            arParams[8] = new FbParameter("@SerialNumber", FbDbType.VarChar, 50);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = serialNumber;

            arParams[9] = new FbParameter("@GTimestamp", FbDbType.TimeStamp);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = gTimestamp;

            arParams[10] = new FbParameter("@OrderNumber", FbDbType.VarChar, 50);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = orderNumber;

            arParams[11] = new FbParameter("@BuyerId", FbDbType.VarChar, 50);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = buyerId;

            arParams[12] = new FbParameter("@FullfillState", FbDbType.VarChar, 50);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = fullfillState;

            arParams[13] = new FbParameter("@FinanceState", FbDbType.VarChar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = financeState;

            arParams[14] = new FbParameter("@EmailListOptIn", FbDbType.SmallInt);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = intEmailListOptIn;

            arParams[15] = new FbParameter("@AvsResponse", FbDbType.VarChar, 5);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = avsResponse;

            arParams[16] = new FbParameter("@CvnResponse", FbDbType.VarChar, 5);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = cvnResponse;

            arParams[17] = new FbParameter("@AuthExpDate", FbDbType.TimeStamp);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = authExpDate;

            arParams[18] = new FbParameter("@AuthAmt", FbDbType.Decimal);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = authAmt;

            arParams[19] = new FbParameter("@DiscountTotal", FbDbType.Decimal);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = discountTotal;

            arParams[20] = new FbParameter("@ShippingTotal", FbDbType.Decimal);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = shippingTotal;

            arParams[21] = new FbParameter("@TaxTotal", FbDbType.Decimal);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = taxTotal;

            arParams[22] = new FbParameter("@OrderTotal", FbDbType.Decimal);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = orderTotal;

            arParams[23] = new FbParameter("@LatestChgAmt", FbDbType.Decimal);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = latestChgAmt;

            arParams[24] = new FbParameter("@TotalChgAmt", FbDbType.Decimal);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = totalChgAmt;

            arParams[25] = new FbParameter("@LatestRefundAmt", FbDbType.Decimal);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = latestRefundAmt;

            arParams[26] = new FbParameter("@TotalRefundAmt", FbDbType.Decimal);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = totalRefundAmt;

            arParams[27] = new FbParameter("@LatestChargeback", FbDbType.Decimal);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = latestChargeback;

            arParams[28] = new FbParameter("@TotalChargeback", FbDbType.Decimal);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = totalChargeback;

            arParams[29] = new FbParameter("@CartXml", FbDbType.VarChar);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = cartXml;


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@RowGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@CartGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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

        public static bool DeleteByStore(Guid storeGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GoogleCheckoutLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = @StoreGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@StoreGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@RowGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetMostRecentByOrder(string googleOrderId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST 1  * ");
            sqlCommand.Append("FROM	mp_GoogleCheckoutLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("OrderNumber = @OrderNumber ");
            sqlCommand.Append("AND CartGuid <> '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("AND NotificationType = 'NewOrderNotification' ");
            sqlCommand.Append(" ");

            sqlCommand.Append("ORDER BY CreatedUtc DESC  ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@OrderNumber", FbDbType.VarChar, 50);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = googleOrderId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
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

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@CartGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
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
            sqlCommand.Append("FROM	mp_GoogleCheckoutLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = @StoreGuid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@StoreGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_GoogleCheckoutLog table.
        /// </summary>
        public static IDataReader GetAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GoogleCheckoutLog ");
            sqlCommand.Append(";");

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                null);

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
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_GoogleCheckoutLog  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CartGuid = @CartGuid ");
            sqlCommand.Append("ORDER BY CreatedUtc DESC  ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@CartGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_GoogleCheckoutLog  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = @StoreGuid ");
            sqlCommand.Append("ORDER BY CreatedUtc DESC  ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@StoreGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }
    }
}
