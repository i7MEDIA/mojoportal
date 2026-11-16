using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using System;
using System.Collections;

namespace mojoPortal.Plugins.RecentContent;

public class RecentContentConfiguration
{
	public string FeedChannelManagingEditor { get; private set; } = string.Empty;
	public string FeedChannelCopyright { get; private set; } = string.Empty;
	public string FeedChannelDescription { get; private set; } = string.Empty;
	public string FeedChannelTitle { get; private set; } = string.Empty;
	public int FeedTimeToLiveInMinutes { get; private set; } = 10;
	public bool EnableFeed { get; private set; } = true;
	public bool ShowFeedLink { get; private set; } = true;
	public int FeedCacheTimeInMinutes { get; private set; } = 10;
	public bool ShowCreatedDate { get; set; } = true;
	public bool ShowLastModDate { get; set; } = true;
	public bool ShowAuthor { get; set; } = true;
	public bool ShowExcerpt { get; private set; } = true;
	public string SearchableFeature { get; private set; } = string.Empty;
	public int MaxRecentItemsToGet { get; private set; } = 5;
	public bool GetCreated { get; private set; } = false;
	public int MaxDaysOldRecentItemsToGet { get; private set; } = 30;
	public string CustomCssClassSetting { get; private set; } = string.Empty;
	public RecentContentConfiguration() { }


	public RecentContentConfiguration(Hashtable settings)
	{
		LoadSettings(settings);
	}


	public Guid[] GetFeatureGuids()
	{
		if (SearchableFeature.Length == 0)
		{
			return null;
		}

		var gstrings = SearchableFeature.SplitOnChar(',');

		if (gstrings.Count == 0)
		{
			return null;
		}
		else
		{
			var guids = new Guid[gstrings.Count];
			var i = 0;

			foreach (var s in gstrings)
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
		if (settings == null)
		{
			throw new ArgumentException("must pass in a hashtable of settings");
		}

		GetCreated = settings.ParseBool("RecentContentUseCreatedDate", GetCreated);
		ShowExcerpt = settings.ParseBool("ShowExcerpt", ShowExcerpt);
		ShowAuthor = settings.ParseBool("ShowAuthor", ShowAuthor);
		ShowCreatedDate = settings.ParseBool("ShowCreatedDate", ShowCreatedDate);
		ShowLastModDate = settings.ParseBool("ShowLastModDate", ShowLastModDate);
		MaxRecentItemsToGet = settings.ParseInt32("MaxRecentItemsToGet", MaxRecentItemsToGet);
		MaxDaysOldRecentItemsToGet = settings.ParseInt32("MaxDaysOldRecentItemsToGet", MaxDaysOldRecentItemsToGet);
		SearchableFeature = settings.ParseString("SearchableFeature", SearchableFeature);
		EnableFeed = settings.ParseBool("EnableFeed", EnableFeed);

		if (EnableFeed && SiteUtils.DisableRecentContentFeed(CacheHelper.GetCurrentSiteSettings()))
		{
			EnableFeed = false;
		}

		ShowFeedLink = settings.ParseBool("ShowFeedLink", ShowFeedLink);
		FeedCacheTimeInMinutes = settings.ParseInt32("FeedCacheTimeInMinutes", FeedCacheTimeInMinutes);
		FeedTimeToLiveInMinutes = settings.ParseInt32("FeedTimeToLiveInMinutes", FeedTimeToLiveInMinutes);
		FeedChannelTitle = settings.ParseString("FeedChannelTitle", FeedChannelTitle);
		FeedChannelDescription = settings.ParseString("FeedChannelDescription", FeedChannelDescription);
		FeedChannelCopyright = settings.ParseString("FeedChannelCopyright", FeedChannelCopyright);
		FeedChannelManagingEditor = settings.ParseString("FeedChannelManagingEditor", FeedChannelManagingEditor);
		CustomCssClassSetting = settings.ParseString("CustomCssClassSetting", CustomCssClassSetting);
	}
}
