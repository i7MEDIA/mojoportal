using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_HtmlContent")]
	public partial class HtmlContent
	{
		[Key]
		public int ItemID { get; set; }

		public int ModuleID { get; set; }

		[StringLength(255)]
		public string Title { get; set; }

		public string Excerpt { get; set; }

		public string Body { get; set; }

		[StringLength(255)]
		public string MoreLink { get; set; }

		public int SortOrder { get; set; }

		public DateTime BeginDate { get; set; }

		public DateTime EndDate { get; set; }

		public DateTime? CreatedDate { get; set; }

		public int? UserID { get; set; }

		public Guid? ItemGuid { get; set; }

		public Guid? ModuleGuid { get; set; }

		public Guid? UserGuid { get; set; }

		public Guid? LastModUserGuid { get; set; }

		public DateTime? LastModUtc { get; set; }

		public bool ExcludeFromRecentContent { get; set; }

		public virtual Module Modules { get; set; }
	}
}
