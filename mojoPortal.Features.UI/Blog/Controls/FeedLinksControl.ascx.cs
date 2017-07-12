// Author:
// Created:       2009-05-04
// Last Modified: 2017-06-20
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Data;
using System.Web.UI;

namespace mojoPortal.Web.BlogUI
{
	public partial class FeedLinksControl : UserControl
	{
		private int pageId = -1;
		private int moduleId = -1;
		private string siteRoot = string.Empty;
		private BlogConfiguration config = new BlogConfiguration();
		private string imageSiteRoot = string.Empty;
		private SiteSettings siteSettings = null;
		protected string addThisAccountId = string.Empty;
		protected string RssImageFile = WebConfigSettings.RSSImageFileName;
		protected string rssLinkTitle = BlogResources.BlogRSSLinkTitle;
		private int categoryId = -1;
		private Module module = null;

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

		public BlogConfiguration Config
		{
			get { return config; }
			set { config = value; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{}

		protected override void OnPreRender(EventArgs e)
		{
			if (this.Visible)
			{
				if (pageId == -1) { return; }
				if (moduleId == -1) { return; }

				LoadSettings();
				PopulateLabels();
				SetupLinks();
			}

			base.OnPreRender(e);
		}

		private void SetupLinks()
		{
			if (siteSettings == null)
			{
				return;
			}

			if (displaySettings.OverrideRssFeedImageUrl.Length > 0)
			{
				RssImageFile = Page.ResolveUrl(displaySettings.OverrideRssFeedImageUrl);
			}

			litRssLink.Text = string.Format(displaySettings.RssFeedLinkFormat,
				GetRssUrl(),
				rssLinkTitle,
				Page.ResolveUrl("~/Data/SiteImages/" + RssImageFile),
				"RSS"
			);

			lnkAddThisRss.HRef = 
				"http://www.addthis.com/feed.php?pub=" +
				addThisAccountId +
				"&amp;h1=" +
				Server.UrlEncode(GetRssUrl()) +
				"&amp;t1="
			;

			imgAddThisRss.Src = Page.ResolveUrl(config.AddThisRssButtonImageUrl);
		}

		private string GetRssUrl()
		{
			if (
				(categoryId == -1) &&
				(config.FeedburnerFeedUrl.Length > 0) &&
				!BlogConfiguration.UseRedirectForFeedburner
			)
			{
				return config.FeedburnerFeedUrl;
			}


			if (config.FeedburnerFeedUrl.Length > 0)
			{
				return
					SiteRoot +
					"/Blog/RSS.aspx?p=" +
					pageId.ToInvariantString() +
					"~" +
					ModuleId.ToInvariantString() +
					"~" +
					categoryId.ToInvariantString() +
					"&amp;r=" +
					Global.FeedRedirectBypassToken.ToString()
				;
			}

			return 
				SiteRoot +
				"/Blog/RSS.aspx?p=" +
				pageId.ToInvariantString() +
				"~" +
				ModuleId.ToInvariantString() +
				"~" + categoryId.ToInvariantString()
			;
		}

		private void PopulateLabels()
		{
			lnkAddThisRss.Title = BlogResources.BlogAddThisSubscribeAltText;
		}

		private void LoadSettings()
		{
			siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (siteSettings == null)
			{
				return;
			}

			if (BlogConfiguration.UseCategoryFeedurlOnCategoryPage)
			{
				categoryId = WebUtils.ParseInt32FromQueryString("cat", categoryId);
			}

			module = new Module(moduleId);

			if (module != null)
			{
				rssLinkTitle = String.Format(BlogResources.BlogRSSLinkTitleFormat, module.ModuleTitle);
			}

			if (categoryId > -1)
			{
				string category = string.Empty;

				using (IDataReader reader = Blog.GetCategory(categoryId))
				{
					if (reader.Read())
					{
						category = reader["Category"].ToString();
					}
				}

				rssLinkTitle = String.Format(BlogResources.BlogRSSLinkTitleForCategoryFormat, module.ModuleTitle, category);
			}

			siteRoot = SiteUtils.GetNavigationSiteRoot();

			// we don't want ssl on the feed urls since it results in browser warnings
			if (!WebConfigSettings.UseSSLForFeedLinks)
			{
				siteRoot = SiteUtils.GetNavigationSiteRoot().Replace("https:", "http:");
			}

			if (config.AddThisAccountId.Length > 0)
			{
				addThisAccountId = config.AddThisAccountId;
			}
			else
			{
				addThisAccountId = siteSettings.AddThisDotComUsername;
			}

			liAddThisRss.Visible = (addThisAccountId.Length > 0);
			liAddThisRss.Visible = (config.ShowAddFeedLinks && (addThisAccountId.Length > 0));
		}
	}
}