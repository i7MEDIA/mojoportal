/// Author:				
/// Created:			2005-03-27
///	Last Modified:		2018-01-02
///	
///	Originally based on code example by Joseph Hill
///	
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.
/// 
/// 2008-09-08 Walter Ferrari added support for excerpt, paging, show author
/// 2008-09-30  moved this feature from main projects into mojoPortal.Features projects
/// 2008-10-01 integrated selective publishing option from Walter Ferrari
/// 2010-01-09 implemented alternate calendar view to allow showing a minio calendar consuming feeds from Event Calendar Pro for example
/// 2010-11-26 added support for a jquery scroller widget to make the feed posts scroll like a newsticker
/// 2013-02-03 added some css classes, made option for right nav to render at the bottom, single column will render as ul instead of table

using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Web.FeedUI
{
    public partial class FeedManagerModule : SiteModuleControl
    {
        #region Properties 

        private int totalPages = 1;
        private DataTable dtFeedList = null;
        protected string allowedImageUrlRegexPattern = SecurityHelper.RegexRelativeImageUrlPatern;
        protected FeedManagerConfiguration config = new FeedManagerConfiguration();
        protected string RssImageFile = WebConfigSettings.RSSImageFileName;
        protected string EditContentImage = WebConfigSettings.EditContentImage;
        protected bool LinkToAuthorSite = true;
        protected bool ShowItemDetail = true;
        protected string ConfirmImage = string.Empty;
        protected string ErrorMessage = string.Empty;
        protected Double timeZoneOffset = 0;
        private TimeZoneInfo timeZone = null;
        protected string FeedItemHeadingElement = FeedManagerConfiguration.FeedItemHeadingElement;
        protected bool useNeatHtml = true;
        private string dateFormat = string.Empty;
        
        protected int ItemID
        {
            get
            {
                if (ViewState["ItemID"] != null)
                {
                    return Convert.ToInt32(ViewState["ItemID"]);
                }
                return -1;
            }
            set { ViewState["ItemID"] = value; }

        }
        
        protected bool EnableInPlaceEditing
        {
            get { return (this.IsEditable && config.EnableInPlacePublishing && config.EnableSelectivePublishing); }
        }

        private bool didBind = false;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            SetupRssLink();
            PopulateLabels();
            PopulateControls();

            if (!IsPostBack)
            {
                if (config.UseCalendar && !displaySettings.DisableUseCalendar)
                {
                    
                    BindCalendar();
                }
                else
                {
                    pgrRptEntries.CurrentIndex = 1;
                    BindRepeater();
                }
            }


        }

        
        private void PopulateControls()
        {
            if (!config.ShowFeedListOnRight)
            {
                divNav.CssClass = "rssnavleft";
                divFeedEntries.CssClass = "rsscenter-leftnav rssentries";

            }

            
            
            if (dtFeedList == null) { dtFeedList = RssFeed.GetFeeds(ModuleId); }

            DataView members = dtFeedList.DefaultView;
            //members.Sort = "Author";

            if (config.UseFeedListAsFilter && !displaySettings.DisableUseFeedListAsFilter)
            {
                if ((ItemID == -1)&&( members.Table.Rows.Count > 0))
                {
                    ItemID = Convert.ToInt32(members.Table.Rows[0]["ItemID"]);

                }
                BindSelectedFeed();

            }
            
            string rssFriendlyUrl = SiteRoot
                    + "/aggregator" + ModuleId.ToInvariantString() + "rss.aspx";

            if (config.ShowAggregateFeedLink && !displaySettings.DisableShowAggregateFeedLink)
            {
                lnkAggregateRSS.HRef = rssFriendlyUrl;
                imgAggregateRSS.Src = ImageSiteRoot + "/Data/SiteImages/" + RssImageFile;

                lnkAggregateRSSBottom.HRef = rssFriendlyUrl;
                imgAggregateRSSBottom.Src = ImageSiteRoot + "/Data/SiteImages/" + RssImageFile;
            }
            else
            {
                lnkAggregateRSS.Visible = false;
                lnkAggregateRSSBottom.Visible = false;
            }

            if ((config.RepeatColumns > 1)&&(!displaySettings.DisableRepeatColumns))
            {
                dlstFeedList.RepeatDirection = RepeatDirection.Horizontal;
                dlstFeedList.RepeatColumns = config.RepeatColumns;
            }

            if (config.ShowIndividualFeedLinks && !displaySettings.DisableShowIndividualFeedLinks)
            {
                if ((config.ShowFeedListOnRight) && (displaySettings.UseBottomNavForRight))
                {
                    divNav.Visible = false;
                    divNavBottom.Visible = true;
                    if ((displaySettings.UseUlForSingleColumn) && (config.RepeatColumns == 1))
                    {
                        rptFeedListBottom.DataSource = members;
                        rptFeedListBottom.DataBind();
                        dlFeedListBottom.Visible = false;

                    }
                    else
                    {
                        rptFeedListBottom.Visible = false;
                        dlFeedListBottom.DataSource = members;
                        dlFeedListBottom.DataBind();
                    }

                }
                else
                {
                    divNav.Visible = true;
                    divNavBottom.Visible = false;

                    if ((displaySettings.UseUlForSingleColumn) && (config.RepeatColumns == 1))
                    {
                        rptFeedListTop.DataSource = members;
                        rptFeedListTop.DataBind();
                        dlstFeedList.Visible = false;

                    }
                    else
                    {
                        rptFeedListTop.Visible = false;
                        dlstFeedList.DataSource = members;
                        dlstFeedList.DataBind();
                    }
                }
            }

            if (
                ((!config.ShowAggregateFeedLink)
                && (!config.ShowIndividualFeedLinks)
                )
                || (this.RenderInWebPartMode)
                || (displaySettings.DisableShowIndividualFeedLinks)
                )
            {
                divNav.Visible = false;
                divNavBottom.Visible = false;
                divFeedEntries.CssClass = "rssentries";
            }

            if ((config.UseScroller)&&(!displaySettings.DisableScroller))
            {
                pnlInnerWrap.CssClass += " feedscroller";
                SetupScrollerScript();
            }

        }

        private void BindSelectedFeed()
        {
            RssFeed feed = new RssFeed(ModuleId, ItemID);
            this.lblFeedHeading.Visible = true;
            this.lblFeedHeading.Text = "<h2>" + feed.Author + "</h2>";

        }

        private DataTable GetEntriesTable()
        {
           
            DataTable entriesTable = FeedCache.GetRssFeedEntries(
                ModuleId,
                ModuleGuid,
                config.EntryCacheTimeout,
                config.MaxDaysOld,
                config.MaxEntriesPerFeed,
                config.EnableSelectivePublishing);
           

            return entriesTable;
        }

        private void BindCalendar()
        {
            pgrRptEntries.Visible = false;
            dataCal1.Visible = true;
            DataTable entries = GetEntriesTable();
            LocalizeTimes(entries);
            dataCal1.DataSource = entries;
            dataCal1.DataBind();

            didBind = true;

        }

        private void LocalizeTimes(DataTable dt)
        {
            if ((timeZoneOffset == 0)&&(timeZone == null)) { return; }

            //2010-05-20 15:45:36.000

            foreach (DataRow row in dt.Rows)
            {
                if (timeZone != null)
                {
                    row["PubDate"] = Convert.ToDateTime(row["PubDate"]).ToLocalTime(timeZone);
                }
                else if (timeZoneOffset > 0)
                {
                    row["PubDate"] = Convert.ToDateTime(row["PubDate"]).AddHours(timeZoneOffset);
                }
            }

        }

        void dataCal1_SelectionChanged(object sender, EventArgs e)
        {
            BindCalendar();
        }

        void dataCal1_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            BindCalendar();
        }


        private void BindRepeater()
        {
            dataCal1.Visible = false;
            rptEntries.Visible = true;
            DataView entries = GetEntriesTable().DefaultView;

            // filter just entries confirmed to end users
            if (!this.EnableInPlaceEditing)
            {
                entries.RowFilter = "Confirmed = true";
            }
            else
            {
                entries.RowFilter = string.Empty;
            }

            if (!config.EnableSelectivePublishing)
            {
                entries.RowFilter = string.Empty;
            }

            if ((config.UseFeedListAsFilter) && (ItemID > -1))
            {
                if(entries.RowFilter == string.Empty)
                {
                    entries.RowFilter = "FeedId = " + ItemID.ToInvariantString();
                }
                else
                {
                    entries.RowFilter += " AND FeedId = " + ItemID.ToInvariantString();
                }
            }

            if (config.SortAscending)
            {
                entries.Sort = "PubDate ASC";
            }
            else
            {
                entries.Sort = "PubDate DESC";
            }

            int pageSize = config.PageSize;
            if (displaySettings.OverridePageSize > 0)
            {
                pageSize = displaySettings.OverridePageSize;
            }

            PagedDataSource pagedDS = new PagedDataSource();

            pagedDS.DataSource = entries;
            pagedDS.AllowPaging = true;
            pagedDS.PageSize = pageSize;
            pagedDS.CurrentPageIndex = pgrRptEntries.CurrentIndex - 1;

            totalPages = 1;
            int totalRows = entries.Count;

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            if (this.totalPages > 1)
            {

                pgrRptEntries.ShowFirstLast = true;
                pgrRptEntries.PageSize = pageSize;
                pgrRptEntries.PageCount = totalPages;
            }
            else
            {
                pgrRptEntries.Visible = false;
            }

            if (config.UseScroller) { pgrRptEntries.Visible = false; }

            rptEntries.DataSource = pagedDS;
            rptEntries.DataBind();

            didBind = true;

        }

        protected void pgrRptEntries_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            pgrRptEntries.CurrentIndex = currentPageIndex;
            BindRepeater();
            //updPnlRSSA.Update();
            updEntries.Update();
        }


        protected void rptEntries_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Confirm")
            {
                string entryInfo = (string)e.CommandArgument;
                int sep = -1;
                sep = entryInfo.IndexOf('_');
                if (sep != -1)
                {
                    string[] entryHash = entryInfo.Split('_');
                    bool published = Convert.ToBoolean(entryHash[1]);
                    if (published)
                    {
                        RssFeed.UnPublish(ModuleGuid, Convert.ToInt32(entryHash[0]));
                    }
                    else
                    {
                        RssFeed.Publish(ModuleGuid, Convert.ToInt32(entryHash[0]));
                    }
                    BindRepeater();

                }

            }
        }

        void rptFeedListTop_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "filter")
            {
                ItemID = Convert.ToInt32(e.CommandArgument);
                pgrRptEntries.CurrentIndex = 1;
                BindSelectedFeed();
                BindRepeater();
            }

        }

        void dlstFeedList_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "filter")
            {
                ItemID = Convert.ToInt32(e.CommandArgument);
                pgrRptEntries.CurrentIndex = 1;
                BindSelectedFeed();
                BindRepeater();
            }
            
        }

        protected string GetDateHeader(DateTime pubDate)
        { 
            // adjust from GMT to user time zone
            

            if (timeZone == null)
            {
                return pubDate.AddHours(timeZoneOffset).ToString(dateFormat);
            }

            return pubDate.ToLocalTime(timeZone).ToString(dateFormat);

        }

        protected string FormatBody(string postBody, string postUrl)
        {
            if(!config.UseExcerpt && !displaySettings.ForceExcerptMode) { return postBody; }

            if (config.UseExcerptSuffixAsLinkToPost)
            {
                string moreLinkText = config.ExcerptSuffix;
                if (string.IsNullOrEmpty(moreLinkText))
                {
                    moreLinkText = FeedResources.ReadMoreLink;
                }

                string onclick = string.Empty;
                
                if (config.OpenLinkInNewWindow) 
                { 
                    onclick = " target='_blank'"; 

                }
                

                return UIHelper.CreateExcerpt(postBody, config.ExcerptLength, "<a class='morelink' href='" + postUrl + "'" + onclick + ">" + moreLinkText + "</a>");
            }

            return UIHelper.CreateExcerpt(postBody, config.ExcerptLength, "<span class='excerptsuffix'>" + config.ExcerptSuffix +"</span>");

        }

        protected string FormatTitle(string link, string title)
        {
            string noFollow = string.Empty;
            if(displaySettings.UseNoFollowOnHeadingLinks)
            {
                noFollow = " rel='nofollow' ";
            }

            return "<" + FeedItemHeadingElement + "><a " + noFollow + " href='" 
                + SecurityHelper.SanitizeHtml(link) 
                + "' " + GetOnClick() + ">" + SecurityHelper.SanitizeHtml(title)
                + "</a></" + FeedItemHeadingElement + ">";
        }

        protected string GetOnClick()
        {
            //if ((config.UseExcerpt) &&(config.OpenLinkInNewWindow))
            if (config.OpenLinkInNewWindow)
            {
                //return "onclick=\"window.open(this.href,'_blank');return false;\"";
                return "target=\"_blank\"";
            }

            return string.Empty;
        }

        private void PopulateLabels()
        {
            Title1.EditUrl = SiteRoot + "/FeedManager/FeedEdit.aspx";
            Title1.EditText = FeedResources.AddButton;
            Title1.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            if ((IsEditable) && (config.EnableSelectivePublishing) && (!EnableInPlaceEditing))
            {
                Title1.LiteralExtraMarkup = "&nbsp;<a href='"
                    + SiteRoot
                    + "/FeedManager/FeedManager.aspx?pageid=" + PageId.ToInvariantString()
                    + "&amp;mid=" + ModuleId.ToInvariantString()
                    + "' class='ModuleEditLink' title='" + FeedResources.ManagePublishingLink + "'>" + FeedResources.ManagePublishingLink + "</a>";
            }

        }

        private void SetupScrollerScript()
        {
            if (IsPostBack) { return; }
            // the main script for the scroller is included in mojocombinedfull.js

            StringBuilder script = new StringBuilder();
            pnlInnerBody.RenderId = true; // added in case it was turned off in the theme.skin
            script.Append("$(document).ready(function() { ");
            //script.Append("$('.rssfeedmodule').find('.rsswrapper').SetScroller({ velocity: 60, ");
            script.Append("$('#" + pnlInnerBody.ClientID + "').SetScroller({ velocity: 60, ");
            script.Append("direction: 'vertical',");
            script.Append("startfrom: 'bottom', ");
            script.Append("loop: 'infinite', ");
            script.Append("movetype: 'linear', ");
            script.Append("onmouseover: 'pause', ");
            script.Append("onmouseout: 'play', ");
            script.Append("onstartup: 'play', ");
            script.Append("cursor: 'pointer' ");
            script.Append("}); ");
            script.Append("}); ");
            
            ScriptManager.RegisterStartupScript(this, typeof(Page),
                   "scroller" + ModuleId.ToInvariantString(), "\n<script type=\"text/javascript\" >"
                   + script.ToString() + "</script>", false);

        }

        private void LoadSettings()
        {
            timeZoneOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
#if NET35
            if (WebConfigSettings.DisablePageViewStateByDefault) {Page.EnableViewState = true; }
#endif
            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }

            //pnlContainer.ModuleId = ModuleId;

            lnkAggregateRSS.Attributes.Add("rel", "nofollow");

            config = new FeedManagerConfiguration(Settings);

            dateFormat = config.DateFormat;
            if (displaySettings.DateFormat.Length > 0) //allow theme override for mobile
            {
                dateFormat = displaySettings.DateFormat;
            }

            useNeatHtml = config.UseNeatHtml;
            // when using excerpt mode we don't need the protection of neathtml because we are already removing the markup
            // using it in addition was preventing opening the read more link in a new window
            if (displaySettings.ForceExcerptMode) { useNeatHtml = false; }
            if (config.UseExcerpt) { useNeatHtml = false; }

            ConfirmImage = this.ImageSiteRoot + "/Data/SiteImages/";

            if (config.AllowExternalImages) allowedImageUrlRegexPattern = SecurityHelper.RegexAnyImageUrlPatern;

            if (RenderInWebPartMode)
            {
                ShowItemDetail = false;
            }

            if (config.ShowErrorMessageOnInvalidPosts)
            {
                ErrorMessage = FeedResources.MalformedMarkupWarning;
            }

            LinkToAuthorSite = !config.UseFeedListAsFilter || displaySettings.DisableUseFeedListAsFilter;



            if (config.ListLabel.Length > 0)
            {
                lblFeedListName.Text = config.ListLabel;
                lblFeedListNameBottom.Text = config.ListLabel;

            }
            else
            {
                lblFeedListName.Visible = false;
                lblFeedListNameBottom.Visible = false;
            }


            if (config.InstanceCssClass.Length > 0) { pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass); }

            if ((config.UseScroller) && (!displaySettings.DisableScroller))
            {
                mojoBasePage basePage = Page as mojoBasePage;
                if (basePage != null) { basePage.ScriptConfig.IncludeJqueryScroller = true; }
            }

            //if (!config.EnableInPlacePublishing)
            //{
            //    rptEntries.EnableViewState = false;
            //}

            //if (!config.UseFeedListAsFilter)
            //{
            //    dlstFeedList.EnableViewState = false;
            //}



           
        }

        protected virtual void SetupRssLink()
        {
            if (!config.UseAutoDiscoveryAggregateFeedLink) { return; }
            if (displaySettings.DisableUseAutoDiscoveryAggregateFeedLink) { return; }
            if (IsPostBack) { return; }

            if (ModuleConfiguration != null)
            {
                if (Page.Master != null)
                {
                    Control head = Page.Master.FindControl("Head1");
                    if (head != null)
                    {
                        string rssFriendlyUrl = ResolveUrl(SiteRoot + "/aggregator" + ModuleId.ToInvariantString() + "rss.aspx");
                        Literal rssLink = new Literal();
                        rssLink.Text = "<link rel=\"alternate\" type=\"application/rss+xml\" title=\""
                                + ModuleConfiguration.ModuleTitle + "\" href=\""
                                + rssFriendlyUrl + "\" />";

                        head.Controls.Add(rssLink);

                    }

                }
            }

        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!didBind)
            {
                if (config.UseCalendar && !displaySettings.DisableUseCalendar)
                {

                    BindCalendar();
                }
                else
                {
                    pgrRptEntries.CurrentIndex = 1;
                    BindRepeater();
                }
            }

            base.OnPreRender(e);
        }
        
        
        protected override void OnInit(EventArgs e)
        {
            
            this.Load += new EventHandler(Page_Load);
            rptEntries.ItemCommand += new RepeaterCommandEventHandler(rptEntries_ItemCommand);
            pgrRptEntries.Command += new CommandEventHandler(pgrRptEntries_Command);
            dlstFeedList.ItemCommand += new DataListCommandEventHandler(dlstFeedList_ItemCommand);

            rptFeedListTop.ItemCommand += rptFeedListTop_ItemCommand;

            dataCal1.VisibleMonthChanged += new MonthChangedEventHandler(dataCal1_VisibleMonthChanged);
            dataCal1.SelectionChanged += new EventHandler(dataCal1_SelectionChanged);
            base.OnInit(e);
        }

        

        

    }
}