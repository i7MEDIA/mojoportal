using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;

namespace mojoPortal.Web;

public class layout : MasterPage
{
	protected SiteMenu SiteMenu1;
	protected Panel divLeft;
	protected PageMenuControl PageMenu1;
	protected ContentPlaceHolder leftContent;
	protected Panel divCenter;
	protected ContentPlaceHolder mainContent;
	protected Panel divRight;
	protected ContentPlaceHolder rightContent;
	protected ContentPlaceHolder pageEditContent;

	private int leftModuleCount = 0;
	private int centerModuleCount = 0;
	private int rightModuleCount = 0;
	private int alt1ModuleCount = 0;
	private int alt2ModuleCount = 0;
	protected SiteSettings siteSettings;
	protected PageSettings currentPage = null;
	private SiteMapDataSource siteMapDataSource = null;
	private SiteMapNode rootNode = null;
	protected string SkinBaseUrl = string.Empty;

	private bool useArtisteer3 = false;
	private bool hideEmptyAlt1 = true;
	private bool hideEmptyAlt2 = true;
	private string leftSideNoRightSideCss = "left-center";
	private string rightSideNoLeftSideCss = "right-center";
	private string leftAndRightNoCenterCss = string.Empty;
	private string leftOnlyCss = string.Empty;
	private string rightOnlyCss = string.Empty;
	private string centerNoLeftSideCss = "col-md-9 center-right";
	private string centerNoRightSideCss = "col-md-9 center-left";
	private string centerNoLeftOrRightSideCss = "col-md-12 nomargins";
	private string centerWithLeftAndRightSideCss = "col-md-6 center-left-right";
	private string emptyCenterCss = string.Empty;
	private bool hideEmptyCenterIfOnlySidesHaveContent = false;

	protected bool isCmsPage = false;
	protected bool isNonCmsBasePage = false;
	protected bool isMobileDevice = false;
	private int mobileOnly = (int)ContentPublishMode.MobileOnly;
	private int webOnly = (int)ContentPublishMode.WebOnly;

	protected virtual void OnPreInit(EventArgs e)
	{
		// this is here to allow adding logic before the Page_Load from the skin layout.master
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (HttpContext.Current is null)
		{
			return;
		}

		siteSettings = CacheHelper.GetCurrentSiteSettings();

		if (siteSettings is null)
		{
			return;
		}

		SkinBaseUrl = SiteUtils.DetermineSkinBaseUrl(allowPageOverride: true, page: Page);

		isMobileDevice = SiteUtils.IsMobileDevice();

		isNonCmsBasePage = Page is NonCmsBasePage;

		if (Page is CmsPage)
		{
			isCmsPage = true;
			currentPage = CacheHelper.GetCurrentPage();
		}
		else if (!isNonCmsBasePage)
		{
			currentPage = CacheHelper.GetPage(WebUtils.ParseInt32FromQueryString("pageid", -1));
		}

		siteMapDataSource = (SiteMapDataSource)FindControl("SiteMapData");
		if (siteMapDataSource is null) { return; }

		siteMapDataSource.SiteMapProvider = $"mojosite{siteSettings.SiteId.ToInvariantString()}";

		try
		{
			rootNode = siteMapDataSource.Provider.RootNode;
		}
		catch (HttpException)
		{
			return;
		}

		Control c = FindControl("StyleSheetCombiner");
		if (c is not null && c is StyleSheetCombiner)
		{
			StyleSheetCombiner style = c as StyleSheetCombiner;
			useArtisteer3 = style.UseArtisteer3;
			hideEmptyAlt1 = style.HideEmptyAlt1;
			hideEmptyAlt2 = style.HideEmptyAlt2;
		}

		if (!useArtisteer3)
		{
			if (Global.SkinConfig.Panels.Count > 0)
			{
				var panelOptions = Global.SkinConfig.Panels;
				leftSideNoRightSideCss = panelOptions.SingleOrDefault(x => x.Name.ToLower() == "leftsidenorightsidecss").Class;
				rightSideNoLeftSideCss = panelOptions.SingleOrDefault(x => x.Name.ToLower() == "rightsidenoleftsidecss").Class;
				leftAndRightNoCenterCss = panelOptions.SingleOrDefault(x => x.Name.ToLower() == "leftandrightnocentercss").Class;
				leftOnlyCss = panelOptions.SingleOrDefault(x => x.Name.ToLower() == "leftonlycss").Class;
				rightOnlyCss = panelOptions.SingleOrDefault(x => x.Name.ToLower() == "rightonlycss").Class;
				centerNoLeftSideCss = panelOptions.SingleOrDefault(x => x.Name.ToLower() == "centernoleftsidecss").Class;
				centerNoRightSideCss = panelOptions.SingleOrDefault(x => x.Name.ToLower() == "centernorightsidecss").Class;
				centerNoLeftOrRightSideCss = panelOptions.SingleOrDefault(x => x.Name.ToLower() == "centernoleftorrightsidecss").Class;
				centerWithLeftAndRightSideCss = panelOptions.SingleOrDefault(x => x.Name.ToLower() == "centerwithleftandrightsidecss").Class;
				emptyCenterCss = panelOptions.SingleOrDefault(x => x.Name.ToLower() == "emptycentercss").Class;
				hideEmptyCenterIfOnlySidesHaveContent = panelOptions.SingleOrDefault(x => x.Name.ToLower() == "hideemptycenterifonlysideshavecontent").Bool;
			} 
			else
			{
				Control l = FindControl("LayoutDisplaySettings1");

				if (l is not null && l is LayoutDisplaySettings)
				{
					LayoutDisplaySettings layoutSettings = l as LayoutDisplaySettings;
					leftSideNoRightSideCss = layoutSettings.LeftSideNoRightSideCss;
					rightSideNoLeftSideCss = layoutSettings.RightSideNoLeftSideCss;
					leftAndRightNoCenterCss = layoutSettings.LeftAndRightNoCenterCss;
					leftOnlyCss = layoutSettings.LeftOnlyCss;
					rightOnlyCss = layoutSettings.RightOnlyCss;
					centerNoLeftSideCss = layoutSettings.CenterNoLeftSideCss;
					centerNoRightSideCss = layoutSettings.CenterNoRightSideCss;
					centerNoLeftOrRightSideCss = layoutSettings.CenterNoLeftOrRightSideCss;
					centerWithLeftAndRightSideCss = layoutSettings.CenterWithLeftAndRightSideCss;
					emptyCenterCss = layoutSettings.EmptyCenterCss;
					hideEmptyCenterIfOnlySidesHaveContent = layoutSettings.HideEmptyCenterIfOnlySidesHaveContent;
				}
			}			
		}

		SetupLayout();
	}

