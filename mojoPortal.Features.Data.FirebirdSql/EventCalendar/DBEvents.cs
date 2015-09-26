/// Author:					Joe Audette
/// Created:				2007-11-03
/// Last Modified:			2010-12-06
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using FirebirdSql.Data.FirebirdClient;


namespace mojoPortal.Data
{
    
    public static class DBEvents
    {
        
        public static String DBPlatform()
        {
            return "FirebirdSql";
        }

        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

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
            #region Bit Conversion

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


            FbParameter[] arParams = new FbParameter[15];

            arParams[0] = new FbParameter(":ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter(":Title", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new FbParameter(":Description", FbDbType.VarChar);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = description;

            arParams[3] = new FbParameter(":ImageName", FbDbType.VarChar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = imageName;

            arParams[4] = new FbParameter(":EventDate", FbDbType.TimeStamp);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = eventDate;

            arParams[5] = new FbParameter(":StartTime", FbDbType.Time);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = startTime;

            arParams[6] = new FbParameter(":EndTime", FbDbType.Time);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = endTime;

            arParams[7] = new FbParameter(":CreatedDate", FbDbType.TimeStamp);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = DateTime.UtcNow;

            arParams[8] = new FbParameter(":UserID", FbDbType.Integer);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = userId;

            arParams[9] = new FbParameter(":ItemGuid", FbDbType.Char, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = itemGuid.ToString();

            arParams[10] = new FbParameter(":ModuleGuid", FbDbType.Char, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = moduleGuid.ToString();

            arParams[11] = new FbParameter(":UserGuid", FbDbType.Char, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = userGuid.ToString();

            arParams[12] = new FbParameter(":Location", FbDbType.VarChar);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = location;

            arParams[13] = new FbParameter(":TicketPrice", FbDbType.Decimal);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = ticketPrice;

            arParams[14] = new FbParameter(":RequiresTicket", FbDbType.SmallInt);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = intRequiresTicket;

            int newID = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_CALENDAREVENTS_INSERT ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
                arParams));

            return newID;

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

            #region Bit Conversion
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

            sqlCommand.Append("UPDATE mp_CalendarEvents ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("ModuleID = @ModuleID, ");
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
            sqlCommand.Append("ItemID = @ItemID ;");

            FbParameter[] arParams = new FbParameter[13];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new FbParameter("@Title", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new FbParameter("@Description", FbDbType.VarChar);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new FbParameter("@ImageName", FbDbType.VarChar, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = imageName;

            arParams[5] = new FbParameter("@EventDate", FbDbType.TimeStamp);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = eventDate;

            arParams[6] = new FbParameter("@StartTime", FbDbType.TimeStamp);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = startTime;

            arParams[7] = new FbParameter("@EndTime", FbDbType.TimeStamp);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = endTime;

            arParams[8] = new FbParameter("@Location", FbDbType.VarChar);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = location;

            arParams[9] = new FbParameter("@LastModUserGuid", FbDbType.Char, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModUserGuid.ToString();

            arParams[10] = new FbParameter("@LastModUtc", FbDbType.TimeStamp);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = lastModUtc;

            arParams[11] = new FbParameter("@TicketPrice", FbDbType.Decimal);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = ticketPrice;

            arParams[12] = new FbParameter("@RequiresTicket", FbDbType.SmallInt);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = intRequiresTicket;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool DeleteCalendarEvent(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = @ItemID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID  = @ModuleID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID) ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }


        public static IDataReader GetCalendarEvent(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ItemID, ");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("ImageName, ");
            sqlCommand.Append("EventDate, ");
            sqlCommand.Append("CAST(StartTime AS TIMESTAMP) As StartTime, ");
            sqlCommand.Append("CAST(EndTime AS TIMESTAMP) As EndTime, ");
            sqlCommand.Append("CreatedDate, ");
            sqlCommand.Append("UserID, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("LastModUserGuid, ");
            sqlCommand.Append("Location, ");
            sqlCommand.Append("LastModUtc, ");
            sqlCommand.Append("RequiresTicket, ");
            sqlCommand.Append("TicketPrice ");

            sqlCommand.Append("FROM	mp_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = @ItemID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static DataSet GetEvents(
            int moduleId,
            DateTime beginDate,
            DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            FbParameter[] arParams = new FbParameter[3];
            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[1] = new FbParameter("@BeginDate", FbDbType.TimeStamp);
            arParams[2] = new FbParameter("@EndDate", FbDbType.TimeStamp);

            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;


            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ItemID, ");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("ImageName, ");
            sqlCommand.Append("EventDate, ");
            sqlCommand.Append("CAST(StartTime AS TIMESTAMP) As StartTime, ");
            sqlCommand.Append("CAST(EndTime AS TIMESTAMP) As EndTime, ");
            sqlCommand.Append("CreatedDate, ");
            sqlCommand.Append("UserID, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("LastModUserGuid, ");
            sqlCommand.Append("Location, ");
            sqlCommand.Append("LastModUtc, ");
            sqlCommand.Append("RequiresTicket, ");
            sqlCommand.Append("TicketPrice ");
            

            sqlCommand.Append("FROM	mp_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ");

            sqlCommand.Append("AND EventDate >= @BeginDate ");
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;


            sqlCommand.Append("AND EventDate <= @EndDate ");
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = endDate;


            sqlCommand.Append(" ;");

            return FBSqlHelper.ExecuteDataset(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static DataTable GetEventsTable(
            int moduleId,
            DateTime beginDate,
            DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            FbParameter[] arParams = new FbParameter[3];
            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[1] = new FbParameter("@BeginDate", FbDbType.TimeStamp);
            arParams[2] = new FbParameter("@EndDate", FbDbType.TimeStamp);

            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;


            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ItemID, ");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("ImageName, ");
            sqlCommand.Append("EventDate, ");
            sqlCommand.Append("CAST(StartTime AS TIMESTAMP) As StartTime, ");
            sqlCommand.Append("CAST(EndTime AS TIMESTAMP) As EndTime, ");
            sqlCommand.Append("CreatedDate, ");
            sqlCommand.Append("UserID, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("LastModUserGuid, ");
            sqlCommand.Append("Location, ");
            sqlCommand.Append("LastModUtc, ");
            sqlCommand.Append("RequiresTicket, ");
            sqlCommand.Append("TicketPrice ");

            sqlCommand.Append("FROM	mp_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ");

            sqlCommand.Append("AND EventDate >= @BeginDate ");

            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;


            sqlCommand.Append("AND EventDate <= @EndDate ");
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = endDate;



            sqlCommand.Append(" ;");

            DataTable dt = new DataTable();

            dt.Columns.Add("ItemID", typeof(int));
            dt.Columns.Add("ModuleID", typeof(int));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("EventDate", typeof(DateTime));

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ce.ItemID, ");
            sqlCommand.Append("ce.ModuleID, ");
            sqlCommand.Append("ce.Title, ");
            sqlCommand.Append("ce.Description, ");
            sqlCommand.Append("ce.ImageName, ");
            sqlCommand.Append("ce.EventDate, ");
            sqlCommand.Append("CAST(ce.StartTime AS TIMESTAMP) As StartTime, ");
            sqlCommand.Append("CAST(ce.EndTime AS TIMESTAMP) As EndTime, ");
            sqlCommand.Append("ce.CreatedDate, ");
            sqlCommand.Append("ce.UserID, ");
            sqlCommand.Append("ce.ItemGuid, ");
            sqlCommand.Append("ce.ModuleGuid, ");
            sqlCommand.Append("ce.UserGuid, ");
            sqlCommand.Append("ce.LastModUserGuid, ");
            sqlCommand.Append("ce.Location, ");
            sqlCommand.Append("ce.LastModUtc, ");
            sqlCommand.Append("ce.RequiresTicket, ");
            sqlCommand.Append("ce.TicketPrice, ");

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

            sqlCommand.Append(" ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }





    }
}
