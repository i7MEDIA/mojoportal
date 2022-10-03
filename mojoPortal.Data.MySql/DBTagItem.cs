using System;
using System.Collections.Generic;
using System.Data;

using MySql.Data.MySqlClient;

namespace mojoPortal.Data
{
	public static class DBTagItem
	{
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
					?TagItemGuid,
					?SiteGuid,
					?FeatureGuid,
					?ModuleGuid,
					?RelatedItemGuid,
					?TagGuid,
					?ExtraGuid,
					?TaggedBy
				);";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?TagItemGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = tagItemGuid.ToString()
				},
				new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				},
				new MySqlParameter("?FeatureGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = featureGuid.ToString()
				},
				new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = moduleGuid.ToString()
				},
				new MySqlParameter("?RelatedItemGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = relatedItemGuid.ToString()
				},
				new MySqlParameter("?TagGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = tagGuid.ToString()
				},
				new MySqlParameter("?ExtraGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = extraGuid.ToString()
				},
				new MySqlParameter("?TaggedBy", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = taggedBy.ToString()
				}
			}.ToArray();

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				arParams
			);

			return rowsAffected > -1;
		}

		#endregion


		#region Delete Methods

		public static bool DeleteBySite(Guid siteGuid)
		{
			const string sqlCommand = @"DELETE FROM mp_TagItem WHERE SiteGuid = ?SiteGuid;";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				arParams
			);

			return (rowsAffected > 0);
		}


		public static bool Delete(Guid tagItemGuid)
		{
			const string sqlCommand = @"DELETE FROM mp_TagItem WHERE TagItemGuid = ?TagItemGuid;";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?TagItemGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = tagItemGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				arParams
			);

			return rowsAffected > 0;
		}


		public static bool DeleteByTag(Guid tagGuid)
		{
			const string sqlCommand = @"DELETE FROM mp_TagItem WHERE TagGuid = ?TagGuid;";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?TagGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = tagGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				arParams
			);

			return rowsAffected > 0;
		}


		public static bool DeleteByModule(Guid moduleGuid)
		{
			const string sqlCommand = @"DELETE FROM mp_TagItem WHERE ModuleGuid = ?ModuleGuid;";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = moduleGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				arParams
			);

			return rowsAffected > 0;
		}


		public static bool DeleteByFeature(Guid featureGuid)
		{
			const string sqlCommand = @"DELETE FROM mp_TagItem WHERE FeatureGuid = ?FeatureGuid;";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?FeatureGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = featureGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				arParams
			);

			return rowsAffected > 0;
		}


		public static bool DeleteByRelatedItem(Guid relatedItemGuid)
		{
			const string sqlCommand = "DELETE FROM mp_TagItem WHERE RelatedItemGuid = ?RelatedItemGuid;";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?RelatedItemGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = relatedItemGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				arParams
			);

			return rowsAffected > 0;
		}


		public static bool DeleteByExtraGuid(Guid extraGuid)
		{
			const string sqlCommand = @"DELETE FROM mp_TagItem WHERE ExtraGuid = ?ExtraGuid;";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?ExtraGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = extraGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
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
				WHERE TagItemGuid = ?TagItemGuid
				ORDER BY TagText";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?TagItemGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = tagItemGuid.ToString()
				}
			}.ToArray();

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand,
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
				WHERE RelatedItemGuid = ?RelatedItemGuid
				AND ti.SiteGuid = ?SiteGuid
				ORDER BY TagText";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?RelatedItemGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = relatedItemGuid.ToString()
				},
				new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				}
			}.ToArray();

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand,
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
				WHERE ExtraGuid = ?ExtraGuid
				AND ti.SiteGuid = ?SiteGuid
				ORDER BY TagText";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?ExtraGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = extraGuid.ToString()
				},
				new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				}
			}.ToArray();

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand,
				arParams
			);
		}

		#endregion
	}
}