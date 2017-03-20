using Newtonsoft.Json;
using System;

namespace mojoPortal.Web.Dtos
{
	public class WebFolderDto
	{
		public string Name { get; set; }
		public string Rights { get; set; }
		public long Size { get; set; }
		[JsonProperty("Date")]
		public DateTime Modified { get; set; }
		public string ContentType { get; set; }
	}
}