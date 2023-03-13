// Author:					
// Created:					2012-09-05
// Last Modified:			2014-05-14
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Net;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using System;

namespace mojoPortal.Features.UI
{

    public partial class CommentsModule : SiteModuleControl, IRefreshAfterPostback
    {
        // FeatureGuid 06451ec6-d4d7-47e3-a1ce-d19aaf7f98fe
        protected string CommentItemHeaderElement = "h4";
        private mojoBasePage basePage = null;
        private CommentsConfiguration config = new CommentsConfiguration();

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);

            basePage = Page as mojoBasePage;

        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }


        private void PopulateControls()
        {
            //TitleControl.EditUrl = SiteRoot + "/Comments/CommentsEdit.aspx";
            TitleControl.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }


        }

        private void SetupCommentSystem()
        {
            
            CommentsWidget comments = InternalCommentSystem as CommentsWidget;
            comments.CommentSystem = config.CommentSystem;

            comments.SiteGuid = basePage.SiteInfo.SiteGuid;
            comments.FeatureGuid = Blog.FeatureGuid;
            comments.ModuleGuid = ModuleConfiguration.ModuleGuid;
            comments.ContentGuid = ModuleConfiguration.ModuleGuid;
            //comments.CommentItemHeaderElement = displaySettings.CommentItemHeaderElement;
            comments.CommentDateTimeFormat = config.DateTimeFormat;
            comments.CommentsClosed = !config.AllowComments;
            comments.CommentsClosedMessage = BlogResources.BlogCommentsClosedMessage;
            comments.CommentUrl = SiteUtils.GetCurrentPageUrl();
            //comments.DefaultCommentTitle = "re: " + blog.Title;
            comments.IncludeIpAddressInNotification = true;
            comments.RequireCaptcha = config.UseCaptcha && !Request.IsAuthenticated;
            comments.ContainerControl = this;
            comments.CheckKeywordBlacklist = config.CheckKeywordBlacklist;
            //comments.UpdateContainerControl = this;
            comments.EditBaseUrl = $"{SiteRoot}/Comments/CommentsDialog.aspx?pageid={PageId}&mid={ModuleId}";

            if (config.NotifyOnComment)
            {
                if ((config.NotifyEmail.Length > 0) && (Email.IsValidEmailAddressSyntax(config.NotifyEmail)))
                {
                    comments.NotificationAddresses.Add(config.NotifyEmail);
                }
            }

            comments.NotificationTemplateName = "BlogCommentNotificationEmail.config";
            comments.SiteRoot = SiteRoot;
            comments.UserCanModerate = IsEditable;
            comments.Visible = true;
            comments.RequireModeration = config.RequireApprovalForComments;
            comments.RequireAuthenticationToPost = config.RequireAuthenticationForComments;
            //comments.AuthenticationRequiredMessage = BlogResources.CommentsRequireAuthenticationMessage;
            comments.UseCommentTitle = config.AllowCommentTitle;
            comments.ShowUserUrl = config.AllowWebSiteUrlForComments;
            comments.SortDescending = config.SortCommentsDescending;
            if(config.DisableAvatars)
            {
                comments.ForceDisableAvatar = true;
            }

        }

        #region IRefreshAfterPostback

        public void RefreshAfterPostback()
        {
            PopulateControls();
        }

        #endregion




        private void PopulateLabels()
        {
            //TitleControl.EditText = "Edit";
        }

        private void LoadSettings()
        {
            config = new CommentsConfiguration(Settings);
            SetupCommentSystem();
            

        }


    }
}
