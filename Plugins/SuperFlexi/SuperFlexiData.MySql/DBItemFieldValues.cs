using mojoPortal.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
			string fieldValue
		)
		{
			const string sqlCommand = @"
				INSERT INTO `i7_sflexi_values` (
					ValueGuid, 
					SiteGuid, 
					FeatureGuid, 
					ModuleGuid, 
					ItemGuid, 
					FieldGuid, 
					FieldValue
				) VALUES (
					?ValueGuid,
					?SiteGuid,
					?FeatureGuid,
					?ModuleGuid,
					?ItemGuid,
					?FieldGuid,
					?FieldValue
				);";

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

			return Convert.ToInt32(
				MySqlHelper.ExecuteNonQuery(
					ConnectionString.GetWriteConnectionString(),
					sqlCommand,
					sqlParams.ToArray()
				)
			);
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
			string fieldValue
		)
		{
			const string sqlCommand = @"
				UPDATE
					`i7_sflexi_values`
				SET
					`SiteGuid` = ?SiteGuid,
					`FeatureGuid` = ?FeatureGuid,
					`ModuleGuid` = ?ModuleGuid,
					`ItemGuid` = ?ItemGuid,
					`FieldGuid` = ?FieldGuid,
					`FieldValue` = ?FieldValue
				WHERE
					`ValueGuid` = ?ValueGuid;";

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
		/// Deletes a row from the i7_sflexi_values table. Returns true if row deleted.
		/// </summary>
		/// <param name="valueGuid"> valueGuid </param>
		/// <returns>bool</returns>
		public static bool Delete(Guid valueGuid)
		{
			const string sqlCommand = "DELETE FROM `i7_sflexi_values` WHERE `ValueGuid` = ?ValueGuid;";

			var sqlParam = new MySqlParameter("?ValueGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = valueGuid };

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);

			return rowsAffected > 0;
		}


		/// <summary>
		/// Deletes rows from the i7_sflexi_values table. Returns true if rows deleted.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <returns>bool</returns>
		public static bool DeleteBySite(Guid siteGuid)
		{
			const string sqlCommand = "DELETE FROM `i7_sflexi_values` WHERE `SiteGuid` = ?SiteGuid;";

			var sqlParam = new MySqlParameter("?SiteGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = siteGuid };

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);

			return rowsAffected > 0;
		}


		/// <summary>
		/// Deletes rows from the i7_sflexi_values table. Returns true if rows deleted.
		/// </summary>
		/// <param name="moduleGuid"> moduleGuid </param>
		/// <returns>bool</returns>
		public static bool DeleteByModule(Guid moduleGuid)
		{
			const string sqlCommand = "DELETE FROM `i7_sflexi_values` WHERE `ModuleGuid` = ?ModuleGuid;";

			var sqlParam = new MySqlParameter("?ModuleGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = moduleGuid };

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);

			return rowsAffected > 0;
		}


		/// <summary>
		/// Deletes rows from the i7_sflexi_values table. Returns true if rows deleted.
		/// </summary>
		/// <param name="fieldGuid"> fieldGuid </param>
		/// <returns>bool</returns>
		public static bool DeleteByField(Guid fieldGuid)
		{
			const string sqlCommand = "DELETE FROM `i7_sflexi_values` WHERE `FieldGuid` = ?FieldGuid;";

			var sqlParam = new MySqlParameter("?FieldGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = fieldGuid };

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);

			return rowsAffected > 0;
		}


		/// <summary>
		/// Deletes rows from the i7_sflexi_values table. Returns true if rows deleted.
		/// </summary>
		/// <param name="itemGuid"> itemGuid </param>
		/// <returns>bool</returns>
		public static bool DeleteByItem(Guid itemGuid)
		{
			const string sqlCommand = "DELETE FROM `i7_sflexi_values` WHERE `ItemGuid` = ?ItemGuid;";

			var sqlParam = new MySqlParameter("?ItemGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = itemGuid };

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);

			return rowsAffected > 0;
		}


		/// <summary>
		/// Gets an IDataReader with one row from the i7_sflexi_values table.
		/// </summary>
		/// <param name="valueGuid"> valueGuid </param>
		public static IDataReader GetOne(Guid valueGuid)
		{
			const string sqlCommand = @"
				SELECT v.*, f.Name AS `FieldName` 
				FROM `i7_sflexi_values` v 
				JOIN `i7_sflexi_fields` f ON f.FieldGuid = v.FieldGuid 
				WHERE `ValueGuid` = ?ValueGuid;";

			var sqlParam = new MySqlParameter("?ValueGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = valueGuid };

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);
		}


		/// <summary>
		/// Gets a count of rows in the i7_sflexi_values table.
		/// </summary>
		public static int GetCount()
		{
			const string sqlCommand = "SELECT Count(*) FROM `i7_sflexi_values`;";

			return Convert.ToInt32(
				MySqlHelper.ExecuteScalar(
					ConnectionString.GetReadConnectionString(),
					sqlCommand
				)
			);
		}


		/// <summary>
		/// Gets an IDataReader with all rows in the i7_sflexi_values table.
		/// </summary>
		public static IDataReader GetAll()
		{
			const string sqlCommand = @"
				SELECT v.*, f.Name AS `FieldName` 
				FROM `i7_sflexi_values` v 
				JOIN `i7_sflexi_fields` f ON f.FieldGuid = v.FieldGuid;";

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand
			);
		}


		/// <summary>
		/// Gets value by ItemGuid and FieldGuid
		/// </summary>
		/// <param name="itemGuid"></param>
		/// <param name="fieldGuid"></param>
		/// <returns></returns>
		public static IDataReader GetByItemField(Guid itemGuid, Guid fieldGuid)
		{
			const string sqlCommand = @"
				SELECT v.*, f.Name AS `FieldName` 
				FROM `i7_sflexi_values` v 
				JOIN `i7_sflexi_fields` f ON f.FieldGuid = v.FieldGuid
				WHERE `ItemGuid` = ?ItemGuid 
				AND `f.FieldGuid` = ?FieldGuid;";

			var sqlParams = new List<MySqlParameter>
			{
				new MySqlParameter("?ItemGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = itemGuid },
				new MySqlParameter("?FieldGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = fieldGuid }
			};

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParams.ToArray()
			);
		}


		public static IDataReader GetByItemGuids(List<Guid> itemGuids)
		{
			// Note: The "WHERE IN" query was busted, and I couldn't figure out how to get it working,
			// so I made it build the query manually.
			//
			// This was how the broken command was rendering: SELECT * FROM i7_sflexi_values WHERE IN ('\'guid1'\','\'guid2'\')
			// TODO: Find out how to make "WHERE IN" work
			var sqlCommand = $@"
				SELECT v.*, f.Name AS `FieldName` 
				FROM `i7_sflexi_values` v 
				JOIN `i7_sflexi_fields` f ON f.FieldGuid = v.FieldGuid
				WHERE {getItems()};";

			string getItems()
			{
				return string.Join(" ", itemGuids.Select((x, i) =>
				{
					var item = $"`ItemGuid` = ?ItemGuid{i}";

					if (i < itemGuids.Count() - 1)
					{
						item += " OR";
					}

					return item;
				}));
			}

			var sqlParams = new List<MySqlParameter>();

			for (var i = 0; i < itemGuids.Count(); i++)
			{
				sqlParams.Add(new MySqlParameter($"?ItemGuid{i}", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = itemGuids[i] });
			}

			return MySqlHelper.ExecuteReader(
			   ConnectionString.GetWriteConnectionString(),
			   sqlCommand,
			   sqlParams.ToArray()
			);
		}

		public static IDataReader GetByItemGuid(Guid itemGuid)
		{
			const string sqlCommand = @"
				SELECT v.*, f.Name AS `FieldName` 
				FROM `i7_sflexi_values` v 
				JOIN `i7_sflexi_fields` f ON f.FieldGuid = v.FieldGuid 
				WHERE `ItemGuid` = ?ItemGuid;";

			var sqlParam = new MySqlParameter("?ItemGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = itemGuid };

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);
		}


		public static IDataReader GetByModuleGuid(Guid moduleGuid)
		{
			const string sqlCommand = @"
				SELECT v.*, f.Name AS `FieldName` 
				FROM `i7_sflexi_values` v 
				JOIN `i7_sflexi_fields` f ON f.FieldGuid = v.FieldGuid 
				WHERE `ModuleGuid` = ?ModuleGuid;";

			var sqlParam = new MySqlParameter("?ModuleGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = moduleGuid };

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);
		}


		public static IDataReader GetByDefinitionGuid(Guid definitionGuid)
		{
			const string sqlCommand = @"
				SELECT v.*, f.Name AS `FieldName` 
				FROM `i7_sflexi_values` v 
				JOIN `i7_sflexi_fields` f ON f.FieldGuid = v.FieldGuid
				WHERE `DefinitionGuid` = ?DefinitionGuid;";

			var sqlParam = new MySqlParameter("?DefinitionGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = definitionGuid };

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);
		}


		public static IDataReader GetByFieldGuid(Guid fieldGuid)
		{
			const string sqlCommand = @"
				SELECT v.*, f.Name AS `FieldName` 
				FROM `i7_sflexi_values` v 
				JOIN `i7_sflexi_fields` f ON f.FieldGuid = v.FieldGuid
				WHERE f.`FieldGuid` = ?FieldGuid;";

			var sqlParam = new MySqlParameter("?FieldGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = fieldGuid };

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);
		}


		public static IDataReader GetByGuidForModule(Guid fieldGuid, Guid moduleGuid)
		{
			const string sqlCommand = @"
				SELECT v.*, f.Name AS `FieldName` 
				FROM `i7_sflexi_values` v 
				JOIN `i7_sflexi_fields` f ON f.FieldGuid = v.FieldGuid
				WHERE `ModuleGuid` = ?ModuleGuid 
				AND f.`FieldGuid` = ?FieldGuid;";

			var sqlParams = new List<MySqlParameter>
			{
				new MySqlParameter("?ModuleGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = moduleGuid },
				new MySqlParameter("?FieldGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = fieldGuid }
			};

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParams.ToArray()
			);
		}


		public static IDataReader GetByGuidForModule(Guid fieldGuid, int moduleId)
		{
			const string sqlCommand = @"
				SELECT * FROM `i7_sflexi_values`
				JOIN `mp_Modules` ON `mp_Modules`.`Guid` = `i7_sflexi_values`.`ModuleGuid`
				WHERE `FieldGuid` = ?FieldGuid
				AND `mp_Modules`.`ModuleID` = ?ModuleID;";

			var sqlParams = new List<MySqlParameter>
			{
				new MySqlParameter("?ModuleID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = moduleId },
				new MySqlParameter("?FieldGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = fieldGuid }
			};

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				sqlParams.ToArray()
			);
		}

		//public static IDataReader GetByGuidForDefinition(Guid fieldGuid, Guid defGuid)
		//{
		//	const string sqlCommand = @"
		//		SELECT * FROM `i7_sflexi_values`
		//		JOIN `mp_Modules` ON `mp_Modules`.`Guid` = `i7_sflexi_values`.`ModuleGuid`
		//		WHERE `FieldGuid` = ?FieldGuid
		//		AND `mp_Modules`.`ModuleID` = ?ModuleID;";

		//	var sqlParams = new List<MySqlParameter>
		//	{
		//		new MySqlParameter("?FieldGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = fieldGuid },
		//		new MySqlParameter("?DefinitionGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = defGuid }
		//	};

		//	return MySqlHelper.ExecuteReader(
		//		ConnectionString.GetWriteConnectionString(),
		//		sqlCommand.ToString(),
		//		sqlParams.ToArray()
		//	);
		//}

		public static IDataReader GetPageOfValuesForField(
			Guid moduleGuid,
			Guid definitionGuid,
			string field,
			int pageNumber,
			int pageSize,
			string searchTerm = "",
			//string searchField = "",
			bool descending = false
		)
		{
			string sqlCommand;
			var offsetRows = (pageSize * pageNumber) - pageSize;

			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				sqlCommand = $@"
					SELECT SQL_CALC_FOUND_ROWS FOUND_ROWS()
					AS TotalRows, v.*, f.`FieldName`
					FROM `i7_sflexi_values` v
					JOIN(
						SELECT DISTINCT `FieldGuid`, `Name` AS `FieldName`
						FROM `i7_sflexi_fields`
						WHERE `Name` = ?Field
						AND `DefinitionGuid` = ?DefinitionGuid
					) f ON f.`FieldGuid` = v.`FieldGuid`
					WHERE `ModuleGuid` = ?ModuleGuid
					AND v.`FieldValue` LIKE '%?SearchTerm%'
					ORDER BY `Id` {(descending ? "DESC" : "ASC")}
					LIMIT ?PageSize
					{ifTrue(pageNumber > 1, "OFFSET ?OffsetRows")};";
			}
			else
			{
				sqlCommand = $@"
					SELECT SQL_CALC_FOUND_ROWS FOUND_ROWS()
					AS TotalRows, v.*, f.Name AS `FieldName`
					FROM `i7_sflexi_values` v
					JOIN(
						SELECT DISTINCT `FieldGuid`, `Name` AS `FieldName`
						FROM `i7_sflexi_fields`
						WHERE `Name` = ?Field
						AND `DefinitionGuid` = ?DefinitionGuid
					) f ON f.`FieldGuid` = v.`FieldGuid`
					WHERE `ModuleGuid` = ?ModuleGuid
					ORDER BY `Id` {(descending ? "DESC" : "ASC")}
					LIMIT ?PageSize
					{ifTrue(pageNumber > 1, "OFFSET ?OffsetRows")}";
			}

			var sqlParams = new List<MySqlParameter>
			{
				new MySqlParameter("?PageSize", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
				new MySqlParameter("?OffsetRows", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = offsetRows },
				new MySqlParameter("?SearchTerm", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = searchTerm },
				new MySqlParameter("?Field", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = field },
				new MySqlParameter("?ModuleGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = moduleGuid },
			};

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand,
				sqlParams.ToArray()
			);
		}


		/// <summary>
		/// Gets a page of data from the i7_sflexi_values table.
		/// </summary>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="totalPages">total pages</param>
		public static IDataReader GetPage(int pageNumber, int pageSize, out int totalPages)
		{
			var pageLowerBound = (pageSize * pageNumber) - pageSize;

			totalPages = 1;

			var totalRows = GetCount();

			if (pageSize > 0)
			{
				totalPages = totalRows / pageSize;
			}

			if (totalRows <= pageSize)
			{
				totalPages = 1;
			}
			else
			{
				Math.DivRem(totalRows, pageSize, out var remainder);

				if (remainder > 0)
				{
					totalPages += 1;
				}
			}

			var sqlCommand = $@"
				SELECT v.*, f.Name AS `FieldName` 
				FROM `i7_sflexi_values` v 
				JOIN `i7_sflexi_fields` f ON f.FieldGuid = v.FieldGuid
				LIMIT ?PageSize {ifTrue(pageNumber > 1, "OFFSET ?OffsetRows")};";

			var sqlParams = new List<MySqlParameter>
			{
				new MySqlParameter("?PageSize", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
				new MySqlParameter("?OffsetRows", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = pageLowerBound }
			};

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand,
				sqlParams.ToArray()
			);
		}


		private static string ifTrue(bool clause, string str)
		{
			return clause ? str : string.Empty;
		}
	}
}
