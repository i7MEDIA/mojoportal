/// Author:					Joe Audette
/// Created:				2008-02-24
/// Last Modified:		    2012-07-20
/// 
/// This implementation is for MySQL. 
/// 
/// The use and distribution terms for this software are covered by the 
/// GPL (http://www.gnu.org/licenses/gpl.html)
/// which can be found in the file GPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using MySql.Data.MySqlClient;
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
            sqlCommand.Append("INSERT INTO ws_CartOrderInfo (");
            sqlCommand.Append("CartGuid, ");
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
            sqlCommand.Append("DeliveryCompany, ");
            sqlCommand.Append("DeliveryAddress1, ");
            sqlCommand.Append("DeliveryAddress2, ");
            sqlCommand.Append("DeliverySuburb, ");
            sqlCommand.Append("DeliveryCity, ");
            sqlCommand.Append("DeliveryPostalCode, ");
            sqlCommand.Append("DeliveryState, ");
            sqlCommand.Append("DeliveryCountry, ");
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
            sqlCommand.Append("Completed, ");
            sqlCommand.Append("CompletedFromIP, ");
            sqlCommand.Append("TaxZoneGuid, ");
            sqlCommand.Append("CustomerFirstName, ");
            sqlCommand.Append("CustomerLastName, ");
            sqlCommand.Append("DeliveryFirstName, ");
            sqlCommand.Append("DeliveryLastName, ");
            sqlCommand.Append("BillingFirstName, ");
            sqlCommand.Append("BillingLastName )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?CartGuid, ");
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
            sqlCommand.Append("?DeliveryCompany, ");
            sqlCommand.Append("?DeliveryAddress1, ");
            sqlCommand.Append("?DeliveryAddress2, ");
            sqlCommand.Append("?DeliverySuburb, ");
            sqlCommand.Append("?DeliveryCity, ");
            sqlCommand.Append("?DeliveryPostalCode, ");
            sqlCommand.Append("?DeliveryState, ");
            sqlCommand.Append("?DeliveryCountry, ");
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
            sqlCommand.Append("?Completed, ");
            sqlCommand.Append("?CompletedFromIP, ");
            sqlCommand.Append("?TaxZoneGuid, ");
            sqlCommand.Append("?CustomerFirstName, ");
            sqlCommand.Append("?CustomerLastName, ");
            sqlCommand.Append("?DeliveryFirstName, ");
            sqlCommand.Append("?DeliveryLastName, ");
            sqlCommand.Append("?BillingFirstName, ");
            sqlCommand.Append("?BillingLastName )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[43];

            arParams[0] = new MySqlParameter("?CartGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();

            arParams[1] = new MySqlParameter("?CustomerCompany", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = customerCompany;

            arParams[2] = new MySqlParameter("?CustomerAddressLine1", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = customerAddressLine1;

            arParams[3] = new MySqlParameter("?CustomerAddressLine2", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = customerAddressLine2;

            arParams[4] = new MySqlParameter("?CustomerSuburb", MySqlDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = customerSuburb;

            arParams[5] = new MySqlParameter("?CustomerCity", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = customerCity;

            arParams[6] = new MySqlParameter("?CustomerPostalCode", MySqlDbType.VarChar, 20);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = customerPostalCode;

            arParams[7] = new MySqlParameter("?CustomerState", MySqlDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = customerState;

            arParams[8] = new MySqlParameter("?CustomerCountry", MySqlDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = customerCountry;

            arParams[9] = new MySqlParameter("?CustomerTelephoneDay", MySqlDbType.VarChar, 32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = customerTelephoneDay;

            arParams[10] = new MySqlParameter("?CustomerTelephoneNight", MySqlDbType.VarChar, 32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = customerTelephoneNight;

            arParams[11] = new MySqlParameter("?CustomerEmail", MySqlDbType.VarChar, 96);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = customerEmail;

            arParams[12] = new MySqlParameter("?CustomerEmailVerified", MySqlDbType.Int32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = intCustomerEmailVerified;

            arParams[13] = new MySqlParameter("?DeliveryCompany", MySqlDbType.VarChar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = deliveryCompany;

            arParams[14] = new MySqlParameter("?DeliveryAddress1", MySqlDbType.VarChar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = deliveryAddress1;

            arParams[15] = new MySqlParameter("?DeliveryAddress2", MySqlDbType.VarChar, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = deliveryAddress2;

            arParams[16] = new MySqlParameter("?DeliverySuburb", MySqlDbType.VarChar, 255);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = deliverySuburb;

            arParams[17] = new MySqlParameter("?DeliveryCity", MySqlDbType.VarChar, 255);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = deliveryCity;

            arParams[18] = new MySqlParameter("?DeliveryPostalCode", MySqlDbType.VarChar, 20);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = deliveryPostalCode;

            arParams[19] = new MySqlParameter("?DeliveryState", MySqlDbType.VarChar, 255);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = deliveryState;

            arParams[20] = new MySqlParameter("?DeliveryCountry", MySqlDbType.VarChar, 255);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = deliveryCountry;

            arParams[21] = new MySqlParameter("?BillingCompany", MySqlDbType.VarChar, 255);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = billingCompany;

            arParams[22] = new MySqlParameter("?BillingAddress1", MySqlDbType.VarChar, 255);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = billingAddress1;

            arParams[23] = new MySqlParameter("?BillingAddress2", MySqlDbType.VarChar, 255);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = billingAddress2;

            arParams[24] = new MySqlParameter("?BillingSuburb", MySqlDbType.VarChar, 255);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = billingSuburb;

            arParams[25] = new MySqlParameter("?BillingCity", MySqlDbType.VarChar, 255);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = billingCity;

            arParams[26] = new MySqlParameter("?BillingPostalCode", MySqlDbType.VarChar, 20);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = billingPostalCode;

            arParams[27] = new MySqlParameter("?BillingState", MySqlDbType.VarChar, 255);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = billingState;

            arParams[28] = new MySqlParameter("?BillingCountry", MySqlDbType.VarChar, 255);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = billingCountry;

            arParams[29] = new MySqlParameter("?CardTypeGuid", MySqlDbType.VarChar, 36);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = cardTypeGuid.ToString();

            arParams[30] = new MySqlParameter("?CardOwner", MySqlDbType.VarChar, 100);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = cardOwner;

            arParams[31] = new MySqlParameter("?CardNumber", MySqlDbType.VarChar, 255);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = cardNumber;

            arParams[32] = new MySqlParameter("?CardExpires", MySqlDbType.VarChar, 6);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = cardExpires;

            arParams[33] = new MySqlParameter("?CardSecurityCode", MySqlDbType.VarChar, 50);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = cardSecurityCode;

            arParams[34] = new MySqlParameter("?Completed", MySqlDbType.DateTime);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = completed;

            arParams[35] = new MySqlParameter("?CompletedFromIP", MySqlDbType.VarChar, 255);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = completedFromIP;

            arParams[36] = new MySqlParameter("?TaxZoneGuid", MySqlDbType.VarChar, 36);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = taxZoneGuid.ToString();

            arParams[37] = new MySqlParameter("?CustomerFirstName", MySqlDbType.VarChar, 100);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = customerFirstName;

            arParams[38] = new MySqlParameter("?CustomerLastName", MySqlDbType.VarChar, 100);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = customerLastName;

            arParams[39] = new MySqlParameter("?DeliveryFirstName", MySqlDbType.VarChar, 100);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = deliveryFirstName;

            arParams[40] = new MySqlParameter("?DeliveryLastName", MySqlDbType.VarChar, 100);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = deliveryLastName;

            arParams[41] = new MySqlParameter("?BillingFirstName", MySqlDbType.VarChar, 100);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = billingFirstName;

            arParams[42] = new MySqlParameter("?BillingLastName", MySqlDbType.VarChar, 100);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = billingLastName;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
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
            sqlCommand.Append("UPDATE ws_CartOrderInfo ");
            sqlCommand.Append("SET  ");
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
            sqlCommand.Append("DeliveryCompany = ?DeliveryCompany, ");
            sqlCommand.Append("DeliveryAddress1 = ?DeliveryAddress1, ");
            sqlCommand.Append("DeliveryAddress2 = ?DeliveryAddress2, ");
            sqlCommand.Append("DeliverySuburb = ?DeliverySuburb, ");
            sqlCommand.Append("DeliveryCity = ?DeliveryCity, ");
            sqlCommand.Append("DeliveryPostalCode = ?DeliveryPostalCode, ");
            sqlCommand.Append("DeliveryState = ?DeliveryState, ");
            sqlCommand.Append("DeliveryCountry = ?DeliveryCountry, ");
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
            sqlCommand.Append("Completed = ?Completed, ");
            sqlCommand.Append("CompletedFromIP = ?CompletedFromIP, ");
            sqlCommand.Append("TaxZoneGuid = ?TaxZoneGuid, ");
            sqlCommand.Append("CustomerFirstName = ?CustomerFirstName, ");
            sqlCommand.Append("CustomerLastName = ?CustomerLastName, ");
            sqlCommand.Append("DeliveryFirstName = ?DeliveryFirstName, ");
            sqlCommand.Append("DeliveryLastName = ?DeliveryLastName, ");
            sqlCommand.Append("BillingFirstName = ?BillingFirstName, ");
            sqlCommand.Append("BillingLastName = ?BillingLastName ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("CartGuid = ?CartGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[43];

            arParams[0] = new MySqlParameter("?CartGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();

            arParams[1] = new MySqlParameter("?CustomerCompany", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = customerCompany;

            arParams[2] = new MySqlParameter("?CustomerAddressLine1", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = customerAddressLine1;

            arParams[3] = new MySqlParameter("?CustomerAddressLine2", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = customerAddressLine2;

            arParams[4] = new MySqlParameter("?CustomerSuburb", MySqlDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = customerSuburb;

            arParams[5] = new MySqlParameter("?CustomerCity", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = customerCity;

            arParams[6] = new MySqlParameter("?CustomerPostalCode", MySqlDbType.VarChar, 20);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = customerPostalCode;

            arParams[7] = new MySqlParameter("?CustomerState", MySqlDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = customerState;

            arParams[8] = new MySqlParameter("?CustomerCountry", MySqlDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = customerCountry;

            arParams[9] = new MySqlParameter("?CustomerTelephoneDay", MySqlDbType.VarChar, 32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = customerTelephoneDay;

            arParams[10] = new MySqlParameter("?CustomerTelephoneNight", MySqlDbType.VarChar, 32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = customerTelephoneNight;

            arParams[11] = new MySqlParameter("?CustomerEmail", MySqlDbType.VarChar, 96);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = customerEmail;

            arParams[12] = new MySqlParameter("?CustomerEmailVerified", MySqlDbType.Int32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = intCustomerEmailVerified;

            arParams[13] = new MySqlParameter("?DeliveryCompany", MySqlDbType.VarChar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = deliveryCompany;

            arParams[14] = new MySqlParameter("?DeliveryAddress1", MySqlDbType.VarChar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = deliveryAddress1;

            arParams[15] = new MySqlParameter("?DeliveryAddress2", MySqlDbType.VarChar, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = deliveryAddress2;

            arParams[16] = new MySqlParameter("?DeliverySuburb", MySqlDbType.VarChar, 255);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = deliverySuburb;

            arParams[17] = new MySqlParameter("?DeliveryCity", MySqlDbType.VarChar, 255);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = deliveryCity;

            arParams[18] = new MySqlParameter("?DeliveryPostalCode", MySqlDbType.VarChar, 20);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = deliveryPostalCode;

            arParams[19] = new MySqlParameter("?DeliveryState", MySqlDbType.VarChar, 255);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = deliveryState;

            arParams[20] = new MySqlParameter("?DeliveryCountry", MySqlDbType.VarChar, 255);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = deliveryCountry;

            arParams[21] = new MySqlParameter("?BillingCompany", MySqlDbType.VarChar, 255);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = billingCompany;

            arParams[22] = new MySqlParameter("?BillingAddress1", MySqlDbType.VarChar, 255);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = billingAddress1;

            arParams[23] = new MySqlParameter("?BillingAddress2", MySqlDbType.VarChar, 255);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = billingAddress2;

            arParams[24] = new MySqlParameter("?BillingSuburb", MySqlDbType.VarChar, 255);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = billingSuburb;

            arParams[25] = new MySqlParameter("?BillingCity", MySqlDbType.VarChar, 255);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = billingCity;

            arParams[26] = new MySqlParameter("?BillingPostalCode", MySqlDbType.VarChar, 20);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = billingPostalCode;

            arParams[27] = new MySqlParameter("?BillingState", MySqlDbType.VarChar, 255);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = billingState;

            arParams[28] = new MySqlParameter("?BillingCountry", MySqlDbType.VarChar, 255);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = billingCountry;

            arParams[29] = new MySqlParameter("?CardTypeGuid", MySqlDbType.VarChar, 36);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = cardTypeGuid.ToString();

            arParams[30] = new MySqlParameter("?CardOwner", MySqlDbType.VarChar, 100);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = cardOwner;

            arParams[31] = new MySqlParameter("?CardNumber", MySqlDbType.VarChar, 255);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = cardNumber;

            arParams[32] = new MySqlParameter("?CardExpires", MySqlDbType.VarChar, 6);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = cardExpires;

            arParams[33] = new MySqlParameter("?CardSecurityCode", MySqlDbType.VarChar, 50);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = cardSecurityCode;

            arParams[34] = new MySqlParameter("?Completed", MySqlDbType.DateTime);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = completed;

            arParams[35] = new MySqlParameter("?CompletedFromIP", MySqlDbType.VarChar, 255);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = completedFromIP;

            arParams[36] = new MySqlParameter("?TaxZoneGuid", MySqlDbType.VarChar, 36);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = taxZoneGuid.ToString();

            arParams[37] = new MySqlParameter("?CustomerFirstName", MySqlDbType.VarChar, 100);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = customerFirstName;

            arParams[38] = new MySqlParameter("?CustomerLastName", MySqlDbType.VarChar, 100);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = customerLastName;

            arParams[39] = new MySqlParameter("?DeliveryFirstName", MySqlDbType.VarChar, 100);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = deliveryFirstName;

            arParams[40] = new MySqlParameter("?DeliveryLastName", MySqlDbType.VarChar, 100);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = deliveryLastName;

            arParams[41] = new MySqlParameter("?BillingFirstName", MySqlDbType.VarChar, 100);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = billingFirstName;

            arParams[42] = new MySqlParameter("?BillingLastName", MySqlDbType.VarChar, 100);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = billingLastName;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


            


        }

        public static bool Delete(Guid cartGuid)
        {
     
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_CartOrderInfo ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CartGuid = ?CartGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?CartGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        public static bool DeleteAnonymousByStore(Guid storeGuid, DateTime olderThan)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_CartOrderInfo ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CartGuid ");
            sqlCommand.Append(" IN (");
            sqlCommand.Append("SELECT CartGuid ");
            sqlCommand.Append("FROM ws_Cart ");
            sqlCommand.Append("WHERE	StoreGuid = ?StoreGuid ");
            sqlCommand.Append("AND UserGuid = '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("AND Created < ?OlderThan ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            arParams[1] = new MySqlParameter("?OlderThan", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = olderThan;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByStore(Guid storeGuid, DateTime olderThan)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_CartOrderInfo ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CartGuid ");
            sqlCommand.Append(" IN (");
            sqlCommand.Append("SELECT CartGuid ");
            sqlCommand.Append("FROM ws_Cart ");
            sqlCommand.Append("WHERE	StoreGuid = ?StoreGuid ");
            sqlCommand.Append("AND Created < ?OlderThan ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            arParams[1] = new MySqlParameter("?OlderThan", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = olderThan;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }


        public static IDataReader Get(Guid cartGuid)
        {
           
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	ws_CartOrderInfo ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CartGuid = ?CartGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?CartGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }


    }
}
