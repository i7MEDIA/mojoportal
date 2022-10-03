// Author:					
// Created:				    2007-09-23
// Last Modified:			2014-04-22
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
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Editor;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Net;
using Resources;

namespace mojoPortal.Web.ELetterUI
{

    public partial class LetterEditPage : NonCmsBasePage
    {
        private LetterInfo letterInfo = null;
        private Letter letter = null;
        private Guid letterInfoGuid = Guid.Empty;
        private Guid letterGuid = Guid.Empty;
        private SiteUser currentUser = null;
        private bool isSiteEditor = false;
        private string imageSiteRoot = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            if ((!isSiteEditor)&&(!WebUser.IsNewsletterAdmin))
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

            lnkDraftList.Text = string.Format(CultureInfo.InvariantCulture,
                Resource.NewsLetterUnsentLettersHeadingFormatString,
                letterInfo.Title);

            if ((letter!= null)&&(letter.SendClickedUtc > DateTime.MinValue))
            {
                lnkDraftList.Text = string.Format(CultureInfo.InvariantCulture,
                Resource.NewsLetterPreviousLettersHeadingFormatString,
                letterInfo.Title);

                lnkDraftList.ToolTip = Resource.NewsLetterArchiveLettersHeading;
                lnkDraftList.NavigateUrl = SiteRoot + "/eletter/LetterArchive.aspx?l=" + letterInfoGuid.ToString();

            }

            if (Page.IsPostBack) return;

            PopulateTemplateList();

            if (letter == null) return;

            

            if (Page.IsPostBack) return;

            heading.Text += " " + letter.Subject;
            txtSubject.Text = letter.Subject;
            edContent.Text = letter.HtmlBody;
            txtPlainText.Text = letter.TextBody;


        }

        

        private void PopulateTemplateList()
        {
            List<LetterHtmlTemplate> LetterHtmlTemplateList = LetterHtmlTemplate.GetAll(siteSettings.SiteGuid);
            if (LetterHtmlTemplateList.Count == 0)
            {
                mojoSetup.CreateDefaultLetterTemplates(siteSettings.SiteGuid);

                LetterHtmlTemplateList = LetterHtmlTemplate.GetAll(siteSettings.SiteGuid);

            }

            ddTemplates.DataSource = LetterHtmlTemplateList;
            ddTemplates.DataBind();


        }

        void btnSave_Click(object sender, EventArgs e)
        {
            
            Page.Validate("newsletteredit");
            if (!Page.IsValid) return;

            SaveLetter();

            if (letter != null)
            {
                string redirectUrl = SiteRoot + "/eletter/LetterEdit.aspx?l=" + letterInfoGuid.ToString()
                    + "&letter=" + letterGuid.ToString();

                WebUtils.SetupRedirect(this, redirectUrl);

            }


        }

        private void SaveLetter()
        {
            if (letter == null) return;
            if (currentUser == null) return;
            // no edits after sending
            if (letter.SendCompleteUtc > DateTime.MinValue)
            {
                // already sent, can't edit it but can save as new draft
                SaveAsNewDraft(letter);
                return;
            }

            letter.HtmlBody = SiteUtils.ChangeRelativeUrlsToFullyQualifiedUrls(SiteRoot, imageSiteRoot, edContent.Text);
            letter.TextBody = txtPlainText.Text;
            letter.Subject = txtSubject.Text;
            letter.LastModBy = currentUser.UserGuid;
            if (letter.LetterGuid == Guid.Empty)
            {
                // new letter
                letter.LetterInfoGuid = letterInfoGuid;
                letter.CreatedBy = currentUser.UserGuid;
            }

            letter.Save();
            letterGuid = letter.LetterGuid;

        }

        private void SaveAsNewDraft(Letter prevEdition)
        {
            letter = new Letter();
           
            letter.HtmlBody = SiteUtils.ChangeRelativeUrlsToFullyQualifiedUrls(SiteRoot, imageSiteRoot, edContent.Text);
            letter.TextBody = txtPlainText.Text;
            letter.Subject = txtSubject.Text;
            letter.LastModBy = currentUser.UserGuid;
            if (letter.LetterGuid == Guid.Empty)
            {
                // new letter
                letter.LetterInfoGuid = letterInfoGuid;
                letter.CreatedBy = currentUser.UserGuid;
            }

            letter.Save();
            letterGuid = letter.LetterGuid;

        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (letter == null) return;
            if (currentUser == null) return;

            Letter.Delete(letterGuid);

            //string redirectUrl = SiteRoot + "/eletter/LetterEdit.aspx?l=" + letterInfoGuid.ToString()
            //        + "&letter=" + letter.LetterGuid.ToString();

            string redirectUrl = SiteRoot + "/eletter/Admin.aspx";

            WebUtils.SetupRedirect(this, redirectUrl);

            

        }

