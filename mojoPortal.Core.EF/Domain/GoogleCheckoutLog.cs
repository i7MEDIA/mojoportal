using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_GoogleCheckoutLog")]
	public partial class GoogleCheckoutLog
	{
		[Key]
		public Guid RowGuid { get; set; }

		public DateTime CreatedUtc { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid UserGuid { get; set; }

		public Guid StoreGuid { get; set; }

		public Guid CartGuid { get; set; }

		[StringLength(255)]
		public string NotificationType { get; set; }

		public string RawResponse { get; set; }

		[StringLength(50)]
		public string SerialNumber { get; set; }

		public DateTime? GTimestamp { get; set; }

		[StringLength(50)]
		public string OrderNumber { get; set; }

		[StringLength(50)]
		public string BuyerId { get; set; }

		[StringLength(50)]
		public string FullfillState { get; set; }

		[StringLength(50)]
		public string FinanceState { get; set; }

		public bool EmailListOptIn { get; set; }

		[StringLength(5)]
		public string AvsResponse { get; set; }

		[StringLength(5)]
		public string CvnResponse { get; set; }

		public DateTime? AuthExpDate { get; set; }

		public decimal AuthAmt { get; set; }

		public decimal DiscountTotal { get; set; }

		public decimal ShippingTotal { get; set; }

		public decimal TaxTotal { get; set; }

		public decimal OrderTotal { get; set; }

		public decimal LatestChgAmt { get; set; }

		public decimal TotalChgAmt { get; set; }

		public decimal LatestRefundAmt { get; set; }

		public decimal TotalRefundAmt { get; set; }

		public decimal LatestChargeback { get; set; }

		public decimal TotalChargeback { get; set; }

		public string CartXml { get; set; }

		[StringLength(255)]
		public string ProviderName { get; set; }
	}
}
