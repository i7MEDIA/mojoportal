using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_Language")]
	public partial class Language
	{
		[Key]
		public Guid Guid { get; set; }

		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		[Required]
		[StringLength(2)]
		public string Code { get; set; }

		public int Sort { get; set; }
	}
}
