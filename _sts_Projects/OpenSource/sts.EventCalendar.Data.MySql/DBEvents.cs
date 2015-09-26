/// Author:					Joe Audette
/// Created:				2008-05-29
/// Last Modified:			2013-04-29

using System;
using System.Globalization;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace sts.Events.Data
{
    
    public static class DBEvents
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
        /// Inserts a row in the mp_CalendarEvents table. Returns new integer id.
        /// </summary>
        /// <param name="itemGuid"> itemGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="moduleID"> moduleID </param>
        /// <param name="title"> title </param>
        /// <param name="description"> description </param>
        /// <param name="imageName"> imageName </param>
        /// <param name="eventDate"> eventDate </param>
        /// <param name="startTime"> startTime </param>
        /// <param name="endTime"> endTime </param>
        /// <param name="userID"> userID </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="location"> location </param>
        /// <param name="requiresTicket"> requiresTicket </param>
        /// <param name="ticketPrice"> ticketPrice </param>
        /// <param name="createdDate"> createdDate </param>
        /// <returns>int</returns>
        public static int AddCalendarEvent(
            Guid itemGuid,
            Guid moduleGuid,
            Guid calendarGuid,
            Guid recurrenceGuid,
            int moduleId,
            string title,
            string description,
            string imageFile,
            DateTime eventDate,
            DateTime endDate,
            DateTime startTime,
            DateTime endTime,
            bool isAllDay,
            string recurrence,
            string location,
            bool showMap,
            string url,
            bool includeInFeed,
            bool requiresTicket,
            decimal ticketPrice,
            DateTime saleBeginUtc,
            DateTime saleEndUtc,
            int totalSeats,
            Guid userGuid,
            DateTime createdUtc,
            DateTime recurEndDate,
            bool allowWillPay,
            string locationAlias,
            string foreColor,
            string backColor,
            string borderColor,
            string getTicketText,
            string metaKeywords,
            string metaDescription,
            string excerpt,
            string mapHeight,
            string mapWidth,
            string mapType,
            string mapZoom,
            bool showMapOptions,
            bool showMapZoom,
            bool showMapBalloon,
            bool showMapSearch,
            bool showMapDirections,
            bool ticketIncludesRecurrence,
            bool useSameSaleDateForRecurrences,
            bool showAddToLinks,
            bool requireTicketNotes,
            string ticketNoteInfo)
        {
            #region Bit Conversion

            int intshowAddToLinks = 0;
            if (showAddToLinks) { intshowAddToLinks = 1; }

            int intrequireTicketNotes = 0;
            if (requireTicketNotes) { intrequireTicketNotes = 1; }

            int sameSalesDate = 0;
            if (useSameSaleDateForRecurrences) { sameSalesDate = 1; }

            int intticketIncludesRecurrence = 0;
            if (ticketIncludesRecurrence)
            {
                intticketIncludesRecurrence = 1;
            }

            int intshowMapOptions = 0;
            if (showMapOptions)
            {
                intshowMapOptions = 1;
            }

            int intshowMapZoom = 0;
            if (showMapZoom)
            {
                intshowMapZoom = 1;
            }

            int intshowMapBalloon = 0;
            if (showMapBalloon)
            {
                intshowMapBalloon = 1;
            }

            int intshowMapSearch = 0;
            if (showMapSearch)
            {
                intshowMapSearch = 1;
            }

            int intshowMapDirections = 0;
            if (showMapDirections)
            {
                intshowMapDirections = 1;
            }

            int intallowWillPay = 0;
            if (allowWillPay)
            {
                intallowWillPay = 1;
            }
            
            int intIsAllDay = 0;
            if (isAllDay)
            {
                intIsAllDay = 1;
            }
            
            int intShowMap = 0;
            if (showMap)
            {
                intShowMap = 1;
            }
            
            int intIncludeInFeed = 0;
            if (includeInFeed)
            {
                intIncludeInFeed = 1;
            }
            
            int intRequiresTicket = 0;
            if (requiresTicket)
            {
                intRequiresTicket = 1;
            }
            

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_CalendarEvents (");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("CalendarGuid, ");
            sqlCommand.Append("RecurrenceGuid, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("ImageFile, ");
            sqlCommand.Append("EventDate, ");
            sqlCommand.Append("EndDate, ");
            sqlCommand.Append("IsAllDay, ");
            sqlCommand.Append("StartTime, ");
            sqlCommand.Append("EndTime, ");
            sqlCommand.Append("Recurrence, ");
            sqlCommand.Append("Location, ");
            sqlCommand.Append("ShowMap, ");
            sqlCommand.Append("Url, ");
            sqlCommand.Append("IncludeInFeed, ");
            sqlCommand.Append("RequiresTicket, ");
            sqlCommand.Append("TicketPrice, ");
            sqlCommand.Append("SaleBeginUtc, ");
            sqlCommand.Append("SaleEndUtc, ");
            sqlCommand.Append("TotalSeats, ");
            sqlCommand.Append("SoldSeats, ");
            sqlCommand.Append("UserGuid, ");

            sqlCommand.Append("RecurEndDate, ");
            sqlCommand.Append("AllowWillPay, ");
            sqlCommand.Append("LocationAlias, ");
            sqlCommand.Append("ForeColor, ");
            sqlCommand.Append("BackColor, ");
            sqlCommand.Append("BorderColor, ");
            sqlCommand.Append("GetTicketText, ");
            sqlCommand.Append("MetaKeywords, ");
            sqlCommand.Append("MetaDescription, ");
            sqlCommand.Append("Excerpt, ");

            sqlCommand.Append("MapHeight, ");
            sqlCommand.Append("MapWidth, ");
            sqlCommand.Append("MapType, ");
            sqlCommand.Append("MapZoom, ");
            sqlCommand.Append("ShowMapOptions, ");
            sqlCommand.Append("ShowMapZoom, ");
            sqlCommand.Append("ShowMapBalloon, ");
            sqlCommand.Append("ShowMapSearch, ");
            sqlCommand.Append("ShowMapDirections, ");
            sqlCommand.Append("TicketIncludesRecur, ");
            sqlCommand.Append("RecurrenceUseSameSaleBegin, ");

            sqlCommand.Append("ShowAddToLinks, ");
            sqlCommand.Append("RequireTicketNotes, ");
            sqlCommand.Append("TicketNoteInfo, ");

            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("LastModUserGuid, ");
            sqlCommand.Append("LastModUtc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?ItemGuid, ");
            sqlCommand.Append("?ModuleID, ");
            sqlCommand.Append("?ModuleGuid, ");
            sqlCommand.Append("?CalendarGuid, ");
            sqlCommand.Append("?RecurrenceGuid, ");
            sqlCommand.Append("?Title, ");
            sqlCommand.Append("?Description, ");
            sqlCommand.Append("?ImageFile, ");
            sqlCommand.Append("?EventDate, ");
            sqlCommand.Append("?EndDate, ");
            sqlCommand.Append("?IsAllDay, ");
            sqlCommand.Append("?StartTime, ");
            sqlCommand.Append("?EndTime, ");
            sqlCommand.Append("?Recurrence, ");
            sqlCommand.Append("?Location, ");
            sqlCommand.Append("?ShowMap, ");
            sqlCommand.Append("?Url, ");
            sqlCommand.Append("?IncludeInFeed, ");
            sqlCommand.Append("?RequiresTicket, ");
            sqlCommand.Append("?TicketPrice, ");
            sqlCommand.Append("?SaleBeginUtc, ");
            sqlCommand.Append("?SaleEndUtc, ");
            sqlCommand.Append("?TotalSeats, ");
            sqlCommand.Append("?SoldSeats, ");
            sqlCommand.Append("?UserGuid, ");

            sqlCommand.Append("?RecurEndDate, ");
            sqlCommand.Append("?AllowWillPay, ");
            sqlCommand.Append("?LocationAlias, ");
            sqlCommand.Append("?ForeColor, ");
            sqlCommand.Append("?BackColor, ");
            sqlCommand.Append("?BorderColor, ");
            sqlCommand.Append("?GetTicketText, ");
            sqlCommand.Append("?MetaKeywords, ");
            sqlCommand.Append("?MetaDescription, ");
            sqlCommand.Append("?Excerpt, ");

            sqlCommand.Append("?MapHeight, ");
            sqlCommand.Append("?MapWidth, ");
            sqlCommand.Append("?MapType, ");
            sqlCommand.Append("?MapZoom, ");
            sqlCommand.Append("?ShowMapOptions, ");
            sqlCommand.Append("?ShowMapZoom, ");
            sqlCommand.Append("?ShowMapBalloon, ");
            sqlCommand.Append("?ShowMapSearch, ");
            sqlCommand.Append("?ShowMapDirections, ");
            sqlCommand.Append("?TicketIncludesRecur, ");
            sqlCommand.Append("?RecurrenceUseSameSaleBegin, ");

            sqlCommand.Append("?ShowAddToLinks, ");
            sqlCommand.Append("?RequireTicketNotes, ");
            sqlCommand.Append("?TicketNoteInfo, ");

            sqlCommand.Append("?CreatedUtc, ");
            sqlCommand.Append("?LastModUserGuid, ");
            sqlCommand.Append("?LastModUtc )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            MySqlParameter[] arParams = new MySqlParameter[52];

            arParams[0] = new MySqlParameter("?ItemGuid", MySqlDbType.VarChar, 37);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemGuid.ToString();

            arParams[1] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new MySqlParameter("?CalendarGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = calendarGuid.ToString();

            arParams[4] = new MySqlParameter("?RecurrenceGuid", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = recurrenceGuid.ToString();

            arParams[5] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = title;

            arParams[6] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = description;

            arParams[7] = new MySqlParameter("?ImageFile", MySqlDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = imageFile;

            arParams[8] = new MySqlParameter("?EventDate", MySqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = eventDate;

            arParams[9] = new MySqlParameter("?EndDate", MySqlDbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = endDate;

            arParams[10] = new MySqlParameter("?IsAllDay", MySqlDbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = intIsAllDay;

            arParams[11] = new MySqlParameter("?StartTime", MySqlDbType.DateTime);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = startTime;

            arParams[12] = new MySqlParameter("?EndTime", MySqlDbType.DateTime);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = endTime;

            arParams[13] = new MySqlParameter("?Recurrence", MySqlDbType.VarChar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = recurrence;

            arParams[14] = new MySqlParameter("?Location", MySqlDbType.Text);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = location;

            arParams[15] = new MySqlParameter("?ShowMap", MySqlDbType.Int32);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = intShowMap;

            arParams[16] = new MySqlParameter("?Url", MySqlDbType.VarChar, 255);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = url;

            arParams[17] = new MySqlParameter("?IncludeInFeed", MySqlDbType.Int32);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = intIncludeInFeed;

            arParams[18] = new MySqlParameter("?RequiresTicket", MySqlDbType.Int32);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = intRequiresTicket;

            arParams[19] = new MySqlParameter("?TicketPrice", MySqlDbType.Decimal);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = ticketPrice;

            arParams[20] = new MySqlParameter("?SaleBeginUtc", MySqlDbType.DateTime);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = saleBeginUtc;

            arParams[21] = new MySqlParameter("?SaleEndUtc", MySqlDbType.DateTime);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = saleEndUtc;

            arParams[22] = new MySqlParameter("?TotalSeats", MySqlDbType.Int32);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = totalSeats;

            arParams[23] = new MySqlParameter("?SoldSeats", MySqlDbType.Int32);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = 0;

            arParams[24] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = userGuid.ToString();

            arParams[25] = new MySqlParameter("?CreatedUtc", MySqlDbType.DateTime);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = createdUtc;

            arParams[26] = new MySqlParameter("?LastModUserGuid", MySqlDbType.VarChar, 36);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = userGuid.ToString();

            arParams[27] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = createdUtc;

            arParams[28] = new MySqlParameter("?RecurEndDate", MySqlDbType.DateTime);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = recurEndDate;

            arParams[29] = new MySqlParameter("?AllowWillPay", MySqlDbType.Int32);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = intallowWillPay;

            arParams[30] = new MySqlParameter("?LocationAlias", MySqlDbType.Text);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = locationAlias;

            arParams[31] = new MySqlParameter("?ForeColor", MySqlDbType.VarChar, 10);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = foreColor;

            arParams[32] = new MySqlParameter("?BackColor", MySqlDbType.VarChar, 10);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = backColor;

            arParams[33] = new MySqlParameter("?BorderColor", MySqlDbType.VarChar, 10);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = borderColor;

            arParams[34] = new MySqlParameter("?GetTicketText", MySqlDbType.VarChar, 255);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = getTicketText;

            arParams[35] = new MySqlParameter("?MetaKeywords", MySqlDbType.VarChar, 255);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = metaKeywords;

            arParams[36] = new MySqlParameter("?MetaDescription", MySqlDbType.VarChar, 255);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = metaDescription;

            arParams[37] = new MySqlParameter("?Excerpt", MySqlDbType.Text);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = excerpt;

            arParams[38] = new MySqlParameter("?MapHeight", MySqlDbType.VarChar, 10);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = mapHeight;

            arParams[39] = new MySqlParameter("?MapWidth", MySqlDbType.VarChar, 10);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = mapWidth;

            arParams[40] = new MySqlParameter("?MapType", MySqlDbType.VarChar, 20);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = mapType;

            arParams[41] = new MySqlParameter("?MapZoom", MySqlDbType.VarChar, 15);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = mapZoom;

            arParams[42] = new MySqlParameter("?ShowMapOptions", MySqlDbType.Int32);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = intshowMapOptions;

            arParams[43] = new MySqlParameter("?ShowMapZoom", MySqlDbType.Int32);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = intshowMapZoom;

            arParams[44] = new MySqlParameter("?ShowMapBalloon", MySqlDbType.Int32);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = intshowMapBalloon;

            arParams[45] = new MySqlParameter("?ShowMapSearch", MySqlDbType.Int32);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = intshowMapSearch;

            arParams[46] = new MySqlParameter("?ShowMapDirections", MySqlDbType.Int32);
            arParams[46].Direction = ParameterDirection.Input;
            arParams[46].Value = intshowMapDirections;

            arParams[47] = new MySqlParameter("?TicketIncludesRecur", MySqlDbType.Int32);
            arParams[47].Direction = ParameterDirection.Input;
            arParams[47].Value = intticketIncludesRecurrence;

            arParams[48] = new MySqlParameter("?RecurrenceUseSameSaleBegin", MySqlDbType.Int32);
            arParams[48].Direction = ParameterDirection.Input;
            arParams[48].Value = sameSalesDate;

            arParams[49] = new MySqlParameter("?ShowAddToLinks", MySqlDbType.Int32);
            arParams[49].Direction = ParameterDirection.Input;
            arParams[49].Value = intshowAddToLinks;

            arParams[50] = new MySqlParameter("?RequireTicketNotes", MySqlDbType.Int32);
            arParams[50].Direction = ParameterDirection.Input;
            arParams[50].Value = intrequireTicketNotes;

            arParams[51] = new MySqlParameter("?TicketNoteInfo", MySqlDbType.Text);
            arParams[51].Direction = ParameterDirection.Input;
            arParams[51].Value = ticketNoteInfo;


            int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return newID;


        }

        /// <summary>
        /// Updates a row in the mp_CalendarEvents table. Returns true if row updated.
        /// </summary>
        /// <param name="itemID"> itemID </param>
        /// <param name="title"> title </param>
        /// <param name="description"> description </param>
        /// <param name="imageName"> imageName </param>
        /// <param name="eventDate"> eventDate </param>
        /// <param name="startTime"> startTime </param>
        /// <param name="endTime"> endTime </param>
        /// <param name="location"> location </param>
        /// <param name="ticketPrice"> ticketPrice </param>
        /// <param name="requiresTicket"> requiresTicket </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="lastModUserGuid"> lastModUserGuid </param>
        /// <returns>bool</returns>
        public static bool UpdateCalendarEvent(
            int itemId,
            Guid recurrenceGuid,
            string title,
            string description,
            string imageFile,
            DateTime eventDate,
            DateTime endDate,
            DateTime startTime,
            DateTime endTime,
            bool isAllDay,
            string recurrence,
            string location,
            bool showMap,
            string url,
            bool includeInFeed,
            bool requiresTicket,
            decimal ticketPrice,
            DateTime saleBeginUtc,
            DateTime saleEndUtc,
            int totalSeats,
            DateTime lastModUtc,
            Guid lastModUserGuid,
            DateTime recurEndDate,
            bool allowWillPay,
            string locationAlias,
            string foreColor,
            string backColor,
            string borderColor,
            string getTicketText,
            string metaKeywords,
            string metaDescription,
            string excerpt,
            string mapHeight,
            string mapWidth,
            string mapType,
            string mapZoom,
            bool showMapOptions,
            bool showMapZoom,
            bool showMapBalloon,
            bool showMapSearch,
            bool showMapDirections,
            bool ticketIncludesRecurrence,
            bool useSameSaleDateForRecurrences,
            bool showAddToLinks,
            bool requireTicketNotes,
            string ticketNoteInfo)
        {
            #region Bit Conversion

            int intshowAddToLinks = 0;
            if (showAddToLinks) { intshowAddToLinks = 1; }

            int intrequireTicketNotes = 0;
            if (requireTicketNotes) { intrequireTicketNotes = 1; }

            int sameSalesDate = 0;
            if (useSameSaleDateForRecurrences) { sameSalesDate = 1; }

            int intticketIncludesRecurrence = 0;
            if (ticketIncludesRecurrence)
            {
                intticketIncludesRecurrence = 1;
            }

            int intshowMapOptions = 0;
            if (showMapOptions)
            {
                intshowMapOptions = 1;
            }

            int intshowMapZoom = 0;
            if (showMapZoom)
            {
                intshowMapZoom = 1;
            }

            int intshowMapBalloon = 0;
            if (showMapBalloon)
            {
                intshowMapBalloon = 1;
            }

            int intshowMapSearch = 0;
            if (showMapSearch)
            {
                intshowMapSearch = 1;
            }

            int intshowMapDirections = 0;
            if (showMapDirections)
            {
                intshowMapDirections = 1;
            }

            int intallowWillPay;
            if (allowWillPay)
            {
                intallowWillPay = 1;
            }
            else
            {
                intallowWillPay = 0;
            }

            int intIsAllDay;
            if (isAllDay)
            {
                intIsAllDay = 1;
            }
            else
            {
                intIsAllDay = 0;
            }

            int intShowMap;
            if (showMap)
            {
                intShowMap = 1;
            }
            else
            {
                intShowMap = 0;
            }

            int intIncludeInFeed;
            if (includeInFeed)
            {
                intIncludeInFeed = 1;
            }
            else
            {
                intIncludeInFeed = 0;
            }

            int intRequiresTicket;
            if (requiresTicket)
            {
                intRequiresTicket = 1;
            }
            else
            {
                intRequiresTicket = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_CalendarEvents ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("Title = ?Title, ");
            sqlCommand.Append("Description = ?Description, ");
            sqlCommand.Append("ImageFile = ?ImageFile, ");
            sqlCommand.Append("EventDate = ?EventDate, ");
            sqlCommand.Append("EndDate = ?EndDate, ");
            sqlCommand.Append("IsAllDay = ?IsAllDay, ");
            sqlCommand.Append("StartTime = ?StartTime, ");
            sqlCommand.Append("EndTime = ?EndTime, ");
            sqlCommand.Append("Recurrence = ?Recurrence, ");
            sqlCommand.Append("RecurrenceGuid = ?RecurrenceGuid, ");
            sqlCommand.Append("Location = ?Location, ");
            sqlCommand.Append("ShowMap = ?ShowMap, ");
            sqlCommand.Append("Url = ?Url, ");
            sqlCommand.Append("IncludeInFeed = ?IncludeInFeed, ");
            sqlCommand.Append("RequiresTicket = ?RequiresTicket, ");
            sqlCommand.Append("TicketPrice = ?TicketPrice, ");
            sqlCommand.Append("SaleBeginUtc = ?SaleBeginUtc, ");
            sqlCommand.Append("SaleEndUtc = ?SaleEndUtc, ");
            sqlCommand.Append("TotalSeats = ?TotalSeats, ");

            sqlCommand.Append("RecurEndDate = ?RecurEndDate, ");
            sqlCommand.Append("AllowWillPay = ?AllowWillPay, ");
            sqlCommand.Append("LocationAlias = ?LocationAlias, ");
            sqlCommand.Append("ForeColor = ?ForeColor, ");
            sqlCommand.Append("BackColor = ?BackColor, ");
            sqlCommand.Append("BorderColor = ?BorderColor, ");
            sqlCommand.Append("GetTicketText = ?GetTicketText, ");
            sqlCommand.Append("MetaKeywords = ?MetaKeywords, ");
            sqlCommand.Append("MetaDescription = ?MetaDescription, ");
            sqlCommand.Append("Excerpt = ?Excerpt, ");

            sqlCommand.Append("MapHeight = ?MapHeight, ");
            sqlCommand.Append("MapWidth = ?MapWidth, ");
            sqlCommand.Append("MapType = ?MapType, ");
            sqlCommand.Append("MapZoom = ?MapZoom, ");
            sqlCommand.Append("ShowMapOptions = ?ShowMapOptions, ");
            sqlCommand.Append("ShowMapZoom = ?ShowMapZoom, ");
            sqlCommand.Append("ShowMapBalloon = ?ShowMapBalloon, ");
            sqlCommand.Append("ShowMapSearch = ?ShowMapSearch, ");
            sqlCommand.Append("ShowMapDirections = ?ShowMapDirections, ");
            sqlCommand.Append("TicketIncludesRecur = ?TicketIncludesRecur, ");
            sqlCommand.Append("RecurrenceUseSameSaleBegin = ?RecurrenceUseSameSaleBegin, ");

            sqlCommand.Append("ShowAddToLinks = ?ShowAddToLinks, ");
            sqlCommand.Append("RequireTicketNotes = ?RequireTicketNotes, ");
            sqlCommand.Append("TicketNoteInfo = ?TicketNoteInfo, ");
           
            sqlCommand.Append("LastModUserGuid = ?LastModUserGuid, ");
            sqlCommand.Append("LastModUtc = ?LastModUtc ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ItemID = ?ItemID ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[46];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = description;

            arParams[3] = new MySqlParameter("?ImageFile", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = imageFile;

            arParams[4] = new MySqlParameter("?EventDate", MySqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = eventDate;

            arParams[5] = new MySqlParameter("?EndDate", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = endDate;

            arParams[6] = new MySqlParameter("?IsAllDay", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = intIsAllDay;

            arParams[7] = new MySqlParameter("?StartTime", MySqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = startTime;

            arParams[8] = new MySqlParameter("?EndTime", MySqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = endTime;

            arParams[9] = new MySqlParameter("?Recurrence", MySqlDbType.VarChar, 50);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = recurrence;

            arParams[10] = new MySqlParameter("?Location", MySqlDbType.Text);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = location;

            arParams[11] = new MySqlParameter("?ShowMap", MySqlDbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = intShowMap;

            arParams[12] = new MySqlParameter("?Url", MySqlDbType.VarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = url;

            arParams[13] = new MySqlParameter("?IncludeInFeed", MySqlDbType.Int32);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = intIncludeInFeed;

            arParams[14] = new MySqlParameter("?RequiresTicket", MySqlDbType.Int32);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = intRequiresTicket;

            arParams[15] = new MySqlParameter("?TicketPrice", MySqlDbType.Decimal);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = ticketPrice;

            arParams[16] = new MySqlParameter("?SaleBeginUtc", MySqlDbType.DateTime);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = saleBeginUtc;

            arParams[17] = new MySqlParameter("?SaleEndUtc", MySqlDbType.DateTime);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = saleEndUtc;

            arParams[18] = new MySqlParameter("?TotalSeats", MySqlDbType.Int32);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = totalSeats;

            arParams[19] = new MySqlParameter("?LastModUserGuid", MySqlDbType.VarChar, 36);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = lastModUserGuid.ToString();

            arParams[20] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = lastModUtc;

            arParams[21] = new MySqlParameter("?RecurEndDate", MySqlDbType.DateTime);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = recurEndDate;

            arParams[22] = new MySqlParameter("?AllowWillPay", MySqlDbType.Int32);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = intallowWillPay;

            arParams[23] = new MySqlParameter("?LocationAlias", MySqlDbType.Text);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = locationAlias;

            arParams[24] = new MySqlParameter("?ForeColor", MySqlDbType.VarChar, 10);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = foreColor;

            arParams[25] = new MySqlParameter("?BackColor", MySqlDbType.VarChar, 10);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = backColor;

            arParams[26] = new MySqlParameter("?BorderColor", MySqlDbType.VarChar, 10);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = borderColor;

            arParams[27] = new MySqlParameter("?GetTicketText", MySqlDbType.VarChar, 255);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = getTicketText;

            arParams[28] = new MySqlParameter("?MetaKeywords", MySqlDbType.VarChar, 255);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = metaKeywords;

            arParams[29] = new MySqlParameter("?MetaDescription", MySqlDbType.VarChar, 255);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = metaDescription;

            arParams[30] = new MySqlParameter("?Excerpt", MySqlDbType.Text);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = excerpt;

            arParams[31] = new MySqlParameter("?MapHeight", MySqlDbType.VarChar, 10);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = mapHeight;

            arParams[32] = new MySqlParameter("?MapWidth", MySqlDbType.VarChar, 10);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = mapWidth;

            arParams[33] = new MySqlParameter("?MapType", MySqlDbType.VarChar, 20);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = mapType;

            arParams[34] = new MySqlParameter("?MapZoom", MySqlDbType.VarChar, 15);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = mapZoom;

            arParams[35] = new MySqlParameter("?ShowMapOptions", MySqlDbType.Int32);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = intshowMapOptions;

            arParams[36] = new MySqlParameter("?ShowMapZoom", MySqlDbType.Int32);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = intshowMapZoom;

            arParams[37] = new MySqlParameter("?ShowMapBalloon", MySqlDbType.Int32);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = intshowMapBalloon;

            arParams[38] = new MySqlParameter("?ShowMapSearch", MySqlDbType.Int32);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = intshowMapSearch;

            arParams[39] = new MySqlParameter("?ShowMapDirections", MySqlDbType.Int32);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = intshowMapDirections;

            arParams[40] = new MySqlParameter("?TicketIncludesRecur", MySqlDbType.Int32);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = intticketIncludesRecurrence;

            arParams[41] = new MySqlParameter("?RecurrenceGuid", MySqlDbType.VarChar, 36);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = recurrenceGuid.ToString();

            arParams[42] = new MySqlParameter("?RecurrenceUseSameSaleBegin", MySqlDbType.Int32);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = sameSalesDate;

            arParams[43] = new MySqlParameter("?ShowAddToLinks", MySqlDbType.Int32);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = intshowAddToLinks;

            arParams[44] = new MySqlParameter("?RequireTicketNotes", MySqlDbType.Int32);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = intrequireTicketNotes;

            arParams[45] = new MySqlParameter("?TicketNoteInfo", MySqlDbType.Text);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = ticketNoteInfo;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        public static bool UpdateSoldTicketCount(Guid itemGuid, Guid recurrenceGuid, bool ticketIncludesRecurrence)
        {
            StringBuilder sqlCommand = new StringBuilder();
            
            sqlCommand.Append("UPDATE sts_CalendarEvents ");

            sqlCommand.Append("SET SoldSeats = COALESCE((  ");
            sqlCommand.Append("SELECT SUM(t.Quantity)  ");
            sqlCommand.Append("FROM sts_EventTicketOrder t  ");

            sqlCommand.Append("WHERE (t.EventGuid = ?ItemGuid   ");
            if (ticketIncludesRecurrence)
            {
                sqlCommand.Append(" OR t.RecurrenceGuid = ?RecurrenceGuid ");
            }
            sqlCommand.Append(")");

            sqlCommand.Append("AND t.OrderStatusGuid <> '00000000-0000-0000-0000-000000000000'  ");
            sqlCommand.Append("AND t.OrderStatusGuid <> 'de3b9331-b98f-493f-be5e-926ffe5003bc'  "); //cancelled

            sqlCommand.Append("),0) ");
            

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sts_CalendarEvents.ItemGuid = ?ItemGuid ");
            if (ticketIncludesRecurrence)
            {
                sqlCommand.Append("OR (sts_CalendarEvents.RecurrenceGuid = ?RecurrenceGuid AND sts_CalendarEvents.TicketIncludesRecur = 1) ");
            }
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ItemGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemGuid.ToString();

            arParams[1] = new MySqlParameter("?RecurrenceGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = recurrenceGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static bool UpdateRecurrenceTicketInfo(
            Guid recurrenceGuid,
            bool requiresTicket,
            bool ticketIncludesRecurrence,
            bool allowWillPay,
            decimal ticketPrice,
            int totalSeats,
            string getTicketText,
            DateTime saleBeginUtc,
            DateTime saleEndUtc,
            DateTime lastModUtc,
            Guid lastModUserGuid)
        {
            #region Bit Conversion

            int intticketIncludesRecurrence = 0;
            if (ticketIncludesRecurrence)
            {
                intticketIncludesRecurrence = 1;
            }

            int intallowWillPay;
            if (allowWillPay)
            {
                intallowWillPay = 1;
            }
            else
            {
                intallowWillPay = 0;
            }

            int intRequiresTicket;
            if (requiresTicket)
            {
                intRequiresTicket = 1;
            }
            else
            {
                intRequiresTicket = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_CalendarEvents ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("RequiresTicket = ?RequiresTicket, ");
            sqlCommand.Append("TicketIncludesRecur = ?TicketIncludesRecur, ");
            sqlCommand.Append("TicketPrice = ?TicketPrice, ");
            sqlCommand.Append("AllowWillPay = ?AllowWillPay, ");
            sqlCommand.Append("GetTicketText = ?GetTicketText, ");
            sqlCommand.Append("SaleBeginUtc = ?SaleBeginUtc, ");
            sqlCommand.Append("SaleEndUtc = ?SaleEndUtc, ");
            sqlCommand.Append("TotalSeats = ?TotalSeats, ");

            sqlCommand.Append("LastModUserGuid = ?LastModUserGuid, ");
            sqlCommand.Append("LastModUtc = ?LastModUtc ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RecurrenceGuid = ?RecurrenceGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[11];

            arParams[0] = new MySqlParameter("?RecurrenceGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = recurrenceGuid.ToString();

            arParams[1] = new MySqlParameter("?RequiresTicket", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = intRequiresTicket;

            arParams[2] = new MySqlParameter("?TicketIncludesRecur", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = intticketIncludesRecurrence;

            arParams[3] = new MySqlParameter("?TicketPrice", MySqlDbType.Decimal);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = ticketPrice;

            arParams[4] = new MySqlParameter("?AllowWillPay", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = intallowWillPay;

            arParams[5] = new MySqlParameter("?GetTicketText", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = getTicketText;

            arParams[6] = new MySqlParameter("?SaleBeginUtc", MySqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = saleBeginUtc;

            arParams[7] = new MySqlParameter("?SaleEndUtc", MySqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = saleEndUtc;

            arParams[8] = new MySqlParameter("?TotalSeats", MySqlDbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = totalSeats;

            arParams[9] = new MySqlParameter("?LastModUserGuid", MySqlDbType.VarChar, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModUserGuid.ToString();

            arParams[10] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = lastModUtc;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UnhookRecurrenceTickets(
            Guid recurrenceGuid,
            bool ticketIncludesRecurrence,
            DateTime lastModUtc,
            Guid lastModUserGuid)
        {
            #region Bit Conversion

            int intticketIncludesRecurrence = 0;
            if (ticketIncludesRecurrence)
            {
                intticketIncludesRecurrence = 1;
            }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_CalendarEvents ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("TicketIncludesRecur = ?TicketIncludesRecur, ");
            sqlCommand.Append("LastModUserGuid = ?LastModUserGuid, ");
            sqlCommand.Append("LastModUtc = ?LastModUtc ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RecurrenceGuid = ?RecurrenceGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?RecurrenceGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = recurrenceGuid.ToString();

            arParams[1] = new MySqlParameter("?TicketIncludesRecur", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = intticketIncludesRecurrence;

            arParams[2] = new MySqlParameter("?LastModUserGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = lastModUserGuid.ToString();

            arParams[3] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = lastModUtc;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool DeleteCalendarEvent(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = ?ItemID ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("DELETE FROM mp_FriendlyUrls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid IN (SELECT ItemGuid FROM sts_CalendarEvents WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = ?SiteID)) ");
            sqlCommand.Append("; ");

            sqlCommand.Append("DELETE FROM sts_EventTicketOrder ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleId IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = ?SiteID) ");
            sqlCommand.Append("; ");

            sqlCommand.Append("DELETE FROM sts_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = ?SiteID) ");
            sqlCommand.Append("; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("DELETE FROM mp_FriendlyUrls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid IN (SELECT ItemGuid FROM sts_CalendarEvents WHERE ModuleID = ?ModuleID) ");
            sqlCommand.Append("; ");

            sqlCommand.Append("DELETE FROM sts_EventTicketOrder ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleId = ?ModuleID ");
            sqlCommand.Append("; ");

            sqlCommand.Append("DELETE FROM sts_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = ?ModuleID ");
            sqlCommand.Append("; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the sts_CalendarEvents table.
        /// </summary>
        public static IDataReader GetForSiteMap()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Url, LastModUtc ");
            sqlCommand.Append("FROM	sts_CalendarEvents ");
            sqlCommand.Append(";");

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                null);
        }

        public static IDataReader GetCalendarsForSeparateViews(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT DISTINCT ");

            sqlCommand.Append("pm.PageID, ");
            sqlCommand.Append("m.ModuleID, ");
            sqlCommand.Append("m.ModuleTitle ");

            sqlCommand.Append("FROM mp_Modules m ");

            sqlCommand.Append("JOIN mp_PageModules pm ");
            sqlCommand.Append("ON pm.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN mp_ModuleSettings ms ");
            sqlCommand.Append("ON ms.ModuleID = m.ModuleID ");
            sqlCommand.Append("AND ms.SettingName = 'AllowViewsOnOtherPages' ");

            sqlCommand.Append("WHERE ");

            sqlCommand.Append("m.SiteID = ?SiteID ");

            sqlCommand.Append("AND ");
            sqlCommand.Append("m.FeatureGuid = '5a343d88-bce1-43d1-98ae-b42d77893e7b' ");

            sqlCommand.Append("AND ");
            sqlCommand.Append(" ms.SettingValue = 'true' ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetCalendarEvent(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = ?ItemID ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetCalendarEvent(Guid itemGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemGuid = ?ItemGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetForFeed(int moduleId, DateTime beginDate, DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = ?ModuleID ");
            sqlCommand.Append("AND EndDate >= ?BeginDate ");
            sqlCommand.Append("AND EndDate <= ?EndDate ");
            sqlCommand.Append("ORDER BY EventDate ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?BeginDate", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new MySqlParameter("?EndDate", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = endDate;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        private static DataTable GetEmptyDataTable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("ItemID", typeof(int));
            dt.Columns.Add("ItemGuid", typeof(string));
            dt.Columns.Add("ModuleID", typeof(int));
            dt.Columns.Add("ModuleGuid", typeof(string));
            dt.Columns.Add("CalendarGuid", typeof(string));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("ImageFile", typeof(string));
            dt.Columns.Add("EventDate", typeof(DateTime));
            dt.Columns.Add("EndDate", typeof(DateTime));
            dt.Columns.Add("StartTime", typeof(DateTime));
            dt.Columns.Add("EndTime", typeof(DateTime));
            dt.Columns.Add("Recurrence", typeof(string));
            dt.Columns.Add("Location", typeof(string));
            dt.Columns.Add("Url", typeof(string));
            dt.Columns.Add("IncludeInFeed", typeof(bool));
            dt.Columns.Add("RequiresTicket", typeof(bool));
            dt.Columns.Add("TicketPrice", typeof(decimal));
            dt.Columns.Add("SaleBeginUtc", typeof(DateTime));
            dt.Columns.Add("SaleEndUtc", typeof(DateTime));
            dt.Columns.Add("TotalSeats", typeof(int));
            dt.Columns.Add("SoldSeats", typeof(int));

            dt.Columns.Add("ForeColor", typeof(string));
            dt.Columns.Add("BackColor", typeof(string));
            dt.Columns.Add("BorderColor", typeof(string));
            dt.Columns.Add("LocationAlias", typeof(string));
            dt.Columns.Add("GetTicketText", typeof(string));

            return dt;

        }

        public static DataTable GetEventRecurrences(Guid recurrenceGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RecurrenceGuid = ?RecurrenceGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?RecurrenceGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = recurrenceGuid.ToString(); 

            DataTable dt = GetEmptyDataTable();

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {

                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["ItemID"] = Convert.ToInt32(reader["ItemID"]);
                    row["ItemGuid"] = reader["ItemGuid"].ToString();
                    row["ModuleID"] = Convert.ToInt32(reader["ModuleID"]);
                    row["ModuleGuid"] = reader["ModuleGuid"].ToString();
                    row["CalendarGuid"] = reader["CalendarGuid"].ToString();
                    row["Title"] = reader["Title"];
                    row["Description"] = reader["Description"];
                    row["ImageFile"] = reader["ImageFile"];
                    row["EventDate"] = reader["EventDate"];
                    row["EndDate"] = reader["EndDate"];
                    row["StartTime"] = reader["StartTime"];
                    row["EndTime"] = reader["EndTime"];
                    row["Recurrence"] = reader["Recurrence"];
                    row["Location"] = reader["Location"];
                    row["Url"] = reader["Url"];
                    row["IncludeInFeed"] = Convert.ToBoolean(reader["IncludeInFeed"]);
                    row["RequiresTicket"] = Convert.ToBoolean(reader["RequiresTicket"]);
                    row["TicketPrice"] = Convert.ToDecimal(reader["TicketPrice"]);
                    row["SaleBeginUtc"] = reader["SaleBeginUtc"];
                    row["SaleEndUtc"] = reader["SaleEndUtc"];
                    row["TotalSeats"] = Convert.ToInt32(reader["TotalSeats"]);
                    row["SoldSeats"] = Convert.ToInt32(reader["SoldSeats"]);

                    row["ForeColor"] = reader["ForeColor"];
                    row["BackColor"] = reader["BackColor"];
                    row["BorderColor"] = reader["BorderColor"];
                    row["LocationAlias"] = reader["LocationAlias"];
                    row["GetTicketText"] = reader["GetTicketText"];

                    dt.Rows.Add(row);

                }

            }
            return dt;


        }

        

        public static DataTable GetEventsTable(
                int moduleId,
                DateTime beginDate,
                DateTime endDate)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_CalendarEvents ");
            sqlCommand.Append("WHERE ");

            // event starts within range
            sqlCommand.Append("(ModuleID = ?ModuleID ");
            sqlCommand.Append("AND (EventDate >= ?BeginDate) ");
            sqlCommand.Append("AND (EventDate <= ?EndDate)) ");

            sqlCommand.Append(" OR ");

            // event starts before range but ends within range
            sqlCommand.Append("(ModuleID = ?ModuleID ");
            sqlCommand.Append("AND (?BeginDate >= EventDate) ");
            sqlCommand.Append("AND (?EndDate >= EndDate)) ");

            sqlCommand.Append(" OR ");

            // event starts before range and ends after range
            sqlCommand.Append("(ModuleID = ?ModuleID ");
            sqlCommand.Append("AND (?BeginDate >= EventDate) ");
            sqlCommand.Append("AND (?EndDate <= EndDate)) ");


            sqlCommand.Append("ORDER BY EventDate ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?BeginDate", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new MySqlParameter("?EndDate", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = endDate;

            DataTable dt = GetEmptyDataTable();

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {

                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["ItemID"] = Convert.ToInt32(reader["ItemID"]);
                    row["ItemGuid"] = reader["ItemGuid"].ToString();
                    row["ModuleID"] = Convert.ToInt32(reader["ModuleID"]);
                    row["ModuleGuid"] = reader["ModuleGuid"].ToString();
                    row["CalendarGuid"] = reader["CalendarGuid"].ToString();
                    row["Title"] = reader["Title"];
                    row["Description"] = reader["Description"];
                    row["ImageFile"] = reader["ImageFile"];
                    row["EventDate"] = reader["EventDate"];
                    row["EndDate"] = reader["EndDate"];
                    row["StartTime"] = reader["StartTime"];
                    row["EndTime"] = reader["EndTime"];
                    row["Recurrence"] = reader["Recurrence"];
                    row["Location"] = reader["Location"];
                    row["Url"] = reader["Url"];
                    row["IncludeInFeed"] = Convert.ToBoolean(reader["IncludeInFeed"]);
                    row["RequiresTicket"] = Convert.ToBoolean(reader["RequiresTicket"]);
                    row["TicketPrice"] = Convert.ToDecimal(reader["TicketPrice"]);
                    row["SaleBeginUtc"] = reader["SaleBeginUtc"];
                    row["SaleEndUtc"] = reader["SaleEndUtc"];
                    row["TotalSeats"] = Convert.ToInt32(reader["TotalSeats"]);
                    row["SoldSeats"] = Convert.ToInt32(reader["SoldSeats"]);

                    row["ForeColor"] = reader["ForeColor"];
                    row["BackColor"] = reader["BackColor"];
                    row["BorderColor"] = reader["BorderColor"];
                    row["LocationAlias"] = reader["LocationAlias"];
                    row["GetTicketText"] = reader["GetTicketText"];

                    dt.Rows.Add(row);

                }

            }
            return dt;
        }

        /// <summary>
        /// Gets a count of rows in the sts_CalendarEvents table.
        /// </summary>
        public static int GetCount(int moduleId, DateTime beginDateUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = ?ModuleID ");
            sqlCommand.Append("AND ((EventDate >= ?BeginDateUtc) OR (EndDate >= ?BeginDateUtc)) ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?BeginDateUtc", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDateUtc;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets a page of data from the sts_CalendarEvents table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            int moduleId,
            DateTime beginDateUtc,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows
                = GetCount(moduleId, beginDateUtc);

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
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = ?ModuleID ");

            sqlCommand.Append("AND ((EventDate >= ?BeginDateUtc) OR (EndDate >= ?BeginDateUtc)) ");

            sqlCommand.Append("ORDER BY EventDate ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?BeginDateUtc", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDateUtc;

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

        public static IDataReader GetEventsByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ce.*, ");
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("m.ViewRoles, ");
            sqlCommand.Append("md.FeatureName ");
            
            sqlCommand.Append("FROM	sts_CalendarEvents ce ");

            sqlCommand.Append("JOIN	mp_Modules m ");
            sqlCommand.Append("ON ce.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN	mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("JOIN	mp_PageModules pm ");
            sqlCommand.Append("ON pm.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN	mp_Pages p ");
            sqlCommand.Append("ON p.PageID = pm.PageID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.SiteID = ?SiteID ");
            sqlCommand.Append("AND pm.PageID = ?PageID ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?PageID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


    }
}
