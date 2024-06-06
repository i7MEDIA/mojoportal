using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Net;
using mojoPortal.Web;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using Resources;


namespace mojoPortal.Web.UI
{
	public partial class CommentEditor : UserControl
	{
		CommentRepository repository = null;

		private SiteSettings siteSettings = null;

		public SiteUser CurrentUser { get; set; } = null;

		private Guid siteGuid = Guid.Empty;

		public Guid SiteGuid
		{
			get { return siteGuid; }
			set { siteGuid = value; }
		}

		public int SiteId { get; set; } = -1;

		private Guid featureGuid = Guid.Empty;

		public Guid FeatureGuid
		{
			get { return featureGuid; }
			set { featureGuid = value; }
		}

		private Guid moduleGuid = Guid.Empty;

		public Guid ModuleGuid
		{
			get { return moduleGuid; }
			set { moduleGuid = value; }
		}

		private Guid contentGuid = Guid.Empty;

		public Guid ContentGuid
		{
			get { return contentGuid; }
			set { contentGuid = value; }
		}

		public bool UserCanModerate { get; set; } = false;

		private bool includeIpAddressInNotification = true;

		public bool IncludeIpAddressInNotification
		{
			get { return includeIpAddressInNotification; }
			set { includeIpAddressInNotification = value; }
		}

		public string NotificationTemplateName { get; set; } = "BlogCommentNotificationEmail.config";

		private List<string> notificationAddresses = new List<string>();

		public List<string> NotificationAddresses
		{
			get { return notificationAddresses; }
			set { notificationAddresses = value; }
		}

		public bool IncludeCommentBodyInNotification { get; set; } = false;

		private string commentUrl = string.Empty;

		public string CommentUrl
		{
			get { return commentUrl; }
			set { commentUrl = value; }
		}

		public bool CommentsClosed { get; set; } = false;

		private bool requireCaptcha = true;

		public bool RequireCaptcha
		{
			get { return requireCaptcha; }
			set { requireCaptcha = value; }
		}

		public bool RequireModeration { get; set; } = false;

		private string siteRoot = string.Empty;

		public string SiteRoot
		{
			get { return siteRoot; }
			set { siteRoot = value; }
		}

		public string DefaultCommentTitle { get; set; } = string.Empty;

		private IRefreshAfterPostback containerControl = null;
		public IRefreshAfterPostback ContainerControl
		{
			get { return containerControl; }
			set { containerControl = value; }
		}

		public IUpdateCommentStats UpdateContainerControl { get; set; } = null;

		private Comment userComment = null;
		public Comment UserComment
		{
			get { return userComment; }
			set { userComment = value; }
		}

		public bool UseCommentTitle { get; set; } = true;

		private bool showRememberMe = false;
		public bool ShowRememberMe
		{
			get { return showRememberMe; }
			set { showRememberMe = value; }
		}

		public bool ShowUserUrl { get; set; } = true;

		public bool CheckKeywordBlacklist { get; set; } = true;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Visible)
			{
				pnlAntiSpam.Visible = false;
				captcha.Visible = false;
				captcha.Enabled = false;
				return;
			}



