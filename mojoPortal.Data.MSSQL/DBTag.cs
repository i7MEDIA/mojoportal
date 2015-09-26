// Author:					Joe Audette
// Created:					2011-10-29
// Last Modified:			2012-05-29
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using mojoPortal.Data;

namespace mojoPortal.Data
{

    public static class DBTag
    {
        /// <summary>
        /// Inserts a row in the mp_Tag table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="featureGuid"> featureGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="tag"> tag </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="createdBy"> createdBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid vocabularyGuid,
            Guid siteGuid,
            Guid featureGuid,
            Guid moduleGuid,
            string tag,
            DateTime createdUtc,
            Guid createdBy)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Tag_Insert", 8);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@VocabularyGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, vocabularyGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@Tag", SqlDbType.NVarChar, 255, ParameterDirection.Input, tag);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_Tag table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>  
        /// <param name="tag"> tag </param>
        /// <param name="modifiedUtc"> modifiedUtc </param>
        /// <param name="modifiedBy"> modifiedBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            Guid vocabularyGuid,
            string tag,
            DateTime modifiedUtc,
            Guid modifiedBy)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Tag_Update", 5);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@VocabularyGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, vocabularyGuid);
            sph.DefineSqlParameter("@Tag", SqlDbType.NVarChar, 255, ParameterDirection.Input, tag);
            sph.DefineSqlParameter("@ModifiedUtc", SqlDbType.DateTime, ParameterDirection.Input, modifiedUtc);
            sph.DefineSqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, modifiedBy);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool UpdateItemCount(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Tag_UpdateItemCount", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the mp_Tag table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Tag_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Tag_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteByFeature(Guid featureGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Tag_DeleteByFeature", 1);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Tag_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_Tag table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOneTag(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Tag_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Tag_SelectByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            return sph.ExecuteReader();
        }


        /// <summary>
        /// Gets a count of rows in the mp_Tag table.
        /// </summary>
        public static int GetCount(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Tag_GetCountByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);

            return Convert.ToInt32(sph.ExecuteScalar());
        }


    }
}
