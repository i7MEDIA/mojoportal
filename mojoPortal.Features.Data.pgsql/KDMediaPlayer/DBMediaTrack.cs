// Author:					
// Created:					2011-12-02
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.


using System;
using System.Data;
using System.Text;
using mojoPortal.Data;
using Npgsql;

namespace mojoPortal.MediaPlayer.Data
{
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
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("INSERT INTO mp_mediatrack (");
            sqlCommand.Append("playerid, ");
            sqlCommand.Append("tracktype, ");
            sqlCommand.Append("trackorder, ");
            sqlCommand.Append("name, ");
            sqlCommand.Append("artist, ");
            sqlCommand.Append("createddate, ");
            sqlCommand.Append("userguid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":playerid, ");
            sqlCommand.Append(":tracktype, ");
            sqlCommand.Append(":trackorder, ");
            sqlCommand.Append(":name, ");
            sqlCommand.Append(":artist, ");
            sqlCommand.Append(":createddate, ");
            sqlCommand.Append(":userguid )");
            sqlCommand.Append(";");
            sqlCommand.Append(" SELECT CURRVAL('mp_mediatrackid_seq');");

            NpgsqlParameter[] arParams = new NpgsqlParameter[7];

            arParams[0] = new NpgsqlParameter("playerid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = playerId;

            arParams[1] = new NpgsqlParameter("tracktype", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = trackType;

            arParams[2] = new NpgsqlParameter("trackorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = trackOrder;

            arParams[3] = new NpgsqlParameter("name", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = name;

            arParams[4] = new NpgsqlParameter("artist", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = artist;

            arParams[5] = new NpgsqlParameter("createddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = DateTime.UtcNow;

            arParams[6] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = userGuid.ToString();


            int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));


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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_mediatrack ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("playerid = :playerid, ");
            sqlCommand.Append("tracktype = :tracktype, ");
            sqlCommand.Append("trackorder = :trackorder, ");
            sqlCommand.Append("name = :name, ");
            sqlCommand.Append("artist = :artist, ");
            
            sqlCommand.Append("userguid = :userguid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("trackid = :trackid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[7];

            arParams[0] = new NpgsqlParameter("trackid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = trackId;

            arParams[1] = new NpgsqlParameter("playerid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = playerId;

            arParams[2] = new NpgsqlParameter("tracktype", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = trackType;

            arParams[3] = new NpgsqlParameter("trackorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = trackOrder;

            arParams[4] = new NpgsqlParameter("name", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = name;

            arParams[5] = new NpgsqlParameter("artist", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = artist;

            arParams[6] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = userGuid.ToString();


            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_mediatrack ");
            sqlCommand.Append("SET trackorder = trackorder - 1 ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("playerid = :playerid ");
            sqlCommand.Append("AND trackorder > :trackorder ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("playerid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = playerId;

            arParams[1] = new NpgsqlParameter("trackorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = trackOrder;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_mediatrack ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("trackid = :trackid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("trackid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = trackId;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes all rows from the doan_MediaTracks table for a particular player instance.
        /// </summary>
        /// <param name="playerID">The ID of the player.</param>
        /// <returns>True if rows were deleted.</returns>
        public static bool DeleteByPlayer(int playerId)
        {
            DBMediaFile.DeleteByPlayer(playerId);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_mediatrack ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("playerid = :playerid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("playerid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = playerId;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool DeleteByModule(int moduleId)
        {
            DBMediaFile.DeleteByModule(moduleId);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_mediatrack ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("playerid IN (SELECT playerid FROM mp_mediaplayer WHERE moduleid = :moduleid) ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(int siteId)
        {
            DBMediaFile.DeleteBySite(siteId);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_mediatrack ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("playerid IN (SELECT playerid FROM mp_mediaplayer WHERE moduleid IN ( ");
            sqlCommand.Append("SELECT moduleid FROM mp_modules WHERE siteid = :siteid)) ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        /// <summary>
        /// Gets a count of rows for a particular player in the doan_MediaTracks table.
        /// </summary>
        /// <param name="playerID">The ID of the player.</param>
        /// <returns>The count of rows.</returns>
        public static int GetCountByPlayer(int playerId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_mediatrack ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("playerid = :playerid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("playerid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = playerId;

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_mediatrack ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("trackid = :trackid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("trackid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = trackId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_mediatrack ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("playerid = :playerid ");
            sqlCommand.Append("ORDER BY trackorder ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("playerid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = playerId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

    }
}
