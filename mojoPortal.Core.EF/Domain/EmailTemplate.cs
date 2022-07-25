using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_EmailTemplate")]
	public partial class EmailTemplate
	{
		[Key]
		public Guid Guid { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid FeatureGuid { get; set; }

		public Guid ModuleGuid { get; set; }

		public Guid SpecialGuid1 { get; set; }

		public Guid SpecialGuid2 { get; set; }

		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		[Required]
		[StringLength(255)]
		public string Subject { get; set; }

		public string TextBody { get; set; }

		public string HtmlBody { get; set; }

		public bool HasHtml { get; set; }

		public bool IsEditable { get; set; }

		public DateTime CreatedUtc { get; set; }

		public DateTime LastModUtc { get; set; }

		public Guid LastModBy { get; set; }
	}
}
