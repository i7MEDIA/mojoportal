using mojoPortal.FileSystem;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mojoPortal.Web.Controllers
{
	public class FileManagerController : Controller
	{
		IFileSystem fileSystem = null;

		// GET: FileManager
		public ActionResult Index()
		{
			LoadSettings();
			var rootName = fileSystem.VirtualRoot.Split('/');

			ViewBag.OverwriteFiles = WebConfigSettings.FileManagerOverwriteFiles;
			ViewBag.RootName = rootName[rootName.Count() - 2];
			ViewBag.fileSystemToken = Global.FileSystemToken.ToString();
			ViewBag.virtualPath = VirtualPathUtility.RemoveTrailingSlash(fileSystem.VirtualRoot.Replace("~", string.Empty));
			ViewBag.view = Request.QueryString["view"];
			ViewBag.type = Request.QueryString["type"];
			ViewBag.editor = Request.QueryString["editor"];

			return View();
		}

		// GET: Pages
		public ActionResult Pages()
		{
			ViewBag.type = Request.QueryString["type"];
			ViewBag.editor = Request.QueryString["editor"];
			return View();
		}

		private void LoadSettings()
		{
			FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
			fileSystem = p.GetFileSystem();
		}
	}
}