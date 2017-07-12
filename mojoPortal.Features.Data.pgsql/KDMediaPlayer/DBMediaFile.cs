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
// Modified 2011-12-03
// Modified 2011-12-06 JA added SelectByPlayer



using System;
using System.Data;
using System.Text;
using mojoPortal.Data;
using Npgsql;

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
            sqlCommand.Append("INSERT INTO mp_mediafile (");
            sqlCommand.Append("trackid, ");
            sqlCommand.Append("filepath, ");
            sqlCommand.Append("addeddate, ");
            sqlCommand.Append("userguid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":trackid, ");
            sqlCommand.Append(":filepath, ");
            sqlCommand.Append(":addeddate, ");
            sqlCommand.Append(":userguid )");
            sqlCommand.Append(";");
            sqlCommand.Append(" SELECT CURRVAL('mp_mediafileid_seq');");

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("trackid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = trackId;

            arParams[1] = new NpgsqlParameter("filepath", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = filePath;

            arParams[2] = new NpgsqlParameter("addeddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            arParams[3] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();


            int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));


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
            sqlCommand.Append("DELETE FROM mp_mediafile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("fileid = :fileid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("fileid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = fileId;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByTrack(int trackId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_mediafile ");
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

        public static bool DeleteByPlayer(int playerId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_mediafile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("fileid  ");
            sqlCommand.Append("IN (");
            sqlCommand.Append("SELECT fileid FROM mp_mediafile WHERE trackid IN (");
            sqlCommand.Append("SELECT trackid FROM mp_mediatrack WHERE playerid = :playerid");
            sqlCommand.Append(")");
            sqlCommand.Append(")");
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_mediafile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("fileid  ");
            sqlCommand.Append("IN (");
            sqlCommand.Append("SELECT fileid FROM mp_mediafile WHERE trackid IN (");
            sqlCommand.Append("SELECT trackid FROM mp_mediatrack WHERE playerid IN (");
            sqlCommand.Append("SELECT playerid FROM mp_mediaplayer WHERE moduleid = :moduleid");
            sqlCommand.Append(")");
            sqlCommand.Append(")");
            sqlCommand.Append(")");
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_mediafile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("fileid  ");
            sqlCommand.Append("IN (");
            sqlCommand.Append("SELECT fileid FROM mp_mediafile WHERE trackid IN (");
            sqlCommand.Append("SELECT trackid FROM mp_mediatrack WHERE playerid IN (");
            sqlCommand.Append("SELECT playerid FROM mp_mediaplayer WHERE moduleid IN (");
            sqlCommand.Append("SELECT moduleid FROM mp_modules WHERE siteid = :siteid");
            sqlCommand.Append(")");
            sqlCommand.Append(")");
            sqlCommand.Append(")");
            sqlCommand.Append(")");
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
        /// Gets a count of rows for a particular Track in the doan_MediaFiles table.
        /// </summary>
        /// <param name="trackID">The ID of the Track.</param>
        /// <returns>The count of rows.</returns>
        public static int GetCountByTrack(int trackId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_mediafile ");
            sqlCommand.Append("WHERE trackid = :trackid;");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("trackid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = trackId;

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("FROM	mp_mediafile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("fileid = :fileid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("fileid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = fileId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("FROM	mp_mediafile ");
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
        /// Selects all Files for a Player from the mp_MediaFile table.
        /// </summary>
        /// <param name="trackID">The ID of the Player.</param>
        /// <returns>An IDataReader containing the Media File(s) data.</returns>
        public static IDataReader SelectByPlayer(int playerId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_mediafile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("trackid IN (SELECT trackid FROM mp_mediatrack WHERE playerid = :playerid) ");
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
