// Author:					    
// Created:				        2007-08-30
// Last Modified:			    2014-12-05
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Globalization;
using System.Threading;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using Resources;

namespace mojoPortal.SearchIndex
{
	/// <summary>
	///
	/// </summary>
	public class HtmlContentIndexBuilderProvider : IndexBuilderProvider
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(HtmlContentIndexBuilderProvider));
		private static bool debugLog = log.IsDebugEnabled;

		public HtmlContentIndexBuilderProvider()
		{ }

		public override void RebuildIndex(PageSettings pageSettings, string indexPath)
		{
			bool disableSearchIndex = ConfigHelper.GetBoolProperty("DisableSearchIndex", false);
			if (disableSearchIndex)
			{
				return;
			}

			if (pageSettings == null)
			{
				log.Error("pageSettings passed in to HtmlContentIndexBuilderProvider.RebuildIndex was null");
				return;
			}

			//don't index pending/unpublished pages
			if (pageSettings.IsPending)
			{
				return;
			}

			try
			{
				var htmlFeatureGuid = new Guid("881e4e00-93e4-444c-b7b0-6672fb55de10");
				var htmlFeature = new ModuleDefinition(htmlFeatureGuid);
				var pageModules = PageModule.GetPageModulesByPage(pageSettings.PageId);
				var repository = new HtmlRepository();
				var dataTable = repository.GetHtmlContentByPage(pageSettings.SiteId, pageSettings.PageId);

				if (dataTable.Rows.Count > 0)
				{
					log.Info($"{Resource.HtmlContentFeatureName} indexing page - {pageSettings.PageName}");
				}

				foreach (DataRow row in dataTable.Rows)
				{
					var includeInSearch = Convert.ToBoolean(row["IncludeInSearch"]);
					var excludeFromRecentContent = Convert.ToBoolean(row["ExcludeFromRecentContent"]);

					var indexItem = new IndexItem
					{
						ExcludeFromRecentContent = excludeFromRecentContent,
						SiteId = pageSettings.SiteId,
						PageId = pageSettings.PageId,
						PageName = pageSettings.PageName
					};

					var authorName = row["CreatedByName"].ToString();
					var authorFirstName = row["CreatedByFirstName"].ToString();
					var authorLastName = row["CreatedByLastName"].ToString();

					if (authorFirstName.Length > 0 && authorLastName.Length > 0)
					{
						indexItem.Author = string.Format(CultureInfo.InvariantCulture,
							Resource.FirstNameLastNameFormat, authorFirstName, authorLastName);
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

					indexItem.ViewRoles = pageSettings.AuthorizedRoles;
					indexItem.ModuleViewRoles = row["ViewRoles"].ToString();
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
					indexItem.FeatureId = htmlFeatureGuid.ToString();
					indexItem.FeatureName = htmlFeature.FeatureName;
					indexItem.FeatureResourceFile = htmlFeature.ResourceFile;

					indexItem.ItemId = Convert.ToInt32(row["ItemID"]);
					indexItem.ModuleId = Convert.ToInt32(row["ModuleID"]);
					indexItem.ModuleTitle = row["ModuleTitle"].ToString();
					indexItem.Title = row["Title"].ToString();
					// added the remove markup 2010-01-30 because some javascript strings like ]]> were apearing in search results if the content conatined jacvascript
					indexItem.Content = row["Body"].ToString().RemoveMarkup();

					indexItem.CreatedUtc = Convert.ToDateTime(row["CreatedDate"]);
					indexItem.LastModUtc = Convert.ToDateTime(row["LastModUtc"]);

					// lookup publish dates
					foreach (var pageModule in pageModules)
					{
						if (indexItem.ModuleId == pageModule.ModuleId)
						{
							indexItem.PublishBeginDate = pageModule.PublishBeginDate;
							indexItem.PublishEndDate = pageModule.PublishEndDate;
						}
					}

					IndexHelper.RebuildIndex(indexItem, indexPath);
					log.Info($"Indexed {indexItem.ModuleTitle}");
					log.Debug("Indexed " + indexItem.Title);
				}
			}
			catch (System.Data.Common.DbException ex)
			{
				log.Error(ex);
			}
		}

		public override void ContentChangedHandler(object sender, ContentChangedEventArgs e)
		{
			bool disableSearchIndex = ConfigHelper.GetBoolProperty("DisableSearchIndex", false);
			if (disableSearchIndex)
			{
				return;
			}

			if (sender is not HtmlContent)
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

				foreach (PageModule pageModule in pageModules)
				{
					IndexHelper.RemoveIndexItem(
						pageModule.PageId,
						content.ModuleId,
						content.ItemId);
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

		private static void IndexItem(object o)
		{
			bool disableSearchIndex = ConfigHelper.GetBoolProperty("DisableSearchIndex", false);
			if (disableSearchIndex || o is not HtmlContent)
			{
				return;
			}

			HtmlContent content = o as HtmlContent;
			IndexItem(content);
		}

		private static void IndexItem(HtmlContent content)
		{
			bool disableSearchIndex = ConfigHelper.GetBoolProperty("DisableSearchIndex", false);
			if (disableSearchIndex)
			{
				return;
			}

			var module = new Module(content.ModuleId);

			var htmlFeatureGuid = new Guid("881e4e00-93e4-444c-b7b0-6672fb55de10");
			var htmlFeature = new ModuleDefinition(htmlFeatureGuid);

			// get list of pages where this module is published
			var pageModules
				= PageModule.GetPageModulesByModule(content.ModuleId);

			foreach (var pageModule in pageModules)
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
					FeatureId = htmlFeatureGuid.ToString(),
					FeatureName = htmlFeature.FeatureName,
					FeatureResourceFile = htmlFeature.ResourceFile,
					ItemId = content.ItemId,
					ModuleId = content.ModuleId,
					ModuleTitle = module.ModuleTitle,
					Title = content.Title,
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
				if (ConfigHelper.GetBoolProperty("IndexPageMeta", false))
				{
					indexItem.PageMetaDescription = pageSettings.PageMetaDescription;
					indexItem.PageMetaKeywords = pageSettings.PageMetaKeyWords;
				}

				if ((content.CreatedByFirstName.Length > 0) && (content.CreatedByLastName.Length > 0))
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
}
