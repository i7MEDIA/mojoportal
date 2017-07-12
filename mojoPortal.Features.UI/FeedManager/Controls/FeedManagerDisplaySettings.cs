// Author:					
// Created:				    2011-06-09
// Last Modified:			2014-06-10
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
//
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.FeedUI
{
    /// <summary>
    /// this control doesn't render anything, it is used only as a themeable collection of settings for things we would like to be able to configure from theme.skin
    /// </summary>
    public class FeedManagerDisplaySettings : WebControl
    {
        private string dateFormat = string.Empty;

        public string DateFormat
        {
            get { return dateFormat; }
            set { dateFormat = value; }
        }

        private bool disableUseFeedListAsFilter = false;

        public bool DisableUseFeedListAsFilter
        {
            get { return disableUseFeedListAsFilter; }
            set { disableUseFeedListAsFilter = value; }
        }

        private bool disableShowAggregateFeedLink = false;

        public bool DisableShowAggregateFeedLink
        {
            get { return disableShowAggregateFeedLink; }
            set { disableShowAggregateFeedLink = value; }
        }

        private bool disableRepeatColumns = false;

        public bool DisableRepeatColumns
        {
            get { return disableRepeatColumns; }
            set { disableRepeatColumns = value; }
        }

        private bool disableShowIndividualFeedLinks = false;

        public bool DisableShowIndividualFeedLinks
        {
            get { return disableShowIndividualFeedLinks; }
            set { disableShowIndividualFeedLinks = value; }
        }

        private bool disableUseCalendar = false;

        public bool DisableUseCalendar
        {
            get { return disableUseCalendar; }
            set { disableUseCalendar = value; }
        }

        private bool disableScroller = false;

        public bool DisableScroller
        {
            get { return disableScroller; }
            set { disableScroller = value; }
        }

        private bool forceExcerptMode = false;

        public bool ForceExcerptMode
        {
            get { return forceExcerptMode; }
            set { forceExcerptMode = value; }
        }

        private bool forceShowHeadingsOnly = false;

        public bool ForceShowHeadingsOnly
        {
            get { return forceShowHeadingsOnly; }
            set { forceShowHeadingsOnly = value; }
        }

        private bool useNoFollowOnHeadingLinks = false;

        public bool UseNoFollowOnHeadingLinks
        {
            get { return useNoFollowOnHeadingLinks; }
            set { useNoFollowOnHeadingLinks = value; }
        }

        private bool useNoFollowOnFeedSiteLinks = false;

        public bool UseNoFollowOnFeedSiteLinks
        {
            get { return useNoFollowOnFeedSiteLinks; }
            set { useNoFollowOnFeedSiteLinks = value; }
        }

        private bool disableUseAutoDiscoveryAggregateFeedLink = false;

        /// <summary>
        /// disables adding the feed link in the head of the page
        /// </summary>
        public bool DisableUseAutoDiscoveryAggregateFeedLink
        {
            get { return disableUseAutoDiscoveryAggregateFeedLink; }
            set { disableUseAutoDiscoveryAggregateFeedLink = value; }
        }

        private int overridePageSize = 0; // only used if greater than 0

        public int OverridePageSize
        {
            get { return overridePageSize; }
            set { overridePageSize = value; }
        }

        private bool useBottomNavForRight = false;

        public bool UseBottomNavForRight
        {
            get { return useBottomNavForRight; }
            set { useBottomNavForRight = value; }
        }

        private bool useUlForSingleColumn = true;

        public bool UseUlForSingleColumn
        {
            get { return useUlForSingleColumn; }
            set { useUlForSingleColumn = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            // nothing to render
        }
    }
}