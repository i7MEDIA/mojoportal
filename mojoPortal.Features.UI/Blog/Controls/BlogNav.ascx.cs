//	Author:				
//	Created:			2011-06-09
//	Last Modified:		2017-06-20
//		
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using System;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.BlogUI
{
	public partial class BlogNav : UserControl
	{
		#region Properties

		protected BlogConfiguration config = null;
		private int pageId = -1;
		private int moduleId = -1;
		private Guid moduleGuid = Guid.Empty;
		private bool isEditable = false;
		private string siteRoot = string.Empty;
		private string imageSiteRoot = string.Empty;
		private int countOfDrafts = 0;
		private bool showCommentCount = true;
		private bool showCalendar = true;

		protected DateTime CalendarDate;
		protected DateTime navDate;

		private DateTime overrideDate = DateTime.MinValue;

		public DateTime OverrideDate
		{
			get { return overrideDate; }
			set { overrideDate = value; }
		}


		public int PageId
		{
			get { return pageId; }
			set { pageId = value; }
		}

		public int ModuleId
		{
			get { return moduleId; }
			set { moduleId = value; }
		}

		public Guid ModuleGuid
		{
			get { return moduleGuid; }
			set { moduleGuid = value; }
		}

		public int CountOfDrafts
		{
			get { return countOfDrafts; }
			set { countOfDrafts = value; }
		}

		public string SiteRoot
		{
			get { return siteRoot; }
			set { siteRoot = value; }
		}

		public string ImageSiteRoot
		{
			get { return imageSiteRoot; }
			set { imageSiteRoot = value; }
		}

		public BlogConfiguration Config
		{
			get { return config; }
			set { config = value; }
		}

		public bool ShowCalendar
		{
			get { return showCalendar; }
			set { showCalendar = value; }
		}

		public bool ShowCommentCount
		{
			get { return showCommentCount; }
			set { showCommentCount = value; }
		}

		public bool IsEditable
		{
			get { return isEditable; }
			set { isEditable = value; }
		}

		#endregion


		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Visible) { return; }

			LoadSettings();
			PopulateLabels();
			if (!Page.IsPostBack)
			{
				PopulateNavigation();
			}
		}

		protected virtual void PopulateNavigation()
		{
			bool isDetailPage = (Page is BlogView);

			pnlSideTop.Visible = false;
			pnlSideBottom.Visible = false;

			if (!String.IsNullOrWhiteSpace(config.UpperSidebar))
			{
				pnlSideTop.Visible = true;
				litUpperSidebar.Text = config.UpperSidebar;
			}

			if (!String.IsNullOrWhiteSpace(config.LowerSidebar))
			{
				pnlSideBottom.Visible = true;
				litLowerSidebar.Text = config.LowerSidebar;
			}

			Feeds.Config = config;
			Feeds.PageId = PageId;
			Feeds.ModuleId = ModuleId;
			bool showFeeds = config.ShowFeedLinks && !displaySettings.HideFeedLinks;
			if (isDetailPage && displaySettings.ShowFeedLinksInPostDetail) { showFeeds = true; }

			Feeds.Visible = showFeeds;

			
			bool showCategories = config.ShowCategories && !displaySettings.DisableShowCategories;
			if (isDetailPage && displaySettings.ShowCategoriesInPostDetail) { showCategories = true; }

			if (showCategories)
			{
				tags.CanEdit = IsEditable;
				tags.PageId = PageId;
				tags.ModuleId = ModuleId;
				tags.SiteRoot = SiteRoot;
				tags.RenderAsTagCloud = config.UseTagCloudForCategories;
				
				
			}
			else
			{
				tags.Visible = false;
				pnlCategories.Visible = false;
			}

			bool showArchives = config.ShowArchives && !displaySettings.DisableShowArchives;
			if (isDetailPage && displaySettings.ShowArchivesInPostDetail) { showArchives = true; }

			if (showArchives)
			{
				archive.PageId = PageId;
				archive.ModuleId = ModuleId;
				archive.SiteRoot = SiteRoot;
				archive.HeadingElement = displaySettings.ArchiveListHeadingElement;
				archive.OverrideHeadingText = displaySettings.ArchiveListOverrideHeadingText;
			}
			else
			{
				archive.Visible = false;
				pnlArchives.Visible = false;
			}

			bool showStats = config.ShowStatistics && !displaySettings.DisableShowStatistics;
			if (isDetailPage && displaySettings.ShowStatisticsInPostDetail) { showStats = true; }

			stats.PageId = PageId;
			stats.ModuleId = ModuleId;
			stats.ModuleGuid = ModuleGuid;
			stats.CountOfDrafts = countOfDrafts;
			stats.Visible = showStats;
			stats.HeadingElement = displaySettings.StatsHeadingElement;
			stats.OverrideHeadingText = displaySettings.StatsOverrideHeadingText;

			pnlStatistics.Visible = showStats;

			if ((config.RelatedItemsToShow > 0) && (displaySettings.RelatedPostsPosition == "Side") && (Page is BlogView))
			{
				relatedPosts.PageId = pageId;
				relatedPosts.ModuleId = moduleId;
				relatedPosts.ItemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);
				relatedPosts.SiteRoot = siteRoot;
				relatedPosts.MaxItems = config.RelatedItemsToShow;
				relatedPosts.UseFriendlyUrls = BlogConfiguration.UseFriendlyUrls(moduleId);
				relatedPosts.HeadingElement = displaySettings.RelatedPostsHeadingElement;
				relatedPosts.OverrideHeadingText = displaySettings.RelatedPostsOverrideHeadingText;

			}

			if (showFeeds || showCategories || showStats || showArchives) { divNav.Visible = true; }

		}


		private void calBlogNav_SelectionChanged(object sender, EventArgs e)
		{
			System.Web.UI.WebControls.Calendar cal = (System.Web.UI.WebControls.Calendar)sender;
			CalendarDate = cal.SelectedDate;
			RedirectFromCalendar();
		}

		private void CalBlogNavVisibleMonthChanged(object sender, MonthChangedEventArgs e)
		{
			CalendarDate = e.NewDate.ToLastDateOfMonth();
			RedirectFromCalendar();
		}

		private void RedirectFromCalendar()
		{
			string pageUrl = SiteRoot + "/Blog/ViewList.aspx"
				   + "?pageid=" + pageId.ToInvariantString()
				   + "&mid=" + moduleId.ToInvariantString()
				   + "&blogdate=" + CalendarDate.Date.ToString("s");

			WebUtils.SetupRedirect(this, pageUrl);

		}

		private void SetupJQueryCalendar()
		{
			StringBuilder script = new StringBuilder();

			
			script.Append("var blogBaseUrl" + moduleId.ToInvariantString() + " = '" + SiteRoot + "/Blog/ViewList.aspx"
				   + "?pageid=" + pageId.ToInvariantString()
				   + "&mid=" + moduleId.ToInvariantString()
				   + "&blogdate=" + "'; \n");

			script.Append("function getLastDay(iMonth, iYear){");
			script.Append("return new Date(iYear, iMonth, 0).getDate(); } \n");

			script.Append("$(function() { ");

			script.Append("$( '#" + pnlDatePicker.ClientID + "' ).datepicker({ ");

			//script.Append("defaultDate:'" + navDate.ToShortDateString() + "',");

			script.Append("defaultDate:'" + navDate.ToString("yyyy-MM-dd") + "',");

			//script.Append("$( ".selector" ).datepicker( "option", "dateFormat", "yy-mm-dd" ); ");
			script.Append("dateFormat: 'yy-mm-dd', ");

			script.Append("onSelect: function(dateText){ ");
			script.Append("location.href = blogBaseUrl" + moduleId.ToInvariantString() + " + dateText;  ");
			script.Append("}, ");

			script.Append("onChangeMonthYear: function(year, month, inst) {");

			script.Append("location.href = blogBaseUrl" + moduleId.ToInvariantString() + " + year.toString() + '-' + month.toString() + '-' + getLastDay(month, year).toString() ;  ");
			//script.Append("location.href = blogBaseUrl + getLastDay(month, year).toString() ;  ");
			script.Append("}");

			script.Append("}); ");

			script.Append("}); ");


			ScriptManager.RegisterStartupScript(this, typeof(Page),
				   "blog-cal", "\n<script type=\"text/javascript\" >"
				   + script.ToString() + "</script>", false);

			string langCode = jDatePicker.GetSupportedLangCode(CultureInfo.CurrentCulture.Name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);

			if (langCode != "en")
			{
				Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
				"jqdatei18n", "\n<script src=\""
				+ jDatePicker.GetJQueryUIBasePath(Page) + "i18n/jquery.ui.datepicker-" + langCode + ".js" + "\" type=\"text/javascript\" ></script>");

			}

		}

		protected virtual void LoadSettings()
		{
			
			CalendarDate = WebUtils.ParseDateFromQueryString("blogdate", DateTime.UtcNow).Date;

			if (CalendarDate > DateTime.UtcNow.Date)
			{
				CalendarDate = DateTime.UtcNow.Date;
			}

			if (overrideDate > DateTime.MinValue)
			{
				CalendarDate = overrideDate;
			}

			navDate = CalendarDate;

			stats.ShowCommentCount = showCommentCount;

			divNav.CssClass = displaySettings.NavClass;

			if (config.NavigationOnRight)
			{
				divNav.CssClass += " " + displaySettings.NavRightClass;
			}
			else
			{
				divNav.CssClass += " " + displaySettings.NavLeftClass;
			}

			if (
				config.ShowCalendar 
				&& this.showCalendar 
				&& !displaySettings.HideCalendar
			   // && (!(Page is BlogCategoryView))
			   // && (!(Page is BlogArchiveView))
				)
			{
				calBlogNav.Visible = true;
				try
				{
					calBlogNav.FirstDayOfWeek = (FirstDayOfWeek)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
				}
				catch (ArgumentNullException) { }
				catch (ArgumentOutOfRangeException) { }
				catch (InvalidOperationException) { }
				catch (InvalidCastException) { }

				if (!Page.IsPostBack)
				{
					this.calBlogNav.SelectedDate = CalendarDate;
					this.calBlogNav.VisibleDate = CalendarDate;

				}

				if (displaySettings.UsejQueryCalendarNavigation)
				{
					calBlogNav.Visible = false;
					pnlDatePicker.Visible = true;
					SetupJQueryCalendar();
				}
				else
				{
					pnlDatePicker.Visible = false;
				}
			}
			else
			{
				calBlogNav.Visible = false;
				pnlDatePicker.Visible = false;
			}


			//pnlStatistics.Visible = config.ShowStatistics && !displaySettings.DisableShowStatistics;

			divNav.Visible = false;

			if (config.ShowCalendar
				|| config.ShowArchives
				|| config.ShowAddFeedLinks
				|| config.ShowCategories
				|| config.ShowFeedLinks
				|| config.ShowStatistics
				|| !String.IsNullOrWhiteSpace(config.UpperSidebar)
				|| !String.IsNullOrWhiteSpace(config.LowerSidebar)
				)
			{
				divNav.Visible = true;
			}

			pnlSideTop.Visible = !displaySettings.HideTopSideBar;
			pnlSideBottom.Visible = !displaySettings.HideBottomSideBar;

			searchBox.Visible = config.ShowBlogSearchBox && displaySettings.ShowSearchInNav;
		}


		private void PopulateLabels()
		{
			calBlogNav.UseAccessibleHeader = true;

		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Load += new EventHandler(Page_Load);
			this.calBlogNav.SelectionChanged += new EventHandler(calBlogNav_SelectionChanged);
			this.calBlogNav.VisibleMonthChanged += new MonthChangedEventHandler(CalBlogNavVisibleMonthChanged);
			this.EnableViewState = false;

		}
	}
}