        void btnSendPreview_Click(object sender, EventArgs e)
        {
            string baseUrl = WebUtils.GetHostRoot();
            if (WebConfigSettings.UseFolderBasedMultiTenants)
            {
                // in folder based sites the relative urls in the editor will already have the folder name
                // so we want to use just the raw site root not the navigation root
                baseUrl = WebUtils.GetSiteRoot();
            }

            string content = SiteUtils.ChangeRelativeUrlsToFullyQualifiedUrls(baseUrl, ImageSiteRoot, edContent.Text);
            // TODO: validate email
            Email.Send(
                GetSmtpSettings(),
                siteSettings.DefaultEmailFromAddress,
                siteSettings.DefaultFromEmailAlias,
                string.Empty,
                txtPreviewAddress.Text,
                string.Empty,
                string.Empty,
                txtSubject.Text,
                content,
                true,
                "Normal");

            //WebUtils.SetupRedirect(this, Request.RawUrl);
            
        }

        void btnSaveAsTemplate_Click(object sender, EventArgs e)
        {
            Page.Validate("newtemplate");
            if (!Page.IsValid) { return; }

            SaveLetter();

            LetterHtmlTemplate template = new LetterHtmlTemplate();
            template.SiteGuid = siteSettings.SiteGuid;
            template.Title = txtNewTemplateName.Text;
            template.Html = edContent.Text;
            template.Save();

            string redirectUrl = SiteRoot + "/eletter/LetterEdit.aspx?l=" + letterInfoGuid.ToString()
                     + "&letter=" + letter.LetterGuid.ToString();

            WebUtils.SetupRedirect(this, redirectUrl);
            
        }

        void btnLoadTemplate_Click(object sender, EventArgs e)
        {
            if (ddTemplates.Items.Count == 0) return;

            if (ddTemplates.SelectedValue.Length != 36) return;

            Guid templateGuid = new Guid(ddTemplates.SelectedValue);
            LetterHtmlTemplate template = new LetterHtmlTemplate(templateGuid);
            edContent.Text = template.Html;
            SaveLetter();

            string redirectUrl = SiteRoot + "/eletter/LetterEdit.aspx?l=" + letterInfoGuid.ToString()
                    + "&letter=" + letter.LetterGuid.ToString();

            WebUtils.SetupRedirect(this, redirectUrl);
              
            
        }

        void btnSendToList_Click(object sender, EventArgs e)
        {
            SaveLetter();
            
            if (!LetterIsValidForSending()) return;

            if (letter.SendCompleteUtc > DateTime.MinValue) return;

            // TODO: implement approval process
            letter.ApprovedBy = currentUser.UserGuid;
            letter.IsApproved = true;

            string baseUrl = WebUtils.GetHostRoot();
            if (WebConfigSettings.UseFolderBasedMultiTenants)
            {
                // in folder based sites the relative urls in the editor will already have the folder name
                // so we want to use just the raw site root not the navigation root
                baseUrl = WebUtils.GetSiteRoot();
            }

            string content = SiteUtils.ChangeRelativeUrlsToFullyQualifiedUrls(baseUrl, WebUtils.GetSiteRoot(), letter.HtmlBody);
            letter.HtmlBody = content;

            SaveLetter();

            letter.TrackSendClicked();
            SmtpSettings smtpSettings = GetSmtpSettings();

			LetterSendTask letterSender = new LetterSendTask
			{
				SiteGuid = siteSettings.SiteGuid,
				QueuedBy = currentUser.UserGuid,
				LetterGuid = letter.LetterGuid,
				UnsubscribeLinkText = Resource.NewsletterUnsubscribeLink,
				ViewAsWebPageText = Resource.NewsletterViewAsWebPageLink,
				UnsubscribeUrl = SiteRoot + "/eletter/Unsubscribe.aspx",
				NotificationFromEmail = siteSettings.DefaultEmailFromAddress,
				NotifyOnCompletion = true,
				NotificationToEmail = currentUser.Email,
				User = smtpSettings.User,
				Password = smtpSettings.Password,
				Server = smtpSettings.Server,
				Port = smtpSettings.Port,
				RequiresAuthentication = smtpSettings.RequiresAuthentication,
				UseSsl = smtpSettings.UseSsl,
				PreferredEncoding = smtpSettings.PreferredEncoding,
				TaskUpdateFrequency = 65,
				MaxToSendPerMinute = WebConfigSettings.NewsletterMaxToSendPerMinute
			};

			if (letterInfo.AllowArchiveView)
            {
                letterSender.WebPageUrl = SiteRoot + "/eletter/LetterView.aspx?l=" + letter.LetterInfoGuid.ToString() + "&amp;letter=" + letter.LetterGuid.ToString();
            }
            
            letterSender.QueueTask();

            string redirectUrl = SiteRoot + "/eletter/SendProgress.aspx?l=" + letterInfoGuid.ToString()
                + "&letter=" + letterGuid.ToString()
                + "&t=" + letterSender.TaskGuid.ToString();

            WebTaskManager.StartOrResumeTasks();

            WebUtils.SetupRedirect(this, redirectUrl);



        }

