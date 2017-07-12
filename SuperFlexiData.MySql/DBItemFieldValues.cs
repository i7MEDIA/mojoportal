// Author:					i7MEDIA
// Created:					2015-3-6
// Last Modified:			2015-3-25
// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using mojoPortal.Data;

namespace SuperFlexiData
{

    public static class DBItemFieldValues
    {


        /// <summary>
        /// Inserts a row in the i7_sflexi_values table. Returns rows affected count.
        /// </summary>
        /// <param name="valueGuid"> valueGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="featureGuid"> featureGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="itemGuid"> itemGuid </param>
        /// <param name="fieldGuid"> fieldGuid </param>
        /// <param name="fieldValue"> fieldValue </param>
        /// <returns>int</returns>
        public static int Create(
            Guid valueGuid,
            Guid siteGuid,
            Guid featureGuid,
            Guid moduleGuid,
            Guid itemGuid,
            Guid fieldGuid,
            string fieldValue)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_values_Insert", 7);
            sph.DefineSqlParameter("@ValueGuid", MyMySqlDbType.Guid, ParameterDirection.Input, valueGuid);
            sph.DefineSqlParameter("@SiteGuid", MyMySqlDbType.Guid, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", MyMySqlDbType.Guid, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@ModuleGuid", MyMySqlDbType.Guid, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@ItemGuid", MyMySqlDbType.Guid, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@FieldGuid", MyMySqlDbType.Guid, ParameterDirection.Input, fieldGuid);
            sph.DefineSqlParameter("@FieldValue", MySqlDbType.Text, ParameterDirection.Input, fieldValue);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the i7_sflexi_values table. Returns true if row updated.
        /// </summary>
        /// <param name="valueGuid"> valueGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="featureGuid"> featureGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="itemGuid"> itemGuid </param>
        /// <param name="fieldGuid"> fieldGuid </param>
        /// <param name="fieldValue"> fieldValue </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid valueGuid,
            Guid siteGuid,
            Guid featureGuid,
            Guid moduleGuid,
            Guid itemGuid,
            Guid fieldGuid,
            string fieldValue)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_values_Update", 7);
            sph.DefineSqlParameter("@ValueGuid", MyMySqlDbType.Guid, ParameterDirection.Input, valueGuid);
            sph.DefineSqlParameter("@SiteGuid", MyMySqlDbType.Guid, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", MyMySqlDbType.Guid, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@ModuleGuid", MyMySqlDbType.Guid, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@ItemGuid", MyMySqlDbType.Guid, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@FieldGuid", MyMySqlDbType.Guid, ParameterDirection.Input, fieldGuid);
            sph.DefineSqlParameter("@FieldValue", MySqlDbType.Text, ParameterDirection.Input, fieldValue);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the i7_sflexi_values table. Returns true if row deleted.
        /// </summary>
        /// <param name="valueGuid"> valueGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(
            Guid valueGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_values_Delete", 1);
            sph.DefineSqlParameter("@ValueGuid", MyMySqlDbType.Guid, ParameterDirection.Input, valueGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the i7_sflexi_values table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_values_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", MyMySqlDbType.Guid, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes rows from the i7_sflexi_values table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_values_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", MyMySqlDbType.Guid, ParameterDirection.Input, moduleGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteByField(Guid fieldGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_values_DeleteByField", 1);
            sph.DefineSqlParameter("@FieldGuid", MyMySqlDbType.Guid, ParameterDirection.Input, fieldGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the i7_sflexi_values table.
        /// </summary>
        /// <param name="valueGuid"> valueGuid </param>
        public static IDataReader GetOne(Guid valueGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_values_SelectOne", 1);
            sph.DefineSqlParameter("@ValueGuid", MyMySqlDbType.Guid, ParameterDirection.Input, valueGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets a count of rows in the i7_sflexi_values table.
        /// </summary>
        public static int GetCount()
        {

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "i7_sflexi_values_GetCount",
                null));

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the i7_sflexi_values table.
        /// </summary>
        public static IDataReader GetAll()
        {

            return SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "i7_sflexi_values_SelectAll",
                null);

        }


        /// <summary>
        /// Gets value by ItemGuid and FieldGuid
        /// </summary>
        /// <param name="itemGuid"></param>
        /// <param name="fieldGuid"></param>
        /// <returns></returns>
        public static IDataReader GetByItemField(Guid itemGuid, Guid fieldGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_values_SelectOneByItemField", 2);
            sph.DefineSqlParameter("@ItemGuid", MyMySqlDbType.Guid, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@FieldGuid", MyMySqlDbType.Guid, ParameterDirection.Input, fieldGuid);
            return sph.ExecuteReader();
        }


        public static IDataReader GetByItemGuid(Guid itemGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_values_SelectAllByItemGuid", 1);
            sph.DefineSqlParameter("@ItemGuid", MyMySqlDbType.Guid, ParameterDirection.Input, itemGuid);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets a page of data from the i7_sflexi_values table.
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_values_SelectPage", 2);
            sph.DefineSqlParameter("@PageNumber", MySqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", MySqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }


    }

}


