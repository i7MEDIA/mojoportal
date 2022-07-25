using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_UserRoles")]
	public partial class UserRole
	{
		public int ID { get; set; }

		public int UserID { get; set; }

		public int RoleID { get; set; }

		public Guid? UserGuid { get; set; }

		public Guid? RoleGuid { get; set; }
	}
}
