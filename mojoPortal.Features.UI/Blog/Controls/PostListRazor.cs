using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.BlogUI
{
    [ToolboxData("<{0}:PostListRazor runat=server></{0}:PostListRazor>")]
    public class PostListRazor : WebControl
    {
        #region Properties
        private static readonly ILog log = LogManager.GetLogger(typeof(PostListRazor));
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


        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            LoadSettings();

            if (module == null)
            {
                Visible = false;
                return;
            }


            if (Page.IsPostBack) return;
            
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

            CalendarDate = WebUtils.ParseDateFromQueryString("blogdate", DateTime.UtcNow).Date;

            if (CalendarDate > DateTime.UtcNow.Date)
            {
                CalendarDate = DateTime.UtcNow.Date;
            }

            if ((blogConfig.UseExcerpt) && (!blogConfig.GoogleMapIncludeWithExcerpt)) { ShowGoogleMap = false; }

            if (blogConfig.UseExcerpt) { EnableContentRating = false; }

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


            ShowTweetThisLink = blogConfig.ShowTweetThisLink && !blogConfig.UseExcerpt;
            ShowPlusOneButton = blogConfig.ShowPlusOneButton && !blogConfig.UseExcerpt;
            UseFacebookLikeButton = blogConfig.UseFacebookLikeButton && !blogConfig.UseExcerpt;
            pageSize = blogConfig.PageSize;

            useFriendlyUrls = BlogConfiguration.UseFriendlyUrls(moduleId);
            if (!WebConfigSettings.UseUrlReWriting) { useFriendlyUrls = false; }
        }

        protected override void RenderContents(HtmlTextWriter output)
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
				model.AuthorUserId = Convert.ToInt32(postRow["UserID"]);

				models.Add(model);
            }
            string text = string.Empty;
            try
            {
                text = RazorBridge.RenderPartialToString(config.Layout, models, "Blog");
            }
            catch (System.Web.HttpException ex)
            {
                log.ErrorFormat("chosen layout ({0}) for _BlogPostList was not found in skin {1}. perhaps it is in a different skin. Error was: {2}", config.Layout, SiteUtils.GetSkinBaseUrl(true, this.Page), ex);
                text = RazorBridge.RenderPartialToString("_BlogPostList", models, "Blog");
            }

            output.Write(text);
        }
    }
}
