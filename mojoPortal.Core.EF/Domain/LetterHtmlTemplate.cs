using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_LetterHtmlTemplate")]
	public partial class LetterHtmlTemplate
	{
		[Key]
		public Guid Guid { get; set; }

		public Guid SiteGuid { get; set; }

		[Required]
		[StringLength(255)]
		public string Title { get; set; }

		public string Html { get; set; }

		public DateTime LastModUTC { get; set; }
	}
}
