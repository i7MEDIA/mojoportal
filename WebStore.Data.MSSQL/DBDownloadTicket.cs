/// Author:				Joe Audette
/// Created:			2007-11-14
/// Last Modified:		2012-01-21
/// 
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
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

            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_FullfillDownloadTicket_Insert", 11);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            sph.DefineSqlParameter("@FullfillTermsGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fullfillTermsGuid);
            sph.DefineSqlParameter("@DownloadsAllowed", SqlDbType.Int, ParameterDirection.Input, downloadsAllowed);
            sph.DefineSqlParameter("@ExpireAfterDays", SqlDbType.Int, ParameterDirection.Input, expireAfterDays);
            sph.DefineSqlParameter("@CountAfterDownload", SqlDbType.Bit, ParameterDirection.Input, countAfterDownload);
            sph.DefineSqlParameter("@PurchaseTime", SqlDbType.DateTime, ParameterDirection.Input, purchaseTime);
            sph.DefineSqlParameter("@DownloadedCount", SqlDbType.Int, ParameterDirection.Input, downloadedCount);

            int rowsAffected = sph.ExecuteNonQuery();
          
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
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_FullfillDownloadTicket_Update", 8);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            sph.DefineSqlParameter("@FullfillTermsGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fullfillTermsGuid);
            sph.DefineSqlParameter("@DownloadsAllowed", SqlDbType.Int, ParameterDirection.Input, downloadsAllowed);
            sph.DefineSqlParameter("@ExpireAfterDays", SqlDbType.Int, ParameterDirection.Input, expireAfterDays);
            sph.DefineSqlParameter("@CountAfterDownload", SqlDbType.Bit, ParameterDirection.Input, countAfterDownload);
            sph.DefineSqlParameter("@PurchaseTime", SqlDbType.DateTime, ParameterDirection.Input, purchaseTime);
            sph.DefineSqlParameter("@DownloadedCount", SqlDbType.Int, ParameterDirection.Input, downloadedCount);

            int rowsAffected = sph.ExecuteNonQuery();

            return (rowsAffected > -1);

        }

        public static bool IncrementDownloadCount(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_FullfillDownloadTicket_IncrementDownloadCount", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            
            int rowsAffected = sph.ExecuteNonQuery();

            return (rowsAffected > -1);

        }

        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_FullfillDownloadTicket_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();

            return (rowsAffected > -1);

        }

        public static bool DeleteByOrder(Guid orderGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_FullfillDownloadTicket_DeleteByOrder", 1);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            int rowsAffected = sph.ExecuteNonQuery();

            return (rowsAffected > -1);

        }

        public static IDataReader Get(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_FullfillDownloadTicket_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();

        }

        public static IDataReader GetDownloadHistory(Guid ticketGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_FullfillDownloadHistory_SelectByTicket", 1);
            sph.DefineSqlParameter("@TicketGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, ticketGuid);
            return sph.ExecuteReader();

        }

        public static IDataReader GetByOrder(Guid orderGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_FullfillDownloadTicket_SelectByOrder", 1);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            return sph.ExecuteReader();

          
        }

        public static IDataReader GetPageByStore(
            Guid storeGuid,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_FullfillDownloadTicket_SelectPage", 3);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);

            return sph.ExecuteReader();

           
        }

        public static int AddDownloadHistory(
            Guid guid,
            Guid ticketGuid,
            DateTime uTCTimestamp,
            string iPAddress)
        {

            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_FullfillDownloadHistory_Insert", 4);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@TicketGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, ticketGuid);
            sph.DefineSqlParameter("@UTCTimestamp", SqlDbType.DateTime, ParameterDirection.Input, uTCTimestamp);
            sph.DefineSqlParameter("@IPAddress", SqlDbType.NVarChar, 255, ParameterDirection.Input, iPAddress);
            int rowsAffected = sph.ExecuteNonQuery();

            return rowsAffected;

            

        }


        public static bool MoveOrder(
             Guid orderGuid,
             Guid newUserGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_FullfillDownloadTicket_MoveOrder", 2);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, newUserGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }



    }
}
