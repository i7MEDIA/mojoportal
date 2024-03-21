using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;

public static class DBTaskQueue
{

	/// <summary>
	/// Inserts a row in the mp_TaskQueue table. Returns rows affected count.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="queuedBy"> queuedBy </param>
	/// <param name="taskName"> taskName </param>
	/// <param name="notifyOnCompletion"> notifyOnCompletion </param>
	/// <param name="notificationToEmail"> notificationToEmail </param>
	/// <param name="notificationFromEmail"> notificationFromEmail </param>
	/// <param name="notificationSubject"> notificationSubject </param>
	/// <param name="taskCompleteMessage"> taskCompleteMessage </param>
	/// <param name="canStop"> canStop </param>
	/// <param name="canResume"> canResume </param>
	/// <param name="updateFrequency"> updateFrequency </param>
	/// <param name="queuedUTC"> queuedUTC </param>
	/// <param name="completeRatio"> completeRatio </param>
	/// <param name="status"> status </param>
	/// <param name="serializedTaskObject"> serializedTaskObject </param>
	/// <param name="serializedTaskType"> serializedTaskType </param>
	/// <returns>int</returns>
	public static int Create(
		Guid guid,
		Guid siteGuid,
		Guid queuedBy,
		string taskName,
		bool notifyOnCompletion,
		string notificationToEmail,
		string notificationFromEmail,
		string notificationSubject,
		string taskCompleteMessage,
		bool canStop,
		bool canResume,
		int updateFrequency,
		DateTime queuedUTC,
		double completeRatio,
		string status,
		string serializedTaskObject,
		string serializedTaskType)
	{

		#region Bit Conversion

		int intNotifyOnCompletion;
		if (notifyOnCompletion)
		{
			intNotifyOnCompletion = 1;
		}
		else
		{
			intNotifyOnCompletion = 0;
		}

		int intCanStop;
		if (canStop)
		{
			intCanStop = 1;
		}
		else
		{
			intCanStop = 0;
		}

		int intCanResume;
		if (canResume)
		{
			intCanResume = 1;
		}
		else
		{
			intCanResume = 0;
		}


		#endregion

		string sqlCommand = @"
INSERT INTO mp_TaskQueue (
    Guid, 
    SiteGuid, 
    QueuedBy, 
    TaskName, 
    NotifyOnCompletion, 
    NotificationToEmail, 
    NotificationFromEmail, 
    NotificationSubject, 
    TaskCompleteMessage, 
    CanStop, 
    CanResume, 
    UpdateFrequency, 
    QueuedUTC, 
    CompleteRatio, 
    Status, 
    SerializedTaskObject, 
    SerializedTaskType 
)
VALUES (
    ?Guid, 
    ?SiteGuid, 
    ?QueuedBy, 
    ?TaskName, 
    ?NotifyOnCompletion, 
    ?NotificationToEmail, 
    ?NotificationFromEmail, 
    ?NotificationSubject, 
    ?TaskCompleteMessage, 
    ?CanStop, 
    ?CanResume, 
    ?UpdateFrequency, 
    ?QueuedUTC, 
    ?CompleteRatio, 
    ?Status, 
    ?SerializedTaskObject, 
    ?SerializedTaskType 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?QueuedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = queuedBy.ToString()
			},

			new("?TaskName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = taskName
			},

			new("?NotifyOnCompletion", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intNotifyOnCompletion
			},

			new("?NotificationToEmail", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = notificationToEmail
			},

			new("?NotificationFromEmail", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = notificationFromEmail
			},

			new("?NotificationSubject", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = notificationSubject
			},

			new("?TaskCompleteMessage", MySqlDbType.Blob)
			{
				Direction = ParameterDirection.Input,
				Value = taskCompleteMessage
			},

			new("?CanStop", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intCanStop
			},

			new("?CanResume", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intCanResume
			},

			new("?UpdateFrequency", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = updateFrequency
			},

			new("?QueuedUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = queuedUTC
			},

			new("?CompleteRatio", MySqlDbType.Float)
			{
				Direction = ParameterDirection.Input,
				Value = completeRatio
			},

			new("?Status", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = status
			},

			new("?SerializedTaskObject", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = serializedTaskObject
			},

			new("?SerializedTaskType", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = serializedTaskType
			}
		};



