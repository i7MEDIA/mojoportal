using mojoPortal.Data;

using System;
using System.Collections.Generic;
using System.Data;


namespace mojoPortal.Business
{
	public static class TagRepository
	{
		#region Public Tag Methods

		public static Tag SaveTag(Tag tag, out bool result)
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

		public static bool DeleteTagByGuid(Guid guid)
		{
			return DBTag.Delete(guid);
		}

		public static bool DeleteTagByModuleGuid(Guid moduleGuid)
		{
			return DBTag.DeleteByModule(moduleGuid);
		}

		public static bool DeleteTagByFeatureGuid(Guid featureGuid)
		{
			return DBTag.DeleteByFeature(featureGuid);
		}

		public static bool DeleteTagBySiteGuid(Guid siteGuid)
		{
			return DBTag.DeleteBySite(siteGuid);
		}

		public static Tag GetTagByGuid(Guid tagGuid)
		{
			using (IDataReader reader = DBTag.GetOneTag(tagGuid))
			{
				return getTagFromIDataReader(reader);
			}
		}

		public static List<Tag> GetTagsByModuleGuid(Guid siteGuid, Guid moduleGuid)
		{
			return getTagListFromIDataReader(DBTag.GetByModule(siteGuid, moduleGuid));
		}

		public static List<Tag> GetTagsByFeatureGuid(Guid siteGuid, Guid featureGuid)
		{
			return getTagListFromIDataReader(DBTag.GetByFeature(siteGuid, featureGuid));
		}

		public static List<Tag> GetTagsBySite(Guid siteGuid)
		{
			return getTagListFromIDataReader(DBTag.GetBySite(siteGuid));
		}

		public static List<Tag> GetTagsBySite(int siteId)
		{
			return getTagListFromIDataReader(DBTag.GetBySite(siteId));
		}

		public static List<Tag> GetTagsByVocabulary(Guid siteGuid, Guid vocabularyGuid)
		{
			return getTagListFromIDataReader(DBTag.GetByVocabulary(siteGuid, vocabularyGuid));
		}

		/// <summary>
		/// Gets count of Tag instances by chosen type. Default is 'site'.
		/// </summary>
		/// <param name="guid"></param>
		/// <param name="type">site,module,feature</param>
		/// <returns></returns>
		public static int GetTagCount(Guid guid, string type = "site")
		{
			return DBTag.GetCount(guid, type);
		}

		#endregion


		#region Public TagItem Methods

		public static TagItem SaveTagItem(TagItem tagItem, out bool result)
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

		public static bool DeleteTagItemByGuid(Guid guid)
		{
			return DBTagItem.Delete(guid);
		}

		public static bool DeleteTagItemByItemGuid(Guid itemGuid)
		{
			return DBTagItem.DeleteByRelatedItem(itemGuid);
		}

		public static bool DeleteTagItemByExtraGuid(Guid extraGuid)
		{
			return DBTagItem.DeleteByExtraGuid(extraGuid);
		}

		public static bool DeleteTagItemByTagGuid(Guid tagGuid)
		{
			return DBTagItem.DeleteByTag(tagGuid);
		}

		public static bool DeleteTagItemByModuleGuid(Guid moduleGuid)
		{
			return DBTagItem.DeleteByModule(moduleGuid);
		}

		public static bool DeleteTagItemByFeatureGuid(Guid featureGuid)
		{
			return DBTagItem.DeleteByFeature(featureGuid);
		}

		public static bool DeleteTagItemBySiteGuid(Guid siteGuid)
		{
			return DBTagItem.DeleteBySite(siteGuid);
		}

		public static List<TagItem> GetTagItemsByRelatedItemGuid(Guid siteGuid, Guid relatedItemGuid)
		{
			return getTagItemListFromIDataReader(DBTagItem.GetByRelatedItem(siteGuid, relatedItemGuid));
		}

		public static List<TagItem> GetTagItemsByExtraGuid(Guid siteGuid, Guid extraGuid)
		{
			return getTagItemListFromIDataReader(DBTagItem.GetByExtra(siteGuid, extraGuid));
		}

		public static TagItem GetTagItemByTagItemGuid(Guid tagItemGuid)
		{
			return getTagItemFromIDataReader(DBTagItem.GetByTagItem(tagItemGuid));
		}

		#endregion


		#region Private Methods

		private static Tag getTagFromIDataReader(IDataReader reader)
		{
			if (reader.Read())
			{
				return mapReaderToTag(reader);
			}

			return new Tag();
		}

		private static List<Tag> getTagListFromIDataReader(IDataReader reader)
		{
			List<Tag> tags = new List<Tag>();

			while (reader.Read())
			{
				tags.Add(mapReaderToTag(reader));
			}

			return tags;
		}

		private static Tag mapReaderToTag(IDataReader reader)
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

		private static TagItem getTagItemFromIDataReader(IDataReader reader)
		{
			if (reader.Read())
			{
				return mapReaderToTagItem(reader);
			}

			return new TagItem();
		}

		private static List<TagItem> getTagItemListFromIDataReader(IDataReader reader)
		{
			List<TagItem> tagItems = new List<TagItem>();

			while (reader.Read())
			{
				tagItems.Add(mapReaderToTagItem(reader));
			}

			return tagItems;
		}

		private static TagItem mapReaderToTagItem(IDataReader reader)
		{
			return new TagItem
			{
				TagItemGuid = new Guid(reader["TagItemGuid"].ToString()),
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

		#endregion
	}
}