// Author:					Joe Audette
// Created:					2010-04-01
// Last Modified:			2013-09-10
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
    public static class DBCommerceReport
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns></returns>
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }

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
            sqlCommand.Append("INSERT INTO mp_CommerceReport ");
            sqlCommand.Append("(");
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
            sqlCommand.Append("RowCreatedUtc ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@RowGuid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@FeatureGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@ModuleTitle, ");
            sqlCommand.Append("@OrderGuid, ");
            sqlCommand.Append("@ItemGuid, ");
            sqlCommand.Append("@ItemName, ");
            sqlCommand.Append("@Quantity, ");
            sqlCommand.Append("@Price, ");
            sqlCommand.Append("@SubTotal, ");
            sqlCommand.Append("@OrderDateUtc, ");
            sqlCommand.Append("@PaymentMethod, ");
            sqlCommand.Append("@IPAddress, ");
            sqlCommand.Append("@AdminOrderLink, ");
            sqlCommand.Append("@UserOrderLink, ");
            sqlCommand.Append("@IncludeInAggregate, ");
            sqlCommand.Append("@RowCreatedUtc ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[19];

            arParams[0] = new SqlCeParameter("@RowGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid;

            arParams[1] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid;

            arParams[2] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid;

            arParams[3] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = featureGuid;

            arParams[4] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = moduleGuid;

            arParams[5] = new SqlCeParameter("@ModuleTitle", SqlDbType.NVarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = moduleTitle;

            arParams[6] = new SqlCeParameter("@OrderGuid", SqlDbType.UniqueIdentifier);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = orderGuid;

            arParams[7] = new SqlCeParameter("@ItemGuid", SqlDbType.UniqueIdentifier);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = itemGuid;

            arParams[8] = new SqlCeParameter("@ItemName", SqlDbType.NVarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = itemName;

            arParams[9] = new SqlCeParameter("@Quantity", SqlDbType.Int);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = quantity;

            arParams[10] = new SqlCeParameter("@Price", SqlDbType.Decimal);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = price;

            arParams[11] = new SqlCeParameter("@SubTotal", SqlDbType.Decimal);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = subTotal;

            arParams[12] = new SqlCeParameter("@OrderDateUtc", SqlDbType.DateTime);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = orderDateUtc;

            arParams[13] = new SqlCeParameter("@PaymentMethod", SqlDbType.NVarChar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = paymentMethod;

            arParams[14] = new SqlCeParameter("@IPAddress", SqlDbType.NVarChar, 250);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = iPAddress;

            arParams[15] = new SqlCeParameter("@AdminOrderLink", SqlDbType.NVarChar, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = adminOrderLink;

            arParams[16] = new SqlCeParameter("@UserOrderLink", SqlDbType.NVarChar, 255);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = userOrderLink;

            arParams[17] = new SqlCeParameter("@RowCreatedUtc", SqlDbType.DateTime);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = rowCreatedUtc;

            arParams[18] = new SqlCeParameter("@IncludeInAggregate", SqlDbType.Bit);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = includeInAggregate;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

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
            sqlCommand.Append("FeatureGuid = @FeatureGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

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
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes rows from the mp_CommerceReport table. Returns true if row deleted.
        /// </summary>
        /// <param name="orderGuid"> orderGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByOrder(Guid orderGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CommerceReport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("OrderGuid = @OrderGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@OrderGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = orderGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetSalesByYearByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("TOP(13) ");
            sqlCommand.Append("DatePart(year, OrderDateUtc) As Y, ");
            sqlCommand.Append("SUM(SubTotal) As Sales, ");
            sqlCommand.Append("SUM(Quantity) As Units ");

            sqlCommand.Append("FROM	mp_CommerceReport ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");

            sqlCommand.Append("GROUP BY DatePart(year,OrderDateUtc) ");
            sqlCommand.Append("ORDER BY DatePart(year,OrderDateUtc) desc ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSalesByYearMonthBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("DatePart(year, OrderDateUtc) As Y, ");
            sqlCommand.Append("DatePart(month, OrderDateUtc) As M, ");
            sqlCommand.Append("SUM(SubTotal) As Sales, ");
            sqlCommand.Append("SUM(Quantity) As Units ");

            sqlCommand.Append("FROM	mp_CommerceReport ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");

            sqlCommand.Append("GROUP BY DatePart(year,OrderDateUtc), DatePart(month,OrderDateUtc) ");
            sqlCommand.Append("ORDER BY DatePart(year,OrderDateUtc) desc, DatePart(month,OrderDateUtc) desc ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSalesByYearMonthByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("TOP(13) ");
            sqlCommand.Append("DatePart(year, OrderDateUtc) As Y, ");
            sqlCommand.Append("DatePart(month, OrderDateUtc) As M, ");
            sqlCommand.Append("SUM(SubTotal) As Sales, ");
            sqlCommand.Append("SUM(Quantity) As Units ");

            sqlCommand.Append("FROM	mp_CommerceReport ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");

            sqlCommand.Append("GROUP BY DatePart(year,OrderDateUtc), DatePart(month,OrderDateUtc) ");
            sqlCommand.Append("ORDER BY DatePart(year,OrderDateUtc) desc, DatePart(month,OrderDateUtc) desc ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSalesByYearMonthByItem(Guid itemGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("TOP(13) ");
            sqlCommand.Append("DatePart(year, OrderDateUtc) As Y, ");
            sqlCommand.Append("DatePart(month, OrderDateUtc) As M, ");
            sqlCommand.Append("SUM(SubTotal) As Sales, ");
            sqlCommand.Append("SUM(Quantity) As Units ");

            sqlCommand.Append("FROM	mp_CommerceReport ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemGuid = @ItemGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");

            sqlCommand.Append("GROUP BY DatePart(year,OrderDateUtc), DatePart(month,OrderDateUtc) ");
            sqlCommand.Append("ORDER BY DatePart(year,OrderDateUtc) desc, DatePart(month,OrderDateUtc) desc ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSalesByYearMonthByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("TOP(13) ");
            sqlCommand.Append("DatePart(year, OrderDateUtc) As Y, ");
            sqlCommand.Append("DatePart(month, OrderDateUtc) As M, ");
            sqlCommand.Append("SUM(SubTotal) As Sales, ");
            sqlCommand.Append("SUM(Quantity) As Units ");

            sqlCommand.Append("FROM	mp_CommerceReport ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");

            sqlCommand.Append("GROUP BY DatePart(year,OrderDateUtc), DatePart(month,OrderDateUtc) ");
            sqlCommand.Append("ORDER BY DatePart(year,OrderDateUtc) desc, DatePart(month,OrderDateUtc) desc ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSalesGroupedByModule(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("ModuleTitle, ");
            sqlCommand.Append("SUM(SubTotal) As Sales, ");
            sqlCommand.Append("SUM(Quantity) As Units ");

            sqlCommand.Append("FROM	mp_CommerceReport ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");

            sqlCommand.Append("GROUP BY ModuleGuid,ModuleTitle ");
            // TODO: cannot use aggregate in order by, how to solve this?
            //sqlCommand.Append("ORDER BY SUM(SubTotal) desc ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSalesGroupedByUser(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP (20) ");
            sqlCommand.Append("c.UserGuid, ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("COALESCE(u.[Name], 'deleted user') AS Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("SUM(c.SubTotal) As Sales ");

            sqlCommand.Append("FROM	mp_CommerceReport c ");
            sqlCommand.Append("LEFT OUTER JOIN [mp_Users] u ");
            sqlCommand.Append("ON	c.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("c.SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND c.IncludeInAggregate = 1 ");

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("c.UserGuid, ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.[Name], ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            // TODO: cannot use aggregate in order by, how to solve this?
            //sqlCommand.Append("ORDER BY SUM(c.SubTotal) desc ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("SUM(SubTotal) As Revenue ");

            sqlCommand.Append("FROM	mp_CommerceReport  ");
            
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemGuid = @ItemGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("ModuleTitle, ");
            sqlCommand.Append("ItemName, ");
            sqlCommand.Append("ItemGuid ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("SUM(Quantity) As UnitsSold, ");
            sqlCommand.Append("SUM(SubTotal) As Revenue ");

            sqlCommand.Append("FROM	mp_CommerceReport ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");

            sqlCommand.Append("GROUP BY ModuleTitle, ItemName, ItemGuid ");
            sqlCommand.Append("ORDER BY ModuleTitle");
            // TODO: cannot use aggregate in order by, how to solve this?
            //sqlCommand.Append(", SUM(SubTotal) DESC ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetItemRevenueByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP(20)  ");
            sqlCommand.Append("ModuleTitle, ");
            sqlCommand.Append("ItemName, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("SUM(Quantity) As UnitsSold, ");
            sqlCommand.Append("SUM(SubTotal) As Revenue ");

            sqlCommand.Append("FROM	mp_CommerceReport ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");

            sqlCommand.Append("GROUP BY ModuleTitle, ItemName, ItemGuid ");
            // TODO: cannot use aggregate in order by, how to solve this?
            //sqlCommand.Append("ORDER BY SUM(SubTotal) DESC ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("SUM(Quantity) As UnitsSold, ");
            sqlCommand.Append("SUM(SubTotal) As Revenue ");

            sqlCommand.Append("FROM	mp_CommerceReport ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");

            sqlCommand.Append("GROUP BY ModuleTitle, ItemName, ItemGuid ");
            sqlCommand.Append("ORDER BY ModuleTitle");
            // TODO: cannot use aggregate in order by, how to solve this?
            //sqlCommand.Append(", SUM(SubTotal) DESC ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static decimal GetAllTimeRevenueBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            
            sqlCommand.Append("COALESCE(SUM(SubTotal),0)  ");

            sqlCommand.Append("FROM	mp_CommerceReport ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return Convert.ToDecimal(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public static decimal GetAllTimeRevenueByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");

            sqlCommand.Append("COALESCE(SUM(SubTotal),0)  ");

            sqlCommand.Append("FROM	mp_CommerceReport ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append("AND IncludeInAggregate = 1 ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            return Convert.ToDecimal(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public static int GetItemCountByUser(Guid siteGuid, Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");

            sqlCommand.Append("COUNT(*)  ");

            sqlCommand.Append("FROM	mp_CommerceReport ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("UserGuid = @UserGuid ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

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
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") * ");

            sqlCommand.Append("FROM	mp_CommerceReport  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("UserGuid = @UserGuid ");

            sqlCommand.Append("ORDER BY OrderDateUtc ");
            sqlCommand.Append("DESC  ");

            sqlCommand.Append(") AS t1 ");
            sqlCommand.Append("ORDER BY OrderDateUtc desc ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");

            //sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid;


            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetDistinctItemCountByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");

            sqlCommand.Append("COUNT(*)  ");

            sqlCommand.Append("FROM	(SELECT DISTINCT ItemGuid FROM [mp_CommerceReport] ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid)  ");
            
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

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
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ")  ");
            sqlCommand.Append("ModuleTitle, ");
            sqlCommand.Append("ItemName, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("SUM(Quantity) As UnitsSold, ");
            sqlCommand.Append("SUM(SubTotal) As Revenue ");

            sqlCommand.Append("FROM	mp_CommerceReport ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");

            sqlCommand.Append("GROUP BY ModuleTitle, ItemName, ItemGuid ");
            // TODO: cannot use aggregate in order by, how to solve this?
            //sqlCommand.Append("ORDER BY SUM(SubTotal) DESC ");

            sqlCommand.Append("FROM	mp_CommerceReport  ");
            //sqlCommand.Append("ORDER BY  ");
            //sqlCommand.Append("DESC  ");

            sqlCommand.Append(") AS t1 ");
            //sqlCommand.Append("ORDER BY  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetDistinctUserItemCount(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");

            sqlCommand.Append("COUNT(*)  ");

            sqlCommand.Append("FROM	(SELECT DISTINCT ItemGuid FROM [mp_CommerceReport] ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid) t ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public static IDataReader GetUserItemPageBySite(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            //TODO: this is not correct or finished

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
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") * ");

            sqlCommand.Append("FROM	mp_CommerceReport  ");
            //sqlCommand.Append("ORDER BY  ");
            //sqlCommand.Append("DESC  ");

            sqlCommand.Append(") AS t1 ");
            //sqlCommand.Append("ORDER BY  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("INSERT INTO mp_CommerceReportOrders ");
            sqlCommand.Append("(");
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
            sqlCommand.Append("RowCreatedUtc ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@RowGuid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@FeatureGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@OrderGuid, ");
            sqlCommand.Append("@BillingFirstName, ");
            sqlCommand.Append("@BillingLastName, ");
            sqlCommand.Append("@BillingCompany, ");
            sqlCommand.Append("@BillingAddress1, ");
            sqlCommand.Append("@BillingAddress2, ");
            sqlCommand.Append("@BillingSuburb, ");
            sqlCommand.Append("@BillingCity, ");
            sqlCommand.Append("@BillingPostalCode, ");
            sqlCommand.Append("@BillingState, ");
            sqlCommand.Append("@BillingCountry, ");
            sqlCommand.Append("@PaymentMethod, ");
            sqlCommand.Append("@SubTotal, ");
            sqlCommand.Append("@TaxTotal, ");
            sqlCommand.Append("@ShippingTotal, ");
            sqlCommand.Append("@OrderTotal, ");
            sqlCommand.Append("@OrderDateUtc, ");
            sqlCommand.Append("@AdminOrderLink, ");
            sqlCommand.Append("@UserOrderLink, ");
            sqlCommand.Append("@IncludeInAggregate, ");
            sqlCommand.Append("@RowCreatedUtc ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[26];

            arParams[0] = new SqlCeParameter("@RowGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid;

            arParams[1] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid;

            arParams[2] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = featureGuid;

            arParams[3] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moduleGuid;

            arParams[4] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userGuid;

            arParams[5] = new SqlCeParameter("@OrderGuid", SqlDbType.UniqueIdentifier);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = orderGuid;

            arParams[6] = new SqlCeParameter("@BillingFirstName", SqlDbType.NVarChar, 100);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = billingFirstName;

            arParams[7] = new SqlCeParameter("@BillingLastName", SqlDbType.NVarChar, 50);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = billingLastName;

            arParams[8] = new SqlCeParameter("@BillingCompany", SqlDbType.NVarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = billingCompany;

            arParams[9] = new SqlCeParameter("@BillingAddress1", SqlDbType.NVarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = billingAddress1;

            arParams[10] = new SqlCeParameter("@BillingAddress2", SqlDbType.NVarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = billingAddress2;

            arParams[11] = new SqlCeParameter("@BillingSuburb", SqlDbType.NVarChar, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = billingSuburb;

            arParams[12] = new SqlCeParameter("@BillingCity", SqlDbType.NVarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = billingCity;

            arParams[13] = new SqlCeParameter("@BillingPostalCode", SqlDbType.NVarChar, 20);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = billingPostalCode;

            arParams[14] = new SqlCeParameter("@BillingState", SqlDbType.NVarChar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = billingState;

            arParams[15] = new SqlCeParameter("@BillingCountry", SqlDbType.NVarChar, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = billingCountry;

            arParams[16] = new SqlCeParameter("@PaymentMethod", SqlDbType.NVarChar, 50);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = paymentMethod;

            arParams[17] = new SqlCeParameter("@SubTotal", SqlDbType.Decimal);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = subTotal;

            arParams[18] = new SqlCeParameter("@TaxTotal", SqlDbType.Decimal);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = taxTotal;

            arParams[19] = new SqlCeParameter("@ShippingTotal", SqlDbType.Decimal);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = shippingTotal;

            arParams[20] = new SqlCeParameter("@OrderTotal", SqlDbType.Decimal);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = orderTotal;

            arParams[21] = new SqlCeParameter("@OrderDateUtc", SqlDbType.DateTime);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = orderDateUtc;

            arParams[22] = new SqlCeParameter("@AdminOrderLink", SqlDbType.NVarChar, 255);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = adminOrderLink;

            arParams[23] = new SqlCeParameter("@UserOrderLink", SqlDbType.NVarChar, 255);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = userOrderLink;

            arParams[24] = new SqlCeParameter("@RowCreatedUtc", SqlDbType.DateTime);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = rowCreatedUtc;

            arParams[25] = new SqlCeParameter("@IncludeInAggregate", SqlDbType.Bit);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = includeInAggregate;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        public static bool DeleteOrder(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CommerceReportOrders ");
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

        public static bool DeleteOrdersBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CommerceReportOrders ");
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

        public static bool DeleteOrdersByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CommerceReportOrders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteOrdersByOrder(Guid orderGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CommerceReportOrders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("OrderGuid = @OrderGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@OrderGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = orderGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteOrdersByFeature(Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CommerceReportOrders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FeatureGuid = @FeatureGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteOrdersByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CommerceReportOrders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool MoveOrder(
            Guid orderGuid,
            Guid newUserGuid)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_CommerceReport ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("OrderGuid = @OrderGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = newUserGuid;

            arParams[1] = new SqlCeParameter("@OrderGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderGuid;

            
            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_CommerceReportOrders ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("OrderGuid = @OrderGuid ");
            sqlCommand.Append(";");

            arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = newUserGuid;

            arParams[1] = new SqlCeParameter("@OrderGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderGuid;


            SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

    }
}
