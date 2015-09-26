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
   
    public static class DBOfferAvailability
    {
        

        public static int Add(
            Guid guid,
            Guid offerGuid,
            DateTime beginUtc,
            DateTime endUtc,
            bool requiresOfferCode,
            string offerCode,
            int maxAllowedPerCustomer,
            DateTime created,
            Guid createdBy,
            string createdFromIP,
            DateTime lastModified,
            Guid lastModifedBy,
            string lastModifedFromIP)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_OfferAvailability_Insert", 13);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
            sph.DefineSqlParameter("@BeginUTC", SqlDbType.DateTime, ParameterDirection.Input, beginUtc);
            sph.DefineSqlParameter("@EndUTC", SqlDbType.DateTime, ParameterDirection.Input, endUtc);
            sph.DefineSqlParameter("@RequiresOfferCode", SqlDbType.Bit, ParameterDirection.Input, requiresOfferCode);
            sph.DefineSqlParameter("@OfferCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, offerCode);
            sph.DefineSqlParameter("@MaxAllowedPerCustomer", SqlDbType.Int, ParameterDirection.Input, maxAllowedPerCustomer);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, createdFromIP);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@LastModifedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModifedBy);
            sph.DefineSqlParameter("@LastModifedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, lastModifedFromIP);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

            

        }

        public static bool Update(
            Guid guid,
            DateTime beginUtc,
            DateTime endUtc,
            bool requiresOfferCode,
            string offerCode,
            int maxAllowedPerCustomer,
            DateTime lastModified,
            Guid lastModifedBy,
            string lastModifedFromIP)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_OfferAvailability_Update", 9);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@BeginUTC", SqlDbType.DateTime, ParameterDirection.Input, beginUtc);
            sph.DefineSqlParameter("@EndUTC", SqlDbType.DateTime, ParameterDirection.Input, endUtc);
            sph.DefineSqlParameter("@RequiresOfferCode", SqlDbType.Bit, ParameterDirection.Input, requiresOfferCode);
            sph.DefineSqlParameter("@OfferCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, offerCode);
            sph.DefineSqlParameter("@MaxAllowedPerCustomer", SqlDbType.Int, ParameterDirection.Input, maxAllowedPerCustomer);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@LastModifedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModifedBy);
            sph.DefineSqlParameter("@LastModifedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, lastModifedFromIP);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            

        }

        public static bool Delete(
            Guid guid,
            Guid deletedBy,
            DateTime deletedTime,
            string deletedFromIP)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_OfferAvailability_Update", 5);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, true);
            sph.DefineSqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, deletedBy);
            sph.DefineSqlParameter("@DeletedTime", SqlDbType.DateTime, ParameterDirection.Input, deletedTime);
            sph.DefineSqlParameter("@DeletedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, deletedFromIP);
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            

        }

        public static IDataReader Get(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_OfferAvailability_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();

        }

        public static IDataReader GetByOffer(Guid offerGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_OfferAvailability_SelectByOffer", 1);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
            return sph.ExecuteReader();

        }


        public static int AddHistory(
            Guid guid,
            Guid availabilityGuid,
            Guid offerGuid,
            DateTime beginUtc,
            DateTime endUtc,
            bool requiresOfferCode,
            string offerCode,
            int maxAllowedPerCustomer,
            DateTime created,
            Guid createdBy,
            string createdFromIP,
            bool isDeleted,
            Guid deletedBy,
            DateTime deletedTime,
            string deletedFromIP,
            DateTime lastModified,
            Guid lastModifedBy,
            string lastModifedFromIP,
            DateTime logTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_OfferAvailabilityHistory_Insert", 19);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@AvailabilityGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, availabilityGuid);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
            sph.DefineSqlParameter("@BeginUTC", SqlDbType.DateTime, ParameterDirection.Input, beginUtc);
            sph.DefineSqlParameter("@EndUTC", SqlDbType.DateTime, ParameterDirection.Input, endUtc);
            sph.DefineSqlParameter("@RequiresOfferCode", SqlDbType.Bit, ParameterDirection.Input, requiresOfferCode);
            sph.DefineSqlParameter("@OfferCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, offerCode);
            sph.DefineSqlParameter("@MaxAllowedPerCustomer", SqlDbType.Int, ParameterDirection.Input, maxAllowedPerCustomer);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, createdFromIP);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            sph.DefineSqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, deletedBy);
            sph.DefineSqlParameter("@DeletedTime", SqlDbType.DateTime, ParameterDirection.Input, deletedTime);
            sph.DefineSqlParameter("@DeletedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, deletedFromIP);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@LastModifedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModifedBy);
            sph.DefineSqlParameter("@LastModifedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, lastModifedFromIP);
            sph.DefineSqlParameter("@LogTime", SqlDbType.DateTime, ParameterDirection.Input, logTime);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

            
        }

        

    }
}
