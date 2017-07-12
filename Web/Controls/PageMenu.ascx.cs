// Author:					
// Created:				    2006-08-28
// Last Modified:			2014-08-28
//		
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	

using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
    public partial class PageMenuControl : UserControl
    {
        #region Private/Protected Properties

        private SiteMapDataSource pageMapDataSource;
        private bool isAdmin = false;
        private bool isContentAdmin = false;
        private bool isSiteEditor = false;
        private SiteSettings siteSettings;
        private PageSettings currentPage;
        private static readonly ILog log = LogManager.GetLogger(typeof(PageMenuControl));

        private int dynamicDisplayLevels = 100;
        private bool useTreeView = false;
        private int treeViewExpandDepth = 0;
        private bool treeViewPopulateOnDemand = true;
        private bool treeViewShowExpandCollapse = true;
        //private bool treeviewPopulateNodesFromClient = true;
        private int startingNodeOffset = 0;
        private string startingNodeUrl = string.Empty;
        private string siteMapDataSource = "PageMapDataSource";

        private bool useSpanInLinks = false;
        private bool use3SpansInLinks = false;
        private bool useSuperfish = false;

        private string direction = "Vertical";

        private int currentPageDepth = 0;
        private bool isSecureRequest = false;
        private string secureSiteRoot = string.Empty;
        private string insecureSiteRoot = string.Empty;
        private bool resolveFullUrlsForMenuItemProtocolDifferences = false;
        private bool includeCornerRounders = true;

        private bool useArtisteer = false;
        private bool isSubMenu = true;

        private bool enableUnclickableLinks = false;
        private bool useMenuTooltipForCustomCss = false;

        private string menuSkinID = "PageMenu";
        private bool isMobileSkin = false;
        private int mobileOnly = (int)ContentPublishMode.MobileOnly;
        private int webOnly = (int)ContentPublishMode.WebOnly;

        private bool useFlexMenu = false;

        

        #endregion

        #region Public Properties

        /// <summary>
        /// the server side control id of the SiteMapDataSourceControl to use
        /// </summary>
        public string SiteMapDataSource
        {
            get { return siteMapDataSource; }
            set { siteMapDataSource = value; }
        }

        public string MenuSkinID
        {
            get { return menuSkinID; }
            set { menuSkinID = value; }
        }

        public bool UseTreeView
        {
            get { return useTreeView; }
            set { useTreeView = value; }
        }

        public bool UseArtisteer
        {
            get { return useArtisteer; }
            set { useArtisteer = value; }
        }

        public bool IsSubMenu
        {
            get { return isSubMenu; }
            set { isSubMenu = value; }
        }

        public bool TreeViewShowExpandCollapse
        {
            get { return treeViewShowExpandCollapse; }
            set { treeViewShowExpandCollapse = value; }
        }

        public bool TreeViewPopulateOnDemand
        {
            get { return treeViewPopulateOnDemand; }
            set { treeViewPopulateOnDemand = value; }
        }

        public int TreeViewExpandDepth
        {
            get { return treeViewExpandDepth; }
            set { treeViewExpandDepth = value; }
        }


        //public bool TreeviewPopulateNodesFromClient
        //{
        //    get { return treeviewPopulateNodesFromClient; }
        //    set { treeviewPopulateNodesFromClient = value; }
        //}


        public int StartingNodeOffset
        {
            get { return startingNodeOffset; }
            set { startingNodeOffset = value; }
        }

        public string StartingNodeUrl
        {
            get { return startingNodeUrl; }
            set { startingNodeUrl = value; }
        }

        public int DynamicDisplayLevels
        {
            get { return dynamicDisplayLevels; }
            set { dynamicDisplayLevels = value; }
        }


        public string Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public bool IncludeCornerRounders
        {
            get { return includeCornerRounders; }
            set { includeCornerRounders = value; }
        }

        public bool UseSpanInLinks
        {
            get { return useSpanInLinks; }
            set { useSpanInLinks = value; }
        }

        public bool Use3SpansInLinks
        {
            get { return use3SpansInLinks; }
            set { use3SpansInLinks = value; }
        }

        public bool UseSuperfish
        {
            get { return useSuperfish; }
            set { useSuperfish = value; }
        }

        public bool UseFlexMenu
        {
            get { return useFlexMenu; }
            set { useFlexMenu = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            isAdmin = WebUser.IsAdmin;
            if (!isAdmin) { isContentAdmin = WebUser.IsContentAdmin; }
            if ((!isAdmin) && (!isContentAdmin)) { isSiteEditor = SiteUtils.UserIsSiteEditor(); }

            if ((Direction == "Horizontal") || (!includeCornerRounders))
            {
                topRounder.Visible = false;
                bottomRounder.Visible = false;
            }

            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }


            PopulateMenu();

            
        }

        private void PopulateMenu()
        {
            
            resolveFullUrlsForMenuItemProtocolDifferences = WebConfigSettings.ResolveFullUrlsForMenuItemProtocolDifferences;
            if (resolveFullUrlsForMenuItemProtocolDifferences)
            {
                secureSiteRoot = WebUtils.GetSecureSiteRoot();
                insecureSiteRoot = secureSiteRoot.Replace("https", "http");
            }

            isSecureRequest = SiteUtils.IsSecureRequest();

            if (Page is mojoBasePage)
            {
                mojoBasePage basePage = Page as mojoBasePage;
                enableUnclickableLinks = basePage.StyleCombiner.EnableNonClickablePageLinks;
                useMenuTooltipForCustomCss = basePage.StyleCombiner.UseMenuTooltipForCustomCss;
            }

            isMobileSkin = SiteUtils.UseMobileSkin();

            pageMapDataSource = (SiteMapDataSource)this.Page.Master.FindControl(siteMapDataSource);

            if (pageMapDataSource == null) { return; }

            pageMapDataSource.SiteMapProvider = "mojosite" + siteSettings.SiteId.ToInvariantString();
            if (WebConfigSettings.DisableViewStateOnSiteMapDataSource)
            {
                pageMapDataSource.EnableViewState = false;
            }

            

            if (startingNodeOffset > 0)
            {
                currentPageDepth = SiteUtils.GetCurrentPageDepth(pageMapDataSource.Provider.RootNode);

                if (currentPageDepth >= startingNodeOffset)
                {
                    startingNodeOffset -= 1;
                }

            }

            if ((isSubMenu) && (SiteUtils.TopPageHasChildren(pageMapDataSource.Provider.RootNode, startingNodeOffset)))
            {
                currentPage = CacheHelper.GetCurrentPage();
                bool showMenu = true;

                if (siteSettings == null)
                {
                    showMenu = false;
                    log.Error("tried to get siteSettings in Page_Load of PageeMenu.ascx but it came back null");
                }

                if (currentPage == null)
                {
                    showMenu = false;
                    log.Error("tried to get currentPage in Page_Load of PageeMenu.ascx but it came back null");
                }

                if (
                    (siteSettings != null)
                    && (currentPage != null)
                    && (startingNodeUrl.Length == 0)
                    && (siteSettings.AllowHideMenuOnPages)
                    )
                {
                    if (currentPage.HideMainMenu)
                    {
                        showMenu = false;
                    }
                }
                if (showMenu)
                {
                    // isAdmin = WebUser.IsAdmin;
                    // isContentAdmin = WebUser.IsContentAdmin;
                    if (useFlexMenu)
                    {
                        RenderFlexMenu();
                    }
                    else if (useTreeView)
                    {
                        RenderTreeView();
                    }
                    else
                    {
                        RenderMenu();
                    }
                }
                else
                {
                    this.Visible = false;
                }
            }
            else if (!isSubMenu)
            {
                // isAdmin = WebUser.IsAdmin;
                // isContentAdmin = WebUser.IsContentAdmin;
                if (useFlexMenu)
                {
                    RenderFlexMenu();
                }
                else if (useTreeView)
                {
                    RenderTreeView();
                }
                else
                {
                    RenderMenu();
                }
            }
            else
            {
                this.Visible = false;
                this.EnableViewState = false;
            }

        }

        private void RenderFlexMenu()
        {
            FlexMenu menu = new FlexMenu();
            menu.SkinID = menuSkinID;
            menu.EnableTheming = true;
            menu.IsMobileSkin = isMobileSkin;
            this.menuPlaceHolder.Controls.Add(menu);
        }


        #region ASP.NET Menu

        private void RenderMenu()
        {
            Menu pageMenu = GetMenu();
            pageMenu.EnableTheming = true;
            pageMenu.SkinID = menuSkinID;
            this.menuPlaceHolder.Controls.Add(pageMenu);
            pageMenu.MenuItemDataBound += new MenuEventHandler(pageMenu_MenuItemDataBound);

            if (direction == "Vertical")
            {
                pageMenu.Orientation = Orientation.Vertical;
            }
            else
            {
                pageMenu.Orientation = Orientation.Horizontal;
            }

            pageMenu.EnableViewState = false;
            this.EnableViewState = false;
            pageMenu.PathSeparator = '|';

            if (currentPage == null) { currentPage = CacheHelper.GetCurrentPage(); }
            
            if (isSubMenu)
            {
                if (
                    (currentPage != null)
                    && (currentPage.ParentId == -1)
                    )
                {  // this is a root level page

                    if (
                        (currentPage.UseUrl)
                        && (currentPage.Url.Length > 0)
                        &&(WebConfigSettings.UseUrlReWriting)
                        )
                    {
                        pageMapDataSource.StartingNodeUrl = currentPage.Url;
                    }
                    else
                    {
                        pageMapDataSource.StartingNodeUrl = "~/Default.aspx?pageid=" + currentPage.PageId.ToString();
                    }


                }
                else
                {
                    // not a root level page
                    pageMapDataSource.StartingNodeUrl = SiteUtils.GetStartUrlForPageMenu(pageMapDataSource.Provider.RootNode, startingNodeOffset);
                }
            }

            if (startingNodeUrl.Length > 0)
            {
                pageMapDataSource.StartingNodeUrl = startingNodeUrl;

            }

           
            pageMenu.MaximumDynamicDisplayLevels = dynamicDisplayLevels;
            
            pageMenu.DataSourceID = pageMapDataSource.ID;
            try
            {
                pageMenu.DataBind();
            }
            catch (ArgumentException ex)
            {
                log.Error(ex);
            }

            DoMenuSelection(pageMenu);

            
            if (pageMenu.Items.Count == 0) this.Visible = false;

        }

        private void DoMenuSelection(Menu menu)
        {
            // TODO: clean up this hairy mess without breaking anything

            MenuItem menuItem = null;
            bool didSelect = false;
            string valuePath;

            if (isSubMenu)
            {
                valuePath = SiteUtils.GetPageMenuActivePageValuePath(pageMapDataSource.Provider.RootNode);
            }
            else
            {
                valuePath = SiteUtils.GetActivePageValuePath(pageMapDataSource.Provider.RootNode, startingNodeOffset, Request.RawUrl);
            }

            if (valuePath.Length > 0)
            {
                menuItem = menu.FindItem(valuePath);

                if (menuItem == null)
                {
                    if (startingNodeOffset > 0)
                    {
                        for (int i = 1; i <= startingNodeOffset; i++)
                        {
                            if (valuePath.IndexOf("|") > -1)
                            {
                                valuePath = valuePath.Remove(0, valuePath.IndexOf("|") + 1);
                            }

                        }
                    }
                }



                if (menuItem == null)
                {
                    valuePath = SiteUtils.GetPageMenuActivePageValuePath(pageMapDataSource.Provider.RootNode);
                    menuItem = menu.FindItem(valuePath);
                }

                if (menuItem != null)
                {
                    try
                    {
                        menuItem.Selected = true;
                        didSelect = true;
                    }
                    catch (InvalidOperationException)
                    {
                        //can happen if node disabled or unselectable
                    }
                }
            }

            if (!didSelect)
            {
                valuePath = SiteUtils.GetActivePageValuePath(pageMapDataSource.Provider.RootNode, startingNodeOffset);


                if (valuePath.Length > 0)
                {

                    menuItem = menu.FindItem(valuePath);

                    if (
                         (menuItem == null)
                         && (valuePath.IndexOf(menu.PathSeparator) > -1)
                        )
                    {
                        valuePath = valuePath.Substring(0, (valuePath.IndexOf(menu.PathSeparator)));
                        menuItem = menu.FindItem(valuePath);
                    }

                    if (
                        (dynamicDisplayLevels == 0)
                        && (menuItem == null)
                        && (valuePath.IndexOf(menu.PathSeparator) > -1)
                        )
                    {

                        foreach (MenuItem m in menu.Items)
                        {
                            if (valuePath.Contains(m.ValuePath))
                            {
                                try
                                {
                                    m.Selected = true;
                                    didSelect = true;
                                }
                                catch (InvalidOperationException)
                                {
                                    //can happen if node disabled or unselectable
                                }
                                return;
                            }

                        }
                    }


                    if (menuItem != null)
                    {
                        try
                        {
                            menuItem.Selected = true;
                            didSelect = true;
                        }
                        catch (InvalidOperationException)
                        {
                            //can happen if node disabled or unselectable
                        }
                    }

                }
            }

            if (!didSelect)
            {
                valuePath = SiteUtils.GetActivePageValuePath(pageMapDataSource.Provider.RootNode, startingNodeOffset, Request.RawUrl);

                if (valuePath.Length > 0)
                {

                    menuItem = menu.FindItem(valuePath);

                    if (menuItem == null)
                    {
                        if (currentPage == null) { currentPage = CacheHelper.GetCurrentPage(); }
                        if (currentPage != null)
                        {
                            menuItem = menu.FindItem(currentPage.PageGuid.ToString());
                        }
                    }


                    if (
                        (dynamicDisplayLevels == 0)
                        && (menuItem == null)
                        && (valuePath.IndexOf(menu.PathSeparator) > -1)
                        )
                    {

                        foreach (MenuItem m in menu.Items)
                        {
                            if (valuePath.Contains(m.ValuePath))
                            {
                                try
                                {
                                    m.Selected = true;
                                }
                                catch (InvalidOperationException)
                                {
                                    //can happen if node disabled or unselectable
                                }
                                return;
                            }

                        }
                    }

                    if (menuItem != null)
                    {
                        try
                        {
                            menuItem.Selected = true;
                            didSelect = true;
                        }
                        catch (InvalidOperationException)
                        {
                            //can happen if node disabled or unselectable
                        }
                    }

                }
            }


        }

        private Menu GetMenu()
        {
            Menu pageMenu;

            if (useSuperfish)
            {
                if (direction == "Vertical")
                {
                    mojoMenuSuperfishVertical sv = new mojoMenuSuperfishVertical();
                    sv.UseMenuTooltipForCustomCss = useMenuTooltipForCustomCss;
                    pageMenu = sv;
                }
                else
                {
                    mojoMenuSuperfish sf = new mojoMenuSuperfish();
                    sf.UseMenuTooltipForCustomCss = useMenuTooltipForCustomCss;
                    pageMenu = sf;
                }
            }
            else if (useArtisteer)
            {
                mojoMenuArtisteerVertical av = new mojoMenuArtisteerVertical();
                av.UseMenuTooltipForCustomCss = useMenuTooltipForCustomCss;
                pageMenu = av;
            }
            else if (use3SpansInLinks)
            {
                mojoMenuWith3SpansInLinks m3s = new mojoMenuWith3SpansInLinks();
                m3s.UseMenuTooltipForCustomCss = useMenuTooltipForCustomCss;
                pageMenu = m3s;
            }
            else if (UseSpanInLinks)
            {
                mojoMenuWithSpanInLinks ms = new mojoMenuWithSpanInLinks();
                ms.UseMenuTooltipForCustomCss = useMenuTooltipForCustomCss;
                pageMenu = ms;
            }
            else
            {
                mojoMenu mm = new mojoMenu();
                mm.UseMenuTooltipForCustomCss = useMenuTooltipForCustomCss;
                pageMenu = mm;
            }

            return pageMenu;


        }

        void pageMenu_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;

            Menu menu = (Menu)sender;
            mojoSiteMapNode mapNode = (mojoSiteMapNode)e.Item.DataItem;
            if (mapNode.MenuImage.Length > 0)
            {
                e.Item.ImageUrl = mapNode.MenuImage;
            }

            if (mapNode.OpenInNewWindow)
            {
                e.Item.Target = "_blank";
            }

            if ((useMenuTooltipForCustomCss) && (mapNode.MenuCssClass.Length > 0))
            {
                e.Item.ToolTip = mapNode.MenuCssClass;
            }

            if (enableUnclickableLinks) { e.Item.Selectable = mapNode.IsClickable; }

            if (resolveFullUrlsForMenuItemProtocolDifferences)
            {
                if (isSecureRequest)
                {
                    if ((!mapNode.UseSsl) && (!siteSettings.UseSslOnAllPages) && (mapNode.Url.StartsWith("~/")))
                    {
                        e.Item.NavigateUrl = insecureSiteRoot + mapNode.Url.Replace("~/", "/");
                    }
                }
                else
                {
                    if ((mapNode.UseSsl) || (siteSettings.UseSslOnAllPages))
                    {
                        if (mapNode.Url.StartsWith("~/"))
                            e.Item.NavigateUrl = secureSiteRoot + mapNode.Url.Replace("~/", "/");
                    }
                }
            }

            // added this 2007-09-07
            // to solve treeview expand issue when page name is the same
            // as Page Name was used for value if not set explicitly
            e.Item.Value = mapNode.PageGuid.ToString();

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

            if ((!isAdmin) && (!isContentAdmin) && (!isSiteEditor) && (!WebUser.IsInRoles(mapNode.ViewRoles))) { remove = true; }

            if (!mapNode.IncludeInMenu) remove = true;
            //if (mapNode.IsPending && !WebUser.IsAdminOrContentAdminOrContentPublisherOrContentAuthor) remove = true;
            if (mapNode.IsPending)
            {
                if (
                    (!isAdmin)
                    && (!isContentAdmin)
                    && (!isSiteEditor)
                    && (!WebUser.IsInRoles(mapNode.EditRoles))
                    && (!WebUser.IsInRoles(mapNode.DraftEditRoles))
                    )
                    remove = true;
            }
            
            if ((mapNode.HideAfterLogin) && (Request.IsAuthenticated)) remove = true;

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


        #endregion

        #region TreeView

        private void RenderTreeView()
        {
#if NET35
            if (WebConfigSettings.DisablePageViewStateByDefault) { Page.EnableViewState = true; }
#endif
          
            if (currentPage == null) { currentPage = CacheHelper.GetCurrentPage(); }

            mojoTreeView treeMenu1;

            if (useArtisteer)
            {
                treeMenu1 = new ArtisteerTreeView();
            }
            else
            {
                treeMenu1 = new mojoTreeView();
            }

            treeMenu1.EnableTheming = true;
            treeMenu1.SkinID = menuSkinID;

            

            treeMenu1.ShowExpandCollapse = treeViewShowExpandCollapse;
            treeMenu1.UseMenuTooltipForCustomCss = useMenuTooltipForCustomCss;

            if (treeViewShowExpandCollapse)
            {
                treeMenu1.ShowExpandCollapse = true;
                treeMenu1.EnableViewState = true;
                treeMenu1.TreeNodeExpanded += new TreeNodeEventHandler(treeMenu1_TreeNodeExpanded);
                //this.EnableViewState = false;
            }
            else
            {
                treeMenu1.ShowExpandCollapse = false;
                treeMenu1.EnableViewState = false;
                this.EnableViewState = false;
            }

           
            treeMenu1.PopulateNodesFromClient = treeViewPopulateOnDemand;
            
            treeMenu1.ExpandDepth = treeViewExpandDepth;

            treeMenu1.TreeNodeDataBound += new TreeNodeEventHandler(treeMenu1_TreeNodeDataBound);

            treeMenu1.CollapseImageToolTip = Resource.TreeMenuCollapseTooltip;
            treeMenu1.ExpandImageToolTip = Resource.TreeMenuExpandTooltip;

            this.menuPlaceHolder.Controls.Add(treeMenu1);

            if (isSubMenu)
            {
                if (
                    (currentPage != null)
                    && (currentPage.ParentId == -1)
                    )
                {
                    if (
                        (currentPage.UseUrl)
                        && (currentPage.Url.Length > 0)
                        &&(WebConfigSettings.UseUrlReWriting)
                        )
                    {
                        pageMapDataSource.StartingNodeUrl = currentPage.Url;
                    }
                    else
                    {
                        pageMapDataSource.StartingNodeUrl = "~/Default.aspx?pageid=" + currentPage.PageId.ToInvariantString();
                    }

                }
                else
                {
                    pageMapDataSource.StartingNodeUrl = SiteUtils.GetStartUrlForPageMenu(pageMapDataSource.Provider.RootNode, startingNodeOffset);
                }
            }

            if (startingNodeUrl.Length > 0)
            {
                pageMapDataSource.StartingNodeUrl = startingNodeUrl;

            }

            if (Page.IsPostBack)
            {
                // return if menu already bound
                if (treeMenu1.Nodes.Count > 0) return;
            }

            
            if (startingNodeOffset > (currentPageDepth + 1))
            {
                this.Visible = false;
                return;
            }

            treeMenu1.PathSeparator = '|';
            treeMenu1.DataSourceID = pageMapDataSource.ID;
            try
            {
                treeMenu1.DataBind();
            }
            catch (ArgumentException ex)
            {
                log.Error(ex);
            }

            

            if (treeMenu1.SelectedNode != null)
            {
                mojoTreeView.ExpandToValuePath(treeMenu1, treeMenu1.SelectedNode.ValuePath);
            }
            else
            {
                
                //bool didSelect = false;
                String valuePath = string.Empty;
                if (isSubMenu)
                {
                    valuePath = SiteUtils.GetPageMenuActivePageValuePath(pageMapDataSource.Provider.RootNode);
                    if (startingNodeOffset > 0)
                    {
                        for (int i = 1; i <= startingNodeOffset; i++)
                        {
                            if (valuePath.IndexOf("|") > -1)
                            {
                                valuePath = valuePath.Remove(0, valuePath.IndexOf("|") + 1);
                            }

                        }
                    }
                }
                else
                {
                    valuePath = SiteUtils.GetActivePageValuePath(pageMapDataSource.Provider.RootNode, startingNodeOffset, Request.RawUrl);
                }

                mojoTreeView.ExpandToValuePath(treeMenu1, valuePath);
                
                TreeNode nodeToSelect = treeMenu1.FindNode(valuePath);
                if (nodeToSelect == null)
                {
                    nodeToSelect = treeMenu1.FindNode(currentPage.PageName);
                }

                if (nodeToSelect != null)
                {
                    try
                    {
                        nodeToSelect.Selected = true;
                        //didSelect = true;
                    }
                    catch (InvalidOperationException)
                    {
                        //can happen if node disabled or unselectable
                    }
                }

                

            }

            if (treeMenu1.Nodes.Count == 0) this.Visible = false;

        }

        protected void treeMenu1_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;
            //this never seems to fire

        }

        protected void treeMenu1_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            if (sender == null) { return; }
            if (e == null) { return; }

            TreeView treeView = sender as TreeView;
            mojoTreeView.ExpandToValuePath(treeView, e.Node.ValuePath);
            
        }

        protected void treeMenu1_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            if (sender == null) { return; }
            if (e == null) { return; }

            

            TreeView menu = (TreeView)sender;
            mojoSiteMapNode mapNode = (mojoSiteMapNode)e.Node.DataItem;
            if (mapNode.MenuImage.Length > 0)
            {
                e.Node.ImageUrl = mapNode.MenuImage;
            }

            if ((useMenuTooltipForCustomCss) && (mapNode.MenuCssClass.Length > 0))
            {
                e.Node.ToolTip = mapNode.MenuCssClass;
            }

            if (treeViewShowExpandCollapse)
            {
                if (e.Node is mojoTreeNode)
                {
                    mojoTreeNode tn = e.Node as mojoTreeNode;
                    tn.HasVisibleChildren = mapNode.HasVisibleChildren();

                }
            }

            

            if (resolveFullUrlsForMenuItemProtocolDifferences)
            {
                if (isSecureRequest)
                {
                    if ((!mapNode.UseSsl) &&(!siteSettings.UseSslOnAllPages) && (mapNode.Url.StartsWith("~/")))
                    {
                        e.Node.NavigateUrl = insecureSiteRoot + mapNode.Url.Replace("~/", "/");
                    }
                }
                else
                {
                    if ((mapNode.UseSsl) || (siteSettings.UseSslOnAllPages))
                    {
                        if (mapNode.Url.StartsWith("~/"))
                            e.Node.NavigateUrl = secureSiteRoot + mapNode.Url.Replace("~/", "/");
                    }
                }
            }

            if (mapNode.OpenInNewWindow)
            {
                e.Node.Target = "_blank";
            }

            

            // added this 2007-09-07
            // to solve treeview expand issue when page name is the same
            // as Page Name was used for value if not set explicitly
            e.Node.Value = mapNode.PageGuid.ToString();

            //log.Info("databound tree node with value path " + e.Node.ValuePath);

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

            if ((!isAdmin) && (!isContentAdmin) && (!isSiteEditor) && (!WebUser.IsInRoles(mapNode.ViewRoles))) { remove = true; }


            if (!mapNode.IncludeInMenu) remove = true;
            //if (mapNode.IsPending && !WebUser.IsAdminOrContentAdminOrContentPublisherOrContentAuthor) remove = true;
            if (mapNode.IsPending)
            {
                if (
                    (!isAdmin)
                    && (!isContentAdmin)
                    && (!isSiteEditor)
                    && (!WebUser.IsInRoles(mapNode.EditRoles))
                    && (!WebUser.IsInRoles(mapNode.DraftEditRoles))
                    )
                    remove = true;
            }
            
            if ((mapNode.HideAfterLogin) && (Request.IsAuthenticated)) remove = true;

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
                if (mapNode.HasVisibleChildren())
                {
                    e.Node.PopulateOnDemand = treeViewPopulateOnDemand;
                }
                else
                {
                    e.Node.PopulateOnDemand = false;
                }
            }

        }

        

        #endregion

    }
}
