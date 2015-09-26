/// Author:				Christian Fredh
/// Created:			2007-07-01
/// Last Modified:		2008-11-08
/// 
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
using System.Configuration;
using mojoPortal.Data;

namespace PollFeature.Data
{
   
    public static class DBPoll
    {

        public static IDataReader GetPolls(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Polls_Select", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            return sph.ExecuteReader();

        }

        public static IDataReader GetActivePolls(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Polls_SelectActive", 2);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            return sph.ExecuteReader();

        }

        public static IDataReader GetPollsByUserGuid(Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Polls_SelectByUserGuid", 1);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            return sph.ExecuteReader();
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Polls_Insert", 10);
            sph.DefineSqlParameter("@PollGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pollGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@Question", SqlDbType.NVarChar, 255, ParameterDirection.Input, question);
            sph.DefineSqlParameter("@AnonymousVoting", SqlDbType.Bit, ParameterDirection.Input, anonymousVoting);
            sph.DefineSqlParameter("@AllowViewingResultsBeforeVoting", SqlDbType.Bit, ParameterDirection.Input, allowViewingResultsBeforeVoting);
            sph.DefineSqlParameter("@ShowOrderNumbers", SqlDbType.Bit, ParameterDirection.Input, showOrderNumbers);
            sph.DefineSqlParameter("@ShowResultsWhenDeactivated", SqlDbType.Bit, ParameterDirection.Input, showResultsWhenDeactivated);
            sph.DefineSqlParameter("@Active", SqlDbType.Bit, ParameterDirection.Input, active);
            sph.DefineSqlParameter("@ActiveFrom", SqlDbType.DateTime, ParameterDirection.Input, activeFrom);
            sph.DefineSqlParameter("@ActiveTo", SqlDbType.DateTime, ParameterDirection.Input, activeTo);
            int rowsAffected = sph.ExecuteNonQuery();
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Polls_Update", 9);
            sph.DefineSqlParameter("@PollGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pollGuid);
            sph.DefineSqlParameter("@Question", SqlDbType.NVarChar, 255, ParameterDirection.Input, question);
            sph.DefineSqlParameter("@AnonymousVoting", SqlDbType.Bit, ParameterDirection.Input, anonymousVoting);
            sph.DefineSqlParameter("@AllowViewingResultsBeforeVoting", SqlDbType.Bit, ParameterDirection.Input, allowViewingResultsBeforeVoting);
            sph.DefineSqlParameter("@ShowOrderNumbers", SqlDbType.Bit, ParameterDirection.Input, showOrderNumbers);
            sph.DefineSqlParameter("@ShowResultsWhenDeactivated", SqlDbType.Bit, ParameterDirection.Input, showResultsWhenDeactivated);
            sph.DefineSqlParameter("@Active", SqlDbType.Bit, ParameterDirection.Input, active);
            sph.DefineSqlParameter("@ActiveFrom", SqlDbType.DateTime, ParameterDirection.Input, activeFrom);
            sph.DefineSqlParameter("@ActiveTo", SqlDbType.DateTime, ParameterDirection.Input, activeTo);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);


        }

        public static IDataReader GetPoll(Guid pollGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Polls_SelectOne", 1);
            sph.DefineSqlParameter("@PollGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pollGuid);
            return sph.ExecuteReader();

        }

        public static IDataReader GetPollByModuleID(int moduleID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Polls_SelectOneByModuleID", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleID);
            return sph.ExecuteReader();

        }

        public static bool ClearVotes(Guid pollGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Polls_ClearVotes", 1);
            sph.DefineSqlParameter("@PollGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pollGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool Delete(Guid pollGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Polls_Delete", 1);
            sph.DefineSqlParameter("@PollGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pollGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        

        public static bool DeleteBySite(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Polls_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }

        public static bool UserHasVoted(Guid pollGuid, Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Polls_UserHasVoted", 2);
            sph.DefineSqlParameter("@PollGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pollGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            int userHasVoted = Convert.ToInt32(sph.ExecuteScalar());
            return (userHasVoted == 1);

        }

        public static bool AddToModule(Guid pollGuid, int moduleID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Polls_AddToModule", 2);
            sph.DefineSqlParameter("@PollGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pollGuid);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }

        public static bool RemoveFromModule(int moduleID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Polls_RemoveFromModule", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }
    }
}
