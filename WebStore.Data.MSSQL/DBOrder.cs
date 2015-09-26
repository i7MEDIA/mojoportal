/// Author:				Joe Audette
/// Created:			2007-11-15
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
    
    public static class DBOrder
    {
        
       
        /// <summary>
        /// Inserts a row in the ws_Order table. Returns rows affected count.
        /// </summary>
        /// <param name="orderGuid"> orderGuid </param>
        /// <param name="orderNo"> orderNo </param>
        /// <param name="storeGuid"> storeGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="customerFirstName"> customerFirstName </param>
        /// <param name="customerLastName"> customerLastName </param>
        /// <param name="customerCompany"> customerCompany </param>
        /// <param name="customerAddressLine1"> customerAddressLine1 </param>
        /// <param name="customerAddressLine2"> customerAddressLine2 </param>
        /// <param name="customerSuburb"> customerSuburb </param>
        /// <param name="customerCity"> customerCity </param>
        /// <param name="customerPostalCode"> customerPostalCode </param>
        /// <param name="customerState"> customerState </param>
        /// <param name="customerCountry"> customerCountry </param>
        /// <param name="customerTelephoneDay"> customerTelephoneDay </param>
        /// <param name="customerTelephoneNight"> customerTelephoneNight </param>
        /// <param name="customerEmail"> customerEmail </param>
        /// <param name="customerEmailVerified"> customerEmailVerified </param>
        /// <param name="deliveryFirstName"> deliveryFirstName </param>
        /// <param name="deliveryLastName"> deliveryLastName </param>
        /// <param name="deliveryCompany"> deliveryCompany </param>
        /// <param name="deliveryAddress1"> deliveryAddress1 </param>
        /// <param name="deliveryAddress2"> deliveryAddress2 </param>
        /// <param name="deliverySuburb"> deliverySuburb </param>
        /// <param name="deliveryCity"> deliveryCity </param>
        /// <param name="deliveryPostalCode"> deliveryPostalCode </param>
        /// <param name="deliveryState"> deliveryState </param>
        /// <param name="deliveryCountry"> deliveryCountry </param>
        /// <param name="billingFirstName"> billingFirstName </param>
        /// <param name="billingLastName"> billingLastName </param>
        /// <param name="billingCompany"> billingCompany </param>
        /// <param name="billingAddress1"> billingAddress1 </param>
        /// <param name="billingAddress2"> billingAddress2 </param>
        /// <param name="billingSuburb"> billingSuburb </param>
        /// <param name="billingCity"> billingCity </param>
        /// <param name="billingPostalCode"> billingPostalCode </param>
        /// <param name="billingState"> billingState </param>
        /// <param name="billingCountry"> billingCountry </param>
        /// <param name="cardTypeGuid"> cardTypeGuid </param>
        /// <param name="cardOwner"> cardOwner </param>
        /// <param name="cardNumber"> cardNumber </param>
        /// <param name="cardExpires"> cardExpires </param>
        /// <param name="cardSecurityCode"> cardSecurityCode </param>
        /// <param name="currencyGuid"> currencyGuid </param>
        /// <param name="currencyValue"> currencyValue </param>
        /// <param name="subTotal"> subTotal </param>
        /// <param name="taxTotal"> taxTotal </param>
        /// <param name="shippingTotal"> shippingTotal </param>
        /// <param name="orderTotal"> orderTotal </param>
        /// <param name="created"> created </param>
        /// <param name="createdFromIP"> createdFromIP </param>
        /// <param name="completed"> completed </param>
        /// <param name="completedFromIP"> completedFromIP </param>
        /// <param name="lastModified"> lastModified </param>
        /// <param name="lastUserActivity"> lastUserActivity </param>
        /// <param name="statusGuid"> statusGuid </param>
        /// <param name="gatewayTransID"> gatewayTransID </param>
        /// <param name="gatewayRawResponse"> gatewayRawResponse </param>
        /// <param name="gatewayAuthCode"> gatewayAuthCode </param>
        /// <param name="taxZoneGuid"> taxZoneGuid </param>
        /// <returns>int</returns>
        public static int Create(
            Guid orderGuid,
            int orderNo,
            Guid storeGuid,
            Guid userGuid,
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
            decimal subTotal,
            decimal taxTotal,
            decimal shippingTotal,
            decimal orderTotal,
            DateTime created,
            string createdFromIP,
            DateTime completed,
            string completedFromIP,
            DateTime lastModified,
            DateTime lastUserActivity,
            Guid statusGuid,
            string gatewayTransID,
            string gatewayRawResponse,
            string gatewayAuthCode,
            Guid taxZoneGuid,
            decimal discount,
            string discountCodesCsv,
            string customData,
            Guid clerkGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Order_Insert", 62);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            sph.DefineSqlParameter("@OrderNo", SqlDbType.Int, ParameterDirection.Input, orderNo);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
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
            
            sph.DefineSqlParameter("@SubTotal", SqlDbType.Decimal, ParameterDirection.Input, subTotal);
            sph.DefineSqlParameter("@TaxTotal", SqlDbType.Decimal, ParameterDirection.Input, taxTotal);
            sph.DefineSqlParameter("@ShippingTotal", SqlDbType.Decimal, ParameterDirection.Input, shippingTotal);
            sph.DefineSqlParameter("@OrderTotal", SqlDbType.Decimal, ParameterDirection.Input, orderTotal);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            sph.DefineSqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, createdFromIP);
            sph.DefineSqlParameter("@Completed", SqlDbType.DateTime, ParameterDirection.Input, completed);
            sph.DefineSqlParameter("@CompletedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, completedFromIP);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@LastUserActivity", SqlDbType.DateTime, ParameterDirection.Input, lastUserActivity);
            sph.DefineSqlParameter("@StatusGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, statusGuid);
            sph.DefineSqlParameter("@GatewayTransID", SqlDbType.NVarChar, 255, ParameterDirection.Input, gatewayTransID);
            sph.DefineSqlParameter("@GatewayRawResponse", SqlDbType.NVarChar, -1, ParameterDirection.Input, gatewayRawResponse);
            sph.DefineSqlParameter("@GatewayAuthCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, gatewayAuthCode);
            sph.DefineSqlParameter("@TaxZoneGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, taxZoneGuid);
            sph.DefineSqlParameter("@CustomData", SqlDbType.NVarChar, -1, ParameterDirection.Input, customData);
            sph.DefineSqlParameter("@DiscountCodesCsv", SqlDbType.NVarChar, -1, ParameterDirection.Input, discountCodesCsv);
            sph.DefineSqlParameter("@Discount", SqlDbType.Decimal, ParameterDirection.Input, discount);
            sph.DefineSqlParameter("@ClerkGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, clerkGuid);
  
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the ws_Order table. Returns true if row updated.
        /// </summary>
        /// <param name="orderGuid"> orderGuid </param>
        /// <param name="orderNo"> orderNo </param>
        /// <param name="storeGuid"> storeGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="customerFirstName"> customerFirstName </param>
        /// <param name="customerLastName"> customerLastName </param>
        /// <param name="customerCompany"> customerCompany </param>
        /// <param name="customerAddressLine1"> customerAddressLine1 </param>
        /// <param name="customerAddressLine2"> customerAddressLine2 </param>
        /// <param name="customerSuburb"> customerSuburb </param>
        /// <param name="customerCity"> customerCity </param>
        /// <param name="customerPostalCode"> customerPostalCode </param>
        /// <param name="customerState"> customerState </param>
        /// <param name="customerCountry"> customerCountry </param>
        /// <param name="customerTelephoneDay"> customerTelephoneDay </param>
        /// <param name="customerTelephoneNight"> customerTelephoneNight </param>
        /// <param name="customerEmail"> customerEmail </param>
        /// <param name="customerEmailVerified"> customerEmailVerified </param>
        /// <param name="deliveryFirstName"> deliveryFirstName </param>
        /// <param name="deliveryLastName"> deliveryLastName </param>
        /// <param name="deliveryCompany"> deliveryCompany </param>
        /// <param name="deliveryAddress1"> deliveryAddress1 </param>
        /// <param name="deliveryAddress2"> deliveryAddress2 </param>
        /// <param name="deliverySuburb"> deliverySuburb </param>
        /// <param name="deliveryCity"> deliveryCity </param>
        /// <param name="deliveryPostalCode"> deliveryPostalCode </param>
        /// <param name="deliveryState"> deliveryState </param>
        /// <param name="deliveryCountry"> deliveryCountry </param>
        /// <param name="billingFirstName"> billingFirstName </param>
        /// <param name="billingLastName"> billingLastName </param>
        /// <param name="billingCompany"> billingCompany </param>
        /// <param name="billingAddress1"> billingAddress1 </param>
        /// <param name="billingAddress2"> billingAddress2 </param>
        /// <param name="billingSuburb"> billingSuburb </param>
        /// <param name="billingCity"> billingCity </param>
        /// <param name="billingPostalCode"> billingPostalCode </param>
        /// <param name="billingState"> billingState </param>
        /// <param name="billingCountry"> billingCountry </param>
        /// <param name="cardTypeGuid"> cardTypeGuid </param>
        /// <param name="cardOwner"> cardOwner </param>
        /// <param name="cardNumber"> cardNumber </param>
        /// <param name="cardExpires"> cardExpires </param>
        /// <param name="cardSecurityCode"> cardSecurityCode </param>
        /// <param name="currencyGuid"> currencyGuid </param>
        /// <param name="currencyValue"> currencyValue </param>
        /// <param name="subTotal"> subTotal </param>
        /// <param name="taxTotal"> taxTotal </param>
        /// <param name="shippingTotal"> shippingTotal </param>
        /// <param name="orderTotal"> orderTotal </param>
        /// <param name="created"> created </param>
        /// <param name="createdFromIP"> createdFromIP </param>
        /// <param name="completed"> completed </param>
        /// <param name="completedFromIP"> completedFromIP </param>
        /// <param name="lastModified"> lastModified </param>
        /// <param name="lastUserActivity"> lastUserActivity </param>
        /// <param name="statusGuid"> statusGuid </param>
        /// <param name="gatewayTransID"> gatewayTransID </param>
        /// <param name="gatewayRawResponse"> gatewayRawResponse </param>
        /// <param name="gatewayAuthCode"> gatewayAuthCode </param>
        /// <param name="taxZoneGuid"> taxZoneGuid </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid orderGuid,
            Guid userGuid,
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
            decimal subTotal,
            decimal taxTotal,
            decimal shippingTotal,
            decimal orderTotal,
            DateTime completed,
            string completedFromIP,
            DateTime lastModified,
            DateTime lastUserActivity,
            Guid statusGuid,
            string gatewayTransID,
            string gatewayRawResponse,
            string gatewayAuthCode,
            Guid taxZoneGuid,
            string paymentMethod,
            decimal discount,
            string discountCodesCsv,
            string customData,
            Guid clerkGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Order_Update", 59);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
   
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
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
            
            sph.DefineSqlParameter("@SubTotal", SqlDbType.Decimal, ParameterDirection.Input, subTotal);
            sph.DefineSqlParameter("@TaxTotal", SqlDbType.Decimal, ParameterDirection.Input, taxTotal);
            sph.DefineSqlParameter("@ShippingTotal", SqlDbType.Decimal, ParameterDirection.Input, shippingTotal);
            sph.DefineSqlParameter("@OrderTotal", SqlDbType.Decimal, ParameterDirection.Input, orderTotal);
            
            sph.DefineSqlParameter("@Completed", SqlDbType.DateTime, ParameterDirection.Input, completed);
            sph.DefineSqlParameter("@CompletedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, completedFromIP);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@LastUserActivity", SqlDbType.DateTime, ParameterDirection.Input, lastUserActivity);
            sph.DefineSqlParameter("@StatusGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, statusGuid);
            sph.DefineSqlParameter("@GatewayTransID", SqlDbType.NVarChar, 255, ParameterDirection.Input, gatewayTransID);
            sph.DefineSqlParameter("@GatewayRawResponse", SqlDbType.NVarChar, -1, ParameterDirection.Input, gatewayRawResponse);
            sph.DefineSqlParameter("@GatewayAuthCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, gatewayAuthCode);
            sph.DefineSqlParameter("@TaxZoneGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, taxZoneGuid);
            sph.DefineSqlParameter("@PaymentMethod", SqlDbType.NVarChar, 50, ParameterDirection.Input, paymentMethod);
            sph.DefineSqlParameter("@CustomData", SqlDbType.NVarChar, -1, ParameterDirection.Input, customData);
            sph.DefineSqlParameter("@DiscountCodesCsv", SqlDbType.NVarChar, -1, ParameterDirection.Input, discountCodesCsv);
            sph.DefineSqlParameter("@Discount", SqlDbType.Decimal, ParameterDirection.Input, discount);
            sph.DefineSqlParameter("@ClerkGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, clerkGuid);
        
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the ws_Order table. Returns true if row deleted.
        /// </summary>
        /// <param name="orderGuid"> orderGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid orderGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Order_Delete", 1);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Updates the mp_CommerceReport table with any missing orders
        /// </summary>
        /// <param name="orderGuid"> orderGuid </param>
        /// <returns>bool</returns>
        public static void EnsureSalesReportData(Guid moduleGuid, int pageId, int moduleId)
        {
            // note we are not using the moduleId param here. It was needed in the signature to
            // make things more expedient in other database mysql

            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Order_EnsureReportData", 2);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            sph.ExecuteNonQuery();
            

        }

        /// <summary>
        /// Updates the mp_CommerceReport table with any missing orders
        /// </summary>
        /// <param name="orderGuid"> orderGuid </param>
        /// <returns>bool</returns>
        public static void EnsureSalesReportOrderData(Guid moduleGuid, int pageId, int moduleId)
        {
            // note we are not using the moduleId param here. It was needed in the signature to
            // make things more expedient in other database mysql

            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Order_EnsureReportOrderData", 2);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            sph.ExecuteNonQuery();


        }

        /// <summary>
        /// Updates the mp_CommerceReport table with any missing orders
        /// </summary>
        /// <param name="orderGuid"> orderGuid </param>
        /// <returns>bool</returns>
        public static void EnsureSalesReportData(Guid orderGuid, Guid moduleGuid, int pageId, int moduleId)
        {
            // note we are not using the moduleId param here. It was needed in the signature to
            // make things more expedient in other database mysql

            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Order_EnsureReportDataForOrder", 3);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            sph.ExecuteNonQuery();


        }

        /// <summary>
        /// Updates the mp_CommerceReport table with any missing orders
        /// </summary>
        /// <param name="orderGuid"> orderGuid </param>
        /// <returns>bool</returns>
        public static void EnsureSalesReportDataForOrders(Guid orderGuid, Guid moduleGuid, int pageId, int moduleId)
        {
            // note we are not using the moduleId param here. It was needed in the signature to
            // make things more expedient in other database mysql

            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Order_EnsureReportOrderDataForOrder", 3);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            sph.ExecuteNonQuery();


        }

        /// <summary>
        /// Flags an order as already tracked in google analytics
        /// </summary>
        /// <param name="orderGuid"> orderGuid </param>
        /// <returns>bool</returns>
        public static bool TrackAnalytics(Guid orderGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Order_TrackAnalytics", 1);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the ws_Order table.
        /// </summary>
        /// <param name="orderGuid"> orderGuid </param>
        public static IDataReader Get(Guid orderGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Order_SelectOne", 1);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            return sph.ExecuteReader();

        }

        public static IDataReader GetProducts(Guid orderGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_OrderOfferProduct_SelectByOrder", 1);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            return sph.ExecuteReader();

            

        }

        public static IDataReader GetByUser(
            Guid storeGuid,
            Guid userGuid)
        {

            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Order_SelectByUser", 2);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            return sph.ExecuteReader();

           
        }


        /// <summary>
		/// Gets a count of rows in the ws_Order table.
		/// </summary>
		public static int GetCount(Guid storeGuid)
		{
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Order_GetCount", 1);
			sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
			
			return Convert.ToInt32(sph.ExecuteScalar());
	
		}


        public static IDataReader GetByStore(
            Guid storeGuid,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Order_SelectPage", 3);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

            

        }

        /// <summary>
		/// Gets a page of data from the ws_Order table.
		/// </summary>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="totalPages">total pages</param>
		public static IDataReader GetPage(
            Guid storeGuid,
			int pageNumber, 
			int pageSize,
			out int totalPages)
		{
			totalPages = 1;
			int totalRows
				= GetCount(storeGuid);
				
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

            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Order_SelectPage", 3);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
			sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
			sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
			return sph.ExecuteReader();
		
		}

        public static IDataReader GetSalesByYearMonth(Guid storeGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Order_GetSalesByYearMonth", 1);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            
            return sph.ExecuteReader();

        }

        public static IDataReader GetRevenueSummary(Guid storeGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Order_GetRevenueSummary", 1);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);

            return sph.ExecuteReader();
        }

        public static decimal GetAllTimeRevenueTotal(Guid storeGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Order_GetAllTimeRevenue", 1);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);

            decimal result = 0;

            try
            {
                result = Convert.ToDecimal(sph.ExecuteScalar());
            }
            catch (InvalidCastException) { }

            return result;
        }

        public static bool MoveOrder(
            Guid orderGuid,
            Guid newUserGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Order_MoveOrder", 2);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, newUserGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }
	
    }
}