	/// <summary>
	/// Count items in each of the 3 columns to determine what css class to assign to center and whether to hide side columns.
	/// This gives us automatic adjustment of column layout from 1 to 3 columns for the main layout.
	/// </summary>
	private void SetupLayout()
	{
		// Count menus if they exist within a content pane and are visible
		CountVisibleMenus();
		CountContentInstances();

		if (hideEmptyAlt1 && alt1ModuleCount == 0)
		{
			if (FindControl("divAlt1") is Panel divAlt1)
			{
				divAlt1.Visible = false;
			}
			else
			{
				if (FindControl("divAltContent") is Panel divAltContent1)
				{
					divAltContent1.Visible = false;
				}
			}
		}

		if (hideEmptyAlt2 && alt2ModuleCount == 0)
		{
			if (FindControl("divAltContent2") is Panel divAltContent2)
			{
				divAltContent2.Visible = false;
			}
		}

		// Set css classes based on count of items in each column panel
		divLeft.Visible = leftModuleCount > 0;
		divRight.Visible = rightModuleCount > 0;

		if (divLeft.Visible && !divRight.Visible)
		{
			divLeft.CssClass = leftSideNoRightSideCss;
		}

		if (divRight.Visible && !divLeft.Visible)
		{
			divRight.CssClass = rightSideNoLeftSideCss;
		}

		if (useArtisteer3)
		{
			divCenter.CssClass =
				divLeft.Visible
					? (divRight.Visible ? "art-layout-cell art-content art-content-narrow center-rightandleftmargins cmszone" : "art-layout-cell art-content center-leftmargin cmszone")
					: (divRight.Visible ? "art-layout-cell art-content center-rightmargin cmszone" : "art-layout-cell art-content-wide center-nomargins cmszone");
		}
		else
		{
			divCenter.CssClass =
				divLeft.Visible
					? (divRight.Visible ? centerWithLeftAndRightSideCss : centerNoRightSideCss)
					: (divRight.Visible ? centerNoLeftSideCss : centerNoLeftOrRightSideCss);
		}

		//https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=11210~1#post46748
		if (isCmsPage && centerModuleCount == 0)
		{
			if (leftModuleCount > 0 && rightModuleCount > 0)
			{
				if (emptyCenterCss.Length > 0)
				{
					divCenter.CssClass = emptyCenterCss;
				}

				divCenter.Visible = !hideEmptyCenterIfOnlySidesHaveContent;

				if (leftAndRightNoCenterCss.Length > 0)
				{
					divLeft.CssClass = leftAndRightNoCenterCss;
					divRight.CssClass = leftAndRightNoCenterCss;
				}

			}
			else if (leftModuleCount > 0)
			{
				if (emptyCenterCss.Length > 0)
				{
					divCenter.CssClass = emptyCenterCss;
				}

				if (leftOnlyCss.Length > 0)
				{
					divLeft.CssClass = leftOnlyCss;
				}
			}
			else if (rightModuleCount > 0)
			{
				if (emptyCenterCss.Length > 0)
				{
					divCenter.CssClass = emptyCenterCss;
				}

				if (rightOnlyCss.Length > 0)
				{
					divRight.CssClass = rightOnlyCss;
				}
			}
		}

		if (!IsPostBack)
		{
			divLeft.CssClass += " cmszone";
			divRight.CssClass += " cmszone";

			// these are optional panels that may exist in some skins
			// but are not part of the automatic column layout scheme

			if (FindControl("divAlt1") is Panel divAlt1)
			{
				divAlt1.CssClass += " cmszone";
			}

			if (FindControl("divAlt2") is Panel divAlt2)
			{
				divAlt2.CssClass += " cmszone";
			}

			if (FindControl("divAltContent2") is Panel divAltContent2)
			{
				divAltContent2.CssClass += " cmszone";
			}
		}
	}

