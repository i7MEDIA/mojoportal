using System.Collections.Generic;
using mojoPortal.Business;
using mojoPortal.Web.UI;

namespace mojoPortal.Web.Models
{
	public class MemberListModel
	{
		public MemberListModel()
		{
			Users = new List<SiteUser>();
		}
		public List<SiteUser> Users { get; set; }
		public PagerInfo PagerInfo { get; set; }
		public MemberListDisplaySettings DisplaySettings { get; set; }
	}
}