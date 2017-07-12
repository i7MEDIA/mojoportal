// Author:					
// Created:					2013-01-19
// Last Modified:			2014-01-24
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using log4net;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace mojoPortal.Web.ContentUI
{

    public partial class RecentContentModule : SiteModuleControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RecentContentModule));
        private TimeZoneInfo timeZone = null;
        protected RecentContentConfiguration config = new RecentContentConfiguration();


        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);

        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            //TitleControl.EditUrl = SiteRoot + "/RecentContent/RecentContentEdit.aspx";
            TitleControl.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            if (WebConfigSettings.DisableSearchIndex)
            {
                lblError.Text = "This feature is disabled because it depends on the search index which is also disabled.";
                log.Info("There is an instance of the recent content feature that is not working because the search index is disabled. "
                    + Request.RawUrl);
                return;
            }

            try
            {
                BindContent();
            }
            catch (System.IO.IOException ex)
            { // this can happen if the search index has not been created or is broken
                log.Error("handled error", ex);
                lblError.Text = "There was an error accessing the search index. Recent content could not be retrieved.";
            }


        }

        private void BindContent()
        {
            List<IndexItem> recentContent;
            Guid[] guids = config.GetFeatureGuids();

            if (config.GetCreated)
            {
                recentContent = IndexHelper.GetRecentCreatedContent(
                    siteSettings.SiteId,
                    guids,
                    DateTime.UtcNow.AddDays(-config.MaxDaysOldRecentItemsToGet),
                    config.MaxRecentItemsToGet);
            }
            else
            {
                recentContent = IndexHelper.GetRecentModifiedContent(
                    siteSettings.SiteId,
                    guids,
                    DateTime.UtcNow.AddDays(-config.MaxDaysOldRecentItemsToGet),
                    config.MaxRecentItemsToGet);
            }

            rptResults.DataSource = recentContent;
            rptResults.DataBind();

            rptResults.Visible = (rptResults.Items.Count > 0);

        }

        public string BuildUrl(IndexItem indexItem)
        {
            if (indexItem.UseQueryStringParams)
            {
                return SiteRoot + "/" + indexItem.ViewPage
                    + "?pageid="
                    + indexItem.PageId.ToInvariantString()
                    + "&mid="
                    + indexItem.ModuleId.ToInvariantString()
                    + "&ItemID="
                    + indexItem.ItemId.ToInvariantString()
                    + indexItem.QueryStringAddendum;

            }
            else
            {
                return SiteRoot + "/" + indexItem.ViewPage;
            }

        }

        public string FormatCreatedDate(IndexItem indexItem)
        {
            if ((!displaySettings.ShowCreatedDate) || (!config.ShowCreatedDate) || (timeZone == null)) { return string.Empty; }

            if (indexItem.CreatedUtc.Date == DateTime.MinValue.Date) { return string.Empty; }

            if (displaySettings.CreatedFormat.Length > 0)
            {
                return string.Format(
                    CultureInfo.CurrentCulture,
                    displaySettings.CreatedFormat,
                    TimeZoneInfo.ConvertTimeFromUtc(indexItem.CreatedUtc, timeZone).ToShortDateString());
            }

            return string.Format(
                    CultureInfo.CurrentCulture,
                    Resource.SearchCreatedHtmlFormat,
                    TimeZoneInfo.ConvertTimeFromUtc(indexItem.CreatedUtc, timeZone).ToShortDateString());



        }

        public string FormatModifiedDate(IndexItem indexItem)
        {
            if ((!displaySettings.ShowLastModDate) || (!config.ShowLastModDate) || (timeZone == null)) { return string.Empty; }

            if (indexItem.LastModUtc.Date == DateTime.MinValue.Date) { return string.Empty; }

            if (displaySettings.ModifiedFormat.Length > 0)
            {
                return string.Format(
                    CultureInfo.CurrentCulture,
                    displaySettings.ModifiedFormat,
                    TimeZoneInfo.ConvertTimeFromUtc(indexItem.LastModUtc, timeZone).ToString(displaySettings.DateFormat));
            }

            return string.Format(
                    CultureInfo.CurrentCulture,
                    Resource.SearchModifiedHtmlFormat,
                    TimeZoneInfo.ConvertTimeFromUtc(indexItem.LastModUtc, timeZone).ToString(displaySettings.DateFormat));



        }

        protected string FormatAuthor(string author)
        {
            if ((!displaySettings.ShowAuthor) ||(!config.ShowAuthor) || (string.IsNullOrEmpty(author))) { return string.Empty; }

            if (displaySettings.AuthorFormat.Length > 0)
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    displaySettings.AuthorFormat,
                    author);
            }

            return string.Format(
                    CultureInfo.InvariantCulture,
                    Resource.SearchAuthorHtmlFormat,
                    author);

            //SearchAuthorHtmlFormat
        }

        protected string FormatLinkText(string pageName, string moduleTtile, string itemTitle)
        {
            if (itemTitle.Length > 0)
            {
                return pageName + " &gt; " + itemTitle;
            }

            return pageName;
        }


        private void PopulateLabels()
        {
            //TitleControl.EditText = "Edit";
        }

        private void LoadSettings()
        {
            config = new RecentContentConfiguration(Settings);

            if (displaySettings.ShowCreatedDate || displaySettings.ShowLastModDate) { timeZone = SiteUtils.GetUserTimeZone(); }

            lnkFeedTop.NavigateUrl = SiteRoot + "/Services/RecentContentRss.aspx?pageid="
                + PageId.ToInvariantString() + "&mid=" + ModuleId.ToInvariantString();
            if (config.FeedburnerFeedUrl.Length > 0)
            {
                lnkFeedTop.NavigateUrl += "&r=" + Global.FeedRedirectBypassToken.ToString();
            }

            lnkFeedTop.ToolTip = Resource.RssFeed;
            lnkFeedTop.ImageUrl = Page.ResolveUrl(displaySettings.FeedIconPath);
            lnkFeedTop.Visible = config.EnableFeed && config.ShowFeedLink && displaySettings.ShowFeedLinkTop;

            lnkFeedBottom.NavigateUrl = lnkFeedTop.NavigateUrl;
            lnkFeedBottom.ToolTip = Resource.RssFeed;
            lnkFeedBottom.ImageUrl = Page.ResolveUrl(displaySettings.FeedIconPath);
            lnkFeedBottom.Visible = config.EnableFeed && config.ShowFeedLink && displaySettings.ShowFeedLinkBottom;
            

        }


    }

    public class RecentContentConfiguration
    {
        public RecentContentConfiguration()
        { }

        public RecentContentConfiguration(Hashtable settings)
        {
            LoadSettings(settings);

        }

        public Guid[] GetFeatureGuids()
        {
            
            if (searchableFeature.Length == 0)
            {
                return null;
                
            }

            List<string> gstrings = searchableFeature.SplitOnChar(',');
            if (gstrings.Count == 0)
            {
                return null;
            }
            else
            {
                Guid[] guids = new Guid[gstrings.Count];
                int i = 0;
                foreach (string s in gstrings)
                {
                    if (s.Length == 36)
                    {
                        guids[i] = new Guid(s);
                        i += 1;
                    }
                }

                return guids;

            }

        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

            //searchableFeature = WebUtils.ParseGuidFromHashTable(settings, "SearchableFeature", searchableFeature);

            getCreated = WebUtils.ParseBoolFromHashtable(settings, "RecentContentUseCreatedDate", getCreated);

            showExcerpt = WebUtils.ParseBoolFromHashtable(settings, "ShowExcerpt", showExcerpt);

            showAuthor = WebUtils.ParseBoolFromHashtable(settings, "ShowAuthor", showAuthor);

            showCreatedDate = WebUtils.ParseBoolFromHashtable(settings, "ShowCreatedDate", showCreatedDate);

            showLastModDate = WebUtils.ParseBoolFromHashtable(settings, "ShowLastModDate", showLastModDate);

            
            maxRecentItemsToGet = WebUtils.ParseInt32FromHashtable(settings, "MaxRecentItemsToGet", maxRecentItemsToGet);

            maxDaysOldRecentItemsToGet = WebUtils.ParseInt32FromHashtable(settings, "MaxDaysOldRecentItemsToGet", maxDaysOldRecentItemsToGet);


            if (settings.Contains("SearchableFeature"))
            {
                searchableFeature = settings["SearchableFeature"].ToString();
            }

            enableFeed = WebUtils.ParseBoolFromHashtable(settings, "EnableFeed", enableFeed);

            showFeedLink = WebUtils.ParseBoolFromHashtable(settings, "ShowFeedLink", showFeedLink);

            feedCacheTimeInMinutes = WebUtils.ParseInt32FromHashtable(settings, "FeedCacheTimeInMinutes", feedCacheTimeInMinutes);

            feedTimeToLiveInMinutes = WebUtils.ParseInt32FromHashtable(settings, "FeedTimeToLiveInMinutes", feedTimeToLiveInMinutes);

            if (settings.Contains("FeedburnerFeedUrl"))
            {
                feedburnerFeedUrl = settings["FeedburnerFeedUrl"].ToString();
            }

            if (settings.Contains("FeedChannelTitle"))
            {
                feedChannelTitle = settings["FeedChannelTitle"].ToString();
            }

            if (settings.Contains("FeedChannelDescription"))
            {
                feedChannelDescription = settings["FeedChannelDescription"].ToString();
            }

            if (settings.Contains("FeedChannelCopyright"))
            {
                feedChannelCopyright = settings["FeedChannelCopyright"].ToString();
            }

            if (settings.Contains("FeedChannelManagingEditor"))
            {
                feedChannelManagingEditor = settings["FeedChannelManagingEditor"].ToString();
            }

        }

        private string feedChannelManagingEditor = string.Empty;

        public string FeedChannelManagingEditor
        {
            get { return feedChannelManagingEditor; }
        }

        private string feedChannelCopyright = string.Empty;

        public string FeedChannelCopyright
        {
            get { return feedChannelCopyright; }
        }

        private string feedChannelDescription = string.Empty;

        public string FeedChannelDescription
        {
            get { return feedChannelDescription; }
        }

        private string feedChannelTitle = string.Empty;

        public string FeedChannelTitle
        {
            get { return feedChannelTitle; }
        }

        private int feedTimeToLiveInMinutes = 10;

        public int FeedTimeToLiveInMinutes
        {
            get { return feedTimeToLiveInMinutes; }
        }

        private bool enableFeed = true;

        public bool EnableFeed
        {
            get { return enableFeed; }
        }

        private bool showFeedLink = true;

        public bool ShowFeedLink
        {
            get { return showFeedLink; }
        }

        private int feedCacheTimeInMinutes = 10;

        public int FeedCacheTimeInMinutes
        {
            get { return feedCacheTimeInMinutes; }
        }

        private string feedburnerFeedUrl = string.Empty;

        public string FeedburnerFeedUrl
        {
            get { return feedburnerFeedUrl; }
        }


        private bool showCreatedDate = true;

        public bool ShowCreatedDate
        {
            get { return showCreatedDate; }
            set { showCreatedDate = value; }
        }

        private bool showLastModDate = true;

        public bool ShowLastModDate
        {
            get { return showLastModDate; }
            set { showLastModDate = value; }
        }

        private bool showAuthor = true;

        public bool ShowAuthor
        {
            get { return showAuthor; }
            set { showAuthor = value; }
        }

        private bool showExcerpt = true;

        public bool ShowExcerpt
        {
            get { return showExcerpt; }
        }


        private string searchableFeature = string.Empty; // a csv of featureguids

        public string SearchableFeature
        {
            get { return searchableFeature; }
        }

        private int maxRecentItemsToGet = 5;

        public int MaxRecentItemsToGet
        {
            get { return maxRecentItemsToGet; }
        }

        private bool getCreated = false;

        public bool GetCreated
        {
            get { return getCreated; }
        }

        private int maxDaysOldRecentItemsToGet = 30;

        public int MaxDaysOldRecentItemsToGet
        {
            get { return maxDaysOldRecentItemsToGet; }
        }
        




    }

}

