using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.Caching;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.Extensions;

namespace mojoPortal.Web;

public class mojoSiteMapProvider : StaticSiteMapProvider
{
    private static readonly ILog log = LogManager.GetLogger(typeof(mojoSiteMapProvider));
    private readonly object objLock = new();

    private SiteMapNode rootNode;
    private Dictionary<int, SiteMapNode> nodes = new Dictionary<int, SiteMapNode>(16);
    private Collection<PageSettings> menuPages;
    private bool useUrlRewriter = true;
    private bool sslIsAvailable = false;
    
    public override void Initialize(string name, NameValueCollection config)
    {
        if (config == null)
        {
            throw new ArgumentNullException("config");
        }

        if (String.IsNullOrEmpty(name))
        {
            name = "mojoSiteMapProvider";
        }

        if (string.IsNullOrEmpty(config["description"]))
        {
            config.Remove("description");
            config.Add("description", "mojoPortal site map provider");
        }

        useUrlRewriter = WebConfigSettings.UseUrlReWriting;
       
        sslIsAvailable = SiteUtils.SslIsAvailable();

        base.Initialize(name, config);            
    }

    public override SiteMapNode BuildSiteMap()
    {
        lock (this)
        {
            if (rootNode != null)
            {
                return rootNode;
            }

            menuPages = CacheHelper.GetMenuPages();
            int depth = 0;
            if (menuPages != null)
            {
                rootNode = CreateRootNode();
                foreach (PageSettings page in menuPages)
                {
                    if (page.ParentId <= -1)
                    {
                        SiteMapNode node = CreateSiteMapNode(page, depth);
                        
                        try
                        {
                            AddNode(node, GetParentNode(page));
                        }
                        catch (InvalidOperationException ex)
                        {
                            log.Error("failed to add node to sitemap", ex);

                        }
                        catch (HttpException ex)
                        {
                            log.Error("failed to add node to sitemap", ex);

                        }

                        if (page.UseUrl && page.Url.StartsWith("http"))
                        {
                            node.Url = page.Url;
                        }
                    }
                }
            }
        }

        string cacheKey = CacheHelper.GetSiteMapCacheKey();

        CacheDependency cacheDependency = CacheHelper.GetSiteMapCacheDependency();
        // the site map is cached by the runtime (not in our direct control) so we are caching a simple object and when that object is invalidated
        // we clear the sitemap with the callback event and then it is rebuilt and cached again
        HttpRuntime.Cache.Insert(
            cacheKey,
            new object(),
            cacheDependency,
            Cache.NoAbsoluteExpiration,
            Cache.NoSlidingExpiration,
            CacheItemPriority.Normal,
           new CacheItemRemovedCallback(OnSiteMapChanged));

        return rootNode;
    }

    private void CreateChildNodes(mojoSiteMapNode node, PageSettings page, int depth)
    {
        
        foreach (PageSettings p in menuPages)
        {
            if (p.ParentId == page.PageId)
            {
                SiteMapNode childNode = CreateSiteMapNode(p, depth);
                try
                {
                    AddNode(childNode, node);
                }
                catch (InvalidOperationException ex)
                {
                    log.Error("failed to add node to sitemap", ex);

                }
                catch (HttpException ex)
                {
                    log.Error("failed to add node to sitemap", ex);

                }

                if (p.UseUrl && p.Url.StartsWith("http"))
                {
                    childNode.Url = p.Url;
                }
            }
        }
    }


