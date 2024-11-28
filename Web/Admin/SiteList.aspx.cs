using System;
using System.Data;
using System.Globalization;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI;

public partial class SiteListPage : NonCmsBasePage
{
	protected string CurrentSiteName = string.Empty;
	private int totalPages = 1;
	private int pageNumber = 1;
	private int pageSize = 15;
	protected bool showSiteIDInSiteList = false;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Request.IsAuthenticated)
		{
			SiteUtils.RedirectToLoginPage(this);
			return;
		}

		if ((!WebUser.IsAdmin) || (!siteSettings.IsServerAdminSite))
		{
			WebUtils.SetupRedirect(this, $"{WebConfigSettings.AdminDirectoryLocation}/SiteSettings.aspx".ToLinkBuilder().ToString());
			return;
		}

		LoadSettings();
		PopulateLabels();
		BindList();

	}


	private void BindList()
	{
		using IDataReader reader = SiteSettings.GetPageOfOtherSites(
			siteSettings.SiteId,
			pageNumber,
			pageSize,
			out totalPages);

		string pageUrl = $"{WebConfigSettings.AdminDirectoryLocation}/SiteList.aspx".ToLinkBuilder().PageNumber("{0}").ToString();

		pgr.PageURLFormat = pageUrl;
		pgr.ShowFirstLast = true;
		pgr.CurrentIndex = pageNumber;
		pgr.PageSize = pageSize;
		pgr.PageCount = totalPages;
		pgr.Visible = totalPages > 1;

		rptSites.DataSource = reader;
		rptSites.DataBind();
	}


	protected string FormatSiteId(int siteId)
	{
		return string.Format(CultureInfo.InvariantCulture, Resource.SiteIdFormat, siteId);
	}


	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.SiteList);
		heading.Text = Resource.SiteList;

		lnkAdminMenu.Text = Resource.AdminMenuLink;
		lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
		lnkAdminMenu.NavigateUrl = $"{WebConfigSettings.AdminDirectoryLocation}/AdminMenu.aspx".ToLinkBuilder().ToString();

		lnkSiteList.Text = Resource.SiteList;
		lnkSiteList.ToolTip = Resource.SiteList;
		lnkSiteList.NavigateUrl = $"{WebConfigSettings.AdminDirectoryLocation}/SiteList.aspx".ToLinkBuilder().ToString();

		lnkNewSite.Text = Resource.CreateNewSite;
		lnkNewSite.NavigateUrl = $"{WebConfigSettings.AdminDirectoryLocation}/SiteSettings.aspx".ToLinkBuilder().SiteId(-1).ToString();
	}

	private void LoadSettings()
	{
		CurrentSiteName = string.Format(CultureInfo.InvariantCulture, Resource.ThisSiteFormat, siteSettings.SiteName.RemoveMarkup());
		pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
		showSiteIDInSiteList = WebConfigSettings.ShowSiteIdInSiteList;
		pageSize = WebConfigSettings.SiteListPageSize;

		AddClassToBody("administration");
	}


	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);

		SuppressMenuSelection();
		SuppressPageMenu();
	}

	#endregion
}
