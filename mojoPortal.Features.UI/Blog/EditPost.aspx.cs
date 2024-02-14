using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.Extensions;
using mojoPortal.FileSystem;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Newtonsoft.Json;
using Resources;

namespace mojoPortal.Web.BlogUI;

public partial class BlogEdit : NonCmsBasePage
{
	private static readonly ILog log = LogManager.GetLogger(typeof(BlogEdit));

	protected int moduleId = -1;
	protected int itemId = -1;
	protected int pageId = -1;
	//protected String cacheDependencyKey;
	protected string virtualRoot;
	protected double timeOffset = 0;
	private TimeZoneInfo timeZone = null;
	protected Hashtable moduleSettings;
	protected BlogConfiguration config = new BlogConfiguration();
	private int pageNumber = 1;
	private int pageSize = 10;
	private int totalPages = 1;
	private Guid restoreGuid = Guid.Empty;
	private Blog blog = null;
	private bool enableContentVersioning = false;
	protected bool isAdmin = false;
	//private string defaultCommentDaysAllowed = "90";
	ContentMetaRespository metaRepository = new ContentMetaRespository();
	private bool useFriendlyUrls = true;

	protected string upLoadPath = string.Empty;
	private IFileSystem fileSystem = null;
	private SiteUser currentUser = null;
	private bool cancelRedirect = false;
	private string currentFeaturedImagePath = string.Empty;

	private SiteUser siteUser = null;

	private string blogMetaConfigFile = string.Empty;
	private string blogMetaConfigDefault = string.Empty;


	private void Page_Load(object sender, EventArgs e)
	{
		if (SiteUtils.SslIsAvailable() && (siteSettings.UseSslOnAllPages || CurrentPage.RequireSsl))
		{
			SiteUtils.ForceSsl();
		}
		else
		{
			SiteUtils.ClearSsl();
		}

		if (!Request.IsAuthenticated)
		{
			SiteUtils.RedirectToLoginPage(this);
			return;
		}

		SecurityHelper.DisableBrowserCache();

		LoadParams();

		if (!UserCanEditModule(moduleId, Blog.FeatureGuid))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		if (SiteUtils.IsFishyPost(this))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		LoadSettings();

		if (blog != null)
		{
			currentFeaturedImagePath = blog.HeadlineImageUrl;

			//existing post check if user can edit it
			if (BlogConfiguration.SecurePostsByUser)
			{
				if (!WebUser.IsInRoles(config.ApproverRoles))
				{
					if ((currentUser == null) || (currentUser.UserId != blog.UserId))
					{
						// this post does not belong to this user
						// redirect back to the blog page
						WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
						return;
					}
				}
			}
		}

		if (ScriptController != null)
		{
			ScriptController.RegisterAsyncPostBackControl(btnAddCategory);
		}
		else
		{
			log.Error("ScriptController was null");
		}

		PopulateLabels();
		SetupScripts();



		if ((!Page.IsPostBack) && (!Page.IsCallback))
		{
			if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
			{
				hdnReturnUrl.Value = Request.UrlReferrer.ToString();
				lnkCancel.NavigateUrl = Request.UrlReferrer.ToString();
				//lnkCancel2.NavigateUrl = lnkCancel.NavigateUrl;
				//lnkCancel3.NavigateUrl = lnkCancel.NavigateUrl;
			}

			PopulateControls();
			PopulateCategories();
			BindAttachments();
			BindMeta();
			BindMetaLinks();
		}
	}

	protected virtual void PopulateControls()
	{
		if (blog != null)
		{
			dpBeginDate.ShowTime = true;
			if (timeZone != null)
			{
				dpBeginDate.Text = blog.StartDate.ToLocalTime(timeZone).ToString("g");
				if (blog.EndDate < DateTime.MaxValue)
				{
					dpEndDate.Text = blog.EndDate.ToLocalTime(timeZone).ToString("g");
				}
			}
			else
			{
				dpBeginDate.Text = DateTimeHelper.LocalizeToCalendar(blog.StartDate.AddHours(timeOffset).ToString("g"));
				if (blog.EndDate < DateTime.MaxValue)
				{
					dpEndDate.Text = DateTimeHelper.LocalizeToCalendar(blog.EndDate.AddHours(timeOffset).ToString("g"));
				}
			}

			txtTitle.Text = blog.Title;
			txtSubTitle.Text = blog.SubTitle;
			txtItemUrl.Text = blog.ItemUrl;
			txtLocation.Text = blog.Location;
			edContent.Text = blog.Description;
			edExcerpt.Text = blog.Excerpt;
			txtMetaDescription.Text = blog.MetaDescription;
			txtMetaKeywords.Text = blog.MetaKeywords;
			this.chkIncludeInFeed.Checked = blog.IncludeInFeed;
			chkIsPublished.Checked = blog.IsPublished;

			chkShowDownloadLink.Checked = blog.ShowDownloadLink;
			chkUseBing.Checked = blog.UseBingMap;

			((GMapTypeSetting)MapTypeControl).SetValue(blog.MapType);
			((GMapZoomLevelSetting)ZoomLevelControl).SetValue(blog.MapZoom.ToInvariantString());
			txtMapHeight.Text = blog.MapHeight;
			txtMapWidth.Text = blog.MapWidth;
			chkShowMapOptions.Checked = blog.ShowMapOptions;
			chkShowMapZoom.Checked = blog.ShowZoomTool;
			chkShowMapBalloon.Checked = blog.ShowLocationInfo;
			chkShowMapDirections.Checked = blog.UseDrivingDirections;

			chkIncludeInSearchIndex.Checked = blog.IncludeInSearch;
			chkExcludeFromRecentContent.Checked = blog.ExcludeFromRecentContent;
			chkIncludeInSiteMap.Checked = blog.IncludeInSiteMap;
			chkShowAuthorName.Checked = blog.ShowAuthorName;
			chkShowAuthorAvatar.Checked = blog.ShowAuthorAvatar;
			chkShowAuthorBio.Checked = blog.ShowAuthorBio;

			chkIncludeInNews.Checked = blog.IncludeInNews;
			txtPublicationName.Text = blog.PubName;
			txtPubLanguage.Text = blog.PubLanguage;
			txtPubGenres.Text = blog.PubGenres;
			txtPubGeoLocations.Text = blog.PubGeoLocations;
			txtPubKeyWords.Text = blog.PubKeyWords;
			txtPubStockTickers.Text = blog.PubStockTickers;
			txtHeadlineImage.Text = blog.HeadlineImageUrl;

			chkFeaturedPost.Checked = config.FeaturedPostId == blog.ItemId ? true : false;

			if (blog.HeadlineImageUrl.Length > 0)
			{
				imgPreview.ImageUrl = blog.HeadlineImageUrl;
			}

			chkIncludeImageInExcerpt.Checked = blog.IncludeImageInExcerpt;
			chkIncludeImageInPost.Checked = blog.IncludeImageInPost;

			ListItem item = ddCommentAllowedForDays.Items.FindByValue(blog.AllowCommentsForDays.ToInvariantString());
			if (item != null)
			{
				ddCommentAllowedForDays.ClearSelection();
				item.Selected = true;
			}

			if (restoreGuid != Guid.Empty)
			{
				ContentHistory rHistory = new ContentHistory(restoreGuid);
				if (rHistory.ContentGuid == blog.BlogGuid)
				{
					edContent.Text = rHistory.ContentText;
				}

			}
			// show preview button for saved drafts
			if ((!blog.IsPublished) || (blog.StartDate > DateTime.UtcNow)) { btnSaveAndPreview.Visible = true; }

			BindHistory();
		}
		else
		{
			chkIncludeInFeed.Checked = true;
			dpBeginDate.Text = DateTimeHelper.LocalizeToCalendar(DateTime.UtcNow.AddHours(timeOffset).ToString("g"));
			this.btnDelete.Visible = false;
			pnlHistory.Visible = false;
		}

		if ((txtItemUrl.Text.Length == 0) && (txtTitle.Text.Length > 0))
		{
			string friendlyUrl;

			if (WebConfigSettings.AppendDateToBlogUrls)
			{
				friendlyUrl = SiteUtils.SuggestFriendlyUrl($"{config.DefaultUrlPrefix}{txtTitle.Text}-{DateTime.UtcNow.AddHours(timeOffset):yyyy-MM-dd}", siteSettings);
			}
			else
			{
				friendlyUrl = SiteUtils.SuggestFriendlyUrl($"{config.DefaultUrlPrefix}{txtTitle.Text}", siteSettings);
			}

			txtItemUrl.Text = "~/" + friendlyUrl;
		}

		if (blog != null)
		{
			hdnTitle.Value = txtTitle.Text;
		}
	}


