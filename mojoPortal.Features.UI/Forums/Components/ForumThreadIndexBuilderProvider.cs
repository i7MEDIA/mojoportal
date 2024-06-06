using System;
using System.Data;
using System.Text;
using System.Threading;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web.ForumUI;

namespace mojoPortal.Features;

public class ForumThreadIndexBuilderProvider : IndexBuilderProvider
{
	private static readonly ILog log = LogManager.GetLogger(typeof(ForumThreadIndexBuilderProvider));

	public ForumThreadIndexBuilderProvider()
	{ }

	public override void RebuildIndex(
		PageSettings pageSettings,
		string indexPath)
	{
		if (WebConfigSettings.DisableSearchIndex) { return; }

		if ((pageSettings == null) || (indexPath == null))
		{
			log.Error("pageSettings object or index path passed to ForumThreadIndexBuilderProvider.RebuildIndex was null");

			return;
		}

		//don't index pending/unpublished pages
		if (pageSettings.IsPending)
		{
			return;
		}

		log.Info($"{Resources.ForumResources.ForumsFeatureName} indexing page - {pageSettings.PageName}");

		try
		{
			var pageModules = PageModule.GetPageModulesByPage(pageSettings.PageId);

			var forumFeatureGuid = new Guid("38aa5a84-9f5c-42eb-8f4c-105983d419fb");
			var forumFeature = new ModuleDefinition(forumFeatureGuid);

			// new implementation 2012-05-22: get threads by page, then for each thread concat the posts into one item for indexing
			// previously were indexing individual posts but this makes multiple results in search results for the same thread

			if (ForumConfiguration.AggregateSearchIndexPerThread)
			{
				DataTable threads = ForumThread.GetThreadsByPage(pageSettings.SiteId, pageSettings.PageId);

				foreach (DataRow row in threads.Rows)
				{
					var threadContent = new StringBuilder();

					int threadId = Convert.ToInt32(row["ThreadID"]);
					DateTime threadDate = Convert.ToDateTime(row["ThreadDate"]);

					DataTable threadPosts = ForumThread.GetPostsByThread(threadId);

					foreach (DataRow r in threadPosts.Rows)
					{
						threadContent.Append(r["Post"].ToString());
					}

					var indexItem = new IndexItem
					{
						CreatedUtc = threadDate,
						SiteId = pageSettings.SiteId,
						PageId = pageSettings.PageId,
						PageName = pageSettings.PageName,
						ViewRoles = pageSettings.AuthorizedRoles,
						ModuleViewRoles = row["ViewRoles"].ToString(),
						FeatureId = forumFeatureGuid.ToString(),
						FeatureName = forumFeature.FeatureName,
						FeatureResourceFile = forumFeature.ResourceFile,
						ItemId = Convert.ToInt32(row["ItemID"]),
						ModuleId = Convert.ToInt32(row["ModuleID"]),
						ModuleTitle = row["ModuleTitle"].ToString(),
						Title = row["Subject"].ToString(),
						Content = threadContent.ToString()
					};

					// lookup publish dates
					foreach (PageModule pageModule in pageModules)
					{
						if (indexItem.ModuleId == pageModule.ModuleId)
						{
							indexItem.PublishBeginDate = pageModule.PublishBeginDate;
							indexItem.PublishEndDate = pageModule.PublishEndDate;
						}
					}

					if (row["MostRecentPostDate"] != DBNull.Value)
					{
						indexItem.LastModUtc = Convert.ToDateTime(row["MostRecentPostDate"]);
					}
					else
					{
						indexItem.LastModUtc = threadDate;
					}

					if (ForumConfiguration.CombineUrlParams)
					{
						indexItem.ViewPage = Invariant($"Forums/Thread.aspx?pageid={pageSettings.PageId}&t={row["ThreadID"]}~1");
						indexItem.UseQueryStringParams = false;
						// still need this since it is aprt of the key
						indexItem.QueryStringAddendum = Invariant($"&thread={row["ThreadID"]}");
					}
					else
					{

						indexItem.ViewPage = "Forums/Thread.aspx";
						indexItem.QueryStringAddendum = Invariant($"&thread={row["ThreadID"]}");
					}

					IndexHelper.RebuildIndex(indexItem, indexPath);
					log.Debug($"Indexed {indexItem.Title}");
				}
			}
			else
			{
				//older implementation indexed posts individually

				DataTable dataTable = ForumThread.GetPostsByPage(pageSettings.SiteId, pageSettings.PageId);

				foreach (DataRow row in dataTable.Rows)
				{
					var indexItem = new IndexItem
					{
						SiteId = pageSettings.SiteId,
						PageId = pageSettings.PageId,
						PageName = pageSettings.PageName,
						ViewRoles = pageSettings.AuthorizedRoles,
						ModuleViewRoles = row["ViewRoles"].ToString(),
						FeatureId = forumFeatureGuid.ToString(),
						FeatureName = forumFeature.FeatureName,
						FeatureResourceFile = forumFeature.ResourceFile,
						ItemId = Convert.ToInt32(row["ItemID"]),
						ModuleId = Convert.ToInt32(row["ModuleID"]),
						ModuleTitle = row["ModuleTitle"].ToString(),
						Title = row["Subject"].ToString(),
						Content = row["Post"].ToString(),
						ViewPage = "Forums/Thread.aspx",
						QueryStringAddendum = $"&thread={row["ThreadID"]}&postid={row["PostID"]}"
					};

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

					log.Debug($"Indexed {indexItem.Title}");
				}
			}
		}
		catch (System.Data.Common.DbException ex)
		{
			log.Error(ex);
		}
	}

