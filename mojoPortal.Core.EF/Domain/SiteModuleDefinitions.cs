using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_SiteModuleDefinitions")]
	public partial class SiteModuleDefinitions
	{
		[Key]
		[Column(Order = 0)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int SiteID { get; set; }

		[Key]
		[Column(Order = 1)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int ModuleDefID { get; set; }

		public string AuthorizedRoles { get; set; }

		public Guid? SiteGuid { get; set; }

		public Guid? FeatureGuid { get; set; }
	}
}
