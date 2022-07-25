using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_ContentMetaLink")]
	public partial class ContentMetaLink
	{
		[Key]
		public Guid Guid { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid ModuleGuid { get; set; }

		public Guid ContentGuid { get; set; }

		[Required]
		[StringLength(255)]
		public string Rel { get; set; }

		[Required]
		[StringLength(255)]
		public string Href { get; set; }

		[StringLength(10)]
		public string HrefLang { get; set; }

		[StringLength(50)]
		public string Rev { get; set; }

		[StringLength(50)]
		public string Type { get; set; }

		[StringLength(50)]
		public string Media { get; set; }

		public int SortRank { get; set; }

		public DateTime CreatedUtc { get; set; }

		public Guid CreatedBy { get; set; }

		public DateTime LastModUtc { get; set; }

		public Guid LastModBy { get; set; }
	}
}
