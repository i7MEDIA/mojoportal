/// Author:				        
/// Created:			        2004-09-11
///	Last Modified:              2014-07-14
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Web.ForumUI
{
    public partial class ForumEdit : NonCmsBasePage
	{
		private int moduleId = -1;
        private int itemId = -1;
        //private String cacheDependencyKey;
        private string virtualRoot;
        private Double timeOffset = 0;
        ISettingControl allowedPostRolesSetting = null;
        ISettingControl moderatorRolesSetting = null;

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
            edContent.ProviderName = SiteUtils.GetEditorProviderName();
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);


            SiteUtils.SetupEditor(edContent, AllowSkinOverride, this);
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

            LoadSettings();

            
            if (!UserCanEditModule(moduleId, Forum.FeatureGuid))
			{
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (SiteUtils.IsFishyPost(this))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

			PopulateLabels();

			if (!IsPostBack) 
			{
				if (itemId > -1)
				{
					PopulateControls();
					
				}
				else
				{
					ShowNewForumControls();
				}

                if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();
                    lnkCancel.NavigateUrl = hdnReturnUrl.Value;

                }
			}

            AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsForumSection", "forums");

		}

		private void PopulateLabels()
		{
            Title = SiteUtils.FormatPageTitle(siteSettings, ForumResources.ForumEditForumLabel);
            heading.Text = ForumResources.ForumEditForumLabel;

            // TODO: implement
            divIsModerated.Visible = false;
            divIsActive.Visible = false;


            btnUpdate.Text = ForumResources.ForumEditUpdateButton;
            SiteUtils.SetButtonAccessKey(btnUpdate, ForumResources.ForumEditUpdateButtonAccessKey);

            ScriptConfig.EnableExitPromptForUnsavedContent = true;
            UIHelper.AddClearPageExitCode(btnUpdate);

            lnkCancel.Text = ForumResources.ForumEditCancelButton;
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            
            btnDelete.Text = ForumResources.ForumEditDeleteButton;
            SiteUtils.SetButtonAccessKey(btnDelete, ForumResources.ForumEditDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialogWithClearExitCode(btnDelete, ForumResources.DeleteForumWarning);

            
	
		}

		private void ShowNewForumControls()
		{
			this.btnDelete.Visible = false;
            this.btnUpdate.Text = ForumResources.ForumEditCreateButton;
			this.chkIsActive.Checked = true;
			this.txtSortOrder.Text = "100";
			this.txtPostsPerPage.Text = "10";
			this.txtThreadsPerPage.Text = "40";
            Forum forum = new Forum();
            allowedPostRolesSetting = allowedPostRoles as AllowedRolesSetting;
            allowedPostRolesSetting.SetValue(forum.RolesThatCanPost);
            chkIncludeInGoogleMap.Checked = forum.IncludeInGoogleMap;
            chkAddNoIndexMeta.Checked = forum.AddNoIndexMeta;
            chkRequireModForNotify.Checked = forum.RequireModForNotify;
            chkAllowTrustedDirectNotify.Checked = forum.AllowTrustedDirectNotify;
            chkVisible.Checked = forum.Visible;
			

		}

		private void PopulateControls()
		{
			Forum forum = new Forum(itemId);

            if (forum.ModuleId != moduleId)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
			
			this.lblCreatedDate.Text = forum.CreatedDate.AddHours(timeOffset).ToString();
            edContent.Text = forum.Description;
			this.txtTitle.Text = SecurityHelper.SanitizeHtml(forum.Title);
			this.chkIsActive.Checked = forum.IsActive;
			//this.chkAllowAnonymousPosts.Checked = forum.AllowAnonymousPosts;
			this.chkIsModerated.Checked = forum.IsModerated;
			this.txtSortOrder.Text = forum.SortOrder.ToString();
			this.txtPostsPerPage.Text = forum.PostsPerPage.ToString();
			this.txtThreadsPerPage.Text = forum.ThreadsPerPage.ToString();
            allowedPostRolesSetting = allowedPostRoles as AllowedRolesSetting;
            allowedPostRolesSetting.SetValue(forum.RolesThatCanPost);

            moderatorRolesSetting = moderatorRoles as AllowedRolesSetting;
            moderatorRolesSetting.SetValue(forum.RolesThatCanModerate);

            chkRequireModForNotify.Checked = forum.RequireModForNotify;
            chkAllowTrustedDirectNotify.Checked = forum.AllowTrustedDirectNotify;
            txtModeratorNotifyEmail.Text = forum.ModeratorNotifyEmail;
            chkIncludeInGoogleMap.Checked = forum.IncludeInGoogleMap;
            chkAddNoIndexMeta.Checked = forum.AddNoIndexMeta;
            chkClosed.Checked = forum.Closed;
            chkVisible.Checked = forum.Visible;
			
		}


		private void btnUpdate_Click(object sender, EventArgs e)
		{
			Forum forum = new Forum(itemId);
            if ((forum.ItemId > -1)&&(forum.ModuleId != moduleId))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
			
            //SiteUser siteUser = new SiteUser(siteSettings, Context.User.Identity.Name);
            SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
            if(siteUser != null)
			forum.CreatedByUserId = siteUser.UserId;

            forum.ModuleId = moduleId;
            forum.Description = edContent.Text;
			forum.Title = SecurityHelper.SanitizeHtml(this.txtTitle.Text);
			forum.IsActive = this.chkIsActive.Checked;
			//forum.AllowAnonymousPosts = this.chkAllowAnonymousPosts.Checked;
			forum.IsModerated = this.chkIsModerated.Checked;
			forum.SortOrder = int.Parse(this.txtSortOrder.Text);
			forum.PostsPerPage = int.Parse(this.txtPostsPerPage.Text);
			forum.ThreadsPerPage = int.Parse(this.txtThreadsPerPage.Text);

            allowedPostRolesSetting = allowedPostRoles as AllowedRolesSetting;
            forum.RolesThatCanPost = allowedPostRolesSetting.GetValue();

            moderatorRolesSetting = moderatorRoles as AllowedRolesSetting;
            forum.RolesThatCanModerate = moderatorRolesSetting.GetValue();

            forum.RequireModForNotify = chkRequireModForNotify.Checked;
            forum.AllowTrustedDirectNotify = chkAllowTrustedDirectNotify.Checked;
            forum.ModeratorNotifyEmail = txtModeratorNotifyEmail.Text;
            forum.IncludeInGoogleMap = chkIncludeInGoogleMap.Checked;
            forum.AddNoIndexMeta = chkAddNoIndexMeta.Checked;
            forum.Closed = chkClosed.Checked;
            forum.Visible = chkVisible.Checked;

			if(forum.Save())
			{
                CurrentPage.UpdateLastModifiedTime();
                //CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
                CacheHelper.ClearModuleCache(forum.ModuleId);
                
                if (hdnReturnUrl.Value.Length > 0)
                {
                    WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                    return;
                }

                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

			}
			
			
		}


		private void btnDelete_Click(object sender, EventArgs e)
		{
            Forum forum = new Forum(itemId);
            if (forum.ModuleId != moduleId)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            Forum.Delete(itemId);
            Forum.UpdateUserStats(-1); // updates all users
            CurrentPage.UpdateLastModifiedTime();

            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

		}

        private void LoadSettings()
        {
            virtualRoot = WebUtils.GetApplicationRoot();
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            itemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);
            //cacheDependencyKey = "Module-" + moduleId.ToString();
            timeOffset = SiteUtils.GetUserTimeOffset();
            edContent.WebEditor.ToolBar = ToolBar.Full;

            AddClassToBody("editforum");

        }
		
		
	}
}
