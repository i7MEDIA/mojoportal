//	Author:				
//	Created:			2010-03-15
//	Last Modified:		2022-01-05
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Controls.google;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// 2013-12-31 - updated for use with Universal Analytics by set UseUniversal property to true from layout.master
    /// 
    /// This control is used in conjunction with AnalyticsAsyncBottomScript and in replacement for mojoGoogleAnalyticsScript
    /// if you want to use the async loading approach described here:
    /// http://code.google.com/apis/analytics/docs/tracking/asyncUsageGuide.html#SplitSnippet
    /// 
    /// Remove mojoGoogleAnalyticsScript from your layout.master and replace with AnalyticsAsyncBottomScript just before the closing form element
    /// Add AnalyticsAsyncTopScript to layout.master just below the opening body element
    /// </summary>
    /// 
    [Obsolete("Control replaced with logic in ScriptLoader. Remove this control from your skins.")]
    public class AnalyticsAsyncTopScript : WebControl
    {
        private SiteSettings siteSettings = null;
        private string googleAnalyticsProfileId = string.Empty;
        private List<AnalyticsTransaction> transactions = new List<AnalyticsTransaction>();
        private string memberLabel = "member-type";
        private string memberType = "member";
        private string pageToTrack = string.Empty;
        private string sectionKey = "section";
        private string section = string.Empty;
        private string overrideDomain = string.Empty;
        private bool logToLocalServer = false;


        /// <summary>
        /// This control should no longer be used and will be removed in a future version.
        /// Google Analytics completely changed and this control only ever support GA.
        /// This control has been replaced with logic in ScriptLoader.
        /// </summary>
        public bool Disable { get; set; } = true;

        /// <summary>
        /// Requires at least one item
        /// </summary>
        public List<AnalyticsTransaction> Transactions
        {
            get { return transactions; }
        }

        /// <summary>
        /// If true pageTracker._setLocalRemoteServerMode(); wil be called resulting in analytics data appearing in the web logs.
        /// This data can be used for additional offline processing using Urchin.
        /// </summary>
        public bool LogToLocalServer
        {
            get { return logToLocalServer; }
            set { logToLocalServer = value; }
        }

        /// <summary>
        /// If you want to ovveride how the page is tracked you can enter it here.
        /// Like in SearchResults.aspx we track /SearchResults.aspx?q=searchterm
        /// </summary>
        public string PageToTrack
        {
            get { return pageToTrack; }
            set { pageToTrack = value; }
        }

        /// <summary>
        /// If specified will set a customVar at page level scope in slot 2
        /// </summary>
        public string Section
        {
            get { return section; }
            set { section = value; }
        }

        /// <summary>
        /// If you specify an overridedomain, ._setDomainName(...) will be called
        /// http://code.google.com/apis/analytics/docs/tracking/gaTrackingSite.html
        /// </summary>
        public string OverrideDomain
        {
            get { return overrideDomain; }
            set { overrideDomain = value; }
        }

        private bool setAllowLinker = true;
        public bool SetAllowLinker
        {
            get { return setAllowLinker; }
            set { setAllowLinker = value; }
        }

        private bool setAllowHash = true;
        public bool SetAllowHash
        {
            get { return setAllowHash; }
            set { setAllowHash = value; }
        }

        private bool anonymizeIp = true;
        public bool AnonymizeIp
        {
            get { return anonymizeIp; }
            set { anonymizeIp = value; }
        }

        private bool trackPageLoadTime = true;

        public bool TrackPageLoadTime
        {
            get { return trackPageLoadTime; }
            set { trackPageLoadTime = value; }
        }

        

        private int adjustedBounceTimeoutSeconds = -1;
        /// <summary>
        /// http://analytics.blogspot.com/2012/07/tracking-adjusted-bounce-rate-in-google.html
        /// set to something > -1 to enable this
        /// </summary>
        public int AdjustedBounceTimeoutSeconds
        {
            get { return adjustedBounceTimeoutSeconds; }
            set { adjustedBounceTimeoutSeconds = value; }
        }

        private bool useUniversal = true; 

        public bool UseUniversal
        {
            get { return useUniversal; }
            set { useUniversal = value; }
        }

        private bool enableDisplayFeatures = false;
        /// <summary>
        /// if you set this to true it will render: ga('require', 'displayfeatures');
        /// which will make it track double click advertising cookies in addition to the analytics cookie
        /// if you do this you should update your privacy policy
        /// see also:https://support.google.com/analytics/answer/2444872?hl=en&utm_id=ad
        /// </summary>
        public bool EnableDisplayFeatures
        {
            get { return enableDisplayFeatures; }
            set { enableDisplayFeatures = value; }
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            DoInit();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

			if (Disable) return;

			if (WebConfigSettings.GoogleAnalyticsForceUniversal)
            {
                useUniversal = true;
            }

            if (WebConfigSettings.GoogleAnalyticsRolesToExclude.Length > 0)
            {
                if (WebUser.IsInRoles(WebConfigSettings.GoogleAnalyticsRolesToExclude))
                {
                    Visible = false; //prevent rendering
                }

            }
           
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

			if (Disable) return;

			if (string.IsNullOrEmpty(section))
            {
                if (Page is mojoBasePage)
                {
                    mojoBasePage basePage = Page as mojoBasePage;
                    section = basePage.AnalyticsSection;
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
			if (Disable) return;

			if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                if (useUniversal)
                {
                    RenderUniversal(writer);
                }
                else
                {
                    RenderLegacy(writer);
                }
            }
        }

        protected void RenderUniversal(HtmlTextWriter writer)
        {
            //https://developers.google.com/analytics/devguides/collection/analyticsjs/advanced

            if (string.IsNullOrEmpty(googleAnalyticsProfileId)) { return; }

            writer.Write("<script type=\"text/javascript\"> ");
            writer.Write("\n");

            writer.Write("(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){");
            writer.Write("\n");

            writer.Write("(i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),");
            writer.Write("\n");
            writer.Write("m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)");
            writer.Write("\n");
            writer.Write("})(window,document,'script','//www.google-analytics.com/analytics.js','ga');");
            writer.Write("\n");

            writer.Write("ga('create', '" + googleAnalyticsProfileId + "'");

            if(Page.Request.Url.ToString().Contains("localhost"))
            {
                writer.Write(", { 'cookieDomain': 'none'}");
            }
            else if (overrideDomain.Length > 0)
            {
                writer.Write(", '" + overrideDomain + "'");
                if (setAllowLinker)
                {
                    writer.Write(",{'allowLinker': true}");
                }
            }
            else
            {
                writer.Write(", 'auto'");
            }

            writer.Write(");");

            //https://www.mojoportal.com/Forums/Thread.aspx?thread=10380&mid=34&pageid=5&ItemID=4&pagenumber=1#post43209

            if (enableDisplayFeatures)
            {
                writer.Write("\n");
                writer.Write("ga('require', 'displayfeatures');");
            }

            if (anonymizeIp)
            {
                writer.Write("\n");
                writer.Write("ga('set', 'anonymizeIp', true);");
                
            }


            //if (overrideDomain.Length > 0)
            //{
            //    //http://code.google.com/apis/analytics/docs/tracking/gaTrackingSite.html

            //    writer.Write("_gaq.push(['_setDomainName','" + overrideDomain + "']);");


            //    // default is false so we only have to set it if we want true
            //    if (setAllowLinker)
            //    {
            //        writer.Write("_gaq.push(['_setAllowLinker',true]);");

            //    }

            //    //default is true so we only have to set it if we want false
            //    if (!setAllowHash)
            //    {
            //        writer.Write("_gaq.push(['_setAllowHash',false]);");

            //    }
            //}

            //if (logToLocalServer)
            //{
            //    writer.Write("_gaq.push(['_setLocalRemoteServerMode']);");

            //}

            SetupUniversalUserTracking(writer);
            SetupUniversalSectionTracking(writer);

            SetupUniversalTransactions(writer);

            if (pageToTrack.Length > 0)
            {
                writer.Write(" ga('send', 'pageview','" + pageToTrack.Replace("'", string.Empty).Replace("\"", string.Empty) + "'); ");
            }
            else
            {
                writer.Write(" ga('send', 'pageview'); ");
            }
            writer.Write("\n");

            //if (adjustedBounceTimeoutSeconds > -1)
            //{
            //    //http://analytics.blogspot.com/2012/07/tracking-adjusted-bounce-rate-in-google.html

            //    writer.Write(" setTimeout(\"_gaq.push(['_trackEvent', '" + adjustedBounceTimeoutSeconds.ToInvariantString()
            //        + "_seconds', 'read'])\"," + (adjustedBounceTimeoutSeconds * 1000).ToInvariantString() + "); ");

            //    writer.Write("\n");
            //}

            //// new feature in google analytics 2011-05-06 Site Speed Report
            //if (trackPageLoadTime)
            //{
            //    writer.Write(" _gaq.push(['_trackPageLoadTime']); ");
            //    writer.Write("\n");
            //}

            writer.Write(" </script>");

        }

        

        private void SetupUniversalSectionTracking(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(section)) { return; }

            // TODO:? I think custom dimensions have to be setup in the account and we don't know what they are
            //ga('set', 'dimension2', 'Paid');
            //https://support.google.com/analytics/answer/2709829

            //writer.Write("_gaq.push(['_setCustomVar', 2, '" + sectionKey + "', '" + section + "', 3]);");

        }

        

        private void SetupUniversalUserTracking(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(memberLabel)) { return; }

            // TODO:? I think custom dimensions have to be setup in the account and we don't know what they are
            //ga('set', 'dimension1', 'Paid');
            //https://support.google.com/analytics/answer/2709829

            //writer.Write("_gaq.push(['_setCustomVar', 1, '" + memberType + "', '" + memberLabel + "', 1]);");

        }

        
        private void SetupUniversalTransactions(HtmlTextWriter writer)
        {
            bool foundValidTransactions = false;

            if(transactions.Count > 0)
            {
                writer.Write("ga('require', 'ecommerce', 'ecommerce.js'); ");
            }

            foreach (AnalyticsTransaction transaction in transactions)
            {
                if (transaction.IsValid())
                {
                    foundValidTransactions = true;

                    writer.Write("ga('ecommerce:addTransaction', {");
                    writer.Write("'id':'" + transaction.OrderId + "'");
                    writer.Write(",'affiliation':'" + transaction.StoreName + "'");
                    writer.Write(",'revenue':'" + transaction.Total + "'");
                    writer.Write(",'shipping':'" + transaction.Shipping + "'");
                    writer.Write(",'tax':" + transaction.Tax + "'");
                    
                    //writer.Write("\"" + transaction.City + "\",");
                    //writer.Write("\"" + transaction.State + "\",");
                    //writer.Write("\"" + transaction.Country + "\"");
                    writer.Write("});");


                    SetupUniversalTransactionItems(writer, transaction);

                }
            }

            if (foundValidTransactions)
            {
                writer.Write("ga('ecommerce:send'); ");
                writer.Write("ga('send','pageview','/TransactionComplete.aspx');");
            }

        }

        

        private void SetupUniversalTransactionItems(HtmlTextWriter writer, AnalyticsTransaction transaction)
        {
            foreach (AnalyticsTransactionItem item in transaction.Items)
            {
                if (item.IsValid())
                {
                    writer.Write("ga('ecommerce:addItem', {");
                    writer.Write("'id':'" + item.OrderId + "'");
                    writer.Write(",'name':'" + item.ProductName + "'");
                    writer.Write(",'sku':'" + item.Sku + "'");
                    writer.Write(",'category':'" + item.Category + "'");
                    writer.Write(",'price':'" + item.Price + "'");
                    writer.Write(",'quantity':'" + item.Quantity + "'");
                    writer.Write("});");

                }
            }

        }



        protected void RenderLegacy(HtmlTextWriter writer)
        {

            if (string.IsNullOrEmpty(googleAnalyticsProfileId)) { return; }

            writer.Write("<script type=\"text/javascript\"> ");
            writer.Write("\n");

            writer.Write("var _gaq = _gaq || []; ");
            writer.Write("\n");

            writer.Write("_gaq.push(['_setAccount','" + googleAnalyticsProfileId + "']); ");
            writer.Write("\n");

            //https://www.mojoportal.com/Forums/Thread.aspx?thread=10380&mid=34&pageid=5&ItemID=4&pagenumber=1#post43209
            //https://developers.google.com/analytics/devguides/collection/gajs/methods/gaJSApi_gat?hl=de#_gat._anonymizeIp
            if (anonymizeIp)
            {
                writer.Write("_gaq.push(['_gat._anonymizeIp']);");
                writer.Write("\n");

            }


            if (overrideDomain.Length > 0)
            {
                //http://code.google.com/apis/analytics/docs/tracking/gaTrackingSite.html

                writer.Write("_gaq.push(['_setDomainName','" + overrideDomain + "']);");


                // default is false so we only have to set it if we want true
                if (setAllowLinker)
                {
                    writer.Write("_gaq.push(['_setAllowLinker',true]);");

                }

                //default is true so we only have to set it if we want false
                if (!setAllowHash)
                {
                    writer.Write("_gaq.push(['_setAllowHash',false]);");

                }
            }

            if (logToLocalServer)
            {
                writer.Write("_gaq.push(['_setLocalRemoteServerMode']);");

            }

            SetupUserTracking(writer);
            SetupSectionTracking(writer);
            SetupTransactions(writer);

            if (pageToTrack.Length > 0)
            {
                writer.Write(" _gaq.push(['_trackPageview','" + pageToTrack.Replace("'", string.Empty).Replace("\"", string.Empty) + "']); ");
            }
            else
            {
                writer.Write(" _gaq.push(['_trackPageview']); ");
            }
            writer.Write("\n");

            if (adjustedBounceTimeoutSeconds > -1)
            {
                //http://analytics.blogspot.com/2012/07/tracking-adjusted-bounce-rate-in-google.html

                writer.Write(" setTimeout(\"_gaq.push(['_trackEvent', '" + adjustedBounceTimeoutSeconds.ToInvariantString()
                    + "_seconds', 'read'])\"," + (adjustedBounceTimeoutSeconds * 1000).ToInvariantString() + "); ");

                writer.Write("\n");
            }

            // new feature in google analytics 2011-05-06 Site Speed Report
            if (trackPageLoadTime)
            {
                writer.Write(" _gaq.push(['_trackPageLoadTime']); ");
                writer.Write("\n");
            }

            writer.Write(" </script>");

        }

        //http://code.google.com/apis/analytics/docs/gaJS/gaJSApiBasicConfiguration.html#_gat.GA_Tracker_._setCustomVar
        //http://code.google.com/apis/analytics/docs/tracking/gaTrackingCustomVariables.html#mixedTypes
        //http://conversionroom.blogspot.com/2010/02/spotlight-on-google-analytics-features.html


        // we are using slot 1 for visitor and slot 2 for section, must use one of the 3 other slots for other custom vars

        private void SetupSectionTracking(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(section)) { return; }

            writer.Write("_gaq.push(['_setCustomVar', 2, '" + sectionKey + "', '" + section + "', 3]);");

        }


        private void SetupUserTracking(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(memberLabel)) { return; }

            writer.Write("_gaq.push(['_setCustomVar', 1, '" + memberType + "', '" + memberLabel + "', 1]);");

        }



        private void SetupTransactions(HtmlTextWriter writer)
        {
            bool foundValidTransactions = false;

            foreach (AnalyticsTransaction transaction in transactions)
            {
                if (transaction.IsValid())
                {
                    foundValidTransactions = true;

                    writer.Write("_gaq.push(['_addTrans',");
                    writer.Write("'" + transaction.OrderId + "',");
                    writer.Write("'" + transaction.StoreName + "',");
                    writer.Write("\"" + transaction.Total + "\",");
                    writer.Write("\"" + transaction.Tax + "\",");
                    writer.Write("\"" + transaction.Shipping + "\",");
                    writer.Write("\"" + transaction.City + "\",");
                    writer.Write("\"" + transaction.State + "\",");
                    writer.Write("\"" + transaction.Country + "\"");
                    writer.Write("]);");


                    SetupTransactionItems(writer, transaction);

                }
            }

            if (foundValidTransactions)
            {
                writer.Write("_gaq.push(['_trackTrans']);");
                writer.Write("_gaq.push(['_trackPageview','/TransactionComplete.aspx']);");
            }

        }

        private void SetupTransactionItems(HtmlTextWriter writer, AnalyticsTransaction transaction)
        {
            foreach (AnalyticsTransactionItem item in transaction.Items)
            {
                if (item.IsValid())
                {
                    writer.Write("_gaq.push(['_addItem',");
                    writer.Write("'" + item.OrderId + "',");
                    writer.Write("'" + item.Sku + "',");
                    writer.Write("'" + item.ProductName + "',");
                    writer.Write("'" + item.Category + "',");
                    writer.Write("'" + item.Price + "',");
                    writer.Write("'" + item.Quantity + "'");
                    writer.Write("]);");
                    
                }
            }

        }

        private void DoInit()
        {
            if (HttpContext.Current == null) { return; }

            this.EnableViewState = false;

            memberType = WebConfigSettings.GoogleAnalyticsMemberLabel;
            memberLabel = WebConfigSettings.GoogleAnalyticsMemberTypeAnonymous;
            LogToLocalServer = WebConfigSettings.LogGoogleAnalyticsDataToLocalWebLog;
            sectionKey = WebConfigSettings.GoogleAnalyticsSectionLabel;


            // lets always label Admins as admins, regardless whether they are also customers
            if (WebUser.IsAdminOrContentAdmin)
            {
                memberLabel = WebConfigSettings.GoogleAnalyticsMemberTypeAdmin;
            }
            else
            {
                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                    if ((siteUser != null) && (siteUser.TotalRevenue > 0))
                    {
                        memberLabel = WebConfigSettings.GoogleAnalyticsMemberTypeCustomer;
                    }
                    else
                    {
                        memberLabel = WebConfigSettings.GoogleAnalyticsMemberTypeAuthenticated;
                    }

                }
            }

            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if ((siteSettings != null) && (siteSettings.GoogleAnalyticsAccountCode.Length > 0))
            {
                googleAnalyticsProfileId = siteSettings.GoogleAnalyticsAccountCode;

            }
            
            // let Web.config setting trump site settings. this meets my needs where I want to track the demo site but am letting people login as admin
            // this way if the remove or change it in site settings it still uses my profile id
            if (ConfigurationManager.AppSettings["GoogleAnalyticsProfileId"] != null)
            {
                googleAnalyticsProfileId = ConfigurationManager.AppSettings["GoogleAnalyticsProfileId"].ToString();
                return;
            }

            Disable = !googleAnalyticsProfileId.StartsWith("UA");                        
        }
    }
}
