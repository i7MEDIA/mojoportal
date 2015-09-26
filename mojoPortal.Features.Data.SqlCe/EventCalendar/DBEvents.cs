// Author:					Joe Audette
// Created:				    2010-07-02
// Last Modified:			2010-07-04
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 
// Note moved into separate class file from dbPortal 2007-11-03

using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;

namespace mojoPortal.Data
{
    public static class DBEvents
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
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
            int moduleId,
            string title,
            string description,
            string imageName,
            DateTime eventDate,
            DateTime startTime,
            DateTime endTime,
            int userId,
            Guid userGuid,
            string location,
            bool requiresTicket,
            decimal ticketPrice,
            DateTime createdDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_CalendarEvents ");
            sqlCommand.Append("(");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("ImageName, ");
            sqlCommand.Append("EventDate, ");
            sqlCommand.Append("StartTime, ");
            sqlCommand.Append("EndTime, ");
            sqlCommand.Append("CreatedDate, ");
            sqlCommand.Append("UserID, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("Location, ");
            sqlCommand.Append("LastModUserGuid, ");
            sqlCommand.Append("LastModUtc, ");
            sqlCommand.Append("TicketPrice, ");
            sqlCommand.Append("RequiresTicket ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ModuleID, ");
            sqlCommand.Append("@Title, ");
            sqlCommand.Append("@Description, ");
            sqlCommand.Append("@ImageName, ");
            sqlCommand.Append("@EventDate, ");
            sqlCommand.Append("@StartTime, ");
            sqlCommand.Append("@EndTime, ");
            sqlCommand.Append("@CreatedDate, ");
            sqlCommand.Append("@UserID, ");
            sqlCommand.Append("@ItemGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@Location, ");
            sqlCommand.Append("@LastModUserGuid, ");
            sqlCommand.Append("@LastModUtc, ");
            sqlCommand.Append("@TicketPrice, ");
            sqlCommand.Append("@RequiresTicket ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[17];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new SqlCeParameter("@Description", SqlDbType.NText);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = description;

            arParams[3] = new SqlCeParameter("@ImageName", SqlDbType.NVarChar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = imageName;

            arParams[4] = new SqlCeParameter("@EventDate", SqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = eventDate;

            arParams[5] = new SqlCeParameter("@StartTime", SqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = startTime;

            arParams[6] = new SqlCeParameter("@EndTime", SqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = endTime;

            arParams[7] = new SqlCeParameter("@CreatedDate", SqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createdDate;

            arParams[8] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = userId;

            arParams[9] = new SqlCeParameter("@ItemGuid", SqlDbType.UniqueIdentifier);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = itemGuid;

            arParams[10] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = moduleGuid;

            arParams[11] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = userGuid;

            arParams[12] = new SqlCeParameter("@Location", SqlDbType.NText);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = location;

            arParams[13] = new SqlCeParameter("@LastModUserGuid", SqlDbType.UniqueIdentifier);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = userGuid;

            arParams[14] = new SqlCeParameter("@LastModUtc", SqlDbType.DateTime);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = createdDate;

            arParams[15] = new SqlCeParameter("@TicketPrice", SqlDbType.Decimal);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = ticketPrice;

            arParams[16] = new SqlCeParameter("@RequiresTicket", SqlDbType.Bit);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = requiresTicket;


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;


        }

        /// <summary>
        /// Updates a row in the mp_CalendarEvents table. Returns true if row updated.
        /// </summary>
        /// <param name="itemID"> itemID </param>
        /// <param name="moduleID"> moduleID </param>
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
            int moduleId,
            string title,
            string description,
            string imageName,
            DateTime eventDate,
            DateTime startTime,
            DateTime endTime,
            string location,
            bool requiresTicket,
            decimal ticketPrice,
            DateTime lastModUtc,
            Guid lastModUserGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_CalendarEvents ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("Title = @Title, ");
            sqlCommand.Append("Description = @Description, ");
            sqlCommand.Append("ImageName = @ImageName, ");
            sqlCommand.Append("EventDate = @EventDate, ");
            sqlCommand.Append("StartTime = @StartTime, ");
            sqlCommand.Append("EndTime = @EndTime, ");
            sqlCommand.Append("Location = @Location, ");
            sqlCommand.Append("LastModUserGuid = @LastModUserGuid, ");
            sqlCommand.Append("LastModUtc = @LastModUtc, ");
            sqlCommand.Append("TicketPrice = @TicketPrice, ");
            sqlCommand.Append("RequiresTicket = @RequiresTicket ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ItemID = @ItemID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[13];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new SqlCeParameter("@Description", SqlDbType.NVarChar);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new SqlCeParameter("@ImageName", SqlDbType.NVarChar, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = imageName;

            arParams[5] = new SqlCeParameter("@EventDate", SqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = eventDate;

            arParams[6] = new SqlCeParameter("@StartTime", SqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = startTime;

            arParams[7] = new SqlCeParameter("@EndTime", SqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = endTime;

            arParams[8] = new SqlCeParameter("@Location", SqlDbType.NVarChar);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = location;

            arParams[9] = new SqlCeParameter("@LastModUserGuid", SqlDbType.UniqueIdentifier);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModUserGuid;

            arParams[10] = new SqlCeParameter("@LastModUtc", SqlDbType.DateTime);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = lastModUtc;

            arParams[11] = new SqlCeParameter("@TicketPrice", SqlDbType.Decimal);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = ticketPrice;

            arParams[12] = new SqlCeParameter("@RequiresTicket", SqlDbType.Bit);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = requiresTicket;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool DeleteCalendarEvent(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = @ItemID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID) ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static IDataReader GetCalendarEvent(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = @ItemID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static DataSet GetEvents(
                int moduleId,
                DateTime beginDate,
                DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append("AND EventDate >= @BeginDate ");
            sqlCommand.Append("AND EventDate <= @EndDate ");
            sqlCommand.Append("ORDER BY	EventDate ");
            sqlCommand.Append(" ;");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@BeginDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SqlCeParameter("@EndDate", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = endDate;

            return SqlHelper.ExecuteDataset(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static DataTable GetEventsTable(
                int moduleId,
                DateTime beginDate,
                DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append("AND EventDate >= @BeginDate ");
            sqlCommand.Append("AND EventDate <= @EndDate ");
            sqlCommand.Append("ORDER BY	EventDate ");
            sqlCommand.Append(" ;");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@BeginDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SqlCeParameter("@EndDate", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = endDate;

            DataTable dt = new DataTable();

            dt.Columns.Add("ItemID", typeof(int));
            dt.Columns.Add("ModuleID", typeof(int));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("EventDate", typeof(DateTime));

            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["ItemID"] = reader["ItemID"];
                    row["ModuleID"] = reader["ModuleID"];
                    row["Title"] = reader["Title"];
                    row["EventDate"] = reader["EventDate"];
                    dt.Rows.Add(row);
                }
            }

            return dt;

        }

        public static IDataReader GetEventsByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ce.*, ");

            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("m.ViewRoles, ");
            sqlCommand.Append("md.FeatureName ");

            sqlCommand.Append("FROM	mp_CalendarEvents ce ");

            sqlCommand.Append("JOIN	mp_Modules m ");
            sqlCommand.Append("ON ce.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN	mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("JOIN	mp_PageModules pm ");
            sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");

            sqlCommand.Append("JOIN	mp_Pages p ");
            sqlCommand.Append("ON p.PageID = pm.PageID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.SiteID = @SiteID ");
            sqlCommand.Append("AND pm.PageID = @PageID ");
            sqlCommand.Append(" ; ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

    }
}
