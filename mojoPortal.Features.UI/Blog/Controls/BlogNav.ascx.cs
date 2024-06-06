using System;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;

namespace mojoPortal.Web.BlogUI;

public partial class BlogNav : UserControl
{
	#region Properties

	protected DateTime CalendarDate;
	protected DateTime navDate;

	public int PageId { get; set; } = -1;
	public int ModuleId { get; set; } = -1;
	public Guid ModuleGuid { get; set; } = Guid.Empty;
	public BlogConfiguration Config { get; set; }
	public DateTime OverrideDate { get; set; } = DateTime.MinValue;
	public int CountOfDrafts { get; set; } = 0;
	public string SiteRoot { get; set; } = string.Empty;
	public bool ShowCalendar { get; set; } = true;
	public bool ShowCommentCount { get; set; } = true;
	public bool IsEditable { get; set; } = false;

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
		bool isDetailPage = Page is BlogView;

		pnlSideTop.Visible = false;
		pnlSideBottom.Visible = false;

		if (!String.IsNullOrWhiteSpace(Config.UpperSidebar))
		{
			pnlSideTop.Visible = true;
			litUpperSidebar.Text = Config.UpperSidebar;
		}

		if (!String.IsNullOrWhiteSpace(Config.LowerSidebar))
		{
			pnlSideBottom.Visible = true;
			litLowerSidebar.Text = Config.LowerSidebar;
		}

		Feeds.Config = Config;
		Feeds.PageId = PageId;
		Feeds.ModuleId = ModuleId;
		bool showFeeds = Config.ShowFeedLinks && !displaySettings.HideFeedLinks;
		if (isDetailPage && displaySettings.ShowFeedLinksInPostDetail)
		{
			showFeeds = true;
		}

		Feeds.Visible = showFeeds;

		bool showCategories = Config.ShowCategories && !displaySettings.DisableShowCategories;
		if (isDetailPage && displaySettings.ShowCategoriesInPostDetail)
		{
			showCategories = true;
		}

		if (showCategories)
		{
			tags.CanEdit = IsEditable;
			tags.PageId = PageId;
			tags.ModuleId = ModuleId;
			tags.SiteRoot = SiteRoot;
			tags.RenderAsTagCloud = Config.UseTagCloudForCategories;
		}
		else
		{
			tags.Visible = false;
			pnlCategories.Visible = false;
		}

		bool showArchives = Config.ShowArchives && !displaySettings.DisableShowArchives;
		if (isDetailPage && displaySettings.ShowArchivesInPostDetail)
		{
			showArchives = true;
		}

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

		bool showStats = Config.ShowStatistics && !displaySettings.DisableShowStatistics;
		if (isDetailPage && displaySettings.ShowStatisticsInPostDetail)
		{
			showStats = true;
		}

		stats.PageId = PageId;
		stats.ModuleId = ModuleId;
		stats.ModuleGuid = ModuleGuid;
		stats.CountOfDrafts = CountOfDrafts;
		stats.Visible = showStats;
		stats.HeadingElement = displaySettings.StatsHeadingElement;
		stats.OverrideHeadingText = displaySettings.StatsOverrideHeadingText;

		pnlStatistics.Visible = showStats;

