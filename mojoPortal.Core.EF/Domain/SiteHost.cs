using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_SiteHosts")]
	public partial class SiteHost
	{
		[Key]
		public int HostID { get; set; }

		public int SiteID { get; set; }

		[Required]
		[StringLength(255)]
		public string HostName { get; set; }

		public Guid? SiteGuid { get; set; }
	}
}
