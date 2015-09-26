// Author:					Joe Audette
// Created:					2011-11-02
// Last Modified:			2014-01-28
// 


using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using MySql.Data.MySqlClient;

namespace sts.SiteMembership.Data
{

    public static class DBMembershipTicket
    {
        
        private static String GetReadConnectionString()
        {
            return ConfigurationManager.AppSettings["MySqlConnectionString"];

        }

        private static String GetWriteConnectionString()
        {
            if (ConfigurationManager.AppSettings["MySqlWriteConnectionString"] != null)
            {
                return ConfigurationManager.AppSettings["MySqlWriteConnectionString"];
            }

            return ConfigurationManager.AppSettings["MySqlConnectionString"];
        }



        /// <summary>
        /// Inserts a row in the sts_MembershipTicket table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="productGuid"> productGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="beginDateUtc"> beginDateUtc </param>
        /// <param name="endDateUtc"> endDateUtc </param>
        /// <param name="grantedRoles"> grantedRoles </param>
        /// <param name="qty"> qty </param>
        /// <param name="price"> price </param>
        /// <param name="tax"> tax </param>
        /// <param name="orderTotal"> orderTotal </param>
        /// <param name="processingFeed"> processingFeed </param>
        /// <param name="paymentMethod"> paymentMethod </param>
        /// <param name="analyticsTracked"> analyticsTracked </param>
        /// <param name="customerFirstName"> customerFirstName </param>
        /// <param name="customerLastName"> customerLastName </param>
        /// <param name="customerCompany"> customerCompany </param>
        /// <param name="customerEmail"> customerEmail </param>
        /// <param name="customerPhone"> customerPhone </param>
        /// <param name="customerAddress1"> customerAddress1 </param>
        /// <param name="customerAddress2"> customerAddress2 </param>
        /// <param name="customerSuburb"> customerSuburb </param>
        /// <param name="customerCity"> customerCity </param>
        /// <param name="customerState"> customerState </param>
        /// <param name="customerPostalCode"> customerPostalCode </param>
        /// <param name="customerCountry"> customerCountry </param>
        /// <param name="taxZoneGuid"> taxZoneGuid </param>
        /// <param name="currencyGuid"> currencyGuid </param>
        /// <param name="createdFromIP"> createdFromIP </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="orderStatusGuid"> orderStatusGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid productGuid,
            Guid memberDefinitionGuid,
            Guid userGuid,
            Guid moduleGuid,
            DateTime beginDateUtc,
            DateTime endDateUtc,
            string grantedRoles,
            decimal price,
            decimal tax,
            decimal orderTotal,
            decimal processingFee,
            string paymentMethod,
            bool analyticsTracked,
            string customerFirstName,
            string customerLastName,
            string customerCompany,
            string customerEmail,
            string customerPhone,
            string customerAddress1,
            string customerAddress2,
            string customerSuburb,
            string customerCity,
            string customerState,
            string customerPostalCode,
            string customerCountry,
            Guid taxZoneGuid,
            Guid currencyGuid,
            string createdFromIP,
            DateTime createdUtc,
            Guid orderStatusGuid,
            Guid siteGuid,
            int durationInDays,
            int gracePeriodDays,
            string productTitle,
            string productDescription,
            string memberDefinitionName,
            string memberDefinitionDescription,
            bool isRenewal)
        {

            #region Bit Conversion

            int intAnalyticsTracked = 0;
            if (analyticsTracked) { intAnalyticsTracked = 1; }

            int intisRenewal = 0;
            if (isRenewal) { intisRenewal = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_MembershipTicket (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("ProductGuid, ");
            sqlCommand.Append("MemberDefGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("BeginDateUtc, ");
            sqlCommand.Append("EndDateUtc, ");
            sqlCommand.Append("GrantedRoles, ");
            sqlCommand.Append("Qty, ");
            sqlCommand.Append("Price, ");
            sqlCommand.Append("Tax, ");
            sqlCommand.Append("OrderTotal, ");
            sqlCommand.Append("ProcessingFee, ");
            sqlCommand.Append("PaymentMethod, ");
            sqlCommand.Append("AnalyticsTracked, ");
            sqlCommand.Append("CustomerFirstName, ");
            sqlCommand.Append("CustomerLastName, ");
            sqlCommand.Append("CustomerCompany, ");
            sqlCommand.Append("CustomerEmail, ");
            sqlCommand.Append("CustomerPhone, ");
            sqlCommand.Append("CustomerAddress1, ");
            sqlCommand.Append("CustomerAddress2, ");
            sqlCommand.Append("CustomerSuburb, ");
            sqlCommand.Append("CustomerCity, ");
            sqlCommand.Append("CustomerState, ");
            sqlCommand.Append("CustomerPostalCode, ");
            sqlCommand.Append("CustomerCountry, ");
            sqlCommand.Append("TaxZoneGuid, ");
            sqlCommand.Append("CurrencyGuid, ");
            sqlCommand.Append("CreatedFromIP, ");
            sqlCommand.Append("LastModifiedFromIP, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("LastModifiedUtc, ");
            sqlCommand.Append("OrderStatusGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("DurationInDays, ");
            sqlCommand.Append("GracePeriodDays, ");
            sqlCommand.Append("ProdTitle, ");
            sqlCommand.Append("ProdDesc, ");
            sqlCommand.Append("MemDefName, ");
            sqlCommand.Append("MemDefDesc, ");
            sqlCommand.Append("IsRenewal ");
            sqlCommand.Append(") ");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?ProductGuid, ");
            sqlCommand.Append("?MemberDefGuid, ");
            sqlCommand.Append("?UserGuid, ");
            sqlCommand.Append("?ModuleGuid, ");
            sqlCommand.Append("?BeginDateUtc, ");
            sqlCommand.Append("?EndDateUtc, ");
            sqlCommand.Append("?GrantedRoles, ");
            sqlCommand.Append("?Qty, ");
            sqlCommand.Append("?Price, ");
            sqlCommand.Append("?Tax, ");
            sqlCommand.Append("?OrderTotal, ");
            sqlCommand.Append("?ProcessingFee, ");
            sqlCommand.Append("?PaymentMethod, ");
            sqlCommand.Append("?AnalyticsTracked, ");
            sqlCommand.Append("?CustomerFirstName, ");
            sqlCommand.Append("?CustomerLastName, ");
            sqlCommand.Append("?CustomerCompany, ");
            sqlCommand.Append("?CustomerEmail, ");
            sqlCommand.Append("?CustomerPhone, ");
            sqlCommand.Append("?CustomerAddress1, ");
            sqlCommand.Append("?CustomerAddress2, ");
            sqlCommand.Append("?CustomerSuburb, ");
            sqlCommand.Append("?CustomerCity, ");
            sqlCommand.Append("?CustomerState, ");
            sqlCommand.Append("?CustomerPostalCode, ");
            sqlCommand.Append("?CustomerCountry, ");
            sqlCommand.Append("?TaxZoneGuid, ");
            sqlCommand.Append("?CurrencyGuid, ");
            sqlCommand.Append("?CreatedFromIP, ");
            sqlCommand.Append("?LastModifiedFromIP, ");
            sqlCommand.Append("?CreatedUtc, ");
            sqlCommand.Append("?LastModifiedUtc, ");
            sqlCommand.Append("?OrderStatusGuid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?DurationInDays, ");
            sqlCommand.Append("?GracePeriodDays, ");
            sqlCommand.Append("?ProdTitle, ");
            sqlCommand.Append("?ProdDesc, ");
            sqlCommand.Append("?MemDefName, ");
            sqlCommand.Append("?MemDefDesc, ");
            sqlCommand.Append("IsRenewal ");
            sqlCommand.Append(") ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[42];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = productGuid.ToString();

            arParams[2] = new MySqlParameter("?MemberDefGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = memberDefinitionGuid.ToString();

            arParams[3] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = moduleGuid.ToString();

            arParams[5] = new MySqlParameter("?BeginDateUtc", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            if (beginDateUtc > DateTime.MinValue)
            {
                arParams[5].Value = beginDateUtc;
            }
            else
            {
                arParams[5].Value = DBNull.Value;
            }

            arParams[6] = new MySqlParameter("?EndDateUtc", MySqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            if (endDateUtc > DateTime.MinValue)
            {
                arParams[6].Value = endDateUtc;
            }
            else
            {
                arParams[6].Value = DBNull.Value;
            }

            arParams[7] = new MySqlParameter("?GrantedRoles", MySqlDbType.Text);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = grantedRoles;

            arParams[8] = new MySqlParameter("?Qty", MySqlDbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = 1;

            arParams[9] = new MySqlParameter("?Price", MySqlDbType.Decimal);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = price;

            arParams[10] = new MySqlParameter("?Tax", MySqlDbType.Decimal);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = tax;

            arParams[11] = new MySqlParameter("?OrderTotal", MySqlDbType.Decimal);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = orderTotal;

            arParams[12] = new MySqlParameter("?ProcessingFee", MySqlDbType.Decimal);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = processingFee;

            arParams[13] = new MySqlParameter("?PaymentMethod", MySqlDbType.VarChar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = paymentMethod;

            arParams[14] = new MySqlParameter("?AnalyticsTracked", MySqlDbType.Int32);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = intAnalyticsTracked;

            arParams[15] = new MySqlParameter("?CustomerFirstName", MySqlDbType.VarChar, 100);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = customerFirstName;

            arParams[16] = new MySqlParameter("?CustomerLastName", MySqlDbType.VarChar, 100);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = customerLastName;

            arParams[17] = new MySqlParameter("?CustomerCompany", MySqlDbType.VarChar, 255);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = customerCompany;

            arParams[18] = new MySqlParameter("?CustomerEmail", MySqlDbType.VarChar, 100);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = customerEmail;

            arParams[19] = new MySqlParameter("?CustomerPhone", MySqlDbType.VarChar, 32);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = customerPhone;

            arParams[20] = new MySqlParameter("?CustomerAddress1", MySqlDbType.VarChar, 255);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = customerAddress1;

            arParams[21] = new MySqlParameter("?CustomerAddress2", MySqlDbType.VarChar, 255);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = customerAddress2;

            arParams[22] = new MySqlParameter("?CustomerSuburb", MySqlDbType.VarChar, 255);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = customerSuburb;

            arParams[23] = new MySqlParameter("?CustomerCity", MySqlDbType.VarChar, 255);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = customerCity;

            arParams[24] = new MySqlParameter("?CustomerState", MySqlDbType.VarChar, 255);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = customerState;

            arParams[25] = new MySqlParameter("?CustomerPostalCode", MySqlDbType.VarChar, 20);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = customerPostalCode;

            arParams[26] = new MySqlParameter("?CustomerCountry", MySqlDbType.VarChar, 255);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = customerCountry;

            arParams[27] = new MySqlParameter("?TaxZoneGuid", MySqlDbType.VarChar, 36);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = taxZoneGuid.ToString();

            arParams[28] = new MySqlParameter("?CurrencyGuid", MySqlDbType.VarChar, 36);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = currencyGuid.ToString();

            arParams[29] = new MySqlParameter("?CreatedFromIP", MySqlDbType.VarChar, 255);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = createdFromIP;

            arParams[30] = new MySqlParameter("?LastModifiedFromIP", MySqlDbType.VarChar, 255);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = createdFromIP;

            arParams[31] = new MySqlParameter("?CreatedUtc", MySqlDbType.DateTime);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = createdUtc;

            arParams[32] = new MySqlParameter("?LastModifiedUtc", MySqlDbType.DateTime);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = createdUtc;

            arParams[33] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = orderStatusGuid.ToString();

            arParams[34] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = siteGuid.ToString();

            arParams[35] = new MySqlParameter("?DurationInDays", MySqlDbType.Int32);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = durationInDays;

            arParams[36] = new MySqlParameter("?GracePeriodDays", MySqlDbType.Int32);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = gracePeriodDays;

            arParams[37] = new MySqlParameter("?ProdTitle", MySqlDbType.VarChar, 255);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = productTitle;

            arParams[38] = new MySqlParameter("?ProdDesc", MySqlDbType.Text);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = productDescription;

            arParams[39] = new MySqlParameter("?MemDefName", MySqlDbType.VarChar, 255);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = memberDefinitionName;

            arParams[40] = new MySqlParameter("?MemDefDesc", MySqlDbType.Text);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = memberDefinitionDescription;

            arParams[41] = new MySqlParameter("?IsRenewal", MySqlDbType.Int32);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = intisRenewal;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

           

        }


        /// <summary>
        /// Updates a row in the sts_MembershipTicket table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="productGuid"> productGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="beginDateUtc"> beginDateUtc </param>
        /// <param name="endDateUtc"> endDateUtc </param>
        /// <param name="grantedRoles"> grantedRoles </param>
        /// <param name="price"> price </param>
        /// <param name="tax"> tax </param>
        /// <param name="orderTotal"> orderTotal </param>
        /// <param name="processingFee"> processingFee </param>
        /// <param name="paymentMethod"> paymentMethod </param>
        /// <param name="analyticsTracked"> analyticsTracked </param>
        /// <param name="customerFirstName"> customerFirstName </param>
        /// <param name="customerLastName"> customerLastName </param>
        /// <param name="customerCompany"> customerCompany </param>
        /// <param name="customerEmail"> customerEmail </param>
        /// <param name="customerPhone"> customerPhone </param>
        /// <param name="customerAddress1"> customerAddress1 </param>
        /// <param name="customerAddress2"> customerAddress2 </param>
        /// <param name="customerSuburb"> customerSuburb </param>
        /// <param name="customerCity"> customerCity </param>
        /// <param name="customerState"> customerState </param>
        /// <param name="customerPostalCode"> customerPostalCode </param>
        /// <param name="customerCountry"> customerCountry </param>
        /// <param name="taxZoneGuid"> taxZoneGuid </param>
        /// <param name="currencyGuid"> currencyGuid </param>
        /// <param name="createdFromIP"> createdFromIP </param>
        /// <param name="lastModifiedFromIP"> lastModifiedFromIP </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="lastModifiedUtc"> lastModifiedUtc </param>
        /// <param name="orderStatusGuid"> orderStatusGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            Guid productGuid,
            Guid userGuid,
            Guid moduleGuid,
            DateTime beginDateUtc,
            DateTime endDateUtc,
            string grantedRoles,
            decimal price,
            decimal tax,
            decimal orderTotal,
            decimal processingFee,
            string paymentMethod,
            bool analyticsTracked,
            string customerFirstName,
            string customerLastName,
            string customerCompany,
            string customerEmail,
            string customerPhone,
            string customerAddress1,
            string customerAddress2,
            string customerSuburb,
            string customerCity,
            string customerState,
            string customerPostalCode,
            string customerCountry,
            Guid taxZoneGuid,
            Guid currencyGuid,
            string lastModifiedFromIP,
            DateTime lastModifiedUtc,
            Guid orderStatusGuid,
            int durationInDays,
            int gracePeriodDays,
            string productTitle,
            string productDescription,
            string memberDefinitionName,
            string memberDefinitionDescription,
            bool isRenewal)
        {
            #region Bit Conversion

            int intAnalyticsTracked = 0;
            if (analyticsTracked) { intAnalyticsTracked = 1; }

            int intisRenewal = 0;
            if (isRenewal) { intisRenewal = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_MembershipTicket ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("ProductGuid = ?ProductGuid, ");
            sqlCommand.Append("UserGuid = ?UserGuid, ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid, ");
            sqlCommand.Append("BeginDateUtc = ?BeginDateUtc, ");
            sqlCommand.Append("EndDateUtc = ?EndDateUtc, ");
            sqlCommand.Append("GrantedRoles = ?GrantedRoles, ");
            sqlCommand.Append("Price = ?Price, ");
            sqlCommand.Append("Tax = ?Tax, ");
            sqlCommand.Append("OrderTotal = ?OrderTotal, ");
            sqlCommand.Append("ProcessingFee = ?ProcessingFee, ");
            sqlCommand.Append("PaymentMethod = ?PaymentMethod, ");
            sqlCommand.Append("AnalyticsTracked = ?AnalyticsTracked, ");
            sqlCommand.Append("CustomerFirstName = ?CustomerFirstName, ");
            sqlCommand.Append("CustomerLastName = ?CustomerLastName, ");
            sqlCommand.Append("CustomerCompany = ?CustomerCompany, ");
            sqlCommand.Append("CustomerEmail = ?CustomerEmail, ");
            sqlCommand.Append("CustomerPhone = ?CustomerPhone, ");
            sqlCommand.Append("CustomerAddress1 = ?CustomerAddress1, ");
            sqlCommand.Append("CustomerAddress2 = ?CustomerAddress2, ");
            sqlCommand.Append("CustomerSuburb = ?CustomerSuburb, ");
            sqlCommand.Append("CustomerCity = ?CustomerCity, ");
            sqlCommand.Append("CustomerState = ?CustomerState, ");
            sqlCommand.Append("CustomerPostalCode = ?CustomerPostalCode, ");
            sqlCommand.Append("CustomerCountry = ?CustomerCountry, ");
            sqlCommand.Append("TaxZoneGuid = ?TaxZoneGuid, ");
            sqlCommand.Append("CurrencyGuid = ?CurrencyGuid, ");
            sqlCommand.Append("LastModifiedFromIP = ?LastModifiedFromIP, ");
            sqlCommand.Append("LastModifiedUtc = ?LastModifiedUtc, ");
            sqlCommand.Append("OrderStatusGuid = ?OrderStatusGuid, ");
            sqlCommand.Append("DurationInDays = ?DurationInDays, ");
            sqlCommand.Append("GracePeriodDays = ?GracePeriodDays, ");
            sqlCommand.Append("ProdTitle = ?ProdTitle, ");
            sqlCommand.Append("ProdDesc = ?ProdDesc, ");
            sqlCommand.Append("MemDefName = ?MemDefName, ");
            sqlCommand.Append("MemDefDesc = ?MemDefDesc ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[37];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = productGuid.ToString();

            arParams[2] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid.ToString();

            arParams[3] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moduleGuid.ToString();

            arParams[4] = new MySqlParameter("?BeginDateUtc", MySqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            if (beginDateUtc > DateTime.MinValue)
            {
                arParams[4].Value = beginDateUtc;
            }
            else
            {
                arParams[4].Value = DBNull.Value;
            }

            arParams[5] = new MySqlParameter("?EndDateUtc", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            if (endDateUtc > DateTime.MinValue)
            {
                arParams[5].Value = endDateUtc;
            }
            else
            {
                arParams[5].Value = DBNull.Value;
            }

            arParams[6] = new MySqlParameter("?GrantedRoles", MySqlDbType.Text);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = grantedRoles;

            arParams[7] = new MySqlParameter("?Price", MySqlDbType.Decimal);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = price;

            arParams[8] = new MySqlParameter("?Tax", MySqlDbType.Decimal);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = tax;

            arParams[9] = new MySqlParameter("?OrderTotal", MySqlDbType.Decimal);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = orderTotal;

            arParams[10] = new MySqlParameter("?ProcessingFee", MySqlDbType.Decimal);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = processingFee;

            arParams[11] = new MySqlParameter("?PaymentMethod", MySqlDbType.VarChar, 50);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = paymentMethod;

            arParams[12] = new MySqlParameter("?AnalyticsTracked", MySqlDbType.Int32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = intAnalyticsTracked;

            arParams[13] = new MySqlParameter("?CustomerFirstName", MySqlDbType.VarChar, 100);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = customerFirstName;

            arParams[14] = new MySqlParameter("?CustomerLastName", MySqlDbType.VarChar, 100);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = customerLastName;

            arParams[15] = new MySqlParameter("?CustomerCompany", MySqlDbType.VarChar, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = customerCompany;

            arParams[16] = new MySqlParameter("?CustomerEmail", MySqlDbType.VarChar, 100);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = customerEmail;

            arParams[17] = new MySqlParameter("?CustomerPhone", MySqlDbType.VarChar, 32);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = customerPhone;

            arParams[18] = new MySqlParameter("?CustomerAddress1", MySqlDbType.VarChar, 255);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = customerAddress1;

            arParams[19] = new MySqlParameter("?CustomerAddress2", MySqlDbType.VarChar, 255);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = customerAddress2;

            arParams[20] = new MySqlParameter("?CustomerSuburb", MySqlDbType.VarChar, 255);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = customerSuburb;

            arParams[21] = new MySqlParameter("?CustomerCity", MySqlDbType.VarChar, 255);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = customerCity;

            arParams[22] = new MySqlParameter("?CustomerState", MySqlDbType.VarChar, 255);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = customerState;

            arParams[23] = new MySqlParameter("?CustomerPostalCode", MySqlDbType.VarChar, 20);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = customerPostalCode;

            arParams[24] = new MySqlParameter("?CustomerCountry", MySqlDbType.VarChar, 255);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = customerCountry;

            arParams[25] = new MySqlParameter("?TaxZoneGuid", MySqlDbType.VarChar, 36);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = taxZoneGuid.ToString();

            arParams[26] = new MySqlParameter("?CurrencyGuid", MySqlDbType.VarChar, 36);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = currencyGuid.ToString();

            arParams[27] = new MySqlParameter("?LastModifiedFromIP", MySqlDbType.VarChar, 255);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = lastModifiedFromIP;

            arParams[28] = new MySqlParameter("?LastModifiedUtc", MySqlDbType.DateTime);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = lastModifiedUtc;

            arParams[29] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = orderStatusGuid.ToString();

            arParams[30] = new MySqlParameter("?DurationInDays", MySqlDbType.Int32);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = durationInDays;

            arParams[31] = new MySqlParameter("?GracePeriodDays", MySqlDbType.Int32);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = gracePeriodDays;

            arParams[32] = new MySqlParameter("?ProdTitle", MySqlDbType.VarChar, 255);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = productTitle;

            arParams[33] = new MySqlParameter("?ProdDesc", MySqlDbType.Text);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = productDescription;

            arParams[34] = new MySqlParameter("?MemDefName", MySqlDbType.VarChar, 255);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = memberDefinitionName;

            arParams[35] = new MySqlParameter("?MemDefDesc", MySqlDbType.Text);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = memberDefinitionDescription;

            arParams[36] = new MySqlParameter("?IsRenewal", MySqlDbType.Int32);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = intisRenewal;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the sts_MembershipTicket table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_MembershipTicket ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the sts_MembershipTicket table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByProduct(Guid productGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_MembershipTicket ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ProductGuid = ?ProductGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = productGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes rows from the sts_MembershipTicket table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_MembershipTicket ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);
        }

        public static bool FlagUserRolesChanged(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Users ");
            sqlCommand.Append("SET RolesChanged = 1 ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = ?UserGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the sts_MembershipTicket table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  mt.*, ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LastActivityDate, ");
            sqlCommand.Append("u.TimeZoneId ");

            sqlCommand.Append("FROM	sts_MembershipTicket mt ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON mt.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("mt.Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets the requested row if it belongs to the user and site and has no order status
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="userGuid"></param>
        /// <param name="siteGuid"></param>
        /// <returns></returns>
        public static IDataReader GetCart(Guid guid, Guid userGuid, Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  mt.*, ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LastActivityDate, ");
            sqlCommand.Append("u.TimeZoneId ");

            sqlCommand.Append("FROM	sts_MembershipTicket mt ");

            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON mt.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("mt.Guid = ?Guid ");
            sqlCommand.Append("AND mt.UserGuid = ?UserGuid ");
            sqlCommand.Append("AND mt.SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND mt.OrderStatusGuid = '00000000-0000-0000-0000-000000000000' ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetMostRecentCart(Guid userGuid, Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  mt.*, ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LastActivityDate, ");
            sqlCommand.Append("u.TimeZoneId ");

            sqlCommand.Append("FROM	sts_MembershipTicket mt ");

            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON mt.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("mt.UserGuid = ?UserGuid ");
            sqlCommand.Append("AND mt.SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND mt.OrderStatusGuid = '00000000-0000-0000-0000-000000000000' ");

            sqlCommand.Append("ORDER BY mt.CreatedUtc DESC ");
            sqlCommand.Append("LIMIT 1 ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetMostRecentByStatus(Guid userGuid, Guid siteGuid, Guid orderStatusGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  mt.*, ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LastActivityDate, ");
            sqlCommand.Append("u.TimeZoneId ");

            sqlCommand.Append("FROM	sts_MembershipTicket mt ");

            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON mt.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("mt.UserGuid = ?UserGuid ");
            sqlCommand.Append("AND mt.SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND mt.OrderStatusGuid <> '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("AND (?OrderStatusGuid = '11111111-1111-1111-1111-111111111111' OR OrderStatusGuid = ?OrderStatusGuid) ");

            sqlCommand.Append("ORDER BY mt.CreatedUtc DESC ");
            sqlCommand.Append("LIMIT 1 ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = orderStatusGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByUserAndStatus(Guid userGuid, Guid siteGuid, Guid orderStatusGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  mt.*, ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LastActivityDate, ");
            sqlCommand.Append("u.TimeZoneId ");

            sqlCommand.Append("FROM	sts_MembershipTicket mt ");

            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON mt.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("mt.UserGuid = ?UserGuid ");
            sqlCommand.Append("AND mt.SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND mt.OrderStatusGuid <> '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("AND (?OrderStatusGuid = '11111111-1111-1111-1111-111111111111' OR OrderStatusGuid = ?OrderStatusGuid) ");

            sqlCommand.Append("ORDER BY mt.CreatedUtc DESC ");
            

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = orderStatusGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetActiveTicketsByUser(Guid userGuid, Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  mt.*, ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LastActivityDate, ");
            sqlCommand.Append("u.TimeZoneId ");

            sqlCommand.Append("FROM	sts_MembershipTicket mt ");

            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON mt.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("mt.UserGuid = ?UserGuid ");
            sqlCommand.Append("AND mt.SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND mt.OrderStatusGuid = '70443443-f665-42c9-b69f-48cbf011a14b' "); //fulfillable status
            sqlCommand.Append("AND ?CurrentTimeUtc  < TIMESTAMPADD(DAY, mt.GracePeriodDays, mt.EndDateUtc) "); //before end of grace period

            sqlCommand.Append("ORDER BY mt.CreatedUtc DESC ");


            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?CurrentTimeUtc", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetUnProcessedExpiredTicketsByUser(Guid userGuid, Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  mt.*, ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LastActivityDate, ");
            sqlCommand.Append("u.TimeZoneId ");

            sqlCommand.Append("FROM	sts_MembershipTicket mt ");

            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON mt.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("mt.UserGuid = ?UserGuid ");
            sqlCommand.Append("AND mt.SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND mt.OrderStatusGuid = '70443443-f665-42c9-b69f-48cbf011a14b' "); //fulfillable status
            sqlCommand.Append("AND ?CurrentTimeUtc  >= TIMESTAMPADD(DAY, mt.GracePeriodDays, mt.EndDateUtc) "); //past end of grace period

           

            sqlCommand.Append("ORDER BY mt.CreatedUtc DESC ");


            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?CurrentTimeUtc", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByEndDateForReminder(
            Guid reminderGuid,
            Guid productGuid,
            Guid orderStatusGuid,
            DateTime beginDate,
            DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  mt.*, ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LastActivityDate, ");
            sqlCommand.Append("u.TimeZoneId ");

            sqlCommand.Append("FROM	sts_MembershipTicket mt ");

            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON mt.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("mt.ProductGuid = ?ProductGuid ");
            sqlCommand.Append("AND mt.OrderStatusGuid = ?OrderStatusGuid ");
            sqlCommand.Append("AND mt.EndDateUtc IS NOT NULL ");
            sqlCommand.Append("AND mt.EndDateUtc > ?BeginDate ");
            sqlCommand.Append("AND mt.EndDateUtc < ?EndDate ");
            sqlCommand.Append("AND mt.Guid NOT IN ");
            sqlCommand.Append("(SELECT rl.TicketGuid ");
            sqlCommand.Append("FROM sts_MembershipReminderLog rl ");
            sqlCommand.Append("WHERE rl.ReminderGuid = ?ReminderGuid) ");

            sqlCommand.Append("ORDER BY mt.CreatedUtc DESC ");


            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?ReminderGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = reminderGuid.ToString();

            arParams[1] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = productGuid.ToString();

            arParams[2] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = orderStatusGuid.ToString();

            arParams[3] = new MySqlParameter("?BeginDate", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = beginDate;

            arParams[4] = new MySqlParameter("?EndDate", MySqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = endDate;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByGracePeriodEndDateForReminder(
            Guid reminderGuid,
            Guid productGuid,
            Guid orderStatusGuid,
            DateTime beginDate,
            DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  mt.*, ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LastActivityDate, ");
            sqlCommand.Append("u.TimeZoneId ");

            sqlCommand.Append("FROM	sts_MembershipTicket mt ");

            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON mt.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("mt.ProductGuid = ?ProductGuid ");
            sqlCommand.Append("AND mt.OrderStatusGuid = ?OrderStatusGuid ");
            sqlCommand.Append("AND mt.EndDateUtc IS NOT NULL ");
            sqlCommand.Append("AND TIMESTAMPADD(DAY, mt.GracePeriodDays, mt.EndDateUtc) > ?BeginDate ");
            sqlCommand.Append("AND TIMESTAMPADD(DAY, mt.GracePeriodDays, mt.EndDateUtc) < ?EndDate ");
            sqlCommand.Append("AND mt.Guid NOT IN ");
            sqlCommand.Append("(SELECT rl.TicketGuid ");
            sqlCommand.Append("FROM sts_MembershipReminderLog rl ");
            sqlCommand.Append("WHERE rl.ReminderGuid = ?ReminderGuid) ");

            sqlCommand.Append("ORDER BY mt.CreatedUtc DESC ");


            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?ReminderGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = reminderGuid.ToString();

            arParams[1] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = productGuid.ToString();

            arParams[2] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = orderStatusGuid.ToString();

            arParams[3] = new MySqlParameter("?BeginDate", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = beginDate;

            arParams[4] = new MySqlParameter("?EndDate", MySqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = endDate;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetUnProcessedExpiredUsers()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT DISTINCT ");
            sqlCommand.Append("mt.UserGuid, ");
            sqlCommand.Append("mt.SiteGuid, ");
            sqlCommand.Append("s.SiteID, ");
            sqlCommand.Append("u.UserID ");
            

            sqlCommand.Append("FROM	sts_MembershipTicket mt ");

            sqlCommand.Append("JOIN mp_Sites s ");
            sqlCommand.Append("ON	s.SiteGuid = mt.SiteGuid ");

            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON mt.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("mt.OrderStatusGuid = '70443443-f665-42c9-b69f-48cbf011a14b' "); //fulfillable status
            sqlCommand.Append("AND ?CurrentTimeUtc  >= TIMESTAMPADD(DAY, mt.GracePeriodDays, mt.EndDateUtc) "); //past end of grace period


            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?CurrentTimeUtc", MySqlDbType.DateTime);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = DateTime.UtcNow;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static int GetCartCount(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_MembershipTicket ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND OrderStatusGuid = '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static int GetCount(Guid siteGuid, Guid orderStatusGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_MembershipTicket ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND OrderStatusGuid <> '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("AND (?OrderStatusGuid = '11111111-1111-1111-1111-111111111111' OR OrderStatusGuid = ?OrderStatusGuid) ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderStatusGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        public static int GetCountExpiring(Guid siteGuid, Guid orderStatusGuid, DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_MembershipTicket ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND OrderStatusGuid = ?OrderStatusGuid ");
            sqlCommand.Append("AND EndDateUtc <= ?EndDate ");
            sqlCommand.Append("AND EndDateUtc > ?CurrentTime ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderStatusGuid.ToString();

            arParams[2] = new MySqlParameter("?EndDate", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = endDate;

            arParams[3] = new MySqlParameter("?CurrentTime", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = DateTime.UtcNow;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        public static int GetCountExpired(Guid siteGuid, Guid orderStatusGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_MembershipTicket ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND OrderStatusGuid = ?OrderStatusGuid ");
            sqlCommand.Append("AND EndDateUtc <= ?EndDate ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderStatusGuid.ToString();

            arParams[2] = new MySqlParameter("?EndDate", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

           
            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static int CountActiveByMembershipDefinition(Guid siteGuid, Guid membershipDefinitionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_MembershipTicket ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND MemberDefGuid = ?MemberDefGuid ");
            sqlCommand.Append("AND OrderStatusGuid = '70443443-f665-42c9-b69f-48cbf011a14b' "); //fulfillable status
            sqlCommand.Append("AND ?CurrentTimeUtc < TIMESTAMPADD(DAY, GracePeriodDays, EndDateUtc) "); //before end of grace period
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?MemberDefGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = membershipDefinitionGuid.ToString();

            arParams[2] = new MySqlParameter("?CurrentTimeUtc", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;


            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        public static int CountActiveByProduct(Guid siteGuid, Guid productGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_MembershipTicket ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ProductGuid = ?ProductGuid ");
            sqlCommand.Append("AND OrderStatusGuid = '70443443-f665-42c9-b69f-48cbf011a14b' "); //fulfillable status
            sqlCommand.Append("AND ?CurrentTimeUtc < TIMESTAMPADD(DAY, GracePeriodDays, EndDateUtc) "); //before end of grace period
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = productGuid.ToString();

            arParams[2] = new MySqlParameter("?CurrentTimeUtc", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;


            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets a page of data from the sts_MembershipTicket table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid siteGuid,
            Guid orderStatusGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(siteGuid, orderStatusGuid);

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
            sqlCommand.Append("SELECT  mt.*, ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LastActivityDate, ");
            sqlCommand.Append("u.TimeZoneId ");

            sqlCommand.Append("FROM	sts_MembershipTicket mt ");

            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON mt.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("mt.SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND mt.OrderStatusGuid <> '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("AND (?OrderStatusGuid = '11111111-1111-1111-1111-111111111111' OR mt.OrderStatusGuid = ?OrderStatusGuid) ");

            sqlCommand.Append("ORDER BY mt.LastModifiedUtc DESC  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderStatusGuid.ToString();

            arParams[2] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetCartPage(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCartCount(siteGuid);

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
            sqlCommand.Append("SELECT  mt.*, ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LastActivityDate, ");
            sqlCommand.Append("u.TimeZoneId ");

            sqlCommand.Append("FROM	sts_MembershipTicket mt ");

            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON mt.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("mt.SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND mt.OrderStatusGuid = '00000000-0000-0000-0000-000000000000' ");
           

            sqlCommand.Append("ORDER BY mt.LastModifiedUtc DESC  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetPageExpiring(
            Guid siteGuid,
            Guid orderStatusGuid,
            DateTime endDate,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountExpiring(siteGuid, orderStatusGuid, endDate);

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
            sqlCommand.Append("SELECT  mt.*, ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LastActivityDate, ");
            sqlCommand.Append("u.TimeZoneId ");

            sqlCommand.Append("FROM	sts_MembershipTicket mt ");

            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON mt.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("mt.SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND mt.OrderStatusGuid = ?OrderStatusGuid ");
            sqlCommand.Append("AND EndDateUtc <= ?EndDate ");
            sqlCommand.Append("AND EndDateUtc > ?CurrentTime ");

            sqlCommand.Append("ORDER BY mt.LastModifiedUtc DESC  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderStatusGuid.ToString();

            arParams[2] = new MySqlParameter("?EndDate", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = endDate;

            arParams[3] = new MySqlParameter("?CurrentTime", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = DateTime.UtcNow;

            arParams[4] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageSize;

            arParams[5] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetPageExpired(
            Guid siteGuid,
            Guid orderStatusGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountExpired(siteGuid, orderStatusGuid);

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
            sqlCommand.Append("SELECT  mt.*, ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LastActivityDate, ");
            sqlCommand.Append("u.TimeZoneId ");

            sqlCommand.Append("FROM	sts_MembershipTicket mt ");

            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON mt.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("mt.SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND mt.OrderStatusGuid = ?OrderStatusGuid ");
            sqlCommand.Append("AND EndDateUtc <= ?EndDate ");
  

            sqlCommand.Append("ORDER BY mt.LastModifiedUtc DESC  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderStatusGuid.ToString();

            arParams[2] = new MySqlParameter("?EndDate", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            arParams[3] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageSize;

            arParams[4] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetCountForSearch(
            Guid siteGuid,
            Guid userGuid,
            Guid productGuid,
            Guid memberDefGuid,
            Guid orderStatusGuid,
            DateTime beginDateUtc,
            DateTime endDateUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_MembershipTicket ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ((?UserGuid = '00000000-0000-0000-0000-000000000000')OR(UserGuid = ?UserGuid)) ");
            sqlCommand.Append("AND ((?ProductGuid = '00000000-0000-0000-0000-000000000000')OR(ProductGuid = ?ProductGuid)) ");
            sqlCommand.Append("AND ((?MemberDefGuid = '00000000-0000-0000-0000-000000000000')OR(MemberDefGuid = ?MemberDefGuid)) ");
            sqlCommand.Append("AND OrderStatusGuid <> '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("AND (?OrderStatusGuid = '11111111-1111-1111-1111-111111111111' OR OrderStatusGuid = ?OrderStatusGuid) ");
            sqlCommand.Append("AND ((?BeginDateUtc IS NULL) OR ((BeginDateUtc IS NOT NULL) AND(BeginDateUtc >= ?BeginDateUtc))) ");
            sqlCommand.Append("AND ((?EndDateUtc IS NULL) OR ((EndDateUtc IS NOT NULL) AND(EndDateUtc <= ?EndDateUtc))) ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[7];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = productGuid.ToString();

            arParams[3] = new MySqlParameter("?MemberDefGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = memberDefGuid.ToString();

            arParams[4] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = orderStatusGuid.ToString();

            arParams[5] = new MySqlParameter("?BeginDateUtc", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            if (beginDateUtc > DateTime.MinValue)
            {
                arParams[5].Value = beginDateUtc;
            }
            else
            {
                arParams[5].Value = DBNull.Value;
            }

            arParams[6] = new MySqlParameter("?EndDateUtc", MySqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            if (endDateUtc > DateTime.MinValue)
            {
                arParams[6].Value = endDateUtc;
            }
            else
            {
                arParams[6].Value = DBNull.Value;
            }

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }


        public static IDataReader Search(
            Guid siteGuid,
            Guid userGuid,
            Guid productGuid,
            Guid memberDefGuid,
            Guid orderStatusGuid,
            DateTime beginDateUtc,
            DateTime endDateUtc,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountForSearch(siteGuid, userGuid, productGuid, memberDefGuid, orderStatusGuid, beginDateUtc, endDateUtc);

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
            sqlCommand.Append("SELECT  mt.*, ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LastActivityDate, ");
            sqlCommand.Append("u.TimeZoneId ");

            sqlCommand.Append("FROM	sts_MembershipTicket mt ");

            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON mt.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("mt.SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ((?UserGuid = '00000000-0000-0000-0000-000000000000')OR(mt.UserGuid = ?UserGuid)) ");
            sqlCommand.Append("AND ((?ProductGuid = '00000000-0000-0000-0000-000000000000')OR(mt.ProductGuid = ?ProductGuid)) ");
            sqlCommand.Append("AND ((?MemberDefGuid = '00000000-0000-0000-0000-000000000000')OR(mt.MemberDefGuid = ?MemberDefGuid)) ");
            sqlCommand.Append("AND mt.OrderStatusGuid <> '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("AND (?OrderStatusGuid = '11111111-1111-1111-1111-111111111111' OR mt.OrderStatusGuid = ?OrderStatusGuid) ");
            sqlCommand.Append("AND ((?BeginDateUtc IS NULL) OR ((mt.BeginDateUtc IS NOT NULL) AND(mt.BeginDateUtc >= ?BeginDateUtc))) ");
            sqlCommand.Append("AND ((?EndDateUtc IS NULL) OR ((mt.EndDateUtc IS NOT NULL) AND(mt.EndDateUtc <= ?EndDateUtc))) ");

            sqlCommand.Append("ORDER BY mt.LastModifiedUtc DESC  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[9];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = productGuid.ToString();

            arParams[3] = new MySqlParameter("?MemberDefGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = memberDefGuid.ToString();

            arParams[4] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = orderStatusGuid.ToString();

            arParams[5] = new MySqlParameter("?BeginDateUtc", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            if (beginDateUtc > DateTime.MinValue)
            {
                arParams[5].Value = beginDateUtc;
            }
            else
            {
                arParams[5].Value = DBNull.Value;
            }

            arParams[6] = new MySqlParameter("?EndDateUtc", MySqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            if (endDateUtc > DateTime.MinValue)
            {
                arParams[6].Value = endDateUtc;
            }
            else
            {
                arParams[6].Value = DBNull.Value;
            }

            arParams[7] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = pageSize;

            arParams[8] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

    }
}
