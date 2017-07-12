/// Author:					
/// Created:				2009-01-30
/// Last Modified:			2013-09-10
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
using System.Globalization;
using mojoPortal.Data;

namespace mojoPortal.Data
{
    public static class DBCommerceReport
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
        /// Inserts a row in the mp_CommerceReport table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="featureGuid"> featureGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="moduleTitle"> moduleTitle </param>
        /// <param name="orderGuid"> orderGuid </param>
        /// <param name="itemGuid"> itemGuid </param>
        /// <param name="itemName"> itemName </param>
        /// <param name="quantity"> quantity </param>
        /// <param name="price"> price </param>
        /// <param name="subTotal"> subTotal </param>
        /// <param name="orderDateUtc"> orderDateUtc </param>
        /// <param name="paymentMethod"> paymentMthod </param>
        /// <param name="iPAddress"> iPAddress </param>
        /// <param name="adminOrderLink"> adminOrderLink </param>
        /// <param name="userOrderLink"> userOrderLink </param>
        /// <param name="rowCreatedUtc"> rowCreatedUtc </param>
        /// <returns>int</returns>
        public static int Create(
            Guid rowGuid,
            Guid siteGuid,
            Guid userGuid,
            Guid featureGuid,
            Guid moduleGuid,
            string moduleTitle,
            Guid orderGuid,
            Guid itemGuid,
            string itemName,
            int quantity,
            decimal price,
            decimal subTotal,
            DateTime orderDateUtc,
            string paymentMethod,
            string iPAddress,
            string adminOrderLink,
            string userOrderLink,
            DateTime rowCreatedUtc,
            bool includeInAggregate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CommerceReport_Insert", 19);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@ModuleTitle", SqlDbType.NVarChar, 255, ParameterDirection.Input, moduleTitle);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@ItemName", SqlDbType.NVarChar, 255, ParameterDirection.Input, itemName);
            sph.DefineSqlParameter("@Quantity", SqlDbType.Int, ParameterDirection.Input, quantity);
            sph.DefineSqlParameter("@Price", SqlDbType.Decimal, ParameterDirection.Input, price);
            sph.DefineSqlParameter("@SubTotal", SqlDbType.Decimal, ParameterDirection.Input, subTotal);
            sph.DefineSqlParameter("@OrderDateUtc", SqlDbType.DateTime, ParameterDirection.Input, orderDateUtc);
            sph.DefineSqlParameter("@PaymentMethod", SqlDbType.NVarChar, 50, ParameterDirection.Input, paymentMethod);
            sph.DefineSqlParameter("@IPAddress", SqlDbType.NVarChar, 250, ParameterDirection.Input, iPAddress);
            sph.DefineSqlParameter("@AdminOrderLink", SqlDbType.NVarChar, 255, ParameterDirection.Input, adminOrderLink);
            sph.DefineSqlParameter("@UserOrderLink", SqlDbType.NVarChar, 255, ParameterDirection.Input, userOrderLink);
            sph.DefineSqlParameter("@RowCreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, rowCreatedUtc);
            sph.DefineSqlParameter("@IncludeInAggregate", SqlDbType.Bit, ParameterDirection.Input, includeInAggregate);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }

