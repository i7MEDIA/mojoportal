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
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI.Pages;

public partial class SearchResults : NonCmsBasePage
{

	private static readonly ILog log = LogManager.GetLogger(typeof(SearchResults));

	private string query = string.Empty;
	private int pageNumber = 1;
	private int pageSize = WebConfigSettings.SearchResultsPageSize;
	private int totalHits = 0;
	private int totalPages = 1;
	private bool indexVerified = false;
	private bool showModuleTitleInResultLink = WebConfigSettings.ShowModuleTitleInSearchResultLink;
	private bool isSiteEditor = false;
	private Guid featureGuid = Guid.Empty;
	private bool queryErrorOccurred = false;
	private DateTime modifiedBeginDate = DateTime.MinValue;
	private DateTime modifiedEndDate = DateTime.MaxValue;
	private TimeZoneInfo timeZone = null;


	#region OnInit
	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		btnDoSearch.Click += new EventHandler(btnDoSearch_Click);
		btnRebuildSearchIndex.Click += new EventHandler(btnRebuildSearchIndex_Click);

		SuppressMenuSelection();
		SuppressPageMenu();

		if (WebConfigSettings.ShowLeftColumnOnSearchResults) { StyleCombiner.AlwaysShowLeftColumn = true; }
		if (WebConfigSettings.ShowRightColumnOnSearchResults) { StyleCombiner.AlwaysShowRightColumn = true; }
	}
	#endregion

	private void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();

		query = string.Empty;

		siteSettings ??= CacheHelper.GetCurrentSiteSettings();

		PopulateLabels();

		string primarySearchProvider = SiteUtils.GetPrimarySearchProvider();

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
		ShowNoResults();

		if (SiteUtils.ShowAlternateSearchIfConfigured())
		{
			string bingApiId = SiteUtils.GetBingApiId();
			string googleCustomSearchId = SiteUtils.GetGoogleCustomSearchId();
			if ((bingApiId.Length > 0) || (googleCustomSearchId.Length > 0)) { spnAltSearchLinks.Visible = true; }

			lnkBingSearch.Visible = (bingApiId.Length > 0);
			lnkGoogleSearch.Visible = (googleCustomSearchId.Length > 0);
		}

		if (!WebConfigSettings.DisableSearchFeatureFilters && displaySettings.ShowFeatureFilter)
		{
			if (!Page.IsPostBack)
			{
				BindFeatureList();
				ddFeatureList.Items.Insert(0, new ListItem(Resource.SearchAllContentItem, Guid.Empty.ToString()));
				if (ddFeatureList.Items.Count > 0)
				{
					ListItem item = ddFeatureList.Items.FindByValue(featureGuid.ToString());
					if (item != null)
					{
						ddFeatureList.ClearSelection();
						item.Selected = true;
					}
				}
				else
				{
					ddFeatureList.Visible = false;
				}
			}
		}
		else
		{
			ddFeatureList.Visible = false;
		}

		//got here by a cross page postback from another page if Page.PreviousPage is not null
		// this occurs when the search input is used in the skin rather than the search link
		if (Page.PreviousPage != null)
		{
			HandleCrossPagePost();
		}
		else
		{
			DoSearch();
		}

		txtSearchInput.Focus();
	}

	private void HandleCrossPagePost()
	{

		var previousPageSearchInput = (SearchInput)PreviousPage.Master.FindControl("SearchInput1");
		// try in page if not found in masterpage
		previousPageSearchInput ??= (SearchInput)PreviousPage.FindControl("SearchInput1");

		if (previousPageSearchInput is not null)
		{
			TextBox prevSearchTextBox = (TextBox)previousPageSearchInput.FindControl("txtSearch");
			if (prevSearchTextBox?.Text.Length > 0)
			{
				WebUtils.SetupRedirect(this, SiteRoot + "/SearchResults.aspx?q=" + Server.UrlEncode(prevSearchTextBox.Text));
				return;
			}
		}

		DoSearch();
	}


	private List<string> GetUserRoles()
	{
		List<string> userRoles = new List<string>();

		userRoles.Add("All Users");
		if (Request.IsAuthenticated)
		{
			SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
			if (currentUser is not null)
			{
				using IDataReader reader = SiteUser.GetRolesByUser(siteSettings.SiteId, currentUser.UserId);
				while (reader.Read())
				{
					userRoles.Add(reader["RoleName"].ToString());
				}
			}
		}

		return userRoles;
	}


	private void DoSearch()
	{
		if (Page.IsPostBack || Request.QueryString.Get("q") == null)
		{
			return;
		}

		query = Request.QueryString.Get("q").RemoveMarkup();

		if (query.Length == 0) { return; }

		txtSearchInput.Text = query;


		// this is only to make sure its initialized
		// before indexing is queued on a thread
		// because there is no HttpContext on
		// external threads and HttpContext is needed to initialize
		// once initialized it's cached

		_ = IndexBuilderManager.Providers;

		queryErrorOccurred = false;

		var searchResults = IndexHelper.Search(
			siteSettings.SiteId,
			isSiteEditor,
			GetUserRoles(),
			featureGuid,
			modifiedBeginDate,
			modifiedEndDate,
			query,
			WebConfigSettings.EnableSearchResultsHighlighting,
			WebConfigSettings.SearchResultsFragmentSize,
			pageNumber,
			pageSize,
			WebConfigSettings.SearchMaxClauseCount,
			out totalHits,
			out queryErrorOccurred);

		if (searchResults.Count == 0)
		{
			//ShowNoResults();
			InitIndexIfNeeded();
			return;
		}

		if (searchResults.Count > 0)
		{
			var duration = searchResults.ExecutionTime * 0.0000001F;
			lblDuration.Visible = true;
			lblDuration.Text = duration.ToString();
			lblSeconds.Visible = true;
		}

		//if problems with encoding of query, swap 'query' out with Server.UrlEncode(query)
		var pagerUrlFormat = "SearchResults.aspx"
			.ToLinkBuilder()
			.AddParams(GetSearchParams(query))
			.AddParam("p", "{0}")
			.ToString();

		var model = SearchResultsViewModel.Create(
			pageNumber,
			pageSize,
			totalHits,
			showModuleTitleInResultLink,
			pagerUrlFormat,
			query,
			timeZone,
			searchResults,
			displaySettings
		);

		try
		{
			litSearchResults.Text = RazorBridge.RenderPartialToString("Results", model, "SearchResults");
		}
		catch (Exception e)
		{
			log.Error("There was an issue rendering the search results page.", e);
		}
	}


	private Dictionary<string, object> GetSearchParams(string query)
	{
		var searchParams = new Dictionary<string, object>
		{
			{ "q", Server.UrlEncode(query) }
		};

		if (modifiedBeginDate.Date != DateTime.MinValue.Date && displaySettings.ShowModifiedDateFilters)
		{
			searchParams.Add("bd", $"{modifiedBeginDate.Date:s}");
		}

		if (modifiedEndDate.Date != DateTime.MaxValue.Date && displaySettings.ShowModifiedDateFilters)
		{
			searchParams.Add("ed", $"{modifiedEndDate.Date:s}");
		}

		if (featureGuid != Guid.Empty)
		{
			searchParams.Add("f", featureGuid);
		}

		return searchParams;
	}


	private void BindFeatureList()
	{
		using IDataReader reader = ModuleDefinition.GetSearchableModules(siteSettings.SiteId);
		ListItem listItem;

		// this flag tells it to look first for a web config setting for the resource string
		// corresponding to SearchListName value
		// it allows you to customize searchlist names whereas by default they are just localized
		// no actually implemented as of 20250305 - jmd0
		bool useConfigOverrides = true;

		while (reader.Read())
		{
			var featureId = reader["Guid"].ToString();

			if (!WebConfigSettings.SearchableFeatureGuidsToExclude.Contains(featureId))
			{
				listItem = new ListItem(
					ResourceHelper.GetResourceString(
					reader["ResourceFile"].ToString(),
					reader["SearchListName"].ToString(),
					useConfigOverrides),
					featureId);

				ddFeatureList.Items.Add(listItem);
			}
		}
	}


	private void InitIndexIfNeeded()
	{
		if (indexVerified)
		{
			return;
		}

		indexVerified = true;

		if (!IndexHelper.VerifySearchIndex(siteSettings))
		{
			lblMessage.Text = Resource.SearchResultsBuildingIndexMessage;

			Thread.Sleep(5000); //wait 5 seconds
			SiteUtils.QueueIndexing();
		}
	}


	private void ShowNoResults()
	{
		if (queryErrorOccurred)
		{
			//lblNoResults.Text = Resource.SearchQueryInvalid;
		}
		//divResults.Visible = false;
		//pnlNoResults.Visible = txtSearchInput.Text.Length > 0;
	}


	private void btnDoSearch_Click(object sender, EventArgs e)
	{
		if (dpBeginDate.Text.Length > 0)
		{
			DateTime.TryParse(dpBeginDate.Text, out modifiedBeginDate);
		}
		else
		{
			modifiedBeginDate = DateTime.MinValue;
		}

		if (dpEndDate.Text.Length > 0)
		{
			DateTime.TryParse(dpEndDate.Text, out modifiedEndDate);
		}
		else
		{
			modifiedEndDate = DateTime.MaxValue;
		}

		if (displaySettings.ShowFeatureFilter)
		{
			if (ddFeatureList.SelectedValue.Length == 36)
			{
				featureGuid = new Guid(ddFeatureList.SelectedValue);
			}
		}

		//string redirectUrl = $"{SiteRoot}/SearchResults.aspx?q={Server.UrlEncode(txtSearchInput.Text)}{GetModBeginDateParam(false)}{GetModEndDateParam(false)}{GetFeatureParam(false)}";

		//if problems with encoding of query, swap 'txtSearchInput.Text' out with Server.UrlEncode(txtSearchInput.Text)
		string redirectUrl = "SearchResults.aspx".ToLinkBuilder().AddParams(GetSearchParams(txtSearchInput.Text)).ToString();

		WebUtils.SetupRedirect(this, redirectUrl);
	}

	void btnRebuildSearchIndex_Click(object sender, EventArgs e)
	{
		IndexingQueue.DeleteAll();
		IndexHelper.DeleteSearchIndex(siteSettings);
		IndexHelper.VerifySearchIndex(siteSettings);

		lblMessage.Text = Resource.SearchResultsBuildingIndexMessage;
		Thread.Sleep(5000); //wait 5 seconds
		SiteUtils.QueueIndexing();
	}

	private void SetupScript()
	{
		if (WebConfigSettings.DisablejQuery) { return; }
		if (!WebConfigSettings.OpenSearchDownloadLinksInNewWindow) { return; }

		// make shared files download links open in a new window
		var script = new StringBuilder();
		script.Append("\n<script data-loader=\"SearchResults.aspx\">");
		script.Append("$(\"a[href*='Download.aspx']\")");
		script.Append(".bind('click', function(){window.open(this.href,'_blank');return false;}); ");
		script.Append("\n</script>");

		Page.ClientScript.RegisterStartupScript(typeof(Page), "searchpage", script.ToString());
	}

	private void LoadSettings()
	{
		isSiteEditor = WebUser.IsAdminOrContentAdmin || (SiteUtils.UserIsSiteEditor());

		pageNumber = WebUtils.ParseInt32FromQueryString("p", true, 1);
		featureGuid = WebUtils.ParseGuidFromQueryString("f", featureGuid);
		modifiedBeginDate = WebUtils.ParseDateFromQueryString("bd", DateTime.MinValue).Date;
		modifiedEndDate = WebUtils.ParseDateFromQueryString("ed", DateTime.MaxValue).Date;

		if (!IsPostBack)
		{
			if (modifiedBeginDate.Date > DateTime.MinValue.Date)
			{
				dpBeginDate.Text = modifiedBeginDate.ToShortDateString();
			}

			if (modifiedEndDate.Date < DateTime.MaxValue.Date)
			{
				dpEndDate.Text = modifiedEndDate.ToShortDateString();
			}
		}

		spnDateRange.Visible = displaySettings.ShowModifiedDateFilters;

		if (displaySettings.ShowCreatedDate || displaySettings.ShowLastModDate)
		{
			timeZone = SiteUtils.GetUserTimeZone();
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

		lblMessage.Text = string.Empty;
		//divResults.Visible = true;

		btnDoSearch.Text = Resource.SearchButtonText;
		SiteUtils.SetButtonAccessKey(btnDoSearch, AccessKeys.SearchButtonTextAccessKey);

		btnRebuildSearchIndex.Text = Resource.SearchRebuildIndexButton;
		UIHelper.AddConfirmationDialog(btnRebuildSearchIndex, Resource.SearchRebuildIndexWarning);

		divDelete.Visible = (WebConfigSettings.ShowRebuildSearchIndexButtonToAdmins && WebUser.IsAdmin);

		//lblNoResults.Text = Resource.SearchResultsNotFound;

		litAltSearchMessage.Text = Resource.AltSearchPrompt;
		lnkBingSearch.Text = Resource.SearchThisSiteWithBing;
		lnkBingSearch.NavigateUrl = $"{SiteRoot}/BingSearch.aspx";
		lnkGoogleSearch.Text = Resource.SearchThisSiteWithGoogle;
		lnkGoogleSearch.NavigateUrl = $"{SiteRoot}/GoogleSearch.aspx";

		litDatePreamble.Text = Resource.SearchDateFilterPreamble;
		litAnd.Text = Resource.and;

		//this page has no content other than nav
		SiteUtils.AddNoIndexFollowMeta(Page);

		AddClassToBody("searchresults");
	}
}
