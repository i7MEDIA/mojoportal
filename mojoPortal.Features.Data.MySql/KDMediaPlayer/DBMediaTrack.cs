using mojoPortal.Data;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.MediaPlayer.Data;

public static class DBMediaTrack
{
	/// <summary>
	/// Inserts a row in the mp_MediaTrack table.
	/// </summary>
	/// <param name="playerID">The ID of the player to which the Media Track is being added.</param>
	/// <param name="trackType">The type of the track.</param>
	/// <param name="trackOrder">The order position of the Media Track.</param>
	/// <param name="name">The name of the Media Track.</param>
	/// <param name="artist">The artist of the Media Track.</param>
	/// <param name="userGuid">The Guid of the user who added the Media Track.</param>
	/// <returns>The ID of the Media Track in the doan_MediaTracks table.</returns>
	public static int Insert(
		int playerId,
		string trackType,
		int trackOrder,
		string name,
		string artist,
		Guid userGuid)
	{
		string sqlCommand = @"
INSERT INTO mp_MediaTrack (
    PlayerID, 
    TrackType, 
    TrackOrder, 
    Name, 
    Artist, 
    CreatedDate, 
    UserGuid )
     VALUES (
    ?PlayerID, 
    ?TrackType, 
    ?TrackOrder, 
    ?Name, 
    ?Artist, 
    ?CreatedDate, 
    ?UserGuid 
);
SELECT LAST_INSERT_ID(); ";

		var arParams = new List<MySqlParameter>
		{
			new("?PlayerID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = playerId
			},

			new("?TrackType", MySqlDbType.VarChar, 10)
			{
				Direction = ParameterDirection.Input,
				Value = trackType
			},

			new("?TrackOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = trackOrder
			},

			new("?Name", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = name
			},

			new("?Artist", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = artist
			},

			new("?CreatedDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DateTime.Now
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams).ToString());
		return newID;
	}

	/// <summary>
	/// Updates a row in the mp_MediaTrack table.
	/// </summary>
	/// <param name="trackID">The ID of the track.</param>
	/// <param name="playerID">The ID of the player instance.</param>
	/// <param name="trackType">The type of the track.</param>
	/// <param name="trackOrder">The order position of the Media Track.</param>
	/// <param name="name">The name of the Media Track.</param>
	/// <param name="artist">The artist of the Media Track.</param>
	/// <param name="userGuid">The Guid of the user who added the Media Track.</param>
	/// <returns>True if the row is successfully updated.</returns>
	public static bool Update(
		int trackId,
		int playerId,
		string trackType,
		int trackOrder,
		string name,
		string artist,
		Guid userGuid)
	{
		string sqlCommand = @"
UPDATE 
    mp_MediaTrack 
SET  
    PlayerID = ?PlayerID, 
    TrackType = ?TrackType, 
    TrackOrder = ?TrackOrder, 
    Name = ?Name, 
    Artist = ?Artist, 
    UserGuid = ?UserGuid 
WHERE 
    TrackID = ?TrackID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?TrackID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = trackId
			},

			new("?PlayerID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = playerId
			},

			new("?TrackType", MySqlDbType.VarChar, 10)
			{
				Direction = ParameterDirection.Input,
				Value = trackType
			},

			new("?TrackOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = trackOrder
			},

			new("?Name", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = name
			},

			new("?Artist", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = artist
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;
	}

	/// <summary>
	/// Updates the TrackOrder values for the tracks that remain for the PlayerID by incrementing any Tracks that have a TrackOrder value
	/// greater than the provided trackOrder.
	/// </summary>
	/// <param name="playerID">The ID of the Player.</param>
	/// <param name="trackOrder">The TrackOrder value.</param>
	/// <returns>The number of rows affected by the update.</returns>
	public static int AdjustTrackOrdersForDelete(int playerId, int trackOrder)
	{
		string sqlCommand = @"
UPDATE mp_MediaTrack 
SET TrackOrder = TrackOrder - 1 
WHERE PlayerID = ?PlayerID 
AND TrackOrder > ?TrackOrder ;";

		var arParams = new List<MySqlParameter>
		{
			new("?TrackOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = trackOrder
			},

			new("?PlayerID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = playerId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;
	}

	/// <summary>
	/// Deletes a row from the doan_MediaTracks table.
	/// </summary>
	/// <param name="trackID">The ID of the track.</param>
	/// <returns>True if a row was deleted.</returns>
	public static bool Delete(int trackId)
	{
		DBMediaFile.DeleteByTrack(trackId);

		string sqlCommand = @"
DELETE FROM mp_MediaTrack 
WHERE TrackID = ?TrackID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?TrackID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = trackId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;
	}

	/// <summary>
	/// Deletes all rows from the doan_MediaTracks table for a particular player instance.
	/// </summary>
	/// <param name="playerID">The ID of the player.</param>
	/// <returns>True if rows were deleted.</returns>
	public static bool DeleteByPlayer(int playerId)
	{
		DBMediaFile.DeleteByPlayer(playerId);

		string sqlCommand = @"
DELETE FROM mp_MediaTrack 
WHERE PlayerID = ?PlayerID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?PlayerID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = playerId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;
	}

	public static bool DeleteByModule(int moduleId)
	{
		DBMediaFile.DeleteByModule(moduleId);

		string sqlCommand = @"
DELETE FROM mp_MediaTrack 
WHERE PlayerID IN (
    SELECT PlayerID 
    FROM mp_MediaPlayer 
    WHERE ModuleID = ?ModuleID
);";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;
	}

	public static bool DeleteBySite(int siteId)
	{
		DBMediaFile.DeleteBySite(siteId);

		string sqlCommand = @"
DELETE FROM mp_MediaTrack 
WHERE 
PlayerID IN (
    SELECT PlayerID 
    FROM mp_MediaPlayer 
    WHERE ModuleID IN ( 
        SELECT ModuleID 
        FROM mp_Modules 
        WHERE SiteID = ?SiteID
    )
); ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}


	/// <summary>
	/// Gets a count of rows for a particular player in the doan_MediaTracks table.
	/// </summary>
	/// <param name="playerID">The ID of the player.</param>
	/// <returns>The count of rows.</returns>
	public static int GetCountByPlayer(int playerId)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_MediaTrack 
WHERE PlayerID = ?PlayerID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?PlayerID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = playerId
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));
	}


	/// <summary>
	/// Selects a particular Track from the doan_MediaTracks table.
	/// </summary>
	/// <param name="trackID">The ID of the track.</param>
	/// <returns>An IDataReader containing the MediaTrack data.</returns>
	public static IDataReader Select(int trackId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_MediaTrack 
WHERE TrackID = ?TrackID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?TrackID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = trackId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}

	/// <summary>
	/// Selects all Tracks for a particular player instance from the doan_MediaTracks table.
	/// </summary>
	/// <param name="playerID">The ID of the player.</param>
	/// <returns>An IDataReader containing the MediaTrack(s) data.</returns>
	public static IDataReader SelectByPlayer(int playerId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_MediaTrack 
WHERE PlayerID = ?PlayerID 
ORDER BY TrackOrder ;";

		var arParams = new List<MySqlParameter>
		{
			new("?PlayerID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = playerId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}

}