	public override void ContentChangedHandler(object sender, ContentChangedEventArgs e)
	{
		if (WebConfigSettings.DisableSearchIndex || sender is null || sender is not ForumThread)
		{
			return;
		}

		var forumThread = (ForumThread)sender;
		var siteSettings = CacheHelper.GetCurrentSiteSettings();
		forumThread.SiteId = siteSettings.SiteId;
		forumThread.SearchIndexPath = IndexHelper.GetSearchIndexPath(siteSettings.SiteId);

		if (e.IsDeleted)
		{
			if (ThreadPool.QueueUserWorkItem(new WaitCallback(RemoveForumIndexItem), forumThread))
			{
				log.Debug("ForumThreadIndexBuilderProvider.RemoveForumIndexItem queued");
			}
			else
			{
				log.Error("Failed to queue a thread for ForumThreadIndexBuilderProvider.RemoveForumIndexItem");
			}
		}
		else
		{
			if (ThreadPool.QueueUserWorkItem(new WaitCallback(IndexItem), forumThread))
			{
				log.Debug("ForumThreadIndexBuilderProvider.IndexItem queued");
			}
			else
			{
				log.Error("Failed to queue a thread for ForumThreadIndexBuilderProvider.IndexItem");
			}
		}
	}

	private static void IndexItem(object oForumThread)
	{
		if (WebConfigSettings.DisableSearchIndex || oForumThread is null || oForumThread is not ForumThread)
		{
			return;
		}

		var forumThread = oForumThread as ForumThread;
		IndexItem(forumThread);
	}

	private static void IndexItem(ForumThread forumThread)
	{
		if (WebConfigSettings.DisableSearchIndex)
		{
			return;
		}

		if (forumThread is null)
		{
			if (log.IsErrorEnabled)
			{
				log.Error("forumThread object passed in ForumThreadIndexBuilderProvider.IndexItem was null");
			}
			return;
		}

		var forum = new Forum(forumThread.ForumId);
		var module = new Module(forum.ModuleId);
		var forumFeatureGuid = new Guid("38aa5a84-9f5c-42eb-8f4c-105983d419fb");
		var forumFeature = new ModuleDefinition(forumFeatureGuid);

		// get list of pages where this module is published
		var pageModules = PageModule.GetPageModulesByModule(forum.ModuleId);

		// must update index for all pages containing
		// this module
		foreach (PageModule pageModule in pageModules)
		{
			var pageSettings = new PageSettings(forumThread.SiteId, pageModule.PageId);

			//don't index pending/unpublished pages
			if (pageSettings.IsPending)
			{
				continue;
			}

			// permissions are kept in sync in search index
			// so that results are filtered by role correctly
			var indexItem = new IndexItem
			{
				SiteId = forumThread.SiteId,
				PageId = pageModule.PageId,
				PageName = pageSettings.PageName,
				ViewRoles = pageSettings.AuthorizedRoles,
				ModuleViewRoles = module.ViewRoles,
				ItemId = forumThread.ForumId,
				ModuleId = forum.ModuleId,
				ModuleTitle = module.ModuleTitle,
				FeatureId = forumFeatureGuid.ToString(),
				FeatureName = forumFeature.FeatureName,
				FeatureResourceFile = forumFeature.ResourceFile,
				Title = forumThread.Subject,
				PublishBeginDate = pageModule.PublishBeginDate,
				PublishEndDate = pageModule.PublishEndDate,
				ViewPage = "Forums/Thread.aspx",
				CreatedUtc = forumThread.ThreadDate,
				LastModUtc = forumThread.MostRecentPostDate
			};

			if (forumThread.SearchIndexPath.Length > 0)
			{
				indexItem.IndexPath = forumThread.SearchIndexPath;
			}
			if (ForumConfiguration.AggregateSearchIndexPerThread)
			{
				indexItem.PublishBeginDate = forumThread.MostRecentPostDate;
				var threadContent = new StringBuilder();

				DataTable threadPosts = ForumThread.GetPostsByThread(forumThread.ThreadId);

				foreach (DataRow r in threadPosts.Rows)
				{
					threadContent.Append(r["Post"].ToString());
				}

				//aggregate contents of posts into one indexable content item
				indexItem.Content = threadContent.ToString();

				if (ForumConfiguration.CombineUrlParams)
				{
					indexItem.ViewPage = Invariant($"{"Forums/Thread.aspx".ToLinkBuilder(false).PageId(pageModule.PageId).AddParam("t", forumThread.ThreadId)}~1");
					//$"Forums/Thread.aspx?pageid={pageModule.PageId.ToInvariantString()}&t={forumThread.ThreadId.ToInvariantString()}~1";
					indexItem.UseQueryStringParams = false;
				}

				indexItem.QueryStringAddendum = Invariant($"&thread={forumThread.ThreadId}");
			}
			else
			{
				//older implementation
				indexItem.Content = forumThread.PostMessage;
				indexItem.QueryStringAddendum = Invariant($"&thread={forumThread.ThreadId}&postid={forumThread.PostId}");
			}

			IndexHelper.RebuildIndex(indexItem);

			log.Debug($"Indexed {forumThread.Subject}");
		}
	}

