using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_Currency")]
	public partial class Currency
	{
		[Key]
		public Guid Guid { get; set; }

		[Required]
		[StringLength(50)]
		public string Title { get; set; }

		[Required]
		[StringLength(3)]
		public string Code { get; set; }

		[StringLength(15)]
		public string SymbolLeft { get; set; }

		[StringLength(15)]
		public string SymbolRight { get; set; }

		[StringLength(1)]
		public string DecimalPointChar { get; set; }

		[StringLength(1)]
		public string ThousandsPointChar { get; set; }

		[StringLength(1)]
		public string DecimalPlaces { get; set; }

		public decimal? Value { get; set; }

		public DateTime? LastModified { get; set; }

		public DateTime Created { get; set; }
	}
}
