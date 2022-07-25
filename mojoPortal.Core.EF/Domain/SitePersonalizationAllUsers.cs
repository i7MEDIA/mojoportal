using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_SitePersonalizationAllUsers")]
	public partial class SitePersonalizationAllUsers
	{
		[Key]
		public Guid PathID { get; set; }

		public byte[] PageSettings { get; set; }

		public DateTime LastUpdate { get; set; }
	}
}
