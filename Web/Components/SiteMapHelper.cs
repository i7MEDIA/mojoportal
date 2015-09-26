

using System;
using System.Collections.Generic;
//using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

using System.Web;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web
{
    /// <summary>
    /// looking for a way to make the arent page selector more scalable, currently a dropdown of all pages filtered by user membership in createChildPageRoles
    /// 
    /// an idea:
    /// use the parent page id if passed in and th euser is in the role
    /// combo box or something where the user can type in the page they are looking for and we bring back the first few matching nodes with role permissions
    /// if nothing is entered bring back the first node that the user does have permissions on
    /// 
    /// will be tricky/expensive because we have to cast as mojoSiteMapNode
    /// 
    /// </summary>
    public class SiteMapHelper
    {
        //http://msdn.microsoft.com/en-us/library/system.web.sitemapnodecollection.aspx

        //http://stackoverflow.com/questions/1960364/asp-net-enumerate-through-sitemapnode-childnodes

        //foreach (var childNode in node.ChildNodes.OrderBy(x => x.Key))

        //public static IEnumerable<SiteMapNode> OrderBy(this SiteMapNodeCollection smnc, Func<SiteMapNode, TKey> expression)
        //{
        //    return smnc.Cast<SiteMapNode>().OrderBy(expression);
        //}

        //http://stackoverflow.com/questions/703130/linq-query-loses-order

        public static SiteMapNode FindNodeAllowedForParentPage(SiteMapNode rootNode, string pageName)
        {
            //SiteMapNode node = rootNode.GetAllNodes().Cast().FirstOrDefault(n => n.Key.Equals(pagekey));
            //return node != null ? node.Url : String.Empty;

            SiteMapNode foundNode = (
                    from SiteMapNode cr in rootNode.GetAllNodes()
                    where cr.Title.StartsWith(pageName)
                        orderby cr.Title
                        select cr
                        ).First();

            mojoSiteMapNode mojoNode = foundNode as mojoSiteMapNode;
            if (mojoNode != null)
            {
                if ((WebUser.IsInRoles(mojoNode.CreateChildPageRoles))||(WebUser.IsInRoles(mojoNode.CreateChildDraftPageRoles))) { return foundNode; }
            }


            return null;

        }

        //public static mojoSiteMapNode GetSiteMapNodeForPage(SiteMapNode rootNode, PageSettings pageSettings)
        //{
        //    if (rootNode == null) { return null; }
        //    if (pageSettings == null) { return null; }
        //    if (!(rootNode is mojoSiteMapNode)) { return null; }

        //    foreach (SiteMapNode childNode in rootNode.ChildNodes)
        //    {
        //        if (!(childNode is mojoSiteMapNode)) { return null; }

        //        mojoSiteMapNode node = childNode as mojoSiteMapNode;
        //        if (node.PageId == pageSettings.PageId) { return node; }

        //        mojoSiteMapNode foundNode = GetSiteMapNodeForPage(node, pageSettings);
        //        if (foundNode != null) { return foundNode; }


        //    }

        //    return null;

        //}

        public static mojoSiteMapNode GetSiteMapNodeForPage(SiteMapNode rootNode, string currentUrl)
        {
            if (rootNode == null) { return null; }
            if (string.IsNullOrEmpty(currentUrl)) { return null; }
            if (!(rootNode is mojoSiteMapNode)) { return null; }

            foreach (SiteMapNode childNode in rootNode.ChildNodes)
            {
                if (!(childNode is mojoSiteMapNode)) { return null; }

                mojoSiteMapNode node = childNode as mojoSiteMapNode;
                if (string.Equals(node.Url.Replace("~", string.Empty), currentUrl, StringComparison.InvariantCultureIgnoreCase)) { return node; }

                mojoSiteMapNode foundNode = GetSiteMapNodeForPage(node, currentUrl);
                if (foundNode != null) { return foundNode; }


            }

            return null;

        }

        /// <summary>
        /// this is a rather heavy method and should only be used judiciously becuase it must 
        /// recurse through the entire site map to determin if the user has any permission to create pages
        /// </summary>
        /// <returns></returns>
        public static bool UserHasAnyCreatePagePermissions(SiteSettings siteSettings)
        {
            if (siteSettings == null) { return false; }

            if (WebUser.IsInRoles(siteSettings.RolesThatCanCreateRootPages)) { return true; }

            SiteMapDataSource siteMapDataSource = new SiteMapDataSource();

            siteMapDataSource.SiteMapProvider = "mojosite" + siteSettings.SiteId.ToInvariantString();

            SiteMapNode siteMapNode = siteMapDataSource.Provider.RootNode;


            return CanCreatePages(siteMapNode);
        }

        

        private static bool CanCreatePages(SiteMapNode siteMapNode)
        {
            mojoSiteMapNode mojoNode = (mojoSiteMapNode)siteMapNode;

            if (!mojoNode.IsRootNode)
            {
                if (WebUser.IsInRoles(mojoNode.CreateChildPageRoles))
                {
                    return true;
                }
            }


            foreach (SiteMapNode childNode in mojoNode.ChildNodes)
            {
                //recurse to populate children
                if (CanCreatePages(childNode)) { return true; }

            }

            return false;
        }

        public static mojoSiteMapNode GetCurrentPageSiteMapNode(SiteMapNode rootNode)
        {
            if (rootNode == null) { return null; }
            PageSettings currentPage = CacheHelper.GetCurrentPage();
            if (currentPage == null) { return null; }

            return GetSiteMapNodeForPage(rootNode, currentPage);

        }

        public static mojoSiteMapNode GetSiteMapNodeForPage(SiteMapNode rootNode, PageSettings pageSettings)
        {
            if (rootNode == null) { return null; }
            if (pageSettings == null) { return null; }
            if (!(rootNode is mojoSiteMapNode)) { return null; }

            foreach (SiteMapNode childNode in rootNode.ChildNodes)
            {
                if (!(childNode is mojoSiteMapNode)) { return null; }

                mojoSiteMapNode node = childNode as mojoSiteMapNode;
                if (node.PageId == pageSettings.PageId) { return node; }

                mojoSiteMapNode foundNode = GetSiteMapNodeForPage(node, pageSettings);
                if (foundNode != null) { return foundNode; }


            }

            return null;

        }
        
    }
}