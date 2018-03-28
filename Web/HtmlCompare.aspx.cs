// Author:					
// Created:				    2009-04-08
// Last Modified:		    2012-07-01
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

namespace mojoPortal.Web.ContentUI
{
    public partial class HtmlCompare : mojoDialogBasePage
    {

        private int pageId = -1;
        private int moduleId = -1;
        private Guid historyGuid = Guid.Empty;
        private Guid workflowGuid = Guid.Empty;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;
        protected string currentFloat = "left";
        protected string historyFloat = "right";
        private bool userCanEdit = false;
        private bool userCanEditAsDraft = false;
        private bool highlightDiff = true;
        private HtmlRepository repository = new HtmlRepository();

        protected void Page_Load(object sender, EventArgs e)
        {

            LoadParams();
            userCanEdit = UserCanEditModule(moduleId);
            userCanEditAsDraft = UserCanOnlyEditModuleAsDraft(moduleId);

            if ((!userCanEdit) && (!userCanEditAsDraft))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (moduleId == -1) { return; }
            //if (module == null) { return; }
            if (historyGuid != Guid.Empty) 
            {
                ShowVsHistory();
                return; 
            }

            if (workflowGuid != Guid.Empty)
            {
                ShowVsDraft();
            }
            

        }

        private void ShowVsHistory()
        {
            HtmlContent html = repository.Fetch(moduleId);
            ContentHistory history = new ContentHistory(historyGuid);
            if (history.ContentGuid != html.ModuleGuid) { return; }

            litCurrentHeading.Text = string.Format(Resource.CurrentVersionHeadingFormat,
                DateTimeHelper.Format(html.LastModUtc, timeZone, "g", timeOffset));

            if ((HtmlConfiguration.UseHtmlDiff)&&(highlightDiff))
            {
                HtmlDiff diffHelper = new HtmlDiff(history.ContentText, html.Body);
                litCurrentVersion.Text = diffHelper.Build();
            }
            else
            {
                litCurrentVersion.Text = html.Body;
            }

            litHistoryHead.Text = string.Format(Resource.VersionAsOfHeadingFormat,
                DateTimeHelper.Format(history.CreatedUtc, timeZone, "g", timeOffset));



            litHistoryVersion.Text = history.ContentText;

            string onClick = "top.window.LoadHistoryInEditor('" + historyGuid.ToString() + "');  return false;";
            btnRestore.Attributes.Add("onclick", onClick);
        }

        private void ShowVsDraft()
        {
            HtmlContent html = repository.Fetch(moduleId);

            if (html == null) { return; }

            ContentWorkflow draftContent = ContentWorkflow.GetWorkInProgress(html.ModuleGuid);

            if (draftContent.Guid != workflowGuid) { return; }

            if ((HtmlConfiguration.UseHtmlDiff)&&(highlightDiff))
            {
                HtmlDiff diffHelper = new HtmlDiff(html.Body, draftContent.ContentText);
                litCurrentVersion.Text = diffHelper.Build();
            }
            else
            {
                litCurrentVersion.Text = draftContent.ContentText;
            }

            litCurrentHeading.Text = string.Format(Resource.CurrentDraftHeadingFormat,
                DateTimeHelper.Format(draftContent.RecentActionOn, timeZone, "g", timeOffset));

            litHistoryHead.Text = string.Format(Resource.CurrentVersionHeadingFormat,
                DateTimeHelper.Format(html.LastModUtc, timeZone, "g", timeOffset));

            litHistoryVersion.Text = html.Body;

            //string onClick = "top.window.LoadHistoryInEditor('" + historyGuid.ToString() + "');  return false;";
            //btnRestore.Attributes.Add("onclick", onClick);
            btnRestore.Visible = false;
        }

        void btnRestore_Click(object sender, EventArgs e)
        {
            // this should only fire if javascript is disabled because we put a client side on click
            string redirectUrl = SiteUtils.GetNavigationSiteRoot() + "/HtmlEdit.aspx?mid=" + moduleId.ToInvariantString()
                + "&pageid=" + pageId.ToInvariantString() + "&r=" + historyGuid.ToString();

            Response.Redirect(redirectUrl);
        }
        



        private void PopulateLabels()
        {
            btnRestore.Text = Resource.RestoreToEditorButton;
        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            //if (moduleId > -1)
            //{
            //    module = new Module(moduleId, pageId);
            //}
            if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
            {
                currentFloat = "right";
                historyFloat = "left";

            }

            if (HtmlConfiguration.UseHtmlDiff)
            {
                if (highlightDiff)
                {
                    lnkToggleHighlight.Text = Resource.DontHighlightDifferences;
                    lnkToggleHighlight.NavigateUrl = SiteRoot + "/HtmlCompare.aspx?pageid="
                        + pageId.ToInvariantString()
                        + "&mid=" + moduleId.ToInvariantString()
                        + "&h=" + historyGuid.ToString()
                        + "&d=" + workflowGuid.ToString()
                        + "&hd=false"
                        ;
                }
                else
                {
                    lnkToggleHighlight.Text = Resource.HighlightDifferences;
                    lnkToggleHighlight.NavigateUrl = SiteRoot + "/HtmlCompare.aspx?pageid="
                        + pageId.ToInvariantString()
                        + "&mid=" + moduleId.ToInvariantString()
                        + "&h=" + historyGuid.ToString()
                        + "&d=" + workflowGuid.ToString()
                        + "&hd=true"
                        ;
                }
            }
            else
            {
                lnkToggleHighlight.Visible = false;
            }
        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            historyGuid = WebUtils.ParseGuidFromQueryString("h", historyGuid);
            workflowGuid = WebUtils.ParseGuidFromQueryString("d", workflowGuid);
            highlightDiff = WebUtils.ParseBoolFromQueryString("hd", highlightDiff);

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            btnRestore.Click += new EventHandler(btnRestore_Click);
        }

        
    }
}