			LoadSetings();
			PopulateControls();
		}

		private void PopulateControls()
		{


			// no content to comment on
			if (contentGuid == Guid.Empty) { return; }

			if (!IsPostBack)
			{
				txtCommentTitle.Text = DefaultCommentTitle;

				if (CurrentUser != null)
				{
					txtName.Text = CurrentUser.Name;
					txtEmail.Text = CurrentUser.Email;
					txtURL.Text = CurrentUser.WebSiteUrl;
				}

				if (userComment != null)
				{
					txtName.Text = userComment.UserName;
					txtEmail.Text = userComment.UserEmail;
					txtURL.Text = userComment.UserUrl;
					txtCommentTitle.Text = userComment.Title;
					edComment.Text = userComment.UserComment;

				}
			}
		}

		private void btnPostComment_Click(object sender, EventArgs e)
		{
			if (CommentsClosed)
			{
				WebUtils.SetupRedirect(this, Request.RawUrl);
				return;
			}

			//string title = txtCommentTitle.Text;
			//string commentText = edComment.Text;
			//string userName = txtName.Text;
			//string userEmail = txtEmail.Text;
			//string userUrl = txtURL.Text;


			if (!IsValidComment())
			{
				//PopulateControls();
				containerControl?.RefreshAfterPostback();

				//txtCommentTitle.Text = title;
				//edComment.Text = commentText;
				//txtName.Text = userName;
				//txtEmail.Text = userEmail;
				//txtURL.Text = userUrl;

				return;
			}
			//if (blog == null) { return; }

			//if (blog.AllowCommentsForDays < 0)
			//{
			//    WebUtils.SetupRedirect(this, Request.RawUrl);
			//    return;
			//}

			//DateTime endDate = blog.StartDate.AddDays((double)blog.AllowCommentsForDays);

			//if ((endDate < DateTime.UtcNow) && (blog.AllowCommentsForDays > 0)) return;

			if (this.chkRememberMe.Checked)
			{
				SetCookies();
			}

			Comment comment;
			if (userComment != null)
			{
				comment = userComment;
			}
			else
			{
				comment = new Comment();


			}
			comment.SiteGuid = SiteGuid;
			comment.FeatureGuid = FeatureGuid;
			comment.ModuleGuid = ModuleGuid;
			comment.ContentGuid = ContentGuid;
			comment.Title = txtCommentTitle.Text;
			comment.UserComment = edComment.Text;
			comment.UserName = txtName.Text;
			comment.UserUrl = txtURL.Text;
			comment.UserEmail = txtEmail.Text;

			if (userComment == null && CurrentUser != null)
			{
				//we check for userComment == null to be sure we aren't changing the user of a comment that is being changed by a moderator
				comment.UserGuid = CurrentUser.UserGuid;
				comment.UserName = CurrentUser.Name;
				comment.UserEmail = CurrentUser.Email;

			}
			comment.UserIp = SiteUtils.GetIP4Address();

			if (RequireModeration)
			{
				comment.ModerationStatus = Comment.ModerationPending;
			}
			else
			{
				comment.ModerationStatus = Comment.ModerationApproved;
			}

			repository.Save(comment);

			if (comment.ModerationStatus == Comment.ModerationApproved)
			{
				if (UpdateContainerControl != null)
				{
					int commentCount = repository.GetCount(comment.ContentGuid, Comment.ModerationApproved);
					UpdateContainerControl.UpdateCommentStats(
						comment.ContentGuid,
						commentCount);
				}
			}

			if (notificationAddresses.Count > 0)
			{
				SendCommentNotificationEmail();
			}

			WebUtils.SetupRedirect(this, Request.RawUrl);

			//Response.Redirect(CommentUrl, true);

			//ScriptManager.RegisterStartupScript(this, 
			//    this.GetType(), 
			//    "blog" + moduleGuid.ToString().Replace("-",string.Empty), 
			//    "location.href = '" + Request.RawUrl + "'", 
			//    true);

		}

		private void SendCommentNotificationEmail()
		{
			SmtpSettings smtpSettings = SiteUtils.GetSmtpSettings();

			string messageTemplate = ResourceHelper.GetMessageTemplate(SiteUtils.GetDefaultUICulture(), NotificationTemplateName);

			StringBuilder message = new StringBuilder();
			message.Append(messageTemplate);
			message.Replace("{SiteName}", siteSettings.SiteName);
			string commentLink;
			if (commentUrl.StartsWith("http"))
			{
				commentLink = commentUrl;
			}
			else
			{
				commentLink = SiteRoot + commentUrl.Replace("~", string.Empty);
			}
			message.Replace("{MessageLink}", commentLink);

			if (IncludeCommentBodyInNotification)
			{
				message.Replace("{CommentBody}", HttpUtility.HtmlDecode(edComment.Text.RemoveMarkup()));
			}
			else
			{
				message.Replace("{CommentBody}", string.Empty);
			}

			if (includeIpAddressInNotification)
			{
				message.Append("\n\nHTTP_USER_AGENT: " + Page.Request.ServerVariables["HTTP_USER_AGENT"] + "\n");
				message.Append("HTTP_HOST: " + Page.Request.ServerVariables["HTTP_HOST"] + "\n");
				message.Append("REMOTE_HOST: " + Page.Request.ServerVariables["REMOTE_HOST"] + "\n");
				message.Append("REMOTE_ADDR: " + SiteUtils.GetIP4Address() + "\n");
				message.Append("LOCAL_ADDR: " + Page.Request.ServerVariables["LOCAL_ADDR"] + "\n");
				message.Append("HTTP_REFERER: " + Page.Request.ServerVariables["HTTP_REFERER"] + "\n");
			}

			foreach (string emailAddress in notificationAddresses)
			{
				Email.SendEmail(
					smtpSettings,
					siteSettings.DefaultEmailFromAddress,
					emailAddress,
					string.Empty,
					string.Empty,
					siteSettings.SiteName,
					message.ToString(),
					false,
					"Normal");
			}
		}

		private bool IsValidComment()
		{

			if (edComment.Text.Length == 0) { return false; }
			if (edComment.Text == "<p>&#160;</p>") { return false; } //some editors add this when its empty

			bool result = true;

			try
			{
				Page.Validate("comments");
				result = Page.IsValid;
			}
			catch (NullReferenceException)
			{
				//Recaptcha throws nullReference here if it is not visible/disabled
			}
			catch (ArgumentNullException)
			{
				//manipulation can make the Challenge null on recaptcha
			}


			if (siteSettings.BadWordCheckingEnabled && (CheckKeywordBlacklist || siteSettings.BadWordCheckingEnforced))
			{
				if (edComment.Text.ContainsBadWords()
					|| txtName.Text.ContainsBadWords()
					|| txtEmail.Text.ContainsBadWords()
					|| txtCommentTitle.Text.ContainsBadWords())
				{
					lblMessage.Text = Resource.KeywordBlacklistHit;
					lblMessage.Visible = true;

					result = false;
				}
			}

			try
			{
				//if ((result) && (config.UseCaptcha))
				if ((requireCaptcha) && (pnlAntiSpam.Visible))
				{
					//result = captcha.IsValid;
					bool captchaIsValid = captcha.IsValid;
					if (captchaIsValid)
					{
						if (!result)
						{
							// they solved the captcha but somehting else is invalid
							// don't make them solve the captcha again
							pnlAntiSpam.Visible = false;
							captcha.Visible = false;
							captcha.Enabled = false;

						}
					}
					else
					{
						//captcha was invalid
						result = false;
					}
				}
			}
			catch (NullReferenceException)
			{
				return false;
			}
			catch (ArgumentNullException)
			{
				//manipulation can make the Challenge null on recaptcha
				return false;
			}

			if (SecurityHelper.IsPossibleXss(txtName.Text) 
				|| SecurityHelper.IsPossibleXss(txtCommentTitle.Text)
				)
			{
				return false;
			}

			return result;
		}

		private void SetCookies()
		{
			var commentUserCookie = new HttpCookie("commentUser", this.txtName.Text);
			commentUserCookie.Expires = DateTime.Now.AddMonths(1);
			Response.Cookies.Add(commentUserCookie);

			var userUrlCookie = new HttpCookie("UserUrl", this.txtURL.Text);
			userUrlCookie.Expires = DateTime.Now.AddMonths(1);
			Response.Cookies.Add(userUrlCookie);
		}

		private void LoadSetings()
		{
			siteSettings = CacheHelper.GetCurrentSiteSettings();
			repository = new CommentRepository();

			edComment.WebEditor.ToolBar = ToolBar.AnonymousUser;
			edComment.WebEditor.Height = Unit.Pixel(170);

			captcha.ProviderName = siteSettings.CaptchaProvider;
			captcha.Captcha.ControlID = "captcha" + contentGuid.ToString().Replace("-", "");
			//captcha.RecaptchaPrivateKey = siteSettings.RecaptchaPrivateKey;
			//captcha.RecaptchaPublicKey = siteSettings.RecaptchaPublicKey;

			regexUrl.ErrorMessage = Resource.WebSiteUrlRegexWarning;
			regexEmail.ErrorMessage = Resource.NewsletterEmailRegexxMessage;

			reqEmail.ErrorMessage = Resource.EmailRequired;
			reqName.ErrorMessage = Resource.RegisterNameRequiredMessage;

			if ((!requireCaptcha) || (Request.IsAuthenticated))
			{
				pnlAntiSpam.Visible = false;
				captcha.Visible = false;
				captcha.Enabled = false;
			}

			divCommentUrl.Visible = ShowUserUrl;

			if (Request.IsAuthenticated)
			{
				divUserName.Visible = displaySettings.ShowNameInputWhenAuthenticated;
				divUserEmail.Visible = displaySettings.ShowEmailInputWhenAuthenticated;
				if (ShowUserUrl)
				{
					divCommentUrl.Visible = displaySettings.ShowUrlInputWhenAuthenticated;
				}
			}

			divTitle.Visible = UseCommentTitle;
			pnlRemeberMe.Visible = showRememberMe && !Request.IsAuthenticated;

		}

		override protected void OnInit(EventArgs e)
		{
			this.Load += new EventHandler(this.Page_Load);

			btnPostComment.Click += new EventHandler(btnPostComment_Click);

			base.OnInit(e);
			////this.EnableViewState = this.UserCanEditPage();
			//basePage = Page as mojoBasePage;
			//SiteRoot = basePage.SiteRoot;
			//ImageSiteRoot = basePage.ImageSiteRoot;

			if (!String.IsNullOrWhiteSpace(displaySettings.PreferredEditor))
			{
				SiteUtils.SetupEditor(this.edComment, WebConfigSettings.UseSkinCssInEditor, displaySettings.PreferredEditor, true, false, Page);
			}
			else
			{
				SiteUtils.SetupEditor(this.edComment, true, Page);
			}

		}
	}
}