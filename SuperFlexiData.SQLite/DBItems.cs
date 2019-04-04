// Created:					2018-01-02
// Last Modified:			2019-04-04

using mojoPortal.Data;
using Mono.Data.Sqlite;
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

		/// <summary>
		/// Gets a count of rows in the i7_sflexi_items table.
		/// </summary>
		public static int GetCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("select count(*) from i7_sflexi_items;");

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                 ConnectionString.GetReadConnectionString(),

				 sqlCommand.ToString()));
        }
		public static int GetCountForModule(int moduleId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append($"select count(*) from i7_sflexi_items where ModuleId = {moduleId};");

			return Convert.ToInt32(SqliteHelper.ExecuteScalar(
				 ConnectionString.GetReadConnectionString(),

				 sqlCommand.ToString()));
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("select * from i7_sflexi_items where ModuleId = :ModuleId order by SortOrder asc;");

            var sqlParam = new SqliteParameter(":ModuleId", DbType.Int32) { Direction = ParameterDirection.Input, Value = moduleID };

            return SqliteHelper.ExecuteReader(
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
    }

}


