using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperFlexiUI.Models
{
	public class ModuleModel
	{
		public int Id { get; set; }
		public Guid Guid { get; set; }
		public bool IsEditable { get; set; }
		public string Pane { get; set; }
		public int PublishedToPageId { get; set; }
		public bool ShowTitle { get; set; }
		public string Title { get; set; }
		public string TitleElement { get; set; }
	}
}