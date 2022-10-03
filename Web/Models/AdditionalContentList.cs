using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mojoPortal.Business;

namespace mojoPortal.Web.Models
{
	public class AdditionalContentList
	{
		public List<Module.GlobalContent> GlobalContent { get; set; }
		public List<ChosenContent> ChosenContent { get; set; }
		public Dictionary<string,int> LocationOptions { get; set; }
	}
	public class ChosenContent
	{
		public Module.GlobalContent GlobalContent { get; set; }
		public string Location { get; set; }
		public string SortOrder { get; set; }
	}
}