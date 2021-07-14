using System.Collections.Generic;

namespace SuperFlexiUI.Models
{
	public class RequestObject
	{
		public int PageId { get; set; } = -1;
		public int ModuleId { get; set; } = -1;
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 10;
		public int ItemId { get; set; } = -1;
		public bool SortDescending { get; set; } = false;
		public string SortField { get; set; } = string.Empty;
		public string SearchField { get; set; } = string.Empty;
		public string SearchTerm { get; set; } = string.Empty;
		public IDictionary<string, string> SearchObject { get; set; }
		public string Field { get; set; } = string.Empty;
		//public Guid SolutionGuid { get; set; } = Guid.Empty;
		public bool GetAllForSolution { get; set; } = false;
	}
}
