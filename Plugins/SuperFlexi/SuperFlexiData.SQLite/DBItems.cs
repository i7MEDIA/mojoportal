using mojoPortal.Data;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

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
			string editRoles)
        {
			var sqlCommand = @"
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
				select last_insert_rowid();";
			
            var sqlParams = new List<SqliteParameter>
            {
                new SqliteParameter(":ItemGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = itemGuid.ToString() },
                new SqliteParameter(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
                new SqliteParameter(":FeatureGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = featureGuid.ToString() },
                new SqliteParameter(":ModuleGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = moduleGuid.ToString() },
                new SqliteParameter(":ModuleId", DbType.Int32) { Direction = ParameterDirection.Input, Value = moduleID },
                new SqliteParameter(":DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() },
                new SqliteParameter(":SortOrder", DbType.Int32) { Direction = ParameterDirection.Input, Value = sortOrder },
                new SqliteParameter(":CreatedUTC", DbType.DateTime) { Direction = ParameterDirection.Input, Value = createdUtc },
                new SqliteParameter(":LastModUTC", DbType.DateTime) { Direction = ParameterDirection.Input, Value = lastModUtc },
                new SqliteParameter(":ViewRoles", DbType.String) { Direction = ParameterDirection.Input, Value = viewRoles },
                new SqliteParameter(":EditRoles", DbType.String) { Direction = ParameterDirection.Input, Value = editRoles }
			};

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand,
                sqlParams.ToArray()).ToString());
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
			var sqlCommand = @"
				update i7_sflexi_items 
				set SiteGuid = :SiteGuid
				   ,FeatureGuid = :FeatureGuid
				   ,ModuleGuid = :ModuleGuid
				   ,ModuleId = :ModuleId
				   ,DefinitionGuid = :DefinitionGuid
				   ,SortOrder = :SortOrder
				   ,CreatedUTC = :CreatedUTC
				   ,LastModUTC = :LastModUTC
				where ItemGuid = :ItemGuid;";

            var sqlParams = new List<SqliteParameter>
            {
                new SqliteParameter(":ItemGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = itemGuid.ToString() },
                new SqliteParameter(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
                new SqliteParameter(":FeatureGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = featureGuid.ToString() },
                new SqliteParameter(":ModuleGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = moduleGuid.ToString() },
                new SqliteParameter(":ModuleId", DbType.Int32) { Direction = ParameterDirection.Input, Value = moduleID },
                new SqliteParameter(":DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() },
                new SqliteParameter(":SortOrder", DbType.Int32) { Direction = ParameterDirection.Input, Value = sortOrder },
                new SqliteParameter(":CreatedUTC", DbType.DateTime) { Direction = ParameterDirection.Input, Value = createdUtc },
                new SqliteParameter(":LastModUTC", DbType.DateTime) { Direction = ParameterDirection.Input, Value = lastModUtc },
				new SqliteParameter(":ViewRoles", DbType.String) { Direction = ParameterDirection.Input, Value = viewRoles },
				new SqliteParameter(":EditRoles", DbType.String) { Direction = ParameterDirection.Input, Value = editRoles }
			};

            int rowsAffected = Convert.ToInt32(SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand,
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
            sqlCommand.Append("delete from i7_sflexi_items where ItemId = :ItemId;");

            var sqlParam = new SqliteParameter(":ItemId", DbType.Int32) { Direction = ParameterDirection.Input, Value = itemID };

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
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
            sqlCommand.Append("delete from i7_sflexi_items where SiteGuid = :SiteGuid;");

            var sqlParam = new SqliteParameter(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() };
            
            int rowsAffected = SqliteHelper.ExecuteNonQuery(
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
            sqlCommand.Append("delete from i7_sflexi_items where ModuleGuid = :ModuleGuid;");

            var sqlParam = new SqliteParameter(":ModuleGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = moduleGuid.ToString() };

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
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
    //    public static bool DeleteByDefinition(Guid definitionGuid)
    //    {
    //        StringBuilder sqlCommand = new StringBuilder();
    //        sqlCommand.Append("delete from i7_sflexi_items where DefinitionGuid = :DefinitionGuid;");

    //        var sqlParam = new SqliteParameter(":DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() };

    //        int rowsAffected = SqliteHelper.ExecuteNonQuery(
    //            ConnectionString.GetWriteConnectionString(),
				//sqlCommand.ToString(),
    //            sqlParam);

    //        return (rowsAffected > 0);
    //    }

        /// <summary>
        /// Gets an IDataReader with one row from the i7_sflexi_items table.
        /// </summary>
        /// <param name="itemID"> itemID </param>
        public static IDataReader GetOne(Guid itemGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("select * from i7_sflexi_items where ItemGuid = :ItemGuid;");

            var sqlParam = new SqliteParameter(":ItemGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = itemGuid.ToString() };

            return SqliteHelper.ExecuteReader(
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
			sqlCommand.Append("select * from i7_sflexi_items where ItemId = :ItemId;");

			var sqlParam = new SqliteParameter(":ItemId", DbType.Int32) { Direction = ParameterDirection.Input, Value = itemId };

			return SqliteHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				sqlParam);
		}

		public static IDataReader GetOneWithValues(int itemId)
		{
			string sqlCommand = $@"
					SELECT i.*, f.Name AS FieldName, v.FieldValue
					FROM i7_sflexi_items i
					JOIN i7_sflexi_values v ON v.ItemGuid = i.ItemGuid
					JOIN i7_sflexi_fields f ON v.FieldGuid = f.FieldGuid
					WHERE i.ItemID = :ItemID;";

			var sqlParam = new SqliteParameter(":ItemID", DbType.Int32) { Direction = ParameterDirection.Input, Value = itemId };

			return SqliteHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand,
				sqlParam
			);
		}



		public static IDataReader GetByIDsWithValues(Guid defGuid, Guid siteGuid, List<int> itemIDs, int pageNumber = 1, int pageSize = 20)
		{

			string sqlCommand = $@"

					SELECT pg.*, f.Name AS FieldName, v.FieldValue
					FROM (
						SELECT i.*, count(*) over() AS TotalRows
						FROM i7_sflexi_items i
						WHERE {getItems()}
						AND i.DefinitionGuid = ?DefinitionGuid
						AND i.SiteGuid = ?SiteGuid
						GROUP BY ItemID
				 		LIMIT :PageSize
				 		OFFSET :OffSetRows) pg
					JOIN i7_sflexi_values v ON  v.ItemGuid = pg.ItemGuid
					JOIN i7_sflexi_fields f ON v.FieldGuid = f.FieldGuid
					WHERE TotalRows > 0";

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

			sqlParams.AddRange(new List<SqliteParameter>()
			{
				new SqliteParameter("?PageSize", DbType.Int32){ Direction = ParameterDirection.Input, Value = pageSize },
				new SqliteParameter("?OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
				new SqliteParameter("?DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = defGuid },
				new SqliteParameter("?SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid }
			});

			return SqliteHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand,
				sqlParams.ToArray()
			);
		}
		/// <summary>
		/// Gets a count of rows in the i7_sflexi_items table.
		/// </summary>
		public static int GetCount()
        {
            var sqlCommand = "select count(*) from i7_sflexi_items;";

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                 ConnectionString.GetReadConnectionString(),
				 sqlCommand));
        }
		public static int GetCountForModule(int moduleId)
		{
			var sqlCommand = $"select count(*) from i7_sflexi_items where ModuleId = {moduleId};";

			return Convert.ToInt32(SqliteHelper.ExecuteScalar(
				 ConnectionString.GetReadConnectionString(),
				 sqlCommand));
		}
		
		/// <summary>
		/// Gets an IDataReader with all rows in the i7_sflexi_items table.
		/// </summary>
		public static IDataReader GetAll(Guid siteGuid)
        {
            var sqlCommand = "select * from i7_sflexi_items where SiteGuid = :SiteGuid;";
			var sqlParam = new SqliteParameter(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid };
            return SqliteHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam);
        }


        /// <summary>
        /// Gets an IDataReader with all items for module.
        /// </summary>
        /// <param name="itemID"> itemID </param>
        public static IDataReader GetModuleItems(int moduleID)
        {
            var sqlCommand = "select * from i7_sflexi_items where ModuleId = :ModuleId order by SortOrder asc;";
            var sqlParam = new SqliteParameter(":ModuleId", DbType.Int32) { Direction = ParameterDirection.Input, Value = moduleID };

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
				sqlCommand,
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
			StringBuilder sqlCommand = new();

			if (String.IsNullOrWhiteSpace(searchField) && !String.IsNullOrWhiteSpace(searchTerm))
			{
				sqlCommand.Append(@"select row_number() over (order by SortOrder) as rowid
					, count(*) over() as totalrows
					, i.*
					from i7_sflexi_items i
						join(
							select distinct ItemGuid
							from i7_sflexi_values
							where FieldValue like '%:SearchTerm%'
							) v on v.ItemGuid = i.ItemGuid
						where ModuleGuid = ':ModuleGuid' 
						order by SortOrder :SortDirection
						limit :PageSize " + (pageNumber > 1 ? "offset :OffsetRows;" : ";"));
			}
			else if (!String.IsNullOrWhiteSpace(searchField) && !String.IsNullOrWhiteSpace(searchTerm))
			{
				sqlCommand.Append(@"select row_number() over (order by SortOrder) as rowid
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
						where ModuleGuid = :ModuleGuid
						order by SortOrder :SortDirection
						limit :PageSize " + (pageNumber > 1 ? "offset :OffsetRows;" : ";"));
			}
			else
			{
				sqlCommand.Append(@"select row_number() over (order by SortOrder) as rowid
					, count(*) over() as totalrows
					, i.*
					from i7_sflexi_items i
					where ModuleGuid = ':ModuleGuid' 
					order by SortOrder :SortDirection
					limit :PageSize " + (pageNumber > 1 ? "offset :OffsetRows;" : ";"));
			}

			int offsetRows = (pageSize * pageNumber) - pageSize;

			var sqlParams = new List<SqliteParameter>
			{
				new SqliteParameter(":PageSize", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
				new SqliteParameter(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
				new SqliteParameter(":SearchTerm", DbType.String, 255) { Direction = ParameterDirection.Input, Value = searchTerm },
				new SqliteParameter(":SearchField", DbType.String, 50) { Direction = ParameterDirection.Input, Value = searchField },
				new SqliteParameter(":ModuleGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = moduleGuid.ToString() },
				new SqliteParameter(":SortDirection", DbType.String, 4) { Direction = ParameterDirection.Input, Value = descending ? "desc" : "asc" }
			};

			return SqliteHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				sqlParams.ToArray());
		}

		public static IDataReader GetPageForDefinition(
			Guid defGuid,
			Guid siteGuid,
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
				sqlCommand.Append(@"select row_number() over (order by SortOrder) as rowid
					, count(*) over() as totalrows
					, i.*
					from i7_sflexi_items i
						join(
							select distinct ItemGuid
							from i7_sflexi_values
							where FieldValue like '%:SearchTerm%'
							) v on v.ItemGuid = i.ItemGuid
						where DefinitionGuid = ':DefinitionGuid' and SiteGuid = ':SiteGuid'
						order by SortOrder :SortDirection
						limit :PageSize " + (pageNumber > 1 ? "offset :OffsetRows;" : ";"));
			}
			else if (!String.IsNullOrWhiteSpace(searchField) && !String.IsNullOrWhiteSpace(searchTerm))
			{
				sqlCommand.Append(@"select row_number() over (order by SortOrder) as rowid
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
						where DefinitionGuid = :DefinitionGuid and SiteGuid = ':SiteGuid'
						order by SortOrder :SortDirection
						limit :PageSize " + (pageNumber > 1 ? "offset :OffsetRows;" : ";"));
			}
			else
			{
				sqlCommand.Append(@"select row_number() over (order by SortOrder) as rowid
					, count(*) over() as totalrows
					, i.*
					from i7_sflexi_items i
					where DefinitionGuid = ':DefinitionGuid' and SiteGuid = ':SiteGuid' 
					order by SortOrder :SortDirection
					limit :PageSize " + (pageNumber > 1 ? "offset :OffsetRows;" : ";"));
			}

			int offsetRows = (pageSize * pageNumber) - pageSize;

			var sqlParams = new List<SqliteParameter>
			{
				new SqliteParameter(":PageSize", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
				new SqliteParameter(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
				new SqliteParameter(":SearchTerm", DbType.String, 255) { Direction = ParameterDirection.Input, Value = searchTerm },
				new SqliteParameter(":SearchField", DbType.String, 50) { Direction = ParameterDirection.Input, Value = searchField },
				new SqliteParameter(":DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = defGuid.ToString() },
				new SqliteParameter(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
				new SqliteParameter(":SortDirection", DbType.String, 4) { Direction = ParameterDirection.Input, Value = descending ? "desc" : "asc" }
			};

			return SqliteHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				sqlParams.ToArray());
		}


		/// <summary>
		/// Gets an IDataReader with all items for a single definition.
		/// </summary>
		/// <param name="itemID"> itemID </param>
		public static IDataReader GetAllForDefinition(Guid definitionGuid, Guid siteGuid)
        {
            string sqlCommand = @"select 
                SiteGuid, 
                FeatureGuid, 
                i.ModuleGuid, 
                i.ModuleId, 
                DefinitionGuid, 
                ItemGuid, 
                ItemId, 
                i.SortOrder, 
                CreatedUTC, 
                LastModUTC, 
                ms.SettingValue as GlobalViewSortOrder 
                from i7_sflexi_items i
                left join mp_Modulesettings ms on ms.ModuleGuid = i.ModuleGuid
                where DefinitionGuid = ':DefGuid' and SiteGuid = ':SiteGuid' and ms.SettingName = 'GlobalViewSortOrder' 
                order by GlobalViewSortOrder asc, i.ModuleId asc, SortOrder asc, CreatedUTC asc;";

			var sqlParam = new List<SqliteParameter> {
				new SqliteParameter(":DefGuid", DbType.String) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() },
				new SqliteParameter(":SiteGuid", DbType.String) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() }
			};

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand,
                sqlParam.ToArray());
        }

        /// <summary>
        /// Gets a page of data from the i7_sflexi_items table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
    //    public static IDataReader GetPage(
    //        int pageNumber,
    //        int pageSize,
    //        out int totalPages)
    //    {
    //        int pageLowerBound = (pageSize * pageNumber) - pageSize;
    //        totalPages = 1;
    //        int totalRows = GetCount();

    //        if (pageSize > 0) totalPages = totalRows / pageSize;

    //        if (totalRows <= pageSize)
    //        {
    //            totalPages = 1;
    //        }
    //        else
    //        {
    //            int remainder;
    //            Math.DivRem(totalRows, pageSize, out remainder);
    //            if (remainder > 0)
    //            {
    //                totalPages += 1;
    //            }
    //        }
    //        StringBuilder sqlCommand = new StringBuilder();

    //        sqlCommand.Append("select * from i7_sflexi_items limit :PageSize" + (pageNumber > 1 ? "offset :OffsetRows;" : ";"));

    //        var sqlParams = new List<SqliteParameter>
    //        {
    //            new SqliteParameter(":PageSize", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
    //            new SqliteParameter(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageLowerBound }
    //        };

    //        return SqliteHelper.ExecuteReader(
    //            ConnectionString.GetReadConnectionString(),
				//sqlCommand.ToString(),
    //            sqlParams.ToArray());
    //    }

        /// <summary>
        /// Gets
        /// </summary>
        /// <param name="siteGuid"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public static IDataReader GetByCMSPage(Guid siteGuid, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append(@"select 
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
                order by SortOrder asc;");

            var sqlParams = new List<SqliteParameter>
            {
                new SqliteParameter(":SiteGuid", DbType.Int32) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
                new SqliteParameter(":PageId", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageId }
            };

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
                sqlParams.ToArray());
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

            var commandText = $@"
                SELECT *
					FROM (
						SELECT *, count(*) over() AS TotalRows
						FROM i7_sflexi_items i
						WHERE i.ModuleID = :ModuleID) t
					WHERE TotalRows > 0
					ORDER BY t.SortOrder {sortDirection}
					{(pageSize > -1 ? "LIMIT :PageSize" : string.Empty)} 
					{(pageNumber > 1 ? "OFFSET :OffsetRows" : string.Empty)};";

			int offsetRows = (pageSize * pageNumber) - pageSize;

			var commandParameters = new SqliteParameter[]
            {
                new SqliteParameter(":ModuleId", DbType.Int32) { Direction = ParameterDirection.Input, Value = moduleID },
			    new SqliteParameter(":PageSize", DbType.Int32){ Direction = ParameterDirection.Input, Value = pageSize },
				new SqliteParameter(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
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
			//string sortField = "",
			string sortDirection = "ASC"
			)
		{
			string sqlCommand;

			sortDirection = santizeSortDirection(sortDirection);
			//var commandText = $@"
			//        SELECT *, f.Name AS FieldName, v.FieldValue
			//        FROM i7_sflexi_items i
			//        JOIN i7_sflexi_values v
			//        ON v.ItemGuid = i.ItemGuid
			//        JOIN i7_sflexi_fields f
			//        ON f.FieldGuid = v.FieldGuid
			//        WHERE ModuleId = :ModuleId
			//        ORDER BY i.SortOrder {sortDirection}";

			if (pageSize > 0)
			{
				//query with paging
				sqlCommand = $@"		
SELECT pg.*, f.Name AS FieldName, v.FieldValue
FROM (
	SELECT i.*
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
WHERE TotalRows > 0;";
			}
			else
			{
				//query without paging
				sqlCommand = $@"
SELECT i.*, f.Name AS FieldName, v.FieldValue
FROM i7_sflexi_items i
JOIN i7_sflexi_values v ON v.ItemGuid = i.ItemGuid
JOIN i7_sflexi_fields f ON v.FieldGuid = f.FieldGuid
WHERE i.ModuleGuid = :ModuleGuid
{(!string.IsNullOrWhiteSpace(searchTerm) ? "AND v.FieldValue LIKE :SearchTerm" : string.Empty)}
{(!string.IsNullOrWhiteSpace(searchField) ? "AND f.Name = :SearchField" : string.Empty)}
ORDER BY i.SortOrder {sortDirection}, CreatedUtc {sortDirection};";
			}

			int offsetRows = (pageSize * pageNumber) - pageSize;

			var sqlParams = new SqliteParameter[]
			{
				new SqliteParameter(":PageSize", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
				new SqliteParameter(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
				new SqliteParameter(":SearchTerm", DbType.String, 255) { Direction = ParameterDirection.Input, Value = "%" + searchTerm + "%"},
				new SqliteParameter(":SearchField", DbType.String, 50) { Direction = ParameterDirection.Input, Value = searchField },
				new SqliteParameter(":ModuleGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = moduleGuid }
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
            //string sortField = "",
            string sortDirection = "ASC")
		{
            string commandText;
            sortDirection = santizeSortDirection(sortDirection);

            if (string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(searchTerm))
            {
                commandText = $@"
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
ORDER BY SortOrder {sortDirection}";
            }
            else if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(searchTerm))
            {
                commandText = $@"
SELECT ItemID, TotalRows = Count(*) OVER ()
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

SELECT TotalRows, i.*, v.FieldValue, f.FieldName, v.FieldGuid
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
DROP TABLE ItemsToGet;";
            }
            else
            {
                commandText = $@"
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
                    ORDER BY SorrtOrder {sortDirection}";
            }
            int offsetRows = (pageSize * pageNumber) - pageSize;

            var commandParameters = new SqliteParameter[]
            {
                new SqliteParameter(":PageSize", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
                new SqliteParameter(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
                new SqliteParameter(":SearchTerm", DbType.String, 255) { Direction = ParameterDirection.Input, Value = searchTerm },
                new SqliteParameter(":SearchField", DbType.String, 50) { Direction = ParameterDirection.Input, Value = searchField },
                new SqliteParameter(":ModuleGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = moduleGuid.ToString() },
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

			//var commandText = $@"
			//    IF {sortDirection} = 'DESC'
			//        SELECT SiteGuid
			//               FeatureGuid,
			//               i.ModuleGuid,
			//               i.ModuleID,
			//               DefinitionGuid,
			//               ItemGuid,
			//               ItemID,
			//               i.SortOrder,
			//               CreatedUtc,
			//               LastModUtc,
			//               i.ViewRoles,
			//               i.EditRoles,
			//               ms.SettingValue AS GlobalViewSortOrder
			//        FROM i7_sflexi_items i
			//        LEFT JOIN mp_ModuleSettings ms ON ms.ModuleGuid = i.ModuleGuid
			//        WHERE DefinitionGuid = :DefinitionGuid
			//        AND i.SiteGuid = :SiteGuid
			//        AND ms.SettingName = 'GLobalViewSortOrder'
			//        ORDER BY GlobalViewSortOrder DESC,
			//                 i.SortOrder DESC,
			//                 i.CreatedUtc DESC
			//    ELSE IF {sortDirection} = 'ASC'
			//        SELECT
			//            SiteGuid,
			//            FeatureGuid,
			//            i.ModuleGuid,
			//            i.ModuleID,
			//            DefinitionGuid,
			//            ItemGuid,
			//            ItemID,
			//            i.SortOrder,
			//            CreatedUtc,
			//            LastModUtc,
			//            i.ViewRoles,
			//            i.EditRoles,
			//            ms.SettingValue AS GlobalViewSortOrder
			//        FROM i7_sflexi_items i
			//        LEFT JOIN mp_ModuleSettings ms ON ms.ModuleGuid = i.ModuleGuid
			//        WHERE 
			//            DefinitionGuid   = :DefinitionGuid
			//            AND i.SiteGuid   = :SiteGuid
			//            AND ms.SettingName = 'GlobalViewSortOrder'
			//        ORDER BY 
			//            GlobalViewSortOrder,
			//            i.SortOrder,
			//            i.CreatedUtc;";

			string sqlCommand = $@"
				SELECT i.*, ms.SettingValue AS GlobalViewSortOrder, Count(i.*) AS TotalRows
				FROM i7_sflexi_items i
				LEFT JOIN mp_ModuleSettings ms ON ms.ModuleGuid = i.ModuleGuid
				WHERE DefinitionGuid = :DefGuid AND i.SiteGuid = :SiteGuid AND ms.SettingName = 'GlobalViewSortOrder' 
				ORDER BY GlobalViewSortOrder {sortDirection}, i.ModuleID {sortDirection}, SortOrder {sortDirection}, CreatedUtc {sortDirection})
				LIMIT :PageSize {(pageNumber > 1 ? "OFFSET :OffsetRows" : string.Empty)};";

			int offsetRows = (pageSize * pageNumber) - pageSize;

			var sqlParams = new SqliteParameter[]
            {
                new SqliteParameter(":DefGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() },
                new SqliteParameter(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
				new SqliteParameter(":PageSize", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
				new SqliteParameter(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
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
			string sortDirection = "ASC"
            )
        {
			string sqlCommand;
			sortDirection = santizeSortDirection(sortDirection);

			//        string commandText = $@"
			//IF {sortDirection} = 'DESC' 
			//	SELECT
			//                    i.*
			//                    ms.SettingValue AS GlobalViewSortOrder,
			//                    f.Name          AS FieldName,
			//                    v.FieldValue,
			//                    v.FieldGuid
			//	FROM i7_sflexi_items i
			//                LEFT JOIN mp_ModuleSettings ms ON	ms.ModuleGuid = i.ModuleGuid
			//                LEFT JOIN i7_sflexi_values v ON v.ItemGuid = i.ItemGuid
			//                LEFT JOIN i7_sflexi_fields f ON f.FieldGuid = v.FieldGuid
			//	WHERE i.DefinitionGuid = :DefinitionGuid
			//                AND i.SiteGuid   = :SiteGuid
			//                AND ms.SettingName = 'GlobalViewSortOrder'
			//	ORDER BY
			//                    GlobalViewSortOrder DESC,
			//                    i.SortOrder DESC,
			//                    i.CreatedUtc DESC
			//ELSE IF {sortDirection} = 'ASC' 
			//	SELECT
			//                    i.*
			//                    ms.SettingValue AS GlobalViewSortOrder,
			//                    f.Name          AS FieldName,
			//                    v.FieldValue,
			//                    v.FieldGuid
			//	FROM i7_sflexi_items i
			//                LEFT JOIN mp_ModuleSettings ms ON ms.ModuleGuid = i.ModuleGuid
			//                LEFT JOIN i7_sflexi_values v ON v.ItemGuid = i.ItemGuid
			//                LEFT JOIN i7_sflexi_fields f ON f.FieldGuid = v.FieldGuid
			//	WHERE i.DefinitionGuid = :DefinitionGuid
			//                AND i.SiteGuid   = :SiteGuid
			//                AND ms.SettingName = 'GlobalViewSortOrder'
			//	ORDER BY
			//                    GlobalViewSortOrder,
			//                    i.SortOrder,
			//                    i.CreatedUtc;";

			if (pageSize > -1)
			{
				//query with paging
				sqlCommand = $@"
					SELECT pg.*, f.Name AS FieldName, v.FieldValue
					FROM (
						SELECT i.*, ms.SettingValue AS GlobalViewSortOrder, COUNT(i.*) AS TotalRows
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
						ORDER BY GlobalViewSortOrder {sortDirection}, i.ModuleID {sortDirection}, i.SortOrder {sortDirection}, i.CreatedUtc {sortDirection}
				 		LIMIT :PageSize
				 		OFFSET :OffsetRows) pg
					JOIN i7_sflexi_values v ON  v.ItemGuid = pg.ItemGuid
					JOIN i7_sflexi_fields f ON v.FieldGuid = f.FieldGuid
					LEFT JOIN mp_ModuleSettings ms ON ms.ModuleGuid = pg.ModuleGuid
					WHERE TotalRows > 0;";
			}
			else
			{
				//query without paging
				sqlCommand = $@"
					SELECT i.*, f.Name AS FieldName, v.FieldValue, ms.SettingValue AS GlobalViewSortOrder
					FROM i7_sflexi_items i
					JOIN i7_sflexi_values v ON v.ItemGuid = i.ItemGuid
					JOIN i7_sflexi_fields f ON v.FieldGuid = f.FieldGuid
					LEFT JOIN mp_ModuleSettings ms ON ms.ModuleGuid = i.ModuleGuid
					WHERE DefinitionGuid = :DefinitionGuid AND i.SiteGuid = :SiteGuid AND ms.SettingName = 'GlobalViewSortOrder' 
					{(!string.IsNullOrWhiteSpace(searchTerm) ? "AND v.FieldValue LIKE :SearchTerm" : string.Empty)}
					{(!string.IsNullOrWhiteSpace(searchField) ? "AND f.Name = :SearchField" : string.Empty)}
					ORDER BY GlobalViewSortOrder {sortDirection}, i.ModuleID {sortDirection}, i.SortOrder {sortDirection}, i.CreatedUtc {sortDirection};";
			}

			var offsetRows = (pageSize * pageNumber) - pageSize;

			var sqlParams = new SqliteParameter[]
            {
				new SqliteParameter(":PageSize", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
				new SqliteParameter(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
				new SqliteParameter(":SearchTerm", DbType.String, 255) { Direction = ParameterDirection.Input, Value = "%" + searchTerm + "%"},
				new SqliteParameter(":SearchField", DbType.String, 50) { Direction = ParameterDirection.Input, Value = searchField },
                new SqliteParameter(":DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = defGuid.ToString() },
                new SqliteParameter(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() }
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
                    commandText = $@"
					    SELECT TOP (:PageSize) *
					    FROM
                            (
                                SELECT
                                    RowID     = ROW_NUMBER() OVER (ORDER BY i.SortOrder),
                                    TotalRows = Count(*) OVER (),
                                    i.*,
                                    v.FieldValue,
                                    f.Name AS FieldName,
                                    v.FieldGuid
                                FROM i7_sflexi_items i
                                JOIN
                                    (
                                        SELECT DISTINCT
                                                        ItemGuid,
                                                        FieldValue,
                                                        FieldGuid
                                        FROM i7_sflexi_values
                                        WHERE FieldValue LIKE '%' + :SearchTerm + '%'
                                    ) v ON v.ItemGuid = i.ItemGuid
                                JOIN i7_sflexi_fields f ON f.FieldGuid = v.FieldGuid
                                WHERE i.DefinitionGuid = :DefGuid
                                AND i.SiteGuid   = :SiteGuid
                            ) a
					    WHERE a.RowID > ((:PageNumber - 1) * :PageSize)
					    ORDER BY SortOrder {sortDirection};";
                }
                else if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(searchTerm))
                {
                    commandText = $@"
					    SELECT TOP (:PageSize) *
					    FROM
                            (
                                SELECT
                                    RowID     = Row_number() OVER ( ORDER BY i.SortOrder),
                                    TotalRows = Count(*) OVER (),
                                    i.*,
                                    v.FieldValue,
                                    f.FieldName,
                                    v.FieldGuid
                                FROM i7_sflexi_items i
                                JOIN
                                    (
                                        SELECT DISTINCT
                                                        ItemGuid,
                                                        FieldGuid,
                                                        FieldValue
                                        FROM i7_sflexi_value
                                        WHERE fieldvalue LIKE '%' + :SearchTerm + '%'
                                    ) v ON v.itemguid = i.itemguid
                                        JOIN
                                            (
                                                SELECT DISTINCT fieldguid, Name AS FieldName
                                                FROM i7_sflexi_field
                                                WHERE NAME               = :SearchField
                                                AND DefinitionGuid = :DefGuid
                                            ) f ON f.fieldguid = v.fieldguid
                                WHERE i.DefinitionGuid = :DefGuid
                                AND i.SiteGuid   = :SiteGuid
                            ) a
					    WHERE a.rowid > ( ( :PageNumber - 1 ) * :PageSize )
					    ORDER BY SortOrder {sortDirection}
				    {(pageNumber > 1 ? "OFFSET :OffsetRows" : string.Empty)};";
                }
                else
                {
                    commandText = $@"
					    SELECT TOP (@PageSize) *
					    FROM
                            (
                                SELECT
                                        RowID     = ROW_NUMBER() OVER (ORDER BY i.SortOrder),
                                        TotalRows = Count(*) OVER (),
                                        i.*,
                                        v.FieldValue,
                                        f.Name AS FieldName,
                                        v.FieldGuid
                                FROM i7_sflexi_items i
                                JOIN i7_sflexi_values v ON v.ItemGuid = i.ItemGuid
                                JOIN i7_sflexi_fields f ON f.FieldGuid = v.FieldGuid
                                WHERE i.DefinitionGuid = :DefGuid
                                AND i.SiteGuid   = :SiteGuid
                            ) a
					    WHERE a.RowID > ((:PageNumber - 1) * :PageSize)
					    ORDER BY SortOrder {sortDirection}
					    {(pageNumber > 1 ? "OFFSET :OffsetRows" : string.Empty)};";
                }

                var offsetRows = (pageSize * pageNumber) - pageSize;

                var commandParameters = new SqliteParameter[]
                {
                    new SqliteParameter(":PageSize", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
                    new SqliteParameter(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
                    new SqliteParameter(":SearchTerm", DbType.String, 255) { Direction = ParameterDirection.Input, Value = searchTerm },
                    new SqliteParameter(":SearchField", DbType.String, 50) { Direction = ParameterDirection.Input, Value = searchField },
                    new SqliteParameter(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
                    new SqliteParameter(":DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() },
                };

                return SqliteHelper.ExecuteReader(
                  ConnectionString.GetWriteConnectionString(),
                  commandText,
                  commandParameters);
        }


        public static int GetHighestSortOrder(int moduleId)
        {
            string commandText = $"SELECT MAX(SortOrder) FROM i7_sflexi_items WHERE moduleId = :moduleid;";

            var commandParameters = new SqliteParameter[]
            {
                new SqliteParameter(":ModuleId", DbType.Int32) { Direction = ParameterDirection.Input, Value = moduleId },
            };

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                commandText,
                commandParameters));
        }
    }
}


