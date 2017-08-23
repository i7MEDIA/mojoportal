using System.Collections.Generic;

namespace mojoPortal.Business
{
	public class PostListModel
	{
		public string ModuleTitle { get; set; }
		public string ModulePageUrl { get; set; }
		public IEnumerable<BlogPostModel> Posts { get; set; }
		public Pagination Pagination { get; set; }
	}

	public class Pagination
	{
		public int ItemsPerPage { get; set; }
	}
}