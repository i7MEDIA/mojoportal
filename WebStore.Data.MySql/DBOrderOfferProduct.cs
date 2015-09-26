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
   
    public static class DBOrderOfferProduct
    {

        

        public static int Add(
            Guid guid,
            Guid orderGuid,
            Guid offerGuid,
            Guid productGuid,
            byte fullfillType,
            Guid fullfillTermsGuid,
            DateTime created)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO ws_OrderOfferProduct (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("OrderGuid, ");
            sqlCommand.Append("OfferGuid, ");
            sqlCommand.Append("ProductGuid, ");
            sqlCommand.Append("FullfillType, ");
            sqlCommand.Append("FullfillTermsGuid, ");
            sqlCommand.Append("Created )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?OrderGuid, ");
            sqlCommand.Append("?OfferGuid, ");
            sqlCommand.Append("?ProductGuid, ");
            sqlCommand.Append("?FullfillType, ");
            sqlCommand.Append("?FullfillTermsGuid, ");
            sqlCommand.Append("?Created )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[7];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderGuid.ToString();

            arParams[2] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = offerGuid.ToString();

            arParams[3] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = productGuid.ToString();

            arParams[4] = new MySqlParameter("?FullfillType", MySqlDbType.Int16);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = fullfillType;

            arParams[5] = new MySqlParameter("?FullfillTermsGuid", MySqlDbType.VarChar, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = fullfillTermsGuid.ToString();

            arParams[6] = new MySqlParameter("?Created", MySqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = created;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

        }

        public static bool Update(
            Guid guid,
            Guid orderGuid,
            Guid offerGuid,
            Guid productGuid,
            byte fullfillType,
            Guid fullfillTermsGuid,
            DateTime created)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_OrderOfferProduct ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("OrderGuid = ?OrderGuid, ");
            sqlCommand.Append("OfferGuid = ?OfferGuid, ");
            sqlCommand.Append("ProductGuid = ?ProductGuid, ");
            sqlCommand.Append("FullfillType = ?FullfillType, ");
            sqlCommand.Append("FullfillTermsGuid = ?FullfillTermsGuid ");
        
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderGuid.ToString();

            arParams[2] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = offerGuid.ToString();

            arParams[3] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = productGuid.ToString();

            arParams[4] = new MySqlParameter("?FullfillType", MySqlDbType.Int16);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = fullfillType;

            arParams[5] = new MySqlParameter("?FullfillTermsGuid", MySqlDbType.VarChar, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = fullfillTermsGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_OrderOfferProduct ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByOrder(Guid orderGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_OrderOfferProduct ");
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

        public static IDataReader Get(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	ws_OrderOfferProduct ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


    }
}
