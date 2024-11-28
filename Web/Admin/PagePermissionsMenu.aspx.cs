using System;
using System.Globalization;
using System.Web.UI;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Web.AdminUI;

public partial class PagePermissionsMenuPage : NonCmsBasePage
{
	private int pageId = -1;
	private bool isAdmin = false;
	private bool isContentAdmin = false;
	private bool isSiteEditor = false;


	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Request.IsAuthenticated)
		{
			SiteUtils.RedirectToLoginPage(this);
			return;
		}
		LoadSettings();

		if (!isAdmin && !isContentAdmin && !isSiteEditor)
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		if ((!isAdmin) && (CurrentPage.EditRoles == "Admins;"))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		PopulateLabels();
	}


	private void PopulateLabels()
	{
		heading.Text = string.Format(CultureInfo.InvariantCulture, Resource.ManagePermissionsFormat, CurrentPage.PageName);
		Title = heading.Text;

		var pagePermissionsUrl = $"{WebConfigSettings.AdminDirectoryLocation}/PagePermission.aspx".ToLinkBuilder().PageId(pageId);

		lnkPageViewRoles.Text = Resource.PageLayoutViewRolesLabel;
		lnkPageViewRoles.NavigateUrl = pagePermissionsUrl.AddParam("p", "v").ToString();

		lnkPageEditRoles.Text = Resource.PageLayoutEditRolesLabel;
		lnkPageEditRoles.NavigateUrl = pagePermissionsUrl.SetParam("p", "e").ToString();

		lnkPageDraftRoles.Text = Resource.PageLayoutDraftEditRolesLabel;
		lnkPageDraftRoles.NavigateUrl = pagePermissionsUrl.SetParam("p", "d").ToString();

		lnkChildPageRoles.Text = Resource.PageLayoutCreateChildPageRolesLabel;
		lnkChildPageRoles.NavigateUrl = pagePermissionsUrl.SetParam("p", "ce").ToString();

		Control c = Master.FindControl("Breadcrumbs");
		if (c != null)
		{
			BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
			crumbs.ForceShowBreadcrumbs = true;
			crumbs.AddedCrumbs
				= $"{crumbs.ItemWrapperTop}<a href=\"{$"{WebConfigSettings.AdminDirectoryLocation}/PageSettings.aspx".ToLinkBuilder().PageId(pageId)}\" class=\"unselectedcrumb\">{Resource.PageSettingsPageTitle}</a>{crumbs.ItemWrapperBottom}";
		}
	}


	private void LoadSettings()
	{
		pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
		isAdmin = WebUser.IsAdmin;
		if (!isAdmin)
		{
			isContentAdmin = WebUser.IsContentAdmin;
			isSiteEditor = SiteUtils.UserIsSiteEditor();
		}
	}


	#region OnInit

	protected override void OnPreInit(EventArgs e)
	{
		pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);

		AllowSkinOverride = pageId > -1;
		base.OnPreInit(e);

		if (
			(pageId > -1)
			   && siteSettings.AllowPageSkins
				&& (CurrentPage != null)
				&& (CurrentPage.Skin.Length > 0)
				)
		{

			if (Global.RegisteredVirtualThemes)
			{
				this.Theme = Invariant($"pageskin-{siteSettings.SiteId}{CurrentPage.Skin}");
			}
			else
			{
				this.Theme = "pageskin";
			}
		}

	}

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		this.Load += new EventHandler(this.Page_Load);

		SuppressPageMenu();
	}

	#endregion
}
