using System;
using System.IO;
using System.Web;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Plugins.HtmlInclude;

public partial class Edit : NonCmsBasePage
{
	int moduleID = -1;

	#region OnInit

	protected override void OnPreInit(EventArgs e)
	{
		AllowSkinOverride = true;
		base.OnPreInit(e);
	}

	override protected void OnInit(EventArgs e)
	{
		Load += new EventHandler(Page_Load);
		btnUpdate.Click += new EventHandler(btnUpdate_Click);
		btnCancel.Click += new EventHandler(btnCancel_Click);
		base.OnInit(e);
	}

	#endregion


	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Request.IsAuthenticated)
		{
			SiteUtils.RedirectToLoginPage(this);
			return;
		}

		SecurityHelper.DisableBrowserCache();

		moduleID = WebUtils.ParseInt32FromQueryString("mid", -1);

		if (!UserCanEditModule(moduleID))
		{
			SiteUtils.RedirectToAccessDeniedPage();
			return;
		}

		PopulateLabels();

		if (!IsPostBack)
		{
			PopulateControls();
			if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
			{
				hdnReturnUrl.Value = Request.UrlReferrer.ToString();

			}
		}
	}


	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, HtmlIncludeResources.EditHtmlFragmentIncludeSettingsLabel);
		heading.Text = HtmlIncludeResources.EditHtmlFragmentIncludeSettingsLabel;
		btnUpdate.Text = HtmlIncludeResources.EditHtmlFragmentUpdateButton;
		SiteUtils.SetButtonAccessKey(btnUpdate, HtmlIncludeResources.EditHtmlFragmentUpdateButtonAccessKey);

		btnCancel.Text = HtmlIncludeResources.EditHtmlFragmentCancelButton;
		SiteUtils.SetButtonAccessKey(btnCancel, HtmlIncludeResources.EditHtmlFragmentCancelButtonAccessKey);
	}


	private void PopulateControls()
	{
		ddInclude.DataSource = GetFragmentList();
		ddInclude.DataBind();

		if (moduleID > -1)
		{
			var settings = ModuleSettings.GetModuleSettings(moduleID);
			var includeFile = settings.ParseString("HtmlFragmentSourceSetting");

			if (!string.IsNullOrWhiteSpace(includeFile))
			{
				ddInclude.SelectedValue = includeFile;
			}
		}
	}


	protected FileInfo[] GetFragmentList()
	{
		string p;

		if (WebConfigSettings.HtmlFragmentUseMediaFolder)
		{
			p = Invariant($"/Data/Sites/{siteSettings.SiteId}/media/htmlfragments");
		}
		else
		{
			p = Invariant($"/Data/Sites/{siteSettings.SiteId}/htmlfragments");
		}

		string filePath = HttpContext.Current.Server.MapPath(WebUtils.GetApplicationRoot() + p);
		if (Directory.Exists(filePath))
		{
			var dir = new DirectoryInfo(filePath);

			return dir?.GetFiles();
		}

		return null;
	}


	private void btnUpdate_Click(object sender, EventArgs e)
	{
		var m = new Module(moduleID);

		ModuleSettings.UpdateModuleSetting(m.ModuleGuid, m.ModuleId, "HtmlFragmentSourceSetting", ddInclude.SelectedValue);

		CurrentPage.UpdateLastModifiedTime();
		CacheHelper.ClearModuleCache(m.ModuleId);

		if (hdnReturnUrl.Value.Length > 0)
		{
			WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
			return;
		}

		WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
	}


	private void btnCancel_Click(object sender, EventArgs e)
	{
		if (hdnReturnUrl.Value.Length > 0)
		{
			WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
			return;
		}

		WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
	}
}