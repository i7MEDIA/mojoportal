using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	public partial class BreadcrumbsControl : UserControl
	{
		#region Fields

		private SiteSettings siteSettings = null;
		private PageSettings currentPage;
		protected string siteRoot = string.Empty;
		private string separator = " > ";
		private bool isAdmin = false;
		private bool isContentAdmin = false;
		private bool isSiteEditor = false;
		private SiteMapDataSource siteMapDataSource;
		private SiteMapNode currentPageNode = null;
		private StringBuilder markup = null;
		protected string rootLinkText = Resource.HomePageLink;
		private bool enableUnclickableLinks = false;

		#endregion


		#region Public Properties

		public string WrapperCssClass { get; set; } = "breadcrumbs";
		public bool ForceShowBreadcrumbs { get; set; } = false;
		public bool UsePageCrumbOnly { get; set; } = false;
		public bool UseTopParentCrumbOnly { get; set; } = false;
		public bool SuppresIfCurrentPageIsParent { get; set; } = false;
		public bool ForceShowChildPageBreadCrumbs { get; set; } = false;
		public string AddedCrumbs { get; set; } = string.Empty;
		public string CssClass { get; set; } = "unselectedcrumb";
		public string CurrentPageCssClass { get; set; } = "selectedcrumb";
		public bool ShowHome { get; set; } = false;
		public string ItemWrapperTop { get; set; } = string.Empty;
		public string ItemWrapperBottom { get; set; } = string.Empty;

		public string Separator
		{
			get { return Server.HtmlEncode(separator); }
			set { separator = value; }
		}

		#endregion


		#region Event Handlers

		protected void Page_Load(object sender, EventArgs e)
		{
			if (HttpContext.Current == null)
			{
				return;
			}

			EnableViewState = false;

			siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (siteSettings == null)
			{
				return;
			}

			currentPage = CacheHelper.GetCurrentPage();

			if (currentPage == null)
			{
				return;
			}

			siteMapDataSource = (SiteMapDataSource)Page.Master.FindControl("SiteMapData");

			if (siteMapDataSource == null)
			{
				return;
			}

			siteMapDataSource.SiteMapProvider = "mojosite" + siteSettings.SiteId.ToInvariantString();

			if (WebConfigSettings.DisableViewStateOnSiteMapDataSource)
			{
				siteMapDataSource.EnableViewState = false;
			}

			isAdmin = WebUser.IsAdmin;

			if (!isAdmin)
			{
				isContentAdmin = WebUser.IsContentAdmin;
			}

			if (!isAdmin && !isContentAdmin)
			{
				isSiteEditor = SiteUtils.UserIsSiteEditor();
			}

			siteRoot = SiteUtils.GetNavigationSiteRoot();

			if (Page is mojoBasePage)
			{
				mojoBasePage basePage = Page as mojoBasePage;
				enableUnclickableLinks = basePage.StyleCombiner.EnableNonClickablePageLinks;
			}

			DoRendering();
		}

		void breadCrumbsControl_ItemDataBound(object sender, SiteMapNodeItemEventArgs e)
		{
			if (enableUnclickableLinks)
			{
				mojoSiteMapNode mapNode = (mojoSiteMapNode)e.Item.SiteMapNode;

				if (mapNode != null && !mapNode.IsClickable)
				{
					e.Item.Enabled = false;
				}
			}
		}

		#endregion


		#region Private Methods

		private void DoRendering()
		{
			if (
				!ForceShowBreadcrumbs &&
				!currentPage.ShowBreadcrumbs &&
				!ForceShowChildPageBreadCrumbs &&
				!currentPage.ShowChildPageBreadcrumbs
			)
			{
				Visible = false;

				return;
			}

			pnlWrapper.CssClass = WrapperCssClass;

			if (WebConfigSettings.UseSiteNameForRootBreadcrumb)
			{
				rootLinkText = siteSettings.SiteName;
			}

			if (ForceShowBreadcrumbs || currentPage.ShowBreadcrumbs)
			{
				breadCrumbsControl.Visible = true;

				BindCrumbs();
			}

			if (ForceShowChildPageBreadCrumbs || currentPage.ShowChildPageBreadcrumbs)
			{
				RenderChildPageBreadcrumbs();
			}

			if (ForceShowBreadcrumbs || currentPage.ShowBreadcrumbs || currentPage.ShowChildPageBreadcrumbs)
			{
				if (AddedCrumbs.Length > 0)
				{
					if (markup == null)
					{
						markup = new StringBuilder();
					}

					AddSeparator(markup);

					markup.Append(AddedCrumbs);
				}

				if (markup != null) { childCrumbs.Text = markup.ToString(); }
			}
		}


		private void BindCrumbs()
		{
			if (siteMapDataSource == null) { return; }

			if (!ShowHome && !currentPage.ShowHomeCrumb)
			{
				int currentPageDepth = SiteUtils.GetCurrentPageDepth(siteMapDataSource.Provider.RootNode);

				breadCrumbsControl.ParentLevelsDisplayed = currentPageDepth;
			}

			currentPageNode = SiteUtils.GetCurrentPageSiteMapNode(siteMapDataSource.Provider.RootNode);

			breadCrumbsControl.OverrideCurrentNode = currentPageNode;
			breadCrumbsControl.PathSeparator = Separator;
			breadCrumbsControl.Provider = siteMapDataSource.Provider;

			breadCrumbsControl.DataBind();
		}


		private void RenderChildPageBreadcrumbs()
		{
			if (HttpContext.Current == null)
			{
				return;
			}

			if (currentPageNode == null)
			{
				currentPageNode = SiteUtils.GetCurrentPageSiteMapNode(siteMapDataSource.Provider.RootNode);
			}

			if (currentPageNode == null)
			{
				return;
			}

			markup = new StringBuilder();
			markup.Append(Separator);

			var childSeparator = string.Empty;
			var addedChildren = 0;

			foreach (SiteMapNode childNode in currentPageNode.ChildNodes)
			{
				if (!(childNode is mojoSiteMapNode))
				{
					continue;
				}

				mojoSiteMapNode node = childNode as mojoSiteMapNode;

				if (!node.IncludeInMenu)
				{
					continue;
				}

				var remove = false;

				if (node.Roles == null)
				{
					if (!isAdmin && !isContentAdmin && !isSiteEditor)
					{
						remove = true;
					}
				}
				else
				{
					if (!isAdmin && node.Roles.Count == 1 && node.Roles[0].ToString() == "Admins")
					{
						remove = true;
					}

					if (!isAdmin && !isContentAdmin && !isSiteEditor && !WebUser.IsInRoles(node.Roles))
					{
						remove = true;
					}
				}

				if (!remove)
				{
					_ = markup.Append($"{childSeparator}<a class=\"{CssClass}\" href=\"{Page.ResolveUrl(node.Url)}\">{node.Title}</a>");

					childSeparator = " - ";
					addedChildren += 1;
				}
			}

			// this gets rid of the initial separator between bread crumbs and child crumbs if no children were rendered
			if (addedChildren == 0)
			{
				markup = null;
			}
		}


		private void AddSeparator(StringBuilder markup)
		{
			if (markup != null)
			{
				markup.Append(Separator);
			}
		}

		#endregion


		#region Init

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Load += new EventHandler(Page_Load);

			breadCrumbsControl.ItemDataBound += new SiteMapNodeItemEventHandler(breadCrumbsControl_ItemDataBound);
		}

		#endregion
	}
}
