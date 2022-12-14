// Author:				        
// Created:			            2012-08-23
//	Last Modified:              2017-03-15
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Web.Framework;
using System;
using System.Collections;


namespace mojoPortal.Web.BlogUI
{
    public partial class CommentDialog : mojoDialogBasePage
    {
        protected int pageId = -1;
        protected int moduleId = -1;
        protected int itemId = -1;
        protected Guid commentGuid = Guid.Empty;
        private Blog blog = null;
        private Module module = null;
        private Hashtable moduleSettings = null;
        private BlogConfiguration config = null;
        private Comment comment = null;
        private CommentRepository commentRepository = null;
        private SiteUser currentUser = null;
        private bool userCanEdit = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            if (SiteUtils.SslIsAvailable() && (siteSettings.UseSslOnAllPages || CurrentPage.RequireSsl))
            {
                SiteUtils.ForceSsl();
            }
            else
            {
                SiteUtils.ClearSsl();
            }
            if (
                (!userCanEdit)
                ||(commentGuid == Guid.Empty)||(blog == null))
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

            if (UserCanEditModule(moduleId, Blog.FeatureGuid)) { return true; }

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

            blog = new Blog(itemId);
            module = GetModule(moduleId, Blog.FeatureGuid);
            commentRepository = new CommentRepository();

            if (blog.ModuleId != module.ModuleId)
            {
                blog = null;
                module = null;
                return;
            }

            comment = commentRepository.Fetch(commentGuid);
            if ((comment.ContentGuid != blog.BlogGuid)||(comment.ModuleGuid != module.ModuleGuid))
            {
                blog = null;
                module = null;
                return;
            }

            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);

            config = new BlogConfiguration(moduleSettings);

            currentUser = SiteUtils.GetCurrentSiteUser();

            userCanEdit = UserCanEditComment();

            commentEditor.SiteGuid = SiteInfo.SiteGuid;
            commentEditor.SiteId = SiteInfo.SiteId;
            commentEditor.SiteRoot = SiteRoot;
            commentEditor.CommentsClosed = false;
            //commentEditor.CommentUrl = Request.RawUrl;
            commentEditor.ContentGuid = blog.BlogGuid;
            //commentEditor.DefaultCommentTitle = defaultCommentTitle;
            commentEditor.FeatureGuid = Blog.FeatureGuid;
            commentEditor.ModuleGuid = module.ModuleGuid;
            //commentEditor.NotificationAddresses = notificationAddresses;
            //commentEditor.NotificationTemplateName = notificationTemplateName;
            commentEditor.RequireCaptcha = false;
            commentEditor.UserCanModerate = userCanEdit;
            //commentEditor.Visible = !commentsClosed;
            commentEditor.CurrentUser = currentUser;
            commentEditor.UserComment = comment;
            commentEditor.ShowRememberMe = false;
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