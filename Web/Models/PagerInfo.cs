namespace mojoPortal.Web.Models;

public class PagerInfo
{
	public PagerInfo() { }


	public PagerInfo(
		int pageNumber,
		int pageSize,
		int pageCount,
		string linkFormat
	)
	{
		CurrentIndex = pageNumber;
		PageSize = pageSize;
		PageCount = pageCount;
		LinkFormat = linkFormat;
	}

	public int CurrentIndex { get; set; } = 0;
	public int PageSize { get; set; } = 20;
	public int PageCount { get; set; } = 0;
	public string LinkFormat { get; set; } = string.Empty;
}