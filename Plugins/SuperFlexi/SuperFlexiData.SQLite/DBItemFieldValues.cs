using mojoPortal.Data;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.AppendFormat("insert into i7_sflexi_values ({0}) values ({1});"
                ,@"ValueGuid
                  ,SiteGuid
                  ,FeatureGuid
                  ,ModuleGuid
                  ,ItemGuid
                  ,FieldGuid
                  ,FieldValue"
                ,@":ValueGuid
                  ,:SiteGuid
                  ,:FeatureGuid
                  ,:ModuleGuid
                  ,:ItemGuid
                  ,:FieldGuid
                  ,:FieldValue");

            var sqlParams = new List<SqliteParameter>
            {
                new SqliteParameter(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
                new SqliteParameter(":FeatureGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = featureGuid.ToString() },
                new SqliteParameter(":ModuleGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = moduleGuid.ToString() },
                new SqliteParameter(":ItemGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = itemGuid.ToString() },
                new SqliteParameter(":FieldGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = fieldGuid.ToString() },
                new SqliteParameter(":ValueGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = valueGuid.ToString() },
                new SqliteParameter(":FieldValue", DbType.Object) { Direction = ParameterDirection.Input, Value = fieldValue }
            };
            
            return Convert.ToInt32(SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
                sqlParams.ToArray()).ToString());
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
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.AppendFormat("update i7_sflexi_values set {0} where ValueGuid = :ValueGuid;"
                , "SiteGuid = :SiteGuid, "
                + "FeatureGuid = :FeatureGuid, "
                + "ModuleGuid = :ModuleGuid, "
                + "ItemGuid = :ItemGuid, "
                + "FieldGuid = :FieldGuid, "
                + "FieldValue = :FieldValue");

            var sqlParams = new List<SqliteParameter>
            {
                new SqliteParameter(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
                new SqliteParameter(":FeatureGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = featureGuid.ToString() },
                new SqliteParameter(":ModuleGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = moduleGuid.ToString() },
                new SqliteParameter(":ItemGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = itemGuid.ToString() },
                new SqliteParameter(":FieldGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = fieldGuid.ToString() },
                new SqliteParameter(":ValueGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = valueGuid.ToString() },
                new SqliteParameter(":FieldValue", DbType.Object) { Direction = ParameterDirection.Input, Value = fieldValue }
            };

            int rowsAffected = Convert.ToInt32(SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
                sqlParams.ToArray()).ToString());

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("delete from i7_sflexi_values where ValueGuid = :ValueGuid;");

            var sqlParam = new SqliteParameter(":ValueGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = valueGuid.ToString() };

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
                sqlParam);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes rows from the i7_sflexi_values table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("delete from i7_sflexi_values where SiteGuid = :SiteGuid;");

            var sqlParam = new SqliteParameter(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() };


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
                sqlParam);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes rows from the i7_sflexi_values table. Returns true if rows deleted.
        /// </summary>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("delete from i7_sflexi_values where ModuleGuid = :ModuleGuid;");

            var sqlParam = new SqliteParameter(":ModuleGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = moduleGuid.ToString() };


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
                sqlParam);

            return (rowsAffected > 0);
        }
        /// <summary>
        /// Deletes rows from the i7_sflexi_values table. Returns true if rows deleted.
        /// </summary>
        /// <param name="fieldGuid"> fieldGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByField(Guid fieldGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("delete from i7_sflexi_values where FieldGuid = :FieldGuid;");

            var sqlParam = new SqliteParameter(":FieldGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = fieldGuid.ToString() };
            
            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
                sqlParam);

            return (rowsAffected > 0);
        }
        /// <summary>
        /// Deletes rows from the i7_sflexi_values table. Returns true if rows deleted.
        /// </summary>
        /// <param name="itemGuid"> itemGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByItem(Guid itemGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("delete from i7_sflexi_values where ItemGuid = :ItemGuid;");

            var sqlParam = new SqliteParameter(":ItemGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = itemGuid.ToString() };

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
                sqlParam);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the i7_sflexi_values table.
        /// </summary>
        /// <param name="valueGuid"> valueGuid </param>
        public static IDataReader GetOne(Guid valueGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("select * from i7_sflexi_values where ValueGuid = :ValueGuid;");

            var sqlParam = new SqliteParameter(":ValueGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = valueGuid.ToString() };

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
                sqlParam);
        }

        /// <summary>
        /// Gets a count of rows in the i7_sflexi_values table.
        /// </summary>
        public static int GetCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("select count(*) from i7_sflexi_values;");

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                 ConnectionString.GetReadConnectionString(),
 
				 sqlCommand.ToString()));
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the i7_sflexi_values table.
        /// </summary>
        public static IDataReader GetAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("select * from i7_sflexi_values;");

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),

				sqlCommand.ToString());
        }


        /// <summary>
        /// Gets value by ItemGuid and FieldGuid
        /// </summary>
        /// <param name="itemGuid"></param>
        /// <param name="fieldGuid"></param>
        /// <returns></returns>
        public static IDataReader GetByItemField(Guid itemGuid, Guid fieldGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("select * from i7_sflexi_values where ItemGuid = :ItemGuid and FieldGuid = :FieldGuid;");

            var sqlParams = new List<SqliteParameter>
            {
                new SqliteParameter(":ItemGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = itemGuid.ToString() },
                new SqliteParameter(":FieldGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = fieldGuid.ToString() }
            };
            
            return SqliteHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
                sqlParams.ToArray());
        }

		public static IDataReader GetByItemGuids(List<Guid> itemGuids)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("select * from i7_sflexi_values where ItemGuid in (:ItemGuids);");

			//select * from i7_sflexi_values where ItemGuid In ('GUID','GUID');
			List<string> guids = new List<string>();
			foreach (var guid in itemGuids)
			{
				guids.Add($"'{guid.ToString()}'");
			}
			
			var sqlParam = new SqliteParameter(":ItemGuids", DbType.Object) { Direction = ParameterDirection.Input, Value = String.Join(",", guids) };

			return SqliteHelper.ExecuteReader(
			   ConnectionString.GetWriteConnectionString(),
			   sqlCommand.ToString(),
			   sqlParam);
		}

		public static IDataReader GetByItemGuid(Guid itemGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("select * from i7_sflexi_values where ItemGuid = :ItemGuid;");

            var sqlParam = new SqliteParameter(":ItemGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = itemGuid.ToString() };

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
                sqlParam);
        }

		public static IDataReader GetByModuleGuid(Guid moduleGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("select * from i7_sflexi_values where ModuleGuid = :ModuleGuid;");

			var sqlParam = new SqliteParameter(":ModuleGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = moduleGuid.ToString() };

			return SqliteHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				sqlParam);
		}

		public static IDataReader GetByDefinitionGuid(Guid definitionGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append(@"select v.* 
				from i7_sflexi_values v 
				join i7_sflexi_fields f 
					on f.FieldGuid = v.FieldGuid 
				where DefinitionGuid = :DefinitionGuid;");

			var sqlParam = new SqliteParameter(":DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() };

			return SqliteHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				sqlParam);
		}


		public static IDataReader GetByFieldGuid(Guid fieldGuid)
        {
            
            var sqlCommand = @"
                SELECT v.*, f.Name as FieldName 
                FROM i7_sflexi_values v
                JOIN i7_sflexi_values f ON f.FieldGuid = v.FieldGuid
                WHERE f.FieldGuid = :FieldGuid;";

            var sqlParam = new SqliteParameter(":FieldGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = fieldGuid.ToString() };
            
            return SqliteHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand,
                sqlParam);
        }

        public static IDataReader GetByFieldGuidForModule(Guid fieldGuid, Guid moduleGuid)
        {
            var sqlCommand = @"
                SELECT v.*, f.Name AS FieldNAme 
                FROM i7_sflexi_values v
                JOIN i7_sflexi_values f ON f.FieldName = v.FieldGuid
                WHERE ModuleGuid = :ModuleGuid
                AND FieldGuid = :FieldGuid";

            var sqlParams = new SqliteParameter[]
            {
                new SqliteParameter(":ModuleGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = moduleGuid.ToString() },
                new SqliteParameter(":FieldGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = fieldGuid.ToString() }
            };

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand,
                sqlParams);
        }
		public static IDataReader GetByGuidForModule(Guid fieldGuid, Guid moduleGuid)
		{
			var sqlCommand = @"select * from i7_sflexi_values 
                join mp_Modules on mp_Modules.guid = i7_sflexi_values.ModuleGuid 
                where FieldGuid = :FieldGuid 
                and mp_Modules.ModuleGuid = :ModuleGuid;";

			var sqlParams = new List<SqliteParameter>
			{
				new SqliteParameter(":ModuleGuid", DbType.String) { Direction = ParameterDirection.Input, Value = moduleGuid },
				new SqliteParameter(":FieldGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = fieldGuid.ToString() }
			};

			return SqliteHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				sqlParams.ToArray());
		}

		public static IDataReader GetByGuidForModule(Guid fieldGuid, int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("select * from i7_sflexi_values "
                +"join mp_Modules on mp_Modules.guid = i7_sflexi_values.ModuleGuid "
                +"where FieldGuid = :FieldGuid " 
                +"and mp_Modules.ModuleId = :ModuleId;");

            var sqlParams = new List<SqliteParameter>
            {
                new SqliteParameter(":ModuleId", DbType.Int32) { Direction = ParameterDirection.Input, Value = moduleId },
                new SqliteParameter(":FieldGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = fieldGuid.ToString() }
            };

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
                sqlParams.ToArray());
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
			StringBuilder sqlCommand = new StringBuilder();
			int offsetRows = (pageSize * pageNumber) - pageSize;

			if (!String.IsNullOrWhiteSpace(searchTerm))
			{
				sqlCommand.Append(@"select sql_calc_found_rows found_rows() as totalrows, v.*
					from i7_sflexi_values v
						join(
								select distinct FieldGuid
								from i7_sflexi_fields
								where name = ':Field'
								and DefinitionGuid = :DefinitionGuid
							) f on f.FieldGuid = v.FieldGuid
						where ModuleGuid = :ModuleGuid
						and v.FieldValue like '%:SearchTerm%'
						order by id :SortDirection
						limit :PageSize " + (pageNumber > 1 ? "offset :OffsetRows;" : ";"));
			}
			else
			{
				sqlCommand.Append(@"select sql_calc_found_rows found_rows() as totalrows, v.*
					from i7_sflexi_values v
						join(
								select distinct FieldGuid
								from i7_sflexi_fields
								where name = ':Field'
								and DefinitionGuid = :DefinitionGuid
							) f on f.FieldGuid = v.FieldGuid
						where ModuleGuid = :ModuleGuid
						order by id :SortDirection
						limit :PageSize " + (pageNumber > 1 ? "offset :OffsetRows;" : ";"));
			}

			var sqlParams = new List<SqliteParameter>
			{
				new SqliteParameter(":PageSize", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
				new SqliteParameter(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
				new SqliteParameter(":SearchTerm", DbType.String, 255) { Direction = ParameterDirection.Input, Value = searchTerm },
				new SqliteParameter(":Field", DbType.String, 50) { Direction = ParameterDirection.Input, Value = field },
				new SqliteParameter(":ModuleGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = moduleGuid.ToString() },
				new SqliteParameter(":SortDirection", DbType.String, 4) { Direction = ParameterDirection.Input, Value = descending ? "DESC" : "ASC" }
			};

			return SqliteHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			sqlParams.ToArray());
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

            sqlCommand.Append("select * from i7_sflexi_values limit :PageSize" + (pageNumber > 1 ? "offset :OffsetRows;" : ";"));

            var sqlParams = new List<SqliteParameter>
            {
                new SqliteParameter(":PageSize", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
                new SqliteParameter(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageLowerBound }
            };

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
                sqlParams.ToArray());

        }
    }

}