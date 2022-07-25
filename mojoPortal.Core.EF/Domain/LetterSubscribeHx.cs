using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_LetterSubscribeHx")]
	public partial class LetterSubscribeHx
	{
		[Key]
		public Guid RowGuid { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid SubscribeGuid { get; set; }

		public Guid LetterInfoGuid { get; set; }

		public Guid UserGuid { get; set; }

		[Required]
		[StringLength(100)]
		public string Email { get; set; }

		public bool IsVerified { get; set; }

		public bool UseHtml { get; set; }

		public DateTime BeginUtc { get; set; }

		public DateTime EndUtc { get; set; }

		[StringLength(100)]
		public string IpAddress { get; set; }
	}
}
