using mojoPortal.Data;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.MediaPlayer.Data;

public static class DBMediaPlayer
{
	/// <summary>
	/// Inserts a row in the doan_MediaPlayers table.
	/// </summary>
	/// <param name="moduleID">The ID of the Module</param>
	/// <param name="playerType">The Player Type.</param>
	/// <param name="createdDate">The Date the Media Player was created.</param>
	/// <param name="userGuid">The Guid of the user who created the Media Player.</param>
	/// <param name="moduleGuid">The Guid of the Module.</param>
	/// <returns>The ID of the Media Player.</returns>
	public static int Insert(
		int moduleId,
		string playerType,
		String skin,
		Guid userGuid,
		Guid moduleGuid)
	{
		string sqlCommand = @"
INSERT INTO mp_MediaPlayer (
    ModuleID, 
    PlayerType, 
    Skin, 
    CreatedDate, 
    UserGuid, 
    ModuleGuid )
     VALUES (
    ?ModuleID, 
    ?PlayerType, 
    ?Skin, 
    ?CreatedDate, 
    ?UserGuid, 
    ?ModuleGuid 
);
SELECT LAST_INSERT_ID() ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?PlayerType", MySqlDbType.VarChar, 10)
			{
				Direction = ParameterDirection.Input,
				Value = playerType
			},

			new("?Skin", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = skin
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
			},

			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			}
		};

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams).ToString());
		return newID;
	}

	/// <summary>
	/// Updates a row in the doan_MediaPlayers table.
	/// </summary>
	/// <param name="playerID">The ID of the player.</param>
	/// <param name="moduleID">The ID of the Module.</param>
	/// <param name="playerType">The Player Type.</param>
	/// <param name="userGuid">The Guid of the user that created the Media Player.</param>
	/// <param name="moduleGuid">The Guid of the Module.</param>
	/// <returns>True if the update was successful.</returns>
	public static bool Update(
		int playerId,
		int moduleId,
		string playerType,
		String skin,
		Guid userGuid,
		Guid moduleGuid)
	{
		string sqlCommand = @"
UPDATE 
    mp_MediaPlayer 
SET  
    ModuleID = ?ModuleID, 
    PlayerType = ?PlayerType, 
    Skin = ?Skin, 
    CreatedDate = ?CreatedDate, 
    UserGuid = ?UserGuid, 
    ModuleGuid = ?ModuleGuid 
WHERE  
    PlayerID = ?PlayerID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?PlayerID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = playerId
			},

			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?PlayerType", MySqlDbType.VarChar, 10)
			{
				Direction = ParameterDirection.Input,
				Value = playerType
			},

			new("?Skin", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = skin
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
			},

			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;
	}

	/// <summary>
	/// Deletes a row from the doan_MediaPlayers table.
	/// </summary>
	/// <param name="playerID">The ID of the Media Player.</param>
	/// <returns>Returns true if row deleted.</returns>
	public static bool Delete(int playerId)
	{
		DBMediaTrack.DeleteByPlayer(playerId);

		string sqlCommand = @"
DELETE FROM mp_MediaPlayer 
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

	/// <summary>
	/// Deletes the Player that is for a Module.
	/// </summary>
	/// <param name="moduleID">The ID of the Module.</param>
	/// <returns>Returns true if row deleted.</returns>
	public static bool DeleteByModule(int moduleId)
	{
		DBMediaTrack.DeleteByModule(moduleId);

		string sqlCommand = @"
DELETE FROM mp_MediaPlayer 
WHERE ModuleID = ?ModuleID ;";

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
		DBMediaTrack.DeleteBySite(siteId);

		string sqlCommand = @"
DELETE FROM mp_MediaPlayer 
WHERE ModuleID IN (
    SELECT ModuleID 
    FROM mp_Modules 
    WHERE SiteID = ?SiteID
) ;";

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
	/// Gets an IDataReader with one row from the doan_MediaPlayers table.
	/// </summary>
	/// <param name="moduleID">The ID of the Media Player.</param>
	/// <returns>The data for the Medai Player as an IDataReader object.</returns>
	public static IDataReader Select(int playerId)
	{
		string sqlCommand = @"
SELECT 
    PlayerID, 
    ModuleID, 
    PlayerType, 
    Skin, 
    CreatedDate, 
    UserGuid, 
    ModuleGuid 
FROM 
    mp_MediaPlayer 
WHERE 
    PlayerID = ?PlayerID ;";

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

	/// <summary>
	/// Gets an IDataReader with the row from the doan_MediaPlayers table that exists for a Module.
	/// </summary>
	/// <param name="moduleID">The ID of the Module.</param>
	/// <returns>The data for the Medai Player as an IDataReader object.</returns>
	public static IDataReader SelectByModule(int moduleId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_MediaPlayer 
WHERE ModuleID = ?ModuleID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}

	///// <summary>
	///// Gets an IDataReader with all rows in the doan_MediaPlayers table.
	///// </summary>
	///// <returns>The data for the Medai Player as an IDataReader object.</returns>
	//public static IDataReader SelectAll()
	//{
	//    string sqlCommand = @"
	//    sqlCommand.Append("SELECT  * ");
	//    sqlCommand.Append("FROM	doan_MediaPlayers ");
	//    sqlCommand.Append(";");

	//    return CommandHelper.ExecuteReader(
	//        ConnectionString.GetReadConnectionString(),
	//        sqlCommand.ToString(),
	//        null);
	//}
}