	private void CountContentInstances()
	{
		if (Page is CmsPage && currentPage is not null)
		{
			foreach (Module module in currentPage.Modules)
			{
				//if ((module.ControlSource == "Modules/LoginModule.ascx") && (Request.IsAuthenticated)) { continue; }
				if (module.ControlSource == "Modules/LoginModule.ascx")
				{
					LoginModuleDisplaySettings loginSettings = new LoginModuleDisplaySettings();
					Controls.Add(loginSettings);
					if (Request.IsAuthenticated && loginSettings.HideWhenAuthenticated) { continue; }
				}

				if (ModuleIsVisible(module))
				{
					if (module.PaneName.IsCaseInsensitiveMatch("leftpane"))
					{
						leftModuleCount++;
					}

					if (module.PaneName.IsCaseInsensitiveMatch("rightpane"))
					{
						rightModuleCount++;
					}

					if (module.PaneName.IsCaseInsensitiveMatch("contentpane"))
					{
						centerModuleCount++;
					}

					if (module.PaneName.IsCaseInsensitiveMatch("altcontent1"))
					{
						alt1ModuleCount++;
					}

					if (module.PaneName.IsCaseInsensitiveMatch("altcontent2"))
					{
						alt2ModuleCount++;
					}
				}
			}

			// this is to make room for ModuleWrapper or custom usercontrols if they exsits anywhere in left or right
			foreach (Control c in divRight.Controls)
			{
				if (c is mojoUserControl) { rightModuleCount++; }
			}

			foreach (Control c in divLeft.Controls)
			{
				if (c is mojoUserControl) { leftModuleCount++; }
			}
		}
	}

	private void CountVisibleMenus()
	{
		// Count menus if they exist within a content pane and are visible
		if (SiteMenu1 is not null && SiteMenu1.Visible)
		{
			// printable view skin doesn't have a menu so it is null there
			if (SiteMenu1.Parent.ID == "divLeft") { leftModuleCount++; }
			if (SiteMenu1.Parent.ID == "divRight") { rightModuleCount++; }
		}

		if (FindControl("PageMenu1") is PageMenuControl p1 && p1.Visible)
		{
			if ((!p1.IsSubMenu) || (SiteUtils.TopPageHasChildren(rootNode, p1.StartingNodeOffset)))
			{
				if (p1.Parent.ID == "divLeft") { leftModuleCount++; }
				if (p1.Parent.ID == "divRight") { rightModuleCount++; }
			}
		}

		if (FindControl("PageMenu2") is PageMenuControl p2 && p2.Visible)
		{
			if (SiteUtils.TopPageHasChildren(rootNode, p2.StartingNodeOffset))
			{
				if (p2.Parent.ID == "divLeft") { leftModuleCount++; }
				if (p2.Parent.ID == "divRight") { rightModuleCount++; }
			}
		}

		if (FindControl("PageMenu3") is PageMenuControl p3 && p3.Visible)
		{
			if (SiteUtils.TopPageHasChildren(rootNode, p3.StartingNodeOffset))
			{
				if (p3.Parent.ID == "divLeft") { leftModuleCount++; }
				if (p3.Parent.ID == "divRight") { rightModuleCount++; }
			}
		}

		if (FindControl("pnlMenu") is Control pnlMenu && (pnlMenu.Parent.ID == "divLeft"))
		{
			leftModuleCount++;
		}

		if (FindControl("StyleSheetCombiner") is StyleSheetCombiner style)
		{
			if (style.AlwaysShowLeftColumn) { leftModuleCount++; }
			if (style.AlwaysShowRightColumn) { rightModuleCount++; }
		}
	}

	private bool ModuleIsVisible(Module module)
	{
		if (module.HideFromAuthenticated && Request.IsAuthenticated) { return false; }
		if (module.HideFromUnauthenticated && !Request.IsAuthenticated) { return false; }
		if (isMobileDevice && module.PublishMode == webOnly) { return false; }
		if (!isMobileDevice && module.PublishMode == mobileOnly)
		{
			if (WebConfigSettings.RolesThatAlwaysViewMobileContent.Length > 0)
			{
				if (WebUser.IsInRoles(WebConfigSettings.RolesThatAlwaysViewMobileContent)) { return true; }
			}
			return false;
		}

		return true;
	}
}