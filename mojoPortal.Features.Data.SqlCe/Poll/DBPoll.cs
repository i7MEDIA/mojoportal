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
    public static class DBPoll
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }


        public static IDataReader GetPolls(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Polls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("ORDER BY ActiveFrom DESC, Question ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetActivePolls(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Polls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            //sqlCommand.Append("AND Active = 1 ");
            sqlCommand.Append("AND ActiveFrom <= @CurrentTime ");
            sqlCommand.Append("AND ActiveTo >= @CurrentTime ");
            sqlCommand.Append("ORDER BY Question ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetPollsByUserGuid(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  p.*, ");
            sqlCommand.Append("po.OptionGuid, ");
            sqlCommand.Append("po.Answer ");

            sqlCommand.Append("FROM	mp_Polls p ");

            sqlCommand.Append("JOIN mp_PollUsers pu ");
            sqlCommand.Append("ON p.PollGuid = pu.PollGuid ");

            sqlCommand.Append("JOIN mp_PollOptions po ");
            sqlCommand.Append("ON pu.OptionGuid = po.OptionGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pu.UserGuid = @UserGuid ");

            sqlCommand.Append("ORDER BY ActiveFrom DESC, Question ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static int Add(
            Guid pollGuid,
            Guid siteGuid,
            String question,
            bool anonymousVoting,
            bool allowViewingResultsBeforeVoting,
            bool showOrderNumbers,
            bool showResultsWhenDeactivated,
            bool active,
            DateTime activeFrom,
            DateTime activeTo)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Polls ");
            sqlCommand.Append("(");
            sqlCommand.Append("PollGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("Question, ");
            sqlCommand.Append("Active, ");
            sqlCommand.Append("AnonymousVoting, ");
            sqlCommand.Append("AllowViewingResultsBeforeVoting, ");
            sqlCommand.Append("ShowOrderNumbers, ");
            sqlCommand.Append("ShowResultsWhenDeactivated, ");
            sqlCommand.Append("ActiveFrom, ");
            sqlCommand.Append("ActiveTo ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@PollGuid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@Question, ");
            sqlCommand.Append("@Active, ");
            sqlCommand.Append("@AnonymousVoting, ");
            sqlCommand.Append("@AllowViewingResultsBeforeVoting, ");
            sqlCommand.Append("@ShowOrderNumbers, ");
            sqlCommand.Append("@ShowResultsWhenDeactivated, ");
            sqlCommand.Append("@ActiveFrom, ");
            sqlCommand.Append("@ActiveTo ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[10];

            arParams[0] = new SqlCeParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid;

            arParams[1] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid;

            arParams[2] = new SqlCeParameter("@Question", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = question;

            arParams[3] = new SqlCeParameter("@Active", SqlDbType.Bit);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = active;

            arParams[4] = new SqlCeParameter("@AnonymousVoting", SqlDbType.Bit);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = anonymousVoting;

            arParams[5] = new SqlCeParameter("@AllowViewingResultsBeforeVoting", SqlDbType.Bit);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = allowViewingResultsBeforeVoting;

            arParams[6] = new SqlCeParameter("@ShowOrderNumbers", SqlDbType.Bit);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = showOrderNumbers;

            arParams[7] = new SqlCeParameter("@ShowResultsWhenDeactivated", SqlDbType.Bit);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = showResultsWhenDeactivated;

            arParams[8] = new SqlCeParameter("@ActiveFrom", SqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = activeFrom;

            arParams[9] = new SqlCeParameter("@ActiveTo", SqlDbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = activeTo;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Polls ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("Question = @Question, ");
            sqlCommand.Append("Active = @Active, ");
            sqlCommand.Append("AnonymousVoting = @AnonymousVoting, ");
            sqlCommand.Append("AllowViewingResultsBeforeVoting = @AllowViewingResultsBeforeVoting, ");
            sqlCommand.Append("ShowOrderNumbers = @ShowOrderNumbers, ");
            sqlCommand.Append("ShowResultsWhenDeactivated = @ShowResultsWhenDeactivated, ");
            sqlCommand.Append("ActiveFrom = @ActiveFrom, ");
            sqlCommand.Append("ActiveTo = @ActiveTo ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("PollGuid = @PollGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[9];

            arParams[0] = new SqlCeParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid;

            arParams[1] = new SqlCeParameter("@Question", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = question;

            arParams[2] = new SqlCeParameter("@Active", SqlDbType.Bit);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = active;

            arParams[3] = new SqlCeParameter("@AnonymousVoting", SqlDbType.Bit);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = anonymousVoting;

            arParams[4] = new SqlCeParameter("@AllowViewingResultsBeforeVoting", SqlDbType.Bit);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = allowViewingResultsBeforeVoting;

            arParams[5] = new SqlCeParameter("@ShowOrderNumbers", SqlDbType.Bit);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = showOrderNumbers;

            arParams[6] = new SqlCeParameter("@ShowResultsWhenDeactivated", SqlDbType.Bit);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = showResultsWhenDeactivated;

            arParams[7] = new SqlCeParameter("@ActiveFrom", SqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = activeFrom;

            arParams[8] = new SqlCeParameter("@ActiveTo", SqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = activeTo;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetPoll(Guid pollGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  p.*, ");
            sqlCommand.Append(" COALESCE(s2.TotalVotes,0) As TotalVotes ");
            
            sqlCommand.Append("FROM	mp_Polls p ");

            sqlCommand.Append("LEFT OUTER JOIN ( ");
            sqlCommand.Append("SELECT PollGuid, COALESCE(SUM(Votes),0) As TotalVotes ");
            sqlCommand.Append("FROM mp_PollOptions ");
            sqlCommand.Append("GROUP BY PollGuid ");
            sqlCommand.Append(") s2  ");
            sqlCommand.Append("ON s2.PollGuid = p.PollGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.PollGuid = @PollGuid ");
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

        public static IDataReader GetPollByModuleID(int moduleID)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  p.*, ");
            sqlCommand.Append(" COALESCE(s2.TotalVotes,0) As TotalVotes ");
           
            sqlCommand.Append("FROM	mp_Polls p ");

            sqlCommand.Append("JOIN mp_PollModules pm ");
            sqlCommand.Append("ON p.PollGuid = pm.PollGuid ");

            sqlCommand.Append("LEFT OUTER JOIN ( ");
            sqlCommand.Append("SELECT PollGuid, COALESCE(SUM(Votes),0) As TotalVotes ");
            sqlCommand.Append("FROM mp_PollOptions ");
            sqlCommand.Append("GROUP BY PollGuid ");
            sqlCommand.Append(") s2  ");
            sqlCommand.Append("ON s2.PollGuid = p.PollGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pm.ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleID;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static bool ClearVotes(Guid pollGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_PollUsers ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PollGuid = @PollGuid ");
            sqlCommand.Append("; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_PollUsers ");
            sqlCommand.Append("SET Votes = 0 ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PollGuid = @PollGuid ");
            sqlCommand.Append("; ");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
            

            return (rowsAffected > -1);

        }

        public static bool Delete(Guid pollGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("DELETE FROM mp_PollModules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PollGuid = @PollGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_PollUsers ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PollGuid = @PollGuid ");
            sqlCommand.Append(";");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_PollOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PollGuid = @PollGuid ");
            sqlCommand.Append(";");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Polls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PollGuid = @PollGuid ");
            sqlCommand.Append(";");

           arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("DELETE FROM mp_PollModules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID) ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_PollUsers ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PollGuid IN (SELECT PollGuid FROM mp_Polls WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID)) ");
            sqlCommand.Append(";");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_PollOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PollGuid IN (SELECT PollGuid FROM mp_Polls WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID)) ");
            sqlCommand.Append(";");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Polls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PollGuid IN (SELECT PollGuid FROM mp_Polls WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID)) ");
            sqlCommand.Append(";");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            
            return (rowsAffected > -1);

        }

        public static bool UserHasVoted(Guid pollGuid, Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_PollUsers ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append("AND PollGuid = @PollGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid;

            arParams[1] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid;

            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public static bool AddToModule(Guid pollGuid, int moduleID)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("DELETE FROM mp_PollModules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid;

            arParams[1] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleID;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_PollModules ");
            sqlCommand.Append("(");
            sqlCommand.Append("PollGuid, ");
            sqlCommand.Append("ModuleID ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@PollGuid, ");
            sqlCommand.Append("@ModuleID ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@PollGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pollGuid;

            arParams[1] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleID;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
            

            return (rowsAffected > -1);
        }

        public static bool RemoveFromModule(int moduleID)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_PollModules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleID;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

    }
}
