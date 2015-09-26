//	Author:				Joe Audette
//	Created:			2012-06-07
//	Last Modified:		2012-06-08
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
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using Resources;

namespace mojoPortal.Web.BlogUI
{
    public partial class RelatedPosts : UserControl
    {
        private bool useFriendlyUrls = true;

        public bool UseFriendlyUrls
        {
            get { return useFriendlyUrls; }
            set { useFriendlyUrls = value; }
        }

        private string siteRoot = string.Empty;

        public string SiteRoot
        {
            get { return siteRoot; }
            set { siteRoot = value; }
        }

        private int pageId = -1;

        public int PageId
        {
            get { return pageId; }
            set { pageId = value; }
        }

        private int moduleId = -1;
        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        private int itemId = -1;
        public int ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        private int maxItems = 5;
        public int MaxItems
        {
            get { return maxItems; }
            set { maxItems = value; }
        }

        private string headingElement = "h3";

        public string HeadingElement
        {
            get { return headingElement; }
            set { headingElement = value; }
        }

        private string overrideHeadingText = string.Empty;

        public string OverrideHeadingText
        {
            get { return overrideHeadingText; }
            set { overrideHeadingText = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
           // LoadSettings();

            if (itemId == -1)
            {
                Visible = false;
                return;
            }

            BindList();
        }

        private void BindList()
        {
            litHeadingOpenTag.Text = "<" + headingElement  + ">";
            litHeadingCloseTag.Text = "</" + headingElement + ">";
            if (overrideHeadingText.Length > 0)
            {
                litHeading.Text = overrideHeadingText;
            }
            else
            {
                litHeading.Text = BlogResources.RelatedPosts;
            }

            DataTable dt = Blog.GetRelatedPosts(itemId, maxItems);
            rptRelatedPosts.DataSource = dt;
            rptRelatedPosts.DataBind();
            if (dt.Rows.Count == 0) { Visible = false; }

        }


        protected string FormatBlogUrl(string itemUrl, int itemId)
        {
            if (useFriendlyUrls && (itemUrl.Length > 0))
                return siteRoot + itemUrl.Replace("~", string.Empty) ;

            return siteRoot + "/Blog/ViewPost.aspx?pageid=" + pageId.ToInvariantString()
                + "&ItemID=" + itemId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString();

        }

        //protected virtual void LoadSettings()
        //{

        //}
    }
}