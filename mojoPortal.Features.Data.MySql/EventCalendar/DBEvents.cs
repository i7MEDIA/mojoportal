using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;

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


		string sqlCommand = @"
INSERT INTO mp_CalendarEvents (
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
	ShowMap
)
VALUES (
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
	?ShowMap
);
SELECT LAST_INSERT_ID();";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32) {
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?Title", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},

			new("?Description", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?ImageName", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = imageName
			},

			new("?EventDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = eventDate
			},

			new("?StartTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = startTime
			},

			new("?EndTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = endTime
			},

			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},

			new("?ItemGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = itemGuid.ToString()
			},

			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?Location", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = location
			},

			new("?RequiresTicket", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intRequiresTicket
			},

			new("?TicketPrice", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = ticketPrice
			},

			new("?CreatedDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdDate
			},

			new("?ShowMap", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intShowMap
			}
		};

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
		ConnectionString.GetWrite(),
		sqlCommand,
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

		string sqlCommand = @"
UPDATE 
    mp_CalendarEvents 
SET  
    ModuleID = ?ModuleID, 
    Title = ?Title, 
    Description = ?Description, 
    ImageName = ?ImageName, 
    EventDate = ?EventDate, 
    StartTime = ?StartTime, 
    EndTime = ?EndTime, 
    Location = ?Location, 
    LastModUserGuid = ?LastModUserGuid, 
    LastModUtc = ?LastModUtc, 
    TicketPrice = ?TicketPrice, 
    RequiresTicket = ?RequiresTicket,
    ShowMap = ?ShowMap 
WHERE  
    ItemID = ?ItemID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			},

			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?Title", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},

			new("?Description", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?ImageName", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = imageName
			},

			new("?EventDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = eventDate
			},

			new ("?StartTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = startTime
			},

			new ("?EndTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = endTime
			},

			new("?LastModUserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = lastModUserGuid.ToString()
			},

			new("?LastModUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastModUtc
			},

			new("?TicketPrice", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = ticketPrice
			},

			new("?RequiresTicket", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intRequiresTicket
			},

			new("?Location", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = location
			},

			new("?ShowMap", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intShowMap
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
		ConnectionString.GetWrite(),
		sqlCommand,
		arParams);

		return (rowsAffected > -1);

	}


	public static bool DeleteCalendarEvent(int itemId)
	{
		string sqlCommand = @"
DELETE FROM mp_CalendarEvents 
WHERE ItemID = ?ItemID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		return rowsAffected > 0;

	}

	public static bool DeleteByModule(int moduleId)
	{
		string sqlCommand = @"
DELETE FROM mp_CalendarEvents 
WHERE ModuleID  = ?ModuleID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		return rowsAffected > 0;

	}

	public static bool DeleteBySite(int siteId)
	{
		string sqlCommand = @"
DELETE FROM mp_CalendarEvents 
WHERE 
ModuleID IN (
    SELECT ModuleID 
    FROM mp_Modules 
    WHERE SiteID = ?SiteID) ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		return rowsAffected > 0;

	}


	public static IDataReader GetCalendarEvent(int itemId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_CalendarEvents 
WHERE ItemID = ?ItemID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);

	}

	public static DataSet GetEvents(
		int moduleId,
		DateTime beginDate,
		DateTime endDate)
	{

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?BeginDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = beginDate
			},

			new("?EndDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = endDate
			}
		};

		string sqlCommand = @"
SELECT * 
FROM mp_CalendarEvents 
WHERE 
	ModuleID = ?ModuleID 
	AND EventDate >= ?BeginDate 
	AND EventDate <= ?EndDate 
ORDER BY EventDate ;";

		return CommandHelper.ExecuteDataset(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);

	}

	public static DataTable GetEventsTable(
		int moduleId,
		DateTime beginDate,
		DateTime endDate)
	{

		var sqlParams = new List<MySqlParameter>()
		{
				new("?ModuleID", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				},
				new("?BeginDate", MySqlDbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = beginDate
				},
				new("?EndDate", MySqlDbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = endDate
				}
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

		using (IDataReader reader = CommandHelper.ExecuteReader(
		ConnectionString.GetRead(),
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
		string sqlCommand = @"
SELECT 
	ce.*,
	m.ModuleTitle,
	m.ViewRoles,
	md.FeatureName 
FROM mp_CalendarEvents ce 
JOIN mp_Modules m 
ON ce.ModuleID = m.ModuleID 
JOIN mp_ModuleDefinitions md 
ON m.ModuleDefID = md.ModuleDefID 
JOIN mp_PageModules pm 
ON m.ModuleID = pm.ModuleID 
JOIN mp_Pages p 
ON p.PageID = pm.PageID 
WHERE p.SiteID = ?SiteID 
AND pm.PageID = ?PageID; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?PageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);

	}





}
