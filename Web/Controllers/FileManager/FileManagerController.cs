using mojoPortal.FileSystem;
using System.Linq;
using System.Web.Mvc;

namespace mojoPortal.Web.Controllers
{
	public class FileManagerController : Controller
	{
		IFileSystem fileSystem = null;

		// GET: FileManager
		public ActionResult Index(string view)
		{
			LoadSettings();
			var rootName = fileSystem.VirtualRoot.Split('/');

			ViewBag.OverwriteFiles = WebConfigSettings.FileManagerOverwriteFiles;
			ViewBag.RootName = rootName[rootName.Count() - 2];
			ViewBag.fileSystemToken = Global.FileSystemToken.ToString();
			ViewBag.virtualPath = fileSystem.VirtualRoot;
			ViewBag.view = view;

			return View();
		}

		private void LoadSettings()
		{
			FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
			fileSystem = p.GetFileSystem();
		}
	}
}