/// Author:				Christian Fredh
/// Created:			2007-07-01
/// Last Modified:		2010-01-28
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
    
    public static class DBPollOption
    {
        
        public static IDataReader GetPollOptions(Guid pollGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_PollOptions_Select", 1);
            sph.DefineSqlParameter("@PollGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pollGuid);
            return sph.ExecuteReader();

        }


        public static IDataReader GetPollOption(Guid optionGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_PollOptions_SelectOne", 1);
            sph.DefineSqlParameter("@OptionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, optionGuid);
            return sph.ExecuteReader();

        }

        public static bool IncrementVotes(
            Guid pollGuid,
            Guid optionGuid,
            Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_PollOptions_IncrementVotes", 3);
            
            sph.DefineSqlParameter("@PollGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pollGuid);
            sph.DefineSqlParameter("@OptionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, optionGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }


        public static int Add(
            Guid optionGuid,
            Guid pollGuid,
            string answer,
            int order)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_PollOptions_Insert", 4);
            sph.DefineSqlParameter("@OptionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, optionGuid);
            sph.DefineSqlParameter("@PollGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pollGuid);
            sph.DefineSqlParameter("@Answer", SqlDbType.NVarChar, 255, ParameterDirection.Input, answer);
            sph.DefineSqlParameter("@Order", SqlDbType.Int, ParameterDirection.Input, order);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }


        public static bool Update(
            Guid optionGuid,
            string answer,
            int order)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_PollOptions_Update", 3);
            sph.DefineSqlParameter("@OptionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, optionGuid);
            sph.DefineSqlParameter("@Answer", SqlDbType.NVarChar, 255, ParameterDirection.Input, answer);
            sph.DefineSqlParameter("@Order", SqlDbType.Int, ParameterDirection.Input, order);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool Delete(Guid optionGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_PollOptions_Delete", 1);
            sph.DefineSqlParameter("@OptionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, optionGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }



    }
}
