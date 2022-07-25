using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_UserLogins")]
	public partial class UserLogin
	{
		[Key]
		[Column(Order = 0)]
		public string LoginProvider { get; set; }

		[Key]
		[Column(Order = 1)]
		public string ProviderKey { get; set; }

		[Key]
		[Column(Order = 2)]
		public string UserId { get; set; }
	}
}
