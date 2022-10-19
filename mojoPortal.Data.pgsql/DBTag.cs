using System;
using System.Collections.Generic;
using System.Data;

using Npgsql;


namespace mojoPortal.Data
{
	public static class DBTag
	{
		#region Create/Update Methods

		public static bool Create(
			Guid guid,
			Guid siteGuid,
			Guid featureGuid,
			Guid moduleGuid,
			string tagText,
			DateTime createdUtc,
			Guid createdBy,
			Guid vocabularyGuid
		)
		{
			const string sqlCommand =
				@"INSERT INTO mp_tag (
					guid,
					siteguid,
					featureguid,
					moduleguid,
					tag,
					createdutc,
					createdby,
					modifiedutc,
					modifiedby,
					itemcount,
					vocabularyguid
				)

				VALUES (
					:guid,
					:siteguid,
					:featureguid,
					:moduleguid,
					:tag,
					:createdutc,
					:createdby,
					:modifiedutc,
					:modifiedby,
					:itemcount,
					:vocabularyguid
				);";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				},
				new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				},
				new NpgsqlParameter(":featureguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = featureGuid.ToString()
				},
				new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = moduleGuid.ToString()
				},
				new NpgsqlParameter(":tag", NpgsqlTypes.NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = tagText
				},
				new NpgsqlParameter(":createdutc", NpgsqlTypes.NpgsqlDbType.Timestamp)
				{
					Direction = ParameterDirection.Input,
					Value = createdUtc
				},
				new NpgsqlParameter(":createdby", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = createdBy.ToString()
				},
				new NpgsqlParameter(":modifiedutc", NpgsqlTypes.NpgsqlDbType.Timestamp)
				{
					Direction = ParameterDirection.Input,
					Value = createdUtc
				},
				new NpgsqlParameter(":modifiedby", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = createdBy.ToString()
				},
				new NpgsqlParameter(":itemcount", NpgsqlTypes.NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = 0
				},
				new NpgsqlParameter(":vocabularyguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = vocabularyGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > -1;
		}


		public static bool Update(
			Guid guid,
			string tagText,
			DateTime modifiedUtc,
			Guid modifiedBy,
			Guid vocabularyGuid
		)
		{
			const string sqlCommand =
				@"UPDATE mp_tag
				SET 
					tag = :tag,
					modifiedutc = :modifiedutc,
					modifiedby = :modifiedby,
					vocabularyguid = :vocabularyguid
				WHERE
					guid = :guid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				},
				new NpgsqlParameter(":tag", NpgsqlTypes.NpgsqlDbType.Varchar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = tagText
				},
				new NpgsqlParameter(":modifiedutc", NpgsqlTypes.NpgsqlDbType.Timestamp)
				{
					Direction = ParameterDirection.Input,
					Value = modifiedUtc
				},
				new NpgsqlParameter(":modifiedby", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = modifiedBy.ToString()
				},
				new NpgsqlParameter(":vocabularyguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = vocabularyGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > -1;
		}


		public static bool UpdateItemCount(Guid guid)
		{
			const string sqlCommand =
				@"UPDATE mp_Tag
				SET itemcount = ( 
						SELECT Count(*)
						FROM   mp_tagitem
						WHERE  tagguid = :guid
					)
				WHERE guid = :guid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				}
			}.ToArray();

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > -1;
		}

		#endregion


		#region Delete Methods

		public static bool DeleteBySite(Guid siteGuid)
		{
			const string sqlCommand = "DELETE FROM mp_tag WHERE siteguid = :siteguid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > -1;
		}


		public static bool Delete(Guid guid)
		{
			const string sqlCommand = "DELETE FROM mp_tag WHERE guid = :guid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				}
			}.ToArray();

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > -1;
		}


		public static bool DeleteByModule(Guid moduleGuid)
		{
			const string sqlCommand = "DELETE FROM mp_tag WHERE moduleguid = :moduleguid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = moduleGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > -1;
		}


		public static bool DeleteByFeature(Guid featureGuid)
		{
			const string sqlCommand = "DELETE FROM mp_tag WHERE featureguid = :featureguid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":featureguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = featureGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > -1;
		}

		#endregion


		#region Get Methods

		public static IDataReader GetOneTag(Guid guid)
		{
			const string sqlCommand = "SELECT * FROM mp_tag WHERE guid = :guid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				}
			}.ToArray();

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);
		}


		public static int GetCount(Guid guid, string type = "site")
		{
			var sqlCommand = string.Empty;

			switch (type)
			{
				case "module":
					sqlCommand = "SELECT Count(*) FROM mp_tag WHERE moduleguid = :guid;";
					break;
				case "site":
				default:
					sqlCommand = "SELECT Count(*) FROM mp_tag WHERE siteguid = :guid;";
					break;
				case "feature":
					sqlCommand = "SELECT Count(*) FROM mp_tag WHERE featureguid = :guid;";
					break;
			}

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				}
			}.ToArray();

			return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
				)
			);
		}


		public static IDataReader GetBySite(Guid siteGuid)
		{
			const string sqlCommand = "SELECT * FROM mp_tag WHERE siteuid = :guid ORDER BY tag";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				}
			}.ToArray();

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);
		}


		public static IDataReader GetBySite(int siteId)
		{
			const string sqlCommand =
				@"SELECT *
				FROM mp_tag
				WHERE siteguid = (
					SELECT siteguid
					FROM   mp_sites
					WHERE  siteid = :id
				)
				ORDER BY tag";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":id", NpgsqlTypes.NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = siteId.ToString()
				}
			}.ToArray();

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);
		}


		public static IDataReader GetByModule(Guid siteGuid, Guid moduleGuid)
		{
			const string sqlCommand = "SELECT * FROM mp_tag WHERE moduleguid = :moduleguid AND siteguid == :siteguid ORDER BY tag;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = moduleGuid.ToString()
				},
				new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				}
			}.ToArray();

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);
		}


		public static IDataReader GetByFeature(Guid siteGuid, Guid featureGuid)
		{
			const string sqlCommand = "SELECT * FROM mp_tag WHERE featureguid = :guid AND siteguid == :siteguid ORDER BY tag";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = featureGuid.ToString()
				},
				new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				}
			}.ToArray();

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);
		}


		public static IDataReader GetByVocabulary(Guid siteGuid, Guid vocabularyGuid)
		{
			const string sqlCommand = "SELECT * FROM mp_tag WHERE vocabularyguid = :guid AND siteguid == :siteguid ORDER BY tag";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = vocabularyGuid.ToString()
				},
				new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				}
			}.ToArray();

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);
		}

		#endregion
	}
}