using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace mojoPortal.Data
{
    /// <summary>
    /// Author:				
    /// Created:			2007-11-14
    /// Last Modified:		2008-06-25
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
    public static class DBTaxRate
    {
        
        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid geoZoneGuid,
            Guid taxClassGuid,
            int priority,
            decimal rate,
            string description,
            DateTime created,
            Guid createdBy,
            DateTime lastModified,
            Guid modifiedBy)
        {

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TaxRate_Insert", 11);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@GeoZoneGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, geoZoneGuid);
            sph.DefineSqlParameter("@TaxClassGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, taxClassGuid);
            sph.DefineSqlParameter("@Priority", SqlDbType.Int, ParameterDirection.Input, priority);
            sph.DefineSqlParameter("@Rate", SqlDbType.Decimal, ParameterDirection.Input, rate);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, 255, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, modifiedBy);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;


        }

        public static bool Update(
            Guid guid,
            Guid geoZoneGuid,
            Guid taxClassGuid,
            int priority,
            decimal rate,
            string description,
            DateTime lastModified,
            Guid modifiedBy)
        {

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TaxRate_Update", 8);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@GeoZoneGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, geoZoneGuid);
            sph.DefineSqlParameter("@TaxClassGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, taxClassGuid);
            sph.DefineSqlParameter("@Priority", SqlDbType.Int, ParameterDirection.Input, priority);
            sph.DefineSqlParameter("@Rate", SqlDbType.Decimal, ParameterDirection.Input, rate);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, 255, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, modifiedBy);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            

        }

        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TaxRate_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static IDataReader GetOne(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_TaxRate_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();

            

        }

        public static IDataReader GetTaxRates(
            Guid siteGuid,
            Guid geoZoneGuid)
        {

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_TaxRate_SelectAll", 2);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@GeoZoneGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, geoZoneGuid);
            return sph.ExecuteReader();

            
        }

        /// <summary>
        /// Inserts a row in the mp_TaxRateHistory table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="taxRateGuid"> taxRateGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="geoZoneGuid"> geoZoneGuid </param>
        /// <param name="taxClassGuid"> taxClassGuid </param>
        /// <param name="priority"> priority </param>
        /// <param name="rate"> rate </param>
        /// <param name="description"> description </param>
        /// <param name="created"> created </param>
        /// <param name="createdBy"> createdBy </param>
        /// <param name="lastModified"> lastModified </param>
        /// <param name="modifiedBy"> modifiedBy </param>
        /// <param name="logTime"> logTime </param>
        /// <returns>int</returns>
        public static int AddHistory(
            Guid guid,
            Guid taxRateGuid,
            Guid siteGuid,
            Guid geoZoneGuid,
            Guid taxClassGuid,
            int priority,
            decimal rate,
            string description,
            DateTime created,
            Guid createdBy,
            DateTime lastModified,
            Guid modifiedBy,
            DateTime logTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TaxRateHistory_Insert", 13);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@TaxRateGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, taxRateGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@GeoZoneGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, geoZoneGuid);
            sph.DefineSqlParameter("@TaxClassGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, taxClassGuid);
            sph.DefineSqlParameter("@Priority", SqlDbType.Int, ParameterDirection.Input, priority);
            sph.DefineSqlParameter("@Rate", SqlDbType.Decimal, ParameterDirection.Input, rate);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, 255, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, modifiedBy);
            sph.DefineSqlParameter("@LogTime", SqlDbType.DateTime, ParameterDirection.Input, logTime);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }


    }
}
