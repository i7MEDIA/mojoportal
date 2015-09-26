using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using MySql.Data.MySqlClient;
using log4net;
//using Common = mojoPortal.Data.Common.WebStore;

namespace WebStore.Data
{
    /// <summary>
    /// Author:					Joe Audette
    /// Created:				2008-02-25
    /// Last Modified:		    2008-07-18
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
    ///  
    /// </summary>
    public static class DBOfferPrice
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DBOfferPrice));

        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["MySqlConnectionString"];

        }


        public static int Add(
            Guid guid,
            Guid offerGuid,
            Guid currencyGuid,
            decimal price,
            DateTime created,
            Guid createdBy,
            string createdFromIP,
            DateTime lastModifed,
            Guid lastModifiedBy,
            string modifiedFromIP)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO ws_OfferPrice (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("OfferGuid, ");
            sqlCommand.Append("CurrencyGuid, ");
            sqlCommand.Append("Price, ");
            sqlCommand.Append("Created, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("CreatedFromIP, ");
            sqlCommand.Append("LastModifed, ");
            sqlCommand.Append("LastModifiedBy, ");
            sqlCommand.Append("ModifiedFromIP )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?OfferGuid, ");
            sqlCommand.Append("?CurrencyGuid, ");
            sqlCommand.Append("?Price, ");
            sqlCommand.Append("?Created, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?CreatedFromIP, ");
            sqlCommand.Append("?LastModifed, ");
            sqlCommand.Append("?LastModifiedBy, ");
            sqlCommand.Append("?ModifiedFromIP )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[10];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = offerGuid.ToString();

            arParams[2] = new MySqlParameter("?CurrencyGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currencyGuid.ToString();

            arParams[3] = new MySqlParameter("?Price", MySqlDbType.Decimal);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = price;

            arParams[4] = new MySqlParameter("?Created", MySqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = created;

            arParams[5] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = createdBy.ToString();

            arParams[6] = new MySqlParameter("?CreatedFromIP", MySqlDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdFromIP;

            arParams[7] = new MySqlParameter("?LastModifed", MySqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = lastModifed;

            arParams[8] = new MySqlParameter("?LastModifiedBy", MySqlDbType.VarChar, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModifiedBy.ToString();

            arParams[9] = new MySqlParameter("?ModifiedFromIP", MySqlDbType.VarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = modifiedFromIP;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

            //return Common.DBOfferPrice.Add(
            //    guid,
            //    offerGuid,
            //    currencyGuid,
            //    price,
            //    created,
            //    createdBy,
            //    createdFromIP,
            //    lastModifed,
            //    lastModifiedBy,
            //    modifiedFromIP);
        }


        public static bool Update(
            Guid guid,
            Guid currencyGuid,
            decimal price,
            DateTime lastModifed,
            Guid lastModifiedBy,
            string modifiedFromIP)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_OfferPrice ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("CurrencyGuid = ?CurrencyGuid, ");
            sqlCommand.Append("Price = ?Price, ");
            
            sqlCommand.Append("LastModifed = ?LastModifed, ");
            sqlCommand.Append("LastModifiedBy = ?LastModifiedBy, ");
            sqlCommand.Append("ModifiedFromIP = ?ModifiedFromIP ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?CurrencyGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currencyGuid.ToString();

            arParams[2] = new MySqlParameter("?Price", MySqlDbType.Decimal);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = price;

            arParams[3] = new MySqlParameter("?LastModifed", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = lastModifed;

            arParams[4] = new MySqlParameter("?LastModifiedBy", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = lastModifiedBy.ToString();

            arParams[5] = new MySqlParameter("?ModifiedFromIP", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = modifiedFromIP;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

            //return Common.DBOfferPrice.Update(
            //    guid,
            //    currencyGuid,
            //    price,
            //    lastModifed,
            //    lastModifiedBy,
            //    modifiedFromIP);

        }

        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_OfferPrice ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

            //return Common.DBOfferPrice.Delete(guid);
        }

        public static IDataReader Get(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	ws_OfferPrice ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return MySqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            //return Common.DBOfferPrice.Get(guid);
        }

        public static IDataReader GetByOffer(Guid offerGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  op.*, ");
            sqlCommand.Append("c.Title As Currency, ");
            sqlCommand.Append("c.Code As CurrencyCode, ");
            sqlCommand.Append("c.SymbolLeft, ");
            sqlCommand.Append("c.SymbolRight ");
            //sqlCommand.Append(" ");

            sqlCommand.Append("FROM	ws_OfferPrice op ");
            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Currency c ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("c.Guid = op.CurrencyGuid ");


            sqlCommand.Append("WHERE ");
            sqlCommand.Append("op.OfferGuid = ?OfferGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = offerGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static int AddHistory(
            Guid guid,
            Guid priceGuid,
            Guid offerGuid,
            Guid currencyGuid,
            decimal price,
            DateTime created,
            Guid createdBy,
            string createdFromIP,
            DateTime lastModifed,
            Guid lastModifiedBy,
            string modifiedFromIP,
            DateTime logTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO ws_OfferPriceHistory (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("PriceGuid, ");
            sqlCommand.Append("OfferGuid, ");
            sqlCommand.Append("CurrencyGuid, ");
            sqlCommand.Append("Price, ");
            sqlCommand.Append("Created, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("CreatedFromIP, ");
            sqlCommand.Append("LastModifed, ");
            sqlCommand.Append("LastModifiedBy, ");
            sqlCommand.Append("ModifiedFromIP, ");
            sqlCommand.Append("LogTime )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?PriceGuid, ");
            sqlCommand.Append("?OfferGuid, ");
            sqlCommand.Append("?CurrencyGuid, ");
            sqlCommand.Append("?Price, ");
            sqlCommand.Append("?Created, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?CreatedFromIP, ");
            sqlCommand.Append("?LastModifed, ");
            sqlCommand.Append("?LastModifiedBy, ");
            sqlCommand.Append("?ModifiedFromIP, ");
            sqlCommand.Append("?LogTime )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[12];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?PriceGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = priceGuid.ToString();

            arParams[2] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = offerGuid.ToString();

            arParams[3] = new MySqlParameter("?CurrencyGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = currencyGuid.ToString();

            arParams[4] = new MySqlParameter("?Price", MySqlDbType.Decimal);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = price;

            arParams[5] = new MySqlParameter("?Created", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = created;

            arParams[6] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdBy.ToString();

            arParams[7] = new MySqlParameter("?CreatedFromIP", MySqlDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createdFromIP;

            arParams[8] = new MySqlParameter("?LastModifed", MySqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModifed;

            arParams[9] = new MySqlParameter("?LastModifiedBy", MySqlDbType.VarChar, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModifiedBy.ToString();

            arParams[10] = new MySqlParameter("?ModifiedFromIP", MySqlDbType.VarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = modifiedFromIP;

            arParams[11] = new MySqlParameter("?LogTime", MySqlDbType.DateTime);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = logTime;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

            //return Common.DBOfferPrice.AddHistory(
            //    guid,
            //    priceGuid,
            //    offerGuid,
            //    currencyGuid,
            //    price,
            //    created,
            //    createdBy,
            //    createdFromIP,
            //    lastModifed,
            //    lastModifiedBy,
            //    modifiedFromIP,
            //    logTime);

        }


    }
}
