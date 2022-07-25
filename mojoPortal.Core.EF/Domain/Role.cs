using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_Roles")]
	public partial class Role
	{
		[Key]
		public int RoleID { get; set; }

		public int SiteID { get; set; }

		[Required]
		[StringLength(50)]
		public string RoleName { get; set; }

		[StringLength(50)]
		public string DisplayName { get; set; }

		public Guid? SiteGuid { get; set; }

		public Guid? RoleGuid { get; set; }

		public virtual Site Sites { get; set; }
	}
}
