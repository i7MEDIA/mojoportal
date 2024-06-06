using System;
using System.Globalization;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Web.BlogUI;

public partial class ManagePage : NonCmsBasePage
{
	private int pageId = -1;
	private int moduleId = -1;
	private int countOfDrafts = 0;
	private int countOfExpiredPosts = 0;
	private BlogConfiguration config;
	private SiteUser currentUser = null;


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
		if (!Request.IsAuthenticated)
		{
			SiteUtils.RedirectToLoginPage(this);
			return;
		}
		LoadParams();


		if (!UserCanEditModule(moduleId, Blog.FeatureGuid))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		LoadSettings();
		PopulateLabels();
		PopulateControls();

	}

	private void PopulateControls()
	{


	}


	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, BlogResources.BlogAdministration);

		heading.Text = BlogResources.BlogAdministration;

		Control c = Master.FindControl("Breadcrumbs");
		if (c != null)
		{
			BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
			crumbs.ForceShowBreadcrumbs = true;
			crumbs.AddedCrumbs = $"{crumbs.ItemWrapperTop}<a href=\"{"Blog/Manage.aspx".ToLinkBuilder().PageId(pageId).ModuleId(moduleId)}\" class=\"selectedcrumb\">{BlogResources.Administration}</a>{crumbs.ItemWrapperBottom}";
		}

		lnkCategories.Text = BlogResources.ManageCategories;
		lnkNewPost.Text = BlogResources.NewPostOrDraft;
		lnkDrafts.Text = string.Format(CultureInfo.InvariantCulture, BlogResources.DraftsFormat, countOfDrafts);
		lnkClosedPosts.Text = string.Format(CultureInfo.InvariantCulture, BlogResources.ExpiredPostsFormat, countOfExpiredPosts);
	}

	private void LoadSettings()
	{
		currentUser = SiteUtils.GetCurrentSiteUser();
		config = new BlogConfiguration(ModuleSettings.GetModuleSettings(moduleId));

		lnkCategories.NavigateUrl = "Blog/EditCategory.aspx".ToLinkBuilder().PageId(pageId).ModuleId(moduleId).ToString();
		//$"{SiteRoot}/Blog/EditCategory.aspx?pageid={pageId.ToInvariantString()}&mid={moduleId.ToInvariantString()}";

		lnkNewPost.NavigateUrl = "Blog/EditPost.aspx".ToLinkBuilder().PageId(pageId).ModuleId(moduleId).ToString();
		//$"{SiteRoot}/Blog/EditPost.aspx?pageid={pageId.ToInvariantString()}&mid={moduleId.ToInvariantString()}";

		lnkDrafts.NavigateUrl = "Blog/Drafts.aspx".ToLinkBuilder().PageId(pageId).ModuleId(moduleId).ToString();
		//$"{SiteRoot}/Blog/Drafts.aspx?pageid={pageId.ToInvariantString()}&mid={moduleId.ToInvariantString()}";

		lnkClosedPosts.NavigateUrl = "Blog/ClosedPosts.aspx".ToLinkBuilder().PageId(pageId).ModuleId(moduleId).ToString();
		//$"{SiteRoot}/Blog/ClosedPosts.aspx?pageid={pageId.ToInvariantString()}&mid={moduleId.ToInvariantString()}";

		if (currentUser is null) { return; }

		if (BlogConfiguration.SecurePostsByUser)
		{
			if (WebUser.IsInRoles(config.ApproverRoles))
			{
				countOfDrafts = Blog.GetCountOfDrafts(moduleId, Guid.Empty);
			}
			else
			{
				countOfDrafts = Blog.GetCountOfDrafts(moduleId, currentUser.UserGuid);
			}
		}
		else
		{
			countOfDrafts = Blog.GetCountOfDrafts(moduleId, Guid.Empty);
		}

		countOfExpiredPosts = Blog.GetCountClosed(moduleId);

	}

	private void LoadParams()
	{
		pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
		moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
	}


	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		this.Load += new EventHandler(this.Page_Load);
	}

	#endregion
}
