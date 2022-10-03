// Author:          
// Created:         2013-01-13
// Last Modified:   2014-12-17

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
		/// <summary>
		/// if specified then the menu will show the child pages of the page corresponding to the specific page id
		/// it must be a page within the current site, ie it must exist in the site map data for the current site
		/// </summary>
		public int StartingNodePageId { get; set; } = -1;

		public int StartingNodeOffset { get; set; } = -1;
		/// <summary>
		/// -1 = no limit
		/// </summary>
		public int MaxDataRenderDepth { get; set; } = -1;

		//private bool isMobileSkin = false;
		private int mobileOnly = (int)ContentPublishMode.MobileOnly;
        private int webOnly = (int)ContentPublishMode.WebOnly;

		public string ContainerElement { get; set; } = string.Empty;

		public string ContainerCssClass { get; set; } = string.Empty;

		public string RootUlCssClass { get; set; } = string.Empty;

		public bool RenderDescription { get; set; } = true;
		public string DescriptionCssClass { get; set; } = string.Empty;

		public string ChildContainerElement { get; set; } = string.Empty;

		public string ChildContainerCssClass { get; set; } = string.Empty;

		public string ChildUlCssClass { get; set; } = string.Empty;

		public int ChildNodesPerUl { get; set; } = -1;

		public string RootLevelLiCssClass { get; set; } = string.Empty;

		public string LiCssClass { get; set; } = string.Empty;

		public string ItemDepthCssPrefix { get; set; } = string.Empty;

		public string ParentLiCssClass { get; set; } = string.Empty;

		public string UlSelectedCssClass { get; set; } = string.Empty;

		public string LiSelectedCssClass { get; set; } = string.Empty;

		public string AnchorSelectedCssClass { get; set; } = string.Empty;

		public string AnchorInnerHtmlTop { get; set; } = string.Empty;

		public string AnchorInnerHtmlBottom { get; set; } = string.Empty;

		public bool RenderCustomClassOnLi { get; set; } = true;

		public bool RenderCustomClassOnAnchor { get; set; } = false;

		public string AnchorCssClass { get; set; } = string.Empty;

		public string AnchorChildSelectedCssClass { get; set; } = string.Empty;

		public string LiChildSelectedCssClass { get; set; } = string.Empty;

		public string UlChildSelectedCssClass { get; set; } = string.Empty;

		/// <summary>
		/// literal markup added inside the begin container tag 
		/// </summary>
		public string ExtraTopMarkup { get; set; } = string.Empty;

		/// <summary>
		/// literal markup added before the closing container tag 
		/// </summary>
		public string ExtraBottomMarkup { get; set; } = string.Empty;

		public bool IsMobileSkin { get; set; } = false;

		public string DividerElement { get; set; } = string.Empty;

		public string DividerCssClass { get; set; } = string.Empty;
		public bool RenderHrefWhenUnclickable { get; set; } = true;

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

            if (StartingNodePageId > -1)
            {
                startingNode = SiteUtils.GetSiteMapNodeForPage(rootNode, StartingNodePageId);
            }
            else if (StartingNodeOffset > -1)
            {
                startingNode = SiteUtils.GetOffsetNode(currentNode, StartingNodeOffset);
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
            if (ChildUlCssClass.Length > 0)
            {
                result += spacer + ChildUlCssClass;
                spacer = " ";
            }

            if ((UlChildSelectedCssClass.Length > 0) && (currentNode != null)
                && (currentNode.IsDescendantOf(mojoNode))
                )
            {
                result += spacer + UlChildSelectedCssClass;
                spacer = " ";
            }

            if ((UlSelectedCssClass.Length > 0) && (currentNode != null)
                && (currentNode.PageGuid == mojoNode.PageGuid)
                )
            {
                result += spacer + UlSelectedCssClass;
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

            if (AnchorCssClass.Length > 0)
            {
                result += AnchorCssClass;
                spacer = " ";
            }

            if ((AnchorChildSelectedCssClass.Length > 0) && (currentNode != null)
                && (currentNode.IsDescendantOf(mojoNode)) 
                )
            {
                result += spacer + AnchorChildSelectedCssClass;
                spacer = " ";
            }

            if ((AnchorSelectedCssClass.Length > 0) && (currentNode != null)
                && (currentNode.PageGuid == mojoNode.PageGuid)
                )
            {
                result += spacer + AnchorSelectedCssClass;
                spacer = " ";
            }

            if ((RenderCustomClassOnAnchor) && (mojoNode.MenuCssClass.Length > 0))
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
                || (mojoNode.Depth == StartingNodeOffset) 
                //|| ((startingNodeOffset > 0) &&(isSubMenu) &&(mojoNode.Depth == startingNodeOffset + 1)))
                || ((StartingNodeOffset > -1) && (mojoNode.Depth == StartingNodeOffset + 1)))
                && (RootLevelLiCssClass.Length > 0))
            {
                result = RootLevelLiCssClass;
                spacer = " ";
            }
            else if(LiCssClass.Length > 0)
            {
                result = LiCssClass;
                spacer = " ";
            }

            if (ItemDepthCssPrefix.Length > 0)
            {
                result += spacer + ItemDepthCssPrefix + mojoNode.Depth.ToInvariantString();
                spacer = " ";
            }

            //if ((parentLiCssClass.Length > 0) && (mojoNode.ChildNodes.Count > 0))
            if ((ParentLiCssClass.Length > 0) && (mojoNode.HasVisibleChildren()))
            {
                result += spacer + ParentLiCssClass;
                spacer = " ";
            }


            if ((LiChildSelectedCssClass.Length > 0) && (currentNode != null)
                && (currentNode.IsDescendantOf(mojoNode)) 
                )
            {
                result += spacer + LiChildSelectedCssClass;
                spacer = " ";
            }

            if ((LiSelectedCssClass.Length > 0) && (currentNode != null)
                && (currentNode.PageGuid == mojoNode.PageGuid)
                )
            {
                result += spacer + LiSelectedCssClass;
                spacer = " ";
            }

            if ((RenderCustomClassOnLi) && (mojoNode.MenuCssClass.Length > 0))
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

            if ((MaxDataRenderDepth > -1)&&(startingNode.ChildNodes.Count > 0))
            {
                mojoSiteMapNode firstChildNode = startingNode.ChildNodes[0] as mojoSiteMapNode;
                if (firstChildNode == null) { return; }
                if (firstChildNode.Depth > MaxDataRenderDepth) { return; }

            }

           
            if (ContainerElement.Length > 0)
            {
                
                writer.Write("<" + ContainerElement);
                if (ContainerCssClass.Length > 0)
                {
                    writer.Write(" class='" +  ContainerCssClass + "'");
                }
                writer.Write(">");
               
            }

            if (ExtraTopMarkup.Length > 0)
            {
                writer.Write(ExtraTopMarkup);
            }

            writer.Write("<ul");
            if (RootUlCssClass.Length > 0)
            {
                writer.Write(" class='" + RootUlCssClass  + "'");
            }
            writer.Write(">");

            int nodePos = 0;
            bool renderDivider = false;
            
            foreach (SiteMapNode childNode in startingNode.ChildNodes)
            {
                mojoSiteMapNode mojoNode = childNode as mojoSiteMapNode;
                if (mojoNode == null) { continue; }

                renderDivider = !String.IsNullOrEmpty(DividerElement) && (nodePos < startingNode.ChildNodes.Count);
                //RenderNode(writer, mojoNode);
                RenderNode(writer, mojoNode, renderDivider);

                nodePos += 1;


            }
            

            writer.WriteLine("</ul>");

            if (ExtraBottomMarkup.Length > 0)
            {
                writer.Write(ExtraBottomMarkup);
            }

            if (ContainerElement.Length > 0)
            {
                writer.WriteLine("</" + ContainerElement + ">");
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

            if (mojoNode.IsClickable || (RenderHrefWhenUnclickable && !mojoNode.IsClickable))
            {
                writer.Write(" href='" + FormatUrl(mojoNode) + "'>");
            }
            else
            {
                writer.Write(">");
            }

            if ((AnchorInnerHtmlTop.Length > 0) && (AnchorInnerHtmlBottom.Length > 0))
            {
                writer.Write(AnchorInnerHtmlTop);
            }

            writer.Write(mojoNode.Title);

            if ((AnchorInnerHtmlTop.Length > 0) && (AnchorInnerHtmlBottom.Length > 0))
            {
                writer.Write(AnchorInnerHtmlBottom);
            }

            writer.Write("</a>");

            if ((RenderDescription)&&(mojoNode.MenuDescription.Length > 0))
            {
                writer.Write("<span");
                if (DescriptionCssClass.Length > 0)
                {
                    writer.Write(" class='" + DescriptionCssClass + "'");
                }
                writer.Write(">");
                writer.Write(mojoNode.MenuDescription);
                writer.Write("</span>");
            }

            //if (mojoNode.ChildNodes.Count > 0)
            if(HasVisibleChildNodes(mojoNode))
            {
                if (ChildContainerElement.Length > 0)
                {
                    writer.Write("<" + ChildContainerElement);
                    if (ChildContainerCssClass.Length > 0)
                    {
                        writer.Write(" class='" + ChildContainerCssClass + "'");
                    }
                    writer.Write(">");
                }

                RenderChildNodes(writer, mojoNode);

                if (ChildContainerElement.Length > 0)
                {
                    writer.Write("</" + ChildContainerElement + ">");
                }
            }

            writer.Write("</li>");

            if (renderDivider)
            {
                writer.Write("<" + DividerElement);
                if (!String.IsNullOrEmpty(DividerCssClass))
                {
                    writer.Write(" class='" + DividerCssClass + "'");
                }
                writer.Write("></" + DividerElement + ">");
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
                renderDivider = !String.IsNullOrEmpty(DividerElement) && (nodePos < startingNode.ChildNodes.Count);
                //RenderNode(writer, mojoNode);
                RenderNode(writer, mojoNode, renderDivider);

                nodePos += 1;

                itemsAdded += 1;
                trueItemsAdded += 1;

                if (ChildNodesPerUl > -1)
                {
                    if ((itemsAdded == ChildNodesPerUl) && (trueItemsAdded < node.ChildNodes.Count))
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

            if ((!IsMobileSkin)&&(mapNode.PublishMode == mobileOnly)) { remove = true; }

            if ((IsMobileSkin) && (mapNode.PublishMode == webOnly)) { remove = true; }

            if ((MaxDataRenderDepth > -1) && (mapNode.Depth > MaxDataRenderDepth)) { remove = true; }

            { return !remove; }

        }

    }
}