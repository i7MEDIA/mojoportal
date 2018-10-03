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
		public IHttpActionResult GetItems([FromUri] string path)
		{
			var gallery = new GalleryCore();
			var items = gallery.GetImages(path);

			return Ok(items);
		}

		// Get cached images from systemfiles/BetterImageGallery
		[HttpGet]
		[Route("api/BetterImageGallery/imagehandler")]
		public IHttpActionResult ImageHandler([FromUri] string imagePath)
		{
			var imgPath = HttpContext.Current.Server.MapPath("/Data/systemfiles/BetterImageGalleryCache/" + imagePath);
			var fileInfo = new FileInfo(imgPath);

			return !fileInfo.Exists
				? (IHttpActionResult)NotFound()
				: new FileResult(fileInfo.FullName);
		}
	}
}