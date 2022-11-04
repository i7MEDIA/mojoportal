using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperFlexiUI.Models
{
	public class WidgetModel
	{
		public ModuleConfiguration Config { get; set; }
		public List<object> Items { get; set; }
		public ModuleModel Module { get; set; }
		public PageModel Page { get; set; }
		public SiteModel Site { get; set; }
		//public string SkinPath { get; set; }
		//public int SiteId { get; set; }
		//public string SiteRoot { get; set; }
	}
}