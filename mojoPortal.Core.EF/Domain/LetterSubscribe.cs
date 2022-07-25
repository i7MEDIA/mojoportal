using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_LetterSubscribe")]
	public partial class LetterSubscribe
	{
		[Key]
		public Guid Guid { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid LetterInfoGuid { get; set; }

		public Guid UserGuid { get; set; }

		[Required]
		[StringLength(100)]
		public string Email { get; set; }

		public bool IsVerified { get; set; }

		public Guid VerifyGuid { get; set; }

		public DateTime BeginUtc { get; set; }

		public bool UseHtml { get; set; }

		[StringLength(100)]
		public string IpAddress { get; set; }
	}
}
