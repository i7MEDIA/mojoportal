using SuperFlexiBusiness;
using System.Collections.Generic;

namespace SuperFlexiUI.Models
{
	public class SuperFlexiObject
	{
		public string FriendlyName { get; set; }
		public string ModuleTitle { get; set; }
		public int GlobalSortOrder { get; set; }
		public List<ItemWithValues> Items { get; set; }
	}
}
