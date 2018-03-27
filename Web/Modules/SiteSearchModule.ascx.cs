// Author:					
// Created:					2012-09-24
// Last Modified:			2013-02-14
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.SearchUI
{

    public partial class SiteSearchModule : SiteModuleControl
    {
        // FeatureGuid 09c0b1fc-a92d-4f3c-9f9b-b167c82d6089
        private TimeZoneInfo timeZone = null;
        protected SearchModuleConfiguration config = new SearchModuleConfiguration();
        private int totalPages = 1;
        private int totalHits = 0;
        private bool isSiteEditor = false;
        //private Guid featureGuid = Guid.Empty;
        private bool queryErrorOccurred = false;
        private DateTime modifiedBeginDate = DateTime.MinValue;
        private DateTime modifiedEndDate = DateTime.MaxValue;
        private int pageNumber = 1;
        private int pageSize = 10;

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            btnSearch.Click += btnSearch_Click;
            pgr.Command += pgr_Command;

        }

        

        

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {     
            TitleControl.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }


        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            BindSearch();

        }

        void pgr_Command(object sender, CommandEventArgs e)
        {
            pageNumber = Convert.ToInt32(e.CommandArgument);
            pgr.CurrentIndex = pageNumber;
            BindSearch();
            
        }

        private void BindSearch()
        {
            queryErrorOccurred = false;

            mojoPortal.SearchIndex.IndexItemCollection searchResults = mojoPortal.SearchIndex.IndexHelper.Search(
                siteSettings.SiteId,
                isSiteEditor,
                GetUserRoles(),
                config.GetFeatureGuids(),
                modifiedBeginDate,
                modifiedEndDate,
                txtSearch.Text,
                WebConfigSettings.EnableSearchResultsHighlighting,
                WebConfigSettings.SearchResultsFragmentSize,
                pageNumber,
                pageSize,
                WebConfigSettings.SearchMaxClauseCount,
                out totalHits,
                out queryErrorOccurred);

            totalPages = 1;
            
            if (pageSize > 0) totalPages = totalHits / pageSize;

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

            
            pgr.ShowFirstLast = true;
            pgr.PageSize = pageSize;
            pgr.PageCount = totalPages;
            pgr.Visible = (totalPages > 1);
            
            

            rptResults.DataSource = searchResults;
            rptResults.DataBind();

            updPnl.Update();

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

        public string BuildUrl(IndexItem indexItem)
        {
            if (indexItem.UseQueryStringParams)
            {
                return SiteRoot + "/" + indexItem.ViewPage
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
                return SiteRoot + "/" + indexItem.ViewPage;
            }

        }

        public string FormatCreatedDate(IndexItem indexItem)
        {
            if ((!displaySettings.ShowCreatedDate) || (timeZone == null)) { return string.Empty; }

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
            if ((!displaySettings.ShowAuthor) || (string.IsNullOrEmpty(author))) { return string.Empty; }

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
                return pageName + " &gt; " + itemTitle;
            }

            return pageName;
        }


        private void PopulateLabels()
        {
            //TitleControl.EditText = "Edit";

            
        }

        private void LoadSettings()
        {
            config = new SearchModuleConfiguration(Settings);

            btnSearch.Text = Resource.SearchButtonText;

            if (displaySettings.OverrideButtonText.Length > 0)
            {
                Search.OverrideButtonText = displaySettings.OverrideButtonText;
                btnSearch.Text = displaySettings.OverrideButtonText;
            }

            if (displaySettings.OverrideWatermarkText.Length > 0)
            {
                Search.OverrideWatermark = displaySettings.OverrideWatermarkText;
                txtSearch.Watermark = displaySettings.OverrideWatermarkText;
            }

            Search.UseWatermark = displaySettings.UseWatermark;

            if (config.ShowResultsInsteadOfRedirect)
            {
                Search.Visible = false;
                pnlSearch.Visible = true;

                if (displaySettings.ShowCreatedDate || displaySettings.ShowLastModDate) { timeZone = SiteUtils.GetUserTimeZone(); }

                pageSize = config.PageSize;
                if (displaySettings.OverridePageSize > 0)
                {
                    pageSize = displaySettings.OverridePageSize;
                }

                if (!displaySettings.UseWatermark) { txtSearch.Watermark = string.Empty; }

            }
            else
            {
                Guid[] featureGuids = config.GetFeatureGuids();
                if(featureGuids.Length > 1)
                {
                    Search.FeatureGuid = featureGuids[0];
                }
            }
            
        }


    }
}

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// this control doesn't render anything, it is used only as a themeable collection of settings for things we would like to be able to configure from theme.skin
    /// </summary>
