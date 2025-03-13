using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using mojoPortal.Data;
using MySql.Data.MySqlClient;

namespace SuperFlexiData;

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
		string sqlCommand = """
			INSERT INTO i7_sflexi_items (
				ItemGuid
				,SiteGuid
				,FeatureGuid
				,ModuleGuid
				,ModuleID
				,DefinitionGuid
				,SortOrder
				,CreatedUt
				,LastModUtc
				,ViewRoles
				,EditRoles
			) VALUES (
				?ItemGuid
				,?SiteGui
				,?FeatureGuid
				,?ModuleGuid
				,?ModuleID
				,?DefinitionGuid
				,?SortOrder
				,?CreatedUtc
				,?LastModUtc
				,?ViewRoles
				,?EditRoles
			);
			SELECT LAST_INSERT_ID();
			""";

		var sqlParams = new MySqlParameter[]
		{
			new("?ItemGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = itemGuid },
			new("?SiteGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = siteGuid },
			new("?FeatureGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = featureGuid },
			new("?ModuleGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = moduleGuid },
			new("?ModuleID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = moduleID },
			new("?DefinitionGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = definitionGuid },
			new("?SortOrder", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = sortOrder },
			new("?CreatedUtc", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = createdUtc },
			new("?LastModUtc", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = lastModUtc },
			new("?ViewRoles", MySqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = viewRoles },
			new("?EditRoles", MySqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = editRoles }
		};

		return Convert.ToInt32(
			MySqlHelper.ExecuteScalar(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParams
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
		string sqlCommand = """
				UPDATE i7_sflexi_items 
				SET 
					SiteGuid = ?SiteGuid
					,FeatureGuid = ?FeatureGuid
					,ModuleGuid = ?ModuleGuid
					,ModuleID = ?ModuleID
					,DefinitionGuid = ?DefinitionGuid
					,SortOrder = ?SortOrder
					,CreatedUtc = ?CreatedUtc
					,LastModUtc = ?LastModUtc
					,ViewRoles = ?ViewRoles
					,EditRoles = ?EditRoles
				WHERE 
					ItemGuid = ?ItemGuid;
			""";

		var sqlParams = new MySqlParameter[]
		{
			new("?ItemGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = itemGuid },
			new("?SiteGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = siteGuid },
			new("?FeatureGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = featureGuid },
			new("?ModuleGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = moduleGuid },
			new("?ModuleID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = moduleID },
			new("?DefinitionGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = definitionGuid },
			new("?SortOrder", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = sortOrder },
			new("?CreatedUtc", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = createdUtc },
			new("?LastModUtc", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = lastModUtc },
			new("?ViewRoles", MySqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = viewRoles },
			new("?EditRoles", MySqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = editRoles }
		};

		int rowsAffected = Convert.ToInt32(
			MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParams
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
		var sqlCommand = "DELETE FROM i7_sflexi_items WHERE ItemID = ?ItemID;";
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
		var sqlCommand = "DELETE FROM i7_sflexi_items WHERE SiteGuid = ?SiteGuid;";
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
		var sqlCommand = "DELETE FROM i7_sflexi_items WHERE ModuleGuid = ?ModuleGuid;";
		var sqlParam = new MySqlParameter("?ModuleGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = moduleGuid };

		int rowsAffected = MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			sqlParam
		);

		return rowsAffected > 0;
	}

	/// <summary>
	/// Gets an IDataReader with one row from the i7_sflexi_items table.
	/// </summary>
	public static IDataReader GetOne(Guid itemGuid)
	{
		var sqlCommand = "SELECT * FROM i7_sflexi_items WHERE ItemGuid = ?ItemGuid;";
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
		var sqlCommand = "SELECT * FROM i7_sflexi_items WHERE ItemID = ?ItemID;";
		var sqlParam = new MySqlParameter("?ItemID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = itemId };

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			sqlParam
		);
	}

	/// <summary>
	/// Gets an IDataReader with item and values
	/// </summary>
	public static IDataReader GetOneWithValues(int itemId)
	{
		var sqlCommand = $"""
			SELECT 
				i.*
				,f.`Name` AS `FieldName`
				,v.`FieldValue`
			FROM `i7_sflexi_items` i
			JOIN `i7_sflexi_values` v ON v.ItemGuid = i.ItemGuid
			JOIN `i7_sflexi_fields` f ON v.FieldGuid = f.FieldGuid
			WHERE i.ItemID = ?ItemID;
			""";

		var sqlParam = new MySqlParameter("?ItemID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = itemId };

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			sqlParam
		);
	}

	public static IDataReader GetByIDsWithValues(
		Guid defGuid,
		Guid siteGuid,
		List<int> itemIDs,
		int pageNumber = 1,
		int pageSize = 20)
	{
		string sqlCommand = $"""
				SELECT 
					pg.*
					,f.`Name` AS `FieldName`
					,v.`FieldValue`
				FROM (
					SELECT 
						i.*
						,COUNT(*) AS `TotalRows`
					FROM i7_sflexi_items i
					WHERE {getItems()}
						AND i.DefinitionGuid = ?DefinitionGuid
						AND i.SiteGuid = ?SiteGuid
					GROUP BY ItemID
			 		LIMIT ?PageSize
			 		OFFSET ?OffSetRows) pg
				JOIN `i7_sflexi_values` v ON  v.ItemGuid = pg.ItemGuid
				JOIN `i7_sflexi_fields` f ON v.FieldGuid = f.FieldGuid
				WHERE `TotalRows` > 0
			""";

		string getItems()
		{
			return string.Join(" ", itemIDs.Select((x, i) =>
			{
				var item = $"`ItemID` = ?ItemID{i}";

				if (i < itemIDs.Count() - 1)
				{
					item += " OR";
				}

				return item;
			}));
		}

		var sqlParams = new List<MySqlParameter>();

		for (var i = 0; i < itemIDs.Count(); i++)
		{
			sqlParams.Add(new MySqlParameter($"?ItemID{i}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = itemIDs[i] });
		}

		int offsetRows = (pageSize * pageNumber) - pageSize;

		sqlParams.AddRange(
		[
			new("?PageSize", MySqlDbType.Int32){ Direction = ParameterDirection.Input, Value = pageSize },
			new("?OffsetRows", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
			new("?DefinitionGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = defGuid },
			new("?SiteGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = siteGuid }
		]);

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			[.. sqlParams]
		);
	}


	/// <summary>
	/// Gets a count of all items for a single module.
	/// </summary>
	/// <returns></returns>
	public static int GetCountForModule(int moduleId)
	{
		var sqlCommand = $"SELECT Count(*) FROM i7_sflexi_items where ModuleID = ?ModuleID;";
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
		var sqlCommand = "SELECT * FROM i7_sflexi_items WHERE SiteGuid = ?SiteGuid;";
		var sqlParam = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = siteGuid };

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			sqlParam
		);
	}

	/// <summary>
	/// Gets an IDataReader with page of items for module.
	/// </summary>
	/// <param name="itemID"> itemID </param>
	public static IDataReader GetForModule(
		int moduleID,
		int pageNumber = 1,
		int pageSize = 20,
		string sortDirection = "ASC")
	{
		sortDirection = santizeSortDirection(sortDirection);
		//string sqlCommand = $"SELECT * FROM i7_sflexi_items WHERE ModuleID = ?ModuleID ORDER BY SortOrder {sortDirection};";

		var sqlCommand = $"""
			SELECT *
			FROM (
				SELECT 
					*
					,FOUND_ROWS() AS `TotalRows`
				FROM `i7_sflexi_items` i
				WHERE i.`ModuleID` = ?ModuleID ) t
			WHERE `TotalRows` > 0
			ORDER BY t.`SortOrder` {sortDirection}
			{(pageSize > -1 ? "LIMIT ?PageSize" : string.Empty)} 
			{(pageNumber > 1 ? "OFFSET ?OffsetRows" : string.Empty)};
			""";

		int offsetRows = (pageSize * pageNumber) - pageSize;

		var sqlParams = new MySqlParameter[]
		{
			new("?ModuleID", MySqlDbType.Int32){ Direction = ParameterDirection.Input, Value = moduleID },
			new("?PageSize", MySqlDbType.Int32){ Direction = ParameterDirection.Input, Value = pageSize },
			new("?OffsetRows", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
		};

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			sqlParams
		);
	}

	public static IDataReader GetForModuleWithValues(
		Guid moduleGuid,
		int pageNumber = 1,
		int pageSize = 20,
		string searchTerm = "",
		string searchField = "",
		//string sortField = "",
		string sortDirection = "ASC"
	)
	{
		string sqlCommand;

		sortDirection = santizeSortDirection(sortDirection);
		if (pageSize > 0)
		{
			//query with paging
			sqlCommand = $"""
					SELECT 
						pg.*
						,f.`Name` AS `FieldName`
						,v.`FieldValue`
					FROM (
						SELECT 
							i.*
							,COUNT(*) AS `TotalRows`
						FROM i7_sflexi_items i
						WHERE i.ItemGuid IN (
							SELECT DISTINCT ItemGuid
							FROM i7_sflexi_values v
							{(!string.IsNullOrWhiteSpace(searchField) ? "JOIN i7_sflexi_fields f ON f.`FieldGuid` = v.`FieldGuid`" : string.Empty)}
							WHERE v.ItemGuid = i.ItemGuid
							{(!string.IsNullOrWhiteSpace(searchTerm) ? "AND v.`FieldValue` LIKE ?SearchTerm" : string.Empty)}
							{(!string.IsNullOrWhiteSpace(searchField) ? "AND f.`Name` = ?SearchField" : string.Empty)}
						) 
						AND i.ModuleGuid = ?ModuleGuid
						GROUP BY ItemID
						ORDER BY SortOrder {sortDirection}, CreatedUtc {sortDirection}
				 		LIMIT ?PageSize
				 		OFFSET ?OffSetRows) pg
					JOIN `i7_sflexi_values` v ON  v.ItemGuid = pg.ItemGuid
					JOIN `i7_sflexi_fields` f ON v.FieldGuid = f.FieldGuid
					WHERE `TotalRows` > 0;
				""";
		}
		else
		{
			//query without paging
			sqlCommand = $"""
				SELECT 
					i.*
					,f.`Name` AS `FieldName`
					,v.`FieldValue`
				FROM `i7_sflexi_items` i
				JOIN `i7_sflexi_values` v ON v.ItemGuid = i.ItemGuid
				JOIN `i7_sflexi_fields` f ON v.FieldGuid = f.FieldGuid
				WHERE i.ModuleGuid = ?ModuleGuid
				{(!string.IsNullOrWhiteSpace(searchTerm) ? "AND v.`FieldValue` LIKE ?SearchTerm" : string.Empty)}
				{(!string.IsNullOrWhiteSpace(searchField) ? "AND f.`Name` = ?SearchField" : string.Empty)}
				ORDER BY 
					SortOrder {sortDirection}
					,CreatedUtc {sortDirection};
				""";
		}

		int offsetRows = (pageSize * pageNumber) - pageSize;

		var sqlParams = new MySqlParameter[]
		{
			new("?PageSize", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
			new("?OffsetRows", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
			new("?SearchTerm", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = "%" + searchTerm + "%"},
			new("?SearchField", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = searchField },
			new("?ModuleGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = moduleGuid }
		};

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			sqlParams
		);
	}

	/// <summary>
	/// Gets an IDataReader with all items for a single definition.
	/// </summary>
	public static IDataReader GetForDefinition(
		Guid definitionGuid,
		Guid siteGuid,
		int pageNumber = 1,
		int pageSize = 20,
		string sortDirection = "ASC")
	{
		sortDirection = santizeSortDirection(sortDirection);
		string sqlCommand = $"""
			SELECT 
				i.*
				,ms.SettingValue AS GlobalViewSortOrder
				,Count(i.*) AS `TotalRows`
			FROM `i7_sflexi_items` i
			LEFT JOIN mp_ModuleSettings ms ON ms.ModuleGuid = i.ModuleGuid
			WHERE DefinitionGuid = ?DefGuid AND i.SiteGuid = ?SiteGuid AND ms.SettingName = 'GlobalViewSortOrder' 
			ORDER BY 
				GlobalViewSortOrder {sortDirection}
				,i.ModuleID {sortDirection}
				,SortOrder {sortDirection}
				,CreatedUtc {sortDirection})
			LIMIT ?PageSize {(pageNumber > 1 ? "OFFSET ?OffsetRows" : string.Empty)};
			""";

		int offsetRows = (pageSize * pageNumber) - pageSize;

		var sqlParams = new MySqlParameter[] {
			new("?DefGuid", MySqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = definitionGuid },
			new("?SiteGuid", MySqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = siteGuid },
			new("?PageSize", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
			new("?OffsetRows", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
		};

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			sqlParams
		);
	}


	public static IDataReader GetForDefinitionWithValues(
		Guid defGuid,
		Guid siteGuid,
		int pageNumber = 1,
		int pageSize = 25,
		string searchTerm = "",
		string searchField = "",
		string sortDirection = "ASC"
	)
	{
		string sqlCommand;

		sortDirection = santizeSortDirection(sortDirection);
		if (pageSize > -1)
		{
			//query with paging
			sqlCommand = $"""
					SELECT 
						pg.*
						,f.`Name` AS `FieldName`
						,v.`FieldValue`
					FROM (
						SELECT 
							i.*
							,ms.SettingValue AS GlobalViewSortOrder
							,COUNT(i.*) AS `TotalRows`
						FROM i7_sflexi_items i
						LEFT JOIN mp_ModuleSettings ms ON ms.ModuleGuid = i.ModuleGuid
						WHERE i.ItemGuid IN (
							SELECT DISTINCT ItemGuid
							FROM i7_sflexi_values v
							{(!string.IsNullOrWhiteSpace(searchField) ? "JOIN i7_sflexi_fields f ON f.`FieldGuid` = v.`FieldGuid`" : string.Empty)}
							WHERE v.ItemGuid = i.ItemGuid
							{(!string.IsNullOrWhiteSpace(searchTerm) ? "AND v.`FieldValue` LIKE ?SearchTerm" : string.Empty)}
							{(!string.IsNullOrWhiteSpace(searchField) ? "AND f.`Name` = ?SearchField" : string.Empty)}
						) 
						AND i.DefinitionGuid = ?DefinitionGuid
						AND i.SiteGuid = ?SiteGuid
						AND ms.SettingName = 'GlobalViewSortOrder' 
						ORDER BY 
							GlobalViewSortOrder {sortDirection}
							,i.ModuleID {sortDirection}
							,i.SortOrder {sortDirection}
							,i.CreatedUtc {sortDirection}
				 		LIMIT ?PageSize
				 		OFFSET ?OffSetRows) pg
					JOIN `i7_sflexi_values` v ON  v.ItemGuid = pg.ItemGuid
					JOIN `i7_sflexi_fields` f ON v.FieldGuid = f.FieldGuid
					LEFT JOIN mp_ModuleSettings ms ON ms.ModuleGuid = pg.ModuleGuid
					WHERE `TotalRows` > 0;
				""";
		}
		else
		{
			//query without paging
			sqlCommand = $"""
				SELECT 
					i.*
					,f.`Name` AS `FieldName`
					,v.`FieldValue`
					,ms.SettingValue AS GlobalViewSortOrder
				FROM `i7_sflexi_items` i
				JOIN `i7_sflexi_values` v ON v.ItemGuid = i.ItemGuid
				JOIN `i7_sflexi_fields` f ON v.FieldGuid = f.FieldGuid
				LEFT JOIN mp_ModuleSettings ms ON ms.ModuleGuid = i.ModuleGuid
				WHERE DefinitionGuid = ?DefinitionGuid AND i.SiteGuid = ?SiteGuid AND ms.SettingName = 'GlobalViewSortOrder' 
				{(!string.IsNullOrWhiteSpace(searchTerm) ? "AND v.`FieldValue` LIKE ?SearchTerm" : string.Empty)}
				{(!string.IsNullOrWhiteSpace(searchField) ? "AND f.`Name` = ?SearchField" : string.Empty)}
				ORDER BY 
					GlobalViewSortOrder {sortDirection}
					,i.ModuleID {sortDirection}
					,i.SortOrder {sortDirection}
					,i.CreatedUtc {sortDirection};
				""";
		}

		var offsetRows = (pageSize * pageNumber) - pageSize;

		var sqlParams = new MySqlParameter[]
		{
			new("?PageSize", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
			new("?OffsetRows", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
			new("?SearchTerm", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = "%" + searchTerm + "%"},
			new("?SearchField", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = searchField },
			new("?DefinitionGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = defGuid },
			new("?SiteGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = siteGuid }
		};

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			sqlParams
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
		var sqlCommand = """
			SELECT 
				i.ModuleID as moduleId
				,i.ItemGuid as itemGuid
				,i.ItemID as itemId
				,i.SortOrder as sortOrder
				,i.CreatedUtc as createdUtc
				,m.ModuleTitle as moduleTitle
				,m.ViewRoles as moduleViewRoles
				,i.ViewRoles as itemViewRoles
				,i.EditRoles as itemEditRoles
				,pm.PublishBeginDate as publishBeginDate
				,pm.PublishEndDate as publishEndDate 
			FROM i7_sflexi_items i 
			JOIN mp_PageModules pm on i.ModuleGuid = pm.ModuleGuid 
			JOIN mp_Modules m on i.ModuleGuid = m.Guid 
			WHERE i.SiteGuid = ?SiteGuid 
				AND pm.PageID = ?PageID 
			ORDER BY SortOrder ASC;
			""";

		var sqlParams = new MySqlParameter[]
		{
			new("?SiteGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = siteGuid },
			new("?PageID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = pageId }
		};

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			sqlParams
		);
	}

	/// <summary>
	/// Gets Highest (largest) SortOrder
	/// </summary>
	/// <param name="moduleId"></param>
	/// <returns></returns>
	public static int GetHighestSortOrder(int moduleId)
	{
		string sqlCommand = "SELECT Max(SortOrder) FROM i7_sflexi_items where ModuleID = ?ModuleID;";
		var sqlParam = new MySqlParameter("?ModuleID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = moduleId };

		return Convert.ToInt32(
			MySqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				sqlCommand,
				sqlParam
			)
		);
	}

	private static string santizeSortDirection(string sortDirection)
	{
		if (sortDirection != "ASC" && sortDirection != "DESC")
		{
			return "ASC";
		}

		return sortDirection;
	}
}
