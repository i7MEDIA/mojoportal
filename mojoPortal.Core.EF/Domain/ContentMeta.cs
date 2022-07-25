using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_ContentMeta")]
	public partial class ContentMeta
	{
		[Key]
		public Guid Guid { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid ModuleGuid { get; set; }

		public Guid ContentGuid { get; set; }

		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		[Required]
		[StringLength(255)]
		public string Scheme { get; set; }

		[StringLength(10)]
		public string LangCode { get; set; }

		[StringLength(3)]
		public string Dir { get; set; }

		public string MetaContent { get; set; }

		public int SortRank { get; set; }

		public DateTime CreatedUtc { get; set; }

		public Guid CreatedBy { get; set; }

		public DateTime LastModUtc { get; set; }

		public Guid LastModBy { get; set; }

		[Required]
		[StringLength(255)]
		public string NameProperty { get; set; }

		[Required]
		[StringLength(255)]
		public string ContentProperty { get; set; }
	}
}
