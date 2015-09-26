// Author:					Joe Audette
// Created:					2011-12-03
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
using System.Data.SqlServerCe;

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
            sqlCommand.Append("INSERT INTO mp_MediaTrack ");
            sqlCommand.Append("(");
            sqlCommand.Append("PlayerID, ");
            sqlCommand.Append("TrackType, ");
            sqlCommand.Append("TrackOrder, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("Artist, ");
            sqlCommand.Append("CreatedDate, ");
            sqlCommand.Append("UserGuid ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@PlayerID, ");
            sqlCommand.Append("@TrackType, ");
            sqlCommand.Append("@TrackOrder, ");
            sqlCommand.Append("@Name, ");
            sqlCommand.Append("@Artist, ");
            sqlCommand.Append("@CreatedDate, ");
            sqlCommand.Append("@UserGuid ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[7];

            arParams[0] = new SqlCeParameter("@PlayerID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = playerId;

            arParams[1] = new SqlCeParameter("@TrackType", SqlDbType.NVarChar, 10);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = trackType;

            arParams[2] = new SqlCeParameter("@TrackOrder", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = trackOrder;

            arParams[3] = new SqlCeParameter("@Name", SqlDbType.NVarChar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = name;

            arParams[4] = new SqlCeParameter("@Artist", SqlDbType.NVarChar, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = artist;

            arParams[5] = new SqlCeParameter("@CreatedDate", SqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = DateTime.UtcNow;

            arParams[6] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = userGuid;


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;
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
            sqlCommand.Append("UPDATE mp_MediaTrack ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("PlayerID = @PlayerID, ");
            sqlCommand.Append("TrackType = @TrackType, ");
            sqlCommand.Append("TrackOrder = @TrackOrder, ");
            sqlCommand.Append("Name = @Name, ");
            sqlCommand.Append("Artist = @Artist, ");

            sqlCommand.Append("UserGuid = @UserGuid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("TrackID = @TrackID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[7];

            arParams[0] = new SqlCeParameter("@TrackID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = trackId;

            arParams[1] = new SqlCeParameter("@PlayerID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = playerId;

            arParams[2] = new SqlCeParameter("@TrackType", SqlDbType.NVarChar, 10);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = trackType;

            arParams[3] = new SqlCeParameter("@TrackOrder", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = trackOrder;

            arParams[4] = new SqlCeParameter("@Name", SqlDbType.NVarChar, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = name;

            arParams[5] = new SqlCeParameter("@Artist", SqlDbType.NVarChar, 100);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = artist;

            arParams[6] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = userGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
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
            sqlCommand.Append("UPDATE mp_MediaTrack ");
            sqlCommand.Append("SET TrackOrder = TrackOrder - 1 ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PlayerID = @PlayerID ");
            sqlCommand.Append("AND TrackOrder > @TrackOrder ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@PlayerID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = playerId;

            arParams[1] = new SqlCeParameter("@TrackOrder", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = trackOrder;

            
            int rowsAffected = SqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
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
            sqlCommand.Append("DELETE FROM mp_MediaTrack ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("TrackID = @TrackID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@TrackID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = trackId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
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
            sqlCommand.Append("DELETE FROM mp_MediaTrack ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PlayerID = @PlayerID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PlayerID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = playerId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByModule(int moduleId)
        {
            DBMediaFile.DeleteByModule(moduleId);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_MediaTrack ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PlayerID IN (SELECT PlayerID FROM mp_MediaPlayer WHERE ModuleID = @ModuleID) ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static bool DeleteBySite(int siteId)
        {
            DBMediaFile.DeleteBySite(siteId);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_MediaTrack ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PlayerID IN (SELECT PlayerID FROM mp_MediaPlayer WHERE ModuleID IN ( ");
            sqlCommand.Append("SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID)) ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
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
            sqlCommand.Append("FROM	mp_MediaTrack ");
            sqlCommand.Append("WHERE PlayerID = @PlayerID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PlayerID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = playerId;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
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
            sqlCommand.Append("FROM	mp_MediaTrack ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("TrackID = @TrackID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@TrackID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = trackId;

            return SqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
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
            sqlCommand.Append("FROM	mp_MediaTrack ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PlayerID = @PlayerID ");
            sqlCommand.Append("ORDER BY TrackOrder ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PlayerID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = playerId;

            return SqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

    }
}
