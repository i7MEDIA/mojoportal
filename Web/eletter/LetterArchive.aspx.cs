/// Author:					
/// Created:				2007-10-10
/// Last Modified:			2011-03-15
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Globalization;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.ELetterUI
{

    public partial class LetterArchivePage : NonCmsBasePage
    {
        private LetterInfo letterInfo = null;
        private Guid letterInfoGuid = Guid.Empty;
        private SiteUser currentUser;
        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 20;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;
        private bool isSiteEditor = false;

       

        protected void Page_Load(object sender, EventArgs e)
        {
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            if ((!isSiteEditor) && (!WebUser.IsNewsletterAdmin))
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
            if (letterInfo == null) return;

            heading.Text = Server.HtmlEncode(string.Format(CultureInfo.InvariantCulture,
                Resource.NewsLetterArchiveLettersHeadingFormatString,
                letterInfo.Title));

            Title = heading.Text;
            lnkThisPage.Text = heading.Text;

            PopulateList();

        }

        private void PopulateList()
        {
            if (letterInfo == null) return;

            List<Letter> letters = Letter.GetPage(
                letterInfo.LetterInfoGuid,
                pageNumber,
                pageSize,
                out totalPages);

            if (this.totalPages > 1)
            {
                string pageUrl = SiteRoot
                    + "/eletter/LetterArchive.aspx?l=" 
                    + letterInfoGuid.ToString() 
                    + "&amp;pagenumber={0}";

                pgrLetter.PageURLFormat = pageUrl;
                pgrLetter.ShowFirstLast = true;
                pgrLetter.CurrentIndex = pageNumber;
                pgrLetter.PageSize = pageSize;
                pgrLetter.PageCount = totalPages;

            }
            else
            {
                pgrLetter.Visible = false;
            }

            grdLetter.DataSource = letters;
            grdLetter.PageIndex = pageNumber;
            grdLetter.PageSize = pageSize;
            grdLetter.DataBind();

        }

        void btnPurgeSendLogs_Click(object sender, EventArgs e)
        {
            LetterSendLog.DeleteByLetterInfo(letterInfoGuid);

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.NewsLetterArchiveLettersHeading);

            heading.Text = Resource.NewsLetterArchiveLettersHeading;

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";


            lnkLetterAdmin.Text = Resource.NewsLetterAdministrationHeading;
            lnkLetterAdmin.ToolTip = Resource.NewsLetterAdministrationHeading;
            lnkLetterAdmin.NavigateUrl = SiteRoot + "/eletter/Admin.aspx";

            lnkThisPage.Text = Resource.NewsLetterArchiveLettersHeading;
            lnkThisPage.ToolTip = Resource.NewsLetterArchiveLettersHeading;
            lnkThisPage.NavigateUrl = SiteRoot + "/eletter/LetterArchive.aspx?l=" + letterInfoGuid.ToString();

            grdLetter.Columns[0].HeaderText = Resource.NewsletterArchiveGridSubjectHeader;
            grdLetter.Columns[1].HeaderText = Resource.NewsletterArchiveGridSendClickedHeader;
            grdLetter.Columns[2].HeaderText = Resource.NewsletterArchiveGridSendCompleteHeader;
            grdLetter.Columns[3].HeaderText = Resource.NewsletterArchiveGridSendCountHeader;

            btnPurgeSendLogs.Text = Resource.NewsletterPurgeSendLogButton;
            UIHelper.AddConfirmationDialog(btnPurgeSendLogs, Resource.NewsletterPurgeSendLogsWarning);

        }

        private void LoadSettings()
        {
            currentUser = SiteUtils.GetCurrentSiteUser();
            letterInfoGuid = WebUtils.ParseGuidFromQueryString("l", Guid.Empty);
            //spnAdmin.Visible = WebUser.IsAdminOrContentAdmin;
            ScriptConfig.IncludeColorBox = true;

            if (letterInfoGuid == Guid.Empty) return;

            letterInfo = new LetterInfo(letterInfoGuid);
            if (letterInfo.SiteGuid != siteSettings.SiteGuid)
            {
                letterInfo = null;
                letterInfoGuid = Guid.Empty;

            }

            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            lnkAdminMenu.Visible = WebUser.IsAdminOrContentAdmin;
            litLinkSeparator1.Visible = lnkAdminMenu.Visible;

            AddClassToBody("administration");
            AddClassToBody("eletterarchive");

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            btnPurgeSendLogs.Click += new EventHandler(btnPurgeSendLogs_Click);
            SuppressMenuSelection();
            SuppressPageMenu();

            ScriptConfig.IncludeJQTable = true;
        }

        

        #endregion

    }
}
