using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using Resources;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mojoPortal.Web.Controllers
{
	public class FileManagerController : Controller
	{
		// GET: FileManager
		[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
		public ActionResult Index()
		{
			var model = LoadSettings();

			return View(model);
		}

		// GET: Pages
		[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
		public ActionResult Pages(string type, string editor)
		{
			var model = LoadSettings();

			return View(model);
		}

		private dynamic LoadSettings()
		{
			IFileSystem fileSystem = null;
			SiteSettings siteSettings = null;
			ILog log = LogManager.GetLogger(typeof(FileServiceController));

			siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (siteSettings == null)
			{
				log.Info(Resource.FileSystemSiteSettingsNotLoaded);
			}

			FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
			if (p == null)
			{
				log.Info(string.Format(Resource.FileSystemProviderNotLoaded, WebConfigSettings.FileSystemProvider));
			}

			fileSystem = p.GetFileSystem();
			if (fileSystem == null)
			{
				log.Info(string.Format(Resource.FileSystemNotLoadedFromProvider, WebConfigSettings.FileSystemProvider));
			}

			var rootName = fileSystem.VirtualRoot.Split('/');
			var queryString = new
			{
				type = Request.QueryString.Get("type"),
				editor = Request.QueryString.Get("editor"),
				inputId = Request.QueryString.Get("inputId"),
				CKEditor = Request.QueryString.Get("CKEditor"),
				CKEditorFuncNum = Request.QueryString.Get("CKEditorFuncNum")
			};

			var model = new Models.FileManager
			{
				OverwriteFiles = WebConfigSettings.FileManagerOverwriteFiles,
				RootName = rootName[rootName.Count() - 2],
				FileSystemToken = Global.FileSystemToken.ToString(),
				VirtualPath = VirtualPathUtility.RemoveTrailingSlash(fileSystem.FileBaseUrl + fileSystem.VirtualRoot.Replace("~", string.Empty)),
				View = Request.QueryString.Get("view"),
				Type = queryString.type,
				Editor = queryString.editor,
				InputId = queryString.inputId,
				CKEditorFuncNumber = queryString.CKEditorFuncNum,
				QueryString = queryString,

				Upload = WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles) ? "true" : "false",
				Rename = WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles) ? "true" : "false",
				Move = WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles) ? "true" : "false",
				Copy = WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles) ? "true" : "false",
				Edit = WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles) ? "true" : "false",
				Compress = WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles) ? "true" : "false",
				CompressChooseName = "true",
				Extract = WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles) ? "true" : "false",
				Download = "true",
				DownloadMultiple = "true",
				Preview = "true",
				Remove = WebUser.IsInRoles(siteSettings.RolesThatCanDeleteFilesInEditor) || WebUser.IsContentAdmin ? "true" : "false",
				CreateFolder = WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles) ? "true" : "false",

				PagePickerLinkText = Resource.FileManagerPagePickerLink,
				BackToWebsiteLinkText = Resource.FileManagerBackToWebsite,
				BackToFileManagerLinkText = Resource.FileManagerBackToManagerLink
			};

			return model;
		}
	}
}