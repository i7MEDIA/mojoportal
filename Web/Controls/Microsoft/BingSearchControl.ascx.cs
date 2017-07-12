//	Author:				
//	Created:			2010-06-07
//	Last Modified:		2012-07-04
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 2012-07-04 updated to use the new Bing api from azure market place


using System;
//using Microsoft.Samples.Web.Bing.Model;
//using Microsoft.Samples.Web.Linq2Bing;
using System.Web.UI;
using System.Net;
using Resources;
using mojoPortal.Web.Framework;
#if !MONO
using Bing;
#endif

namespace mojoPortal.Web.UI
{
    public partial class BingSearchControl : UserControl
    {
        private string searchDomain = string.Empty;
        private string bingAccountKey = string.Empty;
        private string queryText = string.Empty;
        private int pageSize = 10;
        private int resultCount = 0;
        


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Visible) { return; }

            LoadSettings();

            if (bingAccountKey.Length == 0) { this.Visible = false; return; }

            PopulateLabels();
            BindResults();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            UpdateResults();
        }

        void btnFirst_Click(object sender, EventArgs e)
        {
            Offset = 0;
            UpdateResults();
        }

        void btnPrevious_Click(object sender, EventArgs e)
        {
            if (Offset >= pageSize) { Offset = (Offset - pageSize); }

            UpdateResults();
        }

        void btnNext_Click(object sender, EventArgs e)
        {
            Offset = (Offset + pageSize);

            UpdateResults();

        }

        void UpdateResults()
        {
            // if the user types something new in the search box we need to start on the first page again
            if (txtSearch.Text.GetHashCode().ToInvariantString() != hdnSHC.Value) { Offset = 0; }

            queryText = txtSearch.Text;
            BindResults();

            upBing.Update();
        }

        void BindResults()
        {
            if (queryText.Length == 0) { return; }

            hdnSHC.Value = queryText.GetHashCode().ToInvariantString();

            //Linq2Bing bing = new Linq2Bing(bingApiId);
            //IQueryable<BaseResult> LinqExpression;

            //if (searchDomain.Length > 0)
            //{
            //    LinqExpression =
            //       (from w in bing.Web
            //       where w.Site == searchDomain && w.Text == queryText
            //        select w).Skip(Offset).Take(pageSize);
            //}
            //else
            //{
            //    LinqExpression =
            //       (from w in bing.Web
            //       where w.Text == queryText
            //        select w).Skip(Offset).Take(pageSize);
                   
            //}

#if !MONO
            BingSearchContainer bingContainer = new BingSearchContainer(new Uri(WebConfigSettings.BingApiUrl)); 
             
            bingContainer.Credentials = new NetworkCredential(bingAccountKey, bingAccountKey);
            var webResults = bingContainer.Web("site:" + searchDomain + " " + queryText, 
                null, 
                null, 
                null, 
                null, 
                null, 
                null,
                pageSize,
                Offset); 

            try
            {
                //resultCount = LinqExpression.Web().Count<BaseResult>();
                //rptResults.DataSource = LinqExpression.Web();
                rptResults.DataSource = webResults;
                rptResults.DataBind();
                resultCount = rptResults.Items.Count;

            }
            catch (ArgumentNullException) { } //happens when there are no results

            // unfortunately I have not found a way to find the total number of search hits so
            // we can't use traditional paging but can only do next previous first type paging
            // since we don't know how many total pages there are

            btnNext.Visible = (resultCount >= pageSize);

            if (Offset < pageSize)
            {
                btnFirst.Visible = false;
                btnPrevious.Visible = false;
            }
            else
            {
                btnFirst.Visible = true;
                btnPrevious.Visible = true;
            }

            //if (resultCount > 0)
            //{
            //    rptResults.DataSource = LinqExpression.Web();
            //    rptResults.DataBind();
            //}

            pnlNoResults.Visible = (resultCount == 0);

#endif
            

        }

        private int Offset
        {
            get { return Convert.ToInt32(hdnOffset.Value); }
            set { hdnOffset.Value = value.ToInvariantString(); }

           
        }

        private void PopulateLabels()
        {
            btnSearch.Text = Resource.SearchButtonText;
            lblPoweredByBing.Text = Resource.PoweredByBing;
            btnFirst.Text = Resource.FirstButton;
            btnPrevious.Text = Resource.PreviousButton;
            btnNext.Text = Resource.NextButton;
            lblNoResults.Text = Resource.SearchResultsNotFound;
        }

        private void LoadSettings()
        {
            bingAccountKey = SiteUtils.GetBingApiId();
            searchDomain = SiteUtils.GetSearchDomain();
            pageSize = WebConfigSettings.BingSearchPageSize;
            
            // don't touch the textbox on postback, we want what the user entered
            if (Page.IsPostBack) { return; }

            if (Request.QueryString.Get("q") == null) { return; }

            queryText = Request.QueryString.Get("q");
            if (queryText.Length > 0)
            {
                txtSearch.Text = SecurityHelper.SanitizeHtml(queryText);

            }

        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);

            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnFirst.Click += new EventHandler(btnFirst_Click);
            btnNext.Click += new EventHandler(btnNext_Click);
            btnPrevious.Click += new EventHandler(btnPrevious_Click);
        }

       

        

        
    }

    
}