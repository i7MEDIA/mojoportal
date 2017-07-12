// Author:					
// Created:				    2007-08-07
// Last Modified:			2011-03-24
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 2011-03-03 implemented better support for page hierarchy in initial content by implementing a childPages node in the xml and a ChildPagesCollection

using System;
using System.Collections.ObjectModel;
using System.Web;
using System.Xml;
using log4net;

namespace mojoPortal.Web
{
    /// <summary>
    ///
    /// </summary>
    public class ContentPage
    {
        private ContentPage()
        { }

        //private static readonly ILog log = LogManager.GetLogger(typeof(ContentPage));

        private string resourceFile = string.Empty;
        private string name = string.Empty;
        private string title = string.Empty;
        private string url = string.Empty;
        private string menuImage = string.Empty;
        private int pageOrder = 1;
        private bool requireSSL = false;
        private bool showBreadcrumbs = false;
        private string visibleToRoles = "All Users;";
        private string editRoles = string.Empty;
        private string draftEditRoles = string.Empty;
        private string createChildPageRoles = string.Empty;
        private string pageMetaKeyWords = string.Empty;
        private string pageMetaDescription = string.Empty;

       
        private string bodyCssClass = string.Empty;
        private string menuCssClass = string.Empty;
        private bool includeInMenu = true;
        private bool isClickable = true;
        private bool includeInSiteMap = true;
        private bool includeInChildPagesSiteMap = true;
        private bool allowBrowserCaching = true;
        private bool showChildPageBreadcrumbs = false;
        private bool showHomeCrumb = false;
        private bool showChildPagesSiteMap = false;
        private bool hideFromAuthenticated = false;
        private bool enableComments = false;


        private Collection<ContentPageItem> pageItems = new Collection<ContentPageItem>();

        private Collection<ContentPage> childPages = new Collection<ContentPage>();

        public string ResourceFile
        {
            get { return resourceFile; }
        }

        public string Name
        {
            get { return name; }
        }

        public string Title
        {
            get { return title; }
        }
        public string Url
        {
            get { return url; }
        }

        public int PageOrder
        {
            get { return pageOrder; }
        }

        public string MenuImage
        {
            get { return menuImage; }
        }

        public bool RequireSsl
        {
            get { return requireSSL; }
        }

        public bool ShowBreadcrumbs
        {
            get { return showBreadcrumbs; }
        }

        public string VisibleToRoles
        {
            get { return visibleToRoles; }
        }

        public string EditRoles
        {
            get { return editRoles; }
        }

        public string DraftEditRoles
        {
            get { return draftEditRoles; }
        }

        public string CreateChildPageRoles
        {
            get { return createChildPageRoles; }
        }

        public string PageMetaKeyWords
        {
            get { return pageMetaKeyWords; }
        }

        public string PageMetaDescription
        {
            get { return pageMetaDescription; }
        }

        public string BodyCssClass
        {
            get { return bodyCssClass; }
        }

        public string MenuCssClass
        {
            get { return menuCssClass; }
        }

        public bool IncludeInMenu
        {
            get { return includeInMenu; }
        }

        public bool IsClickable
        {
            get { return isClickable; }
        }

        public bool IncludeInSiteMap
        {
            get { return includeInSiteMap; }
        }

        public bool IncludeInChildPagesSiteMap
        {
            get { return includeInChildPagesSiteMap; }
        }

        public bool AllowBrowserCaching
        {
            get { return allowBrowserCaching; }
        }

        public bool ShowChildPageBreadcrumbs
        {
            get { return showChildPageBreadcrumbs; }
        }

        public bool ShowHomeCrumb
        {
            get { return showHomeCrumb; }
        }

        public bool ShowChildPagesSiteMap
        {
            get { return showChildPagesSiteMap; }
        }

        public bool HideFromAuthenticated
        {
            get { return hideFromAuthenticated; }
        }

        public bool EnableComments
        {
            get { return enableComments; }
        }

        public Collection<ContentPage> ChildPages
        {
            get
            {
                return childPages;
            }
        }

        public Collection<ContentPageItem> PageItems
        {
            get
            {
                return pageItems;
            }
        }


        public static void LoadPages(
            ContentPageConfiguration contentPageConfig,
            XmlNode documentElement)
        {
            if (HttpContext.Current == null) return;
            if (documentElement.Name != "siteContent") return;

            XmlNode pagesNode = null;

            foreach (XmlNode node in documentElement.ChildNodes)
            {
                if (node.Name == "pages")
                {
                    pagesNode = node;
                    break;
                }
            }

            if(pagesNode == null) return;

            foreach (XmlNode node in pagesNode.ChildNodes)
            {
                if (node.Name == "page")
                {
                    LoadPage(contentPageConfig, node, null);
                }

            }


        }

