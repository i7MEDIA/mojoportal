using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
    public partial class EmailTemplateEditor : UserControl
    {
        private SiteSettings siteSettings = null;

        private Guid templateGuid = Guid.Empty;
        private EmailTemplate template = null;
        private SiteUser currentUser = null;

        private Guid featureGuid = Guid.Empty;
        public Guid FeatureGuid
        {
            get { return featureGuid; }
            set { featureGuid = value; }
        }

        private Guid moduleGuid = Guid.Empty;
        public Guid ModuleGuid
        {
            get { return moduleGuid; }
            set { moduleGuid = value; }
        }

        private Guid specialGuid1 = Guid.Empty;
        public Guid SpecialGuid1
        {
            get { return specialGuid1; }
            set { specialGuid1 = value; }
        }

        private Guid specialGuid2 = Guid.Empty;
        public Guid SpecialGuid2
        {
            get { return specialGuid2; }
            set { specialGuid2 = value; }
        }

        

        private string editPageBaseUrl = string.Empty;

        public string EditPageBaseUrl
        {
            get { return editPageBaseUrl; }
            set { editPageBaseUrl = value; }
        }

        private bool showSubject = false;
        public bool ShowSubject
        {
            get { return showSubject; }
            set { showSubject = value; }
        }

        private string allowedTokens = string.Empty;
        public string AllowedTokens
        {
            get { return allowedTokens; }
            set { allowedTokens = value; }
        }

        private string allowedTokensHelpKey = string.Empty;
        public string AllowedTokensHelpKey
        {
            get { return allowedTokensHelpKey; }
            set { allowedTokensHelpKey = value; }
        }

        private string siteRoot = string.Empty;
        public string SiteRoot
        {
            get { return siteRoot; }
            set { siteRoot = value; }
        }

        private string imageSiteRoot = string.Empty;
        public string ImageSiteRoot
        {
            get { return imageSiteRoot; }
            set { imageSiteRoot = value; }
        }

        

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();
            
            if (!IsPostBack) { PopulateControls(); }

        }

        private void PopulateControls()
        {
            BindList();

            if (template == null) { return; }

            txtName.Text = template.Name;
            txtSubject.Text = template.Subject;
            edTemplate.Text = template.HtmlBody;
            txtPlainText.Text = template.TextBody;

        }


        private void BindList()
        {
            using (IDataReader reader = EmailTemplate.Get(
                siteSettings.SiteGuid, 
                featureGuid,
                moduleGuid,
                specialGuid1,
                specialGuid2))
            {
                ddTemplates.DataSource = reader;
                ddTemplates.DataBind();
            }

            btnLoad.Enabled = (ddTemplates.Items.Count > 0);
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("Template");
            if (!Page.IsValid) { return; }

            if (template == null)
            {
                template = new EmailTemplate();
                template.SiteGuid = siteSettings.SiteGuid;
                template.FeatureGuid = FeatureGuid;
                template.ModuleGuid = ModuleGuid;
                template.SpecialGuid1 = SpecialGuid1;
                template.SpecialGuid2 = SpecialGuid2;
            }

            template.Name = txtName.Text;
            template.Subject = txtSubject.Text;
            template.HtmlBody = edTemplate.Text;
            template.TextBody = txtPlainText.Text;
            template.HasHtml = true;
            if (currentUser != null)
            {
                template.LastModBy = currentUser.UserGuid;
            }
            template.LastModUtc = DateTime.UtcNow;

            template.Save();

            templateGuid = template.Guid;

            WebUtils.SetupRedirect(this, GetRedirectUrl());

        }

        void btnLoad_Click(object sender, EventArgs e)
        {
            if (ddTemplates.SelectedValue.Length == 36)
            {
                templateGuid = new Guid(ddTemplates.SelectedValue);
            }

            WebUtils.SetupRedirect(this, GetRedirectUrl());

        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            EmailTemplate.Delete(templateGuid);

            WebUtils.SetupRedirect(this, editPageBaseUrl);

        }

        private string GetRedirectUrl()
        {
            if (editPageBaseUrl.Contains("?"))
            {
                return editPageBaseUrl + "&t=" + templateGuid.ToString();
            }

            return editPageBaseUrl + "?t=" + templateGuid.ToString();
        }

        private void PopulateLabels()
        {
            lnkNew.Text = Resource.LetterHtmlTemplateAddNewLink;
            lnkNew.NavigateUrl = editPageBaseUrl;
            btnSave.Text = Resource.SaveButton;
            btnLoad.Text = Resource.Load;
            btnDelete.Text = Resource.DeleteButton;

            litHtmlTab.Text = Resource.HtmlTab;
            litPlainTextTab.Text = Resource.PlainTextTab;

            UIHelper.AddConfirmationDialog(btnDelete, Resource.LetterTemplateDeleteButtonWarning);
			
            reqTitle.ErrorMessage = Resource.TitleRequiredWarning;
            reqSubject.ErrorMessage = Resource.SubjectRequiredWarning;
        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            currentUser = SiteUtils.GetCurrentSiteUser();
            templateGuid = WebUtils.ParseGuidFromQueryString("t", templateGuid);

            divSubject.Visible = ShowSubject;
            reqSubject.Enabled = showSubject;
            reqSubject.Visible = ShowSubject;

            pnlTokens.Visible = allowedTokens.Length > 0;
            litTokens.Text = $"<div class='allowed-tokens'><pre>{allowedTokens.Replace(" ", "\r\n")}</pre></div>";
            hlpTokens.HelpKey = allowedTokensHelpKey;

            if (templateGuid != Guid.Empty)
            {
                template = new EmailTemplate(templateGuid);
                if (template.Guid == Guid.Empty) { template = null; }
                if (template.FeatureGuid != FeatureGuid) { template = null; }
                if (template.SiteGuid != siteSettings.SiteGuid) { template = null; }
                if (template.ModuleGuid != moduleGuid) { template = null; }
            }

            edTemplate.WebEditor.ToolBar = ToolBar.Newsletter;
            edTemplate.WebEditor.FullPageMode = true;
            edTemplate.WebEditor.EditorCSSUrl = string.Empty;

            edTemplate.WebEditor.Width = Unit.Percentage(100);
            edTemplate.WebEditor.Height = Unit.Pixel(800);
        }

        override protected void OnInit(EventArgs e)
        {
            Load += new EventHandler(this.Page_Load);

            btnSave.Click += new EventHandler(btnSave_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnLoad.Click += new EventHandler(btnLoad_Click);

            SiteUtils.SetupNewsletterEditor(edTemplate);
            edTemplate.WebEditor.UseFullyQualifiedUrlsForResources = true;

            base.OnInit(e);	
        }
    }
}