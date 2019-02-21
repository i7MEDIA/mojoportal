using System;
using System.Data;


namespace mojoPortal.Data
{
	public static class DBTagItem
	{
		/// <summary>
		/// Inserts a row in the mp_TagItem table. Returns rows affected count.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <param name="siteGuid"> siteGuid </param>
		/// <param name="featureGuid"> featureGuid </param>
		/// <param name="moduleGuid"> moduleGuid </param>
		/// <param name="itemGuid"> itemGuid </param>
		/// <param name="tagGuid"> tagGuid </param>
		/// <param name="extraGuid"> extraGuid </param>
		/// <param name="taggedBy"> taggedBy </param>
		/// <returns>int</returns>
		public static bool Create(
			Guid guid,
			Guid siteGuid,
			Guid featureGuid,
			Guid moduleGuid,
			Guid itemGuid,
			Guid tagGuid,
			Guid extraGuid,
			Guid taggedBy
		)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TagItem_Insert", 8);

			sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
			sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
			sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
			sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
			sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
			sph.DefineSqlParameter("@TagGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, tagGuid);
			sph.DefineSqlParameter("@ExtraGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, extraGuid);
			sph.DefineSqlParameter("@TaggedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, taggedBy);

			return sph.ExecuteNonQuery() > 0;
		}


		/// <summary>
		/// Deletes a row from the mp_TagItem table. Returns true if row deleted.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <returns>bool</returns>
		public static bool Delete(Guid guid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TagItem_Delete", 1);

			sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);

			return sph.ExecuteNonQuery() > 0;
		}


		/// <summary>
		/// Deletes rows from the mp_TagItem table. Returns true if row deleted.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <returns>bool</returns>
		public static bool DeleteByItem(Guid itemGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TagItem_DeleteByItem", 1);

			sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);

			return sph.ExecuteNonQuery() > 0;
		}


		/// <summary>
		/// Deletes rows from the mp_TagItem table. Returns true if row deleted.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <returns>bool</returns>
		public static bool DeleteByExtraGuid(Guid extraGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TagItem_DeleteByExtraGuid", 1);

			sph.DefineSqlParameter("@ExtraGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, extraGuid);

			return sph.ExecuteNonQuery() > 0;
		}


		/// <summary>
		/// Deletes rows from the mp_TagItem table. Returns true if row deleted.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <returns>bool</returns>
		public static bool DeleteByTag(Guid tagGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TagItem_DeleteByTag", 1);

			sph.DefineSqlParameter("@TagGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, tagGuid);

			return sph.ExecuteNonQuery() > 0;
		}


		/// <summary>
		/// Deletes rows from the mp_TagItem table. Returns true if row deleted.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <returns>bool</returns>
		public static bool DeleteByModule(Guid moduleGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TagItem_DeleteByModule", 1);

			sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);

			return sph.ExecuteNonQuery() > 0;
		}


		/// <summary>
		/// Deletes rows from the mp_TagItem table. Returns true if row deleted.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <returns>bool</returns>
		public static bool DeleteByFeature(Guid featureGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TagItem_DeleteByFeature", 1);

			sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);

			return sph.ExecuteNonQuery() > 0;
		}


		/// <summary>
		/// Deletes rows from the mp_TagItem table. Returns true if row deleted.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <returns>bool</returns>
		public static bool DeleteBySite(Guid siteGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TagItem_DeleteBySite", 1);

			sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);

			return sph.ExecuteNonQuery() > 0;
		}
	}
}