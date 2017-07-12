/// Author:					
/// Created:				2007-11-03
/// Last Modified:			2011-01-19
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace mojoPortal.Data
{
    
    public static class DBUserPage
    {
        
        public static int AddUserPage(
            Guid userPageId,
            Guid siteGuid,
            int siteId,
            Guid userGuid,
            string pageName,
            String pagePath,
            int pageOrder)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserPages_Insert", 7);
            sph.DefineSqlParameter("@UserPageID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userPageId);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@PageName", SqlDbType.NVarChar, ParameterDirection.Input, pageName);
            sph.DefineSqlParameter("@PagePath", SqlDbType.NVarChar, ParameterDirection.Input, pagePath);
            sph.DefineSqlParameter("@PageOrder", SqlDbType.Int, ParameterDirection.Input, pageOrder);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;
        }

        public static bool UpdateUserPage(
            Guid userPageId,
            string pageName,
            int pageOrder)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserPages_Update", 3);
            sph.DefineSqlParameter("@UserPageID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userPageId);
            sph.DefineSqlParameter("@PageName", SqlDbType.NVarChar, ParameterDirection.Input, pageName);
            sph.DefineSqlParameter("@PageOrder", SqlDbType.Int, ParameterDirection.Input, pageOrder);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }


        public static bool DeleteUserPage(Guid userPageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserPages_Delete", 1);
            sph.DefineSqlParameter("@UserPageID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userPageId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteByUser(Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserPages_DeleteByUser", 1);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader GetUserPage(Guid userPageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_UserPages_SelectOne", 1);
            sph.DefineSqlParameter("@UserPageID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userPageId);
            return sph.ExecuteReader();
        }

        public static IDataReader SelectByUser(Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_UserPages_SelectByUser", 1);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            return sph.ExecuteReader();
        }

        public static int GetNextPageOrder(Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_UserPages_GetNextPageOrder", 1);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            int pageOrder = Convert.ToInt32(sph.ExecuteScalar());
            return pageOrder;
        }

        public static bool UpdatePageOrder(Guid userPageId, int pageOrder)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_UserPages_UpdatePageOrder", 2);
            sph.DefineSqlParameter("@UserPageID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userPageId);
            sph.DefineSqlParameter("@PageOrder", SqlDbType.Int, ParameterDirection.Input, pageOrder);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        



    }
}
