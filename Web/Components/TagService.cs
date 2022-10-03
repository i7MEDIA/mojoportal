using System;
using System.Collections.Generic;
using System.Linq;

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;


namespace mojoPortal.Web.Tags
{
	public class TagService
	{
		#region Private Properties

		private Guid _featureGuid;
		private Guid _moduleGuid;
		private SelectTagByType _selectTagByType;
		private SiteUser currentUser = null;
		private SiteSettings siteSettings = null;
		private bool allowed = true;

		#endregion

		/// <summary>
		/// Inistiallizes instance of TagService
		/// </summary>
		/// <param name="featureGuid">featureGuid</param>
		/// <param name="moduleGuid">moduleGuid</param>
		/// <param name="selectTagByType">selectTagByType</param>
		public TagService(Guid featureGuid, Guid moduleGuid, SelectTagByType selectTagByType)
		{
			_featureGuid = featureGuid;
			_moduleGuid = moduleGuid;
			_selectTagByType = selectTagByType;

			siteSettings = CacheHelper.GetCurrentSiteSettings();
			currentUser = SiteUtils.GetCurrentSiteUser();

			if (
				!WebUser.IsAdminOrContentAdmin
				|| !SiteUtils.UserIsSiteEditor()
				|| !WebUser.IsInRoles(siteSettings.TagManagementRoles)
			)
			{
				allowed = false;
			}
		}


		#region Public Methods

		/// <summary>
		/// Gets list of TagModel which has the tag text and guid properties. Selected is always false.
		/// </summary>
		/// <returns>List<TagModel></returns>
		public List<TagModel> GetTagList()
		{
			var tags = selectTags();
			var returnModel = new List<TagModel>();

			foreach (var tag in tags)
			{
				var tagModel = new TagModel
				{
					TagText = tag.TagText,
					TagGuid = tag.Guid,
					Selected = false
				};

				returnModel.Add(tagModel);
			}

			return returnModel;
		}


		/// <summary>
		/// Gets list of TagModel which has the tag text, guid, and selected properties.
		/// </summary>
		/// <param name="guid">The RelatedItemGuid or ExtraGuid.</param>
		/// <param name="selectTagItemType">RelatedItem or Extra</param>
		/// <returns>List<TagModel></returns>
		public List<TagModel> GetTagListWithSelections(Guid guid, SelectTagItemByType selectTagItemType)
		{
			var tags = selectTags();
			var tagItems = selectTagItems(guid, selectTagItemType);
			var returnModel = new List<TagModel>();

			foreach (var tag in tags)
			{
				var tagModel = new TagModel
				{
					TagText = tag.TagText,
					TagGuid = tag.Guid,
					Selected = tagItems.Where(tagItem => tagItem.TagGuid == tag.Guid).Count() > 0
				};

				returnModel.Add(tagModel);
			}

			return returnModel;
		}


		/// <summary>
		/// Takes a list of TagModel and creates/deletes any tags based on what already exists in the DB.
		/// </summary>
		/// <param name="guid">The RelatedItemGuid or ExtraGuid.</param>
		/// <param name="selectTagItemType">RelatedItem or Extra</param>
		/// <param name="returnedTags">List of the tags returned to this method.</param>
		public void UpdateTags(Guid guid, SelectTagItemByType selectTagItemType, List<TagModel> returnedTags)
		{
			if (!allowed) return;

			var tagItems = selectTagItems(guid, selectTagItemType);
			var selectedTags = returnedTags.Where(tag => tag.Selected).ToList();
			var tagItemsToSave = getTagsToSave(selectedTags, tagItems);
			var tagItemsToDelete = getTagItemsToDelete(selectedTags, tagItems);

			// Create new TagItems
			foreach (var tag in tagItemsToSave)
			{
				CreateTagItem(tag.TagGuid, guid, selectTagItemType);
			}

			// Delete TagItems
			foreach (var tag in tagItemsToDelete)
			{
				DeleteTagItem(tag.TagItemGuid);
			}
		}


		/// <summary>
		/// Creates a single tag.
		/// </summary>
		/// <param name="tagText">Text of tag to be created.</param>
		/// <returns>Returns true if successful and the new tag.</returns>
		public (bool, Tag) CreateTag(string tagText)
		{
			if (!allowed) return (false, null);

			var tag = new Tag
			{
				SiteGuid = siteSettings.SiteGuid,
				FeatureGuid = _featureGuid,
				ModuleGuid = _moduleGuid,
				VocabularyGuid = Guid.Empty,
				TagText = tagText.Trim(),
				CreatedBy = currentUser.UserGuid,
				ModifiedBy = currentUser.UserGuid,
				ModifiedUtc = DateTime.UtcNow
			};

			var newTag = TagRepository.SaveTag(tag, out bool result);

			return (result, newTag);
		}


