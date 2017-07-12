// Author:					
// Created:				    2011-03-06
// Last Modified:			2011-11-05
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
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
            // TO DO: display settings?
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
                if (tree.Nodes.Count > 0) return;
            }

            tree.DataSourceID = SiteMapData.ID;
            tree.DataBind();

        }

        void tree_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            if (sender == null) { return; }
            if (e == null) { return; }

            TreeView menu = (TreeView)sender;
            if (menu == null) { return; }

            mojoSiteMapNode mapNode = (mojoSiteMapNode)e.Node.DataItem;

            if (mapNode == null) { return; }

            e.Node.NavigateUrl = "javascript:top.window.SetPage('" + mapNode.PageId + "','" + mapNode.Title + "');";
            e.Node.Value = mapNode.PageGuid.ToString();
          
            if (e.Node is mojoTreeNode)
            {
                mojoTreeNode tn = e.Node as mojoTreeNode;
                tn.HasVisibleChildren = mapNode.HasVisibleChildren();

            }

            bool remove = false;

            if ((!isAdmin) && (!isContentAdmin) && (!isSiteEditor) && (!WebUser.IsInRoles(mapNode.CreateChildPageRoles))) { remove = true; }

            if ((!isAdmin) && (mapNode.ViewRoles == "Admins")) { remove = true; }
            

            // don't let children of this page be a choice for this page parent, its circular and causes an error
            // dont let a page be it's own parent
            if (((mapNode.ParentId == pageId)&&(pageId > -1)) || (mapNode.PageId == pageId)) { remove = true; }


            if (remove)
            {
                if (e.Node.Depth == 0)
                {
                    menu.Nodes.Remove(e.Node);
                }
                else
                {
                    TreeNode parent = e.Node.Parent;
                    if (parent != null)
                    {
                        parent.ChildNodes.Remove(e.Node);
                    }
                }
            }
            else
            {
#if !MONO
                if (mapNode.HasChildNodes)
                {
                    e.Node.PopulateOnDemand = true;
                }
#endif
            }
            


        }

        //void tree_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        //{
        //    //if (sender == null) return;
        //    //if (e == null) return;

        //    //TreeView treeView = sender as TreeView;
        //    //if (e.Node.Parent != null)
        //    //{
        //    //    mojoTreeView.ExpandToValuePath(treeView, e.Node.Parent.ValuePath);
        //    //}

        //}

        //void tree_TreeNodeCollapsed(object sender, TreeNodeEventArgs e)
        //{
        //    //if (sender == null) return;
        //    //if (e == null) return;

        //    //TreeView treeView = sender as TreeView;
        //    //if (e.Node.Parent != null)
        //    //{
        //    //    mojoTreeView.ExpandToValuePath(treeView, e.Node.Parent.ValuePath);
        //    //}
        //}

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);

            isAdmin = WebUser.IsAdmin;
            isContentAdmin = WebUser.IsContentAdmin;
            isSiteEditor = SiteUtils.UserIsSiteEditor();

            SiteMapData.SiteMapProvider = "mojosite" + siteSettings.SiteId.ToInvariantString();

            lnkRoot.Visible = isAdmin || isContentAdmin || isSiteEditor || WebUser.IsInRoles(siteSettings.RolesThatCanCreateRootPages);
            lnkRoot.Text = Resource.PageSettingsRootLabel;
            lnkRoot.NavigateUrl = "javascript:top.window.SetPage('-1','" + Resource.PageSettingsRootLabel + "');";

            litStyleLink.Text = "<link rel='stylesheet' type='text/css' href='" + Page.ResolveUrl("~/Data/style/mojotreeview/style.css") + "' /> ";

            ScriptLoader scriptLoader = Page.Master.FindControl("ScriptInclude") as ScriptLoader;
            if (scriptLoader != null) { scriptLoader.IncludeAspTreeView = true; }

           
        }

        

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            tree.TreeNodeDataBound += new TreeNodeEventHandler(tree_TreeNodeDataBound);
            //tree.TreeNodeExpanded += new TreeNodeEventHandler(tree_TreeNodeExpanded);
            //tree.TreeNodeCollapsed += new TreeNodeEventHandler(tree_TreeNodeCollapsed);
            
        }

        

        

        
    }
}