using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using mojoPortal.Business;

namespace mojoPortal.Web.Models
{
	public class CustomMenu
	{
		public CustomMenu()
		{ }
		public SiteMapDataSource MenuData { get; set; }
		public PageSettings StartingPage { get; set; }
		public PageSettings CurrentPage { get; set; }
		public bool UseTreeView { get; set; }
		public bool ShowStartingNode { get; set; }
		public int MaxDepth { get; set; }
	}
}