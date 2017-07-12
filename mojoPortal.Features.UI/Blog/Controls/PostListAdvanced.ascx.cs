//	Author:				Joe Audette
//	Created:			2004-08-15
//	Last Modified:		2017-05-11
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
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using Newtonsoft.Json.Linq;
using System.IO;
using mojoPortal.Web;
using mojoPortal.Web.UI;
using mojoPortal.Web.BlogUI;
using mojoPortal.BlogUI;
using mojoPortal.Features.Business;
using System.Linq;
using System.Text;
using mojoPortal.Web.Controllers;
using mojoPortal.Web.Components;

namespace mojoPortal.Web.BlogUI
{
    public partial class PostListAdvanced : UserControl
    {
        #region Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(PostListAdvanced));

        //private int countOfDrafts = 0;
        private int pageNumber = 1;
        private int totalPages = 1;
        protected string EditContentImage = WebConfigSettings.EditContentImage;
        protected string EditBlogAltText = "Edit";
        protected Double TimeOffset = 0;
        private TimeZoneInfo timeZone = null;
        protected DateTime CalendarDate;
        protected string addThisAccountId = string.Empty;
        protected bool ShowGoogleMap = true;
        protected string addThisCustomBrand = string.Empty;
        protected string FeedBackLabel = string.Empty;
        protected string GmapApiKey = string.Empty;
        protected bool EnableContentRating = false;
        private string DisqusSiteShortName = string.Empty;
        protected string disqusFlag = string.Empty;
        protected string IntenseDebateAccountId = string.Empty;
        protected bool ShowCommentCounts = true;
        protected string EditLinkText = BlogResources.BlogEditEntryLink;
        protected string EditLinkTooltip = BlogResources.BlogEditEntryLink;
        protected string EditLinkImageUrl = string.Empty;
        private mojoBasePage basePage = null;
        private Module module = null;
        protected BlogConfiguration blogConfig = new BlogConfiguration();
        protected BlogPostListAdvancedConfiguration config = new BlogPostListAdvancedConfiguration();
        private bool useFriendlyUrls = true;
        private int pageId = -1;
        private int moduleId = -1;
        private int categoryId = -1;
        private bool isEditable = false;
        private string siteRoot = string.Empty;
        private string imageSiteRoot = string.Empty;
        private SiteSettings siteSettings = null;
        private int pageSize = 10;
        private DataSet dsBlogPosts = null;
        protected string itemHeadingElement = "h3";
        protected string CategoriesResourceKey = "PostCategories";
        protected int Month = DateTime.UtcNow.Month;
        protected int Year = DateTime.UtcNow.Year;
        protected bool TitleOnly = false;
        protected bool ShowTweetThisLink = false;
        protected bool ShowPlusOneButton = false;
        protected bool UseFacebookLikeButton = false;
        protected bool AllowComments = false;
        protected bool useExcerpt = false;
        protected string attachmentBaseUrl = string.Empty;

        protected bool allowGravatars = false;
        protected bool disableAvatars = true;
        protected Avatar.RatingType MaxAllowedGravatarRating = Avatar.RatingType.PG;
        protected string UserNameTooltipFormat = "View User Profile for {0}";
        private string template = string.Empty;
        private string repeatingTemplate = string.Empty;
        private SiteUser currentUser = null;

        private const string token_RepeaterStart = "$startrepeater$";
        private const string token_RepeaterEnd = "$endrepeater$";

        private int siteId = -1;
        public int SiteId
        {
            get { return siteId; }

        }

        public int PageId
        {
            get { return pageId; }
            set { pageId = value; }
        }

        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        public string SiteRoot
        {
            get { return siteRoot; }
            set { siteRoot = value; }
        }

        public string ImageSiteRoot
        {
            get { return imageSiteRoot; }
            set { imageSiteRoot = value; }
        }

        public BlogConfiguration BlogConfig
        {
            get { return blogConfig; }
            set { blogConfig = value; }
        }

        public BlogPostListAdvancedConfiguration Config
        {
            get { return config; }
            set { config = value; }
        }

        public bool IsEditable
        {
            get { return isEditable; }
            set { isEditable = value; }
        }

        private string displayMode = "DescendingByDate";

        public string DisplayMode
        {
            get { return displayMode; }
            set { displayMode = value; }
        }

        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.EnableViewState = false;
            //rptBlogs.ItemDataBound += new RepeaterItemEventHandler(rptBlogs_ItemDataBound);

        }




        protected virtual void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();

            if (module == null)
            {
                Visible = false;
                return;
            }


