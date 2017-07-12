// Author:          
// Created:         2013-01-13
// Last Modified:   2017-06-29

using System;
using System.Text;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// allows skin developer to create a menu in the layout.master (or anwhere else) with his own logic
    /// </summary>
    public class MenuList : List<mojoMenuItem>
    {
        private SiteMapDataSource siteMapDataSource;
        private bool isAdmin = false;
        private bool isContentAdmin = false;
        private bool isSiteEditor = false;
        private SiteSettings siteSettings;
        private string secureSiteRoot = string.Empty;
        private string insecureSiteRoot = string.Empty;
        private bool resolveFullUrlsForMenuItemProtocolDifferences = false;
        private bool isSecureRequest = false;
        private SiteMapNode rootNode = null;
        private SiteMapNode startingNode = null;
        private mojoSiteMapNode currentNode = null;
        public List<mojoMenuItem> Items = new List<mojoMenuItem>();

        public MenuList()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();

            isAdmin = WebUser.IsAdmin;
            if (!isAdmin) { isContentAdmin = WebUser.IsContentAdmin; }
            if ((!isAdmin) && (!isContentAdmin)) { isSiteEditor = SiteUtils.UserIsSiteEditor(); }

            resolveFullUrlsForMenuItemProtocolDifferences = WebConfigSettings.ResolveFullUrlsForMenuItemProtocolDifferences;
            if (resolveFullUrlsForMenuItemProtocolDifferences)
            {
                secureSiteRoot = WebUtils.GetSecureSiteRoot();
                insecureSiteRoot = WebUtils.GetInSecureSiteRoot();
            }

            isSecureRequest = SiteUtils.IsSecureRequest();

            siteMapDataSource = new SiteMapDataSource();
            siteMapDataSource.SiteMapProvider = "mojosite" + siteSettings.SiteId.ToInvariantString();

            rootNode = siteMapDataSource.Provider.RootNode;
            currentNode = SiteUtils.GetCurrentPageSiteMapNode(rootNode);
            startingNode = rootNode;

            //if (startingNodePageId > -1)
            //{
            //    startingNode = SiteUtils.GetSiteMapNodeForPage(rootNode, startingNodePageId);
            //}
            //else if (startingNodeOffset > -1)
            //{
            //    startingNode = SiteUtils.GetOffsetNode(currentNode, startingNodeOffset);
            //}
            //else if (isSubMenu)
            //{
            //    startingNode = SiteUtils.GetTopLevelParentNode(currentNode);
            //}

            Items = AddNodes(startingNode);



        }

        private List<mojoMenuItem> AddNodes(SiteMapNode startingNode)
        {
            List<mojoMenuItem> items = new List<mojoMenuItem>();
            foreach (SiteMapNode childNode in startingNode.ChildNodes)
            {
                mojoSiteMapNode mojoNode = childNode as mojoSiteMapNode;
                if (mojoNode == null) { continue; }

                if (!ShouldAdd(mojoNode)) { continue; }

                mojoMenuItem item = new mojoMenuItem
                {
                    PageId = mojoNode.PageId,
                    Name = mojoNode.Title,
                    Description = mojoNode.MenuDescription,
                    URL = mojoNode.Url,
                    CSSClass = mojoNode.MenuCssClass,
                    Rel = mojoNode.LinkRel,
                    Clickable = mojoNode.IsClickable,
                    OpenInNewTab = mojoNode.OpenInNewWindow,
                    PublishMode = mojoNode.PublishMode,
                    LastModDate = mojoNode.LastModifiedUtc,
                    Children = AddNodes(childNode)
                };

                //todo: figure out the current page and set current=true if we are

            }
            return items;
        }

        private bool ShouldAdd(mojoSiteMapNode mapNode)
        {
            if (mapNode == null) { return false; }

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


            if (!mapNode.IncludeInMenu) { remove = true; }

            if (mapNode.IsPending && !WebUser.IsAdminOrContentAdminOrContentPublisherOrContentAuthor) { remove = true; }

            if ((mapNode.HideAfterLogin) && (WebUser.IsInRole("Authenticated"))) { remove = true; }

            { return !remove; }

        }

    }
}