// Author:					    
// Created:				        2013-01-18
// Last Modified:			    2013-03-01
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using Argotic.Syndication;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Framework;
using mojoPortal.Web.ContentUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;

namespace mojoPortal.Web.Services
{
    public class RecentContentRss : Page
    {
        private SiteSettings siteSettings = null;
        private DateTime modifiedSinceDate = DateTime.UtcNow.AddDays(-30);
        private int maxItems = 20;
        private Guid featureGuid = Guid.Empty;
        private string siteRoot = string.Empty;
        private string cssBaseUrl = string.Empty;
        private bool getCreated = false;
        private int pageId = -1;
        private int moduleId = -1;
        private PageSettings pageSettings = null;
        private Module module = null;
        private Hashtable moduleSettings = null;
        private RecentContentConfiguration config = null;
        private Guid recentContentFeatureGuid = new Guid("f889c7c8-78e1-4cd2-a4cd-a0723f9a7cf0");
        private string channelTitle = string.Empty;
        private string channelLink = string.Empty;
        private string channelDescription = string.Empty;
        private string channelCopyright = string.Empty;
        private string channelManagingEditor = string.Empty;
        private int channelTimeToLive = 10;
        private int feedCacheTimeInMinutes = 10;
        private bool shouldRender = false;
        private string redirectUrl = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            LoadSettings();

            if (siteSettings == null)
            {
                WebUtils.SetupRedirect(this, SiteUtils.GetNavigationSiteRoot());
                return;
            }

            

            RenderFeed();


        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            cssBaseUrl = WebUtils.GetSiteRoot();
            siteRoot = SiteUtils.GetNavigationSiteRoot();
            modifiedSinceDate = DateTime.UtcNow.AddDays(-SiteUtils.RecentContentFeedMaxDaysOld(siteSettings)); // 30 by default
            maxItems = WebUtils.ParseInt32FromQueryString("n", true, SiteUtils.RecentContentDefaultItemsToRetrieve(siteSettings));
            if (maxItems > WebConfigSettings.RecentContentMaxItemsToRetrieve) { maxItems = SiteUtils.RecentContentMaxItemsToRetrieve(siteSettings); }

            featureGuid = WebUtils.ParseGuidFromQueryString("f", featureGuid);
            getCreated = WebUtils.ParseBoolFromQueryString("gc", getCreated);

            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);

