using System.Collections.Generic;

namespace mojoPortal.Web.Models;

public class FileService
{
	public class RequestObject
	{
		public string Action { get; set; } = string.Empty;
		public string CompressedFilename { get; set; } = string.Empty;
		public string Content { get; set; } = string.Empty;
		public string Destination { get; set; } = string.Empty;
		public string Item { get; set; } = string.Empty;
		public List<string> Items { get; set; } = new List<string>();
		public string NewItemPath { get; set; } = string.Empty;
		public string NewPath { get; set; } = string.Empty;
		public string FolderName { get; set; } = string.Empty;
		public string Path { get; set; } = string.Empty;
		public string Perms { get; set; } = string.Empty;
		public string PermsCode { get; set; } = string.Empty;
		public string Recursive { get; set; } = string.Empty;
		public string SingleFileName { get; set; } = string.Empty;
		public string ToFilename { get; set; } = string.Empty;
	}

	public class ReturnObject
	{
		public ReturnObject(ReturnMessage returnMessage) => Result = returnMessage;

		public ReturnMessage Result { get; set; }
	}

	public class ReturnMessage
	{
		public bool Success { get; set; } = true;
		public string Error { get; set; }
	}
}