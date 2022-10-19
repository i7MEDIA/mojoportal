using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Npgsql;
using NpgsqlTypes;

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
					tagitemguid,
					siteguid,
					featureguid,
					moduleguid,
					relatedttemguid,
					tagguid,
					extraguid,
					taggedby
				)

				VALUES (
					:tagitemguid,
					:siteguid,
					:featureguid,
					:moduleguid,
					:relateditemguid,
					:tagguid,
					:extraguid,
					:taggedby
				);";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":tagitemguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = tagItemGuid.ToString()
				},
				new NpgsqlParameter(":siteguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				},
				new NpgsqlParameter(":featureguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = featureGuid.ToString()
				},
				new NpgsqlParameter(":moduleguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = moduleGuid.ToString()
				},
				new NpgsqlParameter(":relateditemguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = relatedItemGuid.ToString()
				},
				new NpgsqlParameter(":tagguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = tagGuid.ToString()
				},
				new NpgsqlParameter(":extraguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = extraGuid.ToString()
				},
				new NpgsqlParameter(":taggedby", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = taggedBy.ToString()
				}
			}.ToArray();

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams
			);

			return rowsAffected > -1;
		}

		#endregion


		#region Delete Methods

		public static bool DeleteBySite(Guid siteGuid)
		{
			const string sqlCommand = "DELETE FROM mp_tagitem WHERE siteguid = :siteguid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":siteguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams
			);

			return rowsAffected > -1;
		}


		public static bool Delete(Guid tagItemGuid)
		{
			const string sqlCommand = @"DELETE FROM mp_tagitem WHERE tagitemguid = :tagitemguid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":tagitemguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = tagItemGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams
			);

			return rowsAffected > -1;
		}


		public static bool DeleteByTag(Guid tagGuid)
		{
			const string sqlCommand = @"DELETE FROM mp_tagitem WHERE tagguid = :tagguid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":tagguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = tagGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams
			);

			return rowsAffected > -1;
		}


		public static bool DeleteByModule(Guid moduleGuid)
		{
			const string sqlCommand = @"DELETE FROM mp_tagitem WHERE moduleguid = :moduleguid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":moduleguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = moduleGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams
			);

			return rowsAffected > -1;
		}


		public static bool DeleteByFeature(Guid featureGuid)
		{
			const string sqlCommand = @"DELETE FROM mp_tagitem WHERE featureguid = :featureguid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":featureguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = featureGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams
			);

			return rowsAffected > -1;
		}


		public static bool DeleteByRelatedItem(Guid relatedItemGuid)
		{
			const string sqlCommand = "DELETE FROM mp_tagitem WHERE relateditemguid = :relateditemguid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":itemguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = relatedItemGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams
			);

			return rowsAffected > -1;
		}


		public static bool DeleteByExtraGuid(Guid extraGuid)
		{
			const string sqlCommand = @"DELETE FROM mp_tagitem WHERE extraguid = :extraguid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":extraguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = extraGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams
			);

			return rowsAffected > -1;
		}

		#endregion


		#region Get Methods

		public static IDataReader GetByTagItem(Guid tagItemGuid)
		{
			const string sqlCommand =
				@"SELECT 
					ti.tagitemguid,
					ti.relateditemguid,
					ti.siteguid,
					ti.featureguid,
					ti.moduleguid,
					ti.tagguid,
					ti.extraguid,
					ti.taggedby,
					t.tag AS tagtext
				FROM mp_tagitem ti
				INNER JOIN mp_tag t
				ON ti.tagguid = t.guid
				WHERE tagitemguid = :tagitemguid
				ORDER BY tagtext";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":RelatedItemGuid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = tagItemGuid.ToString()
				}
			}.ToArray();

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams
			);
		}


		public static IDataReader GetByRelatedItem(Guid siteGuid, Guid relatedItemGuid)
		{
			const string sqlCommand =
				@"SELECT 
					ti.tagitemguid,
					ti.relateditemguid,
					ti.siteguid,
					ti.featureguid,
					ti.moduleguid,
					ti.tagguid,
					ti.extraguid,
					ti.taggedby,
					t.tag AS tagtext
				FROM mp_tagitem ti
				INNER JOIN mp_tag t
				ON ti.tagguid = t.tuid
				WHERE relateditemguid = :relateditemguid
				AND ti.siteguid = :siteguid
				ORDER BY tagtext";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":relateditemguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = relatedItemGuid.ToString()
				},
				new NpgsqlParameter(":siteguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				}
			}.ToArray();

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams
			);
		}


		public static IDataReader GetByExtra(Guid siteGuid, Guid extraGuid)
		{
			const string sqlCommand =
				@"SELECT 
					ti.tagitemguid,
					ti.relateditemguid,
					ti.siteguid,
					ti.featureguid,
					ti.moduleguid,
					ti.tagguid,
					ti.extraguid,
					ti.taggedby,
					t.tag AS tagtext
				FROM mp_tagitem ti
				INNER JOIN mp_tag t
				ON ti.tagguid = t.guid
				WHERE extraguid = :extraguid
				AND ti.siteguid = siteguid
				ORDER BY tagtext";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":relateditemguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = extraGuid.ToString()
				},
				new NpgsqlParameter(":siteguid", NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				}
			}.ToArray();

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams
			);
		}

		#endregion
	}
}
