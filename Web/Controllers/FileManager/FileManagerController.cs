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
			var userFolder = VirtualPathUtility.RemoveTrailingSlash(fileSystem.FileBaseUrl + fileSystem.Permission.UserFolder.Replace("~", string.Empty));
			var rootName = virtualPath.Split('/');

			var manageFiles = fileSystem.UserHasUploadPermission.ToString().ToLowerInvariant();
			var deleteFiles = (WebUser.IsInRoles(siteSettings.RolesThatCanDeleteFilesInEditor) || WebUser.IsContentAdmin || userFolder == virtualPath).ToString().ToLowerInvariant();

			var model = new Models.FileManager
			{
				OverwriteFiles = WebConfigSettings.FileManagerOverwriteFiles,
				FileSystemToken = Global.FileSystemToken.ToString(),
				RootName = userFolder == virtualPath ? Resource.UserFolder : rootName[rootName.Count() - 1],
				VirtualPath = virtualPath,
				ReturnFullPath = queryParams.returnFullPath,
				View = Request.QueryString.Get("view"),
				Type = queryParams.type,
				Editor = queryParams.editor,
				InputId = queryParams.inputId,
				CKEditorFuncNumber = queryParams.CKEditorFuncNum,
				QueryString = queryParams,
				UserFolder = userFolder,
				UserFolderName = Resource.UserFolder,

				Upload = manageFiles,
				Rename = manageFiles,
				Move = manageFiles,
				Copy = manageFiles,
				Edit = manageFiles,
				Compress = manageFiles,
				CompressChooseName = "true",
				Extract = manageFiles,
				Download = "true",
				DownloadMultiple = "true",
				Preview = "true",
				Remove = deleteFiles,
				CreateFolder = manageFiles,

				PagePickerLinkText = Resource.FileManagerPagePickerLink,
				BackToWebsiteLinkText = Resource.FileManagerBackToWebsite,
				BackToFileManagerLinkText = Resource.FileManagerBackToManagerLink
			};

			return model;
		}
	}
}