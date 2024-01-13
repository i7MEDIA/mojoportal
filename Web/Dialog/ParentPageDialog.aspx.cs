using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.Extensions;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI;

public partial class ParentPageDialog : mojoDialogBasePage
{
	private int pageId = -1;
	private SiteSettings siteSettings = null;
	private bool isAdmin = false;
	private bool isContentAdmin = false;
	private bool isSiteEditor = false;

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
		BindTree();
	}

	private void BindTree()
	{
		tree.UseMenuTooltipForCustomCss = false;
		tree.PathSeparator = '|';
		tree.ShowExpandCollapse = true;
		tree.PopulateNodesFromClient = true;
		tree.ExpandDepth = WebConfigSettings.ParentPageDialogExpansionDepth;
		tree.CollapseImageToolTip = Resource.TreeMenuCollapseTooltip;
		tree.ExpandImageToolTip = Resource.TreeMenuExpandTooltip;
		tree.SuppressImages = true;

		if (Page.IsPostBack)
		{
			// return if already bound
			if (tree.Nodes.Count > 0)
			{
				return;
			}
		}

		tree.DataSourceID = SiteMapData.ID;
		tree.DataBind();
	}

	void tree_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
	{
		if (sender is null) { return; }
		if (e is null) { return; }

		var menu = (TreeView)sender;
		if (menu is null) { return; }

		var mapNode = (mojoSiteMapNode)e.Node.DataItem;

		if (mapNode is null) { return; }

		e.Node.NavigateUrl = "javascript:top.window.SetPage('" + mapNode.PageId + "','" + mapNode.Title + "');";
		e.Node.Value = mapNode.PageGuid.ToString();

		if (e.Node is mojoTreeNode)
		{
			mojoTreeNode tn = e.Node as mojoTreeNode;
			tn.HasVisibleChildren = mapNode.HasVisibleChildren();
		}

		bool remove = false;

		if (!isAdmin
			&& !isContentAdmin
			&& !isSiteEditor
			&& !WebUser.IsInRoles(mapNode.CreateChildPageRoles))
		{
			remove = true;
		}

		if (!isAdmin && mapNode.ViewRoles == "Admins")
		{
			remove = true;
		}


		// don't let children of this page be a choice for this page parent, its circular and causes an error
		// dont let a page be it's own parent
		if ((mapNode.ParentId == pageId && pageId > -1) || mapNode.PageId == pageId)
		{
			remove = true;
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
			if (mapNode.HasChildNodes)
			{
				e.Node.PopulateOnDemand = true;
			}
		}
	}

	private void LoadSettings()
	{
		siteSettings = CacheHelper.GetCurrentSiteSettings();
		pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);

		isAdmin = WebUser.IsAdmin;
		isContentAdmin = WebUser.IsContentAdmin;
		isSiteEditor = SiteUtils.UserIsSiteEditor();

		SiteMapData.SiteMapProvider = $"mojosite{siteSettings.SiteId.ToInvariantString()}";

		lnkRoot.Visible = isAdmin || isContentAdmin || isSiteEditor || WebUser.IsInRoles(siteSettings.RolesThatCanCreateRootPages);
		lnkRoot.Text = Resource.PageSettingsRootLabel;
		lnkRoot.NavigateUrl = $"javascript:top.window.SetPage('-1','{Resource.PageSettingsRootLabel}');";

		litStyleLink.Text = $"<link rel='stylesheet' type='text/css' href='{Page.ResolveUrl("~/Data/style/mojotreeview/style.css")}' /> ";

		if (Page.Master.FindControl("ScriptInclude") is ScriptLoader scriptLoader) { scriptLoader.IncludeAspTreeView = true; }
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		tree.TreeNodeDataBound += new TreeNodeEventHandler(tree_TreeNodeDataBound);
	}
}