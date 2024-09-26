using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using log4net;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.ContentUI;

public partial class RecentContentModule : SiteModuleControl
{
	private static readonly ILog log = LogManager.GetLogger(typeof(RecentContentModule));
	private TimeZoneInfo timeZone = null;
	protected RecentContentConfiguration config = new();

	#region OnInit
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);

	}
	#endregion

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
		PopulateControls();
	}

	private void PopulateControls()
	{
		if (ModuleConfiguration != null)
		{
			Title = ModuleConfiguration.ModuleTitle;
			Description = ModuleConfiguration.FeatureName;
		}

		pnlOuterWrap.SetOrAppendCss(config.CustomCssClassSetting);

		if (WebConfigSettings.DisableSearchIndex)
		{
			lblError.Text = Resource.FeatureDisabledDueToSearchIndexDisabled;
			log.Info($"There is an instance of the recent content feature that is not working because the search index is disabled. {Request.RawUrl}");
			return;
		}

		try
		{
			BindContent();
		}
		catch (System.IO.IOException ex)
		{ // this can happen if the search index has not been created or is broken
			log.Error("handled error", ex);
			lblError.Text = Resource.RecentContentSearchIndexError;
		}
	}

	private void BindContent()
	{
		List<IndexItem> recentContent;
		var guids = config.GetFeatureGuids();

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
		rptResults.Visible = rptResults.Items.Count > 0;
	}

	public string FormatCreatedDate(IndexItem indexItem)
	{
		if (!displaySettings.ShowCreatedDate || !config.ShowCreatedDate || timeZone is null)
		{
			return string.Empty;
		}

		if (indexItem.CreatedUtc.Date == DateTime.MinValue.Date)
		{
			return string.Empty;
		}

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
		if (!displaySettings.ShowLastModDate || !config.ShowLastModDate || timeZone is null)
		{
			return string.Empty;
		}

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
		if (!displaySettings.ShowAuthor || !config.ShowAuthor || string.IsNullOrWhiteSpace(author))
		{
			return string.Empty;
		}

		if (displaySettings.AuthorFormat.Length > 0)
		{
			return string.Format(CultureInfo.InvariantCulture, displaySettings.AuthorFormat, author);
		}

		return string.Format(CultureInfo.InvariantCulture, Resource.SearchAuthorHtmlFormat, author);
	}


	private void LoadSettings()
	{
		config = new RecentContentConfiguration(Settings);

		if (displaySettings.ShowCreatedDate || displaySettings.ShowLastModDate)
		{
			timeZone = SiteUtils.GetUserTimeZone();
		}

		lnkFeedTop.NavigateUrl = $"Services/RecentContentRss.aspx".ToLinkBuilder().PageId(PageId).ModuleId(ModuleId).ToString();

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
}