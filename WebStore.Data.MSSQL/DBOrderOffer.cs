/// Author:				Joe Audette
/// Created:			2007-11-15
/// Last Modified:		2009-02-02
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
    
    public static class DBOrderOffer
    {
       
        
        
        public static int Add(
            Guid itemGuid,
            Guid orderGuid,
            Guid offerGuid,
            Guid taxClassGuid,
            decimal offerPrice,
            DateTime addedToCart,
            int quantity)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_OrderOffers_Insert", 7);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
            sph.DefineSqlParameter("@TaxClassGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, taxClassGuid);
            sph.DefineSqlParameter("@OfferPrice", SqlDbType.Decimal, ParameterDirection.Input, offerPrice);
            sph.DefineSqlParameter("@AddedToCart", SqlDbType.DateTime, ParameterDirection.Input, addedToCart);
            sph.DefineSqlParameter("@Quantity", SqlDbType.Int, ParameterDirection.Input, quantity);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }

        public static bool Update(
            Guid itemGuid,
            Guid orderGuid,
            Guid offerGuid,
            Guid taxClassGuid,
            decimal offerPrice,
            DateTime addedToCart,
            int quantity)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_OrderOffers_Update", 7);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
            sph.DefineSqlParameter("@TaxClassGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, taxClassGuid);
            sph.DefineSqlParameter("@OfferPrice", SqlDbType.Decimal, ParameterDirection.Input, offerPrice);
            sph.DefineSqlParameter("@AddedToCart", SqlDbType.DateTime, ParameterDirection.Input, addedToCart);
            sph.DefineSqlParameter("@Quantity", SqlDbType.Int, ParameterDirection.Input, quantity);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static IDataReader Get(Guid itemGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_OrderOffers_SelectOne", 1);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            return sph.ExecuteReader();

        }

        public static IDataReader GetByOrder(Guid orderGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_OrderOffers_SelectByOrder", 1);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Deletes a row from the ws_OrderOffers table. Returns true if row deleted.
        /// </summary>
        /// <param name="itemGuid"> itemGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByOrder(Guid orderGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_OrderOffers_DeleteByOrder", 1);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        

    }
}
