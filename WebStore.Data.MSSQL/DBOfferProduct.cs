/// Author:				Joe Audette
/// Created:			2007-11-14
/// Last Modified:		2008-10-16
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
    
    public static class DBOfferProduct
    {
       
        

        public static int Add(
            Guid guid,
            Guid offerGuid,
            Guid productGuid,
            byte fullfillType,
            Guid fullFillTermsGuid,
            int quantity,
            int sortOrder,
            DateTime created,
            Guid createdBy,
            string createdFromIP,
            DateTime lastModified,
            Guid lastModifiedBy,
            string lastModifiedFromIP)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_OfferProduct_Insert", 13);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            sph.DefineSqlParameter("@FullfillType", SqlDbType.TinyInt, ParameterDirection.Input, fullfillType);
            sph.DefineSqlParameter("@FullFillTermsGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fullFillTermsGuid);
            sph.DefineSqlParameter("@Quantity", SqlDbType.Int, ParameterDirection.Input, quantity);
            sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, createdFromIP);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@LastModifiedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModifiedBy);
            sph.DefineSqlParameter("@LastModifiedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, lastModifiedFromIP);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }


        public static bool Update(
            Guid guid,
            byte fullfillType,
            Guid fullFillTermsGuid,
            int quantity,
            int sortOrder,
            DateTime lastModified,
            Guid lastModifiedBy,
            string lastModifiedFromIP)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_OfferProduct_Update", 8);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@FullfillType", SqlDbType.TinyInt, ParameterDirection.Input, fullfillType);
            sph.DefineSqlParameter("@FullFillTermsGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fullFillTermsGuid);
            sph.DefineSqlParameter("@Quantity", SqlDbType.Int, ParameterDirection.Input, quantity);
            sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@LastModifiedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModifiedBy);
            sph.DefineSqlParameter("@LastModifiedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, lastModifiedFromIP);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }


        public static bool Delete(
            Guid guid,
            Guid deletedBy,
            string deletedFromIP,
            DateTime deletedTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_OfferProduct_Delete", 4);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, deletedBy);
            sph.DefineSqlParameter("@DeletedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, deletedFromIP);
            sph.DefineSqlParameter("@DeletedTime", SqlDbType.DateTime, ParameterDirection.Input, deletedTime);
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            

        }

        public static bool DeleteByProduct(
            Guid productGuid,
            Guid deletedBy,
            string deletedFromIP,
            DateTime deletedTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_OfferProduct_DeleteByProduct", 4);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            sph.DefineSqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, deletedBy);
            sph.DefineSqlParameter("@DeletedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, deletedFromIP);
            sph.DefineSqlParameter("@DeletedTime", SqlDbType.DateTime, ParameterDirection.Input, deletedTime);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            

        }

        public static IDataReader Get(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_OfferProduct_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();

        
        }

        /// <summary>
        /// Gets a count of rows in the ws_Offer table.
        /// </summary>
        public static int GetCountByOffer(Guid offerGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_OfferProduct_CountByOffer", 1);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);

            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetByOffer(Guid offerGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_OfferProduct_SelectByOffer", 1);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
            
            return sph.ExecuteReader();

           
        }



        
    }
}
