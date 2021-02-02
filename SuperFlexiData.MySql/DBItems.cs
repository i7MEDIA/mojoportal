using mojoPortal.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace SuperFlexiData
{
	public static class DBItems
	{
		/// <summary>
		/// Inserts a row in the i7_sflexi_items table. Returns new integer id.
		/// </summary>
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
			DateTime lastModUtc,
			string viewRoles,
			string editRoles
		)
		{
			string sqlCommand = @"
				INSERT INTO i7_sflexi_items 
					(ItemGuid, 
					SiteGuid, 
					FeatureGuid, 
					ModuleGuid, 
					ModuleID, 
					DefinitionGuid, 
					SortOrder, 
					CreatedUtc,
					LastModUtc, 
					ViewRoles, 
					EditRoles) 
				VALUES (?ItemGuid, 
					?SiteGuid,
					?FeatureGuid, 
					?ModuleGuid, 
					?ModuleID, 
					?DefinitionGuid, 
					?SortOrder, 
					?CreatedUtc, 
					?LastModUtc, 
					?ViewRoles, 
					?EditRoles);
				SELECT LAST_INSERT_ID();";

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
				new MySqlParameter("?LastModUtc", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = lastModUtc },
				new MySqlParameter("?ViewRoles", MySqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = viewRoles },
				new MySqlParameter("?EditRoles", MySqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = editRoles }
			};

			return Convert.ToInt32(
				MySqlHelper.ExecuteScalar(
					ConnectionString.GetWriteConnectionString(),
					sqlCommand,
					sqlParams.ToArray()
				)
			);
		}


		/// <summary>
		/// Updates a row in the i7_sflexi_items table. Returns true if row updated.
		/// </summary>
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
			DateTime lastModUtc,
			string viewRoles,
			string editRoles
		)
		{
			string sqlCommand = @"
				UPDATE i7_sflexi_items 
				SET 
					SiteGuid = ?SiteGuid, 
					FeatureGuid = ?FeatureGuid, 
					ModuleGuid = ?ModuleGuid, 
					ModuleID = ?ModuleID, 
					DefinitionGuid = ?DefinitionGuid, 
					SortOrder = ?SortOrder, 
					CreatedUtc = ?CreatedUtc, 
					LastModUtc = ?LastModUtc,
					ViewRoles = ?ViewRoles,
					EditRoles = ?EditRoles
				WHERE 
					ItemGuid = ?ItemGuid;";

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
				new MySqlParameter("?LastModUtc", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = lastModUtc },
				new MySqlParameter("?ViewRoles", MySqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = viewRoles },
				new MySqlParameter("?EditRoles", MySqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = editRoles }
			};

			int rowsAffected = Convert.ToInt32(
				MySqlHelper.ExecuteNonQuery(
					ConnectionString.GetWriteConnectionString(),
					sqlCommand,
					sqlParams.ToArray()
				)
			);

			return rowsAffected > 0;
		}


		/// <summary>
		/// Deletes a row from the i7_sflexi_items table. Returns true if row deleted.
		/// </summary>
		/// <returns>bool</returns>
		public static bool Delete(int itemID)
		{
			string sqlCommand = "DELETE FROM i7_sflexi_items WHERE ItemID = ?ItemID;";

			var sqlParam = new MySqlParameter("?ItemID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = itemID };

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);

			return rowsAffected > 0;
		}


		/// <summary>
		/// Deletes rows from the i7_sflexi_items table. Returns true if rows deleted.
		/// </summary>
		/// <returns>bool</returns>
		public static bool DeleteBySite(Guid siteGuid)
		{
			string sqlCommand = "DELETE FROM i7_sflexi_items WHERE SiteGuid = ?SiteGuid;";

			var sqlParam = new MySqlParameter("?SiteGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = siteGuid };

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);

			return rowsAffected > 0;
		}


		/// <summary>
		/// Deletes rows from the i7_sflexi_items table. Returns true if rows deleted.
		/// </summary>
		/// <returns>bool</returns>
		public static bool DeleteByModule(Guid moduleGuid)
		{
			string sqlCommand = "DELETE FROM i7_sflexi_items WHERE ModuleGuid = ?ModuleGuid;";

			var sqlParam = new MySqlParameter("?ModuleGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = moduleGuid };

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);

			return rowsAffected > 0;
		}


		/// <summary>
		/// Deletes items and values used by definition guid. Returns true if rows deleted.
		/// </summary>
		/// <returns>bool</returns>
		//public static bool DeleteByDefinition(Guid definitionGuid)
		//{
		//    string sqlCommand = "DELETE FROM i7_sflexi_items WHERE DefinitionGuid = ?DefinitionGuid;";

		//    var sqlParam = new MySqlParameter("?DefinitionGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = definitionGuid };

		//    int rowsAffected = MySqlHelper.ExecuteNonQuery(
		//        ConnectionString.GetWriteConnectionString(),
		//        sqlCommand,
		//        sqlParam);

		//    return (rowsAffected > 0);
		//}


		/// <summary>
		/// Gets an IDataReader with one row from the i7_sflexi_items table.
		/// </summary>
		public static IDataReader GetOne(Guid itemGuid)
		{
			string sqlCommand = "SELECT * FROM i7_sflexi_items WHERE ItemGuid = ?ItemGuid;";

			var sqlParam = new MySqlParameter("?ItemGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = itemGuid };

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);
		}


		/// <summary>
		/// Gets an IDataReader with one row from the i7_sflexi_items table.
		/// </summary>
		public static IDataReader GetOne(int itemId)
		{
			string sqlCommand = "SELECT * FROM i7_sflexi_items WHERE ItemID = ?ItemID;";

			var sqlParam = new MySqlParameter("?ItemID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = itemId };

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand,
				sqlParam
			);
		}


		/// <summary>
		/// Gets a count of rows in the i7_sflexi_items table.
		/// </summary>
		//public static int GetCount()
		//{
		//	string sqlCommand = "SELECT Count(*) FROM i7_sflexi_items;";

		//	return Convert.ToInt32(
		//		MySqlHelper.ExecuteScalar(
		//			ConnectionString.GetReadConnectionString(),
		//			sqlCommand
		//		)
		//	);
		//}


		/// <summary>
		/// Gets a count of all items for a single module.
		/// </summary>
		/// <returns></returns>
		public static int GetCountForModule(int moduleId)
		{
			string sqlCommand = $"SELECT Count(*) FROM i7_sflexi_items where ModuleID = ?ModuleID;";

			var sqlParam = new MySqlParameter("?ModuleID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = moduleId };

			return Convert.ToInt32(
				MySqlHelper.ExecuteScalar(
					ConnectionString.GetReadConnectionString(),
					sqlCommand,
					sqlParam
				)
			);
		}


		/// <summary>
		/// Gets an IDataReader with all rows in the i7_sflexi_items table.
		/// </summary>
		public static IDataReader GetAll(Guid siteGuid)
		{
			string sqlCommand = "SELECT * FROM i7_sflexi_items WHERE SiteGuid = ?SiteGuid;";

			var sqlParam = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = siteGuid };

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand,
				sqlParam
			);
		}


		/// <summary>
		/// Gets an IDataReader with all items for module.
		/// </summary>
		/// <param name="itemID"> itemID </param>
		public static IDataReader GetForModule(int moduleID, string sortDirection = "ASC")
		{
			if (sortDirection != "ASC" && sortDirection != "DESC") sortDirection = "ASC";
			string sqlCommand = $"SELECT * FROM i7_sflexi_items WHERE ModuleID = ?ModuleID ORDER BY SortOrder {sortDirection};";

			var sqlParam = new MySqlParameter("?ModuleID", MySqlDbType.Int32)
			{ Direction = ParameterDirection.Input, Value = moduleID };

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand,
				sqlParam
			);
		}

		public static IDataReader GetForModuleWithValues(int moduleID, string sortDirection)
		{
			if (sortDirection != "ASC" && sortDirection != "DESC") sortDirection = "ASC";
			string sqlCommand = $@"
				SELECT i.*, f.Name AS FieldName, v.FieldValue 
				FROM i7_sflexi_items i
				JOIN i7_sflexi_values v ON v.ItemGuid = i.ItemGuid
				JOIN i7_sflexi_fields f ON f.FieldGuid = v.FieldGuid
				WHERE ModuleID = ?ModuleID 
				ORDER BY i.SortOrder {sortDirection};";
			
			var sqlParam = new MySqlParameter("?ModuleID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = moduleID };

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand,
				sqlParam
			);
		}

		public static IDataReader GetForModuleWithValues_Paged(
			Guid moduleGuid,
			int pageNumber,
			int pageSize,
			string searchTerm = "",
			string searchField = "",
			//string sortField = "",
			string sortDirection = "ASC"
		)
		{
			string sqlCommand;
			if (sortDirection != "ASC" && sortDirection != "DESC") sortDirection = "ASC";
			if (string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(searchTerm))
			{
				sqlCommand = $@"
					SELECT SQL_CALC_FOUND_ROWS FOUND_ROWS() AS TotalRows, i.*, v.FieldValue, f.Name AS `FieldName`, v.FieldGuid
					FROM `i7_sflexi_items` i
					JOIN(
						SELECT DISTINCT ItemGuid, FieldValue, FieldGuid
						FROM `i7_sflexi_values`
						WHERE FieldValue LIKE ?SearchTerm
						) v ON v.ItemGuid = i.ItemGuid
					JOIN `i7_sflexi_fields` f ON f.FieldGuid = v.FieldGuid
					WHERE i.`ModuleGuid` = '?ModuleGuid' 
					ORDER BY i.`SortOrder` {sortDirection}
					LIMIT ?PageSize
					{(pageNumber > 1 ? "OFFSET ?OffsetRows" : string.Empty)};";
			}
			else if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(searchTerm))
			{
				sqlCommand = $@"
					SELECT SQL_CALC_FOUND_ROWS FOUND_ROWS()
					AS TotalRows, i.*, v.FieldValue, f.FieldName, v.FieldGuid
					FROM `i7_sflexi_items` i
					JOIN(
						SELECT DISTINCT `ItemGuid`, `FieldGuid`, `FieldValue`
						FROM `i7_sflexi_values`
						WHERE FieldValue LIKE ?SearchTerm
						) v ON v.ItemGuid = i.ItemGuid
					JOIN(
						SELECT DISTINCT `FieldGuid`, `Name` as `FieldName`
						FROM `i7_sflexi_fields`
						WHERE `Name` = ?SearchField
						) f on f.FieldGuid = v.FieldGuid
					WHERE i.`ModuleGuid` = ?ModuleGuid
					ORDER BY i.`SortOrder` {sortDirection}
					LIMIT ?PageSize
					{(pageNumber > 1 ? "OFFSET ?OffsetRows" : string.Empty)};";
			}
			else
			{
				sqlCommand = $@"
					SELECT SQL_CALC_FOUND_ROWS FOUND_ROWS() AS TotalRows, i.*, v.FieldValue, f.Name AS `FieldName`, v.FieldGuid
					FROM `i7_sflexi_items` i
					JOIN `i7_sflexi_values` v ON v.ItemGuid = i.ItemGuid
					JOIN `i7_sflexi_fields` f ON f.FieldGuid = v.FieldGuid
					WHERE i.`ModuleGuid` = ?ModuleGuid
					ORDER BY i.`SortOrder` {sortDirection}
					LIMIT ?PageSize 
					{(pageNumber > 1 ? "OFFSET ?OffsetRows" : string.Empty)};";
			}

			int offsetRows = (pageSize * pageNumber) - pageSize;

			var sqlParams = new List<MySqlParameter>
			{
				new MySqlParameter("?PageSize", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
				new MySqlParameter("?OffsetRows", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
				new MySqlParameter("?SearchTerm", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = "%" + searchTerm + "%"},
				new MySqlParameter("?SearchField", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = searchField },
				new MySqlParameter("?ModuleGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = moduleGuid }
			};

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand,
				sqlParams.ToArray()
			);
		}


		public static IDataReader GetForDefinitionWithValues_Paged(
			Guid defGuid,
			Guid siteGuid,
			int pageNumber,
			int pageSize,
			string searchTerm = "",
			string searchField = "",
			string sortDirection = "ASC"
		)
		{
			string sqlCommand;
			if (sortDirection != "ASC" && sortDirection != "DESC") sortDirection = "ASC";
			if (string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(searchTerm))
			{
				sqlCommand = $@"
					SELECT SQL_CALC_FOUND_ROWS FOUND_ROWS()
					AS TotalRows, i.*, v.FieldValue, f.Name AS `FieldName`, v.`FieldGuid`
					FROM `i7_sflexi_items` i
					JOIN (
						SELECT DISTINCT `ItemGuid`, `FieldGuid`, `FieldValue`
						FROM `i7_sflexi_values`
						WHERE FieldValue
						LIKE ?SearchTerm
						) v ON v.ItemGuid = i.ItemGuid
					JOIN `i7_sflexi_fields` f ON f.FieldGuid = v.FieldGuid
					WHERE i.`DefinitionGuid` = '?DefinitionGuid'
					AND i.`SiteGuid` = '?SiteGuid'
					ORDER BY i.`SortOrder` {sortDirection}
					LIMIT ?PageSize
					{(pageNumber > 1 ? "OFFSET ?OffsetRows" : string.Empty)};";
			}
			else if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(searchTerm))
			{
				sqlCommand = $@"
					SELECT SQL_CALC_FOUND_ROWS FOUND_ROWS() AS TotalRows, i.*, v.FieldValue, f.`FieldName`, v.`FieldGuid`
					FROM `i7_sflexi_items` i
					JOIN(
						SELECT DISTINCT `ItemGuid`, `FieldGuid`, `FieldValue`
						FROM `i7_sflexi_values`
						WHERE FieldValue LIKE ?SearchTerm
						) v ON v.ItemGuid = i.ItemGuid
					JOIN(
						SELECT DISTINCT `FieldGuid`, `Name` as `FieldName`
						FROM `i7_sflexi_fields`
						WHERE `Name` = ?SearchField
						) f ON f.FieldGuid = v.FieldGuid
					WHERE i.`DefinitionGuid` = ?DefinitionGuid 
					AND i.`SiteGuid` = ?SiteGuid
					ORDER BY i.`SortOrder` {sortDirection}
					LIMIT ?PageSize
					{(pageNumber > 1 ? "OFFSET ?OffsetRows" : string.Empty)};";
			}
			else
			{
				sqlCommand = $@"
					SELECT SQL_CALC_FOUND_ROWS FOUND_ROWS() AS TotalRows, i.*, v.FieldValue, f.Name AS `FieldName`, v.`FieldGuid`
					FROM `i7_sflexi_items` i
					JOIN `i7_sflexi_values` v ON v.ItemGuid = i.ItemGuid
					JOIN `i7_sflexi_fields` f ON f.FieldGuid = v.FieldGuid
					WHERE i.`DefinitionGuid` = ?DefinitionGuid
					AND i.`SiteGuid` = ?SiteGuid
					ORDER BY i.`SortOrder` {sortDirection}
					LIMIT ?PageSize
					{(pageNumber > 1 ? "OFFSET ?OffsetRows" : string.Empty)};";
			}

			var offsetRows = (pageSize * pageNumber) - pageSize;

			var sqlParams = new List<MySqlParameter>
			{
				new MySqlParameter("?PageSize", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
				new MySqlParameter("?OffsetRows", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
				new MySqlParameter("?SearchTerm", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = "%" + searchTerm + "%"},
				new MySqlParameter("?SearchField", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = searchField },
				new MySqlParameter("?DefinitionGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = defGuid },
				new MySqlParameter("?SiteGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = siteGuid },
				//new MySqlParameter("?SortDirection", MySqlDbType.VarChar, 4) { Direction = ParameterDirection.Input, Value = sortDirection }
			};

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand,
				sqlParams.ToArray()
			);
		}


		/// <summary>
		/// Gets an IDataReader with all items for a single definition.
		/// </summary>
		public static IDataReader GetForDefinition(Guid definitionGuid, Guid siteGuid, string sortDirection)
		{
			if (sortDirection != "ASC" && sortDirection != "DESC") sortDirection = "ASC";
			string sqlCommand = $@"
				SELECT 
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
					i.ViewRoles,
					i.EditRoles,
					ms.SettingValue AS GlobalViewSortOrder 
				FROM i7_sflexi_items i
				LEFT JOIN mp_ModuleSettings ms ON ms.ModuleGuid = i.ModuleGuid
				WHERE DefinitionGuid = ?DefGuid AND i.SiteGuid = ?SiteGuid AND ms.SettingName = 'GlobalViewSortOrder' 
				ORDER BY GlobalViewSortOrder {sortDirection}, i.ModuleID {sortDirection}, SortOrder {sortDirection}, CreatedUtc {sortDirection};";

			var sqlParams = new List<MySqlParameter> {
				new MySqlParameter("?DefGuid", MySqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = definitionGuid },
				new MySqlParameter("?SiteGuid", MySqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = siteGuid }
			};

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParams.ToArray()
			);
		}

		/// <summary>
		/// Get all Items with Values for a single definition
		/// </summary>
		/// <param name="definitionGuid"></param>
		/// <param name="siteGuid"></param>
		/// <returns></returns>
		public static IDataReader GetForDefinitionWithValues(Guid definitionGuid, Guid siteGuid, string sortDirection)
		{
			if (sortDirection != "ASC" && sortDirection != "DESC") sortDirection = "ASC";
			string sqlCommand = $@"
				SELECT 
					i.*,
					ms.SettingValue AS GlobalViewSortOrder, 
					f.Name AS FieldName,
					v.FieldValue,
					v.FieldGuid
				FROM i7_sflexi_items i
				LEFT JOIN mp_ModuleSettings ms ON ms.ModuleGuid = i.ModuleGuid
				LEFT JOIN i7_sflexi_values v ON v.ItemGuid = i.ItemGuid
				LEFT JOIN i7_sflexi_fields f ON f.FieldGuid = v.FieldGuid
				WHERE i.DefinitionGuid = ?DefGuid AND i.SiteGuid = ?SiteGuid AND ms.SettingName = 'GlobalViewSortOrder' 
				ORDER BY GlobalViewSortOrder {sortDirection}, i.ModuleID {sortDirection}, i.SortOrder {sortDirection}, i.CreatedUtc {sortDirection};";

			var sqlParams = new List<MySqlParameter> {
				new MySqlParameter("?DefGuid", MySqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = definitionGuid },
				new MySqlParameter("?SiteGuid", MySqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = siteGuid }
			};

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParams.ToArray()
			);
		}

		/// <summary>
		/// Gets
		/// </summary>
		/// <param name="siteGuid"></param>
		/// <param name="pageId"></param>
		/// <returns></returns>
		public static IDataReader GetByCMSPage(Guid siteGuid, int pageId)
		{
			const string sqlCommand = @"
				SELECT 
					i.ModuleID as moduleId, 
					i.ItemGuid as itemGuid, 
					i.ItemID as itemId, 
					i.SortOrder as sortOrder, 
					i.CreatedUtc as createdUtc, 
					m.ModuleTitle as moduleTitle, 
					m.ViewRoles as moduleViewRoles, 
					i.ViewRoles as itemViewRoles, 
					i.EditRoles as itemEditRoles, 
					pm.PublishBeginDate as publishBeginDate, 
					pm.PublishEndDate as publishEndDate 
				FROM i7_sflexi_items i 
				JOIN mp_PageModules pm on i.ModuleGuid = pm.ModuleGuid 
				JOIN mp_Modules m on i.ModuleGuid = m.Guid 
				WHERE i.SiteGuid = ?SiteGuid 
				AND pm.PageID = ?PageID 
				ORDER BY SortOrder ASC;";

			var sqlParams = new List<MySqlParameter>
			{
				new MySqlParameter("?SiteGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = siteGuid },
				new MySqlParameter("?PageID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = pageId }
			};

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParams.ToArray()
			);
		}

		/// <summary>
		/// Gets Highest (largest) SortOrder
		/// </summary>
		/// <param name="moduleId"></param>
		/// <returns></returns>
		public static int GetHighestSortOrder(int moduleId)
		{
			string sqlCommand = $"SELECT Max(SortOrder) FROM i7_sflexi_items where ModuleID = ?ModuleID;";

			var sqlParam = new MySqlParameter("?ModuleID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = moduleId };

			return Convert.ToInt32(
				MySqlHelper.ExecuteScalar(
					ConnectionString.GetReadConnectionString(),
					sqlCommand,
					sqlParam
				)
			);
		}
	}
}
