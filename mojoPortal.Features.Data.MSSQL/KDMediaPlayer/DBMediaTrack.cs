// Author:					Kerry Doan
// Created:					2011-09-14
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// Modified: 2011-12-02 by  initial integration

using System;
using System.Data;
using System.Data.Common;
using mojoPortal.Data;

namespace mojoPortal.MediaPlayer.Data
{
    public static class DBMediaTrack
    {
        /// <summary>
        /// Inserts a row in the doan_MediaTracks table.
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_MediaTrack_Insert", 6);
            sph.DefineSqlParameter("@PlayerID", SqlDbType.Int, ParameterDirection.Input, playerId);
            sph.DefineSqlParameter("@TrackType", SqlDbType.NVarChar, 10, ParameterDirection.Input, trackType);
            sph.DefineSqlParameter("@TrackOrder", SqlDbType.Int, ParameterDirection.Input, trackOrder);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 100, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Artist", SqlDbType.NVarChar, 100, ParameterDirection.Input, artist);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        /// <summary>
        /// Updates a row in the doan_MediaTracks table.
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_MediaTrack_Update", 7);
            sph.DefineSqlParameter("@TrackID", SqlDbType.Int, ParameterDirection.Input, trackId);
            sph.DefineSqlParameter("@PlayerID", SqlDbType.Int, ParameterDirection.Input, playerId);
            sph.DefineSqlParameter("@TrackType", SqlDbType.NVarChar, 10, ParameterDirection.Input, trackType);
            sph.DefineSqlParameter("@TrackOrder", SqlDbType.Int, ParameterDirection.Input, trackOrder);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 100, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Artist", SqlDbType.NVarChar, 100, ParameterDirection.Input, artist);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_MediaTrack_AdjustTrackOrderForDelete", 2);
            sph.DefineSqlParameter("@PlayerID", SqlDbType.Int, ParameterDirection.Input, playerId);
            sph.DefineSqlParameter("@TrackOrder", SqlDbType.Int, ParameterDirection.Input, trackOrder);
            return sph.ExecuteNonQuery();
        }

        /// <summary>
        /// Deletes a row from the doan_MediaTracks table.
        /// </summary>
        /// <param name="trackID">The ID of the track.</param>
        /// <returns>True if a row was deleted.</returns>
        public static bool Delete(int trackId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_MediaTrack_Delete", 1);
            sph.DefineSqlParameter("@TrackID", SqlDbType.Int, ParameterDirection.Input, trackId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes all rows from the doan_MediaTracks table for a particular player instance.
        /// </summary>
        /// <param name="playerID">The ID of the player.</param>
        /// <returns>True if rows were deleted.</returns>
        public static bool DeleteByPlayer(int playerId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_MediaTrack_DeleteByPlayer", 1);
            sph.DefineSqlParameter("@PlayerID", SqlDbType.Int, ParameterDirection.Input, playerId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }


        /// <summary>
        /// Gets a count of rows for a particular player in the doan_MediaTracks table.
        /// </summary>
        /// <param name="playerID">The ID of the player.</param>
        /// <returns>The count of rows.</returns>
        public static int GetCountByPlayer(int playerId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_MediaTrack_GetCountByPlayer", 1);
            sph.DefineSqlParameter("@PlayerID", SqlDbType.Int, ParameterDirection.Input, playerId);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        /// <summary>
        /// Selects a particular Track from the doan_MediaTracks table.
        /// </summary>
        /// <param name="trackID">The ID of the track.</param>
        /// <returns>An IDataReader containing the MediaTrack data.</returns>
        public static IDataReader Select(int trackId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_MediaTrack_Select", 1);
            sph.DefineSqlParameter("@TrackID", SqlDbType.Int, ParameterDirection.Input, trackId);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Selects all Tracks for a particular player instance from the doan_MediaTracks table.
        /// </summary>
        /// <param name="playerID">The ID of the player.</param>
        /// <returns>An IDataReader containing the MediaTrack(s) data.</returns>
        public static IDataReader SelectByPlayer(int playerId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_MediaTrack_SelectByPlayer", 1);
            sph.DefineSqlParameter("@PlayerID", SqlDbType.Int, ParameterDirection.Input, playerId);
            return sph.ExecuteReader();
        }



        

    }
}