            redirectUrl = SiteUtils.GetNavigationSiteRoot();
            

        }

        

        private void RenderFeed()
        {
            if (siteSettings == null) { return; }
            Argotic.Syndication.RssFeed feed = new Argotic.Syndication.RssFeed();
            RssChannel channel = new RssChannel();
            channel.Generator = "mojoPortal CMS Recent Content Feed Geenrator";
            feed.Channel = channel;

            List<IndexItem> recentContent = GetData(); // gets the data and initilizes the channel params

            if (!shouldRender)
            {
                WebUtils.SetupRedirect(this, redirectUrl);
                return;
            }

            if (channelTitle.Length == 0) { channelTitle = "Recent Content"; } //empty string will cause an error
            channel.Title = channelTitle;
            channel.Link = new System.Uri(channelLink);
            if (channelDescription.Length == 0) { channelDescription = "Recent Content"; } //empty string will cause an error
            channel.Description = channelDescription;
            channel.Copyright = channelCopyright;
            channel.ManagingEditor = channelManagingEditor;
            channel.TimeToLive = channelTimeToLive;

            int itemsAdded = 0;
            

            if (recentContent != null)
            {
                foreach (IndexItem indexItem in recentContent)
                {
                    RssItem item = new RssItem();
                    string itemUrl = BuildUrl(indexItem);
                    item.Link = new Uri(itemUrl);
                    item.Guid = new RssGuid(itemUrl);
                    item.Title = FormatLinkText(indexItem);
                    item.PublicationDate = indexItem.LastModUtc;
                    item.Author = indexItem.Author;
                    item.Description = indexItem.ContentAbstract;
                    channel.AddItem(item);
                    itemsAdded += 1;

                }
            }
            
            if (itemsAdded == 0)
            {
                //channel must have at least one item
                RssItem item = new RssItem();
                item.Link = new Uri(siteRoot);
                item.Title = "Stay tuned for future updates. ";
                //item.Description = 
                item.PublicationDate = DateTime.UtcNow;
                
                channel.AddItem(item);

            }
            
            // no cache locally
            if (Request.Url.AbsolutePath.Contains("localhost"))
            {
                Response.Cache.SetExpires(DateTime.Now.AddMinutes(-30));
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                
            }
            else
            {
                Response.Cache.SetExpires(DateTime.Now.AddMinutes(feedCacheTimeInMinutes));
                Response.Cache.SetCacheability(HttpCacheability.Public);
                Response.Cache.VaryByParams["f;gc;n;pageid;mid"] = true;
            }


            Response.ContentType = "application/xml";

            Encoding encoding = new UTF8Encoding();
            Response.ContentEncoding = encoding;

            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(Response.OutputStream, encoding))
            {
                xmlTextWriter.Formatting = Formatting.Indented;

                //////////////////
                // style for RSS Feed viewed in browsers
                if (ConfigurationManager.AppSettings["RSSCSS"] != null)
                {
                    string rssCss = ConfigurationManager.AppSettings["RSSCSS"].ToString();
                    xmlTextWriter.WriteWhitespace(" ");
                    xmlTextWriter.WriteRaw("<?xml-stylesheet type=\"text/css\" href=\"" + cssBaseUrl + rssCss + "\" ?>");

                }

                if (ConfigurationManager.AppSettings["RSSXsl"] != null)
                {
                    string rssXsl = ConfigurationManager.AppSettings["RSSXsl"].ToString();
                    xmlTextWriter.WriteWhitespace(" ");
                    xmlTextWriter.WriteRaw("<?xml-stylesheet type=\"text/xsl\" href=\"" + cssBaseUrl + rssXsl + "\" ?>");

                }
                ///////////////////////////

                feed.Save(xmlTextWriter);


            }


        }

        private List<IndexItem> GetData()
        {
            List<IndexItem> recentContent;

            if ((pageId > -1) && (moduleId > -1))
            {
                recentContent = GetRecentContent();

            }
            else
            {
                shouldRender = !SiteUtils.DisableRecentContentFeed(siteSettings);
                if (!shouldRender) { return null; }

                feedCacheTimeInMinutes = SiteUtils.RecentContentFeedCacheTimeInMinutes(siteSettings);

                channelTitle = siteSettings.SiteName;
                channelLink = WebUtils.ResolveServerUrl(SiteUtils.GetNavigationSiteRoot());
                channelDescription = SiteUtils.RecentContentChannelDescription(siteSettings);
                channelCopyright = SiteUtils.RecentContentChannelCopyright(siteSettings);
                channelManagingEditor = SiteUtils.RecentContentChannelNotifyEmail(siteSettings);
                channelTimeToLive = SiteUtils.RecentContentFeedTimeToLive(siteSettings);

                if (getCreated)
                {
                    recentContent = IndexHelper.GetRecentCreatedContent(
                       siteSettings.SiteId,
                       featureGuid,
                       modifiedSinceDate,
                       maxItems);
                }
                else
                {
                    recentContent = IndexHelper.GetRecentModifiedContent(
                       siteSettings.SiteId,
                       featureGuid,
                       modifiedSinceDate,
                       maxItems);
                }
            }

            return recentContent;
        }

        private List<IndexItem> GetRecentContent()
        {
            List<IndexItem> recentContent = null;

            if (pageId == -1) { return recentContent; }
            if (moduleId == -1) { return recentContent; }

            pageSettings = CacheHelper.GetCurrentPage();
            module = GetModule();

            if (module != null)
            {
                moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
                config = new RecentContentConfiguration(moduleSettings);
                shouldRender = config.EnableFeed;
                if (!shouldRender) { return null; }

                bool shouldRedirectToFeedburner = false;
                if (config.FeedburnerFeedUrl.Length > 0)
                {
                    shouldRedirectToFeedburner = true;
                    if ((Request.UserAgent != null) && (Request.UserAgent.Contains("FeedBurner")))
                    {
                        shouldRedirectToFeedburner = false; // don't redirect if the feedburner bot is reading the feed
                    }

                    Guid redirectBypassToken = WebUtils.ParseGuidFromQueryString("r", Guid.Empty);
                    if (redirectBypassToken == Global.FeedRedirectBypassToken)
                    {
                        shouldRedirectToFeedburner = false; // allows time for user to subscribe to autodiscovery links without redirecting
                    }


                }

                if (shouldRedirectToFeedburner)
                {
                    redirectUrl = config.FeedburnerFeedUrl;
                    shouldRender = false;
                    return null;

                }
               
                feedCacheTimeInMinutes = config.FeedCacheTimeInMinutes;
                channelTitle = config.FeedChannelTitle;
                channelLink = WebUtils.ResolveServerUrl(SiteUtils.GetCurrentPageUrl());
                channelDescription = config.FeedChannelDescription;
                channelCopyright = config.FeedChannelCopyright;
                channelManagingEditor = config.FeedChannelManagingEditor;
                channelTimeToLive = config.FeedTimeToLiveInMinutes;


                if (config.GetCreated)
                {
                    recentContent = IndexHelper.GetRecentCreatedContent(
                        siteSettings.SiteId,
                        config.GetFeatureGuids(),
                        DateTime.UtcNow.AddDays(-config.MaxDaysOldRecentItemsToGet),
                        config.MaxRecentItemsToGet);
                }
                else
                {
                    recentContent = IndexHelper.GetRecentModifiedContent(
                        siteSettings.SiteId,
                        config.GetFeatureGuids(),
                        DateTime.UtcNow.AddDays(-config.MaxDaysOldRecentItemsToGet),
                        config.MaxRecentItemsToGet);
                }
            }


            return recentContent;

        }

        private Module GetModule()
        {
            if (pageSettings != null)
            {
                foreach (Module module in pageSettings.Modules)
                {
                    if ((module.ModuleId == moduleId) && (module.FeatureGuid == recentContentFeatureGuid)) { return module; }
                }
            }
            return null;
        }

        protected string FormatLinkText(IndexItem indexItem)
        { 
            if (indexItem.Title.Length > 0)
            {
                return indexItem.PageName + " > " + indexItem.Title;
            }

            return indexItem.PageName;
        }

        private string BuildUrl(IndexItem indexItem)
        {
            if (indexItem.UseQueryStringParams)
            {
                return siteRoot + "/" + indexItem.ViewPage
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
                return siteRoot + "/" + indexItem.ViewPage;
            }

        }
    }

}
