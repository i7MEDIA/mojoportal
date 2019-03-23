using System;
using System.Data;


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
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TagItem_Insert", 8);//change params in DB

			sph.DefineSqlParameter("@TagItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, tagItemGuid);
			sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
			sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
			sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
			sph.DefineSqlParameter("@RelatedItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, relatedItemGuid);
			sph.DefineSqlParameter("@TagGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, tagGuid);
			sph.DefineSqlParameter("@ExtraGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, extraGuid);
			sph.DefineSqlParameter("@TaggedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, taggedBy);

			return sph.ExecuteNonQuery() > 0;
		}

		#endregion


		#region Delete Methods

		public static bool DeleteBySite(Guid siteGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TagItem_DeleteBySite", 1);

			sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);

			return sph.ExecuteNonQuery() > 0;
		}


		public static bool Delete(Guid tagItemGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TagItem_Delete", 1);//change params in DB

			sph.DefineSqlParameter("@TagItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, tagItemGuid);

			return sph.ExecuteNonQuery() > 0;
		}


		public static bool DeleteByTag(Guid tagGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TagItem_DeleteByTag", 1);

			sph.DefineSqlParameter("@TagGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, tagGuid);

			return sph.ExecuteNonQuery() > 0;
		}


		public static bool DeleteByModule(Guid moduleGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TagItem_DeleteByModule", 1);

			sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);

			return sph.ExecuteNonQuery() > 0;
		}


		public static bool DeleteByFeature(Guid featureGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TagItem_DeleteByFeature", 1);

			sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);

			return sph.ExecuteNonQuery() > 0;
		}


		public static bool DeleteByRelatedItem(Guid relatedItemGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TagItem_DeleteByRelatedItem", 1);

			sph.DefineSqlParameter("@RelatedItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, relatedItemGuid);

			return sph.ExecuteNonQuery() > 0;
		}


		public static bool DeleteByExtraGuid(Guid extraGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TagItem_DeleteByExtraGuid", 1);

			sph.DefineSqlParameter("@ExtraGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, extraGuid);

			return sph.ExecuteNonQuery() > 0;
		}

		#endregion


		#region Get Methods

		public static IDataReader GetByTagItem(Guid tagItemGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TagItem_GetByTagItem", 1);

			sph.DefineSqlParameter("@TagItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, tagItemGuid);

			return sph.ExecuteReader();
		}


		public static IDataReader GetByRelatedItem(Guid siteGuid, Guid relatedItemGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TagItem_GetByRelatedItem", 2);

			sph.DefineSqlParameter("@RelatedItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, relatedItemGuid);
			sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);

			return sph.ExecuteReader();
		}


		public static IDataReader GetByExtra(Guid siteGuid, Guid extraGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_TagItem_GetByExtra", 2);

			sph.DefineSqlParameter("@ExtraGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, extraGuid);
			sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);

			return sph.ExecuteReader();
		}

		#endregion
	}
}