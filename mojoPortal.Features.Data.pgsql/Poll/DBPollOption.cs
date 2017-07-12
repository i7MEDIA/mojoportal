/// Author:				
/// Created:			2008-08-29
/// Last Modified:		2012-08-13
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.


using System;
using System.Data;
using System.Text;
using mojoPortal.Data;
using Npgsql;

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
            sqlCommand.Append("INSERT INTO mp_polloptions (");
            sqlCommand.Append("optionguid, ");
            sqlCommand.Append("pollguid, ");
            sqlCommand.Append("answer, ");
            sqlCommand.Append("votes, ");
            sqlCommand.Append("\"order\" )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":optionguid, ");
            sqlCommand.Append(":pollguid, ");
            sqlCommand.Append(":answer, ");
            sqlCommand.Append(":votes, ");
            sqlCommand.Append(":sort ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[5];

            arParams[0] = new NpgsqlParameter("optionguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = optionGuid.ToString();

            arParams[1] = new NpgsqlParameter("pollguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pollGuid.ToString();

            arParams[2] = new NpgsqlParameter("answer", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = answer;

            arParams[3] = new NpgsqlParameter("votes", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = votes;

            arParams[4] = new NpgsqlParameter("sort", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = order;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("UPDATE mp_polloptions ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("answer = :answer, ");
            sqlCommand.Append("\"order\" = :sort ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("optionguid = :optionguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("optionguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = optionGuid.ToString();

            arParams[1] = new NpgsqlParameter("answer", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = answer;

            arParams[2] = new NpgsqlParameter("sort", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = order;


            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_PollOptions table. Returns true if row deleted.
        /// </summary>
        /// <param name="optionGuid"> optionGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid optionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_polloptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("optionguid = :optionguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("optionguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = optionGuid.ToString();
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader from the mp_PollOptions table.
        /// </summary>
        /// <param name="optionGuid"> pollGuid </param>
        public static IDataReader GetPollOptions(Guid pollGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_polloptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pollguid = :pollguid ");
            sqlCommand.Append("ORDER BY \"order\", answer ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("pollguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("FROM	mp_polloptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("optionguid = :optionguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("optionguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = optionGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
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
                sqlCommand.Append("INSERT INTO mp_pollusers (");
                sqlCommand.Append("pollguid, ");
                sqlCommand.Append("optionguid, ");
                sqlCommand.Append("userguid )");

                sqlCommand.Append(" VALUES (");
                sqlCommand.Append(":pollguid, ");
                sqlCommand.Append(":optionguid, ");
                sqlCommand.Append(":userguid )");
                sqlCommand.Append(";");
            }

            sqlCommand.Append("UPDATE mp_polloptions ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("votes = votes + 1 ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("optionguid = :optionguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("pollguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid.ToString();

            arParams[1] = new NpgsqlParameter("optionguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = optionGuid.ToString();

            arParams[2] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


    }
}
