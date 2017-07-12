/// Author:				    
/// Created:			    2005-01-01
/// Last Modified:		    2012-10-09
/// 2009-02-27 applied patch from Damien White to support a Home crumb
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using Resources;
using System;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;


namespace mojoPortal.Web.UI
{

    public partial class BreadcrumbsControl : UserControl
    {
        private SiteSettings siteSettings = null;
        private PageSettings currentPage;
        protected string siteRoot = String.Empty;
        private string separator = " > ";
        private bool showhome = false;
        private bool isAdmin = false;
        private bool isContentAdmin = false;
        private bool isSiteEditor = false;
        private string wrapperCssClass = "breadcrumbs";
        private string cssClass = "unselectedcrumb";
        private string currentPageCssClass = "selectedcrumb";
        private bool forceShowBreadcrumbs = false;
        private bool forceShowChildPageBreadCrumbs = false;
        private bool usePageCrumbOnly = false;
        private bool useTopParentCrumbOnly = false;
        private bool suppresIfCurrentPageIsParent = false;
        private string addedCrumbs = string.Empty;
        private SiteMapDataSource siteMapDataSource;
        private SiteMapNode currentPageNode = null;
        private StringBuilder markup = null;
        protected string rootLinkText = Resource.HomePageLink;
        private bool enableUnclickableLinks = false;

        #region Public Properties

        public string WrapperCssClass
        {
            get { return wrapperCssClass; }
            set { wrapperCssClass = value; }
        }

        public bool ForceShowBreadcrumbs
        {
            get { return forceShowBreadcrumbs; }
            set { forceShowBreadcrumbs = value; }
        }

        public bool UsePageCrumbOnly
        {
            get { return usePageCrumbOnly; }
            set { usePageCrumbOnly = value; }
        }

        public bool UseTopParentCrumbOnly
        {
            get { return useTopParentCrumbOnly; }
            set { useTopParentCrumbOnly = value; }
        }

        public bool SuppresIfCurrentPageIsParent
        {
            get { return suppresIfCurrentPageIsParent; }
            set { suppresIfCurrentPageIsParent = value; }
        }

        public bool ForceShowChildPageBreadCrumbs
        {
            get { return forceShowChildPageBreadCrumbs; }
            set { forceShowChildPageBreadCrumbs = value; }
        }

        public string AddedCrumbs
        {
            get { return addedCrumbs; }
            set { addedCrumbs = value; }
        }

        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }

        public string CurrentPageCssClass
        {
            get { return currentPageCssClass; }
            set { currentPageCssClass = value; }
        }

        public string Separator
        {
            get { return Server.HtmlEncode(separator); }
            set { separator = value; }
        }

        public bool ShowHome
        {
            get { return showhome; }
            set { showhome = value; }
        }

        private string itemWrapperTop = string.Empty;

        public string ItemWrapperTop
        {
            get { return itemWrapperTop; }
            set { itemWrapperTop = value; }
        }

        private string itemWrapperBottom = string.Empty;

        public string ItemWrapperBottom
        {
            get { return itemWrapperBottom; }
            set { itemWrapperBottom = value; }
        }

