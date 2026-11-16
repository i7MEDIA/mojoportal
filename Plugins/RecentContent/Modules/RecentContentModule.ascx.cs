using log4net;
using mojoPortal.Plugins.RecentContent;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Collections.Generic;
using System.Globalization;

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
