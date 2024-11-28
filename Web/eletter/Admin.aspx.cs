using System;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using Resources;

namespace mojoPortal.Web.ELetterUI;

public partial class AdminPage : NonCmsBasePage
{
	private static readonly ILog log = LogManager.GetLogger(typeof(AdminPage));

	private bool isSiteEditor = false;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Request.IsAuthenticated)
		{
			SiteUtils.RedirectToLoginPage(this);
			return;
		}

		LoadSettings();

		if (!isSiteEditor && !WebUser.IsNewsletterAdmin)
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		PopulateLabels();
		PopulateControls();
	}

	private void PopulateControls()
	{
		var letterInfoList = LetterInfo.GetAll(siteSettings.SiteGuid);

		try
		{
			litLetters.Text = RazorBridge.RenderPartialToString("_Admin", letterInfoList, "eletter");
		}
		catch (System.Web.HttpException ex)
		{
			log.Error($"layout for Newsletter _Admin was not found in skin {SiteUtils.DetermineSkinBaseUrl(true, Page)}. perhaps it is in a different skin. Error was: {ex}");
		}
	}

	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuNewsletterAdminLabel);

		heading.Text = Resource.AdminMenuNewsletterAdminLabel;

		lnkAdminMenu.Text = Resource.AdminMenuLink;
		lnkAdminMenu.NavigateUrl = $"{WebConfigSettings.AdminDirectoryLocation}/AdminMenu.aspx".ToLinkBuilder().ToString();

		lnkThisPage.Text = Resource.NewsLetterAdministrationHeading;
		lnkThisPage.NavigateUrl = "eletter/Admin.aspx".ToLinkBuilder().ToString();
	}

	private void LoadSettings()
	{
		//pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
		isSiteEditor = SiteUtils.UserIsSiteEditor();

		lnkAdminMenu.Visible = WebUser.IsAdminOrContentAdmin;
		litLinkSeparator1.Visible = lnkAdminMenu.Visible;

		AddClassToBody("administration");
		AddClassToBody("eletteradmin");
	}

	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		this.Load += new EventHandler(this.Page_Load);
		SuppressMenuSelection();
		SuppressPageMenu();
		ScriptConfig.IncludeJQTable = true;
	}

	#endregion
}