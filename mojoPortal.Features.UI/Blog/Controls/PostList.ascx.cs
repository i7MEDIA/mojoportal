using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Web.BlogUI;

public partial class PostList : UserControl
{
	#region Properties

	private static readonly ILog log = LogManager.GetLogger(typeof(PostList));

	//private int countOfDrafts = 0;
	private int pageNumber = 1;
	private int totalPages = 1;
	protected string EditContentImage = WebConfigSettings.EditContentImage;
	protected string EditBlogAltText = "Edit";
	protected Double TimeOffset = 0;
	private TimeZoneInfo timeZone = null;
	protected DateTime CalendarDate;
	protected bool ShowGoogleMap = true;
	protected string FeedBackLabel = string.Empty;
	protected string GmapApiKey = string.Empty;
	protected bool EnableContentRating = false;
	private string DisqusSiteShortName = string.Empty;
	protected string disqusFlag = string.Empty;
	protected string IntenseDebateAccountId = string.Empty;
	protected bool ShowCommentCounts = true;
	protected string EditLinkText = BlogResources.BlogEditEntryLink;
	protected string EditLinkTooltip = BlogResources.BlogEditEntryLink;
	protected string EditLinkImageUrl = string.Empty;
	private mojoBasePage basePage = null;
	private Module module = null;
	private bool useFriendlyUrls = true;
	private int categoryId = -1;
	private SiteSettings siteSettings = null;
	private int pageSize = 10;
	private DataSet dsBlogPosts = null;
	protected string itemHeadingElement = "h3";
	protected string CategoriesResourceKey = "PostCategories";
	protected int Month = DateTime.UtcNow.Month;
	protected int Year = DateTime.UtcNow.Year;
	protected bool TitleOnly = false;
	protected bool ShowTweetThisLink = false;
	protected bool UseFacebookLikeButton = false;
	protected bool AllowComments = false;
	protected bool useExcerpt = false;
	protected string attachmentBaseUrl = string.Empty;
	protected bool allowGravatars = false;
	protected bool disableAvatars = true;
	protected Avatar.RatingType MaxAllowedGravatarRating = Avatar.RatingType.PG;
	protected string UserNameTooltipFormat = "View User Profile for {0}";

	private SiteUser currentUser = null;
	public int SiteId { get; private set; } = -1;
	public int PageId { get; set; } = -1;
	public int ModuleId { get; set; } = -1;
	public string SiteRoot { get; set; } = string.Empty;
	public string ImageSiteRoot { get; set; } = string.Empty;
	public BlogConfiguration Config { get; set; } = new BlogConfiguration();
	public bool IsEditable { get; set; } = false;
	public string DisplayMode { get; set; } = "DescendingByDate";
	public bool ShowFeaturedPost { get; set; } = true;

