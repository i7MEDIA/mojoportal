// Author:					
// Created:					2011-12-03
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// Modified 2011-12-06 JA added SelectByPlayer


using System;
using System.Data;
using System.Text;
using mojoPortal.Data;
using Mono.Data.Sqlite;

namespace mojoPortal.MediaPlayer.Data
{
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_MediaFile (");
            sqlCommand.Append("TrackID, ");
            sqlCommand.Append("FilePath, ");
            sqlCommand.Append("AddedDate, ");
            sqlCommand.Append("UserGuid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":TrackID, ");
            sqlCommand.Append(":FilePath, ");
            sqlCommand.Append(":AddedDate, ");
            sqlCommand.Append(":UserGuid )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":TrackID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = trackId;

            arParams[1] = new SqliteParameter(":FilePath", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = filePath;

            arParams[2] = new SqliteParameter(":AddedDate", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            arParams[3] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();


            int newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_MediaFile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FileID = :FileID ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":FileID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = fileId;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static bool DeleteByTrack(int trackId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_MediaFile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("TrackID = :TrackID ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":TrackID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = trackId;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        
        public static bool DeleteByPlayer(int playerId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_MediaFile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FileID  ");
            sqlCommand.Append("IN (");
            sqlCommand.Append("SELECT FileID FROM mp_MediaFile WHERE TrackID IN (");
            sqlCommand.Append("SELECT TrackID FROM mp_MediaTrack WHERE PlayerID = :PlayerID");
            sqlCommand.Append(")");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":PlayerID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = playerId;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static bool DeleteByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_MediaFile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FileID  ");
            sqlCommand.Append("IN (");
            sqlCommand.Append("SELECT FileID FROM mp_MediaFile WHERE TrackID IN (");
            sqlCommand.Append("SELECT TrackID FROM mp_MediaTrack WHERE PlayerID IN (");
            sqlCommand.Append("SELECT PlayerID FROM mp_MediaPlayer WHERE ModuleID = :ModuleID");
            sqlCommand.Append(")");
            sqlCommand.Append(")");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_MediaFile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FileID  ");
            sqlCommand.Append("IN (");
            sqlCommand.Append("SELECT FileID FROM mp_MediaFile WHERE TrackID IN (");
            sqlCommand.Append("SELECT TrackID FROM mp_MediaTrack WHERE PlayerID IN (");
            sqlCommand.Append("SELECT PlayerID FROM mp_MediaPlayer WHERE ModuleID IN (");
            sqlCommand.Append("SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID");
            sqlCommand.Append(")");
            sqlCommand.Append(")");
            sqlCommand.Append(")");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets a count of rows for a particular Track in the doan_MediaFiles table.
        /// </summary>
        /// <param name="trackID">The ID of the Track.</param>
        /// <returns>The count of rows.</returns>
        public static int GetCountByTrack(int trackId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT count(*) FROM mp_MediaFile ");
            sqlCommand.Append("WHERE TrackID = :TrackID;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":TrackID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = trackId;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_MediaFile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FileID = :FileID ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":FileID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = fileId;

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_MediaFile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("TrackID = :TrackID ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":TrackID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = trackId;

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_MediaFile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("TrackID IN (SELECT TrackID FROM mp_MediaTrack WHERE PlayerID = :PlayerID) ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":PlayerID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = playerId;

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

    }
}
