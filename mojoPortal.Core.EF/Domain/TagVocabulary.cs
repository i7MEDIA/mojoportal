using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_TagVocabulary")]
	public partial class TagVocabulary
	{
		[Key]
		public Guid Guid { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid FeatureGuid { get; set; }

		public Guid ModuleGuid { get; set; }

		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		public string Description { get; set; }

		public DateTime CreatedUtc { get; set; }

		public Guid CreatedBy { get; set; }

		public DateTime ModifiedUtc { get; set; }

		public Guid ModifiedBy { get; set; }
	}
}
