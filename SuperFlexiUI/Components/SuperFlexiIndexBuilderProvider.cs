/// Author:					    i7MEDIA
/// Created:				    2015-03-06
/// Last Modified:			    2019-01-24
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web;
using SuperFlexiBusiness;

namespace SuperFlexiUI
{
    public class SuperFlexiIndexBuilderProvider : IndexBuilderProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SuperFlexiIndexBuilderProvider));
        private static bool debugLog = log.IsDebugEnabled;

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
            Module module = new Module(moduleID);
			log.Debug($"moduleid: {moduleID} for module {module.ModuleTitle}");

			ModuleConfiguration config = new ModuleConfiguration(module);
            if (!config.IncludeInSearch) return null;
            SuperFlexiDisplaySettings displaySettings = new SuperFlexiDisplaySettings();
            ModuleDefinition flexiFeature = new ModuleDefinition(config.FeatureGuid);
            IndexItem indexItem = new IndexItem();
            indexItem.SiteId = pageSettings.SiteId;
            indexItem.PageId = pageSettings.PageId;
            indexItem.PageName = pageSettings.PageName;
            indexItem.ViewRoles = pageSettings.AuthorizedRoles;
            indexItem.FeatureId = flexiFeature.FeatureGuid.ToString();
            indexItem.FeatureName = String.IsNullOrWhiteSpace(config.ModuleFriendlyName) ? module.ModuleTitle : config.ModuleFriendlyName;
            indexItem.FeatureResourceFile = flexiFeature.ResourceFile;
            indexItem.ItemId = item.ItemID;
            indexItem.CreatedUtc = item.CreatedUtc;
            indexItem.LastModUtc = item.LastModUtc;
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

            SearchDef searchDef = SearchDef.GetByFieldDefinition(item.DefinitionGuid);

            bool hasSearchDef = true;
            if (searchDef == null)
            {
                searchDef = new SearchDef();
                hasSearchDef = false;
            }
            System.Text.StringBuilder sbTitle = new System.Text.StringBuilder(searchDef.Title);
            System.Text.StringBuilder sbKeywords = new System.Text.StringBuilder(searchDef.Keywords);
            System.Text.StringBuilder sbDescription = new System.Text.StringBuilder(searchDef.Description);
            System.Text.StringBuilder sbLink = new System.Text.StringBuilder(searchDef.Link);
            System.Text.StringBuilder sbLinkQueryAddendum = new System.Text.StringBuilder(searchDef.LinkQueryAddendum);
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
}
