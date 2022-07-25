using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_FriendlyUrls")]
	public partial class FriendlyUrl
	{
		[Key]
		public int UrlID { get; set; }

		public int? SiteID { get; set; }

		[Column("FriendlyUrl")]
		[StringLength(255)]
		public string Url { get; set; }

		[StringLength(255)]
		public string RealUrl { get; set; }

		public bool IsPattern { get; set; }

		public Guid? PageGuid { get; set; }

		public Guid? SiteGuid { get; set; }

		public Guid? ItemGuid { get; set; }
	}
}