		if ((Config.RelatedItemsToShow > 0) && (displaySettings.RelatedPostsPosition == "Side") && (Page is BlogView))
		{
			relatedPosts.PageId = PageId;
			relatedPosts.ModuleId = ModuleId;
			relatedPosts.ItemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);
			relatedPosts.SiteRoot = SiteRoot;
			relatedPosts.MaxItems = Config.RelatedItemsToShow;
			relatedPosts.UseFriendlyUrls = BlogConfiguration.UseFriendlyUrls(ModuleId);
			relatedPosts.HeadingElement = displaySettings.RelatedPostsHeadingElement;
			relatedPosts.OverrideHeadingText = displaySettings.RelatedPostsOverrideHeadingText;

		}

		if (showFeeds || showCategories || showStats || showArchives)
		{
			divNav.Visible = true;
		}

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
		string pageUrl = "/Blog/ViewList.aspx".ToLinkBuilder().PageId(PageId).ModuleId(ModuleId).AddParam("blogdate", CalendarDate.Date.ToString("s")).ToString();
		WebUtils.SetupRedirect(this, pageUrl);
	}

	private void SetupJQueryCalendar()
	{
		var script = Invariant($@"
var blogBaseUrl{ModuleId} = '{"/Blog/ViewList.aspx".ToLinkBuilder().PageId(PageId).ModuleId(ModuleId).AddParam("blogdate", string.Empty)}'; 

function getLastDay(iMonth, iYear) {{
	return new Date(iYear, iMonth, 0).getDate();
}} 

$(function() {{ 
	$('#{pnlDatePicker.ClientID}').datepicker({{
		defaultDate:'{navDate:yyyy-MM-dd}',
		dateFormat: 'yy-mm-dd', 
		onSelect: function(dateText) {{ 
			location.href = blogBaseUrl{ModuleId} + dateText;
		}},
		onChangeMonthYear: function(year, month, inst) {{
			location.href = blogBaseUrl{ModuleId} + year.toString() + '-' + month.toString() + '-' + getLastDay(month, year).toString();
		}}
	}});
}});
");

		ScriptManager.RegisterStartupScript(this, typeof(Page), "blog-cal", $"\n<script data-loader=\"BlogNav\">{script}</script>", false);

		string langCode = jDatePicker.GetSupportedLangCode(CultureInfo.CurrentCulture.Name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);

		if (langCode != "en")
		{
			Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
			"jqdatei18n", $"\n<script src=\"{jDatePicker.GetJQueryUIBasePath(Page)}i18n/jquery.ui.datepicker-{langCode}.js\" data-loader=\"BlogNav\"></script>");
		}
	}

	protected virtual void LoadSettings()
	{

		CalendarDate = WebUtils.ParseDateFromQueryString("blogdate", DateTime.UtcNow).Date;

		if (CalendarDate > DateTime.UtcNow.Date)
		{
			CalendarDate = DateTime.UtcNow.Date;
		}

		if (OverrideDate > DateTime.MinValue)
		{
			CalendarDate = OverrideDate;
		}

		navDate = CalendarDate;

		stats.ShowCommentCount = ShowCommentCount;

		divNav.CssClass = displaySettings.NavClass;

		if (Config.NavigationOnRight)
		{
			divNav.CssClass += $" {displaySettings.NavRightClass}";
		}
		else
		{
			divNav.CssClass += $" {displaySettings.NavLeftClass}";
		}

		if (Config.ShowCalendar
			&& ShowCalendar
			&& !displaySettings.HideCalendar
			)
		{
			if (displaySettings.UsejQueryCalendarNavigation)
			{
				calBlogNav.Visible = false;
				pnlDatePicker.Visible = true;
				SetupJQueryCalendar();
			}
			else
			{
				pnlDatePicker.Visible = false;
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
					calBlogNav.SelectedDate = CalendarDate;
					calBlogNav.VisibleDate = CalendarDate;
				}
			}
		}
		else
		{
			calBlogNav.Visible = false;
			pnlDatePicker.Visible = false;
		}


		//pnlStatistics.Visible = config.ShowStatistics && !displaySettings.DisableShowStatistics;

		divNav.Visible = false;

		if (Config.ShowCalendar
			|| Config.ShowArchives
			|| Config.ShowAddFeedLinks
			|| Config.ShowCategories
			|| Config.ShowFeedLinks
			|| Config.ShowStatistics
			|| !String.IsNullOrWhiteSpace(Config.UpperSidebar)
			|| !String.IsNullOrWhiteSpace(Config.LowerSidebar)
			)
		{
			divNav.Visible = true;
		}

		pnlSideTop.Visible = !displaySettings.HideTopSideBar;
		pnlSideBottom.Visible = !displaySettings.HideBottomSideBar;

		searchBox.Visible = Config.ShowBlogSearchBox && displaySettings.ShowSearchInNav;
	}


	private void PopulateLabels()
	{
		calBlogNav.UseAccessibleHeader = true;

	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		calBlogNav.SelectionChanged += new EventHandler(calBlogNav_SelectionChanged);
		calBlogNav.VisibleMonthChanged += new MonthChangedEventHandler(CalBlogNavVisibleMonthChanged);
		EnableViewState = false;

	}
}