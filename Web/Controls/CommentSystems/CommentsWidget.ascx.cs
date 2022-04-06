using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	[Themeable(true)]
	public partial class CommentsWidget : UserControl, IRefreshAfterPostback, IUpdateCommentStats
	{
		#region Fields

		private CommentRepository repository = null;
		private SiteSettings siteSettings = null;
		private SiteUser currentUser = null;
		private TimeZoneInfo timeZone;

		protected string AllowedImageUrlRegexPatern = SecurityHelper.RegexRelativeImageUrlPatern;
		protected bool CanManageUsers = false;
		protected Avatar.RatingType MaxAllowedGravatarRating = SiteUtils.GetMaxAllowedGravatarRating();

		protected bool allowGravatars = false;
		protected bool disableAvatars = true;
		protected string UserNameTooltipFormat = "View User Profile for {0}";
		protected bool showUserRevenue = false;
		protected bool filterContentFromTrustedUsers = false;
		protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;

		#endregion


		#region Properties 

		[Themeable(false)]
		public string CommentSystem { get; set; } = "internal";

		[Themeable(false)]
		public Guid SiteGuid { get; set; } = Guid.Empty;

		[Themeable(false)]
		public int SiteId { get; set; } = -1;

		[Themeable(false)]
		public Guid FeatureGuid { get; set; } = Guid.Empty;

		[Themeable(false)]
		public Guid ModuleGuid { get; set; } = Guid.Empty;

		[Themeable(false)]
		public Guid ContentGuid { get; set; } = Guid.Empty;

		[Themeable(false)]
		public bool UserCanModerate { get; set; } = false;

		[Themeable(true)]
		public string HeadingText { get; set; } = string.Empty;

		[Themeable(true)]
		public string CommentDateTimeFormat { get; set; } = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;

		[Themeable(true)]
		[Obsolete("Use CommentItemHeaderFormat", true)]
		public string CommentItemHeaderElement { get; set; } = "h4";
		[Themeable(true)]
		public string CommentItemHeaderFormat { get; set; } = "<h4>{0}</h4>";

		[Themeable(false)]
		public bool AllowExternalImages { get; set; } = false;

		[Themeable(false)]
		public bool CommentsClosed { get; set; } = false;

		[Themeable(false)]
		public bool RequireCaptcha { get; set; } = true;

		[Themeable(false)]
		public bool RequireModeration { get; set; } = false;

		[Themeable(true)]
		public bool SortDescending { get; set; } = false;

		[Themeable(false)]
		public List<string> NotificationAddresses { get; } = new List<string>();

		[Themeable(false)]
		public bool IncludeCommentBodyInNotification { get; set; } = false;

		[Themeable(false)]
		public string CommentUrl { get; set; } = string.Empty;

		[Themeable(false)]
		public string EditBaseUrl { get; set; } = string.Empty;

		[Themeable(false)]
		public string SiteRoot { get; set; } = string.Empty;

		[Themeable(false)]
		public bool IncludeIpAddressInNotification { get; set; } = true;

		[Themeable(false)]
		public string NotificationTemplateName { get; set; } = "BlogCommentNotificationEmail.config";

		[Themeable(true)]
		public string DefaultCommentTitle { get; set; } = string.Empty;

		[Themeable(false)]
		public IRefreshAfterPostback ContainerControl { get; set; } = null;

		[Themeable(false)]
		public IUpdateCommentStats UpdateContainerControl { get; set; } = null;

		[Themeable(false)]
		public string UserEditIcon { get; set; } = "~/Data/SiteImages/user_edit.png";

		[Themeable(false)]
		public bool UseCommentTitle { get; set; } = true;

		[Themeable(false)]
		public bool ShowUserUrl { get; set; } = true;

		[Themeable(true)]
		public string CommentsClosedMessage { get; set; } = string.Empty;

		[Themeable(false)]
		public bool RequireAuthenticationToPost { get; set; } = false;

		[Themeable(false)]
		public bool AlwaysShowSignInPromptIfNotAuthenticated { get; set; } = false;

		[Themeable(false)]
		public bool ShowJanrainWidetOnSignInPrompt { get; set; } = false;
		[Themeable(false)]
		public string AuthenticationRequiredMessage { get; set; } = string.Empty;

		[Themeable(false)]
		public int AllowedEditMinutesForUnModeratedPosts { get; set; } = 10;

		[Themeable(false)]
		public string DisqusShortName { get; set; } = string.Empty;

		[Themeable(false)]
		public string IntenseDebateAccountId { get; set; } = string.Empty;

		[Themeable(false)]
		public bool ForceDisableAvatar { get; set; } = false;

		[Themeable(true)]
		public string OuterPanelCssClass { get; set; } = "commentpanel";

		[Themeable(true)]
		public string InnerPanelCssClass { get; set; } = "commentlist";

		[Themeable(true)]
		public string ItemWrapperCssClass { get; set; } = "commentitem";

		[Themeable(true)]
		public string ItemTitleCssClass { get; set; } = "commenttitle";

		[Themeable(true)]
		public string ItemBodyCssClass { get; set; } = "commentbody";

		[Themeable(true)]
		public string ItemHeaderCssClass { get; set; } = "commentheading";

		[Themeable(true)]
		public string DateWrapperCssClass { get; set; } = "commentdate";

		[Themeable(true)]
		public string LeftPanelCssClass { get; set; } = "commentuserinfo";

		[Themeable(true)]
		public string RightPanelCssClass { get; set; } = "theactualdarncomment";

		[Themeable(true)]
		public string UsernameWrapperCssClass { get; set; } = "commentusername";

		[Themeable(true)]
		public string AvatarWrapperCssClass { get; set; } = "commentuseravatar";

		[Themeable(true)]
		public string RevenueWrapperCssClass { get; set; } = "commentuserrevenue";

		[Themeable(true)]
		public string ManageUserLinkFormat { get; set; } = "<a href='{0}'><i class='fa fa-user-circle-o' aria-hidden='true'></i><span class='sr-only'>{1}</span></a>";

		#endregion


		protected void Page_Load(object sender, EventArgs e)
		{

			LoadSettings();
			PopulateLabels();
			ApplyThemeSettings();
		}


		private void ApplyThemeSettings()
		{
			pnlOuterPanel.CssClass = OuterPanelCssClass;
			pnlInnerPanel.CssClass = InnerPanelCssClass;
		}


		private void SetupCommentSystem()
		{
			switch (CommentSystem)
			{
				case "disqus":
					if (!WebConfigSettings.DisableExternalCommentSystems)
					{
						SetupDisqus();
					}

					break;

				case "intensedebate":
					if (!WebConfigSettings.DisableExternalCommentSystems)
					{
						SetupIntenseDebate();
					}

					break;

				case "facebook":
					if (!WebConfigSettings.DisableExternalCommentSystems)
					{
						SetupFacebook();
					}

					break;

				default:
				case "internal":
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

			if (AllowExternalImages)
			{
				AllowedImageUrlRegexPatern = SecurityHelper.RegexAnyImageUrlPatern;
			}

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

			if (ForceDisableAvatar)
			{
				allowGravatars = false;
				disableAvatars = true;
			}

			showUserRevenue = WebConfigSettings.ShowRevenueInForums && WebUser.IsInRoles(siteSettings.CommerceReportViewRoles);

			//CommentEditor editor 
			commentEditor.SiteGuid = siteSettings.SiteGuid;
			commentEditor.SiteId = siteSettings.SiteId;
			commentEditor.SiteRoot = SiteRoot;
			commentEditor.CommentsClosed = CommentsClosed;
			commentEditor.CommentUrl = CommentUrl;
			commentEditor.ContentGuid = ContentGuid;
			commentEditor.DefaultCommentTitle = DefaultCommentTitle;
			commentEditor.FeatureGuid = FeatureGuid;
			commentEditor.ModuleGuid = ModuleGuid;
			commentEditor.NotificationAddresses = NotificationAddresses;
			commentEditor.NotificationTemplateName = NotificationTemplateName;
			commentEditor.RequireCaptcha = RequireCaptcha;
			commentEditor.RequireModeration = RequireModeration;
			commentEditor.UserCanModerate = UserCanModerate;
			commentEditor.Visible = !CommentsClosed;
			commentEditor.CurrentUser = currentUser;
			commentEditor.IncludeIpAddressInNotification = IncludeIpAddressInNotification;
			commentEditor.ContainerControl = this;
			commentEditor.UpdateContainerControl = this;
			commentEditor.UseCommentTitle = UseCommentTitle;
			commentEditor.ShowUserUrl = ShowUserUrl;
			commentEditor.IncludeCommentBodyInNotification = IncludeCommentBodyInNotification;

			pnlCommentsClosed.Visible = CommentsClosed;

			if (!CommentsClosed && RequireAuthenticationToPost && !Request.IsAuthenticated)
			{
				pnlCommentsRequireAuthentication.Visible = true;
				commentEditor.Visible = false;
			}

			if (!CommentsClosed && AlwaysShowSignInPromptIfNotAuthenticated && !Request.IsAuthenticated)
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
			if (ContentGuid == Guid.Empty)
			{
				return;
			}

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

			if (DisqusShortName.Length > 0)
			{
				disqus.SiteShortName = DisqusShortName;
			}

			disqus.RenderWidget = true;

			disqus.WidgetPageUrl = CommentUrl;

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

			if (IntenseDebateAccountId.Length > 0)
			{
				intenseDebate.AccountId = IntenseDebateAccountId;
			}

			intenseDebate.PostUrl = CommentUrl;
		}


		private void SetupFacebook()
		{
			pnlOuterPanel.Visible = false;
			divCommentService.Visible = true;
			fbComments.Visible = true;
			disqus.Disable = true;
			intenseDebate.Visible = false;

			fbComments.DataHref = CommentUrl;

			if (fbComments.DataHref.StartsWith("https"))
			{
				fbComments.DataHref = fbComments.DataHref.Replace("https", "http");
			}

			Control c = Page.Master.FindControl("fbsdk");

			if (c != null && c is FacebookSdk)
			{
				FacebookSdk fbsdk = c as FacebookSdk;

				fbsdk.AlwaysRender = true;
			}
		}


		#region IRefreshAfterPostback

		public void RefreshAfterPostback()
		{
			if (ContainerControl != null)
			{
				ContainerControl.RefreshAfterPostback();
			}

			BindComments();
		}

		#endregion


		public void UpdateCommentStats(Guid contentGuid, int commentCount)
		{
			if (UpdateContainerControl != null)
			{
				UpdateContainerControl.UpdateCommentStats(contentGuid, commentCount);
			}
		}


		private List<Comment> GetComments()
		{
			if (SortDescending)
			{
				return repository.GetByContentDesc(ContentGuid);
			}
			else
			{
				return repository.GetByContentAsc(ContentGuid);
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

			if (UpdateContainerControl != null)
			{
				var commentCount = repository.GetCount(ContentGuid, Comment.ModerationApproved);

				UpdateContainerControl.UpdateCommentStats(ContentGuid, commentCount);
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
			return TimeZoneInfo.ConvertTimeFromUtc(startDate, timeZone).ToString(CommentDateTimeFormat);
		}


		private void PopulateLabels()
		{
			if (HeadingText.Length > 0)
			{
				commentListHeading.Text = HeadingText;
			}
			else
			{
				commentListHeading.Text = Resource.Comments;
			}

			if (CommentsClosedMessage.Length > 0)
			{
				litCommentsClosed.Text = CommentsClosedMessage;
			}
			else
			{
				litCommentsClosed.Text = Resource.CommentsAreClosed;
			}

			SignInOrRegisterPrompt signinPrompt = srPrompt;

			if (AuthenticationRequiredMessage.Length > 0)
			{
				signinPrompt.Instructions = AuthenticationRequiredMessage;
			}
			else
			{
				signinPrompt.Instructions = Resource.CommentsRequireAuthenticationMessage;
			}

			signinPrompt.ShowJanrainWidget = ShowJanrainWidetOnSignInPrompt;
		}


		protected bool UserCanEdit(Guid commentUserGuid, string commentUserEmail, int moderationStatus, DateTime createdUtc)
		{
			if (UserCanModerate)
			{
				return true;
			}

			if (RequireModeration && moderationStatus == Comment.ModerationApproved)
			{
				return false; // no edits by user after moderation
			}

			if (currentUser != null && commentUserGuid == currentUser.UserGuid)
			{
				if (!RequireModeration || moderationStatus == Comment.ModerationPending)
				{
					TimeSpan t = DateTime.UtcNow - createdUtc;

					if (t.Minutes < AllowedEditMinutesForUnModeratedPosts)
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
			var script = @"
<script>
	function ReloadPage() {
		location.reload();
	}

	$('a.ceditlink').colorbox({
		width: '85%',
		height: '85%',
		iframe: true,
		onClosed: ReloadPage
	});
</script>";

			ScriptManager.RegisterStartupScript(this, typeof(Page), "cmoderation", script.ToString(), false);
		}


		private void LoadSettings()
		{
			siteSettings = CacheHelper.GetCurrentSiteSettings();
			SiteId = siteSettings.SiteId;
			currentUser = SiteUtils.GetCurrentSiteUser();

			SetupCommentSystem();
		}


		protected string GetProfileLinkOrLabel(int userId, string userName, string authorUrl)
		{
			if (ShowUserUrl && !string.IsNullOrEmpty(authorUrl))
			{
				return "<a rel='nofollow' href='" + SecurityHelper.SanitizeHtml(authorUrl) + "'>" + userName + "</a>";
			}

			if (userId == -1)
			{
				return userName;
			}

			// user profile follows the same view rules as member list
			if (Request.IsAuthenticated)
			{
				// if we know the user is signed in and not in a role allowed then return username without a profile link
				if (!WebUser.IsInRoles(siteSettings.RolesThatCanViewMemberList))
				{
					return userName;
				}
			}

			// if user is not authenticated we don't know if he will be allowed but he will be prompted to login first so its ok to show the link
			return "<a rel='nofollow' href='" + SiteRoot + "/ProfileView.aspx?userid=" + userId.ToInvariantString() + "'>" + userName + "</a>";
		}


		protected string GetProfileManageIcon(int userId)
		{
			return !(CanManageUsers && Convert.ToInt32(Eval("UserId")) > -1)
				? string.Empty
				: string.Format(ManageUserLinkFormat, SiteRoot + "/Admin/ManageUsers.aspx?userid=" + userId.ToString(), Resource.ManageUsersTitleLabel);
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
			Load += new EventHandler(Page_Load);
			rptComments.ItemCommand += new RepeaterCommandEventHandler(rptComments_ItemCommand);
			rptComments.ItemDataBound += new RepeaterItemEventHandler(rptComments_ItemDataBound);

			base.OnInit(e);
		}
	}
}
