using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_SavedQuery")]
	public partial class SavedQuery
	{
		public Guid Id { get; set; }

		[Required]
		[StringLength(50)]
		public string Name { get; set; }

		public string Statement { get; set; }

		public DateTime CreatedUtc { get; set; }

		public Guid CreatedBy { get; set; }

		public DateTime LastModUtc { get; set; }

		public Guid LastModBy { get; set; }
	}
}
