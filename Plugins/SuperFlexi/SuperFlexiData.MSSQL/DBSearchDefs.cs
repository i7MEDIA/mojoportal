// Author:					i7MEDIA
// Created:					2016-09-12
// Last Modified:			2016-09-13
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using mojoPortal.Data;

namespace SuperFlexiData
{
    public static class DBSearchDefs
    {
        public static bool Create(Guid guid, Guid siteGuid, Guid featureGuid, Guid definitionGuid, string title, string keywords, string description, string link, string linkQueryAddendum)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_searchdefs_Insert", 9);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@FieldDefinitionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, definitionGuid);
            
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, -1, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Keywords", SqlDbType.NVarChar, -1, ParameterDirection.Input, keywords);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@Link", SqlDbType.NVarChar, -1, ParameterDirection.Input, link);
            sph.DefineSqlParameter("@LinkQueryAddendum", SqlDbType.NVarChar, -1, ParameterDirection.Input, linkQueryAddendum);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool Update(Guid guid, Guid siteGuid, Guid featureGuid, Guid definitionGuid, string title, string keywords, string description, string link, string linkQueryAddendum)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_searchdefs_Update", 9);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@FieldDefinitionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, definitionGuid);

            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, -1, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Keywords", SqlDbType.NVarChar, -1, ParameterDirection.Input, keywords);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@Link", SqlDbType.NVarChar, -1, ParameterDirection.Input, link);
            sph.DefineSqlParameter("@LinkQueryAddendum", SqlDbType.NVarChar, -1, ParameterDirection.Input, linkQueryAddendum);


            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteByFieldDefinition(Guid fieldDefGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_searchdefs_DeleteByFieldDefinition", 1);
            sph.DefineSqlParameter("@FieldDefinitionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fieldDefGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_searchdefs_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_searchdefs_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetByFieldDefinition(Guid fieldDefinitionGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_searchdefs_SelectByFieldDefinition", 1);
            sph.DefineSqlParameter("@FieldDefinitionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fieldDefinitionGuid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetOne(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_searchdefs_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();
        }
    }
}
