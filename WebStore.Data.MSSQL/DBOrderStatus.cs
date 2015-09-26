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
    public static class DBOrderStatus
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DBOrderStatus));


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
            int sort)
        {

            SqlParameter[] arParams = new SqlParameter[1];
            if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            {
                arParams = SqlHelperParameterCache.GetParameterSet(GetConnectionString(),
                    "ws_OrderStatus_Insert");

                arParams[0].Value = guid;
                arParams[1].Value = sort;

            }
            else
            {
                arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = guid;

                arParams[1] = new SqlParameter("@Sort", SqlDbType.Int);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = sort;


            }

            int rowsAffected = Convert.ToInt32(SqlHelper.ExecuteNonQuery(GetConnectionString(),
                CommandType.StoredProcedure,
                "ws_OrderStatus_Insert",
                arParams));

            return rowsAffected;

        }


        public static bool Update(
            Guid guid,
            int sort)
        {
            SqlParameter[] arParams = new SqlParameter[2];
            if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            {
                arParams = SqlHelperParameterCache.GetParameterSet(GetConnectionString(),
                    "ws_OrderStatus_Update");

                arParams[0].Value = guid;
                arParams[1].Value = sort;

            }
            else
            {
                arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = guid;

                arParams[1] = new SqlParameter("@Sort", SqlDbType.Int);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = sort;

            }

            int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
                CommandType.StoredProcedure,
                "ws_OrderStatus_Update",
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(Guid guid)
        {
            SqlParameter[] arParams = new SqlParameter[1];
            if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            {
                arParams = SqlHelperParameterCache.GetParameterSet(GetConnectionString(),
                    "ws_OrderStatus_Delete");

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
                "ws_OrderStatus_Delete",
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader Get(Guid guid)
        {
            SqlParameter[] arParams = new SqlParameter[1];
            if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            {
                arParams = SqlHelperParameterCache.GetParameterSet(GetConnectionString(),
                    "ws_OrderStatus_SelectOne");

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
                "ws_OrderStatus_SelectOne",
                arParams);

        }

        public static IDataReader GetAll(Guid languageGuid)
        {
            SqlParameter[] arParams = new SqlParameter[1];
            if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            {
                arParams = SqlHelperParameterCache.GetParameterSet(GetConnectionString(),
                    "ws_OrderStatus_SelectByLanguage");

                arParams[0].Value = languageGuid;

            }
            else
            {
                arParams[0] = new SqlParameter("@Guid", SqlDbType.UniqueIdentifier);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = languageGuid;

            }

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "ws_OrderStatus_SelectByLanguage",
                arParams);

        }



        

        public static int AddDescription(
            Guid statusGuid,
            Guid languageGuid,
            string description)
        {

            SqlParameter[] arParams = new SqlParameter[1];
            if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            {
                arParams = SqlHelperParameterCache.GetParameterSet(GetConnectionString(),
                    "ws_OrderStatusDescription_Insert");

                arParams[0].Value = statusGuid;
                arParams[1].Value = languageGuid;
                arParams[2].Value = description;

            }
            else
            {
                arParams[0] = new SqlParameter("@StatusGuid", SqlDbType.UniqueIdentifier);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = statusGuid;

                arParams[1] = new SqlParameter("@LanguageGuid", SqlDbType.UniqueIdentifier);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = languageGuid;

                arParams[2] = new SqlParameter("@Description", SqlDbType.NVarChar, 255);
                arParams[2].Direction = ParameterDirection.Input;
                arParams[2].Value = description;


            }

            int rowsAffected = Convert.ToInt32(SqlHelper.ExecuteNonQuery(GetConnectionString(),
                CommandType.StoredProcedure,
                "ws_OrderStatusDescription_Insert",
                arParams));

            return rowsAffected;

        }


        public static bool UpdateDescription(
            Guid statusGuid,
            Guid languageGuid,
            string description)
        {
            SqlParameter[] arParams = new SqlParameter[3];
            if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            {
                arParams = SqlHelperParameterCache.GetParameterSet(GetConnectionString(),
                    "ws_OrderStatusDescription_Update");

                arParams[0].Value = statusGuid;
                arParams[1].Value = languageGuid;
                arParams[2].Value = description;

            }
            else
            {
                arParams[0] = new SqlParameter("@StatusGuid", SqlDbType.UniqueIdentifier);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = statusGuid;

                arParams[1] = new SqlParameter("@LanguageGuid", SqlDbType.UniqueIdentifier);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = languageGuid;

                arParams[2] = new SqlParameter("@Description", SqlDbType.NVarChar, 255);
                arParams[2].Direction = ParameterDirection.Input;
                arParams[2].Value = description;

            }

            int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
                CommandType.StoredProcedure,
                "ws_OrderStatusDescription_Update",
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteDescription(
            Guid statusGuid,
            Guid languageGuid)
        {
            SqlParameter[] arParams = new SqlParameter[2];
            if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            {
                arParams = SqlHelperParameterCache.GetParameterSet(GetConnectionString(),
                    "ws_OrderStatusDescription_Delete");

                arParams[0].Value = statusGuid;
                arParams[1].Value = languageGuid;

            }
            else
            {
                arParams[0] = new SqlParameter("@StatusGuid", SqlDbType.UniqueIdentifier);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = statusGuid;

                arParams[1] = new SqlParameter("@LanguageGuid", SqlDbType.UniqueIdentifier);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = languageGuid;

            }

            int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
                CommandType.StoredProcedure,
                "ws_OrderStatusDescription_Delete",
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteDescription(Guid statusGuid)
        {
            SqlParameter[] arParams = new SqlParameter[1];
            if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            {
                arParams = SqlHelperParameterCache.GetParameterSet(GetConnectionString(),
                    "ws_OrderStatusDescription_DeleteByStatus");

                arParams[0].Value = statusGuid;

            }
            else
            {
                arParams[0] = new SqlParameter("@StatusGuid", SqlDbType.UniqueIdentifier);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = statusGuid;

            }

            int rowsAffected = SqlHelper.ExecuteNonQuery(GetConnectionString(),
                CommandType.StoredProcedure,
                "ws_OrderStatusDescription_DeleteByStatus",
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetDescription(
            Guid statusGuid,
            Guid languageGuid)
        {
            SqlParameter[] arParams = new SqlParameter[2];
            if (ConfigurationManager.AppSettings["CacheMSSQLParameters"].ToLower() == "true")
            {
                arParams = SqlHelperParameterCache.GetParameterSet(GetConnectionString(),
                    "ws_OrderStatusDescription_SelectOne");

                arParams[0].Value = statusGuid;
                arParams[1].Value = languageGuid;

            }
            else
            {
                arParams[0] = new SqlParameter("@StatusGuid", SqlDbType.UniqueIdentifier);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = statusGuid;

                arParams[1] = new SqlParameter("@LanguageGuid", SqlDbType.UniqueIdentifier);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = languageGuid;

            }

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "ws_OrderStatusDescription_SelectOne",
                arParams);

        }


        


    }
}
