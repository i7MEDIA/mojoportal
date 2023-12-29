using System.Collections.Generic;

namespace mojoPortal.Web.Models;
public class BreadCrumbs
{
	public BreadCrumbs()
	{
		Crumbs = [];
	}
	public BreadCrumbArea CrumbArea { get; set; } = BreadCrumbArea.Admin;
	public List<BreadCrumb> Crumbs { get; set; }
}

public enum BreadCrumbArea
{
	Admin,
	Content
}