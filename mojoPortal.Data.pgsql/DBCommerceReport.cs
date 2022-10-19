/// Author:					
/// Created:				2009-01-31
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
using System.Text;
using System.Data;
using System.Configuration;
using System.Globalization;
using Npgsql;

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
            sqlCommand.Append("INSERT INTO mp_commercereport (");
            sqlCommand.Append("rowguid, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("featureguid, ");
            sqlCommand.Append("moduleguid, ");
            sqlCommand.Append("moduletitle, ");
            sqlCommand.Append("orderguid, ");
            sqlCommand.Append("itemguid, ");
            sqlCommand.Append("itemname, ");
            sqlCommand.Append("quantity, ");
            sqlCommand.Append("price, ");
            sqlCommand.Append("subtotal, ");
            sqlCommand.Append("orderdateutc, ");
            sqlCommand.Append("paymentmethod, ");
            sqlCommand.Append("ipaddress, ");
            sqlCommand.Append("adminorderlink, ");
            sqlCommand.Append("userorderlink, ");
            sqlCommand.Append("includeinaggregate, ");
            sqlCommand.Append("rowcreatedutc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":rowguid, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":userguid, ");
            sqlCommand.Append(":featureguid, ");
            sqlCommand.Append(":moduleguid, ");
            sqlCommand.Append(":moduletitle, ");
            sqlCommand.Append(":orderguid, ");
            sqlCommand.Append(":itemguid, ");
            sqlCommand.Append(":itemname, ");
            sqlCommand.Append(":quantity, ");
            sqlCommand.Append(":price, ");
            sqlCommand.Append(":subtotal, ");
            sqlCommand.Append(":orderdateutc, ");
            sqlCommand.Append(":paymentmethod, ");
            sqlCommand.Append(":ipaddress, ");
            sqlCommand.Append(":adminorderlink, ");
            sqlCommand.Append(":userorderlink, ");
            sqlCommand.Append(":includeinaggregate, ");
            sqlCommand.Append(":rowcreatedutc ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[19];

            arParams[0] = new NpgsqlParameter(":rowguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid.ToString();

            arParams[3] = new NpgsqlParameter(":featureguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = featureGuid.ToString();

            arParams[4] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = moduleGuid.ToString();

            arParams[5] = new NpgsqlParameter(":moduletitle", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = moduleTitle;

            arParams[6] = new NpgsqlParameter(":orderguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = orderGuid.ToString();

            arParams[7] = new NpgsqlParameter(":itemguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = itemGuid.ToString();

            arParams[8] = new NpgsqlParameter(":itemname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = itemName;

            arParams[9] = new NpgsqlParameter(":quantity", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = quantity;

            arParams[10] = new NpgsqlParameter(":price", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = price;

            arParams[11] = new NpgsqlParameter(":subtotal", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = subTotal;

            arParams[12] = new NpgsqlParameter(":orderdateutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = orderDateUtc;

            arParams[13] = new NpgsqlParameter(":paymentmethod", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = paymentMethod;

            arParams[14] = new NpgsqlParameter(":ipaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 250);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = iPAddress;

            arParams[15] = new NpgsqlParameter(":adminorderlink", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = adminOrderLink;

            arParams[16] = new NpgsqlParameter(":userorderlink", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = userOrderLink;

            arParams[17] = new NpgsqlParameter(":rowcreatedutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = rowCreatedUtc;

            arParams[18] = new NpgsqlParameter(":includeinaggregate", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = includeInAggregate;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("DELETE FROM mp_commercereport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("rowguid = :rowguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":rowguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("DELETE FROM mp_commercereport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid = :siteguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("DELETE FROM mp_commercereport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userguid = :userguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("DELETE FROM mp_commercereport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("featureguid = :featureguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":featureguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("DELETE FROM mp_commercereport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleguid = :moduleguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
        public static bool DeleteByOrder(Guid orderGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_commercereport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("orderguid = :orderguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":orderguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = orderGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetSalesByYearByModule(Guid moduleGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_commercereport_salesbyyearbymodule(:moduleguid)",
                arParams);

        }

        public static IDataReader GetSalesByYearBySite(Guid siteGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_commercereport_salesbyyearbysite(:siteguid)",
                arParams);

        }

        public static IDataReader GetSalesByYearMonthBySite(Guid siteGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_commercereport_salesbyyearmonthbysite(:siteguid)",
                arParams);

        }

        public static IDataReader GetSalesByYearMonthByModule(Guid moduleGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_commercereport_salesbyyearmonthbymodule(:moduleguid)",
                arParams);
        }

        public static IDataReader GetSalesByYearMonthByItem(Guid itemGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":itemguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_commercereport_salesbyyearmonthbyitem(:itemguid)",
                arParams);
        }

        public static IDataReader GetSalesByYearMonthByUser(Guid userGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_commercereport_salesbyyearmonthbyuser(:userguid)",
                arParams);
        }

        public static IDataReader GetSalesGroupedByModule(Guid siteGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_commercereport_getsalesgroupedbymodule(:siteguid)",
                arParams);

        }

        public static IDataReader GetSalesGroupedByUser(Guid siteGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_commercereport_getsalesgroupedbyuser(:siteguid)",
                arParams);

        }

        public static IDataReader GetItemSummary(Guid itemGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":itemguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_commercereport_getitemsummary(:itemguid)",
                arParams);
        }

        public static IDataReader GetItemRevenueBySite(Guid siteGuid)
        {
            // TODO: implement proc

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("moduletitle, ");
            sqlCommand.Append("itemname, ");
            sqlCommand.Append("itemguid, ");
            sqlCommand.Append("SUM(quantity) AS unitssold, ");
            sqlCommand.Append("SUM(subtotal) AS revenue ");
            sqlCommand.Append("FROM	mp_commercereport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid = :siteguid ");

            sqlCommand.Append("AND includeinaggregate = true ");

            sqlCommand.Append("GROUP BY moduletitle, itemname, itemguid ");
            sqlCommand.Append("ORDER BY moduletitle, SUM(subtotal) DESC ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetItemRevenueByModule(Guid moduleGuid)
        {
            // TODO: implement proc

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("moduletitle, ");
            sqlCommand.Append("itemname, ");
            sqlCommand.Append("itemguid, ");
            sqlCommand.Append("SUM(quantity) AS unitssold, ");
            sqlCommand.Append("SUM(subtotal) AS revenue ");
            sqlCommand.Append("FROM	mp_commercereport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleguid = :moduleguid ");
            sqlCommand.Append("AND includeinaggregate = true ");

            sqlCommand.Append("GROUP BY moduletitle, itemname, itemguid ");
            sqlCommand.Append("ORDER BY SUM(subtotal) DESC ");
            sqlCommand.Append("LIMIT 20 ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetItemRevenueByUser(Guid userGuid)
        {
            // TODO: implement proc

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("moduletitle, ");
            sqlCommand.Append("itemname, ");
            sqlCommand.Append("itemguid, ");
            sqlCommand.Append("SUM(quantity) AS unitssold, ");
            sqlCommand.Append("SUM(subtotal) AS revenue ");
            sqlCommand.Append("FROM	mp_commercereport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userguid = :userguid ");
            sqlCommand.Append("AND includeinaggregate = true ");

            sqlCommand.Append("GROUP BY moduletitle, itemname, itemguid ");
            sqlCommand.Append("ORDER BY moduletitle, SUM(subtotal) DESC ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static decimal GetAllTimeRevenueBySite(Guid siteGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            decimal result = 0;

            try
            {
                result = Convert.ToDecimal(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_commercereport_alltimerevenuebysite(:siteguid)",
                arParams), CultureInfo.InvariantCulture);
            }
            catch (InvalidCastException) { }

            return result;

        }

        public static decimal GetAllTimeRevenueByModule(Guid moduleGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            decimal result = 0;

            try
            {
                result = Convert.ToDecimal(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_commercereport_alltimerevenuebymodule(:moduleguid)",
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
            sqlCommand.Append("SELECT Count(s.*) ");
            sqlCommand.Append("FROM ");
            sqlCommand.Append("(SELECT DISTINCT itemguid FROM mp_commercereport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleguid = :moduleguid ");
            sqlCommand.Append(") s ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
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

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new NpgsqlParameter(":pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            arParams[2] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_commercereport_selectitempagebymodule(:moduleguid,:pagenumber,:pagesize)",
                arParams);

        }


        /// <summary>
        /// Gets a count of rows in the mp_CommerceReport table.
        /// </summary>
        public static int GetDistinctUserItemCount(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(s.*) ");
            sqlCommand.Append("FROM ");
            sqlCommand.Append("(SELECT DISTINCT userguid FROM mp_commercereport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid = :siteguid ");
            sqlCommand.Append(") s ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
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

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new NpgsqlParameter(":pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            arParams[2] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_commercereport_selectuserpage(:siteguid,:pagenumber,:pagesize)",
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the mp_CommerceReport table.
        /// </summary>
        public static int GetItemCountByUser(Guid siteGuid, Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM ");
            sqlCommand.Append("mp_commercereport ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid = :siteguid ");
            sqlCommand.Append("AND userguid = :userguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
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

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new NpgsqlParameter(":pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_commercereport  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid = :siteguid ");
            sqlCommand.Append("AND userguid = :userguid ");
            sqlCommand.Append("ORDER BY orderdateutc DESC  ");
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
            sqlCommand.Append("INSERT INTO mp_commercereportorders (");
            sqlCommand.Append("rowguid, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("featureguid, ");
            sqlCommand.Append("moduleguid, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("orderguid, ");
            sqlCommand.Append("billingfirstname, ");
            sqlCommand.Append("billinglastname, ");
            sqlCommand.Append("billingcompany, ");
            sqlCommand.Append("billingaddress1, ");
            sqlCommand.Append("billingaddress2, ");
            sqlCommand.Append("billingsuburb, ");
            sqlCommand.Append("billingcity, ");
            sqlCommand.Append("billingpostalcode, ");
            sqlCommand.Append("billingstate, ");
            sqlCommand.Append("billingcountry, ");
            sqlCommand.Append("paymentmethod, ");
            sqlCommand.Append("subtotal, ");
            sqlCommand.Append("taxtotal, ");
            sqlCommand.Append("shippingtotal, ");
            sqlCommand.Append("ordertotal, ");
            sqlCommand.Append("orderdateutc, ");
            sqlCommand.Append("adminorderlink, ");
            sqlCommand.Append("userorderlink, ");
            sqlCommand.Append("includeinaggregate, ");
            sqlCommand.Append("rowcreatedutc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":rowguid, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":featureguid, ");
            sqlCommand.Append(":moduleguid, ");
            sqlCommand.Append(":userguid, ");
            sqlCommand.Append(":orderguid, ");
            sqlCommand.Append(":billingfirstname, ");
            sqlCommand.Append(":billinglastname, ");
            sqlCommand.Append(":billingcompany, ");
            sqlCommand.Append(":billingaddress1, ");
            sqlCommand.Append(":billingaddress2, ");
            sqlCommand.Append(":billingsuburb, ");
            sqlCommand.Append(":billingcity, ");
            sqlCommand.Append(":billingpostalcode, ");
            sqlCommand.Append(":billingstate, ");
            sqlCommand.Append(":billingcountry, ");
            sqlCommand.Append(":paymentmethod, ");
            sqlCommand.Append(":subtotal, ");
            sqlCommand.Append(":taxtotal, ");
            sqlCommand.Append(":shippingtotal, ");
            sqlCommand.Append(":ordertotal, ");
            sqlCommand.Append(":orderdateutc, ");
            sqlCommand.Append(":adminorderlink, ");
            sqlCommand.Append(":userorderlink, ");
            sqlCommand.Append(":includeinaggregate, ");
            sqlCommand.Append(":rowcreatedutc ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[26];

            arParams[0] = new NpgsqlParameter(":rowguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter(":featureguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = featureGuid.ToString();

            arParams[3] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moduleGuid.ToString();

            arParams[4] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userGuid.ToString();

            arParams[5] = new NpgsqlParameter(":orderguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = orderGuid.ToString();

            arParams[6] = new NpgsqlParameter(":billingfirstname", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = billingFirstName;

            arParams[7] = new NpgsqlParameter(":billinglastname", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = billingLastName;

            arParams[8] = new NpgsqlParameter(":billingcompany", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = billingCompany;

            arParams[9] = new NpgsqlParameter(":billingaddress1", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = billingAddress1;

            arParams[10] = new NpgsqlParameter(":billingaddress2", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = billingAddress2;

            arParams[11] = new NpgsqlParameter(":billingsuburb", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = billingSuburb;

            arParams[12] = new NpgsqlParameter(":billingcity", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = billingCity;

            arParams[13] = new NpgsqlParameter(":billingpostalcode", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = billingPostalCode;

            arParams[14] = new NpgsqlParameter(":billingstate", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = billingState;

            arParams[15] = new NpgsqlParameter(":billingcountry", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = billingCountry;

            arParams[16] = new NpgsqlParameter(":paymentmethod", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = paymentMethod;

            arParams[17] = new NpgsqlParameter(":subtotal", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = subTotal;

            arParams[18] = new NpgsqlParameter(":taxtotal", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = taxTotal;

            arParams[19] = new NpgsqlParameter(":shippingtotal", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = shippingTotal;

            arParams[20] = new NpgsqlParameter(":ordertotal", NpgsqlTypes.NpgsqlDbType.Numeric);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = orderTotal;

            arParams[21] = new NpgsqlParameter(":orderdateutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = orderDateUtc;

            arParams[22] = new NpgsqlParameter(":adminorderlink", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = adminOrderLink;

            arParams[23] = new NpgsqlParameter(":userorderlink", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = userOrderLink;

            arParams[24] = new NpgsqlParameter(":rowcreatedutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = rowCreatedUtc;

            arParams[25] = new NpgsqlParameter(":includeinaggregate", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = includeInAggregate;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("DELETE FROM mp_commercereportorders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("rowguid = :rowguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":rowguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes  from the mp_CommerceReportOrders table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteOrdersBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_commercereportorders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid = :siteguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes from the mp_CommerceReportOrders table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteOrdersByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_commercereportorders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleguid = :moduleguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes from the mp_CommerceReportOrders table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteOrdersByOrder(Guid orderGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_commercereportorders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("orderguid = :orderguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":orderguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = orderGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_CommerceReportOrders table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteOrdersByFeature(Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_commercereportorders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("featureguid = :featureguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":featureguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_CommerceReportOrders table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteOrdersByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_commercereportorders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userguid = :userguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("UPDATE mp_commercereport ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("userguid = :userguid ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("orderguid = :orderguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = newUserGuid.ToString();

            arParams[1] = new NpgsqlParameter(":orderguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderGuid.ToString();

            
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_commercereportorders ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("userguid = :userguid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("orderguid = :orderguid ");
            sqlCommand.Append(";");

            arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = newUserGuid.ToString();

            arParams[1] = new NpgsqlParameter(":orderguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderGuid.ToString();


            NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

    }
}
