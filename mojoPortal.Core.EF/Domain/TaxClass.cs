using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_TaxClass")]
	public partial class TaxClass
	{
		[Key]
		public Guid Guid { get; set; }

		public Guid SiteGuid { get; set; }

		[Required]
		[StringLength(255)]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }

		public DateTime? LastModified { get; set; }

		public DateTime Created { get; set; }
	}
}
