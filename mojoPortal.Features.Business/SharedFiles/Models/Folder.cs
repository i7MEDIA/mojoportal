using System;

namespace mojoPortal.Features.Business.SharedFiles.Models
{
	public class Folder
	{
		public int ID { get; set; }
		public int ModuleID { get; set; }
		public string Name { get; set; }
		public int ParentID { get; set; }
		public Guid ModuleGuid { get; set; }
		public Guid FolderGuid { get; set; }
		public Guid ParentGuid { get; set; }
		public string ViewRoles { get; set; }
	}
}