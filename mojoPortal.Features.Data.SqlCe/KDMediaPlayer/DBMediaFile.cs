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
// Modified 2011-12-06 JA added SelectByPlayer


using System;
using System.Data;
using System.Text;
using mojoPortal.Data;
using System.Data.SqlServerCe;

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
            sqlCommand.Append("INSERT INTO mp_MediaFile ");
            sqlCommand.Append("(");
            sqlCommand.Append("TrackID, ");
            sqlCommand.Append("FilePath, ");
            sqlCommand.Append("AddedDate, ");
            sqlCommand.Append("UserGuid ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@TrackID, ");
            sqlCommand.Append("@FilePath, ");
            sqlCommand.Append("@AddedDate, ");
            sqlCommand.Append("@UserGuid ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@TrackID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = trackId;

            arParams[1] = new SqlCeParameter("@FilePath", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = filePath;

            arParams[2] = new SqlCeParameter("@AddedDate", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            arParams[3] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid;


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;

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
            sqlCommand.Append("FileID = @FileID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@FileID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = fileId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static bool DeleteByTrack(int trackId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_MediaFile ");
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

        public static bool DeleteByPlayer(int playerId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_MediaFile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FileID  ");
            sqlCommand.Append("IN (");
            sqlCommand.Append("SELECT FileID FROM mp_MediaFile WHERE TrackID IN (");
            sqlCommand.Append("SELECT TrackID FROM mp_MediaTrack WHERE PlayerID = @PlayerID");
            sqlCommand.Append(")");
            sqlCommand.Append(")");
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_MediaFile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FileID  ");
            sqlCommand.Append("IN (");
            sqlCommand.Append("SELECT FileID FROM mp_MediaFile WHERE TrackID IN (");
            sqlCommand.Append("SELECT TrackID FROM mp_MediaTrack WHERE PlayerID IN (");
            sqlCommand.Append("SELECT PlayerID FROM mp_MediaPlayer WHERE ModuleID = @ModuleID");
            sqlCommand.Append(")");
            sqlCommand.Append(")");
            sqlCommand.Append(")");
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_MediaFile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FileID  ");
            sqlCommand.Append("IN (");
            sqlCommand.Append("SELECT FileID FROM mp_MediaFile WHERE TrackID IN (");
            sqlCommand.Append("SELECT TrackID FROM mp_MediaTrack WHERE PlayerID IN (");
            sqlCommand.Append("SELECT PlayerID FROM mp_MediaPlayer WHERE ModuleID IN (");
            sqlCommand.Append("SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID");
            sqlCommand.Append(")");
            sqlCommand.Append(")");
            sqlCommand.Append(")");
            sqlCommand.Append(")");
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
        /// Gets a count of rows for a particular Track in the doan_MediaFiles table.
        /// </summary>
        /// <param name="trackID">The ID of the Track.</param>
        /// <returns>The count of rows.</returns>
        public static int GetCountByTrack(int trackId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT count(*) FROM mp_MediaFile ");
            sqlCommand.Append("WHERE TrackID = @TrackID;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@TrackID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = trackId;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("FileID = @FileID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@FileID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = fileId;

            return SqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("TrackID IN (SELECT TrackID FROM mp_MediaTrack WHERE PlayerID = @PlayerID) ");
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
