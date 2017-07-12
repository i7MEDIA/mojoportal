// Author:					
// Created:				    2009-04-09
// Last Modified:		    2017-03-15
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
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using Resources;
using Helpers;

namespace mojoPortal.Web.BlogUI
{
    public partial class BlogCompare : mojoDialogBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private int itemId = -1;
        private Guid historyGuid = Guid.Empty;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;
        protected string currentFloat = "left";
        protected string historyFloat = "right";
        //private Module module = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            LoadParams();
            if (SiteUtils.SslIsAvailable() && (siteSettings.UseSslOnAllPages || CurrentPage.RequireSsl))
            {
                SiteUtils.ForceSsl();
            }
            else
            {
                SiteUtils.ClearSsl();
            }

            if (!UserCanEditModule(moduleId, Blog.FeatureGuid))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (moduleId == -1) { return; }
            if (itemId == -1) { return; }
            //if (module == null) { return; }
            if (historyGuid == Guid.Empty) { return; }

            Blog blog = new Blog(itemId);
            if (blog.ModuleId != moduleId) 
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            ContentHistory history = new ContentHistory(historyGuid);
            if (history.ContentGuid != blog.BlogGuid) { return; }

            litCurrentHeading.Text = string.Format(BlogResources.CurrentVersionHeadingFormat,
                DateTimeHelper.Format(blog.LastModUtc, timeZone, "g", timeOffset));

            if (BlogConfiguration.UseHtmlDiff)
            {
                HtmlDiff diffHelper = new HtmlDiff(history.ContentText, blog.Description);
                litCurrentVersion.Text = diffHelper.Build();
            }
            else
            {
                litCurrentVersion.Text = blog.Description;
            }

            litHistoryHead.Text = string.Format(BlogResources.VersionAsOfHeadingFormat,
                DateTimeHelper.Format(history.CreatedUtc, timeZone, "g", timeOffset));

            litHistoryVersion.Text = history.ContentText;

            string onClick = "top.window.LoadHistoryInEditor('" + historyGuid.ToString() + "');  return false;";
            btnRestore.Attributes.Add("onclick", onClick);

        }

        void btnRestore_Click(object sender, EventArgs e)
        {
            // this should only fire if javascript is disabled because we put a client side on click
            string redirectUrl = SiteUtils.GetNavigationSiteRoot() + "/Blog/EditPost.aspx?mid=" + moduleId.ToInvariantString()
                + "&ItemID=" + itemId.ToInvariantString()
                + "&pageid=" + pageId.ToInvariantString() + "&r=" + historyGuid.ToString();

            Response.Redirect(redirectUrl);
        }




        private void PopulateLabels()
        {
            btnRestore.Text = BlogResources.RestoreToEditorButton;
        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
            {
                currentFloat = "right";
                historyFloat = "left";

            }
           

        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            itemId = WebUtils.ParseInt32FromQueryString("ItemID", itemId);
            historyGuid = WebUtils.ParseGuidFromQueryString("h", historyGuid);

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            btnRestore.Click += new EventHandler(btnRestore_Click);
        }
    }
}
