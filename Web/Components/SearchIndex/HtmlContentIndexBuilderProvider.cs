using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using Resources;

namespace mojoPortal.SearchIndex;

public class HtmlContentIndexBuilderProvider : IndexBuilderProvider
{
	private static readonly ILog log = LogManager.GetLogger(typeof(HtmlContentIndexBuilderProvider));
	private static bool debugLog = log.IsDebugEnabled;
	private static bool disableSearchIndex = ConfigHelper.GetBoolProperty("DisableSearchIndex", false);

	public HtmlContentIndexBuilderProvider()
	{ }

	public override void RebuildIndex(PageSettings pageSettings, string indexPath)
	{
		if (disableSearchIndex)
		{
			return;
		}

		if (pageSettings is null)
		{
			log.Error("pageSettings passed in to HtmlContentIndexBuilderProvider.RebuildIndex was null");
			return;
		}

		//don't index pending/unpublished pages
		if (pageSettings.IsPending)
		{
			return;
		}

		var pageModules = PageModule.GetPageModules(pageSettings.PageId, HtmlContent.FeatureGuid);
		//only index pages with this feature
		if (pageModules.Count == 0)
		{
			return;
		}

		log.Info($"{Resource.HtmlContentFeatureName} indexing page - {pageSettings.PageName}");

		try
		{
			var htmlFeature = new ModuleDefinition(HtmlContent.FeatureGuid);

			var repository = new HtmlRepository();

			var dataTable = repository.GetHtmlContentByPage(pageSettings.SiteId, pageSettings.PageId);

			foreach (DataRow row in dataTable.Rows)
			{
				var includeInSearch = Convert.ToBoolean(row["IncludeInSearch"]);
				var excludeFromRecentContent = Convert.ToBoolean(row["ExcludeFromRecentContent"]);

				var indexItem = new IndexItem
				{
					ExcludeFromRecentContent = excludeFromRecentContent,
					SiteId = pageSettings.SiteId,
					PageId = pageSettings.PageId,
					PageName = pageSettings.PageName,
					FeatureId = HtmlContent.FeatureGuid.ToString(),
					FeatureName = htmlFeature.FeatureName,
					FeatureResourceFile = htmlFeature.ResourceFile,
					ItemId = Convert.ToInt32(row["ItemID"]),
					ModuleId = Convert.ToInt32(row["ModuleID"]),
					ModuleTitle = row["ModuleTitle"].ToString(),
					Title = row["Title"].ToString(),
					Content = row["Body"].ToString().RemoveMarkup(),
					CreatedUtc = Convert.ToDateTime(row["CreatedDate"]),
					LastModUtc = Convert.ToDateTime(row["LastModUtc"]),
					ViewRoles = pageSettings.AuthorizedRoles,
					ModuleViewRoles = row["ViewRoles"].ToString()
				};

				var authorName = row["CreatedByName"].ToString();
				var authorFirstName = row["CreatedByFirstName"].ToString();
				var authorLastName = row["CreatedByLastName"].ToString();

				if (authorFirstName.Length > 0 && authorLastName.Length > 0)
				{
					indexItem.Author = string.Format(
						CultureInfo.InvariantCulture,
						Resource.FirstNameLastNameFormat,
						authorFirstName,
						authorLastName
					);
				}
				else
				{
					indexItem.Author = authorName;
				}

				if (!includeInSearch)
				{
					indexItem.RemoveOnly = true;
				}

				// generally we should not include the page meta because it can result in duplicate results
				// one for each instance of html content on the page because they all use the smae page meta.
				// since page meta should reflect the content of the page it is sufficient to just index the content
				if (WebConfigSettings.IndexPageKeywordsWithHtmlArticleContent)
				{
					indexItem.PageMetaDescription = pageSettings.PageMetaDescription;
					indexItem.PageMetaKeywords = pageSettings.PageMetaKeyWords;
				}

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

				// lookup publish dates
				foreach (PageModule pageModule in pageModules)
				{
					if (indexItem.ModuleId == pageModule.ModuleId)
					{
						indexItem.PublishBeginDate = pageModule.PublishBeginDate;
						indexItem.PublishEndDate = pageModule.PublishEndDate;
					}
				}

				IndexHelper.RebuildIndex(indexItem, indexPath);

				log.Debug($"Indexed {indexItem.Title}");
			}
		}
		catch (System.Data.Common.DbException ex)
		{
			log.Error(ex);
		}
	}

