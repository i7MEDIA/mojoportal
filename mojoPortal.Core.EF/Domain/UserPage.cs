using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_UserPages")]
	public partial class UserPage
	{
		[Key]
		public Guid UserPageID { get; set; }

		public int SiteID { get; set; }

		public Guid UserGuid { get; set; }

		[Required]
		[StringLength(255)]
		public string PageName { get; set; }

		[Required]
		[StringLength(255)]
		public string PagePath { get; set; }

		public int PageOrder { get; set; }

		public Guid? SiteGuid { get; set; }
	}
}
