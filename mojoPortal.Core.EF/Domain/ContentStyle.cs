using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_ContentStyle")]
	public partial class ContentStyle
	{
		[Key]
		public Guid Guid { get; set; }

		public Guid SiteGuid { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; }

		[Required]
		[StringLength(50)]
		public string Element { get; set; }

		[Required]
		[StringLength(50)]
		public string CssClass { get; set; }

		[Required]
		[StringLength(100)]
		public string SkinName { get; set; }

		public bool IsActive { get; set; }

		public DateTime CreatedUtc { get; set; }

		public DateTime LastModUtc { get; set; }

		public Guid CreatedBy { get; set; }

		public Guid LastModBy { get; set; }
	}
}
