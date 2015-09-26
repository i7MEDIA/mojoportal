// Author:				        Joe Audette
// Created:			            2009-05-04
//	Last Modified:              2012-10-27
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.BlogUI
{
    public partial class FeedLinksControl : UserControl
    {
        private int pageId = -1;
        private int moduleId = -1;
        private string siteRoot = string.Empty;
        private BlogConfiguration config = new BlogConfiguration();
        private string imageSiteRoot = string.Empty;
        private SiteSettings siteSettings = null;
        protected string addThisAccountId = string.Empty;
        protected string RssImageFile = WebConfigSettings.RSSImageFileName;
        private int categoryId = -1;

        
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

        //public string ImageSiteRoot
        //{
        //    get { return imageSiteRoot; }
        //    set { imageSiteRoot = value; }
        //}

        public BlogConfiguration Config
        {
            get { return config; }
            set { config = value; }
        }

        

        protected void Page_Load(object sender, EventArgs e)
        {
            
            
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (this.Visible)
            {
                if (pageId == -1) { return; }
                if (moduleId == -1) { return; }
               
                LoadSettings();
                PopulateLabels();
                SetupLinks();
            }


            base.OnPreRender(e);

        }

        private void SetupLinks()
        {
            if (siteSettings == null) { return; }

            lnkRSS.HRef = GetRssUrl();
            imgRSS.Src =  Page.ResolveUrl("~/Data/SiteImages/" + RssImageFile);
            if (displaySettings.OverrideRssFeedImageUrl.Length > 0)
            {
                imgRSS.Src = Page.ResolveUrl(displaySettings.OverrideRssFeedImageUrl);
            }

            lnkAddThisRss.HRef = "http://www.addthis.com/feed.php?pub="
                + addThisAccountId + "&amp;h1=" + Server.UrlEncode(GetRssUrl())
                + "&amp;t1=";

            imgAddThisRss.Src = Page.ResolveUrl(config.AddThisRssButtonImageUrl);

            lnkAddMSN.HRef = "http://my.msn.com/addtomymsn.armx?id=rss&amp;ut=" + GetRssUrl();

            imgMSNRSS.Src = Page.ResolveUrl(displaySettings.MsnSubscribeIconUrl);

            lnkAddToLive.HRef = "http://www.live.com/?add=" + Server.UrlEncode(GetRssUrl());

            imgAddToLive.Src = Page.ResolveUrl(displaySettings.LiveSubscribeIcon);

            lnkAddYahoo.HRef = "http://e.my.yahoo.com/config/promo_content?.module=ycontent&amp;.url="
                + GetRssUrl();

            imgYahooRSS.Src = Page.ResolveUrl(displaySettings.MyYahooSubscribeIcon);

            lnkAddGoogle.HRef = "http://fusion.google.com/add?feedurl="
                + GetRssUrl();

            imgGoogleRSS.Src = Page.ResolveUrl(displaySettings.GoogleSubscribeIcon);

            liOdiogoPodcast.Visible = (config.OdiogoPodcastUrl.Length > 0);
            lnkOdiogoPodcast.HRef = config.OdiogoPodcastUrl;
            lnkOdiogoPodcastTextLink.NavigateUrl = config.OdiogoPodcastUrl;
            imgOdiogoPodcast.Src = Page.ResolveUrl("~/Data/SiteImages/podcast.png");

            


        }

        private string GetRssUrl()
        {
            if (
                (categoryId == -1)
               && (config.FeedburnerFeedUrl.Length > 0) 
                && (!BlogConfiguration.UseRedirectForFeedburner)
                ) 
            { return config.FeedburnerFeedUrl; }
            
            
            //if (WebConfigSettings.UseUrlReWriting)
            //{
            //    return SiteRoot + "/blog" + ModuleId.ToInvariantString() + "rss.aspx";
            //}
            //else
            //{
            //    return SiteRoot + "/Blog/RSS.aspx?pageid=" + pageId.ToInvariantString()
            //        + "&mid=" + ModuleId.ToInvariantString();
            //}

            if (config.FeedburnerFeedUrl.Length > 0)
            {
                return SiteRoot + "/Blog/RSS.aspx?p=" + pageId.ToInvariantString()
                    + "~" + ModuleId.ToInvariantString() + "~" + categoryId.ToInvariantString()
                    + "&amp;r=" + Global.FeedRedirectBypassToken.ToString();
            }

            return SiteRoot + "/Blog/RSS.aspx?p=" + pageId.ToInvariantString()
                    + "~" + ModuleId.ToInvariantString() + "~" + categoryId.ToInvariantString()
                    ;

        }

        private void PopulateLabels()
        {
            lnkRSS.Title = BlogResources.BlogRSSLinkTitle;
            lnkAddThisRss.Title = BlogResources.BlogAddThisSubscribeAltText;
            lnkAddMSN.Title = BlogResources.BlogModuleAddToMyMSNLink;
            lnkAddYahoo.Title = BlogResources.BlogModuleAddToMyYahooLink;
            lnkAddGoogle.Title = BlogResources.BlogModuleAddToGoogleLink;
            lnkAddToLive.Title = BlogResources.BlogModuleAddToWindowsLiveLink;
            lnkOdiogoPodcast.Title = BlogResources.PodcastLink;
            lnkOdiogoPodcastTextLink.Text = BlogResources.PodcastLink;

        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            if (BlogConfiguration.UseCategoryFeedurlOnCategoryPage)
            {
                categoryId = WebUtils.ParseInt32FromQueryString("cat", categoryId);
            }

            //siteRoot = siteSettings.SiteRoot;
            // we don't want ssl on the feed urls since it resultsin browser warnings
            siteRoot = SiteUtils.GetNavigationSiteRoot().Replace("https:","http:");

            if (config.AddThisAccountId.Length > 0)
            {
                addThisAccountId = config.AddThisAccountId;
            }
            else
            {
                addThisAccountId = siteSettings.AddThisDotComUsername;
            }

            liAddThisRss.Visible = (addThisAccountId.Length > 0);

            liAddThisRss.Visible = (config.ShowAddFeedLinks && (addThisAccountId.Length > 0));
            liAddGoogle.Visible = config.ShowAddFeedLinks;
            liAddMSN.Visible = config.ShowAddFeedLinks;
            liAddYahoo.Visible = config.ShowAddFeedLinks;
            liAddToLive.Visible = config.ShowAddFeedLinks;

            if (liAddThisRss.Visible)
            {
                liAddGoogle.Visible = false;
                liAddMSN.Visible = false;
                liAddYahoo.Visible = false;
                liAddToLive.Visible = false;

            }

           //if (imageSiteRoot.Length == 0) { imageSiteRoot = WebUtils.GetSiteRoot(); }

        }


    }
}