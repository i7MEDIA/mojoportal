using System;
using System.Collections.Generic;
using System.Data;
using mojoPortal.Data;

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
		var sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_items_Insert", 11);
		sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
		sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
		sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
		sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
		sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleID);
		sph.DefineSqlParameter("@DefinitionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, definitionGuid);
		sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
		sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
		sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
		sph.DefineSqlParameter("@ViewRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, viewRoles);
		sph.DefineSqlParameter("@EditRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, editRoles);
		int newID = Convert.ToInt32(sph.ExecuteScalar());
		return newID;
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
		var sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_items_Update", 11);
		sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
		sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
		sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
		sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleID);
		sph.DefineSqlParameter("@DefinitionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, definitionGuid);
		sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
		sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
		sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
		sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
		sph.DefineSqlParameter("@ViewRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, viewRoles);
		sph.DefineSqlParameter("@EditRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, editRoles);

		int rowsAffected = sph.ExecuteNonQuery();
		return rowsAffected > 0;
	}

	/// <summary>
	/// Deletes a row from the i7_sflexi_items table. Returns true if row deleted.
	/// </summary>
	/// <returns>bool</returns>
	public static bool Delete(int itemID)
	{
		var sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_items_Delete", 1);
		sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemID);
		int rowsAffected = sph.ExecuteNonQuery();
		return rowsAffected > 0;
	}

	/// <summary>
	/// Deletes rows from the i7_sflexi_items table. Returns true if rows deleted.
	/// </summary>
	/// <returns>bool</returns>
	public static bool DeleteBySite(Guid siteGuid)
	{
		var sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_items_DeleteBySite", 1);
		sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
		int rowsAffected = sph.ExecuteNonQuery();
		return rowsAffected > 0;
	}

	/// <summary>
	/// Deletes rows from the i7_sflexi_items table. Returns true if rows deleted.
	/// </summary>
	/// <returns>bool</returns>
	public static bool DeleteByModule(Guid moduleGuid)
	{
		var sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_items_DeleteByModule", 1);
		sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
		int rowsAffected = sph.ExecuteNonQuery();
		return rowsAffected > 0;
	}

	/// <summary>
	/// Gets an IDataReader with one row from the i7_sflexi_items table.
	/// </summary>
	public static IDataReader GetOne(int itemID)
	{
		var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_items_SelectOne", 1);
		sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemID);
		return sph.ExecuteReader();
	}

	/// <summary>
	/// Gets an IDataReader with one row from the i7_sflexi_items table.
	/// </summary>
	public static IDataReader GetOne(Guid itemGuid)
	{
		var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_items_SelectOneByGuid", 1);
		sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
		return sph.ExecuteReader();
	}

	/// <summary>
	/// Gets an IDataReader with item and values
	/// </summary>
	public static IDataReader GetOneWithValues(int itemId)
	{
		var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_itemsvalues_SelectOneById", 1);
		sph.DefineSqlParameter("@ItemId", SqlDbType.Int, ParameterDirection.Input, itemId);
		return sph.ExecuteReader();
	}

	public static IDataReader GetByIDsWithValues(Guid defGuid, Guid siteGuid, List<int> itemIDs, int pageNumber = 1, int pageSize = 20, string sortDirection = "ASC")
	{
		var idTable = new SqlDataRecordList(itemIDs);

		var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_itemsvalues_SelectByMultipleIDs", 6);
		sph.DefineSqlParameter("@DefinitionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, defGuid);
		sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
		sph.DefineSqlParameter("@ItemIDs", SqlDbType.Structured, "integer_list_tbltype", ParameterDirection.Input, idTable);
		sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
		sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
		sph.DefineSqlParameter("@SortDirection", SqlDbType.VarChar, 4, ParameterDirection.Input, sortDirection);

		return sph.ExecuteReader();
	}

	/// <summary>
	/// Gets a count of rows in the i7_sflexi_items table.
	/// </summary>
	public static int GetCount() => Convert.ToInt32(SqlHelper.ExecuteScalar(
		ConnectionString.GetReadConnectionString(),
		CommandType.StoredProcedure,
		"i7_sflexi_items_GetCount",
		null));

	public static int GetCountForModule(int moduleId)
	{
		var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_items_GetCountForModule", 1);
		sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
		return Convert.ToInt32(sph.ExecuteScalar());
	}

	/// <summary>
	/// Gets an IDataReader with all rows in the i7_sflexi_items table.
	/// </summary>
	public static IDataReader GetAll(Guid siteGuid)
	{
		var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_items_SelectAll", 1);
		sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
		return sph.ExecuteReader();
	}

	/// <summary>
	/// Gets an IDataReader with page of items for module.
	/// </summary>
	public static IDataReader GetForModule(
		int moduleId,
		int pageNumber = 1,
		int pageSize = 20,
		string sortDirection = "ASC"
	)
	{
		var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_items_SelectAllForModule", 4);
		sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
		sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
		sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
		sph.DefineSqlParameter("@SortDirection", SqlDbType.VarChar, 4, ParameterDirection.Input, sortDirection);
		return sph.ExecuteReader();
	}

	/// <summary>
	/// Gets an IDataReader with all items for module with values.
	/// </summary>
	public static IDataReader GetForModuleWithValues(
		Guid moduleGuid,
		int pageNumber = 1,
		int pageSize = 20,
		string searchTerm = "",
		string searchField = "",
		string sortDirection = "ASC"
	)
	{
		var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_items_SelectAllForModuleWithValues", 6);
		sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
		sph.DefineSqlParameter("@SearchTerm", SqlDbType.NVarChar, -1, ParameterDirection.Input, searchTerm);
		sph.DefineSqlParameter("@SearchField", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchField);
		sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
		sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
		sph.DefineSqlParameter("@SortDirection", SqlDbType.VarChar, 4, ParameterDirection.Input, sortDirection);
		return sph.ExecuteReader();
	}

	/// <summary>
	/// Gets an IDataReader with all items for a single definition.
	/// </summary>
	public static IDataReader GetForDefinition(Guid definitionGuid, Guid siteGuid, int pageNumber = 1, int pageSize = 20, string sortDirection = "ASC")
	{
		var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_items_SelectAllForDefinition", 5);
		sph.DefineSqlParameter("@DefinitionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, definitionGuid);
		sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
		sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
		sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
		sph.DefineSqlParameter("@SortDirection", SqlDbType.VarChar, 4, ParameterDirection.Input, sortDirection);
		return sph.ExecuteReader();
	}

	/// <summary>
	/// Gets an IDataReader with all items for a single definition.
	/// </summary>
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
		var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_items_SelectAllForDefinitionWithValues", 7);
		sph.DefineSqlParameter("@DefinitionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, defGuid);
		sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
		sph.DefineSqlParameter("@SearchTerm", SqlDbType.NVarChar, ParameterDirection.Input, searchTerm);
		sph.DefineSqlParameter("@SearchField", SqlDbType.NVarChar, 50, ParameterDirection.Input, searchField);
		sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
		sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
		sph.DefineSqlParameter("@SortDirection", SqlDbType.VarChar, 4, ParameterDirection.Input, sortDirection);
		return sph.ExecuteReader();
	}

	/// <summary>
	/// Gets all superflexi items from all superflexi modules on a page
	/// </summary>
	/// <returns></returns>
	public static IDataReader GetByCMSPage(Guid siteGuid, int pageId)
	{
		var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_items_SelectByCMSPage", 2);
		sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
		sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
		return sph.ExecuteReader();
	}

	public static int GetHighestSortOrder(int moduleId)
	{
		var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_items_GetHighestSortOrder", 1);
		sph.DefineSqlParameter("@ModuleId", SqlDbType.Int, ParameterDirection.Input, moduleId);
		return Convert.ToInt32(sph.ExecuteScalar());
	}
}
