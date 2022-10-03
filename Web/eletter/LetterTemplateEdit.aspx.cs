/// Author:					
/// Created:				2007-12-29
/// Last Modified:			2014-03-18
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Net;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;
using log4net;

namespace mojoPortal.Web.ELetterUI
{
    public partial class LetterTemplateEditPage : NonCmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LetterTemplateEditPage));
        private Guid templateGuid = Guid.Empty;
        LetterHtmlTemplate currentTemplate = null;
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
            if (currentTemplate == null) return;

            if (Page.IsPostBack) return;

            txtTitle.Text = currentTemplate.Title;
            edContent.Text = currentTemplate.Html;

        }


        void btnSendPreview_Click(object sender, EventArgs e)
        {
            Page.Validate("preview");
            if (!Page.IsValid) return;

            string baseUrl = WebUtils.GetHostRoot();
            if (WebConfigSettings.UseFolderBasedMultiTenants)
            {
                // in folder based sites the relative urls in the editor will already have the folder name
                // so we want to use just the raw site root not the navigation root
                baseUrl = WebUtils.GetSiteRoot();
            }

            string content = SiteUtils.ChangeRelativeUrlsToFullyQualifiedUrls(baseUrl, ImageSiteRoot, edContent.Text);

            //log.Info(content);
      
            Email.SendEmail(
                SiteUtils.GetSmtpSettings(),
                siteSettings.DefaultEmailFromAddress,
                txtPreviewAddress.Text,
                string.Empty,
                string.Empty,
                txtTitle.Text,
                content,
                true,
                "Normal");

        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (currentTemplate == null) return;
            LetterHtmlTemplate.Delete(currentTemplate.Guid);
            WebUtils.SetupRedirect(this, lnkTemplateList.NavigateUrl);

        }

        void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("edit");
            if (!Page.IsValid) return;

            SaveTemplate();
            string reditectUrl = SiteRoot + "/eletter/LetterTemplateEdit.aspx?t=" + templateGuid.ToString();
            WebUtils.SetupRedirect(this, reditectUrl);


        }

        private void SaveTemplate()
        {
            if (currentTemplate == null) currentTemplate = new LetterHtmlTemplate();

            currentTemplate.Html = edContent.Text;
            currentTemplate.SiteGuid = siteSettings.SiteGuid;
            currentTemplate.Title = txtTitle.Text;
            currentTemplate.Save();
            templateGuid = currentTemplate.Guid;


        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuNewsletterAdminLabel);
            heading.Text = Resource.NewsLetterEditTemplateHeading;
            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";


            lnkLetterAdmin.Text = Resource.NewsLetterAdministrationHeading;
            lnkLetterAdmin.NavigateUrl = SiteRoot + "/eletter/Admin.aspx";

            lnkTemplateList.Text = Resource.LetterEditManageTemplatesLink;
            lnkTemplateList.ToolTip = Resource.LetterEditManageTemplatesToolTip;
            lnkTemplateList.NavigateUrl = SiteRoot + "/eletter/LetterTemplates.aspx";

            
            edContent.WebEditor.ToolBar = ToolBar.Newsletter;
            edContent.WebEditor.FullPageMode = true;
            edContent.WebEditor.Width = Unit.Percentage(100);
            edContent.WebEditor.Height = Unit.Pixel(800);
            

            btnSave.Text = Resource.LetterTemplateSaveButton;
            btnSave.ToolTip = Resource.LetterTemplateSaveButton;

            btnDelete.Text = Resource.LetterTemplateDeleteButton;
            btnDelete.ToolTip = Resource.LetterTemplateDeleteButton;
            //SiteUtils.SetButtonAccessKey(btnDelete, AccessKeys.NewsLetterEditDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialog(btnDelete, Resource.LetterTemplateDeleteButtonWarning);

            btnSendPreview.Text = Resource.LetterTemplateSendPreviewButton;
            btnSendPreview.ToolTip = Resource.LetterTemplateSendPreviewButton;

            if (templateGuid == Guid.Empty)
            {
                btnDelete.Visible = false;

            }

            reqTitle.ErrorMessage = Resource.NewsletterTemplateTitleRequired;
            reqPreviewAddress.ErrorMessage = Resource.NewsletterPreviewAddressRequired;
            regexPreviewAddress.ErrorMessage = Resource.NewsletterPreviewAddressRequired;
            
        }

        private void LoadSettings()
        {
            
            templateGuid = WebUtils.ParseGuidFromQueryString("t", Guid.Empty);

            if (templateGuid == Guid.Empty) return;

            currentTemplate = new LetterHtmlTemplate(templateGuid);
            if (currentTemplate.SiteGuid != siteSettings.SiteGuid)
            {
                templateGuid = Guid.Empty;
                currentTemplate = null;

            }

            lnkAdminMenu.Visible = WebUser.IsAdminOrContentAdmin;
            litLinkSeparator1.Visible = lnkAdminMenu.Visible;

            AddClassToBody("administration");
            AddClassToBody("lettertemplateedit");

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.btnSendPreview.Click += new EventHandler(btnSendPreview_Click);

            SuppressMenuSelection();
            SuppressPageMenu();

            //SiteUtils.SetupEditor(edContent, false);
            //edContent.WebEditor.UseFullyQualifiedUrlsForResources = true;


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