        public static void LoadPage(
            ContentPageConfiguration contentPageConfig,
            XmlNode node, 
            ContentPage parentPage)
        {
            ContentPage contentPage = new ContentPage();

            XmlAttributeCollection attributeCollection = node.Attributes;

            if (attributeCollection["resourceFile"] != null)
            {
                contentPage.resourceFile = attributeCollection["resourceFile"].Value;
            }

            if (attributeCollection["name"] != null)
            {
                contentPage.name = attributeCollection["name"].Value;
            }


            if (attributeCollection["title"] != null)
            {
                contentPage.title = attributeCollection["title"].Value;
            }


            if (attributeCollection["url"] != null)
            {
                contentPage.url = attributeCollection["url"].Value;
            }

            if (attributeCollection["menuImage"] != null)
            {
                contentPage.menuImage = attributeCollection["menuImage"].Value;
            }

            if (attributeCollection["pageOrder"] != null)
            {
                int sort = 1;
                if (int.TryParse(attributeCollection["pageOrder"].Value,
                    out sort))
                {
                    contentPage.pageOrder = sort;
                }
            }

            if (attributeCollection["visibleToRoles"] != null)
            {
                contentPage.visibleToRoles = attributeCollection["visibleToRoles"].Value;
            }

            if (attributeCollection["editRoles"] != null)
            {
                contentPage.editRoles = attributeCollection["editRoles"].Value;
            }

            if (attributeCollection["draftEditRoles"] != null)
            {
                contentPage.draftEditRoles = attributeCollection["draftEditRoles"].Value;
            }

            if (attributeCollection["createChildPageRoles"] != null)
            {
                contentPage.createChildPageRoles = attributeCollection["createChildPageRoles"].Value;
            }

            if (attributeCollection["pageMetaKeyWords"] != null)
            {
                contentPage.pageMetaKeyWords = attributeCollection["pageMetaKeyWords"].Value;
            }

            if (attributeCollection["pageMetaDescription"] != null)
            {
                contentPage.pageMetaDescription = attributeCollection["pageMetaDescription"].Value;
            }

            if (
                (attributeCollection["requireSSL"] != null)
                && (attributeCollection["requireSSL"].Value.ToLower() == "true")
                )
            {
                contentPage.requireSSL = true;
            }

            if (
                (attributeCollection["showBreadcrumbs"] != null)
                && (attributeCollection["showBreadcrumbs"].Value.ToLower() == "true")
                )
            {
                contentPage.showBreadcrumbs = true;
            }

             if (
                (attributeCollection["includeInMenu"] != null)
                && (attributeCollection["includeInMenu"].Value.ToLower() == "false")
                )
            {
                contentPage.includeInMenu = false;
            }

            if (
                (attributeCollection["isClickable"] != null)
                && (attributeCollection["isClickable"].Value.ToLower() == "false")
                )
            {
                contentPage.isClickable = false;
            }

             if (
                (attributeCollection["includeInSiteMap"] != null)
                && (attributeCollection["includeInSiteMap"].Value.ToLower() == "false")
                )
            {
                contentPage.includeInSiteMap = false;
            }

            if (
                (attributeCollection["includeInChildPagesSiteMap"] != null)
                && (attributeCollection["includeInChildPagesSiteMap"].Value.ToLower() == "false")
                )
            {
                contentPage.includeInChildPagesSiteMap = false;
            }

            if (
                (attributeCollection["allowBrowserCaching"] != null)
                && (attributeCollection["allowBrowserCaching"].Value.ToLower() == "false")
                )
            {
                contentPage.allowBrowserCaching = false;
            }

            if (
                (attributeCollection["showChildPageBreadcrumbs"] != null)
                && (attributeCollection["showChildPageBreadcrumbs"].Value.ToLower() == "true")
                )
            {
                contentPage.showChildPageBreadcrumbs = true;
            }

            if (
                (attributeCollection["showHomeCrumb"] != null)
                && (attributeCollection["showHomeCrumb"].Value.ToLower() == "true")
                )
            {
                contentPage.showHomeCrumb = true;
            }

            if (
                (attributeCollection["showChildPagesSiteMap"] != null)
                && (attributeCollection["showChildPagesSiteMap"].Value.ToLower() == "true")
                )
            {
                contentPage.showChildPagesSiteMap = true;
            }

            if (
                (attributeCollection["hideFromAuthenticated"] != null)
                && (attributeCollection["hideFromAuthenticated"].Value.ToLower() == "true")
                )
            {
                contentPage.hideFromAuthenticated = true;
            }

            if (
                (attributeCollection["enableComments"] != null)
                && (attributeCollection["enableComments"].Value.ToLower() == "true")
                )
            {
                contentPage.enableComments = true;
            }

            if (attributeCollection["bodyCssClass"] != null)
            {
                contentPage.bodyCssClass = attributeCollection["bodyCssClass"].Value;
            }

            if (attributeCollection["menuCssClass"] != null)
            {
                contentPage.menuCssClass = attributeCollection["menuCssClass"].Value;
            }


            foreach (XmlNode contentFeatureNode in node.ChildNodes)
            {
                if (contentFeatureNode.Name == "contentFeature")
                {
                    ContentPageItem.LoadPageItem(
                        contentPage,
                        contentFeatureNode);
                }
            }

            XmlNode childPagesNode = null;

            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name == "childPages")
                {
                    childPagesNode = n;
                    break;
                }
            }

            if (parentPage == null)
            {
                contentPageConfig.ContentPages.Add(contentPage);
            }
            else
            {
                parentPage.ChildPages.Add(contentPage);
            }

            if (childPagesNode != null)
            {
                foreach (XmlNode c in childPagesNode.ChildNodes)
                {
                    if (c.Name == "page")
                    {
                        LoadPage(contentPageConfig, c, contentPage);
                    }
                }
            }

            

        }

    }
}
