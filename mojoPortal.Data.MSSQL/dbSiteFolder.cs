/// Author:					
/// Created:				2007-11-03
/// Last Modified:			2009-03-11
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
   
    public static class DBSiteFolder
    {
        

        public static int Add(
            Guid guid,
            Guid siteGuid,
            string folderName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SiteFolders_Insert", 3);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FolderName", SqlDbType.NVarChar, 255, ParameterDirection.Input, folderName);
            int rowsAffected = Convert.ToInt32(sph.ExecuteNonQuery());
            return rowsAffected;
        }

        public static bool Update(
            Guid guid,
            Guid siteGuid,
            string folderName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SiteFolders_Update", 3);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FolderName", SqlDbType.NVarChar, 255, ParameterDirection.Input, folderName);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SiteFolders_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool Exists(string folderName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SiteFolder_Exists", 1);
            sph.DefineSqlParameter("@FolderName", SqlDbType.NVarChar, 255, ParameterDirection.Input, folderName);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return (count > 0);
        }

        public static IDataReader GetOne(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SiteFolders_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SiteFolders_SelectBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            return sph.ExecuteReader();
        }

        public static Guid GetSiteGuid(string folderName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SiteFolders_SelectSiteGuidByFolder", 1);
            sph.DefineSqlParameter("@FolderName", SqlDbType.NVarChar, 255, ParameterDirection.Input, folderName);
            string strGuid = sph.ExecuteScalar().ToString();
            if (strGuid.Length == 36)
            {
                return new Guid(strGuid);
            }
            return Guid.Empty;
        }

        


    }
}
