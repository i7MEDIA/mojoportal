// Author:					
// Created:				2006-08-28
// Last Modified:			2011-03-18
//		
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.		

using System.Web.UI.WebControls;
using log4net;

namespace mojoPortal.Web.UI
{
   
    public class mojoTreeView : TreeView
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(mojoTreeView));

        private bool useMenuTooltipForCustomCss = false;
        /// <summary>
        /// there are no good ways to expand MenuItem with additional properties so we are using a property for something other than its intended purposes
        /// the MenuAdapterArtisteer is used to override the rendering and there we can use the tooltip property as a way to add a custom css class to soecific menu items.
        /// Admittedly an ugly solution but no other solutions seem feasible
        /// </summary>
        public bool UseMenuTooltipForCustomCss
        {
            get { return useMenuTooltipForCustomCss; }
            set { useMenuTooltipForCustomCss = value; }
        }

        private bool disableViewStateIfNotNeeded = true;

        public bool DisableViewStateIfNotNeeded
        {
            get { return disableViewStateIfNotNeeded; }
            set { disableViewStateIfNotNeeded = value; }
        }

        private bool renderMenuText = true;

        public bool RenderMenuText
        {
            get { return renderMenuText; }
            set { renderMenuText = value; }
        }

        private bool renderCustomClassOnLi = true;

        public bool RenderCustomClassOnLi
        {
            get { return renderCustomClassOnLi; }
            set { renderCustomClassOnLi = value; }
        }

        private bool renderCustomClassOnAnchor = false;

        public bool RenderCustomClassOnAnchor
        {
            get { return renderCustomClassOnAnchor; }
            set { renderCustomClassOnAnchor = value; }
        }

        private bool useDataRole = false;

        public bool UseDataRole
        {
            get { return useDataRole; }
            set { useDataRole = value; }
        }

        private bool suppressImages = false;

        public bool SuppressImages
        {
            get { return suppressImages; }
            set { suppressImages = value; }
        }

        private string containerElement = "div";

        public string ContainerElement
        {
            get { return containerElement; }
            set { containerElement = value; }
        }

        private string containerCssClass = "AspNet-TreeView";

        public string ContainerCssClass
        {
            get { return containerCssClass; }
            set { containerCssClass = value; }
        }

        private string rootUlCssClass = string.Empty;

        public string RootUlCssClass
        {
            get { return rootUlCssClass; }
            set { rootUlCssClass = value; }
        }

        private string childUlCssClass = string.Empty;

        public string ChildUlCssClass
        {
            get { return childUlCssClass; }
            set { childUlCssClass = value; }
        }

        private bool appendDepthToChildUlCssClass = false;
        /// <summary>
        /// if true will append _n to the child ul css class where n is the depth within the current menu
        /// </summary>
        public bool AppendDepthToChildUlCssClass
        {
            get { return appendDepthToChildUlCssClass; }
            set { appendDepthToChildUlCssClass = value; }
        }

        private bool renderLiCssClasses = true;

        public bool RenderLiCssClasses
        {
            get { return renderLiCssClasses; }
            set { renderLiCssClasses = value; }
        }

        private bool renderAnchorCss = false;

        public bool RenderAnchorCss
        {
            get { return renderAnchorCss; }
            set { renderAnchorCss = value; }
        }

        private string anchorCssClass = "inactive";

        public string AnchorCssClass
        {
            get { return anchorCssClass; }
            set { anchorCssClass = value; }
        }

        private string anchorSelectedCssClass = "current";

        public string AnchorSelectedCssClass
        {
            get { return anchorSelectedCssClass; }
            set { anchorSelectedCssClass = value; }
        }

        private string expandedCssClass = "AspNet-TreeView-Collapse";

        public string ExpandedCssClass
        {
            get { return expandedCssClass; }
            set { expandedCssClass = value; }
        }

        private string collapsedCssClass = "AspNet-TreeView-Expand";

        public string CollapsedCssClass
        {
            get { return collapsedCssClass; }
            set { collapsedCssClass = value; }
        }

        private bool renderNoBreakSpaceInExpander = true;

        public bool RenderNoBreakSpaceInExpander
        {
            get { return renderNoBreakSpaceInExpander; }
            set { renderNoBreakSpaceInExpander = value; }
        }

        private string liCssClass = "AspNet-TreeView-Leaf";

        public string LiCssClass
        {
            get { return liCssClass; }
            set { liCssClass = value; }
        }

        private string liRootExpandableCssClass = "AspNet-TreeView-Root";

        public string LiRootExpandableCssClass
        {
            get { return liRootExpandableCssClass; }
            set { liRootExpandableCssClass = value; }
        }

        private string liRootNonExpandableCssClass = "AspNet-TreeView-Root AspNet-TreeView-Leaf";

        public string LiRootNonExpandableCssClass
        {
            get { return liRootNonExpandableCssClass; }
            set { liRootNonExpandableCssClass = value; }
        }

        private string liNonRootExpnadableCssClass = "AspNet-TreeView-Parent";

        public string LiNonRootExpnadableCssClass
        {
            get { return liNonRootExpnadableCssClass; }
            set { liNonRootExpnadableCssClass = value; }
        }

        private string liSelectedCssClass = "AspNet-TreeView-Selected";

        public string LiSelectedCssClass
        {
            get { return liSelectedCssClass; }
            set { liSelectedCssClass = value; }
        }

        private string liChildSelectedCssClass = "AspNet-TreeView-ChildSelected";

        public string LiChildSelectedCssClass
        {
            get { return liChildSelectedCssClass; }
            set { liChildSelectedCssClass = value; }
        }

        private string liParentSelectedCssClass = "AspNet-TreeView-ParentSelected";

        public string LiParentSelectedCssClass
        {
            get { return liParentSelectedCssClass; }
            set { liParentSelectedCssClass = value; }
        }

        //Artisteer stuff
        private bool renderNavigationHeader = false;

        public bool RenderNavigationHeader
        {
            get { return renderNavigationHeader; }
            set { renderNavigationHeader = value; }
        }

        private string navigationHeaderText = string.Empty;
        public string NavigationHeaderText
        {
            get { return navigationHeaderText; }
            set { navigationHeaderText = value; }
        }

        private string extraMarkupMode = string.Empty;

        /// <summary>
        /// valid options are empty, None, Artisteer, Custom
        /// </summary>
        public string ExtraMarkupMode
        {
            get { return extraMarkupMode; }
            set { extraMarkupMode = value; }
        }

        private string extraTopMarkup = string.Empty;

        /// <summary>
        /// literal markup added inside the begin container tag when ExtraMarkupMode == Custom
        /// </summary>
        public string ExtraTopMarkup
        {
            get { return extraTopMarkup; }
            set { extraTopMarkup = value; }
        }

        private string extraBottomMarkup = string.Empty;

        /// <summary>
        /// literal markup added before the closing container tag when ExtraMarkupMode == Custom
        /// </summary>
        public string ExtraBottomMarkup
        {
            get { return extraBottomMarkup; }
            set { extraBottomMarkup = value; }
        }

        private bool suppressCornerDivs = false;

        public bool SuppressCornerDivs
        {
            get { return suppressCornerDivs; }
            set { suppressCornerDivs = value; }
        }

        public static void ExpandToValuePath(TreeView treeView, string valuePath)
        {
            if (treeView == null) return;
            if (valuePath == null) return;

            if (!valuePath.Contains(treeView.PathSeparator.ToString()))
            {
                ExpandValuePath(treeView, valuePath);
            }
            else
            {
                string[] pathSegments
                            = valuePath.Split(new char[] { '|' });

                string pathToExpand = pathSegments[0];
                ExpandValuePath(treeView, pathToExpand);
                for (int i = 1; i < pathSegments.Length; i++)
                {
                    pathToExpand = pathToExpand + treeView.PathSeparator + pathSegments[i];
                    ExpandValuePath(treeView, pathToExpand);
                }
            }

        }

        private static void ExpandValuePath(TreeView treeView, string valuePath)
        {
            if (treeView == null) return;
            if (valuePath == null) return;

            if (valuePath.Length > 0)
            {
                TreeNode treeNode;
                treeNode = treeView.FindNode(valuePath);

                if (treeNode != null)
                {
                    if (
                        (treeNode.Expanded == null)
                        || (treeNode.Expanded.Equals(false))
                        )
                    {
                        treeNode.Expand();
                        log.Debug("Expanded treeNode found for value path " + valuePath);
                    }
                }
                else
                {
                    log.Debug(" treeNode was null for " + valuePath);
                }
            }

        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (ShowExpandCollapse)
            {
                mojoBasePage basePage = Page as mojoBasePage;
                if (basePage != null) { basePage.ScriptConfig.IncludeAspTreeView = true; }
                EnableViewState = true;
            }
            else
            {
                if (disableViewStateIfNotNeeded) { EnableViewState = false; }
            }
        }

        protected override TreeNode CreateNode()
        {
            //return base.CreateNode();
            return new mojoTreeNode();
        }

        //protected override void OnTreeNodeDataBound(TreeNodeEventArgs e)
        //{
        //    base.OnTreeNodeDataBound(e);
            
        //}

        //protected override void OnTreeNodePopulate(TreeNodeEventArgs e)
        //{
        //    base.OnTreeNodePopulate(e);
            
        //}


    }
}
