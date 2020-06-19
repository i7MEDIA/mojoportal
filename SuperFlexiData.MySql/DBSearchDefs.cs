using mojoPortal.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace SuperFlexiData
{
	public static class DBSearchDefs
	{
		public static bool Create(
			Guid guid,
			Guid siteGuid,
			Guid featureGuid,
			Guid definitionGuid,
			string title,
			string keywords,
			string description,
			string link,
			string linkQueryAddendum
		)
		{
			const string sqlCommand = @"
				INSERT INTO `i7_sflexi_searchdefs` (
					`Guid`,
					`SiteGuid`,
					`FeatureGuid`,
					`FieldDefinitionGuid`,
					`Title`,
					`Keywords`,
					`Description`,
					`Link`,
					`LinkQueryAddendum`
				) VALUES (
					?Guid,
					?SiteGuid,
					?FeatureGuid,
					?FieldDefinitionGuid,
					?Title,
					?Keywords,
					?Description,
					?Link,
					?LinkQueryAddendum
				);";

			var sqlParams = new List<MySqlParameter>
			{
				new MySqlParameter("?SiteGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = siteGuid },
				new MySqlParameter("?FeatureGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = featureGuid },
				new MySqlParameter("?FieldDefinitionGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = definitionGuid },
				new MySqlParameter("?Guid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = guid },
				new MySqlParameter("?Title", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = title },
				new MySqlParameter("?Keywords", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = keywords },
				new MySqlParameter("?Description", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = description },
				new MySqlParameter("?Link", MySqlDbType.LongText) { Direction = ParameterDirection.Input, Value = link },
				new MySqlParameter("?LinkQueryAddendum", MySqlDbType.VarChar, 16) { Direction = ParameterDirection.Input, Value = linkQueryAddendum }
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


		public static bool Update(
			Guid guid,
			Guid siteGuid,
			Guid featureGuid,
			Guid definitionGuid,
			string title,
			string keywords,
			string description,
			string link,
			string linkQueryAddendum
		)
		{
			const string sqlCommand = @"
				UPDATE
					i7_sflexi_searchdefs
				SET
					`SiteGuid` = ?SiteGuid,
					`FeatureGuid` = ?FeatureGuid,
					`FieldDefinitionGuid` = ?FieldDefinitionGuid,
					`Title` = ?Title,
					`Keywords` = ?Keywords,
					`Description` = ?Description,
					`Link` = ?Link,
					`LinkQueryAddendum` = ?LinkQueryAddendum
				WHERE
					Guid = ?Guid;";

			var sqlParams = new List<MySqlParameter>
			{
				new MySqlParameter("?SiteGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = siteGuid },
				new MySqlParameter("?FeatureGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = featureGuid },
				new MySqlParameter("?FieldDefinitionGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = definitionGuid },
				new MySqlParameter("?Guid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = guid },
				new MySqlParameter("?Title", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = title },
				new MySqlParameter("?Keywords", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = keywords },
				new MySqlParameter("?Description", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = description },
				new MySqlParameter("?Link", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = link },
				new MySqlParameter("?LinkQueryAddendum", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = linkQueryAddendum }
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


		public static bool DeleteByFieldDefinition(Guid fieldDefGuid)
		{
			const string sqlCommand = "DELETE FROM `i7_sflexi_searchdefs` WHERE `FieldDefinitionGuid` = ?FieldDefinitionGuid;";

			var sqlParam = new MySqlParameter("?FieldDefinitionGuid", MySqlDbType.Guid)
			{
				Direction = ParameterDirection.Input,
				Value = fieldDefGuid
			};

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				sqlParam
			);

			return rowsAffected > 0;
		}


		public static bool DeleteBySite(Guid siteGuid)
		{
			const string sqlCommand = "DELETE FROM `i7_sflexi_searchdefs` WHERE `SiteGuid` = ?SiteGuid;";

			var sqlParam = new MySqlParameter("?SiteGuid", MySqlDbType.Guid)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid
			};

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);

			return rowsAffected > 0;
		}


		public static bool Delete(Guid guid)
		{
			const string sqlCommand = "DELETE FROM `i7_sflexi_searchdefs` WHERE `Guid` = ?Guid;";

			var sqlParam = new MySqlParameter("?Guid", MySqlDbType.Guid)
			{
				Direction = ParameterDirection.Input,
				Value = guid
			};

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);

			return rowsAffected > 0;
		}


		public static IDataReader GetByFieldDefinition(Guid fieldDefinitionGuid)
		{
			const string sqlCommand = "SELECT * FROM `i7_sflexi_searchdefs` WHERE `FieldDefinitionGuid` = ?FieldDefinitionGuid;";

			var sqlParam = new MySqlParameter("?FieldDefinitionGuid", MySqlDbType.Guid)
			{
				Direction = ParameterDirection.Input,
				Value = fieldDefinitionGuid
			};

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);
		}


		public static IDataReader GetOne(Guid guid)
		{
			const string sqlCommand = "SELECT * FROM `i7_sflexi_searchdefs` WHERE `Guid` = ?Guid;";

			var sqlParam = new MySqlParameter("?Guid", MySqlDbType.Guid)
			{
				Direction = ParameterDirection.Input,
				Value = guid
			};

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);
		}
	}
}
