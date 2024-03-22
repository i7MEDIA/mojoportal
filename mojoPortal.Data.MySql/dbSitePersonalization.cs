using log4net;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;

public static class DBSitePersonalization
{
	private static readonly ILog log = LogManager.GetLogger(typeof(DBSitePersonalization));

	public static int GetCountOfState(
		int siteId,
		String path,
		bool allUserScope,
		Guid userGuid,
		DateTime inactiveSince)
	{
		int result = 0;

		Guid pathID = Guid.Empty;
		if ((path != null) && (path.Length > 0))
		{
			pathID = GetOrCreatePathId(siteId, path);
		}


		if (allUserScope)
		{
			if (pathID != Guid.Empty)
			{
				result = GetCountOfStateAllUsers(pathID);
			}
			else
			{
				result = GetCountOfStateAllUsers();
			}

		}
		else
		{

			if (userGuid != Guid.Empty)
			{
				if (pathID != Guid.Empty)
				{
					if (inactiveSince > DateTime.MinValue)
					{
						result = GetCountOfStateByUser(userGuid, pathID, inactiveSince);

					}
					else
					{
						result = GetCountOfStateByUser(userGuid, pathID);
					}
				}
				else
				{
					if (inactiveSince > DateTime.MinValue)
					{
						result = GetCountOfStateByUserAllPaths(userGuid, inactiveSince);

					}
					else
					{
						result = GetCountOfStateByUserAllPaths(userGuid);
					}

				}

			}
			else
			{
				// not a specific user
				if (pathID != Guid.Empty)
				{
					if (inactiveSince > DateTime.MinValue)
					{
						result = GetCountOfStateByUser(pathID, inactiveSince);

					}
					else
					{
						result = GetCountOfStateByUser(inactiveSince);
					}
				}
				else
				{
					// not a specific path
					if (inactiveSince > DateTime.MinValue)
					{
						result = GetCountOfStateByUser(inactiveSince);

					}
					else
					{
						result = GetCountOfStateByUser();
					}

				}

			}

		}

		return result;
	}

	public static int GetCountOfStateByUser(
		Guid userGuid,
		Guid pathId)
	{

		string sqlCommand = @"
SELECT Count(*) 
FROM mp_SitePersonalizationPerUser 
WHERE PathID = ?PathID AND UserID = ?UserID ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?PathID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pathId.ToString()
			},

			new("?UserID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

		return count;


	}

	public static int GetCountOfStateByUser(
		Guid userGuid,
		Guid pathId,
		DateTime inactiveSinceTime)
	{

		string sqlCommand = @"
SELECT Count(pu.*) 
FROM mp_SitePersonalizationPerUser pu 
JOIN mp_Users u 
ON pu.UserID = u.UserGuid 
WHERE pu.PathID = ?PathID AND pu.UserID = ?UserID  
AND u.LastActivityDate <= ?LastActivityDate ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?PathID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pathId.ToString()
			},

			new("?UserID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?LastActivityDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = inactiveSinceTime
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

		return count;


	}

	public static int GetCountOfStateByUser(
		Guid pathId,
		DateTime inactiveSinceTime)
	{

		string sqlCommand = @"
SELECT Count(pu.*) 
FROM mp_SitePersonalizationPerUser pu 
JOIN mp_Users u 
ON pu.UserID = u.UserGuid 
WHERE pu.PathID = ?PathID   
AND u.LastActivityDate <= ?LastActivityDate ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?PathID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pathId.ToString()
			},

			new("?LastActivityDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = inactiveSinceTime
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

		return count;


	}

	public static int GetCountOfStateByUserAllPaths(
		Guid userGuid,
		DateTime inactiveSinceTime)
	{

		string sqlCommand = @"
SELECT Count(pu.*) 
FROM mp_SitePersonalizationPerUser pu 
JOIN mp_Users u 
ON pu.UserID = u.UserGuid 
WHERE pu.UserID = ?UserID   
AND u.LastActivityDate <= ?LastActivityDate ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?UserID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?LastActivityDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = inactiveSinceTime
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

		return count;


	}

	public static int GetCountOfStateByUserAllPaths(Guid userGuid)
	{

		string sqlCommand = @"
SELECT Count(pu.*) 
FROM mp_SitePersonalizationPerUser pu 
JOIN mp_Users u 
ON pu.UserID = u.UserGuid 
WHERE pu.UserID = ?UserID ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?UserID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

		return count;

	}

	public static int GetCountOfStateByUser(DateTime inactiveSinceTime)
	{

		string sqlCommand = @"
SELECT Count(pu.*) 
FROM mp_SitePersonalizationPerUser pu 
JOIN mp_Users u 
ON pu.UserID = u.UserGuid  
WHERE u.LastActivityDate <= ?LastActivityDate ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?LastActivityDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = inactiveSinceTime
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

		return count;


	}

