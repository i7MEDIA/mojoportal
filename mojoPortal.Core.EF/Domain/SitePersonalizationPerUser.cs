using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_SitePersonalizationPerUser")]
	public partial class SitePersonalizationPerUser
	{
		public Guid ID { get; set; }

		public Guid PathID { get; set; }

		public Guid UserID { get; set; }

		public byte[] PageSettings { get; set; }

		public DateTime LastUpdate { get; set; }
	}
}
