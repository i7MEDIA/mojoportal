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
// Modified 2011-12-06 JA added SelectByPlayer

using System;
using System.Data;
using System.Data.Common;
using mojoPortal.Data;

namespace mojoPortal.MediaPlayer.Data
{
    public static class DBMediaFile
    {
        /// <summary>
        /// Inserts a row in the mp_MediaFile table.
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_MediaFile_Insert", 3);
            sph.DefineSqlParameter("@TrackID", SqlDbType.Int, ParameterDirection.Input, trackId);
            sph.DefineSqlParameter("@FilePath", SqlDbType.NVarChar, 100, ParameterDirection.Input, filePath);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }
        
        /// <summary>
        /// Deletes a row from the mp_MediaFile table.
        /// </summary>
        /// <param name="fileID">The ID of the Media File.</param>
        /// <returns>True if row deleted.</returns>
        public static bool Delete(int fileId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_MediaFile_Delete", 1);
            sph.DefineSqlParameter("@FileID", SqlDbType.Int, ParameterDirection.Input, fileId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }


        /// <summary>
        /// Gets a count of rows for a particular Track in the mp_MediaFile table.
        /// </summary>
        /// <param name="trackID">The ID of the Track.</param>
        /// <returns>The count of rows.</returns>
        public static int GetCountByTrack(int trackId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_MediaFile_GetCountByTrack", 1);
            sph.DefineSqlParameter("@TrackID", SqlDbType.Int, ParameterDirection.Input, trackId);
            return Convert.ToInt32(sph.ExecuteScalar());
        }


        /// <summary>
        /// Selects a particular File from the mp_MediaFile table.
        /// </summary>
        /// <param name="fileID">The ID of the file.</param>
        /// <returns>An IDataReader containing the Media File data.</returns>
        public static IDataReader Select(int fileId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_MediaFile_Select", 1);
            sph.DefineSqlParameter("@FileID", SqlDbType.Int, ParameterDirection.Input, fileId);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Selects all Files for a Track from the mp_MediaFile table.
        /// </summary>
        /// <param name="trackID">The ID of the Track.</param>
        /// <returns>An IDataReader containing the Media File(s) data.</returns>
        public static IDataReader SelectByTrack(int trackId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_MediaFile_SelectByTrack", 1);
            sph.DefineSqlParameter("@TrackID", SqlDbType.Int, ParameterDirection.Input, trackId);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Selects all Files for a Player from the mp_MediaFile table.
        /// </summary>
        /// <param name="trackID">The ID of the Player.</param>
        /// <returns>An IDataReader containing the Media File(s) data.</returns>
        public static IDataReader SelectByPlayer(int playerId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_MediaFile_SelectByPlayer", 1);
            sph.DefineSqlParameter("@PlayerID", SqlDbType.Int, ParameterDirection.Input, playerId);
            return sph.ExecuteReader();
        }

    }
}
