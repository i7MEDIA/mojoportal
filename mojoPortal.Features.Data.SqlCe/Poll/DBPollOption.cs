// Author:				Joe Audette
// Created:			    2010-07-02
// Last Modified:		2010-07-08
// 
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.


using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using mojoPortal.Data;

namespace PollFeature.Data
{
    public static class DBPollOption
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }

        public static IDataReader GetPollOptions(Guid pollGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_PollOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PollGuid = @PollGuid ");
            sqlCommand.Append("ORDER BY [Order], Answer ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetPollOption(Guid optionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_PollOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("OptionGuid = @OptionGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@OptionGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = optionGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
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

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid;

            arParams[1] = new SqlCeParameter("@OptionGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = optionGuid;

            arParams[2] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid;

            int rowsAffected;

            StringBuilder sqlCommand = new StringBuilder();
            if (userGuid != Guid.Empty)
            {
                sqlCommand.Append("INSERT INTO mp_PollUsers (");
                sqlCommand.Append("PollGuid, ");
                sqlCommand.Append("OptionGuid, ");
                sqlCommand.Append("UserGuid )");

                sqlCommand.Append(" VALUES (");
                sqlCommand.Append("@PollGuid, ");
                sqlCommand.Append("@OptionGuid, ");
                sqlCommand.Append("@UserGuid )");
                sqlCommand.Append(";");

                rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_PollOptions ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("Votes = Votes + 1 ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("OptionGuid = @OptionGuid ");
            sqlCommand.Append(";");

            arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid;

            arParams[1] = new SqlCeParameter("@OptionGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = optionGuid;

            arParams[2] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static int Add(
            Guid optionGuid,
            Guid pollGuid,
            string answer,
            int order)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_PollOptions ");
            sqlCommand.Append("(");
            sqlCommand.Append("OptionGuid, ");
            sqlCommand.Append("PollGuid, ");
            sqlCommand.Append("Answer, ");
            sqlCommand.Append("Votes, ");
            sqlCommand.Append("[Order] ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@OptionGuid, ");
            sqlCommand.Append("@PollGuid, ");
            sqlCommand.Append("@Answer, ");
            sqlCommand.Append("@Votes, ");
            sqlCommand.Append("@Order ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[5];

            arParams[0] = new SqlCeParameter("@OptionGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = optionGuid;

            arParams[1] = new SqlCeParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pollGuid;

            arParams[2] = new SqlCeParameter("@Answer", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = answer;

            arParams[3] = new SqlCeParameter("@Votes", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = 0;

            arParams[4] = new SqlCeParameter("@Order", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = order;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        public static bool Update(
            Guid optionGuid,
            string answer,
            int order)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_PollOptions ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("Answer = @Answer, ");
            sqlCommand.Append("[Order] = @Order ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("OptionGuid = @OptionGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@OptionGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = optionGuid;

            arParams[1] = new SqlCeParameter("@Answer", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = answer;

            arParams[2] = new SqlCeParameter("@Order", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = order;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(Guid optionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_PollOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("OptionGuid = @OptionGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@OptionGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = optionGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

    }
}
