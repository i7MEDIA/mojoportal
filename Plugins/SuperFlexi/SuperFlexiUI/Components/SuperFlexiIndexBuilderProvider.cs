using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web;
using SuperFlexiBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace SuperFlexiUI;

public class SuperFlexiIndexBuilderProvider : IndexBuilderProvider
{
	private static readonly ILog log = LogManager.GetLogger(typeof(SuperFlexiIndexBuilderProvider));
	private static readonly bool debugLog = log.IsDebugEnabled;


	public SuperFlexiIndexBuilderProvider() { }


	public override void RebuildIndex(PageSettings pageSettings, string indexPath)
	{
		if (WebConfigSettings.DisableSearchIndex) { return; }

		if (pageSettings == null)
		{
			log.Error("RebuildIndex error: pageSettings was null ");
			return;
		}

		//don't index pending/unpublished pages
		if (pageSettings.IsPending) { return; }

		log.InfoFormat(Resources.SuperFlexiResources.FeatureName + " indexing page [{0}]", pageSettings.PageName);

		try
		{
			DataTable dataTable = Item.GetByCMSPage(pageSettings.SiteGuid, pageSettings.PageId);

			foreach (DataRow row in dataTable.Rows)
			{
				Item item = new Item(Convert.ToInt32(row["ItemID"]));
				if (item == null) continue;
				log.DebugFormat("RebuildIndex indexing content [{0}]", row["ModuleTitle"]);
				IndexItem indexItem = GetIndexItem(pageSettings, Convert.ToInt32(row["ModuleID"]), item);
				if (indexItem == null)
				{
					log.DebugFormat("RebuildIndex IndexItem was NULL for content [{0}]", row["ModuleTitle"]);

					continue;
				}
				indexItem.ModuleViewRoles = row["ModuleViewRoles"].ToString() + row["ItemViewRoles"].ToString();
				indexItem.ModuleId = Convert.ToInt32(row["ModuleID"], CultureInfo.InvariantCulture);
				indexItem.ModuleTitle = row["ModuleTitle"].ToString();

				if (row["PublishBeginDate"] != DBNull.Value)
				{
					indexItem.PublishBeginDate = Convert.ToDateTime(row["PublishBeginDate"]);
				}

				if (row["PublishEndDate"] != DBNull.Value)
				{
					indexItem.PublishEndDate = Convert.ToDateTime(row["PublishEndDate"]);
				}

				IndexHelper.RebuildIndex(indexItem, indexPath);

				log.Debug($"RebuildIndex [{indexItem.PageName}\\{indexItem.ModuleTitle}\\{indexItem.Title}]");
			}
		}
		catch (System.Data.Common.DbException ex)
		{
			log.Error(ex);
		}
	}

	private void IndexItem(Item item)
	{
		if (WebConfigSettings.DisableSearchIndex) { return; }

		SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
		if (siteSettings == null)
		{
			if (log.IsErrorEnabled)
			{
				log.Error("SuperFlexi object passed to SuperFlexi. SiteSettings was null");
			}

			return;
		}

		if (item == null)
		{
			if (log.IsErrorEnabled)
			{
				log.Error("SuperFlexi object passed to SuperFlexi. Item was null");
			}

			return;
		}

		Module module = new Module(item.ModuleGuid);

		// get list of pages where this module is published
		List<PageModule> pageModules = PageModule.GetPageModulesByModule(module.ModuleId);

		foreach (PageModule pageModule in pageModules)
		{
			PageSettings pageSettings = new PageSettings(siteSettings.SiteId, pageModule.PageId);
			//don't index pending/unpublished pages
			if (pageSettings.IsPending) { continue; }

			log.DebugFormat("RebuildIndex indexing content [{0}]", module.ModuleTitle);
			IndexItem indexItem = GetIndexItem(pageSettings, module.ModuleId, item);
			if (indexItem == null) continue;
			indexItem.ModuleViewRoles = module.ViewRoles;
			indexItem.ModuleId = module.ModuleId;
			indexItem.PublishBeginDate = pageModule.PublishBeginDate;
			indexItem.PublishEndDate = pageModule.PublishEndDate;

			IndexHelper.RebuildIndex(indexItem);

			if (debugLog) log.Debug("Indexed SuperFlexi Item " + indexItem.Title);
		}
	}

	public override void ContentChangedHandler(object sender, ContentChangedEventArgs e)
	{
		if (WebConfigSettings.DisableSearchIndex) { return; }

		Item item = (Item)sender;
		if (e.IsDeleted)
		{
			// get list of pages where this module is published
			Module module = new Module(item.ModuleGuid);
			List<PageModule> pageModules = PageModule.GetPageModulesByModule(module.ModuleId);

			foreach (PageModule pageModule in pageModules)
			{
				IndexHelper.RemoveIndexItem(
					pageModule.PageId,
					module.ModuleId,
					item.ItemID);
			}
		}
		else
		{
			IndexItem(item);
		}
	}

