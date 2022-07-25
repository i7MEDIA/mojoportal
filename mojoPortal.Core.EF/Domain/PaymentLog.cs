using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_PaymentLog")]
	public partial class PaymentLog
	{
		[Key]
		public Guid RowGuid { get; set; }

		public DateTime CreatedUtc { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid UserGuid { get; set; }

		public Guid StoreGuid { get; set; }

		public Guid CartGuid { get; set; }

		[StringLength(100)]
		public string Provider { get; set; }

		public string RawResponse { get; set; }

		[StringLength(1)]
		public string ResponseCode { get; set; }

		[StringLength(20)]
		public string ResponseReasonCode { get; set; }

		public string Reason { get; set; }

		[StringLength(50)]
		public string AvsCode { get; set; }

		[StringLength(1)]
		public string CcvCode { get; set; }

		[StringLength(1)]
		public string CavCode { get; set; }

		[StringLength(50)]
		public string TransactionId { get; set; }

		[StringLength(50)]
		public string TransactionType { get; set; }

		[StringLength(20)]
		public string Method { get; set; }

		[StringLength(50)]
		public string AuthCode { get; set; }

		public decimal? Amount { get; set; }

		public decimal? Tax { get; set; }

		public decimal? Duty { get; set; }

		public decimal? Freight { get; set; }
	}
}
