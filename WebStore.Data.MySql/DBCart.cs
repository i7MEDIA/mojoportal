/// Author:					Joe Audette
/// Created:				2008-02-24
/// Last Modified:		    2012-07-20
/// 
/// 
/// The use and distribution terms for this software are covered by the 
/// GPL (http://www.gnu.org/licenses/gpl.html)
/// which can be found in the file GPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using MySql.Data.MySqlClient;
using mojoPortal.Data;


namespace WebStore.Data
{   
    
    public static class DBCart
    {

        

        public static int AddCart(
            Guid cartGuid,
            Guid storeGuid,
            Guid userGuid,
            decimal subTotal,
            decimal shippingTotal,
            decimal taxTotal,
            decimal orderTotal,
            DateTime created,
            string createdFromIP,
            DateTime lastModified,
            DateTime lastUserActivity,
            decimal discount,
            string discountCodesCsv,
            string customData,
            Guid clerkGuid)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO ws_Cart (");
            sqlCommand.Append("CartGuid, ");
            sqlCommand.Append("StoreGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("ClerkGuid, ");
            sqlCommand.Append("SubTotal, ");
            sqlCommand.Append("TaxTotal, ");
            sqlCommand.Append("ShippingTotal, ");
            sqlCommand.Append("Discount, ");
            sqlCommand.Append("OrderTotal, ");

            sqlCommand.Append("DiscountCodesCsv, ");
            sqlCommand.Append("CustomData, ");

            sqlCommand.Append("Created, ");
            sqlCommand.Append("CreatedFromIP, ");
            sqlCommand.Append("LastModified, ");
            sqlCommand.Append("LastUserActivity )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?CartGuid, ");
            sqlCommand.Append("?StoreGuid, ");
            sqlCommand.Append("?UserGuid, ");
            sqlCommand.Append("?ClerkGuid, ");
            sqlCommand.Append("?SubTotal, ");
            sqlCommand.Append("?TaxTotal, ");
            sqlCommand.Append("?ShippingTotal, ");
            sqlCommand.Append("?Discount, ");
            sqlCommand.Append("?OrderTotal, ");
            sqlCommand.Append("?DiscountCodesCsv, ");
            sqlCommand.Append("?CustomData, ");
            sqlCommand.Append("?Created, ");
            sqlCommand.Append("?CreatedFromIP, ");
            sqlCommand.Append("?LastModified, ");
            sqlCommand.Append("?LastUserActivity )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[15];

            arParams[0] = new MySqlParameter("?CartGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();

            arParams[1] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = storeGuid.ToString();

            arParams[2] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid.ToString();

            arParams[3] = new MySqlParameter("?SubTotal", MySqlDbType.Decimal);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = subTotal;

            arParams[4] = new MySqlParameter("?TaxTotal", MySqlDbType.Decimal);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = taxTotal;

            arParams[5] = new MySqlParameter("?ShippingTotal", MySqlDbType.Decimal);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = shippingTotal;

            arParams[6] = new MySqlParameter("?OrderTotal", MySqlDbType.Decimal);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = orderTotal;

            arParams[7] = new MySqlParameter("?Created", MySqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = created;

            arParams[8] = new MySqlParameter("?CreatedFromIP", MySqlDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdFromIP;

            arParams[9] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModified;

            arParams[10] = new MySqlParameter("?LastUserActivity", MySqlDbType.DateTime);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = lastUserActivity;

            arParams[11] = new MySqlParameter("?Discount", MySqlDbType.Decimal);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = discount;

            arParams[12] = new MySqlParameter("?DiscountCodesCsv", MySqlDbType.Text);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = discountCodesCsv;

            arParams[13] = new MySqlParameter("?CustomData", MySqlDbType.Text);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = customData;

            arParams[14] = new MySqlParameter("?ClerkGuid", MySqlDbType.VarChar, 36);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = clerkGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

        }


        public static bool UpdateCart(
            Guid cartGuid,
            Guid userGuid,
            decimal subTotal,
            decimal shippingTotal,
            decimal taxTotal,
            decimal orderTotal,
            DateTime lastModified,
            DateTime lastUserActivity,
            decimal discount,
            string discountCodesCsv,
            string customData,
            Guid clerkGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_Cart ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("UserGuid = ?UserGuid, ");
            sqlCommand.Append("ClerkGuid = ?ClerkGuid, ");
            sqlCommand.Append("SubTotal = ?SubTotal, ");
            sqlCommand.Append("TaxTotal = ?TaxTotal, ");
            sqlCommand.Append("ShippingTotal = ?ShippingTotal, ");
            sqlCommand.Append("Discount = ?Discount, ");
            sqlCommand.Append("OrderTotal = ?OrderTotal, ");

            sqlCommand.Append("DiscountCodesCsv = ?DiscountCodesCsv, ");
            sqlCommand.Append("CustomData = ?CustomData, ");

            sqlCommand.Append("LastModified = ?LastModified, ");
            sqlCommand.Append("LastUserActivity = ?LastUserActivity ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("CartGuid = ?CartGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[12];

            arParams[0] = new MySqlParameter("?CartGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new MySqlParameter("?SubTotal", MySqlDbType.Decimal);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = subTotal;

            arParams[3] = new MySqlParameter("?TaxTotal", MySqlDbType.Decimal);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = taxTotal;

            arParams[4] = new MySqlParameter("?ShippingTotal", MySqlDbType.Decimal);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = shippingTotal;

            arParams[5] = new MySqlParameter("?OrderTotal", MySqlDbType.Decimal);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = orderTotal;

            arParams[6] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = lastModified;

            arParams[7] = new MySqlParameter("?LastUserActivity", MySqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = lastUserActivity;

            arParams[8] = new MySqlParameter("?Discount", MySqlDbType.Decimal);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = discount;

            arParams[9] = new MySqlParameter("?DiscountCodesCsv", MySqlDbType.Text);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = discountCodesCsv;

            arParams[10] = new MySqlParameter("?CustomData", MySqlDbType.Text);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = customData;

            arParams[11] = new MySqlParameter("?ClerkGuid", MySqlDbType.VarChar, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = clerkGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
            
        }

        public static bool DeleteCart(Guid cartGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_Cart ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CartGuid = ?CartGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?CartGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        public static bool DeleteAnonymousByStore(Guid storeGuid, DateTime olderThan)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_Cart ");
            
            sqlCommand.Append("WHERE StoreGuid = ?StoreGuid ");
            sqlCommand.Append("AND UserGuid = '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("AND Created < ?OlderThan ");
            
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            arParams[1] = new MySqlParameter("?OlderThan", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = olderThan;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        public static bool DeleteByStore(Guid storeGuid, DateTime olderThan)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_Cart ");
            sqlCommand.Append("WHERE StoreGuid = ?StoreGuid ");
            sqlCommand.Append("AND Created < ?OlderThan ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            arParams[1] = new MySqlParameter("?OlderThan", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = olderThan;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static IDataReader GetCart(Guid cartGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	ws_Cart ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CartGuid = ?CartGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?CartGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

            //return Common.DBCart.GetCart(cartGuid);
        }

        public static IDataReader GetByUser(Guid userGuid, Guid storeGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	ws_Cart ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = ?UserGuid ");
            sqlCommand.Append("AND StoreGuid = ?StoreGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = storeGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

            //return Common.DBCart.GetByUser(userGuid);

        }


        /// <summary>
        /// Gets a count of rows in the ws_Cart table.
        /// </summary>
        public static int GetCount(Guid storeGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	ws_Cart ");
            sqlCommand.Append("WHERE StoreGuid = ?StoreGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        public static int GetItemCountByFulfillmentType(Guid cartGuid, byte fulFillmentType)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");

            sqlCommand.Append("FROM	ws_OfferProduct op ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("ws_CartOffers co ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("op.OfferGuid = co.OfferGuid ");

            sqlCommand.Append("WHERE co.CartGuid = ?CartGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("op.FullfillType = ?FulFillmentType ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("op.IsDeleted = 0 ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?CartGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();

            arParams[1] = new MySqlParameter("?FulFillmentType", MySqlDbType.Byte);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = fulFillmentType;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        /// <summary>
        /// Gets a page of data from the ws_Cart table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(storeGuid);

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

            sqlCommand.Append("SELECT c2.*, ");
            sqlCommand.Append("u3.Name,  ");
            sqlCommand.Append("u3.LoginName,  ");
            sqlCommand.Append("u3.Email,  ");
            sqlCommand.Append("u2.Email As IPUser ");
            sqlCommand.Append("FROM  ");

            sqlCommand.Append("(SELECT c.CartGuid, ");
            //sqlCommand.Append("c.StoreGuid,  ");
            //sqlCommand.Append("c.UserGuid,  ");
            //sqlCommand.Append("c.SubTotal,  ");
            //sqlCommand.Append("c.TaxTotal,  ");
            //sqlCommand.Append("c.ShippingTotal,  ");
            //sqlCommand.Append("c.OrderTotal,  ");
            //sqlCommand.Append("c.Created,  ");
            //sqlCommand.Append("c.CreatedFromIP,  ");
            //sqlCommand.Append("c.LastModified,  ");
            //sqlCommand.Append("c.LastUserActivity,  ");
            //sqlCommand.Append("c.CustomData,  ");
            //sqlCommand.Append("c.DiscountCodesCsv,  ");
            //sqlCommand.Append("c.Discount,  ");
            //sqlCommand.Append("c.ClerkGuid,  ");
            sqlCommand.Append("(SELECT COALESCE(ul.UserGuid, '00000000-0000-0000-0000-000000000000') FROM mp_UserLocation ul WHERE ul.IPAddress = c.CreatedFromIP LIMIT 1 ) AS IPUserGuid ");

            sqlCommand.Append("FROM	ws_Cart c  ");

            //sqlCommand.Append("LEFT OUTER JOIN mp_Users u  ");
           // sqlCommand.Append("ON  ");
            //sqlCommand.Append("u.UserGuid = c.UserGuid  ");
            //sqlCommand.Append("AND ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("c.StoreGuid = ?StoreGuid ");
            sqlCommand.Append(") t ");

            sqlCommand.Append("JOIN ws_Cart c2 ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("t.CartGuid = c2.CartGuid ");

           
            sqlCommand.Append("LEFT OUTER JOIN mp_Users u3  ");
            sqlCommand.Append("ON  ");
            sqlCommand.Append("u3.UserGuid = c2.UserGuid  ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users u2  ");
            sqlCommand.Append("ON  ");
            sqlCommand.Append("u2.UserGuid = t.IPUserGuid  ");



            sqlCommand.Append("WHERE c2.StoreGuid = ?StoreGuid ");

            sqlCommand.Append("ORDER BY c2.LastModified desc  ");

            sqlCommand.Append("LIMIT " + pageLowerBound.ToString(CultureInfo.InvariantCulture)
                + "," + pageSize.ToString(CultureInfo.InvariantCulture));

            sqlCommand.Append("  ");

            sqlCommand.Append(";");

            //sqlCommand.Append("SELECT c.*, ");
            //sqlCommand.Append("u.Name,  ");
            //sqlCommand.Append("u.LoginName,  ");
            //sqlCommand.Append("u.Email,  ");
            //sqlCommand.Append("u2.Email As IPUser ");
            
            //sqlCommand.Append("FROM	ws_Cart c  ");

            //sqlCommand.Append("LEFT OUTER JOIN mp_Users u  ");
            //sqlCommand.Append("ON  ");
            //sqlCommand.Append("u.UserGuid = c.UserGuid ");

            //sqlCommand.Append("LEFT OUTER JOIN mp_Users u2  ");
            //sqlCommand.Append("ON  ");
            //sqlCommand.Append("u2.UserGuid IN  ");
            //sqlCommand.Append("(SELECT ul.UserGuid FROM mp_UserLocation ul WHERE ul.IPAddress = c.CreatedFromIP LIMIT 1 ) ");



            //sqlCommand.Append("WHERE t.StoreGuid = ?StoreGuid ");

            //sqlCommand.Append("ORDER BY t.LastModified desc  ");

            //sqlCommand.Append("LIMIT " + pageLowerBound.ToString(CultureInfo.InvariantCulture)
            //    + "," + pageSize.ToString(CultureInfo.InvariantCulture));

            //sqlCommand.Append("  ");

            //sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }




    }
}
