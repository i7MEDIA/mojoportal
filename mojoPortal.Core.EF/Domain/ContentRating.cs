using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_ContentRating")]
	public partial class ContentRating
	{
		[Key]
		public Guid RowGuid { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid ContentGuid { get; set; }

		public Guid UserGuid { get; set; }

		[StringLength(100)]
		public string EmailAddress { get; set; }

		public int Rating { get; set; }

		public string Comments { get; set; }

		[StringLength(50)]
		public string IpAddress { get; set; }

		public DateTime CreatedUtc { get; set; }

		public DateTime LastModUtc { get; set; }
	}
}
