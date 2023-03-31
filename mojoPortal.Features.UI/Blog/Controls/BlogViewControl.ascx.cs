//  Created:			        2004-08-15
//	Last Modified:              2018-02-24
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Net;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Web.BlogUI
{
	public partial class BlogViewControl : UserControl, IRefreshAfterPostback, IUpdateCommentStats
	{
		#region Properties

		private static readonly ILog log = LogManager.GetLogger(typeof(BlogViewControl));
		private Hashtable moduleSettings;
		protected BlogConfiguration config = new BlogConfiguration();
		private SiteUser currentUser = null;
		private string virtualRoot;
		private string addThisAccountId = string.Empty;
		protected Blog blog = null;
		private Module module;
		protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;

		protected int PageId = -1;
		protected int ModuleId = -1;
		protected int ItemId = -1;
		protected bool AllowComments = false;

		protected string CommentDateTimeFormat;
		protected bool parametersAreInvalid = false;
		protected Double TimeOffset = 0;
		private TimeZoneInfo timeZone = null;

		protected bool IsEditable = false;
		protected string addThisCustomBrand = string.Empty;

		protected string EditContentImage = ConfigurationManager.AppSettings["EditContentImage"];
		protected string GmapApiKey = string.Empty;

		protected string blogAuthor = string.Empty;
		protected string SiteRoot = string.Empty;
		protected string ImageSiteRoot = string.Empty;
		private mojoBasePage basePage;

		private string DisqusSiteShortName = string.Empty;
		private string IntenseDebateAccountId = string.Empty;

		protected string RegexRelativeImageUrlPatern = @"^/.*[_a-zA-Z0-9]+\.(png|jpg|jpeg|gif|PNG|JPG|JPEG|GIF)$";
		private bool useFriendlyUrls = true;
		protected string CategoriesResourceKey = "PostCategories";
		protected string CommentItemHeaderElement = "h4";
		protected string attachmentBaseUrl = string.Empty;

		protected bool allowGravatars = false;
		protected bool disableAvatars = true;
		protected mojoPortal.Web.UI.Avatar.RatingType MaxAllowedGravatarRating = UI.Avatar.RatingType.PG;
		protected string UserNameTooltipFormat = "View User Profile for {0}";

		protected string blogTitle = string.Empty;
		protected string blogSubTitle = string.Empty;

		private CommentsWidget comments = null;

		private bool showCommentCountInNav = true;

		#endregion

		#region OnInit

		override protected void OnInit(EventArgs e)
		{
			this.Load += new EventHandler(this.Page_Load);
			this.btnPostComment.Click += new EventHandler(this.btnPostComment_Click);
			this.dlComments.ItemCommand += new RepeaterCommandEventHandler(dlComments_ItemCommand);
			this.dlComments.ItemDataBound += new RepeaterItemEventHandler(dlComments_ItemDataBound);
			base.OnInit(e);
			//this.EnableViewState = this.UserCanEditPage();
			basePage = Page as mojoBasePage;
			SiteRoot = basePage.SiteRoot;
			ImageSiteRoot = basePage.ImageSiteRoot;


			SiteUtils.SetupEditor(this.edComment, true, Page);

		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
		}


		#endregion

		private void Page_Load(object sender, EventArgs e)
		{
			LoadParams();

			if (basePage == null || !basePage.UserCanViewPage(ModuleId, Blog.FeatureGuid))
			{
				if (!Request.IsAuthenticated)
				{
					SiteUtils.RedirectToLoginPage(this, Request.RawUrl);
					return;
				}

				SiteUtils.RedirectToAccessDeniedPage(this);
				return;
			}

			if (parametersAreInvalid)
			{
				AllowComments = false;
				pnlInnerWrap.Visible = false;
				return;
			}

			LoadSettings();

			SetupRssLink();
			PopulateLabels();

			if (!IsPostBack && ModuleId > 0 && ItemId > 0)
			{
				PopulateControls();
			}

			basePage.LoadSideContent(config.ShowLeftContent, config.ShowRightContent);
			basePage.LoadAltContent(BlogConfiguration.ShowTopContent, BlogConfiguration.ShowBottomContent);
		}



		protected virtual void PopulateControls()
		{
			if (parametersAreInvalid)
			{
				AllowComments = false;
				pnlInnerWrap.Visible = false;
				return;
			}

			if (blog.EndDate < DateTime.UtcNow)
			{
				expired.Visible = true;

				if (ConfigHelper.GetBoolProperty("Blog:Use410StatusOnExpiredPosts", true))
				{
					//http://support.google.com/webmasters/bin/answer.py?hl=en&answer=40132
					// 410 means the resource is gone but once existed
					// google treats it as more permanent than a 404
					// and it should result in de-indexing the content
					Response.StatusCode = 410;
					Response.StatusDescription = "Content Expired";
				}

				if (!basePage.UserCanEditModule(ModuleId, Blog.FeatureGuid))
				{
					pnlInnerWrap.Visible = false;
					return;

				}

			}

			// if not published only the editor can see it
			if (
				((!blog.IsPublished) || (blog.StartDate > DateTime.UtcNow))
				&& (!basePage.UserCanEditModule(ModuleId, Blog.FeatureGuid))
				)
			{
				AllowComments = false;
				pnlInnerWrap.Visible = false;
				WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
				return;
			}

			blogTitle = SecurityHelper.RemoveMarkup(blog.Title);
			blogSubTitle = SecurityHelper.RemoveMarkup(blog.SubTitle);
			heading.Text = blogTitle;
			if (CacheHelper.GetCurrentPage().ShowPageHeading && config.UsePostTitleAsPageHeading)
			{
				Control pageTitle = Page.Master.FindControl("PageTitle1");
				if (pageTitle == null)
				{
					pageTitle = Page.Master.FindControl("PageHeading1");
				}

				if (pageTitle == null)
				{
					heading.Visible = true;
				}
				else
				{
					basePage.PageHeading.Title.Text = blogTitle;
					heading.Visible = false;
				}

				
			}

			if (displaySettings.ShowSubTitleOnDetailPage && (blogSubTitle.Length > 0))
			{
				litSubtitle.Text = 
					"<" +
					displaySettings.PostViewSubtitleElement +
					" class='" +
					displaySettings.PostViewSubtitleClass +
					"'>" +
					blogSubTitle +
					"</" +
					displaySettings.PostViewSubtitleElement +
					">";
			}

			if (CanEditPost(blog))
			{

				string editLink = $"&nbsp;<a href='{SiteRoot}/Blog/EditPost.aspx?pageid={PageId.ToInvariantString()}" +
					$"&amp;mid={ModuleId.ToInvariantString()}" +
					$"&amp;ItemID={ItemId.ToInvariantString()}'" +
					$"class='ModuleEditLink'>{BlogResources.BlogEditEntryLink}</a>";

				if (CacheHelper.GetCurrentPage().ShowPageHeading && config.UsePostTitleAsPageHeading)
				{
					basePage.PageHeading.LiteralExtraMarkup = editLink;
				}
				else
				{
					heading.LiteralExtraMarkup = editLink;
				}
			}


			basePage.Title = SiteUtils.FormatPageTitle(basePage.SiteInfo, blogTitle);
			basePage.MetaDescription = blog.MetaDescription;
			basePage.MetaKeywordCsv = blog.MetaKeywords;
			basePage.AdditionalMetaMarkup = blog.CompiledMeta;
			if (basePage.AnalyticsSection.Length == 0)
			{
				basePage.AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsBlogSection", "blog");
			}

			//addThis1.Visible = !config.HideAddThisButton;
			//addThis1.TitleOfUrlToShare = heading.Text;
			addThisWidget.AccountId = addThisAccountId;
			//addThis1.CustomBrand = addThisCustomBrand;
			//addThis1.UseMouseOverWidget = config.UseAddThisMouseOverWidget;
			//addThis1.ButtonImageUrl = config.AddThisButtonImageUrl;
			//addThis1.CustomLogoBackgroundColor = config.AddThisCustomLogoBackColor;
			//addThis1.CustomLogoColor = config.AddThisCustomLogoForeColor;
			//addThis1.CustomLogoUrl = config.AddThisCustomLogoUrl;
			//addThis1.CustomOptions = config.AddThisCustomOptions;

			divAddThis.Visible = !config.HideAddThisButton;
			addThisWidget.Visible = !config.HideAddThisButton;

			tweetThis1.Visible = config.ShowTweetThisLink;
			tweetThis1.TitleToTweet = blogTitle;
			tweetThis1.UrlToTweet = FormatBlogUrl(blog.ItemUrl, blog.ItemId);


			fblike.Visible = config.UseFacebookLikeButton;
			fblike.ColorScheme = config.FacebookLikeButtonTheme;
			fblike.ShowFaces = config.FacebookLikeButtonShowFaces;
			fblike.HeightInPixels = config.FacebookLikeButtonHeight;
			fblike.WidthInPixels = config.FacebookLikeButtonWidth;
			fblike.UrlToLike = FormatBlogUrl(blog.ItemUrl, blog.ItemId);

			btnPlusOne.Visible = config.ShowPlusOneButton;
			btnPlusOne.TargetUrl = FormatBlogUrl(blog.ItemUrl, blog.ItemId);

			string timeFormat = displaySettings.OverrideDateFormat;
			if (timeFormat.Length == 0) { timeFormat = config.DateTimeFormat; }

			if (timeZone != null)
			{
				litStartDate.Text = TimeZoneInfo.ConvertTimeFromUtc(blog.StartDate, timeZone).ToString(timeFormat);
			}
			else
			{
				litStartDate.Text = blog.StartDate.AddHours(TimeOffset).ToString(timeFormat);
			}
			litStartDateBottom.Text = litStartDate.Text;




			odiogoPlayer.OdiogoFeedId = config.OdiogoFeedId;
			odiogoPlayer.ItemId = blog.ItemId.ToString(CultureInfo.InvariantCulture);
			odiogoPlayer.ItemTitle = blogTitle;

			if (blogAuthor.Length == 0) { blogAuthor = blog.UserName; }


			if (config.BlogAuthor.Length > 0)
			{
				litAuthor.Text = string.Format(CultureInfo.InvariantCulture, BlogResources.PostAuthorFormat, config.BlogAuthor);
			}
			else if ((!string.IsNullOrEmpty(blog.UserFirstName)) && (!string.IsNullOrEmpty(blog.UserLastName)))
			{
				litAuthor.Text = string.Format(CultureInfo.InvariantCulture, BlogResources.PostAuthorFormat, blog.UserFirstName + " " + blog.UserLastName);
			}
			else
			{
				litAuthor.Text = string.Format(CultureInfo.InvariantCulture, BlogResources.PostAuthorFormat, blogAuthor);
			}


			litAuthorBottom.Text = litAuthor.Text;
			litAuthor.Visible = blog.ShowAuthorName;
			litAuthorBottom.Visible = blog.ShowAuthorName;
			litDescription.Text = blog.Description;
			litExcerpt.Text = GetExcerpt(blog);

			if (blog.HeadlineImageUrl != "")
			{
				if (displaySettings.FeaturedImageAbovePost)
				{
					featuredImagePostTop.Visible = true;
					featuredImagePostTop.Text = string.Format(CultureInfo.InvariantCulture, displaySettings.FeaturedImageFormat, ResolveUrl(blog.HeadlineImageUrl), blogTitle);

					featuredImageExcerptTop.Visible = true;
					featuredImageExcerptTop.Text = string.Format(CultureInfo.InvariantCulture, displaySettings.FeaturedImageFormat, ResolveUrl(blog.HeadlineImageUrl), blogTitle);
				}
				else
				{
					featuredImagePostBottom.Visible = true;
					featuredImagePostBottom.Text = string.Format(CultureInfo.InvariantCulture, displaySettings.FeaturedImageFormat, ResolveUrl(blog.HeadlineImageUrl), blogTitle);

					featuredImageExcerptBottom.Visible = true;
					featuredImageExcerptBottom.Text = string.Format(CultureInfo.InvariantCulture, displaySettings.FeaturedImageFormat, ResolveUrl(blog.HeadlineImageUrl), blogTitle);
				}
			}

			if ((blog.PreviousItemId > -1) || (useFriendlyUrls && blog.PreviousPostUrl.Length > 0))
			{
				lnkPreviousPostTop.Visible = true;
				lnkPreviousPost.Visible = true;
				lnkPreviousPostTop.NavigateUrl = FormatBlogUrl(blog.PreviousPostUrl, blog.PreviousItemId);
				lnkPreviousPostTop.ToolTip = blog.PreviousPostTitle;

				lnkPreviousPost.NavigateUrl = FormatBlogUrl(blog.PreviousPostUrl, blog.PreviousItemId);
				lnkPreviousPost.ToolTip = blog.PreviousPostTitle;

			}

			if ((blog.NextItemId > -1) || (useFriendlyUrls && blog.NextPostUrl.Length > 0))
			{
				lnkNextPostTop.Visible = true;
				lnkNextPost.Visible = true;
				lnkNextPostTop.NavigateUrl = FormatBlogUrl(blog.NextPostUrl, blog.NextItemId);
				lnkNextPostTop.ToolTip = blog.NextPostTitle;

				lnkNextPost.NavigateUrl = FormatBlogUrl(blog.NextPostUrl, blog.NextItemId);
				lnkNextPost.ToolTip = blog.NextPostTitle;
			}

			if (blog.Location.Length > 0)
			{
				if (blog.UseBingMap)
				{
					bmap.Visible = true;
					bmap.Location = blog.Location;
					bmap.ShowLocationPin = blog.ShowLocationInfo;
					bmap.Height = Unit.Parse(blog.MapHeight);
					bmap.MapWidth = blog.MapWidth;
					bmap.Zoom = blog.MapZoom;
					bmap.MapStyle = BlogConfiguration.GetBingMapType(blog.MapType);
					bmap.ShowMapControls = blog.ShowMapOptions;

					if (blog.UseDrivingDirections)
					{
						pnlBingDirections.Visible = true;
						bmap.ShowGetDirections = true;
						divDirections.Visible = true;
						bmap.DistanceUnit = BlogConfiguration.BingMapDistanceUnit;
						if (BlogConfiguration.BingMapDistanceUnit == "VERouteDistanceUnit.Kilometer")
						{
							bmap.DistanceUnitLabel = BingResources.Kilometers;
						}
						else
						{
							bmap.DistanceUnitLabel = BingResources.Miles;
						}

						bmap.FromTextBoxClientId = txtFromLocation.ClientID;
						bmap.GetDirectionsButtonClientId = btnGetBingDirections.ClientID;
						bmap.ShowDirectionsPanelClientId = pnlBingDirections.ClientID;
						bmap.ResetSearchLinkUrl = SiteUtils.GetCurrentPageUrl();
						btnGetBingDirections.Text = BingResources.GetDirectionsButton;
						bmap.DistanceLabel = BingResources.Distance;
						bmap.ResetSearchLinkLabel = BingResources.Reset;

					}

				}
				else
				{
					gmap.Visible = true;
					gmap.GMapApiKey = GmapApiKey;
					gmap.Location = blog.Location;
					gmap.EnableMapType = blog.ShowMapOptions;
					gmap.EnableZoom = blog.ShowZoomTool;
					gmap.ShowInfoWindow = blog.ShowLocationInfo;
					gmap.EnableLocalSearch = false; // no longer supported in newer api
					gmap.MapHeight = Convert.ToInt32(blog.MapHeight);
					gmap.MapWidth = blog.MapWidth;
					gmap.EnableDrivingDirections = blog.UseDrivingDirections;
					gmap.GmapType = (mojoPortal.Web.Controls.google.MapType)Enum.Parse(typeof(mojoPortal.Web.Controls.google.MapType), blog.MapType);
					gmap.ZoomLevel = blog.MapZoom;
					gmap.DirectionsButtonText = BlogResources.MapGetDirectionsButton;
					gmap.DirectionsButtonToolTip = BlogResources.MapGetDirectionsButton;
				}
			}



			if (displaySettings.ShowTagsOnPost)
			{
				if (displaySettings.BlogViewUseBottomDate)
				{
					rptBottomCategories.Visible = true;
					rptTopCategories.Visible = false;
					using (IDataReader dataReader = Blog.GetItemCategories(ItemId))
					{
						rptBottomCategories.DataSource = dataReader;
						rptBottomCategories.DataBind();
					}

					rptBottomCategories.Visible = (rptBottomCategories.Items.Count > 0);
				}
				else
				{
					rptBottomCategories.Visible = false;
					rptTopCategories.Visible = true;
					using (IDataReader dataReader = Blog.GetItemCategories(ItemId))
					{
						rptTopCategories.DataSource = dataReader;
						rptTopCategories.DataBind();
					}

					rptTopCategories.Visible = (rptTopCategories.Items.Count > 0);
				}
			}

			List<FileAttachment> attachments = FileAttachment.GetListByItem(blog.BlogGuid);
			rptAttachments.DataSource = attachments;
			rptAttachments.DataBind();
			rptAttachments.Visible = (attachments.Count > 0);
			//if (rptAttachments.Visible)
			//{
			//	basePage.ScriptConfig.IncludeMediaElement = true;
			//	basePage.StyleCombiner.IncludeMediaElement = true;
			//}


			//PopulateNavigation();

			if (Page.Header == null) { return; }

			string canonicalUrl = FormatBlogUrl(blog.ItemUrl, blog.ItemId);
			if (Core.Helpers.WebHelper.IsSecureRequest() && (!basePage.CurrentPage.RequireSsl) && (!basePage.SiteInfo.UseSslOnAllPages))
			{
				if (WebConfigSettings.ForceHttpForCanonicalUrlsThatDontRequireSsl)
				{
					canonicalUrl = canonicalUrl.Replace("https:", "http:");
				}

			}

			Literal link = new Literal();
			link.ID = "blogurl";
			link.Text = "\n<link rel='canonical' href='" + canonicalUrl + "' />";

			Page.Header.Controls.Add(link);

		}

		private bool CanEditPost(Blog blog)
		{
			if (BlogConfiguration.SecurePostsByUser)
			{
				if (WebUser.IsInRoles(config.ApproverRoles)) { return true; }

				if (currentUser == null) { return false; }

				return (blog.UserId == currentUser.UserId);
			}

			return IsEditable;

		}

		#region IRefreshAfterPostback

		public void RefreshAfterPostback()
		{
			PopulateControls();
		}

		#endregion

		public void UpdateCommentStats(Guid contentGuid, int commentCount)
		{
			Blog.UpdateCommentCount(contentGuid, commentCount);
		}



		private void btnPostComment_Click(object sender, EventArgs e)
		{
			if (!ShouldAllowComments())
			{
				WebUtils.SetupRedirect(this, Request.RawUrl);
				return;
			}
			if (!IsValidComment())
			{
				//SetupInternalCommentSystem();
				PopulateControls();
				return;
			}
			if (blog == null) { return; }

			if (blog.AllowCommentsForDays < 0)
			{
				WebUtils.SetupRedirect(this, Request.RawUrl);
				return;
			}

			DateTime endDate = blog.StartDate.AddDays((double)blog.AllowCommentsForDays);

			if ((endDate < DateTime.UtcNow) && (blog.AllowCommentsForDays > 0)) { return; }

			if (this.chkRememberMe.Checked)
			{
				SetCookies();
			}

			Blog.AddBlogComment(
					ModuleId,
					ItemId,
					this.txtName.Text,
					this.txtCommentTitle.Text,
					this.txtURL.Text,
					edComment.Text,
					DateTime.UtcNow);

			if (config.NotifyOnComment)
			{
				//added this 2008-08-07 due to blog coment spam and need to be able to ban the ip of the spammer
				StringBuilder message = new StringBuilder();
				message.Append(basePage.SiteRoot + blog.ItemUrl.Replace("~", string.Empty));

				message.Append("\n\nHTTP_USER_AGENT: " + Page.Request.ServerVariables["HTTP_USER_AGENT"] + "\n");
				message.Append("HTTP_HOST: " + Page.Request.ServerVariables["HTTP_HOST"] + "\n");
				message.Append("REMOTE_HOST: " + Page.Request.ServerVariables["REMOTE_HOST"] + "\n");
				message.Append("REMOTE_ADDR: " + SiteUtils.GetIP4Address() + "\n");
				message.Append("LOCAL_ADDR: " + Page.Request.ServerVariables["LOCAL_ADDR"] + "\n");
				message.Append("HTTP_REFERER: " + Page.Request.ServerVariables["HTTP_REFERER"] + "\n");

				if ((config.NotifyEmail.Length > 0) && (Email.IsValidEmailAddressSyntax(config.NotifyEmail)))
				{

					BlogNotification.SendBlogCommentNotificationEmail(
						SiteUtils.GetSmtpSettings(),
						ResourceHelper.GetMessageTemplate(SiteUtils.GetDefaultUICulture(), "BlogCommentNotificationEmail.config"),
						basePage.SiteInfo.DefaultEmailFromAddress,
						basePage.SiteRoot,
						config.NotifyEmail,
						message.ToString());
				}

				if (config.NotifyEmail != blog.UserEmail)
				{
					BlogNotification.SendBlogCommentNotificationEmail(
							SiteUtils.GetSmtpSettings(),
							ResourceHelper.GetMessageTemplate(SiteUtils.GetDefaultUICulture(), "BlogCommentNotificationEmail.config"),
							basePage.SiteInfo.DefaultEmailFromAddress,
							basePage.SiteRoot,
							blog.UserEmail,
							message.ToString());
				}


			}

			WebUtils.SetupRedirect(this, Request.RawUrl);

		}

		private void PopulateLabels()
		{

			//lnkEdit.ToolTip = BlogResources.BlogEditEntryLink;

			if (BlogConfiguration.UseLegacyCommentSystem)
			{
				edComment.WebEditor.ToolBar = ToolBar.AnonymousUser;
				edComment.WebEditor.Height = Unit.Pixel(170);

				captcha.ProviderName = basePage.SiteInfo.CaptchaProvider;
				captcha.Captcha.ControlID = "captcha" + ModuleId.ToInvariantString();
				//captcha.RecaptchaPrivateKey = basePage.SiteInfo.RecaptchaPrivateKey;
				//captcha.RecaptchaPublicKey = basePage.SiteInfo.RecaptchaPublicKey;


				regexUrl.ErrorMessage = BlogResources.WebSiteUrlRegexWarning;
				commentListHeading.Text = BlogResources.BlogFeedbackLabel;

				btnPostComment.Text = BlogResources.BlogCommentPostCommentButton;
				SiteUtils.SetButtonAccessKey(btnPostComment, BlogResources.BlogCommentPostCommentButtonAccessKey);


				litCommentsClosed.Text = BlogResources.BlogCommentsClosedMessage;
				litCommentsRequireAuthentication.Text = BlogResources.CommentsRequireAuthenticationMessage;
			}

			//addThis1.Text = BlogResources.AddThisButtonAltText;

			lnkPreviousPostTop.Text = Server.HtmlEncode(BlogResources.BlogPreviousPostLink);
			lnkNextPostTop.Text = Server.HtmlEncode(BlogResources.BlogNextPostLink);

			lnkPreviousPost.Text = Server.HtmlEncode(BlogResources.BlogPreviousPostLink);
			lnkNextPost.Text = Server.HtmlEncode(BlogResources.BlogNextPostLink);

			if (displaySettings.OverridePostCategoriesLabel.Length > 0)
			{
				CategoriesResourceKey = displaySettings.OverridePostCategoriesLabel;
			}

			SignInOrRegisterPrompt signinPrompt = srPrompt as SignInOrRegisterPrompt;

			signinPrompt.Instructions = BlogResources.MustSignInToViewFullPost;
			signinPrompt.ShowJanrainWidget = BlogConfiguration.ShowJanrainWidgetForLoginPrompt;




		}

		private void LoadSettings()
		{
			blog = new Blog(ItemId);
			module = basePage.GetModule(ModuleId);
			useFriendlyUrls = BlogConfiguration.UseFriendlyUrls(ModuleId);
			if (!WebConfigSettings.UseUrlReWriting) { useFriendlyUrls = false; }
			attachmentBaseUrl = SiteUtils.GetFileAttachmentUploadPath();
			currentUser = SiteUtils.GetCurrentSiteUser();
			comments = InternalCommentSystem as CommentsWidget;

			if (
				(module.ModuleId == -1)
				|| (blog.ModuleId == -1)
				|| (blog.ModuleId != module.ModuleId)
				|| (basePage.SiteInfo == null)
				)
			{
				// query string params have been manipulated
				pnlInnerWrap.Visible = false;
				AllowComments = false;
				parametersAreInvalid = true;
				return;
			}
			else
			{
				if (Request.IsAuthenticated)
				{
					if (basePage.UserCanEditModule(ModuleId, Blog.FeatureGuid))
					{
						IsEditable = true;
					}
				}
			}

			RegexRelativeImageUrlPatern = SecurityHelper.RegexRelativeImageUrlPatern;

			moduleSettings = ModuleSettings.GetModuleSettings(ModuleId);

			config = new BlogConfiguration(moduleSettings);

			blogAuthor = config.BlogAuthor;

			divTopPager.Visible = config.ShowNextPreviousLinks;
			divBottomPager.Visible = config.ShowNextPreviousLinks;

			GmapApiKey = SiteUtils.GetGmapApiKey();

			if (config.InstanceCssClass.Length > 0)
			{
				pnlInnerWrap.SetOrAppendCss(config.InstanceCssClass);
			}

			searchBoxTop.Visible =
				config.ShowBlogSearchBox &&
				!displaySettings.HideSearchBoxInPostDetail &&
				!displaySettings.ShowSearchInNav
			;

			if (config.Copyright.Length > 0)
			{
				litCopyright.Text = config.Copyright;
				pnlCopyright.Visible = true;
			}

			pnlCopyright.CssClass = displaySettings.CopyrightPanelClass;

			//navTop.ModuleId = ModuleId;
			//navTop.ModuleGuid = module.ModuleGuid;
			//navTop.PageId = PageId;
			//navTop.IsEditable = IsEditable;
			//navTop.Config = config;
			//navTop.SiteRoot = SiteRoot;
			//navTop.ImageSiteRoot = ImageSiteRoot;
			//navTop.OverrideDate = blog.StartDate;
			//navTop.ShowCalendar = config.ShowCalendarOnPostDetail;

			//navBottom.ModuleId = ModuleId;
			//navBottom.ModuleGuid = module.ModuleGuid;
			//navBottom.PageId = PageId;
			//navBottom.IsEditable = IsEditable;
			//navBottom.Config = config;
			//navBottom.SiteRoot = SiteRoot;
			//navBottom.ImageSiteRoot = ImageSiteRoot;
			//navBottom.OverrideDate = blog.StartDate;
			//navBottom.ShowCalendar = config.ShowCalendarOnPostDetail;


			//if (!config.NavigationOnRight)
			//{
			//	divBlog.CssClass = "blogcenter-leftnav";
			//}

			//navTop.Visible = false;

			//if (config.ShowArchives
			//	|| config.ShowAddFeedLinks || displaySettings.ShowArchivesInPostDetail
			//	|| config.ShowCategories || displaySettings.ShowCategoriesInPostDetail
			//	|| config.ShowFeedLinks || displaySettings.ShowFeedLinksInPostDetail
			//	|| config.ShowStatistics || displaySettings.ShowStatisticsInPostDetail
			//	|| (config.UpperSidebar.Length > 0)
			//	|| (config.LowerSidebar.Length > 0)
			//	)
			//{
			//	//divNav.Visible = true;
			//	navTop.Visible = true;
			//}

			//if (!navTop.Visible)
			//{
			//	divBlog.CssClass = "blogcenter-nonav";
			//}


			//navBottom.Visible = false;

			//if ((navTop.Visible) && (displaySettings.UseBottomNavigation))
			//{
			//	navTop.Visible = false;
			//	navBottom.Visible = true;
			//}


			if (!Request.IsAuthenticated)
			{
				//if ((config.HideDetailsFromUnauthencticated) && (blog.Description.Length > config.ExcerptLength))
				if (config.HideDetailsFromUnauthencticated)
				{
					pnlDetails.Visible = false;
					pnlExcerpt.Visible = true;
					AllowComments = false;
					divAddThis.Visible = false;
					tweetThis1.Visible = false;
					fblike.Visible = false;
					btnPlusOne.Visible = false;
					bsocial.Visible = false;
				}
			}

			if (!pnlExcerpt.Visible)
			{
				if (config.AddThisAccountId.Length > 0)
				{
					addThisAccountId = config.AddThisAccountId;
				}
				else
				{
					addThisAccountId = basePage.SiteInfo.AddThisDotComUsername;
				}
			}

			pnlDateTop.RenderId = false;
			pnlDateTop.CssClass = displaySettings.DatePanelClass;

			if (displaySettings.DateBottomPanelClass != "")
			{
				pnlDateTop.CssClass += " " + displaySettings.DateTopPanelClass;
			}

			pnlDateBottom.RenderId = false;
			pnlDateBottom.CssClass = displaySettings.DatePanelClass;

			if (displaySettings.DateBottomPanelClass != "")
			{
				pnlDateBottom.CssClass += " " + displaySettings.DateBottomPanelClass;
			}

			if (!displaySettings.BlogViewUseBottomDate)
			{
				pnlDateTop.Visible = true;
				pnlDateBottom.Visible = false;
			}
			else
			{
				pnlDateTop.Visible = false;
				pnlDateBottom.Visible = true;
			}

			if (displaySettings.BlogViewHideTopPager)
			{
				divTopPager.Visible = false;
			}

			if (displaySettings.BlogViewHideBottomPager)
			{
				divBottomPager.Visible = false;
			}

			if (displaySettings.BlogViewInnerWrapElement.Length > 0)
			{
				pnlInnerWrap.Element = displaySettings.BlogViewInnerWrapElement;
			}

			if (displaySettings.BlogViewInnerBodyExtraCss.Length > 0)
			{
				pnlInnerBody.ExtraCssClasses = displaySettings.BlogViewInnerBodyExtraCss;
				pnlInnerBody.RenderContentsOnly = false;
			}

			if (displaySettings.BlogViewHeaderLiteralTopContent.Length > 0)
			{
				heading.LiteralExtraTopContent = displaySettings.BlogViewHeaderLiteralTopContent;
			}

			if (displaySettings.BlogViewHeaderLiteralBottomContent.Length > 0)
			{
				heading.LiteralExtraBottomContent = displaySettings.BlogViewHeaderLiteralBottomContent;
			}

			if (displaySettings.OverridePostDetailHeadingElement.Length > 0)
			{
				heading.HeadingTag = displaySettings.OverridePostDetailHeadingElement;
			}

			if ((config.RelatedItemsToShow > 0) && (displaySettings.RelatedPostsPosition == "Bottom") && (Page is BlogView))
			{
				relatedPosts.PageId = PageId;
				relatedPosts.ModuleId = ModuleId;
				relatedPosts.ItemId = ItemId;
				relatedPosts.SiteRoot = SiteRoot;
				relatedPosts.MaxItems = config.RelatedItemsToShow;
				relatedPosts.UseFriendlyUrls = BlogConfiguration.UseFriendlyUrls(ModuleId);
				relatedPosts.HeadingElement = displaySettings.RelatedPostsHeadingElement;
				relatedPosts.OverrideHeadingText = displaySettings.RelatedPostsOverrideHeadingText;

			}

			switch (basePage.SiteInfo.AvatarSystem)
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


			if (!pnlExcerpt.Visible) {
				pnlDetails.RenderContentsOnly = !displaySettings.PostViewRenderPostPanel;
			}
			else
			{
				pnlExcerpt.RenderContentsOnly = !displaySettings.PostViewRenderPostPanel;
			}

			pnlDetails.RenderId = false;
			pnlExcerpt.RenderId = false;

			pnlBlogText.CssClass = displaySettings.PostViewPostBodyClass;
			pnlBlogText.RenderId = false;

			pnlBlogTextExpt.CssClass = displaySettings.PostViewPostBodyClass;
			pnlBlogTextExpt.RenderId = false;

			if (!blog.ShowAuthorAvatar) { disableAvatars = true; }
			if (displaySettings.HideAvatarInPostDetail) { disableAvatars = true; }

			pnlAuthor.Visible = !disableAvatars || (blog.ShowAuthorBio && displaySettings.ShowAuthorBioInPostDetail && !pnlExcerpt.Visible);
			pnlAuthor.CssClass = displaySettings.AuthorInfoPanelClass;

			lblAuthorBio.Visible = blog.ShowAuthorBio && displaySettings.ShowAuthorBioInPostDetail && !string.IsNullOrWhiteSpace(blog.AuthorBio);
			lblAuthorBio.Text = blog.AuthorBio;
			lblAuthorBio.CssClass = displaySettings.AuthorBioClass;

			av1.Email = blog.UserEmail;
			av1.UserName = blog.UserName;
			av1.UserId = blog.UserId;
			av1.AvatarFile = blog.UserAvatar;
			av1.MaxAllowedRating = MaxAllowedGravatarRating;
			av1.Disable = disableAvatars;
			av1.UseGravatar = allowGravatars;
			av1.SiteId = basePage.SiteInfo.SiteId;
			av1.UserNameTooltipFormat = displaySettings.AvatarUserNameTooltipFormat;
			av1.UseLink = UseProfileLinkForAvatar();
			av1.SiteRoot = SiteRoot;
			av1.CssClass = displaySettings.AvatarCssClass;
			av1.ExtraCssClass = displaySettings.AvatarExtraCssClass;

			if (
				disableAvatars ||
				displaySettings.HideAvatarInPostList ||
				!Convert.ToBoolean(blog.ShowAuthorAvatar)
			)
			{
				av1.Disable = true;
			}

			// if (pnlExcerpt.Visible) { userAvatar.Visible = false; }

			// Setup Comment before setting up Nav, to set commentCount
			SetupCommentSystem();

			Control cNav = Page.LoadControl("~/Blog/Controls/BlogNav.ascx");

			BlogNav nav = (BlogNav)cNav;

			nav.ModuleId = ModuleId;
			nav.ModuleGuid = module.ModuleGuid;
			nav.PageId = PageId;
			nav.IsEditable = IsEditable;
			nav.Config = config;
			nav.SiteRoot = SiteRoot;
			nav.ImageSiteRoot = ImageSiteRoot;
			nav.OverrideDate = blog.StartDate;
			nav.ShowCalendar = config.ShowCalendarOnPostDetail;
			nav.ShowCommentCount = showCommentCountInNav;

			bool showNav = false;

			if (
				config.ShowArchives ||
				config.ShowAddFeedLinks ||
				displaySettings.ShowArchivesInPostDetail ||
				config.ShowCategories ||
				displaySettings.ShowCategoriesInPostDetail ||
				config.ShowFeedLinks ||
				displaySettings.ShowFeedLinksInPostDetail ||
				config.ShowStatistics ||
				displaySettings.ShowStatisticsInPostDetail ||
				!string.IsNullOrWhiteSpace(config.UpperSidebar) ||
				!string.IsNullOrWhiteSpace(config.LowerSidebar)
			)
			{
				showNav = true;
			}

			divBlog.CssClass = displaySettings.PostViewCenterClass;

			if (showNav)
			{
				if (config.NavigationOnRight)
				{
					phNavRight.Controls.Add(nav);
					divBlog.CssClass += " " + displaySettings.PostViewCenterRightNavClass;
				}
				else
				{
					phNavLeft.Controls.Add(nav);
					divBlog.CssClass += " " + displaySettings.PostViewCenterLeftNavClass;
				}
			}
			else
			{
				divBlog.CssClass += " " + displaySettings.PostViewCenterNoNavClass;
			}

			if (displaySettings.BlogViewDivBlogExtraCss.Length > 0)
			{
				divBlog.CssClass += " " + displaySettings.BlogViewDivBlogExtraCss;
			}
		}

		protected bool UseProfileLinkForAvatar()
		{
			if (!displaySettings.LinkAuthorAvatarToProfile) { return false; }

			if (Request.IsAuthenticated)
			{
				// if we know the user is signed in and not in a role allowed then return username without a profile link
				if (!WebUser.IsInRoles(basePage.SiteInfo.RolesThatCanViewMemberList))
				{
					return false;
				}
			}

			// if user is not authenticated we don't know if he will be allowed but he will be prompted to login first so its ok to show the link
			return true;
		}


		private void SetupCommentSystem()
		{

			if (!ShouldAllowComments())
			{
				pnlNewComment.Visible = false;
				pnlCommentsClosed.Visible = true;
				//divCommentService.Visible = false;

				pnlAntiSpam.Visible = false;
				captcha.Visible = false;
				captcha.Enabled = false;
				//comments.Visible = false;

				return;
			}

			string commentSystem = DetermineCommentSystem();

			comments.Visible = true;
			comments.CommentUrl = FormatBlogUrl(blog.ItemUrl, blog.ItemId);

			switch (commentSystem)
			{
				case "disqus":
					if (config.DisqusSiteShortName.Length > 0)
					{
						DisqusSiteShortName = config.DisqusSiteShortName;
					}
					else
					{
						DisqusSiteShortName = basePage.SiteInfo.DisqusSiteShortName;
					}

					comments.CommentSystem = commentSystem;
					comments.DisqusShortName = DisqusSiteShortName;

					showCommentCountInNav = false;

					//navTop.ShowCommentCount = false;
					//navBottom.ShowCommentCount = false;

					DisableLegacyBlogComments();

					break;

				case "intensedebate":

					if (config.IntenseDebateAccountId.Length > 0)
					{
						IntenseDebateAccountId = config.IntenseDebateAccountId;
					}
					else
					{
						IntenseDebateAccountId = basePage.SiteInfo.IntenseDebateAccountId;
					}


					comments.IntenseDebateAccountId = IntenseDebateAccountId;
					comments.CommentSystem = commentSystem;

					showCommentCountInNav = false;

					//navTop.ShowCommentCount = false;
					//navBottom.ShowCommentCount = false;

					DisableLegacyBlogComments();

					break;

				case "facebook":

					comments.CommentSystem = commentSystem;

					showCommentCountInNav = false;

					//navTop.ShowCommentCount = false;
					//navBottom.ShowCommentCount = false;

					DisableLegacyBlogComments();

					break;

				case "internal":
				default:
					if (BlogConfiguration.UseLegacyCommentSystem)
					{
						SetupLegacyBlogComments();
					}
					else
					{
						DisableLegacyBlogComments();

						SetupInternalCommentSystem();
					}

					break;
			}




		}

		private bool ShouldAllowComments()
		{
			//comments closed globally
			if (!config.AllowComments) { return false; }

			// comments not allowed on this post
			if (blog.AllowCommentsForDays < 0) { return false; }

			if (pnlExcerpt.Visible) { return false; } // should not be able to comment without reading the full article

			return true;
		}

		private bool CommentsAreOpen()
		{
			//comments closed globally
			if (!config.AllowComments) { return false; }

			// comments not allowed on this post
			if (blog.AllowCommentsForDays < 0) { return false; }

			//no limit on comments for this post
			if (blog.AllowCommentsForDays == 0) { return true; }

			if (blog.AllowCommentsForDays > 0) //limited to a specific number of days
			{
				DateTime endDate = blog.StartDate.AddDays((double)blog.AllowCommentsForDays);

				if (endDate > DateTime.UtcNow) { return true; }

			}

			return false;
		}

		private string DetermineCommentSystem()
		{
			// don't use new external comment system for existing posts that already have comments
			if (blog.CommentCount > 0) { return "internal"; }

			return config.CommentSystem;
		}



		private void DisableLegacyBlogComments()
		{
			pnlAntiSpam.Visible = false;
			captcha.Visible = false;
			captcha.Enabled = false;
			pnlFeedback.Visible = false;
		}

		private void SetupInternalCommentSystem()
		{
			basePage.ScriptConfig.IncludeColorBox = true;

			//CommentsWidget comments = InternalCommentSystem as CommentsWidget;

			comments.SiteGuid = basePage.SiteInfo.SiteGuid;
			comments.FeatureGuid = Blog.FeatureGuid;
			comments.ModuleGuid = module.ModuleGuid;
			comments.ContentGuid = blog.BlogGuid;
			comments.CommentItemHeaderFormat = displaySettings.CommentItemHeaderFormat;
			comments.CommentDateTimeFormat = config.DateTimeFormat;
			comments.CommentsClosed = !CommentsAreOpen();
			comments.CommentsClosedMessage = BlogResources.BlogCommentsClosedMessage;
			comments.AlwaysShowSignInPromptIfNotAuthenticated = displaySettings.AlwaysShowSignInPromptForCommentsIfNotAuthenticated;
			comments.ShowJanrainWidetOnSignInPrompt = displaySettings.ShowJanrainWidetOnCommentSignInPrompt;

			comments.DefaultCommentTitle = "re: " + blogTitle;
			comments.IncludeIpAddressInNotification = true;
			comments.RequireCaptcha = config.UseCaptcha && !Request.IsAuthenticated;
			comments.ContainerControl = this;
			comments.UpdateContainerControl = this;
			comments.EditBaseUrl = SiteRoot + "/Blog/CommentDialog.aspx?pageid=" + PageId.ToInvariantString()
				+ "&mid=" + ModuleId.ToInvariantString() + "&ItemID=" + ItemId.ToInvariantString();

			if (config.NotifyOnComment)
			{

				if ((config.NotifyEmail.Length > 0) && (Email.IsValidEmailAddressSyntax(config.NotifyEmail)))
				{
					comments.NotificationAddresses.Add(config.NotifyEmail);
				}

				//if post author email is not the same as default notification email, add it 
				if (config.NotifyEmail != blog.UserEmail)
				{
					comments.NotificationAddresses.Add(blog.UserEmail);
				}

			}

			comments.NotificationTemplateName = "BlogCommentNotificationEmail.config";
			comments.SiteRoot = SiteRoot;
			comments.UserCanModerate = IsEditable;
			comments.AllowedEditMinutesForUnModeratedPosts = config.AllowedEditMinutesForUnModeratedPosts;
			comments.Visible = true;
			comments.RequireModeration = config.RequireApprovalForComments;
			comments.RequireAuthenticationToPost = config.RequireAuthenticationForComments;
			comments.AuthenticationRequiredMessage = BlogResources.CommentsRequireAuthenticationMessage;
			if (!config.RequireAuthenticationForComments && displaySettings.AlwaysShowSignInPromptForCommentsIfNotAuthenticated)
			{
				comments.AuthenticationRequiredMessage = " ";
			}
			comments.UseCommentTitle = config.AllowCommentTitle;
			comments.ShowUserUrl = config.AllowWebSiteUrlForComments;
			comments.SortDescending = config.SortCommentsDescending;



		}

		private void SetupLegacyBlogComments()
		{
			pnlFeedback.Visible = true;
			fldEnterComments.Visible = CommentsAreOpen();
			pnlCommentsClosed.Visible = !fldEnterComments.Visible;

			if ((!config.UseCaptcha) || (!fldEnterComments.Visible) || (Request.IsAuthenticated))
			{
				pnlAntiSpam.Visible = false;
				captcha.Visible = false;
				captcha.Enabled = false;
			}

			CommentDateTimeFormat = config.DateTimeFormat;

			divCommentUrl.Visible = config.AllowWebSiteUrlForComments;

			//CommentItemHeaderElement = displaySettings.CommentItemHeaderElement;

			if ((config.RequireAuthenticationForComments) && (!Request.IsAuthenticated))
			{
				//AllowComments = false;
				pnlNewComment.Visible = false;
				pnlCommentsRequireAuthentication.Visible = true;
			}

			if (!IsPostBack)
			{
				txtCommentTitle.Text = "re: " + blogTitle;

				if (Request.IsAuthenticated)
				{
					SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
					this.txtName.Text = currentUser.Name;
					txtURL.Text = currentUser.WebSiteUrl;
				}
				else
				{
					if (CookieHelper.CookieExists("blogUser"))
					{
						this.txtName.Text = CookieHelper.GetCookieValue("blogUser");
					}
					if (CookieHelper.CookieExists("blogUrl"))
					{
						this.txtURL.Text = CookieHelper.GetCookieValue("blogUrl");
					}
				}
			}

			using (IDataReader dataReader = Blog.GetBlogComments(ModuleId, ItemId))
			{
				dlComments.DataSource = dataReader;
				dlComments.DataBind();
			}
		}



		private string GetExcerpt(Blog blog)
		{
			if ((blog.Excerpt.Length > 0) && (blog.Excerpt != "<p>&#160;</p>"))
			{
				return blog.Excerpt;
			}

			string result = string.Empty;
			if ((blog.Description.Length > config.ExcerptLength))
			{

				return UIHelper.CreateExcerpt(blog.Description, config.ExcerptLength, config.ExcerptSuffix);

			}

			return blog.Description;

		}

		protected string FormatCommentDate(DateTime startDate)
		{
			if (timeZone != null)
			{
				return TimeZoneInfo.ConvertTimeFromUtc(startDate, timeZone).ToString(CommentDateTimeFormat);

			}

			return startDate.AddHours(TimeOffset).ToString(config.DateTimeFormat);

		}

		protected string FormatBlogUrl(string itemUrl, int itemId)
		{
			if (useFriendlyUrls && (itemUrl.Length > 0))
				return SiteRoot + itemUrl.Replace("~", string.Empty);

			return SiteRoot + "/Blog/ViewPost.aspx?pageid=" + PageId.ToInvariantString()
				+ "&ItemID=" + itemId.ToInvariantString()
				+ "&mid=" + ModuleId.ToInvariantString();

		}




		private bool IsValidComment()
		{
			if (parametersAreInvalid) { return false; }

			//if (!AllowComments) { return false; }

			//if ((config.CommentSystem != "internal") && (blog.CommentCount == 0)) { return false; }

			if (edComment.Text.Length == 0) { return false; }
			if (edComment.Text == "<p>&#160;</p>") { return false; }

			bool result = true;

			try
			{

				Page.Validate("blogcomments");
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


			try
			{
				//if ((result) && (config.UseCaptcha))
				if ((config.UseCaptcha) && (pnlAntiSpam.Visible))
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


			return result;
		}

		/// <summary>
		/// Handles the item command
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		protected void dlComments_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "DeleteComment")
			{
				Blog.DeleteBlogComment(int.Parse(e.CommandArgument.ToString()));
				WebUtils.SetupRedirect(this, Request.RawUrl);

			}
		}


		void dlComments_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
			UIHelper.AddConfirmationDialog(btnDelete, BlogResources.BlogDeleteCommentWarning);
		}


		private void SetCookies()
		{
			HttpCookie blogUserCookie = new HttpCookie("blogUser", this.txtName.Text);
			blogUserCookie.Expires = DateTime.Now.AddMonths(1);
			Response.Cookies.Add(blogUserCookie);

			HttpCookie blogUrlCookie = new HttpCookie("LinkUrl", this.txtURL.Text);
			blogUrlCookie.Expires = DateTime.Now.AddMonths(1);
			Response.Cookies.Add(blogUrlCookie);
		}


		protected virtual void SetupRssLink()
		{
			if (WebConfigSettings.DisableBlogRssMetaLink) { return; }
			if (config.FeedIsDisabled) { return; }
			if (!config.AddFeedDiscoveryLink) { return; }

			if (module != null)
			{
				if (Page.Master != null)
				{
					Control head = Page.Master.FindControl("Head1");
					if (head != null)
					{

						Literal rssLink = new Literal();
						rssLink.Text = "<link rel=\"alternate\" type=\"application/rss+xml\" title=\""
								+ module.ModuleTitle + "\" href=\""
								+ GetRssUrl() + "\" />";

						head.Controls.Add(rssLink);
					}
				}
			}
		}

		private string GetRssUrl()
		{
			if (
				 (config.FeedburnerFeedUrl.Length > 0)
				&& (!BlogConfiguration.UseRedirectForFeedburner)
				)
			{ return config.FeedburnerFeedUrl; }

			if (config.FeedburnerFeedUrl.Length > 0)
			{
				return SiteRoot + "/Blog/RSS.aspx?p=" + PageId.ToInvariantString()
					+ "~" + ModuleId.ToInvariantString() + "~-1"
					+ "&amp;r=" + Global.FeedRedirectBypassToken.ToString();
			}

			return SiteRoot + "/Blog/RSS.aspx?p=" + PageId.ToInvariantString()
					+ "~" + ModuleId.ToInvariantString() + "~-1"
					;

		}

		//private string GetRssUrl()
		//{
		//    if ((config.FeedburnerFeedUrl.Length > 0)&&(!BlogConfiguration.UseRedirectForFeedburner)) { return config.FeedburnerFeedUrl; }

		//    return SiteRoot + "/blog" + ModuleId.ToInvariantString() + "rss.aspx";

		//}

		private void LoadParams()
		{
			virtualRoot = WebUtils.GetApplicationRoot();
			TimeOffset = SiteUtils.GetUserTimeOffset();
			timeZone = SiteUtils.GetUserTimeZone();
			PageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
			ModuleId = WebUtils.ParseInt32FromQueryString("mid", -1);
			ItemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);

			if (PageId == -1) parametersAreInvalid = true;
			if (ModuleId == -1) parametersAreInvalid = true;
			if (ItemId == -1) parametersAreInvalid = true;
			if (!basePage.UserCanViewPage(ModuleId)) { parametersAreInvalid = true; }

			addThisAccountId = basePage.SiteInfo.AddThisDotComUsername;
			addThisCustomBrand = basePage.SiteInfo.SiteName;


		}

		//protected override void Render(HtmlTextWriter writer)
		//{
		//    if ((Page.IsPostBack) &&(!pnlFeedback.Visible))
		//    { 
		//        WebUtils.SetupRedirect(this, Request.RawUrl); 
		//        return; 
		//    }

		//    base.Render(writer);
		//}

	}
}