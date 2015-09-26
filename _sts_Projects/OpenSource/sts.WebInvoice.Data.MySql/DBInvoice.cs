// Author:					Joe Audette
// Created:				    2011-02-02
// Last Modified:			2012-05-14

using System;
using System.Globalization;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace sts.WebInvoice.Data
{
    public static class DBInvoice
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
        /// Inserts a row in the sts_Invoice table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="customGuid"> customGuid </param>
        /// <param name="emailSubject"> emailSubject </param>
        /// <param name="textBody"> textBody </param>
        /// <param name="htmlBody"> htmlBody </param>
        /// <param name="productName"> productName </param>
        /// <param name="customData"> customData </param>
        /// <param name="invoiceNumber"> invoiceNumber </param>
        /// <param name="invoiceInt"> invoiceInt </param>
        /// <param name="subTotal"> subTotal </param>
        /// <param name="tax"> tax </param>
        /// <param name="shipping"> shipping </param>
        /// <param name="discount"> discount </param>
        /// <param name="invoiceAmt"> invoiceAmt </param>
        /// <param name="processingFee"> processingFee </param>
        /// <param name="discountCodes"> discountCodes </param>
        /// <param name="invoiceDateUtc"> invoiceDateUtc </param>
        /// <param name="dueDateUtc"> dueDateUtc </param>
        /// <param name="datePaidUtc"> datePaidUtc </param>
        /// <param name="paidFromIpAddress"> paidFromIpAddress </param>
        /// <param name="analyticsTracked"> analyticsTracked </param>
        /// <param name="paymentMethod"> paymentMethod </param>
        /// <param name="paymentEnteredBy"> paymentEnteredBy </param>
        /// <param name="invoiceType"> invoiceType </param>
        /// <param name="createdUser"> createdUser </param>
        /// <param name="customerGuid"> customerGuid </param>
        /// <param name="customerEmail"> customerEmail </param>
        /// <param name="customerFirstName"> customerFirstName </param>
        /// <param name="customerLastName"> customerLastName </param>
        /// <param name="customerCompany"> customerCompany </param>
        /// <param name="customerPhone"> customerPhone </param>
        /// <param name="customerAddress1"> customerAddress1 </param>
        /// <param name="customerAddress2"> customerAddress2 </param>
        /// <param name="customerSuburb"> customerSuburb </param>
        /// <param name="customerCity"> customerCity </param>
        /// <param name="customerState"> customerState </param>
        /// <param name="customerPostalCode"> customerPostalCode </param>
        /// <param name="customerCountry"> customerCountry </param>
        /// <param name="sendCount"> sendCount </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="createdBy"> createdBy </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid moduleGuid,
            Guid productGuid,
            Guid userGuid,
            Guid customGuid,
            string emailSubject,
            string textBody,
            string htmlBody,
            string productName,
            string customData,
            string invoiceNumber,
            int invoiceInt,
            decimal subTotal,
            decimal tax,
            decimal shipping,
            decimal discount,
            decimal invoiceAmt,
            decimal processingFee,
            string discountCodes,
            DateTime invoiceDateUtc,
            DateTime dueDateUtc,
            DateTime datePaidUtc,
            string paidFromIpAddress,
            bool analyticsTracked,
            string paymentMethod,
            Guid paymentEnteredBy,
            string invoiceType,
            bool createdUser,
            Guid customerGuid,
            string customerEmail,
            string customerFirstName,
            string customerLastName,
            string customerCompany,
            string customerPhone,
            string customerAddress1,
            string customerAddress2,
            string customerSuburb,
            string customerCity,
            string customerState,
            string customerPostalCode,
            string customerCountry,
            int sendCount,
            Guid statusGuid,
            DateTime createdUtc,
            Guid createdBy)
        {
            #region Bit Conversion

            int intAnalyticsTracked = 0;
            if (analyticsTracked) { intAnalyticsTracked = 1; }
            int intCreatedUser = 0;
            if (createdUser) { intCreatedUser = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_Invoice (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("ProductGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("CustomGuid, ");
            sqlCommand.Append("EmailSubject, ");
            sqlCommand.Append("TextBody, ");
            sqlCommand.Append("HtmlBody, ");
            sqlCommand.Append("ProductName, ");
            sqlCommand.Append("CustomData, ");
            sqlCommand.Append("InvoiceNumber, ");
            sqlCommand.Append("InvoiceInt, ");
            sqlCommand.Append("SubTotal, ");
            sqlCommand.Append("Tax, ");
            sqlCommand.Append("Shipping, ");
            sqlCommand.Append("Discount, ");
            sqlCommand.Append("InvoiceAmt, ");
            sqlCommand.Append("ProcessingFee, ");
            sqlCommand.Append("DiscountCodes, ");
            sqlCommand.Append("InvoiceDateUtc, ");
            sqlCommand.Append("DueDateUtc, ");
            sqlCommand.Append("DatePaidUtc, ");
            sqlCommand.Append("PaidFromIpAddress, ");
            sqlCommand.Append("AnalyticsTracked, ");
            sqlCommand.Append("PaymentMethod, ");
            sqlCommand.Append("PaymentEnteredBy, ");
            sqlCommand.Append("InvoiceType, ");
            sqlCommand.Append("CreatedUser, ");
            sqlCommand.Append("CustomerGuid, ");
            sqlCommand.Append("CustomerEmail, ");
            sqlCommand.Append("CustomerFirstName, ");
            sqlCommand.Append("CustomerLastName, ");
            sqlCommand.Append("CustomerCompany, ");
            sqlCommand.Append("CustomerPhone, ");
            sqlCommand.Append("CustomerAddress1, ");
            sqlCommand.Append("CustomerAddress2, ");
            sqlCommand.Append("CustomerSuburb, ");
            sqlCommand.Append("CustomerCity, ");
            sqlCommand.Append("CustomerState, ");
            sqlCommand.Append("CustomerPostalCode, ");
            sqlCommand.Append("CustomerCountry, ");
            sqlCommand.Append("SendCount, ");
            sqlCommand.Append("StatusGuid, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("LastModUtc, ");
            sqlCommand.Append("LastModBy )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?ModuleGuid, ");
            sqlCommand.Append("?ProductGuid, ");
            sqlCommand.Append("?UserGuid, ");
            sqlCommand.Append("?CustomGuid, ");
            sqlCommand.Append("?EmailSubject, ");
            sqlCommand.Append("?TextBody, ");
            sqlCommand.Append("?HtmlBody, ");
            sqlCommand.Append("?ProductName, ");
            sqlCommand.Append("?CustomData, ");
            sqlCommand.Append("?InvoiceNumber, ");
            sqlCommand.Append("?InvoiceInt, ");
            sqlCommand.Append("?SubTotal, ");
            sqlCommand.Append("?Tax, ");
            sqlCommand.Append("?Shipping, ");
            sqlCommand.Append("?Discount, ");
            sqlCommand.Append("?InvoiceAmt, ");
            sqlCommand.Append("?ProcessingFee, ");
            sqlCommand.Append("?DiscountCodes, ");
            sqlCommand.Append("?InvoiceDateUtc, ");
            sqlCommand.Append("?DueDateUtc, ");
            sqlCommand.Append("?DatePaidUtc, ");
            sqlCommand.Append("?PaidFromIpAddress, ");
            sqlCommand.Append("?AnalyticsTracked, ");
            sqlCommand.Append("?PaymentMethod, ");
            sqlCommand.Append("?PaymentEnteredBy, ");
            sqlCommand.Append("?InvoiceType, ");
            sqlCommand.Append("?CreatedUser, ");
            sqlCommand.Append("?CustomerGuid, ");
            sqlCommand.Append("?CustomerEmail, ");
            sqlCommand.Append("?CustomerFirstName, ");
            sqlCommand.Append("?CustomerLastName, ");
            sqlCommand.Append("?CustomerCompany, ");
            sqlCommand.Append("?CustomerPhone, ");
            sqlCommand.Append("?CustomerAddress1, ");
            sqlCommand.Append("?CustomerAddress2, ");
            sqlCommand.Append("?CustomerSuburb, ");
            sqlCommand.Append("?CustomerCity, ");
            sqlCommand.Append("?CustomerState, ");
            sqlCommand.Append("?CustomerPostalCode, ");
            sqlCommand.Append("?CustomerCountry, ");
            sqlCommand.Append("?SendCount, ");
            sqlCommand.Append("?StatusGuid, ");
            sqlCommand.Append("?CreatedUtc, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?LastModUtc, ");
            sqlCommand.Append("?LastModBy )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[48];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new MySqlParameter("?CustomGuid", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = customGuid.ToString();

            arParams[5] = new MySqlParameter("?EmailSubject", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = emailSubject;

            arParams[6] = new MySqlParameter("?TextBody", MySqlDbType.Text);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = textBody;

            arParams[7] = new MySqlParameter("?HtmlBody", MySqlDbType.Text);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = htmlBody;

            arParams[8] = new MySqlParameter("?ProductName", MySqlDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = productName;

            arParams[9] = new MySqlParameter("?CustomData", MySqlDbType.Text);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = customData;

            arParams[10] = new MySqlParameter("?InvoiceNumber", MySqlDbType.VarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = invoiceNumber;

            arParams[11] = new MySqlParameter("?InvoiceInt", MySqlDbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = invoiceInt;

            arParams[12] = new MySqlParameter("?SubTotal", MySqlDbType.Decimal);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = subTotal;

            arParams[13] = new MySqlParameter("?Tax", MySqlDbType.Decimal);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = tax;

            arParams[14] = new MySqlParameter("?Shipping", MySqlDbType.Decimal);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = shipping;

            arParams[15] = new MySqlParameter("?Discount", MySqlDbType.Decimal);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = discount;

            arParams[16] = new MySqlParameter("?InvoiceAmt", MySqlDbType.Decimal);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = invoiceAmt;

            arParams[17] = new MySqlParameter("?ProcessingFee", MySqlDbType.Decimal);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = processingFee;

            arParams[18] = new MySqlParameter("?DiscountCodes", MySqlDbType.VarChar, 255);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = discountCodes;

            arParams[19] = new MySqlParameter("?InvoiceDateUtc", MySqlDbType.DateTime);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = invoiceDateUtc;

            arParams[20] = new MySqlParameter("?DueDateUtc", MySqlDbType.DateTime);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = dueDateUtc;
            
            

            arParams[21] = new MySqlParameter("?DatePaidUtc", MySqlDbType.DateTime);
            arParams[21].Direction = ParameterDirection.Input;
            if (datePaidUtc < DateTime.MaxValue)
            {
                arParams[21].Value = datePaidUtc;

            }
            else
            {
                arParams[21].Value = DBNull.Value;
            }

            arParams[22] = new MySqlParameter("?PaidFromIpAddress", MySqlDbType.VarChar, 255);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = paidFromIpAddress;

            arParams[23] = new MySqlParameter("?AnalyticsTracked", MySqlDbType.Int32);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = intAnalyticsTracked;

            arParams[24] = new MySqlParameter("?PaymentMethod", MySqlDbType.VarChar, 50);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = paymentMethod;

            arParams[25] = new MySqlParameter("?PaymentEnteredBy", MySqlDbType.VarChar, 36);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = paymentEnteredBy.ToString();

            arParams[26] = new MySqlParameter("?InvoiceType", MySqlDbType.VarChar, 50);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = invoiceType;

            arParams[27] = new MySqlParameter("?CreatedUser", MySqlDbType.Int32);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = intCreatedUser;

            arParams[28] = new MySqlParameter("?CustomerGuid", MySqlDbType.VarChar, 36);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = customerGuid.ToString();

            arParams[29] = new MySqlParameter("?CustomerEmail", MySqlDbType.VarChar, 255);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = customerEmail;

            arParams[30] = new MySqlParameter("?CustomerFirstName", MySqlDbType.VarChar, 100);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = customerFirstName;

            arParams[31] = new MySqlParameter("?CustomerLastName", MySqlDbType.VarChar, 100);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = customerLastName;

            arParams[32] = new MySqlParameter("?CustomerCompany", MySqlDbType.VarChar, 255);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = customerCompany;

            arParams[33] = new MySqlParameter("?CustomerPhone", MySqlDbType.VarChar, 32);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = customerPhone;

            arParams[34] = new MySqlParameter("?CustomerAddress1", MySqlDbType.VarChar, 255);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = customerAddress1;

            arParams[35] = new MySqlParameter("?CustomerAddress2", MySqlDbType.VarChar, 255);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = customerAddress2;

            arParams[36] = new MySqlParameter("?CustomerSuburb", MySqlDbType.VarChar, 255);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = customerSuburb;

            arParams[37] = new MySqlParameter("?CustomerCity", MySqlDbType.VarChar, 255);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = customerCity;

            arParams[38] = new MySqlParameter("?CustomerState", MySqlDbType.VarChar, 255);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = customerState;

            arParams[39] = new MySqlParameter("?CustomerPostalCode", MySqlDbType.VarChar, 20);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = customerPostalCode;

            arParams[40] = new MySqlParameter("?CustomerCountry", MySqlDbType.VarChar, 255);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = customerCountry;

            arParams[41] = new MySqlParameter("?SendCount", MySqlDbType.Int32);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = sendCount;

            arParams[42] = new MySqlParameter("?StatusGuid", MySqlDbType.VarChar, 36);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = statusGuid.ToString();

            arParams[43] = new MySqlParameter("?CreatedUtc", MySqlDbType.DateTime);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = createdUtc;

            arParams[44] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = createdBy.ToString();

            arParams[45] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = createdUtc;

            arParams[46] = new MySqlParameter("?LastModBy", MySqlDbType.VarChar, 36);
            arParams[46].Direction = ParameterDirection.Input;
            arParams[46].Value = createdBy.ToString();

            arParams[47] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[47].Direction = ParameterDirection.Input;
            arParams[47].Value = productGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the sts_Invoice table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="customGuid"> customGuid </param>
        /// <param name="emailSubject"> emailSubject </param>
        /// <param name="textBody"> textBody </param>
        /// <param name="htmlBody"> htmlBody </param>
        /// <param name="productName"> productName </param>
        /// <param name="customData"> customData </param>
        /// <param name="invoiceNumber"> invoiceNumber </param>
        /// <param name="invoiceInt"> invoiceInt </param>
        /// <param name="subTotal"> subTotal </param>
        /// <param name="tax"> tax </param>
        /// <param name="shipping"> shipping </param>
        /// <param name="discount"> discount </param>
        /// <param name="invoiceAmt"> invoiceAmt </param>
        /// <param name="processingFee"> processingFee </param>
        /// <param name="discountCodes"> discountCodes </param>
        /// <param name="invoiceDateUtc"> invoiceDateUtc </param>
        /// <param name="dueDateUtc"> dueDateUtc </param>
        /// <param name="datePaidUtc"> datePaidUtc </param>
        /// <param name="paidFromIpAddress"> paidFromIpAddress </param>
        /// <param name="analyticsTracked"> analyticsTracked </param>
        /// <param name="paymentMethod"> paymentMethod </param>
        /// <param name="paymentEnteredBy"> paymentEnteredBy </param>
        /// <param name="invoiceType"> invoiceType </param>
        /// <param name="createdUser"> createdUser </param>
        /// <param name="customerGuid"> customerGuid </param>
        /// <param name="customerEmail"> customerEmail </param>
        /// <param name="customerFirstName"> customerFirstName </param>
        /// <param name="customerLastName"> customerLastName </param>
        /// <param name="customerCompany"> customerCompany </param>
        /// <param name="customerPhone"> customerPhone </param>
        /// <param name="customerAddress1"> customerAddress1 </param>
        /// <param name="customerAddress2"> customerAddress2 </param>
        /// <param name="customerSuburb"> customerSuburb </param>
        /// <param name="customerCity"> customerCity </param>
        /// <param name="customerState"> customerState </param>
        /// <param name="customerPostalCode"> customerPostalCode </param>
        /// <param name="customerCountry"> customerCountry </param>
        /// <param name="sendCount"> sendCount </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            Guid productGuid,
            Guid userGuid,
            Guid customGuid,
            string emailSubject,
            string textBody,
            string htmlBody,
            string productName,
            string customData,
            string invoiceNumber,
            int invoiceInt,
            decimal subTotal,
            decimal tax,
            decimal shipping,
            decimal discount,
            decimal invoiceAmt,
            decimal processingFee,
            string discountCodes,
            DateTime invoiceDateUtc,
            DateTime dueDateUtc,
            DateTime datePaidUtc,
            string paidFromIpAddress,
            bool analyticsTracked,
            string paymentMethod,
            Guid paymentEnteredBy,
            string invoiceType,
            bool createdUser,
            Guid customerGuid,
            string customerEmail,
            string customerFirstName,
            string customerLastName,
            string customerCompany,
            string customerPhone,
            string customerAddress1,
            string customerAddress2,
            string customerSuburb,
            string customerCity,
            string customerState,
            string customerPostalCode,
            string customerCountry,
            int sendCount,
            Guid statusGuid,
            DateTime lastModUtc,
            Guid lastModBy)
        {
            #region Bit Conversion

            int intAnalyticsTracked = 0;
            if (analyticsTracked) { intAnalyticsTracked = 1; }
            int intCreatedUser = 0;
            if (createdUser) { intCreatedUser = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_Invoice ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("ProductGuid = ?ProductGuid, ");
            sqlCommand.Append("UserGuid = ?UserGuid, ");
            sqlCommand.Append("CustomGuid = ?CustomGuid, ");
            sqlCommand.Append("EmailSubject = ?EmailSubject, ");
            sqlCommand.Append("TextBody = ?TextBody, ");
            sqlCommand.Append("HtmlBody = ?HtmlBody, ");
            sqlCommand.Append("ProductName = ?ProductName, ");
            sqlCommand.Append("CustomData = ?CustomData, ");
            sqlCommand.Append("InvoiceNumber = ?InvoiceNumber, ");
            sqlCommand.Append("InvoiceInt = ?InvoiceInt, ");
            sqlCommand.Append("SubTotal = ?SubTotal, ");
            sqlCommand.Append("Tax = ?Tax, ");
            sqlCommand.Append("Shipping = ?Shipping, ");
            sqlCommand.Append("Discount = ?Discount, ");
            sqlCommand.Append("InvoiceAmt = ?InvoiceAmt, ");
            sqlCommand.Append("ProcessingFee = ?ProcessingFee, ");
            sqlCommand.Append("DiscountCodes = ?DiscountCodes, ");
            sqlCommand.Append("InvoiceDateUtc = ?InvoiceDateUtc, ");
            sqlCommand.Append("DueDateUtc = ?DueDateUtc, ");
            sqlCommand.Append("DatePaidUtc = ?DatePaidUtc, ");
            sqlCommand.Append("PaidFromIpAddress = ?PaidFromIpAddress, ");
            sqlCommand.Append("AnalyticsTracked = ?AnalyticsTracked, ");
            sqlCommand.Append("PaymentMethod = ?PaymentMethod, ");
            sqlCommand.Append("PaymentEnteredBy = ?PaymentEnteredBy, ");
            sqlCommand.Append("InvoiceType = ?InvoiceType, ");
            sqlCommand.Append("CreatedUser = ?CreatedUser, ");
            sqlCommand.Append("CustomerGuid = ?CustomerGuid, ");
            sqlCommand.Append("CustomerEmail = ?CustomerEmail, ");
            sqlCommand.Append("CustomerFirstName = ?CustomerFirstName, ");
            sqlCommand.Append("CustomerLastName = ?CustomerLastName, ");
            sqlCommand.Append("CustomerCompany = ?CustomerCompany, ");
            sqlCommand.Append("CustomerPhone = ?CustomerPhone, ");
            sqlCommand.Append("CustomerAddress1 = ?CustomerAddress1, ");
            sqlCommand.Append("CustomerAddress2 = ?CustomerAddress2, ");
            sqlCommand.Append("CustomerSuburb = ?CustomerSuburb, ");
            sqlCommand.Append("CustomerCity = ?CustomerCity, ");
            sqlCommand.Append("CustomerState = ?CustomerState, ");
            sqlCommand.Append("CustomerPostalCode = ?CustomerPostalCode, ");
            sqlCommand.Append("CustomerCountry = ?CustomerCountry, ");
            sqlCommand.Append("SendCount = ?SendCount, ");
            sqlCommand.Append("StatusGuid = ?StatusGuid, ");
            
            sqlCommand.Append("LastModUtc = ?LastModUtc, ");
            sqlCommand.Append("LastModBy = ?LastModBy ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[44];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new MySqlParameter("?CustomGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = customGuid.ToString();

            arParams[3] = new MySqlParameter("?EmailSubject", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = emailSubject;

            arParams[4] = new MySqlParameter("?TextBody", MySqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = textBody;

            arParams[5] = new MySqlParameter("?HtmlBody", MySqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = htmlBody;

            arParams[6] = new MySqlParameter("?ProductName", MySqlDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = productName;

            arParams[7] = new MySqlParameter("?CustomData", MySqlDbType.Text);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = customData;

            arParams[8] = new MySqlParameter("?InvoiceNumber", MySqlDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = invoiceNumber;

            arParams[9] = new MySqlParameter("?InvoiceInt", MySqlDbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = invoiceInt;

            arParams[10] = new MySqlParameter("?SubTotal", MySqlDbType.Decimal);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = subTotal;

            arParams[11] = new MySqlParameter("?Tax", MySqlDbType.Decimal);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = tax;

            arParams[12] = new MySqlParameter("?Shipping", MySqlDbType.Decimal);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = shipping;

            arParams[13] = new MySqlParameter("?Discount", MySqlDbType.Decimal);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = discount;

            arParams[14] = new MySqlParameter("?InvoiceAmt", MySqlDbType.Decimal);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = invoiceAmt;

            arParams[15] = new MySqlParameter("?ProcessingFee", MySqlDbType.Decimal);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = processingFee;

            arParams[16] = new MySqlParameter("?DiscountCodes", MySqlDbType.VarChar, 255);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = discountCodes;

            arParams[17] = new MySqlParameter("?InvoiceDateUtc", MySqlDbType.DateTime);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = invoiceDateUtc;

            arParams[18] = new MySqlParameter("?DueDateUtc", MySqlDbType.DateTime);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = dueDateUtc;

            arParams[19] = new MySqlParameter("?DatePaidUtc", MySqlDbType.DateTime);
            arParams[19].Direction = ParameterDirection.Input;

            if (datePaidUtc < DateTime.MaxValue)
            {
                arParams[19].Value = datePaidUtc;

            }
            else
            {
                arParams[19].Value = DBNull.Value;
            }

            arParams[20] = new MySqlParameter("?PaidFromIpAddress", MySqlDbType.VarChar, 255);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = paidFromIpAddress;

            arParams[21] = new MySqlParameter("?AnalyticsTracked", MySqlDbType.Int32);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = intAnalyticsTracked;

            arParams[22] = new MySqlParameter("?PaymentMethod", MySqlDbType.VarChar, 50);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = paymentMethod;

            arParams[23] = new MySqlParameter("?PaymentEnteredBy", MySqlDbType.VarChar, 36);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = paymentEnteredBy.ToString();

            arParams[24] = new MySqlParameter("?InvoiceType", MySqlDbType.VarChar, 50);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = invoiceType;

            arParams[25] = new MySqlParameter("?CreatedUser", MySqlDbType.Int32);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = intCreatedUser;

            arParams[26] = new MySqlParameter("?CustomerGuid", MySqlDbType.VarChar, 36);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = customerGuid.ToString();

            arParams[27] = new MySqlParameter("?CustomerEmail", MySqlDbType.VarChar, 255);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = customerEmail;

            arParams[28] = new MySqlParameter("?CustomerFirstName", MySqlDbType.VarChar, 100);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = customerFirstName;

            arParams[29] = new MySqlParameter("?CustomerLastName", MySqlDbType.VarChar, 100);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = customerLastName;

            arParams[30] = new MySqlParameter("?CustomerCompany", MySqlDbType.VarChar, 255);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = customerCompany;

            arParams[31] = new MySqlParameter("?CustomerPhone", MySqlDbType.VarChar, 32);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = customerPhone;

            arParams[32] = new MySqlParameter("?CustomerAddress1", MySqlDbType.VarChar, 255);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = customerAddress1;

            arParams[33] = new MySqlParameter("?CustomerAddress2", MySqlDbType.VarChar, 255);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = customerAddress2;

            arParams[34] = new MySqlParameter("?CustomerSuburb", MySqlDbType.VarChar, 255);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = customerSuburb;

            arParams[35] = new MySqlParameter("?CustomerCity", MySqlDbType.VarChar, 255);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = customerCity;

            arParams[36] = new MySqlParameter("?CustomerState", MySqlDbType.VarChar, 255);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = customerState;

            arParams[37] = new MySqlParameter("?CustomerPostalCode", MySqlDbType.VarChar, 20);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = customerPostalCode;

            arParams[38] = new MySqlParameter("?CustomerCountry", MySqlDbType.VarChar, 255);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = customerCountry;

            arParams[39] = new MySqlParameter("?SendCount", MySqlDbType.Int32);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = sendCount;

            arParams[40] = new MySqlParameter("?StatusGuid", MySqlDbType.VarChar, 36);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = statusGuid.ToString();

            arParams[41] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = lastModUtc;

            arParams[42] = new MySqlParameter("?LastModBy", MySqlDbType.VarChar, 36);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = lastModBy.ToString();

            arParams[43] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = productGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the sts_Invoice table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_Invoice ");
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
        /// Deletes from the sts_Invoice table. Returns true if row(s) deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_Invoice ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes from the sts_Invoice table. Returns true if row(s) deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_Invoice ");
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

        /// <summary>
        /// Gets an IDataReader with one row from the sts_Invoice table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_Invoice ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
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

        public static IDataReader GetUnPaidByUser(Guid moduleGuid, Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_Invoice ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("UserGuid = ?UserGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SendCount > 0 ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("DatePaidUtc IS NULL ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("InvoiceDateUtc ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetMostRecentPaidByUser(Guid moduleGuid, Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_Invoice ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("UserGuid = ?UserGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SendCount > 0 ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("DatePaidUtc IS NOT NULL ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("InvoiceDateUtc DESC ");
            sqlCommand.Append("LIMIT 1");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the sts_Invoice table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid moduleGuid, string invoiceNumber)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_Invoice ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("InvoiceNumber = ?InvoiceNumber ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?InvoiceNumber", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = invoiceNumber;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets the highest value for InvoiceInt
        /// </summary>
        /// <param name="guid"> guid </param>
        public static int GetMaxInvoiceInt(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  COALESCE(MAX(InvoiceInt),0) ");
            sqlCommand.Append("FROM	sts_Invoice ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets acount of rows in the sts_Invoice table
        /// </summary>
        /// <param name="guid"> guid </param>
        public static int GetCountByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_Invoice ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        /// <summary>
        /// Gets acount of rows in the sts_Invoice table
        /// </summary>
        /// <param name="guid"> guid </param>
        public static int GetCountUnPaidByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_Invoice ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("StatusGuid <> 'de3b9331-b98f-493f-be5e-926ffe5003bc' ");//cancelled
            sqlCommand.Append("AND ");
            sqlCommand.Append("DatePaidUtc IS NULL ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets acount of rows in the sts_Invoice table
        /// </summary>
        /// <param name="guid"> guid </param>
        public static int GetCountPaidByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_Invoice ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("DatePaidUtc IS NOT NULL ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets acount of rows in the sts_Invoice table
        /// </summary>
        /// <param name="guid"> guid </param>
        public static int GetCountUnPaidByModuleAndDate(Guid moduleGuid, DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_Invoice ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("StatusGuid <> 'de3b9331-b98f-493f-be5e-926ffe5003bc' ");//cancelled
            sqlCommand.Append("AND ");
            sqlCommand.Append("DatePaidUtc IS NULL ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("DueDateUtc < ?CurrentTime ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?CurrentTime", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static int GetCountForSearch(
            Guid moduleGuid,
            string invoiceNumber,
            string productName,
            string firstName,
            string lastName,
            string company,
            string email,
            DateTime invoiceDate,
            bool useInvoiceDate
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_Invoice ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((?InvoiceNumber = '') OR (InvoiceNumber LIKE ?InvoiceNumber + '%')) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((?ProductName = '') OR (ProductName LIKE ?ProductName + '%')) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((?FirstName = '') OR (CustomerFirstName LIKE ?FirstName + '%')) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((?LastName = '') OR (CustomerLastName LIKE ?LastName + '%')) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((?Company = '') OR (CustomerCompany LIKE ?Company + '%')) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((?Email = '') OR (CustomerEmail LIKE ?Email + '%')) ");
            if (useInvoiceDate)
            {
                sqlCommand.Append("AND ");
                sqlCommand.Append("InvoiceDateUtc >= ?InvoiceDate ");

            }
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[8];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?InvoiceNumber", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = invoiceNumber;

            arParams[2] = new MySqlParameter("?ProductName", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = productName;

            arParams[3] = new MySqlParameter("?FirstName", MySqlDbType.VarChar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = firstName;

            arParams[4] = new MySqlParameter("?LastName", MySqlDbType.VarChar, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = lastName;

            arParams[5] = new MySqlParameter("?Company", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = company;

            arParams[6] = new MySqlParameter("?Email", MySqlDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = email;

            arParams[7] = new MySqlParameter("?InvoiceDate", MySqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = invoiceDate;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));


        }

        public static IDataReader GetSearchPage(
            Guid moduleGuid,
            string invoiceNumber,
            string productName,
            string firstName,
            string lastName,
            string company,
            string email,
            DateTime invoiceDate,
            bool useInvoiceDate,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountForSearch(
                moduleGuid,
                invoiceNumber,
                productName,
                firstName,
                lastName,
                company,
                email,
                invoiceDate,
                useInvoiceDate
                );

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
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	sts_Invoice  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((?InvoiceNumber = '') OR (InvoiceNumber LIKE ?InvoiceNumber + '%')) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((?ProductName = '') OR (ProductName LIKE ?ProductName + '%')) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((?FirstName = '') OR (CustomerFirstName LIKE ?FirstName + '%')) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((?LastName = '') OR (CustomerLastName LIKE ?LastName + '%')) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((?Company = '') OR (CustomerCompany LIKE ?Company + '%')) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((?Email = '') OR (CustomerEmail LIKE ?Email + '%')) ");
            if (useInvoiceDate)
            {
                sqlCommand.Append("AND ");
                sqlCommand.Append("InvoiceDateUtc >= ?InvoiceDate ");

            }

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("CreatedUtc DESC  ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[10];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?InvoiceNumber", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = invoiceNumber;

            arParams[2] = new MySqlParameter("?ProductName", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = productName;

            arParams[3] = new MySqlParameter("?FirstName", MySqlDbType.VarChar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = firstName;

            arParams[4] = new MySqlParameter("?LastName", MySqlDbType.VarChar, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = lastName;

            arParams[5] = new MySqlParameter("?Company", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = company;

            arParams[6] = new MySqlParameter("?Email", MySqlDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = email;

            arParams[7] = new MySqlParameter("?InvoiceDate", MySqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = invoiceDate;

            arParams[8] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = pageSize;

            arParams[9] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a page of data from the sts_Invoice table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid moduleGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountByModule(moduleGuid);

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
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	sts_Invoice  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("CreatedUtc DESC  ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

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

        /// <summary>
        /// Gets a page of data from the sts_Invoice table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPagePastDue(
            Guid moduleGuid,
            DateTime currentTime,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountUnPaidByModuleAndDate(moduleGuid, currentTime);

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
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	sts_Invoice  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("StatusGuid <> 'de3b9331-b98f-493f-be5e-926ffe5003bc' ");//cancelled
            sqlCommand.Append("AND ");
            sqlCommand.Append("DatePaidUtc IS NULL ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("DueDateUtc < ?CurrentTime ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("CreatedUtc DESC  ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?CurrentTime", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

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

        /// <summary>
        /// Gets a page of data from the sts_Invoice table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPageUnPaid(
            Guid moduleGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountUnPaidByModule(moduleGuid);

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
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	sts_Invoice  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("StatusGuid <> 'de3b9331-b98f-493f-be5e-926ffe5003bc' ");//cancelled
            sqlCommand.Append("AND ");
            sqlCommand.Append("DatePaidUtc IS NULL ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("CreatedUtc DESC  ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

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

        /// <summary>
        /// Gets a page of data from the sts_Invoice table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPagePaid(
            Guid moduleGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountPaidByModule(moduleGuid);

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
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	sts_Invoice  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("DatePaidUtc IS NOT NULL ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("CreatedUtc DESC  ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

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
            sqlCommand.Append("s.Guid AS RowGuid, ");
            sqlCommand.Append("s.SiteGuid, ");
            sqlCommand.Append("s.UserGuid, ");
            sqlCommand.Append("m.FeatureGuid, ");
            sqlCommand.Append("s.ModuleGuid, ");
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("s.Guid, ");
            sqlCommand.Append("s.ProductGuid AS ItemGuid, ");
            sqlCommand.Append("s.ProductName AS ItemName, ");
            sqlCommand.Append("1, ");
            sqlCommand.Append("s.InvoiceAmt AS Price, ");
            sqlCommand.Append("s.InvoiceAmt AS SubTotal, ");
            sqlCommand.Append("s.CreatedUtc AS OrderDateUtc, ");
            sqlCommand.Append("s.PaymentMethod, ");
            sqlCommand.Append("'' AS IPAddress, ");

            sqlCommand.Append("CONCAT('/WebInvoice/Edit.aspx?pageid=' ");
            sqlCommand.Append(", " + pageId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;mid=', " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;id=',s.Guid) AS AdminOrderLink, ");

            sqlCommand.Append("CONCAT('/WebInvoice/ReviewInvoice.aspx?pageid=' ");
            sqlCommand.Append(", " + pageId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;mid=', " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(", '&amp;id=' , s.Guid) AS UserOrderLink, ");

            sqlCommand.Append("now() As RowCreatedUtc ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("sts_Invoice s ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Modules m ");
            sqlCommand.Append("ON s.ModuleGuid = m.Guid ");

           
            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_CommerceReport cr ");
            sqlCommand.Append("ON cr.RowGuid = s.Guid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cr.RowGuid IS NULL ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("m.Guid = ?ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("s.InvoiceAmt > 0 ");
            sqlCommand.Append("AND s.StatusGuid <> 'de3b9331-b98f-493f-be5e-926ffe5003bc' ");
            sqlCommand.Append("AND s.StatusGuid <> '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("AND s.DatePaidUtc IS NOT NULL ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static void EnsureSalesReportOrderData(Guid moduleGuid, int pageId, int moduleId)
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
            sqlCommand.Append("s.Guid AS RowGuid, ");
            sqlCommand.Append("s.SiteGuid, ");
            sqlCommand.Append("m.FeatureGuid, ");
            sqlCommand.Append("s.ModuleGuid, ");
            sqlCommand.Append("s.UserGuid, ");
            sqlCommand.Append("s.Guid, ");
            sqlCommand.Append("s.CustomerFirstName, ");
            sqlCommand.Append("s.CustomerLastName, ");
            sqlCommand.Append("s.CustomerCompany, ");
            sqlCommand.Append("s.CustomerAddress1, ");
            sqlCommand.Append("s.CustomerAddress2, ");
            sqlCommand.Append("s.CustomerSuburb, ");
            sqlCommand.Append("s.CustomerCity, ");
            sqlCommand.Append("s.CustomerPostalCode, ");
            sqlCommand.Append("s.CustomerState, ");
            sqlCommand.Append("s.CustomerCountry, ");
            sqlCommand.Append("s.PaymentMethod, ");
            sqlCommand.Append("s.InvoiceAmt, ");
            sqlCommand.Append("s.Tax, ");
            sqlCommand.Append("s.Shipping, ");
            sqlCommand.Append("s.InvoiceAmt, ");
            sqlCommand.Append("s.LastModUtc, ");

            sqlCommand.Append("CONCAT('/WebInvoice/Edit.aspx?pageid=' ");
            sqlCommand.Append(", " + pageId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;mid=', " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;id=',s.Guid) AS AdminOrderLink, ");

            sqlCommand.Append("CONCAT('/WebInvoice/ReviewInvoice.aspx?pageid=' ");
            sqlCommand.Append(", " + pageId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;mid=', " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(", '&amp;id=' , s.Guid) AS UserOrderLink, ");

            sqlCommand.Append("now() As RowCreatedUtc ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("sts_Invoice s ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Modules m ");
            sqlCommand.Append("ON s.ModuleGuid = m.Guid ");

           

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_CommerceReportOrders cr ");
            sqlCommand.Append("ON cr.RowGuid = s.Guid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cr.RowGuid IS NULL ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("s.ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND s.StatusGuid <> 'de3b9331-b98f-493f-be5e-926ffe5003bc' "); //cancelled
            sqlCommand.Append("AND s.StatusGuid <> '0db28432-d9a9-423e-84f2-8a94db434643' "); //received
            sqlCommand.Append("AND s.StatusGuid <> '00000000-0000-0000-0000-000000000000' "); //none
            sqlCommand.Append("AND s.DatePaidUtc IS NOT NULL ");
            //sqlCommand.Append("AND ");
            //sqlCommand.Append("o.OrderTotal > 0 ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

    }
}