		/// <summary>
		/// Updates a specific tag.
		/// </summary>
		/// <param name="tagText">New tag text.</param>
		/// <param name="guid">Guid of tag to update.</param>
		/// <returns>Returns true if successful and the new tag.</returns>
		public (bool, Tag) UpdateTag(string tagText, Guid guid)
		{
			if (!allowed) return (false, null);

			var oldTag = TagRepository.GetTagByGuid(guid);

			oldTag.TagText = tagText.Trim();
			oldTag.ModifiedBy = currentUser.UserGuid;
			oldTag.ModifiedUtc = DateTime.UtcNow;

			var newTag = TagRepository.SaveTag(oldTag, out bool result);

			return (result, newTag);
		}


		/// <summary>
		/// Creates new TagItem
		/// </summary>
		/// <param name="tagGuid">The related TagGuid of the TagItem to create.</param>
		/// <param name="guid">The RelatedItemGuid or ExtraGuid.</param>
		/// <param name="selectTagItemType">RelatedItem or Extra</param>
		public void CreateTagItem(Guid tagGuid, Guid guid, SelectTagItemByType selectTagItemType)
		{
			if (!allowed) return;

			var tagItem = new TagItem
			{
				SiteGuid = siteSettings.SiteGuid,
				FeatureGuid = _featureGuid,
				ModuleGuid = _moduleGuid,
				RelatedItemGuid = (selectTagItemType == SelectTagItemByType.RelatedItem) ? guid : Guid.Empty,
				ExtraGuid = (selectTagItemType == SelectTagItemByType.Extra) ? guid : Guid.Empty,
				TagGuid = tagGuid,
				TaggedBy = currentUser.UserGuid
			};

			TagRepository.SaveTagItem(tagItem, out bool result);
		}


		/// <summary>
		/// Deletes a single tag guid by it's GUID.
		/// </summary>
		/// <param name="tagGuid"></param>
		/// <returns>Return true if successful.</returns>
		public bool DeleteTag(Guid tagGuid)
		{
			if (!allowed) return false;

			return TagRepository.DeleteTagByGuid(tagGuid);
		}


		/// <summary>
		/// Deletes a single tag item by it's GUID.
		/// </summary>
		/// <param name="tagItemGuid"></param>
		public void DeleteTagItem(Guid tagItemGuid)
		{
			if (!allowed) return;

			TagRepository.DeleteTagItemByGuid(tagItemGuid);
		}

		#endregion


		#region Private Methods

		private List<Tag> selectTags()
		{
			var tags = new List<Tag>();

			switch (_selectTagByType)
			{
				case SelectTagByType.Site:
					tags = TagRepository.GetTagsBySite(siteSettings.SiteGuid);
					break;
				case SelectTagByType.Feature:
					tags = TagRepository.GetTagsByFeatureGuid(siteSettings.SiteGuid, _moduleGuid);
					break;
				case SelectTagByType.Module:
					tags = TagRepository.GetTagsByModuleGuid(siteSettings.SiteGuid, _moduleGuid);
					break;
			}

			return tags;
		}


		private List<TagItem> selectTagItems(Guid guid, SelectTagItemByType selectTagItemType)
		{
			var tagItems = new List<TagItem>();

			switch (selectTagItemType)
			{
				case SelectTagItemByType.RelatedItem:
					tagItems = TagRepository.GetTagItemsByRelatedItemGuid(siteSettings.SiteGuid, guid);
					break;
				case SelectTagItemByType.Extra:
					tagItems = TagRepository.GetTagItemsByExtraGuid(siteSettings.SiteGuid, guid);
					break;
			}

			return tagItems;
		}


		private List<TagModel> getTagsToSave(List<TagModel> tags, List<TagItem> tagItems)
		{
			var tagItemsToSave = new List<TagModel>();

			// Loop through the tags than were returned
			foreach (var tag in tags)
			{
				// The tag will be set as new unless it is found in the TagItems from the DB.
				var newTag = true;

				foreach (var tagItem in tagItems)
				{
					if (tagItem.TagGuid == tag.TagGuid) newTag = false;
				}

				// Add new tag to list for adding in the next loop
				if (newTag) tagItemsToSave.Add(tag);
			}

			return tagItemsToSave;
		}


		private List<TagItem> getTagItemsToDelete(List<TagModel> tags, List<TagItem> tagItems)
		{
			var tagItemsToDelete = new List<TagItem>();

			// Loop through TagItems in the DB
			foreach (var tagItem in tagItems)
			{
				// The TagItem will be set to be deleted unless it is found in the new tags that were returned.
				var deleteTag = true;

				foreach (var tag in tags)
				{
					if (tag.TagGuid == tagItem.TagGuid) deleteTag = false;
				}

				// If the item was not found, add tag to be deleted
				if (deleteTag) tagItemsToDelete.Add(tagItem);
			}

			return tagItemsToDelete;
		}

		#endregion
	}


	public class TagModel
	{
		public string TagText { get; set; } = string.Empty;
		public Guid TagGuid { get; set; } = Guid.Empty;
		public Guid ChannelGuid { get; set; } = Guid.Empty;
		public bool Selected { get; set; } = false;
	}


	public enum SelectTagByType
	{
		Site,
		Module,
		Feature
	}


	public enum SelectTagItemByType
	{
		RelatedItem,
		Extra
	}
}