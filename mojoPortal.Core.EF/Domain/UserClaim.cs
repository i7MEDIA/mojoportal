using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_UserClaims")]
	public partial class UserClaim
	{
		public int Id { get; set; }

		[Required]
		[StringLength(128)]
		public string UserId { get; set; }

		public string ClaimType { get; set; }

		public string ClaimValue { get; set; }
	}
}
