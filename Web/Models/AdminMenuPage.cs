using System.Collections.Generic;
namespace mojoPortal.Web.Models;

public class AdminMenuPage
{
	public AdminMenuPage()
	{
		Links = [];
	}
	public string PageTitle { get; set; }
	public string PageHeading { get; set; }
	public List<ContentAdminLink> Links { get; set; }
	public BreadCrumbs BreadCrumbs { get; set; }
}