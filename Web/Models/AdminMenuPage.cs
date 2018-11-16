using System.Collections.Generic;
using mojoPortal.Web;
namespace mojoPortal.Web.Models
{
	public class AdminMenuPage
	{

		public AdminMenuPage()
		{
			Links = new List<ContentAdminLink>();
		}
		public string PageTitle { get; set; }
		public string PageHeading { get; set; }
		public List<ContentAdminLink> Links { get; set; }

		public BreadCrumbs BreadCrumbs { get; set; }

		//public SkinConfig SkinConfig { get; set; }
	}
}