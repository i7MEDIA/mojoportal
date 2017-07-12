// Author:					
// Created:					2009-10-22
// Last Modified:			2010-06-15
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
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;



namespace mojoPortal.Web.ELetterUI
{

    public partial class ArchivePage : NonCmsBasePage
    {
        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 15;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;
        private LetterInfo letterInfo = null;
        private Guid letterInfoGuid = Guid.Empty;
        private bool canView = false;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();

            if (!canView)
            {
                if (!Request.IsAuthenticated)
                {
                    SiteUtils.RedirectToLoginPage(this);
                    return;
                }

                SiteUtils.RedirectToAccessDeniedPage(this);
                return;

            }

            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (letterInfo.AllowArchiveView)
            {
                litHeading.Text = string.Format(CultureInfo.InvariantCulture, Resource.NewsleterArchiveHeadFormat, letterInfo.Title);

                BindGrid();
            }
            else
            {
                pnlGrid.Visible = false;
                lblMessage.Text = Resource.NewsletterArchivesNotAllowed;

            }

        }

        private void BindGrid()
        {
            List<Letter> LetterList
                        = Letter.GetPage(
                        letterInfoGuid,
                        pageNumber,
                        pageSize,
                        out totalPages);


            string pageUrl = SiteRoot + "/eletter/Archive.aspx?l=" + letterInfoGuid.ToString()
                + "&amp;pagenumber={0}";

            pgrLetter.PageURLFormat = pageUrl;
            pgrLetter.ShowFirstLast = true;
            pgrLetter.CurrentIndex = pageNumber;
            pgrLetter.PageSize = pageSize;
            pgrLetter.PageCount = totalPages;
            pgrLetter.Visible = (totalPages > 1);

            grdLetter.PageIndex = pageNumber;
            grdLetter.PageSize = pageSize;
            grdLetter.DataSource = LetterList;
            grdLetter.DataBind();

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.NewslettersLink);
            lnkNewsletters.Text = Resource.NewslettersLink;
            grdLetter.Columns[0].HeaderText = Resource.NewsletterArchiveSubjectHeader;
            grdLetter.Columns[1].HeaderText = Resource.NewsletterArchiveSentHeader;
            
        }

        private void LoadSettings()
        {
            letterInfoGuid = WebUtils.ParseGuidFromQueryString("l", letterInfoGuid);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            pageSize = WebConfigSettings.NewsletterArchivePageSize;
            ScriptConfig.IncludeColorBox = true;

            letterInfo = new LetterInfo(letterInfoGuid);
            if ((letterInfo.LetterInfoGuid == Guid.Empty) || (letterInfo.SiteGuid != siteSettings.SiteGuid)) { letterInfo = null; }

            if (letterInfo != null)
            {
                canView = WebUser.IsInRoles(letterInfo.AvailableToRoles);
            }

            lnkNewsletters.NavigateUrl = SiteRoot + "/eletter/";

            AddClassToBody("administration");
            AddClassToBody("eletterarchive");
        }

        

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            ScriptConfig.IncludeJQTable = true;

        }

        #endregion
    }
}
