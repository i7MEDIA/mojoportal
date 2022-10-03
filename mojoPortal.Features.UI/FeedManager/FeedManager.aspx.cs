using mojoPortal.Business;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.FeedUI
{
	public partial class FeedManagerPage : mojoBasePage
	{
		protected int PageId = -1;
		protected int ModuleId = -1;
		protected int ItemId = -1;
		private Module module = null;
		private Hashtable moduleSettings = null;
		private bool canEdit = false;
		private int totalPages = 1;
		//private string previousPubDate;
		protected FeedManagerConfiguration config = new FeedManagerConfiguration();
		protected string ConfirmImage = string.Empty;
		protected string allowedImageUrlRegexPattern = SecurityHelper.RegexRelativeImageUrlPatern;
		protected string FeedItemHeadingElement = FeedManagerConfiguration.FeedItemHeadingElement;
		protected double timeZoneOffset = 0;
		private TimeZoneInfo timeZone = null;


		protected void Page_Load(object sender, EventArgs e)
		{
			LoadParams();

			if (!canEdit)
			{
				SiteUtils.RedirectToAccessDeniedPage(this);

				return;
			}

			LoadSettings();

			if (!config.EnableSelectivePublishing)
			{
				WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

				return;
			}

			SetupCss();
			PopulateLabels();
			PopulateControls();
		}


		private void PopulateControls()
		{
			if (!IsPostBack)
			{
				pgrRptEntries.CurrentIndex = 1;
				BindRepeater();
			}
		}


		private DataView GetEntriesTable()
		{
			DataTable entriesTable = FeedCache.GetRssFeedEntries(
				ModuleId,
				module.ModuleGuid,
				config.EntryCacheTimeout,
				config.MaxDaysOld,
				config.MaxEntriesPerFeed,
				config.EnableSelectivePublishing
			);

			return entriesTable.DefaultView;
		}


		private void BindRepeater()
		{
			DataView entries = GetEntriesTable();

			if (config.SortAscending)
			{
				entries.Sort = "PubDate ASC";
			}
			else
			{
				entries.Sort = "PubDate DESC";
			}

			PagedDataSource pagedDS = new PagedDataSource
			{
				DataSource = entries,
				AllowPaging = true,
				PageSize = config.PageSize,
				CurrentPageIndex = pgrRptEntries.CurrentIndex - 1
			};

			totalPages = 1;
			int totalRows = entries.Count;

			if (config.PageSize > 0)
			{
				totalPages = totalRows / config.PageSize;
			}

			if (totalRows <= config.PageSize)
			{
				totalPages = 1;
			}
			else
			{
				Math.DivRem(totalRows, config.PageSize, out var remainder);

				if (remainder > 0)
				{
					totalPages += 1;
				}
			}

			if (totalPages > 1)
			{
				pgrRptEntries.ShowFirstLast = true;
				pgrRptEntries.PageSize = config.PageSize;
				pgrRptEntries.PageCount = totalPages;
			}
			else
			{
				pgrRptEntries.Visible = false;
			}

			rptEntries.DataSource = pagedDS;
			rptEntries.DataBind();
		}


		protected void pgrRptEntries_Command(object sender, CommandEventArgs e)
		{
			int currentPageIndex = Convert.ToInt32(e.CommandArgument);
			pgrRptEntries.CurrentIndex = currentPageIndex;
			BindRepeater();
			updPnlRSSA.Update();
		}


		protected void rptEntries_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "Confirm")
			{
				string entryInfo = (string)e.CommandArgument;
				int sep = entryInfo.IndexOf('_');

				if (sep != -1)
				{
					string[] entryHash = entryInfo.Split('_');
					bool published = Convert.ToBoolean(entryHash[1]);

					if (published)
					{
						RssFeed.UnPublish(module.ModuleGuid, Convert.ToInt32(entryHash[0]));
					}
					else
					{
						RssFeed.Publish(module.ModuleGuid, Convert.ToInt32(entryHash[0]));
					}

					BindRepeater();
				}
			}
		}


		protected string GetDateHeader(DateTime pubDate)
		{
			// adjust from GMT to user time zone
			if (timeZone == null)
			{
				return pubDate.AddHours(timeZoneOffset).ToString(config.DateFormat);
			}

			return pubDate.ToLocalTime(timeZone).ToString(config.DateFormat);
		}


		private void PopulateLabels()
		{
			Title = SiteUtils.FormatPageTitle(siteSettings, FeedResources.ManagePublishingLink);
		}


		private void LoadSettings()
		{
			timeZoneOffset = SiteUtils.GetUserTimeOffset();
			timeZone = SiteUtils.GetUserTimeZone();

			try
			{
				// this keeps the action from changing during ajax postback in folder based sites
				SiteUtils.SetFormAction(Page, Request.RawUrl);
			}
			catch (MissingMethodException)
			{
				//this method was introduced in .NET 3.5 SP1
			}

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

			ConfirmImage = ImageSiteRoot + "/Data/SiteImages/confirmed";

			if (config.AllowExternalImages)
			{
				allowedImageUrlRegexPattern = SecurityHelper.RegexAnyImageUrlPatern;
			}

			heading.Text = string.Format(CultureInfo.InvariantCulture, FeedResources.PublishingHeaderFormat, module.ModuleTitle);
			lnkBackToPage.Text = string.Format(CultureInfo.InvariantCulture, FeedResources.BackToPageLinkFormat, CurrentPage.PageName);

			lnkBackToPage.NavigateUrl = SiteUtils.GetCurrentPageUrl();

			lnkEditFeeds.Text = FeedResources.AddButton;
			lnkEditFeeds.NavigateUrl = $"{ SiteRoot }/FeedManager/FeedEdit.aspx?pageid={ PageId.ToInvariantString() }&mid={ ModuleId.ToInvariantString() }";

			AddClassToBody("feedmanagerpage");
		}


		private void SetupCss()
		{
			// older skins have this
			StyleSheet stylesheet = (StyleSheet)Page.Master.FindControl("StyleSheet");

			if (stylesheet != null)
			{
				if (stylesheet.FindControl("rsscss") == null)
				{
					Literal cssLink = new Literal
					{
						ID = "rsscss",
						Text = $"\n<link href='{ SiteUtils.GetSkinBaseUrl(Page) }rssmodule.css' type='text/css' rel='stylesheet' media='screen' />"
					};

					stylesheet.Controls.Add(cssLink);
				}
			}
		}


		private void LoadParams()
		{
			PageId = WebUtils.ParseInt32FromQueryString("pageid", PageId);
			ModuleId = WebUtils.ParseInt32FromQueryString("mid", ModuleId);
			canEdit = UserCanEditModule(ModuleId, RssFeed.FeatureGuid);
		}


		#region OnInit

		protected override void OnPreInit(EventArgs e)
		{
			AllowSkinOverride = true;
			base.OnPreInit(e);
		}


		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Load += new EventHandler(Page_Load);
			rptEntries.ItemCommand += new RepeaterCommandEventHandler(rptEntries_ItemCommand);
			pgrRptEntries.Command += new CommandEventHandler(pgrRptEntries_Command);
		}

		#endregion
	}
}
