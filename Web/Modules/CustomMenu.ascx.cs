using System;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
	public partial class CustomMenu : SiteModuleControl
	{
		private bool showStartingNode = false;
		private string viewName = "_CustomMenu";
		private int maxDepth = -1;
		private int startingPageId = -2;
		private static readonly ILog log = LogManager.GetLogger(typeof(CustomMenu));

		protected void Page_Load(object sender, EventArgs e)
		{
			startingPageId = WebUtils.ParseInt32FromHashtable(Settings, "CustomMenuStartingPage", startingPageId);
			maxDepth = WebUtils.ParseInt32FromHashtable(Settings, "CustomMenuMaxDepth", maxDepth);
			showStartingNode = WebUtils.ParseBoolFromHashtable(Settings, "CustomMenuShowStartingNode", showStartingNode);

			if (Settings.Contains("CustomMenuView"))
			{
				if (Settings["CustomMenuView"].ToString() != string.Empty)
				{
					viewName = Settings["CustomMenuView"].ToString();
				}
			}

			PageSettings startingPage = new(siteSettings.SiteId, startingPageId);

			SiteMapDataSource menuDataSource = new()
			{
				SiteMapProvider = "mojosite" + siteSettings.SiteId.ToInvariantString()
			};

			var startingNode = menuDataSource.Provider.RootNode;
			if (startingPageId > -1 && startingPage != null)
			{
				startingNode = menuDataSource.Provider.FindSiteMapNode(startingPage.Url);
			}

			var model = new mojoPortal.Web.Models.CustomMenu
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
				renderDefaultView(ex.ToString());
			}

			void renderDefaultView(string error = "")
			{
				if (!string.IsNullOrWhiteSpace(error))
				{
					log.ErrorFormat(
						"chosen layout ({0}) for _CustomMenu was not found in skin {1}. perhaps it is in a different skin. Error was: {2}",
						viewName,
					SiteUtils.GetSkinBaseUrl(true, Page),
					error
					);
				}

				lit1.Text = RazorBridge.RenderPartialToString("_CustomMenu", model, "Common");
			}
		}
	}
}