using mojoPortal.Business;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Collections;
using System.Data;

namespace mojoPortal.Web.FeedUI
{
	public partial class FeedEditPage : NonCmsBasePage
	{
		protected int PageId = -1;
		protected int ModuleId = -1;
		protected int ItemId = -1;
		protected FeedManagerConfiguration config = new FeedManagerConfiguration();
		protected Module module = null;
		private Hashtable moduleSettings = null;
		private DataTable dtFeedList = null;
		protected string EditContentImage = WebConfigSettings.EditContentImage;
		protected string RssImageFile = WebConfigSettings.RSSImageFileName;


		#region OnInit

		protected override void OnPreInit(EventArgs e)
		{
			AllowSkinOverride = true;
			base.OnPreInit(e);
		}


		override protected void OnInit(EventArgs e)
		{
			Load += new EventHandler(Page_Load);
			btnUpdate.Click += new EventHandler(btnUpdate_Click);
			//btnCancel.Click += new EventHandler(btnCancel_Click);
			btnDelete.Click += new EventHandler(btnDelete_Click);
			btnClearCache.Click += new EventHandler(btnClearCache_Click);
			base.OnInit(e);
		}

		#endregion


		private void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}

			SecurityHelper.DisableBrowserCache();

			LoadParams();

			if (!UserCanEditModule(ModuleId, RssFeed.FeatureGuid))
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
			PopulateLabels();
			BindFeedList();

