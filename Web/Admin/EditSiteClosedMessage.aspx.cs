using System;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using Resources;


namespace mojoPortal.Web.AdminUI
{

    public partial class EditSiteClosedMessagePage : NonCmsBasePage
    {
		private int selectedSiteID = -2;
		private SiteSettings selectedSite;

		protected void Page_Load(object sender, EventArgs e)
        {

			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			if (!WebUser.IsAdminOrContentAdmin)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (SiteUtils.IsFishyPost(this))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
            
            LoadSettings();
            PopulateLabels();
            if (!IsPostBack)
            {
                PopulateControls();
            }

        }

        private void PopulateControls()
        {
            editor.Text = selectedSite.SiteIsClosedMessage;

        }

        void btnSave_Click(object sender, EventArgs e)
        {

            selectedSite.SiteIsClosedMessage = editor.Text;

			selectedSite.Save();

            CacheHelper.ClearSiteSettingsCache(selectedSite.SiteId);
            WebUtils.SetupRedirect(this, Request.RawUrl);

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.SiteClosedMessageLabel);

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

			lnkSiteList.Text = Resource.SiteList;
			lnkSiteList.NavigateUrl = SiteRoot + "/Admin/Siteist.aspx";

			lnkSiteSettings.Text = Resource.AdminMenuSiteSettingsLink;
			lnkSiteSettings.NavigateUrl = SiteRoot + "/Admin/SiteSettings.aspx";

			if (selectedSite.SiteId != siteSettings.SiteId)
			{
				lnkSiteSettings.NavigateUrl += $"?SiteId={selectedSite.SiteId}";
			}

			lnkCurrentPage.Text = Resource.SiteClosedMessageLabel;
            lnkCurrentPage.NavigateUrl = SiteRoot + "/Admin/EditSiteClosedMessage.aspx";

            heading.Text = Resource.SiteClosedMessageLabel;

            btnSave.Text = Resource.SaveButton;
            UIHelper.AddClearPageExitCode(btnSave);
            ScriptConfig.EnableExitPromptForUnsavedContent = true;
        }

        private void LoadSettings()
        {

			if (siteSettings.IsServerAdminSite && (Page.Request.Params.Get("SiteID") != null))
			{
				selectedSiteID = WebUtils.ParseInt32FromQueryString("SiteID", selectedSiteID);
			}
			if ((selectedSiteID != siteSettings.SiteId) && (selectedSiteID > -1))
			{
				selectedSite = new SiteSettings(selectedSiteID);
			}
			else
			{
				selectedSite = siteSettings;
			}

			editor.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            AddClassToBody("administration editclosedmessage");
        }

       


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);
            SiteUtils.SetupEditor(editor, false, this);


        }

        

        #endregion
    }
}
