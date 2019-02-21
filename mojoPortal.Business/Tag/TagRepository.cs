using mojoPortal.Data;

using System;
using System.Data;


namespace mojoPortal.Business
{
	public class TagRepository
	{
		#region Public Tag Methods

		public Tag SaveTag(Tag tag, out bool result)
		{
			if (tag.Guid == Guid.Empty)
			{
				tag.Guid = Guid.NewGuid();

				result = DBTag.Create(
					tag.Guid,
					tag.SiteGuid,
					tag.FeatureGuid,
					tag.ModuleGuid,
					tag.TagText,
					tag.CreatedUtc,
					tag.CreatedBy,
					tag.VocabularyGuid
				);
			}
			else
			{
				result = DBTag.Update(
					tag.Guid,
					tag.TagText,
					tag.ModifiedUtc,
					tag.ModifiedBy,
					tag.VocabularyGuid
				);
			}

			return tag;
		}

		public bool DeleteTagByGuid(Guid guid)
		{
			return DBTag.Delete(guid);
		}

		public bool DeleteTagByModuleGuid(Guid moduleGuid)
		{
			return DBTag.DeleteByModule(moduleGuid);
		}

		public bool DeleteTagByFeatureGuid(Guid featureGuid)
		{
			return DBTag.DeleteByFeature(featureGuid);
		}

		public bool DeleteTagBySiteGuid(Guid siteGuid)
		{
			return DBTag.DeleteBySite(siteGuid);
		}

		public Tag GetTagByGuid(Guid tagGuid)
		{
			using (IDataReader reader = DBTag.GetOneTag(tagGuid))
			{
				return getTagFromIDataReader(reader);
			}
		}

		public Tag GetTagByModuleGuid(Guid moduleGuid)
		{
			using (IDataReader reader = DBTag.GetByModule(moduleGuid))
			{
				return getTagFromIDataReader(reader);
			}
		}

		#endregion


		#region Public TagItem Methods

		public TagItem SaveTagItem(TagItem tagItem, out bool result)
		{
			tagItem.Guid = Guid.NewGuid();

			result = DBTagItem.Create(
				tagItem.Guid,
				tagItem.SiteGuid,
				tagItem.FeatureGuid,
				tagItem.ModuleGuid,
				tagItem.ItemGuid,
				tagItem.TagGuid,
				tagItem.ExtraGuid,
				tagItem.TaggedBy
			);

			return tagItem;
		}

		public bool DeleteTagItemByGuid(Guid guid)
		{
			return DBTagItem.Delete(guid);
		}

		public bool DeleteTagItemByItemGuid(Guid itemGuid)
		{
			return DBTagItem.DeleteByItem(itemGuid);
		}

		public bool DeleteTagItemByExtraGuid(Guid extraGuid)
		{
			return DBTagItem.DeleteByExtraGuid(extraGuid);
		}

		public bool DeleteTagItemByTagGuid(Guid tagGuid)
		{
			return DBTagItem.DeleteByTag(tagGuid);
		}

		public bool DeleteTagItemByModuleGuid(Guid moduleGuid)
		{
			return DBTagItem.DeleteByModule(moduleGuid);
		}

		public bool DeleteTagItemByFeatureGuid(Guid featureGuid)
		{
			return DBTagItem.DeleteByFeature(featureGuid);
		}

		public bool DeleteTagItemBySiteGuid(Guid siteGuid)
		{
			return DBTagItem.DeleteBySite(siteGuid);
		}

		#endregion


		#region Private Methods

		private Tag getTagFromIDataReader(IDataReader reader)
		{
			if (reader.Read())
			{
				return new Tag
				{
					Guid = new Guid(reader["Guid"].ToString()),
					SiteGuid = new Guid(reader["SiteGuid"].ToString()),
					FeatureGuid = new Guid(reader["FeatureGuid"].ToString()),
					ModuleGuid = new Guid(reader["ModuleGuid"].ToString()),
					TagText = reader["Tax"].ToString(),
					ItemCount = Convert.ToInt32(reader["Tag"]),
					CreatedUtc = Convert.ToDateTime(reader["CreatedUtc"]),
					CreatedBy = new Guid(reader["CreatedBy"].ToString()),
					ModifiedUtc = Convert.ToDateTime(reader["ModifiedUtc"]),
					ModifiedBy = new Guid(reader["ModifiedBy"].ToString()),
					VocabularyGuid = new Guid(reader["VocabularyGuid"].ToString())
				};
			}

			return null;
		}

		#endregion
	}
}