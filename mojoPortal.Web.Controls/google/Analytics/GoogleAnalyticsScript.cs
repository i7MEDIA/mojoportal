//	Author:				
//	Created:			2008-08-11
//	Last Modified:		2010-03-18
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using mojoPortal.Web.Controls.google;

namespace mojoPortal.Web.Controls
{
    /// <summary>
    /// sets up google analytics tacking	
    /// </summary>
    public class GoogleAnalyticsScript : WebControl
    {
        private string googleAnalyticsProfileId = string.Empty;
        private string trackerName = "pTracker";
        private List<AnalyticsTransaction> transactions = new List<AnalyticsTransaction>();
        private bool trackPageLoadTime = false;
        private bool logToLocalServer = false;
        private string memberLabel = string.Empty;
        private string memberType = "member-type";
        private string pageToTrack = string.Empty;
        private string overrideDomain = string.Empty;

       
        
        #region Public Properties

        /// <summary>
        /// Requires at least one item
        /// </summary>
        public List<AnalyticsTransaction> Transactions
        {
            get { return transactions; }
        }
        
        /// <summary>
        /// Tracking code for google analytics
        /// </summary>
        public string GoogleAnalyticsProfileId
        {
            get { return googleAnalyticsProfileId; }
            set { googleAnalyticsProfileId = value; }
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

        /// <summary>
        /// Instance name of the PageTracker 
        /// </summary>
        public string TrackerName
        {
            get { return trackerName; }
            set { trackerName = value; }
        }

        /// <summary>
        /// If true page load times will be measured
        /// </summary>
        public bool TrackPageLoadTime
        {
            get { return trackPageLoadTime; }
            set { trackPageLoadTime = value; }
        }

        /// <summary>
        /// A key for tracking authenticated user.
        /// </summary>
        public string MemberLabel
        {
            get { return memberLabel; }
            set { memberLabel = value; }
        }

        
        /// <summary>
        /// A label for tagging customers or members
        /// </summary>
        public string MemberType
        {
            get { return memberType; }
            set { memberType = value; }
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

        private string overrideScriptUrl = string.Empty;
        /// <summary>
        /// if you want to host the script locally put the path for the src=
        /// </summary>
        public string OverrideScriptUrl
        {
            get { return overrideScriptUrl; }
            set { overrideScriptUrl = value; }
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

        #endregion

        /// <summary>
        /// Last chance to change what gets rendered.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (this.googleAnalyticsProfileId.Length == 0) { return; }

            SetupMainScript();
            SetupTracker();
        }

        


        private void SetupTracker()
        {
            StringBuilder script = new StringBuilder();
            script.Append("\n");
            script.Append("<script type=\"text/javascript\"> ");
            script.Append("\n");

            //script.Append(" if(_gat != undefined){");
            script.Append("try{\n");
            script.Append("var " + trackerName + " = _gat._getTracker(\"" + googleAnalyticsProfileId + "\");");
            script.Append("\n");
            
            if (overrideDomain.Length > 0)
            {
                //http://code.google.com/apis/analytics/docs/tracking/gaTrackingSite.html

                script.Append(trackerName + "._setDomainName(\"" + overrideDomain + "\");");
                script.Append("\n");

                // default is false so we only have to set it if we want true
                if (setAllowLinker)
                {
                    script.Append(trackerName + "._setAllowLinker(true);");
                    script.Append("\n");
                }

                //default is true so we only have to set it if we want false
                if (!setAllowHash)
                {
                    script.Append(trackerName + "._setAllowHash(false);");
                    script.Append("\n");
                }
            }

            if (logToLocalServer)
            {
                script.Append(trackerName + "._setLocalRemoteServerMode();");
                script.Append("\n");
            }

            SetupUserTracking(script);

            
            if (pageToTrack.Length > 0)
            {
                script.Append(trackerName + "._trackPageview('" + pageToTrack.Replace("'", string.Empty).Replace("\"", string.Empty) + "');");
            }
            else
            {
                script.Append(trackerName + "._trackPageview();");
            }
            script.Append("\n");
            

            SetupTransactions(script);

            if (trackPageLoadTime) { SetupPageLoadPerformanceTracking(script); }

            script.Append("} catch(err) {}");

            //script.Append("}"); //end if (_gat != 'undefined

            script.Append(" </script>");

            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                trackerName,
                script.ToString());
        }

        private void SetupUserTracking(StringBuilder script)
        {
            if (string.IsNullOrEmpty(memberLabel)) { return; }

            script.Append("\n");
            // deprecated
            //script.Append(trackerName + "._setVar(\"" + memberLabel  + "\");");
            script.Append(trackerName + "._setCustomVar(1, \"" + memberType + "\", \"" + memberLabel + "\", 1);");
                
            

        }

        

        private void SetupTransactions(StringBuilder script)
        {
            bool foundValidTransactions = false;

            foreach (AnalyticsTransaction transaction in transactions)
            {
                if (transaction.IsValid())
                {
                    foundValidTransactions = true;

                    script.Append(trackerName + "._addTrans(");
                    script.Append("\"" + transaction.OrderId + "\",");
                    script.Append("\n");

                    script.Append("\"" + transaction.StoreName + "\",");
                    script.Append("\n");

                    script.Append("\"" + transaction.Total + "\",");
                    script.Append("\n");

                    script.Append("\"" + transaction.Tax + "\",");
                    script.Append("\n");

                    script.Append("\"" + transaction.Shipping + "\",");
                    script.Append("\n");

                    script.Append("\"" + transaction.City + "\",");
                    script.Append("\n");

                    script.Append("\"" + transaction.State + "\",");
                    script.Append("\n");

                    script.Append("\"" + transaction.Country + "\"");
                    script.Append("\n");

                    script.Append(");");
                    script.Append("\n");

                    SetupTransactionItems(script, transaction);

                }
            }

            if (foundValidTransactions)
            {
                script.Append(trackerName + "._trackTrans();");
                //deprecated
                //script.Append(trackerName + "._setVar(\"" + customerLabel + "\");");
                //script.Append(trackerName + "._setCustomVar(1, \"" + memberLabel + "\", \"" + memberType + "\", 1);");
                script.Append(trackerName + "._trackPageview('/TransactionComplete.aspx');");
            }

        }

        private void SetupTransactionItems(StringBuilder script, AnalyticsTransaction transaction)
        {
            foreach (AnalyticsTransactionItem item in transaction.Items)
            {
                if (item.IsValid())
                {
                    script.Append(trackerName + "._addItem(");
                    script.Append("\"" + item.OrderId + "\",");
                    script.Append("\n");

                    script.Append("\"" + item.Sku + "\",");
                    script.Append("\n");

                    script.Append("\"" + item.ProductName + "\",");
                    script.Append("\n");

                    script.Append("\"" + item.Category + "\",");
                    script.Append("\n");

                    script.Append("\"" + item.Price + "\",");
                    script.Append("\n");

                    script.Append("\"" + item.Quantity + "\"");
                    script.Append("\n");

                    script.Append(");");
                    script.Append("\n");
                }
            }

        }

        


        


        private void SetupMainScript()
        {
            if (trackPageLoadTime)
            {
                SetupUpperPageLoadTrackingScript();
            }

            StringBuilder script = new StringBuilder();

            if (overrideScriptUrl.Length > 0)
            {
                script.Append("\n<script type=\"text/javascript\" src=\"" + overrideScriptUrl + "\"></script>\n");
            }
            else
            {
                script.Append("\n<script type=\"text/javascript\"> ");
                script.Append("\n");
                script.Append("var gaJsHost = ((\"https:\" == document.location.protocol) ? \"https://ssl.\" : \"http://www.\");");
                script.Append("\n");
                script.Append("document.write(unescape(\"%3Cscript src='\" + gaJsHost + \"google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E\"));");

                script.Append("\n");
                script.Append("</script>");

            }

            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "ganalytics",
                script.ToString());

        }


        private void SetupUpperPageLoadTrackingScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\"> ");
            script.Append("\n");

            script.Append("var " + trackerName + "_Begin = new Date();");
            script.Append("\n");

            script.Append("var " + trackerName + "_Start = " + trackerName + "_Begin.getTime();");
            script.Append("\n");

            script.Append(" </script>");

            Page.ClientScript.RegisterClientScriptBlock(
                this.GetType(),
                trackerName + "perftop",
                script.ToString());

        }

        private void SetupPageLoadPerformanceTracking(StringBuilder script)
        {
            script.Append("var " + trackerName + "_End = new Date();");
            script.Append("\n");

            script.Append("var " + trackerName + "_Stop = " + trackerName + "_End.getTime();");
            script.Append("\n");

            script.Append("var " + trackerName + "_timeElapse = " + trackerName + "_Stop - " + trackerName + "_Start;");
            script.Append("\n");

            script.Append(trackerName + "._trackEvent('Page Load','Load - Time','" + Page.Server.HtmlEncode(Page.Request.RawUrl) + "'," + trackerName + "_timeElapse);");
            script.Append("\n");


        }


        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                // TODO: show a bmp or some other design time thing?
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                // don't render anything, just setup the scripts
            }

        }


    }
}