	private IndexItem GetIndexItem(PageSettings pageSettings, int moduleID, Item item)
	{
		var module = new Module(moduleID);

		log.Debug($"moduleid: {moduleID} for module {module.ModuleTitle}");

		var config = new ModuleConfiguration(module);

		if (!config.IncludeInSearch)
		{
			return null;
		}

		var featureName = config.SearchFriendlyName.Coalesce(config.ModuleFriendlyName.Coalesce(module.ModuleTitle));

		if (config.RelatedSearchPage > -1)
		{
			if (new PageSettings(pageSettings.SiteId, config.RelatedSearchPage) is PageSettings pageToUse)
			{
				pageSettings = pageToUse; 
			}
		}

        var displaySettings = new SuperFlexiDisplaySettings();
		var flexiFeature = new ModuleDefinition(config.FeatureGuid);

		var indexItem = new IndexItem
		{
			SiteId = pageSettings.SiteId,
			PageId = pageSettings.PageId,
			PageName = pageSettings.PageName,
			ViewRoles = pageSettings.AuthorizedRoles,
			ModuleViewRoles = module.ViewRoles,
			FeatureId = flexiFeature.FeatureGuid.ToString(),
			FeatureName = featureName,
			FeatureResourceFile = flexiFeature.ResourceFile,
			ItemId = item.ItemID,
			CreatedUtc = item.CreatedUtc,
			LastModUtc = item.LastModUtc
		};

		if (pageSettings.UseUrl)
		{
			if (pageSettings.UrlHasBeenAdjustedForFolderSites)
			{
				indexItem.ViewPage = pageSettings.UnmodifiedUrl.Replace("~/", string.Empty);
			}
			else
			{
				indexItem.ViewPage = pageSettings.Url.Replace("~/", string.Empty);
			}
			indexItem.UseQueryStringParams = false;
		}

		var searchDef = SearchDef.GetByFieldDefinition(item.DefinitionGuid);

		var hasSearchDef = true;
		if (searchDef == null)
		{
			searchDef = new SearchDef();
			hasSearchDef = false;
		}
		var sbTitle = new System.Text.StringBuilder(searchDef.Title);
		var sbKeywords = new System.Text.StringBuilder(searchDef.Keywords);
		var sbDescription = new System.Text.StringBuilder(searchDef.Description);
		var sbLink = new System.Text.StringBuilder(searchDef.Link);
		var sbLinkQueryAddendum = new System.Text.StringBuilder(searchDef.LinkQueryAddendum);
		SiteSettings siteSettings = new SiteSettings(pageSettings.SiteGuid);
		SuperFlexiHelpers.ReplaceStaticTokens(sbTitle, config, false, displaySettings, module, pageSettings, siteSettings, out sbTitle);
		SuperFlexiHelpers.ReplaceStaticTokens(sbKeywords, config, false, displaySettings, module, pageSettings, siteSettings, out sbKeywords);
		SuperFlexiHelpers.ReplaceStaticTokens(sbDescription, config, false, displaySettings, module, pageSettings, siteSettings, out sbDescription);
		SuperFlexiHelpers.ReplaceStaticTokens(sbLink, config, false, displaySettings, module, pageSettings, siteSettings, out sbLink);
		SuperFlexiHelpers.ReplaceStaticTokens(sbLinkQueryAddendum, config, false, displaySettings, module, pageSettings, siteSettings, out sbLinkQueryAddendum);

		var fieldValues = ItemFieldValue.GetItemValues(item.ItemGuid);
		log.Debug($"SuperFlexi Index: total field value count for ItemGuid ({item.ItemGuid}) is {fieldValues.Count}");
		foreach (ItemFieldValue fieldValue in fieldValues)
		{
			Field field = new Field(fieldValue.FieldGuid);
			if (field == null || !field.Searchable) continue;

			if (hasSearchDef)
			{
				sbTitle.Replace(field.Token, fieldValue.FieldValue);
				sbKeywords.Replace(field.Token, fieldValue.FieldValue);
				sbDescription.Replace(field.Token, fieldValue.FieldValue);
				sbLink.Replace(field.Token, fieldValue.FieldValue);
				sbLinkQueryAddendum.Replace(field.Token, fieldValue.FieldValue);
			}
			else
			{
				sbKeywords.Append(fieldValue.FieldValue);
			}

			if (debugLog) log.DebugFormat("RebuildIndex indexing item [{0} = {1}]", field.Name, fieldValue.FieldValue);
		}

		if (hasSearchDef)
		{
			sbTitle.Replace("$_ItemID_$", item.ItemID.ToString());
			sbKeywords.Replace("$_ItemID_$", item.ItemID.ToString());
			sbDescription.Replace("$_ItemID_$", item.ItemID.ToString());
			sbLink.Replace("$_ItemID_$", item.ItemID.ToString());
			sbLinkQueryAddendum.Replace("$_ItemID_$", item.ItemID.ToString());

			indexItem.Content = sbDescription.ToString();
			indexItem.Title = sbTitle.ToString();

			if (sbLink.Length > 0)
			{
				indexItem.ViewPage = sbLink.ToString();
			}

			if (sbLinkQueryAddendum.Length > 0)
			{
				indexItem.QueryStringAddendum = sbLinkQueryAddendum.ToString();
				indexItem.UseQueryStringParams = false;
			}

		}
		else
		{
			indexItem.ModuleTitle = pageSettings.PageName;
			indexItem.Title = String.IsNullOrWhiteSpace(config.ModuleFriendlyName) ? module.ModuleTitle : config.ModuleFriendlyName;
		}

		indexItem.PageMetaKeywords = sbKeywords.ToString();
		indexItem.Categories = sbKeywords.ToString();

		return indexItem;

	}
}
