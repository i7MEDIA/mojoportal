/// Author:					
/// Created:				2007-11-03
/// Last Modified:			2019-09-18
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using Mono.Data.Sqlite;
using System.Collections.Generic;

namespace mojoPortal.Data
{

	public static class DBEvents
	{
		public static String DBPlatform()
		{
			return "SQLite";
		}

		private static string GetConnectionString()
		{
			string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];
			if (connectionString == "defaultdblocation")
			{
				connectionString = "version=3,URI=file:"
					+ System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/mojo.db.config");

			}
			return connectionString;
		}



		/// <summary>
		/// Inserts a row in the mp_CalendarEvents table. Returns new integer id.
		/// </summary>
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
			sqlCommand.Append("INSERT INTO mp_CalendarEvents (");
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
			sqlCommand.Append("RequiresTicket,");
			sqlCommand.Append("ShowMap )");

			sqlCommand.Append(" VALUES (");
			sqlCommand.Append(":ModuleID, ");
			sqlCommand.Append(":Title, ");
			sqlCommand.Append(":Description, ");
			sqlCommand.Append(":ImageName, ");
			sqlCommand.Append(":EventDate, ");
			sqlCommand.Append(":StartTime, ");
			sqlCommand.Append(":EndTime, ");
			sqlCommand.Append(":CreatedDate, ");
			sqlCommand.Append(":UserID, ");
			sqlCommand.Append(":ItemGuid, ");
			sqlCommand.Append(":ModuleGuid, ");
			sqlCommand.Append(":UserGuid, ");
			sqlCommand.Append(":Location, ");
			sqlCommand.Append(":UserGuid, ");
			sqlCommand.Append(":CreatedDate, ");
			sqlCommand.Append(":TicketPrice, ");
			sqlCommand.Append(":RequiresTicket,");
			sqlCommand.Append(":ShowMap)");
			sqlCommand.Append(";");

			sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

			SqliteParameter[] arParams = new SqliteParameter[16];

			arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			arParams[1] = new SqliteParameter(":Title", DbType.String, 255);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = title;

			arParams[2] = new SqliteParameter(":Description", DbType.Object);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = description;

			arParams[3] = new SqliteParameter(":ImageName", DbType.String, 100);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = imageName;

			arParams[4] = new SqliteParameter(":EventDate", DbType.DateTime);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = eventDate;

			arParams[5] = new SqliteParameter(":StartTime", DbType.DateTime);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = startTime;

			arParams[6] = new SqliteParameter(":EndTime", DbType.DateTime);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = endTime;

			arParams[7] = new SqliteParameter(":CreatedDate", DbType.DateTime);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = createdDate;

			arParams[8] = new SqliteParameter(":UserID", DbType.Int32);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = userId;

			arParams[9] = new SqliteParameter(":ItemGuid", DbType.String, 36);
			arParams[9].Direction = ParameterDirection.Input;
			arParams[9].Value = itemGuid.ToString();

			arParams[10] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = moduleGuid.ToString();

			arParams[11] = new SqliteParameter(":UserGuid", DbType.String, 36);
			arParams[11].Direction = ParameterDirection.Input;
			arParams[11].Value = userGuid.ToString();

			arParams[12] = new SqliteParameter(":Location", DbType.Object);
			arParams[12].Direction = ParameterDirection.Input;
			arParams[12].Value = location;

			arParams[13] = new SqliteParameter(":TicketPrice", DbType.Decimal);
			arParams[13].Direction = ParameterDirection.Input;
			arParams[13].Value = ticketPrice;

			arParams[14] = new SqliteParameter(":RequiresTicket", DbType.Int32);
			arParams[14].Direction = ParameterDirection.Input;
			arParams[14].Value = intRequiresTicket;

			arParams[15] = new SqliteParameter(":ShowMap", DbType.Int32);
			arParams[15].Direction = ParameterDirection.Input;
			arParams[15].Value = intShowMap;

			int newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
				GetConnectionString(),
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
			sqlCommand.Append("ModuleID = :ModuleID, ");
			sqlCommand.Append("Title = :Title, ");
			sqlCommand.Append("Description = :Description, ");
			sqlCommand.Append("ImageName = :ImageName, ");
			sqlCommand.Append("EventDate = :EventDate, ");
			sqlCommand.Append("StartTime = :StartTime, ");
			sqlCommand.Append("EndTime = :EndTime, ");
			sqlCommand.Append("Location = :Location, ");
			sqlCommand.Append("LastModUserGuid = :LastModUserGuid, ");
			sqlCommand.Append("LastModUtc = :LastModUtc, ");
			sqlCommand.Append("TicketPrice = :TicketPrice, ");
			sqlCommand.Append("RequiresTicket = :RequiresTicket,");
			sqlCommand.Append("ShowMap = :ShowMap ");

