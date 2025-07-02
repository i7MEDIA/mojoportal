namespace mojoPortal.Web.Models;

public class PageNotFoundModel
{
	public PageNotFoundModel() { }
	public bool UseGoogle404Enhancement { get; set; } = WebConfigSettings.EnableGoogle404Enhancement;
	public string CultureCode { get; set; } = "en";
	public string SiteRootUrl { get; set; } = "".ToLinkBuilder().ToString();
	public string SiteMapUrl { get; set; } = "SiteMap.aspx".ToLinkBuilder().ToString();
	public string SearchUrl { get; set; } = "SearchResults.aspx".ToLinkBuilder().ToString();
}