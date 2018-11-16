namespace mojoPortal.Web
{
	public class BreadCrumb
	{
		public BreadCrumb() { }
		public string CssClass { get; set; }
		public string IconCssClass { get; set; }
		public bool IsCurrent { get; set; } = false;
		public string Parent { get; set; }
		public int SortOrder { get; set; } = 500;
		public string SystemName { get; set; }
		public string Text { get; set; }
		public string Tooltip { get; set; }
		public string Url { get; set; }
	}
}