using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_Comments")]
	public partial class Comment
	{
		[Key]
		public Guid Guid { get; set; }

		public Guid ParentGuid { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid FeatureGuid { get; set; }

		public Guid ModuleGuid { get; set; }

		public Guid ContentGuid { get; set; }

		public Guid UserGuid { get; set; }

		[StringLength(255)]
		public string Title { get; set; }

		[Required]
		public string UserComment { get; set; }

		[Required]
		[StringLength(50)]
		public string UserName { get; set; }

		[Required]
		[StringLength(100)]
		public string UserEmail { get; set; }

		[StringLength(255)]
		public string UserUrl { get; set; }

		[Required]
		[StringLength(50)]
		public string UserIp { get; set; }

		public DateTime CreatedUtc { get; set; }

		public DateTime LastModUtc { get; set; }

		public byte ModerationStatus { get; set; }

		public Guid ModeratedBy { get; set; }

		[StringLength(255)]
		public string ModerationReason { get; set; }
	}
}
