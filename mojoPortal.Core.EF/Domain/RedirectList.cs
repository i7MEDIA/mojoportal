using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_RedirectList")]
	public partial class RedirectList
	{
		[Key]
		public Guid RowGuid { get; set; }

		public Guid SiteGuid { get; set; }

		public int SiteID { get; set; }

		[Required]
		[StringLength(255)]
		public string OldUrl { get; set; }

		[Required]
		[StringLength(255)]
		public string NewUrl { get; set; }

		public DateTime CreatedUtc { get; set; }

		public DateTime ExpireUtc { get; set; }
	}
}
