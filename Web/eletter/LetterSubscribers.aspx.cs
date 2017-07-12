/// Author:					
/// Created:				2007-09-23
/// Last Modified:			2012-05-18
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.
/// 2012-02-24 added import logic from Steve Railsback but hidden by default because it needs work to be more supportable for
/// users

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.ELetterUI
{
    public partial class LetterSubscribersPage : NonCmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LetterSubscribersPage));
        private LetterInfo letterInfo = null;
        private Guid letterInfoGuid = Guid.Empty;
        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 20;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;
        private bool isAdmin = false;
        private bool isSiteEditor = false;
        private SubscriberRepository subscriptions = new SubscriberRepository();
        protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;

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
            if (letterInfoGuid == Guid.Empty) return;

            letterInfo = new LetterInfo(letterInfoGuid);

            lnkThisPage.Text = string.Format(CultureInfo.InvariantCulture,
                Resource.NewsletterSubscriberListHeadingFormatString,
                letterInfo.Title);

            lnkThisPage.ToolTip = lnkThisPage.Text;

            heading.Text = lnkThisPage.Text;

            Title = heading.Text;

            int countOfUsersThatCouldOptIn = SubscriberRepository.CountUsersNotSubscribedByLetter(
                siteSettings.SiteGuid, 
                letterInfoGuid, 
                WebConfigSettings.NewsletterExcludeAllPreviousOptOutsWhenOptingInUsers);

            if (countOfUsersThatCouldOptIn > 0)
            {
                lnkOptIn.Visible = true;
                lnkOptIn.Text = string.Format(CultureInfo.InvariantCulture, Resource.NewsLetterOptInMembersFormat, countOfUsersThatCouldOptIn);
            }
            
            

            if (Page.IsPostBack) return;

            BindGrid();

        }

        private void BindGrid()
        {

            using (IDataReader reader = subscriptions.GetPage(letterInfoGuid, pageNumber, pageSize, out totalPages))
            {
                grdSubscribers.DataSource = reader;
                grdSubscribers.PageIndex = pageNumber;
                grdSubscribers.PageSize = pageSize;
                grdSubscribers.DataBind();

                string pageUrl = SiteRoot + "/eletter/LetterSubscribers.aspx?l="
                    + letterInfoGuid.ToString() + "&amp;pagenumber={0}";

                pgrLetterSubscriber.PageURLFormat = pageUrl;
                pgrLetterSubscriber.ShowFirstLast = true;
                pgrLetterSubscriber.CurrentIndex = pageNumber;
                pgrLetterSubscriber.PageSize = pageSize;
                pgrLetterSubscriber.PageCount = totalPages;
                pgrLetterSubscriber.Visible = (totalPages > 1);

            }


            
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            using (IDataReader reader = subscriptions.Search(letterInfoGuid, txtSearchInput.Text))
            {
                grdSubscribers.DataSource = reader;
                grdSubscribers.DataBind();
            }
            
            pgrLetterSubscriber.Visible = false;

           
        }

        void btnDeleteUnVerified_Click(object sender, EventArgs e)
        {
            int daysOld = 90;
            int.TryParse(txtDaysOld.Text, out daysOld);
            DateTime cutOffDate = DateTime.UtcNow;
            if (daysOld > 0) { cutOffDate = DateTime.UtcNow.AddDays(-daysOld); }

            subscriptions.DeleteUnverified(letterInfoGuid, cutOffDate);

            WebUtils.SetupRedirect(this, SiteRoot + "/eletter/LetterSubscribers.aspx?l=" + letterInfoGuid.ToString());

        }

        void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = subscriptions.GetVerifiedSubscribersForExport(letterInfoGuid);

            string fileName = "subscriber-data-export-" + DateTimeHelper.GetDateTimeStringForFileName() + ".csv";

            ExportHelper.ExportDataTableToCsv(HttpContext.Current, dt, fileName);

        }

        void grdSubscribers_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            string strG = e.CommandArgument.ToString();
            switch (e.CommandName)
            {
                case "DeleteSubscriber":

                    if (strG.Length == 36)
                    {
                        Guid subscriptionGuid = new Guid(strG);
                        LetterSubscriber s = subscriptions.Fetch(subscriptionGuid);
                        if (s != null) { subscriptions.Delete(s); }

                        LetterInfo.UpdateSubscriberCount(s.LetterInfoGuid);

                        WebUtils.SetupRedirect(this, Request.RawUrl);
                    }

                    break;

                case "SendVerification":

                    if (strG.Length == 36)
                    {
                        Guid subscriptionGuid = new Guid(strG);
                        LetterSubscriber s = subscriptions.Fetch(subscriptionGuid);
                        LetterInfo letterInfo = new LetterInfo(letterInfoGuid);

                        NewsletterHelper.SendSubscriberVerificationEmail(
                            SiteRoot,
                            s.EmailAddress,
                            s.SubscribeGuid,
                            letterInfo,
                            siteSettings);
                    }

                    WebUtils.SetupRedirect(this, Request.RawUrl);

                    break;
            }

        }

        void grdSubscribers_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            Control c = e.Row.FindControl("btnDelete");
            if (c != null)
            {
                ImageButton btnDelete = c as ImageButton;
                UIHelper.AddConfirmationDialog(btnDelete, Resource.NewsletterSubscriberDeleteWarning);

            }
        }


        protected bool ShowUserLink(string userGuid)
        {
            if (!isAdmin) { return false; }

            if (userGuid != "00000000-0000-0000-0000-000000000000") { return true; }


            return false;

        }

        ///// <summary>
        ///// Steve Railsback btnBatchProcessSubscribers_Click
        ///// Batch update subscriber list
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void btnBatchProcessSubscribers_Click(object sender, EventArgs e)
        //{
        //    if (letterInfo == null) { letterInfo = new LetterInfo(letterInfoGuid); }

        //    List<string[]> data = new List<string[]>();
        //    if (letterInfo.LetterInfoGuid != null)
        //    {
        //        // is there a file to process?
        //        if (fileBatchProcessSubscribers.HasFile)
        //        {
        //            try
        //            {
        //                using (StreamReader streamReader = new StreamReader(fileBatchProcessSubscribers.PostedFile.InputStream))
        //                {
        //                    string line;
        //                    string[] row;
        //                    while (((line = streamReader.ReadLine()) != string.Empty)&&(line != null))
        //                    {
        //                        row = line.Split(',');
        //                        data.Add(row);
        //                    }
        //                }

        //                // ints to squirrel the total adds, removes and fails
        //                int subscribersAdded = 0;
        //                int subscribersRemoved = 0;
        //                int subscribersFailed = 0;

        //                // loop through data collection and do it!
        //                for (int i = 0; i < data.Count; i++)
        //                {
        //                    string result = DoSubscription(data[i], letterInfo);

        //                    if (result == "added")
        //                    {
        //                        subscribersAdded += 1;
        //                    }

        //                    if (result == "deleted")
        //                    {
        //                        subscribersRemoved += 1;
        //                    }

        //                    if (result == "failed")
        //                    {
        //                        subscribersFailed += 1;
        //                    }

        //                }

        //                lblBatchProcessSubscribers.Text = String.Format("Done! {0} Added, {1} Removed, {2} Failed (check the log)", subscribersAdded, subscribersRemoved, subscribersFailed);
        //                LetterInfo.UpdateSubscriberCount(letterInfo.LetterInfoGuid);
        //                BindGrid();

        //            }
        //            catch (Exception ex)
        //            {
        //                lblBatchProcessSubscribers.Text = "Error: " + ex.Message.ToString();
        //            }

        //        }
        //        else
        //        {
        //            lblBatchProcessSubscribers.Text = "You have not specified a file.";
        //        }
        //    }
        //    else
        //    {
        //        lblBatchProcessSubscribers.Text = "Newsletter not specified.";
        //    }


        //}

        ///// <summary>
        ///// Steve Railsback DoSubscription adds or deletes a subscriber
        ///// </summary>
        ///// <param name="data"></param>
        ///// <param name="letterInfo"></param>
        //private string DoSubscription(string[] data, LetterInfo letterInfo)
        //{
        //    string action = data[0];
        //    string email = data[1];

        //    // validate email addresses
        //    if (!RegexHelper.IsValidEmailAddressSyntax(email))
        //    {
        //        log.Info(letterInfo.Title + ": Error: email is not valid.");
        //        return "failed";
        //    }

        //    // blacklisted emails
        //    // to do build black list 
        //    if (email == "email@gmail.com")
        //    {
        //        log.Info(letterInfo.Title + ": Error: " + email + " is banned.");
        //        return "failed";
        //    }

        //    // get subscriber using email address
        //    LetterSubscriber subscriber = subscriptions.Fetch(siteSettings.SiteGuid, letterInfoGuid, email);

        //    if (action == "+")
        //    {
        //        return AddSubscriber(subscriber, email);
        //    }
        //    else
        //    {
        //        return DeleteSubscriber(subscriber);
        //    }
        //}

        ///// <summary>
        ///// Steve Railsback Adds the subscriber.
        ///// </summary>
        ///// <param name="subscriber">The subscriber.</param>
        ///// <param name="email">The email.</param>
        ///// <returns></returns>
        //private string AddSubscriber(LetterSubscriber subscriber, string email)
        //{
        //    // get site user
        //    SiteUser siteUser = SiteUser.GetByEmail(siteSettings, email);

        //    // Only concerned about new subscribers
        //    if (subscriber == null)
        //    {
        //        try
        //        {
        //            subscriptions.Save(
        //                new LetterSubscriber
        //                {
        //                    SiteGuid = siteSettings.SiteGuid,
        //                    EmailAddress = email,
        //                    LetterInfoGuid = letterInfoGuid,
        //                    UseHtml = true,
        //                    UserGuid = siteUser != null ? siteUser.UserGuid : new Guid("00000000-0000-0000-0000-000000000000"),
        //                    IsVerified = true,
        //                    BeginUtc = DateTime.UtcNow,
        //                    IpAddress = SiteUtils.GetIP4Address()
        //                }
        //            );

        //            return "added";
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Info(letterInfo.Title + ": Error: could not add " + email + ", " + ex.Message);
        //            return "failed";
        //        }
        //    }
        //    return "";
        //}

        ///// <summary>
        ///// Steve Railsback Deletes the subscriber.
        ///// </summary>
        ///// <param name="subscriber">The subscriber.</param>
        ///// <returns></returns>
        //private string DeleteSubscriber(LetterSubscriber subscriber)
        //{
        //    // only concerned about existing subscribers
        //    if (subscriber != null)
        //    {
        //        try
        //        {
        //            subscriptions.Delete(subscriber);
        //            return "deleted";
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Info(letterInfo.Title + ": Error: " + ex.Message);
        //            return "failed";
        //        }
        //    }
        //    return "";

        //}

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuNewsletterAdminLabel);

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";


            lnkLetterAdmin.Text = Resource.NewsLetterAdministrationHeading;
            lnkLetterAdmin.ToolTip = Resource.NewsLetterAdministrationHeading;
            lnkLetterAdmin.NavigateUrl = SiteRoot + "/eletter/Admin.aspx";
            lnkThisPage.NavigateUrl = SiteRoot + "/eletter/LetterSubscribers.aspx?l=" + letterInfoGuid.ToString();

            btnSearch.Text = Resource.NewsletterSubscriberSearchButton;
            btnDeleteUnVerified.Text = Resource.NewsletterDeleteUnverifiedButton;
            UIHelper.AddConfirmationDialog(btnDeleteUnVerified, Resource.NewsletterDeletedUnverifiedWarning);

            btnExport.Text = Resource.ExportDataAsCSV;

            grdSubscribers.Columns[0].HeaderText = Resource.NewsletterSubscriberListNameHeading;
            grdSubscribers.Columns[1].HeaderText = Resource.NewsletterSubscriberListEmailHeading;
            grdSubscribers.Columns[2].HeaderText = Resource.NewsletterSubscriberListHtmlFormatHeading;
            grdSubscribers.Columns[3].HeaderText = Resource.NewsletterSubscriberListBeginDateHeading;
            grdSubscribers.Columns[4].HeaderText = Resource.NewsletterSubscriberListIsVerifiedHeading;
            // grdSubscribers.Columns[4].HeaderText = Resource.

        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            letterInfoGuid = WebUtils.ParseGuidFromQueryString("l", Guid.Empty);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            isAdmin = WebUser.IsAdmin;
            

            //this can be enabled but it isn't very refined or robust so hiding it by default
            // needs improvement to be more easily supportable before making it visible by default
            //pnlBatchProcessSubscribers.Visible = WebConfigSettings.NewsletterShowImportPanel;
            // 2012-06-24 moved to a standalone .aspx file with inline code that people can download from:
            // https://www.mojoportal.com/usingthenewsletter.aspx

            lnkAdminMenu.Visible = WebUser.IsAdminOrContentAdmin;
            litLinkSeparator1.Visible = lnkAdminMenu.Visible;

            lnkOptIn.NavigateUrl = SiteRoot + "/eletter/OptInUsersDialog.aspx?l=" + letterInfoGuid.ToString();

            AddClassToBody("administration");
            AddClassToBody("lettersubscribers");

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            grdSubscribers.RowCreated += new System.Web.UI.WebControls.GridViewRowEventHandler(grdSubscribers_RowCreated);
            grdSubscribers.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(grdSubscribers_RowCommand);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnDeleteUnVerified.Click += new EventHandler(btnDeleteUnVerified_Click);
            btnExport.Click += new EventHandler(btnExport_Click);

            // load subscribers
            //btnBatchProcessSubscribers.Click += new EventHandler(btnBatchProcessSubscribers_Click);

            SuppressMenuSelection();
            SuppressPageMenu();

            ScriptConfig.IncludeJQTable = true;
        }

        

        

        

        
        #endregion
    }
}
