// Author:					
// Created:					2009-01-30
// Last Modified:			2012-01-21
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using mojoPortal.Data;

namespace mojoPortal.Business
{

    /// <summary>
    /// A common repository where ecommerce features can push in their sales results.
    /// This makes it possible to create one set of reports instead of having to create them for each feature.
    /// It also allows aggregating at the site level across commerce features.
    /// </summary>
    public static class CommerceReport
    {
        public static void AddRow(
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
            string userOrderLink)
        {
             AddRow(
                rowGuid,
                siteGuid,
                userGuid,
                featureGuid,
                moduleGuid,
                moduleTitle,
                orderGuid,
                itemGuid,
                itemName,
                quantity,
                price,
                subTotal,
                orderDateUtc,
                paymentMethod,
                iPAddress,
                adminOrderLink,
                userOrderLink,
                true);
        }


        /// <summary>
        /// Inserts a row in the mp_CommerceReport table. Returns rows affected count.
        /// </summary>
        public static void AddRow(
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
            bool includeInAggregate)
        {
            DBCommerceReport.Create(
                rowGuid,
                siteGuid,
                userGuid,
                featureGuid,
                moduleGuid,
                moduleTitle,
                orderGuid,
                itemGuid,
                itemName,
                quantity,
                price,
                subTotal,
                orderDateUtc,
                paymentMethod,
                iPAddress,
                adminOrderLink,
                userOrderLink,
                DateTime.UtcNow,
                includeInAggregate);

        }
        

       
        /// <summary>
        /// Deletes an instance of CommerceReport. Returns true on success.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            return DBCommerceReport.Delete(rowGuid);
        }

