// Author:				        
// Created:			            2012-09-07
//	Last Modified:              2012-09-07
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using System;
using System.Collections;

namespace mojoPortal.Features.UI.Comments
{
    public partial class CommentsDialog : mojoDialogBasePage
    {
        protected int pageId = -1;
        protected int moduleId = -1;
        protected int itemId = -1;
        protected Guid commentGuid = Guid.Empty;
        
        private Module module = null;
        private Hashtable moduleSettings = null;
        private CommentsConfiguration config = null;
        private Comment comment = null;
        private CommentRepository commentRepository = null;
        private SiteUser currentUser = null;
        private bool userCanEdit = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();

            if (
                (!userCanEdit)
                || (commentGuid == Guid.Empty))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();

            if (!IsPostBack) { PopulateControls(); }

        }

        private void PopulateControls()
        {

        }

        private bool UserCanEditComment()
        {
            if (comment == null) { return false; }

            if (UserCanEditModule(moduleId, CommentsConfiguration.FeatureGuid)) { return true; }

            if ((config.RequireApprovalForComments) && (comment.ModerationStatus == Comment.ModerationApproved)) { return false; } // no edits by user after moderation

            if ((currentUser != null) && (comment.UserGuid == currentUser.UserGuid))
            {
                if ((!config.RequireApprovalForComments) || (comment.ModerationStatus == Comment.ModerationPending))
                {
                    TimeSpan t = DateTime.UtcNow - comment.CreatedUtc;
                    if (t.Minutes < config.AllowedEditMinutesForUnModeratedPosts)
                    {
                        return true;
                    }
                }
            }

            return false;

        }

        private void PopulateLabels()
        {
            //Title = ContactFormResources.ContactFormViewMessagesLink;

        }

        private void LoadSettings()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            itemId = WebUtils.ParseInt32FromQueryString("ItemID", itemId);
            commentGuid = WebUtils.ParseGuidFromQueryString("c", commentGuid);
            if (commentGuid == Guid.Empty) { return; }

           
            module = GetModule(moduleId, CommentsConfiguration.FeatureGuid);

            if (module == null) { return; }

            commentRepository = new CommentRepository();

            

            comment = commentRepository.Fetch(commentGuid);
            if ((comment.ContentGuid != module.ModuleGuid) || (comment.ModuleGuid != module.ModuleGuid))
            {
                
                module = null;
                return;
            }

            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);

            config = new CommentsConfiguration(moduleSettings);

            currentUser = SiteUtils.GetCurrentSiteUser();

            userCanEdit = UserCanEditComment();

            commentEditor.SiteGuid = SiteInfo.SiteGuid;
            commentEditor.SiteId = SiteInfo.SiteId;
            commentEditor.SiteRoot = SiteRoot;
            commentEditor.CommentsClosed = !config.AllowComments;
            //commentEditor.CommentUrl = Request.RawUrl;
            commentEditor.ContentGuid = module.ModuleGuid;
            //commentEditor.DefaultCommentTitle = defaultCommentTitle;
            commentEditor.FeatureGuid = CommentsConfiguration.FeatureGuid;
            commentEditor.ModuleGuid = module.ModuleGuid;
            //commentEditor.NotificationAddresses = notificationAddresses;
            //commentEditor.NotificationTemplateName = notificationTemplateName;
            commentEditor.RequireCaptcha = false;
            commentEditor.UserCanModerate = userCanEdit;
            //commentEditor.Visible = !commentsClosed;
            commentEditor.CurrentUser = currentUser;
            commentEditor.UserComment = comment;
            commentEditor.ShowRememberMe = false;

            commentEditor.UseCommentTitle = config.AllowCommentTitle;
            commentEditor.ShowUserUrl = config.AllowWebSiteUrlForComments;

            //commentEditor.IncludeIpAddressInNotification = includeIpAddressInNotification;
            //commentEditor.ContainerControl = this;

        }

        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);

            base.OnInit(e);


        }
    }
}