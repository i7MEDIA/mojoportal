using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Web.SearchUI
{
	public partial class SiteSearchModule : SiteModuleControl
	{
		// FeatureGuid 09c0b1fc-a92d-4f3c-9f9b-b167c82d6089
		private TimeZoneInfo timeZone = null;
		protected SearchModuleConfiguration config = new();
		private int totalPages = 1;
		private int totalHits = 0;
		private bool isSiteEditor = false;
		private bool queryErrorOccurred = false;
		private DateTime modifiedBeginDate = DateTime.MinValue;
		private DateTime modifiedEndDate = DateTime.MaxValue;
		private int pageNumber = 1;
		private int pageSize = 10;

		#region OnInit
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Load += new EventHandler(Page_Load);
			btnSearch.Click += btnSearch_Click;
			pgr.Command += pgr_Command;
		}
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			LoadSettings();
			PopulateControls();
		}

		private void PopulateControls()
		{
			TitleControl.Visible = !RenderInWebPartMode;
			if (ModuleConfiguration != null)
			{
				Title = ModuleConfiguration.ModuleTitle;
				Description = ModuleConfiguration.FeatureName;
			}
		}

		void btnSearch_Click(object sender, EventArgs e)
		{
			BindSearch();
		}

		void pgr_Command(object sender, CommandEventArgs e)
		{
			pageNumber = Convert.ToInt32(e.CommandArgument);
			pgr.CurrentIndex = pageNumber;
			BindSearch();
		}

		private void BindSearch()
		{
			queryErrorOccurred = false;

			IndexItemCollection searchResults = IndexHelper.Search(
				siteSettings.SiteId,
				isSiteEditor,
				GetUserRoles(),
				config.GetFeatureGuids(),
				modifiedBeginDate,
				modifiedEndDate,
				txtSearch.Text,
				WebConfigSettings.EnableSearchResultsHighlighting,
				WebConfigSettings.SearchResultsFragmentSize,
				pageNumber,
				pageSize,
				WebConfigSettings.SearchMaxClauseCount,
				out totalHits,
				out queryErrorOccurred);

			totalPages = 1;

			if (pageSize > 0) totalPages = totalHits / pageSize;

			if (totalHits <= pageSize)
			{
				totalPages = 1;
			}
			else
			{
				Math.DivRem(totalHits, pageSize, out int remainder);
				if (remainder > 0)
				{
					totalPages += 1;
				}
			}

			pgr.ShowFirstLast = true;
			pgr.PageSize = pageSize;
			pgr.PageCount = totalPages;
			pgr.Visible = totalPages > 1;
			rptResults.DataSource = searchResults;
			rptResults.DataBind();
			updPnl.Update();
		}

		private List<string> GetUserRoles()
		{
			List<string> userRoles = ["All Users"];
			if (Request.IsAuthenticated)
			{
				SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
				if (currentUser != null)
				{
					using IDataReader reader = SiteUser.GetRolesByUser(siteSettings.SiteId, currentUser.UserId);
					while (reader.Read())
					{
						userRoles.Add(reader["RoleName"].ToString());
					}
				}
			}
			return userRoles;
		}

		public string BuildUrl(IndexItem indexItem)
		{
			if (indexItem.UseQueryStringParams)
			{
				return $"{SiteRoot}/{indexItem.ViewPage}?pageid={indexItem.PageId.ToInvariantString()}&mid={indexItem.ModuleId.ToInvariantString()}&ItemID={indexItem.ItemId.ToInvariantString()}{indexItem.QueryStringAddendum}";
			}
			else
			{
				return $"{SiteRoot}/{indexItem.ViewPage}";
			}
		}

		public string FormatCreatedDate(IndexItem indexItem)
		{
			if (!displaySettings.ShowCreatedDate || timeZone is null)
			{
				return string.Empty;
			}

			if (indexItem.CreatedUtc.Date == DateTime.MinValue.Date)
			{
				return string.Empty;
			}

			if (displaySettings.CreatedFormat.Length > 0)
			{
				return string.Format(
					CultureInfo.CurrentCulture,
					displaySettings.CreatedFormat,
					TimeZoneInfo.ConvertTimeFromUtc(indexItem.CreatedUtc, timeZone).ToShortDateString());
			}

			return string.Format(
					CultureInfo.CurrentCulture,
					Resource.SearchCreatedHtmlFormat,
					TimeZoneInfo.ConvertTimeFromUtc(indexItem.CreatedUtc, timeZone).ToShortDateString());
		}

		public string FormatModifiedDate(IndexItem indexItem)
		{
			if (!displaySettings.ShowLastModDate || timeZone is null)
			{
				return string.Empty;
			}

			if (indexItem.LastModUtc.Date == DateTime.MinValue.Date)
			{
				return string.Empty;
			}

			if (displaySettings.ModifiedFormat.Length > 0)
			{
				return string.Format(
					CultureInfo.CurrentCulture,
					displaySettings.ModifiedFormat,
					TimeZoneInfo.ConvertTimeFromUtc(indexItem.LastModUtc, timeZone).ToShortDateString());
			}

			return string.Format(
					CultureInfo.CurrentCulture,
					Resource.SearchModifiedHtmlFormat,
					TimeZoneInfo.ConvertTimeFromUtc(indexItem.LastModUtc, timeZone).ToShortDateString());
		}

		protected string FormatAuthor(string author)
		{
			if (!displaySettings.ShowAuthor || string.IsNullOrEmpty(author))
			{
				return string.Empty;
			}

			if (!string.IsNullOrWhiteSpace(displaySettings.AuthorFormat))
			{
				return string.Format(
					CultureInfo.InvariantCulture,
					displaySettings.AuthorFormat,
					author);
			}

			return string.Format(
					CultureInfo.InvariantCulture,
					Resource.SearchAuthorHtmlFormat,
					author);
		}

		protected string FormatLinkText(string pageName, string moduleTtile, string itemTitle)
		{
			if (itemTitle.Length > 0)
			{
				return $"{pageName} &gt; {itemTitle}";
			}

			return pageName;
		}

		private void LoadSettings()
		{
			config = new SearchModuleConfiguration(Settings);

			btnSearch.Text = Resource.SearchButtonText;

			if (displaySettings.OverrideButtonText.Length > 0)
			{
				Search.OverrideButtonText = displaySettings.OverrideButtonText;
				btnSearch.Text = displaySettings.OverrideButtonText;
			}

			Search.UseWatermark = displaySettings.UseWatermark;

			if (displaySettings.OverrideWatermarkText.Length > 0 && Search.UseWatermark)
			{
				Search.OverrideWatermark = displaySettings.OverrideWatermarkText;
				txtSearch.Attributes.Add("placeholder", displaySettings.OverrideWatermarkText);
			}

			if (config.ShowResultsInsteadOfRedirect)
			{
				Search.Visible = false;
				pnlSearch.Visible = true;

				if (displaySettings.ShowCreatedDate || displaySettings.ShowLastModDate)
				{
					timeZone = SiteUtils.GetUserTimeZone();
				}

				pageSize = config.PageSize;
				if (displaySettings.OverridePageSize > 0)
				{
					pageSize = displaySettings.OverridePageSize;
				}
			}
			else
			{
				var featureGuids = config.GetFeatureGuids();
				if (featureGuids.Length > 1)
				{
					Search.FeatureGuid = featureGuids[0];
				}
			}
		}
	}
}