		//using (var command = new MySqlCommand())
		//{
		//    command.Connection = new(ConnectionString.GetWriteConnectionString());
		//    command.CommandText = sqlCommand.ToString()
		//    command.Parameters.AddRange(arParams);
		//    int rowsAffected = command.ExecuteNonQuery();
		//    return rowsAffected;
		//};
		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;
	}

	/// <summary>
	/// Updates a row in the mp_TaskQueue table. Returns true if row updated.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="startUTC"> startUTC </param>
	/// <param name="completeUTC"> completeUTC </param>
	/// <param name="lastStatusUpdateUTC"> lastStatusUpdateUTC </param>
	/// <param name="completeRatio"> completeRatio </param>
	/// <param name="status"> status </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid guid,
		DateTime startUTC,
		DateTime completeUTC,
		DateTime lastStatusUpdateUTC,
		double completeRatio,
		string status)
	{

		string sqlCommand = @"
UPDATE mp_TaskQueue 
SET  
    LastStatusUpdateUTC = ?LastStatusUpdateUTC, 
    CompleteRatio = ?CompleteRatio, 
    Status = ?Status 
WHERE Guid = ?Guid ;";

		if (startUTC > DateTime.MinValue)
			sqlCommand += "StartUTC = ?StartUTC, ";

		if (completeUTC > DateTime.MinValue)
			sqlCommand += "CompleteUTC = ?CompleteUTC, ";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?StartUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = startUTC
			},

			new("?CompleteUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = completeUTC
			},

			new("?LastStatusUpdateUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastStatusUpdateUTC
			},

			new("?CompleteRatio", MySqlDbType.Float)
			{
				Direction = ParameterDirection.Input,
				Value = completeRatio
			},

			new("?Status", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = status
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Updates a row in the mp_TaskQueue table. Returns true if row updated.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="notificationSentUTC"> notificationSentUTC </param>
	/// <returns>bool</returns>
	public static bool UpdateNotification(
		Guid guid,
		DateTime notificationSentUTC)
	{

		string sqlCommand = @"
UPDATE mp_TaskQueue 
SET NotificationSentUTC = ?NotificationSentUTC 
WHERE Guid = ?Guid;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?NotificationSentUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = notificationSentUTC
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_TaskQueue table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid guid)
	{
		string sqlCommand = @"
DELETE FROM mp_TaskQueue 
WHERE Guid = ?Guid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	/// <summary>
	/// Deletes all completed tasks from mp_TaskQueue table
	/// </summary>
	public static void DeleteCompleted()
	{
		var sqlCommand = "DELETE FROM mp_TaskQueue WHERE CompleteUTC IS NOT NULL;";

		CommandHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(), sqlCommand);
	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_TaskQueue table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetOne(Guid guid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_TaskQueue 
WHERE Guid = ?Guid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets a count of rows in the mp_TaskQueue table.
	/// </summary>
	public static int GetCount()
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_TaskQueue ;";

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString()));
	}

	/// <summary>
	/// Gets a count of rows in the mp_TaskQueue table.
	/// </summary>
	/// <param name="siteGuid"> guid </param>
	public static int GetCount(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_TaskQueue 
WHERE SiteGuid = ?SiteGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));
	}

	public static int GetCountUnfinishedByType(string taskType)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_TaskQueue 
