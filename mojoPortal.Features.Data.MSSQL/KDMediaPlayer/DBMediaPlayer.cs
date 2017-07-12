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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_MediaPlayer_Insert", 5);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@PlayerType", SqlDbType.NVarChar, 10, ParameterDirection.Input, playerType);
            sph.DefineSqlParameter("@Skin", SqlDbType.NVarChar, 50, ParameterDirection.Input, skin);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_MediaPlayer_Update", 6);
            sph.DefineSqlParameter("@PlayerID", SqlDbType.Int, ParameterDirection.Input, playerId);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@PlayerType", SqlDbType.NVarChar, 10, ParameterDirection.Input, playerType);
            sph.DefineSqlParameter("@Skin", SqlDbType.NVarChar, 50, ParameterDirection.Input, skin);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }


        /// <summary>
        /// Deletes a row from the doan_MediaPlayers table.
        /// </summary>
        /// <param name="playerID">The ID of the Media Player.</param>
        /// <returns>Returns true if row deleted.</returns>
        public static bool Delete(int playerId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_MediaPlayer_Delete", 1);
            sph.DefineSqlParameter("@PlayerID", SqlDbType.Int, ParameterDirection.Input, playerId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes the Player that is for a Module.
        /// </summary>
        /// <param name="moduleID">The ID of the Module.</param>
        /// <returns>Returns true if row deleted.</returns>
        public static bool DeleteByModule(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_MediaPlayer_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_MediaPlayer_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }


        /// <summary>
        /// Gets an IDataReader with one row from the doan_MediaPlayers table.
        /// </summary>
        /// <param name="moduleID">The ID of the Media Player.</param>
        /// <returns>The data for the Medai Player as an IDataReader object.</returns>
        public static IDataReader Select(int playerId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_MediaPlayer_Select", 1);
            sph.DefineSqlParameter("@PlayerID", SqlDbType.Int, ParameterDirection.Input, playerId);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets an IDataReader with the row from the doan_MediaPlayers table that exists for a Module.
        /// </summary>
        /// <param name="moduleID">The ID of the Module.</param>
        /// <returns>The data for the Medai Player as an IDataReader object.</returns>
        public static IDataReader SelectByModule(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_MediaPlayer_SelectByModule", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            return sph.ExecuteReader();
        }

        // is this one used?

        ///// <summary>
        ///// Gets an IDataReader with all rows in the doan_MediaPlayers table.
        ///// </summary>
        ///// <returns>The data for the Medai Player as an IDataReader object.</returns>
        //public static IDataReader SelectAll()
        //{
        //    return SqlHelper.ExecuteReader(ConnectionString.GetReadConnectionString(), 
        //        CommandType.StoredProcedure, 
        //        "mp_MediaPlayer_SelectAll", 
        //        null);
        //}


    }
}
