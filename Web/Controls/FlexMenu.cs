// Author:          Joe Audette
// Created:         2013-01-13
// Last Modified:   2014-12-17
// Copyright Source Tree Solutions, LLC

using System;
using System.Text;
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
    /// an alternative to <asp:Menu and <asp:Treeview with no built in javascript
    /// the only strength Treeview has over this menu is the support for expand and collapse like on a site map
    /// </summary>
    public class FlexMenu : WebControl
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

        private int startingNodePageId = -1;
        /// <summary>
        /// if specified then the menu will show the child pages of the page corresponding to the specific page id
        /// it must be a page within the current site, ie it must exist in the site map data for the current site
        /// </summary>
        public int StartingNodePageId
        {
            get { return startingNodePageId; }
            set { startingNodePageId = value; }
        }

        private int startingNodeOffset = -1;

        public int StartingNodeOffset
        {
            get { return startingNodeOffset; }
            set { startingNodeOffset = value; }
        }

        private int maxDataRenderDepth = -1; // no limit

        public int MaxDataRenderDepth
        {
            get { return maxDataRenderDepth; }
            set { maxDataRenderDepth = value; }
        }

        //private bool isMobileSkin = false;
        private int mobileOnly = (int)ContentPublishMode.MobileOnly;
        private int webOnly = (int)ContentPublishMode.WebOnly;

        private string containerElement = string.Empty;

        public string ContainerElement
        {
            get { return containerElement; }
            set { containerElement = value; }
        }

        private string containerCssClass = string.Empty;

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

        private bool renderDescription = true;

        public bool RenderDescription
        {
            get { return renderDescription; }
            set { renderDescription = value; }
        }

        private string descriptionCssClass = string.Empty;
        public string DescriptionCssClass
        {
            get { return descriptionCssClass; }
            set { descriptionCssClass = value; }
        }

        private string childContainerElement = string.Empty;

        public string ChildContainerElement
        {
            get { return childContainerElement; }
            set { childContainerElement = value; }
        }

        private string childContainerCssClass = string.Empty;

        public string ChildContainerCssClass
        {
            get { return childContainerCssClass; }
            set { childContainerCssClass = value; }
        }

        private string childUlCssClass = string.Empty;

        public string ChildUlCssClass
        {
            get { return childUlCssClass; }
            set { childUlCssClass = value; }
        }

        private int childNodesPerUl = -1;

        public int ChildNodesPerUl
        {
            get { return childNodesPerUl; }
            set { childNodesPerUl = value; }
        }

        private string rootLevelLiCssClass = string.Empty;

        public string RootLevelLiCssClass
        {
            get { return rootLevelLiCssClass; }
            set { rootLevelLiCssClass = value; }
        }

        private string liCssClass = string.Empty;

        public string LiCssClass
        {
            get { return liCssClass; }
            set { liCssClass = value; }
        }

        private string itemDepthCssPrefix = string.Empty;

        public string ItemDepthCssPrefix
        {
            get { return itemDepthCssPrefix; }
            set { itemDepthCssPrefix = value; }
        }

        private string parentLiCssClass = string.Empty;

        public string ParentLiCssClass
        {
            get { return parentLiCssClass; }
            set { parentLiCssClass = value; }
        }

        private string ulSelectedCssClass = string.Empty;

        public string UlSelectedCssClass
        {
            get { return ulSelectedCssClass; }
            set { ulSelectedCssClass = value; }
        }

        private string liSelectedCssClass = string.Empty;

        public string LiSelectedCssClass
        {
            get { return liSelectedCssClass; }
            set { liSelectedCssClass = value; }
        }

        private string anchorSelectedCssClass = string.Empty;

        public string AnchorSelectedCssClass
        {
            get { return anchorSelectedCssClass; }
            set { anchorSelectedCssClass = value; }
        }

        private string anchorInnerHtmlTop = string.Empty;

        public string AnchorInnerHtmlTop
        {
            get { return anchorInnerHtmlTop; }
            set { anchorInnerHtmlTop = value; }
        }

        private string anchorInnerHtmlBottom = string.Empty;

        public string AnchorInnerHtmlBottom
        {
            get { return anchorInnerHtmlBottom; }
            set { anchorInnerHtmlBottom = value; }
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

        private string anchorCssClass = string.Empty;

        public string AnchorCssClass
        {
            get { return anchorCssClass; }
            set { anchorCssClass = value; }
        }
        
        private string anchorChildSelectedCssClass = string.Empty;

        public string AnchorChildSelectedCssClass
        {
            get { return anchorChildSelectedCssClass; }
            set { anchorChildSelectedCssClass = value; }
        }

        private string liChildSelectedCssClass = string.Empty;

        public string LiChildSelectedCssClass
        {
            get { return liChildSelectedCssClass; }
            set { liChildSelectedCssClass = value; }
        }

        private string ulChildSelectedCssClass = string.Empty;

        public string UlChildSelectedCssClass
        {
            get { return ulChildSelectedCssClass; }
            set { ulChildSelectedCssClass = value; }
        }

        
        private string extraTopMarkup = string.Empty;

        /// <summary>
        /// literal markup added inside the begin container tag 
        /// </summary>
        public string ExtraTopMarkup
        {
            get { return extraTopMarkup; }
            set { extraTopMarkup = value; }
        }

        private string extraBottomMarkup = string.Empty;

        /// <summary>
        /// literal markup added before the closing container tag 
        /// </summary>
        public string ExtraBottomMarkup
        {
            get { return extraBottomMarkup; }
            set { extraBottomMarkup = value; }
        }

        //private bool isSubMenu = false;

        //public bool IsSubMenu
        //{
        //    get { return isSubMenu; }
        //    set { isSubMenu = value; }
        //}

        private bool isMobileSkin = false;

        public bool IsMobileSkin
        {
            get { return isMobileSkin; }
            set { isMobileSkin = value; }
        }

        private string dividerElement = string.Empty;

        public string DividerElement
        {
            get { return dividerElement; }
            set { dividerElement = value; }
        }

        private string dividerCssClass = string.Empty;

        public string DividerCssClass
        {
            get { return dividerCssClass; }
            set { dividerCssClass = value; }
        }

        private bool renderHrefWhenUnclickable = true;
        public bool RenderHrefWhenUnclickable
        {
            get { return renderHrefWhenUnclickable; }
            set { renderHrefWhenUnclickable = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadSettings();

        }

        private void LoadSettings()
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

            if (startingNodePageId > -1)
            {
                startingNode = SiteUtils.GetSiteMapNodeForPage(rootNode, startingNodePageId);
            }
            else if (startingNodeOffset > -1)
            {
                startingNode = SiteUtils.GetOffsetNode(currentNode, startingNodeOffset);
            }
            //else if (isSubMenu)
            //{
            //    startingNode = SiteUtils.GetTopLevelParentNode(currentNode);
            //}
            
        }

        private string BuildUlClass(mojoSiteMapNode mojoNode)
        {
            string result = string.Empty;
            string spacer = string.Empty;

            //added 2013-11-08 https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=12214~1#post50717
            if (childUlCssClass.Length > 0)
            {
                result += spacer + childUlCssClass;
                spacer = " ";
            }

            if ((ulChildSelectedCssClass.Length > 0) && (currentNode != null)
                && (currentNode.IsDescendantOf(mojoNode))
                )
            {
                result += spacer + ulChildSelectedCssClass;
                spacer = " ";
            }

            if ((ulSelectedCssClass.Length > 0) && (currentNode != null)
                && (currentNode.PageGuid == mojoNode.PageGuid)
                )
            {
                result += spacer + ulSelectedCssClass;
                spacer = " ";
            }


            if (result.Length > 0)
            {
                return " class='" + result + "'";
            }

            return result;

        }

        private string BuildAnchorClass(mojoSiteMapNode mojoNode)
        {
            string result = string.Empty;
            string spacer = string.Empty;

            if (anchorCssClass.Length > 0)
            {
                result += anchorCssClass;
                spacer = " ";
            }

            if ((anchorChildSelectedCssClass.Length > 0) && (currentNode != null)
                && (currentNode.IsDescendantOf(mojoNode)) 
                )
            {
                result += spacer + anchorChildSelectedCssClass;
                spacer = " ";
            }

            if ((anchorSelectedCssClass.Length > 0) && (currentNode != null)
                && (currentNode.PageGuid == mojoNode.PageGuid)
                )
            {
                result += spacer + anchorSelectedCssClass;
                spacer = " ";
            }

            if ((renderCustomClassOnAnchor) && (mojoNode.MenuCssClass.Length > 0))
            {
                result += spacer + mojoNode.MenuCssClass;
                spacer = " ";
            }

            if (!mojoNode.IsClickable)
            {
                result += spacer + "unclickable";
            }

            if (result.Length > 0)
            {
                return " class='" + result + "'";
            }

            return result;

        }

        private string BuildLiClass(mojoSiteMapNode mojoNode)
        {
            string result = string.Empty;
            string spacer = string.Empty;

            if (((mojoNode.Depth == 0) 
                || (mojoNode.Depth == startingNodeOffset) 
                //|| ((startingNodeOffset > 0) &&(isSubMenu) &&(mojoNode.Depth == startingNodeOffset + 1)))
                || ((startingNodeOffset > -1) && (mojoNode.Depth == startingNodeOffset + 1)))
                && (rootLevelLiCssClass.Length > 0))
            {
                result = rootLevelLiCssClass;
                spacer = " ";
            }
            else if(liCssClass.Length > 0)
            {
                result = liCssClass;
                spacer = " ";
            }

            if (itemDepthCssPrefix.Length > 0)
            {
                result += spacer + itemDepthCssPrefix + mojoNode.Depth.ToInvariantString();
                spacer = " ";
            }

            //if ((parentLiCssClass.Length > 0) && (mojoNode.ChildNodes.Count > 0))
            if ((parentLiCssClass.Length > 0) && (mojoNode.HasVisibleChildren()))
            {
                result += spacer + parentLiCssClass;
                spacer = " ";
            }


            if ((liChildSelectedCssClass.Length > 0) && (currentNode != null)
                && (currentNode.IsDescendantOf(mojoNode)) 
                )
            {
                result += spacer + liChildSelectedCssClass;
                spacer = " ";
            }

            if ((liSelectedCssClass.Length > 0) && (currentNode != null)
                && (currentNode.PageGuid == mojoNode.PageGuid)
                )
            {
                result += spacer + liSelectedCssClass;
                spacer = " ";
            }

            if ((renderCustomClassOnLi) && (mojoNode.MenuCssClass.Length > 0))
            {
                result += spacer + mojoNode.MenuCssClass;
            }

            if (result.Length > 0)
            {
                return " class='" + result + "'";
            }

            return result;

        }

        protected override void Render(HtmlTextWriter writer)
        {
            
            if (HttpContext.Current == null) { return; }

            if (rootNode == null) { return; }

            if ((maxDataRenderDepth > -1)&&(startingNode.ChildNodes.Count > 0))
            {
                mojoSiteMapNode firstChildNode = startingNode.ChildNodes[0] as mojoSiteMapNode;
                if (firstChildNode == null) { return; }
                if (firstChildNode.Depth > maxDataRenderDepth) { return; }

            }

           
            if (containerElement.Length > 0)
            {
                
                writer.Write("<" + containerElement);
                if (containerCssClass.Length > 0)
                {
                    writer.Write(" class='" +  containerCssClass + "'");
                }
                writer.Write(">");
               
            }

            if (extraTopMarkup.Length > 0)
            {
                writer.Write(extraTopMarkup);
            }

            writer.Write("<ul");
            if (rootUlCssClass.Length > 0)
            {
                writer.Write(" class='" + rootUlCssClass  + "'");
            }
            writer.Write(">");

            int nodePos = 0;
            bool renderDivider = false;
            
            foreach (SiteMapNode childNode in startingNode.ChildNodes)
            {
                mojoSiteMapNode mojoNode = childNode as mojoSiteMapNode;
                if (mojoNode == null) { continue; }

                renderDivider = !String.IsNullOrEmpty(dividerElement) && (nodePos < startingNode.ChildNodes.Count);
                //RenderNode(writer, mojoNode);
                RenderNode(writer, mojoNode, renderDivider);

                nodePos += 1;


            }
            

            writer.WriteLine("</ul>");

            if (extraBottomMarkup.Length > 0)
            {
                writer.Write(extraBottomMarkup);
            }

            if (containerElement.Length > 0)
            {
                writer.WriteLine("</" + containerElement + ">");
            }

           // writer.WriteLine(" ");

        }



        private void RenderNode(HtmlTextWriter writer, mojoSiteMapNode mojoNode, bool renderDivider)
        {
            if (!ShouldRender(mojoNode)) { return; }

            writer.Write("<li");

            writer.Write(BuildLiClass(mojoNode));
            
            writer.Write(">");

            writer.Write("<a");
            writer.Write(BuildAnchorClass(mojoNode));
            if(mojoNode.OpenInNewWindow)
            {
                writer.Write(" target='_blank'");
            }
            if(mojoNode.LinkRel.Length > 0)
            {
                writer.Write(" rel='" + mojoNode.LinkRel + "'");
            }

            if (mojoNode.IsClickable || (renderHrefWhenUnclickable && !mojoNode.IsClickable))
            {
                writer.Write(" href='" + FormatUrl(mojoNode) + "'>");
            }
            else
            {
                writer.Write(">");
            }

            if ((anchorInnerHtmlTop.Length > 0) && (anchorInnerHtmlBottom.Length > 0))
            {
                writer.Write(anchorInnerHtmlTop);
            }

            writer.Write(mojoNode.Title);

            if ((anchorInnerHtmlTop.Length > 0) && (anchorInnerHtmlBottom.Length > 0))
            {
                writer.Write(anchorInnerHtmlBottom);
            }

            writer.Write("</a>");

            if ((renderDescription)&&(mojoNode.MenuDescription.Length > 0))
            {
                writer.Write("<span");
                if (descriptionCssClass.Length > 0)
                {
                    writer.Write(" class='" + descriptionCssClass + "'");
                }
                writer.Write(">");
                writer.Write(mojoNode.MenuDescription);
                writer.Write("</span>");
            }

            //if (mojoNode.ChildNodes.Count > 0)
            if(HasVisibleChildNodes(mojoNode))
            {
                if (childContainerElement.Length > 0)
                {
                    writer.Write("<" + childContainerElement);
                    if (childContainerCssClass.Length > 0)
                    {
                        writer.Write(" class='" + childContainerCssClass + "'");
                    }
                    writer.Write(">");
                }

                RenderChildNodes(writer, mojoNode);

                if (childContainerElement.Length > 0)
                {
                    writer.Write("</" + childContainerElement + ">");
                }
            }

            writer.Write("</li>");

            if (renderDivider)
            {
                writer.Write("<" + dividerElement);
                if (!String.IsNullOrEmpty(dividerCssClass))
                {
                    writer.Write(" class='" + dividerCssClass + "'");
                }
                writer.Write("></" + dividerElement + ">");
            }
            
        }


        

        private void RenderChildNodes(HtmlTextWriter writer, SiteMapNode node)
        {
            writer.Write("<ul");

            writer.Write(BuildUlClass((mojoSiteMapNode)node));

            writer.Write(">");
           
            
            int itemsAdded = 0;
            int trueItemsAdded = 0;
            int nodePos = 0;
            bool renderDivider = false;

            foreach (SiteMapNode childNode in node.ChildNodes)
            {
                mojoSiteMapNode mojoNode = childNode as mojoSiteMapNode;
                if (mojoNode == null) { continue; }
                if (!ShouldRender(mojoNode)) { continue; }
                renderDivider = !String.IsNullOrEmpty(dividerElement) && (nodePos < startingNode.ChildNodes.Count);
                //RenderNode(writer, mojoNode);
                RenderNode(writer, mojoNode, renderDivider);

                nodePos += 1;

                itemsAdded += 1;
                trueItemsAdded += 1;

                if (childNodesPerUl > -1)
                {
                    if ((itemsAdded == childNodesPerUl) && (trueItemsAdded < node.ChildNodes.Count))
                    {
                        //writer.Write("</ul><ul>");
                        writer.Write("</ul><ul" + BuildUlClass((mojoSiteMapNode)childNode) + ">");
                        itemsAdded = 0;
                    }
                }
            }

            writer.Write("</ul>");
        }

        private string FormatUrl(mojoSiteMapNode mapNode)
        {
            string itemUrl = Page.ResolveUrl(mapNode.Url);
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
                        itemUrl = insecureSiteRoot + mapNode.Url.Replace("~/", "/");
                    }
                }
                else
                {
                    if ((mapNode.UseSsl) || (siteSettings.UseSslOnAllPages))
                    {
                        if (mapNode.Url.StartsWith("~/"))
                        {
                            itemUrl = secureSiteRoot + mapNode.Url.Replace("~/", "/");
                        }
                    }
                }
            }

            return itemUrl;
        }

        private bool HasVisibleChildNodes(mojoSiteMapNode mapNode)
        {
            
            foreach (SiteMapNode childNode in mapNode.ChildNodes)
            {
                mojoSiteMapNode mojoNode = childNode as mojoSiteMapNode;
                if (mojoNode == null) { return false; }

                if (ShouldRender(mojoNode)) { return true; }
            }

            return false;
        }

        private bool ShouldRender(mojoSiteMapNode mapNode)
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

            if ((mapNode.HideAfterLogin) && (Page.Request.IsAuthenticated)) { remove = true; }

            if ((!isMobileSkin)&&(mapNode.PublishMode == mobileOnly)) { remove = true; }

            if ((isMobileSkin) && (mapNode.PublishMode == webOnly)) { remove = true; }

            if ((maxDataRenderDepth > -1) && (mapNode.Depth > maxDataRenderDepth)) { remove = true; }

            { return !remove; }

        }

    }
}