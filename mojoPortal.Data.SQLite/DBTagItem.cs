using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;

using mojoPortal.Data; // add a project reference to mojoPortal.Data.SQLite to get this
using Mono.Data.Sqlite;


namespace mojoPortal.Data
{
	public static class DBTagItem
	{
		public static string DBPlatform() => "SQLite";


		#region Create Method

		public static bool Create(
			Guid tagItemGuid,
			Guid siteGuid,
			Guid featureGuid,
			Guid moduleGuid,
			Guid relatedItemGuid,
			Guid tagGuid,
			Guid extraGuid,
			Guid taggedBy
		)
		{
			const string sqlCommand =
				@"INSERT INTO mp_TagItem (
					TagItemGuid,
					SiteGuid,
					FeatureGuid,
					ModuleGuid,
					RelatedItemGuid,
					TagGuid,
					ExtraGuid,
					TaggedBy
				)

				VALUES (
					:TagItemGuid,
					:SiteGuid,
					:FeatureGuid,
					:ModuleGuid,
					:RelatedItemGuid,
					:TagGuid,
					:ExtraGuid,
					:TaggedBy
				);";

			var arParams = new List<SqliteParameter>
			{
				new SqliteParameter(":TagItemGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = tagItemGuid.ToString()
				},
				new SqliteParameter(":SiteGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				},
				new SqliteParameter(":FeatureGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = featureGuid.ToString()
				},
				new SqliteParameter(":ModuleGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = moduleGuid.ToString()
				},
				new SqliteParameter(":RelatedItemGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = relatedItemGuid.ToString()
				},
				new SqliteParameter(":TagGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = tagGuid.ToString()
				},
				new SqliteParameter(":ExtraGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = extraGuid.ToString()
				},
				new SqliteParameter(":TaggedBy", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = taggedBy.ToString()
				}
			}.ToArray();


			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				ConnectionString.GetConnectionString(),
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > -1;
		}

		#endregion


		#region Delete Methods

		public static bool DeleteBySite(Guid siteGuid)
		{
			const string sqlCommand = @"DELETE FROM mp_TagItem WHERE SiteGuid = :SiteGuid;";

			var arParams = new List<SqliteParameter>
			{
				new SqliteParameter(":SiteGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				ConnectionString.GetConnectionString(),
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > 0;
		}


		public static bool Delete(Guid tagItemGuid)
		{
			const string sqlCommand = @"DELETE FROM mp_TagItem WHERE TagItemGuid = :TagItemGuid;";

			var arParams = new List<SqliteParameter>
			{
				new SqliteParameter(":TagItemGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = tagItemGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				ConnectionString.GetConnectionString(),
				sqlCommand,
				arParams
			);

			return rowsAffected > 0;
		}


		public static bool DeleteByTag(Guid tagGuid)
		{
			const string sqlCommand = @"DELETE FROM mp_TagItem WHERE TagGuid = :TagGuid;";

			var arParams = new List<SqliteParameter>
			{
				new SqliteParameter(":TagGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = tagGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				ConnectionString.GetConnectionString(),
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > 0;
		}


		public static bool DeleteByModule(Guid moduleGuid)
		{
			const string sqlCommand = @"DELETE FROM mp_TagItem WHERE ModuleGuid = :ModuleGuid;";

			var arParams = new List<SqliteParameter>
			{
				new SqliteParameter(":ModuleGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = moduleGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				ConnectionString.GetConnectionString(),
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > 0;
		}


		public static bool DeleteByFeature(Guid featureGuid)
		{
			const string sqlCommand = @"DELETE FROM mp_TagItem WHERE FeatureGuid = :FeatureGuid;";

			var arParams = new List<SqliteParameter>
			{
				new SqliteParameter(":FeatureGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = featureGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				ConnectionString.GetConnectionString(),
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > 0;
		}


		public static bool DeleteByRelatedItem(Guid relatedItemGuid)
		{
			const string sqlCommand = "DELETE FROM mp_TagItem WHERE RelatedItemGuid = :RelatedItemGuid;";

			var arParams = new List<SqliteParameter>
			{
				new SqliteParameter(":RelatedItemGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = relatedItemGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				ConnectionString.GetConnectionString(),
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > 0;
		}


		public static bool DeleteByExtraGuid(Guid extraGuid)
		{
			const string sqlCommand = @"DELETE FROM mp_TagItem WHERE ExtraGuid = :ExtraGuid;";

			var arParams = new List<SqliteParameter>
			{
				new SqliteParameter(":ExtraGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = extraGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				ConnectionString.GetConnectionString(),
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > 0;
		}

		#endregion


		#region Get Methods


		public static IDataReader GetByTagItem(Guid tagItemGuid)
		{
			const string sqlCommand =
				@"SELECT 
					ti.TagItemGuid,
					ti.RelatedItemGuid,
					ti.SiteGuid,
					ti.FeatureGuid,
					ti.ModuleGuid,
					ti.TagGuid,
					ti.ExtraGuid,
					ti.TaggedBy,
					t.Tag AS TagText
				FROM mp_TagItem ti
				INNER JOIN mp_Tag t
				ON ti.TagGuid = t.Guid
				WHERE TagItemGuid = :TagItemGuid
				ORDER BY TagText";

			var arParams = new List<SqliteParameter>
			{
				new SqliteParameter(":RelatedItemGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = tagItemGuid.ToString()
				}
			}.ToArray();

			return SqliteHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams
			);
		}

		public static IDataReader GetByRelatedItem(Guid siteGuid, Guid relatedItemGuid)
		{
			const string sqlCommand =
				@"SELECT 
					ti.TagItemGuid,
					ti.RelatedItemGuid,
					ti.SiteGuid,
					ti.FeatureGuid,
					ti.ModuleGuid,
					ti.TagGuid,
					ti.ExtraGuid,
					ti.TaggedBy,
					t.Tag AS TagText
				FROM mp_TagItem ti
				INNER JOIN mp_Tag t
				ON ti.TagGuid = t.Guid
				WHERE RelatedItemGuid = :RelatedItemGuid
				AND ti.SiteGuid = :SiteGuid
				ORDER BY TagText";

			var arParams = new List<SqliteParameter>
			{
				new SqliteParameter(":RelatedItemGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = relatedItemGuid.ToString()
				},
				new SqliteParameter(":SiteGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				}
			}.ToArray();

			return SqliteHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams
			);
		}


		public static IDataReader GetByExtra(Guid siteGuid, Guid extraGuid)
		{
			const string sqlCommand =
				@"SELECT 
					ti.TagItemGuid,
					ti.RelatedItemGuid,
					ti.SiteGuid,
					ti.FeatureGuid,
					ti.ModuleGuid,
					ti.TagGuid,
					ti.ExtraGuid,
					ti.TaggedBy,
					t.Tag AS TagText
				FROM mp_TagItem ti
				INNER JOIN mp_Tag t
				ON ti.TagGuid = t.Guid
				WHERE ExtraGuid = :ExtraGuid
				AND ti.SiteGuid = :SiteGuid
				ORDER BY TagText";

			var arParams = new List<SqliteParameter>
			{
				new SqliteParameter(":RelatedItemGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = extraGuid.ToString()
				},
				new SqliteParameter(":SiteGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				}
			}.ToArray();

			return SqliteHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams
			);
		}

		#endregion
	}
}
