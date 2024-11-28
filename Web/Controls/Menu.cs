using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;

namespace mojoPortal.Web.Controls.Menu;

public class Menu : WebControl
{
	private static readonly ILog log = LogManager.GetLogger(typeof(Menu));


	public bool ShowStartingPage = false;
	public int StartingPageId { get; set; } = -1;
	public int MaxDepth { get; set; } = -1;
	public string View { get; set; } = "Menu";
	public bool ShowDescriptions { get; set; } = false;
	public bool ShowImages { get; set; } = false;

    private SiteSettings siteSettings;
	private PageSettings startingPage;
	private PageSettings currentPage;

	protected override void RenderContents(HtmlTextWriter writer)
	{
		siteSettings = CacheHelper.GetCurrentSiteSettings();
		currentPage = CacheHelper.GetCurrentPage();

		string renderString;

		SiteMapDataSource menuDataSource = new()
		{
			SiteMapProvider = $"mojosite{siteSettings.SiteId}"
		};

		var startingNode = menuDataSource.Provider.RootNode;

		//change startingNode from Root if a different page is set with StartingPageId
		if (StartingPageId > -1)
		{
			startingPage = new(siteSettings.SiteId, StartingPageId);
			if (startingPage != null)
			{
				startingNode = menuDataSource.Provider.FindSiteMapNode(startingPage.Url);
			}
		}

		var model = new Models.MenuModel
		{
			Id = siteSettings.SiteId,
			Menu = new MenuList(startingNode, ShowStartingPage),
			StartingPage = getMenuItemFromPageSettings(startingPage),
			CurrentPage = getMenuItemFromPageSettings(currentPage),
			ShowStartingNode = ShowStartingPage,
			MaxDepth = MaxDepth
		};

		mojoMenuItem getMenuItemFromPageSettings(PageSettings pageSettings)
		{
			if (pageSettings == null)
			{
				return null;
			}

			return new mojoMenuItem
			{
				PageId = pageSettings.PageId,
				Name = pageSettings.PageName,
				Description = Global.SkinConfig.MenuOptions.UseDescriptions ? pageSettings.MenuDescription : string.Empty,
				ShowDescription = Global.SkinConfig.MenuOptions.UseDescriptions && !string.IsNullOrWhiteSpace(pageSettings.MenuDescription),
				Image = Global.SkinConfig.MenuOptions.UseImages ? pageSettings.MenuImage : string.Empty,
				ShowImage = Global.SkinConfig.MenuOptions.UseImages && !string.IsNullOrWhiteSpace(pageSettings.MenuImage),
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
			renderString = RazorBridge.RenderPartialToString(View, model, "Shared");
		}
		catch (Exception ex)
		{
			renderString = RazorBridge.RenderFallback(View, "Menu control", "Menu", model, "Shared", ex.ToString(), SiteUtils.DetermineSkinBaseUrl(true, Page)); 
		}

		writer.Write(renderString);
	}

	protected override void Render(HtmlTextWriter writer)
	{
		RenderContents(writer);
	}
}