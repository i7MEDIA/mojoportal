/// Author:					Joe Audette
/// Created:				2008-02-24
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
using MySql.Data.MySqlClient;
using mojoPortal.Data;

namespace WebStore.Data
{
    public static class DBCartOffer
    {
        public static int Add(
            Guid itemGuid,
            Guid cartGuid,
            Guid offerGuid,
            Guid taxClassGuid,
            decimal offerPrice,
            DateTime addedToCart,
            int quantity,
            decimal tax,
            bool isDonation)
        {
            int intIsDonation;
            if (isDonation)
            {
                intIsDonation = 1;
            }
            else
            {
                intIsDonation = 0;
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO ws_CartOffers (");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("CartGuid, ");
            sqlCommand.Append("OfferGuid, ");
            sqlCommand.Append("TaxClassGuid, ");
            sqlCommand.Append("OfferPrice, ");
            sqlCommand.Append("AddedToCart, ");
            sqlCommand.Append("Quantity, ");
            sqlCommand.Append("IsDonation, ");
            sqlCommand.Append("Tax )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?ItemGuid, ");
            sqlCommand.Append("?CartGuid, ");
            sqlCommand.Append("?OfferGuid, ");
            sqlCommand.Append("?TaxClassGuid, ");
            sqlCommand.Append("?OfferPrice, ");
            sqlCommand.Append("?AddedToCart, ");
            sqlCommand.Append("?Quantity, ");
            sqlCommand.Append("?IsDonation, ");
            sqlCommand.Append("?Tax )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[9];

            arParams[0] = new MySqlParameter("?ItemGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemGuid.ToString();

            arParams[1] = new MySqlParameter("?CartGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = cartGuid.ToString();

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

            arParams[7] = new MySqlParameter("?Tax", MySqlDbType.Decimal);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = tax;

            arParams[8] = new MySqlParameter("?IsDonation", MySqlDbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = intIsDonation;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }


        public static bool Update(
            Guid itemGuid,
            Guid offerGuid,
            Guid taxClassGuid,
            decimal offerPrice,
            DateTime addedToCart,
            int quantity,
            decimal tax,
            bool isDonation)
        {
            int intIsDonation;
            if (isDonation)
            {
                intIsDonation = 1;
            }
            else
            {
                intIsDonation = 0;
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_CartOffers ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("OfferGuid = ?OfferGuid, ");
            sqlCommand.Append("TaxClassGuid = ?TaxClassGuid, ");
            sqlCommand.Append("OfferPrice = ?OfferPrice, ");
            sqlCommand.Append("AddedToCart = ?AddedToCart, ");
            sqlCommand.Append("Quantity = ?Quantity, ");
            sqlCommand.Append("IsDonation = ?IsDonation, ");
            sqlCommand.Append("Tax = ?Tax ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ItemGuid = ?ItemGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[8];

            arParams[0] = new MySqlParameter("?ItemGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemGuid.ToString();

            arParams[1] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = offerGuid.ToString();

            arParams[2] = new MySqlParameter("?TaxClassGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = taxClassGuid.ToString();

            arParams[3] = new MySqlParameter("?OfferPrice", MySqlDbType.Decimal);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = offerPrice;

            arParams[4] = new MySqlParameter("?AddedToCart", MySqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = addedToCart;

            arParams[5] = new MySqlParameter("?Quantity", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = quantity;

            arParams[6] = new MySqlParameter("?Tax", MySqlDbType.Decimal);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = tax;

            arParams[7] = new MySqlParameter("?IsDonation", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intIsDonation;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
            
        
        }


        public static bool Delete(Guid itemGuid)
        {
           
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_CartOffers ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemGuid = ?ItemGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);
        }

        public static bool DeleteByCart(Guid cartGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_CartOffers ");
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
            sqlCommand.Append("DELETE FROM ws_CartOffers ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CartGuid ");
            sqlCommand.Append(" IN (");
            sqlCommand.Append("SELECT CartGuid ");
            sqlCommand.Append("FROM ws_Cart ");
            sqlCommand.Append("WHERE	StoreGuid = ?StoreGuid ");
            sqlCommand.Append("AND UserGuid = '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("AND Created < ?OlderThan ");
            sqlCommand.Append(")");
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
            sqlCommand.Append("DELETE FROM ws_CartOffers ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CartGuid ");
            sqlCommand.Append(" IN (");
            sqlCommand.Append("SELECT CartGuid ");
            sqlCommand.Append("FROM ws_Cart ");
            sqlCommand.Append("WHERE	StoreGuid = ?StoreGuid ");
            sqlCommand.Append("AND Created < ?OlderThan ");
            sqlCommand.Append(")");
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


        public static IDataReader Get(Guid itemGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	ws_CartOffers ");
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

        public static IDataReader GetByCart(Guid cartGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("co.*, ");
            sqlCommand.Append("o.Name, ");
            sqlCommand.Append("o.Description, ");
            sqlCommand.Append("o.Price ");
            
            sqlCommand.Append(" ");

            sqlCommand.Append("FROM	ws_CartOffers co ");
            
            sqlCommand.Append("JOIN ");
            sqlCommand.Append("ws_Offer o ");
            sqlCommand.Append("ON co.OfferGuid = o.Guid ");
           

            sqlCommand.Append("WHERE ");

            sqlCommand.Append("co.CartGuid = ?CartGuid ");

            sqlCommand.Append("ORDER BY co.AddedToCart ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?CartGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();

            
            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }



    }
}
