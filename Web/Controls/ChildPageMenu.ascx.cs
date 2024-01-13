using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.Extensions;
using Resources;

namespace mojoPortal.Web.UI;

[Themeable(true)]
public partial class ChildPageMenu : UserControl
{
	private PageSettings currentPage;
	private SiteMapDataSource siteMapDataSource;
	private bool treatChildPageIndexAsSiteMap = false;
	private bool isAdmin = false;
	private bool isContentAdmin = false;
	private bool isSiteEditor = false;
	private bool isMobileSkin = false;
	private int mobileOnly = (int)ContentPublishMode.MobileOnly;
	private int webOnly = (int)ContentPublishMode.WebOnly;

	public int MaximumDynamicDisplayLevels { get; set; } = 20;
	public bool UsePageImages { get; set; } = false;
	public bool HidePagesNotInSiteMap { get; set; } = false;
	public string CssClass { get; set; } = "txtnormal";
	public bool ForceDisplay { get; set; } = false;
	public int MaxRenderDepth { get; set; } = -1;
	public bool HonorSiteMapExpandSettings { get; set; } = false;
	public string Message { get; set; }

	protected void Page_Load(object sender, System.EventArgs e)
	{
		currentPage = CacheHelper.GetCurrentPage();
		treatChildPageIndexAsSiteMap = WebConfigSettings.TreatChildPageIndexAsSiteMap;

		EnableViewState = false;

		isAdmin = WebUser.IsAdmin;
		if (!isAdmin)
		{
			isContentAdmin = WebUser.IsContentAdmin;
		}

		if (!isAdmin && !isContentAdmin)
		{
			isSiteEditor = SiteUtils.UserIsSiteEditor();
		}

		isMobileSkin = SiteUtils.UseMobileSkin();

		if (WebConfigSettings.UsePageImagesInSiteMap && treatChildPageIndexAsSiteMap)
		{
			UsePageImages = true;
		}

		if (currentPage != null
			&& ((currentPage.ShowChildPageMenu && Page is CmsPage) || ForceDisplay)
			)
		{
			Visible = true;
			ShowChildPageMap();
		}
		else
		{
			Visible = false;
		}
	}

	private void ShowChildPageMap()
	{
		if (!Visible)
		{
			return;
		}

		var siteSettings = CacheHelper.GetCurrentSiteSettings();
		if (siteSettings is null)
		{
			return;
		}

		if (Page.Master.FindControl("ChildPageSiteMapData") is not SiteMapDataSource siteMapDataSource)
		{
			return;
		}

		siteMapDataSource.SiteMapProvider = $"mojosite{siteSettings.SiteId.ToInvariantString()}";

		siteMapDataSource.EnableViewState = !WebConfigSettings.DisableViewStateOnSiteMapDataSource;

		if (siteMapDataSource.Provider.FindSiteMapNode(currentPage.Url) is SiteMapNode node)
		{
			siteMapDataSource.StartingNodeUrl = node.Url;
		}
		else
		{
			node = siteMapDataSource.Provider.FindSiteMapNode(Request.RawUrl);
			if (node != null)
			{
				siteMapDataSource.StartingNodeUrl = Request.RawUrl;
			}
		}

		SiteMap1.Visible = false;
		SiteMap2.Visible = true;

		SiteMap2.PathSeparator = '|';
		SiteMap2.CollapseImageToolTip = Resource.TreeMenuCollapseTooltip;
		SiteMap2.ExpandImageToolTip = Resource.TreeMenuExpandTooltip;

		SiteMap2.TreeNodeDataBound += new TreeNodeEventHandler(SiteMap2_TreeNodeDataBound);
		SiteMap2.DataSourceID = siteMapDataSource.ID;
		SiteMap2.DataBind();
	}

