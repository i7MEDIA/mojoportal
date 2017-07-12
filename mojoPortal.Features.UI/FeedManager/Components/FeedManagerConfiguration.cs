// Author:				    
// Created:			        2010-06-01
// Last Modified:		    2013-02-03
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.FeedUI
{
    public class FeedManagerConfiguration
    {
        public FeedManagerConfiguration()
        { }

        public FeedManagerConfiguration(Hashtable settings)
        {
            LoadSettings(settings);

        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

            if (settings.Contains("FeedDateFormatSetting"))
            {
                dateFormat = settings["FeedDateFormatSetting"].ToString().Trim();
                if (dateFormat.Length > 0)
                {
                    try
                    {
                        string d = DateTime.Now.ToString(dateFormat, CultureInfo.CurrentCulture);
                    }
                    catch (FormatException)
                    {
                        dateFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
                    }
                }
                else
                {
                    dateFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
                }
            }

            feedListCacheTimeout = WebUtils.ParseInt32FromHashtable(settings, "RSSFeedFeedListCacheTimeoutSetting", feedListCacheTimeout);

            repeatColumns = WebUtils.ParseInt32FromHashtable(settings, "RSSFeedFeedListColumnsSetting", repeatColumns);

            entryCacheTimeout = WebUtils.ParseInt32FromHashtable(settings, "RSSFeedEntryCacheTimeoutSetting", entryCacheTimeout);

            maxDaysOld = WebUtils.ParseInt32FromHashtable(settings, "RSSFeedMaxDayCountSetting", maxDaysOld);

            maxEntriesPerFeed = WebUtils.ParseInt32FromHashtable(settings, "RSSFeedMaxPostsPerFeedSetting", maxEntriesPerFeed);

            showItemDetail = !WebUtils.ParseBoolFromHashtable(settings, "RSSFeedShowHeadingsOnlySetting", false);

            useCalendar = WebUtils.ParseBoolFromHashtable(settings, "RSSFeedUseCalendarView", useCalendar);

            useFillerOnEmptyDays = WebUtils.ParseBoolFromHashtable(settings, "RSSFeedPadEmptyDaysInCalendarView", useFillerOnEmptyDays);


            sortAscending = WebUtils.ParseBoolFromHashtable(settings, "RSSFeedSortAscending", sortAscending);


            allowExternalImages = WebUtils.ParseBoolFromHashtable(settings, "RSSFeedAllowExternalImages", allowExternalImages);

            enableSelectivePublishing = WebUtils.ParseBoolFromHashtable(settings, "EnableSelectivePublishing", enableSelectivePublishing);

            enableInPlacePublishing = WebUtils.ParseBoolFromHashtable(settings, "EnableInPlacePublishing", enableInPlacePublishing);

            pageSize = WebUtils.ParseInt32FromHashtable(settings, "RSSAggregatorPageSizeSetting", pageSize);

            showAuthor = WebUtils.ParseBoolFromHashtable(settings, "RSSAggregatorShowAuthor", showAuthor);

            showFeedName = WebUtils.ParseBoolFromHashtable(settings, "RSSAggregatorShowFeedName", showFeedName);

            showFeedNameBeforeContent = WebUtils.ParseBoolFromHashtable(settings, "RSSAggregatorShowFeedNameBeforeContent", showFeedNameBeforeContent);

            useExcerpt = WebUtils.ParseBoolFromHashtable(settings, "RSSAggregatorExcerptSetting", useExcerpt);

            excerptLength = WebUtils.ParseInt32FromHashtable(settings, "RSSAggregatorExcerptLengthSetting", excerptLength);

            if (settings.Contains("RSSAggregatorExcerptSuffixSetting"))
            {
                excerptSuffix = settings["RSSAggregatorExcerptSuffixSetting"].ToString();
            }

            showFeedListOnRight = WebUtils.ParseBoolFromHashtable(settings, "RSSFeedShowFeedListRightSetting", showFeedListOnRight);

            useFeedListAsFilter = WebUtils.ParseBoolFromHashtable(settings, "RSSFeedUseFeedListAsFilterSetting", useFeedListAsFilter);

            //LinkToAuthorSite = !UseFeedListAsFilter;

            showAggregateFeedLink = WebUtils.ParseBoolFromHashtable(settings, "RSSFeedShowAggregateFeedLink", showAggregateFeedLink);

            showIndividualFeedLinks = WebUtils.ParseBoolFromHashtable(settings, "RSSFeedShowIndividualFeedLinks", showIndividualFeedLinks);

            useNeatHtml = WebUtils.ParseBoolFromHashtable(settings, "FilterInvalidMarkupAndPotentiallyInsecureContent", useNeatHtml);

            if (settings.Contains("CustomCssClassSetting"))
            {
                instanceCssClass = settings["CustomCssClassSetting"].ToString();
            }

            if (settings.Contains("RSSFeedListLabelSetting"))
            {
                listLabel = settings["RSSFeedListLabelSetting"].ToString();
            }

            useExcerptSuffixAsLinkToPost = WebUtils.ParseBoolFromHashtable(settings, "FeedUseExcerptSuffixAsLinkToPost", useExcerptSuffixAsLinkToPost);

            showDate = WebUtils.ParseBoolFromHashtable(settings, "ShowDateSetting", showDate);

            useScroller = WebUtils.ParseBoolFromHashtable(settings, "UseScrollerSetting", useScroller);

            openLinkInNewWindow = WebUtils.ParseBoolFromHashtable(settings, "OpenLinkInNewWindow", openLinkInNewWindow);

            useAutoDiscoveryAggregateFeedLink = WebUtils.ParseBoolFromHashtable(settings, "UseAutoDiscoveryAggregateFeedLink", useAutoDiscoveryAggregateFeedLink);

        }

        private bool useAutoDiscoveryAggregateFeedLink = true;

        public bool UseAutoDiscoveryAggregateFeedLink
        {
            get { return useAutoDiscoveryAggregateFeedLink; }
        }

        private bool openLinkInNewWindow = false;

        public bool OpenLinkInNewWindow
        {
            get { return openLinkInNewWindow; }
        }

        private bool showDate = true;

        public bool ShowDate
        {
            get { return showDate; }
        }

        private bool useExcerptSuffixAsLinkToPost = false;

        public bool UseExcerptSuffixAsLinkToPost
        {
            get { return useExcerptSuffixAsLinkToPost; }
        }

        private string listLabel = string.Empty;

        public string ListLabel
        {
            get { return listLabel; }
        }

        private string instanceCssClass = string.Empty;

        public string InstanceCssClass
        {
            get { return instanceCssClass; }
        }

        private string dateFormat = string.Empty;

        public string DateFormat
        {
            get { return dateFormat; }
        }

        private bool useFeedListAsFilter = false;

        public bool UseFeedListAsFilter
        {
            get { return useFeedListAsFilter; }
        }

        private int feedListCacheTimeout = 3660;

        public int FeedListCacheTimeout
        {
            get { return feedListCacheTimeout; }
        }

        private int entryCacheTimeout = 3620;

        public int EntryCacheTimeout
        {
            get { return entryCacheTimeout; }
        }

        private int maxDaysOld = 90;

        public int MaxDaysOld
        {
            get { return maxDaysOld; }
        }

        private int maxEntriesPerFeed = 90;

        public int MaxEntriesPerFeed
        {
            get { return maxEntriesPerFeed; }
        }


        private bool allowExternalImages = false;

        public bool AllowExternalImages
        {
            get { return allowExternalImages; }
        }

        private bool showFeedListOnRight = true;

        public bool ShowFeedListOnRight
        {
            get { return showFeedListOnRight; }
        }

        private int repeatColumns = 1;

        public int RepeatColumns
        {
            get { return repeatColumns; }
        }

        private bool showAggregateFeedLink = true;

        public bool ShowAggregateFeedLink
        {
            get { return showAggregateFeedLink; }
        }

        private bool showErrorMessageOnInvalidPosts = false;

        public bool ShowErrorMessageOnInvalidPosts
        {
            get { return showErrorMessageOnInvalidPosts; }
        }

        private bool linkToAuthorSite = true;

        public bool LinkToAuthorSite
        {
            get { return linkToAuthorSite; }
        }

        private bool sortAscending = false;

        public bool SortAscending
        {
            get { return sortAscending; }
        }

        private bool showAuthor = false;

        public bool ShowAuthor
        {
            get { return showAuthor; }
        }

        private bool showFeedName = false;

        public bool ShowFeedName
        {
            get { return showFeedName; }
        }

        private bool showFeedNameBeforeContent = false;

        public bool ShowFeedNameBeforeContent
        {
            get { return showFeedNameBeforeContent; }
        }

        private bool useExcerpt = false;

        public bool UseExcerpt
        {
            get { return useExcerpt; }
        }

        private bool enableSelectivePublishing = false;

        public bool EnableSelectivePublishing
        {
            get { return enableSelectivePublishing; }
        }

        private bool enableInPlacePublishing = false;

        public bool EnableInPlacePublishing
        {
            get { return enableInPlacePublishing; }
        }

        private int excerptLength = 250;

        public int ExcerptLength
        {
            get { return excerptLength; }
        }

        private string excerptSuffix = "...";

        public string ExcerptSuffix
        {
            get { return excerptSuffix; }
        }

        private bool showIndividualFeedLinks = true;

        public bool ShowIndividualFeedLinks
        {
            get { return showIndividualFeedLinks; }
        }

        private bool showItemDetail = true;

        public bool ShowItemDetail
        {
            get { return showItemDetail; }
        }

        private bool useFillerOnEmptyDays = false;

        public bool UseFillerOnEmptyDays
        {
            get { return useFillerOnEmptyDays; }
        }

        private bool useCalendar = false;

        public bool UseCalendar
        {
            get { return useCalendar; }
        }

        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
        }


        private bool useNeatHtml = true;

        public bool UseNeatHtml
        {
            get { return useNeatHtml; }
        }

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

        private bool useScroller = false;

        public bool UseScroller
        {
            get { return useScroller; }
        }



        public static bool UseReadWriteLockForCacheMenagement
        {
            get { return ConfigHelper.GetBoolProperty("FeedManager:UseReadWriteLockForCacheMenagement", false); }
        }

    }
}