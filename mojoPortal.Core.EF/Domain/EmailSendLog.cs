using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_EmailSendLog")]
	public partial class EmailSendLog
	{
		[Key]
		public Guid Guid { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid ModuleGuid { get; set; }

		public Guid SpecialGuid1 { get; set; }

		public Guid SpecialGuid2 { get; set; }

		[Required]
		[StringLength(255)]
		public string ToAddress { get; set; }

		[StringLength(255)]
		public string CcAddress { get; set; }

		[StringLength(255)]
		public string BccAddress { get; set; }

		[Required]
		[StringLength(255)]
		public string Subject { get; set; }

		public string TextBody { get; set; }

		public string HtmlBody { get; set; }

		[Required]
		[StringLength(50)]
		public string Type { get; set; }

		public DateTime SentUtc { get; set; }

		[StringLength(100)]
		public string FromAddress { get; set; }

		[StringLength(100)]
		public string ReplyTo { get; set; }

		public Guid? UserGuid { get; set; }
	}
}
