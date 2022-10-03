/// Author:					    
/// Created:				    2007-08-30
/// Last Modified:			    2013-01-15
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web;
using mojoPortal.Web.ForumUI;
using mojoPortal.Web.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;

namespace mojoPortal.Features
{
    
    public class ForumThreadIndexBuilderProvider : IndexBuilderProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ForumThreadIndexBuilderProvider));
        private static bool debugLog = log.IsDebugEnabled;

        public ForumThreadIndexBuilderProvider()
        { }

        public override void RebuildIndex(
            PageSettings pageSettings,
            string indexPath)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            if ((pageSettings == null) || (indexPath == null))
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("pageSettings object or index path passed to ForumThreadIndexBuilderProvider.RebuildIndex was null");
                }
                return;

            }

            //don't index pending/unpublished pages
            if (pageSettings.IsPending) { return; }

            log.Info(Resources.ForumResources.ForumsFeatureName + " indexing page - " + pageSettings.PageName);

            try
            {
                List<PageModule> pageModules
                        = PageModule.GetPageModulesByPage(pageSettings.PageId);

                Guid forumFeatureGuid = new Guid("38aa5a84-9f5c-42eb-8f4c-105983d419fb");
                ModuleDefinition forumFeature = new ModuleDefinition(forumFeatureGuid);

                // new implementation 2012-05-22: get threads by page, then for each thread concat the posts into one item for indexing
                // previously were indexing individual posts but this makes multiple results in search results for the same thread

                if (ForumConfiguration.AggregateSearchIndexPerThread)
                {
                    DataTable threads = ForumThread.GetThreadsByPage(
                        pageSettings.SiteId,
                        pageSettings.PageId);

                   

                    foreach (DataRow row in threads.Rows)
                    {
                        StringBuilder threadContent = new StringBuilder();

                        int threadId = Convert.ToInt32(row["ThreadID"]);
                        DateTime threadDate = Convert.ToDateTime(row["ThreadDate"]);

                        DataTable threadPosts = ForumThread.GetPostsByThread(threadId);



                        foreach (DataRow r in threadPosts.Rows)
                        {
                            threadContent.Append(r["Post"].ToString());
                        }

                        mojoPortal.SearchIndex.IndexItem indexItem = new mojoPortal.SearchIndex.IndexItem();

                        // lookup publish dates
                        foreach (PageModule pageModule in pageModules)
                        {
                            if (indexItem.ModuleId == pageModule.ModuleId)
                            {
                                indexItem.PublishBeginDate = pageModule.PublishBeginDate;
                                indexItem.PublishEndDate = pageModule.PublishEndDate;
                            }
                        }

                        indexItem.CreatedUtc = threadDate;

                        if (row["MostRecentPostDate"] != DBNull.Value)
                        {
                            indexItem.LastModUtc = Convert.ToDateTime(row["MostRecentPostDate"]);
                        }
                        else
                        {
                            indexItem.LastModUtc = threadDate;
                        }

                        

                        indexItem.SiteId = pageSettings.SiteId;
                        indexItem.PageId = pageSettings.PageId;
                        indexItem.PageName = pageSettings.PageName;
                        indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                        indexItem.ModuleViewRoles = row["ViewRoles"].ToString();
                        indexItem.FeatureId = forumFeatureGuid.ToString();
                        indexItem.FeatureName = forumFeature.FeatureName;
                        indexItem.FeatureResourceFile = forumFeature.ResourceFile;

                        indexItem.ItemId = Convert.ToInt32(row["ItemID"]);
                        indexItem.ModuleId = Convert.ToInt32(row["ModuleID"]);
                        indexItem.ModuleTitle = row["ModuleTitle"].ToString();
                        indexItem.Title = row["Subject"].ToString();

                        indexItem.Content = threadContent.ToString();

                        if (ForumConfiguration.CombineUrlParams)
                        {
                            indexItem.ViewPage = "Forums/Thread.aspx?pageid=" + pageSettings.PageId.ToInvariantString()
                                + "&t=" + row["ThreadID"].ToString() + "~1";
                            indexItem.UseQueryStringParams = false;
                            // still need this since it is aprt of the key
                            indexItem.QueryStringAddendum = "&thread=" + row["ThreadID"].ToString();
                        }
                        else
                        {

                            indexItem.ViewPage = "Forums/Thread.aspx";
                            indexItem.QueryStringAddendum = "&thread=" + row["ThreadID"].ToString();
                        }



                        mojoPortal.SearchIndex.IndexHelper.RebuildIndex(indexItem, indexPath);

                        if (debugLog) log.Debug("Indexed " + indexItem.Title);

                    }

                }
                else
                {


                    //older implementation indexed posts individually

                    DataTable dataTable = ForumThread.GetPostsByPage(
                        pageSettings.SiteId,
                        pageSettings.PageId);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        mojoPortal.SearchIndex.IndexItem indexItem = new mojoPortal.SearchIndex.IndexItem();
                        indexItem.SiteId = pageSettings.SiteId;
                        indexItem.PageId = pageSettings.PageId;
                        indexItem.PageName = pageSettings.PageName;
                        indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                        indexItem.ModuleViewRoles = row["ViewRoles"].ToString();
                        indexItem.FeatureId = forumFeatureGuid.ToString();
                        indexItem.FeatureName = forumFeature.FeatureName;
                        indexItem.FeatureResourceFile = forumFeature.ResourceFile;

                        indexItem.ItemId = Convert.ToInt32(row["ItemID"]);
                        indexItem.ModuleId = Convert.ToInt32(row["ModuleID"]);
                        indexItem.ModuleTitle = row["ModuleTitle"].ToString();
                        indexItem.Title = row["Subject"].ToString();
                        indexItem.Content = row["Post"].ToString();
                        indexItem.ViewPage = "Forums/Thread.aspx";
                        indexItem.QueryStringAddendum = "&thread="
                            + row["ThreadID"].ToString()
                            + "&postid=" + row["PostID"].ToString();

                        // lookup publish dates
                        foreach (PageModule pageModule in pageModules)
                        {
                            if (indexItem.ModuleId == pageModule.ModuleId)
                            {
                                indexItem.PublishBeginDate = pageModule.PublishBeginDate;
                                indexItem.PublishEndDate = pageModule.PublishEndDate;
                            }
                        }

                        //indexItem.PublishBeginDate = Convert.ToDateTime(row["PostDate"]);
                        //indexItem.PublishEndDate = DateTime.MaxValue;

                        mojoPortal.SearchIndex.IndexHelper.RebuildIndex(indexItem, indexPath);

                       if (debugLog) log.Debug("Indexed " + indexItem.Title);

                     }
                }

            }
            catch (System.Data.Common.DbException ex)
            {
                log.Error(ex);
            }

        }

        public override void ContentChangedHandler(
            object sender,
            ContentChangedEventArgs e)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (sender == null) return;
            if (!(sender is ForumThread)) return;


            ForumThread forumThread = (ForumThread)sender;
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            forumThread.SiteId = siteSettings.SiteId;
            forumThread.SearchIndexPath = mojoPortal.SearchIndex.IndexHelper.GetSearchIndexPath(siteSettings.SiteId);

            if (e.IsDeleted)
            {
                if (ThreadPool.QueueUserWorkItem(new WaitCallback(RemoveForumIndexItem), forumThread))
                {
                    if (debugLog) log.Debug("ForumThreadIndexBuilderProvider.RemoveForumIndexItem queued");
                }
                else
                {
                    log.Error("Failed to queue a thread for ForumThreadIndexBuilderProvider.RemoveForumIndexItem");
                }

                //RemoveForumIndexItem(forumThread);
            }
            else
            {
                if (ThreadPool.QueueUserWorkItem(new WaitCallback(IndexItem), forumThread))
                {
                    if (debugLog) log.Debug("ForumThreadIndexBuilderProvider.IndexItem queued");
                }
                else
                {
                    log.Error("Failed to queue a thread for ForumThreadIndexBuilderProvider.IndexItem");
                }

                //IndexItem(forumThread);
            }

        }

        private static void IndexItem(object oForumThread)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (oForumThread == null) return;
            if (!(oForumThread is ForumThread)) return;

            ForumThread forumThread = oForumThread as ForumThread;
            IndexItem(forumThread);

        }

        private static void IndexItem(ForumThread forumThread)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            

            if (forumThread == null)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("forumThread object passed in ForumThreadIndexBuilderProvider.IndexItem was null");
                }
                return;

            }

            Forum forum = new Forum(forumThread.ForumId);
            Module module = new Module(forum.ModuleId);
            Guid forumFeatureGuid = new Guid("38aa5a84-9f5c-42eb-8f4c-105983d419fb");
            ModuleDefinition forumFeature = new ModuleDefinition(forumFeatureGuid);

            // get list of pages where this module is published
            List<PageModule> pageModules
                = PageModule.GetPageModulesByModule(forum.ModuleId);

            // must update index for all pages containing
            // this module
            foreach (PageModule pageModule in pageModules)
            {
                PageSettings pageSettings
                    = new PageSettings(
                    forumThread.SiteId,
                    pageModule.PageId);

                //don't index pending/unpublished pages
                if (pageSettings.IsPending) { continue; }


                mojoPortal.SearchIndex.IndexItem indexItem = new mojoPortal.SearchIndex.IndexItem();
                if (forumThread.SearchIndexPath.Length > 0)
                {
                    indexItem.IndexPath = forumThread.SearchIndexPath;
                }
                indexItem.SiteId = forumThread.SiteId;
                indexItem.PageId = pageModule.PageId;
                indexItem.PageName = pageSettings.PageName;
                // permissions are kept in sync in search index
                // so that results are filtered by role correctly
                indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                indexItem.ModuleViewRoles = module.ViewRoles;
                indexItem.ItemId = forumThread.ForumId;
                indexItem.ModuleId = forum.ModuleId;
                indexItem.ModuleTitle = module.ModuleTitle;
                indexItem.FeatureId = forumFeatureGuid.ToString();
                indexItem.FeatureName = forumFeature.FeatureName;
                indexItem.FeatureResourceFile = forumFeature.ResourceFile;
                indexItem.Title = forumThread.Subject;
                    
                indexItem.PublishBeginDate = pageModule.PublishBeginDate;
                indexItem.PublishEndDate = pageModule.PublishEndDate;
                indexItem.ViewPage = "Forums/Thread.aspx";

                indexItem.CreatedUtc = forumThread.ThreadDate;
                indexItem.LastModUtc = forumThread.MostRecentPostDate;

                if (ForumConfiguration.AggregateSearchIndexPerThread)
                {
                    indexItem.PublishBeginDate = forumThread.MostRecentPostDate;
                    StringBuilder threadContent = new StringBuilder();

                    DataTable threadPosts = ForumThread.GetPostsByThread(forumThread.ThreadId);

                    foreach (DataRow r in threadPosts.Rows)
                    {
                        threadContent.Append(r["Post"].ToString());
                    }

                    //aggregate contents of posts into one indexable content item
                    indexItem.Content = threadContent.ToString();

                    if (ForumConfiguration.CombineUrlParams)
                    {
                        indexItem.ViewPage = "Forums/Thread.aspx?pageid=" + pageModule.PageId.ToInvariantString()
                            + "&t=" + forumThread.ThreadId.ToInvariantString() + "~1";
                        indexItem.UseQueryStringParams = false;
                    }
                    
                    indexItem.QueryStringAddendum = "&thread=" + forumThread.ThreadId.ToInvariantString();
                    
                }
                else
                {
                    //older implementation

                    indexItem.Content = forumThread.PostMessage;

                    
                    indexItem.QueryStringAddendum = "&thread="
                        + forumThread.ThreadId.ToString()
                        + "&postid=" + forumThread.PostId.ToString();
                }



                mojoPortal.SearchIndex.IndexHelper.RebuildIndex(indexItem);

                if (debugLog) { log.Debug("Indexed " + forumThread.Subject); }
                

            }

        }

        public void ThreadMovedHandler(object sender, ForumThreadMovedArgs e)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            ForumThread forumThread = (ForumThread)sender;
            DataTable postIDList = forumThread.GetPostIdList();

            Forum origForum = new Forum(e.OriginalForumId);
            foreach (DataRow row in postIDList.Rows)
            {
                int postID = Convert.ToInt32(row["PostID"]);
                ForumThread post = new ForumThread(forumThread.ThreadId, postID);

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
            if (WebConfigSettings.DisableSearchIndex) { return; }

            if (!(oForumThread is ForumThread)) return;

            ForumThread forumThread = oForumThread as ForumThread;

            // get list of pages where this module is published
            List<PageModule> pageModules
                = PageModule.GetPageModulesByModule(forumThread.ModuleId);

            // must update index for all pages containing
            // this module
            foreach (PageModule pageModule in pageModules)
            {
                mojoPortal.SearchIndex.IndexItem indexItem = new mojoPortal.SearchIndex.IndexItem();
                // note we are just assigning the properties 
                // needed to derive the key so it can be found and
                // deleted from the index
                indexItem.SiteId = forumThread.SiteId;
                indexItem.PageId = pageModule.PageId;
                indexItem.ModuleId = forumThread.ModuleId;
                indexItem.ItemId = forumThread.ForumId;

                if (ForumConfiguration.AggregateSearchIndexPerThread)
                {
                    indexItem.QueryStringAddendum = "&thread=" + forumThread.ThreadId.ToInvariantString();
                }
                else
                {

                    indexItem.QueryStringAddendum = "&thread="
                        + forumThread.ThreadId.ToInvariantString()
                        + "&postid=" + forumThread.PostId.ToInvariantString();
                }

                mojoPortal.SearchIndex.IndexHelper.RemoveIndex(indexItem);
            }

            if (debugLog) { log.Debug("Removed Index "); }

        }

        public static void RemoveForumIndexItem(
            int moduleId,
            int itemId,
            int threadId,
            int postId)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (siteSettings == null)
            {
                log.Error("siteSettings object retrieved in ForumThreadIndexBuilderProvider.RemoveForumIndexItem was null");
                return;
            }

            // get list of pages where this module is published
            List<PageModule> pageModules
                = PageModule.GetPageModulesByModule(moduleId);

            // must update index for all pages containing
            // this module
            foreach (PageModule pageModule in pageModules)
            {
                mojoPortal.SearchIndex.IndexItem indexItem = new mojoPortal.SearchIndex.IndexItem();
                // note we are just assigning the properties 
                // needed to derive the key so it can be found and
                // deleted from the index
                indexItem.SiteId = siteSettings.SiteId;
                indexItem.PageId = pageModule.PageId;
                indexItem.ModuleId = moduleId;
                indexItem.ItemId = itemId;

                if ((ForumConfiguration.AggregateSearchIndexPerThread)||(postId == -1))
                {
                    indexItem.QueryStringAddendum = "&thread=" + threadId.ToInvariantString();
                }
                else
                {

                    indexItem.QueryStringAddendum = "&thread="
                        + threadId.ToInvariantString()
                        + "&postid=" + postId.ToInvariantString();
                }

                mojoPortal.SearchIndex.IndexHelper.RemoveIndex(indexItem);
            }

            if (debugLog) log.Debug("Removed Index ");

        }


    }
}