	public override void ContentChangedHandler(object sender, ContentChangedEventArgs e)
	{
		if (!canIndex(sender))
		{
			return;
		}

		var content = (HtmlContent)sender;
		var siteSettings = CacheHelper.GetCurrentSiteSettings();
		content.SiteId = siteSettings.SiteId;
		content.SearchIndexPath = IndexHelper.GetSearchIndexPath(siteSettings.SiteId);

		if (e.IsDeleted)
		{
			// get list of pages where this module is published
			var pageModules = PageModule.GetPageModulesByModule(content.ModuleId);

			foreach (var pageModule in pageModules)
			{
				IndexHelper.RemoveIndexItem(
					pageModule.PageId,
					content.ModuleId,
					content.ItemId
				);
			}
		}
		else
		{
			if (ThreadPool.QueueUserWorkItem(new WaitCallback(IndexItem), content))
			{
				if (debugLog)
				{
					log.Debug("HtmlContentIndexBuilderProvider.IndexItem queued");
				}
			}
			else
			{
				if (log.IsErrorEnabled)
				{
					log.Error("Failed to queue a thread for HtmlContentIndexBuilderProvider.IndexItem");
				}
			}
		}
	}

	private static bool canIndex(object o) => !ConfigHelper.GetBoolProperty("DisableSearchIndex", false) && o is HtmlContent;

	private static void IndexItem(object o)
	{
		if (o is not HtmlContent content)
		{
			return;
		}

		if (!canIndex(content))
		{
			return;
		}

		var module = new Module(content.ModuleId);

		var htmlFeature = new ModuleDefinition(HtmlContent.FeatureGuid);

		// get list of pages where this module is published
		var pageModules = PageModule.GetPageModulesByModule(content.ModuleId);

		foreach (PageModule pageModule in pageModules)
		{
			var pageSettings = new PageSettings(content.SiteId, pageModule.PageId);

			//don't index pending/unpublished pages
			if (pageSettings.IsPending)
			{
				continue;
			}

			var indexItem = new IndexItem
			{
				SiteId = content.SiteId,
				ExcludeFromRecentContent = content.ExcludeFromRecentContent,
				PageId = pageModule.PageId,
				PageName = pageSettings.PageName,
				ViewRoles = pageSettings.AuthorizedRoles,
				ModuleViewRoles = module.ViewRoles,
				FeatureId = HtmlContent.FeatureGuid.ToString(),
				FeatureName = htmlFeature.FeatureName,
				FeatureResourceFile = htmlFeature.ResourceFile,
				ItemId = content.ItemId,
				ModuleId = content.ModuleId,
				ModuleTitle = module.ModuleTitle,
				Title = string.IsNullOrWhiteSpace(content.Title) ? module.ModuleTitle : content.Title,
				Content = content.Body.RemoveMarkup(),
				PublishBeginDate = pageModule.PublishBeginDate,
				PublishEndDate = pageModule.PublishEndDate,
				CreatedUtc = content.CreatedDate,
				LastModUtc = content.LastModUtc
			};

			if (content.SearchIndexPath.Length > 0)
			{
				indexItem.IndexPath = content.SearchIndexPath;
			}

			if (pageSettings.UseUrl)
			{
				indexItem.ViewPage = pageSettings.Url.Replace("~/", string.Empty);
				indexItem.UseQueryStringParams = false;
			}

			// generally we should not include the page meta because it can result in duplicate results
			// one for each instance of html content on the page because they all use the smae page meta.
			// since page meta should reflect the content of the page it is sufficient to just index the content
			if (ConfigurationManager.AppSettings["IndexPageMeta"] != null
				&& ConfigurationManager.AppSettings["IndexPageMeta"] == "true"
				)
			{
				indexItem.PageMetaDescription = pageSettings.PageMetaDescription;
				indexItem.PageMetaKeywords = pageSettings.PageMetaKeyWords;
			}

			if (content.CreatedByFirstName.Length > 0 && content.CreatedByLastName.Length > 0)
			{
				indexItem.Author = string.Format(CultureInfo.InvariantCulture,
					Resource.FirstLastFormat, content.CreatedByFirstName, content.CreatedByLastName);
			}
			else
			{
				indexItem.Author = content.CreatedByName;
			}

			if (!module.IncludeInSearch)
			{
				indexItem.RemoveOnly = true;
			}

			IndexHelper.RebuildIndex(indexItem);
		}

		log.Debug("Indexed " + content.Title);
	}
}
