// Author:					
// Created:					2009-06-02
// Last Modified:			2009-06-02
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

    public static class DBContentStyle
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
        /// Inserts a row in the mp_ContentStyle table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="name"> name </param>
        /// <param name="element"> element </param>
        /// <param name="cssClass"> cssClass </param>
        /// <param name="skinName"> skinName </param>
        /// <param name="isActive"> isActive </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="createdBy"> createdBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            string name,
            string element,
            string cssClass,
            string skinName,
            bool isActive,
            DateTime createdUtc,
            Guid createdBy)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentStyle_Insert", 11);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 100, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Element", SqlDbType.NVarChar, 50, ParameterDirection.Input, element);
            sph.DefineSqlParameter("@CssClass", SqlDbType.NVarChar, 50, ParameterDirection.Input, cssClass);
            sph.DefineSqlParameter("@SkinName", SqlDbType.NVarChar, 100, ParameterDirection.Input, skinName);
            sph.DefineSqlParameter("@IsActive", SqlDbType.Bit, ParameterDirection.Input, isActive);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@LastModBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_ContentStyle table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="name"> name </param>
        /// <param name="element"> element </param>
        /// <param name="cssClass"> cssClass </param>
        /// <param name="skinName"> skinName </param>
        /// <param name="isActive"> isActive </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            Guid siteGuid,
            string name,
            string element,
            string cssClass,
            string skinName,
            bool isActive,
            DateTime lastModUtc,
            Guid lastModBy)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentStyle_Update", 9);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 100, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Element", SqlDbType.NVarChar, 50, ParameterDirection.Input, element);
            sph.DefineSqlParameter("@CssClass", SqlDbType.NVarChar, 50, ParameterDirection.Input, cssClass);
            sph.DefineSqlParameter("@SkinName", SqlDbType.NVarChar, 100, ParameterDirection.Input, skinName);
            sph.DefineSqlParameter("@IsActive", SqlDbType.Bit, ParameterDirection.Input, isActive);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            sph.DefineSqlParameter("@LastModBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModBy);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_ContentStyle table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentStyle_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_ContentStyle table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentStyle_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_ContentStyle table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySkin(Guid siteGuid, string skinName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentStyle_DeleteBySkin", 2);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@SkinName", SqlDbType.NVarChar, 100, ParameterDirection.Input, skinName);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Sets the IsActive field. Returns true if row modified.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool SetActivationBySkin(Guid siteGuid, string skinName, bool isActive)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentStyle_SetActivation", 3);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@SkinName", SqlDbType.NVarChar, 100, ParameterDirection.Input, skinName);
            sph.DefineSqlParameter("@IsActive", SqlDbType.Bit, ParameterDirection.Input, isActive);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_ContentStyle table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentStyle_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();

        }

        

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_ContentStyle table.
        /// </summary>
        public static IDataReader GetAll(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentStyle_SelectAll", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_ContentStyle table.
        /// </summary>
        public static IDataReader GetAll(Guid siteGuid, string skinName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentStyle_SelectAllBySkin", 2);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@SkinName", SqlDbType.NVarChar, 100, ParameterDirection.Input, skinName);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_ContentStyle table.
        /// </summary>
        public static IDataReader GetAllActive(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentStyle_SelectAllActive", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_ContentStyle table.
        /// </summary>
        public static IDataReader GetAllActive(Guid siteGuid, string skinName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentStyle_SelectAllActiveBySkin", 2);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@SkinName", SqlDbType.NVarChar, 100, ParameterDirection.Input, skinName);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets a count of rows in the mp_ContentStyle table.
        /// </summary>
        public static int GetCount(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentStyle_GetCount", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            return Convert.ToInt32(sph.ExecuteScalar());

        }

        /// <summary>
        /// Gets a count of rows in the mp_ContentStyle table.
        /// </summary>
        public static int GetCount(Guid siteGuid, string skinName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentStyle_GetCountBySkin", 2);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@SkinName", SqlDbType.NVarChar, 100, ParameterDirection.Input, skinName);
            return Convert.ToInt32(sph.ExecuteScalar());

        }

        /// <summary>
        /// Gets a page of data from the mp_ContentStyle table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = GetCount(siteGuid);

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentStyle_SelectPage", 3);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets a page of data from the mp_ContentStyle table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid siteGuid,
            string skinName,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = GetCount(siteGuid, skinName);

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentStyle_SelectPageBySkin", 4);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@SkinName", SqlDbType.NVarChar, 100, ParameterDirection.Input, skinName);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

    }

}
