// Author:					    
// Created:				        2007-08-30
// Last Modified:			    2018-04-24
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

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
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Features
{
    
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

            log.Info(BlogResources.BlogFeatureName + " indexing page - " + pageSettings.PageName);

			//try
			//{
			Guid blogFeatureGuid = new("026cbead-2b80-4491-906d-b83e37179ccf");
			ModuleDefinition blogFeature = new(blogFeatureGuid);

            List<PageModule> pageModules = PageModule.GetPageModulesByPage(pageSettings.PageId);

            DataTable dataTable = Blog.GetBlogsByPage(pageSettings.SiteId, pageSettings.PageId);

            foreach (DataRow row in dataTable.Rows)
            {
                bool includeInSearch = Convert.ToBoolean(row["IncludeInSearch"], CultureInfo.InvariantCulture) || !Convert.ToBoolean(row["IsPublished"], CultureInfo.InvariantCulture);
                if (!includeInSearch) { continue; }

                DateTime postEndDate = DateTime.MaxValue;
                if(row["EndDate"] != DBNull.Value)
                {
                    postEndDate = Convert.ToDateTime(row["EndDate"]);

                    if (postEndDate < DateTime.UtcNow) { continue; }
                }

                bool excludeFromRecentContent = Convert.ToBoolean(row["ExcludeFromRecentContent"], CultureInfo.InvariantCulture);                

                mojoPortal.SearchIndex.IndexItem indexItem = new mojoPortal.SearchIndex.IndexItem();
                indexItem.SiteId = pageSettings.SiteId;
                indexItem.PageId = pageSettings.PageId;
                indexItem.ExcludeFromRecentContent = excludeFromRecentContent;
                indexItem.PageName = pageSettings.PageName;
                indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                indexItem.ModuleViewRoles = row["ViewRoles"].ToString();
                indexItem.FeatureId = blogFeatureGuid.ToString();
                indexItem.FeatureName = blogFeature.FeatureName;
                indexItem.FeatureResourceFile = blogFeature.ResourceFile;

                indexItem.ItemId = Convert.ToInt32(row["ItemID"], CultureInfo.InvariantCulture);
                indexItem.ModuleId = Convert.ToInt32(row["ModuleID"], CultureInfo.InvariantCulture);
                indexItem.ModuleTitle = row["ModuleTitle"].ToString();
                indexItem.Title = row["Heading"].ToString();

                string authorName = row["Name"].ToString();
                string authorFirstName = row["FirstName"].ToString();
                string authorLastName = row["LastName"].ToString();

                if ((authorFirstName.Length > 0) && (authorLastName.Length > 0))
                {
                    indexItem.Author = string.Format(CultureInfo.InvariantCulture,
                        BlogResources.FirstLastFormat, authorFirstName, authorLastName);
                }
                else
                {
                    indexItem.Author = authorName;
                }

                if ((!WebConfigSettings.UseUrlReWriting) || (!BlogConfiguration.UseFriendlyUrls(indexItem.ModuleId)))
                {
                    indexItem.ViewPage = "Blog/ViewPost.aspx?pageid=" 
                        + indexItem.PageId.ToInvariantString()
                        + "&mid=" + indexItem.ModuleId.ToInvariantString()
                        + "&ItemID=" + indexItem.ItemId.ToInvariantString()
                        ;
                }
                else
                {
                    indexItem.ViewPage = row["ItemUrl"].ToString().Replace("~/", string.Empty);
                }

                indexItem.PageMetaDescription = row["MetaDescription"].ToString();
                indexItem.PageMetaKeywords = row["MetaKeywords"].ToString();

                DateTime blogStart = Convert.ToDateTime(row["StartDate"]);

                indexItem.CreatedUtc = blogStart;
                indexItem.LastModUtc = Convert.ToDateTime(row["LastModUtc"]);

                //if (indexItem.ViewPage.Length > 0)
                //{
                    indexItem.UseQueryStringParams = false;
                //}
                //else
                //{
                //    indexItem.ViewPage = "Blog/ViewPost.aspx";
                //}
                indexItem.Content = row["Description"].ToString();
                indexItem.ContentAbstract = row["Abstract"].ToString();
                int commentCount = Convert.ToInt32(row["CommentCount"]);

                if (commentCount > 0)
                {	// index comments
                    StringBuilder stringBuilder = new StringBuilder();
                    DataTable comments = Blog.GetBlogCommentsTable(indexItem.ModuleId, indexItem.ItemId);

                    foreach (DataRow commentRow in comments.Rows)
                    {
                        stringBuilder.Append("  " + commentRow["Comment"].ToString());
                        stringBuilder.Append("  " + commentRow["Name"].ToString());

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



                mojoPortal.SearchIndex.IndexHelper.RebuildIndex(indexItem, indexPath);

                if (debugLog) log.Debug("Indexed " + indexItem.Title);

            }
            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex);
            //}


        }


        public override void ContentChangedHandler(object sender, ContentChangedEventArgs e)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (sender == null) return;
            if (!(sender is Blog)) return;

            Blog blog = (Blog)sender;
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            blog.SiteId = siteSettings.SiteId;
            blog.SearchIndexPath = mojoPortal.SearchIndex.IndexHelper.GetSearchIndexPath(siteSettings.SiteId);


            if (e.IsDeleted || !blog.IncludeInSearch || blog.EndDate < DateTime.UtcNow)
            {
				// get list of pages where this module is published
				//List<PageModule> pageModules
				//    = PageModule.GetPageModulesByModule(blog.ModuleId);

				//foreach (PageModule pageModule in pageModules)
				//{
				//    mojoPortal.SearchIndex.IndexHelper.RemoveIndexItem(
				//        pageModule.PageId,
				//        blog.ModuleId,
				//        blog.ItemId);
				//}

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
                //IndexItem(blog);
            }


        }

		private void RemoveIndexedBlogPost(Blog blog)
		{
			// get list of pages where this module is published
			List<PageModule> pageModules
				= PageModule.GetPageModulesByModule(blog.ModuleId);

			foreach (PageModule pageModule in pageModules)
			{
				mojoPortal.SearchIndex.IndexHelper.RemoveIndexItem(
					pageModule.PageId,
					blog.ModuleId,
					blog.ItemId);
			}
		}

        private static void IndexItem(object o)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (o == null) return;
            if (!(o is Blog)) return;

            Blog content = o as Blog;
            IndexItem(content);

        }


        private static void IndexItem(Blog blog)
        {

            if (WebConfigSettings.DisableSearchIndex) { return; }

            if (blog == null)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("blog object passed to BlogIndexBuilderProvider.IndexItem was null");
                }
                return;
            }

            if (!blog.IncludeInSearch) { return; }

            Module module = new Module(blog.ModuleId);
            Guid blogFeatureGuid = new Guid("026cbead-2b80-4491-906d-b83e37179ccf");
            ModuleDefinition blogFeature = new ModuleDefinition(blogFeatureGuid);

            // get comments so  they can be indexed too
            StringBuilder stringBuilder = new StringBuilder();
            using (IDataReader comments = Blog.GetBlogComments(blog.ModuleId, blog.ItemId))
            {
                while (comments.Read())
                {
                    stringBuilder.Append("  " + comments["Comment"].ToString());
                    stringBuilder.Append("  " + comments["Name"].ToString());

                    if (debugLog) log.Debug("BlogIndexBuilderProvider.IndexItem add comment ");

                }
            }

            // get list of pages where this module is published
            List<PageModule> pageModules
                = PageModule.GetPageModulesByModule(blog.ModuleId);

            foreach (PageModule pageModule in pageModules)
            {
                PageSettings pageSettings
                    = new PageSettings(
                    blog.SiteId,
                    pageModule.PageId);

                //don't index pending/unpublished pages
                if (pageSettings.IsPending) { continue; }

                mojoPortal.SearchIndex.IndexItem indexItem = new mojoPortal.SearchIndex.IndexItem();
                if (blog.SearchIndexPath.Length > 0)
                {
                    indexItem.IndexPath = blog.SearchIndexPath;
                }
                indexItem.SiteId = blog.SiteId;
                indexItem.ExcludeFromRecentContent = blog.ExcludeFromRecentContent;
                indexItem.PageId = pageSettings.PageId;
                indexItem.PageName = pageSettings.PageName;
                indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                indexItem.ModuleViewRoles = module.ViewRoles;

                indexItem.PageMetaDescription = blog.MetaDescription;
                indexItem.PageMetaKeywords = blog.MetaKeywords;
                indexItem.ItemId = blog.ItemId;
                indexItem.ModuleId = blog.ModuleId;
                indexItem.ModuleTitle = module.ModuleTitle;
                indexItem.Title = blog.Title;
                indexItem.Content = blog.Description + " " + blog.MetaDescription + " " + blog.MetaKeywords;
                indexItem.ContentAbstract = blog.Excerpt;
                indexItem.FeatureId = blogFeatureGuid.ToString();
                indexItem.FeatureName = blogFeature.FeatureName;
                indexItem.FeatureResourceFile = blogFeature.ResourceFile;

                indexItem.OtherContent = stringBuilder.ToString();

                indexItem.PublishBeginDate = pageModule.PublishBeginDate;
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
                    indexItem.ViewPage = "Blog/ViewPost.aspx?pageid="
                        + indexItem.PageId.ToInvariantString()
                        + "&mid=" + indexItem.ModuleId.ToInvariantString()
                        + "&ItemID=" + indexItem.ItemId.ToInvariantString()
                        ;
                }
                else
                {
                    indexItem.ViewPage = blog.ItemUrl.Replace("~/", string.Empty);

                }

                indexItem.UseQueryStringParams = false;

                

                mojoPortal.SearchIndex.IndexHelper.RebuildIndex(indexItem);
            }

            if (debugLog) log.Debug("Indexed " + blog.Title);

        }

    }
}
