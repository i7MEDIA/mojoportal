// Author:				
// Created:			    2006-06-13
// Last Modified:		2013-12-17
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software. 

using System;
using System.Web;
using System.Collections;
using System.Collections.Specialized;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web
{
    /// <summary>
    /// Purpose: extends SiteMapNode with MenuImage property to enable images in the menu
    /// </summary>
    public class mojoSiteMapNode : SiteMapNode
    {
        private string menuImage = String.Empty;
        private string menuDescription = string.Empty;
        private bool isRootNode = false;
        private int depth = 0;
        private bool includeInMenu = false;
        private bool includeInSiteMap = true;
        private bool expandOnSiteMap = true;
        private bool includeInChildSiteMap = true;
        private bool includeInSearchMap = true;
        private Guid pageGuid = Guid.Empty;
        private int pageID = -1;
        private int parentID = -1;
        private string viewRoles = string.Empty;
        private string editRoles = string.Empty;
        private string draftEditRoles = string.Empty;
        private string createChildPageRoles = string.Empty;
        private string createChildDraftPageRoles = string.Empty;
        private string depthIndicator = string.Empty;
        private PageChangeFrequency changeFrequency = PageChangeFrequency.Daily;
        private string siteMapPriority = "0.5";
        private DateTime lastModifiedUTC = DateTime.MinValue;
        private bool openInNewWindow = false;
        private bool hideAfterLogin = false;
        private bool useSsl = false;
        private bool isPending = false;
        private bool isClickable = true;
        private string menuCssClass = string.Empty;
        private int publishMode = 0; //All
        private string linkRel = string.Empty;
        private DateTime pubDateUtc = DateTime.UtcNow;

        public DateTime PubDateUtc
        {
            get { return pubDateUtc; }
            set { pubDateUtc = value; }
        }

        public string LinkRel
        {
            get { return linkRel; }
            set { linkRel = value; }
        }


        public string MenuDescription
        {
            get { return menuDescription; }
            set { menuDescription = value; }
        }

        public PageChangeFrequency ChangeFrequency
        {
            get { return changeFrequency; }
            set { changeFrequency = value; }
        }

        public string SiteMapPriority
        {
            get { return siteMapPriority; }
            set { siteMapPriority = value; }
        }

        public DateTime LastModifiedUtc
        {
            get { return lastModifiedUTC; }
            set { lastModifiedUTC = value; }
        }

        public string DepthIndicator
        {
            get { return depthIndicator; }
            set { depthIndicator = value; }
        }

        public string ViewRoles
        {
            get { return viewRoles; }
            set { viewRoles = value; }
        }

        public string EditRoles
        {
            get { return editRoles; }
            set { editRoles = value; }
        }

        public string DraftEditRoles
        {
            get { return draftEditRoles; }
            set { draftEditRoles = value; }
        }

        public string CreateChildPageRoles
        {
            get { return createChildPageRoles; }
            set { createChildPageRoles = value; }
        }

        public string CreateChildDraftPageRoles
        {
            get { return createChildDraftPageRoles; }
            set { createChildDraftPageRoles = value; }
        }

        public int PageId
        {
            get { return pageID; }
            set { pageID = value; }
        }

        public int ParentId
        {
            get { return parentID; }
            set { parentID = value; }
        }

        public Guid PageGuid
        {
            get { return pageGuid; }
            set { pageGuid = value; }
        }

        public int PublishMode
        {
            get { return publishMode; }
            set { publishMode = value; }
        }

        public bool IsRootNode
        {
            get { return isRootNode; }
            set { isRootNode = value; }
        }

        public bool IsClickable
        {
            get { return isClickable; }
            set { isClickable = value; }
        }

        public bool IncludeInMenu
        {
            get { return includeInMenu; }
            set { includeInMenu = value; }
        }

        public bool IncludeInSiteMap
        {
            get { return includeInSiteMap; }
            set { includeInSiteMap = value; }
        }

        public bool ExpandOnSiteMap
        {
            get { return expandOnSiteMap; }
            set { expandOnSiteMap = value; }
        }

        

        public bool IncludeInChildSiteMap
        {
            get { return includeInChildSiteMap; }
            set { includeInChildSiteMap = value; }
        }

        public bool IncludeInSearchMap
        {
            get { return includeInSearchMap; }
            set { includeInSearchMap = value; }
        }

        public bool HideAfterLogin
        {
            get { return hideAfterLogin; }
            set { hideAfterLogin = value; }
        }

        public int Depth
        {
            get { return depth; }
            set { depth = value; }
        }

        

        public string MenuImage
        {
            get { return menuImage; }
            set { menuImage = value; }
        }

        public string MenuCssClass
        {
            get { return menuCssClass; }
            set { menuCssClass = value; }
        }

        

        public bool OpenInNewWindow
        {
            get { return openInNewWindow; }
            set { openInNewWindow = value; }
        }

        public bool UseSsl
        {
            get { return useSsl; }
            set { useSsl = value; }
        }

        public bool IsPending
        {
            get { return isPending; }
            set { isPending = value; }
        }

        public bool HasVisibleChildren()
        {
            if (this.ChildNodes == null) { return false; }
            if (this.ChildNodes.Count == 0) { return false; }
            foreach (SiteMapNode child in ChildNodes)
            {
                if (child is mojoSiteMapNode)
                {
                    mojoSiteMapNode mchild = child as mojoSiteMapNode;
                    if ((mchild.IncludeInMenu)&& (WebUser.IsInRoles(mchild.ViewRoles))){ return true; }
                }

            }

            return false;

        }
        

        public mojoSiteMapNode(
            SiteMapProvider provider,
            string key,
            string url,
            string title,
            string description,
            IList roles,
            NameValueCollection attributes,
            NameValueCollection explicitResourceKeys,
            string implicitResourceKey):base( provider, key,
                url,
                title,
                description,
                roles,
                attributes,
                explicitResourceKeys,
                implicitResourceKey)
        {

        }


        public mojoSiteMapNode(
            SiteMapProvider provider,
            string key,
            string url,
            string title,
            string description):base(provider,key,url,title,description)
        {

        }

        public mojoSiteMapNode(
            SiteMapProvider provider,
            string key,
            string url,
            string title)
            : base(provider, key, url, title)
        {

        }

        public mojoSiteMapNode(
            SiteMapProvider provider,
            string key,
            string url)
            : base(provider, key, url)
        {
            
        }

        public mojoSiteMapNode(SiteMapProvider provider, string key)
            : base(provider, key)
        {

        }
        

        

    }
}