	public void ThreadMovedHandler(object sender, ForumThreadMovedArgs e)
	{
		if (WebConfigSettings.DisableSearchIndex)
		{
			return;
		}

		var forumThread = (ForumThread)sender;
		var postIDList = forumThread.GetPostIdList();
		var origForum = new Forum(e.OriginalForumId);

		foreach (DataRow row in postIDList.Rows)
		{
			int postID = Convert.ToInt32(row["PostID"]);
			var post = new ForumThread(forumThread.ThreadId, postID);

			RemoveForumIndexItem(
				origForum.ModuleId,
				e.OriginalForumId,
				forumThread.ThreadId,
				postID);

			IndexItem(post);
		}
	}


	public static void RemoveForumIndexItem(object oForumThread)
	{
		if (WebConfigSettings.DisableSearchIndex || oForumThread is not ForumThread)
		{
			return;
		}

		var forumThread = oForumThread as ForumThread;

		// get list of pages where this module is published
		var pageModules = PageModule.GetPageModulesByModule(forumThread.ModuleId);

		// must update index for all pages containing
		// this module
		foreach (var pageModule in pageModules)
		{
			var indexItem = new IndexItem
			{
				// note we are just assigning the properties 
				// needed to derive the key so it can be found and
				// deleted from the index
				SiteId = forumThread.SiteId,
				PageId = pageModule.PageId,
				ModuleId = forumThread.ModuleId,
				ItemId = forumThread.ForumId
			};

			if (ForumConfiguration.AggregateSearchIndexPerThread)
			{
				indexItem.QueryStringAddendum = Invariant($"&thread={forumThread.ThreadId}");
			}
			else
			{
				indexItem.QueryStringAddendum = Invariant($"&thread={forumThread.ThreadId}&postid={forumThread.PostId}");
			}

			IndexHelper.RemoveIndex(indexItem);
		}

		log.Debug("Removed Index ");
	}


	public static void RemoveForumIndexItem(int moduleId, int itemId, int threadId, int postId)
	{
		if (WebConfigSettings.DisableSearchIndex)
		{
			return;
		}

		if (CacheHelper.GetCurrentSiteSettings() is not SiteSettings siteSettings)
		{
			log.Error("siteSettings object retrieved in ForumThreadIndexBuilderProvider.RemoveForumIndexItem was null");
			return;
		}

		// get list of pages where this module is published
		var pageModules = PageModule.GetPageModulesByModule(moduleId);

		// must update index for all pages containing
		// this module
		foreach (var pageModule in pageModules)
		{
			var indexItem = new IndexItem
			{
				// note we are just assigning the properties 
				// needed to derive the key so it can be found and
				// deleted from the index
				SiteId = siteSettings.SiteId,
				PageId = pageModule.PageId,
				ModuleId = moduleId,
				ItemId = itemId
			};

			if ((ForumConfiguration.AggregateSearchIndexPerThread) || (postId == -1))
			{
				indexItem.QueryStringAddendum = Invariant($"&thread={threadId}");
			}
			else
			{
				indexItem.QueryStringAddendum = Invariant($"&thread={threadId}&postid={postId}");
			}

			IndexHelper.RemoveIndex(indexItem);
		}

		log.Debug("Removed Index ");
	}
}