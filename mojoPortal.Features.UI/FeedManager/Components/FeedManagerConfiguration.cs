using mojoPortal.Web.Framework;
using System;
using System.Collections;
using System.Configuration;
using System.Globalization;

namespace mojoPortal.Web.FeedUI
{
	public class FeedManagerConfiguration
	{
		#region Properties
		public bool UseAutoDiscoveryAggregateFeedLink { get; private set; } = true;
		public bool OpenLinkInNewWindow { get; private set; } = false;
		public bool ShowDate { get; private set; } = true;
		public bool UseExcerptSuffixAsLinkToPost { get; private set; } = false;
		public string ListLabel { get; private set; } = string.Empty;
		public string InstanceCssClass { get; private set; } = string.Empty;
		public string DateFormat { get; private set; } = string.Empty;
		public bool UseFeedListAsFilter { get; private set; } = false;
		public int FeedListCacheTimeout { get; private set; } = 3660;
		public int EntryCacheTimeout { get; private set; } = 3620;
		public int MaxDaysOld { get; private set; } = 90;
		public int MaxEntriesPerFeed { get; private set; } = 90;
		public bool AllowExternalImages { get; private set; } = false;
		public bool ShowFeedListOnRight { get; private set; } = true;
		public int RepeatColumns { get; private set; } = 1;
		public bool ShowAggregateFeedLink { get; private set; } = true;
		public bool ShowErrorMessageOnInvalidPosts { get; } = false;
		public bool LinkToAuthorSite { get; } = true;
		public bool SortAscending { get; private set; } = false;
		public bool ShowAuthor { get; private set; } = false;
		public bool ShowFeedName { get; private set; } = false;
		public bool ShowFeedNameBeforeContent { get; private set; } = false;
		public bool UseExcerpt { get; private set; } = false;
		public bool EnableSelectivePublishing { get; private set; } = false;
		public bool EnableInPlacePublishing { get; private set; } = false;
		public int ExcerptLength { get; private set; } = 250;
		public string ExcerptSuffix { get; private set; } = "...";
		public bool ShowIndividualFeedLinks { get; private set; } = true;
		public bool ShowItemDetail { get; private set; } = true;
		public bool UseFillerOnEmptyDays { get; private set; } = false;
		public bool UseCalendar { get; private set; } = false;
		public int PageSize { get; private set; } = 10;
		public bool UseNeatHtml { get; private set; } = true;
		public static string FeedItemHeadingElement
		{
			get
			{
				if (ConfigurationManager.AppSettings["FeedItemHeadingElement"] != null)
				{
					return ConfigurationManager.AppSettings["FeedItemHeadingElement"];
				}
				return "h3";
			}
		}
		public bool UseScroller { get; private set; } = false;
		public static bool UseReadWriteLockForCacheMenagement
		{
			get { return ConfigHelper.GetBoolProperty("FeedManager:UseReadWriteLockForCacheMenagement", false); }
		}

		#endregion Properties


		public FeedManagerConfiguration()
		{ }


		public FeedManagerConfiguration(Hashtable settings)
		{
			LoadSettings(settings);
		}


