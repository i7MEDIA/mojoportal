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
using System.Globalization;
using MySql.Data.MySqlClient;
using mojoPortal.Data;

namespace WebStore.Data
{
    public static class DBDownloadTicket
    {
        public static int Add(
            Guid guid,
            Guid storeGuid,
            Guid orderGuid,
            Guid userGuid,
            Guid productGuid,
            Guid fullfillTermsGuid,
            int downloadsAllowed,
            int expireAfterDays,
            bool countAfterDownload,
            DateTime purchaseTime,
            int downloadedCount)
        {
            #region Bit Conversion

            int intCountAfterDownload;
            if (countAfterDownload)
            {
                intCountAfterDownload = 1;
            }
            else
            {
                intCountAfterDownload = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO ws_FullfillDownloadTicket (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("StoreGuid, ");
            sqlCommand.Append("OrderGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("ProductGuid, ");
            sqlCommand.Append("FullfillTermsGuid, ");
            sqlCommand.Append("DownloadsAllowed, ");
            sqlCommand.Append("ExpireAfterDays, ");
            sqlCommand.Append("CountAfterDownload, ");
            sqlCommand.Append("PurchaseTime, ");
            sqlCommand.Append("DownloadedCount )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?StoreGuid, ");
            sqlCommand.Append("?OrderGuid, ");
            sqlCommand.Append("?UserGuid, ");
            sqlCommand.Append("?ProductGuid, ");
            sqlCommand.Append("?FullfillTermsGuid, ");
            sqlCommand.Append("?DownloadsAllowed, ");
            sqlCommand.Append("?ExpireAfterDays, ");
            sqlCommand.Append("?CountAfterDownload, ");
            sqlCommand.Append("?PurchaseTime, ");
            sqlCommand.Append("?DownloadedCount )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[11];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = storeGuid.ToString();

            arParams[2] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = orderGuid.ToString();

            arParams[3] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = productGuid.ToString();

            arParams[5] = new MySqlParameter("?FullfillTermsGuid", MySqlDbType.VarChar, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = fullfillTermsGuid.ToString();

            arParams[6] = new MySqlParameter("?DownloadsAllowed", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = downloadsAllowed;

            arParams[7] = new MySqlParameter("?ExpireAfterDays", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = expireAfterDays;

            arParams[8] = new MySqlParameter("?CountAfterDownload", MySqlDbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = intCountAfterDownload;

            arParams[9] = new MySqlParameter("?PurchaseTime", MySqlDbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = purchaseTime;

            arParams[10] = new MySqlParameter("?DownloadedCount", MySqlDbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = downloadedCount;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

        }


        public static bool Update(
            Guid guid,
            Guid productGuid,
            Guid fullfillTermsGuid,
            int downloadsAllowed,
            int expireAfterDays,
            bool countAfterDownload,
            DateTime purchaseTime,
            int downloadedCount)
        {
            #region Bit Conversion

            int intCountAfterDownload;
            if (countAfterDownload)
            {
                intCountAfterDownload = 1;
            }
            else
            {
                intCountAfterDownload = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_FullfillDownloadTicket ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("ProductGuid = ?ProductGuid, ");
            sqlCommand.Append("FullfillTermsGuid = ?FullfillTermsGuid, ");
            sqlCommand.Append("DownloadsAllowed = ?DownloadsAllowed, ");
            sqlCommand.Append("ExpireAfterDays = ?ExpireAfterDays, ");
            sqlCommand.Append("CountAfterDownload = ?CountAfterDownload, ");
            sqlCommand.Append("PurchaseTime = ?PurchaseTime, ");
            sqlCommand.Append("DownloadedCount = ?DownloadedCount ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[8];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = productGuid.ToString();

            arParams[2] = new MySqlParameter("?FullfillTermsGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = fullfillTermsGuid.ToString();

            arParams[3] = new MySqlParameter("?DownloadsAllowed", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = downloadsAllowed;

            arParams[4] = new MySqlParameter("?ExpireAfterDays", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = expireAfterDays;

            arParams[5] = new MySqlParameter("?CountAfterDownload", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intCountAfterDownload;

            arParams[6] = new MySqlParameter("?PurchaseTime", MySqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = purchaseTime;

            arParams[7] = new MySqlParameter("?DownloadedCount", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = downloadedCount;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
            
            
        }


        public static bool IncrementDownloadCount(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_FullfillDownloadTicket ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("DownloadedCount = DownloadedCount + 1 ");

            sqlCommand.Append("WHERE  ");
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

            return (rowsAffected > -1);

        }

        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_FullfillDownloadTicket ");
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
            sqlCommand.Append("DELETE FROM ws_FullfillDownloadTicket ");
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
            sqlCommand.Append("FROM	ws_FullfillDownloadTicket ");
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

        public static IDataReader GetDownloadHistory(Guid ticketGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	ws_FullfillDownloadHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("TicketGuid = ?TicketGuid ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("UTCTimestamp ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?TicketGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = ticketGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }


        public static IDataReader GetByOrder(Guid orderGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  fdt.*, ");
            sqlCommand.Append("pd.Name ");

            sqlCommand.Append("FROM	ws_FullfillDownloadTicket fdt ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("ws_Product pd ");

            sqlCommand.Append("ON ");
            sqlCommand.Append("fdt.ProductGuid = pd.Guid ");
            
            sqlCommand.Append(" ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("fdt.OrderGuid = ?OrderGuid ");
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


        /// <summary>
        /// Gets a count of rows in the ws_FullfillDownloadTicket table.
        /// </summary>
        public static int GetCount(Guid storeGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	ws_FullfillDownloadTicket ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = ?StoreGuid ");
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


        public static IDataReader GetPageByStore(
            Guid storeGuid,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            int totalPages = 1;
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
            sqlCommand.Append("SELECT	*, ");
            sqlCommand.Append(totalPages.ToString(CultureInfo.InvariantCulture) + " As TotalPages ");
            sqlCommand.Append("FROM	ws_FullfillDownloadTicket  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = ?StoreGuid ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append(" PurchaseTime  ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", ?PageSize ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }


        public static int AddDownloadHistory(
            Guid guid,
            Guid ticketGuid,
            DateTime uTCTimestamp,
            string iPAddress)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO ws_FullfillDownloadHistory (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("TicketGuid, ");
            sqlCommand.Append("UTCTimestamp, ");
            sqlCommand.Append("IPAddress )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?TicketGuid, ");
            sqlCommand.Append("?UTCTimestamp, ");
            sqlCommand.Append("?IPAddress )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?TicketGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = ticketGuid.ToString();

            arParams[2] = new MySqlParameter("?UTCTimestamp", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = uTCTimestamp;

            arParams[3] = new MySqlParameter("?IPAddress", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = iPAddress;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

            

        }

        public static bool MoveOrder(
            Guid orderGuid,
            Guid newUserGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_FullfillDownloadTicket ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("UserGuid = ?UserGuid ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("OrderGuid = ?OrderGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = newUserGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }



    }
}
