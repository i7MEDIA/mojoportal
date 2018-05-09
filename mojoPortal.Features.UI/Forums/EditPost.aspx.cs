/// Author:				        
/// Created:			        2004-09-18
///	Last Modified:              2014-07-13

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Net;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace mojoPortal.Web.ForumUI
{

    public partial class ForumPostEdit : NonCmsBasePage
	{
        
		private int moduleId = -1;
        private int forumId = -1;
        private int threadId = -1;
        private int postId = -1;
        private int pageId = -1;
        private int pageNumber = 1;
        private SiteUser theUser;
        protected ForumConfiguration config = null;
        private Forum forum = null;
        private string virtualRoot;
        private Double timeOffset = 0;
        private TimeZoneInfo timeZone = null;
        private bool isSubscribedToForum = false;
        private bool isSubscribedToThread = false;
        protected Hashtable moduleSettings;
        private bool isModerator = false;
 
        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);

            
            
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
            //this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);

            SecurityHelper.DisableBrowserCache();
            

            if ((siteSettings != null) && (CurrentPage != null))
            {
                if ((SiteUtils.SslIsAvailable())
                    && ((siteSettings.UseSslOnAllPages) || (CurrentPage.RequireSsl))
                    )
                {
                    SiteUtils.ForceSsl();
                }
                else
                {
                    SiteUtils.ClearSsl();
                }

            }

            SiteUtils.SetupEditor(edMessage, AllowSkinOverride, this);

            
        }

        #endregion

        private void Page_Load(object sender, EventArgs e)
		{
            LoadSettings();


            if (!UserCanViewPage(moduleId, Forum.FeatureGuid))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (forumId == -1)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
 
            if (forum.ItemId == -1)
            {
                Response.Redirect(SiteRoot);
            }

            if (moduleId != forum.ModuleId)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;

            }

            if (SiteUtils.IsFishyPost(this))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (!WebUser.IsInRoles(forum.RolesThatCanPost))
            {
                if (!Request.IsAuthenticated)
                {
                    SiteUtils.RedirectToLoginPage(this);
                    return;
                }

                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
                
            }

            // forum could allow "All Users" so isinroles would return true above even though not authenticated
            // if not authenticated then use a captcha
            if (Request.IsAuthenticated)
            {
                pnlAntiSpam.Visible = false;
                captcha.Enabled = false;
            }
            

            if((forum.Closed && !isModerator))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
           
			PopulateLabels();

			if(!Page.IsPostBack)
			{
				PopulateControls();
                if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();
                    lnkCancel.NavigateUrl = Request.UrlReferrer.ToString();

                }
			}

            AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsForumSection", "forums");
			

		}

		private void PopulateControls()
		{
            ForumThread thread = null;
		    if(threadId == -1)
			{
				this.btnDelete.Visible = false;
                postList.Visible = false;
                postListAlt.Visible = false;
                Title = SiteUtils.FormatPageTitle(siteSettings, CurrentPage.PageName + " - " + ForumResources.NewThreadLabel);
                
			}
			else
			{
			    
			    if(postId > -1)
				{
					thread = new ForumThread(threadId, postId);
                    if (isModerator
                        || ((this.theUser != null) && (this.theUser.UserId == thread.PostUserId))
                        )
                    {
                        this.txtSubject.Text = thread.PostSubject;
                        edMessage.Text = thread.PostMessage;
                    }
                    else
                    {
                        //user has no permission to edit this post
                        WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                        return;

                    }

                    if (isModerator)
                    {
                        divSortOrder.Visible = true;
                        txtSortOrder.Text = thread.PostSortOrder.ToInvariantString();
                    }
                    else if ((config.AllowEditingPostsLessThanMinutesOld != -1)&&(thread.CurrentPostDate < DateTime.UtcNow.AddMinutes(-config.AllowEditingPostsLessThanMinutesOld)))
                    {
                        // not allowing edit of older posts
                        WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                        return;
                    }
				}
				else
				{  
					thread = new ForumThread(threadId);
                    this.txtSubject.Text
                        = ResourceHelper.GetMessageTemplate(SiteUtils.GetDefaultUICulture(), "ForumPostReplyPrefix.config") 
                        + SecurityHelper.RemoveMarkup(thread.Subject);
                }

                if ((thread.IsLocked || thread.IsClosed(config.CloseThreadsOlderThanDays)) && (!isModerator))
                {
                    WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                    return;
                }


                if ((forum != null) && (thread != null))
                {
                    Title = SiteUtils.FormatPageTitle(siteSettings, forum.Title + " - " + SecurityHelper.RemoveMarkup(thread.Subject));
                }

                if (forumId == -1)
                {
                    forumId = thread.ForumId;
                }

                postList.Forum = forum;
                postListAlt.Forum = forum;

                postList.Thread = thread;
                postListAlt.Thread = thread;

                
			}

            if (forum != null)
            {
                heading.Text = forum.Title;
                litForumDescription.Text = forum.Description;
                divDescription.Visible = (forum.Description.Length > 0) && (!displaySettings.HideForumDescriptionOnPostEdit);
            }

           
            if (threadId == -1) //only focus the subject on new threads
            {
                string hookupInputScript = "<script type=\"text/javascript\">"
                     + "document.getElementById('" + this.txtSubject.ClientID + "').focus();</script>";

                if (!Page.ClientScript.IsStartupScriptRegistered("finitscript"))
                {
                    this.Page.ClientScript.RegisterStartupScript(
                        typeof(Page),
                        "finitscript", hookupInputScript);
                }

                edMessage.WebEditor.SetFocusOnStart = false;
            }
            else
            {

                edMessage.WebEditor.SetFocusOnStart = true;
            }
                
               

            chkNotifyOnReply.Checked = isSubscribedToThread;

            lnkPageCrumb.Text = CurrentPage.PageName;
            lnkPageCrumb.NavigateUrl = SiteUtils.GetCurrentPageUrl();

            if (ForumConfiguration.CombineUrlParams)
            {
                lnkForum.HRef = SiteRoot + "/Forums/ForumView.aspx?pageid=" + pageId.ToInvariantString()
                    + "&amp;f=" + forum.ItemId.ToInvariantString() + "~1" ;
            }
            else
            {
                lnkForum.HRef = SiteRoot + "/Forums/ForumView.aspx?ItemID="
                    + forum.ItemId.ToInvariantString()
                    + "&amp;pageid=" + pageId.ToInvariantString()
                    + "&amp;mid=" + forum.ModuleId.ToInvariantString();
            }

            lnkForum.InnerHtml = forum.Title;
            if (thread != null) { lblThreadDescription.Text = SecurityHelper.RemoveMarkup(thread.Subject); }
		}


		private void btnDelete_Click(object sender, EventArgs e)
		{
			ForumThread thread = new ForumThread(threadId,postId);
            bool userCanEditPost = false;
            if (isModerator
                   || ((this.theUser != null) && (this.theUser.UserId == thread.PostUserId) && (thread.ForumId == forumId))
               )
              {
                  userCanEditPost = true;
              }

            if (!userCanEditPost)
            {
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                return;

            }

            thread.ContentChanged += new ContentChangedEventHandler(thread_ContentChanged);
            
            

			if(thread.DeletePost(postId))
			{
                CurrentPage.UpdateLastModifiedTime();
               
                if (thread.PostUserId > -1)
                {
                    Forum.UpdateUserStats(thread.PostUserId);
                }

                SiteUtils.QueueIndexing();
			}

            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}

        

		private void btnCancel_Click(object sender, EventArgs e)
		{
            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}


		private void btnUpdate_Click(object sender, EventArgs e)
		{
            if (forum == null) { forum = new Forum(forumId); }

            if (WebUser.IsInRoles(forum.RolesThatCanPost))
            {
                if (Request.IsAuthenticated)
                {
                    captcha.Enabled = false;
                    pnlAntiSpam.Visible = false;
                }

            }
            else
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

			Page.Validate("Forum");
			if(!Page.IsValid)
			{
                PopulateControls();
                return;
            }
            else
            {
                if ((config.UseSpamBlockingForAnonymous) && (pnlAntiSpam.Visible)&&(captcha.Enabled))
                {
                    if (!captcha.IsValid)
                    {
                        PopulateControls();
                        return;
                    }
                }

				ForumThread thread;
				bool userIsAllowedToUpdateThisPost = false;
				if(threadId == -1)
				{
                    //new thread
					thread = new ForumThread();
					thread.ForumId = forumId;
                    thread.IncludeInSiteMap = forum.IncludeInGoogleMap;
                    thread.SetNoIndexMeta = forum.AddNoIndexMeta;
				}
				else
				{

					if(postId > -1)
					{
						thread = new ForumThread(threadId,postId);
                        if (isModerator || (this.theUser.UserId == thread.PostUserId))
						{
							userIsAllowedToUpdateThisPost = true;
						}

                        if ((isModerator) && (divSortOrder.Visible))
                        {
                            int sort = thread.PostSortOrder;
                            int.TryParse(txtSortOrder.Text, out sort);
                            thread.PostSortOrder = sort;
                        }

					}
					else
					{
						thread = new ForumThread(threadId);
					}

                    //existing thread but it does not belong to this forum
                    if (forumId != thread.ForumId)
                    {
                        SiteUtils.RedirectToAccessDeniedPage(this);
                        return;
                    }

				}

                thread.ContentChanged += new ContentChangedEventHandler(thread_ContentChanged);
				thread.PostSubject = this.txtSubject.Text;
                thread.PostMessage = edMessage.Text;

                bool isNewPost = (thread.PostId == -1);

                SiteUser siteUser = null;
			
				if(Request.IsAuthenticated)
				{
                    siteUser = SiteUtils.GetCurrentSiteUser();
                    if (siteUser != null) 
					thread.PostUserId = siteUser.UserId;
                    if (chkSubscribeToForum.Checked)
                    {
                        forum.Subscribe(siteUser.UserId);
                    }
                    else
                    {
                        thread.SubscribeUserToThread = this.chkNotifyOnReply.Checked;
                    }
				
				}
				else
				{
					thread.PostUserId = -1; //guest
				}

                string threadViewUrl;
                if (ForumConfiguration.CombineUrlParams)
                {
                    threadViewUrl = SiteRoot + "/Forums/Thread.aspx?pageid=" + pageId.ToInvariantString()
                        + "&t=" + thread.ThreadId.ToInvariantString()
                        + "~" + this.pageNumber.ToInvariantString();
                }
                else
                {
                    threadViewUrl = SiteRoot + "/Forums/Thread.aspx?thread="
                        + thread.ThreadId.ToInvariantString()
                        + "&mid=" + moduleId.ToInvariantString()
                        + "&pageid=" + pageId.ToInvariantString()
                        + "&ItemID=" + forumId.ToInvariantString()
                        + "&pagenumber=" + this.pageNumber.ToInvariantString();
                }

				if((thread.PostId == -1)||(userIsAllowedToUpdateThisPost))
				{
					thread.Post();
                    CurrentPage.UpdateLastModifiedTime();

                    if (ForumConfiguration.CombineUrlParams)
                    {
                        threadViewUrl = SiteRoot + "/Forums/Thread.aspx?pageid=" + pageId.ToInvariantString()
                            + "&t=" + thread.ThreadId.ToInvariantString()
                            + "~" + pageNumber.ToInvariantString()
                            + "#post" + thread.PostId.ToInvariantString();
                    }
                    else
                    {
                        threadViewUrl = SiteRoot + "/Forums/Thread.aspx?thread="
                            + thread.ThreadId.ToInvariantString()
                            + "&mid=" + moduleId.ToInvariantString()
                            + "&pageid=" + pageId.ToInvariantString()
                            + "&ItemID=" + forum.ItemId.ToInvariantString()
                            + "&pagenumber=" + pageNumber.ToInvariantString()
                            + "#post" + thread.PostId.ToInvariantString();
                    }

                    if ((isNewPost) || (!config.SuppressNotificationOfPostEdits))
                    {
                     
                        bool notifyModeratorOnly = false;

                        if (forum.RequireModForNotify)    
                        {
                            notifyModeratorOnly = true;

                            if(forum.AllowTrustedDirectNotify && (siteUser != null) && siteUser.Trusted)
                            {
                                notifyModeratorOnly = false;
                                
                            }

                        }
                        
                        Module m = GetModule(moduleId, Forum.FeatureGuid);

                        ForumNotification.NotifySubscribers(
                            forum,
                            thread,
                            m,
                            siteUser,
                            siteSettings,
                            config,
                            SiteRoot,
                            pageId,
                            pageNumber,
                            SiteUtils.GetDefaultCulture(),
                            ForumConfiguration.GetSmtpSettings(),
                            notifyModeratorOnly
                            );

                        if(!notifyModeratorOnly)
                        {
                            
                            thread.NotificationSent = true;
                            thread.UpdatePost();
                        }

                    }
                    
                    //String cacheDependencyKey = "Module-" + moduleId.ToInvariantString();
                    //CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
                    CacheHelper.ClearModuleCache(moduleId);
                    SiteUtils.QueueIndexing();
                   
				}

				
                Response.Redirect(threadViewUrl);
			}
		}

        

        void thread_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["ForumThreadIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }

        
        private void PopulateLabels()
        {
            reqSubject.ErrorMessage = ForumResources.ForumEditSubjectRequiredHelp;
            lblThreadDescription.Text = ForumResources.NewThreadLabel;

            btnUpdate.Text = ForumResources.ForumPostEditUpdateButton;
            SiteUtils.SetButtonAccessKey(btnUpdate, ForumResources.ForumPostEditUpdateButtonAccessKey);

            ScriptConfig.EnableExitPromptForUnsavedContent = true;

            UIHelper.DisableButtonAfterClickAndClearExitCode(
                btnUpdate,
                ForumResources.ButtonDisabledPleaseWait,
                Page.ClientScript.GetPostBackEventReference(this.btnUpdate, string.Empty)
                );

            lnkCancel.Text = ForumResources.ForumPostEditCancelButton;
            
            btnDelete.Text = ForumResources.ForumPostEditDeleteButton;
            SiteUtils.SetButtonAccessKey(btnDelete, ForumResources.ForumEditDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialogWithClearExitCode(btnDelete, ForumResources.ForumDeletePostWarning);

            if (postId == -1)
            {
                this.btnDelete.Visible = false;
            }

            if (!Request.IsAuthenticated) pnlNotify.Visible = false;
            if (isSubscribedToForum) pnlNotify.Visible = false;

            if (forumId == -1) pnlEdit.Visible = false;
        }


        

	    private void LoadSettings()
        {
            virtualRoot = WebUtils.GetApplicationRoot();

            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            forumId = WebUtils.ParseInt32FromQueryString("forumid", -1);
            threadId = WebUtils.ParseInt32FromQueryString("thread", -1);
            postId = WebUtils.ParseInt32FromQueryString("postid", -1);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            isModerator = UserCanEditModule(moduleId, Forum.FeatureGuid);
            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
            config = new ForumConfiguration(moduleSettings);

            postList.Config = config;
            postList.PageId = pageId;
            postList.ModuleId = moduleId;
            postList.ItemId = forumId;
            postList.ThreadId = threadId;
            postList.PageNumber = pageNumber;
            postList.IsAdmin = WebUser.IsAdmin ;
            postList.IsCommerceReportViewer = WebUser.IsInRoles(siteSettings.CommerceReportViewRoles);
            postList.SiteRoot = SiteRoot;
            postList.ImageSiteRoot = ImageSiteRoot;
            postList.SiteSettings = siteSettings;
            postList.IsEditable = false;
            postList.IsSubscribedToForum = true;

            postListAlt.Config = config;
            postListAlt.PageId = pageId;
            postListAlt.ModuleId = moduleId;
            postListAlt.ItemId = forumId;
            postListAlt.ThreadId = threadId;
            postListAlt.PageNumber = pageNumber;
            postListAlt.IsAdmin = postList.IsAdmin;
            postListAlt.IsCommerceReportViewer = WebUser.IsInRoles(siteSettings.CommerceReportViewRoles);
            postListAlt.SiteRoot = SiteRoot;
            postListAlt.ImageSiteRoot = ImageSiteRoot;
            postListAlt.SiteSettings = siteSettings;
            postListAlt.IsEditable = false;
            postListAlt.IsSubscribedToForum = true;

            if (Request.IsAuthenticated)
            {
                theUser = SiteUtils.GetCurrentSiteUser();
                if (theUser != null)
                {
                    if (forumId > -1)
                    {
                        isSubscribedToForum = Forum.IsSubscribed(forumId, theUser.UserId);
                    }
                    if (threadId > -1)
                    {
                        isSubscribedToThread = ForumThread.IsSubscribed(threadId, theUser.UserId);
                    }
                }
            }

            if (isModerator)
            {
                edMessage.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            }
            else if ((Request.IsAuthenticated)&&(WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles)))
            {
                edMessage.WebEditor.ToolBar = ToolBar.ForumWithImages;
            }
            else
            {
                edMessage.WebEditor.ToolBar = ToolBar.Forum;
            }

            edMessage.WebEditor.SetFocusOnStart = true;
            edMessage.WebEditor.Height = Unit.Parse("350px");

            if (config.UseSpamBlockingForAnonymous)
            {
                captcha.ProviderName = siteSettings.CaptchaProvider;
                captcha.Captcha.ControlID = "captcha" + moduleId.ToString(CultureInfo.InvariantCulture);
                captcha.RecaptchaPrivateKey = siteSettings.RecaptchaPrivateKey;
                captcha.RecaptchaPublicKey = siteSettings.RecaptchaPublicKey;
            }

            forum = new Forum(forumId);

            if (displaySettings.UseAltPostList)
            {
                postList.Visible = false;
                postListAlt.Visible = true;
            }

            AddClassToBody("editforumpost");

        }

	}
}
