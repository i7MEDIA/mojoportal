using System;
using mojoPortal.Business;
using mojoPortal.Net;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Features.UI;

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
		if (config.DisableAvatars)
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
