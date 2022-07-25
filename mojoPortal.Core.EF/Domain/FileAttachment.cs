using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_FileAttachment")]
	public partial class FileAttachment
	{
		[Key]
		public Guid RowGuid { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid ModuleGuid { get; set; }

		public Guid ItemGuid { get; set; }

		public Guid SpecialGuid1 { get; set; }

		public Guid SpecialGuid2 { get; set; }

		[Required]
		[StringLength(255)]
		public string ServerFileName { get; set; }

		[Required]
		[StringLength(255)]
		public string FileName { get; set; }

		public DateTime CreatedUtc { get; set; }

		public Guid CreatedBy { get; set; }

		public long? ContentLength { get; set; }

		[StringLength(50)]
		public string ContentType { get; set; }

		[StringLength(255)]
		public string ContentTitle { get; set; }
	}
}