			sqlCommand.Append("WHERE  ");
			sqlCommand.Append("ItemID = :ItemID ;");

			SqliteParameter[] arParams = new SqliteParameter[14];

			arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = itemId;

			arParams[1] = new SqliteParameter(":ModuleID", DbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleId;

			arParams[2] = new SqliteParameter(":Title", DbType.String, 255);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = title;

			arParams[3] = new SqliteParameter(":Description", DbType.Object);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = description;

			arParams[4] = new SqliteParameter(":ImageName", DbType.String, 100);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = imageName;

			arParams[5] = new SqliteParameter(":EventDate", DbType.DateTime);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = eventDate;

			arParams[6] = new SqliteParameter(":StartTime", DbType.DateTime);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = startTime;

			arParams[7] = new SqliteParameter(":EndTime", DbType.DateTime);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = endTime;

			arParams[8] = new SqliteParameter(":Location", DbType.Object);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = location;

			arParams[9] = new SqliteParameter(":LastModUserGuid", DbType.String, 36);
			arParams[9].Direction = ParameterDirection.Input;
			arParams[9].Value = lastModUserGuid.ToString();

			arParams[10] = new SqliteParameter(":LastModUtc", DbType.DateTime);
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = lastModUtc;

			arParams[11] = new SqliteParameter(":TicketPrice", DbType.Decimal);
			arParams[11].Direction = ParameterDirection.Input;
			arParams[11].Value = ticketPrice;

			arParams[12] = new SqliteParameter(":RequiresTicket", DbType.Int32);
			arParams[12].Direction = ParameterDirection.Input;
			arParams[12].Value = intRequiresTicket;

			arParams[13] = new SqliteParameter(":ShowMap", DbType.Int32);
			arParams[13].Direction = ParameterDirection.Input;
			arParams[13].Value = intShowMap;

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
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
			sqlCommand.Append("ItemID = :ItemID ;");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = itemId;

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
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
			sqlCommand.Append("ModuleID  = :ModuleID ;");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
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
			sqlCommand.Append("ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID) ;");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
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
			sqlCommand.Append("ItemID = :ItemID ;");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = itemId;

			return SqliteHelper.ExecuteReader(
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
			SqliteParameter[] arParams = new SqliteParameter[3];
			arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
			arParams[1] = new SqliteParameter(":BeginDate", DbType.DateTime);
			arParams[2] = new SqliteParameter(":EndDate", DbType.DateTime);

			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;


			sqlCommand.Append("SELECT  * ");
			sqlCommand.Append("FROM	mp_CalendarEvents ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("ModuleID = :ModuleID ");

			sqlCommand.Append("AND EventDate >= :BeginDate ");
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = beginDate;


			sqlCommand.Append("AND EventDate <= :EndDate ");
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = endDate;

			sqlCommand.Append(" ;");

			return SqliteHelper.ExecuteDataset(
				GetConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		public static DataTable GetEventsTable(
			int moduleId,
			DateTime beginDate,
			DateTime endDate)
		{


			var sqlCommand = @"SELECT  * 
FROM    mp_CalendarEvents 
WHERE   ModuleID = :ModuleID 
AND     EventDate >= :BeginDate
AND     EventDate <= :EndDate
ORDER BY EventDate;";

			List<SqliteParameter> sqlParams = new() {
				new SqliteParameter(":ModuleID", DbType.Int32) {Direction = ParameterDirection.Input,Value = moduleId },
				new SqliteParameter(":BeginDate", DbType.DateTime){Direction = ParameterDirection.Input,Value = beginDate },
				new SqliteParameter(":EndDate", DbType.DateTime){Direction = ParameterDirection.Input,Value = endDate }
			};

			DataTable dt = new();
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

			using (IDataReader reader = SqliteHelper.ExecuteReader(
				GetConnectionString(),
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

			sqlCommand.Append("m.ModuleTitle As ModuleTitle, ");
			sqlCommand.Append("m.ViewRoles As ViewRoles, ");
			sqlCommand.Append("md.FeatureName As FeatureName ");

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
			sqlCommand.Append("p.SiteID = :SiteID ");
			sqlCommand.Append("AND pm.PageID = :PageID ");
			sqlCommand.Append(" ; ");

			SqliteParameter[] arParams = new SqliteParameter[2];

			arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			arParams[1] = new SqliteParameter(":PageID", DbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = pageId;

			return SqliteHelper.ExecuteReader(
				GetConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}





	}
}
