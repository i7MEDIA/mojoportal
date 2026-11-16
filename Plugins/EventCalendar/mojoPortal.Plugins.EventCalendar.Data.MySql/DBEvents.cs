using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;

namespace mojoPortal.Data
{
	public static class DBEvents
    {
        
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
            DateTime createdDate,
			bool showMap)
        {
			#region Bit Conversion
			int intRequiresTicket = requiresTicket ? 1 : 0;
            //if (requiresTicket)
            //{
            //    intRequiresTicket = 1;
            //}
            //else
            //{
            //    intRequiresTicket = 0;
            //}

			int intShowMap = showMap ? 1 : 0;

            #endregion


            StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.AppendLine(@"INSERT INTO mp_CalendarEvents (
				ModuleID, 
				Title, 
				Description, 
				ImageName, 
				EventDate,
				StartTime, 
				EndTime,
				CreatedDate, 
				ItemGuid,
				ModuleGuid, 
				UserGuid,
				Location, 
				LastModUserGuid,
				LastModUtc,
				TicketPrice,
				RequiresTicket,
				UserID,
				ShowMap)");

            sqlCommand.AppendLine(@" VALUES (
				?ModuleID, 
				?Title, 
				?Description, 
				?ImageName, 
				?EventDate, 
				?StartTime, 
				?EndTime, 
				?CreatedDate, 
				?ItemGuid, 
				?ModuleGuid, 
				?UserGuid, 
				?Location, 
				?UserGuid, 
				?CreatedDate, 
				?TicketPrice, 
				?RequiresTicket, 
				?UserID,
				?ShowMap);");

            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            MySqlParameter[] arParams = new MySqlParameter[16];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new MySqlParameter("?Description", MySqlDbType.LongText);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = description;

            arParams[3] = new MySqlParameter("?ImageName", MySqlDbType.VarChar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = imageName;

            arParams[4] = new MySqlParameter("?EventDate", MySqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = eventDate;

            arParams[5] = new MySqlParameter("?StartTime", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = startTime;

            arParams[6] = new MySqlParameter("?EndTime", MySqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = endTime;

            arParams[7] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = userId;


            arParams[8] = new MySqlParameter("?ItemGuid", MySqlDbType.VarChar, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = itemGuid.ToString();

            arParams[9] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = moduleGuid.ToString();

            arParams[10] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = userGuid.ToString();

            arParams[11] = new MySqlParameter("?Location", MySqlDbType.Text);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = location;

            arParams[12] = new MySqlParameter("?RequiresTicket", MySqlDbType.Int32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = intRequiresTicket;

            arParams[13] = new MySqlParameter("?TicketPrice", MySqlDbType.Decimal);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = ticketPrice;

            arParams[14] = new MySqlParameter("?CreatedDate", MySqlDbType.DateTime);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = createdDate;

			arParams[15] = new MySqlParameter("?ShowMap", MySqlDbType.Int32);
			arParams[15].Direction = ParameterDirection.Input;
			arParams[15].Value = intShowMap;

			int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return newID;

        }


        /// <summary>
        /// Updates a row in the mp_CalendarEvents table. Returns true if row updated.
        /// </summary>
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
            Guid lastModUserGuid,
			bool showMap)
        {
			#region Bit Conversion

			int intRequiresTicket = requiresTicket ? 1 : 0;
			//if (requiresTicket)
			//{
			//    intRequiresTicket = 1;
			//}
			//else
			//{
			//    intRequiresTicket = 0;
			//}

			int intShowMap = showMap ? 1 : 0;


			#endregion

			StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_CalendarEvents ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("ModuleID = ?ModuleID, ");
            sqlCommand.Append("Title = ?Title, ");
            sqlCommand.Append("Description = ?Description, ");
            sqlCommand.Append("ImageName = ?ImageName, ");
            sqlCommand.Append("EventDate = ?EventDate, ");
            sqlCommand.Append("StartTime = ?StartTime, ");
            sqlCommand.Append("EndTime = ?EndTime, ");
            sqlCommand.Append("Location = ?Location, ");
            sqlCommand.Append("LastModUserGuid = ?LastModUserGuid, ");
            sqlCommand.Append("LastModUtc = ?LastModUtc, ");
            sqlCommand.Append("TicketPrice = ?TicketPrice, ");
			sqlCommand.Append("RequiresTicket = ?RequiresTicket,");
			sqlCommand.Append("ShowMap = ?ShowMap ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ItemID = ?ItemID ;");

            MySqlParameter[] arParams = new MySqlParameter[14];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new MySqlParameter("?Description", MySqlDbType.LongText);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new MySqlParameter("?ImageName", MySqlDbType.VarChar, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = imageName;

            arParams[5] = new MySqlParameter("?EventDate", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = eventDate;

            arParams[6] = new MySqlParameter("?StartTime", MySqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = startTime;

            arParams[7] = new MySqlParameter("?EndTime", MySqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = endTime;

            arParams[8] = new MySqlParameter("?LastModUserGuid", MySqlDbType.VarChar, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModUserGuid.ToString();

            arParams[9] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModUtc;

            arParams[10] = new MySqlParameter("?TicketPrice", MySqlDbType.Decimal);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = ticketPrice;

            arParams[11] = new MySqlParameter("?RequiresTicket", MySqlDbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = intRequiresTicket;

            arParams[12] = new MySqlParameter("?Location", MySqlDbType.Text);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = location;

			arParams[13] = new MySqlParameter("?ShowMap", MySqlDbType.Int32);
			arParams[13].Direction = ParameterDirection.Input;
			arParams[13].Value = intShowMap;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool DeleteCalendarEvent(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = ?ItemID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID  = ?ModuleID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = ?SiteID) ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        public static IDataReader GetCalendarEvent(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = ?ItemID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static DataSet GetEvents(
            int moduleId,
            DateTime beginDate,
            DateTime endDate)
        {
            
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_CalendarEvents ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = ?ModuleID ");
            sqlCommand.Append("AND EventDate >= ?BeginDate ");
            sqlCommand.Append("AND EventDate <= ?EndDate ");
            sqlCommand.Append("ORDER BY	EventDate ");
            sqlCommand.Append(" ;");

            return MySqlHelper.ExecuteDataset(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static DataTable GetEventsTable(
            int moduleId,
            DateTime beginDate,
            DateTime endDate)
        {
            
            var sqlParams = new List<MySqlParameter>()
            {
				new MySqlParameter("?ModuleID", MySqlDbType.Int32) {Direction = ParameterDirection.Input, Value = moduleId },
				new MySqlParameter("?BeginDate", MySqlDbType.DateTime) {Direction = ParameterDirection.Input,Value = beginDate },
				new MySqlParameter("?EndDate", MySqlDbType.DateTime) {Direction = ParameterDirection.Input,Value = endDate }
			};

            var sqlCommand = @"
SELECT  * 
FROM mp_CalendarEvents 
WHERE ModuleID = ?ModuleID
AND EventDate >= ?BeginDate 
AND EventDate <= ?EndDate 
ORDER BY EventDate;";

            DataTable dt = new DataTable();

			dt.Columns.Add("ItemID", typeof(int));
			dt.Columns.Add("ModuleID", typeof(int));
			dt.Columns.Add("Title", typeof(string));
			dt.Columns.Add("Description", typeof(string));
			dt.Columns.Add("ImageName", typeof(string));
			dt.Columns.Add("EventDate", typeof(DateTime));
			dt.Columns.Add("StartTime", typeof(DateTime));
			dt.Columns.Add("EndTime", typeof(DateTime));
			dt.Columns.Add("CreatedDate", typeof(DateTime));
			dt.Columns.Add("UserID", typeof(int));
			dt.Columns.Add("ItemGuid", typeof(Guid));
			dt.Columns.Add("ModuleGuid", typeof(Guid));
			dt.Columns.Add("UserGuid", typeof(Guid));
			dt.Columns.Add("Location", typeof(string));
			dt.Columns.Add("LastModUserGuid", typeof(Guid));
			dt.Columns.Add("LastModUtc", typeof(DateTime));
			dt.Columns.Add("TicketPrice", typeof(decimal));
			dt.Columns.Add("RequiresTicket", typeof(bool));
			dt.Columns.Add("ShowMap", typeof(bool));

			using (IDataReader reader = MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand,
                sqlParams.ToArray()))
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
					row["ItemID"] = reader["ItemID"];
					row["ModuleID"] = reader["ModuleID"];
					row["Title"] = reader["Title"];
					row["Description"] = reader["Description"];
					row["ImageName"] = reader["ImageName"];
					row["EventDate"] = reader["EventDate"];
					row["StartTime"] = reader["StartTime"];
					row["EndTime"] = reader["EndTime"];
					row["CreatedDate"] = reader["CreatedDate"];
					row["UserID"] = reader["UserID"];
					row["ItemGuid"] = reader["ItemGuid"];
					row["ModuleGuid"] = reader["ModuleGuid"];
					row["UserGuid"] = reader["UserGuid"];
					row["Location"] = reader["Location"];
					row["LastModUserGuid"] = reader["LastModUserGuid"];
					row["LastModUtc"] = reader["LastModUtc"];
					row["TicketPrice"] = reader["TicketPrice"];
					row["RequiresTicket"] = reader["RequiresTicket"];
					row["ShowMap"] = reader["ShowMap"];
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
            sqlCommand.Append("p.SiteID = ?SiteID ");
            sqlCommand.Append("AND pm.PageID = ?PageID ");
            sqlCommand.Append(" ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?PageID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }





    }
}
