/// Author:				        Dean Brettle
/// Created:			        2005-09-06
///	Last Modified:              2013-01-15
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Features;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Data;

namespace mojoPortal.Web.ForumUI
{

    public partial class ForumThreadEdit : NonCmsBasePage
	{
        private int moduleId = -1;
		private int threadId = -1;
		private SiteUser siteUser;
		private ForumThread forumThread;
        private bool userCanEdit = false;

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            base.OnInit(e);

            SuppressPageMenu();
        }

        #endregion


        private void Page_Load(object sender, EventArgs e)
		{
            
            SecurityHelper.DisableBrowserCache();

            LoadParams();
            
            userCanEdit = UserCanEditModule(moduleId, Forum.FeatureGuid);
            if (!userCanEdit)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            forumThread = new ForumThread(threadId);
            if (forumThread.ModuleId != moduleId)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (SiteUtils.IsFishyPost(this))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
            
            ForumThreadIndexBuilderProvider indexBuilder 
                = (ForumThreadIndexBuilderProvider)IndexBuilderManager.Providers["ForumThreadIndexBuilderProvider"];
            
            if (indexBuilder != null)
            {
                forumThread.ThreadMoved += new ForumThread.ThreadMovedEventHandler(indexBuilder.ThreadMovedHandler);
            }

            siteUser = SiteUtils.GetCurrentSiteUser();
			
			PopulateLabels();

			if (!IsPostBack) 
			{
				PopulateControls();
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
            Title = SiteUtils.FormatPageTitle(siteSettings, ForumResources.ForumThreadEditLabel);
            heading.Text = ForumResources.ForumThreadEditLabel;
            btnUpdate.Text = ForumResources.ForumThreadUpdateButton;
            SiteUtils.SetButtonAccessKey(btnUpdate, ForumResources.ForumEditUpdateButtonAccessKey);

            lnkCancel.Text = ForumResources.ForumThreadCancelButton;
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            
            btnDelete.Text = ForumResources.ForumThreadDeleteButton;
            SiteUtils.SetButtonAccessKey(btnDelete, ForumResources.ForumEditDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialog(btnDelete, ForumResources.ForumDeleteThreadWarning);

            
		}

		private void PopulateControls()
		{
			Forum forum = new Forum(forumThread.ForumId);
			txtSubject.Text = SecurityHelper.RemoveMarkup(forumThread.Subject);
            txtSortOrder.Text = forumThread.SortOrder.ToInvariantString();
            txtPageTitleOverride.Text = forumThread.PageTitleOverride;
            chkIsLocked.Checked = forumThread.IsLocked;
            chkIncludeInSiteMap.Checked = forumThread.IncludeInSiteMap;
            chkSetNoIndexMeta.Checked = forumThread.SetNoIndexMeta;
            
            using (IDataReader reader = Forum.GetForums(forum.ModuleId, siteUser.UserId))
            {
                ddForumList.DataSource = reader;
                ddForumList.DataBind();
            }
			this.ddForumList.SelectedValue = forumThread.ForumId.ToInvariantString();
			
		}

		


		private void btnUpdate_Click(object sender, EventArgs e)
		{
			forumThread.ForumId = int.Parse(this.ddForumList.SelectedValue);
			forumThread.Subject = this.txtSubject.Text;
            forumThread.PageTitleOverride = txtPageTitleOverride.Text;

            int sort = 100;
            int.TryParse(txtSortOrder.Text, out sort);
            forumThread.SortOrder = sort;
            forumThread.IsLocked = chkIsLocked.Checked;
            forumThread.IncludeInSiteMap = chkIncludeInSiteMap.Checked;
            forumThread.SetNoIndexMeta = chkSetNoIndexMeta.Checked;

			forumThread.UpdateThread();

            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}


		private void btnDelete_Click(object sender, EventArgs e)
		{
            // remove the thread from the search index
            ForumThread forumThread = new ForumThread(threadId);

			ForumThread.Delete(threadId);
            Forum.UpdateUserStats(-1); // updates all users

            ForumThreadIndexBuilderProvider.RemoveForumIndexItem(
                moduleId,
                forumThread.ForumId,
                threadId,
                -1);

            SiteUtils.QueueIndexing();

            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}

        private void LoadParams()
        {
            threadId = WebUtils.ParseInt32FromQueryString("thread", threadId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);

            AddClassToBody("editforumthread");
        }

		
	}
}
