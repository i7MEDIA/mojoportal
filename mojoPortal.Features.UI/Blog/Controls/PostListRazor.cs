using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.BlogUI
{
	[ToolboxData("<{0}:PostListRazor runat=server></{0}:PostListRazor>")]
	public class PostListRazor : WebControl
	{
		#region Properties

		private static readonly ILog log = LogManager.GetLogger(typeof(PostListRazor));
		private int pageNumber = 1;
		private int totalPages = 1;
		private int pageSize = 4;
		protected string EditContentImage = WebConfigSettings.EditContentImage;
		protected string EditBlogAltText = "Edit";
		protected Double TimeOffset = 0;
		private TimeZoneInfo timeZone = null;
		protected DateTime CalendarDate;
		protected string addThisAccountId = string.Empty;
		protected bool ShowGoogleMap = true;
		protected string addThisCustomBrand = string.Empty;
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
		//private mojoBasePage basePage = null;
		private Module module = null;
		protected BlogConfiguration blogConfig = new BlogConfiguration();
		protected BlogPostListAdvancedConfiguration config = new BlogPostListAdvancedConfiguration();
		private bool useFriendlyUrls = true;
		private int pageId = -1;
		private int moduleId = -1;
		private int categoryId = -1;
		private bool isEditable = false;
		private string siteRoot = string.Empty;
		private string imageSiteRoot = string.Empty;
		private string navigationSiteRoot = string.Empty;
		private SiteSettings siteSettings = null;
		protected string CategoriesResourceKey = "PostCategories";
		protected int Month = DateTime.UtcNow.Month;
		protected int Year = DateTime.UtcNow.Year;
		protected bool TitleOnly = false;
		protected bool ShowTweetThisLink = false;
		protected bool ShowPlusOneButton = false;
		protected bool UseFacebookLikeButton = false;
		protected bool AllowComments = false;
		protected bool useExcerpt = false;
		protected string attachmentBaseUrl = string.Empty;

		private SiteUser currentUser = null;

		private int siteId = -1;
		public int SiteId
		{
			get { return siteId; }
		}

		public int PageId
		{
			get { return pageId; }
			set { pageId = value; }
		}

		public int ModuleId
		{
			get { return moduleId; }
			set { moduleId = value; }
		}

		public string SiteRoot
		{
			get { return siteRoot; }
			set { siteRoot = value; }
		}

		public string ImageSiteRoot
		{
			get { return imageSiteRoot; }
			set { imageSiteRoot = value; }
		}

		public BlogConfiguration BlogConfig
		{
			get { return blogConfig; }
			set { blogConfig = value; }
		}

		public BlogPostListAdvancedConfiguration Config
		{
			get { return config; }
			set { config = value; }
		}

		public bool IsEditable
		{
			get { return isEditable; }
			set { isEditable = value; }
		}

		private string displayMode = "DescendingByDate";

		public string DisplayMode
		{
			get { return displayMode; }
			set { displayMode = value; }
		}

		#endregion


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			LoadSettings();

			if (module == null)
			{
				Visible = false;
				return;
			}

			if (Page.IsPostBack)
			{
				return;
			}
		}

		protected virtual void LoadSettings()
		{
			siteSettings = CacheHelper.GetCurrentSiteSettings();
			siteId = siteSettings.SiteId;
			currentUser = SiteUtils.GetCurrentSiteUser();
			TimeOffset = SiteUtils.GetUserTimeOffset();
			timeZone = SiteUtils.GetUserTimeZone();
			GmapApiKey = SiteUtils.GetGmapApiKey();
			addThisAccountId = siteSettings.AddThisDotComUsername;

			if (blogConfig.AddThisAccountId.Length > 0)
			{
				addThisAccountId = blogConfig.AddThisAccountId;
			}

			pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
			categoryId = WebUtils.ParseInt32FromQueryString("cat", categoryId);
			Month = WebUtils.ParseInt32FromQueryString("month", Month);
			Year = WebUtils.ParseInt32FromQueryString("year", Year);
			attachmentBaseUrl = SiteUtils.GetFileAttachmentUploadPath();

			//if (Page is mojoBasePage)
			//{
			//	basePage = Page as mojoBasePage;
			//	module = basePage.GetModule(moduleId, config.FeatureGuid);

			//}

			module = new Module(moduleId);

			//if (module == null)
			//{
			//	return;
			//}

			CalendarDate = WebUtils.ParseDateFromQueryString("blogdate", DateTime.UtcNow).Date;

			if (CalendarDate > DateTime.UtcNow.Date)
			{
				CalendarDate = DateTime.UtcNow.Date;
			}

			if (blogConfig.UseExcerpt && !blogConfig.GoogleMapIncludeWithExcerpt)
			{
				ShowGoogleMap = false;
			}

			if (blogConfig.UseExcerpt)
			{
				EnableContentRating = false;
			}

			if (blogConfig.DisqusSiteShortName.Length > 0)
			{
				DisqusSiteShortName = blogConfig.DisqusSiteShortName;
			}
			else
			{
				DisqusSiteShortName = siteSettings.DisqusSiteShortName;
			}

			if (blogConfig.IntenseDebateAccountId.Length > 0)
			{
				IntenseDebateAccountId = blogConfig.IntenseDebateAccountId;
			}
			else
			{
				IntenseDebateAccountId = siteSettings.IntenseDebateAccountId;
			}

			ShowTweetThisLink = blogConfig.ShowTweetThisLink && !blogConfig.UseExcerpt;
			ShowPlusOneButton = blogConfig.ShowPlusOneButton && !blogConfig.UseExcerpt;
			UseFacebookLikeButton = blogConfig.UseFacebookLikeButton && !blogConfig.UseExcerpt;

			pageSize = config.ItemsPerPage;

			useFriendlyUrls = BlogConfiguration.UseFriendlyUrls(moduleId);

			if (!WebConfigSettings.UseUrlReWriting)
			{
				useFriendlyUrls = false;
			}

			if (WebConfigSettings.UseFolderBasedMultiTenants)
			{
				navigationSiteRoot = SiteUtils.GetNavigationSiteRoot();
				imageSiteRoot = WebUtils.GetSiteRoot();
			}
			else
			{
				navigationSiteRoot = WebUtils.GetHostRoot();
				imageSiteRoot = navigationSiteRoot;

			}

		}

		protected override void RenderContents(HtmlTextWriter output)
		{
			DataSet dsBlogs = null;

			// Check for Featured Post, if it exists grab one less post to keep the count correct
			if (blogConfig.FeaturedPostId == 0)
			{
				dsBlogs = Blog.GetPageDataSet(config.BlogModuleId, DateTime.UtcNow, pageNumber, pageSize, out totalPages);
			}
			else
			{
				dsBlogs = Blog.GetPageDataSet(config.BlogModuleId, DateTime.UtcNow, pageNumber, (pageSize - 1), out totalPages);
			}

			DataRow featuredRow = dsBlogs.Tables["Posts"].NewRow();

			if (blogConfig.FeaturedPostId != 0 && pageNumber == 1)
			{
				using IDataReader reader = Blog.GetSingleBlog(blogConfig.FeaturedPostId);
				while (reader.Read())
				{
					featuredRow["ItemID"] = Convert.ToInt32(reader["ItemID"]);
					featuredRow["ModuleID"] = Convert.ToInt32(reader["ModuleID"]);
					featuredRow["BlogGuid"] = reader["BlogGuid"].ToString();
					featuredRow["CreatedDate"] = Convert.ToDateTime(reader["CreatedDate"]);
					featuredRow["Heading"] = reader["Heading"].ToString();
					featuredRow["SubTitle"] = reader["SubTitle"].ToString();
					featuredRow["StartDate"] = Convert.ToDateTime(reader["StartDate"]);
					featuredRow["EndDate"] = Convert.ToDateTime(reader["EndDate"]);
					featuredRow["Description"] = reader["Description"].ToString();
					featuredRow["Abstract"] = reader["Abstract"].ToString();
					featuredRow["ItemUrl"] = reader["ItemUrl"].ToString();
					featuredRow["Location"] = reader["Location"].ToString();
					featuredRow["MetaKeywords"] = reader["MetaKeywords"].ToString();
					featuredRow["MetaDescription"] = reader["MetaDescription"].ToString();
					featuredRow["LastModUtc"] = Convert.ToDateTime(reader["LastModUtc"]);
					featuredRow["IsPublished"] = true;
					featuredRow["IncludeInFeed"] = Convert.ToBoolean(reader["IncludeInFeed"]);
					featuredRow["CommentCount"] = Convert.ToInt32(reader["CommentCount"]);
					featuredRow["CommentCount"] = 0;
					featuredRow["UserID"] = Convert.ToInt32(reader["UserID"]);
					featuredRow["UserID"] = -1;
					featuredRow["Name"] = reader["Name"].ToString();
					featuredRow["FirstName"] = reader["FirstName"].ToString();
					featuredRow["LastName"] = reader["LastName"].ToString();
					featuredRow["LoginName"] = reader["LoginName"].ToString();
					featuredRow["Email"] = reader["Email"].ToString();
					featuredRow["AvatarUrl"] = reader["AvatarUrl"].ToString();
					featuredRow["AuthorBio"] = reader["AuthorBio"].ToString();
					featuredRow["AllowCommentsForDays"] = Convert.ToInt32(reader["AllowCommentsForDays"]);

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

					featuredRow["MapHeight"] = reader["MapHeight"].ToString();
					featuredRow["MapWidth"] = reader["MapWidth"].ToString();
					featuredRow["MapType"] = reader["MapType"].ToString();

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

					featuredRow["HeadlineImageUrl"] = reader["HeadlineImageUrl"].ToString();

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
				if ((bool)featuredRow["IsPublished"] && (DateTime)featuredRow["StartDate"] <= DateTime.UtcNow && (DateTime)featuredRow["EndDate"] > DateTime.UtcNow)
				{
					//look for featured post in datable
					DataRow found = dsBlogs.Tables["Posts"].Rows.Find(blogConfig.FeaturedPostId);

					if (found != null)
					{
						//remove featured post from datatable so we can insert it at the top if we're on "page" number 1
						dsBlogs.Tables["Posts"].Rows.Remove(found);
					}

					//insert the featured post into the datatable at the top
					//we only want to do this if the current "page" is number 1, don't want the featured post on other pages.
					dsBlogs.Tables["Posts"].Rows.InsertAt(featuredRow, 0);
				}
			}

			List<PageModule> pageModules = PageModule.GetPageModulesByModule(config.BlogModuleId);

			string blogPageUrl = string.Empty;

			if (pageModules.Count > 0)
			{
				blogPageUrl = pageModules[0].PageUrl;
			}

			List<BlogPostModel> models = new List<BlogPostModel>();

			foreach (DataRow postRow in dsBlogs.Tables["posts"].Rows)
			{
				BlogPostModel model = new BlogPostModel();

				if (useFriendlyUrls && (postRow["ItemUrl"].ToString().Length > 0))
				{
					model.ItemUrl = navigationSiteRoot + postRow["ItemUrl"].ToString().Replace("~", string.Empty);
				}
				else
				{
					model.ItemUrl = postRow["ItemID"].ToString() + "&mid=" + postRow["ModuleID"].ToString();
				}

				if (blogConfig.FeaturedPostId == Convert.ToInt32(postRow["ItemID"]) && pageNumber == 1)
				{
					model.FeaturedPost = true;
				}
				else
				{
					model.FeaturedPost = false;
				}
				
				model.Title = postRow["Heading"].ToString();
				model.SubTitle = postRow["SubTitle"].ToString();
				model.Body = postRow["Description"].ToString();
				model.AuthorAvatar = postRow["AvatarUrl"].ToString();
				model.AuthorDisplayName = postRow["Name"].ToString();
				model.AuthorFirstName = postRow["FirstName"].ToString();
				model.AuthorLastName = postRow["LastName"].ToString();
				model.AuthorBio = postRow["AuthorBio"].ToString();
				model.Excerpt = postRow["Abstract"].ToString();
				model.PostDate = Convert.ToDateTime(postRow["StartDate"].ToString());
				model.HeadlineImageUrl = postRow["HeadlineImageUrl"].ToString();
				model.CommentCount = Convert.ToInt32(postRow["CommentCount"]);

				model.AllowCommentsForDays = Convert.ToInt32(postRow["AllowCommentsForDays"]);
				model.ShowAuthorName = Convert.ToBoolean(postRow["ShowAuthorName"]);
				model.ShowAuthorAvatar = Convert.ToBoolean(postRow["ShowAuthorAvatar"]);
				model.ShowAuthorBio = Convert.ToBoolean(postRow["ShowAuthorBio"]);
				model.AuthorUserId = Convert.ToInt32(postRow["UserID"]);

				models.Add(model);
			}

			PostListModel postListObject = new PostListModel();

			if (module != null)
			{
				postListObject.ModuleTitle = module.ModuleTitle;
				postListObject.Module = module;
			}
			else
			{
				postListObject.ModuleTitle = "";
			}

			//postListObject.ModuleTitle = module == null ? "" : module.ModuleTitle;
			postListObject.ModulePageUrl = Page.ResolveUrl(blogPageUrl);
			postListObject.Posts = models;

			string text = string.Empty;

			try
			{
				text = RazorBridge.RenderPartialToString(config.Layout, postListObject, "Blog");
			}
			//catch (System.Web.HttpException ex)
			//{
			//	renderDefaultView(ex.ToString());
			//}
			//catch (ArgumentNullException ex)
			//{
			//	renderDefaultView(ex.ToString());
			//}
			catch (Exception ex)
			{
				renderDefaultView(ex.ToString());
			}

			void renderDefaultView(string error = "")
			{
				if (!string.IsNullOrWhiteSpace(error))
				{
					log.ErrorFormat(
						"chosen layout ({0}) for _BlogPostList was not found in skin {1}. perhaps it is in a different skin. Error was: {2}",
						config.Layout,
						SiteUtils.GetSkinBaseUrl(true, Page),
						error
					);
				}

				text = RazorBridge.RenderPartialToString("_BlogPostList", postListObject, "Blog");
			}
			output.Write(text);
		}
	}
}