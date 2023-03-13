// Created:			            2012-08-15
//	Last Modified:              2018-01-03
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	[Themeable(true)]
    public partial class CommentsWidget : UserControl, IRefreshAfterPostback, IUpdateCommentStats
    {
        CommentRepository repository = null;
        private SiteSettings siteSettings = null;
        private SiteUser currentUser = null;
        private TimeZoneInfo timeZone;
        protected string AllowedImageUrlRegexPatern = SecurityHelper.RegexRelativeImageUrlPatern;
        protected bool CanManageUsers = false;
        protected mojoPortal.Web.UI.Avatar.RatingType MaxAllowedGravatarRating = SiteUtils.GetMaxAllowedGravatarRating();

        protected bool allowGravatars = false;
        protected bool disableAvatars = true;
        protected string UserNameTooltipFormat = "View User Profile for {0}";
        protected bool showUserRevenue = false;
        protected bool filterContentFromTrustedUsers = false;
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;


        #region Properties 


        private string commentSystem = "internal";
		[Themeable(false)]
        public string CommentSystem
        {
            get { return commentSystem; }
            set { commentSystem = value; }
        }


        private Guid siteGuid = Guid.Empty;
        
		[Themeable(false)]
		public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }

        private int siteId = -1;
		[Themeable(false)]
		public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        private Guid featureGuid = Guid.Empty;

		[Themeable(false)]
		public Guid FeatureGuid
        {
            get { return featureGuid; }
            set { featureGuid = value; }
        }

        private Guid moduleGuid = Guid.Empty;

		[Themeable(false)]
		public Guid ModuleGuid
        {
            get { return moduleGuid; }
            set { moduleGuid = value; }
        }

        private Guid contentGuid = Guid.Empty;

		[Themeable(false)]
		public Guid ContentGuid
        {
            get { return contentGuid; }
            set { contentGuid = value; }
        }

        private bool userCanModerate = false;

		[Themeable(false)]
		public bool UserCanModerate
        {
            get { return userCanModerate; }
            set { userCanModerate = value; }
        }

        private string headingText = string.Empty;

		[Themeable(true)]
		public string HeadingText
        {
            get { return headingText; }
            set { headingText = value; }
        }
        

        private string commentDateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
		[Themeable(true)]
		public string CommentDateTimeFormat
        {
            get { return commentDateTimeFormat; }
            set { commentDateTimeFormat = value; }
        }

		private string commentItemHeaderElement = "h4";

		[Themeable(true)]
		[Obsolete("Use CommentItemHeaderFormat", true)]
        public string CommentItemHeaderElement
        {
            get { return commentItemHeaderElement; }
            set { commentItemHeaderElement = value; }
        }

		private string commentItemHeaderFormat = "<h4>{0}</h4>";
		[Themeable(true)]
		public string CommentItemHeaderFormat
		{
			get => commentItemHeaderFormat;
			set => commentItemHeaderFormat = value;
		}

        private bool allowExternalImages = false;

		[Themeable(false)]
		public bool AllowExternalImages
        {
            get { return allowExternalImages; }
            set { allowExternalImages = value; }
        }

        private bool commentsClosed = false;

		[Themeable(false)]
		public bool CommentsClosed
        {
            get { return commentsClosed; }
            set { commentsClosed = value; }
        }

        private bool requireCaptcha = true;

		[Themeable(false)]
		public bool RequireCaptcha
        {
            get { return requireCaptcha; }
            set { requireCaptcha = value; }
        }

        private bool requireModeration = false;

		[Themeable(false)]
		public bool RequireModeration
        {
            get { return requireModeration; }
            set { requireModeration = value; }
        }

        private bool sortDescending = false;

		[Themeable(true)]
		public bool SortDescending
        {
            get { return sortDescending; }
            set { sortDescending = value; }
        }

        private List<string> notificationAddresses = new List<string>();

		[Themeable(false)]
		public List<string> NotificationAddresses
        {
            get { return notificationAddresses; }
        }
		private bool includeCommentBodyInNotification = false;
		[Themeable(false)]
		public bool IncludeCommentBodyInNotification
		{
			get { return includeCommentBodyInNotification; }
			set { includeCommentBodyInNotification = value; }
		}
		private string commentUrl = string.Empty;

		[Themeable(false)]
		public string CommentUrl
        {
            get { return commentUrl; }
            set { commentUrl = value; }
        }

        private string editBaseUrl = string.Empty;

		[Themeable(false)]
		public string EditBaseUrl
        {
            get { return editBaseUrl; }
            set { editBaseUrl = value; }
        }

        private string siteRoot = string.Empty;

		[Themeable(false)]
		public string SiteRoot
        {
            get { return siteRoot; }
            set { siteRoot = value; }
        }

        private bool includeIpAddressInNotification = true;

		[Themeable(false)]
		public bool IncludeIpAddressInNotification
        {
            get { return includeIpAddressInNotification; }
            set { includeIpAddressInNotification = value; }
        }

        private string notificationTemplateName = "BlogCommentNotificationEmail.config";

		[Themeable(false)]
		public string NotificationTemplateName
        {
            get { return notificationTemplateName; }
            set { notificationTemplateName = value; }
        }

        private string defaultCommentTitle = string.Empty;

		[Themeable(true)]
		public string DefaultCommentTitle
        {
            get { return defaultCommentTitle; }
            set { defaultCommentTitle = value; }
        }

        private IRefreshAfterPostback containerControl = null;
		[Themeable(false)]
		public IRefreshAfterPostback ContainerControl
        {
            get { return containerControl; }
            set { containerControl = value; }
        }

        private IUpdateCommentStats updateContainerControl = null;
		[Themeable(false)]
		public IUpdateCommentStats UpdateContainerControl
        {
            get { return updateContainerControl; }
            set { updateContainerControl = value; }
        }

        private string userEditIcon = "~/Data/SiteImages/user_edit.png";
		[Themeable(false)]
		public string UserEditIcon
        {
            get { return userEditIcon; }
            set { userEditIcon = value; }
        }

        private bool useCommentTitle = true;
		[Themeable(false)]
		public bool UseCommentTitle
        {
            get { return useCommentTitle; }
            set { useCommentTitle = value; }
        }

        private bool showUserUrl = true;
		[Themeable(false)]
		public bool ShowUserUrl
        {
            get { return showUserUrl; }
            set { showUserUrl = value; }
        }

        private string commentsClosedMessage = string.Empty;
		[Themeable(true)]
		public string CommentsClosedMessage
        {
            get { return commentsClosedMessage; }
            set { commentsClosedMessage = value; }
        }

        private bool requireAuthenticationToPost = false;

		[Themeable(false)]
		public bool RequireAuthenticationToPost
        {
            get { return requireAuthenticationToPost; }
            set { requireAuthenticationToPost = value; }
        }

        private bool alwaysShowSignInPromptIfNotAuthenticated = false;

		[Themeable(false)]
		public bool AlwaysShowSignInPromptIfNotAuthenticated
        {
            get { return alwaysShowSignInPromptIfNotAuthenticated; }
            set { alwaysShowSignInPromptIfNotAuthenticated = value; }
        }

        private bool showJanrainWidetOnSignInPrompt = false;

		[Themeable(false)]
		public bool ShowJanrainWidetOnSignInPrompt
        {
            get { return showJanrainWidetOnSignInPrompt; }
            set { showJanrainWidetOnSignInPrompt = value; }
        }

        private string authenticationRequiredMessage = string.Empty;
		[Themeable(false)]
		public string AuthenticationRequiredMessage
        {
            get { return authenticationRequiredMessage; }
            set { authenticationRequiredMessage = value; }
        }

        private int allowedEditMinutesForUnModeratedPosts = 10;

		[Themeable(false)]
		public int AllowedEditMinutesForUnModeratedPosts
        {
            get { return allowedEditMinutesForUnModeratedPosts; }
            set { allowedEditMinutesForUnModeratedPosts = value; }
        }

        private string disqusShortName = string.Empty;
		[Themeable(false)]
		public string DisqusShortName
        {
            get { return disqusShortName; }
            set { disqusShortName = value; }
        }

        private string intenseDebateAccountId = string.Empty;

		[Themeable(false)]
		public string IntenseDebateAccountId
        {
            get { return intenseDebateAccountId; }
            set { intenseDebateAccountId = value; }
        }

        private bool forceDisableAvatar = false;

		[Themeable(false)]
		public bool ForceDisableAvatar
        {
            get { return forceDisableAvatar; }
            set { forceDisableAvatar = value; }
        }

		private string outerPanelCssClass = "commentpanel";
		[Themeable(true)]
		public string OuterPanelCssClass
		{
			get => outerPanelCssClass;
			set => outerPanelCssClass = value;
		}

		private string innerPanelCssClass = "commentlist";
		[Themeable(true)]
		public string InnerPanelCssClass
		{
			get => innerPanelCssClass;
			set => innerPanelCssClass = value;
		}

		private string itemWrapperCssClass = "commentitem";
		[Themeable(true)]
		public string ItemWrapperCssClass
		{
			get => itemWrapperCssClass;
			set => itemWrapperCssClass = value;
		}

		private string itemTitleCssClass = "commenttitle";
		[Themeable(true)]
		public string ItemTitleCssClass
		{
			get => itemTitleCssClass;
			set => itemTitleCssClass = value;
		}

		private string itemBodyCssClass = "commentbody";
		[Themeable(true)]
		public string ItemBodyCssClass
		{
			get => itemBodyCssClass;
			set => itemBodyCssClass = value;
		}

		private string itemHeaderCssClass = "commentheading";
		[Themeable(true)]
		public string ItemHeaderCssClass
		{
			get => itemHeaderCssClass;
			set => itemHeaderCssClass = value;
		}

		private string dateWrapperCssClass = "commentdate";
		[Themeable(true)]
		public string DateWrapperCssClass
		{
			get => dateWrapperCssClass;
			set => dateWrapperCssClass = value;
		}

		private string leftPanelCssClass = "commentuserinfo";
		[Themeable(true)]
		public string LeftPanelCssClass
		{
			get => leftPanelCssClass;
			set => leftPanelCssClass = value;
		}

		private string rightPanelCssClass = "theactualdarncomment";
		[Themeable(true)]
		public string RightPanelCssClass
		{
			get => rightPanelCssClass;
			set => rightPanelCssClass = value;
		}

		private string usernameWrapperCssClass = "commentusername";
		[Themeable(true)]
		public string UsernameWrapperCssClass
		{
			get => usernameWrapperCssClass;
			set => usernameWrapperCssClass = value;
		}

		private string avatarWrapperCssClass = "commentuseravatar";
		[Themeable(true)]
		public string AvatarWrapperCssClass
		{
			get => avatarWrapperCssClass;
			set => avatarWrapperCssClass = value;
		}

		private string revenueWrapperCssClass = "commentuserrevenue";
		[Themeable(true)]
		public string RevenueWrapperCssClass
		{
			get => revenueWrapperCssClass;
			set => revenueWrapperCssClass = value;
		}

		private string manageUserLinkFormat = "<a href='{0}'><i class='fa fa-user-circle-o' aria-hidden='true'></i><span class='sr-only'>{1}</span></a>";
		[Themeable(true)]
		public string ManageUserLinkFormat
		{
			get => manageUserLinkFormat;
			set => manageUserLinkFormat = value;
		}

        public bool CheckKeywordBlacklist { get; set; } = true;

		#endregion


		protected void Page_Load(object sender, EventArgs e)
        {
            
            LoadSettings();
            PopulateLabels();
			ApplyThemeSettings();
        }

		private void ApplyThemeSettings()
		{
			pnlOuterPanel.CssClass = outerPanelCssClass;
			pnlInnerPanel.CssClass = innerPanelCssClass;


		}

		private void SetupCommentSystem()
        {
            switch (commentSystem)
            {
                case "disqus":
                    SetupDisqus();
                    break;

                case "intensedebate":
                    SetupIntenseDebate();
                    break;

                case "facebook":
                    SetupFacebook();
                    break;

                case "internal":
                default:
                    SetupInternalCommentSystem();
                    break;
            }

        }

        private void SetupInternalCommentSystem()
        {
            divCommentService.Visible = false;
            disqus.Disable = true;
            intenseDebate.Visible = false;
            fbComments.Visible = false;

            repository = new CommentRepository();
            timeZone = SiteUtils.GetUserTimeZone();
            if (allowExternalImages) { AllowedImageUrlRegexPatern = SecurityHelper.RegexAnyImageUrlPatern; }
            CanManageUsers = WebUser.IsInRoles(siteSettings.RolesThatCanManageUsers);

            switch (siteSettings.AvatarSystem)
            {
                case "gravatar":
                    allowGravatars = true;
                    disableAvatars = false;
                    break;

                case "internal":
                    allowGravatars = false;
                    disableAvatars = false;
                    break;

                case "none":
                default:
                    allowGravatars = false;
                    disableAvatars = true;
                    break;

            }

            if (forceDisableAvatar)
            {
                allowGravatars = false;
                disableAvatars = true;
            }

            showUserRevenue = (WebConfigSettings.ShowRevenueInForums && WebUser.IsInRoles(siteSettings.CommerceReportViewRoles));

            //CommentEditor editor 
            commentEditor.SiteGuid = siteSettings.SiteGuid;
            commentEditor.SiteId = siteSettings.SiteId;
            commentEditor.SiteRoot = siteRoot;
            commentEditor.CommentsClosed = commentsClosed;
            commentEditor.CommentUrl = commentUrl;
            commentEditor.ContentGuid = contentGuid;
            commentEditor.DefaultCommentTitle = defaultCommentTitle;
            commentEditor.FeatureGuid = featureGuid;
            commentEditor.ModuleGuid = moduleGuid;
            commentEditor.NotificationAddresses = notificationAddresses;
            commentEditor.NotificationTemplateName = notificationTemplateName;
            commentEditor.RequireCaptcha = requireCaptcha;
            commentEditor.RequireModeration = requireModeration;
            commentEditor.UserCanModerate = userCanModerate;
            commentEditor.Visible = !commentsClosed;
            commentEditor.CurrentUser = currentUser;
            commentEditor.IncludeIpAddressInNotification = includeIpAddressInNotification;
            commentEditor.ContainerControl = this;
            commentEditor.UpdateContainerControl = this;
            commentEditor.UseCommentTitle = useCommentTitle;
            commentEditor.ShowUserUrl = showUserUrl;
			commentEditor.IncludeCommentBodyInNotification = includeCommentBodyInNotification;
            commentEditor.CheckKeywordBlacklist = CheckKeywordBlacklist;

            pnlCommentsClosed.Visible = commentsClosed;
            if (!commentsClosed && requireAuthenticationToPost && !Request.IsAuthenticated)
            {
                pnlCommentsRequireAuthentication.Visible = true;
                commentEditor.Visible = false;
            }

            if (!commentsClosed && alwaysShowSignInPromptIfNotAuthenticated && !Request.IsAuthenticated)
            {
                pnlCommentsRequireAuthentication.Visible = true;
                
            }

            

            SetupScript();

            if (!IsPostBack)
            {
                BindComments();
            }
            

        }

        private void BindComments()
        {
            // no content to comment on
            if (contentGuid == Guid.Empty) { return; }
          

            rptComments.DataSource = GetComments();
            rptComments.DataBind();


        }

        private void SetupDisqus()
        {
            pnlOuterPanel.Visible = false;
            divCommentService.Visible = true;
            intenseDebate.Visible = false;
            fbComments.Visible = false;

            disqus.SiteShortName = siteSettings.DisqusSiteShortName;
            if (disqusShortName.Length > 0)
            {
                disqus.SiteShortName = disqusShortName;
            }

            disqus.RenderWidget = true;

            disqus.WidgetPageUrl = commentUrl;
            if (disqus.WidgetPageUrl.StartsWith("https"))
            {
                disqus.WidgetPageUrl = disqus.WidgetPageUrl.Replace("https", "http");
            }


        }

        private void SetupIntenseDebate()
        {
            pnlOuterPanel.Visible = false;
            divCommentService.Visible = true;
            intenseDebate.Visible = true;
            disqus.Disable = true;
            fbComments.Visible = false;

            intenseDebate.AccountId = siteSettings.IntenseDebateAccountId;
            if (intenseDebateAccountId.Length > 0)
            {
                intenseDebate.AccountId = intenseDebateAccountId;
            }
            intenseDebate.PostUrl = commentUrl;

        }

        private void SetupFacebook()
        {
            pnlOuterPanel.Visible = false;
            divCommentService.Visible = true;
            fbComments.Visible = true;
            disqus.Disable = true;
            intenseDebate.Visible = false;

            fbComments.DataHref = commentUrl;
            if (fbComments.DataHref.StartsWith("https"))
            {
                fbComments.DataHref = fbComments.DataHref.Replace("https", "http");
            }
            Control c = Page.Master.FindControl("fbsdk");
            if ((c != null) && (c is FacebookSdk))
            {
                FacebookSdk fbsdk = c as FacebookSdk;
                fbsdk.AlwaysRender = true;
            }

        }


        #region IRefreshAfterPostback

        public void RefreshAfterPostback()
        {
            if (containerControl != null)
            {
                containerControl.RefreshAfterPostback();
            }
            BindComments();
        }

        #endregion

        public void UpdateCommentStats(Guid contentGuid, int commentCount)
        {
            if (updateContainerControl != null)
            {
                updateContainerControl.UpdateCommentStats(contentGuid, commentCount);
            }
        }

        private List<Comment> GetComments()
        {
            if (sortDescending)
            {
                return repository.GetByContentDesc(contentGuid);
            }
            else
            {
                return repository.GetByContentAsc(contentGuid);
            }
        }

        

        /// <summary>
        /// Handles the item command
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void rptComments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "DeleteComment":

                    repository.Delete(new Guid(e.CommandArgument.ToString())); 
                    break;

                case "ApproveComment":

                    Comment c = repository.Fetch(new Guid(e.CommandArgument.ToString()));
                if (c != null)
                {
                    c.ModerationStatus = Comment.ModerationApproved;
                    repository.Save(c);

                }

                    break;

            }

         
            if (updateContainerControl != null)
            {
                int commentCount = repository.GetCount(ContentGuid, Comment.ModerationApproved);
                updateContainerControl.UpdateCommentStats(
                    ContentGuid,
                    commentCount);
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
        }


        void rptComments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Button btnDelete = e.Item.FindControl("btnDelete") as Button;
            UIHelper.AddConfirmationDialog(btnDelete, Resource.CommentDeleteWarning);
        }

        protected string FormatCommentDate(DateTime startDate)
        {
            //if (timeZone != null)
            //{
                return TimeZoneInfo.ConvertTimeFromUtc(startDate, timeZone).ToString(CommentDateTimeFormat);

            //}

            //return startDate.AddHours(TimeOffset).ToString(CommentDateTimeFormat);

        }

        private void PopulateLabels()
        {
            if (headingText.Length > 0)
            {
                commentListHeading.Text = headingText;
            }
            else
            {
                commentListHeading.Text = Resource.Comments;
            }

            if (commentsClosedMessage.Length > 0)
            {
                litCommentsClosed.Text = commentsClosedMessage;
            }
            else
            {
                litCommentsClosed.Text = Resource.CommentsAreClosed;
            }

            //if (authenticationRequiredMessage.Length > 0)
            //{
            //    litCommentsRequireAuthentication.Text = authenticationRequiredMessage;
            //}
            //else
            //{
            //    litCommentsRequireAuthentication.Text = Resource.CommentsRequireAuthenticationMessage;
            //}

            SignInOrRegisterPrompt signinPrompt = srPrompt as SignInOrRegisterPrompt;
            if (authenticationRequiredMessage.Length > 0)
            {
                signinPrompt.Instructions = authenticationRequiredMessage;
            }
            else
            {
                signinPrompt.Instructions = Resource.CommentsRequireAuthenticationMessage;
            }
            signinPrompt.ShowJanrainWidget = showJanrainWidetOnSignInPrompt;


           
        }

        protected bool UserCanEdit(Guid commentUserGuid, string commentUserEmail, int moderationStatus, DateTime createdUtc)
        {
            if (userCanModerate) { return true; }

            if ((requireModeration) && (moderationStatus == Comment.ModerationApproved)) { return false; } // no edits by user after moderation

            if ((currentUser != null) &&(commentUserGuid == currentUser.UserGuid)) 
            {
                if ((!requireModeration) || (moderationStatus == Comment.ModerationPending))
                {
                    TimeSpan t = DateTime.UtcNow - createdUtc;
                    if (t.Minutes < allowedEditMinutesForUnModeratedPosts) 
                    {
                        return true;
                    }
                }
            }

            //TO DO or cookie match user


            return false;
        }

        private void SetupScript()
        {
            StringBuilder script = new StringBuilder();

            script.Append("\n<script data-loader='CommentsWidget'>");

            script.Append("function ReloadPage() {");
            script.Append("location.reload(); ");
            script.Append("}");

            script.Append("$('a.ceditlink').colorbox({width:'85%', height:'85%', iframe:true, onClosed:ReloadPage});");

            script.Append("</script>");

            ScriptManager.RegisterStartupScript(this, typeof(Page), "cmoderation", script.ToString(), false);

        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            siteId = siteSettings.SiteId;
            currentUser = SiteUtils.GetCurrentSiteUser();

            SetupCommentSystem();

            

        }

        protected string GetProfileLinkOrLabel(int userId, string userName, string authorUrl)
        {
            if ((showUserUrl) &&(!string.IsNullOrEmpty(authorUrl)))
            {
                return "<a rel='nofollow' href='" + SecurityHelper.SanitizeHtml(authorUrl) + "'>" + userName + "</a>";
            }

            if (userId == -1) {  return userName;  }

            // user profile follows the same view rules as member list
            if (Request.IsAuthenticated)
            {
                // if we know the user is signed in and not in a role allowed then return username without a profile link
                if (!WebUser.IsInRoles(siteSettings.RolesThatCanViewMemberList)) { return userName; }
            }

            

            // if user is not authenticated we don't know if he will be allowed but he will be prompted to login first so its ok to show the link
            return "<a rel='nofollow' href='" + SiteRoot + "/ProfileView.aspx?userid=" + userId.ToInvariantString() + "'>" + userName + "</a>";

        }

		protected string GetProfileManageIcon(int userId)
		{
			if (!(CanManageUsers && (Convert.ToInt32(Eval("UserId")) > -1)))
			{
				return string.Empty;
			}

			return String.Format(ManageUserLinkFormat, SiteRoot + "/Admin/ManageUsers.aspx?userid=" + userId.ToString(), Resource.ManageUsersTitleLabel);
		}


        protected bool UseProfileLink()
        {
           
            if (Request.IsAuthenticated)
            {
                // if we know the user is signed in and not in a role allowed then return username without a profile link
                if (!WebUser.IsInRoles(siteSettings.RolesThatCanViewMemberList))
                {
                    return false;
                }
            }

            // if user is not authenticated we don't know if he will be allowed but he will be prompted to login first so its ok to show the link
            return true;
        }


        override protected void OnInit(EventArgs e)
        {
            Load += new EventHandler(this.Page_Load);
            rptComments.ItemCommand += new RepeaterCommandEventHandler(rptComments_ItemCommand);
            rptComments.ItemDataBound += new RepeaterItemEventHandler(rptComments_ItemDataBound);
            base.OnInit(e);
            
        }
    }

   
}