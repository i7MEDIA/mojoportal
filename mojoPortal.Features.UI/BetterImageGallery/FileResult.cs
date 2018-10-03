using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace mojoPortal.Features.UI.BetterImageGallery
{
	class FileResult : IHttpActionResult
	{
		private readonly string _filePath;
		private readonly string _contentType;

		public FileResult(string filePath, string contentType = null)
		{
			_filePath = filePath ?? throw new ArgumentNullException("filePath");
			_contentType = contentType;
		}

		public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
		{
			var response = new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StreamContent(File.OpenRead(_filePath))
			};

			var contentType = _contentType ?? MimeMapping.GetMimeMapping(Path.GetExtension(_filePath));
			response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

			return Task.FromResult(response);
		}
	}
}