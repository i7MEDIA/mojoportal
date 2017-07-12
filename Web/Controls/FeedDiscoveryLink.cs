// Author:				        
// Created:			            2011-01-25
//	Last Modified:              2011-01-25
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.


using System;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    public class FeedDiscoveryLink : Literal
    {
        private string feedUrl = string.Empty;

        public string FeedUrl
        {
            get { return feedUrl; }
            set { feedUrl = value; }
        }

        private string feedTitle = string.Empty;

        public string FeedTitle
        {
            get { return feedTitle; }
            set { feedTitle = value; }
        }

        private bool useRedirectBypassToken = true;

        public bool UseRedirectBypassToken
        {
            get { return useRedirectBypassToken; }
            set { useRedirectBypassToken = value; }
        }

        
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (feedUrl.Length == 0) { this.Visible = false; }
            if (feedTitle.Length == 0) { this.Visible = false; }

            if (!Visible) { return; }

            string redirectBypassToken = string.Empty;
            if (useRedirectBypassToken) { redirectBypassToken = "?r=" + Global.FeedRedirectBypassToken.ToString(); }

            this.Text = "<link rel=\"alternate\" type=\"application/rss+xml\" title=\"" + feedTitle + "\" href=\"" + feedUrl + redirectBypassToken + "\" />";
        }
    }
}