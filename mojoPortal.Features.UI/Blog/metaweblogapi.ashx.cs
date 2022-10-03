// Author:					
// Created:				    2011-11-11
// Last Modified:			2013-02-19

using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.PageEventHandlers;
using mojoPortal.Core.API.MetaWeblog;
using mojoPortal.FileSystem;
using mojoPortal.SearchIndex;
using mojoPortal.Web.ContentUI;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.BlogUI
{
    /// <summary>
    /// HTTP Handler for MetaWeblog API
    /// </summary>
    public class metaweblogapi : IHttpHandler
    {
        // related docs
        // http://msdn.microsoft.com/en-us/library/aa738906.aspx
        // http://msdn.microsoft.com/en-us/library/bb463263.aspx

        //http://codex.wordpress.org/XML-RPC_Support
        //http://codex.wordpress.org/XML-RPC_wp

        //http://72.233.56.145/XML-RPC/system.listMethods

        private static readonly ILog log = LogManager.GetLogger(typeof(metaweblogapi));
        private SiteSettings siteSettings = null;
        private SiteUser siteUser = null;
        private TimeZoneInfo timeZone = null;
        private string allowedExtensions = WebConfigSettings.AllowedLessPriveledgedUserUploadFileExtensions;
        private string navigationSiteRoot = string.Empty;
        private string imageSiteRoot = string.Empty;
        private bool isAdmin = false;
        private bool isContentAdmin = false;
        private bool isSiteEditor = false;

        

        /// <summary>
        /// Process the HTTP Request.  Create XMLRPC request, find method call, process it and create response object and sent it back.
        /// This is the heart of the MetaWeblog API
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //var rootUrl = SiteUtils.GetNavigationSiteRoot();

                if (WebConfigSettings.DisableMetaWeblogApi)
                {
                    throw new MetaWeblogException("11", MetaweblogResources.MetaweblogApiDisabled);
                }

                if (WebConfigSettings.UseFolderBasedMultiTenants)
                {
                    navigationSiteRoot = SiteUtils.GetNavigationSiteRoot();
                    imageSiteRoot = WebUtils.GetSiteRoot();
                }
                else
                {
                    navigationSiteRoot = WebUtils.GetHostRoot();
                    imageSiteRoot = navigationSiteRoot;

                }

                var input = new XMLRPCRequest(context);
                var output = new XMLRPCResponse(input.MethodName);

                // this method does not require authentication
                if (input.MethodName == "system.listMethods")
                {
                    output.Response(context);
                    return;
                }

                if((SiteUtils.SslIsAvailable())&&(!SiteUtils.IsSecureRequest()))
                {
                    log.Error("user " + input.UserName + " posted to metaweblogapi.ashx without using SSL. This can expose security credentials.");
                    throw new MetaWeblogException("11", MetaweblogResources.SSLRequired);
                }

                //Security.ImpersonateUser(input.UserName, input.Password);
                bool isUser = Membership.ValidateUser(input.UserName, input.Password);

                
                if(!isUser)
                {
                    throw new MetaWeblogException("11", MetaweblogResources.AuthenticationFailed);
                }

                siteSettings = CacheHelper.GetCurrentSiteSettings();

                if (siteSettings == null)
                {
                    throw new MetaWeblogException("11", MetaweblogResources.SiteValidationFailed);
                }

                siteUser = GetSiteUser(input.UserName);

                if (siteUser == null)
                {
                    throw new MetaWeblogException("11", MetaweblogResources.AuthenticationFailed);
                }

                switch (input.MethodName)
                {
                    case "metaWeblog.newPost":
                        output.PostID = this.NewPost(
                            input.BlogID, input.UserName, input.Password, input.Post, input.Publish);
                        break;
                    case "metaWeblog.editPost":
                        output.Completed = this.EditPost(
                            input.PostID, input.UserName, input.Password, input.Post, input.Publish);
                        break;
                    case "metaWeblog.getPost":
                        output.Post = this.GetPost(input.PostID, input.UserName, input.Password);
                        break;
                    case "metaWeblog.newMediaObject":
                    case "wp.uploadFile":
                        output.MediaInfo = this.NewMediaObject(
                            input.BlogID, input.UserName, input.Password, input.MethodName, input.MediaObject, context);
                        break;
                    case "metaWeblog.getCategories":
                        output.Categories = this.GetCategories(input.BlogID, input.UserName, input.Password);
                        break;
                    case "wp.newCategory":
                        output.CategoryID = this.NewCategory(input.BlogID, input.UserName, input.Password, input.Category);
                        break;
                    case "metaWeblog.getRecentPosts":
                        output.Posts = this.GetRecentPosts(
                            input.BlogID, input.UserName, input.Password, input.NumberOfPosts);
                        break;
                    case "blogger.getUsersBlogs":
                    case "metaWeblog.getUsersBlogs":
                    case "wp.getUsersBlogs":
                        output.Blogs = this.GetUserBlogs(input.AppKey, input.UserName, input.Password);
                        break;
                    case "blogger.deletePost":
                        output.Completed = this.DeletePost(
                            input.AppKey, input.PostID, input.UserName, input.Password, input.Publish);
                        break;
                    case "blogger.getUserInfo":
                        // Not implemented.  Not planned.
                        throw new MetaWeblogException("10", "The method GetUserInfo is not implemented.");
                    case "wp.newPage":
                        if (WebConfigSettings.DisableEditingPagesInMetaWeblogApi)
                        {
                            throw new MetaWeblogException("11", MetaweblogResources.PageEditingDisabled);
                        }

                        //throw new MetaWeblogException("10", "The method newPage is not implemented.");
                        output.PageID = this.NewPage(
                            input.BlogID, input.UserName, input.Password, input.Page, input.Publish);
                        break;
                    case "wp.getPageList":
                        if (WebConfigSettings.DisableEditingPagesInMetaWeblogApi)
                        {
                            throw new MetaWeblogException("11", MetaweblogResources.PageEditingDisabled);
                        }
                        
                        output.Pages = this.GetPageList(input.BlogID, input.UserName, input.Password);
                        break;
                    case "wp.getPages":

                        if (WebConfigSettings.DisableEditingPagesInMetaWeblogApi)
                        {
                            throw new MetaWeblogException("11", MetaweblogResources.PageEditingDisabled);
                        }
                        
                        output.Pages = this.GetPages(input.BlogID, input.UserName, input.Password);
                        break;
                    case "wp.getPage":
                        if (WebConfigSettings.DisableEditingPagesInMetaWeblogApi)
                        {
                            throw new MetaWeblogException("11", MetaweblogResources.PageEditingDisabled);
                        }
                        
                        output.Page = this.GetPage(input.BlogID, input.PageID, input.UserName, input.Password);
                        break;
                    case "wp.editPage":
                        if (WebConfigSettings.DisableEditingPagesInMetaWeblogApi)
                        {
                            throw new MetaWeblogException("11", MetaweblogResources.PageEditingDisabled);
                        }
                       
                        output.Completed = this.EditPage(
                            input.BlogID, input.PageID, input.UserName, input.Password, input.Page, input.Publish);
                        break;
                    case "wp.deletePage":
                        if (WebConfigSettings.DisableEditingPagesInMetaWeblogApi)
                        {
                            throw new MetaWeblogException("11", MetaweblogResources.PageEditingDisabled);
                        }

                        if (WebConfigSettings.DisableDeletingPagesInMetaWeblogApi)
                        {
                            throw new MetaWeblogException("11", MetaweblogResources.DeletingPagesDisabled);
                        }
                        // Not implemented. 
                        //throw new MetaWeblogException("10", "The method deletePage is not implemented.");
                        output.Completed = this.DeletePage(input.BlogID, input.PageID, input.UserName, input.Password);
                        break;
                    case "wp.getAuthors":
                        // Not implemented. 
                        throw new MetaWeblogException("10", "The method getAuthors is not implemented.");
                        //output.Authors = this.GetAuthors(input.BlogID, input.UserName, input.Password);
                        //break;
                    case "wp.getTags":
                        // Not implemented. 
                        throw new MetaWeblogException("10", "The method getTags is not implemented.");
                        //output.Keywords = this.GetKeywords(input.BlogID, input.UserName, input.Password);
                        //break;
                }

                output.Response(context);
            }
            catch (MetaWeblogException mex)
            {
                log.Error(mex);
                var output = new XMLRPCResponse("fault");
                var fault = new MWAFault { faultCode = mex.Code, faultString = mex.Message };
                output.Fault = fault;
                output.Response(context);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                var output = new XMLRPCResponse("fault");
                var fault = new MWAFault { faultCode = "0", faultString = ex.Message };
                output.Fault = fault;
                output.Response(context);
            }
        }

        

        #region Blog Methods

        
        /// <summary>
        /// metaWeblog.newPost method
        /// </summary>
        /// <param name="blogId">
        /// always 1000 in BlogEngine since it is a singlar blog instance
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <param name="sentPost">
        /// struct with post details
        /// </param>
        /// <param name="publish">
        /// mark as published?
        /// </param>
        /// <returns>
        /// postID as string
        /// </returns>
        internal string NewPost(string blogId, string userName, string password, MWAPost sentPost, bool publish)
        {
            int moduleId = Convert.ToInt32(blogId);

            if (!UserCanPostToBlog(userName, moduleId))
            {
                throw new MetaWeblogException("11", MetaweblogResources.AccessDenied);
            }

            try
            {
                Module module = new Module(moduleId);
                
               

                Blog post = new Blog();
                post.ModuleId = module.ModuleId;
                post.ModuleGuid = module.ModuleGuid;

                post.UserGuid = siteUser.UserGuid;
                post.LastModUserGuid = siteUser.UserGuid;

                if ((sentPost.postDate != null) && (sentPost.postDate > DateTime.MinValue) && (sentPost.postDate < DateTime.MaxValue))
                {
                    if (!WebConfigSettings.DisableUseOfPassedInDateForMetaWeblogApi) { post.StartDate = sentPost.postDate; }
                }
            
                post.Title = sentPost.title;
                post.Description = SiteUtils.ChangeFullyQualifiedLocalUrlsToRelative(navigationSiteRoot, imageSiteRoot, sentPost.description);

                if (!string.IsNullOrEmpty(sentPost.excerpt))
                {
                    if (BlogConfiguration.UseExcerptFromMetawblogAsMetaDescription)
                    {
                        post.MetaDescription = UIHelper.CreateExcerpt(sentPost.excerpt, BlogConfiguration.MetaDescriptionMaxLengthToGenerate);
                    }
                    post.Excerpt = SiteUtils.ChangeFullyQualifiedLocalUrlsToRelative(navigationSiteRoot, imageSiteRoot, sentPost.excerpt);
                }
                post.IncludeInFeed = true;
                post.IsPublished = publish;
               
                //post.Slug = sentPost.slug;
                //string author = String.IsNullOrEmpty(sentPost.author) ? userName : sentPost.author;

                switch (sentPost.commentPolicy)
                {
                    //closed
                    case "2":
                        post.AllowCommentsForDays = -1; // closed
                        break;

                    // open
                    case "1":
                    default:
                       
                        Hashtable moduleSettings = ModuleSettings.GetModuleSettings(post.ModuleId);
                        BlogConfiguration config = new BlogConfiguration(moduleSettings);
                        post.AllowCommentsForDays = config.DefaultCommentDaysAllowed;
                        
                        break;
                    
                }

                string newUrl = SiteUtils.SuggestFriendlyUrl(post.Title, siteSettings);

                post.ItemUrl = "~/" + newUrl;

                if (!post.Title.Contains("Theme Detection")) // don't index a temp post from livewriter that will be deleted
                {
                    post.ContentChanged += new ContentChangedEventHandler(blog_ContentChanged);
                }

                post.Save();

                int pageId = GetPageIdForModule(moduleId);

                FriendlyUrl newFriendlyUrl = new FriendlyUrl();
                newFriendlyUrl.SiteId = siteSettings.SiteId;
                newFriendlyUrl.SiteGuid = siteSettings.SiteGuid;
                newFriendlyUrl.PageGuid = post.BlogGuid;
                newFriendlyUrl.Url = newUrl;
                newFriendlyUrl.RealUrl = "~/Blog/ViewPost.aspx?pageid="
                    + pageId.ToInvariantString()
                    + "&mid=" + post.ModuleId.ToInvariantString()
                    + "&ItemID=" + post.ItemId.ToInvariantString();

                if (pageId > -1)
                {
                    newFriendlyUrl.Save();
                }

                SetCategories(post, sentPost);

                //post.Tags.Clear();
                //foreach (var item in sentPost.tags.Where(item => item != null && item.Trim() != string.Empty))
                //{
                //    post.Tags.Add(item);
                //}

                if (!post.Title.Contains("Theme Detection")) // don't index a temp post from livewriter that will be deleted
                {
                    SiteUtils.QueueIndexing();
                }
            
           
                return post.ItemId.ToString();

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new MetaWeblogException("12", string.Format("Create new post failed.  Error: {0}", ex.Message));
            }
        }

        /// <summary>
        /// metaWeblog.editPost method
        /// </summary>
        /// <param name="postId">
        /// post guid in string format
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <param name="sentPost">
        /// struct with post details
        /// </param>
        /// <param name="publish">
        /// mark as published?
        /// </param>
        /// <returns>
        /// 1 if successful
        /// </returns>
        internal bool EditPost(string postId, string userName, string password, MWAPost sentPost, bool publish)
        {
            int blogid = Convert.ToInt32(postId);

            

            if (!UserCanEditPost(userName, blogid))
            {
                throw new MetaWeblogException("11", MetaweblogResources.AccessDenied);
            }

            string author = String.IsNullOrEmpty(sentPost.author) ? userName : sentPost.author;

            Blog post = new Blog(blogid);


            if (post.ItemId == -1)
            {
                //not found
                throw new MetaWeblogException("11", MetaweblogResources.PostNotFound);
            }

            try
            {
                Hashtable moduleSettings = ModuleSettings.GetModuleSettings(post.ModuleId);
                BlogConfiguration config = new BlogConfiguration(moduleSettings);
                
                post.LastModUserGuid = siteUser.UserGuid;
                post.Title = sentPost.title;
                post.Description = SiteUtils.ChangeFullyQualifiedLocalUrlsToRelative(navigationSiteRoot, imageSiteRoot, sentPost.description);
                post.IsPublished = publish;
                //post.Slug = sentPost.slug;

                if (!string.IsNullOrEmpty(sentPost.excerpt))
                {
                    if (BlogConfiguration.UseExcerptFromMetawblogAsMetaDescription)
                    {
                        post.MetaDescription = UIHelper.CreateExcerpt(sentPost.excerpt, BlogConfiguration.MetaDescriptionMaxLengthToGenerate);
                    }
                    post.Excerpt = SiteUtils.ChangeFullyQualifiedLocalUrlsToRelative(navigationSiteRoot, imageSiteRoot, sentPost.excerpt);
                }

                
                
                switch (sentPost.commentPolicy)
                {
                    //closed
                    case "2":
                        post.AllowCommentsForDays = -1; // closed
                        break;
                    // open
                    case "1":
                        // if the post was previously closed to comments
                        // re-open it using the default allowed days
                        if (post.AllowCommentsForDays < 0)
                        { 
                            post.AllowCommentsForDays = config.DefaultCommentDaysAllowed;
                        }

                        break;
                    //else unspecified, no change
                }

                post.ContentChanged += new ContentChangedEventHandler(blog_ContentChanged);

                bool enableContentVersioning = config.EnableContentVersioning;

                if ((siteSettings.ForceContentVersioning) || (WebConfigSettings.EnforceContentVersioningGlobally))
                {
                    enableContentVersioning = true;
                }

                if (enableContentVersioning)
                {
                    post.CreateHistory(siteSettings.SiteGuid);
                }

                post.Save();

                SetCategories(post, sentPost);

                SiteUtils.QueueIndexing();

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new MetaWeblogException("12", string.Format("EditPost failed.  Error: {0}", ex.Message));
            }

            
        }

        /// <summary>
        /// blogger.deletePost method
        /// </summary>
        /// <param name="appKey">
        /// Key from application.  Outdated methodology that has no use here.
        /// </param>
        /// <param name="postId">
        /// post guid in string format
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <param name="publish">
        /// mark as published?
        /// </param>
        /// <returns>
        /// Whether deletion was successful or not.
        /// </returns>
        internal bool DeletePost(string appKey, string postId, string userName, string password, bool publish)
        {
            
            int blogid = Convert.ToInt32(postId);

            if (!UserCanEditPost(userName, blogid))
            {
                throw new MetaWeblogException("11", MetaweblogResources.AccessDenied);
            }

            Blog post = new Blog(blogid);

            if (post.ItemId == -1)
            {
                //not found
                throw new MetaWeblogException("11", MetaweblogResources.PostNotFound);
            }

            try
            {
                if (WebConfigSettings.LogIpAddressForContentDeletions)
                {

                    log.Info("user " + siteUser.Name + " deleted blog post via metaweblogapi " + post.Title + " from ip address " + SiteUtils.GetIP4Address());

                }

                post.ContentChanged += new ContentChangedEventHandler(blog_ContentChanged);
                post.Delete();
                FriendlyUrl.DeleteByPageGuid(post.BlogGuid);
                SiteUtils.QueueIndexing();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new MetaWeblogException("12", string.Format("DeletePost failed.  Error: {0}", ex.Message));
            }

            return true;
        }


        /// <summary>
        /// metaWeblog.getPost method
        /// </summary>
        /// <param name="postId">
        /// post guid in string format
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <returns>
        /// struct with post details
        /// </returns>
        internal MWAPost GetPost(string postId, string userName, string password)
        {
            var sendPost = new MWAPost();

            int blogId = Convert.ToInt32(postId);

            if (blogId == -1)
            {
                throw new MetaWeblogException("11", MetaweblogResources.NoBlogConfigured);
            }

            if (!UserCanEditPost(userName, blogId))
            {
                throw new MetaWeblogException("11", MetaweblogResources.AccessDenied);
            }

            Blog post = new Blog(blogId);

            if (post.ItemId == -1)
            {
                //not found
                throw new MetaWeblogException("11", MetaweblogResources.PostNotFound);
            }

            try
            {
                //Hashtable moduleSettings = ModuleSettings.GetModuleSettings(post.ModuleId);
                //BlogConfiguration config = new BlogConfiguration(moduleSettings);

                sendPost.link = FormatUrl(post.ItemUrl, post.ItemId, post.ModuleId);
                sendPost.postID = post.ItemId.ToString();
                sendPost.postDate = post.StartDate.ToLocalTime(timeZone);
                sendPost.title = post.Title;
                sendPost.description = SiteUtils.ChangeRelativeUrlsToFullyQualifiedUrls(navigationSiteRoot, imageSiteRoot, post.Description);
                //sendPost.link = post.AbsoluteLink.AbsoluteUri;
                //sendPost.slug = post.Slug;
                sendPost.excerpt = SiteUtils.ChangeRelativeUrlsToFullyQualifiedUrls(navigationSiteRoot, imageSiteRoot, post.Excerpt);
                
                sendPost.publish = post.IsPublished;

                sendPost.categories = GetCategoriesForPost(post.ItemId);

                if(siteSettings.UseEmailForLogin)
                {
                    sendPost.author = post.UserEmail;
                }
                else
                {
                    sendPost.author = post.UserLoginName;
                }

                if (post.AllowCommentsForDays == -1) //closed
                {
                    sendPost.commentPolicy = "2"; //closed
                }
                else
                {
                    DateTime endDate = post.StartDate.AddDays((double)post.AllowCommentsForDays);
                    if (endDate > DateTime.UtcNow)
                    {
                        sendPost.commentPolicy = "1"; //allowed
                    }
                    else
                    {
                        sendPost.commentPolicy = "2";
                    }
                }

               

                //var tags = post.Tags.ToList();

                //sendPost.tags = tags;

                return sendPost;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new MetaWeblogException("12", string.Format("GetPost new post failed.  Error: {0}", ex.Message));
            }
        }



        /// <summary>
        /// metaWeblog.getRecentPosts method
        /// </summary>
        /// <param name="blogId">
        /// always 1000 in BlogEngine since it is a singlar blog instance
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <param name="numberOfPosts">
        /// number of posts to return
        /// </param>
        /// <returns>
        /// array of post structs
        /// </returns>
        internal List<MWAPost> GetRecentPosts(string blogId, string userName, string password, int numberOfPosts)
        {
            int moduleId = Convert.ToInt32(blogId);

            if (moduleId == -1) { return new List<MWAPost>(); } //pages only no posts

            if (!UserCanPostToBlog(userName, moduleId))
            {
                throw new MetaWeblogException("11", MetaweblogResources.AccessDenied);
            }

            try
            {
                var sendPosts = new List<MWAPost>();

                DataSet blogPosts = Blog.GetBlogsForMetaWeblogApiDataSet(moduleId, DateTime.MaxValue);

                //using (IDataReader rdr = Blog.GetBlogsForMetaWeblogApi(moduleId, DateTime.MaxValue))
                //{
                    //while (rdr.Read())
                foreach (DataRow rdr in blogPosts.Tables["Posts"].Rows)
                    {
                        var tempPost = new MWAPost
                        {
                            postID = Convert.ToInt32(rdr["ItemID"]).ToInvariantString(),
                            postDate = Convert.ToDateTime(rdr["StartDate"]).ToLocalTime(timeZone),
                            title = rdr["Heading"].ToString(),
                            description = SiteUtils.ChangeRelativeUrlsToFullyQualifiedUrls(navigationSiteRoot, imageSiteRoot, rdr["Description"].ToString()),
                            link = FormatUrl(rdr["ItemUrl"].ToString(), Convert.ToInt32(rdr["ItemID"]), Convert.ToInt32(rdr["ModuleID"])),
                            // slug = post.Slug,
                            excerpt = SiteUtils.ChangeRelativeUrlsToFullyQualifiedUrls(navigationSiteRoot, imageSiteRoot, rdr["Abstract"].ToString()),
                            //commentPolicy = post.HasCommentsEnabled ? string.Empty : "0",
                            publish = Convert.ToBoolean(rdr["IsPublished"]),
                            categories = GetCategories(blogPosts.Tables["Categories"], Convert.ToInt32(rdr["ItemID"])),
							//includeImageInExcerpt = Convert.ToBoolean(rdr["IncludeImageInExcerpt"]),
							//includeImageInPost = Convert.ToBoolean(rdr["IncludeImageInPost"]),
							wp_post_thumbnail = rdr["headlineImageUrl"].ToString()
						};

                        //var tempCats = post.Categories.Select(t => Category.GetCategory(t.Id).ToString()).ToList();

                        //tempPost.categories = tempCats;

                        //var tempTags = post.Tags.ToList();

                        //tempPost.tags = tempTags;

                        sendPosts.Add(tempPost);
                    }
                //}

                return sendPosts;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new MetaWeblogException("12", string.Format("GetRecentPosts failed.  Error: {0}", ex.Message));
            }
        }

        private List<string> GetCategories(DataTable categories, int postId)
        {
            List<string> cats = new List<string>();
            foreach (DataRow row in categories.Rows)
            {
                int itemId = Convert.ToInt32(row["ItemID"]);
                if (itemId == postId)
                {
                    cats.Add(row["Category"].ToString());
                }

            }

            return cats;

        }
        

        /// <summary>
        /// metaWeblog.getCategories method
        /// </summary>
        /// <param name="blogId">
        /// always 1000 in BlogEngine since it is a singlar blog instance
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <param name="rootUrl">
        /// The root URL.
        /// </param>
        /// <returns>
        /// array of category structs
        /// </returns>
        internal List<MWACategory> GetCategories(string blogId, string userName, string password)
        {
            int moduleId = Convert.ToInt32(blogId);

            if (moduleId == -1) { return new List<MWACategory>(); } //pages only

            if (!UserCanPostToBlog(userName, moduleId))
            {
                throw new MetaWeblogException("11", MetaweblogResources.AccessDenied);
            }

            try
            {
                return GetCategoriesForBlog(moduleId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new MetaWeblogException("12", string.Format("GetCategories failed.  Error: {0}", ex.Message));
            }
            
        }


        internal string NewCategory(string blogId, string userName, string password, string category)
        {
            int moduleId = Convert.ToInt32(blogId);

            if (!UserCanPostToBlog(userName, moduleId))
            {
                throw new MetaWeblogException("11", MetaweblogResources.AccessDenied);
            }

            List<MWACategory> allBlogCategories = GetCategoriesForBlog(moduleId);

            MWACategory categoryInfo = allBlogCategories.Find(delegate(MWACategory ci)
            { return ci.title.Equals(category, StringComparison.OrdinalIgnoreCase); });

            if ((categoryInfo.id != null) && (categoryInfo.id.Length > 0))
            {
                    return categoryInfo.id;
                
            }
            else
            {
                int newCategoryId = Blog.AddBlogCategory(moduleId, category);
                return newCategoryId.ToInvariantString();
            }
        }
        
        

        /// <summary>
        /// blogger.getUsersBlogs method
        /// </summary>
        /// <param name="appKey">
        /// Key from application.  Outdated methodology that has no use here.
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <param name="rootUrl">
        /// The root URL.
        /// </param>
        /// <returns>
        /// array of blog structs
        /// </returns>
        internal List<MWABlogInfo> GetUserBlogs(string appKey, string userName, string password)
        {
            try
            {
                var allBlogs = new List<MWABlogInfo>();

                using (IDataReader rdr = Module.GetModulesForSite(siteSettings.SiteId, Blog.FeatureGuid))
                {
                    while (rdr.Read())
                    {
                        var temp = new MWABlogInfo 
                        { 
                            url = navigationSiteRoot + rdr["Url"].ToString().Replace("~", string.Empty), 
                            blogID = Convert.ToInt32(rdr["ModuleID"]).ToInvariantString(), 
                            blogName = rdr["ModuleTitle"].ToString() ,
                           // editUserId = Convert.ToInt32(rdr["EditUserID"]), uncomment after 2.3.7.2 script
                            pageEditRoles = rdr["EditRoles"].ToString(),
                            moduleEditRoles = rdr["AuthorizedEditRoles"].ToString(),
                            xmlrpcUrl = navigationSiteRoot + "/xmlrpc.php"

                        };

                        allBlogs.Add(temp);
                    }
                }


                var userBlogs = new List<MWABlogInfo>();

                foreach (MWABlogInfo blog in allBlogs)
                {
                    if (UserCanEdit(blog.pageEditRoles, blog.moduleEditRoles, blog.editUserId)) //change to blog.editUserId after 2.3.7.2 script
                    {
                        userBlogs.Add(blog);
                    }
                }

                if (userBlogs.Count == 0)
                {
                    MWABlogInfo pagesOnly = new MWABlogInfo();
                    pagesOnly.blogID = "-1";
                    pagesOnly.blogName = MetaweblogResources.PagesOnly;
                    pagesOnly.url = navigationSiteRoot;
                    userBlogs.Add(pagesOnly);
                }

                return userBlogs;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new MetaWeblogException("12", string.Format("GetUserBlogs failed.  Error: {0}", ex.Message));
            }
        }

        /// <summary>
        /// metaWeblog.newMediaObject method
        /// </summary>
        /// <param name="blogId">
        /// always 1000 in BlogEngine since it is a singlar blog instance
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <param name="mediaObject">
        /// struct with media details
        /// </param>
        /// <param name="request">
        /// The HTTP request.
        /// </param>
        /// <returns>
        /// struct with url to media
        /// </returns>
        internal MWAMediaInfo NewMediaObject(
            string blogId, string userName, string password, string methodName, MWAMediaObject mediaObject, HttpContext request)
        {
            string virtualUploadPath = GetUserUploadVirtualPath();

            if (virtualUploadPath.Length == 0)
            {
                throw new MetaWeblogException("11", MetaweblogResources.AccessDenied);
            }

            string ext = Path.GetExtension(mediaObject.name);
            if (!SiteUtils.IsAllowedUploadBrowseFile(ext, allowedExtensions))
            {
                throw new MetaWeblogException("11", MetaweblogResources.FileExtensionNotAllowed);
            }

            try
            {
                
               
                //var serverPath = virtualUploadPath;
                var fileName = Path.GetFileName(mediaObject.name).ToCleanFileName();

                

                IFileSystem fileSystem = GetFileSystem();

                if (!fileSystem.FolderExists(virtualUploadPath))
                {
                    fileSystem.CreateFolder(virtualUploadPath);
                }

                if (fileSystem.FileExists(virtualUploadPath + fileName))
                {
                    // Find unique fileName
                    for (var count = 1; count < 30000; count++)
                    {
                        var tempFileName = fileName.Insert(fileName.LastIndexOf('.'), string.Format("_{0}", count));
                        if (fileSystem.FileExists(virtualUploadPath + tempFileName))
                        {
                            continue;
                        }

                        fileName = tempFileName;
                        break;
                    }
                }

                //this is needed for Blogsy apparently or it crashes when uploading images
                if (methodName == "wp.uploadFile")
                {
                    fileName = "wpid-" + fileName;
                }

                // Save File
                //using (var fs = new FileStream(serverPath + fileName, FileMode.Create))
                using (Stream fs = fileSystem.GetWritableStream(virtualUploadPath + fileName))
                {
                    using (var bw = new BinaryWriter(fs))
                    {
                        bw.Write(mediaObject.bits);
                       
                    }
                }

                // Set Url
                var rootUrl = imageSiteRoot + virtualUploadPath.Replace("~", string.Empty);
                //var rootUrl = virtualUploadPath.Replace("~", string.Empty);

                var mediaInfo = new MWAMediaInfo();
                mediaInfo.file = fileName;
                //mediaInfo.file = mediaObject.name;
                //mediaInfo.type = IOHelper.GetMimeType(Path.GetExtension(fileName));
                mediaInfo.type = mediaObject.type; 
            
                mediaInfo.url = rootUrl  + fileName;
                return mediaInfo;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new MetaWeblogException("12", string.Format("NewMediaObject failed.  Error: {0}", ex.Message));
            }
        }


        #endregion


        

        #region Page Methods

        /// <summary>
        /// wp.getPages method
        /// </summary>
        /// <param name="blogId">
        /// blogID in string format
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <returns>
        /// a list of pages
        /// </returns>
        internal List<MWAPage> GetPages(string blogId, string userName, string password)
        {
            List<MWAPage> allPages = new List<MWAPage>();

            HtmlRepository repository = new HtmlRepository();

            using (IDataReader reader = repository.GetHtmlForMetaWeblogApi(siteSettings.SiteId))
            {
                while (reader.Read())
                {
                    MWAPage p = new MWAPage();
                    p.description = reader["Body"].ToString();
                    p.link = FormatPageUrl(Convert.ToInt32(reader["PageID"]), reader["Url"].ToString(), Convert.ToBoolean(reader["UseUrl"]));

            
                    // adjust from utc to user time zone
                    if (reader["PublishBeginDate"] != DBNull.Value)
                    {
                        p.pageUtcDate = Convert.ToDateTime(reader["PublishBeginDate"]);
                        p.pageDate = Convert.ToDateTime(reader["PublishBeginDate"]).ToLocalTime(timeZone);
                        //p.pageDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(Convert.ToDateTime(reader["PublishBeginDate"]), DateTimeKind.Utc), timeZone);
                    }
                    else
                    {
                        p.pageUtcDate = DateTime.UtcNow;
                        p.pageDate = DateTime.UtcNow.AddMinutes(-5).ToLocalTime(timeZone);
                    }
                    
                    p.pageID = reader["PageGuid"].ToString().ToLowerInvariant();
                    p.pageParentID = reader["ParentGuid"].ToString().ToLowerInvariant();

                    p.pageOrder = Convert.ToInt32(reader["PageOrder"]).ToInvariantString();
                   
                   
                    p.title = reader["PageName"].ToString();
                    p.pageEditRoles = reader["EditRoles"].ToString();
                    p.moduleEditRoles = reader["AuthorizedEditRoles"].ToString();

                    bool allowComments = Convert.ToBoolean(reader["EnableComments"]);
                    if (allowComments)
                    {
                        p.commentPolicy = "1";
                    }
                    else
                    {
                        p.commentPolicy = "0";
                    }

                    bool isDraft = Convert.ToBoolean(reader["IsPending"]);
                    if (isDraft)
                    {
                        p.published = "draft";
                    }
                    else
                    {
                        p.published = "publish";
                    }

                    allPages.Add(p);

                    
                }
            }

            List<MWAPage> allowedPages = new List<MWAPage>();

            foreach (MWAPage p in allPages)
            {
                // filter out all except the first html instance on the page
                if (Contains(allowedPages, p.pageID)) { continue; }
                if (UserCanEdit(p)) { allowedPages.Add(p); }
            }

            return allowedPages;

            
        }

        

        /// <summary>
        /// wp.getPageList method
        /// </summary>
        /// <param name="blogId">
        /// blogID in string format
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <returns>
        /// a list of pages used to populate the parent page dropdown
        /// here we have to include all viewrole allowed pages because this is for the parent page dropdown and we
        /// cannot assume anything about the hierarchy. If we left out the parent page on edit it would try to move it to root
        /// so we must make sure this list contains the parent page for any page the user can edit
        /// </returns>
        internal List<MWAPage> GetPageList(string blogId, string userName, string password)
        {
            List<MWAPage> allPages = new List<MWAPage>();

            using (IDataReader reader = PageSettings.GetChildPagesSortedAlphabetic(siteSettings.SiteId, -2))
            {
                while (reader.Read())
                {
                    MWAPage p = new MWAPage();
                    
                    p.pageID = reader["PageGuid"].ToString().ToLowerInvariant();
                    p.pageParentID = reader["ParentGuid"].ToString().ToLowerInvariant();
                    p.pageOrder = Convert.ToInt32(reader["PageOrder"]).ToInvariantString();
                    p.title = reader["PageName"].ToString();
                    p.pageEditRoles = reader["EditRoles"].ToString();

                    // we are purposely leaving out the dates for lists because wlw interprets them strangely for pages
                    // and filters out things with future dates, but since it seems to treat local time as utc
                    // it filters thing sout thsat it shouldn't and really using UTC causes other problems.
                    // we are setting the dates here but they are not rendered for the lists in
                    // XMLRPCResponse.cs
                    p.pageUtcDate = DateTime.UtcNow.AddDays(-2);
                    p.pageDate = DateTime.UtcNow.AddDays(-2).ToLocalTime(timeZone);
                    

                   
                    string viewRoles = reader["AuthorizedRoles"].ToString();
                    if (UserCanView(viewRoles))
                    {
                        allPages.Add(p);
                    }

                }
            }

            

            return allPages;


        }



        /// <summary>
        /// wp.getPage method
        /// </summary>
        /// <param name="blogId">
        /// blogID in string format
        /// </param>
        /// <param name="pageId">
        /// page guid in string format
        /// </param>
        /// <param name="userName">
        /// login username
        /// </param>
        /// <param name="password">
        /// login password
        /// </param>
        /// <returns>
        /// struct with post details
        /// </returns>
        internal MWAPage GetPage(string blogId, string pageId, string userName, string password)
        {
            var p = new MWAPage();
            PageSettings page = CacheHelper.GetPage(new Guid(pageId));

            if (page.PageId == -1)
            { //doesn't exist
                throw new MetaWeblogException("11", MetaweblogResources.PageNotFound);
            }

            Module m  = GetFirstCenterPaneHtmlModule(page);

            if (m == null)
            {
                throw new MetaWeblogException("11", MetaweblogResources.ContentInstanceNotFound);
            }

            HtmlRepository repository = new HtmlRepository();
            HtmlContent html = repository.Fetch(m.ModuleId);

            // html does not immediately exist as soon as module is added to the page
            // it is created upon first edit if it does not so we need to check for null
            if (html != null)
            {
                p.description = html.Body;
                
            }

            p.pageUtcDate = page.LastModifiedUtc;
            p.pageDate = page.LastModifiedUtc.ToLocalTime(timeZone);
            if (page.UrlHasBeenAdjustedForFolderSites)
            {
                p.link = FormatPageUrl(page.PageId, page.UnmodifiedUrl, page.UseUrl);
            }
            else
            {
                p.link = FormatPageUrl(page.PageId, page.Url, page.UseUrl);
            }

            p.pageID = page.PageGuid.ToString();
            p.pageParentID = page.ParentGuid.ToString();

            if (page.ParentGuid != Guid.Empty)
            {
                PageSettings parentPage = new PageSettings(page.ParentGuid);
                p.parentTitle = parentPage.PageName;
            }
            else
            {
                p.pageParentID = string.Empty; // we don't want to pass Guid.Empty it causes problems in wlw
            }

            p.pageOrder = page.PageOrder.ToInvariantString();
    
            // generally module title and page name should be the same
            // except on the home page since we don't want to change the pagename of the home page
            // to match th emodule title we must return the module title when getting the page for edit purposes
            p.title = m.ModuleTitle;
            p.pageEditRoles = page.EditRoles;
            p.moduleEditRoles = m.AuthorizedEditRoles;

            if (page.EnableComments)
            {
                p.commentPolicy = "1";
            }
            else
            {
                p.commentPolicy = "2";
            }

            if (!UserCanEdit(p))
            {
                throw new MetaWeblogException("11", "User doesn't have permission on this content");
            }

            return p;
        }





        /// <summary>
        /// Edits the page.
        /// </summary>
        /// <param name="blogId">
        /// The blog id.
        /// </param>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="mwaPage">
        /// The m page.
        /// </param>
        /// <param name="publish">
        /// The publish.
        /// </param>
        /// <returns>
        /// The edit page.
        /// </returns>
        internal bool EditPage(string blogId, string pageId, string userName, string password, MWAPage mwaPage, bool publish)
        {
            if ((string.IsNullOrEmpty(pageId))||(pageId.Length != 36))
            {
                throw new MetaWeblogException("11", MetaweblogResources.PageNotFound);
            }
            
            PageSettings page = CacheHelper.GetPage(new Guid(pageId));

            if (page.PageId == -1)
            { //doesn't exist
                throw new MetaWeblogException("11", MetaweblogResources.PageNotFound);
            }

            if (page.SiteId != siteSettings.SiteId)
            { //doesn't exist
                throw new MetaWeblogException("11", MetaweblogResources.PageNotFound);
            }

            //if (!publish)
            //{
            //    throw new MetaWeblogException("11", "Sorry draft pages are not supported through this API.");
            //}

            Module m = GetFirstCenterPaneHtmlModule(page);

            if (m == null)
            {
                throw new MetaWeblogException("11", MetaweblogResources.ContentInstanceNotFound);
            }

            if (!UserCanEdit(page, m))
            {
                throw new MetaWeblogException("11", MetaweblogResources.AccessDenied);
            }

            PageSettings parentPage = null;
            Hashtable moduleSettings = ModuleSettings.GetModuleSettings(m.ModuleId);
            HtmlConfiguration config = new HtmlConfiguration(moduleSettings);

            HtmlRepository repository = new HtmlRepository();
            HtmlContent html = repository.Fetch(m.ModuleId);

            if (html == null)
            {
                html = new HtmlContent();
                html.ModuleId = m.ModuleId;
                html.ModuleGuid = m.ModuleGuid;
            }

            bool titleChanged = (m.ModuleTitle != mwaPage.title);
            // updated 2011-11-30 don't change page name for existing pages
            // only update the module title
            // avoid changing the home page name
            // while most content pages should have the module title the same as the page name for best seo
            // this is usually not the case for the home page, ie the content is not titled "home"
            //if (!IsHomePage(page.PageId))
            //{
            //    page.PageName = mwaPage.title;
            //}

           
            if (titleChanged)
            {
                m.ModuleTitle = mwaPage.title;
                m.Save();

            }
            
            html.Body = mwaPage.description;
            if ((mwaPage.mt_keywords != null) &&(mwaPage.mt_keywords.Length > 0))
            {
                page.PageMetaKeyWords = mwaPage.mt_keywords;
            }

            bool needToClearSiteMapCache = false;
            bool pageOrderChanged = false;
            if ((mwaPage.pageOrder != null)&&(mwaPage.pageOrder.Length > 0))
            {
                int newPageOrder = Convert.ToInt32(mwaPage.pageOrder);
                if (page.PageOrder != newPageOrder)
                {
                    page.PageOrder = newPageOrder;
                    needToClearSiteMapCache = true;
                    pageOrderChanged = true;
                }
            }

            bool makeDraft = !publish;

            if (page.IsPending != makeDraft)
            {
                page.IsPending = makeDraft;
                needToClearSiteMapCache = true;
            }

           
            if ((mwaPage.pageParentID != null) &&(mwaPage.pageParentID.Length == 36))
            {
                Guid parentGuid = new Guid(mwaPage.pageParentID);

                if (page.PageGuid == parentGuid)
                {
                    throw new MetaWeblogException("11", MetaweblogResources.PageCantBeItsOwnParent);
                }

                // if parent pageid hasn't changed no need to doo anything
                if (parentGuid != page.ParentGuid)
                {
                    if (parentGuid == Guid.Empty)
                    {
                        if (UserCanCreateRootLevelPages())
                        {
                            page.ParentId = -1;
                            page.PageGuid = Guid.Empty;
                            needToClearSiteMapCache = true;
                        }
                        else
                        {
                            throw new MetaWeblogException("11", MetaweblogResources.NotAllowedToCreateRootPages);
                        }
                    }
                    else
                    {
                        parentPage = new PageSettings(parentGuid);

                        if (parentPage.SiteId != siteSettings.SiteId)
                        {
                            throw new MetaWeblogException("11", MetaweblogResources.ParentPageNotFound);
                        }

                        if (
                            (parentPage.PageId > -1)
                            && (parentPage.PageId != page.PageId) // page cannot be its own parent
                            && (parentPage.ParentId != page.PageId) // a child of the page cannot become its parent
                            )
                        {
                            // parent page exists

                            //verify user can create pages below parent
                            if (!UserCanCreateChildPages(parentPage))
                            {
                                throw new MetaWeblogException("11", MetaweblogResources.NotAllowedParentPage);
                            }

                            // descendant of page (grandchildren cannot become parent)
                            // so make sure selected parent is not a descendant of the current page
                            if (IsValidParentPage(page, parentPage))
                            {
                                page.ParentId = parentPage.PageId;
                                page.PageGuid = parentPage.PageGuid;
                                needToClearSiteMapCache = true;
                            }
                            else
                            {
                                throw new MetaWeblogException("11", MetaweblogResources.DescendentParentsNotAllowed);
                            }

                        }
                    }
                }
            }

            // keep verison history if enabled
            bool enableContentVersioning = config.EnableContentVersioning;

            if ((siteSettings.ForceContentVersioning) || (WebConfigSettings.EnforceContentVersioningGlobally))
            {
                enableContentVersioning = true;
            }

            if ((html.ItemId > -1)&&(enableContentVersioning))
            {
                html.CreateHistory(siteSettings.SiteGuid);
            }

            html.LastModUserGuid = siteUser.UserGuid; 
            html.LastModUtc = DateTime.UtcNow;
            page.LastModifiedUtc = DateTime.UtcNow;
            html.ModuleGuid = m.ModuleGuid; 
            
            html.ContentChanged += new ContentChangedEventHandler(html_ContentChanged);

            repository.Save(html);

            CacheHelper.ClearModuleCache(m.ModuleId);

            

            switch (mwaPage.commentPolicy)
            {
                //closed
                case "0":
                case "2":
                    page.EnableComments = false;
                    break;
                // open
                case "1":
                    // if the post was previously closed to comments
                    // re-open it using the default allowed days
                    page.EnableComments = true;

                    break;
                //else unspecified, no change
            }

            if (page.UrlHasBeenAdjustedForFolderSites)
            {
                page.Url = page.UnmodifiedUrl;
            }

            page.Save();

            if (pageOrderChanged)
            {
                if ((parentPage == null) && (page.ParentGuid != Guid.Empty))
                {
                    parentPage = new PageSettings(page.ParentGuid);
                }

            }

            if (parentPage != null)
            {
                parentPage.ResortPages();
            }

            if (needToClearSiteMapCache)
            {
                CacheHelper.ResetSiteMapCache(siteSettings.SiteId);
            }
            
            
            SiteUtils.QueueIndexing();

            return true;
        }



        /// <summary>
        /// wp.newPage method
        /// </summary>
        /// <param name="blogId">blogID in string format</param>
        /// <param name="userName">login username</param>
        /// <param name="password">login password</param>
        /// <param name="mwaPage">The mwa page.</param>
        /// <param name="publish">if set to <c>true</c> [publish].</param>
        /// <returns>The new page.</returns>
        internal string NewPage(string blogId, string userName, string password, MWAPage mwaPage, bool publish)
        {
            PageSettings page = new PageSettings();
            PageSettings parentPage = null;

            
            Guid parentGuid = Guid.Empty;
            if ((mwaPage.pageParentID != null)&&(mwaPage.pageParentID.Length == 36))
            {
                parentGuid = new Guid(mwaPage.pageParentID);
            }

            if (parentGuid == Guid.Empty) //root level page
            {
                if (!UserCanCreateRootLevelPages())
                {
                    throw new MetaWeblogException("11", MetaweblogResources.NotAllowedToCreateRootPages);
                }

                // TODO: promote these to site settings?
                //page.AuthorizedRoles = WebConfigSettings.DefaultPageRoles;
                //page.EditRoles = WebConfigSettings.DefaultRootPageEditRoles;
                //page.CreateChildPageRoles = WebConfigSettings.DefaultRootPageCreateChildPageRoles;

                page.AuthorizedRoles = siteSettings.DefaultRootPageViewRoles;
                page.EditRoles = siteSettings.DefaultRootPageEditRoles;
                page.CreateChildPageRoles = siteSettings.DefaultRootPageCreateChildPageRoles;
            }
            else
            {
                parentPage = new PageSettings(parentGuid);

                if (parentPage.PageId == -1)
                {
                    throw new MetaWeblogException("11", MetaweblogResources.ParentPageNotFound);
                }

                if (parentPage.SiteId != siteSettings.SiteId)
                {
                    throw new MetaWeblogException("11", MetaweblogResources.ParentPageNotFound);
                }

                if (!UserCanCreateChildPages(parentPage))
                {
                    throw new MetaWeblogException("11", MetaweblogResources.NotAllowedParentPage);
                }
            }

            if (parentPage != null)
            {
                page.ParentId = parentPage.PageId;
                page.ParentGuid = parentPage.PageGuid;
                page.PageOrder = PageSettings.GetNextPageOrder(siteSettings.SiteId, parentPage.PageId);

                // by default inherit settings from parent
                page.AuthorizedRoles = parentPage.AuthorizedRoles;
                page.EditRoles = parentPage.EditRoles;
                page.DraftEditOnlyRoles = parentPage.DraftEditOnlyRoles;
                page.CreateChildPageRoles = parentPage.CreateChildPageRoles;
                page.CreateChildDraftRoles = parentPage.CreateChildDraftRoles;
            }

            if ((mwaPage.pageOrder != null) && (mwaPage.pageOrder.Length > 0))
            {
                 page.PageOrder = Convert.ToInt32(mwaPage.pageOrder);
            }

            page.SiteId = siteSettings.SiteId;
            page.SiteGuid = siteSettings.SiteGuid;
            page.IsPending = !publish;

            page.PageName = mwaPage.title;
            //page.PageTitle = mwaPage.title; // this was the override page title it should not be set here
            if ((mwaPage.mt_keywords != null) && (mwaPage.mt_keywords.Length > 0))
            {
                page.PageMetaKeyWords = mwaPage.mt_keywords;
            }

            if (WebConfigSettings.AutoGeneratePageMetaDescriptionForMetaweblogNewPages)
            {
                page.PageMetaDescription = UIHelper.CreateExcerpt(mwaPage.description, WebConfigSettings.MetaweblogGeneratedMetaDescriptionMaxLength);
            }

            //if (WebConfigSettings.ShowUseUrlSettingInPageSettings)
            //{

            //}

            string friendlyUrlString = SiteUtils.SuggestFriendlyUrl(page.PageName, siteSettings);
            page.Url = "~/" + friendlyUrlString;
            page.UseUrl = true;

            switch (mwaPage.commentPolicy)
            {
                // open
                case "1":
                    // if the post was previously closed to comments
                    // re-open it using the default allowed days
                    page.EnableComments = true;

                    break;

                //closed
                case "0":
                case "2":
                default:
                    page.EnableComments = false;
                    break;
                
            }

            
            // I'm not sure we should support the page created event handler here, people may do redirects there
            // that would interupt our next steps
            // maybe need a config setting to decide

            // page.PageCreated += new PageCreatedEventHandler(PageCreated);

            page.Save();

            FriendlyUrl newFriendlyUrl = new FriendlyUrl();
            newFriendlyUrl.SiteId = siteSettings.SiteId;
            newFriendlyUrl.SiteGuid = siteSettings.SiteGuid;
            newFriendlyUrl.PageGuid = page.PageGuid;
            newFriendlyUrl.Url = friendlyUrlString;
            newFriendlyUrl.RealUrl = "~/Default.aspx?pageid=" + page.PageId.ToInvariantString();
            newFriendlyUrl.Save();
            

            // create html module in center pane
            ModuleDefinition moduleDefinition = new ModuleDefinition(HtmlContent.FeatureGuid);
            Module m = new Module();
            m.SiteId = siteSettings.SiteId;
            m.SiteGuid = siteSettings.SiteGuid;
            m.ModuleDefId = moduleDefinition.ModuleDefId;
            m.FeatureGuid = moduleDefinition.FeatureGuid;
            m.Icon = moduleDefinition.Icon;
            m.CacheTime = moduleDefinition.DefaultCacheTime;
            m.PageId = page.PageId;
            m.ModuleTitle = page.PageTitle;
            m.PaneName = "contentpane";
            m.CreatedByUserId = siteUser.UserId;
            m.ShowTitle = WebConfigSettings.ShowModuleTitlesByDefault;
            m.HeadElement = WebConfigSettings.ModuleTitleTag;
            m.ModuleOrder = 1;
            m.Save();

            HtmlRepository repository = new HtmlRepository();
            HtmlContent html = new HtmlContent();
            html.ModuleId = m.ModuleId;
            html.ModuleGuid = m.ModuleGuid;
            html.Body = mwaPage.description;
            //html.CreatedBy = siteUser.UserId;
            html.UserGuid = siteUser.UserGuid;
            html.CreatedDate = DateTime.UtcNow;
            html.LastModUserGuid = siteUser.UserGuid;
            html.LastModUtc = DateTime.UtcNow;
            
            html.ContentChanged += new ContentChangedEventHandler(html_ContentChanged);

            repository.Save(html);

            mojoPortal.SearchIndex.IndexHelper.RebuildPageIndexAsync(page);
            SiteUtils.QueueIndexing();

            CacheHelper.ResetSiteMapCache(siteSettings.SiteId);

            return page.PageGuid.ToString();
        }


        /// <summary>
        /// Deletes the page.
        /// </summary>
        /// <param name="blogId">
        /// The blog id.
        /// </param>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The delete page.
        /// </returns>
        /// <exception cref="MetaWeblogException">
        /// </exception>
        internal bool DeletePage(string blogId, string pageId, string userName, string password)
        {
            if ((string.IsNullOrEmpty(pageId)) || (pageId.Length != 36))
            {
                throw new MetaWeblogException("11", MetaweblogResources.PageNotFound);
            }

            PageSettings page = CacheHelper.GetPage(new Guid(pageId));

            if (page.PageId == -1)
            { //doesn't exist
                throw new MetaWeblogException("11", MetaweblogResources.PageNotFound);
            }

            if (page.SiteId != siteSettings.SiteId)
            { //doesn't exist
                throw new MetaWeblogException("11", MetaweblogResources.PageNotFound);
            }

            if (!UserCanEdit(page.EditRoles, string.Empty, -1))
            {
                throw new MetaWeblogException("11", MetaweblogResources.AccessDenied);
            }

            Module.DeletePageModules(page.PageId);
            PageSettings.DeletePage(page.PageId);
            FriendlyUrl.DeleteByPageGuid(page.PageGuid);

            mojoPortal.SearchIndex.IndexHelper.ClearPageIndexAsync(page);

            CacheHelper.ResetSiteMapCache();

            if (WebConfigSettings.LogIpAddressForContentDeletions)
            {
               
                log.Info("user " + siteUser.Name + " deleted page via metaweblogapi " + page.PageName + " from ip address " + SiteUtils.GetIP4Address());

            }

            return true;
        }

        #region helper methods

        private IFileSystem GetFileSystem()
        {
            FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
            if (p == null)
            {
                log.Error("Could not load file system provider " + WebConfigSettings.FileSystemProvider);
                return null;
            }

            IFileSystem fileSystem = p.GetFileSystem(GetFileSystemPermission());
            if (fileSystem == null)
            {
                log.Error("Could not load file system from provider " + WebConfigSettings.FileSystemProvider);
                return null;
            }

            return fileSystem;

        }

        private IFileSystemPermission GetFileSystemPermission()
        {
            long bytesPerMegabyte = 1048576;
            FileSystemPermission permission = new FileSystemPermission();

            permission.UserHasUploadPermission = false;

            if (siteUser.IsInRoles("Admins;Content Administrators"))
            {
                permission.VirtualRoot = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/wlw/";
                permission.AllowedExtensions = WebConfigSettings.AllowedUploadFileExtensions.SplitOnChar('|');
                permission.UserHasUploadPermission = true;
                permission.Quota = WebConfigSettings.AdminDiskQuotaInMegaBytes * bytesPerMegabyte;
                permission.MaxFiles = WebConfigSettings.AdminMaxNumberOfFiles;
                permission.MaxFolders = WebConfigSettings.AdminMaxNumberOfFolders;
            }
            else if (siteUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                permission.VirtualRoot = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/wlw/";
                permission.AllowedExtensions = WebConfigSettings.AllowedUploadFileExtensions.SplitOnChar('|');
                permission.UserHasUploadPermission = true;
                permission.Quota = WebConfigSettings.MediaFolderMaxSizePerFileInMegaBytes * bytesPerMegabyte;
                permission.MaxFiles = WebConfigSettings.MediaFolderMaxNumberOfFiles;
                permission.MaxFolders = WebConfigSettings.MediaFolderMaxNumberOfFolders;
            }
            else if (siteUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                permission.VirtualRoot = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/userfiles/" + siteUser.UserId.ToInvariantString() + "/";
                permission.AllowedExtensions = WebConfigSettings.AllowedLessPriveledgedUserUploadFileExtensions.SplitOnChar('|');
                permission.UserHasUploadPermission = true;
                permission.Quota = WebConfigSettings.UserFolderMaxSizePerFileInMegaBytes * bytesPerMegabyte;
                permission.MaxFiles = WebConfigSettings.UserFolderMaxNumberOfFiles;
                permission.MaxFolders = WebConfigSettings.UserFolderMaxNumberOfFolders;
            }


            return permission;
        }



        private string GetUserUploadVirtualPath()
        {
            string virtualPath = string.Empty;

            if (siteUser.IsInRoles("Admins;Content Administrators"))
            {
                virtualPath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/wlw/";
                allowedExtensions = WebConfigSettings.AllowedUploadFileExtensions;
            }
            else if (siteUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                virtualPath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/wlw/";
                allowedExtensions = WebConfigSettings.AllowedUploadFileExtensions;
            }
            else if (siteUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                virtualPath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/userfiles/" + siteUser.UserId.ToInvariantString() + "/";
            }

            return virtualPath;
        }


        private SiteUser GetSiteUser(string loginName)
        {
            siteUser = new SiteUser(siteSettings, loginName);
            if (siteUser.UserId == -1) { return null; }

            if (siteUser.IsInRoles("Admins"))
            {
                isAdmin = true;
            }

            if (!isAdmin)
            {
                if (siteUser.IsInRoles("Content Administrators"))
                {
                    isContentAdmin = true;
                }
            }

            if ((!isAdmin) && (!isContentAdmin))
            {
                if (WebConfigSettings.UseRelatedSiteMode)
                {
                    isSiteEditor = siteUser.IsInRoles(siteSettings.SiteRootEditRoles);
                }
            }

            if (siteUser.TimeZoneId.Length > 0)
            {
                timeZone = SiteUtils.GetTimeZone(siteUser.TimeZoneId);
            }
            else
            {
                timeZone = SiteUtils.GetTimeZone(siteSettings.TimeZoneId);
            }


            return siteUser;
        }

        private bool UserCanEdit(string pageEditRoles, string moduleEditRoles, int editUserId)
        {
            if (isAdmin) { return true; }

            if (pageEditRoles == "Admins") { return false; }

            if (isContentAdmin || isSiteEditor) { return true; }

            if (moduleEditRoles == "Admins") { return false; }

            if (siteUser.UserId == editUserId) { return true; }

            if (siteUser.IsInRoles(moduleEditRoles))
            {
                return true;
            }

            if (siteUser.IsInRoles(pageEditRoles))
            {
                return true;
            }

            return false;

        }

        private bool UserCanPostToBlog(string loginName, int moduleId)
        {

            if (isAdmin) { return true; }

            Module module = new Module(moduleId);
            if (module.ModuleId == -1) { return false; } //invalid module
            if (module.FeatureGuid != Blog.FeatureGuid) { return false; }


            if (module.EditUserId.Equals(siteUser.UserId))
            {
                return true;
            }

            

            int pageId = GetPageIdForModule(moduleId);
            if (pageId > -1)
            {
                PageSettings blogPage = new PageSettings(siteSettings.SiteId, pageId);
                if (blogPage.EditRoles == "Admins") { return false; }

                if (isContentAdmin || isSiteEditor) { return true; }

                if (module.AuthorizedEditRoles == "Admins") { return false; }

                if (siteUser.IsInRoles(blogPage.EditRoles)) { return true; }

                if (siteUser.IsInRoles(module.AuthorizedEditRoles))
                {
                    return true;
                }

                
            }

            return false;
        }

        private bool UserCanEditPost(string loginName, int postId)
        {
            Blog blog = new Blog(postId);
            if (blog.ItemId == -1) { return false; } //not found

            Module module = new Module(blog.ModuleId);
            if (module.ModuleId == -1) { return false; }
            if (module.FeatureGuid != Blog.FeatureGuid) { return false; }

            if (isAdmin) { return true; }

            if (module.EditUserId.Equals(siteUser.UserId)) { return true; }

            int pageId = GetPageIdForModule(blog.ModuleId);

            if (pageId > -1)
            {
                PageSettings blogPage = new PageSettings(siteSettings.SiteId, pageId);

                if (blogPage.EditRoles == "Admins") { return false; }

                if (isContentAdmin || isSiteEditor) { return true; }

                if (module.AuthorizedEditRoles == "Admins") { return false; }

                if (siteUser.IsInRoles(blogPage.EditRoles)) { return true; }

                if (siteUser.IsInRoles(module.AuthorizedEditRoles))
                {
                    return true;
                }

            }

            return false;
        }

        private bool UserCanEdit(MWAPage page)
        {
            if (isAdmin) { return true; }

            if (page.pageEditRoles == "Admins") { return false; }

            if (isContentAdmin || isSiteEditor) { return true; }

            if (page.moduleEditRoles == "Admins") { return false; }

            if ((siteUser.IsInRoles(page.pageEditRoles)) || (siteUser.IsInRoles(page.moduleEditRoles)))
            {
                return true;
            }

            return false;
        }

        private bool UserCanEdit(PageSettings page, Module module)
        {
            if (isAdmin) { return true; }

            if (page.EditRoles == "Admins") { return false; }

            if (isContentAdmin || isSiteEditor) { return true; }

            if (module.AuthorizedEditRoles == "Admins") { return false; }

            if ((siteUser.IsInRoles(page.EditRoles)) || (siteUser.IsInRoles(module.AuthorizedEditRoles)))
            {
                return true;
            }

            return false;
        }

        private bool UserCanView(string viewRoles)
        {
            if (isAdmin) { return true; }

            if (viewRoles.Contains("All Users")) { return true; }

            if (viewRoles == "Admins") { return false; }

            if (isContentAdmin || isSiteEditor) { return true; }

            if (siteUser.IsInRoles(viewRoles))
            {
                return true;
            }

            return false;
        }

        private bool UserCanCreateChildPages(PageSettings page)
        {
            if (isAdmin) { return true; }

            if (page.CreateChildPageRoles == "Admins") { return false; }

            if (isContentAdmin || isSiteEditor) { return true; }

            if (siteUser.IsInRoles(page.CreateChildPageRoles))
            {
                return true;
            }

            return false;
        }

        private bool UserCanCreateRootLevelPages()
        {
            if (isAdmin) { return true; }
            if (siteSettings.RolesThatCanCreateRootPages == "Admins") { return false; }
            if (isContentAdmin || isSiteEditor) { return true; }
            if (siteUser.IsInRoles(siteSettings.RolesThatCanCreateRootPages)) { return true; }

            return false;
        }

        private int GetPageIdForModule(int moduleId)
        {
            DataTable modulePages = Module.GetPageModulesTable(moduleId);
            int pageId = -1;
            if (modulePages.Rows.Count > 0)
            {
                pageId = Convert.ToInt32(modulePages.Rows[0]["PageId"]);
            }

            return pageId;
        }

        private void SetCategories(Blog post, MWAPost sentPost)
        {
            List<MWACategory> allBlogCategories = GetCategoriesForBlog(post.ModuleId);
            List<string> postCategories = GetCategoriesForPost(post.ItemId);

            foreach (string category in sentPost.categories)
            {
                if (category.Trim().Length == 0) { continue; }
                
                MWACategory categoryInfo = allBlogCategories.Find(delegate(MWACategory ci)
                { return ci.title.Equals(category, StringComparison.OrdinalIgnoreCase); });
                if ((categoryInfo.id != null) && (categoryInfo.id.Length > 0))
                {
                    if (!postCategories.Contains(category))
                    {
                        Blog.AddItemCategory(post.ItemId, Convert.ToInt32(categoryInfo.id));
                    }
                }
                else
                {
                    int newCategoryId = Blog.AddBlogCategory(post.ModuleId, category);
                    Blog.AddItemCategory(post.ItemId, newCategoryId);
                }

            }
        }

        private List<MWACategory> GetCategoriesForBlog(int moduleId)
        {
            List<MWACategory> categories = new List<MWACategory>();

            int pageId = GetPageIdForModule(moduleId);

            using (IDataReader rdr = Blog.GetCategoriesList(moduleId))
            {
                while (rdr.Read())
                {
                    MWACategory category = new MWACategory();
                    category.id = Convert.ToInt32(rdr["CategoryID"]).ToInvariantString();
                    category.parentId = "-1";
                    category.title = rdr["Category"].ToString();
                    category.description = rdr["Category"].ToString();

                    category.htmlUrl = navigationSiteRoot + "/Blog/ViewCategory.aspx?cat=" 
                        + Convert.ToInt32(rdr["CategoryID"]).ToInvariantString() 
                        + "&mid=" + moduleId.ToInvariantString()
                        +"&pageid=" + pageId.ToInvariantString();

                    category.rssUrl = navigationSiteRoot + "/Blog/RSS.aspx?cat="
                        + Convert.ToInt32(rdr["CategoryID"]).ToInvariantString()
                        + "&mid=" + moduleId.ToInvariantString()
                        + "&pageid=" + pageId.ToInvariantString();

                    categories.Add(category);
                }
            }
            categories.Sort((a, b) => a.title.CompareTo(b.title));
            return categories;
        }

        private List<string> GetCategoriesForPost(int postId)
        {
            List<string> categories = new List<string>();
            using (IDataReader rdr = Blog.GetItemCategories(postId))
            {
                while (rdr.Read())
                {
                    categories.Add(rdr["Category"].ToString());
                }
            }
            categories.Sort();
            return categories;
        }

        private void blog_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["BlogIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }

        private void PageCreated(object sender, PageCreatedEventArgs e)
        {
            // this is a hook so that custom code can be fired when pages are created
            // implement a PageCreatedEventHandlerPovider and put a config file for it in
            // /Setup/ProviderConfig/pagecreatedeventhandlers
            try
            {
                foreach (PageCreatedEventHandlerPovider handler in PageCreatedEventHandlerPoviderManager.Providers)
                {
                    handler.PageCreatedHandler(sender, e);
                }
            }
            catch (TypeInitializationException ex)
            {
                log.Error(ex);
            }
        }

        private void html_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["HtmlContentIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }

        private string FormatUrl(string itemUrl, int itemId, int moduleId)
        {
            string url = itemUrl.Replace("~", string.Empty);
            if (url.Length > 0)
            {
                //url = siteSettings.SiteRoot + url;
                url = navigationSiteRoot + url;
            }
            else
            {
                //url = siteSettings.SiteRoot + String.Format(CultureInfo.InvariantCulture, "/Blog/ViewPost.aspx?ItemID={0}&mid={1}", itemId, moduleId);
                url = navigationSiteRoot + String.Format(CultureInfo.InvariantCulture, "/Blog/ViewPost.aspx?ItemID={0}&mid={1}", itemId, moduleId);
            }

            return url;
        }

        private string FormatPageUrl(int pageId, string pageUrl, bool useUrl)
        {
            string url = pageUrl.Replace("~", string.Empty);

            if ((url.Length > 0) && (useUrl))
            {
                url = navigationSiteRoot + url;
            }
            else
            {
                url = navigationSiteRoot + String.Format(CultureInfo.InvariantCulture, "/Default.aspx?pageid={0}", pageId);
            }

            return url;
        }



        private Module GetFirstCenterPaneHtmlModule(PageSettings page)
        {
            foreach (Module m in page.Modules)
            {
                if (m.FeatureGuid == HtmlContent.FeatureGuid)
                {
                    if (m.PaneName.ToLower() == "contentpane")
                    {
                        return m;
                    }
                }
            }

            return null;
        }

        private bool IsValidParentPage(PageSettings page, PageSettings requestedParent)
        {
            
            // new parent cannot be a descendant of page

            SiteMapDataSource siteMapDataSource = new SiteMapDataSource();
            siteMapDataSource.SiteMapProvider = "mojosite" + siteSettings.SiteId.ToInvariantString();

            SiteMapNode rootNode = siteMapDataSource.Provider.RootNode;

            mojoSiteMapNode currentPageNode = SiteUtils.GetSiteMapNodeForPage(rootNode, page);

            if (currentPageNode != null)
            {
                SiteMapNodeCollection descendantNodes = currentPageNode.GetAllNodes();
                foreach (SiteMapNode node in descendantNodes)
                {
                    mojoSiteMapNode mojoNode = node as mojoSiteMapNode;
                    if (mojoNode.PageId == requestedParent.PageId) { return false; }
                }
            }
            else
            {
                throw new MetaWeblogException("11", "Could not validate parent page from site map.");
            }

            return true;
        }

        private bool IsHomePage(int pageId)
        {

            SiteMapDataSource siteMapDataSource = new SiteMapDataSource();
            siteMapDataSource.SiteMapProvider = "mojosite" + siteSettings.SiteId.ToInvariantString();

            SiteMapNode rootNode = siteMapDataSource.Provider.RootNode;
            //first child node of root is home page
            mojoSiteMapNode homePage = rootNode.ChildNodes[0] as mojoSiteMapNode;

            if ((homePage != null) && (homePage.PageId == pageId)) { return true; }
            

            return false;
        }

        private bool Contains(List<MWAPage> pageList, string pageId)
        {
            foreach (MWAPage p in pageList)
            {
                if (p.pageID == pageId) { return true; }
            }

            return false;
        }

        #endregion

        ///// <summary>
        ///// Gets authors.
        ///// </summary>
        ///// <param name="blogId">
        ///// The blog id.
        ///// </param>
        ///// <param name="userName">
        ///// The user name.
        ///// </param>
        ///// <param name="password">
        ///// The password.
        ///// </param>
        ///// <returns>
        ///// A list of authors.
        ///// </returns>
        //internal List<MWAAuthor> GetAuthors(string blogId, string userName, string password)
        //{
        //    var authors = new List<MWAAuthor>();

        //    if (Security.IsAuthorizedTo(Rights.EditOtherUsers))
        //    {
        //        int total;

        //        var users = Membership.Provider.GetAllUsers(0, 999, out total);

        //        authors.AddRange(
        //            users.Cast<MembershipUser>().Select(
        //                user =>
        //                new MWAAuthor
        //                {
        //                    user_id = user.UserName,
        //                    user_login = user.UserName,
        //                    display_name = user.UserName,
        //                    user_email = user.Email,
        //                    meta_value = string.Empty
        //                }));
        //    }
        //    else
        //    {
        //        // If not able to administer others, just add that user to the options.
        //        var single = Membership.GetUser(userName);
        //        if (single != null)
        //        {
        //            var temp = new MWAAuthor
        //            {
        //                user_id = single.UserName,
        //                user_login = single.UserName,
        //                display_name = single.UserName,
        //                user_email = single.Email,
        //                meta_value = string.Empty
        //            };
        //            authors.Add(temp);
        //        }
        //    }

        //    return authors;
        //}


        ///// <summary>
        ///// wp.getTags method
        ///// </summary>
        ///// <param name="blogId">The blog id.</param>
        ///// <param name="userName">Name of the user.</param>
        ///// <param name="password">The password.</param>
        ///// <returns>list of tags</returns>
        //internal List<string> GetKeywords(string blogId, string userName, string password)
        //{
        //    var keywords = new List<string>();

        //    if (!Security.IsAuthorizedTo(Rights.CreateNewPosts))
        //        return keywords;

        //    foreach (var tag in
        //        Post.Posts.Where(post => post.IsVisible).SelectMany(post => post.Tags.Where(tag => !keywords.Contains(tag))))
        //    {
        //        keywords.Add(tag);
        //    }

        //    keywords.Sort();

        //    return keywords;
        //}

        /// <summary>
        /// Returns Category Guid from Category name.
        /// </summary>
        /// <remarks>
        /// Reverse dictionary lookups are ugly.
        /// </remarks>
        /// <param name="name">
        /// The category name.
        /// </param>
        /// <param name="cat">
        /// The category.
        /// </param>
        /// <returns>
        /// Whether the category was found or not.
        /// </returns>
        //private static bool LookupCategoryGuidByName(string name, out Category cat)
        //{
        //    cat = new Category();
        //    foreach (var item in Category.Categories.Where(item => item.Title == name))
        //    {
        //        cat = item;
        //        return true;
        //    }

        //    return false;
        //}

        #endregion

        /// <summary>
        ///     Gets a value indicating whether another request can use the <see cref = "T:System.Web.IHttpHandler"></see> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref = "T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}