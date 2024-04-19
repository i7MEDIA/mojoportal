using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Web.GalleryUI;

public partial class FolderGalleryEditPage : NonCmsBasePage
{
	protected static readonly ILog log = LogManager.GetLogger(typeof(FolderGalleryEditPage));

	private int pageId = -1;
	private int moduleId = -1;
	private FolderGalleryConfiguration config = new();
	private Hashtable moduleSettings = null;
	private string basePath = string.Empty;
	private Guid featureGuid = new("9e58fcda-90de-4ed7-abc7-12f096f5c58f");

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Request.IsAuthenticated)
		{
			SiteUtils.RedirectToLoginPage(this);
			return;
		}
		LoadSettings();

		if (!UserCanEditModule(moduleId, featureGuid))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		PopulateLabels();
		PopulateControls();
	}

	private void PopulateControls()
	{
		lblBasePath.Text = basePath;

		if (Page.IsPostBack)
		{
			return;
		}

		if (config.GalleryRootFolder.Length > 0)
		{
			txtFolderName.Text = config.GalleryRootFolder.Replace(basePath, string.Empty);
		}
	}

	void btnSave_Click(object sender, EventArgs e)
	{
		var m = new Module(moduleId);

		string newPath = basePath + txtFolderName.Text;
		try
		{
			if (!Directory.Exists(Server.MapPath(newPath)))
			{
				lblError.Text = FolderGalleryResources.FolderGalleryFolderNotExistsMessage;
				return;
			}
		}
		catch (HttpException)
		{
			//thrown at Server.MapPath if the path is not a valid virtual path
			txtFolderName.Text = string.Empty;
			lblError.Text = FolderGalleryResources.FolderGalleryFolderNotExistsMessage;
			return;
		}

		ModuleSettings.UpdateModuleSetting(m.ModuleGuid, m.ModuleId, "FolderGalleryRootFolder", newPath);
		WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
	}

	void btnUpload_Click(object sender, EventArgs e)
	{
		// as long as javascript is available this code should never execute
		// because the standard file input is replaced by javascript and the file upload happens
		// at the service url /FolderGallery/upload.ashx
		// this is fallback implementation

		string pathToGallery = basePath;

		if (config.GalleryRootFolder.Length > 0)
		{
			pathToGallery = config.GalleryRootFolder;
		}

		try
		{
			if (uploader.HasFile)
			{
				string ext = Path.GetExtension(uploader.FileName);
				if (SiteUtils.IsAllowedUploadBrowseFile(ext, WebConfigSettings.ImageFileExtensions))
				{
					string destPath = Path.Combine(Server.MapPath(pathToGallery), Path.GetFileName(uploader.FileName));
					if (File.Exists(destPath))
					{
						File.Delete(destPath);
					}
					uploader.SaveAs(destPath);
				}
			}

			WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}
		catch (UnauthorizedAccessException ex)
		{
			lblError.Text = ex.Message;
		}
		catch (ArgumentException ex)
		{
			lblError.Text = ex.Message;
		}
	}

	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, FolderGalleryResources.EditPageTitle);

		btnSave.Text = FolderGalleryResources.FolderGallerySaveButton;
		lnkCancel.Text = FolderGalleryResources.FolderGalleryCancelButton;
		lblError.Text = string.Empty;

		btnUpload.Text = FolderGalleryResources.FolderGalleryUploadImagesButton;
		regexUpload.ErrorMessage = FolderGalleryResources.AllowedExtensionsMessage;

		// borowing these from Image Gallery feature instead of replicating them
		uploader.AddFilesText = GalleryResources.SelectFilesButton;
		uploader.AddFileText = GalleryResources.SelectFileButton;
		uploader.DropFilesText = GalleryResources.DropFiles;
		uploader.DropFileText = GalleryResources.DropFile;
		uploader.UploadButtonText = GalleryResources.BulkUploadButton;
		uploader.UploadCompleteText = GalleryResources.UploadComplete;
		uploader.UploadingText = GalleryResources.Uploading;

		Control c = Master.FindControl("Breadcrumbs");
		if (c != null)
		{
			BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
			crumbs.ForceShowBreadcrumbs = true;
		}
	}

	private void LoadSettings()
	{
		lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();

		//this is now done within this class, not in mojoSetup
		//mojoSetup.EnsureFolderGalleryFolder(siteSettings);

		pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
		moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);

		basePath = string.Format(CultureInfo.InvariantCulture, FolderGalleryConfiguration.BasePathFormat, siteSettings.SiteId);

		moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
		config = new FolderGalleryConfiguration(moduleSettings);

		// this check is for backward compat with galleries previously created below ~/Data/Sites/{0}/FolderGalleries/
		if (config.GalleryRootFolder.Length > 0)
		{
			if (!config.GalleryRootFolder.StartsWith(basePath))
			{
				// legacy path
				basePath = Invariant($"~/Data/Sites/{siteSettings.SiteId}/FolderGalleries/");
			}
		}

		if (FileSystemHelper.LoadFileSystem() is IFileSystem fileSystem)
		{
			try
			{
				fileSystem.CreateFolder(basePath);
			}
			catch (IOException ex)
			{
				log.Error(ex);
			}
		}		

		if (!WebUser.IsAdminOrContentAdmin)
		{
			pnlUpload.Visible = config.AllowEditUsersToUpload;
			pnlEdit.Visible = config.AllowEditUsersToChangeFolderPath;
		}

		uploader.MaxFilesAllowed = FolderGalleryConfiguration.MaxFilesToUploadAtOnce;
		uploader.AcceptFileTypes = SecurityHelper.GetRegexValidationForAllowedExtensionsJqueryFileUploader(WebConfigSettings.ImageFileExtensions);
		uploader.UploadButtonClientId = btnUpload.ClientID;
		uploader.ServiceUrl = Invariant($"{SiteRoot}/FolderGallery/upload.ashx?pageid={pageId}&mid={moduleId}");
		uploader.FormFieldClientId = hdnState.ClientID; // not really used but prevents submitting all the form 

		string funcName = $"refresh{moduleId}";
		string refreshFunction = Invariant($"function {funcName} () {{ window.location.href = '{SiteUtils.GetCurrentPageUrl()}'; }}");

		uploader.UploadCompleteCallback = Invariant($"{funcName}");

		ScriptManager.RegisterClientScriptBlock(
			this,
			GetType(), Invariant($"{funcName}"),
			refreshFunction,
			true);

		AddClassToBody("foldergalleryedit");
	}

	#region OnInit

	protected override void OnPreInit(EventArgs e)
	{
		AllowSkinOverride = true;
		base.OnPreInit(e);
	}

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		btnSave.Click += new EventHandler(btnSave_Click);
		btnUpload.Click += new EventHandler(btnUpload_Click);
	}
	#endregion
}