using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_CommerceReport")]
	public partial class CommerceReport
	{
		[Key]
		public Guid RowGuid { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid UserGuid { get; set; }

		public Guid FeatureGuid { get; set; }

		public Guid ModuleGuid { get; set; }

		[StringLength(255)]
		public string ModuleTitle { get; set; }

		public Guid OrderGuid { get; set; }

		public Guid ItemGuid { get; set; }

		[StringLength(255)]
		public string ItemName { get; set; }

		public int Quantity { get; set; }

		public decimal Price { get; set; }

		public decimal SubTotal { get; set; }

		public DateTime OrderDateUtc { get; set; }

		[StringLength(50)]
		public string PaymentMethod { get; set; }

		[StringLength(250)]
		public string IPAddress { get; set; }

		[Required]
		[StringLength(255)]
		public string AdminOrderLink { get; set; }

		[Required]
		[StringLength(255)]
		public string UserOrderLink { get; set; }

		public DateTime RowCreatedUtc { get; set; }

		public bool IncludeInAggregate { get; set; }
	}
}
