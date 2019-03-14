using System;
using mojoPortal.Business;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.Tags
{
	public class TagService
	{
		#region Private Properties

		private Guid _guid;
		private TagScopeType _selectionType;
		private SiteUser currentUser = null;
		private SiteSettings siteSettings = null;
		private bool allowed = true;

		#endregion


		public TagService(Guid guid, TagScopeType selectionType)
		{
			_guid = guid;
			_selectionType = selectionType;

			siteSettings = CacheHelper.GetCurrentSiteSettings();
			currentUser = SiteUtils.GetCurrentSiteUser();

			if (
				!WebUser.IsAdminOrContentAdmin ||
				!SiteUtils.UserIsSiteEditor() ||
				!WebUser.IsInRoles(siteSettings.TagManagementRoles))
			{
				allowed = false;
			}
		}


		#region Public Methods

		public List<TagModel> GetTagList()
		{
			var tags = returnSelectionTags();
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


		public List<TagModel> GetTagListWithSelections(Guid guid, TagItemScopeType tagItemType)
		{
			var tags = returnSelectionTags();
			var tagItems = returnSelectionTagItems(guid, tagItemType);
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


		public void UpdateTags(Guid guid, TagItemScopeType tagItemType, List<TagModel> returnedTags)
		{
			if (!allowed) return;

			var tagItems = returnSelectionTagItems(guid, tagItemType);
			var selectedTags = returnedTags.Where(tag => tag.Selected).ToList();
			var tagItemsToSave = getTagsToSave(selectedTags, tagItems);
			var tagItemsToDelete = getTagItemsToDelete(selectedTags, tagItems);

			// Create new TagItems
			foreach (var tag in tagItemsToSave)
			{
				CreateTagItem(tag.TagGuid, guid, tagItemType);
			}

			// Delete TagItems
			foreach (var tag in tagItemsToDelete)
			{
				DeleteTagItem(tag.TagItemGuid);
			}
		}


		public (bool, Tag) CreateTag(string tagText)
		{
			if (!allowed) return (false, null);

			var tag = new Tag
			{
				SiteGuid = siteSettings.SiteGuid,
				FeatureGuid = (_selectionType == TagScopeType.Feature) ? _guid : Guid.Empty,
				ModuleGuid = (_selectionType == TagScopeType.Module) ? _guid : Guid.Empty,
				VocabularyGuid = Guid.Empty,
				TagText = tagText.Trim(),
				CreatedBy = currentUser.UserGuid,
				ModifiedBy = currentUser.UserGuid,
				ModifiedUtc = DateTime.UtcNow
			};

			var newTag = TagRepository.SaveTag(tag, out bool result);

			return (result, newTag);
		}


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


		public void CreateTagItem(Guid tagGuid, Guid guid, TagItemScopeType tagItemType)
		{
			if (!allowed) return;

			var tagItem = new TagItem
			{
				SiteGuid = siteSettings.SiteGuid,
				FeatureGuid = (_selectionType == TagScopeType.Feature) ? _guid : Guid.Empty,
				ModuleGuid = (_selectionType == TagScopeType.Module) ? _guid : Guid.Empty,
				RelatedItemGuid = (tagItemType == TagItemScopeType.RelatedItem) ? guid : Guid.Empty,
				ExtraGuid = (tagItemType == TagItemScopeType.Extra) ? guid : Guid.Empty,
				TagGuid = tagGuid,
				TaggedBy = currentUser.UserGuid
			};

			TagRepository.SaveTagItem(tagItem, out bool result);
		}


		public bool DeleteTag(Guid tagGuid)
		{
			if (!allowed) return false;

			return TagRepository.DeleteTagByGuid(tagGuid);
		}


		public void DeleteTagItem(Guid tagItemGuid)
		{
			if (!allowed) return;

			TagRepository.DeleteTagItemByGuid(tagItemGuid);
		}

		#endregion


		#region Private Methods

		private List<Tag> returnSelectionTags()
		{
			var tags = new List<Tag>();

			switch (_selectionType)
			{
				case TagScopeType.Site:
					tags = TagRepository.GetTagsBySite(_guid);
					break;
				case TagScopeType.Module:
					tags = TagRepository.GetTagsByModuleGuid(_guid);
					break;
				case TagScopeType.Feature:
					tags = TagRepository.GetTagsByFeatureGuid(_guid);
					break;
			}

			return tags;
		}

		private List<TagItem> returnSelectionTagItems(Guid guid, TagItemScopeType tagItemType)
		{
			var tagItems = new List<TagItem>();

			switch (tagItemType)
			{
				case TagItemScopeType.RelatedItem:
					tagItems = TagRepository.GetTagItemsByRelatedItemGuid(guid);
					break;
				case TagItemScopeType.Extra:
					tagItems = TagRepository.GetTagItemsByExtraGuid(guid);
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
		public bool Selected { get; set; } = false;
	}


	public enum TagScopeType
	{
		Site,
		Module,
		Feature
	}


	public enum TagItemScopeType
	{
		RelatedItem,
		Extra
	}
}