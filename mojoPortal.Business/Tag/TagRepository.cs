using mojoPortal.Data;

using System;
using System.Collections.Generic;
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

		public List<Tag> GetTagsByModuleGuid(Guid moduleGuid)
		{
			return getTagListFromIDataReader(DBTag.GetByModule(moduleGuid));
		}

		public List<Tag> GetTagsByFeatureGuid(Guid featureGuid)
		{
			return getTagListFromIDataReader(DBTag.GetByFeature(featureGuid));
		}

		public List<Tag> GetTagsBySite(Guid siteGuid)
		{
			return getTagListFromIDataReader(DBTag.GetBySite(siteGuid));
		}

		public List<Tag> GetTagsBySite(int siteId)
		{
			return getTagListFromIDataReader(DBTag.GetBySite(siteId));
		}

		public List<Tag> GetTagsByVocabulary(Guid vocabularyGuid)
		{
			return getTagListFromIDataReader(DBTag.GetByVocabulary(vocabularyGuid));
		}

		/// <summary>
		/// Gets count of Tag instances by chosen type. Default is 'site'.
		/// </summary>
		/// <param name="guid"></param>
		/// <param name="type">site,module,feature</param>
		/// <returns></returns>
		public int GetTagCount(Guid guid, string type="site")
		{
			return DBTag.GetCount(guid, type);
		}

		#endregion


		#region Public TagItem Methods

		public TagItem SaveTagItem(TagItem tagItem, out bool result)
		{
			tagItem.TagItemGuid = Guid.NewGuid();

			result = DBTagItem.Create(
				tagItem.TagItemGuid,
				tagItem.SiteGuid,
				tagItem.FeatureGuid,
				tagItem.ModuleGuid,
				tagItem.RelatedItemGuid,
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

		public List<TagItem> GetTagItemsByRelatedItemGuid(Guid relatedItemGuid)
		{
			return getTagItemListFromIDataReader(DBTagItem.GetByRelatedItem(relatedItemGuid));
		}

		public List<TagItem> GetTagItemsByRelatedExtraGuid(Guid extraGuid)
		{
			return getTagItemListFromIDataReader(DBTagItem.GetByExtra(extraGuid));
		}

		public TagItem GetTagItemByTagItemGuid(Guid tagItemGuid)
		{
			return getTagItemFromIDataReader(DBTagItem.GetByTagItem(tagItemGuid));
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
					TagText = reader["Tag"].ToString(),
					ItemCount = Convert.ToInt32(reader["ItemCount"]),
					CreatedUtc = Convert.ToDateTime(reader["CreatedUtc"]),
					CreatedBy = new Guid(reader["CreatedBy"].ToString()),
					ModifiedUtc = Convert.ToDateTime(reader["ModifiedUtc"]),
					ModifiedBy = new Guid(reader["ModifiedBy"].ToString()),
					VocabularyGuid = new Guid(reader["VocabularyGuid"].ToString())
				};
			}

			return null;
		}

		private List<Tag> getTagListFromIDataReader(IDataReader reader)
		{
			List<Tag> tags = new List<Tag>();

			while (reader.Read())
			{
				tags.Add(getTagFromIDataReader(reader));
			}

			return tags;
		}

		private TagItem getTagItemFromIDataReader(IDataReader reader)
		{
			if (reader.Read())
			{
				return new TagItem
				{
					TagItemGuid = new Guid(reader["Guid"].ToString()),
					RelatedItemGuid = new Guid(reader["RelatedItemGuid"].ToString()),
					SiteGuid = new Guid(reader["SiteGuid"].ToString()),
					FeatureGuid = new Guid(reader["FeatureGuid"].ToString()),
					ModuleGuid = new Guid(reader["ModuleGuid"].ToString()),
					TagGuid = new Guid(reader["TagGuid"].ToString()),
					ExtraGuid = new Guid(reader["ExtraGuid"].ToString()),
					TaggedBy = new Guid(reader["TaggedBy"].ToString()),
					TagText = reader["TagText"].ToString(),
				};
			}

			return null;
		}

		private List<TagItem> getTagItemListFromIDataReader(IDataReader reader)
		{
			List<TagItem> tagItems = new List<TagItem>();

			while (reader.Read())
			{
				tagItems.Add(getTagItemFromIDataReader(reader));
			}

			return tagItems;
		}

		#endregion
	}
}