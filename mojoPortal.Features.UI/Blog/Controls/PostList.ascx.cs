//	Author:				
//	Created:			2004-08-15
//	Last Modified:		2015-08-18
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
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.BlogUI
{
    public partial class PostList : UserControl
    {
        #region Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(PostList));

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
        protected BlogConfiguration config = new BlogConfiguration();
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
        protected mojoPortal.Web.UI.Avatar.RatingType MaxAllowedGravatarRating = UI.Avatar.RatingType.PG;
        protected string UserNameTooltipFormat = "View User Profile for {0}";

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

        public BlogConfiguration Config
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
            rptBlogs.ItemDataBound += new RepeaterItemEventHandler(rptBlogs_ItemDataBound);

        }

        


        protected virtual void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();

            if (module == null)
            {
                Visible = false;
                return;
            }

            SetupRssLink();
          
            PopulateLabels();
            if (!Page.IsPostBack)
            {
                PopulateControls();
                
            }

        }

        private void PopulateControls()
        {
            BindBlogs();
            
        }

        private void BindBlogs()
        {
            string pageUrl;

            switch (displayMode)
            {
                case "ByCategory":

                    dsBlogPosts = Blog.GetBlogEntriesByCategory(
                        moduleId,
                        categoryId,
                        DateTime.UtcNow,
                        pageNumber,
                        pageSize,
                        out totalPages);

                    pageUrl = SiteRoot + "/Blog/ViewCategory.aspx?cat=" + categoryId.ToInvariantString()
                           + "&amp;pageid=" + pageId.ToInvariantString()
                           + "&amp;mid=" + moduleId.ToInvariantString()
                           + "&amp;pagenumber={0}";

                    break;

                case "ByMonth":
                    dsBlogPosts = Blog.GetBlogEntriesByMonth(
                        Month,
                        Year,
                        ModuleId,
                        DateTime.UtcNow,
                        pageNumber,
                        pageSize,
                        out totalPages);

                    pageUrl = SiteRoot + "/Blog/ViewArchive.aspx?month=" + Month.ToInvariantString()
                        + "&amp;year=" + Year.ToInvariantString()
                        + "&amp;pageid=" + pageId.ToInvariantString()
                        + "&amp;mid=" + moduleId.ToInvariantString()
                        + "&amp;pagenumber={0}";

                    break;

                case "DescendingByDate":
                default:

                    dsBlogPosts = Blog.GetPageDataSet(ModuleId, CalendarDate.Date.AddDays(1), pageNumber, pageSize, out totalPages);
                    pageUrl = SiteRoot + "/Blog/ViewList.aspx"
                           + "?pageid=" + pageId.ToInvariantString()
                           + "&amp;mid=" + moduleId.ToInvariantString()
                           + "&amp;pagenumber={0}";

                    break;

            }
            
           
            rptBlogs.DataSource = dsBlogPosts.Tables["Posts"];
            rptBlogs.DataBind();

            pgr.PageURLFormat = pageUrl;
            pgr.ShowFirstLast = true;
            pgr.PageSize = config.PageSize;
            pgr.PageCount = totalPages;
            pgr.CurrentIndex = pageNumber;
            pgr.Visible = (totalPages > 1) && config.ShowPager;
            

        }

        void rptBlogs_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (dsBlogPosts == null) { return; }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string ItemId = Convert.ToInt32(((DataRowView)e.Item.DataItem).Row.ItemArray[2]).ToInvariantString();
                Repeater rptCategories = null;
                if (displaySettings.PostListUseBottomDate)
                {
                    rptCategories = (Repeater)e.Item.FindControl("rptBottomCategories");
                }
                else
                {
                    //rptCategoriesTop
                    rptCategories = (Repeater)e.Item.FindControl("rptTopCategories");
                }

                if ((rptCategories != null) && (rptCategories.Visible))
                {

                    string whereClause = string.Format("ItemID = '{0}'", ItemId);
                    DataView dv = new DataView(dsBlogPosts.Tables["Categories"], whereClause, "", DataViewRowState.CurrentRows);

                    rptCategories.DataSource = dv;
                    rptCategories.DataBind();

                    rptCategories.Visible = (rptCategories.Items.Count > 0);
                }

                Repeater rptAttachments = (Repeater)e.Item.FindControl("rptAttachments");
                if ((rptAttachments != null) && (rptAttachments.Visible))
                {
                    string blogGuid = ((DataRowView)e.Item.DataItem).Row.ItemArray[1].ToString();
                    string whereClause = string.Format("ItemGuid = '{0}'", blogGuid);
                    DataView dv = new DataView(dsBlogPosts.Tables["Attachments"], whereClause, "", DataViewRowState.CurrentRows);

                    rptAttachments.DataSource = dv;
                    rptAttachments.DataBind();

                    rptAttachments.Visible = (rptAttachments.Items.Count > 0);

                    if (rptAttachments.Visible)
                    {
                        basePage.ScriptConfig.IncludeMediaElement = true;
                        basePage.StyleCombiner.IncludeMediaElement = true;
                    }

                }

            }

        }

        protected virtual void PopulateLabels()
        {
            EditBlogAltText = BlogResources.EditImageAltText;
            FeedBackLabel = BlogResources.BlogFeedbackLabel;

            mojoBasePage basePage = Page as mojoBasePage;
            if (basePage != null)
            {
                if (!basePage.UseTextLinksForFeatureSettings)
                {
                    EditLinkImageUrl = ImageSiteRoot + "/Data/SiteImages/" + EditContentImage;
                }

                if (basePage.AnalyticsSection.Length == 0)
                {
                    basePage.AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsBlogSection", "blog");
                }

            }

            if (displaySettings.OverridePostCategoriesLabel.Length > 0)
            {
                CategoriesResourceKey = displaySettings.OverridePostCategoriesLabel;
            }

        }

        protected bool CanEditPost(int postAuthorId)
        {
            if (BlogConfiguration.SecurePostsByUser)
            {
                if (WebUser.IsInRoles(config.ApproverRoles)) { return true; }

                if (currentUser == null) { return false; }

                return (postAuthorId == currentUser.UserId);
            }

            return isEditable;

        }

        protected string FormatSubtitle(string subTitle)
        {
            if (!displaySettings.ShowSubTitleOnList) { return string.Empty; }
            if (string.IsNullOrEmpty(subTitle)) { return string.Empty; }


            return "<" + displaySettings.ListItemSubtitleElement
                + " class='subtitle'>" + subTitle + "</" + displaySettings.ListItemSubtitleElement + ">";
        }

        protected string FormatPostAuthor(bool showPostAuthor, string authorName, string firstName, string lastName)
        {
            if (showPostAuthor)
            {
                if (config.BlogAuthor.Length > 0)
                {
                    return string.Format(CultureInfo.InvariantCulture, BlogResources.PostAuthorFormat, config.BlogAuthor);
                }

                if ((!string.IsNullOrEmpty(firstName)) && (!string.IsNullOrEmpty(lastName)))
                {
                    return string.Format(CultureInfo.InvariantCulture, BlogResources.PostAuthorFormat, firstName + " " + lastName);
                }

                return string.Format(CultureInfo.InvariantCulture, BlogResources.PostAuthorFormat, authorName);
            }

            return string.Empty;

        }

        protected string FormatBlogEntry(string blogHtml, string excerpt, string url, int itemId, string imageUrl, bool useImage)
        {
            if (useExcerpt)
            {
                //if excerpt is populated just use it
                if ((excerpt.Length > 0) && (excerpt != "<p>&#160;</p>")) // this was added by the editor(s) when content was empty
                {
                    return excerpt + config.ExcerptSuffix + " <a href='" + FormatBlogUrl(url, itemId) + "' class='morelink'>" + config.MoreLinkText + "</a><div>&nbsp;</div>";
                }

                // no excerpt so need to generate one
                string result = string.Empty;
                if ((blogHtml.Length > config.ExcerptLength) && (config.MoreLinkText.Length > 0))
                {

                    result = UIHelper.CreateExcerpt(blogHtml, config.ExcerptLength, config.ExcerptSuffix);
                    result += " <a href='" + FormatBlogTitleUrl(url, itemId) + "' class='morelink'>" + config.MoreLinkText + "</a><div class='paddiv'>&nbsp;</div>";

                    if(useImage && imageUrl.Length > 0)
                    {
                        
                        string imageMarkup = string.Format(CultureInfo.InvariantCulture, displaySettings.ExcerptImageFormat, ResolveUrl(imageUrl));
                        if(displaySettings.HeadlineImageAboveExcerpt)
                        {
                            return imageMarkup + result;
                        }
                        else
                        {
                            return result + imageMarkup;
                        }
                    }

                    return result;
                }
                else
                { // full post is shorter than excerpt length
                    if (useImage && imageUrl.Length > 0)
                    {

                        string imageMarkup = string.Format(CultureInfo.InvariantCulture, displaySettings.ExcerptImageFormat, ResolveUrl(imageUrl));
                        if (displaySettings.HeadlineImageAboveExcerpt)
                        {
                            return imageMarkup + blogHtml;
                        }
                        else
                        {
                            return blogHtml + imageMarkup;
                        }
                    }

                }

            }

            return blogHtml;
        }

        protected string FormatBlogDate(DateTime startDate)
        {
            string timeFormat = displaySettings.OverrideDateFormat;
            if (timeFormat.Length == 0) { timeFormat = config.DateTimeFormat; }

            if (timeZone != null)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(startDate, timeZone).ToString(timeFormat);

            }

            return startDate.AddHours(TimeOffset).ToString(timeFormat);

        }

        protected string FormatBlogUrl(string itemUrl, int itemId)
        {
            if (useFriendlyUrls && (itemUrl.Length > 0))
                return SiteRoot + itemUrl.Replace("~", string.Empty) + disqusFlag;

            return SiteRoot + "/Blog/ViewPost.aspx?pageid=" + PageId.ToInvariantString()
                + "&ItemID=" + itemId.ToInvariantString()
                + "&mid=" + ModuleId.ToInvariantString()
                + disqusFlag;

        }

        protected string FormatBlogTitleUrl(string itemUrl, int itemId)
        {
            if (useFriendlyUrls && (itemUrl.Length > 0))
                return SiteRoot + itemUrl.Replace("~", string.Empty);

            return SiteRoot + "/Blog/ViewPost.aspx?pageid=" + PageId.ToInvariantString()
                + "&ItemID=" + itemId.ToInvariantString()
                + "&mid=" + ModuleId.ToInvariantString();

        }

        private string GetRssUrl()
        {
           // if ((config.FeedburnerFeedUrl.Length > 0)&&(!BlogConfiguration.UseRedirectForFeedburner)) { return config.FeedburnerFeedUrl; }
            if (
                (categoryId == -1)
               && (config.FeedburnerFeedUrl.Length > 0)
                )
            {
                if (!BlogConfiguration.UseRedirectForFeedburner)
                {
                    return config.FeedburnerFeedUrl;
                }

                return SiteRoot + "/Blog/RSS.aspx?p=" + pageId.ToInvariantString()
                    + "~" + ModuleId.ToInvariantString() + "~" + categoryId.ToInvariantString()
                    + "&amp;r=" + Global.FeedRedirectBypassToken.ToString();

            }

            //return SiteRoot + "/blog" + ModuleId.ToInvariantString() + "rss.aspx";
            return SiteRoot + "/Blog/RSS.aspx?p=" + pageId.ToInvariantString()
                + "~" + ModuleId.ToInvariantString() + "~" + categoryId.ToInvariantString()
                ;

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

            if (config.AddThisAccountId.Length > 0)
            {
                addThisAccountId = config.AddThisAccountId;
            }

            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
            categoryId = WebUtils.ParseInt32FromQueryString("cat", categoryId);
            Month = WebUtils.ParseInt32FromQueryString("month", Month);
            Year = WebUtils.ParseInt32FromQueryString("year", Year);
            attachmentBaseUrl = SiteUtils.GetFileAttachmentUploadPath();

            if (Page is mojoBasePage) 
            { 
                basePage = Page as mojoBasePage;
                module = basePage.GetModule(moduleId, Blog.FeatureGuid);
            }

            if (module == null) { return; }

            MaxAllowedGravatarRating = SiteUtils.GetMaxAllowedGravatarRating();
            UserNameTooltipFormat = displaySettings.AvatarUserNameTooltipFormat;

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

            if (config.UseExcerpt && !displaySettings.ShowAvatarWithExcerpt) { disableAvatars = true; }

            CalendarDate = WebUtils.ParseDateFromQueryString("blogdate", DateTime.UtcNow).Date;

            if (CalendarDate > DateTime.UtcNow.Date)
            {
                CalendarDate = DateTime.UtcNow.Date;
            }

            if ((config.UseExcerpt) && (!config.GoogleMapIncludeWithExcerpt)) { ShowGoogleMap = false; }

            EnableContentRating = config.EnableContentRating && !displaySettings.PostListDisableContentRating;
            if (config.UseExcerpt) { EnableContentRating = false; }

           

            //if (config.AddThisCustomBrand.Length > 0)
            //{
            //    addThisCustomBrand = config.AddThisCustomBrand;
            //}
            
            

            if (config.DisqusSiteShortName.Length > 0)
            {
                DisqusSiteShortName = config.DisqusSiteShortName;
            }
            else
            {
                DisqusSiteShortName = siteSettings.DisqusSiteShortName;
            }

            if (config.IntenseDebateAccountId.Length > 0)
            {
                IntenseDebateAccountId = config.IntenseDebateAccountId;
            }
            else
            {
                IntenseDebateAccountId = siteSettings.IntenseDebateAccountId;
            }

            navTop.ModuleId = ModuleId;
            navTop.ModuleGuid = module.ModuleGuid;
            navTop.PageId = PageId;
            navTop.IsEditable = IsEditable;
            navTop.Config = config;
            navTop.SiteRoot = SiteRoot;
            navTop.ImageSiteRoot = ImageSiteRoot;

            navBottom.ModuleId = ModuleId;
            navBottom.ModuleGuid = module.ModuleGuid;
            navBottom.PageId = PageId;
            navBottom.IsEditable = IsEditable;
            navBottom.Config = config;
            navBottom.SiteRoot = SiteRoot;
            navBottom.ImageSiteRoot = ImageSiteRoot;

            TitleOnly = config.TitleOnly || displaySettings.PostListForceTitleOnly;
            ShowTweetThisLink = config.ShowTweetThisLink && !config.UseExcerpt;
            ShowPlusOneButton = config.ShowPlusOneButton && !config.UseExcerpt;
            UseFacebookLikeButton = config.UseFacebookLikeButton && !config.UseExcerpt;
            useExcerpt = config.UseExcerpt || displaySettings.PostListForceExcerptMode;
            pageSize = config.PageSize;
            AllowComments = Config.AllowComments && ShowCommentCounts;

            //TODO: should we use separate settings for each displaymode?
            switch (displayMode)
            {
                case "ByCategory":

                    if (displaySettings.CategoryListForceTitleOnly)
                    {
                        TitleOnly = true;
                    }

                    if (displaySettings.CategoryListOverridePageSize > 0)
                    {
                        pageSize = displaySettings.CategoryListOverridePageSize;
                    }

                    if (displaySettings.ArchiveViewHideFeedbackLink)
                    {
                        AllowComments = false;
                    }

                    if (displaySettings.OverrideCategoryListItemHeadingElement.Length > 0)
                    {
                        itemHeadingElement = displaySettings.OverrideCategoryListItemHeadingElement;
                    }

                    break;

                case "ByMonth":

                    if (displaySettings.ArchiveListForceTitleOnly)
                    {
                        TitleOnly = true;
                    }

                    if (displaySettings.ArchiveListOverridePageSize > 0)
                    {
                        pageSize = displaySettings.ArchiveListOverridePageSize;
                    }

                    if (displaySettings.OverrideArchiveListItemHeadingElement.Length > 0)
                    {
                        itemHeadingElement = displaySettings.OverrideArchiveListItemHeadingElement;
                    }

                    break;

                case "DescendingByDate":
                default:

                    if (displaySettings.PostListOverridePageSize > 0)
                    {
                        pageSize = displaySettings.PostListOverridePageSize;
                    }

                    if (displaySettings.OverrideListItemHeadingElement.Length > 0)
                    {
                        itemHeadingElement = displaySettings.OverrideListItemHeadingElement;
                    }

                    break;

            }

            


            if (config.AllowComments)
            {
                if ((DisqusSiteShortName.Length > 0) && (config.CommentSystem == "disqus"))
                {
                    disqusFlag = "#disqus_thread";
                    disqus.SiteShortName = DisqusSiteShortName;
                    disqus.RenderCommentCountScript = true;
                    navTop.ShowCommentCount = false;
                    navBottom.ShowCommentCount = false;

                }

                if ((IntenseDebateAccountId.Length > 0) && (config.CommentSystem == "intensedebate"))
                {
                    ShowCommentCounts = false;
                   
                    navTop.ShowCommentCount = false;
                    navBottom.ShowCommentCount = false;
                }

                if (config.CommentSystem == "facebook")
                {
                    ShowCommentCounts = false;
                    navTop.ShowCommentCount = false;
                    navBottom.ShowCommentCount = false;
                }

            }
            else
            {
                navTop.ShowCommentCount = false;
                navBottom.ShowCommentCount = false;
            }


            if (!config.NavigationOnRight)
            {
                this.divblog.CssClass = "blogcenter-leftnav";
            }
           

            if (config.Copyright.Length > 0)
            {
                lblCopyright.Text = config.Copyright;
            }

           

            navTop.Visible = false;

            if (config.ShowCalendar
                || config.ShowArchives
                || config.ShowAddFeedLinks
                || config.ShowCategories
                || config.ShowFeedLinks
                || config.ShowStatistics
                || (config.UpperSidebar.Length > 0)
                || (config.LowerSidebar.Length > 0)
                )
            {
                
                navTop.Visible = true;
            }

           

            if (!navTop.Visible)
            {
                divblog.CssClass = "blogcenter-nonav";
            }

            if (displaySettings.PostListExtraCss.Length > 0)
            {
                divblog.ExtraCssClasses = " " + displaySettings.PostListExtraCss;
            }

            navBottom.Visible = false;

            if ((navTop.Visible) && (displaySettings.UseBottomNavigation))
            {
                navTop.Visible = false;
                navBottom.Visible = true;
            }

            //if (IsEditable)
            //{
            //    countOfDrafts = Blog.CountOfDrafts(ModuleId);
            //}

            useFriendlyUrls = BlogConfiguration.UseFriendlyUrls(moduleId);
            if (!WebConfigSettings.UseUrlReWriting) { useFriendlyUrls = false; }

            


            

            

        }

        

        protected bool UseProfileLink()
        {
            if (!displaySettings.LinkAuthorAvatarToProfile) { return false; }

            if (Request.IsAuthenticated)
            {
                // if we know the user is signed in and not in a role allowed then return username without a profile link
                if (!WebUser.IsInRoles(siteSettings.RolesThatCanViewMemberList))
                {
                    return false;
                }
            }

            // if user is not authenticated we don't know if he will be allowed but he will be prompted to login first so its ok to show the link
            return true;
        }


        protected virtual void SetupRssLink()
        {
            if (WebConfigSettings.DisableBlogRssMetaLink) { return; }
            if (config.FeedIsDisabled) { return; }
            if (!config.AddFeedDiscoveryLink) { return; }

            if (module != null)
            {
                if (Page.Master != null)
                {
                    Control head = Page.Master.FindControl("Head1");
                    if (head != null)
                    {

                        Literal rssLink = new Literal();
                        rssLink.Text = "<link rel=\"alternate\" type=\"application/rss+xml\" title=\""
                                + module.ModuleTitle + "\" href=\""
                                + GetRssUrl() + "\" />";

                        head.Controls.Add(rssLink);

                    }

                }
            }

        }

        

    }
}