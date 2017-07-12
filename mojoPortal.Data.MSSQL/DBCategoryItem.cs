// Author:					
// Created:					2011-10-30
// Last Modified:			2011-10-30
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

    public static class DBCategoryItem
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
        /// Inserts a row in the mp_CategoryItem table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="featureGuid"> featureGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="itemGuid"> itemGuid </param>
        /// <param name="categoryGuid"> categoryGuid </param>
        /// <param name="extraGuid"> extraGuid </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid featureGuid,
            Guid moduleGuid,
            Guid itemGuid,
            Guid categoryGuid,
            Guid extraGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CategoryItem_Insert", 7);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@CategoryGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, categoryGuid);
            sph.DefineSqlParameter("@ExtraGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, extraGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }


        

        /// <summary>
        /// Deletes a row from the mp_CategoryItem table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CategoryItem_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteByItem(Guid itemGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CategoryItem_DeleteByItem", 1);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteByExtraGuid(Guid extraGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CategoryItem_DeleteByExtraGuid", 1);
            sph.DefineSqlParameter("@ExtraGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, extraGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteByCategory(Guid categoryGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CategoryItem_DeleteByCategory", 1);
            sph.DefineSqlParameter("@CategoryGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, categoryGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CategoryItem_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteByFeature(Guid featureGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CategoryItem_DeleteByFeature", 1);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_CategoryItem_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }


    }
}
