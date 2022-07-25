using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_Tag")]
	public partial class Tag
	{
		public Tag()
		{
			TagItem = new HashSet<TagItem>();
		}

		[Key]
		public Guid Guid { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid FeatureGuid { get; set; }

		public Guid ModuleGuid { get; set; }

		[Required]
		[StringLength(255)]
		[Column("Tag")]
		public string Name { get; set; }

		public DateTime CreatedUtc { get; set; }

		public Guid CreatedBy { get; set; }

		public DateTime ModifiedUtc { get; set; }

		public Guid ModifiedBy { get; set; }

		public int ItemCount { get; set; }

		public Guid? VocabularyGuid { get; set; }

		public virtual ICollection<TagItem> TagItem { get; set; }
	}
}