        private SmtpSettings GetSmtpSettings()
        {
            // support alternate smtp settings for newsletter

            if (WebConfigSettings.NewsletterSmtpServer.Length > 0)
            {
                SmtpSettings smtpSettings = new SmtpSettings();
                smtpSettings.Server = WebConfigSettings.NewsletterSmtpServer;
                smtpSettings.Port = WebConfigSettings.NewsletterSmtpServerPort;
                smtpSettings.User = WebConfigSettings.NewsletterSmtpUser;
                smtpSettings.Password = WebConfigSettings.NewsletterSmtpUserPassword;
                smtpSettings.UseSsl = WebConfigSettings.NewsletterSmtpUseSsl;
                smtpSettings.RequiresAuthentication = WebConfigSettings.NewsletterSmtpRequiresAuthentication;
                smtpSettings.PreferredEncoding = WebConfigSettings.NewsletterSmtpPreferredEncoding;

                return smtpSettings;

            }


            return SiteUtils.GetSmtpSettings();
        }

        void btnGeneratePlainText_Click(object sender, EventArgs e)
        {
            if (letter == null) return;
            Page.Validate("newsletteredit");
            if (!Page.IsValid) return;

            txtPlainText.Text = SecurityHelper.RemoveMarkup(UIHelper.ConvertHtmlBreaksToTextBreaks(edContent.Text));
            SaveLetter();

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        private bool LetterIsValidForSending()
        {
            bool result = true;

            if (letter == null)result = false;

            lblErrorMessage.Text = string.Empty;

            if (WebConfigSettings.NewsletterEnforceCanSpam)
            {
                if (!letter.HtmlBody.Contains(Letter.UnsubscribeToken))
                {
                    lblErrorMessage.Text += Resource.NewsletterUnsubscribeTokenNotFoundMessage;
                    result = false;
                }

                if (!letter.TextBody.Contains(Letter.UnsubscribeToken))
                {
                    lblErrorMessage.Text += Resource.NewsletterPlainTextUnsubscribeTokenNotFoundMessage;
                    result = false;
                }
            }

            if (letter.TextBody.Length == 0)
            {
                lblErrorMessage.Text += Resource.NewsletterTextVersionRequiredWarning;
                result = false;
            }

            return result;


        }

        


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuNewsletterAdminLabel);

            heading.Text = Resource.NewsLetterEditLetterHeading;

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";


            lnkLetterAdmin.Text = Resource.NewsLetterAdministrationHeading;
            lnkLetterAdmin.ToolTip = Resource.NewsLetterAdministrationHeading;
            lnkLetterAdmin.NavigateUrl = SiteRoot + "/eletter/Admin.aspx";

            lnkDraftList.Text = Resource.NewsLetterUnsentLettersHeading;
            lnkDraftList.ToolTip = Resource.NewsLetterUnsentLettersHeading;
            lnkDraftList.NavigateUrl = SiteRoot + "/eletter/LetterDrafts.aspx?l=" + letterInfoGuid.ToString();

            lnkManageTemplates.Text = Resource.LetterEditManageTemplatesLink;
            lnkManageTemplates.ToolTip = Resource.LetterEditManageTemplatesToolTip;
            lnkManageTemplates.NavigateUrl = SiteRoot + "/eletter/LetterTemplates.aspx";


            litHtmlTab.Text = Resource.HtmlTab;
            litPlainTextTab.Text = Resource.PlainTextTab;


            edContent.WebEditor.ToolBar = ToolBar.Newsletter;
            edContent.WebEditor.FullPageMode = true;
            edContent.WebEditor.EditorCSSUrl = string.Empty;