        /// <summary>
        /// Deletes a row from the mp_CommerceReport table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CommerceReport_Delete", 1);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_CommerceReport table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CommerceReport_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_CommerceReport table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByUser(Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CommerceReport_DeleteByUser", 1);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_CommerceReport table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByFeature(Guid featureGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CommerceReport_DeleteByFeature", 1);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_CommerceReport table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CommerceReport_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_CommerceReport table. Returns true if row deleted.
        /// </summary>
        /// <param name="orderGuid"> orderGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByOrder(Guid orderGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CommerceReport_DeleteByOrderGuid", 1);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static IDataReader GetSalesByYearByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_CommerceReport_GetSalesByYearByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetSalesByYearMonthBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_CommerceReport_GetSalesByYearMonthBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetSalesByYearMonthByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_CommerceReport_GetSalesByYearMonthByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetSalesByYearMonthByItem(Guid itemGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_CommerceReport_GetSalesByYearMonthByItem", 1);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetSalesByYearMonthByUser(Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_CommerceReport_GetSalesByYearMonthByUser", 1);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetSalesGroupedByModule(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_CommerceReport_GetSalesGroupedByModule", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetSalesGroupedByUser(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_CommerceReport_GetSalesGroupedByUser", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetItemSummary(Guid itemGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_CommerceReport_GetItemSummary", 1);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            return sph.ExecuteReader();
        }


        public static IDataReader GetItemRevenueBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_CommerceReport_GetItemRevenueBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);

            return sph.ExecuteReader();
        }

        public static IDataReader GetItemRevenueByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_CommerceReport_GetItemRevenueByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);

            return sph.ExecuteReader();
        }

        public static IDataReader GetItemRevenueByUser(Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_CommerceReport_GetItemRevenueByUser", 1);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);

            return sph.ExecuteReader();
        }

        public static decimal GetAllTimeRevenueBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_CommerceReport_GetAllTimeRevenueBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);

            decimal result = 0;

            try
            {
                result = Convert.ToDecimal(sph.ExecuteScalar(),CultureInfo.InvariantCulture);
            }
            catch (InvalidCastException) { }

            return result;
        }

        public static decimal GetAllTimeRevenueByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_CommerceReport_GetAllTimeRevenueByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);

            decimal result = 0;

            try
            {
                result = Convert.ToDecimal(sph.ExecuteScalar(), CultureInfo.InvariantCulture);
            }
            catch (InvalidCastException) { }

            return result;
        }

        /// <summary>
        /// Gets a count of rows in the mp_CommerceReport table.
        /// </summary>
        public static int GetItemCountByUser(Guid siteGuid, Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_CommerceReport_GetItemCountByUser", 2);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);

            return Convert.ToInt32(sph.ExecuteScalar());

        }

        /// <summary>
        /// Gets a page of data from the mp_CommerceReport table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetUserItemsPage(
            Guid siteGuid,
            Guid userGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetItemCountByUser(siteGuid, userGuid);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_CommerceReport_SelectUserItemsPage", 4);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

        

        /// <summary>
        /// Gets a count of rows in the mp_CommerceReport table.
        /// </summary>
        public static int GetDistinctItemCountByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_CommerceReport_GetCount", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);

            return Convert.ToInt32(sph.ExecuteScalar());

        }

        /// <summary>
        /// Gets a page of data from the mp_CommerceReport table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetItemsPageByModule(
            Guid moduleGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetDistinctItemCountByModule(moduleGuid);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_CommerceReport_SelectPage", 3);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }


        /// <summary>
        /// Gets a count of rows in the mp_CommerceReport table.
        /// </summary>
        public static int GetDistinctUserItemCount(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_CommerceReport_GetUserItemCountBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);

            return Convert.ToInt32(sph.ExecuteScalar());

        }

        /// <summary>
        /// Gets a page of data from the mp_CommerceReport table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetUserItemPageBySite(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetDistinctUserItemCount(siteGuid);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_CommerceReport_SelectUserItemPage", 3);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }


        /// <summary>
        /// Inserts a row in the mp_CommerceReportOrders table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="featureGuid"> featureGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="orderGuid"> orderGuid </param>
        /// <param name="billingFirstName"> billingFirstName </param>
        /// <param name="billingLastName"> billingLastName </param>
        /// <param name="billingCompany"> billingCompany </param>
        /// <param name="billingAddress1"> billingAddress1 </param>
        /// <param name="billingAddress2"> billingAddress2 </param>
        /// <param name="billingSuburb"> billingSuburb </param>
        /// <param name="billingCity"> billingCity </param>
        /// <param name="billingPostalCode"> billingPostalCode </param>
        /// <param name="billingState"> billingState </param>
        /// <param name="billingCountry"> billingCountry </param>
        /// <param name="paymentMethod"> paymentMethod </param>
        /// <param name="subTotal"> subTotal </param>
        /// <param name="taxTotal"> taxTotal </param>
        /// <param name="shippingTotal"> shippingTotal </param>
        /// <param name="orderTotal"> orderTotal </param>
        /// <param name="orderDateUtc"> orderDateUtc </param>
        /// <param name="adminOrderLink"> adminOrderLink </param>
        /// <param name="userOrderLink"> userOrderLink </param>
        /// <param name="rowCreatedUtc"> rowCreatedUtc </param>
        /// <returns>int</returns>
        public static int CreateOrder(
            Guid rowGuid,
            Guid siteGuid,
            Guid featureGuid,
            Guid moduleGuid,
            Guid userGuid,
            Guid orderGuid,
            string billingFirstName,
            string billingLastName,
            string billingCompany,
            string billingAddress1,
            string billingAddress2,
            string billingSuburb,
            string billingCity,
            string billingPostalCode,
            string billingState,
            string billingCountry,
            string paymentMethod,
            decimal subTotal,
            decimal taxTotal,
            decimal shippingTotal,
            decimal orderTotal,
            DateTime orderDateUtc,
            string adminOrderLink,
            string userOrderLink,
            DateTime rowCreatedUtc,
            bool includeInAggregate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CommerceReportOrders_Insert", 26);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            sph.DefineSqlParameter("@BillingFirstName", SqlDbType.NVarChar, 100, ParameterDirection.Input, billingFirstName);
            sph.DefineSqlParameter("@BillingLastName", SqlDbType.NVarChar, 50, ParameterDirection.Input, billingLastName);
            sph.DefineSqlParameter("@BillingCompany", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingCompany);
            sph.DefineSqlParameter("@BillingAddress1", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingAddress1);
            sph.DefineSqlParameter("@BillingAddress2", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingAddress2);
            sph.DefineSqlParameter("@BillingSuburb", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingSuburb);
            sph.DefineSqlParameter("@BillingCity", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingCity);
            sph.DefineSqlParameter("@BillingPostalCode", SqlDbType.NVarChar, 20, ParameterDirection.Input, billingPostalCode);
            sph.DefineSqlParameter("@BillingState", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingState);
            sph.DefineSqlParameter("@BillingCountry", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingCountry);
            sph.DefineSqlParameter("@PaymentMethod", SqlDbType.NVarChar, 50, ParameterDirection.Input, paymentMethod);
            sph.DefineSqlParameter("@SubTotal", SqlDbType.Decimal, ParameterDirection.Input, subTotal);
            sph.DefineSqlParameter("@TaxTotal", SqlDbType.Decimal, ParameterDirection.Input, taxTotal);
            sph.DefineSqlParameter("@ShippingTotal", SqlDbType.Decimal, ParameterDirection.Input, shippingTotal);
            sph.DefineSqlParameter("@OrderTotal", SqlDbType.Decimal, ParameterDirection.Input, orderTotal);
            sph.DefineSqlParameter("@OrderDateUtc", SqlDbType.DateTime, ParameterDirection.Input, orderDateUtc);
            sph.DefineSqlParameter("@AdminOrderLink", SqlDbType.NVarChar, 255, ParameterDirection.Input, adminOrderLink);
            sph.DefineSqlParameter("@UserOrderLink", SqlDbType.NVarChar, 255, ParameterDirection.Input, userOrderLink);
            sph.DefineSqlParameter("@RowCreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, rowCreatedUtc);
            sph.DefineSqlParameter("@IncludeInAggregate", SqlDbType.Bit, ParameterDirection.Input, includeInAggregate);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }

        /// <summary>
        /// Deletes a row from the mp_CommerceReportOrders table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteOrder(Guid rowGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CommerceReportOrders_Delete", 1);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes  from the mp_CommerceReportOrders table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteOrdersBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CommerceReportOrders_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }


        /// <summary>
        /// Deletes from the mp_CommerceReportOrders table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteOrdersByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CommerceReportOrders_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes from the mp_CommerceReportOrders table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteOrdersByOrder(Guid orderGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CommerceReportOrders_DeleteByOrder", 1);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_CommerceReportOrders table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteOrdersByFeature(Guid featureGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CommerceReportOrders_DeleteByFeature", 1);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_CommerceReportOrders table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteOrdersByUser(Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CommerceReportOrders_DeleteByUser", 1);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool MoveOrder(
            Guid orderGuid,
            Guid newUserGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CommerceReport_MoveOrder", 2);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, newUserGuid);
            int rowsAffected = sph.ExecuteNonQuery();

            sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CommerceReportOrders_MoveOrder", 2);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, newUserGuid);
            sph.ExecuteNonQuery();

            return (rowsAffected > 0);

        }

    }
}
