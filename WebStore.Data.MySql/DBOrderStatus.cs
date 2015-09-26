using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using MySql.Data.MySqlClient;
using log4net;
using Common = mojoPortal.Data.Common.WebStore;

namespace WebStore.Data
{
    /// <summary>
    /// Author:					Joe Audette
    /// Created:				2008-02-25
    /// Last Modified:		    2008-02-25
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
    public static class DBOrderStatus
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DBOrderStatus));

        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["MySqlConnectionString"];

        }


        public static int Add(
            Guid guid,
            int sort)
        {
            return Common.DBOrderStatus.Add(
                guid,
                sort);
        }

        public static bool Update(
            Guid guid,
            int sort)
        {
            return Common.DBOrderStatus.Update(guid, sort);
        }


        public static bool Delete(Guid guid)
        {
            return Common.DBOrderStatus.Delete(guid);
        }

        public static IDataReader Get(Guid guid)
        {
            return Common.DBOrderStatus.Get(guid);
        }

        public static IDataReader GetAll(Guid languageGuid)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  os.*, ");
            sqlCommand.Append("COALESCE(osd.Description, (SELECT Description + ' needs translation' FROM ws_OrderStatusDescription WHERE ws_OrderStatusDescription.StatusGuid = os.Guid LIMIT 1)) As Description ");


            sqlCommand.Append("FROM	ws_OrderStatus os ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("ws_OrderStatusDescription osd ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("os.Guid = osd.StatusGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("osd.LanguageGuid = ?LanguageGuid ");


            //sqlCommand.Append("WHERE ");
            //sqlCommand.Append("os.Guid = ?Guid ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("os.Sort ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?LanguageGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = languageGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);



        }

        public static int AddDescription(
            Guid statusGuid,
            Guid languageGuid,
            string description)
        {
            return Common.DBOrderStatus.AddDescription(
                statusGuid,
                languageGuid,
                description);
        }


        public static bool UpdateDescription(
            Guid statusGuid,
            Guid languageGuid,
            string description)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_OrderStatusDescription ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("Description = ?Description ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("StatusGuid = ?StatusGuid AND ");
            sqlCommand.Append("LanguageGuid = ?LanguageGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?StatusGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = statusGuid.ToString();

            arParams[1] = new MySqlParameter("?LanguageGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = languageGuid.ToString();

            arParams[2] = new MySqlParameter("?Description", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = description;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteDescription(
            Guid statusGuid,
            Guid languageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_OrderStatusDescription ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StatusGuid = ?StatusGuid AND ");
            sqlCommand.Append("LanguageGuid = ?LanguageGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?StatusGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = statusGuid.ToString();

            arParams[1] = new MySqlParameter("?LanguageGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = languageGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);


        }

        public static bool DeleteDescription(Guid statusGuid)
        {
            return Common.DBOrderStatus.DeleteDescription(statusGuid);
        }

        public static IDataReader GetDescription(
            Guid statusGuid,
            Guid languageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	ws_OrderStatusDescription ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StatusGuid = ?StatusGuid AND ");
            sqlCommand.Append("LanguageGuid = ?LanguageGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?StatusGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = statusGuid.ToString();

            arParams[1] = new MySqlParameter("?LanguageGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = languageGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }



    }
}
