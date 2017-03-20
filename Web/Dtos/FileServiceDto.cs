using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace mojoPortal.Web.Dtos
{
	public class FileServiceDto
	{
		public string Name { get; set; }
		public string Rights { get; set; }
		public long Size { get; set; }
		[JsonProperty("Date")]
		//[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
		//[JsonConverter(typeof(IsoDateTimeConverter))]
		public DateTime Modified { get; set; }
		[JsonProperty("Type")]
		public string ContentType { get; set; }
	}
}