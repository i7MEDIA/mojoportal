using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_Modules")]
	public partial class Module
	{
		public Module()
		{
			HtmlContent = new HashSet<HtmlContent>();
			ModuleSettings = new HashSet<ModuleSettings>();
			PageModules = new HashSet<PageModule>();
		}

		[Key]
		public int ModuleID { get; set; }

		public int? SiteID { get; set; }

		public int ModuleDefID { get; set; }

		[StringLength(255)]
		public string ModuleTitle { get; set; }

		public string AuthorizedEditRoles { get; set; }

		public int CacheTime { get; set; }

		public bool? ShowTitle { get; set; }

		public int EditUserID { get; set; }

		public bool AvailableForMyPage { get; set; }

		public bool AllowMultipleInstancesOnMyPage { get; set; }

		[StringLength(255)]
		public string Icon { get; set; }

		public int? CreatedByUserID { get; set; }

		public DateTime? CreatedDate { get; set; }

		public int CountOfUseOnMyPage { get; set; }

		public Guid? Guid { get; set; }

		public Guid? FeatureGuid { get; set; }

		public Guid? SiteGuid { get; set; }

		public Guid? EditUserGuid { get; set; }

		public bool HideFromUnAuth { get; set; }

		public bool HideFromAuth { get; set; }

		public string ViewRoles { get; set; }

		public string DraftEditRoles { get; set; }

		public bool? IncludeInSearch { get; set; }

		public bool? IsGlobal { get; set; }

		[StringLength(25)]
		public string HeadElement { get; set; }

		public int? PublishMode { get; set; }

		public string DraftApprovalRoles { get; set; }

		public virtual ICollection<HtmlContent> HtmlContent { get; set; }

		public virtual ModuleDefinitions ModuleDefinitions { get; set; }

		public virtual ICollection<ModuleSettings> ModuleSettings { get; set; }

		public virtual ICollection<PageModule> PageModules { get; set; }
	}
}
