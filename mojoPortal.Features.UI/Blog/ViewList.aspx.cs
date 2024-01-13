using System;
using mojoPortal.Business;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.BlogUI;

public partial class ViewList : mojoBasePage
{
	protected int pageId = -1;
	protected int moduleId = -1;
	private bool userCanEdit = false;
	private int pageNumber = 1;

	#region OnInit

	protected override void OnPreInit(EventArgs e)
	{
		AllowSkinOverride = true;
		base.OnPreInit(e);
	}

	override protected void OnInit(EventArgs e)
	{
		this.Load += new EventHandler(this.Page_Load);
		base.OnInit(e);
	}

	#endregion

	protected void Page_Load(object sender, EventArgs e)
	{
		if (SiteUtils.SslIsAvailable() && (siteSettings.UseSslOnAllPages || CurrentPage.RequireSsl))
		{
			SiteUtils.ForceSsl();
		}
		else
		{
			SiteUtils.ClearSsl();
		}
		LoadParams();

		if (!UserCanViewPage(moduleId))
		{
			SiteUtils.RedirectToAccessDeniedPage();
			return;
		}

		LoadSettings();
		PopulateControls();

	}

	private void PopulateControls()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, BlogResources.PostList);

		moduleTitle.EditUrl = SiteRoot + "/Blog/EditPost.aspx";
		moduleTitle.EditText = BlogResources.BlogAddPostLabel;
		moduleTitle.ModuleInstance = GetModule(moduleId);
		moduleTitle.CanEdit = userCanEdit;

		if (userCanEdit)
		{
			moduleTitle.LiteralExtraMarkup =
				"&nbsp;<a href='"
				+ SiteRoot
				+ "/Blog/Manage.aspx?pageid=" + pageId.ToInvariantString()
				+ "&amp;mid=" + moduleId.ToInvariantString()
				+ "' class='ModuleEditLink' title='"
				+ BlogResources.Administration + "'>"
				+ BlogResources.Administration + "</a>"
				;
		}

		postList.ModuleId = moduleId;
		postList.PageId = pageId;
		postList.IsEditable = userCanEdit;
		BlogConfiguration config = new BlogConfiguration(ModuleSettings.GetModuleSettings(moduleId));
		postList.Config = config;
		postList.SiteRoot = SiteRoot;
		postList.ImageSiteRoot = ImageSiteRoot;

		if (config.InstanceCssClass.Length > 0) { pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass); }

		searchBoxTop.Visible = config.ShowBlogSearchBox && !displaySettings.HideSearchBoxInPostList;

		//make this page look as close as possible to the way a cms page with the blog module on it looks
		LoadSideContent(true, true);
		LoadAltContent(BlogConfiguration.ShowTopContent, BlogConfiguration.ShowBottomContent);

	}

	private void LoadSettings()
	{
		userCanEdit = UserCanEditModule(moduleId);
		//if (userCanEdit) { countOfDrafts = Blog.CountOfDrafts(moduleId); }

		if ((CurrentPage != null) && (CurrentPage.BodyCssClass.Length > 0))
		{
			AddClassToBody(CurrentPage.BodyCssClass);
		}

		AddClassToBody("blogviewlist");
		if (BlogConfiguration.UseNoIndexFollowMetaOnLists)
		{
			SiteUtils.AddNoIndexFollowMeta(Page);
		}
	}

	private void LoadParams()
	{
		pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
		moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
		pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
	}
}