using System;
using mojoPortal.FileSystem;
using Newtonsoft.Json;

namespace mojoPortal.Web.Dtos;

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

public static class MappingExtensions
{
	public static FileServiceDto ToDto(this WebFile webFile) => new()
	{
		Name = webFile.Name,
		Size = webFile.Size,
		Modified = webFile.Modified,
		ContentType = "file",
	};


	public static FileServiceDto ToDto(this WebFolder webFolder) => new()
	{
		Modified = webFolder.Modified,
		Name = webFolder.Name,
		ContentType = "dir"
	};
}