/// Author:				    
/// Created:			    2006-10-01
/// Last Modified:		    2012-10-12
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI.Pages
{

    public partial class SiteMapPage : NonCmsBasePage
    {
        protected SiteMapDataSource siteMapDataSource;

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            //SiteMap1.MenuItemDataBound += new MenuEventHandler(SiteMap1_MenuItemDataBound);

            if (WebConfigSettings.HideMenusOnSiteMap)
            {
                SuppressAllMenus();
            }
            else
            {
                SuppressMenuSelection();
                if (WebConfigSettings.HidePageMenusOnSiteMap) { SuppressPageMenu(); }
            }

            if (base.StyleCombiner != null)
            {
                treeViewPopulateOnDemand = base.StyleCombiner.SiteMapPopulateOnDemand;
                treeViewExpandDepth = base.StyleCombiner.SiteMapExpandDepth;
            }

            menu.TreeNodeDataBound += new TreeNodeEventHandler(menu_TreeNodeDataBound);
            if (treeViewPopulateOnDemand)
            {
                menu.EnableViewState = true;
                menu.TreeNodeExpanded += new TreeNodeEventHandler(menu_TreeNodeExpanded);
            }
            else
            {
                menu.EnableViewState = false;
                //treeMenu1.EnableClientScript = true;
            }
        }
        #endregion

        private bool useImagesInSiteMap = false;
        private bool resolveFullUrlsForMenuItemProtocolDifferences = false;
        private bool isSecureRequest = false;
        private string secureSiteRoot = string.Empty;
        private string insecureSiteRoot = string.Empty;
        private bool treeViewPopulateOnDemand = true;
        private int treeViewExpandDepth = 0;
        private bool useMenuTooltipForCustomCss = false;
        private bool isMobileSkin = false;
        private int mobileOnly = (int)ContentPublishMode.MobileOnly;
        private int webOnly = (int)ContentPublishMode.WebOnly;
        private bool isAdmin = false;
        private bool isContentAdmin = false;
        private bool isSiteEditor = false;


        protected void Page_Load(object sender, EventArgs e)
        {
            if ((SiteUtils.SslIsAvailable()) && ((WebConfigSettings.UseSslForSiteMap)||(siteSettings.UseSslOnAllPages)))
            {
                SiteUtils.ForceSsl();
            }
            else
            {
                SiteUtils.ClearSsl();
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.SiteMapLink);
            heading.Text = Resource.SiteMapLink;

            //this page has no content other than nav
            SiteUtils.AddNoIndexFollowMeta(Page);
            

            resolveFullUrlsForMenuItemProtocolDifferences = WebConfigSettings.ResolveFullUrlsForMenuItemProtocolDifferences;
            if (resolveFullUrlsForMenuItemProtocolDifferences)
            {
                secureSiteRoot = WebUtils.GetSecureSiteRoot();
                insecureSiteRoot = secureSiteRoot.Replace("https", "http");
            }

            isSecureRequest = SiteUtils.IsSecureRequest();
            isMobileSkin = SiteUtils.UseMobileSkin();
            isAdmin = WebUser.IsAdmin;
            if (!isAdmin) { isContentAdmin = WebUser.IsContentAdmin; }
            if ((!isAdmin) && (!isContentAdmin)) { isSiteEditor = SiteUtils.UserIsSiteEditor(); }

            useMenuTooltipForCustomCss = StyleCombiner.UseMenuTooltipForCustomCss;

            MetaDescription = string.Format(CultureInfo.InvariantCulture,
                Resource.MetaDescriptionSiteMapFormat, siteSettings.SiteName);

            siteMapDataSource = (SiteMapDataSource)this.Page.Master.FindControl("SiteMapData");

            siteMapDataSource.SiteMapProvider = "mojosite" + siteSettings.SiteId.ToInvariantString();

            if (Request.Params["startnode"] != null)
            {
                string startNode = Server.UrlDecode(Request.Params["startnode"]);
                SiteMapNode node
                    = siteMapDataSource.Provider.FindSiteMapNode(startNode);
                if (node != null)
                {
                    siteMapDataSource.StartingNodeUrl = startNode;
                }
            }

            useImagesInSiteMap = WebConfigSettings.UsePageImagesInSiteMap;

            AddClassToBody("sitemappage");
            

            RenderSiteMap();
        }

        private void RenderSiteMap()
        {

            menu.UseMenuTooltipForCustomCss = useMenuTooltipForCustomCss;
            menu.PathSeparator = '|';
            //menu.ShowExpandCollapse = treeViewPopulateOnDemand;
            //menu.PopulateNodesFromClient = treeViewPopulateOnDemand;
            //menu.ExpandDepth = treeViewExpandDepth;
            menu.CollapseImageToolTip = Resource.TreeMenuCollapseTooltip;
            menu.ExpandImageToolTip = Resource.TreeMenuExpandTooltip;

            if (Page.IsPostBack)
            {
                // return if menu already bound
                if (menu.Nodes.Count > 0) return;
            }

            //this.menuPlaceHolder.Controls.Add(menu);

            menu.DataSourceID = this.siteMapDataSource.ID;
            menu.DataBind();

            
        }

        void menu_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            if (sender == null) { return; }
            if (e == null) { return; }

            TreeView treeView = sender as TreeView;
            mojoTreeView.ExpandToValuePath(treeView, e.Node.ValuePath);
        }

        void menu_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            TreeView menu = (TreeView)sender;
            mojoSiteMapNode mapNode = (mojoSiteMapNode)e.Node.DataItem;

            if ((useImagesInSiteMap)&&(mapNode.MenuImage.Length > 0))
            {
                e.Node.ImageUrl = mapNode.MenuImage;
            }

            if (mapNode.OpenInNewWindow)
            {
                e.Node.Target = "_blank";
            }

            if (useMenuTooltipForCustomCss)
            {
                e.Node.ToolTip = mapNode.MenuCssClass;
            }

            //if (treeViewPopulateOnDemand)
            //{
                if (e.Node is mojoTreeNode)
                {
                    mojoTreeNode tn = e.Node as mojoTreeNode;
                    tn.HasVisibleChildren = mapNode.HasVisibleChildren();

                }
           // }

            // added this 2007-09-07
            // to solve treeview expand issue when page name is the same
            // as Page Name was used for value if not set explicitly
            e.Node.Value = mapNode.PageGuid.ToString();

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

            if (!mapNode.IncludeInSiteMap) { remove = true; }
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
                    //e.Node.PopulateOnDemand = treeViewPopulateOnDemand;
                    e.Node.PopulateOnDemand = menu.PopulateNodesFromClient;
                }

                if (menu.ShowExpandCollapse)
                {
                    e.Node.Expanded = mapNode.ExpandOnSiteMap;
                }
            }
        }
        

       

       
    }
}

