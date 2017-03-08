using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using mojoPortal.Web.Models;
using Resources;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mojoPortal.Web.Controllers
{
	public class FileManagerController : Controller
	{
		IFileSystem fileSystem = null;
		private SiteSettings siteSettings = null;
		private static readonly ILog log = LogManager.GetLogger(typeof(FileServiceController));

		// GET: FileManager
		public ActionResult Index(string view, string type, string editor)
		{
			LoadSettings();

			var rootName = fileSystem.VirtualRoot.Split('/');

			var model = new Models.FileManager
			{
				OverwriteFiles = WebConfigSettings.FileManagerOverwriteFiles,
				RootName = rootName[rootName.Count() - 2],
				FileSystemToken = Global.FileSystemToken.ToString(),
				VirtualPath = VirtualPathUtility.RemoveTrailingSlash(fileSystem.VirtualRoot.Replace("~", string.Empty)),
				View = view,
				Type = type,
				Editor = editor,
				Upload = "true",
				Rename = "true",
				Move = "true",
				Copy = "true",
				Edit = "true",
				Compress = "true",
				CompressChooseName = "true",
				Extract = "true",
				Download = "true",
				DownloadMultiple = "true",
				Preview = "true",
				Remove = "true",
				CreateFolder = "true",
				PagePickerLinkText = Resource.FileManagerPagePickerLink,
				BackToWebsiteLinkText = Resource.FileManagerBackToWebsite
			};

			if (!WebUser.IsInRoles(siteSettings.RolesThatCanDeleteFilesInEditor))
			{
				model.Remove = "false";
			}

			return View(model);
		}

		// GET: Pages
		public ActionResult Pages(string type, string editor)
		{
			var model = new Models.FileManager
			{
				Type = type,
				Editor = editor,
				BackToFileManagerLinkText = Resource.FileManagerBackToManagerLink
			};

			return View(model);
		}

		private void LoadSettings()
		{
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
		}
	}
}