        /// <summary>
        /// Deletes rows from the mp_CommerceReport table. Returns true if row deleted.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            return DBCommerceReport.DeleteBySite(siteGuid);
        }

        private static DataTable GetYearEmptyTable()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Y", typeof(int));
            dataTable.Columns.Add("Sales", typeof(decimal));
            dataTable.Columns.Add("Units", typeof(int));
            return dataTable;
        }

        private static void PopulateYearTableFromReader(IDataReader reader, DataTable dataTable)
        {
            try
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();

                    row["Y"] = Convert.ToInt32(reader["Y"], CultureInfo.InvariantCulture);
                    row["Sales"] = Convert.ToDecimal(reader["Sales"], CultureInfo.InvariantCulture);
                    row["Units"] = Convert.ToInt32(reader["Units"], CultureInfo.InvariantCulture);
                    dataTable.Rows.Add(row);
                }
            }
            finally
            {
                reader.Close();
            }
        }

        private static DataTable GetYearMonthEmptyTable()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Y", typeof(int));
            dataTable.Columns.Add("M", typeof(int)); ;
            dataTable.Columns.Add("Sales", typeof(decimal));
            dataTable.Columns.Add("Units", typeof(int));
            return dataTable;
        }

        private static void PopulateYearMonthTableFromReader(IDataReader reader, DataTable dataTable)
        {
            try
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();

                    row["Y"] = Convert.ToInt32(reader["Y"], CultureInfo.InvariantCulture);
                    row["M"] = Convert.ToInt32(reader["M"], CultureInfo.InvariantCulture);
                    row["Sales"] = Convert.ToDecimal(reader["Sales"], CultureInfo.InvariantCulture);
                    row["Units"] = Convert.ToInt32(reader["Units"], CultureInfo.InvariantCulture);
                   
                    dataTable.Rows.Add(row);
                }
            }
            finally
            {
                reader.Close();
            }
        }

        public static DataTable GetSalesByYearMonthBySite(Guid siteGuid)
        {
            DataTable dataTable = GetYearMonthEmptyTable();
            IDataReader reader = DBCommerceReport.GetSalesByYearMonthBySite(siteGuid);
            PopulateYearMonthTableFromReader(reader, dataTable);
            return dataTable;
        }

        public static DataTable GetSalesByYearByModule(Guid moduleGuid)
        {
            DataTable dataTable = GetYearEmptyTable();
            IDataReader reader = DBCommerceReport.GetSalesByYearByModule(moduleGuid);
            PopulateYearTableFromReader(reader, dataTable);
            return dataTable;
        }

        public static DataTable GetSalesByYearMonthByModule(Guid moduleGuid)
        {
            DataTable dataTable = GetYearMonthEmptyTable();
            IDataReader reader = DBCommerceReport.GetSalesByYearMonthByModule(moduleGuid);
            PopulateYearMonthTableFromReader(reader, dataTable);
            return dataTable;
        }

        public static DataTable GetSalesByYearMonthByItem(Guid itemGuid)
        {
            DataTable dataTable = GetYearMonthEmptyTable();
            IDataReader reader = DBCommerceReport.GetSalesByYearMonthByItem(itemGuid);
            PopulateYearMonthTableFromReader(reader, dataTable);
            return dataTable;
        }

        public static DataTable GetSalesByYearMonthByUser(Guid userGuid)
        {
            DataTable dataTable = GetYearMonthEmptyTable();
            IDataReader reader = DBCommerceReport.GetSalesByYearMonthByUser(userGuid);
            PopulateYearMonthTableFromReader(reader, dataTable);
            return dataTable;
        }

        private static DataTable GetItemRevenueEmptyTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("ItemName", typeof(string));
            dataTable.Columns.Add("ItemGuid", typeof(Guid)); ;
            dataTable.Columns.Add("UnitsSold", typeof(int));
            dataTable.Columns.Add("Revenue", typeof(decimal));
            return dataTable;
        }

        private static void PopulateItemRevenueTableFromReader(IDataReader reader, DataTable dataTable)
        {
            try
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["ModuleTitle"] = reader["ModuleTitle"].ToString();
                    row["ItemName"] = reader["ItemName"].ToString();
                    row["ItemGuid"] = new Guid(reader["ItemGuid"].ToString());
                    row["UnitsSold"] = Convert.ToInt32(reader["UnitsSold"], CultureInfo.InvariantCulture);
                    row["Revenue"] = Convert.ToDecimal(reader["Revenue"], CultureInfo.InvariantCulture);

                    dataTable.Rows.Add(row);
                }
            }
            finally
            {
                reader.Close();
            }
        }

        public static DataTable GetItemRevenueBySite(Guid siteGuid)
        {
            DataTable dataTable = GetItemRevenueEmptyTable();
            using (IDataReader reader = DBCommerceReport.GetItemRevenueBySite(siteGuid))
            {
                PopulateItemRevenueTableFromReader(reader, dataTable);
            }
            return dataTable;
        }

        public static DataTable GetItemRevenueByModule(Guid moduleGuid)
        {
            DataTable dataTable = GetItemRevenueEmptyTable();
            using (IDataReader reader = DBCommerceReport.GetItemRevenueByModule(moduleGuid))
            {
                PopulateItemRevenueTableFromReader(reader, dataTable);
            }
            return dataTable;
        }

        public static decimal GetAllTimeRevenueBySite(Guid siteGuid)
        {
            return DBCommerceReport.GetAllTimeRevenueBySite(siteGuid);
        }

        public static decimal GetAllTimeRevenueByModule(Guid moduleGuid)
        {
            return DBCommerceReport.GetAllTimeRevenueByModule(moduleGuid);
        }

        public static IDataReader GetSalesGroupedByModule(Guid siteGuid)
        {
            return DBCommerceReport.GetSalesGroupedByModule(siteGuid);
        }

        public static IDataReader GetSalesGroupedByUser(Guid siteGuid)
        {
            return DBCommerceReport.GetSalesGroupedByUser(siteGuid);
        }

        /// <summary>
        /// Gets a page of data from the mp_CommerceReport table.
        /// </summary>
        public static IDataReader GetUserItemsPage(
            Guid siteGuid,
            Guid userGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBCommerceReport.GetUserItemsPage(siteGuid, userGuid, pageNumber, pageSize, out totalPages);
        }

        
        /// <summary>
        /// Gets a page of data from the mp_CommerceReport table.
        /// </summary>
        public static IDataReader GetItemsPageByModule(
            Guid moduleGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBCommerceReport.GetItemsPageByModule(moduleGuid, pageNumber, pageSize, out totalPages);

        }

        /// <summary>
        /// Gets a page of data from the mp_CommerceReport table.
        /// </summary>
        public static IDataReader GetUserItemPageBySite(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBCommerceReport.GetUserItemPageBySite(siteGuid, pageNumber, pageSize, out totalPages);
        }


        /// <summary>
        /// Deletes a row from the mp_CommerceReport table. Returns true if row deleted.
        /// </summary>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            DBCommerceReport.DeleteOrdersByModule(moduleGuid);
            return DBCommerceReport.DeleteByModule(moduleGuid);
        }

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
            DateTime rowCreatedUtc)
        {
            return CreateOrder(
                rowGuid,
                siteGuid,
                featureGuid,
                moduleGuid,
                userGuid,
                orderGuid,
                billingFirstName,
                billingLastName,
                billingCompany,
                billingAddress1,
                billingAddress2,
                billingSuburb,
                billingCity,
                billingPostalCode,
                billingState,
                billingCountry,
                paymentMethod,
                subTotal,
                taxTotal,
                shippingTotal,
                orderTotal,
                orderDateUtc,
                adminOrderLink,
                userOrderLink,
                rowCreatedUtc,
                true);
        }
        

        /// <summary>
        /// Inserts a row in the mp_CommerceReportOrders table. Returns rows affected count.
        /// </summary>
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
            return DBCommerceReport.CreateOrder(
                rowGuid,
                siteGuid,
                featureGuid,
                moduleGuid,
                userGuid,
                orderGuid,
                billingFirstName,
                billingLastName,
                billingCompany,
                billingAddress1,
                billingAddress2,
                billingSuburb,
                billingCity,
                billingPostalCode,
                billingState,
                billingCountry,
                paymentMethod,
                subTotal,
                taxTotal,
                shippingTotal,
                orderTotal,
                orderDateUtc,
                adminOrderLink,
                userOrderLink,
                rowCreatedUtc,
                includeInAggregate);

        }

        /// <summary>
        /// Deletes a row from the mp_CommerceReportOrders table. Returns true if row deleted.
        /// </summary>
        public static bool DeleteOrder(Guid orderGuid)
        {
            DBCommerceReport.DeleteOrdersByOrder(orderGuid);
            return DBCommerceReport.DeleteByOrder(orderGuid);
        }

        public static bool MoveOrder(
            Guid orderGuid,
            Guid newUserGuid)
        {
            return DBCommerceReport.MoveOrder(orderGuid, newUserGuid);
        }

        ///// <summary>
        ///// Deletes  from the mp_CommerceReportOrders table. Returns true if row deleted.
        ///// </summary>
        ///// <param name="rowGuid"> rowGuid </param>
        ///// <returns>bool</returns>
        //public static bool DeleteOrdersBySite(Guid siteGuid)
        //{
        //    return DBCommerceReport.DeleteOrdersBySite(siteGuid);
        //}

        ///// <summary>
        ///// Deletes from the mp_CommerceReportOrders table. Returns true if row deleted.
        ///// </summary>
        ///// <param name="rowGuid"> rowGuid </param>
        ///// <returns>bool</returns>
        //public static bool DeleteOrdersByModule(Guid moduleGuid)
        //{
        //    return DBCommerceReport.DeleteOrdersByModule(moduleGuid);
        //}

        ///// <summary>
        ///// Deletes a row from the mp_CommerceReportOrders table. Returns true if row deleted.
        ///// </summary>
        ///// <param name="rowGuid"> rowGuid </param>
        ///// <returns>bool</returns>
        //public static bool DeleteOrdersByFeature(Guid featureGuid)
        //{
        //    return DBCommerceReport.DeleteOrdersByFeature(featureGuid);
        //}

        ///// <summary>
        ///// Deletes a row from the mp_CommerceReportOrders table. Returns true if row deleted.
        ///// </summary>
        ///// <param name="rowGuid"> rowGuid </param>
        ///// <returns>bool</returns>
        //public static bool DeleteOrdersByUser(Guid userGuid)
        //{
        //    return DBCommerceReport.DeleteOrdersByUser(userGuid);
        //}


    }

}