    private SiteMapNode CreateSiteMapNode(PageSettings page, int depth)
    {
        List<string> roleList = null;
        if (!String.IsNullOrEmpty(page.AuthorizedRoles))
        {
            roleList = page.AuthorizedRoles.SplitOnChar(';');
        }

        string pageUrl;
        if (
            page.UseUrl
            && !page.Url.StartsWith("http")
            && page.Url.Length > 0
            && useUrlRewriter
            )
        {

            pageUrl = page.Url ;
        }
        else
        {
           pageUrl = Invariant($"~/Default.aspx?pageid={page.PageId}");
        }

			mojoSiteMapNode node = new(
				this,
				page.PageId.ToString(),
				pageUrl,
				HttpContext.Current.Server.HtmlEncode(page.PageName),
				string.Empty,
				roleList,
				null,
				null,
				null)
			{
				PageGuid = page.PageGuid,
				PageId = page.PageId,
				ParentId = page.ParentId,
				Depth = depth,
				ViewRoles = page.AuthorizedRoles,
				EditRoles = page.EditRoles,
				DraftEditRoles = page.DraftEditOnlyRoles,
				CreateChildPageRoles = page.CreateChildPageRoles,
				CreateChildDraftPageRoles = page.CreateChildDraftRoles,
				IncludeInMenu = page.IncludeInMenu,
				IncludeInSiteMap = page.IncludeInSiteMap,
				ExpandOnSiteMap = page.ExpandOnSiteMap,
				IncludeInChildSiteMap = page.IncludeInChildSiteMap,
				IncludeInSearchMap = page.IncludeInSearchMap,
				LastModifiedUtc = page.LastModifiedUtc,
				ChangeFrequency = page.ChangeFrequency,
				SiteMapPriority = page.SiteMapPriority,
				OpenInNewWindow = page.OpenInNewWindow,
				HideAfterLogin = page.HideAfterLogin,
				UseSsl = (page.RequireSsl && sslIsAvailable),
				IsPending = page.IsPending,
				IsClickable = page.IsClickable,
				MenuCssClass = page.MenuCssClass,
				PublishMode = page.PublishMode,
				MenuDescription = page.MenuDescription,
				MenuImage = page.MenuImage,
				LinkRel = page.LinkRel,
				PubDateUtc = page.PubDateUtc
			};

			if (!this.nodes.ContainsKey(page.PageId))
        {
            nodes.Add(page.PageId, node);
        }

        //this.CreateChildNodes(node, page, pageIndex);
        this.CreateChildNodes(node, page, depth + 1);

        return node;
    }

    private SiteMapNode GetParentNode(PageSettings page)
    {
        if (page.ParentId <= -1)
        {
            return this.rootNode;

        }

        if (nodes.ContainsKey(page.ParentId))
        {
            return nodes[page.ParentId];
        }

        return this.rootNode;

    }

    private mojoSiteMapNode CreateRootNode()
    {
        string[] rolelist = new string[0];

        mojoSiteMapNode node = new mojoSiteMapNode(
            this,
            "-1");
        node.IsRootNode = true;

        nodes.Add(-1, node);

        return node;

    }


    protected override SiteMapNode GetRootNodeCore()
    {
         BuildSiteMap();
         return rootNode;  
    }

    public void ClearSiteMap()
    {
        Clear();
    }

    protected override void Clear()
    {
        lock (objLock)
        {
            this.rootNode = null;
            this.nodes.Clear();
            base.Clear();
        }
    }


    public void OnSiteMapChanged(string key, object item, CacheItemRemovedReason reason)
    {
        Clear();

    }

    public static void PopulateArrayList(
        ArrayList arrayList,
        SiteMapNode siteMapNode)
    {
        mojoSiteMapNode mojoNode = (mojoSiteMapNode)siteMapNode;

        if (!mojoNode.IsRootNode)
        {
            mojoNode.DepthIndicator = GetDepthIndicatorString(mojoNode.Depth);
            arrayList.Add(mojoNode);
            
        }

        foreach (SiteMapNode childNode in mojoNode.ChildNodes)
        {
            //recurse to populate children
            PopulateArrayList(arrayList, childNode);

        }


    }

    private static string GetDepthIndicatorString(int depth)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < depth; i++)
        {
            stringBuilder.Append("-");
        }
        return stringBuilder.ToString();
    }

}
