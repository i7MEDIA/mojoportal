using System.Collections.ObjectModel;
using System.Web;
using System.Xml;
using mojoPortal.Core.Extensions;

namespace mojoPortal.Web;

/// <summary>
/// Used for templates on new sites
/// </summary>
public class ContentPage
{
	private ContentPage()
	{ }

	private Collection<ContentPage> childPages = [];

	public string ResourceFile { get; private set; } = string.Empty;

	public string Name { get; private set; } = string.Empty;

	public string Title { get; private set; } = string.Empty;
	public string Url { get; private set; } = string.Empty;

	public int PageOrder { get; private set; } = 1;

	public string MenuImage { get; private set; } = string.Empty;

	public bool RequireSsl { get; private set; } = false;

	public bool ShowBreadcrumbs { get; private set; } = false;

	public string VisibleToRoles { get; private set; } = "All Users;";

	public string EditRoles { get; private set; } = string.Empty;

	public string DraftEditRoles { get; private set; } = string.Empty;

	public string CreateChildPageRoles { get; private set; } = string.Empty;

	public string PageMetaKeyWords { get; private set; } = string.Empty;

	public string PageMetaDescription { get; private set; } = string.Empty;

	public string BodyCssClass { get; private set; } = string.Empty;

	public string MenuCssClass { get; private set; } = string.Empty;

	public bool IncludeInMenu { get; private set; } = true;

	public bool IsClickable { get; private set; } = true;

	public bool IncludeInSiteMap { get; private set; } = true;

	public bool IncludeInChildPagesSiteMap { get; private set; } = true;

	public bool AllowBrowserCaching { get; private set; } = true;

	public bool ShowChildPageBreadcrumbs { get; private set; } = false;

	public bool ShowHomeCrumb { get; private set; } = false;

	public bool ShowChildPagesSiteMap { get; private set; } = false;

	public bool HideFromAuthenticated { get; private set; } = false;

	public bool EnableComments { get; private set; } = false;

	public Collection<ContentPage> ChildPages => childPages;

	public Collection<ContentPageItem> PageItems { get; } = [];

	public static void LoadPages(ContentPageConfiguration contentPageConfig, XmlNode documentElement)
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

		if (pagesNode is null) return;

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
		var contentPage = new ContentPage();

		var attributeCollection = node.Attributes;

		contentPage.ResourceFile = attributeCollection.ParseStringFromAttribute("resourceFile", contentPage.ResourceFile);
		contentPage.Name = attributeCollection.ParseStringFromAttribute("name", contentPage.Name);
		contentPage.Title = attributeCollection.ParseStringFromAttribute("title", contentPage.Title);
		contentPage.Url = attributeCollection.ParseStringFromAttribute("url", contentPage.Url);
		contentPage.MenuImage = attributeCollection.ParseStringFromAttribute("menuImage", contentPage.MenuImage);
		contentPage.PageOrder = attributeCollection.ParseInt32FromAttribute("pageOrder", contentPage.PageOrder);
		contentPage.VisibleToRoles = attributeCollection.ParseStringFromAttribute("visibleToRoles", contentPage.VisibleToRoles);
		contentPage.EditRoles = attributeCollection.ParseStringFromAttribute("editRoles", contentPage.EditRoles);
		contentPage.DraftEditRoles = attributeCollection.ParseStringFromAttribute("draftEditRoles", contentPage.DraftEditRoles);
		contentPage.CreateChildPageRoles = attributeCollection.ParseStringFromAttribute("createChildPageRoles", contentPage.CreateChildPageRoles);
		contentPage.PageMetaKeyWords = attributeCollection.ParseStringFromAttribute("pageMetaKeyWords", contentPage.PageMetaKeyWords);
		contentPage.PageMetaDescription = attributeCollection.ParseStringFromAttribute("pageMetaDescription", contentPage.PageMetaDescription);
		contentPage.RequireSsl = attributeCollection.ParseBoolFromAttribute("requireSSL", contentPage.RequireSsl);
		contentPage.ShowBreadcrumbs = attributeCollection.ParseBoolFromAttribute("showBreadcrumbs", contentPage.ShowBreadcrumbs);
		contentPage.IncludeInMenu = attributeCollection.ParseBoolFromAttribute("includeInMenu", contentPage.IncludeInMenu);
		contentPage.IsClickable = attributeCollection.ParseBoolFromAttribute("isClickable", contentPage.IsClickable);
		contentPage.IncludeInSiteMap = attributeCollection.ParseBoolFromAttribute("includeInSiteMap", contentPage.IncludeInSiteMap);
		contentPage.IncludeInChildPagesSiteMap = attributeCollection.ParseBoolFromAttribute("includeInChildPagesSiteMap", contentPage.IncludeInChildPagesSiteMap);
		contentPage.AllowBrowserCaching = attributeCollection.ParseBoolFromAttribute("allowBrowserCaching", contentPage.AllowBrowserCaching);
		contentPage.ShowChildPageBreadcrumbs = attributeCollection.ParseBoolFromAttribute("showChildPageBreadcrumbs", contentPage.ShowChildPageBreadcrumbs);
		contentPage.ShowHomeCrumb = attributeCollection.ParseBoolFromAttribute("showHomeCrumb", contentPage.ShowHomeCrumb);
		contentPage.ShowChildPagesSiteMap = attributeCollection.ParseBoolFromAttribute("showChildPagesSiteMap", contentPage.ShowChildPagesSiteMap);
		contentPage.HideFromAuthenticated = attributeCollection.ParseBoolFromAttribute("hideFromAuthenticated", contentPage.HideFromAuthenticated);
		contentPage.EnableComments = attributeCollection.ParseBoolFromAttribute("enableComments", contentPage.EnableComments);
		contentPage.BodyCssClass = attributeCollection.ParseStringFromAttribute("bodyCssClass", contentPage.BodyCssClass);
		contentPage.MenuCssClass = attributeCollection.ParseStringFromAttribute("menuCssClass", contentPage.MenuCssClass);

		foreach (XmlNode contentFeatureNode in node.ChildNodes)
		{
			if (contentFeatureNode.Name == "contentFeature")
			{
				ContentPageItem.LoadPageItem(contentPage, contentFeatureNode);
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

		if (parentPage is null)
		{
			contentPageConfig.ContentPages.Add(contentPage);
		}
		else
		{
			parentPage.ChildPages.Add(contentPage);
		}

		if (childPagesNode is not null)
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