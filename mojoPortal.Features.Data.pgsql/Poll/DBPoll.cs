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
    
    public static class DBPoll
    {
        /// <summary>
        /// Inserts a row in the mp_Polls table. Returns rows affected count.
        /// </summary>
        /// <param name="pollGuid"> pollGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="question"> question </param>
        /// <param name="active"> active </param>
        /// <param name="anonymousVoting"> anonymousVoting </param>
        /// <param name="allowViewingResultsBeforeVoting"> allowViewingResultsBeforeVoting </param>
        /// <param name="showOrderNumbers"> showOrderNumbers </param>
        /// <param name="showResultsWhenDeactivated"> showResultsWhenDeactivated </param>
        /// <param name="activeFrom"> activeFrom </param>
        /// <param name="activeTo"> activeTo </param>
        /// <returns>int</returns>
        public static int Add(
            Guid pollGuid,
            Guid siteGuid,
            string question,
            bool active,
            bool anonymousVoting,
            bool allowViewingResultsBeforeVoting,
            bool showOrderNumbers,
            bool showResultsWhenDeactivated,
            DateTime activeFrom,
            DateTime activeTo)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[10];

            arParams[0] = new NpgsqlParameter("pollguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid.ToString();

            arParams[1] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter("question", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = question;

            arParams[3] = new NpgsqlParameter("active", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = active;

            arParams[4] = new NpgsqlParameter("anonymousvoting", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = anonymousVoting;

            arParams[5] = new NpgsqlParameter("allowviewingresultsbeforevoting", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = allowViewingResultsBeforeVoting;

            arParams[6] = new NpgsqlParameter("showordernumbers", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = showOrderNumbers;

            arParams[7] = new NpgsqlParameter("showresultswhendeactivated", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = showResultsWhenDeactivated;

            arParams[8] = new NpgsqlParameter("activefrom", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = activeFrom;

            arParams[9] = new NpgsqlParameter("activeto", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = activeTo;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_polls (");
            sqlCommand.Append("pollguid, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("question, ");
            sqlCommand.Append("active, ");
            sqlCommand.Append("anonymousvoting, ");
            sqlCommand.Append("allowviewingresultsbeforevoting, ");
            sqlCommand.Append("showordernumbers, ");
            sqlCommand.Append("showresultswhendeactivated, ");
            sqlCommand.Append("activefrom, ");
            sqlCommand.Append("activeto )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":pollguid, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":question, ");
            sqlCommand.Append(":active, ");
            sqlCommand.Append(":anonymousvoting, ");
            sqlCommand.Append(":allowviewingresultsbeforevoting, ");
            sqlCommand.Append(":showordernumbers, ");
            sqlCommand.Append(":showresultswhendeactivated, ");
            sqlCommand.Append(":activefrom, ");
            sqlCommand.Append(":activeto ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the mp_Polls table. Returns true if row updated.
        /// </summary>
        /// <param name="pollGuid"> pollGuid </param>
        /// <param name="question"> question </param>
        /// <param name="active"> active </param>
        /// <param name="anonymousVoting"> anonymousVoting </param>
        /// <param name="allowViewingResultsBeforeVoting"> allowViewingResultsBeforeVoting </param>
        /// <param name="showOrderNumbers"> showOrderNumbers </param>
        /// <param name="showResultsWhenDeactivated"> showResultsWhenDeactivated </param>
        /// <param name="activeFrom"> activeFrom </param>
        /// <param name="activeTo"> activeTo </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid pollGuid,
            String question,
            bool anonymousVoting,
            bool allowViewingResultsBeforeVoting,
            bool showOrderNumbers,
            bool showResultsWhenDeactivated,
            bool active,
            DateTime activeFrom,
            DateTime activeTo)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[10];

            arParams[0] = new NpgsqlParameter("pollguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid.ToString();

            arParams[1] = new NpgsqlParameter("question", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = question;

            arParams[2] = new NpgsqlParameter("active", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = active;

            arParams[3] = new NpgsqlParameter("anonymousvoting", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = anonymousVoting;

            arParams[4] = new NpgsqlParameter("allowviewingresultsbeforevoting", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = allowViewingResultsBeforeVoting;

            arParams[5] = new NpgsqlParameter("showordernumbers", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = showOrderNumbers;

            arParams[6] = new NpgsqlParameter("showresultswhendeactivated", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = showResultsWhenDeactivated;

            arParams[7] = new NpgsqlParameter("activefrom", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = activeFrom;

            arParams[8] = new NpgsqlParameter("activeto", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = activeTo;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_polls ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("question = :question, ");
            sqlCommand.Append("active = :active, ");
            sqlCommand.Append("anonymousvoting = :anonymousvoting, ");
            sqlCommand.Append("allowviewingresultsbeforevoting = :allowviewingresultsbeforevoting, ");
            sqlCommand.Append("showordernumbers = :showordernumbers, ");
            sqlCommand.Append("showresultswhendeactivated = :showresultswhendeactivated, ");
            sqlCommand.Append("activefrom = :activefrom, ");
            sqlCommand.Append("activeto = :activeto ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("pollguid = :pollguid ");
            sqlCommand.Append(";");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_Polls table. Returns true if row deleted.
        /// </summary>
        /// <param name="pollGuid"> pollGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid pollGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("DELETE FROM mp_pollmodules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pollguid = :pollguid ");
            sqlCommand.Append(";");

            sqlCommand.Append("DELETE FROM mp_pollusers ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pollguid = :pollguid ");
            sqlCommand.Append(";");

            sqlCommand.Append("DELETE FROM mp_polloptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pollguid = :pollguid ");
            sqlCommand.Append(";");

            sqlCommand.Append("DELETE FROM mp_polls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pollguid = :pollguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("pollguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("DELETE FROM mp_pollmodules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid IN (SELECT moduleid FROM mp_modules WHERE siteid = :siteid) ");
            sqlCommand.Append(";");

            sqlCommand.Append("DELETE FROM mp_pollusers ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pollguid IN (SELECT pollguid FROM mp_polls WHERE siteguid IN (SELECT siteguid FROM mp_sites WHERE siteid = :siteid)) ");
            sqlCommand.Append(";");

            sqlCommand.Append("DELETE FROM mp_polloptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pollguid IN (SELECT pollguid FROM mp_Polls WHERE siteguid IN (SELECT siteguid FROM mp_sites WHERE siteid = :siteid)) ");
            sqlCommand.Append(";");

            sqlCommand.Append("DELETE FROM mp_polls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pollguid IN (SELECT pollguid FROM mp_Polls WHERE siteguid IN (SELECT siteguid FROM mp_sites WHERE siteid = :siteid)) ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Clears the vote count a row from the mp_Polls table. 
        /// </summary>
        /// <param name="pollGuid"> pollGuid </param>
        /// <returns>bool</returns>
        public static bool ClearVotes(Guid pollGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_pollusers ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pollguid = :pollguid ");
            sqlCommand.Append("; ");

            sqlCommand.Append("UPDATE mp_pollusers ");
            sqlCommand.Append("SET Votes = 0 ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pollguid = :pollguid ");
            sqlCommand.Append("; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("pollguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_Polls table.
        /// </summary>
        /// <param name="pollGuid"> pollGuid </param>
        public static IDataReader GetPoll(Guid pollGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  p.*, ");
            sqlCommand.Append("(SELECT SUM(votes) FROM mp_polloptions WHERE mp_polloptions.pollguid = :pollguid) As totalvotes ");

            sqlCommand.Append("FROM	mp_polls p ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.pollguid = :pollguid ");
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
        /// Gets an IDataReader with one row from the mp_Polls table.
        /// </summary>
        /// <param name="pollGuid"> pollGuid </param>
        public static IDataReader GetPollByModuleID(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  p.*, ");
            sqlCommand.Append("(SELECT SUM(po.votes) FROM mp_polloptions po WHERE po.pollguid = p.pollguid) As totalvotes ");

            sqlCommand.Append("FROM	mp_polls p ");

            sqlCommand.Append("JOIN mp_pollmodules pm ");
            sqlCommand.Append("ON p.pollguid = pm.pollguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pm.moduleid = :moduleid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with all rows from the mp_Polls table matching the siteGuid.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        public static IDataReader GetPolls(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_polls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid = :siteguid ");
            sqlCommand.Append("ORDER BY activefrom DESC, question ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        /// <summary>
        /// Gets an IDataReader with all rows from the mp_Polls table matching the siteGuid.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        public static IDataReader GetActivePolls(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_polls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid = :siteguid ");
            //sqlCommand.Append("AND Active = 1 ");
            sqlCommand.Append("AND activefrom <= :currenttime ");
            sqlCommand.Append("AND activeto >= :currenttime ");
            sqlCommand.Append("ORDER BY question ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with all rows from the mp_Polls table matching the siteGuid.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        public static IDataReader GetPollsByUserGuid(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  p.*, ");
            sqlCommand.Append("po.optionguid, ");
            sqlCommand.Append("po.answer ");

            sqlCommand.Append("FROM	mp_polls p ");

            sqlCommand.Append("JOIN mp_pollusers pu ");
            sqlCommand.Append("ON p.pollguid = pu.pollguid ");

            sqlCommand.Append("JOIN mp_polloptions po ");
            sqlCommand.Append("ON pu.optionguid = po.optionguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pu.userguid = :userguid ");

            sqlCommand.Append("ORDER BY activefrom DESC, question ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        /// <summary>
        /// Gets a count of rows in the mp_Polls table.
        /// </summary>
        public static bool UserHasVoted(Guid pollGuid, Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_pollusers ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userguid = :userguid ");
            sqlCommand.Append("AND pollguid = :pollguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter("pollguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pollGuid.ToString();

            int count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        /// <summary>
        /// to comment
        /// </summary>
        /// <param name="pollGuid"> pollGuid </param>
        /// <returns>bool</returns>
        public static bool AddToModule(Guid pollGuid, int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("DELETE FROM mp_pollmodules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid = :moduleid ");
            sqlCommand.Append(";");

            sqlCommand.Append("INSERT INTO mp_pollmodules (pollguid, moduleid) ");
            sqlCommand.Append("VALUES( ");
            sqlCommand.Append(":pollguid, :moduleid ");
            sqlCommand.Append(");");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("pollguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pollGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// to comment
        /// </summary>
        /// <param name="moduleID"> moduleID </param>
        /// <returns>bool</returns>
        public static bool RemoveFromModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("DELETE FROM mp_pollmodules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid = :moduleid ");
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


    }
}
