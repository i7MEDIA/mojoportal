using System.Collections.Generic;

namespace mojoPortal.Web.Models
{
	public class FileService
	{
		public class RequestObject
		{
			public string Action { get; set; }
			public string CompressedFilename { get; set; }
			public string Content { get; set; }
			public string Destination { get; set; }
			public string Item { get; set; }
			public List<string> Items { get; set; }
			public string NewItemPath { get; set; }
			public string NewPath { get; set; }
			public string Path { get; set; }
			public string Perms { get; set; }
			public string PermsCode { get; set; }
			public string Recursive { get; set; }
			public string SingleFileName { get; set; }
			public string ToFilename { get; set; }
		}


		public class ReturnObject
		{
			public ReturnObject(ReturnMessage returnMessage)
			{
				Result = returnMessage;
			}
			public ReturnMessage Result { get; set; }
		}


		public class ReturnMessage
		{
			private bool success = true;
			public bool Success
			{
				get
				{
					return success;
				}
				set
				{
					success = value;
				}
			}
			public string Error { get; set; }
		}
	}
}