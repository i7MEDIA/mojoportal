using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using mojoPortal.Web.Models;
using Resources;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace mojoPortal.Web.Controllers
{
	public class FileManagerController : Controller
	{
		// GET: FileManager
		[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
		public ActionResult Index([FromUri] QueryParams queryParams)
		{
			var model = LoadSettings(queryParams);

			return View(model);
		}

		// GET: Pages
		[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
		public ActionResult Pages([FromUri] QueryParams queryParams)
		{
			var model = LoadSettings(queryParams);

			return View(model);
		}

		private dynamic LoadSettings(QueryParams queryParams)
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

			var virtualPath = VirtualPathUtility.RemoveTrailingSlash(fileSystem.FileBaseUrl + fileSystem.VirtualRoot.Replace("~", string.Empty));
			var rootName = virtualPath.Split('/');

			var model = new Models.FileManager
			{
				OverwriteFiles = WebConfigSettings.FileManagerOverwriteFiles,
				FileSystemToken = Global.FileSystemToken.ToString(),
				RootName = rootName[rootName.Count() - 1],
				VirtualPath = virtualPath,
				ReturnFullPath = queryParams.returnFullPath,
				View = Request.QueryString.Get("view"),
				Type = queryParams.type,
				Editor = queryParams.editor,
				InputId = queryParams.inputId,
				CKEditorFuncNumber = queryParams.CKEditorFuncNum,
				QueryString = queryParams,

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