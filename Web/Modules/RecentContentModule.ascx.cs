using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using log4net;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.ContentUI
{
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
			TitleControl.Visible = !RenderInWebPartMode;
			if (ModuleConfiguration != null)
			{
				Title = ModuleConfiguration.ModuleTitle;
				Description = ModuleConfiguration.FeatureName;
			}

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
				lblError.Text = "There was an error accessing the search index. Recent content could not be retrieved.";
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

		public string BuildUrl(IndexItem indexItem)
		{
			if (indexItem.UseQueryStringParams)
			{
				return $"{SiteRoot}/{indexItem.ViewPage}?pageid={indexItem.PageId.ToInvariantString()}&mid={indexItem.ModuleId.ToInvariantString()}&ItemID={indexItem.ItemId.ToInvariantString()}{indexItem.QueryStringAddendum}";
			}
			else
			{
				return SiteRoot + "/" + indexItem.ViewPage;
			}
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
				return $"{pageName} &gt; {itemTitle}";
			}

			return pageName;
		}

		private void LoadSettings()
		{
			config = new RecentContentConfiguration(Settings);

			if (displaySettings.ShowCreatedDate || displaySettings.ShowLastModDate)
			{
				timeZone = SiteUtils.GetUserTimeZone();
			}

			lnkFeedTop.NavigateUrl = $"{SiteRoot}/Services/RecentContentRss.aspx?pageid={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}";
			if (config.FeedburnerFeedUrl.Length > 0)
			{
				lnkFeedTop.NavigateUrl += $"&r={Global.FeedRedirectBypassToken.ToString()}";
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
			if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

			GetCreated = WebUtils.ParseBoolFromHashtable(settings, "RecentContentUseCreatedDate", GetCreated);
			ShowExcerpt = WebUtils.ParseBoolFromHashtable(settings, "ShowExcerpt", ShowExcerpt);
			ShowAuthor = WebUtils.ParseBoolFromHashtable(settings, "ShowAuthor", ShowAuthor);
			ShowCreatedDate = WebUtils.ParseBoolFromHashtable(settings, "ShowCreatedDate", ShowCreatedDate);
			ShowLastModDate = WebUtils.ParseBoolFromHashtable(settings, "ShowLastModDate", ShowLastModDate);
			MaxRecentItemsToGet = WebUtils.ParseInt32FromHashtable(settings, "MaxRecentItemsToGet", MaxRecentItemsToGet);
			MaxDaysOldRecentItemsToGet = WebUtils.ParseInt32FromHashtable(settings, "MaxDaysOldRecentItemsToGet", MaxDaysOldRecentItemsToGet);
			SearchableFeature = WebUtils.ParseStringFromHashtable(settings, "SearchableFeature", SearchableFeature);
			EnableFeed = WebUtils.ParseBoolFromHashtable(settings, "EnableFeed", EnableFeed);
			ShowFeedLink = WebUtils.ParseBoolFromHashtable(settings, "ShowFeedLink", ShowFeedLink);
			FeedCacheTimeInMinutes = WebUtils.ParseInt32FromHashtable(settings, "FeedCacheTimeInMinutes", FeedCacheTimeInMinutes);
			FeedTimeToLiveInMinutes = WebUtils.ParseInt32FromHashtable(settings, "FeedTimeToLiveInMinutes", FeedTimeToLiveInMinutes);
			FeedburnerFeedUrl = WebUtils.ParseStringFromHashtable(settings, "FeedburnerFeedUrl", FeedburnerFeedUrl);
			FeedChannelTitle = WebUtils.ParseStringFromHashtable(settings, "FeedChannelTitle", FeedChannelTitle);
			FeedChannelDescription = WebUtils.ParseStringFromHashtable(settings, "FeedChannelDescription", FeedChannelDescription);
			FeedChannelCopyright = WebUtils.ParseStringFromHashtable(settings, "FeedChannelCopyright", FeedChannelCopyright);
			FeedChannelManagingEditor = WebUtils.ParseStringFromHashtable(settings, "FeedChannelManagingEditor", FeedChannelManagingEditor);
		}

		public string FeedChannelManagingEditor { get; private set; } = string.Empty;

		public string FeedChannelCopyright { get; private set; } = string.Empty;

		public string FeedChannelDescription { get; private set; } = string.Empty;

		public string FeedChannelTitle { get; private set; } = string.Empty;

		public int FeedTimeToLiveInMinutes { get; private set; } = 10;

		public bool EnableFeed { get; private set; } = true;

		public bool ShowFeedLink { get; private set; } = true;

		public int FeedCacheTimeInMinutes { get; private set; } = 10;

		public string FeedburnerFeedUrl { get; private set; } = string.Empty;

		public bool ShowCreatedDate { get; set; } = true;

		public bool ShowLastModDate { get; set; } = true;

		public bool ShowAuthor { get; set; } = true;

		public bool ShowExcerpt { get; private set; } = true;

		public string SearchableFeature { get; private set; } = string.Empty;

		public int MaxRecentItemsToGet { get; private set; } = 5;

		public bool GetCreated { get; private set; } = false;

		public int MaxDaysOldRecentItemsToGet { get; private set; } = 30;
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
		public string ItemHeadingElement { get; set; } = "h3";

		public bool ShowExcerpt { get; set; } = true;

		public bool ShowAuthor { get; set; } = true;

		public string AuthorFormat { get; set; } = string.Empty;

		public bool ShowCreatedDate { get; set; } = true;

		public string CreatedFormat { get; set; } = string.Empty;

		public bool ShowLastModDate { get; set; } = true;

		public string ModifiedFormat { get; set; } = string.Empty;

		public bool ShowFeedLinkTop { get; set; } = true;

		public bool ShowFeedLinkBottom { get; set; } = false;

		public string FeedIconPath { get; set; } = "~/Data/SiteImages/feed.png";

		/// <summary>
		/// http://msdn.microsoft.com/en-us/library/az4se3k1.aspx
		/// </summary>
		public string DateFormat { get; set; } = "d";

		protected override void Render(HtmlTextWriter writer)
		{
			// nothing to render, this control is just a themeable property bag
		}
	}
}