            edContent.WebEditor.Width = Unit.Percentage(100);
            edContent.WebEditor.Height = Unit.Pixel(800);

            btnSave.Text = Resource.NewsLetterSaveLetterButton;
            btnSave.ToolTip = Resource.NewsLetterSaveLetterButton;

            btnDelete.Text = Resource.NewsLetterDeleteLetterButton;
            btnDelete.ToolTip = Resource.NewsLetterDeleteLetterButton;
            //SiteUtils.SetButtonAccessKey(btnDelete, AccessKeys.NewsLetterEditDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialog(btnDelete, Resource.NewsLetterDeleteLetterButtonWarning);

            btnSendToList.Text = Resource.NewsLetterSendToSubscribersButton;
            btnSendToList.ToolTip = Resource.NewsLetterSendToSubscribersButton;
            UIHelper.AddConfirmationDialog(btnSendToList, Resource.NewsLetterSendToListButtonWarning);
            
            btnSendPreview.Text = Resource.NewsLetterSendPreviewButton;
            btnSendPreview.ToolTip = Resource.NewsLetterSendPreviewButton;

            btnLoadTemplate.Text = Resource.NewsLetterLoadHtmlTemplateButton;
            btnLoadTemplate.ToolTip = Resource.NewsLetterLoadHtmlTemplateButton;

            btnSaveAsTemplate.Text = Resource.NewsLetterSaveCurrentAsTemplateButton;
            btnSaveAsTemplate.ToolTip = Resource.NewsLetterSaveCurrentAsTemplateButtonToolTip;

            btnGeneratePlainText.Text = Resource.NewsletterGeneratePlainTextButton;
            btnGeneratePlainText.ToolTip = Resource.NewsletterGeneratePlainTextButton;

            UIHelper.AddConfirmationDialog(btnGeneratePlainText, Resource.NewsletterGeneratePlainTextButtonWarning);

            reqSubject.ErrorMessage = Resource.SubjectRequiredWarning;

            reqTemplateName.ErrorMessage = Resource.TemplateNameRequired;

        }

        private void LoadSettings()
        {
            currentUser = SiteUtils.GetCurrentSiteUser();
            letterInfoGuid = WebUtils.ParseGuidFromQueryString("l", Guid.Empty);
            imageSiteRoot = WebUtils.GetSiteRoot();

            

            if (letterInfoGuid == Guid.Empty) return;

            letterInfo = new LetterInfo(letterInfoGuid);

            if (letterInfo.SiteGuid != siteSettings.SiteGuid)
            {
                letterInfo = null;
                letterInfoGuid = Guid.Empty;
            }

            letterGuid = WebUtils.ParseGuidFromQueryString("letter", Guid.Empty);

            if (letterGuid == Guid.Empty)
            {
                letter = new Letter();
            }
            else
            {
                letter = new Letter(letterGuid);

                if (letter.LetterInfoGuid != letterInfo.LetterInfoGuid)
                {
                    letterGuid = Guid.Empty;
                    letter = null;


                }
            }

           
            if (letter.SendCompleteUtc > DateTime.MinValue)
            {
                btnSave.Text = Resource.SaveNewsletterAsNewDraft;
                
                
                btnSendToList.Enabled = false;
                // once a letter has been sent only admin can delete it.
                btnDelete.Enabled = WebUser.IsAdmin;
            }

            //spnAdmin.Visible = WebUser.IsAdminOrContentAdmin;
            lnkAdminMenu.Visible = WebUser.IsAdminOrContentAdmin;
            litLinkSeparator1.Visible = lnkAdminMenu.Visible;

            AddClassToBody("administration");
            AddClassToBody("letteredit");

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            
            base.OnInit(e);

            this.Load += new EventHandler(this.Page_Load);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.btnLoadTemplate.Click += new EventHandler(btnLoadTemplate_Click);
            this.btnSaveAsTemplate.Click += new EventHandler(btnSaveAsTemplate_Click);
            this.btnSendPreview.Click += new EventHandler(btnSendPreview_Click);
            this.btnSendToList.Click += new EventHandler(btnSendToList_Click);
            this.btnGeneratePlainText.Click += new EventHandler(btnGeneratePlainText_Click);

            SuppressMenuSelection();
            SuppressPageMenu();
            
            
        }


        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            SiteUtils.SetupNewsletterEditor(edContent);
            edContent.WebEditor.UseFullyQualifiedUrlsForResources = true;
        }
        

        

        #endregion
    }
}
