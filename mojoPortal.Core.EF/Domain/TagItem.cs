using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_TagItem")]
	public partial class TagItem
	{
		[Key]
		public Guid TagItemGuid { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid FeatureGuid { get; set; }

		public Guid ModuleGuid { get; set; }

		public Guid RelatedItemGuid { get; set; }

		public Guid TagGuid { get; set; }

		public Guid ExtraGuid { get; set; }

		public Guid TaggedBy { get; set; }

		public virtual Tag Tag { get; set; }
	}
}
