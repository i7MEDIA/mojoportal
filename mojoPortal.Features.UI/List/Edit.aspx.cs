/// Author:					    
/// Created:				    2004-12-24
///	Last Modified:              2013-01-15

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.LinksUI 
{
    public partial class EditLinks : NonCmsBasePage
	{
		private Hashtable moduleSettings;
        private int moduleId = -1;
        private int itemId = -1;
        private ListConfiguration config = new ListConfiguration();

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);

            SiteUtils.SetupEditor(edDescription, AllowSkinOverride, this);
            
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.updateButton.Click += new EventHandler(this.UpdateBtn_Click);
            this.deleteButton.Click += new EventHandler(this.DeleteBtn_Click);
            

            SuppressPageMenu();
        }
        #endregion

        private void Page_Load(object sender, EventArgs e)
		{
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            SecurityHelper.DisableBrowserCache();

            LoadParams();

            if (!UserCanEditModule(moduleId, Link.FeatureGuid))
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
                if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();
                    lnkCancel.NavigateUrl = Request.UrlReferrer.ToString();

                }
            }
        }

        private void PopulateControls()
        {
            if (itemId > -1)
            {
                Link linkItem = new Link(itemId);
                if (linkItem.ModuleId != moduleId)
                {
                    SiteUtils.RedirectToAccessDeniedPage(this);
                    return;
                }

                txtTitle.Text = linkItem.Title;
                
                edDescription.Text = linkItem.Description;
                

                //String linkProtocol = "http://";
                //if (linkItem.Url.StartsWith("https://"))
                //{
                //    linkProtocol = "https://";
                //}
                //if (linkItem.Url.StartsWith("~/"))
                //{
                //    linkProtocol = "~/";
                //}

                //ListItem listItem = ddProtocol.Items.FindByValue(linkProtocol);
                //if (listItem != null)
                //{
                //    ddProtocol.ClearSelection();
                //    listItem.Selected = true;
                //}

                //txtUrl.Text = linkItem.Url.Replace(linkProtocol, String.Empty);
                txtUrl.Text = linkItem.Url;
                txtViewOrder.Text = linkItem.ViewOrder.ToString();

                if (linkItem.Target == "_blank")
                {
                    chkUseNewWindow.Checked = true;
                }

                SiteUser linkUser = new SiteUser(siteSettings, linkItem.CreatedByUser);

            }
            else
            {
                deleteButton.Visible = false;
            }

        }

        private void UpdateBtn_Click(Object sender, EventArgs e)
		{
            if (Page.IsValid) 
			{
				Link linkItem = new Link(itemId);
                if ((linkItem.ItemId > -1) && (linkItem.ModuleId != moduleId))
                {
                    SiteUtils.RedirectToAccessDeniedPage(this);
                    return;
                }

                linkItem.ContentChanged += new ContentChangedEventHandler(linkItem_ContentChanged);

                Module module = new Module(moduleId);
                linkItem.ModuleGuid = module.ModuleGuid;
				linkItem.ModuleId = this.moduleId;
				linkItem.Title = this.txtTitle.Text;
                if (chkUseNewWindow.Checked)
                {
                    linkItem.Target = "_blank";
                }
                else
                {
                    linkItem.Target = String.Empty;
                }
				
                linkItem.Description = edDescription.Text;

                if ((!ddProtocol.Visible) || (txtUrl.Text.StartsWith("/")))
                {
                    linkItem.Url = txtUrl.Text;
                }
                else
                {
                    linkItem.Url = ddProtocol.SelectedValue + txtUrl.Text.Replace("https://", String.Empty).Replace("http://", String.Empty).Replace("~/", String.Empty);
                }

				
                linkItem.ViewOrder = int.Parse(this.txtViewOrder.Text);
                SiteUser linkUser = SiteUtils.GetCurrentSiteUser();
                if (linkUser != null)
                {
                    linkItem.CreatedByUser = linkUser.UserId;
                    linkItem.UserGuid = linkUser.UserGuid;
                }
				
				if(linkItem.Save())
				{
                    CurrentPage.UpdateLastModifiedTime();
                    //CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
                    CacheHelper.ClearModuleCache(linkItem.ModuleId);
                    SiteUtils.QueueIndexing();
                    if (hdnReturnUrl.Value.Length > 0)
                    {
                        WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                        return;
                    }

                    WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
				}
				
            }
        }

        void linkItem_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["LinksIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }


        private void DeleteBtn_Click(Object sender, EventArgs e)
		{
            if (itemId != -1) 
			{
                Link link = new Link(itemId);

                if (link.ModuleId != moduleId)
                {
                    SiteUtils.RedirectToAccessDeniedPage(this);
                    return;
                }

                link.ContentChanged += new ContentChangedEventHandler(linkItem_ContentChanged);
                link.Delete();
                CurrentPage.UpdateLastModifiedTime();
                //CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
                CacheHelper.ClearModuleCache(moduleId);
                SiteUtils.QueueIndexing();
            }

            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
        }


        

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, LinkResources.EditLinkPageTitle);
            heading.Text = LinkResources.EditLinksDetailsLabel;
            reqTitle.ErrorMessage = LinkResources.LinksTitleRequiredHelp;
            reqUrl.ErrorMessage = LinkResources.LinksUrlRequiredHelp;
            reqViewOrder.ErrorMessage = LinkResources.LinksViewOrderRequiredHelp;

            if (config.DescriptionOnly) { reqUrl.Enabled = false; }

            updateButton.Text = LinkResources.EditLinksUpdateButton;
            SiteUtils.SetButtonAccessKey(updateButton, LinkResources.EditLinksUpdateButtonAccessKey);
            UIHelper.AddClearPageExitCode(updateButton);
            ScriptConfig.EnableExitPromptForUnsavedContent = true;

            lnkCancel.Text = LinkResources.EditLinksCancelButton;
            
            deleteButton.Text = LinkResources.EditLinksDeleteButton;
            SiteUtils.SetButtonAccessKey(deleteButton, LinkResources.EditLinksDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialogWithClearExitCode(deleteButton, LinkResources.LinksDeleteLinkWarning);

            ListItem listItem = ddProtocol.Items.FindByValue("~/");
            if (listItem != null)
            {
                listItem.Text = LinkResources.LinksEditRelativeLinkLabel;
            }

            fileBrowser.Text = LinkResources.Browse;
            fileBrowser.ToolTip = LinkResources.Browse;

        }

        private void LoadParams()
        {
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            itemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);

        }

        private void LoadSettings()
        {
            //cacheDependencyKey = "Module-" + moduleId.ToString();

            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
            config = new ListConfiguration(moduleSettings);

            edDescription.WebEditor.ToolBar = ToolBar.FullWithTemplates;

            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();

            ddProtocol.Visible = ListConfiguration.UseProtocolDropdown;
            fileBrowser.TextBoxClientId = txtUrl.ClientID;
            

            AddClassToBody("listedit");
            
        }
       
    }
}
