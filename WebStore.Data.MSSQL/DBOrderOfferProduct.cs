/// Author:				Joe Audette
/// Created:			2007-11-15
/// Last Modified:		2008-07-18
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

            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_OrderOfferProduct_Insert", 7);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            sph.DefineSqlParameter("@FullfillType", SqlDbType.TinyInt, ParameterDirection.Input, fullfillType);
            sph.DefineSqlParameter("@FullfillTermsGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fullfillTermsGuid);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            int rowsAffected = sph.ExecuteNonQuery();
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
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_OrderOfferProduct_Update", 7);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            sph.DefineSqlParameter("@FullfillType", SqlDbType.TinyInt, ParameterDirection.Input, fullfillType);
            sph.DefineSqlParameter("@FullfillTermsGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fullfillTermsGuid);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            

        }

        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_OrderOfferProduct_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            

        }

        public static IDataReader Get(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_OrderOfferProduct_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();

           

        }

        /// <summary>
        /// Deletes a row from the ws_OrderOfferProduct table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByOrder(Guid orderGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_OrderOfferProduct_DeleteByOrder", 1);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }
        


    }
}
