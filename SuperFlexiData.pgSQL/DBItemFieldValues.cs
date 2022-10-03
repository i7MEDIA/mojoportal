// Created:					2017-12-30
// Last Modified:			2022-08-17

using mojoPortal.Data;
using Npgsql;
using NpgsqlTypes;
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
                ,@"valueguid
                 ,siteguid
                 ,featureguid
                 ,moduleguid
                 ,itemguid
                 ,fieldguid
                 ,fieldvalue"
                ,@":valueguid
                  ,:siteguid
                  ,:featureguid
                  ,:moduleguid
                  ,:itemguid
                  ,:fieldguid
                  ,:fieldvalue");

            var sqlParams = new List<NpgsqlParameter>
            {
                new NpgsqlParameter(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid },
                new NpgsqlParameter(":featureguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = featureGuid },
                new NpgsqlParameter(":moduleguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = moduleGuid },
                new NpgsqlParameter(":itemguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = itemGuid },
                new NpgsqlParameter(":fieldguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldGuid },
                new NpgsqlParameter(":valueguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = valueGuid },
                new NpgsqlParameter(":fieldvalue", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = fieldValue }
            };
            
            return Convert.ToInt32(NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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

            sqlCommand.AppendFormat("update i7_sflexi_values set {0} where valueguid = :valueguid;"
                , "siteguid = :siteguid, "
                + "featureguid = :featureguid, "
                + "moduleguid = :moduleguid, "
                + "itemguid = :itemguid, "
                + "fieldguid = :fieldguid, "
                + "fieldvalue = :fieldvalue");

            var sqlParams = new List<NpgsqlParameter>
            {
                new NpgsqlParameter(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid },
                new NpgsqlParameter(":featureguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = featureGuid },
                new NpgsqlParameter(":moduleguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = moduleGuid },
                new NpgsqlParameter(":itemguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = itemGuid },
                new NpgsqlParameter(":fieldguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldGuid },
                new NpgsqlParameter(":valueguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = valueGuid },
                new NpgsqlParameter(":fieldvalue", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = fieldValue }
            };

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("delete from i7_sflexi_values where valueguid = :valueguid;");

            var sqlParam = new NpgsqlParameter(":valueguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = valueGuid };

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("delete from i7_sflexi_values where siteguid = :siteguid;");

            var sqlParam = new NpgsqlParameter(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid };


            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("delete from i7_sflexi_values where moduleguid = :moduleguid;");

            var sqlParam = new NpgsqlParameter(":moduleguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = moduleGuid };


            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("delete from i7_sflexi_values where fieldguid = :fieldguid;");

            var sqlParam = new NpgsqlParameter(":fieldguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldGuid };
            
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("delete from i7_sflexi_values where itemguid = :itemguid;");

            var sqlParam = new NpgsqlParameter(":itemguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = itemGuid };

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("select * from i7_sflexi_values where valueguid = :valueguid;");

            var sqlParam = new NpgsqlParameter(":valueguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = valueGuid };

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                 ConnectionString.GetReadConnectionString(),
                 CommandType.Text,
				 sqlCommand.ToString()));
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the i7_sflexi_values table.
        /// </summary>
        public static IDataReader GetAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("select * from i7_sflexi_values;");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("select * from i7_sflexi_values where itemguid = :itemguid and fieldguid = :fieldguid;");

            var sqlParams = new List<NpgsqlParameter>
            {
                new NpgsqlParameter(":itemguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = itemGuid },
                new NpgsqlParameter(":fieldguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldGuid }
            };
            
            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
				sqlCommand.ToString(),
                sqlParams.ToArray());
        }

		public static IDataReader GetByItemGuids(List<Guid> itemGuids)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("select * from i7_sflexi_values where itemguid in (:itemguids);");

			//select * from i7_sflexi_values where ItemGuid In ('GUID','GUID');
			List<string> guids = new List<string>();
			foreach (var guid in itemGuids)
			{
				guids.Add($"'{guid.ToString()}'");
			}
			
			var sqlParam = new NpgsqlParameter(":itemguids", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = String.Join(",", guids) };

			return NpgsqlHelper.ExecuteReader(
			   ConnectionString.GetWriteConnectionString(),
               CommandType.Text,
			   sqlCommand.ToString(),
			   sqlParam);
		}

		public static IDataReader GetByItemGuid(Guid itemGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append(@"
				SELECT v.*, f.Name AS FieldName 
				FROM i7_sflexi_values v 
				JOIN i7_sflexi_fields f ON f.FieldGuid = v.FieldGuid 
				WHERE ItemGuid = :ItemGuid;");

            var sqlParam = new NpgsqlParameter(":ItemGuid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = itemGuid };

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
				sqlCommand.ToString(),
                sqlParam);
        }

		public static IDataReader GetByModuleGuid(Guid moduleGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("select * from i7_sflexi_values where moduleguid = :moduleguid;");

			var sqlParam = new NpgsqlParameter(":moduleguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = moduleGuid };

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
				sqlCommand.ToString(),
				sqlParam);
		}

		public static IDataReader GetByDefinitionGuid(Guid definitionGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("select v.* from i7_sflexi_values v join i7_sflexi_fields f on f.fieldguid = v.fieldguid where definitionguid = :definitionguid;");

			var sqlParam = new NpgsqlParameter(":definitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = definitionGuid };

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
				sqlCommand.ToString(),
				sqlParam);
		}


		public static IDataReader GetByFieldGuid(Guid fieldGuid)
        {
            var commandText = @"
                SELECT v.*, f.Name AS FieldName
                FROM public.i7_sflexi_values v
                JOIN i7_sflexi_fields f ON f.FieldGuid = v.FieldGuid
                WHERE f.FieldGuid = :FieldGuild;";

            var commandParamters = new NpgsqlParameter[] {
                new NpgsqlParameter(":FieldGuild", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldGuid }
            };
            
            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                commandText,
                commandParamters
            );
        }


        public static IDataReader GetByGuidForModule(Guid fieldGuid, Guid moduleGuid)
        {
            var commandText = @"
                SELECT v.*, f.Name AS FieldName
                FROM public.i7_sflexi_values v
                JOIN i7_sflexi_fields f ON f.FieldGuid = v.FieldGuid
                WHERE v.ModuleGuid = :ModuleGuid
                AND f.FieldGuid = :FieldGuild;";

            var commandParameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter(":ModuleGuid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = moduleGuid },
                new NpgsqlParameter(":FieldGuid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldGuid }
            };

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                commandText,
                commandParameters
            );
        }


        public static IDataReader GetByGuidForModule(Guid fieldGuid, int moduleId)
        {
            var commandText = @"
                    SELECT
                        ValueGuid,
                        v.SiteGuid,
                        v.FeatureGuid,
                        ModuleGuid,
                        ItemGuid,
                        FieldGuid,
                        FieldValue
                    FROM
                        public.i7_sflexi_values v
                    JOIN
                        public.mp_Modules m ON m.Guid::uuid = v.ModuleGuid
                    WHERE
                        v.FieldGuid = :FieldGuid
                    AND
                        m.ModuleID = :ModuleID;";

            var commandParameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter(":ModuleID", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = moduleId },
                new NpgsqlParameter(":FieldGuid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldGuid }
            };

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                commandText,
                commandParameters
            );
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
							select distinct fieldguid
							from i7_sflexi_fields
							where name = ':field'
							and definitionguid = :definitionguid
							) f on f.fieldguid = v.fieldguid
						where moduleguid = :moduleguid
						and v.fieldvalue like '%:searchterm%'
						order by id :sortdirection
						limit :pagesize " + (pageNumber > 1 ? "offset :offsetrows;" : ";"));
			}
			else
			{
				sqlCommand.Append(@"select sql_calc_found_rows found_rows() as totalrows, v.*
					from i7_sflexi_values v
						join(
							select distinct fieldguid
							from i7_sflexi_fields
							where name = ':field'
							and definitionguid = :definitionguid
							) f on f.fieldguid = v.fieldguid
						where moduleguid = :moduleguid
						order by id :sortdirection
						limit :pagesize " + (pageNumber > 1 ? "offset :offsetrows;" : ";"));
			}

			var sqlParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":pagesize", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = pageSize },
				new NpgsqlParameter(":offsetrows", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = offsetRows },
				new NpgsqlParameter(":searchterm", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = searchTerm },
				new NpgsqlParameter(":field", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = field },
				new NpgsqlParameter(":moduleguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = moduleGuid },
				new NpgsqlParameter(":sortdirection", NpgsqlDbType.Varchar, 4) { Direction = ParameterDirection.Input, Value = descending ? "DESC" : "ASC" }
			};

			return NpgsqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
            CommandType.Text,
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

            sqlCommand.Append("select * from i7_sflexi_values limit :pagesize" + (pageNumber > 1 ? "offset :offsetrows;" : ";"));

            var sqlParams = new List<NpgsqlParameter>
            {
                new NpgsqlParameter(":pagesize", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = pageSize },
                new NpgsqlParameter(":offsetrows", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = pageLowerBound }
            };

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
				sqlCommand.ToString(),
                sqlParams.ToArray());

        }
		//    public static IDataReader GetByFieldGuid(Guid fieldGuid)
		//    {
		//        const string sqlCommand = @"
		//SELECT v.*, f.Name AS `FieldName` 
		//FROM `i7_sflexi_values` v 
		//JOIN `i7_sflexi_fields` f ON f.FieldGuid = v.FieldGuid
		//WHERE f.`FieldGuid` = :FieldGuid;";

		//        var sqlParam = new NpgsqlParameter(":FieldGuid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldGuid };

		//        return NpgsqlHelper.ExecuteReader(
		//                       ConnectionString.GetReadConnectionString(),
		//                       CommandType.Text,
		//                       sqlCommand.ToString(),
		//                       sqlParam);
		//    }


		public static IDataReader GetByFieldGuidForModule(Guid fieldGuid, Guid moduleGuid)
		{
			const string sqlCommand = @"
				SELECT v.*, f.Name AS FieldName 
				FROM i7_sflexi_values v 
				JOIN i7_sflexi_fields f ON f.FieldGuid = v.FieldGuid
				WHERE ModuleGuid = :ModuleGuid 
				AND f.FieldGuid = :FieldGuid;";

			var sqlParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":moduleguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = moduleGuid },
				new NpgsqlParameter(":fieldguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldGuid },
			};

			return NpgsqlHelper.ExecuteReader(
						   ConnectionString.GetReadConnectionString(),
						   CommandType.Text,
						   sqlCommand.ToString(),
						   sqlParams.ToArray());
		}

	}

}