namespace mojoPortal.Web.UI
{
	/// <summary>
	/// this control doesn't render anything, it is used only as a themeable collection of settings for things we would like to be able to configure from theme.skin
	/// </summary>
	public class SearchModuleConfiguration
	{
		public SearchModuleConfiguration()
		{ }

		public SearchModuleConfiguration(Hashtable settings)
		{
			LoadSettings(settings);
		}

		public Guid[] GetFeatureGuids()
		{
			if (searchableFeature.Length == 0)
			{
				return null;
			}

			List<string> gstrings = searchableFeature.SplitOnChar(',');
			if (gstrings.Count == 0)
			{
				return null;
			}
			else
			{
				Guid[] guids = new Guid[gstrings.Count];
				int i = 0;
				foreach (string s in gstrings)
				{
					if (s.Length == 36)
					{
						guids[i] = new Guid(s);
						i += 1;
					}
				}

				return guids;
			}
		}

		private void LoadSettings(Hashtable settings)
		{
			if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

			ShowResultsInsteadOfRedirect = WebUtils.ParseBoolFromHashtable(settings, "ShowSearchResultsInsteadOfRedirect", ShowResultsInsteadOfRedirect);
			PageSize = WebUtils.ParseInt32FromHashtable(settings, "PageSize", PageSize);
			ShowExcerpt = WebUtils.ParseBoolFromHashtable(settings, "ShowExcerpt", ShowExcerpt);
			searchableFeature = WebUtils.ParseStringFromHashtable(settings, "SearchableFeature", searchableFeature);
		}

		private string searchableFeature = "881e4e00-93e4-444c-b7b0-6672fb55de10,026cbead-2b80-4491-906d-b83e37179ccf,38aa5a84-9f5c-42eb-8f4c-105983d419fb,74bdbcc2-0e79-47ff-bcd4-a159270bf36e,dc873d76-5bf2-4ac5-bff7-434a87a3fc8e,d572f6b4-d0ed-465d-ad60-60433893b401,5a343d88-bce1-43d1-98ae-b42d77893e7b,0cefbf18-56de-11dc-8f36-bac755d89593"; // a csv of featureguids

		public bool ShowResultsInsteadOfRedirect { get; set; } = true;

		public int PageSize { get; private set; } = 10;

		public bool ShowExcerpt { get; private set; } = true;
	}
}

namespace mojoPortal.Web.UI
{
	public class SearchModuleDisplaySettings : WebControl
	{
		public int OverridePageSize { get; set; } = 0;

		public string OverrideButtonText { get; set; } = string.Empty;

		public string OverrideWatermarkText { get; set; } = string.Empty;

		public bool UseWatermark { get; set; } = true;

		public string ItemHeadingElement { get; set; } = "h3";

		public bool ShowExcerpt { get; } = true;

		public bool ShowAuthor { get; set; } = false;

		public string AuthorFormat { get; set; } = string.Empty;

		public bool ShowCreatedDate { get; set; } = false;

		public string CreatedFormat { get; set; } = string.Empty;

		public bool ShowLastModDate { get; set; } = false;

		public string ModifiedFormat { get; set; } = string.Empty;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			EnableViewState = false;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			//base.Render(writer);
		}
	}
}