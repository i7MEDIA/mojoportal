using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using log4net;

namespace WebStore.Data
{
    /// <summary>
    /// Author:				Joe Audette
    /// Created:			2007-11-14
    /// Last Modified:		2007-11-14
    /// 
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    ///  
    /// </summary>
    public static class DBOfferPrice
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DBOfferPrice));


        private static string GetConnectionString()
        {
            if (ConfigurationManager.AppSettings["WebStoreMSSQLConnectionString"] != null)
            {
                return ConfigurationManager.AppSettings["WebStoreMSSQLConnectionString"];
            }

            return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        }

        public static String DBPlatform()
        {
            return "MSSQL";
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

            SqlParameter[] arParams = new SqlParameter[9];
            if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            {
                arParams = SqlHelperParameterCache.GetParameterSet(GetConnectionString(),
                    "ws_OfferPrice_Insert");

                arParams[0].Value = guid;
                arParams[1].Value = offerGuid;
                arParams[2].Value = currencyGuid;
                arParams[3].Value = price;
                arParams[4].Value = created;
                arParams[5].Value = createdBy;
                arParams[6].Value = createdFromIP;
                arParams[7].Value = lastModifed;
                arParams[8].Value = lastModifiedBy;
                arParams[9].Value = modifiedFromIP;

            }
            else
            {
                arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = guid;

                arParams[1] = new SqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = offerGuid;

                arParams[2] = new SqlParameter("@CurrencyGuid", SqlDbType.UniqueIdentifier);
                arParams[2].Direction = ParameterDirection.Input;
                arParams[2].Value = currencyGuid;

                arParams[3] = new SqlParameter("@Price", SqlDbType.Decimal);
                arParams[3].Direction = ParameterDirection.Input;
                arParams[3].Value = price;

                arParams[4] = new SqlParameter("@Created", SqlDbType.DateTime);
                arParams[4].Direction = ParameterDirection.Input;
                arParams[4].Value = created;

                arParams[5] = new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier);
                arParams[5].Direction = ParameterDirection.Input;
                arParams[5].Value = createdBy;

                arParams[6] = new SqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 255);
                arParams[6].Direction = ParameterDirection.Input;
                arParams[6].Value = createdFromIP;

                arParams[7] = new SqlParameter("@LastModifed", SqlDbType.DateTime);
                arParams[7].Direction = ParameterDirection.Input;
                arParams[7].Value = lastModifed;

                arParams[8] = new SqlParameter("@LastModifiedBy", SqlDbType.UniqueIdentifier);
                arParams[8].Direction = ParameterDirection.Input;
                arParams[8].Value = lastModifiedBy;

                arParams[9] = new SqlParameter("@ModifiedFromIP", SqlDbType.NVarChar, 255);
                arParams[9].Direction = ParameterDirection.Input;
                arParams[9].Value = modifiedFromIP;


            }

            int rowsAffected = Convert.ToInt32(SqlHelper.ExecuteNonQuery(GetConnectionString(),
                CommandType.StoredProcedure,
                "ws_OfferPrice_Insert",
                arParams));

            return rowsAffected;

        }


        public static bool Update(
            Guid guid,
            Guid currencyGuid,
            decimal price,
            DateTime lastModifed,
            Guid lastModifiedBy,
            string modifiedFromIP)
        {
            SqlParameter[] arParams = new SqlParameter[6];
            if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            {
                arParams = SqlHelperParameterCache.GetParameterSet(GetConnectionString(),
                    "ws_OfferPrice_Update");

                arParams[0].Value = guid;
                arParams[1].Value = currencyGuid;
                arParams[2].Value = price;
                arParams[3].Value = lastModifed;
                arParams[4].Value = lastModifiedBy;
                arParams[5].Value = modifiedFromIP;

            }
            else
            {
                arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = guid;

                arParams[1] = new SqlParameter("@CurrencyGuid", SqlDbType.UniqueIdentifier);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = currencyGuid;

                arParams[2] = new SqlParameter("@Price", SqlDbType.Decimal);
                arParams[2].Direction = ParameterDirection.Input;
                arParams[2].Value = price;

                arParams[3] = new SqlParameter("@LastModifed", SqlDbType.DateTime);
                arParams[3].Direction = ParameterDirection.Input;
                arParams[3].Value = lastModifed;

                arParams[4] = new SqlParameter("@LastModifiedBy", SqlDbType.UniqueIdentifier);
                arParams[4].Direction = ParameterDirection.Input;
                arParams[4].Value = lastModifiedBy;

                arParams[5] = new SqlParameter("@ModifiedFromIP", SqlDbType.NVarChar, 255);
                arParams[5].Direction = ParameterDirection.Input;
                arParams[5].Value = modifiedFromIP;

            }

            int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
                CommandType.StoredProcedure,
                "ws_OfferPrice_Update",
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(Guid guid)
        {
            SqlParameter[] arParams = new SqlParameter[1];
            if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            {
                arParams = SqlHelperParameterCache.GetParameterSet(GetConnectionString(),
                    "ws_OfferPrice_Delete");

                arParams[0].Value = guid;

            }
            else
            {
                arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = guid;


            }

            int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
                CommandType.StoredProcedure,
                "ws_OfferPrice_Delete",
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader Get(Guid guid)
        {
            SqlParameter[] arParams = new SqlParameter[1];
            if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            {
                arParams = SqlHelperParameterCache.GetParameterSet(GetConnectionString(),
                    "ws_OfferPrice_SelectOne");

                arParams[0].Value = guid;

            }
            else
            {
                arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = guid;

            }

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "ws_OfferPrice_SelectOne",
                arParams);

        }

        public static IDataReader GetByOffer(Guid offerGuid)
        {
            SqlParameter[] arParams = new SqlParameter[1];
            if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            {
                arParams = SqlHelperParameterCache.GetParameterSet(GetConnectionString(),
                    "ws_OfferPrice_SelectByOffer");

                arParams[0].Value = offerGuid;

            }
            else
            {
                arParams[0] = new SqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = offerGuid;

            }

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "ws_OfferPrice_SelectByOffer",
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

            SqlParameter[] arParams = new SqlParameter[12];
            if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            {
                arParams = SqlHelperParameterCache.GetParameterSet(GetConnectionString(),
                    "ws_OfferPriceHistory_Insert");

                arParams[0].Value = guid;
                arParams[1].Value = priceGuid;
                arParams[2].Value = offerGuid;
                arParams[3].Value = currencyGuid;
                arParams[4].Value = price;
                arParams[5].Value = created;
                arParams[6].Value = createdBy;
                arParams[7].Value = createdFromIP;
                arParams[8].Value = lastModifed;
                arParams[9].Value = lastModifiedBy;
                arParams[10].Value = modifiedFromIP;
                arParams[11].Value = logTime;

            }
            else
            {
                arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = guid;

                arParams[1] = new SqlParameter("@PriceGuid", SqlDbType.UniqueIdentifier);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = priceGuid;

                arParams[2] = new SqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier);
                arParams[2].Direction = ParameterDirection.Input;
                arParams[2].Value = offerGuid;

                arParams[3] = new SqlParameter("@CurrencyGuid", SqlDbType.UniqueIdentifier);
                arParams[3].Direction = ParameterDirection.Input;
                arParams[3].Value = currencyGuid;

                arParams[4] = new SqlParameter("@Price", SqlDbType.Decimal);
                arParams[4].Direction = ParameterDirection.Input;
                arParams[4].Value = price;

                arParams[5] = new SqlParameter("@Created", SqlDbType.DateTime);
                arParams[5].Direction = ParameterDirection.Input;
                arParams[5].Value = created;

                arParams[6] = new SqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier);
                arParams[6].Direction = ParameterDirection.Input;
                arParams[6].Value = createdBy;

                arParams[7] = new SqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 255);
                arParams[7].Direction = ParameterDirection.Input;
                arParams[7].Value = createdFromIP;

                arParams[8] = new SqlParameter("@LastModifed", SqlDbType.DateTime);
                arParams[8].Direction = ParameterDirection.Input;
                arParams[8].Value = lastModifed;

                arParams[9] = new SqlParameter("@LastModifiedBy", SqlDbType.UniqueIdentifier);
                arParams[9].Direction = ParameterDirection.Input;
                arParams[9].Value = lastModifiedBy;

                arParams[10] = new SqlParameter("@ModifiedFromIP", SqlDbType.NVarChar, 255);
                arParams[10].Direction = ParameterDirection.Input;
                arParams[10].Value = modifiedFromIP;

                arParams[11] = new SqlParameter("@LogTime", SqlDbType.DateTime);
                arParams[11].Direction = ParameterDirection.Input;
                arParams[11].Value = logTime;


            }

            int rowsAffected = Convert.ToInt32(SqlHelper.ExecuteNonQuery(GetConnectionString(),
                CommandType.StoredProcedure,
                "ws_OfferPriceHistory_Insert",
                arParams));

            return rowsAffected;

        }

        

    }
}
