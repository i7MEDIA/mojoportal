using mojoPortal.Data;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.MediaPlayer.Data;

public static class DBMediaFile
{
	/// <summary>
	/// Inserts a row in the doan_MediaFiles table.
	/// </summary>
	/// <param name="trackID">The ID of the Track for which the File is being added.</param>
	/// <param name="filePath">The name of the physical Media File.</param>
	/// <param name="userGuid">The Guid of the user who is adding the File.</param>
	/// <returns>The ID of the Media File in the doan_MediaFiles table.</returns>
	public static int Insert(
		int trackId,
		string filePath,
		Guid userGuid)
	{
		string sqlCommand = @"
INSERT INTO mp_MediaFile (
    TrackID, 
    FilePath, 
    AddedDate, 
    UserGuid )
     VALUES (
    ?TrackID, 
    ?FilePath, 
    ?AddedDate, 
    ?UserGuid 
);
SELECT LAST_INSERT_ID();";

		var arParams = new List<MySqlParameter>
		{
			new("?TrackID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = trackId
			},

			new("?FilePath", MySqlDbType.VarChar, 255) {
			Direction = ParameterDirection.Input,
			Value = filePath
			},

			new("?AddedDate", MySqlDbType.DateTime) {
			Direction = ParameterDirection.Input,
			Value = DateTime.UtcNow
			},

			new("?UserGuid", MySqlDbType.VarChar, 36) {
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
	/// Deletes a row from the doan_MediaFiles table.
	/// </summary>
	/// <param name="fileID">The ID of the Media File.</param>
	/// <returns>True if row deleted.</returns>
	public static bool Delete(int fileId)
	{
		string sqlCommand = @"
DELETE FROM mp_MediaFile 
WHERE FileID = ?FileID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?FileID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = fileId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;
	}

	public static bool DeleteByTrack(int trackId)
	{
		string sqlCommand = @"
DELETE FROM mp_MediaFile 
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

	public static bool DeleteByPlayer(int playerId)
	{
		string sqlCommand = @"
DELETE FROM mp_MediaFile 
WHERE FileID  
IN (
    SELECT FileID 
    FROM mp_MediaFile 
    WHERE TrackID IN (
        SELECT TrackID 
        FROM mp_MediaTrack 
        WHERE PlayerID = ?PlayerID
    )
);";

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
		string sqlCommand = @"
DELETE FROM mp_MediaFile 
WHERE FileID 
IN (
    SELECT FileID 
    FROM mp_MediaFile 
    WHERE TrackID IN (
        SELECT TrackID 
        FROM mp_MediaTrack 
        WHERE PlayerID 
        IN (
            SELECT PlayerID 
            FROM mp_MediaPlayer 
            WHERE ModuleID = ?ModuleID
        )
    )
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
		string sqlCommand = @"
DELETE FROM mp_MediaFile 
WHERE FileID 
IN (
    SELECT FileID 
    FROM mp_MediaFile 
    WHERE TrackID IN (
        SELECT TrackID 
        FROM mp_MediaTrack 
        WHERE PlayerID IN (
            SELECT PlayerID 
            FROM mp_MediaPlayer 
            WHERE ModuleID IN (
                SELECT ModuleID 
                FROM mp_Modules 
                WHERE SiteID = ?SiteID
            )
        )
    )
);";

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
	/// Gets a count of rows for a particular Track in the doan_MediaFiles table.
	/// </summary>
	/// <param name="trackID">The ID of the Track.</param>
	/// <returns>The count of rows.</returns>
	public static int GetCountByTrack(int trackId)
	{
		string sqlCommand = @"
SELECT count(*) 
FROM mp_MediaFile 
WHERE TrackID = ?trackId";

		var arParams = new List<MySqlParameter>
		{
			new("?TrackID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = trackId
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));
	}

	/// <summary>
	/// Selects a particular File from the doan_MediaFiles table.
	/// </summary>
	/// <param name="fileID">The ID of the file.</param>
	/// <returns>An IDataReader containing the Media File data.</returns>
	public static IDataReader Select(int fileId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_MediaFile 
WHERE FileID = ?FileID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?FileID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = fileId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}

	/// <summary>
	/// Selects all Files for a Track from the doan_MediaFiles table.
	/// </summary>
	/// <param name="trackID">The ID of the Track.</param>
	/// <returns>An IDataReader containing the Media File(s) data.</returns>
	public static IDataReader SelectByTrack(int trackId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_MediaFile 
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
	/// Selects all Files for a Player from the mp_MediaFile table.
	/// </summary>
	/// <param name="trackID">The ID of the Player.</param>
	/// <returns>An IDataReader containing the Media File(s) data.</returns>
	public static IDataReader SelectByPlayer(int playerId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_MediaFile 
WHERE TrackID IN (
    SELECT TrackID 
    FROM mp_MediaTrack 
    WHERE PlayerID = ?PlayerID
) ;";

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
