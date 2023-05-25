using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
	public class MenuList : List<mojoMenuItem>
    {
        private SiteMapDataSource siteMapDataSource;
        private bool isAdmin = false;
        private bool isContentAdmin = false;
        private bool isSiteEditor = false;
        private SiteSettings siteSettings;
        //private string secureSiteRoot = string.Empty;
        //private string insecureSiteRoot = string.Empty;
        //private bool resolveFullUrlsForMenuItemProtocolDifferences = false;
        //private bool isSecureRequest = false;
        private SiteMapNode rootNode = null;
        private mojoSiteMapNode currentNode = null;

        public int MaxDepth = -1;
        public int StartingPageId = -1;

        public bool ShowStartingNode = false;
        
        public SiteMapNode StartingNode { get; set; }
        
        public MenuList(SiteMapNode startingNode, bool showStartingNode = false)
        {
            StartingNode = startingNode;
            ShowStartingNode = showStartingNode;
            myInit();
		}

		public MenuList()
        {
            myInit();
        }

        private void myInit()
        {
			siteSettings = CacheHelper.GetCurrentSiteSettings();

			isAdmin = WebUser.IsAdmin;
			if (!isAdmin) { isContentAdmin = WebUser.IsContentAdmin; }
			if ((!isAdmin) && (!isContentAdmin)) { isSiteEditor = SiteUtils.UserIsSiteEditor(); }

			siteMapDataSource = new SiteMapDataSource
			{
				SiteMapProvider = "mojosite" + siteSettings.SiteId.ToInvariantString()
			};

			rootNode = siteMapDataSource.Provider.RootNode;
			currentNode = SiteUtils.GetCurrentPageSiteMapNode(rootNode);
            StartingNode ??= currentNode;
            if (ShowStartingNode) 
            { 
                var startNode = StartingNode as mojoSiteMapNode;
                this.Add(GetMenuItemFromNode(startNode)); 
            }
            else
            {
				this.AddRange(AddNodes(StartingNode));
			}
		}

        private List<mojoMenuItem> AddNodes(SiteMapNode startingNode)
        {
            List<mojoMenuItem> items = new();

            foreach (SiteMapNode childNode in startingNode.ChildNodes)
            {
				if (childNode is not mojoSiteMapNode mojoNode) { continue; }
				if (!ShouldAdd(mojoNode)) { continue; }
                items.Add(GetMenuItemFromNode(mojoNode));
            }

            return items;
        }

        private mojoMenuItem GetMenuItemFromNode (mojoSiteMapNode mojoNode)
        {
            return new mojoMenuItem
			{
				PageId = mojoNode.PageId,
				Name = mojoNode.IsRootNode ? Resources.Resource.PageSettingsRootLabel : mojoNode.Title,
				Description = mojoNode.MenuDescription,
				URL = mojoNode.IsRootNode ? string.Empty : WebUtils.ResolveUrl(mojoNode.Url),
				CssClass = mojoNode.MenuCssClass,
				Rel = mojoNode.LinkRel,
				Clickable = !mojoNode.IsRootNode && mojoNode.IsClickable,
				OpenInNewTab = mojoNode.OpenInNewWindow,
				PublishMode = mojoNode.PublishMode,
				LastModDate = mojoNode.LastModifiedUtc,
                Current = currentNode.PageId == mojoNode.PageId,
				Children = AddNodes(mojoNode)
			};
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

			if (mapNode.HideAfterLogin && WebUser.IsInRole("Authenticated")) { remove = true; }

			return !remove; 
        }
    }
}