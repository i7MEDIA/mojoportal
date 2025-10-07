﻿using System;
using System.Globalization;
using System.IO;
using System.Web.UI;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI;


public partial class SkinListPage : NonCmsBasePage
{
	private string newWindowMarkup = "onclick=\"window.open(this.href,'_blank');return false;\"";
	protected string PreviewText = string.Empty;
	protected string ManageText = string.Empty;
	protected bool allowEditing = false;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		LoadSettings();
		PopulateLabels();
		BindSkinList();
	}

	void btnUpload_Click(object sender, EventArgs e)
	{
		Page.Validate("upload");

		if ((Page.IsValid) && (uploader.HasFile))
		{
			//temporarily store the .zip in the /Data/Sites/[SiteID]/systemfiles folder
			string destFolder = SiteUtils.GetSiteSystemFolder();

			var di = new DirectoryInfo(destFolder);
			if (!di.Exists)
			{
				di.Create();
			}

			string destPath = Path.Combine(destFolder,
				Path.GetFileName(uploader.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles));

			if (File.Exists(destPath))
			{
				File.Delete(destPath);
			}

			uploader.SaveAs(destPath);

			// process the .zip, extract files
			var helper = new SkinHelper();
			helper.InstallSkins(SiteUtils.GetSiteSkinFolderPath(), destPath, chkOverwrite.Checked);

			// delete the temporary file
			File.Delete(destPath);
		}

		WebUtils.SetupRedirect(this, Request.RawUrl);
	}

	private void BindSkinList()
	{
		DirectoryInfo[] skins = SiteUtils.GetSkinList(siteSettings);
		rptSkins.DataSource = skins;
		rptSkins.DataBind();
	}

	protected string BuildDownloadLink(string skinName)
	{
		string innerMarkup = $"<img src='{ImageSiteRoot}/Data/SiteImages/Icons/zip.png' alt='{DevTools.DownloadSkin}' />";

		return $"<a href='{SiteRoot}/DesignTools/DownloadSkin.aspx?s={skinName}' title='{string.Format(CultureInfo.InvariantCulture, Resource.DownloadSkinFormat, skinName)}' class='skinzip' {newWindowMarkup}>{innerMarkup}</a>";
	}

	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, DevTools.SkinManagement);

		heading.Text = DevTools.SkinManagement;

		lnkAdminMenu.Text = Resource.AdminMenuLink;
		lnkAdminMenu.NavigateUrl = $"{SiteRoot}/Admin/AdminMenu.aspx";

		lnkDesignerTools.Text = DevTools.DesignTools;
		lnkDesignerTools.NavigateUrl = $"{SiteRoot}/DesignTools/Default.aspx";

		lnkThisPage.Text = DevTools.SkinManagement;
		lnkThisPage.NavigateUrl = $"{SiteRoot}/DesignTools/SkinList.aspx";

		btnUpload.Text = Resource.UploadSkinButton;
		chkOverwrite.Text = Resource.OverwriteExistingSkinFiles;
		regexZipFile.ErrorMessage = Resource.OnlyZipFilesAllowed;
		reqZipFile.ErrorMessage = Resource.ZipFileIsRequired;

		PreviewText = Resource.View;
		ManageText = Resource.Manage;

		uploader.AddFileText = Resource.SelectFileButton;
		uploader.DropFileText = Resource.DropFile;
		uploader.DropFilesText = Resource.DropSkinFiles;
		uploader.UploadButtonText = Resource.UploadButton;
		uploader.UploadCompleteText = Resource.UploadComplete;
		uploader.UploadingText = Resource.Uploading;
	}

	private void LoadSettings()
	{
		regexZipFile.ValidationExpression = SecurityHelper.GetRegexValidationForAllowedExtensions(".zip");
		allowEditing = WebConfigSettings.AllowEditingSkins && (AppConfig.MultiTenancy.AllowEditingSkins || siteSettings.IsServerAdminSite);
		ScriptConfig.IncludeColorBox = true;
		divUpload.Visible = allowEditing;

		if (allowEditing)
		{
			uploader.MaxFilesAllowed = WebConfigSettings.MaxSkinFilesToUploadAtOnce;
			uploader.AcceptFileTypes = SecurityHelper.GetRegexValidationForAllowedExtensionsJqueryFileUploader("zip");
			uploader.UploadButtonClientId = btnUpload.ClientID;
			string fileSystemToken = Global.FileSystemToken.ToString();
			uploader.ServiceUrl = $"{SiteRoot}/Services/FileService.ashx?cmd=uploadskin&t={fileSystemToken}";
			uploader.FormFieldClientId = chkOverwrite.ClientID; // 

			string refreshFunction = "function refresh () { window.location.reload(true); } ";

			uploader.UploadCompleteCallback = "refresh";

			ScriptManager.RegisterClientScriptBlock(
				this,
				GetType(), "refresh",
				refreshFunction,
				true);
		}

		AddClassToBody("administration");
		AddClassToBody("designtools");
	}

	#region OnInit
	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		btnUpload.Click += new EventHandler(btnUpload_Click);

		SuppressMenuSelection();
		SuppressPageMenu();
	}
	#endregion
}