using mojoPortal.Business;
using mojoPortal.Web;
using System.IO;
using System.Web;
using System.Web.Http;

namespace mojoPortal.Features.UI.BetterImageGallery
{
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
			var imgPath = HttpContext.Current.Server.MapPath("~/Data/systemfiles/BetterImageGalleryCache/" + path);
			var fileInfo = new FileInfo(imgPath);

			return !fileInfo.Exists
				? (IHttpActionResult)NotFound()
				: new FileResult(fileInfo.FullName);
		}
	}
}