            if (!Page.IsPostBack)
            {
                PopulateControls();

            }

        }
        protected virtual void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            siteId = siteSettings.SiteId;
            currentUser = SiteUtils.GetCurrentSiteUser();
            TimeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            GmapApiKey = SiteUtils.GetGmapApiKey();
            addThisAccountId = siteSettings.AddThisDotComUsername;

            if (blogConfig.AddThisAccountId.Length > 0)
            {
                addThisAccountId = blogConfig.AddThisAccountId;
            }

            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
            categoryId = WebUtils.ParseInt32FromQueryString("cat", categoryId);
            Month = WebUtils.ParseInt32FromQueryString("month", Month);
            Year = WebUtils.ParseInt32FromQueryString("year", Year);
            attachmentBaseUrl = SiteUtils.GetFileAttachmentUploadPath();

            if (Page is mojoBasePage)
            {
                basePage = Page as mojoBasePage;
                module = basePage.GetModule(moduleId, config.FeatureGuid);
            }

            if (module == null) { return; }

            MaxAllowedGravatarRating = SiteUtils.GetMaxAllowedGravatarRating();
            //UserNameTooltipFormat = displaySettings.AvatarUserNameTooltipFormat;

            switch (siteSettings.AvatarSystem)
            {
                case "gravatar":
                    allowGravatars = true;
                    disableAvatars = false;
                    break;

                case "internal":
                    allowGravatars = false;
                    disableAvatars = false;
                    break;

                case "none":
                default:
                    allowGravatars = false;
                    disableAvatars = true;
                    break;

            }

            //if (!config.ShowAuthorAvatar) { disableAvatars = true; }

            //if (blogConfig.UseExcerpt && !displaySettings.ShowAvatarWithExcerpt) { disableAvatars = true; }

            CalendarDate = WebUtils.ParseDateFromQueryString("blogdate", DateTime.UtcNow).Date;

            if (CalendarDate > DateTime.UtcNow.Date)
            {
                CalendarDate = DateTime.UtcNow.Date;
            }

            if ((blogConfig.UseExcerpt) && (!blogConfig.GoogleMapIncludeWithExcerpt)) { ShowGoogleMap = false; }

            //EnableContentRating = blogConfig.EnableContentRating && !displaySettings.PostListDisableContentRating;
            if (blogConfig.UseExcerpt) { EnableContentRating = false; }



            //if (config.AddThisCustomBrand.Length > 0)
            //{
            //    addThisCustomBrand = config.AddThisCustomBrand;
            //}



            if (blogConfig.DisqusSiteShortName.Length > 0)
            {
                DisqusSiteShortName = blogConfig.DisqusSiteShortName;
            }
            else
            {
                DisqusSiteShortName = siteSettings.DisqusSiteShortName;
            }

            if (blogConfig.IntenseDebateAccountId.Length > 0)
            {
                IntenseDebateAccountId = blogConfig.IntenseDebateAccountId;
            }
            else
            {
                IntenseDebateAccountId = siteSettings.IntenseDebateAccountId;
            }

            //navTop.ModuleId = ModuleId;
            //navTop.ModuleGuid = module.ModuleGuid;
            //navTop.PageId = PageId;
            //navTop.IsEditable = IsEditable;
            //navTop.Config = config;
            //navTop.SiteRoot = SiteRoot;
            //navTop.ImageSiteRoot = ImageSiteRoot;

            //navBottom.ModuleId = ModuleId;
            //navBottom.ModuleGuid = module.ModuleGuid;
            //navBottom.PageId = PageId;
            //navBottom.IsEditable = IsEditable;
            //navBottom.Config = config;
            //navBottom.SiteRoot = SiteRoot;
            //navBottom.ImageSiteRoot = ImageSiteRoot;

            //TitleOnly = blogConfig.TitleOnly || displaySettings.PostListForceTitleOnly;
            ShowTweetThisLink = blogConfig.ShowTweetThisLink && !blogConfig.UseExcerpt;
            ShowPlusOneButton = blogConfig.ShowPlusOneButton && !blogConfig.UseExcerpt;
            UseFacebookLikeButton = blogConfig.UseFacebookLikeButton && !blogConfig.UseExcerpt;
            //useExcerpt = blogConfig.UseExcerpt || displaySettings.PostListForceExcerptMode;
            pageSize = blogConfig.PageSize;
            //AllowComments = Config.AllowComments && ShowCommentCounts;

            //TODO: implement displaymode
            //switch (displayMode)
            //{
            //    case "ByCategory":

            //        if (displaySettings.CategoryListForceTitleOnly)
            //        {
            //            TitleOnly = true;
            //        }

            //        if (displaySettings.CategoryListOverridePageSize > 0)
            //        {
            //            pageSize = displaySettings.CategoryListOverridePageSize;
            //        }

            //        if (displaySettings.ArchiveViewHideFeedbackLink)
            //        {
            //            AllowComments = false;
            //        }

            //        if (displaySettings.OverrideCategoryListItemHeadingElement.Length > 0)
            //        {
            //            itemHeadingElement = displaySettings.OverrideCategoryListItemHeadingElement;
            //        }

            //        break;

            //    case "ByMonth":

            //        if (displaySettings.ArchiveListForceTitleOnly)
            //        {
            //            TitleOnly = true;
            //        }

            //        if (displaySettings.ArchiveListOverridePageSize > 0)
            //        {
            //            pageSize = displaySettings.ArchiveListOverridePageSize;
            //        }

            //        if (displaySettings.OverrideArchiveListItemHeadingElement.Length > 0)
            //        {
            //            itemHeadingElement = displaySettings.OverrideArchiveListItemHeadingElement;
            //        }

            //        break;

            //    case "DescendingByDate":
            //    default:

            //        if (displaySettings.PostListOverridePageSize > 0)
            //        {
            //            pageSize = displaySettings.PostListOverridePageSize;
            //        }

            //        if (displaySettings.OverrideListItemHeadingElement.Length > 0)
            //        {
            //            itemHeadingElement = displaySettings.OverrideListItemHeadingElement;
            //        }

            //        break;

            //}




            //if (config.AllowComments)
            //{
            //    if ((DisqusSiteShortName.Length > 0) && (config.CommentSystem == "disqus"))
            //    {
            //        disqusFlag = "#disqus_thread";
            //        disqus.SiteShortName = DisqusSiteShortName;
            //        disqus.RenderCommentCountScript = true;
            //        navTop.ShowCommentCount = false;
            //        navBottom.ShowCommentCount = false;

            //    }

            //    if ((IntenseDebateAccountId.Length > 0) && (config.CommentSystem == "intensedebate"))
            //    {
            //        ShowCommentCounts = false;

            //        navTop.ShowCommentCount = false;
            //        navBottom.ShowCommentCount = false;
            //    }

            //    if (config.CommentSystem == "facebook")
            //    {
            //        ShowCommentCounts = false;
            //        navTop.ShowCommentCount = false;
            //        navBottom.ShowCommentCount = false;
            //    }

            //}
            //else
            //{
            //    navTop.ShowCommentCount = false;
            //    navBottom.ShowCommentCount = false;
            //}


            //if (config.Copyright.Length > 0)
            //{
            //    lblCopyright.Text = config.Copyright;
            //}



            //navTop.Visible = false;


            //if (IsEditable)
            //{
            //    countOfDrafts = Blog.CountOfDrafts(ModuleId);
            //}

            useFriendlyUrls = BlogConfiguration.UseFriendlyUrls(moduleId);
            if (!WebConfigSettings.UseUrlReWriting) { useFriendlyUrls = false; }








        }
        private void PopulateControls()
        {
            var dsBlogs = Blog.GetPageDataSet(config.BlogModuleId, DateTime.UtcNow, pageNumber, pageSize, out totalPages);

            StringBuilder posts = new StringBuilder();

            //List<Blog> blogs = new List<Blog>();
            List<BlogPostModel> models = new List<BlogPostModel>();

            foreach (DataRow postRow in dsBlogs.Tables["posts"].Rows)
            {
                BlogPostModel model = new BlogPostModel();

                if (useFriendlyUrls && (postRow["ItemUrl"].ToString().Length > 0))
                {
                    model.ItemUrl = postRow["ItemUrl"].ToString().Replace("~", string.Empty);
                }
                else
                {
                    model.ItemUrl = postRow["ItemID"].ToString() + "&mid=" + postRow["ModuleID"].ToString();
                }

                model.Title = postRow["Heading"].ToString();
                model.SubTitle = postRow["SubTitle"].ToString();
                model.Body = postRow["Description"].ToString();
                model.AuthorAvatar = postRow["AvatarUrl"].ToString();
                model.AuthorDisplayName = postRow["Name"].ToString();
                model.AuthorFirstName = postRow["FirstName"].ToString();
                model.AuthorLastName = postRow["LastName"].ToString();
                model.AuthorBio = postRow["AuthorBio"].ToString();
                model.Excerpt = postRow["Abstract"].ToString();
                model.PostDate = Convert.ToDateTime(postRow["StartDate"].ToString());
                model.HeadlineImageUrl = postRow["HeadlineImageUrl"].ToString();
                model.CommentCount = Convert.ToInt32(postRow["CommentCount"]);
                model.AllowCommentsForDays = Convert.ToInt32(postRow["AllowCommentsForDays"]);
                model.ShowAuthorName = Convert.ToBoolean(postRow["ShowAuthorName"]);
                model.ShowAuthorAvatar = Convert.ToBoolean(postRow["ShowAuthorAvatar"]);
                model.ShowAuthorBio = Convert.ToBoolean(postRow["ShowAuthorBio"]);
                models.Add(model);
            }

            Literal theLit = new Literal();

            try
            {
                theLit.Text = RazorBridge.RenderPartialToString(config.Layout, models, "Blog");
            }
            catch (System.Web.HttpException ex)
            {
                log.ErrorFormat("chosen layout ({0}) for _BlogPostList was not found in skin {1}. perhaps it is in a different skin. Error was: {2}", config.Layout, SiteUtils.GetSkinBaseUrl(true, this.Page), ex);
                theLit.Text = RazorBridge.RenderPartialToString("_BlogPostList", models, "Blog");
            }


            holder.Controls.Add(theLit);
        }


    }
}