        #endregion

        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current == null) return;

            EnableViewState = false;

            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) return;

            currentPage = CacheHelper.GetCurrentPage();
            if (currentPage == null) { return; }

            siteMapDataSource = (SiteMapDataSource)this.Page.Master.FindControl("SiteMapData");
            if (siteMapDataSource == null) return;

            siteMapDataSource.SiteMapProvider
                = "mojosite" + siteSettings.SiteId.ToInvariantString();

            if (WebConfigSettings.DisableViewStateOnSiteMapDataSource)
            {
                siteMapDataSource.EnableViewState = false;
            }

            isAdmin = WebUser.IsAdmin;
            if (!isAdmin) { isContentAdmin = WebUser.IsContentAdmin; }
            if ((!isAdmin) && (!isContentAdmin)) { isSiteEditor = SiteUtils.UserIsSiteEditor(); }

            //siteRoot = siteSettings.SiteRoot;
            siteRoot = SiteUtils.GetNavigationSiteRoot();

            if (Page is mojoBasePage)
            {
                mojoBasePage basePage = Page as mojoBasePage;
                enableUnclickableLinks = basePage.StyleCombiner.EnableNonClickablePageLinks;
            }
            
         
            DoRendering();
        }

        private void DoRendering()
        {
            if (
                (!forceShowBreadcrumbs)
                && (!currentPage.ShowBreadcrumbs)
                && (!forceShowChildPageBreadCrumbs)
                && (!currentPage.ShowChildPageBreadcrumbs)
                )
            {
                this.Visible = false;
                return;
            }

            pnlWrapper.CssClass = wrapperCssClass;

            if (WebConfigSettings.UseSiteNameForRootBreadcrumb)
            {
                rootLinkText = siteSettings.SiteName;
            }

            if ((forceShowBreadcrumbs) || (currentPage.ShowBreadcrumbs))
            {
                breadCrumbsControl.Visible = true;
                BindCrumbs();
            }
            
            if (forceShowChildPageBreadCrumbs
                || (currentPage.ShowChildPageBreadcrumbs)
                )
            {
                RenderChildPageBreadcrumbs();

            }

            if (forceShowBreadcrumbs
                || (currentPage.ShowBreadcrumbs)
                || (currentPage.ShowChildPageBreadcrumbs)
                )
            {
                if (addedCrumbs.Length > 0)
                {
                    if (markup == null) { markup = new StringBuilder(); }

                    AddSeparator(markup);
                    markup.Append(addedCrumbs);

                }

                if (markup != null) { childCrumbs.Text = markup.ToString(); }
            }

            

        }

        private void BindCrumbs()
        {
            if (siteMapDataSource == null){ return; }

            //breadCrumbsControl.ParentLevelsDisplayed = -1;

            if ((!showhome)&&(!currentPage.ShowHomeCrumb))
            {
                int currentPageDepth = SiteUtils.GetCurrentPageDepth(siteMapDataSource.Provider.RootNode);
                breadCrumbsControl.ParentLevelsDisplayed = currentPageDepth;
            }


            currentPageNode = SiteUtils.GetCurrentPageSiteMapNode(siteMapDataSource.Provider.RootNode);
            breadCrumbsControl.OverrideCurrentNode = currentPageNode;
            breadCrumbsControl.PathSeparator = Separator;

            breadCrumbsControl.Provider = siteMapDataSource.Provider;
            breadCrumbsControl.DataBind();

        }

        void breadCrumbsControl_ItemDataBound(object sender, SiteMapNodeItemEventArgs e)
        {
            if (enableUnclickableLinks)
            {
                mojoSiteMapNode mapNode = (mojoSiteMapNode)e.Item.SiteMapNode;
                if((mapNode != null)&&(!mapNode.IsClickable))
                {
                    e.Item.Enabled = false;    
                }
            }

        }

        

        

        private void RenderChildPageBreadcrumbs()
        {
            if (HttpContext.Current == null) return;

            if (currentPageNode == null) { currentPageNode = SiteUtils.GetCurrentPageSiteMapNode(siteMapDataSource.Provider.RootNode); }
            if (currentPageNode == null) { return; }

            markup = new StringBuilder();
            markup.Append(Separator);
            string childSeparator = string.Empty;

            int addedChildren = 0;

            foreach (SiteMapNode childNode in currentPageNode.ChildNodes)
            {
                if (!(childNode is mojoSiteMapNode)) { continue; }

                mojoSiteMapNode node = childNode as mojoSiteMapNode;

                if (!node.IncludeInMenu) { continue; }

                bool remove = false;

                if (node.Roles == null)
                {
                    if ((!isAdmin) && (!isContentAdmin) && (!isSiteEditor)) { remove = true; }
                }
                else
                {
                    if ((!isAdmin) && (node.Roles.Count == 1) && (node.Roles[0].ToString() == "Admins")) { remove = true; }

                    if ((!isAdmin) && (!isContentAdmin) && (!isSiteEditor) && (!WebUser.IsInRoles(node.Roles))) { remove = true; }
                }

                if (!remove)
                {
                    markup.Append(childSeparator + "<a class='"
                                         + this.cssClass + "' href='"
                                         + Page.ResolveUrl(node.Url)
                                         + "'>"
                                         + node.Title + "</a>");

                    childSeparator = " - ";

                    addedChildren += 1;
                }

            }

            // this gets rid of the initial separator between bread crumbs and child crumbs if no children were rendered
            if (addedChildren == 0) { markup = null; }

            
           
        }

        private void AddSeparator(StringBuilder markup)
        {
            if (markup != null)
            {
                markup.Append(Separator);
            }

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);

            breadCrumbsControl.ItemDataBound += new SiteMapNodeItemEventHandler(breadCrumbsControl_ItemDataBound);
            
        }


        //void breadCrumbsControl_ItemDataBound(object sender, SiteMapNodeItemEventArgs e)
        //{
        //    if (sender == null) return;
        //    if (e == null) return;

        //    Menu menu = (Menu)sender;
        //    mojoSiteMapNode mapNode = (mojoSiteMapNode)e.Item.SiteMapNode;

        //    bool remove = false;

        //    if (!(
        //            (isAdmin)
        //            || (
        //                (isContentAdmin)
        //                && (mapNode.Roles != null)
        //                && (!(mapNode.Roles.Count == 1)
        //                && (mapNode.Roles[0].ToString() == "Admins")
        //                   )
        //                )
        //            || ((isContentAdmin) && (mapNode.Roles == null))
        //            || (
        //                (mapNode.Roles != null)
        //                && (WebUser.IsInRoles(mapNode.Roles))
        //                )
        //        ))
        //    {
        //        remove = true;
        //    }

        //    if (!mapNode.IncludeInMenu) remove = true;
        //    if (mapNode.IsPending && !WebUser.IsAdminOrContentAdminOrContentPublisherOrContentAuthor) remove = true;
        //    if ((mapNode.HideAfterLogin) && (Request.IsAuthenticated)) remove = true;

        //    if (remove)
        //    {
        //        //if (e.Item.Depth == 0)
        //        //{
        //        //    breadCrumbsControl.Items.Remove(e.Item);

        //        //}
        //        //else
        //        //{
        //        //    MenuItem parent = e.Item.;
        //        //    if (parent != null)
        //        //    {
        //        //        parent.ChildItems.Remove(e.Item);
        //        //    }
        //        //}
        //    }


        //}
       

        

    }
}
