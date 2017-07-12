// Author:					i7MEDIA
// Created:					2015-3-6
// Last Modified:			2015-5-1
// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using mojoPortal.Data;
using MySql.Data.MySqlClient;

namespace SuperFlexiData
{

    public static class DBItems
    {
        private static string insertFormat = "INSERT INTO {0} ({1}) VALUES ({2});";

        /// <summary>
        /// Inserts a row in the i7_sflexi_items table. Returns new integer id.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="featureGuid"> featureGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="moduleID"> moduleID </param>
        /// <param name="definitionGuid"> definitionGuid </param>
        /// <param name="itemGuid"> itemGuid </param>
        /// <param name="sortOrder"> sortOrder </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <returns>int</returns>
        public static int Create(
            Guid siteGuid,
            Guid featureGuid,
            Guid moduleGuid,
            int moduleID,
            Guid definitionGuid,
            Guid itemGuid,
            int sortOrder,
            DateTime createdUtc,
            DateTime lastModUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.AppendFormat(insertFormat, 
                "i7_sflexi_fields", 
                "SiteGuid,"
                + "FeatureGuid,"
                + "DefinitionGuid,"
                + "FieldGuid,"
                + "DefinitionName,"
                + "Name,"
                + "Label,"
                + "DefaultValue,"
                + "ControlType,"
                + "ControlSrc,"
                + "SortOrder,"
                + "HelpKey,"
                + "Required,"
                + "RequiredMessageFormat,"
                + "Regex,"
                + "RegexMessageFormat,"
                + "Token,"
                + "Searchable,"
                + "EditPageControlWrapperCssClass,"
                + "EditPageLabelCssClass,"
                + "EditPageControlCssClass,"
                + "DatePickerIncludeTimeForDate,"
                + "DatePickerShowMonthList,"
                + "DatePickerShowYearList,"
                + "DatePickerYearRange,"
                + "ImageBrowserEmptyUrl,"
                + "Options,"
                + "CheckBoxReturnBool,"
                + "CheckBoxReturnValueWhenTrue,"
                + "CheckBoxReturnValueWhenFalse,"
                + "DateFormat,"
                + "TextBoxMode,"
                + "Attributes",


            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_items_Insert", 9);
            sph.DefineSqlParameter("@ItemGuid", MyMySqlDbType.Guid, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@SiteGuid", MyMySqlDbType.Guid, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", MyMySqlDbType.Guid, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@ModuleGuid", MyMySqlDbType.Guid, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@ModuleID", MySqlDbType.Int, ParameterDirection.Input, moduleID);
            sph.DefineSqlParameter("@DefinitionGuid", MyMySqlDbType.Guid, ParameterDirection.Input, definitionGuid);            
            sph.DefineSqlParameter("@SortOrder", MySqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@CreatedUtc", MySqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@LastModUtc", MySqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }


        /// <summary>
        /// Updates a row in the i7_sflexi_items table. Returns true if row updated.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="featureGuid"> featureGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="moduleID"> moduleID </param>
        /// <param name="definitionGuid"> definitionGuid </param>
        /// <param name="itemGuid"> itemGuid </param>
        /// <param name="itemID"> itemID </param>
        /// <param name="sortOrder"> sortOrder </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid siteGuid,
            Guid featureGuid,
            Guid moduleGuid,
            int moduleID,
            Guid definitionGuid,
            Guid itemGuid,
            int sortOrder,
            DateTime createdUtc,
            DateTime lastModUtc)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_items_Update", 9);
            sph.DefineSqlParameter("@SiteGuid", MyMySqlDbType.Guid, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", MyMySqlDbType.Guid, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@ModuleGuid", MyMySqlDbType.Guid, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@ModuleID", MySqlDbType.Int, ParameterDirection.Input, moduleID);
            sph.DefineSqlParameter("@DefinitionGuid", MyMySqlDbType.Guid, ParameterDirection.Input, definitionGuid);
            sph.DefineSqlParameter("@ItemGuid", MyMySqlDbType.Guid, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@SortOrder", MySqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@CreatedUtc", MySqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@LastModUtc", MySqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the i7_sflexi_items table. Returns true if row deleted.
        /// </summary>
        /// <param name="itemID"> itemID </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int itemID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_items_Delete", 1);
            sph.DefineSqlParameter("@ItemID", MySqlDbType.Int, ParameterDirection.Input, itemID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the i7_sflexi_items table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_items_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", MyMySqlDbType.Guid, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes rows from the i7_sflexi_items table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_items_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", MyMySqlDbType.Guid, ParameterDirection.Input, moduleGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes items and values used by definition guid. Returns true if rows deleted.
        /// </summary>
        /// <param name="definitionGuid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByDefinition(Guid definitionGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_items_DeleteByDefinition", 1);
            sph.DefineSqlParameter("@DefinitionGuid", MyMySqlDbType.Guid, ParameterDirection.Input, definitionGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the i7_sflexi_items table.
        /// </summary>
        /// <param name="itemID"> itemID </param>
        public static IDataReader GetOne(
            int itemID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_items_SelectOne", 1);
            sph.DefineSqlParameter("@ItemID", MySqlDbType.Int, ParameterDirection.Input, itemID);
            return sph.ExecuteReader();

        }


        /// <summary>
        /// Gets a count of rows in the i7_sflexi_items table.
        /// </summary>
        public static int GetCount()
        {

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "i7_sflexi_items_GetCount",
                null));

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the i7_sflexi_items table.
        /// </summary>
        public static IDataReader GetAll()
        {

            return SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "i7_sflexi_items_SelectAll",
                null);

        }


        /// <summary>
        /// Gets an IDataReader with all items for module.
        /// </summary>
        /// <param name="itemID"> itemID </param>
        public static IDataReader GetModuleItems(int moduleID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_items_SelectAllForModule", 1);
            sph.DefineSqlParameter("@ModuleID", MySqlDbType.Int, ParameterDirection.Input, moduleID);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets a page of data from the i7_sflexi_items table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetCount();

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_items_SelectPage", 2);
            sph.DefineSqlParameter("@PageNumber", MySqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", MySqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets
        /// </summary>
        /// <param name="siteGuid"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public static IDataReader GetByCMSPage(Guid siteGuid, int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_items_SelectByCMSPage", 2);
            sph.DefineSqlParameter("@SiteGuid", MyMySqlDbType.Guid, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@PageID", MySqlDbType.Int, ParameterDirection.Input, pageId);
            return sph.ExecuteReader();
        }
    }

}