	public static int GetCountOfStateByUser()
	{

		string sqlCommand = @"
SELECT Count(pu.*) 
FROM mp_SitePersonalizationPerUser pu ;";

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString()));

		return count;


	}

	public static int GetCountOfStateAllUsers(Guid pathId)
	{

		string sqlCommand = @"
SELECT Count(*) 
FROM mp_SitePersonalizationAllUsers 
WHERE PathID = ?PathID  ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?PathID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pathId.ToString()
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

		return count;

	}

	public static int GetCountOfStateAllUsers()
	{

		string sqlCommand = @"
SELECT Count(*) 
FROM mp_SitePersonalizationAllUsers ; ";

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString()));

		return count;

	}

	public static void SavePersonalizationBlob(
		int siteId,
		String path,
		Guid userGuid,
		byte[] dataBlob,
		DateTime lastUpdateTime)
	{
		Guid pathID = GetOrCreatePathId(siteId, path);
		if (PersonalizationBlobExists(pathID, userGuid))
		{
			UpdatePersonalizationBlob(userGuid, pathID, dataBlob, lastUpdateTime);
		}
		else
		{
			CreatePersonalizationBlob(userGuid, pathID, dataBlob, lastUpdateTime);
		}


	}

	public static byte[] GetPersonalizationBlob(
		int siteId,
		String path,
		Guid userGuid)
	{
		Guid pathID = GetOrCreatePathId(siteId, path);

		string sqlCommand = @"
SELECT PageSettings 
FROM	mp_SitePersonalizationPerUser 
WHERE PathID = ?PathID AND UserID = ?UserID LIMIT 1 ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?PathID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pathID.ToString()
			},

			new("?UserID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};

		byte[] result = null;

		try
		{
			result = (byte[])CommandHelper.ExecuteScalar(
				ConnectionString.GetRead(),
				sqlCommand.ToString(),
				arParams);
		}
		catch (System.InvalidCastException ex)
		{
			if (log.IsErrorEnabled)
			{
				log.Error("dbPortal.SitePersonalization_GetPersonalizationBlob", ex);
			}
		}

		return result;

	}

	public static void ResetPersonalizationBlob(
		int siteId,
		String path,
		Guid userGuid)
	{
		Guid pathID = GetOrCreatePathId(siteId, path);

		string sqlCommand = @"
DELETE 
FROM mp_SitePersonalizationPerUser 
WHERE PathID = ?PathID AND UserID = ?UserID  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?PathID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pathID.ToString()
			},

			new("?UserID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};

		CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWrite(),
				sqlCommand.ToString(),
				arParams);

	}

	public static void ResetPersonalizationBlob(
		int siteId,
		String path)
	{
		string sqlCommand = @"
DELETE 
FROM mp_SitePersonalizationAllUsers 
WHERE PathID IN (   
    SELECT PathID    
    FROM mp_SitePaths    
    WHERE SiteID = ?SiteID  
); ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWrite(),
				sqlCommand.ToString(),
				arParams);

	}

	public static byte[] GetPersonalizationBlobAllUsers(
		int siteId,
		String path)
	{
		Guid pathID = GetOrCreatePathId(siteId, path);

		string sqlCommand = @"
SELECT PageSettings 
FROM mp_SitePersonalizationAllUsers 
WHERE PathID = ?PathID  LIMIT 1 ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?PathID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pathID.ToString()
			}
		};

		byte[] result = null;

		try
		{
			result = (byte[])CommandHelper.ExecuteScalar(
				ConnectionString.GetRead(),
				sqlCommand.ToString(),
				arParams);
		}
		catch (System.InvalidCastException ex)
		{
			if (log.IsErrorEnabled)
			{
				log.Error("dbPortal.SitePersonalization_GetPersonalizationBlobAllUsers", ex);
			}
		}

		return result;

	}

	public static void SavePersonalizationBlobAllUsers(
		int siteId,
		String path,
		byte[] dataBlob,
		DateTime lastUpdateTime)
	{
		Guid pathID = GetOrCreatePathId(siteId, path);

		if (PersonalizationBlobExists(pathID))
		{
			UpdatePersonalizationBlob(pathID, dataBlob, lastUpdateTime);
		}
		else
		{
			CreatePersonalizationBlob(pathID, dataBlob, lastUpdateTime);
		}

	}

	public static void CreatePersonalizationBlob(
		Guid userGuid,
		Guid pathId,
		byte[] dataBlob,
		DateTime lastUpdateTime)
	{

		string sqlCommand = @"
INSERT INTO 
	mp_SitePersonalizationPerUser(
		ID, 
		UserID, 
		PathID, 
		PageSettings, 
		LastUpdate
)
VALUES (
	?ID , 
	?UserID , 
	?PathID , 
	?PageSettings , 
	?LastUpdate  
);";


		var arParams = new List<MySqlParameter>
		{
			new("?ID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = Guid.NewGuid().ToString()
			},

			new("?UserID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?PathID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pathId.ToString()
			},

			new("?PageSettings", MySqlDbType.LongBlob)
			{
				Direction = ParameterDirection.Input,
				Value = dataBlob
			},

			new ("?LastUpdate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastUpdateTime
			}
		};

		int rowsAffected = Convert.ToInt32(CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams).ToString());

	}

	public static void CreatePersonalizationBlob(
		Guid pathId,
		byte[] dataBlob,
		DateTime lastUpdateTime)
	{

		string sqlCommand = @"
INSERT INTO 
	mp_SitePersonalizationAllUsers(
		PathID, 
		PageSettings, 
		LastUpdate
)
VALUES (
	?PathID, 
	?PageSettings, 
	?LastUpdate  
);";

		var arParams = new List<MySqlParameter>
		{
			new("?PathID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pathId.ToString()
			},

			new("?PageSettings", MySqlDbType.LongBlob)
			{
				Direction = ParameterDirection.Input,
				Value = dataBlob
			},

			new("?LastUpdate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastUpdateTime
			}
		};

		int rowsAffected = Convert.ToInt32(CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams).ToString());


	}

	public static void UpdatePersonalizationBlob(
		Guid userGuid,
		Guid pathId,
		byte[] dataBlob,
		DateTime lastUpdateTime)
	{

		string sqlCommand = @"
UPDATE mp_SitePersonalizationPerUser 
SET PageSettings = ?PageSettings, 
LastUpdate = ?LastUpdate 
WHERE UserID = ?UserID AND PathID = ?PathID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?PathID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pathId.ToString()
			},


			new("?PageSettings", MySqlDbType.LongBlob)
			{
				Direction = ParameterDirection.Input,
				Value = dataBlob
			},

			new("?LastUpdate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastUpdateTime
			}
		};

		int rowsAffected = 0;

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);


	}

	public static void UpdatePersonalizationBlob(
		Guid pathId,
		byte[] dataBlob,
		DateTime lastUpdateTime)
	{

		string sqlCommand = @"
UPDATE mp_SitePersonalizationAllUsers 
SET 
PageSettings = ?PageSettings, 
LastUpdate = ?LastUpdate 
WHERE PathID = ?PathID  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?PathID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pathId.ToString()
			},

			new("?PageSettings", MySqlDbType.LongBlob)
			{
				Direction = ParameterDirection.Input,
				Value = dataBlob
			},

			new("?LastUpdate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastUpdateTime
			}
		};

		int rowsAffected = 0;

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

	}

	public static bool PersonalizationBlobExists(Guid pathId, Guid userGuid)
	{
		bool result = false;

		string sqlCommand = @"
SELECT Count(*) 
FROM mp_SitePersonalizationPerUser 
WHERE PathID = ?PathID AND UserID = ?UserID ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?PathID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pathId.ToString()
			},

			new("?UserID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

		result = count > 0;


		return result;
	}

	public static bool PersonalizationBlobExists(Guid pathId)
	{
		bool result = false;

		string sqlCommand = @"
SELECT Count(*) 
FROM mp_SitePersonalizationAllUsers 
WHERE PathID = ?PathID  ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?PathID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pathId.ToString()
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

		result = count > 0;


		return result;
	}

	public static Guid GetOrCreatePathId(int siteId, String path)
	{
		Guid result = Guid.Empty;

		if (PathExists(siteId, path))
		{
			result = GetPathId(siteId, path);
		}
		else
		{
			result = CreatePath(siteId, path);
		}

		return result;
	}

	public static Guid GetPathId(int siteId, String path)
	{
		Guid result = Guid.Empty;

		string sqlCommand = @"
SELECT PathID 
FROM mp_SitePaths 
WHERE SiteID = ?SiteID AND LoweredPath = ?LoweredPath LIMIT 1 ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?LoweredPath", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = path.ToLower()
			}
		};

		string guidString = CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams).ToString();


		result = new Guid(guidString);


		return result;
	}

	public static bool PathExists(int siteId, String path)
	{
		bool result = false;

		string sqlCommand = @"
SELECT Count(*) 
FROM mp_SitePaths 
WHERE SiteID = ?SiteID AND LoweredPath = ?LoweredPath ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?LoweredPath", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = path.ToLower()
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

		result = count > 0;


		return result;
	}

	public static Guid CreatePath(int siteId, String path)
	{
		Guid newPathID = Guid.NewGuid();
		Guid siteGuid = DBSiteSettings.GetSiteGuidFromID(siteId);

		string sqlCommand = @"
INSERT INTO 
	mp_SitePaths(
		PathID, 
		SiteID, 
		SiteGuid, 
		Path, 
		LoweredPath
)	
VALUES (	
	?PathID ,	
	?SiteID ,	
	?SiteGuid ,	
	?Path ,	
	?LoweredPath
);";


		var arParams = new List<MySqlParameter>
		{
			new("?PathID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = newPathID.ToString()
			},

			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?Path", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = path
			},

			new("?LoweredPath", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = path.ToLower()
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}
		};

		int rowsAffected = Convert.ToInt32(CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams).ToString());

		return newPathID;

	}




}
