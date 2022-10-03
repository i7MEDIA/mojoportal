// Author:					    
// Created:				        2005-06-26
//	Last Modified:              2013-01-17
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software. 

using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.SearchIndex;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI.Pages
{

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
            this.Load += new EventHandler(this.Page_Load);
            this.btnDoSearch.Click += new EventHandler(btnDoSearch_Click);
            btnRebuildSearchIndex.Click += new EventHandler(btnRebuildSearchIndex_Click);
           
            SuppressMenuSelection();
            SuppressPageMenu();

            if (WebConfigSettings.ShowLeftColumnOnSearchResults) { StyleCombiner.AlwaysShowLeftColumn = true; }
            if (WebConfigSettings.ShowRightColumnOnSearchResults) { StyleCombiner.AlwaysShowRightColumn = true; }
        }

        

        
        #endregion

		private void Page_Load(object sender, EventArgs e)
		{
            if (SiteUtils.SslIsAvailable()) { SiteUtils.ForceSsl(); }

            LoadSettings();

            

			this.query = string.Empty;

            if (siteSettings == null)
            {
                siteSettings = CacheHelper.GetCurrentSiteSettings();
            }

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

            if ((!WebConfigSettings.DisableSearchFeatureFilters)&& (displaySettings.ShowFeatureFilter))
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
            // this occurs when the seach input is used in the skin rather than the search link
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
            
            SearchInput previousPageSearchInput = (SearchInput)PreviousPage.Master.FindControl("SearchInput1");
            // try in page if not found in masterpage
            if (previousPageSearchInput == null) { previousPageSearchInput = (SearchInput)PreviousPage.FindControl("SearchInput1"); }

            if (previousPageSearchInput != null)
            {
                TextBox prevSearchTextBox = (TextBox)previousPageSearchInput.FindControl("txtSearch");
                if ((prevSearchTextBox != null)&&(prevSearchTextBox.Text.Length > 0))
                {
                    //this.txtSearchInput.Text = prevSearchTextBox.Text;
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
                if (currentUser != null)
                {
                    using (IDataReader reader = SiteUser.GetRolesByUser(siteSettings.SiteId, currentUser.UserId))
                    {
                        while (reader.Read())
                        {
                            userRoles.Add(reader["RoleName"].ToString());
                        }

                    }

                }


            }

            return userRoles;
        }


        private void DoSearch()
        {
            if (Page.IsPostBack) { return; }

            if (Request.QueryString.Get("q") == null) { return; }

            query = Request.QueryString.Get("q");

            if (this.query.Length == 0) { return; }

            

            //txtSearchInput.Text = Server.HtmlEncode(query).Replace("&quot;", "\"") ;
            txtSearchInput.Text = SecurityHelper.SanitizeHtml(query);
            

            // this is only to make sure its initialized
            // before indexing is queued on a thread
            // because there is no HttpContext on
            // external threads and httpcontext is needed to initilaize
            // once initialized its cached
            IndexBuilderProviderCollection
                indexProviders = IndexBuilderManager.Providers;

            queryErrorOccurred = false;
          
            mojoPortal.SearchIndex.IndexItemCollection searchResults = mojoPortal.SearchIndex.IndexHelper.Search(
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
                
                ShowNoResults();
                InitIndexIfNeeded();
                return;
            }

            int start = 1;
            if (pageNumber > 1) 
            { 
                start = ((pageNumber -1) * pageSize) + 1; 
            }

            int end = pageSize;
            if (start > 1) { end += start; }

            if (end > totalHits)
            {
                end = totalHits;
            }

            this.pnlSearchResults.Visible = true;
            this.pnlNoResults.Visible = false;
            this.lblDuration.Visible = true;
            this.lblSeconds.Visible = true;

            this.lblFrom.Text = (start).ToString();
            this.lblTo.Text = end.ToString(CultureInfo.InvariantCulture);
            this.lblTotal.Text = totalHits.ToString(CultureInfo.InvariantCulture);
            this.lblQueryText.Text = Server.HtmlEncode(query);
            float duration = searchResults.ExecutionTime*0.0000001F;
            this.lblDuration.Text = duration.ToString();
            divResults.Visible = true;

            totalPages = 1;
            //int pageLowerBound = (pageSize * pageNumber) - pageSize;

            if (pageSize > 0) { totalPages = totalHits / pageSize; }

            if (totalHits <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalHits, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            //totalPages always seems 1 more than it should be not sure why
            //if (totalPages > 1) { totalPages -= 1; }

            string searchUrl = SiteRoot
                + "/SearchResults.aspx?q=" + Server.UrlEncode(query)
                + "&amp;p={0}"
                + GetModBeginDateParam(true)
                + GetModEndDateParam(true)
                + GetFeatureParam(true);

            pgrTop.PageURLFormat = searchUrl;
            pgrTop.ShowFirstLast = true;
            pgrTop.CurrentIndex = pageNumber;
            pgrTop.PageSize = pageSize;
            pgrTop.PageCount = totalPages;
            pgrTop.Visible = (totalPages > 1);

            pgrBottom.PageURLFormat = searchUrl;
            pgrBottom.ShowFirstLast = true;
            pgrBottom.CurrentIndex = pageNumber;
            pgrBottom.PageSize = pageSize;
            pgrBottom.PageCount = totalPages;
            pgrBottom.Visible = (totalPages > 1);

            

            this.rptResults.DataSource = searchResults;
            this.rptResults.DataBind();

            
            
        }

        private string GetModBeginDateParam(bool encode)
        {
            if (modifiedBeginDate.Date == DateTime.MinValue.Date) { return string.Empty; }
            if (!displaySettings.ShowModifiedDateFilters) { return string.Empty; }

            if (encode) { return "&amp;bd=" + modifiedBeginDate.Date.ToString("s"); }

            return "&bd=" + modifiedBeginDate.Date.ToString("s");

        }

        private string GetModEndDateParam(bool encode)
        {
            if (modifiedEndDate.Date == DateTime.MaxValue.Date) { return string.Empty; }
            if (!displaySettings.ShowModifiedDateFilters) { return string.Empty; }

            if (encode) { return "&amp;ed=" + modifiedEndDate.Date.ToString("s"); }

            return "&ed=" + modifiedEndDate.Date.ToString("s");

        }

        private string GetFeatureParam(bool encode)
        {
            if (featureGuid == Guid.Empty) { return string.Empty; }

            if(encode) { return "&amp;f=" + featureGuid.ToString();}

            return "&f=" + featureGuid.ToString();
        }

        private void BindFeatureList()
        {
            using (IDataReader reader = ModuleDefinition.GetSearchableModules(siteSettings.SiteId))
            {
                ListItem listItem;

                // this flag tells it to look first for a web config setting for the resource string
                // corresponding to SearchListName value
                // it allows you to customize searchlist names wheeas by default they are just localized
                bool useConfigOverrides = true;

                while (reader.Read())
                {
                    string featureid = reader["Guid"].ToString();

                    if (!WebConfigSettings.SearchableFeatureGuidsToExclude.Contains(featureid))
                    {
                        listItem = new ListItem(
                            ResourceHelper.GetResourceString(
                            reader["ResourceFile"].ToString(),
                            reader["SearchListName"].ToString(),
                            useConfigOverrides),
                            featureid);

                        ddFeatureList.Items.Add(listItem);
                    }
                    
                }

            }

        }

        

        private void InitIndexIfNeeded()
        {
            if (indexVerified) { return; }

            indexVerified = true;
            if (!mojoPortal.SearchIndex.IndexHelper.VerifySearchIndex(siteSettings))
            {
                this.lblMessage.Text = Resource.SearchResultsBuildingIndexMessage;
                Thread.Sleep(5000); //wait 5 seconds
                SiteUtils.QueueIndexing();
            }
            
            //lblDuration.Visible = false;
            //lblSeconds.Visible = false;
            //pnlSearchResults.Visible = false;
            //pnlNoResults.Visible = true;
           

        }

        


	    private void ShowNoResults()
        {
            if (queryErrorOccurred)
            {
                lblNoResults.Text = Resource.SearchQueryInvalid;
            }
            divResults.Visible = false;
            pnlNoResults.Visible = (txtSearchInput.Text.Length > 0);
            

            //this.lblFrom.Text = "0";
            //this.lblTo.Text = "0";
            //this.lblTotal.Text = "0";
            //this.lblQueryText.Text = Server.HtmlEncode(query);
            

            //divResults.Visible = (txtSearchInput.Text.Length > 0);

            
            
        }

        protected string FormatLinkText(string pageName, string moduleTtile, string itemTitle)
        {
            if (showModuleTitleInResultLink)
            {
                if (itemTitle.Length > 0)
                {
                    return pageName + " &gt; " + moduleTtile + " &gt; " + itemTitle;
                }

            }

            if (itemTitle.Length > 0)
            {
                return pageName +  " &gt; " + itemTitle;
            }


            return pageName;

  
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

            string redirectUrl = SiteRoot + "/SearchResults.aspx?q="
                    + Server.UrlEncode(this.txtSearchInput.Text)
                    + GetModBeginDateParam(false)
                    + GetModEndDateParam(false)
                    + GetFeatureParam(false);
                    ;

            WebUtils.SetupRedirect(this, redirectUrl);
            
            //if (ddFeatureList.SelectedValue.Length == 36)
            //{
            //    WebUtils.SetupRedirect(this, SiteRoot + "/SearchResults.aspx?q=" 
            //        + Server.UrlEncode(this.txtSearchInput.Text)
            //        + "&f=" + ddFeatureList.SelectedValue
            //        );

            //    return;
            //}

            //WebUtils.SetupRedirect(this, SiteRoot + "/SearchResults.aspx?q=" + Server.UrlEncode(this.txtSearchInput.Text));

        }

        void btnRebuildSearchIndex_Click(object sender, EventArgs e)
        {
            IndexingQueue.DeleteAll();
			IndexHelper.DeleteSearchIndex(siteSettings);
			IndexHelper.VerifySearchIndex(siteSettings);
            
            this.lblMessage.Text = Resource.SearchResultsBuildingIndexMessage;
            Thread.Sleep(5000); //wait 5 seconds
            SiteUtils.QueueIndexing();
        }

        private void SetupScript()
        {
            if (WebConfigSettings.DisablejQuery) { return; }
            if (!WebConfigSettings.OpenSearchDownloadLinksInNewWindow) { return; }

            // make shared files download links open in a new window
            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            script.Append("$(\"a[href*='Download.aspx']\")");
            //script.Append(".bind('click', function(){return confirm('sure you want to download ?')});");
            script.Append(".bind('click', function(){window.open(this.href,'_blank');return false;}); ");

            script.Append("\n</script>");

            this.Page.ClientScript.RegisterStartupScript(
                typeof(Page),
                "searchpage",
                script.ToString());

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

            if (displaySettings.ShowCreatedDate || displaySettings.ShowLastModDate) { timeZone = SiteUtils.GetUserTimeZone(); }
        }


		private void PopulateLabels()
		{
            if (siteSettings == null) return;

            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.SearchPageTitle);

            heading.Text = Resource.SearchPageTitle;

            MetaDescription = string.Format(CultureInfo.InvariantCulture,
                Resource.MetaDescriptionSearchFormat, siteSettings.SiteName);

			lblMessage.Text = string.Empty;
            divResults.Visible = true;

            btnDoSearch.Text = Resource.SearchButtonText;
            SiteUtils.SetButtonAccessKey(btnDoSearch, AccessKeys.SearchButtonTextAccessKey);

            btnRebuildSearchIndex.Text = Resource.SearchRebuildIndexButton;
            UIHelper.AddConfirmationDialog(btnRebuildSearchIndex, Resource.SearchRebuildIndexWarning);

            divDelete.Visible = (WebConfigSettings.ShowRebuildSearchIndexButtonToAdmins && WebUser.IsAdmin);

            lblNoResults.Text = Resource.SearchResultsNotFound;

            litAltSearchMessage.Text = Resource.AltSearchPrompt;
            lnkBingSearch.Text = Resource.SearchThisSiteWithBing;
            lnkBingSearch.NavigateUrl = SiteRoot + "/BingSearch.aspx";
            lnkGoogleSearch.Text = Resource.SearchThisSiteWithGoogle;
            lnkGoogleSearch.NavigateUrl = SiteRoot + "/GoogleSearch.aspx";

            litDatePreamble.Text = Resource.SearchDateFilterPreamble;
            litAnd.Text = Resource.and;

            
            //this page has no content other than nav
            SiteUtils.AddNoIndexFollowMeta(Page);

            AddClassToBody("searchresults");
            
		}

		public string BuildUrl(IndexItem indexItem)
		{
			string value = string.Empty;
			if (indexItem.UseQueryStringParams)
			{
				value = "/" + indexItem.ViewPage
					+ "?pageid="
					+ indexItem.PageId.ToInvariantString()
					+ "&mid="
					+ indexItem.ModuleId.ToInvariantString()
					+ "&ItemID="
					+ indexItem.ItemId.ToInvariantString()
					+ indexItem.QueryStringAddendum;
			}
			else
			{
				value = "/" + indexItem.ViewPage;
			}

			if (value.StartsWith("/"))
			{
				value = SiteRoot + value;
			}

			return value;
		}

		public string FormatCreatedDate(IndexItem indexItem)
        {
            if ((!displaySettings.ShowCreatedDate)||(timeZone == null)) { return string.Empty; }

            if (indexItem.CreatedUtc.Date == DateTime.MinValue.Date) { return string.Empty; }

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
            if ((!displaySettings.ShowLastModDate) || (timeZone == null)) { return string.Empty; }

            if (indexItem.LastModUtc.Date == DateTime.MinValue.Date) { return string.Empty; }

            if (displaySettings.ModifiedFormat.Length > 0)
            {
                return string.Format(
                    CultureInfo.CurrentCulture,
                    displaySettings.ModifiedFormat,
                    TimeZoneInfo.ConvertTimeFromUtc(indexItem.LastModUtc, timeZone).ToShortDateString());
            }

            return string.Format(
                    CultureInfo.CurrentCulture,
                    Resource.SearchModifiedHtmlFormat,
                    TimeZoneInfo.ConvertTimeFromUtc(indexItem.LastModUtc, timeZone).ToShortDateString());

            

        }

        protected string FormatAuthor(string author)
        {
            if ((!displaySettings.ShowAuthor)||(string.IsNullOrEmpty(author))) { return string.Empty; }

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
	}
}

