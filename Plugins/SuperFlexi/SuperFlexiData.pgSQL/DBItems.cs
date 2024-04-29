using mojoPortal.Data;
using Npgsql;
using NpgsqlTypes;
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
            DateTime lastModUtc,
			string viewRoles,
			string editRoles)
        {
            var sqlCommand = @"
				insert into i7_sflexi_items 
					(itemguid
					 ,siteguid 
					 ,featureguid
					 ,moduleguid 
					 ,moduleid
					 ,definitionguid
					 ,sortorder
					 ,createdutc 
					 ,lastmodutc
					 ,viewroles
					 ,editroles)
				values (:itemguid
					 ,:siteguid
					 ,:featureguid
					 ,:moduleguid
					 ,:moduleid
					 ,:definitionguid
					 ,:sortorder
					 ,:createdutc
					 ,:lastmodutc
					 ,:viewroles
					 ,:editroles)
				returning itemid;";
            
            var sqlParams = new List<NpgsqlParameter>
            {
                new NpgsqlParameter(":itemguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = itemGuid },
                new NpgsqlParameter(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid },
                new NpgsqlParameter(":featureguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = featureGuid },
                new NpgsqlParameter(":moduleguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = moduleGuid },
                new NpgsqlParameter(":moduleid", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = moduleID },
                new NpgsqlParameter(":definitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = definitionGuid },
                new NpgsqlParameter(":sortorder", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = sortOrder },
                new NpgsqlParameter(":createdutc", NpgsqlDbType.Timestamp) { Direction = ParameterDirection.Input, Value = createdUtc },
                new NpgsqlParameter(":lastmodutc", NpgsqlDbType.Timestamp) { Direction = ParameterDirection.Input, Value = lastModUtc },
                new NpgsqlParameter(":viewroles", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input, Value = viewRoles },
                new NpgsqlParameter(":editroles", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input, Value = editRoles }
			};

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
				sqlCommand,
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
            DateTime lastModUtc,
			string viewRoles,
			string editRoles)
        {
            var sqlCommand = @"
				update i7_sflexi_items
				set siteguid = :siteguid
				   ,featureguid = :featureguid
				   ,moduleguid = :moduleguid
				   ,moduleid = :moduleid
				   ,definitionguid = :definitionguid
				   ,sortorder = :sortorder
				   ,createdutc = :createdutc
				   ,lastmodutc = :lastmodutc
				   ,viewroles = :viewroles
				   ,editroles = :editroles
				where itemguid = :itemguid;";

            var sqlParams = new List<NpgsqlParameter>
            {
                new NpgsqlParameter(":itemguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = itemGuid },
                new NpgsqlParameter(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid },
                new NpgsqlParameter(":featureguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = featureGuid },
                new NpgsqlParameter(":moduleguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = moduleGuid },
                new NpgsqlParameter(":moduleid", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = moduleID },
                new NpgsqlParameter(":definitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = definitionGuid },
                new NpgsqlParameter(":sortorder", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = sortOrder },
                new NpgsqlParameter(":createdutc", NpgsqlDbType.Timestamp) { Direction = ParameterDirection.Input, Value = createdUtc },
                new NpgsqlParameter(":lastmodutc", NpgsqlDbType.Timestamp) { Direction = ParameterDirection.Input, Value = lastModUtc },
				new NpgsqlParameter(":viewroles", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input, Value = viewRoles },
				new NpgsqlParameter(":editroles", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input, Value = editRoles }
			};

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("delete from i7_sflexi_items where itemid = :itemid;");

            var sqlParam = new NpgsqlParameter(":itemid", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = itemID };

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("delete from i7_sflexi_items where siteguid = :siteguid;");

            var sqlParam = new NpgsqlParameter(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid };
            
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("delete from i7_sflexi_items where moduleguid = :moduleguid;");

            var sqlParam = new NpgsqlParameter(":moduleguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = moduleGuid };

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
    //        sqlCommand.Append("delete from i7_sflexi_items where definitionguid = :definitionguid;");

    //        var sqlParam = new NpgsqlParameter(":definitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = definitionGuid };

    //        int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
    //            ConnectionString.GetWriteConnectionString(),
    //            CommandType.Text,
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
            sqlCommand.Append("select * from i7_sflexi_items where itemguid = :itemguid;");

            var sqlParam = new NpgsqlParameter(":itemguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = itemGuid };

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
				sqlCommand.ToString(),
                sqlParam);
        }

		/// <summary>
		/// Gets an IDataReader with one row from the i7_sflexi_items table.
		/// </summary>
		/// <param name="itemID"> itemID </param>
		public static IDataReader GetOne(int itemId)
		{
			var sqlCommand = "select * from i7_sflexi_items where itemid = :itemid;";

			var sqlParam = new NpgsqlParameter(":itemid", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = itemId };

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
				sqlCommand,
				sqlParam);
		}

		/// <summary>
		/// Gets an IDataReader with item and values
		/// </summary>
		public static IDataReader GetOneWithValues(int itemId)
		{
			string sqlCommand = $@"
					SELECT i.*, f.Name AS FieldName, v.FieldValue
					FROM i7_sflexi_items i
					JOIN i7_sflexi_values v ON v.ItemGuid = i.ItemGuid
					JOIN i7_sflexi_fields f ON v.FieldGuid = f.FieldGuid
					WHERE i.ItemID = ?ItemID;";

			var sqlParam = new NpgsqlParameter("?ItemID", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = itemId };

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
                CommandType.Text,
				sqlCommand,
				sqlParam
			);
		}

		public static IDataReader GetByIDsWithValues(Guid defGuid, Guid siteGuid, List<int> itemIDs, int pageNumber = 1, int pageSize = 20)
		{

			string sqlCommand = $@"

					SELECT pg.*, f.Name AS FieldName, v.FieldValue
					FROM (
						SELECT i.*, COUNT(*) AS TotalRows
						FROM i7_sflexi_items i
						WHERE {getItems()}
						AND i.DefinitionGuid = ?DefinitionGuid
						AND i.SiteGuid = ?SiteGuid
						GROUP BY ItemID
				 		LIMIT ?PageSize
				 		OFFSET ?OffSetRows) pg
					JOIN i7_sflexi_values v ON  v.ItemGuid = pg.ItemGuid
					JOIN i7_sflexi_fields f ON v.FieldGuid = f.FieldGuid
					WHERE TotalRows > 0";

			string getItems()
			{
				return string.Join(" ", itemIDs.Select((x, i) =>
				{
					var item = $"ItemID = ?ItemID{i}";

					if (i < itemIDs.Count() - 1)
					{
						item += " OR";
					}

					return item;
				}));
			}

			var sqlParams = new List<NpgsqlParameter>();

			for (var i = 0; i < itemIDs.Count(); i++)
			{
				sqlParams.Add(new NpgsqlParameter($"?ItemID{i}", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = itemIDs[i] });
			}

			int offsetRows = (pageSize * pageNumber) - pageSize;

			sqlParams.AddRange(new List<NpgsqlParameter>()
			{
				new NpgsqlParameter("?PageSize", NpgsqlDbType.Integer){ Direction = ParameterDirection.Input, Value = pageSize },
				new NpgsqlParameter("?OffsetRows", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = offsetRows },
				new NpgsqlParameter("?DefinitionGuid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = defGuid },
				new NpgsqlParameter("?SiteGuid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid }
			});

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand,
				sqlParams.ToArray()
			);
		}

		/// <summary>
		/// Gets a count of rows in the i7_sflexi_items table.
		/// </summary>
		public static int GetCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("select count(*) from i7_sflexi_items;");

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                 ConnectionString.GetReadConnectionString(),
                CommandType.Text,
				 sqlCommand.ToString()));
        }
		public static int GetCountForModule(int moduleId)
		{
			var sqlCommand = "select count(*) from i7_sflexi_items where moduleid = :moduleid;";
			var sqlParam = new NpgsqlParameter(":moduleid", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = moduleId };

			return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
				 ConnectionString.GetReadConnectionString(),
                CommandType.Text,
				 sqlCommand,
				 sqlParam));
		}
		
		/// <summary>
		/// Gets an IDataReader with all rows in the i7_sflexi_items table.
		/// </summary>
		public static IDataReader GetAll(Guid siteGuid)
        {
			var sqlCommand = "select * from i7_sflexi_items where siteguid = :siteguid;";
			var sqlParam = new NpgsqlParameter(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid };
            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("select * from i7_sflexi_items where moduleid = :moduleid order by sortorder asc;");

            var sqlParam = new NpgsqlParameter(":moduleid", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = moduleID };

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
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
				sqlCommand.Append(@"select row_number() over (order by sortorder) as rowid
					, count(*) over() as totalrows
					, i.*
					from i7_sflexi_items i
						join(
							select distinct itemguid
							from i7_sflexi_values
							where fieldvalue like '%:searchterm%'
							) v on v.itemguid = i.itemguid
						where moduleguid = ':moduleguid' 
						order by sortorder :sortdirection
						limit :pagesize " + (pageNumber > 1 ? "offset :offsetrows;" : ";"));
			}
			else if (!String.IsNullOrWhiteSpace(searchField) && !String.IsNullOrWhiteSpace(searchTerm))
			{
				sqlCommand.Append(@"select row_number() over (order by sortorder) as rowid
					, count(*) over() as totalrows
					, i.*
					from i7_sflexi_items i
						join(
							select distinct itemguid, fieldguid
							from i7_sflexi_values
							where fieldvalue like '%:searchterm%'
							) v on v.itemguid = i.itemguid
						join(
							select distinct fieldguid
							from i7_sflexi_fields
							where name = :searchfield
							) f on f.fieldguid = v.fieldguid
						where moduleguid = :moduleguid
						order by sortorder :sortdirection
						limit :pagesize " + (pageNumber > 1 ? "offset :offsetrows;" : ";"));
			}
			else
			{
				sqlCommand.Append(@"select row_number() over (order by sortorder) as rowid
					, count(*) over() as totalrows
					, i.*
					from i7_sflexi_items i
					where moduleguid = ':moduleguid' 
					order by sortorder :sortdirection
					limit :pagesize " + (pageNumber > 1 ? "offset :offsetrows;" : ";"));
			}

			int offsetRows = (pageSize * pageNumber) - pageSize;

			var sqlParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":pagesize", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = pageSize },
				new NpgsqlParameter(":offsetrows", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = offsetRows },
				new NpgsqlParameter(":searchterm", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = searchTerm },
				new NpgsqlParameter(":searchfield", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = searchField },
				new NpgsqlParameter(":moduleguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = moduleGuid },
				new NpgsqlParameter(":sortdirection", NpgsqlDbType.Varchar, 4) { Direction = ParameterDirection.Input, Value = descending ? "desc" : "asc" }
			};

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
                CommandType.Text,
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
				sqlCommand.Append(@"select row_number() over (order by sortorder) as rowid
					, count(*) over() as totalrows
					, i.*
					from i7_sflexi_items i
						join(
							select distinct itemguid
							from i7_sflexi_values
							where fieldvalue like '%:searchterm%'
							) v on v.itemguid = i.itemguid
						where definitionguid = ':defguid' and siteguid = ':siteguid'
						order by sortorder :sortdirection
						limit :pagesize " + (pageNumber > 1 ? "offset :offsetrows;" : ";"));
			}
			else if (!String.IsNullOrWhiteSpace(searchField) && !String.IsNullOrWhiteSpace(searchTerm))
			{
				sqlCommand.Append(@"select row_number() over (order by sortorder) as rowid
					, count(*) over() as totalrows
					, i.*
					from i7_sflexi_items i
						join(
							select distinct itemguid, fieldguid
							from i7_sflexi_values
							where fieldvalue like '%:searchterm%'
							) v on v.itemguid = i.itemguid
						join(
							select distinct fieldguid
							from i7_sflexi_fields
							where name = :searchfield
							) f on f.fieldguid = v.fieldguid
						where definitionguid = ':defguid' and siteguid = ':siteguid'
						order by sortorder :sortdirection
						limit :pagesize " + (pageNumber > 1 ? "offset :offsetrows;" : ";"));
			}
			else
			{
				sqlCommand.Append(@"select row_number() over (order by sortorder) as rowid
					, count(*) over() as totalrows
					, i.*
					from i7_sflexi_items i
					where definitionguid = ':defguid' and siteguid = ':siteguid'
					order by sortorder :sortdirection
					limit :pagesize " + (pageNumber > 1 ? "offset :offsetrows;" : ";"));
			}

			int offsetRows = (pageSize * pageNumber) - pageSize;

			var sqlParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":pagesize", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = pageSize },
				new NpgsqlParameter(":offsetrows", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = offsetRows },
				new NpgsqlParameter(":searchterm", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = searchTerm },
				new NpgsqlParameter(":searchfield", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = searchField },
				new NpgsqlParameter(":defguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = defGuid },
				new NpgsqlParameter(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid },
				new NpgsqlParameter(":sortdirection", NpgsqlDbType.Varchar, 4) { Direction = ParameterDirection.Input, Value = descending ? "desc" : "asc" }
			};

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
                CommandType.Text,
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
                siteguid, 
                featureguid, 
                i.moduleguid, 
                i.moduleid, 
                definitionguid, 
                itemguid, 
                itemid, 
                i.sortorder, 
                createdutc, 
                lastmodutc, 
                ms.settingvalue as globalviewsortorder 
                from i7_sflexi_items i
                left join mp_modulesettings ms on ms.moduleguid = i.moduleguid
                where definitionguid = :defguid and i.siteguid = :siteguid and ms.settingname = 'GlobalViewSortOrder' 
                order by globalviewsortorder asc, i.moduleid asc, sortorder asc, createdutc asc;";

			var sqlParam = new List<NpgsqlParameter>{
				new NpgsqlParameter(":defguid", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input, Value = definitionGuid },
				new NpgsqlParameter(":siteguid", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input, Value = siteGuid }
			};

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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

    //        sqlCommand.Append("select * from i7_sflexi_items limit :pagesize" + (pageNumber > 1 ? "offset :offsetrows;" : ";"));

    //        var sqlParams = new List<NpgsqlParameter>
    //        {
    //            new NpgsqlParameter(":pagesize", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = pageSize },
    //            new NpgsqlParameter(":offsetrows", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = pageLowerBound }
    //        };

    //        return NpgsqlHelper.ExecuteReader(
    //            ConnectionString.GetReadConnectionString(),
    //            CommandType.Text,
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
                 i.moduleid as moduleid
                ,i.itemguid as itemguid
                ,i.itemid as itemid
                ,i.sortorder as sortorder
                ,i.createdutc as createdutc
                ,m.moduletitle as moduletitle
                ,m.viewroles as viewroles
                ,pm.publishbegindate as publishbegindate
                ,pm.publishenddate as publishenddate
                from i7_sflexi_items i 
                join mp_pagemodules pm on i.moduleguid = pm.moduleguid 
                join mp_modules m on i.moduleguid = m.guid 
                where i.siteguid = :siteguid 
                and pm.pageid = :pageid 
                order by sortorder asc;");

            var sqlParams = new List<NpgsqlParameter>
            {
                new NpgsqlParameter(":siteguid", NpgsqlDbType.Varchar) { Direction = ParameterDirection.Input, Value = siteGuid },
                new NpgsqlParameter(":pageid", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = pageId }
            };

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
				sqlCommand.ToString(),
                sqlParams.ToArray());
        }


        /// <summary>
        /// Gets an IDataReader with all items for module.
        /// </summary>
        /// <param name="itemID"> itemID </param>
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
						SELECT *, COUNT(*) OVER() AS TotalRows
						FROM i7_sflexi_items i
						WHERE i.ModuleID = :ModuleID ) t
					WHERE TotalRows > 0
					ORDER BY t.SortOrder {sortDirection}
					{(pageSize > -1 ? "LIMIT :PageSize" : string.Empty)} 
					{(pageNumber > 1 ? "OFFSET :OffsetRows" : string.Empty)};";

			int offsetRows = (pageSize * pageNumber) - pageSize;

			var commandParameters = new NpgsqlParameter[]
			{
				new NpgsqlParameter(":ModuleID", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = moduleID },
				new NpgsqlParameter("?PageSize", NpgsqlDbType.Integer){ Direction = ParameterDirection.Input, Value = pageSize },
				new NpgsqlParameter("?OffsetRows", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = offsetRows },
			};

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
			string commandText;

			sortDirection = santizeSortDirection(sortDirection);
			if (pageSize > 0)
			{
				//query with paging
				commandText = $@"		
					SELECT pg.*, f.Name AS FieldName, v.FieldValue
					FROM (
						SELECT i.*, COUNT(*) OVER() AS TotalRows
						FROM i7_sflexi_items i
						WHERE i.ItemGuid IN (
							SELECT DISTINCT ItemGuid
							FROM i7_sflexi_values v
							{(!string.IsNullOrWhiteSpace(searchField) ? "JOIN i7_sflexi_fields f ON f.FieldGuid = v.FieldGuid" : string.Empty)}
							WHERE v.ItemGuid = i.ItemGuid
							{(!string.IsNullOrWhiteSpace(searchTerm) ? "AND v.FieldValue LIKE ?SearchTerm" : string.Empty)}
							{(!string.IsNullOrWhiteSpace(searchField) ? "AND f.Name = ?SearchField" : string.Empty)}
						) 
						AND i.ModuleGuid = ?ModuleGuid
						GROUP BY ItemID
						ORDER BY SortOrder {sortDirection}, CreatedUtc {sortDirection}
				 		LIMIT ?PageSize
				 		OFFSET ?OffSetRows) pg
					JOIN i7_sflexi_values v ON  v.ItemGuid = pg.ItemGuid
					JOIN i7_sflexi_fields f ON v.FieldGuid = f.FieldGuid
					WHERE TotalRows > 0;";
			}
			else 
			{
				commandText = $@"
					SELECT *, f.Name AS FieldName, v.FieldValue
					FROM public.i7_sflexi_items i
					JOIN public.i7_sflexi_values v ON v.ItemGuid = i.ItemGuid
					JOIN public.i7_sflexi_fields f ON f.FieldGuid = v.FieldGuid
					WHERE i.ModuleGuid = :ModuleGuid
					{(!string.IsNullOrWhiteSpace(searchTerm) ? "AND v.FieldValue LIKE ?SearchTerm" : string.Empty)}
					{(!string.IsNullOrWhiteSpace(searchField) ? "AND f.Name = ?SearchField" : string.Empty)}
					ORDER BY i.SortOrder {sortDirection}";
			}

			int offsetRows = (pageSize * pageNumber) - pageSize;

			var commandParameters = new NpgsqlParameter[]
			{
				new NpgsqlParameter("?PageSize", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = pageSize },
				new NpgsqlParameter("?OffsetRows", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = offsetRows },
				new NpgsqlParameter("?SearchTerm", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = "%" + searchTerm + "%"},
				new NpgsqlParameter("?SearchField", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = searchField },
				new NpgsqlParameter(":ModuleGuid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = moduleGuid }
			};

            return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				commandText,
				commandParameters
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
			string commandText;
			
			sortDirection = santizeSortDirection(sortDirection);
			
			if (string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(searchTerm))
			{
				commandText = $@"
					SELECT TOP (:PageSize) *
					FROM
					(
						SELECT  RowID     = ROW_NUMBER() OVER (ORDER BY i.SortOrder),
								TotalRows = Count(*) OVER (),
								i.*,
								v.FieldValue,
								f.Name AS FieldName,
								v.FieldGuid
						FROM public.i7_sflexi_items i
							JOIN
							(
								SELECT DISTINCT ItemGuid, FieldValue, FieldGuid
								FROM public.i7_sflexi_values
								WHERE FieldValue LIKE '%' + :SearchTerm + '%'
							)
							v
								ON v.ItemGuid = i.ItemGuid
							JOIN public.i7_sflexi_fields f
								ON f.FieldGuid = v.FieldGuid
						WHERE ModuleGuid = :ModuleGuid
					)
					a
					WHERE a.RowID > ((:PageNumber - 1) * :PageSize)
					ORDER BY SortOrder {sortDirection}";
			}
			else if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(searchTerm))
			{
				commandText = $@"
					SELECT ItemID, TotalRows = Count(*) OVER()
					INTO TEMP TABLE ItemsToGet
					FROM public.i7_sflexi_values v
						JOIN
						(
							SELECT DISTINCT FieldGuid, Name AS FieldName
							FROM public.i7_sflexi_fields
							WHERE Name = :SearchField
						) f ON f.FieldGuid = v.FieldGuid
					JOIN i7_sflexi_items items on items.ItemGuid = v.ItemGuid
					WHERE FieldValue LIKE '%' + :SearchTerm + '%'
					AND v.ModuleGuid = :ModuleGuid
					ORDER BY SortOrder {sortDirection}
					OFFSET ((:PageNumber - 1) * :PageSize) ROWS
					FETCH NEXT :PageSize ROWS ONLY;

					SELECT TotalRows, i.*, v.FieldValue, f.FieldName, v.FieldGuid
					FROM public.i7_sflexi_items i
						JOIN
						(
							SELECT DISTINCT ItemGuid,
							FieldGuid,
							FieldValue
							FROM public.i7_sflexi_values
						) v ON v.ItemGuid = i.ItemGuid
						JOIN 
						(
							SELECT DISTINCT FieldGuid, Name AS FieldName
							FROM public.i7_sflexi_fields
						) f ON f.FieldGuid = v.FieldGuid
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
					WHERE a.RowID > ((:PageNumber - 1) * :PageSize)
					ORDER BY SortOrder {sortDirection}";
			}

			int offsetRows = (pageSize * pageNumber) - pageSize;

			var commandParameters = new NpgsqlParameter[]
			{
				new NpgsqlParameter(":PageSize", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = pageSize },
				new NpgsqlParameter(":OffsetRows", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = offsetRows },
				new NpgsqlParameter(":SearchTerm", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = "%" + searchTerm + "%"},
				new NpgsqlParameter(":SearchField", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = searchField },
				new NpgsqlParameter(":moduleguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = moduleGuid },

			};

            return NpgsqlHelper.ExecuteReader(
                           ConnectionString.GetWriteConnectionString(),
                           CommandType.Text,
						   commandText.ToString(),
						   commandParameters);
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
			//string sqlCommand = $@"
			//	IF  {sortDirection} = 'DESC'
			//		SELECT
			//				SiteGuid,
			//				FeatureGuid,
			//				i.ModuleGuid,
			//				i.ModuleID,
			//				DefinitionGuid,
			//				ItemGuid,
			//				ItemID,
			//				i.SortOrder,
			//				CreatedUtc,
			//				LastModUtc,
			//				i.ViewRoles,
			//				i.EditRoles,
			//				ms.SettingValue AS GlobalViewSortOrder
			//		FROM
			//				  public.i7_sflexi_items i
			//				  left join
			//							mp_ModuleSettings ms
			//							ON
			//									  ms.ModuleGuid = i.ModuleGuid
			//		WHERE
			//				  DefinitionGuid   = :DefinitionGuid
			//				  AND i.SiteGuid   = :SiteGuid
			//				  AND ms.SettingName = 'GlobalViewSortOrder'
			//		ORDER BY
			//				GlobalViewSortOrder DESC,
			//				i.SortOrder DESC,
			//				i.CreatedUtc DESC,
			//	ELSE IF {sortDirection} = 'ASC' 
			//		SELECT
			//				SiteGuid,
			//				FeatureGuid,
			//				i.ModuleGuid,
			//				i.ModuleID,
			//				DefinitionGuid,
			//				ItemGuid,
			//				ItemID,
			//				i.SortOrder,
			//				CreatedUtc,
			//				LastModUtc,
			//				i.ViewRoles,
			//				i.EditRoles,
			//				ms.SettingValue AS GlobalViewSortOrder
			//		FROM
			//				  public.i7_sflexi_items i
			//				  left join
			//							mp_ModuleSettings ms
			//							ON
			//									  ms.ModuleGuid = i.ModuleGuid
			//		WHERE
			//				  DefinitionGuid   = :DefinitionGuid
			//				  AND i.SiteGuid   = :SiteGuid
			//				  AND ms.SettingName = 'GlobalViewSortOrder'
			//		ORDER BY
			//				GlobalViewSortOrder,
			//				i.SortOrder,
			//				i.CreatedUtc;";
			string sqlCommand = $@"
				SELECT i.*, ms.SettingValue AS GlobalViewSortOrder, Count(i.*) OVER() AS TotalRows
				FROM i7_sflexi_items i
				LEFT JOIN mp_ModuleSettings ms ON ms.ModuleGuid = i.ModuleGuid
				WHERE DefinitionGuid = ?DefGuid AND i.SiteGuid = ?SiteGuid AND ms.SettingName = 'GlobalViewSortOrder' 
				ORDER BY GlobalViewSortOrder {sortDirection}, i.ModuleID {sortDirection}, SortOrder {sortDirection}, CreatedUtc {sortDirection})
				LIMIT ?PageSize {(pageNumber > 1 ? "OFFSET ?OffsetRows" : string.Empty)};";

			int offsetRows = (pageSize * pageNumber) - pageSize;

			var sqlParams = new List<NpgsqlParameter> 
			{
				new NpgsqlParameter("?DefGuid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = definitionGuid },
				new NpgsqlParameter("?SiteGuid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid },
				new NpgsqlParameter("?PageSize", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = pageSize },
				new NpgsqlParameter("?OffsetRows", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = offsetRows },
			};

			return NpgsqlHelper.ExecuteReader(
					ConnectionString.GetWriteConnectionString(),
					CommandType.Text,
					sqlCommand.ToString(),
					sqlParams.ToArray()
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
			sortDirection = santizeSortDirection(sortDirection);
			string sqlCommand;
			//sqlCommand = $@"
			//	IF {sortDirection} = 'DESC' 
			//		SELECT
			//				i.*
			//				ms.SettingValue AS GlobalViewSortOrder,
			//				f.Name          AS FieldName,
			//				v.FieldValue,
			//				v.FieldGuid
			//		FROM
			//				public.i7_sflexi_items i
			//				  LEFT JOIN
			//						mp_ModuleSettings ms
			//							ON
			//									ms.ModuleGuid = i.ModuleGuid
			//				  LEFT JOIN
			//							public.i7_sflexi_values v
			//							ON
			//									v.ItemGuid = i.ItemGuid
			//				  LEFT JOIN
			//						public.i7_sflexi_fields f
			//							ON
			//									f.FieldGuid = v.FieldGuid
			//		WHERE
			//				i.DefinitionGuid = :DefinitionGuid
			//				AND i.SiteGuid   = :SiteGuid
			//				AND ms.SettingName = 'GlobalViewSortOrder'
			//		ORDER BY
			//				GlobalViewSortOrder DESC,
			//				i.SortOrder DESC,
			//				i.CreatedUtc DESC
			//	ELSE IF {sortDirection} = 'ASC' 
			//		SELECT
			//				i.*
			//				ms.SettingValue AS GlobalViewSortOrder,
			//				f.Name          AS FieldName,
			//				v.FieldValue,
			//				v.FieldGuid
			//		FROM
			//				public.i7_sflexi_items i
			//				  LEFT JOIN
			//						mp_ModuleSettings ms
			//							ON
			//									ms.ModuleGuid = i.ModuleGuid
			//				  LEFT JOIN
			//							public.i7_sflexi_values v
			//							ON
			//									v.ItemGuid = i.ItemGuid
			//				  LEFT JOIN
			//							public.i7_sflexi_fields f
			//							ON
			//									f.FieldGuid = v.FieldGuid
			//		WHERE
			//				i.DefinitionGuid = :DefinitionGuid
			//				AND i.SiteGuid   = :SiteGuid
			//				AND ms.SettingName = 'GlobalViewSortOrder'
			//		ORDER BY
			//				GlobalViewSortOrder,
			//				i.SortOrder,
			//				i.CreatedUtc;";

			if (pageSize > -1)
			{
				//query with paging
				sqlCommand = $@"
					SELECT pg.*, f.Name AS FieldName, v.FieldValue
					FROM (
						SELECT i.*, ms.SettingValue AS GlobalViewSortOrder, COUNT(i.*) OVER() AS TotalRows
						FROM i7_sflexi_items i
						LEFT JOIN mp_ModuleSettings ms ON ms.ModuleGuid = i.ModuleGuid
						WHERE i.ItemGuid IN (
							SELECT DISTINCT ItemGuid
							FROM i7_sflexi_values v
							{(!string.IsNullOrWhiteSpace(searchField) ? "JOIN i7_sflexi_fields f ON f.FieldGuid = v.FieldGuid" : string.Empty)}
							WHERE v.ItemGuid = i.ItemGuid
							{(!string.IsNullOrWhiteSpace(searchTerm) ? "AND v.FieldValue LIKE ?SearchTerm" : string.Empty)}
							{(!string.IsNullOrWhiteSpace(searchField) ? "AND f.Name = ?SearchField" : string.Empty)}
						) 
						AND i.DefinitionGuid = ?DefinitionGuid
						AND i.SiteGuid = ?SiteGuid
						AND ms.SettingName = 'GlobalViewSortOrder' 
						ORDER BY GlobalViewSortOrder {sortDirection}, i.ModuleID {sortDirection}, i.SortOrder {sortDirection}, i.CreatedUtc {sortDirection}
				 		LIMIT ?PageSize
				 		OFFSET ?OffSetRows) pg
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
					WHERE DefinitionGuid = ?DefGuid AND i.SiteGuid = ?SiteGuid AND ms.SettingName = 'GlobalViewSortOrder' 
					{(!string.IsNullOrWhiteSpace(searchTerm) ? "AND v.FieldValue LIKE ?SearchTerm" : string.Empty)}
					{(!string.IsNullOrWhiteSpace(searchField) ? "AND f.Name = ?SearchField" : string.Empty)}
					ORDER BY GlobalViewSortOrder {sortDirection}, i.ModuleID {sortDirection}, i.SortOrder {sortDirection}, i.CreatedUtc {sortDirection};";
			}
			var offsetRows = (pageSize * pageNumber) - pageSize;

			var commandParameters = new NpgsqlParameter[]
			{
				new NpgsqlParameter("?PageSize", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = pageSize },
				new NpgsqlParameter("?OffsetRows", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = offsetRows },
				new NpgsqlParameter("?SearchTerm", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = "%" + searchTerm + "%"},
				new NpgsqlParameter("?SearchField", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = searchField },
				new NpgsqlParameter("?DefinitionGuid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = defGuid },
				new NpgsqlParameter("?SiteGuid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid },
			};
			return NpgsqlHelper.ExecuteReader(
					ConnectionString.GetWriteConnectionString(),
					CommandType.Text,
					sqlCommand,
					commandParameters);
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
			string commandText;

			sortDirection = santizeSortDirection(sortDirection);

			if (string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(searchTerm))
			{
				commandText = $@"
					SELECT
							 TOP (:PageSize) *
					FROM
							 (
									  SELECT
											RowID     = ROW_NUMBER() OVER (ORDER BY i.SortOrder),
											TotalRows = Count(*) OVER (),
											i.*,
											v.FieldValue,
											f.Name AS FieldName,
											v.FieldGuid
									  FROM public.i7_sflexi_items i
											   JOIN
														(
															   SELECT DISTINCT
																	ItemGuid,
																	FieldValue,
																	FieldGuid
															   FROM
																	public.i7_sflexi_values
															   WHERE
																	FieldValue LIKE '%' + :SearchTerm + '%'
														)
														v
														ON
																v.ItemGuid = i.ItemGuid
											   JOIN public.i7_sflexi_fields f
														ON
																f.FieldGuid = v.FieldGuid
									  WHERE
											   i.DefinitionGuid = :DefGuid
											   AND i.SiteGuid   = :SiteGuid
							 )
							 a
					WHERE
							 a.RowID > ((:PageNumber - 1) * :PageSize)
					ORDER BY SortOrder {sortDirection};";
			}
			else if (!string.IsNullOrWhiteSpace(searchField) && !string.IsNullOrWhiteSpace(searchTerm))
			{
				commandText = $@"
					SELECT
							 TOP (:PageSize) *
					FROM
							 (
									  SELECT
											RowID     = Row_number() OVER ( ORDER BY i.SortOrder),
											TotalRows = Count(*) OVER (),
											i.*,
											v.FieldValue,
											f.FieldName,
											v.FieldGuid
									  FROM
											   public.i7_sflexi_items i
											   JOIN
														(
															   SELECT DISTINCT
																	ItemGuid,
																	FieldGuid,
																	FieldValue
															   FROM
																	public..i7_sflexi_value
															   WHERE
																	fieldvalue LIKE '%' + :SearchTerm + '%'
														)
														v
														ON
																v.itemguid = i.itemguid
											   JOIN
														(
															   SELECT DISTINCT
																			fieldguid,
																	Name AS FieldName
															   FROM
																	public.i7_sflexi_field
															   WHERE
																	NAME               = :SearchField
																	AND DefinitionGuid = :DefGuid
														)
														f
														ON
																f.fieldguid = v.fieldguid
									  WHERE
											   i.DefinitionGuid = :DefGuid
											   AND i.SiteGuid   = :SiteGuid
							 )
							 a
					WHERE
							a.rowid > ( ( :PageNumber - 1 ) * :PageSize )
					ORDER BY SortOrder {sortDirection}
				{(pageNumber > 1 ? "OFFSET :OffsetRows" : string.Empty)};";
			}
			else
			{
				commandText = $@"
					SELECT
								TOP (@PageSize) *
					FROM
								(
										SELECT
											RowID     = ROW_NUMBER() OVER (ORDER BY i.SortOrder),
											TotalRows = Count(*) OVER (),
											i.*,
											v.FieldValue,
											f.Name AS FieldName,
											v.FieldGuid
										FROM
												public.i7_sflexi_items i
												JOIN
														public.i7_sflexi_values v
														ON
																v.ItemGuid = i.ItemGuid
												JOIN
														public.i7_sflexi_fields f
														ON
																f.FieldGuid = v.FieldGuid
										WHERE
												i.DefinitionGuid = :DefGuid
												AND i.SiteGuid   = :SiteGuid
								)
								a
					WHERE
								a.RowID > ((:PageNumber - 1) * :PageSize)
					ORDER BY SortOrder {sortDirection}
					{(pageNumber > 1 ? "OFFSET :OffsetRows" : string.Empty)};";
			}

			var offsetRows = (pageSize * pageNumber) - pageSize;

			var commandParameters = new NpgsqlParameter[]
			{
				new NpgsqlParameter(":PageSize", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = pageSize },
				new NpgsqlParameter(":OffsetRows", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = offsetRows },
				new NpgsqlParameter(":SearchTerm", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = "%" + searchTerm + "%"},
				new NpgsqlParameter(":SearchField", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = searchField },
				new NpgsqlParameter(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid },
				new NpgsqlParameter(":definitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = defGuid }
				//new NpgsqlParameter(":SortDirection", NpgsqlDbType.Varchar, 4) { Direction = ParameterDirection.Input, Value = sortDirection }
			};

			return NpgsqlHelper.ExecuteReader(
						   ConnectionString.GetWriteConnectionString(),
						   CommandType.Text,
						   commandText,
						   commandParameters);
		}
		/// <summary>
		/// Gets Highest (largest) SortOrder
		/// </summary>
		/// <param name="moduleId"></param>
		/// <returns></returns>
		public static int GetHighestSortOrder(int moduleId)
		{
			string sqlCommand = $"SELECT MAX(SortOrder) FROM public.i7_sflexi_items WHERE moduleId = :moduleid;";

			var sqlParam = new NpgsqlParameter(":moduleid", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = moduleId };

			return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
				 ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				 sqlCommand,
				 sqlParam)
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

}


