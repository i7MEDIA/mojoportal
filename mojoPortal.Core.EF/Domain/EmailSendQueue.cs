using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_EmailSendQueue")]
	public partial class EmailSendQueue
	{
		[Key]
		public Guid Guid { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid ModuleGuid { get; set; }

		public Guid UserGuid { get; set; }

		public Guid SpecialGuid1 { get; set; }

		public Guid SpecialGuid2 { get; set; }

		[Required]
		[StringLength(100)]
		public string FromAddress { get; set; }

		[Required]
		[StringLength(100)]
		public string ReplyTo { get; set; }

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

		public DateTime DateToSend { get; set; }

		public DateTime CreatedUtc { get; set; }
	}
}