	#endregion


	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		EnableViewState = false;
		rptBlogs.ItemDataBound += new RepeaterItemEventHandler(rptBlogs_ItemDataBound);
	}


	protected virtual void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();

		if (module is null)
		{
			Visible = false;
			return;
		}

		SetupRssLink();

		PopulateLabels();

		if (!Page.IsPostBack)
		{
			PopulateControls();
		}
	}


	private void PopulateControls()
	{
		BindBlogs();
	}


	private void BindBlogs()
	{
		string pageUrl;

		var commonUrlParams = new Dictionary<string, object>
		{
			{ "pageid", PageId },
			{ "moduleid", ModuleId },
			{ "pagenumber", "{0}" }
		};

		switch (DisplayMode)
		{
			case "ByCategory":
				dsBlogPosts = Blog.GetBlogEntriesByCategory(
					ModuleId,
					categoryId,
					DateTime.UtcNow,
					pageNumber,
					pageSize,
					out totalPages
				);

				pageUrl = "Blog/ViewCategory.aspx".ToLinkBuilder().AddParams(commonUrlParams).ToString();
				break;

			case "ByMonth":
				dsBlogPosts = Blog.GetBlogEntriesByMonth(
					Month,
					Year,
					ModuleId,
					DateTime.UtcNow,
					pageNumber,
					pageSize,
					out totalPages
				);

				pageUrl = "Blog/ViewArchive.aspx".ToLinkBuilder().AddParams(commonUrlParams).AddParam("month", Month).AddParam("year", Year).ToString();
				break;

			case "DescendingByDate":
			default:
				dsBlogPosts = Blog.GetPageDataSet(
					ModuleId,
					CalendarDate.Date.AddDays(1),
					pageNumber,
					pageSize,
					out totalPages
				);

				pageUrl = "Blog/ViewList.aspx".ToLinkBuilder().AddParams(commonUrlParams).ToString();
				break;

		}
		if (ShowFeaturedPost)
		{
			DataRow featuredRow = dsBlogPosts.Tables["Posts"].NewRow();

			if (Config.FeaturedPostId != 0 && pageNumber == 1)
			{
				DateTime endDate = DateTime.MaxValue;
				using IDataReader reader = Blog.GetSingleBlog(Config.FeaturedPostId);
				while (reader.Read())
				{
					featuredRow["ItemID"] = Convert.ToInt32(reader["ItemID"]);
					featuredRow["ModuleID"] = Convert.ToInt32(reader["ModuleID"]);
					featuredRow["BlogGuid"] = reader["BlogGuid"].ToString();
					featuredRow["CreatedDate"] = Convert.ToDateTime(reader["CreatedDate"]);
					featuredRow["Heading"] = reader["Heading"];
					featuredRow["SubTitle"] = reader["SubTitle"];
					featuredRow["StartDate"] = Convert.ToDateTime(reader["StartDate"]);

					if (reader["EndDate"] != DBNull.Value)
					{
						featuredRow["EndDate"] = Convert.ToDateTime(reader["EndDate"]);
						endDate = Convert.ToDateTime(reader["EndDate"]);
					}
					else
					{
						featuredRow["EndDate"] = endDate;
					}

					featuredRow["Description"] = reader["Description"];
					featuredRow["Abstract"] = reader["Abstract"];
					featuredRow["ItemUrl"] = reader["ItemUrl"];
					featuredRow["Location"] = reader["Location"];
					featuredRow["MetaKeywords"] = reader["MetaKeywords"];
					featuredRow["MetaDescription"] = reader["MetaDescription"];
					featuredRow["LastModUtc"] = Convert.ToDateTime(reader["LastModUtc"]);
					featuredRow["IsPublished"] = Convert.ToBoolean(reader["IsPublished"]);
					featuredRow["IncludeInFeed"] = Convert.ToBoolean(reader["IncludeInFeed"]);
					featuredRow["CommentCount"] = Convert.ToInt32(reader["CommentCount"]);
					featuredRow["UserID"] = Convert.ToInt32(reader["UserID"]);
					featuredRow["Name"] = reader["Name"];
					featuredRow["FirstName"] = reader["FirstName"];
					featuredRow["LastName"] = reader["LastName"];
					featuredRow["LoginName"] = reader["LoginName"];
					featuredRow["Email"] = reader["Email"];
					featuredRow["AvatarUrl"] = reader["AvatarUrl"];
					featuredRow["AuthorBio"] = reader["AuthorBio"];

					if (reader["ShowAuthorName"] != DBNull.Value)
					{
						featuredRow["ShowAuthorName"] = Convert.ToBoolean(reader["ShowAuthorName"]);
					}
					else
					{
						featuredRow["ShowAuthorName"] = true;
					}

					if (reader["ShowAuthorAvatar"] != DBNull.Value)
					{
						featuredRow["ShowAuthorAvatar"] = Convert.ToBoolean(reader["ShowAuthorAvatar"]);
					}
					else
					{
						featuredRow["ShowAuthorAvatar"] = true;
					}

					if (reader["ShowAuthorBio"] != DBNull.Value)
					{
						featuredRow["ShowAuthorBio"] = Convert.ToBoolean(reader["ShowAuthorBio"]);
					}
					else
					{
						featuredRow["ShowAuthorBio"] = true;
					}

					if (reader["UseBingMap"] != DBNull.Value)
					{
						featuredRow["UseBingMap"] = Convert.ToBoolean(reader["UseBingMap"]);
					}
					else
					{
						featuredRow["UseBingMap"] = false;
					}

					featuredRow["MapHeight"] = reader["MapHeight"];
					featuredRow["MapWidth"] = reader["MapWidth"];
					featuredRow["MapType"] = reader["MapType"];

					if (reader["MapZoom"] != DBNull.Value)
					{
						featuredRow["MapZoom"] = Convert.ToInt32(reader["MapZoom"]);
					}
					else
					{
						featuredRow["MapZoom"] = 13;
					}

					if (reader["ShowMapOptions"] != DBNull.Value)
					{
						featuredRow["ShowMapOptions"] = Convert.ToBoolean(reader["ShowMapOptions"]);
					}
					else
					{
						featuredRow["ShowMapOptions"] = false;
					}

					if (reader["ShowZoomTool"] != DBNull.Value)
					{
						featuredRow["ShowZoomTool"] = Convert.ToBoolean(reader["ShowZoomTool"]);
					}
					else
					{
						featuredRow["ShowZoomTool"] = false;
					}

					if (reader["ShowLocationInfo"] != DBNull.Value)
					{
						featuredRow["ShowLocationInfo"] = Convert.ToBoolean(reader["ShowLocationInfo"]);
					}
					else
					{
						featuredRow["ShowLocationInfo"] = false;
					}

					if (reader["UseDrivingDirections"] != DBNull.Value)
					{
						featuredRow["UseDrivingDirections"] = Convert.ToBoolean(reader["UseDrivingDirections"]);
					}
					else
					{
						featuredRow["UseDrivingDirections"] = false;
					}

					if (reader["ShowDownloadLink"] != DBNull.Value)
					{
						featuredRow["ShowDownloadLink"] = Convert.ToBoolean(reader["ShowDownloadLink"]);
					}
					else
					{
						featuredRow["ShowDownloadLink"] = false;
					}

					featuredRow["HeadlineImageUrl"] = reader["HeadlineImageUrl"];

					if (reader["IncludeImageInExcerpt"] != DBNull.Value)
					{
						featuredRow["IncludeImageInExcerpt"] = Convert.ToBoolean(reader["IncludeImageInExcerpt"]);
					}
					else
					{
						featuredRow["IncludeImageInExcerpt"] = true;
					}

					if (reader["IncludeImageInPost"] != DBNull.Value)
					{
						featuredRow["IncludeImageInPost"] = Convert.ToBoolean(reader["IncludeImageInPost"]);
					}
					else
					{
						featuredRow["IncludeImageInPost"] = true;
					}
				}
				//we don't want the featured post if it's not published
				if ((bool)featuredRow["IsPublished"] && (DateTime)featuredRow["StartDate"] <= DateTime.UtcNow && endDate > DateTime.UtcNow)
				{
					//look for featured post in datable
					DataRow found = dsBlogPosts.Tables["Posts"].Rows.Find(Config.FeaturedPostId);

					if (found != null)
					{
						//remove featured post from datatable so we can insert it at the top if we're on "page" number 1
						dsBlogPosts.Tables["Posts"].Rows.Remove(found);
					}

					//insert the featured post into the datatable at the top
					//we only want to do this if the current "page" is number 1, don't want the featured post on other pages.
					dsBlogPosts.Tables["Posts"].Rows.InsertAt(featuredRow, 0);
				}
			}
		}

		rptBlogs.DataSource = dsBlogPosts.Tables["Posts"];
		rptBlogs.DataBind();

		pgr.PageURLFormat = pageUrl;
		pgr.ShowFirstLast = true;
		pgr.PageSize = Config.PageSize;
		pgr.PageCount = totalPages;
		pgr.CurrentIndex = pageNumber;
		pgr.Visible = (totalPages > 1) && Config.ShowPager;
	}


	void rptBlogs_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (dsBlogPosts is null)
		{
			return;
		}

		bool showAuthorName = Convert.ToBoolean(((DataRowView)e.Item.DataItem).Row["ShowAuthorName"]);

		BlogPostListItemPanel postItem = (BlogPostListItemPanel)e.Item.FindControl("bi1");
		postItem.CssClass = displaySettings.ListViewPostClass;

		if (Config.FeaturedPostId == Convert.ToInt32(((DataRowView)e.Item.DataItem).Row.ItemArray[2]))
		{
			postItem.CssClass = $"{displaySettings.ListViewPostClass} {displaySettings.FeaturedPostClass}";
		}

		HyperLink postLink = (HyperLink)e.Item.FindControl("lnkTitle");
		postLink.CssClass = displaySettings.ListViewPostLinkClass;

		BasePanel postBody = (BasePanel)e.Item.FindControl("pnlBlogText");
		postBody.CssClass = displaySettings.ListViewPostBodyClass;
		postBody.RenderId = false;

		BasePanel postPanel = (BasePanel)e.Item.FindControl("pnlPost");
		postPanel.RenderContentsOnly = !displaySettings.ListViewRenderPostPanel;
		postPanel.RenderId = false;

		BasePanel authorPanel = (BasePanel)e.Item.FindControl("pnlAuthor");
		authorPanel.CssClass = displaySettings.AuthorInfoPanelClass;

		BasePanel commentLink = (BasePanel)e.Item.FindControl("pnlCommentLink");
		commentLink.CssClass = displaySettings.CommentLinkClass;

		BasePanel topDate = (BasePanel)e.Item.FindControl("pnlTopDate");
		topDate.RenderId = false;
		topDate.Visible = false;
		topDate.CssClass = displaySettings.DatePanelClass;

		if (displaySettings.DateBottomPanelClass != "")
		{
			topDate.CssClass += " " + displaySettings.DateTopPanelClass;
		}

		if (!displaySettings.PostListUseBottomDate && !TitleOnly)
		{
			if (!displaySettings.PostListHideDate || displaySettings.ShowTagsOnPostList || showAuthorName)
			{
				topDate.Visible = true;
			}
		}

		BasePanel bottomDate = (BasePanel)e.Item.FindControl("pnlBottomDate");
		bottomDate.RenderId = false;
		bottomDate.Visible = false;
		bottomDate.CssClass = displaySettings.DatePanelClass;

		if (displaySettings.DateBottomPanelClass != "")
		{
			bottomDate.CssClass += " " + displaySettings.DateBottomPanelClass;
		}

		if (displaySettings.PostListUseBottomDate)
		{
			if (!displaySettings.PostListHideDate || displaySettings.ShowTagsOnPostList || showAuthorName)
			{
				bottomDate.Visible = true;
			}
		}

		BasePanel socialPanel = (BasePanel)e.Item.FindControl("pnlBlogSocial");
		socialPanel.RenderId = false;
		socialPanel.Visible = false;
		socialPanel.CssClass = displaySettings.SocialPanelClass;

		if (ShowTweetThisLink || UseFacebookLikeButton)
		{
			socialPanel.Visible = true;
		}

		Avatar av1 = (Avatar)e.Item.FindControl("av1");
		av1.Email = Convert.ToString(((DataRowView)e.Item.DataItem).Row["Email"]);
		av1.UserName = Convert.ToString(((DataRowView)e.Item.DataItem).Row["Name"]);
		av1.UserId = Convert.ToInt32(((DataRowView)e.Item.DataItem).Row["UserID"]);
		av1.AvatarFile = Convert.ToString(((DataRowView)e.Item.DataItem).Row["AvatarUrl"]);
		av1.MaxAllowedRating = MaxAllowedGravatarRating;
		av1.Disable = disableAvatars;
		av1.UseGravatar = allowGravatars;
		av1.SiteId = basePage.SiteInfo.SiteId;
		av1.UserNameTooltipFormat = displaySettings.AvatarUserNameTooltipFormat;
		av1.UseLink = UseProfileLinkForAvatar();
		av1.SiteRoot = SiteRoot;
		av1.CssClass = displaySettings.AvatarCssClass;
		av1.ExtraCssClass = displaySettings.AvatarExtraCssClass;

		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			string ItemId = Convert.ToInt32(((DataRowView)e.Item.DataItem).Row.ItemArray[2]).ToInvariantString();
			Repeater rptCategories = null;

			if (displaySettings.PostListUseBottomDate)
			{
				rptCategories = (Repeater)e.Item.FindControl("rptBottomCategories");
			}
			else
			{
				//rptCategoriesTop
				rptCategories = (Repeater)e.Item.FindControl("rptTopCategories");
			}

			if ((rptCategories != null) && (rptCategories.Visible))
			{

				string whereClause = string.Format("ItemID = '{0}'", ItemId);
				DataView dv = new DataView(dsBlogPosts.Tables["Categories"], whereClause, "", DataViewRowState.CurrentRows);

				rptCategories.DataSource = dv;
				rptCategories.DataBind();

				rptCategories.Visible = (rptCategories.Items.Count > 0);
			}

			Repeater rptAttachments = (Repeater)e.Item.FindControl("rptAttachments");

			if ((rptAttachments != null) && (rptAttachments.Visible))
			{
				string blogGuid = ((DataRowView)e.Item.DataItem).Row.ItemArray[1].ToString();
				string whereClause = string.Format("ItemGuid = '{0}'", blogGuid);
				DataView dv = new DataView(dsBlogPosts.Tables["Attachments"], whereClause, "", DataViewRowState.CurrentRows);

				rptAttachments.DataSource = dv;
				rptAttachments.DataBind();

				rptAttachments.Visible = rptAttachments.Items.Count > 0;
			}
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

	protected virtual void PopulateLabels()
	{
		EditBlogAltText = BlogResources.EditImageAltText;
		FeedBackLabel = BlogResources.BlogFeedbackLabel;
		mojoBasePage basePage = Page as mojoBasePage;

		if (basePage != null)
		{
			if (!basePage.UseTextLinksForFeatureSettings)
			{
				EditLinkImageUrl = ImageSiteRoot + "/Data/SiteImages/" + EditContentImage;
			}

			if (basePage.AnalyticsSection.Length == 0)
			{
				basePage.AnalyticsSection = mojoPortal.Core.Configuration.ConfigHelper.GetStringProperty("AnalyticsBlogSection", "blog");
			}
		}

		if (displaySettings.OverridePostCategoriesLabel.Length > 0)
		{
			CategoriesResourceKey = displaySettings.OverridePostCategoriesLabel;
		}
	}


	protected bool CanEditPost(int postAuthorId)
	{
		if (BlogConfiguration.SecurePostsByUser)
		{
			if (WebUser.IsInRoles(Config.ApproverRoles))
			{
				return true;
			}

			if (currentUser is null)
			{
				return false;
			}

			return (postAuthorId == currentUser.UserId);
		}

		return IsEditable;
	}


	protected string FormatSubtitle(string subTitle)
	{
		if (!displaySettings.ShowSubTitleOnList)
		{
			return string.Empty;
		}

		if (string.IsNullOrEmpty(subTitle))
		{
			return string.Empty;
		}

		return
			$"<{displaySettings.ListViewItemSubtitleElement} class=\"{displaySettings.ListViewItemSubtitleClass}\">{subTitle.RemoveMarkup()}</{displaySettings.ListViewItemSubtitleElement}>";
	}


	protected string FormatPostAuthor(bool showPostAuthor, string authorName, string firstName, string lastName)
	{
		if (showPostAuthor)
		{
			if (Config.BlogAuthor.Length > 0)
			{
				return string.Format(CultureInfo.InvariantCulture, BlogResources.PostAuthorFormat, Config.BlogAuthor);
			}

			if ((!string.IsNullOrEmpty(firstName)) && (!string.IsNullOrEmpty(lastName)))
			{
				return string.Format(CultureInfo.InvariantCulture, BlogResources.PostAuthorFormat, firstName + " " + lastName);
			}

			return string.Format(CultureInfo.InvariantCulture, BlogResources.PostAuthorFormat, authorName);
		}

		return string.Empty;
	}


	protected string FormatBlogEntry(string blogHtml, string excerpt, string url, int itemId, string imageUrl, bool useImageExcerpt, bool useImagePost, string title)
	{
		if (useExcerpt)
		{
			//if excerpt is populated just use it
			if ((excerpt.Length > 0) && (excerpt != "<p>&#160;</p>")) // this was added by the editor(s) when content was empty
			{
				return excerpt + Config.ExcerptSuffix + " <a href='" + FormatBlogUrl(url, itemId) + "' class='morelink'>" + Config.MoreLinkText + "</a><div>&nbsp;</div>";
			}

			// no excerpt so need to generate one
			string result = string.Empty;

			if ((blogHtml.Length > Config.ExcerptLength) && (Config.MoreLinkText.Length > 0))
			{
				result = UIHelper.CreateExcerpt(blogHtml, Config.ExcerptLength, Config.ExcerptSuffix);
				result += " <a href='" + FormatBlogTitleUrl(url, itemId) + "' class='morelink'>" + Config.MoreLinkText + "</a><div class='paddiv'>&nbsp;</div>";

				if (useImageExcerpt && imageUrl.Length > 0)
				{
					string imageMarkup = string.Format(CultureInfo.InvariantCulture, displaySettings.FeaturedImageFormat, ResolveUrl(imageUrl), title);

					if (displaySettings.FeaturedImageAbovePost)
					{
						return imageMarkup + result;
					}
					else
					{
						return result + imageMarkup;
					}
				}

				return result;
			}
			else
			{ // full post is shorter than excerpt length
				if (useImageExcerpt && imageUrl.Length > 0)
				{
					string imageMarkup = string.Format(CultureInfo.InvariantCulture, displaySettings.FeaturedImageFormat, ResolveUrl(imageUrl), title);

					if (displaySettings.FeaturedImageAbovePost)
					{
						return imageMarkup + blogHtml;
					}
					else
					{
						return blogHtml + imageMarkup;
					}
				}
			}
		}
		else
		{
			if (useImagePost && imageUrl.Length > 0)
			{
				string imageMarkup = string.Format(CultureInfo.InvariantCulture, displaySettings.FeaturedImageFormat, ResolveUrl(imageUrl), title);

				if (displaySettings.FeaturedImageAbovePost)
				{
					return imageMarkup + blogHtml;
				}
				else
				{
					return blogHtml + imageMarkup;
				}
			}
		}

		return blogHtml;
	}

	protected string FormatBlogDate(DateTime startDate)
	{
		string timeFormat = displaySettings.OverrideDateFormat;

		if (timeFormat.Length == 0)
		{
			timeFormat = Config.DateTimeFormat;
		}

		if (timeZone != null)
		{
			return TimeZoneInfo.ConvertTimeFromUtc(startDate, timeZone).ToString(timeFormat);
		}

		return startDate.AddHours(TimeOffset).ToString(timeFormat);
	}


	protected string FormatBlogUrl(string itemUrl, int itemId)
	{
		if (useFriendlyUrls && !string.IsNullOrWhiteSpace(itemUrl))
		{
			return itemUrl.ToLinkBuilder().ToString() + disqusFlag;
		}

		return "Blog/ViewPost.aspx".ToLinkBuilder().PageId(PageId).ModuleId(ModuleId).ItemId(itemId).ToString() + disqusFlag;
	}


	protected string FormatBlogTitleUrl(string itemUrl, int itemId)
	{
		if (useFriendlyUrls && !string.IsNullOrWhiteSpace(itemUrl))
		{
			return itemUrl.ToLinkBuilder().ToString();
		}

		return "Blog/ViewPost.aspx".ToLinkBuilder().PageId(PageId).ModuleId(ModuleId).ItemId(itemId).ToString();
	}


	private string GetRssUrl()
	{
		return "Blog/RSS.aspx".ToLinkBuilder()
						  .AddParam("p", Invariant($"{PageId}~{ModuleId}~{categoryId}"))
						  .AddParam("r", Global.FeedRedirectBypassToken)
						  .ToString();
	}


	protected virtual void LoadSettings()
	{
		siteSettings = CacheHelper.GetCurrentSiteSettings();
		SiteId = siteSettings.SiteId;
		currentUser = SiteUtils.GetCurrentSiteUser();
		TimeOffset = SiteUtils.GetUserTimeOffset();
		timeZone = SiteUtils.GetUserTimeZone();
		GmapApiKey = SiteUtils.GetGmapApiKey();

		pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
		categoryId = WebUtils.ParseInt32FromQueryString("cat", categoryId);
		Month = WebUtils.ParseInt32FromQueryString("month", Month);
		Year = WebUtils.ParseInt32FromQueryString("year", Year);
		attachmentBaseUrl = SiteUtils.GetFileAttachmentUploadPath();

		if (Page is mojoBasePage)
		{
			basePage = Page as mojoBasePage;
			module = basePage.GetModule(ModuleId, Blog.FeatureGuid);
		}

		if (module is null)
		{
			return;
		}

		MaxAllowedGravatarRating = SiteUtils.GetMaxAllowedGravatarRating();
		UserNameTooltipFormat = displaySettings.AvatarUserNameTooltipFormat;

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

		//if (!config.ShowAuthorAvatar) { disableAvatars = true; }

		if (Config.UseExcerpt && !displaySettings.ShowAvatarWithExcerpt)
		{
			disableAvatars = true;
		}

		CalendarDate = WebUtils.ParseDateFromQueryString("blogdate", DateTime.UtcNow).Date;

		if (CalendarDate > DateTime.UtcNow.Date)
		{
			CalendarDate = DateTime.UtcNow.Date;
		}

		if ((Config.UseExcerpt) && (!Config.GoogleMapIncludeWithExcerpt))
		{
			ShowGoogleMap = false;
		}

		EnableContentRating = Config.EnableContentRating && !displaySettings.PostListDisableContentRating;

		if (Config.UseExcerpt)
		{
			EnableContentRating = false;
		}

		if (Config.DisqusSiteShortName.Length > 0)
		{
			DisqusSiteShortName = Config.DisqusSiteShortName;
		}
		else
		{
			DisqusSiteShortName = siteSettings.DisqusSiteShortName;
		}

		if (Config.IntenseDebateAccountId.Length > 0)
		{
			IntenseDebateAccountId = Config.IntenseDebateAccountId;
		}
		else
		{
			IntenseDebateAccountId = siteSettings.IntenseDebateAccountId;
		}

		Control cNav = Page.LoadControl("~/Blog/Controls/BlogNav.ascx");

		BlogNav nav = (BlogNav)cNav;

		nav.ModuleId = ModuleId;
		nav.ModuleGuid = module.ModuleGuid;
		nav.PageId = PageId;
		nav.IsEditable = IsEditable;
		nav.Config = Config;
		nav.SiteRoot = SiteRoot;

		TitleOnly = Config.TitleOnly || displaySettings.PostListForceTitleOnly;
		ShowTweetThisLink = Config.ShowTweetThisLink && !Config.UseExcerpt;
		UseFacebookLikeButton = Config.UseFacebookLikeButton && !Config.UseExcerpt;
		useExcerpt = Config.UseExcerpt || displaySettings.PostListForceExcerptMode;
		pageSize = Config.PageSize;
		AllowComments = Config.AllowComments && ShowCommentCounts;

		switch (DisplayMode)
		{
			case "ByCategory":
				if (displaySettings.CategoryListForceTitleOnly)
				{
					TitleOnly = true;
				}

				if (displaySettings.CategoryListOverridePageSize > 0)
				{
					pageSize = displaySettings.CategoryListOverridePageSize;
				}

				if (displaySettings.ArchiveViewHideFeedbackLink)
				{
					AllowComments = false;
				}

				if (displaySettings.OverrideCategoryListItemHeadingElement.Length > 0)
				{
					itemHeadingElement = displaySettings.OverrideCategoryListItemHeadingElement;
				}

				break;

			case "ByMonth":
				if (displaySettings.ArchiveListForceTitleOnly)
				{
					TitleOnly = true;
				}

				if (displaySettings.ArchiveListOverridePageSize > 0)
				{
					pageSize = displaySettings.ArchiveListOverridePageSize;
				}

				if (displaySettings.OverrideArchiveListItemHeadingElement.Length > 0)
				{
					itemHeadingElement = displaySettings.OverrideArchiveListItemHeadingElement;
				}

				break;

			case "DescendingByDate":
			default:
				if (displaySettings.PostListOverridePageSize > 0)
				{
					pageSize = displaySettings.PostListOverridePageSize;
				}

				if (displaySettings.OverrideListItemHeadingElement.Length > 0)
				{
					itemHeadingElement = displaySettings.OverrideListItemHeadingElement;
				}

				break;
		}

		if (Config.AllowComments)
		{
			if ((DisqusSiteShortName.Length > 0) && (Config.CommentSystem == "disqus"))
			{
				disqusFlag = "#disqus_thread";
				disqus.SiteShortName = DisqusSiteShortName;
				disqus.RenderCommentCountScript = true;
				nav.ShowCommentCount = false;
			}

			if ((IntenseDebateAccountId.Length > 0) && (Config.CommentSystem == "intensedebate"))
			{
				ShowCommentCounts = false;
				nav.ShowCommentCount = false;
			}

			if (Config.CommentSystem == "facebook")
			{
				ShowCommentCounts = false;
				nav.ShowCommentCount = false;
			}
		}
		else
		{
			nav.ShowCommentCount = false;
		}

		bool showNav = false;

		if (
			Config.ShowCalendar ||
			Config.ShowArchives ||
			((Config.ShowFeedLinks == true && displaySettings.HideFeedLinks == false) ? true : false) ||
			Config.ShowCategories ||
			Config.ShowStatistics ||
			!string.IsNullOrWhiteSpace(Config.UpperSidebar) ||
			!string.IsNullOrWhiteSpace(Config.LowerSidebar)
		)
		{
			showNav = true;
		}

		divBlog.CssClass = displaySettings.ListViewCenterClass;

		if (showNav)
		{
			if (Config.NavigationOnRight)
			{
				phNavRight.Controls.Add(nav);
				divBlog.CssClass += " " + displaySettings.ListViewCenterRightNavClass;
			}
			else
			{
				phNavLeft.Controls.Add(nav);
				divBlog.CssClass += " " + displaySettings.ListViewCenterLeftNavClass;
			}
		}
		else
		{
			divBlog.CssClass += " " + displaySettings.ListViewCenterNoNavClass;
		}

		if (displaySettings.PostListExtraCss.Length > 0)
		{
			divBlog.CssClass += " " + displaySettings.PostListExtraCss;
		}

		pnlLayoutRow.RenderId = false;
		pnlLayoutRow.RenderContentsOnly = true;
		pnlLayoutRow.CssClass = displaySettings.LayoutRowClass;

		if (showNav && displaySettings.LayoutRowRender)
		{
			pnlLayoutRow.RenderContentsOnly = false;
		}

		useFriendlyUrls = BlogConfiguration.UseFriendlyUrls(ModuleId);

		if (!WebConfigSettings.UseUrlReWriting)
		{
			useFriendlyUrls = false;
		}

		if (Config.Copyright.Length > 0)
		{
			litCopyright.Text = Config.Copyright;
			pnlCopyright.Visible = true;
		}

		pnlCopyright.CssClass = displaySettings.CopyrightPanelClass;

		pnlPager.CssClass = displaySettings.PagerPanelClass;
	}


	protected bool UseProfileLink()
	{
		if (!displaySettings.LinkAuthorAvatarToProfile)
		{
			return false;
		}

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


	protected virtual void SetupRssLink()
	{
		if (WebConfigSettings.DisableBlogRssMetaLink)
		{
			return;
		}

		if (Config.FeedIsDisabled)
		{
			return;
		}

		if (!Config.AddFeedDiscoveryLink)
		{
			return;
		}

		if (module != null)
		{
			if (Page.Master != null)
			{
				Control head = Page.Master.FindControl("Head1");

				if (head != null)
				{
					Literal rssLink = new Literal();
					rssLink.Text =
						"<link rel=\"alternate\" type=\"application/rss+xml\" title=\"" +
						module.ModuleTitle +
						"\" href=\"" +
						GetRssUrl() + "\" />"
					;

					head.Controls.Add(rssLink);
				}
			}
		}
	}
}