	private void PopulateCategories()
	{
		// Mono doesn't see this in update panel
		// so help find it
		if (chkCategories == null)
		{
			log.Error("chkCategories was null");

			chkCategories = (CheckBoxList)UpdatePanel1.FindControl("chkCategories");
		}


		chkCategories.Items.Clear();

		IDataReader reader;

		using (reader = Blog.GetCategoriesList(moduleId))
		{
			while (reader.Read())
			{
				ListItem listItem = new ListItem();

				listItem.Text = reader["Category"].ToString();
				listItem.Value = reader["CategoryID"].ToString();

				chkCategories.Items.Add(listItem);
			}
		}

		if (itemId > -1)
		{
			using (reader = Blog.GetItemCategories(this.itemId))
			{
				while (reader.Read())
				{
					ListItem item = chkCategories.Items.FindByValue(reader["CategoryID"].ToString());

					if (item != null)
					{
						item.Selected = true;
					}
				}
			}
		}
	}


	private void BindAttachments()
	{
		if (blog == null)
		{
			grdAttachments.Visible = false;

			return;
		}

		using (IDataReader reader = FileAttachment.SelectByItem(blog.BlogGuid))
		{
			grdAttachments.DataSource = reader;
			grdAttachments.DataBind();
		}

		if (grdAttachments.Rows.Count == 0)
		{
			grdAttachments.Visible = false;
		}
	}


	void grdAttachments_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		string sGuid = e.CommandArgument.ToString();

		if (sGuid.Length != 36)
		{
			return;
		}

		Guid attachmentGuid = new Guid(sGuid);

		FileAttachment fileAttachment;

