// Created:					2017-07-13
// Last Modified:			2017-12-20

using mojoPortal.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;
using System.Collections.Generic;

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
            sqlCommand.AppendFormat("INSERT INTO {0} ({1}) VALUES ({2});"
                , "i7_sflexi_values"
                , "ValueGuid, "
                + "SiteGuid, "
                + "FeatureGuid, "
                + "ModuleGuid, "
                + "ItemGuid, "
                + "FieldGuid, "
                + "FieldValue"
                , "?ValueGuid, "
                + "?SiteGuid, "
                + "?FeatureGuid, "
                + "?ModuleGuid, "
                + "?ItemGuid, "
                + "?FieldGuid, "
                + "?FieldValue");

            var sqlParams = new List<MySqlParameter>
            {
                new MySqlParameter("?SiteGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = siteGuid },
                new MySqlParameter("?FeatureGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = featureGuid },
                new MySqlParameter("?ModuleGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = moduleGuid },
                new MySqlParameter("?ItemGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = itemGuid },
                new MySqlParameter("?FieldGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = fieldGuid },
                new MySqlParameter("?ValueGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = valueGuid },
                new MySqlParameter("?FieldValue", MySqlDbType.LongText) { Direction = ParameterDirection.Input, Value = fieldValue }
            };
            
            return Convert.ToInt32(MySqlHelper.ExecuteNonQuery(
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

            sqlCommand.AppendFormat("UPDATE i7_sflexi_values SET {0} WHERE ValueGuid = ?ValueGuid;"
                , "SiteGuid = ?SiteGuid, "
                + "FeatureGuid = ?FeatureGuid, "
                + "ModuleGuid = ?ModuleGuid, "
                + "ItemGuid = ?ItemGuid, "
                + "FieldGuid = ?FieldGuid, "
                + "FieldValue = ?FieldValue");

            var sqlParams = new List<MySqlParameter>
            {
                new MySqlParameter("?SiteGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = siteGuid },
                new MySqlParameter("?FeatureGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = featureGuid },
                new MySqlParameter("?ModuleGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = moduleGuid },
                new MySqlParameter("?ItemGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = itemGuid },
                new MySqlParameter("?FieldGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = fieldGuid },
                new MySqlParameter("?ValueGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = valueGuid },
                new MySqlParameter("?FieldValue", MySqlDbType.LongText) { Direction = ParameterDirection.Input, Value = fieldValue }
            };

            int rowsAffected = Convert.ToInt32(MySqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("DELETE FROM i7_sflexi_values WHERE ValueGuid = ?ValueGuid;");

            var sqlParam = new MySqlParameter("?ValueGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = valueGuid };

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("DELETE FROM i7_sflexi_values WHERE SiteGuid = ?SiteGuid;");

            var sqlParam = new MySqlParameter("?SiteGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = siteGuid };


            int rowsAffected = MySqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("DELETE FROM i7_sflexi_values WHERE ModuleGuid = ?ModuleGuid;");

            var sqlParam = new MySqlParameter("?ModuleGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = moduleGuid };


            int rowsAffected = MySqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("DELETE FROM i7_sflexi_values WHERE FieldGuid = ?FieldGuid;");

            var sqlParam = new MySqlParameter("?FieldGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = fieldGuid };
            
            int rowsAffected = MySqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("DELETE FROM i7_sflexi_values WHERE ItemGuid = ?ItemGuid;");

            var sqlParam = new MySqlParameter("?ItemGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = itemGuid };

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("SELECT * FROM i7_sflexi_values WHERE ValueGuid = ?ValueGuid;");

            var sqlParam = new MySqlParameter("?ValueGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = valueGuid };

            return MySqlHelper.ExecuteReader(
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
            sqlCommand.Append("SELECT Count(*) FROM i7_sflexi_values;");

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                 ConnectionString.GetReadConnectionString(),
                 sqlCommand.ToString()));
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the i7_sflexi_values table.
        /// </summary>
        public static IDataReader GetAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM i7_sflexi_values;");

            return MySqlHelper.ExecuteReader(
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
            sqlCommand.Append("SELECT * FROM i7_sflexi_values WHERE ItemGuid = ?ItemGuid AND FieldGuid = ?FieldGuid;");

            var sqlParams = new List<MySqlParameter>
            {
                new MySqlParameter("?ItemGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = itemGuid },
                new MySqlParameter("?FieldGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = fieldGuid }
            };
            
            return MySqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                sqlParams.ToArray());
        }

		public static IDataReader GetByItemGuids(List<Guid> itemGuids)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT * FROM i7_sflexi_values WHERE ItemGuid IN (?ItemGuids);");

			//select * from i7_sflexi_values where ItemGuid In ('GUID','GUID');
			List<string> guids = new List<string>();
			foreach (var guid in itemGuids)
			{
				guids.Add($"'{guid.ToString()}'");
			}
			
			var sqlParam = new MySqlParameter("?ItemGuids", MySqlDbType.LongText) { Direction = ParameterDirection.Input, Value = String.Join(",", guids) };

			return MySqlHelper.ExecuteReader(
			   ConnectionString.GetWriteConnectionString(),
			   sqlCommand.ToString(),
			   sqlParam);
		}

		public static IDataReader GetByItemGuid(Guid itemGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM i7_sflexi_values WHERE ItemGuid = ?ItemGuid;");

            var sqlParam = new MySqlParameter("?ItemGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = itemGuid };

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                sqlParam);
        }

		public static IDataReader GetByModuleGuid(Guid moduleGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT * FROM i7_sflexi_values WHERE ModuleGuid = ?ModuleGuid;");

			var sqlParam = new MySqlParameter("?ModuleGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = moduleGuid };

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				sqlParam);
		}

		public static IDataReader GetByDefinitionGuid(Guid definitionGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT v.* FROM i7_sflexi_values v JOIN i7_sflexi_fields f on f.FieldGuid = v.FieldGuid WHERE DefinitionGuid = ?DefinitionGuid;");

			var sqlParam = new MySqlParameter("?DefinitionGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = definitionGuid };

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				sqlParam);
		}


			public static IDataReader GetByGuid(Guid fieldGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM i7_sflexi_values WHERE FieldGuid = ?FieldGuid;");

            var sqlParam = new MySqlParameter("?FieldGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = fieldGuid };
            
            return MySqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                sqlParam);
        }

        public static IDataReader GetByGuidForModule(Guid fieldGuid, Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM i7_sflexi_values WHERE ModuleGuid = ?ModuleGuid AND FieldGuid = ?FieldGuid;");

            var sqlParams = new List<MySqlParameter>
            {
                new MySqlParameter("?ModuleGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = moduleGuid },
                new MySqlParameter("?FieldGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = fieldGuid }
            };

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                sqlParams.ToArray());
        }

        public static IDataReader GetByGuidForModule(Guid fieldGuid, int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM i7_sflexi_values "
                +"JOIN mp_Modules ON mp_Modules.Guid = i7_sflexi_values.ModuleGuid "
                +"WHERE FieldGuid = ?FieldGuid " 
                +"AND mp_Modules.ModuleID = ?ModuleID;");

            var sqlParams = new List<MySqlParameter>
            {
                new MySqlParameter("?ModuleID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = moduleId },
                new MySqlParameter("?FieldGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = fieldGuid }
            };

            return MySqlHelper.ExecuteReader(
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
				sqlCommand.Append(@"SELECT SQL_CALC_FOUND_ROWS FOUND_ROWS() AS TotalRows, v.*
					FROM `i7_sflexi_values` v
						JOIN(
							SELECT DISTINCT `FieldGuid`
							FROM `i7_sflexi_fields`
							WHERE Name = '?Field'
							AND `DefinitionGuid` = ?DefinitionGuid
							) f ON f.FieldGuid = v.FieldGuid
						WHERE `ModuleGuid` = ?ModuleGuid
						AND v.FieldValue LIKE '%?SearchTerm%'
						ORDER BY `Id` ?SortDirection
						LIMIT ?PageSize " + (pageNumber > 1 ? "OFFSET ?OffsetRows;" : ";"));
			}
			else
			{
				sqlCommand.Append(@"SELECT SQL_CALC_FOUND_ROWS FOUND_ROWS() AS TotalRows, v.*
					FROM `i7_sflexi_values` v
						JOIN(
							SELECT DISTINCT `FieldGuid`
							FROM `i7_sflexi_fields`
							WHERE Name = '?Field'
							AND `DefinitionGuid` = ?DefinitionGuid
							) f ON f.FieldGuid = v.FieldGuid
						WHERE `ModuleGuid` = ?ModuleGuid
						ORDER BY `Id` ?SortDirection
						LIMIT ?PageSize " + (pageNumber > 1 ? "OFFSET ?OffsetRows;" : ";"));
			}

			var sqlParams = new List<MySqlParameter>
			{
				new MySqlParameter("?PageSize", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
				new MySqlParameter("?OffsetRows", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
				new MySqlParameter("?SearchTerm", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = searchTerm },
				new MySqlParameter("?Field", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = field },
				new MySqlParameter("?ModuleGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = moduleGuid },
				new MySqlParameter("?SortDirection", MySqlDbType.VarChar, 4) { Direction = ParameterDirection.Input, Value = descending ? "DESC" : "ASC" }
			};

			return MySqlHelper.ExecuteReader(
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

            sqlCommand.Append("SELECT * FROM i7_sflexi_values LIMIT ?PageSize" + (pageNumber > 1 ? "OFFSET ?OffsetRows;" : ";"));

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


    }

}