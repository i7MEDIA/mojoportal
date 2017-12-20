// Created:					2017-07-13
// Last Modified:			2017-07-18

using mojoPortal.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SuperFlexiData
{

    public static class DBItems
    {

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
            sqlCommand.AppendFormat("INSERT INTO {0} ({1}) VALUES ({2});"
                , "i7_sflexi_items"
                , "ItemGuid, "
                + "SiteGuid, "
                + "FeatureGuid, "
                + "ModuleGuid, "
                + "ModuleID, "
                + "DefinitionGuid, "
                + "SortOrder, "
                + "CreatedUtc, "
                + "LastModUtc"
                , "?ItemGuid, "
                + "?SiteGuid, "
                + "?FeatureGuid, "
                + "?ModuleGuid, "
                + "?ModuleID, "
                + "?DefinitionGuid, "
                + "?SortOrder, "
                + "?CreatedUtc, "
                + "?LastModUtc");
            sqlCommand.AppendLine("SELECT LAST_INSERT_ID();");

            var sqlParams = new List<MySqlParameter>
            {
                new MySqlParameter("?ItemGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = itemGuid },
                new MySqlParameter("?SiteGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = siteGuid },
                new MySqlParameter("?FeatureGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = featureGuid },
                new MySqlParameter("?ModuleGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = moduleGuid },
                new MySqlParameter("?ModuleID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = moduleID },
                new MySqlParameter("?DefinitionGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = definitionGuid },
                new MySqlParameter("?SortOrder", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = sortOrder },
                new MySqlParameter("?CreatedUtc", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = createdUtc },
                new MySqlParameter("?LastModUtc", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = lastModUtc }
            };

            

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                sqlParams.ToArray()).ToString());
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
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.AppendFormat("UPDATE {0} SET {1} WHERE {2};"
                , "i7_sflexi_items"
                , "SiteGuid = ?SiteGuid, "
                + "FeatureGuid = ?FeatureGuid, "
                + "ModuleGuid = ?ModuleGuid, "
                + "ModuleID = ?ModuleID, "
                + "DefinitionGuid = ?DefinitionGuid, "
                + "SortOrder = ?SortOrder, "
                + "CreatedUtc = ?CreatedUtc, "
                + "LastModUtc = ?LastModUtc"
                , "ItemGuid = ?ItemGuid");

            var sqlParams = new List<MySqlParameter>
            {
                new MySqlParameter("?ItemGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = itemGuid },
                new MySqlParameter("?SiteGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = siteGuid },
                new MySqlParameter("?FeatureGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = featureGuid },
                new MySqlParameter("?ModuleGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = moduleGuid },
                new MySqlParameter("?ModuleID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = moduleID },
                new MySqlParameter("?DefinitionGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = definitionGuid },
                new MySqlParameter("?SortOrder", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = sortOrder },
                new MySqlParameter("?CreatedUtc", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = createdUtc },
                new MySqlParameter("?LastModUtc", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = lastModUtc }
            };

            int rowsAffected = Convert.ToInt32(MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                sqlParams.ToArray()).ToString());

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM i7_sflexi_items WHERE ItemID = ?ItemID;");

            var sqlParam = new MySqlParameter("?ItemID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = itemID };

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                sqlParam);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the i7_sflexi_items table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM i7_sflexi_items WHERE SiteGuid = ?SiteGuid;");

            var sqlParam = new MySqlParameter("?SiteGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = siteGuid };
            
            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                sqlParam);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes rows from the i7_sflexi_items table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM i7_sflexi_items WHERE ModuleGuid = ?ModuleGuid;");

            var sqlParam = new MySqlParameter("?ModuleGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = moduleGuid };

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                sqlParam);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes items and values used by definition guid. Returns true if rows deleted.
        /// </summary>
        /// <param name="definitionGuid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByDefinition(Guid definitionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM i7_sflexi_items WHERE DefinitionGuid = ?DefinitionGuid;");

            var sqlParam = new MySqlParameter("?DefinitionGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = definitionGuid };

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                sqlParam);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the i7_sflexi_items table.
        /// </summary>
        /// <param name="itemID"> itemID </param>
        public static IDataReader GetOne(Guid itemGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM i7_sflexi_items WHERE ItemGuid = ?ItemGuid;");

            var sqlParam = new MySqlParameter("?ItemGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = itemGuid };

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                sqlParam);
        }

		/// <summary>
		/// Gets an IDataReader with one row from the i7_sflexi_items table.
		/// </summary>
		/// <param name="itemID"> itemID </param>
		public static IDataReader GetOne(int itemId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT * FROM i7_sflexi_items WHERE ItemID = ?ItemID;");

			var sqlParam = new MySqlParameter("?ItemID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = itemId };

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				sqlParam);
		}

		/// <summary>
		/// Gets a count of rows in the i7_sflexi_items table.
		/// </summary>
		public static int GetCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) FROM i7_sflexi_items;");

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                 ConnectionString.GetReadConnectionString(),
                 sqlCommand.ToString()));
        }
		public static int GetCountForModule(int moduleId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append($"SELECT Count(*) FROM i7_sflexi_items where ModuleID = {moduleId};");

			return Convert.ToInt32(MySqlHelper.ExecuteScalar(
				 ConnectionString.GetReadConnectionString(),
				 sqlCommand.ToString()));
		}
		
		/// <summary>
		/// Gets an IDataReader with all rows in the i7_sflexi_items table.
		/// </summary>
		public static IDataReader GetAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM i7_sflexi_items;");

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString());
        }


        /// <summary>
        /// Gets an IDataReader with all items for module.
        /// </summary>
        /// <param name="itemID"> itemID </param>
        public static IDataReader GetModuleItems(int moduleID)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM i7_sflexi_items WHERE ModuleID = ?ModuleID ORDER BY SortOrder ASC;");

            var sqlParam = new MySqlParameter("?ModuleID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = moduleID };

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                sqlParam);
        }

		public static IDataReader GetPageOfModuleItems(
			Guid moduleGuid,
			int pageNumber,
			int pageSize,
			string searchTerm = "",
			string searchField = "",
			//string sortField = "",
			bool descending = false)
		{
			StringBuilder sqlCommand = new StringBuilder();

			if (String.IsNullOrWhiteSpace(searchField) && !String.IsNullOrWhiteSpace(searchTerm))
			{
				sqlCommand.Append(@"SELECT SQL_CALC_FOUND_ROWS FOUND_ROWS() AS TotalRows, i.*
					FROM `i7_sflexi_items` i
						JOIN(
							SELECT DISTINCT ItemGuid
							FROM `i7_sflexi_values`
							WHERE FieldValue LIKE '%?SearchTerm%'
							) v ON v.ItemGuid = i.ItemGuid
						WHERE `ModuleGuid` = '?ModuleGuid' 
						ORDER BY `SortOrder` ?SortDirection
						LIMIT ?PageSize " + (pageNumber > 1 ? "OFFSET ?OffsetRows;" : ";"));
			}
			else if (!String.IsNullOrWhiteSpace(searchField) && !String.IsNullOrWhiteSpace(searchTerm))
			{
				sqlCommand.Append(@"SELECT SQL_CALC_FOUND_ROWS FOUND_ROWS() AS TotalRows, i.*
					FROM `i7_sflexi_items` i
						JOIN(
							SELECT DISTINCT `ItemGuid`, `FieldGuid`
							FROM `i7_sflexi_values`
							WHERE FieldValue LIKE '%?SearchTerm%'
							) v ON v.ItemGuid = i.ItemGuid
						JOIN(
							SELECT DISTINCT `FieldGuid`
							FROM `i7_sflexi_fields`
							WHERE `Name` = ?SearchField
							) f on f.FieldGuid = v.FieldGuid
						WHERE `ModuleGuid` = ?ModuleGuid
						ORDER BY `SortOrder` ?SortDirection
						LIMIT ?PageSize " + (pageNumber > 1 ? "OFFSET ?OffsetRows;" : ";"));
			}
			else
			{
				sqlCommand.Append(@"SELECT SQL_CALC_FOUND_ROWS FOUND_ROWS() AS TotalRows, i.*
					FROM `i7_sflexi_items` i
					WHERE `ModuleGuid` = '?ModuleGuid' 
					ORDER BY `SortOrder` ?SortDirection
					LIMIT ?PageSize " + (pageNumber > 1 ? "OFFSET ?OffsetRows;" : ";"));
			}

			int offsetRows = (pageSize * pageNumber) - pageSize;

			var sqlParams = new List<MySqlParameter>
			{
				new MySqlParameter("?PageSize", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
				new MySqlParameter("?OffsetRows", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
				new MySqlParameter("?SearchTerm", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = searchTerm },
				new MySqlParameter("?SearchField", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = searchField },
				new MySqlParameter("?ModuleGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = moduleGuid },
				new MySqlParameter("?SortDirection", MySqlDbType.VarChar, 4) { Direction = ParameterDirection.Input, Value = descending ? "DESC" : "ASC" }
			};

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				sqlParams.ToArray());
		}

			/// <summary>
			/// Gets an IDataReader with all items for a single definition.
			/// </summary>
			/// <param name="itemID"> itemID </param>
			public static IDataReader GetAllForDefinition(Guid definitionGuid)
        {
            string sqlCommand = @"SELECT 
                SiteGuid, 
                FeatureGuid, 
                i.ModuleGuid, 
                i.ModuleID, 
                DefinitionGuid, 
                ItemGuid, 
                ItemID, 
                i.SortOrder, 
                CreatedUtc, 
                LastModUtc, 
                ms.SettingValue AS GlobalViewSortOrder 
                FROM i7_sflexi_items i
                LEFT JOIN mp_ModuleSettings ms ON ms.ModuleGuid = i.ModuleGuid
                WHERE DefinitionGuid = ?DefGuid AND ms.SettingName = 'GlobalViewSortOrder' 
                ORDER BY GlobalViewSortOrder ASC, i.ModuleID ASC, SortOrder ASC, CreatedUtc ASC;";

            var sqlParam = new MySqlParameter("?DefGuid", MySqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = definitionGuid };

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand,
                sqlParam);
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
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount();

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
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT * FROM i7_sflexi_items LIMIT ?PageSize" + (pageNumber > 1 ? "OFFSET ?OffsetRows;" : ";"));

            var sqlParams = new List<MySqlParameter>
            {
                new MySqlParameter("?PageSize", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
                new MySqlParameter("?OffsetRows", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = pageLowerBound }
            };

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                sqlParams.ToArray());
        }

        /// <summary>
        /// Gets
        /// </summary>
        /// <param name="siteGuid"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public static IDataReader GetByCMSPage(Guid siteGuid, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT "
                + "i.ModuleID as moduleId, "
                + "i.ItemGuid as itemGuid, "
                + "i.ItemID as itemId, "
                + "i.SortOrder as sortOrder, "
                + "i.CreatedUtc as createdUtc, "
                + "m.ModuleTitle as moduleTitle, "
                + "m.ViewRoles as viewRoles, "
                + "pm.PublishBeginDate as publishBeginDate, "
                + "pm.PublishEndDate as publishEndDate "
                + "FROM i7_sflexi_items i "
                + "JOIN mp_PageModules pm on i.ModuleGuid = pm.ModuleGuid "
                + "JOIN mp_Modules m on i.ModuleGuid = m.Guid "
                + "WHERE i.SiteGuid = ?SiteGuid "
                + "AND pm.PageID = ?PageID "
                + "ORDER BY SortOrder ASC;");

            var sqlParams = new List<MySqlParameter>
            {
                new MySqlParameter("?SiteGuid", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = siteGuid },
                new MySqlParameter("?PageID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = pageId }
            };

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                sqlParams.ToArray());
        }
    }

}


