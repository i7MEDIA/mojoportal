using System.Collections.Generic;

namespace SuperFlexiUI.Models
{
	public class ReturnObject
	{
		public string Status { get; set; }
		public object Data { get; set; }
		public int TotalPages { get; set; }
		public int TotalRows { get; set; }
		public bool AllowEdit { get; set; }
		public int CmsPageId { get; set; }
		public int CmsModuleId { get; set; }
		public IDictionary<string, string> ExtraData { get; set; }
	}
}
