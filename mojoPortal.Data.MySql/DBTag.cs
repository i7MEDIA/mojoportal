using System;
using System.Collections.Generic;
using System.Data;

using MySql.Data.MySqlClient;


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
				@"INSERT INTO mp_Tag (
					Guid,
					SiteGuid,
					FeatureGuid,
					ModuleGuid,
					Tag,
					CreatedUtc,
					CreatedBy,
					ModifiedUtc,
					ModifiedBy,
					ItemCount,
					VocabularyGuid
				)

				VALUES (
					?Guid,
					?SiteGuid,
					?FeatureGuid,
					?ModuleGuid,
					?Tag,
					?CreatedUtc,
					?CreatedBy,
					?ModifiedUtc,
					?ModifiedBy,
					?ItemCount,
					?VocabularyGuid
				);";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?Guid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
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
				new MySqlParameter("?Tag", MySqlDbType.VarChar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = tagText
				},
				new MySqlParameter("?CreatedUtc", MySqlDbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = createdUtc
				},
				new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = createdBy.ToString()
				},
				new MySqlParameter("?ModifiedUtc", MySqlDbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = createdUtc
				},
				new MySqlParameter("?ModifiedBy", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = createdBy.ToString()
				},
				new MySqlParameter("?ItemCount", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = 0
				},
				new MySqlParameter("?VocabularyGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = vocabularyGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
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
				@"UPDATE mp_Tag
				SET
					Tag = ?Tag,
					ModifiedUtc = ?ModifiedUtc,
					ModifiedBy = ?ModifiedBy,
					VocabularyGuid = ?VocabularyGuid
				WHERE
					Guid = ?Guid;";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?Guid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				},
				new MySqlParameter("?Tag", MySqlDbType.VarChar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = tagText
				},
				new MySqlParameter("?ModifiedUtc", MySqlDbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = modifiedUtc
				},
				new MySqlParameter("?ModifiedBy", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = modifiedBy.ToString()
				},
				new MySqlParameter("?VocabularyGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = vocabularyGuid.ToString()
				}
			}.ToArray();

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				arParams
			);

			return rowsAffected > -1;
		}


		public static bool UpdateItemCount(Guid guid)
		{
			const string sqlCommand =
				@"UPDATE mp_Tag
				SET ItemCount = (
						SELECT Count(*)
						FROM   mp_TagItem
						WHERE  TagGuid = ?Guid
					)
				WHERE Guid = ?Guid;";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?Guid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
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
			const string sqlCommand = "DELETE FROM mp_Tag WHERE SiteGuid = ?SiteGuid;";

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

			return rowsAffected > 0;
		}


		public static bool Delete(Guid guid)
		{
			const string sqlCommand = "DELETE FROM mp_Tag WHERE Guid = ?Guid;";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?Guid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
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
			const string sqlCommand = "DELETE FROM mp_Tag WHERE ModuleGuid = ?ModuleGuid;";

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
			const string sqlCommand = "DELETE FROM mp_Tag WHERE FeatureGuid = ?FeatureGuid;";

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

		#endregion


		#region Get Methods

		public static IDataReader GetOneTag(Guid guid)
		{
			const string sqlCommand = "SELECT * FROM mp_Tag  WHERE Guid = ?Guid;";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?Guid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				}
			}.ToArray();

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
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
					sqlCommand = "SELECT Count(*) FROM mp_Tag WHERE ModuleGuid = ?Guid;";
					break;
				case "site":
				default:
					sqlCommand = "SELECT Count(*) FROM mp_Tag WHERE SiteGuid = ?Guid;";
					break;
				case "feature":
					sqlCommand = "SELECT Count(*) FROM mp_Tag WHERE FeatureGuid = ?Guid;";
					break;
			}

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?Guid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				}
			}.ToArray();

			return Convert.ToInt32(
				MySqlHelper.ExecuteScalar(
					ConnectionString.GetReadConnectionString(),
					sqlCommand,
					arParams
				)
			);
		}


		public static IDataReader GetBySite(Guid siteGuid)
		{
			const string sqlCommand = "SELECT * FROM mp_Tag WHERE SiteGuid = ?Guid ORDER BY Tag";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?Guid", MySqlDbType.VarChar, 36)
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


		public static IDataReader GetBySite(int siteId)
		{
			const string sqlCommand =
				@"SELECT *
				FROM mp_Tag
				WHERE SiteGuid = (
					SELECT SiteGuid
					FROM   mp_Sites
					WHERE  SiteID = ?ID
				)
				ORDER BY Tag";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?ID", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = siteId.ToString()
				}
			}.ToArray();

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand,
				arParams
			);
		}


		public static IDataReader GetByModule(Guid siteGuid, Guid moduleGuid)
		{
			const string sqlCommand = "SELECT * FROM mp_Tag WHERE ModuleGuid = ?ModuleGuid AND SiteGuid = ?SiteGuid ORDER BY Tag;";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = moduleGuid.ToString()
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


		public static IDataReader GetByFeature(Guid siteGuid, Guid featureGuid)
		{
			const string sqlCommand = "SELECT * FROM mp_Tag WHERE FeatureGuid = ?Guid AND SiteGuid = ?SiteGuid ORDER BY Tag";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?Guid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = featureGuid.ToString()
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


		public static IDataReader GetByVocabulary(Guid siteGuid, Guid vocabularyGuid)
		{
			const string sqlCommand = "SELECT * FROM mp_Tag WHERE VocabularyGuid = ?Guid AND SiteGuid = ?SiteGuid ORDER BY Tag";

			var arParams = new List<MySqlParameter>
			{
				new MySqlParameter("?Guid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = vocabularyGuid.ToString()
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
