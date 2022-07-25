using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_LetterSendLog")]
	public partial class LetterSendLog
	{
		[Key]
		public int RowID { get; set; }

		public Guid LetterGuid { get; set; }

		public Guid UserGuid { get; set; }

		[StringLength(100)]
		public string EmailAddress { get; set; }

		public DateTime UTC { get; set; }

		public bool ErrorOccurred { get; set; }

		public string ErrorMessage { get; set; }

		public Guid? SubscribeGuid { get; set; }
	}
}
