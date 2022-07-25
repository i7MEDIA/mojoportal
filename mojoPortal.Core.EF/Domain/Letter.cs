using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_Letter")]
	public partial class Letter
	{
		[Key]
		public Guid LetterGuid { get; set; }

		public Guid LetterInfoGuid { get; set; }

		[Required]
		[StringLength(255)]
		public string Subject { get; set; }

		public string HtmlBody { get; set; }

		public string TextBody { get; set; }

		public Guid CreatedBy { get; set; }

		public DateTime CreatedUTC { get; set; }

		public Guid LastModBy { get; set; }

		public DateTime LastModUTC { get; set; }

		public bool IsApproved { get; set; }

		public Guid ApprovedBy { get; set; }

		public DateTime? SendClickedUTC { get; set; }

		public DateTime? SendStartedUTC { get; set; }

		public DateTime? SendCompleteUTC { get; set; }

		public int SendCount { get; set; }

		public virtual LetterInfo LetterInfo { get; set; }
	}
}
