using System.Collections.Generic;

namespace SuperFlexiUI.Models
{
	public class WidgetModel
	{
		public ModuleConfiguration Config { get; set; }
		public Dictionary<string,List<string>> DynamicLists { get; set; }
		public List<object> Items { get; set; }
		public ModuleModel Module { get; set; }
		public PageModel Page { get; set; }
		public PaginationModel Pagination { get; set; }
		public SiteModel Site { get; set; }
	}

	public class PaginationModel
	{
		public int PageCount { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public int TotalItems { get; set; }
	}
}