namespace mojoPortal.Web.UI
{
    using System.Web.UI;
    using System.Web.UI.WebControls;
    /// <summary>
    /// this control doesn't render anything, it is used only as a themeable collection of settings for things we would like to be able to configure from theme.skin
    /// </summary>
    public class RecentContentDisplaySettings : WebControl
    {
        private string itemHeadingElement = "h3";

        public string ItemHeadingElement
        {
            get { return itemHeadingElement; }
            set { itemHeadingElement = value; }
        }

        private bool showExcerpt = true;

        public bool ShowExcerpt
        {
            get { return showExcerpt; }
        }

        private bool showAuthor = true;

        public bool ShowAuthor
        {
            get { return showAuthor; }
            set { showAuthor = value; }
        }

        private string authorFormat = string.Empty;

        public string AuthorFormat
        {
            get { return authorFormat; }
            set { authorFormat = value; }
        }

        private bool showCreatedDate = true;

        public bool ShowCreatedDate
        {
            get { return showCreatedDate; }
            set { showCreatedDate = value; }
        }

        private string createdFormat = string.Empty;

        public string CreatedFormat
        {
            get { return createdFormat; }
            set { createdFormat = value; }
        }

        private bool showLastModDate = true;

        public bool ShowLastModDate
        {
            get { return showLastModDate; }
            set { showLastModDate = value; }
        }

        private string modifiedFormat = string.Empty;

        public string ModifiedFormat
        {
            get { return modifiedFormat; }
            set { modifiedFormat = value; }
        }

        private bool showFeedLinkTop = true;

        public bool ShowFeedLinkTop
        {
            get { return showFeedLinkTop; }
            set { showFeedLinkTop = value; }
        }

        private bool showFeedLinkBottom = false;

        public bool ShowFeedLinkBottom
        {
            get { return showFeedLinkBottom; }
            set { showFeedLinkBottom = value; }
        }

        private string feedIconPath = "~/Data/SiteImages/feed.png";

        public string FeedIconPath
        {
            get { return feedIconPath; }
            set { feedIconPath = value; }
        }

        private string dateFormat = "d";
  
	    /// <summary>
	    /// http://msdn.microsoft.com/en-us/library/az4se3k1.aspx
	    /// </summary>
	    public string DateFormat
	    {
	        get { return dateFormat; }
	        set { dateFormat = value; }
	    }

        
        protected override void Render(HtmlTextWriter writer)
        {
            // nothing to render, this control is just a themeable property bag
        }
    }

}
