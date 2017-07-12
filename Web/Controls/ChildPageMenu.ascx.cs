///		Author:				
///		Created:			2005-05-21
///		Last Modified:		2011-11-10
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
    [Themeable(true)]
	public partial class ChildPageMenu : UserControl
	{
		private string cssClass = "txtnormal";
        private PageSettings currentPage;
        private SiteMapDataSource siteMapDataSource;
        private bool usePageImages = false;
        private bool hidePagesNotInSiteMap = false;
        private int maximumDynamicDisplayLevels = 20;
        private bool treatChildPageIndexAsSiteMap = false;
        private bool isAdmin = false;
        private bool isContentAdmin = false;
        private bool isSiteEditor = false;
        private bool forceDisplay = false;
        private bool isMobileSkin = false;
        private int mobileOnly = (int)ContentPublishMode.MobileOnly;
        private int webOnly = (int)ContentPublishMode.WebOnly;

        public int MaximumDynamicDisplayLevels
        {
            get { return maximumDynamicDisplayLevels; }
            set { maximumDynamicDisplayLevels = value; }
        }

        public bool UsePageImages
        {
            get { return usePageImages; }
            set { usePageImages = value; }
        }

        public bool HidePagesNotInSiteMap
        {
            get { return hidePagesNotInSiteMap; }
            set { hidePagesNotInSiteMap = value; }
        }


		public string CssClass
		{	
			get {return cssClass;}
			set {cssClass = value;}
		}

       
        public bool ForceDisplay
        {
            get { return forceDisplay; }
            set { forceDisplay = value; }
        }

        private int maxRenderDepth = -1; // no limit

        public int MaxRenderDepth
        {
            get { return maxRenderDepth; }
            set { maxRenderDepth = value; }
        }

        private bool honorSiteMapExpandSettings = false;

        public bool HonorSiteMapExpandSettings
        {
            get { return honorSiteMapExpandSettings; }
            set { honorSiteMapExpandSettings = value; }
        }
        

		protected void Page_Load(object sender, System.EventArgs e)
		{
            currentPage = CacheHelper.GetCurrentPage();
            treatChildPageIndexAsSiteMap = WebConfigSettings.TreatChildPageIndexAsSiteMap;

            EnableViewState = false;

            isAdmin = WebUser.IsAdmin;
            if (!isAdmin) { isContentAdmin = WebUser.IsContentAdmin; }
            if ((!isAdmin) && (!isContentAdmin)) { isSiteEditor = SiteUtils.UserIsSiteEditor(); }

            isMobileSkin = SiteUtils.UseMobileSkin();

            if (WebConfigSettings.UsePageImagesInSiteMap && treatChildPageIndexAsSiteMap)
            {
                usePageImages = true;
            }
            

            if (
                (currentPage != null)
                && (((currentPage.ShowChildPageMenu) && (Page is CmsPage)) || forceDisplay)
                
                )
            {
                // moved and commented out 2007-08-07
                //PreviousImplementation();
                Visible = true;
                ShowChildPageMap();


            }
            else
            {
                this.Visible = false;
            }

		}

        private void ShowChildPageMap()
        {
            if (!Visible) { return; }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) return;

            siteMapDataSource
                = (SiteMapDataSource)this.Page.Master.FindControl("ChildPageSiteMapData");

            if (siteMapDataSource == null) return;

            siteMapDataSource.SiteMapProvider
                    = "mojosite" 
                    + siteSettings.SiteId.ToInvariantString();

            

            if (WebConfigSettings.DisableViewStateOnSiteMapDataSource)
            {
                siteMapDataSource.EnableViewState = false;
            }

            //SiteMapNode node
            //    = siteMapDataSource.Provider.FindSiteMapNode(Request.RawUrl);
            //if (node != null)
            //{
            //    siteMapDataSource.StartingNodeUrl = Request.RawUrl;
            //}

            SiteMapNode node
                = siteMapDataSource.Provider.FindSiteMapNode(currentPage.Url);
            if (node != null)
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

#if NET35

            SiteMap1.MenuItemDataBound += new MenuEventHandler(SiteMap_MenuItemDataBound);
            SiteMap1.Orientation = Orientation.Vertical;

            SiteMap1.PathSeparator = '|';
            SiteMap1.MaximumDynamicDisplayLevels = maximumDynamicDisplayLevels;

            SiteMap1.DataSourceID = siteMapDataSource.ID;
            SiteMap1.DataBind();

#else
            SiteMap1.Visible = false;
            SiteMap2.Visible = true;

            //SiteMap2.SkinID = menuSkinID;
            SiteMap2.PathSeparator = '|';
           
            SiteMap2.CollapseImageToolTip = Resource.TreeMenuCollapseTooltip;
            SiteMap2.ExpandImageToolTip = Resource.TreeMenuExpandTooltip;

            SiteMap2.TreeNodeDataBound += new TreeNodeEventHandler(SiteMap2_TreeNodeDataBound);
            SiteMap2.DataSourceID = siteMapDataSource.ID;
            SiteMap2.DataBind();

#endif

            

           


        }

        void SiteMap2_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;

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
                if ((!isAdmin) && (!isContentAdmin) && (!isSiteEditor)) { remove = true; }
            }
            else
            {
                if ((!isAdmin) && (mapNode.Roles.Count == 1) && (mapNode.Roles[0].ToString() == "Admins")) { remove = true; }

                if ((!isAdmin) && (!isContentAdmin) && (!isSiteEditor) && (!WebUser.IsInRoles(mapNode.Roles))) { remove = true; }
            }

            //if (!mapNode.IncludeInMenu) remove = true;
            if (!mapNode.IncludeInChildSiteMap) remove = true;
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

            if (maxRenderDepth > -1)
            {
                if (e.Node.Depth > maxRenderDepth) { remove = true; }
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
                if (honorSiteMapExpandSettings && menu.ShowExpandCollapse)
                {
                    e.Node.Expanded = mapNode.ExpandOnSiteMap;
                }
            }
        }

        void SiteMap_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            Menu menu = (Menu)sender;
            mojoSiteMapNode mapNode = (mojoSiteMapNode)e.Item.DataItem;
            if ((usePageImages) && (mapNode.MenuImage.Length > 0))
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

            if ((treatChildPageIndexAsSiteMap)||(hidePagesNotInSiteMap))
            {
                if (!mapNode.IncludeInSiteMap) { remove = true; }
            }
            else
            {
                if (!mapNode.IncludeInMenu) { remove = true; }
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


        //this is just to make it themeable
        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
       

		
	}
}
