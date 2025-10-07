#nullable enable
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Models;
using Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI.Pages;

public partial class SearchResults : NonCmsBasePage
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(SearchResults));

	private readonly bool _isSiteEditor = WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor();
	private readonly bool _showModuleTitleInResultLink = WebConfigSettings.ShowModuleTitleInSearchResultLink;
	private readonly int _pageSize = WebConfigSettings.SearchResultsPageSize;

	private string _queryText = string.Empty;
	private int _pageNumber = 1;
	private int _totalHits = 0;
	private bool _indexVerified = false;
	private Guid _featureGuid = Guid.Empty;
	private bool _queryErrorOccurred = false;
	private DateTime _modifiedBeginDate = DateTime.MinValue;
	private DateTime _modifiedEndDate = DateTime.MaxValue;
	private TimeZoneInfo? _timeZone = null;


	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);

		SuppressMenuSelection();
		SuppressPageMenu();

		if (WebConfigSettings.ShowLeftColumnOnSearchResults) { StyleCombiner.AlwaysShowLeftColumn = true; }
		if (WebConfigSettings.ShowRightColumnOnSearchResults) { StyleCombiner.AlwaysShowRightColumn = true; }
	}

	#endregion


	private void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();

		siteSettings ??= CacheHelper.GetCurrentSiteSettings();

		PopulateLabels();

		var primarySearchProvider = SiteUtils.GetPrimarySearchProvider();

		switch (primarySearchProvider)
		{
			case "google":
				pnlInternalSearch.Visible = false;
				pnlBingSearch.Visible = false;
				pnlGoogleSearch.Visible = true;
				gcs.Visible = true;

				break;

			case "bing":
				pnlInternalSearch.Visible = false;
				pnlBingSearch.Visible = true;
				pnlGoogleSearch.Visible = false;
				bingSearch.Visible = true;
				break;

			case "internal":
			default:

				if (WebConfigSettings.DisableSearchIndex)
				{
					WebUtils.SetupRedirect(this, SiteUtils.GetNavigationSiteRoot());
					return;
				}

				pnlInternalSearch.Visible = true;
				pnlBingSearch.Visible = false;
				pnlGoogleSearch.Visible = false;
				SetupInternalSearch();
				break;
		}
	}


	private void SetupInternalSearch()
	{
		SetupScript();

		if (SiteUtils.ShowAlternateSearchIfConfigured())
		{
			var bingApiId = SiteUtils.GetBingApiId();
			var googleCustomSearchId = SiteUtils.GetGoogleCustomSearchId();

			if (bingApiId.Length > 0 || googleCustomSearchId.Length > 0)
			{
				spnAltSearchLinks.Visible = true;
			}

			lnkBingSearch.Visible = !string.IsNullOrWhiteSpace(bingApiId);
			lnkGoogleSearch.Visible = !string.IsNullOrWhiteSpace(googleCustomSearchId);
		}

		if (Page is mojoBasePage basePage)
		{
			basePage.EnsureDefaultModal();
		}

		IndexItemCollection? results;

		//got here by a cross page postback from another page if Page.PreviousPage is not null
		// this occurs when the search input is used in the skin rather than the search link
		if (Page.PreviousPage != null)
		{
			results = HandleCrossPagePost();
		}
		else
		{
			results = GetSearchResults();
		}

		SetupViewModel(results);
	}


	private IndexItemCollection? HandleCrossPagePost()
	{
		var previousPageSearchInput = PreviousPage.Master.FindControl("SearchInput1") as SearchInput;

		// try in page if not found in master page
		previousPageSearchInput ??= PreviousPage.FindControl("SearchInput1") as SearchInput;

		if (previousPageSearchInput is not null)
		{
			var prevSearchTextBox = previousPageSearchInput.FindControl("txtSearch") as TextBox;

			if (prevSearchTextBox is not null && !string.IsNullOrWhiteSpace(prevSearchTextBox.Text))
			{
				WebUtils.SetupRedirect(this, "~/SearchResults.aspx?".ToLinkBuilder().AddParam("q", prevSearchTextBox.Text).ToString());
				return null;
			}
		}

		return GetSearchResults();
	}


	private List<string> GetUserRoles()
	{
		List<string> userRoles = ["All Users"];

		if (Request.IsAuthenticated)
		{
			var currentUser = SiteUtils.GetCurrentSiteUser();

			if (currentUser is not null)
			{
				using var reader = SiteUser.GetRolesByUser(siteSettings.SiteId, currentUser.UserId);

				while (reader.Read())
				{
					userRoles.Add(reader["RoleName"].ToString());
				}
			}
		}

		return userRoles;
	}


	private IndexItemCollection? GetSearchResults()
	{
		if (Page.IsPostBack || string.IsNullOrWhiteSpace(_queryText))
		{
			return null;
		}

		// this is only to make sure it's initialized
		// before indexing is queued on a thread
		// because there is no HttpContext on
		// external threads and HttpContext is needed to initialize
		// once initialized it's cached
		// Ideally this should be done on app startup.
		_ = IndexBuilderManager.Providers;

		var searchResults = IndexHelper.Search(
			siteSettings.SiteId,
			_isSiteEditor,
			GetUserRoles(),
			_featureGuid,
			_modifiedBeginDate,
			_modifiedEndDate,
			_queryText,
			WebConfigSettings.EnableSearchResultsHighlighting,
			WebConfigSettings.SearchResultsFragmentSize,
			_pageNumber,
			_pageSize,
			WebConfigSettings.SearchMaxClauseCount,
			out _totalHits,
			out _queryErrorOccurred
		);

		if ((searchResults?.Count ?? 0) == 0)
		{
			InitIndexIfNeeded();
			return null;
		}

		return searchResults;
	}


	private void SetupViewModel(IndexItemCollection? searchResults = null)
	{
		var pageUrlFormat = "~/SearchResults.aspx"
			.ToLinkBuilder()
			.AddParams(GetSearchParams())
			.AddParam("p", "{0}")
			.ToString();

		var model = SearchResultsViewModel.Create(
			Resource.SearchPageTitle,
			_pageNumber,
			_pageSize,
			_totalHits,
			_showModuleTitleInResultLink,
			pageUrlFormat,
			_queryText,
			_timeZone,
			items: searchResults,
			displaySettings,
			duration: searchResults?.ExecutionTime * 0.0000001F,
			featureList: GetFeatureList(),
			dateStart: _modifiedBeginDate,
			dateEnd: _modifiedEndDate,
			_queryErrorOccurred
		);

		try
		{
			litSearchResults.Text = RazorBridge.RenderPartialToString("Results", model, "SearchResults");
		}
		catch (Exception e)
		{
			litSearchResults.Text = RazorBridge.RenderPartialToString("_SearchResultsFallback", null, "SearchResults");
			_log.Error("There was an issue rendering the search results page.", e);
		}
	}


	private Dictionary<string, object> GetSearchParams()
	{
		var searchParams = new Dictionary<string, object>
		{
			{ "q", Server.UrlEncode(_queryText) }
		};

		if (_modifiedBeginDate.Date != DateTime.MinValue.Date && displaySettings.ShowModifiedDateFilters)
		{
			searchParams.Add("bd", $"{_modifiedBeginDate.Date:s}");
		}

		if (_modifiedEndDate.Date != DateTime.MaxValue.Date && displaySettings.ShowModifiedDateFilters)
		{
			searchParams.Add("ed", $"{_modifiedEndDate.Date:s}");
		}

		if (_featureGuid != Guid.Empty)
		{
			searchParams.Add("f", _featureGuid);
		}

		return searchParams;
	}


	private List<SelectListItem> GetFeatureList()
	{
		using var reader = ModuleDefinition.GetSearchableModules(siteSettings.SiteId);

		var items = new List<SelectListItem>
		{
			new()
			{
				Text = Resource.SearchAllContentItem,
				Value = Guid.Empty.ToString()
			}
		};

		// this flag tells it to look first for a web config setting for the resource string
		// corresponding to SearchListName value
		// it allows you to customize searchlist names whereas by default they are just localized
		// no actually implemented as of 20250305 - jmd0
		var useConfigOverrides = true;

		while (reader.Read())
		{
			var featureId = reader["Guid"].ToString();

			if (!WebConfigSettings.SearchableFeatureGuidsToExclude.Contains(featureId, StringComparison.InvariantCultureIgnoreCase))
			{
				items.Add(new SelectListItem
				{
					Text = ResourceHelper.GetResourceString(
						reader["ResourceFile"].ToString(),
						reader["SearchListName"].ToString(),
						useConfigOverrides
					),
					Value = featureId,
					Selected = _featureGuid.ToString() == featureId
				});
			}
		}

		return items;
	}


	private void InitIndexIfNeeded()
	{
		if (_indexVerified)
		{
			return;
		}

		_indexVerified = true;

		if (!IndexHelper.VerifySearchIndex(siteSettings))
		{
			Thread.Sleep(5000); //wait 5 seconds
			SiteUtils.QueueIndexing();
		}
	}


	private void SetupScript()
	{
		if (!WebConfigSettings.OpenSearchDownloadLinksInNewWindow)
		{
			return;
		}

		// make shared files download links open in a new window
		var script = """
			<script data-loader="SearchResults.aspx">
				const downloadLinks = document.querySelectorAll('a[href*="Download.aspx"]');

				function onClick() {
					window.open(this.href,'_blank'); return false;
				}
				
				downloadLinks.forEach(x => x.addEventListener('click', onClick));
			</script>
			""";

		Page.ClientScript.RegisterStartupScript(typeof(Page), "searchpage", script);
	}


	private void LoadSettings()
	{
		_queryText = WebUtils.ParseStringFromQueryString("q", string.Empty);
		_pageNumber = WebUtils.ParseInt32FromQueryString("p", true, 1);
		_featureGuid = WebUtils.ParseGuidFromQueryString("f", _featureGuid);
		_modifiedBeginDate = WebUtils.ParseDateFromQueryString("bd", DateTime.MinValue).Date;
		_modifiedEndDate = WebUtils.ParseDateFromQueryString("ed", DateTime.MaxValue).Date;

		if (displaySettings.ShowCreatedDate || displaySettings.ShowLastModDate)
		{
			_timeZone = SiteUtils.GetUserTimeZone();
		}
	}


	private void PopulateLabels()
	{
		if (siteSettings is null)
		{
			return;
		}

		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.SearchPageTitle);

		heading.Text = Resource.SearchPageTitle;

		MetaDescription = string.Format(CultureInfo.InvariantCulture, Resource.MetaDescriptionSearchFormat, siteSettings.SiteName);

		litAltSearchMessage.Text = Resource.AltSearchPrompt;
		lnkBingSearch.Text = Resource.SearchThisSiteWithBing;
		lnkBingSearch.NavigateUrl = $"{SiteRoot}/BingSearch.aspx";
		lnkGoogleSearch.Text = Resource.SearchThisSiteWithGoogle;
		lnkGoogleSearch.NavigateUrl = $"{SiteRoot}/GoogleSearch.aspx";

		//this page has no content other than nav
		SiteUtils.AddNoIndexFollowMeta(Page);

		AddClassToBody("searchresults");
	}
}