	void SiteMap2_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
	{
		if (sender is null)
		{
			return;
		}

		if (e is null)
		{
			return;
		}

		TreeView menu = (TreeView)sender;
		mojoSiteMapNode mapNode = (mojoSiteMapNode)e.Node.DataItem;

		if (e.Node is mojoTreeNode)
		{
			mojoTreeNode tn = e.Node as mojoTreeNode;
			tn.HasVisibleChildren = mapNode.HasVisibleChildren();
		}

		e.Node.Value = mapNode.PageGuid.ToString();

		bool remove = false;

		if (mapNode.Roles == null)
		{
			if (!isAdmin && !isContentAdmin && !isSiteEditor)
			{
				remove = true;
			}
		}
		else
		{
			if ((!isAdmin && mapNode.Roles.Count == 1 && mapNode.Roles[0].ToString() == "Admins")
				|| (!isAdmin && !isContentAdmin && !isSiteEditor && !WebUser.IsInRoles(mapNode.Roles)))
			{
				remove = true;
			}
		}

		if (!mapNode.IncludeInChildSiteMap)
		{
			remove = true;
		}

		if (mapNode.IsPending)
		{
			if (!isAdmin
				&& !isContentAdmin
				&& !isSiteEditor
				&& !WebUser.IsInRoles(mapNode.EditRoles)
				&& !WebUser.IsInRoles(mapNode.DraftEditRoles)
				)
			{
				remove = true;
			}
		}


		if (mapNode.HideAfterLogin && Request.IsAuthenticated)
		{
			remove = true;
		}

		if (isMobileSkin)
		{
			if (mapNode.PublishMode == webOnly)
			{
				remove = true;
			}
		}
		else
		{
			if (mapNode.PublishMode == mobileOnly)
			{
				remove = true;
			}
		}

		if (MaxRenderDepth > -1)
		{
			if (e.Node.Depth > MaxRenderDepth)
			{
				remove = true;
			}
		}

		if (remove)
		{
			if (e.Node.Depth == 0)
			{
				menu.Nodes.Remove(e.Node);
			}
			else
			{
				TreeNode parent = e.Node.Parent;
				parent?.ChildNodes.Remove(e.Node);
			}
		}
		else
		{
			if (HonorSiteMapExpandSettings && menu.ShowExpandCollapse)
			{
				e.Node.Expanded = mapNode.ExpandOnSiteMap;
			}
		}
	}

	void SiteMap_MenuItemDataBound(object sender, MenuEventArgs e)
	{
		Menu menu = (Menu)sender;
		mojoSiteMapNode mapNode = (mojoSiteMapNode)e.Item.DataItem;
		if (UsePageImages && mapNode.MenuImage.Length > 0)
		{
			e.Item.ImageUrl = mapNode.MenuImage;
		}

		bool remove = false;

		if (mapNode.Roles == null)
		{
			if ((!isAdmin) && (!isContentAdmin) && (!isSiteEditor)) { remove = true; }
		}
		else
		{
			if ((!isAdmin) && (mapNode.Roles.Count == 1) && (mapNode.Roles[0].ToString() == "Admins")) { remove = true; }

			if ((!isAdmin) && (!isContentAdmin) && (!isSiteEditor) && (!WebUser.IsInRoles(mapNode.Roles))) { remove = true; }
		}

		if (treatChildPageIndexAsSiteMap || HidePagesNotInSiteMap)
		{
			if (!mapNode.IncludeInSiteMap) { remove = true; }
		}
		else
		{
			if (!mapNode.IncludeInMenu) { remove = true; }
		}

		if (mapNode.HideAfterLogin && Request.IsAuthenticated) remove = true;

		if (isMobileSkin)
		{
			if (mapNode.PublishMode == webOnly) { remove = true; }
		}
		else
		{
			if (mapNode.PublishMode == mobileOnly) { remove = true; }
		}

		if (remove)
		{
			if (e.Item.Depth == 0)
			{
				menu.Items.Remove(e.Item);
			}
			else
			{
				MenuItem parent = e.Item.Parent;
				if (parent != null)
				{
					parent.ChildItems.Remove(e.Item);
				}
			}
		}

	}
}