// Author:					
// Created:				    2004-08-28
// Last Modified:			2013-07-15
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
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
	
	public partial class SiteMenu : UserControl
	{
	
		#region Private/Protected Properties

        //private Collection<PageSettings> menuPages;
        private SiteMapDataSource siteMapDataSource;
        private bool useTreeView = false;
        private int treeViewExpandDepth = 0;
        private bool treeViewPopulateOnDemand = true;
        private bool treeViewShowExpandCollapse = true;
        private bool treeviewPopulateNodesFromClient = true;
        private bool topLevelOnly = false;
		private string direction = "Horizontal";
        //private int pageIndex;
        private bool showPages = true;
        private bool isAdmin = false;
        private bool isContentAdmin = false;
        private bool isSiteEditor = false;
        private SiteSettings siteSettings;
        private PageSettings currentPage;
        private static readonly ILog log = LogManager.GetLogger(typeof(SiteMenu));
        private int startingNodeOffset = 0;
        private bool suppressPageSelection = false;
        private bool useSpanInLinks = false;
        private bool use3SpansInLinks = false;
        private bool useArtisteer = false;
        private bool hideMenuOnSiteMap = true;
        private bool isSecureRequest = false;
        private string secureSiteRoot = string.Empty;
        private string insecureSiteRoot = string.Empty;
        private bool resolveFullUrlsForMenuItemProtocolDifferences = false;
        private bool useSuperfish = false;
        private bool includeCornerRounders = false;
        private int dynamicDisplayLevels = 100;

        private bool enableUnclickableLinks = false;
        private string dataSourceId = "SiteMapData";

        private bool useMenuTooltipForCustomCss = true;

        private string menuSkinID = "SiteMenu";
        private bool isMobileSkin = false;
        private int mobileOnly = (int)ContentPublishMode.MobileOnly;
        private int webOnly = (int)ContentPublishMode.WebOnly;
        

		#endregion
		
		#region Public Properties

        public string DataSourceId
        {
            get { return dataSourceId; }
            set { dataSourceId = value; }
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

        /// <summary>
        /// this is now not needed, you can configure a <portal:mojoMenu for superfish via configuration settings in theme.skin
        /// </summary>
        public bool UseSuperfish
        {
            get { return useSuperfish; }
            set { useSuperfish = value; }
        }

        /// <summary>
        /// this is now not needed, you can configure a <portal:mojoMenu for Artisteer via configuration settings in theme.skin see the them.skin file in any included Artisteer skin
        /// </summary>
        public bool UseArtisteer
        {
            get { return useArtisteer; }
            set { useArtisteer = value; }
        }

        public bool SuppressPageSelection
        {
            get { return suppressPageSelection; }
            set { suppressPageSelection = value; }
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

        public bool TreeViewShowExpandCollapse
        {
            get { return treeViewShowExpandCollapse; }
            set { treeViewShowExpandCollapse = value; }
        }

        public bool TreeviewPopulateNodesFromClient
        {
            get { return treeviewPopulateNodesFromClient; }
            set { treeviewPopulateNodesFromClient = value; }
        }

        private bool suppressImages = false;

        public bool SuppressImages
        {
            get { return suppressImages; }
            set { suppressImages = value; }
        }


        public int StartingNodeOffset
        {
            get { return startingNodeOffset; }
            set { startingNodeOffset = value; }
        }

        public int DynamicDisplayLevels
        {
            get { return dynamicDisplayLevels; }
            set { dynamicDisplayLevels = value; }
        }

        public bool TopLevelOnly
        {
            get { return topLevelOnly; }
            set { topLevelOnly = value; }
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

        /// <summary>
        /// this is now not needed, you can configure a <portal:mojoMenu for span in links via configuration settings in theme.skin InnerSpanMode="SingleSpan"
        /// </summary>
        public bool UseSpanInLinks
        {
            get { return useSpanInLinks; }
            set { useSpanInLinks = value; }
        }

        /// <summary>
        /// this is now not needed, you can configure a <portal:mojoMenu for 3 spans in links via configuration settings in theme.skin InnerSpanMode="ThreeSpans"
        /// </summary>
        public bool Use3SpansInLinks
        {
            get { return use3SpansInLinks; }
            set { use3SpansInLinks = value; }
        }

        /// <summary>
        /// legacy property not used
        /// </summary>
        public bool HideMenuOnSiteMap
        {
            get { return hideMenuOnSiteMap; }
            set { hideMenuOnSiteMap = value; }
        }


        private bool useDataRole = false;

        public bool UseDataRole
        {
            get { return useDataRole; }
            set { useDataRole = value; }
        }

        private bool useFlexMenu = false;

        public bool UseFlexMenu
        {
            get { return useFlexMenu; }
            set { useFlexMenu = value; }
        }

		#endregion

		
		protected void Page_Load(object sender, EventArgs e)
		{
            String rawUrl = Request.RawUrl;
            resolveFullUrlsForMenuItemProtocolDifferences = WebConfigSettings.ResolveFullUrlsForMenuItemProtocolDifferences;
            if (resolveFullUrlsForMenuItemProtocolDifferences)
            {
                secureSiteRoot = WebUtils.GetSecureSiteRoot();
                //insecureSiteRoot = secureSiteRoot.Replace("https", "http");
                insecureSiteRoot = WebUtils.GetInSecureSiteRoot();
            }

            isSecureRequest = SiteUtils.IsSecureRequest();
            //useMenuTooltipForCustomCss = WebConfigSettings.UseMenuTooltipForCustomCss;

            if (Page is mojoBasePage)
            {
                mojoBasePage basePage = Page as mojoBasePage;
                enableUnclickableLinks = basePage.StyleCombiner.EnableNonClickablePageLinks;
                useMenuTooltipForCustomCss = basePage.StyleCombiner.UseMenuTooltipForCustomCss;
            }

            //if(
            //    (rawUrl.Contains("MyPage.aspx"))
            //    && (this.direction != "Horizontal")
            //    )
            //{
            //    this.Visible = false;
            //    return;
            //}

            if ((Direction == "Horizontal") || (!includeCornerRounders))
            {
                topRounder.Visible = false;
                bottomRounder.Visible = false;
            }
            

            isAdmin = WebUser.IsAdmin;
            if (!isAdmin) { isContentAdmin = WebUser.IsContentAdmin; }
            if ((!isAdmin) && (!isContentAdmin)) { isSiteEditor = SiteUtils.UserIsSiteEditor(); }
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            currentPage = CacheHelper.GetCurrentPage();
            isMobileSkin = SiteUtils.UseMobileSkin();

            if (siteSettings == null)
            {
                log.Error("tried to get siteSettings in Page_Load of SiteMenu.ascx but it came back null");
            }

            if (currentPage == null)
            {
                log.Error("tried to get currentPage in Page_Load of SiteMenu.ascx but it came back null");
            }

            if (
                (siteSettings != null)
                && (currentPage != null)
                )
            {
                PopulateControls();
            } 
           

        }

        

        private void PopulateControls()
        {
            bool hideMenu = siteSettings.AllowHideMenuOnPages && currentPage.HideMainMenu;
            if (showPages && !hideMenu)
            {
                if (useFlexMenu)
                {
                    FlexMenu menu = new FlexMenu();
                    menu.SkinID = menuSkinID;
                    menu.EnableTheming = true;
                    menu.IsMobileSkin = isMobileSkin;
                    this.menuPlaceHolder.Controls.Add(menu);
                }
                else
                {

                    siteMapDataSource = (SiteMapDataSource)this.Page.Master.FindControl(dataSourceId);
                    if (siteMapDataSource == null) return;

                    siteMapDataSource.SiteMapProvider = "mojosite" + siteSettings.SiteId.ToInvariantString();
                    if (WebConfigSettings.DisableViewStateOnSiteMapDataSource)
                    {
                        siteMapDataSource.EnableViewState = false;
                    }

                    if (this.useTreeView)
                    {
                        RenderTreeView();
                    }
                    else
                    {
                        RenderMenu();
                    }
                }
                
            }
        }

        
        #region ASP.NET Menu

        private void RenderMenu()
        {
            Menu menu = GetMenu();
            menu.EnableTheming = true;
            menu.SkinID = menuSkinID;
            this.menuPlaceHolder.Controls.Add(menu);
            menu.MenuItemDataBound += new MenuEventHandler(pageMenu_MenuItemDataBound);

            if (direction == "Vertical")
            {
                menu.Orientation = Orientation.Vertical;
            }
            else
            {
                menu.Orientation = Orientation.Horizontal;
            }

           
            menu.EnableViewState = false;
            menu.PathSeparator = '|';

            if (topLevelOnly)
            {
                menu.MaximumDynamicDisplayLevels = 0;
            }
            else
            {
                menu.MaximumDynamicDisplayLevels = dynamicDisplayLevels;
            }

            if (startingNodeOffset > 0)
            {
                siteMapDataSource.StartingNodeOffset = startingNodeOffset;
            }

            menu.DataSourceID = siteMapDataSource.ID;
            try
            {
                menu.DataBind();
            }
            catch (ArgumentException ex)
            {
                log.Error(ex);
            }

            DoSelecetion(menu);

            
        }

        private void DoSelecetion(Menu menu)
        {
            if (suppressPageSelection) { return; }
            bool didSelect = false;

            
            String valuePath = SiteUtils.GetActivePageValuePath(siteMapDataSource.Provider.RootNode, startingNodeOffset, Request.RawUrl);

            if (valuePath.Length > 0)
            {
                MenuItem menuItem;
                menuItem = menu.FindItem(valuePath);

                if (
                    (topLevelOnly || menu.MaximumDynamicDisplayLevels == 0)
                    && (menuItem == null)
                    && (valuePath.IndexOf(menu.PathSeparator) > -1)
                    )
                {
                    valuePath = valuePath.Substring(0, (valuePath.IndexOf(menu.PathSeparator)));
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
                valuePath = SiteUtils.GetActivePageValuePath(siteMapDataSource.Provider.RootNode, startingNodeOffset);

                if (valuePath.Length > 0)
                {
                    MenuItem menuItem;
                    menuItem = menu.FindItem(valuePath);

                    if (
                        (topLevelOnly)
                        && (menuItem == null)
                        && (valuePath.IndexOf(menu.PathSeparator) > -1)
                        )
                    {
                        valuePath = valuePath.Substring(0, (valuePath.IndexOf(menu.PathSeparator)));
                        menuItem = menu.FindItem(valuePath);

                        // http://www.mojoportal.com/Forums/Thread.aspx?thread=7277&mid=34&pageid=5&ItemID=5&pagenumber=1#post33725
                        // patch by vijaykarla 2011-05-24
                        // If the page is not Included In Menu, the above line of code wont work (returns null), following code is solution for it
                        //=========
                        if (menuItem == null)
                        {
#if NET35
                            if (!string.IsNullOrEmpty(valuePath))
#else
                            if (!string.IsNullOrWhiteSpace(valuePath))
#endif
                            {
                                int lastSeperatorIndex = valuePath.LastIndexOf(menu.PathSeparator);
                                while (lastSeperatorIndex > 0)
                                {
                                    valuePath = valuePath.Substring(0, lastSeperatorIndex);
                                    menuItem = menu.FindItem(valuePath);
                                    if (menuItem != null)
                                        break;
                                    lastSeperatorIndex = valuePath.LastIndexOf(menu.PathSeparator);
                                }
                            }
                        }
                        //=======
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
            Menu menu;

            if (useSuperfish)
            {
                if (direction == "Vertical")
                {
                    mojoMenuSuperfishVertical sv = new mojoMenuSuperfishVertical();
                    sv.UseMenuTooltipForCustomCss = useMenuTooltipForCustomCss;
                    menu = sv;
                }
                else
                {
                    mojoMenuSuperfish sf = new mojoMenuSuperfish();
                    sf.UseMenuTooltipForCustomCss = useMenuTooltipForCustomCss;
                    menu = sf;
                }
            }
            else if (useArtisteer)
            {
                mojoMenuArtisteer artMenu = new mojoMenuArtisteer();
                artMenu.UseMenuTooltipForCustomCss = useMenuTooltipForCustomCss;
                menu = artMenu;
            }
            else if (use3SpansInLinks)
            {
                mojoMenuWith3SpansInLinks m3s = new mojoMenuWith3SpansInLinks();
                m3s.UseMenuTooltipForCustomCss = useMenuTooltipForCustomCss;
                menu = m3s;
            }
            else if (UseSpanInLinks)
            {
                mojoMenuWithSpanInLinks ms = new mojoMenuWithSpanInLinks();
                ms.UseMenuTooltipForCustomCss = useMenuTooltipForCustomCss;
                menu = ms;
            }
            else
            {
                mojoMenu mm = new mojoMenu();
                mm.UseDataRole = useDataRole;
                mm.UseMenuTooltipForCustomCss = useMenuTooltipForCustomCss;
                menu = mm;
            }

            return menu;
        }

        protected void pageMenu_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            Menu menu = (Menu)sender;
            mojoSiteMapNode mapNode = (mojoSiteMapNode)e.Item.DataItem;
            if (mapNode.MenuImage.Length > 0)
            {
                e.Item.ImageUrl = mapNode.MenuImage;
            }

            //http://cssfriendly.codeplex.com/discussions/13140?ProjectName=cssfriendly
            // we are not using tooltip for menu links so we are hijacking this property to support custom css classes
            if ((useMenuTooltipForCustomCss)&&(mapNode.MenuCssClass.Length > 0))
            {
                e.Item.ToolTip = mapNode.MenuCssClass; 
            }

            if (mapNode.OpenInNewWindow)
            {
                e.Item.Target = "_blank";
            }

            if (enableUnclickableLinks) { e.Item.Selectable = mapNode.IsClickable; }

            // added this 2007-09-07
            // to solve treeview expand issue when page name is the same
            // as Page Name was used for value if not set explicitly
            e.Item.Value = mapNode.PageGuid.ToString();

            if (resolveFullUrlsForMenuItemProtocolDifferences)
            {
                if (isSecureRequest)
                {
                    if (
                        (!mapNode.UseSsl) 
                        && (!siteSettings.UseSslOnAllPages)
                        && (mapNode.Url.StartsWith("~/"))
                        )
                    {
                        e.Item.NavigateUrl = insecureSiteRoot + mapNode.Url.Replace("~/", "/");
                    }
                }
                else
                {
                    if ((mapNode.UseSsl)||(siteSettings.UseSslOnAllPages))
                    {
                        if (mapNode.Url.StartsWith("~/"))
                            e.Item.NavigateUrl = secureSiteRoot + mapNode.Url.Replace("~/", "/");
                    }
                }
            }

            bool remove = false;

            

            if (mapNode.Roles == null)
            {
                if ((!isAdmin) && (!isContentAdmin) && (!isSiteEditor)) { remove = true; }
            }
            else
            {
                if ((!isAdmin) && (mapNode.Roles.Count == 1) && (mapNode.Roles[0].ToString() == "Admins")) { remove = true; }

                if((!isAdmin)&&(!isContentAdmin)&&(!isSiteEditor)&&(!WebUser.IsInRoles(mapNode.Roles))) { remove = true;}
            }

            if ((!isAdmin) && (!isContentAdmin) && (!isSiteEditor) && (!WebUser.IsInRoles(mapNode.ViewRoles))) { remove = true; }

            if (!mapNode.IncludeInMenu) { remove = true; }

            //if (mapNode.IsPending && !WebUser.IsAdminOrContentAdminOrContentPublisherOrContentAuthor) { remove = true; }
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

            if ((mapNode.HideAfterLogin) && (Request.IsAuthenticated)) { remove = true; }

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
            if (WebConfigSettings.DisablePageViewStateByDefault) {Page.EnableViewState = true; }
#endif
            //menu1.Visible = false;

            mojoTreeView treeMenu1;
            

            if (useArtisteer)
            {
                treeMenu1 = new ArtisteerTreeView();
                treeMenu1.EnableViewState = false;
                treeMenu1.ShowExpandCollapse = false;
            }
            else
            {
                treeMenu1 = new mojoTreeView();
                
                treeMenu1.UseDataRole = useDataRole;
                treeMenu1.SuppressImages = suppressImages;

                if (treeViewShowExpandCollapse)
                {
                    treeMenu1.EnableViewState = true;
                    treeMenu1.ShowExpandCollapse = true;
                    treeMenu1.TreeNodeExpanded += new TreeNodeEventHandler(treeMenu1_TreeNodeExpanded);
                }
                else
                {
                    treeMenu1.ShowExpandCollapse = false;
#if !NET35
            //http://www.4guysfromrolla.com/articles/071410-1.aspx
            //optimize viewstate for .NET 4
               this.ViewStateMode = ViewStateMode.Disabled;
               treeMenu1.ViewStateMode = ViewStateMode.Disabled;
#endif
                }
            }

            treeMenu1.EnableTheming = true;
            treeMenu1.SkinID = menuSkinID;

            treeMenu1.UseMenuTooltipForCustomCss = useMenuTooltipForCustomCss;

            this.menuPlaceHolder.Controls.Add(treeMenu1);


#if !MONO
            treeMenu1.PopulateNodesFromClient = treeViewPopulateOnDemand;
#endif
            treeMenu1.CollapseImageToolTip = Resource.TreeMenuCollapseTooltip;
            treeMenu1.ExpandImageToolTip = Resource.TreeMenuExpandTooltip;

            treeMenu1.ExpandDepth = 0;

            
            //treeMenu1.TreeNodePopulate += new TreeNodeEventHandler(treeMenu1_TreeNodePopulate);
            treeMenu1.TreeNodeDataBound += new TreeNodeEventHandler(treeMenu1_TreeNodeDataBound);
            

            
            //older skins have this
            StyleSheet stylesheet = (StyleSheet)Page.Master.FindControl("StyleSheet");
            if (stylesheet != null)
            {
                if (stylesheet.FindControl("treeviewcss") == null)
                {
                    Literal cssLink = new Literal();
                    cssLink.ID = "treeviewcss";
                    cssLink.Text = "\n<link href='"
                    + SiteUtils.GetSkinBaseUrl(Page)
                    + "styletreeview.css' type='text/css' rel='stylesheet' media='screen' />";

                    stylesheet.Controls.Add(cssLink);
                    log.Debug("added stylesheet for treeiew");
                }
            }
                
            
            treeMenu1.ExpandDepth = treeViewExpandDepth;
            //log.Debug("set ExpandDepth to " + treeViewExpandDepth.ToString(CultureInfo.InvariantCulture));

            


            
    
            if (Page.IsPostBack)
            {
                // return if menu already bound
                if(treeMenu1.Nodes.Count > 0) return;
            }
            treeMenu1.PathSeparator = '|';
            treeMenu1.DataSourceID = this.siteMapDataSource.ID;
            try
            {
                treeMenu1.DataBind();
            }
            catch (ArgumentException ex)
            {
                log.Error(ex);
            }
            
            if (
                (treeMenu1.SelectedNode != null)
                &&(!suppressPageSelection)
                )
            {
                mojoTreeView.ExpandToValuePath(treeMenu1, treeMenu1.SelectedNode.ValuePath);
                log.Debug("called mojoTreeview.ExpandToValuePath for selectednode value path: " + treeMenu1.SelectedNode.ValuePath);
            }
            else
            {
                bool didSelect = false;

                if (!suppressPageSelection)
                {
                    String valuePath = SiteUtils.GetActivePageValuePath(siteMapDataSource.Provider.RootNode, startingNodeOffset, Request.RawUrl);
                    mojoTreeView.ExpandToValuePath(treeMenu1, valuePath);
                    log.Debug("called mojoTreeview.ExpandToValuePath for value path: " + valuePath);


                    TreeNode nodeToSelect = treeMenu1.FindNode(valuePath);
                    if (nodeToSelect != null)
                    {
                        try
                        {
                            nodeToSelect.Selected = true;
                            didSelect = true;
                            log.Debug("selected node " + nodeToSelect.Text);
                        }
                        catch (InvalidOperationException)
                        {
                            //can happen if node disabled or unselectable
                        }
                    }

                    if (!didSelect)
                    {
                        valuePath = SiteUtils.GetActivePageValuePath(siteMapDataSource.Provider.RootNode, startingNodeOffset);
                        mojoTreeView.ExpandToValuePath(treeMenu1, valuePath);
                        log.Debug("called mojoTreeview.ExpandToValuePath for value path: " + valuePath);

                        if (valuePath.Length > 0)
                        {
                            nodeToSelect = treeMenu1.FindNode(valuePath);
                            if (nodeToSelect != null)
                            {
                                try
                                {
                                    nodeToSelect.Selected = true;
                                    didSelect = true;
                                    log.Debug("selected node " + nodeToSelect.Text);
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
                        if ((currentPage != null)&&(currentPage.Url.Length > 0))
                        {
                            string u = currentPage.Url.Replace("~/", "/");
                            valuePath = SiteUtils.GetActivePageValuePath(siteMapDataSource.Provider.RootNode, startingNodeOffset, u);
                            mojoTreeView.ExpandToValuePath(treeMenu1, valuePath);

                            nodeToSelect = treeMenu1.FindNode(valuePath);
                            if (nodeToSelect != null)
                            {
                                try
                                {
                                    nodeToSelect.Selected = true;
                                    didSelect = true;
                                    log.Debug("selected node " + nodeToSelect.Text);
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
                        valuePath = SiteUtils.GetActivePageValuePath(siteMapDataSource.Provider.RootNode, startingNodeOffset);

                        if(valuePath.IndexOf(treeMenu1.PathSeparator) > -1)
                        {
                            valuePath = valuePath.Substring(0, (valuePath.IndexOf(treeMenu1.PathSeparator)));
                            nodeToSelect = treeMenu1.FindNode(valuePath);

                            if (nodeToSelect != null)
                            {
                                try
                                {
                                    nodeToSelect.Selected = true;
                                    didSelect = true;
                                    log.Debug("selected node " + nodeToSelect.Text);
                                }
                                catch (InvalidOperationException)
                                {
                                    //can happen if node disabled or unselectable
                                }
                            }
                        }
                    }

                }
            }

        }

        protected void treeMenu1_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;
            //this never seems to fire

        }

        protected void treeMenu1_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;

            TreeView treeView = sender as TreeView;
            if (e.Node.Parent != null)
            {
                mojoTreeView.ExpandToValuePath(treeView, e.Node.Parent.ValuePath);
            }
        }

        protected void treeMenu1_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;

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

            if (e.Node is mojoTreeNode)
            {
                mojoTreeNode tn = e.Node as mojoTreeNode;
                tn.HasVisibleChildren = mapNode.HasVisibleChildren();

            }


            if (resolveFullUrlsForMenuItemProtocolDifferences)
            {
                if (isSecureRequest)
                {
                    if ((!mapNode.UseSsl) && (!siteSettings.UseSslOnAllPages) && (mapNode.Url.StartsWith("~/")))
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
            //e.Node.Value = mapNode.Url;
            e.Node.Value = mapNode.PageGuid.ToString();

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
            //if (mapNode.IsPending && !WebUser.IsAdminOrContentAdminOrContentPublisherOrContentAuthor) { remove = true; }
            if (mapNode.IsPending) 
            { 
                if(
                    (!isAdmin) 
                    && (!isContentAdmin) 
                    && (!isSiteEditor)
                    &&(!WebUser.IsInRoles(mapNode.EditRoles))
                    && (!WebUser.IsInRoles(mapNode.DraftEditRoles))
                    )
                remove = true; 
            }
            if ((mapNode.HideAfterLogin) && (Request.IsAuthenticated)) { remove = true; }

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
#if !MONO
                if (mapNode.HasChildNodes)
                {
                    e.Node.PopulateOnDemand = treeViewPopulateOnDemand;
                }
#endif
            }
            
        }

        #endregion


        
		
	}
}
