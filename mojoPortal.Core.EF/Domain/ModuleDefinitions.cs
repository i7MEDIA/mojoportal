using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_ModuleDefinitions")]
	public partial class ModuleDefinitions
	{
		public ModuleDefinitions()
		{
			Modules = new HashSet<Module>();
		}

		[Key]
		public int ModuleDefID { get; set; }

		[Required]
		[StringLength(255)]
		public string FeatureName { get; set; }

		[Required]
		[StringLength(255)]
		public string ControlSrc { get; set; }

		public int SortOrder { get; set; }

		public bool IsAdmin { get; set; }

		[StringLength(255)]
		public string Icon { get; set; }

		public int DefaultCacheTime { get; set; }

		public Guid Guid { get; set; }

		[StringLength(255)]
		public string ResourceFile { get; set; }

		public bool? IsCacheable { get; set; }

		public bool? IsSearchable { get; set; }

		[StringLength(255)]
		public string SearchListName { get; set; }

		public bool? SupportsPageReuse { get; set; }

		[StringLength(255)]
		public string DeleteProvider { get; set; }

		[StringLength(255)]
		public string PartialView { get; set; }

		[StringLength(255)]
		public string SkinFileName { get; set; }

		public virtual ICollection<Module> Modules { get; set; }
	}
}