			if (!IsPostBack)
			{
				if (ItemId > -1)
				{
					PopulateControls();
				}
				else
				{
					ShowNewFeedControls();
				}

				if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
				{
					hdnReturnUrl.Value = Request.UrlReferrer.ToString();
					lnkCancel.NavigateUrl = hdnReturnUrl.Value;
				}
			}
		}


		private void PopulateControls()
		{
			RssFeed feed = new RssFeed(ModuleId, ItemId);

			if (feed.ModuleId != ModuleId)
			{
				SiteUtils.RedirectToAccessDeniedPage(this);

				return;
			}

			txtAuthor.Text = feed.Author;
			txtWebSite.Text = feed.Url;
			txtRssUrl.Text = feed.RssUrl;
			txtImageUrl.Text = feed.ImageUrl;
			txtSortRank.Text = feed.SortRank.ToInvariantString();
			//FeedType.SetValue(feed.FeedType);
			chkPublishByDefault.Checked = feed.PublishByDefault;
		}


		private void BindFeedList()
		{
			if (dtFeedList == null)
			{
				dtFeedList = RssFeed.GetFeeds(ModuleId);
			}

			DataView members = dtFeedList.DefaultView;
			members.Sort = "Author";

			dlstFeedList.DataSource = members;
			dlstFeedList.DataBind();
		}


		private void ShowNewFeedControls()
		{
			btnDelete.Visible = false;
		}


		private void btnUpdate_Click(object sender, EventArgs e)
		{
			Page.Validate("feeds");

			if (!Page.IsValid)
			{
				return;
			}

			RssFeed feed = new RssFeed(ModuleId, ItemId);

			if (feed.ModuleId != ModuleId)
			{
				SiteUtils.RedirectToAccessDeniedPage(this);

				return;
			}

			feed.ModuleId = ModuleId;
			feed.Author = txtAuthor.Text;
			feed.Url = txtWebSite.Text;
			feed.RssUrl = txtRssUrl.Text;
			feed.ImageUrl = txtImageUrl.Text;
			int sortRank = 500;
			int.TryParse(txtSortRank.Text, out sortRank);
			feed.SortRank = sortRank;

			SiteUser siteUser = SiteUtils.GetCurrentSiteUser();

			if (siteUser == null)
			{
				return;
			}

			Module module = new Module(ModuleId);
			feed.ModuleGuid = module.ModuleGuid;
			feed.UserId = siteUser.UserId;
			feed.UserGuid = siteUser.UserGuid;
			feed.LastModUserGuid = siteUser.UserGuid;
			feed.PublishByDefault = chkPublishByDefault.Checked;

			if (feed.Save())
			{
				CurrentPage.UpdateLastModifiedTime();

				FeedCache.RefreshFeed(
					feed,
					ModuleId,
					module.ModuleGuid,
					config.MaxDaysOld,
					config.MaxEntriesPerFeed,
					config.EnableSelectivePublishing
				);

				string rssFriendlyUrl = $"aggregator{ ModuleId.ToInvariantString() }rss.aspx";

				if (!FriendlyUrl.Exists(siteSettings.SiteId, rssFriendlyUrl))
				{
					FriendlyUrl friendlyUrl = new FriendlyUrl
					{
						SiteId = siteSettings.SiteId,
						SiteGuid = siteSettings.SiteGuid,
						Url = rssFriendlyUrl,
						RealUrl = $"~/FeedManager/FeedAggregate.aspx?pageid={ PageId.ToInvariantString() }&mid={ ModuleId.ToInvariantString() }"
					};

					friendlyUrl.Save();
				}

				if (hdnReturnUrl.Value.Length > 0)
				{
					WebUtils.SetupRedirect(this, hdnReturnUrl.Value);

					return;
				}

				WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
			}
		}


		private void btnDelete_Click(object sender, EventArgs e)
		{
			RssFeed feed = new RssFeed(ModuleId, ItemId);

			if (feed.ModuleId != ModuleId)
			{
				SiteUtils.RedirectToAccessDeniedPage(this);
				return;
			}

			RssFeed.DeleteEntriesByFeed(ItemId);
			RssFeed.DeleteFeed(ItemId);
			CurrentPage.UpdateLastModifiedTime();

			if (hdnReturnUrl.Value.Length > 0)
			{
				WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
				return;
			}

			WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}


		void btnClearCache_Click(object sender, EventArgs e)
		{
			if (module == null)
			{
				return;
			}

			RssFeed.DeleteEntriesByModule(module.ModuleGuid);

			if (hdnReturnUrl.Value.Length > 0)
			{
				WebUtils.SetupRedirect(this, hdnReturnUrl.Value);

				return;
			}

			WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}


		private void PopulateLabels()
		{
			Title = SiteUtils.FormatPageTitle(siteSettings, FeedResources.FeedEditPageTitle);
			heading.Text = FeedResources.EditFeedLabel;
			btnUpdate.Text = FeedResources.SaveButton;
			SiteUtils.SetButtonAccessKey(btnUpdate, FeedResources.SaveButtonAccessKey);

			lnkCancel.Text = FeedResources.CancelButton;
			//btnCancel.Text = FeedResources.CancelButton;
			//SiteUtils.SetButtonAccessKey(btnCancel, FeedResources.CancelButtonAccessKey);

			btnDelete.Text = FeedResources.DeleteButton;
			SiteUtils.SetButtonAccessKey(btnDelete, FeedResources.DeleteButtonAccessKey);
			UIHelper.AddConfirmationDialog(btnDelete, FeedResources.DeleteFeedWarning);

			btnClearCache.Text = FeedResources.ClearCacheButton;
			UIHelper.AddConfirmationDialog(btnClearCache, FeedResources.ClearCacheWarning);

			lnkNewFeed.Text = FeedResources.AddNewFeedLink;
			lnkNewFeed.NavigateUrl = $"{ SiteRoot }/FeedManager/FeedEdit.aspx?pageid={ PageId.ToInvariantString() }&mid={ ModuleId.ToInvariantString() }&ItemID=-1";

			lnkNewFeed.Visible = ItemId != -1;

			reqTitle.ErrorMessage = FeedResources.FeedTitleRequiredWarning;

			reqFeedUrl.ErrorMessage = FeedResources.FeedUrlRequiredWarning;
			regexFeedUrl.ErrorMessage = FeedResources.FeedUrlRegexWarning;
			regexFeedUrl.ValidationExpression = @"^(http://|https://|~/)([^\s]+)/?";

			regexWebSiteUrl.ErrorMessage = FeedResources.WebUrlRegexWarning;
			regexWebSiteUrl.ValidationExpression = @"^(http://|https://|~/)([^\s]+)/?";
		}


		private void LoadParams()
		{
			PageId = WebUtils.ParseInt32FromQueryString("pageid", PageId);
			ModuleId = WebUtils.ParseInt32FromQueryString("mid", ModuleId);
			ItemId = WebUtils.ParseInt32FromQueryString("ItemID", ItemId);
		}


		private void LoadSettings()
		{
			lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();

			if (ModuleId == -1)
			{
				return;
			}

			module = new Module(ModuleId, CurrentPage.PageId);
			moduleSettings = ModuleSettings.GetModuleSettings(ModuleId);

			if (moduleSettings == null)
			{
				return;
			}

			config = new FeedManagerConfiguration(moduleSettings);

			divPublish.Visible = config.EnableSelectivePublishing;

			AddClassToBody("feededit");
		}
	}
}
