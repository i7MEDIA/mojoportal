using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;
using Argotic.Syndication;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web.ContentUI;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.Services;

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
	private Guid recentContentFeatureGuid = new("f889c7c8-78e1-4cd2-a4cd-a0723f9a7cf0");
	private string channelTitle = string.Empty;
	private string channelLink = string.Empty;
	private string channelDescription = string.Empty;
	private string channelCopyright = string.Empty;
	private string channelManagingEditor = string.Empty;
	private string channelLogo = string.Empty;
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

		if (siteSettings == null)
		{
			return;
		}

		cssBaseUrl = WebUtils.GetSiteRoot();
		siteRoot = SiteUtils.GetNavigationSiteRoot();
		modifiedSinceDate = DateTime.UtcNow.AddDays(-SiteUtils.RecentContentFeedMaxDaysOld(siteSettings)); // 30 by default
		maxItems = WebUtils.ParseInt32FromQueryString("n", true, SiteUtils.RecentContentDefaultItemsToRetrieve(siteSettings));

		if (maxItems > WebConfigSettings.RecentContentMaxItemsToRetrieve)
		{
			maxItems = SiteUtils.RecentContentMaxItemsToRetrieve(siteSettings);
		}

		featureGuid = WebUtils.ParseGuidFromQueryString("f", featureGuid);
		getCreated = WebUtils.ParseBoolFromQueryString("gc", getCreated);

		pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
		moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);

		redirectUrl = SiteUtils.GetNavigationSiteRoot();

		shouldRender = !SiteUtils.DisableRecentContentFeed(siteSettings);

	}


	private void RenderFeed()
	{
		if (siteSettings == null)
		{
			return;
		}

		if (!shouldRender)
		{
			WebUtils.SetupRedirect(this, redirectUrl);
			return;
		}

		var recentContent = GetData(); // gets the data and initilizes the channel params

		var mediaFolder = WebConfigSettings.SiteLogoUseMediaFolder ? "media/" : string.Empty;
		var siteLogo = new Uri(
			Invariant($"Data/Sites/{siteSettings.SiteId}/{mediaFolder}logos/{siteSettings.Logo}")
			.ToLinkBuilder().ToString()
			);

		var channel = new RssChannel
		{
			Generator = Resource.RecentContentFeedGenerator,
			Title = channelTitle.Coalesce(Resource.RecentContentRssFeedChannelTitle),
			Link = new Uri(channelLink),
			Description = channelDescription.Coalesce(string.Format(Resource.RecentContentRssFeedChannelDescription, siteSettings.SiteName)),
			Copyright = channelCopyright.Coalesce(siteSettings.CompanyName),
			ManagingEditor = channelManagingEditor,
			TimeToLive = channelTimeToLive,
			Image = new RssImage(
				new Uri(channelLink),
				siteSettings.SiteName,
				siteLogo
			)
		};

		var feed = new RssFeed
		{
			Channel = channel
		};

		int itemsAdded = 0;

		if (recentContent != null)
		{
			foreach (IndexItem indexItem in recentContent)
			{
				var item = new RssItem
				{
					Link = new Uri(indexItem.Url),
					Guid = new RssGuid(indexItem.Url),
					Title = indexItem.LinkText,
					PublicationDate = indexItem.LastModUtc,
					Author = indexItem.Author,
					Description = indexItem.ContentAbstract
				};
				channel.AddItem(item);
				itemsAdded += 1;
			}
		}

		if (itemsAdded == 0)
		{
			//channel must have at least one item
			var item = new RssItem
			{
				Link = new Uri(siteRoot),
				Title = Resource.RecentContentEmptyMessage,
				PublicationDate = DateTime.UtcNow
			};

			channel.AddItem(item);
		}

		// no cache locally
		if (Request.Url.Host.Contains("localhost"))
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

		using var xmlTextWriter = new XmlTextWriter(Response.OutputStream, encoding);
		xmlTextWriter.Formatting = Formatting.Indented;

		xmlTextWriter.WriteRaw("<?xml version=\"1.0\"?>");

		//////////////////
		// style for RSS Feed viewed in browsers
		if (ConfigurationManager.AppSettings["RSSCSS"] != null)
		{
			string rssCss = ConfigurationManager.AppSettings["RSSCSS"].ToString();
			xmlTextWriter.WriteWhitespace(" ");
			xmlTextWriter.WriteRaw($"\r\n<?xml-stylesheet type=\"text/css\" href=\"{rssCss.ToLinkBuilder()}\" ?>");
		}

		if (ConfigurationManager.AppSettings["RSSXsl"] != null)
		{
			string rssXsl = ConfigurationManager.AppSettings["RSSXsl"].ToString();
			xmlTextWriter.WriteWhitespace(" ");
			xmlTextWriter.WriteRaw($"\r\n<?xml-stylesheet type=\"text/xsl\" href=\"{rssXsl.ToLinkBuilder()}\" ?>");
		}
		///////////////////////////

		feed.Save(xmlTextWriter);
	}


	private List<IndexItem> GetData()
	{
		List<IndexItem> recentContent;

		if (pageId > -1 && moduleId > -1)
		{
			recentContent = GetRecentContent();
		}
		else
		{
			shouldRender = !SiteUtils.DisableRecentContentFeed(siteSettings);
			if (!shouldRender)
			{
				return null;
			}

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

		if (pageId == -1 || moduleId == -1)
		{
			return recentContent;
		}

		pageSettings = CacheHelper.GetCurrentPage();
		module = GetModule();

		if (module != null)
		{
			moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
			config = new RecentContentConfiguration(moduleSettings);
			shouldRender = config.EnableFeed;
			if (!shouldRender)
			{
				return null;
			}

			feedCacheTimeInMinutes = config.FeedCacheTimeInMinutes;
			channelTitle = config.FeedChannelTitle;
			channelLink = WebUtils.ResolveServerUrl(SiteUtils.GetCurrentPageUrl());
			channelDescription = config.FeedChannelDescription;
			channelCopyright = config.FeedChannelCopyright.Coalesce(siteSettings.SiteName);
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
				if (module.ModuleId == moduleId && module.FeatureGuid == recentContentFeatureGuid)
				{
					return module;
				}
			}
		}
		return null;
	}
}
