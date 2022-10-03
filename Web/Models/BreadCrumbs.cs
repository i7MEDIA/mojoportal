using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mojoPortal.Web.Models
{
	public class BreadCrumbs
	{
		public BreadCrumbs()
		{
			Crumbs = new List<BreadCrumb>();
		}
		public BreadCrumbArea CrumbArea { get; set; } = BreadCrumbArea.Admin;
		public List<BreadCrumb> Crumbs { get; set; }
	}

	public enum BreadCrumbArea
	{
		Admin,
		Content
	}
}