public class SearchModuleConfiguration
    {
        public SearchModuleConfiguration()
        { }

        public SearchModuleConfiguration(Hashtable settings)
        {
            LoadSettings(settings);

        }

        public Guid[] GetFeatureGuids()
        {

            if (searchableFeature.Length == 0)
            {
                return null;

            }

            List<string> gstrings = searchableFeature.SplitOnChar(',');
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

            //searchableFeature = WebUtils.ParseGuidFromHashTable(settings, "SearchableFeature", searchableFeature);

            showResultsInsteadOfRedirect = WebUtils.ParseBoolFromHashtable(settings, "ShowSearchResultsInsteadOfRedirect", showResultsInsteadOfRedirect);


            pageSize = WebUtils.ParseInt32FromHashtable(settings, "PageSize", pageSize);

            //getCreated = WebUtils.ParseBoolFromHashtable(settings, "RecentContentUseCreatedDate", getCreated);

            showExcerpt = WebUtils.ParseBoolFromHashtable(settings, "ShowExcerpt", showExcerpt);

            //showAuthor = WebUtils.ParseBoolFromHashtable(settings, "ShowAuthor", showAuthor);

            ////showCreatedDate = WebUtils.ParseBoolFromHashtable(settings, "ShowCreatedDate", showCreatedDate);

            //showLastModDate = WebUtils.ParseBoolFromHashtable(settings, "ShowLastModDate", showLastModDate);

            
            //maxRecentItemsToGet = WebUtils.ParseInt32FromHashtable(settings, "MaxRecentItemsToGet", maxRecentItemsToGet);

            //maxDaysOldRecentItemsToGet = WebUtils.ParseInt32FromHashtable(settings, "MaxDaysOldRecentItemsToGet", maxDaysOldRecentItemsToGet);


            if (settings.Contains("SearchableFeature"))
            {
                searchableFeature = settings["SearchableFeature"].ToString();
            }

            

        }

        private string searchableFeature = "881e4e00-93e4-444c-b7b0-6672fb55de10,026cbead-2b80-4491-906d-b83e37179ccf,38aa5a84-9f5c-42eb-8f4c-105983d419fb,74bdbcc2-0e79-47ff-bcd4-a159270bf36e,dc873d76-5bf2-4ac5-bff7-434a87a3fc8e,d572f6b4-d0ed-465d-ad60-60433893b401,5a343d88-bce1-43d1-98ae-b42d77893e7b,0cefbf18-56de-11dc-8f36-bac755d89593"; // a csv of featureguids

        private bool showResultsInsteadOfRedirect = true;

        public bool ShowResultsInsteadOfRedirect
        {
            get { return showResultsInsteadOfRedirect; }
            set { showResultsInsteadOfRedirect = value; }
        }

        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
        }

        //private bool showLastModDate = true;

        //public bool ShowLastModDate
        //{
        //    get { return showLastModDate; }
        //    set { showLastModDate = value; }
        //}

        //private bool showAuthor = true;

        //public bool ShowAuthor
        //{
        //    get { return showAuthor; }
        //    set { showAuthor = value; }
        //}

        private bool showExcerpt = true;

        public bool ShowExcerpt
        {
            get { return showExcerpt; }
        }


        //private string searchableFeature = string.Empty; // a csv of featureguids

        //public string SearchableFeature
        //{
        //    get { return searchableFeature; }
        //}

        //private int maxRecentItemsToGet = 5;

        //public int MaxRecentItemsToGet
        //{
        //    get { return maxRecentItemsToGet; }
        //}

        //private bool getCreated = false;

        //public bool GetCreated
        //{
        //    get { return getCreated; }
        //}

        //private int maxDaysOldRecentItemsToGet = 30;

        //public int MaxDaysOldRecentItemsToGet
        //{
        //    get { return maxDaysOldRecentItemsToGet; }
        //}
        


    }

}

namespace mojoPortal.Web.UI
{
    public class SearchModuleDisplaySettings : WebControl
    {
        private int overridePageSize = 0;

        public int OverridePageSize
        {
            get { return overridePageSize; }
            set { overridePageSize = value; }
        }

        private string overrideButtonText = string.Empty;

        public string OverrideButtonText
        {
            get { return overrideButtonText; }
            set { overrideButtonText = value; }
        }

        private string overrideWatermarkText = string.Empty;

        public string OverrideWatermarkText
        {
            get { return overrideWatermarkText; }
            set { overrideWatermarkText = value; }
        }

        private bool useWatermark = true;

        public bool UseWatermark
        {
            get { return useWatermark; }
            set { useWatermark = value; }
        }

        private string itemHeadingElement = "h3";

        public string ItemHeadingElement
        {
            get { return itemHeadingElement; }
            set { itemHeadingElement = value; }
        }

        private bool showExcerpt = true;

        public bool ShowExcerpt
        {
            get { return showExcerpt; }
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            EnableViewState = false;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //base.Render(writer);
        }


    }

}

