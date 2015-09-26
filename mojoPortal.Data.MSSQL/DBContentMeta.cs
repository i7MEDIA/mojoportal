// Author:					Joe Audette
// Created:					2009-12-02
// Last Modified:			2010-07-01
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

    public static class DBContentMeta
    {
        ///// <summary>
        ///// Gets the connection string for read.
        ///// </summary>
        ///// <returns></returns>
        //private static string GetReadConnectionString()
        //{
        //    return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        //}

        ///// <summary>
        ///// Gets the connection string for write.
        ///// </summary>
        ///// <returns></returns>
        //private static string GetWriteConnectionString()
        //{
        //    if (ConfigurationManager.AppSettings["MSSQLWriteConnectionString"] != null)
        //    {
        //        return ConfigurationManager.AppSettings["MSSQLWriteConnectionString"];
        //    }

        //    return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        //}


        /// <summary>
        /// Inserts a row in the mp_ContentMeta table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="contentGuid"> contentGuid </param>
        /// <param name="name"> name </param>
        /// <param name="scheme"> scheme </param>
        /// <param name="langCode"> langCode </param>
        /// <param name="dir"> dir </param>
        /// <param name="metaContent"> metaContent </param>
        /// <param name="sortRank"> sortRank </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="createdBy"> createdBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid moduleGuid,
            Guid contentGuid,
            string name,
            string scheme,
            string langCode,
            string dir,
            string metaContent,
            int sortRank,
            DateTime createdUtc,
            Guid createdBy)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentMeta_Insert", 14);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@ContentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, contentGuid);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Scheme", SqlDbType.NVarChar, 255, ParameterDirection.Input, scheme);
            sph.DefineSqlParameter("@LangCode", SqlDbType.NVarChar, 10, ParameterDirection.Input, langCode);
            sph.DefineSqlParameter("@Dir", SqlDbType.NVarChar, 3, ParameterDirection.Input, dir);
            sph.DefineSqlParameter("@MetaContent", SqlDbType.NVarChar, -1, ParameterDirection.Input, metaContent);
            sph.DefineSqlParameter("@SortRank", SqlDbType.Int, ParameterDirection.Input, sortRank);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@LastModBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_ContentMeta table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="name"> name </param>
        /// <param name="scheme"> scheme </param>
        /// <param name="langCode"> langCode </param>
        /// <param name="dir"> dir </param>
        /// <param name="metaContent"> metaContent </param>
        /// <param name="sortRank"> sortRank </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            string name,
            string scheme,
            string langCode,
            string dir,
            string metaContent,
            int sortRank,
            DateTime lastModUtc,
            Guid lastModBy)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentMeta_Update", 9);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Scheme", SqlDbType.NVarChar, 255, ParameterDirection.Input, scheme);
            sph.DefineSqlParameter("@LangCode", SqlDbType.NVarChar, 10, ParameterDirection.Input, langCode);
            sph.DefineSqlParameter("@Dir", SqlDbType.NVarChar, 3, ParameterDirection.Input, dir);
            sph.DefineSqlParameter("@MetaContent", SqlDbType.NVarChar, -1, ParameterDirection.Input, metaContent);
            sph.DefineSqlParameter("@SortRank", SqlDbType.Int, ParameterDirection.Input, sortRank);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            sph.DefineSqlParameter("@LastModBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModBy);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_ContentMeta table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentMeta_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_ContentMeta table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentMeta_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_ContentMeta table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentMeta_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_ContentMeta table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByContent(Guid contentGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentMeta_DeleteByContent", 1);
            sph.DefineSqlParameter("@ContentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, contentGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_ContentMeta table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentMeta_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_ContentMeta table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByContent(Guid contentGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentMeta_SelectByContent", 1);
            sph.DefineSqlParameter("@ContentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, contentGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// gets the max sort rank or 1 if null
        /// </summary>
        /// <param name="contentGuid"></param>
        /// <returns>int</returns>
        public static int GetMaxSortRank(Guid contentGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentMeta_GetMaxSortOrder", 1);
            sph.DefineSqlParameter("@ContentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, contentGuid);
            int pageOrder = Convert.ToInt32(sph.ExecuteScalar());
            return pageOrder;
        }

        

        
    }

}
