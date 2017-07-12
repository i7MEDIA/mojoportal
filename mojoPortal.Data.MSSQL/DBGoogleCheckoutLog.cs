/// Author:					
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
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace mojoPortal.Data
{
    
    public static class DBGoogleCheckoutLog
    {
       

        ///// <summary>
        ///// Gets the connection string.
        ///// </summary>
        ///// <returns></returns>
        //private static string GetConnectionString()
        //{
        //    return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        //}


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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_GoogleCheckoutLog_Insert", 31);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            sph.DefineSqlParameter("@NotificationType", SqlDbType.NVarChar, 255, ParameterDirection.Input, notificationType);
            sph.DefineSqlParameter("@RawResponse", SqlDbType.NVarChar, -1, ParameterDirection.Input, rawResponse);
            sph.DefineSqlParameter("@SerialNumber", SqlDbType.NVarChar, 50, ParameterDirection.Input, serialNumber);
            sph.DefineSqlParameter("@GTimestamp", SqlDbType.DateTime, ParameterDirection.Input, gTimestamp);
            sph.DefineSqlParameter("@OrderNumber", SqlDbType.NVarChar, 50, ParameterDirection.Input, orderNumber);
            sph.DefineSqlParameter("@BuyerId", SqlDbType.NVarChar, 50, ParameterDirection.Input, buyerId);
            sph.DefineSqlParameter("@FullfillState", SqlDbType.NVarChar, 50, ParameterDirection.Input, fullfillState);
            sph.DefineSqlParameter("@FinanceState", SqlDbType.NVarChar, 50, ParameterDirection.Input, financeState);
            sph.DefineSqlParameter("@EmailListOptIn", SqlDbType.Bit, ParameterDirection.Input, emailListOptIn);
            sph.DefineSqlParameter("@AvsResponse", SqlDbType.NVarChar, 5, ParameterDirection.Input, avsResponse);
            sph.DefineSqlParameter("@CvnResponse", SqlDbType.NVarChar, 5, ParameterDirection.Input, cvnResponse);
            sph.DefineSqlParameter("@AuthExpDate", SqlDbType.DateTime, ParameterDirection.Input, authExpDate);
            sph.DefineSqlParameter("@AuthAmt", SqlDbType.Decimal, ParameterDirection.Input, authAmt);
            sph.DefineSqlParameter("@DiscountTotal", SqlDbType.Decimal, ParameterDirection.Input, discountTotal);
            sph.DefineSqlParameter("@ShippingTotal", SqlDbType.Decimal, ParameterDirection.Input, shippingTotal);
            sph.DefineSqlParameter("@TaxTotal", SqlDbType.Decimal, ParameterDirection.Input, taxTotal);
            sph.DefineSqlParameter("@OrderTotal", SqlDbType.Decimal, ParameterDirection.Input, orderTotal);
            sph.DefineSqlParameter("@LatestChgAmt", SqlDbType.Decimal, ParameterDirection.Input, latestChgAmt);
            sph.DefineSqlParameter("@TotalChgAmt", SqlDbType.Decimal, ParameterDirection.Input, totalChgAmt);
            sph.DefineSqlParameter("@LatestRefundAmt", SqlDbType.Decimal, ParameterDirection.Input, latestRefundAmt);
            sph.DefineSqlParameter("@TotalRefundAmt", SqlDbType.Decimal, ParameterDirection.Input, totalRefundAmt);
            sph.DefineSqlParameter("@LatestChargeback", SqlDbType.Decimal, ParameterDirection.Input, latestChargeback);
            sph.DefineSqlParameter("@TotalChargeback", SqlDbType.Decimal, ParameterDirection.Input, totalChargeback);
            sph.DefineSqlParameter("@CartXml", SqlDbType.NVarChar, -1, ParameterDirection.Input, cartXml);
            sph.DefineSqlParameter("@ProviderName", SqlDbType.NVarChar, 255, ParameterDirection.Input, providerName);
            int rowsAffected = sph.ExecuteNonQuery();
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_GoogleCheckoutLog_Update", 30);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            sph.DefineSqlParameter("@NotificationType", SqlDbType.NVarChar, 255, ParameterDirection.Input, notificationType);
            sph.DefineSqlParameter("@RawResponse", SqlDbType.NVarChar, -1, ParameterDirection.Input, rawResponse);
            sph.DefineSqlParameter("@SerialNumber", SqlDbType.NVarChar, 50, ParameterDirection.Input, serialNumber);
            sph.DefineSqlParameter("@GTimestamp", SqlDbType.DateTime, ParameterDirection.Input, gTimestamp);
            sph.DefineSqlParameter("@OrderNumber", SqlDbType.NVarChar, 50, ParameterDirection.Input, orderNumber);
            sph.DefineSqlParameter("@BuyerId", SqlDbType.NVarChar, 50, ParameterDirection.Input, buyerId);
            sph.DefineSqlParameter("@FullfillState", SqlDbType.NVarChar, 50, ParameterDirection.Input, fullfillState);
            sph.DefineSqlParameter("@FinanceState", SqlDbType.NVarChar, 50, ParameterDirection.Input, financeState);
            sph.DefineSqlParameter("@EmailListOptIn", SqlDbType.Bit, ParameterDirection.Input, emailListOptIn);
            sph.DefineSqlParameter("@AvsResponse", SqlDbType.NVarChar, 5, ParameterDirection.Input, avsResponse);
            sph.DefineSqlParameter("@CvnResponse", SqlDbType.NVarChar, 5, ParameterDirection.Input, cvnResponse);
            sph.DefineSqlParameter("@AuthExpDate", SqlDbType.DateTime, ParameterDirection.Input, authExpDate);
            sph.DefineSqlParameter("@AuthAmt", SqlDbType.Decimal, ParameterDirection.Input, authAmt);
            sph.DefineSqlParameter("@DiscountTotal", SqlDbType.Decimal, ParameterDirection.Input, discountTotal);
            sph.DefineSqlParameter("@ShippingTotal", SqlDbType.Decimal, ParameterDirection.Input, shippingTotal);
            sph.DefineSqlParameter("@TaxTotal", SqlDbType.Decimal, ParameterDirection.Input, taxTotal);
            sph.DefineSqlParameter("@OrderTotal", SqlDbType.Decimal, ParameterDirection.Input, orderTotal);
            sph.DefineSqlParameter("@LatestChgAmt", SqlDbType.Decimal, ParameterDirection.Input, latestChgAmt);
            sph.DefineSqlParameter("@TotalChgAmt", SqlDbType.Decimal, ParameterDirection.Input, totalChgAmt);
            sph.DefineSqlParameter("@LatestRefundAmt", SqlDbType.Decimal, ParameterDirection.Input, latestRefundAmt);
            sph.DefineSqlParameter("@TotalRefundAmt", SqlDbType.Decimal, ParameterDirection.Input, totalRefundAmt);
            sph.DefineSqlParameter("@LatestChargeback", SqlDbType.Decimal, ParameterDirection.Input, latestChargeback);
            sph.DefineSqlParameter("@TotalChargeback", SqlDbType.Decimal, ParameterDirection.Input, totalChargeback);
            sph.DefineSqlParameter("@CartXml", SqlDbType.NVarChar, -1, ParameterDirection.Input, cartXml);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_GoogleCheckoutLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_GoogleCheckoutLog_Delete", 1);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteByCart(Guid cartGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_GoogleCheckoutLog_DeleteByCart", 1);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_GoogleCheckoutLog_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteByStore(Guid storeGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_GoogleCheckoutLog_DeleteByStore", 1);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_GoogleCheckoutLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetOne(Guid rowGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_GoogleCheckoutLog_SelectOne", 1);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with one row from the ws_GoogleCheckoutLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetMostRecentByOrder(string googleOrderId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_GoogleCheckoutLog_SelectMostRecentByOrder", 1);
            sph.DefineSqlParameter("@OrderNumber", SqlDbType.NVarChar, 50, ParameterDirection.Input, googleOrderId);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets a count of rows in the ws_GoogleCheckoutLog table.
        /// </summary>
        public static int GetCountByCart(Guid cartGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_GoogleCheckoutLog_GetCountByCart", 1);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);

            return Convert.ToInt32(sph.ExecuteScalar());

        }

        /// <summary>
        /// Gets a count of rows in the ws_GoogleCheckoutLog table.
        /// </summary>
        public static int GetCountByStore(Guid storeGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_GoogleCheckoutLog_GetCount", 1);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);

            return Convert.ToInt32(sph.ExecuteScalar());

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_GoogleCheckoutLog table.
        /// </summary>
        public static IDataReader GetAll()
        {

            return SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_GoogleCheckoutLog_SelectAll",
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
            totalPages = 1;
            int totalRows
                = GetCountByCart(cartGuid);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_GoogleCheckoutLog_SelectPageByCart", 3);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

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
            totalPages = 1;
            int totalRows
                = GetCountByStore(storeGuid);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_GoogleCheckoutLog_SelectPage", 3);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

    }

}
