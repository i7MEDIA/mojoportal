//	Author:                 
//  Created:			    2012-07-01
//	Last Modified:		    2012-07-01
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.


using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// http://developers.janrain.com/documentation/widgets/social-sharing-widget/users-guide/get-the-code/
    /// </summary>
    public class JanrainSharingElement : WebControl
    {
        private string element = "a";

        public string Element
        {
            get { return element; }
            set { element = value; }
        }

        private string comment = string.Empty;

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        private string imageUrl = string.Empty;

        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }

        private string urlToShare = string.Empty;

        public string UrlToShare
        {
            get { return urlToShare; }
            set { urlToShare = value; }
        }

        private string linkText = string.Empty;

        public string LinkText
        {
            get { return linkText; }
            set { linkText = value; }
        }

        private string label = string.Empty;

        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        private string summary = string.Empty;

        public string Summary
        {
            get { return summary; }
            set { summary = value; }
        }

        private string provider = string.Empty;

        public string Provider
        {
            get { return provider; }
            set { provider = value; }
        }

        private string innerHtml = string.Empty;

        public string InnerHtml
        {
            get { return innerHtml; }
            set { innerHtml = value; }
        }

        private bool dontRender = false;

        public bool DontRender
        {
            get { return dontRender; }
            set { dontRender = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            EnableViewState = false;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //base.Render(writer);

            if (dontRender) { return; }

            writer.WriteBeginTag(element);
            writer.WriteAttribute("class", CssClass + " janrain_engage_share");

            if (comment.Length > 0)
            {
                writer.WriteAttribute("data-comment", comment.HtmlEscapeQuotes());
            }

            if (imageUrl.Length > 0)
            {
                writer.WriteAttribute("data-image", imageUrl.HtmlEscapeQuotes());
            }

            if (label.Length > 0)
            {
                writer.WriteAttribute("data-label", label.HtmlEscapeQuotes());
            }

            if (urlToShare.Length > 0)
            {
                writer.WriteAttribute("data-link", urlToShare);
            }

            if (linkText.Length > 0)
            {
                writer.WriteAttribute("data-linkText", linkText.HtmlEscapeQuotes());
            }

            if (provider.Length > 0)
            {
                writer.WriteAttribute("data-provider", provider.HtmlEscapeQuotes());
            }

            if (summary.Length > 0)
            {
                writer.WriteAttribute("data-summary", summary.HtmlEscapeQuotes().RemoveLineBreaks());
            }


            writer.Write(HtmlTextWriter.TagRightChar);

            if (innerHtml.Length > 0)
            {
                writer.Write(innerHtml);
            }

            writer.WriteEndTag(element);
        }
    }
}