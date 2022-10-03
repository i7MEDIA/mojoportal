// Author:					i7MEDIA (Joe Davis)
// Created:					2010-03-15
// Last Modified:			2019-04-23
// You must not remove this notice, or any other, from this software.

using System;
using System.Web;
using System.Web.UI.WebControls;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Web.UI;
using mojoPortal.Web.Models;
using Resources;
namespace mojoPortal.Web.UI
{
	public partial class CustomMenu : SiteModuleControl
	{
		private bool showStartingNode = false;
		private string viewName = "_CustomMenu";
		private bool useTreeView = false;
		private int maxDepth = -1;
		private int startingPageId = -2;

		//#region OnInit
		//    protected override void OnInit(EventArgs e)
		//    {
		//        base.OnInit(e);
		//        this.Load += new EventHandler(Page_Load);
		//    }

		//#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			startingPageId = WebUtils.ParseInt32FromHashtable(Settings, "CustomMenuStartingPage", startingPageId);
			useTreeView = WebUtils.ParseBoolFromHashtable(Settings, "CustomMenuUseTreeView", useTreeView);
			maxDepth = WebUtils.ParseInt32FromHashtable(Settings, "CustomMenuMaxDepth", maxDepth);
			showStartingNode = WebUtils.ParseBoolFromHashtable(Settings, "CustomMenuShowStartingNode", showStartingNode);

			if (Settings.Contains("CustomMenuView"))
			{
				if (Settings["CustomMenuView"].ToString() != string.Empty)
				{
					viewName = Settings["CustomMenuView"].ToString();
				}
			}

			PageSettings pageSettings = new PageSettings(siteSettings.SiteId, startingPageId);
			SiteMapDataSource menuDataSource = new SiteMapDataSource();
			menuDataSource.SiteMapProvider = "mojosite" + siteSettings.SiteId.ToInvariantString();
			var model = new mojoPortal.Web.Models.CustomMenu
			{
				MenuData = menuDataSource,
				StartingPage = pageSettings,
				CurrentPage = currentPage,
				UseTreeView = useTreeView,
				ShowStartingNode = showStartingNode,
				MaxDepth = maxDepth
			};

			if (pageSettings == null || pageSettings.PageId == -2)
			{
			}
			else
			{

				SiteMapNode node = menuDataSource.Provider.FindSiteMapNode(pageSettings.Url);

				if (node != null || pageSettings.PageId == -1)
				{
					if (!useTreeView)
					{
						FlexMenu flexMenu = new FlexMenu();

						flexMenu.MaxDataRenderDepth = maxDepth;
						flexMenu.StartingNodePageId = startingPageId;
						//flexMenu.SkinID = skinID;

						if (showStartingNode)
						{
							string startingPageName = pageSettings.PageId == -1 ? "Root" : pageSettings.PageName;

							if (currentPage.PageId == pageSettings.PageId)
							{
								flexMenu.ExtraTopMarkup += $@"<ul class='{flexMenu.RootUlCssClass} {flexMenu.UlSelectedCssClass}'>
								<li class='{flexMenu.RootLevelLiCssClass} {flexMenu.LiSelectedCssClass}'>
									<a href='{WebUtils.ResolveUrl(pageSettings.Url)}' class='{flexMenu.AnchorSelectedCssClass}'>{startingPageName }</a>";
							}
							else
							{
								flexMenu.ExtraTopMarkup += $@"<ul class='{flexMenu.RootUlCssClass} {flexMenu.UlChildSelectedCssClass}'>
									<li class='{flexMenu.RootLevelLiCssClass} {flexMenu.LiChildSelectedCssClass}'>
										<a href='{WebUtils.ResolveUrl(pageSettings.Url)}' class='{flexMenu.AnchorChildSelectedCssClass}'>{startingPageName }</a>";
							}

							flexMenu.ExtraBottomMarkup += "</li></ul>";
						}

						pnlInnerBody.Controls.Add(flexMenu);
					}
					else //useTreeView
					{
						menuDataSource.StartingNodeUrl = node == null ? "~/" : node.Url;
						menuDataSource.ShowStartingNode = showStartingNode;
						mojoTreeView treeView = new mojoTreeView();
						//treeView.SkinID = skinID;
						treeView.MaxDataBindDepth = maxDepth;
						treeView.DataSource = menuDataSource;
						try
						{
							treeView.DataBind();
						}
						catch (ArgumentException ex)
						{
							//log.Error(ex);
						}

						pnlInnerBody.Controls.Add(treeView);
					}
				}
			}
		}
	}
}