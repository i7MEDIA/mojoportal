using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using mojoPortal.Data;
using Mono.Data.Sqlite;

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
		string editRoles)
	{
		var sqlCommand = """
			insert into i7_sflexi_items 
				(ItemGuid
					,SiteGuid 
					,FeatureGuid
					,ModuleGuid 
					,ModuleId
					,DefinitionGuid
					,SortOrder
					,CreatedUTC 
					,LastModUTC
					,ViewRoles
					,EditRoles)
			values (:ItemGuid
					,:SiteGuid
					,:FeatureGuid
					,:ModuleGuid
					,:ModuleId
					,:DefinitionGuid
					,:SortOrder
					,:CreatedUTC
					,:LastModUTC
					,:ViewRoles
					,:EditRoles);
			select last_insert_rowid();
			""";

		var sqlParams = new SqliteParameter[]
		{
			new(":ItemGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = itemGuid.ToString() },
			new(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
			new(":FeatureGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = featureGuid.ToString() },
			new(":ModuleGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = moduleGuid.ToString() },
			new(":ModuleId", DbType.Int32) { Direction = ParameterDirection.Input, Value = moduleID },
			new(":DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() },
			new(":SortOrder", DbType.Int32) { Direction = ParameterDirection.Input, Value = sortOrder },
			new(":CreatedUTC", DbType.DateTime) { Direction = ParameterDirection.Input, Value = createdUtc },
			new(":LastModUTC", DbType.DateTime) { Direction = ParameterDirection.Input, Value = lastModUtc },
			new(":ViewRoles", DbType.String) { Direction = ParameterDirection.Input, Value = viewRoles },
			new(":EditRoles", DbType.String) { Direction = ParameterDirection.Input, Value = editRoles }
		};

		return Convert.ToInt32(SqliteHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			sqlParams).ToString());
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
		string editRoles)
	{
		var sqlCommand = """
			update i7_sflexi_items 
			set 
				SiteGuid = :SiteGuid
				,FeatureGuid = :FeatureGuid
				,ModuleGuid = :ModuleGuid
				,ModuleId = :ModuleId
				,DefinitionGuid = :DefinitionGuid
				,SortOrder = :SortOrder
				,CreatedUTC = :CreatedUTC
				,LastModUTC = :LastModUTC
			where ItemGuid = :ItemGuid;
			""";

		var sqlParams = new SqliteParameter[]
		{
			new(":ItemGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = itemGuid.ToString() },
			new(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
			new(":FeatureGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = featureGuid.ToString() },
			new(":ModuleGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = moduleGuid.ToString() },
			new(":ModuleId", DbType.Int32) { Direction = ParameterDirection.Input, Value = moduleID },
			new(":DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() },
			new(":SortOrder", DbType.Int32) { Direction = ParameterDirection.Input, Value = sortOrder },
			new(":CreatedUTC", DbType.DateTime) { Direction = ParameterDirection.Input, Value = createdUtc },
			new(":LastModUTC", DbType.DateTime) { Direction = ParameterDirection.Input, Value = lastModUtc },
			new(":ViewRoles", DbType.String) { Direction = ParameterDirection.Input, Value = viewRoles },
			new(":EditRoles", DbType.String) { Direction = ParameterDirection.Input, Value = editRoles }
		};

		int rowsAffected = Convert.ToInt32(
			SqliteHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParams).ToString());

		return rowsAffected > 0;
	}

	/// <summary>
	/// Deletes a row from the i7_sflexi_items table. Returns true if row deleted.
	/// </summary>
	/// <param name="itemID"> itemID </param>
	/// <returns>bool</returns>
	public static bool Delete(int itemID)
	{
		var sqlCommand = "delete from i7_sflexi_items where ItemId = :ItemId;";
		var sqlParam = new SqliteParameter(":ItemId", DbType.Int32) { Direction = ParameterDirection.Input, Value = itemID };

		int rowsAffected = SqliteHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			sqlParam);

		return rowsAffected > 0;
	}

	/// <summary>
	/// Deletes rows from the i7_sflexi_items table. Returns true if rows deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool DeleteBySite(Guid siteGuid)
	{
		var sqlCommand = "delete from i7_sflexi_items where SiteGuid = :SiteGuid;";
		var sqlParam = new SqliteParameter(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() };

		int rowsAffected = SqliteHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			sqlParam);

		return rowsAffected > 0;
	}

	/// <summary>
	/// Deletes rows from the i7_sflexi_items table. Returns true if rows deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool DeleteByModule(Guid moduleGuid)
	{
		var sqlCommand = "delete from i7_sflexi_items where ModuleGuid = :ModuleGuid;";
		var sqlParam = new SqliteParameter(":ModuleGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = moduleGuid.ToString() };

		int rowsAffected = SqliteHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			sqlParam);

		return rowsAffected > 0;
	}

	/// <summary>
	/// Gets an IDataReader with one row from the i7_sflexi_items table.
	/// </summary>
	/// <param name="itemID"> itemID </param>
	public static IDataReader GetOne(Guid itemGuid)
	{
		var sqlCommand = "select * from i7_sflexi_items where ItemGuid = :ItemGuid;";
		var sqlParam = new SqliteParameter(":ItemGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = itemGuid.ToString() };

		return SqliteHelper.ExecuteReader(ConnectionString.GetWriteConnectionString(), sqlCommand.ToString(), sqlParam);
	}

	/// <summary>
	/// Gets an IDataReader with one row from the i7_sflexi_items table.
	/// </summary>
	/// <param name="itemID"> itemID </param>
	public static IDataReader GetOne(int itemId)
	{
		var sqlCommand = "select * from i7_sflexi_items where ItemId = :ItemId;";
		var sqlParam = new SqliteParameter(":ItemId", DbType.Int32) { Direction = ParameterDirection.Input, Value = itemId };

		return SqliteHelper.ExecuteReader(ConnectionString.GetWriteConnectionString(), sqlCommand.ToString(), sqlParam);
	}

	public static IDataReader GetOneWithValues(int itemId)
	{
		string sqlCommand = $"""
			SELECT
				i.*
				,f.Name AS FieldName
				,v.FieldValue
			FROM i7_sflexi_items i
			JOIN i7_sflexi_values v 
				ON v.ItemGuid = i.ItemGuid
			JOIN i7_sflexi_fields f 
				ON v.FieldGuid = f.FieldGuid
			WHERE i.ItemID = :ItemID;
			""";

		var sqlParam = new SqliteParameter(":ItemID", DbType.Int32) { Direction = ParameterDirection.Input, Value = itemId };

		return SqliteHelper.ExecuteReader(ConnectionString.GetReadConnectionString(), sqlCommand, sqlParam);
	}

	public static IDataReader GetByIDsWithValues(Guid defGuid, Guid siteGuid, List<int> itemIDs, int pageNumber = 1, int pageSize = 20)
	{
		var sqlCommand = $"""
			SELECT pg.*, f.Name AS FieldName, v.FieldValue
			FROM (
				SELECT 
					i.*
					,count(*) over() AS TotalRows
				FROM i7_sflexi_items i
				WHERE {getItems()}
					AND i.DefinitionGuid = ?DefinitionGuid
					AND i.SiteGuid = ?SiteGuid
				GROUP BY ItemID
				LIMIT :PageSize
				OFFSET :OffSetRows) pg
			JOIN i7_sflexi_values v 
				ON  v.ItemGuid = pg.ItemGuid
			JOIN i7_sflexi_fields f 
				ON v.FieldGuid = f.FieldGuid
			WHERE TotalRows > 0
			""";

		string getItems()
		{
			return string.Join(" ", itemIDs.Select((x, i) =>
			{
				var item = $"ItemID = :ItemID{i}";

				if (i < itemIDs.Count() - 1)
				{
					item += " OR";
				}

				return item;
			}));
		}

		var sqlParams = new List<SqliteParameter>();

		for (var i = 0; i < itemIDs.Count(); i++)
		{
			sqlParams.Add(new SqliteParameter($"?ItemID{i}", DbType.Int32) { Direction = ParameterDirection.Input, Value = itemIDs[i] });
		}

		int offsetRows = (pageSize * pageNumber) - pageSize;

		sqlParams.AddRange(
		[
			new("?PageSize", DbType.Int32){ Direction = ParameterDirection.Input, Value = pageSize },
			new("?OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
			new("?DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = defGuid },
			new("?SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid }
		]);

		return SqliteHelper.ExecuteReader(ConnectionString.GetReadConnectionString(), sqlCommand, [.. sqlParams]);
	}
	/// <summary>
	/// Gets a count of rows in the i7_sflexi_items table.
	/// </summary>
	public static int GetCount() => Convert.ToInt32(SqliteHelper.ExecuteScalar(
		ConnectionString.GetReadConnectionString(),
		"select count(*) from i7_sflexi_items;"));

	public static int GetCountForModule(int moduleId) => Convert.ToInt32(SqliteHelper.ExecuteScalar(
		ConnectionString.GetReadConnectionString(),
		$"select count(*) from i7_sflexi_items where ModuleId = {moduleId};"));

	/// <summary>
	/// Gets an IDataReader with all rows in the i7_sflexi_items table.
	/// </summary>
	public static IDataReader GetAll(Guid siteGuid)
	{
		var sqlCommand = "select * from i7_sflexi_items where SiteGuid = :SiteGuid;";
		var sqlParam = new SqliteParameter(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid };

		return SqliteHelper.ExecuteReader(ConnectionString.GetWriteConnectionString(), sqlCommand, sqlParam);
	}

	/// <summary>
	/// Gets an IDataReader with all items for module.
	/// </summary>
	/// <param name="itemID"> itemID </param>
	public static IDataReader GetModuleItems(int moduleID)
	{
		var sqlCommand = "select * from i7_sflexi_items where ModuleId = :ModuleId order by SortOrder asc;";
		var sqlParam = new SqliteParameter(":ModuleId", DbType.Int32) { Direction = ParameterDirection.Input, Value = moduleID };

		return SqliteHelper.ExecuteReader(ConnectionString.GetReadConnectionString(), sqlCommand, sqlParam);
	}

	public static IDataReader GetPageOfModuleItems(
		Guid moduleGuid,
		int pageNumber,
		int pageSize,
		string searchTerm = "",
		string searchField = "",
		bool descending = false)
	{
		string sqlCommand;

		if (string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(searchTerm))
		{
			sqlCommand = $"""
				select 
					row_number() over (order by SortOrder) as rowid
					,count(*) over() as totalrows
					,i.*
				from i7_sflexi_items i
				join(
					select distinct ItemGuid
					from i7_sflexi_values
					where FieldValue like '%:SearchTerm%'
					) v on v.ItemGuid = i.ItemGuid
				where ModuleGuid = ':ModuleGuid' 
				order by SortOrder :SortDirection
				limit :PageSize 
				{(pageNumber > 1 ? "offset :OffsetRows;" : ";")}
				""";
		}
		else if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(searchTerm))
		{
			sqlCommand = $"""
				select 
					row_number() over (order by SortOrder) as rowid
					, count(*) over() as totalrows
					, i.*
				from i7_sflexi_items i
				join (
					select distinct ItemGuid, FieldGuid
					from i7_sflexi_values
					where FieldValue like '%:SearchTerm%'
					) v on v.ItemGuid = i.ItemGuid
				join (
					select distinct FieldGuid
					from i7_sflexi_fields
					where name = :SearchField
					) f on f.FieldGuid = v.FieldGuid
				where ModuleGuid = :ModuleGuid
				order by SortOrder :SortDirection
				limit :PageSize {(pageNumber > 1 ? "offset :OffsetRows;" : ";")}
				""";
		}
		else
		{
			sqlCommand = $"""
				select 
					row_number() over (order by SortOrder) as rowid
					, count(*) over() as totalrows
					, i.*
				from i7_sflexi_items i
				where ModuleGuid = ':ModuleGuid' 
				order by SortOrder :SortDirection
				limit :PageSize {(pageNumber > 1 ? "offset :OffsetRows;" : ";")}
				""";
		}

		int offsetRows = (pageSize * pageNumber) - pageSize;

		var sqlParams = new SqliteParameter[]
		{
			new(":PageSize", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
			new(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
			new(":SearchTerm", DbType.String, 255) { Direction = ParameterDirection.Input, Value = searchTerm },
			new(":SearchField", DbType.String, 50) { Direction = ParameterDirection.Input, Value = searchField },
			new(":ModuleGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = moduleGuid.ToString() },
			new(":SortDirection", DbType.String, 4) { Direction = ParameterDirection.Input, Value = descending ? "desc" : "asc" }
		};

		return SqliteHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			sqlParams);
	}

	public static IDataReader GetPageForDefinition(
		Guid defGuid,
		Guid siteGuid,
		int pageNumber,
		int pageSize,
		string searchTerm = "",
		string searchField = "",
		bool descending = false)
	{
		string sqlCommand;

		if (string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(searchTerm))
		{
			sqlCommand = $"""
				select 
					row_number() over (order by SortOrder) as rowid
					, count(*) over() as totalrows
					, i.*
				from i7_sflexi_items i
					join(
						select distinct ItemGuid
						from i7_sflexi_values
						where FieldValue like '%:SearchTerm%'
						) v on v.ItemGuid = i.ItemGuid
					where DefinitionGuid = ':DefinitionGuid' 
						and SiteGuid = ':SiteGuid'
					order by SortOrder :SortDirection
					limit :PageSize {(pageNumber > 1 ? "offset :OffsetRows;" : ";")}
				""";
		}
		else if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(searchTerm))
		{
			sqlCommand = $"""
				select 
					row_number() over (order by SortOrder) as rowid
					, count(*) over() as totalrows
					, i.*
				from i7_sflexi_items i
					join(
						select distinct ItemGuid, FieldGuid
						from i7_sflexi_values
						where FieldValue like '%:SearchTerm%'
						) v on v.ItemGuid = i.ItemGuid
					join(
						select distinct FieldGuid
						from i7_sflexi_fields
						where name = :SearchField
						) f on f.FieldGuid = v.FieldGuid
					where DefinitionGuid = :DefinitionGuid 
						and SiteGuid = ':SiteGuid'
					order by SortOrder :SortDirection
					limit :PageSize {(pageNumber > 1 ? "offset :OffsetRows;" : ";")}
				""";
		}
		else
		{
			sqlCommand = $"""
				select 
					row_number() over (order by SortOrder) as rowid
					, count(*) over() as totalrows
					, i.*
				from i7_sflexi_items i
				where DefinitionGuid = ':DefinitionGuid' 
					and SiteGuid = ':SiteGuid' 
				order by SortOrder :SortDirection
				limit :PageSize {(pageNumber > 1 ? "offset :OffsetRows;" : ";")}
				""";
		}

		int offsetRows = (pageSize * pageNumber) - pageSize;

		var sqlParams = new SqliteParameter[]
		{
			new(":PageSize", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
			new(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
			new(":SearchTerm", DbType.String, 255) { Direction = ParameterDirection.Input, Value = searchTerm },
			new(":SearchField", DbType.String, 50) { Direction = ParameterDirection.Input, Value = searchField },
			new(":DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = defGuid.ToString() },
			new(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
			new(":SortDirection", DbType.String, 4) { Direction = ParameterDirection.Input, Value = descending ? "desc" : "asc" }
		};

		return SqliteHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			sqlParams);
	}


	/// <summary>
	/// Gets an IDataReader with all items for a single definition.
	/// </summary>
	/// <param name="itemID"> itemID </param>
	public static IDataReader GetAllForDefinition(Guid definitionGuid, Guid siteGuid)
	{
		string sqlCommand = """
			select 
				SiteGuid
				,FeatureGuid
				,i.ModuleGuid
				,i.ModuleId
				,DefinitionGuid
				,ItemGuid
				,ItemId
				,i.SortOrder
				,CreatedUTC
				,LastModUTC
				,ms.SettingValue as GlobalViewSortOrder 
			from i7_sflexi_items i
			left join mp_Modulesettings ms on ms.ModuleGuid = i.ModuleGuid
			where DefinitionGuid = ':DefGuid' 
				and SiteGuid = ':SiteGuid' 
				and ms.SettingName = 'GlobalViewSortOrder' 
			order by 
				GlobalViewSortOrder asc
				,i.ModuleId asc
				,SortOrder asc
				,CreatedUTC asc;
			""";

		var sqlParam = new SqliteParameter[]
		{
			new(":DefGuid", DbType.String) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() },
			new(":SiteGuid", DbType.String) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() }
		};

		return SqliteHelper.ExecuteReader(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			sqlParam);
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
			select 
				i.ModuleId as ModuleId
				,i.ItemGuid as ItemGuid
				,i.ItemId as ItemId
				,i.SortOrder as SortOrder
				,i.CreatedUTC as CreatedUTC
				,m.ModuleTitle as ModuleTitle
				,m.ViewRoles as ViewRoles
				,pm.PublishBeginDate as PublishBeginDate
				,pm.PublishEndDate as PublishEndDate
			from i7_sflexi_items i 
			join mp_PageModules pm on i.ModuleGuid = pm.ModuleGuid 
			join mp_Modules m on i.ModuleGuid = m.Guid 
			where i.SiteGuid = :SiteGuid 
				and pm.PageId = :PageId 
			order by SortOrder asc;
			""";

		var sqlParams = new SqliteParameter[]
			{
				new(":SiteGuid", DbType.Int32) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
				new(":PageId", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageId }
			};

		return SqliteHelper.ExecuteReader(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			sqlParams);
	}
	private static string santizeSortDirection(string sortDirection)
	{
		if (sortDirection != "ASC" && sortDirection != "DESC")
		{
			return "ASC";
		}
		return sortDirection;
	}

	public static IDataReader GetForModule(
		int moduleID,
		int pageNumber = 1,
		int pageSize = 20,
		string sortDirection = "ASC")
	{
		sortDirection = santizeSortDirection(sortDirection);

		var commandText = $"""
			SELECT *
			FROM (
				SELECT *, count(*) over() AS TotalRows
				FROM i7_sflexi_items i
				WHERE i.ModuleID = :ModuleID) t
			WHERE TotalRows > 0
			ORDER BY t.SortOrder {sortDirection}
			{(pageSize > -1 ? "LIMIT :PageSize" : string.Empty)} 
			{(pageNumber > 1 ? "OFFSET :OffsetRows" : string.Empty)};
			""";

		int offsetRows = (pageSize * pageNumber) - pageSize;

		var commandParameters = new SqliteParameter[]
		{
			new(":ModuleId", DbType.Int32) { Direction = ParameterDirection.Input, Value = moduleID },
			new(":PageSize", DbType.Int32){ Direction = ParameterDirection.Input, Value = pageSize },
			new(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
		};

		return SqliteHelper.ExecuteReader(
			ConnectionString.GetWriteConnectionString(),
			commandText,
			commandParameters);
	}

	public static IDataReader GetForModuleWithValues(
		Guid moduleGuid,
		int pageNumber = 1,
		int pageSize = 20,
		string searchTerm = "",
		string searchField = "",
		string sortDirection = "ASC")
	{
		string sqlCommand;

		sortDirection = santizeSortDirection(sortDirection);

		if (pageSize > 0)
		{
			//query with paging
			sqlCommand = $"""
				SELECT 
					pg.*
					,f.Name AS FieldName
					,v.FieldValue
				FROM (
					SELECT 
						i.*
						,Count(*) OVER () AS TotalRows
					FROM i7_sflexi_items i
					WHERE i.ItemGuid IN (
						SELECT DISTINCT ItemGuid
						FROM i7_sflexi_values v
						{(!string.IsNullOrWhiteSpace(searchField) ? "JOIN i7_sflexi_fields f ON f.FieldGuid = v.FieldGuid" : string.Empty)}
						WHERE v.ItemGuid = i.ItemGuid
						{(!string.IsNullOrWhiteSpace(searchTerm) ? "AND v.FieldValue LIKE :SearchTerm" : string.Empty)}
						{(!string.IsNullOrWhiteSpace(searchField) ? "AND f.Name = :SearchField" : string.Empty)}
					) 
					AND i.ModuleGuid = :ModuleGuid
					GROUP BY i.ItemID
					ORDER BY i.SortOrder {sortDirection}, CreatedUtc {sortDirection}
					LIMIT :PageSize
					OFFSET :OffSetRows) pg
				JOIN i7_sflexi_values v ON  v.ItemGuid = pg.ItemGuid
				JOIN i7_sflexi_fields f ON v.FieldGuid = f.FieldGuid
				WHERE TotalRows > 0;
				""";
		}
		else
		{
			//query without paging
			sqlCommand = $"""
				SELECT 
					i.*
					,f.Name AS FieldName
					,v.FieldValue
				FROM i7_sflexi_items i
				JOIN i7_sflexi_values v ON v.ItemGuid = i.ItemGuid
				JOIN i7_sflexi_fields f ON v.FieldGuid = f.FieldGuid
				WHERE i.ModuleGuid = :ModuleGuid
					{(!string.IsNullOrWhiteSpace(searchTerm) ? "AND v.FieldValue LIKE :SearchTerm" : string.Empty)}
					{(!string.IsNullOrWhiteSpace(searchField) ? "AND f.Name = :SearchField" : string.Empty)}
				ORDER BY 
					i.SortOrder {sortDirection}
					,CreatedUtc {sortDirection};
				""";
		}

		int offsetRows = (pageSize * pageNumber) - pageSize;

		var sqlParams = new SqliteParameter[]
		{
			new(":PageSize", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
			new(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
			new(":SearchTerm", DbType.String, 255) { Direction = ParameterDirection.Input, Value = $"%{searchTerm}%"},
			new(":SearchField", DbType.String, 50) { Direction = ParameterDirection.Input, Value = searchField },
			new(":ModuleGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = moduleGuid }
		};

		return SqliteHelper.ExecuteReader(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			sqlParams);
	}

	public static IDataReader GetForModuleWithValues_Paged(
		Guid moduleGuid,
		int pageNumber,
		int pageSize,
		string searchTerm = "",
		string searchField = "",
		string sortDirection = "ASC")
	{
		string commandText;
		sortDirection = santizeSortDirection(sortDirection);

		if (string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(searchTerm))
		{
			commandText = $"""
				SELECT TOP (:PageSize) *
				FROM
				    (
				        SELECT RowID = ROWNUMBER() OVER (ORDER BY i.SortOrder),
				                TotalRows = Count(*) OVER (),
				                i.*,
				                v.FieldValue,
				                f.Name AS FieldName,
				                v.FieldGuid
				        FROM i7_sflexi_items i
				        JOIN
				            (
				                SELECT DISTINCT ItemGuid, FieldValue, FieldGuid
				                FROM i7_sflexi_values
				                WHERE FieldValue LIKE '%' + :SearchTerm + '%'
				            )
				            v
				        ON v.ItemGuid = i.ItemGuid
				        JOIN i7_sflexi_fields f
				        ON f.FieldGuid = v.FieldGuid
				        WHERE ModueGuid = :ModuleGuid
				    )
				    a
				WHERE a.RowID > ((:PageNumber -1) * :PageSize)
				ORDER BY SortOrder {sortDirection}
				""";
		}
		else if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(searchTerm))
		{
			commandText = $"""
				SELECT 
					ItemID
					, TotalRows = Count(*) OVER ()
				INTO TEMP TABLE ItemsToGet
				FROM i7_sflexi_values v
				JOIN
				    (
				        SELECT DISTINCT FieldGuid, Name AS FieldName
				        FROM i7_sflexi_fields
				        WHERE Name = :SearchField
				    ) f
				    ON f.FieldGuid = v.FieldGuid
				JOIN i7_sflexi_items items ON items.ItemGuid = v.ItemGuid
				WHERE FieldValue LIKE '%' + :SearchTerm + '%'
				AND v.ModuleGuid = :ModuleGuid
				ORDER BY SortOrder {sortDirection}
				OFFSET ((:PageNumber - 1) * :PageSize) ROWS
				FETCH NEXT :PageSize ROWS ONLY;

				SELECT 
					TotalRows
					, i.*
					, v.FieldValue
					, f.FieldName
					, v.FieldGuid
				From i7_sflexi_items i 
				JOIN
				    (
				        SELECT DISTINCT ItemGuid, FieldGuid, FieldVlaue
				        FROM i7_sflexi_values
				    ) v ON v.ItemGuid = i.ItemGuid
				JOIN
				    (
				        SELECT DISTINCT FieldGiud, Name AS FieldName
				        FROM i7_sflexi_fields
				    ) f ON f.FeldGuid = v.FieldGuid
				JOIN ItemsToGet ON ItemsToGet.ItemID = i.ItemID;
				DROP TABLE ItemsToGet;
				""";
		}
		else
		{
			commandText = $"""
				SELECT TOP (:PageSize) *
				FROM
				(
				    SELECT
				    RowID = ROW_NUMBER() OVER (ORDER BY i.SortOrder),
				    TotalRows = Count(*) OVER (),
				    i.*,
				    v.FieldValue,
				    f.Name AS FieldName,
				    v.FieldGuid
				    FROM public.i7_sflexi_items i
				    JOIN public.i7_sflexi_values v
				    ON v.ItemGuid = i.ItemGuid
				    JOIN public.i7_sflexi_fields f
				    ON f.FieldGuid = v.FieldGuid
				    WHERE i.ModuleGuid = :ModuleGuid
				)
				a
				WHERE a.RowID > ((:PageNumber -1 * :PageSize)
				ORDER BY SorrtOrder {sortDirection}
				""";
		}
		int offsetRows = (pageSize * pageNumber) - pageSize;

		var commandParameters = new SqliteParameter[]
		{
				new(":PageSize", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
				new(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
				new(":SearchTerm", DbType.String, 255) { Direction = ParameterDirection.Input, Value = searchTerm },
				new(":SearchField", DbType.String, 50) { Direction = ParameterDirection.Input, Value = searchField },
				new(":ModuleGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = moduleGuid.ToString() },
		};

		return SqliteHelper.ExecuteReader(
			ConnectionString.GetWriteConnectionString(),
			commandText,
			commandParameters);
	}
	public static IDataReader GetForDefinition(
		Guid definitionGuid,
		Guid siteGuid,
		int pageNumber = 1,
		int pageSize = 20,
		string sortDirection = "ASC")
	{
		sortDirection = santizeSortDirection(sortDirection);

		string sqlCommand = $"""
			SELECT i.*
			, ms.SettingValue AS GlobalViewSortOrder
			, Count(i.*) AS TotalRows
			FROM i7_sflexi_items i
			LEFT JOIN mp_ModuleSettings ms ON ms.ModuleGuid = i.ModuleGuid
			WHERE DefinitionGuid = :DefGuid 
				AND i.SiteGuid = :SiteGuid 
				AND ms.SettingName = 'GlobalViewSortOrder' 
			ORDER BY 
				GlobalViewSortOrder {sortDirection}
				, i.ModuleID {sortDirection}
				, SortOrder {sortDirection}
				, CreatedUtc {sortDirection})
			LIMIT :PageSize {(pageNumber > 1 ? "OFFSET :OffsetRows" : string.Empty)};
			""";

		int offsetRows = (pageSize * pageNumber) - pageSize;

		var sqlParams = new SqliteParameter[]
		{
			new(":DefGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() },
			new(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
			new(":PageSize", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
			new(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
		};

		return SqliteHelper.ExecuteReader(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			sqlParams);
	}

	public static IDataReader GetForDefinitionWithValues(
		Guid defGuid,
		Guid siteGuid,
		int pageNumber = 1,
		int pageSize = 25,
		string searchTerm = "",
		string searchField = "",
		string sortDirection = "ASC")
	{
		string sqlCommand;
		sortDirection = santizeSortDirection(sortDirection);

		if (pageSize > -1)
		{
			//query with paging
			sqlCommand = $"""
				SELECT pg.*
				, f.Name AS FieldName
				, v.FieldValue
				FROM (
					SELECT i.*
					, ms.SettingValue AS GlobalViewSortOrder
					, COUNT(i.*) AS TotalRows
					FROM i7_sflexi_items i
					LEFT JOIN mp_ModuleSettings ms ON ms.ModuleGuid = i.ModuleGuid
					WHERE i.ItemGuid IN (
						SELECT DISTINCT ItemGuid
						FROM i7_sflexi_values v
						{(!string.IsNullOrWhiteSpace(searchField) ? "JOIN i7_sflexi_fields f ON f.FieldGuid = v.FieldGuid" : string.Empty)}
						WHERE v.ItemGuid = i.ItemGuid
						{(!string.IsNullOrWhiteSpace(searchTerm) ? "AND v.FieldValue LIKE :SearchTerm" : string.Empty)}
						{(!string.IsNullOrWhiteSpace(searchField) ? "AND f.Name = :SearchField" : string.Empty)}
					) 
					AND i.DefinitionGuid = :DefinitionGuid
					AND i.SiteGuid = :SiteGuid
					AND ms.SettingName = 'GlobalViewSortOrder' 
					ORDER BY 
						GlobalViewSortOrder {sortDirection}
						, i.ModuleID {sortDirection}
						, i.SortOrder {sortDirection}
						, i.CreatedUtc {sortDirection}
					LIMIT :PageSize
					OFFSET :OffsetRows) pg
				JOIN i7_sflexi_values v ON  v.ItemGuid = pg.ItemGuid
				JOIN i7_sflexi_fields f ON v.FieldGuid = f.FieldGuid
				LEFT JOIN mp_ModuleSettings ms ON ms.ModuleGuid = pg.ModuleGuid
				WHERE TotalRows > 0;
				""";
		}
		else
		{
			//query without paging
			sqlCommand = $"""
				SELECT i.*
				,f.Name AS FieldName
				,v.FieldValue
				,ms.SettingValue AS GlobalViewSortOrder
				FROM i7_sflexi_items i
				JOIN i7_sflexi_values v ON v.ItemGuid = i.ItemGuid
				JOIN i7_sflexi_fields f ON v.FieldGuid = f.FieldGuid
				LEFT JOIN mp_ModuleSettings ms ON ms.ModuleGuid = i.ModuleGuid
				WHERE DefinitionGuid = :DefinitionGuid 
					AND i.SiteGuid = :SiteGuid 
					AND ms.SettingName = 'GlobalViewSortOrder' 
				{(!string.IsNullOrWhiteSpace(searchTerm) ? "AND v.FieldValue LIKE :SearchTerm" : string.Empty)}
				{(!string.IsNullOrWhiteSpace(searchField) ? "AND f.Name = :SearchField" : string.Empty)}
				ORDER BY 
					GlobalViewSortOrder {sortDirection}
					,i.ModuleID {sortDirection}
					,i.SortOrder {sortDirection}
					,i.CreatedUtc {sortDirection};
				""";
		}

		var offsetRows = (pageSize * pageNumber) - pageSize;

		var sqlParams = new SqliteParameter[]
		{
			new(":PageSize", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
			new(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
			new(":SearchTerm", DbType.String, 255) { Direction = ParameterDirection.Input, Value = "%" + searchTerm + "%"},
			new(":SearchField", DbType.String, 50) { Direction = ParameterDirection.Input, Value = searchField },
			new(":DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = defGuid.ToString() },
			new(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() }
		};

		return SqliteHelper.ExecuteReader(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			sqlParams);
	}


	public static IDataReader GetForDefinitionWithValues_Paged(
		Guid definitionGuid,
		Guid siteGuid,
		int pageNumber,
		int pageSize,
		string searchTerm = "",
		string searchField = "",
		string sortDirection = "ASC")
	{
		string commandText;

		sortDirection = santizeSortDirection(sortDirection);

		if (string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(searchTerm))
		{
			commandText = $"""
				SELECT TOP (:PageSize) *
				FROM (
					SELECT
						RowID = ROW_NUMBER() OVER (ORDER BY i.SortOrder)
						,TotalRows = Count(*) OVER ()
						,i.*
						,v.FieldValue
						,f.Name AS FieldName
						,v.FieldGuid
					FROM i7_sflexi_items i
					JOIN
						(
							SELECT DISTINCT
								ItemGuid
								,FieldValue
								,FieldGuid
							FROM i7_sflexi_values
							WHERE FieldValue LIKE '%' + :SearchTerm + '%'
						) v ON v.ItemGuid = i.ItemGuid
					JOIN i7_sflexi_fields f ON f.FieldGuid = v.FieldGuid
					WHERE i.DefinitionGuid = :DefGuid
						AND i.SiteGuid   = :SiteGuid
				    ) a
				WHERE a.RowID > ((:PageNumber - 1) * :PageSize)
				ORDER BY SortOrder {sortDirection};
				""";
		}
		else if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(searchTerm))
		{
			commandText = $"""
					SELECT TOP (:PageSize) *
					FROM (
							SELECT
								RowID = Row_number() OVER ( ORDER BY i.SortOrder)
								,TotalRows = Count(*) OVER ()
								,i.*
								,v.FieldValue
								,f.FieldName
								,v.FieldGuid
							FROM i7_sflexi_items i
							JOIN (
									SELECT DISTINCT
										ItemGuid
										,FieldGuid
										,FieldValue
									FROM i7_sflexi_value
									WHERE fieldvalue LIKE '%' + :SearchTerm + '%'
								) v ON v.itemguid = i.itemguid
									JOIN
										(
											SELECT DISTINCT 
												fieldguid
												,Name AS FieldName
											FROM i7_sflexi_field
											WHERE NAME = :SearchField
											AND DefinitionGuid = :DefGuid
										) f ON f.fieldguid = v.fieldguid
							WHERE i.DefinitionGuid = :DefGuid
							AND i.SiteGuid   = :SiteGuid
						) a
					WHERE a.rowid > ( ( :PageNumber - 1 ) * :PageSize )
					ORDER BY SortOrder {sortDirection}
				{(pageNumber > 1 ? "OFFSET :OffsetRows" : string.Empty)};
				""";
		}
		else
		{
			commandText = $"""
				SELECT TOP (@PageSize) *
				FROM (
						SELECT
							RowID = ROW_NUMBER() OVER (ORDER BY i.SortOrder)
							,TotalRows = Count(*) OVER ()
							,i.*
							,v.FieldValue
							,f.Name AS FieldName
							,v.FieldGuid
						FROM i7_sflexi_items i
						JOIN i7_sflexi_values v ON v.ItemGuid = i.ItemGuid
						JOIN i7_sflexi_fields f ON f.FieldGuid = v.FieldGuid
						WHERE i.DefinitionGuid = :DefGuid
						AND i.SiteGuid   = :SiteGuid
				) a
				WHERE a.RowID > ((:PageNumber - 1) * :PageSize)
				ORDER BY SortOrder {sortDirection}
				{(pageNumber > 1 ? "OFFSET :OffsetRows" : string.Empty)};
				""";
		}

		var offsetRows = (pageSize * pageNumber) - pageSize;

		var commandParameters = new SqliteParameter[]
		{
					new(":PageSize", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
					new(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
					new(":SearchTerm", DbType.String, 255) { Direction = ParameterDirection.Input, Value = searchTerm },
					new(":SearchField", DbType.String, 50) { Direction = ParameterDirection.Input, Value = searchField },
					new(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
					new(":DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() },
		};

		return SqliteHelper.ExecuteReader(
			ConnectionString.GetWriteConnectionString(),
			commandText,
			commandParameters);
	}


	public static int GetHighestSortOrder(int moduleId)
	{
		string commandText = "SELECT MAX(SortOrder) FROM i7_sflexi_items WHERE moduleId = :moduleid;";

		var commandParameter = new SqliteParameter(":ModuleId", DbType.Int32) { Direction = ParameterDirection.Input, Value = moduleId };

		return Convert.ToInt32(SqliteHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			commandText,
			commandParameter));
	}
}
