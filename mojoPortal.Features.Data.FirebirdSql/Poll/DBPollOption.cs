/// Author:				Joe Audette
/// Created:			2008-08-29
/// Last Modified:		2008-09-13
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
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using FirebirdSql.Data.FirebirdClient;
//using log4net;
using mojoPortal.Data;

namespace PollFeature.Data
{
   
    public static class DBPollOption
    {
        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

        }

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

            FbParameter[] arParams = new FbParameter[5];

            arParams[0] = new FbParameter("@OptionGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = optionGuid.ToString();

            arParams[1] = new FbParameter("@PollGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pollGuid.ToString();

            arParams[2] = new FbParameter("@Answer", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = answer;

            arParams[3] = new FbParameter("@Votes", FbDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = votes;

            arParams[4] = new FbParameter("@Order", FbDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = order;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_PollOptions (");
            sqlCommand.Append("OptionGuid, ");
            sqlCommand.Append("PollGuid, ");
            sqlCommand.Append("Answer, ");
            sqlCommand.Append("Votes, ");
            sqlCommand.Append("\"Order\" )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("@OptionGuid, ");
            sqlCommand.Append("@PollGuid, ");
            sqlCommand.Append("@Answer, ");
            sqlCommand.Append("@Votes, ");
            sqlCommand.Append("@Order )");
            sqlCommand.Append(";");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("Answer = @Answer, ");
            sqlCommand.Append("\"Order\" = @Order ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("OptionGuid = @OptionGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@OptionGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = optionGuid.ToString();

            arParams[1] = new FbParameter("@Answer", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = answer;

            arParams[2] = new FbParameter("@Order", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = order;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("DELETE FROM mp_PollOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("OptionGuid = @OptionGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@OptionGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = optionGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("FROM	mp_PollOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PollGuid = @PollGuid ");
            sqlCommand.Append("ORDER BY \"Order\", Answer ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@PollGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("OptionGuid = @OptionGuid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@OptionGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = optionGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static bool IncrementVotes(
            Guid pollGuid,
            Guid optionGuid,
            Guid userGuid)
        {
            if (DBPoll.UserHasVoted(pollGuid, userGuid)) return false;

            
            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@PollGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid.ToString();

            arParams[1] = new FbParameter("@OptionGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = optionGuid.ToString();

            arParams[2] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid.ToString();

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
            }

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_PollOptions ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("Votes = Votes + 1 ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("OptionGuid = @OptionGuid ");
            sqlCommand.Append(";");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


    }
}
