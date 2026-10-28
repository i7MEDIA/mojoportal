using System.IO;
using System.Web;
using System.Web.Http;

namespace mojoPortal.Features.UI.BetterImageGallery;

public class BetterImageGalleryController : ApiController
{
	// Get list Folders and Images
	[HttpGet]
	[Route("api/BetterImageGallery/")]
	public IHttpActionResult GetItems([FromUri] string path, int moduleId = -1)
	{
		var gallery = new BetterImageGalleryService(moduleId);
		var items = gallery.GetImages(path);

		return Ok(items);
	}

	// Get cached images from systemfiles/BetterImageGallery
	[HttpGet]
	[Route("api/BetterImageGallery/imagehandler")]
	public IHttpActionResult ImageHandler([FromUri] string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return NotFound();
		}

		string cacheRootVirtualPath = "~/Data/systemfiles/BetterImageGalleryCache/";
		string virtualTargetPath = $"{cacheRootVirtualPath}{path.ToLinkBuilder(false)}";

		if (!Web.Framework.IOHelper.IsDecendentFile(cacheRootVirtualPath, virtualTargetPath))
		{
			return NotFound();
		}

		var imgPath = HttpContext.Current.Server.MapPath(virtualTargetPath);
		var fileInfo = new FileInfo(imgPath);

		return !fileInfo.Exists || !Web.Framework.IOHelper.IsWebImageFile(fileInfo)
			? (IHttpActionResult)NotFound()
			: new FileResult(fileInfo.FullName);
	}
}