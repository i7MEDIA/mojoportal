/// Author:					
/// Created:				2009-01-31
/// Last Modified:			2013-09-10
/// 


using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using MySql.Data.MySqlClient;

namespace mojoPortal.Data
{

    public static class DBCommerceReport
    {
        
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_CommerceReport (");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("FeatureGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("ModuleTitle, ");
            sqlCommand.Append("OrderGuid, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("ItemName, ");
            sqlCommand.Append("Quantity, ");
            sqlCommand.Append("Price, ");
            sqlCommand.Append("SubTotal, ");
            sqlCommand.Append("OrderDateUtc, ");
            sqlCommand.Append("PaymentMethod, ");
            sqlCommand.Append("IPAddress, ");
            sqlCommand.Append("AdminOrderLink, ");
            sqlCommand.Append("UserOrderLink, ");
            sqlCommand.Append("IncludeInAggregate, ");
            sqlCommand.Append("RowCreatedUtc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?RowGuid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?UserGuid, ");
            sqlCommand.Append("?FeatureGuid, ");
            sqlCommand.Append("?ModuleGuid, ");
            sqlCommand.Append("?ModuleTitle, ");
            sqlCommand.Append("?OrderGuid, ");
            sqlCommand.Append("?ItemGuid, ");
            sqlCommand.Append("?ItemName, ");
            sqlCommand.Append("?Quantity, ");
            sqlCommand.Append("?Price, ");
            sqlCommand.Append("?SubTotal, ");
            sqlCommand.Append("?OrderDateUtc, ");
            sqlCommand.Append("?PaymentMethod, ");
            sqlCommand.Append("?IPAddress, ");
            sqlCommand.Append("?AdminOrderLink, ");
            sqlCommand.Append("?UserOrderLink, ");
            sqlCommand.Append("?IncludeInAggregate, ");
            sqlCommand.Append("?RowCreatedUtc )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[19];

            arParams[0] = new MySqlParameter("?RowGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid.ToString();

            arParams[3] = new MySqlParameter("?FeatureGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = featureGuid.ToString();

            arParams[4] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = moduleGuid.ToString();

            arParams[5] = new MySqlParameter("?ModuleTitle", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = moduleTitle;

            arParams[6] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = orderGuid.ToString();

            arParams[7] = new MySqlParameter("?ItemGuid", MySqlDbType.VarChar, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = itemGuid.ToString();

            arParams[8] = new MySqlParameter("?ItemName", MySqlDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = itemName;

            arParams[9] = new MySqlParameter("?Quantity", MySqlDbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = quantity;

            arParams[10] = new MySqlParameter("?Price", MySqlDbType.Decimal);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = price;

            arParams[11] = new MySqlParameter("?SubTotal", MySqlDbType.Decimal);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = subTotal;

            arParams[12] = new MySqlParameter("?OrderDateUtc", MySqlDbType.DateTime);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = orderDateUtc;

            arParams[13] = new MySqlParameter("?PaymentMethod", MySqlDbType.VarChar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = paymentMethod;

            arParams[14] = new MySqlParameter("?IPAddress", MySqlDbType.VarChar, 250);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = iPAddress;

            arParams[15] = new MySqlParameter("?AdminOrderLink", MySqlDbType.VarChar, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = adminOrderLink;

            arParams[16] = new MySqlParameter("?UserOrderLink", MySqlDbType.VarChar, 255);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = userOrderLink;

            arParams[17] = new MySqlParameter("?RowCreatedUtc", MySqlDbType.DateTime);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = rowCreatedUtc;

            arParams[18] = new MySqlParameter("?IncludeInAggregate", MySqlDbType.Int32);
            arParams[18].Direction = ParameterDirection.Input;
            if (includeInAggregate)
            {
                arParams[18].Value = 1;
            }
            else
            {
                arParams[18].Value = 0;
            }

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Deletes a row from the mp_CommerceReport table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CommerceReport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = ?RowGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?RowGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_CommerceReport table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CommerceReport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_CommerceReport table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CommerceReport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = ?UserGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_CommerceReport table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByFeature(Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CommerceReport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FeatureGuid = ?FeatureGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?FeatureGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_CommerceReport table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CommerceReport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_CommerceReport table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByOrder(Guid orderGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CommerceReport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("OrderGuid = ?OrderGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = orderGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        public static IDataReader GetSalesByYearByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("YEAR(OrderDateUtc) As Y,  ");
            sqlCommand.Append("SUM(SubTotal) As Sales, ");
            sqlCommand.Append("SUM(Quantity) As Units ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("mp_CommerceReport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");
            //sqlCommand.Append("AND ");
            //sqlCommand.Append("OrderGuid IN (SELECT OrderGuid FROM mp_CommerceReportOrders WHERE OrderTotal > 0) ");

            sqlCommand.Append("GROUP BY YEAR(OrderDateUtc) ");
            sqlCommand.Append("ORDER BY YEAR(OrderDateUtc) desc ");
            //sqlCommand.Append("LIMIT 13 ");
            sqlCommand.Append("; ");


            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSalesByYearMonthBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("YEAR(OrderDateUtc) As Y,  ");
            sqlCommand.Append("MONTH(OrderDateUtc) As M, ");
            sqlCommand.Append("SUM(SubTotal) As Sales, ");
            sqlCommand.Append("SUM(Quantity) As Units ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("mp_CommerceReport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");

            //sqlCommand.Append("AND ");
            //sqlCommand.Append("OrderGuid IN (SELECT OrderGuid FROM mp_CommerceReportOrders WHERE OrderTotal > 0) ");

            sqlCommand.Append("GROUP BY YEAR(OrderDateUtc), MONTH(OrderDateUtc) ");
            sqlCommand.Append("ORDER BY YEAR(OrderDateUtc) desc, MONTH(OrderDateUtc) desc ");
            
            sqlCommand.Append("; ");


            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSalesByYearMonthByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("YEAR(OrderDateUtc) As Y,  ");
            sqlCommand.Append("MONTH(OrderDateUtc) As M, ");
            sqlCommand.Append("SUM(SubTotal) As Sales, ");
            sqlCommand.Append("SUM(Quantity) As Units ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("mp_CommerceReport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");
            //sqlCommand.Append("AND ");
            //sqlCommand.Append("OrderGuid IN (SELECT OrderGuid FROM mp_CommerceReportOrders WHERE OrderTotal > 0) ");

            sqlCommand.Append("GROUP BY YEAR(OrderDateUtc), MONTH(OrderDateUtc) ");
            sqlCommand.Append("ORDER BY YEAR(OrderDateUtc) desc, MONTH(OrderDateUtc) desc ");
            //sqlCommand.Append("LIMIT 13 ");
            sqlCommand.Append("; ");


            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSalesByYearMonthByItem(Guid itemGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("YEAR(OrderDateUtc) As Y,  ");
            sqlCommand.Append("MONTH(OrderDateUtc) As M, ");
            sqlCommand.Append("SUM(SubTotal) As Sales, ");
            sqlCommand.Append("SUM(Quantity) As Units ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("mp_CommerceReport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemGuid = ?ItemGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");

            //sqlCommand.Append("AND ");
            //sqlCommand.Append("OrderGuid IN (SELECT OrderGuid FROM mp_CommerceReportOrders WHERE OrderTotal > 0) ");

            sqlCommand.Append("GROUP BY YEAR(OrderDateUtc), MONTH(OrderDateUtc) ");
            sqlCommand.Append("ORDER BY YEAR(OrderDateUtc) desc, MONTH(OrderDateUtc) desc ");
            //sqlCommand.Append("LIMIT 13 ");
            sqlCommand.Append("; ");


            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemGuid;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSalesByYearMonthByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("YEAR(OrderDateUtc) As Y,  ");
            sqlCommand.Append("MONTH(OrderDateUtc) As M, ");
            sqlCommand.Append("SUM(SubTotal) As Sales, ");
            sqlCommand.Append("SUM(Quantity) As Units ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("mp_CommerceReport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = ?UserGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");

            //sqlCommand.Append("AND ");
            //sqlCommand.Append("OrderGuid IN (SELECT OrderGuid FROM mp_CommerceReportOrders WHERE OrderTotal > 0) ");

            sqlCommand.Append("GROUP BY YEAR(OrderDateUtc), MONTH(OrderDateUtc) ");
            sqlCommand.Append("ORDER BY YEAR(OrderDateUtc) desc, MONTH(OrderDateUtc) desc ");
            //sqlCommand.Append("LIMIT 13 ");
            sqlCommand.Append("; ");


            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetSalesGroupedByModule(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("ModuleTitle, ");
            sqlCommand.Append("SUM(SubTotal) AS Sales, ");
            sqlCommand.Append("SUM(Quantity) As Units ");

            sqlCommand.Append("FROM	mp_CommerceReport ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");
            
            
            //sqlCommand.Append("AND ");
            //sqlCommand.Append("OrderGuid IN (SELECT OrderGuid FROM mp_CommerceReportOrders WHERE OrderTotal > 0) ");

            sqlCommand.Append("GROUP BY ModuleGuid, ModuleTitle ");

            sqlCommand.Append("ORDER BY SUM(SubTotal) DESC ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }
        public static IDataReader GetSalesGroupedByUser(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("c.UserGuid AS UserGuid, ");
            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("COALESCE(u.Name, 'deleted user') AS Name, ");
            sqlCommand.Append("u.LoginName AS LoginName, ");
            sqlCommand.Append("u.Email AS Email, ");
            sqlCommand.Append("SUM(SubTotal) AS Sales ");

            sqlCommand.Append("FROM	mp_CommerceReport c ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
            sqlCommand.Append("ON c.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("c.SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND c.IncludeInAggregate = 1 ");
            
            //sqlCommand.Append("AND ");
            //sqlCommand.Append("c.OrderGuid IN (SELECT OrderGuid FROM mp_CommerceReportOrders WHERE OrderTotal > 0) ");

            sqlCommand.Append("GROUP BY  ");
            sqlCommand.Append("c.UserGuid, ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("ORDER BY SUM(SubTotal) DESC ");
            sqlCommand.Append("LIMIT 20 ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetItemSummary(Guid itemGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("ModuleTitle, ");
            sqlCommand.Append("ItemName, ");
            sqlCommand.Append("SUM(SubTotal) AS Revenue ");

            sqlCommand.Append("FROM	mp_CommerceReport ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemGuid = ?ItemGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");
            
            
            //sqlCommand.Append("AND ");
            //sqlCommand.Append("OrderGuid IN (SELECT OrderGuid FROM mp_CommerceReportOrders WHERE OrderTotal > 0) ");

            sqlCommand.Append("GROUP BY SiteGuid, ModuleGuid, ItemGuid, ModuleTitle, ItemName ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemGuid;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetItemRevenueBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ModuleTitle, ");
            sqlCommand.Append("ItemName, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("SUM(Quantity) AS UnitsSold, ");
            sqlCommand.Append("SUM(SubTotal) AS Revenue ");
            sqlCommand.Append("FROM	mp_CommerceReport ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");

            //sqlCommand.Append("AND ");
            //sqlCommand.Append("OrderGuid IN (SELECT OrderGuid FROM mp_CommerceReportOrders WHERE OrderTotal > 0) ");
            
            
            sqlCommand.Append("GROUP BY ModuleTitle, ItemName, ItemGuid ");
            sqlCommand.Append("ORDER BY ModuleTitle, SUM(SubTotal) DESC ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetItemRevenueByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ModuleTitle, ");
            sqlCommand.Append("ItemName, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("SUM(Quantity) AS UnitsSold, ");
            sqlCommand.Append("SUM(SubTotal) AS Revenue ");
            sqlCommand.Append("FROM	mp_CommerceReport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");
            
            //sqlCommand.Append("AND ");
            //sqlCommand.Append("OrderGuid IN (SELECT OrderGuid FROM mp_CommerceReportOrders WHERE OrderTotal > 0) ");
            
            
            sqlCommand.Append("GROUP BY ModuleTitle, ItemName, ItemGuid ");
            sqlCommand.Append("ORDER BY SUM(SubTotal) DESC ");
            sqlCommand.Append("LIMIT 20 ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetItemRevenueByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ModuleTitle, ");
            sqlCommand.Append("ItemName, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("SUM(Quantity) AS UnitsSold, ");
            sqlCommand.Append("SUM(SubTotal) AS Revenue ");
            sqlCommand.Append("FROM	mp_CommerceReport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = ?UserGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");
            
            //sqlCommand.Append("AND ");
            //sqlCommand.Append("OrderGuid IN (SELECT OrderGuid FROM mp_CommerceReportOrders WHERE OrderTotal > 0) ");
            
            
            sqlCommand.Append("GROUP BY ModuleTitle, ItemName, ItemGuid ");
            sqlCommand.Append("ORDER BY ModuleTitle, SUM(SubTotal) DESC ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static decimal GetAllTimeRevenueBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("SUM(SubTotal) ");

            sqlCommand.Append("FROM	mp_CommerceReport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");

            //sqlCommand.Append("AND ");
            //sqlCommand.Append("OrderGuid IN (SELECT OrderGuid FROM mp_CommerceReportOrders WHERE OrderTotal > 0) ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            decimal result = 0;

            try
            {

                result = Convert.ToDecimal(MySqlHelper.ExecuteScalar(
                    ConnectionString.GetReadConnectionString(),
                    sqlCommand.ToString(),
                    arParams), CultureInfo.InvariantCulture);
            }
            catch (InvalidCastException) { }

            return result;

        }

        public static decimal GetAllTimeRevenueByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("SUM(SubTotal) ");

            sqlCommand.Append("FROM	mp_CommerceReport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");
            
            //sqlCommand.Append("AND ");
            //sqlCommand.Append("OrderGuid IN (SELECT OrderGuid FROM mp_CommerceReportOrders WHERE OrderTotal > 0) ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            decimal result = 0;

            try
            {

                result = Convert.ToDecimal(MySqlHelper.ExecuteScalar(
                    ConnectionString.GetReadConnectionString(),
                    sqlCommand.ToString(),
                    arParams), CultureInfo.InvariantCulture);
            }
            catch (InvalidCastException) { }

            return result;

        }

        /// <summary>
        /// Gets a count of rows in the mp_CommerceReport table.
        /// </summary>
        public static int GetDistinctItemCountByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	 ");
            sqlCommand.Append(" (SELECT DISTINCT ItemGuid FROM mp_CommerceReport WHERE ModuleGuid = ?ModuleGuid) AS s ");
            sqlCommand.Append(";");
           
            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

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
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetDistinctItemCountByModule(moduleGuid);

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
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("ModuleTitle, ");
            sqlCommand.Append("ItemName, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("SUM(Quantity) AS UnitsSold, ");
            sqlCommand.Append("SUM(SubTotal) AS Revenue ");

            sqlCommand.Append("FROM	mp_CommerceReport  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");

            sqlCommand.Append("GROUP BY ModuleTitle, ItemName, ItemGuid   ");
            sqlCommand.Append("ORDER BY ModuleTitle, ItemName   ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the mp_CommerceReport table.
        /// </summary>
        public static int GetItemCountByUser(Guid siteGuid, Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	 ");
            sqlCommand.Append("mp_CommerceReport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND UserGuid = ?UserGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

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
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetItemCountByUser(siteGuid, userGuid);

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
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_CommerceReport  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND UserGuid = ?UserGuid ");
            sqlCommand.Append("ORDER BY OrderDateUtc DESC   ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT ?PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid;

            arParams[2] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        /// <summary>
        /// Gets a count of rows in the mp_CommerceReport table.
        /// </summary>
        public static int GetDistinctUserItemCount(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	 ");
            sqlCommand.Append("(SELECT DISTINCT UserGuid FROM mp_CommerceReport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid) a ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
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
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetDistinctUserItemCount(siteGuid);

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
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("c.UserGuid AS UserGuid, ");
            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("COALESCE(u.Name, 'deleted user') AS Name, ");
            sqlCommand.Append("u.LoginName AS LoginName, ");
            sqlCommand.Append("u.Email AS Email, ");
            sqlCommand.Append("SUM(c.SubTotal) AS Revenue ");

            sqlCommand.Append("FROM	mp_CommerceReport c  ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
            sqlCommand.Append("ON	c.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("c.SiteGuid = ?SiteGuid ");

            sqlCommand.Append("GROUP BY   ");
            sqlCommand.Append("c.UserGuid, ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("ORDER BY u.Name   ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_CommerceReportOrders (");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("FeatureGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("OrderGuid, ");
            sqlCommand.Append("BillingFirstName, ");
            sqlCommand.Append("BillingLastName, ");
            sqlCommand.Append("BillingCompany, ");
            sqlCommand.Append("BillingAddress1, ");
            sqlCommand.Append("BillingAddress2, ");
            sqlCommand.Append("BillingSuburb, ");
            sqlCommand.Append("BillingCity, ");
            sqlCommand.Append("BillingPostalCode, ");
            sqlCommand.Append("BillingState, ");
            sqlCommand.Append("BillingCountry, ");
            sqlCommand.Append("PaymentMethod, ");
            sqlCommand.Append("SubTotal, ");
            sqlCommand.Append("TaxTotal, ");
            sqlCommand.Append("ShippingTotal, ");
            sqlCommand.Append("OrderTotal, ");
            sqlCommand.Append("OrderDateUtc, ");
            sqlCommand.Append("AdminOrderLink, ");
            sqlCommand.Append("UserOrderLink, ");
            sqlCommand.Append("IncludeInAggregate, ");
            sqlCommand.Append("RowCreatedUtc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?RowGuid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?FeatureGuid, ");
            sqlCommand.Append("?ModuleGuid, ");
            sqlCommand.Append("?UserGuid, ");
            sqlCommand.Append("?OrderGuid, ");
            sqlCommand.Append("?BillingFirstName, ");
            sqlCommand.Append("?BillingLastName, ");
            sqlCommand.Append("?BillingCompany, ");
            sqlCommand.Append("?BillingAddress1, ");
            sqlCommand.Append("?BillingAddress2, ");
            sqlCommand.Append("?BillingSuburb, ");
            sqlCommand.Append("?BillingCity, ");
            sqlCommand.Append("?BillingPostalCode, ");
            sqlCommand.Append("?BillingState, ");
            sqlCommand.Append("?BillingCountry, ");
            sqlCommand.Append("?PaymentMethod, ");
            sqlCommand.Append("?SubTotal, ");
            sqlCommand.Append("?TaxTotal, ");
            sqlCommand.Append("?ShippingTotal, ");
            sqlCommand.Append("?OrderTotal, ");
            sqlCommand.Append("?OrderDateUtc, ");
            sqlCommand.Append("?AdminOrderLink, ");
            sqlCommand.Append("?UserOrderLink, ");
            sqlCommand.Append("?IncludeInAggregate, ");
            sqlCommand.Append("?RowCreatedUtc )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[26];

            arParams[0] = new MySqlParameter("?RowGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?FeatureGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = featureGuid.ToString();

            arParams[3] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moduleGuid.ToString();

            arParams[4] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userGuid.ToString();

            arParams[5] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = orderGuid.ToString();

            arParams[6] = new MySqlParameter("?BillingFirstName", MySqlDbType.VarChar, 100);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = billingFirstName;

            arParams[7] = new MySqlParameter("?BillingLastName", MySqlDbType.VarChar, 50);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = billingLastName;

            arParams[8] = new MySqlParameter("?BillingCompany", MySqlDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = billingCompany;

            arParams[9] = new MySqlParameter("?BillingAddress1", MySqlDbType.VarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = billingAddress1;

            arParams[10] = new MySqlParameter("?BillingAddress2", MySqlDbType.VarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = billingAddress2;

            arParams[11] = new MySqlParameter("?BillingSuburb", MySqlDbType.VarChar, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = billingSuburb;

            arParams[12] = new MySqlParameter("?BillingCity", MySqlDbType.VarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = billingCity;

            arParams[13] = new MySqlParameter("?BillingPostalCode", MySqlDbType.VarChar, 20);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = billingPostalCode;

            arParams[14] = new MySqlParameter("?BillingState", MySqlDbType.VarChar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = billingState;

            arParams[15] = new MySqlParameter("?BillingCountry", MySqlDbType.VarChar, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = billingCountry;

            arParams[16] = new MySqlParameter("?PaymentMethod", MySqlDbType.VarChar, 50);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = paymentMethod;

            arParams[17] = new MySqlParameter("?SubTotal", MySqlDbType.Decimal);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = subTotal;

            arParams[18] = new MySqlParameter("?TaxTotal", MySqlDbType.Decimal);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = taxTotal;

            arParams[19] = new MySqlParameter("?ShippingTotal", MySqlDbType.Decimal);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = shippingTotal;

            arParams[20] = new MySqlParameter("?OrderTotal", MySqlDbType.Decimal);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = orderTotal;

            arParams[21] = new MySqlParameter("?OrderDateUtc", MySqlDbType.DateTime);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = orderDateUtc;

            arParams[22] = new MySqlParameter("?AdminOrderLink", MySqlDbType.VarChar, 255);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = adminOrderLink;

            arParams[23] = new MySqlParameter("?UserOrderLink", MySqlDbType.VarChar, 255);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = userOrderLink;

            arParams[24] = new MySqlParameter("?RowCreatedUtc", MySqlDbType.DateTime);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = rowCreatedUtc;

            arParams[25] = new MySqlParameter("?IncludeInAggregate", MySqlDbType.Int32);
            arParams[25].Direction = ParameterDirection.Input;
            if (includeInAggregate)
            {
                arParams[25].Value = 1;
            }
            else
            {
                arParams[25].Value = 0;
            }

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

        }

        /// <summary>
        /// Deletes a row from the mp_CommerceReportOrders table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteOrder(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CommerceReportOrders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = ?RowGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?RowGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes  from the mp_CommerceReportOrders table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteOrdersBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CommerceReportOrders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes from the mp_CommerceReportOrders table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteOrdersByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CommerceReportOrders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes from the mp_CommerceReportOrders table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteOrdersByOrder(Guid orderGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CommerceReportOrders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("OrderGuid = ?OrderGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = orderGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_CommerceReportOrders table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteOrdersByFeature(Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CommerceReportOrders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FeatureGuid = ?FeatureGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?FeatureGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_CommerceReportOrders table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteOrdersByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CommerceReportOrders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = ?UserGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        
        public static bool MoveOrder(
            Guid orderGuid,
            Guid newUserGuid)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_CommerceReport ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("UserGuid = ?UserGuid ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("OrderGuid = ?OrderGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = newUserGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_CommerceReportOrders ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("UserGuid = ?UserGuid ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("OrderGuid = ?OrderGuid ");
            sqlCommand.Append(";");

            arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = newUserGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderGuid.ToString();

            MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

    }
}
