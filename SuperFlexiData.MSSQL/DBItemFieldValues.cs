// Author:					i7MEDIA
// Created:					2015-03-06
// Last Modified:			2017-11-02
// You must not remove this notice, or any other, from this software.

using mojoPortal.Data;
using System;
using System.Collections.Generic;
using System.Data;

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
            sph.DefineSqlParameter("@ValueGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, valueGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@FieldGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fieldGuid);
            sph.DefineSqlParameter("@FieldValue", SqlDbType.NVarChar, -1, ParameterDirection.Input, fieldValue);
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
            sph.DefineSqlParameter("@ValueGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, valueGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@FieldGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fieldGuid);
            sph.DefineSqlParameter("@FieldValue", SqlDbType.NVarChar, -1, ParameterDirection.Input, fieldValue);
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
            sph.DefineSqlParameter("@ValueGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, valueGuid);
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
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes rows from the i7_sflexi_values table. Returns true if rows deleted.
        /// </summary>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_values_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }
        /// <summary>
        /// Deletes rows from the i7_sflexi_values table. Returns true if rows deleted.
        /// </summary>
        /// <param name="fieldGuid"> fieldGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByField(Guid fieldGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_values_DeleteByField", 1);
            sph.DefineSqlParameter("@FieldGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fieldGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }
        /// <summary>
        /// Deletes rows from the i7_sflexi_values table. Returns true if rows deleted.
        /// </summary>
        /// <param name="itemGuid"> itemGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByItem(Guid itemGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_values_DeleteByItem", 1);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
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
            sph.DefineSqlParameter("@ValueGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, valueGuid);
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
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@FieldGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fieldGuid);
            return sph.ExecuteReader();
        }

		public static IDataReader GetByItemGuids(List<Guid> itemGuids)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_values_SelectByItemGuids", 1);
			sph.DefineSqlParameter("@ItemGuids", SqlDbType.Structured, "guid_list_tbltype", ParameterDirection.Input, new SqlDataRecordList(itemGuids));
			return sph.ExecuteReader();
		}

		public static IDataReader GetByItemGuid(Guid itemGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_values_SelectAllByItemGuid", 1);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            return sph.ExecuteReader();
        }

		public static IDataReader GetByModuleGuid(Guid moduleGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_values_SelectAllByModuleGuid", 1);
			sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
			return sph.ExecuteReader();
		}

		public static IDataReader GetByDefinitionGuid(Guid definitionGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_values_SelectAllByDefinitionGuid", 1);
			sph.DefineSqlParameter("@DefinitionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, definitionGuid);
			return sph.ExecuteReader();
		}

		public static IDataReader GetByFieldGuid(Guid fieldGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_values_SelectByGuid", 1);
            sph.DefineSqlParameter("@FieldGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fieldGuid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByFieldGuidForModule(Guid fieldGuid, Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_values_SelectByGuidForModule", 2);
            sph.DefineSqlParameter("@FieldGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fieldGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            return sph.ExecuteReader();
        }

		public static IDataReader GetByGuidForModule(Guid fieldGuid, Guid moduleGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_values_SelectByGuidForModule", 2);
			sph.DefineSqlParameter("@FieldGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fieldGuid);
			sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
			return sph.ExecuteReader();
		}

		public static IDataReader GetByGuidForModule(Guid fieldGuid, int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_values_SelectByGuidForModuleById", 2);
            sph.DefineSqlParameter("@FieldGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fieldGuid);
            sph.DefineSqlParameter("@ModuleId", SqlDbType.Int, ParameterDirection.Input, moduleId);
            return sph.ExecuteReader();
        }


		public static IDataReader GetPageOfValuesForField(
			Guid moduleGuid,
			Guid definitionGuid,
			string field,
			int pageNumber,
			int pageSize,
			string searchTerm = "",
			//string searchField = "",
			bool descending = false)
		{
			SqlParameterHelper sph = null;

			if (!String.IsNullOrWhiteSpace(searchTerm))
			{
				sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_values_SelectPageForFieldWithTerm", 7);
				sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
				sph.DefineSqlParameter("@DefinitionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, definitionGuid);
				sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
				sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
				sph.DefineSqlParameter("@SearchTerm", SqlDbType.NVarChar, -1, ParameterDirection.Input, searchTerm);
				sph.DefineSqlParameter("@Field", SqlDbType.NVarChar, -1, ParameterDirection.Input, field);
				sph.DefineSqlParameter("@SortDirection", SqlDbType.VarChar, 4, ParameterDirection.Input, descending ? "desc" : "asc");
			}
			//else if (!String.IsNullOrWhiteSpace(searchField) && !String.IsNullOrWhiteSpace(searchTerm))
			//{
			//	sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_values_SelectPageForFieldWithTermAndField", 7);
			//	sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
			//	sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
			//	sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
			//	sph.DefineSqlParameter("@SearchTerm", SqlDbType.NVarChar, -1, ParameterDirection.Input, searchTerm);
			//	sph.DefineSqlParameter("@SearchField", SqlDbType.NVarChar, -1, ParameterDirection.Input, searchField);
			//	sph.DefineSqlParameter("@Field", SqlDbType.NVarChar, -1, ParameterDirection.Input, field);
			//	sph.DefineSqlParameter("@SortDirection", SqlDbType.VarChar, 4, ParameterDirection.Input, descending ? "desc" : "asc");
			//}

			else
			{
				sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_values_SelectPageForField", 6);
				sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
				sph.DefineSqlParameter("@DefinitionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, definitionGuid);
				sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
				sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
				sph.DefineSqlParameter("@Field", SqlDbType.NVarChar, -1, ParameterDirection.Input, field);
				sph.DefineSqlParameter("@SortDirection", SqlDbType.VarChar, 4, ParameterDirection.Input, descending ? "desc" : "asc");
			}
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
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }


    }

}


