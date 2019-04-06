using System;
using System.Data;


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
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Tag_Insert", 8);

			sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
			sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
			sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
			sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
			sph.DefineSqlParameter("@Tag", SqlDbType.NVarChar, 255, ParameterDirection.Input, tagText);
			sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
			sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
			sph.DefineSqlParameter("@VocabularyGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, vocabularyGuid);

			return sph.ExecuteNonQuery() > 0;
		}

		public static bool Update(
			Guid guid,
			string tagText,
			DateTime modifiedUtc,
			Guid modifiedBy,
			Guid vocabularyGuid
		)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Tag_Update", 5);

			sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
			sph.DefineSqlParameter("@Tag", SqlDbType.NVarChar, 255, ParameterDirection.Input, tagText);
			sph.DefineSqlParameter("@ModifiedUtc", SqlDbType.DateTime, ParameterDirection.Input, modifiedUtc);
			sph.DefineSqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, modifiedBy);
			sph.DefineSqlParameter("@VocabularyGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, vocabularyGuid);

			return sph.ExecuteNonQuery() > 0;
		}


		public static bool UpdateItemCount(Guid guid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Tag_UpdateItemCount", 1);

			sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);

			return sph.ExecuteNonQuery() > 0;
		}

		#endregion


		#region Delete Methods

		public static bool DeleteBySite(Guid siteGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Tag_DeleteBySite", 1);

			sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);

			return sph.ExecuteNonQuery() > 0;
		}


		public static bool Delete(Guid guid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Tag_Delete", 1);

			sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);

			return sph.ExecuteNonQuery() > 0;
		}


		public static bool DeleteByModule(Guid moduleGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Tag_DeleteByModule", 1);

			sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);

			return sph.ExecuteNonQuery() > 0;
		}


		public static bool DeleteByFeature(Guid featureGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Tag_DeleteByFeature", 1);

			sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);

			return sph.ExecuteNonQuery() > 0;
		}

		#endregion


		#region Get Methods

		public static IDataReader GetOneTag(Guid guid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Tag_SelectOne", 1);

			sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);

			return sph.ExecuteReader();
		}


		public static int GetCount(Guid guid, string type="site")
		{
			SqlParameterHelper sph = null;
			switch (type)
			{
				case "module":
					sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Tag_GetCountByModule", 1);
					sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
					break;
				case "site":
				default:
					sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Tag_GetCountBySite", 1);
					sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
					break;
				case "feature":
					sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Tag_GetCountByFeature", 1);
					sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
					break;
			}
			return Convert.ToInt32(sph.ExecuteScalar());
		}


		public static IDataReader GetBySite(Guid siteGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Tag_SelectBySiteGuid", 1);

			sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);

			return sph.ExecuteReader();
		}


		public static IDataReader GetBySite(int siteId)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Tag_SelectBySite", 1);

			sph.DefineSqlParameter("@ID", SqlDbType.Int, ParameterDirection.Input, siteId);

			return sph.ExecuteReader();
		}


		public static IDataReader GetByModule(Guid siteGuid, Guid moduleGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Tag_SelectByModule", 2);

			sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
			sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);

			return sph.ExecuteReader();
		}


		public static IDataReader GetByFeature(Guid siteGuid, Guid featureGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Tag_SelectByFeature", 2);

			sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
			sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);

			return sph.ExecuteReader();
		}


		public static IDataReader GetByVocabulary(Guid siteGuid, Guid vocabularyGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Tag_SelectByVocabulary", 2);

			sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, vocabularyGuid);
			sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);

			return sph.ExecuteReader();
		}

		#endregion
	}
}