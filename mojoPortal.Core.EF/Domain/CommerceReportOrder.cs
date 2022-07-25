using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_CommerceReportOrders")]
	public partial class CommerceReportOrder
	{
		[Key]
		public Guid RowGuid { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid FeatureGuid { get; set; }

		public Guid ModuleGuid { get; set; }

		public Guid UserGuid { get; set; }

		public Guid OrderGuid { get; set; }

		[StringLength(100)]
		public string BillingFirstName { get; set; }

		[StringLength(50)]
		public string BillingLastName { get; set; }

		[StringLength(255)]
		public string BillingCompany { get; set; }

		[StringLength(255)]
		public string BillingAddress1 { get; set; }

		[StringLength(255)]
		public string BillingAddress2 { get; set; }

		[StringLength(255)]
		public string BillingSuburb { get; set; }

		[StringLength(255)]
		public string BillingCity { get; set; }

		[StringLength(20)]
		public string BillingPostalCode { get; set; }

		[StringLength(255)]
		public string BillingState { get; set; }

		[StringLength(255)]
		public string BillingCountry { get; set; }

		[StringLength(50)]
		public string PaymentMethod { get; set; }

		public decimal SubTotal { get; set; }

		public decimal TaxTotal { get; set; }

		public decimal ShippingTotal { get; set; }

		public decimal OrderTotal { get; set; }

		public DateTime OrderDateUtc { get; set; }

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
