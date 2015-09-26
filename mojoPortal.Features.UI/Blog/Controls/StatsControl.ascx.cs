// Author:				        Joe Audette
// Created:			            2009-05-04
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
using System.Collections;
using System.Globalization;
using System.Data;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.BlogUI
{
    public partial class StatsControl : UserControl
    {
        private int pageId = -1;
        private int moduleId = -1;
        private Guid moduleGuid = Guid.Empty;
        private int countOfDrafts = 0;
        private bool showCommentCount = true;

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

        public Guid ModuleGuid
        {
            get { return moduleGuid; }
            set { moduleGuid = value; }
        }

        public int CountOfDrafts
        {
            get { return countOfDrafts; }
            set { countOfDrafts = value; }
        }

        public bool ShowCommentCount
        {
            get { return showCommentCount; }
            set { showCommentCount = value; }
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
                PopulateControls();
            }


            base.OnPreRender(e);

        }

        private void PopulateControls()
        {
            if (pageId == -1) { return; }
            if (moduleId == -1) { return; }

            litHeadingOpenTag.Text = "<" + headingElement + ">";
            litHeadingCloseTag.Text = "</" + headingElement + ">";
            if (overrideHeadingText.Length > 0)
            {
                litHeading.Text = overrideHeadingText;
            }
            else
            {
                litHeading.Text = BlogResources.BlogStatisticsLabel;
            }

            int commentCount = 0;

            using (IDataReader reader = Blog.GetBlogStats(ModuleId))
            {
                if (reader.Read())
                {
                    int entryCount = Convert.ToInt32(reader["EntryCount"]);
                    commentCount = Convert.ToInt32(reader["CommentCount"]);

                    litEntryCount.Text = ResourceHelper.FormatCategoryLinkText(BlogResources.BlogEntryCountLabel, (entryCount - countOfDrafts));
                    

                }
                else
                {
                    litEntryCount.Text = ResourceHelper.FormatCategoryLinkText(BlogResources.BlogEntryCountLabel, 0);
                    litCommentCount.Text = ResourceHelper.FormatCategoryLinkText(BlogResources.BlogCommentCountLabel, 0);
                }

            }

            if (!BlogConfiguration.UseLegacyCommentSystem)
            {
                CommentRepository commentRepository = new CommentRepository();
                commentCount = commentRepository.GetCountByModule(ModuleGuid, Comment.ModerationApproved);
            }

            litCommentCount.Text = ResourceHelper.FormatCategoryLinkText(BlogResources.BlogCommentCountLabel, commentCount);

            liComments.Visible = showCommentCount;

        }
    }
}