/// Author:					
/// Created:				2007-09-23
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
using System.Web.UI;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.ELetterUI
{

    public partial class SendLogPage : NonCmsBasePage
    {
        private Letter letter = null;
        private Guid letterGuid = Guid.Empty;
        private LetterInfo letterInfo = null;
        private Guid letterInfoGuid = Guid.Empty;
        //private SiteUser currentUser;
        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 20;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;
        private bool isAdmin = false;
        private bool isSiteEditor = false;

        /// <summary>
        /// True if the current user is in Admins role
        /// </summary>
        protected bool IsAdmin
        {
            get { return isAdmin; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            if ((!isSiteEditor) && (!WebUser.IsNewsletterAdmin))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (letterGuid == Guid.Empty) return;

           
            heading.Text = string.Format(CultureInfo.InvariantCulture,
                Resource.NewsletterSendLogHeadingFormatString,
                letter.Subject);

            lnkThisPage.Text = heading.Text;
            lnkThisPage.ToolTip = heading.Text;

            lnkArchive.Text = string.Format(CultureInfo.InvariantCulture,
                Resource.NewsLetterArchiveLettersHeadingFormatString,
                letterInfo.Title);

            lnkArchive.ToolTip = lnkArchive.Text;

            if (Page.IsPostBack) return;

            BindGrid();


        }

        private void BindGrid()
        {

            List<LetterSendLog> LetterSendLogList
                        = LetterSendLog.GetPage(
                        letterGuid,
                        pageNumber,
                        pageSize,
                        out totalPages);


            if (this.totalPages > 1)
            {
                string pageUrl = SiteUtils.GetNavigationSiteRoot()
                    + "/eletter/SendLog.aspx?letter=" + letterGuid.ToString() + "&amp;pagenumber={0}";

                pgrSendLog.PageURLFormat = pageUrl;
                pgrSendLog.ShowFirstLast = true;
                pgrSendLog.CurrentIndex = pageNumber;
                pgrSendLog.PageSize = pageSize;
                pgrSendLog.PageCount = totalPages;

            }
            else
            {
                pgrSendLog.Visible = false;
            }

            grdSendLog.DataSource = LetterSendLogList;
            grdSendLog.PageIndex = pageNumber;
            grdSendLog.PageSize = pageSize;
            grdSendLog.DataBind();

        }

        void btnPurgeSendLog_Click(object sender, EventArgs e)
        {
            LetterSendLog.DeleteByLetter(letterGuid);
            WebUtils.SetupRedirect(this, Request.RawUrl);
        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuNewsletterAdminLabel);

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";


            lnkLetterAdmin.Text = Resource.NewsLetterAdministrationHeading;
            lnkLetterAdmin.ToolTip = Resource.NewsLetterAdministrationHeading;
            lnkLetterAdmin.NavigateUrl = SiteRoot + "/eletter/Admin.aspx";


            lnkArchive.Text = Resource.NewsLetterArchiveLettersHeading;
            lnkArchive.ToolTip = Resource.NewsLetterArchiveLettersHeading;
            lnkArchive.NavigateUrl = SiteRoot + "/eletter/LetterArchive.aspx?l=" + letterInfoGuid.ToString();


            lnkThisPage.NavigateUrl = SiteRoot + "/eletter/SendLog.aspx?letter=" + letterGuid.ToString();


            grdSendLog.Columns[0].HeaderText = Resource.NewsletterSendLogEmailHeading;
            grdSendLog.Columns[1].HeaderText = Resource.NewsletterSendLogDateSentHeading;
            grdSendLog.Columns[2].HeaderText = Resource.NewsletterSendLogErrorHeading;

            btnPurgeSendLog.Text = Resource.NewsletterPurgeLetterLogButton;
            UIHelper.AddConfirmationDialog(btnPurgeSendLog, Resource.NewsletterPurgeLetterLogWarning);
        }

        private void LoadSettings()
        {
            //spnAdmin.Visible = WebUser.IsAdminOrContentAdmin;
            letterGuid = WebUtils.ParseGuidFromQueryString("letter", Guid.Empty);
            if (letterGuid == Guid.Empty) return;

            letter = new Letter(letterGuid);
            letterInfoGuid = letter.LetterInfoGuid;
            if (letterInfoGuid == Guid.Empty) return;

            letterInfo = new LetterInfo(letterInfoGuid);
            if (letterInfo.SiteGuid != siteSettings.SiteGuid)
            {
                letterInfo = null;
                letterInfoGuid = Guid.Empty;
                letter = null;
                letterGuid = Guid.Empty;
            }

            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            isAdmin = WebUser.IsAdmin;

            lnkAdminMenu.Visible = WebUser.IsAdminOrContentAdmin;
            litLinkSeparator1.Visible = lnkAdminMenu.Visible;

            AddClassToBody("administration");
            AddClassToBody("lettersendlog");

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            btnPurgeSendLog.Click += new EventHandler(btnPurgeSendLog_Click);
            
            SuppressMenuSelection();
            SuppressPageMenu();
            ScriptConfig.IncludeJQTable = true;
        }

        

        #endregion
    }
}
