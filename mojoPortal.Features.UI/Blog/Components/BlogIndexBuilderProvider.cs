using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web;
using mojoPortal.Web.BlogUI;
using Resources;

namespace mojoPortal.Features;


public class BlogIndexBuilderProvider : IndexBuilderProvider
{
	public BlogIndexBuilderProvider()
	{ }

	private static readonly ILog log = LogManager.GetLogger(typeof(BlogIndexBuilderProvider));
	private static bool debugLog = log.IsDebugEnabled;

	public override void RebuildIndex(PageSettings pageSettings, string indexPath)
	{
		if (WebConfigSettings.DisableSearchIndex) { return; }

		if (pageSettings == null)
		{
			log.Error("pageSettings object passed to BlogIndexBuilderProvider.RebuildIndex was null");
			return;
		}

		//don't index pending/unpublished pages
		if (pageSettings.IsPending) { return; }

		log.Info($"{BlogResources.BlogFeatureName} indexing page - {pageSettings.PageName}");

		Guid blogFeatureGuid = new("026cbead-2b80-4491-906d-b83e37179ccf");
		ModuleDefinition blogFeature = new(blogFeatureGuid);

		List<PageModule> pageModules = PageModule.GetPageModulesByPage(pageSettings.PageId);

		DataTable dataTable = Blog.GetBlogsByPage(pageSettings.SiteId, pageSettings.PageId);

		foreach (DataRow row in dataTable.Rows)
		{
			bool includeInSearch = Convert.ToBoolean(row["IncludeInSearch"], CultureInfo.InvariantCulture) || !Convert.ToBoolean(row["IsPublished"], CultureInfo.InvariantCulture);
			if (!includeInSearch) { continue; }

			DateTime postEndDate = DateTime.MaxValue;
			if (row["EndDate"] != DBNull.Value)
			{
				postEndDate = Convert.ToDateTime(row["EndDate"]);

				if (postEndDate < DateTime.UtcNow) { continue; }
			}

			bool excludeFromRecentContent = Convert.ToBoolean(row["ExcludeFromRecentContent"], CultureInfo.InvariantCulture);

			int moduleId = Convert.ToInt32(row["ModuleID"], CultureInfo.InvariantCulture);
			int itemId = Convert.ToInt32(row["ItemID"], CultureInfo.InvariantCulture);
			DateTime blogStart = Convert.ToDateTime(row["StartDate"]);

			string authorName = row["Name"].ToString();
			string authorFirstName = row["FirstName"].ToString();
			string authorLastName = row["LastName"].ToString(); 
			if ((authorFirstName.Length > 0) && (authorLastName.Length > 0))
			{
				authorName = string.Format(CultureInfo.InvariantCulture,
					BlogResources.FirstLastFormat, authorFirstName, authorLastName);
			}
			
			string viewPage = row["ItemUrl"].ToString().Replace("~/", string.Empty);
			if ((!WebConfigSettings.UseUrlReWriting) || (!BlogConfiguration.UseFriendlyUrls(moduleId)))
			{
				viewPage = "Blog/ViewPost.aspx".ToLinkBuilder(false).SiteId(-1).PageId(pageSettings.PageId).ModuleId(moduleId).ItemId(itemId).ToString();
				//viewPage = $"Blog/ViewPost.aspx?pageid={pageSettings.PageId.ToInvariantString()}&mid={moduleId.ToInvariantString()}&ItemID={itemId.ToInvariantString()}";
			}

			var indexItem = new IndexItem
			{
				SiteId = pageSettings.SiteId,
				PageId = pageSettings.PageId,
				ExcludeFromRecentContent = excludeFromRecentContent,
				PageName = pageSettings.PageName,
				ViewRoles = pageSettings.AuthorizedRoles,
				ModuleViewRoles = row["ViewRoles"].ToString(),
				FeatureId = blogFeatureGuid.ToString(),
				FeatureName = blogFeature.FeatureName,
				FeatureResourceFile = blogFeature.ResourceFile,
				ItemId = itemId,
				ModuleId = moduleId,
				ModuleTitle = row["ModuleTitle"].ToString(),
				Title = row["Heading"].ToString(),
				Author = authorName,
				ViewPage = viewPage,
				PageMetaDescription = row["MetaDescription"].ToString(),
				PageMetaKeywords = row["MetaKeywords"].ToString(),
				CreatedUtc = blogStart,
				LastModUtc = Convert.ToDateTime(row["LastModUtc"]),
				UseQueryStringParams = false,
				Content = row["Description"].ToString(),
				ContentAbstract = row["Abstract"].ToString()
			};

			int commentCount = Convert.ToInt32(row["CommentCount"]);
			if (commentCount > 0)
			{   // index comments
				StringBuilder stringBuilder = new StringBuilder();
				DataTable comments = Blog.GetBlogCommentsTable(indexItem.ModuleId, indexItem.ItemId);

				foreach (DataRow commentRow in comments.Rows)
				{
					stringBuilder.Append($"  {commentRow["Comment"]}");
					stringBuilder.Append($"  {commentRow["Name"]}");

					if (debugLog) log.Debug("BlogIndexBuilderProvider.RebuildIndex add comment ");

				}

				indexItem.OtherContent = stringBuilder.ToString();

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

			if (blogStart > indexItem.PublishBeginDate) { indexItem.PublishBeginDate = blogStart; }
			if (postEndDate < indexItem.PublishEndDate) { indexItem.PublishEndDate = postEndDate; }



			IndexHelper.RebuildIndex(indexItem, indexPath);

			if (debugLog) log.Debug("Indexed " + indexItem.Title);

		}
	}


	public override void ContentChangedHandler(object sender, ContentChangedEventArgs e)
	{
		if (WebConfigSettings.DisableSearchIndex) { return; }
		if (sender is null) return;
		if (sender is not Blog) return;

		Blog blog = (Blog)sender;
		SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
		blog.SiteId = siteSettings.SiteId;
		blog.SearchIndexPath = IndexHelper.GetSearchIndexPath(siteSettings.SiteId);


		if (e.IsDeleted || !blog.IncludeInSearch || blog.EndDate < DateTime.UtcNow)
		{
			RemoveIndexedBlogPost(blog);
		}
		else
		{
			if (ThreadPool.QueueUserWorkItem(new WaitCallback(IndexItem), blog))
			{
				if (debugLog) log.Debug("BlogIndexBuilderProvider.IndexItem queued");
			}
			else
			{
				log.Error("Failed to queue a thread for BlogIndexBuilderProvider.IndexItem");
			}
		}
	}

	private void RemoveIndexedBlogPost(Blog blog)
	{
		// get list of pages where this module is published
		List<PageModule> pageModules = PageModule.GetPageModulesByModule(blog.ModuleId);

		foreach (PageModule pageModule in pageModules)
		{
			IndexHelper.RemoveIndexItem(
				pageModule.PageId,
				blog.ModuleId,
				blog.ItemId);
		}
	}

	private static void IndexItem(object o)
	{
		if (WebConfigSettings.DisableSearchIndex) { return; }
		if (o is null) return;
		if (o is not Blog) return;

		Blog content = o as Blog;
		IndexItem(content);
	}

	private static void IndexItem(Blog blog)
	{

		if (WebConfigSettings.DisableSearchIndex) { return; }

		if (blog is null)
		{
			if (log.IsErrorEnabled)
			{
				log.Error("blog object passed to BlogIndexBuilderProvider.IndexItem was null");
			}
			return;
		}

		if (!blog.IncludeInSearch) { return; }

		var module = new Module(blog.ModuleId);
		var blogFeatureGuid = new Guid("026cbead-2b80-4491-906d-b83e37179ccf");
		var blogFeature = new ModuleDefinition(blogFeatureGuid);

		// get comments so  they can be indexed too
		var stringBuilder = new StringBuilder();
		using (IDataReader comments = Blog.GetBlogComments(blog.ModuleId, blog.ItemId))
		{
			while (comments.Read())
			{
				stringBuilder.Append($"  {comments["Comment"]}");
				stringBuilder.Append($"  {comments["Name"]}");

				if (debugLog) log.Debug("BlogIndexBuilderProvider.IndexItem add comment ");
			}
		}

		// get list of pages where this module is published
		var pageModules = PageModule.GetPageModulesByModule(blog.ModuleId);

		foreach (PageModule pageModule in pageModules)
		{
			var pageSettings = new PageSettings(blog.SiteId, pageModule.PageId);

			//don't index pending/unpublished pages
			if (pageSettings.IsPending) { continue; }

			var indexItem = new IndexItem
			{
				SiteId = blog.SiteId,
				ExcludeFromRecentContent = blog.ExcludeFromRecentContent,
				PageId = pageSettings.PageId,
				PageName = pageSettings.PageName,
				ViewRoles = pageSettings.AuthorizedRoles,
				ModuleViewRoles = module.ViewRoles,
				PageMetaDescription = blog.MetaDescription,
				PageMetaKeywords = blog.MetaKeywords,
				ItemId = blog.ItemId,
				ModuleId = blog.ModuleId,
				ModuleTitle = module.ModuleTitle,
				Title = blog.Title,
				Content = $"{blog.Description} {blog.MetaDescription} {blog.MetaKeywords}",
				ContentAbstract = blog.Excerpt,
				FeatureId = blogFeatureGuid.ToString(),
				FeatureName = blogFeature.FeatureName,
				FeatureResourceFile = blogFeature.ResourceFile,
				OtherContent = stringBuilder.ToString(),
				PublishBeginDate = pageModule.PublishBeginDate
			};

			if (blog.SearchIndexPath.Length > 0)
			{
				indexItem.IndexPath = blog.SearchIndexPath;
			}
			if (blog.StartDate > pageModule.PublishBeginDate) { indexItem.PublishBeginDate = blog.StartDate; }

			indexItem.PublishEndDate = pageModule.PublishEndDate;
			if (blog.EndDate < pageModule.PublishEndDate) { indexItem.PublishEndDate = blog.EndDate; }

			if ((blog.UserFirstName.Length > 0) && (blog.UserLastName.Length > 0))
			{
				indexItem.Author = string.Format(CultureInfo.InvariantCulture,
					BlogResources.FirstLastFormat, blog.UserFirstName, blog.UserLastName);
			}
			else
			{
				indexItem.Author = blog.UserName;
			}

			indexItem.CreatedUtc = blog.StartDate;
			indexItem.LastModUtc = blog.LastModUtc;

			if ((!WebConfigSettings.UseUrlReWriting) || (!BlogConfiguration.UseFriendlyUrls(indexItem.ModuleId)))
			{
				indexItem.ViewPage = "Blog/ViewPost.aspx".ToLinkBuilder(false).SiteId(-1).PageId(pageSettings.PageId).ModuleId(indexItem.ModuleId).ItemId(indexItem.ItemId).ToString();
				//indexItem.ViewPage = $"Blog/ViewPost.aspx?pageid={indexItem.PageId.ToInvariantString()}&mid={indexItem.ModuleId.ToInvariantString()}&ItemID={indexItem.ItemId.ToInvariantString()}";
			}
			else
			{
				indexItem.ViewPage = blog.ItemUrl.Replace("~/", string.Empty);
			}

			indexItem.UseQueryStringParams = false;



			IndexHelper.RebuildIndex(indexItem);
		}

		if (debugLog)
		{
			log.Debug("Indexed " + blog.Title);
		}
	}
}