		switch (e.CommandName)
		{
			case "delete":

				fileAttachment = new FileAttachment(attachmentGuid);
				SiteUtils.DeleteAttachmentFile(fileSystem, fileAttachment, upLoadPath);
				FileAttachment.Delete(attachmentGuid);

				WebUtils.SetupRedirect(this, Request.RawUrl);

				break;
		}
	}


	void grdAttachments_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		// do nothing, the rowcommand will do the delete, but this event has to be here because it will fire.
	}


	protected void btnUpload_Click(object sender, EventArgs e)
	{
		if (blog == null)
		{
			return;
		}

		if (currentUser == null)
		{
			return;
		}

		if (fileSystem == null)
		{
			return;
		}

		if (uploader.HasFile)
		{
			string ext = System.IO.Path.GetExtension(uploader.FileName);
			string mimeType = IOHelper.GetMimeType(ext).ToLower();

			if (SiteUtils.IsAllowedUploadBrowseFile(ext, WebConfigSettings.AllowedMediaFileExtensions))
			{
				var fileName = Path.GetFileName(uploader.FileName);
				var f = new FileAttachment
				{
					CreatedBy = currentUser.UserGuid,
					FileName = fileName,
					ServerFileName = blog.ItemId.ToInvariantString() + fileName.ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles),
					ModuleGuid = blog.ModuleGuid,
					SiteGuid = siteSettings.SiteGuid,
					ItemGuid = blog.BlogGuid,
					ContentLength = uploader.FileBytes.Length,
					ContentType = mimeType
				};
				f.Save();

				string destPath = upLoadPath + f.ServerFileName;

				using (uploader.FileContent)
				{
					fileSystem.SaveFile(destPath, uploader.FileContent, mimeType, true);
				}
			}
			else
			{
				return;
			}
		}

		WebUtils.SetupRedirect(this, Request.RawUrl);	}


	protected void btnAddCategory_Click(object sender, EventArgs e)
	{
		if (txtCategory.Text.Length > 0)
		{
			int newCategoryId = Blog.AddBlogCategory(moduleId, txtCategory.Text);

			if (itemId > -1)
			{
				Blog.AddItemCategory(itemId, newCategoryId);
			}

			//preserve the current selections
			var selCats = new List<string>();

			foreach (ListItem i in chkCategories.Items)
			{
				if (i.Selected)
				{
					selCats.Add(i.Value);
				}
			}

			PopulateCategories();
			ListItem item = chkCategories.Items.FindByValue(newCategoryId.ToInvariantString());

			if (item != null)
			{
				item.Selected = true;
			}

			//restore the previous selections
			foreach (string s in selCats)
			{
				item = chkCategories.Items.FindByValue(s);

				if (item != null)
				{
					item.Selected = true;
				}
			}

			txtCategory.Text = string.Empty;

			UpdatePanel1.Update();
		}
	}


	protected virtual void btnUpdate_Click(object sender, EventArgs e)
	{
		Page.Validate("blog");

		if (Page.IsValid)
		{
			Save();

			if (cancelRedirect)
			{
				pnlHistory.Visible = false;
				return;
			}

			if (hdnReturnUrl.Value.Length > 0)
			{
				WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
				return;
			}

			WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}
	}


	void btnSaveAndPreview_Click(object sender, EventArgs e)
	{
		Page.Validate("blog");

		if ((Page.IsValid) && (ParamsAreValid()))
		{
			Save();

			WebUtils.SetupRedirect(this, SiteRoot + blog.ItemUrl.Replace("~/", "/"));
		}
	}

	private bool ParamsAreValid()
	{
		try
		{
			DateTime localTime = DateTime.Parse(dpBeginDate.Text);
		}
		catch (FormatException)
		{
			lblErrorMessage.Text = BlogResources.ParseDateFailureMessage;

			return false;
		}
		catch (ArgumentNullException)
		{
			lblErrorMessage.Text = BlogResources.ParseDateFailureMessage;

			return false;
		}

		return true;
	}

	private void Save()
	{
		if (blog == null)
		{
			blog = new Blog(itemId);

			if ((blog.ItemId > -1) && (blog.ModuleId != moduleId))
			{
				SiteUtils.RedirectToAccessDeniedPage(this);

				return;
			}
		}

		Module module = GetModule(moduleId, Blog.FeatureGuid);

		if (module == null)
		{
			return;
		}

		if (currentUser == null)
		{
			return;
		}

		blog.UserGuid = currentUser.UserGuid;
		blog.LastModUserGuid = currentUser.UserGuid;
		blog.ContentChanged += new ContentChangedEventHandler(blog_ContentChanged);

		blog.ModuleId = moduleId;
		blog.ModuleGuid = module.ModuleGuid;
		DateTime localTime = DateTime.Parse(dpBeginDate.Text);

		if (timeZone != null)
		{
			blog.StartDate = localTime.ToUtc(timeZone);
		}
		else
		{
			blog.StartDate = localTime.AddHours(-timeOffset);
		}

		if (dpEndDate.Text.Length == 0)
		{
			blog.EndDate = DateTime.MaxValue;
		}
		else
		{
			DateTime localEndTime = DateTime.Parse(dpEndDate.Text);

			if (timeZone != null)
			{
				blog.EndDate = localEndTime.ToUtc(timeZone);
			}
			else
			{
				blog.EndDate = localEndTime.AddHours(-timeOffset);
			}
		}

		blog.Title = txtTitle.Text;
		blog.SubTitle = txtSubTitle.Text;
		blog.Location = txtLocation.Text;
		blog.Description = edContent.Text;
		blog.Excerpt = edExcerpt.Text;
		blog.UserName = Context.User.Identity.Name;
		blog.IncludeInFeed = this.chkIncludeInFeed.Checked;
		blog.IsPublished = chkIsPublished.Checked;

		int allowComentsForDays = -1;
		int.TryParse(ddCommentAllowedForDays.SelectedValue, out allowComentsForDays);

		blog.AllowCommentsForDays = allowComentsForDays;
		blog.MetaDescription = txtMetaDescription.Text;
		blog.MetaKeywords = txtMetaKeywords.Text;
		blog.ShowDownloadLink = chkShowDownloadLink.Checked;

		blog.UseBingMap = chkUseBing.Checked;
		blog.MapType = ((GMapTypeSetting)MapTypeControl).GetValue();

		int mapZoom = 13;
		int.TryParse(((GMapZoomLevelSetting)ZoomLevelControl).GetValue(), out mapZoom);

		blog.MapZoom = mapZoom;
		blog.MapHeight = txtMapHeight.Text;
		blog.MapWidth = txtMapWidth.Text;
		blog.ShowMapOptions = chkShowMapOptions.Checked;
		blog.ShowZoomTool = chkShowMapZoom.Checked;
		blog.ShowLocationInfo = chkShowMapBalloon.Checked;
		blog.UseDrivingDirections = chkShowMapDirections.Checked;
		blog.IncludeInSearch = chkIncludeInSearchIndex.Checked;
		blog.ExcludeFromRecentContent = chkExcludeFromRecentContent.Checked;
		blog.IncludeInSiteMap = chkIncludeInSiteMap.Checked;
		blog.ShowAuthorName = chkShowAuthorName.Checked;
		blog.ShowAuthorAvatar = chkShowAuthorAvatar.Checked;
		blog.ShowAuthorBio = chkShowAuthorBio.Checked;

		blog.IncludeInNews = chkIncludeInNews.Checked;
		blog.PubName = txtPublicationName.Text;
		blog.PubLanguage = txtPubLanguage.Text;
		blog.PubAccess = config.PublicationAccess;
		blog.PubGenres = txtPubGenres.Text;
		blog.PubGeoLocations = txtPubGeoLocations.Text;
		blog.PubKeyWords = txtPubKeyWords.Text;
		blog.PubStockTickers = txtPubStockTickers.Text;
		blog.HeadlineImageUrl = txtHeadlineImage.Text;


		if (blog.HeadlineImageUrl != currentFeaturedImagePath || string.IsNullOrWhiteSpace(blog.HeadlineImageUrl))
		{
			//update meta 
			var metas = metaRepository.FetchByContent(blog.BlogGuid);
			var filteredMetas = new List<ContentMeta>();

			if (string.IsNullOrWhiteSpace(currentFeaturedImagePath))
			{
				List<ContentMeta> metaTags;

				if (fileSystem.FileExists(blogMetaConfigFile))
					metaTags = GetJsonFile();
				else
					metaTags = JsonConvert.DeserializeObject<List<ContentMeta>>(blogMetaConfigDefault);

				metaTags = metaTags.Where(m => m.MetaContent == "{{image}}").ToList();
				filteredMetas = metas.Where(m => metaTags.Any(x => x.Name == m.Name)).ToList();
			}
			else
			{
				filteredMetas = metas.Where(m => m.MetaContent == SiteRoot + Page.ResolveUrl(currentFeaturedImagePath)).ToList();
			}

			foreach (ContentMeta meta in filteredMetas)
			{
				if (!string.IsNullOrWhiteSpace(blog.HeadlineImageUrl))
				{
					meta.MetaContent = SiteRoot + Page.ResolveUrl(blog.HeadlineImageUrl);
					metaRepository.Save(meta);
				}
				else
				{ //remove image meta with empty MetaContent
					metaRepository.Delete(meta.Guid);
				}
			}
		}

		//todo: is this needed?
		if (blog.HeadlineImageUrl.Length > 0)
		{
			imgPreview.ImageUrl = blog.HeadlineImageUrl;
		}

		blog.IncludeImageInExcerpt = chkIncludeImageInExcerpt.Checked;
		blog.IncludeImageInPost = chkIncludeImageInPost.Checked;

		if (txtItemUrl.Text.Length == 0)
		{
			txtItemUrl.Text = SuggestUrl();
		}

		string friendlyUrlString = SiteUtils.RemoveInvalidUrlChars(txtItemUrl.Text.Replace("~/", string.Empty));
		FriendlyUrl friendlyUrl = new FriendlyUrl(siteSettings.SiteId, friendlyUrlString);

		if (
			((friendlyUrl.FoundFriendlyUrl) && (friendlyUrl.PageGuid != blog.BlogGuid))
			&& (blog.ItemUrl != txtItemUrl.Text)
			)
		{
			lblError.Text = BlogResources.PageUrlInUseBlogErrorMessage;
			cancelRedirect = true;

			return;
		}

		if (!friendlyUrl.FoundFriendlyUrl)
		{
			if (WebPageInfo.IsPhysicalWebPage("~/" + friendlyUrlString))
			{
				lblError.Text = BlogResources.PageUrlInUseBlogErrorMessage;
				cancelRedirect = true;

				return;
			}
		}

		string oldUrl = blog.ItemUrl.Replace("~/", string.Empty);
		string newUrl = SiteUtils.RemoveInvalidUrlChars(txtItemUrl.Text.Replace("~/", string.Empty));

		blog.ItemUrl = "~/" + newUrl;

		if (enableContentVersioning)
		{
			blog.CreateHistory(siteSettings.SiteGuid);
		}

		blog.Save();

		// Check to see if this post is being created or edited
		if (itemId == -1)
		{
			CreateDefaultMetaTags();
		}

		// This must be below blog.Save() in order to have blog.ItemId set
		if (chkFeaturedPost.Checked && blog.IsPublished && blog.StartDate <= DateTime.UtcNow && blog.EndDate > DateTime.UtcNow)
		{
			ModuleSettings.UpdateModuleSetting(module.ModuleGuid, moduleId, "FeaturedPostId", blog.ItemId.ToString());
		}

		if (config.FeaturedPostId == blog.ItemId && !chkFeaturedPost.Checked)
		{
			ModuleSettings.UpdateModuleSetting(module.ModuleGuid, moduleId, "FeaturedPostId", "0");
		}

		if (!friendlyUrl.FoundFriendlyUrl)
		{
			if ((friendlyUrlString.Length > 0) && (!WebPageInfo.IsPhysicalWebPage("~/" + friendlyUrlString)))
			{
				FriendlyUrl newFriendlyUrl = new FriendlyUrl
				{
					SiteId = siteSettings.SiteId,
					SiteGuid = siteSettings.SiteGuid,
					PageGuid = blog.BlogGuid,
					Url = friendlyUrlString,
					RealUrl = SiteUtils.GetUrlWithQueryParams("~/Blog/ViewPost.aspx", -1, pageId, blog.ModuleId, blog.ItemId, false)
					//$"~/Blog/ViewPost.aspx?pageid={pageId.ToInvariantString()}&mid={blog.ModuleId.ToInvariantString()}&ItemID={blog.ItemId.ToInvariantString()}"
				};

				newFriendlyUrl.Save();
			}

			//if post was renamed url will change, if url changes we need to redirect from the old url to the new with 301
			if ((oldUrl.Length > 0) && (newUrl.Length > 0) && (!SiteUtils.UrlsMatch(oldUrl, newUrl)) && BlogConfiguration.Create301OnPostRename)
			{
				//worry about the risk of a redirect loop if the page is restored to the old url again
				// don't create it if a redirect for the new url exists
				if (
					(!RedirectInfo.Exists(siteSettings.SiteId, oldUrl))
					&& (!RedirectInfo.Exists(siteSettings.SiteId, newUrl))
					)
				{
					RedirectInfo redirect = new RedirectInfo
					{
						SiteGuid = siteSettings.SiteGuid,
						SiteId = siteSettings.SiteId,
						OldUrl = oldUrl,
						NewUrl = newUrl
					};
					redirect.Save();
				}

				// since we have created a redirect we don't need the old friendly url
				var oldFriendlyUrl = new FriendlyUrl(siteSettings.SiteId, oldUrl);

				if (oldFriendlyUrl.FoundFriendlyUrl && oldFriendlyUrl.PageGuid == blog.BlogGuid)
				{
					FriendlyUrl.DeleteUrl(oldFriendlyUrl.UrlId);
				}
			}
		}

		// new item posted so ping services
		if ((itemId == -1) && blog.IsPublished && (blog.StartDate <= DateTime.UtcNow))
		{
			QueuePings();
		}

		CurrentPage.UpdateLastModifiedTime();

		Blog.DeleteItemCategories(blog.ItemId);

		// Mono doesn't see this in update panel
		// so help find it
		if (chkCategories == null)
		{
			log.Error("chkCategories was null");

			chkCategories = (CheckBoxList)UpdatePanel1.FindControl("chkCategories");
		}

		foreach (ListItem listItem in chkCategories.Items)
		{
			if (listItem.Selected)
			{
				int categoryId;

				if (int.TryParse(listItem.Value, out categoryId))
				{
					Blog.AddItemCategory(blog.ItemId, categoryId);
				}
			}
		}

		CacheHelper.ClearModuleCache(moduleId);
		SiteUtils.QueueIndexing();
	}


	void CreateDefaultMetaTags()
	{
		if (fileSystem.FileExists(blogMetaConfigFile))
		{
			List<ContentMeta> metaTags = GetJsonFile();

			CreateItems(metaTags);
		}
		else
		{
			CreateItems(JsonConvert.DeserializeObject<List<ContentMeta>>(blogMetaConfigDefault));
		}

		void CreateItems(List<ContentMeta> metaTags)
		{
			foreach (ContentMeta tag in metaTags)
			{
				int truncateLength = 155;
				bool useMeta = true;
				switch (tag.MetaContent)
				{
					case "{{site-name}}":
						tag.MetaContent = siteSettings.SiteName;
						break;

					case "{{title}}":
						tag.MetaContent = blog.Title;
						break;

					case "{{description}}":
						string uncleanMarkup = null;

						if (!string.IsNullOrWhiteSpace(blog.Excerpt))
						{
							uncleanMarkup = blog.Excerpt;
						}
						else
						{
							uncleanMarkup = blog.Description;
						}

						// Remove markup and whitespace characters
						string cleanedOfMarkup = Regex.Replace(
							HttpUtility.HtmlDecode(SecurityHelper.RemoveMarkup(uncleanMarkup)),
							@"\s+",
							" "
						);

						cleanedOfMarkup =
							(cleanedOfMarkup.Length <= truncateLength) ?
							cleanedOfMarkup.Trim() :
							cleanedOfMarkup.Substring(0, truncateLength).Trim()
						;

						tag.MetaContent = HttpUtility.HtmlDecode(SecurityHelper.RemoveMarkup(cleanedOfMarkup));
						break;

					case "{{image}}":
						if (!string.IsNullOrWhiteSpace(blog.HeadlineImageUrl))
						{
							tag.MetaContent = SiteRoot + Page.ResolveUrl(blog.HeadlineImageUrl);
						}
						else
						{
							useMeta = false;
							tag.MetaContent = string.Empty;
						}
						break;

					case "{{url}}":
						tag.MetaContent = SiteRoot + Page.ResolveUrl(blog.ItemUrl);
						break;

						// The is for Twitter, it expects the @organisation of the person who wrote the article.
						//case "{{site}}":
						//	tag.MetaContent = "";
						//	break;

						// This is for Twitter, it expects the Twitter @username which isn't in mojo ATM.
						//case "{{creator}}":
						//	if (siteUser != null)
						//	{
						//		tag.MetaContent = siteUser.Name;
						//	}
						//	break;
				}

				if (useMeta) //don't want to add meta with empty MetaContent
				{
					createMetaEntry(
						new Guid(),
						tag.NameProperty,
						tag.Name,
						tag.ContentProperty,
						tag.MetaContent,
						tag.Scheme,
						tag.LangCode,
						tag.Dir
					);
				}
			}
		}
	}


	private List<ContentMeta> GetJsonFile()
	{
		using (StreamReader r = new StreamReader(HttpContext.Current.Server.MapPath(blogMetaConfigFile)))
		{
			string json = r.ReadToEnd();

			return JsonConvert.DeserializeObject<List<ContentMeta>>(json);
		}
	}


	private string SuggestUrl()
	{
		string pageName = config.DefaultUrlPrefix + txtTitle.Text;

		if (WebConfigSettings.AppendDateToBlogUrls)
		{
			if (timeZone != null)
			{
				pageName += "-" + DateTime.UtcNow.ToLocalTime(timeZone).ToString("yyyy-MM-dd");
			}
			else
			{
				pageName += "-" + DateTime.UtcNow.AddHours(timeOffset).ToString("yyyy-MM-dd");
			}
		}

		return SiteUtils.SuggestFriendlyUrl(pageName, siteSettings);
	}


	void blog_ContentChanged(object sender, ContentChangedEventArgs e)
	{
		IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["BlogIndexBuilderProvider"];

		if (indexBuilder != null)
		{
			indexBuilder.ContentChangedHandler(sender, e);
		}
	}


	#region Meta Data


	private void BindMeta()
	{
		if (blog == null)
		{
			return;
		}

		if (blog.BlogGuid == Guid.Empty)
		{
			return;
		}

		List<ContentMeta> meta = metaRepository.FetchByContent(blog.BlogGuid);

		grdContentMeta.DataSource = meta;
		grdContentMeta.DataBind();

		btnAddMeta.Visible = true;
	}


	void grdContentMeta_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		if (blog == null)
		{
			return;
		}

		if (blog.BlogGuid == Guid.Empty)
		{
			return;
		}

		GridView grid = (GridView)sender;
		string sGuid = e.CommandArgument.ToString();

		if (sGuid.Length != 36)
		{
			return;
		}

		Guid guid = new Guid(sGuid);
		ContentMeta meta = metaRepository.Fetch(guid);

		if (meta == null)
		{
			return;
		}

		switch (e.CommandName)
		{
			case "MoveUp":
				meta.SortRank -= 3;
				break;

			case "MoveDown":
				meta.SortRank += 3;
				break;
		}

		metaRepository.Save(meta);

		List<ContentMeta> metaList = metaRepository.FetchByContent(blog.BlogGuid);

		metaRepository.ResortMeta(metaList);

		blog.CompiledMeta = metaRepository.GetMetaString(blog.BlogGuid);
		blog.Save();

		BindMeta();
		upMeta.Update();
	}


	void grdContentMeta_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		if (blog == null)
		{
			return;
		}

		if (blog.BlogGuid == Guid.Empty)
		{
			return;
		}

		GridView grid = (GridView)sender;
		Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());

		metaRepository.Delete(guid);

		blog.CompiledMeta = metaRepository.GetMetaString(blog.BlogGuid);
		blog.Save();
		grdContentMeta.Columns[2].Visible = true;
		BindMeta();
		upMeta.Update();
	}


	void grdContentMeta_RowEditing(object sender, GridViewEditEventArgs e)
	{
		GridView grid = (GridView)sender;

		grid.EditIndex = e.NewEditIndex;

		BindMeta();

		Button btnDeleteMeta = (Button)grid.Rows[e.NewEditIndex].Cells[1].FindControl("btnDeleteMeta");

		if (btnDeleteMeta != null)
		{
			btnDeleteMeta.Attributes.Add("OnClick", $"return confirm('{BlogResources.ContentMetaDeleteWarning}');");
		}

		upMeta.Update();
	}


	void grdContentMeta_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		GridView grid = (GridView)sender;

		if (grid.EditIndex > -1)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				DropDownList ddDirection = (DropDownList)e.Row.Cells[1].FindControl("ddDirection");

				if (ddDirection != null)
				{
					if (e.Row.DataItem is ContentMeta)
					{
						ListItem item = ddDirection.Items.FindByValue(((ContentMeta)e.Row.DataItem).Dir);

						if (item != null)
						{
							ddDirection.ClearSelection();
							item.Selected = true;
						}
					}
				}

				if (!(e.Row.DataItem is ContentMeta))
				{
					//the add button was clicked so hide the delete button
					Button btnDeleteMeta = (Button)e.Row.Cells[1].FindControl("btnDeleteMeta");

					if (btnDeleteMeta != null)
					{
						btnDeleteMeta.Visible = false;
					}
				}
			}
		}
	}


	void grdContentMeta_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		if (blog == null) { return; }
		if (blog.BlogGuid == Guid.Empty) { return; }

		GridView grid = (GridView)sender;

		Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());

		TextBox txtName = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtName");
		TextBox txtNameProperty = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtNameProperty");
		TextBox txtMetaContentProperty = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtMetaContentProperty");
		TextBox txtMetaContent = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtMetaContent");
		TextBox txtScheme = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtScheme");
		TextBox txtLangCode = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtLangCode");
		DropDownList ddDirection = (DropDownList)grid.Rows[e.RowIndex].Cells[1].FindControl("ddDirection");

		createMetaEntry(
			guid,
			txtNameProperty.Text,
			txtName.Text,
			txtMetaContentProperty.Text,
			txtMetaContent.Text,
			txtScheme.Text,
			txtLangCode.Text,
			ddDirection.SelectedValue
		);

		grid.EditIndex = -1;
		grdContentMeta.Columns[2].Visible = true;

		BindMeta();

		upMeta.Update();
	}


	void createMetaEntry(
		Guid guid,
		string nameProperty,
		string name,
		string contentProperty,
		string content,
		string scheme,
		string langCode,
		string direction
	)
	{
		ContentMeta meta = null;

		if (guid != Guid.Empty)
		{
			meta = metaRepository.Fetch(guid);
		}
		else
		{
			meta = new ContentMeta();
			Module module = new Module(moduleId);

			meta.ModuleGuid = module.ModuleGuid;

			if (siteUser != null)
			{
				meta.CreatedBy = siteUser.UserGuid;
			}

			meta.SortRank = metaRepository.GetNextSortRank(blog.BlogGuid);
		}

		if (meta != null)
		{
			meta.SiteGuid = siteSettings.SiteGuid;
			meta.ContentGuid = blog.BlogGuid;

			meta.Name = name;
			meta.NameProperty = nameProperty;
			meta.MetaContent = content;
			meta.ContentProperty = contentProperty;
			meta.Scheme = scheme;
			meta.LangCode = langCode;
			meta.Dir = direction;

			if (siteUser != null)
			{
				meta.LastModBy = siteUser.UserGuid;
			}

			metaRepository.Save(meta);

			blog.CompiledMeta = metaRepository.GetMetaString(blog.BlogGuid);
			blog.Save();
		}
	}


	void grdContentMeta_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		grdContentMeta.EditIndex = -1;
		grdContentMeta.Columns[2].Visible = true;

		BindMeta();

		upMeta.Update();
	}


	void btnAddMeta_Click(object sender, EventArgs e)
	{
		DataTable dataTable = new DataTable();

		dataTable.Columns.Add("Guid", typeof(Guid));
		dataTable.Columns.Add("SiteGuid", typeof(Guid));
		dataTable.Columns.Add("ModuleGuid", typeof(Guid));
		dataTable.Columns.Add("ContentGuid", typeof(Guid));
		dataTable.Columns.Add("Name", typeof(string));
		dataTable.Columns.Add("NameProperty", typeof(string));
		dataTable.Columns.Add("MetaContent", typeof(string));
		dataTable.Columns.Add("ContentProperty", typeof(string));
		dataTable.Columns.Add("Scheme", typeof(string));
		dataTable.Columns.Add("LangCode", typeof(string));
		dataTable.Columns.Add("Dir", typeof(string));
		dataTable.Columns.Add("SortRank", typeof(int));

		DataRow row = dataTable.NewRow();

		row["Guid"] = Guid.Empty;
		row["SiteGuid"] = siteSettings.SiteGuid;
		row["ModuleGuid"] = Guid.Empty;
		row["ContentGuid"] = Guid.Empty;
		row["Name"] = string.Empty;
		row["NameProperty"] = "name";
		row["MetaContent"] = string.Empty;
		row["ContentProperty"] = "content";
		row["Scheme"] = string.Empty;
		row["LangCode"] = string.Empty;
		row["Dir"] = string.Empty;
		row["SortRank"] = 3;

		dataTable.Rows.Add(row);

		grdContentMeta.EditIndex = 0;
		grdContentMeta.DataSource = dataTable.DefaultView;
		grdContentMeta.DataBind();
		//grdContentMeta.Columns[2].Visible = false;
		btnAddMeta.Visible = false;

		upMeta.Update();
	}


	private void BindMetaLinks()
	{
		if (blog == null)
		{
			return;
		}

		if (blog.BlogGuid == Guid.Empty)
		{
			return;
		}

		List<ContentMetaLink> meta = metaRepository.FetchLinksByContent(blog.BlogGuid);

		grdMetaLinks.DataSource = meta;
		grdMetaLinks.DataBind();

		btnAddMetaLink.Visible = true;
	}

	void btnAddMetaLink_Click(object sender, EventArgs e)
	{
		DataTable dataTable = new DataTable();

		dataTable.Columns.Add("Guid", typeof(Guid));
		dataTable.Columns.Add("SiteGuid", typeof(Guid));
		dataTable.Columns.Add("ModuleGuid", typeof(Guid));
		dataTable.Columns.Add("ContentGuid", typeof(Guid));
		dataTable.Columns.Add("Rel", typeof(string));
		dataTable.Columns.Add("Href", typeof(string));
		dataTable.Columns.Add("HrefLang", typeof(string));
		dataTable.Columns.Add("SortRank", typeof(int));

		DataRow row = dataTable.NewRow();

		row["Guid"] = Guid.Empty;
		row["SiteGuid"] = siteSettings.SiteGuid;
		row["ModuleGuid"] = Guid.Empty;
		row["ContentGuid"] = Guid.Empty;
		row["Rel"] = string.Empty;
		row["Href"] = string.Empty;
		row["HrefLang"] = string.Empty;
		row["SortRank"] = 3;

		dataTable.Rows.Add(row);

		grdMetaLinks.Columns[2].Visible = false;
		grdMetaLinks.EditIndex = 0;
		grdMetaLinks.DataSource = dataTable.DefaultView;
		grdMetaLinks.DataBind();
		btnAddMetaLink.Visible = false;

		updMetaLinks.Update();
	}


	void grdMetaLinks_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		GridView grid = (GridView)sender;
		if (grid.EditIndex > -1)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				if (!(e.Row.DataItem is ContentMetaLink))
				{
					//the add button was clicked so hide the delete button
					Button btnDeleteMetaLink = (Button)e.Row.Cells[1].FindControl("btnDeleteMetaLink");

					if (btnDeleteMetaLink != null)
					{
						btnDeleteMetaLink.Visible = false;
					}
				}
			}
		}
	}


	void grdMetaLinks_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		if (blog == null)
		{
			return;
		}

		if (blog.BlogGuid == Guid.Empty)
		{
			return;
		}

		GridView grid = (GridView)sender;
		Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());

		metaRepository.DeleteLink(guid);

		blog.CompiledMeta = metaRepository.GetMetaString(blog.BlogGuid);
		blog.Save();

		grid.Columns[2].Visible = true;
		BindMetaLinks();

		updMetaLinks.Update();
	}


	void grdMetaLinks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		grdMetaLinks.EditIndex = -1;
		grdMetaLinks.Columns[2].Visible = true;

		BindMetaLinks();
		updMetaLinks.Update();
	}


	void grdMetaLinks_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		if (blog == null) { return; }
		if (blog.BlogGuid == Guid.Empty) { return; }

		GridView grid = (GridView)sender;

		Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
		TextBox txtRel = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtRel");
		TextBox txtHref = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtHref");
		TextBox txtHrefLang = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtHrefLang");
		SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
		ContentMetaLink meta = null;
		if (guid != Guid.Empty)
		{
			meta = metaRepository.FetchLink(guid);
		}
		else
		{
			meta = new ContentMetaLink();
			Module module = new Module(moduleId);
			meta.ModuleGuid = module.ModuleGuid;
			if (currentUser != null) { meta.CreatedBy = currentUser.UserGuid; }
			meta.SortRank = metaRepository.GetNextLinkSortRank(blog.BlogGuid);
		}

		if (meta != null)
		{
			meta.SiteGuid = siteSettings.SiteGuid;
			meta.ContentGuid = blog.BlogGuid;
			meta.Rel = txtRel.Text;
			meta.Href = txtHref.Text;
			meta.HrefLang = txtHrefLang.Text;

			if (currentUser != null) { meta.LastModBy = currentUser.UserGuid; }
			metaRepository.Save(meta);

			blog.CompiledMeta = metaRepository.GetMetaString(blog.BlogGuid);
			blog.Save();

		}

		grid.EditIndex = -1;
		grdMetaLinks.Columns[2].Visible = true;
		BindMetaLinks();
		updMetaLinks.Update();
	}

	void grdMetaLinks_RowEditing(object sender, GridViewEditEventArgs e)
	{
		GridView grid = (GridView)sender;
		grid.EditIndex = e.NewEditIndex;

		BindMetaLinks();

		Guid guid = new Guid(grid.DataKeys[grid.EditIndex].Value.ToString());

		Button btnDelete = (Button)grid.Rows[e.NewEditIndex].Cells[1].FindControl("btnDeleteMetaLink");
		if (btnDelete != null)
		{
			btnDelete.Attributes.Add("OnClick", $"return confirm(\"{BlogResources.ContentMetaLinkDeleteWarning}\");");

			if (guid == Guid.Empty) { btnDelete.Visible = false; }
		}

		updMetaLinks.Update();
	}

	void grdMetaLinks_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		if (blog == null) { return; }
		if (blog.BlogGuid == Guid.Empty) { return; }

		GridView grid = (GridView)sender;
		string sGuid = e.CommandArgument.ToString();
		if (sGuid.Length != 36) { return; }

		Guid guid = new Guid(sGuid);
		ContentMetaLink meta = metaRepository.FetchLink(guid);
		if (meta == null) { return; }

		switch (e.CommandName)
		{
			case "MoveUp":
				meta.SortRank -= 3;
				break;

			case "MoveDown":
				meta.SortRank += 3;
				break;

		}

		metaRepository.Save(meta);
		List<ContentMetaLink> metaList = metaRepository.FetchLinksByContent(blog.BlogGuid);
		metaRepository.ResortMeta(metaList);

		blog.CompiledMeta = metaRepository.GetMetaString(blog.BlogGuid);
		blog.Save();

		BindMetaLinks();
		updMetaLinks.Update();
	}


	#endregion

	#region History

	private void BindHistory()
	{
		if (!enableContentVersioning) { return; }

		if ((blog == null) || (blog.ItemId == -1))
		{
			pnlHistory.Visible = false;
			return;
		}

		List<ContentHistory> history = ContentHistory.GetPage(blog.BlogGuid, pageNumber, pageSize, out totalPages);

		pgrHistory.ShowFirstLast = true;
		pgrHistory.PageSize = pageSize;
		pgrHistory.PageCount = totalPages;
		pgrHistory.Visible = (this.totalPages > 1);

		grdHistory.DataSource = history;
		grdHistory.DataBind();

		btnDeleteHistory.Visible = (grdHistory.Rows.Count > 0);
		pnlHistory.Visible = (grdHistory.Rows.Count > 0);
		updHx.Update();

	}

	void pgrHistory_Command(object sender, CommandEventArgs e)
	{
		pageNumber = Convert.ToInt32(e.CommandArgument);
		pgrHistory.CurrentIndex = pageNumber;
		BindHistory();
	}

	void grdHistory_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		string g = e.CommandArgument.ToString();
		if (g.Length != 36) { return; }
		Guid historyGuid = new Guid(g);

		switch (e.CommandName)
		{
			case "RestoreToEditor":
				ContentHistory history = new ContentHistory(historyGuid);
				if (history.Guid == Guid.Empty) { return; }

				edContent.Text = history.ContentText;
				BindHistory();
				break;

			case "DeleteHistory":
				ContentHistory.Delete(historyGuid);
				BindHistory();
				break;

			default:

				break;
		}
	}

	void grdHistory_RowDataBound(object sender, GridViewRowEventArgs e)
	{

		Button btnDelete = (Button)e.Row.Cells[0].FindControl("btnDelete");
		if (btnDelete != null)
		{
			btnDelete.Attributes.Add("OnClick", $"return confirm(\"{BlogResources.DeleteHistoryItemWarning}\");");
		}

	}

	void btnRestoreFromGreyBox_Click(object sender, ImageClickEventArgs e)
	{
		if (hdnHxToRestore.Value.Length != 36)
		{
			BindHistory();
			return;
		}

		Guid h = new Guid(hdnHxToRestore.Value);

		ContentHistory history = new ContentHistory(h);
		if (history.Guid == Guid.Empty) { return; }

		edContent.Text = history.ContentText;
		BindHistory();

	}

	void btnDeleteHistory_Click(object sender, EventArgs e)
	{
		if (blog == null) { return; }

		ContentHistory.DeleteByContent(blog.BlogGuid);
		BindHistory();

	}

	#endregion

	private void DoPings(object pingersList)
	{

		if (!(pingersList is List<ServicePinger>)) return;

		List<ServicePinger> pingers = pingersList as List<ServicePinger>;
		foreach (ServicePinger pinger in pingers)
		{
			pinger.Ping();
		}

	}

	protected virtual void QueuePings()
	{
		// TODO: implement more generic support with lookup of pingable services

		//if (config.OdiogoFeedId.Length == 0) return;

		//string odogioRpcUrl = "http://rpc.odiogo.com/ping/";
		//ServicePinger pinger = new ServicePinger(
		//	siteSettings.SiteName,
		//	SiteRoot,
		//	odogioRpcUrl);

		//List<ServicePinger> pingers = new List<ServicePinger>();
		//pingers.Add(pinger);


		//if (!ThreadPool.QueueUserWorkItem(new WaitCallback(DoPings), pingers))
		//{
		//	throw new Exception("Couldn't queue the DoPings on a new thread.");
		//}
	}



	protected void btnCancel_Click(object sender, EventArgs e)
	{
		if (hdnReturnUrl.Value.Length > 0)
		{
			WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
			return;
		}

		WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

		return;

	}

	protected void btnDelete_Click(object sender, EventArgs e)
	{
		if (blog != null)
		{
			if (blog.ItemId == config.FeaturedPostId)
			{
				Module module = GetModule(moduleId, Blog.FeatureGuid);
				ModuleSettings.UpdateModuleSetting(module.ModuleGuid, moduleId, "FeaturedPostId", "0");
			}
			blog.ContentChanged += new ContentChangedEventHandler(blog_ContentChanged);
			blog.Delete();
			FriendlyUrl.DeleteByPageGuid(blog.BlogGuid);
			CurrentPage.UpdateLastModifiedTime();
			SiteUtils.QueueIndexing();

			CommentRepository commentRepository = new CommentRepository();
			commentRepository.DeleteByContent(blog.BlogGuid);

			FileAttachment.DeleteByItem(blog.BlogGuid);
		}

		//if (hdnReturnUrl.Value.Length > 0)
		//{
		//    WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
		//    return;
		//}

		WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

		return;
	}

	private void PopulateCommentDaysDropdown()
	{
		ListItem item = ddCommentAllowedForDays.Items.FindByValue(config.DefaultCommentDaysAllowed.ToInvariantString());
		if (item != null)
		{
			ddCommentAllowedForDays.ClearSelection();
			item.Selected = true;
		}
	}

	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, BlogResources.EditPostPageTitle);

		heading.Text = BlogResources.BlogEditEntryLabel;

		litContentTab.Text = BlogResources.ContentTab;
		litMetaTab.Text = BlogResources.MetaTab;
		litMapSettingsTab.Text = BlogResources.MapSettings;
		litExcerptTab.Text = "<a href='#" + tabExcerpt.ClientID + "'>" + BlogResources.ExcerptTab + "</a>";


		liExcerpt.Visible = config.UseExcerpt || config.UseExcerptInFeed;
		tabExcerpt.Visible = config.UseExcerpt || config.UseExcerptInFeed;

		// Featured Image Tab
		litFeaturedImageTab.Text = BlogResources.FeaturedImageTab;

		litAttachmentsTab.Text = "<a href='#" + tabAttachments.ClientID + "'>" + BlogResources.Attachments + "</a>";
		tabAttachments.Visible = BlogConfiguration.AllowAttachments;
		liAttachment.Visible = BlogConfiguration.AllowAttachments;


		litGoogleNewsSettingsTab.Text = "<a href='#" + divTabGoogleNews.ClientID + "'>" + BlogResources.GoogleNewsSettings + "</a>";
		divTabGoogleNews.Visible = config.ShowGoogleNewsTabInEditPage;
		liGoogleNewsSettigns.Visible = config.ShowGoogleNewsTabInEditPage;

		fbHeadlineImage.TextBoxClientId = txtHeadlineImage.ClientID;
		fbHeadlineImage.PreviewImageClientId = imgPreview.ClientID;
		fbHeadlineImage.Text = LinkResources.Browse;

		if (!Page.IsPostBack)
		{
			txtPublicationName.Text = config.DefaultPublicationName;
			txtPubLanguage.Text = config.DefaultPublicationLanguage;
			txtPubGenres.Text = config.DefaultGenres;
			chkIncludeInNews.Checked = config.DefaultIncludeInNewsChecked;
			chkIncludeImageInExcerpt.Checked = config.DefaultIncludeImageInExcerptChecked;
			chkIncludeImageInPost.Checked = config.DefaultIncludeImageInPostChecked;

			PopulateCommentDaysDropdown();
		}

		edContent.WebEditor.ToolBar = ToolBar.FullWithTemplates;
		edExcerpt.WebEditor.ToolBar = ToolBar.FullWithTemplates;

		this.lnkEditCategories.NavigateUrl = SiteRoot + "/Blog/EditCategory.aspx?pageid=" + CurrentPage.PageId.ToInvariantString()
			+ "&mid=" + this.moduleId.ToInvariantString();

		this.lnkEditCategories.Text = BlogResources.BlogEditCategoriesLabel;


		edContent.WebEditor.Height = config.EditorHeight;
		edExcerpt.WebEditor.Height = config.EditorHeight;


		btnUpdate.Text = BlogResources.BlogEditUpdateButton;
		SiteUtils.SetButtonAccessKey(btnUpdate, BlogResources.BlogEditUpdateButtonAccessKey);
		//btnUpdate2.Text = BlogResources.BlogEditUpdateButton;
		//btnUpdate3.Text = BlogResources.BlogEditUpdateButton;
		//btnUpdate4.Text = BlogResources.BlogEditUpdateButton;
		//btnUpdate5.Text = BlogResources.BlogEditUpdateButton;
		btnSaveAndPreview.Text = BlogResources.SaveAndPreviewButton;

		//this resets the exit page prompt after an ajax update for categories
		ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(UpdatePanel),
			  "requireExitPrompt", "\n<script type=\"text/javascript\">\n requireExitPrompt = true;  \n</script>", false);

		UIHelper.DisableButtonAfterClickAndClearExitCode(
			btnUpdate,
			BlogResources.ButtonDisabledPleaseWait,
			Page.ClientScript.GetPostBackEventReference(this.btnUpdate, string.Empty)
			);

		//UIHelper.DisableButtonAfterClickAndClearExitCode(
		//	btnUpdate2,
		//	BlogResources.ButtonDisabledPleaseWait,
		//	Page.ClientScript.GetPostBackEventReference(this.btnUpdate2, string.Empty)
		//	);

		//UIHelper.DisableButtonAfterClickAndClearExitCode(
		//	btnUpdate3,
		//	BlogResources.ButtonDisabledPleaseWait,
		//	Page.ClientScript.GetPostBackEventReference(this.btnUpdate3, string.Empty)
		//	);

		//UIHelper.DisableButtonAfterClickAndClearExitCode(
		//	btnUpdate4,
		//	BlogResources.ButtonDisabledPleaseWait,
		//	Page.ClientScript.GetPostBackEventReference(this.btnUpdate4, string.Empty)
		//	);

		//UIHelper.DisableButtonAfterClickAndClearExitCode(
		//	btnUpdate5,
		//	BlogResources.ButtonDisabledPleaseWait,
		//	Page.ClientScript.GetPostBackEventReference(this.btnUpdate4, string.Empty)
		//	);

		UIHelper.DisableButtonAfterClickAndClearExitCode(
			btnSaveAndPreview,
			BlogResources.ButtonDisabledPleaseWait,
			Page.ClientScript.GetPostBackEventReference(btnSaveAndPreview, string.Empty)
			);

		lnkCancel.Text = BlogResources.BlogEditCancelButton;
		//lnkCancel2.Text = BlogResources.BlogEditCancelButton;
		//lnkCancel3.Text = BlogResources.BlogEditCancelButton;
		//lnkCancel5.Text = BlogResources.BlogEditCancelButton;
		//btnDelete.Text = BlogResources.BlogEditDeleteButton;
		//btnDelete2.Text = BlogResources.BlogEditDeleteButton;
		//btnDelete3.Text = BlogResources.BlogEditDeleteButton;
		//btnDelete4.Text = BlogResources.BlogEditDeleteButton;
		//btnDelete5.Text = BlogResources.BlogEditDeleteButton;
		SiteUtils.SetButtonAccessKey(btnDelete, BlogResources.BlogEditDeleteButtonAccessKey);
		UIHelper.AddConfirmationDialogWithClearExitCode(btnDelete, BlogResources.BlogDeletePostWarning);
		//UIHelper.AddConfirmationDialogWithClearExitCode(btnDelete2, BlogResources.BlogDeletePostWarning);
		//UIHelper.AddConfirmationDialogWithClearExitCode(btnDelete3, BlogResources.BlogDeletePostWarning);
		//UIHelper.AddConfirmationDialogWithClearExitCode(btnDelete4, BlogResources.BlogDeletePostWarning);

		btnAddCategory.Text = BlogResources.BlogAddCategoryButton;
		SiteUtils.SetButtonAccessKey(btnAddCategory, BlogResources.BlogAddCategoryButtonAccessKey);
		UIHelper.AddClearPageExitCode(btnAddCategory);

		reqTitle.ErrorMessage = BlogResources.TitleRequiredWarning;
		reqStartDate.ErrorMessage = BlogResources.BlogBeginDateRequiredHelp;
		this.dpBeginDate.ClockHours = ConfigurationManager.AppSettings["ClockHours"];
		regexUrl.ErrorMessage = BlogResources.FriendlyUrlRegexWarning;

		//if (!showCategories)
		//{
		//    pnlCategories.Visible = false;
		//}

		litDays.Text = BlogResources.BlogEditCommentsDaysLabel;

		grdHistory.Columns[0].HeaderText = BlogResources.CreatedDateGridHeader;
		grdHistory.Columns[1].HeaderText = BlogResources.ArchiveDateGridHeader;

		btnRestoreFromGreyBox.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/1x1.gif");
		btnRestoreFromGreyBox.Attributes.Add("tabIndex", "-1");
		btnRestoreFromGreyBox.AlternateText = " ";

		btnDeleteHistory.Text = BlogResources.DeleteAllHistoryButton;
		UIHelper.AddConfirmationDialog(btnDeleteHistory, BlogResources.DeleteAllHistoryWarning);

		btnAddMeta.Text = BlogResources.AddMetaButton;
		grdContentMeta.Columns[0].HeaderText = string.Empty;
		grdContentMeta.Columns[1].HeaderText = BlogResources.MetaNameProperty;
		grdContentMeta.Columns[2].HeaderText = BlogResources.ContentMetaNameLabel;
		grdContentMeta.Columns[3].HeaderText = BlogResources.MetaContentProperty;
		grdContentMeta.Columns[4].HeaderText = BlogResources.ContentMetaContentLabel;
		grdContentMeta.Columns[5].HeaderText = BlogResources.ContentMetaSchemeLabel;
		grdContentMeta.Columns[6].HeaderText = BlogResources.ContentMetaLangCodeLabel;
		grdContentMeta.Columns[7].HeaderText = BlogResources.ContentMetaDirLabel;

		btnAddMetaLink.Text = BlogResources.AddMetaLinkButton;
		grdMetaLinks.Columns[0].HeaderText = string.Empty;
		grdMetaLinks.Columns[1].HeaderText = BlogResources.ContentMetaRelLabel;
		grdMetaLinks.Columns[2].HeaderText = BlogResources.ContentMetaHrefLabel;


		btnUpload.Text = BlogResources.Upload;
		litAttachmentWarning.Text = "<p>" + BlogResources.AttachmentRequiresSaveInfo + "</p>";

		regexMapHeight.ErrorMessage = BlogResources.MapHeightRegexWarning;
		regexMapWidth.ErrorMessage = BlogResources.MapWidthRegexWarning;

		Control c = Master.FindControl("Breadcrumbs");
		if (c != null)
		{
			BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
			crumbs.ForceShowBreadcrumbs = true;
			crumbs.AddedCrumbs
				= crumbs.ItemWrapperTop + "<a href='" + SiteRoot + "/Blog/Manage.aspx?pageid="
				+ pageId.ToInvariantString()
				+ "&amp;mid=" + moduleId.ToInvariantString()
				+ "' class='selectedcrumb'>" + BlogResources.Administration
				+ "</a>" + crumbs.ItemWrapperBottom;
		}

		// borowing these from Image Gallery feature instead of replicating them
		uploader.AddFileText = GalleryResources.SelectFileButton;
		uploader.DropFileText = BlogResources.DropFile;
		uploader.DropFilesText = BlogResources.DropFiles;
		uploader.UploadButtonText = GalleryResources.BulkUploadButton;
		uploader.UploadCompleteText = GalleryResources.UploadComplete;
		uploader.UploadingText = GalleryResources.Uploading;

		if (!IsPostBack)
		{
			// apply some defaults to checkboxes
			chkIncludeInFeed.Checked = BlogConfiguration.IncludeInFeedCheckedByDefault;
			chkIncludeInSearchIndex.Checked = BlogConfiguration.IncludeInSearchIndexCheckedByDefault;
			chkExcludeFromRecentContent.Checked = BlogConfiguration.ExcludeFromRecentContentCheckedByDefault;
			chkIncludeInSiteMap.Checked = BlogConfiguration.IncludeInSiteMapCheckedByDefault;
			chkIsPublished.Checked = BlogConfiguration.IsPublishedCheckedByDefault;
			chkShowAuthorName.Checked = BlogConfiguration.ShowAuthorNameCheckedByDefault;
			chkShowAuthorAvatar.Checked = BlogConfiguration.ShowAuthorAvatarCheckedByDefault;
			chkShowAuthorBio.Checked = BlogConfiguration.ShowAuthorBioCheckedByDefault;
		}
	}

	private void LoadSettings()
	{
		if ((WebUser.IsAdminOrContentAdmin) || (SiteUtils.UserIsSiteEditor())) { isAdmin = true; }

		txtTitle.MaxLength = BlogConfiguration.PostTitleMaxLength;

		currentUser = SiteUtils.GetCurrentSiteUser();

		ScriptConfig.IncludeColorBox = true;

		Hashtable moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
		config = new BlogConfiguration(moduleSettings);
		useFriendlyUrls = BlogConfiguration.UseFriendlyUrls(moduleId);

		if (!useFriendlyUrls)
		{
			divUrl.Attributes.Add("style", "display:none;");
		}
		lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
		//lnkCancel2.NavigateUrl = lnkCancel.NavigateUrl;
		//lnkCancel3.NavigateUrl = lnkCancel.NavigateUrl;
		//lnkCancel5.NavigateUrl = lnkCancel.NavigateUrl;

		enableContentVersioning = config.EnableContentVersioning;

		if ((siteSettings.ForceContentVersioning) || (WebConfigSettings.EnforceContentVersioningGlobally))
		{
			enableContentVersioning = true;
		}

		SiteUtils.EnsureFileAttachmentFolder(siteSettings);
		upLoadPath = SiteUtils.GetFileAttachmentUploadPath();

		if (itemId > -1)
		{
			blog = new Blog(itemId);
			if (blog.ModuleId != moduleId)
			{
				SiteUtils.RedirectToAccessDeniedPage(this);
				return;
			}


			uploader.MaxFilesAllowed = BlogConfiguration.MaxAttachmentsToUploadAtOnce;
			//uploader.AcceptFileTypes = SecurityHelper.GetRegexValidationForAllowedExtensionsJqueryFileUploader(WebConfigSettings.AllowedMediaFileExtensions);
			uploader.UploadButtonClientId = btnUpload.ClientID;
			uploader.ServiceUrl = SiteRoot + "/Blog/upload.ashx?pageid=" + pageId.ToInvariantString()
				+ "&mid=" + moduleId.ToInvariantString()
				+ "&ItemID=" + itemId.ToInvariantString();
			uploader.FormFieldClientId = hdnState.ClientID; // not really used but prevents submitting all the form 

			string refreshFunction = "function refresh" + moduleId.ToInvariantString()
					+ " (event) { " +
					"console.log(event);" +
					"if (!event.result.files.some(x=>x.ErrorMessage)) {" +
					"window.location.reload(true);} } ";

			uploader.UploadCompleteCallback = $"refresh{moduleId.ToInvariantString()}";

			ScriptManager.RegisterClientScriptBlock(
				this,
				this.GetType(), "refresh" + moduleId.ToInvariantString(),
				refreshFunction,
				true);



		}

		btnUpload.Enabled = (blog != null);

		litAttachmentWarning.Visible = (blog == null);

		pnlMetaData.Visible = (blog != null);


		divHistoryDelete.Visible = (enableContentVersioning && isAdmin);

		pnlHistory.Visible = enableContentVersioning;

		if (enableContentVersioning)
		{
			SetupHistoryRestoreScript();
		}

		try
		{
			// this keeps the action from changing during ajax postback in folder based sites
			SiteUtils.SetFormAction(Page, Request.RawUrl);
		}
		catch (MissingMethodException)
		{
			//this method was introduced in .NET 3.5 SP1
		}

		siteUser = SiteUtils.GetCurrentSiteUser();

		fileSystem = FileSystemHelper.LoadFileSystem();

		blogMetaConfigFile = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/MetadataConfiguration/blog.json";
		blogMetaConfigDefault = "[{\"NameProperty\":\"itemprop\",\"Name\":\"\",\"ContentProperty\":\"itemtype\",\"MetaContent\":\"http://schema.org/Article\"},{\"NameProperty\":\"itemprop\",\"Name\":\"name\",\"ContentProperty\":\"content\",\"MetaContent\":\"{{title}}\"},{\"NameProperty\":\"itemprop\",\"Name\":\"description\",\"ContentProperty\":\"content\",\"MetaContent\":\"{{description}}\"},{\"NameProperty\":\"itemprop\",\"Name\":\"image\",\"ContentProperty\":\"content\",\"MetaContent\":\"{{image}}\"},{\"NameProperty\":\"property\",\"Name\":\"og:type\",\"ContentProperty\":\"content\",\"MetaContent\":\"article\"},{\"NameProperty\":\"property\",\"Name\":\"og:site_name\",\"ContentProperty\":\"content\",\"MetaContent\":\"{{site-name}}\"},{\"NameProperty\":\"property\",\"Name\":\"og:title\",\"ContentProperty\":\"content\",\"MetaContent\":\"{{title}}\"},{\"NameProperty\":\"property\",\"Name\":\"og:url\",\"ContentProperty\":\"content\",\"MetaContent\":\"{{url}}\"},{\"NameProperty\":\"property\",\"Name\":\"og:description\",\"ContentProperty\":\"content\",\"MetaContent\":\"{{description}}\"},{\"NameProperty\":\"property\",\"Name\":\"og:image\",\"ContentProperty\":\"content\",\"MetaContent\":\"{{image}}\"},{\"NameProperty\":\"name\",\"Name\":\"twitter:card\",\"ContentProperty\":\"content\",\"MetaContent\":\"summary_large_image\"},{\"NameProperty\":\"name\",\"Name\":\"twitter:title\",\"ContentProperty\":\"content\",\"MetaContent\":\"{{title}}\"},{\"NameProperty\":\"name\",\"Name\":\"twitter:description\",\"ContentProperty\":\"content\",\"MetaContent\":\"{{description}}\"},{\"NameProperty\":\"name\",\"Name\":\"twitter:image\",\"ContentProperty\":\"content\",\"MetaContent\":\"{{image}}\"}]";
	}

	private void SetupHistoryRestoreScript()
	{
		StringBuilder script = new StringBuilder();

		script.Append("\n<script type='text/javascript'>");
		script.Append("function LoadHistoryInEditor(hxGuid) {");

		script.Append("var hdn = document.getElementById('" + this.hdnHxToRestore.ClientID + "'); ");
		script.Append("hdn.value = hxGuid; ");
		script.Append("var btn = document.getElementById('" + this.btnRestoreFromGreyBox.ClientID + "');  ");
		script.Append("btn.click(); ");
		script.Append("$.colorbox.close(); ");

		script.Append("}");
		script.Append("</script>");


		Page.ClientScript.RegisterStartupScript(typeof(Page), "gbHandler", script.ToString());

	}

	private void LoadParams()
	{
		timeOffset = SiteUtils.GetUserTimeOffset();
		timeZone = SiteUtils.GetUserTimeZone();
		pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
		moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
		itemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);
		restoreGuid = WebUtils.ParseGuidFromQueryString("r", restoreGuid);
		//cacheDependencyKey = "Module-" + moduleId.ToInvariantString();
		virtualRoot = WebUtils.GetApplicationRoot();

		AddClassToBody("blogeditpost");


	}

	private void SetupScripts()
	{
		//if (!Page.ClientScript.IsClientScriptBlockRegistered("sarissa"))
		//{
		//    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sarissa", "<script src=\""
		//        + ResolveUrl("~/ClientScript/sarissa/sarissa.js") + "\" type=\"text/javascript\"></script>");
		//}

		//if (!Page.ClientScript.IsClientScriptBlockRegistered("sarissa_ieemu_xpath"))
		//{
		//    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sarissa_ieemu_xpath", "<script src=\""
		//        + ResolveUrl("~/ClientScript/sarissa/sarissa_ieemu_xpath.js") + "\" type=\"text/javascript\"></script>");
		//}


		if (!Page.ClientScript.IsClientScriptBlockRegistered("friendlyurlsuggest"))
		{
			Page.ClientScript.RegisterClientScriptBlock(GetType(), "friendlyurlsuggest", "<script src=\""
				+ ResolveUrl($"{WebConfigSettings.FriendlyUrlSuggestScript}?v={siteSettings.SkinVersion}") + "\" type=\"text/javascript\"></script>");
		}

		string focusScript = string.Empty;

		if (itemId == -1)
		{
			focusScript = $"document.getElementById('{txtTitle.ClientID}').focus();";
		}

		string hookupInputScript = $@"<script type=""text/javascript"">
				new UrlHelper(
						document.getElementById('{txtTitle.ClientID}'),
						document.getElementById('{txtItemUrl.ClientID}'),
						document.getElementById('{hdnTitle.ClientID}'),
						document.getElementById('{spnUrlWarning.ClientID}'), 
						""{SiteRoot}/Blog/BlogUrlSuggestService.ashx"",
						""{config.DefaultUrlPrefix}""
					); {focusScript}</script>";

		if (!Page.ClientScript.IsStartupScriptRegistered(UniqueID + "urlscript"))
		{
			Page.ClientScript.RegisterStartupScript(
				GetType(),
				UniqueID + "urlscript", hookupInputScript);
		}
	}

	#region OnInit

	protected override void OnPreInit(EventArgs e)
	{
		AllowSkinOverride = true;
		base.OnPreInit(e);
		SiteUtils.SetupEditor(edContent, AllowSkinOverride, this);
		SiteUtils.SetupEditor(edExcerpt, AllowSkinOverride, this);

		if (BlogConfiguration.EditPostSuppressPageMenu) { SuppressPageMenu(); }
	}

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);

		ScriptConfig.IncludeJQTable = true;


		if (btnAddCategory == null)
		{
			log.Error("btnAddCategory was null");

			btnAddCategory = (mojoButton)UpdatePanel1.FindControl("btnAddCategory");
		}

		btnAddCategory.Click += new EventHandler(btnAddCategory_Click);


		btnUpdate.Click += new EventHandler(btnUpdate_Click);
		//btnUpdate2.Click += new EventHandler(this.btnUpdate_Click);
		//btnUpdate3.Click += new EventHandler(this.btnUpdate_Click);
		btnDelete.Click += new EventHandler(btnDelete_Click);
		//btnDelete2.Click += new EventHandler(this.btnDelete_Click);
		//btnDelete3.Click += new EventHandler(this.btnDelete_Click);
		//btnUpdate4.Click += new EventHandler(this.btnUpdate_Click);
		//btnUpdate5.Click += new EventHandler(this.btnUpdate_Click);
		//btnDelete4.Click += new EventHandler(this.btnDelete_Click);
		//btnDelete5.Click += new EventHandler(this.btnDelete_Click);

		btnSaveAndPreview.Click += new EventHandler(btnSaveAndPreview_Click);

		grdHistory.RowCommand += new GridViewCommandEventHandler(grdHistory_RowCommand);
		grdHistory.RowDataBound += new GridViewRowEventHandler(grdHistory_RowDataBound);
		pgrHistory.Command += new CommandEventHandler(pgrHistory_Command);
		btnRestoreFromGreyBox.Click += new ImageClickEventHandler(btnRestoreFromGreyBox_Click);
		btnDeleteHistory.Click += new EventHandler(btnDeleteHistory_Click);

		grdContentMeta.RowCommand += new GridViewCommandEventHandler(grdContentMeta_RowCommand);
		grdContentMeta.RowEditing += new GridViewEditEventHandler(grdContentMeta_RowEditing);
		grdContentMeta.RowUpdating += new GridViewUpdateEventHandler(grdContentMeta_RowUpdating);
		grdContentMeta.RowCancelingEdit += new GridViewCancelEditEventHandler(grdContentMeta_RowCancelingEdit);
		grdContentMeta.RowDeleting += new GridViewDeleteEventHandler(grdContentMeta_RowDeleting);
		grdContentMeta.RowDataBound += new GridViewRowEventHandler(grdContentMeta_RowDataBound);
		btnAddMeta.Click += new EventHandler(btnAddMeta_Click);

		grdMetaLinks.RowCommand += new GridViewCommandEventHandler(grdMetaLinks_RowCommand);
		grdMetaLinks.RowEditing += new GridViewEditEventHandler(grdMetaLinks_RowEditing);
		grdMetaLinks.RowUpdating += new GridViewUpdateEventHandler(grdMetaLinks_RowUpdating);
		grdMetaLinks.RowCancelingEdit += new GridViewCancelEditEventHandler(grdMetaLinks_RowCancelingEdit);
		grdMetaLinks.RowDeleting += new GridViewDeleteEventHandler(grdMetaLinks_RowDeleting);
		grdMetaLinks.RowDataBound += new GridViewRowEventHandler(grdMetaLinks_RowDataBound);
		btnAddMetaLink.Click += new EventHandler(btnAddMetaLink_Click);

		btnUpload.Click += new EventHandler(btnUpload_Click);
		grdAttachments.RowCommand += new GridViewCommandEventHandler(grdAttachments_RowCommand);
		grdAttachments.RowDeleting += new GridViewDeleteEventHandler(grdAttachments_RowDeleting);
	}

	#endregion
}