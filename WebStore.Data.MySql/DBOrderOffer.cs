/// Author:					Joe Audette
/// Created:				2008-02-25
/// Last Modified:		    2012-07-20
/// 
/// This implementation is for MySQL. 
/// 
/// The use and distribution terms for this software are covered by the 
/// GPL (http://www.gnu.org/licenses/gpl.html)
/// which can be found in the file GPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using MySql.Data.MySqlClient;
using mojoPortal.Data;

namespace WebStore.Data
{
    public static class DBOrderOffer
    {

        

        public static int Add(
            Guid itemGuid,
            Guid orderGuid,
            Guid offerGuid,
            Guid taxClassGuid,
            decimal offerPrice,
            DateTime addedToCart,
            int quantity)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO ws_OrderOffers (");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("OrderGuid, ");
            sqlCommand.Append("OfferGuid, ");
            
            sqlCommand.Append("TaxClassGuid, ");
            sqlCommand.Append("OfferPrice, ");
            sqlCommand.Append("AddedToCart, ");
            sqlCommand.Append("Quantity )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?ItemGuid, ");
            sqlCommand.Append("?OrderGuid, ");
            sqlCommand.Append("?OfferGuid, ");
            
            sqlCommand.Append("?TaxClassGuid, ");
            sqlCommand.Append("?OfferPrice, ");
            sqlCommand.Append("?AddedToCart, ");
            sqlCommand.Append("?Quantity )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[7];

            arParams[0] = new MySqlParameter("?ItemGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderGuid.ToString();

            arParams[2] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = offerGuid.ToString();

            arParams[3] = new MySqlParameter("?TaxClassGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = taxClassGuid.ToString();

            arParams[4] = new MySqlParameter("?OfferPrice", MySqlDbType.Decimal);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = offerPrice;

            arParams[5] = new MySqlParameter("?AddedToCart", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = addedToCart;

            arParams[6] = new MySqlParameter("?Quantity", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = quantity;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

          

        }

        public static bool Update(
            Guid itemGuid,
            Guid orderGuid,
            Guid offerGuid,
            Guid taxClassGuid,
            decimal offerPrice,
            DateTime addedToCart,
            int quantity)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_OrderOffers ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("OrderGuid = ?OrderGuid, ");
            sqlCommand.Append("OfferGuid = ?OfferGuid, ");
            sqlCommand.Append("PriceGuid = ?PriceGuid, ");
           
            sqlCommand.Append("OfferPrice = ?OfferPrice, ");
            sqlCommand.Append("AddedToCart = ?AddedToCart, ");
            sqlCommand.Append("Quantity = ?Quantity ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ItemGuid = ?ItemGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[7];

            arParams[0] = new MySqlParameter("?ItemGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderGuid.ToString();

            arParams[2] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = offerGuid.ToString();

            arParams[3] = new MySqlParameter("?TaxClassGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = taxClassGuid.ToString();

            arParams[4] = new MySqlParameter("?OfferPrice", MySqlDbType.Decimal);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = offerPrice;

            arParams[5] = new MySqlParameter("?AddedToCart", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = addedToCart;

            arParams[6] = new MySqlParameter("?Quantity", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = quantity;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

           
        }


        public static IDataReader Get(Guid itemGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	ws_OrderOffers ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemGuid = ?ItemGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

            
        }

        public static IDataReader GetByOrder(Guid orderGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  o.*, ");
            sqlCommand.Append("od.Name ");

            sqlCommand.Append("FROM	ws_OrderOffers o ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("ws_Offer od ");
            sqlCommand.Append("ON o.OfferGuid = od.Guid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("o.OrderGuid = ?OrderGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = orderGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

           
        }

        public static bool DeleteByOrder(Guid orderGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_OrderOffers ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("OrderGuid = ?OrderGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = orderGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

    }
}