WHERE SerializedTaskType LIKE ?TaskType ;";

		var arParams = new List<MySqlParameter>
		{
			new("?TaskType", MySqlDbType.VarChar, 266)
			{
				Direction = ParameterDirection.Input,
				Value = taskType + "%"
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));
	}

	public static bool DeleteByType(string taskType)
	{
		string sqlCommand = @"
DELETE FROM mp_TaskQueue 
WHERE SerializedTaskType LIKE ?TaskType ;";

		var arParams = new List<MySqlParameter>
		{
			new("?TaskType", MySqlDbType.VarChar, 266)
			{
				Direction = ParameterDirection.Input,
				Value = taskType + "%"
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;
	}

	/// <summary>
	/// Gets a count of rows in the mp_TaskQueue table.
	/// </summary>
	public static int GetCountUnfinished()
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_TaskQueue 
WHERE CompleteUTC IS NULL ;";

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString()));
	}

	/// <summary>
	/// Gets a count of rows in the mp_TaskQueue table.
	/// </summary>
	/// <param name="siteGuid"> guid </param>
	public static int GetCountUnfinished(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_TaskQueue 
WHERE SiteGuid = ?SiteGuid 
AND CompleteUTC IS NULL ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));
	}

	/// <summary>
	/// Gets an IDataReader with all rows in the mp_TaskQueue table.
	/// </summary>
	public static IDataReader GetTasksNotStarted()
	{
		string sqlCommand = @"
SELECT * 
FROM mp_TaskQueue 
WHERE StartUTC IS NULL ;";

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString());
	}

	/// <summary>
	/// Gets an IDataReader with all tasks in the mp_TaskQueue table that have completed but not yet sent notification.
	/// </summary>
	public static IDataReader GetTasksForNotification()
	{
		string sqlCommand = @"
SELECT * 
FROM mp_TaskQueue 
WHERE NotifyOnCompletion = 1 
AND CompleteUTC IS NOT NULL 
AND NotificationSentUTC IS NULL ;";

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString());
	}

	/// <summary>
	/// Gets an IDataReader with all rows in the mp_TaskQueue table.
	/// </summary>
	public static IDataReader GetUnfinished()
	{
		string sqlCommand = @"
SELECT * 
FROM mp_TaskQueue 
WHERE CompleteUTC IS NULL ;";

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString());
	}


	/// <summary>
	/// Gets an IDataReader with all rows in the mp_TaskQueue table.
	/// </summary>
	/// <param name="siteGuid"> guid </param>
	public static IDataReader GetUnfinished(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_TaskQueue 
WHERE SiteGuid = ?SiteGuid 
AND CompleteUTC IS NULL ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}



	/// <summary>
	/// Gets a page of data from the mp_TaskQueue table.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static IDataReader GetPage(
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCount();

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

		string sqlCommand = @"
SELECT * 
FROM mp_TaskQueue  
ORDER BY  
QueuedUTC DESC 
LIMIT " + pageLowerBound.ToString() + ", ?PageSize ";

		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}

	/// <summary>
	/// Gets a page of data from the mp_TaskQueue table.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	/// <param name="siteGuid"> guid </param>
	public static IDataReader GetPageBySite(
		Guid siteGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCount(siteGuid);

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

		string sqlCommand = @"
SELECT * 
FROM mp_TaskQueue  
WHERE SiteGuid = ?SiteGuid 
ORDER BY QueuedUTC DESC  
LIMIT " + pageLowerBound.ToString() + ", ?PageSize ";

		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}


	/// <summary>
	/// Gets a page of data from the mp_TaskQueue table.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static IDataReader GetPageUnfinished(
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCountUnfinished();

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

		string sqlCommand = @"
SELECT * 
FROM mp_TaskQueue  
WHERE CompleteUTC IS NULL 
ORDER BY QueuedUTC DESC  
LIMIT " + pageLowerBound.ToString() + ", ?PageSize ";

		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}


	/// <summary>
	/// Gets a page of data from the mp_TaskQueue table.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	/// <param name="siteGuid"> guid </param>
	public static IDataReader GetPageUnfinishedBySite(
		Guid siteGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCountUnfinished(siteGuid);

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

		string sqlCommand = @"
SELECT * 
FROM mp_TaskQueue  
WHERE SiteGuid = ?SiteGuid 
AND CompleteUTC IS NULL 
ORDER BY QueuedUTC DESC 
LIMIT " + pageLowerBound.ToString() + ", ?PageSize ";

		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}


}
