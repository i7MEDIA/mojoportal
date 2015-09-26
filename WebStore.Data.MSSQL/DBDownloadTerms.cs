/// Author:				Joe Audette
/// Created:			2007-11-14
/// Last Modified:		2010-07-02
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
    
    public static class DBDownloadTerms
    {
        
        public static int Add(
            Guid guid,
            Guid storeGuid,
            int downloadsAllowed,
            int expireAfterDays,
            bool countAfterDownload,
            DateTime created,
            Guid createdBy,
            string createdFromIP,
            DateTime lastModified,
            Guid lastModifedBy,
            string lastModifedFromIPAddress,
            string name,
            string description)
        {

            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_FullfillDownloadTerms_Insert", 13);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@DownloadsAllowed", SqlDbType.Int, ParameterDirection.Input, downloadsAllowed);
            sph.DefineSqlParameter("@ExpireAfterDays", SqlDbType.Int, ParameterDirection.Input, expireAfterDays);
            sph.DefineSqlParameter("@CountAfterDownload", SqlDbType.Bit, ParameterDirection.Input, countAfterDownload);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, createdFromIP);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@LastModifedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModifedBy);
            sph.DefineSqlParameter("@LastModifedFromIPAddress", SqlDbType.NVarChar, 255, ParameterDirection.Input, lastModifedFromIPAddress);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }

        public static bool Update(
            Guid guid,
            int downloadsAllowed,
            int expireAfterDays,
            bool countAfterDownload,
            DateTime lastModified,
            Guid lastModifedBy,
            string lastModifedFromIPAddress,
            string name,
            string description)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_FullfillDownloadTerms_Update", 9);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@DownloadsAllowed", SqlDbType.Int, ParameterDirection.Input, downloadsAllowed);
            sph.DefineSqlParameter("@ExpireAfterDays", SqlDbType.Int, ParameterDirection.Input, expireAfterDays);
            sph.DefineSqlParameter("@CountAfterDownload", SqlDbType.Bit, ParameterDirection.Input, countAfterDownload);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@LastModifedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModifedBy);
            sph.DefineSqlParameter("@LastModifedFromIPAddress", SqlDbType.NVarChar, 255, ParameterDirection.Input, lastModifedFromIPAddress);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
            
        }

        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_FullfillDownloadTerms_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static IDataReader Get(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_FullfillDownloadTerms_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();

        }

        public static IDataReader GetAll(Guid storeGuid)
        {

            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_FullfillDownloadTerms_SelectAll", 1);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            return sph.ExecuteReader();

        }

        public static IDataReader GetFullfillDownloadTermsPage(
            Guid storeGuid,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_FullfillDownloadTerms_SelectPage", 3);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
            
        }

        

    }
}