namespace mojoPortal.Web.UI
{
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    /// <summary>
    /// this control doesn't render anything, it is used only as a themeable collection of settings for things we would like to be able to configure from theme.skin
    /// </summary>
    public class SearchResultsDisplaySettings : WebControl
    {
        private string itemHeadingElement = "h3";

        public string ItemHeadingElement
        {
            get { return itemHeadingElement; }
            set { itemHeadingElement = value; }
        }

        private bool showAuthor = false;

        public bool ShowAuthor
        {
            get { return showAuthor; }
            set { showAuthor = value; }
        }

        private string authorFormat = string.Empty;

        public string AuthorFormat
        {
            get { return authorFormat; }
            set { authorFormat = value; }
        }

        private bool showCreatedDate = false;

        public bool ShowCreatedDate
        {
            get { return showCreatedDate; }
            set { showCreatedDate = value; }
        }

        private string createdFormat = string.Empty;

        public string CreatedFormat
        {
            get { return createdFormat; }
            set { createdFormat = value; }
        }

        private bool showLastModDate = false;

        public bool ShowLastModDate
        {
            get { return showLastModDate; }
            set { showLastModDate = value; }
        }

        private string modifiedFormat = string.Empty;

        public string ModifiedFormat
        {
            get { return modifiedFormat; }
            set { modifiedFormat = value; }
        }

        private bool showModifiedDateFilters = false;

        public bool ShowModifiedDateFilters
        {
            get { return showModifiedDateFilters; }
            set { showModifiedDateFilters = value; }
        }

        private bool showFeatureFilter = true;

        public bool ShowFeatureFilter
        {
            get { return showFeatureFilter; }
            set { showFeatureFilter = value; }
        }

        
        protected override void Render(HtmlTextWriter writer)
        {
            // nothing to render, this control is just a themeable property bag
        }
    }

}
