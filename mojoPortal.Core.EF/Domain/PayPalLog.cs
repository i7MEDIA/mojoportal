using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_PayPalLog")]
	public partial class PayPalLog
	{
		[Key]
		public Guid RowGuid { get; set; }

		public DateTime CreatedUtc { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid UserGuid { get; set; }

		public Guid StoreGuid { get; set; }

		public Guid CartGuid { get; set; }

		[Required]
		[StringLength(255)]
		public string RequestType { get; set; }

		[StringLength(50)]
		public string ApiVersion { get; set; }

		public string RawResponse { get; set; }

		[StringLength(50)]
		public string Token { get; set; }

		[StringLength(50)]
		public string PayerId { get; set; }

		[StringLength(50)]
		public string TransactionId { get; set; }

		[StringLength(10)]
		public string PaymentType { get; set; }

		[StringLength(50)]
		public string PaymentStatus { get; set; }

		[StringLength(255)]
		public string PendingReason { get; set; }

		[StringLength(50)]
		public string ReasonCode { get; set; }

		[StringLength(50)]
		public string CurrencyCode { get; set; }

		public decimal ExchangeRate { get; set; }

		public decimal CartTotal { get; set; }

		public decimal PayPalAmt { get; set; }

		public decimal TaxAmt { get; set; }

		public decimal FeeAmt { get; set; }

		public decimal SettleAmt { get; set; }

		[StringLength(255)]
		public string ProviderName { get; set; }

		[StringLength(255)]
		public string ReturnUrl { get; set; }

		public string SerializedObject { get; set; }

		[StringLength(255)]
		public string PDTProviderName { get; set; }

		[StringLength(255)]
		public string IPNProviderName { get; set; }

		[StringLength(255)]
		public string Response { get; set; }
	}
}
