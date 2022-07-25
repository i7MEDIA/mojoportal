using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_GeoZone")]
	public partial class GeoZone
	{
		[Key]
		public Guid Guid { get; set; }

		public Guid CountryGuid { get; set; }

		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		[Required]
		[StringLength(255)]
		public string Code { get; set; }

		public virtual GeoCountry GeoCountry { get; set; }
	}
}
