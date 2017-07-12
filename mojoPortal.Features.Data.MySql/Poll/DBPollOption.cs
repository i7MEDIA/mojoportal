/// Author:				
/// Created:			2008-01-21
/// Last Modified:		2012-07-20
/// 
/// This implementation is for MySQL. 
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using mojoPortal.Data;

namespace PollFeature.Data
{
    public static class DBPollOption
    {
        
        /// <summary>
        /// Inserts a row in the mp_PollOptions table. Returns rows affected count.
        /// </summary>
        /// <param name="optionGuid"> optionGuid </param>
        /// <param name="pollGuid"> pollGuid </param>
        /// <param name="answer"> answer </param>
        /// <param name="votes"> votes </param>
        /// <param name="order"> order </param>
        /// <returns>int</returns>
        public static int Add(
            Guid optionGuid,
            Guid pollGuid,
            string answer,
            int order)
        {
            int votes = 0;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_PollOptions (");
            sqlCommand.Append("OptionGuid, ");
            sqlCommand.Append("PollGuid, ");
            sqlCommand.Append("Answer, ");
            sqlCommand.Append("Votes, ");
            sqlCommand.Append("`Order` )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?OptionGuid, ");
            sqlCommand.Append("?PollGuid, ");
            sqlCommand.Append("?Answer, ");
            sqlCommand.Append("?Votes, ");
            sqlCommand.Append("?Order )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?OptionGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = optionGuid.ToString();

            arParams[1] = new MySqlParameter("?PollGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pollGuid.ToString();

            arParams[2] = new MySqlParameter("?Answer", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = answer;

            arParams[3] = new MySqlParameter("?Votes", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = votes;

            arParams[4] = new MySqlParameter("?Order", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = order;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the mp_PollOptions table. Returns true if row updated.
        /// </summary>
        /// <param name="optionGuid"> optionGuid </param>
        /// <param name="pollGuid"> pollGuid </param>
        /// <param name="answer"> answer </param>
        /// <param name="votes"> votes </param>
        /// <param name="order"> order </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid optionGuid,
            string answer,
            int order)
        {
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_PollOptions ");
            sqlCommand.Append("SET  ");
           
            sqlCommand.Append("Answer = ?Answer, ");
            sqlCommand.Append("`Order` = ?Order ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("OptionGuid = ?OptionGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?OptionGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = optionGuid.ToString();

            arParams[1] = new MySqlParameter("?Answer", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = answer;

            arParams[2] = new MySqlParameter("?Order", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = order;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_PollOptions table. Returns true if row deleted.
        /// </summary>
        /// <param name="optionGuid"> optionGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid optionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_PollOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("OptionGuid = ?OptionGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?OptionGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = optionGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }



        /// <summary>
        /// Gets an IDataReader from the mp_PollOptions table.
        /// </summary>
        /// <param name="optionGuid"> pollGuid </param>
        public static IDataReader GetPollOptions(Guid pollGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_PollOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PollGuid = ?PollGuid ");
            sqlCommand.Append("ORDER BY `Order`, Answer ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?PollGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_PollOptions table.
        /// </summary>
        /// <param name="optionGuid"> optionGuid </param>
        public static IDataReader GetPollOption(Guid optionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_PollOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("OptionGuid = ?OptionGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?OptionGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = optionGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static bool IncrementVotes(
            Guid pollGuid,
            Guid optionGuid,
            Guid userGuid)
        {
            if (DBPoll.UserHasVoted(pollGuid, userGuid)) return false;

            StringBuilder sqlCommand = new StringBuilder();
            if (userGuid != Guid.Empty)
            {
                sqlCommand.Append("INSERT INTO mp_PollUsers (");
                sqlCommand.Append("PollGuid, ");
                sqlCommand.Append("OptionGuid, ");
                sqlCommand.Append("UserGuid )");

                sqlCommand.Append(" VALUES (");
                sqlCommand.Append("?PollGuid, ");
                sqlCommand.Append("?OptionGuid, ");
                sqlCommand.Append("?UserGuid )");
                sqlCommand.Append(";");
            }


            sqlCommand.Append("UPDATE mp_PollOptions ");
            sqlCommand.Append("SET  ");
   
            sqlCommand.Append("Votes = Votes + 1 ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("OptionGuid = ?OptionGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?OptionGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = optionGuid.ToString();

            arParams[1] = new MySqlParameter("?PollGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pollGuid.ToString();

            arParams[2] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);


        }



    }
}
