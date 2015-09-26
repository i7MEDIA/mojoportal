// Author:					Joe Audette
// Created:				    2008-02-25
// Last Modified:		    2012-07-20
// 
// This implementation is for MySQL. 
// 
// The use and distribution terms for this software are covered by the 
// GPL (http://www.gnu.org/licenses/gpl.html)
// which can be found in the file GPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
//  

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using MySql.Data.MySqlClient;
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
            #region Bit Conversion

            int intCustomerEmailVerified;
            if (customerEmailVerified)
            {
                intCustomerEmailVerified = 1;
            }
            else
            {
                intCustomerEmailVerified = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO ws_Order (");
            sqlCommand.Append("OrderGuid, ");
            sqlCommand.Append("OrderNo, ");
            sqlCommand.Append("StoreGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("ClerkGuid, ");
            sqlCommand.Append("CustomerFirstName, ");
            sqlCommand.Append("CustomerLastName, ");
            sqlCommand.Append("CustomerCompany, ");
            sqlCommand.Append("CustomerAddressLine1, ");
            sqlCommand.Append("CustomerAddressLine2, ");
            sqlCommand.Append("CustomerSuburb, ");
            sqlCommand.Append("CustomerCity, ");
            sqlCommand.Append("CustomerPostalCode, ");
            sqlCommand.Append("CustomerState, ");
            sqlCommand.Append("CustomerCountry, ");
            sqlCommand.Append("CustomerTelephoneDay, ");
            sqlCommand.Append("CustomerTelephoneNight, ");
            sqlCommand.Append("CustomerEmail, ");
            sqlCommand.Append("CustomerEmailVerified, ");
            sqlCommand.Append("DeliveryFirstName, ");
            sqlCommand.Append("DeliveryLastName, ");
            sqlCommand.Append("DeliveryCompany, ");
            sqlCommand.Append("DeliveryAddress1, ");
            sqlCommand.Append("DeliveryAddress2, ");
            sqlCommand.Append("DeliverySuburb, ");
            sqlCommand.Append("DeliveryCity, ");
            sqlCommand.Append("DeliveryPostalCode, ");
            sqlCommand.Append("DeliveryState, ");
            sqlCommand.Append("DeliveryCountry, ");
            sqlCommand.Append("BillingFirstName, ");
            sqlCommand.Append("BillingLastName, ");
            sqlCommand.Append("BillingCompany, ");
            sqlCommand.Append("BillingAddress1, ");
            sqlCommand.Append("BillingAddress2, ");
            sqlCommand.Append("BillingSuburb, ");
            sqlCommand.Append("BillingCity, ");
            sqlCommand.Append("BillingPostalCode, ");
            sqlCommand.Append("BillingState, ");
            sqlCommand.Append("BillingCountry, ");
            sqlCommand.Append("CardTypeGuid, ");
            sqlCommand.Append("CardOwner, ");
            sqlCommand.Append("CardNumber, ");
            sqlCommand.Append("CardExpires, ");
            sqlCommand.Append("CardSecurityCode, ");
           
            sqlCommand.Append("SubTotal, ");
            sqlCommand.Append("TaxTotal, ");
            sqlCommand.Append("ShippingTotal, ");
            sqlCommand.Append("OrderTotal, ");
            sqlCommand.Append("Discount, ");
            sqlCommand.Append("DiscountCodesCsv, ");
            sqlCommand.Append("CustomData, ");
            sqlCommand.Append("Created, ");
            sqlCommand.Append("CreatedFromIP, ");
            sqlCommand.Append("Completed, ");
            sqlCommand.Append("CompletedFromIP, ");
            sqlCommand.Append("LastModified, ");
            sqlCommand.Append("LastUserActivity, ");
            sqlCommand.Append("StatusGuid, ");
            sqlCommand.Append("GatewayTransID, ");
            sqlCommand.Append("GatewayRawResponse, ");
            sqlCommand.Append("GatewayAuthCode, ");
            sqlCommand.Append("AnalyticsTracked, ");
            sqlCommand.Append("TaxZoneGuid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?OrderGuid, ");
            sqlCommand.Append("?OrderNo, ");
            sqlCommand.Append("?StoreGuid, ");
            sqlCommand.Append("?UserGuid, ");
            sqlCommand.Append("?ClerkGuid, ");
            sqlCommand.Append("?CustomerFirstName, ");
            sqlCommand.Append("?CustomerLastName, ");
            sqlCommand.Append("?CustomerCompany, ");
            sqlCommand.Append("?CustomerAddressLine1, ");
            sqlCommand.Append("?CustomerAddressLine2, ");
            sqlCommand.Append("?CustomerSuburb, ");
            sqlCommand.Append("?CustomerCity, ");
            sqlCommand.Append("?CustomerPostalCode, ");
            sqlCommand.Append("?CustomerState, ");
            sqlCommand.Append("?CustomerCountry, ");
            sqlCommand.Append("?CustomerTelephoneDay, ");
            sqlCommand.Append("?CustomerTelephoneNight, ");
            sqlCommand.Append("?CustomerEmail, ");
            sqlCommand.Append("?CustomerEmailVerified, ");
            sqlCommand.Append("?DeliveryFirstName, ");
            sqlCommand.Append("?DeliveryLastName, ");
            sqlCommand.Append("?DeliveryCompany, ");
            sqlCommand.Append("?DeliveryAddress1, ");
            sqlCommand.Append("?DeliveryAddress2, ");
            sqlCommand.Append("?DeliverySuburb, ");
            sqlCommand.Append("?DeliveryCity, ");
            sqlCommand.Append("?DeliveryPostalCode, ");
            sqlCommand.Append("?DeliveryState, ");
            sqlCommand.Append("?DeliveryCountry, ");
            sqlCommand.Append("?BillingFirstName, ");
            sqlCommand.Append("?BillingLastName, ");
            sqlCommand.Append("?BillingCompany, ");
            sqlCommand.Append("?BillingAddress1, ");
            sqlCommand.Append("?BillingAddress2, ");
            sqlCommand.Append("?BillingSuburb, ");
            sqlCommand.Append("?BillingCity, ");
            sqlCommand.Append("?BillingPostalCode, ");
            sqlCommand.Append("?BillingState, ");
            sqlCommand.Append("?BillingCountry, ");
            sqlCommand.Append("?CardTypeGuid, ");
            sqlCommand.Append("?CardOwner, ");
            sqlCommand.Append("?CardNumber, ");
            sqlCommand.Append("?CardExpires, ");
            sqlCommand.Append("?CardSecurityCode, ");
            
            sqlCommand.Append("?SubTotal, ");
            sqlCommand.Append("?TaxTotal, ");
            sqlCommand.Append("?ShippingTotal, ");
            sqlCommand.Append("?OrderTotal, ");
            sqlCommand.Append("?Discount, ");
            sqlCommand.Append("?DiscountCodesCsv, ");
            sqlCommand.Append("?CustomData, ");

            sqlCommand.Append("?Created, ");
            sqlCommand.Append("?CreatedFromIP, ");
            sqlCommand.Append("?Completed, ");
            sqlCommand.Append("?CompletedFromIP, ");
            sqlCommand.Append("?LastModified, ");
            sqlCommand.Append("?LastUserActivity, ");
            sqlCommand.Append("?StatusGuid, ");
            sqlCommand.Append("?GatewayTransID, ");
            sqlCommand.Append("?GatewayRawResponse, ");
            sqlCommand.Append("?GatewayAuthCode, ");
            sqlCommand.Append("0, ");
            sqlCommand.Append("?TaxZoneGuid )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[62];

            arParams[0] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = orderGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderNo", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderNo;

            arParams[2] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = storeGuid.ToString();

            arParams[3] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new MySqlParameter("?CustomerFirstName", MySqlDbType.VarChar, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = customerFirstName;

            arParams[5] = new MySqlParameter("?CustomerLastName", MySqlDbType.VarChar, 100);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = customerLastName;

            arParams[6] = new MySqlParameter("?CustomerCompany", MySqlDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = customerCompany;

            arParams[7] = new MySqlParameter("?CustomerAddressLine1", MySqlDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = customerAddressLine1;

            arParams[8] = new MySqlParameter("?CustomerAddressLine2", MySqlDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = customerAddressLine2;

            arParams[9] = new MySqlParameter("?CustomerSuburb", MySqlDbType.VarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = customerSuburb;

            arParams[10] = new MySqlParameter("?CustomerCity", MySqlDbType.VarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = customerCity;

            arParams[11] = new MySqlParameter("?CustomerPostalCode", MySqlDbType.VarChar, 20);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = customerPostalCode;

            arParams[12] = new MySqlParameter("?CustomerState", MySqlDbType.VarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = customerState;

            arParams[13] = new MySqlParameter("?CustomerCountry", MySqlDbType.VarChar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = customerCountry;

            arParams[14] = new MySqlParameter("?CustomerTelephoneDay", MySqlDbType.VarChar, 32);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = customerTelephoneDay;

            arParams[15] = new MySqlParameter("?CustomerTelephoneNight", MySqlDbType.VarChar, 32);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = customerTelephoneNight;

            arParams[16] = new MySqlParameter("?CustomerEmail", MySqlDbType.VarChar, 96);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = customerEmail;

            arParams[17] = new MySqlParameter("?CustomerEmailVerified", MySqlDbType.Int32);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = intCustomerEmailVerified;

            arParams[18] = new MySqlParameter("?DeliveryFirstName", MySqlDbType.VarChar, 100);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = deliveryFirstName;

            arParams[19] = new MySqlParameter("?DeliveryLastName", MySqlDbType.VarChar, 100);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = deliveryLastName;

            arParams[20] = new MySqlParameter("?DeliveryCompany", MySqlDbType.VarChar, 255);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = deliveryCompany;

            arParams[21] = new MySqlParameter("?DeliveryAddress1", MySqlDbType.VarChar, 255);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = deliveryAddress1;

            arParams[22] = new MySqlParameter("?DeliveryAddress2", MySqlDbType.VarChar, 255);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = deliveryAddress2;

            arParams[23] = new MySqlParameter("?DeliverySuburb", MySqlDbType.VarChar, 255);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = deliverySuburb;

            arParams[24] = new MySqlParameter("?DeliveryCity", MySqlDbType.VarChar, 255);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = deliveryCity;

            arParams[25] = new MySqlParameter("?DeliveryPostalCode", MySqlDbType.VarChar, 20);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = deliveryPostalCode;

            arParams[26] = new MySqlParameter("?DeliveryState", MySqlDbType.VarChar, 255);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = deliveryState;

            arParams[27] = new MySqlParameter("?DeliveryCountry", MySqlDbType.VarChar, 255);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = deliveryCountry;

            arParams[28] = new MySqlParameter("?BillingFirstName", MySqlDbType.VarChar, 100);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = billingFirstName;

            arParams[29] = new MySqlParameter("?BillingLastName", MySqlDbType.VarChar, 100);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = billingLastName;

            arParams[30] = new MySqlParameter("?BillingCompany", MySqlDbType.VarChar, 255);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = billingCompany;

            arParams[31] = new MySqlParameter("?BillingAddress1", MySqlDbType.VarChar, 255);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = billingAddress1;

            arParams[32] = new MySqlParameter("?BillingAddress2", MySqlDbType.VarChar, 255);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = billingAddress2;

            arParams[33] = new MySqlParameter("?BillingSuburb", MySqlDbType.VarChar, 255);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = billingSuburb;

            arParams[34] = new MySqlParameter("?BillingCity", MySqlDbType.VarChar, 255);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = billingCity;

            arParams[35] = new MySqlParameter("?BillingPostalCode", MySqlDbType.VarChar, 20);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = billingPostalCode;

            arParams[36] = new MySqlParameter("?BillingState", MySqlDbType.VarChar, 255);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = billingState;

            arParams[37] = new MySqlParameter("?BillingCountry", MySqlDbType.VarChar, 255);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = billingCountry;

            arParams[38] = new MySqlParameter("?CardTypeGuid", MySqlDbType.VarChar, 36);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = cardTypeGuid.ToString();

            arParams[39] = new MySqlParameter("?CardOwner", MySqlDbType.VarChar, 100);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = cardOwner;

            arParams[40] = new MySqlParameter("?CardNumber", MySqlDbType.VarChar, 255);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = cardNumber;

            arParams[41] = new MySqlParameter("?CardExpires", MySqlDbType.VarChar, 6);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = cardExpires;

            arParams[42] = new MySqlParameter("?CardSecurityCode", MySqlDbType.VarChar, 50);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = cardSecurityCode;

            arParams[43] = new MySqlParameter("?SubTotal", MySqlDbType.Decimal);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = subTotal;

            arParams[44] = new MySqlParameter("?TaxTotal", MySqlDbType.Decimal);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = taxTotal;

            arParams[45] = new MySqlParameter("?ShippingTotal", MySqlDbType.Decimal);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = shippingTotal;

            arParams[46] = new MySqlParameter("?OrderTotal", MySqlDbType.Decimal);
            arParams[46].Direction = ParameterDirection.Input;
            arParams[46].Value = orderTotal;

            arParams[47] = new MySqlParameter("?Created", MySqlDbType.DateTime);
            arParams[47].Direction = ParameterDirection.Input;
            arParams[47].Value = created;

            arParams[48] = new MySqlParameter("?CreatedFromIP", MySqlDbType.VarChar, 255);
            arParams[48].Direction = ParameterDirection.Input;
            arParams[48].Value = createdFromIP;

            arParams[49] = new MySqlParameter("?Completed", MySqlDbType.DateTime);
            arParams[49].Direction = ParameterDirection.Input;
            arParams[49].Value = completed;

            arParams[50] = new MySqlParameter("?CompletedFromIP", MySqlDbType.VarChar, 255);
            arParams[50].Direction = ParameterDirection.Input;
            arParams[50].Value = completedFromIP;

            arParams[51] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[51].Direction = ParameterDirection.Input;
            arParams[51].Value = lastModified;

            arParams[52] = new MySqlParameter("?LastUserActivity", MySqlDbType.DateTime);
            arParams[52].Direction = ParameterDirection.Input;
            arParams[52].Value = lastUserActivity;

            arParams[53] = new MySqlParameter("?StatusGuid", MySqlDbType.VarChar, 36);
            arParams[53].Direction = ParameterDirection.Input;
            arParams[53].Value = statusGuid.ToString();

            arParams[54] = new MySqlParameter("?GatewayTransID", MySqlDbType.VarChar, 255);
            arParams[54].Direction = ParameterDirection.Input;
            arParams[54].Value = gatewayTransID;

            arParams[55] = new MySqlParameter("?GatewayRawResponse", MySqlDbType.Text);
            arParams[55].Direction = ParameterDirection.Input;
            arParams[55].Value = gatewayRawResponse;

            arParams[56] = new MySqlParameter("?GatewayAuthCode", MySqlDbType.VarChar, 50);
            arParams[56].Direction = ParameterDirection.Input;
            arParams[56].Value = gatewayAuthCode;

            arParams[57] = new MySqlParameter("?TaxZoneGuid", MySqlDbType.VarChar, 36);
            arParams[57].Direction = ParameterDirection.Input;
            arParams[57].Value = taxZoneGuid.ToString();

            arParams[58] = new MySqlParameter("?Discount", MySqlDbType.Decimal);
            arParams[58].Direction = ParameterDirection.Input;
            arParams[58].Value = discount;

            arParams[59] = new MySqlParameter("?DiscountCodesCsv", MySqlDbType.Text);
            arParams[59].Direction = ParameterDirection.Input;
            arParams[59].Value = discountCodesCsv;

            arParams[60] = new MySqlParameter("?CustomData", MySqlDbType.Text);
            arParams[60].Direction = ParameterDirection.Input;
            arParams[60].Value = customData;

            arParams[61] = new MySqlParameter("?ClerkGuid", MySqlDbType.VarChar, 36);
            arParams[61].Direction = ParameterDirection.Input;
            arParams[61].Value = clerkGuid.ToString();
            

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
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

            #region Bit Conversion

            int intCustomerEmailVerified;
            if (customerEmailVerified)
            {
                intCustomerEmailVerified = 1;
            }
            else
            {
                intCustomerEmailVerified = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_Order ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("UserGuid = ?UserGuid, ");
            sqlCommand.Append("ClerkGuid = ?ClerkGuid, ");
            sqlCommand.Append("CustomerFirstName = ?CustomerFirstName, ");
            sqlCommand.Append("CustomerLastName = ?CustomerLastName, ");
            sqlCommand.Append("CustomerCompany = ?CustomerCompany, ");
            sqlCommand.Append("CustomerAddressLine1 = ?CustomerAddressLine1, ");
            sqlCommand.Append("CustomerAddressLine2 = ?CustomerAddressLine2, ");
            sqlCommand.Append("CustomerSuburb = ?CustomerSuburb, ");
            sqlCommand.Append("CustomerCity = ?CustomerCity, ");
            sqlCommand.Append("CustomerPostalCode = ?CustomerPostalCode, ");
            sqlCommand.Append("CustomerState = ?CustomerState, ");
            sqlCommand.Append("CustomerCountry = ?CustomerCountry, ");
            sqlCommand.Append("CustomerTelephoneDay = ?CustomerTelephoneDay, ");
            sqlCommand.Append("CustomerTelephoneNight = ?CustomerTelephoneNight, ");
            sqlCommand.Append("CustomerEmail = ?CustomerEmail, ");
            sqlCommand.Append("CustomerEmailVerified = ?CustomerEmailVerified, ");
            sqlCommand.Append("DeliveryFirstName = ?DeliveryFirstName, ");
            sqlCommand.Append("DeliveryLastName = ?DeliveryLastName, ");
            sqlCommand.Append("DeliveryCompany = ?DeliveryCompany, ");
            sqlCommand.Append("DeliveryAddress1 = ?DeliveryAddress1, ");
            sqlCommand.Append("DeliveryAddress2 = ?DeliveryAddress2, ");
            sqlCommand.Append("DeliverySuburb = ?DeliverySuburb, ");
            sqlCommand.Append("DeliveryCity = ?DeliveryCity, ");
            sqlCommand.Append("DeliveryPostalCode = ?DeliveryPostalCode, ");
            sqlCommand.Append("DeliveryState = ?DeliveryState, ");
            sqlCommand.Append("DeliveryCountry = ?DeliveryCountry, ");
            sqlCommand.Append("BillingFirstName = ?BillingFirstName, ");
            sqlCommand.Append("BillingLastName = ?BillingLastName, ");
            sqlCommand.Append("BillingCompany = ?BillingCompany, ");
            sqlCommand.Append("BillingAddress1 = ?BillingAddress1, ");
            sqlCommand.Append("BillingAddress2 = ?BillingAddress2, ");
            sqlCommand.Append("BillingSuburb = ?BillingSuburb, ");
            sqlCommand.Append("BillingCity = ?BillingCity, ");
            sqlCommand.Append("BillingPostalCode = ?BillingPostalCode, ");
            sqlCommand.Append("BillingState = ?BillingState, ");
            sqlCommand.Append("BillingCountry = ?BillingCountry, ");
            sqlCommand.Append("CardTypeGuid = ?CardTypeGuid, ");
            sqlCommand.Append("CardOwner = ?CardOwner, ");
            sqlCommand.Append("CardNumber = ?CardNumber, ");
            sqlCommand.Append("CardExpires = ?CardExpires, ");
            sqlCommand.Append("CardSecurityCode = ?CardSecurityCode, ");
            
            sqlCommand.Append("SubTotal = ?SubTotal, ");
            sqlCommand.Append("TaxTotal = ?TaxTotal, ");
            sqlCommand.Append("ShippingTotal = ?ShippingTotal, ");
            sqlCommand.Append("Discount = ?Discount, ");
            sqlCommand.Append("OrderTotal = ?OrderTotal, ");
            sqlCommand.Append("DiscountCodesCsv = ?DiscountCodesCsv, ");
            sqlCommand.Append("CustomData = ?CustomData, ");
            sqlCommand.Append("PaymentMethod = ?PaymentMethod, ");
           
            sqlCommand.Append("Completed = ?Completed, ");
            sqlCommand.Append("CompletedFromIP = ?CompletedFromIP, ");
            sqlCommand.Append("LastModified = ?LastModified, ");
            sqlCommand.Append("LastUserActivity = ?LastUserActivity, ");
            sqlCommand.Append("StatusGuid = ?StatusGuid, ");
            sqlCommand.Append("GatewayTransID = ?GatewayTransID, ");
            sqlCommand.Append("GatewayRawResponse = ?GatewayRawResponse, ");
            sqlCommand.Append("GatewayAuthCode = ?GatewayAuthCode, ");
            
            sqlCommand.Append("TaxZoneGuid = ?TaxZoneGuid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("OrderGuid = ?OrderGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[59];

            arParams[0] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = orderGuid.ToString();

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new MySqlParameter("?CustomerFirstName", MySqlDbType.VarChar, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = customerFirstName;

            arParams[3] = new MySqlParameter("?CustomerLastName", MySqlDbType.VarChar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = customerLastName;

            arParams[4] = new MySqlParameter("?CustomerCompany", MySqlDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = customerCompany;

            arParams[5] = new MySqlParameter("?CustomerAddressLine1", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = customerAddressLine1;

            arParams[6] = new MySqlParameter("?CustomerAddressLine2", MySqlDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = customerAddressLine2;

            arParams[7] = new MySqlParameter("?CustomerSuburb", MySqlDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = customerSuburb;

            arParams[8] = new MySqlParameter("?CustomerCity", MySqlDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = customerCity;

            arParams[9] = new MySqlParameter("?CustomerPostalCode", MySqlDbType.VarChar, 20);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = customerPostalCode;

            arParams[10] = new MySqlParameter("?CustomerState", MySqlDbType.VarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = customerState;

            arParams[11] = new MySqlParameter("?CustomerCountry", MySqlDbType.VarChar, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = customerCountry;

            arParams[12] = new MySqlParameter("?CustomerTelephoneDay", MySqlDbType.VarChar, 32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = customerTelephoneDay;

            arParams[13] = new MySqlParameter("?CustomerTelephoneNight", MySqlDbType.VarChar, 32);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = customerTelephoneNight;

            arParams[14] = new MySqlParameter("?CustomerEmail", MySqlDbType.VarChar, 96);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = customerEmail;

            arParams[15] = new MySqlParameter("?CustomerEmailVerified", MySqlDbType.Int32);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = intCustomerEmailVerified;

            arParams[16] = new MySqlParameter("?DeliveryFirstName", MySqlDbType.VarChar, 100);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = deliveryFirstName;

            arParams[17] = new MySqlParameter("?DeliveryLastName", MySqlDbType.VarChar, 100);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = deliveryLastName;

            arParams[18] = new MySqlParameter("?DeliveryCompany", MySqlDbType.VarChar, 255);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = deliveryCompany;

            arParams[19] = new MySqlParameter("?DeliveryAddress1", MySqlDbType.VarChar, 255);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = deliveryAddress1;

            arParams[20] = new MySqlParameter("?DeliveryAddress2", MySqlDbType.VarChar, 255);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = deliveryAddress2;

            arParams[21] = new MySqlParameter("?DeliverySuburb", MySqlDbType.VarChar, 255);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = deliverySuburb;

            arParams[22] = new MySqlParameter("?DeliveryCity", MySqlDbType.VarChar, 255);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = deliveryCity;

            arParams[23] = new MySqlParameter("?DeliveryPostalCode", MySqlDbType.VarChar, 20);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = deliveryPostalCode;

            arParams[24] = new MySqlParameter("?DeliveryState", MySqlDbType.VarChar, 255);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = deliveryState;

            arParams[25] = new MySqlParameter("?DeliveryCountry", MySqlDbType.VarChar, 255);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = deliveryCountry;

            arParams[26] = new MySqlParameter("?BillingFirstName", MySqlDbType.VarChar, 100);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = billingFirstName;

            arParams[27] = new MySqlParameter("?BillingLastName", MySqlDbType.VarChar, 100);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = billingLastName;

            arParams[28] = new MySqlParameter("?BillingCompany", MySqlDbType.VarChar, 255);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = billingCompany;

            arParams[29] = new MySqlParameter("?BillingAddress1", MySqlDbType.VarChar, 255);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = billingAddress1;

            arParams[30] = new MySqlParameter("?BillingAddress2", MySqlDbType.VarChar, 255);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = billingAddress2;

            arParams[31] = new MySqlParameter("?BillingSuburb", MySqlDbType.VarChar, 255);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = billingSuburb;

            arParams[32] = new MySqlParameter("?BillingCity", MySqlDbType.VarChar, 255);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = billingCity;

            arParams[33] = new MySqlParameter("?BillingPostalCode", MySqlDbType.VarChar, 20);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = billingPostalCode;

            arParams[34] = new MySqlParameter("?BillingState", MySqlDbType.VarChar, 255);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = billingState;

            arParams[35] = new MySqlParameter("?BillingCountry", MySqlDbType.VarChar, 255);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = billingCountry;

            arParams[36] = new MySqlParameter("?CardTypeGuid", MySqlDbType.VarChar, 36);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = cardTypeGuid.ToString();

            arParams[37] = new MySqlParameter("?CardOwner", MySqlDbType.VarChar, 100);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = cardOwner;

            arParams[38] = new MySqlParameter("?CardNumber", MySqlDbType.VarChar, 255);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = cardNumber;

            arParams[39] = new MySqlParameter("?CardExpires", MySqlDbType.VarChar, 6);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = cardExpires;

            arParams[40] = new MySqlParameter("?CardSecurityCode", MySqlDbType.VarChar, 50);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = cardSecurityCode;

            arParams[41] = new MySqlParameter("?SubTotal", MySqlDbType.Decimal);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = subTotal;

            arParams[42] = new MySqlParameter("?TaxTotal", MySqlDbType.Decimal);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = taxTotal;

            arParams[43] = new MySqlParameter("?ShippingTotal", MySqlDbType.Decimal);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = shippingTotal;

            arParams[44] = new MySqlParameter("?OrderTotal", MySqlDbType.Decimal);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = orderTotal;

            arParams[45] = new MySqlParameter("?Completed", MySqlDbType.DateTime);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = completed;

            arParams[46] = new MySqlParameter("?CompletedFromIP", MySqlDbType.VarChar, 255);
            arParams[46].Direction = ParameterDirection.Input;
            arParams[46].Value = completedFromIP;

            arParams[47] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[47].Direction = ParameterDirection.Input;
            arParams[47].Value = lastModified;

            arParams[48] = new MySqlParameter("?LastUserActivity", MySqlDbType.DateTime);
            arParams[48].Direction = ParameterDirection.Input;
            arParams[48].Value = lastUserActivity;

            arParams[49] = new MySqlParameter("?StatusGuid", MySqlDbType.VarChar, 36);
            arParams[49].Direction = ParameterDirection.Input;
            arParams[49].Value = statusGuid.ToString();

            arParams[50] = new MySqlParameter("?GatewayTransID", MySqlDbType.VarChar, 255);
            arParams[50].Direction = ParameterDirection.Input;
            arParams[50].Value = gatewayTransID;

            arParams[51] = new MySqlParameter("?GatewayRawResponse", MySqlDbType.Text);
            arParams[51].Direction = ParameterDirection.Input;
            arParams[51].Value = gatewayRawResponse;

            arParams[52] = new MySqlParameter("?GatewayAuthCode", MySqlDbType.VarChar, 50);
            arParams[52].Direction = ParameterDirection.Input;
            arParams[52].Value = gatewayAuthCode;

            arParams[53] = new MySqlParameter("?TaxZoneGuid", MySqlDbType.VarChar, 36);
            arParams[53].Direction = ParameterDirection.Input;
            arParams[53].Value = taxZoneGuid.ToString();

            arParams[54] = new MySqlParameter("?PaymentMethod", MySqlDbType.VarChar, 50);
            arParams[54].Direction = ParameterDirection.Input;
            arParams[54].Value = paymentMethod;

            arParams[55] = new MySqlParameter("?Discount", MySqlDbType.Decimal);
            arParams[55].Direction = ParameterDirection.Input;
            arParams[55].Value = discount;

            arParams[56] = new MySqlParameter("?DiscountCodesCsv", MySqlDbType.Text);
            arParams[56].Direction = ParameterDirection.Input;
            arParams[56].Value = discountCodesCsv;

            arParams[57] = new MySqlParameter("?CustomData", MySqlDbType.Text);
            arParams[57].Direction = ParameterDirection.Input;
            arParams[57].Value = customData;

            arParams[58] = new MySqlParameter("?ClerkGuid", MySqlDbType.VarChar, 36);
            arParams[58].Direction = ParameterDirection.Input;
            arParams[58].Value = clerkGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
           
        }

        public static bool Delete(Guid orderGuid)
        {
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_Order ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("OrderGuid = ?OrderGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = orderGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Updates the mp_CommerceReport table with any missing orders
        /// </summary>
        /// <param name="orderGuid"> orderGuid </param>
        /// <returns>bool</returns>
        public static void EnsureSalesReportData(Guid moduleGuid, int pageId, int moduleId)
        {
       
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_CommerceReport ");
            sqlCommand.Append("( ");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("FeatureGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("ModuleTitle, ");
            sqlCommand.Append("OrderGuid, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("ItemName, ");
            sqlCommand.Append("Quantity, ");
            sqlCommand.Append("Price, ");
            sqlCommand.Append("SubTotal, ");
            sqlCommand.Append("OrderDateUtc, ");
            sqlCommand.Append("PaymentMethod, ");
            sqlCommand.Append("IPAddress, ");
            sqlCommand.Append("AdminOrderLink, ");
            sqlCommand.Append("UserOrderLink, ");
            sqlCommand.Append("RowCreatedUtc ");
            sqlCommand.Append(") ");

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("oo.ItemGuid AS RowGuid, ");
            sqlCommand.Append("s.SiteGuid, ");
            sqlCommand.Append("o.UserGuid, ");
            sqlCommand.Append("m.FeatureGuid, ");
            sqlCommand.Append("m.Guid AS ModuleGuid, ");
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("o.OrderGuid, ");
            sqlCommand.Append("oo.OfferGuid AS ItemGuid, ");
            sqlCommand.Append("p.Name AS ItemName, ");
            sqlCommand.Append("oo.Quantity, ");
            sqlCommand.Append("oo.OfferPrice AS Price, ");
            sqlCommand.Append("(oo.Quantity * oo.OfferPrice) AS SubTotal, ");
            sqlCommand.Append("o.Created AS OrderDateUtc, ");
            sqlCommand.Append("o.PaymentMethod, ");
            sqlCommand.Append("o.CreatedFromIP AS IPAddress, ");

            sqlCommand.Append("CONCAT('/WebStore/AdminOrderDetail.aspx?pageid=' ");
            sqlCommand.Append(", " + pageId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;mid=', " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;order=',o.OrderGuid) AS AdminOrderLink, ");

            sqlCommand.Append("CONCAT('/WebStore/OrderDetail.aspx?pageid=' ");
            sqlCommand.Append(", " + pageId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;mid=', " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(", '&amp;order=' , o.OrderGuid) AS UserOrderLink, ");

            sqlCommand.Append("now() As RowCreatedUtc ");
            
            sqlCommand.Append("FROM ");
            sqlCommand.Append("ws_Store s ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Modules m ");
            sqlCommand.Append("ON s.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("ws_Order o ");
            sqlCommand.Append("ON s.Guid = o.StoreGuid ");
            sqlCommand.Append("AND o.StatusGuid <> 'de3b9331-b98f-493f-be5e-926ffe5003bc' "); //cancelled
            sqlCommand.Append("AND o.StatusGuid <> '0db28432-d9a9-423e-84f2-8a94db434643' "); //received
            sqlCommand.Append("AND o.StatusGuid <> '00000000-0000-0000-0000-000000000000' "); //none

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("ws_OrderOffers oo ");
            sqlCommand.Append("ON oo.OrderGuid = o.OrderGuid ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("ws_Offer p ");
            sqlCommand.Append("ON oo.OfferGuid = p.Guid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_CommerceReport cr ");
            sqlCommand.Append("ON cr.RowGuid = oo.ItemGuid ");
            
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cr.RowGuid IS NULL ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("m.Guid = ?ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((o.OrderTotal > 0) OR (o.PaymentMethod = 'NoCharge')) ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Updates the mp_CommerceReport table with any missing orders
        /// </summary>
        /// <param name="orderGuid"> orderGuid </param>
        /// <returns>bool</returns>
        public static void EnsureSalesReportData(Guid orderGuid, Guid moduleGuid, int pageId, int moduleId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_CommerceReport ");
            sqlCommand.Append("( ");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("FeatureGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("ModuleTitle, ");
            sqlCommand.Append("OrderGuid, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("ItemName, ");
            sqlCommand.Append("Quantity, ");
            sqlCommand.Append("Price, ");
            sqlCommand.Append("SubTotal, ");
            sqlCommand.Append("OrderDateUtc, ");
            sqlCommand.Append("PaymentMethod, ");
            sqlCommand.Append("IPAddress, ");
            sqlCommand.Append("AdminOrderLink, ");
            sqlCommand.Append("UserOrderLink, ");
            sqlCommand.Append("RowCreatedUtc ");
            sqlCommand.Append(") ");

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("oo.ItemGuid AS RowGuid, ");
            sqlCommand.Append("s.SiteGuid, ");
            sqlCommand.Append("o.UserGuid, ");
            sqlCommand.Append("m.FeatureGuid, ");
            sqlCommand.Append("m.Guid AS ModuleGuid, ");
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("o.OrderGuid, ");
            sqlCommand.Append("oo.OfferGuid AS ItemGuid, ");
            sqlCommand.Append("p.Name AS ItemName, ");
            sqlCommand.Append("oo.Quantity, ");
            sqlCommand.Append("oo.OfferPrice AS Price, ");
            sqlCommand.Append("(oo.Quantity * oo.OfferPrice) AS SubTotal, ");
            sqlCommand.Append("o.Created AS OrderDateUtc, ");
            sqlCommand.Append("o.PaymentMethod, ");
            sqlCommand.Append("o.CreatedFromIP AS IPAddress, ");

            sqlCommand.Append("CONCAT('/WebStore/AdminOrderDetail.aspx?pageid=' ");
            sqlCommand.Append(", " + pageId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;mid=', " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;order=',o.OrderGuid) AS AdminOrderLink, ");

            sqlCommand.Append("CONCAT('/WebStore/OrderDetail.aspx?pageid=' ");
            sqlCommand.Append(", " + pageId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;mid=', " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(", '&amp;order=' , o.OrderGuid) AS UserOrderLink, ");

            sqlCommand.Append("now() As RowCreatedUtc ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("ws_Store s ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Modules m ");
            sqlCommand.Append("ON s.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("ws_Order o ");
            sqlCommand.Append("ON s.Guid = o.StoreGuid ");
            sqlCommand.Append("AND o.StatusGuid <> 'de3b9331-b98f-493f-be5e-926ffe5003bc' "); //cancelled
            sqlCommand.Append("AND o.StatusGuid <> '0db28432-d9a9-423e-84f2-8a94db434643' "); //received
            sqlCommand.Append("AND o.StatusGuid <> '00000000-0000-0000-0000-000000000000' "); //none

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("ws_OrderOffers oo ");
            sqlCommand.Append("ON oo.OrderGuid = o.OrderGuid ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("ws_Offer p ");
            sqlCommand.Append("ON oo.OfferGuid = p.Guid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_CommerceReport cr ");
            sqlCommand.Append("ON cr.RowGuid = oo.ItemGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("o.OrderGuid = ?OrderGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cr.RowGuid IS NULL ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("m.Guid = ?ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((o.OrderTotal > 0) OR (o.PaymentMethod = 'NoCharge')) ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = orderGuid.ToString();

            arParams[1] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleGuid.ToString();

            MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Updates the mp_CommerceReport table with any missing orders
        /// </summary>
        /// <param name="orderGuid"> orderGuid </param>
        /// <returns>bool</returns>
        public static void EnsureSalesReportOrderData(Guid moduleGuid, int pageId, int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_CommerceReportOrders ");
            sqlCommand.Append("( ");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("FeatureGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid, ");;
            sqlCommand.Append("OrderGuid, ");
            sqlCommand.Append("BillingFirstName, ");
            sqlCommand.Append("BillingLastName, ");
            sqlCommand.Append("BillingCompany, ");
            sqlCommand.Append("BillingAddress1, ");
            sqlCommand.Append("BillingAddress2, ");
            sqlCommand.Append("BillingSuburb, ");
            sqlCommand.Append("BillingCity, ");
            sqlCommand.Append("BillingPostalCode, ");
            sqlCommand.Append("BillingState, ");
            sqlCommand.Append("BillingCountry, ");
            sqlCommand.Append("PaymentMethod, ");
            sqlCommand.Append("SubTotal, ");
            sqlCommand.Append("TaxTotal, ");
            sqlCommand.Append("ShippingTotal, ");
            sqlCommand.Append("OrderTotal, ");
            sqlCommand.Append("OrderDateUtc, ");
            sqlCommand.Append("AdminOrderLink, ");
            sqlCommand.Append("UserOrderLink, ");
            sqlCommand.Append("RowCreatedUtc ");
            sqlCommand.Append(") ");

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("o.OrderGuid AS RowGuid, ");
            sqlCommand.Append("s.SiteGuid, ");
            sqlCommand.Append("m.FeatureGuid, ");
            sqlCommand.Append("m.Guid AS ModuleGuid, ");
            sqlCommand.Append("o.UserGuid, ");
            sqlCommand.Append("o.OrderGuid, ");
            sqlCommand.Append("o.BillingFirstName, ");
            sqlCommand.Append("o.BillingLastName, ");
            sqlCommand.Append("o.BillingCompany, ");
            sqlCommand.Append("o.BillingAddress1, ");
            sqlCommand.Append("o.BillingAddress2, ");
            sqlCommand.Append("o.BillingSuburb, ");
            sqlCommand.Append("o.BillingCity, ");
            sqlCommand.Append("o.BillingPostalCode, ");
            sqlCommand.Append("o.BillingState, ");
            sqlCommand.Append("o.BillingCountry, ");
            sqlCommand.Append("o.PaymentMethod, ");
            sqlCommand.Append("o.SubTotal, ");
            sqlCommand.Append("o.TaxTotal, ");
            sqlCommand.Append("o.ShippingTotal, ");
            sqlCommand.Append("o.OrderTotal, ");
            sqlCommand.Append("o.LastModified, ");

            sqlCommand.Append("CONCAT('/WebStore/AdminOrderDetail.aspx?pageid=' ");
            sqlCommand.Append(", " + pageId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;mid=', " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;order=',o.OrderGuid) AS AdminOrderLink, ");

            sqlCommand.Append("CONCAT('/WebStore/OrderDetail.aspx?pageid=' ");
            sqlCommand.Append(", " + pageId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;mid=', " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(", '&amp;order=' , o.OrderGuid) AS UserOrderLink, ");

            sqlCommand.Append("now() As RowCreatedUtc ");
            
            sqlCommand.Append("FROM ");
            sqlCommand.Append("ws_Store s ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Modules m ");
            sqlCommand.Append("ON s.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("ws_Order o ");
            sqlCommand.Append("ON s.Guid = o.StoreGuid ");
            sqlCommand.Append("AND o.StatusGuid <> 'de3b9331-b98f-493f-be5e-926ffe5003bc' "); //cancelled
            sqlCommand.Append("AND o.StatusGuid <> '0db28432-d9a9-423e-84f2-8a94db434643' "); //received
            sqlCommand.Append("AND o.StatusGuid <> '00000000-0000-0000-0000-000000000000' "); //none

            //

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_CommerceReportOrders cr ");
            sqlCommand.Append("ON cr.RowGuid = o.OrderGuid ");
            
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cr.RowGuid IS NULL ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("m.Guid = ?ModuleGuid ");
            //sqlCommand.Append("AND ");
            //sqlCommand.Append("o.OrderTotal > 0 ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Updates the mp_CommerceReport table with any missing orders
        /// </summary>
        /// <param name="orderGuid"> orderGuid </param>
        /// <returns>bool</returns>
        public static void EnsureSalesReportDataForOrders(Guid orderGuid, Guid moduleGuid, int pageId, int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_CommerceReportOrders ");
            sqlCommand.Append("( ");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("FeatureGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid, "); ;
            sqlCommand.Append("OrderGuid, ");
            sqlCommand.Append("BillingFirstName, ");
            sqlCommand.Append("BillingLastName, ");
            sqlCommand.Append("BillingCompany, ");
            sqlCommand.Append("BillingAddress1, ");
            sqlCommand.Append("BillingAddress2, ");
            sqlCommand.Append("BillingSuburb, ");
            sqlCommand.Append("BillingCity, ");
            sqlCommand.Append("BillingPostalCode, ");
            sqlCommand.Append("BillingState, ");
            sqlCommand.Append("BillingCountry, ");
            sqlCommand.Append("PaymentMethod, ");
            sqlCommand.Append("SubTotal, ");
            sqlCommand.Append("TaxTotal, ");
            sqlCommand.Append("ShippingTotal, ");
            sqlCommand.Append("OrderTotal, ");
            sqlCommand.Append("OrderDateUtc, ");
            sqlCommand.Append("AdminOrderLink, ");
            sqlCommand.Append("UserOrderLink, ");
            sqlCommand.Append("RowCreatedUtc ");
            sqlCommand.Append(") ");

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("o.OrderGuid AS RowGuid, ");
            sqlCommand.Append("s.SiteGuid, ");
            sqlCommand.Append("m.FeatureGuid, ");
            sqlCommand.Append("m.Guid AS ModuleGuid, ");
            sqlCommand.Append("o.UserGuid, ");
            sqlCommand.Append("o.OrderGuid, ");
            sqlCommand.Append("o.BillingFirstName, ");
            sqlCommand.Append("o.BillingLastName, ");
            sqlCommand.Append("o.BillingCompany, ");
            sqlCommand.Append("o.BillingAddress1, ");
            sqlCommand.Append("o.BillingAddress2, ");
            sqlCommand.Append("o.BillingSuburb, ");
            sqlCommand.Append("o.BillingCity, ");
            sqlCommand.Append("o.BillingPostalCode, ");
            sqlCommand.Append("o.BillingState, ");
            sqlCommand.Append("o.BillingCountry, ");
            sqlCommand.Append("o.PaymentMethod, ");
            sqlCommand.Append("o.SubTotal, ");
            sqlCommand.Append("o.TaxTotal, ");
            sqlCommand.Append("o.ShippingTotal, ");
            sqlCommand.Append("o.OrderTotal, ");
            sqlCommand.Append("o.LastModified, ");

            sqlCommand.Append("CONCAT('/WebStore/AdminOrderDetail.aspx?pageid=' ");
            sqlCommand.Append(", " + pageId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;mid=', " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;order=',o.OrderGuid) AS AdminOrderLink, ");

            sqlCommand.Append("CONCAT('/WebStore/OrderDetail.aspx?pageid=' ");
            sqlCommand.Append(", " + pageId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;mid=', " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(", '&amp;order=' , o.OrderGuid) AS UserOrderLink, ");

            sqlCommand.Append("now() As RowCreatedUtc ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("ws_Store s ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Modules m ");
            sqlCommand.Append("ON s.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("ws_Order o ");
            sqlCommand.Append("ON s.Guid = o.StoreGuid ");
            sqlCommand.Append("AND o.StatusGuid <> 'de3b9331-b98f-493f-be5e-926ffe5003bc' "); //cancelled
            // we are still needing to show the receipt for a received order
            // it does not allow downloads until fulfilled
            // if we rebuild reports we leave out received but initially we have to assume it will be completed
            // maybe need to add an order status to the commerce reports
            //sqlCommand.Append("AND o.StatusGuid <> '0db28432-d9a9-423e-84f2-8a94db434643' "); //received
            sqlCommand.Append("AND o.StatusGuid <> '00000000-0000-0000-0000-000000000000' "); //none

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_CommerceReportOrders cr ");
            sqlCommand.Append("ON cr.RowGuid = o.OrderGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("o.OrderGuid = ?OrderGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cr.RowGuid IS NULL ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("m.Guid = ?ModuleGuid ");
            //sqlCommand.Append("AND ");
            //sqlCommand.Append("o.OrderTotal > 0 ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = orderGuid.ToString();

            arParams[1] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleGuid.ToString();

            MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Flags an order as already tracked in google analytics
        /// </summary>
        /// <param name="orderGuid"> orderGuid </param>
        /// <returns>bool</returns>
        public static bool TrackAnalytics(Guid orderGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_Order ");
            sqlCommand.Append("SET AnalyticsTracked = 1 ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("OrderGuid = ?OrderGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = orderGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static IDataReader Get(Guid orderGuid)
        {
            //return Common.DBOrder.Get(orderGuid);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	ws_Order ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("OrderGuid = ?OrderGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = orderGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetProducts(Guid orderGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT DISTINCT  oop.*, ");
            sqlCommand.Append("pd.Name, ");
            sqlCommand.Append("oo.Quantity, ");
            sqlCommand.Append("oo.OfferPrice ");

            sqlCommand.Append("FROM	ws_OrderOfferProduct oop ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("ws_Product pd ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("oop.ProductGuid = pd.Guid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("ws_OrderOffers oo ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("oop.OrderGuid = oo.OrderGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("oop.OfferGuid = oo.OfferGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("oop.OrderGuid = ?OrderGuid ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("pd.Name ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = orderGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetByUser(
            Guid storeGuid,
            Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");

            sqlCommand.Append("FROM	ws_Order ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = ?StoreGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("UserGuid = ?UserGuid ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("Completed DESC ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the ws_Order table.
        /// </summary>
        public static int GetCount(Guid storeGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	ws_Order ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = ?StoreGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        public static IDataReader GetByStore(
            Guid storeGuid,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            int totalPages = 1;
            int totalRows = GetCount(storeGuid);

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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	o.*, ");
            sqlCommand.Append(totalPages.ToString(CultureInfo.InvariantCulture) + " As TotalPages ");
            sqlCommand.Append("FROM	ws_Order o  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("o.StoreGuid = ?StoreGuid ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("o.Created DESC  ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", ?PageSize ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);



        }

        public static IDataReader GetPage(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(storeGuid);

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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	o.*, ");
            sqlCommand.Append(totalPages.ToString(CultureInfo.InvariantCulture) + " As TotalPages ");
            sqlCommand.Append("FROM	ws_Order o  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("o.StoreGuid = ?StoreGuid ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("o.Completed DESC  ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", ?PageSize ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);



        }

        public static IDataReader GetSalesByYearMonth(Guid storeGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("YEAR(Completed) As Y,  ");
            sqlCommand.Append("MONTH(Completed) As M, ");
            sqlCommand.Append("SUM(OrderTotal) As Sales ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("ws_Order ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = ?StoreGuid ");
            
            sqlCommand.Append("GROUP BY YEAR(Completed), MONTH(Completed) ");
            sqlCommand.Append("ORDER BY YEAR(Completed) desc, MONTH(Completed) desc ");
            sqlCommand.Append("LIMIT 13 ");
            sqlCommand.Append("; ");


            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetRevenueSummary(Guid storeGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("o.Name As Name,  ");
            sqlCommand.Append("oo.OfferGuid AS OfferGuid, ");
            sqlCommand.Append("COUNT(*) AS UnitsSold, ");
            sqlCommand.Append("SUM(oo.OfferPrice) AS Revenue ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("ws_OrderOffers oo ");
            sqlCommand.Append("JOIN ws_Offer o ");
            sqlCommand.Append("ON o.Guid = oo.OfferGuid ");
            
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("o.StoreGuid = ?StoreGuid ");
            
            sqlCommand.Append("GROUP BY o.Name, oo.OfferGuid ");
            sqlCommand.Append("ORDER BY SUM(oo.OfferPrice) DESC ");
            
            sqlCommand.Append("; ");


            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static decimal GetAllTimeRevenueTotal(Guid storeGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("SUM(oo.OfferPrice)  ");
           
            sqlCommand.Append("FROM ");
            sqlCommand.Append("ws_OrderOffers oo ");
            sqlCommand.Append("JOIN ws_Offer o ");
            sqlCommand.Append("ON o.Guid = oo.OfferGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("o.StoreGuid = ?StoreGuid ");
            sqlCommand.Append("; ");


            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid;

            decimal result = 0;

            try
            {
                result = Convert.ToDecimal(MySqlHelper.ExecuteScalar(
                    ConnectionString.GetReadConnectionString(),
                    sqlCommand.ToString(),
                    arParams));
            }
            catch (InvalidCastException) { }

            return result;

        }

        public static bool MoveOrder(
            Guid orderGuid,
            Guid newUserGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_Order ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("UserGuid = ?UserGuid ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("OrderGuid = ?OrderGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = newUserGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


    }
}
