// Author:				        Joe Audette
// Created:			            2009-05-02
//	Last Modified:              2012-06-08
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Web.UI;
using mojoPortal.Business;
using Resources;

namespace mojoPortal.Web.BlogUI
{
    public partial class BlogArchiveList : UserControl
    {
        private int pageId = -1;
        private int moduleId = -1;
        private string siteRoot = string.Empty;


        public int PageId
        {
            get { return pageId; }
            set { pageId = value; }
        }

        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        public string SiteRoot
        {
            get { return siteRoot; }
            set { siteRoot = value; }
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
            
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (this.Visible)
            {
                litHeadingOpenTag.Text = "<" + headingElement + ">";
                litHeadingCloseTag.Text = "</" + headingElement + ">";
                if (overrideHeadingText.Length > 0)
                {
                    litHeading.Text = overrideHeadingText;
                }
                else
                {
                    litHeading.Text = BlogResources.BlogArchivesLabel;
                }

                BindList();
            }

            
            base.OnPreRender(e);

        }

        private void BindList()
        {
            if (pageId == -1) { return; }
            if (moduleId == -1) { return; }


            using (IDataReader reader = Blog.GetBlogMonthArchive(ModuleId))
            {
                dlArchive.DataSource = reader;
                dlArchive.DataBind();
            }
            this.Visible = (dlArchive.Items.Count > 0);


        }

    }
}