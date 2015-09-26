/// Author:					Joe Audette
/// Created:				2008-07-27
/// Last Modified:			2011-09-15

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using MySql.Data.MySqlClient;

namespace sts.Events.Data
{
    public static class DBEventTicketOrder
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
        /// Inserts a row in the sts_EventTicketOrder table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="moduleId"> moduleId </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="calendarGuid"> calendarGuid </param>
        /// <param name="eventGuid"> eventGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="quantity"> quantity </param>
        /// <param name="ticketPrice"> ticketPrice </param>
        /// <param name="subTotal"> subTotal </param>
        /// <param name="taxTotal"> taxTotal </param>
        /// <param name="orderTotal"> orderTotal </param>
        /// <param name="processingFee"> processingFee </param>
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
        /// <returns>int</returns>
        public static int Create(
            Guid rowGuid,
            int moduleId,
            Guid moduleGuid,
            Guid calendarGuid,
            Guid eventGuid,
            Guid userGuid,
            int quantity,
            decimal ticketPrice,
            decimal subTotal,
            decimal taxTotal,
            decimal orderTotal,
            decimal processingFee,
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
            Guid recurrenceGuid,
            string ticketNotes)
        {

            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_EventTicketOrder (");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("ModuleId, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("CalendarGuid, ");
            sqlCommand.Append("EventGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("Quantity, ");
            sqlCommand.Append("TicketPrice, ");
            sqlCommand.Append("SubTotal, ");
            sqlCommand.Append("TaxTotal, ");
            sqlCommand.Append("OrderTotal, ");
            sqlCommand.Append("ProcessingFee, ");
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
            sqlCommand.Append("RecurrenceGuid, ");
            sqlCommand.Append("TicketNotes, ");
            sqlCommand.Append("OrderStatusGuid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?RowGuid, ");
            sqlCommand.Append("?ModuleId, ");
            sqlCommand.Append("?ModuleGuid, ");
            sqlCommand.Append("?CalendarGuid, ");
            sqlCommand.Append("?EventGuid, ");
            sqlCommand.Append("?UserGuid, ");
            sqlCommand.Append("?Quantity, ");
            sqlCommand.Append("?TicketPrice, ");
            sqlCommand.Append("?SubTotal, ");
            sqlCommand.Append("?TaxTotal, ");
            sqlCommand.Append("?OrderTotal, ");
            sqlCommand.Append("?ProcessingFee, ");
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
            sqlCommand.Append("?CreatedFromIP, ");
            sqlCommand.Append("?CreatedUtc, ");
            sqlCommand.Append("?CreatedUtc, ");
            sqlCommand.Append("?RecurrenceGuid, ");
            sqlCommand.Append("?TicketNotes, ");
            sqlCommand.Append("?OrderStatusGuid )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[31];

            arParams[0] = new MySqlParameter("?RowGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new MySqlParameter("?ModuleId", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new MySqlParameter("?CalendarGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = calendarGuid.ToString();

            arParams[4] = new MySqlParameter("?EventGuid", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = eventGuid.ToString();

            arParams[5] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = userGuid.ToString();

            arParams[6] = new MySqlParameter("?Quantity", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = quantity;

            arParams[7] = new MySqlParameter("?TicketPrice", MySqlDbType.Decimal);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = ticketPrice;

            arParams[8] = new MySqlParameter("?SubTotal", MySqlDbType.Decimal);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = subTotal;

            arParams[9] = new MySqlParameter("?TaxTotal", MySqlDbType.Decimal);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = taxTotal;

            arParams[10] = new MySqlParameter("?OrderTotal", MySqlDbType.Decimal);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = orderTotal;

            arParams[11] = new MySqlParameter("?ProcessingFee", MySqlDbType.Decimal);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = processingFee;

            arParams[12] = new MySqlParameter("?CustomerFirstName", MySqlDbType.VarChar, 100);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = customerFirstName;

            arParams[13] = new MySqlParameter("?CustomerLastName", MySqlDbType.VarChar, 100);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = customerLastName;

            arParams[14] = new MySqlParameter("?CustomerCompany", MySqlDbType.VarChar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = customerCompany;

            arParams[15] = new MySqlParameter("?CustomerEmail", MySqlDbType.VarChar, 100);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = customerEmail;

            arParams[16] = new MySqlParameter("?CustomerPhone", MySqlDbType.VarChar, 32);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = customerPhone;

            arParams[17] = new MySqlParameter("?CustomerAddress1", MySqlDbType.VarChar, 255);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = customerAddress1;

            arParams[18] = new MySqlParameter("?CustomerAddress2", MySqlDbType.VarChar, 255);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = customerAddress2;

            arParams[19] = new MySqlParameter("?CustomerSuburb", MySqlDbType.VarChar, 255);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = customerSuburb;

            arParams[20] = new MySqlParameter("?CustomerCity", MySqlDbType.VarChar, 255);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = customerCity;

            arParams[21] = new MySqlParameter("?CustomerState", MySqlDbType.VarChar, 255);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = customerState;

            arParams[22] = new MySqlParameter("?CustomerPostalCode", MySqlDbType.VarChar, 20);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = customerPostalCode;

            arParams[23] = new MySqlParameter("?CustomerCountry", MySqlDbType.VarChar, 255);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = customerCountry;

            arParams[24] = new MySqlParameter("?TaxZoneGuid", MySqlDbType.VarChar, 36);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = taxZoneGuid.ToString();

            arParams[25] = new MySqlParameter("?CurrencyGuid", MySqlDbType.VarChar, 36);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = currencyGuid.ToString();

            arParams[26] = new MySqlParameter("?CreatedFromIP", MySqlDbType.VarChar, 255);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = createdFromIP;

            arParams[27] = new MySqlParameter("?CreatedUtc", MySqlDbType.DateTime);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = createdUtc;

            arParams[28] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = orderStatusGuid.ToString();

            arParams[29] = new MySqlParameter("?RecurrenceGuid", MySqlDbType.VarChar, 36);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = recurrenceGuid.ToString();

            arParams[30] = new MySqlParameter("?TicketNotes", MySqlDbType.Text);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = ticketNotes;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the sts_EventTicketOrder table. Returns true if row updated.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="calendarGuid"> calendarGuid </param>
        /// <param name="eventGuid"> eventGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="quantity"> quantity </param>
        /// <param name="ticketPrice"> ticketPrice </param>
        /// <param name="subTotal"> subTotal </param>
        /// <param name="taxTotal"> taxTotal </param>
        /// <param name="orderTotal"> orderTotal </param>
        /// <param name="processingFee"> processingFee </param>
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
        /// <param name="lastModifiedFromIP"> lastModifiedFromIP </param>
        /// <param name="lastModifiedUtc"> lastModifiedUtc </param>
        /// <param name="orderStatusGuid"> orderStatusGuid </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid rowGuid,
            Guid calendarGuid,
            Guid eventGuid,
            Guid userGuid,
            int quantity,
            decimal ticketPrice,
            decimal subTotal,
            decimal taxTotal,
            decimal orderTotal,
            decimal processingFee,
            string paymentMethod,
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
            Guid recurrenceGuid,
            string ticketNotes)
        {
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_EventTicketOrder ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("CalendarGuid = ?CalendarGuid, ");
            sqlCommand.Append("EventGuid = ?EventGuid, ");
            sqlCommand.Append("UserGuid = ?UserGuid, ");
            sqlCommand.Append("Quantity = ?Quantity, ");
            sqlCommand.Append("TicketPrice = ?TicketPrice, ");
            sqlCommand.Append("SubTotal = ?SubTotal, ");
            sqlCommand.Append("TaxTotal = ?TaxTotal, ");
            sqlCommand.Append("OrderTotal = ?OrderTotal, ");
            sqlCommand.Append("ProcessingFee = ?ProcessingFee, ");
            sqlCommand.Append("PaymentMethod = ?PaymentMethod, ");
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
            sqlCommand.Append("RecurrenceGuid = ?RecurrenceGuid, ");
            sqlCommand.Append("TicketNotes = ?TicketNotes, ");
            sqlCommand.Append("OrderStatusGuid = ?OrderStatusGuid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RowGuid = ?RowGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[30];

            arParams[0] = new MySqlParameter("?RowGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new MySqlParameter("?CalendarGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = calendarGuid.ToString();

            arParams[2] = new MySqlParameter("?EventGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = eventGuid.ToString();

            arParams[3] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new MySqlParameter("?Quantity", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = quantity;

            arParams[5] = new MySqlParameter("?TicketPrice", MySqlDbType.Decimal);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = ticketPrice;

            arParams[6] = new MySqlParameter("?SubTotal", MySqlDbType.Decimal);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = subTotal;

            arParams[7] = new MySqlParameter("?TaxTotal", MySqlDbType.Decimal);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = taxTotal;

            arParams[8] = new MySqlParameter("?OrderTotal", MySqlDbType.Decimal);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = orderTotal;

            arParams[9] = new MySqlParameter("?ProcessingFee", MySqlDbType.Decimal);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = processingFee;

            arParams[10] = new MySqlParameter("?CustomerFirstName", MySqlDbType.VarChar, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = customerFirstName;

            arParams[11] = new MySqlParameter("?CustomerLastName", MySqlDbType.VarChar, 100);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = customerLastName;

            arParams[12] = new MySqlParameter("?CustomerCompany", MySqlDbType.VarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = customerCompany;

            arParams[13] = new MySqlParameter("?CustomerEmail", MySqlDbType.VarChar, 100);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = customerEmail;

            arParams[14] = new MySqlParameter("?CustomerPhone", MySqlDbType.VarChar, 32);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = customerPhone;

            arParams[15] = new MySqlParameter("?CustomerAddress1", MySqlDbType.VarChar, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = customerAddress1;

            arParams[16] = new MySqlParameter("?CustomerAddress2", MySqlDbType.VarChar, 255);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = customerAddress2;

            arParams[17] = new MySqlParameter("?CustomerSuburb", MySqlDbType.VarChar, 255);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = customerSuburb;

            arParams[18] = new MySqlParameter("?CustomerCity", MySqlDbType.VarChar, 255);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = customerCity;

            arParams[19] = new MySqlParameter("?CustomerState", MySqlDbType.VarChar, 255);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = customerState;

            arParams[20] = new MySqlParameter("?CustomerPostalCode", MySqlDbType.VarChar, 20);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = customerPostalCode;

            arParams[21] = new MySqlParameter("?CustomerCountry", MySqlDbType.VarChar, 255);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = customerCountry;

            arParams[22] = new MySqlParameter("?TaxZoneGuid", MySqlDbType.VarChar, 36);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = taxZoneGuid.ToString();

            arParams[23] = new MySqlParameter("?CurrencyGuid", MySqlDbType.VarChar, 36);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = currencyGuid.ToString();

            arParams[24] = new MySqlParameter("?LastModifiedFromIP", MySqlDbType.VarChar, 255);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = lastModifiedFromIP;

            arParams[25] = new MySqlParameter("?LastModifiedUtc", MySqlDbType.DateTime);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = lastModifiedUtc;

            arParams[26] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = orderStatusGuid.ToString();

            arParams[27] = new MySqlParameter("?PaymentMethod", MySqlDbType.VarChar, 50);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = paymentMethod;

            arParams[28] = new MySqlParameter("?RecurrenceGuid", MySqlDbType.VarChar, 36);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = recurrenceGuid.ToString();

            arParams[29] = new MySqlParameter("?TicketNotes", MySqlDbType.Text);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = ticketNotes;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the sts_EventTicketOrder table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_EventTicketOrder ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = ?RowGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?RowGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Flags a row from the sts_EventTicketOrder table as tracked in google analytics.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool TrackAnalytics(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_EventTicketOrder ");
            sqlCommand.Append("SET AnalyticsTracked = 1 ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = ?RowGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?RowGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
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
            sqlCommand.Append("s.RowGuid, ");
            sqlCommand.Append("m.SiteGuid, ");
            sqlCommand.Append("s.UserGuid, ");
            sqlCommand.Append("m.FeatureGuid, ");
            sqlCommand.Append("m.Guid AS ModuleGuid, ");
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("s.RowGuid AS OrderGuid, ");
            sqlCommand.Append("s.EventGuid AS ItemGuid, ");
            sqlCommand.Append("o.Title AS ItemName, ");
            sqlCommand.Append("s.Quantity, ");
            sqlCommand.Append("s.TicketPrice AS Price, ");
            sqlCommand.Append("s.SubTotal, ");
            sqlCommand.Append("s.CreatedUtc AS OrderDateUtc, ");
            sqlCommand.Append("s.PaymentMethod, ");
            sqlCommand.Append("s.CreatedFromIP AS IPAddress, ");

            sqlCommand.Append("CONCAT('/Events/AdminOrderDetail.aspx?pageid=' ");
            sqlCommand.Append(", " + pageId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;mid=', " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;orderid=',s.RowGuid) AS AdminOrderLink, ");

            sqlCommand.Append("CONCAT('/Events/OrderDetail.aspx?pageid=' ");
            sqlCommand.Append(", " + pageId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;mid=', " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(", '&amp;orderid=' , s.RowGuid) AS UserOrderLink, ");

            sqlCommand.Append("now() As RowCreatedUtc ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("sts_EventTicketOrder s ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Modules m ");
            sqlCommand.Append("ON s.ModuleId = m.ModuleID ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("sts_CalendarEvents o ");
            sqlCommand.Append("ON s.EventGuid = o.ItemGuid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_CommerceReport cr ");
            sqlCommand.Append("ON cr.RowGuid = s.RowGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("s.OrderStatusGuid <> '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("AND cr.RowGuid IS NULL ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("m.Guid = ?ModuleGuid ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            //arParams[1] = new MySqlParameter("?PageID", MySqlDbType.Int32);
            //arParams[1].Direction = ParameterDirection.Input;
            //arParams[1].Value = pageId;

            MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
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
            sqlCommand.Append("s.RowGuid, ");
            sqlCommand.Append("m.SiteGuid, ");
            sqlCommand.Append("m.FeatureGuid, ");
            sqlCommand.Append("m.Guid AS ModuleGuid, ");
            sqlCommand.Append("s.UserGuid, ");
            sqlCommand.Append("s.RowGuid AS OrderGuid, ");
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
            sqlCommand.Append("s.SubTotal, ");
            sqlCommand.Append("s.TaxTotal, ");
            sqlCommand.Append("0, ");
            sqlCommand.Append("s.OrderTotal, ");
            sqlCommand.Append("s.CreatedUtc AS OrderDateUtc, ");
         
            sqlCommand.Append("CONCAT('/Events/AdminOrderDetail.aspx?pageid=' ");
            sqlCommand.Append(", " + pageId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;mid=', " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;orderid=',s.RowGuid) AS AdminOrderLink, ");

            sqlCommand.Append("CONCAT('/Events/OrderDetail.aspx?pageid=' ");
            sqlCommand.Append(", " + pageId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;mid=', " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(", '&amp;orderid=' , s.RowGuid) AS UserOrderLink, ");

            sqlCommand.Append("now() As RowCreatedUtc ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("sts_EventTicketOrder s ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Modules m ");
            sqlCommand.Append("ON s.ModuleId = m.ModuleID ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("sts_CalendarEvents o ");
            sqlCommand.Append("ON s.EventGuid = o.ItemGuid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_CommerceReportOrders cr ");
            sqlCommand.Append("ON cr.RowGuid = s.RowGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("s.OrderStatusGuid <> '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("AND cr.RowGuid IS NULL ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("m.Guid = ?ModuleGuid ");

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


        /// <summary>
        /// Deletes a row from the sts_EventTicketOrder table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByEvent(Guid eventGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_EventTicketOrder ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("EventGuid = ?EventGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?EventGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = eventGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the sts_EventTicketOrder table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_EventTicketOrder ");
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
        /// Gets an IDataReader with one row from the sts_EventTicketOrder table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetOne(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_EventTicketOrder ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = ?RowGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?RowGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the sts_EventTicketOrder table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetByEvent(Guid eventGuid, Guid orderStatusGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_EventTicketOrder ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("EventGuid = ?EventGuid ");
            sqlCommand.Append("AND OrderStatusGuid = ?OrderStatusGuid ");
            sqlCommand.Append("ORDER BY CreatedUtc ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?EventGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = eventGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderStatusGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the sts_EventTicketOrder table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetByModule(Guid moduleGuid, Guid orderStatusGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_EventTicketOrder ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND OrderStatusGuid = ?OrderStatusGuid ");
            sqlCommand.Append("ORDER BY CreatedUtc ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderStatusGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the sts_EventTicketOrder table.
        /// </summary>
        public static int GetCountByEvent(Guid eventGuid, Guid orderStatusGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_EventTicketOrder ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("EventGuid = ?EventGuid ");
            sqlCommand.Append("AND OrderStatusGuid = ?OrderStatusGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?EventGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = eventGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderStatusGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        /// <summary>
        /// Gets a count of rows in the sts_EventTicketOrder table.
        /// </summary>
        public static int GetOrderCountByUser(int moduleId, Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_EventTicketOrder ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleId = ?ModuleId ");
            sqlCommand.Append("AND UserGuid = ?UserGuid ");
            sqlCommand.Append("AND OrderStatusGuid <> '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleId", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        /// <summary>
        /// Gets a count of rows in the sts_EventTicketOrder table.
        /// </summary>
        public static int GetCountByModule(Guid moduleGuid, Guid orderStatusGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_EventTicketOrder ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND OrderStatusGuid = ?OrderStatusGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderStatusGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets a count of rows in the sts_EventTicketOrder table.
        /// </summary>
        public static int GetOrderCountByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_EventTicketOrder ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleId = ?ModuleId ");
            sqlCommand.Append("AND OrderStatusGuid <> '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleId", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets a count of rows in the sts_EventTicketOrder table.
        /// </summary>
        public static int GetOrderCountByEvent(Guid eventGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_EventTicketOrder ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("EventGuid = ?EventGuid ");
            sqlCommand.Append("AND OrderStatusGuid <> '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?EventGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = eventGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets a count of rows in the sts_EventTicketOrder table.
        /// </summary>
        public static int GetOrderCountByRecurrence(Guid recurrenceGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_EventTicketOrder ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RecurrenceGuid = ?RecurrenceGuid ");
            sqlCommand.Append("AND OrderStatusGuid <> '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?RecurrenceGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = recurrenceGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets a page of data from the sts_EventTicketOrder table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPageByEvent(
            Guid eventGuid,
            Guid orderStatusGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountByEvent(eventGuid, orderStatusGuid);

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
            sqlCommand.Append("FROM	sts_EventTicketOrder  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("EventGuid = ?EventGuid ");
            sqlCommand.Append("AND OrderStatusGuid = ?OrderStatusGuid ");
            sqlCommand.Append("ORDER BY CreatedUtc  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?EventGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = eventGuid.ToString();

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

        /// <summary>
        /// Gets a page of data from the sts_EventTicketOrder table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPageByModule(
            Guid moduleGuid,
            Guid orderStatusGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountByModule(moduleGuid, orderStatusGuid);

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
            sqlCommand.Append("SELECT	t.*, ");
            sqlCommand.Append("u.Name,  ");
            sqlCommand.Append("u.Email,  ");
            sqlCommand.Append("u.LoginName  ");

            sqlCommand.Append("FROM	sts_EventTicketOrder t  ");

            sqlCommand.Append("LEFT OUTER JOIN  ");
            sqlCommand.Append("mp_Users u  ");
            sqlCommand.Append("ON  ");
            sqlCommand.Append("u.UserGuid = t.UserGuid  ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("t.ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND t.OrderStatusGuid = ?OrderStatusGuid ");
            sqlCommand.Append("ORDER BY t.LastModifiedUtc DESC  ");
            //sqlCommand.Append("  ");
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

        /// <summary>
        /// Gets a page of data from the sts_EventTicketOrder table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPageByUser(
            int moduleId,
            Guid userGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetOrderCountByUser(moduleId, userGuid);

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
            sqlCommand.Append("SELECT	t.*, ");
            sqlCommand.Append("e.Title,  ");
            sqlCommand.Append("e.Url  ");

            sqlCommand.Append("FROM	sts_EventTicketOrder t  ");

            sqlCommand.Append("LEFT OUTER JOIN  ");
            sqlCommand.Append("sts_CalendarEvents e  ");
            sqlCommand.Append("ON  ");
            sqlCommand.Append("t.EventGuid = e.ItemGuid  ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("t.ModuleId = ?ModuleId ");
            sqlCommand.Append("AND t.UserGuid = ?UserGuid ");
            sqlCommand.Append("AND t.OrderStatusGuid <> '00000000-0000-0000-0000-000000000000' ");
            
            sqlCommand.Append("ORDER BY t.LastModifiedUtc DESC  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?ModuleId", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

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
        /// Gets a page of data from the sts_EventTicketOrder table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetOrdersPageByModule(
            int moduleId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetOrderCountByModule(moduleId);

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
            sqlCommand.Append("FROM	sts_EventTicketOrder  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleId = ?ModuleId ");
            sqlCommand.Append("AND OrderStatusGuid <> '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("ORDER BY LastModifiedUtc DESC  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ModuleId", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

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
        /// Gets a page of data from the sts_EventTicketOrder table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetOrdersPageByEvent(
            Guid eventGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetOrderCountByEvent(eventGuid);

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
            sqlCommand.Append("SELECT	t.*, ");
            sqlCommand.Append("u.Name,  ");
            sqlCommand.Append("u.Email,  ");
            sqlCommand.Append("u.LoginName  ");

            sqlCommand.Append("FROM	sts_EventTicketOrder t  ");

            sqlCommand.Append("LEFT OUTER JOIN  ");
            sqlCommand.Append("mp_Users u  ");
            sqlCommand.Append("ON  ");
            sqlCommand.Append("u.UserGuid = t.UserGuid  ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("t.EventGuid = ?EventGuid ");
            sqlCommand.Append("AND t.OrderStatusGuid <> '00000000-0000-0000-0000-000000000000' ");

            sqlCommand.Append("ORDER BY t.CustomerLastName, t.CustomerFirstName  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?EventGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = eventGuid.ToString();

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
        /// Gets a page of data from the sts_EventTicketOrder table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetOrdersPageByRecurrence(
            Guid recurrenceGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetOrderCountByRecurrence(recurrenceGuid);

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
            sqlCommand.Append("SELECT	t.*, ");
            sqlCommand.Append("u.Name,  ");
            sqlCommand.Append("u.Email,  ");
            sqlCommand.Append("u.LoginName  ");

            sqlCommand.Append("FROM	sts_EventTicketOrder t  ");

            sqlCommand.Append("LEFT OUTER JOIN  ");
            sqlCommand.Append("mp_Users u  ");
            sqlCommand.Append("ON  ");
            sqlCommand.Append("u.UserGuid = t.UserGuid  ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("t.RecurrenceGuid = ?RecurrenceGuid ");
            sqlCommand.Append("AND t.OrderStatusGuid <> '00000000-0000-0000-0000-000000000000' ");

            sqlCommand.Append("ORDER BY t.CustomerLastName, t.CustomerFirstName  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?RecurrenceGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = recurrenceGuid.ToString();

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


    }
}
