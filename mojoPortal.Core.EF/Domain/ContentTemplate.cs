using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_ContentTemplate")]
	public partial class ContentTemplate
	{
		[Key]
		public Guid Guid { get; set; }

		public Guid SiteGuid { get; set; }

		[Required]
		[StringLength(255)]
		public string Title { get; set; }

		[StringLength(255)]
		public string ImageFileName { get; set; }

		public string Description { get; set; }

		public string Body { get; set; }

		public string AllowedRoles { get; set; }

		public Guid CreatedByUser { get; set; }

		public Guid LastModUser { get; set; }

		public DateTime CreatedUtc { get; set; }

		public DateTime LastModUtc { get; set; }
	}
}
