using System;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Core.Extensions;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI;

public partial class CustomMenu : SiteModuleControl
{
	private int startingPageId = -2;
	private bool showStartingNode = false;
	private int maxDepth = -1;
	private string viewName = "_CustomMenu";
	private string customCssClass = string.Empty;
	private static readonly ILog log = LogManager.GetLogger(typeof(CustomMenu));

	protected void Page_Load(object sender, EventArgs e)
	{
		startingPageId = Settings.ParseInt32("CustomMenuStartingPage", startingPageId);
		showStartingNode = Settings.ParseBool("CustomMenuShowStartingNode", showStartingNode);
		maxDepth = Settings.ParseInt32("CustomMenuMaxDepth", maxDepth);
		viewName = Settings.ParseString("CustomMenuView", viewName);
		customCssClass = Settings.ParseString("CustomCssClassSetting", customCssClass);

		pnlOuterWrap.SetOrAppendCss(customCssClass);

		var startingPage = new PageSettings(siteSettings.SiteId, startingPageId);

		var menuDataSource = new SiteMapDataSource()
		{
			SiteMapProvider = Invariant($"mojosite{siteSettings.SiteId}")
		};

		var startingNode = menuDataSource.Provider.RootNode;
		if (startingPageId > -1 && startingPage != null)
		{
			startingNode = menuDataSource.Provider.FindSiteMapNode(startingPage.Url);
		}

		var model = new Models.MenuModel
		{
			Id = ModuleId,
			Menu = new MenuList(startingNode, showStartingNode),
			StartingPage = startingPage == null ? null : getMenuItemFromPageSettings(startingPage),
			CurrentPage = getMenuItemFromPageSettings(currentPage),
			ShowStartingNode = showStartingNode,
			MaxDepth = maxDepth
		};

		mojoMenuItem getMenuItemFromPageSettings(PageSettings pageSettings)
		{
			return new mojoMenuItem
			{
				PageId = pageSettings.PageId,
				Name = pageSettings.PageName,
				Description = pageSettings.MenuDescription,
				URL = WebUtils.ResolveUrl(pageSettings.Url),
				CssClass = pageSettings.MenuCssClass,
				Rel = pageSettings.LinkRel,
				Clickable = pageSettings.IsClickable,
				OpenInNewTab = pageSettings.OpenInNewWindow,
				PublishMode = pageSettings.PublishMode,
				LastModDate = pageSettings.LastModifiedUtc,
				Current = currentPage.PageId == pageSettings.PageId
			};
		}

		try
		{
			lit1.Text = RazorBridge.RenderPartialToString(viewName, model, "Common");
		}
		catch (Exception ex)
		{
			lit1.Text = RazorBridge.RenderFallback(viewName, "_CustomMenu", "_CustomMenu", model, "Common", ex.ToString(), SiteUtils.DetermineSkinBaseUrl(true, Page));
		}
	}
}