//	Author:                 
//  Created:			    2010-08-10
//	Last Modified:		    2014-01-15
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 2011-05-27 totally revamped this using the new official tweet this widget

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;

namespace mojoPortal.Web.UI
{
    public class TweetThisLink : WebControl
    {
        // https://twitter.com/about/resources/tweetbutton
        // https://dev.twitter.com/blog/ssl-support-tweet-button-and-follow-button

        private string protocol = "http";

        private string urlToTweet = string.Empty;

        public string UrlToTweet
        {
            get { return urlToTweet; }
            set { urlToTweet = value; }
        }

        private string titleToTweet = string.Empty;

        public string TitleToTweet
        {
            get { return titleToTweet; }
            set { titleToTweet = value; }
        }

        private string linkText = "Tweet";

        public string LinkText
        {
            get { return linkText; }
            set { linkText = value; }
        }

        private string countStyle = "horizontal";

        /// <summary>
        /// valid settings are none, horizontal, vertical
        /// </summary>
        public string CountStyle
        {
            get { return countStyle; }
            set { countStyle = value; }
        }

        private string languageCode = "en";

        public string LanguageCode
        {
            get { return languageCode; }
            set { languageCode = value; }
        }

        private string sourceTwitterAccount = string.Empty;

        public string SourceTwitterAccount
        {
            get { return sourceTwitterAccount; }
            set { sourceTwitterAccount = value; }
        }

        private string relatedTwitterAccount = string.Empty;

        public string RelatedTwitterAccount
        {
            get { return relatedTwitterAccount; }
            set { relatedTwitterAccount = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Visible) { return; }

            if (SiteUtils.IsSecureRequest()) { protocol = "https"; }

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                         "twitterwidgets", "\n<script src=\""
                         + protocol + "://platform.twitter.com/widgets.js" + "\" type=\"text/javascript\"></script>");

           
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            DoRender(writer);

               
        }



        private void DoRender(HtmlTextWriter writer)
        {
            if (!Visible) { return; }
           
            string twitterUrl ="http://twitter.com/share";

            if (CssClass.Length == 0) CssClass = "twitter-share-button";

            writer.WriteBeginTag("a");
            writer.WriteAttribute("class", CssClass);
            writer.WriteAttribute("title", Resource.TweetThisLink);
            writer.WriteAttribute("href", twitterUrl);

            if (urlToTweet.Length > 0)
            {
                writer.WriteAttribute("data-url", urlToTweet);
            }

            if (titleToTweet.Length > 0)
            {
                writer.WriteAttribute("data-text", HttpUtility.HtmlAttributeEncode(titleToTweet));
            }

            switch (countStyle)
            {
                case "vertical":
                    writer.WriteAttribute("data-count", "vertical");
                    break;

                case "horizontal":
                    writer.WriteAttribute("data-count", "horizontal");
                    break;

                case "none":
                default:
                    writer.WriteAttribute("data-count", "none");
                    break;
            }

            if (sourceTwitterAccount.Length > 0)
            {
                writer.WriteAttribute("data-via", sourceTwitterAccount);
            }

            if (relatedTwitterAccount.Length > 0)
            {
                writer.WriteAttribute("data-related", relatedTwitterAccount);
            }

            if (languageCode != "en")
            {
                writer.WriteAttribute("data-lang", languageCode);
            }

            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write(linkText);

            writer.WriteEndTag("a");


        }

    }
}