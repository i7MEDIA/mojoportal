using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_BannedIPAddresses")]
	public partial class BannedIPAddress : BaseEntity
	{
		// TODO: This column is named "ID", once that happens we will remove this property.
		[Key]
		[Column("RowID")]
		public new int ID { get; set; }

		[Required]
		[StringLength(50)]
		[Column("BannedIP")]
		public string IPAddress { get; set; }

		public DateTime BannedUTC { get; set; } = DateTime.UtcNow;

		[StringLength(255)]
		[Column("BannedReason")]
		public string Reason { get; set; }
	}
}
