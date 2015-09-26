/// Author:				Joe Audette
/// Created:			2007-11-14
/// Last Modified:		2008-10-18
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
    
    public static class DBCartOrderInfo
    {
        
        
        

        public static int Add(
            Guid cartGuid,
            string customerFirstName,
            string customerLastName,
            string customerCompany,
            string customerAddressLine1,
            string customerAddressLine2,
            string customerSuburb,
            string customerCity,
            string customerPostalCode,
            string customerState,
            string customerCountry,
            string customerTelephoneDay,
            string customerTelephoneNight,
            string customerEmail,
            bool customerEmailVerified,
            string deliveryFirstName,
            string deliveryLastName,
            string deliveryCompany,
            string deliveryAddress1,
            string deliveryAddress2,
            string deliverySuburb,
            string deliveryCity,
            string deliveryPostalCode,
            string deliveryState,
            string deliveryCountry,
            string billingFirstName,
            string billingLastName,
            string billingCompany,
            string billingAddress1,
            string billingAddress2,
            string billingSuburb,
            string billingCity,
            string billingPostalCode,
            string billingState,
            string billingCountry,
            Guid cardTypeGuid,
            string cardOwner,
            string cardNumber,
            string cardExpires,
            string cardSecurityCode,
            DateTime completed,
            string completedFromIP,
            Guid taxZoneGuid)
        {

            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_CartOrderInfo_Insert", 43);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            sph.DefineSqlParameter("@CustomerFirstName", SqlDbType.NVarChar, 100, ParameterDirection.Input, customerFirstName);
            sph.DefineSqlParameter("@CustomerLastName", SqlDbType.NVarChar, 100, ParameterDirection.Input, customerLastName);
            sph.DefineSqlParameter("@CustomerCompany", SqlDbType.NVarChar, 255, ParameterDirection.Input, customerCompany);
            sph.DefineSqlParameter("@CustomerAddressLine1", SqlDbType.NVarChar, 255, ParameterDirection.Input, customerAddressLine1);
            sph.DefineSqlParameter("@CustomerAddressLine2", SqlDbType.NVarChar, 255, ParameterDirection.Input, customerAddressLine2);
            sph.DefineSqlParameter("@CustomerSuburb", SqlDbType.NVarChar, 255, ParameterDirection.Input, customerSuburb);
            sph.DefineSqlParameter("@CustomerCity", SqlDbType.NVarChar, 255, ParameterDirection.Input, customerCity);
            sph.DefineSqlParameter("@CustomerPostalCode", SqlDbType.NVarChar, 20, ParameterDirection.Input, customerPostalCode);
            sph.DefineSqlParameter("@CustomerState", SqlDbType.NVarChar, 255, ParameterDirection.Input, customerState);
            sph.DefineSqlParameter("@CustomerCountry", SqlDbType.NVarChar, 255, ParameterDirection.Input, customerCountry);
            sph.DefineSqlParameter("@CustomerTelephoneDay", SqlDbType.NVarChar, 32, ParameterDirection.Input, customerTelephoneDay);
            sph.DefineSqlParameter("@CustomerTelephoneNight", SqlDbType.NVarChar, 32, ParameterDirection.Input, customerTelephoneNight);
            sph.DefineSqlParameter("@CustomerEmail", SqlDbType.NVarChar, 96, ParameterDirection.Input, customerEmail);
            sph.DefineSqlParameter("@CustomerEmailVerified", SqlDbType.Bit, ParameterDirection.Input, customerEmailVerified);
            sph.DefineSqlParameter("@DeliveryFirstName", SqlDbType.NVarChar, 100, ParameterDirection.Input, deliveryFirstName);
            sph.DefineSqlParameter("@DeliveryLastName", SqlDbType.NVarChar, 100, ParameterDirection.Input, deliveryLastName);
            sph.DefineSqlParameter("@DeliveryCompany", SqlDbType.NVarChar, 255, ParameterDirection.Input, deliveryCompany);
            sph.DefineSqlParameter("@DeliveryAddress1", SqlDbType.NVarChar, 255, ParameterDirection.Input, deliveryAddress1);
            sph.DefineSqlParameter("@DeliveryAddress2", SqlDbType.NVarChar, 255, ParameterDirection.Input, deliveryAddress2);
            sph.DefineSqlParameter("@DeliverySuburb", SqlDbType.NVarChar, 255, ParameterDirection.Input, deliverySuburb);
            sph.DefineSqlParameter("@DeliveryCity", SqlDbType.NVarChar, 255, ParameterDirection.Input, deliveryCity);
            sph.DefineSqlParameter("@DeliveryPostalCode", SqlDbType.NVarChar, 20, ParameterDirection.Input, deliveryPostalCode);
            sph.DefineSqlParameter("@DeliveryState", SqlDbType.NVarChar, 255, ParameterDirection.Input, deliveryState);
            sph.DefineSqlParameter("@DeliveryCountry", SqlDbType.NVarChar, 255, ParameterDirection.Input, deliveryCountry);
            sph.DefineSqlParameter("@BillingFirstName", SqlDbType.NVarChar, 100, ParameterDirection.Input, billingFirstName);
            sph.DefineSqlParameter("@BillingLastName", SqlDbType.NVarChar, 100, ParameterDirection.Input, billingLastName);
            sph.DefineSqlParameter("@BillingCompany", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingCompany);
            sph.DefineSqlParameter("@BillingAddress1", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingAddress1);
            sph.DefineSqlParameter("@BillingAddress2", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingAddress2);
            sph.DefineSqlParameter("@BillingSuburb", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingSuburb);
            sph.DefineSqlParameter("@BillingCity", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingCity);
            sph.DefineSqlParameter("@BillingPostalCode", SqlDbType.NVarChar, 20, ParameterDirection.Input, billingPostalCode);
            sph.DefineSqlParameter("@BillingState", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingState);
            sph.DefineSqlParameter("@BillingCountry", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingCountry);
            sph.DefineSqlParameter("@CardTypeGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cardTypeGuid);
            sph.DefineSqlParameter("@CardOwner", SqlDbType.NVarChar, 100, ParameterDirection.Input, cardOwner);
            sph.DefineSqlParameter("@CardNumber", SqlDbType.NVarChar, 255, ParameterDirection.Input, cardNumber);
            sph.DefineSqlParameter("@CardExpires", SqlDbType.NVarChar, 6, ParameterDirection.Input, cardExpires);
            sph.DefineSqlParameter("@CardSecurityCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, cardSecurityCode);
            sph.DefineSqlParameter("@Completed", SqlDbType.DateTime, ParameterDirection.Input, completed);
            sph.DefineSqlParameter("@CompletedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, completedFromIP);
            sph.DefineSqlParameter("@TaxZoneGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, taxZoneGuid);
            
          
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

            

        }


        public static bool Update(
            Guid cartGuid,
            string customerFirstName,
            string customerLastName,
            string customerCompany,
            string customerAddressLine1,
            string customerAddressLine2,
            string customerSuburb,
            string customerCity,
            string customerPostalCode,
            string customerState,
            string customerCountry,
            string customerTelephoneDay,
            string customerTelephoneNight,
            string customerEmail,
            bool customerEmailVerified,
            string deliveryFirstName,
            string deliveryLastName,
            string deliveryCompany,
            string deliveryAddress1,
            string deliveryAddress2,
            string deliverySuburb,
            string deliveryCity,
            string deliveryPostalCode,
            string deliveryState,
            string deliveryCountry,
            string billingFirstName,
            string billingLastName,
            string billingCompany,
            string billingAddress1,
            string billingAddress2,
            string billingSuburb,
            string billingCity,
            string billingPostalCode,
            string billingState,
            string billingCountry,
            Guid cardTypeGuid,
            string cardOwner,
            string cardNumber,
            string cardExpires,
            string cardSecurityCode,
            DateTime completed,
            string completedFromIP,
            Guid taxZoneGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_CartOrderInfo_Update", 43);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            sph.DefineSqlParameter("@CustomerFirstName", SqlDbType.NVarChar, 100, ParameterDirection.Input, customerFirstName);
            sph.DefineSqlParameter("@CustomerLastName", SqlDbType.NVarChar, 100, ParameterDirection.Input, customerLastName);
            sph.DefineSqlParameter("@CustomerCompany", SqlDbType.NVarChar, 255, ParameterDirection.Input, customerCompany);
            sph.DefineSqlParameter("@CustomerAddressLine1", SqlDbType.NVarChar, 255, ParameterDirection.Input, customerAddressLine1);
            sph.DefineSqlParameter("@CustomerAddressLine2", SqlDbType.NVarChar, 255, ParameterDirection.Input, customerAddressLine2);
            sph.DefineSqlParameter("@CustomerSuburb", SqlDbType.NVarChar, 255, ParameterDirection.Input, customerSuburb);
            sph.DefineSqlParameter("@CustomerCity", SqlDbType.NVarChar, 255, ParameterDirection.Input, customerCity);
            sph.DefineSqlParameter("@CustomerPostalCode", SqlDbType.NVarChar, 20, ParameterDirection.Input, customerPostalCode);
            sph.DefineSqlParameter("@CustomerState", SqlDbType.NVarChar, 255, ParameterDirection.Input, customerState);
            sph.DefineSqlParameter("@CustomerCountry", SqlDbType.NVarChar, 255, ParameterDirection.Input, customerCountry);
            sph.DefineSqlParameter("@CustomerTelephoneDay", SqlDbType.NVarChar, 32, ParameterDirection.Input, customerTelephoneDay);
            sph.DefineSqlParameter("@CustomerTelephoneNight", SqlDbType.NVarChar, 32, ParameterDirection.Input, customerTelephoneNight);
            sph.DefineSqlParameter("@CustomerEmail", SqlDbType.NVarChar, 96, ParameterDirection.Input, customerEmail);
            sph.DefineSqlParameter("@CustomerEmailVerified", SqlDbType.Bit, ParameterDirection.Input, customerEmailVerified);
            sph.DefineSqlParameter("@DeliveryFirstName", SqlDbType.NVarChar, 100, ParameterDirection.Input, deliveryFirstName);
            sph.DefineSqlParameter("@DeliveryLastName", SqlDbType.NVarChar, 100, ParameterDirection.Input, deliveryLastName);
            sph.DefineSqlParameter("@DeliveryCompany", SqlDbType.NVarChar, 255, ParameterDirection.Input, deliveryCompany);
            sph.DefineSqlParameter("@DeliveryAddress1", SqlDbType.NVarChar, 255, ParameterDirection.Input, deliveryAddress1);
            sph.DefineSqlParameter("@DeliveryAddress2", SqlDbType.NVarChar, 255, ParameterDirection.Input, deliveryAddress2);
            sph.DefineSqlParameter("@DeliverySuburb", SqlDbType.NVarChar, 255, ParameterDirection.Input, deliverySuburb);
            sph.DefineSqlParameter("@DeliveryCity", SqlDbType.NVarChar, 255, ParameterDirection.Input, deliveryCity);
            sph.DefineSqlParameter("@DeliveryPostalCode", SqlDbType.NVarChar, 20, ParameterDirection.Input, deliveryPostalCode);
            sph.DefineSqlParameter("@DeliveryState", SqlDbType.NVarChar, 255, ParameterDirection.Input, deliveryState);
            sph.DefineSqlParameter("@DeliveryCountry", SqlDbType.NVarChar, 255, ParameterDirection.Input, deliveryCountry);
            sph.DefineSqlParameter("@BillingFirstName", SqlDbType.NVarChar, 100, ParameterDirection.Input, billingFirstName);
            sph.DefineSqlParameter("@BillingLastName", SqlDbType.NVarChar, 100, ParameterDirection.Input, billingLastName);
            sph.DefineSqlParameter("@BillingCompany", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingCompany);
            sph.DefineSqlParameter("@BillingAddress1", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingAddress1);
            sph.DefineSqlParameter("@BillingAddress2", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingAddress2);
            sph.DefineSqlParameter("@BillingSuburb", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingSuburb);
            sph.DefineSqlParameter("@BillingCity", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingCity);
            sph.DefineSqlParameter("@BillingPostalCode", SqlDbType.NVarChar, 20, ParameterDirection.Input, billingPostalCode);
            sph.DefineSqlParameter("@BillingState", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingState);
            sph.DefineSqlParameter("@BillingCountry", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingCountry);
            sph.DefineSqlParameter("@CardTypeGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cardTypeGuid);
            sph.DefineSqlParameter("@CardOwner", SqlDbType.NVarChar, 100, ParameterDirection.Input, cardOwner);
            sph.DefineSqlParameter("@CardNumber", SqlDbType.NVarChar, 255, ParameterDirection.Input, cardNumber);
            sph.DefineSqlParameter("@CardExpires", SqlDbType.NVarChar, 6, ParameterDirection.Input, cardExpires);
            sph.DefineSqlParameter("@CardSecurityCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, cardSecurityCode);
            sph.DefineSqlParameter("@Completed", SqlDbType.DateTime, ParameterDirection.Input, completed);
            sph.DefineSqlParameter("@CompletedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, completedFromIP);
            sph.DefineSqlParameter("@TaxZoneGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, taxZoneGuid);
            
            
            
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            

        }

        public static bool Delete(Guid cartGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_CartOrderInfo_Delete", 1);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

           

        }

        public static bool DeleteAnonymousByStore(Guid storeGuid, DateTime olderThan)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_CartOrderInfo_DeleteAnonymousByStore", 2);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@OlderThan", SqlDbType.DateTime, ParameterDirection.Input, olderThan);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteByStore(Guid storeGuid, DateTime olderThan)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_CartOrderInfo_DeleteByStore", 2);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@OlderThan", SqlDbType.DateTime, ParameterDirection.Input, olderThan);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static IDataReader Get(Guid cartGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_CartOrderInfo_SelectOne", 1);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            return sph.ExecuteReader();


        }


        


    }
}
