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


using System;
using System.Data;
using System.Text;
using mojoPortal.Data;
using Mono.Data.Sqlite;

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_MediaPlayer (");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("PlayerType, ");
            sqlCommand.Append("Skin, ");
            sqlCommand.Append("CreatedDate, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("ModuleGuid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":ModuleID, ");
            sqlCommand.Append(":PlayerType, ");
            sqlCommand.Append(":Skin, ");
            sqlCommand.Append(":CreatedDate, ");
            sqlCommand.Append(":UserGuid, ");
            sqlCommand.Append(":ModuleGuid )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[6];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":PlayerType", DbType.String, 10);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = playerType;

            arParams[2] = new SqliteParameter(":Skin", DbType.String, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = skin;

            arParams[3] = new SqliteParameter(":CreatedDate", DbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = DateTime.UtcNow;

            arParams[4] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userGuid.ToString();

            arParams[5] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = moduleGuid.ToString();


            int newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
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
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_MediaPlayer ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("ModuleID = :ModuleID, ");
            sqlCommand.Append("PlayerType = :PlayerType, ");
            sqlCommand.Append("Skin = :Skin, ");

            sqlCommand.Append("UserGuid = :UserGuid, ");
            sqlCommand.Append("ModuleGuid = :ModuleGuid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("PlayerID = :PlayerID ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[6];

            arParams[0] = new SqliteParameter(":PlayerID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = playerId;

            arParams[1] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new SqliteParameter(":PlayerType", DbType.String, 10);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = playerType;

            arParams[3] = new SqliteParameter(":Skin", DbType.String, 50);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = skin;

            arParams[4] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userGuid.ToString();

            arParams[5] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = moduleGuid.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the doan_MediaPlayers table.
        /// </summary>
        /// <param name="playerID">The ID of the Media Player.</param>
        /// <returns>Returns true if row deleted.</returns>
        public static bool Delete(int playerId)
        {
            DBMediaTrack.DeleteByPlayer(playerId);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_MediaPlayer ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PlayerID = :PlayerID ");
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

        /// <summary>
        /// Deletes the Player that is for a Module.
        /// </summary>
        /// <param name="moduleID">The ID of the Module.</param>
        /// <returns>Returns true if row deleted.</returns>
        public static bool DeleteByModule(int moduleId)
        {
            DBMediaTrack.DeleteByModule(moduleId);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_MediaPlayer ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = :ModuleID ");
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
            DBMediaTrack.DeleteBySite(siteId);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_MediaPlayer ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID) ");
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
        /// Gets an IDataReader with one row from the doan_MediaPlayers table.
        /// </summary>
        /// <param name="moduleID">The ID of the Media Player.</param>
        /// <returns>The data for the Medai Player as an IDataReader object.</returns>
        public static IDataReader Select(int playerId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_MediaPlayer ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PlayerID = :PlayerID ");
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

        /// <summary>
        /// Gets an IDataReader with the row from the doan_MediaPlayers table that exists for a Module.
        /// </summary>
        /// <param name="moduleID">The ID of the Module.</param>
        /// <returns>The data for the Medai Player as an IDataReader object.</returns>
        public static IDataReader SelectByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_MediaPlayer ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = :ModuleID ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


    }
}