		private void LoadSettings(Hashtable settings)
		{
			if (settings == null)
			{
				throw new ArgumentException("must pass in a hashtable of settings");
			}

			if (settings.Contains("FeedDateFormatSetting"))
			{
				DateFormat = settings["FeedDateFormatSetting"].ToString().Trim();

				if (DateFormat.Length > 0)
				{
					try
					{
						string d = DateTime.Now.ToString(DateFormat, CultureInfo.CurrentCulture);
					}
					catch (FormatException)
					{
						DateFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
					}
				}
				else
				{
					DateFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
				}
			}

			FeedListCacheTimeout = WebUtils.ParseInt32FromHashtable(settings, "RSSFeedFeedListCacheTimeoutSetting", FeedListCacheTimeout);
			RepeatColumns = WebUtils.ParseInt32FromHashtable(settings, "RSSFeedFeedListColumnsSetting", RepeatColumns);
			EntryCacheTimeout = WebUtils.ParseInt32FromHashtable(settings, "RSSFeedEntryCacheTimeoutSetting", EntryCacheTimeout);
			MaxDaysOld = WebUtils.ParseInt32FromHashtable(settings, "RSSFeedMaxDayCountSetting", MaxDaysOld);
			MaxEntriesPerFeed = WebUtils.ParseInt32FromHashtable(settings, "RSSFeedMaxPostsPerFeedSetting", MaxEntriesPerFeed);
			ShowItemDetail = !WebUtils.ParseBoolFromHashtable(settings, "RSSFeedShowHeadingsOnlySetting", false);
			UseCalendar = WebUtils.ParseBoolFromHashtable(settings, "RSSFeedUseCalendarView", UseCalendar);
			UseFillerOnEmptyDays = WebUtils.ParseBoolFromHashtable(settings, "RSSFeedPadEmptyDaysInCalendarView", UseFillerOnEmptyDays);
			SortAscending = WebUtils.ParseBoolFromHashtable(settings, "RSSFeedSortAscending", SortAscending);
			AllowExternalImages = WebUtils.ParseBoolFromHashtable(settings, "RSSFeedAllowExternalImages", AllowExternalImages);
			EnableSelectivePublishing = WebUtils.ParseBoolFromHashtable(settings, "EnableSelectivePublishing", EnableSelectivePublishing);
			EnableInPlacePublishing = WebUtils.ParseBoolFromHashtable(settings, "EnableInPlacePublishing", EnableInPlacePublishing);
			PageSize = WebUtils.ParseInt32FromHashtable(settings, "RSSAggregatorPageSizeSetting", PageSize);
			ShowAuthor = WebUtils.ParseBoolFromHashtable(settings, "RSSAggregatorShowAuthor", ShowAuthor);
			ShowFeedName = WebUtils.ParseBoolFromHashtable(settings, "RSSAggregatorShowFeedName", ShowFeedName);
			ShowFeedNameBeforeContent = WebUtils.ParseBoolFromHashtable(settings, "RSSAggregatorShowFeedNameBeforeContent", ShowFeedNameBeforeContent);
			UseExcerpt = WebUtils.ParseBoolFromHashtable(settings, "RSSAggregatorExcerptSetting", UseExcerpt);
			ExcerptLength = WebUtils.ParseInt32FromHashtable(settings, "RSSAggregatorExcerptLengthSetting", ExcerptLength);

			if (settings.Contains("RSSAggregatorExcerptSuffixSetting"))
			{
				ExcerptSuffix = settings["RSSAggregatorExcerptSuffixSetting"].ToString();
			}

			ShowFeedListOnRight = WebUtils.ParseBoolFromHashtable(settings, "RSSFeedShowFeedListRightSetting", ShowFeedListOnRight);
			UseFeedListAsFilter = WebUtils.ParseBoolFromHashtable(settings, "RSSFeedUseFeedListAsFilterSetting", UseFeedListAsFilter);
			ShowAggregateFeedLink = WebUtils.ParseBoolFromHashtable(settings, "RSSFeedShowAggregateFeedLink", ShowAggregateFeedLink);
			ShowIndividualFeedLinks = WebUtils.ParseBoolFromHashtable(settings, "RSSFeedShowIndividualFeedLinks", ShowIndividualFeedLinks);
			UseNeatHtml = WebUtils.ParseBoolFromHashtable(settings, "FilterInvalidMarkupAndPotentiallyInsecureContent", UseNeatHtml);

			if (settings.Contains("CustomCssClassSetting"))
			{
				InstanceCssClass = settings["CustomCssClassSetting"].ToString();
			}

			if (settings.Contains("RSSFeedListLabelSetting"))
			{
				ListLabel = settings["RSSFeedListLabelSetting"].ToString();
			}

			UseExcerptSuffixAsLinkToPost = WebUtils.ParseBoolFromHashtable(settings, "FeedUseExcerptSuffixAsLinkToPost", UseExcerptSuffixAsLinkToPost);
			ShowDate = WebUtils.ParseBoolFromHashtable(settings, "ShowDateSetting", ShowDate);
			UseScroller = WebUtils.ParseBoolFromHashtable(settings, "UseScrollerSetting", UseScroller);
			OpenLinkInNewWindow = WebUtils.ParseBoolFromHashtable(settings, "OpenLinkInNewWindow", OpenLinkInNewWindow);
			UseAutoDiscoveryAggregateFeedLink = WebUtils.ParseBoolFromHashtable(settings, "UseAutoDiscoveryAggregateFeedLink", UseAutoDiscoveryAggregateFeedLink);
		}
	}
}
