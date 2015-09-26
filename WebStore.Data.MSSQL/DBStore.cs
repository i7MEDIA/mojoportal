/// Author:				Joe Audette
/// Created:			2007-11-14
/// Last Modified:		2008-11-13
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
    
    public static class DBStore
    {
        

        
        public static int Add(
            Guid guid,
            Guid siteGuid,
            int moduleId,
            string name,
            string description,
            string ownerName,
            string ownerEmail,
            string salesEmail,
            string supportEmail,
            string emailFrom,
            string orderBccEmail,
            string phone,
            string fax,
            string address,
            string city,
            Guid zoneGuid,
            string postalCode,
            Guid countryGuid,
            bool isClosed,
            string closedMessage,
            DateTime created,
            Guid createdBy)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Store_Insert", 22);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Description", SqlDbType.NText, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@OwnerName", SqlDbType.NVarChar, 255, ParameterDirection.Input, ownerName);
            sph.DefineSqlParameter("@OwnerEmail", SqlDbType.NVarChar, 100, ParameterDirection.Input, ownerEmail);
            sph.DefineSqlParameter("@SalesEmail", SqlDbType.NVarChar, 100, ParameterDirection.Input, salesEmail);
            sph.DefineSqlParameter("@SupportEmail", SqlDbType.NVarChar, 100, ParameterDirection.Input, supportEmail);
            sph.DefineSqlParameter("@EmailFrom", SqlDbType.NVarChar, 100, ParameterDirection.Input, emailFrom);
            sph.DefineSqlParameter("@OrderBCCEmail", SqlDbType.NVarChar, 100, ParameterDirection.Input, orderBccEmail);
            sph.DefineSqlParameter("@Phone", SqlDbType.NVarChar, 32, ParameterDirection.Input, phone);
            sph.DefineSqlParameter("@Fax", SqlDbType.NVarChar, 32, ParameterDirection.Input, fax);
            sph.DefineSqlParameter("@Address", SqlDbType.NVarChar, 255, ParameterDirection.Input, address);
            sph.DefineSqlParameter("@City", SqlDbType.NVarChar, 255, ParameterDirection.Input, city);
            sph.DefineSqlParameter("@ZoneGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, zoneGuid);
            sph.DefineSqlParameter("@PostalCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, postalCode);
            sph.DefineSqlParameter("@CountryGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, countryGuid);
            sph.DefineSqlParameter("@IsClosed", SqlDbType.Bit, ParameterDirection.Input, isClosed);
            sph.DefineSqlParameter("@ClosedMessage", SqlDbType.NText, ParameterDirection.Input, closedMessage);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

            

        }


        public static bool Update(
            Guid guid,
            string name,
            string description,
            string ownerName,
            string ownerEmail,
            string salesEmail,
            string supportEmail,
            string emailFrom,
            string orderBccEmail,
            string phone,
            string fax,
            string address,
            string city,
            Guid zoneGuid,
            string postalCode,
            Guid countryGuid,
            bool isClosed,
            string closedMessage)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Store_Update", 18);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Description", SqlDbType.NText, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@OwnerName", SqlDbType.NVarChar, 255, ParameterDirection.Input, ownerName);
            sph.DefineSqlParameter("@OwnerEmail", SqlDbType.NVarChar, 100, ParameterDirection.Input, ownerEmail);
            sph.DefineSqlParameter("@SalesEmail", SqlDbType.NVarChar, 100, ParameterDirection.Input, salesEmail);
            sph.DefineSqlParameter("@SupportEmail", SqlDbType.NVarChar, 100, ParameterDirection.Input, supportEmail);
            sph.DefineSqlParameter("@EmailFrom", SqlDbType.NVarChar, 100, ParameterDirection.Input, emailFrom);
            sph.DefineSqlParameter("@OrderBCCEmail", SqlDbType.NVarChar, 100, ParameterDirection.Input, orderBccEmail);
            sph.DefineSqlParameter("@Phone", SqlDbType.NVarChar, 32, ParameterDirection.Input, phone);
            sph.DefineSqlParameter("@Fax", SqlDbType.NVarChar, 32, ParameterDirection.Input, fax);
            sph.DefineSqlParameter("@Address", SqlDbType.NVarChar, 255, ParameterDirection.Input, address);
            sph.DefineSqlParameter("@City", SqlDbType.NVarChar, 255, ParameterDirection.Input, city);
            sph.DefineSqlParameter("@ZoneGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, zoneGuid);
            sph.DefineSqlParameter("@PostalCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, postalCode);
            sph.DefineSqlParameter("@CountryGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, countryGuid);
            sph.DefineSqlParameter("@IsClosed", SqlDbType.Bit, ParameterDirection.Input, isClosed);
            sph.DefineSqlParameter("@ClosedMessage", SqlDbType.NText, ParameterDirection.Input, closedMessage);
            
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Store_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteByModule(int moduleId)
        {
            // TODO: need to update the proc, it does not delete friendly urls for offers
            //and it does not delete offers
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Store_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(int siteId)
        {
            // TODO: need to update the proc, it does not delete friendly urls for offers
            //and it does not delete offers
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Store_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }

        public static IDataReader Get(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Store_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();

        }

        public static IDataReader Get(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Store_SelectOneByModuleID", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            return sph.ExecuteReader();

            

        }


        



    }
}
