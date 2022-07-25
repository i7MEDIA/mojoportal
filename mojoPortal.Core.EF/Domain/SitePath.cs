using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_SitePaths")]
	public partial class SitePath
	{
		[Key]
		public Guid PathID { get; set; }

		public int SiteID { get; set; }

		[Required]
		[StringLength(255)]
		public string Path { get; set; }

		[Required]
		[StringLength(255)]
		public string LoweredPath { get; set; }

		public Guid? SiteGuid { get; set; }

		public virtual Site Sites { get; set; }
	}
}
